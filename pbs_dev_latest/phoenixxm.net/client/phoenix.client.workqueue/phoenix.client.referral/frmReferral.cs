#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2006-2008 Harland Financial Solutions Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmReferral.cs
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
	/// Summary description for frmReferral.
	/// </summary>
	public class frmReferral : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbReferralCriteria;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbIncludeCompleted;
		private Phoenix.Windows.Forms.PGroupBoxStandard pnl3;
		private Phoenix.Windows.Forms.PGrid grdReferral;
		private Phoenix.Windows.Forms.PGridColumn colWorkId;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colDueDt;
		private Phoenix.Windows.Forms.PGridColumn colDueDtTime;
		private Phoenix.Windows.Forms.PGridColumn colOwner;
		private Phoenix.Windows.Forms.PGridColumn colProduct;
		private Phoenix.Windows.Forms.PGridColumn colWorkStatus;
		private Phoenix.Windows.Forms.PGridColumn colStatus;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colClassCode;
		private Phoenix.Windows.Forms.PGridColumn colOwnerEmplId;
		private Phoenix.Windows.Forms.PGridColumn colRefCode;
		private Phoenix.Windows.Forms.PGridColumn colDepLoan;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colApplType;
		private Phoenix.Windows.Forms.PGridColumn colRIMNo;
        private PAction pbNew;
		// DEL_CON:private Phoenix.Windows.Forms.PAction pbClose;
		// DEL_CON:private Phoenix.Windows.Forms.PAction pbNew;
		private Phoenix.Windows.Forms.PAction pbWork;

		public frmReferral()
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
            this.gbReferralCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbIncludeCompleted = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.pnl3 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdReferral = new Phoenix.Windows.Forms.PGrid();
            this.colWorkId = new Phoenix.Windows.Forms.PGridColumn();
            this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colDueDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDueDtTime = new Phoenix.Windows.Forms.PGridColumn();
            this.colOwner = new Phoenix.Windows.Forms.PGridColumn();
            this.colProduct = new Phoenix.Windows.Forms.PGridColumn();
            this.colWorkStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colClassCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colOwnerEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colRefCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colDepLoan = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colApplType = new Phoenix.Windows.Forms.PGridColumn();
            this.colRIMNo = new Phoenix.Windows.Forms.PGridColumn();
            this.pbWork = new Phoenix.Windows.Forms.PAction();
            this.pbNew = new Phoenix.Windows.Forms.PAction();
            this.gbReferralCriteria.SuspendLayout();
            this.pnl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbNew,
            this.pbWork});
            // 
            // gbReferralCriteria
            // 
            this.gbReferralCriteria.Controls.Add(this.cbIncludeCompleted);
            this.gbReferralCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbReferralCriteria.Name = "gbReferralCriteria";
            this.gbReferralCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbReferralCriteria.Size = new System.Drawing.Size(682, 39);
            this.gbReferralCriteria.TabIndex = 0;
            this.gbReferralCriteria.TabStop = false;
            this.gbReferralCriteria.Text = "Referral Criteria";
            // 
            // cbIncludeCompleted
            // 
            this.cbIncludeCompleted.CodeValue = null;
            this.cbIncludeCompleted.Location = new System.Drawing.Point(4, 16);
            this.cbIncludeCompleted.Name = "cbIncludeCompleted";
            this.cbIncludeCompleted.PhoenixUIControl.ObjectId = 3;
            this.cbIncludeCompleted.Size = new System.Drawing.Size(231, 16);
            this.cbIncludeCompleted.TabIndex = 0;
            this.cbIncludeCompleted.Text = "Include Sold and Declined Referrals";
            this.cbIncludeCompleted.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbIncludeCompleted_PhoenixUICheckedChangedEvent);
            // 
            // pnl3
            // 
            this.pnl3.Controls.Add(this.grdReferral);
            this.pnl3.Location = new System.Drawing.Point(4, 39);
            this.pnl3.Name = "pnl3";
            this.pnl3.Size = new System.Drawing.Size(682, 405);
            this.pnl3.TabIndex = 1;
            this.pnl3.TabStop = false;
            // 
            // grdReferral
            // 
            this.grdReferral.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colWorkId,
            this.colCreateDt,
            this.colDescription,
            this.colDueDt,
            this.colDueDtTime,
            this.colOwner,
            this.colProduct,
            this.colWorkStatus,
            this.colStatus,
            this.colAcctType,
            this.colClassCode,
            this.colOwnerEmplId,
            this.colRefCode,
            this.colDepLoan,
            this.colAcctNo,
            this.colApplType,
            this.colRIMNo});
            this.grdReferral.Location = new System.Drawing.Point(8, 12);
            this.grdReferral.Name = "grdReferral";
            this.grdReferral.Size = new System.Drawing.Size(668, 384);
            this.grdReferral.TabIndex = 0;
            this.grdReferral.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdReferral_FetchRowDone);
            this.grdReferral.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdReferral_BeforePopulate);
            // 
            // colWorkId
            // 
            this.colWorkId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colWorkId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colWorkId.PhoenixUIControl.ObjectId = 12;
            this.colWorkId.PhoenixUIControl.XmlTag = "WorkId";
            this.colWorkId.Title = "ID";
            this.colWorkId.Width = 55;
            // 
            // colCreateDt
            // 
            this.colCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCreateDt.PhoenixUIControl.ObjectId = 13;
            this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.colCreateDt.Title = "Create Dt";
            this.colCreateDt.Width = 65;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 19;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 131;
            // 
            // colDueDt
            // 
            this.colDueDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDueDt.PhoenixUIControl.ObjectId = 15;
            this.colDueDt.PhoenixUIControl.XmlTag = "DueDt";
            this.colDueDt.Title = "Due Dt";
            this.colDueDt.Width = 68;
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
            this.colDueDtTime.Width = 56;
            // 
            // colOwner
            // 
            this.colOwner.PhoenixUIControl.ObjectId = 16;
            this.colOwner.PhoenixUIControl.XmlTag = "Name";
            this.colOwner.Title = "Owner";
            this.colOwner.Width = 96;
            // 
            // colProduct
            // 
            this.colProduct.PhoenixUIControl.ObjectId = 14;
            this.colProduct.PhoenixUIControl.XmlTag = "Product";
            this.colProduct.Title = "Product";
            this.colProduct.Width = 109;
            // 
            // colWorkStatus
            // 
            this.colWorkStatus.PhoenixUIControl.ObjectId = 18;
            this.colWorkStatus.PhoenixUIControl.XmlTag = "WorkStatus";
            this.colWorkStatus.Title = "Status";
            this.colWorkStatus.Width = 61;
            // 
            // colStatus
            // 
            this.colStatus.PhoenixUIControl.ObjectId = 17;
            this.colStatus.PhoenixUIControl.XmlTag = "Status";
            this.colStatus.Title = "Status";
            this.colStatus.Visible = false;
            this.colStatus.Width = 0;
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
            // colRefCode
            // 
            this.colRefCode.PhoenixUIControl.XmlTag = "RefferralCode";
            this.colRefCode.Title = "";
            this.colRefCode.Visible = false;
            this.colRefCode.Width = 0;
            // 
            // colDepLoan
            // 
            this.colDepLoan.PhoenixUIControl.XmlTag = "Dep_Loan";
            this.colDepLoan.Title = "Dep Loan";
            this.colDepLoan.Visible = false;
            this.colDepLoan.Width = 0;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.XmlTag = "Acct_No";
            this.colAcctNo.Title = "Acct No";
            this.colAcctNo.Visible = false;
            this.colAcctNo.Width = 0;
            // 
            // colApplType
            // 
            this.colApplType.PhoenixUIControl.XmlTag = "ApplType";
            this.colApplType.Title = "";
            this.colApplType.Visible = false;
            this.colApplType.Width = 0;
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
            // frmReferral
            // 
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbReferralCriteria);
            this.Controls.Add(this.pnl3);
            this.Name = "frmReferral";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmReferral_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmReferral_PInitBeginEvent);
            this.gbReferralCriteria.ResumeLayout(false);
            this.pnl3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        #region Variables
        // Parameters Tied to Main Business Object
        PDecimal _pnRIMNo = new PDecimal("RIMNo");

        PString _psCustomerName = new PString("CustomerName");

        // Other Parameters to be Addressed

        PString _psTINNo = new PString("_psTINNo");

        // grdReferral vars moved here 

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

            // Other Parameters to be Addressed

        }
        #endregion

        #region Methods

        #endregion

        #region Events
        protected ReturnType frmReferral_PInitBeginEvent()
        {
            this.AppToolBarId = AppToolBarType.NoEdit;

            this.ScreenId = Phoenix.Shared.Constants.ScreenId.Referral;

            grdReferral.DoubleClickAction = pbWork;

            pbNew.NextScreenId = Phoenix.Shared.Constants.ScreenId.ReferralEdit;
            pbWork.NextScreenId = Phoenix.Shared.Constants.ScreenId.RefWork;

            return ReturnType.Success;
            #region Commented Code

            #endregion

        }

        protected void frmReferral_PInitCompleteEvent()
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
            grdReferral.PopulateTable();

        }

        public void grdReferral_BeforePopulate(object sender, GridPopulateArgs e)
        {
            GbWorkQueue _gbWorkQueue = new GbWorkQueue();

            // Parameters Tied to Main Business Object

            _gbWorkQueue.RimNo.Value = Convert.ToInt32(_pnRIMNo.Value);

            _gbWorkQueue.CustomerName.Value = _psCustomerName.Value;

            _gbWorkQueue.ResponseTypeId = 11;

            grdReferral.ListViewObject = _gbWorkQueue;

            grdReferral.Filters.Clear();

            Filter includeCompleted = new Filter("1", cbIncludeCompleted.Checked);

            grdReferral.Filters.Add(includeCompleted);

        }

        public void pbNew_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.NewClick);
        }

        public void pbWork_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.WorkClick);
        }
        private void grdReferral_FetchRowDone(object sender, GridRowArgs e)
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
                tempWin = CreateWindow("phoenix.client.referral", "Phoenix.Client.WorkQueue", "frmReferralEdit");
                tempWin.InitParameters(this, "Referral", null, _pnRIMNo.Value, this.colRefCode.UnFormattedValue, _psCustomerName.Value, null, null);

            }
            else if (caseName == CallOtherForm.WorkClick)
            {
                tempWin = CreateWindow("phoenix.client.workrefreq", "Phoenix.Client.WorkQueue", "frmWorkRefReq");

                tempWin.InitParameters(Convert.ToInt32(this.colWorkId.UnFormattedValue == null ? 0 : this.colWorkId.UnFormattedValue), "Referral");
            }

            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                tempWin.ParentGrid = grdReferral;
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
