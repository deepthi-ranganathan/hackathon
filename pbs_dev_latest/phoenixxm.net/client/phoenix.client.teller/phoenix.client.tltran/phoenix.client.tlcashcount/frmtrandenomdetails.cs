#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name:frmtrandenomdetails.cs
// NameSpace:Phoenix.Client.TlCashCount
//-------------------------------------------------------------------------------
///Date        	Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/05/2006	2		mselvaga	#70917 - Denomination details total fields fix for cashIn/cashOut.
//01/23/2007	3		Vreddy		#71440 - 67879 - .NET Teller - Application kicks user out after receiving an application error and KTADMIN or SQLExpress or Outlook, or GEMS is opened
//02/01/2007	4		Vreddy		#71649 - Alignment of decimal columns
//04/03/2007	5		mselvaga	#71893 - Added strap split changes.
//04/10/2007	6		Vreddy		#72361 - Teller 2007 - AD_GB_RSM.employee_id is printing on the Teller Summary Position and Cash Summary Reports, it should be AD_GB_RSM.teller_no
//06/18/2007    7       Vreddy      #73125 - Printer layout setting is rearranged
//08/24/2007    8       BBedi       #72916 - Add TCD Support in Teller 2007
//04/08/2008    9       mselvaga    #75737 - QA Release 2008 TCD- 12093 - Transaction Denomination Details window, frmTranDenomDetails, is not summing the amounts in the Totals group box.
//02/11/2010    10      LSimpson    #79574 - UI changes for Cash Recycler.
//05/28/2010    11      LSimpson    #9161 - Added rollover amount.
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
using System.Text;
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
//Report
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Misc;
using Phoenix.Shared.Windows;
//

namespace Phoenix.Client.TlCashCount
{
	/// <summary>
	/// Summary description for frmTranDenomDetails.
	/// </summary>
	public class frmTranDenomDetails : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGrid gridDenomDetails;
		private Phoenix.Windows.Forms.PGridColumn colComponent;
		private Phoenix.Windows.Forms.PGridColumn colType;
		private Phoenix.Windows.Forms.PGridColumn colDenom;
		private Phoenix.Windows.Forms.PGridColumn colQty;
		private Phoenix.Windows.Forms.PGridColumn colStrapValue;
		private Phoenix.Windows.Forms.PGridColumn colAmt;
		private Phoenix.Windows.Forms.PGridColumn colCasseteID;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblCashInAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfCashInAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblCashOutAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfCashOutAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDCashInAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfTCDCashInAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblTCDCashOutAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfTCDCashOutAmt;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
		private Phoenix.Windows.Forms.PDfDisplay dfCashOutDisplay;
		private Phoenix.Windows.Forms.PGridColumn colCuid;
		private Phoenix.Windows.Forms.PGridColumn colTranType;
		private Phoenix.Windows.Forms.PGridColumn colPhysicalPos;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard2;
		private Phoenix.Windows.Forms.PPanel pPanel3;
		private Phoenix.Windows.Forms.PPanel pPanel2;
		private Phoenix.Windows.Forms.PPanel pPanel1;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard2;
		private Phoenix.Windows.Forms.PLabelStandard pLabelStandard1;
		private Phoenix.Windows.Forms.PDfDisplay dfDifference;
		private Phoenix.Windows.Forms.PLabelStandard lblDifference;
		private Phoenix.Windows.Forms.PDfDisplay dfEndingCash;
		private Phoenix.Windows.Forms.PLabelStandard lblEndingCash;
		private Phoenix.Windows.Forms.PDfDisplay dfCashDrawerCount;
		private Phoenix.Windows.Forms.PLabelStandard lblCashDrawerCount;
		//
		#region Initialize
		//
		private PDecimal _journalPtid = new PDecimal("JournalPtid");
		private PDecimal _cashIn = new PDecimal("CashIn");
		private PDecimal _cashOut = new PDecimal("CashOut");
		private PDecimal _tcdCashIn = new PDecimal("TcdCashIn");
		private PDecimal _tcdCashOut = new PDecimal("TcdCashOut");
		private PDecimal _netAmt = new PDecimal("NetAmt");
		private PSmallInt _batchId = new PSmallInt("BatchId");
		private PSmallInt _subSequence = new PSmallInt("SubSequence");
		private PInt _callerScreenId = new PInt("CallerScreenId");
		private PDecimal _endingCash = new PDecimal("EndingCash");
        private PString _tlTranCode = new PString("TlTranCode"); // #9161
		//
		private Phoenix.BusObj.Teller.TlCashCount _busTlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private TellerVars _tellerVars = TellerVars.Instance;
		private decimal _totalCashDrawerCount = 0;
		private decimal _totalCashIn = 0;
		private decimal _totalCashOut = 0;
        private decimal _totalRolloverAmt = 0; // #9161
		private decimal _totalMadeAmt = 0;
		private decimal _totalBrokeAmt = 0;
		//Report Specific
		private AdGbRsm _adGbRsmTemp = new AdGbRsm();
		private AdGbBranch _adGbBranchBusObj;
		private Phoenix.BusObj.Teller.TlPosition _tlPositionTemp = new Phoenix.BusObj.Teller.TlPosition();
		private TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
		//Report Specific
		private string _mlTranTypeCashIn = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I");
		private string _mlTranTypeCashOut = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O");
		private string _mlTranTypeCloseOut = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "C");
		private string _mlTranTypeMade = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "M");
		private string _mlTranTypeBroke = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "B");
        private string _mlTranTypeRollover = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "R"); // #9161

		Phoenix.Shared.Windows.HtmlPrinter _htmlPrinter; // = new HtmlPrinter();
		Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;
		Phoenix.Shared.Windows.PSetting _printerSettings = new PSetting();
		System.Diagnostics.Process _printProcess = null;
		private bool _isMadeBrokeTran = false; //#71893

		#endregion

		public frmTranDenomDetails()
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
            this.gridDenomDetails = new Phoenix.Windows.Forms.PGrid();
            this.colCuid = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranType = new Phoenix.Windows.Forms.PGridColumn();
            this.colPhysicalPos = new Phoenix.Windows.Forms.PGridColumn();
            this.colComponent = new Phoenix.Windows.Forms.PGridColumn();
            this.colType = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colDenom = new Phoenix.Windows.Forms.PGridColumn();
            this.colQty = new Phoenix.Windows.Forms.PGridColumn();
            this.colStrapValue = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCasseteID = new Phoenix.Windows.Forms.PGridColumn();
            this.gbTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfCashOutDisplay = new Phoenix.Windows.Forms.PDfDisplay();
            this.dfTCDCashOutAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCDCashOutAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDCashInAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblTCDCashInAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashOutAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashOutAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCashInAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblCashInAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.pGroupBoxStandard2 = new Phoenix.Windows.Forms.PGroupBoxStandard();
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
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbTotals.SuspendLayout();
            this.pGroupBoxStandard2.SuspendLayout();
            this.pGroupBoxStandard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridDenomDetails
            // 
            this.gridDenomDetails.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colCuid,
            this.colTranType,
            this.colPhysicalPos,
            this.colComponent,
            this.colType,
            this.colDescription,
            this.colDenom,
            this.colQty,
            this.colStrapValue,
            this.colAmt,
            this.colCasseteID});
            this.gridDenomDetails.LinesInHeader = 2;
            this.gridDenomDetails.Location = new System.Drawing.Point(4, 8);
            this.gridDenomDetails.Name = "gridDenomDetails";
            this.gridDenomDetails.Size = new System.Drawing.Size(676, 356);
            this.gridDenomDetails.TabIndex = 0;
            this.gridDenomDetails.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridDenomDetails_AfterPopulate);
            this.gridDenomDetails.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridDenomDetails_FetchRowDone);
            this.gridDenomDetails.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridDenomDetails_BeforePopulate);
            // 
            // colCuid
            // 
            this.colCuid.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCuid.PhoenixUIControl.XmlTag = "Cuid";
            this.colCuid.Title = "Cuid";
            this.colCuid.Visible = false;
            this.colCuid.Width = 0;
            // 
            // colTranType
            // 
            this.colTranType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranType.PhoenixUIControl.XmlTag = "TranType";
            this.colTranType.Title = "Tran Type";
            this.colTranType.Visible = false;
            this.colTranType.Width = 0;
            // 
            // colPhysicalPos
            // 
            this.colPhysicalPos.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPhysicalPos.PhoenixUIControl.XmlTag = "PhysicalPosition";
            this.colPhysicalPos.Title = "Column";
            this.colPhysicalPos.Visible = false;
            this.colPhysicalPos.Width = 0;
            // 
            // colComponent
            // 
            this.colComponent.PhoenixUIControl.ObjectId = 1;
            this.colComponent.PhoenixUIControl.XmlTag = "";
            this.colComponent.Title = "Component";
            this.colComponent.Width = 100; // #79574 - changed from 85 to 100
            // 
            // colType
            // 
            this.colType.PhoenixUIControl.ObjectId = 2;
            this.colType.PhoenixUIControl.XmlTag = "DenomType";
            this.colType.Title = "Type";
            this.colType.Width = 81;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 18;
            this.colDescription.PhoenixUIControl.XmlTag = "DenomDesc";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 76; // #79574 - changed from 91 to 76
            // 
            // colDenom
            // 
            this.colDenom.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDenom.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDenom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDenom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDenom.PhoenixUIControl.ObjectId = 3;
            this.colDenom.PhoenixUIControl.XmlTag = "Denom";
            this.colDenom.Title = "Denomination";
            this.colDenom.Width = 77;
            // 
            // colQty
            // 
            this.colQty.PhoenixUIControl.ObjectId = 4;
            this.colQty.PhoenixUIControl.XmlTag = "Quantity";
            this.colQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colQty.Title = "Quantity";
            this.colQty.Width = 48;
            // 
            // colStrapValue
            // 
            this.colStrapValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStrapValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStrapValue.PhoenixUIControl.ObjectId = 5;
            this.colStrapValue.PhoenixUIControl.XmlTag = "CountValue";
            this.colStrapValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colStrapValue.Title = "Strap Or Roll Value";
            this.colStrapValue.Width = 88;
            // 
            // colAmt
            // 
            this.colAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.ObjectId = 6;
            this.colAmt.PhoenixUIControl.XmlTag = "Amt";
            this.colAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colAmt.Title = "Amount";
            this.colAmt.Width = 127;
            // 
            // colCasseteID
            // 
            this.colCasseteID.PhoenixUIControl.ObjectId = 7;
            this.colCasseteID.PhoenixUIControl.XmlTag = "x";
            this.colCasseteID.Title = "Cassette ID/ Position";
            this.colCasseteID.Visible = false;
            this.colCasseteID.Width = 75;
            // 
            // gbTotals
            // 
            this.gbTotals.Controls.Add(this.dfCashOutDisplay);
            this.gbTotals.Controls.Add(this.dfTCDCashOutAmt);
            this.gbTotals.Controls.Add(this.lblTCDCashOutAmount);
            this.gbTotals.Controls.Add(this.dfTCDCashInAmt);
            this.gbTotals.Controls.Add(this.lblTCDCashInAmount);
            this.gbTotals.Controls.Add(this.dfCashOutAmt);
            this.gbTotals.Controls.Add(this.lblCashOutAmount);
            this.gbTotals.Controls.Add(this.dfCashInAmt);
            this.gbTotals.Controls.Add(this.lblCashInAmount);
            this.gbTotals.Location = new System.Drawing.Point(4, 368);
            this.gbTotals.Name = "gbTotals";
            this.gbTotals.PhoenixUIControl.ObjectId = 8;
            this.gbTotals.Size = new System.Drawing.Size(684, 76);
            this.gbTotals.TabIndex = 1;
            this.gbTotals.TabStop = false;
            this.gbTotals.Text = "Totals";
            // 
            // dfCashOutDisplay
            // 
            this.dfCashOutDisplay.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCashOutDisplay.Location = new System.Drawing.Point(4, 56);
            this.dfCashOutDisplay.Multiline = true;
            this.dfCashOutDisplay.Name = "dfCashOutDisplay";
            this.dfCashOutDisplay.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCashOutDisplay.PhoenixUIControl.ObjectId = 15;
            this.dfCashOutDisplay.Size = new System.Drawing.Size(463, 16);
            this.dfCashOutDisplay.TabIndex = 8;
            // 
            // dfTCDCashOutAmt
            // 
            this.dfTCDCashOutAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashOutAmt.Location = new System.Drawing.Point(564, 36);
            this.dfTCDCashOutAmt.Multiline = true;
            this.dfTCDCashOutAmt.Name = "dfTCDCashOutAmt";
            this.dfTCDCashOutAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashOutAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashOutAmt.PhoenixUIControl.ObjectId = 12;
            this.dfTCDCashOutAmt.PhoenixUIControl.XmlTag = "TcdCashOut";
            this.dfTCDCashOutAmt.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashOutAmt.TabIndex = 7;
            // 
            // lblTCDCashOutAmount
            // 
            this.lblTCDCashOutAmount.AutoEllipsis = true;
            this.lblTCDCashOutAmount.Location = new System.Drawing.Point(416, 36);
            this.lblTCDCashOutAmount.Name = "lblTCDCashOutAmount";
            this.lblTCDCashOutAmount.PhoenixUIControl.ObjectId = 12;
            this.lblTCDCashOutAmount.Size = new System.Drawing.Size(150, 16);
            this.lblTCDCashOutAmount.TabIndex = 6;
            this.lblTCDCashOutAmount.Text = "TCD/TCR Cash Out Amount:";
            // 
            // dfTCDCashInAmt
            // 
            this.dfTCDCashInAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashInAmt.Location = new System.Drawing.Point(564, 16);
            this.dfTCDCashInAmt.Multiline = true;
            this.dfTCDCashInAmt.Name = "dfTCDCashInAmt";
            this.dfTCDCashInAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTCDCashInAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTCDCashInAmt.PhoenixUIControl.ObjectId = 11;
            this.dfTCDCashInAmt.PhoenixUIControl.XmlTag = "TcdCashIn";
            this.dfTCDCashInAmt.Size = new System.Drawing.Size(112, 16);
            this.dfTCDCashInAmt.TabIndex = 3;
            // 
            // lblTCDCashInAmount
            // 
            this.lblTCDCashInAmount.AutoEllipsis = true;
            this.lblTCDCashInAmount.Location = new System.Drawing.Point(416, 16);
            this.lblTCDCashInAmount.Name = "lblTCDCashInAmount";
            this.lblTCDCashInAmount.PhoenixUIControl.ObjectId = 11;
            this.lblTCDCashInAmount.Size = new System.Drawing.Size(144, 16);
            this.lblTCDCashInAmount.TabIndex = 2;
            this.lblTCDCashInAmount.Text = "TCD/TCR Cash In Amount:";
            // 
            // dfCashOutAmt
            // 
            this.dfCashOutAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOutAmt.Location = new System.Drawing.Point(120, 36);
            this.dfCashOutAmt.Multiline = true;
            this.dfCashOutAmt.Name = "dfCashOutAmt";
            this.dfCashOutAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashOutAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashOutAmt.PhoenixUIControl.ObjectId = 10;
            this.dfCashOutAmt.PhoenixUIControl.XmlTag = "CashOut";
            this.dfCashOutAmt.Size = new System.Drawing.Size(112, 16);
            this.dfCashOutAmt.TabIndex = 5;
            // 
            // lblCashOutAmount
            // 
            this.lblCashOutAmount.AutoEllipsis = true;
            this.lblCashOutAmount.Location = new System.Drawing.Point(4, 36);
            this.lblCashOutAmount.Name = "lblCashOutAmount";
            this.lblCashOutAmount.PhoenixUIControl.ObjectId = 10;
            this.lblCashOutAmount.Size = new System.Drawing.Size(112, 16);
            this.lblCashOutAmount.TabIndex = 4;
            this.lblCashOutAmount.Text = "Cash Out Amount:";
            // 
            // dfCashInAmt
            // 
            this.dfCashInAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashInAmt.Location = new System.Drawing.Point(120, 16);
            this.dfCashInAmt.Multiline = true;
            this.dfCashInAmt.Name = "dfCashInAmt";
            this.dfCashInAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashInAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashInAmt.PhoenixUIControl.ObjectId = 9;
            this.dfCashInAmt.PhoenixUIControl.XmlTag = "CashIn";
            this.dfCashInAmt.Size = new System.Drawing.Size(112, 16);
            this.dfCashInAmt.TabIndex = 1;
            // 
            // lblCashInAmount
            // 
            this.lblCashInAmount.AutoEllipsis = true;
            this.lblCashInAmount.Location = new System.Drawing.Point(4, 16);
            this.lblCashInAmount.Name = "lblCashInAmount";
            this.lblCashInAmount.PhoenixUIControl.ObjectId = 9;
            this.lblCashInAmount.Size = new System.Drawing.Size(112, 16);
            this.lblCashInAmount.TabIndex = 0;
            this.lblCashInAmount.Text = "Cash In Amount:";
            // 
            // pGroupBoxStandard2
            // 
            this.pGroupBoxStandard2.Controls.Add(this.pPanel3);
            this.pGroupBoxStandard2.Controls.Add(this.pPanel2);
            this.pGroupBoxStandard2.Controls.Add(this.pPanel1);
            this.pGroupBoxStandard2.Controls.Add(this.pLabelStandard2);
            this.pGroupBoxStandard2.Controls.Add(this.pLabelStandard1);
            this.pGroupBoxStandard2.Controls.Add(this.dfDifference);
            this.pGroupBoxStandard2.Controls.Add(this.lblDifference);
            this.pGroupBoxStandard2.Controls.Add(this.dfEndingCash);
            this.pGroupBoxStandard2.Controls.Add(this.lblEndingCash);
            this.pGroupBoxStandard2.Controls.Add(this.dfCashDrawerCount);
            this.pGroupBoxStandard2.Controls.Add(this.lblCashDrawerCount);
            this.pGroupBoxStandard2.Location = new System.Drawing.Point(4, 368);
            this.pGroupBoxStandard2.Name = "pGroupBoxStandard2";
            this.pGroupBoxStandard2.PhoenixUIControl.ObjectId = 24;
            this.pGroupBoxStandard2.Size = new System.Drawing.Size(684, 76);
            this.pGroupBoxStandard2.TabIndex = 9;
            this.pGroupBoxStandard2.TabStop = false;
            this.pGroupBoxStandard2.Text = "Teller Balancing Cash Position";
            // 
            // pPanel3
            // 
            this.pPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel3.Location = new System.Drawing.Point(472, 40);
            this.pPanel3.Name = "pPanel3";
            this.pPanel3.Size = new System.Drawing.Size(204, 1);
            this.pPanel3.TabIndex = 10;
            this.pPanel3.TabStop = true;
            // 
            // pPanel2
            // 
            this.pPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel2.Location = new System.Drawing.Point(236, 40);
            this.pPanel2.Name = "pPanel2";
            this.pPanel2.Size = new System.Drawing.Size(204, 1);
            this.pPanel2.TabIndex = 9;
            this.pPanel2.TabStop = true;
            // 
            // pPanel1
            // 
            this.pPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPanel1.Location = new System.Drawing.Point(4, 40);
            this.pPanel1.Name = "pPanel1";
            this.pPanel1.Size = new System.Drawing.Size(204, 1);
            this.pPanel1.TabIndex = 8;
            this.pPanel1.TabStop = true;
            // 
            // pLabelStandard2
            // 
            this.pLabelStandard2.AutoEllipsis = true;
            this.pLabelStandard2.Location = new System.Drawing.Point(456, 16);
            this.pLabelStandard2.Name = "pLabelStandard2";
            this.pLabelStandard2.PhoenixUIControl.ObjectId = 23;
            this.pLabelStandard2.Size = new System.Drawing.Size(8, 20);
            this.pLabelStandard2.TabIndex = 7;
            this.pLabelStandard2.Text = "=";
            // 
            // pLabelStandard1
            // 
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(220, 16);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 22;
            this.pLabelStandard1.Size = new System.Drawing.Size(8, 20);
            this.pLabelStandard1.TabIndex = 6;
            this.pLabelStandard1.Text = "-";
            // 
            // dfDifference
            // 
            this.dfDifference.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfDifference.Location = new System.Drawing.Point(476, 16);
            this.dfDifference.Multiline = true;
            this.dfDifference.Name = "dfDifference";
            this.dfDifference.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfDifference.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfDifference.PhoenixUIControl.ObjectId = 21;
            this.dfDifference.Size = new System.Drawing.Size(200, 20);
            this.dfDifference.TabIndex = 3;
            // 
            // lblDifference
            // 
            this.lblDifference.AutoEllipsis = true;
            this.lblDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifference.Location = new System.Drawing.Point(548, 48);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.PhoenixUIControl.ObjectId = 21;
            this.lblDifference.Size = new System.Drawing.Size(96, 20);
            this.lblDifference.TabIndex = 2;
            this.lblDifference.Text = "Difference";
            // 
            // dfEndingCash
            // 
            this.dfEndingCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfEndingCash.Location = new System.Drawing.Point(240, 16);
            this.dfEndingCash.Multiline = true;
            this.dfEndingCash.Name = "dfEndingCash";
            this.dfEndingCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfEndingCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfEndingCash.PhoenixUIControl.ObjectId = 20;
            this.dfEndingCash.Size = new System.Drawing.Size(204, 20);
            this.dfEndingCash.TabIndex = 5;
            // 
            // lblEndingCash
            // 
            this.lblEndingCash.AutoEllipsis = true;
            this.lblEndingCash.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndingCash.Location = new System.Drawing.Point(308, 48);
            this.lblEndingCash.Name = "lblEndingCash";
            this.lblEndingCash.PhoenixUIControl.ObjectId = 20;
            this.lblEndingCash.Size = new System.Drawing.Size(100, 20);
            this.lblEndingCash.TabIndex = 4;
            this.lblEndingCash.Text = "Ending Cash";
            // 
            // dfCashDrawerCount
            // 
            this.dfCashDrawerCount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dfCashDrawerCount.Location = new System.Drawing.Point(4, 16);
            this.dfCashDrawerCount.Multiline = true;
            this.dfCashDrawerCount.Name = "dfCashDrawerCount";
            this.dfCashDrawerCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfCashDrawerCount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfCashDrawerCount.PhoenixUIControl.ObjectId = 19;
            this.dfCashDrawerCount.Size = new System.Drawing.Size(204, 20);
            this.dfCashDrawerCount.TabIndex = 1;
            // 
            // lblCashDrawerCount
            // 
            this.lblCashDrawerCount.AutoEllipsis = true;
            this.lblCashDrawerCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCashDrawerCount.Location = new System.Drawing.Point(52, 48);
            this.lblCashDrawerCount.Name = "lblCashDrawerCount";
            this.lblCashDrawerCount.PhoenixUIControl.ObjectId = 19;
            this.lblCashDrawerCount.Size = new System.Drawing.Size(128, 20);
            this.lblCashDrawerCount.TabIndex = 0;
            this.lblCashDrawerCount.Text = "Cash Drawer Count";
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.gridDenomDetails);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 0);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(684, 368);
            this.pGroupBoxStandard1.TabIndex = 0;
            this.pGroupBoxStandard1.TabStop = false;
            // 
            // frmTranDenomDetails
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.Controls.Add(this.pGroupBoxStandard2);
            this.Controls.Add(this.gbTotals);
            this.Name = "frmTranDenomDetails";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTranDenomDetails_PInitCompleteEvent);
            this.PMdiPrintEvent += new System.EventHandler(this.frmTranDenomDetails_PMdiPrintEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTranDenomDetails_PInitBeginEvent);
            this.gbTotals.ResumeLayout(false);
            this.gbTotals.PerformLayout();
            this.pGroupBoxStandard2.ResumeLayout(false);
            this.pGroupBoxStandard2.PerformLayout();
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 9)
			{
				_journalPtid.Value = Convert.ToDecimal(paramList[0]);
				_cashIn.Value = ((paramList[1] == null || Convert.ToDecimal(paramList[1]) <= 0)? Convert.ToDecimal(0):Convert.ToDecimal(paramList[1]));
				_cashOut.Value = ((paramList[2] == null || Convert.ToDecimal(paramList[2]) <= 0)? Convert.ToDecimal(0):Convert.ToDecimal(paramList[2]));
				_tcdCashIn.Value = ((paramList[3] == null || Convert.ToDecimal(paramList[3]) <= 0)? Convert.ToDecimal(0):Convert.ToDecimal(paramList[3]));
				_tcdCashOut.Value = ((paramList[4] == null || Convert.ToDecimal(paramList[4]) <= 0)? Convert.ToDecimal(0):Convert.ToDecimal(paramList[4]));
				_netAmt.Value = ((paramList[5] == null || Convert.ToDecimal(paramList[5]) <= 0)? Convert.ToDecimal(0):Convert.ToDecimal(paramList[5]));
				_batchId.Value = Convert.ToInt16(paramList[6]);
				if (_batchId.IsNull || _batchId.Value < 0)
					_batchId.ValueObject = null;
				_subSequence.Value = (paramList[7] == null? Convert.ToInt16(0):Convert.ToInt16(paramList[7]));
				_callerScreenId.Value = Convert.ToInt32(paramList[8]);
				//
				if (paramList.Length > 9)
				{
					_endingCash.Value = Convert.ToDecimal(paramList[9]);
                    // Begin #9161
                    if (_endingCash.IsNull)
                        _endingCash.ValueObject = null;
                    if (paramList.Length > 10)
                    {
                        _tlTranCode.Value = Convert.ToString(paramList[10]);
                        if (_tlTranCode.IsNull)
                            _tlTranCode.ValueObject = null;
                    }
                    // End #9161
				}
				//
				_busTlCashCount.JournalPtid.Value = Convert.ToInt32(_journalPtid.Value);
				_busTlCashCount.OutputTypeId.Value = 1;
				gridDenomDetails.SkipScreenToObject = true;
				if (((!_cashIn.IsNull && _cashIn.Value > 0) ||
					(!_cashOut.IsNull && _cashOut.Value > 0) ||
					(!_tcdCashIn.IsNull &&_tcdCashIn.Value > 0) || 
					(!_tcdCashOut.IsNull && _tcdCashOut.Value > 0) || 
					(!_netAmt.IsNull && _netAmt.Value > 0)) ||
                    (!_tlTranCode.IsNull && _tlTranCode.Value == "RLO") || // #9161
					_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals ||
					_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition) //#71893
					_isMadeBrokeTran = false;
				else
					_isMadeBrokeTran = true;
			}

			if (_subSequence.Value > 0)
			{
				lblTCDCashOutAmount.PhoenixUIControl.ObjectId = 17;
				lblCashOutAmount.PhoenixUIControl.ObjectId = 16;
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion
			//
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.TranDenomDetails;
			this.CurrencyId = _tellerVars.LocalCrncyId;
			//	
			base.InitParameters (paramList);
		}

		#endregion

		#region denom details events
		private ReturnType frmTranDenomDetails_PInitBeginEvent()
		{
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.MainBusinesObject = _busTlCashCount;
			return ReturnType.Success;
		}

		private void frmTranDenomDetails_PInitCompleteEvent()
		{
			//
			if (_subSequence.Value > 0)
			{
				//*Cash Out denominations are detailed on the last transaction component.
				dfCashOutDisplay.Text = CoreService.Translation.GetUserMessageX(360508);
			}

            //#75737 - moved the set denom totals logic to InitCompleteEvent from AfterPopulate
            #region handle close out details
            if (_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals ||
                _callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition)
            {
                this.dfEndingCash.UnFormattedValue = _endingCash.Value;
                this.dfCashDrawerCount.UnFormattedValue = _totalCashDrawerCount;
                this.dfDifference.UnFormattedValue = _totalCashDrawerCount -
                    (_endingCash.IsNull ? 0 : _endingCash.Value);
            }
            else
            {
                this.dfCashInAmt.UnFormattedValue = _totalCashIn; //_cashIn.Value;
                this.dfCashOutAmt.UnFormattedValue = _totalCashOut; //_cashOut.Value;
                this.dfTCDCashInAmt.UnFormattedValue = _tcdCashIn.Value;
                this.dfTCDCashOutAmt.UnFormattedValue = _tcdCashOut.Value;
                //#71893
                if (_isMadeBrokeTran)
                {
                    this.dfTCDCashInAmt.UnFormattedValue = _totalMadeAmt;
                    this.dfTCDCashOutAmt.UnFormattedValue = _totalBrokeAmt;
                }

                if (!_tlTranCode.IsNull && _tlTranCode.Value == "RLO") // #9161
                    this.dfTCDCashInAmt.UnFormattedValue = _totalRolloverAmt;

            }
            #endregion
            
			EnableDisableVisibleLogic("FormComplete");
		}

		private void gridDenomDetails_BeforePopulate(object sender, GridPopulateArgs e)
		{
			if (!_journalPtid.IsNull)
			{
				_busTlCashCount.OutputTypeId.Value = 1;
				if (_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition || 
					_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals)
				{
					_busTlCashCount.TranType.Value = CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "C");
					_busTlCashCount.PositionPtid.Value = (int)_journalPtid.Value;
				}
				else
				{
					_busTlCashCount.JournalPtid.Value = (int)_journalPtid.Value;
				}
			}
			gridDenomDetails.ListViewObject = _busTlCashCount;
		}

		private void gridDenomDetails_AfterPopulate(object sender, GridPopulateArgs e)
		{

            //#75737 - moved to InitCompleteEvent
            #region handle close out details
            //if (_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals ||
            //    _callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition)
            //{
            //    this.dfEndingCash.UnFormattedValue = _endingCash.Value;
            //    this.dfCashDrawerCount.UnFormattedValue = _totalCashDrawerCount;
            //    this.dfDifference.UnFormattedValue = _totalCashDrawerCount - 
            //        (_endingCash.IsNull? 0 : _endingCash.Value);
            //}
            //else
            //{
            //    this.dfCashInAmt.UnFormattedValue = _totalCashIn; //_cashIn.Value;
            //    this.dfCashOutAmt.UnFormattedValue = _totalCashOut; //_cashOut.Value;
            //    this.dfTCDCashInAmt.UnFormattedValue = _tcdCashIn.Value;
            //    this.dfTCDCashOutAmt.UnFormattedValue = _tcdCashOut.Value;
            //    //#71893
            //    if (_isMadeBrokeTran)
            //    {
            //        this.dfTCDCashInAmt.UnFormattedValue = _totalMadeAmt;
            //        this.dfTCDCashOutAmt.UnFormattedValue = _totalBrokeAmt;
            //    }
            //}
			#endregion
		}

		private void gridDenomDetails_FetchRowDone(object sender, GridRowArgs e)
		{
			//
			colComponent.Text = _busTlCashCount.GetComponentDesc(colTranType.Text, colCuid.Text, colType.Text, Convert.ToInt32(colPhysicalPos.UnFormattedValue));
			colCasseteID.Text = _busTlCashCount.GetCassetteIDDesc(colType.Text, colCuid.Text, Convert.ToInt32(colPhysicalPos.UnFormattedValue));
			if (colStrapValue.Text != string.Empty || colDenom.Text != string.Empty)
			{
				if (colStrapValue.Text != string.Empty)
					colAmt.UnFormattedValue = _busTlCashCount.CalcAmtByStrapValue(colType.Text, Convert.ToInt32(colQty.UnFormattedValue), Convert.ToDecimal(colStrapValue.UnFormattedValue));
				else if (colDenom.Text != string.Empty)
					colAmt.UnFormattedValue = _busTlCashCount.CalcAmtByDenom(colType.Text, Convert.ToInt32(colQty.UnFormattedValue), Convert.ToDecimal(colDenom.UnFormattedValue));
			}
			colType.Text = _busTlCashCount.GetDenomTypeDesc(colType.Text);
			//#71893
			if (_isMadeBrokeTran)
			{
				if (colTranType.Text == _mlTranTypeMade)
					_totalMadeAmt = _totalMadeAmt + Convert.ToDecimal(colAmt.UnFormattedValue);
				else if (colTranType.Text == _mlTranTypeBroke)
					_totalBrokeAmt = _totalBrokeAmt + Convert.ToDecimal(colAmt.UnFormattedValue);
			}
			else
			{
				if (colTranType.Text == _mlTranTypeCashIn)
					_totalCashIn = _totalCashIn + Convert.ToDecimal(colAmt.UnFormattedValue);
				else if (colTranType.Text == _mlTranTypeCashOut)
					_totalCashOut = _totalCashOut + Convert.ToDecimal(colAmt.UnFormattedValue);
                else if (colTranType.Text == _mlTranTypeRollover) // #9161
                    _totalRolloverAmt = _totalRolloverAmt + Convert.ToDecimal(colAmt.UnFormattedValue);
				else
					_totalCashDrawerCount = _totalCashDrawerCount + Convert.ToDecimal(colAmt.UnFormattedValue);
			}
		}
		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
			{
                // Begin #72916
				//if (_tellerVars.AdTlControl.TcdDevices.Value != GlobalVars.Instance.ML.Y) //#71893
				//{
					this.colCasseteID.Visible = false;
					this.colDescription.Width = this.colDescription.Width + this.colCasseteID.Width;
				//}
                // End #72916
				if (_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals ||
					_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition)
				{
					this.pGroupBoxStandard2.Visible = true;
					this.gbTotals.Visible = false;
					this.SetFieldColor();
				}
				else
				{
					this.pGroupBoxStandard2.Visible = false;
					this.gbTotals.Visible = true;
					//#71893
					if (_isMadeBrokeTran) //MAK/BRK tran
					{
						this.dfCashInAmt.Visible = false;
						this.dfCashOutAmt.Visible = false;
						this.lblCashInAmount.Visible = false;
						this.lblCashOutAmount.Visible = false;
						this.lblTCDCashInAmount.Text = CoreService.Translation.GetUserMessageX(361021); //Made Amount:
						this.lblTCDCashOutAmount.Text = CoreService.Translation.GetUserMessageX(361022); //Broke Amount:
					}
                    if (!_tlTranCode.IsNull && _tlTranCode.Value == "RLO") // #9161
                    {
                        this.dfCashInAmt.Visible = false;
                        this.dfCashOutAmt.Visible = false;
                        this.lblCashInAmount.Visible = false;
                        this.lblCashOutAmount.Visible = false;
                        this.lblTCDCashOutAmount.Visible = false;
                        this.dfTCDCashOutAmt.Visible = false;
                        this.lblTCDCashInAmount.Text = CoreService.Translation.GetUserMessageX(13257); //TCD/TCR Rollover Amount:
                    }
				}
			}
		}

		#region CallXMThruCDS
		private void CallXMThruCDS(string origin)
		{
			if (origin == "TlPosition")
			{
				_tlPositionTemp.SelectAllFields = false;

				//_tlPositionTemp.Ptid.Value = _busTlCashCount.PositionPtid.Value;
				//_tlPositionTemp.BranchNo.Value = _busTlCashCount.BranchNo.Value;
				//_tlPositionTemp.DrawerNo.Value = _busTlCashCount.DrawerNo.Value;
				if (_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.SummaryPosition || 
					_callerScreenId.Value == Phoenix.Shared.Constants.ScreenId.CashDrawerSummaryTotals)
					_tlPositionTemp.Ptid.Value = _busTlCashCount.PositionPtid.Value;
				else
					_tlPositionTemp.JournalPtid.Value = _busTlCashCount.JournalPtid.Value;

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
		//

		#region SetFieldColor
		private void SetFieldColor()
		{
			//#71493 - For some reason Code was Commented
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) == 0)
				dfDifference.ForeColor = System.Drawing.Color.Black;
			if (Convert.ToDecimal(dfDifference.UnFormattedValue) > 0)
				dfDifference.ForeColor = System.Drawing.Color.Blue;
			else if (Convert.ToDecimal(dfDifference.UnFormattedValue) < 0)
				dfDifference.ForeColor = System.Drawing.Color.Red;
		}
		#endregion SetFieldColor

		#endregion

		#region frmTranDenomDetails_PMdiPrintEvent
		private void frmTranDenomDetails_PMdiPrintEvent(object sender, EventArgs e)
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
			if (_busTlCashCount.PositionPtid.IsNull)
			{
				//PrintDrawerCashCountFromWindow( false, true);
				//Position PTID is not available and Content is NOT Defined in the Report so Susan or Jacque need to decide untill then it will not work!
				return;
			}
			else			
				PrintDrawerCashCount(false, true);			
		}

		#region PrintDrawercashCount
		private void PrintDrawerCashCount(bool displayBrowser, bool riseMessage)
		{			
			if (!_busTlCashCount.PositionPtid.IsNull)
				CallXMThruCDS("TlPosition");
			else
				return;
			if (!TellerVars.Instance.IsAppOnline)
				_printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);
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
		//
		#region Commented Code

		/*
		#region PrintDrawerCashCountFromWindow
		private void PrintDrawerCashCountFromWindow(bool displayBrowser, bool riseMessage)
		{
			#region variables
			if (!TellerVars.Instance.IsAppOnline)
				_printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);
			string cashDrawer = string.Empty;
			string endingCash = string.Empty;
			string difference = string.Empty;
			string tellerNoName = string.Empty;
			string branchNoName = string.Empty;
			string sqrParam = string.Empty;
			string tempValue= string.Empty;	
			Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
			myTempVar.Visible = false;
			myTempVar.TabStop = false;
			myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			string balDenomTracking = "N";
			string closeoutPostingDt;
			string closeoutPostingDateTime;
			//360724 - Generating Teller Cash Drawer Count [Drawer %1!]report.  Please wait...
			dlgInformation.Instance.ShowInfo(CoreService.Translation.GetTokenizeMessageX(360724, TellerVars.Instance.DrawerNo.ToString()));
			//
			CallXMThruCDS("TlPosition");
			CallXMThruCDS("AdGbBranch");
			branchNoName = (_adGbBranchBusObj.BranchNo.Value  + " - " + _adGbBranchBusObj.Name1.Value.Trim());
			CallXMThruCDS("AdGbRsm");

			branchNoName = (GlobalVars.CurrentBranchNo.ToString()  + " - " + _adGbBranchBusObj.Name1.Value.Trim());
			tellerNoName = GlobalVars.EmployeeId.ToString() + " - " + GlobalVars.EmployeeName;
			
			cashDrawer = this.dfCashInAmt.Text;			
			endingCash = this.dfTCDCashOutAmt.Text;				
			difference = this.dfDifference.Text;
			closeoutPostingDt = _tellerVars.PostingDt.ToString("MM/dd/yyyy");
			closeoutPostingDateTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");		
			#endregion variables

			try
			{
				if (TellerVars.Instance.IsAppOnline)
				{
					Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();
					report1.ReportName.Value = "TLO11500.sqr";
					report1.EmplId.Value = GlobalVars.EmployeeId;
					report1.FromDt.Value = GlobalVars.SystemDate;
					report1.ToDt.Value = GlobalVars.SystemDate;
					report1.RunDate.Value = DateTime.Now;
					report1.Param1.Value = "BalDenoTracking=" + balDenomTracking.Trim() + "~BranchName=" + branchNoName + "~TellerName=" + tellerNoName + "~CloseoutPosDt=" + closeoutPostingDt + "~CloseOutTimeStamp=" + closeoutPostingDateTime + "~DrawerNo=" + TellerVars.Instance.DrawerNo.ToString() + "~CashDrawer=" + cashDrawer + "~Endingcash=" + endingCash + "~Difference=" + difference;

					#region LB, LC, BC					
					//If you change the index of these columns you are #!~@!~
					//colLooseDenomId(0),colLooseDenom(1),colLooseDenomType(2),colLooseDemonination(3),colLooseCountValue(4),colLooseQuantity(5),colLooseAmount(6)
					sqrParam = string.Empty;
					for(int i = 0; i < gridDenomDetails.Items.Count; i++)
					{
						//gridDenomDetails.ContextRow = i;
						//structObj.DenomType.Trim() +  "=D=" + structObj.DenomId + "^M=" + structObj.Denom + "^C=" + structObj.CountValue.ToString() + "^Q=" + structObj.Quantity.ToString() + "^A=" + structObj.Amount.ToString() + "~");
						sqrParam = sqrParam + gridDenomDetails.Items[i].SubItems[2].Text.Trim() + 
							"=D=" + gridDenomDetails.Items[i].SubItems[0].Text.Trim() + 
							"^M=" + gridDenomDetails.Items[i].SubItems[3].Text.Trim();
						//
						if (gridDenomDetails.Items[i].SubItems[4].Text != null && gridDenomDetails.Items[i].SubItems[4].Text.Length > 0)
							sqrParam = sqrParam + "^C=" + CurrencyHelper.GetUnformattedValue(gridDenomDetails.Items[i].SubItems[4].Text);
						else
							sqrParam = sqrParam + "^C=" + "0";
						//
						if (gridDenomDetails.Items[i].SubItems[5].Text != null && gridDenomDetails.Items[i].SubItems[5].Text.Length > 0)
							sqrParam = sqrParam + "^Q=" + CurrencyHelper.GetUnformattedValue(gridDenomDetails.Items[i].SubItems[5].Text);
						else
							sqrParam = sqrParam + "^Q=" + "0";
						//
						if (gridDenomDetails.Items[i].SubItems[6].Text != null && gridDenomDetails.Items[i].SubItems[6].Text.Length > 0)
							sqrParam = sqrParam + "^A=" + CurrencyHelper.GetUnformattedValue(gridDenomDetails.Items[i].SubItems[6].Text) + "~";
						else
							sqrParam = sqrParam + "^A=" + "0~";						
					}
					report1.Param2.Value = sqrParam;
					#endregion 
					//

					#region Final Print
					sqrParam = string.Empty;
					report1.Param5.Value = sqrParam;
					report1.Param6.Value = sqrParam;
					report1.MiscParams.Value = sqrParam;
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
						_printProcess =_htmlPrinter.PrintPDFFromUrl(report1.OutputLink.Value, false);
					else					
						_htmlPrinter.ShowUrlPdf(report1.OutputLink.Value);
					#endregion 

				} //End of Online Report
				else
				{
					#region Variables
					StringBuilder htmlPage1  = new StringBuilder();
					string p1part1Html = string.Empty;
					string p1part2Html = string.Empty;
					string p1fixedHeaderTable = string.Empty;
					string p1cashEquationTable = string.Empty;
					string p1detailHeaderTable = string.Empty;
					string p1repeatDetailsTable = string.Empty;
					string p1subDetailHeader = string.Empty;
					string p1reportEndTable = string.Empty;				
					string fixedHeader = string.Empty;					
					string subHeader = p1subDetailHeader;
					string detailHeaderP1 = p1detailHeaderTable;
					string desc = string.Empty;
					string HTMLPagesPage1 = string.Empty;
					string subTitle = string.Empty;
					string detailRow = string.Empty;
					#endregion 

					#region Validate Template
					if (!_tlDrawerBalances.ValidateFileTLF115(1)) //Page Number 1
					{
						//File Not Found Exception
						if (riseMessage)					
						{
							dlgInformation.Instance.HideInfo();
							PMessageBox.Show(360738, MessageType.Warning, MessageBoxButtons.OK, Shared.Constants.UserConstants.TLF11500P1);
							//360738 - Offline Teller Position template %1! is not found.  Please contact support to rectify the problem.
						}
						if (CoreService.LogPublisher.IsLogEnabled)
							CoreService.LogPublisher.LogDebug(CoreService.Translation.GetTokenizeMessageX(360738, Shared.Constants.UserConstants.TLF11500P1));
						return;
					}
					_tlDrawerBalances.ExtractTablesFromHTML115Page1(out p1part1Html, out p1part2Html, out p1fixedHeaderTable, out p1cashEquationTable, out p1detailHeaderTable, 
						out p1repeatDetailsTable, out p1subDetailHeader,  out p1reportEndTable);
					#endregion 

					#region Build The Page 1 & 2 Header
					htmlPage1.Append(p1part1Html + Environment.NewLine);
					fixedHeader = p1fixedHeaderTable;
					fixedHeader = fixedHeader.Replace("phx_ReportNo", "TLO11500");
					fixedHeader = fixedHeader.Replace("phx_ReportName", "TELLER CASH DRAWER COUNT [Drawer #" + TellerVars.Instance.DrawerNo.ToString() + " ]");
					fixedHeader = fixedHeader.Replace("phx_ReportDt", GlobalVars.SystemDate.ToString("MM/dd/yyyy"));
					fixedHeader = fixedHeader.Replace("phx_RunDateTime", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
					fixedHeader = fixedHeader.Replace("phx_branchName", branchNoName);
					fixedHeader = fixedHeader.Replace("phx_CloseOutPdt", closeoutPostingDt);
					fixedHeader = fixedHeader.Replace("phx_TellerNoName", tellerNoName);
					fixedHeader = fixedHeader.Replace("phx_1CloseOutPdtTime", closeoutPostingDateTime); //1 for replacement to Work
					htmlPage1.Append(fixedHeader + Environment.NewLine);
					#endregion 

					//
					#region Buld cashEquation
					string cashEquation = p1cashEquationTable;
					cashEquation = cashEquation.Replace("phx_d_CashDrawer", cashDrawer);
					cashEquation = cashEquation.Replace("phx_d_EndingCash", endingCash);
					cashEquation = cashEquation.Replace("phx_d_Difference", difference);
					htmlPage1.Append(cashEquation);
					//
					htmlPage1.Append(p1detailHeaderTable + Environment.NewLine);
			
					#endregion
					//
					#region LB, LC and BC
					//If you change the index of these columns you are #!~@!~
					//colLooseDenomId(0),colLooseDenom(1),colLooseDenomType(2),colLooseDemonination(3),colLooseCountValue(4),colLooseQuantity(5),colLooseAmount(6)
					sqrParam = string.Empty;
					subTitle = p1subDetailHeader.Replace("phx_SubTitle", "Loose Bills and Loose Coin");
					htmlPage1.Append(subTitle + Environment.NewLine);
					for(int i = 0; i < gridDenomDetails.Items.Count; i++)
					{
						#region CashCount Record
						detailRow = p1repeatDetailsTable;
						desc = gridDenomDetails.Items[i].SubItems[3].Text.Trim();
						if (desc.Length > 25)
							desc = desc.Substring(0, 25);

						detailRow = detailRow.Replace("phx_Description", desc);
						detailRow = detailRow.Replace("phx_Denom", gridDenomDetails.Items[i].SubItems[1].Text.Trim());
						//
						detailRow = detailRow.Replace("phx_CountValue", string.Empty);
//						if (gridDenomDetails.Items[i].SubItems[4].Text != null && gridDenomDetails.Items[i].SubItems[4].Text.Length > 0)
//							detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat(gridDenomDetails.Items[i].SubItems[4].Text));
//						else
//							detailRow = detailRow.Replace("phx_CountValue", GetDecimalFormat("0"));
						//

						if (gridDenomDetails.Items[i].SubItems[5].Text != null && gridDenomDetails.Items[i].SubItems[5].Text.Length > 0)
							detailRow = detailRow.Replace("phx_Quantity", GetDecimalFormat(gridDenomDetails.Items[i].SubItems[5].Text));
						else
							detailRow = detailRow.Replace("phx_Quantity", GetDecimalFormat("0"));

						if (gridDenomDetails.Items[i].SubItems[6].Text != null && gridDenomDetails.Items[i].SubItems[6].Text.Length > 0)
							detailRow = detailRow.Replace("phx_amount", gridDenomDetails.Items[i].SubItems[6].Text );
						else
							detailRow = detailRow.Replace("phx_amount", "$0.00");	
						htmlPage1.Append(detailRow + Environment.NewLine);
						#endregion CashCount Record
					}
					#endregion 
					//

					//

					//
					HTMLPagesPage1 = htmlPage1.ToString() +  Environment.NewLine + p1reportEndTable  + Environment.NewLine + p1part2Html + Environment.NewLine;
					if (CoreService.LogPublisher.IsLogEnabled)
						CoreService.LogPublisher.LogDebug("\n" + HTMLPagesPage1 + "\n");
					// Print this Junk				
					_htmlPrinter.PrintHtml(HTMLPagesPage1, false);
					//
				} //End of Offline Report
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
				dlgInformation.Instance.HideInfo();
				if (!TellerVars.Instance.IsAppOnline  && _printerSettings != null)
					_printerSettings.ChangePageOrientation(_printerSettings.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);				
			}
		}
		#endregion PrintDrawerCashCountFromWindow
		
		

		#region DecimalFormatter
		private string GetDecimalFormat(decimal decimalValue, string mask)
		{
			if (decimalValue == Decimal.MinValue || decimalValue == Decimal.MaxValue)
				return Convert.ToDecimal("0").ToString(mask);
			return decimalValue.ToString(mask);
		}
		private string GetDecimalFormat(decimal decimalValue)
		{
			if (decimalValue == Decimal.MinValue || decimalValue == Decimal.MaxValue)
				return "0.00";
			return decimalValue.ToString("#,##0.00");
		}
		private string GetDecimalFormat(string decimalValue)
		{
			decimal localValue = 0;
			if (decimalValue == null)
				return "";
			if (decimalValue.Length == 0)
				return "";
			if (decimalValue.IndexOf("$") >= 0)
				return decimalValue;
			try
			{
				localValue = Convert.ToDecimal(decimalValue);
			}
			catch(System.InvalidCastException)
			{
				//Invalid Value
				return "";
			}
			if (localValue == Decimal.MinValue || localValue == Decimal.MaxValue)
				return "0.00";
			return localValue.ToString("#,##0.00");
		}
		#endregion 
		//
		//
		
		*/
		#endregion 
		//
		//
		#endregion frmTranDenomDetails_PMdiPrintEvent
	}
}
