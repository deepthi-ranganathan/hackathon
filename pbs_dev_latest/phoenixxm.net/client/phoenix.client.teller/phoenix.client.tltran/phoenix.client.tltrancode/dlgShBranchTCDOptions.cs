using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;

namespace Phoenix.Client.Teller
{
    public partial class dlgShBranchTCDOptions : PfwStandard
    {
        public dlgShBranchTCDOptions()
        {
            InitializeComponent();
        }

        PBoolean _enableTCR = new PBoolean("EnableTCR");
        PBoolean _enableTCD = new PBoolean("EnableTCD");

        public override void OnCreateParameters()
        {
            Parameters.Add(_enableTCR);
            Parameters.Add(_enableTCD);
            base.OnCreateParameters();
        }

        private ReturnType dlgShBranchTCDOptions_PInitBeginEvent()
        {
            this.ScreenId = 3017;
            ActionClose.Image = Images.Ok;
            ActionClose.ObjectId = 5;
            return default(ReturnType);
        }

        private void dlgShBranchTCDOptions_PInitCompleteEvent()
        {
            
            cbTCD.Enabled = _enableTCD.Value;
            cbDenominate.Enabled = _enableTCD.Value;
            cbTCR.Enabled = _enableTCR.Value;

        }

        public override bool OnActionClose()
        {
            Workspace.Variables["IsAcquirerTranTCRCashIn"] = cbTCR.Checked;
            Workspace.Variables["IsAcquirerTranTCDCashOut"] = cbTCD.Checked;
            Workspace.Variables["IsAcquirerTranTCDDenomCashOut"] = cbDenominate.Checked;
            return base.OnActionClose();
        }

        private void cbTCD_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            if (!cbTCD.Checked)
                cbDenominate.Checked = false;
        }

        private void cbDenominate_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            if (cbDenominate.Checked)
                cbTCD.Checked = true;
        }
    }
}