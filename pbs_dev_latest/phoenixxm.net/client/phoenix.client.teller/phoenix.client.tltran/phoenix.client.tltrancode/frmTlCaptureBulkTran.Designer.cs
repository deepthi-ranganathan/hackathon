namespace Phoenix.Client.Teller
{
    partial class frmTlCaptureBulkTran
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbIncludeProcessedBulk = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.grdBulkTransactions = new Phoenix.Windows.Forms.PGrid();
            this.colSeqNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversal = new Phoenix.Windows.Forms.PGridColumn();
            this.colRt = new Phoenix.Windows.Forms.PGridColumn();
            this.colEmployee = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colIsnNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranSetId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colRecordSource = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranStatusValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.pbProcess = new Phoenix.Windows.Forms.PAction();
            this.pbReverse = new Phoenix.Windows.Forms.PAction();
            this.gbGrid = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbProcess,
            this.pbReverse});
            // 
            // cbIncludeProcessedBulk
            // 
            this.cbIncludeProcessedBulk.AutoSize = true;
            this.cbIncludeProcessedBulk.BackColor = System.Drawing.SystemColors.Control;
            this.cbIncludeProcessedBulk.Checked = true;
            this.cbIncludeProcessedBulk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeProcessedBulk.Location = new System.Drawing.Point(5, 10);
            this.cbIncludeProcessedBulk.Name = "cbIncludeProcessedBulk";
            this.cbIncludeProcessedBulk.PhoenixUIControl.ObjectId = 1;
            this.cbIncludeProcessedBulk.Size = new System.Drawing.Size(227, 18);
            this.cbIncludeProcessedBulk.TabIndex = 0;
            this.cbIncludeProcessedBulk.Text = "Include Processed Bulk Transaction Sets";
            this.cbIncludeProcessedBulk.UseVisualStyleBackColor = false;
            this.cbIncludeProcessedBulk.Value = null;
            this.cbIncludeProcessedBulk.CheckedChanged += new System.EventHandler(this.cbIncludeProcessedBulk_CheckedChanged);
            // 
            // grdBulkTransactions
            // 
            this.grdBulkTransactions.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colSeqNo,
            this.colAccount,
            this.colDescription,
            this.colCheckNo,
            this.colAmount,
            this.colReversal,
            this.colRt,
            this.colEmployee,
            this.colStatus,
            this.colIsnNo,
            this.colBatchId,
            this.colTranSetId,
            this.colTranStatus,
            this.colRecordSource,
            this.colTranStatusValue,
            this.colTranCode,
            this.colTranDescription});
            this.grdBulkTransactions.IsMaxNumRowsCustomized = false;
            this.grdBulkTransactions.Location = new System.Drawing.Point(5, 34);
            this.grdBulkTransactions.Name = "grdBulkTransactions";
            this.grdBulkTransactions.Size = new System.Drawing.Size(669, 402);
            this.grdBulkTransactions.TabIndex = 1;
            this.grdBulkTransactions.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdBulkTransactions_FetchRowDone);
            this.grdBulkTransactions.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdBulkTransactions_AfterPopulate);
            this.grdBulkTransactions.Click += new System.EventHandler(this.grdBulkTransactions_Click);
            // 
            // colSeqNo
            // 
            this.colSeqNo.PhoenixUIControl.ObjectId = 3;
            this.colSeqNo.PhoenixUIControl.XmlTag = "SeqNo";
            this.colSeqNo.Title = "Seq #";
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 4;
            this.colAccount.PhoenixUIControl.XmlTag = "TlCaptureAcctNo";
            this.colAccount.Title = "Account";
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 5;
            this.colDescription.PhoenixUIControl.XmlTag = "TranDescription";
            this.colDescription.Title = "Description";
            // 
            // colCheckNo
            // 
            this.colCheckNo.PhoenixUIControl.ObjectId = 6;
            this.colCheckNo.PhoenixUIControl.XmlTag = "TlCaptureCheckNo";
            this.colCheckNo.Title = "Check #";
            // 
            // colAmount
            // 
            this.colAmount.PhoenixUIControl.ObjectId = 7;
            this.colAmount.PhoenixUIControl.XmlTag = "TlCaptureAmount";
            this.colAmount.Title = "Amount";
            // 
            // colReversal
            // 
            this.colReversal.PhoenixUIControl.ObjectId = 8;
            this.colReversal.PhoenixUIControl.XmlTag = "";
            this.colReversal.Title = "Reversal";
            // 
            // colRt
            // 
            this.colRt.PhoenixUIControl.ObjectId = 9;
            this.colRt.PhoenixUIControl.XmlTag = "TlCaptureRoutingNo";
            this.colRt.Title = "RT";
            // 
            // colEmployee
            // 
            this.colEmployee.PhoenixUIControl.ObjectId = 10;
            this.colEmployee.PhoenixUIControl.XmlTag = "Employee";
            this.colEmployee.Title = "Employee";
            // 
            // colStatus
            // 
            this.colStatus.PhoenixUIControl.ObjectId = 11;
            this.colStatus.PhoenixUIControl.XmlTag = "0";
            this.colStatus.Title = "Status";
            // 
            // colIsnNo
            // 
            this.colIsnNo.PhoenixUIControl.ObjectId = 12;
            this.colIsnNo.PhoenixUIControl.XmlTag = "TlCaptureIsn";
            this.colIsnNo.Title = "ISN #";
            // 
            // colBatchId
            // 
            this.colBatchId.PhoenixUIControl.ObjectId = 13;
            this.colBatchId.PhoenixUIControl.XmlTag = "BatchId";
            this.colBatchId.Title = "BatchId";
            this.colBatchId.Visible = false;
            // 
            // colTranSetId
            // 
            this.colTranSetId.PhoenixUIControl.ObjectId = 14;
            this.colTranSetId.PhoenixUIControl.XmlTag = "TranSetId";
            this.colTranSetId.Title = "TranSetId";
            this.colTranSetId.Visible = false;
            // 
            // colTranStatus
            // 
            this.colTranStatus.PhoenixUIControl.ObjectId = 15;
            this.colTranStatus.PhoenixUIControl.XmlTag = "TranStatus";
            this.colTranStatus.Title = "TranStatus";
            this.colTranStatus.Visible = false;
            // 
            // colRecordSource
            // 
            this.colRecordSource.PhoenixUIControl.ObjectId = 16;
            this.colRecordSource.PhoenixUIControl.XmlTag = "RecordSource";
            this.colRecordSource.Title = "RecordSource";
            this.colRecordSource.Visible = false;
            // 
            // colTranStatusValue
            // 
            this.colTranStatusValue.PhoenixUIControl.ObjectId = 19;
            this.colTranStatusValue.PhoenixUIControl.XmlTag = "TranStatus0";
            this.colTranStatusValue.Title = "";
            this.colTranStatusValue.Visible = false;
            // 
            // colTranCode
            // 
            this.colTranCode.PhoenixUIControl.ObjectId = 20;
            this.colTranCode.PhoenixUIControl.XmlTag = "TranCode";
            this.colTranCode.Title = "";
            this.colTranCode.Visible = false;
            // 
            // colTranDescription
            // 
            this.colTranDescription.PhoenixUIControl.ObjectId = 21;
            this.colTranDescription.PhoenixUIControl.XmlTag = "Description";
            this.colTranDescription.Title = "";
            this.colTranDescription.Visible = false;
            // 
            // pbProcess
            // 
            this.pbProcess.LongText = "&Process";
            this.pbProcess.Name = null;
            this.pbProcess.ObjectId = 17;
            this.pbProcess.Tag = null;
            this.pbProcess.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbProcess_Click);
            // 
            // pbReverse
            // 
            this.pbReverse.LongText = "R&everse";
            this.pbReverse.Name = null;
            this.pbReverse.ObjectId = 18;
            this.pbReverse.Tag = null;
            this.pbReverse.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReverse_Click);
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.cbIncludeProcessedBulk);
            this.gbGrid.Controls.Add(this.grdBulkTransactions);
            this.gbGrid.Location = new System.Drawing.Point(4, 2);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(682, 442);
            this.gbGrid.TabIndex = 0;
            this.gbGrid.TabStop = false;
            // 
            // frmTlCaptureBulkTran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbGrid);
            this.EditRecordTitle = "frmTlCaptureBulkTran";
            this.Name = "frmTlCaptureBulkTran";
            this.NewRecordTitle = "frmTlCaptureBulkTran";
            this.ScreenId = 3402;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.Text = "frmTlCaptureBulkTran";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCaptureBulkTran_PInitCompleteEvent);
            this.gbGrid.ResumeLayout(false);
            this.gbGrid.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Windows.Forms.PCheckBoxStandard cbIncludeProcessedBulk;
        private Windows.Forms.PAction pbProcess;
        private Windows.Forms.PAction pbReverse;
        private Windows.Forms.PGrid grdBulkTransactions;
        private Windows.Forms.PGridColumn colSeqNo;
        private Windows.Forms.PGridColumn colAccount;
        private Windows.Forms.PGridColumn colDescription;
        private Windows.Forms.PGridColumn colCheckNo;
        private Windows.Forms.PGridColumn colAmount;
        private Windows.Forms.PGridColumn colReversal;
        private Windows.Forms.PGridColumn colRt;
        private Windows.Forms.PGridColumn colEmployee;
        private Windows.Forms.PGridColumn colStatus;
        private Windows.Forms.PGridColumn colIsnNo;
        private Windows.Forms.PGridColumn colBatchId;
        private Windows.Forms.PGridColumn colTranSetId;
        private Windows.Forms.PGridColumn colTranStatus;
        private Windows.Forms.PGridColumn colRecordSource;
        private Windows.Forms.PGridColumn colTranStatusValue;
        private Windows.Forms.PGridColumn colTranCode;
        private Windows.Forms.PGridColumn colTranDescription;
        private Windows.Forms.PGroupBoxStandard gbGrid;
    }
}
