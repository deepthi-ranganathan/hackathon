
namespace Phoenix.Client.WorkQueue
{
    partial class frmOdCare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOdCare));
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.txtDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbListView = new Phoenix.Windows.Forms.PAction();
            this.flashObject = new Phoenix.Interop.Flash.AxHost.AxShockwaveFlash();
            this.pbChart = new Phoenix.Windows.Forms.PAction();
            this.pGroupBoxStandard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flashObject)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbListView,
            this.pbChart});
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.txtDescription);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 0);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(899, 57);
            this.pGroupBoxStandard1.TabIndex = 2;
            this.pGroupBoxStandard1.TabStop = false;
            // 
            // txtDescription
            // 
            this.txtDescription.AutoEllipsis = true;
            this.txtDescription.Location = new System.Drawing.Point(8, 19);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(791, 20);
            this.txtDescription.TabIndex = 4;
            // 
            // pbListView
            // 
            this.pbListView.LongText = "ListView";
            this.pbListView.Name = null;
            this.pbListView.Tag = null;
            // 
            // flashObject
            // 
            this.flashObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(176)))));
            this.flashObject.Enabled = true;
            this.flashObject.Location = new System.Drawing.Point(5, 63);
            this.flashObject.Name = "flashObject";
            this.flashObject.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("flashObject.OcxState")));
            this.flashObject.Size = new System.Drawing.Size(898, 455);
            this.flashObject.TabIndex = 901;
            // 
            // pbChart
            // 
            this.pbChart.LongText = "Workflow";
            this.pbChart.Name = "pbChart";
            this.pbChart.ShortText = "Workflow";
            this.pbChart.Tag = null;
            // 
            // frmOdCare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.flashObject);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.EditRecordTitle = "Account Analysis";
            this.Name = "frmOdCare";
            this.NewRecordTitle = "Account Analysis";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.Text = "Account Analysis";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmOdCare_PInitCompleteEvent);
            this.pGroupBoxStandard1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flashObject)).EndInit();
            this.ResumeLayout(false);

        }

       





        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
        private Phoenix.Windows.Forms.PLabelStandard txtDescription;
        private Phoenix.Windows.Forms.PAction pbListView;
        public Interop.Flash.AxHost.AxShockwaveFlash flashObject;
        private Windows.Forms.PAction pbChart;
    }
}