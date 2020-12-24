namespace phoenix.client.tlprinting
{
    partial class frmTlBalPrint
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
            this.grdBalancesToPrint = new Phoenix.Windows.Forms.PDataGridView();
            this.colTc = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colDescription = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAccount = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCustName = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRelationship = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colCustNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colPrintBal = new Phoenix.Windows.Forms.PDataGridViewCheckBoxColumn();
            this.colIsEditable = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colSequenceNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colSubSequenceNo = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colOrigPrintCustBal = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.lblOptNotAvail = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDummyDoNotDelete = new Phoenix.Windows.Forms.PdfStandard();
            ((System.ComponentModel.ISupportInitialize)(this.grdBalancesToPrint)).BeginInit();
            this.SuspendLayout();
            // 
            // grdBalancesToPrint
            // 
            this.grdBalancesToPrint.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTc,
            this.colDescription,
            this.colAccount,
            this.colCustName,
            this.colRelationship,
            this.colCustNo,
            this.colPrintBal,
            this.colIsEditable,
            this.colSequenceNo,
            this.colSubSequenceNo,
            this.colOrigPrintCustBal});
            this.grdBalancesToPrint.LinesInHeader = 2;
            this.grdBalancesToPrint.Location = new System.Drawing.Point(2, 4);
            this.grdBalancesToPrint.Name = "grdBalancesToPrint";
            this.grdBalancesToPrint.Size = new System.Drawing.Size(686, 383);
            this.grdBalancesToPrint.TabIndex = 0;
            this.grdBalancesToPrint.Text = "Balance(s) to Print";
            this.grdBalancesToPrint.CurrentCellDirtyStateChanged += new System.EventHandler(grdBalancesToPrint_CurrentCellDirtyStateChanged);
            this.grdBalancesToPrint.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(grdBalancesToPrint_CellClick);
            // 
            // colTc
            // 
            this.colTc.Name = "colTc";
            this.colTc.PhoenixUIControl.ObjectId = 2;
            this.colTc.PhoenixUIControl.XmlTag = "";
            this.colTc.Width = 40;
            // 
            // colDescription
            // 
            this.colDescription.Name = "colDescription";
            this.colDescription.PhoenixUIControl.ObjectId = 3;
            this.colDescription.PhoenixUIControl.XmlTag = "0";
            this.colDescription.Width = 120;
            // 
            // colAccount
            // 
            this.colAccount.Name = "colAccount";
            this.colAccount.PhoenixUIControl.ObjectId = 4;
            this.colAccount.PhoenixUIControl.XmlTag = "1";
            this.colAccount.Width = 120;
            // 
            // colCustName
            // 
            this.colCustName.Name = "colCustName";
            this.colCustName.PhoenixUIControl.ObjectId = 5;
            this.colCustName.PhoenixUIControl.XmlTag = "2";
            this.colCustName.Width = 180;
            // 
            // colRelationship
            // 
            this.colRelationship.Name = "colRelationship";
            this.colRelationship.PhoenixUIControl.ObjectId = 6;
            this.colRelationship.PhoenixUIControl.XmlTag = "3";
            this.colRelationship.Width = 80;
            // 
            // colCustNo
            // 
            this.colCustNo.Name = "colCustNo";
            this.colCustNo.PhoenixUIControl.ObjectId = 7;
            this.colCustNo.PhoenixUIControl.XmlTag = "4";
            this.colCustNo.Width = 90;
            // 
            // colPrintBal
            // 
            this.colPrintBal.Name = "colPrintBal";
            this.colPrintBal.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.Enable;
            this.colPrintBal.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Show;
            this.colPrintBal.PhoenixUIControl.ObjectId = 8;
            this.colPrintBal.PhoenixUIControl.XmlTag = "5";
            this.colPrintBal.ReadOnly = false;
            this.colPrintBal.Width = 50;
            this.colPrintBal.CheckedChanged += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.colPrintBal_CheckedChanged);
            // 
            // colIsEditable
            // 
            this.colIsEditable.Name = "colIsEditable";
            this.colIsEditable.Visible = false;
            this.colIsEditable.Width = 5;
            // 
            // colSequenceNo
            // 
            this.colSequenceNo.Name = "colSequenceNo";
            this.colSequenceNo.PhoenixUIControl.XmlTag = "6";
            this.colSequenceNo.Visible = false;
            this.colSequenceNo.Width = 50;
            // 
            // colSubSequenceNo
            // 
            this.colSubSequenceNo.Name = "colSubSequenceNo";
            this.colSubSequenceNo.PhoenixUIControl.XmlTag = "7";
            this.colSubSequenceNo.Visible = false;
            // 
            // colOrigPrintCustBal
            // 
            this.colOrigPrintCustBal.Name = "colOrigPrintCustBal";
            this.colOrigPrintCustBal.PhoenixUIControl.XmlTag = "9";
            this.colOrigPrintCustBal.Visible = false;
            this.colOrigPrintCustBal.Width = 5;
            // 
            // lblOptNotAvail
            // 
            this.lblOptNotAvail.AutoEllipsis = true;
            this.lblOptNotAvail.Location = new System.Drawing.Point(10, 419);
            this.lblOptNotAvail.Name = "lblOptNotAvail";
            this.lblOptNotAvail.Size = new System.Drawing.Size(302, 20);
            this.lblOptNotAvail.TabIndex = 1;
            // 
            // dfDummyDoNotDelete
            // 
            this.dfDummyDoNotDelete.Location = new System.Drawing.Point(511, 416);
            this.dfDummyDoNotDelete.Name = "dfDummyDoNotDelete";
            this.dfDummyDoNotDelete.PhoenixUIControl.XmlTag = "EnablePrtPrompt";
            this.dfDummyDoNotDelete.Size = new System.Drawing.Size(60, 20);
            this.dfDummyDoNotDelete.TabIndex = 2;
            this.dfDummyDoNotDelete.Visible = false;
            // 
            // frmTlBalPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.dfDummyDoNotDelete);
            this.Controls.Add(this.lblOptNotAvail);
            this.Controls.Add(this.grdBalancesToPrint);
            this.Name = "frmTlBalPrint";
            this.ScreenId = 3059;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.ListWithSave;
            this.Text = "Balance(s) to Print";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.fwStandard_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.fwStandard_PInitBeginEvent);
            ((System.ComponentModel.ISupportInitialize)(this.grdBalancesToPrint)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion

        private Phoenix.Windows.Forms.PDataGridView grdBalancesToPrint;
        private Phoenix.Windows.Forms.PLabelStandard lblOptNotAvail;
        private Phoenix.Windows.Forms.PDataGridViewColumn colTc;
        private Phoenix.Windows.Forms.PDataGridViewColumn colDescription;
        private Phoenix.Windows.Forms.PDataGridViewColumn colAccount;
        private Phoenix.Windows.Forms.PDataGridViewColumn colCustName;
        private Phoenix.Windows.Forms.PDataGridViewColumn colRelationship;
        private Phoenix.Windows.Forms.PDataGridViewColumn colCustNo;
        private Phoenix.Windows.Forms.PDataGridViewCheckBoxColumn colPrintBal;
        private Phoenix.Windows.Forms.PDataGridViewColumn colIsEditable;
        private Phoenix.Windows.Forms.PdfStandard dfDummyDoNotDelete;
        private Phoenix.Windows.Forms.PDataGridViewColumn colSequenceNo;
        private Phoenix.Windows.Forms.PDataGridViewColumn colSubSequenceNo;
        private Phoenix.Windows.Forms.PDataGridViewColumn colOrigPrintCustBal;
    }
}