using System.Collections;
using Phoenix.Shared.Ucm;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;


namespace Phoenix.Client.TcdAdmin
{
    partial class frmTcdAdmin
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
            this.gbTcdDrawerInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfStationId = new Phoenix.Windows.Forms.PdfStandard();
            this.lblStationId = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfMachineId = new Phoenix.Windows.Forms.PdfStandard();
            this.lblMachineId = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfBranch = new Phoenix.Windows.Forms.PdfStandard();
            this.lblBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbSetup = new Phoenix.Windows.Forms.PAction();
            this.pbStatus = new Phoenix.Windows.Forms.PAction();
            this.pbLoadRem = new Phoenix.Windows.Forms.PAction();
            this.pbTotals = new Phoenix.Windows.Forms.PAction();
            this.pbTcdJournal = new Phoenix.Windows.Forms.PAction();
            this.pbTcdHistory = new Phoenix.Windows.Forms.PAction();
            this.pbTlrJournal = new Phoenix.Windows.Forms.PAction();
            this.pbDeplete = new Phoenix.Windows.Forms.PAction();
            this.pbReset = new Phoenix.Windows.Forms.PAction();
            this.gbTcdDrawerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSetup,
            this.pbStatus,
            this.pbLoadRem,
            this.pbDeplete,
            this.pbTotals,
            this.pbTcdJournal,
            this.pbTcdHistory,
            this.pbTlrJournal,
            this.pbReset});
            // 
            // gbTcdDrawerInfo
            // 
            this.gbTcdDrawerInfo.Controls.Add(this.dfStationId);
            this.gbTcdDrawerInfo.Controls.Add(this.lblStationId);
            this.gbTcdDrawerInfo.Controls.Add(this.dfMachineId);
            this.gbTcdDrawerInfo.Controls.Add(this.lblMachineId);
            this.gbTcdDrawerInfo.Controls.Add(this.dfBranch);
            this.gbTcdDrawerInfo.Controls.Add(this.lblBranch);
            this.gbTcdDrawerInfo.Location = new System.Drawing.Point(3, 1);
            this.gbTcdDrawerInfo.Name = "gbTcdDrawerInfo";
            this.gbTcdDrawerInfo.Size = new System.Drawing.Size(684, 445);
            this.gbTcdDrawerInfo.TabIndex = 0;
            this.gbTcdDrawerInfo.TabStop = false;
            this.gbTcdDrawerInfo.Text = "TCD/TCR Machine Information";
            // 
            // dfStationId
            // 
            this.dfStationId.Location = new System.Drawing.Point(103, 68);
            this.dfStationId.Name = "dfStationId";
            this.dfStationId.PhoenixUIControl.ObjectId = 3;
            this.dfStationId.Size = new System.Drawing.Size(191, 20);
            this.dfStationId.TabIndex = 5;
            // 
            // lblStationId
            // 
            this.lblStationId.AutoEllipsis = true;
            this.lblStationId.Location = new System.Drawing.Point(4, 68);
            this.lblStationId.Name = "lblStationId";
            this.lblStationId.Size = new System.Drawing.Size(76, 20);
            this.lblStationId.TabIndex = 4;
            this.lblStationId.Text = "Station ID:";
            // 
            // dfMachineId
            // 
            this.dfMachineId.Location = new System.Drawing.Point(103, 42);
            this.dfMachineId.Name = "dfMachineId";
            this.dfMachineId.PhoenixUIControl.ObjectId = 2;
            this.dfMachineId.Size = new System.Drawing.Size(191, 20);
            this.dfMachineId.TabIndex = 3;
            // 
            // lblMachineId
            // 
            this.lblMachineId.AutoEllipsis = true;
            this.lblMachineId.Location = new System.Drawing.Point(4, 42);
            this.lblMachineId.Name = "lblMachineId";
            this.lblMachineId.Size = new System.Drawing.Size(84, 20);
            this.lblMachineId.TabIndex = 2;
            this.lblMachineId.Text = "Machine ID:";
            // 
            // dfBranch
            // 
            this.dfBranch.Location = new System.Drawing.Point(103, 17);
            this.dfBranch.Name = "dfBranch";
            this.dfBranch.PhoenixUIControl.ObjectId = 1;
            this.dfBranch.Size = new System.Drawing.Size(191, 20);
            this.dfBranch.TabIndex = 1;
            // 
            // lblBranch
            // 
            this.lblBranch.AutoEllipsis = true;
            this.lblBranch.Location = new System.Drawing.Point(4, 17);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.Size = new System.Drawing.Size(69, 20);
            this.lblBranch.TabIndex = 0;
            this.lblBranch.Text = "Branch:";
            // 
            // pbSetup
            // 
            this.pbSetup.LongText = "S&etup";
            this.pbSetup.NextScreenId = 16016;
            this.pbSetup.ObjectId = 5;
            this.pbSetup.ShortText = "S&etup";
            this.pbSetup.Tag = null;
            this.pbSetup.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSetup_Click);
            // 
            // pbStatus
            // 
            this.pbStatus.LongText = "&Status";
            this.pbStatus.NextScreenId = 16017;
            this.pbStatus.ObjectId = 6;
            this.pbStatus.ShortText = "&Status";
            this.pbStatus.Tag = null;
            this.pbStatus.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbStatus_Click);
            // 
            // pbLoadRem
            // 
            this.pbLoadRem.LongText = "&Load/Rem";
            this.pbLoadRem.NextScreenId = 16018;
            this.pbLoadRem.ObjectId = 7;
            this.pbLoadRem.ShortText = "&Load/Rem";
            this.pbLoadRem.Tag = null;
            this.pbLoadRem.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbLoadRem_Click);
            // 
            // pbTotals
            // 
            this.pbTotals.LongText = "&Totals";
            this.pbTotals.NextScreenId = 16019;
            this.pbTotals.ObjectId = 8;
            this.pbTotals.ShortText = "&Totals";
            this.pbTotals.Tag = null;
            this.pbTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTotals_Click);
            // 
            // pbTcdJournal
            // 
            this.pbTcdJournal.LongText = "TCD J&ournal";
            this.pbTcdJournal.NextScreenId = 16020;
            this.pbTcdJournal.ObjectId = 9;
            this.pbTcdJournal.ShortText = "TCD J&ournal";
            this.pbTcdJournal.Tag = null;
            this.pbTcdJournal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTcdJournal_Click);
            // 
            // pbTcdHistory
            // 
            this.pbTcdHistory.LongText = "TCD &History";
            this.pbTcdHistory.NextScreenId = 16021;
            this.pbTcdHistory.ObjectId = 10;
            this.pbTcdHistory.ShortText = "TCD &History";
            this.pbTcdHistory.Tag = null;
            this.pbTcdHistory.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTcdHistory_Click);
            // 
            // pbTlrJournal
            // 
            this.pbTlrJournal.LongText = "Tlr &Journal";
            this.pbTlrJournal.NextScreenId = 3090;
            this.pbTlrJournal.ObjectId = 11;
            this.pbTlrJournal.ShortText = "Tlr &Journal";
            this.pbTlrJournal.Tag = null;
            this.pbTlrJournal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTlrJournal_Click);
            // 
            // pbDeplete
            // 
            this.pbDeplete.LongText = "De&plete";
            this.pbDeplete.NextScreenId = 16018;
            this.pbDeplete.ObjectId = 13;
            this.pbDeplete.ShortText = "De&plete";
            this.pbDeplete.Tag = null;
            this.pbDeplete.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDeplete_Click);
            // 
            // pbReset
            // 
            this.pbReset.LongText = "&Reset";
            this.pbReset.NextScreenId = 3013;
            this.pbReset.ObjectId = 14;
            this.pbReset.ShortText = "&Reset";
            this.pbReset.Tag = null;
            this.pbReset.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReset_Click);
            // 
            // frmTcdAdmin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbTcdDrawerInfo);
            this.Name = "frmTcdAdmin";
            this.ScreenId = 16015;
            this.Text = "TCD Administrative Functions";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTcdAdmin_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTcdAdmin_PInitBeginEvent);
            this.gbTcdDrawerInfo.ResumeLayout(false);
            this.gbTcdDrawerInfo.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private Phoenix.Windows.Forms.PGroupBoxStandard gbTcdDrawerInfo;
        private Phoenix.Windows.Forms.PLabelStandard lblBranch;
        private Phoenix.Windows.Forms.PdfStandard dfStationId;
        private Phoenix.Windows.Forms.PLabelStandard lblStationId;
        private Phoenix.Windows.Forms.PdfStandard dfMachineId;
        private Phoenix.Windows.Forms.PLabelStandard lblMachineId;
        private Phoenix.Windows.Forms.PdfStandard dfBranch;
        private Phoenix.Windows.Forms.PAction pbSetup;
        private Phoenix.Windows.Forms.PAction pbStatus;
        private Phoenix.Windows.Forms.PAction pbLoadRem;
        private Phoenix.Windows.Forms.PAction pbTotals;
        private Phoenix.Windows.Forms.PAction pbTcdJournal;
        private Phoenix.Windows.Forms.PAction pbTcdHistory;
        private Phoenix.Windows.Forms.PAction pbTlrJournal;

        //private int _traceDataTextLen = 0;
        private ArrayList _devOutputInfo = new ArrayList();

        private ArrayList TcdCashOutDenoms = new ArrayList();
        //private int _returnCode;

        private PBaseType TranAmount = new PDecimal("A1");
        private PBaseType FromForward = new PBaseType("A2");
        private PBaseType TlJrnlPtid = new PDecimal("A3");

        private decimal totalAmount;
        private PAction pbDeplete;
        private PAction pbReset;
        
    }
}
