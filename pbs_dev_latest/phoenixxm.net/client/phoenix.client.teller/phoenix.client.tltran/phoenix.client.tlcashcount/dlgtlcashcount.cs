#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name:dlgTlCashCount.cs
// NameSpace:Phoenix.Client.TellerMdi
//-------------------------------------------------------------------------------
///Date        	Ver 	Init    	Change
//-------------------------------------------------------------------------------
//Mar 28 2006	2		mselvaga	Issue#66440 - Fixed grid focus not set issue.
//11/17/2006	3		Vreddy		#67883 - Added cash count page One Offline and Online Report
//11/28/2006	4		mselvaga	#70776 - User will not be prompted to enter cash-in/cash-out denom in case the amt received is 0 or null.
//01/02/2007	5		mselvaga	#71266 - Cash drawer count over/short calc fixed.
//01/06/2007	6		mselvaga	#71044 - The system does not prompts with unsaved msg for TC928. Fixed.
//01/11/2007	7		mselvaga	#71378 - Rearranged the total fields as asked by designer.
//01/17/2007	8		mselvaga	#71441 - Fixed font property bold for total cash in and total cash out. Set Bold = false.
//01/19/2007	9		mselvaga	#71475 - Added conditional save/close feature, the save and close not allowed for drawer close out count.
//01/19/2007	10		vreddy		#71493 - Uncommented Code to change the color
//01/23/2007	11		mselvaga	#71530 - Fixed the save/close cycle for drawer closeout count.
//01/23/2007	12		Vreddy		#71440- Browser object is kiiling the form so I am moving the creation back to Printing
//01/26/2007	13		mselvaga	#71616 - Closing cash drawer count with zero amount issue.
//01/29/2007	14		mselvaga	#71627 - Changes to ML for accelerator characters Cash In/Out Count and Cash Drawer Count window aren't working as they are configured in ML
//01/29/2007	15		mselvaga	#71634 - Removed ResetValue action and quick access menu.
//01/30/2007	16		Vreddy		#71641 - Asked to Remove printing from this window, I just removed only printing event rest of the code intact and working
//02/22/2007	17		mselvaga	#71882 - Fixed the Sort Order for visible columns. No sorts allowed.
//03/05/2007	18		rpoddar		#71885 - Attach the print event for form
//03/05/2007	19		rpoddar		#71886 - Added code for setting of CashUpdateCounterAtCount
//04/10/2007	20		Vreddy		#72361 - Teller 2007 - AD_GB_RSM.employee_id is printing on the Teller Summary Position and Cash Summary Reports, it should be AD_GB_RSM.teller_no
//04/12/2007	21		Vreddy		#71893 - Add Drawer Type Valut or Teller
//05/15/2007	22		mselvaga	#72828 - Change Value Option in Cash windows is no longer there. Fixed.
//06/18/2007    23      Vreddy      #73125 - Printer layout setting is rearranged
//09/14/2007    24      mselvaga    #73720 - Fixed Make/Break action visible problem.
//01/15/2008    25      mselvaga    #74301 - Fixed grid cell focus based on quantity/amount option selected.
//01/22/2008	26		njoshi		#74557 - Changing the default answer to NO on question no.317390,317484
//02/20/2009    27      mramalin    WI - 1644 - Set the SelectFirstControl flag to false, to make sure the focus is maintained by the window
// 06Nov2009	28		GDiNatale	#6615 - SetValue Framework changes
//03/18/2010    29      mselvaga    WI#8287 - Issue with focus on the Cash Count form,  12094 - dlgTlCashCount.
//03/19/2010    30      mselvaga    Enh#79574 - Added cash recycler changes.
//05/24/2010    31      SDighe      WI#8110 - Validated check amount does not match the amount in the teller's journal
//06/24/2010    32      FOyebola    WI9517
// 06Oct2010	33		GDiNatale	#10660/#10661 - Grabbing the wrong columns for loose cash count grid ...
//12/14/2010    34      SDhamija	#11462 - error when clicking on count button in vault drawer
//11/08/2010    35      mselvaga    #79314 - Added remote override teller enh. changes.
//04/11/2013	36		jrhyne		WI#21486 - fixed issue with text being passed to TLO115, which the new SQR runtime can't handle.
//10/04/2013    37		rpoddar     #140895 - Teller Capture Integration.
//10/06/2013    38      DGarcia     WI#31033
//11/03/2016    39      EJose       Task#54515 - Bulk coin value does not update to db when revisiting cash count window.
//11/11/2016    40      EJose       Bug#55279 - Bulk coin value does not update to db when revisiting cash count window.
//11/2/2017     41      RDeepthi    WI#75604. Teller Window. So always refer Decimal Config
//08/28/2018    42      AshishBabu  Task#99223 added F10 functionality of calculator in dlgTlCashCount Window 
//09/07/2018    43      AshishBabu  Task#99209 Fixed Issue Values are Zero for Quantiy and Amount in 'TLO11500' Teller Cash Drawer Count Print
//02/21/2019    44      AshishBabu  Bug#111419 - Added code to fix the Application Error when closing Teller Cash Drawer Count window
//09/05/2019    45      rpoddar     #118850 - Teller WF Changes ( tagged by  #TellerWF )
//------------------------------------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using GlacialComponents.Controls.Common;
using GlacialComponents.Controls;
using System.Xml;
using System.Collections.Generic;
//
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.BusObj.Global;
using Phoenix.Windows.Client;
//Report
using Phoenix.BusObj.Misc;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Admin.Global;
//

namespace Phoenix.Client.TlCashCount
{
	/// <summary>
	/// Summary description for dlgTlCashCount.
	/// </summary>
	public class dlgTlCashCount : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		#region private variables
		//We will not have this
		//private Phoenix.BusObj.Teller.TlPosition _tlPositionTemp = new Phoenix.BusObj.Teller.TlPosition();
		private TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
		private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);

		private Phoenix.BusObj.Admin.Global.AdGbBranch _adGbBranchBusObj; // = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBranch] as AdGbBranch);
		Phoenix.Shared.Windows.HtmlPrinter _htmlPrinter; // = new HtmlPrinter();
		Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;
		Phoenix.Shared.Windows.PSetting _printerSetting = new PSetting();
		System.Diagnostics.Process _printProcess = null;
		private bool _balDenomTracking = false;
        private bool _isDeviceDeposit = false;  //#79574
        //private bool _snapShotRunning = false;

		#endregion

		#region params

		#endregion

		private System.ComponentModel.Container components = null;
        private Phoenix.Windows.Forms.PTabPage Name0;
        private Phoenix.Windows.Forms.PTabPage Name1;
        private Phoenix.Windows.Forms.PTabPage Name2;
		private Phoenix.Windows.Forms.PAction pbClearTab;
		private Phoenix.Windows.Forms.PAction pbClearAllTabs;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbLooseQuantity;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbLooseAmount;
		private Phoenix.Windows.Forms.PLabelStandard lblCountedBy;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblLTotalCashEntered;
		private Phoenix.Windows.Forms.PLabelStandard lblLAmountRemaining;
		private Phoenix.Windows.Forms.PLabelStandard lblLTotalCash;
		private Phoenix.Windows.Forms.PGrid gridLooseCash;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbLooseCount;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalCashEntered;
		private Phoenix.Windows.Forms.PDfDisplay dfAmountRemaining;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalCash;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard2;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbStrappedCount;
		private Phoenix.Windows.Forms.PLabelStandard lblCountedBy1;
		private Phoenix.Windows.Forms.PTabControl picTabs;
		private Phoenix.Windows.Forms.PGrid gridStrappedCash;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbOtherItemsCount;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbStrapAmount;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbStrapQuantity;
		private Phoenix.Windows.Forms.PGrid gridOtherItems;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbOtherAmount;
		private Phoenix.Windows.Forms.PRadioButtonStandard rbOtherQuantity;
		private Phoenix.Windows.Forms.PGridColumn colStrapDenomination;
		private Phoenix.Windows.Forms.PGridColumn colStrapQuantity;
		private Phoenix.Windows.Forms.PGridColumn colStrapAmount;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colCashItemValue;
		private Phoenix.Windows.Forms.PGridColumn colOtherQuantity;
		private Phoenix.Windows.Forms.PGridColumn colOtherAmount;
		private Phoenix.Windows.Forms.PGridColumn colLooseDemonination;
		private Phoenix.Windows.Forms.PGridColumn colLooseQuantity;
		private Phoenix.Windows.Forms.PGridColumn colLooseAmount;
		private Phoenix.Windows.Forms.PGridColumn colLooseDenom;
		private Phoenix.Windows.Forms.PGridColumn colStrapDenom;
		private Phoenix.Windows.Forms.PGridColumn colOtherDenom;
		private Phoenix.Windows.Forms.PAction pbResetValue;
		private Phoenix.Windows.Forms.PGridColumn colLooseDenomType;
		private Phoenix.Windows.Forms.PGridColumn colStrapDenomType;
		private Phoenix.Windows.Forms.PGridColumn colOtherDenomType;
		private Phoenix.Windows.Forms.PGridColumn colLooseCountValue;
		//
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTotals1;
		private Phoenix.Windows.Forms.PLabelStandard lblCashDrawerCount;
		private Phoenix.Windows.Forms.PDfDisplay dfCashDrawerCount;
		private Phoenix.Windows.Forms.PLabelStandard lblEndingCash;
		private Phoenix.Windows.Forms.PDfDisplay dfEndingCash;
		private Phoenix.Windows.Forms.PLabelStandard lblDifference;
		private Phoenix.Windows.Forms.PDfDisplay dfDifference;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard3;
		private Phoenix.Windows.Forms.PPanel pPanel1;
		private Phoenix.Windows.Forms.PPanel pPanel2;
		private Phoenix.Windows.Forms.PPanel pPanel3;
		private Phoenix.Windows.Forms.PGridColumn colLooseDenomId;
		private Phoenix.Windows.Forms.PGridColumn colStrapDenomId;
		private Phoenix.Windows.Forms.PGridColumn colOtherDenomId;
		private Phoenix.Windows.Forms.PGridColumn colStrapRollValue;
		private Phoenix.Windows.Forms.PGridColumn colStrapOrigRollValue;
		private Phoenix.Windows.Forms.PGridColumn colOtherOrigCashItemValue;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbStrapChangeValue;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbOtherChangeValue;
		private Phoenix.Windows.Forms.PGridColumn colMakeBreakStrapRolls;
		private Phoenix.Windows.Forms.PGridColumn colStrapRollMakBrkQty;
		private Phoenix.Windows.Forms.PAction pbMakeStrapRoll;
		private Phoenix.Windows.Forms.PAction pbClearMakeBreak;
		private Phoenix.Windows.Forms.PAction pbBreakStrapRoll;
		private Phoenix.Windows.Forms.PGridColumn colStoredStrapRollMakBrkQty;

		#region Initialize
		private TellerVars _tellerVars = TellerVars.Instance;
		private GbHelper _gbHelper = new GbHelper();
		private Phoenix.BusObj.Teller.TlCashCount _busTlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
		private Phoenix.BusObj.Teller.TlCashCount _tempTlCashCountObj;
		//
		private PDecimal cashAmountReceived = new PDecimal("CashAmountReceived");
		private PSmallInt defaultField = new PSmallInt("DefaultField");
		private PDecimal journalPtid = new PDecimal("JournalPtid");
		private bool isTrancodeEntered = true;
		//
		private ArrayList cashCount = new ArrayList();
		private ArrayList _tran = new ArrayList();
		private TlTransactionSet _tlTranSet = null;
        private ArrayList _savedTlInvItems = new ArrayList();  //#140772
        private ArrayList _tempTlInvItemsDrawer = new ArrayList();  //#140772
        private ArrayList _tempTlInvItemsBranch = new ArrayList();  //#140772
        private ArrayList _tempGridScrapBranchDrawer = new ArrayList(); //#20598
		//
		private decimal outputCashAmount = 0;
		//
		private bool isViewOnly = false;
		private bool isCashInCount = false;
		private bool isCashOutCount = false;
		private bool isCashDrawerCount = false;
		private bool isSaveAndClose = false;
		private bool isSaveActionSuccess = true;
		private bool isDenomTracking = (TellerVars.Instance.AdTlControl.DenomTracking.Value == GlobalVars.Instance.ML.Y? true : false);
		private bool isBalDenomTracking = (TellerVars.Instance.AdTlControl.BalDenomTracking.Value == GlobalVars.Instance.ML.Y? true : false);
		//
		private string mlLB = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "LB");
		private string mlLC = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "LC");
		private string mlBC = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "BC");
		private string mlSB = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "SB");
		private string mlRC = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "RC");
		private string mlOC = CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "OC");
		private PfwStandard _parentForm =  null;
		private PGrid currGrid = null;
		private string _denomTranType;
		private string _returnCodeDesc = "";
		private string _mlListCashIn = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I");
		//private string _mlListCashOut = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O");
		private string _mlListCashCloseOut = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "C");
		private string _title = "";
		private PSmallInt _branchNo = new PSmallInt();
		private PSmallInt _drawerNo = new PSmallInt();
		private PDateTime _effectiveDt = new PDateTime();
		//
		private string _looseCountBySettings = "";
		private string _strapCountBySettings = "";
		private string _otherCountBySettings = "";
        private string _inventoryItemLocBySettings = "";    //#22168
		private bool isFormCompleted = false;
		//#71893
		TlJournal _tellerJournal = null;
        private PGridColumn colLooseTcrQty;
        private PGridColumn colLooseTcrAmt;
        private PLabelStandard lblTCRDepositCounted;
        private PDfDisplay dfTCRCashDeposited;
        private PDfDisplay dfDrawerCashCounted;
        private PLabelStandard lblDrawerCashCounted;
        private PTabPage Name3;
        private PGroupBoxStandard gbInventoryItemsCount;
        private PGrid gridInventoryItems;
        private PRadioButtonStandard rbBranch;
        private PRadioButtonStandard rbDrawer;
        private PCheckBoxStandard cbInvItemChangeValue;
        private PRadioButtonStandard rbInvItemAmount;
        private PRadioButtonStandard rbInvItemQuantity;
        private PLabelStandard lblCountedItemsLocation;
        private PLabelStandard lblCountedItems;
        private PGridColumn colTypeIdDesc;
        private PGridColumn colInvItemDefaultedCount;
        private PGridColumn colInvItemCountedQty;
        private PGridColumnCheckBox colVerified;
        private PGridColumn colDifference;
		Phoenix.BusObj.Teller.TlCashCount _tellerCount = null;
        private PGridColumn colTypeId;
        private PGridColumn colTypeDescription;
        private PGridColumn colInvLocation;
        private PGridColumn colInvClass;
        private PGridColumn colInvUseDrawerLevel;
        private PGridColumn colInvLocationSort;
        private PGridColumn colInvBranchNo;
        private PGridColumn colInvDrawerNo;
        private PGridColumn colInvStatus;
        private PGridColumn colInvStatusSort;
        private PPanel pPanel4;
        Phoenix.BusObj.Teller.TlInventoryItemCount _tlInventoryItemCount = null;
        bool _invokedByWorkflow = false;    // #TellerWF

        #endregion

        public dlgTlCashCount()
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
                    if (!TellerVars.Instance.IsAppOnline && _printerSetting != null)
                        _printerSetting.ChangePageOrientation(_printerSetting.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);

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
            Phoenix.FrameWork.Core.ControlInfo controlInfo4 = new Phoenix.FrameWork.Core.ControlInfo();
            this.pbResetValue = new Phoenix.Windows.Forms.PAction();
            this.pbClearAllTabs = new Phoenix.Windows.Forms.PAction();
            this.pbClearTab = new Phoenix.Windows.Forms.PAction();
            this.picTabs = new Phoenix.Windows.Forms.PTabControl();
            this.Name0 = new Phoenix.Windows.Forms.PTabPage();
            this.gbTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfDrawerCashCounted = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDrawerCashCounted = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCRCashDeposited = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCRDepositCounted = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblLTotalCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAmountRemaining = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTotalCashEntered = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblLAmountRemaining = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblLTotalCashEntered = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbTotals1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pPanel3 = new Phoenix.Windows.Forms.PPanel();
            this.pPanel2 = new Phoenix.Windows.Forms.PPanel();
            this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
            this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pLabelStandard3 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDifference = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDifference = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfEndingCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblEndingCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashDrawerCount = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashDrawerCount = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbLooseCount = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridLooseCash = new Phoenix.Windows.Forms.PGrid();
            this.colLooseDenomId = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseDenom = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseDenomType = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseDemonination = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseCountValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseTcrQty = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseTcrAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseQuantity = new Phoenix.Windows.Forms.PGridColumn();
            this.colLooseAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.lblCountedBy = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbLooseAmount = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbLooseQuantity = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.Name1 = new Phoenix.Windows.Forms.PTabPage();
            this.gbStrappedCount = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbStrapChangeValue = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gridStrappedCash = new Phoenix.Windows.Forms.PGrid();
            this.colStrapDenomId = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapDenom = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapDenomType = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapOrigRollValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapDenomination = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapRollValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapQuantity = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colMakeBreakStrapRolls = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapRollMakBrkQty = new Phoenix.Windows.Forms.PGridColumn();
            this.colStoredStrapRollMakBrkQty = new Phoenix.Windows.Forms.PGridColumn();
            this.lblCountedBy1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbStrapAmount = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbStrapQuantity = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.Name2 = new Phoenix.Windows.Forms.PTabPage();
            this.gbOtherItemsCount = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbOtherChangeValue = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gridOtherItems = new Phoenix.Windows.Forms.PGrid();
            this.colOtherDenomId = new Phoenix.Windows.Forms.PGridColumn();
            this.colOtherDenom = new Phoenix.Windows.Forms.PGridColumn();
            this.colOtherDenomType = new Phoenix.Windows.Forms.PGridColumn();
            this.colOtherOrigCashItemValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashItemValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colOtherQuantity = new Phoenix.Windows.Forms.PGridColumn();
            this.colOtherAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.pLabelStandard2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbOtherAmount = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbOtherQuantity = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.Name3 = new Phoenix.Windows.Forms.PTabPage();
            this.gbInventoryItemsCount = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pPanel4 = new Phoenix.Windows.Forms.PPanel();
            this.lblCountedItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbInvItemQuantity = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbInvItemAmount = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.cbInvItemChangeValue = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gridInventoryItems = new Phoenix.Windows.Forms.PGrid();
            this.colTypeIdDesc = new Phoenix.Windows.Forms.PGridColumn();
            this.colTypeId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTypeDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvItemDefaultedCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvItemCountedQty = new Phoenix.Windows.Forms.PGridColumn();
            this.colVerified = new Phoenix.Windows.Forms.PGridColumnCheckBox();
            this.colDifference = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvLocation = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvClass = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvUseDrawerLevel = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvLocationSort = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvStatusSort = new Phoenix.Windows.Forms.PGridColumn();
            this.rbBranch = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbDrawer = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.lblCountedItemsLocation = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbMakeStrapRoll = new Phoenix.Windows.Forms.PAction();
            this.pbClearMakeBreak = new Phoenix.Windows.Forms.PAction();
            this.pbBreakStrapRoll = new Phoenix.Windows.Forms.PAction();
            this.picTabs.SuspendLayout();
            this.Name0.SuspendLayout();
            this.gbTotals.SuspendLayout();
            this.gbTotals1.SuspendLayout();
            this.gbLooseCount.SuspendLayout();
            this.Name1.SuspendLayout();
            this.gbStrappedCount.SuspendLayout();
            this.Name2.SuspendLayout();
            this.gbOtherItemsCount.SuspendLayout();
            this.Name3.SuspendLayout();
            this.gbInventoryItemsCount.SuspendLayout();
            this.pPanel4.SuspendLayout();
            this.SuspendLayout();
            //
            // ActionManager
            //
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbResetValue,
            this.pbClearTab,
            this.pbClearAllTabs,
            this.pbMakeStrapRoll,
            this.pbBreakStrapRoll,
            this.pbClearMakeBreak});
            //
            // pbResetValue
            //
            this.pbResetValue.LongText = "Reset Value";
            this.pbResetValue.ObjectId = 36;
            this.pbResetValue.ShortText = "Reset Value";
            this.pbResetValue.Tag = null;
            this.pbResetValue.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbResetValue_Click);
            //
            // pbClearAllTabs
            //
            this.pbClearAllTabs.LongText = "Clear All Tabs";
            this.pbClearAllTabs.ObjectId = 7;
            this.pbClearAllTabs.ShortText = "Clear All Tabs";
            this.pbClearAllTabs.Tag = null;
            this.pbClearAllTabs.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClearAllTabs_Click);
            //
            // pbClearTab
            //
            this.pbClearTab.LongText = "Clear Tab";
            this.pbClearTab.ObjectId = 6;
            this.pbClearTab.ShortText = "Clear Tab";
            this.pbClearTab.Tag = null;
            this.pbClearTab.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClearTab_Click);
            //
            // picTabs
            //
            this.picTabs.Controls.Add(this.Name0);
            this.picTabs.Controls.Add(this.Name1);
            this.picTabs.Controls.Add(this.Name2);
            this.picTabs.Controls.Add(this.Name3);
            this.picTabs.Dock = System.Windows.Forms.DockStyle.None;
            this.picTabs.Location = new System.Drawing.Point(8, 4);
            this.picTabs.Name = "picTabs";
            this.picTabs.SelectedIndex = 0;
            this.picTabs.Size = new System.Drawing.Size(690, 448);
            this.picTabs.TabIndex = 0;
            this.picTabs.SelectedIndexChanged += new System.EventHandler(this.picTabs_SelectedIndexChanged);
            //
            // Name0
            //
            this.Name0.Controls.Add(this.gbTotals);
            this.Name0.Controls.Add(this.gbTotals1);
            this.Name0.Controls.Add(this.gbLooseCount);
            this.Name0.Location = new System.Drawing.Point(4, 22);
            this.Name0.MLInfo = controlInfo1;
            this.Name0.Name = "Name0";
            this.Name0.Size = new System.Drawing.Size(682, 422);
            this.Name0.TabIndex = 0;
            this.Name0.Text = "&Loose Bills and  Loose Coin";
            //
            // gbTotals
            //
            this.gbTotals.Controls.Add(this.dfDrawerCashCounted);
            this.gbTotals.Controls.Add(this.lblDrawerCashCounted);
            this.gbTotals.Controls.Add(this.dfTCRCashDeposited);
            this.gbTotals.Controls.Add(this.lblTCRDepositCounted);
            this.gbTotals.Controls.Add(this.dfTotalCash);
            this.gbTotals.Controls.Add(this.lblLTotalCash);
            this.gbTotals.Controls.Add(this.dfAmountRemaining);
            this.gbTotals.Controls.Add(this.dfTotalCashEntered);
            this.gbTotals.Controls.Add(this.lblLAmountRemaining);
            this.gbTotals.Controls.Add(this.lblLTotalCashEntered);
            this.gbTotals.Location = new System.Drawing.Point(0, 332);
            this.gbTotals.Name = "gbTotals";
            this.gbTotals.PhoenixUIControl.ObjectId = 18;
            this.gbTotals.Size = new System.Drawing.Size(680, 88);
            this.gbTotals.TabIndex = 1;
            this.gbTotals.TabStop = false;
            this.gbTotals.Text = "Totals";
            //
            // dfDrawerCashCounted
            //
            this.dfDrawerCashCounted.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDrawerCashCounted.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfDrawerCashCounted.Location = new System.Drawing.Point(128, 64);
            this.dfDrawerCashCounted.Multiline = true;
            this.dfDrawerCashCounted.Name = "dfDrawerCashCounted";
            this.dfDrawerCashCounted.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDrawerCashCounted.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDrawerCashCounted.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDrawerCashCounted.PhoenixUIControl.ObjectId = 53;
            this.dfDrawerCashCounted.Size = new System.Drawing.Size(124, 20);
            this.dfDrawerCashCounted.TabIndex = 9;
            //
            // lblDrawerCashCounted
            //
            this.lblDrawerCashCounted.AutoEllipsis = true;
            this.lblDrawerCashCounted.Location = new System.Drawing.Point(4, 64);
            this.lblDrawerCashCounted.Name = "lblDrawerCashCounted";
            this.lblDrawerCashCounted.PhoenixUIControl.ObjectId = 53;
            this.lblDrawerCashCounted.Size = new System.Drawing.Size(120, 20);
            this.lblDrawerCashCounted.TabIndex = 8;
            this.lblDrawerCashCounted.Text = "Drawer Cash Counted:";
            //
            // dfTCRCashDeposited
            //
            this.dfTCRCashDeposited.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDeposited.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTCRCashDeposited.Location = new System.Drawing.Point(128, 40);
            this.dfTCRCashDeposited.Multiline = true;
            this.dfTCRCashDeposited.Name = "dfTCRCashDeposited";
            this.dfTCRCashDeposited.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCRCashDeposited.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTCRCashDeposited.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCRCashDeposited.PhoenixUIControl.ObjectId = 52;
            this.dfTCRCashDeposited.Size = new System.Drawing.Size(124, 20);
            this.dfTCRCashDeposited.TabIndex = 7;
            //
            // lblTCRDepositCounted
            //
            this.lblTCRDepositCounted.AutoEllipsis = true;
            this.lblTCRDepositCounted.Location = new System.Drawing.Point(4, 40);
            this.lblTCRDepositCounted.Name = "lblTCRDepositCounted";
            this.lblTCRDepositCounted.PhoenixUIControl.ObjectId = 52;
            this.lblTCRDepositCounted.Size = new System.Drawing.Size(120, 20);
            this.lblTCRDepositCounted.TabIndex = 6;
            this.lblTCRDepositCounted.Text = "TCR Cash Deposited:";
            //
            // dfTotalCash
            //
            this.dfTotalCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalCash.Location = new System.Drawing.Point(128, 16);
            this.dfTotalCash.Multiline = true;
            this.dfTotalCash.Name = "dfTotalCash";
            this.dfTotalCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalCash.PhoenixUIControl.ObjectId = 21;
            this.dfTotalCash.Size = new System.Drawing.Size(124, 20);
            this.dfTotalCash.TabIndex = 3;
            //
            // lblLTotalCash
            //
            this.lblLTotalCash.AutoEllipsis = true;
            this.lblLTotalCash.Location = new System.Drawing.Point(4, 16);
            this.lblLTotalCash.Name = "lblLTotalCash";
            this.lblLTotalCash.PhoenixUIControl.ObjectId = 21;
            this.lblLTotalCash.Size = new System.Drawing.Size(120, 20);
            this.lblLTotalCash.TabIndex = 2;
            this.lblLTotalCash.Text = "Total Cash In:";
            //
            // dfAmountRemaining
            //
            this.dfAmountRemaining.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmountRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfAmountRemaining.Location = new System.Drawing.Point(548, 40);
            this.dfAmountRemaining.Multiline = true;
            this.dfAmountRemaining.Name = "dfAmountRemaining";
            this.dfAmountRemaining.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmountRemaining.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAmountRemaining.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAmountRemaining.PhoenixUIControl.ObjectId = 20;
            this.dfAmountRemaining.Size = new System.Drawing.Size(124, 20);
            this.dfAmountRemaining.TabIndex = 5;
            //
            // dfTotalCashEntered
            //
            this.dfTotalCashEntered.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCashEntered.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfTotalCashEntered.Location = new System.Drawing.Point(548, 16);
            this.dfTotalCashEntered.Multiline = true;
            this.dfTotalCashEntered.Name = "dfTotalCashEntered";
            this.dfTotalCashEntered.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCashEntered.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalCashEntered.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalCashEntered.PhoenixUIControl.ObjectId = 19;
            this.dfTotalCashEntered.Size = new System.Drawing.Size(124, 20);
            this.dfTotalCashEntered.TabIndex = 1;
            //
            // lblLAmountRemaining
            //
            this.lblLAmountRemaining.AutoEllipsis = true;
            this.lblLAmountRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLAmountRemaining.Location = new System.Drawing.Point(436, 40);
            this.lblLAmountRemaining.Name = "lblLAmountRemaining";
            this.lblLAmountRemaining.PhoenixUIControl.ObjectId = 20;
            this.lblLAmountRemaining.Size = new System.Drawing.Size(108, 20);
            this.lblLAmountRemaining.TabIndex = 4;
            this.lblLAmountRemaining.Text = "Amount Remaining:";
            //
            // lblLTotalCashEntered
            //
            this.lblLTotalCashEntered.AutoEllipsis = true;
            this.lblLTotalCashEntered.Location = new System.Drawing.Point(436, 16);
            this.lblLTotalCashEntered.Name = "lblLTotalCashEntered";
            this.lblLTotalCashEntered.PhoenixUIControl.ObjectId = 19;
            this.lblLTotalCashEntered.Size = new System.Drawing.Size(108, 20);
            this.lblLTotalCashEntered.TabIndex = 0;
            this.lblLTotalCashEntered.Text = "Total Cash Entered:";
            //
            // gbTotals1
            //
            this.gbTotals1.Controls.Add(this.pPanel3);
            this.gbTotals1.Controls.Add(this.pPanel2);
            this.gbTotals1.Controls.Add(this.pPanel1);
            this.gbTotals1.Controls.Add(this.pLabelStandard1);
            this.gbTotals1.Controls.Add(this.pLabelStandard3);
            this.gbTotals1.Controls.Add(this.dfDifference);
            this.gbTotals1.Controls.Add(this.lblDifference);
            this.gbTotals1.Controls.Add(this.dfEndingCash);
            this.gbTotals1.Controls.Add(this.lblEndingCash);
            this.gbTotals1.Controls.Add(this.dfCashDrawerCount);
            this.gbTotals1.Controls.Add(this.lblCashDrawerCount);
            this.gbTotals1.Location = new System.Drawing.Point(0, 332);
            this.gbTotals1.Name = "gbTotals1";
            this.gbTotals1.PhoenixUIControl.ObjectId = 37;
            this.gbTotals1.Size = new System.Drawing.Size(680, 68);
            this.gbTotals1.TabIndex = 1;
            this.gbTotals1.TabStop = false;
            this.gbTotals1.Text = "Teller Balancing Cash Position";
            //
            // pPanel3
            //
            this.pPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel3.Location = new System.Drawing.Point(472, 40);
            this.pPanel3.Name = "pPanel3";
            this.pPanel3.Size = new System.Drawing.Size(204, 1);
            this.pPanel3.TabIndex = 10;
            this.pPanel3.TabStop = true;
            //
            // pPanel2
            //
            this.pPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel2.Location = new System.Drawing.Point(236, 40);
            this.pPanel2.Name = "pPanel2";
            this.pPanel2.Size = new System.Drawing.Size(204, 1);
            this.pPanel2.TabIndex = 9;
            this.pPanel2.TabStop = true;
            //
            // pPanel1
            //
            this.pPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel1.Location = new System.Drawing.Point(4, 40);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.Size = new System.Drawing.Size(204, 1);
            this.pPanel1.TabIndex = 8;
            this.pPanel1.TabStop = true;
            //
            // pLabelStandard1
            //
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(220, 16);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 41;
            this.pLabelStandard1.Size = new System.Drawing.Size(8, 20);
            this.pLabelStandard1.TabIndex = 0;
            this.pLabelStandard1.Text = "-";
            //
            // pLabelStandard3
            //
            this.pLabelStandard3.AutoEllipsis = true;
            this.pLabelStandard3.Location = new System.Drawing.Point(456, 16);
            this.pLabelStandard3.Name = "pLabelStandard3";
            this.pLabelStandard3.PhoenixUIControl.ObjectId = 42;
            this.pLabelStandard3.Size = new System.Drawing.Size(8, 20);
            this.pLabelStandard3.TabIndex = 7;
            this.pLabelStandard3.Text = "=";
            //
            // dfDifference
            //
            this.dfDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfDifference.Location = new System.Drawing.Point(476, 16);
            this.dfDifference.Multiline = true;
            this.dfDifference.Name = "dfDifference";
            this.dfDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDifference.PhoenixUIControl.ObjectId = 40;
            this.dfDifference.PhoenixUIControl.XmlTag = "TcdCashIn";
            this.dfDifference.Size = new System.Drawing.Size(200, 20);
            this.dfDifference.TabIndex = 3;
            //
            // lblDifference
            //
            this.lblDifference.AutoEllipsis = true;
            this.lblDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifference.Location = new System.Drawing.Point(548, 44);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.PhoenixUIControl.ObjectId = 40;
            this.lblDifference.Size = new System.Drawing.Size(75, 20);
            this.lblDifference.TabIndex = 2;
            this.lblDifference.Text = "Difference";
            //
            // dfEndingCash
            //
            this.dfEndingCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfEndingCash.Location = new System.Drawing.Point(240, 16);
            this.dfEndingCash.Multiline = true;
            this.dfEndingCash.Name = "dfEndingCash";
            this.dfEndingCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfEndingCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfEndingCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfEndingCash.PhoenixUIControl.ObjectId = 39;
            this.dfEndingCash.PhoenixUIControl.XmlTag = "CashOut";
            this.dfEndingCash.Size = new System.Drawing.Size(204, 20);
            this.dfEndingCash.TabIndex = 5;
            //
            // lblEndingCash
            //
            this.lblEndingCash.AutoEllipsis = true;
            this.lblEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndingCash.Location = new System.Drawing.Point(308, 44);
            this.lblEndingCash.Name = "lblEndingCash";
            this.lblEndingCash.PhoenixUIControl.ObjectId = 39;
            this.lblEndingCash.Size = new System.Drawing.Size(90, 20);
            this.lblEndingCash.TabIndex = 4;
            this.lblEndingCash.Text = "Ending Cash";
            //
            // dfCashDrawerCount
            //
            this.dfCashDrawerCount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfCashDrawerCount.Location = new System.Drawing.Point(4, 16);
            this.dfCashDrawerCount.Multiline = true;
            this.dfCashDrawerCount.Name = "dfCashDrawerCount";
            this.dfCashDrawerCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawerCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashDrawerCount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashDrawerCount.PhoenixUIControl.ObjectId = 38;
            this.dfCashDrawerCount.PhoenixUIControl.XmlTag = "CashIn";
            this.dfCashDrawerCount.Size = new System.Drawing.Size(204, 20);
            this.dfCashDrawerCount.TabIndex = 1;
            //
            // lblCashDrawerCount
            //
            this.lblCashDrawerCount.AutoEllipsis = true;
            this.lblCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashDrawerCount.Location = new System.Drawing.Point(52, 44);
            this.lblCashDrawerCount.Name = "lblCashDrawerCount";
            this.lblCashDrawerCount.PhoenixUIControl.ObjectId = 38;
            this.lblCashDrawerCount.Size = new System.Drawing.Size(115, 20);
            this.lblCashDrawerCount.TabIndex = 0;
            this.lblCashDrawerCount.Text = "Cash Drawer Count";
            //
            // gbLooseCount
            //
            this.gbLooseCount.Controls.Add(this.gridLooseCash);
            this.gbLooseCount.Controls.Add(this.lblCountedBy);
            this.gbLooseCount.Controls.Add(this.rbLooseAmount);
            this.gbLooseCount.Controls.Add(this.rbLooseQuantity);
            this.gbLooseCount.Location = new System.Drawing.Point(0, 0);
            this.gbLooseCount.Name = "gbLooseCount";
            this.gbLooseCount.PhoenixUIControl.ObjectId = 8;
            this.gbLooseCount.Size = new System.Drawing.Size(680, 332);
            this.gbLooseCount.TabIndex = 0;
            this.gbLooseCount.TabStop = false;
            this.gbLooseCount.Text = "Loose Bills and Loose Coins Count";
            //
            // gridLooseCash
            //
            this.gridLooseCash.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colLooseDenomId,
            this.colLooseDenom,
            this.colLooseDenomType,
            this.colLooseDemonination,
            this.colLooseCountValue,
            this.colLooseTcrQty,
            this.colLooseTcrAmt,
            this.colLooseQuantity,
            this.colLooseAmount});
            this.gridLooseCash.IsMaxNumRowsCustomized = false;
            this.gridLooseCash.Location = new System.Drawing.Point(4, 40);
            this.gridLooseCash.Name = "gridLooseCash";
            this.gridLooseCash.Size = new System.Drawing.Size(672, 288);
            this.gridLooseCash.TabIndex = 0;
            this.gridLooseCash.Text = "pGrid1";
            //
            // colLooseDenomId
            //
            this.colLooseDenomId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colLooseDenomId.PhoenixUIControl.XmlTag = "DenomId";
            this.colLooseDenomId.Title = "Denom Id";
            this.colLooseDenomId.Visible = false;
            //
            // colLooseDenom
            //
            this.colLooseDenom.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseDenom.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseDenom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseDenom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseDenom.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colLooseDenom.PhoenixUIControl.XmlTag = "Denom";
            this.colLooseDenom.Title = "Currency Factor";
            this.colLooseDenom.Visible = false;
            this.colLooseDenom.Width = 0;
            //
            // colLooseDenomType
            //
            this.colLooseDenomType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colLooseDenomType.PhoenixUIControl.XmlTag = "DenomType";
            this.colLooseDenomType.Title = "Denom Type";
            this.colLooseDenomType.Visible = false;
            this.colLooseDenomType.Width = 0;
            //
            // colLooseDemonination
            //
            this.colLooseDemonination.PhoenixUIControl.ObjectId = 15;
            this.colLooseDemonination.PhoenixUIControl.XmlTag = "CashDenomDesc";
            this.colLooseDemonination.Title = "Denomination";
            this.colLooseDemonination.Width = 239;
            //
            // colLooseCountValue
            //
            this.colLooseCountValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseCountValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseCountValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseCountValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseCountValue.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colLooseCountValue.PhoenixUIControl.XmlTag = "CountValue";
            this.colLooseCountValue.ShowNullAsEmpty = true;
            this.colLooseCountValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colLooseCountValue.Title = "Count Value";
            this.colLooseCountValue.Visible = false;
            this.colLooseCountValue.Width = 0;
            //
            // colLooseTcrQty
            //
            this.colLooseTcrQty.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLooseTcrQty.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLooseTcrQty.PhoenixUIControl.ObjectId = 50;
            this.colLooseTcrQty.PhoenixUIControl.XmlTag = "TcrQuantity";
            this.colLooseTcrQty.ReadOnly = false;
            this.colLooseTcrQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colLooseTcrQty.Title = "TCR Quantity";
            this.colLooseTcrQty.Width = 80;
            //
            // colLooseTcrAmt
            //
            this.colLooseTcrAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseTcrAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseTcrAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseTcrAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseTcrAmt.PhoenixUIControl.ObjectId = 51;
            this.colLooseTcrAmt.PhoenixUIControl.XmlTag = "TcrAmt";
            this.colLooseTcrAmt.ReadOnly = false;
            this.colLooseTcrAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colLooseTcrAmt.Title = "TCR Amount";
            this.colLooseTcrAmt.Width = 120;
            //
            // colLooseQuantity
            //
            this.colLooseQuantity.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLooseQuantity.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLooseQuantity.PhoenixUIControl.ObjectId = 16;
            this.colLooseQuantity.PhoenixUIControl.XmlTag = "Quantity";
            this.colLooseQuantity.ReadOnly = false;
            this.colLooseQuantity.ShowNullAsEmpty = true;
            this.colLooseQuantity.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colLooseQuantity.Title = "Quantity";
            this.colLooseQuantity.Width = 80;
            this.colLooseQuantity.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colLooseQuantity_PhoenixUILeaveEvent);
            this.colLooseQuantity.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colLooseQuantity_PhoenixUIValidateEvent);
            this.colLooseQuantity.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colLooseQuantity_PhoenixUIActivating);
            //
            // colLooseAmount
            //
            this.colLooseAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLooseAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLooseAmount.PhoenixUIControl.ObjectId = 17;
            this.colLooseAmount.PhoenixUIControl.XmlTag = "Amt";
            this.colLooseAmount.ReadOnly = false;
            this.colLooseAmount.ShowNullAsEmpty = true;
            this.colLooseAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colLooseAmount.Title = "Amount";
            this.colLooseAmount.Width = 120;
            this.colLooseAmount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colLooseAmount_PhoenixUILeaveEvent);
            this.colLooseAmount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colLooseAmount_PhoenixUIValidateEvent);
            this.colLooseAmount.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colLooseAmount_PhoenixUIActivating);
            //
            // lblCountedBy
            //
            this.lblCountedBy.AutoEllipsis = true;
            this.lblCountedBy.Location = new System.Drawing.Point(4, 16);
            this.lblCountedBy.Name = "lblCountedBy";
            this.lblCountedBy.PhoenixUIControl.ObjectId = 12;
            this.lblCountedBy.Size = new System.Drawing.Size(76, 20);
            this.lblCountedBy.TabIndex = 1;
            this.lblCountedBy.Text = "Counted By:";
            //
            // rbLooseAmount
            //
            this.rbLooseAmount.BackColor = System.Drawing.SystemColors.Control;
            this.rbLooseAmount.Description = null;
            this.rbLooseAmount.Location = new System.Drawing.Point(192, 16);
            this.rbLooseAmount.Name = "rbLooseAmount";
            this.rbLooseAmount.PhoenixUIControl.ObjectId = 14;
            this.rbLooseAmount.Size = new System.Drawing.Size(104, 20);
            this.rbLooseAmount.TabIndex = 3;
            this.rbLooseAmount.Text = "&Amount";
            this.rbLooseAmount.UseVisualStyleBackColor = false;
            this.rbLooseAmount.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbLooseAmount_PhoenixUICheckedChangedEvent);
            //
            // rbLooseQuantity
            //
            this.rbLooseQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.rbLooseQuantity.Description = null;
            this.rbLooseQuantity.IsMaster = true;
            this.rbLooseQuantity.Location = new System.Drawing.Point(84, 16);
            this.rbLooseQuantity.Name = "rbLooseQuantity";
            this.rbLooseQuantity.PhoenixUIControl.ObjectId = 13;
            this.rbLooseQuantity.Size = new System.Drawing.Size(104, 20);
            this.rbLooseQuantity.TabIndex = 2;
            this.rbLooseQuantity.Text = "&Quantity";
            this.rbLooseQuantity.UseVisualStyleBackColor = false;
            this.rbLooseQuantity.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbLooseQuantity_PhoenixUICheckedChangedEvent);
            //
            // Name1
            //
            this.Name1.Controls.Add(this.gbStrappedCount);
            this.Name1.Location = new System.Drawing.Point(4, 22);
            this.Name1.MLInfo = controlInfo2;
            this.Name1.Name = "Name1";
            this.Name1.Size = new System.Drawing.Size(682, 422);
            this.Name1.TabIndex = 1;
            this.Name1.Text = "Strapped Bills and &Rolled Coin";
            //
            // gbStrappedCount
            //
            this.gbStrappedCount.Controls.Add(this.cbStrapChangeValue);
            this.gbStrappedCount.Controls.Add(this.gridStrappedCash);
            this.gbStrappedCount.Controls.Add(this.lblCountedBy1);
            this.gbStrappedCount.Controls.Add(this.rbStrapAmount);
            this.gbStrappedCount.Controls.Add(this.rbStrapQuantity);
            this.gbStrappedCount.Location = new System.Drawing.Point(0, 0);
            this.gbStrappedCount.Name = "gbStrappedCount";
            this.gbStrappedCount.PhoenixUIControl.ObjectId = 22;
            this.gbStrappedCount.Size = new System.Drawing.Size(680, 332);
            this.gbStrappedCount.TabIndex = 1;
            this.gbStrappedCount.TabStop = false;
            this.gbStrappedCount.Text = "Strapped Bills and Rolled Coin Count";
            //
            // cbStrapChangeValue
            //
            this.cbStrapChangeValue.BackColor = System.Drawing.SystemColors.Control;
            this.cbStrapChangeValue.Checked = true;
            this.cbStrapChangeValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbStrapChangeValue.Location = new System.Drawing.Point(300, 16);
            this.cbStrapChangeValue.Name = "cbStrapChangeValue";
            this.cbStrapChangeValue.PhoenixUIControl.ObjectId = 43;
            this.cbStrapChangeValue.Size = new System.Drawing.Size(104, 20);
            this.cbStrapChangeValue.TabIndex = 5;
            this.cbStrapChangeValue.TabStop = false;
            this.cbStrapChangeValue.Text = "Change Value";
            this.cbStrapChangeValue.UseVisualStyleBackColor = false;
            this.cbStrapChangeValue.Value = null;
            this.cbStrapChangeValue.Click += new System.EventHandler(this.cbStrapChangeValue_Click);
            //
            // gridStrappedCash
            //
            this.gridStrappedCash.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colStrapDenomId,
            this.colStrapDenom,
            this.colStrapDenomType,
            this.colStrapOrigRollValue,
            this.colStrapDenomination,
            this.colStrapRollValue,
            this.colStrapQuantity,
            this.colStrapAmount,
            this.colMakeBreakStrapRolls,
            this.colStrapRollMakBrkQty,
            this.colStoredStrapRollMakBrkQty});
            this.gridStrappedCash.IsMaxNumRowsCustomized = false;
            this.gridStrappedCash.LinesInHeader = 2;
            this.gridStrappedCash.Location = new System.Drawing.Point(4, 40);
            this.gridStrappedCash.Name = "gridStrappedCash";
            this.gridStrappedCash.Size = new System.Drawing.Size(672, 288);
            this.gridStrappedCash.TabIndex = 3;
            this.gridStrappedCash.Text = "pGrid1";
            this.gridStrappedCash.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridStrappedCash_SelectedIndexChanged);
            //
            // colStrapDenomId
            //
            this.colStrapDenomId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colStrapDenomId.PhoenixUIControl.XmlTag = "DenomId";
            this.colStrapDenomId.Title = "Denom Id";
            this.colStrapDenomId.Visible = false;
            //
            // colStrapDenom
            //
            this.colStrapDenom.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapDenom.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapDenom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapDenom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapDenom.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colStrapDenom.PhoenixUIControl.XmlTag = "Denom";
            this.colStrapDenom.Title = "Currency Factor";
            this.colStrapDenom.Visible = false;
            this.colStrapDenom.Width = 0;
            //
            // colStrapDenomType
            //
            this.colStrapDenomType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colStrapDenomType.PhoenixUIControl.XmlTag = "DenomType";
            this.colStrapDenomType.Title = "Denom Type";
            this.colStrapDenomType.Visible = false;
            this.colStrapDenomType.Width = 0;
            //
            // colStrapOrigRollValue
            //
            this.colStrapOrigRollValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapOrigRollValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapOrigRollValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapOrigRollValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapOrigRollValue.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colStrapOrigRollValue.PhoenixUIControl.XmlTag = "OrigCountValue";
            this.colStrapOrigRollValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colStrapOrigRollValue.Title = "Orig Roll Value";
            this.colStrapOrigRollValue.Visible = false;
            //
            // colStrapDenomination
            //
            this.colStrapDenomination.PhoenixUIControl.ObjectId = 33;
            this.colStrapDenomination.PhoenixUIControl.XmlTag = "CashDenomDesc";
            this.colStrapDenomination.Title = "Denomination";
            this.colStrapDenomination.Width = 304;
            //
            // colStrapRollValue
            //
            this.colStrapRollValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapRollValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapRollValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapRollValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapRollValue.PhoenixUIControl.ObjectId = 25;
            this.colStrapRollValue.PhoenixUIControl.XmlTag = "CountValue";
            this.colStrapRollValue.ReadOnly = false;
            this.colStrapRollValue.ShowNullAsEmpty = true;
            this.colStrapRollValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colStrapRollValue.Title = "Strap/Roll Value";
            this.colStrapRollValue.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colStrapRollValue_PhoenixUILeaveEvent);
            this.colStrapRollValue.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapRollValue_PhoenixUIValidateEvent);
            //
            // colStrapQuantity
            //
            this.colStrapQuantity.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStrapQuantity.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStrapQuantity.PhoenixUIControl.ObjectId = 34;
            this.colStrapQuantity.PhoenixUIControl.XmlTag = "Quantity";
            this.colStrapQuantity.ReadOnly = false;
            this.colStrapQuantity.ShowNullAsEmpty = true;
            this.colStrapQuantity.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colStrapQuantity.Title = "Quantity";
            this.colStrapQuantity.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colStrapQuantity_PhoenixUILeaveEvent);
            this.colStrapQuantity.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapQuantity_PhoenixUIValidateEvent);
            this.colStrapQuantity.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapQuantity_PhoenixUIActivating);
            //
            // colStrapAmount
            //
            this.colStrapAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapAmount.PhoenixUIControl.ObjectId = 35;
            this.colStrapAmount.PhoenixUIControl.XmlTag = "Amt";
            this.colStrapAmount.ReadOnly = false;
            this.colStrapAmount.ShowNullAsEmpty = true;
            this.colStrapAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colStrapAmount.Title = "Amount";
            this.colStrapAmount.Width = 135;
            this.colStrapAmount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colStrapAmount_PhoenixUILeaveEvent);
            this.colStrapAmount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapAmount_PhoenixUIValidateEvent);
            this.colStrapAmount.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapAmount_PhoenixUIActivating);
            //
            // colMakeBreakStrapRolls
            //
            this.colMakeBreakStrapRolls.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colMakeBreakStrapRolls.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colMakeBreakStrapRolls.PhoenixUIControl.ObjectId = 45;
            this.colMakeBreakStrapRolls.PhoenixUIControl.XmlTag = "MakeBreak";
            this.colMakeBreakStrapRolls.Title = "Make/Break";
            this.colMakeBreakStrapRolls.Width = 36;
            //
            // colStrapRollMakBrkQty
            //
            this.colStrapRollMakBrkQty.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStrapRollMakBrkQty.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStrapRollMakBrkQty.PhoenixUIControl.ObjectId = 46;
            this.colStrapRollMakBrkQty.PhoenixUIControl.XmlTag = "StrapRollMakBrkQty";
            this.colStrapRollMakBrkQty.Title = "Quantity";
            this.colStrapRollMakBrkQty.Width = 46;
            this.colStrapRollMakBrkQty.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colStrapRollMakBrkQty_PhoenixUILeaveEvent);
            this.colStrapRollMakBrkQty.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colStrapRollMakBrkQty_PhoenixUIValidateEvent);
            //
            // colStoredStrapRollMakBrkQty
            //
            this.colStoredStrapRollMakBrkQty.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStoredStrapRollMakBrkQty.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStoredStrapRollMakBrkQty.Title = "Column";
            this.colStoredStrapRollMakBrkQty.Visible = false;
            this.colStoredStrapRollMakBrkQty.Width = 0;
            //
            // lblCountedBy1
            //
            this.lblCountedBy1.AutoEllipsis = true;
            this.lblCountedBy1.Location = new System.Drawing.Point(4, 16);
            this.lblCountedBy1.Name = "lblCountedBy1";
            this.lblCountedBy1.PhoenixUIControl.ObjectId = 12;
            this.lblCountedBy1.Size = new System.Drawing.Size(76, 20);
            this.lblCountedBy1.TabIndex = 2;
            this.lblCountedBy1.Text = "Counted By:";
            //
            // rbStrapAmount
            //
            this.rbStrapAmount.BackColor = System.Drawing.SystemColors.Control;
            this.rbStrapAmount.Description = null;
            this.rbStrapAmount.Location = new System.Drawing.Point(192, 16);
            this.rbStrapAmount.Name = "rbStrapAmount";
            this.rbStrapAmount.PhoenixUIControl.ObjectId = 24;
            this.rbStrapAmount.Size = new System.Drawing.Size(104, 20);
            this.rbStrapAmount.TabIndex = 1;
            this.rbStrapAmount.Text = "&Amount";
            this.rbStrapAmount.UseVisualStyleBackColor = false;
            this.rbStrapAmount.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbStrapAmount_PhoenixUICheckedChangedEvent);
            //
            // rbStrapQuantity
            //
            this.rbStrapQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.rbStrapQuantity.Description = null;
            this.rbStrapQuantity.IsMaster = true;
            this.rbStrapQuantity.Location = new System.Drawing.Point(84, 16);
            this.rbStrapQuantity.Name = "rbStrapQuantity";
            this.rbStrapQuantity.PhoenixUIControl.ObjectId = 23;
            this.rbStrapQuantity.Size = new System.Drawing.Size(104, 20);
            this.rbStrapQuantity.TabIndex = 0;
            this.rbStrapQuantity.Text = "&Quantity";
            this.rbStrapQuantity.UseVisualStyleBackColor = false;
            this.rbStrapQuantity.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbStrapQuantity_PhoenixUICheckedChangedEvent);
            //
            // Name2
            //
            this.Name2.Controls.Add(this.gbOtherItemsCount);
            this.Name2.Location = new System.Drawing.Point(4, 22);
            this.Name2.MLInfo = controlInfo3;
            this.Name2.Name = "Name2";
            this.Name2.Size = new System.Drawing.Size(682, 422);
            this.Name2.TabIndex = 2;
            this.Name2.Text = "&Other Cash Items";
            //
            // gbOtherItemsCount
            //
            this.gbOtherItemsCount.Controls.Add(this.cbOtherChangeValue);
            this.gbOtherItemsCount.Controls.Add(this.gridOtherItems);
            this.gbOtherItemsCount.Controls.Add(this.pLabelStandard2);
            this.gbOtherItemsCount.Controls.Add(this.rbOtherAmount);
            this.gbOtherItemsCount.Controls.Add(this.rbOtherQuantity);
            this.gbOtherItemsCount.Location = new System.Drawing.Point(0, 0);
            this.gbOtherItemsCount.Name = "gbOtherItemsCount";
            this.gbOtherItemsCount.PhoenixUIControl.ObjectId = 26;
            this.gbOtherItemsCount.Size = new System.Drawing.Size(680, 332);
            this.gbOtherItemsCount.TabIndex = 1;
            this.gbOtherItemsCount.TabStop = false;
            this.gbOtherItemsCount.Text = "Other Cash Items Count";
            //
            // cbOtherChangeValue
            //
            this.cbOtherChangeValue.BackColor = System.Drawing.SystemColors.Control;
            this.cbOtherChangeValue.Checked = true;
            this.cbOtherChangeValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOtherChangeValue.Location = new System.Drawing.Point(300, 16);
            this.cbOtherChangeValue.Name = "cbOtherChangeValue";
            this.cbOtherChangeValue.PhoenixUIControl.ObjectId = 44;
            this.cbOtherChangeValue.Size = new System.Drawing.Size(104, 20);
            this.cbOtherChangeValue.TabIndex = 6;
            this.cbOtherChangeValue.TabStop = false;
            this.cbOtherChangeValue.Text = "Change Value";
            this.cbOtherChangeValue.UseVisualStyleBackColor = false;
            this.cbOtherChangeValue.Value = null;
            this.cbOtherChangeValue.Click += new System.EventHandler(this.cbOtherChangeValue_Click);
            //
            // gridOtherItems
            //
            this.gridOtherItems.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colOtherDenomId,
            this.colOtherDenom,
            this.colOtherDenomType,
            this.colOtherOrigCashItemValue,
            this.colDescription,
            this.colCashItemValue,
            this.colOtherQuantity,
            this.colOtherAmount});
            this.gridOtherItems.IsMaxNumRowsCustomized = false;
            this.gridOtherItems.Location = new System.Drawing.Point(4, 40);
            this.gridOtherItems.Name = "gridOtherItems";
            this.gridOtherItems.Size = new System.Drawing.Size(672, 288);
            this.gridOtherItems.TabIndex = 3;
            this.gridOtherItems.Text = "pGrid1";
            //
            // colOtherDenomId
            //
            this.colOtherDenomId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOtherDenomId.PhoenixUIControl.XmlTag = "DenomId";
            this.colOtherDenomId.Title = "Denom Id";
            this.colOtherDenomId.Visible = false;
            //
            // colOtherDenom
            //
            this.colOtherDenom.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherDenom.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherDenom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherDenom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherDenom.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOtherDenom.PhoenixUIControl.XmlTag = "Denom";
            this.colOtherDenom.Title = "Denom";
            this.colOtherDenom.Visible = false;
            this.colOtherDenom.Width = 0;
            //
            // colOtherDenomType
            //
            this.colOtherDenomType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOtherDenomType.PhoenixUIControl.XmlTag = "Denom Type";
            this.colOtherDenomType.Title = "Denom Type";
            this.colOtherDenomType.Visible = false;
            this.colOtherDenomType.Width = 0;
            //
            // colOtherOrigCashItemValue
            //
            this.colOtherOrigCashItemValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherOrigCashItemValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherOrigCashItemValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherOrigCashItemValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherOrigCashItemValue.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOtherOrigCashItemValue.PhoenixUIControl.XmlTag = "OrigCountValue";
            this.colOtherOrigCashItemValue.Title = "Orig Cash Item Value";
            this.colOtherOrigCashItemValue.Visible = false;
            //
            // colDescription
            //
            this.colDescription.PhoenixUIControl.ObjectId = 29;
            this.colDescription.PhoenixUIControl.XmlTag = "CashDenomDesc";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 304;
            //
            // colCashItemValue
            //
            this.colCashItemValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashItemValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashItemValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashItemValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashItemValue.PhoenixUIControl.ObjectId = 30;
            this.colCashItemValue.PhoenixUIControl.XmlTag = "CountValue";
            this.colCashItemValue.ReadOnly = false;
            this.colCashItemValue.ShowNullAsEmpty = true;
            this.colCashItemValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colCashItemValue.Title = "Cash Item Value";
            this.colCashItemValue.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colCashItemValue_PhoenixUILeaveEvent);
            this.colCashItemValue.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colCashItemValue_PhoenixUIValidateEvent);
            //
            // colOtherQuantity
            //
            this.colOtherQuantity.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colOtherQuantity.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colOtherQuantity.PhoenixUIControl.ObjectId = 31;
            this.colOtherQuantity.PhoenixUIControl.XmlTag = "Quantity";
            this.colOtherQuantity.ReadOnly = false;
            this.colOtherQuantity.ShowNullAsEmpty = true;
            this.colOtherQuantity.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colOtherQuantity.Title = "Quantity";
            this.colOtherQuantity.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colOtherQuantity_PhoenixUILeaveEvent);
            this.colOtherQuantity.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colOtherQuantity_PhoenixUIValidateEvent);
            this.colOtherQuantity.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colOtherQuantity_PhoenixUIActivating);
            //
            // colOtherAmount
            //
            this.colOtherAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOtherAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOtherAmount.PhoenixUIControl.ObjectId = 32;
            this.colOtherAmount.PhoenixUIControl.XmlTag = "Amt";
            this.colOtherAmount.ReadOnly = false;
            this.colOtherAmount.ShowNullAsEmpty = true;
            this.colOtherAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colOtherAmount.Title = "Amount";
            this.colOtherAmount.Width = 135;
            this.colOtherAmount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colOtherAmount_PhoenixUILeaveEvent);
            this.colOtherAmount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colOtherAmount_PhoenixUIValidateEvent);
            this.colOtherAmount.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colOtherAmount_PhoenixUIActivating);
            //
            // pLabelStandard2
            //
            this.pLabelStandard2.AutoEllipsis = true;
            this.pLabelStandard2.Location = new System.Drawing.Point(4, 16);
            this.pLabelStandard2.Name = "pLabelStandard2";
            this.pLabelStandard2.PhoenixUIControl.ObjectId = 12;
            this.pLabelStandard2.Size = new System.Drawing.Size(76, 20);
            this.pLabelStandard2.TabIndex = 4;
            this.pLabelStandard2.Text = "Counted By:";
            //
            // rbOtherAmount
            //
            this.rbOtherAmount.BackColor = System.Drawing.SystemColors.Control;
            this.rbOtherAmount.Description = null;
            this.rbOtherAmount.Location = new System.Drawing.Point(192, 16);
            this.rbOtherAmount.Name = "rbOtherAmount";
            this.rbOtherAmount.PhoenixUIControl.ObjectId = 28;
            this.rbOtherAmount.Size = new System.Drawing.Size(104, 20);
            this.rbOtherAmount.TabIndex = 1;
            this.rbOtherAmount.Text = "&Amount";
            this.rbOtherAmount.UseVisualStyleBackColor = false;
            this.rbOtherAmount.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbOtherAmount_PhoenixUICheckedChangedEvent);
            //
            // rbOtherQuantity
            //
            this.rbOtherQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.rbOtherQuantity.Description = null;
            this.rbOtherQuantity.IsMaster = true;
            this.rbOtherQuantity.Location = new System.Drawing.Point(84, 16);
            this.rbOtherQuantity.Name = "rbOtherQuantity";
            this.rbOtherQuantity.PhoenixUIControl.ObjectId = 27;
            this.rbOtherQuantity.Size = new System.Drawing.Size(104, 20);
            this.rbOtherQuantity.TabIndex = 0;
            this.rbOtherQuantity.Text = "&Quantity";
            this.rbOtherQuantity.UseVisualStyleBackColor = false;
            this.rbOtherQuantity.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbOtherQuantity_PhoenixUICheckedChangedEvent);
            //
            // Name3
            //
            this.Name3.Controls.Add(this.gbInventoryItemsCount);
            this.Name3.Location = new System.Drawing.Point(4, 22);
            this.Name3.MLInfo = controlInfo4;
            this.Name3.Name = "Name3";
            this.Name3.Padding = new System.Windows.Forms.Padding(3);
            this.Name3.Size = new System.Drawing.Size(682, 422);
            this.Name3.TabIndex = 3;
            this.Name3.Text = "&Inventory Items";
            //
            // gbInventoryItemsCount
            //
            this.gbInventoryItemsCount.Controls.Add(this.pPanel4);
            this.gbInventoryItemsCount.Controls.Add(this.gridInventoryItems);
            this.gbInventoryItemsCount.Controls.Add(this.rbBranch);
            this.gbInventoryItemsCount.Controls.Add(this.rbDrawer);
            this.gbInventoryItemsCount.Controls.Add(this.lblCountedItemsLocation);
            this.gbInventoryItemsCount.Location = new System.Drawing.Point(0, 0);
            this.gbInventoryItemsCount.Name = "gbInventoryItemsCount";
            this.gbInventoryItemsCount.PhoenixUIControl.ObjectId = 54;
            this.gbInventoryItemsCount.Size = new System.Drawing.Size(680, 332);
            this.gbInventoryItemsCount.TabIndex = 0;
            this.gbInventoryItemsCount.TabStop = false;
            this.gbInventoryItemsCount.Text = "Inventory Items Count";
            //
            // pPanel4
            //
            this.pPanel4.Controls.Add(this.lblCountedItems);
            this.pPanel4.Controls.Add(this.rbInvItemQuantity);
            this.pPanel4.Controls.Add(this.rbInvItemAmount);
            this.pPanel4.Controls.Add(this.cbInvItemChangeValue);
            this.pPanel4.Location = new System.Drawing.Point(4, 16);
            this.pPanel4.Name = "pPanel4";
            this.pPanel4.Size = new System.Drawing.Size(605, 21);
            this.pPanel4.TabIndex = 8;
            this.pPanel4.TabStop = true;
            //
            // lblCountedItems
            //
            this.lblCountedItems.AutoEllipsis = true;
            this.lblCountedItems.Location = new System.Drawing.Point(0, 0);
            this.lblCountedItems.Name = "lblCountedItems";
            this.lblCountedItems.PhoenixUIControl.ObjectId = 12;
            this.lblCountedItems.Size = new System.Drawing.Size(104, 24);
            this.lblCountedItems.TabIndex = 0;
            this.lblCountedItems.Text = "Counted Items:";
            //
            // rbInvItemQuantity
            //
            this.rbInvItemQuantity.AutoSize = true;
            this.rbInvItemQuantity.BackColor = System.Drawing.SystemColors.Control;
            this.rbInvItemQuantity.Description = null;
            this.rbInvItemQuantity.IsMaster = true;
            this.rbInvItemQuantity.Location = new System.Drawing.Point(136, -1);
            this.rbInvItemQuantity.Name = "rbInvItemQuantity";
            this.rbInvItemQuantity.PhoenixUIControl.ObjectId = 55;
            this.rbInvItemQuantity.PhoenixUIControl.XmlTag = "CountedItems";
            this.rbInvItemQuantity.Size = new System.Drawing.Size(70, 18);
            this.rbInvItemQuantity.TabIndex = 1;
            this.rbInvItemQuantity.Text = "&Quantity";
            this.rbInvItemQuantity.UseVisualStyleBackColor = false;
            this.rbInvItemQuantity.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbInvItemQuantity_PhoenixUICheckedChangedEvent);
            //
            // rbInvItemAmount
            //
            this.rbInvItemAmount.AutoSize = true;
            this.rbInvItemAmount.BackColor = System.Drawing.SystemColors.Control;
            this.rbInvItemAmount.Description = null;
            this.rbInvItemAmount.Location = new System.Drawing.Point(231, 0);
            this.rbInvItemAmount.Name = "rbInvItemAmount";
            this.rbInvItemAmount.PhoenixUIControl.ObjectId = 55;
            this.rbInvItemAmount.PhoenixUIControl.XmlTag = "CountedItems";
            this.rbInvItemAmount.Size = new System.Drawing.Size(67, 18);
            this.rbInvItemAmount.TabIndex = 2;
            this.rbInvItemAmount.Text = "&Amount";
            this.rbInvItemAmount.UseVisualStyleBackColor = false;
            //
            // cbInvItemChangeValue
            //
            this.cbInvItemChangeValue.AutoSize = true;
            this.cbInvItemChangeValue.BackColor = System.Drawing.SystemColors.Control;
            this.cbInvItemChangeValue.Checked = true;
            this.cbInvItemChangeValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInvItemChangeValue.Location = new System.Drawing.Point(336, 0);
            this.cbInvItemChangeValue.Name = "cbInvItemChangeValue";
            this.cbInvItemChangeValue.PhoenixUIControl.ObjectId = 56;
            this.cbInvItemChangeValue.Size = new System.Drawing.Size(99, 18);
            this.cbInvItemChangeValue.TabIndex = 3;
            this.cbInvItemChangeValue.Text = "Change Value";
            this.cbInvItemChangeValue.UseVisualStyleBackColor = false;
            this.cbInvItemChangeValue.Value = null;
            this.cbInvItemChangeValue.Visible = false;
            //
            // gridInventoryItems
            //
            this.gridInventoryItems.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colTypeIdDesc,
            this.colTypeId,
            this.colTypeDescription,
            this.colInvItemDefaultedCount,
            this.colInvItemCountedQty,
            this.colVerified,
            this.colDifference,
            this.colInvLocation,
            this.colInvClass,
            this.colInvUseDrawerLevel,
            this.colInvLocationSort,
            this.colInvBranchNo,
            this.colInvDrawerNo,
            this.colInvStatus,
            this.colInvStatusSort});
            this.gridInventoryItems.IsMaxNumRowsCustomized = false;
            this.gridInventoryItems.LinesInHeader = 2;
            this.gridInventoryItems.Location = new System.Drawing.Point(4, 64);
            this.gridInventoryItems.Name = "gridInventoryItems";
            this.gridInventoryItems.Size = new System.Drawing.Size(672, 265);
            this.gridInventoryItems.TabIndex = 7;
            this.gridInventoryItems.Text = "pGrid1";
            //
            // colTypeIdDesc
            //
            this.colTypeIdDesc.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTypeIdDesc.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTypeIdDesc.PhoenixUIControl.ObjectId = 59;
            this.colTypeIdDesc.PhoenixUIControl.XmlTag = "TypeIdDesc";
            this.colTypeIdDesc.Title = "Type ID";
            this.colTypeIdDesc.Width = 380;
            //
            // colTypeId
            //
            this.colTypeId.PhoenixUIControl.XmlTag = "TypeId";
            this.colTypeId.Title = "Column";
            this.colTypeId.Visible = false;
            //
            // colTypeDescription
            //
            this.colTypeDescription.PhoenixUIControl.XmlTag = "TypeDesc";
            this.colTypeDescription.Title = "Column";
            this.colTypeDescription.Visible = false;
            //
            // colInvItemDefaultedCount
            //
            this.colInvItemDefaultedCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvItemDefaultedCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvItemDefaultedCount.PhoenixUIControl.ObjectId = 60;
            this.colInvItemDefaultedCount.PhoenixUIControl.XmlTag = "NoItems";
            this.colInvItemDefaultedCount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colInvItemDefaultedCount.Title = "Defaulted Quantity";
            this.colInvItemDefaultedCount.Width = 75;
            //
            // colInvItemCountedQty
            //
            this.colInvItemCountedQty.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvItemCountedQty.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvItemCountedQty.PhoenixUIControl.ObjectId = 61;
            this.colInvItemCountedQty.PhoenixUIControl.XmlTag = "CountNoItems";
            this.colInvItemCountedQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colInvItemCountedQty.Title = "Counted Quantity";
            this.colInvItemCountedQty.Width = 75;
            this.colInvItemCountedQty.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colInvItemCountedQty_PhoenixUIValidateEvent);
            //
            // colVerified
            //
            this.colVerified.Checked = false;
            this.colVerified.PhoenixUIControl.ObjectId = 62;
            this.colVerified.PhoenixUIControl.XmlTag = "Verified";
            this.colVerified.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.colVerified.Title = "Verified";
            this.colVerified.Width = 45;
            //
            // colDifference
            //
            this.colDifference.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDifference.PhoenixUIControl.ObjectId = 63;
            this.colDifference.PhoenixUIControl.XmlTag = "Difference";
            this.colDifference.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colDifference.Title = "Difference";
            this.colDifference.Width = 75;
            //
            // colInvLocation
            //
            this.colInvLocation.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvLocation.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvLocation.PhoenixUIControl.XmlTag = "Location";
            this.colInvLocation.Title = "Column";
            this.colInvLocation.Visible = false;
            //
            // colInvClass
            //
            this.colInvClass.PhoenixUIControl.XmlTag = "Class";
            this.colInvClass.Title = "Column";
            this.colInvClass.Visible = false;
            //
            // colInvUseDrawerLevel
            //
            this.colInvUseDrawerLevel.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvUseDrawerLevel.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvUseDrawerLevel.PhoenixUIControl.XmlTag = "UseDrawerLevel";
            this.colInvUseDrawerLevel.Title = "Column";
            this.colInvUseDrawerLevel.Visible = false;
            //
            // colInvLocationSort
            //
            this.colInvLocationSort.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvLocationSort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvLocationSort.PhoenixUIControl.XmlTag = "LocationSort";
            this.colInvLocationSort.Title = "Column";
            this.colInvLocationSort.Visible = false;
            //
            // colInvBranchNo
            //
            this.colInvBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colInvBranchNo.Title = "Column";
            this.colInvBranchNo.Visible = false;
            //
            // colInvDrawerNo
            //
            this.colInvDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colInvDrawerNo.Title = "Column";
            this.colInvDrawerNo.Visible = false;
            //
            // colInvStatus
            //
            this.colInvStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colInvStatus.PhoenixUIControl.XmlTag = "Status";
            this.colInvStatus.Title = "Column";
            this.colInvStatus.Visible = false;
            //
            // colInvStatusSort
            //
            this.colInvStatusSort.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvStatusSort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colInvStatusSort.PhoenixUIControl.XmlTag = "StatusSort";
            this.colInvStatusSort.Title = "Column";
            this.colInvStatusSort.Visible = false;
            //
            // rbBranch
            //
            this.rbBranch.AutoSize = true;
            this.rbBranch.BackColor = System.Drawing.SystemColors.Control;
            this.rbBranch.Description = null;
            this.rbBranch.Location = new System.Drawing.Point(235, 42);
            this.rbBranch.Name = "rbBranch";
            this.rbBranch.PhoenixUIControl.ObjectId = 57;
            this.rbBranch.PhoenixUIControl.XmlTag = "CountedItemsLoc";
            this.rbBranch.Size = new System.Drawing.Size(65, 18);
            this.rbBranch.TabIndex = 6;
            this.rbBranch.Text = "Branch";
            this.rbBranch.UseVisualStyleBackColor = false;
            this.rbBranch.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbBranch_PhoenixUICheckedChangedEvent);
            //
            // rbDrawer
            //
            this.rbDrawer.AutoSize = true;
            this.rbDrawer.BackColor = System.Drawing.SystemColors.Control;
            this.rbDrawer.Description = null;
            this.rbDrawer.IsMaster = true;
            this.rbDrawer.Location = new System.Drawing.Point(140, 42);
            this.rbDrawer.Name = "rbDrawer";
            this.rbDrawer.PhoenixUIControl.ObjectId = 57;
            this.rbDrawer.PhoenixUIControl.XmlTag = "CountedItemsLoc";
            this.rbDrawer.Size = new System.Drawing.Size(65, 18);
            this.rbDrawer.TabIndex = 5;
            this.rbDrawer.Text = "Drawer";
            this.rbDrawer.UseVisualStyleBackColor = false;
            this.rbDrawer.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbDrawer_PhoenixUICheckedChangedEvent);
            //
            // lblCountedItemsLocation
            //
            this.lblCountedItemsLocation.AutoEllipsis = true;
            this.lblCountedItemsLocation.Location = new System.Drawing.Point(4, 41);
            this.lblCountedItemsLocation.Name = "lblCountedItemsLocation";
            this.lblCountedItemsLocation.PhoenixUIControl.ObjectId = 65;
            this.lblCountedItemsLocation.Size = new System.Drawing.Size(133, 20);
            this.lblCountedItemsLocation.TabIndex = 4;
            this.lblCountedItemsLocation.Text = "Counted Items Location:";
            //
            // pbMakeStrapRoll
            //
            this.pbMakeStrapRoll.LongText = "Make Strap/Roll";
            this.pbMakeStrapRoll.ObjectId = 47;
            this.pbMakeStrapRoll.ShortText = "Make Strap/Roll";
            this.pbMakeStrapRoll.Tag = null;
            this.pbMakeStrapRoll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbMakeStrapRoll_Click);
            //
            // pbClearMakeBreak
            //
            this.pbClearMakeBreak.LongText = "Clear M/B";
            this.pbClearMakeBreak.ObjectId = 48;
            this.pbClearMakeBreak.ShortText = "Clear M/B";
            this.pbClearMakeBreak.Tag = null;
            this.pbClearMakeBreak.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClearMakeBreak_Click);
            //
            // pbBreakStrapRoll
            //
            this.pbBreakStrapRoll.LongText = "Break Strap/Roll";
            this.pbBreakStrapRoll.ObjectId = 49;
            this.pbBreakStrapRoll.ShortText = "Break Strap/Roll";
            this.pbBreakStrapRoll.Tag = null;
            this.pbBreakStrapRoll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBreakStrapRoll_Click);
            //
            // dlgTlCashCount
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(692, 450);
            this.Controls.Add(this.picTabs);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgTlCashCount";
            this.ScreenId = 12094;
            this.Text = "Cash In Count";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgTlCashCount_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgTlCashCount_PInitCompleteEvent);
            this.Closed += new System.EventHandler(this.dlgTlCashCount_Closed);
            this.picTabs.ResumeLayout(false);
            this.Name0.ResumeLayout(false);
            this.gbTotals.ResumeLayout(false);
            this.gbTotals.PerformLayout();
            this.gbTotals1.ResumeLayout(false);
            this.gbTotals1.PerformLayout();
            this.gbLooseCount.ResumeLayout(false);
            this.Name1.ResumeLayout(false);
            this.gbStrappedCount.ResumeLayout(false);
            this.Name2.ResumeLayout(false);
            this.gbOtherItemsCount.ResumeLayout(false);
            this.Name3.ResumeLayout(false);
            this.gbInventoryItemsCount.ResumeLayout(false);
            this.gbInventoryItemsCount.PerformLayout();
            this.pPanel4.ResumeLayout(false);
            this.pPanel4.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        #region Init Param
        //Begin #TellerWF
        public override void OnCreateParameters()
        {
            if (Parameters.Count == 0)
            {
                Parameters.Add(new PString("TranSource"));
                Parameters.Add(new PString("DenomTranType"));
                Parameters.Add(new PString("OutputJson"));
            }

        }
        //End #TellerWF
        public override void InitParameters(params object[] paramList)
		{
            //paramList[0] = new PString("TranSource", "Workflow");
            //paramList[1] = new PString("DenomTranType", "I");    // Cash In or Cash Out
            //paramList[2] = new PString("OutputJson", "");
            //Begin #TellerWF
            if (paramList != null && paramList.Length > 1 && paramList[0] is PBaseType)
            {
                base.InitParameters(paramList);
                if (Parameters.Contains("TranSource") && Parameters["TranSource"].StringValue == InputParamTranSet.TranSourceParamValue)
                {
                    _invokedByWorkflow = true;
                    _tlTranSet = new TlTransactionSet();
                    _branchNo.Value = _tellerVars.BranchNo;
                    _drawerNo.Value = _tellerVars.DrawerNo;
                    _effectiveDt.Value = GlobalVars.SystemDate;

                    _denomTranType = Parameters["DenomTranType"].StringValue;
                    _busTlCashCount.SetTranType(_denomTranType);
                    isCashInCount = _denomTranType == _mlListCashIn;
                    isCashOutCount = isCashInCount == false;
                    this.AutoFetch = false;
                    isTrancodeEntered = false;  // switch off total entered cash validations
                    return;
                }
            }
            //End #TellerWF
            if (paramList.Length >= 4)
			{
				if (paramList[0] != null)
					cashAmountReceived.Value = Convert.ToDecimal(paramList[0]);
				if (paramList[1] != null)
				{
					_denomTranType = Convert.ToString(paramList[1]);
					//#71893
					if (_denomTranType != "MB")
					{
						_busTlCashCount.SetTranType(_denomTranType);
						if (_denomTranType != _mlListCashCloseOut)
						{
							if(_denomTranType == _mlListCashIn)
								isCashInCount = true;
							else
								isCashOutCount = true;
						}
						else
							isCashDrawerCount = true;
					}
				}
				isViewOnly = false;
				if (paramList[2] != null)
					isTrancodeEntered = Convert.ToBoolean(paramList[2]);
				if (paramList[3] != null)
				{
					cashCount = (ArrayList)paramList[3];
				}
				// additional params
				if (paramList.Length > 4)
				{
					if (paramList[4] != null)
						_branchNo.Value = Convert.ToInt16(paramList[4]);
					else
						_branchNo.Value = _tellerVars.BranchNo;
					if (paramList[5] != null)
						_drawerNo.Value = Convert.ToInt16(paramList[5]);
					else
						_drawerNo.Value = _tellerVars.DrawerNo;
					if (paramList[6] != null)
						_effectiveDt.Value = Convert.ToDateTime(paramList[6]);
					else
						_effectiveDt.Value = GlobalVars.SystemDate;
				}
				if (paramList.Length > 7)
				{
					if (paramList[7] != null)
					{
						_tlTranSet = (TlTransactionSet)paramList[7];
						_tran = _tlTranSet.Transactions;
					}
                    if (paramList.Length > 8)
                    {
                        if (paramList[8] != null)       //#140772
                        {
                            _savedTlInvItems = (ArrayList)paramList[8];
                        }
                    }
				}
			}
			else
				isViewOnly = true;

            #region #79574
            if (!isViewOnly && isCashInCount && _tlTranSet.TellerVars.IsRecycler)
                _isDeviceDeposit = true;
            else
                _isDeviceDeposit = false;
            #endregion

            #region say no to default framework select
            this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.CashCount;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			#region make editable columns
			this.colLooseAmount.ReadOnly = isViewOnly;
			this.colLooseQuantity.ReadOnly = isViewOnly;
			this.colStrapRollValue.ReadOnly = isViewOnly;
			this.colStrapAmount.ReadOnly = isViewOnly;
			this.colStrapQuantity.ReadOnly = isViewOnly;
			this.colCashItemValue.ReadOnly = isViewOnly;
			this.colOtherAmount.ReadOnly = isViewOnly;
			this.colOtherQuantity.ReadOnly = isViewOnly;
            this.colLooseTcrQty.ReadOnly = true;    //#79574
            this.colLooseTcrAmt.ReadOnly = true;    //#79574
            this.colInvItemCountedQty.ReadOnly = isViewOnly;    //#140772
            this.colInvItemDefaultedCount.ReadOnly = true;  //#140772
			//
			#endregion

			#region skip populate
			if (!isViewOnly)
			{
				gridLooseCash.SkipPopulate = true;
				gridStrappedCash.SkipPopulate = true;
				gridOtherItems.SkipPopulate = true;
                gridInventoryItems.SkipPopulate = true; //#140772
			}
			#endregion

			base.InitParameters (paramList);
		}

		#endregion

		#region standard actions

		public override bool OnActionSave(bool isAddNext)
		{
//			int nRow = 0;
			int contextRow = gridLooseCash.ContextRow;
			bool isValidationFailed = false;
			bool isSuccess = true;
			isSaveActionSuccess = true;
			//#71893
			if (_denomTranType == "MB")
				return true;
			//
			_tempTlCashCountObj = new Phoenix.BusObj.Teller.TlCashCount();

			// in case of no changes do not attempt to save changes
			if (isCashDrawerCount)
			{
                //Begin #140772
                //Moved this code here to save inventory items before cash drawer count
                if (rbDrawer.Checked)
                    LoadInvItems(true, _tempTlInvItemsDrawer, false);
                else if (rbBranch.Checked)
                    LoadInvItems(true, _tempTlInvItemsBranch, false);
                if (_savedTlInvItems.Count > 0)
                    _savedTlInvItems.Clear();
                _savedTlInvItems.AddRange(_tempTlInvItemsDrawer);
                _savedTlInvItems.AddRange(_tempTlInvItemsBranch);
                //End #140772

				if (!this.IsFormDirty())
				{
					if (Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue) == 0 || dfCashDrawerCount.UnFormattedValue == null)
					{
						_tellerVars.DrawerCounted = false;
						_tellerVars.CountedAmount = Decimal.MinValue;
						isSaveActionSuccess = true;
						return true;
					}
				}
			}

			for( int rowId = 0; rowId < gridLooseCash.Count; rowId++ )
			{
				LooseContextRowScreenToObject(rowId, null);
				//
				if (!_tempTlCashCountObj.Quantity.IsNull || !_tempTlCashCountObj.Amt.IsNull)
				{
					if (!ValidateGrid(null, false))
					{
						if (!_tempTlCashCountObj.ValidationReturnCode.IsNull && _tempTlCashCountObj.ValidationReturnCode.Value != 0)
							PMessageBox.Show(this, _tempTlCashCountObj.ValidationReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						isValidationFailed = true;
						break;
					}
				}
			}
			//
			if (!isValidationFailed)
			{
				for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
				{
					StrapContextRowScreenToObject(rowId, null);
					//
					if (!_tempTlCashCountObj.Quantity.IsNull || !_tempTlCashCountObj.Amt.IsNull)
					{
						if (!ValidateGrid(null, false))
						{
							if (!_tempTlCashCountObj.ValidationReturnCode.IsNull && _tempTlCashCountObj.ValidationReturnCode.Value != 0)
								PMessageBox.Show(this, _tempTlCashCountObj.ValidationReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
							isValidationFailed = true;
							break;
						}
					}
				}
			}
			if (!isValidationFailed)
			{
				for( int rowId = 0; rowId < gridOtherItems.Count; rowId++ )
				{
					OtherCashContextRowScreenToObject(rowId, null);
					//
					if (!_tempTlCashCountObj.Quantity.IsNull || !_tempTlCashCountObj.Amt.IsNull)
					{
						if (!ValidateGrid(null, false))
						{
							if (!_tempTlCashCountObj.ValidationReturnCode.IsNull && _tempTlCashCountObj.ValidationReturnCode.Value != 0)
								PMessageBox.Show(this, _tempTlCashCountObj.ValidationReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
							isValidationFailed = true;
							break;
						}
					}
				}
			}
			if (!isValidationFailed)
			{
				isSuccess =true;
			}
			else
				isSuccess = false;

			#region validation success
			if (isSuccess)
			{
				decimal cashReceived = (cashAmountReceived.IsNull? 0 : cashAmountReceived.Value);
				decimal cashEntered = (dfTotalCashEntered.UnFormattedValue == null? 0 : Convert.ToDecimal(dfTotalCashEntered.UnFormattedValue));
				if (!isCashDrawerCount)
				{
					if (isTrancodeEntered)
					{
						if (cashEntered != cashReceived)
                        {   //Begin #140895
                            if (_tlTranSet != null && _tlTranSet.IsTellerCaptureTran)
                            {
                                //14453 - The Total Cash Entered must be equal to the Total Cash %1!.
                                PMessageBox.Show(this, 14453, MessageType.Warning, MessageBoxButtons.OK, new string[] { isCashInCount ? "In" : "Out" } );
                                isSaveActionSuccess = false;
                            }
                            //End #140895

                            /*issue 74557*/
							//if (DialogResult.No == PMessageBox.Show(this, (isCashInCount? 317390 : 317484), MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
							else if (DialogResult.No ==PMessageBox.Show(this, (isCashInCount ? 317390 : 317484), MessageType.Question, MessageBoxButtons.YesNo,MessageBoxDefaultButton.Button2,string.Empty))
							{
								isSaveActionSuccess = false;
								//return isSaveActionSuccess;
							}
							else
							{
								outputCashAmount = Convert.ToDecimal(this.dfTotalCashEntered.UnFormattedValue);
								isSaveActionSuccess = true;
							}
						}
						//
						#region TC928
						if (_tran != null && cashEntered > 0 && isSaveActionSuccess)
						{
							foreach(TlTransaction _curTran in _tran)
							{
								if (_curTran.TranCode.Value == 928 && !_curTran.ReferenceAcctType.IsNull &&
									!_curTran.ReferenceAcctNo.IsNull) //currency/coin order
								{
									_tempTlCashCountObj.ChgBasisAcctType.Value = _curTran.ReferenceAcctType.Value;
									_tempTlCashCountObj.ChgBasisAcctNo.Value = _curTran.ReferenceAcctNo.Value;
									_tempTlCashCountObj.ChgBasisChargeCode.Value = 859;
									//
									if (!_tempTlCashCountObj.ValidateCoinOrderTc(cashCount))
									{
										break;
									}
								}
							}
							//
							if (!_tempTlCashCountObj.ValidationReturnCode.IsNull && _tempTlCashCountObj.ValidationReturnCode.Value != 0)
							{
								//Selva - #71405 Made error message into warning
								string[] msgInput = _tempTlCashCountObj.ValidationReturnMsgInput.Value.Split(',');
								if (msgInput.Length == 2)
								{
									if (DialogResult.No == PMessageBox.Show(this, _tempTlCashCountObj.ValidationReturnCode.Value, MessageType.Question, MessageBoxButtons.YesNo, new string[] { msgInput[0].ToString(), msgInput[1].ToString()}))
									{
										isSaveActionSuccess = false;
										return isSaveActionSuccess;
									}
								}
							}
						}
						#endregion

					}
					//
					if (isSaveActionSuccess)
					{
						if (gridLooseCash.Count > 0 || gridStrappedCash.Count > 0 ||
							gridOtherItems.Count > 0)
						{
							LoadCashItems(true, true);
							CalcTotal();
						}
					}
				}
				else //isCashDrawerCount
				{
					_tellerVars.DrawerCounted = true;
					_tellerVars.CountedAmount = Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue);
					//
					if (gridLooseCash.Count > 0 || gridStrappedCash.Count > 0 ||
						gridOtherItems.Count > 0)
					{
						LoadCashItems(true, true);
						CalcTotal();
                        //Begin #140772
                        //if (rbDrawer.Checked)
                        //    LoadInvItems(true, _tempTlInvItemsDrawer, false);
                        //else if (rbBranch.Checked)
                        //    LoadInvItems(true, _tempTlInvItemsBranch, false);
                        //if (_savedTlInvItems.Count > 0)
                        //    _savedTlInvItems.Clear();
                        //_savedTlInvItems.AddRange(_tempTlInvItemsDrawer);
                        //_savedTlInvItems.AddRange(_tempTlInvItemsBranch);
                        //End #140772
					}
				}
				//
				if (!isCashInCount && _balDenomTracking)
				{
					object objectValue = null;
					int strapRollQty = 0;
					for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
					{
						objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollMakBrkQty.ColumnId );
						if (objectValue != null)
						{
							strapRollQty = Convert.ToInt32(objectValue);
							gridStrappedCash.SetCellValueUnFormatted( rowId, colStoredStrapRollMakBrkQty.ColumnId, strapRollQty);
						}
					}
				}
				//
				if ((!isCashDrawerCount && !isSaveActionSuccess)||
					isCashDrawerCount)
				{
					if (this.LastEnteredControl != null)
						this.LastEnteredControl.Focus();
					//
					if (picTabs.SelectedIndex == 0)
						gridLooseCash.Focus();
					else if (picTabs.SelectedIndex == 1)
						gridStrappedCash.Focus();
					else if (picTabs.SelectedIndex == 2)
						gridOtherItems.Focus();
                    else if (picTabs.SelectedIndex == 3)    //#140772
                        gridInventoryItems.Focus();
				}
			}
			/* Begin #71886 */
			if (isSaveActionSuccess && isCashDrawerCount &&
				cashCount == TellerVars.Instance.CashCount )
				TellerVars.Instance.CashUpdateCounterAtCount = TellerVars.Instance.CashUpdateCounter;
            /* End #71886 */
            //Begin #TellerWF
            if (_invokedByWorkflow && isSaveActionSuccess)
            {
                List<InputParamTranDenom> outputItems = new List<InputParamTranDenom>();
                foreach (Phoenix.BusObj.Teller.TlCashCount capDenom in cashCount)
                {
                    if (capDenom.Amt.Value > 0 && capDenom.Quantity.Value > 0 )
                    {
                        //outputItems.Add(new InputParamTranDenom
                        //{
                        //    Denom = capDenom.Denom.Value,
                        //    DenomType = capDenom.DenomType.Value,
                        //    Quantity = capDenom.Quantity.Value,
                        //    Amount = capDenom.Amt.Value
                        //});

                        InputParamTranDenom paramTranDenom = new InputParamTranDenom();
                        InputParamTranSet.MoveValueFromBusObjectToObject(capDenom, paramTranDenom);
                        outputItems.Add(paramTranDenom);
                    }
                }
                if (Parameters.Contains("OutputJson"))
                {
                    Parameters["OutputJson"].SetValue(Newtonsoft.Json.JsonConvert.SerializeObject(outputItems));
                }
            }
            //End #TellerWF
            return isSaveActionSuccess;
			#endregion

			#region do not call the base
			//return base.OnActionSave (isAddNext);
			#endregion
		}

		public override bool OnActionClose()
		{
			bool isDirtyForm = false;
            //WI#8110
            if (this.Workspace != null) //#31033
            {
                foreach (Control control in (Workspace as Form).Controls)
                {
                    if (control is PCookieCrumb)
                    {
                        control.Enabled = true;
                        break;
                    }

                }
            }
            //end WI#8110
			//
			if (_denomTranType != "MB")
			{
				#region Save To Registry
				HandleRegSettings(true);
				#endregion
				//
				#region drawer count
				if (isCashDrawerCount)
				{
					if (dfCashDrawerCount.UnFormattedValue != null)
					{
						if ((Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue) >= 0 && (cashAmountReceived.IsNull || cashAmountReceived.Value == 0)) ||
							(Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue) > 0 && cashAmountReceived.Value > 0))
						{
							_tellerVars.DrawerCounted = true;
							_tellerVars.CountedAmount = Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue);
						}
					}
				}
				#endregion
				//
				if (isViewOnly || (isSaveAndClose && !isCashDrawerCount)) //#71530
				{
					InsertMakeBreakTrans(); //#71893
					return true;
				}
				else
				{
					isDirtyForm = this.IsFormDirty();
					//
					#region drawer count validation
					if (isCashDrawerCount)
					{
						if (isDirtyForm)
						{
							//if (DialogResult.Yes == PMessageBox.Show(this, dlgTlCashCount.EditTestFailedId, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
							if (DialogResult.Yes == PMessageBox.Show(this, 360822, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
							{
								if (!OnActionSave(false))
									return false;
								InsertMakeBreakTrans(); //#71893
							}
							else
							{
								_tellerVars.DrawerCounted = false;
								_tellerVars.CountedAmount = Decimal.MinValue;
								return true;
							}
						}
						else
						{
							if (EnteredZeroAmt())
							{
								_tellerVars.DrawerCounted = true;
								_tellerVars.CountedAmount = Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue);
								//
								InsertMakeBreakTrans(); //#71893
								return true;
							}
							else if (Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue) == 0 || dfCashDrawerCount.UnFormattedValue == null)
							{
								_tellerVars.DrawerCounted = false;
								_tellerVars.CountedAmount = Decimal.MinValue;
								InsertMakeBreakTrans(); //#71893
								return true;
							}
						}
					}
					#endregion
					//
					#region casnIn/cashOut count validation
					if (!isCashDrawerCount)	// cash In or cash out count
					{
						if ((!isBalDenomTracking && isDenomTracking) || _tlTranSet.CurTran.TranCode.Value == 928)
						{
							if (dfTotalCashEntered.UnFormattedValue == null || Convert.ToDecimal(dfTotalCashEntered.UnFormattedValue) == 0)
							{
								if (DialogResult.No == PMessageBox.Show(this, 360699, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
									return false;
								if (isTrancodeEntered)
									outputCashAmount = cashAmountReceived.Value;
								return true;
							}
							if (isDirtyForm)
							{
								System.Windows.Forms.DialogResult dialogTempResult = DialogResult.None;
								if (isTrancodeEntered)
								{
									dialogTempResult = PMessageBox.Show(this, 360700, MessageType.Question, MessageBoxButtons.YesNoCancel, string.Empty);
									//
									if (dialogTempResult == DialogResult.Cancel)
									{
										SetFocus();
										return false;
									}
									else if (dialogTempResult == DialogResult.No)
									{
										return true;
									}
									else if (dialogTempResult == DialogResult.Yes)
									{
										if (!OnActionSave(false))
											return false;
										InsertMakeBreakTrans(); //#71893
									}
								}
							}
						}
						else if (isBalDenomTracking)
						{
							//#70776 -added additional condition to avoid prompting user to complete denom when amt received is 0
							if ((dfTotalCashEntered.UnFormattedValue == null || Convert.ToDecimal(dfTotalCashEntered.UnFormattedValue) == 0)&&
								!cashAmountReceived.IsNull && cashAmountReceived.Value > 0)
							{
								PMessageBox.Show(this, 360701, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
								SetFocus();
								return false;
							}
							else if (isDirtyForm && !cashAmountReceived.IsNull && cashAmountReceived.Value > 0)
							{
								if (DialogResult.Yes == PMessageBox.Show(this, 360822, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
								{
									if (!OnActionSave(false))
										return false;
									//
									InsertMakeBreakTrans(); //#71893
								}
							}
						}
					}
					#endregion
					//
					return true;
				}
			}
			else
			{
				if (!isSaveAndClose)
				{
					isDirtyForm = this.IsFormDirty();
					if (isDirtyForm)
					{
						if (DialogResult.Yes == PMessageBox.Show(this, 360822, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
						{
							if (!OnActionSave(false))
								return false;
						}
						else
							return true;
					}
				}
				InsertMakeBreakTrans(); //#71893
				return true;
			}
			//
		}


		public override bool IsFormDirty()
		{
			bool isEdited = false;
			int tmpDenomId = 0;
			object objectValue = null;
			object storedQtyObj = null;
			int strapRollQty = 0;
			int storedStrapRollQty = 0;

			#region edit check for tab#1
			if (_denomTranType != "MB")
			{
				for( int rowId = 0; rowId < gridLooseCash.Count; rowId++ )
				{
					objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseDenomId.ColumnId );
					tmpDenomId = Convert.ToInt16(objectValue);
					foreach (Phoenix.BusObj.Teller.TlCashCount ccItem in cashCount)
					{
						if (ccItem.DenomId.Value == tmpDenomId)
						{
							objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseCountValue.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.CountValue.Value != Convert.ToDecimal(objectValue))
								{
									isEdited = true;
									break;
								}
							}
							objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseQuantity.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.Quantity.Value != Convert.ToInt32(objectValue))
								{
									isEdited = true;
									break;
								}
							}
							objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseAmount.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.Amt.Value != Convert.ToDecimal(objectValue))
								{
									isEdited = true;
									break;
								}
							}
						}
					}
					if (isEdited)
						return isEdited;
				}
			}
			#endregion
			//
			#region edit check for tab#2
			if (!isEdited)
			{
				for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
				{
					//region non make/break strap edit check
					if (_denomTranType != "MB")
					{
						objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomId.ColumnId );
						tmpDenomId = Convert.ToInt16(objectValue);
						foreach (Phoenix.BusObj.Teller.TlCashCount ccItem in cashCount)
						{
							if (ccItem.DenomId.Value == tmpDenomId)
							{
								objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollValue.ColumnId );
								if (objectValue != null)
								{
									if (ccItem.CountValue.Value != Convert.ToDecimal(objectValue))
									{
										isEdited = true;
										break;
									}
								}
								objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapQuantity.ColumnId );
								if (objectValue != null)
								{
									if (ccItem.Quantity.Value != Convert.ToInt32(objectValue))
									{
										isEdited = true;
										break;
									}
								}
								objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapAmount.ColumnId );
								if (objectValue != null)
								{
									if (ccItem.Amt.Value != Convert.ToDecimal(objectValue))
									{
										isEdited = true;
										break;
									}
								}
							}
						}
					}
					if (_denomTranType == "MB" || (!isEdited && _denomTranType == _mlListCashCloseOut))
					{
						objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollMakBrkQty.ColumnId );
						storedQtyObj = gridStrappedCash.GetCellValueUnformatted( rowId, colStoredStrapRollMakBrkQty.ColumnId );
						if (objectValue != null && storedQtyObj == null)
						{
							if (Convert.ToInt32(objectValue) > 0)
							{
								isEdited = true;
								break;
							}
						}
						else if (objectValue != null && storedQtyObj != null)
						{
							if (Convert.ToInt32(objectValue) > 0 || Convert.ToInt32(storedQtyObj) > 0)
							{
								strapRollQty = Convert.ToInt32(objectValue);
								storedStrapRollQty = Convert.ToInt32(storedQtyObj);
								if (strapRollQty != storedStrapRollQty && strapRollQty >= 0 && storedStrapRollQty >= 0)
								{
									isEdited = true;
									break;
								}
							}
						}
					}
					if (isEdited)
						return isEdited;
				}
			}
			#endregion
			//
			#region edit check for tab#3
			if (!isEdited && _denomTranType != "MB")
			{
				for( int rowId = 0; rowId < gridOtherItems.Count; rowId++ )
				{
					objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colOtherDenomId.ColumnId );
					tmpDenomId = Convert.ToInt16(objectValue);
					foreach (Phoenix.BusObj.Teller.TlCashCount ccItem in cashCount)
					{
						if (ccItem.DenomId.Value == tmpDenomId)
						{
							objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colCashItemValue.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.CountValue.Value != Convert.ToDecimal(objectValue))
								{
									isEdited = true;
									break;
								}
							}
							objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colOtherQuantity.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.Quantity.Value != Convert.ToInt32(objectValue))
								{
									isEdited = true;
									break;
								}
							}
							objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colOtherAmount.ColumnId );
							if (objectValue != null)
							{
								if (ccItem.Amt.Value != Convert.ToDecimal(objectValue))
								{
									isEdited = true;
									break;
								}
							}
						}
					}
					if (isEdited)
						return isEdited;
				}
			}
			#endregion
			//
			return isEdited;
			#region do not call the base
//			return base.IsFormDirty ();
			#endregion
		}
		#endregion

		#region cash count events

		#region Init events
		private Phoenix.Windows.Forms.ReturnType dlgTlCashCount_PInitBeginEvent()
		{
            //Begin #75604
            #region config
            PdfCurrency.ApplicationAssumeDecimal = (_tellerVars.AdTlControl.AssumeDecimals.Value == GlobalVars.Instance.ML.Y);
            CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            #endregion
            //End #75604
            this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = _busTlCashCount;
			//
			#region Read From Registry
			HandleRegSettings(false);
			#endregion
			//
			/* Begin #71885 */
			if (isCashDrawerCount)
				this.PMdiPrintEvent += new System.EventHandler(this.dlgTlCashCount_PMdiPrintEvent);
			/* End #71885 */
            SelectFirstControl = false; // WI - 1644
            //WI#8110
            if ((this.Workspace != null) && this._denomTranType == "O")
            {
                foreach (Control control in (Workspace as Form).Controls)
                {
                    if (control is PCookieCrumb)
                    {
                        control.Enabled = false;
                        break;
                    }

                }
            }
            //end WI#8110

			return ReturnType.Success;
		}

		private void dlgTlCashCount_PInitCompleteEvent()
		{
			_balDenomTracking = (_tellerVars.AdTlControl.BalDenomTracking.Value == GlobalVars.Instance.ML.Y);
			if (!isViewOnly)
			{
				LoadCashItems(false, false);
				CalcTotal();
                LoadInvItems(false, _savedTlInvItems, true);    //#140772
                //this.rbDrawer.Checked = true;   //#140772
				//
				gridLooseCash.Focus();
				//
				ResetReadOnlyColumn();
				if (gridLooseCash.Count > 0 && !isViewOnly)
				{
					gridLooseCash.SelectRow(0,true);
				}
			}

			#region Initialize
			EnableDisableVisibleLogic("FormComplete");
			//#71893
			if (_balDenomTracking)
				EnableDisableVisibleLogic("BalDenomTrackingOn");
			else
				EnableDisableVisibleLogic("BalDenomTrackingOff");
			//
			if (!isCashDrawerCount)
				lblLTotalCash.Text = (isCashInCount? CoreService.Translation.GetUserMessageX(360486) : CoreService.Translation.GetUserMessageX(360487));
			#endregion
			//
			#region set window title
			if (isCashInCount)
				_title = CoreService.Translation.GetUserMessageX(360484);
			else if (isCashOutCount)
				_title = CoreService.Translation.GetUserMessageX(360485);
			else if (_denomTranType == "MB")	//#71893
				_title = CoreService.Translation.GetUserMessageX(361018);
			else
				_title = CoreService.Translation.GetUserMessageX(360677);
			this.NewRecordTitle = string.Format(this.NewRecordTitle, _title);
			this.EditRecordTitle = string.Format(this.EditRecordTitle, _title);
			#endregion

			this.ActionSave.Click +=new PActionEventHandler(ActionSave_Click);
			_parentForm = Workspace.ContentWindow.CurrentWindow as PfwStandard;
			//
            if (_denomTranType == "MB")	//#71893
            {
                //#73720 - Move the below line before EnableDisableViewLogic call
                picTabs.SelectedIndex = 1;
                // picTabs.SelectedTab.Select();
                //picTabs_SelectedIndexChanged(this, new EventArgs());
                //picTabs.SelectTab(1);
                //picTabs.SelectedTab = picTabs.TabPages[1];
                gridStrappedCash.Focus();
                picTabs.IndexChanged(new EventArgs());

                //if (_balDenomTracking)
                //{
                //    if (isCashOutCount)
                //        EnableDisableVisibleLogic("BreakStrapOnly");
                //    else
                //    {
                //        if (!isCashInCount)
                //        {
                //            this.pbMakeStrapRoll.Visible = (picTabs.SelectedIndex == 1);
                //            this.pbClearMakeBreak.Visible = false;
                //            this.pbBreakStrapRoll.Visible = (picTabs.SelectedIndex == 1);
                //        }
                //    }
                //}
                //gridStrappedCash.Focus(); //hack
                //#74301 - Force column focus based on quantity or amount option selected
                if (rbStrapAmount.Checked)
                    colStrapAmount.Focus();
                else
                    colStrapQuantity.Focus();
            }
            else
            {
                gridLooseCash.Focus();
                //#74301 - Force column focus based on quantity or amount option selected
                if (rbLooseAmount.Checked || IsFirstRowBulkCoin())//#8287
                    colLooseAmount.Focus();
                else
                    colLooseQuantity.Focus();
            }

            Extension.ResizeWindow(this);
        }
        #endregion
        

        //
        #region action click
        private void pbClearTab_Click(object sender, PActionEventArgs e)
		{
			if (picTabs.SelectedIndex == 0)
			{
				if (gridLooseCash.Count > 0)
				{
					ClearLooseCashTab();
					gridLooseCash.SelectRow(0, true);
				}
			}
			else if (picTabs.SelectedIndex == 1)
			{
				if (gridStrappedCash.Count > 0)
				{
					ClearStrapCashTab();
					gridStrappedCash.SelectRow(0, true);
				}
			}
			else if (picTabs.SelectedIndex == 2)
			{
				if (gridOtherItems.Count > 0)
				{
					ClearOtherCashTab();
					gridOtherItems.SelectRow(0, true);
				}
			}
            else if (picTabs.SelectedIndex == 3)    //#140772
            {
                if (gridInventoryItems.Count > 0)
                {
                    ClearInventoryItemsTab();
                    gridInventoryItems.SelectRow(0, true);
                }
            }
			//
			CalcTotal();
			//
			ResetReadOnlyColumn();
		}

		private void pbClearAllTabs_Click(object sender, PActionEventArgs e)
		{
			ClearLooseCashTab();
			ClearStrapCashTab();
			ClearOtherCashTab();
			//
			CalcTotal();
			//
			if (picTabs.SelectedIndex == 0)
			{
				if (gridLooseCash.Count > 0)
					gridLooseCash.SelectRow(0, true);
			}
			else if (picTabs.SelectedIndex == 1)
			{
				if (gridStrappedCash.Count > 0)
					gridStrappedCash.SelectRow(0, true);
			}
			else if (picTabs.SelectedIndex == 2)
			{
				if (gridOtherItems.Count > 0)
					gridOtherItems.SelectRow(0, true);
			}
            else if (picTabs.SelectedIndex == 3)    //#140772
            {
                if (gridInventoryItems.Count > 0)
                    gridInventoryItems.SelectRow(0, true);
            }
			//
			ResetReadOnlyColumn();
		}

		private void dlgTlCashCount_Closed(object sender, EventArgs e)
		{
			if (!isViewOnly)
			{
				if ( _parentForm != null )
				{
                    //Begin #140895
                    if (_tlTranSet != null && _tlTranSet.IsTellerCaptureTran)
                        outputCashAmount = cashAmountReceived.Value;    // for teller capture the outgoing amount should be same as incoming
                    //End #140895

                    if (outputCashAmount == cashAmountReceived.Value)
						_parentForm.CallParent(this.ScreenId, "CashDenom", false);/*Bug #111419 Added ScreenID */
                    else
						_parentForm.CallParent(this.ScreenId, "CashDenom", true);/*Bug #111419 Added screenID*/
                }
			}
		}

		private void ActionSave_Click(object sender, PActionEventArgs e)
		{
            if (_invokedByWorkflow)     // #TellerWF
                return;

			isSaveAndClose = true;
            if (isSaveActionSuccess && !isCashDrawerCount) //#41475
            {
                this.Close();
            }
            else
                isSaveAndClose = false;     // #140895 - reset the flag
		}

		private void pbResetValue_Click(object sender, PActionEventArgs e)
		{
			ResetCountToOrigValues();
		}

		//#71893
		private void pbMakeStrapRoll_Click(object sender, PActionEventArgs e)
		{
			if (this.colMakeBreakStrapRolls.Text != CoreService.Translation.GetUserMessageX(361017)) //Break
			{
				this.colMakeBreakStrapRolls.Text = CoreService.Translation.GetUserMessageX(361016); //Make
				colStrapRollMakBrkQty.ReadOnly = false;
				colStrapRollMakBrkQty.Focus();
			}
			else
			{
				PMessageBox.Show(this, 361020, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return;
			}
		}

		private void pbClearMakeBreak_Click(object sender, PActionEventArgs e)
		{
			colMakeBreakStrapRolls.Text = null;
			colMakeBreakStrapRolls.UnFormattedValue = null;
			//
			colStrapRollMakBrkQty.UnFormattedValue = null;
		}

		private void pbBreakStrapRoll_Click(object sender, PActionEventArgs e)
		{
			if (this.colMakeBreakStrapRolls.Text != CoreService.Translation.GetUserMessageX(361016))	//Make
			{
				this.colMakeBreakStrapRolls.Text = CoreService.Translation.GetUserMessageX(361017); //Break
				colStrapRollMakBrkQty.ReadOnly = false;
				colStrapRollMakBrkQty.Focus();
			}
			else
			{
				PMessageBox.Show(this, 361019, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return;
			}
		}

		#endregion
		//
		#region column validation
		private void colLooseQuantity_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colLooseQuantity);
			if (!ValidateGrid(colLooseQuantity.XmlTag, true))
				e.Cancel = true;
		}

		private void colLooseAmount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colLooseAmount);
			if (!ValidateGrid(colLooseAmount.XmlTag, true))
				e.Cancel = true;
		}

		private void colStrapQuantity_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colStrapQuantity);
			if (!ValidateGrid(colStrapQuantity.XmlTag, true))
				e.Cancel = true;
		}

		private void colStrapAmount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colStrapAmount);
			if (!ValidateGrid(colStrapAmount.XmlTag, true))
				e.Cancel = true;
		}

		private void colOtherQuantity_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colOtherQuantity);
			if (!ValidateGrid(colOtherQuantity.XmlTag, true))
				e.Cancel = true;
		}

		private void colOtherAmount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colOtherAmount);
			if (!ValidateGrid(colOtherAmount.XmlTag, true))
				e.Cancel = true;
		}

		private void colStrapRollValue_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colStrapRollValue);
			colStrapAmount.UnFormattedValue = 0;
			colStrapQuantity.UnFormattedValue = 0;
			if (!ValidateGrid(colStrapRollValue.XmlTag, true))
				e.Cancel = true;
		}

		private void colCashItemValue_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colCashItemValue);
			colOtherQuantity.UnFormattedValue = 0;
			colOtherAmount.UnFormattedValue = 0;
			if (!ValidateGrid(colCashItemValue.XmlTag, true))
				e.Cancel = true;
		}

		private void colStrapRollMakBrkQty_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetFieldValueZero(colStrapRollMakBrkQty);
			if (!ValidateGrid(colStrapRollMakBrkQty.XmlTag, true))
				e.Cancel = true;
		}

		#endregion
		//
		#region column leave events
		private void colLooseQuantity_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colLooseAmount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colStrapRollValue_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colStrapQuantity_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colStrapAmount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colCashItemValue_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colOtherQuantity_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colOtherAmount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal(true);
		}

		private void colStrapRollMakBrkQty_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			colStrapRollMakBrkQty.ReadOnly = true;
			if (colStrapRollMakBrkQty.UnFormattedValue == null ||
				(colStrapRollMakBrkQty.UnFormattedValue != null && Convert.ToInt32(colStrapRollMakBrkQty.UnFormattedValue) <= 0))
				colMakeBreakStrapRolls.Text = null;
			else
			{
				if (_denomTranType == "MB")
					this.pbClearMakeBreak.Visible = true;
			}
		}
		#endregion
		//
		#region column activating events
		private void colLooseQuantity_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			if (colLooseDenomType.Text == mlBC)
				e.Cancel = true;
			else
			{
				e.Cancel = (rbLooseAmount.Checked);
			}
		}

		private void colLooseAmount_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			e.Cancel = (rbLooseQuantity.Checked && colLooseDenomType.Text != mlBC);
		}

		private void colStrapQuantity_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			e.Cancel = (rbStrapAmount.Checked);
		}

		private void colStrapAmount_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			e.Cancel = (rbStrapQuantity.Checked);
		}

		private void colOtherQuantity_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			e.Cancel = (rbOtherAmount.Checked);
		}

		private void colOtherAmount_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			e.Cancel = (rbOtherQuantity.Checked);
		}
		#endregion
		//
		#region option button validation
		private void cbStrapChangeValue_Click(object sender, EventArgs e)
		{
			if (cbStrapChangeValue.Checked)
			{
				colStrapRollValue.ReadOnly = false;
				colStrapRollValue.Focus();

			}
			else
			{
				colStrapRollValue.ReadOnly = true;
				ResetReadOnlyColumn();
			}
		}

		private void cbOtherChangeValue_Click(object sender, EventArgs e)
		{
			if (cbOtherChangeValue.Checked)
			{
				colCashItemValue.ReadOnly = false;
				colCashItemValue.Focus();
			}
			else
			{
				colCashItemValue.ReadOnly = true;
				ResetReadOnlyColumn();
			}
		}

		private void rbLooseQuantity_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

		private void rbLooseAmount_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

		private void rbStrapAmount_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

		private void rbStrapQuantity_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

		private void rbOtherAmount_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

		private void rbOtherQuantity_PhoenixUICheckedChangedEvent(object sender, EventArgs e)
		{
			ResetReadOnlyColumn();
		}

        //Begin #140772
        void colInvItemCountedQty_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            if (colInvItemCountedQty.UnFormattedValue != null)
            {
                if (colInvItemDefaultedCount.UnFormattedValue != null)
                    colDifference.UnFormattedValue = Convert.ToInt32(colInvItemCountedQty.UnFormattedValue) - Convert.ToInt32(colInvItemDefaultedCount.UnFormattedValue);
                else
                    colDifference.UnFormattedValue = Convert.ToInt32(colInvItemCountedQty.UnFormattedValue);
                if (Convert.ToUInt32(colInvItemCountedQty.UnFormattedValue) >= 0)
                    colVerified.Checked = true;
                else
                    colVerified.Checked = false;
            }
            else
                colVerified.Checked = false;
        }

        void rbInvItemQuantity_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            ResetReadOnlyColumn();
        }

        void rbDrawer_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            if (rbDrawer.Checked)
            {
                LoadInvItems(true, _tempTlInvItemsBranch, false);
                LoadInvItems(false, _tempTlInvItemsDrawer, false);
                if (gridInventoryItems.Count > 0 && !isViewOnly)
                {
                    gridInventoryItems.SelectRow(0, true);
                    gridInventoryItems.Focus();
                }
                ResetReadOnlyColumn();
            }
        }

        void rbBranch_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            if (rbBranch.Checked)
            {
                LoadInvItems(true, _tempTlInvItemsDrawer, false);
                LoadInvItems(false, _tempTlInvItemsBranch, false);
                if (gridInventoryItems.Count > 0 && !isViewOnly)
                {
                    gridInventoryItems.SelectRow(0, true);
                    gridInventoryItems.Focus();
                }
                ResetReadOnlyColumn();
            }
        }
        //End #140772
		#endregion
		//
		#region grid events
		//#71893
		private void gridStrappedCash_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
//			if (_denomTranType == "MB" )
//			{
//				if (colStrapRollMakBrkQty.UnFormattedValue == null && colMakeBreakStrapRolls.UnFormattedValue == null)
//					this.pbClearMakeBreak.Visible = false;
//				else
//					this.pbClearMakeBreak.Visible = true;
//			}
			if (!isCashInCount && _balDenomTracking)
			{
				if (e.RowId >= 0)
				{
					object destRowObj = null;
					object currentRowObj = null;
					this.pbClearMakeBreak.Visible = false;
					destRowObj = gridStrappedCash.GetCellValueUnformatted(e.RowId, colStrapRollMakBrkQty.ColumnId);
					if (gridStrappedCash.ContextRow != -1)
						currentRowObj = gridStrappedCash.GetCellValueUnformatted(gridStrappedCash.ContextRow, colStrapRollMakBrkQty.ColumnId);
					if (destRowObj != null || currentRowObj != null)
					{
						if ((e.RowId == gridStrappedCash.ContextRow && currentRowObj != null) || (destRowObj != null && e.RowId != gridStrappedCash.ContextRow))
							this.pbClearMakeBreak.Visible = true;
						else
							this.pbClearMakeBreak.Visible = false;
					}
				}
			}
		}

        //void gridInventoryItems_BeforePopulate(object sender, GridPopulateArgs e)
        //{
        //    //if (AppInfo.Instance.IsAppOnline)
        //    //{
        //    //    _tlInventoryItemCount = new TlInventoryItemCount();
        //    //    _tlInventoryItemCount.ResponseTypeId = 10;
        //    //    gridInventoryItems.Filters.Clear();
        //    //    gridInventoryItems.ListViewObject = _tlInventoryItemCount;
        //    //    _tlInventoryItemCount.FilterBranch.Value = -1;
        //    //    _tlInventoryItemCount.FilterDrawer.Value = -1;

        //    //    if (this.rbDrawer.Checked)
        //    //    {
        //    //        _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
        //    //        _tlInventoryItemCount.FilterDrawer.Value = _drawerNo.Value;
        //    //    }

        //    //    if (this.rbBranch.Checked)
        //    //    {
        //    //        _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
        //    //    }
        //    //    _tlInventoryItemCount.FilterStatus.Value = CoreService.Translation.GetListItemX(ListId.InventoryStatus, "Available");
        //    //    //EnableDisableVisibleLogic(EnableDisableVisible.BeforePopulate);
        //    //}
        //}

        //void gridInventoryItems_FetchRowDone(object sender, GridRowArgs e)
        //{
        //    colTypeIdDesc.UnFormattedValue = Convert.ToString(colTypeId.UnFormattedValue) + " - " + Convert.ToString(colTypeDescription.UnFormattedValue);
        //}

        //void gridInventoryItems_AfterPopulate(object sender, GridPopulateArgs e)
        //{

        //}

        //void colInvItemCountedQtyBranch_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        //{
        //    if (colInvItemCountedQtyBranch.UnFormattedValue != null)
        //    {
        //        if (colInvItemDefaultedCountBranch.UnFormattedValue != null)
        //            colDifferenceBranch.UnFormattedValue = Convert.ToInt32(colInvItemCountedQtyBranch.UnFormattedValue) - Convert.ToInt32(colInvItemDefaultedCountBranch.UnFormattedValue);
        //        else
        //            colDifferenceBranch.UnFormattedValue = Convert.ToInt32(colInvItemCountedQtyBranch.UnFormattedValue);
        //        if (Convert.ToUInt32(colInvItemCountedQtyBranch.UnFormattedValue) >= 0)
        //            colVerifiedBranch.Checked = true;
        //        else
        //            colVerifiedBranch.Checked = false;
        //    }
        //    else
        //        colVerifiedBranch.Checked = false;
        //}

        //void gridInventoryItemsBranch_FetchRowDone(object sender, GridRowArgs e)
        //{
        //    colTypeIdDescBranch.UnFormattedValue = Convert.ToString(colTypeIdBranch.UnFormattedValue) + " - " + Convert.ToString(colTypeDescriptionBranch.UnFormattedValue);
        //}

        //void gridInventoryItemsBranch_AfterPopulate(object sender, GridPopulateArgs e)
        //{

        //}

        //void gridInventoryItemsBranch_BeforePopulate(object sender, GridPopulateArgs e)
        //{
        //    if (AppInfo.Instance.IsAppOnline)
        //    {
        //        _tlInventoryItemCount = new TlInventoryItemCount();
        //        _tlInventoryItemCount.ResponseTypeId = 10;
        //        gridInventoryItemsBranch.Filters.Clear();
        //        gridInventoryItemsBranch.ListViewObject = _tlInventoryItemCount;
        //        _tlInventoryItemCount.FilterBranch.Value = -1;
        //        _tlInventoryItemCount.FilterDrawer.Value = -1;

        //        if (this.rbDrawer.Checked)
        //        {
        //            _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
        //            _tlInventoryItemCount.FilterDrawer.Value = _drawerNo.Value;
        //        }

        //        if (this.rbBranch.Checked)
        //        {
        //            _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
        //        }
        //        _tlInventoryItemCount.FilterStatus.Value = CoreService.Translation.GetListItemX(ListId.InventoryStatus, "Available");
        //        //EnableDisableVisibleLogic(EnableDisableVisible.BeforePopulate);
        //    }
        //}
		#endregion
		//
		#region tab events
		private void picTabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (picTabs.SelectedIndex == 0)
			{
				if (isCashDrawerCount)
				{
					if (!this.gbTotals1.Visible)
						this.Name0.Controls.Add(this.gbTotals1);
				}
				else
				{
					if (!this.gbTotals.Visible)
						this.Name0.Controls.Add(this.gbTotals);
				}
				if (this.pbResetValue.Visible)
					this.pbResetValue.Visible = false;
				//#71893
				this.pbBreakStrapRoll.Visible = false;
				this.pbMakeStrapRoll.Visible = false;
				this.pbClearMakeBreak.Visible = false;

				if (gridLooseCash.Count > 0 && !isViewOnly)
				{
					gridLooseCash.SelectRow(0,true);
					gridLooseCash.Focus();
                    //#74301 - Force column focus based on quantity or amount option selected
                    //if (rbLooseAmount.Checked)
                    //    colLooseAmount.Focus();
                    //else
                    //    colLooseQuantity.Focus();
				}
			}
			else
			{
				this.pbResetValue.Visible = false;	//do not enable
				if (picTabs.SelectedIndex == 1)
				{
					//#71893
					this.pbBreakStrapRoll.Visible = (_balDenomTracking && !isCashInCount);
					this.pbMakeStrapRoll.Visible = (_balDenomTracking && !isCashInCount && !isCashOutCount );
					if (gridStrappedCash.ContextRow >= 0 && _balDenomTracking && !isCashInCount)
					{
						object objStrapRollMakBrkQty = gridStrappedCash.GetCellValueUnformatted(gridStrappedCash.ContextRow, colStrapRollMakBrkQty.ColumnId);
						object objMakeBreakStrapRoll = gridStrappedCash.GetCellValueUnformatted(gridStrappedCash.ContextRow, colMakeBreakStrapRolls.ColumnId);
						this.pbClearMakeBreak.Visible = (_balDenomTracking && objStrapRollMakBrkQty != null && objMakeBreakStrapRoll != null);
					}
					if (isCashDrawerCount)
					{
						this.Name1.Controls.Add(this.gbTotals1);
					}
					else
					{
						this.Name1.Controls.Add(this.gbTotals);
					}
					if (gridStrappedCash.Count > 0 && !isViewOnly)
					{
						gridStrappedCash.SelectRow(0,true);
                        //#74301 - Force column focus based on quantity or amount option selected
                        //if (rbStrapAmount.Checked)
                        //    colStrapAmount.Focus();
                        //else
                        //    colStrapQuantity.Focus();
					}
				}
				else if (picTabs.SelectedIndex == 2)
				{
					//#71893
					this.pbBreakStrapRoll.Visible = false;
					this.pbMakeStrapRoll.Visible = false;
					this.pbClearMakeBreak.Visible = false;
					if (isCashDrawerCount)
					{
						this.Name2.Controls.Add(this.gbTotals1);
					}
					else
					{
						this.Name2.Controls.Add(this.gbTotals);
					}
                    if (gridOtherItems.Count > 0 && !isViewOnly)
                    {
                        gridOtherItems.SelectRow(0, true);
                        //#74301 - Force column focus based on quantity or amount option selected
                        //if (rbOtherAmount.Checked)
                        //    colOtherAmount.Focus();
                        //else
                        //    colOtherQuantity.Focus();
                    }
				}
                else if (picTabs.SelectedIndex == 3)    //#140772
                {
                    //#71893
                    this.rbInvItemAmount.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                    this.rbInvItemQuantity.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    this.rbInvItemQuantity.Checked = true;
                    this.cbInvItemChangeValue.Checked = false;
                    this.cbInvItemChangeValue.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Disable);
                    this.pbMakeStrapRoll.Visible = false;
                    this.pbClearMakeBreak.Visible = false;
                    if (gridInventoryItems.Count > 0 && !isViewOnly)
                    {
                        gridInventoryItems.SelectRow(0, true);
                        gridInventoryItems.Focus();
                    }
                }
			}
			ResetReadOnlyColumn();
		}
        #endregion

        #endregion
        //Begin Task#99223
        #region Call Parent 
        public override void CallParent(params object[] paramList)
        {
            string caller = "";
           
            if (paramList != null && paramList[0] != null)
                caller = Convert.ToString(paramList[0]);

            if (caller == "Calculator")
            {
                if (picTabs.SelectedIndex == 0)
                {
                    if (paramList.Length > 1)
                    {
                        if (rbLooseQuantity.Checked == true)
                        {
                            colLooseQuantity.UnFormattedValue = paramList[1];
                            colLooseQuantity.Focus();
                        }
                        else
                        {
                            colLooseAmount.UnFormattedValue = paramList[1];
                            colLooseAmount.Focus();
                       }                      
                    }
                   
                }
                else if (picTabs.SelectedIndex == 1)
                {
                    if (paramList.Length > 1)
                    {
                        if (rbStrapQuantity.Checked == true)
                        {
                            colStrapQuantity.UnFormattedValue = paramList[1];
                            colStrapQuantity.Focus();
                        }
                        else
                        {
                            colStrapAmount.UnFormattedValue = paramList[1];
                            colStrapAmount.Focus();                           
                        }
                    }
                   
                }
                else if (picTabs.SelectedIndex == 2)
                {
                    if (paramList.Length > 1)
                    {
                        if (rbOtherQuantity.Checked == true)
                        {
                            colOtherQuantity.UnFormattedValue = paramList[1];
                            colOtherQuantity.Focus();                            
                        }
                        else
                        {
                            colOtherAmount.UnFormattedValue = paramList[1];
                            colOtherAmount.Focus();                          
                        }
                    }   
                                     
                }

            }
        }
        #endregion
        //End Task#99223

        #region private methods

        #region get current grid
        private void GetCurrentGrid(int gridId)
		{
			currGrid = null;

			switch (gridId)
			{
				case 1:
					currGrid = (PGrid)gridLooseCash;
					break;
				case 2:
					currGrid = (PGrid)gridStrappedCash;
					break;
				case 3:
					currGrid = (PGrid)gridOtherItems;
					break;
                case 4:
                    currGrid = (PGrid)gridInventoryItems;
                    break;
			}
		}

		private void GetCurrentGrid()
		{
			if (picTabs.SelectedIndex == 0)
				GetCurrentGrid(1);
			else if (picTabs.SelectedIndex == 1)
				GetCurrentGrid(2);
			else if (picTabs.SelectedIndex == 2)
				GetCurrentGrid(3);
            else if (picTabs.SelectedIndex == 3)    //#140772
                GetCurrentGrid(4);
		}
		#endregion

		#region load cash items
		private void LoadDenomTypes()
		{
			#region load cash denom config from admin
			_busTlCashCount.BranchNo.Value = _branchNo.Value;
			_busTlCashCount.DrawerNo.Value = _drawerNo.Value;
			_busTlCashCount.EffectiveDt.Value = _effectiveDt.Value;
			_busTlCashCount.LoadCashCountDenom(cashCount, _denomTranType); //#71893
			#endregion
		}

		private void AddNewItem(Phoenix.BusObj.Teller.TlCashCount cash)
		{
			if (cash != null)
			{
				#region Loose Bills and Coins
				if (cash.DenomType.Value == mlLB ||
					cash.DenomType.Value == mlLC ||
					cash.DenomType.Value == mlBC)
				{
					gridLooseCash.AddNewRow();
					//
					colLooseDemonination.FieldToColumn(cash.CashDenomDesc);
					colLooseDenom.FieldToColumn(cash.Denom);
					colLooseDenomType.FieldToColumn(cash.DenomType);
					colLooseDenomId.FieldToColumn(cash.DenomId);

                    #region #79574
                    if (_isDeviceDeposit && !cash.TcrQuantity.IsNull)
                    {
                        colLooseTcrQty.UnFormattedValue = cash.TcrQuantity.Value;
                        colLooseTcrQty.FormattedValue = cash.TcrQuantity.Value.ToString();
                        colLooseTcrQty.Text = cash.TcrQuantity.Value.ToString();
                    }
                    if (_isDeviceDeposit && !cash.TcrAmt.IsNull)
                    {
                        colLooseTcrAmt.UnFormattedValue = cash.TcrAmt.Value;
                        colLooseTcrAmt.Text = cash.TcrAmt.Value.ToString();
                        colLooseTcrAmt.FormattedValue = CurrencyHelper.GetFormattedValue(cash.TcrAmt.Value);
                    }
                    #endregion

                    if (!cash.Quantity.IsNull)
					{
						colLooseQuantity.UnFormattedValue = cash.Quantity.Value;
						colLooseQuantity.FormattedValue = cash.Quantity.Value.ToString();
						colLooseQuantity.Text = cash.Quantity.Value.ToString();
					}
					if (!cash.Amt.IsNull)
					{
						colLooseAmount.UnFormattedValue = cash.Amt.Value;
						colLooseAmount.FormattedValue = CurrencyHelper.GetFormattedValue(cash.Amt.Value);
						colLooseAmount.Text = cash.Amt.Value.ToString();
					}
                    /* Task#54515 - Begin */
                    /* Bug#55279-added condition - cash.DenomType.Value == mlBC */
                    if (!cash.CountValue.IsNull && cash.DenomType.Value == mlBC)
                    {
                        colLooseCountValue.UnFormattedValue = cash.CountValue.Value;
                        colLooseCountValue.FormattedValue = CurrencyHelper.GetFormattedValue(cash.CountValue.Value);
                        colLooseCountValue.Text = cash.CountValue.Value.ToString();
                    }
                    /* Task#54515 - End */
                }
                #endregion
                //
                #region Strapped Bills and Coins
                if (cash.DenomType.Value == mlSB ||
					cash.DenomType.Value == mlRC)
				{
					gridStrappedCash.AddNewRow();
					//
					gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapDenomination.ColumnId, cash.CashDenomDesc.Value );
					gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapDenom.ColumnId, cash.Denom.Value );
					gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapDenomType.ColumnId, cash.DenomType.Value );
					gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapDenomId.ColumnId, cash.DenomId.Value );

					if (!cash.Quantity.IsNull)
						gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapQuantity.ColumnId, cash.Quantity.Value );
					if (!cash.Amt.IsNull)
						gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapAmount.ColumnId, cash.Amt.Value );
					if (!cash.CountValue.IsNull)
						gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapRollValue.ColumnId, cash.CountValue.Value );
					if (!cash.OrigCountValue.IsNull)
						gridStrappedCash.SetCellValueUnFormatted(gridStrappedCash.ContextRow, colStrapOrigRollValue.ColumnId, cash.OrigCountValue.Value );

				}
				#endregion
				//
				#region Other Cash Items
				if (cash.DenomType.Value == mlOC)
				{
					gridOtherItems.AddNewRow();
					//
					gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colDescription.ColumnId, cash.CashDenomDesc.Value );
					gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherDenom.ColumnId, cash.Denom.Value );
					gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherDenomType.ColumnId, cash.DenomType.Value );
					gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherDenomId.ColumnId, cash.DenomId.Value );

					if (!cash.Quantity.IsNull)
						gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherQuantity.ColumnId, cash.Quantity.Value );
					if (!cash.Amt.IsNull)
						gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherAmount.ColumnId, cash.Amt.Value );
					if (!cash.CountValue.IsNull)
						gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colCashItemValue.ColumnId, cash.CountValue.Value );
					if (!cash.OrigCountValue.IsNull)
						gridOtherItems.SetCellValueUnFormatted(gridOtherItems.ContextRow, colOtherOrigCashItemValue.ColumnId, cash.OrigCountValue.Value );

				}
				#endregion
			}
		}

        private void LoadInventoryTypes()
        {
            XmlNode xNode;

            if (AppInfo.Instance.IsAppOnline)
            {
                _tlInventoryItemCount = new TlInventoryItemCount();
                _tlInventoryItemCount.ResponseTypeId = 10;
                gridInventoryItems.Filters.Clear();
                _tlInventoryItemCount.FilterStatus.Value = CoreService.Translation.GetListItemX(ListId.InventoryStatus, "Available");
                //Get inventory type count for drawer location
                _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
                _tlInventoryItemCount.FilterDrawer.Value = _drawerNo.Value;
                xNode = DataService.Instance.GetListView(_tlInventoryItemCount);
                foreach (XmlNode recordNode in xNode.SelectNodes(StringConstants.RECORD))
                {
                    if (recordNode.NodeType != XmlNodeType.Element)
                        continue;
                    _tlInventoryItemCount = new TlInventoryItemCount();
                    _tlInventoryItemCount.LoadNodeToObject(recordNode, false, true);
                    _tempTlInvItemsDrawer.Add(_tlInventoryItemCount);
                }

                //Get inventory type count for branch location
                _tlInventoryItemCount = new TlInventoryItemCount();
                _tlInventoryItemCount.ResponseTypeId = 10;
                gridInventoryItems.Filters.Clear();
                _tlInventoryItemCount.FilterStatus.Value = CoreService.Translation.GetListItemX(ListId.InventoryStatus, "Available");
                //Get inventory type count for branch location
                _tlInventoryItemCount.FilterBranch.Value = _branchNo.Value;
                _tlInventoryItemCount.FilterDrawer.Value = -1;
                xNode = DataService.Instance.GetListView(_tlInventoryItemCount);
                foreach (XmlNode recordNode in xNode.SelectNodes(StringConstants.RECORD))
                {
                    if (recordNode.NodeType != XmlNodeType.Element)
                        continue;
                    _tlInventoryItemCount = new TlInventoryItemCount();
                    _tlInventoryItemCount.LoadNodeToObject(recordNode, false, true);
                    _tempTlInvItemsBranch.Add(_tlInventoryItemCount);
                }
            }
        }

        private void AddInvItems(ArrayList localInvItems)
        {
            int rowId = 0;
            gridInventoryItems.ResetTable();
            if (localInvItems != null && localInvItems.Count > 0)
            {
                //Load stored items from arraylist to the table
                foreach (TlInventoryItemCount invItem in localInvItems)
                {
                    if (!invItem.TypeId.IsNull)
                    {
                        gridInventoryItems.AddNewRow();

                        gridInventoryItems.SetCellValueUnFormatted(rowId, colTypeId.ColumnId, invItem.TypeId.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colTypeDescription.ColumnId, invItem.TypeDesc.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colTypeIdDesc.ColumnId, invItem.TypeIdDesc.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colInvClass.ColumnId, invItem.Class.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colInvBranchNo.ColumnId, invItem.BranchNo.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colInvDrawerNo.ColumnId, invItem.DrawerNo.Value);
                        if (!invItem.NoItems.IsNull)
                            gridInventoryItems.SetCellValueUnFormatted(rowId, colInvItemDefaultedCount.ColumnId, invItem.NoItems.Value);
                        if (!invItem.CountNoItems.IsNull)
                            gridInventoryItems.SetCellValueUnFormatted(rowId, colInvItemCountedQty.ColumnId, invItem.CountNoItems.Value);
                        gridInventoryItems.SetCellValueUnFormatted(rowId, colVerified.ColumnId, invItem.Verified.Value);
                        if (!invItem.Difference.IsNull)
                            gridInventoryItems.SetCellValueUnFormatted(rowId, colDifference.ColumnId, invItem.Difference.Value);
                        rowId++;
                    }
                }
            }
        }



        private void LoadInvItems(bool isLoadBackList, ArrayList localInvItems, bool isSavedItems)
        {
            int nRow = 0;
            if (!isLoadBackList)
            {
                if (isSavedItems)
                {
                    if (_savedTlInvItems.Count == 0 && _tempTlInvItemsDrawer.Count == 0 && _tempTlInvItemsBranch.Count == 0)
                        LoadInventoryTypes();
                    else if (_savedTlInvItems.Count > 0 && _tempTlInvItemsDrawer.Count == 0 && _tempTlInvItemsBranch.Count == 0)
                    {
                        foreach (TlInventoryItemCount invItem in localInvItems)
                        {
                            if ((!invItem.Location.IsNull && invItem.Location.Value == CoreService.Translation.GetListItemX(ListId.InventoryLocation, "Drawer")) ||
                                invItem.DrawerNo.Value > 0)
                                _tempTlInvItemsDrawer.Add(invItem);
                            else
                                _tempTlInvItemsBranch.Add(invItem);
                        }
                    }
                }
                else
                    AddInvItems(localInvItems);
            }
            else
            {
                if (localInvItems != null && gridInventoryItems.Count > 0)
                {
                    localInvItems.Clear();

                    if (gridInventoryItems.Count > 0)
                    {
                        while (nRow < gridInventoryItems.Count)
                        {
                            _tlInventoryItemCount = new TlInventoryItemCount();

                            _tlInventoryItemCount.TypeId.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colTypeId.ColumnId);
                            _tlInventoryItemCount.TypeDesc.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colTypeDescription.ColumnId);
                            _tlInventoryItemCount.TypeIdDesc.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colTypeIdDesc.ColumnId);
                            _tlInventoryItemCount.Class.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colInvClass.ColumnId);
                            _tlInventoryItemCount.BranchNo.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colInvBranchNo.ColumnId);
                            _tlInventoryItemCount.DrawerNo.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colInvDrawerNo.ColumnId);
                            _tlInventoryItemCount.NoItems.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colInvItemDefaultedCount.ColumnId);
                            _tlInventoryItemCount.CountNoItems.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colInvItemCountedQty.ColumnId);
                            _tlInventoryItemCount.ItemValue.Value = 0;
                            _tlInventoryItemCount.CountValue.Value = 0;
                            _tlInventoryItemCount.Status.Value = CoreService.Translation.GetListItemX(ListId.InventoryStatus, "Available");
                            _tlInventoryItemCount.Verified.Value = ((gridInventoryItems.GetCellValueUnformatted(nRow, colVerified.ColumnId) != null &&
                                                                     Convert.ToString(gridInventoryItems.GetCellValueUnformatted(nRow, colVerified.ColumnId)) != "N") ? "Y" : "N");
                            _tlInventoryItemCount.Difference.ValueObject = gridInventoryItems.GetCellValueUnformatted(nRow, colDifference.ColumnId);
                            if (!_tlInventoryItemCount.TypeId.IsNull)
                                localInvItems.Add(_tlInventoryItemCount);
                            nRow = nRow + 1;
                        }
                    }
                }
            }
        }

		private void LoadCashItems(bool isLoadBackList, bool isbulkSave)
		{
			int tmpDenomId = 0;
			if (!isLoadBackList)
			{
				if (cashCount.Count == 0)
				{
					LoadDenomTypes();
				}
				//
				foreach( Phoenix.BusObj.Teller.TlCashCount cash in cashCount)
				{
					AddNewItem(cash);
				}
			}
			else
			{
				#region Load back structure with modified Loose Bills/Coin
				for( int rowId = 0; rowId < gridLooseCash.Count; rowId++ )
				{
					object objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseDenomId.ColumnId );
					tmpDenomId = Convert.ToInt16(objectValue);
					//
					foreach(Phoenix.BusObj.Teller.TlCashCount cashItem in cashCount)
					{
						if (tmpDenomId == cashItem.DenomId.Value)
						{
							if (isbulkSave)
							{
								cashItem.CountValue.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseCountValue.ColumnId );
								cashItem.Quantity.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseQuantity.ColumnId );
								cashItem.Amt.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseAmount.ColumnId );
							}
							break;
						}
					}
				}
				#endregion
				//
				#region Load back structure with modified Strap Bills/Roll Coin
				for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
				{
					object objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomId.ColumnId );
					tmpDenomId = Convert.ToInt16(objectValue);
					//
					foreach(Phoenix.BusObj.Teller.TlCashCount cashItem in cashCount)
					{
						if (tmpDenomId == cashItem.DenomId.Value)
						{
							if (isbulkSave)
							{
								cashItem.CountValue.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollValue.ColumnId );
								cashItem.Quantity.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapQuantity.ColumnId );
								cashItem.Amt.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapAmount.ColumnId );
							}
							break;
						}
					}
				}
				#endregion
				//
				#region Load back structure with modified Other Cash Items
				for( int rowId = 0; rowId < gridOtherItems.Count; rowId++ )
				{
					object objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colOtherDenomId.ColumnId );
					tmpDenomId = Convert.ToInt16(objectValue);
					//
					foreach(Phoenix.BusObj.Teller.TlCashCount cashItem in cashCount)
					{
						if (tmpDenomId == cashItem.DenomId.Value)
						{
							if (isbulkSave)
							{
								cashItem.CountValue.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colCashItemValue.ColumnId );
								cashItem.Quantity.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherQuantity.ColumnId );
								cashItem.Amt.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherAmount.ColumnId );
							}
							break;
						}
					}
				}
				#endregion
			}
		}
		#endregion

		#region check for grid edit
		private bool IsGridEdited(int gridId)
		{
			bool isEdited = false;
			int nRow = 0;
			int nCol = 0;
			GetCurrentGrid(gridId);

			int startColIndex = currGrid.Columns.GetColumnIndex("CountValue");

			if (currGrid.Count > 0)
			{
				while (nRow < currGrid.Count && !isEdited)
				{
					currGrid.SelectRow(nRow, false);
					Phoenix.BusObj.Teller.TlCashCount _testBusObjEdit = new Phoenix.BusObj.Teller.TlCashCount();

					currGrid.RowToObject(_testBusObjEdit);
					nCol = startColIndex;
					if (currGrid.Columns[nCol].ReadOnly)
						nCol = currGrid.Columns.GetColumnIndex("Quantity");

					Phoenix.BusObj.Teller.TlCashCount ccItem = (Phoenix.BusObj.Teller.TlCashCount)cashCount[nRow];
					while( nCol < currGrid.Columns.Count)
					{
						PGridColumn col1 = (PGridColumn)currGrid.Columns[nCol];

						if (!_testBusObjEdit.GetFieldByXmlTag(col1.XmlTag).IsNull && !ccItem.GetFieldByXmlTag(col1.XmlTag).IsNull)
						{
							if (_testBusObjEdit.GetFieldByXmlTag(col1.XmlTag).Value.ToString() != ccItem.GetFieldByXmlTag(col1.XmlTag).Value.ToString())
							{
								isEdited = true;
								break;
							}
						}
						nCol = nCol + 1;
					}
					nRow = nRow + 1;
				}
			}
			else
				isEdited = false;

			return isEdited;
		}
		#endregion

		#region calc total
		private void CalcTotal(bool isFromColumnEdit)
		{
			if (isFormCompleted)
				CalcTotal();
		}

		private void SetFieldValueZero(PGridColumn col)
		{
			if (col.UnFormattedValue == null || col.Text == string.Empty)
				col.UnFormattedValue = 0;
		}

		private void CalcTotal()
		{
			decimal totalCash = ((cashAmountReceived.IsNull ||
				cashAmountReceived.DecimalValue == Decimal.MinValue)? 0 : cashAmountReceived.Value);
			decimal totalAmountEntered = 0;
            decimal totalTcrAmountEntered = 0;  //#79574
			decimal totalAmountRemaining = 0;
			//
			decimal tmpAmount = 0;
			//
			dfTotalCashEntered.UnFormattedValue = 0;
			dfAmountRemaining.UnFormattedValue = 0;
			dfTotalCash.UnFormattedValue = totalCash;
            dfDrawerCashCounted.UnFormattedValue = 0;   //#79574
            dfTCRCashDeposited.UnFormattedValue = 0;   //#79574
			//
			dfCashDrawerCount.UnFormattedValue = 0;
			dfDifference.UnFormattedValue = 0;
			dfEndingCash.UnFormattedValue = totalCash;
			//
			for( int rowId = 0; rowId < gridLooseCash.Count; rowId++ )
			{
				object objectValue = gridLooseCash.GetCellValueUnformatted( rowId, colLooseAmount.ColumnId );
				if (objectValue == null)
					tmpAmount = 0;
				else
					tmpAmount = Convert.ToDecimal(objectValue);
				totalAmountEntered = totalAmountEntered + tmpAmount;
                //
                #region #79574
                objectValue = gridLooseCash.GetCellValueUnformatted(rowId, colLooseTcrAmt.ColumnId);
                if (objectValue == null)
                    tmpAmount = 0;
                else
                    tmpAmount = Convert.ToDecimal(objectValue);
                totalTcrAmountEntered = totalTcrAmountEntered + tmpAmount;
                #endregion
            }
			//
			for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
			{
				object objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapAmount.ColumnId );
				if (objectValue == null)
					tmpAmount = 0;
				else
					tmpAmount = Convert.ToDecimal(objectValue);
				//
				totalAmountEntered = totalAmountEntered + tmpAmount;
			}
			//
			for( int rowId = 0; rowId < gridOtherItems.Count; rowId++ )
			{
				object objectValue = gridOtherItems.GetCellValueUnformatted( rowId, colOtherAmount.ColumnId );
				if (objectValue == null)
					tmpAmount = 0;
				else
					tmpAmount = Convert.ToDecimal(objectValue);
				//
				totalAmountEntered = totalAmountEntered + tmpAmount;
			}
			//
            totalAmountRemaining = totalCash - totalAmountEntered - totalTcrAmountEntered;  //#79574
			//
			if (!isCashDrawerCount)
			{
                if (_isDeviceDeposit)   //#79574
                {
                    dfTCRCashDeposited.UnFormattedValue = totalTcrAmountEntered;
                    dfDrawerCashCounted.UnFormattedValue = totalAmountEntered;
                    dfTotalCashEntered.UnFormattedValue = totalAmountEntered + totalTcrAmountEntered;
                    dfAmountRemaining.UnFormattedValue = totalAmountRemaining;
                    dfTotalCash.UnFormattedValue = totalCash;
                }
                else
                {
                    dfTotalCashEntered.UnFormattedValue = totalAmountEntered;
                    dfAmountRemaining.UnFormattedValue = totalAmountRemaining;
                    dfTotalCash.UnFormattedValue = totalCash;
                }
			}
			else
			{
				dfCashDrawerCount.UnFormattedValue = totalAmountEntered;
				dfDifference.UnFormattedValue = totalAmountEntered - totalCash; //#71266
			}
			//
			SetFieldColor();
		}
		#endregion

		#region focus
		private void SetFocus()
		{
			if (picTabs.SelectedIndex == 0 && gridLooseCash.Count > 0)
				gridLooseCash.SelectRow(0,true);
			else if (picTabs.SelectedIndex == 1 && gridStrappedCash.Count > 0)
				gridStrappedCash.SelectRow(0,true);
			else if (picTabs.SelectedIndex == 2 && gridOtherItems.Count > 0)
				gridOtherItems.SelectRow(0, true);
		}
		#endregion

		#region reset to original count value
		private void ResetCountToOrigValues()
		{
			int nRow = 0;
			int countValueCol = 0;
			int origCountValueCol = 0;
			//
			if (picTabs.SelectedIndex == 1)
				GetCurrentGrid(2);
			else if (picTabs.SelectedIndex == 2)
				GetCurrentGrid(3);
			//
			if (currGrid == null)
				return;
			//
			countValueCol = currGrid.Columns.GetColumnIndex("CountValue");
			origCountValueCol = currGrid.Columns.GetColumnIndex("OrigCountValue");
			Phoenix.BusObj.Teller.TlCashCount _tempObj = new Phoenix.BusObj.Teller.TlCashCount();
			//
			while(nRow < currGrid.Count)
			{
				currGrid.SelectRow(nRow, false);
				currGrid.RowToObject(_tempObj);
				_tempObj.CountValue.Value = _tempObj.OrigCountValue.Value;
				if (!(_tempObj.Quantity.IsNull || _tempObj.Amt.IsNull))
				{
					if (!_tempObj.ValidateDenom(_tempObj.GetFieldByXmlTag("CountValue").ToString()))
						return;
				}
				nRow = nRow + 1;
			}
		}
		#endregion

		#region clear tab methods
		private void ClearLooseCashTab()
		{
			for(int rowId = 0; rowId < gridLooseCash.Count; rowId++)
			{
				gridLooseCash.SetCellValueUnFormatted(rowId, colLooseQuantity.ColumnId, null);
				gridLooseCash.SetCellValueUnFormatted(rowId, colLooseAmount.ColumnId, null);
			}
			foreach(Phoenix.BusObj.Teller.TlCashCount count in cashCount)
			{
				if (count.DenomType.Value == mlLB ||
					count.DenomType.Value == mlLC ||
					count.DenomType.Value == mlBC)
				{
					count.CountValue.SetValue(null, EventBehavior.None);		// #6615 - count.CountValue.SetValueToNull();
					count.Quantity.SetValue(null, EventBehavior.None);			// #6615 - count.Quantity.SetValueToNull();
					count.Amt.SetValue(null, EventBehavior.None);				// #6615 - count.Amt.SetValueToNull();
				}
			}
		}

		private void ClearStrapCashTab()
		{
			for(int rowId = 0; rowId < gridStrappedCash.Count; rowId++)
			{
				gridStrappedCash.SetCellValueUnFormatted(rowId, colStrapQuantity.ColumnId, null);
				gridStrappedCash.SetCellValueUnFormatted(rowId, colStrapAmount.ColumnId, null);
			}
			foreach(Phoenix.BusObj.Teller.TlCashCount count in cashCount)
			{
				if (count.DenomType.Value == mlSB ||
					count.DenomType.Value == mlRC)
				{
					count.Quantity.SetValue(null, EventBehavior.None);		// #6615 - count.Quantity.SetValueToNull();
					count.Amt.SetValue(null, EventBehavior.None);			// #6615 - count.Amt.SetValueToNull();
				}
			}
		}

		private void ClearOtherCashTab()
		{
			for(int rowId = 0; rowId < gridOtherItems.Count; rowId++)
			{
				gridOtherItems.SetCellValueUnFormatted(rowId, colOtherQuantity.ColumnId, null);
				gridOtherItems.SetCellValueUnFormatted(rowId, colOtherAmount.ColumnId, null);
			}
			foreach(Phoenix.BusObj.Teller.TlCashCount count in cashCount)
			{
				if (count.DenomType.Value == mlOC)
				{
					count.Quantity.SetValue(null, EventBehavior.None);		// #6615 - count.Quantity.SetValueToNull();
					count.Amt.SetValue(null, EventBehavior.None);			// #6615 - count.Amt.SetValueToNull();
				}
			}
		}

        private void ClearInventoryItemsTab()   //#140772
		{
			for(int rowId = 0; rowId < gridInventoryItems.Count; rowId++)
			{
                gridInventoryItems.SetCellValueUnFormatted(rowId, colInvItemCountedQty.ColumnId, null);
                gridInventoryItems.SetCellValueUnFormatted(rowId, colVerified.ColumnId, null);
                gridInventoryItems.SetCellValueUnFormatted(rowId, colDifference.ColumnId, null);
			}
		}
		#endregion

		#region enable/disable logic


		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
			{
				this.pbResetValue.Visible = false;
				picTabs.SelectedIndex = 0;
				//#71893
				if (!isViewOnly && _denomTranType != "MB")
				{
					#region Set Option Button based on registry defaults
					// default
					rbLooseQuantity.Checked = true;
					rbStrapQuantity.Checked = true;
					rbOtherQuantity.Checked = true;
                    //rbDrawer.Checked = true;    //#22168
					// set based on registry value
					rbLooseAmount.Checked = (_looseCountBySettings == "1");
					rbStrapAmount.Checked = (_strapCountBySettings == "2");
					rbOtherAmount.Checked = (_otherCountBySettings == "3");
                    rbBranch.Checked = (_inventoryItemLocBySettings == "4");    //#22168
					#endregion
					//
					colStrapRollValue.ReadOnly = true;
					colCashItemValue.ReadOnly = true;
				}
				//
				if (isCashDrawerCount)
				{
					this.gbTotals.Visible = false;
					this.gbTotals1.Visible = true;
				}
				else if (_denomTranType != "MB")	//#71893
				{
					this.gbTotals1.Visible = false;
					this.gbTotals.Visible = true;
                }
                #region #79574
                if (!_isDeviceDeposit)
                {
                    this.colLooseTcrQty.Visible = false;
                    this.colLooseTcrAmt.Visible = false;
                    //this.colLooseTcrAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
                    //this.colLooseTcrAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
                    //this.colLooseTcrAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
                    //this.colLooseTcrAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
                    this.colLooseDemonination.Width = this.colLooseDemonination.Width + this.colLooseTcrQty.Width + this.colLooseTcrAmt.Width;
                    this.dfDrawerCashCounted.Visible = false;
                    this.dfTCRCashDeposited.Visible = false;
                    this.gbTotals.Height = this.gbTotals.Height - this.dfTCRCashDeposited.Height;
                    //
                    this.gbLooseCount.Height = this.gbLooseCount.Height + this.dfTCRCashDeposited.Height;
                    this.gbStrappedCount.Height = this.gbStrappedCount.Height + this.dfTCRCashDeposited.Height;
                    this.gbOtherItemsCount.Height = this.gbOtherItemsCount.Height + this.dfTCRCashDeposited.Height;
                    //
                    this.gridLooseCash.Height = this.gridLooseCash.Height + this.dfTCRCashDeposited.Height;
                    this.gridStrappedCash.Height = this.gridStrappedCash.Height + this.dfTCRCashDeposited.Height;
                    this.gridOtherItems.Height = this.gridOtherItems.Height + this.dfTCRCashDeposited.Height;
                }
                else
                {
                    if (cashCount == null || cashCount.Count <= 0)
                        _isDeviceDeposit = false;
                    else
                    {
                        _isDeviceDeposit = false;
                        foreach (Phoenix.BusObj.Teller.TlCashCount ccItem in cashCount)
                        {
                            if (!ccItem.TcrAmt.IsNull && !ccItem.TcrQuantity.IsNull)
                            {
                                _isDeviceDeposit = true;
                                break;
                            }
                        }
                    }
                }
                #endregion

                isFormCompleted = true;
                //
                if (!isCashDrawerCount || !TellerVars.Instance.IsAppOnline) //#140772
                    this.Name3.IsDisabled = true;
                else
                {
                    if (TellerVars.Instance.IsInventoryTrackingEnabled)    //#140772
                    {
                        this.Name3.IsDisabled = false;
                        this.colInvItemCountedQty.ReadOnly = false;
                        this.rbInvItemQuantity.Checked = true;   //#140772
                        this.rbDrawer.Checked = true;    //#140772
                        //if (_inventoryItemLocBySettings == "4")    //#22168
                        //    this.rbBranch.Checked = true;
                        //else
                        //    this.rbDrawer.Checked = true;
                    }
                    else
                        this.Name3.IsDisabled = true;
                }
                ResetFormForSupViewOnlyMode();  //#79314
			}
			//#71893
			if (callerInfo == "BalDenomTrackingOn")
			{
				if (_denomTranType == "MB")
				{
					this.pbClearTab.Visible = false;
					this.pbClearAllTabs.Visible = false;
					this.rbLooseAmount.Enabled = false;
					this.rbLooseAmount.Enabled = false;
					this.rbStrapAmount.Enabled = false;
					this.rbStrapQuantity.Enabled = false;
					this.rbOtherAmount.Enabled = false;
					this.rbOtherQuantity.Enabled = false;
					this.cbStrapChangeValue.Visible = false;
					this.cbOtherChangeValue.Visible = false;
					//
					colLooseAmount.ReadOnly = true;
					colLooseQuantity.ReadOnly = true;
					colStrapRollValue.ReadOnly = true;
					colStrapAmount.ReadOnly = true;
					colStrapQuantity.ReadOnly = true;
					colOtherAmount.ReadOnly = true;
					colOtherQuantity.ReadOnly = true;
					colCashItemValue.ReadOnly = true;
				}
				//
				if (isCashInCount)
					EnableDisableVisibleLogic("NoMakeBreakStrapsRoll");
				else if (isCashOutCount)
					EnableDisableVisibleLogic("BreakStrapOnly");
				else
					EnableDisableVisibleLogic("MakeBreakStrapsRoll");
			}
			if (callerInfo == "BalDenomTrackingOff")	//#71893
			{
				EnableDisableVisibleLogic("NoMakeBreakStrapsRoll");
			}
			if (callerInfo == "BreakStrapOnly")			//#71893
			{
				this.pbMakeStrapRoll.Visible = false;
				this.pbBreakStrapRoll.Visible = (picTabs.SelectedIndex == 1);
				this.pbClearMakeBreak.Visible = false;
				//
				EnableDisableVisibleLogic("MakeBreakColumns");
			}
			if (callerInfo == "NoMakeBreakStrapsRoll")	//#71893
			{
				this.pbMakeStrapRoll.Visible = false;
				this.pbClearMakeBreak.Visible = false;
				this.pbBreakStrapRoll.Visible = false;
				//
				this.colMakeBreakStrapRolls.Visible = false;
				this.colStrapRollMakBrkQty.Visible = false;
				this.cbStrapChangeValue.Visible = (_balDenomTracking == false);	//#72828
				this.cbOtherChangeValue.Visible = (_balDenomTracking == false); //#72828
			}
			if (callerInfo == "MakeBreakStrapsRoll")
			{
				this.pbMakeStrapRoll.Visible = (picTabs.SelectedIndex == 1);
				this.pbClearMakeBreak.Visible = false;
				this.pbBreakStrapRoll.Visible = (picTabs.SelectedIndex == 1);
				//
				EnableDisableVisibleLogic("MakeBreakColumns");
				//
				if (_denomTranType == "MB")
				{
					this.gbTotals.Visible = false;
					this.gbTotals1.Visible = false;
					this.gbLooseCount.Height = this.gbLooseCount.Height + this.gbTotals.Height;
					this.gbStrappedCount.Height = this.gbStrappedCount.Height + this.gbTotals.Height;
					this.gbOtherItemsCount.Height = this.gbOtherItemsCount.Height + this.gbTotals.Height;
					//
					this.gridLooseCash.Height = this.gridLooseCash.Height + this.gbTotals.Height;
					this.gridStrappedCash.Height = this.gridStrappedCash.Height + this.gbTotals.Height;
					this.gridOtherItems.Height = this.gridOtherItems.Height + this.gbTotals.Height;
				}
			}
			if (callerInfo == "MakeBreakColumns")
			{
				this.colMakeBreakStrapRolls.Visible = true;
				this.colStrapRollMakBrkQty.Visible = true;
				this.colMakeBreakStrapRolls.ReadOnly= true;
				this.colStrapRollMakBrkQty.ReadOnly = true;
				this.colStrapDenomination.Width = this.colStrapDenomination.Width -
					(colMakeBreakStrapRolls.Width + colStrapRollMakBrkQty.Width);
				//
				this.cbStrapChangeValue.Visible = false;
				this.cbOtherChangeValue.Visible = false;
			}
		}
		#endregion

		#region handle registry
		private void HandleRegSettings( bool save )
		{
			string keyValue = null;
			string keyName = "CountDenomBy";
			string location = ScreenId.ToString();
			string screenSetting = "";
			string[] regValues = null;

			#region read from registry
			if (!save)
			{
				if ( Helper.GetFromRegistry( location, keyName, ref keyValue ))
				{
					if ( keyValue != null && keyValue != String.Empty )
					{
						regValues = keyValue.Split(",".ToCharArray());
						if ( regValues.GetUpperBound(0) > 1 )
						{
							_looseCountBySettings = Convert.ToString( regValues[0]);
							_strapCountBySettings = Convert.ToString( regValues[1]);
							_otherCountBySettings = Convert.ToString( regValues[2]);
                            if (regValues.Length > 3)
                                _inventoryItemLocBySettings = Convert.ToString(regValues[3]);   //#22168
						}
					}
				}
			}
			#endregion

			#region save to registry
			if (save)
			{
				screenSetting = ( rbLooseQuantity.Checked? Convert.ToString(0) : Convert.ToString(1) ) + ",";
				screenSetting = screenSetting + ( rbStrapQuantity.Checked? Convert.ToString(0) : Convert.ToString(2) ) + ",";
				screenSetting = screenSetting + ( rbOtherQuantity.Checked? Convert.ToString(0) : Convert.ToString(3) ) + ",";
                screenSetting = screenSetting + (rbDrawer.Checked ? Convert.ToString(0) : Convert.ToString(4));
				keyValue = screenSetting;
				Helper.SaveToRegistry( location, keyName, keyValue );
			}
			#endregion
		}
		#endregion

		#region reset read only column
		private void ResetReadOnlyColumn()
		{
			if (picTabs.SelectedIndex == 0)
			{
				if (rbLooseQuantity.Checked)
					colLooseQuantity.Focus();
				else
					colLooseAmount.Focus();
			}
			else if (picTabs.SelectedIndex == 1)
			{
				if (rbStrapQuantity.Checked)
					colStrapQuantity.Focus();
				else
					colStrapAmount.Focus();
			}
			else if (picTabs.SelectedIndex == 2)
			{
				if (rbOtherQuantity.Checked)
					colOtherQuantity.Focus();
				else
					colOtherAmount.Focus();
			}
            else if (picTabs.SelectedIndex == 3)
            {
                //if (rbInvItemQuantity.Checked)
                colInvItemCountedQty.Focus();
            }
		}
		#endregion

		#region misc
		private void SetFieldColor()
		{
			//#71493 - For some reason Code was Commented
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) == 0)
				dfDifference.ForeColor = System.Drawing.Color.Black;
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
				dfDifference.ForeColor = System.Drawing.Color.Blue;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
				dfDifference.ForeColor = System.Drawing.Color.Red;
		}
		#endregion

		#region validate grid
		private bool ValidateGrid( string editedField, bool loadRowFromObject)
		{
			bool isSuccess = true;
			//
			if (loadRowFromObject)
			{
				_tempTlCashCountObj = new Phoenix.BusObj.Teller.TlCashCount();
				//
				if (picTabs.SelectedIndex == 0)
					LooseContextRowScreenToObject(gridLooseCash.ContextRow, editedField);
				else if (picTabs.SelectedIndex == 1)
					StrapContextRowScreenToObject(gridStrappedCash.ContextRow, editedField);
				else if (picTabs.SelectedIndex == 2)
					OtherCashContextRowScreenToObject(gridOtherItems.ContextRow, editedField);
			}
			//
			if (editedField != colStrapRollMakBrkQty.XmlTag)
				isSuccess = _tempTlCashCountObj.ValidateDenom(editedField);
			else
				isSuccess = _tempTlCashCountObj.ValidateMakeBreakQty();
			//
			if (loadRowFromObject)
			{
				if(!isSuccess)
				{
					if (!_tempTlCashCountObj.ValidationReturnCode.IsNull && _tempTlCashCountObj.ValidationReturnCode.Value != 0)
					{
						if (_tempTlCashCountObj.ValidationReturnCode.Value < 0)
						{
							_returnCodeDesc = _gbHelper.GetSPMessageText( _tempTlCashCountObj.ValidationReturnCode.Value, false );
							PMessageBox.Show(this, 360687, MessageType.Warning, MessageBoxButtons.OK, new string[] {Convert.ToString(_tempTlCashCountObj.ValidationReturnCode.Value) + " - " + _returnCodeDesc});
						}
						else
						{
							PMessageBox.Show(this, _tempTlCashCountObj.ValidationReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						}
						if (editedField != string.Empty)
						{
							if (picTabs.SelectedIndex == 0)
							{
								if (editedField == colLooseQuantity.XmlTag)
									colLooseQuantity.Focus();
								else if (editedField == colLooseAmount.XmlTag)
									colLooseAmount.Focus();
								gridLooseCash.Focus();
							}
							else if (picTabs.SelectedIndex == 1)
							{
								if (editedField == colStrapQuantity.XmlTag)
									colStrapQuantity.Focus();
								else if (editedField == colStrapAmount.XmlTag)
									colStrapAmount.Focus();
								else if (editedField == colStrapRollValue.XmlTag)
									colStrapRollValue.Focus();
								else if (editedField == colStrapRollMakBrkQty.XmlTag)
									colStrapRollMakBrkQty.Focus();
								gridStrappedCash.Focus();
							}
							else if (picTabs.SelectedIndex == 2)
							{
								if (editedField == colOtherQuantity.XmlTag)
									colOtherQuantity.Focus();
								else if (editedField == colOtherAmount.XmlTag)
									colOtherAmount.Focus();
								else if (editedField == colCashItemValue.XmlTag)
									colCashItemValue.Focus();
								gridOtherItems.Focus();
							}
						}
					}
				}
				else
				{
					if (picTabs.SelectedIndex == 0)
						LooseContextRowObjectToScreen(gridLooseCash.ContextRow);
					else if (picTabs.SelectedIndex == 1)
						StrapContextRowObjectToScreen(gridStrappedCash.ContextRow);
					else if (picTabs.SelectedIndex == 2)
						OtherCashContextRowObjectToScreen(gridOtherItems.ContextRow);
				}
			}
			//
			return isSuccess;
		}
		#endregion

		#region Context row screen to object
		private void LooseContextRowScreenToObject(int rowId, string editedField)
		{
			if (_tempTlCashCountObj == null)
				_tempTlCashCountObj = new Phoenix.BusObj.Teller.TlCashCount();

			if (editedField != null)
			{
				_tempTlCashCountObj.DenomId.Value = Convert.ToInt32(colLooseDenomId.UnFormattedValue);
				_tempTlCashCountObj.DenomType.Value = colLooseDenomType.Text;
				_tempTlCashCountObj.Denom.Value = Convert.ToDecimal(colLooseDenom.UnFormattedValue);
				if (colLooseQuantity.Text != string.Empty)
					_tempTlCashCountObj.Quantity.Value = Convert.ToInt32(colLooseQuantity.UnFormattedValue );
                if (colLooseAmount.Text != string.Empty)
                {
                    //Begin #9517
                    //_tempTlCashCountObj.Amt.Value = Convert.ToDecimal(colLooseAmount.UnFormattedValue);
                    _tempTlCashCountObj.Amt.Value = Math.Round(Convert.ToDecimal(colLooseAmount.UnFormattedValue), 2, MidpointRounding.AwayFromZero);
                    colLooseAmount.UnFormattedValue = Convert.ToString(_tempTlCashCountObj.Amt.Value);
                    //End #9517
                }
                else
                    _tempTlCashCountObj.Amt.SetValue(null, EventBehavior.None);		// #6615 - _tempTlCashCountObj.Amt.SetValueToNull();
			}
			else
			{
				_tempTlCashCountObj.DenomId.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseDenomId.ColumnId );
				_tempTlCashCountObj.DenomType.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseDenomType.ColumnId );
				_tempTlCashCountObj.Denom.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseDenom.ColumnId );
				_tempTlCashCountObj.CountValue.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseCountValue.ColumnId );
				_tempTlCashCountObj.Quantity.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseQuantity.ColumnId );
				_tempTlCashCountObj.Amt.ValueObject = gridLooseCash.GetCellValueUnformatted( rowId, colLooseAmount.ColumnId );
			}

		}

		private void StrapContextRowScreenToObject(int rowId, string editedField)
		{
			if (_tempTlCashCountObj == null)
				_tempTlCashCountObj = new Phoenix.BusObj.Teller.TlCashCount();
			if (editedField != null)
			{
				_tempTlCashCountObj.DenomId.Value = Convert.ToInt32(colStrapDenomId.UnFormattedValue);
				_tempTlCashCountObj.DenomType.Value = colStrapDenomType.Text;
				_tempTlCashCountObj.Denom.Value = Convert.ToDecimal(colStrapDenom.UnFormattedValue);
				if (colStrapRollValue.Text != string.Empty)
					_tempTlCashCountObj.CountValue.Value = Convert.ToDecimal(colStrapRollValue.UnFormattedValue);
				else
					_tempTlCashCountObj.CountValue.SetValue(null, EventBehavior.None);		// #6615 - _tempTlCashCountObj.CountValue.SetValueToNull();
				if (colStrapQuantity.Text != string.Empty)
					_tempTlCashCountObj.Quantity.Value = Convert.ToInt32(colStrapQuantity.UnFormattedValue);
				if (colStrapAmount.Text != string.Empty)
					_tempTlCashCountObj.Amt.Value = Convert.ToDecimal(colStrapAmount.UnFormattedValue);
				else
					_tempTlCashCountObj.Amt.SetValue(null, EventBehavior.None);				// #6615 - _tempTlCashCountObj.Amt.SetValueToNull();
				if (colStrapRollMakBrkQty.Text != string.Empty)	//#71893
					_tempTlCashCountObj.StrapRollMakBrkQty.Value = Convert.ToInt32(colStrapRollMakBrkQty.UnFormattedValue);
			}
			else
			{
				_tempTlCashCountObj.DenomId.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomId.ColumnId );
				_tempTlCashCountObj.DenomType.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomType.ColumnId );
				_tempTlCashCountObj.Denom.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenom.ColumnId );
				_tempTlCashCountObj.CountValue.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollValue.ColumnId );
				_tempTlCashCountObj.Quantity.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapQuantity.ColumnId );
				_tempTlCashCountObj.Amt.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapAmount.ColumnId );
				_tempTlCashCountObj.StrapRollMakBrkQty.ValueObject = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollMakBrkQty.ColumnId ); //#71893
			}

		}

		private void OtherCashContextRowScreenToObject(int rowId, string editedField)
		{
			if (_tempTlCashCountObj == null)
				_tempTlCashCountObj = new Phoenix.BusObj.Teller.TlCashCount();

			if (editedField != null)
			{
				_tempTlCashCountObj.DenomId.Value = Convert.ToInt32(colOtherDenomId.UnFormattedValue);
				_tempTlCashCountObj.DenomType.Value = colOtherDenomType.Text;
				_tempTlCashCountObj.Denom.Value = Convert.ToDecimal(colOtherDenom.UnFormattedValue);
				if (colCashItemValue.Text != string.Empty)
					_tempTlCashCountObj.CountValue.Value = Convert.ToDecimal(colCashItemValue.UnFormattedValue);
				else
					_tempTlCashCountObj.CountValue.SetValue(null, EventBehavior.None);	// #6615 - _tempTlCashCountObj.CountValue.SetValueToNull();
				if (colOtherQuantity.Text != string.Empty)
					_tempTlCashCountObj.Quantity.Value = Convert.ToInt32(colOtherQuantity.UnFormattedValue);
				if (colOtherAmount.Text != string.Empty)
					_tempTlCashCountObj.Amt.Value = Convert.ToDecimal(colOtherAmount.UnFormattedValue);
				else
					_tempTlCashCountObj.Amt.SetValue(null, EventBehavior.None);			// #6615 - _tempTlCashCountObj.Amt.SetValueToNull();
			}
			else
			{
				_tempTlCashCountObj.DenomId.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherDenomId.ColumnId );
				_tempTlCashCountObj.DenomType.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherDenomType.ColumnId );
				_tempTlCashCountObj.Denom.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherDenom.ColumnId );
				_tempTlCashCountObj.CountValue.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colCashItemValue.ColumnId );
				_tempTlCashCountObj.Quantity.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherQuantity.ColumnId );
				_tempTlCashCountObj.Amt.ValueObject = gridOtherItems.GetCellValueUnformatted( rowId, colOtherAmount.ColumnId );
			}

		}
		#endregion

		#region Context row object to screen
		private void LooseContextRowObjectToScreen(int rowId)
		{
			if (_tempTlCashCountObj.DenomType.Value == mlBC && !_tempTlCashCountObj.CountValue.IsNull)
				gridLooseCash.SetCellValueUnFormatted( rowId, colLooseCountValue.ColumnId, _tempTlCashCountObj.CountValue.Value );
			if (!_tempTlCashCountObj.Quantity.IsNull)
				gridLooseCash.SetCellValueUnFormatted( rowId, colLooseQuantity.ColumnId, _tempTlCashCountObj.Quantity.Value );
			if (!_tempTlCashCountObj.Amt.IsNull)
				gridLooseCash.SetCellValueUnFormatted( rowId, colLooseAmount.ColumnId, _tempTlCashCountObj.Amt.Value );
		}

		private void StrapContextRowObjectToScreen(int rowId)
		{
			if (!_tempTlCashCountObj.Quantity.IsNull)
				gridStrappedCash.SetCellValueUnFormatted( rowId, colStrapQuantity.ColumnId, _tempTlCashCountObj.Quantity.Value );
			if (!_tempTlCashCountObj.Amt.IsNull)
				gridStrappedCash.SetCellValueUnFormatted( rowId, colStrapAmount.ColumnId, _tempTlCashCountObj.Amt.Value );
		}

		private void OtherCashContextRowObjectToScreen(int rowId)
		{
			if (!_tempTlCashCountObj.Quantity.IsNull)
				gridOtherItems.SetCellValueUnFormatted( rowId, colOtherQuantity.ColumnId, _tempTlCashCountObj.Quantity.Value );
			if (!_tempTlCashCountObj.Amt.IsNull)
				gridOtherItems.SetCellValueUnFormatted( rowId, colOtherAmount.ColumnId, _tempTlCashCountObj.Amt.Value );
		}
		#endregion

		#region other private methods
		private bool EnteredZeroAmt()
		{
			bool zeroAmtFound = false;
			object objectValue = null;
			string strQty = string.Empty;

			for( int rowId = 0; rowId < gridLooseCash.Count; rowId++ )
			{
				objectValue = gridLooseCash.GetCellValueFormatted( rowId, colLooseQuantity.ColumnId );
				strQty = Convert.ToString(objectValue);
				if (strQty != string.Empty && strQty != null && strQty != "")
				{
					zeroAmtFound = true;
					break;
				}
			}
			//
			if (zeroAmtFound)
				return zeroAmtFound;
			//
			if (!zeroAmtFound)
			{
				strQty = string.Empty;
				for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
				{
					objectValue = gridStrappedCash.GetCellValueFormatted( rowId, colStrapQuantity.ColumnId );
					strQty = Convert.ToString(objectValue);
					if (strQty != string.Empty && strQty != null && strQty != "")
					{
						zeroAmtFound = true;
						break;
					}
				}
				//
				if (zeroAmtFound)
					return zeroAmtFound;
				//
			}
			if (!zeroAmtFound)
			{
				for( int rowId = 0; rowId < gridOtherItems.Count; rowId++ )
				{
					objectValue = gridOtherItems.GetCellValueFormatted( rowId, colOtherQuantity.ColumnId );
					strQty = Convert.ToString(objectValue);
					if (strQty != string.Empty && strQty != null && strQty != "")
					{
						zeroAmtFound = true;
						break;
					}
				}
				if (zeroAmtFound)
					return zeroAmtFound;
			}
			//
			return zeroAmtFound;
		}

		private void CallXMThruCDS(string callerName)
		{
			try
			{
				using (new WaitCursor())
				{
					if (callerName == "InsertMakeBreakStrapTran")
					{
						DataService.Instance.ProcessRequest(_tellerJournal);
					}
					else if (callerName == "InsertMakeStrapTranDenoms")
					{
						DataService.Instance.ProcessRequest(_tellerCount);
					}
				}
			}
			catch(PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void PrepareCommonMakeBreakTranParams()
		{
			_tellerJournal = new TlJournal();
			_tellerJournal.ActionType = XmActionType.New;
			_tellerJournal.EmplId.Value = _tellerVars.EmployeeId;
			_tellerJournal.EffectiveDt.Value = (_effectiveDt.Value == DateTime.MinValue ? GlobalVars.SystemDate : _effectiveDt.Value);
			_tellerJournal.CreateDt.IsCreateDt = true;
			_tellerJournal.BranchNo.Value = _branchNo.Value;
			_tellerJournal.DrawerNo.Value = _drawerNo.Value;
			_tellerJournal.MemoPostAmt.Value = 0;
			_tellerJournal.NetAmt.Value = 0;
			_tellerJournal.Reversal.Value = 0;
			_tellerJournal.SequenceNo.Value = 0;
			_tellerJournal.BatchStatus.Value = 0;
			_tellerJournal.TranStatus.Value = 0;
			_tellerJournal.SuspectAcct.Value = GlobalVars.Instance.ML.N;
			_tellerJournal.PodStatus.Value = 9;
			_tellerJournal.CrncyId.Value = 0;
			_tellerJournal.SubSequence.Value = 0;
			_tellerJournal.MemoFloat.Value = 0;
			_tellerJournal.CashCount.Value = GlobalVars.Instance.ML.Y;
		}

		private void PrepareCommonMakeBreakTranDenomParams()
		{
			_tellerCount = new Phoenix.BusObj.Teller.TlCashCount();
			_tellerCount.ActionType = XmActionType.New;
			_tellerCount.CreateDt.Value = DateTime.Now;
			_tellerCount.RowVersion.Value = 1;
			_tellerCount.Ptid.Value = 2742;
			_tellerCount.BranchNo.Value = _branchNo.Value;
			_tellerCount.DrawerNo.Value = _drawerNo.Value;
			_tellerCount.EffectiveDt.Value = (_effectiveDt.Value == DateTime.MinValue ? GlobalVars.SystemDate : _effectiveDt.Value);
		}

		private void MakeBreakDenomTrans(bool isTranTypeMake, decimal journalPtid)
		{
			#region Insert MAK/BRK cash count denom
			object objectValue = null;
			int tmpStrapMBQty = 0;
			int strapCount = 0;
			for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
			{
				objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollMakBrkQty.ColumnId );
				strapCount = 0;
				tmpStrapMBQty = 0;
				if (objectValue != null)
				{
					tmpStrapMBQty = Convert.ToInt16(objectValue);
					if (tmpStrapMBQty > 0)
					{
						objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colMakeBreakStrapRolls.ColumnId );
						if (objectValue != null)
						{
							if ((isTranTypeMake && Convert.ToString(objectValue) == CoreService.Translation.GetUserMessageX(361016)) ||
								(!isTranTypeMake && Convert.ToString(objectValue) == CoreService.Translation.GetUserMessageX(361017)))//Make
							{
								strapCount = 1;
								//
								#region prepare common count params
								_tellerCount = new Phoenix.BusObj.Teller.TlCashCount();
								_tellerCount.ActionType = XmActionType.New;
								_tellerCount.CreateDt.Value = DateTime.Now;
								_tellerCount.RowVersion.Value = 1;
								//_tellerCount.Ptid.Value = 2742;
								_tellerCount.BranchNo.Value = _branchNo.Value;
								_tellerCount.DrawerNo.Value = _drawerNo.Value;
								_tellerCount.EffectiveDt.Value = (_effectiveDt.Value == DateTime.MinValue ? GlobalVars.SystemDate : _effectiveDt.Value);
								_tellerCount.Quantity.Value = tmpStrapMBQty;
								_tellerCount.DenomId.Value = Convert.ToInt32(gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomId.ColumnId ));
								_tellerCount.DenomType.Value = Convert.ToString(gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenomType.ColumnId ));
								_tellerCount.CountValue.Value = Convert.ToDecimal(gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollValue.ColumnId ));
								_tellerCount.Denom.Value = Convert.ToDecimal(gridStrappedCash.GetCellValueUnformatted( rowId, colStrapDenom.ColumnId ));
								_tellerCount.JournalPtid.Value = (int)journalPtid;
								#endregion
							}
							//
							#region MAK denoms
							if (strapCount > 0 && isTranTypeMake)
								_tellerCount.TranType.Value = "M";
							#endregion
							//
							#region BRK denoms
							if (strapCount > 0 && !isTranTypeMake)
								_tellerCount.TranType.Value = "B";
							#endregion
							//
							#region proecess insert
							if (strapCount > 0)
								CallXMThruCDS("InsertMakeStrapTranDenoms");
							#endregion
						}
					}
				}
			}
			#endregion
		}


		private void InsertMakeBreakTrans()
		{
			object objectValue = null;
			short tmpStrapMBQty = 0;
			int makeStrapCount = 0;
			int breakStrapCount = 0;
			decimal makJournalPtid = 0;
			decimal brkJournalPtid = 0;
			//
			using (new WaitCursor())
			{
				if (_balDenomTracking && !isCashInCount)
				{
					try
					{
						for( int rowId = 0; rowId < gridStrappedCash.Count; rowId++ )
						{
							objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colStrapRollMakBrkQty.ColumnId );
							if (objectValue != null)
							{
								tmpStrapMBQty = Convert.ToInt16(objectValue);
								if (tmpStrapMBQty > 0)
								{
									objectValue = gridStrappedCash.GetCellValueUnformatted( rowId, colMakeBreakStrapRolls.ColumnId );
									if (objectValue != null)
									{
										if (Convert.ToString(objectValue) == CoreService.Translation.GetUserMessageX(361016))//Make
											makeStrapCount = 1;
										else
											breakStrapCount = 1;
									}
								}
							}
						}
						//
						#region Insert MAK tran
						if (makeStrapCount > 0)
						{
							#region prepare common journal params
							PrepareCommonMakeBreakTranParams();
							#endregion
							//
							_tellerJournal.TlTranCode.Value = "MAK";
							_tellerJournal.Description.Value = CoreService.Translation.GetUserMessageX(361010);	//Make Strap/Roll
							CallXMThruCDS("InsertMakeBreakStrapTran");
							//
							makJournalPtid = Convert.ToDecimal(_tellerJournal.IdentityField.Value);
						}
						#endregion
						//
						#region Insert BRK tran
						if (breakStrapCount > 0)
						{
							#region prepare common journal params
							PrepareCommonMakeBreakTranParams();
							#endregion
							//
							_tellerJournal.TlTranCode.Value = "BRK";
							_tellerJournal.Description.Value = CoreService.Translation.GetUserMessageX(361011);	//Break Strap/Roll
							CallXMThruCDS("InsertMakeBreakStrapTran");
							//
							brkJournalPtid = Convert.ToDecimal(_tellerJournal.IdentityField.Value);
						}
						#endregion
						//
						#region Insert MAK/BRK cash count denom
						if (makeStrapCount > 0)
							MakeBreakDenomTrans(true, makJournalPtid);
						if (breakStrapCount > 0)
							MakeBreakDenomTrans(false, brkJournalPtid);
						#endregion
					}
					catch (PhoenixException pe)
					{
						PMessageBox.Show(pe);
					}
				}
			}
        }

        #region #8287
        private bool IsFirstRowBulkCoin()
        {
			//Begin #11462 - error when clicking on count button in vault drawer
			if (gridLooseCash.Count <= 0)
				return false;
			//End #11462

            bool firstRowBC = false;
            object objectValue = null;
            string strDenomType = "";

            objectValue = gridLooseCash.GetCellValueFormatted(0, colLooseDenomType.ColumnId);
            strDenomType = Convert.ToString(objectValue);
            if (strDenomType == mlBC)
                firstRowBC = true;
            //
            return firstRowBC;
        }
        #endregion
        #endregion

        #region #79314
        private void ResetFormForSupViewOnlyMode()
        {
            if (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled &&
                Phoenix.Windows.Client.Helper.IsWorkspaceReadOnly(this.Workspace))
            {
                MakeReadOnly(false);
                foreach (PAction action in ActionManager.Actions)
                {
                    if (!(action == ActionClose))
                        action.Enabled = false;
                }
            }
        }
        #endregion


        #endregion

        #region dlgTlCashCount_PMdiPrintEvent
        //71641 - I am leaving the code if she changes the mind...
		private void dlgTlCashCount_PMdiPrintEvent(object sender, EventArgs e)
		{
			//71440 - These window has heavy trafic and creating lot of trouble so I have moved the code back here...
			_adGbBranchBusObj = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBranch] as AdGbBranch);
			if (TellerVars.Instance.IsAppOnline)
			{
				_pdfFileManipulation = new PdfFileManipulation();
			}
			else
			{
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
					CoreService.LogPublisher.LogDebug("\n(frmtlPosotion window)For some reason creating of HtmlPrinter Failed." + ex.Message);
				}
			}
			string currentGridName = string.Empty;
			if (currGrid != null)
				currentGridName = currGrid.Name;
			else
			{
				GetCurrentGrid(1);
				currentGridName = currGrid.Name;
			}
            //Begin #20598
            if (sender != null && (sender as System.String[]) != null && (sender as System.String[])[0] == "Inventory")
                GenerateReport();   //#20598
            else if (sender != null && (sender as System.String[]) != null && (sender as System.String[])[0] == "Drawer")
                PrintDrawerCashCount(false, true);
            else
                return;
            //End #20598

			if (currentGridName == string.Empty)
				return;
			int gridId = -1;
			if (currentGridName == gridLooseCash.Name)
				gridId = 1;
			else if (currentGridName == gridStrappedCash.Name)
				gridId = 2;
			else if (currentGridName == gridOtherItems.Name)
				gridId = 3;

			if (gridId > 0)
				GetCurrentGrid(gridId);

		}

		#region PrintDrawerCashCount

        #region 79238 - Scrape the screen for new online snapshot report and display the snapshot report ...


        private bool IsBranchItemsCounted()
        {
            bool counted = false;

            if (rbBranch.Checked)
            {
                if (gridInventoryItems.Items.Count > 0)
                {
                    for (int i = 0; i <= gridInventoryItems.Items.Count - 1; i++)
                    {
                        gridInventoryItems.ContextRow = i;

                        if (colVerified.UnFormattedValue != null && colVerified.Checked)
                        {
                            counted = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (_tempTlInvItemsBranch != null && _tempTlInvItemsBranch.Count > 0)
                {
                    foreach (TlInventoryItemCount invItem in _tempTlInvItemsBranch)
                    {
                        if (!invItem.Verified.IsNull && invItem.Verified.Value == "Y")
                        {
                            counted = true;
                            break;
                        }
                    }
                }
            }
            return counted;
        }

        /// <summary>
        /// Assembles data from the grid for the SQR report, including start and end dates for the report
        /// </summary>
        /// <param name="startDate">out - returned to caller</param>
        /// <param name="endDate">out - returned to caller</param>
        /// <returns>Delimited string of the grid's contents</returns>
        private string ScrapeGrid()       //#20598
        {
            StringBuilder RptData = new StringBuilder();
            List<string> GridData = null;
            //this._snapShotRunning = true;   //#18917
            int itemCount = 0;
            bool appendNewLine = true;


            try
            {
                if (rbBranch.Checked || gridInventoryItems.Visible == false)
                {
                    if (_tempTlInvItemsDrawer != null && _tempTlInvItemsDrawer.Count > 0)
                    {
                        foreach (TlInventoryItemCount invItem in _tempTlInvItemsDrawer)
                        {
                            ++itemCount;
                            GridData = new List<string>();

                            // Blow the grid's columns into a List of String ...
                            GridData.Add(invItem.TypeIdDesc.Value);
                            GridData.Add((invItem.NoItems.IsNull ? "0" : invItem.NoItems.StringValue));
                            GridData.Add((invItem.CountNoItems.IsNull ? "NONE" : invItem.CountNoItems.StringValue));
                            GridData.Add((!invItem.Verified.IsNull && invItem.Verified.Value == "Y") ? "Y" : "N");
                            GridData.Add((invItem.Difference.IsNull ? "NONE" : invItem.Difference.StringValue));
                            GridData.Add("D");  //Identifier to seperate drawer items from branch items

                            // Join GridData's string array with "~" character ...
                            RptData.Append(string.Join("~", GridData.ToArray()));

                            // Add CR/LF to the end of the line if there are more grid rows ...
                            ////9913  moved this here as  it was leaving new line if last transaction in a grid  is reversed as a result reports show same item twice
                            if (itemCount != _tempTlInvItemsDrawer.Count)
                            {
                                RptData.Append(Environment.NewLine);
                            }
                        }
                    }
                }

                if (gridInventoryItems.Items.Count > 0)
                {
                    if ((rbDrawer.Checked || IsBranchItemsCounted()) && gridInventoryItems.Visible == true)
                    {
                        for (int i = 0; i <= gridInventoryItems.Items.Count - 1; i++)
                        {
                            gridInventoryItems.ContextRow = i;

                            if (i == 0 && appendNewLine && RptData.Length > 0)
                            {
                                RptData.Append(Environment.NewLine);
                                appendNewLine = false;
                            }

                            GridData = new List<string>();

                            // Blow the grid's columns into a List of String ...
                            GridData.Add(colTypeIdDesc.UnFormattedValue != null ? colTypeIdDesc.Text : string.Empty);
                            GridData.Add(colInvItemDefaultedCount.UnFormattedValue != null ? colInvItemDefaultedCount.UnFormattedValue.ToString() : "0");
                            GridData.Add(colInvItemCountedQty.UnFormattedValue != null ? colInvItemCountedQty.UnFormattedValue.ToString() : "NONE");
                            GridData.Add((colVerified.UnFormattedValue != null && colVerified.Checked) ? "Y" : "N");
                            GridData.Add(colDifference.UnFormattedValue != null ? colDifference.UnFormattedValue.ToString() : "NONE");
                            GridData.Add((rbDrawer.Checked ? "D" : "B"));  //Identifier to seperate drawer items from branch items

                            // Join GridData's string array with "~" character ...
                            RptData.Append(string.Join("~", GridData.ToArray()));

                            // Add CR/LF to the end of the line if there are more grid rows ...
                            ////9913  moved this here as  it was leaving new line if last transaction in a grid  is reversed as a result reports show same item twice
                            if (i != (gridInventoryItems.Items.Count - 1))
                            {
                                RptData.Append(Environment.NewLine);
                            }
                        }
                    }
                }
                itemCount = 0;
                appendNewLine = true;
                if (rbDrawer.Checked || gridInventoryItems.Visible == false)
                {
                    if (_tempTlInvItemsBranch != null && _tempTlInvItemsBranch.Count > 0)
                    {
                        if (IsBranchItemsCounted())
                        {
                            foreach (TlInventoryItemCount invItem in _tempTlInvItemsBranch)
                            {
                                if (appendNewLine && RptData.Length > 0)
                                {
                                    RptData.Append(Environment.NewLine);
                                    appendNewLine = false;
                                }
                                ++itemCount;
                                GridData = new List<string>();

                                // Blow the grid's columns into a List of String ...
                                GridData.Add(invItem.TypeIdDesc.Value);
                                GridData.Add((invItem.NoItems.IsNull ? "0" : invItem.NoItems.StringValue));
                                GridData.Add((invItem.CountNoItems.IsNull ? "NONE" : invItem.CountNoItems.StringValue));
                                GridData.Add((!invItem.Verified.IsNull && invItem.Verified.Value == "Y") ? "Y" : "N");
                                GridData.Add((invItem.Difference.IsNull ? "NONE" : invItem.Difference.StringValue));
                                GridData.Add("B");  //Identifier to seperate drawer items from branch items

                                // Join GridData's string array with "~" character ...
                                RptData.Append(string.Join("~", GridData.ToArray()));

                                // Add CR/LF to the end of the line if there are more grid rows ...
                                ////9913  moved this here as  it was leaving new line if last transaction in a grid  is reversed as a result reports show same item twice
                                if (itemCount != _tempTlInvItemsBranch.Count)
                                {
                                    RptData.Append(Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }

            catch (PhoenixException pEx)            //#18917
            {
                Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pEx.ToString());
                PMessageBox.Show(pEx);
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
            return RptData.ToString();
        }

        /// <summary>
        /// Sets up and displays our SQR report
        /// </summary>
        private void ShowOnlineSnapshot(string reportName)        //#20598
        {
            DateTime StartDate = GlobalVars.SystemDate;
            DateTime EndDate = GlobalVars.SystemDate;
            RunSqrReport SnapShot = new RunSqrReport();
            string tellerNoName = string.Empty;
            string branchNoName = string.Empty;
            //string closeoutPostingDt;
            //string closeoutPostingDateTime;
            branchNoName = (GlobalVars.CurrentBranchNo.ToString() + " - " + _adGbBranchBusObj.Name1.Value.Trim());
            tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;

            SnapShot.ReportName.Value = reportName;
            SnapShot.EmplId.Value = GlobalVars.EmployeeId;
            //SnapShot.RunDate.Value = GlobalVars.SystemDate;
            SnapShot.RunDate.Value = DateTime.Now;
            SnapShot.Param1.Value = branchNoName;
            SnapShot.Param2.Value = tellerNoName;
            SnapShot.Param3.Value = _tellerVars.PostingDt.ToShortDateString();
            SnapShot.Param4.Value = Convert.ToString(_drawerNo.Value);  //#21476
            SnapShot.MiscParams.Value = ScrapeGrid();

            // Set start and end dates ...
            SnapShot.FromDt.Value = StartDate;
            SnapShot.ToDt.Value = EndDate;

            try
            {
                _pdfFileManipulation = new PdfFileManipulation();

                DataService.Instance.ProcessRequest(XmActionType.Select, SnapShot);
                _pdfFileManipulation.ShowUrlPdf(SnapShot.OutputLink.Value);
            }
            catch (PhoenixException pEx)
            {
                Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pEx.ToString());
                PMessageBox.Show(pEx);
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        #endregion

        public void GenerateReport()      //#20598
        {
            string ReportName = null;
            string text1 = string.Empty;
            string depLoan = string.Empty;
            RunSqrReport report1 = new RunSqrReport();

            ReportName = "TLO11600.SQR";

            if (!string.IsNullOrEmpty(ReportName))
            {
                using (new WaitCursor())
                {
                    ShowOnlineSnapshot(ReportName);
                }
            }
            else
            {
                // #79238
                // TODO - Lose this once loan snap is implemented!!!
                // #704 - PMessageBox.Show(MessageType.Message, "Unknown report type specified.");
                PMessageBox.Show(13287, MessageType.Message, null);
            }

        }

		private void PrintDrawerCashCount(bool displayBrowser, bool riseMessage)
		{
			#region variables
			if (!TellerVars.Instance.IsAppOnline)
				_printerSetting.ChangePageOrientation(_printerSetting.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);
			string cashDrawer = string.Empty;
			string endingCash = string.Empty;
			string difference = string.Empty;
			string tellerNoName = string.Empty;
			string branchNoName = string.Empty;
			string sqrParam = string.Empty;
			string tempValue= string.Empty;
			Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
			myTempVar.Visible = false;
			myTempVar.TabStop = false;
			myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			string balDenomTracking = "N";
			string closeoutPostingDt;
			string closeoutPostingDateTime;
			//360724 - Generating Teller Cash Drawer Count [Drawer %1!]report.  Please wait...
			dlgInformation.Instance.ShowInfo(CoreService.Translation.GetTokenizeMessageX(360724, _drawerNo.StringValue));
			//
			branchNoName = (GlobalVars.CurrentBranchNo.ToString()  + " - " + _adGbBranchBusObj.Name1.Value.Trim());
			tellerNoName = _adGbRsm.TellerNo.StringValue + " - " + GlobalVars.EmployeeName;

			if (!isCashDrawerCount)
			{
				cashDrawer = dfTotalCash.Text;
				endingCash = dfTotalCashEntered.Text;
				difference = dfAmountRemaining.Text;
			}
			/* Begin #71885 */
			else
			{
				cashDrawer = dfCashDrawerCount.Text;
				endingCash = dfEndingCash.Text;
				difference = dfDifference.Text;
			}
			//closeoutPostingDt = _tellerVars.PostingDt.ToString("MM/dd/yyyy");
			//closeoutPostingDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
			closeoutPostingDt = "N/A";
			closeoutPostingDateTime = closeoutPostingDt;
			/* End #71885 */

			string sCurrencyCount;			// WI#21486
			double nCurencyCount;			// WI#21486
			bool bIsCurrencyCountValid;		// WI#21486
			#endregion variables

			try
			{
				if (TellerVars.Instance.IsAppOnline)
				{
					Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();
					report1.ReportName.Value = "TLO11500.sqr";
					report1.EmplId.Value = GlobalVars.EmployeeId;
					report1.FromDt.Value = GlobalVars.SystemDate;
					report1.ToDt.Value = GlobalVars.SystemDate;
                    report1.RunDate.Value = DateTime.Now;

					string drawerType = "T";
					//Object is created at the beginning
					drawerType = _tlDrawerBalances.GetDrawerType(_branchNo.Value, _drawerNo.Value);
					report1.Param1.Value = "BalDenoTracking=" + balDenomTracking.Trim() + "~BranchName=" + branchNoName + "~TellerName=" + tellerNoName + "~CloseoutPosDt=" + closeoutPostingDt + "~CloseOutTimeStamp=" + closeoutPostingDateTime + "~DrawerNo=" + _drawerNo.StringValue + "~CashDrawer=" + cashDrawer + "~Endingcash=" + endingCash + "~Difference=" + difference + "~IsVault=" + drawerType + "~";

					#region LB, LC, BC
					GetCurrentGrid(1);
					//If you change the index of these columns you are #!~@!~
					//colLooseDenomId(0),colLooseDenom(1),colLooseDenomType(2),colLooseDemonination(3),colLooseCountValue(4),colLooseQuantity(5),colLooseAmount(6)
					sqrParam = string.Empty;
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
                        //currGrid.ContextRow = i;
                        //structObj.DenomType.Trim() +  "=D=" + structObj.DenomId + "^M=" + structObj.Denom + "^C=" + structObj.CountValue.ToString() + "^Q=" + structObj.Quantity.ToString() + "^A=" + structObj.Amount.ToString() + "~");
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //sqrParam = sqrParam + currGrid.Items[i].SubItems[2].Text.Trim() +
							//"=D=" + currGrid.Items[i].SubItems[0].Text.Trim() +
							//"^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[1].Text.Trim());  // #71885 - Replaced SubItems[3] by SubItems[1] and added CurrencyHelper.GetUnformattedValue

                        sqrParam = sqrParam + currGrid.Items[i].SubItems[currGrid.Columns["DenomType"].ColumnId].Text.Trim() +
                            "=D=" + currGrid.Items[i].SubItems[currGrid.Columns["DenomId"].ColumnId].Text.Trim() +
                            "^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());
                        //End Task#99209
                        //
                        // #10660/10661 - Change this SubItem index to 3 ...
                        //if (currGrid.Items[i].SubItems[4].Text != null && currGrid.Items[i].SubItems[4].Text.Length > 0)
                        //    sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[4].Text);

                        //sCurrencyCount = currGrid.Items[i].SubItems[4].Text;	// WI#21486	-  add variable, and change from 3 back to 4 //Task#99209
                        sCurrencyCount = currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text; //Task#99209

                        if (!String.IsNullOrEmpty(sCurrencyCount))	// WI#21486 - replace !=NULL and Length>0 with IsNullOrEmpty
						{
							bIsCurrencyCountValid = double.TryParse(sCurrencyCount, out nCurencyCount);	// WI#21486

							// WI#21486 - use value only if it is a valid number, else use 0
							if (bIsCurrencyCountValid)
								sqrParam = sqrParam + "^C=" + sCurrencyCount;
							else
								sqrParam = sqrParam + "^C=" + "0";
						}
						else
							sqrParam = sqrParam + "^C=" + "0";
                        //
                        // #10660/10661 - Change this SubItem index to 7 ...
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
                        //    sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[5].Text;		// #71885 - Removed CurrencyHelper.GetUnformattedValue
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //if (currGrid.Items[i].SubItems[7].Text != null && currGrid.Items[i].SubItems[7].Text.Length > 0)
                        //sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[7].Text;       // #71885 - Removed CurrencyHelper.GetUnformattedValue
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text;
                        //End Task#99209
                        else
                            sqrParam = sqrParam + "^Q=" + "0";
                        //
                        // #10660/10661 - Change this SubItem index to 8 ...
                        //if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
                        //    sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[6].Text) + "~";
                        //Begin Task#99209
                        //if (currGrid.Items[i].SubItems[8].Text != null && currGrid.Items[i].SubItems[8].Text.Length > 0)
                        //sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[8].Text) + "~";
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text) + "~";
                        //End Task#99209
                        else
                            sqrParam = sqrParam + "^A=" + "0~";
					}
					report1.Param2.Value = sqrParam;
					#endregion
					//
					//
					#region SB, SR
					sqrParam = string.Empty;
					GetCurrentGrid(2);
					//If you change the index of these columns you are #!~@!~
					//colStrapDenomId(0),colStrapDenom(1),colStrapDenomType(2),colStrapOrigRollValue(3),colStrapDenomination(4),colStrapRollValue(5),colStrapQuantity(6),colStrapAmount(7)
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
                        //currGrid.ContextRow = i;
                        //structObj.DenomType.Trim() +  "=D=" + structObj.DenomId + "^M=" + structObj.Denom + "^C=" + structObj.CountValue.ToString() + "^Q=" + structObj.Quantity.ToString() + "^A=" + structObj.Amount.ToString() + "~");
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //sqrParam = sqrParam + currGrid.Items[i].SubItems[2].Text.Trim() +
                        //"=D=" + currGrid.Items[i].SubItems[0].Text.Trim() +
                        //"^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[1].Text.Trim());  // #71885 - Replaced SubItems[4] by SubItems[1] and added CurrencyHelper.GetUnformattedValue
                        sqrParam = sqrParam + currGrid.Items[i].SubItems[currGrid.Columns["DenomType"].ColumnId].Text.Trim() +
                            "=D=" + currGrid.Items[i].SubItems[currGrid.Columns["DenomId"].ColumnId].Text.Trim() +
                            "^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());
                        //
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
							//sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[5].Text);
                        if (currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text);
                        else
							sqrParam = sqrParam + "^C=" + "0";
						//
						//if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
							//sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[6].Text;		// #71885 - Removed CurrencyHelper.GetUnformattedValue
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text;
                        else
							sqrParam = sqrParam + "^Q=" + "0";
						//
						//if (currGrid.Items[i].SubItems[7].Text != null && currGrid.Items[i].SubItems[7].Text.Length > 0)
							//sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[7].Text) + "~";
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text) + "~";
                        //End Task#99209
                        else
                            sqrParam = sqrParam + "^A=" + "0~";
					}
					report1.Param3.Value = sqrParam;
					#endregion

					#region OC

					sqrParam = string.Empty;
					GetCurrentGrid(3);
					//If you change the index of these columns you are #!~@!~
					//colOtherDenomId(0),colOtherDenom(1),colOtherDenomType(2),colOtherOrigCashItemValue(3),colDescription(4),colCashItemValue(5),colOtherQuantity(6),colOtherAmount(7)
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
                        //currGrid.ContextRow = i;
                        //structObj.DenomType.Trim() +  "=D=" + structObj.DenomId + "^M=" + structObj.Denom + "^C=" + structObj.CountValue.ToString() + "^Q=" + structObj.Quantity.ToString() + "^A=" + structObj.Amount.ToString() + "~");
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //sqrParam = sqrParam + currGrid.Items[i].SubItems[2].Text.Trim() +
                        //"=D=" + currGrid.Items[i].SubItems[0].Text.Trim() +
                        //"^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[1].Text.Trim());  // #71885 - Replaced SubItems[4] by SubItems[1] and added CurrencyHelper.GetUnformattedValue
                        sqrParam = sqrParam + currGrid.Items[i].SubItems[currGrid.Columns["Denom Type"].ColumnId].Text.Trim() +
                            "=D=" + currGrid.Items[i].SubItems[currGrid.Columns["DenomId"].ColumnId].Text.Trim() +
                            "^M=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());
                        //
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
							//sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[5].Text);
                        if (currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text);
                        else
							sqrParam = sqrParam + "^C=" + "0";
						//
						//if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
							//sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[6].Text;       // #71885 - Removed CurrencyHelper.GetUnformattedValue
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^Q=" + currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text;
                        else
							sqrParam = sqrParam + "^Q=" + "0";
						//
						//if (currGrid.Items[i].SubItems[7].Text != null && currGrid.Items[i].SubItems[7].Text.Length > 0)
							//sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[7].Text) + "~";
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text) + "~";
                        //End Task#99209
                        else
                            sqrParam = sqrParam + "^A=" + "0~";
					}
					/* Begin #71885 */
					report1.Param3.Value = report1.Param3.Value + sqrParam;
					//report1.Param4.Value = sqrParam;
					/* End #71885 */
					#endregion

					#region Final Print
					sqrParam = string.Empty;
					report1.Param5.Value = sqrParam;
					report1.Param6.Value = sqrParam;
					report1.MiscParams.Value = sqrParam;
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
						//_printProcess = _htmlPrinter.PrintPDFFromUrl(report1.OutputLink.Value, false);
						_printProcess = _pdfFileManipulation.PrintPDFFromUrl(report1.OutputLink.Value, false);
					else
						//_htmlPrinter.ShowUrlPdf(report1.OutputLink.Value);
						_pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
					#endregion

				} //End of Online Report
				else
				{
					#region Variables
					StringBuilder htmlPage1  = new StringBuilder();
					string p1part1Html = string.Empty;
					string p1part2Html = string.Empty;
					string p1fixedHeaderTable = string.Empty;
					string p1cashEquationTable = string.Empty;
					string p1detailHeaderTable = string.Empty;
					string p1repeatDetailsTable = string.Empty;
					string p1subDetailHeader = string.Empty;
					string p1reportEndTable = string.Empty;
					string fixedHeader = string.Empty;
					string subHeader = p1subDetailHeader;
					string detailHeaderP1 = p1detailHeaderTable;
					string desc = string.Empty;
					string HTMLPagesPage1 = string.Empty;
					string subTitle = string.Empty;
					string detailRow = string.Empty;
					#endregion

					#region Validate Template
					if (!_tlDrawerBalances.ValidateFileTLF115(1)) //Page Number 1
					{
						//File Not Found Exception
						if (riseMessage)
						{
							dlgInformation.Instance.HideInfo();
							PMessageBox.Show(360738, MessageType.Warning, MessageBoxButtons.OK, Shared.Constants.UserConstants.TLF11500P1);
							//360738 - Offline Teller Position template %1! is not found.  Please contact support to rectify the problem.
						}
						if (CoreService.LogPublisher.IsLogEnabled)
							CoreService.LogPublisher.LogDebug(CoreService.Translation.GetTokenizeMessageX(360738, Shared.Constants.UserConstants.TLF11500P1));
						return;
					}
					_tlDrawerBalances.ExtractTablesFromHTML115Page1(out p1part1Html, out p1part2Html, out p1fixedHeaderTable, out p1cashEquationTable, out p1detailHeaderTable,
						out p1repeatDetailsTable, out p1subDetailHeader,  out p1reportEndTable);
					#endregion

					#region Build The Page 1 & 2 Header
					htmlPage1.Append(p1part1Html + Environment.NewLine);
					fixedHeader = p1fixedHeaderTable;
					fixedHeader = fixedHeader.Replace("phx_ReportNo", "TLO11500");
					fixedHeader = fixedHeader.Replace("phx_ReportName", "TELLER CASH DRAWER COUNT [Drawer #" + _drawerNo.StringValue + " ]");
					fixedHeader = fixedHeader.Replace("phx_ReportDt", GlobalVars.SystemDate.ToString("MM/dd/yyyy"));
					fixedHeader = fixedHeader.Replace("phx_RunDateTime", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
					fixedHeader = fixedHeader.Replace("phx_branchName", branchNoName);
					fixedHeader = fixedHeader.Replace("phx_CloseOutPdt", closeoutPostingDt);
					fixedHeader = fixedHeader.Replace("phx_TellerNoName", tellerNoName);
					fixedHeader = fixedHeader.Replace("phx_1CloseOutPdtTime", closeoutPostingDateTime); //1 for replacement to Work
					htmlPage1.Append(fixedHeader + Environment.NewLine);
					#endregion

					//
					#region Buld cashEquation
					string cashEquation = p1cashEquationTable;
					cashEquation = cashEquation.Replace("phx_d_CashDrawer", cashDrawer);
					cashEquation = cashEquation.Replace("phx_d_EndingCash", endingCash);
					cashEquation = cashEquation.Replace("phx_d_Difference", difference);
					htmlPage1.Append(cashEquation);
					//
					htmlPage1.Append(p1detailHeaderTable + Environment.NewLine);

					#endregion
					//
					#region LB, LC and BC
					GetCurrentGrid(1);
					//If you change the index of these columns you are #!~@!~
					//colLooseDenomId(0),colLooseDenom(1),colLooseDenomType(2),colLooseDemonination(3),colLooseCountValue(4),colLooseQuantity(5),colLooseAmount(6)
					sqrParam = string.Empty;
					subTitle = p1subDetailHeader.Replace("phx_SubTitle", "Loose Bills and Loose Coin");
					htmlPage1.Append(subTitle + Environment.NewLine);
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
						#region CashCount Record
						detailRow = p1repeatDetailsTable;
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //desc = currGrid.Items[i].SubItems[3].Text.Trim();
                        desc = currGrid.Items[i].SubItems[currGrid.Columns["CashDenomDesc"].ColumnId].Text.Trim();
                        //End Task#99209
                        if (desc.Length > 25)
							desc = desc.Substring(0, 25);

						detailRow = detailRow.Replace("phx_Description", desc);
                        //Begin Task#99209
                        //detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[1].Text.Trim());
                        detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());
                        //End Task#99209
                        //
                        //71509 - Later on Addition
                        detailRow = detailRow.Replace("phx_CountValue", string.Empty);
                        //if (currGrid.Items[i].SubItems[4].Text != null && currGrid.Items[i].SubItems[4].Text.Length > 0)
                        //detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(currGrid.Items[i].SubItems[4].Text));
                        //else
                        //	detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat("0"));
                        //
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
                        //detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[5].Text);
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text);
                        else
							detailRow = detailRow.Replace("phx_Quantity", "0");

						//if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[6].Text );
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text);
                        //End Task#99209
                        else
                            detailRow = detailRow.Replace("phx_amount", "$0.00");
						htmlPage1.Append(detailRow + Environment.NewLine);
						#endregion CashCount Record
					}
					#endregion
					//
					#region SB, SR
					GetCurrentGrid(2);
					//If you change the index of these columns you are #!~@!~
					//colStrapDenomId(0),colStrapDenom(1),colStrapDenomType(2),colStrapOrigRollValue(3),colStrapDenomination(4),colStrapRollValue(5),colStrapQuantity(6),colStrapAmount(7)
					subTitle = p1subDetailHeader.Replace("phx_SubTitle", "Strapped Bills and Rolled Coin");
					htmlPage1.Append(subTitle + Environment.NewLine);
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
						#region CashCount Record
						detailRow = p1repeatDetailsTable;
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //desc = currGrid.Items[i].SubItems[4].Text.Trim();
                        desc = currGrid.Items[i].SubItems[currGrid.Columns["CashDenomDesc"].ColumnId].Text.Trim();
                        //End Task#99209
                        if (desc.Length > 25)
							desc = desc.Substring(0, 25);

						detailRow = detailRow.Replace("phx_Description", desc);
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[1].Text.Trim());
                        detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());
                        //
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(currGrid.Items[i].SubItems[5].Text));
                        if (currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text));
                        else
							detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat("0"));
						//

						//if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[6].Text);      // #71885 - Removed GetDecimalFormat
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text);
                        else
							detailRow = detailRow.Replace("phx_Quantity", "0");					// #71885 - Removed GetDecimalFormat

						//if (currGrid.Items[i].SubItems[7].Text != null && currGrid.Items[i].SubItems[7].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[7].Text );
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text);
                        //End Task#99209
                        else
                            detailRow = detailRow.Replace("phx_amount", "$0.00");
						htmlPage1.Append(detailRow + Environment.NewLine);
						#endregion CashCount Record
					}
					#endregion
					//
					#region OC
					GetCurrentGrid(3);
					//If you change the index of these columns you are #!~@!~
					//colOtherDenomId(0),colOtherDenom(1),colOtherDenomType(2),colOtherOrigCashItemValue(3),colDescription(4),colCashItemValue(5),colOtherQuantity(6),colOtherAmount(7)
					if  (currGrid.Items.Count > 0)
					{
						subTitle = p1subDetailHeader.Replace("phx_SubTitle", "Other Cash Items");
						htmlPage1.Append(subTitle + Environment.NewLine);
					}
					for(int i = 0; i < currGrid.Items.Count; i++)
					{
						#region CashCount Record
						detailRow = p1repeatDetailsTable;
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //desc = currGrid.Items[i].SubItems[4].Text.Trim();
                        desc = currGrid.Items[i].SubItems[currGrid.Columns["CashDenomDesc"].ColumnId].Text.Trim();
                        //End Task#99209
                        if (desc.Length > 25)
							desc = desc.Substring(0, 25);

						detailRow = detailRow.Replace("phx_Description", desc);
                        //Begin Task#99209 updated the subitem Index to select using ColumnId
                        //detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[1].Text.Trim());
                        detailRow = detailRow.Replace("phx_Denom", currGrid.Items[i].SubItems[currGrid.Columns["Denom"].ColumnId].Text.Trim());

                        //
                        //if (currGrid.Items[i].SubItems[5].Text != null && currGrid.Items[i].SubItems[5].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(currGrid.Items[i].SubItems[5].Text));
                        if (currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(currGrid.Items[i].SubItems[currGrid.Columns["CountValue"].ColumnId].Text));
                        else
							detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat("0"));
						//

						//if (currGrid.Items[i].SubItems[6].Text != null && currGrid.Items[i].SubItems[6].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[6].Text);	// #71885 - Removed GetDecimalFormat
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_Quantity", currGrid.Items[i].SubItems[currGrid.Columns["Quantity"].ColumnId].Text);
                        else
							detailRow = detailRow.Replace("phx_Quantity", "0");									// #71885 - Removed GetDecimalFormat

						//if (currGrid.Items[i].SubItems[7].Text != null && currGrid.Items[i].SubItems[7].Text.Length > 0)
							//detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[7].Text );
                        if (currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text != null && currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text.Length > 0)
                            detailRow = detailRow.Replace("phx_amount", currGrid.Items[i].SubItems[currGrid.Columns["Amt"].ColumnId].Text);
                        //End Task#99209
                        else
                            detailRow = detailRow.Replace("phx_amount", "$0.00");
						htmlPage1.Append(detailRow + Environment.NewLine);
						#endregion CashCount Record
					}
					#endregion
					//
					HTMLPagesPage1 = htmlPage1.ToString() +  Environment.NewLine + p1reportEndTable  + Environment.NewLine + p1part2Html + Environment.NewLine;
					if (CoreService.LogPublisher.IsLogEnabled)
						CoreService.LogPublisher.LogDebug("\n" + HTMLPagesPage1 + "\n");
					// Print this Junk
					_htmlPrinter.PrintHtml(HTMLPagesPage1, false);
					//
				} //End of Offline Report
			}
			catch(Exception ex)
			{
				dlgInformation.Instance.HideInfo();
				if (riseMessage)
					PMessageBox.Show(360726, MessageType.Warning, MessageBoxButtons.OK, ex.Message);
				//360726 - Failed to generate Teller Cash Drawer Count report.
			}
			finally
			{
				if (!riseMessage && _printProcess != null)
				{
					try
					{
						_printProcess.Kill();
					}
					finally
					{
						_printProcess.Dispose();
					}
				}
				dlgInformation.Instance.HideInfo();
                //73125
                //if (!TellerVars.Instance.IsAppOnline  && _printerSetting != null)
                //    _printerSetting.ChangePageOrientation(_printerSetting.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
			}
		}
		#endregion PrintDrawerCashCount

		#region DecimalFormatter
		private string GetDecimalFormat(decimal decimalValue, string mask)
		{
			if (decimalValue == Decimal.MinValue || decimalValue == Decimal.MaxValue)
				return Convert.ToDecimal("0").ToString(mask);
			return decimalValue.ToString(mask);
		}
		private string GetDecimalFormat(decimal decimalValue)
		{
			if (decimalValue == Decimal.MinValue || decimalValue == Decimal.MaxValue)
				return "0.00";
			return decimalValue.ToString("#,##0.00");
		}
		private string GetDecimalFormat(string decimalValue)
		{
			decimal localValue = 0;
			if (decimalValue == null)
				return "";
			if (decimalValue.Length == 0)
				return "";
			if (decimalValue.IndexOf("$") >= 0)
				return decimalValue;
			try
			{
				localValue = Convert.ToDecimal(decimalValue);
			}
			catch(System.InvalidCastException)
			{
				//Invalid Value
				return "";
			}
			if (localValue == Decimal.MinValue || localValue == Decimal.MaxValue)
				return "0.00";
			return localValue.ToString("#,##0.00");
		}
		#endregion

		#endregion dlgTlCashCount_PMdiPrintEvent
	}

}
