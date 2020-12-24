namespace phoenix.client.tlprinting
{
    partial class frmMTForm
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
            this.gbMTTranInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.rbLast = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbFirst = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.dfNoCopies = new Phoenix.Windows.Forms.PdfStandard();
            this.cmbFooter = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cmbHeader = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblPrintOrder = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNoCopies = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblFooter = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblHeader = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbMTTranInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbMTTranInfo
            // 
            this.gbMTTranInfo.Controls.Add(this.rbLast);
            this.gbMTTranInfo.Controls.Add(this.rbFirst);
            this.gbMTTranInfo.Controls.Add(this.dfNoCopies);
            this.gbMTTranInfo.Controls.Add(this.cmbFooter);
            this.gbMTTranInfo.Controls.Add(this.cmbHeader);
            this.gbMTTranInfo.Controls.Add(this.lblPrintOrder);
            this.gbMTTranInfo.Controls.Add(this.lblNoCopies);
            this.gbMTTranInfo.Controls.Add(this.lblFooter);
            this.gbMTTranInfo.Controls.Add(this.lblHeader);
            this.gbMTTranInfo.Location = new System.Drawing.Point(3, 0);
            this.gbMTTranInfo.Name = "gbMTTranInfo";
            this.gbMTTranInfo.PhoenixUIControl.ObjectId = 1;
            this.gbMTTranInfo.Size = new System.Drawing.Size(684, 271);
            this.gbMTTranInfo.TabIndex = 0;
            this.gbMTTranInfo.TabStop = false;
            this.gbMTTranInfo.Text = "Multiple Transaction Printing Information";
            // 
            // rbLast
            // 
            this.rbLast.AutoSize = true;
            this.rbLast.Description = null;
            this.rbLast.Location = new System.Drawing.Point(290, 176);
            this.rbLast.Name = "rbLast";
            this.rbLast.PhoenixUIControl.ObjectId = 6;
            this.rbLast.PhoenixUIControl.XmlTag = "";
            this.rbLast.Size = new System.Drawing.Size(51, 18);
            this.rbLast.TabIndex = 9;
            this.rbLast.Text = "Last";
            this.rbLast.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbLast_PhoenixUICheckedChangedEvent);
            // 
            // rbFirst
            // 
            this.rbFirst.AutoSize = true;
            this.rbFirst.Description = null;
            this.rbFirst.IsMaster = true;
            this.rbFirst.Location = new System.Drawing.Point(208, 176);
            this.rbFirst.Name = "rbFirst";
            this.rbFirst.PhoenixUIControl.ObjectId = 5;
            this.rbFirst.PhoenixUIControl.XmlTag = "";
            this.rbFirst.Size = new System.Drawing.Size(50, 18);
            this.rbFirst.TabIndex = 8;
            this.rbFirst.Text = "First";
            this.rbFirst.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbFirst_PhoenixUICheckedChangedEvent);
            // 
            // dfNoCopies
            // 
            this.dfNoCopies.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoCopies.Location = new System.Drawing.Point(208, 130);
            this.dfNoCopies.MaxLength = 2;
            this.dfNoCopies.Name = "dfNoCopies";
            this.dfNoCopies.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoCopies.PhoenixUIControl.MaxPrecision = 2;
            this.dfNoCopies.PhoenixUIControl.ObjectId = 4;
            this.dfNoCopies.PhoenixUIControl.XmlTag = "NoCopies";
            this.dfNoCopies.Size = new System.Drawing.Size(100, 20);
            this.dfNoCopies.TabIndex = 7;
            this.dfNoCopies.Leave += new System.EventHandler(this.dfNoCopies_Leave);
            // 
            // cmbFooter
            // 
            this.cmbFooter.Location = new System.Drawing.Point(208, 86);
            this.cmbFooter.Name = "cmbFooter";
            this.cmbFooter.PhoenixUIControl.ObjectId = 3;
            this.cmbFooter.PhoenixUIControl.XmlTag = "FooterFormId";
            this.cmbFooter.Size = new System.Drawing.Size(121, 21);
            this.cmbFooter.TabIndex = 6;
            this.cmbFooter.Value = null;
            this.cmbFooter.SelectedIndexChanged += new System.EventHandler(this.cmbFooter_SelectedIndexChanged);
            // 
            // cmbHeader
            // 
            this.cmbHeader.Location = new System.Drawing.Point(208, 42);
            this.cmbHeader.Name = "cmbHeader";
            this.cmbHeader.PhoenixUIControl.ObjectId = 2;
            this.cmbHeader.PhoenixUIControl.XmlTag = "HeaderFormId";
            this.cmbHeader.Size = new System.Drawing.Size(121, 21);
            this.cmbHeader.TabIndex = 5;
            this.cmbHeader.Value = null;
            this.cmbHeader.SelectedIndexChanged += new System.EventHandler(this.cmbHeader_SelectedIndexChanged);
            // 
            // lblPrintOrder
            // 
            this.lblPrintOrder.AutoEllipsis = true;
            this.lblPrintOrder.Location = new System.Drawing.Point(27, 174);
            this.lblPrintOrder.Name = "lblPrintOrder";
            this.lblPrintOrder.PhoenixUIControl.ObjectId = 10;
            this.lblPrintOrder.Size = new System.Drawing.Size(100, 20);
            this.lblPrintOrder.TabIndex = 3;
            this.lblPrintOrder.Text = "Print Order:";
            // 
            // lblNoCopies
            // 
            this.lblNoCopies.AutoEllipsis = true;
            this.lblNoCopies.Location = new System.Drawing.Point(27, 130);
            this.lblNoCopies.Name = "lblNoCopies";
            this.lblNoCopies.PhoenixUIControl.ObjectId = 4;
            this.lblNoCopies.Size = new System.Drawing.Size(145, 20);
            this.lblNoCopies.TabIndex = 2;
            this.lblNoCopies.Text = "# of Copies to Print:";
            // 
            // lblFooter
            // 
            this.lblFooter.AutoEllipsis = true;
            this.lblFooter.Location = new System.Drawing.Point(27, 87);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.PhoenixUIControl.ObjectId = 3;
            this.lblFooter.Size = new System.Drawing.Size(100, 20);
            this.lblFooter.TabIndex = 1;
            this.lblFooter.Text = "Footer:";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoEllipsis = true;
            this.lblHeader.Location = new System.Drawing.Point(27, 43);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.PhoenixUIControl.ObjectId = 2;
            this.lblHeader.Size = new System.Drawing.Size(100, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Header:";
            // 
            // frmMTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbMTTranInfo);
            this.Name = "frmMTForm";
            this.ScreenId = 3042;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.Text = "frmMTForm";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.fwStandard_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.fwStandard_PInitBeginEvent);
            this.gbMTTranInfo.ResumeLayout(false);
            this.gbMTTranInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbMTTranInfo;
        private Phoenix.Windows.Forms.PLabelStandard lblHeader;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbHeader;
        private Phoenix.Windows.Forms.PLabelStandard lblPrintOrder;
        private Phoenix.Windows.Forms.PLabelStandard lblNoCopies;
        private Phoenix.Windows.Forms.PLabelStandard lblFooter;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbLast;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbFirst;
        private Phoenix.Windows.Forms.PdfStandard dfNoCopies;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbFooter;
    }
}