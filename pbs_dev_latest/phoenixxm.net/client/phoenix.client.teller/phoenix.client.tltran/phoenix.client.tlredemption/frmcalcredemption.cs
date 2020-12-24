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
// File Name: frmCalcRedemption.cs
// NameSpace: Phoenix.Client.TlRedemption
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//??/??/????	1		Rajiv		???
//01/04/2006	2		Vreddy		#70582, 70262 - Other issues are fixed
//01/04/2006	3		FOyebola	#74042 - Reset totals when row item is deleted
// 05Nov2009	4		GDiNatale	#6615 - SetValue Framework change
//05/19/2010    5       LSimpson    #8770 - Corrected issues with pbCopyItem not being enabled and behavior of grid.
//08/06/2012    6       Mkrishna    #19058 - Adding call to base on initParameters.
//07/09/2013    7       mselvaga    #140798 - Savings Bond Redemption History Desc changes added.
//09/11/2013    8       mselvaga    #24612 - Savings Bond Redemption Hist- 10931 - frmCalcRedemption- Serial # bypassed.
//12/4/2017     9       SVasu       #77441 - savings bond Serial no datatype changed
//09/05/2019    10      rpoddar     #118850 - Teller WF Changes ( tagged by  #TellerWF )
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
//#140798
using System.Collections.Generic;
using System.Text;
using System.Xml;

using Phoenix.Windows.Forms;
using Phoenix.BusObj.Control;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Global;
//#140798
using Phoenix.FrameWork.CDS;
using Phoenix.BusObj.Misc;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.Shared.Windows;


namespace Phoenix.Windows.TlRedemption
{
	/// <summary>
	/// Summary description for dfwCalcRedemption.
	/// </summary>
	public class frmCalcRedemption : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbRedemptionDate;
		private Phoenix.Windows.Forms.PLabelStandard lblMonth;
		private Phoenix.Windows.Forms.PdfStandard dfMonth;
		private Phoenix.Windows.Forms.PLabelStandard lblYear;
		private Phoenix.Windows.Forms.PdfStandard dfYear;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbRedemptionCalculations;
		private Phoenix.Windows.Forms.PGridColumnComboBox colInstrumentType;
		private Phoenix.Windows.Forms.PGridColumnComboBox colFaceValue;
		private Phoenix.Windows.Forms.PGridColumn colIssueMonth;
		private Phoenix.Windows.Forms.PGridColumn colIssueYear;
		private Phoenix.Windows.Forms.PGridColumn colInterestEarned;
		private Phoenix.Windows.Forms.PGridColumn colRedemptionValue;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbCalculatedTotals;
		private Phoenix.Windows.Forms.PLabelStandard lblofItems;
		private Phoenix.Windows.Forms.PDfDisplay dfNoOfItems;
		private Phoenix.Windows.Forms.PLabelStandard lblInterestEarned;
		private Phoenix.Windows.Forms.PDfDisplay dfInterestEarned;
		private Phoenix.Windows.Forms.PLabelStandard lblRedemptionValue;
		private Phoenix.Windows.Forms.PDfDisplay dfRedemptionValue;
		private Phoenix.Windows.Forms.PAction pbReturn;
		private Phoenix.Windows.Forms.PAction pbCopyItem;
		private Phoenix.Windows.Forms.PAction pbDeleteItem;
		private Phoenix.Windows.Forms.PGrid grdCalculateRedemption;
		private Phoenix.Windows.Forms.PAction pbClear;
		private Phoenix.Windows.Forms.PLabelStandard lblHelp;

		private PcRedeemValues _pcRedeemValues = null;
		private bool processingRecalc = false;
		private PfwStandard _parentForm =  null;
		private DateTime effectiveDt;
		private bool _riseCloseMsg = true;
        //Begin #140798
        private PDecimal _pnJournalRimPtid = new PDecimal("JournalRimPtid");
        private PGridColumn colSerialNo;
        private PAction pbCancel;
        private PInt _pnScreenID = new PInt("ScreenID");
        private ArrayList SavedBondsList = null;
        private ArrayList CurrentBondsList = new ArrayList();
        private PLabelStandard lblHelp2;
        private PBoolean _pbIsSerialNoRequired = new PBoolean("IsSerialNoRequired");
        private bool _isViewOnlyMode = false;
        //private Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;    //#140798
        private Phoenix.BusObj.Global.GbBondRedemptHistory _busObjGbBondHist = new GbBondRedemptHistory();
        private PGridColumn colEffectiveDt;   //#140798
        private PInt _pnRimNo = new PInt("RimNo");
        //End #140798
        bool _invokedByWorkflow = false;    // #TellerWF

        public frmCalcRedemption()
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
            this.gbRedemptionDate = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfYear = new Phoenix.Windows.Forms.PdfStandard();
            this.lblYear = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfMonth = new Phoenix.Windows.Forms.PdfStandard();
            this.lblMonth = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbRedemptionCalculations = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblHelp2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.grdCalculateRedemption = new Phoenix.Windows.Forms.PGrid();
            this.colInstrumentType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colFaceValue = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colIssueMonth = new Phoenix.Windows.Forms.PGridColumn();
            this.colIssueYear = new Phoenix.Windows.Forms.PGridColumn();
            this.colSerialNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colInterestEarned = new Phoenix.Windows.Forms.PGridColumn();
            this.colRedemptionValue = new Phoenix.Windows.Forms.PGridColumn();
            this.lblHelp = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbCalculatedTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfRedemptionValue = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblRedemptionValue = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfInterestEarned = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblInterestEarned = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNoOfItems = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblofItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbReturn = new Phoenix.Windows.Forms.PAction();
            this.pbCopyItem = new Phoenix.Windows.Forms.PAction();
            this.pbDeleteItem = new Phoenix.Windows.Forms.PAction();
            this.pbClear = new Phoenix.Windows.Forms.PAction();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.colEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.gbRedemptionDate.SuspendLayout();
            this.gbRedemptionCalculations.SuspendLayout();
            this.gbCalculatedTotals.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbReturn,
            this.pbCopyItem,
            this.pbDeleteItem,
            this.pbClear,
            this.pbCancel});
            // 
            // gbRedemptionDate
            // 
            this.gbRedemptionDate.Controls.Add(this.dfYear);
            this.gbRedemptionDate.Controls.Add(this.lblYear);
            this.gbRedemptionDate.Controls.Add(this.dfMonth);
            this.gbRedemptionDate.Controls.Add(this.lblMonth);
            this.gbRedemptionDate.Location = new System.Drawing.Point(4, 4);
            this.gbRedemptionDate.Name = "gbRedemptionDate";
            this.gbRedemptionDate.PhoenixUIControl.ObjectId = 1;
            this.gbRedemptionDate.Size = new System.Drawing.Size(680, 36);
            this.gbRedemptionDate.TabIndex = 0;
            this.gbRedemptionDate.TabStop = false;
            this.gbRedemptionDate.Text = "Redemption Date";
            // 
            // dfYear
            // 
            this.dfYear.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfYear.Location = new System.Drawing.Point(163, 12);
            this.dfYear.MaxLength = 4;
            this.dfYear.Name = "dfYear";
            this.dfYear.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfYear.PhoenixUIControl.ObjectId = 3;
            this.dfYear.Size = new System.Drawing.Size(43, 20);
            this.dfYear.TabIndex = 3;
            this.dfYear.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfYear_PhoenixUIValidateEvent);
            // 
            // lblYear
            // 
            this.lblYear.AutoEllipsis = true;
            this.lblYear.Location = new System.Drawing.Point(106, 14);
            this.lblYear.Name = "lblYear";
            this.lblYear.PhoenixUIControl.ObjectId = 3;
            this.lblYear.Size = new System.Drawing.Size(56, 20);
            this.lblYear.TabIndex = 2;
            this.lblYear.Text = "&Year:";
            // 
            // dfMonth
            // 
            this.dfMonth.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMonth.Location = new System.Drawing.Point(69, 12);
            this.dfMonth.MaxLength = 2;
            this.dfMonth.Name = "dfMonth";
            this.dfMonth.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfMonth.PhoenixUIControl.ObjectId = 2;
            this.dfMonth.Size = new System.Drawing.Size(26, 20);
            this.dfMonth.TabIndex = 1;
            this.dfMonth.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfMonth_PhoenixUIValidateEvent);
            // 
            // lblMonth
            // 
            this.lblMonth.AutoEllipsis = true;
            this.lblMonth.Location = new System.Drawing.Point(6, 14);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.PhoenixUIControl.ObjectId = 2;
            this.lblMonth.Size = new System.Drawing.Size(63, 20);
            this.lblMonth.TabIndex = 0;
            this.lblMonth.Text = "&Month:";
            // 
            // gbRedemptionCalculations
            // 
            this.gbRedemptionCalculations.Controls.Add(this.lblHelp2);
            this.gbRedemptionCalculations.Controls.Add(this.grdCalculateRedemption);
            this.gbRedemptionCalculations.Controls.Add(this.lblHelp);
            this.gbRedemptionCalculations.Location = new System.Drawing.Point(4, 40);
            this.gbRedemptionCalculations.Name = "gbRedemptionCalculations";
            this.gbRedemptionCalculations.PhoenixUIControl.ObjectId = 4;
            this.gbRedemptionCalculations.Size = new System.Drawing.Size(680, 364);
            this.gbRedemptionCalculations.TabIndex = 1;
            this.gbRedemptionCalculations.TabStop = false;
            this.gbRedemptionCalculations.Text = "Redemption Calculations";
            // 
            // lblHelp2
            // 
            this.lblHelp2.AutoEllipsis = true;
            this.lblHelp2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelp2.ForeColor = System.Drawing.Color.Blue;
            this.lblHelp2.Location = new System.Drawing.Point(8, 339);
            this.lblHelp2.Name = "lblHelp2";
            this.lblHelp2.PhoenixUIControl.ObjectId = 24;
            this.lblHelp2.Size = new System.Drawing.Size(664, 20);
            this.lblHelp2.TabIndex = 2;
            this.lblHelp2.Text = "Serial # is required. If serial # not available please enter \\\'0\\\' to continue.";
            // 
            // grdCalculateRedemption
            // 
            this.grdCalculateRedemption.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colInstrumentType,
            this.colFaceValue,
            this.colIssueMonth,
            this.colIssueYear,
            this.colSerialNo,
            this.colInterestEarned,
            this.colRedemptionValue,
            this.colEffectiveDt});
            this.grdCalculateRedemption.IsMaxNumRowsCustomized = false;
            this.grdCalculateRedemption.LinesInHeader = 2;
            this.grdCalculateRedemption.Location = new System.Drawing.Point(6, 42);
            this.grdCalculateRedemption.Name = "grdCalculateRedemption";
            this.grdCalculateRedemption.Size = new System.Drawing.Size(670, 294);
            this.grdCalculateRedemption.TabIndex = 0;
            this.grdCalculateRedemption.EndOfRows += new Phoenix.Windows.Forms.GridEventHandler(this.grdCalculateRedemption_EndOfRows);
            this.grdCalculateRedemption.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdCalculateRedemption_SelectedIndexChanged);
            // 
            // colInstrumentType
            // 
            this.colInstrumentType.AutoDrop = false;
            this.colInstrumentType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colInstrumentType.PhoenixUIControl.ObjectId = 6;
            this.colInstrumentType.ReadOnly = false;
            this.colInstrumentType.Title = "Instrument Type";
            this.colInstrumentType.Width = 110;
            this.colInstrumentType.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colInstrumentType_PhoenixUIValidateEvent);
            // 
            // colFaceValue
            // 
            this.colFaceValue.AutoDrop = false;
            this.colFaceValue.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colFaceValue.PhoenixUIControl.ObjectId = 7;
            this.colFaceValue.PhoenixUIControl.XmlTag = "0";
            this.colFaceValue.ReadOnly = false;
            this.colFaceValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colFaceValue.Title = "Face Value";
            this.colFaceValue.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colFaceValue_PhoenixUIValidateEvent);
            // 
            // colIssueMonth
            // 
            this.colIssueMonth.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colIssueMonth.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colIssueMonth.PhoenixUIControl.InputMask = "";
            this.colIssueMonth.PhoenixUIControl.MaxScale = 2;
            this.colIssueMonth.PhoenixUIControl.ObjectId = 8;
            this.colIssueMonth.PhoenixUIControl.XmlTag = "1";
            this.colIssueMonth.ReadOnly = false;
            this.colIssueMonth.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colIssueMonth.Title = "Issue Month";
            this.colIssueMonth.Width = 44;
            this.colIssueMonth.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colIssueMonth_PhoenixUIValidateEvent);
            // 
            // colIssueYear
            // 
            this.colIssueYear.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colIssueYear.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colIssueYear.PhoenixUIControl.InputMask = "";
            this.colIssueYear.PhoenixUIControl.ObjectId = 9;
            this.colIssueYear.PhoenixUIControl.XmlTag = "2";
            this.colIssueYear.ReadOnly = false;
            this.colIssueYear.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colIssueYear.Title = "Issue Year";
            this.colIssueYear.Width = 44;
            this.colIssueYear.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colIssueYear_PhoenixUIValidateEvent);
            // 
            // colSerialNo
            // 
            //Begin #77441 - changed integer to decimal
            this.colSerialNo.CurrencyType = Phoenix.Windows.Forms.UICurrencyType.UserDefined;
            this.colSerialNo.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSerialNo.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colSerialNo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
            this.colSerialNo.PhoenixUIControl.InputMask = "99999999999999";
            //End #77441
            this.colSerialNo.PhoenixUIControl.XmlTag = "SerialNo";
            this.colSerialNo.ReadOnly = false;
            this.colSerialNo.Title = "Serial #";

            this.colSerialNo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colSerialNo_PhoenixUIValidateEvent);
            // 
            // colInterestEarned
            // 
            this.colInterestEarned.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colInterestEarned.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colInterestEarned.PhoenixUIControl.ObjectId = 10;
            this.colInterestEarned.PhoenixUIControl.XmlTag = "3";
            this.colInterestEarned.ShowNullAsEmpty = true;
            this.colInterestEarned.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colInterestEarned.Title = "Interest Earned";
            this.colInterestEarned.Width = 128;
            // 
            // colRedemptionValue
            // 
            this.colRedemptionValue.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colRedemptionValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colRedemptionValue.PhoenixUIControl.ObjectId = 11;
            this.colRedemptionValue.PhoenixUIControl.XmlTag = "4";
            this.colRedemptionValue.ShowNullAsEmpty = true;
            this.colRedemptionValue.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colRedemptionValue.Title = "Redemption Value";
            this.colRedemptionValue.Width = 137;
            // 
            // lblHelp
            // 
            this.lblHelp.AutoEllipsis = true;
            this.lblHelp.Location = new System.Drawing.Point(8, 16);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.PhoenixUIControl.ObjectId = 22;
            this.lblHelp.Size = new System.Drawing.Size(664, 20);
            this.lblHelp.TabIndex = 1;
            this.lblHelp.Text = "When finished, press F10 to return the amount to the transaction window.";
            // 
            // gbCalculatedTotals
            // 
            this.gbCalculatedTotals.Controls.Add(this.dfRedemptionValue);
            this.gbCalculatedTotals.Controls.Add(this.lblRedemptionValue);
            this.gbCalculatedTotals.Controls.Add(this.dfInterestEarned);
            this.gbCalculatedTotals.Controls.Add(this.lblInterestEarned);
            this.gbCalculatedTotals.Controls.Add(this.dfNoOfItems);
            this.gbCalculatedTotals.Controls.Add(this.lblofItems);
            this.gbCalculatedTotals.Location = new System.Drawing.Point(4, 404);
            this.gbCalculatedTotals.Name = "gbCalculatedTotals";
            this.gbCalculatedTotals.PhoenixUIControl.ObjectId = 12;
            this.gbCalculatedTotals.Size = new System.Drawing.Size(680, 40);
            this.gbCalculatedTotals.TabIndex = 2;
            this.gbCalculatedTotals.TabStop = false;
            this.gbCalculatedTotals.Text = "Calculated Totals";
            // 
            // dfRedemptionValue
            // 
            this.dfRedemptionValue.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRedemptionValue.Location = new System.Drawing.Point(420, 16);
            this.dfRedemptionValue.Multiline = true;
            this.dfRedemptionValue.Name = "dfRedemptionValue";
            this.dfRedemptionValue.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRedemptionValue.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfRedemptionValue.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfRedemptionValue.Size = new System.Drawing.Size(75, 16);
            this.dfRedemptionValue.TabIndex = 0;
            // 
            // lblRedemptionValue
            // 
            this.lblRedemptionValue.AutoEllipsis = true;
            this.lblRedemptionValue.Location = new System.Drawing.Point(296, 16);
            this.lblRedemptionValue.Name = "lblRedemptionValue";
            this.lblRedemptionValue.PhoenixUIControl.ObjectId = 15;
            this.lblRedemptionValue.Size = new System.Drawing.Size(112, 20);
            this.lblRedemptionValue.TabIndex = 1;
            this.lblRedemptionValue.Text = "Redemption Value:";
            // 
            // dfInterestEarned
            // 
            this.dfInterestEarned.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInterestEarned.Location = new System.Drawing.Point(216, 16);
            this.dfInterestEarned.Multiline = true;
            this.dfInterestEarned.Name = "dfInterestEarned";
            this.dfInterestEarned.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfInterestEarned.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfInterestEarned.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfInterestEarned.Size = new System.Drawing.Size(68, 16);
            this.dfInterestEarned.TabIndex = 2;
            // 
            // lblInterestEarned
            // 
            this.lblInterestEarned.AutoEllipsis = true;
            this.lblInterestEarned.Location = new System.Drawing.Point(120, 16);
            this.lblInterestEarned.Name = "lblInterestEarned";
            this.lblInterestEarned.PhoenixUIControl.ObjectId = 14;
            this.lblInterestEarned.Size = new System.Drawing.Size(88, 20);
            this.lblInterestEarned.TabIndex = 3;
            this.lblInterestEarned.Text = "Interest Earned:";
            // 
            // dfNoOfItems
            // 
            this.dfNoOfItems.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfNoOfItems.Location = new System.Drawing.Point(72, 16);
            this.dfNoOfItems.Multiline = true;
            this.dfNoOfItems.Name = "dfNoOfItems";
            this.dfNoOfItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfNoOfItems.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoOfItems.Size = new System.Drawing.Size(38, 16);
            this.dfNoOfItems.TabIndex = 4;
            // 
            // lblofItems
            // 
            this.lblofItems.AutoEllipsis = true;
            this.lblofItems.Location = new System.Drawing.Point(6, 16);
            this.lblofItems.Name = "lblofItems";
            this.lblofItems.PhoenixUIControl.ObjectId = 13;
            this.lblofItems.Size = new System.Drawing.Size(58, 20);
            this.lblofItems.TabIndex = 5;
            this.lblofItems.Text = "# of Items:";
            // 
            // pbReturn
            // 
            this.pbReturn.LongText = "&Return";
            this.pbReturn.Name = "&Return";
            this.pbReturn.ObjectId = 18;
            this.pbReturn.Shortcut = System.Windows.Forms.Keys.F10;
            this.pbReturn.ShortText = "&Return";
            this.pbReturn.Tag = null;
            this.pbReturn.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReturn_Click);
            // 
            // pbCopyItem
            // 
            this.pbCopyItem.LongText = "Copy &Item";
            this.pbCopyItem.Name = "Copy &Item";
            this.pbCopyItem.ObjectId = 19;
            this.pbCopyItem.ShortText = "Copy &Item";
            this.pbCopyItem.Tag = null;
            this.pbCopyItem.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCopyItem_Click);
            // 
            // pbDeleteItem
            // 
            this.pbDeleteItem.LongText = "&Delete Item";
            this.pbDeleteItem.Name = "&Delete Item";
            this.pbDeleteItem.ObjectId = 20;
            this.pbDeleteItem.ShortText = "&Delete Item";
            this.pbDeleteItem.Tag = null;
            this.pbDeleteItem.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDeleteItem_Click);
            // 
            // pbClear
            // 
            this.pbClear.LongText = "&Clear";
            this.pbClear.Name = "&Clear";
            this.pbClear.ObjectId = 21;
            this.pbClear.ShortText = "&Clear";
            this.pbClear.Tag = null;
            this.pbClear.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClear_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.LongText = "&Cancel";
            this.pbCancel.Name = "pbCancel";
            this.pbCancel.ShortText = "&Cancel";
            this.pbCancel.Tag = null;
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // colEffectiveDt
            // 
            this.colEffectiveDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colEffectiveDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colEffectiveDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colEffectiveDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colEffectiveDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.colEffectiveDt.Title = "Column";
            this.colEffectiveDt.Visible = false;
            // 
            // frmCalcRedemption
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbCalculatedTotals);
            this.Controls.Add(this.gbRedemptionCalculations);
            this.Controls.Add(this.gbRedemptionDate);
            this.Name = "frmCalcRedemption";
            this.ScreenId = 10931;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmCalcRedemption_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmCalcRedemption_PInitCompleteEvent);
            this.PMdiPrintEvent += new System.EventHandler(this.frmCalcRedemption_PMdiPrintEvent);
            this.gbRedemptionDate.ResumeLayout(false);
            this.gbRedemptionDate.PerformLayout();
            this.gbRedemptionCalculations.ResumeLayout(false);
            this.gbCalculatedTotals.ResumeLayout(false);
            this.gbCalculatedTotals.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        //Begin #TellerWF
        public override void OnCreateParameters()
        {
            if (Parameters.Count == 0)
            {
                Parameters.Add(new PString("TranSource"));
                Parameters.Add(new PInt("RimNo"));
                Parameters.Add(new PString("OutputJson"));
            }
        }
        //End #TellerWF
        public override void InitParameters(params object[] paramList)
		{
            //paramList[0] = new PString("TranSource", "Workflow");
            //paramList[1] = new PString("TlTranCode", "201");
            //paramList[2] = new PString("OutputJson", "");
            //Begin #TellerWF
            if (paramList != null && paramList.Length > 1 && paramList[0] is PBaseType)
            {
                base.InitParameters(paramList);
                if (Parameters.Contains("TranSource") && Parameters["TranSource"].StringValue == InputParamTranSet.TranSourceParamValue)
                {
                    _invokedByWorkflow = true;

                    if (Parameters["RimNo"].IntValue > 0)
                        _pnRimNo.Value = Parameters["RimNo"].IntValue;

                    _pbIsSerialNoRequired.Value = TellerVars.Instance.AdTlControl.ReqSerialNoBondRedempt.Value == "Y";
                    SavedBondsList = new ArrayList();
                    _pnScreenID.Value = Phoenix.Shared.Constants.ScreenId.TlTranCode;
                    //this.AutoFetch = false;
                    this.IsDirty = true;    // set the form dirty by default to trigger save
                    return;
                }
            }
            //End #TellerWF
            if (paramList.Length == 1)
				effectiveDt = Convert.ToDateTime(paramList[0]);
            if (paramList.Length > 1)   //#140798
            {
                if (paramList[1] != null)
                    _pnScreenID.Value = Convert.ToInt32(paramList[1]);
                if (paramList.Length > 2)
                {
                    if (paramList[2] != null)
                        SavedBondsList = (ArrayList)paramList[2];
                    if (paramList.Length > 3)
                    {
                        if (paramList[3] != null)
                            _pnJournalRimPtid.Value = Convert.ToDecimal(paramList[3]);
                    }
                    if (paramList.Length > 4)
                    {
                        if (paramList[4] != null)
                            _pbIsSerialNoRequired.Value = Convert.ToBoolean(paramList[4]);
                    }
                    if (paramList.Length > 5)
                    {
                        if (paramList[5] != null)
                            _pnRimNo.Value = Convert.ToInt32(paramList[5]);
                    }
                }
            }
            base.InitParameters(paramList); //#19058
		}

        #region Methods
        private void InsertNewRow( bool reset )
		{
			InsertNewRow( reset, true );
		}
		private void InsertNewRow( bool reset, bool setFocus )
		{
			int prevContext = 0;
			if ( reset )
			{
				grdCalculateRedemption.ResetTable();
				dfNoOfItems.UnFormattedValue = 0;
				dfInterestEarned.UnFormattedValue = 0;
				dfRedemptionValue.UnFormattedValue = 0;
			}

			prevContext = grdCalculateRedemption.ContextRow;
			if ( grdCalculateRedemption.Count > 0 )
				grdCalculateRedemption.ContextRow = grdCalculateRedemption.Count - 1;

			if ( IsValidRow() || grdCalculateRedemption.Count == 0 )
			{
				grdCalculateRedemption.ContextRow = grdCalculateRedemption.AddNewRow();
//				if ( setFocus )
//					colInstrumentType.Focus();
			}
			else
				grdCalculateRedemption.ContextRow = prevContext;

			if ( reset )
				grdCalculateRedemption.SelectRow(0, true);
			//Make sure when scroll appears grid moves correctly
			grdCalculateRedemption.SelectRow(grdCalculateRedemption.Count - 1);
		}

		private bool ValidateEntry( PGridColumn editedColumn, bool addRow )
		{
			string focusField = null;
            int prevContextRow = grdCalculateRedemption.ContextRow; //#140798

			// in case some underired column validate event is fired
			if ( addRow && processingRecalc )
				return true;
			_pcRedeemValues.CleanUpValues();
			_pcRedeemValues.RedemptionMonth.Value = Convert.ToInt16( dfMonth.UnFormattedValue );
			_pcRedeemValues.RedemptionYear.Value = Convert.ToInt16( dfYear.UnFormattedValue);
			MoveBetweenColAndObj( colInstrumentType, false );
			MoveBetweenColAndObj( colFaceValue, false );
			//_pcRedeemValues.InstrumentType.Value = Convert.ToString( colInstrumentType.UnFormattedValue );
			//_pcRedeemValues.FaceValue.Value = Convert.ToString( colFaceValue.UnFormattedValue );
			//colInstrumentType.ColumnToField( _pcRedeemValues.InstrumentType );
			//colFaceValue.ColumnToField( _pcRedeemValues.FaceValue );
			colIssueYear.ColumnToField( _pcRedeemValues.IssueYear );
			colIssueMonth.ColumnToField( _pcRedeemValues.IssueMonth );
            colSerialNo.ColumnToField(_pcRedeemValues.SerialNo);    //#140798
			UpdateTotals( false );
			if (! _pcRedeemValues.ValidateEntry( GetXmlTagForControl( editedColumn ), ref focusField ))
			{
				if ( _pcRedeemValues.Messages.Count > 0 )
					PMessageBox.Show( _pcRedeemValues.Messages[0] as PMessage );
				//if ( focusField == GetXmlTagForControl( editedColumn ) )
					//editedColumn.Focus();
				return false;
			}
			if ( _pcRedeemValues.Messages.Count > 0 )
				PMessageBox.Show( _pcRedeemValues.Messages[0] as PMessage );

			MoveBetweenColAndObj( colInstrumentType, true );
			MoveBetweenColAndObj( colFaceValue, true );
//			colInstrumentType.UnFormattedValue =_pcRedeemValues.InstrumentType.Value;
//			colFaceValue.UnFormattedValue = _pcRedeemValues.FaceValue.Value;
//			colInstrumentType.FieldToColumn( _pcRedeemValues.InstrumentType );
//			colFaceValue.FieldToColumn( _pcRedeemValues.FaceValue );
			colIssueYear.FieldToColumn( _pcRedeemValues.IssueYear );
			colIssueMonth.FieldToColumn( _pcRedeemValues.IssueMonth );
            colSerialNo.FieldToColumn(_pcRedeemValues.SerialNo);    //#140798
			if ( !_pcRedeemValues.IntEarned.IsNull )
			{
				colRedemptionValue.FieldToColumn( _pcRedeemValues.RedemptionValue );
				colInterestEarned.FieldToColumn( _pcRedeemValues.IntEarned );
				UpdateTotals( true );
                #region #8770
                //#140798
                //if (addRow)
                //{
                //    InsertNewRow(false);
                //    colIssueYear.Text = _pcRedeemValues.IssueYear.StringValue; // #8770 - hack
                //}
                if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true &&
                _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)
                {
                    if (colSerialNo.UnFormattedValue != null && string.IsNullOrEmpty(colSerialNo.Text))
                    {
                        if (addRow)
                        {
                            InsertNewRow(false);
                            grdCalculateRedemption.ContextRow = prevContextRow;
                            //colIssueYear.Text = _pcRedeemValues.IssueYear.StringValue; // #8770 - hack
                            editedColumn.Text = _pcRedeemValues.GetFieldByXmlTag(editedColumn.XmlTag).StringValue;
                            grdCalculateRedemption.ContextRow = prevContextRow + 1;
                        }
                    }
                }
                else if (editedColumn == colSerialNo)
                {
                    if (addRow)
                    {
                        InsertNewRow(false);
                        grdCalculateRedemption.ContextRow = prevContextRow;
                        //colIssueYear.Text = _pcRedeemValues.IssueYear.StringValue; // #8770 - hack
                        editedColumn.Text = _pcRedeemValues.GetFieldByXmlTag(editedColumn.XmlTag).StringValue;
                        //grdCalculateRedemption.ContextRow = prevContextRow + 1;
                    }
                }
                #endregion
            }
			else if ( addRow && _pcRedeemValues.Messages.Count > 0 )
			{
				//grdCalculateRedemption.Focus();
				grdCalculateRedemption.SelectRow( grdCalculateRedemption.ContextRow, true );
			}

            EnableDisableVisibleLogic("grdClicked"); // #8770

			return true;

		}

		private bool ValidateRedeemYrMon( Control editedControl )
		{
			bool retValue = false;
			int year = 0;
			DialogResult response;
			try
			{
				if ( editedControl == dfYear )
				{
					year = Convert.ToInt32( dfYear.UnFormattedValue );
					retValue = _pcRedeemValues.ValidateRedeemYear( ref year );
					if ( retValue )
						dfYear.UnFormattedValue = year;
				}
				else
				{
					retValue = _pcRedeemValues.ValidateMonth( Convert.ToInt32( dfMonth.UnFormattedValue ));
				}

				if (!retValue && _pcRedeemValues.Messages.Count > 0 )
				{
					PMessageBox.Show( _pcRedeemValues.Messages[0] as PMessage );
					return false;
				}

				if ( Convert.ToInt16( dfNoOfItems.UnFormattedValue ) == 0 )
					return true;

				if ( !(editedControl as PdfStandard).IsDirty )
					return true;
				response = PMessageBox.Show( 314323, MessageType.Question, MessageBoxButtons.YesNoCancel, String.Empty );

				if ( response == DialogResult.Cancel )
					(editedControl as PdfStandard).SavePrevValue( true );
				else if ( response == DialogResult.No )
				{
					InsertNewRow( true );
				}
				else
				{
					processingRecalc = true;
					for ( int i=0; i< grdCalculateRedemption.Count; i++ )
					{
						grdCalculateRedemption.ContextRow = i;
						ValidateEntry( null, false );
					}
				}
				return true;
			}
			finally
			{
				if (retValue)
					(editedControl as PdfStandard).SavePrevValue( false );
				processingRecalc = false;
			}
		}

		private void UpdateTotals( bool add )
		{
			int noItems = 0;
			decimal intEarned = 0;
			decimal redValue = 0;
			if ( IsValidRow())
			{
				noItems = Convert.ToInt16( dfNoOfItems.UnFormattedValue );
				intEarned = Convert.ToDecimal( dfInterestEarned.UnFormattedValue );
				redValue = Convert.ToDecimal( dfRedemptionValue.UnFormattedValue );
				if ( add )
				{
					dfNoOfItems.UnFormattedValue = noItems + 1;
					dfInterestEarned.UnFormattedValue = intEarned + Convert.ToDecimal( colInterestEarned.UnFormattedValue );
					dfRedemptionValue.UnFormattedValue = redValue + Convert.ToDecimal( colRedemptionValue.UnFormattedValue );
				}
				else
				{
					dfNoOfItems.UnFormattedValue = noItems - 1;
					dfInterestEarned.UnFormattedValue = intEarned - Convert.ToDecimal( colInterestEarned.UnFormattedValue );
					dfRedemptionValue.UnFormattedValue = redValue - Convert.ToDecimal( colRedemptionValue.UnFormattedValue );
					colInterestEarned.UnFormattedValue = null;
					colRedemptionValue.UnFormattedValue = null;
				}
			}
		}

		private string GetXmlTagForControl( PGridColumn column )
		{
			if ( column == colInstrumentType )
				return _pcRedeemValues.InstrumentType.XmlTag;
			else if ( column == colFaceValue )
				return _pcRedeemValues.FaceValue.XmlTag;
			else if ( column == colIssueYear )
				return _pcRedeemValues.IssueYear.XmlTag;
			else if ( column == colIssueMonth )
				return _pcRedeemValues.IssueMonth.XmlTag;
            else if (column == colSerialNo) //#140798
                return _pcRedeemValues.SerialNo.XmlTag;
			return null;
		}

		private bool IsValidRow()
		{
			return ( colInterestEarned.Text != null && colInterestEarned.Text.Length > 0 && colInterestEarned.UnFormattedValue != null && colInterestEarned.UnFormattedValue.ToString() != String.Empty );
		}

		private void EnableDisableVisibleLogic( string origin )
		{
			if ( origin == "grdClicked" )
			{
				pbCopyItem.Enabled = IsValidRow();
				pbDeleteItem.Enabled = this.grdCalculateRedemption.Count > 0;
			}
			else if ( origin == "FormCreate" )
			{
                //Begin #140798
                if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)
                {
                    this.pbReturn.Enabled = true;
                    this.DefaultAction = this.pbReturn;
                    dfYear.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    dfMonth.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    this.ActionClose.Visible = false;
                    this.pbCancel.Visible = true;
                    if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true)
                    {
                        this.lblHelp2.Visible = true;
                    }
                    else
                    {
                        this.grdCalculateRedemption.Size = new System.Drawing.Size(this.grdCalculateRedemption.Size.Width,
                            this.grdCalculateRedemption.Size.Height + this.lblHelp2.Size.Height + 4);
                        this.lblHelp2.Visible = false;
                    }
                }
                else
                {
                    this.ActionClose.Visible = true;
                    this.pbCancel.Visible = false;
                    this.DefaultAction = ActionClose;
                    this.pbReturn.Enabled = false;
                    this.pbCopyItem.Visible = !_isViewOnlyMode;
                    this.pbDeleteItem.Visible = !_isViewOnlyMode;
                    this.pbClear.Visible = !_isViewOnlyMode;
                    this.grdCalculateRedemption.Size = new System.Drawing.Size(this.grdCalculateRedemption.Size.Width,
                        this.grdCalculateRedemption.Size.Height + this.lblHelp2.Size.Height + 4);
                    this.lblHelp2.Visible = false;
                    if (_isViewOnlyMode)
                    {
                        dfYear.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                        dfMonth.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    }
                }
                //pbReturn.Enabled = _parentForm != null;
                //if ( _parentForm != null )
                //{
                //    dfYear.SetObjectStatus( NullabilityState.Default, VisibilityState.Default, EnableState.Disable );
                //    dfMonth.SetObjectStatus( NullabilityState.Default, VisibilityState.Default, EnableState.Disable );
                //}
                //End #140798
			}
		}

		private void MoveBetweenColAndObj(PGridColumn column , bool objToScreen )
		{
			string unformatValue = null;

			//unformatValue = Convert.ToString( column.UnFormattedValue );
			unformatValue = Convert.ToString( column.Text );
			if ( column == colInstrumentType )
			{
				if ( objToScreen )
				{
					if (_pcRedeemValues.InstrumentType.IsNull )
						column.UnFormattedValue = null;
					else
						column.UnFormattedValue = _pcRedeemValues.InstrumentType.Value;
				}
				else
				{

					if ( unformatValue == null || unformatValue == String.Empty )
						_pcRedeemValues.InstrumentType.SetValue(null, EventBehavior.None);	// #6615 - _pcRedeemValues.InstrumentType.SetValueToNull();
					else
						_pcRedeemValues.InstrumentType.Value = unformatValue;
				}
			}
			else if ( column == colFaceValue )
			{
				if ( objToScreen )
				{
					if (_pcRedeemValues.FaceValue.IsNull )
						column.UnFormattedValue = null;
					else
						column.UnFormattedValue = _pcRedeemValues.FaceValue.Value;
				}
				else
				{

					if ( unformatValue == null || unformatValue == String.Empty )
						_pcRedeemValues.FaceValue.SetValue(null, EventBehavior.None);		// #6615 - _pcRedeemValues.FaceValue.SetValueToNull();
					else
						_pcRedeemValues.FaceValue.Value = unformatValue;
				}
			}
		}

        #region #140798
        public bool VerifyDuplicateCheck(ref int duplicateRow, ref string bondInfo)
        {
            int nRow = 0;
            decimal serialNo = 0;//#7741 changed to decimal
            string instrumentType = string.Empty;
            string facevalue = string.Empty;
            string serialNoList = string.Empty;
            string curSerialNo = string.Empty;
            bool dupExists = false;
            duplicateRow = -1;
            bondInfo = string.Empty;

            if (grdCalculateRedemption.Count > 0)
            {
                while (nRow < grdCalculateRedemption.Count)
                {

                    if (grdCalculateRedemption.GetCellValueUnformatted(nRow, colRedemptionValue.ColumnId) != null &&
                        Convert.ToDecimal(grdCalculateRedemption.GetCellValueUnformatted(nRow, colRedemptionValue.ColumnId)) > 0)
                    {
                        if (grdCalculateRedemption.GetCellValueUnformatted(nRow, colSerialNo.ColumnId) != null)
                        {
                            serialNo = Convert.ToDecimal(grdCalculateRedemption.GetCellValueUnformatted(nRow, colSerialNo.ColumnId));//#7741 changed to decimal
                            if (serialNo > 0)
                            {
                                instrumentType = Convert.ToString(grdCalculateRedemption.GetCellValueUnformatted(nRow, colInstrumentType.ColumnId));
                                facevalue = Convert.ToString(grdCalculateRedemption.GetCellValueUnformatted(nRow, colFaceValue.ColumnId));
                                instrumentType = instrumentType.Trim();
                                facevalue = facevalue.Trim();
                                curSerialNo = instrumentType + "-" + facevalue + "-" + Convert.ToString(serialNo).Trim();
                                if (string.IsNullOrEmpty(serialNoList))
                                    serialNoList = curSerialNo;
                                else
                                {
                                    if (serialNoList.IndexOf(curSerialNo) >= 0)
                                    {
                                        dupExists = true;
                                        duplicateRow = nRow;
                                        bondInfo = instrumentType + "~" + facevalue + "~" + Convert.ToString(serialNo).Trim();
                                        break;
                                    }
                                    else
                                    {
                                        serialNoList = serialNoList + "~" + curSerialNo;
                                    }
                                }
                            }
                        }
                    }
                    nRow = nRow + 1;
                }
                if (dupExists)
                    return false;
            }
            return true;
        }

        private bool SaveBonds(ArrayList tempBondsList)
        {
            GbBondRedemptHistory bondObj;
            int nRow = 0;

            if (tempBondsList != null)
                tempBondsList.Clear();

            if (grdCalculateRedemption.Count > 0)
            {
                while (nRow < grdCalculateRedemption.Count)
                {
                    bondObj = new GbBondRedemptHistory();

                    if (grdCalculateRedemption.GetCellValueUnformatted(nRow, colRedemptionValue.ColumnId) != null &&
                        Convert.ToDecimal(grdCalculateRedemption.GetCellValueUnformatted(nRow, colRedemptionValue.ColumnId)) > 0)
                    {
                        bondObj.InstrumentType.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colInstrumentType.ColumnId);
                        bondObj.FaceValue.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colFaceValue.ColumnId);
                        bondObj.IssueMonth.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colIssueMonth.ColumnId);
                        bondObj.IssueYear.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colIssueYear.ColumnId);
                        bondObj.SerialNo.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colSerialNo.ColumnId);
                        bondObj.IntEarned.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colInterestEarned.ColumnId);
                        bondObj.RedemptionValue.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colRedemptionValue.ColumnId);
                        if (_isViewOnlyMode)
                            bondObj.EffectiveDt.ValueObject = grdCalculateRedemption.GetCellValueUnformatted(nRow, colEffectiveDt.ColumnId);
                        else
                            bondObj.EffectiveDt.ValueObject = effectiveDt.Date;
                        bondObj.RedemptionMonth.ValueObject = dfMonth.UnFormattedValue;
                        bondObj.RedemptionYear.ValueObject = dfYear.UnFormattedValue;
                        if (!_pnRimNo.IsNull && _pnRimNo.Value > 0)
                            bondObj.RimNo.Value = _pnRimNo.Value;
                        tempBondsList.Add(bondObj);
                    }
                    nRow = nRow + 1;
                } 
            }
            return true;
        }

        private ArrayList GetListViewObj()
        {
            GbBondRedemptHistory bondHist = new GbBondRedemptHistory();
            if (_pnJournalRimPtid.IsNull)
                return null;
            if (!_pnJournalRimPtid.IsNull && _pnJournalRimPtid.Value > 0)
            {
                if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.Journal ||
                    _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.JournalDisplay)
                    bondHist.JournalPtid.Value = _pnJournalRimPtid.Value;
                else
                    bondHist.RimPtid.Value = _pnJournalRimPtid.Value;
                bondHist.ResponseTypeId = 11;
                return DataService.Instance.GetListViewObjects(bondHist);
            }
            return null;
        }

        private void PopulateSavedBondsList()
        {
            ArrayList bondListViewObj = GetListViewObj();
            if (bondListViewObj == null)
                return;
            if (SavedBondsList != null)
                SavedBondsList.Clear();
            else
                SavedBondsList = new ArrayList();
            SavedBondsList.AddRange(bondListViewObj);
        }

        private void LoadSavingsBonds()
        {
            int rowId = 0;
            decimal intEarned = 0;
            decimal redValue = 0;
            if (SavedBondsList != null && SavedBondsList.Count > 0)
            {
                //Load bonds from the stored arraylist
                foreach (GbBondRedemptHistory bond in SavedBondsList)
                {
                    grdCalculateRedemption.AddNewRow();

                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colInstrumentType.ColumnId, bond.InstrumentType.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colFaceValue.ColumnId, bond.FaceValue.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colIssueMonth.ColumnId, bond.IssueMonth.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colIssueYear.ColumnId, bond.IssueYear.Value);
                    if (!bond.SerialNo.IsNull && bond.SerialNo.Value >= 0)
                        grdCalculateRedemption.SetCellValueUnFormatted(rowId, colSerialNo.ColumnId, bond.SerialNo.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colInterestEarned.ColumnId, bond.IntEarned.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colRedemptionValue.ColumnId, bond.RedemptionValue.Value);
                    grdCalculateRedemption.SetCellValueUnFormatted(rowId, colEffectiveDt.ColumnId, bond.EffectiveDt.Value);
                    rowId++;
                    intEarned += (bond.IntEarned.IsNull ? 0 : bond.IntEarned.Value);
                    redValue += (bond.RedemptionValue.IsNull ? 0 : bond.RedemptionValue.Value);
                }
                dfNoOfItems.UnFormattedValue = rowId;
                dfRedemptionValue.UnFormattedValue = redValue;
                dfInterestEarned.UnFormattedValue = intEarned;
                CurrentBondsList.AddRange(SavedBondsList);
            }
        }

        private void SetColumnsReadOnly()
        {
            int nCol = 0;

            while (nCol < grdCalculateRedemption.Columns.Count)
            {
                (grdCalculateRedemption.Columns[nCol] as PGridColumn).ReadOnly = true;
                nCol += 1;
            }
        }
        #endregion

        #endregion

        private void dfYear_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateRedeemYrMon( dfYear ))
				e.Cancel = true;
		}

		private void dfMonth_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateRedeemYrMon( dfMonth ))
				e.Cancel = true;
		}

		private void grdCalculateRedemption_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			grdCalculateRedemption.ContextRow = e.RowId;
			EnableDisableVisibleLogic( "grdClicked" );
		}

		private void grdCalculateRedemption_EndOfRows(object sender, GridRowArgs e)
		{
            //#140798
            //InsertNewRow(false);
            //if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true &&
            //    _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode &&
            //    colSerialNo.UnFormattedValue == null || (colSerialNo.UnFormattedValue != null &&
            //    string.IsNullOrEmpty(colSerialNo.Text)))
            //{
            //    //14318 - The serial # cannot be left blank, please enter serial #, or \'0\' to continue.
            //    PMessageBox.Show(this, 14318, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
            //    colSerialNo.UnFormattedValue = null;
            //    colSerialNo.Focus();
            //}
            //else
            //    InsertNewRow( false );
            EndOfRowValidation();
		}

		private void colInstrumentType_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateEntry( colInstrumentType, true ))
				e.Cancel = true;
		}

		private void colFaceValue_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!ValidateEntry( colFaceValue, true ))
				e.Cancel = true;
		}

		private void colIssueMonth_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (colIssueMonth.UnFormattedValue != null && (Convert.ToInt32(colIssueMonth.UnFormattedValue) > short.MaxValue || Convert.ToInt32(colIssueMonth.UnFormattedValue) < short.MinValue ))
			{
				//360828 - The number %1! too small or too large for Year.
				PMessageBox.Show(this, 360828, MessageType.Warning, MessageBoxButtons.OK, colIssueMonth.UnFormattedValue.ToString());
				e.Cancel = true;
			}
			else
			{
				if (!ValidateEntry( colIssueMonth, true ))
					e.Cancel = true;
			}
		}

		private void colIssueYear_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (colIssueYear.UnFormattedValue != null && (Convert.ToInt32(colIssueYear.UnFormattedValue) > short.MaxValue || Convert.ToInt32(colIssueYear.UnFormattedValue) < short.MinValue ))
			{
				//360827 - The number %1! too small or too large for Year.
				PMessageBox.Show(this, 360827, MessageType.Warning, MessageBoxButtons.OK, colIssueYear.UnFormattedValue.ToString());
				e.Cancel = true;
			}
			else
			{
				if (!ValidateEntry( colIssueYear, true ))
					e.Cancel = true;
			}
		}

        private void colSerialNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)  //#140798
        {
            if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true &&
                _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode &&
                colSerialNo.UnFormattedValue == null || (colSerialNo.UnFormattedValue != null &&
                string.IsNullOrEmpty(colSerialNo.Text)))
            {
                //14318 - The serial # cannot be left blank, please enter serial #, or \'0\' to continue.
                PMessageBox.Show(this, 14318, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                e.Cancel = true;
            }
            else //#140798
            {
                if (!ValidateEntry(colSerialNo, true))
                    e.Cancel = true;
            }
        }

        private bool EndOfRowValidation()   //#24612
        {
            //#140798
            //InsertNewRow(false);
            if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true &&
                _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode &&
                colSerialNo.UnFormattedValue == null || (colSerialNo.UnFormattedValue != null &&
                string.IsNullOrEmpty(colSerialNo.Text)))
            {
                if (colRedemptionValue.UnFormattedValue != null && Convert.ToDecimal(colRedemptionValue.UnFormattedValue) > 0)
                {
                    //14318 - The serial # cannot be left blank, please enter serial #, or \'0\' to continue.
                    PMessageBox.Show(this, 14318, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                    colSerialNo.UnFormattedValue = null;
                    colSerialNo.Focus();
                    return false;
                }
                else
                    return true;
            }
            else
            {
                InsertNewRow(false);
                return true;
            }
        }


		private void pbReturn_Click(object sender, PActionEventArgs e)
		{
            //#140798
            //if ( _parentForm != null )
            int duplicateRow = -1;
            string bondInfo = string.Empty;
            if (EndOfRowValidation())   //#24612
            {
                if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)
                {
                    if (!VerifyDuplicateCheck(ref duplicateRow, ref bondInfo))
                    {
                        //14324 - The serial # cannot be duplicated, please enter serial #, or "0" to continue. 
                        PMessageBox.Show(this, 14324, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                        grdCalculateRedemption.SelectRow(duplicateRow, true);
                        grdCalculateRedemption.Columns[grdCalculateRedemption.Columns.GetColumnIndex("SerialNo")].Focus();
                        e.IsSuccess = false;
                    }
                    else
                    {                        
                        SaveBonds(SavedBondsList);
                        //Begin #TellerWF
                        if (_invokedByWorkflow)
                        {
                            List<InputParamTranBond> outputItems = new List<InputParamTranBond>();
                            foreach (GbBondRedemptHistory bondObj in SavedBondsList)
                            {
                                //outputItems.Add(new InputParamTranBond
                                //{
                                //    InstrumentType = bondObj.InstrumentType.Value,
                                //    FaceValue = bondObj.FaceValue.Value,
                                //    IssueMonth = bondObj.IssueMonth.Value,
                                //    IssueYear = bondObj.IssueYear.Value,
                                //    SerialNo = bondObj.SerialNo.Value,
                                //    IntEarned = bondObj.IntEarned.Value,
                                //    RedemptionValue = bondObj.RedemptionValue.Value,
                                //    RedemptionMonth = bondObj.RedemptionMonth.Value,
                                //    RedemptionYear = bondObj.RedemptionYear.Value,
                                //    RimNo = bondObj.RimNo.Value,
                                //    EffectiveDt = bondObj.EffectiveDt.Value
                                //});

                                InputParamTranBond paramTranBond = new InputParamTranBond();
                                InputParamTranSet.MoveValueFromBusObjectToObject(bondObj, paramTranBond);
                                outputItems.Add(paramTranBond);
                            }
                            if (Parameters.Contains("OutputJson"))
                            {
                                Parameters["OutputJson"].SetValue(Newtonsoft.Json.JsonConvert.SerializeObject(outputItems));
                            }
                            e.IsSuccess = true;
                            _riseCloseMsg = false;
                            this.IsDirty = false;
                            return;
                        }
                        //End #TellerWF
                        _parentForm.CallParent("RedemptionOut", dfNoOfItems.UnFormattedValue,
                        dfInterestEarned.UnFormattedValue, dfRedemptionValue.UnFormattedValue);
                        _riseCloseMsg = false;
                        Close();
                    }
                }
            }
		}

        void pbCancel_Click(object sender, PActionEventArgs e)  //#140798
        {
            //_riseCloseMsg = false;
            Close();
        }

		private void pbCopyItem_Click(object sender, PActionEventArgs e)
		{
			if ( IsValidRow() )
			{
				//hack 
				colInstrumentType.Focus();
				colInstrumentType.ColumnToField( _pcRedeemValues.InstrumentType );
				colFaceValue.ColumnToField( _pcRedeemValues.FaceValue );
				colIssueYear.ColumnToField( _pcRedeemValues.IssueYear );
				colIssueMonth.ColumnToField( _pcRedeemValues.IssueMonth );
				colRedemptionValue.ColumnToField( _pcRedeemValues.RedemptionValue );
				colInterestEarned.ColumnToField( _pcRedeemValues.IntEarned );
				grdCalculateRedemption.ContextRow = grdCalculateRedemption.Count - 1;
				if ( IsValidRow() )
					InsertNewRow(false);
				colInstrumentType.FieldToColumn( _pcRedeemValues.InstrumentType );
				colFaceValue.FieldToColumn( _pcRedeemValues.FaceValue );
				colIssueYear.FieldToColumn( _pcRedeemValues.IssueYear );
				colIssueMonth.FieldToColumn( _pcRedeemValues.IssueMonth );
				colRedemptionValue.FieldToColumn( _pcRedeemValues.RedemptionValue );
				colInterestEarned.FieldToColumn( _pcRedeemValues.IntEarned );
                if (!_pbIsSerialNoRequired.IsNull && _pbIsSerialNoRequired.Value == true &&
                _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)  //#140798
                    colSerialNo.UnFormattedValue = 0;
				UpdateTotals( true );
				if ( IsValidRow() )
					InsertNewRow(false);
			}
		}

		private void pbDeleteItem_Click(object sender, PActionEventArgs e)
		{
            UpdateTotals(false);  // #74042
            grdCalculateRedemption.RemoveRow( grdCalculateRedemption.ContextRow );
            if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)
                SaveBonds(CurrentBondsList);    //#140798
			//If there are no rows insert one
			if (grdCalculateRedemption.Count == 0)			
				InsertNewRow( true );
			//Set the row select
			grdCalculateRedemption.SelectRow( grdCalculateRedemption.ContextRow);
		}

		private void pbClear_Click(object sender, PActionEventArgs e)
		{
			InsertNewRow( true );
            if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode)
                SaveBonds(CurrentBondsList);    //#140798
		}

		private ReturnType frmCalcRedemption_PInitBeginEvent()
		{
			this.SelectFirstControl = false;
			//colIssueMonth.FormatType = Phoenix.Windows.Forms.UIFieldFormat.UserDefined;
			//colIssueMonth.PhoenixUIControl.InputMask = "#0";
            //Begin #140798
            if (_pnScreenID.IsNull || (!_pnScreenID.IsNull && _pnScreenID.Value == 0))
                _pnScreenID.Value = Phoenix.Shared.Constants.ScreenId.TellerMDI;
            if (_pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TlTranCode ||
                _pnScreenID.Value == Phoenix.Shared.Constants.ScreenId.TellerMDI)
            {
                _isViewOnlyMode = false;
                this.ScreenId = Phoenix.Shared.Constants.ScreenId.Redemption;
            }
            else
            {
                _isViewOnlyMode = true;
                this.ScreenId = Phoenix.Shared.Constants.ScreenId.RedemptionHistory;
            }
            //End #140798
			return ReturnType.Success;
		}

		private void frmCalcRedemption_PInitCompleteEvent()
		{
			//_parentForm = Workspace.ContentWindow.CurrentWindow as PfwStandard;
			PDateTime inputEffectiveDt = new PDateTime("A1");
			_pcRedeemValues = new PcRedeemValues();
			_parentForm = Workspace.ContentWindow.CurrentWindow as PfwStandard;
			if ( _parentForm != null && _parentForm.ScreenId != Phoenix.Shared.Constants.ScreenId.TlTranCode )
				_parentForm = null;
            if (_isViewOnlyMode)    //#140798
            {
                SetColumnsReadOnly();
                _riseCloseMsg = false;
            }
            else
            {
                colInstrumentType.PopulateFromField(_pcRedeemValues.InstrumentType);
                colFaceValue.PopulateFromField(_pcRedeemValues.FaceValue);
                colInstrumentType.AutoDrop = false;
                colFaceValue.AutoDrop = false;
            }
			if ( _parentForm == null )
				effectiveDt = TellerVars.Instance.PostingDt;
			else
			{
				_parentForm.CallParent( "RedemptionIn", inputEffectiveDt );
				if (!inputEffectiveDt.IsNull )
					effectiveDt = inputEffectiveDt.Value;
				else
					effectiveDt = TellerVars.Instance.PostingDt;
			}

            //Begin #140798
            pbReturn.Image = Images.Return;
            pbCancel.Image = Images.Cancel;
            if (_isViewOnlyMode)
            {
                PopulateSavedBondsList();
                if (SavedBondsList != null && SavedBondsList.Count > 0)
                {
                    foreach (GbBondRedemptHistory bond in SavedBondsList)
                    {
                        dfYear.UnFormattedValue = bond.RedemptionYear.Value;
                        dfMonth.UnFormattedValue = bond.RedemptionMonth.Value;
                        break;
                    }
                }
                else
                {
                    dfYear.UnFormattedValue = effectiveDt.Year;
                    dfMonth.UnFormattedValue = effectiveDt.Month;
                }
            }
            else
            {
                dfYear.UnFormattedValue = effectiveDt.Year;
                dfMonth.UnFormattedValue = effectiveDt.Month;
            }
            if (SavedBondsList != null && SavedBondsList.Count > 0)
            {
                LoadSavingsBonds();
                if (!_isViewOnlyMode)
                    InsertNewRow(false, false);
            }
            else
                InsertNewRow(true, false);
            //End #140798
			EnableDisableVisibleLogic( "FormCreate" );
			//Set the Focus to grid
            //#140798
			//grdCalculateRedemption.ContextRow = 0;
            grdCalculateRedemption.ContextRow = grdCalculateRedemption.Count;
			grdCalculateRedemption.Focus();
			colInstrumentType.Focus();
		}

        void frmCalcRedemption_PMdiPrintEvent(object sender, EventArgs e)
        {
            Phoenix.Windows.Client.WindowHelper winHelper = new WindowHelper();
            SaveBonds(CurrentBondsList);
            //_busObjGbBondHist.GenerateReport(CurrentBondsList);
            winHelper.GenerateReport(CurrentBondsList);
        }

        #region ProcessCmdKey, To have new entry
        //		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //		{
        //			if (grdCalculateRedemption.Focus() && keyData == Keys.Insert )
        //			{				
        //				InsertNewRow( true);
        //				return true;
        //			}
        //			return base.ProcessCmdKey( ref msg, keyData );
        //		}
        #endregion ProcessCmdKey

        #region OnActionClose

        //override on

        //Begin #TellerWF
        public override bool OnActionSave(bool isAddNext)
        {
            if (_invokedByWorkflow)
            {
                var e = new PActionEventArgs();
                pbReturn_Click(null, e);
                return e.IsSuccess;
            }

            return base.OnActionSave( isAddNext );
        }
        //End #TellerWF
        public override bool OnActionClose()
		{
			bool closeWin = true;
			if (_riseCloseMsg)
			{
				for (int i = 0; i < grdCalculateRedemption.Count; i++)
				{
					if (grdCalculateRedemption.Items[i].SubItems[5].Text != null && grdCalculateRedemption.Items[i].SubItems[5].Text.Trim().Length > 0)
					{
						//360833 - You will lose all information entered when you Close the window.  Are you sure you want to Close the window?
						DialogResult result = PMessageBox.Show(this, 360833, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty);
						if (result == DialogResult.No)
						{
							closeWin = false;
							break;
						}
						else if (result == DialogResult.Yes)
							break;
					}
				}
			}
			if (!closeWin)
			{
				grdCalculateRedemption.Focus();
				return false;
			}
			else
				return base.OnActionClose ();
		}
		#endregion OnActionClose
		//
	}
}
