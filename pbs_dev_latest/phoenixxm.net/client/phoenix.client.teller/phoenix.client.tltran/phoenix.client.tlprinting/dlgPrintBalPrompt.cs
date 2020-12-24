#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: dlgPrintBalPrompt.cs
// NameSpace: phoenix.client.tlprinting
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//1/20/2011 	1		LSimpson    Created  
//06/23/2011    2       LSimpson    #14578 - Handle default button to use on Key Enter when tabbing.
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;



namespace phoenix.client.tlprinting
{
    public partial class dlgPrintBalPrompt : PfwStandard
    {
        #region Private Variables
        private DialogResult dialogResult = DialogResult.None;
        private DialogResult dialogDefault = DialogResult.None;
        private Phoenix.BusObj.Teller.TlTransactionSet _tlTranSet = new TlTransactionSet();
        private string _printBalOpt = null;
        private bool _isMt = false;
        private bool _isOffline = false;
        #endregion
        #region Constructors
        public dlgPrintBalPrompt()
        {
            InitializeComponent();
        }




        #endregion

        #region Public Properties
        public string PrintBalOpt
        {
            get
            {
                return _printBalOpt;
            }
            set
            {
                _printBalOpt = value;
            }
        }
        #endregion

        #region Public Methods
        public override void InitParameters(params object[] paramList)
        {
            _tlTranSet = paramList[0] as TlTransactionSet;
            _isMt = (bool)paramList[1];
            _isOffline = (bool)paramList[2];

            // Must call the base to store the parameters.
            base.InitParameters(paramList);
        }


        #endregion



        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            this.IsActionPaneNeeded = false;
            SetDefaultButton();

            return ReturnType.Success;
        }

        private void fwStandard_PInitCompleteEvent()
        {
            picQuestion.Image = System.Drawing.SystemIcons.Question.ToBitmap();
        }

        private void SetDefaultButton()
        {
            if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "A" && !_isOffline)
            {
                pbAll.Select();
                pbAll.Focus();
                this.AcceptButton = pbAll;
                this.DialogResult = DialogResult.OK;
                this.dialogDefault = DialogResult.OK;
            }
            else if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "N")
            {
                pbNone.Select();
                pbNone.Focus();
                this.AcceptButton = pbNone;
                this.DialogResult = DialogResult.None;
                this.dialogDefault = DialogResult.None;
            }
            else if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "S" && !_isOffline)
            {
                pbSelect.Select();
                pbSelect.Focus();
                this.AcceptButton = pbSelect;
                this.DialogResult = DialogResult.Yes;
                this.dialogDefault = DialogResult.Yes;
            }
            else
            {
                pbNone.Select();
                pbNone.Focus();
                this.AcceptButton = pbNone;
                this.DialogResult = DialogResult.None;
                this.dialogDefault = DialogResult.None;
            }
            if (_isOffline)
            {
                pbAll.Enabled = false;
                pbSelect.Enabled = false;
                pbNone.Select();
                pbNone.Focus();
                this.AcceptButton = pbNone;
            }
        }

        #region #14578
        void pbNone_GotFocus(object sender, System.EventArgs e)
        {
            if (this.AcceptButton != pbNone)
            {
                this.AcceptButton = pbNone;
                this.dialogDefault = DialogResult.None;
            }
        }

        void pbAll_GotFocus(object sender, System.EventArgs e)
        {
            if (this.AcceptButton != pbAll)
            {
                this.AcceptButton = pbAll;
                this.dialogDefault = DialogResult.OK;
            }
        }

        void pbSelect_GotFocus(object sender, System.EventArgs e)
        {
            if (this.AcceptButton != pbSelect)
            {
                this.AcceptButton = pbSelect;
                this.dialogDefault = DialogResult.Yes;
            }
        }
        #endregion

        private void CallOtherForms(string origin)
        {
            PfwStandard tempWin = null;
            PfwStandard tempDlg = null;

            try
            {
                if (origin == "Select")
                {
                    #region Select
                    tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "phoenix.client.tlprinting", "frmTlBalPrint");
                    tempDlg.InitParameters(_tlTranSet, _isMt);
                    #endregion
                }

                if (tempWin != null)
                {
                    //formLostFocus = true;
                    tempWin.Closed += new EventHandler(tempWin_Closed);
                    //tempWin.Workspace = this.Workspace;
                    tempWin.Show();
                    //do not remove or comment out the following this fixes the focus problem for cash count window
                    if (origin == "CashInDenom" || origin == "CashOutDenom" || origin == "PostCashOutDenom" ||
                        origin == "LoanPmtRecalc" || origin == "Select")  //#76425
                        tempWin.Focus();
                }
                // #8910 - checkDetailsCaptured is not needed!   else if (tempDlg != null && !checkDetailsCaptured)	//#79572
                else if (tempDlg != null)
                {
                    //tempDlg.Workspace = this.Workspace;
                    tempDlg.Closed += new EventHandler(tempDlg_Closed); //Selva-ChkPrintFix
                    //Begin #2327 - Do not comment or modify the dialogResult assignment PrintForm requires it
                    dialogResult = tempDlg.ShowDialog(); //#2327 - original code uncommeted

                    //AfterDialogClosed(origin);  // #79510
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
                return;
            }
        }

        private void tempDlg_Closed(object sender, EventArgs e)    //Selva-ChkPrintFix
        {
            Form form = sender as Form;
        }

        private void tempWin_Closed(object sender, EventArgs e)
        {
            Form form = sender as Form;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;
            const int WM_SYSKEYDOWN = 0x104;
            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Enter:
                        this.DialogResult = this.dialogDefault;
                        break;
                }
            }
            if (keyData == Keys.Enter && this.DialogResult == DialogResult.Yes)
                CallOtherForms("Select");

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region events

        void pbSelect_Click(object sender, System.EventArgs e)
        {
            PrintBalOpt = "S";
            this.DialogResult = DialogResult.Yes;
            CallOtherForms("Select");
        }

        void pbNone_Click(object sender, System.EventArgs e)
        {
            PrintBalOpt = "N";
            this.DialogResult = DialogResult.None;
            this.Close();
        }

        void pbAll_Click(object sender, System.EventArgs e)
        {
            PrintBalOpt = "A";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion
        #endregion


    }
}