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
// File Name: frmTlCapturedItems.cs
// NameSpace: Phoenix.Client.TlCapturedItems
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//6/6/2006		1		mselvaga	Issue#67873 - Created.
//12/14/2006	2		mselvaga	#71135 - Added value for Dup wosa tag since its the time of reprint.
//01/17/2007	3		mselvaga	#71452 - Added view only check for end of row validation.
//01/20/2007	4		mselvaga	#71490 - Fixed the duplicate trancode list.
//01/24/2007	5		mselvaga	#71553 - Fixed lastprintline param value for item capture reprint all.
//02/20/2007	6		rpoddar		#71873 - Commented the closing of XfsPrinter.
//03/14/2007	7		rpoddar		#71741 - Reset print info
//03/23/2007	8		mselvaga	#72095 - Receiving Application error when selecting Remove action to remove the last row left in Captured Items window.
//04/11/2007	9		rpoddar		#72362 - Set the global print information.
//May-24-07		10   	Muthu       #72780 VS2005 Migration
//09/27/2007    11      mselvaga    #73972 - Fixed the item count populate based on the row selected.
//01/03/2008    12      JStainth    #74015 - Multiple signature notification
//01/14/2008    13      mselvaga    #74448 - Added fix for acct type combo box column not firing validation after the window closed and re-opened problem.
//01/22/2008    14      JStainth    #74488 - Title1 and Title 2 should display in view only mode
//02/26/2008    15      JStainth    #74778 - Tfr Acct rim restrictions
//02/28/2008    16      mselvaga    #74777 - Teller2007 - Close out of and reopen Captured Items window causes last entry to be listed again.
//03/03/2008    17      mselvaga    #74784 - Teller2007 - # of Items is incorrectly listed on the Captured Items window when it is reopened
//03/14/2008    18      mselvaga    #75325 - The first item capture transaction disappear after posting due to incorrect index starting number used in IsFormDirty.
//03/26/2008    19      JStainth    #74744 - Null reference error in SQL Server
//04/01/2008    20      mselvaga    #75557 - Teller2007 - Receiving PE #208 when entering On-Us items in a deposit posted in offline mode.
//03/26/2008    21      JStainth    #75709 - Prevent offline errors
//04/25/2008    22      bhughes     #76137 - Teller2007 - # Signatures Required is incorrect when viewing item capture window from the teller journal
//04/30/2008    23      mselvaga    #76195 - Teller2007 - Check Types are changing on Captured Items window although window was saved prior to closing it.
//06/12/2008    24      jstainth    #76048 - Search by Member
//10/10/2008    25      jstainth    #1395 - Offline error
//11/07/2008    26      mselvaga    #1661 - Fixed GetNameRim for null validation.
//11/07/2008    27      iezikeanyi  #76429 - Added code to trap incoming acct  no during cross ref. swap
//12/12/2008    28      mselvaga    #2029 - Get error when viewing item capture items from journal.
//12/26/2008    29      mselvaga    #76458 - Added EX account changes.
//02/03/2009	30	    SDighe	    #77173 - Item Capture amount calculated incorrectly when using the mouse and selecting the bank code on the item capture window
//02/20/2009    31      mselvaga    #2677 - Fixed accttype filter.
//04/02/2009    32      LSimpson    #2584 - Set rows in header to 3 for gridCapturedItems.
//04/16/2009    33      mselvaga    WI - 2970 - Set the SelectFirstControl flag to false, to make sure the focus is maintained by the window
//04/24/2009    34      mramalin    WI-3475 - Terminal Services Printing Enhancement
//05/04/2009    35      LSimpson    #2690 - Set cursor focus on bank code column.
//05/21/2009    36      mselvaga    WI#3714 - Capture window not allowing trancode delete.
//05/29/2009    37      rpoddar     #01285 - Called method TlAcctDetails.GetRimAndJointNames
//06/01/2009	38		njoshi		#4168	Warning Message in Teller / After PTF7, Error 314456 - when pressing F6 to bring up the calculator on the item capture window
//06/14/2009    39      LSimpson    #4050 - Added external accounts to onus member name functionality.
//06/15/2009    40      Ashok       #3624 - Changed the Drop Down Style (New Prop) to DropDown to maintain existing functionality.
//06/20/2009    41      iezikeanyi  #4450 - Hide Cross Ref Accounts when Cross Ref is not enabled
//06/22/2009    42      mselvaga    WI#4724 - Captured Items window (10480 - frmTlCapturedItems) highlights wrong row when entering items.
//06/22/2009    43      mselvaga    WI#4694 - SD Low-Unable to add Acct type & #.
//06/29/2009    44      iezikeanyi  #2224 - Added code to ensure that colIncomingAcctNo is properly populated
// 15Jul2009	45		GDiNatale	#5020 - Make sure we get titles after using the Get Acct button ...
//07/21/2009    46      rpoddar     #05081 - Fixed the GL Security Screen Id
//07/30/2009    47      rpoddar     #05144 - Fix the ClearRow method
// 06Nov2009	48		GDiNatale	#6615 - SetValue Framework change
// 04/06/2010   49      iezikeanyi  #6917 - Reset the mask so user could enter incoming account number for cross ref
//10/21/2010    50      LSimpson    #80620 - Teller PopUps.
//11/08/2010    51      LSimpson    #11300 - Set ShowNotes to true if notes available.
//12/03/2010    52      LSimpson    #11412 - Popup modifications.
//01/11/2011    53      LSimpson    #11300 - Corrected core issue with colDepLoan not being populated at times and also added
//                                           Shown event to correct core issue with alerts not handled correctly for the first row.
//01/21/2011    54      mselvaga    #79314 - Added remote supervusir changes.
//02/04/2011    55      LSimpson    #12725 - Corrected error received due to null value.
//03/21/2011    56      mselvaga    Enh#80618 - Added enhance item capture window behavior changes.
//03/25/2011    57      mselvaga    #13192 - Receiving application error in offline teller journal when captured items window contains on-us item.
//04/11/2011    58      rpoddar     #79420 - Float Changes
//04/27/2011    59      rpoddar     #79420, #13851 - Float Changes
//05/20/2011    60      mselvaga    #13770 - Item Capture window displays and posts duplicate transactions..
//06/24/2011    61      DEIland     WI#13780/#13781 - colAcctNo allows GL posting so we must enforce GL Access restrictions if turned on
//06/27/2011    62      rpoddar     #14448 - Fixes
//07/08/2011	63		jrhyne		WI#14665 - formatting and layout issues
//07/11/2011    64      rpoddar     #14649 - Float Fixes
//07/14/2011	65		jrhyne		WI#14665 (2) - gridCapturedItems_SelectedIndexChanged was not called if only one row was present (no change), so replaced SelectRow with a direct call to this method. Also added code to focus on the grid.
//07/18/2011    66      LSimpson    #14776 - Changed colExceptRsnCode data type to text to correct justification issue.
//07/21/2011    67      rpoddar     #14789 - Focussing issues
//07/25/2011    68      rpoddar     #14865 - Float refresh issues with the + key being used
//08/01/2011    69      rpoddar     #14912 - Enable float button only for realtime.
//09/09/2011    70      sdhamija	#15431 - moved disclaimer (label) to groupbox, UI only changes.
//12/11/2011    71      rpoddar     #79420, #15357 - Float exception code changes
//12/06/2011    72      LSimpson    #80660 - Suspicious Transaction Scoring and Alerts modifications.
//12/20/2011    73      rpoddar     #79420, #15071 - Process Deposit Float Only Changes
//02/07/2012    74      rpoddar     #16656 - Receiving PW 300047 on Captured Items window and should not be
//03/01/2012    75      mselvaga    #16995 - Receiving error messages when viewing the Captured Items window via Teller Journal.
//03/05/2012    76      LSimpson    #17068 - If isViewOnly, pass params from columns to frmGbStopHold.
//03/22/2012    77      LSimpson    #17290 - Load colAcctType.CodeValue from colAcctType.Text if null and text is not.
//05/17/2012    78      rpoddar     #17924 - Fixed the rowcount when row is removed.
//09/18/2012	79		rpoddar		#19415 - Performance Fixes
//8/3/2013		80		apitava		#157637 uses new xfs printer
//10/04/2013    81      rpoddar     #140895 - Teller Capture Integration.
//09/22/2014    82      rpoddar     #161243, #30710 - Flaot fixes
//02/02/2015    83      BSchlottman #34401 Add Primary Rim Name
//09/02/2014    84      mselvaga    #30969 - #140895 - AVTC Part I changes added.
//05/06/2015    85      mselvaga    #33455 - Teller Capture 2014 Standardization - push button( Suspt Dtl) from  Item Capture Window - Phoenix Error #20.
//06/09/2015    86      mselvaga    #37398 - Information entered in item capture is not always accurate.
//11/25/2015    87      mselvaga    #39946 - EHF - Unable to post transactions in offline - log into offline mode and click post results in error
//02/10/2015    88      SChacko     Task#41151 - Item capture won't allow to enter more than 255 items in the number of items column.
//03/14/2016    89      BSchlottman #42006 - If the available remaining is negative, display it as a positive value.
//05/30/2017    90      SVasu       #65459 - CVT Issue 115816 - Dereference after null check
//11/2/2017     91      RDeepthi    WI#75604. Teller Window. So always refer Decimal Config
//05/25/2018    92      Kiran       #90669 - Added REQUIRE_TELLER_FRAUD_DETAILS validation
//11/03/2018    93      RDeepthi    ##95650. Pass Enable_real_time flag to the method which calcualte next day avail balance. From Item Capture we can not get Posting method without this.
//11/27/2018    94      mselvaga    Task#102719 - Add checks only AVTC transaction.
//01/04/2019    95      mselvaga    Task#105867 - CVT
//04/05/2019    96      DEiland     Task#112379 - Fix issue with AVTC getting Object Null Reference when only one Note was displayed and new framework changes for memory issues have changed cleared window behavior
//09/05/2019    97      rpoddar     #118850 - Teller WF Changes ( tagged by  #TellerWF )
//03/06/2020    98      Sandeep.S   #96359 - HF - Title/Description of GL Accts do not appear in the Item Capture window and Input Tran window
//11/25/2020    99      Akhil V     #134431 - change limit of NoItems
//-------------------------------------------------------------------------------
#endregion
//   //         //WARNING: Any new column change to the grid should be addressed to RearrangeColumns() method.
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using GlacialComponents.Controls;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Windows.Client;
using GlacialComponents.Controls.Common;
using Phoenix.BusObj.Teller;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Xfs;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Admin.RIM;
using Phoenix.BusObj.Control;		//#9519
using Phoenix.BusObj.GL;        //WI#13780
using System.Collections.Generic;

namespace Phoenix.Client.TlCapturedItems
{
	/// <summary>
	/// Summary description for frmTlCapturedItems.
	/// </summary>
	public class frmTlCapturedItems : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbAcceleratedInput;
		private Phoenix.Windows.Forms.PLabelStandard lblObjectInput;
		private Phoenix.Windows.Forms.PdfStandard dfObjectInput;
		private Phoenix.Windows.Forms.PdfStandard dfAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colIndex;
		private Phoenix.Windows.Forms.PGridColumn colBankCode;
		private Phoenix.Windows.Forms.PGridColumn colRoutingNo;
        private PGridColumn colSuspectPtid;
		private Phoenix.Windows.Forms.PGridColumnComboBox colCheckType;
		private Phoenix.Windows.Forms.PGridColumnComboBox colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colCheckNo;
		private Phoenix.Windows.Forms.PGridColumnComboBox colTranCode;
		private Phoenix.Windows.Forms.PGridColumn colAmount;
		private Phoenix.Windows.Forms.PAction pbRemove;
		private Phoenix.Windows.Forms.PAction pbCopy;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
		private Phoenix.Windows.Forms.PGrid gridCapturedItems;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalChecksAsCash;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalChecksAsCash;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalOnUsChecks;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalOnUsChecks;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalTransitChecks;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalTransitChecks;
		private Phoenix.Windows.Forms.PAction pbNewRow;
		private Phoenix.Windows.Forms.PGridColumn colNoOfItems;
		private Phoenix.Windows.Forms.PLabelStandard lblNoOfItems;
		private Phoenix.Windows.Forms.PDfDisplay dfNoOfItems;

        private AdGbRsmFailedAccess _adGbRsmFailedAccess = null; //74778
        private AdGbBankControl _adGbBankControl = (AdGbBankControl)GlobalObjects.Instance[Phoenix.Shared.Variables.GlobalObjectNames.AdGbBankControl];   //#76048
		private PcCustomOptions _pcCustomOptions = new PcCustomOptions();//#9519
        private bool _isFraudPreventionEnabled = false;	//#9519

		#region Initialize
		private TellerVars _tellerVars = TellerVars.Instance;
		private TlItemCapture _busObjCapturedItems = new TlItemCapture();
        //private TlItemCapture _tempObj;
		private GbHelper _gbHelper = new GbHelper();
		private TlTransactionSet _tlTranSet = null;
		private TlJournal _tlJournal;
		private TlHelper _tellerHelper = new TlHelper();
		private EnumValueCollection acctEnumCol = new EnumValueCollection();
		//
		private PSmallInt branchNo = new PSmallInt("BranchNo");
		private PSmallInt drawerNo = new PSmallInt("DrawerNo");
		private PSmallInt sequenceNo = new PSmallInt("SequenceNo");
		private PDateTime effectiveDt = new PDateTime("EffectiveDt");
		private PSmallInt subSequence = new PSmallInt("SubSequence");
		private PDecimal journalPtid = new PDecimal("JournalPtid");
		//
		private PSmallInt _reprintFormId;
		private string _reprintInfo = "";
		private string _checkItemInfo = "";
		private string _tempChkAmount = "";
		private string _reprintTextQrp = "";
		private string _partialPrintString = "";
		private string _wosaServiceName = "";
		//private string _logicalService = "";
		private string _formName = "";
		private string _mediaName = "";
		private int	_checkInfoRimNo = 0;
		private short _checkPrintNo = 0;
		private PrintInfo _wosaPrintInfo = new PrintInfo();
		private XfsPrinter _xfsPrinter;
		private DialogResult dialogResult = DialogResult.None;
		private string gLSearchAcctNo = null;
		private string _searchAcctNo = null;
		private string _searchAcctType = null;
        private bool _bumpedRowCountWidth = false;  //#80618

        //
        private ArrayList origItems = new ArrayList();  //#80618
        private bool _isSaveTriggered = false;    //#80618

		//
		private ArrayList enableList = new ArrayList();
		private ArrayList disableList = new ArrayList();
        private ArrayList items = new ArrayList();
		//
		private bool isViewOnly = false;
		//
		private string mlOnUs = CoreService.Translation.GetListItemX( ListId.CheckType, "O" );
		private string mlChksAsCash = CoreService.Translation.GetListItemX( ListId.CheckType, "C" );
		private string mlTransit = CoreService.Translation.GetListItemX( ListId.CheckType, "T" );
		private string returnCodeDesc = "";
		//
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PDfDisplay dfItemNo;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbItemTotals;
		private Phoenix.Windows.Forms.PGridColumn colAcctApplType;
		private Phoenix.Windows.Forms.PGridColumn colDepLoan;
		private Phoenix.Windows.Forms.PAction pbGetAcct;
		private Phoenix.Windows.Forms.PAction pbGetGlAcct;
		private Phoenix.Windows.Forms.PAction pbReprint;
		private Phoenix.Windows.Forms.PAction pbReprintAll;
		private Phoenix.Windows.Forms.PGridColumn colRowValidated;
		private Phoenix.Windows.Forms.PGridColumn colItemNo;
		private Phoenix.Windows.Forms.PGridColumn colTranEffectiveDt;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		//
		private string _focusField = null;
		private string _validAcctTypes = "";
        private PGroupBoxStandard gbOnUsAcctInformation;
        private PLabelStandard lblTitle2;
        private PLabelStandard lblTitle1;
        private PDfDisplay dfOnUsSignatures;
        private PDfDisplay dfOnUsTitle2;
        private PDfDisplay dfOnUsTitle1;
        private PAction pbDisplay;
        private PAction pbSuspectDtls; // #80660
        private PAction pbSignature;
		private bool _tcListLoaded = false;
		private bool _isGridLoaded = false;
        private bool _IsFromSelectRow = false;
        private PGridColumn colTitle1;
        private PGridColumn colTitle2;
        private PGridColumn colNoSignatures;
        private PGridColumn colRimNo;
        private PGridColumn colAcctId;
        private PGridColumn colDepType;
        private PGridColumn colTitle3;
        private PGridColumn colRimRestricted;
        private bool _IsForceContextRow = false; //Selva
        private AdRmRestrict _adRmRestrict;
        private PGridColumn colJointRimFirstName;
        private PGridColumn colJointRimMiddleInitial;
        private PGridColumn colJointRimLastName;
        private PGridColumn colJointRimNo;
        private PGridColumn colRimFirstName;
        private PGridColumn colRimMiddleInitial;
        private PGridColumn colRimLastName;
        private PGridColumn colIncomingAcctNo;                          //74778
        private AdGbRsmRim _adGbRsmRim;                              //74778
        private string _exMaskedAcct = "";      //#76458
        private PGridColumn colRealAcctNo;      //#76458
        private bool _isExternalAcct = false;    //#76458
        private decimal prevTotalAvailBal = decimal.MinValue;     //#79420

        //Begin #79420, #13851
        private PBaseType exceptRsnCode = new PBaseType("exceptRsnCode");
        private PBaseType floatDays = new PBaseType("floatDays");
        private bool zeroFloatCall = false;
        //End #79420, #13851
        bool _freezeDataInput = false; //#37398

        #region #80620
        private PAction pbOnUsRestrictions;
        private PAction pbOnUsNotes;
        private ArrayList notePtids = new ArrayList();	//#4889
        #endregion


        #region 3475
        private PDecimal _noCopies ;
        private PGridColumn colNxtDayBal;
        private PGridColumn colCalcNxtDayBal;
        private PGridColumn colFloatBal;
        private PGridColumn colFloatDays;
        private PGridColumn colExpireDate;
        private PGridColumnComboBox colExceptRsnCode;
        private PGroupBoxStandard gbRegCCInformation;
        private PDfDisplay dfRegCcCode;
        private PLabelStandard lblRegCcCode;
        private PDfDisplay dfImmediateAvailAmt;
        private PLabelStandard lblImmediateAvailAmt;
        private PLabelStandard lblMakeAvail;
        private PDfDisplay dfImmediateAvailUsed;
        private PLabelStandard lblImmediateAvailUsed;
        private PdfCurrency dfMakeAvail;
        private PDfDisplay dfAvailRemaining;
        private PLabelStandard lblAvailRemaining;
        private PGridColumn colRowCount;
        private PAction pbFloatInfo;
        private PString _printerService;
        #endregion

        //bool validatingGrid = false;                  // #14448
        //int unValidatedRowId = -1;                      // #14448
        //int lastSelectedRowId = -1;                     // #14448
        bool newRowAdded = false;                       // #14448
        //bool validatingItemOnSelChanged = false;        // #14448
        //bool processingCmdKeyVal = false;               // #14448
        GLColumn lastFocusCol = null;                   // #14448
        PGridColumn firstColToFocus = null;          // #14448
        GlacialList glacialTable = null;            // #14448
        bool makeAvailEdited = false;
        private Label lblNewRegCc;
        private PGridColumn colReqExceptRsnCode;
        private PGridColumn colFloatBalExcept;
        private PGridColumn colFloatDaysExcept;
        private PAction pbApplyFloat;
        private PGridColumn colExpireDateExcept;
        private PAction pbTranAcctDisp;
        bool valBankCodeToFocus = false;
        private PGridColumn colTlCaptureISN;
        private PGridColumn colTlCaptureParentISN;
        private PGridColumn colTlCaptureAux;        // #14789

        //Begin #140895
        //IPhoenixForm _workSpaceCurForm = null;
        int _firstNotesRow = -1;
        bool _invokedByWorkflow = false;    // #TellerWF
        private bool IsCreatingTlCaptureTranSet
        {
            get
            {
                return (Workspace != null && _tlTranSet != null && _tlTranSet.IsTellerCaptureTran &&
                    Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet"] != null &&
                    Convert.ToBoolean(Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet"]));
            }
        }
        private bool IsTlCaptureTranSetItemValFailed
        {
            get
            {
                return (Workspace != null && _tlTranSet != null && _tlTranSet.IsTellerCaptureTran &&
                    Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet_itemValFailed"] != null &&
                    Convert.ToBoolean(Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet_itemValFailed"]));
            }
            set
            {
                if (Workspace != null)
                {
                    Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet_itemValFailed"] = value;
                }
            }
        }
        //End #140895
		#endregion

        public AdGbRsmRim AdGbRsmRim
        {
            get
            {
                return _adGbRsmRim;
            }
        }
		public frmTlCapturedItems()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            if (IsDesignMode == false)
            {
                RearrangeColumns();
            }
			this.SupportedActions |= StandardAction.Save;
		}

        private void RearrangeColumns()
        {
            //WARNING: Any new column change to the grid should be addressed to RearrangeColumns() method.
            //
            if (TellerVars.Instance.ReorderAmtItems)
            {
                this.gridCapturedItems.Columns.Clear();
                //
                this.gridCapturedItems.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colRowCount,
            this.colAmount,
            this.colNoOfItems,
            this.colBankCode,
            this.colRoutingNo,
            this.colCheckType,
            this.colAcctType,
            this.colAcctNo,
            this.colCheckNo,
            this.colTranCode,
            //Begin #79420
            this.colFloatBal,
            this.colFloatDays,
            this.colExpireDate,
            this.colExceptRsnCode,
            //End #79420
            // Begin #161243
            this.colFloatBalExcept,
            this.colFloatDaysExcept,
            this.colExpireDateExcept,
            // End #161243
            this.colIndex,
            this.colAcctApplType,
            this.colRowValidated,
            this.colDepLoan,
            this.colItemNo,
            this.colTranEffectiveDt,
            this.colCreateDt,
            this.colTitle1,
            this.colTitle2,
            this.colTitle3,
            this.colNoSignatures,
            this.colRimNo,
            this.colAcctId,
            this.colDepType,
            this.colRimRestricted,
            this.colJointRimFirstName,
            this.colJointRimMiddleInitial,
            this.colJointRimLastName,
            this.colJointRimNo,
            this.colRimFirstName,
            this.colRimMiddleInitial,
            this.colRimLastName,
            this.colIncomingAcctNo,
            this.colRealAcctNo,
            //Begin #79420
            this.colNxtDayBal,
            this.colCalcNxtDayBal,
            //End #79420
            this.colSuspectPtid,
            //Begin #79420, #15357
            this.colReqExceptRsnCode,
            //End #79420, #15357
            //Begin #30926
            this.colTlCaptureISN,
            this.colTlCaptureParentISN,
            this.colTlCaptureAux
            //End #30926
                });
            }
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
            this.gbAcceleratedInput = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfAcctNo = new Phoenix.Windows.Forms.PdfStandard();
            this.dfObjectInput = new Phoenix.Windows.Forms.PdfStandard();
            this.lblObjectInput = new Phoenix.Windows.Forms.PLabelStandard();
            this.colIndex = new Phoenix.Windows.Forms.PGridColumn();
            this.colBankCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colRoutingNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSuspectPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.pbRemove = new Phoenix.Windows.Forms.PAction();
            this.pbCopy = new Phoenix.Windows.Forms.PAction();
            this.pbNewRow = new Phoenix.Windows.Forms.PAction();
            this.pbSuspectDtls = new Phoenix.Windows.Forms.PAction();
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfItemNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.gridCapturedItems = new Phoenix.Windows.Forms.PGrid();
            this.colRowCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoOfItems = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctApplType = new Phoenix.Windows.Forms.PGridColumn();
            this.colRowValidated = new Phoenix.Windows.Forms.PGridColumn();
            this.colDepLoan = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTitle1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colTitle2 = new Phoenix.Windows.Forms.PGridColumn();
            this.colTitle3 = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoSignatures = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctId = new Phoenix.Windows.Forms.PGridColumn();
            this.colDepType = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimRestricted = new Phoenix.Windows.Forms.PGridColumn();
            this.colJointRimFirstName = new Phoenix.Windows.Forms.PGridColumn();
            this.colJointRimMiddleInitial = new Phoenix.Windows.Forms.PGridColumn();
            this.colJointRimLastName = new Phoenix.Windows.Forms.PGridColumn();
            this.colJointRimNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimFirstName = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimMiddleInitial = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimLastName = new Phoenix.Windows.Forms.PGridColumn();
            this.colIncomingAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRealAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colNxtDayBal = new Phoenix.Windows.Forms.PGridColumn();
            this.colCalcNxtDayBal = new Phoenix.Windows.Forms.PGridColumn();
            this.colFloatBal = new Phoenix.Windows.Forms.PGridColumn();
            this.colFloatDays = new Phoenix.Windows.Forms.PGridColumn();
            this.colExpireDate = new Phoenix.Windows.Forms.PGridColumn();
            this.colExceptRsnCode = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colFloatBalExcept = new Phoenix.Windows.Forms.PGridColumn();
            this.colFloatDaysExcept = new Phoenix.Windows.Forms.PGridColumn();
            this.colExpireDateExcept = new Phoenix.Windows.Forms.PGridColumn();
            this.colReqExceptRsnCode = new Phoenix.Windows.Forms.PGridColumn();
            this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNoOfItems = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblNoOfItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalChecksAsCash = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalChecksAsCash = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalOnUsChecks = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalOnUsChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalTransitChecks = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalTransitChecks = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbItemTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblNewRegCc = new System.Windows.Forms.Label();
            this.pbGetAcct = new Phoenix.Windows.Forms.PAction();
            this.pbGetGlAcct = new Phoenix.Windows.Forms.PAction();
            this.pbReprint = new Phoenix.Windows.Forms.PAction();
            this.pbReprintAll = new Phoenix.Windows.Forms.PAction();
            this.gbOnUsAcctInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfOnUsSignatures = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsTitle2 = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfOnUsTitle1 = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTitle2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTitle1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbDisplay = new Phoenix.Windows.Forms.PAction();
            this.pbSignature = new Phoenix.Windows.Forms.PAction();
            this.pbOnUsRestrictions = new Phoenix.Windows.Forms.PAction();
            this.pbOnUsNotes = new Phoenix.Windows.Forms.PAction();
            this.gbRegCCInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfAvailRemaining = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblAvailRemaining = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfMakeAvail = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblMakeAvail = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfImmediateAvailUsed = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblImmediateAvailUsed = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfImmediateAvailAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblImmediateAvailAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRegCcCode = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblRegCcCode = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbFloatInfo = new Phoenix.Windows.Forms.PAction();
            this.pbApplyFloat = new Phoenix.Windows.Forms.PAction();
            this.pbTranAcctDisp = new Phoenix.Windows.Forms.PAction();
            this.colTlCaptureISN = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureParentISN = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureAux = new Phoenix.Windows.Forms.PGridColumn();
            this.gbAcceleratedInput.SuspendLayout();
            this.pGroupBoxStandard1.SuspendLayout();
            this.gbItemTotals.SuspendLayout();
            this.gbOnUsAcctInformation.SuspendLayout();
            this.gbRegCCInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbRemove,
            this.pbCopy,
            this.pbNewRow,
            this.pbGetAcct,
            this.pbGetGlAcct,
            this.pbReprint,
            this.pbReprintAll,
            this.pbDisplay,
            this.pbSignature,
            this.pbOnUsRestrictions,
            this.pbOnUsNotes,
            this.pbFloatInfo,
            this.pbApplyFloat,
            this.pbTranAcctDisp,
            this.pbSuspectDtls});
            // 
            // gbAcceleratedInput
            // 
            this.gbAcceleratedInput.Controls.Add(this.dfAcctNo);
            this.gbAcceleratedInput.Controls.Add(this.dfObjectInput);
            this.gbAcceleratedInput.Controls.Add(this.lblObjectInput);
            this.gbAcceleratedInput.Location = new System.Drawing.Point(4, 64);
            this.gbAcceleratedInput.Name = "gbAcceleratedInput";
            this.gbAcceleratedInput.PhoenixUIControl.ObjectId = 1;
            this.gbAcceleratedInput.Size = new System.Drawing.Size(684, 40);
            this.gbAcceleratedInput.TabIndex = 1;
            this.gbAcceleratedInput.TabStop = false;
            this.gbAcceleratedInput.Text = "Accelerated Input";
            // 
            // dfAcctNo
            // 
            this.dfAcctNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAcctNo.Location = new System.Drawing.Point(176, 16);
            this.dfAcctNo.MaxLength = 12;
            this.dfAcctNo.Name = "dfAcctNo";
            this.dfAcctNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAcctNo.PhoenixUIControl.ObjectId = 3;
            this.dfAcctNo.Size = new System.Drawing.Size(476, 20);
            this.dfAcctNo.TabIndex = 1;
            this.dfAcctNo.Visible = false;
            // 
            // dfObjectInput
            // 
            this.dfObjectInput.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfObjectInput.Location = new System.Drawing.Point(176, 16);
            this.dfObjectInput.Name = "dfObjectInput";
            this.dfObjectInput.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfObjectInput.PhoenixUIControl.ObjectId = 2;
            this.dfObjectInput.Size = new System.Drawing.Size(476, 20);
            this.dfObjectInput.TabIndex = 1;
            // 
            // lblObjectInput
            // 
            this.lblObjectInput.AutoEllipsis = true;
            this.lblObjectInput.Location = new System.Drawing.Point(4, 16);
            this.lblObjectInput.Name = "lblObjectInput";
            this.lblObjectInput.PhoenixUIControl.ObjectId = 2;
            this.lblObjectInput.Size = new System.Drawing.Size(88, 20);
            this.lblObjectInput.TabIndex = 0;
            this.lblObjectInput.Text = "&Object Input:";
            // 
            // colIndex
            // 
            this.colIndex.PhoenixUIControl.ObjectId = 5;
            this.colIndex.PhoenixUIControl.XmlTag = "Index";
            this.colIndex.Title = "Index";
            this.colIndex.Visible = false;
            this.colIndex.Width = 0;
            // 
            // colBankCode
            // 
            this.colBankCode.PhoenixUIControl.InputMask = "!!!!!";
            this.colBankCode.PhoenixUIControl.ObjectId = 6;
            this.colBankCode.PhoenixUIControl.XmlTag = "BankCode";
            this.colBankCode.ReadOnly = false;
            this.colBankCode.Title = "Bank Code";
            this.colBankCode.Width = 51;
            this.colBankCode.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colBankCode_PhoenixUIValidateEvent);
            // 
            // colRoutingNo
            // 
            this.colRoutingNo.PhoenixUIControl.InputMask = "9999-99999";
            this.colRoutingNo.PhoenixUIControl.ObjectId = 7;
            this.colRoutingNo.PhoenixUIControl.XmlTag = "RoutingNo";
            this.colRoutingNo.ReadOnly = false;
            this.colRoutingNo.Title = "Routing No";
            this.colRoutingNo.Width = 80;
            this.colRoutingNo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colRoutingNo_PhoenixUIValidateEvent);
            // 
            // colSuspectPtid
            // 
            this.colSuspectPtid.PhoenixUIControl.ObjectId = 51;
            this.colSuspectPtid.PhoenixUIControl.XmlTag = "SuspectPtid";
            this.colSuspectPtid.Title = "Suspect Ptid";
            this.colSuspectPtid.Visible = false;
            this.colSuspectPtid.Width = 30;
            // 
            // colCheckType
            // 
            this.colCheckType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colCheckType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.colCheckType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colCheckType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colCheckType.PhoenixUIControl.ObjectId = 8;
            this.colCheckType.PhoenixUIControl.XmlTag = "CheckType";
            this.colCheckType.ReadOnly = false;
            this.colCheckType.Title = "Check Type";
            this.colCheckType.Width = 92;
            this.colCheckType.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colCheckType_PhoenixUILeaveEvent);
            this.colCheckType.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colCheckType_PhoenixUIValidateEvent);
            // 
            // colAcctType
            // 
            this.colAcctType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctType.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.colAcctType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.colAcctType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctType.PhoenixUIControl.ObjectId = 9;
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.ReadOnly = false;
            this.colAcctType.Title = "Acct Type";
            this.colAcctType.Width = 47;
            this.colAcctType.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAcctType_PhoenixUIValidateEvent);
            // 
            // colAcctNo
            // 
            this.colAcctNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctNo.PhoenixUIControl.ObjectId = 10;
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.ReadOnly = false;
            this.colAcctNo.Title = "Account Number";
            this.colAcctNo.Width = 145;
            this.colAcctNo.PhoenixUIEnterEvent += new Phoenix.Windows.Forms.EnterEventHandler(this.colAcctNo_PhoenixUIEnterEvent);
            this.colAcctNo.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colAcctNo_PhoenixUILeaveEvent);
            this.colAcctNo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAcctNo_PhoenixUIValidateEvent);
            // 
            // colCheckNo
            // 
            this.colCheckNo.PhoenixUIControl.ObjectId = 11;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.ReadOnly = false;
            this.colCheckNo.ShowNullAsEmpty = true;
            this.colCheckNo.Title = "Check No";
            this.colCheckNo.Width = 58;
            this.colCheckNo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colCheckNo_PhoenixUIValidateEvent);
            // 
            // colTranCode
            // 
            this.colTranCode.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.colTranCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.colTranCode.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colTranCode.PhoenixUIControl.ObjectId = 12;
            this.colTranCode.PhoenixUIControl.XmlTag = "TranCode";
            this.colTranCode.ReadOnly = false;
            this.colTranCode.ShowNullAsEmpty = true;
            this.colTranCode.Title = "TC";
            this.colTranCode.Width = 40;
            this.colTranCode.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colTranCode_PhoenixUIValidateEvent);
            // 
            // colAmount
            // 
            this.colAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.ObjectId = 13;
            this.colAmount.PhoenixUIControl.XmlTag = "Amount";
            this.colAmount.ReadOnly = false;
            this.colAmount.ShowNullAsEmpty = true;
            this.colAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colAmount.Title = "Amount";
            this.colAmount.Width = 86;
            this.colAmount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colAmount_PhoenixUILeaveEvent);
            this.colAmount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAmount_PhoenixUIValidateEvent);
            // 
            // pbRemove
            // 
            this.pbRemove.LongText = "pbRemove";
            this.pbRemove.Name = "pbRemove";
            this.pbRemove.ObjectId = 22;
            this.pbRemove.ShortText = "pbRemove";
            this.pbRemove.Tag = null;
            this.pbRemove.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRemove_Click);
            // 
            // pbCopy
            // 
            this.pbCopy.LongText = "pbCopy";
            this.pbCopy.Name = "pbCopy";
            this.pbCopy.ObjectId = 23;
            this.pbCopy.ShortText = "pbCopy";
            this.pbCopy.Tag = null;
            this.pbCopy.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCopy_Click);
            // 
            // pbNewRow
            // 
            this.pbNewRow.LongText = "pbNewRow";
            this.pbNewRow.Name = "pbNewRow";
            this.pbNewRow.ObjectId = 24;
            this.pbNewRow.ShortText = "pbNewRow";
            this.pbNewRow.Tag = null;
            this.pbNewRow.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbNewRow_Click);
            // 
            // pbSuspectDtls
            // 
            this.pbSuspectDtls.LongText = "Suspt Dtl ";
            this.pbSuspectDtls.Name = "Suspt Dtl ";
            this.pbSuspectDtls.ObjectId = 50;
            this.pbSuspectDtls.ShortText = "Suspt Dtl ";
            this.pbSuspectDtls.Tag = null;
            this.pbSuspectDtls.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSuspectDtls_Click);
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.dfItemNo);
            this.pGroupBoxStandard1.Controls.Add(this.gridCapturedItems);
            this.pGroupBoxStandard1.Controls.Add(this.pLabelStandard1);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 104);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(684, 200);
            this.pGroupBoxStandard1.TabIndex = 2;
            this.pGroupBoxStandard1.TabStop = false;
            // 
            // dfItemNo
            // 
            this.dfItemNo.Location = new System.Drawing.Point(56, 182);
            this.dfItemNo.Multiline = true;
            this.dfItemNo.Name = "dfItemNo";
            this.dfItemNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfItemNo.PhoenixUIControl.ObjectId = 26;
            this.dfItemNo.Size = new System.Drawing.Size(100, 13);
            this.dfItemNo.TabIndex = 2;
            this.dfItemNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gridCapturedItems
            // 
            this.gridCapturedItems.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colRowCount,
            this.colBankCode,
            this.colRoutingNo,
            this.colCheckType,
            this.colAcctType,
            this.colAcctNo,
            this.colCheckNo,
            this.colTranCode,
            this.colAmount,
            this.colNoOfItems,
            this.colIndex,
            this.colAcctApplType,
            this.colRowValidated,
            this.colDepLoan,
            this.colItemNo,
            this.colTranEffectiveDt,
            this.colCreateDt,
            this.colTitle1,
            this.colTitle2,
            this.colTitle3,
            this.colNoSignatures,
            this.colRimNo,
            this.colAcctId,
            this.colDepType,
            this.colRimRestricted,
            this.colJointRimFirstName,
            this.colJointRimMiddleInitial,
            this.colJointRimLastName,
            this.colJointRimNo,
            this.colRimFirstName,
            this.colRimMiddleInitial,
            this.colRimLastName,
            this.colIncomingAcctNo,
            this.colRealAcctNo,
            this.colNxtDayBal,
            this.colCalcNxtDayBal,
            this.colFloatBal,
            this.colFloatDays,
            this.colExpireDate,
            this.colExceptRsnCode,
            this.colFloatBalExcept,
            this.colFloatDaysExcept,
            this.colExpireDateExcept,
            this.colSuspectPtid,
            this.colReqExceptRsnCode,
            this.colTlCaptureISN,
            this.colTlCaptureParentISN,
            this.colTlCaptureAux});
            this.gridCapturedItems.IsMaxNumRowsCustomized = false;
            this.gridCapturedItems.LinesInHeader = 3;
            this.gridCapturedItems.Location = new System.Drawing.Point(4, 12);
            this.gridCapturedItems.Name = "gridCapturedItems";
            this.gridCapturedItems.Size = new System.Drawing.Size(676, 169);
            this.gridCapturedItems.TabIndex = 0;
            this.gridCapturedItems.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridCapturedItems_BeforePopulate);
            this.gridCapturedItems.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridCapturedItems_FetchRowDone);
            this.gridCapturedItems.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridCapturedItems_AfterPopulate);
            this.gridCapturedItems.EndOfRows += new Phoenix.Windows.Forms.GridEventHandler(this.gridCapturedItems_EndOfRows);
            this.gridCapturedItems.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridCapturedItems_SelectedIndexChanged);
            /* Begin Task#41151 */
            this.gridCapturedItems.Items.Parent.hPanelScrollBar.Scroll += new ScrollEventHandler(hPanelScrollBar_Scroll);
            /* End Task#41151 */
            // 
            // colRowCount
            // 
            this.colRowCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowCount.PhoenixUIControl.XmlTag = "0";
            this.colRowCount.Title = "";
            this.colRowCount.Width = 12;
            // 
            // colNoOfItems
            // 
            this.colNoOfItems.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoOfItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoOfItems.PhoenixUIControl.ObjectId = 25;
            this.colNoOfItems.PhoenixUIControl.XmlTag = "NoItems";
            this.colNoOfItems.ReadOnly = false;
            this.colNoOfItems.ShowNullAsEmpty = true;
            this.colNoOfItems.Title = "# of Items";
            this.colNoOfItems.Width = 44;
            this.colNoOfItems.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colNoOfItems_PhoenixUILeaveEvent);
            this.colNoOfItems.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colNoOfItems_PhoenixUIValidateEvent);
            // 
            // colAcctApplType
            // 
            this.colAcctApplType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctApplType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctApplType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctApplType.PhoenixUIControl.XmlTag = "ApplType";
            this.colAcctApplType.Title = "Appl Type";
            this.colAcctApplType.Visible = false;
            this.colAcctApplType.Width = 0;
            // 
            // colRowValidated
            // 
            this.colRowValidated.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowValidated.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowValidated.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colRowValidated.Title = "Row Validated";
            this.colRowValidated.Visible = false;
            this.colRowValidated.Width = 0;
            // 
            // colDepLoan
            // 
            this.colDepLoan.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colDepLoan.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colDepLoan.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDepLoan.PhoenixUIControl.XmlTag = "DepLoan";
            this.colDepLoan.Title = "DepLoan";
            this.colDepLoan.Visible = false;
            this.colDepLoan.Width = 0;
            // 
            // colItemNo
            // 
            this.colItemNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colItemNo.PhoenixUIControl.XmlTag = "ItemNo";
            this.colItemNo.Title = "Item No";
            this.colItemNo.Visible = false;
            this.colItemNo.Width = 0;
            // 
            // colTranEffectiveDt
            // 
            this.colTranEffectiveDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTranEffectiveDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colTranEffectiveDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTranEffectiveDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colTranEffectiveDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranEffectiveDt.PhoenixUIControl.XmlTag = "TranEffectiveDt";
            this.colTranEffectiveDt.Title = "Tran Effective Dt";
            this.colTranEffectiveDt.Visible = false;
            this.colTranEffectiveDt.Width = 0;
            // 
            // colCreateDt
            // 
            this.colCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
            this.colCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
            this.colCreateDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.colCreateDt.Title = "Create Dt";
            this.colCreateDt.Visible = false;
            this.colCreateDt.Width = 0;
            // 
            // colTitle1
            // 
            this.colTitle1.PhoenixUIControl.XmlTag = "Title1";
            this.colTitle1.Title = "Title 1";
            this.colTitle1.Visible = false;
            this.colTitle1.Width = 0;
            // 
            // colTitle2
            // 
            this.colTitle2.PhoenixUIControl.XmlTag = "Title2";
            this.colTitle2.Title = "Title 2";
            this.colTitle2.Visible = false;
            this.colTitle2.Width = 0;
            // 
            // colTitle3
            // 
            this.colTitle3.PhoenixUIControl.XmlTag = "00";
            this.colTitle3.Title = "Title3";
            this.colTitle3.Visible = false;
            this.colTitle3.Width = 0;
            // 
            // colNoSignatures
            // 
            this.colNoSignatures.PhoenixUIControl.XmlTag = "NoSignatures";
            this.colNoSignatures.Title = "No Signatures";
            this.colNoSignatures.Visible = false;
            this.colNoSignatures.Width = 0;
            // 
            // colRimNo
            // 
            this.colRimNo.PhoenixUIControl.XmlTag = "RimNo";
            this.colRimNo.Title = "Rim No";
            this.colRimNo.Visible = false;
            this.colRimNo.Width = 0;
            // 
            // colAcctId
            // 
            this.colAcctId.PhoenixUIControl.XmlTag = "AcctId";
            this.colAcctId.Title = "Acct ID";
            this.colAcctId.Visible = false;
            this.colAcctId.Width = 0;
            // 
            // colDepType
            // 
            this.colDepType.PhoenixUIControl.XmlTag = "DepType";
            this.colDepType.Title = "DepType";
            this.colDepType.Visible = false;
            this.colDepType.Width = 0;
            // 
            // colRimRestricted
            // 
            this.colRimRestricted.PhoenixUIControl.XmlTag = "1";
            this.colRimRestricted.Title = "Rim Restricted";
            this.colRimRestricted.Visible = false;
            this.colRimRestricted.Width = 0;
            // 
            // colJointRimFirstName
            // 
            this.colJointRimFirstName.PhoenixUIControl.XmlTag = "JointRimFirstName";
            this.colJointRimFirstName.Title = "Joint Rim First Name";
            this.colJointRimFirstName.Visible = false;
            this.colJointRimFirstName.Width = 0;
            // 
            // colJointRimMiddleInitial
            // 
            this.colJointRimMiddleInitial.PhoenixUIControl.XmlTag = "JointRimMiddleInitial";
            this.colJointRimMiddleInitial.Title = "Joint Rim Middle Initial";
            this.colJointRimMiddleInitial.Visible = false;
            this.colJointRimMiddleInitial.Width = 0;
            // 
            // colJointRimLastName
            // 
            this.colJointRimLastName.PhoenixUIControl.XmlTag = "JointRimLastName";
            this.colJointRimLastName.Title = "Joint Rim Last Name";
            this.colJointRimLastName.Visible = false;
            this.colJointRimLastName.Width = 0;
            // 
            // colJointRimNo
            // 
            this.colJointRimNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colJointRimNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colJointRimNo.PhoenixUIControl.XmlTag = "JointRimNo";
            this.colJointRimNo.Title = "Joint Rim No";
            this.colJointRimNo.Visible = false;
            this.colJointRimNo.Width = 0;
            // 
            // colRimFirstName
            // 
            this.colRimFirstName.PhoenixUIControl.XmlTag = "RimFirstName";
            this.colRimFirstName.Title = "Rim First Name";
            this.colRimFirstName.Visible = false;
            this.colRimFirstName.Width = 0;
            // 
            // colRimMiddleInitial
            // 
            this.colRimMiddleInitial.PhoenixUIControl.XmlTag = "RimMiddleInitial";
            this.colRimMiddleInitial.Title = "Rim Middle Initial";
            this.colRimMiddleInitial.Visible = false;
            this.colRimMiddleInitial.Width = 0;
            // 
            // colRimLastName
            // 
            this.colRimLastName.PhoenixUIControl.XmlTag = "RimLastName";
            this.colRimLastName.Title = "Rim Last Name";
            this.colRimLastName.Visible = false;
            this.colRimLastName.Width = 0;
            // 
            // colIncomingAcctNo
            // 
            this.colIncomingAcctNo.PhoenixUIControl.ObjectId = 37;
            this.colIncomingAcctNo.PhoenixUIControl.XmlTag = "IncomingAcctNo";
            this.colIncomingAcctNo.Title = "Incoming Account Number";
            this.colIncomingAcctNo.Width = 0;
            // 
            // colRealAcctNo
            // 
            this.colRealAcctNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colRealAcctNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colRealAcctNo.PhoenixUIControl.XmlTag = "RealAcctNo";
            this.colRealAcctNo.Title = "Real Acct No";
            this.colRealAcctNo.Visible = false;
            this.colRealAcctNo.Width = 0;
            // 
            // colNxtDayBal
            // 
            this.colNxtDayBal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNxtDayBal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNxtDayBal.PhoenixUIControl.XmlTag = "NxtDayBal";
            this.colNxtDayBal.Title = "Column";
            this.colNxtDayBal.Visible = false;
            // 
            // colCalcNxtDayBal
            // 
            this.colCalcNxtDayBal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCalcNxtDayBal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCalcNxtDayBal.PhoenixUIControl.XmlTag = "2";
            this.colCalcNxtDayBal.Title = "Column";
            this.colCalcNxtDayBal.Visible = false;
            // 
            // colFloatBal
            // 
            this.colFloatBal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFloatBal.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFloatBal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFloatBal.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFloatBal.PhoenixUIControl.ObjectId = 45;
            this.colFloatBal.PhoenixUIControl.XmlTag = "FloatBal";
            this.colFloatBal.Title = "Float Amount";
            // 
            // colFloatDays
            // 
            this.colFloatDays.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colFloatDays.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colFloatDays.PhoenixUIControl.ObjectId = 47;
            this.colFloatDays.PhoenixUIControl.XmlTag = "FloatDays";
            this.colFloatDays.ReadOnly = false;
            this.colFloatDays.Title = "Float Days";
            this.colFloatDays.Width = 80;
            this.colFloatDays.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colFloatDays_PhoenixUIValidateEvent);
            // 
            // colExpireDate
            // 
            this.colExpireDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colExpireDate.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colExpireDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colExpireDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colExpireDate.PhoenixUIControl.ObjectId = 48;
            this.colExpireDate.PhoenixUIControl.XmlTag = "FloatDate";
            this.colExpireDate.Title = "Expire Date";
            // 
            // colExceptRsnCode
            // 
            this.colExceptRsnCode.AutoDrop = false;
            this.colExceptRsnCode.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colExceptRsnCode.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colExceptRsnCode.PhoenixUIControl.ObjectId = 46;
            this.colExceptRsnCode.PhoenixUIControl.XmlTag = "ExceptRsnCode";
            this.colExceptRsnCode.ReadOnly = false;
            this.colExceptRsnCode.Title = "Except Rsn Code";
            this.colExceptRsnCode.Width = 200;
            // 
            // colFloatBalExcept
            // 
            this.colFloatBalExcept.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFloatBalExcept.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFloatBalExcept.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFloatBalExcept.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFloatBalExcept.PhoenixUIControl.ObjectId = 52;
            this.colFloatBalExcept.PhoenixUIControl.XmlTag = "FloatBalExcept";
            this.colFloatBalExcept.ShowNullAsEmpty = true;
            this.colFloatBalExcept.Title = "Float Bal Execpt";
            // 
            // colFloatDaysExcept
            // 
            this.colFloatDaysExcept.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colFloatDaysExcept.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colFloatDaysExcept.PhoenixUIControl.ObjectId = 53;
            this.colFloatDaysExcept.PhoenixUIControl.XmlTag = "FloatDaysExcept";
            this.colFloatDaysExcept.ShowNullAsEmpty = true;
            this.colFloatDaysExcept.Title = "Float Days Except";
            // 
            // colExpireDateExcept
            // 
            this.colExpireDateExcept.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colExpireDateExcept.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colExpireDateExcept.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colExpireDateExcept.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colExpireDateExcept.PhoenixUIControl.ObjectId = 54;
            this.colExpireDateExcept.PhoenixUIControl.XmlTag = "FloatDateExcept";
            this.colExpireDateExcept.Title = "Except Expiry Date";
            // 
            // colReqExceptRsnCode
            // 
            this.colReqExceptRsnCode.PhoenixUIControl.XmlTag = "ReqExceptRsnCode";
            this.colReqExceptRsnCode.Title = "Column";
            this.colReqExceptRsnCode.Visible = false;
            this.colReqExceptRsnCode.Width = 0;
            // 
            // pLabelStandard1
            // 
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(4, 182);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 26;
            this.pLabelStandard1.Size = new System.Drawing.Size(44, 13);
            this.pLabelStandard1.TabIndex = 1;
            this.pLabelStandard1.Text = "Item #:";
            // 
            // dfNoOfItems
            // 
            this.dfNoOfItems.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoOfItems.Location = new System.Drawing.Point(544, 40);
            this.dfNoOfItems.Multiline = true;
            this.dfNoOfItems.Name = "dfNoOfItems";
            this.dfNoOfItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoOfItems.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoOfItems.PhoenixUIControl.ObjectId = 27;
            this.dfNoOfItems.Size = new System.Drawing.Size(132, 13);
            this.dfNoOfItems.TabIndex = 8;
            // 
            // lblNoOfItems
            // 
            this.lblNoOfItems.AutoEllipsis = true;
            this.lblNoOfItems.Location = new System.Drawing.Point(408, 40);
            this.lblNoOfItems.Name = "lblNoOfItems";
            this.lblNoOfItems.Size = new System.Drawing.Size(132, 13);
            this.lblNoOfItems.TabIndex = 7;
            this.lblNoOfItems.Text = "# of Items:";
            // 
            // dfTotalChecksAsCash
            // 
            this.dfTotalChecksAsCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChecksAsCash.Location = new System.Drawing.Point(544, 16);
            this.dfTotalChecksAsCash.Multiline = true;
            this.dfTotalChecksAsCash.Name = "dfTotalChecksAsCash";
            this.dfTotalChecksAsCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChecksAsCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalChecksAsCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalChecksAsCash.PhoenixUIControl.ObjectId = 16;
            this.dfTotalChecksAsCash.Size = new System.Drawing.Size(132, 13);
            this.dfTotalChecksAsCash.TabIndex = 4;
            // 
            // lblTotalChecksAsCash
            // 
            this.lblTotalChecksAsCash.AutoEllipsis = true;
            this.lblTotalChecksAsCash.Location = new System.Drawing.Point(408, 16);
            this.lblTotalChecksAsCash.Name = "lblTotalChecksAsCash";
            this.lblTotalChecksAsCash.PhoenixUIControl.ObjectId = 16;
            this.lblTotalChecksAsCash.Size = new System.Drawing.Size(132, 13);
            this.lblTotalChecksAsCash.TabIndex = 3;
            this.lblTotalChecksAsCash.Text = "Checks As Cash Items:";
            // 
            // dfTotalOnUsChecks
            // 
            this.dfTotalOnUsChecks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalOnUsChecks.Location = new System.Drawing.Point(128, 40);
            this.dfTotalOnUsChecks.Multiline = true;
            this.dfTotalOnUsChecks.Name = "dfTotalOnUsChecks";
            this.dfTotalOnUsChecks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalOnUsChecks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalOnUsChecks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalOnUsChecks.PhoenixUIControl.ObjectId = 15;
            this.dfTotalOnUsChecks.Size = new System.Drawing.Size(132, 13);
            this.dfTotalOnUsChecks.TabIndex = 6;
            // 
            // lblTotalOnUsChecks
            // 
            this.lblTotalOnUsChecks.AutoEllipsis = true;
            this.lblTotalOnUsChecks.Location = new System.Drawing.Point(4, 40);
            this.lblTotalOnUsChecks.Name = "lblTotalOnUsChecks";
            this.lblTotalOnUsChecks.PhoenixUIControl.ObjectId = 15;
            this.lblTotalOnUsChecks.Size = new System.Drawing.Size(120, 13);
            this.lblTotalOnUsChecks.TabIndex = 5;
            this.lblTotalOnUsChecks.Text = "On-Us Items:";
            // 
            // dfTotalTransitChecks
            // 
            this.dfTotalTransitChecks.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalTransitChecks.Location = new System.Drawing.Point(128, 16);
            this.dfTotalTransitChecks.Multiline = true;
            this.dfTotalTransitChecks.Name = "dfTotalTransitChecks";
            this.dfTotalTransitChecks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalTransitChecks.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalTransitChecks.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalTransitChecks.PhoenixUIControl.ObjectId = 14;
            this.dfTotalTransitChecks.Size = new System.Drawing.Size(132, 13);
            this.dfTotalTransitChecks.TabIndex = 2;
            // 
            // lblTotalTransitChecks
            // 
            this.lblTotalTransitChecks.AutoEllipsis = true;
            this.lblTotalTransitChecks.Location = new System.Drawing.Point(4, 16);
            this.lblTotalTransitChecks.Name = "lblTotalTransitChecks";
            this.lblTotalTransitChecks.PhoenixUIControl.ObjectId = 14;
            this.lblTotalTransitChecks.Size = new System.Drawing.Size(120, 13);
            this.lblTotalTransitChecks.TabIndex = 1;
            this.lblTotalTransitChecks.Text = "Transit Items:";
            // 
            // gbItemTotals
            // 
            this.gbItemTotals.Controls.Add(this.lblNewRegCc);
            this.gbItemTotals.Controls.Add(this.dfNoOfItems);
            this.gbItemTotals.Controls.Add(this.lblNoOfItems);
            this.gbItemTotals.Controls.Add(this.dfTotalChecksAsCash);
            this.gbItemTotals.Controls.Add(this.lblTotalChecksAsCash);
            this.gbItemTotals.Controls.Add(this.dfTotalOnUsChecks);
            this.gbItemTotals.Controls.Add(this.lblTotalOnUsChecks);
            this.gbItemTotals.Controls.Add(this.dfTotalTransitChecks);
            this.gbItemTotals.Controls.Add(this.lblTotalTransitChecks);
            this.gbItemTotals.Location = new System.Drawing.Point(4, 368);
            this.gbItemTotals.Name = "gbItemTotals";
            this.gbItemTotals.PhoenixUIControl.ObjectId = 28;
            this.gbItemTotals.Size = new System.Drawing.Size(684, 78);
            this.gbItemTotals.TabIndex = 4;
            this.gbItemTotals.TabStop = false;
            this.gbItemTotals.Text = "Item Totals";
            // 
            // lblNewRegCc
            // 
            this.lblNewRegCc.AutoSize = true;
            this.lblNewRegCc.Location = new System.Drawing.Point(4, 59);
            this.lblNewRegCc.Name = "lblNewRegCc";
            this.lblNewRegCc.Size = new System.Drawing.Size(64, 13);
            this.lblNewRegCc.TabIndex = 10;
            this.lblNewRegCc.Text = "Disclaimer   ";
            // 
            // pbGetAcct
            // 
            this.pbGetAcct.LongText = "Get &Acct";
            this.pbGetAcct.Name = "Get &Acct";
            this.pbGetAcct.ObjectId = 29;
            this.pbGetAcct.ShortText = "Get &Acct";
            this.pbGetAcct.Tag = null;
            this.pbGetAcct.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbGetAcct_Click);
            // 
            // pbGetGlAcct
            // 
            this.pbGetGlAcct.LongText = "Get &GL Acct";
            this.pbGetGlAcct.Name = "Get &GL Acct";
            this.pbGetGlAcct.ObjectId = 30;
            this.pbGetGlAcct.ShortText = "Get &GL Acct";
            this.pbGetGlAcct.Tag = null;
            this.pbGetGlAcct.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbGetGlAcct_Click);
            // 
            // pbReprint
            // 
            this.pbReprint.LongText = "&Reprint";
            this.pbReprint.Name = "&Reprint";
            this.pbReprint.ObjectId = 31;
            this.pbReprint.ShortText = "&Reprint";
            this.pbReprint.Tag = null;
            this.pbReprint.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReprint_Click);
            // 
            // pbReprintAll
            // 
            this.pbReprintAll.LongText = "Reprint &All";
            this.pbReprintAll.Name = "Reprint &All";
            this.pbReprintAll.ObjectId = 32;
            this.pbReprintAll.ShortText = "Reprint &All";
            this.pbReprintAll.Tag = null;
            this.pbReprintAll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReprintAll_Click);
            // 
            // gbOnUsAcctInformation
            // 
            this.gbOnUsAcctInformation.Controls.Add(this.dfOnUsSignatures);
            this.gbOnUsAcctInformation.Controls.Add(this.dfOnUsTitle2);
            this.gbOnUsAcctInformation.Controls.Add(this.dfOnUsTitle1);
            this.gbOnUsAcctInformation.Controls.Add(this.lblTitle2);
            this.gbOnUsAcctInformation.Controls.Add(this.lblTitle1);
            this.gbOnUsAcctInformation.Location = new System.Drawing.Point(4, 0);
            this.gbOnUsAcctInformation.Name = "gbOnUsAcctInformation";
            this.gbOnUsAcctInformation.Size = new System.Drawing.Size(684, 64);
            this.gbOnUsAcctInformation.TabIndex = 0;
            this.gbOnUsAcctInformation.TabStop = false;
            this.gbOnUsAcctInformation.Text = "Account Title Information";  //#96359
            this.gbOnUsAcctInformation.Enter += new System.EventHandler(this.gbOnUsAcctInformation_Enter);
            // 
            // dfOnUsSignatures
            // 
            this.dfOnUsSignatures.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsSignatures.Location = new System.Drawing.Point(400, 16);
            this.dfOnUsSignatures.Multiline = true;
            this.dfOnUsSignatures.Name = "dfOnUsSignatures";
            this.dfOnUsSignatures.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsSignatures.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsSignatures.Size = new System.Drawing.Size(272, 13);
            this.dfOnUsSignatures.TabIndex = 4;
            this.dfOnUsSignatures.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dfOnUsTitle2
            // 
            this.dfOnUsTitle2.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsTitle2.Location = new System.Drawing.Point(84, 40);
            this.dfOnUsTitle2.Multiline = true;
            this.dfOnUsTitle2.Name = "dfOnUsTitle2";
            this.dfOnUsTitle2.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsTitle2.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsTitle2.PhoenixUIControl.ObjectId = 34;
            this.dfOnUsTitle2.Size = new System.Drawing.Size(312, 13);
            this.dfOnUsTitle2.TabIndex = 3;
            this.dfOnUsTitle2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dfOnUsTitle1
            // 
            this.dfOnUsTitle1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsTitle1.Location = new System.Drawing.Point(84, 16);
            this.dfOnUsTitle1.Multiline = true;
            this.dfOnUsTitle1.Name = "dfOnUsTitle1";
            this.dfOnUsTitle1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOnUsTitle1.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOnUsTitle1.PhoenixUIControl.ObjectId = 33;
            this.dfOnUsTitle1.Size = new System.Drawing.Size(312, 13);
            this.dfOnUsTitle1.TabIndex = 2;
            this.dfOnUsTitle1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitle2
            // 
            this.lblTitle2.AutoEllipsis = true;
            this.lblTitle2.Location = new System.Drawing.Point(8, 40);
            this.lblTitle2.Name = "lblTitle2";
            this.lblTitle2.PhoenixUIControl.ObjectId = 34;
            this.lblTitle2.Size = new System.Drawing.Size(72, 13);
            this.lblTitle2.TabIndex = 1;
            this.lblTitle2.Text = "Title 2:";
            // 
            // lblTitle1
            // 
            this.lblTitle1.AutoEllipsis = true;
            this.lblTitle1.Location = new System.Drawing.Point(8, 16);
            this.lblTitle1.Name = "lblTitle1";
            this.lblTitle1.PhoenixUIControl.ObjectId = 33;
            this.lblTitle1.Size = new System.Drawing.Size(72, 13);
            this.lblTitle1.TabIndex = 0;
            this.lblTitle1.Text = "Title 1:";
            // 
            // pbDisplay
            // 
            this.pbDisplay.LongText = "&On-Us Dsp...";
            this.pbDisplay.Name = "&On-Us Dsp...";
            this.pbDisplay.ObjectId = 35;
            this.pbDisplay.ShortText = "&On-Us Dsp...";
            this.pbDisplay.Tag = null;
            this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
            // 
            // pbSignature
            // 
            this.pbSignature.LongText = "O&n-Us Sig...";
            this.pbSignature.Name = "O&n-Us Sig...";
            this.pbSignature.ObjectId = 36;
            this.pbSignature.ShortText = "O&n-Us Sig...";
            this.pbSignature.Tag = null;
            this.pbSignature.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSignature_Click);
            // 
            // pbOnUsRestrictions
            // 
            this.pbOnUsRestrictions.LongText = "On-Us R&es...";
            this.pbOnUsRestrictions.Name = "On-Us R&es...";
            this.pbOnUsRestrictions.ObjectId = 38;
            this.pbOnUsRestrictions.ShortText = "On-Us R&es...";
            this.pbOnUsRestrictions.Tag = null;
            this.pbOnUsRestrictions.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOnUsRestrictions_Click);
            // 
            // pbOnUsNotes
            // 
            this.pbOnUsNotes.LongText = "On-Us No&t...";
            this.pbOnUsNotes.Name = "On-Us No&t...";
            this.pbOnUsNotes.ObjectId = 39;
            this.pbOnUsNotes.ShortText = "On-Us No&t...";
            this.pbOnUsNotes.Tag = null;
            this.pbOnUsNotes.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOnUsNotes_Click);
            // 
            // gbRegCCInformation
            // 
            this.gbRegCCInformation.Controls.Add(this.dfAvailRemaining);
            this.gbRegCCInformation.Controls.Add(this.lblAvailRemaining);
            this.gbRegCCInformation.Controls.Add(this.dfMakeAvail);
            this.gbRegCCInformation.Controls.Add(this.lblMakeAvail);
            this.gbRegCCInformation.Controls.Add(this.dfImmediateAvailUsed);
            this.gbRegCCInformation.Controls.Add(this.lblImmediateAvailUsed);
            this.gbRegCCInformation.Controls.Add(this.dfImmediateAvailAmt);
            this.gbRegCCInformation.Controls.Add(this.lblImmediateAvailAmt);
            this.gbRegCCInformation.Controls.Add(this.dfRegCcCode);
            this.gbRegCCInformation.Controls.Add(this.lblRegCcCode);
            this.gbRegCCInformation.Location = new System.Drawing.Point(4, 304);
            this.gbRegCCInformation.Name = "gbRegCCInformation";
            this.gbRegCCInformation.Size = new System.Drawing.Size(684, 64);
            this.gbRegCCInformation.TabIndex = 3;
            this.gbRegCCInformation.TabStop = false;
            this.gbRegCCInformation.Text = "Reg CC Information";
            // 
            // dfAvailRemaining
            // 
            this.dfAvailRemaining.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAvailRemaining.Location = new System.Drawing.Point(564, 40);
            this.dfAvailRemaining.Name = "dfAvailRemaining";
            this.dfAvailRemaining.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAvailRemaining.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAvailRemaining.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAvailRemaining.PhoenixUIControl.ObjectId = 44;
            this.dfAvailRemaining.Size = new System.Drawing.Size(116, 13);
            this.dfAvailRemaining.TabIndex = 9;
            // 
            // lblAvailRemaining
            // 
            this.lblAvailRemaining.AutoEllipsis = true;
            this.lblAvailRemaining.Location = new System.Drawing.Point(440, 40);
            this.lblAvailRemaining.Name = "lblAvailRemaining";
            this.lblAvailRemaining.PhoenixUIControl.ObjectId = 44;
            this.lblAvailRemaining.Size = new System.Drawing.Size(120, 13);
            this.lblAvailRemaining.TabIndex = 8;
            this.lblAvailRemaining.Text = "Avail Remaining:";
            // 
            // dfMakeAvail
            // 
            this.dfMakeAvail.Location = new System.Drawing.Point(324, 40);
            this.dfMakeAvail.Name = "dfMakeAvail";
            this.dfMakeAvail.PhoenixUIControl.ObjectId = 43;
            this.dfMakeAvail.Size = new System.Drawing.Size(112, 20);
            this.dfMakeAvail.TabIndex = 7;
            this.dfMakeAvail.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfMakeAvail_PhoenixUIValidateEvent);
            // 
            // lblMakeAvail
            // 
            this.lblMakeAvail.AutoEllipsis = true;
            this.lblMakeAvail.Location = new System.Drawing.Point(256, 40);
            this.lblMakeAvail.Name = "lblMakeAvail";
            this.lblMakeAvail.PhoenixUIControl.ObjectId = 43;
            this.lblMakeAvail.Size = new System.Drawing.Size(64, 13);
            this.lblMakeAvail.TabIndex = 6;
            this.lblMakeAvail.Text = "&Make Avail:";
            // 
            // dfImmediateAvailUsed
            // 
            this.dfImmediateAvailUsed.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfImmediateAvailUsed.Location = new System.Drawing.Point(132, 40);
            this.dfImmediateAvailUsed.Name = "dfImmediateAvailUsed";
            this.dfImmediateAvailUsed.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfImmediateAvailUsed.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfImmediateAvailUsed.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfImmediateAvailUsed.PhoenixUIControl.ObjectId = 42;
            this.dfImmediateAvailUsed.Size = new System.Drawing.Size(116, 13);
            this.dfImmediateAvailUsed.TabIndex = 5;
            // 
            // lblImmediateAvailUsed
            // 
            this.lblImmediateAvailUsed.AutoEllipsis = true;
            this.lblImmediateAvailUsed.Location = new System.Drawing.Point(4, 40);
            this.lblImmediateAvailUsed.Name = "lblImmediateAvailUsed";
            this.lblImmediateAvailUsed.PhoenixUIControl.ObjectId = 42;
            this.lblImmediateAvailUsed.Size = new System.Drawing.Size(124, 13);
            this.lblImmediateAvailUsed.TabIndex = 4;
            this.lblImmediateAvailUsed.Text = "Immediate Avail Used:";
            // 
            // dfImmediateAvailAmt
            // 
            this.dfImmediateAvailAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfImmediateAvailAmt.Location = new System.Drawing.Point(564, 16);
            this.dfImmediateAvailAmt.Name = "dfImmediateAvailAmt";
            this.dfImmediateAvailAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfImmediateAvailAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfImmediateAvailAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfImmediateAvailAmt.PhoenixUIControl.ObjectId = 41;
            this.dfImmediateAvailAmt.Size = new System.Drawing.Size(116, 13);
            this.dfImmediateAvailAmt.TabIndex = 3;
            // 
            // lblImmediateAvailAmt
            // 
            this.lblImmediateAvailAmt.AutoEllipsis = true;
            this.lblImmediateAvailAmt.Location = new System.Drawing.Point(440, 16);
            this.lblImmediateAvailAmt.Name = "lblImmediateAvailAmt";
            this.lblImmediateAvailAmt.PhoenixUIControl.ObjectId = 41;
            this.lblImmediateAvailAmt.Size = new System.Drawing.Size(120, 13);
            this.lblImmediateAvailAmt.TabIndex = 2;
            this.lblImmediateAvailAmt.Text = "Immediate Avail Amt:";
            // 
            // dfRegCcCode
            // 
            this.dfRegCcCode.Location = new System.Drawing.Point(88, 16);
            this.dfRegCcCode.Name = "dfRegCcCode";
            this.dfRegCcCode.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRegCcCode.PhoenixUIControl.ObjectId = 40;
            this.dfRegCcCode.Size = new System.Drawing.Size(348, 13);
            this.dfRegCcCode.TabIndex = 1;
            this.dfRegCcCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRegCcCode
            // 
            this.lblRegCcCode.AutoEllipsis = true;
            this.lblRegCcCode.Location = new System.Drawing.Point(4, 16);
            this.lblRegCcCode.Name = "lblRegCcCode";
            this.lblRegCcCode.PhoenixUIControl.ObjectId = 40;
            this.lblRegCcCode.Size = new System.Drawing.Size(80, 13);
            this.lblRegCcCode.TabIndex = 0;
            this.lblRegCcCode.Text = "Reg CC Code:";
            // 
            // pbFloatInfo
            // 
            this.pbFloatInfo.LongText = "Modif&y Flt...";
            this.pbFloatInfo.Name = "Modif&y Flt...";
            this.pbFloatInfo.ObjectId = 49;
            this.pbFloatInfo.ShortText = "Modif&y Flt...";
            this.pbFloatInfo.Tag = null;
            this.pbFloatInfo.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbFloatInfo_Click);
            // 
            // pbApplyFloat
            // 
            this.pbApplyFloat.LongText = "A&pply Float";
            this.pbApplyFloat.Name = "pbApplyFloat";
            this.pbApplyFloat.ObjectId = 55;
            this.pbApplyFloat.ShortText = "A&pply Float";
            this.pbApplyFloat.Tag = null;
            this.pbApplyFloat.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbApplyFloat_Click);
            // 
            // pbTranAcctDisp
            // 
            this.pbTranAcctDisp.LongText = "Tran Disp...";
            this.pbTranAcctDisp.Name = "pbTranAcctDisp";
            this.pbTranAcctDisp.ObjectId = 56;
            this.pbTranAcctDisp.ShortText = "Tran Disp...";
            this.pbTranAcctDisp.Tag = null;
            this.pbTranAcctDisp.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTranAcctDisp_Click);
            // 
            // colTlCaptureISN
            // 
            this.colTlCaptureISN.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureISN.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureISN.PhoenixUIControl.ObjectId = 57;
            this.colTlCaptureISN.PhoenixUIControl.XmlTag = "TlCaptureIsn";
            this.colTlCaptureISN.Title = "ISN";
            // 
            // colTlCaptureParentISN
            // 
            this.colTlCaptureParentISN.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureParentISN.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureParentISN.PhoenixUIControl.ObjectId = 58;
            this.colTlCaptureParentISN.PhoenixUIControl.XmlTag = "TlCaptureParentIsn";
            this.colTlCaptureParentISN.Title = "Parent ISN";
            // 
            // colTlCaptureAux
            // 
            this.colTlCaptureAux.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureAux.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colTlCaptureAux.PhoenixUIControl.ObjectId = 59;
            this.colTlCaptureAux.PhoenixUIControl.XmlTag = "TlCaptureAux";
            this.colTlCaptureAux.Title = "Aux";
            // 
            // frmTlCapturedItems
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbRegCCInformation);
            this.Controls.Add(this.gbOnUsAcctInformation);
            this.Controls.Add(this.gbItemTotals);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.Controls.Add(this.gbAcceleratedInput);
            this.Name = "frmTlCapturedItems";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCapturedItems_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCapturedItems_PInitCompleteEvent);
            this.PShowCompletedEvent += new System.EventHandler(this.frmTlCapturedItems_PShowCompletedEvent);
            this.Load += new System.EventHandler(this.frmTlCapturedItems_Load);
            this.Shown += new System.EventHandler(this.frmTlCapturedItems_Shown);
            this.gbAcceleratedInput.ResumeLayout(false);
            this.gbAcceleratedInput.PerformLayout();
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.pGroupBoxStandard1.PerformLayout();
            this.gbItemTotals.ResumeLayout(false);
            this.gbItemTotals.PerformLayout();
            this.gbOnUsAcctInformation.ResumeLayout(false);
            this.gbOnUsAcctInformation.PerformLayout();
            this.gbRegCCInformation.ResumeLayout(false);
            this.gbRegCCInformation.PerformLayout();
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
                Parameters.Add(new PString("TlTranCode"));
                Parameters.Add(new PString("AcctType"));
                Parameters.Add(new PString("AcctNo"));
                Parameters.Add(new PString("OutputJson"));
            }

        }
        //End #TellerWF
        public override void InitParameters(params object[] paramList)
		{
            //paramList[0] = new PString("TranSource", "Workflow");
            //paramList[1] = new PString("TlTranCode", "201");
            //paramList[2] = new PString("OutputJson", "");
            //Begin #TellerWF
            if (paramList != null && paramList.Length > 1 && paramList[0] is PBaseType)
            {
                base.InitParameters(paramList);
                if (Parameters.Contains("TranSource") &&  Parameters["TranSource"].StringValue == InputParamTranSet.TranSourceParamValue)
                {
                    _invokedByWorkflow = true;
                    _tlTranSet = new TlTransactionSet();
                    _tlTranSet.CurTran.TlTranCode.Value = Parameters["TlTranCode"].StringValue;
                    if (_tlTranSet.AdTlTc != null && _tlTranSet.AdTlTc.TranCode.Value > 0)
                        _tlTranSet.CurTran.TranCode.Value = _tlTranSet.AdTlTc.TranCode.Value;
                    items = _tlTranSet.CurTran.Items;
                    if (!string.IsNullOrEmpty(Parameters["AcctType"].StringValue) && 
                        !string.IsNullOrEmpty(Parameters["AcctNo"].StringValue))
                    {
                        _tlTranSet.CurTran.TranAcct.AcctType.Value = Parameters["AcctType"].StringValue;
                        _tlTranSet.CurTran.TranAcct.AcctNo.Value = Parameters["AcctNo"].StringValue;
                    }
                    this.AutoFetch = false;
                    this.ScreenId = Phoenix.Shared.Constants.ScreenId.CapturedItems;
                    this.CurrencyId = _tellerVars.LocalCrncyId;
                    return;
                }
            }
            //End #TellerWF

            if (paramList.Length >= 7)
			{
				isViewOnly = Convert.ToBoolean(paramList[0]);
				branchNo.Value = Convert.ToInt16(paramList[1]);
				drawerNo.Value = Convert.ToInt16(paramList[2]);
				sequenceNo.Value = Convert.ToInt16(paramList[3]);
				if (paramList[4] != null)
					effectiveDt.Value = Convert.ToDateTime(paramList[4]);
				subSequence.Value = Convert.ToInt16(paramList[5]);
				if (paramList[6] != null)
				{
					_tlTranSet = (TlTransactionSet)paramList[6];
					items = _tlTranSet.CurTran.Items;
                    LoadOrigItems(items);   //#80618
				}
				if (paramList.Length > 7) // required for reprint
				{
					if (paramList[7] != null)
						journalPtid.Value = Convert.ToDecimal(paramList[7]);
				}

				if (isViewOnly )
				{
					_busObjCapturedItems.BranchNo.Value = branchNo.Value;
					_busObjCapturedItems.DrawerNo.Value = drawerNo.Value;
					_busObjCapturedItems.SequenceNo.Value = sequenceNo.Value;
					_busObjCapturedItems.EffectiveDt.Value = effectiveDt.Value;
					_busObjCapturedItems.SubSequence.Value = subSequence.Value;
                    // Begin #75709
                    //_busObjCapturedItems.OutputTypeId.Value = 10; // custom list view only
                    if (_tlTranSet == null)
                        _tlTranSet = new TlTransactionSet();
                    if (!_tlTranSet.TellerVars.IsAppOnline)
                        _busObjCapturedItems.OutputTypeId.Value = 1; // We are offline
                    else
                        _busObjCapturedItems.OutputTypeId.Value = 10; // We are not offline so can do custom listview
                    // End #75709
				}
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.CapturedItems;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			#region make editable columns
			this.colBankCode.ReadOnly = isViewOnly;
			this.colRoutingNo.ReadOnly = isViewOnly;
			this.colCheckType.ReadOnly = isViewOnly;
			this.colAcctType.ReadOnly = isViewOnly;
			this.colAcctNo.ReadOnly = isViewOnly;
			this.colCheckNo.ReadOnly = isViewOnly;
			this.colTranCode.ReadOnly = isViewOnly;
			this.colAmount.ReadOnly = isViewOnly;
			this.colNoOfItems.ReadOnly = isViewOnly;
            this.colRowCount.ReadOnly = true;   //#80618
			#endregion

			base.InitParameters (paramList);
		}

		#endregion

		#region standard actions
		public override bool OnActionSave(bool isAddNext)
		{
            _isSaveTriggered = true;  //#80618
            TlItemCapture item = new TlItemCapture();
            string onUsChkAccounts = string.Empty;  //#80618
            if (gridCapturedItems.Count > 0)
                item = new TlItemCapture(); //Selva - hack
            bool isSuccess = true;

			//
			RemoveBlankRow();
            //#74777 and #74784 - moved load items after removeblankrow to fix the problem.
            if (gridCapturedItems.Count > items.Count && !isViewOnly)
                this.LoadItems(false, false, null);
            //
			for( int rowId = 0; rowId < gridCapturedItems.Count; rowId++ )
			{
                if (gridCapturedItems.Count > 0)
                {
                    _busObjCapturedItems = (items[rowId] as TlItemCapture); //Selva - hack
                }

				ContextRowScreenToObject(rowId, false);
				//
                if (!ValidateGrid(null, false)) //Selva-New Change
                {
                    if (!_busObjCapturedItems.ActionReturnCode.IsNull && _busObjCapturedItems.ActionReturnCode.Value != 0)
                    {
                        if (_busObjCapturedItems.ActionReturnCode.Value < 0)
                        {
                            returnCodeDesc = _gbHelper.GetSPMessageText(_busObjCapturedItems.ActionReturnCode.Value, false);
                            PMessageBox.Show(this, 360532, MessageType.Warning, MessageBoxButtons.OK, new string[] { Convert.ToString(_busObjCapturedItems.ActionReturnCode.Value) + " - " + returnCodeDesc });
                        }
                        else
                        {
                            PMessageBox.Show(this, _busObjCapturedItems.ActionReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                        }
                    }

                    if (_focusField != null)
                    {
                        _IsFromSelectRow = true;
                        gridCapturedItems.SelectRow(rowId, true);
                        _IsFromSelectRow = false;
                        gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)].Focus();   //#80618
                        //gridCapturedItems.NextFocusColumn =
                        //    gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)]; //#80618
                    }
                    //
                    isSuccess = false;
                    break;
                }
                else  //#80618
                {
                    if (!_busObjCapturedItems.VerifyDuplicateCheck(ref onUsChkAccounts))
                    {
                        _focusField = _busObjCapturedItems.CheckNo.XmlTag;   //#80618
                        PMessageBox.Show(this, 13440, MessageType.Error, MessageBoxButtons.OK, new string[] { _busObjCapturedItems.AcctType.Value + "~" + _busObjCapturedItems.AcctNo.Value + "~" + _busObjCapturedItems.CheckNo.StringValue });
                        //
                        if (_focusField != null)
                        {
                            _IsFromSelectRow = true;
                            gridCapturedItems.SelectRow(rowId, true);
                            _IsFromSelectRow = false;
                            gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)].Focus();   //#80618
                        }
                        //
                        isSuccess = false;
                        break;
                    }
                }
                //    EndOfRowValidation(rowId);  //#3714
                #region #9519
                //#90669:  Addition of new validation RequireTellerFraudDetails
                if (_isFraudPreventionEnabled && _busObjCapturedItems.CheckType.Value == "T" && _tellerVars.AdTlControl.RequireTellerFraudDetails.Value == "Y")
                {
                    string errStr = "";
					if (_busObjCapturedItems.AcctNo.IsNull || _busObjCapturedItems.AcctNo.Value.Trim()=="")
					{
						gridCapturedItems.SelectRow(rowId, true);
						errStr += "\"Account #\" field";
						gridCapturedItems.SelectRow(rowId, true);
						colAcctNo.Focus();
					}
					if (_busObjCapturedItems.RoutingNo.IsNull || _busObjCapturedItems.RoutingNo.Value.Trim() == "")
					{
						if (errStr =="")
						{
							errStr += "\"Routing #\" field";
							gridCapturedItems.SelectRow(rowId, true);
							colRoutingNo.Focus();
						}
						else
						{
							errStr += " and \"Routing #\" field";
						}
					}
					if (_busObjCapturedItems.CheckNo.IsNull)
					{
						if (errStr == "")
						{
							errStr += "\"Check #\" field";
							gridCapturedItems.SelectRow(rowId, true);
							colCheckNo.Focus();
						}
						else
						{
							errStr += " and \"Check #\" field";
						}

					}
					if (errStr != "")
					{
						PMessageBox.Show(13282, MessageType.Error, errStr);
						return false;
					}
				}
				#endregion #9519
				gridCapturedItems.SetCellValueUnFormatted(rowId, colAcctApplType.ColumnId, _busObjCapturedItems.ApplType.Value);
			}
			if (isSuccess)
			{
                if (gridCapturedItems.Count > 0)
                {
                    LoadItems(false, false, null);
                    CalcTotal();

                    //Begin #79420
                    isSuccess = ValidateFloatOnSave(ActionSave);
                    //End #79420

                    //Begin #79420, #15357
                    if (!isSuccess)
                        return isSuccess;
                    //End #79420, #15357

                    //
                    if (isSuccess)  // #79420
                    {
                        _IsFromSelectRow = false; //Selva
                        _IsForceContextRow = true; //Selva-hack
                        gridCapturedItems.SelectRow(0, true);
                        _IsForceContextRow = false; //Selva-hack
                        _IsFromSelectRow = false;
                    }
                    //Begin #80618
                    //Load origItems collection with everything from the items collection
                    //
                    //if (origItems != null && origItems.Count > 0)
                    //    origItems.Clear();
                    //origItems.AddRange(items);
                    //Begin #79420, #15357, #15085
                    //delete any unsynced items which may have crept in due to all the curly code in the window.
                    if (items.Count > gridCapturedItems.Count)
                    {
                        int itemCount = items.Count ;
                        for (int i = itemCount - 1; i >= gridCapturedItems.Count; i--)
                        {
                            items.RemoveAt(i);
                        }
                    }
                    //End #79420, #15357, #15085
                    LoadOrigItems(items);
                    //
                    //
                    // End ##80618
                    _IsFromSelectRow = false; //Selva
                    _IsForceContextRow = true; //Selva-hack
                    gridCapturedItems.SelectRow(0, true);
                    _IsForceContextRow = false; //Selva-hack
                    _IsFromSelectRow = false;

                    //Begin #TellerWF
                    if ( _invokedByWorkflow)
                    {
                        List<InputParamTranItem> outputItems = new List<InputParamTranItem>();
                        foreach( TlItemCapture capItem in items)
                        {
                            //outputItems.Add(new InputParamTranItem
                            //{
                            //    AcctNo = capItem.AcctNo.Value,
                            //    AcctType = capItem.AcctType.Value,
                            //    Amount = capItem.Amount.Value,
                            //    CheckNo = capItem.CheckNo.IsNull ? null : capItem.CheckNo.StringValue,
                            //    CheckType = capItem.CheckType.Value,
                            //    RoutingNo = capItem.RoutingNo.Value,
                            //    TranCode = capItem.TranCode.IsNull ? (short)0 : capItem.TranCode.Value,
                            //    NoItems = capItem.NoItems.IsNull ? (short)1 : capItem.NoItems.Value
                            //});

                            InputParamTranItem paramTranItem = new InputParamTranItem();
                            InputParamTranSet.MoveValueFromBusObjectToObject(capItem, paramTranItem);
                            outputItems.Add(paramTranItem);
                        }
                        if ( Parameters.Contains("OutputJson"))
                        {
                            Parameters["OutputJson"].SetValue(Newtonsoft.Json.JsonConvert.SerializeObject(outputItems));
                        }
                    }
                    //End #TellerWF
                }
                else
                {
                    //Begin #14448
                    items.Clear();
                    origItems.Clear();
                    ValidateFloatOnSave(ActionSave);
                    //End #14448
                }

                Workspace.Variables["frmTlTranCode_creatingTlCaptureTranSet_MakeAvail"] = Convert.ToDecimal(dfMakeAvail.Value);     //#87743
            }
            return isSuccess;

			#region do not call the base
			//return base.OnActionSave (isAddNext);
			#endregion
		}

		public override bool OnActionClose()
		{
            //Begin #140895
            if (IsTlCaptureTranSetItemValFailed)
            {
                AvoidSave = true;
                return base.OnActionClose();
            }
            //End #140895

            _isSaveTriggered = false; //#80618
            if (gridCapturedItems.Count > 0 && !isViewOnly)
                RemoveBlankRow();
            // Begin #74488
            if (gridCapturedItems.Count > items.Count && !isViewOnly)
                this.LoadItems(false, false, null);
            // End #74488


			if( base.OnActionClose ())
			{
                if (!_isSaveTriggered)    //#80618
                {
                    DiscardUnsavedItems(items);
                }
                //Begin #79420
                if (!isViewOnly && !ValidateFloatOnSave( ActionClose ))
                    return false;
                //End #79420
				if (!isViewOnly && _invokedByWorkflow == false) //  #TellerWF - Check for _invokedByWorkflow
                {
					PfwStandard parentForm =  this.Workspace.ContentWindow.PreviousWindow as PfwStandard;
					if( parentForm != null )
					{
						parentForm.CallParent("ItemCapture");
					}
				}
				return true;
			}
            gridCapturedItems.Focus();  // #79420, #15357 - to force the focus back on grid so that the escpape key works
			return false;
		}


		public override bool IsFormDirty()
		{
			if(isViewOnly)
				return false;
			bool isEdited = false;
			string fieldValue = string.Empty;
            //object objectValue = null;    //#80618
            //int noOfItemsColumnId = colNoOfItems.ColumnId;    //#80618
            //
			#region is grid edited

            #region 80618 - commented out the code added to identify the any edits, since this is not working
            //if (gridCapturedItems.Count > 0)
            //{
            //    for (int rowId = 0; rowId < gridCapturedItems.Count; rowId++)
            //    {
            //        _busObjCapturedItems = (items[rowId] as TlItemCapture); //Selva - hack
            //        ContextRowScreenToObject(rowId, false);
            //        //
            //        objectValue = gridCapturedItems.GetCellValueUnformatted(rowId, colIndex.ColumnId);
            //        if (objectValue != null) //existing row
            //        {
            //            if (Convert.ToInt32(objectValue) >= 0 && Convert.ToInt32(objectValue) <= noOfItemsColumnId)   //#75325 - replaced > 0 with >= 0
            //            {
            //                TlItemCapture item1 = (TlItemCapture)items[rowId];

            //                if (_busObjCapturedItems.AcctType.Value == "" || _busObjCapturedItems.AcctType.Value == string.Empty)
            //                    _busObjCapturedItems.AcctType.SetValue(null, EventBehavior.None);		// #6615 - _busObjCapturedItems.AcctType.SetValueToNull();
            //                //
            //                if (_busObjCapturedItems.BankCode.Value != item1.BankCode.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.RoutingNo.Value != item1.RoutingNo.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.CheckType.Value != item1.CheckType.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.AcctType.Value != item1.AcctType.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.AcctNo.Value != item1.AcctNo.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.CheckNo.Value != item1.CheckNo.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.TranCode.Value != item1.TranCode.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.Amount.Value != item1.Amount.Value)
            //                    isEdited = true;
            //                else if (_busObjCapturedItems.NoItems.Value != item1.NoItems.Value)
            //                    isEdited = true;
            //            }
            //        }
            //        else //new row
            //        {
            //            if (!_busObjCapturedItems.BankCode.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.RoutingNo.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.CheckType.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.AcctType.IsNull && !string.IsNullOrEmpty(_busObjCapturedItems.AcctType.Value))
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.AcctNo.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.CheckNo.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.TranCode.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.Amount.IsNull)
            //                isEdited = true;
            //            else if (!_busObjCapturedItems.NoItems.IsNull)
            //                isEdited = true;
            //        }
            //        //
            //        if (isEdited)
            //            break;
            //    }
            //}
            #endregion

            #region 80618-New Solution
            if ((items != null && items.Count > 0) || (origItems != null && origItems.Count > 0))
            {
                //Verify row count
                if (origItems == null || (origItems != null && origItems.Count != items.Count))
                    isEdited = true;
                //
                if (!isEdited)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        TlItemCapture item1 = origItems[i] as TlItemCapture;
                        foreach (FieldBase field in (items[i] as TlItemCapture).DbFields)
                        {
                            if (!field.IsNull && item1.GetFieldByXmlTag(field.XmlTag).Value != field.Value)
                            {
                                if (field.FieldType == FieldType.Char)
                                {
                                    fieldValue = field.Value.ToString().Trim();
                                    if (!item1.GetFieldByXmlTag(field.XmlTag).IsNull &&
                                        fieldValue != item1.GetFieldByXmlTag(field.XmlTag).Value.ToString().Trim())
                                        isEdited = true;
                                }
                                else
                                    isEdited = true;

                                //Begin #14448
                                if ( isEdited && item1.GetFieldByXmlTag(field.XmlTag).Value != null &&
                                    field.Value != null && item1.GetFieldByXmlTag(field.XmlTag).Value.Equals( field.Value ))
                                    isEdited = false;
                                //End #14448

                                //Begin #16656
                                if (isEdited && field.XmlTag == item1.FloatBal.XmlTag &&
                                    item1.GetFieldByXmlTag(field.XmlTag).Value == null &&
                                    field.Value != null && Convert.ToDecimal(field.Value) == 0M)
                                    isEdited = false;
                                //End #16656

                                if ( isEdited )     //#14448
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #endregion
            //
			return isEdited;

			#region do not call the base
			//return base.IsFormDirty ();
			#endregion
		}
		#endregion

		#region call parent
		public override void CallParent( params object[] paramList )
		{

			string caller = "";
			PGridColumn focusCol = null;
			string paramLocal;

			if ( paramList != null && paramList[0] != null )
				caller = Convert.ToString( paramList[0] );

            //Begin #140895-ReadOnly
            if (_tlTranSet != null && (_tlTranSet.IsTellerCaptureTran || _tlTranSet.IsTellerCaptureCheckOnlyTran) && (caller == "Calculator"))  //#102719
                return;
            //End #140895-ReadOnly

			if ( caller == "Calculator" )
			{
				if ( paramList.Length > 1 )
				{
					colAmount.UnFormattedValue = paramList[1];
					focusCol = colAmount;
				}
				if ( paramList.Length > 2 && Convert.ToString( colCheckType.UnFormattedValue ) != mlOnUs )
				{
					colNoOfItems.UnFormattedValue = paramList[2];
					focusCol = colNoOfItems;
				}
			}
			else if ( caller == Phoenix.Shared.Constants.ScreenId.GlAccountSearch.ToString() )
			{
				if ( paramList.Length > 3 )
				{
					gLSearchAcctNo = Convert.ToString( paramList[3] );
					focusCol = colCheckNo;
				}
			}
			else if (caller == Shared.Constants.ScreenId.GbAccountList.ToString())
			{
				#region Acct Type - paramList[1]
				paramLocal = Convert.ToString(paramList[1]);
				if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)
				{
					try
					{
						this._searchAcctType = paramLocal.Trim();
						this.colAcctNo.Text = string.Empty;
						this.colAcctNo.UnFormattedValue = string.Empty;
						this.colAcctType.Text = _searchAcctType;
						//
						ValidateAcctType();
						focusCol = colAcctNo;

					}
					catch(PhoenixException ex)
					{
						//We do not want to catch anything
						Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Acct Type:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
					}
				}
				#endregion
				//
				#region Acct No - paramList[2]
				paramLocal = Convert.ToString(paramList[2]);
				if 	( paramLocal != string.Empty && paramLocal.Trim().Length > 0)
				{
					try
					{
						this._searchAcctNo = paramLocal.Trim();
						this.colAcctNo.Text = _searchAcctNo;
						this.colAcctNo.UnFormattedValue = _searchAcctNo;

						//ValidateAcctNo();		#5020

						// #5020 - Call following event handler after returning from GbAccountList,
						// which will call ValidateAcctNo as above AND set the Title info ...
						colAcctNo_PhoenixUIValidateEvent(this, new PCancelEventArgs(0, 0));

						focusCol = colCheckNo;
					}
					catch(PhoenixException ex)
					{
						//We do not want to catch anything
						Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Acct No: Unknown Error:" + ex.Message + "\n" + ex.InnerException);
					}
				}
				#endregion
			}
			//
			if ( focusCol != null )
			{
				gridCapturedItems.Focus();
				focusCol.Focus();
			}
		}
		#endregion

		#region item events
		private ReturnType frmTlCapturedItems_PInitBeginEvent()
		{
            //Begin #75604. Teller Window. So always refer Decimal Config
            #region config
            PdfCurrency.ApplicationAssumeDecimal = (_tellerVars.AdTlControl.AssumeDecimals.Value == GlobalVars.Instance.ML.Y);
            CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            #endregion
            //End #75604
            this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = _busObjCapturedItems;
			this.ActionSave.NextScreenId = 0;
			this.colCheckType.AutoDrop = false;
			this.colAcctType.AutoDrop = false;
			this.colTranCode.AutoDrop = false;
			// The following will take care of check type decoding in view mode
			if (isViewOnly)
			{
				colCheckType.Append(CoreService.Translation.GetListItemX( ListId.CheckType, "C" ), CoreService.Translation.GetUserMessageX(360589));
				colCheckType.Append(CoreService.Translation.GetListItemX( ListId.CheckType, "O" ), CoreService.Translation.GetUserMessageX(360590));
				colCheckType.Append(CoreService.Translation.GetListItemX( ListId.CheckType, "T" ), CoreService.Translation.GetUserMessageX(360591));
			}
            this.pbSuspectDtls.NextScreenId = Phoenix.Shared.Constants.ScreenId.SuspiciousTranDetails; // #80660
            pbGetGlAcct.NextScreenId = Phoenix.Shared.Constants.ScreenId.TlGlAccountSearch; //#05081 - replace GlAccountSearch with TlGlAccountSearch for sec purpose only.
            SelectFirstControl = false; //#2970
			_isFraudPreventionEnabled = _pcCustomOptions.CheckCustomOptions(PcCustomOptions.CO_Constants.CO_FraudPrevention); //#9519
            //Begin #79510
            if (isViewOnly)
            {
                colExceptRsnCode.XmlTag = "ExceptRsnCodeDesc";
            }
            //End #79510

            //Begin #14448
            foreach (PGridColumn column in gridCapturedItems.Columns)
            {
                if (!column.ReadOnly)
                {
                    column.PhoenixUIEnterEvent += new EnterEventHandler(column_PhoenixUIEnterEvent);
                    if (firstColToFocus == null)
                        firstColToFocus = column;
                }

            }
            //foreach( PAction action in this.ActionManager.Actions )
            //{
            //    if (action.ImageButton != null)
            //    {
            //        action.ImageButton.GotFocus += new EventHandler(ImageButton_GotFocus);
            //        action.Click += new PActionEventHandler(action_Click);
            //    }
            //}
            dfMakeAvail.KeyDown += new KeyEventHandler(dfMakeAvail_KeyDown);
            //End #14448
			return ReturnType.Success;
		}







        //Begin #14448
        //void action_Click(object sender, PActionEventArgs e)
        //{
        //    imageButtonFocussed = false;
        //}
        //void ImageButton_GotFocus(object sender, EventArgs e)
        //{
        //    imageButtonFocussed = true;
        //}
        void column_PhoenixUIEnterEvent(object sender, PEventArgs e)
        {
            lastFocusCol = sender as GLColumn;
            if (lastFocusCol != null && glacialTable == null)
            {
                glacialTable = lastFocusCol.Parent;
            }
        }
        void dfMakeAvail_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e != null && !e.Alt && !e.Shift )
                makeAvailEdited = true;
        }
        //End #14448

		private void frmTlCapturedItems_PInitCompleteEvent()
		{
            try   // #140895
            {
                WriteToDebugLogDetailed("Begin frmTlCapturedItems_PInitCompleteEvent", true);   // #140895
                if (!isViewOnly)
                {
                    colAcctType.CodeValue = null;
                    //#2677
                    if (!_tlTranSet.CurTran.TranAcct.TranCode.IsNull &&
                        _tlTranSet.CurTran.TranAcct.TranCode.Value == 520)
                        PopulateAcctTypeCombo(colAcctType, false, false);
                    else
                        PopulateAcctTypeCombo(colAcctType, false, true);
                    WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:LoadTranCodeCombo begin", true);   // #140895
                    LoadTranCodeCombo(_busObjCapturedItems.GetTCComboList(null));
                    WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:LoadTranCodeCombo end", true);   // #140895
                    //
                    if (_tlTranSet.AdTlTc.ChkAsCashEnable.Value == GlobalVars.Instance.ML.Y)
                        colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "C"), CoreService.Translation.GetUserMessageX(360589));
                    if (_tlTranSet.AdTlTc.OnUsChksEnable.Value == GlobalVars.Instance.ML.Y)
                        colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "O"), CoreService.Translation.GetUserMessageX(360590));
                    if (_tlTranSet.AdTlTc.TransitChkEnable.Value == GlobalVars.Instance.ML.Y)
                        colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "T"), CoreService.Translation.GetUserMessageX(360591));
                    //
                    //Begin #79420
                    if (_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline)
                    {
                        if (_tlTranSet.TellerVars.ComboCache["frmTlCapturedItems.colExceptRsnCode"] == null)
                        {
                            CallXMThruCDS("MiscComboPopulate");
                            _tlTranSet.TellerVars.ComboCache["frmTlCapturedItems.colExceptRsnCode"] = _busObjCapturedItems.ExceptRsnCode.Constraint.EnumValues;
                        }
                        colExceptRsnCode.Append(_tlTranSet.TellerVars.ComboCache["frmTlCapturedItems.colExceptRsnCode"]);
                    }
                    //End #79420

                    LoadItems(true, false, null);
                    CalcTotal();
                    WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:CalcTotal end", true);   // #140895
                    //
                    foreach (EnumValue enumVal in _tellerHelper.AcctType.Constraint.EnumValues)
                    {
                        if (_validAcctTypes == "")
                            _validAcctTypes = _validAcctTypes + enumVal.CodeValue.ToString();
                        else
                            _validAcctTypes = _validAcctTypes + "~" + enumVal.CodeValue.ToString();
                    }
                    //
                    if (gridCapturedItems.Count > 0)
                    {
                        gridCapturedItems.SelectRow(0, true);
                        dfItemNo.UnFormattedValue = gridCapturedItems.ContextRow + 1;
                    }

                    for (int i = 0; i < this.gridCapturedItems.Columns.Count; i++)  //#80618
                    {
                        (this.gridCapturedItems.Columns[i] as PGridColumn).SortOrder = SortOrder.None;
                    }
                }
                else
                {
                    //#16995
                    if (colTranCode.UnFormattedValue != null &&
                        Convert.ToInt32(colTranCode.UnFormattedValue) == 520)
                        PopulateAcctTypeCombo(colAcctType, false, false);
                    else
                        PopulateAcctTypeCombo(colAcctType, false, true);
                }
                //
                #region Initialize
                EnableDisableVisibleLogic("ActionCopyAndRemove");
                EnableDisableVisibleLogic("FormComplete");
                this.colCheckType.AutoDrop = false;
                this.colAcctType.AutoDrop = false;
                this.colTranCode.AutoDrop = false;
                EnableDisableVisibleLogic("CrossRefAccounts");  //#4450
                #endregion
                //
                if (!isViewOnly && items.Count >= 0)        //#80618
                {
                    AddNewRow();
                }
                this.colCheckNo.PhoenixUIControl.InputMask = "9999999999";
                //
                if (isViewOnly)
                {
                    if (_tlTranSet == null)
                    {
                        _tlTranSet = new TlTransactionSet();
                    }
                    if (_tlJournal == null)
                        _tlJournal = new TlJournal();
                    //
                    //this.OnUsInformation();   //#16995
                }

                // Begin #74015
                if (gridCapturedItems.ContextRow >= 0)
                {
                    //Begin #76458
                    if (GetExternalMaskedAcct(colAcctType.Text, colAcctNo.Text, colAcctApplType.Text) == null)
                        this._busObjCapturedItems.AcctNo.Value = colAcctNo.Text;
                    else
                    {
                        if (!isViewOnly)
                            this._busObjCapturedItems.AcctNo.Value = (items[gridCapturedItems.ContextRow] as TlItemCapture).AcctNo.Value;
                        else
                            this._busObjCapturedItems.AcctNo.Value = colRealAcctNo.Text;
                    }
                    this._busObjCapturedItems.AcctType.Value = colAcctType.Text;
                    //End #76458
                }
                this.OnUsInformation();
                ((Workspace as PwksWindow).WksExtension as Phoenix.Shared.Windows.WkspaceExtension).RefreshAlerts();
                this.EnableDisableVisibleLogic("Alerts");
                _isGridLoaded = true;
                //End #74015

                //Begin #79420
                decimal nxtDayAmtAvail = 0;
                decimal nxtDayAmtUsed = 0;
                string regCcCode = null;
                int regCcPtid = 0;
                int retCode = 0;
                decimal makeAvl = 0;

                if (isViewOnly)
                {
                    try
                    {
                        _tlJournal.BranchNo.Value = branchNo.Value;
                        _tlJournal.DrawerNo.Value = drawerNo.Value;
                        _tlJournal.EffectiveDt.Value = effectiveDt.Value;
                        _tlJournal.Ptid.Value = journalPtid.Value;
                        _tlJournal.ActionType = XmActionType.Select;
                        _tlJournal.OutputType.Value = 4;	// #72362 - this will get TC desc
                        _tlJournal.SelectAllFields = false;
                        _tlJournal.RegCcCode.Selected = true;
                        _tlJournal.RegCcPtid.Selected = true;
                        _tlJournal.NxtDayAmt.Selected = true;
                        _tlJournal.MakeAvl.Selected = true;
                        _tlJournal.AcctType.Selected = true;
                        _tlJournal.AcctNo.Selected = true;
                        _tlJournal.TranCode.Selected = true;
                        _tlJournal.RimNo.Selected = true;           // #161243

                        CallXMThruCDS("LoadCurTran");

                    }
                    finally
                    {
                        _tlJournal.SelectAllFields = false;
                    }

                    _tlTranSet.CurTran.RegCcCode.Value = _tlJournal.RegCcCode.Value;
                    _tlTranSet.CurTran.RegCcPtid.Value = _tlJournal.RegCcPtid.Value;
                    _tlTranSet.CurTran.NxtDayAmt.Value = _tlJournal.NxtDayAmt.Value;
                    _tlTranSet.CurTran.MakeAvl.Value = _tlJournal.MakeAvl.Value;
                    _tlTranSet.CurTran.TranCode.Value = _tlJournal.TranCode.Value;
                    _tlTranSet.CurTran.TranAcct.AcctType.Value = _tlJournal.AcctType.Value;
                    _tlTranSet.CurTran.TranAcct.AcctNo.Value = _tlJournal.AcctNo.Value;
                    // Begin #161243
                    _tlTranSet.CurTran.TranAcct.RimNo.Value = _tlJournal.RimNo.Value;
                    AcctTypeDetail acctTypeDetail = _gbHelper.GetAcctTypeDetails(_tlTranSet.CurTran.TranAcct.AcctType.Value, null);
                    if (acctTypeDetail != null)
                    {
                        _tlTranSet.CurTran.TranAcct.DepLoan.Value = acctTypeDetail.DepLoan;
                        _tlTranSet.CurTran.TranAcct.ApplType.Value = acctTypeDetail.ApplType;
                    }
                    // End #161243
                }

                if (_busObjCapturedItems.EffectiveDt.IsNull)
                    _busObjCapturedItems.EffectiveDt.Value = _tlTranSet.EffectiveDt.Value;

                // Begin #79420, #13851
                if (!isViewOnly && !_tlTranSet.CurTran.TranEffectiveDt.IsNull)
                    _busObjCapturedItems.TranEffectiveDt.Value = _tlTranSet.CurTran.TranEffectiveDt.Value;
                // End #79420, #13851

                if (!_tlTranSet.CurTran.RegCcCode.IsNull)
                    regCcCode = _tlTranSet.CurTran.RegCcCode.Value;
                if (!_tlTranSet.CurTran.RegCcPtid.IsNull)
                    regCcPtid = _tlTranSet.CurTran.RegCcPtid.Value;

                if (_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline &&
                    _busObjCapturedItems.PopulateFloatInfo(_tlTranSet.TellerHelper, _tlTranSet.CurTran.TranCode.Value))
                {
                    if (_busObjCapturedItems.GetAcctRegDetails(_tlTranSet.CurTran.TranAcct.AcctType.Value,
                        _tlTranSet.CurTran.TranAcct.AcctNo.Value, _tlTranSet.CurTran.TranCode.Value, ref regCcCode, ref regCcPtid, ref nxtDayAmtAvail,
                        ref nxtDayAmtUsed, ref retCode))
                    {
                        if (!isViewOnly)
                        {
                            _tlTranSet.CurTran.RegCcCode.Value = regCcCode;
                            _tlTranSet.CurTran.RegCcPtid.Value = regCcPtid;
                            _tlTranSet.CurTran.NxtDayAmt.Value = nxtDayAmtUsed;
                        }
                        else
                        {
                            regCcCode = _tlTranSet.CurTran.RegCcCode.Value;
                            regCcPtid = _tlTranSet.CurTran.RegCcPtid.Value;
                            if (!_tlTranSet.CurTran.NxtDayAmt.IsNull)
                                nxtDayAmtUsed = _tlTranSet.CurTran.NxtDayAmt.Value;
                            else
                                nxtDayAmtUsed = 0.0M;
                        }
                    }
                    else
                    {
                        ProcessRetCode(retCode);
                    }
                }
                if (!_tlTranSet.CurTran.MakeAvl.IsNull)
                    makeAvl = _tlTranSet.CurTran.MakeAvl.Value;
                else if (!_tlTranSet.CurTran.CalcMakeAvl.IsNull)
                    makeAvl = _tlTranSet.CurTran.CalcMakeAvl.Value;
                else
                {
                    makeAvl = nxtDayAmtAvail > nxtDayAmtUsed ? nxtDayAmtAvail - nxtDayAmtUsed : 0.0M;
                    makeAvl = 0.0M;
                }
                WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:Executing Float code", true);   // #140895
                dfMakeAvail.SetValue(makeAvl);
                dfImmediateAvailAmt.SetValue(nxtDayAmtAvail);
                dfImmediateAvailUsed.SetValue(nxtDayAmtUsed);
                dfAvailRemaining.SetValue(Convert.ToDecimal(dfImmediateAvailAmt.Value) - Convert.ToDecimal(dfImmediateAvailUsed.Value) - (Convert.ToDecimal(dfMakeAvail.Value)));
                if (Convert.ToDecimal(dfAvailRemaining.Value) < 0)  //#42006
                {
                    dfAvailRemaining.SetValue(Convert.ToDecimal(dfAvailRemaining.Value) * -1);  //display it as a positive value
                }
                if (!string.IsNullOrEmpty(regCcCode))
                {
                    // WI#14665 - change "Under Reg Reg CC" to "Under Reg CC" and change "IndexOf... >0" to >=0 ("Emergency" conditions wasn't being found)
                    dfRegCcCode.SetValue(CoreService.Translation.GetListDecodeValueX(ListId.RegCC, regCcCode) +
                        ("ENOR".IndexOf(regCcCode) >= 0 ? " Under Reg CC" : null));
                }

                if (_adGbBankControl.ExpireAmtToday.Value == "1")
                {
                    lblImmediateAvailAmt.Text = lblImmediateAvailAmt.Text.Replace("Immediate", "Next-Day");
                    lblImmediateAvailUsed.Text = lblImmediateAvailUsed.Text.Replace("Immediate", "Next-Day");
                }
                gridCapturedItems.Focus();

                // Begin #79420, #13851
                this.pbFloatInfo.NextScreenId = -1;
                EnableDisableVisibleLogic("FloatInfo");
                // End #79420, #13851
                SetReqExceptRsnRowColor(-1);  //#79420, #15357

                //Begin #79420, #14926
                if (regCcCode == "N")
                {
                    lblImmediateAvailAmt.Text = lblImmediateAvailAmt.Text + "*";
                    lblImmediateAvailUsed.Text = lblImmediateAvailUsed.Text + "*";
                    lblNewRegCc.Text = "* Avail Amt and Amt Used are the combined amounts for Checks as Cash and New Account Checks";
                    lblNewRegCc.ForeColor = dfImmediateAvailAmt.ForeColor;
                }
                else
                {
                    lblNewRegCc.Text = string.Empty;
                }
                //End #79420, #14926

                //Begin #79420, #15357
                //This to fetch the routing details since the routing details can be fetched only after the account reg details are fetched which is happening earlier.
                if (!isViewOnly && items.Count >= 0 && !string.IsNullOrEmpty(_tellerVars.DefaultShortCode))
                {
                    if (!IsCreatingTlCaptureTranSet && !_tlTranSet.IsTellerCaptureTran && !_tlTranSet.IsTellerCaptureCheckOnlyTran)    //#140895    #102719
                        ValidateGrid(colBankCode.XmlTag, true);
                }
                //End #79420, #15357

                //End #79420
            }
            //Begin #140895
            finally
            {
                //if invoked while creating AVTC transaction , then silently close it
                if (IsCreatingTlCaptureTranSet)
                {
                    WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:Executing Teller capture code", true);
                    CalculateNxtDayAvl(-2, true, true, null);
                    WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:Executed CalculateNxtDayAvl", true);
                    AvoidSave = true;
                    if (OnActionSave(false))
                    {
                        _firstNotesRow = -1;
                        WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:Executed OnActionSave", true);

                        // handle notes for on us items
                        if (Convert.ToDecimal(dfTotalOnUsChecks.UnFormattedValue) > 0)
                        {
                            //_workSpaceCurForm = this.Workspace.CurrentWindow;
                            for (int i = 0; i < gridCapturedItems.Count; i++)
                            {
                                gridCapturedItems.ContextRow = i;
                                ValidateAcctNo();
                                if (_firstNotesRow >= 0)
                                    break;
                            }
                        }

                        if (_firstNotesRow < 0)
                        {
                            WriteToDebugLogDetailed("frmTlCapturedItems_PInitCompleteEvent:Executing OnActionClose", true);
                            base.OnActionClose();
                        }
                        //base.OnActionClose();
                    }
                    else
                    {
                        IsTlCaptureTranSetItemValFailed = true;
                    }
                }
            }
            //End #140895
		}

        #region #11300
        void frmTlCapturedItems_Shown(object sender, EventArgs e)
        {
            if (gridCapturedItems.Count > 0)
            {
                /* Get note info and enable/disable pbOnUsNotes  */
                //if (colCheckType.UnFormattedValue.ToString() == mlOnUs && _tlTranSet.TellerVars.AdTlCls.ShowOnusAcctNotes.Value == "Y")
                if (colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() == mlOnUs
                    && _tlTranSet.TellerVars.IsAppOnline)  // #12725    //#13192
                {
                    string applType = null;
                    string depLoan = null;
                    string format = null;
                    // #17290
                    if (colAcctType.CodeValue == null && colAcctType.Text != "")
                        colAcctType.CodeValue = colAcctType.Text;
                    GetAcctTypeDetails(colAcctType, ref applType, ref depLoan, ref format);
                    colDepLoan.Text = depLoan;
                    TlAcctDetails acctDetails = new TlAcctDetails();

                    acctDetails.AcctNo.Value = this.colAcctNo.Text;
                    acctDetails.AcctType.Value = this.colAcctType.Text;

                    acctDetails.ApplType.Value = this.colAcctApplType.Text;
                    acctDetails.DepLoan.Value = this.colDepLoan.Text;
                    acctDetails.CustomType.Value = 1;
                    acctDetails.TfrRefAccount.Value = 1;
                    acctDetails.SelectAllFields = true;

                    acctDetails.ActionType = XmActionType.Select;
                    CoreService.DataService.ProcessRequest(acctDetails);
                    Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(acctDetails, "LoadTfrNotePtids");

                    NoteInfo = new PNotes();
                    NoteInfo.ScreenPtid = acctDetails.RimNo.DecimalValue;
                    NoteInfo.ScreenId = this.ScreenId;
                    NoteInfo.AcctNo = colAcctNo.Text;
                    NoteInfo.AcctType = colAcctType.Text;
                    NoteInfo.RimNo = acctDetails.RimNo.IntValue;

                    string[] notesList = null;
                    int notesCount = 0;
                    ShowNotes = false;
                    pbOnUsNotes.Enabled = false;
                    FormExtension t = Extension as FormExtension;

                    if (!acctDetails.CustomParam1.IsNull && _tlTranSet.TellerVars.AdTlCls.ShowOnusAcctNotes.Value == "Y")
                    {
                        notesCount = acctDetails.CustomParam2.IntValue + acctDetails.CustomParam3.IntValue;
                        notesList = acctDetails.CustomParam1.StringValue.Split(',');
                        if (notesList.Length > 0)
                        {
                            pbOnUsNotes.Enabled = true;
                            ShowNotes = true;
                            t.IsNotesAvailable = true;
                        }
                        else
                        {
                            pbOnUsNotes.Enabled = false;
                            t.IsNotesAvailable = false;
                        }
                    }
                    this.EnableDisableVisibleLogic("Alerts");
                }
            }
        }
        #endregion

        private void frmTlCapturedItems_PShowCompletedEvent(object sender, EventArgs e)  //#79314
        {
            WriteToDebugLogDetailed("frmTlCapturedItems_PShowCompletedEvent:Starting..", true);   // #140895
            ResetFormForSupViewOnlyMode();
            //Begin #14448
            if (!isViewOnly && gridCapturedItems.ContextRow >= 0 && newRowAdded )
            {
                gridCapturedItems.SelectRow(gridCapturedItems.ContextRow, true);
                int columnIndex = -1;
                if (_tellerVars.ReorderAmtItems)    //#80618
                    columnIndex = gridCapturedItems.Columns.GetColumnIndex("Amount");
                else
                    columnIndex = gridCapturedItems.Columns.GetColumnIndex("BankCode");
                if (columnIndex > -1)
                    gridCapturedItems.Columns[columnIndex].Focus();

            }
            //End #14448

            //Begin #140895
            if (IsTlCaptureTranSetItemValFailed)
            {
                WriteToDebugLogDetailed("frmTlCapturedItems_PShowCompletedEvent:Executed IsTlCaptureTranSetItemValFailed", true);
                EnableDisableVisibleLogic("FormComplete");
            }
            else if (IsCreatingTlCaptureTranSet)
            {
                WriteToDebugLogDetailed("frmTlCapturedItems_PShowCompletedEvent:Executed IsCreatingTlCaptureTranSet", true);
                if (_firstNotesRow >= 0)
                {
                    gridCapturedItems.ContextRow = _firstNotesRow;
                    TlCaptureShowNotes();
                }
            }
            //End #140895

            EnableDisableVisibleLogic("ShowComplete");      // #161243
        }

		private void gridCapturedItems_BeforePopulate(object sender, GridPopulateArgs e)
		{
			if (!isViewOnly)
				gridCapturedItems.SkipPopulate = true;
			gridCapturedItems.ListViewObject = _busObjCapturedItems;
		}

        void gridCapturedItems_FetchRowDone(object sender, GridRowArgs e)    //#76458
        {
            if (isViewOnly)
            {
                if (colAcctNo.UnFormattedValue != null && colCheckType.Text == "On-Us" &&
                    colTranCode.Text == "570")
                {
                    colRealAcctNo.Text = colAcctNo.Text;
                    colRealAcctNo.UnFormattedValue = colAcctNo.Text;
                    colAcctNo.Text = _gbHelper.GetMaskedExtAcct(colAcctType.Text, colAcctNo.Text);
                }

				// begin WI#14665
				colRowCount.UnFormattedValue = gridCapturedItems.Count;	// increment row count
				if (colBankCode.UnFormattedValue == null)
				{
					string bankCode = null;
					string routingNo = Convert.ToString(colRoutingNo.UnFormattedValue);
					string checkType = null;
					string chksAsCash = null;

					// lookup bank code using existing helper method
					if (_tellerHelper.RoutingNoLookUp(_tellerVars.AdGbFloatArray, ref bankCode, ref routingNo, ref checkType, ref chksAsCash))
					{
						colBankCode.UnFormattedValue = bankCode;
					}
				}
				// end WI#14665
            }
        }

		private void gridCapturedItems_AfterPopulate(object sender, GridPopulateArgs e)
		{
			EnableDisableVisibleLogic("ActionCopyAndRemove");
			if (isViewOnly)
			{
				CalcTotal();
			}
			if (gridCapturedItems.Count > 0)
			{
				gridCapturedItems_SelectedIndexChanged(sender, new GridClickEventArgs(0, 0));	// WI#14665 (2)
				//gridCapturedItems.SelectRow(0, true);											// WI#14665 (2)
				//dfItemNo.UnFormattedValue = gridCapturedItems.ContextRow + 1;
				//dfItemNo.UnFormattedValue = 0;												// WI#14665 - SelectedIndexChanged setting this value
			}
		}

		private void gridCapturedItems_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
            //Begin #14448
            //if (validatingItemOnSelChanged)
            //    return;
            //if (e != null && lastSelectedRowId == e.RowId)
            //{
            //    lastSelectedRowId = -1;
            //    return;
            //}
            //End #14448

            try
            {
                if (gridCapturedItems.ContextRow == -1 && gridCapturedItems.Count > 0)
                    gridCapturedItems.SelectRow(0, false);
                //#80618
                //else
                //{
                //    if (!isViewOnly) //#71452
                //    {
                //        if (gridCapturedItems.ContextRow != gridCapturedItems.Count - 1 && !_IsFromSelectRow)
                //        {
                //            if (!EndOfRowValidation())
                //                return;
                //        }
                //    }
                //}
                //if (gridCapturedItems.Count > 0)
                //    dfItemNo.UnFormattedValue = e.RowId + 1; //#73972 - bumped with 1
                //else
                //    dfItemNo.Text = string.Empty;
                else
                {
                    if (!isViewOnly) //#71452
                    {
                        //Begin #14448
                        // The code below has been commented because it was not validating the row just left but trying to validate the current row( row getting focus )
                        // already validated which is of no use.Even if the current row has some invalid stuff , then it will be figured out when
                        // save is hit.
                        //if (gridCapturedItems.ContextRow != gridCapturedItems.Count - 1 && !_IsFromSelectRow)
                        //{
                        //    if (!EndOfRowValidation(-1, true))  // #79420 - Added the parameters
                        //        return;
                        //}
                        if (gridCapturedItems.ContextRow < items.Count && !_IsFromSelectRow)
                        {
                            _busObjCapturedItems = items[gridCapturedItems.ContextRow] as TlItemCapture;
                        }
                        //Begin #14448
                        //try
                        //{
                        //    validatingItemOnSelChanged = true;
                        //    if (unValidatedRowId >= 0)
                        //    {
                        //        if (!ValidateSingleRow(unValidatedRowId))
                        //            return;
                        //        else
                        //            unValidatedRowId = -1;
                        //    }

                        //}
                        //finally
                        //{
                        //    validatingItemOnSelChanged = false;
                        //}
                        //End #14448

                    }
                }
                if (gridCapturedItems.Count > 0)
                    dfItemNo.UnFormattedValue = e.RowId + 1; //#73972 - bumped with 1
                else
                    dfItemNo.Text = string.Empty;

                //#74782 - check type fix -- hack
                if (!isViewOnly && gridCapturedItems.Count > 0 && _isGridLoaded && !_IsFromSelectRow &&
                    gridCapturedItems.ContextRow <= gridCapturedItems.Count - 2 &&
                    items != null && items.Count >= gridCapturedItems.ContextRow
                    && items.Count > 0)    // #140895 - check for count
                {
                    _busObjCapturedItems = items[gridCapturedItems.ContextRow] as TlItemCapture;

                    //Begin #80618
                    if (!isViewOnly) //#71452
                    {
                        //Begin #14448
                        // The code below has been commented because it was not validating the row just left but trying to validate the current row( row getting focus )
                        // already validated which is of no use.Even if the current row has some invalid stuff , then it will be figured out when
                        // save is hit.
                        //if (gridCapturedItems.ContextRow != gridCapturedItems.Count - 1 && !_IsFromSelectRow)
                        //{
                        //    if (!EndOfRowValidation(-1, true))  // #79420 - Added the parameters
                        //        return;
                        //}
                        //End #14448
                    }

                    if (gridCapturedItems.Count > 0)
                        dfItemNo.UnFormattedValue = e.RowId + 1; //#73972 - bumped with 1
                    else
                        dfItemNo.Text = string.Empty;
                    //End #80618

                    if (_busObjCapturedItems.CheckType.Value == mlOnUs)
                    {
                        gridCapturedItems.SetCellValueFormatted(gridCapturedItems.ContextRow, colAcctType.ColumnId, _busObjCapturedItems.AcctType.Value);
                        colCheckType.Text = CoreService.Translation.GetUserMessageX(360590);
                        colCheckType.CodeValue = mlOnUs;
                    }
                    else if (_busObjCapturedItems.CheckType.Value == mlTransit)
                    {
                        colCheckType.Text = CoreService.Translation.GetUserMessageX(360591);
                        colCheckType.CodeValue = mlTransit;
                    }
                    else
                    {
                        colCheckType.Text = CoreService.Translation.GetUserMessageX(360589);
                        colCheckType.CodeValue = mlChksAsCash;
                    }
                }
                // Begin #74015
                if (gridCapturedItems.Count > 0 && _isGridLoaded && !_IsFromSelectRow && _tlTranSet.TellerVars.IsAppOnline) //#13192
                {
                    #region #80620
                    /* Get note info and enable/disable pbOnUsNotes  */
                    if (colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() == mlOnUs && _tlTranSet.TellerVars.AdTlCls.ShowOnusAcctNotes.Value == "Y")    //#80618
                    {
                        #region #11300
                        string applType = null;
                        string depLoan = null;
                        string format = null;
                        // #17290
                        if (colAcctType.CodeValue == null && colAcctType.Text != "")
                            colAcctType.CodeValue = colAcctType.Text;
                        GetAcctTypeDetails(colAcctType, ref applType, ref depLoan, ref format);
                        colDepLoan.Text = depLoan;
                        #endregion
                        TlAcctDetails acctDetails = new TlAcctDetails();

                        acctDetails.AcctNo.Value = this.colAcctNo.Text;
                        acctDetails.AcctType.Value = this.colAcctType.Text;

                        acctDetails.ApplType.Value = this.colAcctApplType.Text;
                        acctDetails.DepLoan.Value = this.colDepLoan.Text;
                        acctDetails.CustomType.Value = 1;
                        acctDetails.TfrRefAccount.Value = 1;
                        acctDetails.SelectAllFields = true;

                        acctDetails.ActionType = XmActionType.Select;
                        CoreService.DataService.ProcessRequest(acctDetails);
                        #region #11412
                        Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(acctDetails, "LoadTfrNotePtids");

                        NoteInfo = new PNotes();
                        NoteInfo.ScreenPtid = acctDetails.RimNo.DecimalValue;
                        NoteInfo.ScreenId = this.ScreenId;
                        NoteInfo.AcctNo = colAcctNo.Text;
                        NoteInfo.AcctType = colAcctType.Text;
                        NoteInfo.RimNo = acctDetails.RimNo.IntValue;
                        #endregion

                        string[] notesList = null;
                        int notesCount = 0;
                        ShowNotes = false;
                        pbOnUsNotes.Enabled = false;
                        FormExtension t = Extension as FormExtension; // #11412

                        if (!acctDetails.CustomParam1.IsNull)
                        {
                            notesCount = acctDetails.CustomParam2.IntValue + acctDetails.CustomParam3.IntValue;
                            notesList = acctDetails.CustomParam1.StringValue.Split(',');
                            if (notesList.Length > 0)
                            {
                                pbOnUsNotes.Enabled = true;
                                ShowNotes = true;
                                t.IsNotesAvailable = true; // #11300
                            }
                            else
                            {
                                pbOnUsNotes.Enabled = false;
                                t.IsNotesAvailable = false; // #11412
                            }
                        }
                    }
                    #endregion #80620
                    this.EnableDisableVisibleLogic("Alerts");
                    this.OnUsInformation();
                }
                // End #74015
                // Begin #79420, #13851
                EnableDisableVisibleLogic("FloatInfo");
                // End #79420, #13851
                #region #80660
                //if (isViewOnly)
                EnableDisableVisibleLogic("SuspectTran");
                #endregion
            }
            finally
            {
                //if (unValidatedRowId >= 0 && _focusField != null)
                //{
                //    _IsFromSelectRow = true;
                //    gridCapturedItems.SelectRow(unValidatedRowId, true);
                //    _IsFromSelectRow = false;
                //    gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)].Focus();   //#80618
                //    //gridCapturedItems.NextFocusColumn =
                //    //    gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)]; //#80618
                //}
                ////Begin #14448
                //if (e != null)
                //    lastSelectedRowId = e.RowId;
                ////End #14448
            }
		}

        /* Begin Task#41151 */
        void hPanelScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (Convert.ToString(colCheckType.UnFormattedValue) != mlOnUs && Convert.ToInt16(colNoOfItems.UnFormattedValue) > 255)
            {
                GLColumn column = this.gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag)];
                column.ActivateCell(gridCapturedItems.ContextRow, gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag));
                glacialTable = column.Parent;

                System.Reflection.MethodInfo privMethod = glacialTable.GetType().GetMethod("ActivateEmbeddedControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                privMethod.Invoke(glacialTable, new object[] { gridCapturedItems.ContextRow, gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag) });
                return;
            }            
        }
        /* End Task#41151 */

        ///*Begin #50074 -    There is an issue, after enetering the value in NoOfItems column which is greater than 255 and click on the scrollbar after presenting the error #360984 the window may get hanged.
        //                    this code is added to get the focus to the NoOfItems column after presenting the error #360984.*/
        //void hPanelScrollBar_Scroll(object sender, ScrollEventArgs e)
        //{
        //    using (new WaitCursor())
        //    {
        //        if (Convert.ToString(colCheckType.UnFormattedValue) != mlOnUs && Convert.ToInt16(colNoOfItems.UnFormattedValue) > 255)
        //        {
        //            GLColumn column = this.gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag)];
        //            column.ActivateCell(gridCapturedItems.ContextRow, gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag));
        //            glacialTable = column.Parent;

        //            System.Reflection.MethodInfo privMethod = glacialTable.GetType().GetMethod("ActivateEmbeddedControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //            privMethod.Invoke(glacialTable, new object[] { gridCapturedItems.ContextRow, gridCapturedItems.Columns.GetColumnIndex(colNoOfItems.XmlTag) });
        //            return;
        //        }
        //    }
        //}
        ///*End #50074*/

        #region pbSuspectDtls_Click - #80660
        void pbSuspectDtls_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("SuspectDetail");
        }
        #endregion

		private void pbNewRow_Click(object sender, PActionEventArgs e)
		{
			 /*  begin wi#4168*/
             if (gridCapturedItems.ContextRow >= 0)
		     {
		        if (EndOfRowValidation())
		       {
		         LoadItems(false, false, null);
		         CalcTotal();
		         if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)
		             AddNewRow();
		        }
		     }
		     else
			{
			   AddNewRow();
            }
			/*  end wi#4168*/
		}

		private void pbCopy_Click(object sender, PActionEventArgs e)
		{
			if (!IsBlankRow(gridCapturedItems.ContextRow))
			{
				if (EndOfRowValidation())
				{
					ContextRowScreenToObject(gridCapturedItems.ContextRow, true);
					LoadItems(true, true, _busObjCapturedItems.CopyItem());
				}
			}
		}

		private void pbRemove_Click(object sender, PActionEventArgs e)
		{
            if (gridCapturedItems.Count <= 0)   //#80618
            {
                EnableDisableVisibleLogic("ActionCopyAndRemove");
                EnableDisableVisibleLogic("OnUs");
            }
            else
            {
                if (gridCapturedItems.ContextRow >= 0)
                {
                    if (colIndex.UnFormattedValue != null)
                    {
                        #region remove from structure
                        if (Convert.ToInt32(colIndex.UnFormattedValue) > 0)
                        {
                        	//Begin #79420
                        	//items.RemoveAt(Convert.ToInt32(colIndex.UnFormattedValue) - 1);
                        	if ( items.Count > gridCapturedItems.ContextRow )
                            	items.RemoveAt(gridCapturedItems.ContextRow);
                        	//End #79420
                        }
                        else if ((Convert.ToInt32(colIndex.UnFormattedValue) == 0))//else part added for WI4168
                        {
                            items.RemoveAt(Convert.ToInt32(colIndex.UnFormattedValue));
                        }
                        #endregion
                    }
                    //#72095
                    if (gridCapturedItems.RemoveRow(gridCapturedItems.ContextRow))
                        ResetGridIndex();
                    _IsFromSelectRow = true;
                    gridCapturedItems.SelectRow(gridCapturedItems.ContextRow);
                    _IsFromSelectRow = false;
                    //
                    CalcTotal();
                	//Begin #79420
                	CalculateNxtDayAvl(-2, true, true, null);
                	//End #79420
                }
                //
                if (gridCapturedItems.Count <= 0)   //#80618
                {
                    EnableDisableVisibleLogic("ActionCopyAndRemove");
                    EnableDisableVisibleLogic("OnUs");
                }
            }
            //if ( gridCapturedItems.Count > 0 )
            //    gridCapturedItems.SelectRow(gridCapturedItems.Count - 1 );  // #14865
            gridCapturedItems.Focus();  // #14865
		}

		private void pbReprint_Click(object sender, PActionEventArgs e)
		{
			if (LoadPrintObject(false))
			{
				LoadItemPrintInfo();
				HandlePrinting();
			}
		}

		private void pbReprintAll_Click(object sender, PActionEventArgs e)
		{
			bool rePrint = false;
			bool skipChecks = false;
			//
			if (LoadPrintObject(true))
			{
				int nRow = 0;
				if (gridCapturedItems.Count > 0)
				{
					while (nRow < gridCapturedItems.Count)
					{
                        _IsFromSelectRow = true;
                        gridCapturedItems.SelectRow(nRow, false);
                        _IsFromSelectRow = false;
						//
						_tempChkAmount = colAmount.MakeFormattedValue(colAmount.UnFormattedValue);
						_tempChkAmount = _tempChkAmount.PadLeft(14, '*');
						_tempChkAmount = _tempChkAmount.Replace("*", " ");
						_checkItemInfo = colItemNo.UnFormattedValue.ToString() + " - " + colCheckType.Text + _tempChkAmount;
						_checkPrintNo = Convert.ToInt16(colItemNo.UnFormattedValue);
						//
						LoadItemPrintInfo();
						//
						CallOtherForms("PrintForms");
						//
						rePrint = dialogResult == DialogResult.Retry;
						skipChecks = dialogResult == DialogResult.Abort;
						//
						if (rePrint)
							nRow = nRow - 1;
						else
							nRow = nRow + 1;
						//
						if (skipChecks)
							break;
					}
				}
			}
		}

        // Begin #74015
        private void pbDisplay_Click(object sender, PActionEventArgs e)
        {
            this.CallOtherForms("DisplayClick");
        }

        private void pbSignature_Click(object sender, PActionEventArgs e)
        {
            this.CallOtherForms("SignatureClick");
        }
        // End #74015

        #region #80620

        void pbOnUsNotes_Click(object sender, PActionEventArgs e)
        {
            TlAcctDetails acctDetails = new TlAcctDetails();

            acctDetails.AcctNo.Value = this.colAcctNo.Text;
            acctDetails.AcctType.Value = this.colAcctType.Text;

            acctDetails.ApplType.Value = this.colAcctApplType.Text;
            acctDetails.DepLoan.Value = this.colDepLoan.Text;
            acctDetails.CustomType.Value = 1;
            acctDetails.TfrRefAccount.Value = 1;
            acctDetails.SelectAllFields = true;

            acctDetails.ActionType = XmActionType.Select;
            CoreService.DataService.ProcessRequest(acctDetails);

            int[] notesList = null;
            int notesCount = 0;
            bool showNotes = true;
            if (Phoenix.FrameWork.CDS.DataService.Instance.PrimaryDbAvailable == true)
                notesCount = _tlTranSet.GetOnUsNotesList(ref showNotes, ref notesList);
            else
                notesCount = 0;
            #region #11412
            NoteInfo = new PNotes();
            NoteInfo.ScreenPtid = acctDetails.RimNo.DecimalValue;
            NoteInfo.ScreenId = this.ScreenId;
            NoteInfo.AcctNo = colAcctNo.Text;
            NoteInfo.AcctType = colAcctType.Text;
            NoteInfo.RimNo = acctDetails.RimNo.IntValue;
            #endregion

            Extension.ShowNotes(this);
        }

        void pbOnUsRestrictions_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("OnUsRestrictionsClick");
        }
        #endregion

        #region Grid column Validate
        private void colBankCode_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
            //Begin #14865
            if (_IsFromSelectRow)       // getting fired due to SelectRow in LoadItems when ProcessCmdKey is fired
                return;
            //End #14865
            if (ClearRow(colBankCode)) //#3714
            {
                if (!ValidateGrid(colBankCode.XmlTag, true))
                    e.Cancel = true;

                if (!isViewOnly && colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() != mlOnUs) //Selva - #76195
                    this.LoadItems(); //#3714
            }

            //Begin #14448
            if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)
            {
                newRowAdded = false;
                valBankCodeToFocus = false; //#14789
            }
            CalcTotal();
            //End #14448
		}

		private void colRoutingNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
            //Begin #14865
            if (_IsFromSelectRow)       // getting fired due to SelectRow in LoadItems when ProcessCmdKey is fired
                return;
            //End #14865
            if (ClearRow(colRoutingNo)) //#3714
            {
                if (!ValidateGrid(colRoutingNo.XmlTag, true))
                    e.Cancel = true;

                if (!isViewOnly && colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() != mlOnUs) //Selva - #76195
                    this.LoadItems(); //#3714
            }
            //Begin #14448
            if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)
            {
                newRowAdded = false;
                valBankCodeToFocus = false; //#14789
            }
            CalcTotal();
            //End #14448
		}

		private void colCheckType_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
            //Begin #14865
            if (_IsFromSelectRow)       // getting fired due to SelectRow in LoadItems when ProcessCmdKey is fired
                return;
            //End #14865

            if (!isViewOnly && colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() != mlOnUs) //Selva - #76195
                this.LoadItems(); //#3714

            if (ClearRow(colCheckType)) //#3714
            {
                //Begin #14448
                //if (colCheckType.Text == string.Empty)
                //{
                //    colCheckType.Text = CoreService.Translation.GetUserMessageX(360590);
                //    colCheckType.CodeValue = CoreService.Translation.GetListItemX(ListId.CheckType, "O");
                //}
                //End #14448
                if (colCheckType.PhoenixUIControl.BusObjectProperty.Constraint.Values.GetDescriptionIndex(colCheckType.Text) >= 0)
                {
                    if (ValidateGrid(colCheckType.XmlTag, true))
                    {
                        if (colCheckType.UnFormattedValue != null)
                        {
                            if (colCheckType.Text != mlOnUs)
                            {
								colTranCode.PhoenixUIControl.BusObjectProperty.SetValue(null, EventBehavior.None);	// #6615 - colTranCode.PhoenixUIControl.BusObjectProperty.SetValueToNull();
                                colTranCode.UnFormattedValue = null;
                            }
                        }
                    }
                    else
                        e.Cancel = true;
                }
                else if ( !string.IsNullOrEmpty( colCheckType.Text ))   // #14448
                    e.Cancel = true;
            }

		}

		private void colAcctType_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
//			return;
//			if (  _tellerHelper.AcctType.Constraint.Values.GetKeyIndex(colAcctType.Text) >= 0 ||
//				colAcctType.Text == string.Empty || colAcctType.Text == GlobalVars.Instance.ML.GL) //#71226
			if ( colAcctType.Text == GlobalVars.Instance.ML.GL ||colAcctType.Text == string.Empty ||
				(colAcctType.Text != string.Empty && _validAcctTypes.IndexOf(colAcctType.Text) != -1))
			{
				if (!ValidateAcctType())
					e.Cancel = true;
			}
			else
				e.Cancel = true;
		}

		private void colAcctNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
 			using (new WaitCursor())
			{
				if (!ValidateAcctNo())
					e.Cancel = true;
                //this.LoadItems(); //Selva - 74015: Added overload //#13770
                _isSaveTriggered = false;   //#13770
                //LoadItems(false, false, null);  //#13770
                this.EnableDisableVisibleLogic("Alerts");
                this.OnUsInformation();
			}
		}

		private void colCheckNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateGrid(colCheckNo.XmlTag, true))
				e.Cancel = true;
		}

		private void colTranCode_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateGrid(colTranCode.XmlTag, true))
				e.Cancel = true;
		}

		private void colAmount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
            //unValidatedRowId = gridCapturedItems.ContextRow;    // #14448
            //Begin #14448
            decimal result = 0;
            if (colAmount.UnFormattedValue != null && decimal.TryParse(colAmount.UnFormattedValue.ToString(), out result))
            {
                colAmount.Text = CurrencyHelper.GetFormattedValue( result);
            }
            //End #14448
            using (new WaitCursor())
			{
				if (!ValidateGrid(colAmount.XmlTag, true))
					e.Cancel = true;
			}
            //Begin #14448
            //End #14448
            if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)    // #14448
                newRowAdded = false;
		}

		private void colNoOfItems_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
            /* Begin Task#41151 */
            if (Convert.ToString(colCheckType.UnFormattedValue) != mlOnUs && Convert.ToInt16(colNoOfItems.UnFormattedValue) > 1000)/*#134431 - change limit from 255 to 1000 */
            {
                PMessageBox.Show(this, 360984, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                e.Cancel = true;
                return;
            }
            /* End Task#41151 */
			if (!ValidateGrid(colNoOfItems.XmlTag, true))
				e.Cancel = true;
		}

		private void gridCapturedItems_EndOfRows(object sender, GridRowArgs e)
		{
            try
            {
                if (EndOfRowValidation())
                {
                    LoadItems(false, false, null);
                    CalcTotal();
                    //
                    //Begin #79420
                    CalculateNxtDayAvl(-2, true, false, null);
                    //End #79420
                    if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)
                        AddNewRow();
                }
            }
            catch
            {
                dlgInformation.Instance.HideInfo();
                _freezeDataInput = false;
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
                _freezeDataInput = false;
            }
		}

		private void pbGetAcct_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms( "GetAcct" );
		}

		private void pbGetGlAcct_Click(object sender, PActionEventArgs e)
		{
			gLSearchAcctNo = null;
			CallOtherForms( "GetGLAcct" );
		}

		private void tempWin_Closed(object sender, EventArgs e)
		{
			Form form = sender as Form;
			if ( form.Name == "frmGlAccountSearch" )
			{
				if ( gLSearchAcctNo != null && gLSearchAcctNo != string.Empty )
				{
					colAcctNo.UnFormattedValue = gLSearchAcctNo;
					SetAcctNoField(colAcctNo, colAcctType);
                    // Begin WI#13780 - Validate GL account Has posting rights
                    if (!ValidateGlAccess(colAcctNo.Text, colAcctType.Text))
                    {
                        // Clear out Invalid GL Account
                        colAcctNo.Text = String.Empty;
                        colAcctNo.Focus();
                    }
                    // End WI#13780
				}
			}
			EnableDisableVisibleLogic("DisableGetAccount");
		}

		private void colAcctNo_PhoenixUIEnterEvent(object sender, PEventArgs e)
		{
			EnableDisableVisibleLogic("GetAccount");
		}

		private void colAcctNo_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			//EnableDisableVisibleLogic("DisableGetAccount");
			if (colAcctType.Text == GlobalVars.Instance.ML.GL && !pbGetGlAcct.ImageButton.ContainsFocus)
				EnableDisableVisibleLogic("DisableGetAccount");
			else if (colAcctType.Text != GlobalVars.Instance.ML.GL && !pbGetAcct.ImageButton.ContainsFocus)
				EnableDisableVisibleLogic("DisableGetAccount");
		}

		private void colAmount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			//wi#4168 removed 77173 ,fixed it in the pbnew_click method
		   //Issue # 77173
		     //  OnActionSave(false);
                  //endIssue # 77173
			CalcTotal();
		}

		private void colNoOfItems_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal();
		}

		private void colCheckType_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
            CalcTotal();
		}
		#endregion

		#endregion

		#region private methods

        // Begin #74015

        // Begin #74778
        private bool getRimRestrict(int rimNo)
		{
            this._adRmRestrict = new AdRmRestrict();
		    this._adRmRestrict.ActionType = XmActionType.Select;
		    this._adRmRestrict.RimNo.Value = rimNo;
		    this._adRmRestrict.RestrictId.Value = -1;
		    this._adRmRestrict.OutputTypeId.Value = 1;
		    if ( TellerVars.Instance.IsAppOnline )
				DataService.Instance.ProcessRequest(_adRmRestrict);

            //#75557 - do not try to access rim restriction for offline
            if (TellerVars.Instance.IsAppOnline)
            {
                if (_tellerVars.DebugSecurity || _adRmRestrict.RestrictLevel.Value < _tellerVars.EmplRestrictLevel)
                {
                    if (!_tellerVars.SetContextObject("AdGbRsmRimArray", rimNo))
                        return false;

                    _adGbRsmRim = _tellerVars.AdGbRsmRim;

                    if (AdGbRsmRim.Edit.Value != GlobalVars.Instance.ML.Y)
                    {
                        PMessageBox.Show(this, 319018, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                        return false;
                    }
                    return true;
                }
            }
            return true;
		}

        private bool ScreenHasAccess(string screenType)
        {
            string depLoan = null;
            int rimNo = 0;      //74778
            int screenId = 0;
            //Begin #76458
            string acctType = "";

            // Begin Task#112379 - Fix issue when gridCapturedItems is null due to Framework changes in memory management --  When Single Note and Called By AVTC the Caputure Window is not displayed
            if (gridCapturedItems != null && gridCapturedItems.Columns != null)
            { 
                if (!isViewOnly)
                {
                    if (items != null && items.Count > 0)   //#102719
                        acctType = (items[gridCapturedItems.ContextRow] as TlItemCapture).ItemAcct.AcctType.Value;
                }
                else
                    acctType = (colAcctType.UnFormattedValue != null ? colAcctType.UnFormattedValue.ToString() : string.Empty);
                //End #76458

                if (gridCapturedItems.ContextRow >= 0 )
                {
                    if (isViewOnly)
                    {
                        if (colDepLoan.UnFormattedValue != null)    //#13192
                            depLoan = colDepLoan.UnFormattedValue.ToString();
                        if (colRimNo.UnFormattedValue != null)  //#13192
                            rimNo = (Int32)colRimNo.UnFormattedValue;    //#74778
                    }
                    else
                    {
                        if (items != null && items.Count > 0)   //#105867
                        {
                            depLoan = (items[gridCapturedItems.ContextRow] as TlItemCapture).ItemAcct.DepLoan.Value;
                            rimNo = (items[gridCapturedItems.ContextRow] as TlItemCapture).ItemAcct.RimNo.Value;  //#74778
                        }
                    }
                }
                else
                    return false;
            } // End Task#112379

            // Begin #161243
            if (screenType == "TranAcctDisplay")
            {
                acctType = _tlTranSet.CurTran.TranAcct.AcctType.Value;
                rimNo = _tlTranSet.CurTran.TranAcct.RimNo.Value;
                depLoan = _tlTranSet.CurTran.TranAcct.DepLoan.Value;
                screenType = "display";
            }
            // End #161243

            if (screenType == "display")
            {
                // Begin #74778
                if (!getRimRestrict(rimNo))
                    return false;
                // End #74778
                if (depLoan == GlobalVars.Instance.ML.DP)
                    screenId = Phoenix.Shared.Constants.ScreenId.DpDisplay;
                else if (depLoan == GlobalVars.Instance.ML.LN)
                    screenId = Phoenix.Shared.Constants.ScreenId.LnDisplay;
                else if (depLoan == GlobalVars.Instance.ML.RM)
                    screenId = Phoenix.Shared.Constants.ScreenId.RmDisplay;
                else if (depLoan == GlobalVars.Instance.ML.SD)
                    screenId = Phoenix.Shared.Constants.ScreenId.SdDepositDisplay;
                else if (depLoan == GlobalVars.Instance.ML.Ext || depLoan == "EX")
                {
                    //Begin #76458
                    //screenId = Phoenix.Shared.Constants.ScreenId.OnExAcctDisp1;
                    screenId = _tlTranSet.GetExternalDisplayScreenId(acctType);
                    //End #76458
                }
            }
            if (screenId > 0 && ((CoreService.UIAccessProvider.GetScreenAccess(screenId) &
                AuthorizationType.Read) == AuthorizationType.Read || screenId == Phoenix.Shared.Constants.ScreenId.GbAccountListRim))
            {
                return true;
            }
            return false;
        }

        private void AddNewRow()        //#80618 - removed the param
		{
            //Begin #140895-ReadOnly
            if (_tlTranSet != null && (_tlTranSet.IsTellerCaptureTran || _tlTranSet.IsTellerCaptureCheckOnlyTran))  //#102719
                return;
            //End #140895-ReadOnly

            _busObjCapturedItems = new TlItemCapture();
            int columnIndex = -1;   //#80618
            //Begin #4694
            if (gridCapturedItems.Count > 0)
            {
                ContextRowScreenToObject(gridCapturedItems.ContextRow, true);
                //if (!ValidateGrid(null, true))
                //    return;
            }
            _busObjCapturedItems = new TlItemCapture(); // #05144

            //Begin #79420
            if (_tlTranSet != null)
                _busObjCapturedItems.EffectiveDt.Value = _tlTranSet.EffectiveDt.Value;
            //End #79420
            // Begin #79420, #13851
            if (!isViewOnly && !_tlTranSet.CurTran.TranEffectiveDt.IsNull)
                _busObjCapturedItems.TranEffectiveDt.Value = _tlTranSet.CurTran.TranEffectiveDt.Value;
            // End #79420, #13851

			int newRowId = gridCapturedItems.AddNewRow();   // #14448

            //End #4694
            gridCapturedItems.SelectRow(gridCapturedItems.Count - 1, true); //#2690 - commented
			colNoOfItems.UnFormattedValue = 1;
            colCheckType.UnFormattedValue = null;   // #05144 - for some reson it's not cleaning up
            //Begin #80618
            colRowCount.UnFormattedValue = gridCapturedItems.Count; //#80618
            if (gridCapturedItems.Count > 9 && !_bumpedRowCountWidth)
            {
                _bumpedRowCountWidth = true;
                colBankCode.Width -= colRowCount.Width;
                colRowCount.Width += colRowCount.Width;
                this.UpdateView();
            }
			EnableDisableVisibleLogic("ActionCopyAndRemove");
            #region #2690
            //gridCapturedItems.Focus();    //#13372
            #endregion
            //Begin #80618
            if (!string.IsNullOrEmpty(_tellerVars.DefaultShortCode))
            {
                colBankCode.UnFormattedValue = _tellerVars.DefaultShortCode;
                ValidateBankCode();
            }
            if (_tellerVars.ReorderAmtItems)    //#80618
                columnIndex = gridCapturedItems.Columns.GetColumnIndex("Amount");
            else
                columnIndex = gridCapturedItems.Columns.GetColumnIndex("BankCode");
            if (columnIndex > -1)
                gridCapturedItems.Columns[columnIndex].Focus();
            //End #80618
            ClearTitleInfo();
            EnableDisableVisibleLogic("ClearTitle"); //Selva-74015
            //
            LoadItems();    //13770

            newRowAdded = true; // #14448
            //gridCapturedItems.Focus();  // #14448
            //if ( firstColToFocus != null )
            //    firstColToFocus.Focus();    // #14448
            gridCapturedItems.NextFocusColumn = null;   // #14448 - hack

            valBankCodeToFocus = true;  // #14789

		}

        private void ClearTitleInfo()
        {
            dfOnUsTitle1.Clear();
            dfOnUsTitle2.Clear();
            dfOnUsSignatures.Clear();
        }

		private void ContextRowScreenToObject( int rowId, bool isLoadRowToObject)
		{
			if (isLoadRowToObject)
			{
				//Begin #79420
                if (items.Count > rowId)
                    _busObjCapturedItems = items[rowId] as TlItemCapture;
                //End #79420

                gridCapturedItems.RowToObject(_busObjCapturedItems);
                //Begin #76458
                if (!_busObjCapturedItems.AcctType.IsNull && !_busObjCapturedItems.AcctNo.IsNull)
                {
                    if (gridCapturedItems.GetCellValueFormatted(rowId, colRealAcctNo.ColumnId) != "" &&
                        gridCapturedItems.GetCellValueFormatted(rowId, colRealAcctNo.ColumnId) != null)
                    {
                        _busObjCapturedItems.AcctNo.ValueObject = gridCapturedItems.GetCellValueFormatted(rowId, colRealAcctNo.ColumnId);
                    }
                }
                //End #76458
			}
			else
			{
				_busObjCapturedItems.BankCode.ValueObject = gridCapturedItems.GetCellValueFormatted( rowId, colBankCode.ColumnId );
				_busObjCapturedItems.RoutingNo.ValueObject = gridCapturedItems.GetCellValueFormatted( rowId, colRoutingNo.ColumnId );
                if (!string.IsNullOrEmpty(gridCapturedItems.Items[rowId].SubItems[colCheckType.ColumnId].Text))      // #14448
                    _busObjCapturedItems.CheckType.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colCheckType.ColumnId);
                else
                    _busObjCapturedItems.CheckType.SetValue(null);  // #14448
				_busObjCapturedItems.AcctType.ValueObject = gridCapturedItems.GetCellValueFormatted( rowId, colAcctType.ColumnId );
                //Begin #76458
                if (_busObjCapturedItems.AcctType.IsNull || (!_busObjCapturedItems.AcctType.IsNull && !_tlTranSet.IsExternalAdapterAcct(_busObjCapturedItems.AcctType.Value)))
                    _busObjCapturedItems.AcctNo.ValueObject = gridCapturedItems.GetCellValueFormatted(rowId, colAcctNo.ColumnId);
                //End #76458
				_busObjCapturedItems.CheckNo.ValueObject = gridCapturedItems.GetCellValueUnformatted( rowId, colCheckNo.ColumnId );
				_busObjCapturedItems.TranCode.ValueObject = gridCapturedItems.GetCellValueUnformatted( rowId, colTranCode.ColumnId );
				_busObjCapturedItems.Amount.ValueObject = gridCapturedItems.GetCellValueUnformatted( rowId, colAmount.ColumnId );
				_busObjCapturedItems.NoItems.ValueObject = gridCapturedItems.GetCellValueUnformatted( rowId, colNoOfItems.ColumnId );
				_busObjCapturedItems.ApplType.ValueObject = gridCapturedItems.GetCellValueFormatted( rowId, colAcctApplType.ColumnId );

                //Begin #79420
                _busObjCapturedItems.FloatDays.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colFloatDays.ColumnId);
                _busObjCapturedItems.FloatDate.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colExpireDate.ColumnId);
                _busObjCapturedItems.NxtDayBal.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colNxtDayBal.ColumnId);
                _busObjCapturedItems.CalcNxtDayBal.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colCalcNxtDayBal.ColumnId);
                _busObjCapturedItems.FloatBal.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colFloatBal.ColumnId);
                //End #79420
                //Begin #79420, #15357
                _busObjCapturedItems.ReqExceptRsnCode.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colReqExceptRsnCode.ColumnId );
                //End #79420, #15357
                // Begin #161243
                _busObjCapturedItems.FloatBalExcept.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colFloatBalExcept.ColumnId);
                _busObjCapturedItems.FloatDaysExcept.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colFloatDaysExcept.ColumnId);
                _busObjCapturedItems.FloatDateExcept.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colExpireDateExcept.ColumnId);
                // End #161243

                //Begin #30969
                _busObjCapturedItems.TlCaptureISN.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colTlCaptureISN.ColumnId);
                _busObjCapturedItems.TlCaptureParentISN.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colTlCaptureParentISN.ColumnId);
                _busObjCapturedItems.TlCaptureAux.ValueObject = gridCapturedItems.GetCellValueUnformatted(rowId, colTlCaptureAux.ColumnId);
                //End #30969
			}
		}

		private void ContextRowObjectToScreen( int rowId, string editedField)
		{
			if (editedField != null)
			{
				gridCapturedItems.ObjectToRow(_busObjCapturedItems);
			}
			else
			{
				gridCapturedItems.SetCellValueUnFormatted( rowId, colBankCode.ColumnId, _busObjCapturedItems.BankCode.Value );
				gridCapturedItems.SetCellValueUnFormatted( rowId, colRoutingNo.ColumnId, _busObjCapturedItems.RoutingNo.Value );
				gridCapturedItems.SetCellValueUnFormatted( rowId, colCheckType.ColumnId, _busObjCapturedItems.CheckType.Value );
				gridCapturedItems.SetCellValueUnFormatted( rowId, colAcctType.ColumnId, _busObjCapturedItems.AcctType.Value );
                //Begin #76458
                if (!_busObjCapturedItems.AcctType.IsNull && !_busObjCapturedItems.AcctNo.IsNull)
                {
                    if (GetExternalMaskedAcct(_busObjCapturedItems.AcctType.Value, _busObjCapturedItems.AcctNo.Value, _busObjCapturedItems.ApplType.Value) == null)
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colAcctNo.ColumnId, _busObjCapturedItems.AcctNo.Value);
                    else
                    {
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colAcctNo.ColumnId, _exMaskedAcct);
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colRealAcctNo.ColumnId, _busObjCapturedItems.AcctNo.Value);
                    }
                }
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colAcctNo.ColumnId, _busObjCapturedItems.AcctNo.Value);
                //End #76458
                if (!_busObjCapturedItems.CheckNo.IsNull)
					gridCapturedItems.SetCellValueUnFormatted( rowId, colCheckNo.ColumnId, _busObjCapturedItems.CheckNo.Value );
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colCheckNo.ColumnId, null);
				if (!_busObjCapturedItems.TranCode.IsNull)
					gridCapturedItems.SetCellValueUnFormatted( rowId, colTranCode.ColumnId, _busObjCapturedItems.TranCode.Value );
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTranCode.ColumnId, null);
				if (!_busObjCapturedItems.Amount.IsNull)
					gridCapturedItems.SetCellValueUnFormatted( rowId, colAmount.ColumnId, _busObjCapturedItems.Amount.Value );
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colAmount.ColumnId, null);
				if (!_busObjCapturedItems.NoItems.IsNull)
					gridCapturedItems.SetCellValueUnFormatted( rowId, colNoOfItems.ColumnId, _busObjCapturedItems.NoItems.Value );
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colNoOfItems.ColumnId, null);
                //
				gridCapturedItems.SetCellValueUnFormatted( rowId, colAcctApplType.ColumnId, _busObjCapturedItems.ApplType.Value );

                //Begin #79420
                if (!_busObjCapturedItems.FloatDays.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDays.ColumnId, _busObjCapturedItems.FloatDays.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDays.ColumnId, null);

                if (!_busObjCapturedItems.FloatDate.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDate.ColumnId, _busObjCapturedItems.FloatDate.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDate.ColumnId, null);

                if (!_busObjCapturedItems.NxtDayBal.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colNxtDayBal.ColumnId, _busObjCapturedItems.NxtDayBal.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colNxtDayBal.ColumnId, null);

                if (!_busObjCapturedItems.CalcNxtDayBal.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colCalcNxtDayBal.ColumnId, _busObjCapturedItems.CalcNxtDayBal.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colCalcNxtDayBal.ColumnId, null);

                if (!_busObjCapturedItems.FloatBal.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBal.ColumnId, _busObjCapturedItems.FloatBal.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBal.ColumnId, null);

                //End #79420

                //Begin #79420, #15357
                if (!_busObjCapturedItems.ReqExceptRsnCode.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colReqExceptRsnCode.ColumnId, _busObjCapturedItems.ReqExceptRsnCode.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colReqExceptRsnCode.ColumnId, null);
                //End #79420, #15357

                // Begin #161243
                if (!_busObjCapturedItems.FloatBalExcept.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBalExcept.ColumnId, _busObjCapturedItems.FloatBalExcept.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBalExcept.ColumnId, null);
                if (!_busObjCapturedItems.FloatDaysExcept.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDaysExcept.ColumnId, _busObjCapturedItems.FloatDaysExcept.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDaysExcept.ColumnId, null);
                if (!_busObjCapturedItems.FloatDateExcept.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDateExcept.ColumnId, _busObjCapturedItems.FloatDateExcept.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDateExcept.ColumnId, null);
                // End #161243

                //Begin #30969
                if (!_busObjCapturedItems.TlCaptureISN.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureISN.ColumnId, _busObjCapturedItems.TlCaptureISN.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureISN.ColumnId, null);
                if (!_busObjCapturedItems.TlCaptureParentISN.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureParentISN.ColumnId, _busObjCapturedItems.TlCaptureParentISN.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureParentISN.ColumnId, null);
                if (!_busObjCapturedItems.TlCaptureAux.IsNull)
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureAux.ColumnId, _busObjCapturedItems.TlCaptureAux.Value);
                else
                    gridCapturedItems.SetCellValueUnFormatted(rowId, colTlCaptureAux.ColumnId, null);
                //End #30969
			}
		}

		private bool EndOfRowValidation()
		{
            return EndOfRowValidation(-1, false);   // #79420 - Passed skipNxtDayCal as false;
		}

        private bool EndOfRowValidation(int rowId, bool skipNxtDayCal) // #79420 - Added skipNxtDayCal
        {
            bool isSuccess = true;
            int columnIndex = -1;   //#80618
            if (gridCapturedItems != null && gridCapturedItems.Count < 1)   //#80618
                return isSuccess;
            //if (rowId >= 0)
            //    gridCapturedItems.ContextRow = rowId;

            //if (gridCapturedItems.Count > 0 && gridCapturedItems.ContextRow != gridCapturedItems.Count - 1) //Selva-74015-New
            //    _busObjCapturedItems = items[gridCapturedItems.ContextRow] as TlItemCapture;
            isSuccess = ValidateGrid(null, true);
            //
            if (!isSuccess)
            {
                if (_focusField != null)
                {
                    //Begin #14448
                    //gridCapturedItems.SelectRow(gridCapturedItems.ContextRow, true);
                    if (glacialTable != null && gridCapturedItems.ContextRow >= 0 && glacialTable.FocusedItem != glacialTable.Items[gridCapturedItems.ContextRow])
                    {
                        glacialTable.FocusedItem = glacialTable.Items[gridCapturedItems.ContextRow];
                        glacialTable.EnsureVisible(gridCapturedItems.ContextRow);
                    }
                    //End #14448
                    if (rowId == -1)
                    {
                        columnIndex = gridCapturedItems.Columns.GetColumnIndex(_focusField);
                        if (columnIndex > -1)
                            gridCapturedItems.Columns[columnIndex].Focus();
                    }
                }
            }

            //Begin #79420
            if (isSuccess && !skipNxtDayCal)
            {
                isSuccess = CalculateNxtDayAvl(-2,true, false, null);
            }
            //End #79420
            return isSuccess;
        }

		private void AddNewItem(TlItemCapture item, bool isCopyItem)
		{
			//Selva-74015
            if (isCopyItem)
                _busObjCapturedItems = new TlItemCapture();
            //Begin #79420
            if (_tlTranSet != null)
                _busObjCapturedItems.EffectiveDt.Value = _tlTranSet.EffectiveDt.Value;
            //End #79420
            // Begin #79420, #13851
            if (!isViewOnly && !_tlTranSet.CurTran.TranEffectiveDt.IsNull)
                _busObjCapturedItems.TranEffectiveDt.Value = _tlTranSet.CurTran.TranEffectiveDt.Value;
            // End #79420, #13851
            //
            gridCapturedItems.AddNewRow();
			//
			colTranCode.FieldToColumn(item.TranCode);
			colAcctApplType.FieldToColumn(item.ApplType);
			colAcctType.FieldToColumn(item.AcctType);
			colBankCode.FieldToColumn(item.BankCode);
			colRoutingNo.FieldToColumn(item.RoutingNo);
			colCheckType.FieldToColumn(item.CheckType);
            //Begin #76458
            if (GetExternalMaskedAcct(item.AcctType.Value, item.AcctNo.Value, item.ApplType.Value) == null)
                colAcctNo.FieldToColumn(item.AcctNo);
            else
            {
                colRealAcctNo.FieldToColumn(item.AcctNo);
                colAcctNo.UnFormattedValue = _exMaskedAcct;
            }
            //End #76458
            //Begin #79420
            colFloatDays.FieldToColumn(item.FloatDays);
            colExpireDate.FieldToColumn(item.FloatDate);
            //End #79420

            //Begin #79420, #15357
            colReqExceptRsnCode.FieldToColumn(item.ReqExceptRsnCode);
            //End #79420, #15357

            //Begin #30969
            colTlCaptureISN.FieldToColumn(item.TlCaptureISN);
            colTlCaptureParentISN.FieldToColumn(item.TlCaptureParentISN);
            colTlCaptureAux.FieldToColumn(item.TlCaptureAux);
            //End #30969

            if( !isCopyItem )
			{
				colCheckNo.FieldToColumn(item.CheckNo);
				colAmount.FieldToColumn( item.Amount );
				colNoOfItems.FieldToColumn(item.NoItems);
				colIndex.UnFormattedValue = gridCapturedItems.Count;
                //Begin #79420
                colNxtDayBal.FieldToColumn(item.NxtDayBal);
                colFloatBal.FieldToColumn(item.FloatBal);
                colExceptRsnCode.FieldToColumn(item.ExceptRsnCode);
                // Begin #161243
                colFloatBalExcept.FieldToColumn(item.FloatBalExcept);
                colFloatDaysExcept.FieldToColumn(item.FloatDaysExcept);
                colExpireDateExcept.FieldToColumn(item.FloatDateExcept);
                // End #161243
                //End #79420
			}
			else
			{
				colNoOfItems.UnFormattedValue = 1;
			}
            //Selva-74015
            if (isCopyItem)
            {
                ContextRowScreenToObject(gridCapturedItems.ContextRow, true);
                _busObjCapturedItems.ItemAcct = item.ItemAcct;
                _busObjCapturedItems.CalcFloatDays.Value = item.CalcFloatDays.Value;    // #79420
                this.LoadItems(); //Selva - 74015
            }
            //Selva-73015
            #region #80618
            colRowCount.UnFormattedValue = gridCapturedItems.Count;
            if (gridCapturedItems.Count > 9 && !_bumpedRowCountWidth)
            {
                _bumpedRowCountWidth = true;
                colBankCode.Width -= colRowCount.Width;
                colRowCount.Width += colRowCount.Width;
            }
            #endregion
		}

        //Begin #76458
        private string GetExternalMaskedAcct(string acctType, string acctNo, string applType)
        {
            if (acctType == "" || acctType == null || acctType == string.Empty ||
                acctNo == "" || acctNo == null || acctNo == string.Empty)
                return null;

            _isExternalAcct = false;
            _exMaskedAcct = null;
            if (applType != "" && applType != null && applType != string.Empty && applType == "EX")
                _isExternalAcct = true;
            if (!_isExternalAcct)
            {
                if (_tlTranSet.IsExternalAdapterAcct(acctType))
                    _isExternalAcct = true;
            }
            if (_isExternalAcct)
            {
                _exMaskedAcct = _tlTranSet.TellerVars.DisplayAcctNos.GetMaskedAcctNo(acctType, acctNo);
                if (_exMaskedAcct == "" || _exMaskedAcct == null || _exMaskedAcct == acctType)
                {
                    _exMaskedAcct = null;
                    _exMaskedAcct = _gbHelper.GetMaskedExtAcct(acctType, acctNo);
                    _tlTranSet.TellerVars.DisplayAcctNos.SetMaskedAcctNo(acctType, acctNo, _exMaskedAcct);
                }
            }
            if (_exMaskedAcct == "" || _exMaskedAcct == null || _exMaskedAcct == acctType)
                return null;
            return _exMaskedAcct;
        }
        //End #76458

        private void LoadItems()
        {
            if (colIndex.UnFormattedValue == null)
            {
                colAcctApplType.ColumnToField(_busObjCapturedItems.ApplType);
                colBankCode.ColumnToField(_busObjCapturedItems.BankCode);
                colRoutingNo.ColumnToField(_busObjCapturedItems.RoutingNo);
                colCheckType.ColumnToField(_busObjCapturedItems.CheckType);
                colAcctType.ColumnToField(_busObjCapturedItems.AcctType);
                //Begin #76458
                if (colTranCode.UnFormattedValue == null || (colTranCode.UnFormattedValue != null &&
                    colTranCode.Text.Trim() != "570"))
                    colAcctNo.ColumnToField(_busObjCapturedItems.AcctNo);
                else
                    colRealAcctNo.ColumnToField(_busObjCapturedItems.AcctNo); //New
                //End #76458
                colCheckNo.ColumnToField(_busObjCapturedItems.CheckNo);
                colTranCode.ColumnToField(_busObjCapturedItems.TranCode);
                colAmount.ColumnToField(_busObjCapturedItems.Amount);
                colNoOfItems.ColumnToField(_busObjCapturedItems.NoItems);

                //Begin #79420
                colFloatDays.ColumnToField(_busObjCapturedItems.FloatDays);
                colExpireDate.ColumnToField(_busObjCapturedItems.FloatDate);
                colNxtDayBal.ColumnToField(_busObjCapturedItems.NxtDayBal);
                colCalcNxtDayBal.ColumnToField(_busObjCapturedItems.CalcNxtDayBal);
                colFloatBal.ColumnToField(_busObjCapturedItems.FloatBal);
                //End #79420
                //Begin #79420, #15357
                colReqExceptRsnCode.ColumnToField(_busObjCapturedItems.ReqExceptRsnCode);
                //End #79420, #15357

                // Begin #161243
                colFloatBalExcept.ColumnToField(_busObjCapturedItems.FloatBalExcept);
                colFloatDaysExcept.ColumnToField(_busObjCapturedItems.FloatDaysExcept);
                colExpireDateExcept.ColumnToField(_busObjCapturedItems.FloatDateExcept);
                // End #161243

                //Begin #30969
                colTlCaptureISN.ColumnToField(_busObjCapturedItems.TlCaptureISN);
                colTlCaptureParentISN.ColumnToField(_busObjCapturedItems.TlCaptureParentISN);
                colTlCaptureAux.ColumnToField(_busObjCapturedItems.TlCaptureAux);
                //End #30969

                HandleWhiteSpace(_busObjCapturedItems);
                //
                items.Add(_busObjCapturedItems);
                //
                colIndex.UnFormattedValue = gridCapturedItems.ContextRow;
            }
        }

        #region #80618
        /// <summary>
        /// This method is responsible for copying the saved items into the original items collection to preserve it for isformdirty comparison and replace the working
        /// items collection in case the user decide not to save the edited changes.
        /// </summary>
        /// <param name="items">saved items collection</param>
        private void LoadOrigItems(ArrayList items)
        {
            if (items != null && items.Count > 0)
            {
                if (origItems != null && origItems.Count > 0)
                    origItems.Clear();
                foreach (TlItemCapture item in items)
                {
                    TlItemCapture item1 = new TlItemCapture();
                    foreach (FieldBase field in item.DbFields)
                    {
                        if (!field.IsNull)
                            item1.GetFieldByXmlTag(field.XmlTag).Value = field.Value;
                    }
                    foreach (FieldBase field in item.CalcOnlyFields)
                    {
                        if (!field.IsNull)
                            item1.GetFieldByXmlTag(field.XmlTag).Value = field.Value;
                    }
                    //
                    HandleWhiteSpace(item1);
                    item1.SaveFloatInfo();  // #79420
                    origItems.Add(item1);
                }
            }
        }

        private void DiscardUnsavedItems(ArrayList items)
        {
            if ((items != null && items.Count > 0) || (origItems != null && origItems.Count > 0))
            {
                if (items != null && items.Count > 0)
                    items.Clear();
                if (origItems != null && origItems.Count > 0)
                    items.AddRange(origItems);
            }
        }
        #endregion

        private void LoadItems(bool loadGridFromList, bool isCopyItem, TlItemCapture copyItem)
        {
            if (loadGridFromList)
            {
                if (!isCopyItem)
                    gridCapturedItems.ResetTable();

                if (copyItem == null)
                {
                    foreach (TlItemCapture item in items)
                    {
                        AddNewItem(item, isCopyItem);
                    }
                }
                else if (copyItem != null)
                    AddNewItem(copyItem, isCopyItem);
                // update total
                if (!isCopyItem)
                    CalcTotal();
            }
            else
            {
                int nRow = 0;
                //
                while (nRow < gridCapturedItems.Count)
                {
                    _IsFromSelectRow = true;
					//if (_isSaveTriggered)    //#13770
                    	gridCapturedItems.SelectRow(nRow, false);
                    _IsFromSelectRow = false;
                    ContextRowScreenToObject(nRow, true);
                    //
                    if (colIndex.UnFormattedValue != null && colIndex.Text.Length >= 0)
                    {
                        TlItemCapture item = null;
                        item = (items[nRow] as TlItemCapture);
                        colAcctApplType.ColumnToField(item.ApplType);
                        colBankCode.ColumnToField(item.BankCode);
                        colRoutingNo.ColumnToField(item.RoutingNo);
                        colCheckType.ColumnToField(item.CheckType);
                        colAcctType.ColumnToField(item.AcctType);
                        //Begin #76458
                        colAcctType.ColumnToField(item.AcctType);
                        if (colTranCode.UnFormattedValue == null || (colTranCode.UnFormattedValue != null &&
                            colTranCode.Text.Trim() != "570"))
                            colAcctNo.ColumnToField(item.AcctNo);
                        else
                            colRealAcctNo.ColumnToField(item.AcctNo);   //New
                        //End #76458
                        colCheckNo.ColumnToField(item.CheckNo);
                        colTranCode.ColumnToField(item.TranCode);
                        colAmount.ColumnToField(item.Amount);
                        colNoOfItems.ColumnToField(item.NoItems);
                        colIncomingAcctNo.ColumnToField(item.IncomingAcctNo); //#2224

                        //Begin #79420
                        colFloatDays.ColumnToField(item.FloatDays);
                        colExpireDate.ColumnToField(item.FloatDate);
                        colNxtDayBal.ColumnToField(item.NxtDayBal);
                        colCalcNxtDayBal.ColumnToField(item.CalcNxtDayBal);
                        colFloatBal.ColumnToField(item.FloatBal);
                        //End #79420
                        //Begin #79420, #15357
                        colReqExceptRsnCode.ColumnToField(item.ReqExceptRsnCode);
                        //End #79420, #15357

                        // Begin #161243
                        colFloatBalExcept.ColumnToField(item.FloatBalExcept);
                        colFloatDaysExcept.ColumnToField(item.FloatDaysExcept);
                        colExpireDateExcept.ColumnToField(item.FloatDateExcept);
                        // End #161243
                        //Begin #30969
                        colTlCaptureISN.ColumnToField(item.TlCaptureISN);
                        colTlCaptureParentISN.ColumnToField(item.TlCaptureParentISN);
                        colTlCaptureAux.ColumnToField(item.TlCaptureAux);
                        //End #30969
                        HandleWhiteSpace(item);
                    }
                    else
                    {
                        //Selva - 74015
                        colAcctApplType.ColumnToField(_busObjCapturedItems.ApplType);
                        colBankCode.ColumnToField(_busObjCapturedItems.BankCode);
                        colRoutingNo.ColumnToField(_busObjCapturedItems.RoutingNo);
                        colCheckType.ColumnToField(_busObjCapturedItems.CheckType);
                        colAcctType.ColumnToField(_busObjCapturedItems.AcctType);
                        //Begin #76458
                        if (colTranCode.UnFormattedValue == null || (colTranCode.UnFormattedValue != null &&
                            colTranCode.Text.Trim() != "570"))
                            colAcctNo.ColumnToField(_busObjCapturedItems.AcctNo);
                        else
                            colRealAcctNo.ColumnToField(_busObjCapturedItems.AcctNo);   //New
                        //End #76458
                        colCheckNo.ColumnToField(_busObjCapturedItems.CheckNo);
                        colTranCode.ColumnToField(_busObjCapturedItems.TranCode);
                        colAmount.ColumnToField(_busObjCapturedItems.Amount);
                        colNoOfItems.ColumnToField(_busObjCapturedItems.NoItems);
                        colIncomingAcctNo.ColumnToField(_busObjCapturedItems.IncomingAcctNo); //2224

                        //Begin #79420
                        colFloatDays.ColumnToField(_busObjCapturedItems.FloatDays);
                        colExpireDate.ColumnToField(_busObjCapturedItems.FloatDate);
                        colNxtDayBal.ColumnToField(_busObjCapturedItems.NxtDayBal);
                        colCalcNxtDayBal.ColumnToField(_busObjCapturedItems.CalcNxtDayBal);
                        colFloatBal.ColumnToField(_busObjCapturedItems.FloatBal);
                        //End #79420
                        //Begin #79420, #15357
                        colReqExceptRsnCode.ColumnToField(_busObjCapturedItems.ReqExceptRsnCode);
                        //End #79420, #15357

                        // Begin #161243
                        colFloatBalExcept.ColumnToField(_busObjCapturedItems.FloatBalExcept);
                        colFloatDaysExcept.ColumnToField(_busObjCapturedItems.FloatDaysExcept);
                        colExpireDateExcept.ColumnToField(_busObjCapturedItems.FloatDateExcept);
                        // End #161243

                        //Begin #30969
                        colTlCaptureISN.ColumnToField(_busObjCapturedItems.TlCaptureISN);
                        colTlCaptureParentISN.ColumnToField(_busObjCapturedItems.TlCaptureParentISN);
                        colTlCaptureAux.ColumnToField(_busObjCapturedItems.TlCaptureAux);
                        //End #30969

                        HandleWhiteSpace(_busObjCapturedItems);
                        items.Add(_busObjCapturedItems);
                        colIndex.UnFormattedValue = nRow;
                        //Selva - 74015 End
                    }
                    nRow = nRow + 1;
                }
            }
        }

		private void CalcTotal()
		{
			dfTotalTransitChecks.UnFormattedValue = 0;
			dfTotalOnUsChecks.UnFormattedValue = 0;
			dfTotalChecksAsCash.UnFormattedValue = 0;
			dfNoOfItems.UnFormattedValue = 0;
			decimal onUsChksAmt = 0;
			decimal chksAsCashAmt = 0;
			decimal transitChksAmt = 0;
			decimal amt = 0;
			int noOfItemsCount = 0;
			int noItem = 0;
			object objectValue = null;
			//
			for( int rowId = 0; rowId < gridCapturedItems.Count; rowId++ )
			{
				amt = 0;
				noItem = 0;
				//
				objectValue = gridCapturedItems.GetCellValueUnformatted( rowId, colAmount.ColumnId );
				if (objectValue == null)
					amt = 0;
				else
					amt = Convert.ToDecimal(objectValue);
				objectValue = gridCapturedItems.GetCellValueFormatted( rowId, colCheckType.ColumnId );
				if (objectValue != null)
				{
					if (Convert.ToString(objectValue) == CoreService.Translation.GetListItemX(ListId.CheckType, "Transit"))
						transitChksAmt = transitChksAmt + amt;
					else if (Convert.ToString(objectValue) == CoreService.Translation.GetListItemX(ListId.CheckType, "On-Us"))
						onUsChksAmt = onUsChksAmt + amt;
					else if (Convert.ToString(objectValue) == CoreService.Translation.GetListItemX(ListId.CheckType, "Check As Cash"))
						chksAsCashAmt = chksAsCashAmt + amt;
				}
				//
				objectValue = gridCapturedItems.GetCellValueUnformatted( rowId, colNoOfItems.ColumnId );
				if (objectValue == null)
					noItem = 0;
				else
					noItem = Convert.ToInt32(objectValue);
				noOfItemsCount = noOfItemsCount + noItem;
			}
			dfTotalTransitChecks.UnFormattedValue = transitChksAmt;
			dfTotalOnUsChecks.UnFormattedValue = onUsChksAmt;
			dfTotalChecksAsCash.UnFormattedValue = chksAsCashAmt;
			dfNoOfItems.UnFormattedValue = noOfItemsCount;
		}

		private bool ValidateGrid( string editedField, bool loadRowFromObject)
		{
            //Begin #14448
            string logInfo = string.Format(@"ValidateGrid editedField={0}, loadRowFromObject = {1}, _busObjCapturedItems.Amount.Value = {2}
colAmount.UnFormattedValue = {3}, colAmount.Text = {4}, gridCapturedItems.ContextRow={5}",
                    editedField, loadRowFromObject, _busObjCapturedItems.Amount.ValueObject, colAmount.UnFormattedValue, colAmount.Text, gridCapturedItems.ContextRow );

            LogDebugInfo(logInfo, 3);
            //End #14448

            _focusField = null;
			bool isSuccess = true;

			//
			if (loadRowFromObject)
				ContextRowScreenToObject(gridCapturedItems.ContextRow, true);
			//
            // Begin #79420, #13851
            if (!isViewOnly && !_tlTranSet.CurTran.TranEffectiveDt.IsNull)
                _busObjCapturedItems.TranEffectiveDt.Value = _tlTranSet.CurTran.TranEffectiveDt.Value;
            // End #79420, #13851

			isSuccess = _busObjCapturedItems.ValidateItem(_tlTranSet.TellerHelper,
				_gbHelper, _tlTranSet.TellerVars, _tlTranSet.AdTlTc, editedField,
				(_tlTranSet.AdTlTc.RealTimeEnable.Value == GlobalVars.Instance.ML.Y? "R" : "M"),
                null, null,
                _tlTranSet.CurTran.RegCcCode.Value, _tlTranSet.CurTran.RegCcPtid.Value,  // #79420 - Passed RegCcCode/ RegCcPtid
                ref _focusField);


			//
			if (loadRowFromObject)
			{
				if(!isSuccess)
				{
					if (!_busObjCapturedItems.ActionReturnCode.IsNull && _busObjCapturedItems.ActionReturnCode.Value != 0)
					{
						if (_busObjCapturedItems.ActionReturnCode.Value < 0)
						{
							returnCodeDesc = _gbHelper.GetSPMessageText( _busObjCapturedItems.ActionReturnCode.Value, false );
							PMessageBox.Show(this, 360532, MessageType.Warning, MessageBoxButtons.OK, new string[] {Convert.ToString(_busObjCapturedItems.ActionReturnCode.Value) + " - " + returnCodeDesc});
						}
                        //Begin WI#40789
                        else if (_busObjCapturedItems.ActionReturnCode.Value == 13805)
                        {
                            //The %1! account %2! has reached its maximum Reg D Transfers and Withdrawals per-period limit.
                            //Do you want to continue?
                            if (PMessageBox.Show(this, 13805, MessageType.Warning, MessageBoxButtons.YesNo, new string[] { _busObjCapturedItems.AcctType.Value, _busObjCapturedItems.AcctNo.Value }) == DialogResult.Yes)
                            {
                                //Set value back to true is user selects to continue.
                                isSuccess = true;
                            }
                        }
                        //End WI#40789
						else
						{
							PMessageBox.Show(this, _busObjCapturedItems.ActionReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						}
						if (editedField != _focusField)
						{
							if (_focusField != null && editedField != null)
							{
									gridCapturedItems.NextFocusColumn = gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)];
							}
						}
					}
				}

                //else WI#40789
                if (isSuccess)
				{
                    // Begin #79420, #13851
                    EnableDisableVisibleLogic("FloatInfo");
                    // End #79420, #13851

                    //Begin #79420-2
                    //#79420, #13851 - removed the check for amount tag
                    //if ( _busObjCapturedItems.ValidateItemFloatInfo() && items.IndexOf(_busObjCapturedItems)>= 0 )    // #14448 - make the call unconditionally
                    {
                        CalculateNxtDayAvl(-2, true, false, null);
                    }
                    //End #79420-2
                    SetReqExceptRsnRowColor(-1);  //#79420, #15357


                    //Begin #80618 - Set field focus only when teller fraud is not enabled.
					ContextRowObjectToScreen(gridCapturedItems.ContextRow, editedField);
					//
                    if (editedField != null) // #14789 - removed the && !_isFraudPreventionEnabled)
					{
                        //Begin #14789
                        //clear out the focus field name if amount if first column and already populated.
                        if (_focusField == colAmount.XmlTag && _tellerVars.ReorderAmtItems && !_busObjCapturedItems.Amount.IsNull)
                        {
                            _focusField = null;
                        }
                        //End #14789
                        if (_focusField != null)
                        {
                            gridCapturedItems.NextFocusColumn =
                                gridCapturedItems.Columns[gridCapturedItems.Columns.GetColumnIndex(_focusField)]; //.Focus();
                        }
                        //Begin #14789
                        else
                        {
                            int nextColumnId = -1;
                            int currentColumnId = gridCapturedItems.Columns.GetColumnIndex(editedField);
                            if (currentColumnId >= 0)
                            {
                                for (int index = currentColumnId + 1; index < gridCapturedItems.Columns.Count; index++)
                                {
                                    GLColumn nextItem = gridCapturedItems.Columns[index];
                                    if (nextItem != null)
                                    {
                                        if (nextItem.Visible && !nextItem.ReadOnly && !nextItem.Disabled) // nextItem.ActivatedEmbeddedType != ActivatedEmbeddedTypes.None )
                                        {
                                            nextColumnId = index;
                                            break;
                                        }
                                        nextItem = null;
                                    }

                                }
                                if (nextColumnId >= 0)
                                {
                                    gridCapturedItems.NextFocusColumn = gridCapturedItems.Columns[nextColumnId];
                                }
                            }
                        }
                        //End #14789
					}
                    //End #80618


				}
			}

			return isSuccess;
		}

		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "ActionCopyAndRemove")
			{
				this.pbRemove.Enabled = ((gridCapturedItems.Count > 0 && !isViewOnly)? true : false);
				this.pbCopy.Enabled = ((gridCapturedItems.Count > 0 && !isViewOnly)? true : false);
                ResetFormForSupViewOnlyMode();  //#79314
            }
            #region #80660
            else if (callerInfo == "SuspectTran")
            {
                #region Enable/Disable pbSuspTranDtls
                if (isViewOnly)
                {
                    if (colSuspectPtid.Text != "" && CoreService.UIAccessProvider.HasReadAcces(Phoenix.Shared.Constants.ScreenId.SuspiciousTranDetails) && TellerVars.Instance.SuspiciousTransactionScoringAlertsCustomOption)
                        pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                    else
                        pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                }
                else
                    pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Disable);    //#33455
                #endregion
            }
            #endregion
            else if (callerInfo == "FormComplete")
            {
                this.dfObjectInput.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);

                if (isViewOnly)
                {
                    this.AvoidSave = true;
                    this.ActionSave.Visible = false;
                    this.pbCopy.Visible = false;
                    this.pbNewRow.Visible = false;
                    this.pbRemove.Visible = false;
                    this.pbGetAcct.Visible = false;
                    this.pbGetGlAcct.Visible = false;
                    //
                    this.pbReprint.Visible = true;
                    this.pbReprintAll.Visible = true;
                }
                else
                {
                    this.pbReprint.Visible = false;
                    this.pbReprintAll.Visible = false;
                    this.pbGetAcct.Enabled = false;
                    this.pbGetGlAcct.Enabled = false;
                }

                //Begin #79420
                if (isViewOnly || (_tlTranSet != null && _tlTranSet.CurTran.PostRealTime.Value != 1) || !(_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline) //#65459 added _tlTranSet != null
                    || !(_tlTranSet != null && _busObjCapturedItems.PopulateFloatInfo(_tlTranSet.TellerHelper, _tlTranSet.CurTran.TranCode.Value))
                    || (_tlTranSet != null && (_tlTranSet.CurTran.TranCode.Value == 140 || _tlTranSet.CurTran.TranCode.Value == 141)))    //#79420, #13851
                {
                    dfMakeAvail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);

                }
                //#79420, #13851 - Moved outside of the above block
                colFloatDays.ReadOnly = true;
                colExceptRsnCode.ReadOnly = true;
                //End #79420

                //Begin #140895-ReadOnly
                if (_tlTranSet != null && (_tlTranSet.IsTellerCaptureTran || _tlTranSet.IsTellerCaptureCheckOnlyTran))  //#102719
                {
                    this.pbCopy.Visible = false;
                    this.pbNewRow.Visible = false;
                    this.pbRemove.Visible = false;
                    this.pbGetAcct.Visible = false;
                    this.pbGetGlAcct.Visible = false;
                    //
                    this.pbReprint.Visible = false;
                    this.pbReprintAll.Visible = false;
                    for (int i = 0; i < gridCapturedItems.Columns.Count; i++)
                        gridCapturedItems.Columns[i].ReadOnly = true;
                    if (IsTlCaptureTranSetItemValFailed)
                    {
                        ActionSave.Enabled = false;
                        pbFloatInfo.Visible = false;
                    }
                }
                //End #140895-ReadOnly

                colTlCaptureISN.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y" && isViewOnly); //#30926
                colTlCaptureParentISN.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y" && isViewOnly); //#30926
                colTlCaptureAux.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y" && isViewOnly); //#30926
            }
            else if (callerInfo == "GetAccount")
            {
                if (!isViewOnly)
                {
                    if (colAcctType.Text == GlobalVars.Instance.ML.GL)
                    {
                        this.pbGetGlAcct.Enabled = true;
                        this.pbGetAcct.Enabled = false;
                    }
                    else
                    {
                        this.pbGetAcct.Enabled = true;
                        this.pbGetGlAcct.Enabled = false;
                    }
                }
            }
            else if (callerInfo == "DisableGetAccount")
            {
                this.pbGetAcct.Enabled = false;
                this.pbGetGlAcct.Enabled = false;
            }
            // Begin #74015
            else if (callerInfo == "OnUs")
            {
                if (((items != null && gridCapturedItems.ContextRow >= 0
                        && items.Count > gridCapturedItems.ContextRow) || isViewOnly)
                      && colCheckType.UnFormattedValue != null
                      && colCheckType.UnFormattedValue.ToString() == mlOnUs &&
                      _tlTranSet.TellerVars.IsAppOnline)    //#13334
                {
                    this.dfOnUsTitle1.Visible = true;
                    this.dfOnUsTitle2.Visible = true;
                    this.dfOnUsSignatures.Visible = true;
                    this.pbDisplay.Enabled = ScreenHasAccess("display");
                    this.pbSignature.Enabled = true; // #80620
                    //Begin #76458
                    pbDisplay.Enabled = pbDisplay.Enabled && !_tlTranSet.IsExternalAdapterAcct(colAcctType.Text); //#76458
                    pbSignature.Enabled = pbSignature.Enabled && !_tlTranSet.IsExternalAdapterAcct(colAcctType.Text); //#76458
                    //End #76458
                    #region #80620
                    //pbOnUsNotes.Enabled = true;
                    pbOnUsRestrictions.Enabled = true;
                    #endregion
                    this.dfOnUsTitle1.Enabled = true;
                    this.dfOnUsTitle2.Enabled = true;
                    this.dfOnUsSignatures.Enabled = true;
                }
                else
                {
                    this.pbSignature.Enabled = false;
                    this.pbDisplay.Enabled = false;
                    this.dfOnUsTitle1.Enabled = false;
                    this.dfOnUsTitle2.Enabled = false;
                    this.dfOnUsSignatures.Enabled = false;
                    this.dfOnUsTitle1.Visible = false;
                    this.dfOnUsTitle2.Visible = false;
                    this.dfOnUsSignatures.Visible = false;
                    #region #80620
                    pbOnUsNotes.Enabled = false;
                    pbOnUsRestrictions.Enabled = false;
                    #endregion
                }
            }
            else if (callerInfo == "ClearTitle")
            {
                this.pbSignature.Enabled = false;
                this.pbDisplay.Enabled = false;
                #region #80620
                pbOnUsNotes.Enabled = false;
                pbOnUsRestrictions.Enabled = false;
                #endregion
            }
            else if (callerInfo == "Alerts")
            {
                if (_IsForceContextRow)
                    gridCapturedItems.ContextRow = 0; //Selva-hack for fixing row focus for first row after save
                if (gridCapturedItems.ContextRow >= 0)
                {
                    //Begin #19415
                    AcctAlerts acctAlerts = null;
                    if (_busObjCapturedItems != null && _busObjCapturedItems.ItemAcct.AcctType.Value == colAcctType.Text &&
                        _busObjCapturedItems.ItemAcct.AcctNo.Value == colAcctNo.Text)
                    {
                        if (!_busObjCapturedItems.ItemAcct.AlertInfo.IsNull)
                        {
                            acctAlerts = new AcctAlerts();
                            if (!acctAlerts.LoadFromString(_busObjCapturedItems.ItemAcct.AlertInfo.Value))
                                acctAlerts = null;
                        }
                    }
                    //End #19415
                    ((Workspace as PwksWindow).WksExtension as WkspaceExtension).SetAlertAccount(
                                        colAcctType.Text, colAcctNo.Text, 0, true, acctAlerts); // #19415 - replaced null by acctAlerts

                    UpdateView();

                }
            }
            // End #74015

             //Begin #4450
            else if (callerInfo == "CrossRefAccounts")
            {
                if (_adGbBankControl.EnableCrossRef.Value != "Y")
                    this.colIncomingAcctNo.Visible = false;
                else
                    this.colIncomingAcctNo.Visible = true;
            }
            //End   #4450

            // Begin #79420, #13851
            else if (callerInfo == "FloatInfo")
            {
                TlItemCapture curItem = GetCurItem();
                pbFloatInfo.Enabled = !isViewOnly && TellerVars.Instance.IsAppOnline && curItem != null && curItem.ValidateItemFloatInfo() &&
                    _tlTranSet != null && _tlTranSet.CurTran.PostRealTime.Value == 1  && // #14912
                     curItem.PopulateFloatInfo(_tlTranSet.TellerHelper, _tlTranSet.CurTran.TranCode.Value ); // #79420, #15071
                // Begin #161243
                pbApplyFloat.Enabled = pbFloatInfo.Enabled && curItem.FloatDays.Value == 0 &&
                    curItem.FloatBal.Value == 0 && curItem.UserExceptRsnCode.IsNull  ;
                // End #161243
            }
            // End #79420, #13851

            // Begin #161243
            else if (callerInfo == "ShowComplete")
            {
                pbTranAcctDisp.Enabled = ScreenHasAccess("TranAcctDisplay");
            }
            // End #161243
		}

		private bool IsBlankRow(int rowId)
		{
            if (!string.IsNullOrEmpty(_tellerVars.DefaultShortCode))   //#80618
            {
                if (string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAcctType.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAcctNo.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colCheckNo.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAmount.ColumnId)))
                {

                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colBankCode.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colRoutingNo.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colCheckType.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAcctType.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAcctNo.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colCheckNo.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colTranCode.ColumnId)) &&
                    string.IsNullOrEmpty(gridCapturedItems.GetCellValueFormatted(rowId, colAmount.ColumnId)))
                {

                    return true;
                }
            }
			//
			return false;
		}

		private void RemoveBlankRow()
		{
            bool rowRemoved = false;        //#17924
            if (!isViewOnly)
			{
				for( int rowId = 0; rowId < gridCapturedItems.Count; rowId++ )
				{
                    if (IsBlankRow(rowId) && rowId == (gridCapturedItems.Count - 1 ) && newRowAdded )   // #14448 - added && - delete only newly added empty last row.
                    {
                        //Begin #05144
                        #region remove from structure
                        string indexRow = gridCapturedItems.GetCellValueFormatted(rowId, colIndex.ColumnId);

                        if (!string.IsNullOrEmpty(indexRow ) && items.Count >= Convert.ToInt32(indexRow) && items.Count > 0 )
                        {
                            if (Convert.ToInt32(indexRow) > 0)
                            {
                                //Begin #79420
                                //items.RemoveAt(Convert.ToInt32(indexRow) - 1);
                                if (items.Count > rowId)
                                    items.RemoveAt(rowId);
                                //End #79420
                            }
                            else if ((Convert.ToInt32(indexRow) == 0))//else part added for WI4168
                            {
                                items.RemoveAt(Convert.ToInt32(indexRow));
                            }
                        }
                        #endregion
                        //End #05144
                        gridCapturedItems.RemoveRow(rowId);
                        rowRemoved = true;        //#17924
                    }
				}
                if ( rowRemoved )
                    ResetRowCountColumn();  //#17924
				//
				CalcTotal();
                newRowAdded = false;        // #14448
			}
		}

		private void HandleWhiteSpace( TlItemCapture item )
		{
			if (item.AcctType.Value == String.Empty)
				item.AcctType.SetValue(null, EventBehavior.None);		// #6615 - item.AcctType.SetValueToNull();
			if (item.RoutingNo.Value == String.Empty)
				item.RoutingNo.SetValue(null, EventBehavior.None);		// #6615 - item.RoutingNo.SetValueToNull();
			if (item.ApplType.Value == String.Empty)
				item.ApplType.SetValue(null, EventBehavior.None);		// #6615 - item.ApplType.SetValueToNull();
			if (item.BankCode.Value == String.Empty)
				item.BankCode.SetValue(null, EventBehavior.None);		// #6615 - item.BankCode.SetValueToNull();
			if (item.AcctNo.Value == String.Empty)
				item.AcctNo.SetValue(null, EventBehavior.None);			// #6615 - item.AcctNo.SetValueToNull();

		}

		private bool SetAcctFields( string acctType, string acctNo, PGridColumn acctNoField,
			PGridColumnComboBox acctTypeField )
		{
			bool retValue = false;
			acctTypeField.CodeValue = acctType;
			if ( acctTypeField.SelectedIndex >= 0 )
			{
				SetMask( acctTypeField, acctNoField );
				if ( acctNo != null && acctNo != String.Empty )
					retValue = SetAcctNoField( acctNoField, acctTypeField );
				else
				{
					ValidateGrid(colAcctType.XmlTag, true);
					retValue = true;
				}
			}
			return retValue;
		}

		private bool SetAcctNoField( PGridColumn acctNoField,
			PGridColumnComboBox acctTypeField )
		{
			string applType = null;
			string depLoan = null;
			string format = null;
			string acctNo = acctNoField.Text;
            string realAcctNo = ""; //#76458
            string maskedAcctNo = ""; //#76458

			try
			{
				if ( acctNo != null && acctNo != string.Empty && acctNo != "" && acctTypeField.Text != null )
				{
					GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );
                    #region #80620
                    if (colDepLoan.UnFormattedValue == null)
                        colDepLoan.UnFormattedValue = depLoan;
                    #endregion
                    if ( format != null && format != String.Empty )
					{
						acctNoField.UnFormattedValue = _tellerHelper.FormatAccount( acctNo, format );
					}
					else
                    {
                        //Begin #76458
                        //acctNoField.UnFormattedValue = acctNo;
                        realAcctNo = acctNo;
                        if (acctTypeField.CodeValue != null)
                        {
                            if (_tlTranSet.IsExternalAdapterAcct(acctTypeField.CodeValue.ToString()))
                            {
                                maskedAcctNo = _tlTranSet.TellerVars.DisplayAcctNos.GetMaskedAcctNo(acctTypeField.CodeValue.ToString(), realAcctNo);
                                if (maskedAcctNo == realAcctNo || maskedAcctNo == "")
                                {
                                    maskedAcctNo = "";
                                    maskedAcctNo = _tlTranSet.GlobalHelper.GetMaskedExtAcct(acctTypeField.CodeValue.ToString(), realAcctNo);
                                    _tlTranSet.TellerVars.DisplayAcctNos.SetMaskedAcctNo(acctTypeField.CodeValue.ToString(), realAcctNo, maskedAcctNo);
                                }
                                acctNoField.UnFormattedValue = maskedAcctNo;
                            }
                            else
                                acctNoField.UnFormattedValue = acctNo;
                        }
                        //End #76458
                    }
                }
				return true;
			}
			catch(PhoenixException pe)
			{
				PMessageBox.Show(pe);
				return false;
			}
		}

		private string GetDepLoan( PGridColumnComboBox acctTypeField )
		{
			string applType = null;
			string depLoan = null;
			string format = null;
			GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );
			return depLoan;
		}

		private void PopulateAcctTypeCombo( PGridColumnComboBox acctTypeField, bool appendRM, bool appendGL )
		{

			try
			{
				string codeValue = null;
				string desc = null;
				bool acctTypeSrch = (this._tellerVars.AdTlControl.AcctTypeSearch.Value == GlobalVars.Instance.ML.Y );
                string depLoanFilter = "";  //#76458
                short cmbType = 0;    //#76458

                //Begin #76458 - added additional cmbType for account type combo cache
                depLoanFilter = _tlTranSet.GetTfrAcctTypeDepLoanFilter(_tlTranSet.CurTran.TranAcct.TranCode.Value, out cmbType);
                //if (this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType"] == null)
                if (this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType" + cmbType.ToString()] == null)
                {
                    this._tellerHelper.FilterByDepLoan.Value = depLoanFilter;
                    this._tellerHelper.TlWhereClause.Value = (acctTypeSrch ? "Y" : "N");
                    this._tellerHelper.SkipSafeDeposit.Value = 1;
                    CallXMThruCDS("AcctTypePopulate");
                    this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType" + cmbType.ToString()] = this._tellerHelper.AcctType.Constraint.EnumValues;
                }
                else
                {
                    //#74448 - Selva - Added the following fix to bypass addrange overload since it is not working and there is framework fix for it.
                    //this._tellerHelper.AcctType.Constraint.EnumValues.AddRange(this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType"]);
                    foreach (EnumValue en in this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType" + cmbType.ToString()])
                    {
                        ((Phoenix.FrameWork.BusFrame.Constraint)_tellerHelper.AcctType.Constraint).Add(en); //74448
                    }
                }

				acctTypeField.Reset();
                acctTypeField.Append(this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType" + cmbType.ToString()]);
                //End #76458

				if ( appendGL )
				{
					codeValue = GlobalVars.Instance.ML.GL;
					desc = GlobalVars.Instance.ML.GL + "~" + GlobalVars.Instance.ML.GL + "~" + this._tellerVars.XpControl.GlAcctNoFormat.Value;

					acctTypeField.Append(codeValue, desc);
				}
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void SetMask( PGridColumnComboBox acctTypeField, PGridColumn acctNoField )
		{
			try
			{
				string applType = null;
				string depLoan = null;
				string format = null;
				GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );

				if ( format != null && format != string.Empty )
					acctNoField.PhoenixUIControl.InputMask = format;
				else
					acctNoField.PhoenixUIControl.InputMask = String.Empty;
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}

		}

		private void GetAcctTypeDetails( PGridColumnComboBox acctTypeField,
			ref string applType, ref string depLoan, ref string format )
		{

			try
			{
				string[] applDetails = null;
				// hack
				if ( acctTypeField.PhoenixUIControl.BusObjectProperty == null )
					acctTypeField.PhoenixUIControl.BusObjectProperty = this._tellerHelper.AcctType;

				if ( acctTypeField.CodeValue == null || acctTypeField.Text == "" || acctTypeField.Text == String.Empty )
				{
					return;
				}

				applDetails = acctTypeField.GetEnumValue(acctTypeField.UnFormattedValue.ToString()).Description.Split("~".ToCharArray());
				applType = applDetails[0];
				depLoan = applDetails[1];
				format = applDetails[2];
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}

		}

		private void CallXMThruCDS(string origin)
		{
			if (origin == "AcctTypePopulate")
			{
				Phoenix.FrameWork.CDS.DataService.Instance.EnumValues( this._tellerHelper, this._tellerHelper.AcctType );
			}
			else if (origin == "LoadCurTran")
			{
				Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_tlJournal);
			}
            //Begin #79420
            else if (origin == "MiscComboPopulate")
            {
                Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_busObjCapturedItems, _busObjCapturedItems.ExceptRsnCode);
            }
            //End #79420
		}

		private void LoadTranCodeCombo( string tcList)
		{
			string[] tempList = null;
			string delimiterCarat = "^";
			char[] delimiter = delimiterCarat.ToCharArray();
			if (tcList.Length > 0 && _tcListLoaded == false) //#71490
			{
				tempList = tcList.Split(delimiter);
				foreach(string tc in tempList)
				{
					colTranCode.Append(tc, tc);
					_tcListLoaded = true;
				}
			}
		}

		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;
			PfwStandard tempDlg = null;

			try
			{
				if ( origin == "AdHocReceipt" )
				{
					_reprintFormId = new PSmallInt("FormId");
                    _noCopies = new PDecimal("NoCopies");
                    _noCopies.Value = 1;
                    _printerService = new PString("PrinterService");
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt" );
					//tempDlg.InitParameters( _reprintInfo, _reprintFormId, this.ScreenId, _checkItemInfo );
                    //tempDlg.InitParameters( _reprintInfo, _reprintFormId, _reprintFormId.Value, null, null, _noCopies );
                    tempDlg.InitParameters(_reprintInfo, _reprintFormId, this.ScreenId, _checkItemInfo, null, _noCopies, _printerService);

				}
				else if ( origin == "PrintForms" )
				{
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgPrintForms" );
					//#71553 - replaced param value 1 with -1 for param lastLinePrinted
					tempDlg.InitParameters( _reprintFormId.Value, _checkPrintNo, -1, _wosaPrintInfo,
						_checkItemInfo);
				}
				else if ( origin == "CheckInfo" )
				{
					_checkInfoRimNo = _tlTranSet.CurTran.RimNo.Value;
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgCheckInfo" );
					tempDlg.InitParameters( _checkInfoRimNo, _wosaPrintInfo );
				}
				else if ( origin == "GetGLAcct" )
				{
					tempWin = Helper.CreateWindow( "phoenix.client.glacctsearch","Phoenix.Client.GlAcctSearch","frmGlAccountSearch");
					tempWin.InitParameters( ScreenId, Phoenix.Shared.Constants.UserConstants.ORIGIN_GetAcct,1, "Teller", false,
						null, null, null, null, null, null );
					tempWin.Closed +=new EventHandler(tempWin_Closed);
				}
				else if ( origin == "GetAcct" )
				{
					tempWin = Helper.CreateWindow("phoenix.client.acctsearch", "Phoenix.Client.Search", "frmGloAccountList" );
					#region Comment - Parameter Meaning
					//				//-1, Embedded Window , 0 - From MDI, > 0 Coming from Other Windows for Search
					//				paramScreenId = Convert.ToInt32(paramList[0]);
					//				/RimNo
					//				_paramRimNo = Convert.ToInt32(paramList[1]);
					//				//Records to Select
					//				_paramSingleSelect = Convert.ToBoolean(paramList[2]);
					//				//Origin of the call
					//				_paramOrigin = Convert.ToInt32(paramList[3]);
					//				//New Window title
					//				_paramTitle = Convert.ToString(paramList[4]);
					//				//Dep Loan
					//				_paramDepLoan = Convert.ToString(paramList[5]);
					#endregion
					//We have No RimNo, and No DepLoan prefarence
					tempWin.InitParameters(Shared.Constants.ScreenId.CapturedItems, -1, 1, Shared.Constants.UserConstants.ORIGIN_GetAcct, string.Empty, string.Empty);
					tempWin.Closed +=new EventHandler(tempWin_Closed);
				}
                // Begin #74015
                else if (origin == "DisplayClick" && gridCapturedItems.ContextRow >= 0)
                {
                    #region OnUsDisplayClick

                    // Begin #74488
                    if (isViewOnly)
                    {
                        if (colDepLoan.UnFormattedValue.ToString() == GlobalVars.Instance.ML.DP)
                        {
                            string timeTran = colDepType.UnFormattedValue.ToString() == GlobalVars.Instance.ML.Tran ? "frmDpDisplayTran" : "frmDpDisplayTime";
                            tempWin = Helper.CreateWindow("phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", timeTran);
                            tempWin.InitParameters(colAcctType.UnFormattedValue, colAcctNo.UnFormattedValue);
                        }
                        else if (colDepLoan.UnFormattedValue.ToString() == GlobalVars.Instance.ML.LN)
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.lndisplay", "Phoenix.Client.Loan", "frmLnDisplay");
                            tempWin.InitParameters(colAcctType.UnFormattedValue, colAcctNo.UnFormattedValue);
                        }
                        else if (colDepLoan.UnFormattedValue.ToString() == GlobalVars.Instance.ML.SD)
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.sddisplay", "Phoenix.Client.Sdb", "frmSdDisplay");
                            tempWin.InitParameters(colAcctType.UnFormattedValue, colAcctNo.UnFormattedValue, colAcctId.UnFormattedValue);
                        }
                    }
                    // End #74488
                    else
                    {
                        TlItemCapture currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;

                        if (currentItem.ItemAcct.DepLoan.Value == GlobalVars.Instance.ML.DP)
                        {
                            string timeTran = currentItem.ItemAcct.DepType.Value == GlobalVars.Instance.ML.Tran ? "frmDpDisplayTran" : "frmDpDisplayTime";
                            tempWin = Helper.CreateWindow("phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", timeTran);
                            tempWin.InitParameters(currentItem.ItemAcct.AcctType.Value, currentItem.ItemAcct.AcctNo.Value);
                        }
                        else if (currentItem.ItemAcct.DepLoan.Value == GlobalVars.Instance.ML.LN)
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.lndisplay", "Phoenix.Client.Loan", "frmLnDisplay");
                            tempWin.InitParameters(currentItem.ItemAcct.AcctType.Value, currentItem.ItemAcct.AcctNo.Value);
                        }
                        else if (currentItem.ItemAcct.DepLoan.Value == GlobalVars.Instance.ML.SD)
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.sddisplay", "Phoenix.Client.Sdb", "frmSdDisplay");
                            tempWin.InitParameters(currentItem.ItemAcct.AcctType.Value, currentItem.ItemAcct.AcctNo.Value, currentItem.ItemAcct.AcctId);
                        }
                    }
                    #endregion
                }
                else if (origin == "SignatureClick" && gridCapturedItems.ContextRow >= 0)
                {
                    #region OnUsSigners
                    // Begin #74488
                    if (isViewOnly)
                    {
                        tempWin = Helper.CreateWindow("Phoenix.Client.DpLnSigners", "Phoenix.Client", "frmDpLnSigners");
                        tempWin.InitParameters(colAcctType.UnFormattedValue,
                                                colAcctNo.UnFormattedValue,
                                                colRimNo.UnFormattedValue,
                                                colAcctId.UnFormattedValue);
                    }
                    // End #74488
                    else
                    {
                        TlItemCapture currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                        tempWin = Helper.CreateWindow("Phoenix.Client.DpLnSigners", "Phoenix.Client", "frmDpLnSigners");
                        tempWin.InitParameters(currentItem.ItemAcct.AcctType.Value,
                                                currentItem.ItemAcct.AcctNo.Value,
                                                currentItem.ItemAcct.RimNo.Value,
                                                currentItem.ItemAcct.AcctId.Value);
                    }
                    #endregion
                }
                // End #74015
                else if (origin == "OnUsRestrictionsClick")
                {
                    #region Restrictions Click
                    if (isViewOnly) //#17068
                    {
                        tempWin = Helper.CreateWindow("Phoenix.Client.GbHoldStop", "Phoenix.Client.HoldStop", "frmGbStopHold");
                        tempWin.InitParameters(colAcctType.UnFormattedValue,
                                                colAcctNo.UnFormattedValue,
                                                colDepLoan.UnFormattedValue,
                                                colRimNo.UnFormattedValue);
                    }
                    else
                    {
                        TlItemCapture currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                        tempWin = Helper.CreateWindow("Phoenix.Client.GbHoldStop", "Phoenix.Client.HoldStop", "frmGbStopHold");
                        tempWin.InitParameters(currentItem.ItemAcct.AcctType.Value,
                                                currentItem.ItemAcct.AcctNo.Value,
                                                currentItem.ItemAcct.DepLoan.Value,
                                                currentItem.ItemAcct.RimNo.Value);
                    }
                    #endregion
                }
                //Begin #79420, #13851
                else if (origin == "FloatInfo")
                {
                    exceptRsnCode.SetValue(null);
                    floatDays.SetValue(null);

                    PBaseType calcFloatDays = null;        //#79420, #15357
                    short globalRegCFloatDays = 0;        //#161243, #30710
                    if (!zeroFloatCall)
                    {
                        TlItemCapture curItem = GetCurItem();
                        if (curItem != null)
                        {
                            if (!curItem.ExceptRsnCode.IsNull)
                                exceptRsnCode.Value = curItem.ExceptRsnCode.Value;

                            // Begin #161243
                            if (curItem.ApplyNewExceptCodeLogic)
                            {
                                if (!curItem.UserExceptRsnCode.IsNull)
                                    exceptRsnCode.Value = curItem.UserExceptRsnCode.Value;
                            }
                            // End #161243

                            if (!curItem.FloatDays.IsNull)
                                floatDays.Value = curItem.FloatDays.Value;

                            calcFloatDays = curItem.CalcFloatDays;      //#79420, #15357

                            if ( !curItem.GlobalRegCFloatDays.IsNull )
                                globalRegCFloatDays = curItem.GlobalRegCFloatDays.Value;    //#161243, #30710
                        }
                    }

                    tempDlg = Helper.CreateWindow("phoenix.client.tlcaptureditems", "Phoenix.Client.Teller", "dlgTlFloatInfo");
                    tempDlg.InitParameters( !_tlTranSet.CurTran.TranEffectiveDt.IsNull ? _tlTranSet.CurTran.TranEffectiveDt.Value : _tlTranSet.TellerVars.PostingDt,
                        zeroFloatCall, exceptRsnCode, floatDays, _tlTranSet.CurTran.PostRealTime.Value != 1,        // #14799 - Added the last para
                        calcFloatDays,    //#79420, #15357 - Added calcFloatDays
                        globalRegCFloatDays);  //#161243, #30710
                }
                //End #79420, #13851
                #region Suspect Dtl - #80660
                else if (origin == "SuspectDetail")
                {
                    tempWin = CreateWindow("Phoenix.Client.GbSuspectAlertControl", "Phoenix.Client.Global", "frmSuspiciousTranDetail");
                    tempWin.InitParameters(decimal.MinValue,
                                            decimal.MinValue,
                                            decimal.MinValue,
                                            Convert.ToDecimal(colSuspectPtid.UnFormattedValue),
                                            colAcctType.Text,
                                            colAcctNo.UnFormattedValue,
                                            null,
                                            null);
                }
                #endregion

                // Begin #161243
                else if (origin == "TranAcctDisplayClick" )
                {
                    string acctType = _tlTranSet.CurTran.TranAcct.AcctType.Value;
                    string acctNo = _tlTranSet.CurTran.TranAcct.AcctNo.Value;
                    string depLoan = _tlTranSet.CurTran.TranAcct.DepLoan.Value;
                    string depType = _tlTranSet.CurTran.TranAcct.DepType.Value;
                    string applType = _tlTranSet.CurTran.TranAcct.ApplType.Value;
                    int rimNo = _tlTranSet.CurTran.TranAcct.RimNo.IsNull ? 0 : _tlTranSet.CurTran.TranAcct.RimNo.Value;
                    int acctId = _tlTranSet.CurTran.TranAcct.AcctId.IsNull ? -1 : _tlTranSet.CurTran.TranAcct.AcctId.Value;

                    if (depLoan == GlobalVars.Instance.ML.DP && string.IsNullOrEmpty(depType))
                    {
                        if (applType == "CD")
                            depType = GlobalVars.Instance.ML.Time;
                        else
                            depType = GlobalVars.Instance.ML.Tran;
                    }


                    if (depLoan  == GlobalVars.Instance.ML.DP)
                    {
                        string timeTran = _tlTranSet.CurTran.TranAcct.DepType.Value == GlobalVars.Instance.ML.Time ? "frmDpDisplayTime" : "frmDpDisplayTran";
                        tempWin = Helper.CreateWindow("phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", timeTran);
                        tempWin.InitParameters(acctType, acctNo);
                    }
                    else if (depLoan == GlobalVars.Instance.ML.LN)
                    {
                        tempWin = Helper.CreateWindow("phoenix.client.lndisplay", "Phoenix.Client.Loan", "frmLnDisplay");
                        tempWin.InitParameters(acctType, acctNo);
                    }
                    else if (depLoan == GlobalVars.Instance.ML.SD)
                    {
                        tempWin = Helper.CreateWindow("phoenix.client.sddisplay", "Phoenix.Client.Sdb", "frmSdDisplay");
                        tempWin.InitParameters(acctType, acctNo, acctId);
                    }
                    else if (depLoan == GlobalVars.Instance.ML.CM)  /*Begin #7860*/
                    {
                        tempWin = Helper.CreateWindow("phoenix.client.cmtdisplay", "Phoenix.Client.Commitment", "frmIraLnCommitmentDisplay");
                        tempWin.InitParameters(acctType, acctNo, rimNo);
                    }       /*End #7860*/
                    else if (depLoan == GlobalVars.Instance.ML.Ext || depLoan == "EX")
                    {
                        if (_tlTranSet.IsExternalAdapterAcct(acctType))
                        {
                            //Launch XAML Window
                            ShowFormHelper.XAMLWindow(this, "ExAcctDisplay", acctType, acctNo, rimNo);
                        }
                        else
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.exAcctdisplay", "Phoenix.Client.Ex", "frmOnExAcctDisp1");
                            tempWin.InitParameters(acctType, acctNo);
                        }
                        //End #76458
                    }
                    else if (depLoan == GlobalVars.Instance.ML.RM)
                    {
                        tempWin = Helper.CreateWindow("phoenix.client.rmdisplay", "Phoenix.Client", "frmRmDisplay");
                        tempWin.InitParameters(rimNo);
                    }

                }
                // End #161243

				if ( tempWin != null )
				{
					tempWin.Workspace = this.Workspace;
					tempWin.Show();
				}

				else if ( tempDlg != null )
				{
					dialogResult = tempDlg.ShowDialog(this);
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

		private bool ValidateAcctType()
		{
//			if (this._tellerVars.ComboCache["frmTlCapturedItems.colAcctType"].BinarySearch(colAcctType.Text) == -1)
//				return false;
			//SetMask( colAcctType , colAcctNo ); //#6917
			SetAcctNoField( colAcctNo, colAcctType );
			LoadTranCodeCombo(_busObjCapturedItems.GetTCComboList(GetDepLoan(colAcctType)));
			return ValidateGrid(colAcctType.XmlTag, true);
		}

		private bool ValidateAcctNo()
		{
            //Begin #76429
            if (colAcctNo != null)
            {
                //_busObjCapturedItems.IncomingAcctNo.Value = Convert.ToString(colAcctNo.UnFormattedValue);
                _busObjCapturedItems.ItemAcct.IncomingTfrAcctNo.Value = Convert.ToString(colAcctNo.UnFormattedValue);
                _busObjCapturedItems.IncomingAcctNo.Value = _busObjCapturedItems.ItemAcct.IncomingTfrAcctNo.Value;
                _busObjCapturedItems.ItemAcct.SwapAcct.Value = 0;
                this.colIncomingAcctNo.UnFormattedValue = this.colAcctNo.UnFormattedValue; //#2224
            }
            //End #76429

			SetAcctNoField( colAcctNo, colAcctType );
			if (!ValidateGrid(colAcctNo.XmlTag, true))
			{
                //Begin #76429
                _busObjCapturedItems.IncomingAcctNo.Value = _busObjCapturedItems.ItemAcct.IncomingTfrAcctNo.Value;
                //End #76429
				this.colAcctNo.Text = String.Empty;
				return false;
			}

            // Begin WI#13780 - Validate GL account
            if (!ValidateGlAccess(colAcctNo.Text, colAcctType.Text))
            {
                // Clear out Invalid GL Account
                colAcctNo.Text = String.Empty;
                colAcctNo.Focus();
                return false;
            }
            // End WI#13780

            //Begin #76429
            _busObjCapturedItems.IncomingAcctNo.Value = _busObjCapturedItems.ItemAcct.IncomingTfrAcctNo.Value;
            //End #76429
            //Begin #76458
            if (!_busObjCapturedItems.AcctNo.IsNull && !_busObjCapturedItems.AcctType.IsNull &&
                !_busObjCapturedItems.TranCode.IsNull && _busObjCapturedItems.TranCode.Value == 570)
            {
                _exMaskedAcct = _gbHelper.GetMaskedExtAcct(_busObjCapturedItems.AcctType.Value, _busObjCapturedItems.AcctNo.Value);
                _tlTranSet.TellerVars.DisplayAcctNos.SetMaskedAcctNo(_busObjCapturedItems.AcctType.Value,
                    _busObjCapturedItems.AcctNo.Value,
                    _exMaskedAcct);
                this.colAcctNo.UnFormattedValue = _exMaskedAcct;
                this.colAcctNo.Text = _exMaskedAcct;
                this.colRealAcctNo.UnFormattedValue = _busObjCapturedItems.AcctNo.Value;
                this.colRealAcctNo.Text = _busObjCapturedItems.AcctNo.Value;
            }
            //End #76458
            #region #80620
            /* Get note info, enable/disable pbOnUsNotes and show notes if they exist */
            if (_tlTranSet.TellerVars.IsAppOnline && colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() == mlOnUs && _tlTranSet.TellerVars.AdTlCls.ShowOnusAcctNotes.Value == "Y")    //#80618 #39946
            {
                TlAcctDetails acctDetails = new TlAcctDetails();

                acctDetails.AcctNo.Value = this.colAcctNo.Text;
                acctDetails.AcctType.Value = this.colAcctType.Text;

                acctDetails.ApplType.Value = this.colAcctApplType.Text;
                acctDetails.DepLoan.Value = this.colDepLoan.Text;
                acctDetails.CustomType.Value = 1;
                acctDetails.TfrRefAccount.Value = 1;
                acctDetails.SelectAllFields = true;

                acctDetails.ActionType = XmActionType.Select;
                //Begin #140895
                if (IsCreatingTlCaptureTranSet)
                {
                    if (_tlTranSet.CurTran.Items.Count > gridCapturedItems.ContextRow)
                    {
                        acctDetails.CachedAcctDetails = (_tlTranSet.CurTran.Items[gridCapturedItems.ContextRow] as TlItemCapture).ItemAcct;
                    }
                }
                //End #140895
                CoreService.DataService.ProcessRequest(acctDetails);
                Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(acctDetails, "LoadTfrNotePtids"); // #11412
                string[] notesList = null;
                int notesCount = 0;
                bool notePtidFound = false;
                int nNotePtid;
                ShowNotes = false;
                FormExtension t = Extension as FormExtension; // #11412

                if (!acctDetails.CustomParam1.IsNull)
                {
                    notesCount = acctDetails.CustomParam2.IntValue + acctDetails.CustomParam3.IntValue;
                    notesList = acctDetails.CustomParam1.StringValue.Split(',');
                    if (notesList.Length > 0)
                    {

                        pbOnUsNotes.Enabled = true;
                        ShowNotes = true;
                        t.IsNotesAvailable = true; // #11300
                    }
                    else
                    {
                        pbOnUsNotes.Enabled = false;
                        t.IsNotesAvailable = false; // #11412
                    }
                }
this.EnableDisableVisibleLogic("Alerts");// #11300
                if (notesCount > 0 && ShowNotes && _tlTranSet.TellerVars.AdTlCls.ShowOnusAcctNotes.Value == "Y")
                {
                    //Begin #140895
                    if (IsCreatingTlCaptureTranSet && !this.Visible)
                    {
                        _firstNotesRow = gridCapturedItems.ContextRow;
                        return true;
                    }
                    //End #140895

                    #region #11412
                    NoteInfo = new PNotes();
                    NoteInfo.ScreenPtid = acctDetails.RimNo.DecimalValue;
                    NoteInfo.ScreenId = this.ScreenId;
                    NoteInfo.AcctNo = colAcctNo.Text;
                    NoteInfo.AcctType = colAcctType.Text;
                    NoteInfo.RimNo = acctDetails.RimNo.IntValue;
                    #endregion
                    if (notesCount > 1)
                        Extension.ShowNotes(this);
                    else
                    {
                            // send msg to show show pop up notes
                            Extension.Reset();
                            #region #4889
                            foreach (string notePtid in notesList)
                            {
                                nNotePtid = Convert.ToInt32(notePtid);
                                notePtidFound = false;
                                foreach (int notePtidFromList in notePtids)
                                {
                                    if (notePtidFromList == nNotePtid)
                                    {
                                        notePtidFound = true;
                                        break;
                                    }
                                }
                                if (!notePtidFound)
                                {
                                    notePtids.Add(nNotePtid);
                                    Extension.AddPopup(nNotePtid);
                                }
                            }
                            #endregion
                            Extension.ShowNextNote(this);
                    }
                }
            }
            #endregion
            return true;
		}

		private void LoadCurTran()
		{
			#region select transaction info
			_tlJournal.BranchNo.Value = branchNo.Value;
			_tlJournal.DrawerNo.Value = drawerNo.Value;
			_tlJournal.EffectiveDt.Value = effectiveDt.Value;
			_tlJournal.Ptid.Value = journalPtid.Value;
			_tlJournal.ActionType = XmActionType.Select;
			_tlJournal.OutputType.Value = 4;	// #72362 - this will get TC desc
			//
			CallXMThruCDS("LoadCurTran");
			//
			#region add tcd fields
			if (!_tlJournal.TcdDrawerNo.IsNull)
			{
				if (!_tlJournal.TcdCashOut.IsNull)
					_wosaPrintInfo.TCDCashOut = _tlJournal.TcdCashOut.Value;
				if (!_tlJournal.TcdCashIn.IsNull)
					_wosaPrintInfo.TCDCashIn = _tlJournal.TcdCashIn.Value;
				_wosaPrintInfo.TCDDrawerNo = _tlJournal.TcdDrawerNo.Value;

			}
			#endregion
			//
			#region add common print fields
			if (!_tlJournal.CashIn.IsNull)
				_wosaPrintInfo.CashInAmt = _tlJournal.CashIn.Value;
			if (!_tlJournal.CashIn.IsNull)
			_wosaPrintInfo.CashOutAmt = _tlJournal.CashOut.Value;
			if (!_tlJournal.CheckNo.IsNull)
				_wosaPrintInfo.CheckNo = _tlJournal.CheckNo.Value;
			if (!_tlJournal.ChksAsCash.IsNull)
				_wosaPrintInfo.ChksAsCashAmt = _tlJournal.ChksAsCash.Value;
			if (!_tlJournal.CcAmt.IsNull)
				_wosaPrintInfo.FeeAmt = _tlJournal.CcAmt.Value;
			if (!_tlJournal.IntAmt.IsNull)
				_wosaPrintInfo.IntPaidAmt = _tlJournal.IntAmt.Value;
			if (!_tlJournal.OnUsChks.IsNull)
				_wosaPrintInfo.OnUsChksAmt = _tlJournal.OnUsChks.Value;
			if (!_tlJournal.RimNo.IsNull)
				_wosaPrintInfo.RimNo = _tlJournal.RimNo.Value;
			if (!_tlJournal.SequenceNo.IsNull)
				_wosaPrintInfo.SequenceNo = _tlJournal.SequenceNo.Value;
			if (!_tlJournal.NetAmt.IsNull)
				_wosaPrintInfo.TranAmt = _tlJournal.NetAmt.Value;
			if (!_tlJournal.TransitChks.IsNull)
				_wosaPrintInfo.TransitChksAmt = _tlJournal.TransitChks.Value;
			if (!_tlJournal.UtilityId.IsNull)
				_wosaPrintInfo.UtilityId = _tlJournal.UtilityId.Value;
			if (!_tlJournal.AcctNo.IsNull)
				_wosaPrintInfo.AcctNo = _tlJournal.AcctNo.Value;
			if (!_tlJournal.AcctType.IsNull)
				_wosaPrintInfo.AcctType = _tlJournal.AcctType.Value;
			if (!_tlJournal.CashIn.IsNull)
			_wosaPrintInfo.GbTranCode = _tlJournal.TranCode.Value;
			if (!_tlJournal.TfrAcctType.IsNull)
				_wosaPrintInfo.TfrAcct = _tlJournal.TfrAcctType.Value;
			if (!_tlJournal.TfrAcctNo.IsNull)
				_wosaPrintInfo.TfrAcctNo = _tlJournal.TfrAcctNo.Value;
			//
			if (!_tlJournal.TlTranCode.IsNull)
				_wosaPrintInfo.TlTranCode = _tlJournal.TlTranCode.Value;
			if (!_tlJournal.BatchId.IsNull)
				_wosaPrintInfo.BatchId = _tlJournal.BatchId.Value;
			if (!_tlJournal.ItemCount.IsNull)
				_wosaPrintInfo.ItemCount = Convert.ToInt32(dfNoOfItems.UnFormattedValue); // #71051
			if (_tlJournal.TlTranCode.Value == CoreService.Translation.GetUserMessageX(360092)) //BAT
				_wosaPrintInfo.ItemCount = Convert.ToInt32(dfNoOfItems.UnFormattedValue); // #71051
			if (!_tlJournal.TfrChkNo.IsNull)
				_wosaPrintInfo.TfrChkNo = _tlJournal.TfrChkNo.Value;
			if (!_tlJournal.BranchNo.IsNull)
				_wosaPrintInfo.BranchNo = _tlJournal.BranchNo.Value;
			if (!_tlJournal.DrawerNo.IsNull)
				_wosaPrintInfo.TellerDrawer = _tlJournal.DrawerNo.Value;
			if (!_tlJournal.EffectiveDt.IsNull)
				_wosaPrintInfo.PostingDate = _tlJournal.EffectiveDt.Value;
			if (!_tlJournal.SequenceNo.IsNull)
				_wosaPrintInfo.SequenceNo = _tlJournal.SequenceNo.Value;
			if (!_tlJournal.TranEffectiveDt.IsNull)
				_wosaPrintInfo.TranEffDt = _tlJournal.TranEffectiveDt.Value;
			if (!_tlJournal.TranDescription.IsNull)
				_wosaPrintInfo.TlTranCodeDesc = _tlJournal.TranDescription.Value;
			if (!_tlJournal.UtilityId.IsNull)
				_wosaPrintInfo.UtilityId = _tlJournal.UtilityId.Value;
			//Add Duplicate since its reprint time
			_wosaPrintInfo.Duplicate = true; //#71135
			#endregion
			//
			#region add tran status fields
			if (!_tlJournal.TranStatus.IsNull)
			{
				foreach(EnumValue e in _tlJournal.TranStatus.Constraint.EnumValues )
				{
					if ( Convert.ToInt32(e.CodeValue) == _tlJournal.TranStatus.Value )
					{
						_wosaPrintInfo.TranStatus = e.Description;
						break;
					}
				}
			}
			#endregion

			/* Begin #72362 */
			#region get global/acct data
			if ( _tlTranSet != null )
			{
				_tlTranSet.CurTran.BranchNo.Value = _tlJournal.BranchNo.Value;
				_tlTranSet.CurTran.DrawerNo.Value = _tlJournal.DrawerNo.Value;
				_tlTranSet.CurTran.EffectiveDt.Value = _tlJournal.EffectiveDt.Value;
				_tlTranSet.CurTran.EmplId.Value = _tlJournal.EmplId.Value;
				_tlTranSet.GetGlobalPrintInfo( _wosaPrintInfo );
				if ( !_tlJournal.AcctType.IsNull && !_tlJournal.AcctNo.IsNull )
				{
					_tlTranSet.CurTran.TranAcct.AcctType.Value = _tlJournal.AcctType.Value;
					_tlTranSet.CurTran.TranAcct.AcctNo.Value = _tlJournal.AcctNo.Value;
					_tlTranSet.CurTran.TranAcct.GetAcctReprintData( _gbHelper );
					if ( !_tlTranSet.CurTran.TranAcct.Title1.IsNull )
						_wosaPrintInfo.AcctTitle1 = _tlTranSet.CurTran.TranAcct.Title1.Value;
				}
			}
			#endregion
			/* End #72362 */
			#endregion
		}

		private void LoadItemPrintInfo()
		{
			#region add check print fields info
			_wosaPrintInfo.ItemNo = Convert.ToInt32(colItemNo.UnFormattedValue);
			_wosaPrintInfo.ItemRoutingNo = colRoutingNo.Text;
			_wosaPrintInfo.ItemCheckType = colCheckType.Text;
			_wosaPrintInfo.ItemAcctType = colAcctType.Text;
			_wosaPrintInfo.ItemAcctNo = colAcctNo.Text;
			if (colCheckNo.UnFormattedValue != null)
				_wosaPrintInfo.ItemCheckNo = Convert.ToInt32(colCheckNo.UnFormattedValue);
			else
				_wosaPrintInfo.ItemCheckNo = DbInt.Null;	// #71741

			if (colTranCode.UnFormattedValue != null)
				_wosaPrintInfo.ItemTranCode = Convert.ToInt32(colTranCode.UnFormattedValue);
			else
				_wosaPrintInfo.ItemTranCode = DbInt.Null;	// #71741

			_wosaPrintInfo.ItemAmount = Convert.ToDecimal(colAmount.UnFormattedValue);

			if (colTranEffectiveDt.UnFormattedValue != null)
				_wosaPrintInfo.ItemTranEffDt = Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue);
			else
				_wosaPrintInfo.ItemTranEffDt = DbDateTime.Null;	// #71741

			_wosaPrintInfo.ItemCreateDt = Convert.ToDateTime(colCreateDt.UnFormattedValue);
			#endregion
		}

		private bool LoadPrintObject(bool isReprintAll)
		{
			if (_tellerVars.AdTlControl.WosaPrinting.Value != GlobalVars.Instance.ML.Y)
			{
				PMessageBox.Show(this, 360622, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return false;
			}
			_reprintInfo = CoreService.Translation.GetUserMessageX(360621);
			if (!isReprintAll)
			{
				_tempChkAmount = colAmount.MakeFormattedValue(colAmount.UnFormattedValue);
				_tempChkAmount = _tempChkAmount.PadLeft(14, '*');
				_tempChkAmount = _tempChkAmount.Replace("*", " ");
				//
				_checkItemInfo = colItemNo.UnFormattedValue.ToString() + " - " + colCheckType.Text + _tempChkAmount;
			}
			else
				_checkItemInfo = CoreService.Translation.GetUserMessageX(360652);

			//
			CallOtherForms("AdHocReceipt");
			//
			if (_reprintFormId.IsNull || _reprintFormId.Value == 0)
				return false;
			//
			#region get form info
			if (_tellerVars.SetContextObject("AdTlFormArray", null, _reprintFormId.Value))
			{
				_reprintTextQrp = _tellerVars.AdTlForm.TextQrp.Value;
				_partialPrintString = _tellerVars.AdTlForm.PrintString.Value;
                _wosaServiceName = _printerService.Value; //WI3475
				//_wosaServiceName = _tellerVars.AdTlForm.LogicalService.Value;
				//_logicalService = _tellerVars.AdTlForm.ServiceType.Value;
				_mediaName = _tellerVars.AdTlForm.MediaName.Value;
				_formName = _tellerVars.AdTlForm.FormName.Value;
			}
			#endregion
			//
			#region load cur tran
			LoadCurTran();
			#endregion
			//
			return true;
			//
		}

		private void HandlePrinting()
		{
			try
			{
				if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
				{
					_xfsPrinter = new XfsPrinter(_wosaServiceName);	//#157637
					_xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo);  //#157637
				}
			}
			finally
			{
				_xfsPrinter.Close();	//#157637
			}
		}

		//#72095

        // Begin #74778
        private void RecordFailedAccess(int rimNo)
        {
                if (rimNo <= 0)
                    return;

                _adGbRsmFailedAccess = new AdGbRsmFailedAccess();
                _adGbRsmFailedAccess.ActionType = XmActionType.New;
                _adGbRsmFailedAccess.RimNo.Value = rimNo;
                _adGbRsmFailedAccess.EmployeeId.Value = _tlTranSet.TellerVars.EmployeeId;
                _adGbRsmFailedAccess.AccessTime.Value = System.DateTime.Now;
                _adGbRsmFailedAccess.CopyBackStatus.Value = 1;

                CallXMThruCDS("RecordFailedAccess");

        }
        // End #74778
		private void ResetGridIndex()
		{
			object objectValue = null;
			int itemPos = 0;
			int count = 1;
			//
			for( int rowId = 0; rowId < gridCapturedItems.Count; rowId++ )
			{

				itemPos = 0;
				//
				objectValue = gridCapturedItems.GetCellValueUnformatted( rowId, colIndex.ColumnId );
				if (objectValue == null)
					itemPos = 0;
				else
					itemPos = Convert.ToInt32(objectValue);
				//
				if (itemPos > 0)
				{
					gridCapturedItems.SetCellValueUnFormatted(rowId, colIndex.ColumnId, count);
					count++;
				}
			}
            ResetRowCountColumn();  //#17924
		}

        //Begin #17924
        private void ResetRowCountColumn()
        {
            for (int rowId = 0; rowId < gridCapturedItems.Count; rowId++)
            {
                gridCapturedItems.SetCellValueUnFormatted(rowId, colRowCount.ColumnId, rowId + 1);                //End #17924
            }
            if (gridCapturedItems.Count <= 9 && _bumpedRowCountWidth)
            {
                _bumpedRowCountWidth = false;
                colRowCount.Width = colRowCount.Width / 2;
                colBankCode.Width += colRowCount.Width;
                //this.UpdateView();
            }
        }
        //End #17924

        //Begin #76041
        private string GetNameRim()
        {
            StringBuilder nameRim = new StringBuilder(string.Empty);

            if (isViewOnly)
            {
                nameRim.Append(this.colRimNo.UnFormattedValue.ToString());
                nameRim.Append(" - ");
                //Begin #1661
                //nameRim.Append(_gbHelper.ConcateNameX(this.colRimFirstName.UnFormattedValue.ToString(),
                //                              this.colRimMiddleInitial.UnFormattedValue.ToString(),
                //                              this.colRimLastName.UnFormattedValue.ToString(),
                //                              false));
                nameRim.Append(_gbHelper.ConcateNameX((this.colRimFirstName.UnFormattedValue == null ? string.Empty : this.colRimFirstName.UnFormattedValue.ToString()),
                                              (this.colRimMiddleInitial.UnFormattedValue == null ? string.Empty : this.colRimMiddleInitial.UnFormattedValue.ToString()),
                                              (this.colRimLastName.UnFormattedValue == null ? string.Empty : this.colRimLastName.UnFormattedValue.ToString()),
                                              false));
                //End #1661
            }
            else if (items != null
                    && gridCapturedItems.ContextRow >= 0
                    && items.Count > gridCapturedItems.ContextRow
                    && colCheckType.UnFormattedValue != null
                    && colCheckType.UnFormattedValue.ToString() == mlOnUs)
            {
                TlItemCapture currentItem = currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                nameRim.Append(currentItem.ItemAcct.RimNo.Value);
                nameRim.Append(" - ");
                nameRim.Append(_gbHelper.ConcateNameX(currentItem.ItemAcct.RimFirstName.Value,
                                                        currentItem.ItemAcct.RimMiddleInitial.Value,
                                                        currentItem.ItemAcct.RimLastName.Value,
                                                        false));
            }

            return nameRim.ToString();
        }
        private string GetNameRimJoint()
        {
            StringBuilder nameRim = new StringBuilder(string.Empty);

            if (_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline)  //#1395)
            {
                if (isViewOnly)
                {
                        nameRim.Append(_busObjCapturedItems.GetJointDetails(colAcctType.UnFormattedValue.ToString(), colAcctNo.UnFormattedValue.ToString()));
                }
                else if (items != null
                        && gridCapturedItems.ContextRow >= 0
                        && items.Count > gridCapturedItems.ContextRow
                        && colCheckType.UnFormattedValue != null
                        && colCheckType.UnFormattedValue.ToString() == mlOnUs)
                {
                    TlItemCapture currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                    nameRim.Append(_busObjCapturedItems.GetJointDetails(currentItem.ItemAcct.AcctType.Value, currentItem.ItemAcct.AcctNo.Value));
                }
            }
            return nameRim.ToString();
        }
        private string GetOnUsSignatures()
        {
            string sigs = string.Empty;

            if (isViewOnly)
            {
                if (colNoSignatures.UnFormattedValue != null && (int)colNoSignatures.UnFormattedValue > 1) //#76458 - added null check
                    sigs = colNoSignatures.UnFormattedValue.ToString() + " " + CoreService.Translation.GetUserMessageX(360006);
            }
            else if (items != null
                    && gridCapturedItems.ContextRow >= 0
                    && items.Count > gridCapturedItems.ContextRow
                    && colCheckType.UnFormattedValue != null
                    && colCheckType.UnFormattedValue.ToString() == mlOnUs)
            {
                TlItemCapture currentItem = currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;

                if (!currentItem.ItemAcct.NoSignatures.IsNull && currentItem.ItemAcct.NoSignatures.Value > 1) //#76458 - added null check
                    sigs = currentItem.ItemAcct.NoSignatures.Value.ToString() + " " + CoreService.Translation.GetUserMessageX(360006);
            }

            return sigs;
        }
        private string GetOnUsDepLoan()
        {
            string depLoan = string.Empty;

            if (isViewOnly)
            {
                if (colDepLoan.UnFormattedValue != null) //#74744 03/26/2008
                    depLoan = colDepLoan.UnFormattedValue.ToString();
            }
            else if (items != null
                    && gridCapturedItems.ContextRow >= 0
                    && items.Count > gridCapturedItems.ContextRow
                    && colCheckType.UnFormattedValue != null
                    && colCheckType.UnFormattedValue.ToString() == mlOnUs)
            {
                TlItemCapture currentItem = currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                depLoan = currentItem.ItemAcct.DepLoan.Value;
            }

            return depLoan;
        }
        private void OnUsInformation()
        {
            string onUsDepLoan;
            TlItemCapture currentItem;

            this.setLabels();


            if (_tlTranSet.TellerVars.AdTlCls.ShowTitles.Value == "Y")
            {
                onUsDepLoan = this.GetOnUsDepLoan();
                if (isViewOnly)
                {
                    //#2029 - indended the on-us title logic under the following if condition
                    if (colDepLoan.UnFormattedValue != null)    //meant for only on-us items
                    {
                        onUsDepLoan = colDepLoan.UnFormattedValue.ToString();

                        if (onUsDepLoan == GlobalVars.Instance.ML.DP || onUsDepLoan == GlobalVars.Instance.ML.LN ||
                            onUsDepLoan == GlobalVars.Instance.ML.SD || onUsDepLoan == GlobalVars.Instance.ML.CM ||
                            onUsDepLoan == GlobalVars.Instance.ML.GL || onUsDepLoan == GlobalVars.Instance.ML.RM ||
                            onUsDepLoan == "EX" || onUsDepLoan == GlobalVars.Instance.ML.Ext) //#74744 03/26/2008 #76458 - added EX | #4050 - Added Ext
                        {
                            if (_adGbBankControl.InstitutionType.Value.ToUpper() == "CU" || _tlTranSet.TellerVars.AdTlCls.MapRimCust.Value == GlobalVars.Instance.ML.Y)
                            {
                                //Begin #01285
                                //this.dfOnUsTitle1.UnFormattedValue = this.GetNameRim();
                                //this.dfOnUsTitle2.UnFormattedValue = this.GetNameRimJoint();
                                TlAcctDetails _tlAcctDetails = new TlAcctDetails();
                                _tlAcctDetails.AcctType.Value = Convert.ToString(this.colAcctType.UnFormattedValue);
                                _tlAcctDetails.AcctNo.Value = Convert.ToString(this.colAcctNo.UnFormattedValue);
                                _tlAcctDetails.RimNo.Value = Convert.ToInt32(this.colRimNo.UnFormattedValue);
                                string rimName = null;
                                string jointRimName = null;
                                string primaryRimName = null;   //#34401
                                _tlAcctDetails.GetRimAndJointNames(true, ref rimName, ref jointRimName, ref primaryRimName);    //#34401
                                this.dfOnUsTitle1.UnFormattedValue = rimName;
                                this.dfOnUsTitle2.UnFormattedValue = jointRimName;
                                //End #01285
                            }
                            else
                            {
                                this.dfOnUsTitle1.UnFormattedValue = this.colTitle1.UnFormattedValue;
                                this.dfOnUsTitle2.UnFormattedValue = this.colTitle2.UnFormattedValue;
                            }
                            this.dfOnUsSignatures.UnFormattedValue = this.GetOnUsSignatures();
                            this.dfOnUsSignatures.ForeColor = Color.Red;
                        }
                    }
                }
                else if (items != null
                        && gridCapturedItems.ContextRow >= 0
                        && items.Count > gridCapturedItems.ContextRow
                        && colCheckType.UnFormattedValue != null
                        && colCheckType.UnFormattedValue.ToString() == mlOnUs)
                {
                    currentItem = currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                    onUsDepLoan = currentItem.ItemAcct.DepLoan.Value;
                    // #4050 - Added external account types
                    if (onUsDepLoan == GlobalVars.Instance.ML.DP || onUsDepLoan == GlobalVars.Instance.ML.LN ||
                        onUsDepLoan == GlobalVars.Instance.ML.SD || onUsDepLoan == GlobalVars.Instance.ML.CM ||
                        onUsDepLoan == GlobalVars.Instance.ML.EX || onUsDepLoan == GlobalVars.Instance.ML.Ext ||
                        onUsDepLoan == GlobalVars.Instance.ML.GL)
                    {
                        //#96359 if the selected account is GL, show the description of the GL account
                        if ( onUsDepLoan == GlobalVars.Instance.ML.GL )
                        {
                            this.dfOnUsTitle1.UnFormattedValue = currentItem.ItemAcct.Title1.Value;
                            this.dfOnUsTitle2.UnFormattedValue = currentItem.ItemAcct.Title2.Value;                            
                        }
                        else if (_adGbBankControl.InstitutionType.Value.ToUpper() == "CU" || _tlTranSet.TellerVars.AdTlCls.MapRimCust.Value == GlobalVars.Instance.ML.Y)
                        {
                            //Begin #01285
                            //this.dfOnUsTitle1.UnFormattedValue = this.GetNameRim();
                            //this.dfOnUsTitle2.UnFormattedValue = this.GetNameRimJoint();
                            TlAcctDetails _tlAcctDetails = currentItem.ItemAcct;
                            string rimName = null;
                            string jointRimName = null;
                            string primaryRimName = null;   //#34401
                            _tlAcctDetails.GetRimAndJointNames(true, ref rimName, ref jointRimName, ref primaryRimName);    //#34401
                            this.dfOnUsTitle1.UnFormattedValue = rimName;
                            this.dfOnUsTitle2.UnFormattedValue = jointRimName;
                            //End #01285
                        }
                        else
                        {
                            this.dfOnUsTitle1.UnFormattedValue = currentItem.ItemAcct.Title1.Value;
                            this.dfOnUsTitle2.UnFormattedValue = currentItem.ItemAcct.Title2.Value;
                        }
                        this.dfOnUsSignatures.UnFormattedValue = this.GetOnUsSignatures();
                        this.dfOnUsSignatures.ForeColor = Color.Red;
                    }
                }
                else
                {
                    this.dfOnUsTitle1.SetDefaultValue();
                    this.dfOnUsTitle2.SetDefaultValue();
                    this.dfOnUsSignatures.SetDefaultValue();
                }
                this.EnableDisableVisibleLogic("OnUs");
            }
            else
            {
                this.ClearTitleInfo();
                EnableDisableVisibleLogic("ClearTitle");
            }
        }
        private void setLabels()
        {
            if (_adGbBankControl.InstitutionType.Value.ToUpper() == "CU")
            {
                //#96359 if the account selected is GL, change the label to title, else the existing logic
                if (colAcctType.UnFormattedValue != null && colAcctType.UnFormattedValue.ToString() == GlobalVars.Instance.ML.GL)
                {
                    this.lblTitle1.Text = "Title 1:";
                    this.lblTitle2.Text = "Title 2:";
                }
                else
                {
                    this.lblTitle1.Text = "Member 1:";
                    this.lblTitle2.Text = "Member 2:";
                }
            }
            else if (_tlTranSet.TellerVars.AdTlCls.MapRimCust.Value == "Y")
            {
                //#96359 if the account selected is GL, change the label to title, else the existing logic
                if (colAcctType.UnFormattedValue != null && colAcctType.UnFormattedValue.ToString() == GlobalVars.Instance.ML.GL)
                {
                    this.lblTitle1.Text = "Title 1:";
                    this.lblTitle2.Text = "Title 2:";
                }
                else
                {
                    this.lblTitle1.Text = "Customer 1:";
                    this.lblTitle2.Text = "Customer 2:";
                }
            }
        }

        // Begin WI#13780
        // Purpose: Validates that the Ledger No of the GL Account passed has posting rights.
        // Returns: true is have gl posting access, false if not  (It also displays a message)
        private bool ValidateGlAccess(string acctNo, string acctType)
        {
            // Verify GlAccess Only During Online Mode
            if (_tlTranSet.TellerVars.IsAppOnline)
            {
                if (acctNo != null && acctNo != string.Empty && acctNo != "" && acctType != null && acctType == GlobalVars.Instance.ML.GL)
                {
                    // Parse Out Ledger And Prefix
                    string _sLedgerNo = string.Empty;
                    string _sPrefix = string.Empty;
                    string delimiterHyphen = "-";
                    char[] delimiter = delimiterHyphen.ToCharArray();
                    string _sTemp = acctNo;

                    string[] AcctArray = _sTemp.Split(delimiter);

                    _sLedgerNo = AcctArray[AcctArray.Length - 1];
                    _sPrefix = _sTemp.Substring(0, _sTemp.IndexOf(_sLedgerNo) - 1);

                    // Create Gl Acct Object
                    Phoenix.BusObj.GL.GlAcct _glAcct = new GlAcct();

                    // Call Vaidation Method
                    if (!_glAcct.ValidateGLEmplAccess(_sPrefix, _sLedgerNo))
                    {
                        //361300 - You do not have GL access rights to post transactions to any GL accounts with ledger %1!  Please modify your selection criteria or have your system administrator modify your GL access permissions.
                        PMessageBox.Show(this, 361300, MessageType.Error, MessageBoxButtons.OK, _sLedgerNo);
                        return false;
                    }
                }

            }

            return true;
        }
        // End WI#13780

        //Begin #3714
        private bool ClearRow(PGridColumn column)
        {
            if (gridCapturedItems.Count > 0)
            {
                if (column.PhoenixUIControl.BusObjectProperty.XmlTag == _busObjCapturedItems.BankCode.XmlTag)
                {
                    if (colRoutingNo.UnFormattedValue == null && colCheckType.UnFormattedValue == null)
                        return true;

                    //Begin #05144
                    if (_busObjCapturedItems.BankCode.IsNull)
                        return true;
                    //End #05144
                }
                if (column.PhoenixUIControl.BusObjectProperty.XmlTag == _busObjCapturedItems.RoutingNo.XmlTag)
                {
                    if (colBankCode.UnFormattedValue == null && colCheckType.UnFormattedValue == null)
                        return true;

                    //Begin #05144
                    if (_busObjCapturedItems.RoutingNo.IsNull)
                        return true;
                    //End #05144
                }
                if (column.PhoenixUIControl.BusObjectProperty.XmlTag == _busObjCapturedItems.CheckType.XmlTag)
                {
                    if (colBankCode.UnFormattedValue == null && colRoutingNo.UnFormattedValue == null)
                        return true;

                    //Begin #05144
                    if (_busObjCapturedItems.CheckType.IsNull)
                        return true;
                    //End #05144
                }



                //Let action save handle this...
                if (items != null && gridCapturedItems.ContextRow >= 0 && colIndex.UnFormattedValue != null &&
                items.Count > gridCapturedItems.ContextRow)
                {
                    TlItemCapture currentItem = items[gridCapturedItems.ContextRow] as TlItemCapture;
                    //
					currentItem.BankCode.SetValue(null, EventBehavior.None);		// #6615 - currentItem.BankCode.SetValueToNull();
					currentItem.RoutingNo.SetValue(null, EventBehavior.None);		// #6615 - currentItem.RoutingNo.SetValueToNull();
					currentItem.CheckType.SetValue(null, EventBehavior.None);		// #6615 - currentItem.CheckType.SetValueToNull();
					currentItem.ApplType.SetValue(null, EventBehavior.None);		// #6615 - currentItem.ApplType.SetValueToNull();
					currentItem.AcctType.SetValue(null, EventBehavior.None);		// #6615 - currentItem.AcctType.SetValueToNull();
					currentItem.AcctNo.SetValue(null, EventBehavior.None);			// #6615 - currentItem.AcctNo.SetValueToNull();
					currentItem.CheckNo.SetValue(null, EventBehavior.None);			// #6615 - currentItem.CheckNo.SetValueToNull();
					currentItem.TranCode.SetValue(null, EventBehavior.None);		// #6615 - currentItem.TranCode.SetValueToNull();
                    currentItem.NoItems.Value = 1;
                    if ( !TellerVars.Instance.ReorderAmtItems )         // #14448
					    currentItem.Amount.SetValue(null, EventBehavior.None);			// #6615 - currentItem.Amount.SetValueToNull();
                }
                //
                /*begin wi 4168*/

                //Begin #14448
                //With changes for #80618, there will always be an item in the collection for every row in the window. Creating a new item here
                //changes the item reference altogether in the collection
                 //if (items.Count !=0 && gridCapturedItems.ContextRow + 1 == gridCapturedItems.Count)
                 //    _busObjCapturedItems = new TlItemCapture();
                 //End #14448
                 //Begin #79420
                 if (_tlTranSet != null)
                     _busObjCapturedItems.EffectiveDt.Value = _tlTranSet.EffectiveDt.Value;
                 //End #79420
                 // Begin #79420, #13851
                 if (!isViewOnly && !_tlTranSet.CurTran.TranEffectiveDt.IsNull)
                     _busObjCapturedItems.TranEffectiveDt.Value = _tlTranSet.CurTran.TranEffectiveDt.Value;
                 // End #79420, #13851
                /*end wi 4168*/

                foreach(FieldBase field in _busObjCapturedItems.DbFields)
                {
                    if (field.XmlTag != "Amount" || !TellerVars.Instance.ReorderAmtItems)         // #14448
                        field.SetValue(null);		// #6615 - field.SetValueToNull();

                }
				_busObjCapturedItems.BankCode.SetValue(null, EventBehavior.None);		// #6615 - _busObjCapturedItems.BankCode.SetValueToNull();
				_busObjCapturedItems.ApplType.SetValue(null, EventBehavior.None);		// #6615 - _busObjCapturedItems.ApplType.SetValueToNull();
                _busObjCapturedItems.NoItems.Value = 1;
                ContextRowObjectToScreen(gridCapturedItems.ContextRow, null);

                if (_tellerVars.ReorderAmtItems)    //#80618
                    gridCapturedItems.NextFocusColumn = colBankCode; // #14448 replaced colAmount;
                else
                    gridCapturedItems.NextFocusColumn = colBankCode;
                if (column.PhoenixUIControl.BusObjectProperty.XmlTag == _busObjCapturedItems.BankCode.XmlTag)
                    return true;
                else
                    return false;
            }
            return true;
        }
        //End #3714

        private void gbOnUsAcctInformation_Enter(object sender, EventArgs e)
        {

        }

        private void frmTlCapturedItems_Load(object sender, EventArgs e)
        {

        }
        //End #76048

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
                    if (!(action == ActionClose || action == pbDisplay || action == pbSignature))
                        action.Enabled = false;
                }
            }
        }
        #endregion

        #region #80618
        /// <summary>
        /// This override method will receive user keystroke for plus key from 10-kay pad and mimic the add new row in case the endofrowvalidation() is successful.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_tellerVars.UsePlusKey && keyData == System.Windows.Forms.Keys.Add)
            {
                if (gridCapturedItems.ContextRow >= 0)
                {
                    //Begin #14448
                    //validate the focus column first
                    if (lastFocusCol != null ) //&& !ValidateGrid( lastFocusCol.XmlTag, true ))
                    {
                        PCancelEventArgs eventArg = new PCancelEventArgs(new CancelEventArgs());
                        //if (lastFocusCol.PhoenixUIValidateEvent != null)
                        //{
                        //    lastFocusCol.PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        //    if (eventArg.Cancel)
                        //    {
                        //        lastFocusCol.Focus();
                        //        return true;
                        //    }
                        //}

                        eventArg.Cancel = false;
                        bool routingCheckModified = false; // #14865

                        if (lastFocusCol.XmlTag == colAcctType.XmlTag)
                            colAcctType_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        else if (lastFocusCol.XmlTag == colAcctNo.XmlTag)
                            colAcctNo_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        else if (lastFocusCol.XmlTag == colBankCode.XmlTag)
                        {
                            if ( _busObjCapturedItems.BankCode.Value != Convert.ToString( colBankCode.UnFormattedValue ) )  // #14865 - ClearRow messing up the stuff
                                colBankCode_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        }
                        else if (lastFocusCol.XmlTag == colRoutingNo.XmlTag)
                        {
                            if (_busObjCapturedItems.RoutingNo.Value != Convert.ToString(colRoutingNo.UnFormattedValue))  // #14865 - ClearRow messing up the stuff
                            {
                                routingCheckModified = true;       // #14865
                                colRoutingNo_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                            }
                        }
                        else if (lastFocusCol.XmlTag == colAmount.XmlTag)
                            colAmount_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        else if (lastFocusCol.XmlTag == colCheckType.XmlTag)
                        {
                            if (_busObjCapturedItems.CheckType.Value != Convert.ToString(colCheckType.UnFormattedValue))  // #14865 - ClearRow messing up the stuff
                            {
                                routingCheckModified = true;       // #14865
                                colCheckType_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                            }
                        }
                        else if (lastFocusCol.XmlTag == colNoOfItems.XmlTag)
                            colNoOfItems_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                        else if (lastFocusCol.XmlTag == colTranCode.XmlTag)
                            colTranCode_PhoenixUIValidateEvent(lastFocusCol, eventArg);

                        if (eventArg.Cancel)
                        {
                            lastFocusCol.Focus();
                            return true;
                        }
                        //Begin #14865
                        else if (routingCheckModified)     // if routing/check is modfied , then the use need to re-enter the bank code/ check type/ routing based on #3714
                        {
                            if (gridCapturedItems.NextFocusColumn == colBankCode)
                            {
                                CalculateNxtDayAvl(-2, true, false, null);
                                try
                                {
                                    _IsFromSelectRow = true;    // to skip retriggering of the validate event due to set focus below.
                                    colBankCode.Focus();
                                    gridCapturedItems.NextFocusColumn = null;
                                    EnableDisableVisibleLogic("FloatInfo");
                                    SetReqExceptRsnRowColor(-1);  //#79420, #15357
                                }
                                finally
                                {
                                    _IsFromSelectRow = false;
                                }
                                return true;
                            }
                        }
                        //End #14865
                        //if (!lastFocusCol.DeactivateCell())
                        //    return true;
                    }
                    //End #14448
                    if (EndOfRowValidation())
                    {
                        LoadItems(false, false, null);
                        CalcTotal();
                        CalculateNxtDayAvl(-2, true, false, null);      //#14865
                        if (gridCapturedItems.ContextRow == gridCapturedItems.Count - 1)
                            AddNewRow();
                    }
                }
                else
                {
                    AddNewRow();
                }

                // newRowAdded = false;           // #14448 - The user has added manually the row.So do not treat as new row for clean up purpose       //#14789 - commneted
                return true;
            }
            //Begin #14789
            //force the focus to appropriate field by triggering the bank code validate event in case
            // the bank code is already defaulted to the default short code.
            if (keyData == System.Windows.Forms.Keys.Tab && lastFocusCol != null &&
                lastFocusCol.XmlTag == colBankCode.XmlTag && _tellerVars.DefaultShortCode != null &&
                Convert.ToString(colBankCode.UnFormattedValue) == _tellerVars.DefaultShortCode &&
                valBankCodeToFocus && gridCapturedItems.ContextRow == (gridCapturedItems.Count - 1))
            {

                // this is causing clear row to fire which cause other problems.
                //PCancelEventArgs eventArg = new PCancelEventArgs(new CancelEventArgs());
                //eventArg.Cancel = false;
                //colBankCode_PhoenixUIValidateEvent(lastFocusCol, eventArg);
                //if (eventArg.Cancel)
                //{
                //    lastFocusCol.Focus();
                //    return true;
                //}
                if (!ValidateGrid(colBankCode.XmlTag, true))
                {
                    lastFocusCol.Focus();
                    return true;
                }
                valBankCodeToFocus = false;
            }
            //#37398
            if (keyData == System.Windows.Forms.Keys.Tab && lastFocusCol != null &&
                lastFocusCol.XmlTag == colTranCode.XmlTag && _tellerVars.ReorderAmtItems && 
                gridCapturedItems.ContextRow == (gridCapturedItems.Count - 1))
            {
                _freezeDataInput = true;
                dlgInformation.Instance.ShowInfo("End of Row Validation...");
            }
            if (_freezeDataInput && (keyData == System.Windows.Forms.Keys.D0 || keyData == System.Windows.Forms.Keys.D1 || keyData == System.Windows.Forms.Keys.D2 ||
                keyData == System.Windows.Forms.Keys.D3 || keyData == System.Windows.Forms.Keys.D4 || keyData == System.Windows.Forms.Keys.D5 ||
                keyData == System.Windows.Forms.Keys.D6 || keyData == System.Windows.Forms.Keys.D7 || keyData == System.Windows.Forms.Keys.D8 ||
                keyData == System.Windows.Forms.Keys.D9 || keyData == System.Windows.Forms.Keys.NumPad0 || keyData == System.Windows.Forms.Keys.NumPad1 ||
                keyData == System.Windows.Forms.Keys.NumPad2 || keyData == System.Windows.Forms.Keys.NumPad3 || keyData == System.Windows.Forms.Keys.NumPad4 ||
                keyData == System.Windows.Forms.Keys.NumPad5 || keyData == System.Windows.Forms.Keys.NumPad6 || keyData == System.Windows.Forms.Keys.NumPad7 ||
                keyData == System.Windows.Forms.Keys.NumPad8 || keyData == System.Windows.Forms.Keys.NumPad9))
            {
                return true;
            }
            //End #14789
            return base.ProcessCmdKey(ref msg, keyData);
        }
        /// <summary>
        /// This method decides the next focus column based on admin setup.
        /// </summary>
        private void SetNextCellFocus()
        {
            if (_tellerVars.ReorderAmtItems)
                gridCapturedItems.NextFocusColumn = colAmount;
            else
            {
                if (_tellerVars.DefaultShortCode != string.Empty)
                    gridCapturedItems.NextFocusColumn = colRoutingNo;
                else
                    gridCapturedItems.NextFocusColumn = colBankCode;
            }
        }
        /// <summary>
        /// This method clears the bank code and related fields to wipe out the default values since the user changed the original bank code value.
        /// </summary>
        private void ValidateBankCode()
        {
            if (ClearRow(colBankCode))
            {
                if (!ValidateGrid(colBankCode.XmlTag, true))
                    return;

                if (!isViewOnly && colCheckType.UnFormattedValue != null && colCheckType.UnFormattedValue.ToString() != mlOnUs) //Selva - #76195
                    this.LoadItems();
            }
        }
        #endregion

        //Begin #79420
        private void dfMakeAvail_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            if (!(this.ActiveControl is PGrid || this.ActiveControl is IEmbeddedControl) &&
                this.ActionManager.OkToProcessAction && makeAvailEdited)   // #14888 - event is getting fired when action manager is processing some action.
            {
                if (CalculateNxtDayAvl(Convert.ToDecimal(dfMakeAvail.Value), true, false, dfMakeAvail))
                {
                    if (dfMakeAvail.UnFormattedValue == null || string.IsNullOrEmpty(dfMakeAvail.Text))
                    {
                        _tlTranSet.CurTran.MakeAvl.SetValue(null);
                    }
                    else
                    {
                        _tlTranSet.CurTran.MakeAvl.Value = Convert.ToDecimal(dfMakeAvail.Value);
                        //Begin #14448
                        string logInfo = "setting make avail value";
                        LogDebugInfo(logInfo, 5);
                        //End #14448
                    }
                    makeAvailEdited = false;
                }
                else if ( e!= null )
                    e.Cancel = true;
            }

        }

        // Begin #79420, #13851
        private bool CalculateNxtDayAvl(decimal totalAvailBal, bool vfyTotalAvail, bool forceRecalc,
            object sender)
        {
            return CalculateNxtDayAvl(totalAvailBal, vfyTotalAvail, forceRecalc, sender, false );
        }
        // End #79420, #13851

        private bool CalculateNxtDayAvl(decimal totalAvailBal, bool vfyTotalAvail, bool forceRecalc,
            object sender, bool recursiveCall ) // #13851
        {
            int retCode = 0;
            decimal totalCalcAvailBal = 0;
            int rowId = 0;

            bool resetMakeAvailField = (dfMakeAvail.UnFormattedValue == null || string.IsNullOrEmpty(dfMakeAvail.Text));


            try
            {
                if (!(_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline))
                    return true;
                if (!_busObjCapturedItems.PopulateFloatInfo(_tlTranSet.TellerHelper, _tlTranSet.CurTran.TranCode.Value))
                    return true;
                if ((sender != dfMakeAvail && totalAvailBal == -2) || resetMakeAvailField )
                {
                    if (_tlTranSet.CurTran.MakeAvl.IsNull || resetMakeAvailField )
                        totalAvailBal = -1;
                    else
                        totalAvailBal = _tlTranSet.CurTran.MakeAvl.Value;
                }

                decimal onUsChksAmt = 0;
                decimal chksAsCashAmt = 0;
                decimal transitChksAmt = 0;
                decimal origTotalAvailBal = totalAvailBal;
                int noItems = 0;

                _busObjCapturedItems.CalculateTotals(items, ref onUsChksAmt, ref chksAsCashAmt, ref transitChksAmt, ref noItems);
                if (chksAsCashAmt + transitChksAmt == 0)
                {
                    dfMakeAvail.SetValue(0,EventBehavior.None);
                    dfAvailRemaining.SetValue(Convert.ToDecimal(dfImmediateAvailAmt.Value) - Convert.ToDecimal(dfImmediateAvailUsed.Value) - (Convert.ToDecimal(dfMakeAvail.Value)));
                    if (Convert.ToDecimal(dfAvailRemaining.Value) < 0)  //#42006
                    {
                        dfAvailRemaining.SetValue(Convert.ToDecimal(dfAvailRemaining.Value) * -1);  //display it as a positive value
                    }
                    // Begin #14448
                    _tlTranSet.CurTran.CalcMakeAvl.SetValue(null);
                    _tlTranSet.CurTran.MakeAvl.SetValue(null);
                    // End #14448
                    if ( !_busObjCapturedItems.ReCalculateFloat(items)) // #14448
                        return true;
                }

                if (vfyTotalAvail && (totalAvailBal >= 0 || (sender == dfMakeAvail && !resetMakeAvailField)))
                    _busObjCapturedItems.ValidateMakeAvailAmt(items, Convert.ToDecimal(dfMakeAvail.Value), ref retCode);
                if (retCode != 0)
                {
                    if (sender != dfMakeAvail)
                    {
                        //Begin #14448
                        if (sender == ActionSave || sender == ActionClose)
                        {
                            dfMakeAvail.Focus();
                            return false;
                        }
                        else
                        {
                            totalAvailBal = -1;
                            origTotalAvailBal = -1;
                            _tlTranSet.CurTran.MakeAvl.SetValue(null);
                            retCode = 0;
                        }
                        //End #14448
                    }
                    else
                        return false;
                }

                //#14448 - removed check for totalAvailBal != prevTotalAvailBal when sender = dfMakeAvail
                if (!(forceRecalc || ( sender == dfMakeAvail ) || _busObjCapturedItems.ReCalculateFloat(items)))
                    return true;

                using (new WaitCursor())
                {
                    dlgInformation.Instance.ShowInfo("Calculating Float...");
                    string _enableRealTime = _tlTranSet.AdTlTc == null ? string.Empty : _tlTranSet.AdTlTc.RealTimeEnable.Value;/*#95650. Get enable_real_time flag for the Transaction.*/
                    _busObjCapturedItems.CalculateNxtDayAvl(items, _tlTranSet.CurTran.TranAcct.AcctType.Value,
                        _tlTranSet.CurTran.TranAcct.AcctNo.Value, _tlTranSet.CurTran.TranCode.Value, _enableRealTime,
                        _tlTranSet.CurTran.TranEffectiveDt.IsNull ? _tlTranSet.TellerVars.PostingDt : _tlTranSet.CurTran.TranEffectiveDt.Value,
                        ref totalAvailBal, ref retCode);/*#95650.Pass it to the method.*/
                    if (retCode != 0)
                        return false;



                    foreach (TlItemCapture item in items)
                    {
                        if (item.ValidateItemFloatInfo() && rowId < gridCapturedItems.Count && ( sender != ActionClose || _isSaveTriggered ))
                        {
                            //colNxtDayBal.FieldToColumn(item.NxtDayBal);
                            //colCalcNxtDayBal.FieldToColumn(item.CalcNxtDayBal);
                            totalCalcAvailBal += item.CalcNxtDayBal.Value;

                            if (!item.NxtDayBal.IsNull)
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colNxtDayBal.ColumnId, item.NxtDayBal.Value);
                            else
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colNxtDayBal.ColumnId, null);

                            if (!item.CalcNxtDayBal.IsNull)
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colCalcNxtDayBal.ColumnId, item.CalcNxtDayBal.Value);
                            else
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colCalcNxtDayBal.ColumnId, null);

                            decimal floatBal = Convert.ToDecimal(colAmount.UnFormattedValue) - Convert.ToDecimal(colNxtDayBal.UnFormattedValue);

                            gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBal.ColumnId, item.FloatBal.Value);

                            if (!item.FloatDays.IsNull)
                            {
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDays.ColumnId, item.FloatDays.Value);
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDate.ColumnId, item.FloatDate.Value);
                            }

                            // Begin #161243
                            if (!item.FloatBalExcept.IsNull)
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBalExcept.ColumnId, item.FloatBalExcept.Value);
                            else
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatBalExcept.ColumnId, null);

                            if (!item.FloatDaysExcept.IsNull)
                            {
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDaysExcept.ColumnId, item.FloatDaysExcept.Value);
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDateExcept.ColumnId, item.FloatDateExcept.Value);
                            }
                            else
                            {
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDaysExcept.ColumnId, null);
                                gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDateExcept.ColumnId, null);
                            }
                            // End #161243

                            //Begin #79420, #15357
                            if (!recursiveCall)
                            {
                                if (!item.ExceptRsnCode.IsNull)
                                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExceptRsnCode.ColumnId, item.ExceptRsnCode.Value);
                                else
                                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExceptRsnCode.ColumnId, null);
                            }
                            SetReqExceptRsnRowColor(rowId);
                            //End #79420, #15357


                            // Begin #79420, #13851
                            if (recursiveCall && exceptRsnCode.IntValue > 0 )
                            {
                                if (item.CalcFloatDays.Value == 0 && item.FloatDays.Value == floatDays.IntValue &&  ( item.FloatBal.Value > 0 ||
                                    (item.ReqExceptRsnCode.Value == GlobalVars.Instance.ML.Y && item.ExceptRsnCode.IsNull)))    //#79420, #15357 - if an exception reason is required , then assign the selected exception reason
                                {
                                    item.ExceptRsnCode.Value = Convert.ToInt16(exceptRsnCode.Value);
                                    gridCapturedItems.SetCellValueUnFormatted(rowId, colExceptRsnCode.ColumnId, item.ExceptRsnCode.Value);
                                    SetReqExceptRsnRowColor(rowId);  //#79420, #15357
                                }
                            }
                            // End #79420, #13851

                        }

                        rowId++;
                    }

                    _tlTranSet.CurTran.CalcMakeAvl.Value = totalCalcAvailBal;
                    if (origTotalAvailBal < 0)
                    {
                        //Begin #14448
                        if (totalAvailBal < 0)
                            totalAvailBal = 0;
                        //End #14448

                        dfMakeAvail.SetValue(totalAvailBal);
                    }
                    else if (totalAvailBal > origTotalAvailBal)
                    {
                        if (sender == dfMakeAvail)
                        {
                            rowId = 0;
                            int focusRow = -1;
                            foreach (TlItemCapture item in items)
                            {
                                if (item.ValidateItemFloatInfo())
                                {
                                    if (item.FloatDays.Value == 0)
                                    {
                                        focusRow = rowId;
                                        break;
                                    }
                                }
                                rowId++;
                            }

                            //Begin #13851
                            if (focusRow >= 0 && !recursiveCall)
                            {
                                //PMessageBox.Show(13492, MessageType.Warning, string.Empty);
                                //gridCapturedItems.ContextRow = focusRow;
                                //colFloatDays.Focus();
                                zeroFloatCall = true;
                                CallOtherForms("FloatInfo");
                                if (dialogResult == DialogResult.OK && floatDays.DecimalValue > 0)
                                {
                                    rowId = 0;
                                    foreach (TlItemCapture item in items)
                                    {
                                        if (item.ValidateItemFloatInfo())
                                        {
                                            if (item.FloatDays.Value == 0)
                                            {
                                                item.FloatDays.Value = Convert.ToInt16(floatDays.Value);
                                                item.FloatDate.Value = item.CalcExpiryDate(item.FloatDays.Value);   // #14649
                                                // Begin #161243
                                                if (exceptRsnCode.IntValue > 0 ) // && item.UserExceptRsnCode.IsNull )
                                                {
                                                    item.UserExceptRsnCode.Value = Convert.ToInt16(exceptRsnCode.Value);
                                                }
                                                // End #161243
                                            }
                                        }
                                        rowId++;
                                    }

                                    return CalculateNxtDayAvl(origTotalAvailBal, vfyTotalAvail, forceRecalc,
                                        sender, true);
                                }
                            }
                            //End #13851
                        }
                        //Begin #13851, #14448 - commented  below
                        //else
                        //{
                        //    //force the MakeAvail BO Field to calculated value...
                        //    _tlTranSet.CurTran.MakeAvl.Value = totalAvailBal;
                        //}
                        //End #13851
                        dfMakeAvail.SetValue(totalAvailBal);
                    }
                    dfAvailRemaining.SetValue(Convert.ToDecimal(dfImmediateAvailAmt.Value) - Convert.ToDecimal(dfImmediateAvailUsed.Value) - (Convert.ToDecimal(dfMakeAvail.Value)));
                    if (Convert.ToDecimal(dfAvailRemaining.Value) < 0)  //#42006
                    {
                        dfAvailRemaining.SetValue(Convert.ToDecimal(dfAvailRemaining.Value) * -1);  //display it as a positive value
                    }
                    if ( sender == dfMakeAvail  )
                        prevTotalAvailBal = totalAvailBal;

                    //gridCapturedItems.Focus();
                }

                return retCode == 0;
            }
            finally
            {
                dlgInformation.Instance.HideInfo();

                //Begin #79420, #15357
                //ProcessRetCode(retCode);
                if ( ProcessRetCode(retCode) )
                    SetRegCCFieldLargeDepNewAcctInfo();
                //End #79420, #15357
                EnableDisableVisibleLogic("FloatInfo");     // #161243
            }
        }

        private void colFloatDays_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            using (new WaitCursor())
            {
                if (!ValidateGrid(colFloatDays.XmlTag, true))
                    e.Cancel = true;
                else
                {

                }
            }
        }
        private bool ProcessRetCode(int retCode)
        {
            if (retCode != 0)
            {
                if (retCode < 0)
                {
                    returnCodeDesc = _gbHelper.GetSPMessageText(retCode, false);
                    PMessageBox.Show(this, 360532, MessageType.Warning, MessageBoxButtons.OK, new string[] { Convert.ToString(retCode) + " - " + returnCodeDesc });
                }
                else
                {
                    PMessageBox.Show(this, retCode, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                }
            }
            return retCode == 0;
        }

        private bool ValidateFloatOnSave( object sender )
        {
            bool isSuccess = true;
            if (_tlTranSet != null && _tlTranSet.TellerVars.IsAppOnline)
            {
                isSuccess = CalculateNxtDayAvl(-2, true, false, sender);
                if (isSuccess)
                {
                    int amtModifiedItemNo = -1;
                    int daysModifiedItemNo = -1;
                    int reqExceptRsnCodeItemNo = -1;    //#79420, #15357
                    int retCode = 0;

                    isSuccess = _busObjCapturedItems.ValidateFloatInfo(items, ref amtModifiedItemNo, ref daysModifiedItemNo, ref reqExceptRsnCodeItemNo, //#79420, #15357 - Added reqExceptRsnCodeItemNo
                        ref retCode);
                    if (!ProcessRetCode(retCode))
                    {
                        //Begin #79420, #15357
                        if (reqExceptRsnCodeItemNo > 0 && reqExceptRsnCodeItemNo <= gridCapturedItems.Count)
                        {
                            _IsFromSelectRow = true;
                            gridCapturedItems.SelectRow(reqExceptRsnCodeItemNo - 1, true);
                            _IsFromSelectRow = false;
                            gridCapturedItems.Columns[colExceptRsnCode.ColumnId].Focus();
                        }
                        //End #79420, #15357
                        else if (amtModifiedItemNo > 0)
                            ActionSave.ImageButton.Focus();
                        else if (daysModifiedItemNo > 0 && daysModifiedItemNo <= gridCapturedItems.Count)
                        {
                            _IsFromSelectRow = true;
                            gridCapturedItems.SelectRow(daysModifiedItemNo - 1, true);
                            _IsFromSelectRow = false;
                            gridCapturedItems.Columns[colExceptRsnCode.ColumnId].Focus();

                        }
                    }
                }
            }

            return isSuccess;

        }
        // Begin #79420, #13851
        private void pbFloatInfo_Click(object sender, PActionEventArgs e)
        {

            TlItemCapture item = null;
            int rowId = gridCapturedItems.ContextRow;

            item = GetCurItem();

            if (item == null)
                return;

            if (item.ValidateItemFloatInfo())
            {
                zeroFloatCall = false;
                // Begin #161243
                //CallOtherForms("FloatInfo");
                if (sender == pbApplyFloat)
                {
                    short caseByCaseExceptRsnCode = 1;
                    int newFloatDays = 0;
                    exceptRsnCode.SetValue( caseByCaseExceptRsnCode );

                    if (item.CalcFloatDays.Value > 0 && item.ExtendCalcDaysForExceptRsn(caseByCaseExceptRsnCode))
                        newFloatDays += item.CalcFloatDays.Value;

                    newFloatDays += item.GetExceptRsnFloatDays(caseByCaseExceptRsnCode);
                    floatDays.SetValue(newFloatDays);
                    dialogResult = DialogResult.OK;
                }
                else
                {
                    CallOtherForms("FloatInfo");
                }
                // End #161243
                if (dialogResult == DialogResult.OK)
                {
                    if (!floatDays.IsNull && item.FloatDays.Value != Convert.ToInt16(floatDays.Value))
                    {
                        item.FloatDays.Value = Convert.ToInt16(floatDays.Value);

                        string focusFieldName = null;
                        item.ValidateFloatDays(TellerVars.Instance, ref focusFieldName);

                        gridCapturedItems.SetCellValueUnFormatted(rowId, colFloatDays.ColumnId, item.FloatDays.Value);
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colExpireDate.ColumnId, item.FloatDate.Value);
                    }
                    if (!exceptRsnCode.IsNull)
                    {
                        // Begin #161243
                        if ( item.ExceptRsnCode.IsNull || item.ExceptRsnCode.Value !=  exceptRsnCode.IntValue )
                            item.UserExceptRsnCode.Value = Convert.ToInt16(exceptRsnCode.Value);
                        // End #161243
                        item.ExceptRsnCode.Value = Convert.ToInt16(exceptRsnCode.Value);
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colExceptRsnCode.ColumnId, item.ExceptRsnCode.Value);
                    }
                    else
                    {
                        item.ExceptRsnCode.SetValue(null);
                        item.UserExceptRsnCode.SetValue(null);   // #161243
                        gridCapturedItems.SetCellValueUnFormatted(rowId, colExceptRsnCode.ColumnId, item.ExceptRsnCode.Value);
                    }
                    CalculateNxtDayAvl(-2, true, false, null);
                    SetReqExceptRsnRowColor(-1);  //#79420, #15357
                }
                gridCapturedItems.Focus();       //#79420, #15357

                EnableDisableVisibleLogic("FloatInfo");     // #161243

            }

        }
        // End #79420, #13851

        //Begin #79420, #15357
        private void SetReqExceptRsnRowColor( int rowId )
        {
            TlItemCapture curItem;

            if (rowId < 0)
                rowId = gridCapturedItems.ContextRow;

            if (rowId >= 0 && rowId < items.Count)
                curItem = items[rowId] as TlItemCapture;
            else
                curItem = _busObjCapturedItems;

            if (rowId >= 0 && rowId < gridCapturedItems.Count )
            {
                if (!isViewOnly && TellerVars.Instance.IsAppOnline && curItem != null && curItem.ValidateItemFloatInfo() &&
                    _tlTranSet != null && _tlTranSet.CurTran.PostRealTime.Value == 1 &&
                    curItem.ReqExceptRsnCode.Value == GlobalVars.Instance.ML.Y &&
                    curItem.ExceptRsnCode.IsNull)
                {
                    gridCapturedItems.Items[rowId].ForeColor = Color.Red;
                }
                else
                {
                    gridCapturedItems.Items[rowId].ForeColor = Color.Black;
                }
            }

        }
        private void SetRegCCFieldLargeDepNewAcctInfo( )
        {
            bool largeDepFound = false;
            bool newAcctFound = false;

            foreach (TlItemCapture item in items)
            {
                if (item.ValidateItemFloatInfo())
                {
                    if (item.RegCcCode.Value == "L")
                        largeDepFound = true;
                    if (item.RegCcCode.Value == "N")
                        newAcctFound = true;
                }
            }
            string regCCText = Convert.ToString(dfRegCcCode.Value);
            string largeDepText = " (Large Deposit Float Applies)";
            string newAcctText = " (New Account Float Applies)";

            if (regCCText == null)
                regCCText = string.Empty;

            if (largeDepFound)
            {
                if (regCCText.IndexOf(newAcctText) >= 0)
                    regCCText = regCCText.Replace(newAcctText, string.Empty);
                if (regCCText.IndexOf(largeDepText) < 0)
                    regCCText = regCCText + largeDepText;
            }
            else if (newAcctFound)
            {
                if (regCCText.IndexOf(largeDepText) >= 0)
                    regCCText = regCCText.Replace(largeDepText, string.Empty);
                if (regCCText.IndexOf(newAcctText) < 0)
                    regCCText = regCCText + newAcctText;
            }
            else
            {
                if (regCCText.IndexOf(largeDepText) >= 0)
                    regCCText = regCCText.Replace(largeDepText, string.Empty);
                if (regCCText.IndexOf(newAcctText) >= 0)
                    regCCText = regCCText.Replace(newAcctText, string.Empty);
            }

            dfRegCcCode.SetValue(regCCText);
        }
        //End #79420, #15357

        private TlItemCapture GetCurItem()
        {
            TlItemCapture item = null;
            int rowId = gridCapturedItems.ContextRow;

            if (rowId >= 0 && rowId < items.Count)
                item = items[rowId] as TlItemCapture;
            else
                item = _busObjCapturedItems;

            return item;
        }
        //End #79420

        //private bool ValidateSingleRow(int rowId)
        //{
        //    TlItemCapture curItem = _busObjCapturedItems;
        //    int curContext = gridCapturedItems.ContextRow;

        //    try
        //    {
        //        if (items.Count <= rowId)
        //            return true;

        //        if (gridCapturedItems.Count > 0)
        //        {
        //            _busObjCapturedItems = (items[rowId] as TlItemCapture); //Selva - hack
        //        }

        //        ContextRowScreenToObject(rowId, false);
        //        //
        //        if (!ValidateGrid(null, false)) //Selva-New Change
        //        {
        //            if (!_busObjCapturedItems.ActionReturnCode.IsNull && _busObjCapturedItems.ActionReturnCode.Value != 0)
        //            {
        //                if (_busObjCapturedItems.ActionReturnCode.Value < 0)
        //                {
        //                    returnCodeDesc = _gbHelper.GetSPMessageText(_busObjCapturedItems.ActionReturnCode.Value, false);
        //                    PMessageBox.Show(this, 360532, MessageType.Warning, MessageBoxButtons.OK, new string[] { Convert.ToString(_busObjCapturedItems.ActionReturnCode.Value) + " - " + returnCodeDesc });
        //                }
        //                else
        //                {
        //                    PMessageBox.Show(this, _busObjCapturedItems.ActionReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
        //                }
        //            }
        //            //


        //            return false;

        //        }
        //        _busObjCapturedItems = curItem;
        //        gridCapturedItems.ContextRow = curContext;
        //        return true;
        //    }
        //    finally
        //    {
        //    }
        //}

        //private bool  ValidateEntireGrid()
        //{
        //    TlItemCapture curItem = _busObjCapturedItems;
        //    int curContext = gridCapturedItems.ContextRow;

        //    if (gridCapturedItems.Count == 0)
        //        return true;

        //    try
        //    {
        //        validatingGrid = true;
        //        if (!ValidateSingleRow(gridCapturedItems.ContextRow))
        //            return false;
        //        for (int rowId = 0; rowId < gridCapturedItems.Count; rowId++)
        //        {
        //            if (!ValidateSingleRow(rowId))
        //                return false;
        //        }
        //    }
        //    finally
        //    {
        //        validatingGrid = false;
        //    }
        //    _busObjCapturedItems = curItem;
        //    gridCapturedItems.ContextRow = curContext;

        //    return true;
        //}

        //Begin #14448
        public void LogDebugInfo(string debugText, int stackLevel)
        {
            if (CoreService.LogPublisher.DebugLevel == LogLevel.MoreDetailed)
            {
                string logInfo = debugText;

                if (stackLevel > 0)
                {
                    logInfo = logInfo + @"
STACK:";



                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);

                    //System.Diagnostics.StackFrame[] stackFrames = st.GetFrames();
                    //foreach (System.Diagnostics.StackFrame stackFrame in stackFrames)
                    //{
                    //    CoreService.LogPublisher.LogDebug(stackFrame.GetMethod().Name);   // write method name
                    //}

                    int stFrmCnt = st.GetFrames().Length;

                    if (stFrmCnt > stackLevel)
                        stFrmCnt = stackLevel;
                    for (int i = 0; i < stFrmCnt; i++)
                    {
                        System.Diagnostics.StackFrame sf = st.GetFrame(i);
                        logInfo += @"
" + sf.GetMethod().Name;
                    }
                }

                CoreService.LogPublisher.LogDebug(logInfo);

            }
        }
        //End #14448

        //Begin #140895
        void TlCaptureShowNotes()
        {
            ValidateAcctNo();
            if (Workspace.CurrentWindow != this)
            {
                Form popUpForm = Workspace.CurrentWindow as Form;
                if (popUpForm != null)
                {
                    popUpForm.FormClosed += new FormClosedEventHandler(popUpForm_FormClosed);
                    return;
                }
            }
            popUpForm_FormClosed(null, null);   // move to the next record if available else close
        }

        void popUpForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (gridCapturedItems.ContextRow + 1 < gridCapturedItems.Count)
            {
                gridCapturedItems.ContextRow = gridCapturedItems.ContextRow + 1;
                TlCaptureShowNotes();
            }
            else
            {
                this.Close();
            }
        }
        private void WriteToDebugLogDetailed(string logInput, bool isTellerCapture)
        {
            if (isTellerCapture)
                logInput = "Teller Capture:" + logInput;
            if (CoreService.LogPublisher.DebugLevel == LogLevel.MoreDetailed)
            {
                CoreService.LogPublisher.LogDebug(logInput);
            }
        }
        //End #140895

        // Begin #161243
        private void pbApplyFloat_Click(object sender, PActionEventArgs e)
        {
            pbFloatInfo_Click(pbApplyFloat, new PActionEventArgs());
        }

        private void pbTranAcctDisp_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("TranAcctDisplayClick");
        }
        // End #161243

    }
}
