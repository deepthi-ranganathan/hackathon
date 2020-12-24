#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlQuickTranCode.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
#endregion

#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//04/12/2018    2       mselvaga    Task#87717 
//04/13/2018    3       FOyebola    Enh#220316-Task#87721
//04/16/2018    4       FOyebola    Enh#220316-Task#87721-2
//04/19/2018    5       DGarcia     Enh#220316-Task#87723
//04/23/2018    6       FOyebola    Enh#220316-Task#87725
//04/30/2018    7       FOyebola    Enh#220316-Task#89883
//04/30/2018    8       DGarcia     Bug#89895
//05/02/2018    9       FOyebola    Enh#220316 - Task 89899
//05/04/2018    10      FOyebola    Enh#220316 - Task 89907
//05/09/2018    11      FOyebola    Enh#220316 - Task 87735
//05/16/2018    12      DGarcia     Task#89976 - Display and History Action buttons in Quick Transaction tab
//05/15/2018    13      FOyebola    Enh#220316 - Task 89979
//05/17/2018    14      CRoy        US #88994 Task #88995 - Add Clear all feature.
//05/21/2018    15      CRoy        Bug #91398 - Search info lost when coming from RMS.
//05/23/2018    16      DGarcia     Task#89967 Account Alerts & Notifications
//05/23/2018    17      CRoy        Bug #91948 - Add clear all for the # of items, checks in and the available.
//05/24/2018    19      CRoy        Bug #89987 - Add possibility to search again with ctrl + enter hotkey 
//05/25/2018    20      CRoy        Bug #91420 - Fix ctrl + enter key not being hit from every panel.
//05/15/2018    21      FOyebola    Enh#220316 - Task 89979
//05/23/2018    22      FOyebola    Enh#220316 - Task 87757
//05/23/2018    23      DGarcia     Bug#92189
//06/01/2018    24      CRoy        US #89316 Task #91954 - Add Save layout and restore default layout.
//06/04/2018    25      FOyebola    Enh#220316 - Bug 92696
//06/05/2018    26      CRoy        Bug 92488 - Search again with multiple account should bring back
//06/06/2018    27      CRoy        Bug #92023 - Item Capture should be disabled until a search is performed.
//06/07/2018    28      CRoy        Bug #92489 - Search again, do not reload if RIM or acct is the same.
//06/05/2018    26      FOyebola    Enh#220316 - Task 87759
//06/08/2018    27      DGarcia     Bug#93072
//06/08/2018    28      FOyebola    Enh#220316 - Task 87761
//06/08/2018    29      DGarcia     Bug#93075
//06/13/2018    30      FOyebola    Enh#220316 - Task 87751
//06/21/2018    31      FOyebola    Enh#220316 - Task 87753
//07/02/2018    32      CRoy        Task 89995 - Transfer alert information.
//07/05/2018    23      DGarcia     Task#94607/94608
//07/13/2018    24      CRoy        Bug #93040 - Message box not disposing properly.
//07/16/2018    25      DGarcia     Task#87763 - Support for Select Secondary functionality.
//07/23/2018    26      DGarcia     Bug #96236/96237/96238
//07/25/2018    27      CRoy        Task #87719 - Automatically resize accounts panel
//07/25/2018    28      FOyebola    Enh#220316 - Bug 95598
//07/25/2018    29      DGarcia     Task#87765 - Quick Acct Relationships
//07/30/2018    30      DGarcia     Bug#97017/97018
//08/02/2018    31      DGarcia     Bug#97688 - Select Secondary
//08/03/2018    32      DGarcia     Task#89981 - Cross Ref Accounts
//08/14/2018    33      CRoy        Bug #98535 - Transaction panel header buttons.
//08/17/2018    34      CRoy        Bug #98671 - Look and Feel
//08/17/2018    35      DGarcia     Task#97603 - Quick Fraud
//09/04/2018    36      CRoy        Bug #99048 - Cosmetic issues
//09/06/2018    37      CRoy        Bug #100897 - Refresh rim info when rms is updated.
//09/11/2018    38      DGarcia     Task#90007 - Offline teller for Quick Transactions
//09/25/2018    39      CRoy        Task#99053 - Add Transaction menu indicator.
//10/05/2018    40      DGarcia     Task#100769 - Quick Acct Confidential Accounts
//10/15/2018    41      CRoy	    Bug #102824 - Member Initiated ID is not feeding the RIM# into the Quick Tab
//10/15/2018    42      DGarcia     Task#102715 - Offline teller for Quick Transactions-Fix known issues.
//10/30/2018    43      DGArcia     Bug#103638 - eReceipt
//11/02/2018    44      CRoy        Task #103255 - Add validation before posting transaction
//11/14/2018    45      CRoy        Task #104614 - Minimize and maximize Quick Tran makes panels dissapear
//11/15/2018    46      CRoy        Bug #103512 - Offline mode enter key makes screen goes blank.
//11/27/2018    47      FOyebola    Enh#220316 - Task#104267 
//12/07/2018    48      mselvaga    Task#102719 - Add checks only AVTC transaction.
//01/07/2019    49      mselvaga    Task#100724 - CVT.
//01/10/2019    50      DGarcia     Bug #108450 - Search for rim with no account failing after a previous search
//01/11/2019    51      DGarcia     Bug #108545 - Clear Checks In and No Items everytime a search is initiated.
//01/11/2019    52      DGarcia     Bug #108552
//01/28/2019    53      DGarcia     Bug #109821
//01/30/2019    54      DGarcia     Bug #109869
//03/28/2019    55      CRoy        Task #112924 - Rename quick transactions for multi transactions in the designer.
//04/29/2019    56      CRoy        Task #113989 - Remove 0 from check number.
//05/03/2019    57      CRoy        Task #114200 - Add focus depending on institution type and always enable search button.
//05/07/2019    58      CRoy        Bug #114298 - Fix panel not showing insution type properly when CU
//05/09/2019    59      CRoy        Bug #114526 - When default tab is Quick tran, proper field is not selected
//05/20/2019    60      DGarcia     Task#115140 - Fixed issues with account number search not working with dashes. 
//05/31/2019    61      CRoy        Task #115070 - Account panel shrinking too much and hiding existing accounts.
//06/04/2019    62      CRoy        Bug #115691 - Fix preferences that are not being restored properly.
//06/05/2019    63      CRoy        Bug #115162 - Add screen id for Multi Tran.
//06/06/2019    64      DGarcia     Task#115070 - New Multi Tran Hotkeys for Transactions
//06/12/2019    65      CRoy        Task #115712 - Focus first incomplete row in transaction panel when using ctrl + down arrow.
//06/21/2019    66      CRoy        Task #115691 - Fix Quick tran preferences not being restored properly on SQL server 2016
//11/19/2019    67      FOyebola    Task#121713
//11/26/2019    68      FOyebola    Task#121714
//12/04/2019    69      FOyebola    Task#122355
//12/10/2019    70      FOyebola    Task#122356
//12/18/2019    71      mselvaga    Bug#122727 - MT Tab:  Add accelerator key 'G' on Get Acct button
//02/11/2020    72      mselvaga    Bug#124210 - Action panel not refreshed after removing all rows in Tran panel
//02/20/2020    73      mselvaga    Bug#124295 - MT 2020 - Open check column for Loan Advance
//02/20/2020    74      mselvaga    Bug#124210 - Fixed Column 'AcctNo' does not belong to table.
//02/21/2020    75      bhughes     121712:DEV - Application Insights.
//03/02/2020    76      mselvaga    #124249 - Added changes to remove empty transaction row.
#endregion

using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;
using Phoenix.BusObj.Global;
using Phoenix.FrameWork.Core;
using Phoenix.MultiTranTeller.Base;
using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.MultiTranTeller.Base.ViewModels;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phoenix.Shared.Variables; //Task#87721-2
using System.Collections; //87757
using Phoenix.BusObj.Admin.Global;
using Phoenix.Shared.Windows;
using Phoenix.FrameWork.BusFrame;
using System.IO;
using System.Xml.Linq;
using Phoenix.FrameWork.Core.Utilities;  //Task#100769
using DevExpress.XtraBars.Docking;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;

namespace Phoenix.Client.Teller
{
    public partial class frmTlQuickTranCode : PfwStandard, ITellerWindow, IQuickTranWindow
    {
        #region Private Variables
        private EasyCaptureTranSetViewModel _transactionSet = new EasyCaptureTranSetViewModel();
        ITlTranCodeWindow _tlTranCodeWindow;
        private bool _isProcessingMap = false;
        private int _appThreadId = Thread.CurrentThread.ManagedThreadId;

        public string _InstitutionType; //Task#87721

        private bool FocusOnAcctControl; //#87725

        private IPhoenixWorkspace _curWkspace;
        private string _curTcDescription;
        private string _curTranCode;
        private string _defaultLayout = ""; //Task #91954
        private EasyCaptureTran _curEasyCaptureTran;
        private bool _isQuickTranRealTime;
        private bool _isNewPref = false;
        private bool _isInitialized = false;
        private bool _hasCustomLayout = false;
        private bool _isCustomerPanelResized = false; //Bug #98671
        private AdGbRsmUiPref _rsmUiPref;
        public ArrayList _tfrAcctTypes = new ArrayList(); //87757
        public ArrayList _acctTypeDepLoanList = new ArrayList(); //Task#90007
        private object _tempHardHoldInfo;   //#87767
        private bool _allowHardHold;   //#87767
        private int _nextTranId = 0;  //#87767
        private int _curTranId = 0;  //#87767
        private bool _assumeDecimals;   //#87767
        private decimal _deviceDeposit; //#89998
        private string _currentLayout = ""; //Task 104614
        private DockPanel _previousActivePanel = null;//Task 104614
        private bool _isMinimized = false; //Task 104614
        //Begin Task#100769
        private AdGbRsm adGbRsm;
        private AdGbBankControl adGbBankControl;
        private bool _confAcctSecurity;
        private bool _userConfidentialAccess;
        private bool _lastControlIsGetAcct = false; //#122727
        private DataRow _currentAccountRow = null;  //#124210
        //End Task#100769

        #endregion
        //
        #region Constructor

        public frmTlQuickTranCode()
        {
            this.TrackWindowView = true; //121712
            this.PShowCompletedEvent += OnShowCompleted; //Task #114200
            SelectFirstControl = false; //Bug #114526
            InitializeComponent();
            //TBD - Task#87717
            //Begin Task#87721
            this.ucCustomer1.Visible = true;
            this.ucAccounts1.Visible = true;
            //End Task#87721
            this.ucCashIn1.Visible = true;
            this.ucTransactions1.Visible = true;
            this.ScreenId = 3674; //Bug #115162
            dockMgr.EndSizing += EndSizing; //Task #104614 
            dockMgr.EndDocking += EndDocking; //Task #104614
        }

        #endregion

        #region Overrides
        protected override void WndProc(ref Message m)
        {
            int WM_CONTEXTMENU = 0x7B;
            // Remove the Contet Menu from the workspace
            if (m.Msg == WM_CONTEXTMENU)
            {
                return;
            }

            base.WndProc(ref m);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Initialize the User Controls
            ucAccounts1.InitializeTeller(this);
            ucCustomer1.InitializeTeller(this);
            ucTransactions1.InitializeTeller(this);
            ucTransactions1.SetTfrAcctTypes();  //87757-3
            ucCashIn1.InitializeTeller(this);
            // Set the Tab Order
            dockPanelCustomer.TabIndex = 0;
            dockPanelCashIn.TabIndex = 1;
            dockPanelAccounts.TabIndex = 2;
            panelTransactions.TabIndex = 3;

            //Begin Task#87721-2
            _InstitutionType = Phoenix.Shared.Variables.GlobalVars.InstitutionType;
            ucCustomer1.SetInstitutionType(_InstitutionType);
            //End Task#87721-2
            ucCashIn1.EnableDisableControls("Default");

            ucCustomer1.OnSearchPerformed += (bool searchSucceeded) => { OnSearchPerformed(searchSucceeded); }; //Bug #92023
            ucCustomer1.OnSearchPerformed += ucTransactions1.OnSearchPerformed; //Bug#108552

            ucCustomer1.SetDockPanelSearchType(); //Bug #114298
            _isInitialized = true;
            PdfCurrency.ApplicationAssumeDecimal = _assumeDecimals;
            CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal;
            this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal;

            GetConfidentialSettings(); //Task#100769

            OfflineSettings(); //Task#90007 

            this.Resize += OnResize; //
            SelectFirstControl = false; //Bug #114526
            ucCustomer1.FocusProperField(); //Bug #114526
        }

        //Begin Task #114200
        private void OnShowCompleted(object sender, EventArgs e)
        {
            this.PShowCompletedEvent -= OnShowCompleted; //Bug #115691
            if (string.IsNullOrEmpty(_defaultLayout)) //Bug #115691
                this._defaultLayout = this.dockMgr.GetDockLayout(); //Task #91954 Bug #115691
            RestorePreferredLayoutIfExists(); //Task #91954 Bug #115691
            ucCustomer1.FocusProperField();
        }
        //End Task #114200

        //Begin Bug #98671
        private void OnResize(object sender, EventArgs e)
        {
            if (this.Width == 0 && this.Height > 0 || this.Width > 0 && this.Height == 0)
                return;

            HandleMinimizeLayoutChange(); //Task 104614

            if (!_isInitialized || _hasCustomLayout)
                return;

            Point offsetPointSecurityInfo = new Point(-673, 30);
            Point offsetPointReceipt = new Point(-664, 30);
            int heightIncrease = offsetPointSecurityInfo.Y;
            int currentHeightIncrease = 0;
            if (!_isCustomerPanelResized && this.Size.Width < ucCustomer1.GetMinimumSize())
            {
                _isCustomerPanelResized = true;
                currentHeightIncrease = heightIncrease;
                if (!ucCustomer1.IsSecurityInfoOffsetBy(offsetPointSecurityInfo))
                    ucCustomer1.OffsetSecurityInfoAndDelMethod(offsetPointSecurityInfo, offsetPointReceipt); //Bug #99048
            }
            else if (_isCustomerPanelResized && this.Size.Width > ucCustomer1.GetMinimumSize())
            {
                _isCustomerPanelResized = false;
                currentHeightIncrease = -heightIncrease;
                if (!ucCustomer1.IsSecurityInfoOffsetBy(new Point(-offsetPointSecurityInfo.X, -offsetPointSecurityInfo.Y)))
                    ucCustomer1.OffsetSecurityInfoAndDelMethod(new Point(-offsetPointSecurityInfo.X, -offsetPointSecurityInfo.Y),
                                                           new Point(-offsetPointReceipt.X, -offsetPointReceipt.Y)); //Bug #99048
            }

            if (currentHeightIncrease != 0)
            {
                Dictionary<PDxDockPanelUnderline, UserControl> controls = new Dictionary<PDxDockPanelUnderline, UserControl>()
                {
                    {dockPanelCustomer,ucCustomer1},
                    {dockPanelCashIn,ucCashIn1},
                    {dockPanelAccounts,ucAccounts1},
                };
                foreach (var control in controls)
                {
                    var panel = control.Key;
                    var usrControl = control.Value;
                    int panelWidth = panel.Size.Width;
                    int panelHeight = panel.Size.Height + currentHeightIncrease;
                    int controlWidth = usrControl.Size.Width;
                    int controlHeight = usrControl.Size.Height + currentHeightIncrease;
                    if (panelWidth > 0 && panelHeight > 0 && controlWidth > 0 && controlHeight > 0)
                    {
                        bool previousIsResizable = panel.IsResizable;
                        panel.IsResizable = true;
                        panel.Size = new Size(panelWidth, panelHeight);
                        usrControl.Size = new Size(controlWidth, controlHeight);
                        usrControl.Refresh();
                        panel.Refresh();
                        panel.IsResizable = previousIsResizable;
                    }
                }
            }
        }

        //Begin Task #104614

        private void HandleMinimizeLayoutChange()
        {
            bool isMinimizedNow = this.Width == 0 && this.Height == 0;
            bool restoreCurrentLayout = !isMinimizedNow && _isMinimized;

            if (restoreCurrentLayout)
            {
                this.Resize -= OnResize;
                this.dockMgr.EndSizing -= EndSizing;
                this.dockMgr.EndDocking -= EndDocking;
                ThreadPool.QueueUserWorkItem(o => {
                    Thread.Sleep(150);
                    dockMgr.LoadDockLayout(_currentLayout);
                    dockMgr.ActivePanel = _previousActivePanel;
                    this.Resize += OnResize;
                    this.dockMgr.EndSizing += EndSizing;
                    this.dockMgr.EndDocking += EndDocking;
                    _previousActivePanel = dockMgr.ActivePanel;
                    _currentLayout = dockMgr.GetDockLayout();
                    _isMinimized = this.Width == 0 && this.Height == 0;
                    if (!_isMinimized)
                    {
                        _previousActivePanel = dockMgr.ActivePanel;
                        _currentLayout = dockMgr.GetDockLayout();
                    }
                });
            }
            else
            {
                _isMinimized = this.Width == 0 && this.Height == 0;
                if (!_isMinimized)
                {
                    _previousActivePanel = dockMgr.ActivePanel;
                    _currentLayout = dockMgr.GetDockLayout();
                }
            }
        }


        private void EndDocking(object sender, EndDockingEventArgs e)
        {
            //Add tiny delay for layout to place itself.
            ThreadPool.QueueUserWorkItem(o => {
                Thread.Sleep(400);
                if (!e.Canceled && ucAccounts1.Height > 0 && ucCashIn1.Height > 0 && ucCustomer1.Height > 0)
                {
                    _previousActivePanel = dockMgr.ActivePanel;
                    _currentLayout = dockMgr.GetDockLayout();
                }
            });
        }

        private void EndSizing(object sender, EndSizingEventArgs e)
        {
            if (!e.Canceled && ucAccounts1.Height > 0 && ucCashIn1.Height > 0 && ucCustomer1.Height > 0)
            {
                _previousActivePanel = dockMgr.ActivePanel;
                _currentLayout = dockMgr.GetDockLayout();
            }
        }

        //End Task #104614



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (TellerVars.Instance.IsAppOnline) //Task #90007
            {
                if (keyData == (Keys.Enter))
                {
                    //if (ucTransactions1.IsAnyControlFocused() || ucTransactions1.Focused) //Task #114936 //#122359
                    if (ucTransactions1.IsAnyControlFocused()) //Task #114936 //#122359 - removed ucTransactions1.Focused
                    {
                        return false;
                    }
                    if (Customers == null || Customers.Rows == null || Customers.Rows.Count == 0)
                        ucCustomer1.SearchCustomer();
                    else
                    {
                        if (ActionSave != null && ActionSave.Visible == true && ActionSave.Enabled == true && !ActionSave.ImageButton.Focused)
                            ActionSave.ImageButton.Focus();

                        PerformQuickTranActionClick("Post", true, null);    //#124249
                    }
                    return true;
                }
                //Begin Task #89987
                else if (keyData == (Keys.Control | Keys.Enter) && ucCustomer1.SearchAgainVisible) //Bug #91420
                {
                    ucCustomer1.SearchAgain();
                    return true;
                }
                //End Task #89987
                else if (keyData == (Keys.Control | Keys.R))
                {
                    if (ucCustomer1.SearchAgainVisible)
                    {
                        ucCustomer1.SearchAgain();
                    }
                    else
                    {
                        ucCustomer1.FocusProperField();
                    }
                }
                //Begin Task# 115070
                else if (keyData == (Keys.Control | Keys.Down))
                {
                    if (ucCashIn1.ContainsFocus)
                        ucAccounts1.Focus();
                    else if (ucAccounts1.ContainsFocus)
                    {
                        ucTransactions1.Focus();
                        ucTransactions1.FocusFirstIncompleteRow(); //Task #115712
                    }
                    else if (ucTransactions1.ContainsFocus && (!ucTransactions1.IsFocusOnCashOut()))
                        ucTransactions1.FocusOnCashOut(); //122355-#2: was ucCashIn1.Focus();
                    //Begin 122355-#2
                    else if (ucTransactions1.ContainsFocus && ucTransactions1.IsFocusOnCashOut())
                        ucCashIn1.Focus();
                    //End 122355-#2

                    return true;
                }
                else if (keyData == (Keys.Control | Keys.Up))
                {
                    if (ucCashIn1.ContainsFocus)
                        ucTransactions1.FocusOnCashOut();  //122355-#2: was ucTransactions1.Focus();
                    else if (ucAccounts1.ContainsFocus)
                        ucCashIn1.Focus();
                    else if (ucTransactions1.ContainsFocus && (!ucTransactions1.IsFocusOnCashOut())) //122355-#2
                        ucAccounts1.Focus();
                    //Begin 122355-#2
                    else if (ucTransactions1.ContainsFocus && ucTransactions1.IsFocusOnCashOut())
                    {
                        ucAccounts1.Focus();
                        ucTransactions1.Focus();
                        ucTransactions1.FocusFirstIncompleteRow();
                    }
                    //End 122355-#2

                    return true;
                }
                else if (keyData == (Keys.Control | Keys.Right))
                {
                    if (ucCashIn1.ContainsFocus || ucAccounts1.ContainsFocus || ucTransactions1.ContainsFocus)
                        if (ActionSave != null && ActionSave.Visible == true && ActionSave.Enabled == true && !ActionSave.ImageButton.Focused)
                            ActionSave.ImageButton.Focus();
                }
                else if (keyData == (Keys.Control | Keys.Left))
                {
                    ucCashIn1.Focus();
                }
                //End Task Task# 115070
                else if (keyData == (Keys.Control | Keys.I))
                {
                    ucCashIn1.FocusPanel();
                }
                else if (keyData == (Keys.Control | Keys.O))
                {
                    ucAccounts1.Focus();
                }
                else if (keyData == (Keys.Control | Keys.N))
                {
                    ucTransactions1.FocusPanel();
                }
                else if (keyData == (Keys.Control | Keys.T))
                {
                    //#89998
                    if (GetCurrentRim() > 0)
                        QuickTranDeviceDeposit();
                }
                else if (keyData == (Keys.Control | Keys.D))
                {
                    //#89998
                    if (GetCurrentRim() > 0)
                        ucCashIn1.ToggleTCDIcon();
                }
                else if (keyData == (Keys.F9))
                {
                    //#89998
                    if (GetCurrentRim() > 0)
                        PerformQuickTranActionClick("ItemCapture", false, null);    //#124249
                }
                //Begin 121713-#2
                else if (keyData == (Keys.Control | Keys.U))
                {
                    ucTransactions1.FocusOnCashOut();
                }
                //End 121713-#2
                else if (keyData == (Keys.Alt | Keys.G))    //#122727
                {
                    _lastControlIsGetAcct = true;
                }
            }
            else //Begin Task#90007 Offline Mode
            {
                if (keyData == (Keys.Enter))
                {
                    if (ActionSave != null && ActionSave.Visible == true && ActionSave.Enabled == true && !ActionSave.ImageButton.Focused)
                        ActionSave.ImageButton.Focus();
                    PerformQuickTranActionClick("Post", true, null);    //#124249
                    return true; //Bug #103512
                }
                else if (keyData == (Keys.Control | Keys.I))
                {
                    ucCashIn1.FocusPanel();
                }
                else if (keyData == (Keys.Control | Keys.N))
                {
                    ucTransactions1.FocusPanel();
                }
                else if (keyData == (Keys.Control | Keys.T))
                {
                    //#89998
                    QuickTranDeviceDeposit();
                }
                else if (keyData == (Keys.Control | Keys.D))
                {
                    //#89998
                    ucCashIn1.ToggleTCDIcon();
                }
                else if (keyData == (Keys.F9))
                {
                    //#89998
                    PerformQuickTranActionClick("ItemCapture", false, null);    //#124249
                }
                //Begin 121713-#2
                else if (keyData == (Keys.Control | Keys.U))
                {
                    ucTransactions1.FocusOnCashOut();
                }
                //End 121713-#2

                //End Task#90007 Offline Mode
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Begin Task #91954

        private void RestorePreferredLayoutIfExists()
        {
            dockMgr.EndSizing -= EndSizing; //Task #104614 
            dockMgr.EndDocking -= EndDocking; //Task #104614
            _hasCustomLayout = false;

            if (!TellerVars.Instance.IsAppOnline)//Task# 90007
            {
                return;
            }

            // Get UI Pref from cache (if exists)
            UIPreferences UiPrefs = new UIPreferences();

            //Load from DB
            _rsmUiPref = (AdGbRsmUiPref)UiPrefs.GetApplicationPreferences(this.ScreenId, Convert.ToInt16(GlobalVars.EmployeeId), GlobalVars.Module);
            //If there are not any application prefs set need to set the property.
            _isNewPref = _rsmUiPref.Ptid.IsNull;
            if (!_isNewPref)
            {
                try
                {
                    string layout = this.dockMgr.GetDockLayout();
                    if (!string.IsNullOrEmpty(_rsmUiPref.XmlPreferences.Value))
                    {
                        this.dockMgr.LoadDockLayout(_rsmUiPref.XmlPreferences.Value); //Bug #115691
                    }
                    else if (!string.IsNullOrEmpty(_defaultLayout) && !_defaultLayout.Equals(layout)) //Bug #115691
                    {
                        RestoreDefaultLayout();
                    }

                    layout = this.dockMgr.GetDockLayout(); //Bug #115691
                    _hasCustomLayout = !string.IsNullOrEmpty(_defaultLayout) && !_defaultLayout.Equals(layout); //Bug #115691
                }
                catch (Exception e)
                {
                    RestoreDefaultLayout();
                    _hasCustomLayout = false;
                }
            }
            dockMgr.EndSizing += EndSizing; //Task #104614 
            dockMgr.EndDocking += EndDocking; //Task #104614
        }

        public void SaveCurrentLayout()
        {
            SaveLayout(this.dockMgr.GetDockLayout());
        }

        private void SaveLayout(string layoutValue)
        {
            //Save to Db
            if (_rsmUiPref == null)
                _rsmUiPref = new AdGbRsmUiPref();
            _rsmUiPref.ScreenId.Value = this.ScreenId;
            _rsmUiPref.EmplID.Value = Convert.ToInt16(GlobalVars.EmployeeId);
            _rsmUiPref.XmlPreferences.Value = layoutValue;

            //Saving the preferences to the database.
            if (_isNewPref)
            {
                //Setting the required application preferences.
                CoreService.DataService.ProcessRequest(XmActionType.New, _rsmUiPref);
            }
            else
            {
                CoreService.DataService.ProcessRequest(XmActionType.Update, _rsmUiPref);
            }

            //Do not show Zero Records Updated error as the UI pref may or may not have changed by user and we do not care.
            if (_rsmUiPref.Messages.GetMessageById(300080) != null)
            {
                _rsmUiPref.Messages.Clear();
            }

            if (HandleBusObjMessages(_rsmUiPref))
            {
                // Get UI Pref from cache (if exists)
                UIPreferences UiPrefs = new UIPreferences();
                //Updating the cache.
                UiPrefs.UpdateCache(_rsmUiPref);
                _isNewPref = false;
            }
            string layout = this.dockMgr.GetDockLayout();
            _currentLayout = layout; //Task #104614
            _hasCustomLayout = !_defaultLayout.Equals(layout);
        }

        public void RestoreDefaultLayout()
        {
            dockMgr.EndSizing -= EndSizing; //Task #104614 
            dockMgr.EndDocking -= EndDocking; //Task #104614
            _hasCustomLayout = false;
            _isCustomerPanelResized = false;
            SaveLayout(string.Empty);
            this.dockMgr.LoadDockLayout(_defaultLayout);
            OnResize(null, null);
            ucCustomer1.FocusProperField(); //Bug #114526
            dockMgr.EndSizing += EndSizing; //Task #104614 
            dockMgr.EndDocking += EndDocking; //Task #104614
        }
        //End Task #91954

        #endregion

        #region  ITellerWindow Implementation
        public DataTable Customers { get; set; }
        public DataTable Accounts { get; set; }
        public DataTable CustomerRelations { get; set; }
        public DataTable AcctRelations { get; set; }
        public Control LastActiveControl { get; set; }

        public PAction ActionSave { get; set; }

        public event CustomerInfoChanged OnCustomerChanged;
        public event Action<bool> OnSearchPerformed = delegate { }; //Bug #92023

        public EasyCaptureTranSetViewModel TransactionSet { get { return _transactionSet; } }

        public string[] TfrAcctList { get; set; } //87757

        //Begin Bug #102824
        public async Task<bool> SearchCustomer(int RimNo)
        {
            var resp = await PopulateCustomerInfo(RimNo);
            return resp.Success;
        }
        //End Bug #102824

        public async Task<CustomerInfoResponse> PopulateCustomerInfo(int RimNo)
        {

            ucCashIn1.ResetChecksAndNoItemsInfo(); //Bug #108545 Clear Checks and No of Items everytime a search is initiated.
            ucTransactions1.rimNo = Convert.ToString(RimNo);  //87753
            ucAccounts1.rimNo = Convert.ToString(RimNo);  //87753

            var resp = new CustomerInfoResponse()
            {
                RowCount = 0, //Bug #92488
                RimNo = RimNo, //Bug #92489
                AccountNumbers = new List<string>(),
                Success = true
            };

            if (ucCustomer1.CurrentRimNo > 0)
            {
                if (RimNo != ucCustomer1.CurrentRimNo)
                {
                    ClearCashAndTrans();

                    //Clear Account Info #87755
                    ucAccounts1.ClearData();
                    if (Accounts != null)
                        Accounts.Clear();
                    _currentAccountRow = null;  //#124210
                }
                else
                {
                    //Begin Bug #92488
                    ucCustomer1.ShowSummaryPanel();
                    ucAccounts1.ShowGrid();
                    return resp;
                    //End Bug #92488
                }
            }

            var customerAccountsResponse = await ucAccounts1.PopulateCustomerAccounts(RimNo); // Bug 89895
            resp.Success = customerAccountsResponse.Success;
            resp.AccountNumbers = customerAccountsResponse.AccountNumbers; //Bug #92489

            //Begin Task#87721: Uncommented code
            resp.Success = await ucCustomer1.PopulateCustomerInfo(RimNo);
            if (!resp.Success)
            {
                ClearCashAndTrans();    //#87749
                _tlTranCodeWindow.EnableDisableQuickButtons(false); //Task#94607/94608
                return resp;
            }
            else if (!_hasCustomLayout)
            {
                ResizeAccountAndTransactionPanel(); //Task #87719
            }

            PopulateCustomerRelationship(RimNo);

            ucTransactions1.rimStatus = ucCustomer1.rimStatus;  //87753
            ucTransactions1.OnSearchPerformed(resp.Success); //Bug# 108552
            //OnCustomerChanged?.Invoke();
            if (OnCustomerChanged != null)  //#87731 - Build Fix
                OnCustomerChanged();
            //End Task#87721

            //Begin #87725
            if (FocusOnAcctControl)
            {
                ucAccounts1.Focus();
                FocusOnAcctControl = false;
            }
            //End #87725


            //Begin Task#94607/94608
            //If Customer does not have accounts
            if (!customerAccountsResponse.Success)
            {
                SetTransactionAccountInfo("RIM", Convert.ToString(RimNo)); //Bug #109821
            }
            //End Task#94607/94608


            //Bug108450 Move this line to run after GetDisplayHistoryInfo
            _tlTranCodeWindow.PopulateRMSWithQuickTranInfo(RimNo);   //89899

            _tlTranCodeWindow.CheckFraud(); //Task#97603 

            setTitleBarName(RimNo, true); //Bug#103638
            _tlTranCodeWindow.EnableDisableQuickButtons(true);

            //Cash in focus
            if (resp.Success)
            {
                ucCashIn1.FocusCashInField();
            }

            return resp; //TBD - Task#87717
        }


        //Begin Task#Task 87723
        public async Task<CustomerInfoResponse> PopulateCustomerInfo(string SearchAcctNo)
        {
            int foundRimNo = 0;
            //Begin Task#87763
            string foundAcctNo = "";
            string foundAcctType = "";
            string options = "";
            string[] responseList = null;
            //End Task#87763

            var t = await ucCustomer1.GetCustomerByAccount(SearchAcctNo);
            var custResponse = new CustomerInfoResponse()
            {
                RowCount = t.Rows != null ? t.Rows.Count : 0, //Bug #92488
                AccountNumbers = new List<string>() { SearchAcctNo }, //Bug #92489
                UserCanceled = false //Bug #96236/96237/96238
            };

            //Begin Task#89981
            _tlTranCodeWindow.CrossRefSearchAcct = null;
            if (t.Columns.Contains("CrossRef"))
                _tlTranCodeWindow.CrossRefSearchAcct = SearchAcctNo;
            //End Task#89981

            //If account search returns only ONE row
            if (t.Rows.Count == 1 || (t.Rows.Count > 1 && t.DefaultView.ToTable(true, "RimNo").Rows.Count == 1)) //Bug#92696
            {
                GetAcctSearchResults(t, null, ref foundRimNo, ref foundAcctType, ref foundAcctNo);
            }
            //Begin 89979
            //If account search returns only MULTIPLE rows
            else if (t.Rows.Count > 1)
            {
                //options = SearchAcctNo + "~";

                if (MsgBoxShowDialog(this.ScreenId, "AcctSearch", t, ref responseList) == System.Windows.Forms.DialogResult.OK)
                {
                    GetAcctSearchResults(null, responseList, ref foundRimNo, ref foundAcctType, ref foundAcctNo);
                    custResponse.RimNo = foundRimNo;
                }
                else
                {
                    custResponse.UserCanceled = true; //Bug #96236/96237/96238
                    return custResponse;
                }
            }
            //End 89979
            //If account search returns only ZERO rows
            else
            {
                custResponse.Success = false;
                ClearCashAndTrans();    //#87749
                return custResponse;
            }


            // Begin 89883
            ucAccounts1.RmsAccount = TranHelper.Instance.CombineAcctNo(foundAcctNo, foundAcctType);
            FocusOnAcctControl = true;
            // End 89883
            ucAccounts1.SetGridFocusedRow(ucAccounts1.RmsAccount); //Bug #92488


            //Begin Task#87763
            options = null;
            responseList = null;

            options += "AcctType~" + foundAcctType + "~";
            options += "AcctNo~" + foundAcctNo + "~";

            if (_tlTranCodeWindow.GetSelectSecondOwner() == "Y") //Bug#97688
            {
                DialogResult result = MsgBoxShowDialog(this.ScreenId, "SelectSecondaryOwner", options, ref responseList);

                if (result == DialogResult.OK)
                {
                    foundRimNo = custResponse.RimNo = Convert.ToInt32(responseList[1]); //Bug #92489
                }
                else if (result == DialogResult.Cancel)
                {
                    custResponse.UserCanceled = true; //Bug #96236/96237/96238
                    //Begin Task# 115140
                    //Bug If relationship is not found, continue with foundRimNo
                    //else if (result == DialogResult.Abort)
                    //    custResponse.Success = false;
                    //End Task# 115140
                    return custResponse;
                }
                //End Task#87763
            }

            if (ucCustomer1.CurrentRimNo > 0 && foundRimNo != ucCustomer1.CurrentRimNo) //Bug#97017/97018
                ClearCashAndTrans();

            var resp = await PopulateCustomerInfo(foundRimNo);
            //Cash in focus
            if (resp.Success)
            {
                ucCashIn1.FocusCashInField();
            }
            custResponse.Success = true;
            return custResponse;
        }
        //End Task#Task 87723

        //Begin Task #87719
        private void ResizeAccountAndTransactionPanel()
        {
            int rowHeight = ucAccounts1.GridRowHeight + 3; //Task #115070
            int minAccountsToDisplay = 3;
            int maxAccountsToDisplay = 8;
            int rowCount = Accounts.Rows.Count + 1; //Task #115070
            int nbAccountsToDisplay = rowCount > maxAccountsToDisplay ? maxAccountsToDisplay : rowCount;
            if (nbAccountsToDisplay < minAccountsToDisplay)
            {
                nbAccountsToDisplay = minAccountsToDisplay;
            }
            int newAcctSize = rowHeight * nbAccountsToDisplay;
            if (ucAccounts1.Height != newAcctSize)
            {
                bool prevAcctIsResizable = dockPanelAccounts.IsResizable;
                int heightDifference = dockPanelAccounts.Size.Height - dockPanel2_Container.Size.Height;

                dockPanelAccounts.IsResizable = true;
                ucAccounts1.Size = new Size(ucAccounts1.Size.Width, newAcctSize);
                dockPanelAccounts.Size = new Size(dockPanelAccounts.Size.Width, newAcctSize + heightDifference);

                ucAccounts1.Refresh();
                dockPanelAccounts.Refresh();
                dockPanelAccounts.IsResizable = prevAcctIsResizable;
            }
        }
        //End Task #87719
        void GetAcctSearchResults(DataTable t, string[] responseList, ref int foundRimNo, ref string foundAcctType, ref string foundAcctNo)
        {
            if (t != null)
            {
                DataRow row = t.Rows[0];
                foundRimNo = Convert.ToInt32(row["RimNo"]); //Bug #92489
                //Begin Task#87763
                foundAcctNo = Convert.ToString(row["AcctNo"]);
                foundAcctType = Convert.ToString(row["AcctType"]);
                //End Task#87763
            }
            else if (responseList != null)
            {
                foundRimNo = Convert.ToInt32(responseList[1]); //Bug #92489

                //Begin Task# 115140
                string account = Convert.ToString(responseList[2]).Trim();

                string accountSeparator = " - "; //Combined account is separated by space dash space. 
                int accountSeparatorStartPos = account.IndexOf(accountSeparator);
                int accountSeparatorEndPos = account.IndexOf(accountSeparator) + accountSeparator.Length;

                foundAcctType = account.Substring(0, accountSeparatorStartPos);
                foundAcctNo = account.Substring(accountSeparatorEndPos);
                //Begin Task# 115140

                //Begin Task#87763
                //string[] account = Convert.ToString(responseList[2]).Split("-".ToCharArray(), StringSplitOptions.None);
                //foundAcctType = Convert.ToString(account[0]).Trim();
                //foundAcctNo = Convert.ToString(account[1]).Trim();
                //End Task#87763
            }

            //Begin Task#89981
            if (_tlTranCodeWindow.CrossRefSearchAcct != null)
            {
                _tlTranCodeWindow.CrossRefSearchMapAcctNo = foundAcctNo;
                _tlTranCodeWindow.CrossRefSearchMapAcctType = foundAcctType;
            }
            //End Task#89981
        }

        DialogResult MsgBoxShowDialog(int screenId, string configSource, DataTable dataTable, ref string[] responseList)
        {
            string options = "";
            foreach (DataRow row in dataTable.Rows)
            {
                options += "^" + Convert.ToString(row.GetIntValue("RimNo")).Trim() + "~" + row.GetStringValue("Account").Trim() + "~" + row.GetStringValue("Name").Trim();
            }

            options = options.TrimStart('^');

            return MsgBoxShowDialog(screenId, configSource, options, ref responseList);
        }

        //Begin Task#87763
        DialogResult MsgBoxShowDialog(int screenId, string configSource, string options, ref string[] responseList)
        {
            string response;
            DialogResult result = new System.Windows.Forms.DialogResult();
            using (Phoenix.Client.MsgBox.frmMsgBoxOption msgBox = new Phoenix.Client.MsgBox.frmMsgBoxOption()) //Bug #93040
            {
                msgBox.InitParameters(this.ScreenId, configSource, options);
                msgBox.ShowDialog();
                result = msgBox.DialogResult;

                response = msgBox.DialogOptions;
                responseList = response.Split("~".ToCharArray(), StringSplitOptions.None);
                msgBox.Dispose();
                return result;
            }
        }
        //End Task#87763


        //Begin Task 89995
        public DataTable GetDetailCustomerByAccountSync(string AcctNo, string acctType = null)
        {
            return ucCustomer1.GetCustomerByAccountSync(AcctNo, acctType);
        }
        //End Task 89995
        public void PopulateAccountRelationship(string acctType, string acctNo)
        {
            // TODO
        }

        public async void PopulateCustomerRelationship(int rimNo)
        {
            bool success = await ucCustomer1.PopulateCustomerRelations(rimNo);  //Task#87721
            return; //TBD - Task#87717
        }

        public void SetTransactionMenuIndicator()
        {
            ucTransactions1.SetMenuIndicatorColumn(); //Bug #99053
        }

        public void MoveAcctToTran(object selectedMenuItem)
        {
            DevExpress.Utils.Menu.DXMenuItem selectMenuItem = (DevExpress.Utils.Menu.DXMenuItem)selectedMenuItem;
        }


        public AcctAlerts GetAlertInfo(string acctType, string acctNo)
        {
            return ucAccounts1.GetAlertInfo(acctType, acctNo); //Task 89995
        }

        #endregion

        #region IQuickTranWindow Implementation

        public EasyCaptureTranSet TranSet { get { return _transactionSet; } }

        public IPhoenixWorkspace CurWkspace { get { return _curWkspace; } }

        public bool IsQuickTranRealTime { get { return _isQuickTranRealTime; } }

        public ITlTranCodeWindow TlTranCodeWindow { get { return _tlTranCodeWindow; } }

        public bool AllowHardHold { get { return _allowHardHold; } }

        public int CurTranId { get { return _curTranId; } set { _curTranId = value; } }

        public bool AssumeDecimals { get { return _assumeDecimals; } set { _assumeDecimals = value; } }

        public decimal DeviceDeposit { get { return _deviceDeposit; } set { _deviceDeposit = value; } }

        //Begin Task#100769
        public bool ConfAcctSecurity { get { return _confAcctSecurity; } }
        public bool UserConfidentialAccess { get { return _userConfidentialAccess; } }
        //End Task#100769

        public void SetTlTranCodeWindow(ITlTranCodeWindow tlTranCodeWindow)
        {
            _tlTranCodeWindow = tlTranCodeWindow;
            //this.Workspace = (tlTranCodeWindow as PfwStandard).Workspace;
            _curWkspace = (tlTranCodeWindow as PfwStandard).Workspace;
            TranSet.Transactions.CollectionChanged += Transactions_CollectionChanged;
            TranSet.PropertyChanged += TranSet_PropertyChanged;
            TellerVars.Instance.SetContextObject("AdTlTcArray", "Q01");
            _isQuickTranRealTime = (TellerVars.Instance.AdTlTc.RealTimeEnable.Value == "Y");
            _tlTranCodeWindow.EnableDisableQuickButtons(false); //Task#89976

            //Begin 87757
            _tlTranCodeWindow.GetTfrAcctTypes(ref _tfrAcctTypes);
            if (_tfrAcctTypes.Count > 0)
            {
                SetTfrAcctTypes(_tfrAcctTypes);
            }
            //End 87757

            //Get valid account types.
            _tlTranCodeWindow.GetTfrAcctTypes(ref _acctTypeDepLoanList); //Task#102715

            _allowHardHold = (CoreService.UIAccessProvider.GetScreenAccess(Phoenix.Shared.Constants.ScreenId.GbStopHoldEdit) & AuthorizationType.Read) == AuthorizationType.Read &&
                TellerVars.Instance.IsAppOnline;  //Selva Code review
            //HandleTCDDrawerIcon();  //#89998
            ucCustomer1.FocusProperField(); //Bug #114526
        }

        public void TlTranCodeWindowCloseNotify()
        {
            //TODO: Add any cleanup  code since the Transaction window is closing
        }

        public void ChildWindowClosedNotify(Form form)
        {
            if (LastActiveControl != null)  //#122727
            {
                LastActiveControl.Focus();
                if (LastActiveControl.Name == "gcTransactions" && _lastControlIsGetAcct)
                {
                    ucTransactions1.CloseEditorSetFocus();
                    _lastControlIsGetAcct = false;
                }
            }
        }

        //Begin 89907
        public void ChildControlFocused(object sender)
        {
            Control control = sender as Control;

            LastActiveControl = control; //87735

            string GridColumn = string.Empty;
            string GridColFldName = string.Empty; //87757
            bool ReadOnlyTfrfld = false;             //122355-#5

            if (control.Name == "gcTransactions")
            {
                DevExpress.XtraGrid.Views.Grid.GridView gridView;
                gridView = ((DevExpress.XtraGrid.Views.Base.ColumnView)((DevExpress.XtraGrid.GridControl)control).FocusedView) as DevExpress.XtraGrid.Views.Grid.GridView;

                if (gridView.FocusedColumn != null)
                {
                    GridColumn = gridView.FocusedColumn.ToString();
                    GridColFldName = gridView.FocusedColumn.FieldName;  //87757

                    //Begin 122355-#5
                    if ((GridColFldName == "TfrAcctType" && gridView.FocusedColumn.ColumnEdit.ReadOnly) || (GridColFldName == "TfrAcctNo" && gridView.FocusedColumn.ReadOnly))
                    {
                        ReadOnlyTfrfld = true;
                    }
                    //End 122355-#5
                }
            }

            if ((control.Name == "dfCashIn" || control.Name == "dfCashOut" || GridColumn == "Amount"))
                _tlTranCodeWindow.SetCalcControl(sender);
            else
                _tlTranCodeWindow.SetCalcControl(null);


            //Begin 87757

            string reqResultStr = string.Empty;

            if ((GridColFldName == "TfrAcctType" || GridColFldName == "TfrAcctNo") && ReadOnlyTfrfld == false) //122355-#5
                _tlTranCodeWindow.ActionRequest("EnableDisableGetAcct", "true", ref reqResultStr);
            else
                _tlTranCodeWindow.ActionRequest("EnableDisableGetAcct", "false", ref reqResultStr);

            //End 87757
        }

        public void SetCalcValue2GridColumn(Control control, decimal CalculatorValue)
        {
            DevExpress.XtraGrid.Views.Grid.GridView gridView;
            gridView = ((DevExpress.XtraGrid.Views.Base.ColumnView)((DevExpress.XtraGrid.GridControl)control).FocusedView) as DevExpress.XtraGrid.Views.Grid.GridView;

            int FocusedRow = gridView.FocusedRowHandle;
            gridView.SetRowCellValue(FocusedRow, "Amount", CalculatorValue);
        }

        //End 89907

        //public EasyCaptureTran GetSelectedTran()
        //{
        //    // return ucTransactions1.GetSelectedTran();
        //    return new EasyCaptureTran(); //TBD - Task#87717
        //}

        public EasyCaptureTran GetCurrentEasyCaptureTran()
        {
            if (_curEasyCaptureTran == null)
            {
                _curEasyCaptureTran = new EasyCaptureTran();
                _curEasyCaptureTran.MappedTranIndex = 1;    //hack
            }
            return _curEasyCaptureTran;
        }

        public EasyCaptureTran GetCurrentEasyCaptureTran(int tranId)
        {
            if (tranId > 0)
                return TranSet.Find(tranId);
            else
                return _curEasyCaptureTran;
        }

        public void ResetCurrentEasyCaptureTran()
        {
            if (_curEasyCaptureTran != null)
            {
                _curEasyCaptureTran = null;
                _curTcDescription = string.Empty;
                _curTranCode = string.Empty;
            }
        }

        public void SetCurrentTran(string description, string trancode)
        {
            _curTcDescription = description;
            _curTranCode = trancode;
            _curEasyCaptureTran = TranSet.Find(description, trancode);
            if (_curEasyCaptureTran == null)
            {
                _curEasyCaptureTran = new EasyCaptureTran();
                _curEasyCaptureTran.TcDescription = description;
                _curEasyCaptureTran.TranDef.TranCode = trancode;
                _curEasyCaptureTran.TranDef.Description = description;
            }
            else
            {
                if (_curEasyCaptureTran.TranId <= 0 || (_curEasyCaptureTran.TranId < TranSet.Transactions.Count && _nextTranId != TranSet.Transactions.Count))
                {
                    _nextTranId += 1;
                    TranSet.UpdateTranId(description, trancode, _nextTranId);
                    _curTranId = _nextTranId;
                }
            }
        }

        public void PerformQuickTranActionClick(string actionName, bool fromEnterKey, params object[] paramList)    //#124249
        {
            try
            {
                if (actionName == "Post" && fromEnterKey)   //#124249
                {
                    if (ucTransactions1.ForceDeleteEmptyRow())
                        _tlTranCodeWindow.PerformQuickTranActionClick(actionName, paramList);
                }
                else
                    _tlTranCodeWindow.PerformQuickTranActionClick(actionName, paramList);
            }
            finally
            {

            }
        }

        public string GetTransactionGridFocusedColumn()
        {
            return ucTransactions1.GetGridFocusedColumn();
        }

        public void QuickTranDeviceDeposit()
        {

            try
            {
                if (TellerVars.Instance.IsTCDEnabled && TellerVars.Instance.IsTCDConnected && TellerVars.Instance.IsRecycler)
                    _tlTranCodeWindow.QuickTranDeviceDeposit();
                MapEasyCaptureToTranSetAsync(true, "CashIn");
            }
            finally
            {
                if (DeviceDeposit != TransactionSet.CashIn)
                    PerformQuickTranActionClick("CashIn", false, null); //#124249
            }
        }

        public void QuickTranReverseDeviceDeposit()
        {
            if (TellerVars.Instance.IsTCDEnabled && TellerVars.Instance.IsTCDConnected && TellerVars.Instance.IsRecycler)
                _tlTranCodeWindow.QuickTranReverseDeviceDeposit();
            MapEasyCaptureToTranSetAsync(true, "CashIn");
        }

        public void QuickTranTcdDrawerIconToggle(PictureBox picTempTCD, PictureBox picTempDrawer) //#89998
        {
            if (TellerVars.Instance.IsTCDEnabled)
                _tlTranCodeWindow.QuickTranTcdDrawerIconToggle(picTempTCD, picTempDrawer); //#89998
            ucTransactions1.HandleDenominate();
        }

        public void PerformEnableDisableControls(string origin)
        {
            _tlTranCodeWindow.PerformEnableDisableControls(origin);
        }

        public void RefreshQuickTran(string source)  //#102719
        {
            if (source == "ChecksIn")
                ucCashIn1.UpdateCheckAmount();
        }

        public void InitializeTranSet()
        {
            _tlTranCodeWindow.QuickAccountSelectedRow = null;
            _tlTranCodeWindow.isTransactionGridPopulated = false; //Bug# 109838
            //Disable display and History Buttons.
            _tlTranCodeWindow.EnableDisableQuickButtons(false); //Task #88995

            //Clear Customer Info
            if (Customers != null)
                Customers.Clear();
            ucCustomer1.ResetCustomerInfo();

            //Clear Account Info
            ucAccounts1.ClearData();
            if (Accounts != null)
                Accounts.Clear();
            _currentAccountRow = null;  //#124210
            ClearCashAndTrans();

            //#89998
            //HandleTCDDrawerIcon();
            //Reset Customer/Account Search Focus
            ucCustomer1.ResetCustomerSearchFocus();
            ucCustomer1.Focus(); //87735
            return;
        }

        public void HandleTCDDrawerIcon()   //#89998
        {
            ucCashIn1.EnableDisableControls("HandleTCDIcon");
            ucTransactions1.HandleDenominate();
        }

        public void HandleWaiveSignature()   //#90021
        {
            ucCashIn1.EnableDisableControls("HandleWaiveSig");
        }

        public void ClearCashAndTrans()
        {
            //Clear Tran Info
            if (TranSet.Transactions != null)
                TranSet.Transactions.Clear();

            //Clear CashIn Info
            if (TranSet.CashIns != null)
                TranSet.CashIns.Clear();
            TranSet.CashIn = 0;
            TranSet.ChecksIn = 0; //Bug #91948
            TranSet.NoItems = 0; //Bug #91948 
            TranSet.ChecksMakeAvail = 0; //Bug #91948
            ucCashIn1.ResetCashInInfo();
            TranSet.CashInTran = TranSet.CashInTran;

            //Clear Items/Checks Info
            if (TranSet.Checks != null)
                TranSet.Checks.Clear();

            //Clear CashOut Info
            if (TranSet.CashOuts != null)
                TranSet.CashOuts.Clear();

            TranSet.CashOut = 0;
            TranSet.CashOutTran = TranSet.CashOutTran;
            ucTransactions1.ResetCashOutInfo();

            //Clear Error Info
            ucTransactions1.ClearErrorInfo();
            ucTransactions1.ClearTranMenuCachedInfo(); //Task #115712

            ucTransactions1.HideAlerts();//Task 89995


            //Begin Bug#108552
            //ucTransactions1.SetHeaderButtonsState();
            ucTransactions1.OnSearchPerformed(false); //Bug#108552
            setTitleBar("");
            //End Bug#108552

            //Begin Task#89981
            _tlTranCodeWindow.CrossRefSearchAcct = null;
            _tlTranCodeWindow.CrossRefSearchMapAcctNo = null;
            _tlTranCodeWindow.CrossRefSearchMapAcctType = null;
            //End Task#89981
            _curTranId = 0;
            _nextTranId = 0;
            return;
        }

        public object GetCurrentHardHoldBalInfo()   //#87767
        {
            return _tlTranCodeWindow.GetCurrentHardHoldBalInfo();
        }

        public void SetCurrentHoldBalInfo(object hardHoldObj)   //#87767
        {
            _tlTranCodeWindow.SetCurrentHoldBalInfo(hardHoldObj);
        }

        public void HandleQuickTranStatusText(string statusText)   //#90021
        {
            _tlTranCodeWindow.HandleQuickTranStatusText(statusText);
        }

        #endregion

        public void ClearInformation()
        {
            ucAccounts1.ClearData();
            _tlTranCodeWindow.QuickAccountSelectedRow = null;
            _currentAccountRow = null;  //#124210
        }


        private void TranSet_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CashIn" || e.PropertyName == "ChecksIn" || e.PropertyName == "CashOut" || e.PropertyName == "Denominate" || e.PropertyName == "WaiveSignature")    //#89998  #90021
                MapEasyCaptureToTranSetAsync(true, e.PropertyName);

            if (e.PropertyName == "ChecksIn")
            {
                ucCashIn1.UpdateCheckAmount();
            }
        }

        private void Transactions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var tran in TranSet.Transactions)
            {
                tran.PropertyChanged -= Tran_PropertyChanged;
                tran.PropertyChanged += Tran_PropertyChanged;
            }

            MapEasyCaptureToTranSetAsync(true, "CollectionChanged");
        }

        public async void MapEasyCaptureToTranSetAsync(bool populateViewTran, string triggerSource, Action callback = null)
        {
            if (Thread.CurrentThread.ManagedThreadId != _appThreadId)   // to avoid nested calls due to setting of properties
                return;

            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        CoreService.LogPublisher.LogDebug(string.Format("Easy Capture:MapEasyCaptureToTranSetAsync START for Thread {0} triggerSource {1}",
                            Thread.CurrentThread.ManagedThreadId, triggerSource));

                        _isProcessingMap = true;

                        //ucTransactions1.ClearErrorInfo();
                        this.BeginInvoke((Action)(() =>
                        {
                            _tlTranCodeWindow.MapEasyCaptureToTranSet(populateViewTran, true, false, false);
                            if (callback != null)
                            {
                                callback();
                            }
                        }));

                        ucTransactions1.RefreshGrid();
                        CoreService.LogPublisher.LogDebug(string.Format("Easy Capture:MapEasyCaptureToTranSetAsync FINISH for Thread {0} triggerSource {1}",
                            Thread.CurrentThread.ManagedThreadId, triggerSource));
                    }
                    finally
                    {
                        _isProcessingMap = false;
                        //_semaphoreSlim.Release();
                        ucTransactions1.RefreshGrid();  //102719
                    }
                });
            }
            finally
            {

            }
        }


        private void Tran_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount" || e.PropertyName == "CcAmt" || e.PropertyName == "TcDescription" || e.PropertyName == "TfrAmount" ||
                e.PropertyName == "Options" || e.PropertyName == "TfrAcctNo" || e.PropertyName == "TfrAcctType" || e.PropertyName == "AcctNo" ||
                e.PropertyName == "TranDef" || e.PropertyName == "IncomingAccount" || e.PropertyName == "HoldBal" || e.PropertyName == "SearchRimNo")//87757 #92886 Task#89981 #87767 Bug#103638

            {
                if (e.PropertyName == "AcctNo" || e.PropertyName == "TcDescription" || e.PropertyName == "TranDef" || e.PropertyName == "SearchRimNo") //Bug#103638
                {
                    EasyCaptureTran tran = (sender as EasyCaptureTran);
                    if (tran != null)
                    {
                        if (e.PropertyName == "TranDef")
                        {
                            if (tran.TranDef.Description == EasyCaptureTranCodeDesc.PaymentAutoSplitPaytoZero.ToString() ||
                                tran.TranDef.Description == EasyCaptureTranCodeDesc.PrincipalPayment.ToString() ||
                                tran.TranDef.Description == EasyCaptureTranCodeDesc.PaymentAutoSplit.ToString()) //21713-#7:Bug-fix

                                tran.Amount = 0;
                        }
                        else
                        {
                            if (e.PropertyName == "TcDescription")
                            {
                                tran.AcctNo = string.Empty;
                                tran.AcctType = string.Empty;
                                tran.AcctCombined = string.Empty;
                            }

                            tran.Amount = 0;
                            tran.CcAmt = 0;
                            tran.CheckNo = string.Empty; //Task #113989
                            tran.Description = string.Empty;
                            tran.HoldBal = 0;
                            tran.Reference = string.Empty;
                            tran.TfrAcctNo = string.Empty;
                            tran.TfrAcctType = string.Empty;
                            tran.TfrAmount = 0;
                            tran.RegD = false; //121714-Bug-fix
                            if (TellerVars.Instance.IsAppOnline) //Bug# 109869
                                tran.Options = string.Empty;
                        }
                    }
                }

                MapEasyCaptureToTranSetAsync(true, e.PropertyName);
            }
            else if (e.PropertyName == "Description" || e.PropertyName == "Reference" ||
                e.PropertyName == "CheckNo" || e.PropertyName == "RegD" ||
                e.PropertyName == "PassDescriptionToHistory")

            {
                var currentTran = _curEasyCaptureTran;  //#87749
                if (TranSet.Transactions.IndexOf(currentTran) != (TranSet.Transactions.Count - 1)) // is not last tran
                {
                    MapEasyCaptureToTranSetAsync(true, e.PropertyName);
                }
            }

            //Begin 87757-2
            if (e.PropertyName == "Options" || e.PropertyName == "TfrAcctNo" || e.PropertyName == "TcDescription")
            {
                ucTransactions1.ResetTfrFlds();

                //Begin 87759
                if (e.PropertyName == "Options" || e.PropertyName == "TfrAcctNo") //87751
                {
                    ucTransactions1.ValidLoanfrAcctNo(); //87751
                }
                //End 87759
            }
            //End 87757-2

            //Begin 121713-#7
            /*
            if (e.PropertyName == "Amount")
            {
                ucTransactions1.FocusPanel();
            }
            */
            //End 121713-#7
        }


        //Begin #87725
        public void SetRMSInformation(int RimNo, string Account)
        {
            if (Account != null)
            {
                //Set focus on the selected account from the RSM window
                Account = Account.Replace("*", String.Empty);
                ucAccounts1.RmsAccount = Account;
                ucAccounts1.SetGridFocusedRow(ucAccounts1.RmsAccount); //Bug#93075
                FocusOnAcctControl = true;
            }

            if (RimNo != int.MinValue)
            {
                PopulateCustomerInfo(RimNo);
            }
            ucCustomer1.SetCustomerInformation(RimNo, Account); //Bug #91398
        }

        //End #87725

        public void SetTransactionAccountInfo(string accType = null, string acctNo = null)  //#124210
        {
            SetTransactionAccountInfo(accType, acctNo, false);
        }

        //Begin Task#94607/94608
        public void SetTransactionAccountInfo(string accType = null, string acctNo = null, bool fromTCDeleteRows = false)  //#124210
        {
            _tlTranCodeWindow.isTransactionGridPopulated = false; //Bug #109821

            //Begin Task# 90007 Offline
            if (!TellerVars.Instance.IsAppOnline)//Task# 90007
            {
                _tlTranCodeWindow.isTransactionGridPopulated = (!string.IsNullOrEmpty(accType) && !string.IsNullOrEmpty(acctNo));
                _tlTranCodeWindow.EnableDisableQuickButtons(false);
                return;
            }
            //End Task# 90007


            string account;
            DataRow row = null;

            //If customer does not have accounts, use rim no from searchn to allow user to Display/History.
            if (Accounts != null && Accounts.Rows.Count == 0)
            {
                accType = "RIM";
                acctNo = ucAccounts1.rimNo;
            }


            if (!string.IsNullOrEmpty(accType) && !string.IsNullOrEmpty(acctNo))
            {
                _tlTranCodeWindow.isTransactionGridPopulated = true; //Bug #109821

                if (accType != "RIM")
                {
                    //If Account row
                    account = TranHelper.Instance.CombineAcctNo(acctNo, accType);
                    //Locate account in account grid to get all neccesary columns in order to perform Enable/Disable logic.
                    row = ucAccounts1.LocateAccountRow(account);
                }
                else
                {
                    //If RIM row, create a row with the Rim information so Display/History buttons can work properly.
                    DataTable table = new DataTable();
                    row = table.NewRow();
                    table.Columns.Add("DepLoan");
                    table.Columns.Add("RimNo");
                    table.Columns.Add("AcctType");
                    row["DepLoan"] = GlobalVars.Instance.ML.RM;
                    row["RimNo"] = acctNo;
                    row["AcctType"] = "RIM";
                }

                SetQuickAccountSelectedRow(row);
            }
            else
            {
                //#124210
                if (!fromTCDeleteRows)
                    _tlTranCodeWindow.EnableDisableQuickButtons(false);
                else
                {
                    if (_currentAccountRow != null && _currentAccountRow.Table.Columns.Count > 0)   //#124210
                    {                        
                        SetQuickAccountSelectedRow(_currentAccountRow);
                        if (_currentAccountRow.Table.Columns.Contains("AcctNo"))    //#MT124210
                            ucAccounts1.SetGridFocusedRow(TranHelper.Instance.CombineAcctNo(Convert.ToString(_currentAccountRow["AcctNo"]), Convert.ToString(_currentAccountRow["AcctType"])));
                    }
                    ucAccounts1.Focus();
                }

                return;
            }
        }
        //End Task#94607/94608

        //Begin Task#89976
        public void SetQuickAccountSelectedRow(DataRow row)
        {
            _tlTranCodeWindow.QuickAccountSelectedRow = row;
            _currentAccountRow = row;   //#124210
            _tlTranCodeWindow.EnableDisableQuickButtons(true); //Bug#92189
        }
        //End Task#89976

        //Begin Task#89967
        public int GetRmAcctPtid(int rmAcctPtid)
        {
            return _tlTranCodeWindow.RmAcctPtid = rmAcctPtid;
        }
        //End Task#89967


        //Begin 87757
        public void SetTfrAcctTypes(ArrayList TfrAcctTypeList)
        {
            int elementsCnt = TfrAcctTypeList.Count;
            string[] TfrAcctTypes = new string[elementsCnt];
            int itemsCnt = -1;

            foreach (object accttypes in TfrAcctTypeList)
            {
                itemsCnt++;
                TfrAcctTypes[itemsCnt] = ((Phoenix.FrameWork.BusFrame.EnumValue)accttypes).CodeValue.ToString();
            }

            TfrAccountList = TfrAcctTypes;

            //ucTransactions1.SetTfrAcctTypes();  //87757-2 //87757-3
        }

        public string[] TfrAccountList
        {
            get { return TfrAcctList; }
            set { TfrAcctList = value; }
        }

        public void ActionRequest(string req, string value, ref string reqResultStr)
        {
            if (req == "FormatTfrAcctNo")
            {
                string[] TfrAcctTypes = TfrAccountList;

                if (TfrAcctTypes.Length > 0 && _tfrAcctTypes.Count > 0)
                {
                    int acctTypeIndex = Array.FindIndex(TfrAcctTypes, row => row == value);  //87761

                    if (acctTypeIndex >= 0)
                    {
                        string acctDesc = ((Phoenix.FrameWork.BusFrame.EnumValue)_tfrAcctTypes[acctTypeIndex]).Description + "~" + value; //87757-2: Added acct type

                        _tlTranCodeWindow.ActionRequest(req, acctDesc, ref reqResultStr);
                    }
                }
            }

            //Begin 87757-2
            if (req == "GetTC163RegDValue" || req == "GetTC164RegDValue" || req == "GetTC102RegDValue" || req == "GetEnableDescRefFlags" || req == "GetEnableCheckNoFlags") //104267 //87759  #124295
            {
                _tlTranCodeWindow.ActionRequest(req, value, ref reqResultStr);
            }
            //End 87757-2

            //Begin 87759
            if (req == "GetDepLoan")
            {
                string[] TfrAcctTypes = TfrAccountList;

                if (TfrAcctTypes.Length > 0 && _tfrAcctTypes.Count > 0)
                {
                    int acctTypeIndex = Array.FindIndex(TfrAcctTypes, row => row == value);  //87761

                    if (acctTypeIndex >= 0)
                    {
                        string acctDesc = string.Empty;
                        acctDesc = ((Phoenix.FrameWork.BusFrame.EnumValue)_tfrAcctTypes[acctTypeIndex]).Description;

                        if (acctDesc != string.Empty)
                        {
                            reqResultStr = acctDesc.Split("~".ToCharArray())[1]; //Deploan
                        }
                    }
                }
            }
            //End 87759

            /* Begin 95598 */
            if (req == "GetDescValue")
            {
                _tlTranCodeWindow.ActionRequest(req, value, ref reqResultStr);
            }
            /* End 95598 */

            //Begin Task#90007 Offline
            if (!TellerVars.Instance.IsAppOnline && req == "FormatAcctNo")
            {
                string acctTypes = "";
                ActionRequest("GetAcctTypeList", "ALL", ref acctTypes);
                string[] acctTypesList = acctTypes.Split('~');

                if (acctTypesList.Length > 0 && _acctTypeDepLoanList.Count > 0)
                {
                    int acctTypeIndex = Array.FindIndex(acctTypesList, row => row == value);

                    if (acctTypeIndex >= 0)
                    {
                        string acctDesc = ((Phoenix.FrameWork.BusFrame.EnumValue)_acctTypeDepLoanList[acctTypeIndex]).Description + "~" + value;

                        _tlTranCodeWindow.ActionRequest(req, acctDesc, ref reqResultStr);

                    }
                }
            }

            if (!TellerVars.Instance.IsAppOnline && req == "GetAcctTypeList")
            {
                string acctType, depLoan, acctDesc;

                if (value == "RM") //Task 102715
                {
                    reqResultStr = "RIM";
                }
                else
                {
                    foreach (object item in _acctTypeDepLoanList)
                    {
                        acctType = ((Phoenix.FrameWork.BusFrame.EnumValue)item).CodeValue.ToString();//AcctType
                        acctDesc = ((Phoenix.FrameWork.BusFrame.EnumValue)item).Description.ToString(); //Description
                        depLoan = acctDesc.Split("~".ToCharArray())[1]; //Deploan

                        if (value == "ALL")
                        {
                            reqResultStr += acctType + "~";
                        }
                        else if (value == depLoan)
                        {
                            reqResultStr += acctType + "~";
                        }
                    }

                    //if ALL, add RIM to ALL deploans 
                    if (value == "ALL") //Task 102715
                        reqResultStr = reqResultStr + "RIM";
                    else
                        reqResultStr = reqResultStr.Remove(reqResultStr.Length - 1);
                }
            }
            //End Task#90007
        }

        //Begin 87759
        public void GetTfrDepLoan(string acctType, ref string deploan)
        {
            ActionRequest("GetDepLoan", acctType, ref deploan);
        }
        //End 87759

        public void SetGetAcctValue(string acctType, string acctNo)
        {
            ucTransactions1.SetGetAcctValue(acctType, acctNo);
        }

        public void FocusProblematicField(EasyCaptureTranResponse tranResponse, int tranIndex)
        {
            int row = GetRowForTransactionIndex(tranIndex);
            ucTransactions1.FocusProblematicField(tranResponse, row);
        }

        public void DeleteEmptyTranRow()
        {
            ucTransactions1.ForceDeleteEmptyRow();
            return;
        }

        public int GetRowForTransactionIndex(int tranIndex)
        {
            int row = -1;
            int index = 0;
            foreach (var tran in _transactionSet.Transactions)
            {
                if (tranIndex == tran.MappedTranIndex)
                {
                    row = index;
                    break;
                }
                ++index;
            }

            return row;
        }

        //End 87757

        private void tempWin_Closed(object sender, EventArgs e)
        {
            try
            {
                Form form = sender as Form;
                if (form == null)
                {
                    return;
                }
                if (form.Name == "frmGbStopHoldEdit")
                    SetCurrentHoldBalInfo(_tempHardHoldInfo);
            }
            finally
            {

            }
        }



        public void CallOtherForms(string origin)
        {
            string acctNo = null;
            string acctType = null;
            string depLoan = null;
            int rimNo = -1;
            PfwStandard tempWin = null;
            _tempHardHoldInfo = null;
            var tran = (_curTranId > 0 ? GetCurrentEasyCaptureTran(_curTranId) : GetCurrentEasyCaptureTran());

            if (origin == "HardHold") // #87767
            {
                if (tran == null || tran.SelectedDepLoan != "DP" || !_allowHardHold)    //#100724
                    return;
            }

            if (origin == "HardHold") // #87767
            {
                acctType = tran.SelectedAcctType;
                acctNo = tran.SelectedAcctNo;
                depLoan = tran.SelectedDepLoan;
                rimNo = GetCurrentRim();
                _tempHardHoldInfo = GetCurrentHardHoldBalInfo();
                tempWin = Phoenix.Windows.Client.Helper.CreateWindow("Phoenix.Client.GbHoldStop", "Phoenix.Client.HoldStop", "frmGbStopHoldEdit");
                bool _pbIsTeller = true;
                //tempWin.InitParameters(acctType, acctNo, depLoan, rimNo, null, null, null, null, null, null, null, null, _pbIsTeller);
                tempWin.InitParameters(_tempHardHoldInfo, acctType, acctNo, depLoan, null, "new", -1, rimNo, null, null, _pbIsTeller);
                //GbHoldStopEdit.InitParameters(tempObj, this._busobjGbHoldStop.AcctType.Value, this._busobjGbHoldStop.AcctNo.Value, depLoan.StringValue, holdId.IntValue, "new", -1, rimNo.Value,null,null,_pbIsTeller.Value); // #79370 - pass RimNo //#83857 - Pass parameter to identify if new restrictions window is called from Tltrancode
            }

            if (tempWin != null)
            {
                tempWin.Workspace = _curWkspace;
                tempWin.Closed += new EventHandler(tempWin_Closed);
                tempWin.Show();
            }
        }

        //// Assigning a required content for each auto generated Document
        //void tabbedView1_QueryControl(object sender, DevExpress.XtraBars.Docking2010.Views.QueryControlEventArgs e)
        //{

        //    //if (e.Document == documentTransaction)
        //    //    e.Control = this.ucTransactions1;
        //    if (e.Control == null)
        //        e.Control = new System.Windows.Forms.Control();
        //}

        //Begin Bug #100897
        public async void RefreshCurrentRimInfo()
        {
            if (ucCustomer1.CurrentRimNo > 0)
            {
                await ucCustomer1.PopulateCustomerInfo(ucCustomer1.CurrentRimNo, false);
            }
        }
        //End Bug #100897

        public int GetCurrentRim()
        {
            return ucCustomer1.CurrentRimNo;
        }

        public void Destroy()
        {
            ucCustomer1.Destroy();
            ucAccounts1.Destroy();
            ucTransactions1.Destroy();
        }

        public void setTitleBar(string rimName)
        {
            _tlTranCodeWindow.setTitleBar(rimName);
        }

        public void setTitleBarName(int rimNo, bool pushRimIntoTran) //Bug#103638
        {
            _tlTranCodeWindow.setTitleBarName(rimNo, true);
        }

        /// <summary>
        /// Offline Task#90007
        /// </summary>
        void OfflineSettings()
        {
            if (!TellerVars.Instance.IsAppOnline)
            {
                dockPanelCustomer.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;
                dockPanelAccounts.Visibility = DevExpress.XtraBars.Docking.DockVisibility.Hidden;

                ucCashIn1.Focus();

            }
        }

        //Begin Task#100769
        /// <summary>
        /// //Set values for Confidential
        /// </summary>
        void GetConfidentialSettings()
        {
            #region Cache AdGbRsm

            if (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] == null)
            {
                adGbRsm = new AdGbRsm();
                adGbRsm.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                adGbRsm.SelectAllFields = true;
                if (CoreService.AppSetting.IsServer)
                    adGbRsm.DoAction(XmActionType.Select, CoreService.DbHelper);
                else
                    CoreService.DataService.ProcessRequest(XmActionType.Select, adGbRsm);
                GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] = adGbRsm;
            }
            else
            {
                adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);
            }
            #endregion

            #region Cache adGbBankControl

            if (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBankControl] == null)
            {
                adGbBankControl = new AdGbBankControl();
                adGbBankControl.SelectAllFields = true;
                if (CoreService.AppSetting.IsServer)
                    adGbBankControl.DoAction(XmActionType.Select, CoreService.DbHelper);
                else
                    CoreService.DataService.ProcessRequest(XmActionType.Select, adGbBankControl);
                GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBankControl] = adGbBankControl;
            }
            else
            {
                adGbBankControl = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBankControl] as AdGbBankControl);
            }
            #endregion

            _confAcctSecurity = (adGbBankControl.ConfAcctSecurity.Value == "Y");
            _userConfidentialAccess = (adGbRsm.ConfAcctView.Value == "N" && adGbRsm.ConfAcctIgnore.Value == "N");
        }
        //End Task#100769

        //Begin Task #103255
        public bool ValidateTransactions()
        {
            return ucTransactions1.ValidateTransactions();
        }

        public void FocusInitialField()
        {
            ucCustomer1.FocusProperField();
        }
        //End Task #103255

        //Begin 122356  
        public bool IsFocusOnPanel()
        {
            return (ucAccounts1.ContainsFocus || ucCustomer1.ContainsFocus);
        }
        //End 122356
    }
}
