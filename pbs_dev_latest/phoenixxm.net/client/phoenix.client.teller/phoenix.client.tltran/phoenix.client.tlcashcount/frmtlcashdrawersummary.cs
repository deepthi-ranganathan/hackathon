#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name:frmTlCashDrawerSummary.cs
// NameSpace:Phoenix.Client.TlCashCount
//-------------------------------------------------------------------------------
///Date        	Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/19/2006	2		mselvaga	#71193 - Removed save action button.
//01/02/2007	3		mselvaga	#71263 - Fixed incorrect difference column value after close out.
//01/23/2007	4		Vreddy		#71440- Browser object is kiiling the form so I am moving the creation back to Printing
//02/01/2007	5		Vreddy		#71649 - Alignment of decimal columns
//02/28/2007	6		rpoddar		#71886 - Cache CashCount globally
//04/04/2007	7		mselvaga	#71893 - Added strap split changes.
//04/10/2007	8		Vreddy		#72361 - Teller 2007 - AD_GB_RSM.employee_id is printing on the Teller Summary Position and Cash Summary Reports, it should be AD_GB_RSM.teller_no
//06/18/2007    9       Vreddy      #73125 - Printer layout setting is rearranged
//12/17/2012    10      mselvaga    #140772 - Inventory Tracking Changes added.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GlacialComponents.Controls.Common;
using GlacialComponents.Controls;
//
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Windows.Client;
//Report
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Misc;
using Phoenix.Shared.Windows;
//
namespace Phoenix.Client.TlCashCount
{
	/// <summary>
	/// Summary description for frmTlCashDrawerSummary.
	/// </summary>
	public class frmTlCashDrawerSummary : Phoenix.Windows.Forms.PfwStandard
	{		

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblCashDrawerCount;
		private Phoenix.Windows.Forms.PDfDisplay dfCashDrawerCount;
		private Phoenix.Windows.Forms.PLabelStandard lblEndingCash;
		private Phoenix.Windows.Forms.PDfDisplay dfEndingCash;
		private Phoenix.Windows.Forms.PLabelStandard lblDifference;
		private Phoenix.Windows.Forms.PDfDisplay dfDifference;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbCashDrawerCountSumTotals;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbIncludeCashInCashOutAmtDenom;
		private Phoenix.Windows.Forms.PGrid gridDrawerCashCountSummary;
		private Phoenix.Windows.Forms.PGridColumn colCashIn;
		private Phoenix.Windows.Forms.PGridColumn colCashOut;
		private Phoenix.Windows.Forms.PGridColumn colNetCash;
		private Phoenix.Windows.Forms.PGridColumn colCashDrawerCount;
		private Phoenix.Windows.Forms.PGridColumn colEndingCash;
		private Phoenix.Windows.Forms.PGridColumn colDifference;
		private Phoenix.Windows.Forms.PGridColumn colDonomDesc;
		private Phoenix.Windows.Forms.PGridColumn colBeginningCash;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard2;
		private Phoenix.Windows.Forms.PPanel pPanel1;
		private Phoenix.Windows.Forms.PPanel pPanel2;
		private Phoenix.Windows.Forms.PPanel pPanel3;
		private Phoenix.Windows.Forms.PAction pbCashCount;
		private Phoenix.Windows.Forms.PAction pbJournal;
		private Phoenix.Windows.Forms.PGridColumn colDenom;
		private Phoenix.Windows.Forms.PGridColumn colBranchNo;
		private Phoenix.Windows.Forms.PGridColumn colDrawerNo;
		private Phoenix.Windows.Forms.PGridColumn colUseDescDenom;
		private Phoenix.Windows.Forms.PGridColumn colDenomId;
		private Phoenix.Windows.Forms.PGridColumn colDenomType;
		//
		#region Initialize
		private TlCashPositionSummary _busTlCashCountSummary = new TlCashPositionSummary();
		private Phoenix.BusObj.Teller.TlCashCount _busObjTlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
		private TellerVars _tellerVars = TellerVars.Instance;
		private ArrayList _cashCountSummary = new ArrayList();
		private ArrayList _cashCount = new ArrayList();
        private ArrayList _savedTlInvItems = new ArrayList();  //#140772
		//
		private PSmallInt _branchNo = new PSmallInt();
		private PSmallInt _drawerNo = new PSmallInt();
		private PDateTime _postingDt = new PDateTime();
		private PDecimal _endingCash = new PDecimal();
		private PDecimal _positionPtid = new PDecimal();
		//
		private bool _isViewOnly = false;
		private string _screenTitle = "";
		private decimal _drawerCountTotal = 0;
		//Report Specific
		private AdGbRsm _adGbRsmTemp = new AdGbRsm();
		private AdGbBranch _adGbBranchBusObj;
		private Phoenix.BusObj.Teller.TlPosition _tlPositionTemp = new Phoenix.BusObj.Teller.TlPosition();
		private TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
		Phoenix.Shared.Windows.HtmlPrinter _htmlPrinter; // = new HtmlPrinter();
		Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;
		Phoenix.Shared.Windows.PSetting _printerSettings = new PSetting();
		private Phoenix.Windows.Forms.PGridColumn colMadeAmt;
		private Phoenix.Windows.Forms.PGridColumn colBrokeAmt;
		private Phoenix.Windows.Forms.PGridColumn colNetMadeBrokeAmt;
		System.Diagnostics.Process _printProcess = null;
		//Report Specific
		#endregion

		public frmTlCashDrawerSummary()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
//			this.SupportedActions |= StandardAction.Save;
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
				if (_htmlPrinter != null)
				{
					//_htmlPrinter.Dispose();
                    //73125
                    if (!TellerVars.Instance.IsAppOnline && _printerSettings != null)
                        _printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);

				}
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
			this.gridDrawerCashCountSummary = new Phoenix.Windows.Forms.PGrid();
			this.colDonomDesc = new Phoenix.Windows.Forms.PGridColumn();
			this.colBeginningCash = new Phoenix.Windows.Forms.PGridColumn();
			this.colCashIn = new Phoenix.Windows.Forms.PGridColumn();
			this.colCashOut = new Phoenix.Windows.Forms.PGridColumn();
			this.colNetCash = new Phoenix.Windows.Forms.PGridColumn();
			this.colMadeAmt = new Phoenix.Windows.Forms.PGridColumn();
			this.colBrokeAmt = new Phoenix.Windows.Forms.PGridColumn();
			this.colNetMadeBrokeAmt = new Phoenix.Windows.Forms.PGridColumn();
			this.colCashDrawerCount = new Phoenix.Windows.Forms.PGridColumn();
			this.colEndingCash = new Phoenix.Windows.Forms.PGridColumn();
			this.colDifference = new Phoenix.Windows.Forms.PGridColumn();
			this.colDenom = new Phoenix.Windows.Forms.PGridColumn();
			this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colUseDescDenom = new Phoenix.Windows.Forms.PGridColumn();
			this.colDenomId = new Phoenix.Windows.Forms.PGridColumn();
			this.colDenomType = new Phoenix.Windows.Forms.PGridColumn();
			this.gbTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.pPanel3 = new Phoenix.Windows.Forms.PPanel();
			this.pPanel2 = new Phoenix.Windows.Forms.PPanel();
			this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
			this.pLabelStandard2 = new Phoenix.Windows.Forms.PLabelStandard();
			this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfDifference = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblDifference = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfEndingCash = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblEndingCash = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfCashDrawerCount = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblCashDrawerCount = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbCashDrawerCountSumTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.cbIncludeCashInCashOutAmtDenom = new Phoenix.Windows.Forms.PCheckBoxStandard();
			this.pbCashCount = new Phoenix.Windows.Forms.PAction();
			this.pbJournal = new Phoenix.Windows.Forms.PAction();
			this.gbTotals.SuspendLayout();
			this.gbCashDrawerCountSumTotals.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionManager
			// 
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
																						this.pbCashCount,
																						this.pbJournal});
			// 
			// gridDrawerCashCountSummary
			// 
			this.gridDrawerCashCountSummary.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
																										 this.colDonomDesc,
																										 this.colBeginningCash,
																										 this.colCashIn,
																										 this.colCashOut,
																										 this.colNetCash,
																										 this.colMadeAmt,
																										 this.colBrokeAmt,
																										 this.colNetMadeBrokeAmt,
																										 this.colCashDrawerCount,
																										 this.colEndingCash,
																										 this.colDifference,
																										 this.colDenom,
																										 this.colBranchNo,
																										 this.colDrawerNo,
																										 this.colUseDescDenom,
																										 this.colDenomId,
																										 this.colDenomType});
			this.gridDrawerCashCountSummary.LinesInHeader = 2;
			this.gridDrawerCashCountSummary.Location = new System.Drawing.Point(4, 40);
			this.gridDrawerCashCountSummary.Name = "gridDrawerCashCountSummary";
			this.gridDrawerCashCountSummary.Size = new System.Drawing.Size(676, 316);
			this.gridDrawerCashCountSummary.TabIndex = 0;
			this.gridDrawerCashCountSummary.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridDrawerCashCountSummary_FetchRowDone);
			this.gridDrawerCashCountSummary.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridDrawerCashCountSummary_BeforePopulate);
			this.gridDrawerCashCountSummary.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridDrawerCashCountSummary_AfterPopulate);
			// 
			// colDonomDesc
			// 
			this.colDonomDesc.PhoenixUIControl.ObjectId = 4;
			this.colDonomDesc.PhoenixUIControl.XmlTag = "CashDenomDesc";
			this.colDonomDesc.Title = "Denomination/Description";
			this.colDonomDesc.Width = 143;
			// 
			// colBeginningCash
			// 
			this.colBeginningCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colBeginningCash.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colBeginningCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colBeginningCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colBeginningCash.PhoenixUIControl.ObjectId = 5;
			this.colBeginningCash.PhoenixUIControl.XmlTag = "BeginningCash";
			this.colBeginningCash.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colBeginningCash.Title = "Beginning Cash Amount";
			this.colBeginningCash.Width = 85;
			// 
			// colCashIn
			// 
			this.colCashIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashIn.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashIn.PhoenixUIControl.ObjectId = 6;
			this.colCashIn.PhoenixUIControl.XmlTag = "CashIn";
			this.colCashIn.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colCashIn.Title = "Cash In";
			this.colCashIn.Width = 85;
			// 
			// colCashOut
			// 
			this.colCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashOut.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashOut.PhoenixUIControl.ObjectId = 7;
			this.colCashOut.PhoenixUIControl.XmlTag = "CashOut";
			this.colCashOut.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colCashOut.Title = "Cash Out";
			this.colCashOut.Width = 85;
			// 
			// colNetCash
			// 
			this.colNetCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colNetCash.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colNetCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colNetCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colNetCash.PhoenixUIControl.ObjectId = 8;
			this.colNetCash.PhoenixUIControl.XmlTag = "NetCash";
			this.colNetCash.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colNetCash.Title = "Net Cash In/ Cash Out";
			this.colNetCash.Width = 85;
			// 
			// colMadeAmt
			// 
			this.colMadeAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colMadeAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colMadeAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colMadeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colMadeAmt.PhoenixUIControl.ObjectId = 20;
			this.colMadeAmt.PhoenixUIControl.XmlTag = "Made";
			this.colMadeAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colMadeAmt.Title = "Made Amount";
			this.colMadeAmt.Width = 85;
			// 
			// colBrokeAmt
			// 
			this.colBrokeAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colBrokeAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colBrokeAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colBrokeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colBrokeAmt.PhoenixUIControl.ObjectId = 21;
			this.colBrokeAmt.PhoenixUIControl.XmlTag = "Broke";
			this.colBrokeAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colBrokeAmt.Title = "Broke Amount";
			this.colBrokeAmt.Width = 85;
			// 
			// colNetMadeBrokeAmt
			// 
			this.colNetMadeBrokeAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colNetMadeBrokeAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colNetMadeBrokeAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colNetMadeBrokeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colNetMadeBrokeAmt.PhoenixUIControl.ObjectId = 22;
			this.colNetMadeBrokeAmt.PhoenixUIControl.XmlTag = "NetMadeBroke";
			this.colNetMadeBrokeAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colNetMadeBrokeAmt.Title = "Net Made/Broke";
			this.colNetMadeBrokeAmt.Width = 85;
			// 
			// colCashDrawerCount
			// 
			this.colCashDrawerCount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashDrawerCount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashDrawerCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCashDrawerCount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCashDrawerCount.PhoenixUIControl.ObjectId = 9;
			this.colCashDrawerCount.PhoenixUIControl.XmlTag = "DrawerCount";
			this.colCashDrawerCount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colCashDrawerCount.Title = "Cash Drawer Count";
			this.colCashDrawerCount.Width = 85;
			// 
			// colEndingCash
			// 
			this.colEndingCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colEndingCash.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colEndingCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colEndingCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colEndingCash.PhoenixUIControl.ObjectId = 10;
			this.colEndingCash.PhoenixUIControl.XmlTag = "EndingCash";
			this.colEndingCash.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colEndingCash.Title = "Ending Cash";
			this.colEndingCash.Width = 85;
			// 
			// colDifference
			// 
			this.colDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colDifference.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colDifference.PhoenixUIControl.ObjectId = 11;
			this.colDifference.PhoenixUIControl.XmlTag = "Difference";
			this.colDifference.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colDifference.Title = "Difference";
			this.colDifference.Width = 85;
			// 
			// colDenom
			// 
			this.colDenom.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colDenom.PhoenixUIControl.XmlTag = "Denom";
			this.colDenom.Title = "Denomination";
			this.colDenom.Visible = false;
			// 
			// colBranchNo
			// 
			this.colBranchNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
			this.colBranchNo.Title = "Branch No";
			this.colBranchNo.Visible = false;
			// 
			// colDrawerNo
			// 
			this.colDrawerNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
			this.colDrawerNo.Title = "Drawer No";
			this.colDrawerNo.Visible = false;
			// 
			// colUseDescDenom
			// 
			this.colUseDescDenom.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colUseDescDenom.PhoenixUIControl.XmlTag = "UseDescDenom";
			this.colUseDescDenom.Title = "Use Desc Denom";
			this.colUseDescDenom.Visible = false;
			// 
			// colDenomId
			// 
			this.colDenomId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colDenomId.PhoenixUIControl.XmlTag = "DenomId";
			this.colDenomId.Title = "Denom Id";
			this.colDenomId.Visible = false;
			// 
			// colDenomType
			// 
			this.colDenomType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colDenomType.PhoenixUIControl.XmlTag = "DenomType";
			this.colDenomType.Title = "Denom Type";
			this.colDenomType.Visible = false;
			// 
			// gbTotals
			// 
			this.gbTotals.Controls.Add(this.pPanel3);
			this.gbTotals.Controls.Add(this.pPanel2);
			this.gbTotals.Controls.Add(this.pPanel1);
			this.gbTotals.Controls.Add(this.pLabelStandard2);
			this.gbTotals.Controls.Add(this.pLabelStandard1);
			this.gbTotals.Controls.Add(this.dfDifference);
			this.gbTotals.Controls.Add(this.lblDifference);
			this.gbTotals.Controls.Add(this.dfEndingCash);
			this.gbTotals.Controls.Add(this.lblEndingCash);
			this.gbTotals.Controls.Add(this.dfCashDrawerCount);
			this.gbTotals.Controls.Add(this.lblCashDrawerCount);
			this.gbTotals.Location = new System.Drawing.Point(4, 368);
			this.gbTotals.Name = "gbTotals";
			this.gbTotals.PhoenixUIControl.ObjectId = 12;
			this.gbTotals.Size = new System.Drawing.Size(684, 76);
			this.gbTotals.TabIndex = 1;
			this.gbTotals.TabStop = false;
			this.gbTotals.Text = "Teller Balancing Cash Position";
			// 
			// pPanel3
			// 
			this.pPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pPanel3.Location = new System.Drawing.Point(472, 40);
			this.pPanel3.Name = "pPanel3";
			this.pPanel3.Size = new System.Drawing.Size(204, 1);
			this.pPanel3.TabIndex = 9;
			this.pPanel3.TabStop = true;
			// 
			// pPanel2
			// 
			this.pPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pPanel2.Location = new System.Drawing.Point(236, 40);
			this.pPanel2.Name = "pPanel2";
			this.pPanel2.Size = new System.Drawing.Size(204, 1);
			this.pPanel2.TabIndex = 7;
			this.pPanel2.TabStop = true;
			// 
			// pPanel1
			// 
			this.pPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pPanel1.Location = new System.Drawing.Point(4, 40);
			this.pPanel1.Name = "pPanel1";
			this.pPanel1.Size = new System.Drawing.Size(204, 1);
			this.pPanel1.TabIndex = 5;
			this.pPanel1.TabStop = true;
			// 
			// pLabelStandard2
			// 
			this.pLabelStandard2.Location = new System.Drawing.Point(456, 16);
			this.pLabelStandard2.Name = "pLabelStandard2";
			this.pLabelStandard2.PhoenixUIControl.ObjectId = 17;
			this.pLabelStandard2.Size = new System.Drawing.Size(8, 20);
			this.pLabelStandard2.TabIndex = 3;
			this.pLabelStandard2.Text = "=";
			// 
			// pLabelStandard1
			// 
			this.pLabelStandard1.Location = new System.Drawing.Point(220, 16);
			this.pLabelStandard1.Name = "pLabelStandard1";
			this.pLabelStandard1.PhoenixUIControl.ObjectId = 16;
			this.pLabelStandard1.Size = new System.Drawing.Size(8, 20);
			this.pLabelStandard1.TabIndex = 1;
			this.pLabelStandard1.Text = "-";
			// 
			// dfDifference
			// 
			this.dfDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dfDifference.Location = new System.Drawing.Point(476, 16);
			this.dfDifference.Multiline = true;
			this.dfDifference.Name = "dfDifference";
			this.dfDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfDifference.PhoenixUIControl.ObjectId = 15;
			this.dfDifference.Size = new System.Drawing.Size(200, 20);
			this.dfDifference.TabIndex = 4;
			// 
			// lblDifference
			// 
			this.lblDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblDifference.Location = new System.Drawing.Point(548, 48);
			this.lblDifference.Name = "lblDifference";
			this.lblDifference.PhoenixUIControl.ObjectId = 15;
			this.lblDifference.Size = new System.Drawing.Size(76, 20);
			this.lblDifference.TabIndex = 10;
			this.lblDifference.Text = "Difference";
			// 
			// dfEndingCash
			// 
			this.dfEndingCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dfEndingCash.Location = new System.Drawing.Point(240, 16);
			this.dfEndingCash.Multiline = true;
			this.dfEndingCash.Name = "dfEndingCash";
			this.dfEndingCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfEndingCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfEndingCash.PhoenixUIControl.ObjectId = 14;
			this.dfEndingCash.Size = new System.Drawing.Size(204, 20);
			this.dfEndingCash.TabIndex = 2;
			// 
			// lblEndingCash
			// 
			this.lblEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblEndingCash.Location = new System.Drawing.Point(308, 48);
			this.lblEndingCash.Name = "lblEndingCash";
			this.lblEndingCash.PhoenixUIControl.ObjectId = 14;
			this.lblEndingCash.Size = new System.Drawing.Size(92, 20);
			this.lblEndingCash.TabIndex = 8;
			this.lblEndingCash.Text = "Ending Cash";
			// 
			// dfCashDrawerCount
			// 
			this.dfCashDrawerCount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dfCashDrawerCount.Location = new System.Drawing.Point(4, 16);
			this.dfCashDrawerCount.Multiline = true;
			this.dfCashDrawerCount.Name = "dfCashDrawerCount";
			this.dfCashDrawerCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfCashDrawerCount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfCashDrawerCount.PhoenixUIControl.ObjectId = 13;
			this.dfCashDrawerCount.Size = new System.Drawing.Size(204, 20);
			this.dfCashDrawerCount.TabIndex = 0;
			// 
			// lblCashDrawerCount
			// 
			this.lblCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCashDrawerCount.Location = new System.Drawing.Point(52, 48);
			this.lblCashDrawerCount.Name = "lblCashDrawerCount";
			this.lblCashDrawerCount.PhoenixUIControl.ObjectId = 13;
			this.lblCashDrawerCount.Size = new System.Drawing.Size(124, 20);
			this.lblCashDrawerCount.TabIndex = 6;
			this.lblCashDrawerCount.Text = "Cash Drawer Count";
			// 
			// gbCashDrawerCountSumTotals
			// 
			this.gbCashDrawerCountSumTotals.Controls.Add(this.cbIncludeCashInCashOutAmtDenom);
			this.gbCashDrawerCountSumTotals.Controls.Add(this.gridDrawerCashCountSummary);
			this.gbCashDrawerCountSumTotals.Location = new System.Drawing.Point(4, 4);
			this.gbCashDrawerCountSumTotals.Name = "gbCashDrawerCountSumTotals";
			this.gbCashDrawerCountSumTotals.PhoenixUIControl.ObjectId = 1;
			this.gbCashDrawerCountSumTotals.Size = new System.Drawing.Size(684, 364);
			this.gbCashDrawerCountSumTotals.TabIndex = 0;
			this.gbCashDrawerCountSumTotals.TabStop = false;
			this.gbCashDrawerCountSumTotals.Text = "Cash Drawer Count Summary Totals";
			// 
			// cbIncludeCashInCashOutAmtDenom
			// 
			this.cbIncludeCashInCashOutAmtDenom.CodeValue = null;
			this.cbIncludeCashInCashOutAmtDenom.Location = new System.Drawing.Point(380, 16);
			this.cbIncludeCashInCashOutAmtDenom.Name = "cbIncludeCashInCashOutAmtDenom";
			this.cbIncludeCashInCashOutAmtDenom.PhoenixUIControl.ObjectId = 2;
			this.cbIncludeCashInCashOutAmtDenom.Size = new System.Drawing.Size(296, 20);
			this.cbIncludeCashInCashOutAmtDenom.TabIndex = 1;
			this.cbIncludeCashInCashOutAmtDenom.Text = "Include Cash In and Cash Out Amounts by Denomination";
			this.cbIncludeCashInCashOutAmtDenom.Click += new System.EventHandler(this.cbIncludeCashInCashOutAmtDenom_Click);
			// 
			// pbCashCount
			// 
			this.pbCashCount.ObjectId = 18;
			this.pbCashCount.ShortText = "Cash Count";
			this.pbCashCount.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCashCount_Click);
			// 
			// pbJournal
			// 
			this.pbJournal.ObjectId = 19;
			this.pbJournal.ShortText = "Journal";
			this.pbJournal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbJournal_Click);
			// 
			// frmTlCashDrawerSummary
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.gbCashDrawerCountSumTotals);
			this.Controls.Add(this.gbTotals);
			this.Name = "frmTlCashDrawerSummary";
			this.ScreenId = 16006;
			this.PMdiPrintEvent += new System.EventHandler(this.frmTlCashDrawerSummary_PMdiPrintEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCashDrawerSummary_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCashDrawerSummary_PInitCompleteEvent);
			this.gbTotals.ResumeLayout(false);
			this.gbCashDrawerCountSumTotals.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 5)
			{
				if (paramList[0] != null)
					_branchNo.Value = Convert.ToInt16(paramList[0]);
				if (paramList[1] != null)
					_drawerNo.Value = Convert.ToInt16(paramList[1]);
				if (paramList[2] != null)
					_postingDt.Value = Convert.ToDateTime(paramList[2]);
				if (paramList[3] != null)
					_endingCash.Value = Convert.ToDecimal(paramList[3]);
				if (paramList[4] != null)
				{
					_positionPtid.Value = Convert.ToDecimal(paramList[4]);
					if (_positionPtid.Value > 0)
						_isViewOnly = true;
				}
				if (paramList[5] != null)
				{
					_cashCountSummary = (ArrayList)paramList[5];
				}
				if (paramList[6] != null)
				{
					_cashCount = (ArrayList)paramList[6];
				}
                if (paramList.Length >= 7)
                {
                    if (paramList[7] != null)   //#140772
                    {
                        _savedTlInvItems = (ArrayList)paramList[7];
                    }
                }
			}
			//
			#region say no to default framework select
			this.AutoFetch = false;
			#endregion
			//
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals;
			this.CurrencyId = _tellerVars.LocalCrncyId;
			//
			#region skip populate
			if (!_isViewOnly)
			{
				gridDrawerCashCountSummary.SkipPopulate = true;
			}
			#endregion
			//
			base.InitParameters (paramList);
		}

		#endregion

		#region standard actions 
		public override bool OnActionClose()
		{
			if (_isViewOnly)
				return true;
			else
			{
				LoadCashSummary(false);
				CalcTotal();
				return true;
			}
		}
		#endregion

		#region cash drawer count summary events
		private ReturnType frmTlCashDrawerSummary_PInitBeginEvent()
		{
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = _busTlCashCountSummary;
			//
			#region security
			this.pbCashCount.NextScreenId = Phoenix.Shared.Constants.ScreenId.CashCount;
			this.pbJournal.NextScreenId = Phoenix.Shared.Constants.ScreenId.Journal;
			#endregion
			//
			return ReturnType.Success;
		}

		private void frmTlCashDrawerSummary_PInitCompleteEvent()
		{
			
			#region title
			_screenTitle = CoreService.Translation.GetTokenizeMessageX(360676, new string[] {Convert.ToString(_drawerNo.Value)});
			this.NewRecordTitle = string.Format(NewRecordTitle, _screenTitle);
			this.EditRecordTitle = string.Format(EditRecordTitle, _screenTitle);
			#endregion
			//
			this.dfEndingCash.UnFormattedValue = _endingCash.Value;
			this.cbIncludeCashInCashOutAmtDenom.Checked = false;
			EnableDisableVisibleLogic("IncludeCashInCashOutAmts");
			//
			#region load from summary position arraylist
			if (!_isViewOnly)
			{
				LoadCashSummary(true);
			}
			#endregion
			//
			EnableDisableVisibleLogic("FormComplete");
		}

		private void pbCashCount_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("CashCount");
		}

		private void pbJournal_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("Journal");
		}

		private void cbIncludeCashInCashOutAmtDenom_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("IncludeCashInCashOutAmts");
		}

		private void gridDrawerCashCountSummary_BeforePopulate(object sender, GridPopulateArgs e)
		{
			if (_isViewOnly)
			{
				_busTlCashCountSummary.OutputTypeId.Value = 1;
				_busTlCashCountSummary.PositionPtid.Value = (int)_positionPtid.Value;
				gridDrawerCashCountSummary.ListViewObject = _busTlCashCountSummary;
			}
		}

		private void tempWin_Closed(object sender, EventArgs e)
		{
			Form form = sender as Form;
//			int nRow = 0;
			if (_cashCount.Count > 0)
			{
				//#71893
				#region repopulate make/break info
				_busTlCashCountSummary.GetMadeBrokeInfo(_cashCountSummary);
				#endregion
				//
				#region load drawer count
				foreach(Phoenix.BusObj.Teller.TlCashCount count in _cashCount)
				{
					foreach(TlCashPositionSummary posSum in _cashCountSummary)
					{
						if (posSum.DenomId.Value == count.DenomId.Value && 
							!count.Amt.IsNull && count.Amt.Value != DbDecimal.MinValue &&
							count.Amt.Value != DbDecimal.MaxValue)
						{
							posSum.DrawerCount.Value = count.Amt.Value;
							break;
						}
					}
				}
				#endregion
				//
				#region recalculate totals
				_busTlCashCountSummary.LoadDrawerCashPosSummary(_cashCountSummary, true);
				#endregion
				//
				object objectValue = null;
				int tmpDenomId = 0;
				//
				for( int rowId = 0; rowId < gridDrawerCashCountSummary.Count; rowId++ )
				{
					tmpDenomId = 0;
					//
					objectValue = gridDrawerCashCountSummary.GetCellValueUnformatted( rowId, colDenomId.ColumnId );
					if (objectValue != null)
						tmpDenomId = Convert.ToInt32(objectValue);
					//
					foreach(TlCashPositionSummary posSum in _cashCountSummary)
					{
						if (posSum.DenomId.Value == tmpDenomId)
						{
							if (!posSum.Made.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colMadeAmt.ColumnId, posSum.Made.Value);
							if (!posSum.Broke.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colBrokeAmt.ColumnId, posSum.Broke.Value);
							if (!posSum.NetMadeBroke.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colNetMadeBrokeAmt.ColumnId, posSum.NetMadeBroke.Value);
							//
							if (!posSum.EndingCash.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colEndingCash.ColumnId, posSum.EndingCash.Value);
							if (!posSum.DrawerCount.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colCashDrawerCount.ColumnId, posSum.DrawerCount.Value);
							if (!posSum.Difference.IsNull)
								gridDrawerCashCountSummary.SetCellValueUnFormatted(rowId, colDifference.ColumnId, posSum.Difference.Value);
							break;
						}
					}
				}

//				nRow = 0;
//				while (nRow <= gridDrawerCashCountSummary.Count)
//				{
//					gridDrawerCashCountSummary.SelectRow(nRow, false);
//					//
//					foreach(Phoenix.BusObj.Teller.TlCashCount count in _cashCount)
//					{
//						if (Convert.ToInt32(colDenomId.UnFormattedValue) == count.DenomId.Value)
//						{
//							if (!count.Amt.IsNull)
//							{
//								colCashDrawerCount.FieldToColumn(count.Amt);
//								colDifference.UnFormattedValue = Convert.ToDecimal(colCashDrawerCount.UnFormattedValue) - 
//									Convert.ToDecimal(colEndingCash.UnFormattedValue);
//							}
//							break;
//						}
//					}
//					//
//					nRow = nRow + 1;
//				}
				//
				CalcTotal();
			}
		}

		#endregion

		#region private methods
		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;

			if ( origin == "Journal" )
			{
				tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournal");
				tempWin.InitParameters(GlobalVars.Instance.ML.N, 
					_branchNo.Value, 
					_drawerNo.Value,
					_postingDt.Value, 
					GlobalVars.Instance.ML.Open);
			}
			else if ( origin == "CashCount" )
			{
				if (_isViewOnly)
				{
					tempWin = Helper.CreateWindow( "phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "frmTranDenomDetails" );
					tempWin.InitParameters( _positionPtid.Value,
						null, //cash in
						null, //cash out
						null, //tcd cash in
						null, //tcd cash out
						null, //net amt
						null, //batch id
						null, //sub sequence
						this.ScreenId,
						_endingCash.Value);
				}
				else
				{
					tempWin = Helper.CreateWindow( "phoenix.client.tlcashcount", "Phoenix.Client.TlCashCount", "dlgTlCashCount" );
					tempWin.InitParameters( _endingCash.Value, 
						CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "C"), 
						false,
						_cashCount,
						_branchNo.Value,
						_drawerNo.Value,
						_postingDt.Value,
                        null,
                        _savedTlInvItems);  //#140772 - added new parameter value
					//
					tempWin.Closed +=new EventHandler(tempWin_Closed);
				}
			}
			if ( tempWin != null )
			{
				tempWin.Workspace = this.Workspace;
				tempWin.Show();
			}
		}

		private void LoadDenomTypes()
		{
			#region load cash denom config from admin
			_busTlCashCountSummary.BranchNo.Value = _branchNo.Value;
			_busTlCashCountSummary.DrawerNo.Value = _drawerNo.Value;
			_busTlCashCountSummary.ClosedDt.Value = _postingDt.Value;
			_busTlCashCountSummary.LoadDrawerCashPosSummary(_cashCountSummary);
			//
			//recalculate total fields
			_busTlCashCountSummary.LoadDrawerCashPosSummary(_cashCountSummary, true);
			/* Begin #71886 */
			MoveCountToSummary();
			/* End #71886 */

			#endregion
		}

		/* Begin #71886 */
		private void MoveCountToSummary()
		{
			foreach(TlCashPositionSummary cashSum in _cashCountSummary)
			{
				foreach(Phoenix.BusObj.Teller.TlCashCount count in _cashCount )
				{
					if (cashSum.DenomId.Value == count.DenomId.Value)
					{
						if (!count.Amt.IsNull)
						{
							cashSum.DrawerCount.Value = count.Amt.Value;
						}
						break;
					}
				}
			}

		}
		/* End #71886 */


		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "IncludeCashInCashOutAmts")
			{
				if (cbIncludeCashInCashOutAmtDenom.Checked)
				{
					this.colCashIn.Visible = true;
					this.colCashOut.Visible = true;
					this.colMadeAmt.Visible = true;	//#71893
					this.colBrokeAmt.Visible = true;//#71893
				}
				else
				{
					this.colCashIn.Visible = false;
					this.colCashOut.Visible = false;
					this.colMadeAmt.Visible = false;	//#71893
					this.colBrokeAmt.Visible = false;	//#71893
				}
			}
			else if (callerInfo == "FormComplete")
			{
				if (gridDrawerCashCountSummary.Count > 0)
				{
					this.gridDrawerCashCountSummary.Focus();
					this.gridDrawerCashCountSummary.SelectRow(0);
				}
				if (_isViewOnly)
				{
					this.pbJournal.Visible = false;
					this.cbIncludeCashInCashOutAmtDenom.Enabled = false;
				}
				else
				{
					this.pbJournal.Visible = true;
				}
				//
				if (gridDrawerCashCountSummary.Count <= 0)
				{
					if (this.pbJournal.Visible)
						this.pbJournal.Enabled = false;
					this.pbCashCount.Enabled = false;
					this.cbIncludeCashInCashOutAmtDenom.Enabled = false;
				}
			}
		}

		private void LoadCashSummary(bool loadGridFromList)
		{
			if (loadGridFromList)
			{
				//
				if (_cashCountSummary.Count == 0)
				{
					LoadDenomTypes();
				}

				foreach( TlCashPositionSummary cashSum in _cashCountSummary )
				{
					gridDrawerCashCountSummary.AddNewRow();
					//
					colDonomDesc.FieldToColumn(cashSum.CashDenomDesc);
					colBeginningCash.FieldToColumn(cashSum.BeginningCash);
					colCashIn.FieldToColumn(cashSum.CashIn);
					colCashOut.FieldToColumn(cashSum.CashOut);
					colNetCash.FieldToColumn(cashSum.NetCash);
					colCashDrawerCount.FieldToColumn(cashSum.DrawerCount);
					colEndingCash.FieldToColumn(cashSum.EndingCash);
					colDifference.FieldToColumn(cashSum.Difference);
					colBranchNo.FieldToColumn(cashSum.BranchNo);
					colDrawerNo.FieldToColumn(cashSum.DrawerNo);
					colDenom.FieldToColumn(cashSum.Denom);
					colDenomId.FieldToColumn(cashSum.DenomId);
					colDenomType.FieldToColumn(cashSum.DenomType);
					//#71893
					colMadeAmt.FieldToColumn(cashSum.Made);
					colBrokeAmt.FieldToColumn(cashSum.Broke);
					colNetMadeBrokeAmt.FieldToColumn(cashSum.NetMadeBroke);
				}
				//
				CalcTotal();
			}
			else
			{
				int nRow = 0;
				//
				while (nRow < gridDrawerCashCountSummary.Count)
				{
					gridDrawerCashCountSummary.SelectRow(nRow, false);
					//
					foreach(TlCashPositionSummary cashSum in _cashCountSummary)
					{
						if (colDenomType.Text == cashSum.DenomType.Value && 
							Convert.ToInt32(colDenomId.UnFormattedValue) == cashSum.DenomId.Value)
						{
							colCashDrawerCount.ColumnToField(cashSum.DrawerCount);
							colDifference.ColumnToField(cashSum.Difference); //71263
							break;
						}
					}
					//
					nRow = nRow + 1;
				}
			}
		}

		private void CalcTotal()
		{
			if (!_isViewOnly)
			{
				this.dfCashDrawerCount.UnFormattedValue = _busObjTlCashCount.TotalDenomAmt(_cashCount);
			}
			else
			{
				this.dfCashDrawerCount.UnFormattedValue = _drawerCountTotal;
				this.dfEndingCash.UnFormattedValue = _endingCash.Value;
			}
			this.dfDifference.UnFormattedValue = Convert.ToDecimal(dfCashDrawerCount.UnFormattedValue) -
						Convert.ToDecimal(dfEndingCash.UnFormattedValue);
			//
			SetFieldColor();
		}

		private void SetFieldColor()
		{
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) == 0)
				dfDifference.ForeColor = System.Drawing.Color.Black;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
				dfDifference.ForeColor = System.Drawing.Color.Blue;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
				dfDifference.ForeColor = System.Drawing.Color.Red;
		}

		#region CallXMThruCDS
		private void CallXMThruCDS(string origin)
		{
			if (origin == "TlPosition")
			{
				_tlPositionTemp.SelectAllFields = false;
				_tlPositionTemp.Ptid.Value = _positionPtid.Value;
				_tlPositionTemp.CreateDt.Selected = true;
				_tlPositionTemp.ClosedDt.Selected = true;				
				_tlPositionTemp.CashDrawer.Selected = true;
				_tlPositionTemp.JournalPtid.Selected = true;
				_tlPositionTemp.Ptid.Selected = true;
				_tlPositionTemp.EndingCash.Selected = true;
				_tlPositionTemp.DrawerNo.Selected = true;
				_tlPositionTemp.EmplId.Selected = true;
				_tlPositionTemp.BranchNo.Selected = true;
				_tlPositionTemp.BalDenomTracking.Selected = true;
				_tlPositionTemp.Description.Selected = true;
				_tlPositionTemp.ActionType = XmActionType.Select;
				DataService.Instance.ProcessRequest(_tlPositionTemp);
			}
			else if (origin == "AdGbBranch")
			{
				_adGbBranchBusObj = new AdGbBranch();
				_adGbBranchBusObj.ActionType = XmActionType.Select;
				_adGbBranchBusObj.SelectAllFields = false;
				_adGbBranchBusObj.Name1.Selected = true;
				_adGbBranchBusObj.BranchNo.Value = _tlPositionTemp.BranchNo.Value;
				DataService.Instance.ProcessRequest(_adGbBranchBusObj);
			}
			else if (origin == "AdGbRsm")
			{
				_adGbRsmTemp = new AdGbRsm();
				_adGbRsmTemp.SelectAllFields = false;
				_adGbRsmTemp.EmployeeName.Selected = true;		
				_adGbRsmTemp.TellerNo.Selected = true;
				_adGbRsmTemp.EmployeeId.Value = _tlPositionTemp.EmplId.Value; //_tlPosition.EmplId.Value;				
				_adGbRsmTemp.ActionType = XmActionType.Select;
				DataService.Instance.ProcessRequest(_adGbRsmTemp);
			}
		}
		#endregion CallXMThruCDS

		#endregion

		private void gridDrawerCashCountSummary_FetchRowDone(object sender, GridRowArgs e)
		{
			if (_isViewOnly)
				_drawerCountTotal = _drawerCountTotal + Convert.ToDecimal(colCashDrawerCount.UnFormattedValue);
		}

		private void gridDrawerCashCountSummary_AfterPopulate(object sender, GridPopulateArgs e)
		{
			if (_isViewOnly)
				CalcTotal();
		}

		#region frmTlCashDrawerSummary_PMdiPrintEvent


		private void frmTlCashDrawerSummary_PMdiPrintEvent(object sender, EventArgs e)
		{
			//71440 - These window has heavy trafic and creating lot of trouble so I have moved the code back here...
			if (TellerVars.Instance.IsAppOnline)
			{
				_pdfFileManipulation = new PdfFileManipulation();
			}
			else
			{
				try
				{
					_htmlPrinter = new HtmlPrinter();
					//Hack - Not much we can do
					for (int i = 0; i < 1000; i++ )
					{
						if (CoreService.LogPublisher.IsLogEnabled)
						{
							if (_htmlPrinter == null)
								CoreService.LogPublisher.LogDebug("frmTlPosition_PMdiPrintEvent - Still object is getting created");
							else
								CoreService.LogPublisher.LogDebug("frmTlPosition_PMdiPrintEvent Browser object created");
						}
						if (_htmlPrinter != null)
							break;
					}
				}
				catch(System.Runtime.InteropServices.COMException ex)
				{
					CoreService.LogPublisher.LogDebug("\n(frmtlPosotion window)For some reason creating of HtmlPrinter Failed." + ex.Message);
				}	
			}
			if (_positionPtid.IsNull || _positionPtid.Value == 0 )
			{
				//Not Sure What to Do
			}
			else			
				PrintDrawerCashCount(false, true);	
		}
		//
		#region PrintDrawercashCount
		private void PrintDrawerCashCount(bool displayBrowser, bool riseMessage)
		{	
			if (!TellerVars.Instance.IsAppOnline)
				_printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);
			if (!_positionPtid.IsNull && _positionPtid.Value > 0)
				CallXMThruCDS("TlPosition");
			else
				return;

			if (!_tlPositionTemp.Ptid.IsNull )
			{
				
				try
				{
					//360724 - Generating Teller Cash Drawer Count [Drawer %1!]report.  Please wait...
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetTokenizeMessageX(360724, _tlPositionTemp.DrawerNo.StringValue));
					//
					Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();

					string cashDrawer = string.Empty;
					string endingCash = string.Empty;
					string difference = string.Empty;
					string tellerNoName = string.Empty;
					string branchNoName = string.Empty;
					string[] sqrParams = new string[7];
					Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
					myTempVar.Visible = false;
					myTempVar.TabStop = false;
					myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
					myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;

					myTempVar.UnFormattedValue = _tlPositionTemp.CashDrawer.Value;
					cashDrawer = myTempVar.Text;
					//
					myTempVar.UnFormattedValue = _tlPositionTemp.EndingCash.Value;
					endingCash = myTempVar.Text;
					//
					myTempVar.UnFormattedValue = (_tlPositionTemp.CashDrawer.Value - _tlPositionTemp.EndingCash.Value);
					difference = myTempVar.Text;


					CallXMThruCDS("AdGbBranch");
					branchNoName = (_adGbBranchBusObj.BranchNo.Value  + " - " + _adGbBranchBusObj.Name1.Value.Trim());
					CallXMThruCDS("AdGbRsm");
					tellerNoName = _adGbRsmTemp.TellerNo.StringValue + " - " + (_adGbRsmTemp.EmployeeName.IsNull?"unknown":_adGbRsmTemp.EmployeeName.Value.Trim());
					if (TellerVars.Instance.IsAppOnline) //SQR
					{
						sqrParams = _tlDrawerBalances.GetDrawerCashCountReportParams(
							_tlPositionTemp.Ptid.IntValue, _tlPositionTemp.JournalPtid.Value, 
							(_tlPositionTemp.BalDenomTracking.IsNull?"N":_tlPositionTemp.BalDenomTracking.Value), 
							_tlPositionTemp.BranchNo.Value, 
							_tlPositionTemp.DrawerNo.Value, 
							Convert.ToDateTime(_tlPositionTemp.ClosedDt.Value), 
							Convert.ToDateTime(_tlPositionTemp.CreateDt.Value),
							branchNoName,
							tellerNoName, 
							_tlPositionTemp.ClosedDt.Value.ToString("MM/dd/yyyy"), 
							_tlPositionTemp.CreateDt.Value.ToString("MM/dd/yyyy hh:mm:ss tt"),
							cashDrawer, endingCash, difference);

						report1.ReportName.Value = "TLO11500.sqr";
						report1.EmplId.Value = GlobalVars.EmployeeId;
						report1.FromDt.Value = GlobalVars.SystemDate;
						report1.ToDt.Value = GlobalVars.SystemDate;
						report1.RunDate.Value = DateTime.Now;
						report1.Param1.Value = sqrParams[0];
						report1.Param2.Value = sqrParams[1];
						report1.Param3.Value = sqrParams[2];
						report1.Param4.Value = sqrParams[3];
						report1.Param5.Value = sqrParams[4];
						report1.Param6.Value = sqrParams[5];
						report1.MiscParams.Value = sqrParams[6];


						if (CoreService.LogPublisher.IsLogEnabled)
						{
							CoreService.LogPublisher.LogDebug("SQR Param1=(" + report1.Param1.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param2=(" + report1.Param2.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param3=(" + report1.Param3.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param4=(" + report1.Param4.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param5=(" + report1.Param5.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR Param6=(" + report1.Param6.StringValue + ")");
							CoreService.LogPublisher.LogDebug("SQR MiscNo=(" + report1.MiscParams.StringValue + ")");
						}
						DataService.Instance.ProcessRequest(XmActionType.Select, report1);

						if (!riseMessage)
							//_printProcess = _htmlPrinter.PrintPDFFromUrl(report1.OutputLink.Value, false);
							_printProcess = _pdfFileManipulation.PrintPDFFromUrl(report1.OutputLink.Value, false);
						else					
							//_htmlPrinter.ShowUrlPdf(report1.OutputLink.Value);
							_pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
					}
					else
					{
						sqrParams = _tlDrawerBalances.GetOfflineTLF115Report(
							_tlPositionTemp.Ptid.IntValue, _tlPositionTemp.JournalPtid.Value, 
							(_tlPositionTemp.BalDenomTracking.IsNull?"N":_tlPositionTemp.BalDenomTracking.Value), 
							_tlPositionTemp.BranchNo.Value, 
							_tlPositionTemp.DrawerNo.Value, 
							Convert.ToDateTime(_tlPositionTemp.ClosedDt.Value), 
							Convert.ToDateTime(_tlPositionTemp.CreateDt.Value),
							branchNoName,
							tellerNoName, 
							_tlPositionTemp.ClosedDt.Value.ToString("MM/dd/yyyy"), 
							_tlPositionTemp.CreateDt.Value.ToString("MM/dd/yyyy hh:mm:ss tt"),
							cashDrawer, endingCash, difference);

						if (!riseMessage)
						{
							//							if (sqrParams[1].Trim().Length == 0)
							//								_htmlPrinter.PrintHtml(sqrParams[0], displayBrowser);
							//							else															
							//								_htmlPrinter.PrintHtml(sqrParams, displayBrowser);	
							_htmlPrinter.PrintHtml(sqrParams[0] + "\n" +sqrParams[1], displayBrowser);
						}
						else		
						{
							//							if (sqrParams[1].Trim().Length == 0)
							//								_htmlPrinter.PrintHtml(sqrParams[0], displayBrowser);
							//							else															
							//								_htmlPrinter.PrintHtml(sqrParams, displayBrowser);
							_htmlPrinter.PrintHtml(sqrParams[0] + "\n" +sqrParams[1], displayBrowser);
						}
					}
				}
				catch(PhoenixException pe)
				{
					dlgInformation.Instance.HideInfo();
					Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
					if (riseMessage)
						PMessageBox.Show(360725, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					//360725 - Failed to generate Teller Cash Drawer Count report.
				}
				catch(Exception ex)
				{
					dlgInformation.Instance.HideInfo();
					if (riseMessage)
						PMessageBox.Show(360726, MessageType.Warning, MessageBoxButtons.OK, ex.Message);
					//360726 - Failed to generate Teller Cash Drawer Count report.
				}
				finally
				{
					if (!riseMessage && _printProcess != null)
					{
						try
						{
							_printProcess.Kill();
						}
						finally
						{
							_printProcess.Dispose();
						}
					}
					dlgInformation.Instance.HideInfo();
                    //73125
                    //if (!TellerVars.Instance.IsAppOnline  && _printerSettings != null)
                    //    _printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
				}
			}
		}
		#endregion 
		//
		#endregion frmTlCashDrawerSummary_PMdiPrintEvent
	}
}
