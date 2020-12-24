#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlJournalDisplay.cs
// NameSpace: Phoenix.Client.Journal
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//5/3/2006		1		mselvaga	Created.
//6/5/2006		2		mselvaga	#68881 - Fixed teller journal display for TCs 505/555.
//12/19/2006	3		mselvaga	#71030 - Offline force forwarded transactions override logic added.
//01/17/2007	4		mselvaga	#71434 - Fixed on-us account desc for TCs 128 and 156.
//01/25/07		5		Vreddy		#71578 - 67879 - .NET Teller - Wire Transfer Confirmation Notice - Enable for TC 156 and TC 191 (Outgoing Wire Debits to Accounts) and Amount is wrong on form
//01/30/2007	6		mselvaga	#71656 - ABC, ACI, ACO, BAT, CLO, CLC transactions should not indicate a supervisor override was performed.  Overrides action should never be enabled for txns
//01/16/2008    7       LSimpson    #74011 - Added FormSource logic.
//05/04/2008    8       mselvaga    #76241 - Mintest 2008_19 - Teller Journal Display, 10444 - frmTlJournalDisplay, in Teller 2007 is missing data.
//07/22/2008    9       mselvaga    #76043 - Added loan prepay penalty charges changes added.
//07/29/2008    10      mselvaga    #74834 - Added .NET system not capturing when using sup override changes.
//09/17/2008    11      LSimpson    #76057 - Added pbChkDetails.
//12/18/2008    12      mselvaga    #1884  - Added IsAppOnline check for pbChkDetails.
//01/27/2009    13      mselvaga    #76458 - Added masked acct for ex.
//05/29/2009    14      bhughes     #4002 - Incorrect acctount type.
//02/11/2010    15      LSimpson    #79574 - UI changes for Cash Recycler.
//05/25/2010    16      rpoddar     #09100 - Fix for Transfer account
//05/28/2010    17      LSimpson    #9161 - Added rollover amount.
//06/30/2010    18      rpoddar     #79510, #09368 - Make the journal display window work in 24 x 7 mode.
//09/03/2010    19      sdhamija	#9539 - disable buttons when this window is involed from CM.
//11/03/2010    20      VDevadoss   #80631 - Added "Local Dt Time" field
//02/18/2011    21      SDhamija	#80617 - added Escrow Agent field, UI changes only.
//11/18/2011    22      mselvaga    #80660 - Added logic to handle supervisor override for warningOnlySuspect overrides.
//06/08/2012    23      Nishad      #140782 -   Added new data field 'Withholding Amount', new Push button 'CD Redemp...'
//                                              Added new Hidden fields 'dfHiddenFedWhAmt','dfHiddenStateWhAmt' and 'dfHiddenOtherWhAmt'
//                                              Added refrence to bus obj 'DpRedempCalc' for enable/disable
//12/17/2012    24      mselvaga    #140772 - Inventory Tracking changes added.
//02/04/2013    25      mselvaga    #20928 - 3212 - frmGbInventoryTfrAdd - NewTfr From Location = Branch where Drawer _no= 0-NOT WORKING.
//02/21/2013    26      mselvaga    #21249 - Fixed the inventory amount background text object id.
//22Feb2013     27      dfutcher    #20959, 20556 Calc access.
//03/01/2013    28      mselvaga    #21274 - Fixed Inventory search parameter values.
//07/16/2013    29      mselvaga    #140798 - Savings Bond Redemption History Desc changes added.
//01/17/2014    30      spatterson  #140895 - Teller Capture Integration
//01/08/2016    31      NikhilNK    #38813 - Corrected position of 2nd "Charges" label control.
//08/20/2019    32      FOyebola    Task#118228
//08/28/2019    33      FOyebola    Task#118306
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
//
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Misc;
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Deposit;
using Phoenix.BusObj.Global; //#76458
//using Phoenix.Client.TlPosition;  //118226

namespace Phoenix.Client.Journal
{
	/// <summary>
	/// Summary description for dfwTlJournalDisplay.
	/// </summary>
	public class frmTlJournalDisplay : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		TlJournal _busObjTlJournalDisp	= new TlJournal();
		TlHelper _tellerHelper = new TlHelper();
		TellerVars _tellerVars = TellerVars.Instance;
		private PSmallInt branchNo		= new PSmallInt();
		private PSmallInt drawerNo		= new PSmallInt();
		private PDateTime effectiveDt	= new PDateTime();
		private PDecimal ptid			= new PDecimal();
		private ArrayList AmtField		= new ArrayList();
		private string	tempTransactionDesc = "";
		private short tempRealTranCode  = 0;
		private string tempTfrAcctDesc  = "";
        private PInt parentScreenId = new PInt("ParentScreenId");   //#74834
        private decimal _calcPtid = decimal.MinValue;       //140782-3//20959
		Phoenix.BusObj.Misc.Eforms _eformBusObj;
		Phoenix.BusObj.Deposit.DpAcct _dpAcctBusObj;
		private AdGbAcctType _adGbAcctType;
        private DpRedempCalc _dpRedempCalc;                 //#140782
		private GbHelper _globalHelper = new GbHelper();    //#76458
		private Phoenix.Windows.Forms.PAction pbCashDetails;
        private Phoenix.Windows.Forms.PAction pbChkDetails;         //#76057
		private Phoenix.Windows.Forms.PLabelStandard lblSequence;
		private Phoenix.Windows.Forms.PDfDisplay dfSequenceNo;
		private Phoenix.Windows.Forms.PDfDisplay dfReversed;
		private Phoenix.Windows.Forms.PLabelStandard lblTransaction;
		private Phoenix.Windows.Forms.PDfDisplay dfTransaction;
		private Phoenix.Windows.Forms.PLabelStandard lblDescription;
		private Phoenix.Windows.Forms.PDfDisplay dfDescription;
		private Phoenix.Windows.Forms.PLabelStandard lblReference;
		private Phoenix.Windows.Forms.PDfDisplay dfReference;
		private Phoenix.Windows.Forms.PLabelStandard lblTranStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfTranStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblPODStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfPODStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblBatchStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfBatchStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblCTRStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfCTRStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblAccount;
		private Phoenix.Windows.Forms.PDfDisplay dfAccount;
		private Phoenix.Windows.Forms.PLabelStandard lblPassbookUpdated;
		private Phoenix.Windows.Forms.PDfDisplay dfPbUpdated;
		private Phoenix.Windows.Forms.PLabelStandard lblTfrAccount;
		private Phoenix.Windows.Forms.PDfDisplay dfTransferAccount;
		private Phoenix.Windows.Forms.PLabelStandard lblUMBCode;
		private Phoenix.Windows.Forms.PDfDisplay dfUMBCode;
		private Phoenix.Windows.Forms.PLabelStandard lblTfrPassbookUpdated;
		private Phoenix.Windows.Forms.PDfDisplay dfTfrPbUpdated;
		private Phoenix.Windows.Forms.PLabelStandard lblTfrCheck;
		private Phoenix.Windows.Forms.PDfDisplay dfTfrChkNo;
		private Phoenix.Windows.Forms.PLabelStandard lblReferenceAccount;
		private Phoenix.Windows.Forms.PDfDisplay dfRefAccount;
		private Phoenix.Windows.Forms.PLabelStandard lblUtility;
		private Phoenix.Windows.Forms.PDfDisplay dfUtility;
		private Phoenix.Windows.Forms.PLabelStandard lblCashIn;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDCashIn;
		private Phoenix.Windows.Forms.PLabelStandard lblChecksAsCash;
		private Phoenix.Windows.Forms.PLabelStandard lblOnUsChecks;
		private Phoenix.Windows.Forms.PLabelStandard lblTransitChecks;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOut;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDCashOut;
		private Phoenix.Windows.Forms.PLabelStandard lblAmount;
		private Phoenix.Windows.Forms.PLinkLabel lblCharges;
        private Phoenix.Windows.Forms.PLabelStandard lblCharges1;
		private Phoenix.Windows.Forms.PLabelStandard lblCashCount;
		private Phoenix.Windows.Forms.PDfDisplay dfCashCount;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDDrawerPosition;
		private Phoenix.Windows.Forms.PDfDisplay dfDrawerPosition;
		private Phoenix.Windows.Forms.PLabelStandard lblPayrollWithdrawal;
		private Phoenix.Windows.Forms.PDfDisplay dfPayRollWd;
		private Phoenix.Windows.Forms.PLabelStandard lblCapturedItems;
		private Phoenix.Windows.Forms.PDfDisplay dfItemCount;
		private Phoenix.Windows.Forms.PLabelStandard lblPenalty;
		private Phoenix.Windows.Forms.PLabelStandard lblCalculatedPenalty;
		private Phoenix.Windows.Forms.PLabelStandard lblInterestPaid;
		private Phoenix.Windows.Forms.PLabelStandard lblCheck;
		private Phoenix.Windows.Forms.PDfDisplay dfCheckNo;
		private Phoenix.Windows.Forms.PLabelStandard lblTeller;
		private Phoenix.Windows.Forms.PLabelStandard lblReversalEmplId;
		private Phoenix.Windows.Forms.PDfDisplay dfReversalEmplId;
		private Phoenix.Windows.Forms.PLabelStandard lblReversalDateTime;
		private Phoenix.Windows.Forms.PDfDisplay dfReversalDateTime;
		private Phoenix.Windows.Forms.PAction pbItemCapt;
		private Phoenix.Windows.Forms.PAction pbOverrides;
		private Phoenix.Windows.Forms.PDfDisplay dfCashIn;
		private Phoenix.Windows.Forms.PDfDisplay dfTcdCashIn;
		private Phoenix.Windows.Forms.PDfDisplay dfChksAsCash;
		private Phoenix.Windows.Forms.PDfDisplay dfOnUsChks;
		private Phoenix.Windows.Forms.PDfDisplay dfTransitChks;
		private Phoenix.Windows.Forms.PDfDisplay dfCashOut;
		private Phoenix.Windows.Forms.PDfDisplay dfTcdCashOut;
		private Phoenix.Windows.Forms.PDfDisplay dfAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfCcAmt;
		private Phoenix.Windows.Forms.PDfDisplay dfIntAmt;
		private Phoenix.Windows.Forms.PDfDisplay dfPenaltyAmt;
		private Phoenix.Windows.Forms.PDfDisplay dfCalcPenaltyAmt;
		private System.Windows.Forms.Label label1;
		private Phoenix.Windows.Forms.PDfDisplay dfSubSequence;
		private Phoenix.Windows.Forms.PPanel pPanel1;
		private Phoenix.Windows.Forms.PPanel pPanel2;
		private Phoenix.Windows.Forms.PPanel pPanel3;
		private Phoenix.Windows.Forms.PPanel pPanel4;
		private Phoenix.Windows.Forms.PDfDisplay dfOfflineSeqNo;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PAction pbWireTfrPrint;
        private PAction pbShBrTranDisplay; // 79510
        private PLabelStandard lblTCRReverseDeposit;
        private PLabelStandard lblTCRExpectedDeposit;
        private PDfDisplay dfTCRExpectedDeposit;
        private PDfDisplay dfTCRReverseDeposit;
        private PDfDisplay dfPostingDtTm;
        private PLabelStandard pLabelStandard2;
		private PDfDisplay dfLnEscrowAgent;
		private PLabelStandard lnEscrowAgent;
        private PDfDisplay dfWithholdingAmt;
        private PLabelStandard lblWithholdingAmt;
        private PAction pbCDRedemp;
        private PdfCurrency dfHiddenFedWhAmt;
        private PdfCurrency dfHiddenStateWhAmt;
        private PdfCurrency dfHiddenOtherWhAmt;
        private PDfDisplay dfNoInvItems;
        private PLinkLabel lblNoInvItems;
        private PDfDisplay pDfDisplay2;
        private PLabelStandard lblInvItemAmt;
        private PAction pbBondDetails;
        private PDfDisplay dfTlCaptureISN;
        private PDfDisplay dfTlCaptureOption;
        private PLabelStandard lblTlCaptureISN;
        private PLabelStandard lblTlCaptureOption;
		private Phoenix.Windows.Forms.PDfDisplay dfTellerName;

		public frmTlJournalDisplay()
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
            Phoenix.FrameWork.Core.ControlInfo controlInfo1 = new Phoenix.FrameWork.Core.ControlInfo();
            Phoenix.FrameWork.Core.ControlInfo controlInfo2 = new Phoenix.FrameWork.Core.ControlInfo();
            this.pbCashDetails = new Phoenix.Windows.Forms.PAction();
            this.pbChkDetails = new Phoenix.Windows.Forms.PAction();
            this.dfSubSequence = new Phoenix.Windows.Forms.PDfDisplay();
            this.label1 = new System.Windows.Forms.Label();
            this.dfCalcPenaltyAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfPenaltyAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfIntAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCcAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfAmount = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTcdCashOut = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCashOut = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTransitChks = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsChks = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfChksAsCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTcdCashIn = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCashIn = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfReversalDateTime = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblReversalDateTime = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReversalEmplId = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblReversalEmplId = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTellerName = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTeller = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCheckNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCheck = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblInterestPaid = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCalculatedPenalty = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblPenalty = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfItemCount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCapturedItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPayRollWd = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPayrollWithdrawal = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDrawerPosition = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCDDrawerPosition = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashCount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashCount = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCharges = new Phoenix.Windows.Forms.PLinkLabel();
            this.lblCharges1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTCDCashOut = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCashOut = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTransitChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblOnUsChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblChecksAsCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTCDCashIn = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCashIn = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfUtility = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblUtility = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRefAccount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblReferenceAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTfrChkNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTfrCheck = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTfrPbUpdated = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTfrPassbookUpdated = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfUMBCode = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblUMBCode = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTransferAccount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTfrAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPbUpdated = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPassbookUpdated = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAccount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCTRStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCTRStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfBatchStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblBatchStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPODStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPODStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTranStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTranStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReference = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblReference = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDescription = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTransaction = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTransaction = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReversed = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfSequenceNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblSequence = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbItemCapt = new Phoenix.Windows.Forms.PAction();
            this.pbOverrides = new Phoenix.Windows.Forms.PAction();
            this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
            this.dfLnEscrowAgent = new Phoenix.Windows.Forms.PDfDisplay();
            this.lnEscrowAgent = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanel2 = new Phoenix.Windows.Forms.PPanel();
            this.dfNoInvItems = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblNoInvItems = new Phoenix.Windows.Forms.PLinkLabel();
            this.pDfDisplay2 = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblInvItemAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfWithholdingAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblWithholdingAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanel3 = new Phoenix.Windows.Forms.PPanel();
            this.dfPostingDtTm = new Phoenix.Windows.Forms.PDfDisplay();
            this.pLabelStandard2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanel4 = new Phoenix.Windows.Forms.PPanel();
            this.dfTCRReverseDeposit = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTCRExpectedDeposit = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCRReverseDeposit = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTCRExpectedDeposit = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOfflineSeqNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbWireTfrPrint = new Phoenix.Windows.Forms.PAction();
            this.pbShBrTranDisplay = new Phoenix.Windows.Forms.PAction();
            this.pbCDRedemp = new Phoenix.Windows.Forms.PAction();
            this.dfHiddenFedWhAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfHiddenStateWhAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfHiddenOtherWhAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblTlCaptureOption = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTlCaptureISN = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTlCaptureOption = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTlCaptureISN = new Phoenix.Windows.Forms.PDfDisplay();
            this.pbBondDetails = new Phoenix.Windows.Forms.PAction();
            this.pPanel1.SuspendLayout();
            this.pPanel2.SuspendLayout();
            this.pPanel3.SuspendLayout();
            this.pPanel4.SuspendLayout();
            this.SuspendLayout();
            //
            // ActionManager
            //
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbCashDetails,
            this.pbItemCapt,
            this.pbOverrides,
            this.pbWireTfrPrint,
            this.pbChkDetails,
            this.pbShBrTranDisplay,
            this.pbCDRedemp,
            this.pbBondDetails});
            //
            // pbCashDetails
            //
            this.pbCashDetails.LongText = "Cash &Details";
            this.pbCashDetails.Name = "Cash &Details";
            this.pbCashDetails.ObjectId = 41;
            this.pbCashDetails.ShortText = "Cash &Details";
            this.pbCashDetails.Tag = null;
            this.pbCashDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCashDetails_Click);
            //
            // pbChkDetails
            //
            this.pbChkDetails.LongText = "Chec&k Dtls...";
            this.pbChkDetails.Name = "Chec&k Dtls...";
            this.pbChkDetails.ObjectId = 56;
            this.pbChkDetails.ShortText = "Chec&k Dtls...";
            this.pbChkDetails.Tag = null;
            this.pbChkDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbChkDetails_Click);
            //
            // dfSubSequence
            //
            this.dfSubSequence.Location = new System.Drawing.Point(320, 5);
            this.dfSubSequence.Multiline = true;
            this.dfSubSequence.Name = "dfSubSequence";
            this.dfSubSequence.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSubSequence.PhoenixUIControl.XmlTag = "SubSequence";
            this.dfSubSequence.Size = new System.Drawing.Size(12, 16);
            this.dfSubSequence.TabIndex = 76;
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(308, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "/";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // dfCalcPenaltyAmt
            //
            this.dfCalcPenaltyAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCalcPenaltyAmt.Location = new System.Drawing.Point(224, 98);
            this.dfCalcPenaltyAmt.Multiline = true;
            this.dfCalcPenaltyAmt.Name = "dfCalcPenaltyAmt";
            this.dfCalcPenaltyAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCalcPenaltyAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCalcPenaltyAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCalcPenaltyAmt.PhoenixUIControl.ObjectId = 48;
            this.dfCalcPenaltyAmt.PhoenixUIControl.XmlTag = "OrigPenaltyAmt";
            this.dfCalcPenaltyAmt.Size = new System.Drawing.Size(108, 16);
            this.dfCalcPenaltyAmt.TabIndex = 13;
            //
            // dfPenaltyAmt
            //
            this.dfPenaltyAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPenaltyAmt.Location = new System.Drawing.Point(224, 78);
            this.dfPenaltyAmt.Multiline = true;
            this.dfPenaltyAmt.Name = "dfPenaltyAmt";
            this.dfPenaltyAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPenaltyAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPenaltyAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfPenaltyAmt.PhoenixUIControl.ObjectId = 21;
            this.dfPenaltyAmt.PhoenixUIControl.XmlTag = "PenaltyAmt";
            this.dfPenaltyAmt.Size = new System.Drawing.Size(108, 16);
            this.dfPenaltyAmt.TabIndex = 9;
            //
            // dfIntAmt
            //
            this.dfIntAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfIntAmt.Location = new System.Drawing.Point(224, 82);
            this.dfIntAmt.Multiline = true;
            this.dfIntAmt.Name = "dfIntAmt";
            this.dfIntAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfIntAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfIntAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfIntAmt.PhoenixUIControl.ObjectId = 22;
            this.dfIntAmt.PhoenixUIControl.XmlTag = "IntAmt";
            this.dfIntAmt.Size = new System.Drawing.Size(108, 16);
            this.dfIntAmt.TabIndex = 9;
            //
            // dfCcAmt
            //
            this.dfCcAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCcAmt.Location = new System.Drawing.Point(224, 159);
            this.dfCcAmt.Multiline = true;
            this.dfCcAmt.Name = "dfCcAmt";
            this.dfCcAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCcAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCcAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCcAmt.PhoenixUIControl.ObjectId = 18;
            this.dfCcAmt.PhoenixUIControl.XmlTag = "CcAmt";
            this.dfCcAmt.Size = new System.Drawing.Size(108, 16);
            this.dfCcAmt.TabIndex = 17;
            //
            // dfAmount
            //
            this.dfAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfAmount.Location = new System.Drawing.Point(224, 140);
            this.dfAmount.Multiline = true;
            this.dfAmount.Name = "dfAmount";
            this.dfAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAmount.PhoenixUIControl.ObjectId = 17;
            this.dfAmount.PhoenixUIControl.XmlTag = "NetAmt";
            this.dfAmount.Size = new System.Drawing.Size(108, 16);
            this.dfAmount.TabIndex = 15;
            //
            // dfTcdCashOut
            //
            this.dfTcdCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTcdCashOut.Location = new System.Drawing.Point(224, 121);
            this.dfTcdCashOut.Multiline = true;
            this.dfTcdCashOut.Name = "dfTcdCashOut";
            this.dfTcdCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTcdCashOut.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTcdCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTcdCashOut.PhoenixUIControl.ObjectId = 38;
            this.dfTcdCashOut.PhoenixUIControl.XmlTag = "TcdCashOut";
            this.dfTcdCashOut.Size = new System.Drawing.Size(108, 16);
            this.dfTcdCashOut.TabIndex = 13;
            //
            // dfCashOut
            //
            this.dfCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOut.Location = new System.Drawing.Point(224, 101);
            this.dfCashOut.Multiline = true;
            this.dfCashOut.Name = "dfCashOut";
            this.dfCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOut.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOut.PhoenixUIControl.ObjectId = 16;
            this.dfCashOut.PhoenixUIControl.XmlTag = "CashOut";
            this.dfCashOut.Size = new System.Drawing.Size(108, 16);
            this.dfCashOut.TabIndex = 11;
            //
            // dfTransitChks
            //
            this.dfTransitChks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTransitChks.Location = new System.Drawing.Point(224, 82);
            this.dfTransitChks.Multiline = true;
            this.dfTransitChks.Name = "dfTransitChks";
            this.dfTransitChks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTransitChks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTransitChks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTransitChks.PhoenixUIControl.ObjectId = 15;
            this.dfTransitChks.PhoenixUIControl.XmlTag = "TransitChks";
            this.dfTransitChks.Size = new System.Drawing.Size(108, 16);
            this.dfTransitChks.TabIndex = 9;
            //
            // dfOnUsChks
            //
            this.dfOnUsChks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChks.Location = new System.Drawing.Point(224, 62);
            this.dfOnUsChks.Multiline = true;
            this.dfOnUsChks.Name = "dfOnUsChks";
            this.dfOnUsChks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsChks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOnUsChks.PhoenixUIControl.ObjectId = 14;
            this.dfOnUsChks.PhoenixUIControl.XmlTag = "OnUsChks";
            this.dfOnUsChks.Size = new System.Drawing.Size(108, 16);
            this.dfOnUsChks.TabIndex = 7;
            //
            // dfChksAsCash
            //
            this.dfChksAsCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksAsCash.Location = new System.Drawing.Point(224, 42);
            this.dfChksAsCash.Multiline = true;
            this.dfChksAsCash.Name = "dfChksAsCash";
            this.dfChksAsCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksAsCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfChksAsCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfChksAsCash.PhoenixUIControl.ObjectId = 13;
            this.dfChksAsCash.PhoenixUIControl.XmlTag = "ChksAsCash";
            this.dfChksAsCash.Size = new System.Drawing.Size(108, 16);
            this.dfChksAsCash.TabIndex = 5;
            //
            // dfTcdCashIn
            //
            this.dfTcdCashIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTcdCashIn.Location = new System.Drawing.Point(224, 24);
            this.dfTcdCashIn.Multiline = true;
            this.dfTcdCashIn.Name = "dfTcdCashIn";
            this.dfTcdCashIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTcdCashIn.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTcdCashIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTcdCashIn.PhoenixUIControl.ObjectId = 37;
            this.dfTcdCashIn.PhoenixUIControl.XmlTag = "TcdCashIn";
            this.dfTcdCashIn.Size = new System.Drawing.Size(108, 16);
            this.dfTcdCashIn.TabIndex = 3;
            //
            // dfCashIn
            //
            this.dfCashIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIn.Location = new System.Drawing.Point(224, 4);
            this.dfCashIn.Multiline = true;
            this.dfCashIn.Name = "dfCashIn";
            this.dfCashIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIn.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashIn.PhoenixUIControl.ObjectId = 12;
            this.dfCashIn.PhoenixUIControl.XmlTag = "CashIn";
            this.dfCashIn.Size = new System.Drawing.Size(108, 16);
            this.dfCashIn.TabIndex = 1;
            //
            // dfReversalDateTime
            //
            this.dfReversalDateTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfReversalDateTime.Location = new System.Drawing.Point(188, 62);
            this.dfReversalDateTime.Multiline = true;
            this.dfReversalDateTime.Name = "dfReversalDateTime";
            this.dfReversalDateTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfReversalDateTime.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfReversalDateTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
            this.dfReversalDateTime.PhoenixUIControl.ObjectId = 51;
            this.dfReversalDateTime.PhoenixUIControl.XmlTag = "ReversalCreateDt";
            this.dfReversalDateTime.Size = new System.Drawing.Size(144, 16);
            this.dfReversalDateTime.TabIndex = 7;
            //
            // lblReversalDateTime
            //
            this.lblReversalDateTime.AutoEllipsis = true;
            this.lblReversalDateTime.Location = new System.Drawing.Point(4, 62);
            this.lblReversalDateTime.Name = "lblReversalDateTime";
            this.lblReversalDateTime.Size = new System.Drawing.Size(128, 16);
            this.lblReversalDateTime.TabIndex = 6;
            this.lblReversalDateTime.Text = "Reversal Date/Time:";
            //
            // dfReversalEmplId
            //
            this.dfReversalEmplId.Location = new System.Drawing.Point(140, 42);
            this.dfReversalEmplId.Multiline = true;
            this.dfReversalEmplId.Name = "dfReversalEmplId";
            this.dfReversalEmplId.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfReversalEmplId.PhoenixUIControl.ObjectId = 50;
            this.dfReversalEmplId.PhoenixUIControl.XmlTag = "ReversalEmplId";
            this.dfReversalEmplId.Size = new System.Drawing.Size(192, 16);
            this.dfReversalEmplId.TabIndex = 5;
            //
            // lblReversalEmplId
            //
            this.lblReversalEmplId.AutoEllipsis = true;
            this.lblReversalEmplId.Location = new System.Drawing.Point(4, 42);
            this.lblReversalEmplId.Name = "lblReversalEmplId";
            this.lblReversalEmplId.Size = new System.Drawing.Size(128, 16);
            this.lblReversalEmplId.TabIndex = 4;
            this.lblReversalEmplId.Text = "Reversal Employee:";
            //
            // dfTellerName
            //
            this.dfTellerName.Location = new System.Drawing.Point(140, 24);
            this.dfTellerName.Multiline = true;
            this.dfTellerName.Name = "dfTellerName";
            this.dfTellerName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTellerName.PhoenixUIControl.ObjectId = 27;
            this.dfTellerName.PhoenixUIControl.XmlTag = "EmplId";
            this.dfTellerName.Size = new System.Drawing.Size(192, 16);
            this.dfTellerName.TabIndex = 3;
            //
            // lblTeller
            //
            this.lblTeller.AutoEllipsis = true;
            this.lblTeller.Location = new System.Drawing.Point(4, 24);
            this.lblTeller.Name = "lblTeller";
            this.lblTeller.PhoenixUIControl.ObjectId = 27;
            this.lblTeller.Size = new System.Drawing.Size(76, 16);
            this.lblTeller.TabIndex = 2;
            this.lblTeller.Text = "Teller:";
            //
            // dfCheckNo
            //
            this.dfCheckNo.Location = new System.Drawing.Point(224, 23);
            this.dfCheckNo.Multiline = true;
            this.dfCheckNo.Name = "dfCheckNo";
            this.dfCheckNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCheckNo.PhoenixUIControl.ObjectId = 23;
            this.dfCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.dfCheckNo.Size = new System.Drawing.Size(108, 16);
            this.dfCheckNo.TabIndex = 3;
            //
            // lblCheck
            //
            this.lblCheck.AutoEllipsis = true;
            this.lblCheck.Location = new System.Drawing.Point(4, 23);
            this.lblCheck.Name = "lblCheck";
            this.lblCheck.PhoenixUIControl.ObjectId = 23;
            this.lblCheck.Size = new System.Drawing.Size(124, 16);
            this.lblCheck.TabIndex = 2;
            this.lblCheck.Text = "Check #:";
            //
            // lblInterestPaid
            //
            this.lblInterestPaid.AutoEllipsis = true;
            this.lblInterestPaid.Location = new System.Drawing.Point(4, 82);
            this.lblInterestPaid.Name = "lblInterestPaid";
            this.lblInterestPaid.PhoenixUIControl.ObjectId = 22;
            this.lblInterestPaid.Size = new System.Drawing.Size(125, 16);
            this.lblInterestPaid.TabIndex = 8;
            this.lblInterestPaid.Text = "Interest Paid:";
            //
            // lblCalculatedPenalty
            //
            this.lblCalculatedPenalty.AutoEllipsis = true;
            this.lblCalculatedPenalty.Location = new System.Drawing.Point(4, 98);
            this.lblCalculatedPenalty.Name = "lblCalculatedPenalty";
            this.lblCalculatedPenalty.PhoenixUIControl.ObjectId = 48;
            this.lblCalculatedPenalty.Size = new System.Drawing.Size(124, 16);
            this.lblCalculatedPenalty.TabIndex = 12;
            this.lblCalculatedPenalty.Text = "Calculated Penalty:";
            //
            // lblPenalty
            //
            this.lblPenalty.AutoEllipsis = true;
            this.lblPenalty.Location = new System.Drawing.Point(4, 78);
            this.lblPenalty.Name = "lblPenalty";
            this.lblPenalty.PhoenixUIControl.ObjectId = 21;
            this.lblPenalty.Size = new System.Drawing.Size(124, 16);
            this.lblPenalty.TabIndex = 8;
            this.lblPenalty.Text = "Penalty:";
            //
            // dfItemCount
            //
            this.dfItemCount.Location = new System.Drawing.Point(252, 101);
            this.dfItemCount.Multiline = true;
            this.dfItemCount.Name = "dfItemCount";
            this.dfItemCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfItemCount.PhoenixUIControl.ObjectId = 20;
            this.dfItemCount.PhoenixUIControl.XmlTag = "ItemCount";
            this.dfItemCount.Size = new System.Drawing.Size(80, 16);
            this.dfItemCount.TabIndex = 11;
            //
            // lblCapturedItems
            //
            this.lblCapturedItems.AutoEllipsis = true;
            this.lblCapturedItems.Location = new System.Drawing.Point(4, 101);
            this.lblCapturedItems.Name = "lblCapturedItems";
            this.lblCapturedItems.PhoenixUIControl.ObjectId = 20;
            this.lblCapturedItems.Size = new System.Drawing.Size(125, 16);
            this.lblCapturedItems.TabIndex = 10;
            this.lblCapturedItems.Text = "# Captured Items:";
            //
            // dfPayRollWd
            //
            this.dfPayRollWd.Location = new System.Drawing.Point(288, 59);
            this.dfPayRollWd.Multiline = true;
            this.dfPayRollWd.Name = "dfPayRollWd";
            this.dfPayRollWd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPayRollWd.PhoenixUIControl.ObjectId = 44;
            this.dfPayRollWd.PhoenixUIControl.XmlTag = "PayRoll";
            this.dfPayRollWd.Size = new System.Drawing.Size(44, 16);
            this.dfPayRollWd.TabIndex = 7;
            //
            // lblPayrollWithdrawal
            //
            this.lblPayrollWithdrawal.AutoEllipsis = true;
            this.lblPayrollWithdrawal.Location = new System.Drawing.Point(4, 59);
            this.lblPayrollWithdrawal.Name = "lblPayrollWithdrawal";
            this.lblPayrollWithdrawal.PhoenixUIControl.ObjectId = 44;
            this.lblPayrollWithdrawal.Size = new System.Drawing.Size(124, 16);
            this.lblPayrollWithdrawal.TabIndex = 6;
            this.lblPayrollWithdrawal.Text = "Payroll Withdrawal:";
            //
            // dfDrawerPosition
            //
            this.dfDrawerPosition.Location = new System.Drawing.Point(200, 121);
            this.dfDrawerPosition.Multiline = true;
            this.dfDrawerPosition.Name = "dfDrawerPosition";
            this.dfDrawerPosition.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDrawerPosition.PhoenixUIControl.ObjectId = 40;
            this.dfDrawerPosition.PhoenixUIControl.XmlTag = "TcdDrawerPosition";
            this.dfDrawerPosition.Size = new System.Drawing.Size(132, 16);
            this.dfDrawerPosition.TabIndex = 13;
            //
            // lblTCDDrawerPosition
            //
            this.lblTCDDrawerPosition.AutoEllipsis = true;
            this.lblTCDDrawerPosition.Location = new System.Drawing.Point(4, 121);
            this.lblTCDDrawerPosition.Name = "lblTCDDrawerPosition";
            this.lblTCDDrawerPosition.PhoenixUIControl.ObjectId = 40;
            this.lblTCDDrawerPosition.Size = new System.Drawing.Size(190, 16);
            this.lblTCDDrawerPosition.TabIndex = 12;
            this.lblTCDDrawerPosition.Text = "TCD/TCR Mach ID/Station ID:";
            //
            // dfCashCount
            //
            this.dfCashCount.Location = new System.Drawing.Point(252, 4);
            this.dfCashCount.Multiline = true;
            this.dfCashCount.Name = "dfCashCount";
            this.dfCashCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashCount.PhoenixUIControl.ObjectId = 39;
            this.dfCashCount.PhoenixUIControl.XmlTag = "CashCount";
            this.dfCashCount.Size = new System.Drawing.Size(80, 16);
            this.dfCashCount.TabIndex = 1;
            //
            // lblCashCount
            //
            this.lblCashCount.AutoEllipsis = true;
            this.lblCashCount.Location = new System.Drawing.Point(4, 4);
            this.lblCashCount.Name = "lblCashCount";
            this.lblCashCount.PhoenixUIControl.ObjectId = 39;
            this.lblCashCount.Size = new System.Drawing.Size(125, 16);
            this.lblCashCount.TabIndex = 0;
            this.lblCashCount.Text = "Cash Count:";
            //
            // lblCharges
            //
            this.lblCharges.AutoEllipsis = true;
            this.lblCharges.BackColor = System.Drawing.SystemColors.Control;
            this.lblCharges.Location = new System.Drawing.Point(4, 159);
            this.lblCharges.MLInfo = controlInfo1;
            this.lblCharges.Name = "lblCharges";
            this.lblCharges.PhoenixUIControl.ObjectId = 18;
            this.lblCharges.Size = new System.Drawing.Size(108, 16);
            this.lblCharges.TabIndex = 16;
            this.lblCharges.TabStop = true;
            this.lblCharges.Text = "Charges:";
            this.lblCharges.Click += new System.EventHandler(this.lblCharges_Click);
            //
            // lblCharges1
            //
            this.lblCharges1.AutoEllipsis = true;
            this.lblCharges1.Location = new System.Drawing.Point(4, 159); /** #38813 - Corrected position **/
            this.lblCharges1.Name = "lblCharges1";
            this.lblCharges1.PhoenixUIControl.ObjectId = 18;
            this.lblCharges1.Size = new System.Drawing.Size(108, 16);
            this.lblCharges1.TabIndex = 71;
            this.lblCharges1.TabStop = true;
            this.lblCharges1.Text = "Charges:";
            //
            // lblAmount
            //
            this.lblAmount.AutoEllipsis = true;
            this.lblAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAmount.Location = new System.Drawing.Point(4, 140);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.PhoenixUIControl.ObjectId = 17;
            this.lblAmount.Size = new System.Drawing.Size(132, 16);
            this.lblAmount.TabIndex = 14;
            this.lblAmount.Text = "Amount:";
            //
            // lblTCDCashOut
            //
            this.lblTCDCashOut.AutoEllipsis = true;
            this.lblTCDCashOut.Location = new System.Drawing.Point(4, 121);
            this.lblTCDCashOut.Name = "lblTCDCashOut";
            this.lblTCDCashOut.PhoenixUIControl.ObjectId = 38;
            this.lblTCDCashOut.Size = new System.Drawing.Size(132, 16);
            this.lblTCDCashOut.TabIndex = 12;
            this.lblTCDCashOut.Text = "TCD/TCR Cash Out:";
            //
            // lblCashOut
            //
            this.lblCashOut.AutoEllipsis = true;
            this.lblCashOut.Location = new System.Drawing.Point(4, 101);
            this.lblCashOut.Name = "lblCashOut";
            this.lblCashOut.PhoenixUIControl.ObjectId = 16;
            this.lblCashOut.Size = new System.Drawing.Size(132, 16);
            this.lblCashOut.TabIndex = 10;
            this.lblCashOut.Text = "Cash Out:";
            //
            // lblTransitChecks
            //
            this.lblTransitChecks.AutoEllipsis = true;
            this.lblTransitChecks.Location = new System.Drawing.Point(4, 82);
            this.lblTransitChecks.Name = "lblTransitChecks";
            this.lblTransitChecks.PhoenixUIControl.ObjectId = 15;
            this.lblTransitChecks.Size = new System.Drawing.Size(132, 16);
            this.lblTransitChecks.TabIndex = 8;
            this.lblTransitChecks.Text = "Transit Checks:";
            //
            // lblOnUsChecks
            //
            this.lblOnUsChecks.AutoEllipsis = true;
            this.lblOnUsChecks.Location = new System.Drawing.Point(4, 62);
            this.lblOnUsChecks.Name = "lblOnUsChecks";
            this.lblOnUsChecks.PhoenixUIControl.ObjectId = 14;
            this.lblOnUsChecks.Size = new System.Drawing.Size(132, 16);
            this.lblOnUsChecks.TabIndex = 6;
            this.lblOnUsChecks.Text = "On-Us Checks:";
            //
            // lblChecksAsCash
            //
            this.lblChecksAsCash.AutoEllipsis = true;
            this.lblChecksAsCash.Location = new System.Drawing.Point(4, 42);
            this.lblChecksAsCash.Name = "lblChecksAsCash";
            this.lblChecksAsCash.PhoenixUIControl.ObjectId = 13;
            this.lblChecksAsCash.Size = new System.Drawing.Size(132, 16);
            this.lblChecksAsCash.TabIndex = 4;
            this.lblChecksAsCash.Text = "Checks As Cash:";
            //
            // lblTCDCashIn
            //
            this.lblTCDCashIn.AutoEllipsis = true;
            this.lblTCDCashIn.Location = new System.Drawing.Point(4, 24);
            this.lblTCDCashIn.Name = "lblTCDCashIn";
            this.lblTCDCashIn.PhoenixUIControl.ObjectId = 37;
            this.lblTCDCashIn.Size = new System.Drawing.Size(132, 16);
            this.lblTCDCashIn.TabIndex = 2;
            this.lblTCDCashIn.Text = "TCD/TCR Cash In:";
            //
            // lblCashIn
            //
            this.lblCashIn.AutoEllipsis = true;
            this.lblCashIn.Location = new System.Drawing.Point(4, 4);
            this.lblCashIn.Name = "lblCashIn";
            this.lblCashIn.PhoenixUIControl.ObjectId = 12;
            this.lblCashIn.Size = new System.Drawing.Size(132, 16);
            this.lblCashIn.TabIndex = 0;
            this.lblCashIn.Text = "Cash In:";
            //
            // dfUtility
            //
            this.dfUtility.Location = new System.Drawing.Point(60, 117);
            this.dfUtility.Multiline = true;
            this.dfUtility.Name = "dfUtility";
            this.dfUtility.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfUtility.PhoenixUIControl.ObjectId = 9;
            this.dfUtility.PhoenixUIControl.XmlTag = "UtilityId";
            this.dfUtility.Size = new System.Drawing.Size(272, 16);
            this.dfUtility.TabIndex = 15;
            //
            // lblUtility
            //
            this.lblUtility.AutoEllipsis = true;
            this.lblUtility.Location = new System.Drawing.Point(4, 117);
            this.lblUtility.Name = "lblUtility";
            this.lblUtility.PhoenixUIControl.ObjectId = 9;
            this.lblUtility.Size = new System.Drawing.Size(52, 16);
            this.lblUtility.TabIndex = 14;
            this.lblUtility.Text = "Utility:";
            //
            // dfRefAccount
            //
            this.dfRefAccount.Location = new System.Drawing.Point(116, 78);
            this.dfRefAccount.Multiline = true;
            this.dfRefAccount.Name = "dfRefAccount";
            this.dfRefAccount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRefAccount.PhoenixUIControl.ObjectId = 43;
            this.dfRefAccount.Size = new System.Drawing.Size(216, 16);
            this.dfRefAccount.TabIndex = 11;
            //
            // lblReferenceAccount
            //
            this.lblReferenceAccount.AutoEllipsis = true;
            this.lblReferenceAccount.Location = new System.Drawing.Point(4, 78);
            this.lblReferenceAccount.Name = "lblReferenceAccount";
            this.lblReferenceAccount.PhoenixUIControl.ObjectId = 43;
            this.lblReferenceAccount.Size = new System.Drawing.Size(108, 16);
            this.lblReferenceAccount.TabIndex = 10;
            this.lblReferenceAccount.Text = "Reference Account:";
            //
            // dfTfrChkNo
            //
            this.dfTfrChkNo.Location = new System.Drawing.Point(224, 174);
            this.dfTfrChkNo.Multiline = true;
            this.dfTfrChkNo.Name = "dfTfrChkNo";
            this.dfTfrChkNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTfrChkNo.PhoenixUIControl.ObjectId = 31;
            this.dfTfrChkNo.PhoenixUIControl.XmlTag = "TfrChkNo";
            this.dfTfrChkNo.Size = new System.Drawing.Size(108, 16);
            this.dfTfrChkNo.TabIndex = 19;
            //
            // lblTfrCheck
            //
            this.lblTfrCheck.AutoEllipsis = true;
            this.lblTfrCheck.Location = new System.Drawing.Point(4, 174);
            this.lblTfrCheck.Name = "lblTfrCheck";
            this.lblTfrCheck.PhoenixUIControl.ObjectId = 31;
            this.lblTfrCheck.Size = new System.Drawing.Size(84, 16);
            this.lblTfrCheck.TabIndex = 18;
            this.lblTfrCheck.Text = "On-Us Check #:";
            //
            // dfTfrPbUpdated
            //
            this.dfTfrPbUpdated.Location = new System.Drawing.Point(288, 192);
            this.dfTfrPbUpdated.Multiline = true;
            this.dfTfrPbUpdated.Name = "dfTfrPbUpdated";
            this.dfTfrPbUpdated.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTfrPbUpdated.PhoenixUIControl.ObjectId = 26;
            this.dfTfrPbUpdated.PhoenixUIControl.XmlTag = "TfrPbUpdated";
            this.dfTfrPbUpdated.Size = new System.Drawing.Size(44, 16);
            this.dfTfrPbUpdated.TabIndex = 21;
            //
            // lblTfrPassbookUpdated
            //
            this.lblTfrPassbookUpdated.AutoEllipsis = true;
            this.lblTfrPassbookUpdated.Location = new System.Drawing.Point(4, 192);
            this.lblTfrPassbookUpdated.Name = "lblTfrPassbookUpdated";
            this.lblTfrPassbookUpdated.PhoenixUIControl.ObjectId = 26;
            this.lblTfrPassbookUpdated.Size = new System.Drawing.Size(176, 16);
            this.lblTfrPassbookUpdated.TabIndex = 20;
            this.lblTfrPassbookUpdated.Text = "On-Us Passbook Updated:";
            //
            // dfUMBCode
            //
            this.dfUMBCode.Location = new System.Drawing.Point(72, 135);
            this.dfUMBCode.Multiline = true;
            this.dfUMBCode.Name = "dfUMBCode";
            this.dfUMBCode.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfUMBCode.PhoenixUIControl.ObjectId = 47;
            this.dfUMBCode.PhoenixUIControl.XmlTag = "UmbDescription";
            this.dfUMBCode.Size = new System.Drawing.Size(260, 16);
            this.dfUMBCode.TabIndex = 15;
            //
            // lblUMBCode
            //
            this.lblUMBCode.AutoEllipsis = true;
            this.lblUMBCode.Location = new System.Drawing.Point(4, 135);
            this.lblUMBCode.Name = "lblUMBCode";
            this.lblUMBCode.PhoenixUIControl.ObjectId = 47;
            this.lblUMBCode.Size = new System.Drawing.Size(64, 16);
            this.lblUMBCode.TabIndex = 14;
            this.lblUMBCode.Text = "UMB Code:";
            //
            // dfTransferAccount
            //
            this.dfTransferAccount.Location = new System.Drawing.Point(132, 155);
            this.dfTransferAccount.Multiline = true;
            this.dfTransferAccount.Name = "dfTransferAccount";
            this.dfTransferAccount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTransferAccount.PhoenixUIControl.ObjectId = 7;
            this.dfTransferAccount.Size = new System.Drawing.Size(200, 16);
            this.dfTransferAccount.TabIndex = 17;
            this.dfTransferAccount.TextChanged += new System.EventHandler(this.dfTransferAccount_TextChanged);
            //
            // lblTfrAccount
            //
            this.lblTfrAccount.AutoEllipsis = true;
            this.lblTfrAccount.Location = new System.Drawing.Point(4, 155);
            this.lblTfrAccount.Name = "lblTfrAccount";
            this.lblTfrAccount.PhoenixUIControl.ObjectId = 7;
            this.lblTfrAccount.Size = new System.Drawing.Size(124, 16);
            this.lblTfrAccount.TabIndex = 16;
            this.lblTfrAccount.Text = "On-Us Account:";
            //
            // dfPbUpdated
            //
            this.dfPbUpdated.Location = new System.Drawing.Point(288, 41);
            this.dfPbUpdated.Multiline = true;
            this.dfPbUpdated.Name = "dfPbUpdated";
            this.dfPbUpdated.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPbUpdated.PhoenixUIControl.ObjectId = 25;
            this.dfPbUpdated.PhoenixUIControl.XmlTag = "PbUpdated";
            this.dfPbUpdated.Size = new System.Drawing.Size(44, 16);
            this.dfPbUpdated.TabIndex = 5;
            //
            // lblPassbookUpdated
            //
            this.lblPassbookUpdated.AutoEllipsis = true;
            this.lblPassbookUpdated.Location = new System.Drawing.Point(4, 41);
            this.lblPassbookUpdated.Name = "lblPassbookUpdated";
            this.lblPassbookUpdated.PhoenixUIControl.ObjectId = 25;
            this.lblPassbookUpdated.Size = new System.Drawing.Size(124, 16);
            this.lblPassbookUpdated.TabIndex = 4;
            this.lblPassbookUpdated.Text = "Passbook Updated:";
            //
            // dfAccount
            //
            this.dfAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfAccount.Location = new System.Drawing.Point(92, 5);
            this.dfAccount.Multiline = true;
            this.dfAccount.Name = "dfAccount";
            this.dfAccount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAccount.PhoenixUIControl.ObjectId = 5;
            this.dfAccount.Size = new System.Drawing.Size(240, 16);
            this.dfAccount.TabIndex = 1;
            //
            // lblAccount
            //
            this.lblAccount.AutoEllipsis = true;
            this.lblAccount.Location = new System.Drawing.Point(4, 5);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.PhoenixUIControl.ObjectId = 5;
            this.lblAccount.Size = new System.Drawing.Size(84, 16);
            this.lblAccount.TabIndex = 0;
            this.lblAccount.Text = "Account:";
            //
            // dfCTRStatus
            //
            this.dfCTRStatus.Location = new System.Drawing.Point(120, 192);
            this.dfCTRStatus.Multiline = true;
            this.dfCTRStatus.Name = "dfCTRStatus";
            this.dfCTRStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCTRStatus.PhoenixUIControl.ObjectId = 42;
            this.dfCTRStatus.PhoenixUIControl.XmlTag = "CTRStatusDesc";
            this.dfCTRStatus.Size = new System.Drawing.Size(212, 16);
            this.dfCTRStatus.TabIndex = 23;
            //
            // lblCTRStatus
            //
            this.lblCTRStatus.AutoEllipsis = true;
            this.lblCTRStatus.Location = new System.Drawing.Point(4, 192);
            this.lblCTRStatus.Name = "lblCTRStatus";
            this.lblCTRStatus.PhoenixUIControl.ObjectId = 42;
            this.lblCTRStatus.Size = new System.Drawing.Size(112, 16);
            this.lblCTRStatus.TabIndex = 22;
            this.lblCTRStatus.Text = "CTR Status:";
            //
            // dfBatchStatus
            //
            this.dfBatchStatus.Location = new System.Drawing.Point(128, 174);
            this.dfBatchStatus.Multiline = true;
            this.dfBatchStatus.Name = "dfBatchStatus";
            this.dfBatchStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfBatchStatus.PhoenixUIControl.ObjectId = 11;
            this.dfBatchStatus.PhoenixUIControl.XmlTag = "BatchStatusDesc";
            this.dfBatchStatus.Size = new System.Drawing.Size(204, 16);
            this.dfBatchStatus.TabIndex = 21;
            //
            // lblBatchStatus
            //
            this.lblBatchStatus.AutoEllipsis = true;
            this.lblBatchStatus.Location = new System.Drawing.Point(4, 174);
            this.lblBatchStatus.Name = "lblBatchStatus";
            this.lblBatchStatus.PhoenixUIControl.ObjectId = 11;
            this.lblBatchStatus.Size = new System.Drawing.Size(120, 16);
            this.lblBatchStatus.TabIndex = 20;
            this.lblBatchStatus.Text = "Batch Status:";
            //
            // dfPODStatus
            //
            this.dfPODStatus.Location = new System.Drawing.Point(128, 155);
            this.dfPODStatus.Multiline = true;
            this.dfPODStatus.Name = "dfPODStatus";
            this.dfPODStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPODStatus.PhoenixUIControl.ObjectId = 10;
            this.dfPODStatus.PhoenixUIControl.XmlTag = "PodStatus";
            this.dfPODStatus.Size = new System.Drawing.Size(204, 16);
            this.dfPODStatus.TabIndex = 19;
            //
            // lblPODStatus
            //
            this.lblPODStatus.AutoEllipsis = true;
            this.lblPODStatus.Location = new System.Drawing.Point(4, 155);
            this.lblPODStatus.Name = "lblPODStatus";
            this.lblPODStatus.PhoenixUIControl.ObjectId = 10;
            this.lblPODStatus.Size = new System.Drawing.Size(120, 16);
            this.lblPODStatus.TabIndex = 18;
            this.lblPODStatus.Text = "POD Status:";
            //
            // dfTranStatus
            //
            this.dfTranStatus.Location = new System.Drawing.Point(132, 135);
            this.dfTranStatus.Multiline = true;
            this.dfTranStatus.Name = "dfTranStatus";
            this.dfTranStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTranStatus.PhoenixUIControl.ObjectId = 8;
            this.dfTranStatus.PhoenixUIControl.XmlTag = "TranStatus";
            this.dfTranStatus.Size = new System.Drawing.Size(200, 16);
            this.dfTranStatus.TabIndex = 17;
            //
            // lblTranStatus
            //
            this.lblTranStatus.AutoEllipsis = true;
            this.lblTranStatus.Location = new System.Drawing.Point(4, 135);
            this.lblTranStatus.Name = "lblTranStatus";
            this.lblTranStatus.PhoenixUIControl.ObjectId = 8;
            this.lblTranStatus.Size = new System.Drawing.Size(124, 16);
            this.lblTranStatus.TabIndex = 16;
            this.lblTranStatus.Text = "Tran Status:";
            //
            // dfReference
            //
            this.dfReference.Location = new System.Drawing.Point(128, 59);
            this.dfReference.Multiline = true;
            this.dfReference.Name = "dfReference";
            this.dfReference.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfReference.PhoenixUIControl.ObjectId = 30;
            this.dfReference.PhoenixUIControl.XmlTag = "Reference";
            this.dfReference.Size = new System.Drawing.Size(204, 16);
            this.dfReference.TabIndex = 9;
            //
            // lblReference
            //
            this.lblReference.AutoEllipsis = true;
            this.lblReference.Location = new System.Drawing.Point(4, 59);
            this.lblReference.Name = "lblReference";
            this.lblReference.PhoenixUIControl.ObjectId = 30;
            this.lblReference.Size = new System.Drawing.Size(68, 16);
            this.lblReference.TabIndex = 8;
            this.lblReference.Text = "Reference:";
            //
            // dfDescription
            //
            this.dfDescription.Location = new System.Drawing.Point(88, 41);
            this.dfDescription.Multiline = true;
            this.dfDescription.Name = "dfDescription";
            this.dfDescription.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDescription.PhoenixUIControl.ObjectId = 29;
            this.dfDescription.PhoenixUIControl.XmlTag = "Description";
            this.dfDescription.Size = new System.Drawing.Size(244, 16);
            this.dfDescription.TabIndex = 7;
            //
            // lblDescription
            //
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(4, 41);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 29;
            this.lblDescription.Size = new System.Drawing.Size(80, 16);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "Description:";
            //
            // dfTransaction
            //
            this.dfTransaction.Location = new System.Drawing.Point(76, 23);
            this.dfTransaction.Multiline = true;
            this.dfTransaction.Name = "dfTransaction";
            this.dfTransaction.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTransaction.PhoenixUIControl.ObjectId = 6;
            this.dfTransaction.PhoenixUIControl.XmlTag = "TlTranCode";
            this.dfTransaction.Size = new System.Drawing.Size(256, 16);
            this.dfTransaction.TabIndex = 5;
            //
            // lblTransaction
            //
            this.lblTransaction.AutoEllipsis = true;
            this.lblTransaction.Location = new System.Drawing.Point(4, 23);
            this.lblTransaction.Name = "lblTransaction";
            this.lblTransaction.PhoenixUIControl.ObjectId = 6;
            this.lblTransaction.Size = new System.Drawing.Size(68, 16);
            this.lblTransaction.TabIndex = 4;
            this.lblTransaction.Text = "Transaction:";
            //
            // dfReversed
            //
            this.dfReversed.Location = new System.Drawing.Point(148, 5);
            this.dfReversed.Multiline = true;
            this.dfReversed.Name = "dfReversed";
            this.dfReversed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfReversed.PhoenixUIControl.ObjectId = 4;
            this.dfReversed.Size = new System.Drawing.Size(76, 16);
            this.dfReversed.TabIndex = 1;
            //
            // dfSequenceNo
            //
            this.dfSequenceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfSequenceNo.Location = new System.Drawing.Point(264, 5);
            this.dfSequenceNo.Multiline = true;
            this.dfSequenceNo.Name = "dfSequenceNo";
            this.dfSequenceNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSequenceNo.PhoenixUIControl.ObjectId = 3;
            this.dfSequenceNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.dfSequenceNo.Size = new System.Drawing.Size(44, 16);
            this.dfSequenceNo.TabIndex = 2;
            //
            // lblSequence
            //
            this.lblSequence.AutoEllipsis = true;
            this.lblSequence.Location = new System.Drawing.Point(4, 5);
            this.lblSequence.Name = "lblSequence";
            this.lblSequence.PhoenixUIControl.ObjectId = 3;
            this.lblSequence.Size = new System.Drawing.Size(128, 16);
            this.lblSequence.TabIndex = 0;
            this.lblSequence.Text = "Sequence #/Sub Seq #:";
            //
            // pbItemCapt
            //
            this.pbItemCapt.LongText = "&Item Capt";
            this.pbItemCapt.Name = "&Item Capt";
            this.pbItemCapt.ObjectId = 53;
            this.pbItemCapt.ShortText = "&Item Capt";
            this.pbItemCapt.Tag = null;
            this.pbItemCapt.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbItemCapt_Click);
            //
            // pbOverrides
            //
            this.pbOverrides.LongText = "&Override(s)";
            this.pbOverrides.Name = "&Override(s)";
            this.pbOverrides.ObjectId = 52;
            this.pbOverrides.ShortText = "&Override(s)";
            this.pbOverrides.Tag = null;
            this.pbOverrides.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOverrides_Click);
            //
            // pPanel1
            //
            this.pPanel1.Controls.Add(this.dfTlCaptureISN);
            this.pPanel1.Controls.Add(this.dfTlCaptureOption);
            this.pPanel1.Controls.Add(this.lblTlCaptureISN);
            this.pPanel1.Controls.Add(this.lblTlCaptureOption);
            this.pPanel1.Controls.Add(this.dfLnEscrowAgent);
            this.pPanel1.Controls.Add(this.lnEscrowAgent);
            this.pPanel1.Controls.Add(this.lblSequence);
            this.pPanel1.Controls.Add(this.label1);
            this.pPanel1.Controls.Add(this.dfSubSequence);
            this.pPanel1.Controls.Add(this.dfUtility);
            this.pPanel1.Controls.Add(this.lblUtility);
            this.pPanel1.Controls.Add(this.dfRefAccount);
            this.pPanel1.Controls.Add(this.lblReferenceAccount);
            this.pPanel1.Controls.Add(this.dfCTRStatus);
            this.pPanel1.Controls.Add(this.lblCTRStatus);
            this.pPanel1.Controls.Add(this.dfBatchStatus);
            this.pPanel1.Controls.Add(this.lblBatchStatus);
            this.pPanel1.Controls.Add(this.dfPODStatus);
            this.pPanel1.Controls.Add(this.lblPODStatus);
            this.pPanel1.Controls.Add(this.dfTranStatus);
            this.pPanel1.Controls.Add(this.lblTranStatus);
            this.pPanel1.Controls.Add(this.dfReference);
            this.pPanel1.Controls.Add(this.lblReference);
            this.pPanel1.Controls.Add(this.dfDescription);
            this.pPanel1.Controls.Add(this.lblDescription);
            this.pPanel1.Controls.Add(this.dfTransaction);
            this.pPanel1.Controls.Add(this.lblTransaction);
            this.pPanel1.Controls.Add(this.dfReversed);
            this.pPanel1.Controls.Add(this.dfSequenceNo);
            this.pPanel1.Location = new System.Drawing.Point(4, 0);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.RaisedBorder = true;
            this.pPanel1.Size = new System.Drawing.Size(340, 244);
            this.pPanel1.TabIndex = 0;
            this.pPanel1.TabStop = true;
            //
            // dfLnEscrowAgent
            //
            this.dfLnEscrowAgent.Location = new System.Drawing.Point(88, 98);
            this.dfLnEscrowAgent.Multiline = true;
            this.dfLnEscrowAgent.Name = "dfLnEscrowAgent";
            this.dfLnEscrowAgent.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfLnEscrowAgent.PhoenixUIControl.ObjectId = 61;
            this.dfLnEscrowAgent.PhoenixUIControl.XmlTag = "Loan3rdPartyIdString";
            this.dfLnEscrowAgent.Size = new System.Drawing.Size(244, 16);
            this.dfLnEscrowAgent.TabIndex = 13;
            //
            // lnEscrowAgent
            //
            this.lnEscrowAgent.AutoEllipsis = true;
            this.lnEscrowAgent.Location = new System.Drawing.Point(4, 98);
            this.lnEscrowAgent.Name = "lnEscrowAgent";
            this.lnEscrowAgent.PhoenixUIControl.ObjectId = 61;
            this.lnEscrowAgent.Size = new System.Drawing.Size(80, 16);
            this.lnEscrowAgent.TabIndex = 12;
            this.lnEscrowAgent.Text = "escrow  agent:";
            //
            // pPanel2
            //
            this.pPanel2.Controls.Add(this.dfNoInvItems);
            this.pPanel2.Controls.Add(this.lblNoInvItems);
            this.pPanel2.Controls.Add(this.pDfDisplay2);
            this.pPanel2.Controls.Add(this.lblInvItemAmt);
            this.pPanel2.Controls.Add(this.dfWithholdingAmt);
            this.pPanel2.Controls.Add(this.lblWithholdingAmt);
            this.pPanel2.Controls.Add(this.lblPayrollWithdrawal);
            this.pPanel2.Controls.Add(this.dfPayRollWd);
            this.pPanel2.Controls.Add(this.lblPenalty);
            this.pPanel2.Controls.Add(this.lblCalculatedPenalty);
            this.pPanel2.Controls.Add(this.lblCheck);
            this.pPanel2.Controls.Add(this.dfCheckNo);
            this.pPanel2.Controls.Add(this.dfPenaltyAmt);
            this.pPanel2.Controls.Add(this.dfCalcPenaltyAmt);
            this.pPanel2.Controls.Add(this.dfTfrChkNo);
            this.pPanel2.Controls.Add(this.lblTfrCheck);
            this.pPanel2.Controls.Add(this.dfTfrPbUpdated);
            this.pPanel2.Controls.Add(this.lblTfrPassbookUpdated);
            this.pPanel2.Controls.Add(this.dfUMBCode);
            this.pPanel2.Controls.Add(this.lblUMBCode);
            this.pPanel2.Controls.Add(this.dfTransferAccount);
            this.pPanel2.Controls.Add(this.lblTfrAccount);
            this.pPanel2.Controls.Add(this.dfPbUpdated);
            this.pPanel2.Controls.Add(this.lblPassbookUpdated);
            this.pPanel2.Controls.Add(this.dfAccount);
            this.pPanel2.Controls.Add(this.lblAccount);
            this.pPanel2.Location = new System.Drawing.Point(344, 0);
            this.pPanel2.Name = "pPanel2";
            this.pPanel2.RaisedBorder = true;
            this.pPanel2.Size = new System.Drawing.Size(340, 244);
            this.pPanel2.TabIndex = 1;
            this.pPanel2.TabStop = true;
            //
            // dfNoInvItems
            //
            this.dfNoInvItems.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItems.Location = new System.Drawing.Point(232, 210);
            this.dfNoInvItems.Multiline = true;
            this.dfNoInvItems.Name = "dfNoInvItems";
            this.dfNoInvItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItems.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoInvItems.PhoenixUIControl.ObjectId = 64;
            this.dfNoInvItems.PhoenixUIControl.XmlTag = "CheckNo";
            this.dfNoInvItems.Size = new System.Drawing.Size(100, 13);
            this.dfNoInvItems.TabIndex = 23;
            //
            // lblNoInvItems
            //
            this.lblNoInvItems.AutoEllipsis = true;
            this.lblNoInvItems.BackColor = System.Drawing.SystemColors.Control;
            this.lblNoInvItems.Location = new System.Drawing.Point(4, 208);
            this.lblNoInvItems.MLInfo = controlInfo2;
            this.lblNoInvItems.Name = "lblNoInvItems";
            this.lblNoInvItems.PhoenixUIControl.ObjectId = 31;
            this.lblNoInvItems.Size = new System.Drawing.Size(132, 16);
            this.lblNoInvItems.TabIndex = 22;
            this.lblNoInvItems.Text = "# Of Inventory Items:";
            this.lblNoInvItems.Click += new System.EventHandler(this.lblNoInvItems_Click);
            //
            // pDfDisplay2
            //
            this.pDfDisplay2.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.pDfDisplay2.Location = new System.Drawing.Point(200, 227);
            this.pDfDisplay2.Multiline = true;
            this.pDfDisplay2.Name = "pDfDisplay2";
            this.pDfDisplay2.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.pDfDisplay2.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.pDfDisplay2.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.pDfDisplay2.PhoenixUIControl.ObjectId = 65;
            this.pDfDisplay2.PhoenixUIControl.XmlTag = "InvItemAmt";
            this.pDfDisplay2.Size = new System.Drawing.Size(132, 13);
            this.pDfDisplay2.TabIndex = 25;
            //
            // lblInvItemAmt
            //
            this.lblInvItemAmt.AutoEllipsis = true;
            this.lblInvItemAmt.Location = new System.Drawing.Point(4, 225);
            this.lblInvItemAmt.Name = "lblInvItemAmt";
            this.lblInvItemAmt.PhoenixUIControl.ObjectId = 65;
            this.lblInvItemAmt.Size = new System.Drawing.Size(176, 16);
            this.lblInvItemAmt.TabIndex = 24;
            this.lblInvItemAmt.Text = "Amount of Inventory Items:";
            //
            // dfWithholdingAmt
            //
            this.dfWithholdingAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfWithholdingAmt.Location = new System.Drawing.Point(224, 117);
            this.dfWithholdingAmt.Multiline = true;
            this.dfWithholdingAmt.Name = "dfWithholdingAmt";
            this.dfWithholdingAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfWithholdingAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfWithholdingAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfWithholdingAmt.PhoenixUIControl.ObjectId = 62;
            this.dfWithholdingAmt.Size = new System.Drawing.Size(108, 16);
            this.dfWithholdingAmt.TabIndex = 11;
            //
            // lblWithholdingAmt
            //
            this.lblWithholdingAmt.AutoEllipsis = true;
            this.lblWithholdingAmt.Location = new System.Drawing.Point(4, 117);
            this.lblWithholdingAmt.Name = "lblWithholdingAmt";
            this.lblWithholdingAmt.PhoenixUIControl.ObjectId = 62;
            this.lblWithholdingAmt.Size = new System.Drawing.Size(124, 16);
            this.lblWithholdingAmt.TabIndex = 10;
            this.lblWithholdingAmt.Text = "Withholding Amount:";
            //
            // pPanel3
            //
            this.pPanel3.Controls.Add(this.dfPostingDtTm);
            this.pPanel3.Controls.Add(this.pLabelStandard2);
            this.pPanel3.Controls.Add(this.dfCashIn);
            this.pPanel3.Controls.Add(this.dfTcdCashIn);
            this.pPanel3.Controls.Add(this.dfChksAsCash);
            this.pPanel3.Controls.Add(this.dfOnUsChks);
            this.pPanel3.Controls.Add(this.dfTransitChks);
            this.pPanel3.Controls.Add(this.dfCashOut);
            this.pPanel3.Controls.Add(this.dfTcdCashOut);
            this.pPanel3.Controls.Add(this.dfAmount);
            this.pPanel3.Controls.Add(this.dfCcAmt);
            this.pPanel3.Controls.Add(this.lblCharges);
            this.pPanel3.Controls.Add(this.lblCharges1);
            this.pPanel3.Controls.Add(this.lblAmount);
            this.pPanel3.Controls.Add(this.lblTCDCashOut);
            this.pPanel3.Controls.Add(this.lblCashOut);
            this.pPanel3.Controls.Add(this.lblTransitChecks);
            this.pPanel3.Controls.Add(this.lblOnUsChecks);
            this.pPanel3.Controls.Add(this.lblChecksAsCash);
            this.pPanel3.Controls.Add(this.lblTCDCashIn);
            this.pPanel3.Controls.Add(this.lblCashIn);
            this.pPanel3.Location = new System.Drawing.Point(4, 247);
            this.pPanel3.Name = "pPanel3";
            this.pPanel3.RaisedBorder = true;
            this.pPanel3.Size = new System.Drawing.Size(340, 197);
            this.pPanel3.TabIndex = 2;
            this.pPanel3.TabStop = true;
            //
            // dfPostingDtTm
            //
            this.dfPostingDtTm.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPostingDtTm.Location = new System.Drawing.Point(148, 177);
            this.dfPostingDtTm.Multiline = true;
            this.dfPostingDtTm.Name = "dfPostingDtTm";
            this.dfPostingDtTm.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPostingDtTm.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPostingDtTm.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
            this.dfPostingDtTm.PhoenixUIControl.ObjectId = 60;
            this.dfPostingDtTm.PhoenixUIControl.XmlTag = "LocalCreateDt";
            this.dfPostingDtTm.Size = new System.Drawing.Size(184, 16);
            this.dfPostingDtTm.TabIndex = 19;
            //
            // pLabelStandard2
            //
            this.pLabelStandard2.AutoEllipsis = true;
            this.pLabelStandard2.Location = new System.Drawing.Point(4, 177);
            this.pLabelStandard2.Name = "pLabelStandard2";
            this.pLabelStandard2.PhoenixUIControl.ObjectId = 60;
            this.pLabelStandard2.Size = new System.Drawing.Size(132, 16);
            this.pLabelStandard2.TabIndex = 18;
            this.pLabelStandard2.Text = "Local Date/Time:";
            //
            // pPanel4
            //
            this.pPanel4.Controls.Add(this.dfTCRReverseDeposit);
            this.pPanel4.Controls.Add(this.dfTCRExpectedDeposit);
            this.pPanel4.Controls.Add(this.lblTCRReverseDeposit);
            this.pPanel4.Controls.Add(this.lblTCRExpectedDeposit);
            this.pPanel4.Controls.Add(this.dfOfflineSeqNo);
            this.pPanel4.Controls.Add(this.pLabelStandard1);
            this.pPanel4.Controls.Add(this.lblCapturedItems);
            this.pPanel4.Controls.Add(this.dfItemCount);
            this.pPanel4.Controls.Add(this.lblInterestPaid);
            this.pPanel4.Controls.Add(this.lblTeller);
            this.pPanel4.Controls.Add(this.dfTellerName);
            this.pPanel4.Controls.Add(this.lblReversalEmplId);
            this.pPanel4.Controls.Add(this.dfReversalEmplId);
            this.pPanel4.Controls.Add(this.lblReversalDateTime);
            this.pPanel4.Controls.Add(this.dfReversalDateTime);
            this.pPanel4.Controls.Add(this.dfIntAmt);
            this.pPanel4.Controls.Add(this.dfDrawerPosition);
            this.pPanel4.Controls.Add(this.lblTCDDrawerPosition);
            this.pPanel4.Controls.Add(this.dfCashCount);
            this.pPanel4.Controls.Add(this.lblCashCount);
            this.pPanel4.Location = new System.Drawing.Point(344, 247);
            this.pPanel4.Name = "pPanel4";
            this.pPanel4.RaisedBorder = true;
            this.pPanel4.Size = new System.Drawing.Size(340, 197);
            this.pPanel4.TabIndex = 3;
            this.pPanel4.TabStop = true;
            //
            // dfTCRReverseDeposit
            //
            this.dfTCRReverseDeposit.Location = new System.Drawing.Point(288, 159);
            this.dfTCRReverseDeposit.Multiline = true;
            this.dfTCRReverseDeposit.Name = "dfTCRReverseDeposit";
            this.dfTCRReverseDeposit.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCRReverseDeposit.PhoenixUIControl.ObjectId = 59;
            this.dfTCRReverseDeposit.PhoenixUIControl.XmlTag = "TcrRevDeposit";
            this.dfTCRReverseDeposit.Size = new System.Drawing.Size(44, 16);
            this.dfTCRReverseDeposit.TabIndex = 17;
            //
            // dfTCRExpectedDeposit
            //
            this.dfTCRExpectedDeposit.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRExpectedDeposit.Location = new System.Drawing.Point(224, 140);
            this.dfTCRExpectedDeposit.Multiline = true;
            this.dfTCRExpectedDeposit.Name = "dfTCRExpectedDeposit";
            this.dfTCRExpectedDeposit.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRExpectedDeposit.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCRExpectedDeposit.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCRExpectedDeposit.PhoenixUIControl.ObjectId = 58;
            this.dfTCRExpectedDeposit.PhoenixUIControl.XmlTag = "TcrExpectedAmt";
            this.dfTCRExpectedDeposit.Size = new System.Drawing.Size(108, 16);
            this.dfTCRExpectedDeposit.TabIndex = 15;
            //
            // lblTCRReverseDeposit
            //
            this.lblTCRReverseDeposit.AutoEllipsis = true;
            this.lblTCRReverseDeposit.Location = new System.Drawing.Point(4, 159);
            this.lblTCRReverseDeposit.Name = "lblTCRReverseDeposit";
            this.lblTCRReverseDeposit.PhoenixUIControl.ObjectId = 59;
            this.lblTCRReverseDeposit.Size = new System.Drawing.Size(125, 16);
            this.lblTCRReverseDeposit.TabIndex = 16;
            this.lblTCRReverseDeposit.Text = "TCR Reverse Deposit:";
            //
            // lblTCRExpectedDeposit
            //
            this.lblTCRExpectedDeposit.AutoEllipsis = true;
            this.lblTCRExpectedDeposit.Location = new System.Drawing.Point(4, 140);
            this.lblTCRExpectedDeposit.Name = "lblTCRExpectedDeposit";
            this.lblTCRExpectedDeposit.PhoenixUIControl.ObjectId = 58;
            this.lblTCRExpectedDeposit.Size = new System.Drawing.Size(125, 16);
            this.lblTCRExpectedDeposit.TabIndex = 14;
            this.lblTCRExpectedDeposit.Text = "TCR Expected Deposit:";
            //
            // dfOfflineSeqNo
            //
            this.dfOfflineSeqNo.Location = new System.Drawing.Point(136, 177);
            this.dfOfflineSeqNo.Multiline = true;
            this.dfOfflineSeqNo.Name = "dfOfflineSeqNo";
            this.dfOfflineSeqNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOfflineSeqNo.PhoenixUIControl.ObjectId = 40;
            this.dfOfflineSeqNo.PhoenixUIControl.XmlTag = "OfflineSequenceNo";
            this.dfOfflineSeqNo.Size = new System.Drawing.Size(196, 16);
            this.dfOfflineSeqNo.TabIndex = 19;
            //
            // pLabelStandard1
            //
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(4, 177);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 54;
            this.pLabelStandard1.Size = new System.Drawing.Size(125, 16);
            this.pLabelStandard1.TabIndex = 18;
            this.pLabelStandard1.Text = "Offline Sequence#";
            //
            // pbWireTfrPrint
            //
            this.pbWireTfrPrint.LongText = "&Print WireTfr";
            this.pbWireTfrPrint.Name = "&Print WireTfr";
            this.pbWireTfrPrint.ObjectId = 55;
            this.pbWireTfrPrint.ShortText = "&Print WireTfr";
            this.pbWireTfrPrint.Tag = null;
            this.pbWireTfrPrint.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbWireTfrPrint_Click);
            //
            // pbShBrTranDisplay
            //
            this.pbShBrTranDisplay.LongText = "&Shared Branch...";
            this.pbShBrTranDisplay.Name = "&Shared Branch...";
            this.pbShBrTranDisplay.ObjectId = 57;
            this.pbShBrTranDisplay.ShortText = "&Shared Branch...";
            this.pbShBrTranDisplay.Tag = null;
            this.pbShBrTranDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbShBrTranDisplay_Click);
            //
            // pbCDRedemp
            //
            this.pbCDRedemp.LongText = "CD &Redemp...";
            this.pbCDRedemp.Name = "CD &Redemp...";
            this.pbCDRedemp.NextScreenId = 3116;
            this.pbCDRedemp.ObjectId = 63;
            this.pbCDRedemp.ShortText = "CD &Redemp...";
            this.pbCDRedemp.Tag = null;
            this.pbCDRedemp.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCDRedemp_Click);
            //
            // dfHiddenFedWhAmt
            //
            this.dfHiddenFedWhAmt.Location = new System.Drawing.Point(24, 456);
            this.dfHiddenFedWhAmt.Name = "dfHiddenFedWhAmt";
            this.dfHiddenFedWhAmt.PhoenixUIControl.XmlTag = "FedWhAmt";
            this.dfHiddenFedWhAmt.Size = new System.Drawing.Size(140, 20);
            this.dfHiddenFedWhAmt.TabIndex = 4;
            this.dfHiddenFedWhAmt.Visible = false;
            //
            // dfHiddenStateWhAmt
            //
            this.dfHiddenStateWhAmt.Location = new System.Drawing.Point(180, 456);
            this.dfHiddenStateWhAmt.Name = "dfHiddenStateWhAmt";
            this.dfHiddenStateWhAmt.PhoenixUIControl.XmlTag = "StateWhAmt";
            this.dfHiddenStateWhAmt.Size = new System.Drawing.Size(132, 20);
            this.dfHiddenStateWhAmt.TabIndex = 5;
            this.dfHiddenStateWhAmt.Visible = false;
            //
            // dfHiddenOtherWhAmt
            //
            this.dfHiddenOtherWhAmt.Location = new System.Drawing.Point(324, 456);
            this.dfHiddenOtherWhAmt.Name = "dfHiddenOtherWhAmt";
            this.dfHiddenOtherWhAmt.PhoenixUIControl.XmlTag = "OtherWhAmt";
            this.dfHiddenOtherWhAmt.Size = new System.Drawing.Size(128, 20);
            this.dfHiddenOtherWhAmt.TabIndex = 6;
            this.dfHiddenOtherWhAmt.Visible = false;
            //
            // lblTlCaptureOption
            //
            this.lblTlCaptureOption.AutoEllipsis = true;
            this.lblTlCaptureOption.Location = new System.Drawing.Point(4, 208);
            this.lblTlCaptureOption.Name = "lblTlCaptureOption";
            this.lblTlCaptureOption.PhoenixUIControl.ObjectId = 67;
            this.lblTlCaptureOption.Size = new System.Drawing.Size(132, 16);
            this.lblTlCaptureOption.TabIndex = 77;
            this.lblTlCaptureOption.Text = "Teller Capture Option:";
            //
            // lblTlCaptureISN
            //
            this.lblTlCaptureISN.AutoEllipsis = true;
            this.lblTlCaptureISN.Location = new System.Drawing.Point(4, 224);
            this.lblTlCaptureISN.Name = "lblTlCaptureISN";
            this.lblTlCaptureISN.PhoenixUIControl.ObjectId = 68;
            this.lblTlCaptureISN.Size = new System.Drawing.Size(112, 16);
            this.lblTlCaptureISN.TabIndex = 78;
            this.lblTlCaptureISN.Text = "Teller Capture ISN #:";
            //
            // dfTlCaptureOption
            //
            this.dfTlCaptureOption.Location = new System.Drawing.Point(132, 208);
            this.dfTlCaptureOption.Multiline = true;
            this.dfTlCaptureOption.Name = "dfTlCaptureOption";
            this.dfTlCaptureOption.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTlCaptureOption.PhoenixUIControl.ObjectId = 67;
            this.dfTlCaptureOption.PhoenixUIControl.XmlTag = "";
            this.dfTlCaptureOption.Size = new System.Drawing.Size(200, 16);
            this.dfTlCaptureOption.TabIndex = 79;
            //
            // dfTlCaptureISN
            //
            this.dfTlCaptureISN.Location = new System.Drawing.Point(132, 224);
            this.dfTlCaptureISN.Multiline = true;
            this.dfTlCaptureISN.Name = "dfTlCaptureISN";
            this.dfTlCaptureISN.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTlCaptureISN.PhoenixUIControl.ObjectId = 68;
            this.dfTlCaptureISN.PhoenixUIControl.XmlTag = "CTRStatusDesc";
            this.dfTlCaptureISN.Size = new System.Drawing.Size(200, 16);
            this.dfTlCaptureISN.TabIndex = 80;
            //
            // pbBondDetails
            //
            this.pbBondDetails.LongText = "B&ond Details...";
            this.pbBondDetails.Name = "pbBondDetails";
            this.pbBondDetails.ObjectId = 66;
            this.pbBondDetails.Tag = null;
            this.pbBondDetails.Click += new PActionEventHandler(pbBondDetails_Click);
            //
            // frmTlJournalDisplay
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.dfHiddenOtherWhAmt);
            this.Controls.Add(this.dfHiddenStateWhAmt);
            this.Controls.Add(this.dfHiddenFedWhAmt);
            this.Controls.Add(this.pPanel4);
            this.Controls.Add(this.pPanel3);
            this.Controls.Add(this.pPanel2);
            this.Controls.Add(this.pPanel1);
            this.EditRecordTitle = "Teller Journal Display";
            this.Name = "frmTlJournalDisplay";
            this.NewRecordTitle = "Teller Journal Display";
            this.ScreenId = 10444;
            this.Text = "Teller Journal Display";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlJournalDisplay_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlJournalDisplay_PInitCompleteEvent);
            this.Load += new System.EventHandler(this.frmTlJournalDisplay_Load);
            this.pPanel1.ResumeLayout(false);
            this.pPanel1.PerformLayout();
            this.pPanel2.ResumeLayout(false);
            this.pPanel2.PerformLayout();
            this.pPanel3.ResumeLayout(false);
            this.pPanel3.PerformLayout();
            this.pPanel4.ResumeLayout(false);
            this.pPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #region #76057
        void pbChkDetails_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("CheckDetails");
        }
        #endregion
        #endregion

        #region Init param
        public override void InitParameters(params object[] paramList)
		{
            if (paramList.Length >= 4) // #74834 - Added 5th parameter to pass in history window screen id
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				effectiveDt.Value = Convert.ToDateTime(paramList[2]);
				ptid.Value = Convert.ToDecimal(paramList[3]);

				_busObjTlJournalDisp.BranchNo.Value = branchNo.Value;
				_busObjTlJournalDisp.DrawerNo.Value = drawerNo.Value;
				_busObjTlJournalDisp.EffectiveDt.Value = effectiveDt.Value;
				_busObjTlJournalDisp.Ptid.Value = ptid.Value;
				_busObjTlJournalDisp.OutputType.Value = 4;
                if (paramList.Length > 4)  // #74834
                {
                    parentScreenId.Value = Convert.ToInt32(paramList[4]);
                }
                this._busObjTlJournalDisp.EnumType = XmEnumerationType.EnumFKValues; //#76241
                #region #74011
                DataService.Instance.ProcessRequest(XmActionType.Select, _busObjTlJournalDisp);
                if (_busObjTlJournalDisp != null && _busObjTlJournalDisp.AcctType.Value != null)
                {
                    switch (_busObjTlJournalDisp.AcctType.Value.ToString())
                    {
                        case "CDA":
                        case "CDK":
                        case "CKA":
                        case "DDA":
                        case "RSV":
                        case "SAV":
                            this.FormSource = "DP";
                            break;
                    }
                }
                #endregion

			}
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.JournalDisplay;
			this.CurrencyId = _tellerVars.LocalCrncyId;
			base.InitParameters (paramList);
		}

		#endregion

		#region Journal Disp Events
		private ReturnType frmTlJournalDisplay_PInitBeginEvent()
		{
			try
			{
				this.AppToolBarId = AppToolBarType.NoEdit;
				this.MainBusinesObject = _busObjTlJournalDisp;
				this.CurrencyId = _tellerVars.LocalCrncyId;
                this.AutoFetch = false; // #74011
                this.pbBondDetails.NextScreenId = Phoenix.Shared.Constants.ScreenId.RedemptionHistory; //#140798
				this.dfSubSequence.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			}
			catch (PhoenixException pebegin)
			{
				PMessageBox.Show(pebegin);
			}

			return ReturnType.Success;
		}

        private void frmTlJournalDisplay_PInitCompleteEvent()
        {
            #region Get FK Value
            this.dfTranStatus.Text = GetFKDesc(_busObjTlJournalDisp.TranStatus.Value, _busObjTlJournalDisp.TranStatus.FKValue);
            this.dfUtility.Text = GetFKDesc(_busObjTlJournalDisp.UtilityId.Value, _busObjTlJournalDisp.UtilityId.FKValue);
            this.dfPODStatus.Text = GetFKDesc(_busObjTlJournalDisp.PodStatus.Value, _busObjTlJournalDisp.PodStatus.FKValue);
            this.dfReversalEmplId.Text = GetFKDesc(_busObjTlJournalDisp.ReversalEmplId.Value, _busObjTlJournalDisp.ReversalEmplId.FKValue);
            this.dfTellerName.Text = GetFKDesc(_busObjTlJournalDisp.EmplId.Value, _busObjTlJournalDisp.EmplId.FKValue);
            if (!_tellerHelper.IsGenericTran(_busObjTlJournalDisp.TlTranCode.Value) && !_tellerHelper.IsMiscTran(_busObjTlJournalDisp.TlTranCode.Value))
                tempTransactionDesc = GetFKDesc(0, _busObjTlJournalDisp.TlTranCode.FKValue);
            #endregion

            if (this._busObjTlJournalDisp.SubSequence.IsNull)
                this.dfSubSequence.Text = Convert.ToString(0);

            if (tempTransactionDesc != "")
                this.dfTransaction.Text = this._busObjTlJournalDisp.TlTranCode.Value + " - " + tempTransactionDesc;
            else
                this.dfTransaction.Text = this._busObjTlJournalDisp.TlTranCode.Value;

            if (_tellerHelper.IsGenericTran(_busObjTlJournalDisp.TlTranCode.Value))
                this.dfTransaction.Text = this._busObjTlJournalDisp.TlTranCode.Value + " - " + this._busObjTlJournalDisp.Description.Value;

            #region Info from client bus obj
            //			if (!_busObjTlJournalDisp.AcctType.IsNull && (!_busObjTlJournalDisp.AcctNo.IsNull || !_busObjTlJournalDisp.RimNo.IsNull))
            this.dfAccount.Text = _busObjTlJournalDisp.GetAccountDesc((_busObjTlJournalDisp.AcctType.IsNull ? string.Empty : _busObjTlJournalDisp.AcctType.Value),
                (_busObjTlJournalDisp.AcctNo.IsNull ? string.Empty : _busObjTlJournalDisp.AcctNo.Value),
                (_busObjTlJournalDisp.RimNo.IsNull ? 0 : _busObjTlJournalDisp.RimNo.Value));

            if (_tellerHelper.IsMiscTran(_busObjTlJournalDisp.TlTranCode.Value))
            {
                this._busObjTlJournalDisp.GetTellerTranCodeDesc(this._busObjTlJournalDisp.TlTranCode.Value, out tempTransactionDesc, out tempRealTranCode);
                this.dfTransaction.Text = tempTransactionDesc;
            }

            if ((!_busObjTlJournalDisp.TfrAcctType.IsNull && !_busObjTlJournalDisp.TfrAcctNo.IsNull) ||
                (_busObjTlJournalDisp.SharedBranch.Value == GlobalVars.Instance.ML.Y && !_busObjTlJournalDisp.TfrAcctNo.IsNull))  // #09100
            {
                //#71434
                if (_busObjTlJournalDisp.TranCode.Value == 128 || _busObjTlJournalDisp.TranCode.Value == 156) //wire transfer
                    this.dfTransferAccount.Text = _busObjTlJournalDisp.GetAccountDesc(_busObjTlJournalDisp.TfrAcctType.Value, _busObjTlJournalDisp.TfrAcctNo.Value);
                else
                {
                    tempTfrAcctDesc = _busObjTlJournalDisp.GetTfrAccountDesc(_busObjTlJournalDisp.TfrAcctType.Value, _busObjTlJournalDisp.TfrAcctNo.Value, _busObjTlJournalDisp.TranCode.Value);
                    //Begin #09100
                    if (_busObjTlJournalDisp.SharedBranch.Value == GlobalVars.Instance.ML.Y && string.IsNullOrEmpty(tempTfrAcctDesc))
                        tempTfrAcctDesc = _busObjTlJournalDisp.TfrAcctNo.Value;
                    //End #09100

                    this.dfTransaction.Text = this.dfTransaction.Text.Trim() + " " + tempTfrAcctDesc;
                    this.dfTransferAccount.Text = tempTfrAcctDesc;
                }
            }

            this.dfTfrPbUpdated.Text = _busObjTlJournalDisp.GetMLDecodeForYesNo(_busObjTlJournalDisp.TfrPbUpdated.Value);
            this.dfPbUpdated.Text = _busObjTlJournalDisp.GetMLDecodeForYesNo((_busObjTlJournalDisp.PbUpdated.IsNull ? string.Empty : _busObjTlJournalDisp.PbUpdated.Value));
            this.dfCashCount.Text = _busObjTlJournalDisp.GetMLDecodeForYesNo((_busObjTlJournalDisp.CashCount.IsNull ? string.Empty : _busObjTlJournalDisp.CashCount.Value));
            this.dfPayRollWd.Text = _busObjTlJournalDisp.GetMLDecodeForYesNo((_busObjTlJournalDisp.PayrollWd.IsNull ? string.Empty : _busObjTlJournalDisp.PayrollWd.Value));
            this.dfTCRReverseDeposit.Text = _busObjTlJournalDisp.GetMLDecodeForYesNo((_busObjTlJournalDisp.TcrRevDeposit.IsNull ? string.Empty : _busObjTlJournalDisp.TcrRevDeposit.Value));    //#79574
            #endregion

            if (!_busObjTlJournalDisp.ReferenceAcctType.IsNull && !_busObjTlJournalDisp.ReferenceAcctNo.IsNull)
                this.dfRefAccount.Text = _busObjTlJournalDisp.ReferenceAcctType.Value + " - " + _busObjTlJournalDisp.ReferenceAcctNo.Value;

            if (!this._busObjTlJournalDisp.UmbCode.IsNull && !this._busObjTlJournalDisp.IrsValue.IsNull && !this._busObjTlJournalDisp.UmbDescription.IsNull)
                this.dfUMBCode.Text = this._busObjTlJournalDisp.IrsValue.Value + " - " + this._busObjTlJournalDisp.UmbDescription.Value;
            else
                this.dfUMBCode.Text = "";

            if (_busObjTlJournalDisp.Reversal.Value == 2)
                dfReversed.Text = CoreService.Translation.GetUserMessageX(360283);
            this.lblCheck.Text = _busObjTlJournalDisp.GetCheckFieldLableText();
            //
            //#76043
            if ((this._busObjTlJournalDisp.TranCode.Value == 300 || this._busObjTlJournalDisp.TranCode.Value == 301 ||
                this._busObjTlJournalDisp.TranCode.Value == 304 || this._busObjTlJournalDisp.TranCode.Value == 345) &&
                !this._busObjTlJournalDisp.CcAmt.IsNull && this._busObjTlJournalDisp.CcAmt.Value == 0)
                this.lblCharges.Text = CoreService.Translation.GetUserMessageX(361201); //Loan Charges:
            else
                this.lblCharges.Text = CoreService.Translation.GetUserMessageX(361202); //Charges:

            //Begin #76458
            if (this._busObjTlJournalDisp.TranCode.Value == 520 ||
                _busObjTlJournalDisp.TranCode.Value == 570)
            {
                this.dfAccount.Text = _busObjTlJournalDisp.AcctType.Value + "-" + _globalHelper.GetMaskedExtAcct(_busObjTlJournalDisp.AcctType.Value, _busObjTlJournalDisp.AcctNo.Value);
            }
            else if (!_busObjTlJournalDisp.TfrAcctType.IsNull && !_busObjTlJournalDisp.TfrAcctNo.IsNull)
            {
                /* 4002 Fixed, TfrAcctType */
                if (_globalHelper.IsExternalAdapterAcct(_busObjTlJournalDisp.TfrAcctType.Value))
                    this.dfTransferAccount.Text = _busObjTlJournalDisp.TfrAcctType.Value + "-" + _globalHelper.GetMaskedExtAcct(_busObjTlJournalDisp.TfrAcctType.Value, _busObjTlJournalDisp.TfrAcctNo.Value);
            }
            //End #76458

            //Begin #140782, #20959

            //dfWithholdingAmt.Text = (Convert.ToDecimal(dfHiddenFedWhAmt.UnFormattedValue == null ? "0" : dfHiddenFedWhAmt.UnFormattedValue)
            //    + Convert.ToDecimal(dfHiddenStateWhAmt.UnFormattedValue == null ? "0" : dfHiddenStateWhAmt.UnFormattedValue)
            //    + Convert.ToDecimal(dfHiddenOtherWhAmt.UnFormattedValue == null ? "0" : dfHiddenOtherWhAmt.UnFormattedValue)).ToString("C");
            decimal fedWh = (_busObjTlJournalDisp.FedWhAmt.IsNull) ? 0 : _busObjTlJournalDisp.FedWhAmt.Value;
            decimal stateWh = (_busObjTlJournalDisp.StateWhAmt.IsNull) ? 0 : _busObjTlJournalDisp.StateWhAmt.Value;
            decimal otherWh = (_busObjTlJournalDisp.OtherWhAmt.IsNull) ? 0 : _busObjTlJournalDisp.OtherWhAmt.Value;
            dfWithholdingAmt.Text = (fedWh + stateWh + otherWh).ToString("C");

            short tranCode = _busObjTlJournalDisp.TranCode.Value;
            if (tranCode == 176 || tranCode == 179 || tranCode == 187)
            {
                _dpRedempCalc = new DpRedempCalc();
                _calcPtid = _dpRedempCalc.GetCalcPtid("XpTranLog", _busObjTlJournalDisp.XpPtid.Value);
            }
            // 3310 = Phoenix.Shared.Constants.ScreenId.DpRedempCalc_Journal
            if (_calcPtid > 0
                && CoreService.UIAccessProvider.HasReadAcces(3310)) //20556
            {
                pbCDRedemp.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
            }
            else
            {
                pbCDRedemp.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
            }
            //End   #140782

            //#140798
            if (this._busObjTlJournalDisp.TranCode.Value != 938 &&
                _busObjTlJournalDisp.TranCode.Value != 939)
                dfNoInvItems.UnFormattedValue = 0;

            // Begin #140895 Teller Capture Integration
            if (!_busObjTlJournalDisp.TlCaptureISN.IsNull)
            {
                dfTlCaptureISN.Text = _busObjTlJournalDisp.TlCaptureISN.Value;
                dfTlCaptureOption.Text = _busObjTlJournalDisp.TlCaptureOptionType.Value == "S" ?
                    "Single Related" : "Bulk Unrelated";
            }
            // End #140895 Teller Capture Integration

            EnableDisableVisibleLogic("FormComplete");
        }

		private void lblCharges_Click(object sender, EventArgs e)
		{
			if (!this._busObjTlJournalDisp.CcAmt.IsNull)
				CallOtherForms("Charges");
		}

        void lblNoInvItems_Click(object sender, EventArgs e)    //#140772 - Part II
        {
            if (((CoreService.UIAccessProvider.GetScreenAccess(Phoenix.Shared.Constants.ScreenId.InventoryItemSearchTeller) & AuthorizationType.Read) == AuthorizationType.Read &&
                                            _tellerVars.IsInventoryTrackingEnabled))  //#140772
                CallOtherForms("InventoryItemSearch");
        }

		private void pbItemCapt_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("ItemCapture");
		}

		private void pbOverrides_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("Override");
		}

		private void pbCashDetails_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("CashDenomDetails");
		}

        void pbBondDetails_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("SavingsBond");
        }
		#endregion

		#region private methods
		private string GetFKDesc( int fieldCodeValue, string fieldFKValue)
		{
			string desc = "";

			if (_tellerHelper.GetFKValueDesc(fieldCodeValue, fieldFKValue) != "")
				desc = _tellerHelper.GetFKValueDesc(fieldCodeValue, fieldFKValue);

			return desc;
		}

		private void LoadAmountField()
		{
			if (AmtField.Count == 0)
			{
				AmtField.Add(dfCashIn);
				AmtField.Add(dfCashOut);
				AmtField.Add(dfChksAsCash);
				AmtField.Add(dfOnUsChks);
				AmtField.Add(dfTransitChks);
				AmtField.Add(dfPenaltyAmt);
				AmtField.Add(dfCcAmt);
				AmtField.Add(dfIntAmt);
				AmtField.Add(dfAmount);
				AmtField.Add(dfCalcPenaltyAmt);
				AmtField.Add(dfTcdCashIn);
				AmtField.Add(dfTcdCashOut);
			}

			foreach( PDfDisplay amtField in AmtField)
			{
				amtField.ObjectToScreen();
				if ( amtField.UnFormattedValue != null && ( amtField.UnFormattedValue.ToString() == String.Empty ) )
				{
					amtField.UnFormattedValue = null;
				}
			}
		}

		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;
			if (origin == "Charges")
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tlcaptureditems", "Phoenix.Client.TlCapturedItems", "frmTlCharges" );
				tempWin.InitParameters( _busObjTlJournalDisp.Ptid.Value, null );
			}
			else if (origin == "ItemCapture")
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tlcaptureditems", "Phoenix.Client.TlCapturedItems", "frmTlCapturedItems" );
				tempWin.InitParameters( true, _busObjTlJournalDisp.BranchNo.Value,
					_busObjTlJournalDisp.DrawerNo.Value, _busObjTlJournalDisp.SequenceNo.Value,
					_busObjTlJournalDisp.EffectiveDt.Value, _busObjTlJournalDisp.SubSequence.Value,
					null,
					_busObjTlJournalDisp.Ptid.Value);
			}
			else if (origin == "Override")
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tloverride", "Phoenix.Windows.TlOverride", "dlgTlSupervisorOverride" );
                tempWin.InitParameters("JournalView", _busObjTlJournalDisp.Ptid.Value, _busObjTlJournalDisp.TlTranCode.Value, _busObjTlJournalDisp.EffectiveDt.Value);  //#13993
            }
            else if (origin == "CheckDetails")      // #76057
            {
                tempWin = Helper.CreateWindow("phoenix.client.chkdetails", "phoenix.client.TellerChkDetails", "frmTlChkDetailsList");
                tempWin.InitParameters(_busObjTlJournalDisp.Ptid.Value);
            }
            else if (origin == "CashDenomDetails")
			{

                //Begin 118228
                if (_busObjTlJournalDisp.TlTranCode.Value == "CLO" || _busObjTlJournalDisp.TlTranCode.Value == "MDC")       
                {
                    //*** Retrieve the PTID & Ending Cash from tl_position ***
                    BusObjectBase _tlPosition = BusObjHelper.MakeClientObject("TL_POSITION");

                    _tlPosition.SelectAllFields = true;

                    //Begin 118306
                    FieldBase fieldJournalPtid = _tlPosition.GetFieldByXmlTag("JournalPtid");

                    FieldBase fieldPtid = _tlPosition.GetFieldByXmlTag("Ptid");

                    if (fieldJournalPtid != null)
                    {
                        fieldJournalPtid.Selected = true;

                        fieldPtid.Selected = true;

                        if (_busObjTlJournalDisp.MemoPtid != null && _busObjTlJournalDisp.MemoPtid.Value > 0)
                        {
                            fieldPtid.Value = _busObjTlJournalDisp.MemoPtid.Value;
                        }
                        else
                        {
                            fieldJournalPtid.Value = ptid.Value;
                        }   
                    }
                    //End 118306

                    DataService.Instance.ProcessRequest(XmActionType.Select, _tlPosition);

                    if (_tlPosition.GetFieldByXmlTag("Ptid") != null)
                    {
                        FieldBase TlPosPTID = _tlPosition.GetFieldByXmlTag("Ptid");
                        FieldBase TlPosEndingCash = _tlPosition.GetFieldByXmlTag("EndingCash");                      

                        tempWin = Helper.CreateWindow("phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "frmTranDenomDetails");

                        tempWin.InitParameters(Convert.ToDecimal(TlPosPTID.Value), null, null, null, null, null, null, null,
                                               Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals,
                                               Convert.ToDecimal(TlPosEndingCash.Value));
                    }
                }
                else
                {
                    tempWin = Helper.CreateWindow("phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "frmTranDenomDetails");
                    tempWin.InitParameters(_busObjTlJournalDisp.Ptid.Value,
                        _busObjTlJournalDisp.CashIn.Value,
                        _busObjTlJournalDisp.CashOut.Value,
                        _busObjTlJournalDisp.TcdCashIn.Value,
                        _busObjTlJournalDisp.TcdCashOut.Value,
                        _busObjTlJournalDisp.NetAmt.Value,
                        _busObjTlJournalDisp.BatchId.Value,
                        _busObjTlJournalDisp.SubSequence.Value,
                        this.ScreenId,
                        null,
                        _busObjTlJournalDisp.TlTranCode.Value); // #9161 Pass TlTranCode to detect rollover (RLO)
                }
                //End 118228
            }
            else if (origin == "SharedBranch")  //Begin #79510
            {
                tempWin = new frmShBranchTranDisplay();
                tempWin.InitParameters(_busObjTlJournalDisp.Ptid.Value, _busObjTlJournalDisp.BranchNo.Value, _busObjTlJournalDisp.DrawerNo.Value,
                    _busObjTlJournalDisp.SequenceNo.Value, _busObjTlJournalDisp.SubSequence.Value, _busObjTlJournalDisp.EffectiveDt.Value,
                    _busObjTlJournalDisp.RecordSource.Value);
            }
            else if (origin == "InventoryItemSearch")   //#140772
            {
                if (TellerVars.Instance.IsAppOnline)
                {
                    tempWin = CreateWindow("phoenix.client.gbinventoryitem", "Phoenix.Client.Global", "frmInventoryItemSearch");
                    tempWin.InitParameters(true, -1,
                        ((!_busObjTlJournalDisp.TranCode.IsNull && _busObjTlJournalDisp.TranCode.Value == 938 &&
                        _busObjTlJournalDisp.Reversal.Value != 2) ? _busObjTlJournalDisp.RimNo.Value : -1),
                        _busObjTlJournalDisp.TypeId,
                        _busObjTlJournalDisp.BranchNo,
                        _busObjTlJournalDisp.DrawerNo,
                        _busObjTlJournalDisp.TranCode,
                        _busObjTlJournalDisp.Ptid,
                        ((!_busObjTlJournalDisp.TranCode.IsNull && _busObjTlJournalDisp.TranCode.Value == 938 &&
                        _busObjTlJournalDisp.Reversal.Value != 2) ? _busObjTlJournalDisp.EmplId.Value : -1),
                        (!_busObjTlJournalDisp.NonCust.IsNull && _busObjTlJournalDisp.NonCust.Value == "Y"));   //#20928
                }
            }
            else if (origin == "CDRedemp")      //#140782, 20959
            {
                tempWin = CreateWindow("phoenix.client.dpredempcalc", "phoenix.client.dpredempcalc", "frmDpRedempCalc");
                tempWin.InitParameters(
                    _busObjTlJournalDisp.AcctType.Value, //Acct Type
                    _busObjTlJournalDisp.AcctNo.Value,  //Acct No
                    _busObjTlJournalDisp.TranEffectiveDt.Value, //Effective Dt
                    this.ScreenId, //parent screen id
                    null,   //history ptid
                    _busObjTlJournalDisp.Ptid.Value,   //journal ptid
                    _calcPtid,   //calc ptid
                    null,   //tran code
                    null,   //Amount
                    null,   //OvrdPenalty
                    null    //UmbCode
                    );
            }
            else if (origin == "SavingsBond")   //#140798
            {
                tempWin = Helper.CreateWindow("phoenix.client.tlredemption", "Phoenix.Windows.TlRedemption", "frmCalcRedemption");
                tempWin.InitParameters(null, this.ScreenId,
                                        null,
                                        _busObjTlJournalDisp.Ptid.Value,
                                        false,
                                        _busObjTlJournalDisp.RimNo.Value);
            }

			if ( tempWin != null )
			{
				tempWin.Workspace = this.Workspace;
				tempWin.Show();
			}
		}

		private void EnableDisableVisibleLogic( string origin )
		{
			if (origin == "FormComplete")
			{
				if (_busObjTlJournalDisp.ItemCount.Value > 0 && _busObjTlJournalDisp.TlTranCode.Value != "BAT" )
					this.pbItemCapt.Enabled= true;
				else
					this.pbItemCapt.Enabled = false;
				// #71030
				if (((_busObjTlJournalDisp.SuperEmplId.Value > 0 || _busObjTlJournalDisp.SuperEmplId.Value == -1) &&
					"ABC~ACI~ACO~CLC~CLO".IndexOf(_busObjTlJournalDisp.TlTranCode.Value) < 0) ||
					(!_busObjTlJournalDisp.ForwardCreateDt.IsNull &&
					!_busObjTlJournalDisp.OfflnOvrdInfoExists.IsNull &&
					_busObjTlJournalDisp.OfflnOvrdInfoExists.Value == 1))   //#80660
					this.pbOverrides.Enabled = true;
				else
					this.pbOverrides.Enabled = false;
				//
				if (_busObjTlJournalDisp.CashCount.Value == GlobalVars.Instance.ML.Y)
					this.pbCashDetails.Enabled = true;
				else
					this.pbCashDetails.Enabled = false;
				if (TellerVars.Instance.IsAppOnline)
				{
					if ( !_busObjTlJournalDisp.TranCode.IsNull &&
						( _busObjTlJournalDisp.TranCode.Value == 156 || _busObjTlJournalDisp.TranCode.Value == 196)
						)
					{
						this.pbWireTfrPrint.Enabled = true;
						if (!_busObjTlJournalDisp.AcctType.IsNull && _busObjTlJournalDisp.AcctType.Value != FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM)
							CallXMThruCDS( 2 );
					}
					else
						this.pbWireTfrPrint.Enabled = false;
				}
				else
					this.pbWireTfrPrint.Enabled = false;
                //Begin #74834
                if (!this.parentScreenId.IsNull && (this.parentScreenId.Value == Phoenix.Shared.Constants.ScreenId.DpTCHistoryDisplay ||
                    this.parentScreenId.Value == 218))
                {
                    pbItemCapt.Visible = false;
                    pbWireTfrPrint.Visible = false;
                    pbCashDetails.Visible = false;
                    lblCharges.Visible = false;
                    lblCharges1.Visible = true;
                    pbChkDetails.Visible = false;   //#76057
                }
                else
                {
                    lblCharges.Visible = true;
                    lblCharges1.Visible = false;
                }
                //End #74834
                #region #76057
                if (("902, 910").IndexOf(_busObjTlJournalDisp.TranCode.Value.ToString()) == -1 || !AppInfo.Instance.IsAppOnline)    //#1884 - added AppOnline check
                    pbChkDetails.Visible = false;
                #endregion

                #region 79510
                pbShBrTranDisplay.Visible = TellerVars.Instance.SharedBranchCustomOption;
                if (_busObjTlJournalDisp.SharedBranch.Value == GlobalVars.Instance.ML.Y)
                {
                    pbShBrTranDisplay.Enabled = true;
                }
                else
                    pbShBrTranDisplay.Enabled = false;
               #endregion 79510

                //#region 79510
                //if (TellerVars.Instance.SharedBranchCustomOption)
                //{
                //    pbShBrTranDisplay.Visible = true;
                //    if (_busObjTlJournalDisp.SharedBranch.Value == GlobalVars.Instance.ML.Y)
                //    {
                //        pbShBrTranDisplay.Enabled = true;
                //    }
                //    else
                //        pbShBrTranDisplay.Enabled = false;
                //}
                //else
                //    pbShBrTranDisplay.Visible = false;
                //#endregion 79510

                //Begin #79510, #09368
                //if (DataService.Instance.XmDbStatus != XmDbStatus.Day && _busObjTlJournalDisp.RecordSource.Value = "P")
                //{
                //    pbCashDetails.Enabled = false;
                //    pbChkDetails.Enabled = false;
                //    pbItemCapt.Enabled = false;
                //    pbOverrides.Enabled = false;
                //    pbShBrTranDisplay.Enabled = false;
                //    pbWireTfrPrint.Enabled = false;
                //}
                //End #79510, #09368

				//Begin #9539
				if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
				{
					pbItemCapt.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					pbWireTfrPrint.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					pbCashDetails.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					pbChkDetails.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
				}
				//End #9539

                //#140798
                if (!_busObjTlJournalDisp.TranCode.IsNull &&
                    (_busObjTlJournalDisp.TranCode.Value == 912 || _busObjTlJournalDisp.TranCode.Value == 921) &&
                    _busObjTlJournalDisp.Reversal.Value != 2)
                {
                    this.pbBondDetails.Enabled = true;
                }
                else
                    this.pbBondDetails.Enabled = false;
            }
		}

		#region CallXMThruCDS
		private bool CallXMThruCDS( int idXmCds )
		{
			if (idXmCds == 1)
			{
				using (new WaitCursor())
				{
					_dpAcctBusObj = new DpAcct();
					_dpAcctBusObj.ActionType = XmActionType.Select;
					_dpAcctBusObj.SelectAllFields = false;
					_dpAcctBusObj.AcctNo.Value = _busObjTlJournalDisp.AcctNo.Value;
					_dpAcctBusObj.AcctType.Value = _busObjTlJournalDisp.AcctType.Value;
					_dpAcctBusObj.RimNo.Selected = true;
					_dpAcctBusObj.ClassCode.Selected = true;
					DataService.Instance.ProcessRequest(this._dpAcctBusObj);
				}
			}
			if (idXmCds == 2)
			{
				using (new WaitCursor())
				{
					_adGbAcctType = new AdGbAcctType();
					_adGbAcctType.SelectAllFields = false;
					_adGbAcctType.DepLoan.Selected = true;
					_adGbAcctType.ApplType.Selected = true;
					_adGbAcctType.AcctType.Value = _busObjTlJournalDisp.AcctType.Value;
					_adGbAcctType.ActionType = XmActionType.Select;
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_adGbAcctType);
				}
			}
			return true;
		}
		#endregion

		#endregion


		#region pbWireTfrPrint_Click
		private void pbWireTfrPrint_Click(object sender, PActionEventArgs e)
		{
			try
			{
				if (DataService.Instance.PrimaryDbAvailable && TellerVars.Instance.IsAppOnline)
				{
					//360973 - Generating Wire Transfer Confirmation. Please wait...
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360973));
					_eformBusObj = new Phoenix.BusObj.Misc.Eforms();
					if (_busObjTlJournalDisp.AcctType.Value.Trim() != FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM)
					{
						CallXMThruCDS( 1 );
						_eformBusObj.AcctType.Value = _busObjTlJournalDisp.AcctType.Value;
						_eformBusObj.AcctNo.Value = _busObjTlJournalDisp.AcctNo.Value;
						_eformBusObj.ClassCode.Value = _dpAcctBusObj.ClassCode.Value;
						_eformBusObj.RimNo.Value = _dpAcctBusObj.RimNo.Value;
					}
					else
					{
						_eformBusObj.AcctType.Value = FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM;
						_eformBusObj.AcctNo.Value = _busObjTlJournalDisp.RimNo.Value.ToString();
						_eformBusObj.ClassCode.Value = _busObjTlJournalDisp.RimNo.Value;
						_eformBusObj.RimNo.Value = _busObjTlJournalDisp.RimNo.Value;
					}

					_eformBusObj.FormId.Value = 42;
					_eformBusObj.EFormTemplateName.Value = "451E.pdf";
					_eformBusObj.EFormLocalPath.Value = Environment.CurrentDirectory + @"\forms\";
					//Procedure not using this value
					_eformBusObj.WireTfrAmt.Value = _busObjTlJournalDisp.OnUsChks.Value;
					_eformBusObj.WireTfrFee.Value = (_busObjTlJournalDisp.CcAmt.IsNull?0:_busObjTlJournalDisp.CcAmt.Value);
					_eformBusObj.AcctId.Value = 0;
					_eformBusObj.SPProcName.Value = "psp_eforms_wire_tfr_confirm";
					_eformBusObj.DataSource.Value = "C";
					_eformBusObj.PDFMode.Value = "View";
					_eformBusObj.NoCopies.Value = 0;
					_eformBusObj.PrintType.Value = "I";
					_eformBusObj.CreateAudit.Value = "Y";
					_eformBusObj.GenerateFDF();
				}
				else
				{
					//300084
					PMessageBox.Show(300084, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					//300084 - The option you have selected is unavailable while the Phoenix database is not available.
				}
			}
			catch(PhoenixException pe)  //We do not want catch all
			{
				dlgInformation.Instance.HideInfo();
				if (pe.MLMessageId == 10223 || pe.MLMessageId == 10224)
				{
					//360987 - An error has occurred while trying to print using Adobe Acrobat Reader.  Please verify the Adobe Acrobat Reader application does not have any Update windows open or a problem with the installation.
					PMessageBox.Show(360987, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				}
				else if (pe.MLMessageId > 0 )
				{
					//Any Server Message that were thrown here...
					PMessageBox.Show(pe.MLMessageId, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				}
				else
				{
					//All other that server or client or any other that are thrown
					Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
					PMessageBox.Show(360974, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					//360974 - Failed to generate Wire Transfer Confirmation.
				}
				Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);

			}
			finally
			{
				dlgInformation.Instance.HideInfo();
			}

		}
		#endregion pbWireTfrPrint_Click

        private void dfTransferAccount_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmTlJournalDisplay_Load(object sender, EventArgs e)
        {

        }

        #region 79510
        private void pbShBrTranDisplay_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("SharedBranch");
        }
        #endregion

        #region 140782
        public override void CallParent(params object[] paramList)
        {
            //to refresh window on returning from CD Redemption window
            int childScreenId = 0;
            if (paramList.Length > 0)
            {
                Int32.TryParse(Convert.ToString(paramList[0]), out childScreenId);
                if (childScreenId == Phoenix.Shared.Constants.ScreenId.DpRedempCalc)
                {
                    this._busObjTlJournalDisp.ActionType = XmActionType.Select;
                    this._busObjTlJournalDisp.SelectAllFields = false;
                    this._busObjTlJournalDisp.FedWhAmt.Selected = true;
                    this._busObjTlJournalDisp.StateWhAmt.Selected = true;
                    this._busObjTlJournalDisp.OtherWhAmt.Selected = true;
                    CoreService.DataService.ProcessRequest(this._busObjTlJournalDisp);

                    dfHiddenFedWhAmt.UnFormattedValue = this._busObjTlJournalDisp.FedWhAmt.Value;
                    dfHiddenStateWhAmt.UnFormattedValue = this._busObjTlJournalDisp.StateWhAmt.Value;
                    dfHiddenOtherWhAmt.UnFormattedValue = this._busObjTlJournalDisp.OtherWhAmt.Value;

                    dfWithholdingAmt.Text = (Convert.ToDecimal(dfHiddenFedWhAmt.UnFormattedValue == null ? "0" : dfHiddenFedWhAmt.UnFormattedValue) +
                        Convert.ToDecimal(dfHiddenStateWhAmt.UnFormattedValue == null ? "0" : dfHiddenStateWhAmt.UnFormattedValue) +
                        Convert.ToDecimal(dfHiddenOtherWhAmt.UnFormattedValue == null ? "0" : dfHiddenOtherWhAmt.UnFormattedValue)).ToString("C");
                }
            }
            base.CallParent(paramList);
        }

        private void pbCDRedemp_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("CDRedemp");
        }
        #endregion 140782



    }
}
