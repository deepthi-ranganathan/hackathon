namespace Phoenix.Client.Teller
{
    partial class dlgPEDSignatureCapture
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
            this.dfCustomerInfo = new Phoenix.Windows.Forms.PDfDisplay();
            this.picSignature = new Phoenix.Shared.Windows.PImageBox();
            this.pbOk = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbRetrySignature = new Phoenix.Windows.Forms.PButtonStandard();
            this.lblCustomerInfo = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbSignatureInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).BeginInit();
            this.gbSignatureInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // dfCustomerInfo
            // 
            this.dfCustomerInfo.Location = new System.Drawing.Point(63, 20);
            this.dfCustomerInfo.Name = "dfCustomerInfo";
            this.dfCustomerInfo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfCustomerInfo.PreviousValue = null;
            this.dfCustomerInfo.Size = new System.Drawing.Size(461, 13);
            this.dfCustomerInfo.TabIndex = 1;
            // 
            // picSignature
            // 
            this.picSignature.Location = new System.Drawing.Point(18, 75);
            this.picSignature.Name = "picSignature";
            this.picSignature.Size = new System.Drawing.Size(493, 115);
            this.picSignature.TabIndex = 3;
            this.picSignature.TabStop = false;
            // 
            // pbOk
            // 
            this.pbOk.Location = new System.Drawing.Point(18, 205);
            this.pbOk.Name = "pbOk";
            this.pbOk.Size = new System.Drawing.Size(91, 23);
            this.pbOk.TabIndex = 2;
            this.pbOk.Text = "&Ok";
            this.pbOk.Click += new System.EventHandler(this.pbOk_Click);
            // 
            // pbRetrySignature
            // 
            this.pbRetrySignature.Location = new System.Drawing.Point(420, 205);
            this.pbRetrySignature.Name = "pbRetrySignature";
            this.pbRetrySignature.Size = new System.Drawing.Size(91, 23);
            this.pbRetrySignature.TabIndex = 3;
            this.pbRetrySignature.Text = "&Retry Signature";
            this.pbRetrySignature.Click += new System.EventHandler(this.pbRetrySignature_Click);
            // 
            // lblCustomerInfo
            // 
            this.lblCustomerInfo.AutoEllipsis = true;
            this.lblCustomerInfo.BackColor = System.Drawing.SystemColors.Control;
            this.lblCustomerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblCustomerInfo.Location = new System.Drawing.Point(6, 16);
            this.lblCustomerInfo.Name = "lblCustomerInfo";
            this.lblCustomerInfo.Size = new System.Drawing.Size(51, 20);
            this.lblCustomerInfo.TabIndex = 0;
            this.lblCustomerInfo.Text = "Sign:";
            // 
            // gbSignatureInfo
            // 
            this.gbSignatureInfo.Controls.Add(this.dfCustomerInfo);
            this.gbSignatureInfo.Controls.Add(this.lblCustomerInfo);
            this.gbSignatureInfo.Controls.Add(this.picSignature);
            this.gbSignatureInfo.Controls.Add(this.pbRetrySignature);
            this.gbSignatureInfo.Controls.Add(this.pbOk);
            this.gbSignatureInfo.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.gbSignatureInfo.Location = new System.Drawing.Point(4, 4);
            this.gbSignatureInfo.Name = "gbSignatureInfo";
            this.gbSignatureInfo.Size = new System.Drawing.Size(531, 240);
            this.gbSignatureInfo.TabIndex = 0;
            this.gbSignatureInfo.TabStop = false;
            // 
            // dlgPEDSignatureCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 246);
            this.Controls.Add(this.gbSignatureInfo);
            this.EditRecordTitle = "Signature Capture";
            this.Name = "dlgPEDSignatureCapture";
            this.NewRecordTitle = "Signature Capture";
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.Text = "Signature Capture";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgPEDSignatureCapture_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgPEDSignatureCapture_PInitCompleteEvent);
            ((System.ComponentModel.ISupportInitialize)(this.picSignature)).EndInit();
            this.gbSignatureInfo.ResumeLayout(false);
            this.gbSignatureInfo.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Windows.Forms.PDfDisplay dfCustomerInfo;
        private Shared.Windows.PImageBox picSignature;
        private Windows.Forms.PButtonStandard pbOk;
        private Windows.Forms.PButtonStandard pbRetrySignature;
        private Windows.Forms.PLabelStandard lblCustomerInfo;
        private Windows.Forms.PGroupBoxStandard gbSignatureInfo;

    }
}