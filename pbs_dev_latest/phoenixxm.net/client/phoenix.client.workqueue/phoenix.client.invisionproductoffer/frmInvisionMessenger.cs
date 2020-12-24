#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

//-------------------------------------------------------------------------------
// File Name: frmInvisionMessenger.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	    Change              
//-------------------------------------------------------------------------------
//4/3/2017      1       GlaxeenaJ       Created
//4/3/2017      2       GlaxeenaJ       Window for Product Offer PopUp when searching a customer based on the ad_gb_rsm sales_popup value
//4/11/2017     3       GlaxeenaJ       Enh #209693, US #60618, Task #62108  Added Ident Button to identify the customer so that I can check his/her identity proof as per the system records
//4/21/2017     4       GlaxeenaJ       Design changed as per Rahul's recommendation.
//5/23/2017     5       GlaxeenaJ       considered 11206 – frmRmSignatureEdit- window to identify the customers.
//1/6/2017      6       GlaxeenaJ       Enh #209693, US #65380, Task #65382  Product Offer Window for teller and Cust Mgnt as an Alert, then to dialogue box
//09/11/2017    7       NKasim          Coverity Issue -#66944 - CID 115810 Resource Leak
//02/15/2018    8       Akhil V         #84061-Removed using statement added to resolve Coverity Issue -#66944
//02/19/2018    9       Minu            Task #84178-Addressing code review comments for Invision
//02/23/2018    10      Akhil V         Bug #84060-HotKey issue resolved
//5/20/2020     11      RDeepthi        Bug #126310. Doclose work already invoked before calling referral window. so on Close no need to call DoCloseWork() again.
//-------------------------------------------------------------------------------
#endregion


using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Client;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Phoenix.Client.WorkQueue
{
    public class frmInvisionMessenger : Phoenix.Windows.Forms.PfwStandard
    {


        #region Private var

        private Phoenix.BusObj.Misc.BiInvision _biInvision = new Phoenix.BusObj.Misc.BiInvision();
        private Phoenix.BusObj.Control.PcCustomOptions _pcCustOptions = new Phoenix.BusObj.Control.PcCustomOptions();
        private Phoenix.BusObj.RIM.RmAcct _rmAcct = new BusObj.RIM.RmAcct(); //Task #65378

        /// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.IContainer components = null;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbCustomerResponse;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbDeferUntilLater;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbAccepted;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbDeclined;
        private Phoenix.Windows.Forms.PLabelStandard lblReason;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbQuickNote;
        private Phoenix.Windows.Forms.PLabelStandard lblComment;
        private Phoenix.Windows.Forms.PdfStandard mlComment;
        private Phoenix.Windows.Forms.PAction pbReferral;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbProductOfferDetails;
        private Phoenix.Windows.Forms.PdfStandard mlOfferDetails;
        private Phoenix.Windows.Forms.PAction pbAddID; //#62108


        private int pnRimNo;
        private string sInvisionMsg = string.Empty;
        private string sradionBtnClickedValue = null;
        private string sSalesAcceptMethod = string.Empty;
        private bool _bInvokeReferralWindow = false;
        private string sButtonClicked = string.Empty;
        private string sCampaignOfferLogKey = string.Empty;

        private PwksWindow _parentForm;
        private Form _parentForTeller;

        /*Begin - #65378 */
        private PDecimal pnImageID = new PDecimal("pnImageID");
        /*End - #65378 */
        #endregion

        #region Init
        public override void InitParameters(params object[] paramList)// Task #84178- InitParameters is used instead of OnCreateParameters since there are 2 parameters of type Form and PwksWindow, which is not supported in OnCreateParameters.
        {
            //// Must call the base to store the parameters.
            pnRimNo = Convert.ToInt32(paramList[0]);
            /*Begin Task #65382*/
            sInvisionMsg = Convert.ToString(paramList[1]);
            sradionBtnClickedValue = Convert.ToString(paramList[2]);
            sCampaignOfferLogKey = Convert.ToString(paramList[3]);
            _parentForm = paramList[4] as PwksWindow;
            _parentForTeller = paramList[4] as Form;

            /*End Task #65382*/
            base.InitParameters(paramList);
            // Must call the base to store the parameters.
            //pnRimNo = Convert.ToInt32(paramList[0]);
            //base.InitParameters(paramList);
        }
        private void InitXmlTags()
        {
            this.cmbQuickNote.PhoenixUIControl.XmlTag = "QuickNote";
        }
        #endregion



        public frmInvisionMessenger()
        {
            InitializeComponent();
        }


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbCustomerResponse = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.mlComment = new Phoenix.Windows.Forms.PdfStandard();
            this.lblComment = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbQuickNote = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblReason = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbDeclined = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbAccepted = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbDeferUntilLater = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.pbReferral = new Phoenix.Windows.Forms.PAction();
            this.gbProductOfferDetails = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.mlOfferDetails = new Phoenix.Windows.Forms.PdfStandard();
            this.pbAddID = new Phoenix.Windows.Forms.PAction();
            this.gbCustomerResponse.SuspendLayout();
            this.gbProductOfferDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbReferral,
            this.pbAddID});
            // 
            // gbCustomerResponse
            // 
            this.gbCustomerResponse.Controls.Add(this.mlComment);
            this.gbCustomerResponse.Controls.Add(this.lblComment);
            this.gbCustomerResponse.Controls.Add(this.cmbQuickNote);
            this.gbCustomerResponse.Controls.Add(this.lblReason);
            this.gbCustomerResponse.Controls.Add(this.rbDeclined);
            this.gbCustomerResponse.Controls.Add(this.rbAccepted);
            this.gbCustomerResponse.Controls.Add(this.rbDeferUntilLater);
            this.gbCustomerResponse.Location = new System.Drawing.Point(4, 116);
            this.gbCustomerResponse.Name = "gbCustomerResponse";
            this.gbCustomerResponse.Size = new System.Drawing.Size(536, 176);
            this.gbCustomerResponse.TabIndex = 1;
            this.gbCustomerResponse.TabStop = false;
            this.gbCustomerResponse.Text = "Customer Response";
            // 
            // mlComment
            // 
            this.mlComment.AcceptsReturn = true;
            this.mlComment.Location = new System.Drawing.Point(76, 68);
            this.mlComment.MaxLength = 254;
            this.mlComment.Multiline = true;
            this.mlComment.Name = "mlComment";
            this.mlComment.PhoenixUIControl.ObjectId = 8;
            this.mlComment.PreviousValue = null;
            this.mlComment.Size = new System.Drawing.Size(452, 99);
            this.mlComment.TabIndex = 1;
            // 
            // lblComment
            // 
            this.lblComment.AutoEllipsis = true;
            this.lblComment.Location = new System.Drawing.Point(4, 64);
            this.lblComment.Name = "lblComment";
            this.lblComment.PhoenixUIControl.ObjectId = 8;
            this.lblComment.Size = new System.Drawing.Size(65, 16);
            this.lblComment.TabIndex = 5;
            this.lblComment.Text = "Comment:";
            // 
            // cmbQuickNote
            // 
            this.cmbQuickNote.Location = new System.Drawing.Point(76, 44);
            this.cmbQuickNote.Name = "cmbQuickNote";
            this.cmbQuickNote.PhoenixUIControl.ObjectId = 7;
            this.cmbQuickNote.Size = new System.Drawing.Size(452, 21);
            this.cmbQuickNote.TabIndex = 0;
            this.cmbQuickNote.Value = null;
            // 
            // lblReason
            // 
            this.lblReason.AutoEllipsis = true;
            this.lblReason.Location = new System.Drawing.Point(4, 40);
            this.lblReason.Name = "lblReason";
            this.lblReason.PhoenixUIControl.ObjectId = 7;
            this.lblReason.Size = new System.Drawing.Size(65, 16);
            this.lblReason.TabIndex = 3;
            this.lblReason.Text = "Reason:";
            // 
            // rbDeclined
            // 
            this.rbDeclined.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbDeclined.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeclined.Description = null;
            this.rbDeclined.Location = new System.Drawing.Point(440, 16);
            this.rbDeclined.Name = "rbDeclined";
            this.rbDeclined.PhoenixUIControl.ObjectId = 6;
            this.rbDeclined.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.rbDeclined.Size = new System.Drawing.Size(88, 16);
            this.rbDeclined.TabIndex = 4;
            this.rbDeclined.Text = "Declined Offer";
            this.rbDeclined.UseVisualStyleBackColor = false;
            this.rbDeclined.CheckedChanged += new System.EventHandler(this.rbDeclined_CheckedChanged);
            // 
            // rbAccepted
            // 
            this.rbAccepted.BackColor = System.Drawing.SystemColors.Control;
            this.rbAccepted.Description = null;
            this.rbAccepted.Location = new System.Drawing.Point(256, 16);
            this.rbAccepted.Name = "rbAccepted";
            this.rbAccepted.PhoenixUIControl.ObjectId = 5;
            this.rbAccepted.Size = new System.Drawing.Size(119, 16);
            this.rbAccepted.TabIndex = 3;
            this.rbAccepted.Text = "Accepted Offer";
            this.rbAccepted.UseVisualStyleBackColor = false;
            this.rbAccepted.CheckedChanged += new System.EventHandler(this.rbAccepted_CheckedChanged);
            // 
            // rbDeferUntilLater
            // 
            this.rbDeferUntilLater.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeferUntilLater.Description = null;
            this.rbDeferUntilLater.Location = new System.Drawing.Point(84, 16);
            this.rbDeferUntilLater.Name = "rbDeferUntilLater";
            this.rbDeferUntilLater.PhoenixUIControl.ObjectId = 4;
            this.rbDeferUntilLater.Size = new System.Drawing.Size(119, 16);
            this.rbDeferUntilLater.TabIndex = 2;
            this.rbDeferUntilLater.Text = "Defer Until Later";
            this.rbDeferUntilLater.UseVisualStyleBackColor = false;
            this.rbDeferUntilLater.CheckedChanged += new System.EventHandler(this.rbDeferUntilLater_CheckedChanged);
            // 
            // pbReferral
            // 
            this.pbReferral.LongText = "pbReferral";
            this.pbReferral.Name = "pbReferral";
            this.pbReferral.NextScreenId = 12528;
            this.pbReferral.ObjectId = 11;
            this.pbReferral.ShortText = "pbReferral";
            this.pbReferral.Tag = null;
            this.pbReferral.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReferral_Click);
            // 
            // gbProductOfferDetails
            // 
            this.gbProductOfferDetails.Controls.Add(this.mlOfferDetails);
            this.gbProductOfferDetails.Location = new System.Drawing.Point(4, 4);
            this.gbProductOfferDetails.Name = "gbProductOfferDetails";
            this.gbProductOfferDetails.PhoenixUIControl.ObjectId = 1;
            this.gbProductOfferDetails.Size = new System.Drawing.Size(536, 112);
            this.gbProductOfferDetails.TabIndex = 0;
            this.gbProductOfferDetails.TabStop = false;
            this.gbProductOfferDetails.Text = "Product Offer Details";
            // 
            // mlOfferDetails
            // 
            this.mlOfferDetails.Location = new System.Drawing.Point(4, 16);
            this.mlOfferDetails.Multiline = true;
            this.mlOfferDetails.Name = "mlOfferDetails";
            this.mlOfferDetails.PhoenixUIControl.ObjectId = 2;
            this.mlOfferDetails.PreviousValue = null;
            this.mlOfferDetails.ReadOnly = true;
            this.mlOfferDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mlOfferDetails.Size = new System.Drawing.Size(524, 89);
            this.mlOfferDetails.TabIndex = 0;
            this.mlOfferDetails.TabStop = false;
            // 
            // pbAddID
            // 
            this.pbAddID.LongText = "pbAddID";
            this.pbAddID.Name = "pbAddID";
            this.pbAddID.ObjectId = 12;
            this.pbAddID.ShortText = "pbAddID";
            this.pbAddID.Tag = null;
            this.pbAddID.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbAddID_Click);
            // 
            // frmInvisionMessenger
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(544, 294);
            this.Controls.Add(this.gbProductOfferDetails);
            this.Controls.Add(this.gbCustomerResponse);
            this.Name = "frmInvisionMessenger";
            this.ScreenId = 3563;
            this.ShowNotes = false;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmInvisionMessenger_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmInvisionMessenger_PInitCompleteEvent);
            this.gbCustomerResponse.ResumeLayout(false);
            this.gbCustomerResponse.PerformLayout();
            this.gbProductOfferDetails.ResumeLayout(false);
            this.gbProductOfferDetails.PerformLayout();
            this.ResumeLayout(false);

        }
        //Begin #62108
        public void pbAddID_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.AddIdClick);
        }
        //End #62108

        private void CallOtherForms(CallOtherForm caseName)
        {
            this.Close();
            PfwStandard tempWin = null;
            switch (caseName)

            {
                case CallOtherForm.AddIdClick: //#62108
                    /*Begin - Task #65378*/
                    string sImageId = "";
                    string Id = "-1";
                    pnImageID.Value = 0;
                    _rmAcct.RimNo.Value = pnRimNo;
                    _rmAcct.GetImageId(ref sImageId);
                    pnImageID.Value = (sImageId == "" ? 0 : Convert.ToInt32(sImageId));
                    if (pnImageID.Value != 0)
                    {
                        Id = Convert.ToString(pnImageID.Value);
                    }
                    tempWin = CreateWindow("phoenix.client.DpLnSigners", "Phoenix.Windows.Client", "frmRmSignatureEdit");
                    tempWin.InitParameters(
                    Convert.ToString(Convert.ToInt32(pnRimNo == 0 ? 0 : pnRimNo)),
                    null,
                    null,
                    null,
                    -1,
                    -1,
                    Id,
                    //"10349"
                    pbAddID.NextScreenId
                    );
                    /*End - Task #65378*/
                    break;
            }
            if (tempWin != null)
            {
                tempWin.Workspace = _parentForm.Workspace;
                // TODO: if you want a grid to refresh on this form when this window is closed,
                // set the following property: tempWin.ParentGrid = this.grid;	
                //tempWin.Closed += new EventHandler(tempWin_Closed); //#26417
                tempWin.Show();
            }
        }

        #endregion

        #region Window Events

        #region Window Begin/Complete Events
        private Phoenix.Windows.Forms.ReturnType frmInvisionMessenger_PInitBeginEvent()
        {         
            this.MainBusinesObject = _biInvision;
            this.IsNew = true;
            this.AutoFetch = false;
            this.rbDeferUntilLater.Focus();
            this.ActionClose.ObjectId = 10;     //"Save & Close"

            this.mlOfferDetails.ForeColor = Color.DarkBlue;
            this.mlOfferDetails.Font = new Font("Arial", 10);
            // Begin #62108
            //CTD:Set bValidateForm = TRUE
            pbAddID.ValidateForm = true;
            //End #62108

            /*Begin Task #65382*/
            //GetMessengerData(pnRimNo);
            /*End Task #65382*/

            //if (sInvisionMsg == null || sInvisionMsg.Trim() == "")
            //{
            //    AvoidSave = true;
            //    base.OnActionClose();
            //    return ReturnType.None;
            //}

            if (sradionBtnClickedValue == "radioDeferClicked")
            {
                rbDeferUntilLater.Checked = true;
                RefreshQuickNote();
            }
            else if (sradionBtnClickedValue == "radioAcceptClicked")
            {
                rbAccepted.Checked = true;
                RefreshQuickNote();
            }
            else if (sradionBtnClickedValue == "radioDeclineClicked")
            {
                rbDeclined.Checked = true;
                RefreshQuickNote();
            }

            //Uncommented for working as such in ID for edit customer window, which needs rim number only
            /*Begin - task #65378*/
            if (Phoenix.Shared.Variables.GlobalVars.Module == "Teller")
            {
                this.pbAddID.NextScreenId = Phoenix.Shared.Constants.ScreenId.RmSignatureCardTeller;
            }
            else
            {
                this.pbAddID.NextScreenId = Phoenix.Shared.Constants.ScreenId.RmSignatureCardEditView;
            }
            pbAddID.Enabled = false;
            ///*End - task #65378*/
            return ReturnType.Success;
        }


        private void frmInvisionMessenger_PInitCompleteEvent()
        {
            this._biInvision.SalesAcceptMethod.Value = sSalesAcceptMethod;
            this.mlOfferDetails.UnFormattedValue = this.sInvisionMsg;
            // this.rbDeferUntilLater.Checked = true;
            this.cmbQuickNote.Focus();
        }

        #endregion

        #region Radio Button Events
        private void rbDeferUntilLater_CheckedChanged(object sender, System.EventArgs e)
        {
            //repop the quick note combo
            this._biInvision.QuickNoteType.Value = "Deferred";
            RefreshQuickNote();
        }

        private void rbAccepted_CheckedChanged(object sender, System.EventArgs e)
        {
            //repop the quick note combo
            this._biInvision.QuickNoteType.Value = "Accepted";
            RefreshQuickNote();
        }

        private void rbDeclined_CheckedChanged(object sender, System.EventArgs e)
        {
            //repop the quick note combo
            this._biInvision.QuickNoteType.Value = "Declined";
            RefreshQuickNote();
        }
        #endregion

        #region Push Button Events
        public override bool OnActionClose()
        {
            if (!this._bInvokeReferralWindow && !DoCloseWork()) //126310. when _bInvokeReferralWindow is true then DoCloseWork() is already been done and no need to do it again on Close
                return false;
            return base.OnActionClose();
        }



        private void pbReferral_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            this.sButtonClicked = "pbReferral";
            if (!DoCloseWork())
                return;
            if (this._bInvokeReferralWindow)
            {
                LaunchReferralWindow();
                this.pbReferral.Enabled = false;
                this.ActionClose.Enabled = false;
            }
        }


        #endregion

        #endregion

        #region Private Methods

        private enum CallOtherForm
        {
            AddIdClick
        }
        #endregion

        #region Private Functions/Procedures
        private void EnableDisableVisibleLogic(string caseType)
        {
            switch (caseType)
            {
                case "RadioClick":
                    #region Radio click Logic
                    if (this.rbAccepted.Checked)
                    {
                        //TODO: check for custom_options
                        this.pbReferral.Enabled = this._pcCustOptions.CheckCustomOptions(Phoenix.BusObj.Control.PcCustomOptions.CO_Constants.CO_SalesAndService);

                    }
                    else
                    {
                        this.pbReferral.Enabled = false;
                    }
                    #endregion
                    break;
            }

            return;
        }
        private void RefreshQuickNote()
        {
            EnableDisableVisibleLogic("RadioClick");

            Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_biInvision, _biInvision.QuickNote);
            cmbQuickNote.Populate(_biInvision.QuickNote.Constraint.EnumValues);
        }
        private bool ValidateFields()
        {
            if (!rbDeferUntilLater.Checked && cmbQuickNote.Text.Trim() == "")
            {
                PMessageBox.Show(this, 360648, MessageType.Warning, MessageBoxButtons.OK);
                //"Reason" field cannot be blank.
                cmbQuickNote.Focus();
                return false;
            }

            return true;
        }
        private bool DoCloseWork()
        {
            /*we do this check cos sometime we nay need to close this window in the begin event and at that
            //time we do not want to save any stuff*/
            if (this.mlOfferDetails.Text.Trim() != "")
            {
                if (!ValidateFields())
                    return false;

                //Calls method defined in BO for saving contact history to RM_CONTACT_HIST and posting Respose from user to Invision Server
                this._biInvision.SaveMsgrData(this._biInvision.QuickNoteType.Value, this.cmbQuickNote.Text.Trim(), this.mlComment.Text.Trim(), "SaveAndClose", sCampaignOfferLogKey, pnRimNo);

                #region Check if we need to bring up the Referral window
                bool bInvokeReferralWindow = false;
                if (this.sButtonClicked != "pbReferral")
                {
                    if (rbAccepted.Checked && !this._biInvision.SalesAcceptMethod.IsNull)
                    {
                        if (this._biInvision.SalesAcceptMethod.Value.Trim() != "Manual")
                        {
                            if (this._biInvision.SalesAcceptMethod.Value.Trim() == "Ask Question")
                            {
                                //This customer has accepted the product offer. Would you like to create a referral at this time?
                                if (PMessageBox.Show(this, 360649, MessageType.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    bInvokeReferralWindow = true;
                                else
                                    bInvokeReferralWindow = false;
                            }
                            else //Pop-Up
                            {
                                bInvokeReferralWindow = true;
                            }
                        }
                    }
                }
                else
                {
                    bInvokeReferralWindow = true;
                }

                _bInvokeReferralWindow = bInvokeReferralWindow;
                #endregion
            }

            return true;
        }

        private void LaunchReferralWindow()
        {
            this.Close();
            /* #66944 - Added using stateemnt to make sure object is disposed */
            /*#84061-using statement is removed due to scope issue for object temp */
            frmReferralEdit temp = new frmReferralEdit();                
            if (_parentForm != null)
                {
                      temp.InitParameters(this, "Referral",
                        null,
                        (this._biInvision.RimNo.IsNull ? 0 : this._biInvision.RimNo.Value),
                        null,
                        null,
                        null,
                        null);
                    temp.Workspace = _parentForm.Workspace;
                    temp.Show();
                }
                else if (_parentForTeller != null)
                {
                    IPhoenixWorkspace curWkspace = (_parentForTeller as PfwStandard).Workspace;
                    temp.InitParameters(this, "Referral",
                        null,
                        (this._biInvision.RimNo.IsNull ? 0 : this._biInvision.RimNo.Value),
                        null,
                        null,
                        null,
                        null);
                    temp.Workspace = curWkspace;
                    temp.Show();
                /*Begin #84060-to get focus on the window*/
                SendKeys.Send("{TAB}");
                SendKeys.Send("{TAB}");
                /*End #84060*/
            }
        }      
        #endregion
    }
}
