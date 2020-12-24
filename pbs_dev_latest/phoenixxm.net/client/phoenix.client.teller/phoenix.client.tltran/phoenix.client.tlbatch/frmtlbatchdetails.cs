#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmTlCharges.cs
// NameSpace: Phoenix.Client.TlCapturedItems
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//6/6/2006		1		mselvaga	Issue#67873 - Created.
//11/28/2006	2		mselvaga	Issue#70282 - Made column alignment changes to make most columns visible without scroll.	
//08/24/2007    3       bbedi       #72916 - Add TCD Support to Teller 2007
//02/11/2010    4       LSimpson    #79574 - UI changes for Cash Recycler.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;

namespace Phoenix.Windows.Client
{
	/// <summary>
	/// Summary description for frmTlBatchDetails.
	/// </summary>
	public class frmTlBatchDetails : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TlJournal _busObjTlJrnl = new TlJournal();
		private TellerVars _tellerVars = TellerVars.Instance;
		private int rowCounter;
		private PDecimal totalTlCashOut = new PDecimal();
		private PDecimal totalTcdCashOut = new PDecimal();
		private PDecimal totalTranAmt = new PDecimal();
		
		private PSmallInt branchNo = new PSmallInt();
		private PSmallInt drawerNo = new PSmallInt();
		private PSmallInt batchId = new PSmallInt();
		private Phoenix.Windows.Forms.PGrid gridBatchDetails;
		private Phoenix.Windows.Forms.PGridColumn colSeqNo;
		private Phoenix.Windows.Forms.PGridColumn colSubSeqNo;
		private Phoenix.Windows.Forms.PGridColumn colAccount;
		private Phoenix.Windows.Forms.PGridColumn colTranCode;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colCheck;
		private Phoenix.Windows.Forms.PGridColumn colTellerCashOut;
		private Phoenix.Windows.Forms.PGridColumn colTCDCashOut;
		private Phoenix.Windows.Forms.PGridColumn colTranAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalNumberofTransactions;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalTransactions;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalTransactionAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalTransactionAmt;
		private Phoenix.Windows.Forms.PdfStandard dfAction;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTotalsInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalTellerCashOut;
		private Phoenix.Windows.Forms.PDfDisplay dfTotTellerCashOut;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalTCDCashOut;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colRimNo;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
		private Phoenix.Windows.Forms.PDfDisplay dfTotTCDCashOut;

		public frmTlBatchDetails()
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
            this.gridBatchDetails = new Phoenix.Windows.Forms.PGrid();
            this.colSeqNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSubSeqNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheck = new Phoenix.Windows.Forms.PGridColumn();
            this.colTellerCashOut = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDCashOut = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
            this.lblTotalNumberofTransactions = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalTransactions = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalTransactionAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalTransactionAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfAction = new Phoenix.Windows.Forms.PdfStandard();
            this.gbTotalsInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTotTCDCashOut = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalTCDCashOut = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotTellerCashOut = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTotalTellerCashOut = new Phoenix.Windows.Forms.PLabelStandard();
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbTotalsInformation.SuspendLayout();
            this.pGroupBoxStandard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridBatchDetails
            // 
            this.gridBatchDetails.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gridBatchDetails.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colSeqNo,
            this.colSubSeqNo,
            this.colAccount,
            this.colTranCode,
            this.colDescription,
            this.colCheck,
            this.colTellerCashOut,
            this.colTCDCashOut,
            this.colTranAmt,
            this.colAcctType,
            this.colAcctNo,
            this.colRimNo});
            this.gridBatchDetails.LinesInHeader = 2;
            this.gridBatchDetails.Location = new System.Drawing.Point(4, 12);
            this.gridBatchDetails.Name = "gridBatchDetails";
            this.gridBatchDetails.Size = new System.Drawing.Size(676, 372);
            this.gridBatchDetails.TabIndex = 0;
            this.gridBatchDetails.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridBatchDetails_AfterPopulate);
            this.gridBatchDetails.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridBatchDetails_FetchRowDone);
            this.gridBatchDetails.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridBatchDetails_BeforePopulate);
            // 
            // colSeqNo
            // 
            this.colSeqNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSeqNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSeqNo.PhoenixUIControl.ObjectId = 2;
            this.colSeqNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.colSeqNo.Title = "Seq#";
            this.colSeqNo.Width = 30;
            // 
            // colSubSeqNo
            // 
            this.colSubSeqNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSubSeqNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSubSeqNo.PhoenixUIControl.ObjectId = 3;
            this.colSubSeqNo.PhoenixUIControl.XmlTag = "SubSequence";
            this.colSubSeqNo.Title = "Sub";
            this.colSubSeqNo.Width = 30;
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 4;
            this.colAccount.PhoenixUIControl.XmlTag = "Account";
            this.colAccount.Title = "Account";
            this.colAccount.Width = 102;
            // 
            // colTranCode
            // 
            this.colTranCode.PhoenixUIControl.ObjectId = 5;
            this.colTranCode.PhoenixUIControl.XmlTag = "TlTranCode";
            this.colTranCode.Title = "TC";
            this.colTranCode.Width = 38;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 6;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 145;
            // 
            // colCheck
            // 
            this.colCheck.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCheck.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colCheck.PhoenixUIControl.ObjectId = 10;
            this.colCheck.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheck.Title = "Check# / #Items";
            this.colCheck.Width = 55;
            // 
            // colTellerCashOut
            // 
            this.colTellerCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTellerCashOut.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTellerCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTellerCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTellerCashOut.PhoenixUIControl.ObjectId = 11;
            this.colTellerCashOut.PhoenixUIControl.XmlTag = "TellerCashOut";
            this.colTellerCashOut.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colTellerCashOut.Title = "Teller Cash Out";
            this.colTellerCashOut.Width = 84;
            // 
            // colTCDCashOut
            // 
            this.colTCDCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashOut.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashOut.PhoenixUIControl.ObjectId = 12;
            this.colTCDCashOut.PhoenixUIControl.XmlTag = "TcdCashOut";
            this.colTCDCashOut.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colTCDCashOut.Title = "TCD/TCR Cash Out";
            this.colTCDCashOut.Width = 84;
            // 
            // colTranAmt
            // 
            this.colTranAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTranAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTranAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTranAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTranAmt.PhoenixUIControl.ObjectId = 7;
            this.colTranAmt.PhoenixUIControl.XmlTag = "NetAmt";
            this.colTranAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colTranAmt.Title = "Transaction Amount";
            this.colTranAmt.Width = 84;
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "Acct Type";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 0;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.Title = "Acct No";
            this.colAcctNo.Visible = false;
            this.colAcctNo.Width = 0;
            // 
            // colRimNo
            // 
            this.colRimNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRimNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colRimNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colRimNo.PhoenixUIControl.XmlTag = "RimNo";
            this.colRimNo.Title = "Rim No";
            this.colRimNo.Visible = false;
            this.colRimNo.Width = 0;
            // 
            // lblTotalNumberofTransactions
            // 
            this.lblTotalNumberofTransactions.AutoEllipsis = true;
            this.lblTotalNumberofTransactions.Location = new System.Drawing.Point(4, 16);
            this.lblTotalNumberofTransactions.Name = "lblTotalNumberofTransactions";
            this.lblTotalNumberofTransactions.PhoenixUIControl.ObjectId = 8;
            this.lblTotalNumberofTransactions.Size = new System.Drawing.Size(168, 16);
            this.lblTotalNumberofTransactions.TabIndex = 0;
            this.lblTotalNumberofTransactions.Text = "Total Number of Transactions:";
            // 
            // dfTotalTransactions
            // 
            this.dfTotalTransactions.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalTransactions.Location = new System.Drawing.Point(192, 16);
            this.dfTotalTransactions.Multiline = true;
            this.dfTotalTransactions.Name = "dfTotalTransactions";
            this.dfTotalTransactions.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTotalTransactions.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalTransactions.PhoenixUIControl.ObjectId = 8;
            this.dfTotalTransactions.Size = new System.Drawing.Size(69, 16);
            this.dfTotalTransactions.TabIndex = 1;
            // 
            // lblTotalTransactionAmount
            // 
            this.lblTotalTransactionAmount.AutoEllipsis = true;
            this.lblTotalTransactionAmount.Location = new System.Drawing.Point(4, 36);
            this.lblTotalTransactionAmount.Name = "lblTotalTransactionAmount";
            this.lblTotalTransactionAmount.PhoenixUIControl.ObjectId = 9;
            this.lblTotalTransactionAmount.Size = new System.Drawing.Size(164, 16);
            this.lblTotalTransactionAmount.TabIndex = 4;
            this.lblTotalTransactionAmount.Text = "Total Transaction Amount:";
            // 
            // dfTotalTransactionAmt
            // 
            this.dfTotalTransactionAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalTransactionAmt.Location = new System.Drawing.Point(192, 36);
            this.dfTotalTransactionAmt.Multiline = true;
            this.dfTotalTransactionAmt.Name = "dfTotalTransactionAmt";
            this.dfTotalTransactionAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalTransactionAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalTransactionAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalTransactionAmt.PhoenixUIControl.ObjectId = 9;
            this.dfTotalTransactionAmt.Size = new System.Drawing.Size(69, 16);
            this.dfTotalTransactionAmt.TabIndex = 5;
            // 
            // dfAction
            // 
            this.dfAction.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAction.Location = new System.Drawing.Point(256, 24);
            this.dfAction.Name = "dfAction";
            this.dfAction.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfAction.PhoenixUIControl.ObjectId = 10;
            this.dfAction.Size = new System.Drawing.Size(56, 20);
            this.dfAction.TabIndex = 0;
            this.dfAction.Visible = false;
            // 
            // gbTotalsInformation
            // 
            this.gbTotalsInformation.Controls.Add(this.dfTotTCDCashOut);
            this.gbTotalsInformation.Controls.Add(this.lblTotalTCDCashOut);
            this.gbTotalsInformation.Controls.Add(this.dfTotTellerCashOut);
            this.gbTotalsInformation.Controls.Add(this.lblTotalTellerCashOut);
            this.gbTotalsInformation.Controls.Add(this.dfTotalTransactionAmt);
            this.gbTotalsInformation.Controls.Add(this.lblTotalTransactionAmount);
            this.gbTotalsInformation.Controls.Add(this.dfTotalTransactions);
            this.gbTotalsInformation.Controls.Add(this.lblTotalNumberofTransactions);
            this.gbTotalsInformation.Location = new System.Drawing.Point(4, 388);
            this.gbTotalsInformation.Name = "gbTotalsInformation";
            this.gbTotalsInformation.PhoenixUIControl.ObjectId = 15;
            this.gbTotalsInformation.Size = new System.Drawing.Size(684, 56);
            this.gbTotalsInformation.TabIndex = 1;
            this.gbTotalsInformation.TabStop = false;
            this.gbTotalsInformation.Text = "Totals Information";
            // 
            // dfTotTCDCashOut
            // 
            this.dfTotTCDCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotTCDCashOut.Location = new System.Drawing.Point(576, 36);
            this.dfTotTCDCashOut.Multiline = true;
            this.dfTotTCDCashOut.Name = "dfTotTCDCashOut";
            this.dfTotTCDCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotTCDCashOut.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotTCDCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotTCDCashOut.PhoenixUIControl.ObjectId = 14;
            this.dfTotTCDCashOut.Size = new System.Drawing.Size(101, 16);
            this.dfTotTCDCashOut.TabIndex = 7;
            // 
            // lblTotalTCDCashOut
            // 
            this.lblTotalTCDCashOut.AutoEllipsis = true;
            this.lblTotalTCDCashOut.Location = new System.Drawing.Point(436, 36);
            this.lblTotalTCDCashOut.Name = "lblTotalTCDCashOut";
            this.lblTotalTCDCashOut.PhoenixUIControl.ObjectId = 14;
            this.lblTotalTCDCashOut.Size = new System.Drawing.Size(142, 16);
            this.lblTotalTCDCashOut.TabIndex = 6;
            this.lblTotalTCDCashOut.Text = "Total TCD/TCR Cash Out:";
            // 
            // dfTotTellerCashOut
            // 
            this.dfTotTellerCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotTellerCashOut.Location = new System.Drawing.Point(564, 16);
            this.dfTotTellerCashOut.Multiline = true;
            this.dfTotTellerCashOut.Name = "dfTotTellerCashOut";
            this.dfTotTellerCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotTellerCashOut.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotTellerCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotTellerCashOut.PhoenixUIControl.ObjectId = 13;
            this.dfTotTellerCashOut.Size = new System.Drawing.Size(113, 16);
            this.dfTotTellerCashOut.TabIndex = 3;
            this.dfTotTellerCashOut.TextChanged += new System.EventHandler(this.dfTotTellerCashOut_TextChanged);
            // 
            // lblTotalTellerCashOut
            // 
            this.lblTotalTellerCashOut.AutoEllipsis = true;
            this.lblTotalTellerCashOut.Location = new System.Drawing.Point(436, 16);
            this.lblTotalTellerCashOut.Name = "lblTotalTellerCashOut";
            this.lblTotalTellerCashOut.PhoenixUIControl.ObjectId = 13;
            this.lblTotalTellerCashOut.Size = new System.Drawing.Size(116, 16);
            this.lblTotalTellerCashOut.TabIndex = 2;
            this.lblTotalTellerCashOut.Text = "Total Teller Cash Out:";
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.gridBatchDetails);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 0);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(684, 388);
            this.pGroupBoxStandard1.TabIndex = 2;
            this.pGroupBoxStandard1.TabStop = false;
            // 
            // frmTlBatchDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.Controls.Add(this.gbTotalsInformation);
            this.Name = "frmTlBatchDetails";
            this.ScreenId = 11960;
            this.Text = "Teller Batch Details";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlBatchDetails_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlBatchDetails_PInitBeginEvent);
            this.gbTotalsInformation.ResumeLayout(false);
            this.gbTotalsInformation.PerformLayout();
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Init param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 3)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				batchId.Value = Convert.ToInt16(paramList[2]);

				// assign param to main business obj properties
				_busObjTlJrnl.BranchNo.Value = branchNo.Value;
				_busObjTlJrnl.DrawerNo.Value = drawerNo.Value;
				_busObjTlJrnl.BatchId.Value = batchId.Value;
				_busObjTlJrnl.OutputType.Value = 3;

				#region say no to default framework select
				this.AutoFetch = false;
				#endregion
			}

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.BatchDetails;
			this.CurrencyId = TellerVars.Instance.LocalCrncyId;

			base.InitParameters (paramList);

		}
		#endregion			

		#region events

		private ReturnType frmTlBatchDetails_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.MainBusinesObject = _busObjTlJrnl;

			return ReturnType.Success;
		}

		private void frmTlBatchDetails_PInitCompleteEvent()
		{
			EnableDisableVisibleLogic("FormComplete");
		}

		private void gridBatchDetails_BeforePopulate(object sender, GridPopulateArgs e)
		{
			this.gridBatchDetails.ListViewObject = _busObjTlJrnl;
			rowCounter = 0;
			dfTotalTransactions.Text = rowCounter.ToString();
			totalTlCashOut.Value = 0;
			totalTcdCashOut.Value = 0;
			totalTranAmt.Value =0;
		}

		private void gridBatchDetails_AfterPopulate(object sender, GridPopulateArgs e)
		{
			this.dfTotTellerCashOut.UnFormattedValue = totalTlCashOut.Value;
			this.dfTotTCDCashOut.UnFormattedValue = totalTcdCashOut.Value;
			this.dfTotalTransactionAmt.UnFormattedValue = totalTranAmt.Value;
		}

		private void gridBatchDetails_FetchRowDone(object sender, GridRowArgs e)
		{
			this.colAccount.UnFormattedValue = this.colAcctType.FormattedValue + " - " + this.colAcctNo.FormattedValue;
			
			if (colAcctType.Text.Trim() == GlobalVars.Instance.ML.RIM)
			{
				this.colAccount.UnFormattedValue = this.colAcctType.FormattedValue + " - " + Convert.ToString(this.colRimNo.UnFormattedValue);
			}

			if ( colTranCode.Text == "BAT" && colCheck.UnFormattedValue != null )
				rowCounter+= Convert.ToInt32( colCheck.UnFormattedValue ) ;
			else
				++rowCounter;

			dfTotalTransactions.Text = rowCounter.ToString();

			totalTlCashOut.Value = totalTlCashOut.Value + Convert.ToDecimal(this.colTellerCashOut.Text.Trim() == "" ? 0 : this.colTellerCashOut.UnFormattedValue);
			totalTcdCashOut.Value = totalTcdCashOut.Value + Convert.ToDecimal(this.colTCDCashOut.Text.Trim() == "" ? 0 : this.colTCDCashOut.UnFormattedValue);
			totalTranAmt.Value = totalTranAmt.Value + Convert.ToDecimal(this.colTranAmt.Text.Trim() == "" ? 0 : this.colTranAmt.UnFormattedValue);
		}

		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
			{
				if (_tellerVars.AdTlControl.TcdDevices.Value == GlobalVars.Instance.ML.Y)
				{
					this.colTCDCashOut.Visible = true;
					this.colTCDCashOut.Width = 84;
				}
				else
				{
					this.colTCDCashOut.Visible = false;
					this.colTCDCashOut.Width = 0;
				}
			}
		}
		#endregion

        private void dfTotTellerCashOut_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
