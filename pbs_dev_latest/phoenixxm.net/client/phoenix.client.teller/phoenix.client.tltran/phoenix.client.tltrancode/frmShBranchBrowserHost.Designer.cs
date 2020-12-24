namespace Phoenix.Windows.Client
{
    partial class frmShBranchBrowserHost
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
            this.webBrowserShBranch = new Phoenix.Windows.Forms.ExtendedWebBrowser();
            this.SuspendLayout();
            // 
            // webBrowserShBranch
            // 
            this.webBrowserShBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserShBranch.Location = new System.Drawing.Point(0, 0);
            this.webBrowserShBranch.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserShBranch.Name = "webBrowserShBranch";
            this.webBrowserShBranch.Size = new System.Drawing.Size(792, 566);
            this.webBrowserShBranch.TabIndex = 0;
            // 
            // frmShBranchBrowserHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(792, 566);
            this.Controls.Add(this.webBrowserShBranch);
            this.Name = "frmShBranchBrowserHost";
            this.Text = "frmShBranchBrowserHost";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmShBranchBrowserHost_PInitBeginEvent);
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.ExtendedWebBrowser webBrowserShBranch;
    }
}