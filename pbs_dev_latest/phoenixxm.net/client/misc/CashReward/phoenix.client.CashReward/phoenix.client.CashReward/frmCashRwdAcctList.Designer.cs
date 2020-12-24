using DevExpress.Utils;

namespace phoenix.client.CashReward
{
    partial class frmCashRwdAcctList
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CashBal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcctTp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcctNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAtmAcctNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAtmAcctType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAtmAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.layoutView1 = new DevExpress.XtraGrid.Views.Layout.LayoutView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn2 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn3 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn4 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn5 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn6 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.LayoutViewColumn();
            this.layoutViewField_gridColumn7 = new DevExpress.XtraGrid.Views.Layout.LayoutViewField();
            this.layoutViewCard1 = new DevExpress.XtraGrid.Views.Layout.LayoutViewCard();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.tileViewColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tileViewColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gbCashRwdRedmption = new Phoenix.Windows.Forms.PGroupBoxStandard();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.gbCashRwdRedmption.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(3, 16);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHyperLinkEdit1});
            this.gridControl1.Size = new System.Drawing.Size(902, 511);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1,
            this.layoutView1,
            this.gridView2});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAccount,
            this.colStatus,
            this.CashBal,
            this.colAcctTp,
            this.colAcctNum,
            this.colAtmAcctNum,
            this.colAtmAcctType,
            this.colAtmAccount});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colAccount
            // 
            this.colAccount.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colAccount.AppearanceHeader.Options.UseForeColor = true;
            this.colAccount.Caption = "Account Number";
            this.colAccount.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.colAccount.FieldName = "Account";
            this.colAccount.MinWidth = 25;
            this.colAccount.Name = "colAccount";
            this.colAccount.Visible = true;
            this.colAccount.VisibleIndex = 1;
            this.colAccount.Width = 94;
            // 
            // colAtmAccount
            // 
            this.colAtmAccount.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colAtmAccount.AppearanceHeader.Options.UseForeColor = true;
            this.colAtmAccount.Caption = "ATM Card";
            this.colAtmAccount.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.colAtmAccount.FieldName = "AtmAccount";
            this.colAtmAccount.MinWidth = 25;
            this.colAtmAccount.Name = "colAccount";
            this.colAtmAccount.Visible = true;
            this.colAtmAccount.VisibleIndex = 0;
            this.colAtmAccount.Width = 94;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            this.repositoryItemHyperLinkEdit1.Click += new System.EventHandler(this.repositoryItemHyperLinkEdit1_Click);
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.MinWidth = 25;
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.AllowFocus = false;
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 1;
            this.colStatus.Width = 94;
            // 
            // CashBal
            // 
            this.CashBal.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.CashBal.AppearanceCell.Options.UseForeColor = true;
            this.CashBal.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.CashBal.AppearanceHeader.Options.UseForeColor = true;
            this.CashBal.Caption = "Available Cash Balance";
            this.CashBal.DisplayFormat.FormatString = "c2";
            this.CashBal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CashBal.FieldName = "CashRwdBal";
            this.CashBal.MinWidth = 25;
            this.CashBal.Name = "CashBal";
            this.CashBal.OptionsColumn.AllowEdit = false;
            this.CashBal.OptionsColumn.AllowFocus = false;
            this.CashBal.OptionsColumn.ReadOnly = true;
            this.CashBal.Visible = true;
            this.CashBal.VisibleIndex = 2;
            this.CashBal.Width = 94;
            // 
            // colAcctTp
            // 
            this.colAcctTp.Caption = "Acct Type";
            this.colAcctTp.FieldName = "AcctType";
            this.colAcctTp.MinWidth = 25;
            this.colAcctTp.Name = "colAcctTp";
            this.colAcctTp.OptionsColumn.ReadOnly = true;
            this.colAcctTp.Width = 94;
            // 
            // colAcctNum
            // 
            this.colAcctNum.Caption = "Acct Number";
            this.colAcctNum.FieldName = "AcctNo";
            this.colAcctNum.MinWidth = 25;
            this.colAcctNum.Name = "colAcctNum";
            this.colAcctNum.OptionsColumn.ReadOnly = true;
            this.colAcctNum.Width = 94;
            // 
            // colAtmAcctNum
            // 
            this.colAtmAcctNum.Caption = "Acct Number";
            this.colAtmAcctNum.FieldName = "AtmAcctNo";
            this.colAtmAcctNum.MinWidth = 25;
            this.colAtmAcctNum.Name = "colAtmAcctNum";
            this.colAtmAcctNum.OptionsColumn.ReadOnly = true;
            this.colAtmAcctNum.Visible = true;
            this.colAtmAcctNum.VisibleIndex = 3;
            this.colAtmAcctNum.Width = 94;
            // 
            // colAtmAcctType
            // 
            this.colAtmAcctType.Caption = "Acct Type";
            this.colAtmAcctType.FieldName = "AtmAcctType";
            this.colAtmAcctType.MinWidth = 25;
            this.colAtmAcctType.Name = "colAtmAcctType";
            this.colAtmAcctType.OptionsColumn.ReadOnly = true;
            this.colAtmAcctType.Width = 94;
            // 
            // layoutView1
            // 
            this.layoutView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.LayoutViewColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.layoutView1.GridControl = this.gridControl1;
            this.layoutView1.Name = "layoutView1";
            this.layoutView1.TemplateCard = this.layoutViewCard1;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Account";
            this.gridColumn1.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.gridColumn1.FieldName = "Account";
            this.gridColumn1.LayoutViewField = this.layoutViewField_gridColumn1;
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Width = 94;
            // 
            // layoutViewField_gridColumn1
            // 
            this.layoutViewField_gridColumn1.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewField_gridColumn1.Name = "layoutViewField_gridColumn1";
            this.layoutViewField_gridColumn1.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn1.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Relationship";
            this.gridColumn2.FieldName = "Relationship";
            this.gridColumn2.LayoutViewField = this.layoutViewField_gridColumn2;
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.ReadOnly = true;
            this.gridColumn2.Width = 94;
            // 
            // layoutViewField_gridColumn2
            // 
            this.layoutViewField_gridColumn2.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn2.Location = new System.Drawing.Point(0, 26);
            this.layoutViewField_gridColumn2.Name = "layoutViewField_gridColumn2";
            this.layoutViewField_gridColumn2.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn2.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Available Cash Balance";
            this.gridColumn3.LayoutViewField = this.layoutViewField_gridColumn3;
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.ReadOnly = true;
            this.gridColumn3.Width = 94;
            // 
            // layoutViewField_gridColumn3
            // 
            this.layoutViewField_gridColumn3.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn3.Location = new System.Drawing.Point(0, 52);
            this.layoutViewField_gridColumn3.Name = "layoutViewField_gridColumn3";
            this.layoutViewField_gridColumn3.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn3.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Minimum Required Balance to Redeem";
            this.gridColumn4.LayoutViewField = this.layoutViewField_gridColumn4;
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.ReadOnly = true;
            this.gridColumn4.Width = 94;
            // 
            // layoutViewField_gridColumn4
            // 
            this.layoutViewField_gridColumn4.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn4.Location = new System.Drawing.Point(0, 78);
            this.layoutViewField_gridColumn4.Name = "layoutViewField_gridColumn4";
            this.layoutViewField_gridColumn4.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn4.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Amount to be Redeemed";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.LayoutViewField = this.layoutViewField_gridColumn5;
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.ReadOnly = true;
            this.gridColumn5.Width = 94;
            // 
            // layoutViewField_gridColumn5
            // 
            this.layoutViewField_gridColumn5.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn5.Location = new System.Drawing.Point(0, 104);
            this.layoutViewField_gridColumn5.Name = "layoutViewField_gridColumn5";
            this.layoutViewField_gridColumn5.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn5.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Acct Type";
            this.gridColumn6.FieldName = "AcctType";
            this.gridColumn6.LayoutViewField = this.layoutViewField_gridColumn6;
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.ReadOnly = true;
            this.gridColumn6.Width = 94;
            // 
            // layoutViewField_gridColumn6
            // 
            this.layoutViewField_gridColumn6.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn6.Location = new System.Drawing.Point(0, 130);
            this.layoutViewField_gridColumn6.Name = "layoutViewField_gridColumn6";
            this.layoutViewField_gridColumn6.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn6.TextSize = new System.Drawing.Size(185, 13);
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Acct Number";
            this.gridColumn7.LayoutViewField = this.layoutViewField_gridColumn7;
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.ReadOnly = true;
            this.gridColumn7.Width = 94;
            // 
            // layoutViewField_gridColumn7
            // 
            this.layoutViewField_gridColumn7.EditorPreferredWidth = -31;
            this.layoutViewField_gridColumn7.Location = new System.Drawing.Point(0, 156);
            this.layoutViewField_gridColumn7.Name = "layoutViewField_gridColumn7";
            this.layoutViewField_gridColumn7.Size = new System.Drawing.Size(203, 26);
            this.layoutViewField_gridColumn7.TextSize = new System.Drawing.Size(185, 13);
            // 
            // layoutViewCard1
            // 
            this.layoutViewCard1.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutViewCard1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutViewField_gridColumn1,
            this.layoutViewField_gridColumn2,
            this.layoutViewField_gridColumn3,
            this.layoutViewField_gridColumn4,
            this.layoutViewField_gridColumn5,
            this.layoutViewField_gridColumn6,
            this.layoutViewField_gridColumn7});
            this.layoutViewCard1.Name = "layoutViewTemplateCard";
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.tileViewColumn1,
            this.tileViewColumn2,
            this.tileViewColumn3,
            this.tileViewColumn4,
            this.tileViewColumn5,
            this.tileViewColumn6,
            this.tileViewColumn7});
            this.gridView2.GridControl = this.gridControl1;
            this.gridView2.Name = "gridView2";
            this.gridView2.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // tileViewColumn1
            // 
            this.tileViewColumn1.Caption = "Account";
            this.tileViewColumn1.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.tileViewColumn1.FieldName = "Account";
            this.tileViewColumn1.MinWidth = 25;
            this.tileViewColumn1.Name = "tileViewColumn1";
            this.tileViewColumn1.Visible = true;
            this.tileViewColumn1.VisibleIndex = 0;
            this.tileViewColumn1.Width = 94;
            // 
            // tileViewColumn2
            // 
            this.tileViewColumn2.Caption = "Relationship";
            this.tileViewColumn2.FieldName = "Relationship";
            this.tileViewColumn2.MinWidth = 25;
            this.tileViewColumn2.Name = "tileViewColumn2";
            this.tileViewColumn2.OptionsColumn.AllowEdit = false;
            this.tileViewColumn2.OptionsColumn.ReadOnly = true;
            this.tileViewColumn2.Visible = true;
            this.tileViewColumn2.VisibleIndex = 1;
            this.tileViewColumn2.Width = 94;
            // 
            // tileViewColumn3
            // 
            this.tileViewColumn3.Caption = "Available Cash Balance";
            this.tileViewColumn3.MinWidth = 25;
            this.tileViewColumn3.Name = "tileViewColumn3";
            this.tileViewColumn3.OptionsColumn.ReadOnly = true;
            this.tileViewColumn3.Visible = true;
            this.tileViewColumn3.VisibleIndex = 2;
            this.tileViewColumn3.Width = 94;
            // 
            // tileViewColumn4
            // 
            this.tileViewColumn4.Caption = "Minimum Required Balance to Redeem";
            this.tileViewColumn4.MinWidth = 25;
            this.tileViewColumn4.Name = "tileViewColumn4";
            this.tileViewColumn4.OptionsColumn.ReadOnly = true;
            this.tileViewColumn4.Visible = true;
            this.tileViewColumn4.VisibleIndex = 3;
            this.tileViewColumn4.Width = 94;
            // 
            // tileViewColumn5
            // 
            this.tileViewColumn5.Caption = "Amount to be Redeemed";
            this.tileViewColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.tileViewColumn5.MinWidth = 25;
            this.tileViewColumn5.Name = "tileViewColumn5";
            this.tileViewColumn5.OptionsColumn.ReadOnly = true;
            this.tileViewColumn5.Visible = true;
            this.tileViewColumn5.VisibleIndex = 4;
            this.tileViewColumn5.Width = 94;
            // 
            // tileViewColumn6
            // 
            this.tileViewColumn6.Caption = "Acct Type";
            this.tileViewColumn6.FieldName = "AcctType";
            this.tileViewColumn6.MinWidth = 25;
            this.tileViewColumn6.Name = "tileViewColumn6";
            this.tileViewColumn6.OptionsColumn.ReadOnly = true;
            this.tileViewColumn6.Visible = true;
            this.tileViewColumn6.VisibleIndex = 5;
            this.tileViewColumn6.Width = 94;
            // 
            // tileViewColumn7
            // 
            this.tileViewColumn7.Caption = "Acct Number";
            this.tileViewColumn7.MinWidth = 25;
            this.tileViewColumn7.Name = "tileViewColumn7";
            this.tileViewColumn7.OptionsColumn.ReadOnly = true;
            this.tileViewColumn7.Visible = true;
            this.tileViewColumn7.VisibleIndex = 6;
            this.tileViewColumn7.Width = 94;
            // 
            // gbCashRwdRedmption
            // 
            this.gbCashRwdRedmption.Controls.Add(this.gridControl1);
            this.gbCashRwdRedmption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCashRwdRedmption.Location = new System.Drawing.Point(0, 0);
            this.gbCashRwdRedmption.Name = "gbCashRwdRedmption";
            this.gbCashRwdRedmption.PhoenixUIControl.ObjectId = 5;
            this.gbCashRwdRedmption.Size = new System.Drawing.Size(908, 530);
            this.gbCashRwdRedmption.TabIndex = 1;
            this.gbCashRwdRedmption.TabStop = false;
            this.gbCashRwdRedmption.Text = "Card List";
            // 
            // frmCashRwdAcctList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.gbCashRwdRedmption);
            this.EditRecordTitle = "Active Card List";
            this.Name = "frmCashRwdAcctList";
            this.NewRecordTitle = "Active Card List";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.Text = "Active Cards List";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmCashRwdInquiry_PInitCompleteEvent);
            this.PAfterSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmCashRwdInquiry_PAfterSave);
            this.PShowCompletedEvent += new System.EventHandler(this.frmCashRwdInquiry_PShowCompletedEvent);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewField_gridColumn7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutViewCard1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.gbCashRwdRedmption.ResumeLayout(false);
            this.ResumeLayout(false);

        }
       

        #endregion
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colAccount;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn CashBal;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctTp;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctNum;
        private DevExpress.XtraGrid.Columns.GridColumn colAtmAcctNum;
        private DevExpress.XtraGrid.Columns.GridColumn colAtmAcctType;
        private DevExpress.XtraGrid.Columns.GridColumn colAtmAccount;
        private DevExpress.XtraGrid.Views.Layout.LayoutView layoutView1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn1;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn2;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn2;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn3;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn3;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn4;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn4;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn5;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn5;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn6;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn6;
        private DevExpress.XtraGrid.Columns.LayoutViewColumn gridColumn7;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewField layoutViewField_gridColumn7;
        private DevExpress.XtraGrid.Views.Layout.LayoutViewCard layoutViewCard1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn tileViewColumn7;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbCashRwdRedmption;
    }
}