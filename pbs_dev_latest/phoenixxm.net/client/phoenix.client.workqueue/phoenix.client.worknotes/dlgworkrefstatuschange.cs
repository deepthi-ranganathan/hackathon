#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - dlgWorkrefStatusChange.cs - Edit ther Request record.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: dlgWorkrefStatusChange.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/16/06		1		VDevadoss	#69248 - Created
//01/25/2009    2       Nelsehety   #01698 - Bug Fixing
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Phoenix.BusObj.Global;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;
using Phoenix.FrameWork.Translation;

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for dlgWorkRefStatusChange.
	/// </summary>
	public class dlgWorkRefStatusChange : Phoenix.Windows.Forms.PfwStandard
	{
		#region Public Vars (used similar to return vars)
		public int nReasonId = int.MinValue;
		public string sReasonType = string.Empty;
		#endregion

		#region Private Variables
		private Phoenix.BusObj.Global.GbWorkQueue _gbWorkQueue = new Phoenix.BusObj.Global.GbWorkQueue();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbStatusChangeInformation;
		private Phoenix.Windows.Forms.PdfStandard dfMsgText;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbNoChange;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbSold;
		private  Phoenix.Windows.Forms.PRadioButtonStandard rbDeclined;
		private Phoenix.Windows.Forms.PLabelStandard lblReason;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbReason;
		#endregion

		#region Constructor
		public dlgWorkRefStatusChange()
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

		#region Desstructor
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
			this.gbStatusChangeInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.rbDeclined = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbSold = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbNoChange = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.dfMsgText = new Phoenix.Windows.Forms.PdfStandard();
			this.cmbReason = new Phoenix.Windows.Forms.PComboBoxStandard();
			this.lblReason = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbStatusChangeInformation.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbStatusChangeInformation
			// 
			this.gbStatusChangeInformation.Controls.Add(this.rbDeclined);
			this.gbStatusChangeInformation.Controls.Add(this.rbSold);
			this.gbStatusChangeInformation.Controls.Add(this.rbNoChange);
			this.gbStatusChangeInformation.Controls.Add(this.dfMsgText);
			this.gbStatusChangeInformation.Controls.Add(this.cmbReason);
			this.gbStatusChangeInformation.Controls.Add(this.lblReason);
            this.gbStatusChangeInformation.Location = new System.Drawing.Point(4, 0);
			this.gbStatusChangeInformation.Name = "gbStatusChangeInformation";
			this.gbStatusChangeInformation.PhoenixUIControl.ObjectId = 6;
            this.gbStatusChangeInformation.Size = new System.Drawing.Size(502, 164);
			this.gbStatusChangeInformation.TabIndex = 0;
			this.gbStatusChangeInformation.TabStop = false;
			this.gbStatusChangeInformation.Text = "Status Change Information";
			// 
			// rbDeclined
			// 
			this.rbDeclined.Description = null;
			this.rbDeclined.Location = new System.Drawing.Point(428, 108);
			this.rbDeclined.Name = "rbDeclined";
			this.rbDeclined.PhoenixUIControl.ObjectId = 3;
			this.rbDeclined.Size = new System.Drawing.Size(64, 16);
			this.rbDeclined.TabIndex = 3;
			this.rbDeclined.Text = "Declined";
			this.rbDeclined.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbDeclined_PhoenixUICheckedChangedEvent);
			// 
			// rbSold
			// 
			this.rbSold.Description = null;
			this.rbSold.Location = new System.Drawing.Point(240, 108);
			this.rbSold.Name = "rbSold";
			this.rbSold.PhoenixUIControl.ObjectId = 2;
			this.rbSold.Size = new System.Drawing.Size(46, 16);
			this.rbSold.TabIndex = 2;
			this.rbSold.Text = "Sold";
			this.rbSold.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbSold_PhoenixUICheckedChangedEvent);
			// 
			// rbNoChange
			// 
			this.rbNoChange.Description = null;
			this.rbNoChange.IsMaster = true;
			this.rbNoChange.Location = new System.Drawing.Point(8, 108);
			this.rbNoChange.Name = "rbNoChange";
			this.rbNoChange.PhoenixUIControl.ObjectId = 1;
			this.rbNoChange.Size = new System.Drawing.Size(104, 16);
			this.rbNoChange.TabIndex = 1;
			this.rbNoChange.Text = "No Change";
			this.rbNoChange.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbNoChange_PhoenixUICheckedChangedEvent);
			// 
			// dfMsgText
			// 
			this.dfMsgText.AcceptsReturn = true;
			this.dfMsgText.Location = new System.Drawing.Point(8, 16);
			this.dfMsgText.Multiline = true;
			this.dfMsgText.Name = "dfMsgText";
			this.dfMsgText.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfMsgText.PhoenixUIControl.ObjectId = 8;
			this.dfMsgText.ReadOnly = true;
            this.dfMsgText.Size = new System.Drawing.Size(488, 84);
			this.dfMsgText.TabIndex = 0;
			this.dfMsgText.TabStop = false;
			// 
			// cmbReason
			// 
            this.cmbReason.Location = new System.Drawing.Point(64, 136);
			this.cmbReason.Name = "cmbReason";
			this.cmbReason.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
			this.cmbReason.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.True;
			this.cmbReason.PhoenixUIControl.ObjectId = 4;
			this.cmbReason.Size = new System.Drawing.Size(432, 21);
			this.cmbReason.TabIndex = 5;
			// 
			// lblReason
			// 
            this.lblReason.AutoEllipsis = true;
			this.lblReason.Location = new System.Drawing.Point(8, 136);
			this.lblReason.Name = "lblReason";
			this.lblReason.PhoenixUIControl.ObjectId = 4;
			this.lblReason.Size = new System.Drawing.Size(52, 16);
			this.lblReason.TabIndex = 4;
			this.lblReason.Text = "Reason:";
			// 
			// dlgWorkRefStatusChange
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(508, 166);
			this.Controls.Add(this.gbStatusChangeInformation);
			this.Name = "dlgWorkRefStatusChange";
			this.ScreenId = 12543;
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgWorkRefStatusChange_PInitCompleteEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgWorkRefStatusChange_PInitBeginEvent);
			this.gbStatusChangeInformation.ResumeLayout(false);
            this.gbStatusChangeInformation.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region Window Events

		#region Window Begin/Complete Events
		private Phoenix.Windows.Forms.ReturnType dlgWorkRefStatusChange_PInitBeginEvent()
		{
			this.MainBusinesObject = _gbWorkQueue;
			this.IsNew = false;
			this.AutoFetch = false;
			this.ActionClose.ObjectId = 5;
			return ReturnType.Success;
		}

		private void dlgWorkRefStatusChange_PInitCompleteEvent()
		{			
			this.rbNoChange.Checked = true;
			RefreshReason();

			this.dfMsgText.UnFormattedValue = Phoenix.FrameWork.Core.CoreService.Translation.GetUserMessageX(360683);
			this.dfMsgText.UnFormattedValue += Phoenix.FrameWork.Core.CoreService.Translation.GetUserMessageX(360684);
			this.dfMsgText.UnFormattedValue += "\r\n\r\n" + Phoenix.FrameWork.Core.CoreService.Translation.GetUserMessageX(360685);
		}

		#endregion

		#region Radio Button Events
		private void rbNoChange_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
		{
			this.cmbReason.Items.Clear();

            cmbReason.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);

		}

		private void rbSold_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
		{
			this._gbWorkQueue.WorkStatus.Value = "Sold";

            cmbReason.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);

			RefreshReason();
		}

		private void rbDeclined_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
		{
			this._gbWorkQueue.WorkStatus.Value = "Declined";

            cmbReason.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);

			RefreshReason();
		}
		#endregion

		#region Push Button Events
		public override bool OnActionClose()
		{
			if (this.cmbReason.Text.Trim() != string.Empty)
				this.nReasonId = Convert.ToInt32(cmbReason.CodeValue.ToString());

			if (this.rbNoChange.Checked)
				sReasonType = "NoChange";
			else if (this.rbSold.Checked)
				sReasonType = "Sold";
			else if (this.rbDeclined.Checked)
				sReasonType = "Declined";

            this.DialogResult = DialogResult.OK;

			return base.OnActionClose ();
		}

		#endregion

		#endregion

		#region Private Functions
		private void RefreshReason()
		{
			if (!this.rbNoChange.Checked)
			{
				Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.ReasonId);
				cmbReason.Populate(_gbWorkQueue.ReasonId.Constraint.EnumValues);
			}
			else
				this.cmbReason.Items.Clear();
		}
		#endregion
	}
}
