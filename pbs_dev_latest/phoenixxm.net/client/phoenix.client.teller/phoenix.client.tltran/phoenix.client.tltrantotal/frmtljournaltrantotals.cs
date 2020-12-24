#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2010 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmtljournaltrantotals.cs
// NameSpace: Phoenix.Windows.Forms
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//02/26/2010    2		mramalin    #79510 - Shared Branch
//05/27/2010    3		rpoddar     #09143 - Shared Branch
//09/08/2011    4		SDhamija	#15431 - windows resizing, UI only change
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
//
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Variables;

namespace Phoenix.Client.TlTranTotal
{
	/// <summary>
	/// Summary description for frmTlJournalTranTotals.
	/// </summary>
	public class frmTlJournalTranTotals : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		TlJournal _busObjTlJrnl = new TlJournal();
		private PSmallInt branchNo	= new PSmallInt();
		private PSmallInt drawerNo	= new PSmallInt();
		private PDateTime effectiveDt	= new PDateTime();
		private PDecimal ptid		= new PDecimal();
		private Phoenix.Windows.Forms.PGrid gridJournalTranTotals;
		private Phoenix.Windows.Forms.PGridColumn colTranCode;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colCount;
		private Phoenix.Windows.Forms.PGridColumn colIsoCode;
        private PGroupBoxStandard gbDisplayCriteria; 		// 79510
        private PGroupBoxStandard gbGrid; 					// 79501
        private PRadioButtonStandard rbSbTransactionOnly; 	// 79510
        private PRadioButtonStandard rbAllTransactions; 	// 79510
		private Phoenix.Windows.Forms.PGridColumn colAmt;

		public frmTlJournalTranTotals()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gridJournalTranTotals = new Phoenix.Windows.Forms.PGrid();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colIsoCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.gbDisplayCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.rbSbTransactionOnly = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbAllTransactions = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.gbGrid = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbDisplayCriteria.SuspendLayout();
            this.gbGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridJournalTranTotals
            // 
            this.gridJournalTranTotals.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colTranCode,
            this.colDescription,
            this.colCount,
            this.colIsoCode,
            this.colAmt});
            this.gridJournalTranTotals.LinesInHeader = 2;
            this.gridJournalTranTotals.Location = new System.Drawing.Point(4, 10);
            this.gridJournalTranTotals.Name = "gridJournalTranTotals";
            this.gridJournalTranTotals.Size = new System.Drawing.Size(672, 382);
            this.gridJournalTranTotals.TabIndex = 0;
            this.gridJournalTranTotals.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridJournalTranTotals_FetchRowDone);
            this.gridJournalTranTotals.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridJournalTranTotals_BeforePopulate);
            // 
            // colTranCode
            // 
            this.colTranCode.PhoenixUIControl.ObjectId = 3;
            this.colTranCode.PhoenixUIControl.XmlTag = "TlTranCode";
            this.colTranCode.Title = "TC";
            this.colTranCode.Width = 56;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 4;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 368;
            // 
            // colCount
            // 
            this.colCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCount.PhoenixUIControl.ObjectId = 5;
            this.colCount.PhoenixUIControl.XmlTag = "NoOfTransactions";
            this.colCount.Title = "# of Transactions";
            this.colCount.Width = 89;
            // 
            // colIsoCode
            // 
            this.colIsoCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colIsoCode.PhoenixUIControl.ObjectId = 9;
            this.colIsoCode.PhoenixUIControl.XmlTag = "Isocode";
            this.colIsoCode.Title = "Iso Code";
            this.colIsoCode.Visible = false;
            this.colIsoCode.Width = 0;
            // 
            // colAmt
            // 
            this.colAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.ObjectId = 6;
            this.colAmt.PhoenixUIControl.XmlTag = "TransactionsTotal";
            this.colAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colAmt.Title = "Transaction Total";
            this.colAmt.Width = 148;
            // 
            // gbDisplayCriteria
            // 
            this.gbDisplayCriteria.Controls.Add(this.rbSbTransactionOnly);
            this.gbDisplayCriteria.Controls.Add(this.rbAllTransactions);
            this.gbDisplayCriteria.Location = new System.Drawing.Point(4, 4);
            this.gbDisplayCriteria.Name = "gbDisplayCriteria";
            this.gbDisplayCriteria.PhoenixUIControl.ObjectId = 10;
            this.gbDisplayCriteria.Size = new System.Drawing.Size(680, 40);
            this.gbDisplayCriteria.TabIndex = 1;
            this.gbDisplayCriteria.TabStop = false;
            this.gbDisplayCriteria.Text = "Display Criteria";
            // 
            // rbSbTransactionOnly
            // 
            this.rbSbTransactionOnly.AutoSize = true;
            this.rbSbTransactionOnly.Description = null;
            this.rbSbTransactionOnly.Location = new System.Drawing.Point(52, 16);
            this.rbSbTransactionOnly.Name = "rbSbTransactionOnly";
            this.rbSbTransactionOnly.PhoenixUIControl.ObjectId = 12;
            this.rbSbTransactionOnly.Size = new System.Drawing.Size(161, 18);
            this.rbSbTransactionOnly.TabIndex = 1;
            this.rbSbTransactionOnly.Text = "Shared Br Transaction Only";
            this.rbSbTransactionOnly.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbSbTransactionOnly_PhoenixUICheckedChangedEvent);
            // 
            // rbAllTransactions
            // 
            this.rbAllTransactions.AutoSize = true;
            this.rbAllTransactions.Checked = true;
            this.rbAllTransactions.Description = null;
            this.rbAllTransactions.Location = new System.Drawing.Point(8, 16);
            this.rbAllTransactions.Name = "rbAllTransactions";
            this.rbAllTransactions.PhoenixUIControl.ObjectId = 11;
            this.rbAllTransactions.Size = new System.Drawing.Size(42, 18);
            this.rbAllTransactions.TabIndex = 0;
            this.rbAllTransactions.TabStop = true;
            this.rbAllTransactions.Text = "All";
            this.rbAllTransactions.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbAllTransactions_PhoenixUICheckedChangedEvent);
            // 
            // gbGrid
            // 
            this.gbGrid.Controls.Add(this.gridJournalTranTotals);
            this.gbGrid.Location = new System.Drawing.Point(4, 44);
            this.gbGrid.Name = "gbGrid";
            this.gbGrid.Size = new System.Drawing.Size(680, 396);
            this.gbGrid.TabIndex = 2;
            this.gbGrid.TabStop = false;
            // 
            // frmTlJournalTranTotals
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.gbDisplayCriteria);
            this.Controls.Add(this.gbGrid);
            this.Name = "frmTlJournalTranTotals";
            this.ScreenId = 10446;
            this.Text = "Teller Transaction Totals";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlJournalTranTotals_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlJournalTranTotals_PInitBeginEvent);
            this.gbDisplayCriteria.ResumeLayout(false);
            this.gbDisplayCriteria.PerformLayout();
            this.gbGrid.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			branchNo.Value = Convert.ToInt16(paramList[0]);
			drawerNo.Value = Convert.ToInt16(paramList[1]);
			effectiveDt.Value = Convert.ToDateTime(paramList[2]);

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion
		
			this.ScreenId  = Phoenix.Shared.Constants.ScreenId.JournalTranTotals;			
			base.InitParameters (paramList);
		}
		#endregion

		#region events
		private ReturnType frmTlJournalTranTotals_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.NoEdit;
			EnableDisableVisibleLogic("FormInit");
			return ReturnType.Success;
		}

		private void gridJournalTranTotals_BeforePopulate(object sender, GridPopulateArgs e)
		{
			this._busObjTlJrnl.BranchNo.Value = branchNo.Value;
			this._busObjTlJrnl.DrawerNo.Value = drawerNo.Value;
			this._busObjTlJrnl.EffectiveDt.Value = effectiveDt.Value;
            if( rbAllTransactions.Checked ) // 79510
			    this._busObjTlJrnl.OutputType.Value = 1;
            else
                this._busObjTlJrnl.OutputType.Value = 7; // 79510
			this.gridJournalTranTotals.ListViewObject = _busObjTlJrnl;
		}

		private void gridJournalTranTotals_FetchRowDone(object sender, GridRowArgs e)
		{
			if (colTranCode.Text.Trim() == "ADD" || colTranCode.Text.Trim() == "REM" || colTranCode.Text.Trim() == "SWP")
				colDescription.Text = this._busObjTlJrnl.GetTCDTranCodeDesc(colTranCode.UnFormattedValue.ToString());
		}
		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormInit")
				this.colIsoCode.Visible = false;
		}
		#endregion

#region 79510
        private void rbAllTransactions_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            gridJournalTranTotals.ObjectToScreen();
        }

        private void rbSbTransactionOnly_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            gridJournalTranTotals.ObjectToScreen();
        }

        private void frmTlJournalTranTotals_PInitCompleteEvent()
        {
            if (rbAllTransactions.MLInfo != null)
                rbAllTransactions.Text = rbAllTransactions.MLInfo.Label;
            if( rbSbTransactionOnly.MLInfo != null )
                rbSbTransactionOnly.Text = rbSbTransactionOnly.MLInfo.Label;

            gbDisplayCriteria.Enabled = TellerVars.Instance.ShareBranchEnabled;  // #09143 - replaced SharedBranchCustomOption
        }
#endregion
     

     
	}
}
