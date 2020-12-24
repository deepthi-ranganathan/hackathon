#region Comments
//-------------------------------------------------------------------------------
// File Name: ucCashIn.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
#endregion

#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//05/04/2018    2       FOyebola    Enh#220316 - Task 89907
//05/09/2018    3       mselvaga    Enh#220316 - Task#90561 - Quick Transaction - Stuck with "Cash In Count" screen
//05/09/2018    4       FOyebola    Enh#220316 - Task 87735
//05/11/2018    5       FOyebola    CVT#90914 - Dereference after null check
//07/30/2018    6       CRoy        Task #87737
//09/04/2018    7       CRoy        Bug #99048 - Cosmetic issues
//01/11/2019    8       DGarcia     Bug #108545 - Clear ChecksIn and # of Items a search is done.
//05/06/2019    9       CRoy        Task #114235 - Make sure that the $ sign doesn't get removed.
//07/11/2019    10      FOyebola    Task#114934
//08/21/2019    11      mselvaga    Task#118299 - ECM Voucher printing changes added
//11/20/2019    12      FOyebola    Task#121713
//12/04/2019    13      FOyebola    Task#122355
//02/20/2019    14      mselvaga    Task#124388 - ECM Voucher printing changes added

#endregion

using System;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Variables; //#89998

namespace Phoenix.Client.Teller
{

    public partial class ucCashIn : UserControl
    {
        ITellerWindow _parentWindow;
        public DockPanel _parentDockPanel;

        public ucCashIn()
        {
            InitializeComponent();
            dfMakeAvail.Enabled = false;
            //picDrawer.Location = new System.Drawing.Point(picTCD.Location.X, picTCD.Location.Y);
            //dfCashIn.TextChanged += CashInTextChanged; //#113631
            //dfMakeAvail.TextChanged += MakeAvailTextChanged; //121713-#3 //122355
            //dfChecksIn.TextChanged += DfChecksIn_TextChanged;   //#113631  //122355
        }

        //Begin 122355
        /*
        private void DfChecksIn_TextChanged(object sender, EventArgs e) //#113631
        {
            if (!dfChecksIn.Text.Contains("$") && !dfChecksIn.Focused)
            {
                dfChecksIn.UnFormattedValue = dfChecksIn.UnFormattedValue;
            }
        }
        */
        //End 122355

        //Begin Task #114235
        //private void CashInTextChanged(object sender, EventArgs e)    //#122728
        //{
        //    if (!dfCashIn.Text.Contains("$") && !dfCashIn.Focused)
        //    {
        //        dfCashIn.UnFormattedValue = dfCashIn.UnFormattedValue;
        //    }
        //}
        //End Task #114235

        //Begin 122355
        //Begin 121713-#3
        /*
        private void MakeAvailTextChanged(object sender, EventArgs e)
        {
            if (!dfMakeAvail.Text.Contains("$") && !dfMakeAvail.Focused)
            {
                dfMakeAvail.UnFormattedValue = dfMakeAvail.UnFormattedValue;
            }
        }
        */
        //End 121713-#3
        //End 122355

        public void InitializeTeller(ITellerWindow parentWindow)
        {
            _parentDockPanel = WinHelper.GetParentDockingControl(this);
            _parentWindow = parentWindow;
            if (_parentWindow != null)
            {
                _parentWindow.OnCustomerChanged += _parentWindow_OnCustomerChanged;

            }


            //Begin CVT#90914
            if (_parentWindow != null)
            {
                this.dfCashIn.DataBindings.Add(
                "Text", _parentWindow.TransactionSet, "CashIn", false, DataSourceUpdateMode.OnValidation);

                this.dfCashIn.DataBindings.Add("UnFormattedValue", _parentWindow.TransactionSet, "CashIn");


                this.dfChecksIn.DataBindings.Add(
                   "Text", _parentWindow.TransactionSet, "ChecksIn");   //#102719 //#113631  //122355

                this.dfChecksIn.DataBindings.Add("UnFormattedValue", _parentWindow.TransactionSet, "ChecksIn");   //#102719  //#113631  //122355

                this.dfMakeAvail.DataBindings.Add(
                   "Text", _parentWindow.TransactionSet, "ChecksMakeAvail");

                this.dfMakeAvail.DataBindings.Add("UnFormattedValue", _parentWindow.TransactionSet, "ChecksMakeAvail");   //122355

                this.dfNoOfItems.DataBindings.Add(
                   "Text", _parentWindow.TransactionSet, "NoItems");   //#113631

                this.dfNoOfItems.DataBindings.Add("UnFormattedValue", _parentWindow.TransactionSet, "NoItems");   //#122532
            }
            //End CVT#90914

            //Begin 89907

            foreach (Control control in this.Controls)
            {
                control.GotFocus += Control_GotFocus;
            }

            dfCashIn.GotFocus += Control_GotFocus;
            dfChecksIn.GotFocus += Control_GotFocus;
            dfNoOfItems.GotFocus += Control_GotFocus;

            //End 89907

            dfCashIn.PhoenixUIValidateEvent += DfCashIn_PhoenixUIValidateEvent1;
            dfChecksIn.PhoenixUIValidateEvent += dfChecksIn_PhoenixUIValidateEvent1;
            dfNoOfItems.PhoenixUIValidateEvent += dfNoOfItems_PhoenixUIValidateEvent1;
            picTCD.Click += PicTCD_Click;
            picDrawer.Click += PicDrawer_Click;
            dfCashIn.UnFormattedValue = 0.00;
            dfMakeAvail.UnFormattedValue = 0.00;  //121713-#3
            var btn = WinHelper.GenerateInvisibleButton(16);//#99048
            _parentDockPanel.CustomHeaderButtons.Add(btn);//#99048
            //#89998
            picTCD.Image = Phoenix.Shared.Images.TellerImages.TCDSmall;
            picDrawer.Image = Phoenix.Shared.Images.TellerImages.Drawer;
            this.picDrawer.Visible = false;
            this.picTCD.Visible = false;

            this.cbWaiveSignature.Visible = false;  //#90021

            cbWaiveSignature.CheckedChanged += CbWaiveSignature_CheckedChanged;
            cbWaiveSignature.Click += CbWaiveSignature_Click;
            cbWaiveSignature.Enter += CbWaiveSignature_Enter;
            cbWaiveSignature.Leave += CbWaiveSignature_Leave;
        }

        private void CbWaiveSignature_Enter(object sender, EventArgs e)
        {
            _parentWindow.HandleQuickTranStatusText("Select this check box to waive the signature requirement for all transactions.");
        }

        private void CbWaiveSignature_Leave(object sender, EventArgs e)
        {
            _parentWindow.HandleQuickTranStatusText(string.Empty);
        }

        private void CbWaiveSignature_Click(object sender, EventArgs e)
        {
            _parentWindow.HandleQuickTranStatusText("Select this check box to waive the signature requirement for all transactions.");
        }

        private void PicDrawer_Click(object sender, EventArgs e)
        {
            if (_parentWindow != null)
                _parentWindow.QuickTranTcdDrawerIconToggle(picTCD, picDrawer);
        }

        private void PicTCD_Click(object sender, EventArgs e)
        {
            if (_parentWindow != null)
                _parentWindow.QuickTranTcdDrawerIconToggle(picTCD, picDrawer);
        }

        public void FocusPanel()
        {
            dfCashIn.Focus();
        }

        private void DfCashIn_PhoenixUIValidateEvent1(object sender, Windows.Forms.PCancelEventArgs e)
        {
            decimal devDeposit = 0;
            if (TellerVars.Instance.IsTCDConnected && TellerVars.Instance.IsRecycler &&
                dfCashIn.UnFormattedValue != null)
            {
                devDeposit = _parentWindow.DeviceDeposit;
                if (devDeposit > 0 && devDeposit > Convert.ToDecimal(dfCashIn.UnFormattedValue))
                {
                    PMessageBox.Show(16100, MessageType.Warning, MessageBoxButtons.OK, devDeposit.ToString("$#,#0.00"));
                    dfCashIn.Focus();
                    dfCashIn.UnFormattedValue = devDeposit;
                    e.Cancel = false;
                    return;
                }
            }

            if (dfCashIn.UnFormattedValue == null)
                dfCashIn.UnFormattedValue = 0;

            if (Convert.ToDecimal(dfCashIn.UnFormattedValue) >= 1 && !string.IsNullOrEmpty(dfCashIn.Text) && dfCashIn.Text.Contains(".") && !Convert.ToString(dfCashIn.UnFormattedValue).Contains("."))  //114934
                dfCashIn.UnFormattedValue = Convert.ToDecimal(string.Format("{0}.00", dfCashIn.UnFormattedValue.ToString()));

            if (dfCashIn.UnFormattedValue != null && Convert.ToDecimal(dfCashIn.UnFormattedValue) < 0)
            {
                PMessageBox.Show(360188, MessageType.Warning, MessageBoxButtons.OK, new string[] { "Cash In cannot be less than zero." }); //Task #87737
                dfCashIn.Focus();
            }
            else if (dfCashIn.UnFormattedValue != null && !string.IsNullOrEmpty(dfCashIn.Text) && _parentWindow.TransactionSet.CashIn != Convert.ToDecimal(dfCashIn.UnFormattedValue))   //#90561
            {
                //PdfCurrency.ApplicationAssumeDecimal = true;
                //CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
                _parentWindow.TransactionSet.CashIn = Convert.ToDecimal(dfCashIn.UnFormattedValue);
                _parentWindow.PerformQuickTranActionClick("CashIn", false, null);  //#124249
            }
        }

        private void dfChecksIn_PhoenixUIValidateEvent1(object sender, Windows.Forms.PCancelEventArgs e)    //#113631
        {
            if (dfChecksIn.UnFormattedValue == null)
                dfChecksIn.UnFormattedValue = 0;

            if (Convert.ToDecimal(dfChecksIn.UnFormattedValue) >= 1 && !string.IsNullOrEmpty(dfChecksIn.Text) && dfChecksIn.Text.Contains(".") && !Convert.ToString(dfChecksIn.UnFormattedValue).Contains("."))  //114934
                dfChecksIn.UnFormattedValue = Convert.ToDecimal(string.Format("{0}.00", dfChecksIn.UnFormattedValue.ToString()));

            if (dfChecksIn.UnFormattedValue != null && Convert.ToDecimal(dfChecksIn.UnFormattedValue) < 0)
            {
                PMessageBox.Show(360188, MessageType.Warning, MessageBoxButtons.OK, new string[] { "Checks In cannot be less than zero." });
                dfChecksIn.Focus();
            }
            else if (dfChecksIn.UnFormattedValue != null && !string.IsNullOrEmpty(dfChecksIn.Text) && _parentWindow.TransactionSet.ChecksIn != Convert.ToDecimal(dfChecksIn.UnFormattedValue))
            {
                _parentWindow.TransactionSet.ChecksIn = Convert.ToDecimal(dfChecksIn.UnFormattedValue);
            }
        }

        private void dfNoOfItems_PhoenixUIValidateEvent1(object sender, Windows.Forms.PCancelEventArgs e)    //#113631
        {
            if (dfNoOfItems.UnFormattedValue == null)
                dfNoOfItems.UnFormattedValue = 0;

            if (dfNoOfItems.UnFormattedValue != null && Convert.ToInt16(dfNoOfItems.UnFormattedValue) < 0)
            {
                PMessageBox.Show(360188, MessageType.Warning, MessageBoxButtons.OK, new string[] { "Number of Items cannot be less than zero." });
                dfNoOfItems.Focus();
            }
        }

        private void Control_GotFocus(object sender, EventArgs e)
        {
            //_parentWindow.LastActiveControl = sender as Control; //87735

            _parentWindow.ChildControlFocused(sender); //#89907
        }

        private void _parentWindow_OnCustomerChanged()
        {
            //this.repItemAccounts.DataSource = _parentWindow.Accounts;
        }

        private void CbWaiveSignature_CheckedChanged(object sender, EventArgs e)
        {
            //if (!_parentWindow.IsWaiveSigWarningShown)
            //{
            //    if (DialogResult.No == PMessageBox.Show(this, 361056, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
            //    {
            //        _parentWindow.IsWaiveSigWarningShown = true;
            //        if (cbWaiveSignature.Checked)
            //            cbWaiveSignature.Checked = false;
            //        else
            //            cbWaiveSignature.Checked = true;
            //    }
            //}

            _parentWindow.TransactionSet.WaiveSignature = (cbWaiveSignature.Checked ? "Y" : "N");   //#90021
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Hooked to the context menu click in the Accounts grid via the main form
        public void AddNewTran(EasyCaptureTran tran)
        {
            return;
        }

        public void ResetCashInInfo()
        {
            this.dfCashIn.Text = string.Empty;
            dfCashIn.UnFormattedValue = 0.00;
            this.dfChecksIn.Text = string.Empty;
            dfChecksIn.UnFormattedValue = 0.00;
            this.dfNoOfItems.Text = string.Empty;
            dfNoOfItems.UnFormattedValue = 0;
            //Begin 121713-#3
            this.dfMakeAvail.Text = string.Empty;
            dfMakeAvail.UnFormattedValue = 0;
            //End 121713-#3

            this.picDrawer.Visible = false;
            this.picTCD.Visible = false;

            this.cbWaiveSignature.Checked = false;  //#90021
            this.cbWaiveSignature.Visible = false;
        }

        //Begin Bug #108545
        public void ResetChecksAndNoItemsInfo()
        {
            this.dfChecksIn.Text = string.Empty;
            dfChecksIn.UnFormattedValue = 0.00;
            this.dfNoOfItems.Text = string.Empty;
            dfNoOfItems.UnFormattedValue = 0;
            //Begin 121713-#3
            this.dfMakeAvail.Text = string.Empty;
            dfMakeAvail.UnFormattedValue = 0.00;
            //End 121713-#3
        }


        //End Bug #108545
        public void ToggleTCDIcon()
        {
            if (_parentWindow != null && (picTCD.Visible || picDrawer.Visible))
                _parentWindow.QuickTranTcdDrawerIconToggle(picTCD, picDrawer);
        }

        public void EnableDisableControls(string origin)
        {
            if (_parentWindow != null)
            {
                //#113631
                //The following enable/disable logic modified for ChecksIn and NoOfItems fields to permanently disable the fields until single item capture feature added to MT tab.
                //dfChecksIn.Enabled = !_parentWindow.IsQuickTranRealTime;
                //dfNoOfItems.Enabled = !_parentWindow.IsQuickTranRealTime;

                dfChecksIn.Enabled = false;
                dfNoOfItems.Enabled = false;
            }

            if (origin == "HandleTCDIcon")
            {
                picDrawer.Visible = TellerVars.Instance.IsTCDEnabled && !TellerVars.Instance.IsTCDConnected;
                if (picDrawer.Visible)
                {
                    picTCD.SendToBack();
                    picDrawer.BringToFront();
                }
                picTCD.Visible = TellerVars.Instance.IsTCDEnabled && TellerVars.Instance.IsTCDConnected;
                if (picTCD.Visible)
                {
                    picDrawer.SendToBack();
                    picTCD.BringToFront();
                }
                if (!TellerVars.Instance.IsTCDEnabled)
                {
                    picTCD.Visible = picDrawer.Visible = false;
                }
            }
            else if (origin == "HandleWaiveSig")    //#90021
            {
                cbWaiveSignature.Visible = TellerVars.Instance.IsDriveThruWorkstation != 1 && (TellerVars.Instance.IsHylandVoucherAvailable || TellerVars.Instance.IsECMVoucherAvailable);  //#118299 #124388
            }
        }

        ////This fires when combobox is opened.  Can be used to populate values in the dropdown
        //private void gvTransactions_ShownEditor(object sender, EventArgs e)
        //{
        //    DataRow currTranRow = gvTransactions.GetFocusedDataRow();
        //    //gridView2.SetRowCellValue(gridView2.GetSelectedRows()[0], "gridColAccount", currTranRow.Field());
        //}

        //Fires when a row in dropdown lookup is seleted
        //private void repItemAccounts_EditValueChanged(object sender, EventArgs e)
        //{
        //    LookUpEdit edit = sender as LookUpEdit;
        //    if (edit != null)
        //    {
        //        var test = edit.EditValue;
        //        DataRowView row = edit.GetSelectedDataRow() as DataRowView;
        //        if (row != null && row.Row != null)
        //        {
        //            string acctType = row.Row.GetStringValue("AcctType");
        //            string acctNo = row.Row.GetStringValue("AcctNo");
        //            string dpLN = row.Row.GetStringValue("DepLoan");
        //            EasyCaptureTran rowValue = GetSelectedRow();
        //            if (rowValue != null)
        //            {
        //                rowValue.AcctNo = row.Row.GetStringValue("AcctNo");
        //                rowValue.AcctType = row.Row.GetStringValue("AcctType");
        //            }
        //        }

        //    }
        //}


        //private EasyCaptureTran GetSelectedRow()
        //{
        //    if (gvTransactions.SelectedRowsCount == 1)
        //    {
        //        return gvTransactions.GetRow(gvTransactions.GetSelectedRows()[0]) as EasyCaptureTran;
        //    }
        //    return null;


        //}
        //private void repItemTC_EditValueChanged(object sender, EventArgs e)
        //{
        //    LookUpEdit edit = sender as LookUpEdit;

        //    if (edit != null)
        //    {
        //        var test = edit.EditValue;
        //        TransactionDefinition selectedTranDefRow = edit.GetSelectedDataRow() as TransactionDefinition;
        //        EasyCaptureTran selctedTranRow = GetSelectedRow();
        //        if (selctedTranRow != null)
        //        {
        //            selctedTranRow.TranDef = selectedTranDefRow;
        //        }

        //    }
        //}

        //private void gvTransactions_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        //{
        //    int rowHandle = e.RowHandle;
        //    DataRow row = gvTransactions.GetDataRow(rowHandle);

        //}

        //private void gvTransactions_KeyDown(object sender, KeyEventArgs e)
        //{

        //}

        //private void pbAddNewRow_Click(object sender, EventArgs e)
        //{
        //    //_parentWindow.MapEasyCaptureToTranSetAsync(true);
        //    _parentWindow.TransactionSet.AddNewTransaction();

        //}

        //private void pbDeleteCurrentRow_Click(object sender, EventArgs e)
        //{
        //    gvTransactions.DeleteSelectedRows();

        //}

        //private void pbMapTran_Click(object sender, EventArgs e)
        //{
        //    _parentWindow.MapEasyCaptureToTranSetAsync(true);
        //}

        //public void SetErrorInfo(string error)
        //{

        //    if ( !string.IsNullOrEmpty(error))
        //    {
        //        dfErrorInfo.ForeColor = System.Drawing.Color.Yellow;
        //        dfErrorInfo.BackColor = System.Drawing.Color.Red;
        //    }
        //    else
        //    {
        //        dfErrorInfo.ForeColor = Control.DefaultForeColor;
        //        dfErrorInfo.BackColor = System.Drawing.SystemColors.Control;
        //    }
        //    dfErrorInfo.Text = error;
        //}

        //public void ClearErrorInfo()
        //{
        //    dfErrorInfo.Text = string.Empty;
        //}


        public void UpdateCheckAmount()
        {
            dfChecksIn.Text = _parentWindow.TransactionSet.ChecksIn.ToString("#.##");
            dfNoOfItems.Text = Convert.ToString(_parentWindow.TransactionSet.NoItems);  //#122532
            dfMakeAvail.Text = _parentWindow.TransactionSet.ChecksMakeAvail.ToString("#.##");   //#122532
        }

        public void FocusCashInField()
        {
            dfCashIn.Focus();
        }
    }
}
