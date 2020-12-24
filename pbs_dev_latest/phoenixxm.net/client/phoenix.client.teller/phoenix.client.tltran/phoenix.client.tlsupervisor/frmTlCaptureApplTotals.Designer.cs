namespace Phoenix.Client.Teller
{
	partial class frmTlCaptureApplTotals
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
            this.lblBranchNo = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbBranch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblEffectiveDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfEffectiveDate = new Phoenix.Windows.Forms.PdfStandard();
            this.gbControl = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCreditAmtTot = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfDebitAmtTot = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotals = new Phoenix.Windows.Forms.PLabelStandard();
            this.grdApplicationTotals = new Phoenix.Windows.Forms.PDataGridView();
            this.colApplType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colDrItemCount = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colDebitAmt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCrItemCount = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCreditAmt = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.pbSearch = new Phoenix.Windows.Forms.PAction();
            this.pbApplDetails = new Phoenix.Windows.Forms.PAction();
            this.colRevItemsFound = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.dfRevItemsLegend = new Phoenix.Windows.Forms.PDfDisplay();
            this.colApplTypeDesc = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.gbDisplayCriteria.SuspendLayout();
            this.gbControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApplicationTotals)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSearch,
            this.pbApplDetails});
            // 
            // gbDisplayCriteria
            // 
            this.gbDisplayCriteria.Controls.Add(this.lblBranchNo);
            this.gbDisplayCriteria.Controls.Add(this.cmbBranch);
            this.gbDisplayCriteria.Controls.Add(this.lblEffectiveDate);
            this.gbDisplayCriteria.Controls.Add(this.dfEffectiveDate);
            this.gbDisplayCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbDisplayCriteria.Name = "gbDisplayCriteria";
            this.gbDisplayCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbDisplayCriteria.Size = new System.Drawing.Size(683, 44);
            this.gbDisplayCriteria.TabIndex = 0;
            this.gbDisplayCriteria.TabStop = false;
            this.gbDisplayCriteria.Text = "Display Criteria";
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
            // cmbBranch
            // 
            this.cmbBranch.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbBranch.Location = new System.Drawing.Point(77, 16);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.PhoenixUIControl.ObjectId = 2;
            this.cmbBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.cmbBranch.Size = new System.Drawing.Size(204, 21);
            this.cmbBranch.TabIndex = 1;
            this.cmbBranch.Value = null;
            this.cmbBranch.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbBranch_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.AutoEllipsis = true;
            this.lblEffectiveDate.Location = new System.Drawing.Point(530, 16);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.PhoenixUIControl.ObjectId = 3;
            this.lblEffectiveDate.Size = new System.Drawing.Size(43, 20);
            this.lblEffectiveDate.TabIndex = 2;
            this.lblEffectiveDate.Text = "Date:";
            // 
            // dfEffectiveDate
            // 
            this.dfEffectiveDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.Location = new System.Drawing.Point(579, 16);
            this.dfEffectiveDate.Name = "dfEffectiveDate";
            this.dfEffectiveDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfEffectiveDate.PhoenixUIControl.ObjectId = 3;
            this.dfEffectiveDate.PreviousValue = null;
            this.dfEffectiveDate.Size = new System.Drawing.Size(100, 20);
            this.dfEffectiveDate.TabIndex = 3;
            this.dfEffectiveDate.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfEffectiveDate_PhoenixUIValidateEvent);
            // 
            // gbControl
            // 
            this.gbControl.Controls.Add(this.dfRevItemsLegend);
            this.gbControl.Controls.Add(this.dfCreditAmtTot);
            this.gbControl.Controls.Add(this.dfDebitAmtTot);
            this.gbControl.Controls.Add(this.lblTotals);
            this.gbControl.Controls.Add(this.grdApplicationTotals);
            this.gbControl.Location = new System.Drawing.Point(4, 43);
            this.gbControl.Name = "gbControl";
            this.gbControl.Size = new System.Drawing.Size(683, 401);
            this.gbControl.TabIndex = 1;
            this.gbControl.TabStop = false;
            // 
            // dfCreditAmtTot
            // 
            this.dfCreditAmtTot.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCreditAmtTot.Location = new System.Drawing.Point(570, 378);
            this.dfCreditAmtTot.Name = "dfCreditAmtTot";
            this.dfCreditAmtTot.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCreditAmtTot.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCreditAmtTot.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCreditAmtTot.PhoenixUIControl.ObjectId = 11;
            this.dfCreditAmtTot.PreviousValue = null;
            this.dfCreditAmtTot.Size = new System.Drawing.Size(100, 13);
            this.dfCreditAmtTot.TabIndex = 4;
            // 
            // dfDebitAmtTot
            // 
            this.dfDebitAmtTot.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDebitAmtTot.Location = new System.Drawing.Point(372, 378);
            this.dfDebitAmtTot.Name = "dfDebitAmtTot";
            this.dfDebitAmtTot.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDebitAmtTot.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDebitAmtTot.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDebitAmtTot.PhoenixUIControl.ObjectId = 11;
            this.dfDebitAmtTot.PreviousValue = null;
            this.dfDebitAmtTot.Size = new System.Drawing.Size(100, 13);
            this.dfDebitAmtTot.TabIndex = 3;
            // 
            // lblTotals
            // 
            this.lblTotals.AutoEllipsis = true;
            this.lblTotals.Location = new System.Drawing.Point(6, 374);
            this.lblTotals.Name = "lblTotals";
            this.lblTotals.PhoenixUIControl.ObjectId = 11;
            this.lblTotals.Size = new System.Drawing.Size(100, 20);
            this.lblTotals.TabIndex = 2;
            this.lblTotals.Text = "Totals:";
            // 
            // grdApplicationTotals
            // 
            this.grdApplicationTotals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colApplTypeDesc,
            this.colDrItemCount,
            this.colDebitAmt,
            this.colCrItemCount,
            this.colCreditAmt,
            this.colRevItemsFound,
            this.colApplType});
            this.grdApplicationTotals.IsDataGridReadOnly = false;
            this.grdApplicationTotals.IsMaxNumRowsCustomized = false;
            this.grdApplicationTotals.LinesInHeader = 2;
            this.grdApplicationTotals.Location = new System.Drawing.Point(4, 16);
            this.grdApplicationTotals.Name = "grdApplicationTotals";
            this.grdApplicationTotals.Size = new System.Drawing.Size(671, 339);
            this.grdApplicationTotals.TabIndex = 0;
            this.grdApplicationTotals.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdApplicationTotals_BeforePopulate);
            this.grdApplicationTotals.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdApplicationTotals_FetchRowDone);
            this.grdApplicationTotals.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdApplicationTotals_AfterPopulate);
            this.grdApplicationTotals.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdApplicationTotals_SelectedIndexChanged);
            // 
            // colApplType
            // 
            this.colApplType.HeaderText = "Appl Type";
            this.colApplType.Name = "colApplType";
            this.colApplType.PhoenixUIControl.ObjectId = 5;
            this.colApplType.PhoenixUIControl.XmlTag = "";
            this.colApplType.Text = "";
            this.colApplType.Title = "Appl Type";
            this.colApplType.Visible = false;
            // 
            // colDrItemCount
            // 
            this.colDrItemCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrItemCount.HeaderText = "Debit Count";
            this.colDrItemCount.Name = "colDrItemCount";
            this.colDrItemCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrItemCount.PhoenixUIControl.ObjectId = 6;
            this.colDrItemCount.PhoenixUIControl.XmlTag = "DrItemCount";
            this.colDrItemCount.Text = "";
            this.colDrItemCount.Title = "Debit Count";
            // 
            // colDebitAmt
            // 
            this.colDebitAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDebitAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDebitAmt.HeaderText = "Debit Amount";
            this.colDebitAmt.Name = "colDebitAmt";
            this.colDebitAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDebitAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDebitAmt.PhoenixUIControl.ObjectId = 7;
            this.colDebitAmt.PhoenixUIControl.XmlTag = "DebitAmt";
            this.colDebitAmt.Text = "";
            this.colDebitAmt.Title = "Debit Amount";
            // 
            // colCrItemCount
            // 
            this.colCrItemCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCrItemCount.HeaderText = "Credit Count";
            this.colCrItemCount.Name = "colCrItemCount";
            this.colCrItemCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCrItemCount.PhoenixUIControl.ObjectId = 8;
            this.colCrItemCount.PhoenixUIControl.XmlTag = "CrItemCount";
            this.colCrItemCount.Text = "";
            this.colCrItemCount.Title = "Credit Count";
            // 
            // colCreditAmt
            // 
            this.colCreditAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCreditAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCreditAmt.HeaderText = "Credit Amount";
            this.colCreditAmt.Name = "colCreditAmt";
            this.colCreditAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCreditAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCreditAmt.PhoenixUIControl.ObjectId = 9;
            this.colCreditAmt.PhoenixUIControl.XmlTag = "CreditAmt";
            this.colCreditAmt.Text = "";
            this.colCreditAmt.Title = "Credit Amount";
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
            // pbApplDetails
            // 
            this.pbApplDetails.LongText = "&Appl Details...";
            this.pbApplDetails.Name = "pbApplDetails";
            this.pbApplDetails.ObjectId = 10;
            this.pbApplDetails.ShortText = "&Appl Details...";
            this.pbApplDetails.Tag = null;
            this.pbApplDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbApplDetails_Click);
            // 
            // colRevItemsFound
            // 
            this.colRevItemsFound.HeaderText = "Rev Items Found";
            this.colRevItemsFound.Name = "colRevItemsFound";
            this.colRevItemsFound.PhoenixUIControl.XmlTag = "RevItemsFound";
            this.colRevItemsFound.Text = "";
            this.colRevItemsFound.Title = "Rev Items Found";
            this.colRevItemsFound.Visible = false;
            // 
            // dfRevItemsLegend
            // 
            this.dfRevItemsLegend.Location = new System.Drawing.Point(4, 361);
            this.dfRevItemsLegend.Name = "dfRevItemsLegend";
            this.dfRevItemsLegend.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRevItemsLegend.PreviousValue = null;
            this.dfRevItemsLegend.Size = new System.Drawing.Size(432, 13);
            this.dfRevItemsLegend.TabIndex = 1;
            // 
            // colApplTypeDesc
            // 
            this.colApplTypeDesc.HeaderText = "Appl Type";
            this.colApplTypeDesc.Name = "colApplTypeDesc";
            this.colApplTypeDesc.PhoenixUIControl.ObjectId = 5;
            this.colApplTypeDesc.PhoenixUIControl.XmlTag = "ApplTypeDesc";
            this.colApplTypeDesc.Text = "";
            this.colApplTypeDesc.Title = "Appl Type";
            this.colApplTypeDesc.Width = 267;
            // 
            // frmTlCaptureApplTotals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbControl);
            this.Controls.Add(this.gbDisplayCriteria);
            this.EditRecordTitle = "Application Totals";
            this.Name = "frmTlCaptureApplTotals";
            this.NewRecordTitle = "Application Totals";
            this.Text = "Application Totals";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCaptureApplTotals_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCaptureApplTotals_PInitCompleteEvent);
            this.gbDisplayCriteria.ResumeLayout(false);
            this.gbDisplayCriteria.PerformLayout();
            this.gbControl.ResumeLayout(false);
            this.gbControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdApplicationTotals)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbDisplayCriteria;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbControl;
		private Phoenix.Windows.Forms.PLabelStandard lblEffectiveDate;
		private Phoenix.Windows.Forms.PdfStandard dfEffectiveDate;
		private Phoenix.Windows.Forms.PAction pbSearch;
        private Windows.Forms.PDataGridView grdApplicationTotals;
        private Windows.Forms.PDataGridViewColumn colApplType;
        private Windows.Forms.PDataGridViewColumn colDrItemCount;
        private Windows.Forms.PDataGridViewColumn colDebitAmt;
        private Windows.Forms.PDataGridViewColumn colCrItemCount;
        private Windows.Forms.PDataGridViewColumn colCreditAmt;
        private Windows.Forms.PLabelStandard lblBranchNo;
        private Windows.Forms.PComboBoxStandard cmbBranch;
        private Windows.Forms.PAction pbApplDetails;
        private Windows.Forms.PDfDisplay dfCreditAmtTot;
        private Windows.Forms.PDfDisplay dfDebitAmtTot;
        private Windows.Forms.PLabelStandard lblTotals;
        private Windows.Forms.PDataGridViewColumn colRevItemsFound;
        private Windows.Forms.PDfDisplay dfRevItemsLegend;
        private Windows.Forms.PDataGridViewColumn colApplTypeDesc;
	}
}