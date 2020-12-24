namespace phoenix.client.tlproofdetails
{
    partial class frmTlProofDetails
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
            this.grdProofDetails = new Phoenix.Windows.Forms.PGrid();
            this.colTellerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSeqNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSub = new Phoenix.Windows.Forms.PGridColumn();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoOfItems = new Phoenix.Windows.Forms.PGridColumn();
            this.colEffDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colJrlPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckType = new Phoenix.Windows.Forms.PGridColumn();
            this.gbTotInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTotTranAmt = new Phoenix.Windows.Forms.PdfStandard();
            this.dfTotNumTran = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTotTranAmt = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTotNumTran = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbDisplay = new Phoenix.Windows.Forms.PAction();
            this.gbTotInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbDisplay});
            // 
            // grdProofDetails
            // 
            this.grdProofDetails.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colTellerNo,
            this.colSeqNo,
            this.colSub,
            this.colAccount,
            this.colCheckNo,
            this.colAmount,
            this.colNoOfItems,
            this.colEffDt,
            this.colJrlPtid,
            this.colAcctType,
            this.colAcctNo,
            this.colDrawerNo,
            this.colCheckType});
            this.grdProofDetails.IsMaxNumRowsCustomized = false;
            this.grdProofDetails.LinesInHeader = 2;
            this.grdProofDetails.Location = new System.Drawing.Point(4, 4);
            this.grdProofDetails.Name = "grdProofDetails";
            this.grdProofDetails.Size = new System.Drawing.Size(682, 372);
            this.grdProofDetails.TabIndex = 0;
            this.grdProofDetails.Text = "pGrid1";
            this.grdProofDetails.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdProofDetails_BeforePopulate);
            this.grdProofDetails.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdProofDetails_FetchRowDone);
            this.grdProofDetails.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdProofDetails_AfterPopulate);
            // 
            // colTellerNo
            // 
            this.colTellerNo.PhoenixUIControl.ObjectId = 2;
            this.colTellerNo.PhoenixUIControl.XmlTag = "EmplId";
            this.colTellerNo.Title = "Column";
            this.colTellerNo.Visible = false;
            // 
            // colSeqNo
            // 
            this.colSeqNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSeqNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSeqNo.PhoenixUIControl.ObjectId = 3;
            this.colSeqNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.colSeqNo.Title = "Seq #";
            this.colSeqNo.Width = 60;
            // 
            // colSub
            // 
            this.colSub.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSub.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSub.PhoenixUIControl.ObjectId = 4;
            this.colSub.PhoenixUIControl.XmlTag = "SubSequence";
            this.colSub.Title = "Sub";
            this.colSub.Width = 50;
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 5;
            this.colAccount.PhoenixUIControl.XmlTag = "";
            this.colAccount.Title = "Account";
            this.colAccount.Width = 160;
            // 
            // colCheckNo
            // 
            this.colCheckNo.PhoenixUIControl.ObjectId = 6;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.Title = "Check #";
            // 
            // colAmount
            // 
            this.colAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.InputMask = "";
            this.colAmount.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.False;
            this.colAmount.PhoenixUIControl.ObjectId = 7;
            this.colAmount.PhoenixUIControl.XmlTag = "Amount";
            this.colAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colAmount.Title = "Amount";
            this.colAmount.Width = 180;
            // 
            // colNoOfItems
            // 
            this.colNoOfItems.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoOfItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoOfItems.PhoenixUIControl.ObjectId = 8;
            this.colNoOfItems.PhoenixUIControl.XmlTag = "NoItems";
            this.colNoOfItems.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colNoOfItems.Title = "# of Items";
            this.colNoOfItems.Width = 120;
            // 
            // colEffDt
            // 
            this.colEffDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.colEffDt.Title = "Column";
            this.colEffDt.Visible = false;
            // 
            // colJrlPtid
            // 
            this.colJrlPtid.PhoenixUIControl.XmlTag = "Ptid";
            this.colJrlPtid.Title = "Column";
            this.colJrlPtid.Visible = false;
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "Column";
            this.colAcctType.Visible = false;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.XmlTag = "JrlAcctNo";
            this.colAcctNo.Title = "Column";
            this.colAcctNo.Visible = false;
            // 
            // colDrawerNo
            // 
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "Column";
            this.colDrawerNo.Visible = false;
            // 
            // colCheckType
            // 
            this.colCheckType.PhoenixUIControl.XmlTag = "CheckType";
            this.colCheckType.Title = "Check Type";
            this.colCheckType.Visible = false;
            // 
            // gbTotInfo
            // 
            this.gbTotInfo.Controls.Add(this.dfTotTranAmt);
            this.gbTotInfo.Controls.Add(this.dfTotNumTran);
            this.gbTotInfo.Controls.Add(this.lblTotTranAmt);
            this.gbTotInfo.Controls.Add(this.lblTotNumTran);
            this.gbTotInfo.Location = new System.Drawing.Point(4, 374);
            this.gbTotInfo.Name = "gbTotInfo";
            this.gbTotInfo.PhoenixUIControl.ObjectId = 9;
            this.gbTotInfo.Size = new System.Drawing.Size(682, 71);
            this.gbTotInfo.TabIndex = 1;
            this.gbTotInfo.TabStop = false;
            this.gbTotInfo.Text = "Totals Information";
            // 
            // dfTotTranAmt
            // 
            this.dfTotTranAmt.Enabled = false;
            this.dfTotTranAmt.Location = new System.Drawing.Point(461, 28);
            this.dfTotTranAmt.Name = "dfTotTranAmt";
            this.dfTotTranAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotTranAmt.PhoenixUIControl.ObjectId = 11;
            this.dfTotTranAmt.PhoenixUIControl.XmlTag = "";
            this.dfTotTranAmt.Size = new System.Drawing.Size(213, 20);
            this.dfTotTranAmt.TabIndex = 3;
            // 
            // dfTotNumTran
            // 
            this.dfTotNumTran.Enabled = false;
            this.dfTotNumTran.Location = new System.Drawing.Point(147, 28);
            this.dfTotNumTran.Name = "dfTotNumTran";
            this.dfTotNumTran.PhoenixUIControl.ObjectId = 10;
            this.dfTotNumTran.PhoenixUIControl.XmlTag = "";
            this.dfTotNumTran.Size = new System.Drawing.Size(100, 20);
            this.dfTotNumTran.TabIndex = 2;
            // 
            // lblTotTranAmt
            // 
            this.lblTotTranAmt.AutoEllipsis = true;
            this.lblTotTranAmt.Location = new System.Drawing.Point(290, 28);
            this.lblTotTranAmt.Name = "lblTotTranAmt";
            this.lblTotTranAmt.PhoenixUIControl.ObjectId = 11;
            this.lblTotTranAmt.Size = new System.Drawing.Size(165, 20);
            this.lblTotTranAmt.TabIndex = 1;
            this.lblTotTranAmt.Text = "Total Transaction Amount:";
            // 
            // lblTotNumTran
            // 
            this.lblTotNumTran.AutoEllipsis = true;
            this.lblTotNumTran.Location = new System.Drawing.Point(8, 28);
            this.lblTotNumTran.Name = "lblTotNumTran";
            this.lblTotNumTran.PhoenixUIControl.ObjectId = 10;
            this.lblTotNumTran.Size = new System.Drawing.Size(133, 20);
            this.lblTotNumTran.TabIndex = 0;
            this.lblTotNumTran.Text = "Total Number of Checks:";
            // 
            // pbDisplay
            // 
            this.pbDisplay.LongText = "&Display...";
            this.pbDisplay.ObjectId = 14;
            this.pbDisplay.ShortText = "&Display...";
            this.pbDisplay.Tag = null;
            this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
            // 
            // frmTlProofDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbTotInfo);
            this.Controls.Add(this.grdProofDetails);
            this.Name = "frmTlProofDetails";
            this.Text = "Teller Proof Details";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.fwStandard_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.fwStandard_PInitCompleteEvent);
            this.PMdiPrintEvent += new System.EventHandler(this.frmTlProofDetails_PMdiPrintEvent);
            this.Load += new System.EventHandler(this.frmTlProofDetails_Load);
            this.gbTotInfo.ResumeLayout(false);
            this.gbTotInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Phoenix.Windows.Forms.PGrid grdProofDetails;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbTotInfo;
        private Phoenix.Windows.Forms.PdfStandard dfTotTranAmt;
        private Phoenix.Windows.Forms.PdfStandard dfTotNumTran;
        private Phoenix.Windows.Forms.PLabelStandard lblTotTranAmt;
        private Phoenix.Windows.Forms.PLabelStandard lblTotNumTran;
        private Phoenix.Windows.Forms.PGridColumn colTellerNo;
        private Phoenix.Windows.Forms.PGridColumn colSeqNo;
        private Phoenix.Windows.Forms.PGridColumn colSub;
        private Phoenix.Windows.Forms.PGridColumn colAccount;
        private Phoenix.Windows.Forms.PGridColumn colCheckNo;
        private Phoenix.Windows.Forms.PGridColumn colAmount;
        private Phoenix.Windows.Forms.PGridColumn colNoOfItems;
        private Phoenix.Windows.Forms.PAction pbDisplay;
        private Phoenix.Windows.Forms.PGridColumn colEffDt;
        private Phoenix.Windows.Forms.PGridColumn colJrlPtid;
        private Phoenix.Windows.Forms.PGridColumn colAcctType;
        private Phoenix.Windows.Forms.PGridColumn colAcctNo;
        private Phoenix.Windows.Forms.PGridColumn colDrawerNo;
        private Phoenix.Windows.Forms.PGridColumn colCheckType;
    }
}