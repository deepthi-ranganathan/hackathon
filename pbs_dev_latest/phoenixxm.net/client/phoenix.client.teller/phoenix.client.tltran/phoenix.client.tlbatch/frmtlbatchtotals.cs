#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmTlCharges.cs
// NameSpace: Phoenix.Client.TlCapturedItems
//-------------------------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//--------------------------------------------------------------------------------------------------
//   BELOW NOTE THIS CS FILE HAD NO MOD LOG TO BEGIN WITH ,THE MOD LOG WAS CREATED WITH ISSUE 74810 
//2/21/2008		1		njoshi 	Issue#74810 - When the printing of the batch fails, 
//											  the batch totals form is not refreashed
//04/08/2008    2       mselvaga    #75736 - QA Release 2008 TCD - BAT with TCD Cash Out should not update TL_JOURNAL.tcd_drawer_no and tcd_drawer_positn
//04/24/2009    3       mramalin    WI-3475 - Terminal Services Printing Enhancement
//01/25/2011    4       mselvaga    WI#11782 - Error received when batching checks on a TCR.
//08/06/2012    7       Mkrishna    #19058 - Adding call to base on initParameters.
//8/3/2013		8		apitava		#157637 Uses new xfsprinter
//06/05/2015    9       mselvaga    #37296 - Drawers are posting multiple batches.
//---------------------------------------------------------------------------------------------------
#endregion


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Client;
using Phoenix.Shared.Variables;

namespace Phoenix.Windows.Client
{
	/// <summary>
	/// Summary description for dfwTlBatchTotals.
	/// </summary>
	public class frmTlBatchTotals : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PPanel pnl0;
		private Phoenix.Windows.Forms.PGrid ctwBatchTotals;
		private Phoenix.Windows.Forms.PGridColumn colBatchID;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colTranCount;
		private Phoenix.Windows.Forms.PGridColumn colDrawerBatchAmt;
		private Phoenix.Windows.Forms.PGridColumn colTCDDispAmt;
		private Phoenix.Windows.Forms.PGridColumn colBatchTotal;
		private Phoenix.Windows.Forms.PAction pbBatchChecks;
		private Phoenix.Windows.Forms.PAction pbBatchDetails;
		private TlJournal _busObjTlJrnl = new TlJournal();

		private short branchNo;
		private short drawerNo;
		private DateTime postingDt;
		private short emplId;
		private DialogResult dialogResult = DialogResult.None;
        // Begin #72916
        private TellerVars _tellerVars = TellerVars.Instance;
        int localTcdMachineID;
        int localTcdDrawerNo;

        // End #72916
        #region WI-3475
        private PDecimal _noCopies = new PDecimal("NoCopies");
        private PString _printerService = new PString("PrinterService");
        private string _wosaServiceName;
		private PSmallInt _reprintFormId = new PSmallInt("FormId");
        #endregion


		public frmTlBatchTotals()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnl0 = new Phoenix.Windows.Forms.PPanel();
            this.ctwBatchTotals = new Phoenix.Windows.Forms.PGrid();
            this.colBatchID = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerBatchAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDDispAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchTotal = new Phoenix.Windows.Forms.PGridColumn();
            this.pbBatchChecks = new Phoenix.Windows.Forms.PAction();
            this.pbBatchDetails = new Phoenix.Windows.Forms.PAction();
            this.pnl0.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbBatchChecks,
            this.pbBatchDetails});
            // 
            // pnl0
            // 
            this.pnl0.Controls.Add(this.ctwBatchTotals);
            this.pnl0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl0.Location = new System.Drawing.Point(0, 0);
            this.pnl0.Name = "pnl0";
            this.pnl0.Size = new System.Drawing.Size(690, 448);
            this.pnl0.TabIndex = 0;
            this.pnl0.TabStop = true;
            // 
            // ctwBatchTotals
            // 
            this.ctwBatchTotals.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colBatchID,
            this.colDescription,
            this.colTranCount,
            this.colDrawerBatchAmt,
            this.colTCDDispAmt,
            this.colBatchTotal});
            this.ctwBatchTotals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctwBatchTotals.LinesInHeader = 2;
            this.ctwBatchTotals.Location = new System.Drawing.Point(0, 0);
            this.ctwBatchTotals.Name = "ctwBatchTotals";
            this.ctwBatchTotals.Size = new System.Drawing.Size(690, 448);
            this.ctwBatchTotals.TabIndex = 0;
            this.ctwBatchTotals.Click += new System.EventHandler(this.ctwBatchTotals_Click);
            // 
            // colBatchID
            // 
            this.colBatchID.PhoenixUIControl.ObjectId = 2;
            this.colBatchID.PhoenixUIControl.XmlTag = "BatchId";
            this.colBatchID.Title = "Batch Id";
            this.colBatchID.Visible = false;
            this.colBatchID.Width = 195;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 3;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 307;
            // 
            // colTranCount
            // 
            this.colTranCount.PhoenixUIControl.ObjectId = 4;
            this.colTranCount.PhoenixUIControl.XmlTag = "NoOfTransactions";
            this.colTranCount.Title = "# Of Transactions";
            this.colTranCount.Width = 75;
            // 
            // colDrawerBatchAmt
            // 
            this.colDrawerBatchAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDrawerBatchAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDrawerBatchAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDrawerBatchAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDrawerBatchAmt.PhoenixUIControl.ObjectId = 10;
            this.colDrawerBatchAmt.PhoenixUIControl.XmlTag = "DrawerBatchAmount";
            this.colDrawerBatchAmt.Title = "Teller Batch Amount";
            // 
            // colTCDDispAmt
            // 
            this.colTCDDispAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDDispAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDDispAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDDispAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDDispAmt.PhoenixUIControl.ObjectId = 11;
            this.colTCDDispAmt.PhoenixUIControl.XmlTag = "TcdDispAmount";
            this.colTCDDispAmt.Title = "TCD Batch Amount";
            // 
            // colBatchTotal
            // 
            this.colBatchTotal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colBatchTotal.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colBatchTotal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colBatchTotal.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colBatchTotal.PhoenixUIControl.ObjectId = 5;
            this.colBatchTotal.PhoenixUIControl.XmlTag = "BatchTotalAmount";
            this.colBatchTotal.Title = "Batch Total";
            this.colBatchTotal.Width = 104;
            // 
            // pbBatchChecks
            // 
            this.pbBatchChecks.ObjectId = 8;
            this.pbBatchChecks.ShortText = "Batch Checks";
            this.pbBatchChecks.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBatchChecks_Click);
            // 
            // pbBatchDetails
            // 
            this.pbBatchDetails.ObjectId = 9;
            this.pbBatchDetails.ShortText = "Batch Details";
            this.pbBatchDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBatchDetails_Click);
            // 
            // frmTlBatchTotals
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.pnl0);
            this.Name = "frmTlBatchTotals";
            this.ScreenId = 10487;
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlBatchTotals_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlBatchTotals_PInitBeginEvent);
            this.pnl0.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		public override void InitParameters(params object[] paramList)
		{
			if( paramList.Length == 4 )
			{
				branchNo = Convert.ToInt16( paramList[0] ) ;
				drawerNo = Convert.ToInt16( paramList[1] ) ;
				postingDt = Convert.ToDateTime( paramList[2]);
				emplId = Convert.ToInt16( paramList[3] ) ;
			}

            base.InitParameters(paramList); //#19058

		}

		private Phoenix.Windows.Forms.ReturnType frmTlBatchTotals_PInitBeginEvent()
		{
			pbBatchChecks.NextScreenId = Phoenix.Shared.Constants.ScreenId.BatchChecks;
			pbBatchDetails.NextScreenId = Phoenix.Shared.Constants.ScreenId.BatchDetails;
			//pbBatchDetails.NextScreenId = 0; // consistency with centura
			return new Phoenix.Windows.Forms.ReturnType();
		}

		private void frmTlBatchTotals_PInitCompleteEvent()
		{
			//EnableDisableVisibleLogic( "FormCreate" );
			MainBusinesObject = _busObjTlJrnl;
			_busObjTlJrnl.BranchNo.Value = branchNo;
			_busObjTlJrnl.DrawerNo.Value = drawerNo;
			_busObjTlJrnl.EffectiveDt.Value = postingDt;
			_busObjTlJrnl.OutputType.Value = 2;
			ctwBatchTotals.ListViewObject = _busObjTlJrnl;
			ctwBatchTotals.ObjectToScreen();
			EnableDisableVisibleLogic( "FormCreate" );
			ctwBatchTotals.DoubleClickAction = pbBatchChecks;
			

		}

		#region pbBatchDetails
		private void pbBatchDetails_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			CallOtherForms( "BatchDetails" );
		}

		#endregion
		
		#region pbBatchChecks
		private void pbBatchChecks_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			TlJournal TlJournal = new TlJournal();
			bool abortProc = true;
            //Begin #72916
            decimal totalBatchAmt = Convert.ToDecimal(colDrawerBatchAmt.UnFormattedValue);
            if (colTCDDispAmt.UnFormattedValue != null)
                totalBatchAmt = totalBatchAmt + Convert.ToDecimal(colTCDDispAmt.UnFormattedValue);


			if ( DialogResult.No == PMessageBox.Show( 313595, MessageType.Question, 
				MessageBoxButtons.YesNo, new string[2]{ Convert.ToString( colDescription.UnFormattedValue), 
														  Convert.ToString( totalBatchAmt )}))
				return;
            //End #72916

			TlJournal.BranchNo.Value = branchNo;
			TlJournal.DrawerNo.Value = drawerNo;
			TlJournal.EffectiveDt.Value = postingDt;
			TlJournal.EmplId.Value = emplId;
			TlJournal.BatchId.Value = Convert.ToInt16( colBatchID.UnFormattedValue );
			TlJournal.CashOut.Value = Convert.ToDecimal( colDrawerBatchAmt.UnFormattedValue );
			TlJournal.NetAmt.Value = Convert.ToDecimal( colBatchTotal.UnFormattedValue );
			TlJournal.ItemCount.Value = Convert.ToInt16( colTranCount.UnFormattedValue );
			TlJournal.TlTranCode.Value = "BAT";
            /* Begin #72916 */
            TlJournal.TcdCashOut.Value = Convert.ToDecimal(colTCDDispAmt.UnFormattedValue);
            /* End #72916 */

            /* Begin #72916 - Phase 3*/
            //#75736 - removed to avoid TCD machine info for tl_journal update
            //TlJournal.TcdDrawerNo.Value = _tellerVars.TcdDrawerNo;
            /* End #72916 - Phase 3*/


			try
			{
				Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( XmActionType.New, TlJournal );
				abortProc = false;
                //#75736 - instead added here after the insert to TlJournal for printing
                TlJournal.TcdDrawerNo.Value = _tellerVars.TcdDrawerNo;
                //
				if ( TellerVars.Instance.OfflineCDS != null )
				{
					TellerVars.Instance.OfflineCDS.ProcessCustomAction( TlJournal, "UpdateBalancesForTran" );
				}

                /* Begin #72916 - Phase 3 */

                bool isBatchDetailsPrinted = false;

                //HandlePrinting(TlJournal);
                if (!TlJournal.CashOut.IsNull && TlJournal.CashOut.Value > 0)
                {
                    HandlePrinting(TlJournal);
                    isBatchDetailsPrinted = true;
                }
                string printStr = string.Empty;
                if (!TlJournal.TcdCashOut.IsNull && TlJournal.TcdCashOut.Value > 0)
                    printStr = TlJournal.GetBatchTcdPrintInfo();

                if (printStr != null && printStr != string.Empty && printStr != "")
                {
                    string[] rowList = null;
                    rowList = printStr.Split('^');

                    foreach (string row in rowList)
                    {
                        HandlePrinting(TlJournal, ref isBatchDetailsPrinted, true, row);
                    }
                }
                /* End #72916 - Phase 3 */              
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show(pe);
				if ( abortProc )
					return;
			}
			finally
			{
				//#37296
                //CallParent( "UpdateUnbatchedAmtTotal" );
                //frmTlBatchTotals_PInitCompleteEvent();/*issue 74810 moved this up*/
                try
                {
                    CallParent("UpdateUnbatchedAmtTotal");
                }
                catch
                {
                    frmTlBatchTotals_PInitCompleteEvent();/*issue 74810 moved this up*/
                }
                frmTlBatchTotals_PInitCompleteEvent();/*issue 74810 moved this up*/
			}
			//frmTlBatchTotals_PInitCompleteEvent();/*issue 74810*/
		}
		#endregion

		#region ctwBatchTotals
		private void ctwBatchTotals_Click(object sender, System.EventArgs e)
		{
			EnableDisableVisibleLogic( "TableClick" );
		}

		#endregion

		private void EnableDisableVisibleLogic( string origin )
		{
			if ( origin == "TableClick" || origin == "FormCreate" )
			{
				pbBatchChecks.Enabled = ( colBatchID.UnFormattedValue != null && 
					Convert.ToDecimal( colBatchTotal.UnFormattedValue ) > 0 );
				pbBatchDetails.Enabled = ( colBatchID.UnFormattedValue != null && 
					Convert.ToDecimal( colBatchTotal.UnFormattedValue ) > 0 );
			}
            /* Begin #72916 */
            //if ( origin == "FormCreate" )
            //{
            //    colDrawerBatchAmt.Visible = false;
            //    colTCDDispAmt.Visible = false;
            //}
            if (_tellerVars.IsTCDEnabled)
            {
               // colDrawerBatchAmt.Visible = true;
                colTCDDispAmt.Visible = true;
                colDescription.Width = 307;
            }
            else
            {
              //  colDrawerBatchAmt.Visible = false;
                colTCDDispAmt.Visible = false;
                colDescription.Width = 407;
            }
            /* End #72916 */
		}

		private void CallOtherForms( string origin )
		{
			PfwStandard tempDlg = null;

			try
			{
				if ( origin == "AdHocReceipt" )
				{
                    _reprintFormId = new PSmallInt("FormId");
                    _printerService = new PString("PrinterService");
                    _noCopies = new PDecimal("NoCopies");
                    //
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting","Phoenix.Client.TlPrinting","dlgAdHocReceipt");
					//tempDlg.InitParameters( CoreService.Translation.GetUserMessageX(360743),_reprintFormId );
                    tempDlg.InitParameters(CoreService.Translation.GetUserMessageX(360743), _reprintFormId, null, null, null, _noCopies, _printerService);
       
                    dialogResult = tempDlg.ShowDialog(this);
				}
                /* Begin #72916 - Phase 3*/
                if (origin == "TcdAdHocReceipt")
                {
                    tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt");
                    
                    string strMachIDDrNo = "";

                    if (localTcdMachineID != -1)
                        strMachIDDrNo = Convert.ToString(localTcdMachineID);

                    if (strMachIDDrNo != null)
                        strMachIDDrNo = strMachIDDrNo + "/";

                    if (localTcdDrawerNo != -1)
                        strMachIDDrNo = strMachIDDrNo + localTcdDrawerNo;

                    
                    if (_tellerVars.IsTCDEnabled && Convert.ToDecimal(this.colTCDDispAmt.UnFormattedValue) > 0)
                    {
                        // 361085 - TCD Cash Out/Batch Items
                        //tempDlg.InitParameters(CoreService.Translation.GetUserMessageX(361085), _reprintFormId, null, null, strMachIDDrNo);
                        tempDlg.InitParameters(CoreService.Translation.GetUserMessageX(361085), _reprintFormId, null, null, strMachIDDrNo, _noCopies, _printerService);
       
                        //PASS DISPENSER ID + PORT ADDRESS AS ???? HOW TO GET THAT. PAGE 7 OF TECH SPEC
                    }
                    
                    dialogResult = tempDlg.ShowDialog(this);
                }
                /* End #72916 - Phase 3*/
				else if ( origin == "BatchDetails" )
				{
					frmTlBatchDetails BatchDetailsForm = new frmTlBatchDetails();
					BatchDetailsForm.Workspace = this.Workspace;
					BatchDetailsForm.InitParameters( branchNo, drawerNo, Convert.ToInt16( colBatchID.UnFormattedValue ) );
					BatchDetailsForm.Show();
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
				return;
			}
//			catch( Exception e )
//			{
//				MessageBox.Show( e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop );
//				return;
//			}
		}


        /* Begin #72916 - Phase 3*/
        //private void HandlePrinting( TlJournal tlJournal )
        //{
        //    PrintInfo printInfo;
        //    AdTlForm adTlForm;
        //    XfsPrinter xfsPrinter =  XfsPrinter.Instance; //Helper.XfsPrinter;

        //    _reprintFormId = new PSmallInt( "A1" );
        //    CallOtherForms( "AdHocReceipt" );
        //    if ( dialogResult != DialogResult.OK )
        //        return;

        //    if (!TellerVars.Instance.SetContextObject( "AdTlFormArray", _reprintFormId.Value ))
        //    {
        //        PMessageBox.Show( 360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
        //        return;
        //    }
        //    adTlForm = TellerVars.Instance.AdTlForm;
        //    try
        //    {
        //        tlJournal.Description.Value = Convert.ToString(colDescription.UnFormattedValue);
				
                
        //        printInfo = tlJournal.GetBatchPrintInfo( adTlForm.PrintString.Value );
                

        //        if ( printInfo != null )
        //            xfsPrinter.PrintForm(PApplication.Instance.MdiMain, adTlForm.LogicalService.Value, 
        //            adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo );
        //    }
        //    catch( PhoenixException pe )
        //    {
        //        PMessageBox.Show( pe, MessageBoxButtons.OK );
        //        return;
        //    }
        //}

        private void HandlePrinting(TlJournal tlJournal) 
		{
			bool isBatchDetailsPrinted = false;
			HandlePrinting( tlJournal, ref isBatchDetailsPrinted, false, null);
		}

        /* End #72916 - Phase 3 */

        /* Begin #72916 */
        private void HandlePrinting(TlJournal tlJournal, ref bool isBatchDetailsPrinted, bool isTcdBatch, string tcdBatchInfo)
        {
            PrintInfo printInfo = null;
			AdTlForm adTlForm;

            //Home#72916
            //Define TCD variables
            //Parse tcdBatchInfo if not null and assign them to the respected variable
            int fetchTcdDrawerNo = -1;
            decimal fetchTcdCashOut = -1;
            int fetchTcdDeviceNo = -1;
            string fetchTcdDeviceName = "";
            string[] colList = null;

			_reprintFormId = new PSmallInt( "A1" );
		
			if (!isTcdBatch)
				CallOtherForms( "AdHocReceipt" );
			else
			{
                if (tcdBatchInfo != null && tcdBatchInfo != "" && tcdBatchInfo != string.Empty)
                    colList = tcdBatchInfo.Split('~');

                if (colList != null && colList.Length > 0)
                {

                    if (colList.GetValue(0) != null)
                    {
                        fetchTcdDrawerNo = Convert.ToInt32(colList.GetValue(0).ToString());
                    }

                    if (colList.GetValue(1) != null)
                    {
                        fetchTcdCashOut = Convert.ToDecimal(colList.GetValue(1).ToString());
                    }

                    if (colList.GetValue(2) != null)
                    {
                        fetchTcdDeviceNo = Convert.ToInt32(colList.GetValue(2).ToString());
                    }

                    if (colList.GetValue(3) != null)
                    {
                        fetchTcdDeviceName = colList.GetValue(3).ToString();
                    }
                    //Define 2 form level variable for tcdMachineId and tcdDrawerNo
                    //Assign form level tcdDrawerNo and tcdMachineId from the local variables
                    localTcdMachineID = fetchTcdDeviceNo;
                    localTcdDrawerNo = fetchTcdDrawerNo;
                    //
                    if (isTcdBatch)
                        CallOtherForms("TcdAdHocReceipt");
                }               
			}
            //
            if (!isTcdBatch || (isTcdBatch && colList != null && colList.Length > 0))
            {
                if (dialogResult != DialogResult.OK)
                    return;

                if (!TellerVars.Instance.SetContextObject("AdTlFormArray", _reprintFormId.Value))
                {
                    PMessageBox.Show(360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                    return;
                }
                adTlForm = TellerVars.Instance.AdTlForm;
                try
                {
                    tlJournal.Description.Value = Convert.ToString(colDescription.UnFormattedValue);

                    if (!isBatchDetailsPrinted)
                    {
                        printInfo = tlJournal.GetBatchPrintInfo(adTlForm.PrintString.Value);
                        isBatchDetailsPrinted = true;
                    }
                    else
                    {
                        printInfo = new PrintInfo();
                    }

                    if (printInfo != null)
                    {
                        if (isTcdBatch)
                        {
                            //Home#72916 Assign respective tcd printinfo variables for printing
                            printInfo.TCDCashOut = fetchTcdCashOut;
                            printInfo.TCDMachineID = fetchTcdDeviceNo;
                            printInfo.TCDDrawerNo = fetchTcdDrawerNo;
                        }
                        //
                        if (!_printerService.IsNull)
                        {
                            _wosaServiceName = _printerService.Value; //WI-3475
							XfsPrinter xfsPrinter = new XfsPrinter(_wosaServiceName);  //#157637
							xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);	 //#157637
							xfsPrinter.Close();	//#157637
                        }

                    }
                }
                catch (PhoenixException pe)
                {
                    PMessageBox.Show(pe, MessageBoxButtons.OK);
                    return;
                }
            }
        }
        /* End #72916 */
	}
}
