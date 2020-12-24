namespace Phoenix.Client.Journal
{
    partial class frmTlCaptureImgViewer
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
            this.pbFront = new Phoenix.Windows.Forms.PAction();
            this.pbRear = new Phoenix.Windows.Forms.PAction();
            this.pbZoomIn = new Phoenix.Windows.Forms.PAction();
            this.pbZoomOut = new Phoenix.Windows.Forms.PAction();
            this.spltContainer = new System.Windows.Forms.SplitContainer();
            this.lblNoImage = new Phoenix.Windows.Forms.PLabelStandard();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.dfItems = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdItems = new MyPDataGridView();
            this.colType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAuxiliary = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRT = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAccount = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAmount = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colISN = new Phoenix.Windows.Forms.PDataGridViewColumn();
            ((System.ComponentModel.ISupportInitialize)(this.spltContainer)).BeginInit();
            this.spltContainer.Panel1.SuspendLayout();
            this.spltContainer.Panel2.SuspendLayout();
            this.spltContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.dfItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbFront,
            this.pbRear,
            this.pbZoomIn,
            this.pbZoomOut});
            // 
            // pbFront
            // 
            this.pbFront.LongText = "Front";
            this.pbFront.ObjectId = 10;
            this.pbFront.Tag = null;
            this.pbFront.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbFront_Click);
            // 
            // pbRear
            // 
            this.pbRear.LongText = "Rear";
            this.pbRear.ObjectId = 11;
            this.pbRear.Tag = null;
            this.pbRear.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRear_Click);
            // 
            // pbZoomIn
            // 
            this.pbZoomIn.LongText = "Zoom In";
            this.pbZoomIn.ObjectId = 12;
            this.pbZoomIn.Tag = null;
            this.pbZoomIn.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbZoomIn_Click);
            // 
            // pbZoomOut
            // 
            this.pbZoomOut.LongText = "Zoom Out";
            this.pbZoomOut.ObjectId = 13;
            this.pbZoomOut.Tag = null;
            this.pbZoomOut.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbZoomOut_Click);
            // 
            // spltContainer
            // 
            this.spltContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.spltContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltContainer.Location = new System.Drawing.Point(0, 0);
            this.spltContainer.Name = "spltContainer";
            this.spltContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltContainer.Panel1
            // 
            this.spltContainer.Panel1.AutoScroll = true;
            this.spltContainer.Panel1.Controls.Add(this.lblNoImage);
            this.spltContainer.Panel1.Controls.Add(this.picImage);
            // 
            // spltContainer.Panel2
            // 
            this.spltContainer.Panel2.Controls.Add(this.dfItems);
            this.spltContainer.Size = new System.Drawing.Size(690, 448);
            this.spltContainer.SplitterDistance = 260;
            this.spltContainer.TabIndex = 5;
            this.spltContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.spltContainer_SplitterMoved);
            // 
            // lblNoImage
            // 
            this.lblNoImage.AutoEllipsis = true;
            this.lblNoImage.AutoSize = true;
            this.lblNoImage.Location = new System.Drawing.Point(318, 123);
            this.lblNoImage.Name = "lblNoImage";
            this.lblNoImage.PhoenixUIControl.ObjectId = 15;
            this.lblNoImage.Size = new System.Drawing.Size(53, 13);
            this.lblNoImage.TabIndex = 7;
            this.lblNoImage.Text = "No Image";
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(7, 5);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(674, 248);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 6;
            this.picImage.TabStop = false;
            // 
            // dfItems
            // 
            this.dfItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dfItems.Controls.Add(this.grdItems);
            this.dfItems.Location = new System.Drawing.Point(5, 3);
            this.dfItems.Name = "dfItems";
            this.dfItems.Size = new System.Drawing.Size(678, 176);
            this.dfItems.TabIndex = 1;
            this.dfItems.TabStop = false;
            this.dfItems.Text = "Items";
            // 
            // grdItems
            // 
            this.grdItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colType,
            this.colAuxiliary,
            this.colRT,
            this.colAccount,
            this.colTranCode,
            this.colAmount,
            this.colISN});
            this.grdItems.IsMaxNumRowsCustomized = false;
            this.grdItems.Location = new System.Drawing.Point(6, 19);
            this.grdItems.Name = "grdItems";
            this.grdItems.Size = new System.Drawing.Size(664, 148);
            this.grdItems.TabIndex = 0;
            this.grdItems.Click += new System.EventHandler(this.grdItems_Click);
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.PhoenixUIControl.ObjectId = 3;
            this.colType.Text = "";
            this.colType.Title = "Type";
            this.colType.Width = 108;
            // 
            // colAuxiliary
            // 
            this.colAuxiliary.HeaderText = "Auxiliary";
            this.colAuxiliary.Name = "colAuxiliary";
            this.colAuxiliary.PhoenixUIControl.ObjectId = 4;
            this.colAuxiliary.Text = "";
            this.colAuxiliary.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAuxiliary.Title = "Auxiliary";
            this.colAuxiliary.Width = 116;
            // 
            // colRT
            // 
            this.colRT.HeaderText = "RT";
            this.colRT.Name = "colRT";
            this.colRT.PhoenixUIControl.ObjectId = 6;
            this.colRT.Text = "";
            this.colRT.Title = "RT";
            this.colRT.Width = 108;
            // 
            // colAccount
            // 
            this.colAccount.HeaderText = "Account";
            this.colAccount.Name = "colAccount";
            this.colAccount.PhoenixUIControl.ObjectId = 7;
            this.colAccount.Text = "";
            this.colAccount.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAccount.Title = "Account";
            this.colAccount.Width = 112;
            // 
            // colTranCode
            // 
            this.colTranCode.HeaderText = "Tran Code";
            this.colTranCode.Name = "colTranCode";
            this.colTranCode.PhoenixUIControl.ObjectId = 8;
            this.colTranCode.Text = "";
            this.colTranCode.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTranCode.Title = "Tran Code";
            this.colTranCode.Width = 108;
            // 
            // colAmount
            // 
            this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.ObjectId = 9;
            this.colAmount.Text = "";
            this.colAmount.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAmount.Title = "Amount";
            this.colAmount.Width = 108;
            // 
            // colISN
            // 
            this.colISN.HeaderText = "ISN";
            this.colISN.Name = "colISN";
            this.colISN.PhoenixUIControl.ObjectId = 14;
            this.colISN.Text = "";
            this.colISN.Title = "ISN";
            this.colISN.Visible = false;
            // 
            // frmTlCaptureImgViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.spltContainer);
            this.Name = "frmTlCaptureImgViewer";
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.Text = "Teller Capture Image Viewer [%1!]";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.form_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCaptureImgViewer_PInitCompleteEvent);
            this.spltContainer.Panel1.ResumeLayout(false);
            this.spltContainer.Panel1.PerformLayout();
            this.spltContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spltContainer)).EndInit();
            this.spltContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.dfItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdItems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PAction pbFront;
        private Phoenix.Windows.Forms.PAction pbRear;
        private Phoenix.Windows.Forms.PAction pbZoomIn;
        private Phoenix.Windows.Forms.PAction pbZoomOut;
        private System.Windows.Forms.SplitContainer spltContainer;
        private Windows.Forms.PGroupBoxStandard dfItems;
        private MyPDataGridView grdItems;
        private Windows.Forms.PDataGridViewColumn colType;
        private Windows.Forms.PDataGridViewColumn colAuxiliary;
        private Windows.Forms.PDataGridViewColumn colRT;
        private Windows.Forms.PDataGridViewColumn colAccount;
        private Windows.Forms.PDataGridViewColumn colTranCode;
        private Windows.Forms.PDataGridViewColumn colAmount;
        private Windows.Forms.PDataGridViewColumn colISN;
        private Windows.Forms.PLabelStandard lblNoImage;
        private System.Windows.Forms.PictureBox picImage;
    }
}