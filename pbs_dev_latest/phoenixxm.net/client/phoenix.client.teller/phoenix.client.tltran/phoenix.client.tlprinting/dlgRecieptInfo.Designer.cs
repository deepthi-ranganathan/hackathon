namespace phoenix.client.tlprinting
{
    partial class dlgRecieptInfo
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
            this.gbTransactionInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfHiddenTextNote3 = new Phoenix.Windows.Forms.PdfStandard();
            this.lblMulti = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCustomer = new Phoenix.Windows.Forms.PdfStandard();
            this.lblCustomer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAccount = new Phoenix.Windows.Forms.PdfStandard();
            this.lblAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfAmount = new Phoenix.Windows.Forms.PdfStandard();
            this.lblAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTransaction = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTransaction = new Phoenix.Windows.Forms.PdfStandard();
            this.gbReceiptDeliveryInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfHiddenTextReceipt = new Phoenix.Windows.Forms.PdfStandard();
            this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
            this.rbPrint = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbEmail = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbPrintAndEmail = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.dfNewEmail = new Phoenix.Windows.Forms.PdfStandard();
            this.lblNewEmail = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbEmail = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblEmail = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblReceiptDeliveryDefault = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbContinue = new Phoenix.Windows.Forms.PAction();
            this.gbPrintingFormsInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfHiddenTextPrint = new Phoenix.Windows.Forms.PdfStandard();
            this.dfPbLastLine = new Phoenix.Windows.Forms.PdfStandard();
            this.lblNumberofLastPrintedLine = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbWosaService = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblLogicalService = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfForm = new Phoenix.Windows.Forms.PdfStandard();
            this.lblForm = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblText = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNote = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNote2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbReprintLast = new Phoenix.Windows.Forms.PAction();
            this.pbSkip = new Phoenix.Windows.Forms.PAction();
            this.pbArchiveOnly = new Phoenix.Windows.Forms.PAction();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.dfHiddenTextNote = new Phoenix.Windows.Forms.PdfStandard();
            this.dfHiddenTextNote2 = new Phoenix.Windows.Forms.PdfStandard();
            this.gbTransactionInformation.SuspendLayout();
            this.gbReceiptDeliveryInformation.SuspendLayout();
            this.pPanel1.SuspendLayout();
            this.gbPrintingFormsInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbContinue,
            this.pbReprintLast,
            this.pbSkip,
            this.pbArchiveOnly,
            this.pbCancel});
            // 
            // gbTransactionInformation
            // 
            this.gbTransactionInformation.Controls.Add(this.dfHiddenTextNote3);
            this.gbTransactionInformation.Controls.Add(this.lblMulti);
            this.gbTransactionInformation.Controls.Add(this.dfCustomer);
            this.gbTransactionInformation.Controls.Add(this.lblCustomer);
            this.gbTransactionInformation.Controls.Add(this.dfAccount);
            this.gbTransactionInformation.Controls.Add(this.lblAccount);
            this.gbTransactionInformation.Controls.Add(this.dfAmount);
            this.gbTransactionInformation.Controls.Add(this.lblAmount);
            this.gbTransactionInformation.Controls.Add(this.lblTransaction);
            this.gbTransactionInformation.Controls.Add(this.dfTransaction);
            this.gbTransactionInformation.Location = new System.Drawing.Point(4, 0);
            this.gbTransactionInformation.Name = "gbTransactionInformation";
            this.gbTransactionInformation.Size = new System.Drawing.Size(412, 124);
            this.gbTransactionInformation.TabIndex = 0;
            this.gbTransactionInformation.TabStop = false;
            this.gbTransactionInformation.Text = "Transaction Information";
            // 
            // dfHiddenTextNote3
            // 
            this.dfHiddenTextNote3.Location = new System.Drawing.Point(370, 93);
            this.dfHiddenTextNote3.Name = "dfHiddenTextNote3";
            this.dfHiddenTextNote3.PhoenixUIControl.ObjectId = 27;
            this.dfHiddenTextNote3.ReadOnly = true;
            this.dfHiddenTextNote3.Size = new System.Drawing.Size(10, 20);
            this.dfHiddenTextNote3.TabIndex = 9;
            this.dfHiddenTextNote3.Visible = false;
            // 
            // lblMulti
            // 
            this.lblMulti.AutoEllipsis = true;
            this.lblMulti.Location = new System.Drawing.Point(138, 93);
            this.lblMulti.Name = "lblMulti";
            this.lblMulti.Size = new System.Drawing.Size(266, 20);
            this.lblMulti.TabIndex = 8;
            this.lblMulti.Text = "*First Transaction of a Multiple Transaction Receipt.";
            this.lblMulti.WordWrap = true;
            // 
            // dfCustomer
            // 
            this.dfCustomer.Location = new System.Drawing.Point(138, 17);
            this.dfCustomer.Name = "dfCustomer";
            this.dfCustomer.PhoenixUIControl.ObjectId = 5;
            this.dfCustomer.Size = new System.Drawing.Size(266, 20);
            this.dfCustomer.TabIndex = 7;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoEllipsis = true;
            this.lblCustomer.Location = new System.Drawing.Point(4, 17);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.PhoenixUIControl.ObjectId = 5;
            this.lblCustomer.Size = new System.Drawing.Size(80, 20);
            this.lblCustomer.TabIndex = 6;
            this.lblCustomer.Text = "Customer:";
            // 
            // dfAccount
            // 
            this.dfAccount.Location = new System.Drawing.Point(138, 68);
            this.dfAccount.Name = "dfAccount";
            this.dfAccount.PhoenixUIControl.ObjectId = 4;
            this.dfAccount.Size = new System.Drawing.Size(110, 20);
            this.dfAccount.TabIndex = 5;
            // 
            // lblAccount
            // 
            this.lblAccount.AutoEllipsis = true;
            this.lblAccount.Location = new System.Drawing.Point(4, 68);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.PhoenixUIControl.ObjectId = 4;
            this.lblAccount.Size = new System.Drawing.Size(80, 20);
            this.lblAccount.TabIndex = 4;
            this.lblAccount.Text = "Account:";
            // 
            // dfAmount
            // 
            this.dfAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmount.Location = new System.Drawing.Point(306, 69);
            this.dfAmount.Name = "dfAmount";
            this.dfAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfAmount.PhoenixUIControl.ObjectId = 3;
            this.dfAmount.Size = new System.Drawing.Size(98, 20);
            this.dfAmount.TabIndex = 3;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoEllipsis = true;
            this.lblAmount.Location = new System.Drawing.Point(257, 68);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.PhoenixUIControl.ObjectId = 3;
            this.lblAmount.Size = new System.Drawing.Size(46, 20);
            this.lblAmount.TabIndex = 2;
            this.lblAmount.Text = "Amount:";
            // 
            // lblTransaction
            // 
            this.lblTransaction.AutoEllipsis = true;
            this.lblTransaction.Location = new System.Drawing.Point(4, 43);
            this.lblTransaction.Name = "lblTransaction";
            this.lblTransaction.PhoenixUIControl.ObjectId = 2;
            this.lblTransaction.Size = new System.Drawing.Size(80, 20);
            this.lblTransaction.TabIndex = 0;
            this.lblTransaction.Text = "Transaction:";
            // 
            // dfTransaction
            // 
            this.dfTransaction.Location = new System.Drawing.Point(138, 43);
            this.dfTransaction.Name = "dfTransaction";
            this.dfTransaction.PhoenixUIControl.ObjectId = 2;
            this.dfTransaction.Size = new System.Drawing.Size(266, 20);
            this.dfTransaction.TabIndex = 1;
            // 
            // gbReceiptDeliveryInformation
            // 
            this.gbReceiptDeliveryInformation.Controls.Add(this.dfHiddenTextReceipt);
            this.gbReceiptDeliveryInformation.Controls.Add(this.pPanel1);
            this.gbReceiptDeliveryInformation.Controls.Add(this.dfNewEmail);
            this.gbReceiptDeliveryInformation.Controls.Add(this.lblNewEmail);
            this.gbReceiptDeliveryInformation.Controls.Add(this.cmbEmail);
            this.gbReceiptDeliveryInformation.Controls.Add(this.lblEmail);
            this.gbReceiptDeliveryInformation.Controls.Add(this.lblReceiptDeliveryDefault);
            this.gbReceiptDeliveryInformation.Location = new System.Drawing.Point(4, 130);
            this.gbReceiptDeliveryInformation.Name = "gbReceiptDeliveryInformation";
            this.gbReceiptDeliveryInformation.Size = new System.Drawing.Size(412, 94);
            this.gbReceiptDeliveryInformation.TabIndex = 1;
            this.gbReceiptDeliveryInformation.TabStop = false;
            this.gbReceiptDeliveryInformation.Text = "Receipt Delivery Information";
            // 
            // dfHiddenTextReceipt
            // 
            this.dfHiddenTextReceipt.Location = new System.Drawing.Point(370, 44);
            this.dfHiddenTextReceipt.Name = "dfHiddenTextReceipt";
            this.dfHiddenTextReceipt.PhoenixUIControl.ObjectId = 7;
            this.dfHiddenTextReceipt.ReadOnly = true;
            this.dfHiddenTextReceipt.Size = new System.Drawing.Size(10, 20);
            this.dfHiddenTextReceipt.TabIndex = 5;
            this.dfHiddenTextReceipt.Visible = false;
            // 
            // pPanel1
            // 
            this.pPanel1.Controls.Add(this.rbPrint);
            this.pPanel1.Controls.Add(this.rbEmail);
            this.pPanel1.Controls.Add(this.rbPrintAndEmail);
            this.pPanel1.Location = new System.Drawing.Point(136, 16);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.Size = new System.Drawing.Size(270, 22);
            this.pPanel1.TabIndex = 1;
            this.pPanel1.TabStop = true;
            // 
            // rbPrint
            // 
            this.rbPrint.AutoSize = true;
            this.rbPrint.BackColor = System.Drawing.SystemColors.Control;
            this.rbPrint.Description = null;
            this.rbPrint.IsMaster = true;
            this.rbPrint.Location = new System.Drawing.Point(2, 1);
            this.rbPrint.Name = "rbPrint";
            this.rbPrint.Size = new System.Drawing.Size(46, 17);
            this.rbPrint.TabIndex = 0;
            this.rbPrint.Text = "Print";
            this.rbPrint.UseVisualStyleBackColor = false;
            this.rbPrint.CheckedChanged += new System.EventHandler(this.rbPrint_CheckedChanged);
            // 
            // rbEmail
            // 
            this.rbEmail.AutoSize = true;
            this.rbEmail.BackColor = System.Drawing.SystemColors.Control;
            this.rbEmail.Description = null;
            this.rbEmail.Location = new System.Drawing.Point(70, 1);
            this.rbEmail.Name = "rbEmail";
            this.rbEmail.Size = new System.Drawing.Size(50, 17);
            this.rbEmail.TabIndex = 1;
            this.rbEmail.Text = "Email";
            this.rbEmail.UseVisualStyleBackColor = false;
            this.rbEmail.CheckedChanged += new System.EventHandler(this.rbEmail_CheckedChanged);
            // 
            // rbPrintAndEmail
            // 
            this.rbPrintAndEmail.BackColor = System.Drawing.SystemColors.Control;
            this.rbPrintAndEmail.Description = null;
            this.rbPrintAndEmail.Location = new System.Drawing.Point(148, 2);
            this.rbPrintAndEmail.Name = "rbPrintAndEmail";
            this.rbPrintAndEmail.Size = new System.Drawing.Size(122, 18);
            this.rbPrintAndEmail.TabIndex = 2;
            this.rbPrintAndEmail.Text = "Print and Email";
            this.rbPrintAndEmail.UseVisualStyleBackColor = false;
            this.rbPrintAndEmail.CheckedChanged += new System.EventHandler(this.rbPrintAndEmail_CheckedChanged);
            // 
            // dfNewEmail
            // 
            this.dfNewEmail.Location = new System.Drawing.Point(138, 68);
            this.dfNewEmail.Name = "dfNewEmail";
            this.dfNewEmail.PhoenixUIControl.ObjectId = 12;
            this.dfNewEmail.Size = new System.Drawing.Size(266, 20);
            this.dfNewEmail.TabIndex = 4;
            this.dfNewEmail.Leave += new System.EventHandler(this.dfNewEmail_Leave);
            // 
            // lblNewEmail
            // 
            this.lblNewEmail.AutoEllipsis = true;
            this.lblNewEmail.Location = new System.Drawing.Point(4, 68);
            this.lblNewEmail.Name = "lblNewEmail";
            this.lblNewEmail.PhoenixUIControl.ObjectId = 12;
            this.lblNewEmail.Size = new System.Drawing.Size(110, 20);
            this.lblNewEmail.TabIndex = 3;
            this.lblNewEmail.Text = "New Email Address:";
            // 
            // cmbEmail
            // 
            this.cmbEmail.Location = new System.Drawing.Point(138, 44);
            this.cmbEmail.Name = "cmbEmail";
            this.cmbEmail.PhoenixUIControl.ObjectId = 11;
            this.cmbEmail.Size = new System.Drawing.Size(266, 21);
            this.cmbEmail.TabIndex = 2;
            this.cmbEmail.Value = null;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoEllipsis = true;
            this.lblEmail.Location = new System.Drawing.Point(4, 42);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.PhoenixUIControl.ObjectId = 11;
            this.lblEmail.Size = new System.Drawing.Size(44, 20);
            this.lblEmail.TabIndex = 1;
            this.lblEmail.Text = "Email:";
            // 
            // lblReceiptDeliveryDefault
            // 
            this.lblReceiptDeliveryDefault.AutoEllipsis = true;
            this.lblReceiptDeliveryDefault.Location = new System.Drawing.Point(4, 17);
            this.lblReceiptDeliveryDefault.Name = "lblReceiptDeliveryDefault";
            this.lblReceiptDeliveryDefault.PhoenixUIControl.ObjectId = 7;
            this.lblReceiptDeliveryDefault.Size = new System.Drawing.Size(130, 20);
            this.lblReceiptDeliveryDefault.TabIndex = 0;
            this.lblReceiptDeliveryDefault.Text = "Receipt Delivery Default:";
            // 
            // pbContinue
            // 
            this.pbContinue.LongText = "Continue  (F2)";
            this.pbContinue.Name = "pbContinue";
            this.pbContinue.ObjectId = 20;
            this.pbContinue.Shortcut = System.Windows.Forms.Keys.F2;
            this.pbContinue.ShortText = "Continue  (F2)";
            this.pbContinue.Tag = null;
            this.pbContinue.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbContinue_Click);
            // 
            // gbPrintingFormsInformation
            // 
            this.gbPrintingFormsInformation.Controls.Add(this.dfHiddenTextPrint);
            this.gbPrintingFormsInformation.Controls.Add(this.dfPbLastLine);
            this.gbPrintingFormsInformation.Controls.Add(this.lblNumberofLastPrintedLine);
            this.gbPrintingFormsInformation.Controls.Add(this.cmbWosaService);
            this.gbPrintingFormsInformation.Controls.Add(this.lblLogicalService);
            this.gbPrintingFormsInformation.Controls.Add(this.dfForm);
            this.gbPrintingFormsInformation.Controls.Add(this.lblForm);
            this.gbPrintingFormsInformation.Controls.Add(this.lblText);
            this.gbPrintingFormsInformation.Location = new System.Drawing.Point(4, 230);
            this.gbPrintingFormsInformation.Name = "gbPrintingFormsInformation";
            this.gbPrintingFormsInformation.Size = new System.Drawing.Size(412, 130);
            this.gbPrintingFormsInformation.TabIndex = 2;
            this.gbPrintingFormsInformation.TabStop = false;
            this.gbPrintingFormsInformation.Text = "Printing Forms Information";
            // 
            // dfHiddenTextPrint
            // 
            this.dfHiddenTextPrint.Location = new System.Drawing.Point(370, 16);
            this.dfHiddenTextPrint.Name = "dfHiddenTextPrint";
            this.dfHiddenTextPrint.PhoenixUIControl.ObjectId = 14;
            this.dfHiddenTextPrint.ReadOnly = true;
            this.dfHiddenTextPrint.Size = new System.Drawing.Size(10, 20);
            this.dfHiddenTextPrint.TabIndex = 1;
            this.dfHiddenTextPrint.Visible = false;
            // 
            // dfPbLastLine
            // 
            this.dfPbLastLine.Location = new System.Drawing.Point(360, 104);
            this.dfPbLastLine.Name = "dfPbLastLine";
            this.dfPbLastLine.PhoenixUIControl.ObjectId = 17;
            this.dfPbLastLine.Size = new System.Drawing.Size(44, 20);
            this.dfPbLastLine.TabIndex = 7;
            // 
            // lblNumberofLastPrintedLine
            // 
            this.lblNumberofLastPrintedLine.AutoEllipsis = true;
            this.lblNumberofLastPrintedLine.Location = new System.Drawing.Point(4, 104);
            this.lblNumberofLastPrintedLine.Name = "lblNumberofLastPrintedLine";
            this.lblNumberofLastPrintedLine.PhoenixUIControl.ObjectId = 17;
            this.lblNumberofLastPrintedLine.Size = new System.Drawing.Size(148, 20);
            this.lblNumberofLastPrintedLine.TabIndex = 6;
            this.lblNumberofLastPrintedLine.Text = "Number of Last Printed Line:";
            // 
            // cmbWosaService
            // 
            this.cmbWosaService.Location = new System.Drawing.Point(138, 80);
            this.cmbWosaService.Name = "cmbWosaService";
            this.cmbWosaService.PhoenixUIControl.ObjectId = 16;
            this.cmbWosaService.Size = new System.Drawing.Size(266, 21);
            this.cmbWosaService.TabIndex = 5;
            this.cmbWosaService.Value = null;
            // 
            // lblLogicalService
            // 
            this.lblLogicalService.AutoEllipsis = true;
            this.lblLogicalService.Location = new System.Drawing.Point(4, 80);
            this.lblLogicalService.Name = "lblLogicalService";
            this.lblLogicalService.PhoenixUIControl.ObjectId = 16;
            this.lblLogicalService.Size = new System.Drawing.Size(148, 20);
            this.lblLogicalService.TabIndex = 4;
            this.lblLogicalService.Text = "Logical Service:";
            // 
            // dfForm
            // 
            this.dfForm.Location = new System.Drawing.Point(138, 56);
            this.dfForm.Name = "dfForm";
            this.dfForm.PhoenixUIControl.ObjectId = 15;
            this.dfForm.Size = new System.Drawing.Size(266, 20);
            this.dfForm.TabIndex = 3;
            // 
            // lblForm
            // 
            this.lblForm.AutoEllipsis = true;
            this.lblForm.Location = new System.Drawing.Point(4, 56);
            this.lblForm.Name = "lblForm";
            this.lblForm.PhoenixUIControl.ObjectId = 15;
            this.lblForm.Size = new System.Drawing.Size(148, 20);
            this.lblForm.TabIndex = 2;
            this.lblForm.Text = "Form:";
            // 
            // lblText
            // 
            this.lblText.AutoEllipsis = true;
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblText.Location = new System.Drawing.Point(4, 16);
            this.lblText.Name = "lblText";
            this.lblText.PhoenixUIControl.ObjectId = 14;
            this.lblText.Size = new System.Drawing.Size(384, 33);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Your transaction has posted successfully.  Please insert the Form listed in the t" +
    "eller printer and press <ENTER> when ready:";
            this.lblText.WordWrap = true;
            // 
            // lblNote
            // 
            this.lblNote.AutoEllipsis = true;
            this.lblNote.Location = new System.Drawing.Point(8, 373);
            this.lblNote.Name = "lblNote";
            this.lblNote.PhoenixUIControl.ObjectId = 18;
            this.lblNote.Size = new System.Drawing.Size(384, 12);
            this.lblNote.TabIndex = 3;
            this.lblNote.Text = "Note: To reprint the last form, press the Reprint Last button (F10).";
            // 
            // lblNote2
            // 
            this.lblNote2.AutoEllipsis = true;
            this.lblNote2.Location = new System.Drawing.Point(8, 388);
            this.lblNote2.Name = "lblNote2";
            this.lblNote2.PhoenixUIControl.ObjectId = 19;
            this.lblNote2.Size = new System.Drawing.Size(384, 28);
            this.lblNote2.TabIndex = 5;
            this.lblNote2.Text = "To bypass printing only for Passbook accounts, press the \'Skip\' button. The trans" +
    "action(s) is still considered booked.";
            this.lblNote2.WordWrap = true;
            // 
            // pbReprintLast
            // 
            this.pbReprintLast.LongText = "Reprint Last";
            this.pbReprintLast.Name = "pbReprintLast";
            this.pbReprintLast.ObjectId = 21;
            this.pbReprintLast.Shortcut = System.Windows.Forms.Keys.F10;
            this.pbReprintLast.ShortText = "Reprint Last";
            this.pbReprintLast.Tag = null;
            this.pbReprintLast.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReprintLast_Click);
            // 
            // pbSkip
            // 
            this.pbSkip.LongText = "Skip (F8)";
            this.pbSkip.Name = "pbSkip";
            this.pbSkip.ObjectId = 22;
            this.pbSkip.Shortcut = System.Windows.Forms.Keys.F8;
            this.pbSkip.ShortText = "Skip (F8)";
            this.pbSkip.Tag = null;
            this.pbSkip.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSkip_Click);
            // 
            // pbArchiveOnly
            // 
            this.pbArchiveOnly.LongText = "Archive Only";
            this.pbArchiveOnly.Name = "pbArchiveOnly";
            this.pbArchiveOnly.ObjectId = 23;
            this.pbArchiveOnly.ShortText = "Archive Only";
            this.pbArchiveOnly.Tag = null;
            this.pbArchiveOnly.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbArchiveOnly_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.LongText = "Cancel";
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.ObjectId = 24;
            this.pbCancel.ShortText = "Cancel";
            this.pbCancel.Tag = null;
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // dfHiddenTextNote
            // 
            this.dfHiddenTextNote.Location = new System.Drawing.Point(370, 373);
            this.dfHiddenTextNote.Name = "dfHiddenTextNote";
            this.dfHiddenTextNote.PhoenixUIControl.ObjectId = 18;
            this.dfHiddenTextNote.ReadOnly = true;
            this.dfHiddenTextNote.Size = new System.Drawing.Size(10, 20);
            this.dfHiddenTextNote.TabIndex = 4;
            this.dfHiddenTextNote.Visible = false;
            // 
            // dfHiddenTextNote2
            // 
            this.dfHiddenTextNote2.Location = new System.Drawing.Point(370, 388);
            this.dfHiddenTextNote2.Name = "dfHiddenTextNote2";
            this.dfHiddenTextNote2.PhoenixUIControl.ObjectId = 19;
            this.dfHiddenTextNote2.ReadOnly = true;
            this.dfHiddenTextNote2.Size = new System.Drawing.Size(10, 20);
            this.dfHiddenTextNote2.TabIndex = 6;
            this.dfHiddenTextNote2.Visible = false;
            // 
            // dlgRecieptInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(419, 419);
            this.Controls.Add(this.dfHiddenTextNote2);
            this.Controls.Add(this.dfHiddenTextNote);
            this.Controls.Add(this.lblNote2);
            this.Controls.Add(this.lblNote);
            this.Controls.Add(this.gbPrintingFormsInformation);
            this.Controls.Add(this.gbReceiptDeliveryInformation);
            this.Controls.Add(this.gbTransactionInformation);
            this.EditRecordTitle = "fwStandard";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgRecieptInfo";
            this.NewRecordTitle = "fwStandard";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.ScreenId = 3528;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.Text = "Reciept Infomation";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgRecieptInfo_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgRecieptInfo_PInitCompleteEvent);
            this.gbTransactionInformation.ResumeLayout(false);
            this.gbTransactionInformation.PerformLayout();
            this.gbReceiptDeliveryInformation.ResumeLayout(false);
            this.gbReceiptDeliveryInformation.PerformLayout();
            this.pPanel1.ResumeLayout(false);
            this.pPanel1.PerformLayout();
            this.gbPrintingFormsInformation.ResumeLayout(false);
            this.gbPrintingFormsInformation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbTransactionInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblTransaction;
        private Phoenix.Windows.Forms.PdfStandard dfTransaction;
        private Phoenix.Windows.Forms.PLabelStandard lblAmount;
        private Phoenix.Windows.Forms.PdfStandard dfAmount;
        private Phoenix.Windows.Forms.PLabelStandard lblAccount;
        private Phoenix.Windows.Forms.PdfStandard dfAccount;
        private Phoenix.Windows.Forms.PLabelStandard lblCustomer;
        private Phoenix.Windows.Forms.PdfStandard dfCustomer;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbReceiptDeliveryInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblReceiptDeliveryDefault;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbPrint;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbEmail;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbPrintAndEmail;       
        private Phoenix.Windows.Forms.PLabelStandard lblEmail;
        private Phoenix.Windows.Forms.PdfStandard dfNewEmail;
        private Phoenix.Windows.Forms.PLabelStandard lblNewEmail;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbEmail;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbPrintingFormsInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblText;
        private Phoenix.Windows.Forms.PLabelStandard lblForm;
        private Phoenix.Windows.Forms.PdfStandard dfForm;
        private Phoenix.Windows.Forms.PLabelStandard lblLogicalService;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbWosaService;
        private Phoenix.Windows.Forms.PLabelStandard lblNumberofLastPrintedLine;
        private Phoenix.Windows.Forms.PdfStandard dfPbLastLine;
        private Phoenix.Windows.Forms.PLabelStandard lblNote;
        private Phoenix.Windows.Forms.PLabelStandard lblNote2;
        private Phoenix.Windows.Forms.PAction pbContinue;
        private Phoenix.Windows.Forms.PAction pbReprintLast;
        private Phoenix.Windows.Forms.PAction pbSkip;
        private Phoenix.Windows.Forms.PAction pbArchiveOnly;
        private Phoenix.Windows.Forms.PAction pbCancel;
        private Phoenix.Windows.Forms.PPanel pPanel1;
        private Phoenix.Windows.Forms.PdfStandard dfHiddenTextReceipt;
        private Phoenix.Windows.Forms.PdfStandard dfHiddenTextPrint;
        private Phoenix.Windows.Forms.PdfStandard dfHiddenTextNote;
        private Phoenix.Windows.Forms.PdfStandard dfHiddenTextNote2;
        private Phoenix.Windows.Forms.PLabelStandard lblMulti;
        private Phoenix.Windows.Forms.PdfStandard dfHiddenTextNote3;
    }
}