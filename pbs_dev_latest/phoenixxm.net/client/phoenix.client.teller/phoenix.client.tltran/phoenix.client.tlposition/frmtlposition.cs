#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlPosition.cs
// NameSpace: Phoenix.Client.TlPosition
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//03/11/2006	1		mselvaga	Created.
//08/14/2006	2		mselvaga	#67880 - Offline storing changes added.
//10/31/2006	3		vreddy		#67883 - Offline Added Offline Report
//11/29/2006	4		mselvaga	#70920 - Fixed dfDifference field refresh issue.
//11/29/2006	5		mselvaga	#70921 - Fixed pbAdjTotals action enable/disable for offline.
//11/30/2006	6		mselvaga	#70940 - Expand the field length for dfCurrencyExchange.
//12/01/2006	7		mselvaga	#70958 - Added enable/disable logic based on position view and AppOnline values for pod totals actions.
//12/19/2006	8		mselvaga	#71132 - Moved _balDenomTracking assignment statement to FormComplete event.
//12/20/2006	9		mselvaga	#71243 - Over/Short changes added.
//01/22/2007	10		mselvaga	#71435 - Modified close out information box message text. Commented out verify POD message
//02/01/2007	11		mselvaga	#71651 - Ending Cash figure is calculating Cash Drawer figure entered if user counts cash on their drawer and accesses a position from Supervisor window.
//02/12/2007	12		mselvaga	#60648 - Foreign Wire Tran changes added.
//02/27/2007	13		rpoddar		#71886 - Cache CashCount globally
//04/10/2007	14		Vreddy		#72361 - Teller 2007 - AD_GB_RSM.employee_id is printing on the Teller Summary Position and Cash Summary Reports, it should be AD_GB_RSM.teller_no
//05/25/2007	15		mselvaga	#72847 - Supervisor functions do not allow user to perform a closeout if the supervisor has just closed out their drawer.
//06/19/2007    16      Vreddy      #73125 - Moving the Resetting the Printer Layout orientation to on disposing and commented the code at Report locations...
//07/26/2007    17      mselvaga    #73218 - Ending cash totals are incorrect when supervisor closes out own drawer then accesses supervisor to close out another teller's drawer.
//08/17/2007    18      DEiland     #72916 - Updates to TLO110,TLF110 reports for TCD changes
//08/24/2007    19      BBedi       #72916 - Add TCD Support in Teller 2007 ye
//09/10/2007    20      mselvaga    #72916 - Add Tcd support -Phase III.
//09/20/2007    21      bbedi       #72916 - Issue Fix.
//09/20/2007    22      bbedi       #72916 - Issue Fix.
//10/02/2007    23      mselvaga    #72916 - Added fix for rollover issues.
//03/14/2008    24      JStainth    #74721 - Restriction action button is disabled in 24 by 7 mode.
//04/10/2008    25      mselvaga    #75739 - QA Release 2008 TCD - TCD Rollover on Carryover date is not writing to TL__POSITION.
//04/14/2008    26      mselvaga    #75864 - QA Release 2008 TCD - When the TCD DispenserTotals is viewed from TCD Administrative Functions, on closing the window re-set .ini for Allow zero inventory
//12/05/2008    27      mselvaga    #76458 - Added EX account changes.
//02/19/2009    28      LSimpson    #76409 - Added teller check proof modifications.
//05/05/2009    29      SDhamija    #3132 - Short Keys are not working on Teller Summary Position window after counting drawer to close it out
//05/14/2009    30      SDhamija    #3132 - If I access teller and select the Balance Drawer/Batch Checks icon on the MDI toolbar (F5) all of the accelerator keys work.
//                                          If I have another window open, (Teller Transaction, Teller Journal, etc) and then access the
//                                          Balance Drawer/Batch Checks icon on the MDI toolbar (F5) none of the accelerator keys
//                                          on the Teller Summary Position window work.
//06/02/2009    31      LSimpson    #4366 - Added date param for frmtlprooftotals.
//01/25/2010    32      LSimpson    #79619 - Suppress messages 311084 and 361001 if non cash drawer.
//02/16/2010    33      mselvaga    WI#7752 - Fixed proof fields alignment problem.
//03/29/2010    34      LSimpson    #79574 - Added cash recycler changes for tlo110 and tlf110.
//04/27/2010    35      mselvaga    Enh#79574 - Added cash recycler changes.
//05/06/2010    36      mselvaga    #79574 - Added fix for clear all and rollover.
//06/04/2010    37      LSimpson    #9271 - Business object property is losing its value on form load.  Temporary hack to correct for UAT.
//06/21/2010    38      mselvaga    WI#9469 - DUT - On Closeout through Supervisor functions, the RLO - Rollover transaction is not writing to TL_JOURNAL.
//06/22/2010    39      LSimpson    #9510 - Get non-cash setting when closing drawer from Supervisor.
//06/29/2010    40      mselvaga    WI#9624 - Added TCD/TCR window title fix.
//11/22/2010    41      mselvaga    #79314 - Added remote override changes.
//03/29/2011    42      mselvaga    #13117 - Remote Override request approved by supervisor and the Telller reposted the transaction with a closed drawer.
//06/06/2011    43      vsharma     WI#11026/11027 -  Added code for TCD machine - Rollover button.
//08/10/2011    44      SDighe      WI#13178 - The Branch Position Summary shows a difference in cash for teller drawers that were not used that day.
//09/30/2011    45      SDighe      WI#11962 - When a rollover is processed, the teller prints their teller cash drawer count and the quantity and amount are cleared to all zero's.
//01/09/2012	46		NJoshi		wi#15865 - Offages are not updating drawer balances.(This is when multiple tellers log in and out of the same drawer
//02/24/2012	47		NJoshi		wi#16658 - Glory Cash Recycler does not work in Phoenix
//08/08/2012	48		NJoshi		wi#18900 - Proofing and Batching here does not refresh the tl journal grid.
//12/13/2012    49      fspath      #140772 - Added two parameters NoInvItemPurch NoInvItemReturn to report.
//01/29/2013    50      mselvaga    #20598  - Updates to TLF110 reports for inventory changes
//03/07/2013    51      mselvaga    #21476 - Fixed the inventory purch and return params to report TLO110.
//04/08/2013    52      SDighe      WI#22234- TCR Journal detected an overage, TLD31000 out of balance
//10/18/2013    53      SPatterson  WI#23798 - Changed the difference calculation.
//10/15/2013    54      mselvaga    #140895 - Added Teller Capture Changes.
//05/30/2014	55		JRhyne		WI#28710 - set first tab TabStop = true. The framework sets focus on the first editable control. This is determined by Tab Order. Because the first tab was not set with TabStop = Y, it was ignored.
//06/25/2014    56      DEiland     WI#29358/WI#29748 - When logging in as Supervisor the Ending Cash value is wrong because it does not do calculate the same way as logging in with Same Branch Number 
//08/08/2014    57      mselvaga    #30473 - Teller Capture - Release 2014 - Processing Pending Bulk Transactions.
//09/02/2014    58      mselvaga    #30969 - #140895 - AVTC Part I changes added.
//04/02/2015    59      AnishKumar  WI#35358 - When closeout is completed, white box on toolbar shows cash amount does not go blank
//09/28/2015    60      rpoddar     #195669, #35513 - Automate EOB Changes
//05/24/2016    61      RDeepthi    #45678. Set Dock to None for TAB Control and set Back color to "Control" for every tab page
//04/17/2017    62      AneeshKumar Task#60315 - Unable to close drawer out for another user when Supervisor is logged in without a drawer
//06/29/2017    63      AshishBabu  Task#64564 - Unable to close out teller drawer. Receive Error: Failed to insert into tl_cash_count
//07/10/2017    64      AshishBabu  Bug#68380 - Fixed the Error : Failed to insert into tl_cash_count when trying to close out the Drawer as Supervisor
//03/23/2018    65      SChacko     Task#86292 - sometimes cash counter amount has the value Decimal.MinValue and this causing issues.
//03/26/2018    66      SChacko     Bug#86407 - Refresh Issue in teller summary position window in case of clearing out the cash denomination.
//07/09/2018	67		RBhavsar	#95425 - Bank IOWA Transporter flag issue.
//07/17/2018    68      Minu        #94549- If cur_posting_dt is null when we are tying to close out drawer which is already closed out, show error message to user.
//08/06/2018    69      DEiland     Task#95932 - Fix issue with custom call not setting values needed for the TlJournal business object that is used in it
//02/14/2019    70      AshishBabu  Task#110967 - Fixed Application Error on Teller summary position window
//08/15/2019    71      FOyebola    Task#118226
//08/21/2019    72      FOyebola    Bug#118528
//08/21/2019    73      FOyebola    Bug#118530
//08/22/2019    74      FOyebola    Task#118229
//08/22/2019    75      FOyebola    Bug#118569
//12/12/2019    76      AshishBabu  Task#121857 summary poition window was not refreshing values while using Make Current Window Action, fixed it
//03/30/2020    77      SChacko     Userstory#/Task#124859 - when closing the drawer, if drawer cash balance is 0, then we dont need to validate for the backdating configuration.
//--------------------------------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;



using Phoenix.Windows.Forms;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Admin.Global;
using Phoenix.Shared.Windows;
using Phoenix.Shared.Ucm;
using Phoenix.Shared.Communicator;  //#13117
using Phoenix.Client.Teller;  //#140895

namespace Phoenix.Client.TlPosition
{
	/// <summary>
	/// Summary description for frmTlPosition.
	/// </summary>
	public class frmTlPosition : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.BusObj.Teller.TlHelper _tellerHelper = new BusObj.Teller.TlHelper();    //#140895
		private TlDrawer _tlDrawer = new TlDrawer();
		private TlDrawer _tlDrawerTemp = new TlDrawer();
		private TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
		private Phoenix.BusObj.Teller.TlPosition _tlPosition = new Phoenix.BusObj.Teller.TlPosition();
		private Phoenix.BusObj.Teller.TlPosition _tlPositionTemp = new Phoenix.BusObj.Teller.TlPosition();
		private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);
        private Phoenix.BusObj.Teller.TlPosition _tempPosObj = new Phoenix.BusObj.Teller.TlPosition(); //#72916
		private AdGbRsm _adGbRsmTemp = new AdGbRsm();
		private AdGbBranch _adGbBranchBusObj;
		private TlJournal _tlJournal = new TlJournal();
		private TellerVars _tellerVars = TellerVars.Instance;
		private PComboBoxStandard drawerCombo = null;
		private TlJournalOvrd _tlJournalOvrd = null;
		private DialogResult dialogResult = DialogResult.None;
        private CashDispenser cashDisp = null; //#72916
        private ArrayList _deviceOutput = new ArrayList(); //#72916
        private CommunicatorService _commService = CommunicatorService.Instance;    //#13117

		private string employeeChange = "";
		private string systemCloseOut = "";
		private bool gbOffline = false;
		private bool isRealCloseOut = false;
		private bool isRequireSuperOvrd = false;
		private int unforwardedTranCount = 0;
		private bool _isBalDenomTracking = false;
		private bool _isDrawerClosedOut = false;
		private bool _isDrawerCounted = false;
        private bool _signedOnDrawerIsNonCashDrawer = false;  // # 9510
        private bool _supervisorFlag = false;       // WI#35358

		private PDateTime curPostingDate = new PDateTime("curPostingDate");//15864
		private PSmallInt positionView = new PSmallInt();
		private PSmallInt branchNo = new PSmallInt("branchNo");//15864
		private PSmallInt drawerNo = new PSmallInt("drawerNo");//15864
		private PDateTime closedDate = new PDateTime("closedDate");//15864
		private PDecimal positionPtid = new PDecimal();
		private PSmallInt previousEmplId = new PSmallInt();
		private PSmallInt newEmplId = new PSmallInt();
		private PString branchName = new PString();
		private bool returnGrandTotal = false;
		private PSmallInt employeeId = new PSmallInt();

		private PBaseType ClosingDt	= new PDateTime("A0");
		private PBaseType PostingDt = new PDateTime("A1");
		private PBaseType Difference = new PDecimal("A2");
		private PBaseType VerifyPOD = new PString("A3");
		private PBaseType CashDrawer = new PDecimal("A4");
		private PBaseType SystemDt = new PDateTime("A5");
		private PBaseType TellerCashAcct = new PString("A6");
		private PBaseType IsFromCloseOut = new PString("A7");
		private PBaseType TlJrnlPtid = new PDecimal("A8");
		private PBaseType SuperEmplId = new PSmallInt("A9");
		private PBaseType ForwardFrom = new PBaseType("A10");
		private PBaseType BalDenomTracking = new PString("A11");

		private PBaseType RptPosPtid = new PInt("A12");
		private PBaseType RptPosDesc = new PString("A13");
		private PBaseType RptPosCreatDt = new PDateTime("A14");
		private PBaseType RptPosCloseDt = new PDateTime("A15");



		private decimal totalCash = 0;
		private short	_podTotalsType = 0;
		/* Begin #71886 */
		private bool prevDrawerCounted = false;
		private decimal prevCountedAmount = 0;
		private bool drawerIsSignOn = false;
		private int lastCashUpdateCounter = 0;
		/* End #71886 */
        private bool proofTotalsViewed = false; // #76409
        private decimal TlJrnlPtidCO = 0; //WI#11962
		//
		PBaseType PodTotalsType	= new PSmallInt("PodType");
		PBaseType LastPodPtid = new PDecimal("LastPodPtid");
		PBaseType PrevLastPodPtid = new PDecimal("PrevLastPodPtid");
		PBaseType TotalPodCreditAmt = new PDecimal("TotalPodCreditAmt");
		PBaseType TotalPodDebitAmt = new PDecimal("TotalPodDebitAmt");
		//
		private bool _printReportSilent = false;
		private int _reportEmplId = GlobalVars.EmployeeId;
		private int _reportPosPtid = -1;
		Phoenix.Shared.Windows.PSetting _printerSettings = new PSetting();
		Phoenix.Shared.Windows.HtmlPrinter _htmlPrinter; // = new HtmlPrinter();
		Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;
		System.Diagnostics.Process _printProcess = null;
        private TlCashCount _tlCashCount = new TlCashCount();   //#79574
        private bool _isOkToBlockF1 = false;    //#79574
        private PAction tcdAction = null; //#79574

        //begin #15864
		private PBaseType tempCashShort = new PDecimal("TempCashShort");
		private PBaseType tempCashOver = new PDecimal("TempCashOver");
		//end #15864
        private TellerCapture _tellerCapture = null;  //#140895
        private bool _showTransportMonitorWarning = false;  //#140895
        private string _tellerCaptBulkTranXML = string.Empty;   //#140895
        private int _tellerCaptBulkBatchId = 0;   //#140895
        private string middayCashCount = "";      //118226
        private bool isMidDayCount = false;       //118226
        private bool isCountReqOnOpen = false;    //118569

        //
        private Phoenix.Windows.Forms.PTabControl picTabs;
		private Phoenix.Windows.Forms.PTabPage Name0;
		private Phoenix.Windows.Forms.PTabPage Name1;
		private Phoenix.Windows.Forms.PTabPage Name2;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbPurchasesandPayments;
		private Phoenix.Windows.Forms.PLabelStandard lblOfficialChecksPurchased;
		private Phoenix.Windows.Forms.PDfDisplay dfOfficialChks;
		private Phoenix.Windows.Forms.PLabelStandard lblMoneyOrdersPurchased;
		private Phoenix.Windows.Forms.PDfDisplay dfMoneyOrder;
		private Phoenix.Windows.Forms.PLabelStandard lblTravelersChecksPurchased;
		private Phoenix.Windows.Forms.PDfDisplay dfTrChksPurch;
		private Phoenix.Windows.Forms.PLabelStandard lblInventoryItemPurchased;
		private Phoenix.Windows.Forms.PDfDisplay dfInvItemPurchAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblInventoryItemReturned;
		private Phoenix.Windows.Forms.PDfDisplay dfInvItemReturnAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblUtilityPayments;
		private Phoenix.Windows.Forms.PDfDisplay dfUtilPmts;
		private Phoenix.Windows.Forms.PLabelStandard lblTreasuryTaxLoanPayments;
		private Phoenix.Windows.Forms.PDfDisplay dfTtlPmts;
		private Phoenix.Windows.Forms.PDfDisplay dfTabTitle0;
		private Phoenix.Windows.Forms.PDfDisplay dfTabTitle1;
		private Phoenix.Windows.Forms.PDfDisplay dfTabTitle2;
		private Phoenix.Windows.Forms.PAction pbTranTotals;
        private Phoenix.Windows.Forms.PAction pbBatchTotals;
        private Phoenix.Windows.Forms.PAction pbProofTotals;    // #76409
        private Phoenix.Windows.Forms.PAction pbCountDrawer;
		private Phoenix.Windows.Forms.PAction pbCloseOut;
		private Phoenix.Windows.Forms.PAction pbHistory;
		private Phoenix.Windows.Forms.PAction pbAdjTotals;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTellerPosition;
		private Phoenix.Windows.Forms.PLabelStandard lblDescription;
		private Phoenix.Windows.Forms.PdfStandard dfDescription;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbCashBalancing;
		private Phoenix.Windows.Forms.PLabelStandard lblBeginningCash;
		private Phoenix.Windows.Forms.PDfDisplay dfBeginningCash;
		private Phoenix.Windows.Forms.PDfDisplay dfCashIns;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOuts;
		private Phoenix.Windows.Forms.PDfDisplay dfCashOuts;
		private Phoenix.Windows.Forms.PLabelStandard lblTellerUnbatchedCashOuts;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalUnbatchedAmount;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOverage;
		private Phoenix.Windows.Forms.PDfDisplay dfCashOver;
		private Phoenix.Windows.Forms.PLabelStandard lblCashShortage;
		private Phoenix.Windows.Forms.PDfDisplay dfCashShort;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDUnbatchedCashOuts;
		private Phoenix.Windows.Forms.PDfDisplay dfTCDUnbatchCashOut;
		private Phoenix.Windows.Forms.PLabelStandard lblEndingCash;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalCash;
		private Phoenix.Windows.Forms.PLabelStandard lblCashDrawer;
		private Phoenix.Windows.Forms.PDfDisplay dfCashDrawer;
		private Phoenix.Windows.Forms.PLabelStandard lblDifference;
		private Phoenix.Windows.Forms.PDfDisplay dfDifference;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbDepositsandPayments;
		private Phoenix.Windows.Forms.PLabelStandard lblDeposits;
		private Phoenix.Windows.Forms.PDfDisplay dfDeposits;
		private Phoenix.Windows.Forms.PLabelStandard lblTransferDeposits;
		private Phoenix.Windows.Forms.PDfDisplay dfTfrCredits;
		private Phoenix.Windows.Forms.PLabelStandard lblIncomingWireTransfers;
		private Phoenix.Windows.Forms.PDfDisplay dfInWire;
		private Phoenix.Windows.Forms.PLabelStandard lblTotals;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalDeposits;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbDepositedItems;
		private Phoenix.Windows.Forms.PLabelStandard lblCashDeposited;
		private Phoenix.Windows.Forms.PDfDisplay dfCashDep;
		private Phoenix.Windows.Forms.PLabelStandard lblOnUsChecksItemsDeposited;
		private Phoenix.Windows.Forms.PDfDisplay dfOnUsChksDep;
		private Phoenix.Windows.Forms.PLabelStandard lblChecksDepositedAsCash;
		private Phoenix.Windows.Forms.PDfDisplay dfChksAsCashDep;
		private Phoenix.Windows.Forms.PLabelStandard lblTransitChecksDeposited;
		private Phoenix.Windows.Forms.PDfDisplay dfTransitChksDep;
		private Phoenix.Windows.Forms.PLabelStandard lblTotals_Dup1;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalChecks;
		private Phoenix.Windows.Forms.PLabelStandard lblOutgoingWireTransfers;
		private Phoenix.Windows.Forms.PDfDisplay dfOutWire;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbMiscellaneous;
		private Phoenix.Windows.Forms.PLabelStandard lblCashBought;
		private Phoenix.Windows.Forms.PDfDisplay dfCashBought;
		private Phoenix.Windows.Forms.PLabelStandard lblCashSold;
		private Phoenix.Windows.Forms.PDfDisplay dfCashSold;
		private Phoenix.Windows.Forms.PLabelStandard lblGeneralLedgerCredits;
		private Phoenix.Windows.Forms.PDfDisplay dfGlCredits;
		private Phoenix.Windows.Forms.PLabelStandard lblGeneralLedgerDebits;
		private Phoenix.Windows.Forms.PDfDisplay dfGlDebits;
		private Phoenix.Windows.Forms.PLabelStandard lblChargesCollected;
		private Phoenix.Windows.Forms.PDfDisplay dfCCAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblCTRsTriggered;
		private Phoenix.Windows.Forms.PDfDisplay dfCtrTriggered;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalOfTransactions;
		private Phoenix.Windows.Forms.PDfDisplay dfSequenceNo;
		private Phoenix.Windows.Forms.PDfDisplay dfDraftPurch;
		private Phoenix.Windows.Forms.PDfDisplay dfFCPurch;
		private Phoenix.Windows.Forms.PLabelStandard lblLoanPayments;
		private Phoenix.Windows.Forms.PDfDisplay dfLnPayments;
		private Phoenix.Windows.Forms.PLabelStandard lblSafeDepositPayments;
		private Phoenix.Windows.Forms.PDfDisplay dfSdPayments;
		private Phoenix.Windows.Forms.PLabelStandard lblAnalysisFeePayments;
		private Phoenix.Windows.Forms.PDfDisplay dfAnalFeePmt;
		private Phoenix.Windows.Forms.PDfDisplay dfFCExch;
		private Phoenix.Windows.Forms.PDfDisplay dfChkExch;
		private Phoenix.Windows.Forms.PDfDisplay dfOnUsExch;
		private Phoenix.Windows.Forms.PDfDisplay dfBatchChkAmt;
		private Phoenix.Windows.Forms.PDfDisplay dfCashWd;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTCDCashItems;
		private Phoenix.Windows.Forms.PdfStandard dfHiddenTCDCashItem;
		private Phoenix.Windows.Forms.PDfDisplay dfTCDCashDisp;
		private Phoenix.Windows.Forms.PLabelStandard lblCashBoughtfromTCD;
		private Phoenix.Windows.Forms.PDfDisplay dfTCDCashBought;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbWithdrawalsandAdvances;
		private Phoenix.Windows.Forms.PLabelStandard lblChecksCashed;
		private Phoenix.Windows.Forms.PDfDisplay dfChksCashedWd;
		private Phoenix.Windows.Forms.PLabelStandard lblOnUsChecksCashed;
		private Phoenix.Windows.Forms.PDfDisplay dfOnUsChksCashed;
		private Phoenix.Windows.Forms.PLabelStandard lblWithdrawals;
		private Phoenix.Windows.Forms.PDfDisplay dfOthWd;
		private Phoenix.Windows.Forms.PLabelStandard lblTransferWithdrawals;
		private Phoenix.Windows.Forms.PDfDisplay dfTfrDebits;
		private Phoenix.Windows.Forms.PLabelStandard lblAccountCloseouts;
		private Phoenix.Windows.Forms.PDfDisplay dfAccountCloseouts;
		private Phoenix.Windows.Forms.PLabelStandard lblLoanAdvances;
		private Phoenix.Windows.Forms.PDfDisplay dfLnAdvances;
		private Phoenix.Windows.Forms.PLabelStandard lblTotals_Dup2;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalWithdrawals;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbExchangeandAdvances;
		private Phoenix.Windows.Forms.PLabelStandard lblTravelersChecksCashed;
		private Phoenix.Windows.Forms.PDfDisplay dfTrChksExch;
		private Phoenix.Windows.Forms.PLabelStandard lblSavingsBondsCashed;
		private Phoenix.Windows.Forms.PDfDisplay dfBondExch;
		private Phoenix.Windows.Forms.PLabelStandard lblIBondsCashed;
		private Phoenix.Windows.Forms.PDfDisplay dfIBondExch;
		private Phoenix.Windows.Forms.PLabelStandard lblCreditCardCashAdvance;
		private Phoenix.Windows.Forms.PDfDisplay dfCCAdvance;
		private Phoenix.Windows.Forms.PLabelStandard lblCurrencyExchanges;
		private Phoenix.Windows.Forms.PPanel pPanelCashBal;
		private Phoenix.Windows.Forms.PPanel pPanelDepositItem;
		private Phoenix.Windows.Forms.PPanel pPanelDepPay;
		private Phoenix.Windows.Forms.PPanel pPanelWd;
		private Phoenix.Windows.Forms.PLabelStandard lblCashIns;
		private Phoenix.Windows.Forms.PLabelStandard lblTcdCashDispensed;
		private Phoenix.Windows.Forms.PLabelStandard lblPostingDate;
		private Phoenix.Windows.Forms.PLabelStandard lblClosedDateTime;
		private Phoenix.Windows.Forms.PDfDisplay dfSavedDateTime;
		private Phoenix.Windows.Forms.PDfDisplay dfSavedTime;
		private Phoenix.Windows.Forms.PAction pbCurrentPod;
		private Phoenix.Windows.Forms.PAction pbLastPod;
		private Phoenix.Windows.Forms.PAction pbDailyPod;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PDfDisplay dfInFrWire;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard2;
		private Phoenix.Windows.Forms.PDfDisplay dfOutFrWire;
        private PDfDisplay dfTCDCashLoaded;
        private PLabelStandard lblTcdCashLoaded;
        private PLabelStandard lblTcdCashRemoved;
        private PDfDisplay dfTCDCashRemoved;
        private PAction pbRollover;
        private PGroupBoxStandard gbExternals;
        private PLabelStandard lblExtDebits;
        private PLabelStandard lblExtCredits;
        private PDfDisplay dfExtDebits;
        private PDfDisplay dfExtCredits;
        private PGroupBoxStandard gbChkBal;
        private PLabelStandard lblOnUsChecksNotProofed;
        private PLabelStandard lblNotOnUsChecksNotProofed;
        private PLabelStandard lblOnUsChecksProofed;
        private PLabelStandard lblNotOnUsChecksProofed;
        private PLabelStandard lblCheckTotals;
        private PPanel pPanel1;
        private PDfDisplay dfOnUsChecksNotProofed;
        private PDfDisplay dfNotOnUsChecksNotProofed;
        private PDfDisplay dfOnUsChecksProofed;
        private PDfDisplay dfNotOnUsChecksProofed;
        private PDfDisplay dfTotal;
        private PLabelStandard lblTcrCashDepleted;
        private PDfDisplay dfTCRCashDepleted;
        private PLabelStandard lblTcrCashSold;
        private PDfDisplay dfTCDCashSold;
        private PLabelStandard lblTcrCashDeposited;
        private PDfDisplay dfTCRCashDeposited;
        private PDfDisplay dfNoInvItemReturn;
        private PDfDisplay dfNoInvItemPurch;
        private PAction pbEOB;
        private PAction pbBulkProcess;
        private PDfDisplay dfTlCaptOnUsChks;
        private PLabelStandard lblOnUsTlCapturedChecks;
        private PDfDisplay dfTlCaptTransitChks;
        private PLabelStandard lblNotOnUsTlCapturedChecks;
        private PAction pbMiddayCount;
        private Phoenix.Windows.Forms.PDfDisplay dfCurrencyExchange;

		public frmTlPosition()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.SupportedActions |= StandardAction.Save;
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
				if (_htmlPrinter != null)
				{
					//_htmlPrinter.Dispose();
                    //73125
                    if (_printerSettings != null)
                    {
                        _printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
                    }
				}
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
            Phoenix.FrameWork.Core.ControlInfo controlInfo3 = new Phoenix.FrameWork.Core.ControlInfo();
            this.picTabs = new Phoenix.Windows.Forms.PTabControl();
            this.Name0 = new Phoenix.Windows.Forms.PTabPage();
            this.gbChkBal = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTlCaptOnUsChks = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOnUsTlCapturedChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTlCaptTransitChks = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblNotOnUsTlCapturedChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotal = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsChecksNotProofed = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfNotOnUsChecksNotProofed = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsChecksProofed = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfNotOnUsChecksProofed = new Phoenix.Windows.Forms.PDfDisplay();
            this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
            this.lblOnUsChecksNotProofed = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNotOnUsChecksNotProofed = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblOnUsChecksProofed = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNotOnUsChecksProofed = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCheckTotals = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbDepositedItems = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pPanelDepositItem = new Phoenix.Windows.Forms.PPanel();
            this.lblTotals_Dup1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTransitChksDep = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTransitChecksDeposited = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfChksAsCashDep = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblChecksDepositedAsCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOnUsChksDep = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOnUsChecksItemsDeposited = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashDep = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashDeposited = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalDeposits = new Phoenix.Windows.Forms.PDfDisplay();
            this.gbDepositsandPayments = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfInFrWire = new Phoenix.Windows.Forms.PDfDisplay();
            this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanelDepPay = new Phoenix.Windows.Forms.PPanel();
            this.lblTotals = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfInWire = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblIncomingWireTransfers = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTfrCredits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTransferDeposits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDeposits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDeposits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAnalFeePmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblAnalysisFeePayments = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfSdPayments = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblSafeDepositPayments = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfLnPayments = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblLoanPayments = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalChecks = new Phoenix.Windows.Forms.PDfDisplay();
            this.gbCashBalancing = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblCashIns = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanelCashBal = new Phoenix.Windows.Forms.PPanel();
            this.dfDifference = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDifference = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashDrawer = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashDrawer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblEndingCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDUnbatchCashOut = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCDUnbatchedCashOuts = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashShort = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashShortage = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashOver = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashOverage = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalUnbatchedAmount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTellerUnbatchedCashOuts = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashOuts = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashOuts = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashIns = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfBeginningCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblBeginningCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbTellerPosition = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfSavedTime = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfSavedDateTime = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblClosedDateTime = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblPostingDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDescription = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.Name1 = new Phoenix.Windows.Forms.PTabPage();
            this.gbExternals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfExtDebits = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfExtCredits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblExtDebits = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblExtCredits = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbExchangeandAdvances = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCurrencyExchange = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCurrencyExchanges = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCCAdvance = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCreditCardCashAdvance = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfIBondExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblIBondsCashed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfBondExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblSavingsBondsCashed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTrChksExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTravelersChecksCashed = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbWithdrawalsandAdvances = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfOutFrWire = new Phoenix.Windows.Forms.PDfDisplay();
            this.pLabelStandard2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pPanelWd = new Phoenix.Windows.Forms.PPanel();
            this.dfTotalWithdrawals = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotals_Dup2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfLnAdvances = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblLoanAdvances = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAccountCloseouts = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblAccountCloseouts = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTfrDebits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTransferWithdrawals = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOthWd = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblWithdrawals = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOnUsChksCashed = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOnUsChecksCashed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfChksCashedWd = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblChecksCashed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOutWire = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOutgoingWireTransfers = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbMiscellaneous = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCashWd = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfBatchChkAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfChkExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfFCExch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfFCPurch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfDraftPurch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfSequenceNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalOfTransactions = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCtrTriggered = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCTRsTriggered = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCCAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblChargesCollected = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfGlDebits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblGeneralLedgerDebits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfGlCredits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblGeneralLedgerCredits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashSold = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashSold = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashBought = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashBought = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbPurchasesandPayments = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfNoInvItemReturn = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfNoInvItemPurch = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTabTitle2 = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTabTitle1 = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTabTitle0 = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTtlPmts = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTreasuryTaxLoanPayments = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfUtilPmts = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblUtilityPayments = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfInvItemReturnAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblInventoryItemReturned = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfInvItemPurchAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblInventoryItemPurchased = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTrChksPurch = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTravelersChecksPurchased = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfMoneyOrder = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblMoneyOrdersPurchased = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOfficialChks = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOfficialChecksPurchased = new Phoenix.Windows.Forms.PLabelStandard();
            this.Name2 = new Phoenix.Windows.Forms.PTabPage();
            this.gbTCDCashItems = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTCRCashDeposited = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTcrCashDeposited = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashSold = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTcrCashSold = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCRCashDepleted = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTcrCashDepleted = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashRemoved = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTcdCashRemoved = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashLoaded = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTcdCashLoaded = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTcdCashDispensed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashBought = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashBoughtfromTCD = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashDisp = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfHiddenTCDCashItem = new Phoenix.Windows.Forms.PdfStandard();
            this.pbTranTotals = new Phoenix.Windows.Forms.PAction();
            this.pbBatchTotals = new Phoenix.Windows.Forms.PAction();
            this.pbProofTotals = new Phoenix.Windows.Forms.PAction();
            this.pbCountDrawer = new Phoenix.Windows.Forms.PAction();
            this.pbCloseOut = new Phoenix.Windows.Forms.PAction();
            this.pbHistory = new Phoenix.Windows.Forms.PAction();
            this.pbAdjTotals = new Phoenix.Windows.Forms.PAction();
            this.pbCurrentPod = new Phoenix.Windows.Forms.PAction();
            this.pbLastPod = new Phoenix.Windows.Forms.PAction();
            this.pbDailyPod = new Phoenix.Windows.Forms.PAction();
            this.pbRollover = new Phoenix.Windows.Forms.PAction();
            this.pbEOB = new Phoenix.Windows.Forms.PAction();
            this.pbBulkProcess = new Phoenix.Windows.Forms.PAction();
            this.pbMiddayCount = new Phoenix.Windows.Forms.PAction();
            this.picTabs.SuspendLayout();
            this.Name0.SuspendLayout();
            this.gbChkBal.SuspendLayout();
            this.gbDepositedItems.SuspendLayout();
            this.gbDepositsandPayments.SuspendLayout();
            this.gbCashBalancing.SuspendLayout();
            this.gbTellerPosition.SuspendLayout();
            this.Name1.SuspendLayout();
            this.gbExternals.SuspendLayout();
            this.gbExchangeandAdvances.SuspendLayout();
            this.gbWithdrawalsandAdvances.SuspendLayout();
            this.gbMiscellaneous.SuspendLayout();
            this.gbPurchasesandPayments.SuspendLayout();
            this.Name2.SuspendLayout();
            this.gbTCDCashItems.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbBatchTotals,
            this.pbProofTotals,
            this.pbCountDrawer,
            this.pbCloseOut,
            this.pbMiddayCount,
            this.pbCurrentPod,
            this.pbLastPod,
            this.pbDailyPod,
            this.pbTranTotals,
            this.pbHistory,
            this.pbAdjTotals,
            this.pbRollover,
            this.pbEOB,
            this.pbBulkProcess});
            // 
            // picTabs
            // 
            this.picTabs.Controls.Add(this.Name0);
            this.picTabs.Controls.Add(this.Name1);
            this.picTabs.Controls.Add(this.Name2);
            this.picTabs.Dock = System.Windows.Forms.DockStyle.None;
            this.picTabs.Location = new System.Drawing.Point(0, 0);
            this.picTabs.Name = "picTabs";
            this.picTabs.SelectedIndex = 0;
            this.picTabs.Size = new System.Drawing.Size(690, 448);
            this.picTabs.TabIndex = 0;
            this.picTabs.TabStop = true;
            this.picTabs.SelectedIndexChanged += new System.EventHandler(this.picTabs_SelectedIndexChanged);
            // 
            // Name0
            // 
            this.Name0.Controls.Add(this.gbChkBal);
            this.Name0.Controls.Add(this.gbDepositedItems);
            this.Name0.Controls.Add(this.gbDepositsandPayments);
            this.Name0.Controls.Add(this.gbCashBalancing);
            this.Name0.Controls.Add(this.gbTellerPosition);
            this.Name0.Location = new System.Drawing.Point(4, 22);
            this.Name0.MLInfo = controlInfo1;
            this.Name0.Name = "Name0";
            this.Name0.Size = new System.Drawing.Size(682, 422);
            this.Name0.TabIndex = 0;
            this.Name0.Text = "C&ash Balancing";
            // 
            // gbChkBal
            // 
            this.gbChkBal.Controls.Add(this.dfTlCaptOnUsChks);
            this.gbChkBal.Controls.Add(this.lblOnUsTlCapturedChecks);
            this.gbChkBal.Controls.Add(this.dfTlCaptTransitChks);
            this.gbChkBal.Controls.Add(this.lblNotOnUsTlCapturedChecks);
            this.gbChkBal.Controls.Add(this.dfTotal);
            this.gbChkBal.Controls.Add(this.dfOnUsChecksNotProofed);
            this.gbChkBal.Controls.Add(this.dfNotOnUsChecksNotProofed);
            this.gbChkBal.Controls.Add(this.dfOnUsChecksProofed);
            this.gbChkBal.Controls.Add(this.dfNotOnUsChecksProofed);
            this.gbChkBal.Controls.Add(this.pPanel1);
            this.gbChkBal.Controls.Add(this.lblOnUsChecksNotProofed);
            this.gbChkBal.Controls.Add(this.lblNotOnUsChecksNotProofed);
            this.gbChkBal.Controls.Add(this.lblOnUsChecksProofed);
            this.gbChkBal.Controls.Add(this.lblNotOnUsChecksProofed);
            this.gbChkBal.Controls.Add(this.lblCheckTotals);
            this.gbChkBal.Location = new System.Drawing.Point(0, 266);
            this.gbChkBal.Name = "gbChkBal";
            this.gbChkBal.PhoenixUIControl.ObjectId = 101;
            this.gbChkBal.Size = new System.Drawing.Size(340, 150);
            this.gbChkBal.TabIndex = 4;
            this.gbChkBal.TabStop = false;
            this.gbChkBal.Text = "Check Balancing";
            this.gbChkBal.Enter += new System.EventHandler(this.gbChkBal_Enter);
            // 
            // dfTlCaptOnUsChks
            // 
            this.dfTlCaptOnUsChks.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptOnUsChks.Location = new System.Drawing.Point(218, 102);
            this.dfTlCaptOnUsChks.Multiline = true;
            this.dfTlCaptOnUsChks.Name = "dfTlCaptOnUsChks";
            this.dfTlCaptOnUsChks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptOnUsChks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTlCaptOnUsChks.PhoenixUIControl.ObjectId = 113;
            this.dfTlCaptOnUsChks.PhoenixUIControl.XmlTag = "";
            this.dfTlCaptOnUsChks.PreviousValue = null;
            this.dfTlCaptOnUsChks.Size = new System.Drawing.Size(112, 16);
            this.dfTlCaptOnUsChks.TabIndex = 30;
            // 
            // lblOnUsTlCapturedChecks
            // 
            this.lblOnUsTlCapturedChecks.AutoEllipsis = true;
            this.lblOnUsTlCapturedChecks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnUsTlCapturedChecks.Location = new System.Drawing.Point(4, 102);
            this.lblOnUsTlCapturedChecks.Name = "lblOnUsTlCapturedChecks";
            this.lblOnUsTlCapturedChecks.PhoenixUIControl.ObjectId = 113;
            this.lblOnUsTlCapturedChecks.Size = new System.Drawing.Size(190, 16);
            this.lblOnUsTlCapturedChecks.TabIndex = 29;
            this.lblOnUsTlCapturedChecks.Text = "On-Us Teller Captured Checks:";
            // 
            // dfTlCaptTransitChks
            // 
            this.dfTlCaptTransitChks.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptTransitChks.Location = new System.Drawing.Point(218, 84);
            this.dfTlCaptTransitChks.Multiline = true;
            this.dfTlCaptTransitChks.Name = "dfTlCaptTransitChks";
            this.dfTlCaptTransitChks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptTransitChks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTlCaptTransitChks.PhoenixUIControl.ObjectId = 112;
            this.dfTlCaptTransitChks.PhoenixUIControl.XmlTag = "";
            this.dfTlCaptTransitChks.PreviousValue = null;
            this.dfTlCaptTransitChks.Size = new System.Drawing.Size(112, 16);
            this.dfTlCaptTransitChks.TabIndex = 28;
            // 
            // lblNotOnUsTlCapturedChecks
            // 
            this.lblNotOnUsTlCapturedChecks.AutoEllipsis = true;
            this.lblNotOnUsTlCapturedChecks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotOnUsTlCapturedChecks.Location = new System.Drawing.Point(4, 84);
            this.lblNotOnUsTlCapturedChecks.Name = "lblNotOnUsTlCapturedChecks";
            this.lblNotOnUsTlCapturedChecks.PhoenixUIControl.ObjectId = 112;
            this.lblNotOnUsTlCapturedChecks.Size = new System.Drawing.Size(190, 16);
            this.lblNotOnUsTlCapturedChecks.TabIndex = 27;
            this.lblNotOnUsTlCapturedChecks.Text = "Not On-Us Teller Captured Checks:";
            // 
            // dfTotal
            // 
            this.dfTotal.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotal.Location = new System.Drawing.Point(208, 128);
            this.dfTotal.Multiline = true;
            this.dfTotal.Name = "dfTotal";
            this.dfTotal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTotal.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotal.PhoenixUIControl.ObjectId = 106;
            this.dfTotal.PhoenixUIControl.XmlTag = "TotalChecks";
            this.dfTotal.PreviousValue = null;
            this.dfTotal.Size = new System.Drawing.Size(124, 16);
            this.dfTotal.TabIndex = 26;
            this.dfTotal.TextChanged += new System.EventHandler(this.pDfDisplay5_TextChanged);
            // 
            // dfOnUsChecksNotProofed
            // 
            this.dfOnUsChecksNotProofed.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsChecksNotProofed.Location = new System.Drawing.Point(218, 67);
            this.dfOnUsChecksNotProofed.Multiline = true;
            this.dfOnUsChecksNotProofed.Name = "dfOnUsChecksNotProofed";
            this.dfOnUsChecksNotProofed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsChecksNotProofed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsChecksNotProofed.PhoenixUIControl.ObjectId = 105;
            this.dfOnUsChecksNotProofed.PhoenixUIControl.XmlTag = "";
            this.dfOnUsChecksNotProofed.PreviousValue = null;
            this.dfOnUsChecksNotProofed.Size = new System.Drawing.Size(112, 16);
            this.dfOnUsChecksNotProofed.TabIndex = 25;
            // 
            // dfNotOnUsChecksNotProofed
            // 
            this.dfNotOnUsChecksNotProofed.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNotOnUsChecksNotProofed.Location = new System.Drawing.Point(218, 49);
            this.dfNotOnUsChecksNotProofed.Multiline = true;
            this.dfNotOnUsChecksNotProofed.Name = "dfNotOnUsChecksNotProofed";
            this.dfNotOnUsChecksNotProofed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNotOnUsChecksNotProofed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNotOnUsChecksNotProofed.PhoenixUIControl.ObjectId = 104;
            this.dfNotOnUsChecksNotProofed.PhoenixUIControl.XmlTag = "";
            this.dfNotOnUsChecksNotProofed.PreviousValue = null;
            this.dfNotOnUsChecksNotProofed.Size = new System.Drawing.Size(112, 16);
            this.dfNotOnUsChecksNotProofed.TabIndex = 24;
            // 
            // dfOnUsChecksProofed
            // 
            this.dfOnUsChecksProofed.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsChecksProofed.Location = new System.Drawing.Point(218, 31);
            this.dfOnUsChecksProofed.Multiline = true;
            this.dfOnUsChecksProofed.Name = "dfOnUsChecksProofed";
            this.dfOnUsChecksProofed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsChecksProofed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsChecksProofed.PhoenixUIControl.ObjectId = 103;
            this.dfOnUsChecksProofed.PhoenixUIControl.XmlTag = "";
            this.dfOnUsChecksProofed.PreviousValue = null;
            this.dfOnUsChecksProofed.Size = new System.Drawing.Size(112, 16);
            this.dfOnUsChecksProofed.TabIndex = 23;
            // 
            // dfNotOnUsChecksProofed
            // 
            this.dfNotOnUsChecksProofed.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNotOnUsChecksProofed.Location = new System.Drawing.Point(218, 14);
            this.dfNotOnUsChecksProofed.Multiline = true;
            this.dfNotOnUsChecksProofed.Name = "dfNotOnUsChecksProofed";
            this.dfNotOnUsChecksProofed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNotOnUsChecksProofed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNotOnUsChecksProofed.PhoenixUIControl.ObjectId = 102;
            this.dfNotOnUsChecksProofed.PhoenixUIControl.XmlTag = "";
            this.dfNotOnUsChecksProofed.PreviousValue = null;
            this.dfNotOnUsChecksProofed.Size = new System.Drawing.Size(112, 16);
            this.dfNotOnUsChecksProofed.TabIndex = 22;
            // 
            // pPanel1
            // 
            this.pPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel1.Location = new System.Drawing.Point(206, 122);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.Size = new System.Drawing.Size(124, 1);
            this.pPanel1.TabIndex = 21;
            this.pPanel1.TabStop = true;
            // 
            // lblOnUsChecksNotProofed
            // 
            this.lblOnUsChecksNotProofed.AutoEllipsis = true;
            this.lblOnUsChecksNotProofed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnUsChecksNotProofed.Location = new System.Drawing.Point(4, 67);
            this.lblOnUsChecksNotProofed.Name = "lblOnUsChecksNotProofed";
            this.lblOnUsChecksNotProofed.PhoenixUIControl.ObjectId = 105;
            this.lblOnUsChecksNotProofed.Size = new System.Drawing.Size(148, 16);
            this.lblOnUsChecksNotProofed.TabIndex = 20;
            this.lblOnUsChecksNotProofed.Text = "On-Us Checks Not Proofed:";
            this.lblOnUsChecksNotProofed.Click += new System.EventHandler(this.pLabelStandard7_Click);
            // 
            // lblNotOnUsChecksNotProofed
            // 
            this.lblNotOnUsChecksNotProofed.AutoEllipsis = true;
            this.lblNotOnUsChecksNotProofed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotOnUsChecksNotProofed.Location = new System.Drawing.Point(4, 49);
            this.lblNotOnUsChecksNotProofed.Name = "lblNotOnUsChecksNotProofed";
            this.lblNotOnUsChecksNotProofed.PhoenixUIControl.ObjectId = 104;
            this.lblNotOnUsChecksNotProofed.Size = new System.Drawing.Size(168, 16);
            this.lblNotOnUsChecksNotProofed.TabIndex = 19;
            this.lblNotOnUsChecksNotProofed.Text = "Not On-Us Checks Not Proofed:";
            // 
            // lblOnUsChecksProofed
            // 
            this.lblOnUsChecksProofed.AutoEllipsis = true;
            this.lblOnUsChecksProofed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOnUsChecksProofed.Location = new System.Drawing.Point(4, 31);
            this.lblOnUsChecksProofed.Name = "lblOnUsChecksProofed";
            this.lblOnUsChecksProofed.PhoenixUIControl.ObjectId = 103;
            this.lblOnUsChecksProofed.Size = new System.Drawing.Size(148, 16);
            this.lblOnUsChecksProofed.TabIndex = 18;
            this.lblOnUsChecksProofed.Text = "On-Us Checks Proofed:";
            // 
            // lblNotOnUsChecksProofed
            // 
            this.lblNotOnUsChecksProofed.AutoEllipsis = true;
            this.lblNotOnUsChecksProofed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNotOnUsChecksProofed.Location = new System.Drawing.Point(4, 14);
            this.lblNotOnUsChecksProofed.Name = "lblNotOnUsChecksProofed";
            this.lblNotOnUsChecksProofed.PhoenixUIControl.ObjectId = 102;
            this.lblNotOnUsChecksProofed.Size = new System.Drawing.Size(148, 16);
            this.lblNotOnUsChecksProofed.TabIndex = 17;
            this.lblNotOnUsChecksProofed.Text = "Not On-Us Checks Proofed:";
            // 
            // lblCheckTotals
            // 
            this.lblCheckTotals.AutoEllipsis = true;
            this.lblCheckTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCheckTotals.Location = new System.Drawing.Point(4, 128);
            this.lblCheckTotals.Name = "lblCheckTotals";
            this.lblCheckTotals.PhoenixUIControl.ObjectId = 106;
            this.lblCheckTotals.Size = new System.Drawing.Size(120, 16);
            this.lblCheckTotals.TabIndex = 16;
            this.lblCheckTotals.Text = "Totals:";
            // 
            // gbDepositedItems
            // 
            this.gbDepositedItems.Controls.Add(this.pPanelDepositItem);
            this.gbDepositedItems.Controls.Add(this.lblTotals_Dup1);
            this.gbDepositedItems.Controls.Add(this.dfTransitChksDep);
            this.gbDepositedItems.Controls.Add(this.lblTransitChecksDeposited);
            this.gbDepositedItems.Controls.Add(this.dfChksAsCashDep);
            this.gbDepositedItems.Controls.Add(this.lblChecksDepositedAsCash);
            this.gbDepositedItems.Controls.Add(this.dfOnUsChksDep);
            this.gbDepositedItems.Controls.Add(this.lblOnUsChecksItemsDeposited);
            this.gbDepositedItems.Controls.Add(this.dfCashDep);
            this.gbDepositedItems.Controls.Add(this.lblCashDeposited);
            this.gbDepositedItems.Controls.Add(this.dfTotalDeposits);
            this.gbDepositedItems.Location = new System.Drawing.Point(344, 56);
            this.gbDepositedItems.Name = "gbDepositedItems";
            this.gbDepositedItems.Size = new System.Drawing.Size(336, 124);
            this.gbDepositedItems.TabIndex = 2;
            this.gbDepositedItems.TabStop = false;
            this.gbDepositedItems.Text = "Deposited Items";
            // 
            // pPanelDepositItem
            // 
            this.pPanelDepositItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanelDepositItem.Location = new System.Drawing.Point(204, 94);
            this.pPanelDepositItem.Name = "pPanelDepositItem";
            this.pPanelDepositItem.Size = new System.Drawing.Size(124, 1);
            this.pPanelDepositItem.TabIndex = 8;
            this.pPanelDepositItem.TabStop = true;
            // 
            // lblTotals_Dup1
            // 
            this.lblTotals_Dup1.AutoEllipsis = true;
            this.lblTotals_Dup1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotals_Dup1.Location = new System.Drawing.Point(4, 100);
            this.lblTotals_Dup1.Name = "lblTotals_Dup1";
            this.lblTotals_Dup1.PhoenixUIControl.ObjectId = 30;
            this.lblTotals_Dup1.Size = new System.Drawing.Size(120, 16);
            this.lblTotals_Dup1.TabIndex = 9;
            this.lblTotals_Dup1.Text = "Totals:";
            // 
            // dfTransitChksDep
            // 
            this.dfTransitChksDep.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTransitChksDep.Location = new System.Drawing.Point(216, 70);
            this.dfTransitChksDep.Multiline = true;
            this.dfTransitChksDep.Name = "dfTransitChksDep";
            this.dfTransitChksDep.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTransitChksDep.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTransitChksDep.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTransitChksDep.PhoenixUIControl.ObjectId = 28;
            this.dfTransitChksDep.PhoenixUIControl.XmlTag = "TransitChksDep";
            this.dfTransitChksDep.PreviousValue = null;
            this.dfTransitChksDep.Size = new System.Drawing.Size(112, 16);
            this.dfTransitChksDep.TabIndex = 7;
            // 
            // lblTransitChecksDeposited
            // 
            this.lblTransitChecksDeposited.AutoEllipsis = true;
            this.lblTransitChecksDeposited.Location = new System.Drawing.Point(4, 70);
            this.lblTransitChecksDeposited.Name = "lblTransitChecksDeposited";
            this.lblTransitChecksDeposited.PhoenixUIControl.ObjectId = 28;
            this.lblTransitChecksDeposited.Size = new System.Drawing.Size(160, 16);
            this.lblTransitChecksDeposited.TabIndex = 6;
            this.lblTransitChecksDeposited.Text = "Transit Checks Deposited:";
            // 
            // dfChksAsCashDep
            // 
            this.dfChksAsCashDep.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksAsCashDep.Location = new System.Drawing.Point(216, 31);
            this.dfChksAsCashDep.Multiline = true;
            this.dfChksAsCashDep.Name = "dfChksAsCashDep";
            this.dfChksAsCashDep.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksAsCashDep.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfChksAsCashDep.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfChksAsCashDep.PhoenixUIControl.ObjectId = 27;
            this.dfChksAsCashDep.PhoenixUIControl.XmlTag = "ChksAsCashDep";
            this.dfChksAsCashDep.PreviousValue = null;
            this.dfChksAsCashDep.Size = new System.Drawing.Size(112, 16);
            this.dfChksAsCashDep.TabIndex = 3;
            // 
            // lblChecksDepositedAsCash
            // 
            this.lblChecksDepositedAsCash.AutoEllipsis = true;
            this.lblChecksDepositedAsCash.Location = new System.Drawing.Point(4, 31);
            this.lblChecksDepositedAsCash.Name = "lblChecksDepositedAsCash";
            this.lblChecksDepositedAsCash.PhoenixUIControl.ObjectId = 27;
            this.lblChecksDepositedAsCash.Size = new System.Drawing.Size(164, 16);
            this.lblChecksDepositedAsCash.TabIndex = 2;
            this.lblChecksDepositedAsCash.Text = "Checks Deposited As Cash:";
            // 
            // dfOnUsChksDep
            // 
            this.dfOnUsChksDep.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChksDep.Location = new System.Drawing.Point(216, 50);
            this.dfOnUsChksDep.Multiline = true;
            this.dfOnUsChksDep.Name = "dfOnUsChksDep";
            this.dfOnUsChksDep.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChksDep.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsChksDep.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOnUsChksDep.PhoenixUIControl.ObjectId = 26;
            this.dfOnUsChksDep.PhoenixUIControl.XmlTag = "OnUsChksDep";
            this.dfOnUsChksDep.PreviousValue = null;
            this.dfOnUsChksDep.Size = new System.Drawing.Size(112, 16);
            this.dfOnUsChksDep.TabIndex = 5;
            // 
            // lblOnUsChecksItemsDeposited
            // 
            this.lblOnUsChecksItemsDeposited.AutoEllipsis = true;
            this.lblOnUsChecksItemsDeposited.Location = new System.Drawing.Point(4, 50);
            this.lblOnUsChecksItemsDeposited.Name = "lblOnUsChecksItemsDeposited";
            this.lblOnUsChecksItemsDeposited.PhoenixUIControl.ObjectId = 26;
            this.lblOnUsChecksItemsDeposited.Size = new System.Drawing.Size(176, 16);
            this.lblOnUsChecksItemsDeposited.TabIndex = 4;
            this.lblOnUsChecksItemsDeposited.Text = "On-Us Checks/Items Deposited:";
            // 
            // dfCashDep
            // 
            this.dfCashDep.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDep.Location = new System.Drawing.Point(216, 13);
            this.dfCashDep.Multiline = true;
            this.dfCashDep.Name = "dfCashDep";
            this.dfCashDep.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDep.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashDep.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashDep.PhoenixUIControl.ObjectId = 25;
            this.dfCashDep.PhoenixUIControl.XmlTag = "CashDep";
            this.dfCashDep.PreviousValue = null;
            this.dfCashDep.Size = new System.Drawing.Size(112, 16);
            this.dfCashDep.TabIndex = 1;
            // 
            // lblCashDeposited
            // 
            this.lblCashDeposited.AutoEllipsis = true;
            this.lblCashDeposited.Location = new System.Drawing.Point(4, 13);
            this.lblCashDeposited.Name = "lblCashDeposited";
            this.lblCashDeposited.PhoenixUIControl.ObjectId = 25;
            this.lblCashDeposited.Size = new System.Drawing.Size(160, 16);
            this.lblCashDeposited.TabIndex = 0;
            this.lblCashDeposited.Text = "Cash Deposited:";
            // 
            // dfTotalDeposits
            // 
            this.dfTotalDeposits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDeposits.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalDeposits.Location = new System.Drawing.Point(204, 100);
            this.dfTotalDeposits.Multiline = true;
            this.dfTotalDeposits.Name = "dfTotalDeposits";
            this.dfTotalDeposits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDeposits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalDeposits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalDeposits.PhoenixUIControl.ObjectId = 30;
            this.dfTotalDeposits.PhoenixUIControl.XmlTag = "TotalDeposits";
            this.dfTotalDeposits.PreviousValue = null;
            this.dfTotalDeposits.Size = new System.Drawing.Size(124, 16);
            this.dfTotalDeposits.TabIndex = 10;
            // 
            // gbDepositsandPayments
            // 
            this.gbDepositsandPayments.Controls.Add(this.dfInFrWire);
            this.gbDepositsandPayments.Controls.Add(this.pLabelStandard1);
            this.gbDepositsandPayments.Controls.Add(this.pPanelDepPay);
            this.gbDepositsandPayments.Controls.Add(this.lblTotals);
            this.gbDepositsandPayments.Controls.Add(this.dfInWire);
            this.gbDepositsandPayments.Controls.Add(this.lblIncomingWireTransfers);
            this.gbDepositsandPayments.Controls.Add(this.dfTfrCredits);
            this.gbDepositsandPayments.Controls.Add(this.lblTransferDeposits);
            this.gbDepositsandPayments.Controls.Add(this.dfDeposits);
            this.gbDepositsandPayments.Controls.Add(this.lblDeposits);
            this.gbDepositsandPayments.Controls.Add(this.dfAnalFeePmt);
            this.gbDepositsandPayments.Controls.Add(this.lblAnalysisFeePayments);
            this.gbDepositsandPayments.Controls.Add(this.dfSdPayments);
            this.gbDepositsandPayments.Controls.Add(this.lblSafeDepositPayments);
            this.gbDepositsandPayments.Controls.Add(this.dfLnPayments);
            this.gbDepositsandPayments.Controls.Add(this.lblLoanPayments);
            this.gbDepositsandPayments.Controls.Add(this.dfTotalChecks);
            this.gbDepositsandPayments.Location = new System.Drawing.Point(344, 180);
            this.gbDepositsandPayments.Name = "gbDepositsandPayments";
            this.gbDepositsandPayments.Size = new System.Drawing.Size(336, 236);
            this.gbDepositsandPayments.TabIndex = 3;
            this.gbDepositsandPayments.TabStop = false;
            this.gbDepositsandPayments.Text = "Deposits and Payments";
            this.gbDepositsandPayments.Enter += new System.EventHandler(this.gbDepositsandPayments_Enter);
            // 
            // dfInFrWire
            // 
            this.dfInFrWire.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInFrWire.Location = new System.Drawing.Point(216, 76);
            this.dfInFrWire.Multiline = true;
            this.dfInFrWire.Name = "dfInFrWire";
            this.dfInFrWire.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInFrWire.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfInFrWire.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfInFrWire.PhoenixUIControl.ObjectId = 93;
            this.dfInFrWire.PhoenixUIControl.XmlTag = "InFrWire";
            this.dfInFrWire.PreviousValue = null;
            this.dfInFrWire.Size = new System.Drawing.Size(112, 16);
            this.dfInFrWire.TabIndex = 7;
            // 
            // pLabelStandard1
            // 
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(4, 76);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 93;
            this.pLabelStandard1.Size = new System.Drawing.Size(160, 16);
            this.pLabelStandard1.TabIndex = 6;
            this.pLabelStandard1.Text = "Incoming Fgn Wire Transfers:";
            // 
            // pPanelDepPay
            // 
            this.pPanelDepPay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanelDepPay.Location = new System.Drawing.Point(204, 156);
            this.pPanelDepPay.Name = "pPanelDepPay";
            this.pPanelDepPay.Size = new System.Drawing.Size(124, 1);
            this.pPanelDepPay.TabIndex = 14;
            this.pPanelDepPay.TabStop = true;
            // 
            // lblTotals
            // 
            this.lblTotals.AutoEllipsis = true;
            this.lblTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotals.Location = new System.Drawing.Point(4, 164);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.PhoenixUIControl.ObjectId = 30;
            this.lblTotals.Size = new System.Drawing.Size(120, 16);
            this.lblTotals.TabIndex = 15;
            this.lblTotals.Text = "Totals:";
            // 
            // dfInWire
            // 
            this.dfInWire.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInWire.Location = new System.Drawing.Point(216, 56);
            this.dfInWire.Multiline = true;
            this.dfInWire.Name = "dfInWire";
            this.dfInWire.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInWire.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfInWire.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfInWire.PhoenixUIControl.ObjectId = 44;
            this.dfInWire.PhoenixUIControl.XmlTag = "InWire";
            this.dfInWire.PreviousValue = null;
            this.dfInWire.Size = new System.Drawing.Size(112, 16);
            this.dfInWire.TabIndex = 5;
            // 
            // lblIncomingWireTransfers
            // 
            this.lblIncomingWireTransfers.AutoEllipsis = true;
            this.lblIncomingWireTransfers.Location = new System.Drawing.Point(4, 56);
            this.lblIncomingWireTransfers.Name = "lblIncomingWireTransfers";
            this.lblIncomingWireTransfers.PhoenixUIControl.ObjectId = 44;
            this.lblIncomingWireTransfers.Size = new System.Drawing.Size(160, 16);
            this.lblIncomingWireTransfers.TabIndex = 4;
            this.lblIncomingWireTransfers.Text = "Incoming Dom Wire Transfers:";
            // 
            // dfTfrCredits
            // 
            this.dfTfrCredits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTfrCredits.Location = new System.Drawing.Point(216, 36);
            this.dfTfrCredits.Multiline = true;
            this.dfTfrCredits.Name = "dfTfrCredits";
            this.dfTfrCredits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTfrCredits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTfrCredits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTfrCredits.PhoenixUIControl.ObjectId = 50;
            this.dfTfrCredits.PhoenixUIControl.XmlTag = "TfrCredits";
            this.dfTfrCredits.PreviousValue = null;
            this.dfTfrCredits.Size = new System.Drawing.Size(112, 16);
            this.dfTfrCredits.TabIndex = 3;
            // 
            // lblTransferDeposits
            // 
            this.lblTransferDeposits.AutoEllipsis = true;
            this.lblTransferDeposits.Location = new System.Drawing.Point(4, 36);
            this.lblTransferDeposits.Name = "lblTransferDeposits";
            this.lblTransferDeposits.PhoenixUIControl.ObjectId = 50;
            this.lblTransferDeposits.Size = new System.Drawing.Size(160, 16);
            this.lblTransferDeposits.TabIndex = 2;
            this.lblTransferDeposits.Text = "Transfer Deposits:";
            // 
            // dfDeposits
            // 
            this.dfDeposits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDeposits.Location = new System.Drawing.Point(216, 16);
            this.dfDeposits.Multiline = true;
            this.dfDeposits.Name = "dfDeposits";
            this.dfDeposits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDeposits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDeposits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDeposits.PhoenixUIControl.ObjectId = 71;
            this.dfDeposits.PhoenixUIControl.XmlTag = "Deposits";
            this.dfDeposits.PreviousValue = null;
            this.dfDeposits.Size = new System.Drawing.Size(112, 16);
            this.dfDeposits.TabIndex = 1;
            // 
            // lblDeposits
            // 
            this.lblDeposits.AutoEllipsis = true;
            this.lblDeposits.Location = new System.Drawing.Point(4, 16);
            this.lblDeposits.Name = "lblDeposits";
            this.lblDeposits.PhoenixUIControl.ObjectId = 71;
            this.lblDeposits.Size = new System.Drawing.Size(160, 16);
            this.lblDeposits.TabIndex = 0;
            this.lblDeposits.Text = "Deposits:";
            // 
            // dfAnalFeePmt
            // 
            this.dfAnalFeePmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAnalFeePmt.Location = new System.Drawing.Point(216, 96);
            this.dfAnalFeePmt.Multiline = true;
            this.dfAnalFeePmt.Name = "dfAnalFeePmt";
            this.dfAnalFeePmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAnalFeePmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAnalFeePmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAnalFeePmt.PhoenixUIControl.ObjectId = 72;
            this.dfAnalFeePmt.PhoenixUIControl.XmlTag = "AnalFeePmt";
            this.dfAnalFeePmt.PreviousValue = null;
            this.dfAnalFeePmt.Size = new System.Drawing.Size(112, 16);
            this.dfAnalFeePmt.TabIndex = 9;
            // 
            // lblAnalysisFeePayments
            // 
            this.lblAnalysisFeePayments.AutoEllipsis = true;
            this.lblAnalysisFeePayments.Location = new System.Drawing.Point(4, 96);
            this.lblAnalysisFeePayments.Name = "lblAnalysisFeePayments";
            this.lblAnalysisFeePayments.PhoenixUIControl.ObjectId = 72;
            this.lblAnalysisFeePayments.Size = new System.Drawing.Size(160, 16);
            this.lblAnalysisFeePayments.TabIndex = 8;
            this.lblAnalysisFeePayments.Text = "Analysis Fee Payments:";
            // 
            // dfSdPayments
            // 
            this.dfSdPayments.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfSdPayments.Location = new System.Drawing.Point(216, 136);
            this.dfSdPayments.Multiline = true;
            this.dfSdPayments.Name = "dfSdPayments";
            this.dfSdPayments.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfSdPayments.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSdPayments.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfSdPayments.PhoenixUIControl.ObjectId = 52;
            this.dfSdPayments.PhoenixUIControl.XmlTag = "SafeDepPmt";
            this.dfSdPayments.PreviousValue = null;
            this.dfSdPayments.Size = new System.Drawing.Size(112, 16);
            this.dfSdPayments.TabIndex = 13;
            // 
            // lblSafeDepositPayments
            // 
            this.lblSafeDepositPayments.AutoEllipsis = true;
            this.lblSafeDepositPayments.Location = new System.Drawing.Point(4, 136);
            this.lblSafeDepositPayments.Name = "lblSafeDepositPayments";
            this.lblSafeDepositPayments.PhoenixUIControl.ObjectId = 52;
            this.lblSafeDepositPayments.Size = new System.Drawing.Size(160, 16);
            this.lblSafeDepositPayments.TabIndex = 12;
            this.lblSafeDepositPayments.Text = "Safe Deposit Payments:";
            // 
            // dfLnPayments
            // 
            this.dfLnPayments.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLnPayments.Location = new System.Drawing.Point(216, 116);
            this.dfLnPayments.Multiline = true;
            this.dfLnPayments.Name = "dfLnPayments";
            this.dfLnPayments.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLnPayments.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfLnPayments.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfLnPayments.PhoenixUIControl.ObjectId = 51;
            this.dfLnPayments.PhoenixUIControl.XmlTag = "Payments";
            this.dfLnPayments.PreviousValue = null;
            this.dfLnPayments.Size = new System.Drawing.Size(112, 16);
            this.dfLnPayments.TabIndex = 11;
            // 
            // lblLoanPayments
            // 
            this.lblLoanPayments.AutoEllipsis = true;
            this.lblLoanPayments.Location = new System.Drawing.Point(4, 116);
            this.lblLoanPayments.Name = "lblLoanPayments";
            this.lblLoanPayments.PhoenixUIControl.ObjectId = 51;
            this.lblLoanPayments.Size = new System.Drawing.Size(160, 16);
            this.lblLoanPayments.TabIndex = 10;
            this.lblLoanPayments.Text = "Loan Payments:";
            // 
            // dfTotalChecks
            // 
            this.dfTotalChecks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChecks.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalChecks.Location = new System.Drawing.Point(204, 164);
            this.dfTotalChecks.Multiline = true;
            this.dfTotalChecks.Name = "dfTotalChecks";
            this.dfTotalChecks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChecks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalChecks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalChecks.PhoenixUIControl.ObjectId = 30;
            this.dfTotalChecks.PhoenixUIControl.XmlTag = "TotalChecks";
            this.dfTotalChecks.PreviousValue = null;
            this.dfTotalChecks.Size = new System.Drawing.Size(124, 16);
            this.dfTotalChecks.TabIndex = 16;
            // 
            // gbCashBalancing
            // 
            this.gbCashBalancing.Controls.Add(this.lblCashIns);
            this.gbCashBalancing.Controls.Add(this.pPanelCashBal);
            this.gbCashBalancing.Controls.Add(this.dfDifference);
            this.gbCashBalancing.Controls.Add(this.lblDifference);
            this.gbCashBalancing.Controls.Add(this.dfCashDrawer);
            this.gbCashBalancing.Controls.Add(this.lblCashDrawer);
            this.gbCashBalancing.Controls.Add(this.dfTotalCash);
            this.gbCashBalancing.Controls.Add(this.lblEndingCash);
            this.gbCashBalancing.Controls.Add(this.dfTCDUnbatchCashOut);
            this.gbCashBalancing.Controls.Add(this.lblTCDUnbatchedCashOuts);
            this.gbCashBalancing.Controls.Add(this.dfCashShort);
            this.gbCashBalancing.Controls.Add(this.lblCashShortage);
            this.gbCashBalancing.Controls.Add(this.dfCashOver);
            this.gbCashBalancing.Controls.Add(this.lblCashOverage);
            this.gbCashBalancing.Controls.Add(this.dfTotalUnbatchedAmount);
            this.gbCashBalancing.Controls.Add(this.lblTellerUnbatchedCashOuts);
            this.gbCashBalancing.Controls.Add(this.dfCashOuts);
            this.gbCashBalancing.Controls.Add(this.lblCashOuts);
            this.gbCashBalancing.Controls.Add(this.dfCashIns);
            this.gbCashBalancing.Controls.Add(this.dfBeginningCash);
            this.gbCashBalancing.Controls.Add(this.lblBeginningCash);
            this.gbCashBalancing.Location = new System.Drawing.Point(0, 56);
            this.gbCashBalancing.Name = "gbCashBalancing";
            this.gbCashBalancing.Size = new System.Drawing.Size(340, 209);
            this.gbCashBalancing.TabIndex = 1;
            this.gbCashBalancing.TabStop = false;
            this.gbCashBalancing.Text = "Cash Balancing";
            // 
            // lblCashIns
            // 
            this.lblCashIns.AutoEllipsis = true;
            this.lblCashIns.Location = new System.Drawing.Point(4, 31);
            this.lblCashIns.Name = "lblCashIns";
            this.lblCashIns.PhoenixUIControl.ObjectId = 12;
            this.lblCashIns.Size = new System.Drawing.Size(128, 16);
            this.lblCashIns.TabIndex = 2;
            this.lblCashIns.Text = "Cash Ins:";
            // 
            // pPanelCashBal
            // 
            this.pPanelCashBal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanelCashBal.Location = new System.Drawing.Point(208, 131);
            this.pPanelCashBal.Name = "pPanelCashBal";
            this.pPanelCashBal.Size = new System.Drawing.Size(124, 1);
            this.pPanelCashBal.TabIndex = 12;
            this.pPanelCashBal.TabStop = true;
            // 
            // dfDifference
            // 
            this.dfDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfDifference.Location = new System.Drawing.Point(208, 169);
            this.dfDifference.Multiline = true;
            this.dfDifference.Name = "dfDifference";
            this.dfDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDifference.PhoenixUIControl.ObjectId = 18;
            this.dfDifference.PhoenixUIControl.XmlTag = "Difference";
            this.dfDifference.PreviousValue = null;
            this.dfDifference.Size = new System.Drawing.Size(124, 16);
            this.dfDifference.TabIndex = 18;
            // 
            // lblDifference
            // 
            this.lblDifference.AutoEllipsis = true;
            this.lblDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifference.Location = new System.Drawing.Point(4, 169);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.PhoenixUIControl.ObjectId = 18;
            this.lblDifference.Size = new System.Drawing.Size(152, 16);
            this.lblDifference.TabIndex = 17;
            this.lblDifference.Text = "Difference:";
            // 
            // dfCashDrawer
            // 
            this.dfCashDrawer.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawer.Location = new System.Drawing.Point(220, 152);
            this.dfCashDrawer.Multiline = true;
            this.dfCashDrawer.Name = "dfCashDrawer";
            this.dfCashDrawer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawer.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashDrawer.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashDrawer.PhoenixUIControl.ObjectId = 16;
            this.dfCashDrawer.PhoenixUIControl.XmlTag = "CashDrawer";
            this.dfCashDrawer.PreviousValue = null;
            this.dfCashDrawer.Size = new System.Drawing.Size(112, 16);
            this.dfCashDrawer.TabIndex = 16;
            // 
            // lblCashDrawer
            // 
            this.lblCashDrawer.AutoEllipsis = true;
            this.lblCashDrawer.Location = new System.Drawing.Point(4, 152);
            this.lblCashDrawer.Name = "lblCashDrawer";
            this.lblCashDrawer.PhoenixUIControl.ObjectId = 16;
            this.lblCashDrawer.Size = new System.Drawing.Size(152, 16);
            this.lblCashDrawer.TabIndex = 15;
            this.lblCashDrawer.Text = "Cash Drawer:";
            // 
            // dfTotalCash
            // 
            this.dfTotalCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalCash.Location = new System.Drawing.Point(208, 136);
            this.dfTotalCash.Multiline = true;
            this.dfTotalCash.Name = "dfTotalCash";
            this.dfTotalCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalCash.PhoenixUIControl.ObjectId = 15;
            this.dfTotalCash.PhoenixUIControl.XmlTag = "EndingCash";
            this.dfTotalCash.PreviousValue = null;
            this.dfTotalCash.Size = new System.Drawing.Size(124, 16);
            this.dfTotalCash.TabIndex = 14;
            // 
            // lblEndingCash
            // 
            this.lblEndingCash.AutoEllipsis = true;
            this.lblEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndingCash.Location = new System.Drawing.Point(4, 136);
            this.lblEndingCash.Name = "lblEndingCash";
            this.lblEndingCash.PhoenixUIControl.ObjectId = 15;
            this.lblEndingCash.Size = new System.Drawing.Size(148, 16);
            this.lblEndingCash.TabIndex = 13;
            this.lblEndingCash.Text = "Ending Cash:";
            // 
            // dfTCDUnbatchCashOut
            // 
            this.dfTCDUnbatchCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDUnbatchCashOut.Location = new System.Drawing.Point(220, 188);
            this.dfTCDUnbatchCashOut.Multiline = true;
            this.dfTCDUnbatchCashOut.Name = "dfTCDUnbatchCashOut";
            this.dfTCDUnbatchCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDUnbatchCashOut.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDUnbatchCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDUnbatchCashOut.PhoenixUIControl.ObjectId = 79;
            this.dfTCDUnbatchCashOut.PhoenixUIControl.XmlTag = "TCDUnbatchCashOutAmt";
            this.dfTCDUnbatchCashOut.PreviousValue = null;
            this.dfTCDUnbatchCashOut.Size = new System.Drawing.Size(112, 16);
            this.dfTCDUnbatchCashOut.TabIndex = 20;
            // 
            // lblTCDUnbatchedCashOuts
            // 
            this.lblTCDUnbatchedCashOuts.AutoEllipsis = true;
            this.lblTCDUnbatchedCashOuts.Location = new System.Drawing.Point(4, 188);
            this.lblTCDUnbatchedCashOuts.Name = "lblTCDUnbatchedCashOuts";
            this.lblTCDUnbatchedCashOuts.PhoenixUIControl.ObjectId = 79;
            this.lblTCDUnbatchedCashOuts.Size = new System.Drawing.Size(180, 16);
            this.lblTCDUnbatchedCashOuts.TabIndex = 19;
            this.lblTCDUnbatchedCashOuts.Text = "TCD/TCR Unbatched Cash Outs:";
            // 
            // dfCashShort
            // 
            this.dfCashShort.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashShort.Location = new System.Drawing.Point(220, 110);
            this.dfCashShort.Multiline = true;
            this.dfCashShort.Name = "dfCashShort";
            this.dfCashShort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashShort.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashShort.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashShort.PhoenixUIControl.ObjectId = 78;
            this.dfCashShort.PhoenixUIControl.XmlTag = "CashShort";
            this.dfCashShort.PreviousValue = null;
            this.dfCashShort.Size = new System.Drawing.Size(112, 16);
            this.dfCashShort.TabIndex = 11;
            // 
            // lblCashShortage
            // 
            this.lblCashShortage.AutoEllipsis = true;
            this.lblCashShortage.Location = new System.Drawing.Point(4, 110);
            this.lblCashShortage.Name = "lblCashShortage";
            this.lblCashShortage.PhoenixUIControl.ObjectId = 78;
            this.lblCashShortage.Size = new System.Drawing.Size(168, 16);
            this.lblCashShortage.TabIndex = 10;
            this.lblCashShortage.Text = "Cash Shortage:";
            // 
            // dfCashOver
            // 
            this.dfCashOver.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOver.Location = new System.Drawing.Point(220, 90);
            this.dfCashOver.Multiline = true;
            this.dfCashOver.Name = "dfCashOver";
            this.dfCashOver.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOver.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashOver.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOver.PhoenixUIControl.ObjectId = 77;
            this.dfCashOver.PhoenixUIControl.XmlTag = "CashOver";
            this.dfCashOver.PreviousValue = null;
            this.dfCashOver.Size = new System.Drawing.Size(112, 16);
            this.dfCashOver.TabIndex = 9;
            // 
            // lblCashOverage
            // 
            this.lblCashOverage.AutoEllipsis = true;
            this.lblCashOverage.Location = new System.Drawing.Point(4, 90);
            this.lblCashOverage.Name = "lblCashOverage";
            this.lblCashOverage.PhoenixUIControl.ObjectId = 77;
            this.lblCashOverage.Size = new System.Drawing.Size(168, 16);
            this.lblCashOverage.TabIndex = 8;
            this.lblCashOverage.Text = "Cash Overage:";
            // 
            // dfTotalUnbatchedAmount
            // 
            this.dfTotalUnbatchedAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalUnbatchedAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalUnbatchedAmount.Location = new System.Drawing.Point(220, 70);
            this.dfTotalUnbatchedAmount.Multiline = true;
            this.dfTotalUnbatchedAmount.Name = "dfTotalUnbatchedAmount";
            this.dfTotalUnbatchedAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalUnbatchedAmount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalUnbatchedAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalUnbatchedAmount.PhoenixUIControl.ObjectId = 20;
            this.dfTotalUnbatchedAmount.PhoenixUIControl.XmlTag = "UnbatchedAmt";
            this.dfTotalUnbatchedAmount.PreviousValue = null;
            this.dfTotalUnbatchedAmount.Size = new System.Drawing.Size(112, 16);
            this.dfTotalUnbatchedAmount.TabIndex = 7;
            // 
            // lblTellerUnbatchedCashOuts
            // 
            this.lblTellerUnbatchedCashOuts.AutoEllipsis = true;
            this.lblTellerUnbatchedCashOuts.Location = new System.Drawing.Point(4, 70);
            this.lblTellerUnbatchedCashOuts.Name = "lblTellerUnbatchedCashOuts";
            this.lblTellerUnbatchedCashOuts.PhoenixUIControl.ObjectId = 20;
            this.lblTellerUnbatchedCashOuts.Size = new System.Drawing.Size(168, 16);
            this.lblTellerUnbatchedCashOuts.TabIndex = 6;
            this.lblTellerUnbatchedCashOuts.Text = "Teller Unbatched Cash Outs:";
            // 
            // dfCashOuts
            // 
            this.dfCashOuts.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOuts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfCashOuts.Location = new System.Drawing.Point(220, 50);
            this.dfCashOuts.Multiline = true;
            this.dfCashOuts.Name = "dfCashOuts";
            this.dfCashOuts.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOuts.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashOuts.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOuts.PhoenixUIControl.ObjectId = 13;
            this.dfCashOuts.PhoenixUIControl.XmlTag = "CashOut";
            this.dfCashOuts.PreviousValue = null;
            this.dfCashOuts.Size = new System.Drawing.Size(112, 16);
            this.dfCashOuts.TabIndex = 5;
            // 
            // lblCashOuts
            // 
            this.lblCashOuts.AutoEllipsis = true;
            this.lblCashOuts.Location = new System.Drawing.Point(4, 50);
            this.lblCashOuts.Name = "lblCashOuts";
            this.lblCashOuts.PhoenixUIControl.ObjectId = 13;
            this.lblCashOuts.Size = new System.Drawing.Size(168, 16);
            this.lblCashOuts.TabIndex = 4;
            this.lblCashOuts.Text = "Cash Outs:";
            // 
            // dfCashIns
            // 
            this.dfCashIns.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIns.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfCashIns.Location = new System.Drawing.Point(220, 31);
            this.dfCashIns.Multiline = true;
            this.dfCashIns.Name = "dfCashIns";
            this.dfCashIns.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIns.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashIns.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashIns.PhoenixUIControl.ObjectId = 12;
            this.dfCashIns.PhoenixUIControl.XmlTag = "CashIn";
            this.dfCashIns.PreviousValue = null;
            this.dfCashIns.Size = new System.Drawing.Size(112, 16);
            this.dfCashIns.TabIndex = 3;
            // 
            // dfBeginningCash
            // 
            this.dfBeginningCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBeginningCash.Location = new System.Drawing.Point(220, 13);
            this.dfBeginningCash.Multiline = true;
            this.dfBeginningCash.Name = "dfBeginningCash";
            this.dfBeginningCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBeginningCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfBeginningCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfBeginningCash.PhoenixUIControl.ObjectId = 10;
            this.dfBeginningCash.PhoenixUIControl.XmlTag = "ClosingCash";
            this.dfBeginningCash.PreviousValue = null;
            this.dfBeginningCash.Size = new System.Drawing.Size(112, 16);
            this.dfBeginningCash.TabIndex = 1;
            // 
            // lblBeginningCash
            // 
            this.lblBeginningCash.AutoEllipsis = true;
            this.lblBeginningCash.Location = new System.Drawing.Point(4, 13);
            this.lblBeginningCash.Name = "lblBeginningCash";
            this.lblBeginningCash.PhoenixUIControl.ObjectId = 10;
            this.lblBeginningCash.Size = new System.Drawing.Size(168, 16);
            this.lblBeginningCash.TabIndex = 0;
            this.lblBeginningCash.Text = "Beginning Cash:";
            // 
            // gbTellerPosition
            // 
            this.gbTellerPosition.Controls.Add(this.dfSavedTime);
            this.gbTellerPosition.Controls.Add(this.dfSavedDateTime);
            this.gbTellerPosition.Controls.Add(this.lblClosedDateTime);
            this.gbTellerPosition.Controls.Add(this.lblPostingDate);
            this.gbTellerPosition.Controls.Add(this.dfDescription);
            this.gbTellerPosition.Controls.Add(this.lblDescription);
            this.gbTellerPosition.Location = new System.Drawing.Point(0, 0);
            this.gbTellerPosition.Name = "gbTellerPosition";
            this.gbTellerPosition.Size = new System.Drawing.Size(680, 56);
            this.gbTellerPosition.TabIndex = 0;
            this.gbTellerPosition.TabStop = false;
            this.gbTellerPosition.Text = "Teller Position";
            // 
            // dfSavedTime
            // 
            this.dfSavedTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfSavedTime.Location = new System.Drawing.Point(460, 36);
            this.dfSavedTime.Multiline = true;
            this.dfSavedTime.Name = "dfSavedTime";
            this.dfSavedTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfSavedTime.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSavedTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
            this.dfSavedTime.PhoenixUIControl.ObjectId = 38;
            this.dfSavedTime.PhoenixUIControl.XmlTag = "SavedDateTime";
            this.dfSavedTime.PreviousValue = null;
            this.dfSavedTime.Size = new System.Drawing.Size(116, 16);
            this.dfSavedTime.TabIndex = 5;
            this.dfSavedTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dfSavedDateTime
            // 
            this.dfSavedDateTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfSavedDateTime.Location = new System.Drawing.Point(460, 16);
            this.dfSavedDateTime.Multiline = true;
            this.dfSavedDateTime.Name = "dfSavedDateTime";
            this.dfSavedDateTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfSavedDateTime.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSavedDateTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfSavedDateTime.PhoenixUIControl.ObjectId = 9;
            this.dfSavedDateTime.PhoenixUIControl.XmlTag = "ClosedDt";
            this.dfSavedDateTime.PreviousValue = null;
            this.dfSavedDateTime.Size = new System.Drawing.Size(68, 16);
            this.dfSavedDateTime.TabIndex = 3;
            this.dfSavedDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClosedDateTime
            // 
            this.lblClosedDateTime.AutoEllipsis = true;
            this.lblClosedDateTime.Location = new System.Drawing.Point(348, 36);
            this.lblClosedDateTime.Name = "lblClosedDateTime";
            this.lblClosedDateTime.PhoenixUIControl.ObjectId = 38;
            this.lblClosedDateTime.Size = new System.Drawing.Size(100, 16);
            this.lblClosedDateTime.TabIndex = 4;
            this.lblClosedDateTime.Text = "Closed Date/Time:";
            // 
            // lblPostingDate
            // 
            this.lblPostingDate.AutoEllipsis = true;
            this.lblPostingDate.Location = new System.Drawing.Point(348, 16);
            this.lblPostingDate.Name = "lblPostingDate";
            this.lblPostingDate.PhoenixUIControl.ObjectId = 9;
            this.lblPostingDate.Size = new System.Drawing.Size(100, 16);
            this.lblPostingDate.TabIndex = 2;
            this.lblPostingDate.Text = "Posting Date:";
            // 
            // dfDescription
            // 
            this.dfDescription.Location = new System.Drawing.Point(76, 12);
            this.dfDescription.Name = "dfDescription";
            this.dfDescription.PhoenixUIControl.ObjectId = 8;
            this.dfDescription.PreviousValue = null;
            this.dfDescription.Size = new System.Drawing.Size(208, 20);
            this.dfDescription.TabIndex = 1;
            this.dfDescription.TextChanged += new System.EventHandler(this.dfDescription_TextChanged);
            this.dfDescription.Validating += new System.ComponentModel.CancelEventHandler(this.dfDescription_Validating);
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(4, 16);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 8;
            this.lblDescription.Size = new System.Drawing.Size(64, 16);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "Description:";
            // 
            // Name1
            // 
            this.Name1.Controls.Add(this.gbExternals);
            this.Name1.Controls.Add(this.gbExchangeandAdvances);
            this.Name1.Controls.Add(this.gbWithdrawalsandAdvances);
            this.Name1.Controls.Add(this.gbMiscellaneous);
            this.Name1.Controls.Add(this.gbPurchasesandPayments);
            this.Name1.Location = new System.Drawing.Point(4, 22);
            this.Name1.MLInfo = controlInfo2;
            this.Name1.Name = "Name1";
            this.Name1.Size = new System.Drawing.Size(682, 422);
            this.Name1.TabIndex = 1;
            this.Name1.Text = "P&urchases and Exchanges";
            // 
            // gbExternals
            // 
            this.gbExternals.Controls.Add(this.dfExtDebits);
            this.gbExternals.Controls.Add(this.dfExtCredits);
            this.gbExternals.Controls.Add(this.lblExtDebits);
            this.gbExternals.Controls.Add(this.lblExtCredits);
            this.gbExternals.Location = new System.Drawing.Point(344, 328);
            this.gbExternals.Name = "gbExternals";
            this.gbExternals.Size = new System.Drawing.Size(336, 88);
            this.gbExternals.TabIndex = 4;
            this.gbExternals.TabStop = false;
            this.gbExternals.Text = "Externals";
            // 
            // dfExtDebits
            // 
            this.dfExtDebits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfExtDebits.Location = new System.Drawing.Point(216, 36);
            this.dfExtDebits.Multiline = true;
            this.dfExtDebits.Name = "dfExtDebits";
            this.dfExtDebits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfExtDebits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfExtDebits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfExtDebits.PhoenixUIControl.ObjectId = 99;
            this.dfExtDebits.PhoenixUIControl.XmlTag = "ExtDebits";
            this.dfExtDebits.PreviousValue = null;
            this.dfExtDebits.Size = new System.Drawing.Size(112, 16);
            this.dfExtDebits.TabIndex = 3;
            // 
            // dfExtCredits
            // 
            this.dfExtCredits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfExtCredits.Location = new System.Drawing.Point(216, 14);
            this.dfExtCredits.Multiline = true;
            this.dfExtCredits.Name = "dfExtCredits";
            this.dfExtCredits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfExtCredits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfExtCredits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfExtCredits.PhoenixUIControl.ObjectId = 98;
            this.dfExtCredits.PhoenixUIControl.XmlTag = "ExtCredits";
            this.dfExtCredits.PreviousValue = null;
            this.dfExtCredits.Size = new System.Drawing.Size(112, 16);
            this.dfExtCredits.TabIndex = 1;
            // 
            // lblExtDebits
            // 
            this.lblExtDebits.AutoEllipsis = true;
            this.lblExtDebits.Location = new System.Drawing.Point(4, 40);
            this.lblExtDebits.Name = "lblExtDebits";
            this.lblExtDebits.Size = new System.Drawing.Size(100, 20);
            this.lblExtDebits.TabIndex = 2;
            this.lblExtDebits.Text = "External Debits:";
            // 
            // lblExtCredits
            // 
            this.lblExtCredits.AutoEllipsis = true;
            this.lblExtCredits.Location = new System.Drawing.Point(4, 16);
            this.lblExtCredits.Name = "lblExtCredits";
            this.lblExtCredits.Size = new System.Drawing.Size(100, 20);
            this.lblExtCredits.TabIndex = 0;
            this.lblExtCredits.Text = "External Credits:";
            // 
            // gbExchangeandAdvances
            // 
            this.gbExchangeandAdvances.Controls.Add(this.dfCurrencyExchange);
            this.gbExchangeandAdvances.Controls.Add(this.lblCurrencyExchanges);
            this.gbExchangeandAdvances.Controls.Add(this.dfCCAdvance);
            this.gbExchangeandAdvances.Controls.Add(this.lblCreditCardCashAdvance);
            this.gbExchangeandAdvances.Controls.Add(this.dfIBondExch);
            this.gbExchangeandAdvances.Controls.Add(this.lblIBondsCashed);
            this.gbExchangeandAdvances.Controls.Add(this.dfBondExch);
            this.gbExchangeandAdvances.Controls.Add(this.lblSavingsBondsCashed);
            this.gbExchangeandAdvances.Controls.Add(this.dfTrChksExch);
            this.gbExchangeandAdvances.Controls.Add(this.lblTravelersChecksCashed);
            this.gbExchangeandAdvances.Location = new System.Drawing.Point(344, 56);
            this.gbExchangeandAdvances.Name = "gbExchangeandAdvances";
            this.gbExchangeandAdvances.Size = new System.Drawing.Size(336, 116);
            this.gbExchangeandAdvances.TabIndex = 2;
            this.gbExchangeandAdvances.TabStop = false;
            this.gbExchangeandAdvances.Text = "Exchange and Advances";
            // 
            // dfCurrencyExchange
            // 
            this.dfCurrencyExchange.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCurrencyExchange.Location = new System.Drawing.Point(216, 96);
            this.dfCurrencyExchange.Multiline = true;
            this.dfCurrencyExchange.Name = "dfCurrencyExchange";
            this.dfCurrencyExchange.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCurrencyExchange.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCurrencyExchange.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCurrencyExchange.PhoenixUIControl.ObjectId = 85;
            this.dfCurrencyExchange.PhoenixUIControl.XmlTag = "CurrencyExch";
            this.dfCurrencyExchange.PreviousValue = null;
            this.dfCurrencyExchange.Size = new System.Drawing.Size(112, 16);
            this.dfCurrencyExchange.TabIndex = 9;
            // 
            // lblCurrencyExchanges
            // 
            this.lblCurrencyExchanges.AutoEllipsis = true;
            this.lblCurrencyExchanges.Location = new System.Drawing.Point(4, 96);
            this.lblCurrencyExchanges.Name = "lblCurrencyExchanges";
            this.lblCurrencyExchanges.PhoenixUIControl.ObjectId = 85;
            this.lblCurrencyExchanges.Size = new System.Drawing.Size(180, 16);
            this.lblCurrencyExchanges.TabIndex = 8;
            this.lblCurrencyExchanges.Text = "Currency Orders & Exchanges:";
            // 
            // dfCCAdvance
            // 
            this.dfCCAdvance.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCCAdvance.Location = new System.Drawing.Point(216, 76);
            this.dfCCAdvance.Multiline = true;
            this.dfCCAdvance.Name = "dfCCAdvance";
            this.dfCCAdvance.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCCAdvance.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCCAdvance.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCCAdvance.PhoenixUIControl.ObjectId = 69;
            this.dfCCAdvance.PhoenixUIControl.XmlTag = "CcAdvances";
            this.dfCCAdvance.PreviousValue = null;
            this.dfCCAdvance.Size = new System.Drawing.Size(112, 16);
            this.dfCCAdvance.TabIndex = 7;
            // 
            // lblCreditCardCashAdvance
            // 
            this.lblCreditCardCashAdvance.AutoEllipsis = true;
            this.lblCreditCardCashAdvance.Location = new System.Drawing.Point(4, 76);
            this.lblCreditCardCashAdvance.Name = "lblCreditCardCashAdvance";
            this.lblCreditCardCashAdvance.PhoenixUIControl.ObjectId = 69;
            this.lblCreditCardCashAdvance.Size = new System.Drawing.Size(177, 16);
            this.lblCreditCardCashAdvance.TabIndex = 6;
            this.lblCreditCardCashAdvance.Text = "Credit Card Cash Advance:";
            // 
            // dfIBondExch
            // 
            this.dfIBondExch.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfIBondExch.Location = new System.Drawing.Point(216, 56);
            this.dfIBondExch.Multiline = true;
            this.dfIBondExch.Name = "dfIBondExch";
            this.dfIBondExch.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfIBondExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfIBondExch.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfIBondExch.PhoenixUIControl.ObjectId = 75;
            this.dfIBondExch.PhoenixUIControl.XmlTag = "IBondExch";
            this.dfIBondExch.PreviousValue = null;
            this.dfIBondExch.Size = new System.Drawing.Size(112, 16);
            this.dfIBondExch.TabIndex = 5;
            // 
            // lblIBondsCashed
            // 
            this.lblIBondsCashed.AutoEllipsis = true;
            this.lblIBondsCashed.Location = new System.Drawing.Point(4, 56);
            this.lblIBondsCashed.Name = "lblIBondsCashed";
            this.lblIBondsCashed.PhoenixUIControl.ObjectId = 75;
            this.lblIBondsCashed.Size = new System.Drawing.Size(177, 16);
            this.lblIBondsCashed.TabIndex = 4;
            this.lblIBondsCashed.Text = "I Bonds Cashed:";
            // 
            // dfBondExch
            // 
            this.dfBondExch.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBondExch.Location = new System.Drawing.Point(216, 36);
            this.dfBondExch.Multiline = true;
            this.dfBondExch.Name = "dfBondExch";
            this.dfBondExch.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBondExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfBondExch.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfBondExch.PhoenixUIControl.ObjectId = 68;
            this.dfBondExch.PhoenixUIControl.XmlTag = "BondExch";
            this.dfBondExch.PreviousValue = null;
            this.dfBondExch.Size = new System.Drawing.Size(112, 16);
            this.dfBondExch.TabIndex = 3;
            // 
            // lblSavingsBondsCashed
            // 
            this.lblSavingsBondsCashed.AutoEllipsis = true;
            this.lblSavingsBondsCashed.Location = new System.Drawing.Point(4, 36);
            this.lblSavingsBondsCashed.Name = "lblSavingsBondsCashed";
            this.lblSavingsBondsCashed.PhoenixUIControl.ObjectId = 68;
            this.lblSavingsBondsCashed.Size = new System.Drawing.Size(177, 16);
            this.lblSavingsBondsCashed.TabIndex = 2;
            this.lblSavingsBondsCashed.Text = "Savings Bonds Cashed:";
            // 
            // dfTrChksExch
            // 
            this.dfTrChksExch.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTrChksExch.Location = new System.Drawing.Point(216, 16);
            this.dfTrChksExch.Multiline = true;
            this.dfTrChksExch.Name = "dfTrChksExch";
            this.dfTrChksExch.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTrChksExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTrChksExch.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTrChksExch.PhoenixUIControl.ObjectId = 67;
            this.dfTrChksExch.PhoenixUIControl.XmlTag = "TcExch";
            this.dfTrChksExch.PreviousValue = null;
            this.dfTrChksExch.Size = new System.Drawing.Size(112, 16);
            this.dfTrChksExch.TabIndex = 1;
            // 
            // lblTravelersChecksCashed
            // 
            this.lblTravelersChecksCashed.AutoEllipsis = true;
            this.lblTravelersChecksCashed.Location = new System.Drawing.Point(4, 16);
            this.lblTravelersChecksCashed.Name = "lblTravelersChecksCashed";
            this.lblTravelersChecksCashed.PhoenixUIControl.ObjectId = 67;
            this.lblTravelersChecksCashed.Size = new System.Drawing.Size(177, 16);
            this.lblTravelersChecksCashed.TabIndex = 0;
            this.lblTravelersChecksCashed.Text = "Traveler\'s Checks Cashed:";
            // 
            // gbWithdrawalsandAdvances
            // 
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfOutFrWire);
            this.gbWithdrawalsandAdvances.Controls.Add(this.pLabelStandard2);
            this.gbWithdrawalsandAdvances.Controls.Add(this.pPanelWd);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfTotalWithdrawals);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblTotals_Dup2);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfLnAdvances);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblLoanAdvances);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfAccountCloseouts);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblAccountCloseouts);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfTfrDebits);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblTransferWithdrawals);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfOthWd);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblWithdrawals);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfOnUsChksCashed);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblOnUsChecksCashed);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfChksCashedWd);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblChecksCashed);
            this.gbWithdrawalsandAdvances.Controls.Add(this.dfOutWire);
            this.gbWithdrawalsandAdvances.Controls.Add(this.lblOutgoingWireTransfers);
            this.gbWithdrawalsandAdvances.Location = new System.Drawing.Point(0, 56);
            this.gbWithdrawalsandAdvances.Name = "gbWithdrawalsandAdvances";
            this.gbWithdrawalsandAdvances.Size = new System.Drawing.Size(340, 204);
            this.gbWithdrawalsandAdvances.TabIndex = 0;
            this.gbWithdrawalsandAdvances.TabStop = false;
            this.gbWithdrawalsandAdvances.Text = "Withdrawals and Advances";
            // 
            // dfOutFrWire
            // 
            this.dfOutFrWire.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOutFrWire.Location = new System.Drawing.Point(220, 136);
            this.dfOutFrWire.Multiline = true;
            this.dfOutFrWire.Name = "dfOutFrWire";
            this.dfOutFrWire.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOutFrWire.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOutFrWire.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOutFrWire.PhoenixUIControl.ObjectId = 94;
            this.dfOutFrWire.PhoenixUIControl.XmlTag = "OutFrWire";
            this.dfOutFrWire.PreviousValue = null;
            this.dfOutFrWire.Size = new System.Drawing.Size(112, 16);
            this.dfOutFrWire.TabIndex = 13;
            // 
            // pLabelStandard2
            // 
            this.pLabelStandard2.AutoEllipsis = true;
            this.pLabelStandard2.Location = new System.Drawing.Point(4, 136);
            this.pLabelStandard2.Name = "pLabelStandard2";
            this.pLabelStandard2.PhoenixUIControl.ObjectId = 94;
            this.pLabelStandard2.Size = new System.Drawing.Size(160, 16);
            this.pLabelStandard2.TabIndex = 12;
            this.pLabelStandard2.Text = "Outgoing Fgn Wire Transfers:";
            // 
            // pPanelWd
            // 
            this.pPanelWd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanelWd.Location = new System.Drawing.Point(208, 176);
            this.pPanelWd.Name = "pPanelWd";
            this.pPanelWd.Size = new System.Drawing.Size(124, 1);
            this.pPanelWd.TabIndex = 16;
            this.pPanelWd.TabStop = true;
            // 
            // dfTotalWithdrawals
            // 
            this.dfTotalWithdrawals.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalWithdrawals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalWithdrawals.Location = new System.Drawing.Point(208, 184);
            this.dfTotalWithdrawals.Multiline = true;
            this.dfTotalWithdrawals.Name = "dfTotalWithdrawals";
            this.dfTotalWithdrawals.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalWithdrawals.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalWithdrawals.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalWithdrawals.PhoenixUIControl.ObjectId = 30;
            this.dfTotalWithdrawals.PhoenixUIControl.XmlTag = "TotalWithdrawals";
            this.dfTotalWithdrawals.PreviousValue = null;
            this.dfTotalWithdrawals.Size = new System.Drawing.Size(124, 16);
            this.dfTotalWithdrawals.TabIndex = 18;
            // 
            // lblTotals_Dup2
            // 
            this.lblTotals_Dup2.AutoEllipsis = true;
            this.lblTotals_Dup2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotals_Dup2.Location = new System.Drawing.Point(4, 184);
            this.lblTotals_Dup2.Name = "lblTotals_Dup2";
            this.lblTotals_Dup2.PhoenixUIControl.ObjectId = 30;
            this.lblTotals_Dup2.Size = new System.Drawing.Size(132, 16);
            this.lblTotals_Dup2.TabIndex = 17;
            this.lblTotals_Dup2.Text = "Totals:";
            this.lblTotals_Dup2.Visible = false;
            // 
            // dfLnAdvances
            // 
            this.dfLnAdvances.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLnAdvances.Location = new System.Drawing.Point(220, 156);
            this.dfLnAdvances.Multiline = true;
            this.dfLnAdvances.Name = "dfLnAdvances";
            this.dfLnAdvances.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLnAdvances.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfLnAdvances.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfLnAdvances.PhoenixUIControl.ObjectId = 55;
            this.dfLnAdvances.PhoenixUIControl.XmlTag = "LnAdvances";
            this.dfLnAdvances.PreviousValue = null;
            this.dfLnAdvances.Size = new System.Drawing.Size(112, 16);
            this.dfLnAdvances.TabIndex = 15;
            // 
            // lblLoanAdvances
            // 
            this.lblLoanAdvances.AutoEllipsis = true;
            this.lblLoanAdvances.Location = new System.Drawing.Point(4, 156);
            this.lblLoanAdvances.Name = "lblLoanAdvances";
            this.lblLoanAdvances.PhoenixUIControl.ObjectId = 55;
            this.lblLoanAdvances.Size = new System.Drawing.Size(160, 16);
            this.lblLoanAdvances.TabIndex = 14;
            this.lblLoanAdvances.Text = "Loan Advances:";
            // 
            // dfAccountCloseouts
            // 
            this.dfAccountCloseouts.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAccountCloseouts.Location = new System.Drawing.Point(220, 76);
            this.dfAccountCloseouts.Multiline = true;
            this.dfAccountCloseouts.Name = "dfAccountCloseouts";
            this.dfAccountCloseouts.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAccountCloseouts.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAccountCloseouts.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAccountCloseouts.PhoenixUIControl.XmlTag = "AcctCloseouts";
            this.dfAccountCloseouts.PreviousValue = null;
            this.dfAccountCloseouts.Size = new System.Drawing.Size(112, 16);
            this.dfAccountCloseouts.TabIndex = 7;
            // 
            // lblAccountCloseouts
            // 
            this.lblAccountCloseouts.AutoEllipsis = true;
            this.lblAccountCloseouts.Location = new System.Drawing.Point(4, 76);
            this.lblAccountCloseouts.Name = "lblAccountCloseouts";
            this.lblAccountCloseouts.Size = new System.Drawing.Size(160, 16);
            this.lblAccountCloseouts.TabIndex = 6;
            this.lblAccountCloseouts.Text = "Account Closeouts:";
            // 
            // dfTfrDebits
            // 
            this.dfTfrDebits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTfrDebits.Location = new System.Drawing.Point(220, 96);
            this.dfTfrDebits.Multiline = true;
            this.dfTfrDebits.Name = "dfTfrDebits";
            this.dfTfrDebits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTfrDebits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTfrDebits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTfrDebits.PhoenixUIControl.XmlTag = "TfrDebits";
            this.dfTfrDebits.PreviousValue = null;
            this.dfTfrDebits.Size = new System.Drawing.Size(112, 16);
            this.dfTfrDebits.TabIndex = 9;
            // 
            // lblTransferWithdrawals
            // 
            this.lblTransferWithdrawals.AutoEllipsis = true;
            this.lblTransferWithdrawals.Location = new System.Drawing.Point(4, 96);
            this.lblTransferWithdrawals.Name = "lblTransferWithdrawals";
            this.lblTransferWithdrawals.Size = new System.Drawing.Size(160, 16);
            this.lblTransferWithdrawals.TabIndex = 8;
            this.lblTransferWithdrawals.Text = "Transfer Withdrawals:";
            // 
            // dfOthWd
            // 
            this.dfOthWd.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOthWd.Location = new System.Drawing.Point(220, 56);
            this.dfOthWd.Multiline = true;
            this.dfOthWd.Name = "dfOthWd";
            this.dfOthWd.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOthWd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOthWd.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOthWd.PhoenixUIControl.ObjectId = 32;
            this.dfOthWd.PhoenixUIControl.XmlTag = "OthWd";
            this.dfOthWd.PreviousValue = null;
            this.dfOthWd.Size = new System.Drawing.Size(112, 16);
            this.dfOthWd.TabIndex = 5;
            // 
            // lblWithdrawals
            // 
            this.lblWithdrawals.AutoEllipsis = true;
            this.lblWithdrawals.Location = new System.Drawing.Point(4, 56);
            this.lblWithdrawals.Name = "lblWithdrawals";
            this.lblWithdrawals.PhoenixUIControl.ObjectId = 32;
            this.lblWithdrawals.Size = new System.Drawing.Size(160, 16);
            this.lblWithdrawals.TabIndex = 4;
            this.lblWithdrawals.Text = "Withdrawals:";
            // 
            // dfOnUsChksCashed
            // 
            this.dfOnUsChksCashed.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChksCashed.Location = new System.Drawing.Point(220, 36);
            this.dfOnUsChksCashed.Multiline = true;
            this.dfOnUsChksCashed.Name = "dfOnUsChksCashed";
            this.dfOnUsChksCashed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOnUsChksCashed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsChksCashed.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOnUsChksCashed.PhoenixUIControl.ObjectId = 74;
            this.dfOnUsChksCashed.PhoenixUIControl.XmlTag = "OnUsChksCashed";
            this.dfOnUsChksCashed.PreviousValue = null;
            this.dfOnUsChksCashed.Size = new System.Drawing.Size(112, 16);
            this.dfOnUsChksCashed.TabIndex = 3;
            // 
            // lblOnUsChecksCashed
            // 
            this.lblOnUsChecksCashed.AutoEllipsis = true;
            this.lblOnUsChecksCashed.Location = new System.Drawing.Point(4, 36);
            this.lblOnUsChecksCashed.Name = "lblOnUsChecksCashed";
            this.lblOnUsChecksCashed.PhoenixUIControl.ObjectId = 74;
            this.lblOnUsChecksCashed.Size = new System.Drawing.Size(160, 16);
            this.lblOnUsChecksCashed.TabIndex = 2;
            this.lblOnUsChecksCashed.Text = "On-Us Checks Cashed:";
            // 
            // dfChksCashedWd
            // 
            this.dfChksCashedWd.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksCashedWd.Location = new System.Drawing.Point(220, 16);
            this.dfChksCashedWd.Multiline = true;
            this.dfChksCashedWd.Name = "dfChksCashedWd";
            this.dfChksCashedWd.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChksCashedWd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfChksCashedWd.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfChksCashedWd.PhoenixUIControl.ObjectId = 33;
            this.dfChksCashedWd.PhoenixUIControl.XmlTag = "ChksCashedWd";
            this.dfChksCashedWd.PreviousValue = null;
            this.dfChksCashedWd.Size = new System.Drawing.Size(112, 16);
            this.dfChksCashedWd.TabIndex = 1;
            // 
            // lblChecksCashed
            // 
            this.lblChecksCashed.AutoEllipsis = true;
            this.lblChecksCashed.Location = new System.Drawing.Point(4, 16);
            this.lblChecksCashed.Name = "lblChecksCashed";
            this.lblChecksCashed.PhoenixUIControl.ObjectId = 33;
            this.lblChecksCashed.Size = new System.Drawing.Size(160, 16);
            this.lblChecksCashed.TabIndex = 0;
            this.lblChecksCashed.Text = "Checks Cashed:";
            // 
            // dfOutWire
            // 
            this.dfOutWire.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOutWire.Location = new System.Drawing.Point(220, 116);
            this.dfOutWire.Multiline = true;
            this.dfOutWire.Name = "dfOutWire";
            this.dfOutWire.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOutWire.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOutWire.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOutWire.PhoenixUIControl.ObjectId = 45;
            this.dfOutWire.PhoenixUIControl.XmlTag = "OutWire";
            this.dfOutWire.PreviousValue = null;
            this.dfOutWire.Size = new System.Drawing.Size(112, 16);
            this.dfOutWire.TabIndex = 11;
            // 
            // lblOutgoingWireTransfers
            // 
            this.lblOutgoingWireTransfers.AutoEllipsis = true;
            this.lblOutgoingWireTransfers.Location = new System.Drawing.Point(4, 116);
            this.lblOutgoingWireTransfers.Name = "lblOutgoingWireTransfers";
            this.lblOutgoingWireTransfers.PhoenixUIControl.ObjectId = 45;
            this.lblOutgoingWireTransfers.Size = new System.Drawing.Size(160, 16);
            this.lblOutgoingWireTransfers.TabIndex = 10;
            this.lblOutgoingWireTransfers.Text = "Outgoing Dom Wire Transfers:";
            // 
            // gbMiscellaneous
            // 
            this.gbMiscellaneous.Controls.Add(this.dfCashWd);
            this.gbMiscellaneous.Controls.Add(this.dfBatchChkAmt);
            this.gbMiscellaneous.Controls.Add(this.dfOnUsExch);
            this.gbMiscellaneous.Controls.Add(this.dfChkExch);
            this.gbMiscellaneous.Controls.Add(this.dfFCExch);
            this.gbMiscellaneous.Controls.Add(this.dfFCPurch);
            this.gbMiscellaneous.Controls.Add(this.dfDraftPurch);
            this.gbMiscellaneous.Controls.Add(this.dfSequenceNo);
            this.gbMiscellaneous.Controls.Add(this.lblTotalOfTransactions);
            this.gbMiscellaneous.Controls.Add(this.dfCtrTriggered);
            this.gbMiscellaneous.Controls.Add(this.lblCTRsTriggered);
            this.gbMiscellaneous.Controls.Add(this.dfCCAmt);
            this.gbMiscellaneous.Controls.Add(this.lblChargesCollected);
            this.gbMiscellaneous.Controls.Add(this.dfGlDebits);
            this.gbMiscellaneous.Controls.Add(this.lblGeneralLedgerDebits);
            this.gbMiscellaneous.Controls.Add(this.dfGlCredits);
            this.gbMiscellaneous.Controls.Add(this.lblGeneralLedgerCredits);
            this.gbMiscellaneous.Controls.Add(this.dfCashSold);
            this.gbMiscellaneous.Controls.Add(this.lblCashSold);
            this.gbMiscellaneous.Controls.Add(this.dfCashBought);
            this.gbMiscellaneous.Controls.Add(this.lblCashBought);
            this.gbMiscellaneous.Location = new System.Drawing.Point(0, 260);
            this.gbMiscellaneous.Name = "gbMiscellaneous";
            this.gbMiscellaneous.Size = new System.Drawing.Size(340, 156);
            this.gbMiscellaneous.TabIndex = 1;
            this.gbMiscellaneous.TabStop = false;
            this.gbMiscellaneous.Text = "Miscellaneous";
            // 
            // dfCashWd
            // 
            this.dfCashWd.Location = new System.Drawing.Point(619, 88);
            this.dfCashWd.Multiline = true;
            this.dfCashWd.Name = "dfCashWd";
            this.dfCashWd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashWd.PhoenixUIControl.ObjectId = 34;
            this.dfCashWd.PreviousValue = null;
            this.dfCashWd.Size = new System.Drawing.Size(91, 20);
            this.dfCashWd.TabIndex = 15;
            this.dfCashWd.Visible = false;
            // 
            // dfBatchChkAmt
            // 
            this.dfBatchChkAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBatchChkAmt.Location = new System.Drawing.Point(104, 116);
            this.dfBatchChkAmt.Multiline = true;
            this.dfBatchChkAmt.Name = "dfBatchChkAmt";
            this.dfBatchChkAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfBatchChkAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfBatchChkAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfBatchChkAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.dfBatchChkAmt.PhoenixUIControl.ObjectId = 21;
            this.dfBatchChkAmt.PhoenixUIControl.XmlTag = "BatchChkAmt";
            this.dfBatchChkAmt.PreviousValue = null;
            this.dfBatchChkAmt.Size = new System.Drawing.Size(96, 16);
            this.dfBatchChkAmt.TabIndex = 14;
            this.dfBatchChkAmt.Visible = false;
            // 
            // dfOnUsExch
            // 
            this.dfOnUsExch.Location = new System.Drawing.Point(11, 231);
            this.dfOnUsExch.Multiline = true;
            this.dfOnUsExch.Name = "dfOnUsExch";
            this.dfOnUsExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsExch.PhoenixUIControl.ObjectId = 70;
            this.dfOnUsExch.PreviousValue = null;
            this.dfOnUsExch.Size = new System.Drawing.Size(0, 20);
            this.dfOnUsExch.TabIndex = 3;
            // 
            // dfChkExch
            // 
            this.dfChkExch.Location = new System.Drawing.Point(11, 231);
            this.dfChkExch.Multiline = true;
            this.dfChkExch.Name = "dfChkExch";
            this.dfChkExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfChkExch.PhoenixUIControl.ObjectId = 47;
            this.dfChkExch.PreviousValue = null;
            this.dfChkExch.Size = new System.Drawing.Size(0, 20);
            this.dfChkExch.TabIndex = 4;
            // 
            // dfFCExch
            // 
            this.dfFCExch.Location = new System.Drawing.Point(11, 231);
            this.dfFCExch.Multiline = true;
            this.dfFCExch.Name = "dfFCExch";
            this.dfFCExch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfFCExch.PhoenixUIControl.ObjectId = 42;
            this.dfFCExch.PreviousValue = null;
            this.dfFCExch.Size = new System.Drawing.Size(0, 20);
            this.dfFCExch.TabIndex = 5;
            // 
            // dfFCPurch
            // 
            this.dfFCPurch.Location = new System.Drawing.Point(11, 231);
            this.dfFCPurch.Multiline = true;
            this.dfFCPurch.Name = "dfFCPurch";
            this.dfFCPurch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfFCPurch.PhoenixUIControl.ObjectId = 41;
            this.dfFCPurch.PreviousValue = null;
            this.dfFCPurch.Size = new System.Drawing.Size(0, 20);
            this.dfFCPurch.TabIndex = 13;
            // 
            // dfDraftPurch
            // 
            this.dfDraftPurch.Location = new System.Drawing.Point(11, 231);
            this.dfDraftPurch.Multiline = true;
            this.dfDraftPurch.Name = "dfDraftPurch";
            this.dfDraftPurch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDraftPurch.PhoenixUIControl.ObjectId = 43;
            this.dfDraftPurch.PreviousValue = null;
            this.dfDraftPurch.Size = new System.Drawing.Size(0, 20);
            this.dfDraftPurch.TabIndex = 14;
            // 
            // dfSequenceNo
            // 
            this.dfSequenceNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSequenceNo.Location = new System.Drawing.Point(280, 136);
            this.dfSequenceNo.Multiline = true;
            this.dfSequenceNo.Name = "dfSequenceNo";
            this.dfSequenceNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSequenceNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSequenceNo.PhoenixUIControl.ObjectId = 22;
            this.dfSequenceNo.PhoenixUIControl.XmlTag = "NoTrans";
            this.dfSequenceNo.PreviousValue = null;
            this.dfSequenceNo.Size = new System.Drawing.Size(52, 16);
            this.dfSequenceNo.TabIndex = 13;
            // 
            // lblTotalOfTransactions
            // 
            this.lblTotalOfTransactions.AutoEllipsis = true;
            this.lblTotalOfTransactions.Location = new System.Drawing.Point(4, 136);
            this.lblTotalOfTransactions.Name = "lblTotalOfTransactions";
            this.lblTotalOfTransactions.PhoenixUIControl.ObjectId = 22;
            this.lblTotalOfTransactions.Size = new System.Drawing.Size(152, 16);
            this.lblTotalOfTransactions.TabIndex = 12;
            this.lblTotalOfTransactions.Text = "Total # Of Transactions:";
            // 
            // dfCtrTriggered
            // 
            this.dfCtrTriggered.Location = new System.Drawing.Point(280, 116);
            this.dfCtrTriggered.Multiline = true;
            this.dfCtrTriggered.Name = "dfCtrTriggered";
            this.dfCtrTriggered.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCtrTriggered.PhoenixUIControl.ObjectId = 86;
            this.dfCtrTriggered.PhoenixUIControl.XmlTag = "CtrTriggered";
            this.dfCtrTriggered.PreviousValue = null;
            this.dfCtrTriggered.Size = new System.Drawing.Size(52, 16);
            this.dfCtrTriggered.TabIndex = 11;
            // 
            // lblCTRsTriggered
            // 
            this.lblCTRsTriggered.AutoEllipsis = true;
            this.lblCTRsTriggered.Location = new System.Drawing.Point(4, 116);
            this.lblCTRsTriggered.Name = "lblCTRsTriggered";
            this.lblCTRsTriggered.PhoenixUIControl.ObjectId = 86;
            this.lblCTRsTriggered.Size = new System.Drawing.Size(88, 16);
            this.lblCTRsTriggered.TabIndex = 10;
            this.lblCTRsTriggered.Text = "CTRs Triggered:";
            // 
            // dfCCAmt
            // 
            this.dfCCAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCCAmt.Location = new System.Drawing.Point(220, 56);
            this.dfCCAmt.Multiline = true;
            this.dfCCAmt.Name = "dfCCAmt";
            this.dfCCAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCCAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCCAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCCAmt.PhoenixUIControl.ObjectId = 23;
            this.dfCCAmt.PhoenixUIControl.XmlTag = "CcAmt";
            this.dfCCAmt.PreviousValue = null;
            this.dfCCAmt.Size = new System.Drawing.Size(112, 16);
            this.dfCCAmt.TabIndex = 5;
            // 
            // lblChargesCollected
            // 
            this.lblChargesCollected.AutoEllipsis = true;
            this.lblChargesCollected.Location = new System.Drawing.Point(4, 56);
            this.lblChargesCollected.Name = "lblChargesCollected";
            this.lblChargesCollected.PhoenixUIControl.ObjectId = 23;
            this.lblChargesCollected.Size = new System.Drawing.Size(152, 16);
            this.lblChargesCollected.TabIndex = 4;
            this.lblChargesCollected.Text = "Charges Collected:";
            // 
            // dfGlDebits
            // 
            this.dfGlDebits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfGlDebits.Location = new System.Drawing.Point(220, 96);
            this.dfGlDebits.Multiline = true;
            this.dfGlDebits.Name = "dfGlDebits";
            this.dfGlDebits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfGlDebits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfGlDebits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfGlDebits.PhoenixUIControl.ObjectId = 56;
            this.dfGlDebits.PhoenixUIControl.XmlTag = "GlDebits";
            this.dfGlDebits.PreviousValue = null;
            this.dfGlDebits.Size = new System.Drawing.Size(112, 16);
            this.dfGlDebits.TabIndex = 9;
            // 
            // lblGeneralLedgerDebits
            // 
            this.lblGeneralLedgerDebits.AutoEllipsis = true;
            this.lblGeneralLedgerDebits.Location = new System.Drawing.Point(4, 96);
            this.lblGeneralLedgerDebits.Name = "lblGeneralLedgerDebits";
            this.lblGeneralLedgerDebits.PhoenixUIControl.ObjectId = 56;
            this.lblGeneralLedgerDebits.Size = new System.Drawing.Size(152, 16);
            this.lblGeneralLedgerDebits.TabIndex = 8;
            this.lblGeneralLedgerDebits.Text = "General Ledger Debits:";
            // 
            // dfGlCredits
            // 
            this.dfGlCredits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfGlCredits.Location = new System.Drawing.Point(220, 36);
            this.dfGlCredits.Multiline = true;
            this.dfGlCredits.Name = "dfGlCredits";
            this.dfGlCredits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfGlCredits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfGlCredits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfGlCredits.PhoenixUIControl.ObjectId = 57;
            this.dfGlCredits.PhoenixUIControl.XmlTag = "GlCredits";
            this.dfGlCredits.PreviousValue = null;
            this.dfGlCredits.Size = new System.Drawing.Size(112, 16);
            this.dfGlCredits.TabIndex = 3;
            // 
            // lblGeneralLedgerCredits
            // 
            this.lblGeneralLedgerCredits.AutoEllipsis = true;
            this.lblGeneralLedgerCredits.Location = new System.Drawing.Point(4, 36);
            this.lblGeneralLedgerCredits.Name = "lblGeneralLedgerCredits";
            this.lblGeneralLedgerCredits.PhoenixUIControl.ObjectId = 57;
            this.lblGeneralLedgerCredits.Size = new System.Drawing.Size(152, 16);
            this.lblGeneralLedgerCredits.TabIndex = 2;
            this.lblGeneralLedgerCredits.Text = "General Ledger Credits:";
            // 
            // dfCashSold
            // 
            this.dfCashSold.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashSold.Location = new System.Drawing.Point(220, 76);
            this.dfCashSold.Multiline = true;
            this.dfCashSold.Name = "dfCashSold";
            this.dfCashSold.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashSold.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashSold.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashSold.PhoenixUIControl.ObjectId = 59;
            this.dfCashSold.PhoenixUIControl.XmlTag = "CashSold";
            this.dfCashSold.PreviousValue = null;
            this.dfCashSold.Size = new System.Drawing.Size(112, 16);
            this.dfCashSold.TabIndex = 7;
            // 
            // lblCashSold
            // 
            this.lblCashSold.AutoEllipsis = true;
            this.lblCashSold.Location = new System.Drawing.Point(4, 76);
            this.lblCashSold.Name = "lblCashSold";
            this.lblCashSold.PhoenixUIControl.ObjectId = 59;
            this.lblCashSold.Size = new System.Drawing.Size(88, 16);
            this.lblCashSold.TabIndex = 6;
            this.lblCashSold.Text = "Cash Sold:";
            // 
            // dfCashBought
            // 
            this.dfCashBought.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashBought.Location = new System.Drawing.Point(220, 16);
            this.dfCashBought.Multiline = true;
            this.dfCashBought.Name = "dfCashBought";
            this.dfCashBought.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashBought.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashBought.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashBought.PhoenixUIControl.ObjectId = 58;
            this.dfCashBought.PhoenixUIControl.XmlTag = "CashBought";
            this.dfCashBought.PreviousValue = null;
            this.dfCashBought.Size = new System.Drawing.Size(112, 16);
            this.dfCashBought.TabIndex = 1;
            // 
            // lblCashBought
            // 
            this.lblCashBought.AutoEllipsis = true;
            this.lblCashBought.Location = new System.Drawing.Point(4, 16);
            this.lblCashBought.Name = "lblCashBought";
            this.lblCashBought.PhoenixUIControl.ObjectId = 58;
            this.lblCashBought.Size = new System.Drawing.Size(152, 16);
            this.lblCashBought.TabIndex = 0;
            this.lblCashBought.Text = "Cash Bought:";
            // 
            // gbPurchasesandPayments
            // 
            this.gbPurchasesandPayments.Controls.Add(this.dfNoInvItemReturn);
            this.gbPurchasesandPayments.Controls.Add(this.dfNoInvItemPurch);
            this.gbPurchasesandPayments.Controls.Add(this.dfTabTitle2);
            this.gbPurchasesandPayments.Controls.Add(this.dfTabTitle1);
            this.gbPurchasesandPayments.Controls.Add(this.dfTabTitle0);
            this.gbPurchasesandPayments.Controls.Add(this.dfTtlPmts);
            this.gbPurchasesandPayments.Controls.Add(this.lblTreasuryTaxLoanPayments);
            this.gbPurchasesandPayments.Controls.Add(this.dfUtilPmts);
            this.gbPurchasesandPayments.Controls.Add(this.lblUtilityPayments);
            this.gbPurchasesandPayments.Controls.Add(this.dfInvItemReturnAmt);
            this.gbPurchasesandPayments.Controls.Add(this.lblInventoryItemReturned);
            this.gbPurchasesandPayments.Controls.Add(this.dfInvItemPurchAmt);
            this.gbPurchasesandPayments.Controls.Add(this.lblInventoryItemPurchased);
            this.gbPurchasesandPayments.Controls.Add(this.dfTrChksPurch);
            this.gbPurchasesandPayments.Controls.Add(this.lblTravelersChecksPurchased);
            this.gbPurchasesandPayments.Controls.Add(this.dfMoneyOrder);
            this.gbPurchasesandPayments.Controls.Add(this.lblMoneyOrdersPurchased);
            this.gbPurchasesandPayments.Controls.Add(this.dfOfficialChks);
            this.gbPurchasesandPayments.Controls.Add(this.lblOfficialChecksPurchased);
            this.gbPurchasesandPayments.Location = new System.Drawing.Point(344, 172);
            this.gbPurchasesandPayments.Name = "gbPurchasesandPayments";
            this.gbPurchasesandPayments.Size = new System.Drawing.Size(336, 156);
            this.gbPurchasesandPayments.TabIndex = 3;
            this.gbPurchasesandPayments.TabStop = false;
            this.gbPurchasesandPayments.Text = "Purchases and Payments";
            // 
            // dfNoInvItemReturn
            // 
            this.dfNoInvItemReturn.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItemReturn.Location = new System.Drawing.Point(142, 98);
            this.dfNoInvItemReturn.Name = "dfNoInvItemReturn";
            this.dfNoInvItemReturn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItemReturn.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoInvItemReturn.PhoenixUIControl.ObjectId = 76;
            this.dfNoInvItemReturn.PhoenixUIControl.XmlTag = "NoInvItemReturn";
            this.dfNoInvItemReturn.PreviousValue = null;
            this.dfNoInvItemReturn.Size = new System.Drawing.Size(59, 13);
            this.dfNoInvItemReturn.TabIndex = 16;
            // 
            // dfNoInvItemPurch
            // 
            this.dfNoInvItemPurch.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItemPurch.Location = new System.Drawing.Point(142, 78);
            this.dfNoInvItemPurch.Name = "dfNoInvItemPurch";
            this.dfNoInvItemPurch.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoInvItemPurch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoInvItemPurch.PhoenixUIControl.ObjectId = 63;
            this.dfNoInvItemPurch.PhoenixUIControl.XmlTag = "NoInvItemPurch";
            this.dfNoInvItemPurch.PreviousValue = null;
            this.dfNoInvItemPurch.Size = new System.Drawing.Size(59, 13);
            this.dfNoInvItemPurch.TabIndex = 15;
            // 
            // dfTabTitle2
            // 
            this.dfTabTitle2.Location = new System.Drawing.Point(40, 200);
            this.dfTabTitle2.Multiline = true;
            this.dfTabTitle2.Name = "dfTabTitle2";
            this.dfTabTitle2.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTabTitle2.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.dfTabTitle2.PhoenixUIControl.ObjectId = 89;
            this.dfTabTitle2.PreviousValue = null;
            this.dfTabTitle2.Size = new System.Drawing.Size(8, 20);
            this.dfTabTitle2.TabIndex = 14;
            // 
            // dfTabTitle1
            // 
            this.dfTabTitle1.Location = new System.Drawing.Point(44, 200);
            this.dfTabTitle1.Multiline = true;
            this.dfTabTitle1.Name = "dfTabTitle1";
            this.dfTabTitle1.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTabTitle1.PhoenixUIControl.ObjectId = 49;
            this.dfTabTitle1.PreviousValue = null;
            this.dfTabTitle1.Size = new System.Drawing.Size(8, 20);
            this.dfTabTitle1.TabIndex = 1;
            // 
            // dfTabTitle0
            // 
            this.dfTabTitle0.Location = new System.Drawing.Point(44, 200);
            this.dfTabTitle0.Multiline = true;
            this.dfTabTitle0.Name = "dfTabTitle0";
            this.dfTabTitle0.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTabTitle0.PhoenixUIControl.ObjectId = 48;
            this.dfTabTitle0.PreviousValue = null;
            this.dfTabTitle0.Size = new System.Drawing.Size(8, 20);
            this.dfTabTitle0.TabIndex = 2;
            // 
            // dfTtlPmts
            // 
            this.dfTtlPmts.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTtlPmts.Location = new System.Drawing.Point(216, 136);
            this.dfTtlPmts.Multiline = true;
            this.dfTtlPmts.Name = "dfTtlPmts";
            this.dfTtlPmts.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTtlPmts.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTtlPmts.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTtlPmts.PhoenixUIControl.ObjectId = 65;
            this.dfTtlPmts.PhoenixUIControl.XmlTag = "TtlPmts";
            this.dfTtlPmts.PreviousValue = null;
            this.dfTtlPmts.Size = new System.Drawing.Size(112, 16);
            this.dfTtlPmts.TabIndex = 13;
            // 
            // lblTreasuryTaxLoanPayments
            // 
            this.lblTreasuryTaxLoanPayments.AutoEllipsis = true;
            this.lblTreasuryTaxLoanPayments.Location = new System.Drawing.Point(4, 136);
            this.lblTreasuryTaxLoanPayments.Name = "lblTreasuryTaxLoanPayments";
            this.lblTreasuryTaxLoanPayments.PhoenixUIControl.ObjectId = 65;
            this.lblTreasuryTaxLoanPayments.Size = new System.Drawing.Size(167, 16);
            this.lblTreasuryTaxLoanPayments.TabIndex = 12;
            this.lblTreasuryTaxLoanPayments.Text = "Treasury Tax && Loan Payments:";
            // 
            // dfUtilPmts
            // 
            this.dfUtilPmts.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfUtilPmts.Location = new System.Drawing.Point(216, 116);
            this.dfUtilPmts.Multiline = true;
            this.dfUtilPmts.Name = "dfUtilPmts";
            this.dfUtilPmts.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfUtilPmts.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfUtilPmts.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfUtilPmts.PhoenixUIControl.ObjectId = 64;
            this.dfUtilPmts.PhoenixUIControl.XmlTag = "UtilPmts";
            this.dfUtilPmts.PreviousValue = null;
            this.dfUtilPmts.Size = new System.Drawing.Size(112, 16);
            this.dfUtilPmts.TabIndex = 11;
            // 
            // lblUtilityPayments
            // 
            this.lblUtilityPayments.AutoEllipsis = true;
            this.lblUtilityPayments.Location = new System.Drawing.Point(4, 116);
            this.lblUtilityPayments.Name = "lblUtilityPayments";
            this.lblUtilityPayments.PhoenixUIControl.ObjectId = 64;
            this.lblUtilityPayments.Size = new System.Drawing.Size(167, 16);
            this.lblUtilityPayments.TabIndex = 10;
            this.lblUtilityPayments.Text = "Utility Payments:";
            // 
            // dfInvItemReturnAmt
            // 
            this.dfInvItemReturnAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInvItemReturnAmt.Location = new System.Drawing.Point(216, 96);
            this.dfInvItemReturnAmt.Multiline = true;
            this.dfInvItemReturnAmt.Name = "dfInvItemReturnAmt";
            this.dfInvItemReturnAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInvItemReturnAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfInvItemReturnAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfInvItemReturnAmt.PhoenixUIControl.ObjectId = 76;
            this.dfInvItemReturnAmt.PhoenixUIControl.XmlTag = "InvItemReturn";
            this.dfInvItemReturnAmt.PreviousValue = null;
            this.dfInvItemReturnAmt.Size = new System.Drawing.Size(112, 16);
            this.dfInvItemReturnAmt.TabIndex = 9;
            // 
            // lblInventoryItemReturned
            // 
            this.lblInventoryItemReturned.AutoEllipsis = true;
            this.lblInventoryItemReturned.Location = new System.Drawing.Point(4, 96);
            this.lblInventoryItemReturned.Name = "lblInventoryItemReturned";
            this.lblInventoryItemReturned.PhoenixUIControl.ObjectId = 76;
            this.lblInventoryItemReturned.Size = new System.Drawing.Size(136, 16);
            this.lblInventoryItemReturned.TabIndex = 8;
            this.lblInventoryItemReturned.Text = "Inventory Item Returned:";
            // 
            // dfInvItemPurchAmt
            // 
            this.dfInvItemPurchAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInvItemPurchAmt.Location = new System.Drawing.Point(216, 76);
            this.dfInvItemPurchAmt.Multiline = true;
            this.dfInvItemPurchAmt.Name = "dfInvItemPurchAmt";
            this.dfInvItemPurchAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInvItemPurchAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfInvItemPurchAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfInvItemPurchAmt.PhoenixUIControl.ObjectId = 63;
            this.dfInvItemPurchAmt.PhoenixUIControl.XmlTag = "InvItemPurch";
            this.dfInvItemPurchAmt.PreviousValue = null;
            this.dfInvItemPurchAmt.Size = new System.Drawing.Size(112, 16);
            this.dfInvItemPurchAmt.TabIndex = 7;
            // 
            // lblInventoryItemPurchased
            // 
            this.lblInventoryItemPurchased.AutoEllipsis = true;
            this.lblInventoryItemPurchased.Location = new System.Drawing.Point(4, 76);
            this.lblInventoryItemPurchased.Name = "lblInventoryItemPurchased";
            this.lblInventoryItemPurchased.PhoenixUIControl.ObjectId = 63;
            this.lblInventoryItemPurchased.Size = new System.Drawing.Size(136, 16);
            this.lblInventoryItemPurchased.TabIndex = 6;
            this.lblInventoryItemPurchased.Text = "Inventory Item Purchased:";
            // 
            // dfTrChksPurch
            // 
            this.dfTrChksPurch.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTrChksPurch.Location = new System.Drawing.Point(216, 56);
            this.dfTrChksPurch.Multiline = true;
            this.dfTrChksPurch.Name = "dfTrChksPurch";
            this.dfTrChksPurch.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTrChksPurch.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTrChksPurch.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTrChksPurch.PhoenixUIControl.ObjectId = 62;
            this.dfTrChksPurch.PhoenixUIControl.XmlTag = "TcPurch";
            this.dfTrChksPurch.PreviousValue = null;
            this.dfTrChksPurch.Size = new System.Drawing.Size(112, 16);
            this.dfTrChksPurch.TabIndex = 5;
            // 
            // lblTravelersChecksPurchased
            // 
            this.lblTravelersChecksPurchased.AutoEllipsis = true;
            this.lblTravelersChecksPurchased.Location = new System.Drawing.Point(4, 56);
            this.lblTravelersChecksPurchased.Name = "lblTravelersChecksPurchased";
            this.lblTravelersChecksPurchased.PhoenixUIControl.ObjectId = 62;
            this.lblTravelersChecksPurchased.Size = new System.Drawing.Size(167, 16);
            this.lblTravelersChecksPurchased.TabIndex = 4;
            this.lblTravelersChecksPurchased.Text = "Traveler\'s Checks Purchased:";
            // 
            // dfMoneyOrder
            // 
            this.dfMoneyOrder.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMoneyOrder.Location = new System.Drawing.Point(216, 36);
            this.dfMoneyOrder.Multiline = true;
            this.dfMoneyOrder.Name = "dfMoneyOrder";
            this.dfMoneyOrder.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMoneyOrder.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfMoneyOrder.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfMoneyOrder.PhoenixUIControl.ObjectId = 61;
            this.dfMoneyOrder.PhoenixUIControl.XmlTag = "MoneyOrders";
            this.dfMoneyOrder.PreviousValue = null;
            this.dfMoneyOrder.Size = new System.Drawing.Size(112, 16);
            this.dfMoneyOrder.TabIndex = 3;
            // 
            // lblMoneyOrdersPurchased
            // 
            this.lblMoneyOrdersPurchased.AutoEllipsis = true;
            this.lblMoneyOrdersPurchased.Location = new System.Drawing.Point(4, 36);
            this.lblMoneyOrdersPurchased.Name = "lblMoneyOrdersPurchased";
            this.lblMoneyOrdersPurchased.PhoenixUIControl.ObjectId = 61;
            this.lblMoneyOrdersPurchased.Size = new System.Drawing.Size(166, 16);
            this.lblMoneyOrdersPurchased.TabIndex = 2;
            this.lblMoneyOrdersPurchased.Text = "Money Orders Purchased:";
            // 
            // dfOfficialChks
            // 
            this.dfOfficialChks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOfficialChks.Location = new System.Drawing.Point(216, 16);
            this.dfOfficialChks.Multiline = true;
            this.dfOfficialChks.Name = "dfOfficialChks";
            this.dfOfficialChks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfOfficialChks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOfficialChks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfOfficialChks.PhoenixUIControl.ObjectId = 60;
            this.dfOfficialChks.PhoenixUIControl.XmlTag = "OfficialChks";
            this.dfOfficialChks.PreviousValue = null;
            this.dfOfficialChks.Size = new System.Drawing.Size(112, 16);
            this.dfOfficialChks.TabIndex = 1;
            // 
            // lblOfficialChecksPurchased
            // 
            this.lblOfficialChecksPurchased.AutoEllipsis = true;
            this.lblOfficialChecksPurchased.Location = new System.Drawing.Point(4, 16);
            this.lblOfficialChecksPurchased.Name = "lblOfficialChecksPurchased";
            this.lblOfficialChecksPurchased.PhoenixUIControl.ObjectId = 60;
            this.lblOfficialChecksPurchased.Size = new System.Drawing.Size(166, 16);
            this.lblOfficialChecksPurchased.TabIndex = 0;
            this.lblOfficialChecksPurchased.Text = "Official Checks Purchased:";
            // 
            // Name2
            // 
            this.Name2.Controls.Add(this.gbTCDCashItems);
            this.Name2.Controls.Add(this.dfHiddenTCDCashItem);
            this.Name2.Location = new System.Drawing.Point(4, 22);
            this.Name2.MLInfo = controlInfo3;
            this.Name2.Name = "Name2";
            this.Name2.Size = new System.Drawing.Size(682, 422);
            this.Name2.TabIndex = 2;
            this.Name2.Text = "TCD/TCR Cash Ite&ms";
            // 
            // gbTCDCashItems
            // 
            this.gbTCDCashItems.Controls.Add(this.dfTCRCashDeposited);
            this.gbTCDCashItems.Controls.Add(this.lblTcrCashDeposited);
            this.gbTCDCashItems.Controls.Add(this.dfTCDCashSold);
            this.gbTCDCashItems.Controls.Add(this.lblTcrCashSold);
            this.gbTCDCashItems.Controls.Add(this.dfTCRCashDepleted);
            this.gbTCDCashItems.Controls.Add(this.lblTcrCashDepleted);
            this.gbTCDCashItems.Controls.Add(this.dfTCDCashRemoved);
            this.gbTCDCashItems.Controls.Add(this.lblTcdCashRemoved);
            this.gbTCDCashItems.Controls.Add(this.dfTCDCashLoaded);
            this.gbTCDCashItems.Controls.Add(this.lblTcdCashLoaded);
            this.gbTCDCashItems.Controls.Add(this.lblTcdCashDispensed);
            this.gbTCDCashItems.Controls.Add(this.dfTCDCashBought);
            this.gbTCDCashItems.Controls.Add(this.lblCashBoughtfromTCD);
            this.gbTCDCashItems.Controls.Add(this.dfTCDCashDisp);
            this.gbTCDCashItems.Location = new System.Drawing.Point(0, 56);
            this.gbTCDCashItems.Name = "gbTCDCashItems";
            this.gbTCDCashItems.Size = new System.Drawing.Size(680, 96);
            this.gbTCDCashItems.TabIndex = 0;
            this.gbTCDCashItems.TabStop = false;
            this.gbTCDCashItems.Text = "TCD Cash Items";
            // 
            // dfTCRCashDeposited
            // 
            this.dfTCRCashDeposited.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDeposited.Location = new System.Drawing.Point(560, 16);
            this.dfTCRCashDeposited.Multiline = true;
            this.dfTCRCashDeposited.Name = "dfTCRCashDeposited";
            this.dfTCRCashDeposited.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDeposited.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCRCashDeposited.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCRCashDeposited.PhoenixUIControl.ObjectId = 107;
            this.dfTCRCashDeposited.PhoenixUIControl.XmlTag = "TcrCashDeposited";
            this.dfTCRCashDeposited.PreviousValue = null;
            this.dfTCRCashDeposited.Size = new System.Drawing.Size(112, 16);
            this.dfTCRCashDeposited.TabIndex = 3;
            // 
            // lblTcrCashDeposited
            // 
            this.lblTcrCashDeposited.AutoEllipsis = true;
            this.lblTcrCashDeposited.Location = new System.Drawing.Point(348, 16);
            this.lblTcrCashDeposited.Name = "lblTcrCashDeposited";
            this.lblTcrCashDeposited.Size = new System.Drawing.Size(140, 16);
            this.lblTcrCashDeposited.TabIndex = 2;
            this.lblTcrCashDeposited.Text = "TCR Cash Deposited:";
            // 
            // dfTCDCashSold
            // 
            this.dfTCDCashSold.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashSold.Location = new System.Drawing.Point(560, 36);
            this.dfTCDCashSold.Multiline = true;
            this.dfTCDCashSold.Name = "dfTCDCashSold";
            this.dfTCDCashSold.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashSold.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDCashSold.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashSold.PhoenixUIControl.ObjectId = 108;
            this.dfTCDCashSold.PhoenixUIControl.XmlTag = "TcdCashSold";
            this.dfTCDCashSold.PreviousValue = null;
            this.dfTCDCashSold.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashSold.TabIndex = 7;
            // 
            // lblTcrCashSold
            // 
            this.lblTcrCashSold.AutoEllipsis = true;
            this.lblTcrCashSold.Location = new System.Drawing.Point(348, 36);
            this.lblTcrCashSold.Name = "lblTcrCashSold";
            this.lblTcrCashSold.Size = new System.Drawing.Size(139, 16);
            this.lblTcrCashSold.TabIndex = 6;
            this.lblTcrCashSold.Text = "Cash Sold to TCR:";
            // 
            // dfTCRCashDepleted
            // 
            this.dfTCRCashDepleted.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDepleted.Location = new System.Drawing.Point(176, 76);
            this.dfTCRCashDepleted.Multiline = true;
            this.dfTCRCashDepleted.Name = "dfTCRCashDepleted";
            this.dfTCRCashDepleted.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDepleted.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCRCashDepleted.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCRCashDepleted.PhoenixUIControl.ObjectId = 109;
            this.dfTCRCashDepleted.PhoenixUIControl.XmlTag = "TcrCashDepleted";
            this.dfTCRCashDepleted.PreviousValue = null;
            this.dfTCRCashDepleted.Size = new System.Drawing.Size(112, 16);
            this.dfTCRCashDepleted.TabIndex = 13;
            // 
            // lblTcrCashDepleted
            // 
            this.lblTcrCashDepleted.AutoEllipsis = true;
            this.lblTcrCashDepleted.Location = new System.Drawing.Point(4, 76);
            this.lblTcrCashDepleted.Name = "lblTcrCashDepleted";
            this.lblTcrCashDepleted.Size = new System.Drawing.Size(152, 16);
            this.lblTcrCashDepleted.TabIndex = 12;
            this.lblTcrCashDepleted.Text = "TCR Cash Depleted:";
            // 
            // dfTCDCashRemoved
            // 
            this.dfTCDCashRemoved.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashRemoved.Location = new System.Drawing.Point(176, 56);
            this.dfTCDCashRemoved.Multiline = true;
            this.dfTCDCashRemoved.Name = "dfTCDCashRemoved";
            this.dfTCDCashRemoved.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashRemoved.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDCashRemoved.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashRemoved.PhoenixUIControl.XmlTag = "TcdRemoveCash";
            this.dfTCDCashRemoved.PreviousValue = null;
            this.dfTCDCashRemoved.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashRemoved.TabIndex = 9;
            // 
            // lblTcdCashRemoved
            // 
            this.lblTcdCashRemoved.AutoEllipsis = true;
            this.lblTcdCashRemoved.Location = new System.Drawing.Point(4, 56);
            this.lblTcdCashRemoved.Name = "lblTcdCashRemoved";
            this.lblTcdCashRemoved.Size = new System.Drawing.Size(152, 16);
            this.lblTcdCashRemoved.TabIndex = 8;
            this.lblTcdCashRemoved.Text = "TCD Cash Removed:";
            // 
            // dfTCDCashLoaded
            // 
            this.dfTCDCashLoaded.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashLoaded.Location = new System.Drawing.Point(560, 56);
            this.dfTCDCashLoaded.Multiline = true;
            this.dfTCDCashLoaded.Name = "dfTCDCashLoaded";
            this.dfTCDCashLoaded.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashLoaded.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDCashLoaded.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashLoaded.PhoenixUIControl.XmlTag = "TcdAddCash";
            this.dfTCDCashLoaded.PreviousValue = null;
            this.dfTCDCashLoaded.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashLoaded.TabIndex = 11;
            // 
            // lblTcdCashLoaded
            // 
            this.lblTcdCashLoaded.AutoEllipsis = true;
            this.lblTcdCashLoaded.Location = new System.Drawing.Point(348, 56);
            this.lblTcdCashLoaded.Name = "lblTcdCashLoaded";
            this.lblTcdCashLoaded.Size = new System.Drawing.Size(139, 16);
            this.lblTcdCashLoaded.TabIndex = 10;
            this.lblTcdCashLoaded.Text = "TCD/TCR Cash Loaded:";
            // 
            // lblTcdCashDispensed
            // 
            this.lblTcdCashDispensed.AutoEllipsis = true;
            this.lblTcdCashDispensed.Location = new System.Drawing.Point(4, 16);
            this.lblTcdCashDispensed.Name = "lblTcdCashDispensed";
            this.lblTcdCashDispensed.PhoenixUIControl.ObjectId = 81;
            this.lblTcdCashDispensed.Size = new System.Drawing.Size(152, 16);
            this.lblTcdCashDispensed.TabIndex = 0;
            this.lblTcdCashDispensed.Text = "TCD/TCR Cash Dispensed:";
            // 
            // dfTCDCashBought
            // 
            this.dfTCDCashBought.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashBought.Location = new System.Drawing.Point(176, 36);
            this.dfTCDCashBought.Multiline = true;
            this.dfTCDCashBought.Name = "dfTCDCashBought";
            this.dfTCDCashBought.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashBought.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDCashBought.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashBought.PhoenixUIControl.ObjectId = 82;
            this.dfTCDCashBought.PhoenixUIControl.XmlTag = "TcdCashBought";
            this.dfTCDCashBought.PreviousValue = null;
            this.dfTCDCashBought.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashBought.TabIndex = 5;
            // 
            // lblCashBoughtfromTCD
            // 
            this.lblCashBoughtfromTCD.AutoEllipsis = true;
            this.lblCashBoughtfromTCD.Location = new System.Drawing.Point(4, 36);
            this.lblCashBoughtfromTCD.Name = "lblCashBoughtfromTCD";
            this.lblCashBoughtfromTCD.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Show;
            this.lblCashBoughtfromTCD.PhoenixUIControl.ObjectId = 82;
            this.lblCashBoughtfromTCD.Size = new System.Drawing.Size(152, 16);
            this.lblCashBoughtfromTCD.TabIndex = 4;
            this.lblCashBoughtfromTCD.Text = "Cash Bought from TCD/TCR:";
            // 
            // dfTCDCashDisp
            // 
            this.dfTCDCashDisp.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashDisp.Location = new System.Drawing.Point(176, 16);
            this.dfTCDCashDisp.Multiline = true;
            this.dfTCDCashDisp.Name = "dfTCDCashDisp";
            this.dfTCDCashDisp.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashDisp.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCDCashDisp.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashDisp.PhoenixUIControl.ObjectId = 81;
            this.dfTCDCashDisp.PhoenixUIControl.XmlTag = "TcdCashOut";
            this.dfTCDCashDisp.PreviousValue = null;
            this.dfTCDCashDisp.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashDisp.TabIndex = 1;
            // 
            // dfHiddenTCDCashItem
            // 
            this.dfHiddenTCDCashItem.Location = new System.Drawing.Point(401, 129);
            this.dfHiddenTCDCashItem.Name = "dfHiddenTCDCashItem";
            this.dfHiddenTCDCashItem.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.dfHiddenTCDCashItem.PhoenixUIControl.ObjectId = 83;
            this.dfHiddenTCDCashItem.PreviousValue = null;
            this.dfHiddenTCDCashItem.Size = new System.Drawing.Size(8, 20);
            this.dfHiddenTCDCashItem.TabIndex = 1;
            this.dfHiddenTCDCashItem.Visible = false;
            // 
            // pbTranTotals
            // 
            this.pbTranTotals.LongText = "&Tran Totals";
            this.pbTranTotals.Name = "&Tran Totals";
            this.pbTranTotals.NextScreenId = 10446;
            this.pbTranTotals.ObjectId = 3;
            this.pbTranTotals.ShortText = "&Tran Totals";
            this.pbTranTotals.Tag = null;
            this.pbTranTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTranTotals_Click);
            // 
            // pbBatchTotals
            // 
            this.pbBatchTotals.LongText = "Batch Totals";
            this.pbBatchTotals.Name = "Batch Totals";
            this.pbBatchTotals.NextScreenId = 10487;
            this.pbBatchTotals.ObjectId = 4;
            this.pbBatchTotals.ShortText = "Batch Totals";
            this.pbBatchTotals.Tag = null;
            this.pbBatchTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBatchTotals_Click);
            // 
            // pbProofTotals
            // 
            this.pbProofTotals.LongText = "P&roof Totals...";
            this.pbProofTotals.Name = "P&roof Totals...";
            this.pbProofTotals.NextScreenId = 2866;
            this.pbProofTotals.ObjectId = 100;
            this.pbProofTotals.ShortText = "P&roof Totals...";
            this.pbProofTotals.Tag = null;
            this.pbProofTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbProofTotals_Click);
            // 
            // pbCountDrawer
            // 
            this.pbCountDrawer.LongText = "Count";
            this.pbCountDrawer.Name = "Count";
            this.pbCountDrawer.NextScreenId = 10442;
            this.pbCountDrawer.ObjectId = 5;
            this.pbCountDrawer.ShortText = "Count";
            this.pbCountDrawer.Tag = null;
            this.pbCountDrawer.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCountDrawer_Click);
            // 
            // pbCloseOut
            // 
            this.pbCloseOut.LongText = "CloseOut";
            this.pbCloseOut.Name = "CloseOut";
            this.pbCloseOut.NextScreenId = 10460;
            this.pbCloseOut.ObjectId = 6;
            this.pbCloseOut.ShortText = "CloseOut";
            this.pbCloseOut.Tag = null;
            this.pbCloseOut.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCloseOut_Click);
            // 
            // pbHistory
            // 
            this.pbHistory.LongText = "&History";
            this.pbHistory.Name = "&History";
            this.pbHistory.NextScreenId = 10766;
            this.pbHistory.ObjectId = 7;
            this.pbHistory.ShortText = "&History";
            this.pbHistory.Tag = null;
            this.pbHistory.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbHistory_Click);
            // 
            // pbAdjTotals
            // 
            this.pbAdjTotals.LongText = "&Adj. Totals";
            this.pbAdjTotals.Name = "&Adj. Totals";
            this.pbAdjTotals.NextScreenId = 12236;
            this.pbAdjTotals.ObjectId = 88;
            this.pbAdjTotals.ShortText = "&Adj. Totals";
            this.pbAdjTotals.Tag = null;
            this.pbAdjTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbAdjTotals_Click);
            // 
            // pbCurrentPod
            // 
            this.pbCurrentPod.LongText = "Current &POD";
            this.pbCurrentPod.Name = "Current &POD";
            this.pbCurrentPod.ObjectId = 90;
            this.pbCurrentPod.ShortText = "Current &POD";
            this.pbCurrentPod.Tag = null;
            this.pbCurrentPod.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCurrentPod_Click);
            // 
            // pbLastPod
            // 
            this.pbLastPod.LongText = "&Last POD";
            this.pbLastPod.Name = "&Last POD";
            this.pbLastPod.ObjectId = 91;
            this.pbLastPod.ShortText = "&Last POD";
            this.pbLastPod.Tag = null;
            this.pbLastPod.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbLastPod_Click);
            // 
            // pbDailyPod
            // 
            this.pbDailyPod.LongText = "&Daily POD";
            this.pbDailyPod.Name = "&Daily POD";
            this.pbDailyPod.ObjectId = 92;
            this.pbDailyPod.ShortText = "&Daily POD";
            this.pbDailyPod.Tag = null;
            this.pbDailyPod.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDailyPod_Click);
            // 
            // pbRollover
            // 
            this.pbRollover.LongText = "Ro&llover";
            this.pbRollover.Name = "Ro&llover";
            this.pbRollover.ObjectId = 97;
            this.pbRollover.ShortText = "Ro&llover";
            this.pbRollover.Tag = null;
            this.pbRollover.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRollover_Click);
            // 
            // pbEOB
            // 
            this.pbEOB.LongText = "&EOB";
            this.pbEOB.Name = "&EOB";
            this.pbEOB.ObjectId = 110;
            this.pbEOB.ShortText = "&EOB";
            this.pbEOB.Tag = null;
            this.pbEOB.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEOB_Click);
            // 
            // pbBulkProcess
            // 
            this.pbBulkProcess.LongText = "B&ulk Process...";
            this.pbBulkProcess.Name = "B&ulk Process...";
            this.pbBulkProcess.ObjectId = 111;
            this.pbBulkProcess.ShortText = "B&ulk Process...";
            this.pbBulkProcess.Tag = null;
            this.pbBulkProcess.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBulkProcess_Click);
            // 
            // pbMiddayCount
            // 
            this.pbMiddayCount.LongText = "&MiddayCount";
            this.pbMiddayCount.Name = "&MiddayCount";
            this.pbMiddayCount.NextScreenId = 10460;
            this.pbMiddayCount.ObjectId = 114;
            this.pbMiddayCount.ShortText = "&MiddayCount";
            this.pbMiddayCount.Tag = null;
            this.pbMiddayCount.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.PbMiddayCount_Click);
            // 
            // frmTlPosition
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.picTabs);
            this.Name = "frmTlPosition";
            this.ScreenId = 10430;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlPosition_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlPosition_PInitCompleteEvent);
            this.PMdiPrintEvent += new System.EventHandler(this.frmTlPosition_PMdiPrintEvent);
            this.PShowCompletedEvent += new System.EventHandler(this.frmTlPosition_PShowCompletedEvent);
            this.Load += new System.EventHandler(this.frmTlPosition_Load);
            this.Enter += new System.EventHandler(this.frmTlPosition_Enter);//Task#121857
            this.picTabs.ResumeLayout(false);
            this.Name0.ResumeLayout(false);
            this.gbChkBal.ResumeLayout(false);
            this.gbChkBal.PerformLayout();
            this.gbDepositedItems.ResumeLayout(false);
            this.gbDepositedItems.PerformLayout();
            this.gbDepositsandPayments.ResumeLayout(false);
            this.gbDepositsandPayments.PerformLayout();
            this.gbCashBalancing.ResumeLayout(false);
            this.gbCashBalancing.PerformLayout();
            this.gbTellerPosition.ResumeLayout(false);
            this.gbTellerPosition.PerformLayout();
            this.Name1.ResumeLayout(false);
            this.gbExternals.ResumeLayout(false);
            this.gbExternals.PerformLayout();
            this.gbExchangeandAdvances.ResumeLayout(false);
            this.gbExchangeandAdvances.PerformLayout();
            this.gbWithdrawalsandAdvances.ResumeLayout(false);
            this.gbWithdrawalsandAdvances.PerformLayout();
            this.gbMiscellaneous.ResumeLayout(false);
            this.gbMiscellaneous.PerformLayout();
            this.gbPurchasesandPayments.ResumeLayout(false);
            this.gbPurchasesandPayments.PerformLayout();
            this.Name2.ResumeLayout(false);
            this.Name2.PerformLayout();
            this.gbTCDCashItems.ResumeLayout(false);
            this.gbTCDCashItems.PerformLayout();
            this.ResumeLayout(false);

		}
        //Begin Task#121857
        private void frmTlPosition_Enter(object sender, EventArgs e)
        {
            RefreshData();
        }
        //End Task#121857
        void pbBulkProcess_Click(object sender, PActionEventArgs e)
        {
            ProcessPendingBulkTran(false);
        }

        void pbEOB_Click(object sender, PActionEventArgs e)
        {

            if (string.IsNullOrEmpty(TellerVars.Instance.TlCaptureEOBSourceDesc))
                TellerVars.Instance.TlCaptureEOBSourceDesc = "Force EOB thru Summary Position Action button";        //#195669, #35513

            if (!HandleEndOfTellerCaptureBatch())
            {
                this.pbEOB.Enabled = true;
            }
        }

        void pbRollover_Click(object sender, PActionEventArgs e)
        {
            if (_tellerVars.IsTCDEnabled && _tellerVars.IsTCDConnected)
            {
                try
                {
                    InitializeCashDispense();
                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361087));
                    //#9624
                    #region Turn ON Allow Inventory Zeros to supress Rollover
                    //cashDisp.SetIniFileSettings("General", "Allow Inventory Zeroes", "YES", string.Empty);
                    //cashDisp.WindowTitle = "Rollover";
                    //cashDisp.Open(); //force open
                    #endregion
                    //cashDisp.WindowTitle = "Rollover";
                    //cashDisp.Open(); //force open
                    if (cashDisp.WindowTitle != "TCD/TCR Totals")
                    {
                        cashDisp.IsConnOpen = false;
                        cashDisp.WindowTitle = "TCD/TCR Totals";
                    }
                    //
                    #region call totals
                    cashDisp.GetTotals(drawerNo.Value.ToString(), false, _deviceOutput, true); //#79574 - added clear all/rollover changes
                    #endregion
                }
                finally
                {
                    //Begin #75864
                    #region Turn OFF Allow Inventory Zeros
                    //cashDisp.SetIniFileSettings("General", "Allow Inventory Zeroes", "NO", string.Empty);
                    #endregion
                    //End #75864

                    dlgInformation.Instance.HideInfo();
                    //#79574
                    //cashDisp.WindowTitle = "TCD Dispense";
                    cashDisp.WindowTitle = _tellerVars.DeviceDispWindowTitle;
                    /*16658 commented the following*/
                   // cashDisp.Open(); //force open

                }
            }
        }

        #endregion

        #region Init param
        public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 8)  //#79574
			{
				positionView.Value = Convert.ToInt16(paramList[0]);
				branchNo.Value = Convert.ToInt16(paramList[1]);
				drawerNo.Value = Convert.ToInt16(paramList[2]);
				closedDate.Value = Convert.ToDateTime(paramList[3]);
				if (paramList[4] != null)
					positionPtid.Value = Convert.ToDecimal(paramList[4]);
				if (paramList[5] != null)
					previousEmplId.Value = Convert.ToInt16(paramList[5]);
				else
					previousEmplId.Value = DbSmallInt.Null;
				if (paramList[6] != null)
					newEmplId.Value = Convert.ToInt16(paramList[6]);
				if (paramList[7] != null)
					returnGrandTotal = (bool)paramList[7];

                //if (paramList.Length == 9)    //WI#35358 - Commented this line and wrote new condition
                if (paramList.Length > 8)
				{
					if (paramList[8] != null)
						drawerCombo = paramList[8] as PComboBoxStandard;
				}
                if (paramList.Length == 10) //#79574
                {
                    tcdAction = paramList[9] as PAction;
                }
				GetPositionView(true);
			}



			this.CurrencyId = _tellerVars.LocalCrncyId;
			if (positionView.Value == 3 || positionView.Value == 4)
				employeeId.Value = previousEmplId.Value;
			else
				employeeId.Value = _tellerVars.EmployeeId;

			base.InitParameters (paramList);
		}
		#endregion

		#region Call Parent
		public override void CallParent(params object[] paramList)
		{
			string caller = "";

			if ( paramList != null && paramList[0] != null )
				caller = Convert.ToString( paramList[0] );

			if ( caller == "DrawerAdj" )
			{
				if (pbAdjTotals.Enabled)
				{
					GetPositionView(false);
					RecalcTotals();
				}
			}
			else if (caller == "UpdateUnbatchedAmtTotal" )
			{
				GetPositionView(false);
                //#71494
                if (_tellerVars.DrawerCounted)
                    /* Begin #86292 */
                    /*this.dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount;*/
                    this.dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount == Decimal.MinValue ? 0 : _tellerVars.CountedAmount;
                    /* End #86292 */
                RecalcTotals();
			}
			else if (caller == "Override" )
			{
				if ( paramList.Length > 1 )
				{
					if ( Convert.ToBoolean( paramList[1] ) == true )
						dialogResult = DialogResult.OK;
					else
						dialogResult = DialogResult.Cancel;
					//
					if (dialogResult == DialogResult.OK)
					{
						if (_tlJournalOvrd != null && isRequireSuperOvrd)
						{
							if (_tlJournalOvrd.SuperEmplId.IsNull)
								SuperEmplId.ValueObject = 0;
							else
								SuperEmplId.ValueObject = _tlJournalOvrd.SuperEmplId.Value;
                            if (_tlJournalOvrd.MessageId.IsNull)    //#15117
                                _tlDrawerBalances.OvrdMessageId.Value = 0;
                            else
                                _tlDrawerBalances.OvrdMessageId.Value = _tlJournalOvrd.MessageId.Value;
                            if (_tlJournalOvrd.OvrdStatus.IsNull)
                                _tlDrawerBalances.OvrdStatus.Value = 0;
                            else
                                _tlDrawerBalances.OvrdStatus.Value = _tlJournalOvrd.OvrdStatus.Value;
						}
						DrawerCloseOut();
					}
				}
			}
			else
			{
				base.CallParent( paramList ); /*18900 uncommented this */
			}
		}
		#endregion

		#region standard actions
		public override bool OnActionSave(bool isAddNext)
		{

			string tempSystemCloseOut = "";
			string tempEmployeeChange = "";
            string tempMiddayCashCount = "";  //118226

            if (dfDescription.Text != string.Empty && dfDescription.Text != "" &&
				dfDescription.Text != null && dfDescription.Text.Trim() != null)
			{
				string desc = dfDescription.Text.Trim().ToUpper();
				tempSystemCloseOut = systemCloseOut.ToUpper();
				tempEmployeeChange = employeeChange.ToUpper();

				if ((desc.StartsWith(tempSystemCloseOut) ||
					desc.StartsWith(tempEmployeeChange)))
				{
					PMessageBox.Show(this,314235,MessageType.Warning,MessageBoxButtons.OK);
					return false;
				}

                //Begin 118226
                tempMiddayCashCount = middayCashCount.ToUpper();

                if ((desc.StartsWith(tempMiddayCashCount)))
                {
                    //16416 - Description can not start with 'Midday Cash Count'
                    PMessageBox.Show(this, 16416, MessageType.Warning, MessageBoxButtons.OK);
                    return false;
                }
                //End 118226
            }

            isRealCloseOut = false;

            isMidDayCount = false; //118226

            if (!DrawerCloseOut())
				return false;
			return true;
//			return base.OnActionSave (isAddNext);
		}

		public override bool OnActionClose()
		{
			bool closeForm = true;
			//if (_tellerVars.DrawerCounted && !_tellerVars.ClosedOut && positionView.Value == 0)
			if (_tellerVars.DrawerCounted && !_isDrawerClosedOut && pbCloseOut.Visible && pbCloseOut.Enabled &&
				(positionView.Value == 0 || positionView.Value == 3) && !drawerIsSignOn )		// #71886 - Added drawerIsSignOn
			{
				if (DialogResult.Yes == PMessageBox.Show(this, 360994, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
					closeForm = true;
				else
					closeForm = false;
			}
			//
			if (closeForm)
			{
				/* Begin #71886 */
				//_tellerVars.DrawerCounted = false;
				//_tellerVars.CountedAmount = 0;
				if (!drawerIsSignOn)
				{
					_tellerVars.DrawerCounted = prevDrawerCounted;
					_tellerVars.CountedAmount = prevCountedAmount;
				}
				/* End #71886 */

				/*begin #18900*/
				CallParent(this.ScreenId,"RefreshTellerJournalGrid");/*Task#110967 passed screenid*/
				/*end  #18900*/
				return base.OnActionClose ();
			}
			else
				return false;
			//
		}

		#endregion

		#region events

		private ReturnType frmTlPosition_PInitBeginEvent()
		{
			try
			{
				this.AppToolBarId = AppToolBarType.NoEdit;
				this.MainBusinesObject = _tlDrawerBalances;
				this.ActionSave.NextScreenId = 0;
                _supervisorFlag = ((_adGbRsm.Supervisor.IsNull ? "N" : _adGbRsm.Supervisor.Value) == GlobalVars.Instance.ML.Y);        // WI#35358

				// #71132 - moved to PInitCompleteEvent
//				if (!positionPtid.IsNull && positionPtid.Value > 0) //History
//					_isBalDenomTracking = (_tlDrawerBalances.BalDenomTracking.Value == GlobalVars.Instance.ML.Y);
//				else
//					_isBalDenomTracking = (_tellerVars.AdTlControl.BalDenomTracking.Value == GlobalVars.Instance.ML.Y);

				//this.DefaultAction = this.ActionClose;
				ScreenAccessConfig();

				SetFormDefaults();
				/* Begin #71886 */
				drawerIsSignOn = branchNo.Value == _tellerVars.BranchNo && drawerNo.Value == _tellerVars.DrawerNo &&
					( positionView.Value == 0 || positionView.Value == 3 );
				if ( drawerIsSignOn )
				{
					_tlDrawerBalances.CashCount = _tellerVars.CashCount;
					_tlDrawerBalances.CashDrawerSummary = _tellerVars.CashDrawerSummary;
					if ( _tellerVars.CashUpdateCounter != _tellerVars.CashUpdateCounterAtCount )
						_tlDrawerBalances.CashDrawerSummary.Clear();
					lastCashUpdateCounter = _tellerVars.CashUpdateCounter;

				}
				else
				{
					prevDrawerCounted = _tellerVars.DrawerCounted;
					prevCountedAmount = _tellerVars.CountedAmount;
					_tellerVars.DrawerCounted = false;
					_tellerVars.CountedAmount = 0;
                }
				if ( _isDrawerClosedOut ){};
				/* End #71886 */
			}
			catch (PhoenixException pebegin)
			{
				PMessageBox.Show(pebegin);
			}
// 			catch (Exception ebegin)
// 			{
// 				MessageBox.Show(this, ebegin.Message + "\r\n" + ebegin.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
// 			}
			return ReturnType.Success;
		}

		private void frmTlPosition_PInitCompleteEvent()
		{
			try
			{
                if (_tellerVars.IsTellerCaptureEnabled) //#140895
                {
                    _tellerCapture = Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper;
                    //_tellerCapture.CurForm = this;
                }

				employeeChange = CoreService.Translation.GetTokenizeMessageX(360167, new string[] {previousEmplId.Value.ToString(), newEmplId.Value.ToString()});	// Employee Change from: %1! To %2!'
				systemCloseOut = CoreService.Translation.GetUserMessageX(360168);
                middayCashCount = CoreService.Translation.GetUserMessageX(16415);  //118226 

                EnableDisableVisibleLogic("FormComplete");

                #region screen title
                //				this.Text = FixScreenTitle();
				this.NewRecordTitle = string.Format(NewRecordTitle, FixScreenTitle());
				this.EditRecordTitle = string.Format(EditRecordTitle, FixScreenTitle());
				#endregion

				RecalcTotals();
				//
				if (branchName.IsNull)
				{
					if (_tellerHelper.GetFKValueDesc(_tlDrawerBalances.BranchNo.Value, _tlDrawerBalances.BranchNo.FKValue) != "")
						branchName.Value = _tellerHelper.GetFKValueDesc(_tlDrawerBalances.BranchNo.Value, _tlDrawerBalances.BranchNo.FKValue);
				}
				//
				if (!positionPtid.IsNull && positionPtid.Value > 0) //History
					_isBalDenomTracking = (_tlDrawerBalances.BalDenomTracking.Value == GlobalVars.Instance.ML.Y);
				else
					_isBalDenomTracking = (_tellerVars.AdTlControl.BalDenomTracking.Value == GlobalVars.Instance.ML.Y);

				/* Begin #71886 */
				if ( drawerIsSignOn && _tellerVars.DrawerCounted )
					tempWin_Closed( this, null );
				/* End #71886 */
				//#71893
//				if (this.dfDescription.Enabled && this.pbCountDrawer.Enabled)
//				{
//					this.pbCountDrawer
//				}
				//#72335
				if (this.pbCountDrawer.Enabled && this.pbCountDrawer.Visible)
					this.DefaultAction = this.pbCountDrawer;
				else
					this.DefaultAction = this.ActionClose;
				//
				if (this.dfDescription.Enabled && this.pbCountDrawer.Enabled)
				{
					this.dfDescription.TabStop = false;
				}
                /* Begin #72916 */
                this.dfTCDUnbatchCashOut.UnFormattedValue = this._tlDrawerBalances.TCDUnbatchCashOutAmt.Value;

                this.dfTCDCashDisp.UnFormattedValue = this._tlDrawerBalances.TcdCashOut.Value;
                this.dfTCDCashBought.UnFormattedValue = this._tlDrawerBalances.TcdCashBought.Value;

                this.dfTCDCashLoaded.UnFormattedValue = this._tlDrawerBalances.TcdAddCash.Value;
                this.dfTCDCashRemoved.UnFormattedValue = this._tlDrawerBalances.TcdRemoveCash.Value;
                /* End #72916 */
                // #9271 - Hack - business object property is losing its value on form load....
                _tlDrawerBalances.EndingCash.Value = totalCash; // #9271

                //Begin 118569
                if ((!_tellerVars.DrawerCounted && !_tellerVars.IsNonCashDrawer) || (drawerIsSignOn && _tellerVars.CashUpdateCounter != _tellerVars.CashUpdateCounterAtCount && !_tellerVars.IsNonCashDrawer))
                    isCountReqOnOpen = true;
                else
                    isCountReqOnOpen = false;
                //End 118569
            }
            catch (PhoenixException pecomp)
			{
				PMessageBox.Show(pecomp);
			}
// 			catch (Exception ecomp)
// 			{
// 				MessageBox.Show(this, ecomp.Message + "\r\n" + ecomp.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
// 			}
		}

        void frmTlPosition_PShowCompletedEvent(object sender, EventArgs e)
       {
            /*  Begin #3132
                    If I access teller and select the Balance Drawer/Batch Checks icon on the MDI toolbar (F5) all of the accelerator keys work.
                    If I have another window open, (Teller Transaction, Teller Journal, etc) and then access the Balance Drawer/Batch Checks icon
                    on the MDI toolbar (F5) none of the accelerator keys on the Teller Summary Position window work.

                    applying same fix as for when other window is closed on top of this window, search for #3132
                 */

            int selTab = this.picTabs.SelectedIndex;
            if (selTab == 0)
                this.Name0.Focus();
            else if (selTab == 1)
                this.Name1.Focus();
            else
                this.Name2.Focus();
            //end #3132
        }

		private void pbHistory_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("PositionHistory");
		}

		private void pbTranTotals_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("TranTotals");
		}

        //Begin 118226
        private void PbMiddayCount_Click(object sender, PActionEventArgs e)
        {
            //*** Fisrst check that drawer has been counted ***
            if (!_tellerVars.DrawerCounted && !_tellerVars.IsNonCashDrawer)
            {
                //16413 - Please count your drawer before proceeding with the Midday Count.  /*** ADD TO ML MESSAGES ***/
                PMessageBox.Show(this, 16413, MessageType.Warning, MessageBoxButtons.OK);
                return;
            }

            //Begin Bug#118530
            if (drawerIsSignOn && _tellerVars.CashUpdateCounter != _tellerVars.CashUpdateCounterAtCount && !_tellerVars.IsNonCashDrawer)   
            {
                //_tlDrawerBalances.CashDrawerSummary.Clear();

                // PMessageBox.Show( this,
                //	"The cash drawer may have changed since last count due to transactional activity. Please reverify your count.",
                //	"Phoenix Warning - #361001", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
                PMessageBox.Show(this, 361001, MessageType.Warning, MessageBoxButtons.OK);
                RefreshData();
                return;
            }
            //End Bug#118530

            //Begin 118569
            if (!isCountReqOnOpen)
            {
                //16424 - The cash drawer has not changed since the last Midday Count.  
                PMessageBox.Show(this, 16424, MessageType.Warning, MessageBoxButtons.OK);
                return;
            }
            //End 118569

            if (positionView.Value == 0)
            {
                this.dfDescription.Text = middayCashCount;
            }

            try
            {
                isRealCloseOut = false; //118226-2

                isMidDayCount = true;

                DrawerCloseOut();
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe);
                return;
            }
        }
        //End 118226

        private void pbCloseOut_Click(object sender, PActionEventArgs e)
        {
            #region #9510 - Is the drawer being closed a non-cash drawer?
            _signedOnDrawerIsNonCashDrawer = _tellerVars.IsNonCashDrawer;
            if (!drawerIsSignOn)
            {
                if (_tlDrawer.NonCash.Value == "Y")
                    _tellerVars.IsNonCashDrawer = true;
                else
                    _tellerVars.IsNonCashDrawer = false;
            }
            #endregion

            if (positionView.Value == 0)  // Do the checks necessary for close-out only
			{
				this.dfDescription.Text = systemCloseOut;
				if (gbOffline)
				{
					if ( DialogResult.No == PMessageBox.Show( 311405, MessageType.Question, MessageBoxButtons.YesNo, String.Empty ))
						return;
				}
				else
				{
					if ( DialogResult.No == PMessageBox.Show( 311082, MessageType.Question, MessageBoxButtons.YesNo, String.Empty ))
						return;
				}

				// Check unforwarded tranactions
				if (unforwardedTranCount > 0)
				{
                    PMessageBox.Show( 311435, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
					return;
				}
                if (!_tellerVars.AdTlControl.RemoteOvrd.IsNull && _tellerVars.AdTlControl.RemoteOvrd.Value == GlobalVars.Instance.ML.Y)   //#13117
                {
                    if (_commService != null && _commService.GetPendingMsgInfo() != null)
                    {
                        //13480 - Pending overrides exist.  Please resolve all pending overrides before continue with drawer close out.
                        PMessageBox.Show(13480, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                        return;
                    }
                }
			}

            isMidDayCount = false;  //118226-2

            isRealCloseOut = true;


            #region Teller Capture #140895  #28714
            if (pbEOB.Enabled)
            {
                TellerVars.Instance.TlCaptureEOBSourceDesc = "Teller Drawer Close Out";        //#195669, #35513

                if (!HandleEndOfTellerCaptureBatch())
                {
                    this.pbEOB.Enabled = true;
                    return;
                }
            }
            #endregion


            if (!CloseOutValidation())
            {
                #region #9510 - If we're here, we're returning, so restore value back to original value
                _tellerVars.IsNonCashDrawer = _signedOnDrawerIsNonCashDrawer;
                #endregion
                return;
            }

            #region #9510 - Made it through, so restore value back to original value
            _tellerVars.IsNonCashDrawer = _signedOnDrawerIsNonCashDrawer;
            #endregion
		}

		private void pbBatchTotals_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms( "BatchTotals" );
		}

        #region #76409
        void pbProofTotals_Click(object sender, PActionEventArgs e)
        {
            proofTotalsViewed = true;
            CallOtherForms("ProofTotals");
        }
        #endregion

		private void pbAdjTotals_Click(object sender, PActionEventArgs e)
		{
			if ( DialogResult.Yes == PMessageBox.Show( 318342, MessageType.Question, MessageBoxButtons.YesNo, String.Empty ))
			{
				CallOtherForms( "DrawerAdj" );
			}
		}

		private void dfDescription_Validating(object sender, CancelEventArgs e)
		{
			//EnableDisableVisibleLogic("HandleCloDesc");
//			if (dfDescription.Text.IndexOf("System Close-Out",0) <= 0 || dfDescription.Text.IndexOf("Employee Change from:",0) <= 0)
//			{
//				return;
//			}
		}

		private void picTabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (picTabs.SelectedIndex == 0)
			{
				if (!this.gbTellerPosition.Visible)
					this.Name0.Controls.Add(this.gbTellerPosition);
			}
			else if (picTabs.SelectedIndex == 1)
				this.Name1.Controls.Add(this.gbTellerPosition);
			else if (picTabs.SelectedIndex == 2)
				this.Name2.Controls.Add(this.gbTellerPosition);
		}

		private void pbCountDrawer_Click(object sender, PActionEventArgs e)
		{
			/* Begin #71886 */
			if ( drawerIsSignOn && _tellerVars.CashUpdateCounter != _tellerVars.CashUpdateCounterAtCount )
			{
				prevDrawerCounted = _tellerVars.DrawerCounted;
				_tellerVars.DrawerCounted = false;
				lastCashUpdateCounter = _tellerVars.CashUpdateCounter;
			}
			/* End #71886 */

			CallOtherForms( "CashDrawerCount" );
		}

		private void pbCurrentPod_Click(object sender, PActionEventArgs e)
		{
			_podTotalsType = 1;
			if (CalcPODTotals())
				return;
		}

		private void pbLastPod_Click(object sender, PActionEventArgs e)
		{
			_podTotalsType = 2;
			if (CalcPODTotals())
				return;
		}

		private void pbDailyPod_Click(object sender, PActionEventArgs e)
		{
			_podTotalsType = 3;
			if (CalcPODTotals())
				return;
		}

		private void SetFieldColor()
		{
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) == 0)
				dfDifference.ForeColor = System.Drawing.Color.Black;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
				dfDifference.ForeColor = System.Drawing.Color.Blue;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
				dfDifference.ForeColor = System.Drawing.Color.Red;
		}

		private void tempWin_Closed(object sender, EventArgs e)
		{
			Form form = sender as Form;
            if (form.Name == "dlgTlCashCount" || form.Name == "frmTlCashDrawerSummary" || form.Name == "frmTlProofTotals" || sender == this) /* #71886 - Added or */ /* #76409 - Added frmTlProofTotals */
			{
                #region #76409
                if (form.Name == "frmTlProofTotals")
                    EnableDisableVisibleLogic("FormComplete");
                #endregion

                /* Begin #71886 */
				if ( drawerIsSignOn && sender != this )
				{
					RefreshData();
					if (_tellerVars.DrawerCounted)
					{
						if ( lastCashUpdateCounter > _tellerVars.CashUpdateCounterAtCount )
							_tellerVars.CashUpdateCounterAtCount = lastCashUpdateCounter;
					}
					else
						_tellerVars.DrawerCounted = prevDrawerCounted;
				}
				/* End #71886 */
				if (_tellerVars.DrawerCounted)
				{
                    /*Begin #86292 */
                    /*dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount;*/
                    dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount == Decimal.MinValue ? 0 : _tellerVars.CountedAmount;
                    /* End #86292 */
                    _isDrawerCounted = true;
					_tlDrawerBalances.EndingCash.Value = Convert.ToDecimal(dfTotalCash.UnFormattedValue);
					_tlDrawerBalances.CashDrawer.Value = Convert.ToDecimal(dfCashDrawer.UnFormattedValue);
					if (_tlDrawerBalances.CashDrawer.IsNull)
						_tlDrawerBalances.CashDrawer.Value = 0;
					if (_tlDrawerBalances.EndingCash.IsNull)
						_tlDrawerBalances.EndingCash.Value = 0;


                    // WI#23798 - SPatterson
                    //if (Convert.ToDecimal(this.dfTotalCash.UnFormattedValue) > 0)
                    //    this.dfDifference.UnFormattedValue = _tlDrawerBalances.CashDrawer.Value - _tlDrawerBalances.EndingCash.Value;
                    //else
                    //    this.dfDifference.UnFormattedValue = _tlDrawerBalances.CashDrawer.Value +
                    //        _tlDrawerBalances.EndingCash.Value;
                    this.dfDifference.UnFormattedValue = _tlDrawerBalances.CashDrawer.Value - _tlDrawerBalances.EndingCash.Value;

					if (dfDifference.UnFormattedValue == null)
						dfDifference.UnFormattedValue = 0;
					//
					SetFieldColor();
				}
                /*Begin #86407 - need to refresh the cash drawer and difference amount*/
                else if (_tellerVars.CountedAmount == Decimal.MinValue)
                {
                    dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount == Decimal.MinValue ? 0 : _tellerVars.CountedAmount;
                    _tlDrawerBalances.EndingCash.Value = Convert.ToDecimal(dfTotalCash.UnFormattedValue);
                    _tlDrawerBalances.CashDrawer.Value = Convert.ToDecimal(dfCashDrawer.UnFormattedValue);
                    if (_tlDrawerBalances.CashDrawer.IsNull)
                        _tlDrawerBalances.CashDrawer.Value = 0;
                    if (_tlDrawerBalances.EndingCash.IsNull)
                        _tlDrawerBalances.EndingCash.Value = 0;
                    this.dfDifference.UnFormattedValue = _tlDrawerBalances.CashDrawer.Value - _tlDrawerBalances.EndingCash.Value;

                    if (dfDifference.UnFormattedValue == null)
                        dfDifference.UnFormattedValue = 0;
                   
                    SetFieldColor();
                }
                /* End #86407 */
            }
            else if (form.Name == "frmTlPositionHistory")
			{
				_tellerVars.DrawerCounted = _isDrawerCounted;
			}
            else if (form.Name == "frmTlCaptureBulkTran")   //#140895
            {
                if (this.pbBulkProcess.Enabled)
                    ProcessPendingBulkTran(false);
            }
            /*  Begin #3132
             * not sure what broke this but it use to work till build 5(2009) and broken from build 8 onwards.
             * putting a hack here. right fix would have been to set focus on the description field and call UpdateView on ActionPanel.
             * but doing so messes up the functionality. the description is not attached to a property and
             * loses value when focused and then new window launched. calling UpdateView(on workspace and/or ActionPanel
             * is not helping either. so doing this. it doesnt make sense, but works.
             */
            /*int selTab = this.picTabs.SelectedIndex;
            if (selTab == 0)
                this.Name0.Focus();
            else if (selTab == 1)
                this.Name1.Focus();
            else
                this.Name2.Focus();
             code moved to frmTlPosition_PShowCompletedEvent cause need to run code when this event is fired.*/
            frmTlPosition_PShowCompletedEvent(sender, e);
            //end #3132

		}
		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			try
			{
				if (callerInfo == "FormComplete")
				{
					//70958
					this.pbCurrentPod.Visible = (_tellerVars.IsAppOnline && (positionView.Value == 0 || positionView.Value == 3));
					this.pbLastPod.Visible = (_tellerVars.IsAppOnline && (positionView.Value == 0 || positionView.Value == 3));
					this.pbDailyPod.Visible = (_tellerVars.IsAppOnline && (positionView.Value == 0 || positionView.Value == 3));
                    //#28714
                    //this.pbEOB.Visible = (_tellerVars.IsAppOnline && (positionView.Value == 0 || positionView.Value == 3));
                    this.pbEOB.Visible = _tellerVars.IsAppOnline; //#28714

                    this.pbBulkProcess.Visible = (_tellerVars.IsAppOnline && (positionView.Value == 0 || positionView.Value == 3)); //#140895

                    //
                    this.pbProofTotals.Enabled = true; //#76409
					if (positionView.Value == 1 || positionView.Value == 2)
						this.pbHistory.Visible = false;
					if (_tellerVars.IsAppOnline && positionView.Value != 3)
						this.pbAdjTotals.Enabled = false;
					else
						this.pbAdjTotals.Enabled = true;
					//#72847

                    #region #79619 #140895
                    _tlDrawer.BranchNo.Value = branchNo.Value;
                    _tlDrawer.DrawerNo.Value = drawerNo.Value;
                    _tlDrawer.SelectAllFields = true;

                    _tlDrawer.ActionType = XmActionType.Select;
                    CoreService.DataService.ProcessRequest(_tlDrawer);
                    #endregion

                    this.pbEOB.Enabled = EnableDisableEOB();  //#28714

                    if ((positionView.Value == 0 && !_tellerVars.ClosedOut )|| positionView.Value == 3)
					{
                        if (positionView.Value == 0 && !_tellerVars.ClosedOut)    //#72847
						{
							this.dfDescription.Text = systemCloseOut;
							dfDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
							if (_tellerVars.IsOvernightRunning)
							{
								this.pbCloseOut.Enabled = false;
								this.ActionSave.Enabled = false;
                                this.pbRollover.Enabled = false; //#71916
                                this.pbRollover.Visible = false; //#74721
                                //this.pbEOB.Enabled = false; //#140895 //#28714
                                this.pbBulkProcess.Enabled = false; //#140895
                                this.pbMiddayCount.Enabled = false; //118226
                            }
							else
							{
								this.pbCloseOut.Enabled = true;
								this.ActionSave.Enabled = true;
                                this.pbProofTotals.Enabled = true; //#76409
                                this.pbMiddayCount.Enabled = true; //#118226

                                //Begin #72916
                                if (_tellerVars.IsTCDEnabled && _tellerVars.IsTCDConnected)
                                {
                                    this.pbRollover.Enabled = true;
                                    this.pbRollover.Visible = true; // #74721
                                }
                                else
                                {
                                    this.pbRollover.Enabled = false;
                                    this.pbRollover.Visible = false;// #74721
                                }
                                //End #72916
                                //this.pbEOB.Enabled = EnableDisableEOB();  //#28714
                                //#30969
                                if (AppInfo.Instance.IsAppOnline)
                                    this.pbBulkProcess.Enabled = ProcessPendingBulkTran(true);
                                else
                                    this.pbBulkProcess.Enabled = false;
							}
						}
						else if (positionView.Value == 3)
						{
							systemCloseOut = CoreService.Translation.GetUserMessageX(360169);	//System Close-Out by Supervisor
							dfDescription.Text = systemCloseOut + " " + _tellerVars.EmployeeId.ToString();

							dfDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
							this.ActionSave.Enabled = false;

							if (_tellerVars.BranchNo == branchNo.Value && _tellerVars.DrawerNo == drawerNo.Value)
							{
								this.pbTranTotals.Visible = false;
								this.pbBatchTotals.Visible = false;
								this.pbCountDrawer.Visible = false;
								this.pbCloseOut.Visible = false;
                                this.pbRollover.Visible = false; //#72916
                                this.pbProofTotals.Visible = false; //#76409
                                this.pbMiddayCount.Visible = false; //#118226
                            }
							else
							{
								this.pbTranTotals.Visible = true;
								this.pbBatchTotals.Visible = true;
								this.pbCountDrawer.Visible = true;
								this.pbCloseOut.Visible = true;
                                this.pbRollover.Visible = false; //#72916
                                this.pbProofTotals.Visible = true; //#76409
                                this.pbMiddayCount.Visible = false; //#118226-2
                            }
						}
					}
					else
					{
						this.dfDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
						this.ActionSave.Enabled = false;
						this.pbTranTotals.Visible = false;
						this.pbBatchTotals.Visible = false;
						this.pbCountDrawer.Visible = false;
						this.pbCloseOut.Visible = false;
                        this.pbRollover.Visible = false; //#72916
                        this.pbProofTotals.Visible = false; //#76409
                        //this.pbEOB.Visible = false;   //#28714
                        this.pbBulkProcess.Visible = false; //#140895
                        this.pbMiddayCount.Visible = false; //#118226

                        if (_tellerVars.ClosedOut)
							this.pbAdjTotals.Visible = false;
                        if (positionView.Value == 2 && (!positionPtid.IsNull && positionPtid.Value > 0))
                        {
                            this.pbCountDrawer.Visible = true;

                            //Begin 118229: Enable the Count Drawer push button only for close outs & midday cash counts
                            _reportPosPtid = _tlDrawerBalances.PositionPtid.Value;
                            CallXMThruCDS("TlPosition");

                            if (_tlPositionTemp.Description.Value != systemCloseOut && _tlPositionTemp.Description.Value != middayCashCount)
                            {
                                this.pbCountDrawer.Enabled = false;
                            }
                            //End 118229 
                        }
                        if (positionView.Value == 0 && _tellerVars.DrawerCounted)
                            /* Begin #86292 */
                            /* this.dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount;*/
                            this.dfCashDrawer.UnFormattedValue = _tellerVars.CountedAmount == Decimal.MinValue ? 0 : _tellerVars.CountedAmount;
                            /* End #86292 */
                    }
                    #region #76409
                    if (this._tellerVars.AdTlControl.RealTimeProof.Value != "Y")
                        this.pbProofTotals.Enabled = false;
                    #endregion

                    #region #79619 #140895
                    //_tlDrawer.BranchNo.Value = branchNo.Value;
                    //_tlDrawer.DrawerNo.Value = drawerNo.Value;
                    //_tlDrawer.SelectAllFields = true;

                    //_tlDrawer.ActionType = XmActionType.Select;
                    //CoreService.DataService.ProcessRequest(_tlDrawer);
                    if (_tlDrawer.NonCash.StringValue == "Y")
                        this.pbCountDrawer.Enabled = false;
                    #endregion
                }
				else if (callerInfo == "AfterCloseOut")
				{
					if (isRealCloseOut)
					{
						this.pbCloseOut.Enabled = false;
						this.pbCountDrawer.Enabled = false;
						this.pbTranTotals.Enabled = false;
						this.pbBatchTotals.Enabled = false;
                        this.pbMiddayCount.Enabled = false; //#118226-3
                        this.ActionSave.Enabled = false;
						if (this.pbAdjTotals.Visible)
							this.pbAdjTotals.Enabled = false;
						this.dfDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
						//
						//clear out global cache for drawer count status
						if (positionView.Value == 0)
						{
							_tellerVars.DrawerCounted = true;
						}
						else
						{
							_tellerVars.DrawerCounted = false;
							_isDrawerCounted = false;
							_tellerVars.CountedAmount = DbDecimal.Null;
						}
					}
					else
					{
						this.ActionSave.Enabled = false;
						this.dfDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);

                        //Begin Bug#118530
                        if (isMidDayCount)
                        {
                            this.pbCloseOut.Enabled = false;
                            this.pbMiddayCount.Enabled = false;
                        }
                        //End Bug#118530
                    }
                }
				else if (callerInfo == "HandleCloDesc")
				{
					if (!_tellerVars.IsOvernightRunning && positionView.Value == 0)
						ActionSave.Enabled = true;
					if (dfDescription.Text == string.Empty ||dfDescription.Text == null ||
						dfDescription.Text == "" || (dfDescription.Text != null && dfDescription.Text.Trim() == ""))
					{
						ActionSave.Enabled = false;
					}
				}

                #region #79619 #140895 - moved this code under FormComplete
                //_tlDrawer.BranchNo.Value = branchNo.Value;
                //_tlDrawer.DrawerNo.Value = drawerNo.Value;
                //_tlDrawer.SelectAllFields = true;

                //_tlDrawer.ActionType = XmActionType.Select;
                //CoreService.DataService.ProcessRequest(_tlDrawer);
                //if (_tlDrawer.NonCash.StringValue == "Y")
                //    this.pbCountDrawer.Enabled = false;
                #endregion

            }
            catch (PhoenixException peedvlogic)
			{
				PMessageBox.Show(peedvlogic);
			}
// 			catch (Exception eedvlogic)
// 			{
// 				MessageBox.Show(this, eedvlogic.Message + "\r\n" + eedvlogic.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
// 			}
		}

		private void CallXMThruCDS(string origin)
		{
			try
			{
				if (origin == "AfterCloseOut") // make this work for all branch and drawer loaded from dfwTlSupervisor grid
				{
					GetPositionView(false);
					RecalcTotals();
				}
				else if (origin == "ForceSelect")
				{
					_tlDrawerBalances.ActionType = XmActionType.Select;
					DataService.Instance.ProcessRequest(_tlDrawerBalances);
				}
				else if (origin == "TlPosition")
				{
					_tlPositionTemp.SelectAllFields = false;
					_tlPositionTemp.CreateDt.Selected = true;
					_tlPositionTemp.ClosedDt.Selected = true;
					_tlPositionTemp.Ptid.Value = _reportPosPtid;
					//_tlPositionTemp.Ptid.Value = positionPtid.Value;
					_tlPositionTemp.BranchNo.Value = branchNo.Value;
					_tlPositionTemp.DrawerNo.Value = drawerNo.Value;
					_tlPositionTemp.ClosedDt.Value = closedDate.Value;
					_tlPositionTemp.CashDrawer.Selected = true;
					_tlPositionTemp.JournalPtid.Selected = true;
					_tlPositionTemp.Ptid.Selected = true;
					_tlPositionTemp.EndingCash.Selected = true;
					_tlPositionTemp.DrawerNo.Selected = true;
					_tlPositionTemp.EmplId.Selected = true;
					_tlPositionTemp.BranchNo.Selected = true;
					_tlPositionTemp.BalDenomTracking.Selected = true;
					_tlPositionTemp.Description.Selected = true;
					_tlPositionTemp.ActionType = XmActionType.Select;
					DataService.Instance.ProcessRequest(_tlPositionTemp);
				}
				else if (origin == "AdGbBranch")
				{
					_adGbBranchBusObj = new AdGbBranch();
					_adGbBranchBusObj.ActionType = XmActionType.Select;
					_adGbBranchBusObj.SelectAllFields = false;
					_adGbBranchBusObj.Name1.Selected = true;
					if (!_tlDrawerBalances.BranchNo.IsNull)
						_adGbBranchBusObj.BranchNo.Value = _tlDrawerBalances.BranchNo.Value;
					else
						_adGbBranchBusObj.BranchNo.Value = _tlPosition.BranchNo.Value;
					DataService.Instance.ProcessRequest(_adGbBranchBusObj);
				}
				else if (origin == "TlDrwaer")
				{
					//_tlDrawer
					_tlDrawerTemp.SelectAllFields = false;
					_tlDrawerTemp.CurPostingDt.Selected = true;
					_tlDrawerTemp.BranchNo.Value = branchNo.Value;
					_tlDrawerTemp.DrawerNo.Value = drawerNo.Value;
					_tlDrawerTemp.ActionType = XmActionType.Select;
					DataService.Instance.ProcessRequest(_tlDrawerTemp);
				}
				else if (origin == "AdGbRsm")
				{
					_adGbRsmTemp = new AdGbRsm();
					_adGbRsmTemp.SelectAllFields = false;
					_adGbRsmTemp.EmployeeName.Selected = true;
					_adGbRsmTemp.TellerNo.Selected = true;
					_adGbRsmTemp.EmployeeId.Value = _reportEmplId; //_tlPosition.EmplId.Value;
					_adGbRsmTemp.ActionType = XmActionType.Select;
					DataService.Instance.ProcessRequest(_adGbRsmTemp);
				}
                else if (origin == "RolloverPosInsert") //#72916
                {
                    if (TellerVars.Instance.IsAppOnline)
                        DataService.Instance.ProcessRequest(_tempPosObj);
                    else
                    {
                        if (TellerVars.Instance.OfflineCDS != null)
                        {
                            TellerVars.Instance.OfflineCDS.ProcessRequest(_tempPosObj); //#72916*
                        }
                        else
                        {
                            DataService offlineService = new DataService(true);
                            offlineService.ProcessRequest(_tempPosObj); //#72916*
                        }
                    }
                }
			}
			catch(PhoenixException excds)
			{
				//360979 - %1!
				PMessageBox.Show(this, 360979, MessageType.Error, MessageBoxButtons.OK, (excds.Message + "\r\n" + excds.InnerException));
			}
		}

		private void SetFormDefaults()
		{
			this.curPostingDate.Value = _tellerVars.PostingDt;
		}

		private string FixScreenTitle()
		{
			string screenTitle = "";
			if (positionView.Value == 1)
			{
				screenTitle = CoreService.Translation.GetTokenizeMessageX(360172, new string[] {branchName.Value});
			}
			else
			{
				screenTitle = CoreService.Translation.GetTokenizeMessageX(360173, new string[] {Convert.ToString(drawerNo.Value)});
			}
			return screenTitle;
		}

		private bool CloseOutValidation()
		{
			if (isRealCloseOut)
			{
                //Begin #64564
                PInt numBackDays = new PInt("numBackDays");
                //Begin Bug#68380
                PDateTime curPostDate = new PDateTime("curPostDate");
                drawerNo.ValueObject  = this.drawerNo.Value;
                branchNo.ValueObject  = this.branchNo.Value;
                //End Bug#68380
                DataService.Instance.ProcessCustomAction(_tlPosition, "GetNoBackDays", numBackDays,curPostDate,branchNo,drawerNo);//Bug#68380-- Added curPostDate,branchNo,drawerNo
                if (curPostDate.Value != DateTime.MinValue) //94549- If curPostDate is not null,then show backdate message
                {
                    /* Task #124859 - If drawer cash balance is 0, then we dont need to validate for the backdating configuration */
                    if ((_tlDrawerBalances.EndingCash == null ? 0 : _tlDrawerBalances.EndingCash.Value) != 0)
                    {
                        if (curPostDate.Value < GlobalVars.SystemDate.AddDays(-1 * Convert.ToInt16(numBackDays.ValueObject))) //Bug#68380-- Modified if condition with curPostDate
                        {
                            //15754 - The posting date of the drawer is older than the number of days you are allowed to backdate.
                            //If the Drawer needs to be closed the required number of back days to be increased.
                            PMessageBox.Show(this, 15754, MessageType.Error, MessageBoxButtons.OK);


                            return false;
                        }
                    }
                    //End #64564
                }
                else //94549 - Show message to user, if drawer is already closed out.
                {
                    //#94549- The selected drawer has already been closed out.
                    PMessageBox.Show(this, 16027, MessageType.Error, MessageBoxButtons.OK);
                    return false;
                }
                if (_tellerVars.AdTlControl.DisplayCtrWarn.Value == GlobalVars.Instance.ML.Y && _tlDrawerBalances.CtrTranCount.Value > 0)
				{
					if (DialogResult.No == PMessageBox.Show(this, 317884, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
						return false;
                }

                #region #76409
                bool isProofPending = IsProofPending();

                if (isProofPending)
                {
                    PMessageBox.Show(this, 11132, MessageType.Error, MessageBoxButtons.OK);
                    return false;
                }
                #endregion

                if (!_tellerVars.DrawerCounted && !_tellerVars.IsNonCashDrawer) // #79619 added IsNonCashDrawer check.
				{
					PMessageBox.Show(this, 311084, MessageType.Warning, MessageBoxButtons.OK);
					return false;
				}

				/* Begin #71886 */
                if (drawerIsSignOn && _tellerVars.CashUpdateCounter != _tellerVars.CashUpdateCounterAtCount && !_tellerVars.IsNonCashDrawer)    // #79619 added IsNonCashDrawer check.
				{
					_tlDrawerBalances.CashDrawerSummary.Clear();

					// PMessageBox.Show( this,
					//	"The cash drawer may have changed since last count due to transactional activity. Please reverify your count.",
					//	"Phoenix Warning - #361001", MessageBoxButtons.OK, MessageBoxIcon.Asterisk );
					PMessageBox.Show(this, 361001, MessageType.Warning, MessageBoxButtons.OK);
					RefreshData( );
					return false;
				}
				/* End #71886 */

				if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
				{
					if (DialogResult.No == PMessageBox.Show(this, 311085, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
						return false;
				}
				else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
				{
					if (DialogResult.No == PMessageBox.Show(this, 311086, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
						return false;
				}

				try
				{
					Difference.ValueObject = dfDifference.UnFormattedValue;
					VerifyPOD.ValueObject = (_tellerVars.VerifyPOD ? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N);
					CashDrawer.ValueObject = dfCashDrawer.UnFormattedValue;
					TellerCashAcct.ValueObject = _tellerVars.CashAcctNo;

                    //Begin Task#60315 - Set the TellerCashAcct to avoid null when supervisor closeout drawer of another user without login to drawer
                    if (TellerCashAcct.IsNull)
                    {
                        DataService.Instance.ProcessCustomAction(_tlDrawer, "GetGlCashAcct", TellerCashAcct);
                    }
                    //End Task#60315

                    #region Pre-CloseOut
                    if (isRealCloseOut)
					{
						using (new WaitCursor())
						{
							this._tlDrawerBalances.MessageId.Value = 0;
							this._tlDrawerBalances.PosDescription.Value = this.dfDescription.Text;

							DataService.Instance.ProcessCustomAction(_tlDrawerBalances,"PreCloseOut",VerifyPOD,
								TellerCashAcct, Difference);
						}
						//
						#region Handle Supervisor Override
						HandleOverrides(Convert.ToDecimal(dfDifference.UnFormattedValue));
						#endregion
					}
					#endregion
				}
				catch( PhoenixException pe )
				{
					PMessageBox.Show( pe );
					return false;
				}
				if (this._tlDrawerBalances.MessageId.Value != 0)
				{
					PMessageBox.Show(this, this._tlDrawerBalances.MessageId.Value, MessageType.Warning, MessageBoxButtons.OK);
					dlgInformation.Instance.HideInfo();
					return false;
				}
			}

			return true;
		}

        private bool IsProofPending()
        {
            PDateTime postingDate = new PDateTime("postingDate");
            postingDate.Value = _tellerVars.PostingDt;
            PInt isProofedValue = new PInt("isProofedValue");
            PSmallInt drNo = new PSmallInt("drNo");
            drNo.Value = this.drawerNo.Value;
            Phoenix.BusObj.Teller.TlDrawerBalances _tempDrBalObj = new TlDrawerBalances();
            _tempDrBalObj.ActionType = XmActionType.Custom;
            _tempDrBalObj.BranchNo.Value = branchNo.Value;
            _tempDrBalObj.DrawerNo.Value = drNo.Value;
            _tempDrBalObj.ClosedDt.Value = Convert.ToDateTime(_tellerVars.PostingDt);
            DataService.Instance.ProcessCustomAction(_tempDrBalObj, "IsProofPending", postingDate, drNo, isProofedValue);
            if (isProofedValue.Value >= 1)
                return true;
            else
                return false;
        }

		private void GetPositionView( bool isInitialFormLoad)
		{
			_tlDrawerBalances.PosView.Value = positionView.Value;
			_tlDrawerBalances.BranchNo.Value = branchNo.Value;
			_tlDrawerBalances.DrawerNo.Value = drawerNo.Value;
			_tlDrawerBalances.ClosedDt.Value = closedDate.Value;
			_tlDrawerBalances.CrncyId.Value = _tellerVars.LocalCrncyId;
			_tlDrawerBalances.PositionPtid.Value = (int)positionPtid.Value;
			_tlDrawerBalances.PrevEmplId.Value = previousEmplId.Value;
			_tlDrawerBalances.NewEmplId.Value = newEmplId.Value;
			_tlDrawerBalances.OutputType.Value = 2;
			_tlDrawerBalances.TellerOffOnStatus.Value = (short)TranOfflineStatus.OnlineOnly;
			_tlDrawerBalances.SelectType.Value = 1;

			if (!isInitialFormLoad)
			{
				CallXMThruCDS("ForceSelect");
				this.ObjectToScreen(XmActionType.Select);
			}
		}

		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;
			PfwStandard tempDlg = null;

			if ( origin == "PositionHistory" )
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tlposition", "Phoenix.Client.TlPosition", "frmTlPositionHistory" );
				tempWin.InitParameters(branchNo.Value, drawerNo.Value);
			}
			else if ( origin == "BatchTotals" )
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tlbatch", "Phoenix.Windows.Client", "frmTlBatchTotals" );
				tempWin.InitParameters(branchNo.Value, drawerNo.Value, closedDate.Value, employeeId.Value);
			}
            else if (origin == "ProofTotals")   //#76409
            {
                tempWin = Helper.CreateWindow("phoenix.client.tlprooftotals", "phoenix.client.tlprooftotals", "frmTlProofTotals");
                tempWin.InitParameters(branchNo.Value, drawerNo.Value, _tellerVars.PostingDt.ToString()); // #4366
            }
			else if ( origin == "CashDrawerCount" )
			{
				// #67882 - pending
				if (_isBalDenomTracking)
				{
					tempWin = Helper.CreateWindow( "phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "frmTlCashDrawerSummary" );
					if (!positionPtid.IsNull && positionPtid.Value > 0) // view only
					{
						tempWin.InitParameters(branchNo.Value,
							drawerNo.Value,
							closedDate.Value,
							Convert.ToDecimal(this.dfTotalCash.UnFormattedValue),
							positionPtid.Value,
							null,
							null,
                            null);    //#140772 - Added new parameter value
					}
					else
					{
						tempWin.InitParameters(branchNo.Value,
							drawerNo.Value,
							closedDate.Value,
							Convert.ToDecimal(this.dfTotalCash.UnFormattedValue),
							null,
							_tlDrawerBalances.CashDrawerSummary,
							_tlDrawerBalances.CashCount,
                            _tlDrawerBalances.InvItemCount);    //#140772 - Added new parameter value
					}
				}
				else
				{
					if (!positionPtid.IsNull && positionPtid.Value > 0) // view only
					{
						tempWin = Helper.CreateWindow( "phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "frmTranDenomDetails" );
						tempWin.InitParameters( positionPtid.Value,
							null, //cash in
							null, //cash out
							null, //tcd cash in
							null, //tcd cash out
							null, //net amt
							null, //batch id
							null, //sub sequence
							this.ScreenId,
							Convert.ToDecimal(this.dfTotalCash.UnFormattedValue));
					}
					else
					{
						tempWin = Helper.CreateWindow( "phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "dlgTlCashCount" );
						tempWin.InitParameters( Convert.ToDecimal(this.dfTotalCash.UnFormattedValue) ,
							CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "C"),
							false,
							_tlDrawerBalances.CashCount,
							branchNo.Value,
							drawerNo.Value,
							closedDate.Value,
                            null,
                            _tlDrawerBalances.InvItemCount);    //#140772 - Added new parameter value
					}
				}
				//tempWin.Closed +=new EventHandler(tempWin_Closed);
			}
			else if ( origin == "DrawerAdj" )
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tlcashdrawer", "Phoenix.Client.TlCashDrawer", "frmTlCashDrawerAdj" );
				tempWin.InitParameters(branchNo.Value,
					drawerNo.Value,
					closedDate.Value,
					previousEmplId.Value,
					drawerCombo);
			}
			else if ( origin == "TranTotals" )
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tltrantotal", "Phoenix.Client.TlTranTotal", "frmTlJournalTranTotals" );
				tempWin.InitParameters(branchNo.Value, drawerNo.Value, closedDate.Value);
			}
			else if ( origin == "Override" )
			{
				tempWin = Helper.CreateWindow( "phoenix.client.tloverride", "Phoenix.Windows.TlOverride", "dlgTlSupervisorOverride" );
                tempWin.InitParameters("Position", _tlJournalOvrd, branchNo.Value, drawerNo.Value, closedDate.Value); //#79314 - added branch, drawer# and closed date params
			}
			else if ( origin == "PODTotals" )
			{
				tempDlg = Helper.CreateWindow( "phoenix.client.tlposition", "Phoenix.Client.TlPosition", "dlgPODTotals" );
				tempDlg.InitParameters(_podTotalsType, branchNo.Value, branchName.Value, drawerNo.Value, _tellerVars.PostingDt);
			}
            else if (origin == "BulkTran")  //#30473
            {
                tempWin = CreateWindow("Phoenix.Client.TlTranCode", "Phoenix.Client.Teller", "frmTlCaptureBulkTran");
                tempWin.InitParameters(_tellerCaptBulkTranXML, branchNo.Value, drawerNo.Value, _tellerCaptBulkBatchId, "Post");
            }
			if ( tempWin != null )
			{
				tempWin.Closed +=new EventHandler(tempWin_Closed);
				tempWin.Workspace = this.Workspace;
				tempWin.Show();
            }
			else if ( tempDlg != null )
			{
				dialogResult = tempDlg.ShowDialog();
			}
		}

		private void ScreenAccessConfig()
		{
			this.pbTranTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalTranTotals;
			this.pbBatchTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.BatchTotals;
            this.pbProofTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.TlProofTotals;  //#76409
			if (_isBalDenomTracking)
				this.pbCountDrawer.NextScreenId = Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals;
			else
				this.pbCountDrawer.NextScreenId = Phoenix.Shared.Constants.ScreenId.CashCount;
			this.pbCloseOut.NextScreenId = Phoenix.Shared.Constants.ScreenId.CloseOut;
			this.pbHistory.NextScreenId = Phoenix.Shared.Constants.ScreenId.PositionHistory;
			this.pbAdjTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.CashDrawerAdj;
			this.pbEOB.NextScreenId = Phoenix.Shared.Constants.ScreenId.CloseOut;   //#140895
		}

		private void RecalcTotals()
		{
			//
			decimal cashDrawer = 0;
            #region #76409
            decimal totalCheckAmt = 0;
            int totalChecks = 0;
            #endregion
            _tlDrawerBalances.CleanUpValues( true );
			//
			this.dfTCDUnbatchCashOut.UnFormattedValue = Convert.ToDecimal(0);

			this.dfTotalDeposits.UnFormattedValue = this._tlDrawerBalances.CashDep.Value +
				this._tlDrawerBalances.OnUsChksDep.Value  +
				this._tlDrawerBalances.ChksAsCashDep.Value +
				this._tlDrawerBalances.TransitChksDep.Value;

			this.dfTotalWithdrawals.UnFormattedValue =  this._tlDrawerBalances.OnUsChksCashed.Value +
				this._tlDrawerBalances.OthWd.Value +
				this._tlDrawerBalances.LnAdvances.Value +
				this._tlDrawerBalances.TfrDebits.Value  +
				this._tlDrawerBalances.ChksCashedWd.Value +
				this._tlDrawerBalances.OutWire.Value +
				this._tlDrawerBalances.AcctCloseouts.Value +
				this._tlDrawerBalances.OutFrWire.Value;

			this.dfTotalChecks.UnFormattedValue = this._tlDrawerBalances.Deposits.Value +
				this._tlDrawerBalances.TfrCredits.Value +
				this._tlDrawerBalances.AnalFeePmt.Value +
				this._tlDrawerBalances.Payments.Value +
				this._tlDrawerBalances.SafeDepPmt.Value +
				this._tlDrawerBalances.InWire.Value +
				this._tlDrawerBalances.InFrWire.Value;

			this.dfTCDCashDisp.UnFormattedValue = this._tlDrawerBalances.TcdCashOut.Value +
				this._tlDrawerBalances.TcdBatchAmt.Value;

			if (this.positionView.Value != 2)	//Not a History View
			{
				//begin 15864
				tempCashShort.ValueObject = 0;
				tempCashOver.ValueObject = 0;
				curPostingDate.ValueObject = this.curPostingDate.Value;
				drawerNo.ValueObject = this.drawerNo.Value;
				branchNo.ValueObject = this.branchNo.Value;
				closedDate.ValueObject = this.closedDate.Value;
				if (Convert.ToInt32(drawerNo.ValueObject) > 0)
				{
				    DataService.Instance.ProcessCustomAction(_tlPosition, "GetTotalShortOverCash", curPostingDate, drawerNo,branchNo, tempCashOver, tempCashShort);
				}
				else
				{
				    DataService.Instance.ProcessCustomAction(_tlPosition, "GetTotalShortOverCash", closedDate, drawerNo,branchNo, tempCashOver, tempCashShort);
				}
				this.dfCashOver.UnFormattedValue = Convert.ToDecimal(tempCashOver.ValueObject);
				this.dfCashShort.UnFormattedValue = Convert.ToDecimal(tempCashShort.ValueObject);

                //end 15864
				//#73218 - Fixed ending cash jumbled up problem	/*15864	 added tempcashover and cashshort values and or drawer conditions
                // Begin WI#29358
                //totalCash = ((_tellerVars.ClosedOut && drawerNo.Value == _tellerVars.DrawerNo) ? _tellerVars.SavedBeginningCash : this._tlDrawerBalances.ClosingCash.Value) +
				//this._tlDrawerBalances.CashIn.Value -
				//this._tlDrawerBalances.CashOut.Value -
				//this._tlDrawerBalances.UnbatchedAmt.Value +
				//((_tellerVars.ClosedOut || drawerNo.Value!= _tellerVars.DrawerNo)? Convert.ToDecimal(0) :  (this._tlDrawerBalances.CashOver.Value + Convert.ToDecimal(tempCashOver.ValueObject))) -
				//((_tellerVars.ClosedOut || drawerNo.Value != _tellerVars.DrawerNo) ? Convert.ToDecimal(0) : (this._tlDrawerBalances.CashShort.Value + Convert.ToDecimal(tempCashShort.ValueObject)));
                totalCash = ((_tellerVars.ClosedOut && drawerNo.Value == _tellerVars.DrawerNo) ? _tellerVars.SavedBeginningCash : this._tlDrawerBalances.ClosingCash.Value) +
                this._tlDrawerBalances.CashIn.Value -
                this._tlDrawerBalances.CashOut.Value -
                this._tlDrawerBalances.UnbatchedAmt.Value +
                ((_tellerVars.ClosedOut ) ? Convert.ToDecimal(0) : (this._tlDrawerBalances.CashOver.Value + Convert.ToDecimal(tempCashOver.ValueObject))) -
                ((_tellerVars.ClosedOut ) ? Convert.ToDecimal(0) : (this._tlDrawerBalances.CashShort.Value + Convert.ToDecimal(tempCashShort.ValueObject)));
                // End WI#29358 

                // Begin WI#29358
                //WI#13178	 //15864 added drawer condition.
                //if (!_tellerVars.ClosedOut && (curPostingDate.Value >= closedDate.Value) && drawerNo.Value == _tellerVars.DrawerNo)
                //    totalCash = totalCash - (this._tlDrawerBalances.CashOver.Value) + (this._tlDrawerBalances.CashShort.Value);
                if (!_tellerVars.ClosedOut && (curPostingDate.Value >= closedDate.Value) )
                    totalCash = totalCash - (this._tlDrawerBalances.CashOver.Value) + (this._tlDrawerBalances.CashShort.Value);
                // End WI#29358
				//WI#13178

				dfTotalCash.UnFormattedValue = totalCash;
			}
			else
				totalCash = (_tlDrawerBalances.EndingCash.IsNull? Convert.ToDecimal(0) : _tlDrawerBalances.EndingCash.Value);

			if (!positionPtid.IsNull && positionPtid.Value > 0)
				cashDrawer = _tlDrawerBalances.CashDrawer.Value;
			else
			{
                /* Begin #86292 */
                /*cashDrawer = (_tellerVars.DrawerCounted? _tellerVars.CountedAmount : this._tlDrawerBalances.CashDrawer.Value);*/
                if (_tellerVars.DrawerCounted)
                {
                    cashDrawer = _tellerVars.CountedAmount == Decimal.MinValue ? 0 : _tellerVars.CountedAmount;
                }
                else
                {
                    cashDrawer = this._tlDrawerBalances.CashDrawer.Value;
                }
                /* End #86292 */
            }


            // WI#23798 - SPatterson
            //if (Convert.ToDecimal(this.dfTotalCash.UnFormattedValue) > 0)
            //    this.dfDifference.UnFormattedValue = cashDrawer  - totalCash;
            //else
            //    this.dfDifference.UnFormattedValue = cashDrawer + totalCash;
            this.dfDifference.UnFormattedValue = cashDrawer - totalCash;


			if (positionView.Value == 4 && !previousEmplId.IsNull)
				dfSavedTime.UnFormattedValue = this._tlDrawerBalances.DrawerCurPostingDt.Value;

			dfTCDCashBought.UnFormattedValue = 0;
			dfTCDCashDisp.UnFormattedValue = 0;

			this.dfSavedTime.ObjectToScreen();
			this.dfSavedDateTime.ObjectToScreen();
			if (this._tlDrawerBalances.SavedDateTime.IsNull)
			{
				this.dfSavedTime.Visible = false;
				this.lblClosedDateTime.Visible = false;
			}
			if (!this._tlDrawerBalances.SavedDate.IsNull)
				this.dfSavedDateTime.UnFormattedValue = this._tlDrawerBalances.SavedDate.Value;
            //
            //#72916*
            this.dfTCDUnbatchCashOut.UnFormattedValue = this._tlDrawerBalances.TCDUnbatchCashOutAmt.Value;
            this.dfTCDCashDisp.UnFormattedValue = this._tlDrawerBalances.TcdCashOut.Value;
            this.dfTCDCashBought.UnFormattedValue = this._tlDrawerBalances.TcdCashBought.Value;
            this.dfTCDCashLoaded.UnFormattedValue = this._tlDrawerBalances.TcdAddCash.Value;
            this.dfTCDCashRemoved.UnFormattedValue = this._tlDrawerBalances.TcdRemoveCash.Value;
            #region #76409
            if (_tellerVars.AdTlControl.ItemType.Value == "Not On Us Checks" || _tellerVars.AdTlControl.ItemType.Value == "Both")
            {
                if (this._tlDrawerBalances.NoTranChksPrf.IsNull)
                    this._tlDrawerBalances.NoTranChksPrf.Value = 0;
                if (this._tlDrawerBalances.NoTranChksNoPrf.IsNull)
                    this._tlDrawerBalances.NoTranChksNoPrf.Value = 0;
                //this.dfNotOnUsChecksProofed.UnFormattedValue = this._tlDrawerBalances.NoTranChksPrf.Value + "   " + this._tlDrawerBalances.TranChksPrf.Value.ToString("C");
                //this.dfNotOnUsChecksNotProofed.UnFormattedValue = this._tlDrawerBalances.NoTranChksNoPrf.Value + "   " + this._tlDrawerBalances.TranChksNoPrf.Value.ToString("C");
                this.dfNotOnUsChecksProofed.UnFormattedValue = this._tlDrawerBalances.NoTranChksPrf.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.TranChksPrf.Value).ToString("C");
                this.dfNotOnUsChecksNotProofed.UnFormattedValue = this._tlDrawerBalances.NoTranChksNoPrf.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.TranChksNoPrf.Value).ToString("C");
                totalChecks += this._tlDrawerBalances.NoTranChksPrf.Value;
                totalChecks += this._tlDrawerBalances.NoTranChksNoPrf.Value;
                totalCheckAmt += this._tlDrawerBalances.TranChksPrf.Value;
                totalCheckAmt += this._tlDrawerBalances.TranChksNoPrf.Value;
            }
            if (_tellerVars.AdTlControl.ItemType.Value == "On Us Checks" || _tellerVars.AdTlControl.ItemType.Value == "Both")
            {
                if (this._tlDrawerBalances.NoOnUsChksPrf.IsNull)
                    this._tlDrawerBalances.NoOnUsChksPrf.Value = 0;
                if (this._tlDrawerBalances.NoOnUsChksNoPrf.IsNull)
                    this._tlDrawerBalances.NoOnUsChksNoPrf.Value = 0;
                this.dfOnUsChecksProofed.UnFormattedValue = this._tlDrawerBalances.NoOnUsChksPrf.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.OnUsChksPrf.Value).ToString("C");
                this.dfOnUsChecksNotProofed.UnFormattedValue = this._tlDrawerBalances.NoOnUsChksNoPrf.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.OnUsChksNoPrf.Value).ToString("C");
                //this.dfOnUsChecksProofed.UnFormattedValue = this._tlDrawerBalances.NoOnUsChksPrf.Value + "   " + this._tlDrawerBalances.OnUsChksPrf.Value.ToString("C");
                //this.dfOnUsChecksNotProofed.UnFormattedValue = this._tlDrawerBalances.NoOnUsChksNoPrf.Value + "   " + this._tlDrawerBalances.OnUsChksNoPrf.Value.ToString("C");
                totalChecks += this._tlDrawerBalances.NoOnUsChksPrf.Value;
                totalChecks += this._tlDrawerBalances.NoOnUsChksNoPrf.Value;
                totalCheckAmt += this._tlDrawerBalances.OnUsChksPrf.Value;
                totalCheckAmt += this._tlDrawerBalances.OnUsChksNoPrf.Value;
            }
            if (_tellerVars.AdTlControl.TellerCapture.Value == "Y")   //#180460
            {
                if (this._tlDrawerBalances.NoTlCaptTransitChks.IsNull)
                    this._tlDrawerBalances.NoTlCaptTransitChks.Value = 0;
                if (this._tlDrawerBalances.NoTlCaptOnUsChks.IsNull)
                    this._tlDrawerBalances.NoTlCaptOnUsChks.Value = 0;
                this.dfTlCaptTransitChks.UnFormattedValue = this._tlDrawerBalances.NoTlCaptTransitChks.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.TlCaptTransitChks.Value).ToString("C");
                this.dfTlCaptOnUsChks.UnFormattedValue = this._tlDrawerBalances.NoTlCaptOnUsChks.Value + "   " + Convert.ToDecimal(this._tlDrawerBalances.TlCaptOnUsChks.Value).ToString("C");
                totalChecks += this._tlDrawerBalances.NoTlCaptTransitChks.Value;
                totalChecks += this._tlDrawerBalances.NoTlCaptOnUsChks.Value;
                totalCheckAmt += this._tlDrawerBalances.TlCaptTransitChks.Value;
                totalCheckAmt += this._tlDrawerBalances.TlCaptOnUsChks.Value;
            }
            if (_tellerVars.AdTlControl.ItemType.Value != null)
                this.dfTotal.UnFormattedValue = totalChecks + "   " + Convert.ToDecimal(totalCheckAmt).ToString("C");

            #endregion
        }

		private void UpdateDrawerBalance()
		{
            if (!_supervisorFlag)       // WI#35358
            {
                if (drawerCombo != null && drawerNo.Value == _tellerVars.DrawerNo)
                {
                    drawerCombo.Items.Clear();
                }
            }
		}

		private void SyncUpOfflineDrawerBalance()
		{
			Phoenix.BusObj.Control.PcOfflineTables offlineHelper;
			if ( TellerVars.Instance.OfflineCDS != null )
			{
				offlineHelper = new Phoenix.BusObj.Control.PcOfflineTables();
				try
				{
					dlgInformation.Instance.ShowInfo("Synching Offline Drawer Balances..." );
					offlineHelper.ReplicateDrawerInfo();
				}
				catch( PhoenixException pe )
				{
					PMessageBox.Show( pe, MessageBoxButtons.OK );
					return;
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
				}
			}
		}

		private bool DrawerCloseOut()
		{
            bool isInsertRollOverInfo = false; //#72916
            int messageId = 0;
            string messageParamText = string.Empty;
            decimal netRollOverAmount = 0; //#72916
            decimal actualNetAmt = 0; //WI#22234
            decimal actualcointAmt = 0; //WI#22234
            DialogResult rollOverDialog = new DialogResult();
            _deviceOutput = new ArrayList(); //#72916*
            #region handle printing
			// handle message pop-up for #1088
			
            //Begin 118226-2
            if (isMidDayCount)
            {
                _printReportSilent = false;
            }
            else
            {
                // handle message pop-up for #1088
                if (DialogResult.Yes == PMessageBox.Show(this, 311088, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
                {
                    _printReportSilent = true;
                }
                else
                    _printReportSilent = false;
                // uncomment the error message condition when cash drawer count report available
                //			if (DialogResult.Yes == PMessageBox.Show(this, 360661, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
                //				GenerateReport();
            }
            //End 118226-2
            #endregion
            //
            _isDrawerClosedOut = false;
			if (isRealCloseOut)
			{
				//if (_tellerVars.VerifyPOD)
				//	dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360170));
				//else
				dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360171));
				//360171 - Please wait while the system closes out the teller drawer...
			}
            //Begin 118226
            else if (isMidDayCount)
            {
                //16417 - Saving Midday Cash Count.
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(16417));
            }
            //End 118226
            else
            {
				dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360185));
			}

			try
            {
                #region tcd rollover
                if (isRealCloseOut && _tellerVars.IsTCDEnabled) //#72916* - removed the TCDConnection check
                {
                    Phoenix.BusObj.Teller.TlPosition _tempPosObj = new Phoenix.BusObj.Teller.TlPosition();
                    Phoenix.BusObj.Teller.TlDrawerBalances _tempDrBalObj = new TlDrawerBalances();
                    _tempDrBalObj.ActionType = XmActionType.Custom;
                    _tempDrBalObj.BranchNo.Value = branchNo.Value;
                    _tempDrBalObj.DrawerNo.Value = _tellerVars.TcdDrawerNo;
                    _tempDrBalObj.ClosedDt.Value = Convert.ToDateTime(_tellerVars.PostingDt);
                    PInt PosPtid = new PInt("PosPtid");
                    DataService.Instance.ProcessCustomAction(_tempDrBalObj, "GetPosPtid", PosPtid);
                    if (!PosPtid.IsNull && PosPtid.Value > 0)
                    {
                        messageId = 361079;
                        messageParamText = _tellerVars.TcdMachineId.ToString();
                    }
                    else
                    {
                        messageId = 361080;
                        messageParamText = _tellerVars.TcdMachineId.ToString();
                    }
                    //
                    if (messageId > 0 && _tellerVars.PostingDt >= GlobalVars.SystemDate) //#72916*
                    {
                        rollOverDialog = PMessageBox.Show(this, messageId, MessageType.Question, MessageBoxButtons.YesNoCancel, new string[] { messageParamText });
                        if (rollOverDialog == DialogResult.Cancel)
                            return true;
                        else if (rollOverDialog == DialogResult.Yes)
                        {

                            /* #72916 - Issue Fix */
                            dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361098));

                            try
                            {
                                InitializeCashDispense();
                                //#9624
                                #region Turn OFF Allow Inventory Zeros to allow Rollover
                                //#79574
                                string allowInventory = string.Empty; //#11026
                                cashDisp.GetIniFileSettings("General", "Allow Inventory Zeroes", string.Empty, ref allowInventory, string.Empty); //#11026
                                cashDisp.SetIniFileSettings("General", "Allow Inventory Zeroes", "NO", string.Empty); //#11026
                                //cashDisp.Open(); //force open
                                #endregion
                                //
                                if (cashDisp.WindowTitle != "TCD/TCR Rollover")
                                {
                                    cashDisp.IsConnOpen = false;
                                    cashDisp.WindowTitle = "TCD/TCR Rollover";
                                }
                                //
                                #region call status
                                //WI#22234
                                cashDisp.GetStatus(drawerNo.Value.ToString(), true, _deviceOutput);
                                if (_deviceOutput != null && _deviceOutput.Count > 0)
                                {
                                    foreach (DeviceOutputDetails statusout in _deviceOutput)
                                    {
                                        if (statusout.DenOutputType == 'T' && statusout.ActualNet > 0) //#75739
                                        {
                                            actualNetAmt = statusout.ActualNet;
                                            break;
                                        }
                                    }
                                }
                                _deviceOutput.Clear();
                                //end WI#22234

                                #endregion
                                #region call totals
                                cashDisp.GetTotals(drawerNo.Value.ToString(), false, _deviceOutput, false);    //#79574 - added clear all/rollover changes
                                cashDisp.SetIniFileSettings("General", "Allow Inventory Zeroes", allowInventory, string.Empty); //#11026
                                #endregion
                                //
                                //WI#22234 - Calculate the coinAmt
                                if (_deviceOutput != null && _deviceOutput.Count > 0)
                                {
                                    foreach (DeviceOutputDetails devOut in _deviceOutput)
                                    {
                                        if (devOut.DenOutputType == 'C' && devOut.CoinAmount >= 0) //#75739
                                        {
                                            actualcointAmt = devOut.CoinAmount;
                                            break;
                                        }
                                    }
                                }
                                actualNetAmt = actualNetAmt + actualcointAmt;
                                //end WI#22234
                                #region handle rollover
                                if (_deviceOutput != null && _deviceOutput.Count > 0) //#72916*
                                {
                                    foreach (DeviceOutputDetails devOut in _deviceOutput)
                                    {
                                        if (devOut.DenOutputType == 'T' && devOut.NetAmtAfterRollover >= 0) //#75739
                                        {
                                            netRollOverAmount = devOut.NetAmtAfterRollover;
                                            isInsertRollOverInfo = true;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    //A TCD Roll Over was not completed for this close out.
                                    PMessageBox.Show(this, 361099, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                                }
                                #endregion
                            }
                            finally
                            {
                                dlgInformation.Instance.HideInfo();
                            }
                        }
                    }


                }
                #endregion

                #region Close-Out
                using (new WaitCursor())
				{
					PostingDt.ValueObject = Convert.ToDateTime(_tellerVars.PostingDt);
					ClosingDt.ValueObject = Convert.ToDateTime(closedDate.Value);
					SystemDt.ValueObject = Convert.ToDateTime(GlobalVars.SystemDate);
					Difference.ValueObject = dfDifference.UnFormattedValue;
					VerifyPOD.ValueObject = (_tellerVars.VerifyPOD ? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N);
					CashDrawer.ValueObject = dfCashDrawer.UnFormattedValue;
					TellerCashAcct.ValueObject = _tellerVars.CashAcctNo;
                    //IsFromCloseOut.ValueObject = (isRealCloseOut ? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N);                     //118226
                    IsFromCloseOut.ValueObject = (isRealCloseOut ? GlobalVars.Instance.ML.Y : isMidDayCount ? "M" : GlobalVars.Instance.ML.N); //118226
                    _tellerVars.SavedBeginningCash = Convert.ToDecimal(this.dfBeginningCash.UnFormattedValue);
					this._tlDrawerBalances.MessageId.Value = 0;
					this._tlDrawerBalances.PosDescription.Value = this.dfDescription.Text;
					this._tlDrawerBalances.GlobalEmplId.Value = _tellerVars.EmployeeId;
                    this._tlDrawerBalances.TcdDrawerNo.Value = _tellerVars.TcdDrawerNo; //#72916*
                    this._tlDrawerBalances.TcdDrawerPosition.Value = _tellerVars.TcdStationId; //#72916*
					BalDenomTracking.ValueObject = _tellerVars.AdTlControl.BalDenomTracking.Value;
					this._tlDrawerBalances.EndingCash.Value = Convert.ToDecimal(dfTotalCash.UnFormattedValue);

					if (_tlDrawerBalances.CashDrawerSummary.Count > 0 || _tlDrawerBalances.CashCount.Count > 0)
					{
						_tlDrawerBalances.CloDenomDetails.Value = _tlDrawerBalances.LoadStructureToXML();
					}

					ForwardFrom.ValueObject = false;
					DataService.Instance.ProcessCustomAction(_tlDrawerBalances,"CloseOut",ClosingDt,PostingDt,
					Difference,VerifyPOD, CashDrawer, SystemDt, TellerCashAcct, IsFromCloseOut, SuperEmplId, TlJrnlPtid, ForwardFrom, BalDenomTracking,
						RptPosPtid, RptPosDesc, RptPosCreatDt, RptPosCloseDt);
					//#71243
					if (Convert.ToDecimal(dfDifference.UnFormattedValue) != 0 && dfDifference.UnFormattedValue != null)
					{
						if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
							this.dfCashOver.UnFormattedValue = Convert.ToDecimal(dfDifference.UnFormattedValue);
						else
							this.dfCashShort.UnFormattedValue = Convert.ToDecimal(dfDifference.UnFormattedValue) * -1;
						this.dfCashOver.ScreenToObject();
						this.dfCashShort.ScreenToObject();
						//begin 15864
						tempCashOver.ValueObject = 0;
						tempCashShort.ValueObject = 0;
						DataService.Instance.ProcessCustomAction(_tlPosition, "GetTotalShortOverCash", curPostingDate, drawerNo,branchNo, tempCashOver, tempCashShort);
						this.dfCashOver.UnFormattedValue = Convert.ToDecimal(tempCashOver.ValueObject);
						this.dfCashShort.UnFormattedValue = Convert.ToDecimal(tempCashShort.ValueObject);
					   //end 15864
					}
                    //WI#11962
                    TlJrnlPtidCO = TlJrnlPtid.DecimalValue;
                    //WI#11962

                    //Home#72916
                    //#9469 - Modified the rollover condition since it was causing the problem for supervisor drawer close-out option
                    //if (isInsertRollOverInfo && positionView.Value == 0 && isRealCloseOut)
                    if (isInsertRollOverInfo && isRealCloseOut && _tellerVars.IsTCDEnabled) //#9469
                    {
                        //WI#22234
                        if (actualNetAmt == 0) //handle TCD's as they do not have actualNet
                        {
                            actualNetAmt = netRollOverAmount;
                        }
                        //end WI#22234
                        InsertTcdRollOver(netRollOverAmount, actualNetAmt, TlJrnlPtid.DecimalValue); //WI#22234
                        //
                        #region #79574
                        //insert RLO transaction into tl_journal
                        DataService.Instance.ProcessCustomAction(_tlDrawerBalances, "TcdTcrDeviceRollover", TlJrnlPtid);
                        //insert RLO denom details
                        if (!TlJrnlPtid.IsNull && _deviceOutput != null && _deviceOutput.Count > 0)
                        {
                            RolloverCasbDenom(TlJrnlPtid);
                        }
                        #endregion
                    }

				}
				#endregion

				#region Print the Reports
				if (_printReportSilent)
				{
					if (TellerVars.Instance.IsAppOnline)
						GenerateReport( true );
					else
						GenerateOfflineReport(false, false); //Silent printing No Messages...

					PrintDrawerCashCount( false, false );
				}
				#endregion
				//
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe );
				return false;
			}
			finally
			{
				dlgInformation.Instance.HideInfo();
			}

			if (this._tlDrawerBalances.MessageId.Value != 0)
			{
				PMessageBox.Show(this, this._tlDrawerBalances.MessageId.Value, MessageType.Warning, MessageBoxButtons.OK);
				return false;
			}

            if (_showTransportMonitorWarning)   //#140895
            {
                _showTransportMonitorWarning = false;
                PMessageBox.Show(this, 14451, MessageType.Message, MessageBoxButtons.OK, string.Empty);
            }

			if (positionView.Value == 0 && isRealCloseOut)	// set global variable for the successful system close-out
			{
				_tellerVars.ClosedOut = true;
				if (positionView.Value == 0)
				{
					_tellerVars.DrawerCounted = true;
					_isDrawerCounted = true;
				}
				else
				{
					_tellerVars.DrawerCounted = false;
					_isDrawerCounted = false;
				}
				UpdateDrawerBalance();
				SyncUpOfflineDrawerBalance();
				PMessageBox.Show(this, 311092, MessageType.Warning, MessageBoxButtons.OK);
			}
			if ((positionView.Value == 0 || positionView.Value == 3) && isRealCloseOut) // just for message pop-up
				_isDrawerClosedOut = true;
			EnableDisableVisibleLogic("AfterCloseOut");
			return true;
        }

        #region #79574
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (_isOkToBlockF1)
                hevent.Handled = true;
            else
                base.OnHelpRequested(hevent);
        }

        private void RolloverCasbDenom(PBaseType TlJrnlPtid)
        {
            ArrayList TcdRollOverDenoms = new ArrayList();


            bool coinUpdated = false;
            int denomCount = 0;
            decimal denomValue = 0;
            decimal coinAmount = 0;

            try
            {
                #region IdentifyTran
                if (_deviceOutput != null)
                {
                    /* #72916 - Issue Fix */
                    if (_deviceOutput.Count > 0) //#72916*
                    {

                        #region LoadRollOverCashDenom

                        #region LoadObject
                        if (!TlJrnlPtid.IsNull && TlJrnlPtid.DecimalValue > 0)
                        {
                            _tlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
                            _tlCashCount.BranchNo.Value = branchNo.Value;
                            _tlCashCount.DrawerNo.Value = drawerNo.Value;
                            _tlCashCount.EffectiveDt.Value = GlobalVars.SystemDate;
                            _tlCashCount.LoadCashCountDenom(TcdRollOverDenoms,
                             CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "R"), true);
                        }
                        #endregion

                        if (_deviceOutput != null)
                        {
                            #region LoadCashDenomCounts
                            foreach (DeviceOutputDetails devOut in _deviceOutput)
                            {
                                if (devOut.DenomCount > 0 && devOut.DenOutputType.ToString() == "B") //#75738
                                {
                                    denomCount = devOut.DenomCount;
                                    denomValue = devOut.DenomValue;
                                    foreach (Phoenix.BusObj.Teller.TlCashCount rollOverCount in TcdRollOverDenoms)
                                    {
                                        if (rollOverCount.Denom.Value == (decimal)devOut.Denom && rollOverCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "LB"))
                                        {
                                            rollOverCount.Quantity.Value = denomCount;
                                            rollOverCount.Amt.Value = (decimal)denomValue;
                                            break;
                                        }
                                    }
                                }
                                if (!coinUpdated && devOut.DenOutputType.ToString() == "C")
                                {
                                    foreach (Phoenix.BusObj.Teller.TlCashCount rollOverCount in TcdRollOverDenoms)
                                    {
                                        coinAmount = devOut.CoinAmount; //#72916
                                        if (rollOverCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "BC"))
                                        {
                                            rollOverCount.CountValue.Value = (decimal)coinAmount;
                                            rollOverCount.Quantity.Value = 1;
                                            rollOverCount.Amt.Value = (decimal)coinAmount;
                                            break;
                                        }
                                    }
                                    coinUpdated = true;
                                }


                            }
                            #endregion

                            #region SaveRollOverDenom

                            Phoenix.FrameWork.CDS.DataService.Instance.Reset();

                            foreach (Phoenix.BusObj.Teller.TlCashCount rollOverDenom in TcdRollOverDenoms)
                            {
                                if (rollOverDenom.Amt.Value > 0)
                                {
                                    rollOverDenom.JournalPtid.Value = Convert.ToInt32(TlJrnlPtid.ValueObject);
                                    rollOverDenom.RowVersion.Value = 1;
                                    rollOverDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "R"));
                                    rollOverDenom.ActionType = XmActionType.New;
                                    rollOverDenom.CopyBackStatus.Value = 1;     //#79574
                                    Phoenix.FrameWork.CDS.DataService.Instance.AddObject(rollOverDenom);

                                    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
                                    Phoenix.FrameWork.CDS.DataService.Instance.Reset();
                                }
                            }
                            #endregion
                        }
                        #endregion
                        //End #75742
                    }
                }
                #endregion
            }
            finally
            {
                if (_deviceOutput != null && _deviceOutput.Count != 0)
                {
                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11624));
                }
                dlgInformation.Instance.HideInfo();
                _isOkToBlockF1 = false;
                if (tcdAction != null)
                    tcdAction.Shortcut = Keys.None;
            }
        }
        #endregion

        private void HandleOverrides(decimal diffAmt)
		{
			isRequireSuperOvrd = false;

			if (diffAmt < 0 || diffAmt > 0)
			{
				_tlJournalOvrd = new TlJournalOvrd();
				if (diffAmt > 0)
					_tlJournalOvrd.OvrdId.Value = OverrideID.OverLimit;
				else
					_tlJournalOvrd.OvrdId.Value = OverrideID.ShortLimit;
				_tlJournalOvrd.AcctNo.Value = "";
				_tlJournalOvrd.AcctType.Value = "";
				_tlJournalOvrd.TranCode.Value = (diffAmt > Convert.ToDecimal(0)? Convert.ToInt16(506) : Convert.ToInt16(556));
				_tlJournalOvrd.Amount.Value = (diffAmt > 0? diffAmt : (diffAmt * -1));
				//
				if (_tlJournalOvrd.RequireOverShtLimitOvr(_tlJournalOvrd.OvrdId.Value, _adGbRsm,
					_tellerVars.AdGbRsmLimits, (diffAmt > Convert.ToDecimal(0)? diffAmt : (diffAmt * -1))))
				{
					isRequireSuperOvrd = true;
					CallOtherForms( "Override" );
					return;
				}
				else
					DrawerCloseOut();
			}
			else
				DrawerCloseOut();
			//
		}

		private bool CalcPODTotals()
		{
			// 1 - Current POD Totals
			// 2 - Last POD Totals
			// 3 - Daily POD Totals
			if (dfTotalUnbatchedAmount.UnFormattedValue != null)
			{
				if (Convert.ToDecimal(dfTotalUnbatchedAmount.UnFormattedValue) > 0)
				{
					PMessageBox.Show(this, 360659, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					return false;
				}
			}
			//
			PodTotalsType.ValueObject = _podTotalsType;
			//
			CallOtherForms("PODTotals");
			return true;
		}
		#endregion

		#region frmTlPosition_PMdiPrintEvent
		private void frmTlPosition_PMdiPrintEvent(object sender, EventArgs e)
		{
			if (TellerVars.Instance.IsAppOnline)
				GenerateReport(false);
			else
				GenerateOfflineReport(false, true);
		}

		#region GenerateReport
		public void GenerateReport(bool silentPrinting)
		{
			//360625 - Generating teller summary position report.  Please wait...
			dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360625));
			//71440 - These window has heavy trafic and creating lot of trouble so I have moved the code back here...
			try
			{
				//_htmlPrinter = new HtmlPrinter();
				_pdfFileManipulation = new PdfFileManipulation();
			}
			catch(System.Runtime.InteropServices.COMException ex)
			{
				CoreService.LogPublisher.LogDebug("\n(frmtlPosotion window/ GenerateReport)For some reason creating of HtmlPrinter Failed." + ex.Message);
			}
			//
			Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();
			report1.ReportName.Value = "TLO11000.sqr";
			report1.EmplId.Value = GlobalVars.EmployeeId;
			report1.FromDt.Value = GlobalVars.SystemDate;
			report1.ToDt.Value = GlobalVars.SystemDate;
			report1.RunDate.Value = DateTime.Now;
            report1.ExecutionMode.Value = Phoenix.BusObj.Misc.SQRExecutionMode.Online.ToString(); //#76458
			string desc;
			string tellerNoName = GlobalVars.EmployeeName;
			desc = dfDescription.Text.Trim();
			string CloseOutDateTime;
			string CloseOutPostingDate;
			string reportTitle;

			#region Report Title
			if (positionView.IntValue == 0)//returnGrandTotal
			{
				//21 = Teller Summary Position
				reportTitle = CoreService.Translation.GetTokenizeMessageX(360628, drawerNo.StringValue);
			}
			else if (positionView.IntValue == 1)
			{
				if (returnGrandTotal)
					reportTitle = CoreService.Translation.GetUserMessageX(360635); //360635 --  Bank Summary Report
				else
					reportTitle = CoreService.Translation.GetUserMessageX(360632); //360632 - Branch Summary Position
			}
			else if (positionView.IntValue == 4)
			{
				if (returnGrandTotal)
					reportTitle = CoreService.Translation.GetUserMessageX(360632); //360632 --  Bank Summary Report
				else
					reportTitle = CoreService.Translation.GetTokenizeMessageX(360628, drawerNo.StringValue); //360632 - Branch Summary Position

			}
			else
			{
				reportTitle = CoreService.Translation.GetTokenizeMessageX(360629, drawerNo.StringValue); //360629 - Teller History Summary Position
			}
			#endregion

			#region Set Null Values to Zero
			string draftPurch;
			if (dfDraftPurch.UnFormattedValue == null)
				draftPurch = "0";
			else
				draftPurch = dfDraftPurch.UnFormattedValue.ToString();
			//
			string fCExch;
			if (dfFCExch.UnFormattedValue == null)
				fCExch = "0";
			else
				fCExch = dfFCExch.UnFormattedValue.ToString();
			//
			string fCPurch;
			if (dfFCPurch.UnFormattedValue == null)
				fCPurch = "0";
			else
				fCPurch = dfFCPurch.UnFormattedValue.ToString();
			//
			string chkExch;
			if (dfChkExch.UnFormattedValue == null)
				chkExch = "0";
			else
				chkExch = dfChkExch.UnFormattedValue.ToString();
			//
			string onUsExch;
			if (dfOnUsExch.UnFormattedValue == null)
				onUsExch = "0";
			else
				onUsExch = dfOnUsExch.UnFormattedValue.ToString();
			//
			string cashOver;
			if (dfCashOver.UnFormattedValue == null)
				cashOver = "0";
			else
				cashOver = dfCashOver.UnFormattedValue.ToString();
			//
			string cashShort;
			if (dfCashShort.UnFormattedValue == null)
				cashShort = "0";
			else
				cashShort = dfCashShort.UnFormattedValue.ToString();
            //
            #region #76409
            string transitChksPrfd;
            if (dfNotOnUsChecksProofed.UnFormattedValue == null)
                transitChksPrfd = "0";
            else
                transitChksPrfd = dfNotOnUsChecksProofed.UnFormattedValue.ToString();
            //
            string onusChksPrfd;
            if (dfOnUsChecksProofed.UnFormattedValue == null)
                onusChksPrfd = "0";
            else
                onusChksPrfd = dfOnUsChecksProofed.UnFormattedValue.ToString();
            //
            string transitChksNotPrfd;
            if (dfNotOnUsChecksNotProofed.UnFormattedValue == null)
                transitChksNotPrfd = "0";
            else
                transitChksNotPrfd = dfNotOnUsChecksNotProofed.UnFormattedValue.ToString();
            //
            string onusChksNotPrfd;
            if (dfOnUsChecksNotProofed.UnFormattedValue == null)
                onusChksNotPrfd = "0";
            else
                onusChksNotPrfd = dfOnUsChecksNotProofed.UnFormattedValue.ToString();
            #endregion

            #endregion

            if (positionView.Value == 2)
			{
				_reportPosPtid = _tlDrawerBalances.PositionPtid.Value;
				CallXMThruCDS("TlPosition");
				CloseOutPostingDate = Convert.ToDateTime(dfSavedDateTime.UnFormattedValue).ToString("MM/dd/yyyy");
				CloseOutDateTime = Convert.ToDateTime(dfSavedTime.UnFormattedValue).ToString("MM/dd/yyyy hh:mm:ss tt");
				//Because we do not know who originally was the Teller Print current employee
				if (_tlPositionTemp.EmplId.IsNull)
					tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
				else
				{
					_reportEmplId = _tlPositionTemp.EmplId.Value;
					CallXMThruCDS("AdGbRsm");
					tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
				}
			}
			else
			{
				if (silentPrinting || _printReportSilent) //We are still on the same window so do the same trick
				{
					desc = (RptPosDesc.IsNull?"":RptPosDesc.StringValue);
					CloseOutPostingDate = Convert.ToDateTime(RptPosCloseDt.ValueObject).ToString("MM/dd/yyyy");
					CloseOutDateTime = Convert.ToDateTime(RptPosCreatDt.ValueObject).ToString("MM/dd/yyyy hh:mm:ss tt");
					if (!_tlDrawerBalances.DrawerCurEmplId.IsNull && _tlDrawerBalances.DrawerCurEmplId.Value != GlobalVars.EmployeeId)
					{
						_reportEmplId = _tlDrawerBalances.DrawerCurEmplId.Value;
						CallXMThruCDS("AdGbRsm");
						tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
					}
					else
						tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
				}
				else
				{
					if (dfDescription.Enabled)
						desc = desc + " " + "*** Pending";
					CloseOutPostingDate = Convert.ToDateTime(dfSavedDateTime.UnFormattedValue).ToString("MM/dd/yyyy");
					if (dfSavedTime.Visible && dfSavedTime.UnFormattedValue != null)
						CloseOutDateTime = Convert.ToDateTime(dfSavedTime.UnFormattedValue).ToString("MM/dd/yyyy hh:mm:ss tt");
					else
						CloseOutDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

					if (positionPtid.IsNull && _tlDrawerBalances.Status.Value == "Open")
					{
						if (!_tlDrawerBalances.DrawerCurEmplId.IsNull && _tlDrawerBalances.DrawerCurEmplId.Value != GlobalVars.EmployeeId)
						{
							_reportEmplId = _tlDrawerBalances.DrawerCurEmplId.Value;
							CallXMThruCDS("AdGbRsm");
							tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
						}
						else
							tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
					}
					else
					{
						//
						if (employeeId.Value > 0 &&  employeeId.Value != GlobalVars.EmployeeId)
						{
							_reportEmplId = employeeId.Value;
							CallXMThruCDS("AdGbRsm");
							tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
						}
						else
							tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
					}
				}
			}
			//I am using the shortcut, sorry
			//'View=~BranchNo=~DrawerNo=~ClosedDt=~PositionPTID=~PrevEmplID=~NewEmplID=~CrncyID=~GrandTot='
			report1.Param1.Value =
				"View="					+ positionView.StringValue						+ "~" +
				"BranchNo="				+ branchNo.StringValue							+ "~" +
				"DrawerNo="				+ drawerNo.StringValue							+ "~" +
				"ClosedDt="				+ closedDate.StringValue						+ "~" +
				"PositionPTID="			+ positionPtid.StringValue						+ "~" +
				"PrevEmplID="			+ previousEmplId.StringValue					+ "~" +
				"NewEmplID="			+ newEmplId.StringValue							+ "~" +
				"CrncyID="				+ CurrencyId.ToString()							+ "~" +
				"GrandTot="				+ Convert.ToInt16(returnGrandTotal).ToString()	+ "~" +
				"Desc="					+ desc											+ "~" +
				"Teller="				+ tellerNoName                                  + " ~" +
				"reportTitle="          + reportTitle;
            //#140772
            report1.Param2.Value =
                "BeginningCash=" + ((dfBeginningCash.UnFormattedValue == null) ? "0" : dfBeginningCash.UnFormattedValue.ToString()) + "~" +
                "CashIns=" + ((dfCashIns.UnFormattedValue == null) ? "0" : dfCashIns.UnFormattedValue.ToString()) + "~" +
                "CashOuts=" + ((dfCashOuts.UnFormattedValue == null) ? "0" : dfCashOuts.UnFormattedValue.ToString()) + "~" +
                "TotalUnbatchedAmount=" + ((dfTotalUnbatchedAmount.UnFormattedValue == null) ? "0" : dfTotalUnbatchedAmount.UnFormattedValue.ToString()) + "~" +
                "TotalCash=" + ((dfTotalCash.UnFormattedValue == null) ? "0" : dfTotalCash.UnFormattedValue.ToString()) + "~" +
                "CashDrawer=" + ((dfCashDrawer.UnFormattedValue == null) ? "0" : dfCashDrawer.UnFormattedValue.ToString()) + "~" +
                "Difference=" + ((dfDifference.UnFormattedValue == null) ? "0" : dfDifference.UnFormattedValue.ToString()) + "~" +
                "CashBought=" + ((dfCashBought.UnFormattedValue == null) ? "0" : dfCashBought.UnFormattedValue.ToString()) + "~" +
                "CashSold=" + ((dfCashSold.UnFormattedValue == null) ? "0" : dfCashSold.UnFormattedValue.ToString()) + "~" +
                "GlCredits=" + ((dfGlCredits.UnFormattedValue == null) ? "0" : dfGlCredits.UnFormattedValue.ToString()) + "~" +
                "GlDebits=" + ((dfGlDebits.UnFormattedValue == null) ? "0" : dfGlDebits.UnFormattedValue.ToString()) + "~" +
                "CCAmt=" + ((dfCCAmt.UnFormattedValue == null) ? "0" : dfCCAmt.UnFormattedValue.ToString()) + "~" +
                "OfficialChks=" + ((dfOfficialChks.UnFormattedValue == null) ? "0" : dfOfficialChks.UnFormattedValue.ToString()) + "~" +
                "MoneyOrder=" + ((dfMoneyOrder.UnFormattedValue == null) ? "0" : dfMoneyOrder.UnFormattedValue.ToString()) + "~" +
                "TrChksPurch=" + ((dfTrChksPurch.UnFormattedValue == null) ? "0" : dfTrChksPurch.UnFormattedValue.ToString()) + "~" +
                "DraftPurch=" + draftPurch + "~" +
                "InWire=" + ((dfInWire.UnFormattedValue == null) ? "0" : dfInWire.UnFormattedValue.ToString()) + "~" +
                "UtilPmts=" + ((dfUtilPmts.UnFormattedValue == null) ? "0" : dfUtilPmts.UnFormattedValue.ToString()) + "~" +
                "TtlPmts=" + ((dfTtlPmts.UnFormattedValue == null) ? "0" : dfTtlPmts.UnFormattedValue.ToString());

			report1.Param3.Value =
				"FCPurch=" 				+ fCPurch										+ "~" +
				"CashDep=" 				+ (( dfCashDep.UnFormattedValue == null )?"0":dfCashDep.UnFormattedValue.ToString())			+ "~" +
				"TfrCredits=" 			+ (( dfTfrCredits.UnFormattedValue == null )?"0":dfTfrCredits.UnFormattedValue.ToString())		+ "~" +
				"LnPayments=" 			+ (( dfLnPayments.UnFormattedValue == null )?"0":dfLnPayments.UnFormattedValue.ToString())		+ "~" +
				"SdPayments=" 			+ (( dfSdPayments.UnFormattedValue == null )?"0":dfSdPayments.UnFormattedValue.ToString())		+ "~" +
				"TotalDeposits=" 		+ (( dfTotalDeposits.UnFormattedValue  == null )?"0":dfTotalDeposits.UnFormattedValue.ToString())	+ "~" +
				"OnUsChksDep=" 			+ (( dfOnUsChksDep.UnFormattedValue == null )?"0":dfOnUsChksDep.UnFormattedValue.ToString())		+ "~" +
				"ChksAsCashDep=" 		+ (( dfChksAsCashDep.UnFormattedValue == null )?"0":dfChksAsCashDep.UnFormattedValue.ToString())	+ "~" +
				"TransitChksDep=" 		+ (( dfTransitChksDep.UnFormattedValue == null )?"0":dfTransitChksDep.UnFormattedValue.ToString())	+ "~" +
				"TotalChecks=" 			+ (( dfTotalChecks.UnFormattedValue == null )?"0":dfTotalChecks.UnFormattedValue.ToString())		+ "~" +
				"OthWd=" 				+ (( dfOthWd.UnFormattedValue == null )?"0":dfOthWd.UnFormattedValue.ToString())			+ "~" +
				"TfrDebits=" 			+ (( dfTfrDebits.UnFormattedValue == null )?"0":dfTfrDebits.UnFormattedValue.ToString())		+ "~" +
				"LnAdvances=" 			+ (( dfLnAdvances.UnFormattedValue == null )?"0":dfLnAdvances.UnFormattedValue.ToString())		+ "~" +
				"TotalWithdrawals="		+ (( dfTotalWithdrawals.UnFormattedValue == null )?"0":dfTotalWithdrawals.UnFormattedValue.ToString()) + "~" +
				"TrChksExch=" 			+ (( dfTrChksExch.UnFormattedValue == null )?"0":dfTrChksExch.UnFormattedValue.ToString())		+ "~" +
				"OutWire=" 				+ (( dfOutWire.UnFormattedValue == null )?"0":dfOutWire.UnFormattedValue.ToString())			+ "~" +
				"BondExch=" 			+ (( dfBondExch.UnFormattedValue == null )?"0":dfBondExch.UnFormattedValue.ToString())		+ "~" +
				"CCAdvance=" 			+ (( dfCCAdvance.UnFormattedValue == null )?"0":dfCCAdvance.UnFormattedValue.ToString())		+ "~" +
				"FCExch=" 				+ fCExch										+ "~" +
				"ChkExch=" 				+ chkExch;

			report1.Param4.Value =
				"OnUsExch=" 			+ onUsExch 											+ "~" +
				"SavedDateTime=" 		+ dfSavedDateTime.UnFormattedValue.ToString() 		+ "~" +
				"AnalFeePmt=" 			+ (( dfAnalFeePmt.UnFormattedValue == null )?"0":dfAnalFeePmt.UnFormattedValue.ToString()) 			+ "~" +
				"ChksAsCashDep=" 		+ (( dfChksAsCashDep.UnFormattedValue == null )?"0":dfChksAsCashDep.UnFormattedValue.ToString()) 		+ "~" +
				"Deposits=" 			+ (( dfDeposits.UnFormattedValue == null )?"0":dfDeposits.UnFormattedValue.ToString()) 			+ "~" +
				"ChksCashedWd=" 		+ (( dfChksCashedWd.UnFormattedValue == null )?"0":dfChksCashedWd.UnFormattedValue.ToString()) 		+ "~" +
				"OnUsChksCashed=" 		+ (( dfOnUsChksCashed.UnFormattedValue == null )?"0":dfOnUsChksCashed.UnFormattedValue.ToString()) 		+ "~" +
				"SequenceNo=" 			+ (( dfSequenceNo.UnFormattedValue == null )?"0":dfSequenceNo.UnFormattedValue.ToString()) 			+ "~" +
				"IBondExch=" 			+ (( dfIBondExch.UnFormattedValue == null )?"0":dfIBondExch.UnFormattedValue.ToString()) 			+ "~" +
				"CashOver=" 			+ cashOver 			                                + "~" +
				"CashShort=" 			+ cashShort 			                            + "~" +
				"TCDUnbatchCashOut=" 	+ (( dfTCDUnbatchCashOut.UnFormattedValue == null )?"0":dfTCDUnbatchCashOut.UnFormattedValue.ToString()) 	+ "~" +
				"TCDCashDisp=" 			+ (( dfTCDCashDisp.UnFormattedValue == null )?"0":dfTCDCashDisp.UnFormattedValue.ToString()) 		+ "~" +
				"TCDCashBought=" 		+ (( dfTCDCashBought.UnFormattedValue == null )?"0":dfTCDCashBought.UnFormattedValue.ToString()) 		+ "~" +
				//"TCDItemTotal=" 			+ nTCDItemTotal.UnFormattedValue.ToString() 		+ "~" +
				"CtrTriggered=" 		+ (( dfCtrTriggered.UnFormattedValue == null )?"0":dfCtrTriggered.UnFormattedValue.ToString()) 		+ "~" +
				"CurrencyExchange=" 	+ (( dfCurrencyExchange.UnFormattedValue == null )?"0":dfCurrencyExchange.UnFormattedValue.ToString())	+ "~" +
				"AccountCloseouts=" 	+ (( dfAccountCloseouts.UnFormattedValue == null )?"0":dfAccountCloseouts.UnFormattedValue.ToString());
            #region #76409
            /* Only pass data related to itemtype  */
            if (this._tellerVars.AdTlControl.ItemType.Value == "On Us Checks")
            {
                this._tlDrawerBalances.TranChksPrf.Value = 0;
                this._tlDrawerBalances.NoTranChksPrf.Value = 0;
                this._tlDrawerBalances.TranChksNoPrf.Value = 0;
                this._tlDrawerBalances.NoTranChksNoPrf.Value = 0;
            }

            if (this._tellerVars.AdTlControl.ItemType.Value == "Not On Us Checks")
            {
                this._tlDrawerBalances.OnUsChksPrf.Value = 0;
                this._tlDrawerBalances.NoOnUsChksPrf.Value = 0;
                this._tlDrawerBalances.OnUsChksNoPrf.Value = 0;
                this._tlDrawerBalances.NoOnUsChksNoPrf.Value = 0;
            }
            #endregion

            report1.Param5.Value =
                "ClosedDt=" + CloseOutPostingDate + "~" +
                "PosCreateDt=" + CloseOutDateTime + "~" +
                "InFrWire=" + ((dfInFrWire.UnFormattedValue == null) ? "0" : dfInFrWire.UnFormattedValue.ToString()) + "~" + 	// #60648
                "OutFrWire=" + ((dfOutFrWire.UnFormattedValue == null) ? "0" : dfOutFrWire.UnFormattedValue.ToString()) + "~" + 	// #60648
                "TCDCashLoaded=" + ((dfTCDCashLoaded.UnFormattedValue == null) ? "0" : dfTCDCashLoaded.UnFormattedValue.ToString()) + "~" +  // #72916
                "TCDCashRemoved=" + ((dfTCDCashRemoved.UnFormattedValue == null) ? "0" : dfTCDCashRemoved.UnFormattedValue.ToString()) + "~" + // #72916
                "ExternalCredits=" + ((dfExtCredits.UnFormattedValue == null) ? "0" : dfExtCredits.UnFormattedValue.ToString()) + "~" + // #76458
                "ExternalDebits=" + ((dfExtDebits.UnFormattedValue == null) ? "0" : dfExtDebits.UnFormattedValue.ToString()) + "~" + // #76458
                "TransitProofed=" + (this._tlDrawerBalances.TranChksPrf.Value.ToString()) + "~" + // #76409
                "NoTransitProofed=" + (this._tlDrawerBalances.NoTranChksPrf.Value.ToString()) + "~" + // #76409
                "OnUsProofed=" + (this._tlDrawerBalances.OnUsChksPrf.Value.ToString()) + "~" + // #76409
                "NoOnUsProofed=" + (this._tlDrawerBalances.NoOnUsChksPrf.Value.ToString()) + "~" + // #76409
                "TransitNotProofed=" + (this._tlDrawerBalances.TranChksNoPrf.Value.ToString()) + "~" + // #76409
                "NoTransitNotProofed=" + (this._tlDrawerBalances.NoTranChksNoPrf.Value.ToString()) + "~" + // #76409
                "OnUsNotProofed=" + (this._tlDrawerBalances.OnUsChksNoPrf.Value.ToString()) + "~" + // #76409
                "NoOnUsNotProofed=" + (this._tlDrawerBalances.NoOnUsChksNoPrf.Value.ToString()) + "~" + // #76409
                "TCRCashDepleted=" + (this._tlDrawerBalances.TcrCashDepleted.Value.ToString()) + "~" + // #79574
                "TCRCashDeposited=" + (this._tlDrawerBalances.TcrCashDeposited.Value.ToString()) + "~" + // #79574
                "TCDCashSold=" + (this._tlDrawerBalances.TcdCashSold.Value.ToString()); // #79574

            report1.Param6.Value =
                "NoInvItemPurch=" + ((dfNoInvItemPurch.UnFormattedValue == null) ? "0" : dfNoInvItemPurch.UnFormattedValue.ToString()) + "~" +
                "InvItemPurchAmt=" + ((dfInvItemPurchAmt.UnFormattedValue == null) ? "0" : dfInvItemPurchAmt.UnFormattedValue.ToString()) + "~" +
                "NoInvItemReturn=" + ((dfNoInvItemReturn.UnFormattedValue == null) ? "0" : dfNoInvItemReturn.UnFormattedValue.ToString()) + "~" +
                "InvItemReturnAmt=" + ((dfInvItemReturnAmt.UnFormattedValue == null) ? "0" : dfInvItemReturnAmt.UnFormattedValue.ToString()) + "~" +   //#20598
                "NoTlCaptTransitChks=" + (this._tlDrawerBalances.NoTlCaptTransitChks.Value.ToString()) + "~" +
                "TlCaptTransitChks=" + (this._tlDrawerBalances.TlCaptTransitChks.Value.ToString()) + "~" +
                "NoTlCaptOnUsChks=" + (this._tlDrawerBalances.NoTlCaptOnUsChks.Value.ToString()) + "~" +
                "TlCaptOnUsChks=" + (this._tlDrawerBalances.TlCaptOnUsChks.Value.ToString()); //#180460

			try
			{

				if (CoreService.LogPublisher.IsLogEnabled)
				{
					CoreService.LogPublisher.LogDebug("SQR Param1=(" + report1.Param1.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR Param2=(" + report1.Param2.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR Param3=(" + report1.Param3.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR Param4=(" + report1.Param4.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR Param5=(" + report1.Param5.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR Param6=(" + report1.Param6.StringValue + ")");
					CoreService.LogPublisher.LogDebug("SQR MiscNo=(" + report1.MiscParams.StringValue + ")");
				}
				DataService.Instance.ProcessRequest(XmActionType.Select, report1);
				//Phoenix.Shared.Windows.HtmlPrinter htmlPrinter = new HtmlPrinter();
				if (!silentPrinting)
					//_htmlPrinter.ShowUrlPdf(report1.OutputLink.Value);
					_pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
				else
					//_printProcess = _htmlPrinter.PrintPDFFromUrl(report1.OutputLink.Value, false); //No printer Dialog
					_printProcess = _pdfFileManipulation.PrintPDFFromUrl(report1.OutputLink.Value, false); //No printer Dialog
			}
			catch(PhoenixException pe)
			{
				dlgInformation.Instance.HideInfo();
				Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
				PMessageBox.Show(360626, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				//360626 - Failed generate this teller summary position report.
			}
			finally
			{
				if (silentPrinting && _printProcess != null)
				{
//					try
//					{
//						_printProcess.Kill();
//					}
//					finally
//					{
//						_printProcess.Dispose();
//					}
				}
				dlgInformation.Instance.HideInfo();
			}
			//text1 = report1.OutputLink.Value;
		}
		#endregion

		#region GenerateOfflineReport
		private void GenerateOfflineReport(bool displayBrowser, bool riseMessage)
		{
			//360625 - Generating teller summary position report.  Please wait...
			dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360625));
			//71440 - These window has heavy trafic and creating lot of trouble so I have moved the code back here...
			try
			{
				_htmlPrinter = new HtmlPrinter();
				//Hack - Not much we can do
				for (int i = 0; i < 1000; i++ )
				{
					if (CoreService.LogPublisher.IsLogEnabled)
					{
						if (_htmlPrinter == null)
							CoreService.LogPublisher.LogDebug("frmTlPosition_PMdiPrintEvent - Still object is getting created");
						else
							CoreService.LogPublisher.LogDebug("frmTlPosition_PMdiPrintEvent Browser object created");
					}
					if (_htmlPrinter != null)
						break;
				}
			}
			catch(System.Runtime.InteropServices.COMException ex)
			{
				CoreService.LogPublisher.LogDebug("\n(frmtlPosotion window/GenerateOfflineReport)For some reason creating of HtmlPrinter Failed." + ex.Message);
			}
			string templateFileName = Shared.Constants.UserConstants.TLF11000; //"TLF11000.html";
			string machineTmpDir = @"C:\TEMP\Phoenix\offlineReports\" ;
			string htmlString;
			string desc;
			string tellerNoName = GlobalVars.EmployeeName;
			_printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);

			Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
			myTempVar.Visible = false;
			myTempVar.TabStop = false;
			myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			decimal rawValue = 0;
			desc = dfDescription.Text.Trim();
			string closeOutDateTime;
			string closeOutPostingDate;
			try
			{
				if (!System.IO.Directory.Exists(machineTmpDir))
					System.IO.Directory.CreateDirectory(machineTmpDir);
				templateFileName = Environment.CurrentDirectory + @"\forms\" + templateFileName;
				if (!System.IO.File.Exists(templateFileName))
				{
					//File Not Found Exception
					if (riseMessage)
					{
						dlgInformation.Instance.HideInfo();
						PMessageBox.Show(360718, MessageType.Warning, MessageBoxButtons.OK, templateFileName);
						//360718 - Offline Teller Position template %1! is not found.  Please contact support to rectify the problem.
					}
					if (CoreService.LogPublisher.IsLogEnabled)
						CoreService.LogPublisher.LogDebug(CoreService.Translation.GetTokenizeMessageX(360718, templateFileName));
					return;
				}
				//string sDestinationPDF = MachineTmpDir.Value.Trim() + @"\" + templateFileName;

				using (StreamReader sr = new StreamReader(templateFileName))
				{
					htmlString = sr.ReadToEnd();
				}

				//Now Replace the contents
				#region Header
				string reportTitle;
				CallXMThruCDS("AdGbBranch");

				#region Report Title
				if (positionView.IntValue == 0)//returnGrandTotal
				{
					//21 = Teller Summary Position
					reportTitle = CoreService.Translation.GetTokenizeMessageX(360628, drawerNo.StringValue);
				}
				else if (positionView.IntValue == 1)
				{
					if (returnGrandTotal)
						reportTitle = CoreService.Translation.GetUserMessageX(360635); //360635 --  Bank Summary Report
					else
						reportTitle = CoreService.Translation.GetUserMessageX(360632); //360632 - Branch Summary Position
				}
				else if (positionView.IntValue == 4)
				{
					if (returnGrandTotal)
						reportTitle = CoreService.Translation.GetUserMessageX(360632); //360632 --  Bank Summary Report
					else
						reportTitle = CoreService.Translation.GetTokenizeMessageX(360628, drawerNo.StringValue); //360632 - Branch Summary Position

				}
				else
				{
					reportTitle = CoreService.Translation.GetTokenizeMessageX(360629, drawerNo.StringValue); //360629 - Teller History Summary Position
				}
				#endregion

				#region Set Null Values to Zero
				string draftPurch;
				if (dfDraftPurch.UnFormattedValue == null)
					draftPurch = "0";
				else
					draftPurch = dfDraftPurch.UnFormattedValue.ToString();
				//
				string fCExch;
				if (dfFCExch.UnFormattedValue == null)
					fCExch = "0";
				else
					fCExch = dfFCExch.UnFormattedValue.ToString();
				//
				string fCPurch;
				if (dfFCPurch.UnFormattedValue == null)
					fCPurch = "0";
				else
					fCPurch = dfFCPurch.UnFormattedValue.ToString();
				//
				string chkExch;
				if (dfChkExch.UnFormattedValue == null)
					chkExch = "0";
				else
					chkExch = dfChkExch.UnFormattedValue.ToString();
				//
				string onUsExch;
				if (dfOnUsExch.UnFormattedValue == null)
					onUsExch = "0";
				else
					onUsExch = dfOnUsExch.UnFormattedValue.ToString();
				//
				#endregion
				//
				htmlString = htmlString.Replace("phx_ReportTitle", reportTitle.Trim());
				//Not Needed
				//htmlString = htmlString.Replace("phx_Drawer", drawerNo.StringValue);
				htmlString = htmlString.Replace("phx_ReportDate", GlobalVars.SystemDate.ToString("MM/dd/yyyy"));

				if (positionView.Value == 2)
				{
					_reportPosPtid = _tlDrawerBalances.PositionPtid.Value;
					CallXMThruCDS("TlPosition");
					closeOutPostingDate = Convert.ToDateTime(dfSavedDateTime.UnFormattedValue).ToString("MM/dd/yyyy");
					closeOutDateTime = Convert.ToDateTime(dfSavedTime.UnFormattedValue).ToString("MM/dd/yyyy hh:mm:ss tt");
					//Because we do not know who originally was the Teller Print current employee
					if (_tlPositionTemp.EmplId.IsNull)
						tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
					else
					{
						_reportEmplId = _tlPositionTemp.EmplId.Value;
						CallXMThruCDS("AdGbRsm");
						tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
					}
				}
				else
				{
					if (!riseMessage || _printReportSilent)
					{
						desc = (RptPosDesc.IsNull?"":RptPosDesc.StringValue);
						closeOutPostingDate = Convert.ToDateTime(RptPosCloseDt.ValueObject).ToString("MM/dd/yyyy");
						closeOutDateTime = Convert.ToDateTime(RptPosCreatDt.ValueObject).ToString("MM/dd/yyyy hh:mm:ss tt");
						if (!_tlDrawerBalances.DrawerCurEmplId.IsNull && _tlDrawerBalances.DrawerCurEmplId.Value != GlobalVars.EmployeeId)
						{
							_reportEmplId = _tlDrawerBalances.DrawerCurEmplId.Value;
							CallXMThruCDS("AdGbRsm");
							tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
						}
						else
							tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
					}
					else
					{
						if (dfDescription.Enabled)
							desc = desc + " " + "*** Pending";
						closeOutPostingDate = Convert.ToDateTime(dfSavedDateTime.UnFormattedValue).ToString("MM/dd/yyyy");
						if (dfSavedTime.Visible && dfSavedTime.UnFormattedValue != null)
							closeOutDateTime = Convert.ToDateTime(dfSavedTime.UnFormattedValue).ToString("MM/dd/yyyy hh:mm:ss tt");
						else
							closeOutDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

						if (positionPtid.IsNull && _tlDrawerBalances.Status.Value == "Open")
						{
							if (!_tlDrawerBalances.DrawerCurEmplId.IsNull && _tlDrawerBalances.DrawerCurEmplId.Value != GlobalVars.EmployeeId)
							{
								_reportEmplId = _tlDrawerBalances.DrawerCurEmplId.Value;
								CallXMThruCDS("AdGbRsm");
								tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
							}
							else
								tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;
						}
						else
						{
							//Current Employee
							if (employeeId.Value > 0 && employeeId.Value != GlobalVars.EmployeeId)
							{
								_reportEmplId = employeeId.Value;
								CallXMThruCDS("AdGbRsm");
								tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
							}
							else
								tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;

							//tellerNoName = GlobalVars.EmployeeId + " - " + GlobalVars.EmployeeName;
						}
					}
				}
				htmlString = htmlString.Replace("phx_Description", desc);
				htmlString = htmlString.Replace("phx_RunDate", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
				htmlString = htmlString.Replace("phx_BranchNoName", (_adGbBranchBusObj.BranchNo.Value  + " - " + _adGbBranchBusObj.Name1.Value.Trim()));
				htmlString = htmlString.Replace("phx_CloseoutPostingDate", closeOutPostingDate);
				htmlString = htmlString.Replace("phx_TellerNumberName", tellerNoName);
				htmlString = htmlString.Replace("phx_CloseoutDate", closeOutDateTime);
				#endregion Header
				//
				#region Section1
				htmlString = htmlString.Replace("phx_CashBalancing", this.dfBeginningCash.Text);
				htmlString = htmlString.Replace("phx_CashIns", this.dfCashIns.Text);
				htmlString = htmlString.Replace("phx_OfficialChecksPurchased", this.dfOfficialChks.Text);

				htmlString = htmlString.Replace("phx_CashOuts", this.dfCashOuts.Text);
				htmlString = htmlString.Replace("phx_MoneyOrdersPurchased", this.dfMoneyOrder.Text);

                htmlString = htmlString.Replace("phx_TellerUnbatchedCashOuts", this.dfTotalUnbatchedAmount.Text);
				htmlString = htmlString.Replace("phx_TravelersChecksPurchased", this.dfTrChksPurch.Text);
                //Begin #20598
				htmlString = htmlString.Replace("phx_CashOverage", this.dfCashOver.Text);
				//htmlString = htmlString.Replace("phx_SavingsBondsPurchased", this.dfInvItemPurchAmt.Text);

				htmlString = htmlString.Replace("phx_CashShortage", this.dfCashShort.Text);
				//htmlString = htmlString.Replace("phx_IBondsPurchased", this.dfInvItemReturnAmt.Text);

				htmlString = htmlString.Replace("phx_EndingCash", this.dfTotalCash.Text);
				htmlString = htmlString.Replace("phx_UtilityPayments", this.dfUtilPmts.Text);

				htmlString = htmlString.Replace("phx_TellerCashDrawer", this.dfCashDrawer.Text);
				htmlString = htmlString.Replace("phx_TreasuryTaxLoanPayments", this.dfTtlPmts.Text);

                htmlString = htmlString.Replace("phx_NoInventoryItemsPurchased", this.dfNoInvItemPurch.Text);
                htmlString = htmlString.Replace("phx_AmtInventoryItemsPurchased", this.dfInvItemPurchAmt.Text);

                htmlString = htmlString.Replace("phx_NoInventoryItemsReturned", this.dfNoInvItemReturn.Text);
                htmlString = htmlString.Replace("phx_AmtInventoryItemsReturned", this.dfInvItemReturnAmt.Text);
                //End #20598

				htmlString = htmlString.Replace("phx_Difference", this.dfDifference.Text);

				#region Total 1
				if (dfOfficialChks.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfOfficialChks.UnFormattedValue);
				if (dfMoneyOrder.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfMoneyOrder.UnFormattedValue);
				if (dfTrChksPurch.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfTrChksPurch.UnFormattedValue);
				if (dfInvItemPurchAmt.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfInvItemPurchAmt.UnFormattedValue);
				if (dfInvItemReturnAmt.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfInvItemReturnAmt.UnFormattedValue);
				if (dfUtilPmts.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfUtilPmts.UnFormattedValue);
				if (dfTtlPmts.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfTtlPmts.UnFormattedValue);
				myTempVar.UnFormattedValue = rawValue;
				#endregion Total 1

				htmlString = htmlString.Replace("phx_Total1", myTempVar.Text);
				htmlString = htmlString.Replace("phx_CDUnbatchedCashOuts", this.dfTCDUnbatchCashOut.Text);

				#endregion Section1

				#region Section 2
				htmlString = htmlString.Replace("phx_TravelerChecksCashed", dfTrChksExch.Text);

				htmlString = htmlString.Replace("phx_CashDeposited", dfCashDep.Text);
				htmlString = htmlString.Replace("phx_SavingsBondsCashed", dfBondExch.Text);

				htmlString = htmlString.Replace("phx_ChecksDepositedAsCash", dfChksAsCashDep.Text);
				htmlString = htmlString.Replace("phx_IBondsCashed", dfIBondExch.Text);

				htmlString = htmlString.Replace("phx_OnUsChecks", dfOnUsChksDep.Text);
				htmlString = htmlString.Replace("phx_CreditCardCashAdvances", dfCCAdvance.Text);
				//
				htmlString = htmlString.Replace("phx_TransitChecksDeposited", dfTransitChksDep.Text);
				htmlString = htmlString.Replace("phx_CurrencyExchanges", dfCurrencyExchange.Text);

				htmlString = htmlString.Replace("phx_Total2", dfTotalDeposits.Text);

				#region Total 3
				rawValue = 0;
				if (dfTrChksExch.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfTrChksExch.UnFormattedValue);
				if (dfBondExch.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfBondExch.UnFormattedValue);
				if (dfIBondExch.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfIBondExch.UnFormattedValue);
				if (dfCCAdvance.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfCCAdvance.UnFormattedValue);
				if (dfCurrencyExchange.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfCurrencyExchange.UnFormattedValue);
				myTempVar.UnFormattedValue = rawValue;
				#endregion Total 3

				htmlString = htmlString.Replace("phx_Total3", myTempVar.Text);

				#endregion Section 2

				#region Section 3
				htmlString = htmlString.Replace("phx_Deposits", dfDeposits.Text);
				htmlString = htmlString.Replace("phx_CashBought", dfCashBought.Text);

				htmlString = htmlString.Replace("phx_TransferDeposits", dfTfrCredits.Text);
				htmlString = htmlString.Replace("phx_GeneralLedgerCredits", dfGlCredits.Text);

				htmlString = htmlString.Replace("phx_IncomingWireTransfers", dfInWire.Text);
				htmlString = htmlString.Replace("phx_IncomingFgnWireTransfers", dfInFrWire.Text);
				htmlString = htmlString.Replace("phx_FeesCollected", dfCCAmt.Text);

				htmlString = htmlString.Replace("phx_AnalysisFeePayments", dfAnalFeePmt.Text);
				htmlString = htmlString.Replace("phx_CashSold", dfCashSold.Text);

				htmlString = htmlString.Replace("phx_LoanPayments", dfLnPayments.Text);
				htmlString = htmlString.Replace("phx_GeneralLedgerDebits", dfGlDebits.Text);

				htmlString = htmlString.Replace("phx_SafeDepositPayments", dfSdPayments.Text);
				htmlString = htmlString.Replace("phx_CTRsTriggered", dfCtrTriggered.Text);

				htmlString = htmlString.Replace("phx_Total4", dfTotalChecks.Text);
				htmlString = htmlString.Replace("phx_TotalTransactions", dfSequenceNo.Text);

				#region Total 5
				rawValue = 0;
				if (dfCashBought.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfCashBought.UnFormattedValue);
				if (dfGlCredits.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfGlCredits.UnFormattedValue);
				if (dfCCAmt.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfCCAmt.UnFormattedValue);
				if (dfCashSold.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfCashSold.UnFormattedValue);
				if (dfGlDebits.UnFormattedValue != null)
					rawValue = rawValue + Convert.ToDecimal(dfGlDebits.UnFormattedValue);

				myTempVar.UnFormattedValue = rawValue;
				#endregion Total 5

				htmlString = htmlString.Replace("phx_Total5", myTempVar.Text);
				#endregion Section 3

				#region Section 4
				htmlString = htmlString.Replace("phx_ChecksCashed", dfChksCashedWd.Text);
				//
				htmlString = htmlString.Replace("phx_1OnUsChecksCashed", dfOnUsChksCashed.Text);
				htmlString = htmlString.Replace("phx_TCDCashDispensed", dfTCDCashDisp.Text);
				//
				htmlString = htmlString.Replace("phx_Withdrawals", dfOthWd.Text);
				htmlString = htmlString.Replace("phx_1CashBoughtFromTCD", dfTCDCashBought.Text);
				//
				htmlString = htmlString.Replace("phx_AccountCloseouts", dfAccountCloseouts.Text);
                htmlString = htmlString.Replace("phx_TCDCashLoaded", this.dfTCDCashLoaded.Text); // #72916 - DHE
				//htmlString = htmlString.Replace("phx_Total6", "$0.00"); //Not Implemented Yet - Removed #72916 - DHE
				//
				htmlString = htmlString.Replace("phx_TransferWithdrawals", dfTfrDebits.Text);
                htmlString = htmlString.Replace("phx_TCDCashRemoved", this.dfTCDCashRemoved.Text); // #72916 - DHE
				//
				htmlString = htmlString.Replace("phx_OutgoingWireTransfers", dfOutWire.Text);
                htmlString = htmlString.Replace("phx_TCRCashDepleted", this.dfTCRCashDepleted.Text); // #79574
				//
                htmlString = htmlString.Replace("phx_OutgoingFgnWireTransfers", dfOutFrWire.Text);
                htmlString = htmlString.Replace("phx_TCRCashDeposited", this.dfTCRCashDeposited.Text); // #79574
				//
				htmlString = htmlString.Replace("phx_LoanAdvances", dfLnAdvances.Text);
                htmlString = htmlString.Replace("phx_TCRCashSold", this.dfTCDCashSold.Text); // #79574
				//
				htmlString = htmlString.Replace("phx_Total7", dfTotalWithdrawals.Text);
                //
                //Begin #76458
                htmlString = htmlString.Replace("phx_ExtCredits", dfExtCredits.Text);
                htmlString = htmlString.Replace("phx_ExtDebits", dfExtDebits.Text);
                //End #76458

				#endregion Section 4

                _htmlPrinter.PrintHtml(htmlString, displayBrowser);


                //If browser causes lot of trouble we can handle this this way...
                //if (displayBrowser)
                //    _htmlPrinter.PrintHtml(htmlString, displayBrowser);
                //else
                //{
                //    string pageBreak = "<div STYLE=\"page-break-before: always\">";
                //    htmlString = htmlString.Replace(@"<div>", pageBreak);
                //    PrintDrawerCashCount(false, false, htmlString);
                //}


			}
			catch(PhoenixException pe)
			{
				dlgInformation.Instance.HideInfo();
				Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
				PMessageBox.Show(360626, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				//360626 - Failed generate this teller summary position report.
			}
			finally
			{
				dlgInformation.Instance.HideInfo();
				//Printing this report as Standby Report
                //73125
                //if (riseMessage)
                //{
                //    if (_printerSettings != null)
                //    {
                //        _printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
                //    }
                //}
			}

			//Now Print or Open the File... As You Like...
		}

		#endregion GenerateOfflineReport

		#region PrintDrawercashCount
		//private void PrintDrawerCashCount(bool displayBrowser, bool riseMessage, string strPositionReport)
        private void PrintDrawerCashCount(bool displayBrowser, bool riseMessage)
		{

			if (RptPosPtid.IsNull )
			{
				if (positionPtid.IsNull)
					return;  //There is nothing we can do for this report
				else
				{
					_reportPosPtid  = positionPtid.IntValue;
					CallXMThruCDS("TlPosition");
					RptPosPtid.ValueObject = positionPtid.Value;
					RptPosCloseDt.ValueObject = _tlPositionTemp.ClosedDt.Value;
					RptPosCreatDt.ValueObject = _tlPositionTemp.CreateDt.Value;
					RptPosDesc.ValueObject = _tlPositionTemp.Description.Value;
					TlJrnlPtid.ValueObject = _tlPositionTemp.JournalPtid.Value;
					BalDenomTracking.ValueObject = (_tlPositionTemp.BalDenomTracking.IsNull?"N":_tlPositionTemp.BalDenomTracking.Value);

				}
			}
			if (!RptPosPtid.IsNull )
			{
				try
				{
					//360724 - Generating Teller Cash Drawer Count [Drawer %1!]report.  Please wait...
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetTokenizeMessageX(360724, _tlDrawerBalances.DrawerNo.StringValue));
					//
					Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();

					Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
					myTempVar.Visible = false;
					myTempVar.TabStop = false;
					myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
					myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
					string cashDrawer = string.Empty;
					string endingCash = string.Empty;
					string difference = string.Empty;
					string tellerNoName = string.Empty;
					string branchNoName = string.Empty;
					string[] sqrParams = new string[7];

					myTempVar.UnFormattedValue = ( (dfCashDrawer.UnFormattedValue == null )?"0":dfCashDrawer.UnFormattedValue);
					cashDrawer = myTempVar.Text;
					//
					myTempVar.UnFormattedValue = ( (dfTotalCash.UnFormattedValue == null )?"0":dfTotalCash.UnFormattedValue);
					endingCash = myTempVar.Text;
					//
					myTempVar.UnFormattedValue = ( (dfDifference.UnFormattedValue == null )?"0":dfDifference.UnFormattedValue);
					difference = myTempVar.Text;


					CallXMThruCDS("AdGbBranch");
					branchNoName = (_adGbBranchBusObj.BranchNo.Value  + " - " + _adGbBranchBusObj.Name1.Value.Trim());
					CallXMThruCDS("AdGbRsm");
					//if (_tlPositionTemp.EmplId.IsNull)
					//	tellerNoName = _tlPositionTemp.EmplId.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
					//else
					tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());

					if (TellerVars.Instance.IsAppOnline) //SQR
					{
                        //WI#11962 - Changed the value of TlJrnlPtid to TlJrnlPtidCO
						sqrParams = _tlDrawerBalances.GetDrawerCashCountReportParams(
                            RptPosPtid.IntValue, Convert.ToDecimal(TlJrnlPtidCO),
							BalDenomTracking.ValueObject.ToString(),
							_tlDrawerBalances.BranchNo.Value,
							_tlDrawerBalances.DrawerNo.Value,
							Convert.ToDateTime(RptPosCloseDt.ValueObject),
							Convert.ToDateTime(RptPosCreatDt.ValueObject),
							branchNoName,
							tellerNoName,
							Convert.ToDateTime(RptPosCloseDt.ValueObject).ToString("MM/dd/yyyy"),
							Convert.ToDateTime(RptPosCreatDt.ValueObject).ToString("MM/dd/yyyy hh:mm:ss tt"),
							cashDrawer, endingCash,
							difference);

						report1.ReportName.Value = "TLO11500.sqr";
						report1.EmplId.Value = GlobalVars.EmployeeId;
						report1.FromDt.Value = GlobalVars.SystemDate;
						report1.ToDt.Value = GlobalVars.SystemDate;
						report1.RunDate.Value = DateTime.Now;
						report1.Param1.Value = sqrParams[0];
						report1.Param2.Value = sqrParams[1];
						report1.Param3.Value = sqrParams[2];
						report1.Param4.Value = sqrParams[3];
						report1.Param5.Value = sqrParams[4];
						report1.Param6.Value = sqrParams[5];
						report1.MiscParams.Value = sqrParams[6];


						if (CoreService.LogPublisher.IsLogEnabled)
						{
							CoreService.LogPublisher.LogDebug("SQR Param1=(" + report1.Param1.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param2=(" + report1.Param2.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param3=(" + report1.Param3.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param4=(" + report1.Param4.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param5=(" + report1.Param5.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param6=(" + report1.Param6.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR MiscNo=(" + report1.MiscParams.StringValue + ")");
						}
						DataService.Instance.ProcessRequest(XmActionType.Select, report1);

						if (!riseMessage)
						{
							//_printProcess = _htmlPrinter.PrintPDFFromUrl(report1.OutputLink.Value, false);
							_printProcess = _pdfFileManipulation.PrintPDFFromUrl(report1.OutputLink.Value, false);
						}
						else
							//_htmlPrinter.ShowUrlPdf(report1.OutputLink.Value);
							_pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
					}
					else
					{
						sqrParams = _tlDrawerBalances.GetOfflineTLF115Report(
							RptPosPtid.IntValue, Convert.ToDecimal(TlJrnlPtid.ValueObject),
							BalDenomTracking.ValueObject.ToString(),
							_tlDrawerBalances.BranchNo.Value,
							_tlDrawerBalances.DrawerNo.Value,
							Convert.ToDateTime(RptPosCloseDt.ValueObject),
							Convert.ToDateTime(RptPosCreatDt.ValueObject),
							branchNoName,
							tellerNoName,
							Convert.ToDateTime(RptPosCloseDt.ValueObject).ToString("MM/dd/yyyy"),
							Convert.ToDateTime(RptPosCreatDt.ValueObject).ToString("MM/dd/yyyy hh:mm:ss tt"),
							cashDrawer, endingCash,
							difference);

						if (!riseMessage)
						{
                            _htmlPrinter.PrintHtml(sqrParams[0] + "\n" + sqrParams[1], displayBrowser);
                            //We need to fix style sheet...
                            //_htmlPrinter.PrintHtml(strPositionReport + "\n" + sqrParams[0] + "\n" + sqrParams[1], displayBrowser);
						}
						else
						{
                            _htmlPrinter.PrintHtml(sqrParams[0] + "\n" + sqrParams[1], displayBrowser);
                            //We need to fix style sheet...
                            //_htmlPrinter.PrintHtml(strPositionReport + "\n" + sqrParams[0] + "\n" + sqrParams[1], displayBrowser);
						}
					}
				}
				catch(PhoenixException pe)
				{
					dlgInformation.Instance.HideInfo();
					Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
					if (riseMessage)
						PMessageBox.Show(360725, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						//360725 - Failed to generate Teller Cash Drawer Count report.
				}
				finally
				{
					if (!riseMessage && _printProcess != null)
					{
//						try
//						{
//							_printProcess.Kill();
//						}
//						finally
//						{
//							_printProcess.Dispose();
//						}
					}
					dlgInformation.Instance.HideInfo();
					//Do this only when Printing Silently
                    //73125
                    //if (!riseMessage)
                    //{
                    //    if (_printerSettings != null)
                    //    {
                    //        _printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
                    //    }
                    //}
				}
			}
		}
		#endregion

		#endregion frmTlPosition_PMdiPrintEvent

		/* Begin #71886 */
		private bool RefreshData( )
		{
			if ( (drawerIsSignOn && lastCashUpdateCounter != _tellerVars.CashUpdateCounter) || proofTotalsViewed ) //#76409 - Added proofTotalsViewed
			{
				try
				{
					dlgInformation.Instance.ShowInfo("Refreshing drawer balances...");
					GetPositionView( false );
					RecalcTotals();
					tempWin_Closed( this, null );
					lastCashUpdateCounter = _tellerVars.CashUpdateCounter;
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
                    proofTotalsViewed = false;  //#76409
				}
				return true;
			}
			return false;
		}
		/* End #71886 */

		private void dfDescription_TextChanged(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("HandleCloDesc");
		}

        //Begin #72916
        private void InitializeCashDispense()
        {
            if (cashDisp == null)
                cashDisp = CashDispenser.Instance;
        }
        private void InsertTcdRollOver(decimal endingCash, decimal actualAmt, decimal journalPtid)
        {
            _tempPosObj = new Phoenix.BusObj.Teller.TlPosition();
            _tempPosObj.BranchNo.Value = branchNo.Value;
            _tempPosObj.DrawerNo.Value = _tellerVars.TcdDrawerNo;
            _tempPosObj.ClosedDt.Value = Convert.ToDateTime(_tellerVars.PostingDt);
            _tempPosObj.CrncyId.Value = 1;
            _tempPosObj.CreateDt.Value = DateTime.Now;
            _tempPosObj.JournalPtid.Value = journalPtid;
            _tempPosObj.EndingCash.Value = endingCash;
            _tempPosObj.CashDrawer.Value = actualAmt;//WI#22234
            //NOT NULL fields
            _tempPosObj.BeginningCash.Value = 0;
            _tempPosObj.CashIns.Value = 0;
            _tempPosObj.CashOuts.Value = 0;
            _tempPosObj.ChkExch.Value = 0;
            _tempPosObj.OnUsExch.Value = 0;
            _tempPosObj.InWire.Value = 0;
            _tempPosObj.OutWire.Value = 0;
            _tempPosObj.DraftPurch.Value = 0;
            _tempPosObj.FcExch.Value = 0;
            _tempPosObj.FcPurch.Value = 0;
            _tempPosObj.Deposits.Value = 0;
            _tempPosObj.TfrCredits.Value = 0;
            _tempPosObj.TfrDebits.Value = 0;
            _tempPosObj.Payments.Value = 0;
            _tempPosObj.SafeDepPmt.Value = 0;
            _tempPosObj.LnAdvances.Value = 0;
            _tempPosObj.CashBought.Value = 0;
            _tempPosObj.CashSold.Value = 0;
            _tempPosObj.GlCredits.Value = 0;
            _tempPosObj.GlDebits.Value = 0;
            _tempPosObj.OfficialChks.Value = 0;
            _tempPosObj.MoneyOrders.Value = 0;
            _tempPosObj.TcPurch.Value = 0;
            _tempPosObj.TcExch.Value = 0;
            _tempPosObj.BondExch.Value = 0;
            _tempPosObj.BondPurch.Value = 0;
            _tempPosObj.UtilPmts.Value = 0;
            _tempPosObj.TtlPmts.Value = 0;
            _tempPosObj.CcAdvances.Value = 0;
           // _tempPosObj.CashDrawer.Value = 0; //WI#22234
            _tempPosObj.AnalFeePmt.Value = 0;
            _tempPosObj.OnUsChksCashed.Value = 0;
            _tempPosObj.IbondExch.Value = 0;
            _tempPosObj.IbondPurch.Value = 0;
            _tempPosObj.CopyBackStatus.Value = 1;   //#79574
            //
            _tempPosObj.Description.Value = CoreService.Translation.GetTokenizeTranslateX(361081, 361081, new string[] { _tellerVars.TcdMachineId.ToString() });
            _tempPosObj.EmplId.Value = _tellerVars.EmployeeId;
            if (!_tellerVars.IsAppOnline)
    	        _tempPosObj.TranStatus.Value = (short)TlJournalTranStatus.NetworkOfflineNotForwarded;
            else
                _tempPosObj.TranStatus.Value = (short)TlJournalTranStatus.RealTimePosted;
            //
            _tempPosObj.ActionType = XmActionType.New;
            //
            CallXMThruCDS("RolloverPosInsert");
        }

        private void frmTlPosition_Load(object sender, EventArgs e)
        {

        }

        private void pLabelStandard7_Click(object sender, EventArgs e)
        {

        }

        private void gbDepositsandPayments_Enter(object sender, EventArgs e)
        {

        }

        private void gbChkBal_Enter(object sender, EventArgs e)
        {

        }

        private void pDfDisplay5_TextChanged(object sender, EventArgs e)
        {

        }
        //End #72916

        //#140895
        //#28714
        //private bool EnableDisableEOB()
        //{
        //    if (positionView.Value == 0 || positionView.Value == 3)
        //    {
        //        if (positionView.Value == 0 && drawerIsSignOn && !_tlDrawer.TlCaptureWorkstation.IsNull &&
        //            _tlDrawer.TlCaptureWorkstation.Value == System.Environment.MachineName)
        //        {
        //            if (_tellerCapture != null && (_tellerCapture.IsTransactionInProgress || _tellerCapture.IsBatchInProgress))
        //                return true;
        //            else
        //                return false;
        //        }
        //        else
        //            return false;
        //    }
        //    else
        //        return false;
        //}

        private bool EnableDisableEOB() //#28714
        {
            //if ((_tellerCapture != null && (_tellerCapture.IsTransactionInProgress || _tellerCapture.IsBatchInProgress)) &&
            //    ((positionView.Value == 0 || positionView.Value == 3) && drawerIsSignOn) ||
            //        _tlJournal.IsEOBPending(System.Environment.MachineName, branchNo.Value, drawerNo.Value))
            //{
            //    return true;
            //}
            //else
            //    return false;

            if (_tellerCapture != null && (_tellerCapture.IsTransactionInProgress || _tellerCapture.IsBatchInProgress))
            {
                return true;









            }
            else
                return false;
        }

        private bool HandleEndOfTellerCaptureBatch()
        {
            //#28714
            #region old code
            //if ((positionView.Value == 0 || positionView.Value == 3) && _tellerCapture != null && _tellerCapture.IsBatchInProgress)
            //{
            //    if (this.pbEOB.Enabled)
            //        this.pbEOB.Enabled = false;

            //    _tlDrawer.BranchNo.Value = branchNo.Value;
            //    _tlDrawer.DrawerNo.Value = drawerNo.Value;
            //    _tlDrawer.SelectAllFields = true;
            //    _tlDrawer.ActionType = XmActionType.Select;
            //    CoreService.DataService.ProcessRequest(_tlDrawer);

            //    //#28714
            //    //if (!_tlDrawer.TlCaptureWorkstation.IsNull &&
            //    //    _tlDrawer.TlCaptureWorkstation.Value == System.Environment.MachineName)
            //    if ((positionView.Value == 0 && drawerIsSignOn && !_tlDrawer.TlCaptureConnected.IsNull &&
            //        _tlDrawer.TlCaptureConnected.Value == GlobalVars.Instance.ML.Y) ||
            //        (positionView.Value == 3 && !_tlDrawer.TlCaptureWorkstation.IsNull &&
            //        _tlDrawer.TlCaptureWorkstation.Value == System.Environment.MachineName))
            //    {
            //        if (_tellerCapture != null)
            //        {
            //            if (_tellerCapture.IsTransactionInProgress)
            //            {
            //                _tellerCapture.PromptTransactionInProgress();
            //                return false;
            //            }
            //            else
            //            {
            //                if (!_tellerCapture.PerformEndOfBatch(false))
            //                    return false;
            //                else
            //                    _showTransportMonitorWarning = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (_tlDrawer.TlCaptureConnected.Value == GlobalVars.Instance.ML.Y)
            //        {
            //            //if (!_tlDrawer.TlCaptureBOBPtid.IsNull &&
            //            //    (_tlDrawer.TlCaptureEOBPtid.IsNull || (_tlDrawer.TlCaptureEOBPtid.Value < _tlDrawer.TlCaptureBOBPtid.Value)))
            //            //{
            //            //    if (_tlDrawer.TlCaptureEndBatchStatus.Value == "Pending")
            //            //    {
            //            //        PMessageBox.Show(14472, MessageType.Warning, MessageBoxButtons.OK, new string[] { drawerNo.StringValue, System.Environment.MachineName });
            //            //        return false;
            //            //    }
            //            //    else if (_tlDrawer.TlCaptureEndBatchStatus.IsNull || _tlDrawer.TlCaptureEndBatchStatus.Value != "Completed")
            //            //    {
            //            //        if (DialogResult.No == PMessageBox.Show(14426, MessageType.Question, MessageBoxButtons.YesNo, new string[] { System.Environment.MachineName, drawerNo.StringValue }))
            //            //            return false;
            //            //        else
            //            //        {
            //            //            _tlDrawer.TlCaptureEndBatchStatus.Value = "Pending";
            //            //            _tlDrawer.UpdateTlCaptureEndBatchStatus();
            //            //            return false;
            //            //        }
            //            //    }
            //            //}
            //            //#14720 - Please sign on to drawer# %1! in branch# %2! from workstation %3! to end the current teller capture batch.
            //            PMessageBox.Show(14720, MessageType.Warning, MessageBoxButtons.OK, new string[] { drawerNo.StringValue, branchNo.StringValue, _tlDrawer.TlCaptureWorkstation.Value });
            //            return false;
            //        }
            //    }
            //}
            #endregion

            //if ((_tellerCapture != null && (_tellerCapture.IsTransactionInProgress || _tellerCapture.IsBatchInProgress)) &&
            //               ((positionView.Value == 0 || positionView.Value == 3) && drawerIsSignOn) ||
            //                   _tlJournal.IsEOBPending(System.Environment.MachineName, branchNo.Value, drawerNo.Value))
            //33261

            // Begin IOWA Changes
            PInt countImageCommitFlagNotY = new PInt("countImageCommitFlagNotY");

            // Begin Task#95932 - We must set values for _tlJournal
            _tlJournal.DrawerNo.Value = drawerNo.Value;
            _tlJournal.BranchNo.Value = branchNo.Value;
            _tlJournal.EffectiveDt.Value = _tellerVars.PostingDt;
            // End Task#95932

            DataService.Instance.ProcessCustomAction(_tlJournal, "CountTlCaptureImageCommitFlag", countImageCommitFlagNotY);
            if (countImageCommitFlagNotY.Value > 0)
            {
                if (DialogResult.No == PMessageBox.Show(this, 16007, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
                    return false;
            }
            // End IOWA Changes

            if (_tellerCapture != null && (_tellerCapture.IsTransactionInProgress || _tellerCapture.IsBatchInProgress))
            {
                if (_tellerCapture.IsTransactionInProgress)
                {
                    _tellerCapture.PromptTransactionInProgress();
                    return false;
                }
                else
                {
                    if (this.pbEOB.Enabled)
                        this.pbEOB.Enabled = false;
                        
                    if (!_tellerCapture.PerformEndOfBatch(false))
                        return false;
                    else
                        _showTransportMonitorWarning = true;
                }
            }
            return true;
        }

        private bool ProcessPendingBulkTran(bool lookUpOnly)
        {
            //#30926
            if (lookUpOnly && !CoreService.UIAccessProvider.HasWriteAccess(Phoenix.Shared.Constants.ScreenId.TlCaptureBulkTran))
                return false;

            _tellerCaptBulkBatchId = 0;
            _tellerCaptBulkTranXML = string.Empty;
            Phoenix.BusObj.Teller.TlCaptureBulkTran bulkTran = new TlCaptureBulkTran();

            bulkTran.GetPendingBatch(branchNo.Value,
                drawerNo.Value,
                _tellerVars.PostingDt,
                System.Environment.MachineName, out _tellerCaptBulkBatchId, out _tellerCaptBulkTranXML);           

            if (lookUpOnly)
                return !string.IsNullOrEmpty(_tellerCaptBulkTranXML);

            if (!string.IsNullOrEmpty(_tellerCaptBulkTranXML) && _tellerCaptBulkBatchId > 0)
            {
                CallOtherForms("BulkTran");
            }

            return true;
        }

	}
}
