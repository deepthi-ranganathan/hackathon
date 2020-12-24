#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlJournal.Methods.cs
// NameSpace: Phoenix.Client.Journal
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//11/20/2010	1		LSimpson	#80615 - Created.
//02/03/2011    2       LSimpson    #12711 - Removed unnecessary code related to print order.
//02/11/2011    3       LSimpson    #12687 - Changes for MtCashInTotal.
//02/13/2011    4       LSimpson    #12610 - Excluded transfers from populating MtCksOnUs.
//02/14/2011    5       LSimpson    CR#12881 - Added Print Balance logic.
//02/15/2011    6       LSimpson    #12886 - MT modifications.
//02/22/2011    7       LSimpson    #12992 - Added unlock for printer when cut has been disabled.
//02/24/2011    8       LSimpson    #13024 - Added retrieval of logical service for thin client.
//03/15/2011    9       LSimpson    #79502 - Enhanced Voucher Printing.
//05/20/2011    10      LSimpson    #13655 - Corrected offline issues.
//06/17/2011    11      LSimpson    #12925 - Return from SuppressBalances if printinfo is null.
//07/11/2011    12      LSimpson    #14628 - Load TranAcct and TfrAcct values within GetPrintBalancePrompt().
//07/12/2011    13      LSimpson    #14708 - Print 'Unavailable' for balances when offline.
//1/20/2012		14		NJoshi		#16578 - Reprinted receipt shows incorrect information.
//03/15/2012    15      LSimpson    #17166 - If print balance prompt not used, post access only for balances was not being considered.
//05/29/2012    16      LSimpson    #17818 - Added confidential balance printing check for AllowEmployeeToViewConfidentialFlag and AllowEmployeeToIgnoreConfidentialFlag.
//12/20/2012    100     mselvaga    #140772 - WI#20589 - Merged up Inventory Tracking - Phase II changes into the latest.
//01/18/2013    101     njoshi      #19703 - When you go into the "Reprint" option for a shared branch transaction the balance is showing.
//8/3/2013		102		apitava		#157637	Uses new xfsprinter
//08/12/2015    103     mselvaga    #38308 - REL 2015 - Reprinting Teller Receipts from the Journal is returning Application error.
//11/06/2015    104     BSchlottman #194535 - eReceipts
//12/16/2015    105     BSchlottman #40196 If the user select Print, clear out the email address.
//02/18/2016    106     BSchlottman #41316 Code Review issues
//                                  #41424 Tell the printer to cut after the footer.
//08/08/2016    107     DEIland     Task#49267 - MtCksOffUs is getting set here and then in GetNextMTPrintObject which is causing reprints to double the Transit Check Amounts
//09/26/2019    108     mselvaga    #119734 - Added ECM print and archive changes.
//10/10/2019    109     mselvaga    Bug#120256 - Shared Branch Transaction A11 Reprint failed due to the application error
//10/16/2019    110     mselvaga    #120397 - Receipts via Documents paper clip - sub sequence number not displaying value
//---------------------------------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Misc;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Hyland.Service;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Forms;
//begin #194535
using System.Drawing;
using phoenix.client.tlprinting;
using Phoenix.Windows.Client;
//end   #194535

namespace Phoenix.Client.Journal
{
    partial class frmTlJournal
    {

        /// <summary>
        /// HandleMtPrinting handles reprinting of MT Receipts
        /// </summary>
        private void HandleMtPrinting()
        {
            //begin #194535
            Boolean isPrintAndEmail = false;    //#41316
            bool skipTran = false;
            bool rePrint = false;
            bool skip = false;
            bool isEmail = false;  //#41316
            PrintInfo printInfo = null;
            byte[] image = null;	// #157637
            printInfo = new PrintInfo();
            printInfo.MtReceiptText = _tlTranSet.MtReceiptText.Value;
            int rimNo = 0;
            string mtCustomerName = "";
            //end   #194535

            if (_reprintFormId.IsNull || _reprintFormId.Value == 0)
                return;
            #region Initialize

            string logicalService = null;
            string mediaName = null;
            string formName = null;
            bool formFound = false;
            int tmpFormId = 0; //#119734
            #endregion

            #region Get ad_tl_form_mt_tran
            AdTlFormMtTran _adTlFormMtTran = new AdTlFormMtTran();
            _adTlFormMtTran.Ptid.Value = 1;
            _adTlFormMtTran.SelectAllFields = true;
            _adTlFormMtTran.ActionType = XmActionType.Select;
            CoreService.DataService.ProcessRequest(_adTlFormMtTran);

            if (_adTlFormMtTran.HeaderFormId.IsNull || _adTlFormMtTran.FooterFormId.IsNull)
                return;
            #endregion

            #region Load print info and print
            if (_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "T") ||
                _reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "S") ||    //#76057
                _reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "P") ||    //#76409
                _reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "R") ||
                _reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "C"))
            {

                #region Initialize
                int formId = _reprintFormId.Value;
                int copiesCount = 0;
                int loopIteration = 0; //#80615
                bool collectChkInfo = false;

				formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", _adTlFormMtTran.HeaderFormId.Value);
				logicalService = (_wosaServiceName != null && _wosaServiceName.Length > 0) ? _wosaServiceName : _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;

                _wosaPrintInfo.PrintSourceScreenId = this.ScreenId;

                _tlTranSet.MtNetAmt.Value = 0;
                _tlTranSet.MtCashInTotal.Value = 0;
                _tlTranSet.MtCashOutTotal.Value = 0;
                _tlTranSet.MtCksOffUs.Value = 0;
                _tlTranSet.MtCksOnUs.Value = 0;
                _tlTranSet.MtAmtToSignFor.Value = 0;
                #endregion

				#region Print MT Transactions
				try
				{
					#region set sequence
					int chosenSequenceNo = Convert.ToInt32(colSequenceNo.Text);
					int chosenSubSequenceNo = Convert.ToInt32(colSubSequence.Text);
					int chosenRow = gridJournal.ContextRow;
					if (chosenSubSequenceNo > 1)
					{
						chosenRow -= (chosenSubSequenceNo - 1);
						chosenSubSequenceNo = 1;
					}
					#endregion

					#region #79502
					HoldTlTranSet.Clear();
					foreach (TlTransaction holdTran in thisTranSet.Transactions)
					{
						HoldTlTranSet.Add(holdTran);
					}
					#endregion

					#region Loop through MT Transactions
					// Loop through MT Transactions....
					do
					{
						GetNextMtRow(chosenRow);
						//
						if (Convert.ToInt32(colSequenceNo.Text) != chosenSequenceNo || chosenRow >= gridJournal.Items.Count)
							break;
						//
						#region load MT variables
						//#12687 - Commented if (_tlTranSet.CurTran.CashIn.Value > 0 )
						//    _tlTranSet.MtCashInTotal.Value += _tlTranSet.CurTran.CashIn.Value;
						//#12886 - Commented if (_tlTranSet.CurTran.CashOut.Value > 0)
                        //    _tlTranSet.MtCashOutTotal.Value += _tlTranSet.CurTran.CashOut.Value;
                        #region // Begin Task#49267 - Comment Out because value is poplutate in GetNextMTPrintObject
                        //if (_tlTranSet.CurTran.TransitChks.Value > 0 && _tlTranSet.AdTlTc.IncludeMtPrint.Value == "Y") // #12886
						//	_tlTranSet.MtCksOffUs.Value += _tlTranSet.CurTran.TransitChks.Value;
						//#12610 - Commented if (_tlTranSet.CurTran.OnUsChks.Value > 0)
                        //_tlTranSet.MtCksOnUs.Value += _tlTranSet.CurTran.OnUsChks.Value;
                        #endregion // End Task#49267 -
                        #endregion

                        #region Print Header
                        if (colSubSequence.Text == "0" || colSubSequence.Text == "1")
						{
							if (colSubSequence.Text == "0")
								_tlTranSet.MtReceiptText.Value = "";
							else
								_tlTranSet.MtReceiptText.Value = "*****Multiple Transaction Receipt*****";

							//Print the Header Form....
							#region get form info
							_wosaPrintInfo.MtReceiptText = _tlTranSet.MtReceiptText.Value;

							formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", _adTlFormMtTran.HeaderFormId.Value);
							if (_wosaServiceName != null && _wosaServiceName.Length > 0)
								logicalService = _wosaServiceName;
							else
								logicalService = _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;
							mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
							formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
                            tmpFormId = _tlTranSet.TellerVars.AdTlForm.FormId.Value;   //#119734
                            #endregion

                        }
						#endregion

						_tlTranSet.IsAcquirerTran = (colSharedBranch.Text == GlobalVars.Instance.ML.Y); // #79510, #10883-2

						_tlTranSet.GetNextMTPrintObject(_tlTranSet.CurTran, ref formId, ref copiesCount,
							ref collectChkInfo, ref _wosaPrintInfo);
						chosenRow += 1;
						//
						#region #79502
						thisTran = (TlTransaction)thisTranSet.Transactions[loopIteration];
						_tlTranSet.CurTran.PrintTfrBalance.Value = thisTran.PrintTfrBalance.Value;
						_tlTranSet.CurTran.PrintBalance.Value = thisTran.PrintBalance.Value;
						#endregion

						if (loopIteration == 0) // Print header at beginning of loop after we've loaded printInfo
						{
							#region- Actual Header Print
							try
							{

								#region set form info
								_wosaPrintInfo.WosaFormName = formName;
								_wosaPrintInfo.WosaMediaName = mediaName;
								_wosaPrintInfo.WosaLogicalService = logicalService;
                                _wosaPrintInfo.FormID = tmpFormId; //#119734
								_tlTranSet.GetGlobalPrintInfo(_wosaPrintInfo);

								#endregion

								#region Print form
								#region #79502
								_tlTranSet.CurTran.PrintTfrBalance.Value = thisTran.PrintTfrBalance.Value;
								_tlTranSet.CurTran.PrintBalance.Value = thisTran.PrintBalance.Value;
								acctHasPostAccessOnly = thisTran.TranAcct.bAcctHasPostAccessOnly.Value;

								_printTfrBalances = true;
								_printPrimaryBalances = true;

								#region #17166
								if ((_tlTranSet.AdTlTc.IncludePrtPrompt.Value != "Y") || (colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
								{
									if (thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true)
									{
										_tlTranSet.CurTran.PrintBalance.Value = "N";
										_printPrimaryBalances = false;
									}
									if ((!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != "") && thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true) || (colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
									{
										_tlTranSet.CurTran.PrintTfrBalance.Value = "N";
										_printTfrBalances = false;
									}
									_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
								}
								#endregion

								if (_printBalOption == "A")
								{
									if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true
										&& !((_tlTranSet.GlobalHelper.IsConfAcct(thisTran.TranAcct.AcctType.Value, thisTran.TranAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
										&& !(thisTran.TranAcct.TranCode.Value >= 300 && thisTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
										&& !(thisTran.TranAcct.DepLoan.Value == "GL")
										&& !(thisTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(thisTran.TranAcct.TranCode.Value))
										&& !(colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
									{
										_tlTranSet.CurTran.PrintBalance.Value = "Y";
									}
									else
									{
										_tlTranSet.CurTran.PrintBalance.Value = "N";
									}
									if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
									{
										if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
											&& !((_tlTranSet.GlobalHelper.IsConfAcct(thisTran.TfrAcct.AcctType.Value, thisTran.TfrAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
											&& !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
											&& !(thisTran.TfrAcct.DepLoan.Value == "GL")
											&& !(thisTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))
											&& !(colSharedBranch.Text == GlobalVars.Instance.ML.Y)) // #13713/*19703*/
										{
											_tlTranSet.CurTran.PrintTfrBalance.Value = "Y";
										}
										else
										{
											_tlTranSet.CurTran.PrintTfrBalance.Value = "N";
										}
									}
								}
								if ((!_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y") || (colSharedBranch.Text == GlobalVars.Instance.ML.Y)) // #79502/*19703*/
								{
									_printPrimaryBalances = false;
									_printTfrBalances = false;
									_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
								}
								if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
								{
									if (_tlTranSet.CurTran.PrintBalance.Value == "N")
										_printPrimaryBalances = false;
									if ((_tlTranSet.CurTran.TfrTranCode.IsNull && (_tlTranSet.CurTran.TfrAcctNo.IsNull || _tlTranSet.CurTran.TfrAcctNo.Value.Trim() == "")) || _tlTranSet.CurTran.PrintTfrBalance.Value == "N")
										_printTfrBalances = false;
									if (!_printPrimaryBalances || !_printTfrBalances)
										_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
								}
								#endregion
                                //begin #194535
                                if (_tellerVars.AdTlControl.EnableEreceipt.Value == "Y")
                                {
                                    // Show dlgReceiptInfo
                                    _tlTranSet.CurTran.TranAcct.FromMTForms.Value = "Y";
                                    CallOtherForms("eReceipt");

                                    skipTran = dialogResult == DialogResult.Cancel;
                                    rePrint = dialogResult == DialogResult.Retry;
                                    skip = dialogResult == DialogResult.Ignore;

                                    rimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
                                    mtCustomerName = _tlTranSet.CurTran.TranAcct.RimFirstName.Value + " " +
                                        _tlTranSet.CurTran.TranAcct.RimMiddleInitial.Value + " " + _tlTranSet.CurTran.TranAcct.RimLastName.Value;

                                    if (rePrint || skipTran || skip)
                                    {
                                        return;
                                    }

                                    if (dialogResult == DialogResult.OK)
                                    {
                                        if (myInstruction.ReceiptDelMethod == PRINTANDEMAIL)
                                        {
                                            isPrintAndEmail = true; //#41316 Print the form and tell Hyland to archive this form. When it archives it it will email it.
                                        }
                                        else if (myInstruction.ReceiptDelMethod == EMAIL)
                                        {
                                            isEmail = true;    //#41316 Tell Hyland to archive this form. When it archives it it will email it.
                                        }
                                        //begin #40196
                                        else if (myInstruction.ReceiptDelMethod == PRINT)
                                        {
                                            emailAddress = "";  //Do not email this form.
                                        }
                                        //end   #40196
                                    }
                                }
                                //end   #194535
								#region #12881
								//#79502 Commented - if (!_printBalances)
								//#79502 Commented -     _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
								#endregion
								if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y")
								{
                                    //begin #194535
                                    bool continuePrint = true;

                                    if (isPrintAndEmail || isEmail)    //#194535    #41316
                                    {
                                        _xfsPrinter.UseImaging = true;
                                        continuePrint = _xfsPrinter.PrintMtFormAndReturnImage(mediaName, formName, _wosaPrintInfo, false, out image, isEmail); // Print and/or create an image to email.
                                    }
                                    else
                                    {
									    continuePrint = _xfsPrinter.PrintMtForm(mediaName, formName, _wosaPrintInfo);	//#157637
                                    }
                                    //end  #194535
									if (!continuePrint)
										return;
								}
								#endregion
							}
							finally
							{
							}
							#endregion
						}
						loopIteration += 1;
						//
						#region append additional print info
						//
						#region calculator tape
						_calculatorTape = string.Empty;
						if (colTranCode.Text == "CLC")
						{
							_calculatorTape = colCalcData1.Text + colCalcData2.Text;
							if (_calculatorTape == string.Empty || _calculatorTape == "")
							{
								_busObjTlJrnlCalc.JournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
								CallXMThruCDS("Calculator");
								_calculatorTape = _busObjTlJrnlCalc.CalcData.Value;
							}
							_calculatorTape = _calculatorTape.Replace("\n", "\r\n");
							_wosaPrintInfo.CalculatorTape = _calculatorTape;
						}
						#endregion
						//
						#region BAT Items
						if (colTranCode.Text == CoreService.Translation.GetUserMessageX(360092))
						{
							_busObjTlJournal.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
							_busObjTlJournal.DrawerNo.Value = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
							_busObjTlJournal.EffectiveDt.Value = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
							_busObjTlJournal.BatchId.Value = Convert.ToInt16(colBatchId.UnFormattedValue);
							_busObjTlJournal.Ptid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
							_tempPrintInfo = _busObjTlJournal.GetBatchPrintInfo(_partialPrintString);
							if (_tempPrintInfo.BatchItems != string.Empty)
								_wosaPrintInfo.BatchItems = _tempPrintInfo.BatchItems;
							if (_tempPrintInfo.ItemCount > 0)
								_wosaPrintInfo.ItemCount = _tempPrintInfo.ItemCount;
						}
						#endregion
						//
						#region get check info
						GetCheckInfo();
						#endregion
						//
						#region Ref Acct Details
						//begin 16278
						/*  _wosaPrintInfo.TlTranCodeDesc = colDescription.Text; */
						#region Get  Trancode desc from adtltc
						AdTlTc adTlTc = new AdTlTc();
						adTlTc.TlTranCode.Value = Convert.ToString(_tlTranSet.CurTran.TlTranCode.Value);
						adTlTc.SelectAllFields = true;
						adTlTc.ActionType = XmActionType.Select;
						CoreService.DataService.ProcessRequest(adTlTc);
						_wosaPrintInfo.TlTranCodeDesc = adTlTc.Description.Value;
						#endregion
						//end 16278

						_wosaPrintInfo.Description = colTranDescription.Text;

						//Reference Account
						if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 500 ||
							Convert.ToInt32(colRealTranCode.UnFormattedValue) == 550 ||
							Convert.ToInt32(colRealTranCode.UnFormattedValue) == 928)	//#71668 - added TC928
						{
							_wosaPrintInfo.RefAcctNo = colReferenceAcct.Text;
						}
						// Begin #72248 //
						tempRefAcctNo = colReferenceAcct.Text;
						indexOfDash = tempRefAcctNo.IndexOf('-');
						if (indexOfDash != -1)
						{
							_wosaPrintInfo.RefAcctNoOnly = tempRefAcctNo.Substring(indexOfDash + 1);
							_wosaPrintInfo.RefAcctType = tempRefAcctNo.Substring(0, indexOfDash);
						}
						// End #72248 //

						#endregion

						#endregion

                            #region handle printing
                            try
                            {

                                #region Get Float Lines - #74269
                                string[] asRegCCHolds = null;
                                bool skipChecks = false;
                                bool skipPrintWithOfacMatch = false;    // #76057

                                if (_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "S") && !_reprintFormId.IsNull && _tlTranSet.IsCheckPrintAvailable(true, _reprintFormId.Value, false))    //#1841 #1871
                                {
                                    #region Check/Money Order Additional Processing- #76057
                                    string copyStatus = Phoenix.FrameWork.CDS.DataService.Instance.XmDbStatus.ToString().Substring(0, 1).ToUpper();
                                    bool isPrimaryDb = (copyStatus == "D" || copyStatus == "C");
                                    #region OfacSearch
                                    bool isFound = false;
                                    if (!_tlTranSet.SearchGbOfacSdnList(_wosaPrintInfo.CheckLine1, Convert.ToInt32(colRimNo.UnFormattedValue)))
                                    {
                                        if (_tlTranSet.SearchGbOfacSdnList(_wosaPrintInfo.CheckLine2, Convert.ToInt32(colRimNo.UnFormattedValue)))
                                            isFound = true;
                                    }
                                    else
                                        isFound = true;
                                    if (isFound)
                                    {
                                        // 11/07/2008 - It was decided not to prompt user to print check - just print always
                                        //if (PMessageBox.Show(this, 10788, MessageType.Message, MessageBoxButtons.YesNo) == DialogResult.No)
                                        //{
                                        //    // do not print check
                                        //    skipPrintWithOfacMatch = true;
                                        //}
                                        PMessageBox.Show(this, 10788, MessageType.Message, MessageBoxButtons.OK);
                                    }
                                #endregion

                                if (!skipPrintWithOfacMatch)
                                {
                                    _tlTranSet.Transactions.Add(_tlTranSet.CurTran); //Follow posting trancode logic

                                    // Regen a new check. Assign new Check No etc, number (Server side)
                                    int checkNo = Convert.ToInt32(colCheckNo.Text);
                                    _tlTranSet.SaveTlChkFormInfo(_wosaPrintInfo, true, -1, ref checkNo);

                                    _tlTranSet.CurTran.CheckNo.Value = checkNo;

                                    // Print Check
                                    _wosaPrintInfo.CheckNo = _tlTranSet.CurTran.CheckNo.Value; //9591
                                    _wosaPrintInfo.JournalPtid = _tlTranSet.CurTran.Ptid.Value; //9591
                                    _wosaPrintInfo.SbSeqNo = _tlTranSet.CurTran.SubSequence.StringValue;    //#120397
                                    GenerateChkPrintingReport(_wosaPrintInfo); //9591

                                    gridJournal.PopulateTable();
                                }
                                    #endregion
                                    return;
                                }


                                if (_wosaPrintInfo.RegCCHolds != null)
                                {
                                    asRegCCHolds = _wosaPrintInfo.RegCCHolds.Split('^');
                                }
                                #endregion
                                #region 79502
                                _tlTranSet.CurTran.PrintTfrBalance.Value = thisTran.PrintTfrBalance.Value;
                                _tlTranSet.CurTran.PrintBalance.Value = thisTran.PrintBalance.Value;
                                acctHasPostAccessOnly = thisTran.TranAcct.bAcctHasPostAccessOnly.Value;

                                _printTfrBalances = true;
                                _printPrimaryBalances = true;

                                #region #17166
                                if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value != "Y")
                                {
                                    if (thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true)
                                    {
                                        _tlTranSet.CurTran.PrintBalance.Value = "N";
                                        _printPrimaryBalances = false;
                                    }
                                    if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != "") && thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true)
                                    {
                                        _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                        _printTfrBalances = false;
                                    }
                                    _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                }
                                #endregion

                                if (_printBalOption == "A")
                                {
                                    if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true
                                        && !(thisTran.TranAcct.TranCode.Value >= 300 && thisTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                        && !(thisTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(thisTran.TranAcct.TranCode.Value)))
                                    {
                                        _tlTranSet.CurTran.PrintBalance.Value = "Y";
                                    }
                                    else
                                    {
                                        _tlTranSet.CurTran.PrintBalance.Value = "N";
                                    }
                                    if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
                                    {
                                        if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
                                            && !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                            && !(thisTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))) // #13713
                                        {
                                            _tlTranSet.CurTran.PrintTfrBalance.Value = "Y";
                                        }
                                        else
                                        {
                                            _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                        }
                                    }
                                }
                                if (!_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
                                {
                                    _printPrimaryBalances = false;
                                    _printTfrBalances = false;
                                    _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                }
                                if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
                                {
                                    if (_tlTranSet.CurTran.PrintBalance.Value == "N")
                                        _printPrimaryBalances = false;
                                    if ((_tlTranSet.CurTran.TfrTranCode.IsNull && (_tlTranSet.CurTran.TfrAcctNo.IsNull || _tlTranSet.CurTran.TfrAcctNo.Value.Trim() == "")) || _tlTranSet.CurTran.PrintTfrBalance.Value == "N")
                                        _printTfrBalances = false;
                                    if (!_printPrimaryBalances || !_printTfrBalances)
                                        _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                }
                                #endregion
                                #region #12881
                                //#79502 Commented - if (!_printBalances)
                                //#79502 Commented -     _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                #endregion

                                #region Print Single/Multiple Forms - #74269
                                if (asRegCCHolds != null)
                                {
                                    if (asRegCCHolds.Length <= 3)
                                    {
                                        _wosaPrintInfo.RegCCHolds = _wosaPrintInfo.RegCCHolds.Replace("^", " | ");
                                        if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y && _tlTranSet.AdTlTc.IncludeMtPrint.Value == "Y") // #12886
                                        {
                                            if (isPrintAndEmail || isEmail)    //#41316
                                            {
                                                _xfsPrinter.PrintFormAndReturnImage(_mediaName, _formName, _wosaPrintInfo, out image, isEmail);
                                            }
                                            else
                                            {
                                                _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string holdInfo = "";
                                        for (int b = 0; b < asRegCCHolds.Length; b++)
                                        {
                                            if (b == asRegCCHolds.Length - 1)
                                                holdInfo += asRegCCHolds[b];
                                            else
                                                holdInfo += asRegCCHolds[b] + " | ";
                                            if (b == 2)
                                            {
                                                if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y && _tlTranSet.AdTlTc.IncludeMtPrint.Value == "Y") // #12886
                                                {
                                                    _wosaPrintInfo.RegCCHolds = holdInfo;
                                                    if (isPrintAndEmail || isEmail)    //#41316
                                                    {
                                                        _xfsPrinter.PrintFormAndReturnImage(_mediaName, _formName, _wosaPrintInfo, out image, isEmail);
                                                    }
                                                    else
                                                    {
                                                        _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
                                                    }
                                                    holdInfo = "";
                                                }
                                            }
                                            else if ((b + 1) % 3 == 0 || b == asRegCCHolds.Length - 1)
                                            {
                                                _wosaPrintInfo.RegCCHolds = holdInfo;
                                                if (_tellerVars.AdTlControl.EnableEreceipt.Value == "Y") //#41316
                                                {
                                                    CallOtherForms("eReceipts");
                                                }
                                                else
                                                {
                                                    CallOtherForms("PrintForm");
                                                }
                                                holdInfo = "";
                                                skipTran = dialogResult == DialogResult.Cancel;
                                                rePrint = dialogResult == DialogResult.Retry;
                                                skipChecks = dialogResult == DialogResult.Abort;
                                                if (rePrint || skipTran || skipChecks) // Break out of print loop
                                                    break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
                                    {
                                        if (_tlTranSet.AdTlTc.IncludeMtPrint.Value == "Y")
                                        {
                                            AdTlTcForm _adTlTcForm = new AdTlTcForm();
                                            _adTlTcForm.TlTranCode.Value = colTranCode.Text;
                                            _adTlTcForm.OutputTypeId.Value = 2;
                                            MTForm = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(_adTlTcForm);
                                            if (MTForm != null && MTForm.Count > 0)
                                            {
                                                try
                                                {
                                                    foreach (Phoenix.BusObj.Admin.Teller.AdTlTcForm adtltcform in MTForm)
                                                    {
                                                        formId = adtltcform.FormId.Value;
                                                    }
                                                }
                                                finally
                                                {
                                                }
                                            }

                                            #region get form info
                                            if (formId.ToString().Trim() != "")
                                            {
                                                formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", formId);
                                                if (_wosaServiceName != null && _wosaServiceName.Length > 0)
                                                    logicalService = _wosaServiceName;
                                                else
                                                    logicalService = _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;
                                                mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
                                                formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
                                                tmpFormId = _tlTranSet.TellerVars.AdTlForm.FormId.Value;   //#119734
                                        }
                                            #endregion
                                            try
                                            {
                                                _wosaPrintInfo.WosaFormName = formName;
                                                _wosaPrintInfo.WosaMediaName = mediaName;
                                                _wosaPrintInfo.WosaLogicalService = logicalService;
                                            _wosaPrintInfo.FormID = tmpFormId; //#119734
                                                #region 79502
                                                _tlTranSet.CurTran.PrintTfrBalance.Value = thisTran.PrintTfrBalance.Value;
                                                _tlTranSet.CurTran.PrintBalance.Value = thisTran.PrintBalance.Value;

                                                _printTfrBalances = true;
                                                _printPrimaryBalances = true;

                                                #region #17166
                                                if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value != "Y")
                                                {
                                                    if (thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true)
                                                    {
                                                        _tlTranSet.CurTran.PrintBalance.Value = "N";
                                                        _printPrimaryBalances = false;
                                                    }
                                                    if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != "") && thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true)
                                                    {
                                                        _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                                        _printTfrBalances = false;
                                                    }
                                                    _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                                }
                                                #endregion

                                                if (_printBalOption == "A")
                                                {
                                                    if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true
                                                        && !(thisTran.TranAcct.TranCode.Value >= 300 && thisTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                                        && !(thisTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(thisTran.TranAcct.TranCode.Value)))
                                                    {
                                                        _tlTranSet.CurTran.PrintBalance.Value = "Y";
                                                    }
                                                    else
                                                    {
                                                        _tlTranSet.CurTran.PrintBalance.Value = "N";
                                                    }
                                                    if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
                                                    {
                                                        if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
                                                            && !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                                            && !(thisTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))) // #13713
                                                        {
                                                            _tlTranSet.CurTran.PrintTfrBalance.Value = "Y";
                                                        }
                                                        else
                                                        {
                                                            _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                                        }
                                                    }
                                                }
                                                if (!_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
                                                {
                                                    _printPrimaryBalances = false;
                                                    _printTfrBalances = false;
                                                    _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                                }
                                                if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
                                                {
                                                    if (_tlTranSet.CurTran.PrintBalance.Value == "N")
                                                        _printPrimaryBalances = false;
                                                    if ((_tlTranSet.CurTran.TfrTranCode.IsNull && (_tlTranSet.CurTran.TfrAcctNo.IsNull || _tlTranSet.CurTran.TfrAcctNo.Value.Trim() == "")) || _tlTranSet.CurTran.PrintTfrBalance.Value == "N")
                                                        _printTfrBalances = false;
                                                    if (!_printPrimaryBalances || !_printTfrBalances)
                                                        _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                                }
                                                #endregion
                                                #region #12881
                                                //#79502 Commented - if (!_printBalances)
                                                //#79502 Commented -     _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                                #endregion

                                                if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y")
                                                {
                                                    if (isPrintAndEmail || isEmail)    //#194535
                                                    {
                                                        _xfsPrinter.PrintMtFormAndReturnImage(mediaName, formName, _wosaPrintInfo, false, out image, isEmail);  // Print and/or create an image to email.
                                                    }
                                                    else
                                                    {
                                                        _xfsPrinter.PrintMtForm(mediaName, formName, _wosaPrintInfo); //#157637
                                                    }
                                                }
                                            }
                                            finally
                                            {
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            finally
                            {
                            }
                            #endregion
					}
					while (Convert.ToInt32(colSequenceNo.Text) == chosenSequenceNo && chosenRow <= gridJournal.Items.Count);

					#endregion

				}
				finally
				{

					_tlTranSet.IsAcquirerTran = false;  // #79510, #10883-2
				}
				#endregion

                #region Print Footer
                // Now print the Footer Form....
                #region get form info
                logicalService = null;
                mediaName = null;
                formName = null;
                tmpFormId = 0; //#119734

                formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", _adTlFormMtTran.FooterFormId.Value);
                if (_wosaServiceName != null && _wosaServiceName.Length > 0)
                    logicalService = _wosaServiceName;
                else
                    logicalService = _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;
                mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
                formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
                tmpFormId = _tlTranSet.TellerVars.AdTlForm.FormId.Value;   //#119734

                #endregion
                try
                {
                    _wosaPrintInfo.ImageFilePath = null;
                    if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y")
                    {
                        #region 79502
                        _tlTranSet.CurTran.PrintTfrBalance.Value = thisTran.PrintTfrBalance.Value;
                        _tlTranSet.CurTran.PrintBalance.Value = thisTran.PrintBalance.Value;

                        _printTfrBalances = true;
                        _printPrimaryBalances = true;

                        #region #17166
                        if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value != "Y")
                        {
                            if (thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true)
                            {
                                _tlTranSet.CurTran.PrintBalance.Value = "N";
                                _printPrimaryBalances = false;
                            }
                            if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != "") && thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true)
                            {
                                _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                _printTfrBalances = false;
                            }
                            _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                        }
                        #endregion

                        if (_printBalOption == "A")
                        {
                            if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true
                                && !(thisTran.TranAcct.TranCode.Value >= 300 && thisTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                && !(thisTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(thisTran.TranAcct.TranCode.Value)))
                            {
                                _tlTranSet.CurTran.PrintBalance.Value = "Y";
                            }
                            else
                            {
                                _tlTranSet.CurTran.PrintBalance.Value = "N";
                            }
                            if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
                            {
                                if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
                                    && !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
                                    && !(thisTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))) // #13713
                                {
                                    _tlTranSet.CurTran.PrintTfrBalance.Value = "Y";
                                }
                                else
                                {
                                    _tlTranSet.CurTran.PrintTfrBalance.Value = "N";
                                }
                            }
                        }
                        if (!_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
                        {
                            _printPrimaryBalances = false;
                            _printTfrBalances = false;
                            _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                        }
                        if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
                        {
                            if (_tlTranSet.CurTran.PrintBalance.Value == "N")
                                _printPrimaryBalances = false;
                            if ((_tlTranSet.CurTran.TfrTranCode.IsNull && (_tlTranSet.CurTran.TfrAcctNo.IsNull || _tlTranSet.CurTran.TfrAcctNo.Value.Trim() == "")) || _tlTranSet.CurTran.PrintTfrBalance.Value == "N")
                                _printTfrBalances = false;
                            if (!_printPrimaryBalances || !_printTfrBalances)
                                _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                        }
                        #endregion
                        #region #12881
                        //#79502 Commented - if (!_printBalances)
                        //#79502 Commented -     _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                        #endregion
                        if (isPrintAndEmail || isEmail)    //#194535
                        {
                            _xfsPrinter.PrintMtFormAndReturnImage(mediaName, formName, _wosaPrintInfo, true, out image, isEmail);  //#41424    // Print and/or create an image to email.

                            _printCustName = mtCustomerName;
                            _printRimNo = rimNo;

                            if (combinedBitmap == null)	// WI#26012
                            {
                                combinedBitmap = null;	// WI#26012
                                sigPos = null;			// WI#26012
                                _xfsPrinter.GetCombinedBitmapAndSigBoxDetails(out combinedBitmap, out sigPos);  //Turn the image into a bitmap
                            }

                            _xfsPrinter.ClearBitmaps();
                            //#119734
                            //SignAndArchiveVoucher(combinedBitmap, sigPos, printInfo, false, true, emailAddress);	// WI#157637 (4)    //#194535   //Tell Hyland to email the form.
                            SignAndArchiveVoucher(combinedBitmap, sigPos, _wosaPrintInfo, false, true, emailAddress);	// WI#157637 (4)    //#194535   //Tell Hyland to email the form.
                        }
                        else
                        {
                            _xfsPrinter.PrintMtForm(mediaName, formName, _wosaPrintInfo, true);	 //#157637
                        }
                    }
                }
                finally
                {
                }

                #endregion
            }
            #endregion
        }

        public bool SignAndArchiveVoucher(Bitmap combinedBitmap, SigBoxDetails sigPos, PrintInfo printInfo, bool requireSignature, bool mtTransaction, string emailAddress)    //#194535
        {
            bool completed = false;
            string combinedImageFile = string.Empty;
            List<string> emailList = new List<string>();              //#194535

            try
            {
                //WI#12980
                bool offline = !_tlTranSet.TellerVars.IsAppOnline;
                if (offline)
                    return true;
                //end WI#12980

                _archivePtid = _tlTranSet.CurTran.Ptid.Value;
                combinedImageFile = Path.Combine(_tlTranSet.GetVoucherImageFilePath(), _tlTranSet.GetVoucherImageFileName(_archivePtid));
                combinedBitmap.Save(combinedImageFile);

                bool sigFieldPresent = (sigPos != null);

                bool isdDriveThru = (TellerVars.Instance.IsDriveThruWorkstation == 1);
                bool promptForSignature = (sigFieldPresent && requireSignature && (!isdDriveThru));

                if (promptForSignature && _idsSignedArchieved.IndexOf(_tlTranSet.CurTran.Ptid.StringValue) >= 0)
                    return true;

                //Begin #5117
                int tellerNo = (_adGbRsm != null) ? ((!_adGbRsm.TellerNo.IsNull) ? Convert.ToInt32(_adGbRsm.TellerNo.Value) : 0) : 0;
                //End #5117 

                // begin #157637 (2)
                int rimNo;
                string custName;

                if (_tlTranSet.IsAcquirerTran)
                {
                    custName = printInfo.SbMemberName;
                    rimNo = 0;
                }
                else if (_printRimNo != 0)
                {
                    rimNo = _printRimNo;
                    custName = _printCustName;
                    printInfo.CustomerName = _printCustName;
                }
                else
                {
                    rimNo = (_tlTranSet.CurTran.AcctType.Value == GlobalVars.Instance.ML.RIM) ?
                        Convert.ToInt32(_tlTranSet.CurTran.AcctNo.Value) : _tlTranSet.CurTran.RimNo.Value;
                    custName = printInfo.CustomerName;
                }
                // end #157637 (2)

                DocumentService doc;
                ParameterService param;
                string colStorMsg = string.Empty;
                int instNo = _tlTranSet.TellerVars.HylandInstitutionNo;

                // begin #157637 (4)
                double amtToSignFor = 0;
                double amtNet = 0;

                if (mtTransaction)
                {
                    #region AmtToSignFor
                    decimal tmpAmtToSignFor = 0;
                    foreach (TlTransaction tlTran in _tlTranSet.Transactions)
                    {
                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", tlTran.TlTranCode.Value);
                        tmpAmtToSignFor += _tlTranSet.GetAmtToSignFor(tlTran, _tlTranSet.TellerVars.AdTlTc);
                    }

                    if (!double.TryParse(Convert.ToString(tmpAmtToSignFor), out amtToSignFor))
                        amtToSignFor = 0;
                    if (!_tlTranSet.CurTran.TlTranCode.IsNull)	// set value back
                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", _tlTranSet.CurTran.TlTranCode.Value);
                    #endregion

                    #region Amount

                    if (!double.TryParse(Convert.ToString(_tlTranSet.MtNetAmt.Value), out amtNet))
                        amtNet = 0;
                    #endregion
                }
                else
                {
                    amtToSignFor = (double)(printInfo.AmtToSignFor != DbDecimal.Null ? printInfo.AmtToSignFor : _tlTranSet.CurTran.NetAmt.Value);
                    amtNet = (double)_tlTranSet.CurTran.NetAmt.Value;
                }
                // end #157637 (4)

                // #140784-2 - Added parameter for IsSharedBranch (_tlTranSet.IsAcquirerTran)
                emailList.Add(emailAddress);  //#194535
                //emailList.Add("Brian.Schlottman@dh.com");
                param = new ParameterService(ovrdSig, rimNo,
                    amtToSignFor,	// #157637 (4) (double)(printInfo.AmtToSignFor != DbDecimal.Null ? printInfo.AmtToSignFor : _tlTranSet.CurTran.NetAmt.Value),   // #04778 - check for amt to sign for as null
                    amtNet,			// #157637 (4) (double)_tlTranSet.MtNetAmt.Value, 
                    custName,      // #79510 - Replaced wosaPrintInfo.CustomerName by custName
                    _tlTranSet.CurTran.Ptid.StringValue,
                    combinedImageFile, promptForSignature,
                    instNo,
                    sigPos.x1, sigPos.y1, sigPos.x2, sigPos.y2, sigPos.CombinedFormWidth, sigPos.CombinedformHeight,
                    tellerNo, Convert.ToInt32( Phoenix.FrameWork.Shared.Variables.GlobalVars.CurrentBranchNo), Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeName, emailList, _tlTranSet.IsAcquirerTran);		//#5117 //#194535

                doc = new DocumentService();
                doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable; //#119734
                if (doc.IsECMEnabled) //#119734
                {
                    List<TranArchiveDetail> tranDetails = new List<TranArchiveDetail>();

                    tranDetails.Add(new TranArchiveDetail
                    {
                        JournalPtid = (printInfo.CheckNo == Decimal.MinValue ? 0 : printInfo.JournalPtid),
                        SeqNo = (printInfo.SequenceNo == Int32.MinValue ? Convert.ToInt16(0) : Convert.ToInt16(printInfo.SequenceNo)),
                        SubSequence = (string.IsNullOrEmpty(printInfo.SbSeqNo) ? Convert.ToInt16(0) : Convert.ToInt16(printInfo.SbSeqNo)),
                        AcctType = printInfo.AcctType,
                        AcctNo = printInfo.AcctNo,
                        BranchName = Phoenix.FrameWork.Shared.Variables.GlobalVars.BranchName,
                        CheckNo = (printInfo.CheckNo == Int32.MinValue ? 0 : printInfo.CheckNo),
                        CreateDt = printInfo.CreateDt,
                        FormId = (printInfo.FormID == Int32.MinValue ? Convert.ToInt16(0) : Convert.ToInt16(printInfo.FormID)),
                        TlTranCode = printInfo.TlTranCode,
                        FormName = printInfo.WosaFormName,
                        TranAmt = (printInfo.TranAmt == Decimal.MinValue ? 0 : printInfo.TranAmt)
                    });

                    param.TranDetails.AddRange(tranDetails);
                }
                completed = (doc.SignAndArchiveVourcher(param, ref colStorMsg));

                if (completed && promptForSignature)
                    _idsSignedArchieved = (_idsSignedArchieved == "") ? _idsSignedArchieved + _tlTranSet.CurTran.Ptid.StringValue : _idsSignedArchieved = _idsSignedArchieved + "~" + _tlTranSet.CurTran.Ptid.StringValue;
                _tlTranSet.CurTran.ColdStorComplete.Value = (completed) ? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N;
                _tlTranSet.CurTran.ColdStorMessage.Value = colStorMsg;
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }
            finally
            {
                File.Delete(combinedImageFile);
            }

            return completed;
        }

        #region #79502 - Get MT Print Balance Prompt
        /// <summary>
        /// Loops through MT transactions to check for any having IncludePrtPrompt set to "Y"
        /// </summary>
        private void GetPrintBalancePrompt(bool IsMt)
        {
            #region set sequence
            int chosenSequenceNo = Convert.ToInt32(colSequenceNo.Text);
            int chosenSubSequenceNo = Convert.ToInt32(colSubSequence.Text);
            int chosenRow = gridJournal.ContextRow;
            bool isTranCodeNumeric = true; // #15067
            int nParseOut;  // #15067

            if (chosenSubSequenceNo > 1 && IsMt)
            {
                chosenRow -= (chosenSubSequenceNo - 1);
                chosenSubSequenceNo = 1;
            }
            #endregion
            thisTranSet = new TlTransactionSet();
            if (IsMt)
            {
                do
                {
                    //
                    if (chosenRow >= gridJournal.Items.Count || Convert.ToInt32(gridJournal.Items[chosenRow].SubItems[colSequenceNo.ColumnId].Text) != chosenSequenceNo)
                        break;
                    //
                    _formFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text);
                    if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
                        _promptBalances = true;
                    chosenRow += 1;
                    thisTran = new TlTransaction();
                    thisTran.TlTranCode.Value = gridJournal.Items[chosenRow - 1].SubItems[colTranCode.ColumnId].Text;
                    isTranCodeNumeric = Int32.TryParse(gridJournal.Items[chosenRow - 1].SubItems[colTranCode.ColumnId].Text, out nParseOut); // #15067
                    if (isTranCodeNumeric) // #15067
                        thisTran.TranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colTranCode.ColumnId].Text);
                    thisTran.TranCodeDesc.Value = gridJournal.Items[chosenRow - 1].SubItems[colTranDescription.ColumnId].Text;
                    thisTran.AcctType.Value = gridJournal.Items[chosenRow - 1].SubItems[colAcctType.ColumnId].Text;
                    thisTran.AcctNo.Value = gridJournal.Items[chosenRow - 1].SubItems[colAcctNo.ColumnId].Text;
                    thisTran.TfrAcctType.Value = gridJournal.Items[chosenRow - 1].SubItems[colTfrAcctType.ColumnId].Text;
                    thisTran.TfrAcctNo.Value = gridJournal.Items[chosenRow - 1].SubItems[colTfrAcctNo.ColumnId].Text;
                    #region #14628
                    //Begin 175838
                    //#38308 - I am uncommenting the Performance optimization fix added below to reduce the number of CDS call because we need to make these calls for reprint to work
                    _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                    if (colAcctType.UnFormattedValue != null && colAcctType.UnFormattedValue.ToString() != null)    //#120256
                        thisTran.TranAcct.DepLoan.Value = _acctTypeDetails.DepLoan;     
                    //End 175838.
                    if (colTfrAcctType.UnFormattedValue != null && colTfrAcctType.UnFormattedValue.ToString() != null)
                    {
                        _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                        thisTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                    }
                    //if (_acctTypeDetails != null)
                    //{
                    //    if (colAcctType.UnFormattedValue.ToString() != null)
                    //        thisTran.TranAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                    //    //_acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                    //    //End 175838.
                    //    if (colAcctType.UnFormattedValue.ToString() != null)
                    //        thisTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                    //}
                    //End #38308
                    thisTran.TranAcct.AcctType.Value = gridJournal.Items[chosenRow - 1].SubItems[colAcctType.ColumnId].Text;
                    thisTran.TranAcct.AcctNo.Value = gridJournal.Items[chosenRow - 1].SubItems[colAcctNo.ColumnId].Text;
                    thisTran.TfrAcct.AcctType.Value = gridJournal.Items[chosenRow - 1].SubItems[colTfrAcctType.ColumnId].Text;
                    thisTran.TfrAcct.AcctNo.Value = gridJournal.Items[chosenRow - 1].SubItems[colTfrAcctNo.ColumnId].Text;
                    if (isTranCodeNumeric) // #15067
                        thisTran.TranAcct.TranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colTranCode.ColumnId].Text);
                    #endregion
                    // known issue - journal does not have TfrTranCode - thisTran.TfrTranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colTfrTranCode.ColumnId].Text);
                    if (gridJournal.Items[chosenRow - 1].SubItems[colRimNo.ColumnId].Text != "")
                        thisTran.RimNo.Value = Convert.ToInt32(gridJournal.Items[chosenRow - 1].SubItems[colRimNo.ColumnId].Text);
                    thisTran.SequenceNo.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colSequenceNo.ColumnId].Text);
                    thisTran.SubSequence.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colSubSequence.ColumnId].Text);
                    thisTran.NonCust.Value = gridJournal.Items[chosenRow - 1].SubItems[colNonCust.ColumnId].Text;

                    // set access for each transaction
                    tempAcctType = thisTran.AcctType.StringValue;
                    tempAcctNo = thisTran.AcctNo.StringValue;
                    tempRimNo = thisTran.RimNo.Value;
                    tempNonCust = thisTran.NonCust.StringValue;	//#9311, CR#8075

                    bool isRimAccessAllowed = true;
                    CallXMThruCDS("GridClick");
                    //
                    if ((_tellerVars.DebugSecurity || _adRmRestrict.RestrictLevel.Value < _tellerVars.EmplRestrictLevel) && tempNonCust != "Y" && TellerVars.Instance.IsAppOnline) // #13655
                    {
                        if (SetContext())
                        {
                            if (AdGbRsmRim.Edit.Value != GlobalVars.Instance.ML.Y)
                                isRimAccessAllowed = false;
                        }
                    }
                    thisTran.TranAcct.bAcctHasPostAccessOnly.Value = !isRimAccessAllowed;
                    //
                    thisTranSet.Transactions.Add(thisTran);
                }
                while (Convert.ToInt32(gridJournal.Items[chosenRow - 1].SubItems[colSequenceNo.ColumnId].Text) == chosenSequenceNo && chosenRow <= gridJournal.Items.Count);
            }
            else
            {
                _formFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text);
                if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
                    _promptBalances = true;
                thisTran = new TlTransaction();
                thisTran.TlTranCode.Value = gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text;
                isTranCodeNumeric = Int32.TryParse(gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text, out nParseOut); // #15067
                if (isTranCodeNumeric) // #15067
                    thisTran.TranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text);
                thisTran.TranCodeDesc.Value = gridJournal.Items[chosenRow].SubItems[colTranDescription.ColumnId].Text;
                thisTran.AcctType.Value = gridJournal.Items[chosenRow].SubItems[colAcctType.ColumnId].Text;
                thisTran.AcctNo.Value = gridJournal.Items[chosenRow].SubItems[colAcctNo.ColumnId].Text;
                thisTran.TfrAcctType.Value = gridJournal.Items[chosenRow].SubItems[colTfrAcctType.ColumnId].Text;
                thisTran.TfrAcctNo.Value = gridJournal.Items[chosenRow].SubItems[colTfrAcctNo.ColumnId].Text;
                #region #14628

                // Begin  175838
                //#38308 - I am uncommenting the Performance optimization fix added below to reduce the number of CDS call because we need to make these calls for reprint to work
                _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                if (_acctTypeDetails != null)
                    thisTran.TranAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                //_acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                //if (_acctTypeDetails != null)
                //    thisTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;

                //if (colAcctType.UnFormattedValue.ToString() != string.Empty)
                //{
                //    thisTran.TranAcct.DepLoan.Value = colDepType.UnFormattedValue.ToString();
                //}
                //_acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;

                if (colTfrAcctType.UnFormattedValue != null && colTfrAcctType.UnFormattedValue.ToString() != string.Empty)  //#38308
                {
                    _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                    thisTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                }

                // End 175838.
                thisTran.TranAcct.AcctType.Value = gridJournal.Items[chosenRow].SubItems[colAcctType.ColumnId].Text;
                thisTran.TranAcct.AcctNo.Value = gridJournal.Items[chosenRow].SubItems[colAcctNo.ColumnId].Text;
                thisTran.TfrAcct.AcctType.Value = gridJournal.Items[chosenRow].SubItems[colTfrAcctType.ColumnId].Text;
                thisTran.TfrAcct.AcctNo.Value = gridJournal.Items[chosenRow].SubItems[colTfrAcctNo.ColumnId].Text;
                if (isTranCodeNumeric) // #15067
                    thisTran.TranAcct.TranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow].SubItems[colTranCode.ColumnId].Text);
                #endregion
                // known issue - journal does not have TfrTranCode - thisTran.TfrTranCode.Value = Convert.ToInt16(gridJournal.Items[chosenRow - 1].SubItems[colTfrTranCode.ColumnId].Text);
                if (gridJournal.Items[chosenRow].SubItems[colRimNo.ColumnId].Text != "")
                    thisTran.RimNo.Value = Convert.ToInt32(gridJournal.Items[chosenRow].SubItems[colRimNo.ColumnId].Text);
                thisTran.SequenceNo.Value = Convert.ToInt16(gridJournal.Items[chosenRow].SubItems[colSequenceNo.ColumnId].Text);
                thisTran.SubSequence.Value = Convert.ToInt16(gridJournal.Items[chosenRow].SubItems[colSubSequence.ColumnId].Text);

                // set access for each transaction
                tempAcctType = thisTran.AcctType.StringValue;
                tempAcctNo = thisTran.AcctNo.StringValue;
                tempRimNo = thisTran.RimNo.Value;
                tempNonCust = thisTran.NonCust.StringValue;	//#9311, CR#8075

                bool isRimAccessAllowed = true;
                CallXMThruCDS("GridClick");
                //
                if ((_tellerVars.DebugSecurity || _adRmRestrict.RestrictLevel.Value < _tellerVars.EmplRestrictLevel) && tempNonCust != "Y" && TellerVars.Instance.IsAppOnline) // #13655
                {
                    if (SetContext())
                    {
                        if (AdGbRsmRim.Edit.Value != GlobalVars.Instance.ML.Y)
                            isRimAccessAllowed = false;
                    }
                }
                thisTran.TranAcct.bAcctHasPostAccessOnly.Value = !isRimAccessAllowed;
                //
                thisTranSet.Transactions.Add(thisTran);
            }
        }
        #endregion

        #region #12881 - Suppress Balances
        /// <summary>
        /// SuppressBalances - Suppresses balance printing
        /// </summary>
        /// <param name="printInfo"></param>
        private PrintInfo SuppressBalances(PrintInfo printInfo)
        {
            if (printInfo != null) // #12925
            {
                if (!TellerVars.Instance.IsAppOnline) // #14708
                {
                    printInfo.PrintBalances = false;
                    printInfo.PrintTfrBalances = false;
                    printInfo.IsOffline = true;
                }
                else
                {
                    if (!_printPrimaryBalances)
                        printInfo.PrintBalances = false;
                    if (!_printTfrBalances)
                        printInfo.PrintTfrBalances = false;

                    if (acctHasPostAccessOnly)
                        printInfo.PadLength = 4;
                    else
                        printInfo.PadLength = 3;
                }
            }
            return printInfo;
        }
        #endregion

        /// <summary>
        /// GetNextMtRow gets next row from grid associated to MT Transaction
        /// </summary>
        /// <param name="rowNumber">the grid row number</param>
        private void GetNextMtRow(int rowNumber)
        {
            gridJournal.ContextRow = rowNumber;
            LoadAndRePrint("MT");
            #region #79502
            thisTranSet = new TlTransactionSet();
            for (int h = 0; h < HoldTlTranSet.Count; h++)
            {
                thisTranSet.Transactions.Add((TlTransaction)HoldTlTranSet[h]);
            }
            #endregion
        }

        private PLabelStandard lblNoInvItems;
        private PAction pbInventory;
        private PGridColumn colTypeId;
        private PGridColumn colPacketId;
        private PGridColumn colClass;
        private PGridColumn colInvItemAmt;
        private PGridColumn colStateTaxAmt;
        private PGridColumn colLocalTaxAmt;
        private PGridColumn colTlCaptureTranNo;
        private PGridColumn colTlCaptureISN;
        private PGridColumn colTlCaptureBatchPtid;
        private PGridColumn colTlCaptureBatchId;
        private PGridColumn colTlCaptureImageCommited;
        private PGridColumn colTlCaptureWorkstation;
        private PGridColumn colTlCaptureOption;
        private PGridColumn colTlCaptureOptionString;
        private PAction pbImage;
        private PAction pbBondDetails;
        private PComboBoxStandard cmbTlCaptureWorkstation;
        private PdfStandard dfTlCaptureISN;
        private PGridColumn colDepType;
        private PGridColumn colApplType;
        private PAction pbViewReceipt;
    }
}
