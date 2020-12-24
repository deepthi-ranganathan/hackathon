#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2006-2008 Harland Financial Solutions Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmRequest.cs
// NameSpace: Phoenix.Client.Global
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//
//11/17/2008    2       Nelsehety     #01698 - .Net Porting
//01/25/2009    3       Nelsehety     #01698 -Bug Fixing
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
//
using Phoenix.Windows.Forms;
using Phoenix.Shared;

using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Shared.Constants;
//
using Phoenix.BusObj.Global;

namespace Phoenix.Client.Global
{
	/// <summary>
	/// Summary description for frmRequest.
	/// </summary>
	public class frmRequest : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbRequestCriteria;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbIncludeCompleted;
		private Phoenix.Windows.Forms.PGroupBoxStandard pnl3;
		private Phoenix.Windows.Forms.PGrid grdRequest;
		private Phoenix.Windows.Forms.PGridColumn colWorkId;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colDueDt;
		private Phoenix.Windows.Forms.PGridColumn colDueDtTime;
		private Phoenix.Windows.Forms.PGridColumn colOwner;
		private Phoenix.Windows.Forms.PGridColumn colWorkStatus;
		private Phoenix.Windows.Forms.PGridColumn colStatus;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colClassCode;
		private Phoenix.Windows.Forms.PGridColumn colOwnerEmplId;
		private Phoenix.Windows.Forms.PGridColumn colRIMNo;
        private PAction pbNew;
		// DEL_CON:private Phoenix.Windows.Forms.PAction pbClose;
		// DEL_CON:private Phoenix.Windows.Forms.PAction pbNew;
		private Phoenix.Windows.Forms.PAction pbWork;

		public frmRequest()
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
            this.gbRequestCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbIncludeCompleted = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.pnl3 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdRequest = new Phoenix.Windows.Forms.PGrid();
            this.colWorkId = new Phoenix.Windows.Forms.PGridColumn();
            this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colDueDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDueDtTime = new Phoenix.Windows.Forms.PGridColumn();
            this.colOwner = new Phoenix.Windows.Forms.PGridColumn();
            this.colWorkStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colClassCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colOwnerEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colRIMNo = new Phoenix.Windows.Forms.PGridColumn();
            this.pbWork = new Phoenix.Windows.Forms.PAction();
            this.pbNew = new Phoenix.Windows.Forms.PAction();
            this.gbRequestCriteria.SuspendLayout();
            this.pnl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbNew,
            this.pbWork});
            // 
            // gbRequestCriteria
            // 
            this.gbRequestCriteria.Controls.Add(this.cbIncludeCompleted);
            this.gbRequestCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbRequestCriteria.Name = "gbRequestCriteria";
            this.gbRequestCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbRequestCriteria.Size = new System.Drawing.Size(681, 40);
            this.gbRequestCriteria.TabIndex = 0;
            this.gbRequestCriteria.TabStop = false;
            this.gbRequestCriteria.Text = "Request Criteria";
            // 
            // cbIncludeCompleted
            // 
            this.cbIncludeCompleted.CodeValue = null;
            this.cbIncludeCompleted.Location = new System.Drawing.Point(4, 16);
            this.cbIncludeCompleted.Name = "cbIncludeCompleted";
            this.cbIncludeCompleted.PhoenixUIControl.ObjectId = 3;
            this.cbIncludeCompleted.Size = new System.Drawing.Size(181, 16);
            this.cbIncludeCompleted.TabIndex = 0;
            this.cbIncludeCompleted.Text = "Include Completed Requests";
            this.cbIncludeCompleted.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbIncludeCompleted_PhoenixUICheckedChangedEvent);
            // 
            // pnl3
            // 
            this.pnl3.Controls.Add(this.grdRequest);
            this.pnl3.Location = new System.Drawing.Point(5, 40);
            this.pnl3.Name = "pnl3";
            this.pnl3.Size = new System.Drawing.Size(681, 404);
            this.pnl3.TabIndex = 1;
            this.pnl3.TabStop = false;
            // 
            // grdRequest
            // 
            this.grdRequest.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colWorkId,
            this.colCreateDt,
            this.colDescription,
            this.colDueDt,
            this.colDueDtTime,
            this.colOwner,
            this.colWorkStatus,
            this.colStatus,
            this.colAcctType,
            this.colClassCode,
            this.colOwnerEmplId,
            this.colRIMNo});
            this.grdRequest.Location = new System.Drawing.Point(7, 12);
            this.grdRequest.Name = "grdRequest";
            this.grdRequest.Size = new System.Drawing.Size(666, 384);
            this.grdRequest.TabIndex = 0;
            this.grdRequest.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdRequest_FetchRowDone);
            this.grdRequest.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdRequest_BeforePopulate);
            // 
            // colWorkId
            // 
            this.colWorkId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colWorkId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colWorkId.PhoenixUIControl.ObjectId = 12;
            this.colWorkId.PhoenixUIControl.XmlTag = "WorkId";
            this.colWorkId.Title = "ID";
            this.colWorkId.Width = 57;
            // 
            // colCreateDt
            // 
            this.colCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCreateDt.PhoenixUIControl.ObjectId = 13;
            this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.colCreateDt.Title = "Create Dt";
            this.colCreateDt.Width = 79;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 14;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 191;
            // 
            // colDueDt
            // 
            this.colDueDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDueDt.PhoenixUIControl.ObjectId = 15;
            this.colDueDt.PhoenixUIControl.XmlTag = "DueDt";
            this.colDueDt.Title = "Due Dt";
            this.colDueDt.Width = 74;
            // 
            // colDueDtTime
            // 
            this.colDueDtTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colDueDtTime.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.colDueDtTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colDueDtTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.colDueDtTime.PhoenixUIControl.ObjectId = 20;
            this.colDueDtTime.PhoenixUIControl.XmlTag = "DueDtTime";
            this.colDueDtTime.Title = "Time";
            this.colDueDtTime.Width = 58;
            // 
            // colOwner
            // 
            this.colOwner.PhoenixUIControl.ObjectId = 16;
            this.colOwner.PhoenixUIControl.XmlTag = "Name";
            this.colOwner.Title = "Owner";
            this.colOwner.Width = 115;
            // 
            // colWorkStatus
            // 
            this.colWorkStatus.PhoenixUIControl.ObjectId = 19;
            this.colWorkStatus.PhoenixUIControl.XmlTag = "WorkStatus";
            this.colWorkStatus.Title = "Status";
            this.colWorkStatus.Width = 68;
            // 
            // colStatus
            // 
            this.colStatus.PhoenixUIControl.ObjectId = 17;
            this.colStatus.PhoenixUIControl.XmlTag = "Status";
            this.colStatus.Title = "Status";
            this.colStatus.Visible = false;
            this.colStatus.Width = 61;
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "Acct Type";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 0;
            // 
            // colClassCode
            // 
            this.colClassCode.PhoenixUIControl.XmlTag = "ClassCode";
            this.colClassCode.Title = "Class Code";
            this.colClassCode.Visible = false;
            this.colClassCode.Width = 0;
            // 
            // colOwnerEmplId
            // 
            this.colOwnerEmplId.PhoenixUIControl.XmlTag = "OwnerEmpId";
            this.colOwnerEmplId.Title = "Owner Empl ID";
            this.colOwnerEmplId.Visible = false;
            this.colOwnerEmplId.Width = 0;
            // 
            // colRIMNo
            // 
            this.colRIMNo.PhoenixUIControl.XmlTag = "RimNo";
            this.colRIMNo.Title = "Class Code";
            this.colRIMNo.Visible = false;
            this.colRIMNo.Width = 0;
            // 
            // pbWork
            // 
            this.pbWork.ObjectId = 10;
            this.pbWork.ShortText = "pbWork";
            this.pbWork.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbWork_Click);
            // 
            // pbNew
            // 
            this.pbNew.ObjectId = 9;
            this.pbNew.ShortText = "pbNew";
            this.pbNew.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbNew_Click);
            // 
            // frmRequest
            // 
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbRequestCriteria);
            this.Controls.Add(this.pnl3);
            this.Name = "frmRequest";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmRequest_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmRequest_PInitBeginEvent);
            this.gbRequestCriteria.ResumeLayout(false);
            this.pnl3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        #region Variables
        // Parameters Tied to Main Business Object
        PDecimal _pnRIMNo = new PDecimal("RIMNo");
        PString _psCustomerName = new PString("CustomerName");
        PString _psAcctType = new PString("AcctType");
        PString _psAcctNo = new PString("AcctNo");
        // Other Parameters to be Addressed

        // grdRequest vars moved here 

        private PString gsTemp = new PString("gsTemp");
        private enum CallOtherForm
        {
            NewClick,
            WorkClick
        }
        #endregion

        #region Overrides
        public override void OnCreateParameters()
        {
            // Parameters Tied to Main Business Object
            Parameters.Add(_pnRIMNo);

            Parameters.Add(_psCustomerName);

            Parameters.Add(_psAcctType);

            Parameters.Add(_psAcctNo);

            // Other Parameters to be Addressed

        }
        #endregion

        #region Methods

        #endregion

        #region Events
        protected ReturnType frmRequest_PInitBeginEvent()
        {
            this.AppToolBarId = AppToolBarType.NoEdit;

            this.ScreenId = Phoenix.Shared.Constants.ScreenId.Request;
            grdRequest.DoubleClickAction = pbWork;
            pbNew.NextScreenId = Phoenix.Shared.Constants.ScreenId.RequestEdit;
            pbWork.NextScreenId = Phoenix.Shared.Constants.ScreenId.RefWork;

            return ReturnType.Success;

            #region Commented Code

            #endregion
        }

        protected void frmRequest_PInitCompleteEvent()
        {
            if (StringHelper.StrTrimX(_psCustomerName.Value) != string.Empty)
            {
                gsTemp.Value = (IsNew ? this.NewRecordTitle : this.EditRecordTitle);

                gsTemp.Value = gsTemp.Value + " [" + StringHelper.StrTrimX(_psCustomerName.Value) + "]";

                EditRecordTitle = gsTemp.Value;

                NewRecordTitle = gsTemp.Value;

            }

        }

        public void cbIncludeCompleted_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            grdRequest.PopulateTable();

        }

        public void grdRequest_BeforePopulate(object sender, GridPopulateArgs e)
        {
            GbWorkQueue _gbWorkQueue = new GbWorkQueue();

            // Parameters Tied to Main Business Object

            _gbWorkQueue.RimNo.Value = Convert.ToInt32(_pnRIMNo.Value);

            _gbWorkQueue.CustomerName.Value = _psCustomerName.Value;

            _gbWorkQueue.AcctType.Value = _psAcctType.Value;

            _gbWorkQueue.AcctNo.Value = _psAcctNo.Value;

            _gbWorkQueue.ResponseTypeId = 12;

            grdRequest.ListViewObject = _gbWorkQueue;

            grdRequest.Filters.Clear();

            Filter includeCompleted = new Filter("1", cbIncludeCompleted.Checked);

            grdRequest.Filters.Add(includeCompleted);

        }

        public void pbNew_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.NewClick);
        }

        public void pbWork_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.WorkClick);
        }
        private void grdRequest_FetchRowDone(object sender, GridRowArgs e)
        {
            this.colWorkId.TextAlignment = ContentAlignment.MiddleLeft;
        }

        #endregion

        #region CallXMThruCDS

        #endregion

        #region CallOtherForms
        private void CallOtherForms(CallOtherForm caseName)
        {
            PfwStandard tempWin = null;

            if (caseName == CallOtherForm.NewClick)
            {
                tempWin = CreateWindow("phoenix.client.request", "Phoenix.Client.WorkQueue", "frmRequestEdit");

                tempWin.InitParameters(this, "Request", null, _pnRIMNo.Value, _psCustomerName.Value, _psAcctType.Value, _psAcctNo.Value);

            }
            else if (caseName == CallOtherForm.WorkClick)
            {
                tempWin = CreateWindow("phoenix.client.workrefreq", "Phoenix.Client.WorkQueue", "frmWorkRefReq");

                tempWin.InitParameters(Convert.ToInt32(this.colWorkId.UnFormattedValue == null ? 0 : this.colWorkId.UnFormattedValue), "Request");

            }

            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                tempWin.ParentGrid = grdRequest;
                tempWin.Show();
            }
        }
        #endregion

        #region EnableDisableLogic

        #endregion

        #region Commented Code

        #endregion

    }
}
