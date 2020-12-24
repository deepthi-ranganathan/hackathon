#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - frmRmContact - Search window for records in RM_CONTACT_HIST table.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: frmWorkRefReq.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/27/06		1		VDevadoss	#69248 - Created
//03/06/11		2		vsharma	    #13321 - Addressed the workspace issue, causing the purple screen
//12/27/2011    3       LSimpson    #15828 - Modifications to make window get message data.
//01/25/2012    4       LSimpson    #16630 - Modifications for closing window if no Messenger data.
//08/06/2012    5       Mkrishna    #19058 - Adding call to base on initParameters.
//4/19/2017     6       GlaxeenaJ   Enh #209693,Task #63042  Added Ident Button to identify the customer so that I can check his/her identity proof as per the system records
//4/21/2017     7       GlaxeenaJ   Changed window resolution and design as per Rahul's suggestion.
//5/24/2017     8       GlaxeenaJ   US #65377, Task #65378 - considered 11206 – frmRmSignatureEdit- window to identify the customers.
//2/21/2018     9       Akhil V     Bug #84499 -To close the product offer window in touche once trying to save and refer the accepted offer. 
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;

using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.BusObj.Misc;
using Phoenix.BusObj.Control;
using Phoenix.Windows.Client;

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for frmOnMessenger.
	/// </summary>
	public class frmOnMessenger : Phoenix.Windows.Forms.PfwStandard
	{
        #region Private Vars
        private Phoenix.BusObj.RIM.RmAcct _rmAcct = new BusObj.RIM.RmAcct(); //Task #65378
        private enum CallOtherForm
        {
            AddIdClick
        }

        private string sButtonClicked = string.Empty;
		private string sSalesAcceptMethod = string.Empty;
		private string sToucheMsg = string.Empty;
		private Phoenix.BusObj.Misc.Messenger _oToucheMsgr = new Phoenix.BusObj.Misc.Messenger();
		private Phoenix.BusObj.Control.PcCustomOptions _pcCustOptions = new Phoenix.BusObj.Control.PcCustomOptions();
        private Phoenix.BusObj.Admin.Global.AdGbRsm _adGbRsm = new Phoenix.BusObj.Admin.Global.AdGbRsm(); // #15828

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbCustomerResponse;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbDeferUntilLater;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbAccepted;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbDeclined;
		private Phoenix.Windows.Forms.PLabelStandard lblReason;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbQuickNote;
		private Phoenix.Windows.Forms.PLabelStandard lblComment;
		private Phoenix.Windows.Forms.PdfStandard mlComment;
		private Phoenix.Windows.Forms.PAction pbReferral;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbProductOfferDetails;
		private Phoenix.Windows.Forms.PdfStandard mlOfferDetails;
        private Phoenix.Windows.Forms.PAction pbAddID; //#63042
		//#13321
        private bool _bInvokeReferralWindow = false;
        #region #15828
        private int pnRimNo;
        private int pnAcctId;
        private string psAcctType;
        private string psAcctNo;
        #endregion

        /*Begin - #65378 */
        private PDecimal pnImageID = new PDecimal("pnImageID");
        /*End - #65378 */

        #endregion

        #region Constructor
        public frmOnMessenger()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion
		
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
            this.lblComment.Location = new System.Drawing.Point(4, 72);
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
            this.lblReason.Location = new System.Drawing.Point(4, 44);
            this.lblReason.Name = "lblReason";
            this.lblReason.PhoenixUIControl.ObjectId = 7;
            this.lblReason.Size = new System.Drawing.Size(69, 16);
            this.lblReason.TabIndex = 3;
            this.lblReason.Text = "Reason:";
            // 
            // rbDeclined
            // 
            this.rbDeclined.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeclined.Description = null;
            this.rbDeclined.Location = new System.Drawing.Point(440, 20);
            this.rbDeclined.Name = "rbDeclined";
            this.rbDeclined.PhoenixUIControl.ObjectId = 6;
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
            this.rbAccepted.Location = new System.Drawing.Point(232, 20);
            this.rbAccepted.Name = "rbAccepted";
            this.rbAccepted.PhoenixUIControl.ObjectId = 5;
            this.rbAccepted.Size = new System.Drawing.Size(100, 16);
            this.rbAccepted.TabIndex = 3;
            this.rbAccepted.Text = "Accepted Offer";
            this.rbAccepted.UseVisualStyleBackColor = false;
            this.rbAccepted.CheckedChanged += new System.EventHandler(this.rbAccepted_CheckedChanged);
            // 
            // rbDeferUntilLater
            // 
            this.rbDeferUntilLater.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeferUntilLater.Description = null;
            this.rbDeferUntilLater.Location = new System.Drawing.Point(4, 20);
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
            // frmOnMessenger
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(544, 294);
            this.Controls.Add(this.gbProductOfferDetails);
            this.Controls.Add(this.gbCustomerResponse);
            this.Name = "frmOnMessenger";
            this.ScreenId = 12485;
            this.ShowNotes = false;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmOnMessenger_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmOnMessenger_PInitCompleteEvent);
            this.SizeChanged += new System.EventHandler(this.FrmOnMessenger_SizeChanged);
            this.gbCustomerResponse.ResumeLayout(false);
            this.gbCustomerResponse.PerformLayout();
            this.gbProductOfferDetails.ResumeLayout(false);
            this.gbProductOfferDetails.PerformLayout();
            this.ResumeLayout(false);

		}

       
       

        private void CallOtherForms(CallOtherForm caseName)
        {
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
                tempWin.Workspace = this.Workspace;
                // TODO: if you want a grid to refresh on this form when this window is closed,
                // set the following property: tempWin.ParentGrid = this.grid;	
                //tempWin.Closed += new EventHandler(tempWin_Closed); //#26417
                tempWin.Show();
            }
        }

        //End #63042


        #endregion

        #region Init
        public override void InitParameters(params Object[] paramList)
		{
            #region #15828
            if (paramList.Length >= 4)
            {
                if (paramList[0] != null)
                {
                    this._oToucheMsgr.RimNo.Value = Convert.ToInt32(paramList[0]);
                    pnRimNo = Convert.ToInt32(paramList[0]);
                }

                if (paramList[1] != null)
                {
                    this._oToucheMsgr.AcctType.Value = paramList[1].ToString();
                    psAcctType = paramList[1].ToString();
                }

                if (paramList[2] != null)
                {
                    this._oToucheMsgr.AcctNo.Value = paramList[2].ToString();
                    psAcctNo = paramList[2].ToString();
                }

                if (paramList[3] != null)
                {
                    this._oToucheMsgr.AcctId.Value = Convert.ToInt32(paramList[3]);
                    pnAcctId = Convert.ToInt32(paramList[3]);
                }
            }
            #endregion

            InitXmlTags();

            base.InitParameters(paramList); //#19058
		}
		private void InitXmlTags()
		{ 
			this.cmbQuickNote.PhoenixUIControl.XmlTag = "QuickNote";
		}
		#endregion

		#region Window Events
		#region Window Begin/Complete Events
		private Phoenix.Windows.Forms.ReturnType frmOnMessenger_PInitBeginEvent()
		{
			this.MainBusinesObject = _oToucheMsgr;
			this.IsNew = true;
			this.AutoFetch = false;
			this.rbDeferUntilLater.Focus();
			this.ActionClose.ObjectId = 10;		//"Save & Close"
			
			this.mlOfferDetails.ForeColor = Color.DarkBlue;
			this.mlOfferDetails.Font = new Font("Arial", 10);
            // Begin #63042
            //CTD:Set bValidateForm = TRUE
            pbAddID.ValidateForm = true;
            //End #63042
            #region #15828
            GetMessengerData(pnRimNo, psAcctType, psAcctNo, pnAcctId);

            #endregion
            // #16630 - If no Messenger data, close window
            if (sToucheMsg == null || sToucheMsg.Trim() == "")
            {
                AvoidSave = true;
                base.OnActionClose();
                return ReturnType.None;
            }
            // #16630
            //Uncommented for working as such in ID for edit customer window, which needs rim number only
            /*Begin - task #65378*/
            //if (Phoenix.Shared.Variables.GlobalVars.Module == "Teller")
            //{
            //    this.pbAddID.NextScreenId = Phoenix.Shared.Constants.ScreenId.RmSignatureCardTeller;
            //}
            //else
            //{
                this.pbAddID.NextScreenId = Phoenix.Shared.Constants.ScreenId.RmSignatureCardEditView;
            //}
            ///*End - task #65378*/
            pbAddID.Enabled = false;
            return ReturnType.Success;			
		}

		private void frmOnMessenger_PInitCompleteEvent()
        {
            this._oToucheMsgr.SalesAcceptMethod.Value = sSalesAcceptMethod;
			this.mlOfferDetails.UnFormattedValue = this.sToucheMsg;
			this.rbDeferUntilLater.Checked = true;
			this.cmbQuickNote.Focus();          
        }

        #endregion

        #region Radio Button Events
        private void rbDeferUntilLater_CheckedChanged(object sender, System.EventArgs e)
		{
			//repop the quick note combo
			this._oToucheMsgr.QuickNoteType.Value = "Deferred";
			RefreshQuickNote();
		}

		private void rbAccepted_CheckedChanged(object sender, System.EventArgs e)
		{
			//repop the quick note combo
			this._oToucheMsgr.QuickNoteType.Value = "Accepted";
			RefreshQuickNote();		
		}

		private void rbDeclined_CheckedChanged(object sender, System.EventArgs e)
		{
			//repop the quick note combo
			this._oToucheMsgr.QuickNoteType.Value = "Declined";
			RefreshQuickNote();		
		}
		#endregion

		#region Push Button Events
		public override bool OnActionClose()
		{
			if (!DoCloseWork())
				return false;
			return base.OnActionClose ();
		}

		private void pbReferral_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			this.sButtonClicked = "pbReferral";
			//this.Close();
			//#13321
            if (!DoCloseWork())
                return;
            if (this._bInvokeReferralWindow)
            {
                LaunchReferralWindow();
                this.pbReferral.Enabled = false;
                this.ActionClose.Enabled = false;
                //this.pb
            }
		}

        private void pbAddID_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.AddIdClick);
        }

        
        #endregion
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

			Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_oToucheMsgr, _oToucheMsgr.QuickNote);
			cmbQuickNote.Populate(_oToucheMsgr.QuickNote.Constraint.EnumValues);
		}
		private bool ValidateFields()
		{
			if (! rbDeferUntilLater.Checked && cmbQuickNote.Text.Trim()== "")
			{
				PMessageBox.Show(this, 360648, MessageType.Warning, MessageBoxButtons.OK);
				//360648 - "Reason" field cannot be blank.
				cmbQuickNote.Focus();
				return false;
			}

			return true;
		}
	
		private bool DoCloseWork()
		{
			/*we do this check cos sometime we nay need to close this window in the begin event and at that
			time we do not want to save any stuff*/
			if (this.mlOfferDetails.Text.Trim() != "")
			{
				if (!ValidateFields())
					return false;

				this._oToucheMsgr.SaveMsgrData(this._oToucheMsgr.QuickNoteType.Value, this.cmbQuickNote.Text.Trim(), this.mlComment.Text.Trim(), "SaveAndClose");

				#region Check if we need to bring up the Referral window
				bool bInvokeReferralWindow = false;
				if (this.sButtonClicked != "pbReferral" )
				{
					if (rbAccepted.Checked && !this._oToucheMsgr.SalesAcceptMethod.IsNull)
					{
						if (this._oToucheMsgr.SalesAcceptMethod.Value.Trim() != "Manual")
						{
							if (this._oToucheMsgr.SalesAcceptMethod.Value.Trim() == "Ask Question")
							{
								//360649 - This customer has accepted the product offer. Would you like to create a referral at this time?
								if (PMessageBox.Show(this, 360649, MessageType.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
									bInvokeReferralWindow = true;
								else
									bInvokeReferralWindow = false;
							}
							else	//Pop-Up
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

				//#13321
                _bInvokeReferralWindow = bInvokeReferralWindow;
                //if (bInvokeReferralWindow)
                //    LaunchReferralWindow();
				#endregion
			}

			return true;
		}
		private void LaunchReferralWindow()
		{
            this.Close();//#84499 -To resolve the bug Touche_unable to close the product offer window in touche once trying to save  and refer the accepted offer
            frmReferralEdit temp = new frmReferralEdit();
			temp.InitParameters( this, "Referral", 
				null, 
				(this._oToucheMsgr.RimNo.IsNull? 0 : this._oToucheMsgr.RimNo.Value),
				(this._oToucheMsgr.ResContactNumber.IsNull? string.Empty : this._oToucheMsgr.ResContactNumber.Value),
				null,
				null,
				null);
			//#13321	
			//temp.Workspace =  PApplication.Instance.ActiveWksWindow; //this.Workspace;
            temp.Workspace = this.Workspace; // Vijay Added 
			temp.Show();

        }

        #region #15828
        private void GetMessengerData(int nAcctRimNo, string sAcctType, string sAcctNo, int nAcctId)
		{
			//Get the AD_GB_RSM Details.
			#region Iterate the AD_GB_RSM object to get the s&s flags
            _adGbRsm = (Phoenix.BusObj.Admin.Global.AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm];
			#endregion

            this._oToucheMsgr.MsgrPopUp.Value = this._adGbRsm.SalesPopUp.Value;

            if (this._oToucheMsgr.MsgrPopUp.Value == "Y")
			{
				//Get the Touche message			
                if (nAcctRimNo <= 0)
                {
                    if (Phoenix.FrameWork.Core.CoreService.LogPublisher.IsLogEnabled)
                    {
                        Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("**** ERROR - The RIM Number sent to Touche Messemger is: " + nAcctRimNo + ".  A valid RIM Number is required for fetch message from Touche.");
                    }

                    this._oToucheMsgr.DestroyMsgrWindow.Value = "Y";
                }
                else
                {
                    sToucheMsg = this._oToucheMsgr.GetToucheMessage(nAcctRimNo);
                }
			}
		}
        #endregion
        //Begin 122269
        private void FrmOnMessenger_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = true;
            }
        }
        //End 122269
        #endregion
    }
}
