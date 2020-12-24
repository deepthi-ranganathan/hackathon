namespace Phoenix.Client.Teller
{
	partial class frmTlCaptureApplDetails
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
            this.cbIncludeRev = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfDrCrAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblDrCrAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTlCaptureISN = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTlCaptureISN = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbTlCaptureWorkstation = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblWorkstation = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblDrawer = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbDrawer = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblBranchNo = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfBranch = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblEffectiveDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfEffectiveDate = new Phoenix.Windows.Forms.PDfDisplay();
            this.gbControl = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCreditAmtTot = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfDebitAmtTot = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotals = new Phoenix.Windows.Forms.PLabelStandard();
            this.grdApplicationDetails = new Phoenix.Windows.Forms.PDataGridView();
            this.colDrawer = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colDebitAmt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCreditAmt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTlCaptureParentISN = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTlCaptureISN = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTlCaptureBatchID = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTlCaptureWorkstation = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colReversal = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.pbSearch = new Phoenix.Windows.Forms.PAction();
            this.gbDisplayCriteria.SuspendLayout();
            this.gbControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApplicationDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSearch});
            // 
            // gbDisplayCriteria
            // 
            this.gbDisplayCriteria.Controls.Add(this.cbIncludeRev);
            this.gbDisplayCriteria.Controls.Add(this.dfDrCrAmt);
            this.gbDisplayCriteria.Controls.Add(this.lblDrCrAmt);
            this.gbDisplayCriteria.Controls.Add(this.dfTlCaptureISN);
            this.gbDisplayCriteria.Controls.Add(this.lblTlCaptureISN);
            this.gbDisplayCriteria.Controls.Add(this.cmbTlCaptureWorkstation);
            this.gbDisplayCriteria.Controls.Add(this.lblWorkstation);
            this.gbDisplayCriteria.Controls.Add(this.lblDrawer);
            this.gbDisplayCriteria.Controls.Add(this.cmbDrawer);
            this.gbDisplayCriteria.Controls.Add(this.lblBranchNo);
            this.gbDisplayCriteria.Controls.Add(this.dfBranch);
            this.gbDisplayCriteria.Controls.Add(this.lblEffectiveDate);
            this.gbDisplayCriteria.Controls.Add(this.dfEffectiveDate);
            this.gbDisplayCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbDisplayCriteria.Name = "gbDisplayCriteria";
            this.gbDisplayCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbDisplayCriteria.Size = new System.Drawing.Size(683, 91);
            this.gbDisplayCriteria.TabIndex = 0;
            this.gbDisplayCriteria.TabStop = false;
            this.gbDisplayCriteria.Text = "Display Criteria";
            // 
            // cbIncludeRev
            // 
            this.cbIncludeRev.AutoSize = true;
            this.cbIncludeRev.BackColor = System.Drawing.SystemColors.Control;
            this.cbIncludeRev.Checked = true;
            this.cbIncludeRev.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeRev.Location = new System.Drawing.Point(560, 17);
            this.cbIncludeRev.Name = "cbIncludeRev";
            this.cbIncludeRev.PhoenixUIControl.ObjectId = 22;
            this.cbIncludeRev.PhoenixUIControl.XmlTag = "IncludeRev";
            this.cbIncludeRev.Size = new System.Drawing.Size(116, 18);
            this.cbIncludeRev.TabIndex = 4;
            this.cbIncludeRev.Text = "Include Reversed";
            this.cbIncludeRev.UseVisualStyleBackColor = false;
            this.cbIncludeRev.Value = null;
            // 
            // dfDrCrAmt
            // 
            this.dfDrCrAmt.Location = new System.Drawing.Point(407, 64);
            this.dfDrCrAmt.Name = "dfDrCrAmt";
            this.dfDrCrAmt.PhoenixUIControl.ObjectId = 21;
            this.dfDrCrAmt.PhoenixUIControl.XmlTag = "NetAmt";
            this.dfDrCrAmt.PreviousValue = null;
            this.dfDrCrAmt.Size = new System.Drawing.Size(171, 20);
            this.dfDrCrAmt.TabIndex = 12;
            // 
            // lblDrCrAmt
            // 
            this.lblDrCrAmt.AutoEllipsis = true;
            this.lblDrCrAmt.Location = new System.Drawing.Point(284, 65);
            this.lblDrCrAmt.Name = "lblDrCrAmt";
            this.lblDrCrAmt.PhoenixUIControl.ObjectId = 21;
            this.lblDrCrAmt.Size = new System.Drawing.Size(110, 20);
            this.lblDrCrAmt.TabIndex = 11;
            this.lblDrCrAmt.Text = "Debit/Credit Amount:";
            // 
            // dfTlCaptureISN
            // 
            this.dfTlCaptureISN.Location = new System.Drawing.Point(73, 64);
            this.dfTlCaptureISN.Name = "dfTlCaptureISN";
            this.dfTlCaptureISN.PhoenixUIControl.ObjectId = 20;
            this.dfTlCaptureISN.PhoenixUIControl.XmlTag = "TlCaptureISN";
            this.dfTlCaptureISN.PreviousValue = null;
            this.dfTlCaptureISN.Size = new System.Drawing.Size(173, 20);
            this.dfTlCaptureISN.TabIndex = 10;
            // 
            // lblTlCaptureISN
            // 
            this.lblTlCaptureISN.AutoEllipsis = true;
            this.lblTlCaptureISN.Location = new System.Drawing.Point(4, 64);
            this.lblTlCaptureISN.Name = "lblTlCaptureISN";
            this.lblTlCaptureISN.PhoenixUIControl.ObjectId = 20;
            this.lblTlCaptureISN.Size = new System.Drawing.Size(63, 20);
            this.lblTlCaptureISN.TabIndex = 9;
            this.lblTlCaptureISN.Text = "Item ISN#:";
            // 
            // cmbTlCaptureWorkstation
            // 
            this.cmbTlCaptureWorkstation.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbTlCaptureWorkstation.Location = new System.Drawing.Point(407, 39);
            this.cmbTlCaptureWorkstation.Name = "cmbTlCaptureWorkstation";
            this.cmbTlCaptureWorkstation.PhoenixUIControl.ObjectId = 19;
            this.cmbTlCaptureWorkstation.PhoenixUIControl.XmlTag = "TlCaptureWorkstation";
            this.cmbTlCaptureWorkstation.Size = new System.Drawing.Size(171, 21);
            this.cmbTlCaptureWorkstation.TabIndex = 8;
            this.cmbTlCaptureWorkstation.Value = null;
            this.cmbTlCaptureWorkstation.PhoenixUISelectedIndexChangedEvent += new Windows.Forms.SelectedIndexChangedEventHandler(cmbTlCaptureWorkstation_PhoenixUISelectedIndexChangedEvent);
            this.cmbTlCaptureWorkstation.Validating += new System.ComponentModel.CancelEventHandler(cmbTlCaptureWorkstation_Validating);
            // 
            // lblWorkstation
            // 
            this.lblWorkstation.AutoEllipsis = true;
            this.lblWorkstation.Location = new System.Drawing.Point(284, 40);
            this.lblWorkstation.Name = "lblWorkstation";
            this.lblWorkstation.PhoenixUIControl.ObjectId = 19;
            this.lblWorkstation.Size = new System.Drawing.Size(110, 20);
            this.lblWorkstation.TabIndex = 7;
            this.lblWorkstation.Text = "Workstation:";
            // 
            // lblDrawer
            // 
            this.lblDrawer.AutoEllipsis = true;
            this.lblDrawer.Location = new System.Drawing.Point(4, 40);
            this.lblDrawer.Name = "lblDrawer";
            this.lblDrawer.PhoenixUIControl.ObjectId = 18;
            this.lblDrawer.Size = new System.Drawing.Size(63, 20);
            this.lblDrawer.TabIndex = 5;
            this.lblDrawer.Text = "Drawer:";
            // 
            // cmbDrawer
            // 
            this.cmbDrawer.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbDrawer.Location = new System.Drawing.Point(73, 39);
            this.cmbDrawer.Name = "cmbDrawer";
            this.cmbDrawer.PhoenixUIControl.ObjectId = 18;
            this.cmbDrawer.PhoenixUIControl.XmlTag = "DrawerNo";
            this.cmbDrawer.Size = new System.Drawing.Size(173, 21);
            this.cmbDrawer.TabIndex = 6;
            this.cmbDrawer.Value = null;
            this.cmbDrawer.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbDrawer_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblBranchNo
            // 
            this.lblBranchNo.AutoEllipsis = true;
            this.lblBranchNo.Location = new System.Drawing.Point(4, 16);
            this.lblBranchNo.Name = "lblBranchNo";
            this.lblBranchNo.PhoenixUIControl.ObjectId = 2;
            this.lblBranchNo.Size = new System.Drawing.Size(63, 20);
            this.lblBranchNo.TabIndex = 0;
            this.lblBranchNo.Text = "Branch:";
            // 
            // dfBranch
            // 
            this.dfBranch.Location = new System.Drawing.Point(73, 20);
            this.dfBranch.Name = "dfBranch";
            this.dfBranch.PhoenixUIControl.ObjectId = 2;
            this.dfBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.dfBranch.PreviousValue = null;
            this.dfBranch.Size = new System.Drawing.Size(204, 13);
            this.dfBranch.TabIndex = 1;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoEllipsis = true;
            this.lblEffectiveDate.Location = new System.Drawing.Point(284, 16);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.PhoenixUIControl.ObjectId = 3;
            this.lblEffectiveDate.Size = new System.Drawing.Size(110, 20);
            this.lblEffectiveDate.TabIndex = 2;
            this.lblEffectiveDate.Text = "Date:";
            // 
            // dfEffectiveDate
            // 
            this.dfEffectiveDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.Location = new System.Drawing.Point(407, 20);
            this.dfEffectiveDate.Name = "dfEffectiveDate";
            this.dfEffectiveDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfEffectiveDate.PhoenixUIControl.ObjectId = 3;
            this.dfEffectiveDate.PreviousValue = null;
            this.dfEffectiveDate.Size = new System.Drawing.Size(100, 13);
            this.dfEffectiveDate.TabIndex = 3;
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.dfCreditAmtTot);
            this.gbControl.Controls.Add(this.dfDebitAmtTot);
            this.gbControl.Controls.Add(this.lblTotals);
            this.gbControl.Controls.Add(this.grdApplicationDetails);
            this.gbControl.Location = new System.Drawing.Point(4, 90);
            this.gbControl.Name = "gbControl";
            this.gbControl.Size = new System.Drawing.Size(683, 355);
            this.gbControl.TabIndex = 1;
            this.gbControl.TabStop = false;
            // 
            // dfCreditAmtTot
            // 
            this.dfCreditAmtTot.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCreditAmtTot.Location = new System.Drawing.Point(327, 333);
            this.dfCreditAmtTot.Name = "dfCreditAmtTot";
            this.dfCreditAmtTot.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCreditAmtTot.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCreditAmtTot.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCreditAmtTot.PhoenixUIControl.ObjectId = 5;
            this.dfCreditAmtTot.PreviousValue = null;
            this.dfCreditAmtTot.Size = new System.Drawing.Size(100, 13);
            this.dfCreditAmtTot.TabIndex = 3;
            // 
            // dfDebitAmtTot
            // 
            this.dfDebitAmtTot.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDebitAmtTot.Location = new System.Drawing.Point(224, 333);
            this.dfDebitAmtTot.Name = "dfDebitAmtTot";
            this.dfDebitAmtTot.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDebitAmtTot.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDebitAmtTot.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDebitAmtTot.PhoenixUIControl.ObjectId = 5;
            this.dfDebitAmtTot.PreviousValue = null;
            this.dfDebitAmtTot.Size = new System.Drawing.Size(100, 13);
            this.dfDebitAmtTot.TabIndex = 2;
            // 
            // lblTotals
            // 
            this.lblTotals.AutoEllipsis = true;
            this.lblTotals.Location = new System.Drawing.Point(4, 329);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.PhoenixUIControl.ObjectId = 5;
            this.lblTotals.Size = new System.Drawing.Size(100, 20);
            this.lblTotals.TabIndex = 1;
            this.lblTotals.Text = "Totals:";
            // 
            // grdApplicationDetails
            // 
            this.grdApplicationDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDrawer,
            this.colAcctType,
            this.colAcctNo,
            this.colTranCode,
            this.colDebitAmt,
            this.colCreditAmt,
            this.colCheckNo,
            this.colTlCaptureParentISN,
            this.colTlCaptureISN,
            this.colTlCaptureBatchID,
            this.colTlCaptureWorkstation,
            this.colReversal});
            this.grdApplicationDetails.IsDataGridReadOnly = false;
            this.grdApplicationDetails.IsMaxNumRowsCustomized = false;
            this.grdApplicationDetails.LinesInHeader = 2;
            this.grdApplicationDetails.Location = new System.Drawing.Point(4, 16);
            this.grdApplicationDetails.Name = "grdApplicationDetails";
            this.grdApplicationDetails.Size = new System.Drawing.Size(671, 308);
            this.grdApplicationDetails.TabIndex = 0;
            this.grdApplicationDetails.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdApplicationDetails_BeforePopulate);
            this.grdApplicationDetails.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdApplicationDetails_FetchRowDone);
            this.grdApplicationDetails.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdApplicationDetails_AfterPopulate);
            // 
            // colDrawer
            // 
            this.colDrawer.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawer.HeaderText = "Drawer#";
            this.colDrawer.Name = "colDrawer";
            this.colDrawer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawer.PhoenixUIControl.ObjectId = 6;
            this.colDrawer.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawer.Text = "";
            this.colDrawer.Title = "Drawer#";
            this.colDrawer.Width = 55;
            // 
            // colAcctType
            // 
            this.colAcctType.HeaderText = "Acct Type";
            this.colAcctType.Name = "colAcctType";
            this.colAcctType.PhoenixUIControl.ObjectId = 7;
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Text = "";
            this.colAcctType.Title = "Acct Type";
            this.colAcctType.Width = 50;
            // 
            // colAcctNo
            // 
            this.colAcctNo.HeaderText = "Acct Number";
            this.colAcctNo.Name = "colAcctNo";
            this.colAcctNo.PhoenixUIControl.ObjectId = 8;
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.Text = "";
            this.colAcctNo.Title = "Acct Number";
            this.colAcctNo.Width = 90;
            // 
            // colTranCode
            // 
            this.colTranCode.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTranCode.HeaderText = "TC";
            this.colTranCode.Name = "colTranCode";
            this.colTranCode.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTranCode.PhoenixUIControl.ObjectId = 9;
            this.colTranCode.PhoenixUIControl.XmlTag = "TranCode";
            this.colTranCode.Text = "";
            this.colTranCode.Title = "TC";
            this.colTranCode.Width = 30;
            // 
            // colDebitAmt
            // 
            this.colDebitAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDebitAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDebitAmt.HeaderText = "Debit Amount";
            this.colDebitAmt.Name = "colDebitAmt";
            this.colDebitAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDebitAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDebitAmt.PhoenixUIControl.ObjectId = 10;
            this.colDebitAmt.PhoenixUIControl.XmlTag = "DebitAmt";
            this.colDebitAmt.Text = "";
            this.colDebitAmt.Title = "Debit Amount";
            // 
            // colCreditAmt
            // 
            this.colCreditAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCreditAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCreditAmt.HeaderText = "Credit Amount";
            this.colCreditAmt.Name = "colCreditAmt";
            this.colCreditAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCreditAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCreditAmt.PhoenixUIControl.ObjectId = 11;
            this.colCreditAmt.PhoenixUIControl.XmlTag = "CreditAmt";
            this.colCreditAmt.Text = "";
            this.colCreditAmt.Title = "Credit Amount";
            // 
            // colCheckNo
            // 
            this.colCheckNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCheckNo.HeaderText = "Check#";
            this.colCheckNo.Name = "colCheckNo";
            this.colCheckNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCheckNo.PhoenixUIControl.ObjectId = 12;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.Text = "";
            this.colCheckNo.Title = "Check#";
            this.colCheckNo.Width = 80;
            // 
            // colTlCaptureParentISN
            // 
            this.colTlCaptureParentISN.HeaderText = "Parent ISN#";
            this.colTlCaptureParentISN.Name = "colTlCaptureParentISN";
            this.colTlCaptureParentISN.PhoenixUIControl.ObjectId = 13;
            this.colTlCaptureParentISN.PhoenixUIControl.XmlTag = "TlCaptureParentIsn";
            this.colTlCaptureParentISN.Text = "";
            this.colTlCaptureParentISN.Title = "Parent ISN#";
            this.colTlCaptureParentISN.Width = 80;
            // 
            // colTlCaptureISN
            // 
            this.colTlCaptureISN.HeaderText = "Item ISN#";
            this.colTlCaptureISN.Name = "colTlCaptureISN";
            this.colTlCaptureISN.PhoenixUIControl.ObjectId = 14;
            this.colTlCaptureISN.PhoenixUIControl.XmlTag = "TlCaptureIsn";
            this.colTlCaptureISN.Text = "";
            this.colTlCaptureISN.Title = "Item ISN#";
            this.colTlCaptureISN.Width = 80;
            // 
            // colTlCaptureBatchID
            // 
            this.colTlCaptureBatchID.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTlCaptureBatchID.HeaderText = "Batch ID";
            this.colTlCaptureBatchID.Name = "colTlCaptureBatchID";
            this.colTlCaptureBatchID.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTlCaptureBatchID.PhoenixUIControl.ObjectId = 15;
            this.colTlCaptureBatchID.PhoenixUIControl.XmlTag = "TlCaptureBatchId";
            this.colTlCaptureBatchID.Text = "";
            this.colTlCaptureBatchID.Title = "Batch ID";
            // 
            // colTlCaptureWorkstation
            // 
            this.colTlCaptureWorkstation.HeaderText = "Workstation";
            this.colTlCaptureWorkstation.Name = "colTlCaptureWorkstation";
            this.colTlCaptureWorkstation.PhoenixUIControl.ObjectId = 16;
            this.colTlCaptureWorkstation.PhoenixUIControl.XmlTag = "TlCaptureWorkstation";
            this.colTlCaptureWorkstation.Text = "";
            this.colTlCaptureWorkstation.Title = "Workstation";
            // 
            // colReversal
            // 
            this.colReversal.HeaderText = "Reversed";
            this.colReversal.Name = "colReversal";
            this.colReversal.PhoenixUIControl.ObjectId = 17;
            this.colReversal.PhoenixUIControl.XmlTag = "Reversed";
            this.colReversal.Text = "";
            this.colReversal.Title = "Reversed";
            // 
            // pbSearch
            // 
            this.pbSearch.LongText = "&Search";
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.ObjectId = 4;
            this.pbSearch.ShortText = "&Search";
            this.pbSearch.Tag = null;
            this.pbSearch.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSearch_Click);
            // 
            // frmTlCaptureApplDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbControl);
            this.Controls.Add(this.gbDisplayCriteria);
            this.EditRecordTitle = "Application Details";
            this.Name = "frmTlCaptureApplDetails";
            this.NewRecordTitle = "Application Details";
            this.Text = "Application Details";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCaptureApplDetails_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCaptureApplDetails_PInitCompleteEvent);
            this.gbDisplayCriteria.ResumeLayout(false);
            this.gbDisplayCriteria.PerformLayout();
            this.gbControl.ResumeLayout(false);
            this.gbControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApplicationDetails)).EndInit();
            this.ResumeLayout(false);

		}


		#endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbDisplayCriteria;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbControl;
		private Phoenix.Windows.Forms.PLabelStandard lblEffectiveDate;
		private Phoenix.Windows.Forms.PDfDisplay dfEffectiveDate;
		private Phoenix.Windows.Forms.PAction pbSearch;
        private Windows.Forms.PDataGridView grdApplicationDetails;
        private Windows.Forms.PDataGridViewColumn colDrawer;
        private Windows.Forms.PDataGridViewColumn colAcctType;
        private Windows.Forms.PDataGridViewColumn colDebitAmt;
        private Windows.Forms.PDataGridViewColumn colCreditAmt;
        private Windows.Forms.PLabelStandard lblBranchNo;
        private Windows.Forms.PDfDisplay dfBranch;
        private Windows.Forms.PDfDisplay dfCreditAmtTot;
        private Windows.Forms.PDfDisplay dfDebitAmtTot;
        private Windows.Forms.PLabelStandard lblTotals;
        private Windows.Forms.PdfCurrency dfDrCrAmt;
        private Windows.Forms.PLabelStandard lblDrCrAmt;
        private Windows.Forms.PdfStandard dfTlCaptureISN;
        private Windows.Forms.PLabelStandard lblTlCaptureISN;
        private Windows.Forms.PComboBoxStandard cmbTlCaptureWorkstation;
        private Windows.Forms.PLabelStandard lblWorkstation;
        private Windows.Forms.PLabelStandard lblDrawer;
        private Windows.Forms.PComboBoxStandard cmbDrawer;
        private Windows.Forms.PDataGridViewColumn colAcctNo;
        private Windows.Forms.PDataGridViewColumn colTranCode;
        private Windows.Forms.PDataGridViewColumn colCheckNo;
        private Windows.Forms.PDataGridViewColumn colTlCaptureParentISN;
        private Windows.Forms.PDataGridViewColumn colTlCaptureISN;
        private Windows.Forms.PDataGridViewColumn colTlCaptureBatchID;
        private Windows.Forms.PDataGridViewColumn colTlCaptureWorkstation;
        private Windows.Forms.PDataGridViewColumn colReversal;
        private Windows.Forms.PCheckBoxStandard cbIncludeRev;
	}
}