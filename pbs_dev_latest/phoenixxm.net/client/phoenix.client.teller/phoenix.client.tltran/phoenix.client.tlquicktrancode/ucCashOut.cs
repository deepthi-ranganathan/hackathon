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
//05/09/2018    3       FOyebola    Enh#220316 - Task 87735
//05/11/2018    4       FOyebola    CVT#90912 - Dereference after null check
//06/29/2018    5       DGarcia     Enh#220316 - Bug#93951
//07/30/2018    6       CRoy        Task #87737
//08/29/2018    7       CRoy        Bug #99165
//07/11/2019    8       FOyebola    Task#114934
//11/19/2019    9       FOyebola    Task#121713
//12/17/2019    10      mselvaga    Bug#122728 -  MT Tran tab difference field could not handle bigger/negative values.
#endregion



using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Variables; //#89998

namespace Phoenix.Client.Teller
{
    public partial class ucCashOut : UserControl
    {
        public ucCashOut()
        {
            InitializeComponent();
            SetFieldsBackgroundColor(); //Bug #99165
            //#89907: Enabled for testing the Calculator value assignment 
            dfCashOut.Enabled = true;
            lblCashOut.Enabled = true;
        }

        //Begin Bug #99165
        private void SetFieldsBackgroundColor()
        {
            PdfStandard[] fields = new PdfStandard[] {
                dfTotalCR,
                dfTotalDebits,
                dfDifference,
            };

            foreach (var field in fields)
            {
                field.BackColor = ThemeHelper.WindowBgColor; //  Color.FromArgb(235, 236, 239);
            }
        }
        //End Bug #99165

        ITellerWindow _parentWindow;
        public DockPanel _parentDockPanel;
        public void InitializeTeller(ITellerWindow parentWindow)
        {
            _parentDockPanel = WinHelper.GetParentDockingControl(this);
            _parentWindow = parentWindow;
            if (_parentWindow != null)
            {
                _parentWindow.OnCustomerChanged += _parentWindow_OnCustomerChanged;

            }


            //Begin CVT#90912
            if (_parentWindow != null)
            {
                //87735: Uncommented
                //#89907: Temporary commented the binding of the Cash Out field to avoid the validation carried out.
                this.dfCashOut.DataBindings.Add(
                "Text", _parentWindow.TransactionSet, "CashOut");

                this.dfTotalCR.DataBindings.Add(
                "Text", _parentWindow.TransactionSet, "TotalCredits");

                this.dfTotalDebits.DataBindings.Add(
                 "Text", _parentWindow.TransactionSet, "TotalDebits");

                this.dfDifference.DataBindings.Add(
                "Text", _parentWindow.TransactionSet, "TotalDifference");

                //Begin Bug#93951
                this.dfCashOut.DataBindings.Add(
                "UnFormattedValue", _parentWindow.TransactionSet, "CashOut");

                this.dfTotalCR.DataBindings.Add(
                "UnFormattedValue", _parentWindow.TransactionSet, "TotalCredits");

                this.dfTotalDebits.DataBindings.Add(
                 "UnFormattedValue", _parentWindow.TransactionSet, "TotalDebits");

                this.dfDifference.DataBindings.Add(
                "UnFormattedValue", _parentWindow.TransactionSet, "TotalDifference");
                //End Bug#93951
                this.cbDenominate.Visible = false;  //#89998
            }
            //End CVT#90912

            foreach (Control control in this.Controls)
            {
                control.GotFocus += Control_GotFocus;
            }

            dfCashOut.GotFocus += Control_GotFocus;  //#89907

            dfCashOut.PhoenixUIValidateEvent += DfCashOut_PhoenixUIValidateEvent;

            cbDenominate.CheckedChanged += CbDenominate_CheckedChanged;

            dfDifference.TextChanged += DifferenceTextChanged;    //121713-#6
        }

        //Daniel
        //public void RefreshTotals()
        //{
        //   // dfTotalDebits.UnFormattedValue = _parentWindow.TransactionSet.TotalDebits;
        //    //dfTotalDebits.Text = _parentWindow.TransactionSet.TotalDebits.ToString();
        //    //dfTotalCR.UnFormattedValue = _parentWindow.TransactionSet.TotalCredits;
        //    //dfDifference.UnFormattedValue = _parentWindow.TransactionSet.TotalDifference;
        //}
        //Daniel

        //Begin 121713-#6
        private void DifferenceTextChanged(object sender, EventArgs e)
        {
            SetDifferenceForeColor();
        }
        //End 121713-#6

        private void DfCashOut_PhoenixUIValidateEvent(object sender, Windows.Forms.PCancelEventArgs e)
        {
            if (dfCashOut.UnFormattedValue == null)
                dfCashOut.UnFormattedValue = 0;

            if (Convert.ToDecimal(dfCashOut.UnFormattedValue) >= 1 && !string.IsNullOrEmpty(dfCashOut.Text) && dfCashOut.Text.Contains(".") && !Convert.ToString(dfCashOut.UnFormattedValue).Contains(".")) //114934
                dfCashOut.UnFormattedValue = Convert.ToString(string.Format("{0}.00", dfCashOut.UnFormattedValue.ToString()));

            if (dfCashOut.UnFormattedValue != null && Convert.ToDecimal(dfCashOut.UnFormattedValue) < 0)
            {
                PMessageBox.Show(360188, MessageType.Warning, MessageBoxButtons.OK, new string[] { "Cash Out cannot be less than zero." }); //Task #87737
                dfCashOut.Focus();
            }
            else if (dfCashOut.UnFormattedValue != null && !string.IsNullOrEmpty(dfCashOut.Text))
                _parentWindow.TransactionSet.CashOut = Convert.ToDecimal(dfCashOut.UnFormattedValue);

            SetCashOutForeColor(); //121713-#4
        }

        private void CbDenominate_CheckedChanged(object sender, EventArgs e)
        {
            _parentWindow.TransactionSet.Denominate = (cbDenominate.Checked ? "Y" : "N");   //#89998
        }


        private void Control_GotFocus(object sender, EventArgs e)
        {
            //_parentWindow.LastActiveControl = sender as Control; //87735

            _parentWindow.ChildControlFocused(sender); //#89907
        }

        private void _parentWindow_OnCustomerChanged()
        {
            // this.repItemAccounts.DataSource = _parentWindow.Accounts;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void ResetCashOutInfo()  //#87731
        {
            this.dfCashOut.Text = string.Empty;
            this.dfCashOut.UnFormattedValue = 0;
            this.dfTotalCR.Text = string.Empty;
            this.dfTotalCR.UnFormattedValue = 0;
            this.dfTotalDebits.Text = string.Empty;
            this.dfTotalDebits.UnFormattedValue = 0;
            this.dfDifference.Text = string.Empty;
            this.dfDifference.UnFormattedValue = 0;
            this.cbDenominate.Checked = false;  //#89998
            this.cbDenominate.Visible = false;
            SetCashOutForeColor();    //121713-#4
            SetDifferenceForeColor(); //121713-#6
        }

        public void EnableDisableControls()
        {
            cbDenominate.Visible = TellerVars.Instance.IsTCDEnabled && TellerVars.Instance.IsTCDConnected;  //#89998
        }

        //Begin 121713-#2
        public void FocusPanel()
        {
            dfCashOut.Focus();
        }
        //End 121713-#2

        //Begin 121713-#4
        public void SetCashOutForeColor()
        {
            if (dfCashOut.UnFormattedValue != null && Convert.ToDecimal(dfCashOut.UnFormattedValue) > 0)
            {
                dfCashOut.ForeColor = System.Drawing.Color.Red;
                dfCashOut.Font = new Font(dfCashOut.Font, FontStyle.Bold);
            }
            else
            {
                dfCashOut.ForeColor = System.Drawing.Color.Black;
                dfCashOut.Font = new Font(dfCashOut.Font, FontStyle.Regular);
            }
        }
        //End 121713-#4
        //Begin 121713-#6
        public void SetDifferenceForeColor()
        {
            if (dfDifference.UnFormattedValue != null)
            {
                dfDifference.Font = new Font(dfCashOut.Font, FontStyle.Bold);

                if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
                {
                    dfDifference.ForeColor = System.Drawing.Color.Green;
                }
                else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
                {
                    dfDifference.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    dfDifference.ForeColor = System.Drawing.Color.Black;
                    dfDifference.Font = new Font(dfCashOut.Font, FontStyle.Regular);
                }
            }
        }
        //End 121713-#6
    }
}
