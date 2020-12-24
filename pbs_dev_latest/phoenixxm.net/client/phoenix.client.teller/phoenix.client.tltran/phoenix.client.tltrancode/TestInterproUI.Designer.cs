namespace Phoenix.Windows.Client
{
    partial class TestInterproUI
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
            this.dfResponse = new Phoenix.Windows.Forms.PdfStandard();
            this.pbProcess = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbAddNext = new Phoenix.Windows.Forms.PButtonStandard();
            this.SuspendLayout();
            // 
            // dfResponse
            // 
            this.dfResponse.Location = new System.Drawing.Point(21, 14);
            this.dfResponse.Multiline = true;
            this.dfResponse.Name = "dfResponse";
            this.dfResponse.Size = new System.Drawing.Size(443, 306);
            this.dfResponse.TabIndex = 0;
            // 
            // pbProcess
            // 
            this.pbProcess.Location = new System.Drawing.Point(491, 43);
            this.pbProcess.Name = "pbProcess";
            this.pbProcess.Size = new System.Drawing.Size(75, 23);
            this.pbProcess.TabIndex = 1;
            this.pbProcess.Text = "Process";
            this.pbProcess.Click += new System.EventHandler(this.pbProcess_Click);
            // 
            // pbAddNext
            // 
            this.pbAddNext.Location = new System.Drawing.Point(490, 84);
            this.pbAddNext.Name = "pbAddNext";
            this.pbAddNext.Size = new System.Drawing.Size(76, 23);
            this.pbAddNext.TabIndex = 2;
            this.pbAddNext.Text = "Add Next";
            this.pbAddNext.Click += new System.EventHandler(this.pbAddNext_Click);
            // 
            // TestInterproUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.pbAddNext);
            this.Controls.Add(this.pbProcess);
            this.Controls.Add(this.dfResponse);
            this.Name = "TestInterproUI";
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.Text = "TestInterproUI";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.TestInterproUI_PInitCompleteEvent);
            this.PShowCompletedEvent += new System.EventHandler(this.TestInterproUI_PShowCompletedEvent);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Phoenix.Windows.Forms.PdfStandard dfResponse;
        private Phoenix.Windows.Forms.PButtonStandard pbProcess;
        private Phoenix.Windows.Forms.PButtonStandard pbAddNext;
    }
}