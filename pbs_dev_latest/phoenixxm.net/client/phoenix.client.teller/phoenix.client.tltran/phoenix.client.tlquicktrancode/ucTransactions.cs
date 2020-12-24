#region Comments
//-------------------------------------------------------------------------------
// File Name: ucCashIn.cs
// NameSpace: Phoenix.Client.Teller
//12/11/2019    63      partha      Task#121715 - When you add a row and then delete it and then add another row, DevExpress takes the focus to the last column on which the focus was set
//-------------------------------------------------------------------------------
#endregion

#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//05/04/2018    2       FOyebola    Enh#220316 - Task 89907
//05/09/2018    3       FOyebola    Enh#220316 - Task 87735
//05/11/2018    4       FOyebola    CVT#90913 - Dereference after null check
//05/23/2018    5       FOyebola    Enh#220316 - Task 87757
//06/04/2018    6       FOyebola    Enh#220316 - Task 87759
//06/08/2018    7       FOyebola    Enh#220316 - Task 87761
//06/11/2018    8       CRoy        Bug #91947
//06/13/2018    9       FOyebola    Enh#220316 - Task 87751
//06/20/2018    10      FOyebola    Enh#220316 - Task 87753
//06/20/2018    11      FOyebola    Enh#220316 - Bug 93993
//06/22/2018    12      FOyebola    CVT Issue 184308 - Task 94126 
//06/22/2018    13      FOyebola    CVT Issue 184582 - Task 94134 
//06/26/2018    14      CRoy        Bug #93963 
//06/28/2018    15      FOyebola    Enh#220316 - Bug 93253
//07/02/2018    16      CRoy        Task 89995 - Transfer alert information.
//07/05/2018    17      DGarcia     Task 94607/94608
//07/12/2018    18      DGarcia     Bug 95564
//07/17/2018    19      CRoy        Bug #95593
//07/24/2018    20      FOyebola    Enh#220316 - Bug 95598
//07/30/2018    21      CRoy        Task #87737
//07/31/2018    22      CRoy        Bug #97326
//08/10/2018    23      CRoy        Task #90004
//08/14/2018    24      CRoy        Bug #98535 - Transaction panel header buttons.
//08/17/2018    25      CRoy        Bug #98671 - Look and Feel
//08/23/2018    26      CRoy        Bug #99049 - Add tooltip and fix image
//08/27/2018    27      CRoy        Bug #99050 - Line up column headings
//09/05/2018    28      CRoy        Bug #98178 - Send keys to put emphasis on focused column.
//09/10/2018    29      mselvaga    Bug #99058 - Teller charges shows incorrect amounts.
//09/12/2018    30      CRoy        Bug #101592 - Disable devexpress search panel when hitting CTRL+F
//09/12/2018    31      DGarcia     Task#90007 - Offline teller for Quick Transactions
//09/14/2018    32      CRoy        Bug #101885 - Set the enable and disable state of add new row and delete a row.
//09/19/2018    33      CRoy        Bug #99051 - Change Tfr Acct No for an editable combo box.
//09/24/2018    34      CRoy        Bug #99053 - Add indicator that submenu exists.
//09/27/2018    35      CRoy        Bug #102499 - Add support for option key and remove grid default right click menu.
//10/10/2018    36      DGarcia     Task#102715 - Offline teller for Quick Transactions-Fix known issues.
//11/02/2018    37      CRoy        Task #103255 - Add validation before posting transaction
//11/14/2018    38      DGarcia     Bug#103647 - Offline misc
//11/27/2018    39      FOyebola    Enh#220316 - Task#104267 
//01/08/2019    40      CRoy        Bug #107399 
//01/09/2019    41      FOyebola    Bug #108015
//01/10/2019    42      CRoy        Bug #107400
//01/14/2019    43      CRoy        Bug #105770
//01/14/2019    44      CRoy        Bug #108094
//01/14/2019    45      DGarcia     Bug #108552
//01/16/2019    46      CRoy        Bug #108662
//01/16/2019    47      CRoy        Bug #108700
//01/17/2019    48      CRoy        Bug #108757 - Add code to disable the funnel icon in the headers.
//11/27/2018    49      FOyebola    Enh#220316 - Task#108811
//01/28/2019    50      DGarcia     Bug #109821
//04/29/2019    51      CRoy        Task #113989 - Remove 0 from check number.
//05/02/2019    52      CRoy        Bug #114145 - Truncate 0 from check number.
//05/08/2019    53      CRoy        Task #114302 - Fix Transaction grid column with and account dropdown.
//05/29/2019    54      CRoy        Bug #115163 - Fix secondary Accounts missing from DpAccounts.
//05/31/2019    55      CRoy        Task #114936 - Move cursor to next cell when combo box option is selected.
//06/11/2019    56      CRoy        Bug #115881 - Fix Control + Insert key not popping up tran type combo box menu.
//06/12/2019    57      CRoy        Task #115712 - Focus first incomplete row in transaction panel when using ctrl + down arrow.
//06/21/2019    58      CRoy        Task #116300 - Add space shortcut when selecting tran menu in transaction grid to open transaction menu
//06/21/2019    59      CRoy        Bug #115230 - Remove filters in repository item lookup edit and leave it as autocomplete.
//07/11/2019    60      FOyebola    Task#114934
//11/18/2019    61      FOyebola    Task#121713
//11/25/2019    62      FOyebola    Task#121714
//12/04/2019    63      FOyebola    Task#122355
//12/12/2019    64      FOyebola    Task#122493
//12/18/2019    65      mselvaga    Bug#122727 - MT Tab:  Add accelerator key 'G' on Get Acct button
//01/03/2020    66      FOyebola    Task#122798
//01/28/2020    67      mselvaga    Bug#123952 - Fixed the _parentControl from disappearing after the grid refresh the first time.
//02/11/2020    68      mselvaga    Bug#124210 - Action panel not refreshed after removing all rows in Tran panel
//02/11/2020    69      FOyebola    Bug#124209 - CTRL + from MT tran panel brings up floating windnow - must disable
//02/11/2020    70      FOyebola    Bug#124214 - The Charges column under transaction panel is editable for deposit 
//02/20/2020    71      mselvaga    Bug#124295 - MT 2020 - Open check column for Loan Advance
//02/20/2020    72      mselvaga    Bug#124210 - Fixed Column 'AcctNo' does not belong to table.
//02/24/2020    73      mselvaga    Bug#124249 - MT 2020 only- Must ignore empty Transaction row on Post
//02/25/2020    74      FOyebola    Task#124297
//02/26/2020    75      mselvaga    Task#124403 - MT Tran Offline Issues found during H&R
//03/02/2020    76      mselvaga    #124249 - Added changes to remove empty transaction row.
//03/04/2020    77      mselvaga    #124917 - MT Offline - User has to select the Tran Type 2 times 
#endregion

using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraEditors;
using Phoenix.MultiTranTeller.Base;
using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.MultiTranTeller.Base.ViewModels;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.EditForm.Helpers;
using DevExpress.Utils;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Global;
using Phoenix.Client.Teller;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using System.Drawing;
using System.IO;

using System.Collections.Generic; //87735

using DevExpress.XtraEditors.Repository; //87735

using DevExpress.XtraGrid.Menu; //87757
using DevExpress.XtraGrid.Columns;
using System.Threading;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Skins;
using DevExpress.LookAndFeel;

//Begin Task#90007
using Phoenix.Shared.Variables;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using System.ComponentModel;
//End Task#90007

using DevExpress.XtraEditors.ViewInfo;    //121714-2EFG

namespace Phoenix.Client.Teller
{
    public partial class ucTransactions : EditFormUserControl
    {
        ITellerWindow _parentWindow;
        private GroupControl _parentControl;
        CustomHeaderButton _btnAddNew;
        CustomHeaderButton _btnDeleteRow;
        TransactionAlerts _tranAlerts; //Task 89995
        HashSet<int> _tranOptionSelectedForRows = new HashSet<int>(); //Task #115712


        //Begin 87735
        //Begin 87759
        //List<TransactionDefinition> _DpTCDefList = new List<TransactionDefinition>(); 
        //List<TransactionDefinition> _LnTCDefList = new List<TransactionDefinition>();
        //List<TransactionDefinition> _RmTCDefList = new List<TransactionDefinition>();

        List<TransactionTypeDefinition> _DpTCDefList = new List<TransactionTypeDefinition>();
        List<TransactionTypeDefinition> _LnTCDefList = new List<TransactionTypeDefinition>();
        List<TransactionTypeDefinition> _RmTCDefList = new List<TransactionTypeDefinition>();
        //End 87759
        //End 87735
        //Begin 87751
        List<TransactionTypeDefinition> _ExtTCDefList = new List<TransactionTypeDefinition>();
        List<TransactionTypeDefinition> _AllTCDefList = new List<TransactionTypeDefinition>();
        //End 87751

        //Begin 87753
        DataTable _DpAccts = new DataTable();
        DataTable _LnAccts = new DataTable();
        DataTable _ExAccts = new DataTable();
        DataTable _RmAccts = new DataTable();
        DataTable _AllAccts = new DataTable();
        //End 87753


        //Begin 87757
        //string[] TransferOptions = new string[2] { "Deposit", "Withdrawal" };
        //string[] LoanOptions = new string[3] { "Payment1", "Payment2", "Payment3" };
        //End 87757

        //Begin 87753
        public string rimNo = string.Empty;
        public string rimStatus = string.Empty;
        //End 87753

        //Begin Task #87737
        private HashSet<int> _rowsToShowError = new HashSet<int>();
        private Dictionary<string, string> _mandatoryFields = new Dictionary<string, string>();
        private string _errorFieldToFocus = "";
        //End Task #87737

        private TellerVars _tellerVars; //Moved to constructor = TellerVars.Instance; //Task#90007
        private int _gridRowHeight = 0; //Bug #108094

        private bool custSearchSucceeded = false; //Bug #108552
        private bool _resetTranAmount = false; //Bug #108700
        private bool _popupFocused = false; //Task #114936

        private bool _autoTranTypeSelect = false;  //121714-2EFG
        GridColumn PrevColumn;                     //121714-2EFG

        private bool _newTranRecordMode = false; //122493

        //Begin Task #114936
        public bool IsAnyControlFocused()
        {
            if (_popupFocused)
                return true;

            bool focused = this.Focused;

            foreach (Control control in this.Controls)
            {
                if (focused)
                    break;
                focused = control.Focused;
            }

            return focused;
        }
        //End Task #114936

        public ucTransactions()
        {
            InitializeComponent();
            //
            if (PDesignModeHelper.IsDesignMode == false)  // Muthu Moved it from the decleration
            {
                this.gridColOffsetAccount.ColumnEdit = (_tellerVars != null && _tellerVars.IsAppOnline) ? this.repItemTfrAccounts : null;
                _tellerVars = TellerVars.Instance; //Task#90007
            }
            CenterColumnHeadings();
            DisableColumnFiltering(); //Bug #108757

            //These flags must be set so tabbing will work within the grid
            gvTransactions.OptionsBehavior.FocusLeaveOnTab = true;
            gvTransactions.OptionsNavigation.UseTabKey = true;
            gvTransactions.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            gvTransactions.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;

            //Begin Task#102715
            if (PDesignModeHelper.IsDesignMode == false) // Muthu - Added to enabled designtime experience 
            {
                if (_tellerVars.IsAppOnline)
                {
                    _mandatoryFields = new Dictionary<string, string>() {
                    {"TcDescription","A transaction type needs to be selected."},
                    //{"AcctCombined","An account needs to be selected."}
                };
                }
                else
                {
                    _mandatoryFields = new Dictionary<string, string>() {
                    {"TcDescription","A transaction type needs to be selected."},
                    //{"AcctType","An account type needs to be selected."},
                    //{"AcctNo","An account needs to be selected."}
                };
                }
            }
            //End Begin Task#102715
            gvTransactions.ShownEditor += OnShowEditor; //Bug #102499
            gvTransactions.CalcRowHeight += OnCalcRowHeight; //Bug #108094  

            //Begin Task #114936
            repositoryItemTC.Popup += OnPopupOpen;
            repositoryItemTC.QueryCloseUp += OnPopupClose;

            repItemAccounts.Popup += OnPopupOpen;
            repItemAccounts.QueryCloseUp += OnPopupClose;

            repItemTfrAcctType.Popup += OnPopupOpen;
            repItemTfrAcctType.QueryCloseUp += OnPopupClose;

            repItemTfrAccounts.Popup += OnPopupOpen;
            repItemTfrAccounts.QueryCloseUp += OnPopupClose;
            //End Task #114936

            SetColumnsFixed();

            //Remove plus sign at the beginning of a row.
            gvTransactions.OptionsDetail.EnableMasterViewMode = false;

            gvTransactions.OptionsCustomization.AllowSort = false; //121714-2EFG


        }

        private void SetColumnsFixed()
        {
            foreach (BandedGridColumn col in this.gvTransactions.Columns)
            {
                col.OptionsColumn.AllowMove = false;
            }
        }

        //Begin Task #114936
        private void OnPopupClose(object sender, CancelEventArgs e)
        {
            _popupFocused = false;
        }

        private void OnPopupOpen(object sender, EventArgs e)
        {
            LookUpEdit lookup = sender as LookUpEdit;
            if (lookup != null)
            {
                lookup.Properties.UseCtrlScroll = false;
            }
            _popupFocused = true;
        }
        //End Task #114936

        //Begin Bug #99050
        private void CenterColumnHeadings()
        {
            BandedGridColumn[] headers = new BandedGridColumn[] {
                gridColDesc,
                gridColAccount,
                gridColAmount,
                gridColHoldBal,
                gridColOffsetAccount,
                gridColOptions,
                gridColPassToHistory,
                gridColRefField,
                gridColRegD,
                gridColTfrAcctType,
                gridColTfrAmt,
                gridColTran,
                gridColTranDef,
                gridColCheckNo,
                gridColCcAmt,
                gridColMenuIndicator
            };

            foreach (var header in headers)
            {
                header.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            }
        }
        //End Bug #99050

        //Begin Bug #102499
        private void OnShowEditor(object sender, EventArgs e)
        {
            GridView view = sender as GridView;

            bool ignoreShowPopUp = false; //121714-2EFG

            if (view != null)
            {
                TextEdit te = view.ActiveEditor as TextEdit;
                if (te != null)
                {
                    te.Properties.BeforeShowMenu += new DevExpress.XtraEditors.Controls.BeforeShowMenuEventHandler(Properties_BeforeShowMenu);
                }

                //Begin 121714-1AB&2A
                /*** if the column is enabled set the color to LightBlue otherwise setthe color to LightGray ***/
                if (view.ActiveEditor.ReadOnly)
                    view.ActiveEditor.BackColor = Color.LightGray;
                //End 121714-1AB&2A 

                //Begin 121714-2EFG
                LookUpEdit activeEditor = view.ActiveEditor as LookUpEdit;

                if (activeEditor != null && (!view.ActiveEditor.ReadOnly))
                {
                    if (activeEditor.ItemIndex == -1)
                    {
                        _autoTranTypeSelect = true;

                        activeEditor.EditValue = activeEditor.Properties.GetDataSourceValue(activeEditor.Properties.ValueMember, 0);
                        activeEditor.Focus();
                    }

                    if (!ignoreShowPopUp)
                        activeEditor.ShowPopup();
                }
                //End 121714-2EFG 
            }
        }

        void Properties_BeforeShowMenu(object sender, DevExpress.XtraEditors.Controls.BeforeShowMenuEventArgs e)
        {
            e.Menu.Items.Clear();
        }
        //End Bug #102499

        //Begin Task #114302
        public void SetDataSource(RepositoryItemLookUpEdit lookupEdit, DataTable dataTable)
        {
            lookupEdit.DataSource = dataTable;

            int rowCount = 0;
            if (dataTable != null && dataTable.Rows != null
                && dataTable.Rows.Count > 0)
            {
                rowCount = Math.Min(dataTable.Rows.Count, 10);
            }

            lookupEdit.DropDownRows = rowCount;
            lookupEdit.UseCtrlScroll = false;
        }

        public void SetDataSource(LookUpEdit lookupEdit, DataTable dataTable)
        {
            lookupEdit.Properties.DataSource = dataTable;

            int rowCount = 0;
            if (dataTable != null && dataTable.Rows != null
                && dataTable.Rows.Count > 0)
            {
                rowCount = Math.Min(dataTable.Rows.Count, 10);
            }

            lookupEdit.Properties.DropDownRows = rowCount;
            lookupEdit.Properties.UseCtrlScroll = false;
        }
        //End Task #114302

        public void InitializeTeller(ITellerWindow parentWindow)
        {
            _parentWindow = parentWindow;
            if (_parentWindow != null)
            {
                _parentWindow.OnCustomerChanged += _parentWindow_OnCustomerChanged;
            }
            _parentControl = WinHelper.GetParent<GroupControl>(this);

            if (_parentControl != null)
            {
                _tranAlerts = new TransactionAlerts(parentWindow, _parentControl); //Task 89995
                _btnDeleteRow = new DevExpress.XtraBars.Docking.CustomHeaderButton("Delete", imgTransactions.Images[(int)ImageList.Delete], -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1); //Bug #99049
                _btnDeleteRow.ToolTip = "Delete selected row  Ctrl+Delete"; //Bug #99049
                _btnAddNew = new DevExpress.XtraBars.Docking.CustomHeaderButton("Add", imgTransactions.Images[(int)ImageList.Insert], -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1); //Bug #99049
                _btnAddNew.ToolTip = "Add new row  Ctrl+Insert"; //Bug #99049
                _parentControl.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _btnDeleteRow, _btnAddNew });
                _parentControl.CustomButtonClick += ParentControl_CustomButtonClick;
                HideAlerts(); //Task 89995
                SkinElement element = SkinManager.GetSkinElement(SkinProductId.Docking, UserLookAndFeel.Default, "DockWindowCaption");
                element.ContentMargins.Top = 5;
                element.ContentMargins.Bottom = 2;
                LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            }

            //Begin 87735

            //List<TransactionDefinition> TCDefList = TranHelper.Instance.TranList;       //87759
            List<TransactionTypeDefinition> TCDefList = TranHelper.Instance.TranTypeList; // 87759 


            //Begin Task#102715
            if (!_tellerVars.IsAppOnline)
                TCDefList.RemoveAll((x => x.DpLN == "EX"));
            //End Task#102715

            //Begin 93253
            try
            {
                this.repositoryItemTC.DataSource = TCDefList;
            }
            catch (Exception e)
            {
            }
            //End 93253


            _AllTCDefList = TCDefList; //87751

            //foreach (TransactionDefinition TranDef in TCDefList)    //87759 
            foreach (TransactionTypeDefinition TranDef in TCDefList)  //87759 
            {
                if (TranDef.DpLN == "DP")
                    _DpTCDefList.Add(TranDef);
                else if (TranDef.DpLN == "LN")
                    _LnTCDefList.Add(TranDef);
                else if (TranDef.DpLN == "EX") //87751
                    _ExtTCDefList.Add(TranDef);
                else if (TranDef.DpLN == "RM")
                {
                    _RmTCDefList.Add(TranDef);
                    _DpTCDefList.Add(TranDef);
                    _LnTCDefList.Add(TranDef);
                    _ExtTCDefList.Add(TranDef);   //87751
                }
            }

            //End 87735


            //TranHelper.Instance.CustomerTransactions.Clear();
            //TranHelper.Instance.CustomerTransactions.Add(new CustomerTransaction());

            //Begin 93253
            try
            {
                //Begin CVT#90913
                if (_parentWindow != null)
                {
                    this.gcTransactions.DataSource = _parentWindow.TransactionSet.Transactions; //.Instance.CustomerTransactions;
                }
                //End CVT#90913

                this.gcTransactions.RefreshDataSource();
            }
            catch (Exception e)
            {
            }
            //End 93253

            //
            EasyCaptureTran _temp = new EasyCaptureTran();
            //
            this.gridColAccount.FieldName = "AcctCombined";

            //Begin 93253
            try
            {
                SetDataSource(this.repItemAccounts, _parentWindow.Accounts); //Begin Task #114302
            }
            catch (Exception e)
            {
            }
            //End 93253

            this.repItemAccounts.DisplayMember = "AccountCombined";
            this.repItemAccounts.ValueMember = "AccountCombined";

            //this.repItemAccounts.ValueMember = "AccountCombined";  //sets the value to show in cell when dropdown row is selected
            this.gridColTran.FieldName = "TcDescription";
            this.repositoryItemTC.DisplayMember = "MenuDisplayDescription";  //87759: was "Description";
            this.repositoryItemTC.ValueMember = "TranType";    //87759: was "Description";  "Description"; //sets the value to show in cell when dropdown row is selected

            this.gvTransactions.FocusedRowChanged += gvTransactions_FocusedRowChanged;
            this.gvTransactions.CellValueChanged += GvTransactions_CellValueChanged;
            this.gvTransactions.CellValueChanging += CellValueChanging; //Bug #108700
            this.gvTransactions.FocusedColumnChanged += GvTransactions_FocusedColumnChanged; //#89907
            this.gvTransactions.Click += GvTransactions_Click;  //#87767
            this.gvTransactions.ValidatingEditor += GvTransactions_ValidatingEditor; //Task #113989
            this.gvTransactions.RowCellStyle += GvTransactions_RowCellStyle; //Task #113989
            this.gvTransactions.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp; //Task #113989

            //Begin 87757
            this.gcTransactions.MouseDown += GcTransactions_MouseDown;
            //End 87757

            //
            this.ucCashOut1.InitializeTeller(parentWindow);

            //Begin 89907
            foreach (Control control in this.Controls)
            {
                control.GotFocus += Control_GotFocus;
            }
            //End 89907

            repItemAccounts.EditValueChanging += RepItemAccounts_EditValueChanging;//87753

            repItemAccounts.Enter += RepItemAccounts_Enter; //87753-2

            SetHeaderButtonsState();//Bug #98535 

            OfflineSettings(); //Task#90007

            //Begin 121714-2C
            gvTransactions.Appearance.FocusedRow.BackColor = Color.LightYellow; //Color.FromArgb(100, 180, 80);             //Applied when the grid is focused 
                                                                                //gvTransactions.Appearance.HideSelectionRow.BackColor = Color.Transparent //Color.White; //Color.LightGray;    //Applied when the grid is not focused
                                                                                //GridViewAppearances.EvenRow

            //gvTransactions.RowStyle += GvTransactions_RowStyle;
            //End 121714-2C

            gvTransactions.ShowingEditor += GvTransactions_ShowingEditor; //122493
        }

        //Begin 122493
        private void GvTransactions_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = sender as GridView;

            e.Cancel = ((view.FocusedColumn.FieldName == "RegD" || view.FocusedColumn.FieldName == "PassDescriptionToHistory") && view.FocusedColumn.ColumnEdit.ReadOnly == true);

        }
        //End 122493

        //Begin 121714-2C
        private void GvTransactions_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle >= 0 && e.RowHandle != gvTransactions.FocusedRowHandle)
            {
                if (e.RowHandle % 2 == 1)
                    e.Appearance.BackColor = Color.LightCyan;
                else
                    e.Appearance.BackColor = Color.Transparent;
            }
        }
        //End 121714-2C

        //Begin Task #115712
        public void FocusFirstIncompleteRow()
        {
            for (int i = 0; i < _parentWindow.TransactionSet.Transactions.Count; i++)
            {
                string rowError = GetErrorMessage(i);
                bool tranMenuOptionSelected = _tranOptionSelectedForRows.Contains(i);
                if (!string.IsNullOrEmpty(rowError) || (DoesRowContainsTranMenu(i) && !tranMenuOptionSelected))
                {
                    if (DoesRowContainsTranMenu(i) && !tranMenuOptionSelected && _errorFieldToFocus == "Amount")
                    {
                        _errorFieldToFocus = "MenuIndicator";
                    }
                    FocusProblematicField(_errorFieldToFocus, i);
                    break;
                }
            }
        }
        //End Task #115712


        //Begin Bug #108700
        private void CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            _resetTranAmount = false;
            string strVal = e.Value as string;

            strVal = (!string.IsNullOrEmpty(strVal)) ? strVal.Trim() : strVal; //121713-#7:Bug-fix

            if (e.Column == gridColAmount && string.IsNullOrEmpty(strVal))
            {
                _resetTranAmount = true;
            }
        }
        //End Bug #108700

        //Begin 89907
        private void GvTransactions_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
        {
            Control_GotFocus(gcTransactions, null);

            //Begin Task 94607/94608
            string acctNo = null;
            string acctType = null;
            DevExpress.XtraGrid.Views.BandedGrid.BandedGridView view = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;

            if (view.GetRowCellValue(view.FocusedRowHandle, "AcctNo") != null && view.GetRowCellValue(view.FocusedRowHandle, "AcctNo").ToString() != string.Empty &&
                   view.GetRowCellValue(view.FocusedRowHandle, "AcctType") != null && view.GetRowCellValue(view.FocusedRowHandle, "AcctType").ToString() != string.Empty)
            {
                acctNo = view.GetRowCellValue(view.FocusedRowHandle, "AcctNo").ToString(); //Task 89995
                acctType = view.GetRowCellValue(view.FocusedRowHandle, "AcctType").ToString(); //Task 89995
            }
            //End Task 94607/94608

            //Begin 87757-2
            if (e.PrevFocusedColumn != null && e.PrevFocusedColumn.Name == "gridColOffsetAccount")
            {
                if (view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctNo") != null && view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctNo").ToString() != string.Empty &&
                   view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctType") != null && view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctType").ToString() != string.Empty)
                {
                    string formattedAcctNo = view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctNo").ToString();
                    string tfrAcctType = view.GetRowCellValue(view.FocusedRowHandle, "TfrAcctType").ToString();

                    _parentWindow.ActionRequest("FormatTfrAcctNo", tfrAcctType, ref formattedAcctNo);

                    if (formattedAcctNo != null && formattedAcctNo != string.Empty)
                    {
                        view.SetRowCellValue(view.FocusedRowHandle, "TfrAcctNo", formattedAcctNo);

                        ValidLoanfrAcctNo(); //87751 //87759

                        ResetTfrFlds();

                        ShowAlerts(acctType, acctNo, tfrAcctType, formattedAcctNo); //Task 89995
                    }
                    else
                    {
                        PMessageBox.Show(318515, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                        view.SetRowCellValue(view.FocusedRowHandle, "TfrAcctNo", null);
                        view.FocusedColumn = gridColOffsetAccount;
                    }
                }
                else if (!string.IsNullOrEmpty(acctNo) && !string.IsNullOrEmpty(acctType))
                {
                    ShowAlerts(acctType, acctNo); //Task 89995
                }
                else
                {
                    _tranAlerts.HideAlerts(); //Task 89995
                }
            }
            //End 87757-2

            _parentWindow.SetTransactionAccountInfo(acctType, acctNo); //Task#94607/94608 //Bug #109821

            RevalidateRows();

            //Begin Task#90007
            if (!_tellerVars.IsAppOnline && acctType != "RIM" && e.PrevFocusedColumn != null && e.PrevFocusedColumn.Name == "gridColAcctNo" &&
                    acctNo != null && acctType != null)
            {
                string formattedAcctNo = acctNo;
                _parentWindow.ActionRequest("FormatAcctNo", acctType, ref formattedAcctNo);
                if (!string.IsNullOrEmpty(formattedAcctNo))
                    view.SetRowCellValue(view.FocusedRowHandle, "AcctNo", formattedAcctNo);
            }
            //End Task#90007
        }

        //End 89907

        //Begin 87759
        public void ValidLoanfrAcctNo()   //87751
        {
            if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty)
            {
                //Begin 87751
                if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo") != null &&
                    gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo").ToString() != string.Empty &&
                    gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType") != null &&
                    gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType").ToString() != string.Empty)
                {
                    string acctType = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType").ToString();
                    string acctNo = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo").ToString();

                    string deploan = string.Empty;
                    _parentWindow.ActionRequest("GetDepLoan", acctType, ref deploan);

                    if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() == EasyCaptureTranTfrOptions.AutoLoanPayment.ToString())
                    {
                        if (deploan != "LN") //loan account type
                        {
                            //16004 - The account number and/or account type specified for this transaction is invalid.
                            PMessageBox.Show(16004, MessageType.Error, MessageBoxButtons.OK, String.Empty);

                            gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo", null);
                            gvTransactions.FocusedColumn = gridColOffsetAccount;
                        }
                    }
                    else if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() == EasyCaptureTranTfrOptions.TransferDeposit.ToString() ||
                             gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() == EasyCaptureTranTfrOptions.TransferWithdrawal.ToString())
                    {
                        string formattedAcctNo = acctNo;

                        _parentWindow.ActionRequest("FormatTfrAcctNo", acctType, ref formattedAcctNo);

                        if (formattedAcctNo != null && formattedAcctNo != string.Empty)
                        {
                            gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo", formattedAcctNo);
                        }
                        else
                        {
                            PMessageBox.Show(318515, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                            gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo", null);
                            gvTransactions.FocusedColumn = gridColOffsetAccount;
                        }
                    }
                }
                //End 87751
            }
        }
        //End 87759


        //Begin 87757-2
        public void ResetTfrFlds()
        {
            //Begin 87761
            if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription") != null &&
                gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "AcctCombined") != null &&
                (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                 gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty))
            {

                //Begin 122493: commented code
                /*
                string regDEnable = "N";
                string regDCheck = "N";
            
                var tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];
                */
                //End 122493: commented code

                if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString() == EasyCaptureTranType.Transfer.ToString())  //87759
                {
                    //Begin 122493: commented code
                    /*
                    //Begin 87759 
                    if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                        gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty) //87761
                    {
                        //*** To make sure that we have the current selected TC ***
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString());
                    }
                    //End 87759 

                    //if (tran.TranDef.TranCode == "Q36" || tran.TranDef.TranCode == "Q37" || tran.TranDef.TranCode == "Q38") //87759 
                    if (tran.TranDef != null && //87761
                        (tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferWithdrawal.ToString() ||
                        tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferDeposit.ToString() ||
                        tran.TranDef.Description == EasyCaptureTranCodeDesc.AutoLoanPayment.ToString()))
                    {
                        string regDValue = string.Empty;

                        //TC163 or TC164
                        if (tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferWithdrawal.ToString() ||
                            tran.TranDef.Description == EasyCaptureTranCodeDesc.AutoLoanPayment.ToString())
                        {
                            if ((tran.AcctNo != null && tran.AcctNo.ToString() != string.Empty) &&
                                (tran.AcctType != null && tran.AcctType.ToString() != string.Empty))
                            {
                                regDValue = tran.AcctType.Trim() + "~" + tran.AcctNo.Trim();
                                _parentWindow.ActionRequest((tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferWithdrawal.ToString() ? "GetTC163RegDValue" : "GetTC164RegDValue"), tran.TranDef.TranCode, ref regDValue);
                            }
                        }
                        //TC102
                        else
                        {
                            if ((tran.TfrAcctNo != null && tran.TfrAcctNo.ToString() != string.Empty) &&
                                (tran.TfrAcctType != null && tran.TfrAcctType.ToString() != string.Empty))
                            {
                                regDValue = tran.TfrAcctType.Trim() + "~" + tran.TfrAcctNo.Trim();
                                _parentWindow.ActionRequest("GetTC102RegDValue", tran.TranDef.TranCode, ref regDValue);
                            }
                        }

                        if (regDValue != string.Empty)
                        {
                            regDEnable = regDValue.Split("~".ToCharArray())[0];
                            regDCheck = regDValue.Split("~".ToCharArray())[1];
                        }
                    }
                    */
                    //End 122493: commented code

                    gridColOffsetAccount.OptionsColumn.ReadOnly = false;   //121714-1AB&2A
                    //gridColOffsetAccount.OptionsColumn.AllowEdit = true; //121714-1AB&2A
                    gridColOffsetAccount.OptionsColumn.TabStop = true;

                    repItemTfrAcctType.ReadOnly = false;
                    repItemTfrAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.True;
                    gridColTfrAcctType.OptionsColumn.TabStop = true;
                }
                else
                {
                    gridColOffsetAccount.OptionsColumn.ReadOnly = true;      //121714-1AB&2A
                    //gridColOffsetAccount.OptionsColumn.AllowEdit = false;  //121714-1AB&2A
                    gridColOffsetAccount.OptionsColumn.TabStop = false;

                    repItemTfrAcctType.ReadOnly = true;
                    repItemTfrAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.False;
                    gridColTfrAcctType.OptionsColumn.TabStop = false;
                }


                //Begin 122493: commented code
                /*
                gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "RegD", (regDCheck == "Y"));
                repositoryItemRegD.ReadOnly = (regDEnable != "Y");
                repositoryItemRegD.HotTrackWhenReadOnly = (regDEnable == "Y");
                gridColRegD.OptionsColumn.TabStop = (regDEnable == "Y");
                */
                //End 122493: commented code
            }
            //End 87761
            SetHeaderButtonsState();//Bug #98535
        }
        //End 87757-2

        private void CreateNewRowIfCurrentIsValid()
        {
            _parentWindow.MapEasyCaptureToTranSetAsync(false, "CollectionChanged", () => {
                string errorMessage = GetErrorMessage(gvTransactions.FocusedRowHandle);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowErrorAndFocusField(errorMessage, gvTransactions.FocusedRowHandle); //Task #87737
                }
                else
                {
                    _rowsToShowError.Remove(gvTransactions.FocusedRowHandle);
                    gvTransactions.AddNewRow();
                    AddNewRow();
                }
                SetHeaderButtonsState();//Bug #98535
            });
        }

        //Begin Bug #108757
        private void DisableColumnFiltering()
        {
            foreach (var column in gvTransactions.Columns)
            {
                var bandedGridColumn = column as BandedGridColumn;
                if (bandedGridColumn != null)
                {
                    bandedGridColumn.OptionsFilter.AllowFilter = false;
                }
            }
            //#120877
            gvTransactions.OptionsFind.AllowFindPanel = false;
            gvTransactions.OptionsFind.AlwaysVisible = false;
            gvTransactions.OptionsMenu.EnableColumnMenu = false;
        }
        //End Bug #108757

        public bool ForceDeleteEmptyRow()   //#124249
        {
            if (gvTransactions.RowCount > 0)
            {
                if (gvTransactions.FocusedColumn != null)
                {
                    GridView view = gvTransactions as GridView;
                    if (view.IsEditing)
                    {
                        SendKeys.SendWait("{ENTER}");
                        return false;
                    }
                    else
                    {
                        //#124249
                        EasyCaptureTran selectedTranRow = GetSelectedRow();
                        if (selectedTranRow != null && selectedTranRow.IsValidated == false && string.IsNullOrEmpty(selectedTranRow.SelectedAcctNo))
                        {
                            DeleteRow();
                        }
                        else if (string.IsNullOrEmpty(Convert.ToString(view.GetRowCellValue(view.FocusedRowHandle, "AcctNo"))))
                        {
                            DeleteRow();
                        }
                        if (gvTransactions.RowCount == 0) //Remove the last displayed alerts
                        {
                            HideAlerts();
                        }
                        else if (gvTransactions.RowCount >= 1) //Refresh the alerts for the account in the focused row
                        {
                            gvTransactions.FocusedColumn = null;    //121715
                            gvTransactions_FocusedRowChanged(null, null);
                        }
                        return true;
                    }
                }
            }
            else
            {
                //if (_tellerVars.IsAppOnline)  //#124403 - Commmented out
                _parentWindow.SetTransactionAccountInfo(null, null, true);
                return false;
            }
            return true;
        }

        //public bool ForceDeleteEmptyRow()   //#124249
        //{
        //    if (gvTransactions.RowCount > 0)
        //    {
        //        if (gvTransactions.FocusedColumn != null)
        //        {
        //            GridView view = gvTransactions as GridView;
        //            if (view.IsEditing && _popupFocused)
        //            {
        //                SendKeys.SendWait("{ENTER}");
        //                return false;
        //            }
        //            else
        //            {
        //                //#124249
        //                EasyCaptureTran selectedTranRow = GetSelectedRow();
        //                if (selectedTranRow != null && selectedTranRow.IsValidated == false && string.IsNullOrEmpty(selectedTranRow.SelectedAcctNo))
        //                {
        //                    DeleteRow();
        //                    if (gvTransactions.RowCount == 0) //Remove the last displayed alerts
        //                    {
        //                        HideAlerts();
        //                    }
        //                    else if (gvTransactions.RowCount >= 1) //Refresh the alerts for the account in the focused row
        //                    {
        //                        gvTransactions.FocusedColumn = null;    //121715
        //                        gvTransactions_FocusedRowChanged(null, null);
        //                    }
        //                }
        //                return true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //if (_tellerVars.IsAppOnline)  //#124403 - Commmented out
        //        _parentWindow.SetTransactionAccountInfo(null, null, true);
        //        return false;
        //    }
        //    return true;
        //}

        //Begin Task #103255
        public bool ValidateTransactions()
        {
            bool valid = true;

            for (int i = 0; i < gvTransactions.RowCount; i++)
            {
                string errorMessage = GetErrorMessage(i);
                valid = string.IsNullOrEmpty(errorMessage);
                if (!valid)
                {
                    ShowErrorAndFocusField(errorMessage, i);
                    break;
                }
                else
                {
                    _rowsToShowError.Remove(i);
                }
            }

            return valid;
        }
        //End Task #103255

        private void ParentControl_CustomButtonClick(object sender, BaseButtonEventArgs e)
        {
            if (e.Button == _btnDeleteRow)
            {
                DeleteRow();

                //Begin 108015 

                //GvTransactions_FocusedColumnChanged(null, null); //Bug 107399

                if (gvTransactions.RowCount == 0) //Remove the last displayed alerts
                {
                    HideAlerts();
                }
                else if (gvTransactions.RowCount >= 1) //Refresh the alerts for the account in the focused row
                {
                    gvTransactions.FocusedColumn = null;    //121715
                    gvTransactions_FocusedRowChanged(null, null);
                }
                //End 108015 

            }
            else if (e.Button == _btnAddNew)
            {
                ButtonAddNewClick();
            }
        }

        private void ButtonAddNewClick()
        {
            //Begin Bug# 108552 
            if (custSearchSucceeded || !_tellerVars.IsAppOnline)
                CreateNewRowIfCurrentIsValid();
            //End Bug# 108552 

            //Task#90007
            //if (!_tellerVars.IsAppOnline || (_parentWindow.Accounts != null && _parentWindow.Accounts.Rows != null && _parentWindow.Accounts.Rows.Count > 0))
            //{
            //CreateNewRowIfCurrentIsValid();
            //}
        }

        //Begin Bug #98535
        public void SetHeaderButtonsState()
        {
            //Begin Bug# 108552 
            if (custSearchSucceeded || !_tellerVars.IsAppOnline)
                _btnAddNew.Enabled = true;
            else
                _btnAddNew.Enabled = false;
            //_btnAddNew.Enabled = !_tellerVars.IsAppOnline || (_parentWindow.Accounts != null && _parentWindow.Accounts.Rows != null && _parentWindow.Accounts.Rows.Count > 0); //Task#90007
            //End Bug# 108552 

            _btnDeleteRow.Enabled = _parentWindow.TransactionSet != null && _parentWindow.TransactionSet.Transactions != null && _parentWindow.TransactionSet.Transactions.Count > 0;
        }
        //End Bug #98535

        //Begin Task #90004
        private void RevalidateRows()
        {
            string errorMessage = GetErrorMessage(gvTransactions.FocusedRowHandle);
            if (string.IsNullOrEmpty(errorMessage))
            {
                _rowsToShowError.Remove(gvTransactions.FocusedRowHandle);
            }
        }
        //End Task #90004

        private void GvTransactions_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //Begin Bug #108700
            if (e.Column == this.gridColAmount && _resetTranAmount)
            {
                _resetTranAmount = false;
                this.gvTransactions.SetRowCellValue(e.RowHandle, "Amount", 0);
            }
            //End Bug #108700


            if (e.Column == this.gridColPassToHistory || e.Column == this.gridColRegD || e.Column == this.gridColOptions) //87757
            {
                gvTransactions.UpdateCurrentRow();
            }

            //Begin 87757
            if (e.Column == this.gridColOptions || e.Column == this.gridColTran || e.Column == this.gridColAmount || e.Column == this.gridColAccount ||
                e.Column == this.gridColAcctNo || e.Column == this.gridColActType) //Task# 90007
            {
                DevExpress.XtraGrid.Views.BandedGrid.BandedGridView view = sender as DevExpress.XtraGrid.Views.BandedGrid.BandedGridView;

                var tran = _parentWindow.TransactionSet.Transactions[view.FocusedRowHandle];

                //Begin 87761
                if (view.GetRowCellValue(view.FocusedRowHandle, "TcDescription") != null)
                {
                    //87761: Moved code here outside the Transfer TranType condition
                    if (view.GetRowCellValue(view.FocusedRowHandle, "Options") != null && e.Column == this.gridColOptions)
                    {
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == view.GetRowCellValue(view.FocusedRowHandle, "Options").ToString()); //87759
                    }

                    if (view.GetRowCellValue(view.FocusedRowHandle, "TcDescription").ToString() == EasyCaptureTranType.Transfer.ToString())  //87759
                    {
                        if (view.GetRowCellValue(view.FocusedRowHandle, "Options") == null && e.Column == this.gridColTran)
                        {
                            view.SetRowCellValue(view.FocusedRowHandle, "Options", EasyCaptureTranTfrOptions.TransferWithdrawal.ToString());

                            tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranTfrOptions.TransferWithdrawal.ToString());  //87759
                        }

                        if (e.Column == this.gridColAmount)
                        {
                            view.SetRowCellValue(view.FocusedRowHandle, "TfrAmount", Convert.ToDecimal(view.GetRowCellValue(view.FocusedRowHandle, "Amount")));
                        }
                    }
                    else if (view.GetRowCellValue(view.FocusedRowHandle, "TcDescription").ToString() == EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString()) //87761
                    {
                        if (view.GetRowCellValue(view.FocusedRowHandle, "Options") == null && e.Column == this.gridColTran)
                            view.SetRowCellValue(view.FocusedRowHandle, "Options", EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString()); //87761
                    }
                    //Begin 87751
                    else if (view.GetRowCellValue(view.FocusedRowHandle, "TcDescription").ToString() == EasyCaptureTranType.External.ToString())
                    {
                        if (view.GetRowCellValue(view.FocusedRowHandle, "Options") == null && e.Column == this.gridColTran)
                        {
                            view.SetRowCellValue(view.FocusedRowHandle, "Options", EasyCaptureTranExtOptions.ExternalCredit.ToString());
                            tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranExtOptions.ExternalCredit.ToString());
                        }
                    }
                    //End 87751
                    else if (view.GetRowCellValue(view.FocusedRowHandle, "TcDescription").ToString() == EasyCaptureTranPurchaseOptions.CashiersCheck.ToString()) //87755
                    {
                        if (view.GetRowCellValue(view.FocusedRowHandle, "Options") == null && e.Column == this.gridColTran)
                            view.SetRowCellValue(view.FocusedRowHandle, "Options", EasyCaptureTranPurchaseOptions.CashiersCheck.ToString());
                    }
                }
                //End 87761

                //Begin Task# 90007
                if (!_tellerVars.IsAppOnline && (e.Column == this.gridColAcctNo || e.Column == this.gridColActType))
                {
                    if (view.GetRowCellValue(view.FocusedRowHandle, "AcctType") != null && view.GetRowCellValue(view.FocusedRowHandle, "AcctNo") != null)
                    {
                        tran.SelectedAcctNo = view.GetRowCellValue(view.FocusedRowHandle, "AcctNo").ToString();
                        tran.SelectedAcctType = view.GetRowCellValue(view.FocusedRowHandle, "AcctType").ToString();

                        if (!string.IsNullOrEmpty(tran.SelectedAcctType))
                        {
                            string deploan = string.Empty;
                            _parentWindow.ActionRequest("GetDepLoan", tran.SelectedAcctType, ref deploan);

                            tran.SelectedDepLoan = deploan;
                        }
                    }
                }
                //End Task# 90007
            }
            //End 87757

            RevalidateRows(); //Task #90004 
        }


        //Begin 87757
        public void SetGetAcctValue(string acctType, string acctNo)
        {
            if (!string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo))
            {
                //Begin 87751
                if (_parentWindow.TfrAcctList != null && _parentWindow.TfrAcctList.Length > 0)
                {
                    if (Array.FindIndex(_parentWindow.TfrAcctList, item => item == acctType) < 0)
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType", null);
                    else
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType", acctType);
                }
                //End 87751

                gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo", acctNo);
            }
        }

        public void SetTfrAcctTypes()
        {
            if (_parentWindow.TfrAcctList != null && _parentWindow.TfrAcctList.Length > 0)
            {
                repItemTfrAcctType.DataSource = _parentWindow.TfrAcctList;
            }

            //Disable editing 
            repItemTfrAcctType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

            //repItemOptions.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        private void GcTransactions_MouseDown(object sender, MouseEventArgs e)
        {
            string tcDescription = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription") as string;
            if (!string.IsNullOrEmpty(tcDescription) &&  //87761
                (tcDescription == "Transfer" ||
                tcDescription == "Payment" ||
                tcDescription == "External" ||
                tcDescription == "Purchase"))   //87751 #87755
            {
                // Check if the right mouse button has been pressed 
                GridHitInfo hitInfo = gvTransactions.CalcHitInfo(e.Location);
                //Begin Bug #99053
                if (e.Button == MouseButtons.Left && (hitInfo.Column != gridColMenuIndicator || gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "MenuIndicator") == null))
                    return;
                //End Bug #99053

                if (hitInfo.InRowCell)
                {
                    gvTransactions.SelectRow(hitInfo.RowHandle);
                    gvTransactions.FocusedRowHandle = hitInfo.RowHandle;
                    (e as DXMouseEventArgs).Handled = true;
                    DoShowMenu(hitInfo);
                }
            }
        }

        //Begin Bug #108094
        private void OnCalcRowHeight(object sender, RowHeightEventArgs e)
        {
            _gridRowHeight = e.RowHeight * 2;
        }
        //End Bug #108094

        void DoShowMenu(GridHitInfo hi)
        {
            // Create the menu. 
            GridViewMenu menu = null;
            // Check whether the header panel button has been clicked. 
            if (hi.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                menu = new TransactionsMenu(hi.View, hi.RowHandle, _parentWindow, this, () => {
                    SelectNextCell(gridColMenuIndicator);
                    _tranOptionSelectedForRows.Add(hi.RowHandle); //Task #115712
                });
                menu.Init(hi);

                // Display the menu. 
                //Begin 124297
                //menu.Show(hi.HitPoint);
                menu.ShowPopup(gcTransactions, hi.HitPoint);
                SendKeys.SendWait("{DOWN}");
                //End 124297
            }
        }

        //End 87757
        //Begin Bug #98671
        void DoShowMenu(int rowHandle, int visibleRowHandle = -1)
        {
            if (gvTransactions.FocusedColumn == null)
                return;
            // Create the menu. 
            GridViewMenu menu = new TransactionsMenu(gvTransactions, rowHandle, _parentWindow, this, () => {
                SelectNextCell(gridColMenuIndicator);
                _tranOptionSelectedForRows.Add(rowHandle); //Task #115712
            });
            HashSet<int> uniqueWidths = new HashSet<int>(); //Bug #99053
            int x = 0;
            int rowHeight = this._gridRowHeight;
            foreach (BandedGridColumn col in gvTransactions.Columns)
            {
                if (col.RowIndex == 0 && uniqueWidths.Add(col.VisibleWidth))
                {
                    if (col.Caption == gvTransactions.FocusedColumn.Caption)
                    {
                        x += col.VisibleWidth / 2;
                        break;
                    }
                    else
                    {
                        x += col.VisibleWidth;
                    }
                }
            }
            visibleRowHandle = visibleRowHandle >= 0 ? visibleRowHandle : rowHandle;
            var view = gvTransactions.FocusedColumn.View;
            var bounds = gvTransactions.GridControl.Bounds;
            int y = bounds.Top + ((1 + visibleRowHandle) * rowHeight) + rowHeight / 2;
            Point point = new Point(x, y);
            menu.Init(view);

            //Begin 124297
            //menu.Show(point);
            menu.ShowPopup(gcTransactions, point);
            SendKeys.SendWait("{DOWN}");
            //End 124297
        }
        //End Bug #98671

        private void Control_GotFocus(object sender, EventArgs e)
        {
            //_parentWindow.LastActiveControl = sender as Control; //87735

            _parentWindow.ChildControlFocused(sender);  //#89907

            //Begin 121714-2EFG
            if (gvTransactions.FocusedColumn == gridColTfrAcctType || gvTransactions.FocusedColumn == gridColTranDef || gvTransactions.FocusedColumn == gridColAccount) //#122532
            {
                gvTransactions.ShowEditor();
                LookUpEdit activeEditor = gvTransactions.ActiveEditor as LookUpEdit;
                if (activeEditor != null)
                {
                    activeEditor.ShowPopup();
                }
            }
            else
            {
                gvTransactions.CloseEditor();   //#122532
            }
            //End 121714-2EFG



        }

        //private void DockPanel_CustomButtonClick(object sender, ButtonEventArgs e)
        //{
        //    if (e.Button == _btnDeleteRow)
        //    {
        //        DeleteRow();
        //    }
        //    else if (e.Button == _btnAddNew)
        //    {
        //        AddNewRow();
        //    }

        //}

        private void _parentWindow_OnCustomerChanged()
        {
            //Begin 93253
            try
            {
                SetDataSource(this.repItemAccounts, _parentWindow.Accounts); //Task #114302
                SetDataSource(this.repItemTfrAccounts, _parentWindow.Accounts); //Task #114302
            }
            catch (Exception e)
            {
            }
            //End 93253

            //Begin 87751

            string[] ValidDepLoan = new string[3] { "DP", "LN", "EXT" };

            DataTable workAccts = new DataTable();
            DataTable workAccts2 = new DataTable();//108811

            //Begin 93253
            try
            {
                workAccts = _parentWindow.Accounts.Copy();

                //Begin 87753
                _DpAccts.Reset();
                _LnAccts.Reset();
                _ExAccts.Reset();
                _RmAccts.Reset();
                _AllAccts.Reset();

                _DpAccts = _parentWindow.Accounts.Clone();
                _LnAccts = _parentWindow.Accounts.Clone();
                _ExAccts = _parentWindow.Accounts.Clone();
                _RmAccts = _parentWindow.Accounts.Clone();
                _AllAccts = _parentWindow.Accounts.Clone();
                //End 87753
            }
            catch (Exception e)
            {
            }
            //End 93253

            if (workAccts != null)
            {
                if (workAccts.Rows.Count > 0)
                {
                    for (int i = 0; i < workAccts.Rows.Count; ++i)
                    {
                        DataRow row = workAccts.Rows[i];

                        //Begin 87753-2
                        if ((((string)row["DepLoan"] + "").Trim() == "EXT" && ((string)row["Adapter"] + "").Trim() != "Y") ||    //External accounts with adapter = N
                            (Array.FindIndex(ValidDepLoan, item => item == ((string)row["DepLoan"] + "").Trim()) < 0) ||    //Accounts not valid to post transactions 
                            (((string)row["DepLoan"] + "").Trim() == "DP" && ((string)row["ApplType"] + "").Trim() == "CD"))     //95598: Exclude CDs
                        {
                            workAccts.Rows.Remove(row);
                            --i; //Bug #115163
                            continue;
                        }

                        if (((string)row["DepLoan"] + "").Trim() == "DP")
                        {
                            _DpAccts.ImportRow(row);
                            _AllAccts.ImportRow(row);
                        }
                        else if (((string)row["DepLoan"] + "").Trim() == "LN")
                        {
                            _LnAccts.ImportRow(row);
                            _AllAccts.ImportRow(row);
                        }
                        else if (((string)row["DepLoan"] + "").Trim() == "EXT")
                        {
                            _ExAccts.ImportRow(row);
                            _AllAccts.ImportRow(row);
                        }

                        //End 87753-2
                    }
                }
                else
                {
                    workAccts.Reset();
                    workAccts.Columns.Add("AcctNo");
                    workAccts.Columns.Add("AcctType");
                    workAccts.Columns.Add("AccountCombined");
                    workAccts.Columns.Add("DepLoan");
                    workAccts.Columns.Add("Status");
                    //workAccts.Columns.Add("CurBalanceAmt");
                }

                workAccts2 = workAccts.Copy();  //108811: Exclude the RIM record from the Transfer Account Type Lookup combo box

                //Begin 87753
                DataRow RimAcct = workAccts.NewRow();
                RimAcct["AcctNo"] = rimNo;
                RimAcct["AcctType"] = "RIM";
                RimAcct["AccountCombined"] = string.Format("{0} - {1}", "RIM", rimNo);
                RimAcct["DepLoan"] = "RM";
                RimAcct["Status"] = rimStatus;
                //RimAcct["CurBalanceAmt"] = 0.00;
                workAccts.Rows.Add(RimAcct);
                //End 87753

                //Begin 87753-2
                if (workAccts.Rows.Count > 0)
                {
                    /*_DpAccts.ImportRow(RimAcct);
                    _LnAccts.ImportRow(RimAcct);
                    _ExAccts.ImportRow(RimAcct);*/ //Task #114302
                    _RmAccts.ImportRow(RimAcct);
                    _AllAccts.ImportRow(RimAcct);
                }
                //End 87753-2

                //Begin 93253
                try
                {
                    SetDataSource(this.repItemAccounts, workAccts); //Task #114302
                    SetDataSource(this.repItemTfrAccounts, workAccts2); //108811  //Bug #99051 //Task #114302
                }
                catch (Exception e)
                {
                }
                //End 93253    
            }

            //End 87751

            _parentWindow.HandleTCDDrawerIcon();    //#89998  
            _parentWindow.HandleWaiveSignature();   //#90021
        }

        //Begin Task #115712
        public void ClearTranMenuCachedInfo()
        {
            _tranOptionSelectedForRows.Clear();
        }
        //End Task #115712


        ////Hooked to the context menu click in the Accounts grid via the main form
        //public void AddNewTran(EasyCaptureTran tran)
        //{
        //    return;
        //}

        //This fires when combobox is opened.  Can be used to populate values in the dropdown

        private void gvTransactions_ShownEditor(object sender, EventArgs e)
        {
            DataRow currTranRow = gvTransactions.GetFocusedDataRow();
            //gridView2.SetRowCellValue(gridView2.GetSelectedRows()[0], "gridColAccount", currTranRow.Field());

            gvTransactions.ActiveEditor.MouseDown -= ActiveEditor_MouseDown;    //Vidya
            gvTransactions.ActiveEditor.MouseDown += ActiveEditor_MouseDown;    //Vidya
        }

        //Begin Vidya
        //private void gvTransactions_GotFocus(object sender, EventArgs e)
        //{
        //    /*This GotFocus code will prevent the editform dialog from being disposed using ESC key.  For the grid to work properly by getting edit focus on each cell, 
        //    the editform must be closed using the CANCEL button in the editform*/

        //    SendKeys.Send("{F2}");
        //    SendKeys.Flush();
        //}

        private void ActiveEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                InvokeEditRowEditor(e);

                //Do Not reset to inplace editing here but do it in UPDATE or CANCEL so the inplae editing will work normally else inplace will NOT work after the editform is disposed.
                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            }
        }

        private void gvTransactions_EditFormShowing(object sender, EditFormShowingEventArgs e)
        {
            //EasyCaptureTran ctran = (EasyCaptureTran)gvTransactions.GetRow(e.RowHandle);
            //ucTranEdit ucEditForm = new ucTranEdit(ctran);
            // gvTransactions.OptionsEditForm.CustomEditFormLayout = ucEditForm;
        }

        private void gvTransactions_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {
            //e.Panel.Controls[1].Controls[1].Click += UcAccounts_EditForm_Update_Click;
            //e.Panel.Controls[1].Controls[2].Click += UcAccounts_EditForm_Cancel_Click;

            MethodInfo method = typeof(GridView).GetMethod("EnsureEditFormController", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(gvTransactions, null);
            PropertyInfo property = typeof(GridView).GetProperty("EditFormController", BindingFlags.NonPublic | BindingFlags.Instance);
            EditFormController controller = property.GetValue(gvTransactions, null) as EditFormController;
            controller.CloseEditForm += new EventHandler(Controller_CloseEditForm);
        }
        private void Controller_CloseEditForm(object sender, EventArgs e)
        {
            EditFormController controller = (EditFormController)sender;
            controller.CloseEditForm -= Controller_CloseEditForm;
            gvTransactions.OptionsBehavior.EditingMode = GridEditingMode.Default;
        }

        //private void UcAccounts_EditForm_Update_Click(object sender, EventArgs e)
        //{
        //    gvTransactions.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
        //}

        //private void UcAccounts_EditForm_Cancel_Click(object sender, EventArgs e)
        //{
        //    gvTransactions.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
        //}

        private void InvokeEditRowEditor(MouseEventArgs e)
        {
            gvTransactions.PostEditor();
            gvTransactions.UpdateCurrentRow();
            gvTransactions.OptionsBehavior.EditingMode = GridEditingMode.Default;   //124209: Was GridEditingMode.EditFormInplace
            if (e != null) DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            gvTransactions.ShowEditForm();
            //gvTransactions.OptionsBehavior.EditingMode = GridEditingMode.Default;     //TODO need to chang eto EditFormInPlace once we figure out the row focus issue with ESC key

            return;
        }
        //End Vidya


        //Begin 87753
        private void RepItemAccounts_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription") == null ||
                (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString() != "CashCheck" &&
                 gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString() != "Purchase"))
            {
                if (e != null && e.NewValue.ToString().Length >= 3 && e.NewValue.ToString().Substring(0, 3) == "RIM")
                {
                    e.Cancel = true;
                }
            }
        }

        //End 87753

        //Fires when a row in dropdown lookup is seleted
        private void repItemAccounts_EditValueChanged(object sender, EventArgs e)
        {
            //Begin 87761
            string currOption = string.Empty;

            if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty)
            {
                currOption = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString();
            }
            //End 87761

            LookUpEdit edit = sender as LookUpEdit;
            if (edit != null)
            {
                var test = edit.EditValue;
                DataRowView row = edit.GetSelectedDataRow() as DataRowView;
                if (row != null && row.Row != null)
                {
                    string acctType = row.Row.GetStringValue("AcctType").Trim();
                    string acctNo = row.Row.GetStringValue("AcctNo").Trim();
                    string dpLN = row.Row.GetStringValue("DepLoan").Trim(); //Bug #95593
                    EasyCaptureTran rowValue = GetSelectedRow();
                    if (rowValue != null)
                    {
                        rowValue.AcctNo = row.Row.GetStringValue("AcctNo").Trim();
                        rowValue.AcctType = row.Row.GetStringValue("AcctType").Trim();

                        //Begin 87751
                        //Begin 87753-2
                        if (rowValue.SelectedAcctNo != null && rowValue.SelectedAcctNo != string.Empty && rowValue.SelectedAcctNo != "AddNew")
                        {
                            rowValue.SelectedAcctNo = row.Row.GetStringValue("AcctNo").Trim();
                            rowValue.SelectedAcctType = row.Row.GetStringValue("AcctType").Trim();
                            rowValue.SelectedDepLoan = ((string)row.Row["DepLoan"] + "").Trim();
                        }
                        //End 87753-2
                        //End 87751

                        //Begin 93993
                        if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription") != null &&
                            gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString() != string.Empty &&
                            gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString() == "Deposit"
                            )
                        {
                            if (row.Row.GetStringValue("Status") == "Unfunded")
                                rowValue.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.OpeningDeposit.ToString());
                            else
                                rowValue.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.Deposit.ToString());
                        }
                        //End 93993

                        if (rowValue.AcctType != "RIM") //87753
                        {
                            if (rowValue.TranType == EasyCaptureTranType.Transfer)
                            {
                                _tranAlerts.ShowFromToAlerts(rowValue.AcctType, rowValue.AcctNo, rowValue.TfrAcctType, rowValue.TfrAcctNo); //Task 89995
                            }
                            else
                            {
                                _tranAlerts.ShowAlerts(rowValue.AcctType, rowValue.AcctNo); //Task 89995
                            }
                        }
                    }
                }

                //Begin 121714-2EFG
                if (_autoTranTypeSelect)
                    _autoTranTypeSelect = false;
                else
                    SelectNextCell(); //Task #114936
                //End 121714-2EFG
            }

            SetOptionType(currOption); //87761
        }


        //Begin Bug #99051
        private void repItemTfrAccounts_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit edit = sender as LookUpEdit;
            if (edit != null)
            {
                DataRowView row = edit.GetSelectedDataRow() as DataRowView;
                if (row != null && row.Row != null)
                {
                    string tfrAcctType = row.Row.ItemArray[2].ToString().Trim();
                    string tfrAcctNo = row.Row.ItemArray[3].ToString().Trim();
                    if (!string.IsNullOrEmpty(tfrAcctType) && !string.IsNullOrEmpty(tfrAcctNo))
                    {
                        EasyCaptureTran rowValue = GetSelectedRow();
                        rowValue.TfrAcctType = tfrAcctType;
                        rowValue.TfrAcctNo = tfrAcctNo;
                    }
                }

                //Begin 121714-2EFG
                if (_autoTranTypeSelect)
                    _autoTranTypeSelect = false;
                else
                    SelectNextCell(); //Task #114936
                //End 121714-2EFG
            }
        }
        //End Bug #99051

        public string GetGridFocusedColumn()
        {
            string focusedColumn = "";

            if (gvTransactions != null && gvTransactions.FocusedColumn != null)
            {
                focusedColumn = gvTransactions.FocusedColumn.Name;
            }

            return focusedColumn;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool keyHandled = false; //Bug #101592
            switch (keyData)
            {
                //Begin Bug #102499
                case (Keys.Apps):
                    if (gvTransactions.IsFocusedView && gvTransactions.RowCount > 0)
                    {
                        int index = gvTransactions.GetVisibleRowHandle(gvTransactions.FocusedRowHandle);
                        DoShowMenu(index);
                    }
                    break;
                //End Bug #102499
                case (Keys.Control | Keys.Add):

                case Keys.Control | Keys.Oemplus:
                    InvokeEditRowEditor(null);
                    break;
                case (Keys.Control | Keys.F):
                    keyHandled = true; //Bug #101592
                    break;
                case (Keys.Insert | Keys.Control):
                    if (_btnAddNew.Enabled)
                        ButtonAddNewClick(); //Bug #115881
                    break;

                case (Keys.Delete | Keys.Control):
                    DeleteRow();
                    break;
                case (Keys.Control | Keys.M): //Bug #98671
                    if (gvTransactions.FocusedRowHandle >= 0)
                    {
                        int index = gvTransactions.GetVisibleRowHandle(gvTransactions.FocusedRowHandle);
                        int firstVisibleRow = 0;
                        bool rowVisible = false;
                        while (!rowVisible && firstVisibleRow < gvTransactions.RowCount)
                        {
                            var rowVisibleState = gvTransactions.IsRowVisible(firstVisibleRow);
                            rowVisible = rowVisibleState == RowVisibleState.Partially || rowVisibleState == RowVisibleState.Visible;
                            if (!rowVisible)
                                firstVisibleRow++;
                        }
                        int gridRowIndex = Math.Max(0, gvTransactions.FocusedRowHandle - firstVisibleRow);
                        DoShowMenu(index, gridRowIndex);
                        keyHandled = true; //Task #105856
                    }
                    break;
                //Begin Task #114936
                case (Keys.Tab):
                    if (gvTransactions.FocusedRowHandle < 0 || gvTransactions.FocusedRowHandle >= _parentWindow.TransactionSet.Transactions.Count)
                        break;

                    var tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];

                    if (gvTransactions.FocusedColumn == gridColAmount && tran.TranType == EasyCaptureTranType.Transfer)
                    {
                        SelectNextCell();
                        keyHandled = true;
                    }
                    break;
                //End Task #114936
                //Begin Task #116300
                case (Keys.Space):
                    if (DoesRowContainsTranMenu(gvTransactions.FocusedRowHandle) && gvTransactions.FocusedColumn == gridColMenuIndicator)
                    {
                        DoShowMenu(gvTransactions.FocusedRowHandle);
                        keyHandled = true;

                        //SendKeys.SendWait("{DOWN}");  //122355-#3 //124297
                    }
                    break;
                //End Task #116300
                //Begin Task #124210
                case (Keys.Enter):
                    if (gvTransactions.IsFocusedView && gvTransactions.RowCount > 0)
                    {
                        if (gvTransactions.FocusedColumn != null)
                        {
                            GridView view = gvTransactions as GridView;
                            if (view.IsEditing && _popupFocused)
                                keyHandled = true;
                            else
                            {
                                //#124249
                                EasyCaptureTran selectedTranRow = GetSelectedRow();
                                if (selectedTranRow != null && selectedTranRow.IsValidated == false && string.IsNullOrEmpty(selectedTranRow.SelectedAcctNo))
                                {
                                    DeleteRow();
                                    if (gvTransactions.RowCount == 0) //Remove the last displayed alerts
                                    {
                                        HideAlerts();
                                    }
                                    else if (gvTransactions.RowCount >= 1) //Refresh the alerts for the account in the focused row
                                    {
                                        gvTransactions.FocusedColumn = null;    //121715
                                        gvTransactions_FocusedRowChanged(null, null);
                                    }
                                }
                                keyHandled = false;
                            }
                        }
                    }
                    else
                    {
                        //if (_tellerVars.IsAppOnline)  //#124403 - Commmented out
                        _parentWindow.SetTransactionAccountInfo(null, null, true);
                        keyHandled = true;
                    }
                    break;
                    //End Task #124210
            }

            //Do NOT jump to next panel as TAB needs to focus on next cell
            //((PDxDockPanel)(this._dockPanel)).OnPanelKeyDown(new KeyEventArgs(keyData));
            if (keyHandled)
            {
                base.ProcessCmdKey(ref msg, keyData); //Bug #101592
                return keyHandled; //Bug #101592
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        private EasyCaptureTran GetSelectedRow()
        {
            if (gvTransactions.SelectedRowsCount == 1)
            {
                return gvTransactions.GetRow(gvTransactions.GetSelectedRows()[0]) as EasyCaptureTran;
            }
            return null;
        }

        //Begin 87753-2



        private void RepItemAccounts_Enter(object sender, EventArgs e)
        {
            EasyCaptureTran selectedTranRow = GetSelectedRow();

            DevExpress.XtraEditors.LookUpEdit AcctLookupEdit = sender as DevExpress.XtraEditors.LookUpEdit;

            if (selectedTranRow != null && selectedTranRow.TranDef != null && _parentWindow.Accounts != null && _parentWindow.Accounts.Rows.Count > 0)
            {
                if (selectedTranRow.TranDef.DpLN == "DP")
                    SetDataSource(AcctLookupEdit, _DpAccts); //Task #114302
                else if (selectedTranRow.TranDef.DpLN == "LN")
                    SetDataSource(AcctLookupEdit, _LnAccts); //Task #114302
                else if (selectedTranRow.TranDef.DpLN == "EX")
                    SetDataSource(AcctLookupEdit, _ExAccts); //Task #114302
                else
                    SetDataSource(AcctLookupEdit, _RmAccts); //Task #114302
            }
            else
            {
                if (_parentWindow.Accounts != null && _parentWindow.Accounts.Rows.Count > 0)
                    SetDataSource(AcctLookupEdit, _AllAccts); //Task #114302
                else
                    SetDataSource(AcctLookupEdit, _RmAccts); //Task #114302
            }
        }
        //End 87753-2

        //Begin 87735

        private void RepositoryItemTC_Enter(object sender, System.EventArgs e)
        {
            EasyCaptureTran selectedTranRow = GetSelectedRow();

            DevExpress.XtraEditors.LookUpEdit TcLookupEdit = sender as DevExpress.XtraEditors.LookUpEdit;

            //Begin 87751: Cleaned up the code and added condition for external accounts

            List<TransactionTypeDefinition> _WorkExtTCDefList = new List<TransactionTypeDefinition>(_ExtTCDefList);

            DataTable workAccts = new DataTable();

            //Begin 87753
            if (_parentWindow.Accounts != null && _tellerVars.IsAppOnline)  //#124403 - Added Offline Check
            {
                workAccts = _parentWindow.Accounts.Copy();
            }
            //End 87753

            Boolean CDAcct = false; //95598

            if (selectedTranRow != null && selectedTranRow.TranDef != null && workAccts != null && workAccts.Rows.Count > 0) //87753
            {
                //DataTable rows = ((System.Data.DataTable)repItemAccounts.DataSource);
                string expression = "AcctNo = '" + selectedTranRow.SelectedAcctNo + "' and " + "AcctType = '" + selectedTranRow.SelectedAcctType + "'";
                DataRow[] RowMatch = workAccts.Select(expression);

                if (RowMatch.Length > 0 && RowMatch[0].GetStringValue("DepLoan") == "EXT")
                {
                    if (RowMatch[0].GetStringValue("Adapter") != "Y")
                    {
                        TransactionTypeDefinition a = _WorkExtTCDefList.Find(x => x.TranType == EasyCaptureTranType.External.ToString());

                        _WorkExtTCDefList.Remove(_WorkExtTCDefList[_WorkExtTCDefList.IndexOf(a)]);
                    }
                }

                //Begin 95598
                if (selectedTranRow.SelectedDepLoan == "DP" && RowMatch.Length > 0 && RowMatch[0].GetStringValue("ApplType").Trim() == "CD")
                {
                    CDAcct = true;
                }
                //End 95598

                //Begin 87753-2
                //Assign the appropriate transaction definition list to the datasource based on the default Tran Type              
                if (selectedTranRow.SelectedAcctNo == null || selectedTranRow.SelectedAcctNo == string.Empty || selectedTranRow.SelectedAcctNo == "AddNew")
                {
                    TcLookupEdit.Properties.DataSource = _AllTCDefList;

                    if (selectedTranRow.SelectedAcctNo == null || selectedTranRow.SelectedAcctNo == string.Empty)
                        selectedTranRow.SelectedAcctNo = "AddNew";
                }
                else
                {
                    if (selectedTranRow.SelectedDepLoan == "DP" && CDAcct == false) //95598
                        TcLookupEdit.Properties.DataSource = _DpTCDefList;          //87759  //_WorkDpTCDefList; //_DpTCDefList;
                    else if (selectedTranRow.SelectedDepLoan == "LN")
                        TcLookupEdit.Properties.DataSource = _LnTCDefList;
                    //else if (selectedTranRow.TranDef.DpLN == "RM")
                    //    TcLookupEdit.Properties.DataSource = _RmTCDefList;
                    else if (selectedTranRow.SelectedDepLoan == "EXT")
                        TcLookupEdit.Properties.DataSource = _WorkExtTCDefList;
                    else
                        TcLookupEdit.Properties.DataSource = _RmTCDefList;
                }
                //End 87753-2
            }
            else
            {
                //Begin 87753
                if (workAccts != null && workAccts.Rows.Count > 0 || !_tellerVars.IsAppOnline) //Task#90007
                    TcLookupEdit.Properties.DataSource = _AllTCDefList;
                else
                    TcLookupEdit.Properties.DataSource = _RmTCDefList;
                //End 87753
            }

            //End 87751
        }

        //End 87735

        private void repItemTC_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit edit = sender as LookUpEdit;

            if (edit != null)
            {
                TransactionTypeDefinition selectedTranDefRow = edit.GetSelectedDataRow() as TransactionTypeDefinition; //87761

                SetMenuIndicatorColumn(gvTransactions.FocusedRowHandle); //Bug #99053
                EasyCaptureTran selectedTranRow = GetSelectedRow();

                //Begin 121714-2EFG
                if (selectedTranDefRow == null)
                    return;
                //End 121714-2EFG

                if (selectedTranRow != null)
                {
                    selectedTranRow.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == selectedTranDefRow.Description);           //87761
                    selectedTranRow.TranType = TranHelper.Instance.TranList.Find(x => x.Description == selectedTranDefRow.Description).TranType; //87761
                    //Begin Bug#103647
                    if (!_tellerVars.IsAppOnline && selectedTranRow.TranType == EasyCaptureTranType.Deposit)
                    {
                        selectedTranRow.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.Deposit.ToString());
                    }

                    //End Bug#103647
                }

                if (selectedTranRow != null && selectedTranRow.TcDescription != null && selectedTranDefRow != null && !string.IsNullOrEmpty(selectedTranDefRow.TranCode)) //87761 //94126
                {
                    _parentWindow.SetCurrentTran(selectedTranRow.TcDescription, selectedTranRow.TranDef.TranCode);
                    _parentWindow.PerformEnableDisableControls("TranCodeSelect");

                    //Begin Bug #108662
                    if (selectedTranRow.TranDef.DpLN != "RM")
                    {
                        ShowAlerts(selectedTranRow.AcctType, selectedTranRow.AcctNo, selectedTranRow.TfrAcctType, selectedTranRow.TfrAcctNo);
                    }
                    else
                    {
                        HideAlerts();
                    }
                    //End Bug #108662
                }

                SetOptionType(null); //87761 
                SetMenuIndicatorColumn(gvTransactions.FocusedRowHandle); //Bug #107400

                //Begin 121714-2EFG
                if (_autoTranTypeSelect)
                    _autoTranTypeSelect = false;
                else
                    SelectNextCell();//Task #114936                                  
                //End 121714-2EFG
            }
        }

        //Begin Task #114936
        private void repItemTfrAcctType_EditValueChanged(object sender, EventArgs e)
        {
            //Begin 121714-2EFG
            if (_autoTranTypeSelect)
                _autoTranTypeSelect = false;
            else
                SelectNextCell();
            //End 121714-2EFG

            gvTransactions.ShowEditor(); //122355-#4
        }

        void SelectNextCell(GridColumn fromColumn = null)
        {
            GridColumn currentColumn = fromColumn ?? gvTransactions.FocusedColumn;
            GridColumn nextColumn = null;

            bool currentColIsNextCol = false;

            if (currentColumn == gridColMenuIndicator)
            {
                nextColumn = gridColAccount;
            }
            else
            {
                foreach (GridColumn col in gvTransactions.Columns)
                {
                    if (currentColIsNextCol)
                    {
                        nextColumn = col;
                        if (nextColumn == gridColMenuIndicator)
                        {
                            Image cellValue = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "MenuIndicator") as Image;
                            if (cellValue != null)
                                break;
                        }
                        else
                            break;
                    }

                    if (col == currentColumn)
                    {
                        currentColIsNextCol = true;
                    }
                }
            }

            if (nextColumn != null)
            {
                gvTransactions.FocusedColumn = nextColumn;
                if (nextColumn == gridColMenuIndicator)
                {
                    DoShowMenu(gvTransactions.FocusedRowHandle);
                }
                else if (nextColumn.ColumnEdit != null)
                {
                    gvTransactions.ShowEditor();

                    //Begin 121714-2EFG: The popup is carried out in the OnShowEditor method
                    /*
                    LookUpEdit activeEditor = gvTransactions.ActiveEditor as LookUpEdit;
                    if (activeEditor != null)
                    {
                        activeEditor.ShowPopup();
                    }
                    */
                    //End 121714-2EFG
                }

                else if (nextColumn == gridColAmount)
                {
                    //gvTransactions.ShowEditor();    //121714-Bug-fix
                    //SendKeys.Send(" ");             //121713-#7:Bug-fix
                }
            }
        }
        //End Task #114936

        //Begin 87761
        public void SetOptionType(string CurrentOption)
        {
            if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription") != null)
            {
                string TcDesc = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "TcDescription").ToString();

                if (TcDesc != "Transfer")
                {
                    //Begin 121714-Bug-fix
                    /*
                    //gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Amount", null);   
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAmount", null);
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctNo", null);
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "TfrAcctType", null);
                    //gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "RegD", false);    //95598 
                    */
                    //End 121714-Bug-fix

                    ResetTfrFlds();

                    gridColOffsetAccount.OptionsColumn.ReadOnly = true;     //121714-1AB&2A
                    //gridColOffsetAccount.OptionsColumn.AllowEdit = false; //121714-1AB&2A
                    gridColOffsetAccount.OptionsColumn.TabStop = false;

                    repItemTfrAcctType.ReadOnly = true;
                    repItemTfrAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.False;
                    gridColTfrAcctType.OptionsColumn.TabStop = false;

                    repositoryItemRegD.ReadOnly = true;
                    repositoryItemRegD.HotTrackWhenReadOnly = false;
                    gridColRegD.OptionsColumn.TabStop = false;
                }
                else
                {
                    gridColOffsetAccount.OptionsColumn.ReadOnly = false;
                    //gridColOffsetAccount.OptionsColumn.AllowEdit = true;  //121714-1AB&2A
                    gridColOffsetAccount.OptionsColumn.TabStop = true;

                    repItemTfrAcctType.ReadOnly = false;
                    repItemTfrAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.True;
                    gridColTfrAcctType.OptionsColumn.TabStop = true;

                    repositoryItemRegD.ReadOnly = false;
                    repositoryItemRegD.HotTrackWhenReadOnly = true;
                    gridColRegD.OptionsColumn.TabStop = true;
                }

                if (TcDesc != "Transfer" && TcDesc != "Payment" && TcDesc != "External" && TcDesc != "Purchase") //87751 #87755
                {
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", null);
                }

                if ((TcDesc == "Transfer" || TcDesc == "Payment" || TcDesc == "External" || TcDesc == "Purchase") && //87751 #87755
                    (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") == null ||
                     gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() == string.Empty))
                {
                    //Begin 87751: Cleaned up & added condition for external

                    string option = string.Empty;

                    if (TcDesc == "Transfer")
                        option = EasyCaptureTranTfrOptions.TransferWithdrawal.ToString();
                    else if (TcDesc == "Payment")
                        option = EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString();
                    else if (TcDesc == "External")
                        option = EasyCaptureTranExtOptions.ExternalCredit.ToString();
                    else if (TcDesc == "Purchase")
                        option = EasyCaptureTranPurchaseOptions.CashiersCheck.ToString();

                    //End 87751

                    if (CurrentOption != null && CurrentOption != string.Empty)
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", CurrentOption);
                    else
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", option);
                }

                //Begin 87753 

                if (TcDesc == "CashCheck" || TcDesc == "Purchase")
                {
                    string opt = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") as string;
                    EasyCaptureTran selectedTranRow = GetSelectedRow();

                    //Begin 94134
                    if (selectedTranRow != null)
                    {
                        selectedTranRow.AcctNo = rimNo;
                        selectedTranRow.AcctType = "RIM";
                        selectedTranRow.Options = opt;
                    }
                    //End 94134

                    string CurRimNo = string.Format("{0} - {1}", "RIM", rimNo);
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Account", CurRimNo);

                    gridColAccount.OptionsColumn.AllowEdit = false;
                    gridColAccount.OptionsColumn.TabStop = false;
                }
                else
                {
                    if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "AccountCombined") != null &&
                        gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "AccountCombined").ToString().Substring(0, 3) == "RIM")
                    {
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "AccountCombined", null);
                    }

                    gridColAccount.OptionsColumn.AllowEdit = true;
                    gridColAccount.OptionsColumn.TabStop = true;
                }

                //End 87753 

                /* Begin 95598 */

                string DescValue = string.Empty;
                string DescEnable = "N";
                string DescCheck = "N";
                string enableCheckNo = "N"; //#124295

                var tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];

                if (tran != null && tran.TranDef != null)
                {
                    _parentWindow.ActionRequest("GetDescValue", tran.TranDef.TranCode, ref DescValue);

                    if (DescValue != string.Empty)
                    {
                        DescEnable = DescValue.Split("~".ToCharArray())[0];
                        DescCheck = DescValue.Split("~".ToCharArray())[1];

                        //Begin 122493
                        if (_newTranRecordMode)
                            gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "PassDescriptionToHistory", (DescCheck == "Y"));
                        else
                            gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "PassDescriptionToHistory", (tran.PassDescriptionToHistory));
                        //End 122493
                    }

                    /* Begin 104267 */
                    string EnableDesc = "N";
                    string EnableRef = "N";

                    _parentWindow.ActionRequest("GetEnableDescRefFlags", tran.TranDef.TranCode, ref DescValue);

                    if (DescValue != string.Empty)
                    {
                        EnableDesc = DescValue.Split("~".ToCharArray())[0];
                        EnableRef = DescValue.Split("~".ToCharArray())[1];

                        //Enable/disable description columns
                        gridColDesc.OptionsColumn.ReadOnly = (EnableDesc != "Y");
                        //gridColDesc.OptionsColumn.AllowEdit = (EnableDesc == "Y"); //121714-1AB&2A
                        gridColDesc.OptionsColumn.TabStop = (EnableDesc == "Y");

                        repositoryItemPassToHistory.ReadOnly = (EnableDesc != "Y");
                        repositoryItemRegD.HotTrackWhenReadOnly = (EnableDesc == "Y");
                        gridColPassToHistory.OptionsColumn.TabStop = (EnableDesc == "Y");

                        //Enable/disable reference column
                        gridColRefField.OptionsColumn.ReadOnly = (EnableRef != "Y");
                        //gridColRefField.OptionsColumn.AllowEdit = (EnableRef == "Y"); //121714-1AB&2A
                        gridColRefField.OptionsColumn.TabStop = (EnableRef == "Y");
                    }
                    /* End 104267 */

                    //Begin 122493
                    string regDEnable = "N", regDCheck = "N";

                    if (tran.TranDef.DpLN == "DP")
                    {
                        string regDValue = string.Empty;

                        //Begin 122798
                        DataTable workAccts = new DataTable();
                        if (_parentWindow.Accounts != null && _tellerVars.IsAppOnline)  //#124403 - Added Offline Check
                        {
                            workAccts = _parentWindow.Accounts.Copy();
                        }
                        //End 122798

                        if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                            gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty &&
                            tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferDeposit.ToString()
                            )
                        {
                            if ((tran.TfrAcctNo != null && tran.TfrAcctNo.ToString() != string.Empty) &&
                               (tran.TfrAcctType != null && tran.TfrAcctType.ToString() != string.Empty))
                            {
                                //Begin 122798
                                //regDValue = tran.TfrAcctType.Trim() + "~" + tran.TfrAcctNo.Trim(); 
                                if (workAccts != null && workAccts.Rows.Count > 0)  //#124403 - Added for Offline
                                {
                                    string expression = "AcctNo = '" + tran.TfrAcctNo.Trim() + "' and " + "AcctType = '" + tran.TfrAcctType.Trim() + "'";
                                    DataRow[] RowMatch = workAccts.Select(expression);
                                    if (RowMatch.Length > 0)
                                    {
                                        regDValue = RowMatch[0].GetStringValue("IsRegDAccount").Trim();
                                    }
                                    //End 122798
                                }

                                _parentWindow.ActionRequest("GetTC102RegDValue", tran.TranDef.TranCode, ref regDValue);
                            }
                        }
                        else if (tran.TranDef.Description == EasyCaptureTranCodeDesc.Withdrawal.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.CashedCheck.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferWithdrawal.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.AutoLoanPayment.ToString()
                                )
                        {
                            if ((tran.AcctNo != null && tran.AcctNo.ToString() != string.Empty) &&
                                 (tran.AcctType != null && tran.AcctType.ToString() != string.Empty))
                            {
                                //Begin 122798
                                //regDValue = tran.AcctType.Trim() + "~" + tran.AcctNo.Trim();  
                                if (workAccts != null && workAccts.Rows.Count > 0)  //#124403 - Added for Offline
                                {
                                    string expression = "AcctNo = '" + tran.AcctNo.Trim() + "' and " + "AcctType = '" + tran.AcctType.Trim() + "'";
                                    DataRow[] RowMatch = workAccts.Select(expression);
                                    if (RowMatch.Length > 0)
                                    {
                                        regDValue = RowMatch[0].GetStringValue("IsRegDAccount").Trim();
                                    }
                                    //End 122798
                                }

                                _parentWindow.ActionRequest("GetTC102RegDValue", tran.TranDef.TranCode, ref regDValue);
                            }
                        }

                        if (regDValue != string.Empty)
                        {
                            regDEnable = regDValue.Split("~".ToCharArray())[0];
                            regDCheck = regDValue.Split("~".ToCharArray())[1];
                        }

                        //#124295
                        if (tran.TranDef.Description == EasyCaptureTranCodeDesc.Deposit.ToString())
                        {
                            if (_parentWindow.Accounts != null)
                            {
                                string expression = "AcctNo = '" + tran.AcctNo.Trim() + "' and " + "AcctType = '" + tran.AcctType.Trim() + "'";
                                if (workAccts != null && workAccts.Rows.Count > 0)
                                {
                                    DataRow[] RowMatch = workAccts.Select(expression);
                                    if (RowMatch.Length > 0)
                                    {
                                        enableCheckNo = RowMatch[0].GetStringValue("IsReconAccount").Trim();
                                    }
                                }
                            }
                        }
                    }

                    if (_newTranRecordMode)
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "RegD", (regDCheck == "Y"));
                    else
                        gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "RegD", (tran.RegD));

                    repositoryItemRegD.ReadOnly = (regDEnable != "Y");
                    repositoryItemRegD.HotTrackWhenReadOnly = (regDEnable == "Y");
                    gridColRegD.OptionsColumn.TabStop = (regDEnable == "Y");
                    //End 122493

                    #region Enable/Disable Check#
                    _parentWindow.ActionRequest("GetEnableCheckNoFlags", tran.TranDef.TranCode, ref enableCheckNo);
                    enableCheckNo = enableCheckNo.Split("~".ToCharArray())[0];
                    gridColCheckNo.OptionsColumn.ReadOnly = (enableCheckNo == "N");
                    gridColCheckNo.OptionsColumn.TabStop = (!gridColCheckNo.OptionsColumn.ReadOnly);
                    #endregion
                }

                /* End 95598 */

                //Begin Task#90007
                if (!_tellerVars.IsAppOnline)
                {
                    repItemAcctType.ReadOnly = false;
                    repItemAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.True;
                    gridColActType.OptionsColumn.TabStop = true;
                }
                //End Task#90007

                //Begin 121714-1AB&2A
                if (DoesRowContainsTranMenu(gvTransactions.FocusedRowHandle))
                {
                    gridColMenuIndicator.OptionsColumn.ReadOnly = false;
                    gridColMenuIndicator.OptionsColumn.TabStop = true;
                }
                else
                {
                    gridColMenuIndicator.OptionsColumn.ReadOnly = true;
                    gridColMenuIndicator.OptionsColumn.TabStop = false;
                }

                EasyCaptureTran FocusedTranRow2 = GetSelectedRow();

                if (!(FocusedTranRow2.TranDef.DpLN == "DP" && FocusedTranRow2.TranDef.TranType != EasyCaptureTranType.Transfer))
                {
                    //Disable and tab pass the Check, charges & hold grid columns
                    gridColCcAmt.OptionsColumn.ReadOnly = true;
                    gridColCcAmt.OptionsColumn.TabStop = false;

                    //#124295
                    //gridColCheckNo.OptionsColumn.ReadOnly = true;
                    //gridColCheckNo.OptionsColumn.TabStop = false;

                    gridColHoldBal.OptionsColumn.ReadOnly = true;
                    gridColHoldBal.OptionsColumn.TabStop = false;
                    repositoryItemHyperLinkEdit1.ReadOnly = true;
                }
                else
                {
                    //Might need to check when to enable the Charge amt column
                    gridColCcAmt.OptionsColumn.ReadOnly = true; //124214: was false
                    gridColCcAmt.OptionsColumn.TabStop = false;  //#124403 - was true

                    //#124295
                    //Might need to check when to enable the checkNo column
                    //gridColCheckNo.OptionsColumn.ReadOnly = false;
                    //gridColCheckNo.OptionsColumn.TabStop = true;

                    gridColHoldBal.OptionsColumn.ReadOnly = false;
                    gridColHoldBal.OptionsColumn.TabStop = true;
                    repositoryItemHyperLinkEdit1.ReadOnly = false;
                }

                if (gvTransactions.FocusedColumn == gridColAmount)
                {
                    if (FocusedTranRow2.TranDef.TranType == EasyCaptureTranType.Payment)
                    {
                        SendKeys.SendWait("{BREAK}");
                        gvTransactions.ShowEditor();
                    }
                    else
                        gvTransactions.ShowEditor();
                }

                if (gvTransactions.FocusedColumn == gridColOffsetAccount)
                {
                    gvTransactions.ShowEditor();
                }
                //End 121714-1AB&2A
            }
            //Begin 87753 
            else
            {
                //Begin Task#90007
                if (_tellerVars.IsAppOnline)
                {
                    gridColAccount.OptionsColumn.AllowEdit = true;
                    gridColAccount.OptionsColumn.TabStop = true;
                }
                else
                {
                    //#124403
                    gridColOffsetAccount.OptionsColumn.AllowEdit = true;    //was set to false
                    gridColOffsetAccount.OptionsColumn.TabStop = true;      //was set to false

                    repItemTfrAcctType.ReadOnly = true;
                    repItemTfrAcctType.AllowDropDownWhenReadOnly = DefaultBoolean.False;
                    gridColTfrAcctType.OptionsColumn.TabStop = false;

                    repositoryItemRegD.ReadOnly = true;
                    repositoryItemRegD.HotTrackWhenReadOnly = false;
                    gridColRegD.OptionsColumn.TabStop = false;

                }
                //End Task#90007
            }
            //End 87753 
        }

        //End 87761

        private void gvTransactions_InitNewRow(object sender, InitNewRowEventArgs e)
        {
            int rowHandle = e.RowHandle;
            DataRow row = gvTransactions.GetDataRow(rowHandle);

        }


        //private void pbAddNewRow_Click(object sender, EventArgs e)
        //{
        //    //_parentWindow.MapEasyCaptureToTranSetAsync(true);
        //    AddNewRow();
        //}

        private void AddNewRow()
        {
            _parentWindow.TransactionSet.AddNewTransaction();
            gvTransactions.UpdateCurrentRow();
            SetMenuIndicatorColumn(_parentWindow.TransactionSet.Transactions.Count - 1); //Bug #99053
            SetHeaderButtonsState(); //Bug #101885

            //Begin Task #114936
            gvTransactions.Focus();
            gvTransactions.FocusedColumn = gridColTran;
            gvTransactions.FocusedRowHandle = Math.Max(0, gvTransactions.RowCount - 1);
            gvTransactions.Focus();
            gvTransactions.ShowEditor();

            //Begin 121714-2EFG
            if (_tellerVars.IsAppOnline)    //#124403
            {
                SendKeys.SendWait("{BREAK}");
                SendKeys.SendWait("");
                SendKeys.SendWait("DIR {ENTER}");
            }
            else //#124917
            {
                //SendKeys.SendWait("{SHIFT}");
                //SendKeys.SendWait("");
                SendKeys.SendWait("+ {BACKSPACE}");
            }
            //End 121714-2EFG          

            LookUpEdit activeEditor = gvTransactions.ActiveEditor as LookUpEdit;
            if (activeEditor != null)
            {
                activeEditor.ShowPopup();
            }

            //End Task #114936
        }

        //Begin Bug #99053
        public void SetMenuIndicatorColumn(int rowIndex = -1)
        {
            if (rowIndex == -1)
                rowIndex = gvTransactions.FocusedRowHandle;
            if (rowIndex == -1 || gvTransactions.GetRowCellValue(rowIndex, "TcDescription") == null)
                return;

            Image img = null;

            if (DoesRowContainsTranMenu(rowIndex))
            {
                img = imgTransactions.Images[(int)ImageList.HorizontalGridLine];
            }

            gvTransactions.SetRowCellValue(rowIndex, "MenuIndicator", img);
            gvTransactions.UpdateCurrentRow();
        }
        //End Bug #99053

        private void DeleteRow()
        {
            _rowsToShowError.Remove(gvTransactions.FocusedRowHandle);
            _tranOptionSelectedForRows.Remove(gvTransactions.FocusedRowHandle); //Task #115712
            gvTransactions.DeleteSelectedRows();
            string description = string.Empty;
            string trancode = string.Empty;
            DataRow curTranRow = gvTransactions.GetFocusedDataRow();
            if (curTranRow != null)
            {
                description = curTranRow.GetStringValue("TcDescription");

                //Begin Bug #109821
                string acctType = (!string.IsNullOrEmpty(curTranRow.GetStringValue("AcctType")) ? curTranRow.GetStringValue("AcctType").Trim() : null);
                string acctNo = (!string.IsNullOrEmpty(curTranRow.GetStringValue("AcctNo")) ? curTranRow.GetStringValue("AcctNo").Trim() : null);

                _parentWindow.SetTransactionAccountInfo(acctType, acctNo);
                //End Bug #109821
            }
            else
            {
                _parentWindow.SetTransactionAccountInfo(null, null, true);  //Bug #109821
            }
            SetHeaderButtonsState();//Bug #98535



        }

        //private void pbDeleteCurrentRow_Click(object sender, EventArgs e)
        //{

        //    DeleteRow();
        //}

        //private void pbMapTran_Click(object sender, EventArgs e)
        //{
        //    _parentWindow.MapEasyCaptureToTranSetAsync(true);
        //}

        private bool DoesRowContainsTranMenu(int rowIndex)
        {
            if (gvTransactions == null || rowIndex < 0 || rowIndex >= gvTransactions.RowCount)
                return false;

            string cellValue = gvTransactions.GetRowCellValue(rowIndex, "TcDescription") as string;
            return (cellValue == "Transfer" || cellValue == "Payment" || cellValue == "External" || cellValue == "Purchase");
        }

        public void SetErrorInfo(string error)
        {

            if (!string.IsNullOrEmpty(error))
            {
                dfErrorInfo.ForeColor = System.Drawing.Color.Yellow;
                dfErrorInfo.BackColor = System.Drawing.Color.Red;
                //dfErrorInfo.Visible = true;
            }
            else
            {
                dfErrorInfo.ForeColor = Control.DefaultForeColor;
                dfErrorInfo.BackColor = System.Drawing.SystemColors.Control;
                //dfErrorInfo.Visible = false;
            }
            dfErrorInfo.Text = error;
        }

        public void RefreshGrid()
        {
            //Begin 93253
            try
            {
                this.gcTransactions.RefreshDataSource();
                if (_parentControl != null) //#123952
                    _parentControl.Refresh();
            }
            catch (Exception e)
            {
            }
            //End 93253          
        }

        public void ClearErrorInfo()
        {
            dfErrorInfo.Text = string.Empty;
            _errorFieldToFocus = string.Empty;
            _rowsToShowError.Clear();
        }

        public EasyCaptureTran GetSelectedTran()
        {
            return GetSelectedRow();
        }

        public void HandleDenominate()   //#89998
        {
            ucCashOut1.EnableDisableControls();
        }

        private void repositoryItemHyperLinkEdit1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //#87767
            //if (e.KeyChar.ToString().ToLower() == ((char)Keys.H).ToString().ToLower() || e.KeyChar.ToString().ToLower() == ((char)Keys.Space).ToString().ToLower())
            //{
            //    MessageBox.Show("Invoke the hold bal window here");
            //}
        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Invoke the hold bal window here");
            //#87767
            EasyCaptureTran tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];
            _parentWindow.CurTranId = tran.TranId;
            _parentWindow.CallOtherForms("HardHold");
        }

        public void HideAlerts()
        {
            _tranAlerts.HideAlerts(); //Task 89995
            _tranAlerts.ResetAlertCache(); //Task 89995
        }

        //Begin Task 89995
        private void ForceRedrawAlerts()
        {
            foreach (var obj in _parentControl.CustomHeaderButtons)
            {
                CustomHeaderButton btn = obj as CustomHeaderButton;
                if (btn != null)
                {
                    btn.BeginUpdate();
                    btn.EndUpdate();
                }
            }
        }
        //End Task 89995

        private void GvTransactions_Click(object sender, EventArgs e)
        {
            if (_parentWindow.TransactionSet != null && _parentWindow.TransactionSet.Transactions != null && _parentWindow.TransactionSet.Transactions.Count > 0)
            {
                var tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];
                if (tran.TranId > 0)
                    _parentWindow.CurTranId = tran.TranId;
            }

        }

        //Begin Task #113989

        private void GvTransactions_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.Column == gridColCheckNo)
            {
                e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
            }
        }

        private void GvTransactions_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
        {
            string errorMessage = "";
            string val = e.Value as string;

            if (gvTransactions.FocusedColumn == gridColCheckNo && !string.IsNullOrEmpty(val))
            {
                bool customError = false;
                try
                {
                    int nb = int.Parse(val);
                    if (nb < 0)
                    {
                        customError = true;
                        throw new Exception("The entered value cannot be negative.");
                    }
                    e.Value = nb; //Bug #114145
                }
                catch (Exception ex)
                {
                    if (customError)
                        errorMessage = ex.Message;
                    else
                        errorMessage = "Invalid number format.";
                }
            }

            val = (!string.IsNullOrEmpty(val)) ? val.Trim() : val; //121713-#7:Bug-fix

            //Begin 114934
            if (gvTransactions.FocusedColumn == gridColAmount && !string.IsNullOrEmpty(val))
            {
                if (_parentWindow.AssumeDecimals)
                {
                    if (Convert.ToDecimal(e.Value) > 0 && (!val.Contains(".")))
                    {
                        decimal workamt = CurrencyHelper.GetUnformattedValue(val);
                        e.Value = workamt;
                    }
                }
            }
            //End 114934

            if (!string.IsNullOrEmpty(errorMessage))
            {
                e.Valid = false;
                e.ErrorText = errorMessage;
            }
        }

        //End Task #113989

        private void gvTransactions_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gvTransactions.FocusedRowHandle == int.MinValue)
                return;

            EasyCaptureTran tran = null;

            if (gvTransactions.FocusedRowHandle >= 0 && gvTransactions.FocusedRowHandle < _parentWindow.TransactionSet.Transactions.Count)
            {
                tran = _parentWindow.TransactionSet.Transactions[gvTransactions.FocusedRowHandle];

                //Begin 122493
                if (tran.TranId > 0)
                {
                    _parentWindow.CurTranId = tran.TranId;  //#99058

                    _newTranRecordMode = false;
                }
                else
                {
                    _newTranRecordMode = true;
                }
                //End 122493
            }

            if (tran != null && !string.IsNullOrEmpty(tran.TcDescription))
            {
                _parentWindow.SetCurrentTran(tran.TcDescription, tran.TranDef.TranCode);
                _parentWindow.PerformEnableDisableControls("TranCodeSelect");
                ShowAlerts(tran.AcctType, tran.AcctNo, tran.TfrAcctType, tran.TfrAcctNo);
            }
            else
            {
                HideAlerts();
            }

            //Begin Task#94607/94608
            if (tran != null && !string.IsNullOrEmpty(tran.AcctType) && !string.IsNullOrEmpty(tran.AcctNo))
            {
                _parentWindow.SetTransactionAccountInfo(tran.AcctType, tran.AcctNo); //Bug #109821
            }
            else
            {
                _parentWindow.SetTransactionAccountInfo(null, null); //Bug 95564 //Bug #109821
            }
            //End Task#94607/94608

            Control_GotFocus(gcTransactions, null); //89907 ???

            //Begin 87757 
            if (tran != null && tran.TranDef != null)
            {
                SetOption(tran);

                SetMenuIndicatorColumn(gvTransactions.FocusedRowHandle); //Bug #99053
            }
            //End 87757 

            SetOptionType(null); //87761

            //Begin 87757-2  
            if (tran != null && tran.TranDef != null && tran.TranDef.TranType.ToString() != string.Empty)
            {
                ResetTfrFlds();
            }
            //End 87757-2 
        }

        private void SetOption(EasyCaptureTran tran)
        {
            //Begin 87751: Cleaned up code & added condition for external accounts
            string options = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") as string;
            if (tran != null && tran.TranDef != null && !string.IsNullOrEmpty(options))
            {
                var cellValBefore = gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options");
                if (tran.TranDef.TranType.ToString() == EasyCaptureTranType.Transfer.ToString())  //87759
                {
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", EasyCaptureTranTfrOptions.TransferWithdrawal.ToString());
                }
                else if (tran.TranDef.TranType.ToString() == EasyCaptureTranType.Payment.ToString())
                {
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString());
                }
                else if (tran.TranDef.TranType.ToString() == EasyCaptureTranType.External.ToString())
                {
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", EasyCaptureTranExtOptions.ExternalCredit.ToString());
                }
                else if (tran.TranDef.TranType.ToString() == EasyCaptureTranType.Purchase.ToString())
                {
                    gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, "Options", EasyCaptureTranPurchaseOptions.CashiersCheck.ToString());
                }
            }
            //End 87751
        }

        //Begin Task 89995
        private void ShowAlerts(string fromAcctType, string fromAcctNo, string toAcctType = null, string toAcctNo = null)
        {
            //Begin Task# 90007 Offline
            if (!_tellerVars.IsAppOnline)
                return;
            //End Task# 90007

            if (!string.IsNullOrEmpty(fromAcctType) && !string.IsNullOrEmpty(fromAcctNo))
            {
                if (!string.IsNullOrEmpty(toAcctType) && !string.IsNullOrEmpty(toAcctNo))
                {
                    _tranAlerts.ShowFromToAlerts(fromAcctType, fromAcctNo, toAcctType, toAcctNo);
                }
                else
                {
                    _tranAlerts.ShowAlerts(fromAcctType, fromAcctNo);
                }
            }
        }
        //End Task 89995

        //This event is to process the TAB key within the grid
        private void gcTransactions_ProcessGridKey(object sender, KeyEventArgs e)
        {
            gvTransactions.OptionsBehavior.FocusLeaveOnTab = true;

            PDxGridControl grid = sender as PDxGridControl;
            GridView view = grid.FocusedView as GridView;
            bool focusedAnErrorField = false;

            //Do NOT handle any modifiers such as SHIFT or CTRL keys
            if (e.Modifiers != Keys.None)
            {
                e.Handled = false;
                return;
            }


            if (e.KeyCode == Keys.Tab)
            {
                //if ((e.Modifiers == Keys.None && view.IsNewItemRow(view.FocusedRowHandle) && view.FocusedColumn.VisibleIndex == view.VisibleColumns.Count - 1))
                int lastEnabledCellIndex = GetLastEnabledCellIndexForView(view);           //114934

                //if ((view.FocusedColumn.VisibleIndex == view.VisibleColumns.Count - 1))  //114934
                if ((view.FocusedColumn != null && view.FocusedColumn.VisibleIndex == lastEnabledCellIndex))             //114934 //#122727
                {
                    if (view.IsEditing)
                    {
                        view.CloseEditor();
                    }

                    //If we are tabbing from a row that is NOT the last row, do not add a new row
                    if (view.FocusedRowHandle == (view.RowCount - 1))
                    {
                        CreateNewRowIfCurrentIsValid();
                    }
                    else
                    {
                        view.FocusedRowHandle = view.FocusedRowHandle + 1;
                        gvTransactions.SelectRow(view.FocusedRowHandle);
                    }

                    if (!focusedAnErrorField)
                    {
                        view.FocusedColumn = view.VisibleColumns[0];
                    }
                    grid.BeginInvoke(new MethodInvoker(() => { view.ShowEditor(); }));

                    //SelectNextControl(grid, e.Modifiers == Keys.None, false, false, true);
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                if (view.IsEditing)
                {
                    view.CloseEditor();
                    if (_popupFocused)  //#122359 - Popup editor closed with the Escape click
                    {
                        _popupFocused = false;
                    }
                    //    gvTransactions.ShowEditor();    //#122359 - This will enable show editor that is disabled by Escape click
                    //gvTransactions.OptionsNavigation.UseTabKey = false; //Use tab key to jump out of grid //#122359 - This line of code is messing up Tab key behavior
                }
                e.Handled = true;
            }
        }


        //Begin 121714-1AB&2A
        private int GetNextEnabledCell(GridView view)
        {
            int NextEnabledCellIndex = 0;
            bool bNextEnabledCol = false;
            GridColumn currentColumn = view.FocusedColumn;
            bool IsReadOnlyCol = false;

            foreach (BandedGridColumn col in view.VisibleColumns)
            {
                if (bNextEnabledCol)
                {
                    IsReadOnlyCol = (col.OptionsColumn.ReadOnly || col.ColumnEdit != null && col.ColumnEdit.ReadOnly) ? true : false;
                    if (!IsReadOnlyCol)
                    {
                        NextEnabledCellIndex = col.VisibleIndex;
                        break;
                    }
                }

                if (col == currentColumn)
                    bNextEnabledCol = true;
            }

            return NextEnabledCellIndex;
        }
        //End 121714-1AB&2A


        //Begin 114934
        private int GetLastEnabledCellIndexForView(GridView view)
        {
            int lastEnabledCellIndex = 0;
            foreach (BandedGridColumn col in view.VisibleColumns)
            {
                var colEdit = col.ColumnEdit as RepositoryItem;
                if (col.VisibleIndex > lastEnabledCellIndex)
                {
                    if ((colEdit != null && !colEdit.ReadOnly) ||
                        (colEdit == null && !col.OptionsColumn.ReadOnly))
                    {
                        lastEnabledCellIndex = col.VisibleIndex;
                    }
                }
            }

            return lastEnabledCellIndex;
        }
        //End 114934


        //Begin Task #87737
        private bool ShowErrorAndFocusField(string errorMessage, int row = -1)
        {
            if (row == -1)
                row = gvTransactions.FocusedRowHandle;

            bool focusedAnErrorField = false;

            _rowsToShowError.Add(gvTransactions.FocusedRowHandle);
            PMessageBox.Show(MessageType.Warning, errorMessage);
            var tranResponse = _parentWindow.TransactionSet.Transactions[row].TranResponse;
            if (!string.IsNullOrEmpty(_errorFieldToFocus))
            {
                FocusProblematicField(_errorFieldToFocus, row);
                focusedAnErrorField = true;
            }
            else if (tranResponse.Count > 0)
            {
                FocusProblematicField(tranResponse[0], row);
                focusedAnErrorField = true;
            }

            return focusedAnErrorField;
        }
        //End Task #87737

        private void repositoryItemRegD_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gvTransactions_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            var errorText = GetErrorMessage(e.RowHandle);
            if (_rowsToShowError.Contains(e.RowHandle) && !string.IsNullOrEmpty(errorText))
            {
                e.Appearance.BackColor = Color.Pink;
            }

            //Begin 122493
            GridView view = sender as GridView;

            if (e.Column.FieldName == "RegD" || e.Column.FieldName == "PassDescriptionToHistory" || e.Column.FieldName == "CheckNo") //#124295
            {
                var tran = _parentWindow.TransactionSet.Transactions[e.RowHandle];

                if (e.Column.FieldName == "RegD")
                {
                    string regDEnable = "N", regDCheck = "N";


                    if (tran.TranDef.DpLN == "DP")
                    {
                        string regDValue = string.Empty;

                        //Begin 122798
                        DataTable workAccts = new DataTable();
                        if (_parentWindow.Accounts != null && _tellerVars.IsAppOnline)  //#124403 - Offline Check added
                        {
                            workAccts = _parentWindow.Accounts.Copy();
                        }
                        //End 122798

                        if (gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options") != null &&
                            gvTransactions.GetRowCellValue(gvTransactions.FocusedRowHandle, "Options").ToString() != string.Empty &&
                            tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferDeposit.ToString()
                            )
                        {
                            if ((tran.TfrAcctNo != null && tran.TfrAcctNo.ToString() != string.Empty) &&
                               (tran.TfrAcctType != null && tran.TfrAcctType.ToString() != string.Empty))
                            {
                                //Begin 122798
                                //regDValue = tran.TfrAcctType.Trim() + "~" + tran.TfrAcctNo.Trim(); 

                                string expression = "AcctNo = '" + tran.TfrAcctNo.Trim() + "' and " + "AcctType = '" + tran.TfrAcctType.Trim() + "'";
                                DataRow[] RowMatch = workAccts.Select(expression);
                                if (RowMatch.Length > 0)
                                {
                                    regDValue = RowMatch[0].GetStringValue("IsRegDAccount").Trim();
                                }
                                //End 122798

                                _parentWindow.ActionRequest("GetTC102RegDValue", tran.TranDef.TranCode, ref regDValue);
                            }
                        }
                        else if (tran.TranDef.Description == EasyCaptureTranCodeDesc.Withdrawal.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.CashedCheck.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.TransferWithdrawal.ToString() ||
                                 tran.TranDef.Description == EasyCaptureTranCodeDesc.AutoLoanPayment.ToString()
                                )
                        {
                            if ((tran.AcctNo != null && tran.AcctNo.ToString() != string.Empty) &&
                                 (tran.AcctType != null && tran.AcctType.ToString() != string.Empty))
                            {
                                //Begin 122798
                                //regDValue = tran.AcctType.Trim() + "~" + tran.AcctNo.Trim();  //122798
                                if (workAccts != null && workAccts.Rows.Count > 0)  //#124403 - for offline mode
                                {
                                    string expression = "AcctNo = '" + tran.AcctNo.Trim() + "' and " + "AcctType = '" + tran.AcctType.Trim() + "'";
                                    DataRow[] RowMatch = workAccts.Select(expression);
                                    if (RowMatch.Length > 0)
                                    {
                                        regDValue = RowMatch[0].GetStringValue("IsRegDAccount").Trim();
                                    }
                                    //End 122798
                                }

                                _parentWindow.ActionRequest("GetTC102RegDValue", tran.TranDef.TranCode, ref regDValue);
                            }
                        }

                        if (regDValue != string.Empty)
                        {
                            regDEnable = regDValue.Split("~".ToCharArray())[0];
                            regDCheck = regDValue.Split("~".ToCharArray())[1];
                        }
                    }

                    if (regDEnable != "Y")
                        e.Handled = true;
                }
                else if (e.Column.FieldName == "PassDescriptionToHistory") //#124295
                {
                    string EnableDesc = "N", EnableRef = "N", DescValue = string.Empty;

                    _parentWindow.ActionRequest("GetEnableDescRefFlags", tran.TranDef.TranCode, ref DescValue);

                    if (DescValue != string.Empty)
                    {
                        EnableDesc = DescValue.Split("~".ToCharArray())[0];
                        EnableRef = DescValue.Split("~".ToCharArray())[1];

                        if (EnableDesc != "Y")
                            e.Handled = true;
                    }
                }
                else  //#124295
                {
                    #region Enable/Disable Check#
                    string enableCheckNo = "N"; //#124295

                    if (tran.TranDef.DpLN == "DP")
                    {
                        if (tran.TranDef.Description == EasyCaptureTranCodeDesc.Deposit.ToString())
                        {
                            DataTable workAccts = new DataTable();
                            if (_parentWindow.Accounts != null)
                            {
                                workAccts = _parentWindow.Accounts.Copy();

                                string expression = "AcctNo = '" + tran.AcctNo.Trim() + "' and " + "AcctType = '" + tran.AcctType.Trim() + "'";
                                if (workAccts != null && workAccts.Rows.Count > 0)
                                {
                                    DataRow[] RowMatch = workAccts.Select(expression);
                                    if (RowMatch.Length > 0)
                                    {
                                        enableCheckNo = RowMatch[0].GetStringValue("IsReconAccount").Trim();
                                    }
                                }
                            }
                        }
                    }
                    _parentWindow.ActionRequest("GetEnableCheckNoFlags", tran.TranDef.TranCode, ref enableCheckNo);
                    enableCheckNo = enableCheckNo.Split("~".ToCharArray())[0];
                    if (enableCheckNo != "Y")
                        e.Handled = true;
                    #endregion
                }
            }
            //End 122493
        }

        private void gvTransactions_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator)
            {
                var errorText = GetErrorMessage(e.RowHandle);
                if (_rowsToShowError.Contains(e.RowHandle) && !string.IsNullOrEmpty(errorText) && ucImages.Images.Count >= 1)    //#124210 #124295
                {
                    e.Info.ImageIndex = -1;
                    e.Painter.DrawObject(e.Info);
                    Rectangle r = e.Bounds;
                    var bitmap = ucImages.Images[ucImages.ImageNames.Error16];
                    r.Inflate(-1, -1);
                    int x = r.X + (r.Width - bitmap.Size.Width) / 2;
                    int y = r.Y + (r.Height - bitmap.Size.Height) / 2;
                    //
                    e.Graphics.DrawImageUnscaled(bitmap, x, y);
                    //
                    e.Handled = true;
                }
            }
        }

        private string GetErrorMessage(int rowIndex)
        {
            _errorFieldToFocus = "";
            string error = null;
            if (rowIndex >= 0 && _parentWindow.TransactionSet.Transactions.Count > 0)
            {
                //Begin #87737
                if (gvTransactions.FocusedRowHandle < gvTransactions.RowCount)
                {
                    //Begin Task #103255
                    foreach (var mandatoryField in _mandatoryFields)
                    {
                        string value = gvTransactions.GetRowCellValue(rowIndex, mandatoryField.Key) as string;
                        if (value != null)
                        {
                            value = value.Trim();
                        }
                        //Check if value has only one character since account type combined has a -
                        if (string.IsNullOrEmpty(value) || value.Length == 1)
                        {
                            _errorFieldToFocus = mandatoryField.Key;
                            error = mandatoryField.Value;
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(error))
                    {
                        var tran = _parentWindow.TransactionSet.Transactions[rowIndex];
                        error = tran.GetErrorText();
                        if (!string.IsNullOrEmpty(error) && tran.TranResponse != null && tran.TranResponse.Count > 0)
                        {
                            var tranResponse = tran.TranResponse[0];
                            if (tranResponse != null)
                            {
                                _errorFieldToFocus = GetColumnToFocus(tranResponse.FocusField);
                            }
                        }
                    }
                    //End Task #103255
                }

                //End #87737
            }
            return error;
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gcTransactions) return;

            ToolTipControlInfo info = null;
            //Get the view at the current mouse position
            GridView view = gcTransactions.GetViewAt(e.ControlMousePosition) as GridView;
            if (view == null)
                return;
            //Get the view's element information that resides at the current position
            GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
            //Display a hint for row indicator cells
            if (hi.HitTest == GridHitTest.RowIndicator)
            {
                var errorText = GetErrorMessage(hi.RowHandle);
                if (string.IsNullOrEmpty(errorText) == false)
                {
                    //An object that uniquely identifies a row indicator cell
                    object o = hi.HitTest.ToString() + hi.RowHandle.ToString();
                    info = new ToolTipControlInfo(o, errorText);
                }
            }
            if (hi.InRow)
            {
                var errorText = GetErrorMessage(hi.RowHandle);
                if (string.IsNullOrEmpty(errorText) == false)
                {
                    //An object that uniquely identifies a row indicator cell
                    object o = hi.HitTest.ToString() + hi.RowHandle.ToString();
                    info = new ToolTipControlInfo(o, errorText);
                }
            }
            //Supply tooltip information if applicable, otherwise preserve default tooltip (if any)
            if (info != null)
                e.Info = info;
        }
        public void ResetCashOutInfo()
        {
            ucCashOut1.ResetCashOutInfo();
        }

        public void GetLastTransactionRowErrorMessage(Action<string> callback = null)
        {
            string error = "";

            _parentWindow.MapEasyCaptureToTranSetAsync(false, "CollectionChanged", () =>
            {
                if (gvTransactions.RowCount > 0)
                {
                    error = GetErrorMessage(gvTransactions.RowCount - 1);
                }
                if (callback != null)
                {
                    callback(error);
                }
            });
        }

        public void FocusPanel()
        {
            gvTransactions.Focus();
            if (gvTransactions.RowCount > 0)
            {
                gvTransactions.FocusedRowHandle = gvTransactions.RowCount - 1;
                gvTransactions.FocusedColumn = gvTransactions.Columns["Amount"];
                gvTransactions.ShowEditor(); //121713-#7
            }
        }

        //Begin Bug #91947
        public void FocusProblematicField(EasyCaptureTranResponse tranResponse, int rowIndex = -1)
        {
            if (gvTransactions.RowCount == 0) //Task# 90007 //Task #105770
                return;


            if (rowIndex == -1)
                rowIndex = gvTransactions.FocusedRowHandle;

            if (tranResponse == null || rowIndex < 0 || rowIndex >= gvTransactions.RowCount)
                return;
            string focusField = "";
            string columnToFocus = GetColumnToFocus(tranResponse.FocusField, tranResponse.ErrorCode);
            if (!string.IsNullOrEmpty(_errorFieldToFocus))
            {
                focusField = _errorFieldToFocus;
            }
            else if (!string.IsNullOrEmpty(columnToFocus))
            {
                focusField = columnToFocus;
            }

            if (!string.IsNullOrEmpty(focusField))
            {
                FocusProblematicField(focusField, rowIndex);
            }
        }

        public void CloseEditorSetFocus()   //#122727
        {
            if (gvTransactions.FocusedColumn != null)
            {
                if (gvTransactions.FocusedColumn.FieldName == "TfrAcctType")
                {
                    GridView view = gvTransactions as GridView;
                    if (view.IsEditing)
                        SendKeys.Send("{tab}");
                    SendKeys.Send("{tab}");
                }
            }
        }

        public void FocusProblematicField(string columnToFocus, int rowIndex)
        {
            if (columnToFocus == null || rowIndex < 0 || rowIndex >= gvTransactions.RowCount)
                return;

            gvTransactions.Focus();
            gvTransactions.FocusedRowHandle = rowIndex;
            if (columnToFocus != "MenuIndicator")
                gvTransactions.SetRowCellValue(gvTransactions.FocusedRowHandle, columnToFocus, null);

            gvTransactions.FocusedColumn = gvTransactions.Columns[columnToFocus];
            gvTransactions.ShowEditor();

            //Begin Task #115712
            if (columnToFocus == "MenuIndicator")
            {
                DoShowMenu(rowIndex);
            }
            else if (gvTransactions.ActiveEditor != null)
            {
                if (gvTransactions.ActiveEditor.GetType() == typeof(LookUpEdit))
                {
                    LookUpEdit lookupEdit = gvTransactions.ActiveEditor as LookUpEdit;
                    lookupEdit.ShowPopup();
                }
                else if (gvTransactions.ActiveEditor.GetType() == typeof(TextEdit))
                {
                    TextEdit textEditor = gvTransactions.ActiveEditor as TextEdit;
                    textEditor.SelectionStart = 0;
                    textEditor.SelectionLength = textEditor.Text.Length;
                }
            }
            //End Task #115712

            if (columnToFocus == "TfrAcctType")
                SendKeys.Send(" "); //Bug #98178
        }

        private string GetColumnToFocus(string focusField, int errorCode = -1)
        {
            string columnToFocus = "";
            bool isAppOnline = _tellerVars != null && _tellerVars.IsAppOnline;
            Dictionary<string, ColumnErrorInfo> columnForField = new Dictionary<string, ColumnErrorInfo>()
            {
                {"TcDescription",new ColumnErrorInfo("TlTranCode") },
                {isAppOnline ? "Account":"AcctNo",new ColumnErrorInfo("AcctNo",311344)},
                {"Amount",new ColumnErrorInfo("NetAmt",314965) },
                {"TfrAcctType",new ColumnErrorInfo("TfrAcctType", 311342) },
                {"Options",new ColumnErrorInfo() },
                {"TfrAmount",new ColumnErrorInfo() },
                {"TfrAcctNo",new ColumnErrorInfo("TfrAcctNo") },
                {"TranDef",new ColumnErrorInfo() },
                {"CcAmt",new ColumnErrorInfo() },
                {"CheckNo",new ColumnErrorInfo("CheckNo","TfrChkNo") },
                {"RegD",new ColumnErrorInfo() },
                {"HoldBal",new ColumnErrorInfo() },
                {"Reference",new ColumnErrorInfo("Reference",360047) }, //Bug 97326
                {"PassDescriptionToHistory",new ColumnErrorInfo() },
                {"Description",new ColumnErrorInfo("Description",360138,360046) }, //Bug 97326
                {"AcctType",new ColumnErrorInfo("AcctType") },
            };

            foreach (var pair in columnForField)
            {
                if (pair.Value.IsValidColumnForError(focusField, errorCode))
                {
                    columnToFocus = pair.Key;
                    break;
                }
            }

            return columnToFocus;
        }

        public void Destroy()
        {
            this.gvTransactions.FocusedRowChanged -= gvTransactions_FocusedRowChanged;
            this.gvTransactions.CellValueChanged -= GvTransactions_CellValueChanged;
            this.gvTransactions.FocusedColumnChanged -= GvTransactions_FocusedColumnChanged; //#89907
            this.gcTransactions.MouseDown -= GcTransactions_MouseDown;
            this.gvTransactions.Click -= GvTransactions_Click;
            this.gvTransactions.Dispose();
            this.gvTransactions = null;

            foreach (Control control in this.Controls)
            {
                control.GotFocus -= Control_GotFocus;
            }

            repItemAccounts.EditValueChanging -= RepItemAccounts_EditValueChanging;//87753

            repItemAccounts.Enter -= RepItemAccounts_Enter; //87753-2

            _rowsToShowError.Clear(); //Task #87737

            _errorFieldToFocus = ""; //Task #87737
        }

        public class ColumnErrorInfo
        {
            public List<string> FocusFields { get; set; }
            public List<int> ErrorCodes { get; set; }

            public ColumnErrorInfo()
            {
                this.FocusFields = new List<string>();
                this.ErrorCodes = new List<int>();
            }

            public ColumnErrorInfo(params int[] errorCodes) : this(new List<string>() { "" }, errorCodes)
            {

            }

            public ColumnErrorInfo(string focusField, params int[] errorCodes) : this(new List<string>() { focusField }, errorCodes)
            {

            }

            public ColumnErrorInfo(string focusField, string focusField2, params int[] errorCodes) : this(new List<string>() { focusField, focusField2 }, errorCodes)
            {

            }

            public ColumnErrorInfo(string focusField, string focusField2, string focusField3, params int[] errorCodes) : this(new List<string>() { focusField, focusField2, focusField3 }, errorCodes)
            {

            }

            private ColumnErrorInfo(List<string> focusFields, params int[] errorCodes)
            {
                this.FocusFields = focusFields;
                this.ErrorCodes = new List<int>();
                if (errorCodes != null && errorCodes.Length > 0)
                {
                    foreach (var code in errorCodes)
                    {
                        this.ErrorCodes.Add(code);
                    }
                }
            }

            public bool IsValidColumnForError(string focusField, int errorCode)
            {
                bool isValid = false;

                if (!string.IsNullOrEmpty(focusField))
                {
                    isValid = this.FocusFields.Contains(focusField);
                }
                if (!isValid && ErrorCodes.Count > 0)
                {
                    isValid = ErrorCodes.Contains(errorCode);
                }

                return isValid;
            }
        }

        //End Bug #91947

        //Begin Bug #99049
        enum ImageList
        {
            Delete = 0,
            Insert,
            HorizontalGridLine,
        }
        //End Bug #99049

        //Begin Task#90007
        /// <summary>
        /// 
        /// </summary>
        void OfflineSettings()
        {

            if (!_tellerVars.IsAppOnline)

            {
                #region Hide unused columns
                //gridColCcAmt.Visible = false;
                gridColHoldBal.Visible = false;
                gridColAccount.Visible = false;
                #endregion

                #region Display hidden AcctNo/AcctType Columns
                gvTransactions.Bands["gridBandTransaction"].Columns.Add(this.gridColActType);
                gridColActType.Visible = true;
                gridColActType.OptionsColumn.ShowInCustomizationForm = true;
                gridColActType.AutoFillDown = true;
                gridColActType.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;


                repItemAcctType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                repItemAcctType.NullText = "";
                gridBandTransaction.Columns.MoveTo(2, gridColActType);  //#124403
                repItemAcctType.DataSource = GetAcctTypeTable("ALL");
                gridColActType.ColumnEdit = this.repItemAcctType;
                repItemAcctType.Enter += RepItemAcctType_Enter;

                gvTransactions.Bands["gridBandTransaction"].Columns.Add(this.gridColAcctNo);
                gridColAcctNo.Visible = true;
                gridColAcctNo.OptionsColumn.ShowInCustomizationForm = true;
                gridColAcctNo.AutoFillDown = true;
                gridColAcctNo.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                gridBandTransaction.Columns.MoveTo(3, gridColAcctNo);   //#124403
                gridBandTransaction.Columns.MoveTo(0, gridColTran);
                #endregion

            }


            //End Task#90007

        }

        //Begin Task# 90007
        private void RepItemAcctType_Enter(object sender, EventArgs e)
        {
            EasyCaptureTran selectedTranRow = GetSelectedRow();

            if (selectedTranRow != null && selectedTranRow.TranDef != null)
            {
                DevExpress.XtraEditors.LookUpEdit AcctTypeLookupEdit = sender as DevExpress.XtraEditors.LookUpEdit;


                DataTable _acctTypeTable = GetAcctTypeTable(selectedTranRow.TranDef.DpLN);
                AcctTypeLookupEdit.Properties.DataSource = _acctTypeTable;

                //List<AccountTypeDefinition> _acctTypeDefList = new List<AccountTypeDefinition>();
                //_acctTypeDefList = GetAcctTypeDefinitionList(selectedTranRow.TranDef.DpLN);
                //AcctTypeLookupEdit.Properties.DataSource = _acctTypeDefList;

                //List<string> _acctTypeListString = new List<string>();
                //_acctTypeListString = GetAcctTypeStringList(selectedTranRow.TranDef.DpLN);
                //AcctTypeLookupEdit.Properties.DataSource = _acctTypeListString;
            }

        }

        //End Task# 90007


        //Begin  Task#90007
        /// <summary>
        /// 
        /// </summary>
        /// <param name="depLoan"></param>
        /// <returns></returns>
        List<string> GetAcctTypeStringList(string depLoan)
        {
            string _acctTypeList = null;
            List<string> _acctTypeListString = new List<string>();

            _parentWindow.ActionRequest("GetAcctTypeList", depLoan, ref _acctTypeList);

            if (!string.IsNullOrEmpty(_acctTypeList))
                _acctTypeListString.AddRange(_acctTypeList.Split('~'));


            return _acctTypeListString;
        }

        DataTable GetAcctTypeTable(string depLoan)
        {
            string _acctTypeList = null;
            List<string> _acctTypeListString = new List<string>();
            DataTable dt = new DataTable();
            //DataColumn dc = new DataColumn("AcctType", typeof(String));
            //DataColumn dc = new DataColumn("", typeof(String));
            dt.Columns.Add("AcctType");

            _parentWindow.ActionRequest("GetAcctTypeList", depLoan, ref _acctTypeList);

            if (!string.IsNullOrEmpty(_acctTypeList))
                _acctTypeListString.AddRange(_acctTypeList.Split('~'));

            foreach (string acctType in _acctTypeListString)
            {
                DataRow dr = dt.NewRow();
                //dr[0] = acctType;
                dr["AcctType"] = acctType;
                dt.Rows.Add(dr);
                //dt.ImportRow(dr);
            }

            return dt;
        }

        private class AccountTypeDefinition
        {
            public string AcctType { get; set; }

            public AccountTypeDefinition(string accttype)
            {
                AcctType = accttype;
            }
        }

        List<AccountTypeDefinition> GetAcctTypeDefinitionList(string depLoan)
        {
            string _acctTypeList = null;
            List<string> _acctTypeListString = new List<string>();
            AccountTypeDefinition acctTypeDef = new AccountTypeDefinition("");

            List<AccountTypeDefinition> _acctTypeDefList = new List<AccountTypeDefinition>();

            _parentWindow.ActionRequest("GetAcctTypeList", depLoan, ref _acctTypeList);

            if (!string.IsNullOrEmpty(_acctTypeList))
            {
                _acctTypeListString.AddRange(_acctTypeList.Split('~'));

                foreach (string acctType in _acctTypeListString)
                {
                    acctTypeDef.AcctType = acctType;
                    _acctTypeDefList.Add(acctTypeDef);
                }

            }

            return _acctTypeDefList;
        }

        //Begin Bug#108552
        public void OnSearchPerformed(bool searchSucceeded)
        {
            custSearchSucceeded = searchSucceeded;
            SetHeaderButtonsState();
        }
        //End Bug#108552


        //Begin 121713-#2
        public void FocusOnCashOut()
        {
            ucCashOut1.FocusPanel();
        }
        //End 121713-#2

        //Begin 122355-#2
        public bool IsFocusOnCashOut()
        {
            return ucCashOut1.ContainsFocus;
        }
        //End 122355-#2
    }
}
