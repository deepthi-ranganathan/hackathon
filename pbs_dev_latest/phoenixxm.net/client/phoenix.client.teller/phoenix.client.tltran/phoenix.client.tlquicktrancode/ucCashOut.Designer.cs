namespace Phoenix.Client.Teller
{
    partial class ucCashOut
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
            this.lblCashOut = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalDebits = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalCredits = new DevExpress.XtraEditors.LabelControl();
            this.lblTotalDiff = new DevExpress.XtraEditors.LabelControl();
            this.dfTotalDebits = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfTotalCR = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfDifference = new Phoenix.Windows.Forms.PdfCurrency();
            this.dfCashOut = new Phoenix.Windows.Forms.PdfCurrency();
            this.cbDenominate = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.SuspendLayout();
            // 
            // lblCashOut
            // 
            this.lblCashOut.AllowHtmlString = true;
            this.lblCashOut.Appearance.Options.UseTextOptions = true;
            this.lblCashOut.Location = new System.Drawing.Point(7, 8);
            this.lblCashOut.Margin = new System.Windows.Forms.Padding(2);
            this.lblCashOut.Name = "lblCashOut";
            this.lblCashOut.Size = new System.Drawing.Size(49, 13);
            this.lblCashOut.TabIndex = 0;
            this.lblCashOut.Text = "Cash O<u>u</u>t:";
            // 
            // lblTotalDebits
            // 
            this.lblTotalDebits.Location = new System.Drawing.Point(236, 8);
            this.lblTotalDebits.Margin = new System.Windows.Forms.Padding(2);
            this.lblTotalDebits.Name = "lblTotalDebits";
            this.lblTotalDebits.Size = new System.Drawing.Size(56, 13);
            this.lblTotalDebits.TabIndex = 3;
            this.lblTotalDebits.Text = "Total Debit:";
            // 
            // lblTotalCredits
            // 
            this.lblTotalCredits.Location = new System.Drawing.Point(377, 8);
            this.lblTotalCredits.Margin = new System.Windows.Forms.Padding(2);
            this.lblTotalCredits.Name = "lblTotalCredits";
            this.lblTotalCredits.Size = new System.Drawing.Size(60, 13);
            this.lblTotalCredits.TabIndex = 5;
            this.lblTotalCredits.Text = "Total Credit:";
            // 
            // lblTotalDiff
            // 
            this.lblTotalDiff.Location = new System.Drawing.Point(520, 8);
            this.lblTotalDiff.Margin = new System.Windows.Forms.Padding(2);
            this.lblTotalDiff.Name = "lblTotalDiff";
            this.lblTotalDiff.Size = new System.Drawing.Size(54, 13);
            this.lblTotalDiff.TabIndex = 7;
            this.lblTotalDiff.Text = "Difference:";
            // 
            // dfTotalDebits
            // 
            this.dfTotalDebits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDebits.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalDebits.Location = new System.Drawing.Point(297, 8);
            this.dfTotalDebits.Name = "dfTotalDebits";
            this.dfTotalDebits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalDebits.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalDebits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalDebits.PhoenixUIControl.MaxPrecision = 14;
            this.dfTotalDebits.PreviousValue = null;
            this.dfTotalDebits.ReadOnly = true;
            this.dfTotalDebits.Size = new System.Drawing.Size(75, 13);
            this.dfTotalDebits.TabIndex = 4;
            this.dfTotalDebits.TabStop = false;
            // 
            // dfTotalCR
            // 
            this.dfTotalCR.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCR.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalCR.Location = new System.Drawing.Point(440, 8);
            this.dfTotalCR.Name = "dfTotalCR";
            this.dfTotalCR.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCR.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalCR.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalCR.PhoenixUIControl.MaxPrecision = 14;
            this.dfTotalCR.PreviousValue = null;
            this.dfTotalCR.ReadOnly = true;
            this.dfTotalCR.Size = new System.Drawing.Size(75, 13);
            this.dfTotalCR.TabIndex = 6;
            this.dfTotalCR.TabStop = false;
            // 
            // dfDifference
            // 
            this.dfDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDifference.Location = new System.Drawing.Point(579, 8);
            this.dfDifference.Name = "dfDifference";
            this.dfDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDifference.PhoenixUIControl.MaxPrecision = 14;
            this.dfDifference.PreviousValue = null;
            this.dfDifference.ReadOnly = true;
            this.dfDifference.Size = new System.Drawing.Size(95, 13);
            this.dfDifference.TabIndex = 8;
            this.dfDifference.TabStop = false;
            // 
            // dfCashOut
            // 
            this.dfCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOut.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOut.Location = new System.Drawing.Point(61, 4);
            this.dfCashOut.Name = "dfCashOut";
            this.dfCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOut.PhoenixUIControl.MaxPrecision = 14;
            this.dfCashOut.PhoenixUIControl.XmlTag = "";
            this.dfCashOut.PreviousValue = null;
            this.dfCashOut.Size = new System.Drawing.Size(75, 20);
            this.dfCashOut.TabIndex = 1;
            // 
            // cbDenominate
            // 
            this.cbDenominate.AutoSize = true;
            this.cbDenominate.BackColor = System.Drawing.SystemColors.Control;
            this.cbDenominate.Checked = true;
            this.cbDenominate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDenominate.Location = new System.Drawing.Point(141, 5);
            this.cbDenominate.Name = "cbDenominate";
            this.cbDenominate.Size = new System.Drawing.Size(89, 18);
            this.cbDenominate.TabIndex = 2;
            this.cbDenominate.Text = "Denominate";
            this.cbDenominate.UseVisualStyleBackColor = false;
            this.cbDenominate.Value = null;
            // 
            // ucCashOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbDenominate);
            this.Controls.Add(this.dfCashOut);
            this.Controls.Add(this.dfDifference);
            this.Controls.Add(this.dfTotalCR);
            this.Controls.Add(this.dfTotalDebits);
            this.Controls.Add(this.lblTotalDiff);
            this.Controls.Add(this.lblTotalCredits);
            this.Controls.Add(this.lblTotalDebits);
            this.Controls.Add(this.lblCashOut);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ucCashOut";
            this.Size = new System.Drawing.Size(682, 28);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl lblCashOut;
        private DevExpress.XtraEditors.LabelControl lblTotalDebits;
        private DevExpress.XtraEditors.LabelControl lblTotalCredits;
        private DevExpress.XtraEditors.LabelControl lblTotalDiff;
        private Phoenix.Windows.Forms.PdfCurrency dfTotalDebits;
        private Phoenix.Windows.Forms.PdfCurrency dfTotalCR;
        private Phoenix.Windows.Forms.PdfCurrency dfDifference;
        private Phoenix.Windows.Forms.PdfCurrency dfCashOut;
        private Windows.Forms.PCheckBoxStandard cbDenominate;
    }
}
