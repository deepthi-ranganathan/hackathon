namespace Phoenix.Client.Teller
{
    partial class ucCustomer
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
            this.panelSearch = new System.Windows.Forms.Panel();
            this.pbSearch = new System.Windows.Forms.Button();
            this.dfAccountNo = new Phoenix.Windows.Forms.PdfStandard();
            this.lbAccountNo = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCustomerNo = new Phoenix.Windows.Forms.PdfStandard();
            this.lbCustomerNo = new Phoenix.Windows.Forms.PLabelStandard();
            this.panelSummary = new System.Windows.Forms.Panel();
            this.dfUsualName = new Phoenix.Windows.Forms.PDfDisplay();
            this.lbUsualName = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfName = new Phoenix.Windows.Forms.PDfDisplay();
            this.lbName = new Phoenix.Windows.Forms.PLabelStandard();
            this.lbReceiptDelMethod = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReceiptDelMethod = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfStatus = new Phoenix.Windows.Forms.PDfDisplay();
            this.lbSecurityInformation = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfSecInfo = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTIN = new Phoenix.Windows.Forms.PDfDisplay();
            this.lbRelations = new Phoenix.Windows.Forms.PLabelStandard();
            this.lbStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.lbTIN = new Phoenix.Windows.Forms.PLabelStandard();
            this.leCustomerRelations = new DevExpress.XtraEditors.LookUpEdit();
            this.panelSearch.SuspendLayout();
            this.panelSummary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leCustomerRelations.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.pbSearch);
            this.panelSearch.Controls.Add(this.dfAccountNo);
            this.panelSearch.Controls.Add(this.lbAccountNo);
            this.panelSearch.Controls.Add(this.dfCustomerNo);
            this.panelSearch.Controls.Add(this.lbCustomerNo);
            this.panelSearch.Location = new System.Drawing.Point(13, 0);
            this.panelSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(1534, 37);
            this.panelSearch.TabIndex = 1;
            // 
            // pbSearch
            // 
            this.pbSearch.Location = new System.Drawing.Point(723, 6);
            this.pbSearch.Margin = new System.Windows.Forms.Padding(4);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(39, 28);
            this.pbSearch.TabIndex = 4;
            this.pbSearch.UseVisualStyleBackColor = true;
            this.pbSearch.Click += new System.EventHandler(this.pbSearch_Click);
            this.pbSearch.Leave += new System.EventHandler(this.pbSearch_Leave);
            // 
            // dfAccountNo
            // 
            this.dfAccountNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccountNo.Location = new System.Drawing.Point(520, 8);
            this.dfAccountNo.Margin = new System.Windows.Forms.Padding(4);
            this.dfAccountNo.MaxLength = 19; //Bug 108074
            this.dfAccountNo.Name = "dfAccountNo";
            this.dfAccountNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccountNo.PhoenixUIControl.ObjectId = 1;
            this.dfAccountNo.PreviousValue = null;
            this.dfAccountNo.Size = new System.Drawing.Size(153, 22);
            this.dfAccountNo.TabIndex = 3;
            this.dfAccountNo.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.dfAccountNo_PhoenixUILeaveEvent);
            this.dfAccountNo.TextChanged += new System.EventHandler(this.dfAccountNo_TextChanged);
            this.dfAccountNo.Leave += new System.EventHandler(this.dfAccountNo_Leave);
            // 
            // lbAccountNo
            // 
            this.lbAccountNo.AutoEllipsis = true;
            this.lbAccountNo.Location = new System.Drawing.Point(377, 5);
            this.lbAccountNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAccountNo.Name = "lbAccountNo";
            this.lbAccountNo.PhoenixUIControl.ObjectId = 1;
            this.lbAccountNo.Size = new System.Drawing.Size(135, 25);
            this.lbAccountNo.TabIndex = 2;
            this.lbAccountNo.Text = "Account Number:";
            // 
            // dfCustomerNo
            // 
            this.dfCustomerNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfCustomerNo.Location = new System.Drawing.Point(153, 8);
            this.dfCustomerNo.Margin = new System.Windows.Forms.Padding(4);
            this.dfCustomerNo.MaxLength = 10;
            this.dfCustomerNo.Name = "dfCustomerNo";
            this.dfCustomerNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfCustomerNo.PreviousValue = null;
            this.dfCustomerNo.Size = new System.Drawing.Size(153, 22);
            this.dfCustomerNo.TabIndex = 1;
            this.dfCustomerNo.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.dfCustomerNo_PhoenixUILeaveEvent);
            this.dfCustomerNo.TextChanged += new System.EventHandler(this.dfCustomerNo_TextChanged);
            this.dfCustomerNo.Leave += new System.EventHandler(this.dfCustomerNo_Leave);
            this.dfCustomerNo.PhoenixUIControl.InputMask = "9999999999"; // Bug 110043
            // 
            // lbCustomerNo
            // 
            this.lbCustomerNo.AutoEllipsis = true;
            this.lbCustomerNo.Location = new System.Drawing.Point(4, 5);
            this.lbCustomerNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCustomerNo.Name = "lbCustomerNo";
            this.lbCustomerNo.Size = new System.Drawing.Size(141, 25);
            this.lbCustomerNo.TabIndex = 0;
            this.lbCustomerNo.Text = "Customer Number:";
            // 
            // panelSummary
            // 
            this.panelSummary.Controls.Add(this.dfUsualName);
            this.panelSummary.Controls.Add(this.lbUsualName);
            this.panelSummary.Controls.Add(this.dfName);
            this.panelSummary.Controls.Add(this.lbName);
            this.panelSummary.Controls.Add(this.lbReceiptDelMethod);
            this.panelSummary.Controls.Add(this.dfReceiptDelMethod);
            this.panelSummary.Controls.Add(this.dfStatus);
            this.panelSummary.Controls.Add(this.lbSecurityInformation);
            this.panelSummary.Controls.Add(this.dfSecInfo);
            this.panelSummary.Controls.Add(this.dfTIN);
            this.panelSummary.Controls.Add(this.lbRelations);
            this.panelSummary.Controls.Add(this.lbStatus);
            this.panelSummary.Controls.Add(this.lbTIN);
            this.panelSummary.Controls.Add(this.leCustomerRelations);
            this.panelSummary.Location = new System.Drawing.Point(13, 39);
            this.panelSummary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelSummary.Name = "panelSummary";
            this.panelSummary.Size = new System.Drawing.Size(1534, 39);
            this.panelSummary.TabIndex = 2;
            // 
            // dfUsualName
            // 
            this.dfUsualName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfUsualName.Location = new System.Drawing.Point(413, 6);
            this.dfUsualName.Margin = new System.Windows.Forms.Padding(4);
            this.dfUsualName.Multiline = true;
            this.dfUsualName.Name = "dfUsualName";
            this.dfUsualName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfUsualName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfUsualName.PhoenixUIControl.ObjectId = 8;
            this.dfUsualName.PhoenixUIControl.XmlTag = "";
            this.dfUsualName.PreviousValue = null;
            this.dfUsualName.Size = new System.Drawing.Size(225, 20);
            this.dfUsualName.TabIndex = 3;
            this.dfUsualName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbUsualName
            // 
            this.lbUsualName.AutoEllipsis = true;
            this.lbUsualName.Location = new System.Drawing.Point(364, 5);
            this.lbUsualName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUsualName.Name = "lbUsualName";
            this.lbUsualName.PhoenixUIControl.ObjectId = 1;
            this.lbUsualName.Size = new System.Drawing.Size(55, 25);
            this.lbUsualName.TabIndex = 2;
            this.lbUsualName.Text = "Usual:";
            // 
            // dfName
            // 
            this.dfName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfName.Location = new System.Drawing.Point(55, 6);
            this.dfName.Margin = new System.Windows.Forms.Padding(4);
            this.dfName.MaxLength = 40;
            this.dfName.Multiline = true;
            this.dfName.Name = "dfName";
            this.dfName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfName.PhoenixUIControl.ObjectId = 8;
            this.dfName.PhoenixUIControl.XmlTag = "";
            this.dfName.PreviousValue = null;
            this.dfName.Size = new System.Drawing.Size(304, 20);
            this.dfName.TabIndex = 1;
            this.dfName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbName
            // 
            this.lbName.AutoEllipsis = true;
            this.lbName.Location = new System.Drawing.Point(4, 5);
            this.lbName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbName.Name = "lbName";
            this.lbName.PhoenixUIControl.ObjectId = 1;
            this.lbName.Size = new System.Drawing.Size(51, 25);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name:";
            // 
            // lbReceiptDelMethod
            // 
            this.lbReceiptDelMethod.AutoEllipsis = true;
            this.lbReceiptDelMethod.Location = new System.Drawing.Point(1249, 5);
            this.lbReceiptDelMethod.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbReceiptDelMethod.Name = "lbReceiptDelMethod";
            this.lbReceiptDelMethod.PhoenixUIControl.ObjectId = 2;
            this.lbReceiptDelMethod.Size = new System.Drawing.Size(64, 25);
            this.lbReceiptDelMethod.TabIndex = 10;
            this.lbReceiptDelMethod.Text = "Receipt:";
            // 
            // dfReceiptDelMethod
            // 
            this.dfReceiptDelMethod.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfReceiptDelMethod.Location = new System.Drawing.Point(1313, 6);
            this.dfReceiptDelMethod.Margin = new System.Windows.Forms.Padding(4);
            this.dfReceiptDelMethod.MaxLength = 15;
            this.dfReceiptDelMethod.Multiline = true;
            this.dfReceiptDelMethod.Name = "dfReceiptDelMethod";
            this.dfReceiptDelMethod.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfReceiptDelMethod.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfReceiptDelMethod.PhoenixUIControl.ObjectId = 8;
            this.dfReceiptDelMethod.PhoenixUIControl.XmlTag = "";
            this.dfReceiptDelMethod.PreviousValue = null;
            this.dfReceiptDelMethod.Size = new System.Drawing.Size(99, 20);
            this.dfReceiptDelMethod.TabIndex = 11;
            this.dfReceiptDelMethod.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dfStatus
            // 
            this.dfStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfStatus.Location = new System.Drawing.Point(828, 6);
            this.dfStatus.Margin = new System.Windows.Forms.Padding(4);
            this.dfStatus.Multiline = true;
            this.dfStatus.Name = "dfStatus";
            this.dfStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfStatus.PhoenixUIControl.ObjectId = 8;
            this.dfStatus.PhoenixUIControl.XmlTag = "";
            this.dfStatus.PreviousValue = null;
            this.dfStatus.Size = new System.Drawing.Size(69, 20);
            this.dfStatus.TabIndex = 7;
            this.dfStatus.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbSecurityInformation
            // 
            this.lbSecurityInformation.AutoEllipsis = true;
            this.lbSecurityInformation.Location = new System.Drawing.Point(902, 5);
            this.lbSecurityInformation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSecurityInformation.Name = "lbSecurityInformation";
            this.lbSecurityInformation.Size = new System.Drawing.Size(93, 25);
            this.lbSecurityInformation.TabIndex = 8;
            this.lbSecurityInformation.Text = "Security Info:";
            // 
            // dfSecInfo
            // 
            this.dfSecInfo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfSecInfo.Location = new System.Drawing.Point(995, 6);
            this.dfSecInfo.Margin = new System.Windows.Forms.Padding(4);
            this.dfSecInfo.MaxLength = 40;
            this.dfSecInfo.Multiline = true;
            this.dfSecInfo.Name = "dfSecInfo";
            this.dfSecInfo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfSecInfo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfSecInfo.PhoenixUIControl.ObjectId = 8;
            this.dfSecInfo.PhoenixUIControl.XmlTag = "";
            this.dfSecInfo.PreviousValue = null;
            this.dfSecInfo.Size = new System.Drawing.Size(249, 20);
            this.dfSecInfo.TabIndex = 9;
            this.dfSecInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // dfTIN
            // 
            this.dfTIN.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTIN.Location = new System.Drawing.Point(678, 6);
            this.dfTIN.Margin = new System.Windows.Forms.Padding(4);
            this.dfTIN.Multiline = true;
            this.dfTIN.Name = "dfTIN";
            this.dfTIN.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTIN.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTIN.PhoenixUIControl.ObjectId = 8;
            this.dfTIN.PhoenixUIControl.XmlTag = "";
            this.dfTIN.PreviousValue = null;
            this.dfTIN.Size = new System.Drawing.Size(92, 20);
            this.dfTIN.TabIndex = 5;
            this.dfTIN.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lbRelations
            // 
            this.lbRelations.AutoEllipsis = true;
            this.lbRelations.Location = new System.Drawing.Point(1074, 5);
            this.lbRelations.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRelations.Name = "lbRelations";
            this.lbRelations.PhoenixUIControl.ObjectId = 3;
            this.lbRelations.Size = new System.Drawing.Size(73, 25);
            this.lbRelations.TabIndex = 3;
            this.lbRelations.Text = "Relations:";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoEllipsis = true;
            this.lbStatus.Location = new System.Drawing.Point(775, 5);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.PhoenixUIControl.ObjectId = 2;
            this.lbStatus.Size = new System.Drawing.Size(53, 25);
            this.lbStatus.TabIndex = 6;
            this.lbStatus.Text = "Status:";
            // 
            // lbTIN
            // 
            this.lbTIN.AutoEllipsis = true;
            this.lbTIN.Location = new System.Drawing.Point(643, 5);
            this.lbTIN.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTIN.Name = "lbTIN";
            this.lbTIN.PhoenixUIControl.ObjectId = 1;
            this.lbTIN.Size = new System.Drawing.Size(35, 25);
            this.lbTIN.TabIndex = 4;
            this.lbTIN.Text = "TIN:";
            // 
            // leCustomerRelations
            // 
            this.leCustomerRelations.Location = new System.Drawing.Point(1149, 5);
            this.leCustomerRelations.Margin = new System.Windows.Forms.Padding(4);
            this.leCustomerRelations.Name = "leCustomerRelations";
            // 
            // 
            // 
            this.leCustomerRelations.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.leCustomerRelations.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FirstName", "FirstName", 45, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("LastName", "LastName", 45, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Relationship", "Relationship", 40, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Status", "Status", 15, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.leCustomerRelations.Properties.PopupFormMinSize = new System.Drawing.Size(280, 0);
            this.leCustomerRelations.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.leCustomerRelations.Properties.UseDropDownRowsAsMaxCount = true;
            this.leCustomerRelations.Size = new System.Drawing.Size(28, 22);
            this.leCustomerRelations.TabIndex = 3;
            // 
            // ucCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSummary);
            this.Controls.Add(this.panelSearch);
            this.ForeColor = System.Drawing.SystemColors.WindowText;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ucCustomer";
            this.Size = new System.Drawing.Size(1550, 87);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelSummary.ResumeLayout(false);
            this.panelSummary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.leCustomerRelations.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel panelSearch;
        protected System.Windows.Forms.Panel panelSummary;
        protected Windows.Forms.PdfStandard dfCustomerNo;
        protected Windows.Forms.PLabelStandard lbCustomerNo;
        protected Windows.Forms.PLabelStandard lbAccountNo;
        protected Windows.Forms.PdfStandard dfAccountNo;
        protected Windows.Forms.PLabelStandard lbSecurityInformation;
        protected Windows.Forms.PLabelStandard lbTIN;
        protected Windows.Forms.PLabelStandard lbStatus;
        protected Windows.Forms.PLabelStandard lbRelations;
        protected Windows.Forms.PDfDisplay dfSecInfo;
        protected Windows.Forms.PDfDisplay dfTIN;
        protected Windows.Forms.PDfDisplay dfStatus;
        public System.Windows.Forms.Button pbSearch;
        protected Windows.Forms.PDfDisplay dfReceiptDelMethod;
        protected Windows.Forms.PLabelStandard lbReceiptDelMethod;
        protected Windows.Forms.PDfDisplay dfName;
        protected Windows.Forms.PLabelStandard lbName;
        protected Windows.Forms.PDfDisplay dfUsualName;
        protected Windows.Forms.PLabelStandard lbUsualName;
        private DevExpress.XtraEditors.LookUpEdit leCustomerRelations;
    }
}
