namespace Phoenix.Client.Teller
{
	partial class frmTlCaptureViewPendingEOB
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
            this.gbDisplayCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblEffectiveDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfEffectiveDate = new Phoenix.Windows.Forms.PdfStandard();
            this.cbAllBranches = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbControl = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdPendingEOB = new Phoenix.Windows.Forms.PDataGridView();
            this.colWorkstation = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colLastTranBranchNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colLastTranDrawerNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colLastTranEmployee = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colLastTranCreateDt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colBOBBranchNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colBOBDrawerNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colBOBEmplName = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colBOBCreateDt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.pbRefresh = new Phoenix.Windows.Forms.PAction();
            this.gbDisplayCriteria.SuspendLayout();
            this.gbControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdPendingEOB)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbRefresh});
            // 
            // gbDisplayCriteria
            // 
            this.gbDisplayCriteria.Controls.Add(this.lblEffectiveDate);
            this.gbDisplayCriteria.Controls.Add(this.dfEffectiveDate);
            this.gbDisplayCriteria.Controls.Add(this.cbAllBranches);
            this.gbDisplayCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbDisplayCriteria.Name = "gbDisplayCriteria";
            this.gbDisplayCriteria.Size = new System.Drawing.Size(683, 50);
            this.gbDisplayCriteria.TabIndex = 2;
            this.gbDisplayCriteria.TabStop = false;
            this.gbDisplayCriteria.Text = "Display Criteria";
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoEllipsis = true;
            this.lblEffectiveDate.Location = new System.Drawing.Point(487, 20);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.PhoenixUIControl.ObjectId = 12;
            this.lblEffectiveDate.Size = new System.Drawing.Size(80, 20);
            this.lblEffectiveDate.TabIndex = 4;
            this.lblEffectiveDate.Text = "Effective Date:";
            // 
            // dfEffectiveDate
            // 
            this.dfEffectiveDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.Location = new System.Drawing.Point(579, 20);
            this.dfEffectiveDate.Name = "dfEffectiveDate";
            this.dfEffectiveDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfEffectiveDate.PhoenixUIControl.ObjectId = 12;
            this.dfEffectiveDate.Size = new System.Drawing.Size(100, 20);
            this.dfEffectiveDate.TabIndex = 3;
            this.dfEffectiveDate.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfEffectiveDate_PhoenixUIValidateEvent);
            // 
            // cbAllBranches
            // 
            this.cbAllBranches.AutoSize = true;
            this.cbAllBranches.BackColor = System.Drawing.SystemColors.Control;
            this.cbAllBranches.Checked = true;
            this.cbAllBranches.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllBranches.Location = new System.Drawing.Point(8, 20);
            this.cbAllBranches.Name = "cbAllBranches";
            this.cbAllBranches.PhoenixUIControl.ObjectId = 11;
            this.cbAllBranches.Size = new System.Drawing.Size(91, 18);
            this.cbAllBranches.TabIndex = 2;
            this.cbAllBranches.Text = "All Branches";
            this.cbAllBranches.UseVisualStyleBackColor = false;
            this.cbAllBranches.Value = null;
            this.cbAllBranches.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbAllBranches_PhoenixUICheckedChangedEvent);
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.grdPendingEOB);
            this.gbControl.Location = new System.Drawing.Point(4, 50);
            this.gbControl.Name = "gbControl";
            this.gbControl.Size = new System.Drawing.Size(683, 394);
            this.gbControl.TabIndex = 900;
            this.gbControl.TabStop = false;
            // 
            // grdPendingEOB
            // 
            this.grdPendingEOB.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWorkstation,
            this.colLastTranBranchNo,
            this.colLastTranDrawerNo,
            this.colLastTranEmployee,
            this.colLastTranCreateDt,
            this.colBOBBranchNo,
            this.colBOBDrawerNo,
            this.colBOBEmplName,
            this.colBOBCreateDt});
            this.grdPendingEOB.IsMaxNumRowsCustomized = false;
            this.grdPendingEOB.LinesInHeader = 2;
            this.grdPendingEOB.Location = new System.Drawing.Point(6, 17);
            this.grdPendingEOB.Name = "grdPendingEOB";
            this.grdPendingEOB.Size = new System.Drawing.Size(671, 369);
            this.grdPendingEOB.TabIndex = 0;
            this.grdPendingEOB.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdPendingEOB_BeforePopulate);
            this.grdPendingEOB.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdPendingEOB_AfterPopulate);
            // 
            // colWorkstation
            // 
            this.colWorkstation.Frozen = true;
            this.colWorkstation.HeaderText = "Workstation";
            this.colWorkstation.Name = "colWorkstation";
            this.colWorkstation.PhoenixUIControl.ObjectId = 1;
            this.colWorkstation.PhoenixUIControl.XmlTag = "TlCaptureWorkstation";
            this.colWorkstation.Text = "";
            this.colWorkstation.Title = "Workstation";
            this.colWorkstation.Width = 200;
            // 
            // colLastTranBranchNo
            // 
            this.colLastTranBranchNo.HeaderText = "Last Tran Branch #";
            this.colLastTranBranchNo.Name = "colLastTranBranchNo";
            this.colLastTranBranchNo.PhoenixUIControl.ObjectId = 2;
            this.colLastTranBranchNo.PhoenixUIControl.XmlTag = "LastTranBranchNo";
            this.colLastTranBranchNo.Text = "";
            this.colLastTranBranchNo.Title = "Last Tran Branch #";
            this.colLastTranBranchNo.Width = 65;
            // 
            // colLastTranDrawerNo
            // 
            this.colLastTranDrawerNo.HeaderText = "Last Tran Drawer #";
            this.colLastTranDrawerNo.Name = "colLastTranDrawerNo";
            this.colLastTranDrawerNo.PhoenixUIControl.ObjectId = 3;
            this.colLastTranDrawerNo.PhoenixUIControl.XmlTag = "LastTranDrawerNo";
            this.colLastTranDrawerNo.Text = "";
            this.colLastTranDrawerNo.Title = "Last Tran Drawer #";
            this.colLastTranDrawerNo.Width = 65;
            // 
            // colLastTranEmployee
            // 
            this.colLastTranEmployee.HeaderText = "Last Tran Employee";
            this.colLastTranEmployee.Name = "colLastTranEmployee";
            this.colLastTranEmployee.PhoenixUIControl.ObjectId = 4;
            this.colLastTranEmployee.PhoenixUIControl.XmlTag = "LastTranEmplName";
            this.colLastTranEmployee.Text = "";
            this.colLastTranEmployee.Title = "Last Tran Employee";
            this.colLastTranEmployee.Width = 150;
            // 
            // colLastTranCreateDt
            // 
            this.colLastTranCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastTranCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colLastTranCreateDt.HeaderText = "Last Tran Date/Time";
            this.colLastTranCreateDt.Name = "colLastTranCreateDt";
            this.colLastTranCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastTranCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colLastTranCreateDt.PhoenixUIControl.ObjectId = 5;
            this.colLastTranCreateDt.PhoenixUIControl.XmlTag = "LastTranCreateDt";
            this.colLastTranCreateDt.Text = "";
            this.colLastTranCreateDt.Title = "Last Tran Date/Time";
            this.colLastTranCreateDt.Width = 125;
            // 
            // colBOBBranchNo
            // 
            this.colBOBBranchNo.HeaderText = "BOB Branch #";
            this.colBOBBranchNo.Name = "colBOBBranchNo";
            this.colBOBBranchNo.PhoenixUIControl.ObjectId = 6;
            this.colBOBBranchNo.PhoenixUIControl.XmlTag = "BobBranchNo";
            this.colBOBBranchNo.Text = "";
            this.colBOBBranchNo.Title = "BOB Branch #";
            this.colBOBBranchNo.Width = 65;
            // 
            // colBOBDrawerNo
            // 
            this.colBOBDrawerNo.HeaderText = "BOB Drawer #";
            this.colBOBDrawerNo.Name = "colBOBDrawerNo";
            this.colBOBDrawerNo.PhoenixUIControl.ObjectId = 7;
            this.colBOBDrawerNo.PhoenixUIControl.XmlTag = "BobDrawerNo";
            this.colBOBDrawerNo.Text = "";
            this.colBOBDrawerNo.Title = "BOB Drawer #";
            this.colBOBDrawerNo.Width = 65;
            // 
            // colBOBEmplName
            // 
            this.colBOBEmplName.HeaderText = "BOB Employee";
            this.colBOBEmplName.Name = "colBOBEmplName";
            this.colBOBEmplName.PhoenixUIControl.ObjectId = 8;
            this.colBOBEmplName.PhoenixUIControl.XmlTag = "BobEmplName";
            this.colBOBEmplName.Text = "";
            this.colBOBEmplName.Title = "BOB Employee";
            this.colBOBEmplName.Width = 150;
            // 
            // colBOBCreateDt
            // 
            this.colBOBCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colBOBCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colBOBCreateDt.HeaderText = "BOB Date/Time";
            this.colBOBCreateDt.Name = "colBOBCreateDt";
            this.colBOBCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colBOBCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colBOBCreateDt.PhoenixUIControl.ObjectId = 9;
            this.colBOBCreateDt.PhoenixUIControl.XmlTag = "BobCreateDt";
            this.colBOBCreateDt.Text = "";
            this.colBOBCreateDt.Title = "BOB Date/Time";
            this.colBOBCreateDt.Width = 125;
            // 
            // pbRefresh
            // 
            this.pbRefresh.LongText = "Refresh";
            this.pbRefresh.ObjectId = 10;
            this.pbRefresh.ShortText = "Refresh";
            this.pbRefresh.Tag = null;
            this.pbRefresh.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRefresh_Click);
            // 
            // frmTlCaptureViewPendingEOB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbControl);
            this.Controls.Add(this.gbDisplayCriteria);
            this.Name = "frmTlCaptureViewPendingEOB";
            this.Text = "Pending EOBs";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCaptureViewPendingEOB_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCaptureViewPendingEOB_PInitCompleteEvent);
            this.gbDisplayCriteria.ResumeLayout(false);
            this.gbDisplayCriteria.PerformLayout();
            this.gbControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdPendingEOB)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Phoenix.Windows.Forms.PGroupBoxStandard gbDisplayCriteria;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbAllBranches;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbControl;
		private Phoenix.Windows.Forms.PLabelStandard lblEffectiveDate;
		private Phoenix.Windows.Forms.PdfStandard dfEffectiveDate;
		private Phoenix.Windows.Forms.PAction pbRefresh;
        private Windows.Forms.PDataGridView grdPendingEOB;
        private Windows.Forms.PDataGridViewColumn colWorkstation;
        private Windows.Forms.PDataGridViewColumn colLastTranBranchNo;
        private Windows.Forms.PDataGridViewColumn colLastTranDrawerNo;
        private Windows.Forms.PDataGridViewColumn colLastTranEmployee;
        private Windows.Forms.PDataGridViewColumn colLastTranCreateDt;
        private Windows.Forms.PDataGridViewColumn colBOBBranchNo;
        private Windows.Forms.PDataGridViewColumn colBOBDrawerNo;
        private Windows.Forms.PDataGridViewColumn colBOBEmplName;
        private Windows.Forms.PDataGridViewColumn colBOBCreateDt;
	}
}