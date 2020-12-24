using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;

namespace Phoenix.Client.Teller
{
    public partial class dlgPEDSignatureCapture : PfwStandard
    {
        public dlgPEDSignatureCapture()
        {
            InitializeComponent();
        }

        string _customerInfo;
        Image imgSignature = null;


        public override void InitParameters(params object[] paramList)
        {
            if (paramList.Length >= 2)
            {
                if (paramList[0] != null)
                    _customerInfo = Convert.ToString(paramList[0]);
                if (paramList[1] != null)
                    imgSignature = (Image)(paramList[1]);
            }
            base.InitParameters(paramList);
        }

        private ReturnType dlgPEDSignatureCapture_PInitBeginEvent()
        {
            this.ScreenId = 0;
            //ActionClose.Image = Images.Ok;
            //ActionClose.ObjectId = 5;
            this.pbOk.Image = Images.Ok;
            this.IsActionPaneNeeded = false;
            this.AvoidSave = true;
            return default(ReturnType);
        }

        private void dlgPEDSignatureCapture_PInitCompleteEvent()
        {
            dfCustomerInfo.Text = _customerInfo;
            if (imgSignature != null)
                this.picSignature.Image = (Image)imgSignature;
        }

        void pbRetrySignature_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
            this.Close();            
        }

        void pbOk_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


    }
}
