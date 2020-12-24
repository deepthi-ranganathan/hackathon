using DevExpress.Utils;

namespace phoenix.client.CashReward
{
    partial class frmCashRwdInquiry
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
            DevExpress.XtraGrid.Columns.GridColumn colRel;
            this.gbCashRwdInquiry = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblCashRewardAvailable = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblCashRewardRedeemedLtd = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashRewardAvailable = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCashRewardRedeemedLtd = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfCashRewardEarnedLtd = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashRewardEarnedLtd = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbCashRwdRedmption = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAccount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CashBal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMinRe = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRedAMt = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcctTp = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcctNum = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pbRedeem = new Phoenix.Windows.Forms.PAction();
            colRel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gbCashRwdInquiry.SuspendLayout();
            this.gbCashRwdRedmption.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbRedeem});
            // 
            // colRel
            // 
            colRel.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            colRel.AppearanceCell.Options.UseForeColor = true;
            colRel.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            colRel.AppearanceHeader.Options.UseForeColor = true;
            colRel.Caption = "Relationship";
            colRel.FieldName = "Relationship";
            colRel.MinWidth = 25;
            colRel.Name = "colRel";
            colRel.OptionsColumn.AllowEdit = false;
            colRel.OptionsColumn.AllowFocus = false;
            colRel.OptionsColumn.ReadOnly = true;
            colRel.Visible = true;
            colRel.VisibleIndex = 1;
            colRel.Width = 94;
            // 
            // gbCashRwdInquiry
            // 
            this.gbCashRwdInquiry.Controls.Add(this.lblCashRewardAvailable);
            this.gbCashRwdInquiry.Controls.Add(this.lblCashRewardRedeemedLtd);
            this.gbCashRwdInquiry.Controls.Add(this.dfCashRewardAvailable);
            this.gbCashRwdInquiry.Controls.Add(this.dfCashRewardRedeemedLtd);
            this.gbCashRwdInquiry.Controls.Add(this.dfCashRewardEarnedLtd);
            this.gbCashRwdInquiry.Controls.Add(this.lblCashRewardEarnedLtd);
            this.gbCashRwdInquiry.Location = new System.Drawing.Point(3, 0);
            this.gbCashRwdInquiry.Name = "gbCashRwdInquiry";
            this.gbCashRwdInquiry.PhoenixUIControl.ObjectId = 1;
            this.gbCashRwdInquiry.Size = new System.Drawing.Size(902, 85);
            this.gbCashRwdInquiry.TabIndex = 0;
            this.gbCashRwdInquiry.TabStop = false;
            this.gbCashRwdInquiry.Text = "Cash Reward Inquiry";
            // 
            // lblCashRewardAvailable
            // 
            this.lblCashRewardAvailable.AutoEllipsis = true;
            this.lblCashRewardAvailable.Location = new System.Drawing.Point(583, 38);
            this.lblCashRewardAvailable.Name = "lblCashRewardAvailable";
            this.lblCashRewardAvailable.PhoenixUIControl.ObjectId = 4;
            this.lblCashRewardAvailable.Size = new System.Drawing.Size(206, 16);
            this.lblCashRewardAvailable.TabIndex = 4;
            this.lblCashRewardAvailable.Text = "Cash Reward Available for Redemption: ";
            // 
            // lblCashRewardRedeemedLtd
            // 
            this.lblCashRewardRedeemedLtd.AutoEllipsis = true;
            this.lblCashRewardRedeemedLtd.Location = new System.Drawing.Point(274, 38);
            this.lblCashRewardRedeemedLtd.Name = "lblCashRewardRedeemedLtd";
            this.lblCashRewardRedeemedLtd.PhoenixUIControl.ObjectId = 3;
            this.lblCashRewardRedeemedLtd.Size = new System.Drawing.Size(159, 16);
            this.lblCashRewardRedeemedLtd.TabIndex = 2;
            this.lblCashRewardRedeemedLtd.Text = "Cash Reward Redeemed LTD:";
            // 
            // dfCashRewardAvailable
            // 
            this.dfCashRewardAvailable.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardAvailable.Location = new System.Drawing.Point(794, 40);
            this.dfCashRewardAvailable.Name = "dfCashRewardAvailable";
            this.dfCashRewardAvailable.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardAvailable.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashRewardAvailable.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashRewardAvailable.PhoenixUIControl.ObjectId = 4;
            this.dfCashRewardAvailable.PhoenixUIControl.XmlTag = "TotCashRwdBal";
            this.dfCashRewardAvailable.Size = new System.Drawing.Size(72, 13);
            this.dfCashRewardAvailable.TabIndex = 5;
            // 
            // dfCashRewardRedeemedLtd
            // 
            this.dfCashRewardRedeemedLtd.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardRedeemedLtd.Location = new System.Drawing.Point(434, 40);
            this.dfCashRewardRedeemedLtd.Name = "dfCashRewardRedeemedLtd";
            this.dfCashRewardRedeemedLtd.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardRedeemedLtd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashRewardRedeemedLtd.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashRewardRedeemedLtd.PhoenixUIControl.ObjectId = 3;
            this.dfCashRewardRedeemedLtd.PhoenixUIControl.XmlTag = "TotCashRwddRedeemLtd";
            this.dfCashRewardRedeemedLtd.Size = new System.Drawing.Size(72, 13);
            this.dfCashRewardRedeemedLtd.TabIndex = 3;
            // 
            // dfCashRewardEarnedLtd
            // 
            this.dfCashRewardEarnedLtd.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardEarnedLtd.Location = new System.Drawing.Point(148, 40);
            this.dfCashRewardEarnedLtd.Name = "dfCashRewardEarnedLtd";
            this.dfCashRewardEarnedLtd.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashRewardEarnedLtd.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCashRewardEarnedLtd.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashRewardEarnedLtd.PhoenixUIControl.ObjectId = 2;
            this.dfCashRewardEarnedLtd.PhoenixUIControl.XmlTag = "TotCashRwdEarnLtd";
            this.dfCashRewardEarnedLtd.Size = new System.Drawing.Size(72, 13);
            this.dfCashRewardEarnedLtd.TabIndex = 1;
            // 
            // lblCashRewardEarnedLtd
            // 
            this.lblCashRewardEarnedLtd.AutoEllipsis = true;
            this.lblCashRewardEarnedLtd.Location = new System.Drawing.Point(8, 38);
            this.lblCashRewardEarnedLtd.Name = "lblCashRewardEarnedLtd";
            this.lblCashRewardEarnedLtd.PhoenixUIControl.ObjectId = 2;
            this.lblCashRewardEarnedLtd.Size = new System.Drawing.Size(140, 16);
            this.lblCashRewardEarnedLtd.TabIndex = 0;
            this.lblCashRewardEarnedLtd.Text = "Cash Reward Earned LTD:";
            // 
            // gbCashRwdRedmption
            // 
            this.gbCashRwdRedmption.Controls.Add(this.gridControl1);
            this.gbCashRwdRedmption.Location = new System.Drawing.Point(3, 85);
            this.gbCashRwdRedmption.Name = "gbCashRwdRedmption";
            this.gbCashRwdRedmption.PhoenixUIControl.ObjectId = 5;
            this.gbCashRwdRedmption.Size = new System.Drawing.Size(902, 445);
            this.gbCashRwdRedmption.TabIndex = 1;
            this.gbCashRwdRedmption.TabStop = false;
            this.gbCashRwdRedmption.Text = "Cash Reward Redemption";
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(3, 18);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemHyperLinkEdit1});
            this.gridControl1.Size = new System.Drawing.Size(897, 420);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAccount,
            colRel,
            this.colStatus,
            this.CashBal,
            this.colMinRe,
            this.colRedAMt,
            this.colAcctTp,
            this.colAcctNum});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colAccount
            // 
            this.colAccount.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colAccount.AppearanceHeader.Options.UseForeColor = true;
            this.colAccount.Caption = "Account";
            this.colAccount.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.colAccount.FieldName = "Account";
            this.colAccount.MinWidth = 25;
            this.colAccount.Name = "colAccount";
            this.colAccount.Visible = true;
            this.colAccount.VisibleIndex = 0;
            this.colAccount.Width = 94;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            this.repositoryItemHyperLinkEdit1.Click += new System.EventHandler(this.repositoryItemHyperLinkEdit1_Click);
            // 
            // colStatus
            // 
            this.colStatus.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.colStatus.AppearanceCell.Options.UseForeColor = true;
            this.colStatus.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colStatus.AppearanceHeader.Options.UseForeColor = true;
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.MinWidth = 25;
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.AllowFocus = false;
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 2;
            this.colStatus.Width = 84;
            // 
            // CashBal
            // 
            this.CashBal.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.CashBal.AppearanceCell.Options.UseForeColor = true;
            this.CashBal.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.CashBal.AppearanceHeader.Options.UseForeColor = true;
            this.CashBal.Caption = "Available Cash Reward";
            this.CashBal.DisplayFormat.FormatString = "c2";
            this.CashBal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.CashBal.FieldName = "CashRwdBal";
            this.CashBal.MinWidth = 25;
            this.CashBal.Name = "CashBal";
            this.CashBal.OptionsColumn.AllowEdit = false;
            this.CashBal.OptionsColumn.AllowFocus = false;
            this.CashBal.OptionsColumn.ReadOnly = true;
            this.CashBal.Visible = true;
            this.CashBal.VisibleIndex = 3;
            this.CashBal.Width = 94;
            // 
            // colMinRe
            // 
            this.colMinRe.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.colMinRe.AppearanceCell.Options.UseForeColor = true;
            this.colMinRe.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colMinRe.AppearanceHeader.Options.UseForeColor = true;
            this.colMinRe.Caption = "Minimum Required Balance to Redeem";
            this.colMinRe.DisplayFormat.FormatString = "c2";
            this.colMinRe.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMinRe.FieldName = "MinRewardAmt";
            this.colMinRe.MinWidth = 25;
            this.colMinRe.Name = "colMinRe";
            this.colMinRe.OptionsColumn.AllowEdit = false;
            this.colMinRe.OptionsColumn.AllowFocus = false;
            this.colMinRe.OptionsColumn.ReadOnly = true;
            this.colMinRe.Visible = true;
            this.colMinRe.VisibleIndex = 4;
            this.colMinRe.Width = 130;
            // 
            // colRedAMt
            // 
            this.colRedAMt.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.colRedAMt.AppearanceCell.Options.UseForeColor = true;
            this.colRedAMt.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colRedAMt.AppearanceHeader.Options.UseForeColor = true;
            this.colRedAMt.Caption = "Amount to be Redeemed";
            this.colRedAMt.DisplayFormat.FormatString = "c2";
            this.colRedAMt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRedAMt.FieldName = "AmtRedeemed";
            this.colRedAMt.MinWidth = 25;
            this.colRedAMt.Name = "colRedAMt";
            this.colRedAMt.Visible = true;
            this.colRedAMt.VisibleIndex = 5;
            this.colRedAMt.Width = 94;
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
            // pbRedeem
            // 
            this.pbRedeem.LongText = "Redeem";
            this.pbRedeem.Name = "pbRedeem";
            this.pbRedeem.ShortText = "Redeem";
            this.pbRedeem.Tag = null;
            this.pbRedeem.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.PbRedeem_Click);
            // 
            // frmCashRwdInquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.gbCashRwdRedmption);
            this.Controls.Add(this.gbCashRwdInquiry);
            this.EditRecordTitle = "Cash Reward Inquiry and Redemption";
            this.Name = "frmCashRwdInquiry";
            this.NewRecordTitle = "Cash Reward Inquiry and Redemption";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.ScreenId = 3750;
            this.Text = "Cash Reward Inquiry and Redemption";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmCashRwdInquiry_PInitCompleteEvent);
            this.PShowCompletedEvent += new System.EventHandler(this.frmCashRwdInquiry_PShowCompletedEvent);
            this.gbCashRwdInquiry.ResumeLayout(false);
            this.gbCashRwdInquiry.PerformLayout();
            this.gbCashRwdRedmption.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            this.ResumeLayout(false);

        }
       

        #endregion
        private Phoenix.Windows.Forms.PGroupBoxStandard gbCashRwdInquiry;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbCashRwdRedmption;
        private Phoenix.Windows.Forms.PDfDisplay dfCashRewardAvailable;
        private Phoenix.Windows.Forms.PDfDisplay dfCashRewardRedeemedLtd;
        private Phoenix.Windows.Forms.PDfDisplay dfCashRewardEarnedLtd;
        private Phoenix.Windows.Forms.PLabelStandard lblCashRewardEarnedLtd;
        private Phoenix.Windows.Forms.PLabelStandard lblCashRewardRedeemedLtd;
        private Phoenix.Windows.Forms.PLabelStandard lblCashRewardAvailable;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colAccount;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn CashBal;
        private DevExpress.XtraGrid.Columns.GridColumn colMinRe;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctTp;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctNum;
        private DevExpress.XtraGrid.Columns.GridColumn colRedAMt;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private Phoenix.Windows.Forms.PAction pbRedeem;
    }
}