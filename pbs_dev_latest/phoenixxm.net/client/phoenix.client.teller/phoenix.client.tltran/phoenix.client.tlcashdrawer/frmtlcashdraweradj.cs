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
// File Name: frmTlCashDrawerAdj.cs
// NameSpace: Phoenix.Client.TlCashDrawer
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//01/09/2007	2		mselvaga	#71049 - Added adjusted beginning cash to drawer combo update.
//01/15/2007	3		mselvaga	#71417 - Fixed the after save action, the action will kick in only when user accepts #318343 and save adjustments.
//01/19/2007	4		mselvaga	#71475 - Moved drawer adjustment reset totals from finally block to after save action.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;
using GlacialComponents.Controls;

namespace Phoenix.Client.TlCashDrawer
{
	/// <summary>
	/// Summary description for frmTlCashDrawerAdj.
	/// </summary>
	public class frmTlCashDrawerAdj : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbDrawerStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblDrawerTeller;
		private Phoenix.Windows.Forms.PdfStandard dfDrawerTeller;
		private Phoenix.Windows.Forms.PLabelStandard lblPostingDate;
		private Phoenix.Windows.Forms.PdfStandard dfPostingDt;
		private Phoenix.Windows.Forms.PLabelStandard lblCurrentTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblNewTotals;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbDrawerTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblBeginningCash;
		private Phoenix.Windows.Forms.PdfCurrency dfBeginningCash;
		private Phoenix.Windows.Forms.PLabelStandard lblBeginningCash_Dup1;
		private Phoenix.Windows.Forms.PdfCurrency dfBeginningCashNew;
		private Phoenix.Windows.Forms.PLabelStandard lblCashIns;
		private Phoenix.Windows.Forms.PdfCurrency dfCashIns;
		private Phoenix.Windows.Forms.PLabelStandard lblCashIns_Dup1;
		private Phoenix.Windows.Forms.PdfCurrency dfCashInsNew;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOuts;
		private Phoenix.Windows.Forms.PdfCurrency dfCashOuts;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOuts_Dup1;
		private Phoenix.Windows.Forms.PdfCurrency dfCashOutsNew;

		#region Initialize

		private PSmallInt branchNo = new PSmallInt("BranchNo");
		private PSmallInt drawerNo = new PSmallInt("DrawerNo");
		private PDateTime postingDt = new PDateTime("PostingDt");
		private PSmallInt employeeId = new PSmallInt("EmployeeId");
		//		
		private TlDrawerBalances _busObjTlDrawerbal = new TlDrawerBalances();
		private TellerVars _tellerVars = TellerVars.Instance;
		private TlHelper _tellerHelper = new TlHelper();
		private PComboBoxStandard drawerCombo = null;
		private decimal _netCash = 0;

		#endregion

		public frmTlCashDrawerAdj()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.SupportedActions |= StandardAction.Save;
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
			this.gbDrawerStatus = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.lblNewTotals = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPostingDt = new Phoenix.Windows.Forms.PdfStandard();
			this.lblPostingDate = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfDrawerTeller = new Phoenix.Windows.Forms.PdfStandard();
			this.lblDrawerTeller = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblCurrentTotals = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbDrawerTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfCashOutsNew = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCashOuts_Dup1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfCashOuts = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCashOuts = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfCashInsNew = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCashIns_Dup1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfCashIns = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCashIns = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfBeginningCashNew = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblBeginningCash_Dup1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfBeginningCash = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblBeginningCash = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbDrawerStatus.SuspendLayout();
			this.gbDrawerTotals.SuspendLayout();
			this.SuspendLayout();
			// 
			// gbDrawerStatus
			// 
			this.gbDrawerStatus.Controls.Add(this.dfPostingDt);
			this.gbDrawerStatus.Controls.Add(this.lblPostingDate);
			this.gbDrawerStatus.Controls.Add(this.dfDrawerTeller);
			this.gbDrawerStatus.Controls.Add(this.lblDrawerTeller);
			this.gbDrawerStatus.Location = new System.Drawing.Point(4, 0);
			this.gbDrawerStatus.Name = "gbDrawerStatus";
			this.gbDrawerStatus.PhoenixUIControl.ObjectId = 1;
			this.gbDrawerStatus.Size = new System.Drawing.Size(684, 40);
			this.gbDrawerStatus.TabIndex = 0;
			this.gbDrawerStatus.TabStop = false;
			this.gbDrawerStatus.Text = "Drawer Status";
			// 
			// lblNewTotals
			// 
			this.lblNewTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblNewTotals.Location = new System.Drawing.Point(296, 16);
			this.lblNewTotals.Name = "lblNewTotals";
			this.lblNewTotals.PhoenixUIControl.ObjectId = 15;
			this.lblNewTotals.Size = new System.Drawing.Size(88, 20);
			this.lblNewTotals.TabIndex = 1;
			this.lblNewTotals.Text = "New Totals:";
			// 
			// dfPostingDt
			// 
			this.dfPostingDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.dfPostingDt.Location = new System.Drawing.Point(392, 16);
			this.dfPostingDt.Name = "dfPostingDt";
			this.dfPostingDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.dfPostingDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
			this.dfPostingDt.PhoenixUIControl.ObjectId = 3;
			this.dfPostingDt.PhoenixUIControl.XmlTag = "";
			this.dfPostingDt.ReadOnly = true;
			this.dfPostingDt.Size = new System.Drawing.Size(66, 20);
			this.dfPostingDt.TabIndex = 3;
			this.dfPostingDt.TabStop = false;
			// 
			// lblPostingDate
			// 
			this.lblPostingDate.Location = new System.Drawing.Point(296, 16);
			this.lblPostingDate.Name = "lblPostingDate";
			this.lblPostingDate.PhoenixUIControl.ObjectId = 3;
			this.lblPostingDate.Size = new System.Drawing.Size(88, 20);
			this.lblPostingDate.TabIndex = 2;
			this.lblPostingDate.Text = "Posting Date:";
			// 
			// dfDrawerTeller
			// 
			this.dfDrawerTeller.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfDrawerTeller.Location = new System.Drawing.Point(100, 16);
			this.dfDrawerTeller.Name = "dfDrawerTeller";
			this.dfDrawerTeller.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfDrawerTeller.PhoenixUIControl.ObjectId = 2;
			this.dfDrawerTeller.PhoenixUIControl.XmlTag = "PrevEmplId";
			this.dfDrawerTeller.ReadOnly = true;
			this.dfDrawerTeller.Size = new System.Drawing.Size(172, 20);
			this.dfDrawerTeller.TabIndex = 1;
			this.dfDrawerTeller.TabStop = false;
			// 
			// lblDrawerTeller
			// 
			this.lblDrawerTeller.Location = new System.Drawing.Point(4, 16);
			this.lblDrawerTeller.Name = "lblDrawerTeller";
			this.lblDrawerTeller.PhoenixUIControl.ObjectId = 2;
			this.lblDrawerTeller.Size = new System.Drawing.Size(92, 20);
			this.lblDrawerTeller.TabIndex = 0;
			this.lblDrawerTeller.Text = "Drawer / Teller:";
			// 
			// lblCurrentTotals
			// 
			this.lblCurrentTotals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCurrentTotals.Location = new System.Drawing.Point(4, 16);
			this.lblCurrentTotals.Name = "lblCurrentTotals";
			this.lblCurrentTotals.PhoenixUIControl.ObjectId = 14;
			this.lblCurrentTotals.Size = new System.Drawing.Size(94, 20);
			this.lblCurrentTotals.TabIndex = 0;
			this.lblCurrentTotals.Text = "Current Totals:";
			// 
			// gbDrawerTotals
			// 
			this.gbDrawerTotals.Controls.Add(this.dfCashOutsNew);
			this.gbDrawerTotals.Controls.Add(this.lblCashOuts_Dup1);
			this.gbDrawerTotals.Controls.Add(this.dfCashOuts);
			this.gbDrawerTotals.Controls.Add(this.lblCashOuts);
			this.gbDrawerTotals.Controls.Add(this.dfCashInsNew);
			this.gbDrawerTotals.Controls.Add(this.lblCashIns_Dup1);
			this.gbDrawerTotals.Controls.Add(this.dfCashIns);
			this.gbDrawerTotals.Controls.Add(this.lblCashIns);
			this.gbDrawerTotals.Controls.Add(this.dfBeginningCashNew);
			this.gbDrawerTotals.Controls.Add(this.lblBeginningCash_Dup1);
			this.gbDrawerTotals.Controls.Add(this.dfBeginningCash);
			this.gbDrawerTotals.Controls.Add(this.lblBeginningCash);
			this.gbDrawerTotals.Controls.Add(this.lblCurrentTotals);
			this.gbDrawerTotals.Controls.Add(this.lblNewTotals);
			this.gbDrawerTotals.Location = new System.Drawing.Point(4, 40);
			this.gbDrawerTotals.Name = "gbDrawerTotals";
			this.gbDrawerTotals.Size = new System.Drawing.Size(684, 112);
			this.gbDrawerTotals.TabIndex = 1;
			this.gbDrawerTotals.TabStop = false;
			this.gbDrawerTotals.Text = "Drawer Totals";
			// 
			// dfCashOutsNew
			// 
			this.dfCashOutsNew.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashOutsNew.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashOutsNew.Location = new System.Drawing.Point(392, 88);
			this.dfCashOutsNew.Name = "dfCashOutsNew";
			this.dfCashOutsNew.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashOutsNew.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashOutsNew.PhoenixUIControl.MaxPrecision = 14;
			this.dfCashOutsNew.PhoenixUIControl.ObjectId = 10;
			this.dfCashOutsNew.PhoenixUIControl.XmlTag = "CashOutNew";
			this.dfCashOutsNew.Size = new System.Drawing.Size(132, 20);
			this.dfCashOutsNew.TabIndex = 13;
			this.dfCashOutsNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblCashOuts_Dup1
			// 
			this.lblCashOuts_Dup1.Location = new System.Drawing.Point(296, 88);
			this.lblCashOuts_Dup1.Name = "lblCashOuts_Dup1";
			this.lblCashOuts_Dup1.PhoenixUIControl.ObjectId = 10;
			this.lblCashOuts_Dup1.Size = new System.Drawing.Size(88, 20);
			this.lblCashOuts_Dup1.TabIndex = 12;
			this.lblCashOuts_Dup1.Text = "Cash Outs:";
			// 
			// dfCashOuts
			// 
			this.dfCashOuts.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashOuts.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashOuts.Location = new System.Drawing.Point(100, 88);
			this.dfCashOuts.Name = "dfCashOuts";
			this.dfCashOuts.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashOuts.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashOuts.PhoenixUIControl.MaxPrecision = 14;
			this.dfCashOuts.PhoenixUIControl.ObjectId = 9;
			this.dfCashOuts.PhoenixUIControl.XmlTag = "CashOut";
			this.dfCashOuts.ReadOnly = true;
			this.dfCashOuts.Size = new System.Drawing.Size(132, 20);
			this.dfCashOuts.TabIndex = 11;
			this.dfCashOuts.TabStop = false;
			this.dfCashOuts.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblCashOuts
			// 
			this.lblCashOuts.Location = new System.Drawing.Point(4, 88);
			this.lblCashOuts.Name = "lblCashOuts";
			this.lblCashOuts.PhoenixUIControl.ObjectId = 9;
			this.lblCashOuts.Size = new System.Drawing.Size(92, 20);
			this.lblCashOuts.TabIndex = 10;
			this.lblCashOuts.Text = "Cash Outs:";
			// 
			// dfCashInsNew
			// 
			this.dfCashInsNew.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashInsNew.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashInsNew.Location = new System.Drawing.Point(392, 64);
			this.dfCashInsNew.Name = "dfCashInsNew";
			this.dfCashInsNew.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashInsNew.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashInsNew.PhoenixUIControl.MaxPrecision = 14;
			this.dfCashInsNew.PhoenixUIControl.ObjectId = 8;
			this.dfCashInsNew.PhoenixUIControl.XmlTag = "CashInNew";
			this.dfCashInsNew.Size = new System.Drawing.Size(132, 20);
			this.dfCashInsNew.TabIndex = 9;
			this.dfCashInsNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblCashIns_Dup1
			// 
			this.lblCashIns_Dup1.Location = new System.Drawing.Point(296, 64);
			this.lblCashIns_Dup1.Name = "lblCashIns_Dup1";
			this.lblCashIns_Dup1.PhoenixUIControl.ObjectId = 8;
			this.lblCashIns_Dup1.Size = new System.Drawing.Size(88, 20);
			this.lblCashIns_Dup1.TabIndex = 8;
			this.lblCashIns_Dup1.Text = "Cash Ins:";
			// 
			// dfCashIns
			// 
			this.dfCashIns.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashIns.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashIns.Location = new System.Drawing.Point(100, 64);
			this.dfCashIns.Name = "dfCashIns";
			this.dfCashIns.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashIns.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashIns.PhoenixUIControl.MaxPrecision = 14;
			this.dfCashIns.PhoenixUIControl.ObjectId = 7;
			this.dfCashIns.PhoenixUIControl.XmlTag = "CashIn";
			this.dfCashIns.ReadOnly = true;
			this.dfCashIns.Size = new System.Drawing.Size(132, 20);
			this.dfCashIns.TabIndex = 7;
			this.dfCashIns.TabStop = false;
			this.dfCashIns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblCashIns
			// 
			this.lblCashIns.Location = new System.Drawing.Point(4, 64);
			this.lblCashIns.Name = "lblCashIns";
			this.lblCashIns.PhoenixUIControl.ObjectId = 7;
			this.lblCashIns.Size = new System.Drawing.Size(92, 20);
			this.lblCashIns.TabIndex = 6;
			this.lblCashIns.Text = "Cash Ins:";
			// 
			// dfBeginningCashNew
			// 
			this.dfBeginningCashNew.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfBeginningCashNew.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfBeginningCashNew.Location = new System.Drawing.Point(392, 40);
			this.dfBeginningCashNew.Name = "dfBeginningCashNew";
			this.dfBeginningCashNew.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfBeginningCashNew.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfBeginningCashNew.PhoenixUIControl.MaxPrecision = 14;
			this.dfBeginningCashNew.PhoenixUIControl.ObjectId = 6;
			this.dfBeginningCashNew.PhoenixUIControl.XmlTag = "ClosingCashNew";
			this.dfBeginningCashNew.Size = new System.Drawing.Size(132, 20);
			this.dfBeginningCashNew.TabIndex = 5;
			this.dfBeginningCashNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblBeginningCash_Dup1
			// 
			this.lblBeginningCash_Dup1.Location = new System.Drawing.Point(296, 40);
			this.lblBeginningCash_Dup1.Name = "lblBeginningCash_Dup1";
			this.lblBeginningCash_Dup1.PhoenixUIControl.ObjectId = 6;
			this.lblBeginningCash_Dup1.Size = new System.Drawing.Size(88, 20);
			this.lblBeginningCash_Dup1.TabIndex = 4;
			this.lblBeginningCash_Dup1.Text = "Beginning Cash:";
			// 
			// dfBeginningCash
			// 
			this.dfBeginningCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfBeginningCash.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfBeginningCash.Location = new System.Drawing.Point(100, 40);
			this.dfBeginningCash.Name = "dfBeginningCash";
			this.dfBeginningCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfBeginningCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfBeginningCash.PhoenixUIControl.MaxPrecision = 14;
			this.dfBeginningCash.PhoenixUIControl.ObjectId = 5;
			this.dfBeginningCash.PhoenixUIControl.XmlTag = "ClosingCash";
			this.dfBeginningCash.ReadOnly = true;
			this.dfBeginningCash.Size = new System.Drawing.Size(132, 20);
			this.dfBeginningCash.TabIndex = 3;
			this.dfBeginningCash.TabStop = false;
			this.dfBeginningCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblBeginningCash
			// 
			this.lblBeginningCash.Location = new System.Drawing.Point(4, 40);
			this.lblBeginningCash.Name = "lblBeginningCash";
			this.lblBeginningCash.PhoenixUIControl.ObjectId = 5;
			this.lblBeginningCash.Size = new System.Drawing.Size(92, 20);
			this.lblBeginningCash.TabIndex = 2;
			this.lblBeginningCash.Text = "Beginning Cash:";
			// 
			// frmTlCashDrawerAdj
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.gbDrawerTotals);
			this.Controls.Add(this.gbDrawerStatus);
			this.Name = "frmTlCashDrawerAdj";
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCashDrawerAdj_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCashDrawerAdj_PInitCompleteEvent);
			this.gbDrawerStatus.ResumeLayout(false);
			this.gbDrawerTotals.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion		

		#region Init param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 4)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				postingDt.Value = Convert.ToDateTime(paramList[2]);
				employeeId.Value = Convert.ToInt16(paramList[3]);
				//
				_busObjTlDrawerbal.PrevEmplId.Value = employeeId.Value;
//				_busObjTlDrawerbal.EmplId.Value = employeeId.Value;
				_busObjTlDrawerbal.BranchNo.Value = branchNo.Value;
				_busObjTlDrawerbal.DrawerNo.Value = drawerNo.Value;
				_busObjTlDrawerbal.CrncyId.Value = _tellerVars.LocalCrncyId;
				_busObjTlDrawerbal.ClosedDt.Value = postingDt.Value;
				_busObjTlDrawerbal.SelectType.Value = 2;
				//
				if (paramList.Length == 5)
				{
					if (paramList[4] != null)
						drawerCombo = paramList[4] as PComboBoxStandard;
				}
			}
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.CashDrawerAdj;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			base.InitParameters (paramList);
		}
		#endregion

		#region standard actions
		public override bool OnActionSave(bool isAddNext)
		{
			string fieldName = "";
			string amountChange = "";
			//
			if (Convert.ToDecimal(dfBeginningCash.UnFormattedValue) != Convert.ToDecimal(dfBeginningCashNew.UnFormattedValue) ||
				Convert.ToDecimal(dfCashIns.UnFormattedValue) != Convert.ToDecimal(dfCashInsNew.UnFormattedValue) ||
				Convert.ToDecimal(dfCashOuts.UnFormattedValue) != Convert.ToDecimal(dfCashOutsNew.UnFormattedValue))
			{
				try
				{
					if (TellerVars.Instance.IsAppOnline)
					{					
						if (_busObjTlDrawerbal.UnbatchedAmt.Value > 0)
						{
							//The Teller Drawer contains items that have not been batched.  You must batch these items before continuing with the Cash Drawer Adjustment.
							PMessageBox.Show(this, 318345, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
							return false;
						}
						//
						if (Convert.ToDecimal(this.dfCashInsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashIns.UnFormattedValue) && 
							Convert.ToDecimal(this.dfCashOutsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashOuts.UnFormattedValue))
						{
							fieldName = "Cash In and Cash Out";
							amountChange = "Cash In amount of " + "$" + Convert.ToDecimal(dfCashIns.UnFormattedValue).ToString() +
								" and Cash Out amount of " + "$" + Convert.ToDecimal(dfCashOuts.UnFormattedValue).ToString();

						}
						else if (Convert.ToDecimal(this.dfCashInsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashIns.UnFormattedValue))
						{
							fieldName = "Cash In";
							amountChange = "Cash In amount of " + "$" + Convert.ToDecimal(dfCashIns.UnFormattedValue).ToString();
						}
						else if (Convert.ToDecimal(this.dfCashOutsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashOuts.UnFormattedValue))
						{
							fieldName = "Cash Out";
							amountChange = "Cash Out amount of " + "$" + Convert.ToDecimal(dfCashOuts.UnFormattedValue).ToString();
						}
						//
						if ( fieldName != "" && amountChange != "" )
						{
							//The %1! amount entered does not match the total %2! entered on the system for this teller drawer.  Are you sure you want to continue with the %3! amount entered?
							if ( DialogResult.No == PMessageBox.Show(this, 360506, MessageType.Question, MessageBoxButtons.YesNo, new string[] {fieldName, amountChange, fieldName}))
								return false;
						}
					}
					//
					//You are about to modify the Teller Drawer totals.  This action is not reversible.  Do you wish to continue? 
					if ( DialogResult.No == PMessageBox.Show(this, 318343, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
						return true;
					
					using (new WaitCursor())
					{
						dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360507));
						//
						this.dfBeginningCashNew.ScreenToObject();
						this.dfCashInsNew.ScreenToObject();
						this.dfCashOutsNew.ScreenToObject();
						_busObjTlDrawerbal.CustomActionName = "CashDrawerAdj";
						CallXMThruCDS("PostCashDraweAdj");
						//
						#region update drawer combo
						if (drawerCombo != null && !_tellerVars.IsAppOnline)
						{
							//#71049 - added beginning cash for drawer combo update
							if ((Convert.ToDecimal(this.dfCashInsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashIns.UnFormattedValue)) || 
								(Convert.ToDecimal(this.dfCashOutsNew.UnFormattedValue) != Convert.ToDecimal(this.dfCashOuts.UnFormattedValue)) || 
								(Convert.ToDecimal(this.dfBeginningCashNew.UnFormattedValue) != Convert.ToDecimal(this.dfBeginningCash.UnFormattedValue)))
							{
								_netCash = (Convert.ToDecimal(dfBeginningCashNew.UnFormattedValue) - Convert.ToDecimal(dfBeginningCash.UnFormattedValue)) + 
									(Convert.ToDecimal(dfCashInsNew.UnFormattedValue) - Convert.ToDecimal(dfCashIns.UnFormattedValue)) -
									(Convert.ToDecimal(dfCashOutsNew.UnFormattedValue) - Convert.ToDecimal(dfCashOuts.UnFormattedValue));
								UpdateDrawerBalance(_netCash);
							}
						}
						#endregion
						//
						#region after save
						EnableDisableVisibleLogic("AfterSave");
						#endregion
					}
				}
				catch (PhoenixException pe)
				{
					PMessageBox.Show( pe );
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
					//#71417 - commented out, kick in after save only when there is a real adjustment.
					//EnableDisableVisibleLogic("AfterSave");
				}
			}

			CallParent( "DrawerAdj" );
			
			return true;
			//return base.OnActionSave (isAddNext);
		}

		public override bool OnActionClose()
		{
			if (Convert.ToDecimal(dfBeginningCash.UnFormattedValue) != Convert.ToDecimal(dfBeginningCashNew.UnFormattedValue) ||
				Convert.ToDecimal(dfCashIns.UnFormattedValue) != Convert.ToDecimal(dfCashInsNew.UnFormattedValue) ||
				Convert.ToDecimal(dfCashOuts.UnFormattedValue) != Convert.ToDecimal(dfCashOutsNew.UnFormattedValue))
			{
				if ( DialogResult.No == PMessageBox.Show(this, 318344, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
					return true;

				if (!OnActionSave(false))
					return false;
			}
			return true;
			//return base.OnActionClose ();
		}		
		#endregion

		#region drawer adj events
		private ReturnType frmTlCashDrawerAdj_PInitBeginEvent()
		{
			this.MainBusinesObject = _busObjTlDrawerbal;
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.AvoidSave = true;
			//
			this.dfPostingDt.UnFormattedValue = postingDt.Value;
			this.ActionSave.NextScreenId = 0;
			return ReturnType.Success;
		}

		private void frmTlCashDrawerAdj_PInitCompleteEvent()
		{			
			#region Initialize
			this.dfDrawerTeller.Text = drawerNo.Value.ToString();
			if (_busObjTlDrawerbal.PrevEmplId.FKValue != null)
			{
				if (_tellerHelper.GetFKValueDesc(_busObjTlDrawerbal.PrevEmplId.Value, _busObjTlDrawerbal.PrevEmplId.FKValue) != "")
					this.dfDrawerTeller.Text = drawerNo.Value.ToString() + "/" + _tellerHelper.GetFKValueDesc(_busObjTlDrawerbal.PrevEmplId.Value, _busObjTlDrawerbal.PrevEmplId.FKValue);
			}
			//
			EnableDisableVisibleLogic("FormComplete");
			#endregion
		}
		#endregion

		#region private methods

		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "AfterSave")
			{
				//#71475
				this.dfBeginningCash.UnFormattedValue = this.dfBeginningCashNew.UnFormattedValue;
				this.dfCashIns.UnFormattedValue = this.dfCashInsNew.UnFormattedValue;
				this.dfCashOuts.UnFormattedValue = this.dfCashOutsNew.UnFormattedValue;				
				this.ActionSave.Enabled = false;
			}
			else if (callerInfo == "FormComplete")
			{
				this.dfDrawerTeller.SetObjectStatus(NullabilityState.Default,VisibilityState.Default,EnableState.DisableShowText);
				this.dfPostingDt.SetObjectStatus(NullabilityState.Default,VisibilityState.Default,EnableState.DisableShowText);
				this.dfBeginningCash.SetObjectStatus(NullabilityState.Default,VisibilityState.Default,EnableState.DisableShowText);
				this.dfCashIns.SetObjectStatus(NullabilityState.Default,VisibilityState.Default,EnableState.DisableShowText);
				this.dfCashOuts.SetObjectStatus(NullabilityState.Default,VisibilityState.Default,EnableState.DisableShowText);
			}
		}

		private void CallXMThruCDS(string origin)
		{
			if (origin == "PostCashDraweAdj")
			{
				_busObjTlDrawerbal.GlobalEmplId.Value = _tellerVars.EmployeeId;
				DataService.Instance.ProcessCustomAction(_busObjTlDrawerbal, "CashDrawerAdj");
				if ( TellerVars.Instance.OfflineCDS != null )
				{
					PBaseType DwrUpdateOnly = new PBaseType("A1");
					DwrUpdateOnly.ValueObject = true;
					TellerVars.Instance.OfflineCDS.ProcessCustomAction(_busObjTlDrawerbal, "CashDrawerAdj", DwrUpdateOnly );
				}
			}
		}

		private void UpdateDrawerBalance(decimal netCash)
		{
			_tellerVars.DrawerCash = _tellerVars.DrawerCash + netCash;
			if ( drawerCombo != null )
			{
				drawerCombo.Items.Clear();
				drawerCombo.Items.Add(CurrencyHelper.GetFormattedValue( _tellerVars.DrawerCash ));
				drawerCombo.SelectedIndex = 0;

				// TO DO update the drawer combo
				if ( _tellerVars.DrawerCash < _tellerVars.AdGbRsmLimits.LowDrawerLim.Value )
				{
					drawerCombo.ForeColor = System.Drawing.Color.Blue;
					PMessageBox.Show( 311350, MessageType.Warning, MessageBoxButtons.OK, "USD" );
				}
				else if ( _tellerVars.DrawerCash > _tellerVars.AdGbRsmLimits.HighDrawerLim.Value )
				{
					drawerCombo.ForeColor = System.Drawing.Color.Red;
					PMessageBox.Show( 311349, MessageType.Warning, MessageBoxButtons.OK, "USD" );
				}
				else
					drawerCombo.ForeColor = System.Drawing.Color.Black;
			}
		}
		#endregion

	}
}
