namespace Phoenix.Client.Teller
{
    partial class frmInventoryItemPurchaseReturnDetail
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
            this.gbTypeInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfPacketDesc = new Phoenix.Windows.Forms.PdfStandard();
            this.dfClass = new Phoenix.Windows.Forms.PdfStandard();
            this.lblClass = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPacketId = new Phoenix.Windows.Forms.PdfStandard();
            this.lblPacketId = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTypeId = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTypeId = new Phoenix.Windows.Forms.PdfStandard();
            this.pbListView = new Phoenix.Windows.Forms.PAction();
            this.gbAccountInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfName = new Phoenix.Windows.Forms.PdfStandard();
            this.lblName = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRimNo = new Phoenix.Windows.Forms.PdfStandard();
            this.lblRimNo = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbNonCustomer = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbCounterInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblPurchReturn = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblAvailable = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfLocalTax = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblLocalTax = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfStateTax = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblStateTax = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbPercent = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbAmount = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.dfTotalPurchAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblTotalPurchAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalDiscOvrd = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTotalDiscOvrd = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalPurchCount = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTotalPurchCount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalAvailAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblTotalAvailAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalAvailCount = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTotalAvailCount = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbMoveLeft = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbMoveRight = new Phoenix.Windows.Forms.PButtonStandard();
            this.grdPurchReturnList = new Phoenix.Windows.Forms.PGrid();
            this.colPurchReturnItemNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRowNo1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colLastActivityDt1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colSerialNo1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colCurrentAmt1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemValue1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketItemValue1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colUpchargeAmt1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colPurchReturnModified = new Phoenix.Windows.Forms.PGridColumn();
            this.colTypeId1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketId1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colClass1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocation1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchNo1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatusSort1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colUseDrawerLevel1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocationSort1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemExists1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colSoldAmt1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colPrevCurrentAmt1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketDesc1 = new Phoenix.Windows.Forms.PGridColumn();
            this.grdAvailableList = new Phoenix.Windows.Forms.PGrid();
            this.colAvailableItemNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRowNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colLastActivityDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colSerialNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCurrentAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketItemValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colUpchargeAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colAvailableModified = new Phoenix.Windows.Forms.PGridColumn();
            this.colTypeId = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketId = new Phoenix.Windows.Forms.PGridColumn();
            this.colClass = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocation = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colStatusSort = new Phoenix.Windows.Forms.PGridColumn();
            this.colUseDrawerLevel = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocationSort = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemExists = new Phoenix.Windows.Forms.PGridColumn();
            this.colSoldAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketDesc = new Phoenix.Windows.Forms.PGridColumn();
            this.gbTypeInfo.SuspendLayout();
            this.gbAccountInfo.SuspendLayout();
            this.gbCounterInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbListView});
            // 
            // gbTypeInfo
            // 
            this.gbTypeInfo.Controls.Add(this.dfPacketDesc);
            this.gbTypeInfo.Controls.Add(this.dfClass);
            this.gbTypeInfo.Controls.Add(this.lblClass);
            this.gbTypeInfo.Controls.Add(this.dfPacketId);
            this.gbTypeInfo.Controls.Add(this.lblPacketId);
            this.gbTypeInfo.Controls.Add(this.lblTypeId);
            this.gbTypeInfo.Controls.Add(this.dfTypeId);
            this.gbTypeInfo.Location = new System.Drawing.Point(4, 0);
            this.gbTypeInfo.Name = "gbTypeInfo";
            this.gbTypeInfo.PhoenixUIControl.ObjectId = 1;
            this.gbTypeInfo.Size = new System.Drawing.Size(683, 64);
            this.gbTypeInfo.TabIndex = 0;
            this.gbTypeInfo.TabStop = false;
            this.gbTypeInfo.Text = "Type Information";
            // 
            // dfPacketDesc
            // 
            this.dfPacketDesc.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfPacketDesc.Location = new System.Drawing.Point(147, 43);
            this.dfPacketDesc.Name = "dfPacketDesc";
            this.dfPacketDesc.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfPacketDesc.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPacketDesc.PhoenixUIControl.ObjectId = 4;
            this.dfPacketDesc.PhoenixUIControl.XmlTag = "";
            this.dfPacketDesc.ReadOnly = true;
            this.dfPacketDesc.Size = new System.Drawing.Size(250, 13);
            this.dfPacketDesc.TabIndex = 6;
            this.dfPacketDesc.TabStop = false;
            this.dfPacketDesc.Visible = false;
            // 
            // dfClass
            // 
            this.dfClass.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfClass.Location = new System.Drawing.Point(577, 20);
            this.dfClass.Name = "dfClass";
            this.dfClass.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfClass.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfClass.PhoenixUIControl.ObjectId = 3;
            this.dfClass.PhoenixUIControl.XmlTag = "";
            this.dfClass.ReadOnly = true;
            this.dfClass.Size = new System.Drawing.Size(100, 13);
            this.dfClass.TabIndex = 3;
            this.dfClass.TabStop = false;
            // 
            // lblClass
            // 
            this.lblClass.AutoEllipsis = true;
            this.lblClass.Location = new System.Drawing.Point(528, 16);
            this.lblClass.Name = "lblClass";
            this.lblClass.PhoenixUIControl.ObjectId = 3;
            this.lblClass.Size = new System.Drawing.Size(43, 20);
            this.lblClass.TabIndex = 2;
            this.lblClass.Text = "Class:";
            // 
            // dfPacketId
            // 
            this.dfPacketId.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfPacketId.Location = new System.Drawing.Point(82, 43);
            this.dfPacketId.Name = "dfPacketId";
            this.dfPacketId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfPacketId.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfPacketId.PhoenixUIControl.ObjectId = 4;
            this.dfPacketId.PhoenixUIControl.XmlTag = "";
            this.dfPacketId.ReadOnly = true;
            this.dfPacketId.Size = new System.Drawing.Size(413, 13);
            this.dfPacketId.TabIndex = 5;
            this.dfPacketId.TabStop = false;
            // 
            // lblPacketId
            // 
            this.lblPacketId.AutoEllipsis = true;
            this.lblPacketId.Location = new System.Drawing.Point(6, 39);
            this.lblPacketId.Name = "lblPacketId";
            this.lblPacketId.PhoenixUIControl.ObjectId = 4;
            this.lblPacketId.Size = new System.Drawing.Size(68, 20);
            this.lblPacketId.TabIndex = 4;
            this.lblPacketId.Text = "Packet ID:";
            // 
            // lblTypeId
            // 
            this.lblTypeId.AutoEllipsis = true;
            this.lblTypeId.Location = new System.Drawing.Point(6, 16);
            this.lblTypeId.Name = "lblTypeId";
            this.lblTypeId.PhoenixUIControl.ObjectId = 2;
            this.lblTypeId.Size = new System.Drawing.Size(70, 20);
            this.lblTypeId.TabIndex = 0;
            this.lblTypeId.Text = "Type ID:";
            // 
            // dfTypeId
            // 
            this.dfTypeId.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTypeId.Location = new System.Drawing.Point(82, 20);
            this.dfTypeId.Name = "dfTypeId";
            this.dfTypeId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTypeId.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTypeId.PhoenixUIControl.ObjectId = 2;
            this.dfTypeId.PhoenixUIControl.XmlTag = "";
            this.dfTypeId.ReadOnly = true;
            this.dfTypeId.Size = new System.Drawing.Size(440, 13);
            this.dfTypeId.TabIndex = 1;
            this.dfTypeId.TabStop = false;
            // 
            // pbListView
            // 
            this.pbListView.LongText = "ListView";
            this.pbListView.Name = null;
            this.pbListView.Tag = null;
            // 
            // gbAccountInfo
            // 
            this.gbAccountInfo.Controls.Add(this.dfName);
            this.gbAccountInfo.Controls.Add(this.lblName);
            this.gbAccountInfo.Controls.Add(this.dfRimNo);
            this.gbAccountInfo.Controls.Add(this.lblRimNo);
            this.gbAccountInfo.Controls.Add(this.cbNonCustomer);
            this.gbAccountInfo.Location = new System.Drawing.Point(4, 65);
            this.gbAccountInfo.Name = "gbAccountInfo";
            this.gbAccountInfo.PhoenixUIControl.ObjectId = 5;
            this.gbAccountInfo.Size = new System.Drawing.Size(683, 65);
            this.gbAccountInfo.TabIndex = 1;
            this.gbAccountInfo.TabStop = false;
            this.gbAccountInfo.Text = "Account Information";
            // 
            // dfName
            // 
            this.dfName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfName.Location = new System.Drawing.Point(248, 44);
            this.dfName.Name = "dfName";
            this.dfName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfName.PhoenixUIControl.ObjectId = 7;
            this.dfName.PhoenixUIControl.XmlTag = "";
            this.dfName.ReadOnly = true;
            this.dfName.Size = new System.Drawing.Size(429, 13);
            this.dfName.TabIndex = 4;
            this.dfName.TabStop = false;
            // 
            // lblName
            // 
            this.lblName.AutoEllipsis = true;
            this.lblName.Location = new System.Drawing.Point(199, 40);
            this.lblName.Name = "lblName";
            this.lblName.PhoenixUIControl.ObjectId = 7;
            this.lblName.Size = new System.Drawing.Size(43, 20);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "Name:";
            // 
            // dfRimNo
            // 
            this.dfRimNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfRimNo.Location = new System.Drawing.Point(82, 44);
            this.dfRimNo.Name = "dfRimNo";
            this.dfRimNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfRimNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRimNo.PhoenixUIControl.ObjectId = 6;
            this.dfRimNo.PhoenixUIControl.XmlTag = "";
            this.dfRimNo.ReadOnly = true;
            this.dfRimNo.Size = new System.Drawing.Size(100, 13);
            this.dfRimNo.TabIndex = 2;
            this.dfRimNo.TabStop = false;
            // 
            // lblRimNo
            // 
            this.lblRimNo.AutoEllipsis = true;
            this.lblRimNo.Location = new System.Drawing.Point(6, 40);
            this.lblRimNo.Name = "lblRimNo";
            this.lblRimNo.PhoenixUIControl.ObjectId = 6;
            this.lblRimNo.Size = new System.Drawing.Size(82, 20);
            this.lblRimNo.TabIndex = 1;
            this.lblRimNo.Text = "Customer No:";
            // 
            // cbNonCustomer
            // 
            this.cbNonCustomer.AutoSize = true;
            this.cbNonCustomer.BackColor = System.Drawing.SystemColors.Control;
            this.cbNonCustomer.Checked = true;
            this.cbNonCustomer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNonCustomer.Enabled = false;
            this.cbNonCustomer.Location = new System.Drawing.Point(6, 19);
            this.cbNonCustomer.Name = "cbNonCustomer";
            this.cbNonCustomer.PhoenixUIControl.ObjectId = 24;
            this.cbNonCustomer.PhoenixUIControl.XmlTag = "";
            this.cbNonCustomer.Size = new System.Drawing.Size(99, 18);
            this.cbNonCustomer.TabIndex = 0;
            this.cbNonCustomer.TabStop = false;
            this.cbNonCustomer.Text = "Non-Customer";
            this.cbNonCustomer.UseVisualStyleBackColor = false;
            this.cbNonCustomer.Value = null;
            // 
            // gbCounterInfo
            // 
            this.gbCounterInfo.Controls.Add(this.lblPurchReturn);
            this.gbCounterInfo.Controls.Add(this.lblAvailable);
            this.gbCounterInfo.Controls.Add(this.dfLocalTax);
            this.gbCounterInfo.Controls.Add(this.lblLocalTax);
            this.gbCounterInfo.Controls.Add(this.dfStateTax);
            this.gbCounterInfo.Controls.Add(this.lblStateTax);
            this.gbCounterInfo.Controls.Add(this.rbPercent);
            this.gbCounterInfo.Controls.Add(this.rbAmount);
            this.gbCounterInfo.Controls.Add(this.dfTotalPurchAmt);
            this.gbCounterInfo.Controls.Add(this.lblTotalPurchAmt);
            this.gbCounterInfo.Controls.Add(this.dfTotalDiscOvrd);
            this.gbCounterInfo.Controls.Add(this.lblTotalDiscOvrd);
            this.gbCounterInfo.Controls.Add(this.dfTotalPurchCount);
            this.gbCounterInfo.Controls.Add(this.lblTotalPurchCount);
            this.gbCounterInfo.Controls.Add(this.dfTotalAvailAmt);
            this.gbCounterInfo.Controls.Add(this.lblTotalAvailAmt);
            this.gbCounterInfo.Controls.Add(this.dfTotalAvailCount);
            this.gbCounterInfo.Controls.Add(this.lblTotalAvailCount);
            this.gbCounterInfo.Controls.Add(this.pbMoveLeft);
            this.gbCounterInfo.Controls.Add(this.pbMoveRight);
            this.gbCounterInfo.Controls.Add(this.grdPurchReturnList);
            this.gbCounterInfo.Controls.Add(this.grdAvailableList);
            this.gbCounterInfo.Location = new System.Drawing.Point(4, 131);
            this.gbCounterInfo.Name = "gbCounterInfo";
            this.gbCounterInfo.PhoenixUIControl.ObjectId = 8;
            this.gbCounterInfo.Size = new System.Drawing.Size(683, 314);
            this.gbCounterInfo.TabIndex = 2;
            this.gbCounterInfo.TabStop = false;
            this.gbCounterInfo.Text = "Counter Information";
            // 
            // lblPurchReturn
            // 
            this.lblPurchReturn.AutoEllipsis = true;
            this.lblPurchReturn.Location = new System.Drawing.Point(357, 16);
            this.lblPurchReturn.Name = "lblPurchReturn";
            this.lblPurchReturn.PhoenixUIControl.ObjectId = 10;
            this.lblPurchReturn.Size = new System.Drawing.Size(100, 20);
            this.lblPurchReturn.TabIndex = 4;
            this.lblPurchReturn.Text = "Purchase:";
            // 
            // lblAvailable
            // 
            this.lblAvailable.AutoEllipsis = true;
            this.lblAvailable.Location = new System.Drawing.Point(6, 16);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.PhoenixUIControl.ObjectId = 9;
            this.lblAvailable.Size = new System.Drawing.Size(100, 20);
            this.lblAvailable.TabIndex = 0;
            this.lblAvailable.Text = "Available:";
            // 
            // dfLocalTax
            // 
            this.dfLocalTax.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLocalTax.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfLocalTax.Location = new System.Drawing.Point(599, 293);
            this.dfLocalTax.Name = "dfLocalTax";
            this.dfLocalTax.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfLocalTax.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfLocalTax.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfLocalTax.PhoenixUIControl.MaxPrecision = 14;
            this.dfLocalTax.PhoenixUIControl.ObjectId = 23;
            this.dfLocalTax.ReadOnly = true;
            this.dfLocalTax.Size = new System.Drawing.Size(78, 13);
            this.dfLocalTax.TabIndex = 21;
            this.dfLocalTax.TabStop = false;
            this.dfLocalTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblLocalTax
            // 
            this.lblLocalTax.AutoEllipsis = true;
            this.lblLocalTax.Location = new System.Drawing.Point(528, 289);
            this.lblLocalTax.Name = "lblLocalTax";
            this.lblLocalTax.PhoenixUIControl.ObjectId = 23;
            this.lblLocalTax.Size = new System.Drawing.Size(65, 20);
            this.lblLocalTax.TabIndex = 20;
            this.lblLocalTax.Text = "Local Tax:";
            // 
            // dfStateTax
            // 
            this.dfStateTax.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfStateTax.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfStateTax.Location = new System.Drawing.Point(417, 293);
            this.dfStateTax.Name = "dfStateTax";
            this.dfStateTax.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfStateTax.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfStateTax.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfStateTax.PhoenixUIControl.MaxPrecision = 14;
            this.dfStateTax.PhoenixUIControl.ObjectId = 22;
            this.dfStateTax.ReadOnly = true;
            this.dfStateTax.Size = new System.Drawing.Size(78, 13);
            this.dfStateTax.TabIndex = 19;
            this.dfStateTax.TabStop = false;
            this.dfStateTax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblStateTax
            // 
            this.lblStateTax.AutoEllipsis = true;
            this.lblStateTax.Location = new System.Drawing.Point(357, 289);
            this.lblStateTax.Name = "lblStateTax";
            this.lblStateTax.PhoenixUIControl.ObjectId = 22;
            this.lblStateTax.Size = new System.Drawing.Size(57, 20);
            this.lblStateTax.TabIndex = 18;
            this.lblStateTax.Text = "State Tax:";
            // 
            // rbPercent
            // 
            this.rbPercent.AutoSize = true;
            this.rbPercent.BackColor = System.Drawing.SystemColors.Control;
            this.rbPercent.Description = null;
            this.rbPercent.Location = new System.Drawing.Point(610, 256);
            this.rbPercent.Name = "rbPercent";
            this.rbPercent.PhoenixUIControl.ObjectId = 20;
            this.rbPercent.PhoenixUIControl.XmlTag = "DiscountType";
            this.rbPercent.Size = new System.Drawing.Size(68, 18);
            this.rbPercent.TabIndex = 15;
            this.rbPercent.Text = "Percent";
            this.rbPercent.UseVisualStyleBackColor = false;
            this.rbPercent.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbPercent_PhoenixUICheckedChangedEvent);
            // 
            // rbAmount
            // 
            this.rbAmount.AutoSize = true;
            this.rbAmount.BackColor = System.Drawing.SystemColors.Control;
            this.rbAmount.Description = null;
            this.rbAmount.IsMaster = true;
            this.rbAmount.Location = new System.Drawing.Point(610, 238);
            this.rbAmount.Name = "rbAmount";
            this.rbAmount.PhoenixUIControl.ObjectId = 19;
            this.rbAmount.PhoenixUIControl.XmlTag = "DiscountType";
            this.rbAmount.Size = new System.Drawing.Size(67, 18);
            this.rbAmount.TabIndex = 14;
            this.rbAmount.Text = "Amount";
            this.rbAmount.UseVisualStyleBackColor = false;
            this.rbAmount.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbAmount_PhoenixUICheckedChangedEvent);
            // 
            // dfTotalPurchAmt
            // 
            this.dfTotalPurchAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalPurchAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalPurchAmt.Location = new System.Drawing.Point(501, 271);
            this.dfTotalPurchAmt.Name = "dfTotalPurchAmt";
            this.dfTotalPurchAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalPurchAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalPurchAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalPurchAmt.PhoenixUIControl.MaxPrecision = 14;
            this.dfTotalPurchAmt.PhoenixUIControl.ObjectId = 21;
            this.dfTotalPurchAmt.ReadOnly = true;
            this.dfTotalPurchAmt.Size = new System.Drawing.Size(100, 13);
            this.dfTotalPurchAmt.TabIndex = 17;
            this.dfTotalPurchAmt.TabStop = false;
            this.dfTotalPurchAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalPurchAmt
            // 
            this.lblTotalPurchAmt.AutoEllipsis = true;
            this.lblTotalPurchAmt.Location = new System.Drawing.Point(357, 267);
            this.lblTotalPurchAmt.Name = "lblTotalPurchAmt";
            this.lblTotalPurchAmt.PhoenixUIControl.ObjectId = 21;
            this.lblTotalPurchAmt.Size = new System.Drawing.Size(118, 20);
            this.lblTotalPurchAmt.TabIndex = 16;
            this.lblTotalPurchAmt.Text = "Total Purchased Amt:";
            // 
            // dfTotalDiscOvrd
            // 
            this.dfTotalDiscOvrd.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDiscOvrd.Location = new System.Drawing.Point(505, 244);
            this.dfTotalDiscOvrd.MaxLength = 15;
            this.dfTotalDiscOvrd.Name = "dfTotalDiscOvrd";
            this.dfTotalDiscOvrd.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDiscOvrd.PhoenixUIControl.ObjectId = 18;
            this.dfTotalDiscOvrd.PhoenixUIControl.XmlTag = "DiscountOvrd";
            this.dfTotalDiscOvrd.Size = new System.Drawing.Size(100, 20);
            this.dfTotalDiscOvrd.TabIndex = 13;
            this.dfTotalDiscOvrd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dfTotalDiscOvrd.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfTotalDiscOvrd_PhoenixUIValidateEvent);
            this.dfTotalDiscOvrd.PhoenixUIEditedEvent += new Phoenix.Windows.Forms.ValueEditedEventHandler(this.dfTotalDiscOvrd_PhoenixUIEditedEvent);
            // 
            // lblTotalDiscOvrd
            // 
            this.lblTotalDiscOvrd.AutoEllipsis = true;
            this.lblTotalDiscOvrd.Location = new System.Drawing.Point(357, 244);
            this.lblTotalDiscOvrd.Name = "lblTotalDiscOvrd";
            this.lblTotalDiscOvrd.PhoenixUIControl.ObjectId = 18;
            this.lblTotalDiscOvrd.Size = new System.Drawing.Size(122, 20);
            this.lblTotalDiscOvrd.TabIndex = 12;
            this.lblTotalDiscOvrd.Text = "Total Discount Ovrd:";
            // 
            // dfTotalPurchCount
            // 
            this.dfTotalPurchCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalPurchCount.Location = new System.Drawing.Point(501, 225);
            this.dfTotalPurchCount.Name = "dfTotalPurchCount";
            this.dfTotalPurchCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalPurchCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalPurchCount.PhoenixUIControl.ObjectId = 17;
            this.dfTotalPurchCount.ReadOnly = true;
            this.dfTotalPurchCount.Size = new System.Drawing.Size(100, 13);
            this.dfTotalPurchCount.TabIndex = 9;
            this.dfTotalPurchCount.TabStop = false;
            this.dfTotalPurchCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalPurchCount
            // 
            this.lblTotalPurchCount.AutoEllipsis = true;
            this.lblTotalPurchCount.Location = new System.Drawing.Point(357, 221);
            this.lblTotalPurchCount.Name = "lblTotalPurchCount";
            this.lblTotalPurchCount.PhoenixUIControl.ObjectId = 17;
            this.lblTotalPurchCount.Size = new System.Drawing.Size(123, 20);
            this.lblTotalPurchCount.TabIndex = 8;
            this.lblTotalPurchCount.Text = "Total Purchased Count:";
            // 
            // dfTotalAvailAmt
            // 
            this.dfTotalAvailAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalAvailAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalAvailAmt.Location = new System.Drawing.Point(225, 248);
            this.dfTotalAvailAmt.Name = "dfTotalAvailAmt";
            this.dfTotalAvailAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalAvailAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalAvailAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalAvailAmt.PhoenixUIControl.MaxPrecision = 14;
            this.dfTotalAvailAmt.PhoenixUIControl.ObjectId = 16;
            this.dfTotalAvailAmt.ReadOnly = true;
            this.dfTotalAvailAmt.Size = new System.Drawing.Size(100, 13);
            this.dfTotalAvailAmt.TabIndex = 11;
            this.dfTotalAvailAmt.TabStop = false;
            this.dfTotalAvailAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalAvailAmt
            // 
            this.lblTotalAvailAmt.AutoEllipsis = true;
            this.lblTotalAvailAmt.Location = new System.Drawing.Point(6, 244);
            this.lblTotalAvailAmt.Name = "lblTotalAvailAmt";
            this.lblTotalAvailAmt.PhoenixUIControl.ObjectId = 16;
            this.lblTotalAvailAmt.Size = new System.Drawing.Size(119, 20);
            this.lblTotalAvailAmt.TabIndex = 10;
            this.lblTotalAvailAmt.Text = "Total Available Amt:";
            // 
            // dfTotalAvailCount
            // 
            this.dfTotalAvailCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalAvailCount.Location = new System.Drawing.Point(225, 225);
            this.dfTotalAvailCount.Name = "dfTotalAvailCount";
            this.dfTotalAvailCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalAvailCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalAvailCount.PhoenixUIControl.ObjectId = 15;
            this.dfTotalAvailCount.ReadOnly = true;
            this.dfTotalAvailCount.Size = new System.Drawing.Size(100, 13);
            this.dfTotalAvailCount.TabIndex = 7;
            this.dfTotalAvailCount.TabStop = false;
            this.dfTotalAvailCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalAvailCount
            // 
            this.lblTotalAvailCount.AutoEllipsis = true;
            this.lblTotalAvailCount.Location = new System.Drawing.Point(6, 221);
            this.lblTotalAvailCount.Name = "lblTotalAvailCount";
            this.lblTotalAvailCount.PhoenixUIControl.ObjectId = 15;
            this.lblTotalAvailCount.Size = new System.Drawing.Size(118, 20);
            this.lblTotalAvailCount.TabIndex = 6;
            this.lblTotalAvailCount.Text = "Total Available Count:";
            // 
            // pbMoveLeft
            // 
            this.pbMoveLeft.Location = new System.Drawing.Point(329, 139);
            this.pbMoveLeft.Name = "pbMoveLeft";
            this.pbMoveLeft.Size = new System.Drawing.Size(25, 23);
            this.pbMoveLeft.TabIndex = 3;
            this.pbMoveLeft.Text = "<";
            this.pbMoveLeft.Click += new System.EventHandler(this.pbMoveLeft_Click);
            // 
            // pbMoveRight
            // 
            this.pbMoveRight.Location = new System.Drawing.Point(330, 110);
            this.pbMoveRight.Name = "pbMoveRight";
            this.pbMoveRight.Size = new System.Drawing.Size(24, 23);
            this.pbMoveRight.TabIndex = 2;
            this.pbMoveRight.Text = ">";
            this.pbMoveRight.Click += new System.EventHandler(this.pbMoveRight_Click);
            // 
            // grdPurchReturnList
            // 
            this.grdPurchReturnList.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.grdPurchReturnList.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colPurchReturnItemNo,
            this.colRowNo1,
            this.colLastActivityDt1,
            this.colSerialNo1,
            this.colCurrentAmt1,
            this.colItemValue1,
            this.colPacketItemValue1,
            this.colUpchargeAmt1,
            this.colPurchReturnModified,
            this.colTypeId1,
            this.colPacketId1,
            this.colClass1,
            this.colLocation1,
            this.colBranchNo1,
            this.colDrawerNo1,
            this.colStatus1,
            this.colStatusSort1,
            this.colUseDrawerLevel1,
            this.colLocationSort1,
            this.colItemExists1,
            this.colSoldAmt1,
            this.colPacketDesc1,
            this.colPrevCurrentAmt1});
            this.grdPurchReturnList.IsMaxNumRowsCustomized = false;
            this.grdPurchReturnList.Location = new System.Drawing.Point(357, 39);
            this.grdPurchReturnList.MultiSelect = true;
            this.grdPurchReturnList.Name = "grdPurchReturnList";
            this.grdPurchReturnList.Size = new System.Drawing.Size(321, 178);
            this.grdPurchReturnList.TabIndex = 5;
            this.grdPurchReturnList.Text = "pGrid2";
            this.grdPurchReturnList.SelectedIndexChanged += new Windows.Forms.GridClickedEventHandler(grdPurchReturnList_SelectedIndexChanged); //23981
            // 
            // colPurchReturnItemNo
            // 
            this.colPurchReturnItemNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colPurchReturnItemNo.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colPurchReturnItemNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colPurchReturnItemNo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colPurchReturnItemNo.PhoenixUIControl.InputMask = "999999999999";
            this.colPurchReturnItemNo.PhoenixUIControl.XmlTag = "Ptid";
            this.colPurchReturnItemNo.Title = "Item #";
            this.colPurchReturnItemNo.Visible = false;
            // 
            // colRowNo1
            // 
            this.colRowNo1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowNo1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowNo1.PhoenixUIControl.ObjectId = 11;
            this.colRowNo1.PhoenixUIControl.XmlTag = "RowNum";
            this.colRowNo1.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colRowNo1.Title = "Row #";
            this.colRowNo1.Width = 40;
            // 
            // colLastActivityDt1
            // 
            this.colLastActivityDt1.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastActivityDt1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colLastActivityDt1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastActivityDt1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colLastActivityDt1.PhoenixUIControl.ObjectId = 12;
            this.colLastActivityDt1.PhoenixUIControl.XmlTag = "LastActivityDt";
            this.colLastActivityDt1.Title = "Date";
            this.colLastActivityDt1.Width = 68;
            // 
            // colSerialNo1
            // 
            this.colSerialNo1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSerialNo1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSerialNo1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo1.PhoenixUIControl.InputMask = "9999999999999999";
            this.colSerialNo1.PhoenixUIControl.ObjectId = 13;
            this.colSerialNo1.PhoenixUIControl.XmlTag = "SerialNo";
            this.colSerialNo1.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colSerialNo1.Title = "Serial #";
            this.colSerialNo1.Width = 112;
            // 
            // colCurrentAmt1
            // 
            this.colCurrentAmt1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCurrentAmt1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCurrentAmt1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCurrentAmt1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCurrentAmt1.PhoenixUIControl.ObjectId = 14;
            this.colCurrentAmt1.PhoenixUIControl.XmlTag = "InvItemAmt";
            this.colCurrentAmt1.ReadOnly = false;
            this.colCurrentAmt1.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colCurrentAmt1.Title = "Current Amt";
            this.colCurrentAmt1.Width = 80;
            this.colCurrentAmt1.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colCurrentAmt1_PhoenixUIValidateEvent);
            // 
            // colItemValue1
            // 
            this.colItemValue1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colItemValue1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colItemValue1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colItemValue1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colItemValue1.PhoenixUIControl.XmlTag = "ItemValue";
            this.colItemValue1.Title = "Column";
            this.colItemValue1.Visible = false;
            // 
            // colPacketItemValue1
            // 
            this.colPacketItemValue1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketItemValue1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPacketItemValue1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketItemValue1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPacketItemValue1.PhoenixUIControl.XmlTag = "PacketItemValue";
            this.colPacketItemValue1.Title = "Column";
            this.colPacketItemValue1.Visible = false;
            // 
            // colUpchargeAmt1
            // 
            this.colUpchargeAmt1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUpchargeAmt1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUpchargeAmt1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUpchargeAmt1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUpchargeAmt1.PhoenixUIControl.XmlTag = "UpchargeAmt";
            this.colUpchargeAmt1.Title = "Column";
            this.colUpchargeAmt1.Visible = false;
            // 
            // colPurchReturnModified
            // 
            this.colPurchReturnModified.PhoenixUIControl.XmlTag = "Modified";
            this.colPurchReturnModified.Title = "Modified";
            this.colPurchReturnModified.Visible = false;
            // 
            // colTypeId1
            // 
            this.colTypeId1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId1.PhoenixUIControl.XmlTag = "TypeId";
            this.colTypeId1.Title = "Column";
            this.colTypeId1.Visible = false;
            // 
            // colPacketId1
            // 
            this.colPacketId1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId1.PhoenixUIControl.XmlTag = "PacketId";
            this.colPacketId1.Title = "Column";
            this.colPacketId1.Visible = false;
            // 
            // colClass1
            // 
            this.colClass1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass1.PhoenixUIControl.XmlTag = "Class";
            this.colClass1.Title = "Column";
            this.colClass1.Visible = false;
            // 
            // colLocation1
            // 
            this.colLocation1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocation1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocation1.PhoenixUIControl.XmlTag = "Location";
            this.colLocation1.Title = "Column";
            this.colLocation1.Visible = false;
            // 
            // colBranchNo1
            // 
            this.colBranchNo1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo1.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo1.Title = "Column";
            this.colBranchNo1.Visible = false;
            // 
            // colDrawerNo1
            // 
            this.colDrawerNo1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo1.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo1.Title = "Column";
            this.colDrawerNo1.Visible = false;
            // 
            // colStatus1
            // 
            this.colStatus1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus1.PhoenixUIControl.XmlTag = "Status";
            this.colStatus1.Title = "Column";
            this.colStatus1.Visible = false;
            // 
            // colStatusSort1
            // 
            this.colStatusSort1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStatusSort1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStatusSort1.PhoenixUIControl.XmlTag = "StatusSort";
            this.colStatusSort1.Title = "Column";
            this.colStatusSort1.Visible = false;
            // 
            // colUseDrawerLevel1
            // 
            this.colUseDrawerLevel1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colUseDrawerLevel1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colUseDrawerLevel1.PhoenixUIControl.XmlTag = "UseDrawerLevel";
            this.colUseDrawerLevel1.Title = "Column";
            this.colUseDrawerLevel1.Visible = false;
            // 
            // colLocationSort1
            // 
            this.colLocationSort1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLocationSort1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLocationSort1.PhoenixUIControl.XmlTag = "LocationSort";
            this.colLocationSort1.Title = "Column";
            this.colLocationSort1.Visible = false;
            // 
            // colItemExists1
            // 
            this.colItemExists1.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemExists1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemExists1.PhoenixUIControl.XmlTag = "ItemExists";
            this.colItemExists1.Title = "Column";
            this.colItemExists1.Visible = false;
            // 
            // colSoldAmt1
            // 
            this.colSoldAmt1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSoldAmt1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colSoldAmt1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSoldAmt1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colSoldAmt1.PhoenixUIControl.XmlTag = "SoldAmt";
            this.colSoldAmt1.Title = "Column";
            this.colSoldAmt1.Visible = false;
            // 
            // colPrevCurrentAmt1
            // 
            this.colPrevCurrentAmt1.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPrevCurrentAmt1.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPrevCurrentAmt1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPrevCurrentAmt1.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPrevCurrentAmt1.Title = "Column";
            this.colPrevCurrentAmt1.Visible = false;
            // 
            // colPacketDesc1
            // 
            this.colPacketDesc1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colPacketDesc1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colPacketDesc1.PhoenixUIControl.XmlTag = "0";
            this.colPacketDesc1.Title = "Column";
            this.colPacketDesc1.Visible = false;
            // 
            // grdAvailableList
            // 
            this.grdAvailableList.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.grdAvailableList.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colAvailableItemNo,
            this.colRowNo,
            this.colLastActivityDt,
            this.colSerialNo,
            this.colCurrentAmt,
            this.colItemValue,
            this.colPacketItemValue,
            this.colUpchargeAmt,
            this.colAvailableModified,
            this.colTypeId,
            this.colPacketId,
            this.colClass,
            this.colLocation,
            this.colBranchNo,
            this.colDrawerNo,
            this.colStatus,
            this.colStatusSort,
            this.colUseDrawerLevel,
            this.colLocationSort,
            this.colItemExists,
            this.colSoldAmt,
            this.colPacketDesc});
            this.grdAvailableList.IsMaxNumRowsCustomized = false;
            this.grdAvailableList.Location = new System.Drawing.Point(6, 39);
            this.grdAvailableList.MultiSelect = true;
            this.grdAvailableList.Name = "grdAvailableList";
            this.grdAvailableList.Size = new System.Drawing.Size(321, 178);
            this.grdAvailableList.TabIndex = 1;
            this.grdAvailableList.Text = "pGrid1";
            this.grdAvailableList.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdAvailableList_BeforePopulate);
            this.grdAvailableList.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdAvailableList_FetchRowDone);
            this.grdAvailableList.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdAvailableList_AfterPopulate);
            this.grdAvailableList.SelectedIndexChanged += new Windows.Forms.GridClickedEventHandler(grdAvailableList_SelectedIndexChanged); //23981
            // 
            // colAvailableItemNo
            // 
            this.colAvailableItemNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colAvailableItemNo.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colAvailableItemNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colAvailableItemNo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colAvailableItemNo.PhoenixUIControl.InputMask = "999999999999";
            this.colAvailableItemNo.PhoenixUIControl.XmlTag = "Ptid";
            this.colAvailableItemNo.Title = "Item #";
            this.colAvailableItemNo.Visible = false;
            // 
            // colRowNo
            // 
            this.colRowNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRowNo.PhoenixUIControl.ObjectId = 11;
            this.colRowNo.PhoenixUIControl.XmlTag = "RowNum";
            this.colRowNo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colRowNo.Title = "Row #";
            this.colRowNo.Width = 40;
            // 
            // colLastActivityDt
            // 
            this.colLastActivityDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastActivityDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colLastActivityDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLastActivityDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colLastActivityDt.PhoenixUIControl.ObjectId = 12;
            this.colLastActivityDt.PhoenixUIControl.XmlTag = "LastActivityDt";
            this.colLastActivityDt.Title = "Date";
            this.colLastActivityDt.Width = 68;
            // 
            // colSerialNo
            // 
            this.colSerialNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSerialNo.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSerialNo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo.PhoenixUIControl.InputMask = "9999999999999999";
            this.colSerialNo.PhoenixUIControl.ObjectId = 13;
            this.colSerialNo.PhoenixUIControl.XmlTag = "SerialNo";
            this.colSerialNo.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colSerialNo.Title = "Serial #";
            this.colSerialNo.Width = 112;
            // 
            // colCurrentAmt
            // 
            this.colCurrentAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCurrentAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCurrentAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCurrentAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCurrentAmt.PhoenixUIControl.ObjectId = 14;
            this.colCurrentAmt.PhoenixUIControl.XmlTag = "InvItemAmt";
            this.colCurrentAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colCurrentAmt.Title = "Current Amt";
            this.colCurrentAmt.Width = 80;
            // 
            // colItemValue
            // 
            this.colItemValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colItemValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colItemValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colItemValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colItemValue.PhoenixUIControl.XmlTag = "ItemValue";
            this.colItemValue.Title = "Column";
            this.colItemValue.Visible = false;
            // 
            // colPacketItemValue
            // 
            this.colPacketItemValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketItemValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPacketItemValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketItemValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPacketItemValue.PhoenixUIControl.XmlTag = "PacketItemValue";
            this.colPacketItemValue.Title = "Column";
            this.colPacketItemValue.Visible = false;
            // 
            // colUpchargeAmt
            // 
            this.colUpchargeAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUpchargeAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUpchargeAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUpchargeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUpchargeAmt.PhoenixUIControl.XmlTag = "UpchargeAmt";
            this.colUpchargeAmt.Title = "Column";
            this.colUpchargeAmt.Visible = false;
            // 
            // colAvailableModified
            // 
            this.colAvailableModified.PhoenixUIControl.XmlTag = "Modified";
            this.colAvailableModified.Title = "Modified";
            this.colAvailableModified.Visible = false;
            // 
            // colTypeId
            // 
            this.colTypeId.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId.PhoenixUIControl.XmlTag = "TypeId";
            this.colTypeId.Title = "Column";
            this.colTypeId.Visible = false;
            // 
            // colPacketId
            // 
            this.colPacketId.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId.PhoenixUIControl.XmlTag = "PacketId";
            this.colPacketId.Title = "Column";
            this.colPacketId.Visible = false;
            // 
            // colClass
            // 
            this.colClass.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass.PhoenixUIControl.XmlTag = "Class";
            this.colClass.Title = "Column";
            this.colClass.Visible = false;
            // 
            // colLocation
            // 
            this.colLocation.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocation.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocation.PhoenixUIControl.XmlTag = "Location";
            this.colLocation.Title = "Column";
            this.colLocation.Visible = false;
            // 
            // colBranchNo
            // 
            this.colBranchNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo.Title = "Column";
            this.colBranchNo.Visible = false;
            // 
            // colDrawerNo
            // 
            this.colDrawerNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "Column";
            this.colDrawerNo.Visible = false;
            // 
            // colStatus
            // 
            this.colStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colStatus.PhoenixUIControl.XmlTag = "Status";
            this.colStatus.Title = "Column";
            this.colStatus.Visible = false;
            // 
            // colStatusSort
            // 
            this.colStatusSort.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStatusSort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colStatusSort.PhoenixUIControl.XmlTag = "StatusSort";
            this.colStatusSort.Title = "Column";
            this.colStatusSort.Visible = false;
            // 
            // colUseDrawerLevel
            // 
            this.colUseDrawerLevel.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colUseDrawerLevel.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colUseDrawerLevel.PhoenixUIControl.XmlTag = "UseDrawerLevel";
            this.colUseDrawerLevel.Title = "Column";
            this.colUseDrawerLevel.Visible = false;
            // 
            // colLocationSort
            // 
            this.colLocationSort.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLocationSort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colLocationSort.PhoenixUIControl.XmlTag = "LocationSort";
            this.colLocationSort.Title = "Column";
            this.colLocationSort.Visible = false;
            // 
            // colItemExists
            // 
            this.colItemExists.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemExists.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemExists.PhoenixUIControl.XmlTag = "ItemExists";
            this.colItemExists.Title = "Column";
            this.colItemExists.Visible = false;
            // 
            // colSoldAmt
            // 
            this.colSoldAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSoldAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colSoldAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSoldAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colSoldAmt.PhoenixUIControl.XmlTag = "SoldAmt";
            this.colSoldAmt.Title = "Column";
            this.colSoldAmt.Visible = false;
            // 
            // colPacketDesc
            // 
            this.colPacketDesc.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colPacketDesc.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colPacketDesc.PhoenixUIControl.XmlTag = "PacketDesc";
            this.colPacketDesc.Title = "Column";
            this.colPacketDesc.Visible = false;
            // 
            // frmInventoryItemPurchaseReturnDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.gbCounterInfo);
            this.Controls.Add(this.gbAccountInfo);
            this.Controls.Add(this.gbTypeInfo);
            this.EditRecordTitle = "Inventory Item Purchase/Return Detail";
            this.Name = "frmInventoryItemPurchaseReturnDetail";
            this.NewRecordTitle = "Inventory Item Purchase/Return Detail";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.ListWithSave;
            this.Text = "Inventory Item Purchase/Return Detail";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmInventoryItemPurchaseReturnDetail_PInitCompleteEvent);
            this.gbTypeInfo.ResumeLayout(false);
            this.gbTypeInfo.PerformLayout();
            this.gbAccountInfo.ResumeLayout(false);
            this.gbAccountInfo.PerformLayout();
            this.gbCounterInfo.ResumeLayout(false);
            this.gbCounterInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbTypeInfo;
        private Phoenix.Windows.Forms.PAction pbListView;
        private Windows.Forms.PdfStandard dfClass;
        private Windows.Forms.PLabelStandard lblClass;
        private Windows.Forms.PdfStandard dfPacketId;
        private Windows.Forms.PLabelStandard lblPacketId;
        private Windows.Forms.PLabelStandard lblTypeId;
        private Windows.Forms.PdfStandard dfTypeId;
        private Windows.Forms.PGroupBoxStandard gbAccountInfo;
        private Windows.Forms.PdfStandard dfName;
        private Windows.Forms.PLabelStandard lblName;
        private Windows.Forms.PdfStandard dfRimNo;
        private Windows.Forms.PLabelStandard lblRimNo;
        private Windows.Forms.PCheckBoxStandard cbNonCustomer;
        private Windows.Forms.PGroupBoxStandard gbCounterInfo;
        private Windows.Forms.PGrid grdPurchReturnList;
        private Windows.Forms.PGrid grdAvailableList;
        private Windows.Forms.PButtonStandard pbMoveLeft;
        private Windows.Forms.PButtonStandard pbMoveRight;
        private Windows.Forms.PRadioButtonStandard rbPercent;
        private Windows.Forms.PRadioButtonStandard rbAmount;
        private Windows.Forms.PdfCurrency dfTotalPurchAmt;
        private Windows.Forms.PLabelStandard lblTotalPurchAmt;
        private Windows.Forms.PdfStandard dfTotalDiscOvrd;
        private Windows.Forms.PLabelStandard lblTotalDiscOvrd;
        private Windows.Forms.PdfStandard dfTotalPurchCount;
        private Windows.Forms.PLabelStandard lblTotalPurchCount;
        private Windows.Forms.PdfCurrency dfTotalAvailAmt;
        private Windows.Forms.PLabelStandard lblTotalAvailAmt;
        private Windows.Forms.PdfStandard dfTotalAvailCount;
        private Windows.Forms.PLabelStandard lblTotalAvailCount;
        private Windows.Forms.PdfCurrency dfLocalTax;
        private Windows.Forms.PLabelStandard lblLocalTax;
        private Windows.Forms.PdfCurrency dfStateTax;
        private Windows.Forms.PLabelStandard lblStateTax;
        private Windows.Forms.PLabelStandard lblPurchReturn;
        private Windows.Forms.PLabelStandard lblAvailable;
        private Windows.Forms.PGridColumn colRowNo;
        private Windows.Forms.PGridColumn colLastActivityDt;
        private Windows.Forms.PGridColumn colSerialNo;
        private Windows.Forms.PGridColumn colCurrentAmt;
        private Windows.Forms.PGridColumn colRowNo1;
        private Windows.Forms.PGridColumn colLastActivityDt1;
        private Windows.Forms.PGridColumn colSerialNo1;
        private Windows.Forms.PGridColumn colCurrentAmt1;
        private Windows.Forms.PGridColumn colPurchReturnItemNo;
        private Windows.Forms.PGridColumn colAvailableItemNo;
        private Windows.Forms.PGridColumn colPurchReturnModified;
        private Windows.Forms.PGridColumn colAvailableModified;
        private Windows.Forms.PGridColumn colPacketItemValue;
        private Windows.Forms.PGridColumn colUpchargeAmt;
        private Windows.Forms.PGridColumn colItemValue;
        private Windows.Forms.PGridColumn colTypeId1;
        private Windows.Forms.PGridColumn colPacketId1;
        private Windows.Forms.PGridColumn colClass1;
        private Windows.Forms.PGridColumn colLocation1;
        private Windows.Forms.PGridColumn colBranchNo1;
        private Windows.Forms.PGridColumn colDrawerNo1;
        private Windows.Forms.PGridColumn colStatus1;
        private Windows.Forms.PGridColumn colStatusSort1;
        private Windows.Forms.PGridColumn colTypeId;
        private Windows.Forms.PGridColumn colPacketId;
        private Windows.Forms.PGridColumn colClass;
        private Windows.Forms.PGridColumn colLocation;
        private Windows.Forms.PGridColumn colBranchNo;
        private Windows.Forms.PGridColumn colDrawerNo;
        private Windows.Forms.PGridColumn colStatus;
        private Windows.Forms.PGridColumn colStatusSort;
        private Windows.Forms.PGridColumn colUseDrawerLevel1;
        private Windows.Forms.PGridColumn colUseDrawerLevel;
        private Windows.Forms.PGridColumn colLocationSort1;
        private Windows.Forms.PGridColumn colLocationSort;
        private Windows.Forms.PGridColumn colItemValue1;
        private Windows.Forms.PGridColumn colPacketItemValue1;
        private Windows.Forms.PGridColumn colUpchargeAmt1;
        private Windows.Forms.PGridColumn colItemExists1;
        private Windows.Forms.PGridColumn colItemExists;
        private Windows.Forms.PGridColumn colSoldAmt1;
        private Windows.Forms.PGridColumn colSoldAmt;
        private Windows.Forms.PGridColumn colPrevCurrentAmt1;
        private Windows.Forms.PdfStandard dfPacketDesc;
        private Windows.Forms.PGridColumn colPacketDesc;
        private Windows.Forms.PGridColumn colPacketDesc1;
    }
}