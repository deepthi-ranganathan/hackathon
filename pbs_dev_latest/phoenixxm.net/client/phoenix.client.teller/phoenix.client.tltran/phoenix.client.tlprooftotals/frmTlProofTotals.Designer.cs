namespace phoenix.client.tlprooftotals
{
    partial class frmTlProofTotals
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
            this.gbDisplayOpt = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdProofTotals = new Phoenix.Windows.Forms.PGrid();
            this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoChecks = new Phoenix.Windows.Forms.PGridColumn();
            this.colTotalAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatusText = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckType = new Phoenix.Windows.Forms.PGridColumn();
            this.rbAll = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbProofed = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbNotProofed = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.pbProcess = new Phoenix.Windows.Forms.PAction();
            this.pbProofDetails = new Phoenix.Windows.Forms.PAction();
            this.colProofPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.gbDisplayOpt.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbProcess,
            this.pbProofDetails});
            // 
            // gbDisplayOpt
            // 
            this.gbDisplayOpt.Controls.Add(this.grdProofTotals);
            this.gbDisplayOpt.Controls.Add(this.rbAll);
            this.gbDisplayOpt.Controls.Add(this.rbProofed);
            this.gbDisplayOpt.Controls.Add(this.rbNotProofed);
            this.gbDisplayOpt.Location = new System.Drawing.Point(2, 1);
            this.gbDisplayOpt.Name = "gbDisplayOpt";
            this.gbDisplayOpt.PhoenixUIControl.ObjectId = 1;
            this.gbDisplayOpt.Size = new System.Drawing.Size(685, 445);
            this.gbDisplayOpt.TabIndex = 0;
            this.gbDisplayOpt.TabStop = false;
            this.gbDisplayOpt.Text = "Display Options";
            // 
            // grdProofTotals
            // 
            this.grdProofTotals.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colBranchNo,
            this.colDrawerNo,
            this.colDescription,
            this.colNoChecks,
            this.colTotalAmt,
            this.colStatusText,
            this.colStatus,
            this.colCheckType,
            this.colProofPtid});
            this.grdProofTotals.LinesInHeader = 2;
            this.grdProofTotals.Location = new System.Drawing.Point(0, 43);
            this.grdProofTotals.Name = "grdProofTotals";
            this.grdProofTotals.Size = new System.Drawing.Size(685, 402);
            this.grdProofTotals.TabIndex = 3;
            this.grdProofTotals.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdProofTotals_AfterPopulate);
            this.grdProofTotals.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdProofTotals_SelectedIndexChanged);
            this.grdProofTotals.RowClicked += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdProofTotals_RowClicked);
            this.grdProofTotals.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdProofTotals_FetchRowDone);
            this.grdProofTotals.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdProofTotals_BeforePopulate);
            // 
            // colBranchNo
            // 
            this.colBranchNo.Disabled = true;
            this.colBranchNo.PhoenixUIControl.ObjectId = 6;
            this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo.Title = "Column";
            this.colBranchNo.Visible = false;
            this.colBranchNo.Width = 50;
            // 
            // colDrawerNo
            // 
            this.colDrawerNo.Disabled = true;
            this.colDrawerNo.PhoenixUIControl.ObjectId = 7;
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "Column";
            this.colDrawerNo.Visible = false;
            this.colDrawerNo.Width = 50;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 8;
            this.colDescription.PhoenixUIControl.XmlTag = "1";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 230;
            // 
            // colNoChecks
            // 
            this.colNoChecks.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoChecks.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoChecks.PhoenixUIControl.ObjectId = 9;
            this.colNoChecks.PhoenixUIControl.XmlTag = "NoItems";
            this.colNoChecks.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colNoChecks.Title = "# Of Checks";
            this.colNoChecks.Width = 140;
            // 
            // colTotalAmt
            // 
            this.colTotalAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTotalAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTotalAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTotalAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTotalAmt.PhoenixUIControl.ObjectId = 10;
            this.colTotalAmt.PhoenixUIControl.XmlTag = "Amount";
            this.colTotalAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colTotalAmt.Title = "Total Amount";
            this.colTotalAmt.Width = 210;
            // 
            // colStatusText
            // 
            this.colStatusText.PhoenixUIControl.XmlTag = "2";
            this.colStatusText.Title = "Status";
            // 
            // colStatus
            // 
            this.colStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus.PhoenixUIControl.ObjectId = 11;
            this.colStatus.PhoenixUIControl.XmlTag = "ProofStatus";
            this.colStatus.Title = "Status";
            this.colStatus.Visible = false;
            this.colStatus.Width = 137;
            // 
            // colCheckType
            // 
            this.colCheckType.PhoenixUIControl.XmlTag = "CheckType";
            this.colCheckType.Title = "Column";
            this.colCheckType.Visible = false;
            this.colCheckType.Width = 130;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Description = null;
            this.rbAll.Location = new System.Drawing.Point(266, 19);
            this.rbAll.Name = "rbAll";
            this.rbAll.PhoenixUIControl.ObjectId = 4;
            this.rbAll.Size = new System.Drawing.Size(42, 18);
            this.rbAll.TabIndex = 2;
            this.rbAll.Text = "All";
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbProofed
            // 
            this.rbProofed.AutoSize = true;
            this.rbProofed.Description = null;
            this.rbProofed.Location = new System.Drawing.Point(145, 19);
            this.rbProofed.Name = "rbProofed";
            this.rbProofed.PhoenixUIControl.ObjectId = 3;
            this.rbProofed.Size = new System.Drawing.Size(68, 18);
            this.rbProofed.TabIndex = 1;
            this.rbProofed.Text = "Proofed";
            this.rbProofed.CheckedChanged += new System.EventHandler(this.rbProofed_CheckedChanged);
            // 
            // rbNotProofed
            // 
            this.rbNotProofed.AutoSize = true;
            this.rbNotProofed.Description = null;
            this.rbNotProofed.Location = new System.Drawing.Point(10, 19);
            this.rbNotProofed.Name = "rbNotProofed";
            this.rbNotProofed.PhoenixUIControl.ObjectId = 2;
            this.rbNotProofed.Size = new System.Drawing.Size(88, 18);
            this.rbNotProofed.TabIndex = 0;
            this.rbNotProofed.Text = "Not Proofed";
            this.rbNotProofed.CheckedChanged += new System.EventHandler(this.rbNotProofed_CheckedChanged);
            // 
            // pbProcess
            // 
            this.pbProcess.ObjectId = 77;
            this.pbProcess.ShortText = "&Proof Checks";
            this.pbProcess.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbProcess_Click);
            // 
            // pbProofDetails
            // 
            this.pbProofDetails.ObjectId = 78;
            this.pbProofDetails.ShortText = "Proof &Detail...";
            this.pbProofDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbProofDetails_Click);
            // 
            // colProofPtid
            // 
            this.colProofPtid.PhoenixUIControl.XmlTag = "ProofPtid";
            this.colProofPtid.Title = "ProofPtid";
            this.colProofPtid.Visible = false;
            // 
            // frmTlProofTotals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbDisplayOpt);
            this.Name = "frmTlProofTotals";
            this.ScreenId = 2866;
            this.Text = "Proof Totals";
            this.Load += new System.EventHandler(this.frmTlProofTotals_Load);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.fwStandard_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.fwStandard_PInitBeginEvent);
            this.gbDisplayOpt.ResumeLayout(false);
            this.gbDisplayOpt.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbDisplayOpt;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbNotProofed;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbProofed;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbAll;
        private Phoenix.Windows.Forms.PGrid grdProofTotals;
        private Phoenix.Windows.Forms.PGridColumn colBranchNo;
        private Phoenix.Windows.Forms.PGridColumn colDrawerNo;
        private Phoenix.Windows.Forms.PGridColumn colDescription;
        private Phoenix.Windows.Forms.PGridColumn colNoChecks;
        private Phoenix.Windows.Forms.PGridColumn colTotalAmt;
        private Phoenix.Windows.Forms.PGridColumn colStatus;
        private Phoenix.Windows.Forms.PAction pbProcess;
        private Phoenix.Windows.Forms.PAction pbProofDetails;
        private Phoenix.Windows.Forms.PGridColumn colCheckType;
        private Phoenix.Windows.Forms.PGridColumn colStatusText;
        private Phoenix.Windows.Forms.PGridColumn colProofPtid;


    }
}