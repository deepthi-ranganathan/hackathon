namespace Phoenix.Client.Teller
{
    partial class ucCashIn
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
            this.lblCashIn = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashIn = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfChecksIn = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfMakeAvail = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblMakeAvail = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNoOfItems = new Phoenix.Windows.Forms.PdfStandard();
            this.lblNoOfItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbWaiveSignature = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.Panel();
            this.picDrawer = new System.Windows.Forms.PictureBox();
            this.picTCD = new System.Windows.Forms.PictureBox();
            this.lblChecksIn = new Phoenix.Windows.Forms.PLabelStandard();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDrawer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTCD)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCashIn
            // 
            this.lblCashIn.AutoEllipsis = true;
            this.lblCashIn.Location = new System.Drawing.Point(2, 2);
            this.lblCashIn.Margin = new System.Windows.Forms.Padding(2);
            this.lblCashIn.Name = "lblCashIn";
            this.lblCashIn.Size = new System.Drawing.Size(50, 20);
            this.lblCashIn.TabIndex = 0;
            this.lblCashIn.Text = "Cash In:";
            // 
            // dfCashIn
            // 
            this.dfCashIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIn.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashIn.Location = new System.Drawing.Point(90, 2);
            this.dfCashIn.Name = "dfCashIn";
            this.dfCashIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashIn.PhoenixUIControl.MaxPrecision = 14;
            this.dfCashIn.PhoenixUIControl.XmlTag = "";
            this.dfCashIn.PreviousValue = null;
            this.dfCashIn.Size = new System.Drawing.Size(100, 20);
            this.dfCashIn.TabIndex = 1;
            // 
            // dfChecksIn
            // 
            this.dfChecksIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChecksIn.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfChecksIn.Location = new System.Drawing.Point(268, 2);
            this.dfChecksIn.Name = "dfChecksIn";
            this.dfChecksIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfChecksIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfChecksIn.PhoenixUIControl.MaxPrecision = 14;
            this.dfChecksIn.PreviousValue = null;
            this.dfChecksIn.Size = new System.Drawing.Size(100, 20);
            this.dfChecksIn.TabIndex = 3;
            // 
            // dfMakeAvail
            // 
            this.dfMakeAvail.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMakeAvail.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfMakeAvail.Location = new System.Drawing.Point(578, 2);
            this.dfMakeAvail.Name = "dfMakeAvail";
            this.dfMakeAvail.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMakeAvail.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfMakeAvail.PhoenixUIControl.MaxPrecision = 14;
            this.dfMakeAvail.PreviousValue = null;
            this.dfMakeAvail.Size = new System.Drawing.Size(73, 20);
            this.dfMakeAvail.TabIndex = 7;
            // 
            // lblMakeAvail
            // 
            this.lblMakeAvail.AutoEllipsis = true;
            this.lblMakeAvail.Location = new System.Drawing.Point(509, 2);
            this.lblMakeAvail.Margin = new System.Windows.Forms.Padding(2);
            this.lblMakeAvail.Name = "lblMakeAvail";
            this.lblMakeAvail.Size = new System.Drawing.Size(64, 20);
            this.lblMakeAvail.TabIndex = 6;
            this.lblMakeAvail.Text = "Available:";
            // 
            // dfNoOfItems
            // 
            this.dfNoOfItems.Location = new System.Drawing.Point(449, 2);
            this.dfNoOfItems.Name = "dfNoOfItems";
            this.dfNoOfItems.PreviousValue = null;
            this.dfNoOfItems.Size = new System.Drawing.Size(53, 20);
            this.dfNoOfItems.TabIndex = 5;
            // 
            // lblNoOfItems
            // 
            this.lblNoOfItems.AutoEllipsis = true;
            this.lblNoOfItems.Location = new System.Drawing.Point(376, 2);
            this.lblNoOfItems.Margin = new System.Windows.Forms.Padding(2);
            this.lblNoOfItems.Name = "lblNoOfItems";
            this.lblNoOfItems.Size = new System.Drawing.Size(68, 20);
            this.lblNoOfItems.TabIndex = 4;
            this.lblNoOfItems.Text = "# Of Items:";
            // 
            // cbWaiveSignature
            // 
            this.cbWaiveSignature.AutoSize = true;
            this.cbWaiveSignature.Location = new System.Drawing.Point(665, 4);
            this.cbWaiveSignature.Name = "cbWaiveSignature";
            this.cbWaiveSignature.Size = new System.Drawing.Size(105, 17);
            this.cbWaiveSignature.TabIndex = 8;
            this.cbWaiveSignature.Text = "Waive Signature";
            this.cbWaiveSignature.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.lblCashIn);
            this.flowLayoutPanel1.Controls.Add(this.picDrawer);
            this.flowLayoutPanel1.Controls.Add(this.picTCD);
            this.flowLayoutPanel1.Controls.Add(this.dfCashIn);
            this.flowLayoutPanel1.Controls.Add(this.lblChecksIn);
            this.flowLayoutPanel1.Controls.Add(this.dfChecksIn);
            this.flowLayoutPanel1.Controls.Add(this.lblNoOfItems);
            this.flowLayoutPanel1.Controls.Add(this.dfNoOfItems);
            this.flowLayoutPanel1.Controls.Add(this.lblMakeAvail);
            this.flowLayoutPanel1.Controls.Add(this.dfMakeAvail);
            this.flowLayoutPanel1.Controls.Add(this.cbWaiveSignature);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(794, 33);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // picDrawer
            // 
            this.picDrawer.Location = new System.Drawing.Point(57, 2);
            this.picDrawer.Name = "picDrawer";
            this.picDrawer.Size = new System.Drawing.Size(24, 20);
            this.picDrawer.TabIndex = 10;
            this.picDrawer.TabStop = false;
            this.picDrawer.Visible = false;
            // 
            // picTCD
            // 
            this.picTCD.Location = new System.Drawing.Point(57, 2);
            this.picTCD.Name = "picTCD";
            this.picTCD.Size = new System.Drawing.Size(24, 20);
            this.picTCD.TabIndex = 9;
            this.picTCD.TabStop = false;
            // 
            // lblChecksIn
            // 
            this.lblChecksIn.AutoEllipsis = true;
            this.lblChecksIn.Location = new System.Drawing.Point(199, 2);
            this.lblChecksIn.Margin = new System.Windows.Forms.Padding(2);
            this.lblChecksIn.Name = "lblChecksIn";
            this.lblChecksIn.Size = new System.Drawing.Size(64, 20);
            this.lblChecksIn.TabIndex = 2;
            this.lblChecksIn.Text = "Checks In:";
            // 
            // ucCashIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucCashIn";
            this.Size = new System.Drawing.Size(794, 33);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDrawer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTCD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Phoenix.Windows.Forms.PLabelStandard lblCashIn;
        private Phoenix.Windows.Forms.PdfCurrency dfCashIn;
        private Phoenix.Windows.Forms.PdfCurrency dfChecksIn;
        private Phoenix.Windows.Forms.PdfCurrency dfMakeAvail;
        private Phoenix.Windows.Forms.PLabelStandard lblMakeAvail;
        private Phoenix.Windows.Forms.PdfStandard dfNoOfItems;
        private Phoenix.Windows.Forms.PLabelStandard lblNoOfItems;
        private System.Windows.Forms.CheckBox cbWaiveSignature;
        private System.Windows.Forms.Panel flowLayoutPanel1;
        private Phoenix.Windows.Forms.PLabelStandard lblChecksIn;
        private System.Windows.Forms.PictureBox picTCD;
        private System.Windows.Forms.PictureBox picDrawer;
    }
}
