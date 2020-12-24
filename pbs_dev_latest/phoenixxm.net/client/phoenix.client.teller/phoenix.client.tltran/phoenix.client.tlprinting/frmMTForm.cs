#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmMTForm.cs
// NameSpace: phoenix.client.tlprinting
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//11/08/2010    1	    LSimpson    #80615 - Created.  
//02/04/2011    2       LSimpson    #12705 - Changes to dfNoCopies allowed range.
//02/15/2011    3		LSimpson	#12903 - Added update for no_copies.
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.BusObj.Admin.Teller;

using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;

namespace phoenix.client.tlprinting
{
    public partial class frmMTForm : PfwStandard
    {
        #region Private Variables
        private AdTlFormMtTran _adTlFormMTTran = new AdTlFormMtTran();
        private AdTlForm _adTlForm = new AdTlForm();

        #endregion
        #region Constructors
        public frmMTForm()
        {
            InitializeComponent();
        }




        #endregion

        #region Public Properties

        #endregion

        #region Public Methods

        #region InitParameters
        public override void InitParameters(params object[] paramList)
        {            
            this.ScreenId = Phoenix.Shared.Constants.ScreenId.AdMtForm;
            base.InitParameters(paramList);
        }
        #endregion InitParameters

        #endregion

        #region #12903 Save and Update Copies
        public override bool OnActionSave(bool isAddNext)
        {
            if (dfNoCopies.IsDirty)
            {
                _adTlFormMTTran.ActionType = XmActionType.Custom;
                _adTlFormMTTran.CustomActionName = "UpdateCopies";
                Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(_adTlFormMTTran, "UpdateCopies");
            }

            bool isSaved = base.OnActionSave(isAddNext);

            return isSaved;
        }
        #endregion

        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            _adTlFormMTTran = new AdTlFormMtTran();
            _adTlFormMTTran.Ptid.Value = 1;
            this.MainBusinesObject = _adTlFormMTTran;


            return ReturnType.Success;
        }

        private void fwStandard_PInitCompleteEvent()
        {
            if (this._adTlFormMTTran.PrintOrder.Value == null)
                this.IsNew = true;
            else
                this.IsNew = false;

            if (this._adTlFormMTTran.PrintOrder.Value == "F")
                this.rbFirst.Checked = true;
            else if (this._adTlFormMTTran.PrintOrder.Value == "L")
                this.rbLast.Checked = true;
            else
                this.rbFirst.Checked = true;
        }

        void cmbFooter_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        void cmbHeader_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        void dfNoCopies_Leave(object sender, System.EventArgs e)
        {
            if (dfNoCopies.Text.Trim() == "" || dfNoCopies.Text.Trim() == "-" || dfNoCopies.Text.Trim() == "--" || dfNoCopies.Text.Trim() == "+" || dfNoCopies.Text.Trim() == "++" || dfNoCopies.Text.IndexOf(',') >= 0 || dfNoCopies.Text.IndexOf('.') >= 0)
            {
                dfNoCopies.Text = "1"; // #12705
                dfNoCopies.UnFormattedValue = 1;// #12705
            }
            if (Convert.ToInt16(dfNoCopies.Text) <= 0)
            {
                if (Convert.ToInt16(dfNoCopies.Text) < 0)
                {
                    dfNoCopies.Text = "1"; // #12705
                    dfNoCopies.UnFormattedValue = 1; // #12705
                    throw CoreService.ExceptionMgr.NewException(ErrorCodes.NOT_POSITIVENUMBER, 300091);
                }
                dfNoCopies.Text = "1"; // #12705
                dfNoCopies.UnFormattedValue = 1; // #12705
            }
        }

        void rbLast_PhoenixUICheckedChangedEvent(object sender, Phoenix.Windows.Forms.PEventArgs e)
        {
            if (rbLast.Checked)
                this._adTlFormMTTran.PrintOrder.Value = "L";
            else
                this._adTlFormMTTran.PrintOrder.Value = "F";
        }

        void rbFirst_PhoenixUICheckedChangedEvent(object sender, Phoenix.Windows.Forms.PEventArgs e)
        {
            if (rbFirst.Checked)
                this._adTlFormMTTran.PrintOrder.Value = "F";
            else
                this._adTlFormMTTran.PrintOrder.Value = "L";
        }

        #endregion

    }
}