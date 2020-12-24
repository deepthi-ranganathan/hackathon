namespace phoenix.client.tlprinting
{
    partial class dlgPrintBalPrompt
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
            this.picQuestion = new System.Windows.Forms.PictureBox();
            this.lblInformation = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbAll = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbNone = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbSelect = new Phoenix.Windows.Forms.PButtonStandard();
            ((System.ComponentModel.ISupportInitialize)(this.picQuestion)).BeginInit();
            this.SuspendLayout();
            // 
            // picQuestion
            // 
            this.picQuestion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picQuestion.Location = new System.Drawing.Point(12, 0);
            this.picQuestion.Name = "picQuestion";
            this.picQuestion.Size = new System.Drawing.Size(46, 42);
            this.picQuestion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picQuestion.TabIndex = 10;
            this.picQuestion.TabStop = false;
            // 
            // lblInformation
            // 
            this.lblInformation.AutoEllipsis = true;
            this.lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInformation.Location = new System.Drawing.Point(65, 9);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.PhoenixUIControl.ObjectId = 1;
            this.lblInformation.Size = new System.Drawing.Size(285, 33);
            this.lblInformation.TabIndex = 11;
            this.lblInformation.Text = "Which balances do you wish to print on receipts?";
            this.lblInformation.WordWrap = true;
            // 
            // pbAll
            // 
            this.pbAll.Location = new System.Drawing.Point(79, 48);
            this.pbAll.Name = "pbAll";
            this.pbAll.PhoenixUIControl.ObjectId = 2;
            this.pbAll.Size = new System.Drawing.Size(75, 23);
            this.pbAll.TabIndex = 12;
            this.pbAll.Text = "&All";
            this.pbAll.Click += new System.EventHandler(this.pbAll_Click);
            this.pbAll.GotFocus += new System.EventHandler(pbAll_GotFocus);
            // 
            // pbNone
            // 
            this.pbNone.Location = new System.Drawing.Point(160, 48);
            this.pbNone.Name = "pbNone";
            this.pbNone.PhoenixUIControl.ObjectId = 3;
            this.pbNone.Size = new System.Drawing.Size(75, 23);
            this.pbNone.TabIndex = 13;
            this.pbNone.Text = "&None";
            this.pbNone.Click += new System.EventHandler(this.pbNone_Click);
            this.pbNone.GotFocus += new System.EventHandler(pbNone_GotFocus);
            // 
            // pbSelect
            // 
            this.pbSelect.Location = new System.Drawing.Point(241, 48);
            this.pbSelect.Name = "pbSelect";
            this.pbSelect.PhoenixUIControl.ObjectId = 4;
            this.pbSelect.Size = new System.Drawing.Size(75, 23);
            this.pbSelect.TabIndex = 14;
            this.pbSelect.Text = "&Select";
            this.pbSelect.Click += new System.EventHandler(this.pbSelect_Click);
            this.pbSelect.GotFocus += new System.EventHandler(pbSelect_GotFocus);
            // 
            // dlgPrintBalPrompt
            // 
            this.AcceptButton = this.pbAll;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 82);
            this.Controls.Add(this.pbSelect);
            this.Controls.Add(this.pbNone);
            this.Controls.Add(this.pbAll);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this.picQuestion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "dlgPrintBalPrompt";
            this.ScreenId = 3060;
            this.Text = "Phoenix Question";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.fwStandard_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.fwStandard_PInitBeginEvent);
            ((System.ComponentModel.ISupportInitialize)(this.picQuestion)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox picQuestion;
        private Phoenix.Windows.Forms.PLabelStandard lblInformation;
        private Phoenix.Windows.Forms.PButtonStandard pbAll;
        private Phoenix.Windows.Forms.PButtonStandard pbNone;
        private Phoenix.Windows.Forms.PButtonStandard pbSelect;
    }
}