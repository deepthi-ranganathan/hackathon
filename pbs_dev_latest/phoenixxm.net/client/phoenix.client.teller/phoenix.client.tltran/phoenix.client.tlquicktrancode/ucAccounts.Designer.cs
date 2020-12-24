namespace Phoenix.Client.Teller
{
    partial class ucAccounts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucAccounts));
            this.gridControlAccounts = new Phoenix.Windows.Forms.PDxGridControl();
            this.gridViewAccounts = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTranType = new DevExpress.XtraGrid.Columns.GridColumn(); //Task #115710
            this.repItemTranType = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit(); //Task #115710
            this.colAcctNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCurrentBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAvailableBalance = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNickName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcctRel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTitle1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTitle2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.imgAccounts = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAccounts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTranType)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlAccounts
            // 
            this.SetBoundPropertyName(this.gridControlAccounts, "");
            this.gridControlAccounts.ColumnHeadingLines = 1;
            this.gridControlAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // 
            // 
            this.gridControlAccounts.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControlAccounts.GenerateHiddenColumns = false;
            this.gridControlAccounts.IsResizable = false;
            this.gridControlAccounts.Location = new System.Drawing.Point(0, 0);
            this.gridControlAccounts.MainView = this.gridViewAccounts;
            this.gridControlAccounts.Margin = new System.Windows.Forms.Padding(2);
            this.gridControlAccounts.MaxNumberOfRowsToFetch = 5000;
            this.gridControlAccounts.Name = "gridControlAccounts";
            this.gridControlAccounts.Size = new System.Drawing.Size(794, 260);
            this.gridControlAccounts.TabIndex = 1;
            this.gridControlAccounts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewAccounts});
            // 
            // gridViewAccounts
            // 
            this.gridViewAccounts.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTranType, //Task #115710
            this.colAcctNo,
            this.colCurrentBalance,
            this.colAvailableBalance,
            this.colNickName,
            this.colAcctRel,
            this.colStatus,
            this.colTitle1,
            this.colTitle2});
            this.gridViewAccounts.GridControl = this.gridControlAccounts;
            this.gridViewAccounts.Name = "gridViewAccounts";
            this.gridViewAccounts.OptionsBehavior.Editable = false;
            this.gridViewAccounts.OptionsBehavior.FocusLeaveOnTab = true;
            this.gridViewAccounts.OptionsView.ShowGroupPanel = false;
            this.gridViewAccounts.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
            this.gridViewAccounts.EditFormShowing += new DevExpress.XtraGrid.Views.Grid.EditFormShowingEventHandler(this.gridView1_EditFormShowing);
            this.gridViewAccounts.EditFormPrepared += new DevExpress.XtraGrid.Views.Grid.EditFormPreparedEventHandler(this.gridView1_EditFormPrepared);
            this.gridViewAccounts.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.gridView1_SelectionChanged);
            this.gridViewAccounts.ShownEditor += new System.EventHandler(this.gridView1_ShownEditor);
            this.gridViewAccounts.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            this.gridViewAccounts.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.gridView1_ValidateRow);
            this.gridViewAccounts.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridView1_MouseDown);
            this.gridViewAccounts.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.gridView1_ValidatingEditor);
            this.gridViewAccounts.CustomDrawCell += OnCustomDrawCell; //Task #102896
            this.gridViewAccounts.CalcRowHeight += OnCalcRowHeight;

            //Begin Task #115710
            // 
            // colTranType
            // 
            this.colTranType.Caption = "Tran Type";
            this.colTranType.ColumnEdit = this.repItemTranType;
            this.colTranType.FieldName = "TranType";
            this.colTranType.MinWidth = 75;
            this.colTranType.Name = "colTranType";
            this.colTranType.Visible = true;
            this.colTranType.VisibleIndex = 0;
            // 
            // repItemTranType
            // 
            this.repItemTranType.Name = "repItemTranType";
            this.repItemTranType.NullText = " ";
            this.repItemTranType.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            //End Task #115710

            // 
            // colAcctNo
            // 
            this.colAcctNo.Caption = "Account";
            this.colAcctNo.FieldName = "AccountCombined";
            this.colAcctNo.MinWidth = 105;
            this.colAcctNo.Name = "colAcctNo";
            this.colAcctNo.Visible = true;
            this.colAcctNo.VisibleIndex = 1;
            this.colAcctNo.Width = 105;
            // 
            // colCurrentBalance
            // 
            this.colCurrentBalance.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colCurrentBalance.Caption = "Current";
            this.colCurrentBalance.DisplayFormat.FormatString = "c2";
            this.colCurrentBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCurrentBalance.FieldName = "CurBalanceAmt";
            this.colCurrentBalance.MinWidth = 65;
            this.colCurrentBalance.Name = "colCurrentBalance";
            this.colCurrentBalance.Visible = true;
            this.colCurrentBalance.VisibleIndex = 2;
            this.colCurrentBalance.Width = 65;
            // 
            // colAvailableBalance
            // 
            this.colAvailableBalance.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colAvailableBalance.Caption = "Available";
            this.colAvailableBalance.DisplayFormat.FormatString = "c2";
            this.colAvailableBalance.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colAvailableBalance.FieldName = "AvailableBalance";
            this.colAvailableBalance.MinWidth = 65;
            this.colAvailableBalance.Name = "colAvailableBalance";
            this.colAvailableBalance.Visible = true;
            this.colAvailableBalance.VisibleIndex = 3;
            this.colAvailableBalance.Width = 65;
            // 
            // colNickName
            // 
            this.colNickName.Caption = "Nickname";
            this.colNickName.FieldName = "AcctNickname";
            this.colNickName.MinWidth = 50;
            this.colNickName.Name = "colNickName";
            this.colNickName.Visible = true;
            this.colNickName.VisibleIndex = 4;
            this.colNickName.Width = 56;
            // 
            // colAcctRel
            // 
            this.colAcctRel.Caption = "Ownership";
            this.colAcctRel.FieldName = "Ownership";
            this.colAcctRel.MinWidth = 60;
            this.colAcctRel.Name = "colAcctRel";
            this.colAcctRel.Visible = true;
            this.colAcctRel.VisibleIndex = 5;
            this.colAcctRel.Width = 67;
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Status";
            this.colStatus.FieldName = "Status";
            this.colStatus.MinWidth = 55;
            this.colStatus.Name = "colStatus";
            this.colStatus.Visible = true;
            this.colStatus.VisibleIndex = 6;
            this.colStatus.Width = 64;
            // 
            // colTitle1
            // 
            this.colTitle1.Caption = "Title 1";
            this.colTitle1.FieldName = "Title1";
            this.colTitle1.MinWidth = 100;
            this.colTitle1.Name = "colTitle1";
            this.colTitle1.Visible = true;
            this.colTitle1.VisibleIndex = 7;
            this.colTitle1.Width = 100;
            // 
            // colTitle2
            // 
            this.colTitle2.Caption = "Title 2";
            this.colTitle2.FieldName = "Title2";
            this.colTitle2.MinWidth = 100;
            this.colTitle2.Name = "colTitle2";
            this.colTitle2.Visible = true;
            this.colTitle2.VisibleIndex = 8;
            this.colTitle2.Width = 100;

            // 
            // repItemMenuIndicator
            // 
            this.repItemTranType.Name = "repItemTranType";
            this.repItemTranType.NullText = " ";
            this.repItemTranType.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;


            // 
            // imgAccounts
            // 
            this.imgAccounts.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgAccounts.ImageStream"))); //Task #115710
            this.imgAccounts.TransparentColor = System.Drawing.Color.Transparent; //Task #115710
            this.imgAccounts.Images.SetKeyName(0, "HorizontalGridlinesMajor_16x16.png"); //Task #115710
            // 
            // ucAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlAccounts);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucAccounts";
            this.Size = new System.Drawing.Size(794, 260);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewAccounts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItemTranType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PDxGridControl gridControlAccounts;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewAccounts;
        private DevExpress.XtraGrid.Columns.GridColumn colNickName;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctRel;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colAcctNo;
        private DevExpress.XtraGrid.Columns.GridColumn colTranType; //Task #115710
        private DevExpress.XtraGrid.Columns.GridColumn colCurrentBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colAvailableBalance;
        private DevExpress.XtraGrid.Columns.GridColumn colTitle1;
        private DevExpress.XtraGrid.Columns.GridColumn colTitle2;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repItemTranType; //Task #115710
        private System.Windows.Forms.ImageList imgAccounts; //Task #115710
    }
}
