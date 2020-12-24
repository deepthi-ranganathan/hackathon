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
// File Name: frmTlEndorseItems.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//6/26/2010		1		rpoddar	    #79510, #09368 - Created.
//12/14/2010    2       Deiland     WI#11294/#11295 - New Branch fields for Endorsement Printing
//12/15/2010    3       DEiland     WI#11632/#11633 - New Branch Sequence Number For Endorsement Printing
//03/11/2011    4       sanwar      WI#13150/#13151 - Date range serch is nor working correctly when Printed radio is checked
//8/3/2013		5		apitava		#157637 uses new xfsprinter
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using GlacialComponents.Controls;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Windows.Client;
using GlacialComponents.Controls.Common;
using Phoenix.BusObj.Teller;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Xfs;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Admin.RIM;


namespace Phoenix.Client.Teller
{
	/// <summary>
	/// Summary description for frmTlEndorseItems.
	/// </summary>
	public class frmTlEndorseItems : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
        private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGridColumnComboBox colCheckType;
		private Phoenix.Windows.Forms.PGridColumnComboBox colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colGuestAcctNo;
        private Phoenix.Windows.Forms.PGridColumn colCheckNo;
        private Phoenix.Windows.Forms.PGridColumn colAmount;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
        private Phoenix.Windows.Forms.PGrid gridEndorseItems;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbDisplayCriteria;
        private Phoenix.Windows.Forms.PAction pbEndorse;
        private Phoenix.Windows.Forms.PAction pbEndorseAll;
        private Phoenix.Windows.Forms.PGridColumn colItemNo;
        private PRadioButtonStandard rbPrinted;
        private PRadioButtonStandard rbNotPrinted;
        private PGridColumn colTeller;
        private PGridColumn colGuestMbrName;
        private PGridColumn colGuestCUName;
        private PGridColumn colBranchNo;
        private PGridColumn colDrawerNo;
        private PGridColumn colSequenceNo;
        private PGridColumn colPtid;

        #region Initialize
		private TellerVars _tellerVars = TellerVars.Instance;
		//
		private PSmallInt branchNo = new PSmallInt("BranchNo");
		private PSmallInt drawerNo = new PSmallInt("DrawerNo");
        private PDateTime fromCreateDt = new PDateTime("fromCreateDt");
        private PDateTime toCreateDt = new PDateTime("toCreateDt");
		//
		private PSmallInt _reprintFormId;
		private string _reprintInfo = "";
		private string _checkItemInfo = "";
		private string _tempChkAmount = "";
		private string _reprintTextQrp = "";
		private string _partialPrintString = "";
		private string _wosaServiceName = "";
		//private string _logicalService = "";
		private string _formName = "";
		private string _mediaName = "";
        //private int	_checkInfoRimNo = 0;
		private short _checkPrintNo = 0;
		private PrintInfo _wosaPrintInfo = new PrintInfo();
		private XfsPrinter _xfsPrinter;
		private DialogResult dialogResult = DialogResult.None;
        private PDecimal _noCopies;
        private PString _printerService;
        private PGridColumn colRecordSource;
        private PGridColumn colAcctNo;
        private PGridColumn colRoutingNo;
        private PGroupBoxStandard gbItemTotals;
        private PDfDisplay dfNoOfItems;
        private PLabelStandard lblNoOfItems;
        private PDfDisplay dfItemTotals;
        private PLabelStandard lblItemTotals;
        private PGridColumn colEffectiveDt;
        private PAction pbDisplay;
        private PGridColumn colJrnlPtid;
        private PGridColumn colTellerNo;
        private PGridColumn colCreateDt;
        private PGridColumn colGuestCURoutingNo;
        private PComboBoxStandard cmbBranch;
        private PLabelStandard lblBranch;
        private PComboBoxStandard cmbDrawers;
        private PLabelStandard lblDrawerTeller;
        private PdfStandard dfTo;
        private PLabelStandard lblToEnteredDt;
        private PdfStandard dfFrom;
        private PLabelStandard lblFromEnteredDt;
        private PAction pbSearch;
        TlItemCapture _busObjCapturedItems = null;
        private PCheckBoxStandard cbDate;
        TlJournal _busObjTlJournal = null;      // #10117
        TlJournalAddlInfo _busObjTlJournalAddlInfo = null;// WI#11632
        #endregion


		public frmTlEndorseItems()
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
            this.colCheckType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colGuestAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridEndorseItems = new Phoenix.Windows.Forms.PGrid();
            this.colTeller = new Phoenix.Windows.Forms.PGridColumn();
            this.colGuestMbrName = new Phoenix.Windows.Forms.PGridColumn();
            this.colGuestCUName = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRoutingNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSequenceNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colRecordSource = new Phoenix.Windows.Forms.PGridColumn();
            this.colEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colJrnlPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colTellerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colGuestCURoutingNo = new Phoenix.Windows.Forms.PGridColumn();
            this.pbEndorse = new Phoenix.Windows.Forms.PAction();
            this.pbEndorseAll = new Phoenix.Windows.Forms.PAction();
            this.gbDisplayCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblFromEnteredDt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTo = new Phoenix.Windows.Forms.PdfStandard();
            this.lblToEnteredDt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfFrom = new Phoenix.Windows.Forms.PdfStandard();
            this.cmbDrawers = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblDrawerTeller = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbBranch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.rbPrinted = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.lblBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbNotPrinted = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.gbItemTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfNoOfItems = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblNoOfItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfItemTotals = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblItemTotals = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbDisplay = new Phoenix.Windows.Forms.PAction();
            this.pbSearch = new Phoenix.Windows.Forms.PAction();
            this.cbDate = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.pGroupBoxStandard1.SuspendLayout();
            this.gbDisplayCriteria.SuspendLayout();
            this.gbItemTotals.SuspendLayout();
            this.SuspendLayout();
            //
            // ActionManager
            //
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbEndorse,
            this.pbEndorseAll,
            this.pbDisplay,
            this.pbSearch});
            //
            // colCheckType
            //
            this.colCheckType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colCheckType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.colCheckType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colCheckType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colCheckType.PhoenixUIControl.ObjectId = 6;
            this.colCheckType.PhoenixUIControl.XmlTag = "CheckType";
            this.colCheckType.Title = "Check Type";
            this.colCheckType.Width = 125;
            //
            // colAcctType
            //
            this.colAcctType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctType.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.colAcctType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.colAcctType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAcctType.PhoenixUIControl.ObjectId = 9;
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "Acct Type";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 47;
            //
            // colGuestAcctNo
            //
            this.colGuestAcctNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colGuestAcctNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colGuestAcctNo.PhoenixUIControl.ObjectId = 7;
            this.colGuestAcctNo.PhoenixUIControl.XmlTag = "GuestAcctNo";
            this.colGuestAcctNo.Title = "Guest Account Number";
            this.colGuestAcctNo.Width = 130;
            //
            // colCheckNo
            //
            this.colCheckNo.PhoenixUIControl.ObjectId = 14;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.ShowNullAsEmpty = true;
            this.colCheckNo.Title = "Check No";
            this.colCheckNo.Width = 58;
            //
            // colAmount
            //
            this.colAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.ObjectId = 5;
            this.colAmount.PhoenixUIControl.XmlTag = "Amount";
            this.colAmount.ShowNullAsEmpty = true;
            this.colAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colAmount.Title = "Amount";
            this.colAmount.Width = 80;
            //
            // pGroupBoxStandard1
            //
            this.pGroupBoxStandard1.Controls.Add(this.gridEndorseItems);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 72);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(684, 328);
            this.pGroupBoxStandard1.TabIndex = 0;
            this.pGroupBoxStandard1.TabStop = false;
            //
            // gridEndorseItems
            //
            this.gridEndorseItems.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colTeller,
            this.colAmount,
            this.colGuestAcctNo,
            this.colGuestMbrName,
            this.colGuestCUName,
            this.colCheckNo,
            this.colAcctNo,
            this.colRoutingNo,
            this.colCheckType,
            this.colItemNo,
            this.colAcctType,
            this.colBranchNo,
            this.colDrawerNo,
            this.colSequenceNo,
            this.colPtid,
            this.colRecordSource,
            this.colEffectiveDt,
            this.colJrnlPtid,
            this.colTellerNo,
            this.colCreateDt,
            this.colGuestCURoutingNo});
            this.gridEndorseItems.LinesInHeader = 2;
            this.gridEndorseItems.Location = new System.Drawing.Point(4, 8);
            this.gridEndorseItems.Name = "gridEndorseItems";
            this.gridEndorseItems.Size = new System.Drawing.Size(676, 316);
            this.gridEndorseItems.TabIndex = 0;
            this.gridEndorseItems.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridEndorseItems_AfterPopulate);
            this.gridEndorseItems.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridEndorseItems_BeforePopulate);
            //
            // colTeller
            //
            this.colTeller.PhoenixUIControl.ObjectId = 4;
            this.colTeller.PhoenixUIControl.XmlTag = "EmplName";
            this.colTeller.Title = "Teller";
            this.colTeller.Width = 144;
            //
            // colGuestMbrName
            //
            this.colGuestMbrName.PhoenixUIControl.ObjectId = 8;
            this.colGuestMbrName.PhoenixUIControl.XmlTag = "GuestMbrName";
            this.colGuestMbrName.Title = "Guest Mbr Name";
            this.colGuestMbrName.Width = 169;
            //
            // colGuestCUName
            //
            this.colGuestCUName.PhoenixUIControl.ObjectId = 9;
            this.colGuestCUName.PhoenixUIControl.XmlTag = "GuestCUName";
            this.colGuestCUName.Title = "Guest CU Name";
            this.colGuestCUName.Width = 169;
            //
            // colAcctNo
            //
            this.colAcctNo.PhoenixUIControl.ObjectId = 12;
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.Title = "Account Number";
            //
            // colRoutingNo
            //
            this.colRoutingNo.PhoenixUIControl.ObjectId = 13;
            this.colRoutingNo.PhoenixUIControl.XmlTag = "RoutingNo";
            this.colRoutingNo.Title = "Routing Number";
            //
            // colItemNo
            //
            this.colItemNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colItemNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colItemNo.PhoenixUIControl.XmlTag = "ItemNo";
            this.colItemNo.Title = "Item No";
            this.colItemNo.Visible = false;
            this.colItemNo.Width = 0;
            //
            // colBranchNo
            //
            this.colBranchNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo.Title = "Column";
            this.colBranchNo.Visible = false;
            this.colBranchNo.Width = 0;
            //
            // colDrawerNo
            //
            this.colDrawerNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "Column";
            this.colDrawerNo.Visible = false;
            this.colDrawerNo.Width = 0;
            //
            // colSequenceNo
            //
            this.colSequenceNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSequenceNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colSequenceNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.colSequenceNo.Title = "Column";
            this.colSequenceNo.Visible = false;
            this.colSequenceNo.Width = 0;
            //
            // colPtid
            //
            this.colPtid.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPtid.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPtid.PhoenixUIControl.XmlTag = "Ptid";
            this.colPtid.Title = "Column";
            this.colPtid.Visible = false;
            this.colPtid.Width = 0;
            //
            // colRecordSource
            //
            this.colRecordSource.PhoenixUIControl.XmlTag = "RecordSource";
            this.colRecordSource.Title = "Column";
            this.colRecordSource.Visible = false;
            this.colRecordSource.Width = 0;
            //
            // colEffectiveDt
            //
            this.colEffectiveDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.colEffectiveDt.Title = "Column";
            this.colEffectiveDt.Visible = false;
            this.colEffectiveDt.Width = 0;
            //
            // colJrnlPtid
            //
            this.colJrnlPtid.PhoenixUIControl.XmlTag = "JournalPtid";
            this.colJrnlPtid.Title = "Column";
            this.colJrnlPtid.Visible = false;
            //
            // colTellerNo
            //
            this.colTellerNo.PhoenixUIControl.XmlTag = "TellerNo";
            this.colTellerNo.Title = "Column";
            this.colTellerNo.Visible = false;
            //
            // colCreateDt
            //
            this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.colCreateDt.Title = "Column";
            this.colCreateDt.Visible = false;
            //
            // colGuestCURoutingNo
            //
            this.colGuestCURoutingNo.PhoenixUIControl.XmlTag = "GuestCURoutingNo";
            this.colGuestCURoutingNo.Title = "Column";
            this.colGuestCURoutingNo.Visible = false;
            //
            // pbEndorse
            //
            this.pbEndorse.LongText = "E&ndorse";
            this.pbEndorse.ObjectId = 10;
            this.pbEndorse.ShortText = "E&ndorse";
            this.pbEndorse.Tag = null;
            this.pbEndorse.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEndorse_Click);
            //
            // pbEndorseAll
            //
            this.pbEndorseAll.LongText = "Endorse &All";
            this.pbEndorseAll.ObjectId = 11;
            this.pbEndorseAll.ShortText = "Endorse &All";
            this.pbEndorseAll.Tag = null;
            this.pbEndorseAll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEndorseAll_Click);
            //
            // gbDisplayCriteria
            //
            this.gbDisplayCriteria.Controls.Add(this.cbDate);
            this.gbDisplayCriteria.Controls.Add(this.lblFromEnteredDt);
            this.gbDisplayCriteria.Controls.Add(this.dfTo);
            this.gbDisplayCriteria.Controls.Add(this.lblToEnteredDt);
            this.gbDisplayCriteria.Controls.Add(this.dfFrom);
            this.gbDisplayCriteria.Controls.Add(this.cmbDrawers);
            this.gbDisplayCriteria.Controls.Add(this.lblDrawerTeller);
            this.gbDisplayCriteria.Controls.Add(this.cmbBranch);
            this.gbDisplayCriteria.Controls.Add(this.rbPrinted);
            this.gbDisplayCriteria.Controls.Add(this.lblBranch);
            this.gbDisplayCriteria.Controls.Add(this.rbNotPrinted);
            this.gbDisplayCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbDisplayCriteria.Name = "gbDisplayCriteria";
            this.gbDisplayCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbDisplayCriteria.Size = new System.Drawing.Size(684, 72);
            this.gbDisplayCriteria.TabIndex = 0;
            this.gbDisplayCriteria.TabStop = false;
            this.gbDisplayCriteria.Text = "Display Criteria";
            //
            // lblFromEnteredDt
            //
            this.lblFromEnteredDt.AutoEllipsis = true;
            this.lblFromEnteredDt.Location = new System.Drawing.Point(196, 44);
            this.lblFromEnteredDt.Name = "lblFromEnteredDt";
            this.lblFromEnteredDt.PhoenixUIControl.ObjectId = 22;
            this.lblFromEnteredDt.Size = new System.Drawing.Size(36, 20);
            this.lblFromEnteredDt.TabIndex = 7;
            this.lblFromEnteredDt.Text = "From:";
            //
            // dfTo
            //
            this.dfTo.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTo.Location = new System.Drawing.Point(388, 44);
            this.dfTo.Name = "dfTo";
            this.dfTo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfTo.PhoenixUIControl.InputMask = "";
            this.dfTo.PhoenixUIControl.ObjectId = 23;
            this.dfTo.Size = new System.Drawing.Size(76, 20);
            this.dfTo.TabIndex = 10;
            //
            // lblToEnteredDt
            //
            this.lblToEnteredDt.AutoEllipsis = true;
            this.lblToEnteredDt.Location = new System.Drawing.Point(340, 44);
            this.lblToEnteredDt.Name = "lblToEnteredDt";
            this.lblToEnteredDt.PhoenixUIControl.ObjectId = 23;
            this.lblToEnteredDt.Size = new System.Drawing.Size(28, 20);
            this.lblToEnteredDt.TabIndex = 9;
            this.lblToEnteredDt.Text = "To:";
            //
            // dfFrom
            //
            this.dfFrom.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFrom.Location = new System.Drawing.Point(244, 44);
            this.dfFrom.Name = "dfFrom";
            this.dfFrom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFrom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfFrom.PhoenixUIControl.InputMask = "";
            this.dfFrom.PhoenixUIControl.ObjectId = 22;
            this.dfFrom.Size = new System.Drawing.Size(76, 20);
            this.dfFrom.TabIndex = 8;
            //
            // cmbDrawers
            //
            this.cmbDrawers.Location = new System.Drawing.Point(532, 16);
            this.cmbDrawers.Name = "cmbDrawers";
            this.cmbDrawers.PhoenixUIControl.ObjectId = 20;
            this.cmbDrawers.PhoenixUIControl.XmlTag = "DrawerNo";
            this.cmbDrawers.Size = new System.Drawing.Size(148, 21);
            this.cmbDrawers.TabIndex = 5;
            this.cmbDrawers.Value = null;
            //
            // lblDrawerTeller
            //
            this.lblDrawerTeller.AutoEllipsis = true;
            this.lblDrawerTeller.Location = new System.Drawing.Point(484, 16);
            this.lblDrawerTeller.Name = "lblDrawerTeller";
            this.lblDrawerTeller.PhoenixUIControl.ObjectId = 20;
            this.lblDrawerTeller.Size = new System.Drawing.Size(44, 20);
            this.lblDrawerTeller.TabIndex = 4;
            this.lblDrawerTeller.Text = "Drawer:";
            //
            // cmbBranch
            //
            this.cmbBranch.Location = new System.Drawing.Point(244, 16);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.PhoenixUIControl.ObjectId = 19;
            this.cmbBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.cmbBranch.Size = new System.Drawing.Size(220, 21);
            this.cmbBranch.TabIndex = 3;
            this.cmbBranch.Value = null;
            this.cmbBranch.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbBranch_PhoenixUISelectedIndexChangedEvent);
            //
            // rbPrinted
            //
            this.rbPrinted.AutoSize = true;
            this.rbPrinted.Description = null;
            this.rbPrinted.Location = new System.Drawing.Point(92, 16);
            this.rbPrinted.Name = "rbPrinted";
            this.rbPrinted.PhoenixUIControl.ObjectId = 3;
            this.rbPrinted.Size = new System.Drawing.Size(64, 18);
            this.rbPrinted.TabIndex = 1;
            this.rbPrinted.Text = "Printed";
            this.rbPrinted.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbPrinted_PhoenixUICheckedChangedEvent);
            //
            // lblBranch
            //
            this.lblBranch.AutoEllipsis = true;
            this.lblBranch.Location = new System.Drawing.Point(196, 16);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.PhoenixUIControl.ObjectId = 19;
            this.lblBranch.Size = new System.Drawing.Size(44, 20);
            this.lblBranch.TabIndex = 2;
            this.lblBranch.Text = "Branch:";
            //
            // rbNotPrinted
            //
            this.rbNotPrinted.AutoSize = true;
            this.rbNotPrinted.Description = null;
            this.rbNotPrinted.Location = new System.Drawing.Point(8, 16);
            this.rbNotPrinted.Name = "rbNotPrinted";
            this.rbNotPrinted.PhoenixUIControl.ObjectId = 2;
            this.rbNotPrinted.Size = new System.Drawing.Size(84, 18);
            this.rbNotPrinted.TabIndex = 0;
            this.rbNotPrinted.Text = "Not-Printed";
            this.rbNotPrinted.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbNotPrinted_PhoenixUICheckedChangedEvent);
            //
            // gbItemTotals
            //
            this.gbItemTotals.Controls.Add(this.dfNoOfItems);
            this.gbItemTotals.Controls.Add(this.lblNoOfItems);
            this.gbItemTotals.Controls.Add(this.dfItemTotals);
            this.gbItemTotals.Controls.Add(this.lblItemTotals);
            this.gbItemTotals.Location = new System.Drawing.Point(4, 400);
            this.gbItemTotals.Name = "gbItemTotals";
            this.gbItemTotals.PhoenixUIControl.ObjectId = 28;
            this.gbItemTotals.Size = new System.Drawing.Size(684, 44);
            this.gbItemTotals.TabIndex = 3;
            this.gbItemTotals.TabStop = false;
            this.gbItemTotals.Text = "Item Totals";
            //
            // dfNoOfItems
            //
            this.dfNoOfItems.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoOfItems.Location = new System.Drawing.Point(616, 16);
            this.dfNoOfItems.Multiline = true;
            this.dfNoOfItems.Name = "dfNoOfItems";
            this.dfNoOfItems.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfNoOfItems.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfNoOfItems.PhoenixUIControl.ObjectId = 16;
            this.dfNoOfItems.Size = new System.Drawing.Size(60, 20);
            this.dfNoOfItems.TabIndex = 8;
            //
            // lblNoOfItems
            //
            this.lblNoOfItems.AutoEllipsis = true;
            this.lblNoOfItems.Location = new System.Drawing.Point(548, 16);
            this.lblNoOfItems.Name = "lblNoOfItems";
            this.lblNoOfItems.PhoenixUIControl.ObjectId = 16;
            this.lblNoOfItems.Size = new System.Drawing.Size(64, 20);
            this.lblNoOfItems.TabIndex = 7;
            this.lblNoOfItems.Text = "# of Items:";
            //
            // dfItemTotals
            //
            this.dfItemTotals.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfItemTotals.Location = new System.Drawing.Point(76, 16);
            this.dfItemTotals.Multiline = true;
            this.dfItemTotals.Name = "dfItemTotals";
            this.dfItemTotals.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfItemTotals.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfItemTotals.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfItemTotals.PhoenixUIControl.ObjectId = 15;
            this.dfItemTotals.Size = new System.Drawing.Size(132, 20);
            this.dfItemTotals.TabIndex = 2;
            //
            // lblItemTotals
            //
            this.lblItemTotals.AutoEllipsis = true;
            this.lblItemTotals.Location = new System.Drawing.Point(4, 16);
            this.lblItemTotals.Name = "lblItemTotals";
            this.lblItemTotals.PhoenixUIControl.ObjectId = 15;
            this.lblItemTotals.Size = new System.Drawing.Size(68, 20);
            this.lblItemTotals.TabIndex = 1;
            this.lblItemTotals.Text = "Item Totals:";
            //
            // pbDisplay
            //
            this.pbDisplay.LongText = "&Display...";
            this.pbDisplay.ObjectId = 17;
            this.pbDisplay.ShortText = "&Display...";
            this.pbDisplay.Tag = null;
            this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
            //
            // pbSearch
            //
            this.pbSearch.LongText = "&Search";
            this.pbSearch.ObjectId = 18;
            this.pbSearch.ShortText = "&Search";
            this.pbSearch.StatusText = "";
            this.pbSearch.Tag = null;
            this.pbSearch.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSearch_Click);
            //
            // cbDate
            //
            this.cbDate.Checked = true;
            this.cbDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDate.Location = new System.Drawing.Point(8, 44);
            this.cbDate.Name = "cbDate";
            this.cbDate.PhoenixUIControl.ObjectId = 21;
            this.cbDate.Size = new System.Drawing.Size(120, 20);
            this.cbDate.TabIndex = 17;
            this.cbDate.Text = "Entered Date";
            this.cbDate.Value = null;
            this.cbDate.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.cbDate_PhoenixUICheckedChangedEvent);
            //
            // frmTlEndorseItems
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbItemTotals);
            this.Controls.Add(this.gbDisplayCriteria);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.Name = "frmTlEndorseItems";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlEndorseItems_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlEndorseItems_PInitBeginEvent);
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.gbDisplayCriteria.ResumeLayout(false);
            this.gbDisplayCriteria.PerformLayout();
            this.gbItemTotals.ResumeLayout(false);
            this.gbItemTotals.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 2)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);

			}
            if (paramList.Length >= 4)
            {
                fromCreateDt.Value = Convert.ToDateTime(paramList[2]);
                toCreateDt.Value = Convert.ToDateTime(paramList[3]);

            }

			base.InitParameters (paramList);
		}

		#endregion


		#region item events
		private ReturnType frmTlEndorseItems_PInitBeginEvent()
		{
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.TlEndorseItems;

            // The following will take care of check type decoding in view mode
            colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "C"), CoreService.Translation.GetUserMessageX(360589));
            colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "O"), CoreService.Translation.GetUserMessageX(360590));
            colCheckType.Append(CoreService.Translation.GetListItemX(ListId.CheckType, "T"), CoreService.Translation.GetUserMessageX(360591));

            rbNotPrinted.Checked = true;

            //Begin #10117
            rbNotPrinted_PhoenixUICheckedChangedEvent(null, null);
            cmbBranch.PhoenixUIControl.IsNullable = NullabilityState.NotNull;
            cmbDrawers.PhoenixUIControl.IsNullable = NullabilityState.NotNull;
            _busObjTlJournal = new TlJournal();
            Helper.PopulateCombo(cmbBranch, _busObjTlJournal, _busObjTlJournal.BranchNo);
            cmbBranch.Append(-1, GlobalVars.Instance.ML.All);
            cmbBranch.SetValue(branchNo.Value);
            cmbBranch_PhoenixUISelectedIndexChangedEvent(null, null);
            cmbDrawers.SetValue(drawerNo.Value);
            this.DefaultAction = pbSearch;
            fromCreateDt.SetValue(null);    // ignore any date filters passed from journal window
            toCreateDt.SetValue(null);
            //End #10117

            return ReturnType.Success;
		}

		private void frmTlEndorseItems_PInitCompleteEvent()
		{
            //
		}

		private void gridEndorseItems_BeforePopulate(object sender, GridPopulateArgs e)
		{
            _busObjCapturedItems = new TlItemCapture();

            //Begin #10117
            if (cbDate.Checked)
            {
                if (rbPrinted.Checked)
                {
                    if (dfFrom.Value == null || Convert.ToDateTime(dfFrom.Value) == DateTime.MinValue)
                    {
                        dfFrom.SetValue(DateTime.Now.AddDays(-5));
                    }
                    if (dfTo.Value == null || Convert.ToDateTime(dfTo.Value) == DateTime.MinValue)
                    {
                        dfTo.SetValue(DateTime.Now);
                    }
                }
                if (dfTo.Value != null && Convert.ToDateTime(dfTo.Value) != DateTime.MinValue &&
                    dfFrom.Value != null && Convert.ToDateTime(dfFrom.Value) != DateTime.MinValue)
                {
                    if (Convert.ToDateTime(dfTo.Value) < Convert.ToDateTime(dfFrom.Value))
                    {
                        //13291 - "From" date must be less than or equal to the "To" date.
                        PMessageBox.Show(this, 13291, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                        dfFrom.Focus();
                        return;
                    }
                    if (rbPrinted.Checked)
                    {
                        if (Convert.ToDateTime(dfTo.Value).AddDays(-5) > Convert.ToDateTime(dfFrom.Value))
                        {
                            //13292 - Printed Items can be viewed up to 5 days at a time. Please adjust your date filter.
                            PMessageBox.Show(this, 13292, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                            dfFrom.Focus();
                            return;
                        }
                    }
                }
                //Begin #13150
                //fromCreateDt.SetValue(dfFrom.Value);
                //toCreateDt.SetValue(dfTo.Value);
                fromCreateDt.SetValue( Convert.ToDateTime(dfFrom.Value).Date);
                toCreateDt.SetValue(Convert.ToDateTime(dfTo.Value).Date.AddDays(1).AddSeconds(-1));
                //End #13150
            }
            else
            {
                fromCreateDt.SetValue(null);
                toCreateDt.SetValue(null);
            }
            branchNo.SetValue(cmbBranch.CodeValue);
            drawerNo.SetValue(cmbDrawers.CodeValue);
            //End #10117

            if ( branchNo.Value >= 0 )
                _busObjCapturedItems.BranchNo.Value = branchNo.Value;
            if (drawerNo.Value >= 0)
                _busObjCapturedItems.DrawerNo.Value = drawerNo.Value;

            gridEndorseItems.Filters.Clear();

            if ( !fromCreateDt.IsNull )
                gridEndorseItems.Filters.Add( new Filter(_busObjCapturedItems.CreateDt.XmlTag, fromCreateDt.Value, FilterOperator.GE ));

            if (!toCreateDt.IsNull)
                gridEndorseItems.Filters.Add(new Filter(_busObjCapturedItems.CreateDt.XmlTag, toCreateDt.Value, FilterOperator.LE));

            if (rbNotPrinted.Checked)
                _busObjCapturedItems.EndorseStatus.Value = "N";
            else
            {
                _busObjCapturedItems.EndorseStatus.Value = "P";

                // if not date filter has been passed then restric the list of endorsed items to just five days back
                if ( fromCreateDt.IsNull && toCreateDt.IsNull )
                    gridEndorseItems.Filters.Add(new Filter(_busObjCapturedItems.CreateDt.XmlTag, DateTime.Now.AddDays(-5), FilterOperator.GE));
            }

            _busObjCapturedItems.OutputTypeId.Value = 11;

            gridEndorseItems.ListViewObject = _busObjCapturedItems;
		}

		private void gridEndorseItems_AfterPopulate(object sender, GridPopulateArgs e)
		{
			if (gridEndorseItems.Count > 0)
			{
				gridEndorseItems.SelectRow(0,true);
			}
            CalcTotal();
            EnableDisableVisibleLogic("AfterPopulate");
		}

        private void pbEndorse_Click(object sender, PActionEventArgs e)
		{
			if (LoadPrintObject(false))
			{
				LoadItemPrintInfo();
				if ( HandlePrinting() && rbNotPrinted.Checked )
                    CallXMThruCDS("UpdateEndorseStatus");
			}
            gridEndorseItems.PopulateTable();
		}

		private void pbEndorseAll_Click(object sender, PActionEventArgs e)
		{
			bool rePrint = false;
			bool skipChecks = false;
			//
			if (LoadPrintObject(true))
			{
				int nRow = 0;
				if (gridEndorseItems.Count > 0)
				{
					while (nRow < gridEndorseItems.Count)
					{
                        gridEndorseItems.SelectRow(nRow, false);
						//
						_tempChkAmount = colAmount.MakeFormattedValue(colAmount.UnFormattedValue);
						_tempChkAmount = _tempChkAmount.PadLeft(14, '*');
						_tempChkAmount = _tempChkAmount.Replace("*", " ");
                        //_checkItemInfo = colItemNo.UnFormattedValue.ToString() + " - " + colCheckType.Text + _tempChkAmount;
                        _checkItemInfo = colCheckType.Text + _tempChkAmount;
                        //_checkPrintNo = Convert.ToInt16(colItemNo.UnFormattedValue);
						//
						LoadItemPrintInfo();
						//
						CallOtherForms("PrintForms");
						//
						rePrint = dialogResult == DialogResult.Retry;
						skipChecks = dialogResult == DialogResult.Abort;

                        if (dialogResult == DialogResult.OK && rbNotPrinted.Checked )
                            CallXMThruCDS("UpdateEndorseStatus");
						//
						if (rePrint)
							nRow = nRow - 1;
						else
							nRow = nRow + 1;
						//
						if (skipChecks)
							break;
					}
				}
			}

            gridEndorseItems.PopulateTable();
		}

		#endregion

		#region private methods


		private void EnableDisableVisibleLogic( string callerInfo )
		{
            if (callerInfo == "AfterPopulate")
            {
                pbEndorse.Enabled = gridEndorseItems.Items.Count > 0;
                pbEndorseAll.Enabled = gridEndorseItems.Items.Count > 1;
            }
            //Begin #10117
            else if (callerInfo == "DateClicked")
            {
                if (cbDate.Checked)
                {
                    dfFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    lblFromEnteredDt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    dfTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    lblToEnteredDt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                }
                else
                {
                    dfFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    dfTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    lblFromEnteredDt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    lblToEnteredDt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                }
            }
            else if (callerInfo == "NonPrintedClicked")
            {
                cbDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
            }
            else if (callerInfo == "PrintedClicked")
            {
                cbDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
            }
            //End #10117
		}


		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;
			PfwStandard tempDlg = null;

			try
			{
				if ( origin == "AdHocReceipt" )
				{
					_reprintFormId = new PSmallInt("FormId");
                    _noCopies = new PDecimal("NoCopies");
                    _noCopies.Value = 1;
                    _printerService = new PString("PrinterService");
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt" );
					tempDlg.InitParameters(_reprintInfo, _reprintFormId, this.ScreenId, _checkItemInfo, null, _noCopies, _printerService);

				}
				else if ( origin == "PrintForms" )
				{
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgPrintForms" );
					tempDlg.InitParameters( _reprintFormId.Value, _checkPrintNo, -1, _wosaPrintInfo,
						_checkItemInfo);
				}

                else if (origin == "DisplayClick")
                {
                    tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournalDisplay");
                    tempWin.InitParameters(colBranchNo.UnFormattedValue, colDrawerNo.UnFormattedValue,
                        colEffectiveDt.UnFormattedValue, colJrnlPtid.UnFormattedValue );
                }

				if ( tempWin != null )
				{
					tempWin.Workspace = this.Workspace;
					tempWin.Show();
				}

				else if ( tempDlg != null )
				{
					dialogResult = tempDlg.ShowDialog(this);
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
				return;
			}

		}


		private void LoadItemPrintInfo()
		{
			#region add check print fields info
            AdGbBank adGbBank = GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBank] as AdGbBank;
            #region WI#11294
            AdGbBranch adGbBranch = GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBranch] as AdGbBranch;
            #endregion //WI#11294
            #region WI#11632
            // Get Tl Journal Additional Information
            _busObjTlJournalAddlInfo = new TlJournalAddlInfo();
            _busObjTlJournalAddlInfo.JournalPtid.Value = Convert.ToDecimal(this.colJrnlPtid.Text);
            _busObjTlJournalAddlInfo.ActionType = XmActionType.Select;
            CallXMThruCDS("GetTlJournalAddlInfo");
            _wosaPrintInfo.SbSeqNo = null;      // reset Value in case no value found
            if (_busObjTlJournalAddlInfo != null)
            {
                if (_busObjTlJournalAddlInfo.OtherInfo.Contains("SBSequenceNo"))
                {
                    _wosaPrintInfo.SbSeqNo = Convert.ToString(_busObjTlJournalAddlInfo.OtherInfo["SBSequenceNo"].Value);
                }
            }
            #endregion //WI#11632
            if (adGbBank != null)
            {
                _wosaPrintInfo.BankName = adGbBank.Name1.Value;
                _wosaPrintInfo.BankAddress = adGbBank.AddressLine1.Value + (!string.IsNullOrEmpty(adGbBank.AddressLine2.Value) ? (" " + adGbBank.AddressLine2.Value) : null);
                _wosaPrintInfo.BankCityAndState = adGbBank.City.Value + " " + adGbBank.State.Value;
            }
            #region WI#11294
            if (adGbBranch != null)
            {
                _wosaPrintInfo.BranchAddress = adGbBranch.AddressLine1.Value + (!string.IsNullOrEmpty(adGbBranch.AddressLine2.Value) ? (" " + adGbBranch.AddressLine2.Value) : null);
                _wosaPrintInfo.BranchCityState = adGbBranch.City.Value + " " + adGbBranch.State.Value;
                _wosaPrintInfo.BranchZip = adGbBranch.Zip.Value;
                _wosaPrintInfo.BranchPhone = adGbBranch.Phone1.Value;
            }
            #endregion //WI#11294
            _wosaPrintInfo.ItemNo = Convert.ToInt32(colItemNo.UnFormattedValue);
            //_wosaPrintInfo.ItemRoutingNo = colRoutingNo.Text;
			_wosaPrintInfo.ItemCheckType = colCheckType.Text;
			_wosaPrintInfo.ItemAcctType = colAcctType.Text;
			_wosaPrintInfo.ItemAcctNo = colGuestAcctNo.Text;
			if (colCheckNo.UnFormattedValue != null)
				_wosaPrintInfo.ItemCheckNo = Convert.ToInt32(colCheckNo.UnFormattedValue);
			else
				_wosaPrintInfo.ItemCheckNo = DbInt.Null;	// #71741

 			_wosaPrintInfo.ItemAmount = Convert.ToDecimal(colAmount.UnFormattedValue);
            _wosaPrintInfo.SbMemberCuName = colGuestCUName.Text;
            _wosaPrintInfo.SbMemberName = colGuestMbrName.Text;
            _wosaPrintInfo.SbMemberCuRoutingNo = colGuestCURoutingNo.Text;
            _wosaPrintInfo.EmployeeName = colTeller.Text;
            _wosaPrintInfo.TellerNo = Convert.ToInt16(colTellerNo.UnFormattedValue);
            _wosaPrintInfo.AcctNo = colGuestAcctNo.Text;
            _wosaPrintInfo.CreateDt = Convert.ToDateTime(colCreateDt.UnFormattedValue);
            _wosaPrintInfo.SequenceNo = Convert.ToInt16( colSequenceNo.UnFormattedValue);
            //if (colTranEffectiveDt.UnFormattedValue != null)
            //    _wosaPrintInfo.ItemTranEffDt = Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue);
            //else
            //    _wosaPrintInfo.ItemTranEffDt = DbDateTime.Null;	// #71741

            //_wosaPrintInfo.ItemCreateDt = Convert.ToDateTime(colCreateDt.UnFormattedValue);
			#endregion
		}

		private bool LoadPrintObject(bool isReprintAll)
		{
			if (_tellerVars.AdTlControl.WosaPrinting.Value != GlobalVars.Instance.ML.Y)
			{
				PMessageBox.Show(this, 360622, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return false;
			}
			_reprintInfo = CoreService.Translation.GetUserMessageX(360621);
			if (!isReprintAll)
			{
				_tempChkAmount = colAmount.MakeFormattedValue(colAmount.UnFormattedValue);
				_tempChkAmount = _tempChkAmount.PadLeft(14, '*');
				_tempChkAmount = _tempChkAmount.Replace("*", " ");
				//
                //_checkItemInfo = colItemNo.UnFormattedValue.ToString() + " - " + colCheckType.Text + _tempChkAmount;
                _checkItemInfo = colCheckType.Text + _tempChkAmount;
			}
			else
				_checkItemInfo = CoreService.Translation.GetUserMessageX(360652);

			//
			CallOtherForms("AdHocReceipt");
			//
			if (_reprintFormId.IsNull || _reprintFormId.Value == 0)
				return false;
			//
			#region get form info
			if (_tellerVars.SetContextObject("AdTlFormArray", null, _reprintFormId.Value))
			{
				_reprintTextQrp = _tellerVars.AdTlForm.TextQrp.Value;
				_partialPrintString = _tellerVars.AdTlForm.PrintString.Value;
                _wosaServiceName = _printerService.Value; //WI3475
				//_wosaServiceName = _tellerVars.AdTlForm.LogicalService.Value;
				//_logicalService = _tellerVars.AdTlForm.ServiceType.Value;
				_mediaName = _tellerVars.AdTlForm.MediaName.Value;
				_formName = _tellerVars.AdTlForm.FormName.Value;
			}
			#endregion
			//
			return true;
			//
		}

		private bool HandlePrinting()
		{
			try
			{
				if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
				{
					_xfsPrinter = new XfsPrinter(_wosaServiceName);	  //#157637
					return _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo);   //#157637
				}
                return false;
			}
			finally
			{
				_xfsPrinter.Close();	//#157637
			}
		}

        private void CallXMThruCDS(string origin)
        {
            if (origin == "UpdateEndorseStatus")
            {
                _busObjCapturedItems = new TlItemCapture();
                _busObjCapturedItems.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
                _busObjCapturedItems.DrawerNo.Value = Convert.ToInt16(colDrawerNo.UnFormattedValue);
                _busObjCapturedItems.SequenceNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
                _busObjCapturedItems.Ptid.Value = Convert.ToDecimal( colPtid.UnFormattedValue);
                _busObjCapturedItems.RecordSource.Value = colRecordSource.Text;
                _busObjCapturedItems.EndorseStatus.Value = "P";

                Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Update, _busObjCapturedItems );
            }
            // Begin WI#11632
            else if (origin == "GetTlJournalAddlInfo")
            {
                Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_busObjTlJournalAddlInfo);
            }
            // End WI#11632

        }

        private void CalcTotal()
        {
            dfItemTotals.UnFormattedValue = 0;
            dfNoOfItems.UnFormattedValue = 0;
            decimal itemTotals = 0;
            decimal amt = 0;
            int noOfItemsCount = 0;

            object objectValue = null;
            //
            for (int rowId = 0; rowId < gridEndorseItems.Count; rowId++)
            {
                amt = 0;
                //
                objectValue = gridEndorseItems.GetCellValueUnformatted(rowId, colAmount.ColumnId);
                if (objectValue == null)
                    amt = 0;
                else
                    amt = Convert.ToDecimal(objectValue);

                itemTotals = itemTotals + amt;
                noOfItemsCount = noOfItemsCount + 1;
            }
            dfItemTotals.UnFormattedValue = itemTotals;
            dfNoOfItems.UnFormattedValue = noOfItemsCount;
        }
		#endregion

        private void rbNotPrinted_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            //Begin #10117
            //if ( rbNotPrinted.Checked )
            //    gridEndorseItems.PopulateTable();
            EnableDisableVisibleLogic("NonPrintedClicked");
            this.cbDate.Checked = false;
            cbDate_PhoenixUICheckedChangedEvent(null, null );
            //End #10117
        }

        private void rbPrinted_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            //Begin #10117
            //if ( rbPrinted.Checked )
            //    gridEndorseItems.PopulateTable();
            EnableDisableVisibleLogic("PrintedClicked");
            this.cbDate.Checked = true;
            cbDate_PhoenixUICheckedChangedEvent(null, null);
            //End #10117
        }

        private void pbDisplay_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("DisplayClick");
        }

        //Begin #10117
        private void pbSearch_Click(object sender, PActionEventArgs e)
        {
            gridEndorseItems.PopulateTable();
        }

        private void cmbBranch_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
        {
            _busObjTlJournal.BranchNo.SetValue(cmbBranch.CodeValue);
            _busObjTlJournal.IsIncludeSupervisor.Value = GlobalVars.Instance.ML.Y; // this to get drwaers poplaued with drawer #
            Helper.PopulateCombo(cmbDrawers, _busObjTlJournal, _busObjTlJournal.DrawerNo);
            cmbDrawers.Append(-1, GlobalVars.Instance.ML.All);
            cmbDrawers.SetValue(-1);
        }

        private void cbDate_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            EnableDisableVisibleLogic("DateClicked");
        }
        //End #10117

	}
}
