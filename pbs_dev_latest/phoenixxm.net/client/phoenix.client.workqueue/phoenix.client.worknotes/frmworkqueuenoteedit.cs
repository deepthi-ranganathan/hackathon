#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - frmWorkQueueNoteEdit.cs - Edit ther Request record.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: frmWorkQueueNoteEdit.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/16/06		1		VDevadoss	#69248 - Created
//11/17/2008    2       Nelsehety   #01698 - .Net Porting
//01/22/2009    3       Nelsehety   #01698 - Bug Fixing
//              4       MOna        #04262 - Added Validate Event of DueTime
//08/06/2012    5       Mkrishna    #19058 - Adding call to base on initParameters.
//02/19/2015    6       Alfred      #174961 - Sales and Service. New radio buttons added.
//09/17/2015    7       Hirankumar  #38805 - 174961 - 174961 - Sales and Service - Referrals are not being scored properly in the scoring Functionality - 2 window changes.
//09/07/2017    8       SivaHCL     #70476 - Phoenix Error - #102 Incorrect syntax when setting a task in Sales and Service to Decline and Email owner is selected
//05/09/2018    9       Sudha-HCL   #84686-Master CAS-1644428-GB_WORK_QUEUE_NOTE is not writing to audit trail correctly nor updating COMPLETED_DT when the task is set to completed
//01/10/2019    10      SChacko     Task#108099 - modified code changes done in Task#84686
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

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for frmWorkQueueNoteEdit.
	/// </summary>
	public class frmWorkQueueNoteEdit : Phoenix.Windows.Forms.PfwStandard
	{
		#region Private Variables
		private Phoenix.BusObj.Global.GbHelper _gbHelper = new Phoenix.BusObj.Global.GbHelper();
		private Phoenix.BusObj.Global.GbWorkQueue _gbWorkQueue = new Phoenix.BusObj.Global.GbWorkQueue();
		private Phoenix.BusObj.Global.GbWorkQueueNote _gbWorkQueueNote = new Phoenix.BusObj.Global.GbWorkQueueNote();
		private int nRimNo = int.MinValue;
		private string sCustomerName = string.Empty;
		private string sAcctType = string.Empty;
		private string sAcctNo = string.Empty;
		private bool bRefreshParent = false;

        private string _source;
		//private bool _inAddNext;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbNoteInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblType;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbNoteType;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbEmailOwner;
		private Phoenix.Windows.Forms.PLabelStandard lblDueDate;
		private Phoenix.Windows.Forms.PdfStandard dfDueDt;
		private Phoenix.Windows.Forms.PLabelStandard lblTime;
		private Phoenix.Windows.Forms.PdfStandard dfDueTime;
		private Phoenix.Windows.Forms.PLabelStandard lblDescription;
		private Phoenix.Windows.Forms.PdfStandard dfNoteTitle;
		private Phoenix.Windows.Forms.PLabelStandard lblDetails;
		private Phoenix.Windows.Forms.PdfStandard mlNoteText;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbResolution;
		private Phoenix.Windows.Forms.PLabelStandard lblComments;
		private Phoenix.Windows.Forms.PdfStandard mlComments;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbControl;
		private Phoenix.Windows.Forms.PLabelStandard lblEffective;
		private Phoenix.Windows.Forms.PdfStandard dfCreateDt;
		private Phoenix.Windows.Forms.PLabelStandard lblStatus;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbStatus;
		private Phoenix.Windows.Forms.PdfStandard dfNoteText1;
		private Phoenix.Windows.Forms.PdfStandard dfNoteText2;
		private Phoenix.Windows.Forms.PdfStandard dfNoteText4;
		private Phoenix.Windows.Forms.PdfStandard dfNoteText3;
		private Phoenix.Windows.Forms.PdfStandard dfDueDateWithTime;
        private PRadioButtonStandard rbSold;
        private PRadioButtonStandard rbDeclined;
        private PLabelStandard lblProdServDecesion;
		private Phoenix.Windows.Forms.PdfStandard dfCreateDtWithTime;		
		#endregion

		#region Constructor
		public frmWorkQueueNoteEdit()
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
            this.gbNoteInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.mlNoteText = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDetails = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNoteTitle = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueTime = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTime = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueDt = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDueDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbEmailOwner = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbNoteType = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblType = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbResolution = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.mlComments = new Phoenix.Windows.Forms.PdfStandard();
            this.lblComments = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbControl = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblProdServDecesion = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbSold = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbDeclined = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.cmbStatus = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCreateDt = new Phoenix.Windows.Forms.PdfStandard();
            this.lblEffective = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNoteText1 = new Phoenix.Windows.Forms.PdfStandard();
            this.dfNoteText2 = new Phoenix.Windows.Forms.PdfStandard();
            this.dfNoteText4 = new Phoenix.Windows.Forms.PdfStandard();
            this.dfNoteText3 = new Phoenix.Windows.Forms.PdfStandard();
            this.dfCreateDtWithTime = new Phoenix.Windows.Forms.PdfStandard();
            this.dfDueDateWithTime = new Phoenix.Windows.Forms.PdfStandard();
            this.gbNoteInformation.SuspendLayout();
            this.gbResolution.SuspendLayout();
            this.gbControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbNoteInformation
            // 
            this.gbNoteInformation.Controls.Add(this.mlNoteText);
            this.gbNoteInformation.Controls.Add(this.lblDetails);
            this.gbNoteInformation.Controls.Add(this.dfNoteTitle);
            this.gbNoteInformation.Controls.Add(this.lblDescription);
            this.gbNoteInformation.Controls.Add(this.dfDueTime);
            this.gbNoteInformation.Controls.Add(this.lblTime);
            this.gbNoteInformation.Controls.Add(this.dfDueDt);
            this.gbNoteInformation.Controls.Add(this.lblDueDate);
            this.gbNoteInformation.Controls.Add(this.cbEmailOwner);
            this.gbNoteInformation.Controls.Add(this.cmbNoteType);
            this.gbNoteInformation.Controls.Add(this.lblType);
            this.gbNoteInformation.Location = new System.Drawing.Point(4, 0);
            this.gbNoteInformation.Name = "gbNoteInformation";
            this.gbNoteInformation.PhoenixUIControl.ObjectId = 1;
            this.gbNoteInformation.Size = new System.Drawing.Size(732, 172);
            this.gbNoteInformation.TabIndex = 0;
            this.gbNoteInformation.TabStop = false;
            this.gbNoteInformation.Text = "Note Information";
            // 
            // mlNoteText
            // 
            this.mlNoteText.AcceptsReturn = true;
            this.mlNoteText.Location = new System.Drawing.Point(128, 88);
            this.mlNoteText.MaxLength = 1000;
            this.mlNoteText.Multiline = true;
            this.mlNoteText.Name = "mlNoteText";
            this.mlNoteText.PhoenixUIControl.ObjectId = 6;
            this.mlNoteText.PreviousValue = null;
            this.mlNoteText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mlNoteText.Size = new System.Drawing.Size(600, 77);
            this.mlNoteText.TabIndex = 12;
            this.mlNoteText.Validated += new System.EventHandler(this.mlNoteText_Validated);
            // 
            // lblDetails
            // 
            this.lblDetails.AutoEllipsis = true;
            this.lblDetails.Location = new System.Drawing.Point(8, 88);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.PhoenixUIControl.ObjectId = 6;
            this.lblDetails.Size = new System.Drawing.Size(64, 20);
            this.lblDetails.TabIndex = 11;
            this.lblDetails.Text = "Note Text:";
            // 
            // dfNoteTitle
            // 
            this.dfNoteTitle.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNoteTitle.Location = new System.Drawing.Point(128, 64);
            this.dfNoteTitle.MaxLength = 80;
            this.dfNoteTitle.Name = "dfNoteTitle";
            this.dfNoteTitle.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfNoteTitle.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.dfNoteTitle.PhoenixUIControl.ObjectId = 5;
            this.dfNoteTitle.PreviousValue = null;
            this.dfNoteTitle.Size = new System.Drawing.Size(600, 20);
            this.dfNoteTitle.TabIndex = 10;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 64);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 5;
            this.lblDescription.Size = new System.Drawing.Size(112, 20);
            this.lblDescription.TabIndex = 9;
            // 
            // dfDueTime
            // 
            this.dfDueTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.Location = new System.Drawing.Point(287, 40);
            this.dfDueTime.Name = "dfDueTime";
            this.dfDueTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfDueTime.PhoenixUIControl.ObjectId = 16;
            this.dfDueTime.PreviousValue = null;
            this.dfDueTime.Size = new System.Drawing.Size(71, 20);
            this.dfDueTime.TabIndex = 8;
            this.dfDueTime.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfDueTime_PhoenixUIValidateEvent);
            // 
            // lblTime
            // 
            this.lblTime.AutoEllipsis = true;
            this.lblTime.Location = new System.Drawing.Point(248, 40);
            this.lblTime.Name = "lblTime";
            this.lblTime.PhoenixUIControl.ObjectId = 16;
            this.lblTime.Size = new System.Drawing.Size(36, 20);
            this.lblTime.TabIndex = 7;
            this.lblTime.Text = "Time:";
            // 
            // dfDueDt
            // 
            this.dfDueDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDt.Location = new System.Drawing.Point(128, 40);
            this.dfDueDt.Name = "dfDueDt";
            this.dfDueDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfDueDt.PhoenixUIControl.ObjectId = 4;
            this.dfDueDt.PreviousValue = null;
            this.dfDueDt.Size = new System.Drawing.Size(74, 20);
            this.dfDueDt.TabIndex = 6;
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoEllipsis = true;
            this.lblDueDate.Location = new System.Drawing.Point(8, 40);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.PhoenixUIControl.ObjectId = 4;
            this.lblDueDate.Size = new System.Drawing.Size(56, 20);
            this.lblDueDate.TabIndex = 5;
            this.lblDueDate.Text = "Due Date:";
            // 
            // cbEmailOwner
            // 
            this.cbEmailOwner.BackColor = System.Drawing.SystemColors.Control;
            this.cbEmailOwner.Checked = true;
            this.cbEmailOwner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEmailOwner.Location = new System.Drawing.Point(412, 16);
            this.cbEmailOwner.Name = "cbEmailOwner";
            this.cbEmailOwner.PhoenixUIControl.ObjectId = 3;
            this.cbEmailOwner.PhoenixUIControl.XmlTag = "";
            this.cbEmailOwner.Size = new System.Drawing.Size(111, 20);
            this.cbEmailOwner.TabIndex = 2;
            this.cbEmailOwner.Text = "E-Mail Owner";
            this.cbEmailOwner.UseVisualStyleBackColor = false;
            this.cbEmailOwner.Value = null;
            // 
            // cmbNoteType
            // 
            this.cmbNoteType.Location = new System.Drawing.Point(128, 16);
            this.cmbNoteType.Name = "cmbNoteType";
            this.cmbNoteType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbNoteType.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.False;
            this.cmbNoteType.PhoenixUIControl.ObjectId = 2;
            this.cmbNoteType.Size = new System.Drawing.Size(230, 21);
            this.cmbNoteType.TabIndex = 1;
            this.cmbNoteType.Value = null;
            this.cmbNoteType.SelectedIndexChanged += new System.EventHandler(this.cmbNoteType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoEllipsis = true;
            this.lblType.Location = new System.Drawing.Point(8, 16);
            this.lblType.Name = "lblType";
            this.lblType.PhoenixUIControl.ObjectId = 2;
            this.lblType.Size = new System.Drawing.Size(52, 20);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type:";
            // 
            // gbResolution
            // 
            this.gbResolution.Controls.Add(this.mlComments);
            this.gbResolution.Controls.Add(this.lblComments);
            this.gbResolution.Location = new System.Drawing.Point(4, 172);
            this.gbResolution.Name = "gbResolution";
            this.gbResolution.PhoenixUIControl.ObjectId = 7;
            this.gbResolution.Size = new System.Drawing.Size(732, 88);
            this.gbResolution.TabIndex = 1;
            this.gbResolution.TabStop = false;
            this.gbResolution.Text = "Resolution";
            // 
            // mlComments
            // 
            this.mlComments.AcceptsReturn = true;
            this.mlComments.Location = new System.Drawing.Point(128, 12);
            this.mlComments.MaxLength = 254;
            this.mlComments.Multiline = true;
            this.mlComments.Name = "mlComments";
            this.mlComments.PhoenixUIControl.ObjectId = 8;
            this.mlComments.PreviousValue = null;
            this.mlComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mlComments.Size = new System.Drawing.Size(600, 71);
            this.mlComments.TabIndex = 1;
            // 
            // lblComments
            // 
            this.lblComments.AutoEllipsis = true;
            this.lblComments.Location = new System.Drawing.Point(8, 12);
            this.lblComments.Name = "lblComments";
            this.lblComments.PhoenixUIControl.ObjectId = 8;
            this.lblComments.Size = new System.Drawing.Size(63, 20);
            this.lblComments.TabIndex = 0;
            this.lblComments.Text = "Comments:";
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.lblProdServDecesion);
            this.gbControl.Controls.Add(this.rbSold);
            this.gbControl.Controls.Add(this.rbDeclined);
            this.gbControl.Controls.Add(this.cmbStatus);
            this.gbControl.Controls.Add(this.lblStatus);
            this.gbControl.Controls.Add(this.dfCreateDt);
            this.gbControl.Controls.Add(this.lblEffective);
            this.gbControl.Location = new System.Drawing.Point(4, 260);
            this.gbControl.Name = "gbControl";
            this.gbControl.PhoenixUIControl.ObjectId = 9;
            this.gbControl.Size = new System.Drawing.Size(732, 64);
            this.gbControl.TabIndex = 2;
            this.gbControl.TabStop = false;
            this.gbControl.Text = "Control";
            // 
            // lblProdServDecesion
            // 
            this.lblProdServDecesion.AutoEllipsis = true;
            this.lblProdServDecesion.Location = new System.Drawing.Point(472, 40);
            this.lblProdServDecesion.Name = "lblProdServDecesion";
            this.lblProdServDecesion.PhoenixUIControl.ObjectId = 20;
            this.lblProdServDecesion.Size = new System.Drawing.Size(136, 20);
            this.lblProdServDecesion.TabIndex = 4;
            this.lblProdServDecesion.Text = "Product/Service Decision:";
            // 
            // rbSold
            // 
            this.rbSold.BackColor = System.Drawing.SystemColors.Control;
            this.rbSold.Description = null;
            this.rbSold.Location = new System.Drawing.Point(679, 40);
            this.rbSold.Name = "rbSold";
            this.rbSold.PhoenixUIControl.ObjectId = 19;
            this.rbSold.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbSold.Size = new System.Drawing.Size(44, 20);
            this.rbSold.TabIndex = 6;
            this.rbSold.Text = "Sold";
            this.rbSold.UseVisualStyleBackColor = false;
            // 
            // rbDeclined
            // 
            this.rbDeclined.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeclined.Description = null;
            this.rbDeclined.IsMaster = true;
            this.rbDeclined.Location = new System.Drawing.Point(612, 40);
            this.rbDeclined.Name = "rbDeclined";
            this.rbDeclined.PhoenixUIControl.ObjectId = 18;
            this.rbDeclined.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbDeclined.Size = new System.Drawing.Size(64, 20);
            this.rbDeclined.TabIndex = 5;
            this.rbDeclined.Text = "Declined";
            this.rbDeclined.UseVisualStyleBackColor = false;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Location = new System.Drawing.Point(612, 12);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.False;
            this.cmbStatus.PhoenixUIControl.ObjectId = 11;
            this.cmbStatus.Size = new System.Drawing.Size(116, 21);
            this.cmbStatus.TabIndex = 3;
            this.cmbStatus.Value = null;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoEllipsis = true;
            this.lblStatus.Location = new System.Drawing.Point(472, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.PhoenixUIControl.ObjectId = 11;
            this.lblStatus.Size = new System.Drawing.Size(44, 20);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status:";
            // 
            // dfCreateDt
            // 
            this.dfCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDt.Location = new System.Drawing.Point(128, 12);
            this.dfCreateDt.Name = "dfCreateDt";
            this.dfCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfCreateDt.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfCreateDt.PhoenixUIControl.ObjectId = 10;
            this.dfCreateDt.PreviousValue = null;
            this.dfCreateDt.Size = new System.Drawing.Size(74, 20);
            this.dfCreateDt.TabIndex = 1;
            // 
            // lblEffective
            // 
            this.lblEffective.AutoEllipsis = true;
            this.lblEffective.Location = new System.Drawing.Point(8, 12);
            this.lblEffective.Name = "lblEffective";
            this.lblEffective.PhoenixUIControl.ObjectId = 10;
            this.lblEffective.Size = new System.Drawing.Size(64, 20);
            this.lblEffective.TabIndex = 0;
            this.lblEffective.Text = "Effective:";
            // 
            // dfNoteText1
            // 
            this.dfNoteText1.Location = new System.Drawing.Point(4, 336);
            this.dfNoteText1.Name = "dfNoteText1";
            this.dfNoteText1.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfNoteText1.PreviousValue = null;
            this.dfNoteText1.Size = new System.Drawing.Size(724, 20);
            this.dfNoteText1.TabIndex = 4;
            this.dfNoteText1.Visible = false;
            // 
            // dfNoteText2
            // 
            this.dfNoteText2.Location = new System.Drawing.Point(4, 364);
            this.dfNoteText2.Name = "dfNoteText2";
            this.dfNoteText2.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfNoteText2.PreviousValue = null;
            this.dfNoteText2.Size = new System.Drawing.Size(724, 20);
            this.dfNoteText2.TabIndex = 5;
            this.dfNoteText2.Visible = false;
            // 
            // dfNoteText4
            // 
            this.dfNoteText4.Location = new System.Drawing.Point(4, 420);
            this.dfNoteText4.Name = "dfNoteText4";
            this.dfNoteText4.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfNoteText4.PreviousValue = null;
            this.dfNoteText4.Size = new System.Drawing.Size(724, 20);
            this.dfNoteText4.TabIndex = 7;
            this.dfNoteText4.Visible = false;
            // 
            // dfNoteText3
            // 
            this.dfNoteText3.Location = new System.Drawing.Point(4, 392);
            this.dfNoteText3.Name = "dfNoteText3";
            this.dfNoteText3.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfNoteText3.PreviousValue = null;
            this.dfNoteText3.Size = new System.Drawing.Size(724, 20);
            this.dfNoteText3.TabIndex = 6;
            this.dfNoteText3.Visible = false;
            // 
            // dfCreateDtWithTime
            // 
            this.dfCreateDtWithTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDtWithTime.Location = new System.Drawing.Point(12, 444);
            this.dfCreateDtWithTime.Name = "dfCreateDtWithTime";
            this.dfCreateDtWithTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDtWithTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.dfCreateDtWithTime.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfCreateDtWithTime.PhoenixUIControl.ObjectId = 10;
            this.dfCreateDtWithTime.PreviousValue = null;
            this.dfCreateDtWithTime.Size = new System.Drawing.Size(180, 20);
            this.dfCreateDtWithTime.TabIndex = 4;
            this.dfCreateDtWithTime.Visible = false;
            // 
            // dfDueDateWithTime
            // 
            this.dfDueDateWithTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDateWithTime.Location = new System.Drawing.Point(216, 444);
            this.dfDueDateWithTime.Name = "dfDueDateWithTime";
            this.dfDueDateWithTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDateWithTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.dfDueDateWithTime.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.dfDueDateWithTime.PhoenixUIControl.ObjectId = 10;
            this.dfDueDateWithTime.PreviousValue = null;
            this.dfDueDateWithTime.Size = new System.Drawing.Size(216, 20);
            this.dfDueDateWithTime.TabIndex = 8;
            this.dfDueDateWithTime.Visible = false;
            // 
            // frmWorkQueueNoteEdit
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.dfDueDateWithTime);
            this.Controls.Add(this.dfNoteText4);
            this.Controls.Add(this.dfNoteText3);
            this.Controls.Add(this.dfNoteText2);
            this.Controls.Add(this.dfNoteText1);
            this.Controls.Add(this.gbControl);
            this.Controls.Add(this.gbResolution);
            this.Controls.Add(this.gbNoteInformation);
            this.Controls.Add(this.dfCreateDtWithTime);
            this.Name = "frmWorkQueueNoteEdit";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.EditableWithAddNext;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmWorkQueueNoteEdit_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmWorkQueueNoteEdit_PInitCompleteEvent);
            this.PAddNextInit += new Phoenix.Windows.Forms.FormActionHandler(this.frmWorkQueueNoteEdit_PAddNextInit);
            this.PBeforeSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmWorkQueueNoteEdit_PBeforeSave);
            this.PAfterSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmWorkQueueNoteEdit_PAfterSave);
            this.gbNoteInformation.ResumeLayout(false);
            this.gbNoteInformation.PerformLayout();
            this.gbResolution.ResumeLayout(false);
            this.gbResolution.PerformLayout();
            this.gbControl.ResumeLayout(false);
            this.gbControl.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
        
		#region Init
		public override void InitParameters(params Object[] paramList)
		{
			if (paramList.Length == 7)
			{
				if(paramList[0] != null)
				{
					this._gbWorkQueueNote.Ptid.Value = Convert.ToInt32(paramList[0]);
					this.IsNew = false;
				}
				else
				{
					this.IsNew = true;
				}

				if(paramList[1] != null)
				{
					this._gbWorkQueueNote.WorkId.Value = Convert.ToInt32(paramList[1]);
				}
				if(paramList[2] != null)
				{
					this._gbWorkQueueNote.RecordType.Value = paramList[2].ToString().Trim();

                    _source = paramList[2].ToString().Trim();

				}
				if(paramList[3] != null)
				{
					this.nRimNo = Convert.ToInt32(paramList[3]);
				}
				if(paramList[4] != null)
				{
					this.sCustomerName = paramList[4].ToString().Trim();
				}
				if(paramList[5] != null)
				{
					this.sAcctType = paramList[5].ToString().Trim();
				}
				if(paramList[6] != null)
				{
					this.sAcctNo = paramList[6].ToString().Trim();
				}
                if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "Teller")
                {
				if (this._gbWorkQueueNote.RecordType.Value.Trim() == "Referral")
					this.ScreenId = 12506;
				else
					this.ScreenId = 12518;
                }
                #region 01698 Bug Fixing

                else if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
                    if (this._gbWorkQueueNote.RecordType.Value.Trim() == "Referral")
                        this.ScreenId = Phoenix.Shared.Constants.ScreenId.RefWorkQueueNoteEdit;
                    else
                        this.ScreenId = Phoenix.Shared.Constants.ScreenId.ReqWorkQueueNoteEdit;

                #endregion
                this._gbWorkQueueNote.OutputType.Value = this.ScreenId;

			}

			InitXmlTags();
            base.InitParameters(paramList); //#19058

		}
		private void InitXmlTags()
		{ 
			//TODO: Verify the XmlTag Mapping	
			this.cmbNoteType.PhoenixUIControl.XmlTag = "Type";
			this.cbEmailOwner.PhoenixUIControl.XmlTag = "EmailOwner";
			this.dfNoteTitle.PhoenixUIControl.XmlTag = "NoteTitle";
			this.mlComments.PhoenixUIControl.XmlTag = "TaskInfo";
			this.cmbStatus.PhoenixUIControl.XmlTag = "Status";
			this.dfDueDateWithTime.PhoenixUIControl.XmlTag = "DueDt";			
			this.dfCreateDtWithTime.PhoenixUIControl.XmlTag = "CreateDt";			
			this.dfNoteText1.PhoenixUIControl.XmlTag = "NoteText1";
			this.dfNoteText2.PhoenixUIControl.XmlTag = "NoteText2";
			this.dfNoteText3.PhoenixUIControl.XmlTag = "NoteText3";
			this.dfNoteText4.PhoenixUIControl.XmlTag = "NoteText4";
		} 

		#endregion	

		#region Window Events

		#region Window Begin / Complete Events
		private Phoenix.Windows.Forms.ReturnType frmWorkQueueNoteEdit_PInitBeginEvent()
		{
            this.UseStateFromBusinessObject = true;
			this.MainBusinesObject = this._gbWorkQueueNote;
			this.dfCreateDt.UnFormattedValue = Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate;
			return ReturnType.Success;
		}

		private void frmWorkQueueNoteEdit_PInitCompleteEvent()
		{
            #region Set Window Title

            PString sTmpWindowTitle = new PString("TmpWindowTitle");
            PString gsTemp = new PString("gsTemp");

            if (sTmpWindowTitle.Value == string.Empty)            //ITSC Issue #01698 //We do this so as to avoid appending the name multiple times in add next
            {
                if (Phoenix.Shared.StringHelper.StrTrimX(sCustomerName) != string.Empty)
                {
                    gsTemp.Value = (IsNew ? this.NewRecordTitle : this.EditRecordTitle);

                    if (_source == "Referral")
                    {
                        gsTemp.Value = gsTemp.Value + " (" + (sCustomerName) + ")";
                    }
                    else
                    {
                        if (sAcctType != string.Empty && sAcctNo != string.Empty)
                        {
                            gsTemp.Value = gsTemp.Value + " (" + sCustomerName + "  " + sAcctType + " - " + sAcctNo + ")";

                        }
                        else
                        {
                            gsTemp.Value = gsTemp.Value + " (" + sCustomerName;

                        }

                    }
                }

                EditRecordTitle = gsTemp.Value;
                NewRecordTitle = gsTemp.Value;
                sTmpWindowTitle.Value = gsTemp.Value;

            }
            else
            {
                EditRecordTitle = sTmpWindowTitle.Value;
                NewRecordTitle = sTmpWindowTitle.Value;
            }

            #endregion

			if (this.IsNew)
			{
				this.dfCreateDt.UnFormattedValue = Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate;
			}
			else
			{
				if (!this._gbWorkQueueNote.DueDt.IsNull)
				{
					this.dfDueDt.UnFormattedValue = Convert.ToDateTime(this._gbWorkQueueNote.DueDt.Value);
					this.dfDueTime.UnFormattedValue = Convert.ToDateTime(this._gbWorkQueueNote.DueDt.Value);
				}

				SetContactDetails();
			}
            #region Bug Fixing
            if(Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "Teller")
            {
			if (this.IsNew)
				LocCheckPMA();
            }
            else
            {
                LocCheckPMA();

            }
            #endregion

			this.EnableDisableVisibleLogic("Initialize");
			this.DefaultAction = ActionSave;
		}

		#endregion

		#region Validated Events
		private void mlNoteText_Validated(object sender, System.EventArgs e)
		{
			if (this.mlNoteText.Text.Trim() != string.Empty)
				ParseContactDetails(this.mlNoteText.Text.Trim());
		}
		#endregion

		#region Combo Box Events
		private void cmbNoteType_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetObjectStatusForNoteType();
		}
		#endregion

		#region Save Events
		private void frmWorkQueueNoteEdit_PBeforeSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
		{
            if (PerformCheck(CheckType.EditTest,false))
            {
			#region Validations

                if (DoValidations())
                {
			#endregion

			#region Set the Vars for Save work in Bus obj.
			if (this.cmbStatus.Text.Trim() == "Completed" && this.cmbNoteType.Text.Trim() == "Task")
			{
				if (this._gbWorkQueueNote.TotalPendingNoteTasks.Value == 0)
				{
					if (this._gbWorkQueueNote.RecordType.Value == "Request")
					{
						if (this._gbWorkQueueNote.ParentWorkStatus.Value.Trim() != "Completed")
						{
                                    /*319962 - The status of this task has been marked as Completed. 
                                    No other pending tasks exist for this request. Do you wish to change the status of the request itself to Completed?*/
                                    if (PMessageBox.Show(this, 319962, MessageType.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)

                                    {
                                        this._gbWorkQueueNote.UpdateParentStatus.Value = "Y";                                       
                                    }                                  
                                    else
                                        this._gbWorkQueueNote.UpdateParentStatus.Value = "N";
						}
					}
					else
					{


						if (this._gbWorkQueueNote.ParentWorkStatus.Value.Trim() == "Pending" || this._gbWorkQueueNote.ParentWorkStatus.Value.Trim() == "In Process")
						{
							Phoenix.Client.WorkQueue.dlgWorkRefStatusChange tempWin = new Phoenix.Client.WorkQueue.dlgWorkRefStatusChange();
							//tempWin = Phoenix.Windows.Client.Helper.CreateWindow("phoenix.client.worknotes", "Phoenix.Client.WorkQueue", "dlgWorkRefStatusChange" );
							//tempWin.Workspace = this.Workspace;

                                    this.SuspendLayout();

                                    if (tempWin.ShowDialog(this) == DialogResult.OK)
                                    {
							int nSelectedReasonId = tempWin.nReasonId;
							string sSelectedReasonType = tempWin.sReasonType;

							if (sSelectedReasonType == "Sold" || sSelectedReasonType == "Declined")
							{
								if (sSelectedReasonType == "Sold")
									this._gbWorkQueueNote.RefStatusCode.Value = 2;
								else
									this._gbWorkQueueNote.RefStatusCode.Value = 3;

								if (nSelectedReasonId > 0)
									this._gbWorkQueueNote.RefReasonId.Value = Convert.ToInt16(nSelectedReasonId);
							}
                                        this.Invalidate(true);
                                        this.ResumeLayout(true);
                                    }
                                    else
                                    {
                                        this.Invalidate(true);
                                        this.ResumeLayout(true);
                                        e.Cancel = true;
                                        return;
                                    }

						}
					}
				}
			}

			#endregion

			#region Build the Due Date with the time
			if (this.dfDueDt.Text.Trim() != string.Empty)
			{
				DateTime dttmpdt = Convert.ToDateTime(this.dfDueDt.UnFormattedValue);

				if (this.dfDueTime.Text.Trim() != string.Empty)
				{
					DateTime dttmptime = Convert.ToDateTime(this.dfDueTime.UnFormattedValue);
					this.dfDueDateWithTime.UnFormattedValue = new DateTime(dttmpdt.Year, dttmpdt.Month, dttmpdt.Day, dttmptime.Hour, dttmptime.Minute, dttmptime.Second);					
				}
				else
				{
					this.dfDueDateWithTime.UnFormattedValue = new DateTime(dttmpdt.Year, dttmpdt.Month, dttmpdt.Day, 0, 0, 0);
				}

				this._gbWorkQueue.DueDt.Value = Convert.ToDateTime(this.dfDueDateWithTime.UnFormattedValue);
			}
			#endregion	

			#region Set the Complete Dt as needed

			this._gbWorkQueueNote.CompleteDt.Value = DateTime.MinValue;

			if (this.cmbStatus.Text.Trim() == "Completed" && this.cmbNoteType.Text.Trim() == "Task")
			{
				this._gbWorkQueueNote.CompleteDt.Value = new DateTime(DateTime.Now.Year, 
																		DateTime.Now.Month,
																		DateTime.Now.Day,
																		DateTime.Now.Hour,
																		DateTime.Now.Minute, 
																		DateTime.Now.Second);
			}
			#endregion

			#region Set the Create Dt as needed
            //Commented the below code for fixing the WI #38805.
            //this.dfCreateDtWithTime.UnFormattedValue = new DateTime(DateTime.Now.Year, 
            //                                                            DateTime.Now.Month,
            //                                                            DateTime.Now.Day,
            //                                                            DateTime.Now.Hour,
            //                                                            DateTime.Now.Minute, 
            //                                                            DateTime.Now.Second);
			#endregion

			#region Deocde the Status Sort
			if (this.cmbStatus.Text.Trim() == "Pending")
				this._gbWorkQueueNote.StatusSort.Value = 10;
			else
				this._gbWorkQueueNote.StatusSort.Value = 100;
			#endregion

			#region Send Email
			this._gbWorkQueueNote.SendMailSQL.Value = string.Empty;

			if (this.cbEmailOwner.Checked)
                        //#70476 - Begin
                        //this._gbWorkQueueNote.SendWQMail(this.IsNew, this.sCustomerName, this._gbWorkQueueNote.RecordType.Value.Trim(),
                        //                                this.dfNoteText1.Text.Trim(), this.dfNoteText2.Text.Trim(),
                        //                        this.dfNoteText3.Text.Trim(), this.dfNoteText4.Text.Trim());
                        this._gbWorkQueueNote.SendWQMail(this.IsNew, Phoenix.Shared.StringHelper.StringFixApostrophes(this.sCustomerName), this._gbWorkQueueNote.RecordType.Value.Trim(),
                                                            this.dfNoteText1.Text.Trim(), this.dfNoteText2.Text.Trim(),
                                                    this.dfNoteText3.Text.Trim(), this.dfNoteText4.Text.Trim());
                    //#70476 - End
                    #endregion

                    #region Check Mail
                    if (_gbWorkQueueNote.Messages.Count != 0)
                    {
                        HandleBusObjMessages(_gbWorkQueueNote);
                        e.Cancel = true;
                        return;
                    }
                    #endregion

			this.ScreenToObject(XmActionType.Default);
		}

                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

		private void frmWorkQueueNoteEdit_PAfterSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
		{
			bRefreshParent = true;		

            if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
            {
                LocCheckPMA();

            }
			//this.EnableDisableVisibleLogic("AfterSave");
            this.EnableDisableVisibleLogic("Initialize");//#77295 - Needed to adjust the enable disable logic 
                                                        //       - as logic is control from UI , BO logic cause discrepancy   
            
            /*Begin #174961 - Focus is setting back after Save*/
            if (!this.IsNew)
            {
                cbEmailOwner.Focus();
            }
            /*End #174961*/
		}
		#endregion

		#region Pushbutton Events
		public override bool OnActionClose()
		{
			if (base.OnActionClose ())
			{
				PfwStandard parentForm =  this.Workspace.ContentWindow.PreviousWindow as PfwStandard;
				if( parentForm != null && bRefreshParent )
				{
					parentForm.CallParent(this.ScreenId);
				}
				return true;
			}
			return false;
		}
		public override bool OnActionAddNext()
		{
			return base.OnActionAddNext ();
		}
       
        /*Begin #174961*/
        public override bool OnActionSave(bool isAddNext)
        {
            rbDeclined.ScreenToObject();
            rbSold.ScreenToObject();
 
            return base.OnActionSave(isAddNext);
        }
        /*End #174961*/
		#endregion

		#endregion

		#region Functions/Procedures
		private void SetContactDetails()
		{
            #region Bug Fixing

			if (!this._gbWorkQueueNote.NoteText1.IsNull)
			{
				this.mlNoteText.UnFormattedValue = this._gbWorkQueueNote.NoteText1.Value;

				if (!this._gbWorkQueueNote.NoteText2.IsNull)
				{
                    this.mlNoteText.UnFormattedValue = this.mlNoteText.UnFormattedValue + this._gbWorkQueueNote.NoteText2.Value;

					if (!this._gbWorkQueueNote.NoteText3.IsNull)
					{
                        this.mlNoteText.UnFormattedValue = this.mlNoteText.UnFormattedValue + this._gbWorkQueueNote.NoteText3.Value;

						if (!this._gbWorkQueueNote.NoteText4.IsNull)
						{
                            this.mlNoteText.UnFormattedValue = this.mlNoteText.UnFormattedValue + this._gbWorkQueueNote.NoteText4.Value;

						}
					}
				}
			}
            #endregion
		}
		private void ParseContactDetails(string psContactDetails)
		{
			ArrayList sDetailsArr = new ArrayList();

			if (psContactDetails.Trim() != "")
			{
				this._gbHelper.SplitString(psContactDetails.Trim(), 250, ref sDetailsArr);
			}

			int nCntr = 0;
			IEnumerator details = sDetailsArr.GetEnumerator();
			while (details.MoveNext())
			{
				if (nCntr == 0)
					this.dfNoteText1.UnFormattedValue = details.Current.ToString();
				if (nCntr == 1)
					this.dfNoteText2.UnFormattedValue = details.Current.ToString();
				if (nCntr == 2)
					this.dfNoteText3.UnFormattedValue = details.Current.ToString();
				if (nCntr == 3)
					this.dfNoteText4.UnFormattedValue = details.Current.ToString();

				nCntr++;
			}

			return;
		}

		private void EnableDisableVisibleLogic(string caseType)
		{
			switch (caseType)
			{
				case "Initialize":
					#region Initialize Logic
					if (this.IsNew)
					{
                        cmbNoteType.Enabled = true;
						this.cmbStatus.Enabled = false;						
					}
					else
					{
						cmbNoteType.Enabled = false;
						this.cmbStatus.Enabled = true;
						//SetObjectStatusForNoteType();
					}
                    /*Begin #174961 - Enable disable logic for the radio buttons*/

                    if (_gbWorkQueueNote.RecordType.Value == "Request" ||cmbNoteType.Text.Trim() != "Task" ||  _gbWorkQueueNote.TaskType.IsNull || cmbStatus.Text.Trim() == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Pending)
                    {
                        rbDeclined.SetObjectStatus(NullabilityState.Null, VisibilityState.Hide, EnableState.Disable);
                        rbSold.SetObjectStatus(NullabilityState.Null, VisibilityState.Hide, EnableState.Disable);
                    }
                    else if (_gbWorkQueueNote.TaskType.Value == "P" || _gbWorkQueueNote.TaskType.Value == "G" || _gbWorkQueueNote.TaskType.Value == "S")
                    {
                        rbDeclined.SetObjectStatus(NullabilityState.Default, VisibilityState.Show , EnableState.Enable);
                        rbSold.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    }

                     if ((_gbWorkQueueNote.RecordType.Value == "Referral") && (_gbWorkQueueNote.TaskType.Value == "P" || _gbWorkQueueNote.TaskType.Value == "G" || _gbWorkQueueNote.TaskType.Value == "S"))
                     {
                         // Need to change the label dynamically when it is having a valu in TaskType. Here 17 is the object Id for "Product(s)/Service:"
                         SetMlObjectId(lblDescription, 17);
 
                         dfNoteTitle.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);     
                     }

                    /*End #174961*/
					#endregion
					break;

			}

			return;
		}
		private void LocCheckPMA()
		{
			string IsPMAEnabled = this._gbWorkQueue.IsPMAEnabled();

			if (IsPMAEnabled.Trim() == "N" || this._gbWorkQueueNote.ParentOwnerEmplId.IsNull)
			{
				this.cbEmailOwner.Checked = false;
				this.cbEmailOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);
			}
			else
			{
				if (!this._gbWorkQueueNote.ParentOwnerEmplId.IsNull)
				{
					this.cbEmailOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
				}				
			}
		}

		private void SetObjectStatusForNoteType()
		{
			if (cmbNoteType.Text.Trim() == "Note")
			{
				dfDueDt.Enabled = false;
				dfDueTime.Enabled = false;
				mlComments.Enabled = false;
				this.cmbStatus.SetValueAndSelect("Completed", true);
			}
			else
			{
				dfDueDt.Enabled = true;
				dfDueTime.Enabled = true;
				mlComments.Enabled = true;
				this.cmbStatus.SetValueAndSelect("Pending", true);
			}
		 }

        private bool DoValidations()
		{
			if (this.mlNoteText.Text.Trim() != string.Empty && this.dfNoteTitle.Text.Trim() == string.Empty)
			{
				PMessageBox.Show(this, 319892, MessageType.NullTest, MessageBoxButtons.OK);
				//319892 - "Note Title" field cannot be blank.
				this.dfNoteTitle.Focus();
                return false ;
			}

			if (this.cmbNoteType.Text.Trim() == "Task")
			{
				if (this.cmbStatus.Text.Trim() == "Completed" && this.mlComments.Text.Trim() == string.Empty)
				{
					PMessageBox.Show(this, 319894, MessageType.NullTest, MessageBoxButtons.OK);
					//319894 - "Comments" field cannot be blank.
					this.mlComments.Focus();
                    return false;
				}

				if (this.mlComments.Text.Trim() != string.Empty)
				{
					if (this.mlComments.Text.Trim() != this._gbWorkQueueNote.TaskInfo.PreviousValue && this.cmbStatus.Text.Trim() == "Pending")
					{
						//319893 - A comment has been entered for the Task Resolution.  Do you wish to change the "Status" to "Completed"?
						if (PMessageBox.Show(this, 319893, MessageType.Question, MessageBoxButtons.YesNo) == DialogResult.Yes)
						{
							this.cmbStatus.SetValueAndSelect("Completed", true);
						}
					}
				}
			}

            return true ;
		}

        /*Begin #174961 - This method is used to change the label name dynamically using an ML object Id */
        private void SetMlObjectId(PLabelStandard labelControl, int nObjectId)
        {
            labelControl.ObjectId = nObjectId;

            labelControl.MLInfo = CoreService.Translation.GetUIControlInfo(CoreService.Translation.BaseLanguageId, CoreService.AppSetting.ApplicationId, this.ScreenId, labelControl.ObjectId);

        }
        /*End #174961*/
		#endregion

		private void frmWorkQueueNoteEdit_PAddNextInit(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
		{
            
            decimal workId = this._gbWorkQueueNote.WorkId.Value;
			string recordType = this._gbWorkQueueNote.RecordType.Value;
			this._gbWorkQueueNote = new GbWorkQueueNote();
			this._gbWorkQueueNote.WorkId.Value = workId;
			this._gbWorkQueueNote.RecordType.Value = recordType;
            //84686 - screen id  set when add next button
            /* Begin #108099 */
            //this._gbWorkQueueNote.OutputType.Value = Phoenix.Shared.Constants.ScreenId.ReqWorkQueueNoteEdit;
            this._gbWorkQueueNote.OutputType.Value = this.ScreenId;
            /* End #108099 */
        }

        #region #04262
        private void dfDueTime_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {

            //If NOT SalIsNull( hWndItem )		! No point validating an empty field
            if (dfDueTime.Text != null || dfDueTime.Text != string.Empty)
            {
                DateTime result = new DateTime();
                if (!DateTime.TryParse(dfDueTime.Text, out result))
                {
                    // Call IntMLMessage( 13, MB_Error, 0, gsaNull )		! #4262
                    //ITSC Issue #4262 //Call IntMLMessage( 11379, MB_Error, 0, gsaNull );/! #4262
                    PMessageBox.Show(11379, MessageType.Error, string.Empty);

                    return;
                }
            }
        } 
        #endregion



     
	}
}
