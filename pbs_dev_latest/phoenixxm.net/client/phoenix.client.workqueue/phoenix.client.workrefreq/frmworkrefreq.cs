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
//09/20/06		1		VDevadoss	#69248 - Created
//11/17/2008    2       Nelsehety   #01698 - .Net Porting
//09/09/2008    3       iezikeanyi/Mona #77295 -Added code to repopulate parent window when you exit Request/Referral windows 
//09/13/2009    4       aHussein    #77295 -1 Re-Porting issues
//10/15/2009    8       Mona        #06057 
//01/08/2010    9       mramalin    WI-7307 Disabled  pbCustSumm button as per the logic from ktonsalesworkqueue.apl
//08/06/2012    7       Mkrishna    #19058 - Adding call to base on initParameters.
//07/11/2013	8		JRhyne		WI#23345 - two ML objects existed for the text ... So I recycled one to be for the label which was missing. 28 became 37, and the lblCreatedBy became 28 which was modified to have : at the end
//05/22/2014	9		JRHyne		WI#29008 - set allow ui pref = false 
//10/24/2014    10      MBachala    176996 - porting of relationship summary window.
//02/16/2015    11      Sandeep.S   #174961 - Sales and Service - Changed the lblProduct's Label Text - Referral Mode
//07/19/2016    12      DEiland     Task#48372 - Add Next Screen ID to the pbCustSumm so it will use Customer Security
//12/17/2019    13      Jissa       ENH #243937 | US #121376 | Task #122735 - Fetch Work Queue Tabledump
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
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Core;
using Phoenix.Client.RimAccounts;
using Phoenix.BusObj.Admin.Global;

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for frmWorkRefReq.
	/// </summary>
	public class frmWorkRefReq : Phoenix.Windows.Forms.PfwStandard
	{
		#region Private Vars

		private Phoenix.BusObj.Global.GbWorkQueue _gbWorkQueue = new Phoenix.BusObj.Global.GbWorkQueue();

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PTabControl picTabs;
		private Phoenix.Windows.Forms.PTabPage dfTabTitle1;
		private Phoenix.Windows.Forms.PTabPage dfTabTitle0;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbBasicInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblID;
		private Phoenix.Windows.Forms.PDfDisplay dfRefReqID;
		private Phoenix.Windows.Forms.PLabelStandard lblCategory;
		private Phoenix.Windows.Forms.PDfDisplay dfType;
		private Phoenix.Windows.Forms.PLabelStandard lblCustomer;
		private Phoenix.Windows.Forms.PDfDisplay dfCustomer;
		private Phoenix.Windows.Forms.PDfDisplay dfProspect;
		private Phoenix.Windows.Forms.PLabelStandard lblDueDate;
		private Phoenix.Windows.Forms.PDfDisplay dfDueDt;
		private Phoenix.Windows.Forms.PLabelStandard lblAccount;
		private Phoenix.Windows.Forms.PDfDisplay dfAccount;
		private Phoenix.Windows.Forms.PLabelStandard lblCreateDate;
		private Phoenix.Windows.Forms.PDfDisplay dfCreateDt;
		private Phoenix.Windows.Forms.PLabelStandard lblDescription;
		private Phoenix.Windows.Forms.PDfDisplay dfDescription;
		private Phoenix.Windows.Forms.PLabelStandard lblContactMethod;
		private Phoenix.Windows.Forms.PDfDisplay dfContactMethod;
		private Phoenix.Windows.Forms.PLabelStandard lblContactName;
		private Phoenix.Windows.Forms.PDfDisplay dfContactName;
		private Phoenix.Windows.Forms.PLabelStandard lblContactInformation;
		private Phoenix.Windows.Forms.PDfDisplay dfContactInfo;
		private Phoenix.Windows.Forms.PLabelStandard lblWorkStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfWorkStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblOwner;
		private Phoenix.Windows.Forms.PDfDisplay dfOwner;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbHistoricalInformation;
//		private Phoenix.Windows.Forms.PdfStandard dfTabTitle0;
//		private Phoenix.Windows.Forms.PdfStandard dfTabTitle1;
		private Phoenix.Windows.Forms.PGrid gridPendingTasks;
		private Phoenix.Windows.Forms.PGridColumn colTaskCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colTaskDueDt;
		private Phoenix.Windows.Forms.PGridColumn colTaskDesc;
		private Phoenix.Windows.Forms.PGridColumn colTaskCreatedBy;
		private Phoenix.Windows.Forms.PGridColumn colTaskPTID;
		private Phoenix.Windows.Forms.PGridColumn colTaskOrderSeq;
		private Phoenix.Windows.Forms.PGrid gridHistory;
		private Phoenix.Windows.Forms.PGridColumn colNoteCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colNoteType;
		private Phoenix.Windows.Forms.PGridColumn colNoteTitle;
		private Phoenix.Windows.Forms.PGridColumn colNoteCompletedDt;
		private Phoenix.Windows.Forms.PGridColumn colNoteCreatedBy;
		private Phoenix.Windows.Forms.PGridColumn colNotePTID;
		private Phoenix.Windows.Forms.PAction pbCustSumm;
		private Phoenix.Windows.Forms.PAction pbNewTask;
		private Phoenix.Windows.Forms.PAction pbEditPendingTask;
		private Phoenix.Windows.Forms.PAction pbEditTask;
		private Phoenix.Windows.Forms.PLabelStandard lblProduct;
        private PLabelStandard lblCreatedBy;
        private PDfDisplay dfCreatedBy;
		//private Phoenix.Windows.Forms.PAction pbEdit;
		private Phoenix.Windows.Forms.PAction pbWorkNotes;
		#endregion

		#region Constructor
		public frmWorkRefReq()
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
            Phoenix.FrameWork.Core.ControlInfo controlInfo1 = new Phoenix.FrameWork.Core.ControlInfo();
            Phoenix.FrameWork.Core.ControlInfo controlInfo2 = new Phoenix.FrameWork.Core.ControlInfo();
            this.picTabs = new Phoenix.Windows.Forms.PTabControl();
            this.dfTabTitle0 = new Phoenix.Windows.Forms.PTabPage();
            this.gridPendingTasks = new Phoenix.Windows.Forms.PGrid();
            this.colTaskCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTaskDueDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTaskDesc = new Phoenix.Windows.Forms.PGridColumn();
            this.colTaskCreatedBy = new Phoenix.Windows.Forms.PGridColumn();
            this.colTaskPTID = new Phoenix.Windows.Forms.PGridColumn();
            this.colTaskOrderSeq = new Phoenix.Windows.Forms.PGridColumn();
            this.dfTabTitle1 = new Phoenix.Windows.Forms.PTabPage();
            this.gridHistory = new Phoenix.Windows.Forms.PGrid();
            this.colNoteCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoteType = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoteTitle = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoteCompletedDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoteCreatedBy = new Phoenix.Windows.Forms.PGridColumn();
            this.colNotePTID = new Phoenix.Windows.Forms.PGridColumn();
            this.gbBasicInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCreatedBy = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCreatedBy = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfOwner = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblOwner = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfWorkStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblWorkStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfContactInfo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblContactInformation = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfContactName = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblContactName = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfContactMethod = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblContactMethod = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDescription = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCreateDt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCreateDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAccount = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfDueDt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDueDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfProspect = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCustomer = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCustomer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfType = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCategory = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRefReqID = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblID = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblProduct = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbHistoricalInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pbCustSumm = new Phoenix.Windows.Forms.PAction();
            this.pbNewTask = new Phoenix.Windows.Forms.PAction();
            this.pbEditPendingTask = new Phoenix.Windows.Forms.PAction();
            this.pbEditTask = new Phoenix.Windows.Forms.PAction();
            this.pbWorkNotes = new Phoenix.Windows.Forms.PAction();
            this.picTabs.SuspendLayout();
            this.dfTabTitle0.SuspendLayout();
            this.dfTabTitle1.SuspendLayout();
            this.gbBasicInformation.SuspendLayout();
            this.gbHistoricalInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbCustSumm,
            this.pbNewTask,
            this.pbEditPendingTask,
            this.pbEditTask,
            this.pbWorkNotes});
            // 
            // picTabs
            // 
            this.picTabs.Controls.Add(this.dfTabTitle0);
            this.picTabs.Controls.Add(this.dfTabTitle1);
            this.picTabs.Location = new System.Drawing.Point(3, 16);
            this.picTabs.Name = "picTabs";
            this.picTabs.SelectedIndex = 0;
            this.picTabs.Size = new System.Drawing.Size(678, 265);
            this.picTabs.TabIndex = 0;
            this.picTabs.SelectedIndexChanged += new System.EventHandler(this.picTabs_SelectedIndexChanged);
            // 
            // dfTabTitle0
            // 
            this.dfTabTitle0.Controls.Add(this.gridPendingTasks);
            this.dfTabTitle0.Location = new System.Drawing.Point(4, 22);
            controlInfo1.ObjectId = 14;
            this.dfTabTitle0.MLInfo = controlInfo1;
            this.dfTabTitle0.Name = "dfTabTitle0";
            this.dfTabTitle0.ObjectId = 14;
            this.dfTabTitle0.Size = new System.Drawing.Size(670, 239);
            this.dfTabTitle0.TabIndex = 0;
            this.dfTabTitle0.Text = "&Basic Information";
            // 
            // gridPendingTasks
            // 
            this.gridPendingTasks.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colTaskCreateDt,
            this.colTaskDueDt,
            this.colTaskDesc,
            this.colTaskCreatedBy,
            this.colTaskPTID,
            this.colTaskOrderSeq});
            this.gridPendingTasks.IsMaxNumRowsCustomized = false;
            this.gridPendingTasks.Location = new System.Drawing.Point(4, 4);
            this.gridPendingTasks.Name = "gridPendingTasks";
            this.gridPendingTasks.Size = new System.Drawing.Size(664, 232);
            this.gridPendingTasks.TabIndex = 0;
            this.gridPendingTasks.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridPendingTasks_BeforePopulate);
            this.gridPendingTasks.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridPendingTasks_FetchRowDone);
            this.gridPendingTasks.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridPendingTasks_AfterPopulate);
            // 
            // colTaskCreateDt
            // 
            this.colTaskCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTaskCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colTaskCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTaskCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colTaskCreateDt.PhoenixUIControl.ObjectId = 25;
            this.colTaskCreateDt.PhoenixUIControl.XmlTag = "Pendcreatedt";
            this.colTaskCreateDt.Title = "Create Dt";
            this.colTaskCreateDt.Width = 114;
            // 
            // colTaskDueDt
            // 
            this.colTaskDueDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTaskDueDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colTaskDueDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTaskDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colTaskDueDt.PhoenixUIControl.ObjectId = 26;
            this.colTaskDueDt.PhoenixUIControl.XmlTag = "Pendduedt";
            this.colTaskDueDt.Title = "Task Due Dt";
            this.colTaskDueDt.Width = 115;
            // 
            // colTaskDesc
            // 
            this.colTaskDesc.PhoenixUIControl.ObjectId = 27;
            this.colTaskDesc.PhoenixUIControl.XmlTag = "Pendnotetitle";
            this.colTaskDesc.Title = "Description";
            this.colTaskDesc.Width = 270;
            // 
            // colTaskCreatedBy
            // 
            this.colTaskCreatedBy.PhoenixUIControl.ObjectId = 37;
            this.colTaskCreatedBy.PhoenixUIControl.XmlTag = "Pendname";
            this.colTaskCreatedBy.Title = "Created By";
            this.colTaskCreatedBy.Width = 140;
            // 
            // colTaskPTID
            // 
            this.colTaskPTID.PhoenixUIControl.XmlTag = "Pendptid";
            this.colTaskPTID.Title = "PTID";
            this.colTaskPTID.Visible = false;
            this.colTaskPTID.Width = 0;
            // 
            // colTaskOrderSeq
            // 
            this.colTaskOrderSeq.PhoenixUIControl.XmlTag = "Pendsort";
            this.colTaskOrderSeq.Title = "Order Seq";
            this.colTaskOrderSeq.Visible = false;
            this.colTaskOrderSeq.Width = 0;
            // 
            // dfTabTitle1
            // 
            this.dfTabTitle1.Controls.Add(this.gridHistory);
            this.dfTabTitle1.Location = new System.Drawing.Point(4, 22);
            controlInfo2.ObjectId = 15;
            this.dfTabTitle1.MLInfo = controlInfo2;
            this.dfTabTitle1.Name = "dfTabTitle1";
            this.dfTabTitle1.ObjectId = 15;
            this.dfTabTitle1.Size = new System.Drawing.Size(670, 239);
            this.dfTabTitle1.TabIndex = 1;
            this.dfTabTitle1.Text = "&View All Notes && Tasks";
            // 
            // gridHistory
            // 
            this.gridHistory.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colNoteCreateDt,
            this.colNoteType,
            this.colNoteTitle,
            this.colNoteCompletedDt,
            this.colNoteCreatedBy,
            this.colNotePTID});
            this.gridHistory.IsMaxNumRowsCustomized = false;
            this.gridHistory.Location = new System.Drawing.Point(4, 4);
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.Size = new System.Drawing.Size(664, 232);
            this.gridHistory.TabIndex = 0;
            this.gridHistory.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridHistory_BeforePopulate);
            this.gridHistory.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridHistory_AfterPopulate);
            // 
            // colNoteCreateDt
            // 
            this.colNoteCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colNoteCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colNoteCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colNoteCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colNoteCreateDt.PhoenixUIControl.ObjectId = 29;
            this.colNoteCreateDt.PhoenixUIControl.XmlTag = "Histcreatedt";
            this.colNoteCreateDt.Title = "Create Dt";
            this.colNoteCreateDt.Width = 117;
            // 
            // colNoteType
            // 
            this.colNoteType.PhoenixUIControl.ObjectId = 30;
            this.colNoteType.PhoenixUIControl.XmlTag = "Histtype";
            this.colNoteType.Title = "Note Type";
            this.colNoteType.Width = 70;
            // 
            // colNoteTitle
            // 
            this.colNoteTitle.PhoenixUIControl.ObjectId = 31;
            this.colNoteTitle.PhoenixUIControl.XmlTag = "Histnotetitle";
            this.colNoteTitle.Title = "Note Title";
            this.colNoteTitle.Width = 324;
            // 
            // colNoteCompletedDt
            // 
            this.colNoteCompletedDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colNoteCompletedDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colNoteCompletedDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colNoteCompletedDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colNoteCompletedDt.PhoenixUIControl.ObjectId = 32;
            this.colNoteCompletedDt.PhoenixUIControl.XmlTag = "Histcompletedt";
            this.colNoteCompletedDt.Title = "Completed Dt";
            this.colNoteCompletedDt.Width = 135;
            // 
            // colNoteCreatedBy
            // 
            this.colNoteCreatedBy.PhoenixUIControl.ObjectId = 37;
            this.colNoteCreatedBy.PhoenixUIControl.XmlTag = "Histname";
            this.colNoteCreatedBy.Title = "Created By";
            this.colNoteCreatedBy.Width = 144;
            // 
            // colNotePTID
            // 
            this.colNotePTID.PhoenixUIControl.XmlTag = "Histptid";
            this.colNotePTID.Title = "PTID";
            this.colNotePTID.Visible = false;
            this.colNotePTID.Width = 0;
            // 
            // gbBasicInformation
            // 
            this.gbBasicInformation.Controls.Add(this.dfCreatedBy);
            this.gbBasicInformation.Controls.Add(this.lblCreatedBy);
            this.gbBasicInformation.Controls.Add(this.dfOwner);
            this.gbBasicInformation.Controls.Add(this.lblOwner);
            this.gbBasicInformation.Controls.Add(this.dfWorkStatus);
            this.gbBasicInformation.Controls.Add(this.lblWorkStatus);
            this.gbBasicInformation.Controls.Add(this.dfContactInfo);
            this.gbBasicInformation.Controls.Add(this.lblContactInformation);
            this.gbBasicInformation.Controls.Add(this.dfContactName);
            this.gbBasicInformation.Controls.Add(this.lblContactName);
            this.gbBasicInformation.Controls.Add(this.dfContactMethod);
            this.gbBasicInformation.Controls.Add(this.lblContactMethod);
            this.gbBasicInformation.Controls.Add(this.dfDescription);
            this.gbBasicInformation.Controls.Add(this.lblDescription);
            this.gbBasicInformation.Controls.Add(this.dfCreateDt);
            this.gbBasicInformation.Controls.Add(this.lblCreateDate);
            this.gbBasicInformation.Controls.Add(this.dfAccount);
            this.gbBasicInformation.Controls.Add(this.dfDueDt);
            this.gbBasicInformation.Controls.Add(this.lblDueDate);
            this.gbBasicInformation.Controls.Add(this.dfProspect);
            this.gbBasicInformation.Controls.Add(this.dfCustomer);
            this.gbBasicInformation.Controls.Add(this.lblCustomer);
            this.gbBasicInformation.Controls.Add(this.dfType);
            this.gbBasicInformation.Controls.Add(this.lblCategory);
            this.gbBasicInformation.Controls.Add(this.dfRefReqID);
            this.gbBasicInformation.Controls.Add(this.lblID);
            this.gbBasicInformation.Controls.Add(this.lblProduct);
            this.gbBasicInformation.Controls.Add(this.lblAccount);
            this.gbBasicInformation.Location = new System.Drawing.Point(4, 0);
            this.gbBasicInformation.Name = "gbBasicInformation";
            this.gbBasicInformation.PhoenixUIControl.ObjectId = 1;
            this.gbBasicInformation.Size = new System.Drawing.Size(684, 160);
            this.gbBasicInformation.TabIndex = 0;
            this.gbBasicInformation.TabStop = false;
            this.gbBasicInformation.Text = "Basic Information";
            // 
            // dfCreatedBy
            // 
            this.dfCreatedBy.Location = new System.Drawing.Point(460, 76);
            this.dfCreatedBy.Multiline = true;
            this.dfCreatedBy.Name = "dfCreatedBy";
            this.dfCreatedBy.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCreatedBy.PhoenixUIControl.ObjectId = 38;
            this.dfCreatedBy.PhoenixUIControl.XmlTag = "";
            this.dfCreatedBy.PreviousValue = null;
            this.dfCreatedBy.Size = new System.Drawing.Size(216, 16);
            this.dfCreatedBy.TabIndex = 27;
            this.dfCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.AutoEllipsis = true;
            this.lblCreatedBy.Location = new System.Drawing.Point(372, 76);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.PhoenixUIControl.ObjectId = 28;
            this.lblCreatedBy.Size = new System.Drawing.Size(69, 16);
            this.lblCreatedBy.TabIndex = 26;
            this.lblCreatedBy.Text = "Created By:";
            // 
            // dfOwner
            // 
            this.dfOwner.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOwner.Location = new System.Drawing.Point(460, 136);
            this.dfOwner.Multiline = true;
            this.dfOwner.Name = "dfOwner";
            this.dfOwner.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOwner.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfOwner.PhoenixUIControl.ObjectId = 12;
            this.dfOwner.PhoenixUIControl.XmlTag = "OwnerName";
            this.dfOwner.PreviousValue = null;
            this.dfOwner.Size = new System.Drawing.Size(216, 16);
            this.dfOwner.TabIndex = 25;
            this.dfOwner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOwner
            // 
            this.lblOwner.AutoEllipsis = true;
            this.lblOwner.Location = new System.Drawing.Point(372, 136);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.PhoenixUIControl.ObjectId = 12;
            this.lblOwner.Size = new System.Drawing.Size(81, 16);
            this.lblOwner.TabIndex = 24;
            this.lblOwner.Text = "Owner:";
            // 
            // dfWorkStatus
            // 
            this.dfWorkStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfWorkStatus.Location = new System.Drawing.Point(120, 136);
            this.dfWorkStatus.Multiline = true;
            this.dfWorkStatus.Name = "dfWorkStatus";
            this.dfWorkStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfWorkStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfWorkStatus.PhoenixUIControl.ObjectId = 33;
            this.dfWorkStatus.PhoenixUIControl.XmlTag = "WorkStatus";
            this.dfWorkStatus.PreviousValue = null;
            this.dfWorkStatus.Size = new System.Drawing.Size(74, 16);
            this.dfWorkStatus.TabIndex = 23;
            this.dfWorkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWorkStatus
            // 
            this.lblWorkStatus.AutoEllipsis = true;
            this.lblWorkStatus.Location = new System.Drawing.Point(8, 136);
            this.lblWorkStatus.Name = "lblWorkStatus";
            this.lblWorkStatus.PhoenixUIControl.ObjectId = 33;
            this.lblWorkStatus.Size = new System.Drawing.Size(106, 16);
            this.lblWorkStatus.TabIndex = 22;
            this.lblWorkStatus.Text = "Work Status:";
            // 
            // dfContactInfo
            // 
            this.dfContactInfo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactInfo.Location = new System.Drawing.Point(120, 116);
            this.dfContactInfo.Multiline = true;
            this.dfContactInfo.Name = "dfContactInfo";
            this.dfContactInfo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactInfo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfContactInfo.PhoenixUIControl.ObjectId = 11;
            this.dfContactInfo.PhoenixUIControl.XmlTag = "OtherInfo";
            this.dfContactInfo.PreviousValue = null;
            this.dfContactInfo.Size = new System.Drawing.Size(556, 16);
            this.dfContactInfo.TabIndex = 21;
            this.dfContactInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactInformation
            // 
            this.lblContactInformation.AutoEllipsis = true;
            this.lblContactInformation.Location = new System.Drawing.Point(8, 116);
            this.lblContactInformation.Name = "lblContactInformation";
            this.lblContactInformation.PhoenixUIControl.ObjectId = 11;
            this.lblContactInformation.Size = new System.Drawing.Size(108, 16);
            this.lblContactInformation.TabIndex = 20;
            this.lblContactInformation.Text = "Contact Information:";
            // 
            // dfContactName
            // 
            this.dfContactName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.Location = new System.Drawing.Point(460, 96);
            this.dfContactName.Multiline = true;
            this.dfContactName.Name = "dfContactName";
            this.dfContactName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfContactName.PhoenixUIControl.ObjectId = 10;
            this.dfContactName.PhoenixUIControl.XmlTag = "ContactName";
            this.dfContactName.PreviousValue = null;
            this.dfContactName.Size = new System.Drawing.Size(216, 16);
            this.dfContactName.TabIndex = 19;
            this.dfContactName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactName
            // 
            this.lblContactName.AutoEllipsis = true;
            this.lblContactName.Location = new System.Drawing.Point(372, 96);
            this.lblContactName.Name = "lblContactName";
            this.lblContactName.PhoenixUIControl.ObjectId = 10;
            this.lblContactName.Size = new System.Drawing.Size(81, 16);
            this.lblContactName.TabIndex = 18;
            this.lblContactName.Text = "Contact Name:";
            // 
            // dfContactMethod
            // 
            this.dfContactMethod.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactMethod.Location = new System.Drawing.Point(120, 96);
            this.dfContactMethod.Multiline = true;
            this.dfContactMethod.Name = "dfContactMethod";
            this.dfContactMethod.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactMethod.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfContactMethod.PhoenixUIControl.ObjectId = 9;
            this.dfContactMethod.PhoenixUIControl.XmlTag = "ContactMethod";
            this.dfContactMethod.PreviousValue = null;
            this.dfContactMethod.Size = new System.Drawing.Size(248, 16);
            this.dfContactMethod.TabIndex = 17;
            this.dfContactMethod.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblContactMethod
            // 
            this.lblContactMethod.AutoEllipsis = true;
            this.lblContactMethod.Location = new System.Drawing.Point(8, 96);
            this.lblContactMethod.Name = "lblContactMethod";
            this.lblContactMethod.PhoenixUIControl.ObjectId = 9;
            this.lblContactMethod.Size = new System.Drawing.Size(106, 16);
            this.lblContactMethod.TabIndex = 16;
            this.lblContactMethod.Text = "Contact Method:";
            // 
            // dfDescription
            // 
            this.dfDescription.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.Location = new System.Drawing.Point(120, 76);
            this.dfDescription.Multiline = true;
            this.dfDescription.Name = "dfDescription";
            this.dfDescription.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDescription.PhoenixUIControl.ObjectId = 8;
            this.dfDescription.PhoenixUIControl.XmlTag = "Description";
            this.dfDescription.PreviousValue = null;
            this.dfDescription.Size = new System.Drawing.Size(248, 16);
            this.dfDescription.TabIndex = 15;
            this.dfDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 76);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 8;
            this.lblDescription.Size = new System.Drawing.Size(106, 16);
            this.lblDescription.TabIndex = 14;
            this.lblDescription.Text = "Description:";
            // 
            // dfCreateDt
            // 
            this.dfCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDt.Location = new System.Drawing.Point(460, 56);
            this.dfCreateDt.Multiline = true;
            this.dfCreateDt.Name = "dfCreateDt";
            this.dfCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfCreateDt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.dfCreateDt.PhoenixUIControl.ObjectId = 7;
            this.dfCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.dfCreateDt.PreviousValue = null;
            this.dfCreateDt.Size = new System.Drawing.Size(216, 16);
            this.dfCreateDt.TabIndex = 13;
            this.dfCreateDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCreateDate
            // 
            this.lblCreateDate.AutoEllipsis = true;
            this.lblCreateDate.Location = new System.Drawing.Point(372, 56);
            this.lblCreateDate.Name = "lblCreateDate";
            this.lblCreateDate.PhoenixUIControl.ObjectId = 7;
            this.lblCreateDate.Size = new System.Drawing.Size(69, 16);
            this.lblCreateDate.TabIndex = 12;
            this.lblCreateDate.Text = "Create Date:";
            // 
            // dfAccount
            // 
            this.dfAccount.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccount.Location = new System.Drawing.Point(120, 56);
            this.dfAccount.Multiline = true;
            this.dfAccount.Name = "dfAccount";
            this.dfAccount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfAccount.PhoenixUIControl.ObjectId = 6;
            this.dfAccount.PreviousValue = null;
            this.dfAccount.Size = new System.Drawing.Size(248, 16);
            this.dfAccount.TabIndex = 11;
            this.dfAccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dfDueDt
            // 
            this.dfDueDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDt.Location = new System.Drawing.Point(460, 36);
            this.dfDueDt.Multiline = true;
            this.dfDueDt.Name = "dfDueDt";
            this.dfDueDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.dfDueDt.PhoenixUIControl.ObjectId = 5;
            this.dfDueDt.PhoenixUIControl.XmlTag = "DueDt";
            this.dfDueDt.PreviousValue = null;
            this.dfDueDt.Size = new System.Drawing.Size(216, 16);
            this.dfDueDt.TabIndex = 8;
            this.dfDueDt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoEllipsis = true;
            this.lblDueDate.Location = new System.Drawing.Point(372, 36);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.PhoenixUIControl.ObjectId = 5;
            this.lblDueDate.Size = new System.Drawing.Size(69, 16);
            this.lblDueDate.TabIndex = 7;
            this.lblDueDate.Text = "Due Date:";
            // 
            // dfProspect
            // 
            this.dfProspect.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfProspect.Location = new System.Drawing.Point(208, 16);
            this.dfProspect.Multiline = true;
            this.dfProspect.Name = "dfProspect";
            this.dfProspect.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfProspect.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfProspect.PhoenixUIControl.ObjectId = 36;
            this.dfProspect.PreviousValue = null;
            this.dfProspect.Size = new System.Drawing.Size(158, 16);
            this.dfProspect.TabIndex = 2;
            this.dfProspect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dfCustomer
            // 
            this.dfCustomer.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.Location = new System.Drawing.Point(120, 36);
            this.dfCustomer.Multiline = true;
            this.dfCustomer.Name = "dfCustomer";
            this.dfCustomer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCustomer.PhoenixUIControl.ObjectId = 4;
            this.dfCustomer.PhoenixUIControl.XmlTag = "CustomerName";
            this.dfCustomer.PreviousValue = null;
            this.dfCustomer.Size = new System.Drawing.Size(248, 16);
            this.dfCustomer.TabIndex = 6;
            this.dfCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoEllipsis = true;
            this.lblCustomer.Location = new System.Drawing.Point(8, 36);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.PhoenixUIControl.ObjectId = 4;
            this.lblCustomer.Size = new System.Drawing.Size(106, 16);
            this.lblCustomer.TabIndex = 5;
            this.lblCustomer.Text = "Customer:";
            // 
            // dfType
            // 
            this.dfType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfType.Location = new System.Drawing.Point(460, 16);
            this.dfType.Multiline = true;
            this.dfType.Name = "dfType";
            this.dfType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfType.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfType.PhoenixUIControl.ObjectId = 3;
            this.dfType.PhoenixUIControl.XmlTag = "CategoryDesc";
            this.dfType.PreviousValue = null;
            this.dfType.Size = new System.Drawing.Size(216, 16);
            this.dfType.TabIndex = 4;
            this.dfType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCategory
            // 
            this.lblCategory.AutoEllipsis = true;
            this.lblCategory.Location = new System.Drawing.Point(372, 16);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.PhoenixUIControl.ObjectId = 3;
            this.lblCategory.Size = new System.Drawing.Size(75, 16);
            this.lblCategory.TabIndex = 3;
            this.lblCategory.Text = "Category:";
            // 
            // dfRefReqID
            // 
            this.dfRefReqID.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRefReqID.Location = new System.Drawing.Point(120, 16);
            this.dfRefReqID.Multiline = true;
            this.dfRefReqID.Name = "dfRefReqID";
            this.dfRefReqID.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRefReqID.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRefReqID.PhoenixUIControl.ObjectId = 2;
            this.dfRefReqID.PhoenixUIControl.XmlTag = "WorkId";
            this.dfRefReqID.PreviousValue = null;
            this.dfRefReqID.Size = new System.Drawing.Size(74, 16);
            this.dfRefReqID.TabIndex = 1;
            this.dfRefReqID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblID
            // 
            this.lblID.AutoEllipsis = true;
            this.lblID.Location = new System.Drawing.Point(8, 16);
            this.lblID.Name = "lblID";
            this.lblID.PhoenixUIControl.ObjectId = 2;
            this.lblID.Size = new System.Drawing.Size(106, 16);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "ID:";
            // 
            // lblProduct
            // 
            this.lblProduct.AutoEllipsis = true;
            this.lblProduct.Location = new System.Drawing.Point(8, 56);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.PhoenixUIControl.ObjectId = 6;
            this.lblProduct.Size = new System.Drawing.Size(106, 16);
            this.lblProduct.TabIndex = 10;
            this.lblProduct.Text = "Product(s)/Service:";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoEllipsis = true;
            this.lblAccount.Location = new System.Drawing.Point(8, 56);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.PhoenixUIControl.ObjectId = 6;
            this.lblAccount.Size = new System.Drawing.Size(106, 16);
            this.lblAccount.TabIndex = 9;
            this.lblAccount.Text = "Account:";
            // 
            // gbHistoricalInformation
            // 
            this.gbHistoricalInformation.Controls.Add(this.picTabs);
            this.gbHistoricalInformation.Location = new System.Drawing.Point(4, 160);
            this.gbHistoricalInformation.Name = "gbHistoricalInformation";
            this.gbHistoricalInformation.PhoenixUIControl.ObjectId = 13;
            this.gbHistoricalInformation.Size = new System.Drawing.Size(684, 284);
            this.gbHistoricalInformation.TabIndex = 1;
            this.gbHistoricalInformation.TabStop = false;
            this.gbHistoricalInformation.Text = "Historical Information";
            // 
            // pbCustSumm
            // 
            this.pbCustSumm.LongText = "pbCustSumm";
            this.pbCustSumm.Name = "pbCustSumm";
            this.pbCustSumm.NextScreenId = 0;
            this.pbCustSumm.ObjectId = 21;
            this.pbCustSumm.ShortText = "pbCustSumm";
            this.pbCustSumm.Tag = null;
            this.pbCustSumm.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCustSumm_Click);
            // 
            // pbNewTask
            // 
            this.pbNewTask.LongText = "pbNewTask";
            this.pbNewTask.Name = "pbNewTask";
            this.pbNewTask.ObjectId = 34;
            this.pbNewTask.ShortText = "pbNewTask";
            this.pbNewTask.Tag = null;
            this.pbNewTask.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbNewTask_Click);
            // 
            // pbEditPendingTask
            // 
            this.pbEditPendingTask.LongText = "pbEditPendingTask";
            this.pbEditPendingTask.Name = "pbEditPendingTask";
            this.pbEditPendingTask.ObjectId = 35;
            this.pbEditPendingTask.ShortText = "pbEditPendingTask";
            this.pbEditPendingTask.Tag = null;
            this.pbEditPendingTask.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEditPendingTask_Click);
            // 
            // pbEditTask
            // 
            this.pbEditTask.LongText = "pbEditTask";
            this.pbEditTask.Name = "pbEditTask";
            this.pbEditTask.ObjectId = 23;
            this.pbEditTask.ShortText = "pbEditTask";
            this.pbEditTask.Tag = null;
            this.pbEditTask.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEditTask_Click);
            // 
            // pbWorkNotes
            // 
            this.pbWorkNotes.LongText = "pbWorkNotes";
            this.pbWorkNotes.Name = "pbWorkNotes";
            this.pbWorkNotes.ObjectId = 24;
            this.pbWorkNotes.ShortText = "pbWorkNotes";
            this.pbWorkNotes.Tag = null;
            this.pbWorkNotes.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbWorkNotes_Click);
            // 
            // frmWorkRefReq
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbBasicInformation);
            this.Controls.Add(this.gbHistoricalInformation);
            this.IsAllowedToBeMaximizedAlways = false;
            this.Name = "frmWorkRefReq";
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.List;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmWorkRefReq_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmWorkRefReq_PInitCompleteEvent);
            this.picTabs.ResumeLayout(false);
            this.dfTabTitle0.ResumeLayout(false);
            this.dfTabTitle1.ResumeLayout(false);
            this.gbBasicInformation.ResumeLayout(false);
            this.gbBasicInformation.PerformLayout();
            this.gbHistoricalInformation.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        void pbCustSumm_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("dfwGloAccountListRim");
        }
		#endregion


        PBoolean _refreshed = new PBoolean("Refreshed");   //#77295 

		#region Init Parameters
		public override void InitParameters(params Object[] paramList)
		{
			if (paramList.Length == 2)
			{
				this._gbWorkQueue.WorkId.Value = Convert.ToInt32(paramList[0]);

                //this._gbWorkQueue.WorkId.Value = Convert.ToInt32((paramList[0] as PBaseType).ValueObject );

				this._gbWorkQueue.Type.Value = paramList[1].ToString().Trim();

				if (this._gbWorkQueue.Type.Value.Trim() == "Referral")
				{
					this.ScreenId = 12528;
				}
				else
				{
					this.ScreenId = 12529;
				}

				this._gbWorkQueue.OutputType.Value = this.ScreenId;

			}

            base.InitParameters(paramList); //#19058
		}
		#endregion

		#region Window Events
		#region Begin / Complete Window Events
		private Phoenix.Windows.Forms.ReturnType frmWorkRefReq_PInitBeginEvent()
		{
           this.AppToolBarId = AppToolBarType.Display;

			this.MainBusinesObject = _gbWorkQueue;
            this.MainBusinesObject.ResponseTypeId = 18; // #122735
            this.ActionEdit.ObjectId = 20;	//change the text to "Edit Details"

			if (this._gbWorkQueue.Type.Value.Trim() == "Referral")
				lblProduct.ObjectId = 38;	// Product
			else
				lblProduct.ObjectId = 6;	// Account

			this.PerformAction(XmActionType.Select);
			this.AutoFetch = false; //Don't let the framework do the select again.			

            // Task#48372 
            pbCustSumm.NextScreenId = Phoenix.Shared.Constants.ScreenId.GbAccountListRim;
            // End Task#48372 

			return ReturnType.Success;
		}

		private void frmWorkRefReq_PInitCompleteEvent()
		{			
			#region Get the customer Name and set the window title
			//#06057//if (!this._gbWorkQueue.RimNo.IsNull)
            if (!this._gbWorkQueue.RimNo.IsNull && _gbWorkQueue.RimNo.Value != 0)
				this.dfCustomer.UnFormattedValue = this._gbWorkQueue.GetCustomerName(this._gbWorkQueue.RimNo.Value);

			if (!this._gbWorkQueue.CustomerName.IsNull)
			{
				if (this._gbWorkQueue.Type.Value.Trim() == "Referral")
					this.NewRecordTitle = this.NewRecordTitle + " (" + this._gbWorkQueue.CustomerName.Value.Trim() + ")";
				else
					this.NewRecordTitle = this.NewRecordTitle + " (" + (this._gbWorkQueue.CustomerName.IsNull ? string.Empty : this._gbWorkQueue.CustomerName.Value.Trim()) + "  " + (this._gbWorkQueue.AcctType.IsNull ? string.Empty : this._gbWorkQueue.AcctType.Value.Trim()) + " - " + (this._gbWorkQueue.AcctNo.IsNull ? string.Empty : this._gbWorkQueue.AcctNo.Value.Trim()) + ")";
			}

			this.EditRecordTitle = this.NewRecordTitle;
			#endregion

			//Get the other data

            this.dfOwner.UnFormattedValue = null; //#077295 - 1 

			if (!this._gbWorkQueue.OwnerEmplId.IsNull)
				this.dfOwner.UnFormattedValue = this._gbWorkQueue.GetOwnerName(this._gbWorkQueue.OwnerEmplId.Value);

			if (!this._gbWorkQueue.CategoryId.IsNull)
			{
				this.dfType.UnFormattedValue = this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatDesc);
			}

			if (this._gbWorkQueue.Type.Value.Trim() == "Referral")
			{
				if (this._gbWorkQueue.NewProspect.Value.Trim() == "Y")
				{
					this.dfProspect.ForeColor = System.Drawing.Color.DarkRed;
					this.dfProspect.Text = "NEW PROSPECT";
				}
				else
				{
					this.dfProspect.Text = string.Empty;
				}
			}

			#region Set the Product Desc
			this._gbWorkQueue.GetProductDesc();

			if (!this._gbWorkQueue.ProductDesc.IsNull)
				this.dfAccount.Text = this._gbWorkQueue.ProductDesc.Value.Trim();		
			#endregion

			#region Change the Due Dt color is past due
			if (!this._gbWorkQueue.DueDt.IsNull)
				if (this._gbWorkQueue.DueDt.Value < Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate)
					this.dfDueDt.ForeColor = System.Drawing.Color.DarkRed;
			#endregion

			EnableDisableVisibleLogic("Initialize");

			#region Set the Grid Properties and Pop the tables
			this.gridPendingTasks.DoubleClickAction = pbEditPendingTask;
			this.gridHistory.DoubleClickAction = pbEditTask;

			//Pop the tables.
			//LocPopTables(gridPendingTasks);
			//LocPopTables(gridHistory);
			#endregion

            GbHelper gbhelper = new GbHelper();

            dfCreatedBy.UnFormattedValue = gbhelper.GetEmployeeName(Convert.ToInt32(this._gbWorkQueue.EmplId.Value));

			SetTabAssociates(picTabs.SelectedIndex);


            #region #77295
            if (this.IsFormInitialized)
            {
                // this means that form has been repopulated based on change of child screens. 
                _refreshed.Value = true; // 
            }
            #endregion
		}
		#endregion

		#region Grid Events
		private void gridPendingTasks_BeforePopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
		{
			this._gbWorkQueue.GridNameToPop.Value = "PendingTask";
			this.gridPendingTasks.ListViewObject = this._gbWorkQueue;
		}
		private void gridPendingTasks_AfterPopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
		{
			if (gridPendingTasks.Items.Count == 0)
				this.pbEditPendingTask.Enabled = false;
			else
				this.pbEditPendingTask.Enabled = true;
		}

		private void gridPendingTasks_FetchRowDone(object sender, Phoenix.Windows.Forms.GridRowArgs e)
		{
			if (this.colTaskDueDt.Text != "" && Convert.ToDateTime(this.colTaskDueDt.Text) < Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate )
				e.CurrentRow.ForeColor = System.Drawing.Color.Red;
		}

		private void gridHistory_BeforePopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
		{
			this._gbWorkQueue.GridNameToPop.Value = "History";
			this.gridHistory.ListViewObject = this._gbWorkQueue;
		}
		private void gridHistory_AfterPopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
		{
			if (gridHistory.Items.Count == 0)
				this.pbEditTask.Enabled = false;
			else
				this.pbEditTask.Enabled = true;
		}

		#endregion

		#region Tab Events
		private void picTabs_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SetTabAssociates(picTabs.SelectedIndex);
		}

		#endregion

		#region Call Parent
		public override void CallParent(params object[] paramList)
		{
			/*if (paramList.Length > 0)
			{
				if (Convert.ToInt32(paramList[0]) == 12503)	//12503 - referral edit window
				{
					this.InitializeForm();
				}
			}*/
        
			this.InitializeForm();
			return;
		}

		#endregion

		#region Push Button Events
		/*private void pbCustSumm_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			Phoenix.Client.RimAccounts.frmGloAccountListRim temp = new Phoenix.Client.RimAccounts.frmGloAccountListRim();
			temp.InitParameters((this._gbWorkQueue.RimNo.IsNull? 0 : this._gbWorkQueue.RimNo.Value),
					(this._gbWorkQueue.AcctType.IsNull? string.Empty : this._gbWorkQueue.AcctType.Value),
					(this._gbWorkQueue.AcctNo.IsNull? string.Empty : this._gbWorkQueue.AcctNo.Value));
			temp.Workspace = this.Workspace;
			temp.Show();
		}*/
		public override void OnActionEdit()
		{
			if (!this._gbWorkQueue.Type.IsNull)
				CallOtherForms(this._gbWorkQueue.Type.Value.Trim());
			base.OnActionEdit ();
		}

		private void pbNewTask_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			PfwStandard tempWin = null;
			tempWin = new Phoenix.Client.WorkQueue.frmWorkQueueNoteEdit();
            #region #01698 Bug Fixing

            if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
            {
                tempWin.InitParameters(
                                        null,	//ptid of the note record
                                        this._gbWorkQueue.WorkId.Value,
                                        this._gbWorkQueue.Type.Value,
                                        this._gbWorkQueue.RimNo.Value,
                                        this._gbWorkQueue.CustomerName.Value,
                                        string.Empty,
                                        string.Empty);
            }
            else
            {
			tempWin.InitParameters( 
									null,	//ptid of the note record
									this._gbWorkQueue.WorkId.Value, 
									this._gbWorkQueue.Type.Value, 
									this._gbWorkQueue.RimNo.Value,
									this._gbWorkQueue.CustomerName.Value,
									this._gbWorkQueue.AcctType.Value,
									this._gbWorkQueue.AcctNo.Value);

            }
            #endregion

			tempWin.Workspace =  this.Workspace;
			tempWin.Show();
		}
		private void pbEditTask_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			PfwStandard tempWin = null;
			tempWin = new Phoenix.Client.WorkQueue.frmWorkQueueNoteEdit();
			tempWin.InitParameters( 
				this.colNotePTID.Text.Trim(),
				this._gbWorkQueue.WorkId.Value, 
				this._gbWorkQueue.Type.Value, 
				this._gbWorkQueue.RimNo.Value,
				this._gbWorkQueue.CustomerName.Value,
				(this._gbWorkQueue.Type.Value == "Referral"?null:this._gbWorkQueue.AcctType.Value),
				(this._gbWorkQueue.Type.Value == "Referral"?null:this._gbWorkQueue.AcctNo.Value));

			tempWin.Workspace =  this.Workspace;
			tempWin.Show();		
		}

		private void pbEditPendingTask_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			PfwStandard tempWin = null;
			tempWin = new Phoenix.Client.WorkQueue.frmWorkQueueNoteEdit();
			tempWin.InitParameters( 
				this.colTaskPTID.Text.Trim(),
				this._gbWorkQueue.WorkId.Value, 
				this._gbWorkQueue.Type.Value, 
				this._gbWorkQueue.RimNo.Value,
				this._gbWorkQueue.CustomerName.Value,
				(this._gbWorkQueue.Type.Value == "Referral"?null:this._gbWorkQueue.AcctType.Value),
				(this._gbWorkQueue.Type.Value == "Referral"?null:this._gbWorkQueue.AcctNo.Value));

			tempWin.Workspace =  this.Workspace;
			tempWin.Show();			
		}

		private void pbWorkNotes_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
		{
			Phoenix.Client.WorkQueue.frmWorkQueueNotesDisplay tempwin = new Phoenix.Client.WorkQueue.frmWorkQueueNotesDisplay();
			tempwin.InitParameters(this._gbWorkQueue.WorkId.Value, this._gbWorkQueue.Type.Value);
			tempwin.Workspace = this.Workspace;
			tempwin.Show();
		}

        #region #77295
        public override bool OnActionClose()
        {

            // Begin #77295
			if (_refreshed.Value ) // we will depend on parent Screen Call Parnet to Detect this call 
            {
                base.CallParent(this.ScreenId, "Populate");
            }
            // End #77295

            return true;
            //return base.OnActionClose();
        } 
        #endregion
		#endregion

		#region Private Functions/Procedures
		private void EnableDisableVisibleLogic(string caseType)
		{
			switch (caseType)
			{
				case "Initialize":
					#region Initialize Logic
					this.pbCustSumm.Visible = false; //Aaaron did not want to provide the cust summ hookup at present.
					this.ActionNew.Visible = false;	//No need in this window.

					/*if (this._gbWorkQueue.Type.Value.Trim() == "Referral")
					{
						lblProduct.Visible = true;
						lblAccount.Visible = false;
					}
					else
					{
						lblProduct.Visible = false;
						lblAccount.Visible = true;
					}*/

                    if (Phoenix.Shared.Variables.GlobalVars.Module != "Teller")
                    {
                        this.pbCustSumm.Visible = true;
                        if( this._gbWorkQueue.RimNo.IsNull )
                        {
                            this.pbCustSumm.Enabled = false;
                        }
                        else if( this.Workspace != null )
                        {
                            PfwStandard prevWindow = this.Workspace.CurrentWindow as PfwStandard;
                            
                            if (this.Workspace.CurrentWindow == this )
                            {
                                prevWindow = this.Workspace.PreviousWindow as PfwStandard;
                            }
                            if(  prevWindow != null && (prevWindow.Name == "frmReferral" || prevWindow.Name == "frmRequest" ))
                                this.pbCustSumm.Enabled = false;
                        }
                        
                    }

					#endregion
					break;
			}

			return;
		}

		private void LocPopTables(PGrid phgridToPop)
		{
//			try
//			{
//				using ( new WaitCursor())
//				{
//					if (phgridToPop.Name == "gridPendingTasks")
//					{
//						this._gbWorkQueue.GridNameToPop.Value = "PendingTask";
//						this.gridPendingTasks.ListViewObject = this._gbWorkQueue;
//						//Populate the Table window.
//						gridPendingTasks.PopulateTable();
//					}
//					else
//					{
//						this._gbWorkQueue.GridNameToPop.Value = "History";
//						this.gridHistory.ListViewObject = this._gbWorkQueue;
//						//Populate the Table window.
//						gridHistory.PopulateTable();
//					}
//				}				
//			}
//			catch (PhoenixException ex)
//			{
//				PMessageBox.Show(ex);
//				return;
//			}
		}

		private void SetTabAssociates(int pnTabIndex)
		{
			if (pnTabIndex == 0)
			{
				this.pbEditPendingTask.Visible = true;
				this.pbEditTask.Visible = false;				
			}
			else
			{
				this.pbEditTask.Visible = true;
				this.pbEditPendingTask.Visible = false;
			}
			this.Workspace.UpdateView();
		}
		private void CallOtherForms(string psOrigin)
		{
			PfwStandard tempWin = null;

            if (psOrigin == "dfwGloAccountListRim")
            {
                //176996 - porting of relationship summary window
              //  Phoenix.Windows.Client.Helper.CreateCTDWin(this, "dfwGloAccountListRim", _gbWorkQueue.RimNo.IntValue, 0);
                tempWin = CreateWindow("Phoenix.Client.rimaccounts", "Phoenix.Client.RimAccounts", "frmGloAccountListRimCustMgmt");
                tempWin.InitParameters(_gbWorkQueue.RimNo.IntValue, null, null);
            }
			else if (psOrigin == "Referral")
			{
				tempWin = new Phoenix.Client.WorkQueue.frmReferralEdit();
				tempWin.InitParameters( 
					this, 
					this._gbWorkQueue.Type.Value, 
					this._gbWorkQueue.WorkId.Value, 
					this._gbWorkQueue.RimNo.Value,
					null,
					this._gbWorkQueue.CustomerName.Value,
					this._gbWorkQueue.AcctType.Value,
					this._gbWorkQueue.ClassCode.Value);
			}
			else
			{
				tempWin = new Phoenix.Client.WorkQueue.frmRequestEdit();
				tempWin.InitParameters( 
					this, 
					this._gbWorkQueue.Type.Value, 
					this._gbWorkQueue.WorkId.Value, 
					this._gbWorkQueue.RimNo.Value,
					this._gbWorkQueue.CustomerName.Value,
					this._gbWorkQueue.AcctType.Value,
					this._gbWorkQueue.AcctNo.Value);
			}
            if (tempWin != null)
            {
			tempWin.Workspace =  this.Workspace;
			tempWin.Show();
            }
		}
		#endregion
		#endregion
	}
}
