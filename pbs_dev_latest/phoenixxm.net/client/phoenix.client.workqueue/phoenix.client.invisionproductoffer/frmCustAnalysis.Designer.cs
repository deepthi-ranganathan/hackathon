namespace Phoenix.Client.WorkQueue
{
    partial class frmCustAnalysis
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
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdCustAnalDetails = new Phoenix.Windows.Forms.PDataGridView();
            this.colDescription = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colUseCaseID = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.pbDetail = new Phoenix.Windows.Forms.PAction();
            this.pGroupBoxStandard1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCustAnalDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbDetail});
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.grdCustAnalDetails);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 0);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(700, 352);
            this.pGroupBoxStandard1.TabIndex = 2;
            this.pGroupBoxStandard1.TabStop = false;
            this.pGroupBoxStandard1.Text = "Customer Analysis";
            // 
            // grdCustAnalDetails
            // 
            this.grdCustAnalDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescription,
            this.colUseCaseID});
            this.grdCustAnalDetails.IsDataGridReadOnly = false;
            this.grdCustAnalDetails.IsMaxNumRowsCustomized = false;
            this.grdCustAnalDetails.Location = new System.Drawing.Point(8, 24);
            this.grdCustAnalDetails.Name = "grdCustAnalDetails";
            this.grdCustAnalDetails.Size = new System.Drawing.Size(672, 320);
            this.grdCustAnalDetails.TabIndex = 0;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.HeaderText = "Description";
            this.colDescription.MinimumWidth = 6;
            this.colDescription.Name = "colDescription";
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Text = "";
            this.colDescription.Title = "Description";
            // 
            // colUseCaseID
            // 
            this.colUseCaseID.HeaderText = "colUseCaseID";
            this.colUseCaseID.MinimumWidth = 6;
            this.colUseCaseID.Name = "colUseCaseID";
            this.colUseCaseID.PhoenixUIControl.XmlTag = "CaseId";
            this.colUseCaseID.Text = "";
            this.colUseCaseID.Title = "colUseCaseID";
            this.colUseCaseID.Visible = false;
            this.colUseCaseID.Width = 125;
            // 
            // pbDetail
            // 
            this.pbDetail.LongText = "&Detail...";
            this.pbDetail.Name = "pbDetail";
            this.pbDetail.ShortText = "&Detail...";
            this.pbDetail.Tag = null;
            this.pbDetail.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDetail_Click);
            // 
            // frmCustAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 352);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.EditRecordTitle = "Customer Analysis";
            this.Name = "frmCustAnalysis";
            this.NewRecordTitle = "Customer Analysis";
            this.Text = "Customer Analysis";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.pGroupBoxStandard1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCustAnalDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
        private Windows.Forms.PDataGridView grdCustAnalDetails;
        private Windows.Forms.PAction pbDetail;
        private Windows.Forms.PDataGridViewColumn colDescription;
        private Windows.Forms.PDataGridViewColumn colUseCaseID;
    }
}