namespace Phoenix.Client.Teller
{
    partial class dlgShBranchTCDOptions
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
            this.cbTCR = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbTCD = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbDenominate = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbOptions = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbTCR
            // 
            this.cbTCR.AutoSize = true;
            this.cbTCR.Checked = true;
            this.cbTCR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTCR.Location = new System.Drawing.Point(12, 16);
            this.cbTCR.Name = "cbTCR";
            this.cbTCR.PhoenixUIControl.ObjectId = 2;
            this.cbTCR.Size = new System.Drawing.Size(132, 18);
            this.cbTCR.TabIndex = 0;
            this.cbTCR.Text = "Deposit Cash In &TCR";
            this.cbTCR.Value = null;
            // 
            // cbTCD
            // 
            this.cbTCD.AutoSize = true;
            this.cbTCD.Checked = true;
            this.cbTCD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTCD.Location = new System.Drawing.Point(12, 40);
            this.cbTCD.Name = "cbTCD";
            this.cbTCD.PhoenixUIControl.ObjectId = 3;
            this.cbTCD.Size = new System.Drawing.Size(162, 18);
            this.cbTCD.TabIndex = 1;
            this.cbTCD.Text = "Dispense Cash Out Of TC&D";
            this.cbTCD.Value = null;
            this.cbTCD.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbTCD_PhoenixUICheckedChangedEvent);
            // 
            // cbDenominate
            // 
            this.cbDenominate.AutoSize = true;
            this.cbDenominate.Checked = true;
            this.cbDenominate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDenominate.Location = new System.Drawing.Point(12, 64);
            this.cbDenominate.Name = "cbDenominate";
            this.cbDenominate.PhoenixUIControl.ObjectId = 4;
            this.cbDenominate.Size = new System.Drawing.Size(161, 18);
            this.cbDenominate.TabIndex = 2;
            this.cbDenominate.Text = "De&nominate TCD Dispense";
            this.cbDenominate.Value = null;
            this.cbDenominate.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbDenominate_PhoenixUICheckedChangedEvent);
            // 
            // gbOptions
            // 
            this.gbOptions.Controls.Add(this.cbDenominate);
            this.gbOptions.Controls.Add(this.cbTCD);
            this.gbOptions.Controls.Add(this.cbTCR);
            this.gbOptions.Location = new System.Drawing.Point(4, 0);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.PhoenixUIControl.ObjectId = 1;
            this.gbOptions.Size = new System.Drawing.Size(256, 88);
            this.gbOptions.TabIndex = 3;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Deposit/Dispense Options";
            // 
            // dlgShBranchTCDOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 91);
            this.Controls.Add(this.gbOptions);
            this.Name = "dlgShBranchTCDOptions";
            this.Text = "TCD Options";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgShBranchTCDOptions_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgShBranchTCDOptions_PInitBeginEvent);
            this.gbOptions.ResumeLayout(false);
            this.gbOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PCheckBoxStandard cbTCR;
        private Phoenix.Windows.Forms.PCheckBoxStandard cbTCD;
        private Phoenix.Windows.Forms.PCheckBoxStandard cbDenominate;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbOptions;
    }
}