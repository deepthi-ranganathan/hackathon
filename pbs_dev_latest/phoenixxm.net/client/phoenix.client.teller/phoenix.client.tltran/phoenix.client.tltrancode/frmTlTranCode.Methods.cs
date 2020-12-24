#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlTrancode.Methods.cs
// NameSpace: Phoenix.Windows.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//11/20/2010	1		LSimpson	#80615 - Created.
//01/17/2011    2       LSimpson    #12420 - Tweaked sig coordinate logic.
//02/11/2011    3       LSimpson    #12687 - Changes for MtCashInTotal.
//02/13/2011    4       LSimpson    #12610 - Changes for MtCksOnUs.
//02/13/2011    5       LSimpson    CR#12878 - Added MT Receipt print dialog.
//02/15/2011    6       LSimpson    #12886 - MT modifications.
//02/17/2011    7       LSimpson    #12903 - Changes for number of copies.
//02/18/2011    8       LSimpson    #12619 - Corrected base issues with waiving signature.
//02/22/2011	9	    SDighe	    #12980 - MT voucher- When in Offline mode should bypass the MT Voucher printing
//02/22/2011    10      LSimpson    #12992 - Added unlock for printer when cut has been disabled.
//02/22/2011    11      LSimpson    #12989 - Do not display prompt to print MT if no MT forms attached.
//02/23/2011    12      LSimpson    #12983 - Removed check printing from MT printing and removed commented code deleted during code cleanup.
//02/23/2011    13      LSimpson    #12994 - Capture first member for MT Voucher printing.
//02/24/2011    14      LSimpson    #13024 - Added retrieval of logical service for thin client.
//03/02/2011    15      LSimpson    #13065 - Modifications for sig pad and drive through.
//03/15/2011    16      LSimpson    #79502 - Enhanced Voucher Printing.
//03/29/2011    17      LSimpson    #12887 - Changes for setting sigReqired.
//04/14/2011	18		NJoshi		#13435 - Adding to wi 12878 if the dialog is answered with a NO still go on with archiving.
//04/25/2011    19      LSimpson    #13811 - Disabled PrintBalPrompt for offline.
//04/28/2011    20      LSimpson    #13914 - Added check for drive thru for code added with #12886.
//05/13/2011    21      LSimpson    #13832 - Modifications for retrieval of adtlformmttran b/o.
//05/23/2011    22      LSimpson    #14258 - Added check for confidential account when printing all balances.
//06/17/2011    23      LSimpson    #12925 - Return from SuppressBalances if printinfo is null.
//07/12/2011    24      LSimpson    #14708 - Print 'Unavailable' for balances when offline.
//12/2/2011		19		NJoshi		#16278 - removing the above 12618 fix  redoing it another way.
//05/18/2012    20      LSimpson    #17818 - Added confidential balance printing check for AllowEmployeeToViewConfidentialFlag and AllowEmployeeToIgnoreConfidentialFlag.
//08/16/2012	21		NJoshi		#17571 - Relationship management summary switching to primary member
//10/10/2012    22      LSimpson    #140784-2 - Added parameter for IsSharedBranch (_tlTranSet.IsAcquirerTran) to ParameterService.
//8/3/2013		23		apitava		#157637 Optimized xfs-printing/imaging/archiving related routines for performance; Imaging functions moved to xfs solution
//12/04/2013	24		JRhyne		WI#26012 - fix mt printing lock issues
//01/09/2014	25		DGarcia		#140792 - Post Pending Charges
//4/8/2014		26		jrhyne		#157637 (2) uses new xfsprinter
//5/6/2014		27		jrhyne		#157637 (3) and #157637 (4) amttosign and teller drivethru
//12/02/2015    28      BSchlottman #194535 - eReceipts
//03/07/2016    29      BSchlottman #42151 Do not delete files in SignAndArchiveVoucher when offline.
//06/17/2016    30      mselvaga    #47041 - TL – Appl error after Time out of device.
//08/24/2016    31      mselvaga    #49555 - Pad is still being invoked when waive signature is ON
//08/24/2016    32      mselvaga    #49556 - Pad is being invoked when Drive thrus is set
//08/21/2019    33      mselvaga    Task#118299 - ECM Voucher printing changes added
//09/17/2019    34      FOyebola    Task#119058 - 232338 - 10 - DEV - Able to Archive a receipt through teller transaction
//09/26/2019    35      mselvaga    #119734 - Added ECM print and archive changes.
//10/11/2019    36      FOyebola    Bug#120142
//03/05/2020    37      mselvaga    #124918 - customer search lookup failed in offline mode.
//03/12/2020    38      mselvaga    Bug#125233 - HF - Phoenix question does not appear for some teller classes.
//10/05/2020    39      mselvaga    Task#132711 - HF - Addresses occasionally print on vouchers.
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
using Phoenix.BusObj.Misc;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Constants;
using Phoenix.Hyland.Service;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Variables;
using System.Drawing;
using Phoenix.Shared.PED; //#45773

namespace Phoenix.Windows.Client
{
    partial class frmTlTranCode
    {
        Boolean PrintAndArchive = false; /*13435*/
		Bitmap combinedBitmap = null;	// WI#26012
		SigBoxDetails sigPos = null;	// WI#26012
        System.Drawing.Image pedSignature;  //#45773
        Boolean retryPEDSignature = false;  //#45773

		//#157637 Optimized to take advantage of new xfsprinter imaging functionality

        /// <summary>
        /// HandleMTPrinting prints MT and Non-MT forms based on print order
        /// configured in Admin (ad_tl_form_mt_tran)
        /// </summary>
		private void HandleMTPrinting()
		{
			#region Initialize
			ArrayList MtTran = new ArrayList(); // #13832
			int copiesCount = 0;
			bool formFound = false;
			bool mtFound = false; // #12878
            //begin #194535
            bool skipTran = false;
            bool rePrint = false;
            bool skip = false;
            //end   #194535
			bool offline = !_tlTranSet.TellerVars.IsAppOnline; // #13811
			bool bArchiveOnly = false;	// #157637 (2)
			bool bIsHylandServiceAvailable = _tlTranSet.IsHylandVoucherSvcAvailable() || _tlTranSet.TellerVars.IsECMVoucherAvailable; // #157637 (2) #118299
			AdTlFormMtTran adTlFormMtTran = new AdTlFormMtTran();

			if (!printPending)
				return;

			#region #13832
			adTlFormMtTran.OutputTypeId.Value = 0;
			MtTran = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(adTlFormMtTran);
			if (MtTran != null && MtTran.Count > 0)
			{
				foreach (Phoenix.BusObj.Admin.Teller.AdTlFormMtTran _adTlFormMtTran in MtTran)
				{
					adTlFormMtTran = _adTlFormMtTran;
				}
			}
			#endregion

			if (adTlFormMtTran.HeaderFormId.IsNull || adTlFormMtTran.FooterFormId.IsNull)
				return;
			copiesCount = adTlFormMtTran.NoCopies.Value;

			// Get Mt Tran Count, if not greater than one, clear MtReceiptText
			_mtTranCount = 0;
			foreach (TlTransaction tlTranTest in _tlTranSet.Transactions)
			{
				#region #12878 - Get count of all MT trancodes having MT forms attached....
				AdTlTcForm tmpAdTlTcForm = null;
				mtFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", tlTranTest.TlTranCode.Value);

				if (mtFound && _tlTranSet.TellerVars.AdTlTc.IncludeMtPrint.Value == "Y")
				{
					formFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcFormArray", tlTranTest.TlTranCode.Value);

					for (int thisForm = 0; thisForm < _tlTranSet.TellerVars.AdTlTcFormArray.Count; thisForm++)
					{
						tmpAdTlTcForm = _tlTranSet.TellerVars.AdTlTcFormArray[thisForm] as AdTlTcForm;
						if (tmpAdTlTcForm.TlTranCode.Value == tlTranTest.TlTranCode.Value)
						{
							int thisFormId = tmpAdTlTcForm.FormId.Value;
							formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", thisFormId);
							if (formFound && _tlTranSet.TellerVars.AdTlForm.TextQrp.Value == "R")
							{
								if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
									_promptBalances = true;

								_mtTranCount += 1;
							}
						}
					}
				}
				#endregion
			}
			if (_mtTranCount <= 1)
				_tlTranSet.MtReceiptText.Value = "";

			#endregion

			//#157637 Rewritten to use new xfsprinter

			try
			{
				#region Print Forms Based On Print Order
				if (adTlFormMtTran.PrintOrder.Value == "F")
				{
					if (_mtTranCount >= 1) // #12989
					{
						/*begin 13435*/
						// begin #157637(2)
						// if they have archiving setup, display Print Archive Cancel
                        if (_tlTranSet.TellerVars.AdTlControl.EnableEreceipt.Value == "N" || !_tlTranSet.TellerVars.IsAppOnline)  //#194535 //#42151
                        {
                            //Do you want to print Multiple Transaction receipts?
                            if (bIsHylandServiceAvailable)
                                dialogResult = Phoenix.Client.MsgBox.frmMsgBoxCustom.ShowDialog("Phoenix Question", "Do you want to print Multiple Transaction receipts?", null, "&Print and Archive", "&Archive Only", "&Skip Receipts", SystemIcons.Question.ToBitmap());
                            else
                                dialogResult = PMessageBox.Show(13437, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1, "");

                            // if _tlTranSet.IsHylandVoucherSvcAvailable(), NO = Archive Only, else, NO = NO
                            if (dialogResult == System.Windows.Forms.DialogResult.Yes || (bIsHylandServiceAvailable && dialogResult == System.Windows.Forms.DialogResult.No))	// #157637(2)
                                PrintAndArchive = true;
                            else
                                PrintAndArchive = false;

                            if (bIsHylandServiceAvailable)	// #157637(2)
                                bArchiveOnly = (dialogResult == System.Windows.Forms.DialogResult.No);
                        }
                        else
                        {
                            _tlTranSet.CurTran.TranAcct.FromMTForms.Value = "Y";
                            formId = adTlFormMtTran.HeaderFormId.Value;
                            CallOtherForms("eReceipt");
                            bArchiveOnly = (dialogResult == System.Windows.Forms.DialogResult.No);
                        }

                        skipTran = dialogResult == DialogResult.Cancel;
                        rePrint = dialogResult == DialogResult.Retry;
                        skip = dialogResult == DialogResult.Ignore;

                        if (rePrint || skipTran || skip)
                        {
                        }
                        else
                        {
                            if (dialogResult == DialogResult.OK && _tlTranSet.TellerVars.AdTlControl.EnableEreceipt.Value == "Y" && _tlTranSet.TellerVars.IsAppOnline)  //#42151
                            {
                                if (myInstruction.ReceiptDelMethod == PRINTANDEMAIL)
                                {
                                    PrintAndArchive = true;
                                }
                                else if (myInstruction.ReceiptDelMethod == EMAIL)
                                {
                                    bArchiveOnly = true;
                                }
                                else if (myInstruction.ReceiptDelMethod == PRINT)
                                {
                                    PrintAndArchive = true;
                                    emailAddress = "";
                                }
                            }

                            //end   #194535
						    // end #157637(2)

						    // #13811 - added offline check
						    if (!offline && printPending && _promptBalances && !_balancesAlreadyPrompted && _tlTranSet.TellerVars.AdTlControl.EnablePrtPrompt.Value == "Y")  //#125233 - removed PrintAndArchive
                                CallOtherForms("PrintBalPrompt");

                            if (PrintAndArchive || bArchiveOnly)    //#194535
                            {
                                // begin #157637 (3)
                                bool forceTwoCopies = (TellerVars.Instance.IsDriveThruWorkstation == 1);	// rename and refactor

                                if (forceTwoCopies)
                                {
                                    bool tranSetHasSignature = false;
                                    foreach (TlTransaction tlTran in _tlTranSet.Transactions)
                                    {
                                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", tlTran.TlTranCode.Value);
                                        if (_tlTranSet.TellerVars.AdTlTc.RequireSignature.Value == "Y")
                                            tranSetHasSignature = true;
                                    }

                                    forceTwoCopies = tranSetHasSignature;

                                    if (!_tlTranSet.CurTran.TlTranCode.IsNull)	// set value back
                                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", _tlTranSet.CurTran.TlTranCode.Value);
                                }
                                // end #157637 (3)

                                int CopyNum = (forceTwoCopies && adTlFormMtTran.NoCopies.IntValue < 2) ? 2 : adTlFormMtTran.NoCopies.IntValue;	//#157637 (3)
                                //first call to PrintMTFormXXX variation will lock printer
                                formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", adTlFormMtTran.HeaderFormId.Value);
                                string logicalService = (TellerVars.Instance.IsRemotePrinting) ? TellerVars.Instance.PrinterSvcReceipt : _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;
                                //begin #194535
                                if (myLogicalService != "")
                                {
                                    logicalService = myLogicalService;
                                }
                                //end   #194535

                                _printer = new XfsPrinter(logicalService);
                                _printer.UseImaging = TellerVars.Instance.IsHylandVoucherAvailable || TellerVars.Instance.IsECMVoucherAvailable;    //#118299
                                int mtCnt = 0;
                                try
                                {
                                    // lock the printer, so nobody else's receipts come between our copies
                                    // PrintMTForms will unlock on last copy
                                    _printer.DoLock();  // WI#26012
                                    for (mtCnt = 1; mtCnt <= CopyNum; mtCnt++)
                                    {
                                        _tlTranSet.MtNetAmt.Value = 0;          // #157637 (4)
                                        _tlTranSet.MtCashInTotal.Value = 0;     // #157637 (4)
                                        _tlTranSet.MtCashOutTotal.Value = 0;    // #157637 (4)
                                        _tlTranSet.MtCksOffUs.Value = 0;        // #157637 (4)
                                        _tlTranSet.MtCksOnUs.Value = 0;         // #157637 (4)
                                        _tlTranSet.MtAmtToSignFor.Value = 0;    // #157637 (4)
                                        PrintMTForms(logicalService, mtCnt, CopyNum, bArchiveOnly); // #12903
                                    }
                                }
                                finally
                                {
                                    // if PrintMTForms throws an exception, then forloop would exit without incrementing mtCnt
                                    // in this case, we want to ensure the printer is unlocked/closed so it does not continue locking
                                    if (mtCnt != CopyNum)   // WI#26012
                                        _printer.Unlock();

                                    // close printer after all copies have been printed and signature captured (or exception thrown) 
                                    _printer.Close();
                                }

                            }
						}
						_tlTranSet.PrinterPtid.SetValueToNull(); // #12983 Prevent Cashier's Check Info from popping up twice

						/*end 13435*/
					}
					_idsSignedArchieved = "";
					HandlePrinting();
				}
				else
				{
					HandlePrinting();
					_idsSignedArchieved = "";
					if (_mtTranCount >= 1) // #12989
					{
						/*begin 13435*/
						
                        //begin #194535
                        if (_tlTranSet.TellerVars.AdTlControl.EnableEreceipt.Value == "N" || !_tlTranSet.TellerVars.IsAppOnline) //#42151
                        {
                            // begin #157637(2)
                            //Do you want to print Multiple Transaction receipts?
                            DialogResult dialogResult;
                            if (bIsHylandServiceAvailable)
                                dialogResult = Phoenix.Client.MsgBox.frmMsgBoxCustom.ShowDialog("Phoenix Question", "Do you want to print Multiple Transaction receipts?", null, "&Print and Archive", "&Archive Only", "&Skip Receipts", SystemIcons.Question.ToBitmap());
                            else
                                dialogResult = PMessageBox.Show(13437, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1, "");
                            // if _tlTranSet.IsHylandVoucherSvcAvailable(), NO = Archive Only, else, NO = NO
                            if (dialogResult == System.Windows.Forms.DialogResult.Yes || (bIsHylandServiceAvailable && dialogResult == System.Windows.Forms.DialogResult.No))	// #157637(2)
                                PrintAndArchive = true;
                            else
                                PrintAndArchive = false;
                            if (bIsHylandServiceAvailable)	// #157637(2)
                                bArchiveOnly = (dialogResult == System.Windows.Forms.DialogResult.No);
                            // end #157637(2)
                        }
                        else
                        {
                            _tlTranSet.CurTran.TranAcct.FromMTForms.Value = "Y";
                            formId = adTlFormMtTran.HeaderFormId.Value;
                            CallOtherForms("eReceipt");
                            bArchiveOnly = (dialogResult == System.Windows.Forms.DialogResult.No);
                        }

                        skipTran = dialogResult == DialogResult.Cancel;
                        rePrint = dialogResult == DialogResult.Retry;
                        skip = dialogResult == DialogResult.Ignore;

                        if (rePrint || skipTran || skip)
                        {
                        }
                        else
                        {
                            

                            //if (bIsHylandServiceAvailable)	// #157637(2)
                            bArchiveOnly = (dialogResult == System.Windows.Forms.DialogResult.No);
                            if (dialogResult == DialogResult.OK && _tlTranSet.TellerVars.AdTlControl.EnableEreceipt.Value == "Y" && _tlTranSet.TellerVars.IsAppOnline)
                            {
                                if (myInstruction.ReceiptDelMethod == PRINTANDEMAIL)
                                {
                                    PrintAndArchive = true;
                                }
                                else if (myInstruction.ReceiptDelMethod == EMAIL)
                                {
                                    bArchiveOnly = true;
                                }
                                else if (myInstruction.ReceiptDelMethod == PRINT)
                                {
                                    PrintAndArchive = true;
                                    emailAddress = "";
                                }
                            }
                            //end   #194535
                            // end #157637(2)

                            // #13811 - Added offline check
                            if (!offline && printPending && _promptBalances && !_balancesAlreadyPrompted && _tlTranSet.TellerVars.AdTlControl.EnablePrtPrompt.Value == "Y")  //#125233 - removed PrintAndArchive
                                CallOtherForms("PrintBalPrompt");

                            if (PrintAndArchive)
                            {
                                // begin #157637 (3)
                                bool forceTwoCopies = (TellerVars.Instance.IsDriveThruWorkstation == 1);	// rename and refactor

                                if (forceTwoCopies)
                                {
                                    bool tranSetHasSignature = false;
                                    foreach (TlTransaction tlTran in _tlTranSet.Transactions)
                                    {
                                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", tlTran.TlTranCode.Value);
                                        if (_tlTranSet.TellerVars.AdTlTc.RequireSignature.Value == "Y")
                                            tranSetHasSignature = true;
                                    }

                                    forceTwoCopies = tranSetHasSignature;

                                    if (!_tlTranSet.CurTran.TlTranCode.IsNull)	// set value back
                                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", _tlTranSet.CurTran.TlTranCode.Value);
                                }
                                // end #157637 (3)

                                int CopyNum = (forceTwoCopies && adTlFormMtTran.NoCopies.IntValue < 2) ? 2 : adTlFormMtTran.NoCopies.IntValue;	// #157637 (3)
                                //first call to PrintMTFormXXX variation will lock printer
                                formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", adTlFormMtTran.HeaderFormId.Value);
                                string logicalService = (TellerVars.Instance.IsRemotePrinting) ? TellerVars.Instance.PrinterSvcReceipt : _tlTranSet.TellerVars.AdTlForm.LogicalService.Value;
                                //begin #194535
                                if (myLogicalService != "")
                                {
                                    logicalService = myLogicalService;
                                }
                                //end   #194535

                                _printer = new XfsPrinter(logicalService);
                                _printer.UseImaging = TellerVars.Instance.IsHylandVoucherAvailable || TellerVars.Instance.IsECMVoucherAvailable;    //#118299
                                int mtCnt = 0;
                                try
                                {
                                    // lock the printer, so nobody else's receipts come between our copies
                                    // PrintMTForms will unlock on last copy
                                    _printer.DoLock();  // WI#26012
                                    for (mtCnt = 1; mtCnt <= CopyNum; mtCnt++)
                                        PrintMTForms(logicalService, mtCnt, CopyNum, bArchiveOnly); // #12903
                                }
                                finally
                                {
                                    // if PrintMTForms throws an exception, then forloop would exit without incrementing mtCnt
                                    // in this case, we want to ensure the printer is unlocked/closed so it does not continue locking
                                    if (mtCnt != CopyNum)   // WI#26012
                                        _printer.Unlock();

                                    // close printer after all copies have been printed and signature captured (or exception thrown) 
                                    _printer.Close();
                                }
                            }
                            _tlTranSet.PrinterPtid.SetValueToNull(); // #12983 Prevent Cashier's Check Info from popping up twice

                            /*end 13435*/
                        }
					}
				}
				#endregion
			}
			finally
			{
				#region Endorsements
				//Begin #79510, #09368
				if (_tlTranSet.IsAcquirerTran)
				{
					_tlTranSet.UpdateEndorsementStatus();
				}
				//End #79510, #09368
				#endregion

				#region Bonds
				//#04449 - Added loop
				foreach (TlTransaction tlTran in _tlTranSet.Transactions)
				{
					if (tlTran.BondExists)
					{
						if (PMessageBox.Show(11214, MessageType.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							printBondFDFs();
						}
						break;
					}
				}
				#endregion

				#region Cleanup
				printPending = false;
				showItemField = false;
				#endregion
				//
				if (_isFormRecalcPmtHistoryInvoked) //#76425
					ActionAfterHandlePrinting();
			}
		}

        private string GetPEDDisplayDetailForTC(int trancode)
        {
            string tcDisplayStr = string.Empty;

            if (trancode <= 0)
                return tcDisplayStr;

            if (trancode >= 100 && trancode <= 149)
                tcDisplayStr = "Deposit";
            else if (trancode >= 150 && trancode <= 199)
                tcDisplayStr = "Withdrawal";
            else if ((trancode >= 300 && trancode <= 349) || trancode == 545 || trancode == 548)
                tcDisplayStr = "Payment";
            else if (trancode >= 350 && trancode <= 399)
                tcDisplayStr = "Advance";
            else if (trancode == 596 || trancode == 597 || trancode == 598)
                tcDisplayStr = "Refund";
            else if (trancode == 902)
                tcDisplayStr = "Official Check";
            else if (trancode == 910)
                tcDisplayStr = "Money Order";
            else if (trancode == 912 || trancode == 921 || trancode == 922)
                tcDisplayStr = "Savings Bond";
            else if (trancode == 915 || trancode == 916)
                tcDisplayStr = "Travelers Check";
            else if (trancode == 920)
                tcDisplayStr = "Utility Payment";
            else if (trancode == 923 || trancode == 924 || trancode == 926 || trancode == 927)
                tcDisplayStr = "Wire";
            else if (trancode == 925)
                tcDisplayStr = "T T & L";
            else if (trancode == 938 || trancode == 939)
                tcDisplayStr = "Inventory";

            return tcDisplayStr;
        }

		//#157637 Optimized to take advantage of new xfprinter imaging functionality
        private void PrintMTForms(string logicalService, int currentCopy, int numberOfCopies, bool archiveOnly)	// 157637(2)JSR
		{
			if (_mtTranCount > 0)
			{
				#region Initialize

				int tranNo = -1;
				int chargeNo = 0;
				int copiesCount = 1; // #12903
				int arrayOffset = 0;
				int itemNo = -1;
				int printTranCode = 0;  //#76057
				string[] asRegCCHolds = null; // #74269
				PrintInfo printInfo = null;
				PrintInfo holdPrintInfo = null;
				ArrayList prevPrintInfo = null;
				ArrayList MtTran = new ArrayList(); // #13832
				AdTlFormMtTran adTlFormMtTran = new AdTlFormMtTran();
				bool skipTran = false;

				bool canContinue = true;
				bool canPrint = false;
				bool rePrint = false;
				bool skipChecks = false;
				bool collectChkInfo = false;
				int isConditionalPrintForm = 0; //#3401
				bool formFound = false;
				string mediaName = null;
				string formName = null;
                int tmpFormID = 0; //#119734
				_tlTranSet.MtCashInTotal.Value = 0; // #12687
				_tlTranSet.MtCksOnUs.Value = 0; // #12610
				_tlTranSet.MtCashOutTotal.Value = 0; // #12886
				#endregion

				#region Get AdTlFormMtTran Info
				// #13832
				adTlFormMtTran.OutputTypeId.Value = 0;
				MtTran = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(adTlFormMtTran);
				if (MtTran != null && MtTran.Count > 0)
				{
					foreach (Phoenix.BusObj.Admin.Teller.AdTlFormMtTran _adTlFormMtTran in MtTran)
					{
						adTlFormMtTran = _adTlFormMtTran;
					}
				}
				// End #13832

				if (_tlTranSet.Transactions.Count <= 1)
					_tlTranSet.MtReceiptText.Value = "";

				#endregion

				#region get form info
				printInfo = new PrintInfo();

				//Get Header Form Info....
				printInfo.MtReceiptText = _tlTranSet.MtReceiptText.Value;
				formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", adTlFormMtTran.HeaderFormId.Value);


				mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
				formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
                tmpFormID = _tlTranSet.TellerVars.AdTlForm.FormId.Value ;  //#119734


                #endregion

                if (_isFormRecalcPmtHistoryClosed) //#76425
				{
					try
					{
						PrintInfoArray.Clear();	// #157637

						#region PRINT HEADER
						#region Load Customer Info

						if (_printRimNo != 0)
						{
                            //#132711
                            if (this._acctSearchBusObj.RimNo.IsNull || this._acctSearchBusObj.RimNo.Value != _printRimNo || this._acctSearchBusObj.Zip.IsNull || string.IsNullOrEmpty(this._acctSearchBusObj.Zip.StringValue))
                            {
                                this._acctSearchBusObj.ActionType = XmActionType.Select;
                                this._acctSearchBusObj.RimNo.Value = _printRimNo;
                                this._acctSearchBusObj.OutputType.Value = 2;
                                CoreService.DataService.ProcessRequest(this._acctSearchBusObj);
                            }

                            printInfo.RimNo = _printRimNo;

							printInfo.CustomerCityState = this._acctSearchBusObj.City.StringValue + " " + this._acctSearchBusObj.State.StringValue;
							printInfo.CustomerAddress = this._acctSearchBusObj.Address1.StringValue;
							printInfo.CustomerZip = this._acctSearchBusObj.Zip.StringValue;
							printInfo.CustomerPhone = this._acctSearchBusObj.RimHomePhoneNo.StringValue;
							printInfo.CustomerName = _tlTranSet.GlobalHelper.ConcateNameX(this._acctSearchBusObj.RimFirstName.Value, this._acctSearchBusObj.RimLastName.Value, this._acctSearchBusObj.RimMiddleInitial.Value, false);
							_printCustName = printInfo.CustomerName;
						}
						else
						{
							// WI#XXX - redundant code
							//if (!_tlTranSet.CurTran.TranAcct.RimNo.IsNull)
							//    printInfo.RimNo = _tlTranSet.CurTran.TranAcct.RimNo.Value;

							if (!_tlTranSet.CurTran.TranAcct.RimNo.IsNull && _tlTranSet.CurTran.TranAcct.RimNo.Value > 0)	// && (_tlTranSet.CurTran.TranAcct.RimFirstName.IsNull == false || _tlTranSet.CurTran.TranAcct.RimLastName.IsNull == false))     //#76033	// WI#XXX //#124918
							{
								// printInfo.CustomerName = _tlTranSet.GlobalHelper.ConcateNameX(_tlTranSet.CurTran.TranAcct.RimFirstName.Value, _tlTranSet.CurTran.TranAcct.RimLastName.Value, _tlTranSet.CurTran.TranAcct.RimMiddleInitial.Value, false);	// WI#XXX

								#region get customer info #80615

								this._acctSearchBusObj.ActionType = XmActionType.Select;
								this._acctSearchBusObj.RimNo.Value = _tlTranSet.CurTran.TranAcct.RimNo.Value; ;
								this._acctSearchBusObj.OutputType.Value = 2;
								CoreService.DataService.ProcessRequest(this._acctSearchBusObj);

								printInfo.CustomerName = _tlTranSet.GlobalHelper.ConcateNameX(this._acctSearchBusObj.RimFirstName.Value, this._acctSearchBusObj.RimLastName.Value, this._acctSearchBusObj.RimMiddleInitial.Value, false);	// WI#XXX
								printInfo.CustomerCityState = this._acctSearchBusObj.City.StringValue + " " + this._acctSearchBusObj.State.StringValue;
								printInfo.CustomerAddress = this._acctSearchBusObj.Address1.StringValue;
								printInfo.CustomerZip = this._acctSearchBusObj.Zip.StringValue;
								printInfo.CustomerPhone = this._acctSearchBusObj.RimHomePhoneNo.StringValue;
								#endregion
								_printCustName = printInfo.CustomerName;
								_printRimNo = _tlTranSet.CurTran.TranAcct.RimNo.Value;
							}
						}
						#endregion

						printInfo.WosaFormName = formName;
						printInfo.WosaMediaName = mediaName;
						printInfo.WosaLogicalService = logicalService;
                        printInfo.FormID = tmpFormID;   //#119734
						_tlTranSet.GetGlobalPrintInfo(printInfo);
                        if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y" && (PrintAndArchive || archiveOnly))//13435 added printandarchive boolean   //#194535
						{
							CoreService.LogPublisher.LogDebug("starting header" + formName);
							byte[] image = null;	// #157637
							bool continuePrint = _printer.PrintMtFormAndReturnImage(mediaName, formName, printInfo, out image, archiveOnly);	  //#157637  // 157637(2)JSR
							CoreService.LogPublisher.LogDebug("end header");
							if (!continuePrint)
								return;
						}
						#endregion

						PrintInfoArray.Add(printInfo);
						holdPrintInfo = printInfo;

						#region PRINT FORMS
						do
						{
							try
							{
								canContinue = _tlTranSet.GetNextMTPrintObject(null, skipTran, rePrint, skipChecks,
									ref showItemField, ref tranNo, ref chargeNo, ref itemNo, ref formId,
									ref copiesCount, ref collectChkInfo,
									ref printInfo, ref arrayOffset, ref prevPrintInfo, ref canReprint, ref isConditionalPrintForm);
							}
							catch (PhoenixException pe)
							{
								PMessageBox.Show(pe, MessageBoxButtons.OK);
							}
							finally
							{
								dlgInformation.Instance.HideInfo();
							}
                            if (printInfo != null)  //#118299 - fixed base bug
                                printInfo.RequireSignature = (_tlTranSet.AdTlTc.RequireSignature.Value == "Y");	// #157637 (2)
							//_tlTranSet.TellerVars.SetContextObject("AdTlTcArray", Convert.ToInt32(printInfo.TlTranCode));
							//printInfo.RequireSignature = (_tlTranSet.TellerVars.AdTlTc.RequireSignature.Value == GlobalVars.Instance.ML.Y);


							canPrint = !(!ProcessResponses(null) || printInfo == null);

							if (canContinue && canPrint)
							{
								#region Get Float Lines - #74269
								if (printInfo.RegCCHolds != null)
								{
									asRegCCHolds = printInfo.RegCCHolds.Split('^');
								}
								#endregion

								printInfoForm = printInfo;

								printTranCode = Convert.ToInt16(_tlTranSet.CurTran.TranCode.Value);

								if (!(_tlTranSet.IsCheckPrintAvailable(false, -1, false) && collectChkInfo && (printTranCode == 902 || printTranCode == 910))) // #76057 Do not print Wosa if Sqr
								{
									for (int i = 1; i <= copiesCount; i++)
									{
										if (i > 1)
											printInfo.Duplicate = true;
										passbookAcctDetails = null;

										#region Print Single/Multiple Forms - #74269

										if (isConditionalPrintForm >= 0)
										{
											if (_tlTranSet.AdTlTc.IncludeMtPrint.Value == "Y")
											{
												if (formId.ToString().Trim() != "")
												{
													if (_printRimNo == 0 && _tlTranSet.CurTran.TranAcct.RimNo.Value != int.MinValue)
													{
														_printRimNo = _tlTranSet.CurTran.TranAcct.RimNo.Value;
														_printCustName = printInfo.CustomerName;
													}
													formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", formId);

													if (_tlTranSet.TellerVars.AdTlForm.TextQrp.Value == "R")
													{

														mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
														formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
                                                        tmpFormID = _tlTranSet.TellerVars.AdTlForm.FormId.Value;    //#119734

                                                        #region Balances

                                                        _printTfrBalances = true;
														_printPrimaryBalances = true;
														if (_printBalOption == "A")
														{
															if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y" && !_tlTranSet.CurTran.TranAcct.bAcctHasPostAccessOnly.Value == true
																&& !((_tlTranSet.GlobalHelper.IsConfAcct(_tlTranSet.CurTran.AcctType.Value, _tlTranSet.CurTran.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
																&& !(_tlTranSet.CurTran.TranAcct.TranCode.Value >= 300 && _tlTranSet.CurTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.TellerVars.AdTlTc.RealTimeEnable.Value != "Y")
																&& !(_tlTranSet.CurTran.TranAcct.DepLoan.Value == "GL")
																&& !(_tlTranSet.CurTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TranAcct.TranCode.Value)))
															{
																_tlTranSet.CurTran.PrintBalance.Value = "Y";
															}
															else
															{
																_tlTranSet.CurTran.PrintBalance.Value = "N";
															}
															if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
															{
																if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y" && !_tlTranSet.CurTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
																	&& !((_tlTranSet.GlobalHelper.IsConfAcct(_tlTranSet.CurTran.TfrAcctType.Value, _tlTranSet.CurTran.TfrAcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
																	&& !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.TellerVars.AdTlTc.RealTimeEnable.Value != "Y")
																	&& !(_tlTranSet.CurTran.TfrAcct.DepLoan.Value == "GL")
																	&& !(_tlTranSet.CurTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))) // #13713
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
															printInfo = SuppressBalances(printInfo, _tlTranSet.CurTran.TranAcct.bAcctHasPostAccessOnly.Value || _tlTranSet.CurTran.TfrAcct.bAcctHasPostAccessOnly.Value);
														}
														if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
														{
															if (_tlTranSet.CurTran.PrintBalance.Value == "N")
																_printPrimaryBalances = false;
															if (_tlTranSet.CurTran.TfrTranCode.IsNull || _tlTranSet.CurTran.PrintTfrBalance.Value == "N")
																_printTfrBalances = false;
															if (!_printPrimaryBalances || !_printTfrBalances)
																printInfo = SuppressBalances(printInfo, _tlTranSet.CurTran.TranAcct.bAcctHasPostAccessOnly.Value || _tlTranSet.CurTran.TfrAcct.bAcctHasPostAccessOnly.Value);
														}

														#endregion

														try
														{
															printInfo.WosaFormName = formName;
															printInfo.WosaMediaName = mediaName;
															printInfo.WosaLogicalService = logicalService;
                                                            printInfo.FormID = tmpFormID;   //#119734
                                                            if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y" && (PrintAndArchive || archiveOnly))//13435 //#194535
															{
																CoreService.LogPublisher.LogDebug("starting " + formName);
																byte[] image = null;	// #157637
																_printer.PrintMtFormAndReturnImage(mediaName, formName, printInfo, out image, archiveOnly);	//#157637	
																CoreService.LogPublisher.LogDebug("ending " + formName);
															}
														}
														finally
														{
														}
														PrintInfoArray.Add(printInfo);

														holdPrintInfo = printInfo;

													}
												}
											}
										}
										#endregion

									}
									//Begin #79510, #09368
									if (_tlTranSet.IsAcquirerTran && dialogResult == DialogResult.OK && printInfo.ItemNo > 0 && printInfo.ItemNo <= _tlTranSet.CurTran.Items.Count)
									{
										(_tlTranSet.CurTran.Items[printInfo.ItemNo - 1] as TlItemCapture).EndorseStatus.Value = "P";
									}
									//End #79510, #09368
								} // #76057 End If
							}
						} while (canContinue);
						#endregion

						#region Print FOOTER

						printInfo = holdPrintInfo.Clone();

						#region get form info

						formFound = _tlTranSet.TellerVars.SetContextObject("AdTlFormArray", adTlFormMtTran.FooterFormId.Value);


						mediaName = _tlTranSet.TellerVars.AdTlForm.MediaName.Value;
						formName = _tlTranSet.TellerVars.AdTlForm.FormName.Value;
						printInfo.WosaLogicalService = logicalService;

						#endregion


						if (currentCopy > 1)
							printInfo.Duplicate = true;

                        if (_tlTranSet.TellerVars.AdTlControl.WosaPrinting.Value == "Y" && (PrintAndArchive || archiveOnly))//13435 //#194535
						{
							CoreService.LogPublisher.LogDebug("starting footer");
							byte[] image = null;
							_printer.PrintMtFormAndReturnImage(mediaName, formName, printInfo, true, out image, archiveOnly);  //#157637
							CoreService.LogPublisher.LogDebug("end footer");
						}

						PrintInfoArray.Add(printInfo);

						#endregion
					}
					finally
					{
						// unlock printer before signatures 
						if (currentCopy == numberOfCopies)	// WI#26012
							_printer.Unlock();
					}

					//#157637 simplified processing since imaging is done by the xfsprinter
					// WI#26012 - remove currentCopy from if
					if ((_tlTranSet.IsHylandVoucherSvcAvailable() || _tlTranSet.TellerVars.IsECMVoucherAvailable) &&
						_tlTranSet.IsVoucherForm(_tlTranSet.TellerVars.AdTlForm.FormId.Value, printInfo))   //#118299
					{
						// waive signature would go here, except was removed in WI#12619
						// we think that maybe this should be required:
						// if (!_tlTranSet.CurTran.WaiveSignature.IsNull && _tlTranSet.CurTran.WaiveSignature.Value == "Y"), 
						// do not break, set require = FALSE, go to next record
						// not making the change now because not a reported issue, and don't want to take on added risk in HF release
						bool mtRequireSignature = false;
                        string customerNameDispplay = string.Empty;
                        string cashBackDisplay = string.Empty;
                        string combinedPEDDisplayDetail = string.Empty;
                        string acctInfoDisplay = string.Empty;
                        string tranCodeDesc = string.Empty;
						foreach (PrintInfo pInfo in PrintInfoArray)
						{
							if (pInfo.RequireSignature)
							{
								mtRequireSignature = true;
                                //break;    //#45773 - Continue to get complete display details
							}

                            //Begin #45773
                            tranCodeDesc = GetPEDDisplayDetailForTC(pInfo.GbTranCode);
                            if (string.IsNullOrEmpty(customerNameDispplay) && pInfo.RimNo > 0)
                                customerNameDispplay = string.Format(@"{0} #{1}", pInfo.CustomerName, pInfo.RimNo);
                            if (string.IsNullOrEmpty(cashBackDisplay) && !_tlTranSet.TranSetTotalCashOut.IsNull && _tlTranSet.TranSetTotalCashOut.Value > 0)
                                cashBackDisplay = string.Format(@"{0} ${1}", "Cash Back", _tlTranSet.TranSetTotalCashOut.Value);
                            if (!string.IsNullOrEmpty(pInfo.AcctNo) && !string.IsNullOrEmpty(pInfo.AcctType) && pInfo.TranAmt > 0 &&
                                pInfo.WosaFormName.IndexOf("Header") == -1 && pInfo.WosaFormName.IndexOf("Footer") == -1)
                                acctInfoDisplay = string.Format(@"{0}-{1} {2} ${3}", pInfo.AcctType, (pInfo.AcctNo != null && pInfo.AcctNo.Length > 4 ?
                                    Phoenix.Shared.UtilityFunctions.MaskInput(pInfo.AcctNo, pInfo.AcctNo.Length - 4, false) : pInfo.AcctNo),
                                    (string.IsNullOrEmpty(tranCodeDesc) ? pInfo.TlTranCodeDesc : tranCodeDesc), pInfo.TranAmt);
                            if (string.IsNullOrEmpty(combinedPEDDisplayDetail) && !string.IsNullOrEmpty(acctInfoDisplay))
                                combinedPEDDisplayDetail = acctInfoDisplay;
                            else if (!string.IsNullOrEmpty(combinedPEDDisplayDetail) && !string.IsNullOrEmpty(acctInfoDisplay))
                                combinedPEDDisplayDetail = string.Format("{0}^{1}", combinedPEDDisplayDetail, acctInfoDisplay);
                            acctInfoDisplay = string.Empty;
                            //End #45773
						}

                        if (!string.IsNullOrEmpty(combinedPEDDisplayDetail))    //#45773
                        {
                            combinedPEDDisplayDetail = string.Format("{0}^{1}", combinedPEDDisplayDetail, cashBackDisplay);
                            _tlTranSet.DisplayDetailForPED.Value = combinedPEDDisplayDetail;
                            _tlTranSet.DisplaySummaryForPED.Value = customerNameDispplay;
                            CoreService.LogPublisher.LogDebug("PrintMTForms: PED Display Summary Text " + customerNameDispplay);
                            CoreService.LogPublisher.LogDebug("PrintMTForms: PED Display Details Text " + combinedPEDDisplayDetail);
                        }

                        if (currentCopy == 1 || combinedBitmap == null)	// WI#26012 //#194535
						{
							combinedBitmap = null;	// WI#26012
							sigPos = null;			// WI#26012
							_printer.GetCombinedBitmapAndSigBoxDetails(out combinedBitmap, out sigPos);
						}

						if (currentCopy == numberOfCopies)
						{
							_printer.ClearBitmaps();
                            SignAndArchiveVoucher(combinedBitmap, sigPos, printInfo, mtRequireSignature, true, emailAddress);	// WI#157637 (4)    //#194535
						}
					}
				}
			}
		}

        //Begin #45773
        private bool GetNexusWebAPISignature(out Image signature)
        {
            bool gotSignature = false;
            string[] displayDetailArray = null;
            signature = null;

            if (!_tlTranSet.TellerVars.IsAppOnline)
            {
                CoreService.LogPublisher.LogError("GetNexusWebAPISignature: Nexus Signature Capture not available in offline mode");
                return gotSignature;
            }

            PinDevice _pinDevice = PinDevice.Instance;

            //#75436
            Phoenix.BusObj.Admin.Global.AdGbInterface nexusInterface = new BusObj.Admin.Global.AdGbInterface();
            nexusInterface.ProductId.Value = 4;
            nexusInterface.ActionType = XmActionType.Select;
            CoreService.DataService.AddObject(nexusInterface);
            CoreService.DataService.ProcessRequest();

            if (!nexusInterface.SaveToBranchSubfolder.IsNull && nexusInterface.SaveToBranchSubfolder.Value == "Y")
            {
                _pinDevice.VerifyNexusCertificateRoot = true;
            }

            try
            {
                //#47041
                //Force Default Time Out to 1 minute
                //_pinDevice.DefaultTimeOut = 30; //Set PED display time to 30 seconds for view and sign
                _pinDevice.ForwardException = true; //Handle exception locally instead of wrapper
                if (!_tlTranSet.DisplayDetailForPED.IsNull)
                    displayDetailArray = _tlTranSet.DisplayDetailForPED.Value.Split('^');
                //dlgInformation.Instance.ShowInfo("Invoking Nexus Signature API to capture signature...please wait");
                dlgInformation.Instance.ShowInfo(CoreService.TranslateText("Waiting for customer's signature...", GlobalVars.InstitutionType, this.FormSource));
                if (!_pinDevice.GetSignature("BMP", (_tlTranSet.DisplaySummaryForPED.IsNull ? "Summary" : _tlTranSet.DisplaySummaryForPED.Value),
                    (_tlTranSet.DisplayDetailForPED.IsNull ? new string[] { "Please sign" } : displayDetailArray), ref signature))
                {
                    CoreService.LogPublisher.LogError("GetNexusWebAPISignature: Nexus PED Web API - Get Signature failed");
                    string deviceErrorReason = string.Empty;
                    if (_pinDevice.LastErrorType == PinDevice.ErrorType.DeviceNotReady)
                        deviceErrorReason = " due to device not ready";
                    else if (_pinDevice.LastErrorType == PinDevice.ErrorType.DeviceTimeOut)
                        deviceErrorReason = " due to timeout";
                    else if (_pinDevice.LastException != null)
                        _pinDevice.ProcessLastException();

                    //15515 - Signature not captured%1!. Do you want to retry?
                    if (DialogResult.No == PMessageBox.Show(15515, MessageType.Warning, MessageBoxButtons.YesNo, new string[] { deviceErrorReason }))
                    {
                        return gotSignature;
                    }
                    else
                    {
                        signature = null;
                        return GetNexusWebAPISignature(out signature);
                    }
                }

                if (signature != null)
                {
                    gotSignature = true;

                    if (gotSignature)   //#45773
                    {
                        pedSignature = signature;
                        CallOtherForms("PEDSignatureCapture");
                        if (retryPEDSignature)
                        {
                            signature = null;
                            GetNexusWebAPISignature(out signature);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //#47041
                //The operation has timed out
                CoreService.LogPublisher.LogError("GetNexusWebAPISignature: " + e.Message);
                //Option1: Handle Time Out return
                //Option2: Call CallOtherForms("PEDSignatureCapture") to review and redo signature
                //Option3: Set DefaultTimeout = 0 to remove time out when the fix is available from Nexus
                //if (!string.IsNullOrEmpty(e.Message) && e.Message.IndexOf("timed out") > 0) //#47041
                //{
                //    CoreService.LogPublisher.LogError("GetNexusWebAPISignature: The operation has timed out");
                //    //Send it to Signature Capture for resign
                //    pedSignature = signature;
                //    CallOtherForms("PEDSignatureCapture");
                //    if (retryPEDSignature)
                //    {
                //        signature = null;
                //        GetNexusWebAPISignature(out signature);
                //    }
                //}
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
            return gotSignature;
        }

        private string CombineBitmaps(string voucherImageFilePath, string voucherImageName, Image sigImage)
        {
            string outFilePath = string.Empty;
            if (string.IsNullOrEmpty(voucherImageName))
            {
                CoreService.LogPublisher.LogError("CombineBitmaps: The voucher image file name is empty");
                return outFilePath;
            }

            if (string.IsNullOrEmpty(voucherImageFilePath))
            {
                CoreService.LogPublisher.LogError("CombineBitmaps: The voucher image file path is empty");
                return outFilePath;
            }

            if (sigImage == null)
            {
                CoreService.LogPublisher.LogError("CombineBitmaps: The signature image is null");
                return outFilePath;
            }

            //Make a copy of voucher image to work
            System.IO.File.Copy(Path.Combine(voucherImageFilePath, voucherImageName), Path.Combine(voucherImageFilePath, "Copy_" + voucherImageName));
            Image dImg = System.Drawing.Image.FromFile(Path.Combine(voucherImageFilePath, "Copy_" + voucherImageName));

            Bitmap[] files = new Bitmap[2];
            files[0] = new Bitmap(dImg);
            files[1] = new Bitmap(sigImage);

            var images = new List<Bitmap>();
            Bitmap finalImage = null;



            try
            {
                int width = 0;
                int height = 0;
                //int adjImageHeight = 0;
                //int adjOffsetX = 0;

                int receiptHeight = files[0].Height;
                int receiptWidth = files[0].Width;
                int sigHeight = sigImage.Height;
                int sigWidth = sigImage.Width;
                int sigImageXLoc = sigWidth >= receiptWidth ? 0 : (receiptWidth - sigWidth) / 2;

                //determine the bottom of printed portion of receipt
                int sigImageYLoc = 0;
                int rowInterval = 1;  // configure the loop row interval to scan for non white pixel in the image.
                //int receiptHeightAdj = 0;
                try
                {
                    for (int row = receiptHeight - 1; row >= 0; row -= rowInterval)
                    {
                        for (int i = 0; i < receiptWidth; ++i)
                        {
                            if (files[0].GetPixel(i, row).R != 255)
                            {
                                sigImageYLoc = row;
                                break;
                            }
                        }
                        if (sigImageYLoc != 0)
                            break;
                    }
                }
                catch (Exception e)
                {
                    CoreService.LogPublisher.LogError("CombineBitmaps: Error determining the bottom of image :" + e.ToString());
                }

                if (sigImageYLoc == 0)
                    sigImageYLoc = receiptHeight;
                else
                {
                    sigImageYLoc = sigImageYLoc + 200 + rowInterval;   // gap between last printed line and signature
                    //receiptHeightAdj = sigImageYLoc - receiptHeight;
                }


                foreach (Bitmap image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = image;

                    //update the size of the final bitmap
                    //Cut down 5 inches of height to fit in signature close to the receipt content
                    width = bitmap.Width > width ? bitmap.Width : width;
                    //height += (height == 0? (bitmap.Height - 680) : bitmap.Height);
                    height += bitmap.Height;
                    images.Add(bitmap);
                }

                height = sigImageYLoc + sigHeight + 100;  // adjust the height of combined image to signature location and it's height;

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.White);

                    //go through each image and draw it on the final image
                    //int offset = 0;

                    //foreach (Bitmap image in images)
                    //{
                    //    if (offset == 0)
                    //    {
                    //        adjImageHeight = image.Height - 1920;   //Reduced 20 inches from the bottom to stich the sig image closer to the receipt
                    //        adjOffsetX = 0;
                    //    }
                    //    else
                    //    {
                    //        adjImageHeight = image.Height;
                    //        adjOffsetX = 576;   //Adjusted 6 inches
                    //    }
                    //    g.DrawImage(image, new Rectangle(adjOffsetX, offset, image.Width, adjImageHeight));
                    //    offset += adjImageHeight;
                    //}

                    g.DrawImage(images[0], new RectangleF(0, 0, images[0].Width, sigImageYLoc),
                        new RectangleF(0, 0, images[0].Width, sigImageYLoc), GraphicsUnit.Pixel);
                    g.DrawImage(images[1], new Rectangle(sigImageXLoc, sigImageYLoc, images[1].Width, images[1].Height));
                }

                //Save Final Image for archival
                outFilePath = Path.Combine(voucherImageFilePath, "SignedCopy_" + voucherImageName);
                finalImage.Save(outFilePath);
            }
            finally
            {

                if (finalImage != null)
                    finalImage.Dispose();
                if (dImg != null)
                    dImg.Dispose();

                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }

            return outFilePath;
        }
        //End #45773
		
		//#157637  Simplified processing since imaging is allready by the xfsprinter
		public bool SignAndArchiveVoucher(Bitmap combinedBitmap, SigBoxDetails sigPos, PrintInfo printInfo, bool requireSignature, bool mtTransaction, string emailAddress)    //#194535
        {
			bool completed = false;
			string combinedImageFile = string.Empty;
            List<string> emailList = new List<string>();              //#194535
            string combinedImageFileCopy = string.Empty; //#45773
            string signedCombinedImageFile = string.Empty; //#45773

            if (!_tlTranSet.TellerVars.IsAppOnline)
                return true;         //#42151

            try
            {
                //WI#12980 //#42151 Comment out and move up
                ////bool offline = !_tlTranSet.TellerVars.IsAppOnline;
                ////if (offline)
                ////    return true;
                //end WI#12980

                _archivePtid = _tlTranSet.CurTran.Ptid.Value;
                combinedImageFile = Path.Combine(_tlTranSet.GetVoucherImageFilePath(), _tlTranSet.GetVoucherImageFileName(_archivePtid));
                combinedBitmap.Save(combinedImageFile);

                bool sigFieldPresent = (sigPos != null);

                bool isdDriveThru = (TellerVars.Instance.IsDriveThruWorkstation == 1);
                bool promptForSignature = (sigFieldPresent && requireSignature && (!isdDriveThru));

                if (promptForSignature && _idsSignedArchieved.IndexOf(_tlTranSet.CurTran.Ptid.StringValue) >= 0)
                {
                    //Begin 120142
                    if (_tlTranSet.TellerVars.IsECMVoucherAvailable)
                        promptForSignature = false; //do no prompt for a signature only archive the receipt
                    else
                        return true;
                    //End 120142
                }

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
                //Begin 119058-3
                decimal amtCashIn = 0;
                decimal amtCashOut = 0;
                //End 119058-3

                if (mtTransaction)
                {
                    #region AmtToSignFor
                    decimal tmpAmtToSignFor = 0;
                    foreach (TlTransaction tlTran in _tlTranSet.Transactions)
                    {
                        _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", tlTran.TlTranCode.Value);
                        tmpAmtToSignFor += _tlTranSet.GetAmtToSignFor(tlTran, _tlTranSet.TellerVars.AdTlTc);

                        if (promptForSignature) //#49555/#49556
                        {
                            if (!tlTran.WaiveSignature.IsNull && tlTran.WaiveSignature.Value == "Y")
                                promptForSignature = false;
                        }

                        //Begin 119058-3
                        amtCashIn += (tlTran.CashIn.IsNull ? 0 : tlTran.CashIn.Value);
                        amtCashOut += (tlTran.CashOut.IsNull ? 0 : tlTran.CashOut.Value);
                        //End 119058-3
                    }

                    if (!double.TryParse(Convert.ToString(tmpAmtToSignFor), out amtToSignFor))
                        amtToSignFor = 0;
                    if (!_tlTranSet.CurTran.TlTranCode.IsNull)  // set value back
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

                    foreach (TlTransaction tlTran in _tlTranSet.Transactions) //#49555/#49556
                    {
                        if (promptForSignature)
                        {
                            if (!tlTran.WaiveSignature.IsNull && tlTran.WaiveSignature.Value == "Y")
                            {
                                promptForSignature = false;
                                break;
                            }
                        }
                    }
                }
                // end #157637 (4)

                // #140784-2 - Added parameter for IsSharedBranch (_tlTranSet.IsAcquirerTran)
                emailList.Add(emailAddress);  //#194535

                //Begin #45773
                if (_tlTranSet.TellerVars.IsNexusPEDEnabled)
                {
                    Image signature = null;
                    if (promptForSignature) //#49555/#49556
                    {
                        try
                        {
                            promptForSignature = false;
                            if (GetNexusWebAPISignature(out signature))
                            {
                                signedCombinedImageFile = CombineBitmaps(_tlTranSet.GetVoucherImageFilePath(), _tlTranSet.GetVoucherImageFileName(_archivePtid), signature);
                                if (string.IsNullOrEmpty(signedCombinedImageFile))
                                    CoreService.LogPublisher.LogError("SignAndArchiveVoucher: Failed to burn signature image on voucher receipt");
                            }
                            else
                                CoreService.LogPublisher.LogError("SignAndArchiveVoucher: Nexus PED Web API Signature Capture Failed");
                        }
                        finally
                        {
                            if (signature != null)
                                signature.Dispose();

                            if (pedSignature != null)
                                pedSignature.Dispose();
                        }
                    }
                }
                //#45773 - Modified the combinedImageFile param value based on condition
                param = new ParameterService(ovrdSig, rimNo,
                    amtToSignFor,   // #157637 (4) (double)(printInfo.AmtToSignFor != DbDecimal.Null ? printInfo.AmtToSignFor : _tlTranSet.CurTran.NetAmt.Value),   // #04778 - check for amt to sign for as null
                    amtNet,         // #157637 (4) (double)_tlTranSet.MtNetAmt.Value, 
                    custName,      // #79510 - Replaced wosaPrintInfo.CustomerName by custName
                    _tlTranSet.CurTran.Ptid.StringValue,
                    (string.IsNullOrEmpty(signedCombinedImageFile) ? combinedImageFile : signedCombinedImageFile), promptForSignature,
                    instNo,
                    sigPos.x1, sigPos.y1, sigPos.x2, sigPos.y2, sigPos.CombinedFormWidth, sigPos.CombinedformHeight,
                    tellerNo, Convert.ToInt32(Phoenix.FrameWork.Shared.Variables.GlobalVars.CurrentBranchNo),
                    Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeName, emailList, _tlTranSet.IsAcquirerTran);		//#5117 //#194535

                doc = new DocumentService();
                doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable; //#118299
                if (doc.IsECMEnabled) //#118299
                {
                    //Begin 119058-2

                    List<TranArchiveDetail> tranDetails = new List<TranArchiveDetail>();

                    if (mtTransaction)
                    {
                        //#119734
                        //foreach (TlTransaction tlTran in _tlTranSet.Transactions)
                        //{
                        //    tranDetails.Add(new TranArchiveDetail
                        //    {
                        //        JournalPtid = tlTran.Ptid.Value,
                        //        SeqNo = (tlTran.SequenceNo.IsNull ? Convert.ToInt16(0) : tlTran.SequenceNo.Value),
                        //        SubSequence = (tlTran.SubSequence.IsNull ? Convert.ToInt16(0) : tlTran.SubSequence.Value),
                        //        //Begin 119058
                        //        AcctType = tlTran.AcctType.Value,
                        //        AcctNo = tlTran.AcctNo.Value,
                        //        BranchName = Phoenix.FrameWork.Shared.Variables.GlobalVars.BranchName,
                        //        CheckNo = tlTran.CheckNo.IsNull ? 0 : tlTran.CheckNo.Value,
                        //        CreateDt = tlTran.CreateDt.Value,
                        //        FormId = _tlTranSet.TellerVars.AdTlForm.FormId.IsNull ? Convert.ToInt16(0) : Convert.ToInt16(_tlTranSet.TellerVars.AdTlForm.FormId.Value),
                        //        TlTranCode = tlTran.TlTranCode.Value,
                        //        FormName = _tlTranSet.TellerVars.AdTlForm.FormName.IsNull ? string.Empty : _tlTranSet.TellerVars.AdTlForm.FormName.Value,
                        //        TranAmt = (_tlTranSet.TellerVars.AdTlForm.FormName.Value == "Cash In" ? amtCashIn :
                        //                   _tlTranSet.TellerVars.AdTlForm.FormName.Value == "Cash Out" ? amtCashOut : tlTran.NetAmt.Value) //119058-3
                        //        //End 119058
                        //    });
                        //}

                        foreach (PrintInfo printInfo1 in PrintInfoArray)
                        {
                            if (printInfo1.WosaFormName.IndexOf("Header") == -1 && printInfo1.WosaFormName.IndexOf("Footer") == -1)
                            {
                                tranDetails.Add(new TranArchiveDetail
                                {
                                    JournalPtid = (printInfo1.CheckNo == Decimal.MinValue ? 0 : printInfo1.JournalPtid),
                                    SeqNo = (printInfo1.SequenceNo == Int32.MinValue ? Convert.ToInt16(0) : Convert.ToInt16(printInfo1.SequenceNo)),
                                    SubSequence = (string.IsNullOrEmpty(printInfo1.SbSeqNo) ? Convert.ToInt16(0) : Convert.ToInt16(printInfo1.SbSeqNo)),
                                    AcctType = printInfo1.AcctType,
                                    AcctNo = printInfo1.AcctNo,
                                    BranchName = Phoenix.FrameWork.Shared.Variables.GlobalVars.BranchName,
                                    CheckNo = (printInfo1.CheckNo == Int32.MinValue ? 0 : printInfo1.CheckNo),
                                    CreateDt = printInfo1.CreateDt,
                                    FormId = (printInfo1.FormID == Int32.MinValue ? Convert.ToInt16(0) : Convert.ToInt16(printInfo1.FormID)),
                                    TlTranCode = printInfo1.TlTranCode,
                                    FormName = printInfo1.WosaFormName,
                                    TranAmt = (printInfo1.TranAmt == Decimal.MinValue ? 0 : printInfo1.TranAmt)
                                });
                            }
                        }
                    }
                    else
                    {
                        tranDetails.Add(new TranArchiveDetail
                        {
                            //#119734
                            //JournalPtid = _tlTranSet.CurTran.Ptid.Value,
                            //SeqNo = (_tlTranSet.CurTran.SequenceNo.IsNull ? Convert.ToInt16(0) : _tlTranSet.CurTran.SequenceNo.Value),
                            //SubSequence = (_tlTranSet.CurTran.SubSequence.IsNull ? Convert.ToInt16(0) : _tlTranSet.CurTran.SubSequence.Value),
                            ////Begin 119058
                            //AcctType = _tlTranSet.CurTran.AcctType.Value,
                            //AcctNo = _tlTranSet.CurTran.AcctNo.Value,
                            //BranchName = Phoenix.FrameWork.Shared.Variables.GlobalVars.BranchName,
                            //CheckNo = _tlTranSet.CurTran.CheckNo.IsNull ? 0 : _tlTranSet.CurTran.CheckNo.Value,
                            //CreateDt = _tlTranSet.CurTran.CreateDt.Value,
                            //FormId = _tlTranSet.TellerVars.AdTlForm.FormId.IsNull ? Convert.ToInt16(0) : Convert.ToInt16(_tlTranSet.TellerVars.AdTlForm.FormId.Value),
                            //TlTranCode = _tlTranSet.CurTran.TlTranCode.Value,
                            //FormName = _tlTranSet.TellerVars.AdTlForm.FormName.IsNull ? string.Empty : _tlTranSet.TellerVars.AdTlForm.FormName.Value,
                            //TranAmt = (_tlTranSet.TellerVars.AdTlForm.FormName.Value == "Cash In" ? (_tlTranSet.CurTran.CashIn.IsNull ? 0 : _tlTranSet.CurTran.CashIn.Value) : 
                            //           _tlTranSet.TellerVars.AdTlForm.FormName.Value == "Cash Out" ? (_tlTranSet.CurTran.CashOut.IsNull ? 0 :_tlTranSet.CurTran.CashOut.Value) : 
                            //           _tlTranSet.CurTran.NetAmt.Value) //119058-3
                            //End 119058

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
                    }
                    //End 119058-2

                    param.TranDetails.AddRange(tranDetails);
                }
                completed = (doc.SignAndArchiveVourcher(param, ref colStorMsg));
                //End #45773
                if (completed && (promptForSignature || !string.IsNullOrEmpty(signedCombinedImageFile)))    //#45773
                    _idsSignedArchieved = (_idsSignedArchieved == "") ? _idsSignedArchieved + _tlTranSet.CurTran.Ptid.StringValue : _idsSignedArchieved = _idsSignedArchieved + "~" + _tlTranSet.CurTran.Ptid.StringValue;
                _tlTranSet.CurTran.ColdStorComplete.Value = (completed) ? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N;
                _tlTranSet.CurTran.ColdStorMessage.Value = colStorMsg;

                if (_tlTranSet.CurTran.ColdStorComplete.Value != GlobalVars.Instance.ML.Y)  //TBD - Remove it later #118299
                {
                    string outFileName = string.Empty;
                    string outFilePath = (string.IsNullOrEmpty(signedCombinedImageFile) ? combinedImageFile : signedCombinedImageFile);
                    string seqNo = Convert.ToString((_tlTranSet.CurTran.SequenceNo.IsNull ? Convert.ToInt16(0) : _tlTranSet.CurTran.SequenceNo.Value));
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 10000);
                    string subSequence = Convert.ToString((_tlTranSet.CurTran.SubSequence.IsNull ? Convert.ToInt16(0) : _tlTranSet.CurTran.SubSequence.Value));
                    outFileName = string.Format(@"{0}{1}{2}.{3}", Path.GetFileName(outFilePath).Replace(".bmp", ""), (string.IsNullOrEmpty(_tlTranSet.CurTran.SequenceNo.StringValue) ? "" : "_" + seqNo), "_" + subSequence + "_" + randomNumber.ToString(), "png");
                    FileInfo fi = new FileInfo(outFilePath);
                    if (fi.Exists)
                    {
                        fi.MoveTo(Path.Combine(@"c:\temp\", outFileName));
                    }
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }
            finally
            {
                if (_tlTranSet.TellerVars.IsNexusPEDEnabled) //#45773
                {
                    combinedImageFile = Path.Combine(_tlTranSet.GetVoucherImageFilePath(), _tlTranSet.GetVoucherImageFileName(_archivePtid));
                    combinedImageFileCopy = Path.Combine(_tlTranSet.GetVoucherImageFilePath(), "Copy_" + _tlTranSet.GetVoucherImageFileName(_archivePtid));
                    if (File.Exists(combinedImageFile))
                        File.Delete(combinedImageFile);
                    if (!string.IsNullOrEmpty(combinedImageFileCopy) && File.Exists(combinedImageFileCopy))
                        File.Delete(combinedImageFileCopy);
                    if (!string.IsNullOrEmpty(signedCombinedImageFile) && File.Exists(signedCombinedImageFile))
                        File.Delete(signedCombinedImageFile);
                }
                else
                    File.Delete(combinedImageFile);
            }

			return completed;
        }

        #region #79502 - Suppress Balances
        /// <summary>
        /// SuppressBalances - Suppresses balance printing
        /// </summary>
        /// <param name="printInfo"></param>
        private PrintInfo SuppressBalances(PrintInfo printInfo, bool postAccessOnly)
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

                    if (postAccessOnly)
                        printInfo.PadLength = 4;
                    else
                        printInfo.PadLength = 3;
                }
            }
            return printInfo;
        }
        #endregion

		private PdfStandard dfInvSerialNo;
		private PdfStandard dfNoInvAvailable;
		private PLabelStandard lblNoInvAvailable;
		private PLabelStandard lblPacket;
		private PComboBoxStandard cmbInventoryType;
		private PLabelStandard lblInventoryType;
		private PComboBoxStandard cmbInventoryPacket;
		private PAction pbInventory;
        private PAction pbPendingCC;
        private PAction pbVod;
        private PAction pbRelationship;
        private PAction pbeReceipt;
        private PTabPage tabEasyCapture;
        private PAction pbAdvancedSep;
        private PAction pbAdvancedView;
        private PAction pbExceptionHold;

        //private PAction pbService; WI#29312 Reversed pbService button

    }
}
