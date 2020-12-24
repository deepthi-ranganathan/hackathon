using System;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using Phoenix.Windows.Forms;

namespace Phoenix.Client.Teller
{
    partial class ucTransactions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTransactions));
            this.gcTransactions = new Phoenix.Windows.Forms.PDxGridControl();
            this.gvTransactions = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.gridBandTransaction = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColTran = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemTC = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItemAcctType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();  //Task# 90007
            this.gridColAccount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repItemAccounts = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColAmount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColTfrAcctType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repItemTfrAcctType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColOptions = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColTfrAmt = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColOffsetAccount = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repItemTfrAccounts = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColTranDef = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBandMiscellaneous = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColCcAmt = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColCheckNo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColRefField = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColHoldBal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
            this.gridColDesc = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBandDescriptions = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColPassToHistory = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemPassToHistory = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColRegD = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemRegD = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColMenuIndicator = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repItemMenuIndicator = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.gridColActType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColAcctNo = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repositoryItemAmount = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.repItemOptions = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.dfErrorInfo = new System.Windows.Forms.TextBox();
            this.ucCashOut1 = new Phoenix.Client.Teller.ucCashOut();
            this.imgTransactions = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gcTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTransactions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemAcctType)).BeginInit(); //Task# 90007 
            ((System.ComponentModel.ISupportInitialize)(this.repItemAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTfrAcctType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTfrAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPassToHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRegD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMenuIndicator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemOptions)).BeginInit();
            this.SuspendLayout();
            // 
            // gcTransactions
            // 
            this.gcTransactions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetBoundPropertyName(this.gcTransactions, "");
            this.gcTransactions.ColumnHeadingLines = 1;
            // 
            // 
            // 
            this.gcTransactions.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gcTransactions.GenerateHiddenColumns = false;
            this.gcTransactions.IsResizable = false;
            gridLevelNode1.RelationName = "Level1";
            gridLevelNode2.RelationName = "Level2";
            this.gcTransactions.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1,
            gridLevelNode2});
            this.gcTransactions.Location = new System.Drawing.Point(0, 0);
            this.gcTransactions.MainView = this.gvTransactions;
            this.gcTransactions.Margin = new System.Windows.Forms.Padding(2);
            this.gcTransactions.MaxNumberOfRowsToFetch = 5000;
            this.gcTransactions.Name = "gcTransactions";
            this.gcTransactions.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTC, 
            this.repItemAcctType, //Task# 90007
            this.repItemAccounts,
            this.repItemTfrAccounts,
            this.repositoryItemPassToHistory,
            this.repItemMenuIndicator,
            this.repositoryItemRegD,
            this.repositoryItemHyperLinkEdit1,
            this.repositoryItemAmount,
            this.repItemTfrAcctType,
            this.repItemOptions});
            this.gcTransactions.Size = new System.Drawing.Size(978, 214);
            this.gcTransactions.TabIndex = 8;
            this.gcTransactions.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvTransactions});
            this.gcTransactions.ProcessGridKey += new System.Windows.Forms.KeyEventHandler(this.gcTransactions_ProcessGridKey);
            // 
            // gvTransactions
            // 
            this.gvTransactions.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBandTransaction,
            this.gridBandMiscellaneous,
            this.gridBandDescriptions
            });
            this.gvTransactions.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gridColTran,
            this.gridColMenuIndicator,
            this.gridColAccount,
            this.gridColAmount,
            this.gridColTfrAcctType,
            this.gridColOffsetAccount,
            this.gridColActType,
            this.gridColAcctNo,
            this.gridColTranDef,
            this.gridColRefField,
            this.gridColCcAmt,
            this.gridColHoldBal,
            this.gridColCheckNo,
            this.gridColRegD,
            this.gridColPassToHistory,
            this.gridColDesc,
            this.gridColOptions,
            this.gridColTfrAmt
            });
            this.gvTransactions.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFullFocus;
            this.gvTransactions.GridControl = this.gcTransactions;
            this.gvTransactions.Name = "gvTransactions";
            this.gvTransactions.OptionsNavigation.AutoFocusNewRow = true;
            this.gvTransactions.OptionsView.ColumnAutoWidth = true;
            this.gvTransactions.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gvTransactions.OptionsView.ShowBands = false;
            this.gvTransactions.OptionsView.ShowGroupPanel = false;
            this.gvTransactions.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.gvTransactions_CustomDrawRowIndicator);
            this.gvTransactions.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvTransactions_CustomDrawCell);
            // 
            // gridBandTransaction
            // 
            this.gridBandTransaction.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridBandTransaction.AppearanceHeader.Options.UseFont = true;
            this.gridBandTransaction.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBandTransaction.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBandTransaction.Caption = "Transaction Info";
            this.gridBandTransaction.Columns.Add(this.gridColTran);
            this.gridBandTransaction.Columns.Add(this.gridColMenuIndicator);
            this.gridBandTransaction.Columns.Add(this.gridColAccount);
            this.gridBandTransaction.Columns.Add(this.gridColAmount);
            this.gridBandTransaction.Columns.Add(this.gridColTfrAcctType);
            this.gridBandTransaction.Columns.Add(this.gridColOptions);
            this.gridBandTransaction.Columns.Add(this.gridColTfrAmt);
            this.gridBandTransaction.Columns.Add(this.gridColOffsetAccount);
            this.gridBandTransaction.Columns.Add(this.gridColTranDef);
            this.gridBandTransaction.Name = "gridBandTransaction";
            this.gridBandTransaction.VisibleIndex = 0;
            this.gridBandTransaction.Width = 330;
            // 
            // gridColTran
            // 
            this.gridColTran.AutoFillDown = true;
            this.gridColTran.Caption = "Tran Type";
            this.gridColTran.ColumnEdit = this.repositoryItemTC;
            this.gridColTran.FieldName = "TcDescription";
            this.gridColTran.Name = "gridColTran";
            this.gridColTran.Visible = true;
            this.gridColTran.Width = 63;
            // 
            // repositoryItemTC
            // 
            this.repositoryItemTC.AutoHeight = false;
            this.repositoryItemTC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemTC.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("MenuDisplayDescription", "TranType"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("TranCode", "Trancode", 40, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Description", "Description", 80, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Combined", "Combined", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.repositoryItemTC.DisplayMember = "TranType";
            this.repositoryItemTC.Name = "repositoryItemTC";
            this.repositoryItemTC.NullText = "";
            this.repositoryItemTC.ValueMember = "TranType";
            this.repositoryItemTC.EditValueChanged += new System.EventHandler(this.repItemTC_EditValueChanged);
            this.repositoryItemTC.Enter += new System.EventHandler(this.RepositoryItemTC_Enter);
            this.repositoryItemTC.UseCtrlScroll = false;
            this.repositoryItemTC.SearchMode = SearchMode.AutoComplete; //Bug #115230

            // 
            // gridColActType
            // 
            this.gridColActType.AutoFillDown = true;
            this.gridColActType.Caption = "Acct Type";
            this.gridColActType.FieldName = "AcctType";
            this.gridColActType.Name = "gridColActType";
            this.gridColActType.OptionsColumn.ShowInCustomizationForm = false;
            this.gridColActType.Width = 74;
            // 
            // repItemAcctType
            //
            this.repItemAcctType.AutoHeight = false;
            this.repItemAcctType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemAcctType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AcctType", "AcctType")});
            this.repItemAcctType.DisplayMember = "AcctType";
            this.repItemAcctType.Name = "repItemAcctType";
            this.repItemAcctType.NullText = "";
            this.repItemAcctType.ValueMember = "AcctType";
            this.repItemAcctType.UseCtrlScroll = false;
            this.repItemAcctType.SearchMode = SearchMode.AutoComplete; //Bug #115230

            // 
            // gridColAccount
            // 
            this.gridColAccount.AutoFillDown = true;
            this.gridColAccount.Caption = "Account";
            this.gridColAccount.ColumnEdit = this.repItemAccounts;
            this.gridColAccount.FieldName = "Account";
            this.gridColAccount.Name = "gridColAccount";
            this.gridColAccount.Visible = true;
            this.gridColAccount.Width = 68;

            // 
            // repItemAccounts
            // 
            this.repItemAccounts.AutoHeight = false;
            this.repItemAccounts.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemAccounts.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AccountCombined", "Account", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CurBalanceAmt", "Current", 100, DevExpress.Utils.FormatType.Numeric, "c2", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Status", "Status",100)});
            this.repItemAccounts.Name = "repItemAccounts";
            this.repItemAccounts.NullText = "";
            this.repItemAccounts.EditValueChanged += new System.EventHandler(this.repItemAccounts_EditValueChanged);
            this.repItemAccounts.PopupWidth = 350;
            this.repItemAccounts.BestFitRowCount = 15;
            this.repItemAccounts.AutoHeight = false;
            this.repItemAccounts.UseCtrlScroll = false;
            this.repItemAccounts.SearchMode = SearchMode.AutoComplete; //Bug #115230

            // 
            // gridColAmount
            // 
            this.gridColAmount.AutoFillDown = true;
            this.gridColAmount.Caption = "Amount";
            this.gridColAmount.DisplayFormat.FormatString = "c2";
            this.gridColAmount.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColAmount.FieldName = "Amount";
            this.gridColAmount.Name = "gridColAmount";
            this.gridColAmount.Visible = true;
            this.gridColAmount.Width = 52;

            // 
            // gridColTfrAcctType
            // 
            this.gridColTfrAcctType.AutoFillDown = true;
            this.gridColTfrAcctType.Caption = "Tfr Acct Type";
            this.gridColTfrAcctType.ColumnEdit = this.repItemTfrAcctType;
            this.gridColTfrAcctType.FieldName = "TfrAcctType";
            this.gridColTfrAcctType.Name = "gridColTfrAcctType";
            this.gridColTfrAcctType.Visible = true;
            this.gridColTfrAcctType.Width = 45;
           
            // 
            // repItemTfrAcctType
            // 
            this.repItemTfrAcctType.AutoHeight = false;
            this.repItemTfrAcctType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemTfrAcctType.Name = "repItemTfrAcctType";
            this.repItemTfrAcctType.NullText = "";
            this.repItemTfrAcctType.EditValueChanged += repItemTfrAcctType_EditValueChanged;
            this.repItemTfrAcctType.CloseUpKey = new DevExpress.Utils.KeyShortcut(Keys.Space);

            // 
            // gridColOptions
            // 
            this.gridColOptions.AutoFillDown = true;
            this.gridColOptions.Caption = "Options";
            this.gridColOptions.FieldName = "Options";
            this.gridColOptions.Name = "gridColOptions";
            this.gridColOptions.Width = 68;
            // 
            // gridColTfrAmt
            // 
            this.gridColTfrAmt.AutoFillDown = true;
            this.gridColTfrAmt.Caption = "Tfr Amount";
            this.gridColTfrAmt.DisplayFormat.FormatString = "c2";
            this.gridColTfrAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColTfrAmt.FieldName = "TfrAmount";
            this.gridColTfrAmt.Name = "gridColTfrAmt";
            this.gridColTfrAmt.OptionsColumn.ShowInCustomizationForm = false;
            this.gridColTfrAmt.Width = 62;
            // 
            // gridColOffsetAccount
            // 
            this.gridColOffsetAccount.AutoFillDown = true;
            this.gridColOffsetAccount.Caption = "Tfr Acct No";
            this.gridColOffsetAccount.FieldName = "TfrAcctNo";
            this.gridColOffsetAccount.Name = "gridColOffsetAccount";
            this.gridColOffsetAccount.Visible = true;
            this.gridColOffsetAccount.Width = 52;
            
            // 
            // repItemTfrAccounts
            // 
            this.repItemTfrAccounts.AutoHeight = false;
            this.repItemTfrAccounts.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemTfrAccounts.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("AccountCombined", "Account", 150, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CurBalanceAmt", "Current", 100, DevExpress.Utils.FormatType.Numeric, "c2", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Status", "Status",100)});
            this.repItemTfrAccounts.DisplayMember = "AcctNo";
            this.repItemTfrAccounts.Name = "repItemTfrAccounts";
            this.repItemTfrAccounts.NullText = "";
            this.repItemTfrAccounts.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repItemTfrAccounts.ValueMember = "AcctNo";  
            this.repItemTfrAccounts.EditValueChanged += new System.EventHandler(this.repItemTfrAccounts_EditValueChanged);
            this.repItemTfrAccounts.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup; //108811
            this.repItemTfrAccounts.PopupWidth = 350;
            this.repItemTfrAccounts.AutoHeight = false;
            this.repItemTfrAccounts.UseCtrlScroll = false;
            this.repItemTfrAccounts.SearchMode = SearchMode.AutoComplete; //Bug #115230

            // 
            // gridColTranDef
            // 
            this.gridColTranDef.Caption = "TranDef";
            this.gridColTranDef.FieldName = "TranDef";
            this.gridColTranDef.Name = "gridColTranDef";
            this.gridColTranDef.OptionsColumn.ShowInCustomizationForm = false;
            this.gridColTranDef.RowIndex = 1;
            this.gridColTranDef.Width = 86;
            // 
            // gridBandMiscellaneous
            // 
            this.gridBandMiscellaneous.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridBandMiscellaneous.AppearanceHeader.Options.UseFont = true;
            this.gridBandMiscellaneous.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBandMiscellaneous.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBandMiscellaneous.Caption = "Miscellaneous Info";
            this.gridBandMiscellaneous.Columns.Add(this.gridColCcAmt);
            this.gridBandMiscellaneous.Columns.Add(this.gridColCheckNo);
            this.gridBandMiscellaneous.Columns.Add(this.gridColRefField);
            this.gridBandMiscellaneous.Columns.Add(this.gridColHoldBal);
            this.gridBandMiscellaneous.Columns.Add(this.gridColDesc);
            this.gridBandMiscellaneous.Name = "gridBandMiscellaneous";
            this.gridBandMiscellaneous.VisibleIndex = 1;
            this.gridBandMiscellaneous.Width = 360;
            // 
            // gridColCcAmt
            // 
            this.gridColCcAmt.Caption = "Charges";
            this.gridColCcAmt.DisplayFormat.FormatString = "c2";
            this.gridColCcAmt.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColCcAmt.FieldName = "CcAmt";
            this.gridColCcAmt.Name = "gridColCcAmt";
            //this.gridColCcAmt.OptionsColumn.AllowEdit = false; //121714-1AB&2A
            this.gridColCcAmt.OptionsColumn.ReadOnly = true;
            this.gridColCcAmt.Visible = true;
            this.gridColCcAmt.Width = 48;
            // 
            // gridColCheckNo
            // 
            this.gridColCheckNo.Caption = "Check #";
            this.gridColCheckNo.FieldName = "CheckNo";
            this.gridColCheckNo.Name = "gridColCheckNo";
            this.gridColCheckNo.Visible = true;
            this.gridColCheckNo.Width = 80;
            // 
            // gridColRefField
            // 
            this.gridColRefField.Caption = "Reference Field";
            this.gridColRefField.FieldName = "Reference";
            this.gridColRefField.Name = "gridColRefField";
            this.gridColRefField.Visible = true;
            this.gridColRefField.Width = 92;
            // 
            // gridColHoldBal
            // 
            this.gridColHoldBal.Caption = "Hold Bal";
            this.gridColHoldBal.ColumnEdit = this.repositoryItemHyperLinkEdit1;
            this.gridColHoldBal.DisplayFormat.FormatString = "c2";
            this.gridColHoldBal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColHoldBal.FieldName = "HoldBal";
            this.gridColHoldBal.Name = "gridColHoldBal";
            this.gridColHoldBal.RowIndex = 1;
            this.gridColHoldBal.Visible = true;
            this.gridColHoldBal.Width = 48;
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            this.repositoryItemHyperLinkEdit1.Click += new System.EventHandler(this.repositoryItemHyperLinkEdit1_Click);
            this.repositoryItemHyperLinkEdit1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.repositoryItemHyperLinkEdit1_KeyPress);
            // 
            // gridColDesc
            // 
            this.gridColDesc.AutoFillDown = true;
            this.gridColDesc.Caption = "Description";
            this.gridColDesc.FieldName = "Description";
            this.gridColDesc.Name = "gridColDesc";
            this.gridColDesc.RowIndex = 1;
            this.gridColDesc.Visible = true;
            this.gridColDesc.Width = 172;
            // 
            // gridBandDescriptions
            // 
            this.gridBandDescriptions.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridBandDescriptions.AppearanceHeader.Options.UseFont = true;
            this.gridBandDescriptions.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBandDescriptions.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBandDescriptions.Caption = "Description Info";
            this.gridBandDescriptions.Columns.Add(this.gridColPassToHistory);
            this.gridBandDescriptions.Columns.Add(this.gridColRegD);
            this.gridBandDescriptions.Name = "gridBandDescriptions";
            this.gridBandDescriptions.VisibleIndex = 2;
            this.gridBandDescriptions.Width = 130;
            // 
            // gridColPassToHistory
            // 
            this.gridColPassToHistory.AutoFillDown = true;
            this.gridColPassToHistory.Caption = "Pass to History";
            this.gridColPassToHistory.ColumnEdit = this.repositoryItemPassToHistory;
            this.gridColPassToHistory.FieldName = "PassDescriptionToHistory";
            this.gridColPassToHistory.Name = "gridColPassToHistory";
            this.gridColPassToHistory.RowIndex = 1;
            this.gridColPassToHistory.Visible = true;
            this.gridColPassToHistory.Width = 50;
            // 
            // repositoryItemPassToHistory
            // 
            this.repositoryItemPassToHistory.AutoHeight = false;
            this.repositoryItemPassToHistory.Name = "repositoryItemPassToHistory";
            this.repositoryItemPassToHistory.RadioGroupIndex = 1;
            // 
            // gridColRegD
            // 
            this.gridColRegD.Caption = "Reg D";
            this.gridColRegD.ColumnEdit = this.repositoryItemRegD;
            this.gridColRegD.FieldName = "RegD";
            this.gridColRegD.Name = "gridColRegD";
            this.gridColRegD.Visible = true;
            this.gridColRegD.Width = 50;
            // 
            // repositoryItemRegD
            // 
            this.repositoryItemRegD.AutoHeight = false;
            this.repositoryItemRegD.Name = "repositoryItemRegD";
            this.repositoryItemRegD.RadioGroupIndex = 2;
            this.repositoryItemRegD.CheckedChanged += new System.EventHandler(this.repositoryItemRegD_CheckedChanged);
            // 
            // gridColMenuIndicator
            // 
            this.gridColMenuIndicator.AutoFillDown = true;
            this.gridColMenuIndicator.Caption = "Tran Menu";
            this.gridColMenuIndicator.ColumnEdit = this.repItemMenuIndicator;
            this.gridColMenuIndicator.FieldName = "MenuIndicator";
            this.gridColMenuIndicator.Name = "gridColMenuIndicator";
            this.gridColMenuIndicator.Visible = true;
            this.gridColMenuIndicator.Width = 40;
            // 
            // repItemMenuIndicator
            // 
            this.repItemMenuIndicator.Name = "repItemMenuIndicator";
            this.repItemMenuIndicator.NullText = " ";
            this.repItemMenuIndicator.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.repItemMenuIndicator.ShowMenu = false;
            // 
            // gridColAcctNo
            // 
            this.gridColAcctNo.Caption = "Acct No";
            this.gridColAcctNo.FieldName = "AcctNo";
            this.gridColAcctNo.Name = "gridColAcctNo";
            this.gridColAcctNo.OptionsColumn.ShowInCustomizationForm = false;
            this.gridColAcctNo.Width = 265;

            // 
            // repositoryItemAmount
            // 
            this.repositoryItemAmount.AutoHeight = false;
            this.repositoryItemAmount.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemAmount.Name = "repositoryItemAmount";
            // 
            // repItemOptions
            // 
            this.repItemOptions.AutoHeight = false;
            this.repItemOptions.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItemOptions.DisplayMember = "Column";
            this.repItemOptions.Name = "repItemOptions";
            this.repItemOptions.NullText = "";
            this.repItemOptions.PopupWidth = 80;
            this.repItemOptions.ValueMember = "Column";
            // 
            // dfErrorInfo
            // 
            this.dfErrorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetBoundPropertyName(this.dfErrorInfo, "");
            this.dfErrorInfo.Location = new System.Drawing.Point(0, 228);
            this.dfErrorInfo.Margin = new System.Windows.Forms.Padding(2);
            this.dfErrorInfo.Multiline = true;
            this.dfErrorInfo.Name = "dfErrorInfo";
            this.dfErrorInfo.ReadOnly = true;
            this.dfErrorInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.dfErrorInfo.Size = new System.Drawing.Size(979, 22);
            this.dfErrorInfo.TabIndex = 30;
            this.dfErrorInfo.TabStop = false;
            this.dfErrorInfo.Visible = false;
            // 
            // ucCashOut1
            // 
            this.SetBoundPropertyName(this.ucCashOut1, "");
            this.ucCashOut1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucCashOut1.Location = new System.Drawing.Point(0, 220);
            this.ucCashOut1.Margin = new System.Windows.Forms.Padding(2);
            this.ucCashOut1.Name = "ucCashOut1";
            this.ucCashOut1.Size = new System.Drawing.Size(980, 34);
            this.ucCashOut1.TabIndex = 31;
            // 
            // imgTransactions
            // 
            this.imgTransactions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTransactions.ImageStream")));
            this.imgTransactions.TransparentColor = System.Drawing.Color.Transparent;
            this.imgTransactions.Images.SetKeyName(0, "DeleteList_16x16.png");
            this.imgTransactions.Images.SetKeyName(1, "Insert_16x16.png");
            this.imgTransactions.Images.SetKeyName(2, "HorizontalGridlinesMajor_16x16.png");
            // 
            // ucTransactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucCashOut1);
            this.Controls.Add(this.dfErrorInfo);
            this.Controls.Add(this.gcTransactions);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucTransactions";
            this.Size = new System.Drawing.Size(980, 254);
            ((System.ComponentModel.ISupportInitialize)(this.gcTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvTransactions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTC)).EndInit(); 
            ((System.ComponentModel.ISupportInitialize)(this.repItemAcctType)).EndInit(); //Task# 90007
            ((System.ComponentModel.ISupportInitialize)(this.repItemAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTfrAcctType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTfrAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPassToHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRegD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemMenuIndicator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemOptions)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PDxGridControl gcTransactions;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemTC;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemAcctType; //Task# 90007
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemAccounts;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemTfrAccounts;
        private System.Windows.Forms.TextBox dfErrorInfo;
        private System.Windows.Forms.TextBox dfMakeAvail;
        private DevExpress.XtraEditors.LabelControl lblMakeAvail;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView gvTransactions;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColTran;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColAccount;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColAmount;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColOffsetAccount;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColTranDef;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColActType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColAcctNo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColRefField;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColCcAmt;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColCheckNo;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColDesc;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColRegD;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColMenuIndicator;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemPassToHistory;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColPassToHistory;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemRegD;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repItemMenuIndicator;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColHoldBal;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit repositoryItemAmount;
        private ucCashOut ucCashOut1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemTfrAcctType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColTfrAcctType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColOptions;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repItemOptions;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColTfrAmt;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandTransaction;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandMiscellaneous;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBandDescriptions;
        private System.Windows.Forms.ImageList imgTransactions;
        //private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn TfrAcctType;
        
    }
}
