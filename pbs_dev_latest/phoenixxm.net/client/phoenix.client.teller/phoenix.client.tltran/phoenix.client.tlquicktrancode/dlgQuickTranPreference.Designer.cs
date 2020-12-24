namespace Phoenix.Client.Teller
{
    partial class dlgQuickTranPreference
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
            this.gbPreference = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
            this.rbRestoreOriginalLayout = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbSaveCurrentLayout = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.gbPreference.SuspendLayout();
            this.pPanel1.SuspendLayout();
            this.SuspendLayout();
            this.ScreenType = Windows.Forms.PScreenType.Editable;
            // 
            // ActionManager
            // 
            //this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {});
            // 
            // gbPreference
            // 
            this.gbPreference.Controls.Add(this.pPanel1);
            this.gbPreference.Location = new System.Drawing.Point(8, 7);
            this.gbPreference.Margin = new System.Windows.Forms.Padding(4);
            this.gbPreference.Name = "gbPreference";
            this.gbPreference.Padding = new System.Windows.Forms.Padding(4);
            this.gbPreference.Size = new System.Drawing.Size(267, 94);
            this.gbPreference.TabIndex = 2;
            this.gbPreference.TabStop = false;
            this.gbPreference.Text = "Select Action";
            // 
            // pPanel1
            // 
            this.pPanel1.Controls.Add(this.rbRestoreOriginalLayout);
            this.pPanel1.Controls.Add(this.rbSaveCurrentLayout);
            this.pPanel1.Location = new System.Drawing.Point(8, 23);
            this.pPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.Size = new System.Drawing.Size(230, 63);
            this.pPanel1.TabIndex = 1;
            this.pPanel1.TabStop = true;
            // 
            // rbRestoreOriginalLayout
            // 
            this.rbRestoreOriginalLayout.AutoSize = true;
            this.rbRestoreOriginalLayout.BackColor = System.Drawing.SystemColors.Control;
            this.rbRestoreOriginalLayout.Description = null;
            this.rbRestoreOriginalLayout.IsMaster = true;
            this.rbRestoreOriginalLayout.Location = new System.Drawing.Point(3, 31);
            this.rbRestoreOriginalLayout.Margin = new System.Windows.Forms.Padding(4);
            this.rbRestoreOriginalLayout.Name = "rbRestoreOriginalLayout";
            this.rbRestoreOriginalLayout.Size = new System.Drawing.Size(178, 22);
            this.rbRestoreOriginalLayout.TabIndex = 0;
            this.rbRestoreOriginalLayout.Text = "Restore Default Layout";
            this.rbRestoreOriginalLayout.UseVisualStyleBackColor = false;
            // 
            // rbSaveCurrentLayout
            // 
            this.rbSaveCurrentLayout.BackColor = System.Drawing.SystemColors.Control;
            this.rbSaveCurrentLayout.Description = null;
            this.rbSaveCurrentLayout.Location = new System.Drawing.Point(4, 0);
            this.rbSaveCurrentLayout.Margin = new System.Windows.Forms.Padding(4);
            this.rbSaveCurrentLayout.Name = "rbSaveCurrentLayout";
            this.rbSaveCurrentLayout.Size = new System.Drawing.Size(248, 22);
            this.rbSaveCurrentLayout.TabIndex = 2;
            this.rbSaveCurrentLayout.Text = "Save Current Layout";
            this.rbSaveCurrentLayout.UseVisualStyleBackColor = false;
            // 
            // dlgQuickTranPreference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 113);
            this.Controls.Add(this.gbPreference);
            this.EditRecordTitle = "Layout Preference";
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "dlgQuickTranPreference";
            this.NewRecordTitle = "Layout Preference";
            this.Text = "Layout Preference";
            this.gbPreference.ResumeLayout(false);
            this.pPanel1.ResumeLayout(false);
            this.pPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Windows.Forms.PGroupBoxStandard gbPreference;
        private Windows.Forms.PPanel pPanel1;
        private Windows.Forms.PRadioButtonStandard rbRestoreOriginalLayout;
        private Windows.Forms.PRadioButtonStandard rbSaveCurrentLayout;
    }
}