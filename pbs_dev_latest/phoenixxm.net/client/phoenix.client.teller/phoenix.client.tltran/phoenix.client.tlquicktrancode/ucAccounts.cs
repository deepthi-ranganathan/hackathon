#region Comments
//-------------------------------------------------------------------------------
// File Name: ucAccounts.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
#endregion

#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//04/13/2018    2       FOyebola    Enh#220316-Task#87721
//04/24/2018    3       DGarcia     Bug 89210  
//04/24/2018    4       mselvaga    Enh#220316-#87731-1
//04/25/2018    5       FOyebola    Enh#220316-Task#87725
//05/15/2018    6       DGarcia     Bug#89991
//05/15/2018    7       DGarcia     Task#89976 - Display and History Action buttons in Quick Transaction tab
//05/22/2018    8       CRoy        Bug #91419 - Add function to clear all rows.
//05/23/2018    9       DGarcia     Task#89967 - Account Alerts & Notifications
//06/04/2018    10      DGarcia     Bug#92485 - Umbrella plan alert is not showing up in quick transaction
//06/07/2018    11      CRoy        Bug #92489 - Search again, do not reload if RIM or acct is the same.
//06/07/2018    12      FOyebola    Enh#220316 - Task 87759
//06/07/2018    13      CRoy        Bug #92490 - Hide account alerts on clear all.
//06/07/2018    14      FOyebola    CVTIssue 178871 - Bug 90929
//06/08/2018    15      DGarcia     Bug#93072
//06/18/2018    16      FOyebola    Enh#220316 - Task 87751
//06/19/2018    17      FOyebola    Enh#220316 - Task 87753
//07/02/2018    18      CRoy        Task 89995 - Transfer alert information.
//07/11/2018    19      CRoy        Bug #95593 - Add values to non used properties when creating alerts
//07/13/2018    20      DGarcia     Task#95882
//07/17/2018    21      CRoy        Bug #95593
//07/20/2018    22      CRoy        Bug #95971 - Fix umbrella alerts display
//07/25/2018    23      FOyebola    Enh#220316 - Bug 95598
//08/21/2018    24      CRoy        Bug #98671 - Look and Feel
//08/27/2018    25      CRoy        Bug #99050 - Center up column headings
//09/04/2018    26      CRoy        Bug #99048 - Cosmetic issues
//09/12/2018    27      CRoy        Bug #101592 - Disable devexpress search panel when hitting CTRL+F
//10/05/2018    28      DGarcia     Task#100769 - Quick Acct Confidential Accounts
//12/07/2018    29      CRoy        Task #102896 - Add row colors to invalid accounts.
//01/14/2019    30      CRoy        Task #105856 - CVT Issue 190233	'Constant' variable guards dead code
//01/17/2019    31      CRoy        Bug #108757 - Add code to disable the funnel icon in the headers.
//05/31/2019    32      CRoy        Task #115070 - Account panel shrinking too much and hiding existing accounts.
//06/06/2019    33      DGarcia     Task #115070 - New Multi Tran Hotkeys for Transactions
//06/11/2019    34      CRoy        Task #115710 - Add Tran Type column with icon to show tran menu.
//06/11/2019    35      CRoy        Task #115070 - Add base grid row height.
//11/25/2019    36      FOyebola    Task#121714
//12/05/2019    37      FOyebola    Task#122355
//12/09/2019    38      FOyebola    Task#122356
//02/25/2020    39      FOyebola    Task#124297
#endregion

using System;
using System.Drawing;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Async.XmClient;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Menu;
using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;
using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.MultiTranTeller.Base;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Global;
using System.Collections.Generic;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using Phoenix.Shared;
using Phoenix.FrameWork.Shared.Variables;

namespace Phoenix.Client.Teller
{
    public partial class ucAccounts : EditFormUserControl
    {

        private DataRow dr = null;
        CustomHeaderButton _btnSearch;
        CustomHeaderButton _btnRelationShip;
        ITellerWindow _parentWindow;
        public DockPanel _parentDockPanel;
        private QuickTranAlerts _alerts;
        public string RmsAccount; //#87725
        AcctAlerts _acctAlerts = null;
        public string rimNo = string.Empty; //#87755
        CustomHeaderButton _btnInvisible; //#99048
        Dictionary<int, Color> _invalidRows; //Task #102896
        int _gridRowHeight = 18; //Task #115070
        CustomHeaderButton _iconSeparator; //122356
        CustomHeaderButton _signatureAlert; //122356

        //Begin Task #115070
        public int GridRowHeight
        {
            get { return _gridRowHeight; }
        }
        //End Task #115070

        public ucAccounts()
        {
            InitializeComponent();
            CenterColumnHeadings();//Bug #99050
            DisableColumnFiltering(); //Bug #108757
            _invalidRows = new Dictionary<int, Color>(); //Task #102896
        }

        //Begin Task #102896
        private void OnCustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
        {
            if (_invalidRows.ContainsKey(e.RowHandle))
            {
                e.Appearance.ForeColor = _invalidRows[e.RowHandle];
            }
        }
        //End Task #102896

        //Begin Bug #99050
        private void CenterColumnHeadings()
        {
            GridColumn[] headers = new GridColumn[] {
                colTranType, //Task #115710
                colAcctNo,
                colCurrentBalance,
                colAvailableBalance,
                colNickName,
                colStatus,
                colTitle1,
                colTitle2,
                colAcctRel
            };

            foreach (var header in headers)
            {
                header.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
            }
        }
        //End Bug #99050

        //Begin Bug #108757
        private void DisableColumnFiltering()
        {
            foreach (var column in gridViewAccounts.Columns)
            {
                var bandedGridColumn = column as GridColumn;
                if (bandedGridColumn != null)
                {
                    bandedGridColumn.OptionsFilter.AllowFilter = false;
                }
            }
            //#120877
            gridViewAccounts.OptionsFind.AllowFindPanel = false;
            gridViewAccounts.OptionsFind.AlwaysVisible = false;
            gridViewAccounts.OptionsMenu.EnableColumnMenu = false;
        }
        //End Bug #108757

        public async Task<CustomerAccountsResponse> PopulateCustomerAccounts(int RimNo)
        {
            CustomerAccountsResponse response = new CustomerAccountsResponse()
            {
                Success = false,
                AccountNumbers = new List<string>() //Bug #92489
            };
            InitializeAlerts();
            this.gridControlAccounts.Visible = false;
            try
            {

                var t = await GetListOfAccounts(RimNo);

                _parentWindow.Accounts = t;

                if (t != null && t.Rows.Count == 0) //Task#89976
                    return response;

                if (t != null && t.Columns.Contains("AccountCombined") == false)
                {
                    t.Columns.Add("AccountCombined");
                    //Begin Task#100769
                    //t.Columns.Add("CurBalanceAmt", typeof(System.Decimal));
                    //t.Columns.Add("AvailableBalance", typeof(System.Decimal));     //Task#87721
                    t.Columns.Add("CurBalanceAmt");
                    t.Columns.Add("AvailableBalance");
                    //End Task#100769
                }

                //Begin 90929
                if (t != null)
                {
                    int i = 0;
                    foreach (DataRow row in t.Rows)
                    {
                        try
                        {
                            //Begin Task #102896
                            Color rowColor = GetRowColor(row, i);
                            if (rowColor != Color.Empty)
                                _invalidRows[i] = rowColor;
                            //End Task #102896

                            string accountNumber = row.GetStringValue("AcctNo"); //Bug #92489
                            response.AccountNumbers.Add(accountNumber); //Bug #92489
                            row["AccountCombined"] = TranHelper.Instance.CombineAcctNo(accountNumber, row.GetStringValue("AcctType")); //  string.Format("{ 0} - {1}", row["AcctType"], row["AcctNo"]);

                            //Begin Task#100769
                            //row["CurBalanceAmt"] = row.GetDecimalValue("CurBal");       //Task#87721  //  Convert.ToDecimal(row["CurrentBalance"]);
                            //row["AvailableBalance"] = row.GetDecimalValue("AvailBal");  //Task#87721
                            if (_parentWindow.ConfAcctSecurity && _parentWindow.UserConfidentialAccess && row.GetStringValue("Confidential") == "Y")
                            {
                                row["CurBalanceAmt"] = "Confidential";
                                row["AvailableBalance"] = "Confidential";
                            }
                            else
                            {
                                row["CurBalanceAmt"] = FormatAmounts(row.GetStringValue("CurBal"));
                                row["AvailableBalance"] = FormatAmounts(row.GetStringValue("AvailBal"));
                            }
                            //End Task#100769
                        }
                        catch
                        {
                            return response;
                        }
                        ++i;
                    }
                }
                //End 90929

                //Begin Task #115710
                Image img = imgAccounts.Images[0];
                repItemTranType.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                t.Columns.Add("TranType", typeof(Image));
                foreach (DataRow row in t.Rows)
                {
                    row["TranType"] = img;
                }
                //End Task #115710

                this.gridControlAccounts.DataSource = t;
                this.gridViewAccounts.RefreshData();

                //Begin #87725
                if (RmsAccount != null)
                {
                    SetGridFocusedRow(RmsAccount);
                    RmsAccount = null;
                }
                //End #87725

                //Begin Task#89976
                _parentWindow.SetQuickAccountSelectedRow(gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle));
                //End Task#89976
                ShowAlerts();
                response.Success = true;

                return response;
            }
            finally
            {
                this.gridControlAccounts.Visible = true;
            }
        }

        //Begin Task #102896
        private Color GetRowColor(DataRow row, int index)
        {
            string colCurrentBalance = row["CurBal"] as string;
            string colODLimitTotal = row["ODLimitTotal"] as string;
            string colTabGroup = row["TabGroup"] as string;
            string colMatMethod = row["MatMethod"] as string;
            string colDelqDT = row["DelqDate"] as string;
            string colStatus = row["Status"] as string;
            string colChargedOff = row["ChargedOff"] as string;
            string colMatDtString = row["MatDt"] as string;
            DateTime colMatDt;
            if (!DateTime.TryParse(colMatDtString, out colMatDt))
            {
                colMatDt = DateTime.MaxValue;
            }
            decimal colCurrentBalanceDec = 0;
            decimal.TryParse(colCurrentBalance, out colCurrentBalanceDec);

            if (Convert.ToDecimal(colCurrentBalanceDec) < 0)
            {
                decimal ntmp = Convert.ToDecimal(colCurrentBalanceDec) * -1;
                decimal ntmpodlimit = 0;

                if (StringHelper.StrTrimX(colODLimitTotal) == string.Empty || StringHelper.StrTrimX(colODLimitTotal) == "Confidential") // Task#45378 - 'Confidential' added in condition
                    ntmpodlimit = 0;
                else
                    ntmpodlimit = Convert.ToDecimal(colODLimitTotal);

                if (ntmp > ntmpodlimit)
                {
                    return Color.Red;
                }
            }
            if (colTabGroup == "CD" && colMatMethod == "Single" && colMatDt <= GlobalVars.SystemDate)
            {
                return Color.Red;
            }
            // Loans
            // Check for delinquency
            if (StringHelper.StrTrimX(colDelqDT) != ""
                && colDelqDT != "Closed")
            {
                return Color.Red;
            }
            else if ((colStatus == "NonAccrual")
            || (colStatus == "Active Charge-Off")
            || (colStatus == "Inactive Charge-Off"))
            {
                return Color.Blue;
            }
            // Closed account charged off
            if (StringHelper.StrTrimX(colChargedOff) != ""
                && StringHelper.StrTrimX(colChargedOff) == "Y"
                && colChargedOff == "Closed")
            {
                return Color.Red;
            }

            return Color.Empty;
        }
        //End Task #102896

        public void ShowGrid()
        {
            this.gridControlAccounts.Visible = true;
        }

        public async Task<DataTable> GetListOfAccounts(int customerNo)
        {
            BusObjectBase busObject = BusObjHelper.MakeClientObject("CUSTOMER_SEARCH");

            busObject.ActionType = XmActionType.ListView;

            FieldBase fieldRimNo = busObject.GetFieldByXmlTag("RimNo");
            FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            FieldBase fieldDepLoan = busObject.GetFieldByXmlTag("DepLoan");
            FieldBase fieldIncludeClosed = busObject.GetFieldByXmlTag("IncludeClosed");
            FieldBase fieldRequestMode = busObject.GetFieldByXmlTag("RequestMode");
            FieldBase fieldShowAcctNickname = busObject.GetFieldByXmlTag("ShowAcctNickname");
            FieldBase fieldShowTitles = busObject.GetFieldByXmlTag("ShowTitles");  //Task#87721

            //
            fieldRimNo.Value = customerNo;
            fieldOutputType.Value = 34;    //Task#87721: was 3 
            fieldDepLoan.Value = "All";    //Task#87721: was ALL 
            fieldIncludeClosed.Value = "N";
            fieldRequestMode.Value = "A"; // "P"; //Task#95882
            fieldShowAcctNickname.Value = "Y";
            fieldShowTitles.Value = "Y";    //Task#87721
            //
            var asyncCDS = CoreService.DataService.GetAsyncCDS();
            var response = await asyncCDS.GetListViewAsync(new System.Threading.CancellationToken(), busObject);
            //
            return ucCustomer.ResponseToDataTable(response, "CUSTOMER_SEARCH", true);
        }

        public void ClearData()
        {
            this.gridControlAccounts.DataSource = null;
            _invalidRows.Clear(); //Task #102896
            HideAlerts(); // Bug #92490

            //Begin 122356    
            _signatureAlert.Caption = "";
            _signatureAlert.Visible = false;
            _iconSeparator.Visible = false;
            //End 122356
        }

        //Begin Bug #92490
        public void HideAlerts()
        {
            if (_alerts != null && _acctAlerts != null)
            {
                _alerts.HideAcctAlerts(_acctAlerts);
            }
        }
        //End Bug #92490

        /// <summary>
        /// Parent Window that implements ITellerWindow interface
        /// </summary>
        ITellerWindow ParentWindow
        {
            get
            {
                if (_parentWindow == null)
                {
                    _parentWindow = WinHelper.GetParent<ITellerWindow>(this);
                }
                return _parentWindow;
            }
        }

        public void InitializeTeller(ITellerWindow parentWindow)
        {
            _parentWindow = parentWindow;
            _parentDockPanel = WinHelper.GetParentDockingControl(this);

            //if (_parentDockPanel != null)
            //{
            //    //_btnRelationShip = new DevExpress.XtraBars.Docking.CustomHeaderButton("Relationship", ((System.Drawing.Image)(resources.GetObject("panelCustomer.CustomHeaderButtons1"))), -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
            //    //_btnSearch = new DevExpress.XtraBars.Docking.CustomHeaderButton("Search", ((System.Drawing.Image)(resources.GetObject("panelCustomer.CustomHeaderButtons"))), -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
            //    //_parentDockPanel.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _btnRelationShip, _btnSearch });
            //}

            //Vidya
            gridControlAccounts.SetDefaultProperties(gridViewAccounts);
            gridViewAccounts.OptionsBehavior.Editable = false;
            gridViewAccounts.OptionsBehavior.ReadOnly = false;
            gridViewAccounts.OptionsBehavior.KeepFocusedRowOnUpdate = true;
            gridViewAccounts.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewAccounts.OptionsSelection.EnableAppearanceFocusedRow = true;
            gridViewAccounts.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

            foreach (Control control in this.Controls)
            {
                control.GotFocus += Control_GotFocus;
            }

            //Begin 122356
            _signatureAlert = new CustomHeaderButton("", null, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, false, false, true, null, null, null, -1);
            _signatureAlert.Enabled = false;
            _signatureAlert.UseCaption = true;
            _signatureAlert.Caption = "";
            _signatureAlert.ToolTip = "Multiple signatures are required to withdraw funds from this account";

            _iconSeparator = new CustomHeaderButton("", null, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", true, -1, true, null, false, false, true, null, null, null, -1);
            _iconSeparator.Enabled = false;
            _iconSeparator.UseCaption = true;
            _iconSeparator.Caption = "|";
            //End 122356

            _btnInvisible = WinHelper.GenerateInvisibleButton(16); //#99048
            _parentDockPanel.CustomHeaderButtons.Add(_btnInvisible); //#99048

            gridViewAccounts.Appearance.FocusedRow.ForeColor = Color.White;                        //121714-2B
            gridViewAccounts.Appearance.FocusedRow.BackColor = Color.FromArgb(255, 0, 120, 215);   //121714-2B
        }

        private void MoveInvisibleHeaderButtonToEnd()
        {
            if (_parentDockPanel != null)
            {
                if (_parentDockPanel.CustomHeaderButtons.Contains(_btnInvisible))
                {
                    _parentDockPanel.CustomHeaderButtons.Remove(_btnInvisible);
                    _parentDockPanel.CustomHeaderButtons.Add(_btnInvisible);
                }
            }
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            _parentWindow.LastActiveControl = sender as Control;
        }

        private void InitializeAlerts()
        {
            if (_alerts != null)    // already initialized 
                return;

            _alerts = new QuickTranAlerts();
            _alerts.Add(AlertsNames.NSF, null);
            _alerts.Add(AlertsNames.UCF, null);
            _alerts.Add(AlertsNames.REJ, null);
            _alerts.Add(AlertsNames.Stop, null);
            _alerts.Add(AlertsNames.Hold, null);
            _alerts.Add(AlertsNames.Caution, null);
            _alerts.Add(AlertsNames.AcctNotes, null);
            _alerts.Add(AlertsNames.AcctCrossRef, null);
            _alerts.Add(AlertsNames.Sweep, null);
            _alerts.Add(AlertsNames.Analysis, null);
            _alerts.Add(AlertsNames.Retirement, null);
            _alerts.Add(AlertsNames.AcctRegD, null);
            _alerts.Add(AlertsNames.AdvRestrict, null);
            _alerts.Add(AlertsNames.AcctBankrupt, null);

            _alerts.Initialize(_parentWindow.TlTranCodeWindow as PfwStandard, _parentDockPanel, MoveInvisibleHeaderButtonToEnd);

            //Begin 122356
            _parentDockPanel.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _iconSeparator, _signatureAlert });
            _signatureAlert.Visible = false;
            _iconSeparator.Visible = false;
            //End 122356
        }

        private void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;
            // Check if the right mouse button has been pressed 
            GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
            if (hitInfo.InRowCell)
            {
                view.SelectRow(hitInfo.RowHandle);
                view.FocusedRowHandle = hitInfo.RowHandle;
                if (e.Button == MouseButtons.Right || hitInfo.Column == colTranType) //Task #115710
                {
                    DoShowMenu(hitInfo);
                    (e as DXMouseEventArgs).Handled = true;
                }
            }
        }

        //These methods make the Inline editing and editform work together.
        private void gridView1_ShownEditor(object sender, EventArgs e)
        {
            gridViewAccounts.ActiveEditor.MouseDown -= ActiveEditor_MouseDown;
            gridViewAccounts.ActiveEditor.MouseDown += ActiveEditor_MouseDown;
        }

        private void ActiveEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                gridViewAccounts.PostEditor();
                gridViewAccounts.UpdateCurrentRow();
                gridViewAccounts.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;
                DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                gridViewAccounts.ShowEditForm();

                //Do Not reset to inplace editing here but do it in UPDATE or CANCEL so the inplae editing will work normally else inplace will NOT work after the editform is disposed.
                //gridView1.OptionsBehavior.EditingMode = GridEditingMode.Default;
            }
        }

        void DoShowMenu(GridHitInfo hi)
        {
            // Create the menu. 
            GridViewMenu menu = null;
            // Check whether the header panel button has been clicked. 
            if (hi.HitTest == DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitTest.RowCell)
            {
                menu = new TransactionsMenu(hi.View, hi.RowHandle, _parentWindow, this);

                menu.Init(hi);
                // Display the menu. 

                //Begin 124297
                //menu.Show(hi.HitPoint);
                menu.ShowPopup(gridControlAccounts, hi.HitPoint);
                //End 124297

                SendKeys.SendWait("{DOWN}");  //122355-#3
            }
        }
        private void OnCalcRowHeight(object sender, RowHeightEventArgs e)
        {
            if (e.RowHeight > _gridRowHeight)
                _gridRowHeight = e.RowHeight;
        }


        //Begin Bug #98671
        void DoShowMenu(int rowHandle, int visibleRowHandle = -1)
        {
            if (gridViewAccounts.FocusedColumn == null)
                return;
            // Create the menu. 
            GridViewMenu menu = new TransactionsMenu(gridViewAccounts, rowHandle, _parentWindow, this);
            int x = 0;
            int rowHeight = this._gridRowHeight;
            foreach (GridColumn col in gridViewAccounts.Columns)
            {
                if (col.Caption == gridViewAccounts.FocusedColumn.Caption)
                {
                    x += col.VisibleWidth / 2;
                    break;
                }
                else
                {
                    x += col.VisibleWidth;
                }
            }
            visibleRowHandle = visibleRowHandle >= 0 ? visibleRowHandle : rowHandle;
            var view = gridViewAccounts.FocusedColumn.View;
            var bounds = gridViewAccounts.GridControl.Bounds;
            int y = bounds.Top + ((1 + visibleRowHandle) * rowHeight) + rowHeight / 2;
            Point point = new Point(x, y);
            menu.Init(view);

            //Begin 124297
            //menu.Show(point);
            menu.ShowPopup(gridControlAccounts, point);
            //End 124297

            SendKeys.SendWait("{DOWN}");  //122355-#3
        }
        //End Bug #98671

        private void gridView1_RowClick(object sender, RowClickEventArgs e)
        {
            //if (gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colAcctNo).ToString().Trim().Substring(0, 3) == "CDA")
            if (gridViewAccounts.GetRowCellValue(e.RowHandle, colAcctNo).ToString().Trim().Substring(0, 3) == "CDA")
            {
                //gridView1.OptionsEditForm.CustomEditFormLayout = new DepositRowEditor();
                //gridView1.OptionsEditForm.CustomEditFormLayout = new RowEdit(gridView1.GetDataRow(gridView1.FocusedRowHandle).ItemArray);
            }
            else
            {
                //gridView1.OptionsEditForm.CustomEditFormLayout = new LoanRowEditor();
                //gridView1.OptionsEditForm.CustomEditFormLayout = new RowEdit(gridView1.GetDataRow(gridView1.FocusedRowHandle).ItemArray);
            }
            //gridView1.OptionsBehavior.EditingMode = GridEditingMode.EditFormInplace;

            //gridView1.GetDataRow(gridView1.FocusedRowHandle).ItemArray[2]

            //Begin Task#89976
            _parentWindow.SetQuickAccountSelectedRow(gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle));
            ShowAlerts();
            //End Task#89976
        }

        private void gridView1_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            //Do Cell level validations here.

            //GridView view = sender as GridView;
            //if (view.FocusedColumn == colCreateDt)
            //{
            //    DateTime? CreateDate = gridView1.GetRowCellValue(view.FocusedRowHandle, colCreateDt) as DateTime?;

            //    if (CreateDate > DateTime.Today)
            //    {
            //        e.Valid = false;
            //        e.ErrorText = "Create Date is greater than the today date";
            //    }
            //}
        }

        private void gridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            //Do Row level validations here.  This happens when the user clicks UPDATE in popup editor or moves focus to another row in inplace editing mode.

            //Ideally this event need not be used as the BO should do the validations

            //Example
            DateTime? CreateDate = gridViewAccounts.GetRowCellValue(e.RowHandle, "CreateDt") as DateTime?;

            if (CreateDate > DateTime.Today)
            {
                e.Valid = false;
                e.ErrorText = "Create Date is greater than the today date";
            }
        }

        private void gridView1_EditFormShowing(object sender, EditFormShowingEventArgs e)
        {
            dr = gridViewAccounts.GetDataRow(e.RowHandle);
            //ucREdit = new ucRowEdit(dr);
            //gridView1.OptionsEditForm.CustomEditFormLayout = ucREdit;
        }

        //subscribe to UPDATE and CANCEL button click event of the editform
        private void gridView1_EditFormPrepared(object sender, EditFormPreparedEventArgs e)
        {
            e.Panel.Controls[1].Controls[1].Click += UcAccounts_EditForm_Update_Click;
            e.Panel.Controls[1].Controls[2].Click += UcAccounts_EditForm_Cancel_Click;
        }

        private void UcAccounts_EditForm_Update_Click(object sender, EventArgs e)
        {
            gridViewAccounts.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
        }

        private void UcAccounts_EditForm_Cancel_Click(object sender, EventArgs e)
        {
            gridViewAccounts.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.Default;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //gridView1.OptionsNavigation.UseTabKey = false;
            KeyEventArgs ka;
            TransactionTypeDefinition tranDef = ProcessMenuItem(keyData);  //87759
            bool keyHandled = false; //Bug #101592
            switch (keyData)
            {
                //Begin Bug #98671
                case (Keys.Space): //Task #115710
                case (Keys.Right): //Bug #98671 //Task# 115070 case (Keys.Control | Keys.M):
                    if (gridViewAccounts.FocusedRowHandle >= 0)
                    {
                        int firstVisibleRow = 0;
                        bool rowVisible = false;
                        while (!rowVisible && firstVisibleRow < gridViewAccounts.RowCount)
                        {
                            var rowVisibleState = gridViewAccounts.IsRowVisible(firstVisibleRow);
                            rowVisible = rowVisibleState == RowVisibleState.Partially || rowVisibleState == RowVisibleState.Visible;
                            firstVisibleRow++;
                        }
                        int gridRowIndex = Math.Max(0, gridViewAccounts.FocusedRowHandle - firstVisibleRow + 1);
                        DoShowMenu(gridViewAccounts.FocusedRowHandle, gridRowIndex);
                        keyHandled = true; //Task #105856
                    }
                    break;
                //End Bug #98671
                default:
                    if (tranDef != null)
                    {
                        //After performing the quick action, put focus on the Transaction panel.
                        ka = new KeyEventArgs(tranDef.ShortcutKey);
                        ((PDxDockPanel)(this._parentDockPanel)).OnPanelKeyDown(ka);
                        keyHandled = true; //Task #105856
                    }
                    break;
            }
            //else
            //{
            //    ka = new KeyEventArgs(keyData);
            //}

            if (keyHandled)
            {
                base.ProcessCmdKey(ref msg, keyData); //Bug #101592
                return keyHandled; //Bug #101592
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        protected TransactionTypeDefinition ProcessMenuItem(Keys keyData)  //87759
        {
            TransactionTypeDefinition a = null;  //87759

            //Find the tran type based on shortcut key pressed
            a = TranHelper.Instance.TranTypeList.Find(x => x.ShortcutKey == keyData);
            a = (a == null) ? TranHelper.Instance.TranTypeList.Find(x => x.SecondShortcutKey == keyData) : a; // Task# 115070

            if (a == null)
                return null;

            EasyCaptureTran tran = new EasyCaptureTran();

            tran.TranType = TranHelper.Instance.TranList.Find(x => x.Description == a.Description).TranType; //87759

            DataRow row = gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle);

            //if (row == null) return null;   //#87755

            //Begin 87753
            if (a.DpLN == "RM")
            {
                if (row == null)
                    tran.AcctNo = rimNo;
                else
                    tran.AcctNo = row.GetStringValue("RimNo");
                tran.AcctType = "RIM";
            }
            else if (row != null)
            {
                tran.AcctNo = row.GetStringValue("AcctNo");
                tran.AcctType = row.GetStringValue("AcctType");
            }
            //End 87753

            tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == a.Description); //87759

            if (row != null)    //#87755
            {
                if ((row.GetStringValue("DepLoan") + "").Trim() != a.DpLN && a.DpLN != "RM" && (!(row.GetStringValue("DepLoan") == "EXT" && a.DpLN == "EX"))) //87751-2
                    return null;

                //Begin 87751
                if (a.DpLN == "EX")
                {
                    if (row.GetStringValue("DepLoan") == "EXT" && row.GetStringValue("Adapter") != "Y")
                        return null;
                }

                //Begin 95598
                if (a.DpLN == "DP" && row.GetStringValue("ApplType").Trim() == "CD")
                {
                    return null;
                }
                //End 95598

                tran.SelectedAcctNo = row.GetStringValue("AcctNo");
                tran.SelectedAcctType = row.GetStringValue("AcctType");
                tran.SelectedDepLoan = ((string)row["DepLoan"] + "").Trim();
                //End 87751

                if (tran.TranType == EasyCaptureTranType.Deposit && tran.TranDef != null)   //#87731-1
                {
                    if (row.GetStringValue("Status") == "Unfunded")
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.OpeningDeposit.ToString());
                    else
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.Deposit.ToString());

                    //End 87759
                }
            }
            if (a.DpLN == "RM" || row != null)  //#87755
                _parentWindow.TransactionSet.Transactions.Add(tran);

            return a;
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {


        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow row = gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle);

            if (row != null)
            {
                ShowAlerts();
                //Begin Task#89976
                _parentWindow.SetQuickAccountSelectedRow(row);
                //End Task#89976
            }
        }

        public void ShowAlerts()
        {
            DataRow row = gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle);
            if (row != null)
            {
                if (_alerts != null)
                {
                    _acctAlerts = GetAlertInfo(row.GetStringValue("AcctType"), row.GetStringValue("AcctNo"));
                    _alerts.ShowAcctAlerts(_acctAlerts); //Task#89967
                }
            }
        }

        public bool HasFocus()
        {
            return this.Focused || gridControlAccounts.IsFocused || gridControlAccounts.Focused;
        }

        public AcctAlerts GetAlertInfo(string acctType, string acctNo)
        {
            AcctAlerts acctAlert = new AcctAlerts();

            if (string.IsNullOrEmpty(acctType) || string.IsNullOrEmpty(acctNo))
                return acctAlert;

            DataRow row = gridViewAccounts.GetDataRow(gridViewAccounts.FocusedRowHandle);

            bool rowFound = (row.GetStringValue("AcctType") == acctType && row.GetStringValue("AcctNo") == acctNo);

            if (!rowFound)
            {
                var rows = _parentWindow.Accounts.Rows;
                for (int i = 0; i < rows.Count; i++)
                {
                    row = rows[i];
                    rowFound = (row.GetStringValue("AcctType") == acctType && row.GetStringValue("AcctNo") == acctNo);
                    if (rowFound)
                        break;
                }
            }

            //Begin Task 89995
            if (!rowFound)
            {
                string formattedAcctNo = TranHelper.Instance.FormatAcctNo(acctNo).ToString();
                DataTable table = _parentWindow.GetDetailCustomerByAccountSync(formattedAcctNo, acctType);
                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        string rowAcctNo = table.Rows[i].GetStringValue("AcctNo");
                        string rowFormattedAcctNo = TranHelper.Instance.FormatAcctNo(rowAcctNo).ToString();
                        if (rowFormattedAcctNo == formattedAcctNo)
                        {
                            row = table.Rows[i];
                            rowFound = true;
                            break;
                        }
                    }
                }
            }
            //End Task 89995

            if (!rowFound)
                return acctAlert;

            acctAlert = GetAlertInfo(row);

            return acctAlert;
        }

        public AcctAlerts GetAlertInfo(DataRow row)
        {
            AcctAlerts acctAlert = new AcctAlerts();

            acctAlert.AcctType = row.GetStringValue("AcctType");
            acctAlert.AcctNo = row.GetStringValue("AcctNo");
            acctAlert.RimNo = row.GetIntValue("RimNo");

            acctAlert.NSF = row.GetStringValue("Nsf") == "Y";
            acctAlert.UCF = row.GetStringValue("Ucf") == "Y";
            acctAlert.REJ = row.GetStringValue("Rejected") == "Y";
            acctAlert.Stop = row.GetStringValue("Stops") == "Y";

            string depLoan = row.GetStringValue("DepLoan").Trim(); //Bug #95593

            //Begin Bug#89991
            acctAlert.Hold = false;
            if (depLoan.Contains("DP") || depLoan.Contains("LN"))
                acctAlert.Hold = row.GetDecimalValue("HoldBal") > 0;
            //End Bug#89991

            acctAlert.Caution = row.GetStringValue("Cautions") == "Y";
            acctAlert.Analysis = row.GetStringValue("Analysis") == "Y";
            acctAlert.CrossRef = row.GetStringValue("CrossRef") == "Y";
            acctAlert.Sweep = !string.IsNullOrEmpty(row.GetStringValue("Sweeps")) && row.GetStringValue("Sweeps") != "N";
            acctAlert.AcctNotesCount = row.GetIntValue("AcctNotesCount");
            acctAlert.AcctRegD = row.GetStringValue("AcctRegD") == "Y";
            acctAlert.AcctBankrupt = row.GetStringValue("AcctBankrupt") == "Y";
            acctAlert.AdvRestrict = row.GetStringValue("AdvRestrict") == "Y";
            acctAlert.Retirement = !string.IsNullOrEmpty(row.GetStringValue("PlanNo")) && !string.IsNullOrEmpty(row.GetStringValue("PlanNo")) && row.GetStringValue("PlanNo").Trim().ToLower() != "n"; //Bug #95971

            //Begin Bug #95593
            int analId = 0;
            int analRimNo = 0;
            int sweeps = 0;

            int.TryParse(row.GetStringValue("AnalysisID"), out analId);
            int.TryParse(row.GetStringValue("AnalysisRimNo"), out analRimNo);
            int.TryParse(row.GetStringValue("Sweeps"), out sweeps);

            acctAlert.DepLoan = depLoan;
            acctAlert.CurAnalId = analId;
            acctAlert.AnalysisRimNo = analRimNo;
            acctAlert.SweepPtid = sweeps;
            //End Bug #95593

            //Begin 122356
            if (_parentWindow.IsFocusOnPanel())
            {
                if ((Convert.ToInt32(row.GetStringValue("NoSignatures")) > 1))
                {
                    _signatureAlert.Caption = row.GetStringValue("NoSignatures").Trim() + " signatures required to withdraw funds";
                    _signatureAlert.Visible = true;
                    _iconSeparator.Visible = true;
                    _signatureAlert.Enabled = true;
                    _signatureAlert.Appearance.Options.UseForeColor = true;
                    _signatureAlert.Appearance.ForeColor = Color.Red;
                }
                else
                {
                    _signatureAlert.Caption = "";
                    _signatureAlert.Visible = false;
                    _iconSeparator.Visible = false;
                }
            }
            //End 122356

            return acctAlert;
        }

        //Begin #87725
        public void SetGridFocusedRow(string Account)
        {
            if (string.IsNullOrEmpty(Account))
                return;

            int row = gridViewAccounts.LocateByValue("AccountCombined", Account, null);
            if (row != int.MinValue)
            {
                gridViewAccounts.FocusedRowHandle = row;
                gridViewAccounts.SelectRow(row);
            }
        }
        //End #87725


        //Daniel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Account"></param>
        /// <returns></returns>
        public DataRow LocateAccountRow(string Account)
        {
            DataRow row = null;

            if (!string.IsNullOrEmpty(Account))
            {
                int rowHandle = gridViewAccounts.LocateByValue("AccountCombined", Account, null);

                if (rowHandle != int.MinValue)
                {
                    row = gridViewAccounts.GetDataRow(rowHandle);
                }
            }

            return row;
        }

        public void Destroy() ///Bug#93072
        {
            if (_alerts != null)
            {
                _alerts.Dispose();
            }
        }

        //Begin Task#100769
        private string FormatAmounts(string amountstring)
        {
            amountstring = (string.IsNullOrEmpty(amountstring) ? "0.00" : amountstring);
            amountstring = String.Format("{0:C}", Convert.ToDecimal(amountstring));
            return amountstring;
        }
        //End Task#100769
    }


    //Begin Bug #92489
    public class CustomerAccountsResponse
    {
        public bool Success { get; set; }
        public List<string> AccountNumbers { get; set; }
    }
    //End Bug #92489


    //public class GridViewColumnButtonMenu : GridViewMenu
    //{
    //    int _rowHandle;
    //    ITellerWindow _parentWindow;
    //    private ucAccounts ucAcctsCaller = null;

    //    public GridViewColumnButtonMenu(DevExpress.XtraGrid.Views.Grid.GridView view, int rowHandle, ITellerWindow _parentWindow, ucAccounts ucAccts) : base(view) {
    //        _rowHandle = rowHandle;
    //        this._parentWindow = _parentWindow;
    //        this.ucAcctsCaller = ucAccts;
    //    }
    //    // Create menu items. 
    //    // This method is automatically called by the menu's public Init method. 
    //    protected override void CreateItems()
    //    {
    //        Items.Clear();
    //        //

    //        DataRow row = View.GetDataRow(_rowHandle);
    //        string rowDepLoan = ((string)row["DepLoan"] + "").Trim();
    //        string skipType = rowDepLoan == "DP"?"LN":"DP";

    //        foreach( var a in TranHelper.Instance.TranList)
    //        {
    //            if (a.DpLN == skipType) continue;
    //            Items.Add(CreateMenuItem(a.Combined, (Image) null, a, true));
    //        }
    //    }
    //    protected override void OnMenuItemClick(object sender, EventArgs e)
    //    {
    //        if (RaiseClickEvent(sender, null)) return;

    //        DXMenuItem item = sender as DXMenuItem;
    //        _parentWindow.MoveAcctToTran(item);
    //        //
    //        TransactionDefinition a = item.Tag as TransactionDefinition;
    //        if (a == null)
    //            return;

    //        EasyCaptureTran tran = new EasyCaptureTran();
    //        tran.TranType = a.TranType;

    //        DataRow row = this.View.GetDataRow(_rowHandle);
    //        if (a.DpLN == "RM")
    //        {
    //            //if (_parentWindow.Customers == null || _parentWindow.Customers.Rows.Count == 0)
    //            //    return;

    //            //var rimNo = _parentWindow.Customers.Rows[0]["RimNo"];
    //            tran.AcctNo = row.GetStringValue("RimNo");
    //            tran.AcctType = "RIM";
    //        }
    //        else
    //        {
    //            tran.AcctNo = row.GetStringValue("AcctNo");
    //            tran.AcctType = row.GetStringValue("AcctType");
    //        }

    //        tran.TranDef = a;
    //        _parentWindow.TransactionSet.Transactions.Add(tran);

    //        //Vidya - Move focus to tran grid
    //        KeyEventArgs ka = new KeyEventArgs(a.ShortcutKey);
    //        ((PDxDockPanel)(this.ucAcctsCaller._parentDockPanel)).OnPanelKeyDown(ka);
    //    }
    //}
}
