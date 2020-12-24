#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmTlJournal.cs
// NameSpace: Phoenix.Client.Journal
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//07/24/2006    2       Vreddy		#69335 - Limited UserName to 8 Chars and password to 14 and Removed to Upper for Password, Added Change Modification log
//08/26/2008    3       jabreu      #76091 - SSO functionality
//5/6/2009      4       Ashok       #2360- In Offline- For Override just ask for Network logon of Supervisor.
//02/24/2012    5       mselvaga    #16946 - Employee clicked override in error, cancels out of override and receives Authenticating user message.
//08/31/2012	6		fspath		#140772 - Changing the instruction if coming from inventory verification window a -1 will be passed in to the parameter.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Authentication;
using Phoenix.Shared.Variables;         //2360
using Phoenix.Client.OfflineRsm;        //2360

#region #76091

using Phoenix.Shared.Authentication;

#endregion

namespace Phoenix.Windows.TlOverride
{
	/// <summary>
	/// Summary description for dlgForcePostPassword.
	/// </summary>
	public class dlgOverride : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbInstructions;
		private Phoenix.Windows.Forms.PLabelStandard lblUserName;
		private Phoenix.Windows.Forms.PLabelStandard lblPassword;
		private Phoenix.Windows.Forms.PAction pbCancel;
		private Phoenix.Windows.Forms.PAction pbOk;
		private Phoenix.Windows.Forms.PdfStandard dfPassword;
		private Phoenix.Windows.Forms.PLabelStandard lblInstruction;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbAuthorizedUserInformation;
		private Phoenix.Windows.Forms.PdfStandard dfUserName;

		#region private vars
		private PSmallInt _superEmplId;
        private TellerVars _tellerVars = TellerVars.Instance;       //2360
		#endregion

		public dlgOverride()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gbInstructions = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.lblInstruction = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbAuthorizedUserInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfPassword = new Phoenix.Windows.Forms.PdfStandard();
			this.lblPassword = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfUserName = new Phoenix.Windows.Forms.PdfStandard();
			this.lblUserName = new Phoenix.Windows.Forms.PLabelStandard();
			this.pbCancel = new Phoenix.Windows.Forms.PAction();
			this.pbOk = new Phoenix.Windows.Forms.PAction();
			this.gbInstructions.SuspendLayout();
			this.gbAuthorizedUserInformation.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionManager
			// 
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbOk,
            this.pbCancel});
			// 
			// gbInstructions
			// 
			this.gbInstructions.Controls.Add(this.lblInstruction);
			this.gbInstructions.Location = new System.Drawing.Point(4, 0);
			this.gbInstructions.Name = "gbInstructions";
			this.gbInstructions.PhoenixUIControl.ObjectId = 1;
			this.gbInstructions.Size = new System.Drawing.Size(306, 60);
			this.gbInstructions.TabIndex = 2;
			this.gbInstructions.TabStop = false;
			this.gbInstructions.Text = "Instructions";
			// 
			// lblInstruction
			// 
			this.lblInstruction.AutoEllipsis = true;
			this.lblInstruction.Location = new System.Drawing.Point(7, 13);
			this.lblInstruction.Name = "lblInstruction";
			this.lblInstruction.PhoenixUIControl.ObjectId = 1;
			this.lblInstruction.Size = new System.Drawing.Size(289, 39);
			this.lblInstruction.TabIndex = 0;
			this.lblInstruction.Text = "Please enter the user name and password for a supervisor that has sufficient righ" +
    "ts to override the highlighted error(s).";
			this.lblInstruction.WordWrap = true;
			// 
			// gbAuthorizedUserInformation
			// 
			this.gbAuthorizedUserInformation.Controls.Add(this.dfPassword);
			this.gbAuthorizedUserInformation.Controls.Add(this.lblPassword);
			this.gbAuthorizedUserInformation.Controls.Add(this.dfUserName);
			this.gbAuthorizedUserInformation.Controls.Add(this.lblUserName);
			this.gbAuthorizedUserInformation.Location = new System.Drawing.Point(4, 60);
			this.gbAuthorizedUserInformation.Name = "gbAuthorizedUserInformation";
			this.gbAuthorizedUserInformation.PhoenixUIControl.ObjectId = 2;
			this.gbAuthorizedUserInformation.Size = new System.Drawing.Size(306, 64);
			this.gbAuthorizedUserInformation.TabIndex = 0;
			this.gbAuthorizedUserInformation.TabStop = false;
			this.gbAuthorizedUserInformation.Text = "Identification Information";
			// 
			// dfPassword
			// 
			this.dfPassword.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPassword.Location = new System.Drawing.Point(119, 39);
			this.dfPassword.MaxLength = 14;
			this.dfPassword.Name = "dfPassword";
			this.dfPassword.PasswordChar = '*';
			this.dfPassword.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPassword.PhoenixUIControl.ObjectId = 4;
			this.dfPassword.Size = new System.Drawing.Size(113, 20);
			this.dfPassword.TabIndex = 3;
			// 
			// lblPassword
			// 
			this.lblPassword.AutoEllipsis = true;
			this.lblPassword.Location = new System.Drawing.Point(6, 39);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.PhoenixUIControl.ObjectId = 4;
			this.lblPassword.Size = new System.Drawing.Size(63, 20);
			this.lblPassword.TabIndex = 2;
			this.lblPassword.Text = "&Password:";
			// 
			// dfUserName
			// 
			this.dfUserName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			this.dfUserName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfUserName.Location = new System.Drawing.Point(119, 15);
			this.dfUserName.MaxLength = 14;
			this.dfUserName.Name = "dfUserName";
			this.dfUserName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfUserName.PhoenixUIControl.ObjectId = 3;
			this.dfUserName.Size = new System.Drawing.Size(113, 20);
			this.dfUserName.TabIndex = 1;
			// 
			// lblUserName
			// 
			this.lblUserName.AutoEllipsis = true;
			this.lblUserName.Location = new System.Drawing.Point(6, 15);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.PhoenixUIControl.ObjectId = 3;
			this.lblUserName.Size = new System.Drawing.Size(69, 20);
			this.lblUserName.TabIndex = 0;
			this.lblUserName.Text = "&User Name:";
			// 
			// pbCancel
			// 
			this.pbCancel.ObjectId = 7;
			this.pbCancel.Tag = null;
			this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
			// 
			// pbOk
			// 
			this.pbOk.ObjectId = 6;
			this.pbOk.Tag = null;
			this.pbOk.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOk_Click);
			// 
			// dlgOverride
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 129);
			this.Controls.Add(this.gbAuthorizedUserInformation);
			this.Controls.Add(this.gbInstructions);
			this.Name = "dlgOverride";
			this.ScreenId = 16002;
			this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgOverride_PInitCompleteEvent);
			this.gbInstructions.ResumeLayout(false);
			this.gbAuthorizedUserInformation.ResumeLayout(false);
			this.gbAuthorizedUserInformation.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		public override void InitParameters(params object[] paramList)
		{

			//If the value is -1 it is coming from the inventory verification window in order to change the instruction text. //#140772
			if( paramList.Length == 1 )
				_superEmplId = paramList[0] as PSmallInt;

		}


		private void dlgOverride_PInitCompleteEvent()
		{
			this.pbOk.Image = Images.Ok;
			this.pbCancel.Image = Images.Cancel;
			this.DefaultAction = this.pbOk;
			if (_superEmplId.IntValue == -1) //#140772
			{
				this.lblInstruction.Text = @"Please enter the user name and password for a employee that will be verifying your inventory type.";
			}
			else
			{
				this.lblInstruction.Text = @"Please enter the user name and password for a supervisor that has sufficient rights to override the highlighted error(s).";
			}

			
			#region #76091
			
			if (BusGlobalVars.SsoEnabled)
			{
				pbOk.ResetObject(8);
				gbAuthorizedUserInformation.Visible = false;
			}
			
			#endregion
			
		}

		#region pbOk
		private void pbOk_Click( object sender, PActionEventArgs e )
		{
			try
			{
				UserLogon logonObj = new UserLogon();
				dlgInformation.Instance.ShowInfo( "Authenticating user..."  );
				
				#region #76091
				
				if (BusGlobalVars.SsoEnabled)
				{
                    PDialogResult dlgCredResult   = PDialogResult.Cancel;        //2360
                    DialogResult dlgOfflineResult = DialogResult.Cancel;        //2360
                    string networkUserName  =   string.Empty;

                    if (_tellerVars.IsAppOnline)
                    {
                        CredentialPrompt credUI = new CredentialPrompt();
                        dlgCredResult = credUI.Show(this);
                        if (dlgCredResult == PDialogResult.OK)
                            networkUserName = credUI.UserName;
                    }
                    else if (_tellerVars.IsOfflineSupported && _tellerVars.IsOfflineDbAvailable)//2360
                    {
                        frmSelectOfflineRsm frmSsoUsers = new frmSelectOfflineRsm();
                        dlgOfflineResult = frmSsoUsers.ShowDialog(this);
                        if (dlgOfflineResult == DialogResult.OK)
                            networkUserName = frmSsoUsers.NetworkAcct;
                    }

                    if ( (dlgCredResult == PDialogResult.OK || dlgOfflineResult == DialogResult.OK) && networkUserName.Length > 0)
                    {

                        if (logonObj.VerifyUser(networkUserName, String.Empty))
                        {

                            dlgInformation.Instance.ShowInfo(null);
                            _superEmplId.Value = (short)logonObj.EmployeeId;
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            dlgInformation.Instance.ShowInfo(null);
                            PMessageBox.Show(this, PMessageBox.DefaultId, MessageType.Error, MessageBoxButtons.OK, (logonObj.FailReason == null) ? logonObj.ErrorInfo : logonObj.FailReason);
                        }

                    }
                    else
                    {
                        dlgInformation.Instance.ShowInfo(null); //#16946
                        e.IsSuccess = false;
                        return;
                    }

				}
				else
				{
				
				#endregion

					if (logonObj.VerifyUser(dfUserName.Text, dfPassword.Text))
					{

						dlgInformation.Instance.ShowInfo(null);
						_superEmplId.Value = (short)logonObj.EmployeeId;
						this.DialogResult = DialogResult.OK;
						this.Close();
					}
					else
					{
						dlgInformation.Instance.ShowInfo(null);
						PMessageBox.Show(this, PMessageBox.DefaultId, MessageType.Error, MessageBoxButtons.OK, (logonObj.FailReason == null) ? logonObj.ErrorInfo : logonObj.FailReason);
					}

				#region #76091
				
				}
				
				#endregion
				
			}
			catch ( Exception ex )
			{
				dlgInformation.Instance.ShowInfo(null);
				PMessageBox.Show( this, PMessageBox.DefaultId, MessageType.Error, MessageBoxButtons.OK, ex.Message );

			}
		}
		#endregion

		#region pbCancel
		private void pbCancel_Click( object sender, PActionEventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion





	}
}

