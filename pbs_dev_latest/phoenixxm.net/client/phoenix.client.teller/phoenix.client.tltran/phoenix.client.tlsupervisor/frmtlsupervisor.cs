#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

//-------------------------------------------------------------------------------
// File Name: frmTlTrancode.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//??/??/??		1		RPoddar		#????? - Created.
//11/27/06		2		mselvaga	#70577 - Fixed cmbBranch code value to fix NULL problem.
//12/07/2006	3		mselvaga	#69257 - Added additional param to frmTlJournal call to support sup. reversal.
//12/19/2006	4		vreddy		#71195 - Increased width of employee is col unforforwed trans are disabled
//01/16/2007	5		mselvaga	#71425 - Commented out changes added for issue#71195 as well as 
//									set the visible property of colUnfwdTransactions = false, not supported in .NET.
//04/17/2007	6		mselvaga	#72270 - Fixed closed date parameter value for position call  for new drawers.
//03/31/2009    7       mselvaga    #76033 - Added Hyland onbase integration changes.
//06/02/2009    8       LSimpson    #4366 - Added date param for frmtlprooftotals.
//06/29/2009    9       mselvaga    WI#4779 - Fix the drive through setup problem.
//09/10/2009    10      mselvaga    WI#5215/#5216/#5671 - Fixed the drive through reg setting using machine name.
//11/04/2013    11      mselvaga    #140895 - Teller Capture Integration changes added.
//01/20/2014    12      spatterson  #140895 - Teller Capture Integration.
//09/02/2014    13      mselvaga    #30969 - #140895 - AVTC Part I changes added.
//09/26/2014    14      mselvaga    #52625 - Teller Capture Application Totals changes added.
//06/25/2019    15      AshishBabu  Bug#116424  - Preventing closedout drawer from reassigning
//08/14/2019    16      RDeepthi    #117975. Added New control and made status visible. On new checkbox changed, enable Search button. Pass checkbox value to BO
//08/14/2019    17      bhughes     117975b - Fixed syntax error, from initial check-in,
//09/18/2019    18      mselvaga    Task#119479 - Added ECM Voucher validation changes.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using System.Xml;
//
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Admin.Global;

namespace Phoenix.Client.TlSupervisor
{
	/// <summary>
	/// Summary description for frmTlSupervisor.
	/// </summary>
	public class frmTlSupervisor : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbSearchCriteria;
		private Phoenix.Windows.Forms.PLabelStandard lblBranch;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbBranch;
		private Phoenix.Windows.Forms.PLabelStandard lblPositionDate;
		private Phoenix.Windows.Forms.PdfStandard dfPositionDt;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbDrawerStatusInformation;
		private Phoenix.Windows.Forms.PGrid gridTellerDrawers;
		private Phoenix.Windows.Forms.PGridColumn colEmployeeId;
		private Phoenix.Windows.Forms.PGridColumn colDrawerPtid;
		private Phoenix.Windows.Forms.PGridColumn colCrncyID;
		private Phoenix.Windows.Forms.PGridColumn colDrawerStatus;
		private Phoenix.Windows.Forms.PGridColumn colClosingCash;
		private Phoenix.Windows.Forms.PGridColumn colCashIn;
		private Phoenix.Windows.Forms.PGridColumn colCashOut;
		private Phoenix.Windows.Forms.PGridColumn colDrawerNo;
		private Phoenix.Windows.Forms.PGridColumn colBranchNo;
		private Phoenix.Windows.Forms.PGridColumn colCurPostingDt;
		private Phoenix.Windows.Forms.PGridColumn colISOCode;
		private Phoenix.Windows.Forms.PGridColumn colEmployee;
		private Phoenix.Windows.Forms.PGridColumn colNoTransactions;
		private Phoenix.Windows.Forms.PGridColumn colUnfwdTransactions;
		private Phoenix.Windows.Forms.PGridColumn colDrawerBalance;
		private Phoenix.Windows.Forms.PGridColumn colBranchName;
		private Phoenix.Windows.Forms.PGridColumn colNoDrawers;
		private Phoenix.Windows.Forms.PAction pbSearch;
		private Phoenix.Windows.Forms.PAction pbJournal;
		private Phoenix.Windows.Forms.PAction pbTranTotals;
		private Phoenix.Windows.Forms.PAction pbPosition;
		private Phoenix.Windows.Forms.PAction pbBranchPosition;
		private Phoenix.Windows.Forms.PAction pbDirectLoad;
		private Phoenix.Windows.Forms.PAction pbFwdOffline;
		private Phoenix.Windows.Forms.PAction pbForceOffline;
		private Phoenix.Windows.Forms.PAction pbCreatePOD;
		private Phoenix.Windows.Forms.PAction pbDirectPodLoad;
		private Phoenix.Windows.Forms.PAction pbPurge;
		private Phoenix.Windows.Forms.PAction pbOffline;
		private Phoenix.Windows.Forms.PAction pbDrawerMaint;
		private Phoenix.Windows.Forms.PAction pbCTRReview;
		private Phoenix.Windows.Forms.PAction pbResetPwd;
		private Phoenix.Windows.Forms.PAction pbProofTotals;    //#76409


		#region Initialize
		private TlDrawerBalances _busObjDrawerBal = new TlDrawerBalances();
		private TellerVars _tellerVars = TellerVars.Instance;
		private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);
//		private bool isUnFwdTransExists = false;
		private bool isAllSearch = false;
		private bool bAllSearch = false;
		private Phoenix.Windows.Forms.PLabelStandard lblDrawerisOpenforadateinthepast;
		private Phoenix.Windows.Forms.PGridColumn colEmployeeOrig;
		private Phoenix.Windows.Forms.PGridColumn colUnbatchedAmt;
		private Phoenix.Windows.Forms.PGridColumn colCrncyStatus;
		private Phoenix.Windows.Forms.PGridColumn colClosingDt;
		private XmlNode gridNode;
		private bool fromDrawerBal = false;
		private Phoenix.Windows.Forms.PGridColumn colDrCurPostingDt;
		private bool openDrawer = false;
		private PComboBoxStandard drawerCombo = null;
		private int _employeeColWidth = 0;
		private PAction pbDriveThru;
		private PAction pbInventory;
		private PAction pbBulkMaint;
        private PGridColumn colPendingATVCBatches;
        private PAction pbPendingEOB;
        private PAction pbApplTotals;
        private PCheckBoxStandard cbIncludeClosed;
        private int _unfwdTransactionsColWidth = 0;
		#endregion

		public frmTlSupervisor()
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
            this.gbSearchCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfPositionDt = new Phoenix.Windows.Forms.PdfStandard();
            this.lblPositionDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbBranch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbDrawerStatusInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblDrawerisOpenforadateinthepast = new Phoenix.Windows.Forms.PLabelStandard();
            this.gridTellerDrawers = new Phoenix.Windows.Forms.PGrid();
            this.colEmployeeId = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colCrncyID = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colCrncyStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colClosingCash = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashIn = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashOut = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCurPostingDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colClosingDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colISOCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colEmployee = new Phoenix.Windows.Forms.PGridColumn();
            this.colEmployeeOrig = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoTransactions = new Phoenix.Windows.Forms.PGridColumn();
            this.colUnfwdTransactions = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerBalance = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchName = new Phoenix.Windows.Forms.PGridColumn();
            this.colNoDrawers = new Phoenix.Windows.Forms.PGridColumn();
            this.colUnbatchedAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrCurPostingDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colPendingATVCBatches = new Phoenix.Windows.Forms.PGridColumn();
            this.pbSearch = new Phoenix.Windows.Forms.PAction();
            this.pbJournal = new Phoenix.Windows.Forms.PAction();
            this.pbTranTotals = new Phoenix.Windows.Forms.PAction();
            this.pbPosition = new Phoenix.Windows.Forms.PAction();
            this.pbBranchPosition = new Phoenix.Windows.Forms.PAction();
            this.pbDirectLoad = new Phoenix.Windows.Forms.PAction();
            this.pbFwdOffline = new Phoenix.Windows.Forms.PAction();
            this.pbForceOffline = new Phoenix.Windows.Forms.PAction();
            this.pbCreatePOD = new Phoenix.Windows.Forms.PAction();
            this.pbDirectPodLoad = new Phoenix.Windows.Forms.PAction();
            this.pbPurge = new Phoenix.Windows.Forms.PAction();
            this.pbOffline = new Phoenix.Windows.Forms.PAction();
            this.pbDrawerMaint = new Phoenix.Windows.Forms.PAction();
            this.pbCTRReview = new Phoenix.Windows.Forms.PAction();
            this.pbResetPwd = new Phoenix.Windows.Forms.PAction();
            this.pbProofTotals = new Phoenix.Windows.Forms.PAction();
            this.pbDriveThru = new Phoenix.Windows.Forms.PAction();
            this.pbInventory = new Phoenix.Windows.Forms.PAction();
            this.pbBulkMaint = new Phoenix.Windows.Forms.PAction();
            this.pbPendingEOB = new Phoenix.Windows.Forms.PAction();
            this.pbApplTotals = new Phoenix.Windows.Forms.PAction();
            this.cbIncludeClosed = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbSearchCriteria.SuspendLayout();
            this.gbDrawerStatusInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSearch,
            this.pbJournal,
            this.pbTranTotals,
            this.pbProofTotals,
            this.pbPosition,
            this.pbBranchPosition,
            this.pbDirectLoad,
            this.pbFwdOffline,
            this.pbForceOffline,
            this.pbCreatePOD,
            this.pbPurge,
            this.pbOffline,
            this.pbDrawerMaint,
            this.pbCTRReview,
            this.pbDirectPodLoad,
            this.pbResetPwd,
            this.pbDriveThru,
            this.pbInventory,
            this.pbBulkMaint,
            this.pbPendingEOB,
            this.pbApplTotals});
            // 
            // gbSearchCriteria
            // 
            this.gbSearchCriteria.Controls.Add(this.cbIncludeClosed);
            this.gbSearchCriteria.Controls.Add(this.dfPositionDt);
            this.gbSearchCriteria.Controls.Add(this.lblPositionDate);
            this.gbSearchCriteria.Controls.Add(this.cmbBranch);
            this.gbSearchCriteria.Controls.Add(this.lblBranch);
            this.gbSearchCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbSearchCriteria.Name = "gbSearchCriteria";
            this.gbSearchCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbSearchCriteria.Size = new System.Drawing.Size(684, 40);
            this.gbSearchCriteria.TabIndex = 1;
            this.gbSearchCriteria.TabStop = false;
            this.gbSearchCriteria.Text = "Search Criteria";
            // 
            // dfPositionDt
            // 
            this.dfPositionDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPositionDt.Location = new System.Drawing.Point(515, 16);
            this.dfPositionDt.Name = "dfPositionDt";
            this.dfPositionDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPositionDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfPositionDt.PhoenixUIControl.ObjectId = 33;
            this.dfPositionDt.PhoenixUIControl.XmlTag = "PostingDt";
            this.dfPositionDt.PreviousValue = null;
            this.dfPositionDt.Size = new System.Drawing.Size(66, 20);
            this.dfPositionDt.TabIndex = 0;
            this.dfPositionDt.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfPositionDt_PhoenixUIValidateEvent);
            // 
            // lblPositionDate
            // 
            this.lblPositionDate.AutoEllipsis = true;
            this.lblPositionDate.Location = new System.Drawing.Point(427, 16);
            this.lblPositionDate.Name = "lblPositionDate";
            this.lblPositionDate.PhoenixUIControl.ObjectId = 33;
            this.lblPositionDate.Size = new System.Drawing.Size(88, 20);
            this.lblPositionDate.TabIndex = 1;
            this.lblPositionDate.Text = "Position Date:";
            // 
            // cmbBranch
            // 
            this.cmbBranch.Location = new System.Drawing.Point(68, 16);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.PhoenixUIControl.ObjectId = 2;
            this.cmbBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.cmbBranch.Size = new System.Drawing.Size(188, 21);
            this.cmbBranch.TabIndex = 2;
            this.cmbBranch.Value = null;
            this.cmbBranch.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbBranch_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblBranch
            // 
            this.lblBranch.AutoEllipsis = true;
            this.lblBranch.Location = new System.Drawing.Point(4, 16);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.PhoenixUIControl.ObjectId = 2;
            this.lblBranch.Size = new System.Drawing.Size(52, 20);
            this.lblBranch.TabIndex = 3;
            this.lblBranch.Text = "Branch:";
            // 
            // gbDrawerStatusInformation
            // 
            this.gbDrawerStatusInformation.Controls.Add(this.lblDrawerisOpenforadateinthepast);
            this.gbDrawerStatusInformation.Controls.Add(this.gridTellerDrawers);
            this.gbDrawerStatusInformation.Location = new System.Drawing.Point(4, 40);
            this.gbDrawerStatusInformation.Name = "gbDrawerStatusInformation";
            this.gbDrawerStatusInformation.PhoenixUIControl.ObjectId = 3;
            this.gbDrawerStatusInformation.Size = new System.Drawing.Size(684, 404);
            this.gbDrawerStatusInformation.TabIndex = 0;
            this.gbDrawerStatusInformation.TabStop = false;
            this.gbDrawerStatusInformation.Text = "Drawer Status Information";
            // 
            // lblDrawerisOpenforadateinthepast
            // 
            this.lblDrawerisOpenforadateinthepast.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDrawerisOpenforadateinthepast.AutoEllipsis = true;
            this.lblDrawerisOpenforadateinthepast.Location = new System.Drawing.Point(4, 380);
            this.lblDrawerisOpenforadateinthepast.Name = "lblDrawerisOpenforadateinthepast";
            this.lblDrawerisOpenforadateinthepast.PhoenixUIControl.ObjectId = 41;
            this.lblDrawerisOpenforadateinthepast.Size = new System.Drawing.Size(232, 20);
            this.lblDrawerisOpenforadateinthepast.TabIndex = 1;
            this.lblDrawerisOpenforadateinthepast.Text = "* Drawer is Open for a date in the past.";
            // 
            // gridTellerDrawers
            // 
            this.gridTellerDrawers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTellerDrawers.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colEmployeeId,
            this.colDrawerPtid,
            this.colCrncyID,
            this.colCrncyStatus,
            this.colClosingCash,
            this.colCashIn,
            this.colCashOut,
            this.colDrawerNo,
            this.colBranchNo,
            this.colCurPostingDt,
            this.colClosingDt,
            this.colDrawerStatus,
            this.colISOCode,
            this.colEmployee,
            this.colEmployeeOrig,
            this.colNoTransactions,
            this.colUnfwdTransactions,
            this.colDrawerBalance,
            this.colBranchName,
            this.colNoDrawers,
            this.colUnbatchedAmt,
            this.colDrCurPostingDt,
            this.colPendingATVCBatches});
            this.gridTellerDrawers.IsMaxNumRowsCustomized = false;
            this.gridTellerDrawers.LinesInHeader = 2;
            this.gridTellerDrawers.Location = new System.Drawing.Point(4, 16);
            this.gridTellerDrawers.Name = "gridTellerDrawers";
            this.gridTellerDrawers.Size = new System.Drawing.Size(676, 356);
            this.gridTellerDrawers.TabIndex = 0;
            this.gridTellerDrawers.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridTellerDrawers_BeforePopulate);
            this.gridTellerDrawers.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridTellerDrawers_FetchRowDone);
            this.gridTellerDrawers.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridTellerDrawers_AfterPopulate);
            this.gridTellerDrawers.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridTellerDrawers_SelectedIndexChanged);
            // 
            // colEmployeeId
            // 
            this.colEmployeeId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEmployeeId.PhoenixUIControl.ObjectId = 23;
            this.colEmployeeId.PhoenixUIControl.XmlTag = "EmplId";
            this.colEmployeeId.Title = "Column";
            this.colEmployeeId.Visible = false;
            this.colEmployeeId.Width = 0;
            // 
            // colDrawerPtid
            // 
            this.colDrawerPtid.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDrawerPtid.PhoenixUIControl.ObjectId = 24;
            this.colDrawerPtid.PhoenixUIControl.XmlTag = "DrawerPtid";
            this.colDrawerPtid.Title = "Column";
            this.colDrawerPtid.Visible = false;
            this.colDrawerPtid.Width = 0;
            // 
            // colCrncyID
            // 
            this.colCrncyID.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCrncyID.PhoenixUIControl.ObjectId = 23;
            this.colCrncyID.PhoenixUIControl.XmlTag = "CrncyId";
            this.colCrncyID.Title = "Column";
            this.colCrncyID.Visible = false;
            this.colCrncyID.Width = 0;
            // 
            // colDrawerStatus
            // 
            this.colDrawerStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDrawerStatus.PhoenixUIControl.ObjectId = 26;
            this.colDrawerStatus.PhoenixUIControl.XmlTag = "DrawerStatus";
            this.colDrawerStatus.Title = "Status";
            this.colDrawerStatus.Width = 50;
            // 
            // colCrncyStatus
            // 
            this.colCrncyStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCrncyStatus.PhoenixUIControl.XmlTag = "CrncyStatus";
            this.colCrncyStatus.Title = "Crncy Status";
            this.colCrncyStatus.Visible = false;
            this.colCrncyStatus.Width = 0;
            // 
            // colClosingCash
            // 
            this.colClosingCash.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colClosingCash.PhoenixUIControl.ObjectId = 26;
            this.colClosingCash.PhoenixUIControl.XmlTag = "ClosingCash";
            this.colClosingCash.Title = "Column";
            this.colClosingCash.Visible = false;
            this.colClosingCash.Width = 0;
            // 
            // colCashIn
            // 
            this.colCashIn.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCashIn.PhoenixUIControl.ObjectId = 26;
            this.colCashIn.PhoenixUIControl.XmlTag = "CashIn";
            this.colCashIn.Title = "Column";
            this.colCashIn.Visible = false;
            this.colCashIn.Width = 0;
            // 
            // colCashOut
            // 
            this.colCashOut.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCashOut.PhoenixUIControl.ObjectId = 26;
            this.colCashOut.PhoenixUIControl.XmlTag = "CashOut";
            this.colCashOut.Title = "Column";
            this.colCashOut.Visible = false;
            this.colCashOut.Width = 0;
            // 
            // colDrawerNo
            // 
            this.colDrawerNo.PhoenixUIControl.ObjectId = 5;
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "Drawer #";
            this.colDrawerNo.Width = 54;
            // 
            // colBranchNo
            // 
            this.colBranchNo.PhoenixUIControl.ObjectId = 27;
            this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo.Title = "Branch #";
            this.colBranchNo.Width = 54;
            // 
            // colCurPostingDt
            // 
            this.colCurPostingDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCurPostingDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCurPostingDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCurPostingDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colCurPostingDt.PhoenixUIControl.ObjectId = 25;
            this.colCurPostingDt.PhoenixUIControl.XmlTag = "LastLogonDt";
            this.colCurPostingDt.Title = "Last Open Date";
            this.colCurPostingDt.Width = 85;
            // 
            // colClosingDt
            // 
            this.colClosingDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colClosingDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colClosingDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colClosingDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colClosingDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colClosingDt.PhoenixUIControl.XmlTag = "ClosingDt";
            this.colClosingDt.Title = "Closing Date";
            this.colClosingDt.Visible = false;
            this.colClosingDt.Width = 0;
            // 
            // colISOCode
            // 
            this.colISOCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colISOCode.PhoenixUIControl.ObjectId = 26;
            this.colISOCode.PhoenixUIControl.XmlTag = "IsoCode";
            this.colISOCode.Title = "Column";
            this.colISOCode.Visible = false;
            this.colISOCode.Width = 0;
            // 
            // colEmployee
            // 
            this.colEmployee.PhoenixUIControl.ObjectId = 6;
            this.colEmployee.PhoenixUIControl.XmlTag = "Employee";
            this.colEmployee.Title = "Employee";
            this.colEmployee.Width = 257;
            // 
            // colEmployeeOrig
            // 
            this.colEmployeeOrig.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEmployeeOrig.Title = "EmployeeOrig";
            this.colEmployeeOrig.Visible = false;
            this.colEmployeeOrig.Width = 0;
            // 
            // colNoTransactions
            // 
            this.colNoTransactions.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoTransactions.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colNoTransactions.PhoenixUIControl.ObjectId = 7;
            this.colNoTransactions.PhoenixUIControl.XmlTag = "NoTrans";
            this.colNoTransactions.Title = "# Transactions";
            this.colNoTransactions.Width = 75;
            // 
            // colUnfwdTransactions
            // 
            this.colUnfwdTransactions.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUnfwdTransactions.PhoenixUIControl.ObjectId = 40;
            this.colUnfwdTransactions.PhoenixUIControl.XmlTag = "UnfwdTrans";
            this.colUnfwdTransactions.Title = "Unfwd Trans";
            this.colUnfwdTransactions.Visible = false;
            this.colUnfwdTransactions.Width = 75;
            // 
            // colDrawerBalance
            // 
            this.colDrawerBalance.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDrawerBalance.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDrawerBalance.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colDrawerBalance.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colDrawerBalance.PhoenixUIControl.ObjectId = 8;
            this.colDrawerBalance.PhoenixUIControl.XmlTag = "DrawerBalance";
            this.colDrawerBalance.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colDrawerBalance.Title = "Drawer Balance";
            this.colDrawerBalance.Width = 128;
            // 
            // colBranchName
            // 
            this.colBranchName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBranchName.PhoenixUIControl.ObjectId = 28;
            this.colBranchName.PhoenixUIControl.XmlTag = "BranchName";
            this.colBranchName.Title = "Branch Name";
            this.colBranchName.Visible = false;
            this.colBranchName.Width = 0;
            // 
            // colNoDrawers
            // 
            this.colNoDrawers.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colNoDrawers.PhoenixUIControl.ObjectId = 30;
            this.colNoDrawers.PhoenixUIControl.XmlTag = "NoDrawers";
            this.colNoDrawers.Title = "# Drawers";
            this.colNoDrawers.Visible = false;
            this.colNoDrawers.Width = 0;
            // 
            // colUnbatchedAmt
            // 
            this.colUnbatchedAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUnbatchedAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUnbatchedAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colUnbatchedAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colUnbatchedAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUnbatchedAmt.PhoenixUIControl.XmlTag = "UnbatchedAmt";
            this.colUnbatchedAmt.Title = "Unbatched Amt";
            this.colUnbatchedAmt.Visible = false;
            this.colUnbatchedAmt.Width = 0;
            // 
            // colDrCurPostingDt
            // 
            this.colDrCurPostingDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colDrCurPostingDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDrCurPostingDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colDrCurPostingDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colDrCurPostingDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDrCurPostingDt.PhoenixUIControl.XmlTag = "CurPostingDt";
            this.colDrCurPostingDt.Title = "Drawer Cur Pos Dt";
            this.colDrCurPostingDt.Visible = false;
            this.colDrCurPostingDt.Width = 0;
            // 
            // colPendingATVCBatches
            // 
            this.colPendingATVCBatches.PhoenixUIControl.XmlTag = "PendingAVTCBatches";
            this.colPendingATVCBatches.Title = "";
            this.colPendingATVCBatches.Visible = false;
            // 
            // pbSearch
            // 
            this.pbSearch.Name = null;
            this.pbSearch.ObjectId = 34;
            this.pbSearch.Tag = null;
            this.pbSearch.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSearch_Click);
            // 
            // pbJournal
            // 
            this.pbJournal.LongText = "pbJournal";
            this.pbJournal.Name = "pbJournal";
            this.pbJournal.ObjectId = 11;
            this.pbJournal.ShortText = "pbJournal";
            this.pbJournal.Tag = null;
            this.pbJournal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbJournal_Click);
            // 
            // pbTranTotals
            // 
            this.pbTranTotals.LongText = "pbTranTotals";
            this.pbTranTotals.Name = "pbTranTotals";
            this.pbTranTotals.ObjectId = 12;
            this.pbTranTotals.ShortText = "pbTranTotals";
            this.pbTranTotals.Tag = null;
            this.pbTranTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTranTotals_Click);
            // 
            // pbPosition
            // 
            this.pbPosition.LongText = "pbPosition";
            this.pbPosition.Name = "pbPosition";
            this.pbPosition.ObjectId = 14;
            this.pbPosition.ShortText = "pbPosition";
            this.pbPosition.Tag = null;
            this.pbPosition.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPosition_Click);
            // 
            // pbBranchPosition
            // 
            this.pbBranchPosition.LongText = "pbBranchPosition";
            this.pbBranchPosition.Name = "pbBranchPosition";
            this.pbBranchPosition.ObjectId = 35;
            this.pbBranchPosition.ShortText = "pbBranchPosition";
            this.pbBranchPosition.Tag = null;
            this.pbBranchPosition.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBranchPosition_Click);
            // 
            // pbDirectLoad
            // 
            this.pbDirectLoad.LongText = "pbDirectLoad";
            this.pbDirectLoad.Name = "pbDirectLoad";
            this.pbDirectLoad.ObjectId = 18;
            this.pbDirectLoad.ShortText = "pbDirectLoad";
            this.pbDirectLoad.Tag = null;
            // 
            // pbFwdOffline
            // 
            this.pbFwdOffline.LongText = "pbFwdOffline";
            this.pbFwdOffline.Name = "pbFwdOffline";
            this.pbFwdOffline.ObjectId = 39;
            this.pbFwdOffline.ShortText = "pbFwdOffline";
            this.pbFwdOffline.Tag = null;
            // 
            // pbForceOffline
            // 
            this.pbForceOffline.LongText = "pbForceOffline";
            this.pbForceOffline.Name = "pbForceOffline";
            this.pbForceOffline.ObjectId = 38;
            this.pbForceOffline.ShortText = "pbForceOffline";
            this.pbForceOffline.Tag = null;
            // 
            // pbCreatePOD
            // 
            this.pbCreatePOD.LongText = "pbCreatePOD";
            this.pbCreatePOD.Name = "pbCreatePOD";
            this.pbCreatePOD.ObjectId = 19;
            this.pbCreatePOD.ShortText = "pbCreatePOD";
            this.pbCreatePOD.Tag = null;
            // 
            // pbDirectPodLoad
            // 
            this.pbDirectPodLoad.LongText = "pbDirectPodLoad";
            this.pbDirectPodLoad.Name = "pbDirectPodLoad";
            this.pbDirectPodLoad.ObjectId = 32;
            this.pbDirectPodLoad.ShortText = "pbDirectPodLoad";
            this.pbDirectPodLoad.Tag = null;
            // 
            // pbPurge
            // 
            this.pbPurge.LongText = "pbPurge";
            this.pbPurge.Name = "pbPurge";
            this.pbPurge.ObjectId = 20;
            this.pbPurge.ShortText = "pbPurge";
            this.pbPurge.Tag = null;
            // 
            // pbOffline
            // 
            this.pbOffline.LongText = "pbOffline";
            this.pbOffline.Name = "pbOffline";
            this.pbOffline.ObjectId = 21;
            this.pbOffline.ShortText = "pbOffline";
            this.pbOffline.Tag = null;
            // 
            // pbDrawerMaint
            // 
            this.pbDrawerMaint.LongText = "pbDrawerMaint";
            this.pbDrawerMaint.Name = "pbDrawerMaint";
            this.pbDrawerMaint.ObjectId = 22;
            this.pbDrawerMaint.ShortText = "pbDrawerMaint";
            this.pbDrawerMaint.Tag = null;
            this.pbDrawerMaint.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDrawerMaint_Click);
            // 
            // pbCTRReview
            // 
            this.pbCTRReview.LongText = "pbCTRReview";
            this.pbCTRReview.Name = "pbCTRReview";
            this.pbCTRReview.ObjectId = 36;
            this.pbCTRReview.ShortText = "pbCTRReview";
            this.pbCTRReview.Tag = null;
            this.pbCTRReview.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCTRReview_Click);
            // 
            // pbResetPwd
            // 
            this.pbResetPwd.LongText = "pbResetPwd";
            this.pbResetPwd.Name = "pbResetPwd";
            this.pbResetPwd.ObjectId = 37;
            this.pbResetPwd.ShortText = "pbResetPwd";
            this.pbResetPwd.Tag = null;
            // 
            // pbProofTotals
            // 
            this.pbProofTotals.LongText = "P&roof Totals...";
            this.pbProofTotals.Name = "P&roof Totals...";
            this.pbProofTotals.NextScreenId = 2866;
            this.pbProofTotals.ObjectId = 42;
            this.pbProofTotals.ShortText = "P&roof Totals...";
            this.pbProofTotals.Tag = null;
            this.pbProofTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbProofTotals_Click);
            // 
            // pbDriveThru
            // 
            this.pbDriveThru.LongText = "pbDriveThru";
            this.pbDriveThru.Name = "pbDriveThru";
            this.pbDriveThru.ObjectId = 43;
            this.pbDriveThru.ShortText = "pbDriveThru";
            this.pbDriveThru.Tag = null;
            this.pbDriveThru.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDriveThru_Click);
            // 
            // pbInventory
            // 
            this.pbInventory.Name = null;
            this.pbInventory.ObjectId = 45;
            this.pbInventory.Tag = null;
            this.pbInventory.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbInventory_Click);
            // 
            // pbBulkMaint
            // 
            this.pbBulkMaint.LongText = "Bulk Maint...";
            this.pbBulkMaint.MLInfo.StatusLine = "Bulk Maint...";
            this.pbBulkMaint.MLInfo.ToolTip = "Bulk Maint...";
            this.pbBulkMaint.Name = null;
            this.pbBulkMaint.ObjectId = 46;
            this.pbBulkMaint.StatusText = "Bulk Maint...";
            this.pbBulkMaint.Tag = null;
            this.pbBulkMaint.ToolTip = "Bulk Maint...";
            this.pbBulkMaint.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBulkMaint_Click);
            // 
            // pbPendingEOB
            // 
            this.pbPendingEOB.LongText = "Pending EOB...";
            this.pbPendingEOB.Name = "Pending EOB...";
            this.pbPendingEOB.ObjectId = 47;
            this.pbPendingEOB.ShortText = "Pending EOB...";
            this.pbPendingEOB.Tag = null;
            this.pbPendingEOB.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPendingEOB_Click);
            // 
            // pbApplTotals
            // 
            this.pbApplTotals.LongText = "App&l Totals...";
            this.pbApplTotals.Name = "pbApplTotals";
            this.pbApplTotals.ObjectId = 48;
            this.pbApplTotals.ShortText = "App&l Totals...";
            this.pbApplTotals.Tag = null;
            this.pbApplTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbApplTotals_Click);
            // 
            // cbIncludeClosed
            // 
            this.cbIncludeClosed.BackColor = System.Drawing.SystemColors.Control;
            this.cbIncludeClosed.Location = new System.Drawing.Point(591, 16);
            this.cbIncludeClosed.Name = "cbIncludeClosed";
            this.cbIncludeClosed.Size = new System.Drawing.Size(88, 18);
            this.cbIncludeClosed.TabIndex = 7;
            this.cbIncludeClosed.Text = "Include Closed";
            this.cbIncludeClosed.UseVisualStyleBackColor = false;
            this.cbIncludeClosed.Value = null;
            this.cbIncludeClosed.PhoenixUICheckedChangedEvent += CbIncludeClosed_PhoenixUICheckedChangedEvent;
            // 
            // frmTlSupervisor
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbDrawerStatusInformation);
            this.Controls.Add(this.gbSearchCriteria);
            this.Name = "frmTlSupervisor";
            this.ScreenId = 10448;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlSupervisor_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlSupervisor_PInitCompleteEvent);
            this.Load += new System.EventHandler(this.frmTlSupervisor_Load);
            this.gbSearchCriteria.ResumeLayout(false);
            this.gbSearchCriteria.PerformLayout();
            this.gbDrawerStatusInformation.ResumeLayout(false);
            this.ResumeLayout(false);

		}
        //Begin 117975
        private void CbIncludeClosed_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            EnableDisableVisibleLogic("IncludeClosedEdit");
        }
        //End 117975
        void pbDriveThru_Click(object sender, PActionEventArgs e)
		{
			HandleRegSettings(true);
		}
		#endregion
		
		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 1)
			{
					if (paramList[0] != null)
						drawerCombo = paramList[0] as PComboBoxStandard;
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.Supervisor;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			base.InitParameters (paramList);
		}
		#endregion

		public override void OnFetchEditData()
		{
			if( MainBusinesObject != null )
			{
				_busObjDrawerBal.BranchNo.Value = _tellerVars.BranchNo;
				MainBusinesObject.ActionType = XmActionType.EnumOnly;
				MainBusinesObject.EnumType	=	XmEnumerationType.EnumAll;
				this.PerformAction(XmActionType.EnumOnly);
				this.IsNew = true;
			}			
		}

		public override void CallParent(params object[] paramList)
		{
			string caller = "";

			if ( paramList != null && paramList[0] != null )
				caller = Convert.ToString( paramList[0] );
			
			if ( caller == "DrawerMaint" )
			{
				PopulateGrid();
			}
		}


		#region supervisor events
		private ReturnType frmTlSupervisor_PInitBeginEvent()
		{
			try
			{
				this.MainBusinesObject = _busObjDrawerBal;
				this.AppToolBarId = AppToolBarType.NoEdit;

				#region Initialize
				this.dfPositionDt.UnFormattedValue = _tellerVars.PostingDt;
				AssignNextScreenId();
				#endregion

				#region populate combo box
				this.AutoFetch = true;
				this.IsNew = true;
				#endregion
			}
			catch (PhoenixException peBegin)
			{
				PMessageBox.Show(peBegin);
			}

			#region Fix No Security
			this.pbSearch.NextScreenId = 0;
			#endregion

			#region capture initial width property
			_employeeColWidth = gridTellerDrawers.Columns[gridTellerDrawers.Columns.GetColumnIndex("Employee")].Width;
			_unfwdTransactionsColWidth = gridTellerDrawers.Columns[gridTellerDrawers.Columns.GetColumnIndex("UnfwdTrans")].Width;
			#endregion

			return ReturnType.Success;
		}

		private void frmTlSupervisor_PInitCompleteEvent()
		{
			try
			{
				this.cmbBranch.Append(-1, GlobalVars.Instance.ML.All);
				//pbSearch.PerformClick();
				PopulateGrid();
				if (gridTellerDrawers.Items.Count > 0)
				{
					gridTellerDrawers.Focus();
					gridTellerDrawers.SelectRow(0);
				}
				EnableDisableVisibleLogic("FormComplete");

			}
			catch (PhoenixException peComp)
			{
				PMessageBox.Show(peComp);
			}
		}

		private void gridTellerDrawers_BeforePopulate(object sender, GridPopulateArgs e)
		{
//			_rowCount = 0;
		}

		private void gridTellerDrawers_FetchRowDone(object sender, GridRowArgs e)
		{
			#region ported from centura but not used so as centura
//			if (colDrawerStatus.Text.Trim() != GlobalVars.Instance.ML.Open)
//				colEmployee.Text = string.Empty;
//			if (colCrncyStatus.Text.Trim() == GlobalVars.Instance.ML.Open)
//			{
//				#region set drawer bal
//				colDrawerBalance.UnFormattedValue = Convert.ToDecimal(colClosingCash.UnFormattedValue) +
//					Convert.ToDecimal(colCashIn.UnFormattedValue) - 
//					Convert.ToDecimal(colCashOut.UnFormattedValue) -
//					Convert.ToDecimal(colUnbatchedAmt.UnFormattedValue);
//				#endregion
//
//				#region set empl
//				if (Convert.ToDateTime(colCurPostingDt.UnFormattedValue) < Convert.ToDateTime(dfPositionDt.UnFormattedValue))
//					colEmployee.Text = "*" + colEmployee.Text;
//				#endregion
//			}
//			else
//			{
//				colDrawerBalance.UnFormattedValue = colClosingCash.UnFormattedValue;
//				colNoTransactions.UnFormattedValue = null;
//			}
			#endregion
			
			//
			colDrawerBalance.UnFormattedValue = colClosingCash.UnFormattedValue;
//			colDrawerStatus.Text = colCrncyStatus.Text;

			if (colCurPostingDt.UnFormattedValue != null && Convert.ToDateTime(colCurPostingDt.UnFormattedValue) < Convert.ToDateTime(dfPositionDt.UnFormattedValue))
				colEmployee.Text = "*" + colEmployee.Text;

			if (colDrawerStatus.Text.Trim() == GlobalVars.Instance.ML.Open)
			{
				if (Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue) <= Convert.ToDateTime(dfPositionDt.UnFormattedValue))
				{
					openDrawer = true;
					fromDrawerBal = true;
				}
				else
				{
					fromDrawerBal = false;
					openDrawer = false;
				}

			}
			else
			{
				if (Convert.ToDateTime(colClosingDt.UnFormattedValue) < Convert.ToDateTime(dfPositionDt.UnFormattedValue))
				{
					fromDrawerBal = true;
					openDrawer = false;
				}
				else
				{
					fromDrawerBal = false;
					openDrawer = false;
				}
			}

			if (!openDrawer)
			{
				colEmployee.Text = string.Empty;
				colEmployeeId.UnFormattedValue = null;
				if (fromDrawerBal)
				{
					colNoTransactions.UnFormattedValue = null;
					colCurPostingDt.UnFormattedValue = colClosingDt.UnFormattedValue;
					colDrawerBalance.UnFormattedValue = colClosingCash.UnFormattedValue;
				}
			}
			else
			{
				if (fromDrawerBal)
				{
					#region set drawer bal
					colDrawerBalance.UnFormattedValue = Convert.ToDecimal(colClosingCash.UnFormattedValue) +
						Convert.ToDecimal(colCashIn.UnFormattedValue) - 
						Convert.ToDecimal(colCashOut.UnFormattedValue) -
						Convert.ToDecimal(colUnbatchedAmt.UnFormattedValue);
					#endregion
				}
				else
				{
					#region set drawer bal
					colDrawerBalance.UnFormattedValue = Convert.ToDecimal(colClosingCash.UnFormattedValue) +
						Convert.ToDecimal(colCashIn.UnFormattedValue) - 
						Convert.ToDecimal(colCashOut.UnFormattedValue);
					#endregion
				}
			}
			//Handle unfwd transactions count			
		}

		private void gridTellerDrawers_AfterPopulate(object sender, GridPopulateArgs e)
		{
//			EnableDisableVisibleLogic("AfterPopulate");
		}

		private void pbSearch_Click(object sender, PActionEventArgs e)
		{
			PopulateGrid();
		}

		private void pbJournal_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("TlJournal");
		}

		private void pbCTRReview_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("CTRReview");
		}

		void pbInventory_Click(object sender, PActionEventArgs e)   //#140772
		{
			CallOtherForms("Inventory");
		}

		private void pbTranTotals_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("TlTranTotals");
		}

		private void pbPosition_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("Position");
		}

		private void pbBranchPosition_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("BranchPosition");
		}

		private void pbDrawerMaint_Click(object sender, PActionEventArgs e)
		{
            //Begin Bug#116424
            int i = this.gridTellerDrawers.LastSelectedIndex;
            PopulateGrid();
            this.gridTellerDrawers.SelectRow(i);
            if (colEmployee.Text == null || colEmployee.Text == string.Empty || colEmployee.Text == "")
            {
                //"Refresh Supervisor window.  A closeout has already been performed on this drawer and unable to reassign drawer to an Employee".
                PMessageBox.Show(16382, MessageType.Error, MessageBoxButtons.OK, string.Empty);
            }
            else
            {
                CallOtherForms("DrMaint");
            }
            //CallOtherForms("DrMaint");
            //End Bug#116424
        }

		private void pbBulkMaint_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("BulkMaint");
		}

		#region #76409
		void pbProofTotals_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("ProofTotals");
		}
		#endregion

		private void cmbBranch_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			bAllSearch = false;
			if (cmbBranch.Description.Trim() == GlobalVars.Instance.ML.All)
				bAllSearch = true;
			EnableDisableVisibleLogic("BranchEdit");
		}

		private void gridTellerDrawers_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			GridClick();
		}
		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "AfterPopulate")
			{
				if (isAllSearch || Convert.ToInt16(cmbBranch.CodeValue) == _tellerVars.BranchNo)
				{
					//#71425 - Commented out changes added for issue#71195 as well set the visible property of colUnfwdTransactions false as it is not supported in .NET.
					//71195 - Added width to back to employee
					//colEmployee.Width = (colEmployee.Width + colUnfwdTransactions.Width);
//					colEmployee.Width = (_employeeColWidth + _unfwdTransactionsColWidth);
//					colUnfwdTransactions.Visible = false;
//					colUnfwdTransactions.Width = 0;
				}
				else if (_tellerVars.NetworkOffline )// || gbDualDatabases
				{
//					colEmployee.Width = _employeeColWidth;
//					colUnfwdTransactions.Width = _unfwdTransactionsColWidth;					
//					colUnfwdTransactions.Visible = true;
				}

				if (this.gridTellerDrawers.Items.Count <= 0)
				{
					pbJournal.Enabled = false;
					pbTranTotals.Enabled = false;
					pbPosition.Enabled = false;
					pbBranchPosition.Enabled = false;
					pbDrawerMaint.Enabled = false;
					pbCTRReview.Enabled = false;
				}
				if (this.gridTellerDrawers.Items.Count <= 0)
				{
					pbJournal.Enabled = true;
					pbTranTotals.Enabled = true;
					pbPosition.Enabled = true;
					pbBranchPosition.Enabled = true;
					pbDrawerMaint.Enabled = true;
					pbCTRReview.Enabled = true;
				}
			}
			else if (callerInfo == "BranchEdit")
			{
				pbSearch.Enabled = true;
				//
				if (bAllSearch)
					pbBranchPosition.Enabled = false;
				else
					pbBranchPosition.Enabled = true;
			}
			else if (callerInfo == "PositionDtEdit")
			{
				pbSearch.Enabled = true;
			}
            else if (callerInfo == "IncludeClosedEdit") //117975
            {
                pbSearch.Enabled = true; //117975b. Enable search on checkbox changed event
            }
            else if (callerInfo == "SearchClick")
			{
				if (bAllSearch)
					pbBranchPosition.Enabled = false;
				else
					pbBranchPosition.Enabled = true;
			}
			else if (callerInfo == "FormComplete")
			{
				pbSearch.Enabled = false;
				pbInventory.Enabled = ((CoreService.UIAccessProvider.GetScreenAccess(Phoenix.Shared.Constants.ScreenId.InventoryItemSearchTeller) & AuthorizationType.Read) == AuthorizationType.Read &&
									_tellerVars.IsInventoryTrackingEnabled);  //#140772

				#region Non supported actions
				this.pbFwdOffline.Visible = false;
				this.pbForceOffline.Visible = false;
				this.pbOffline.Visible = false;
				this.pbDirectPodLoad.Visible = false;
				this.pbResetPwd.Visible = false;
				this.pbPurge.Visible = false;
				this.pbDirectLoad.Visible = false;
				this.colUnfwdTransactions.Visible = false;

				if (_tellerVars.NetworkOffline)
					this.pbCreatePOD.Visible = true;
				else
					this.pbCreatePOD.Visible = false;

				#endregion

				//Begin #76033
				if (_tellerVars.IsHylandVoucherAvailable || _tellerVars.IsECMVoucherAvailable)  //#119479
				{
					string regSetting = HandleRegSettings(false);
					if (!string.IsNullOrEmpty(regSetting))
					{
						this.pbDriveThru.Visible = true;
						SetDriveThruLabel(regSetting);
					}
					else
						this.pbDriveThru.Visible = false;
				}
				else
					this.pbDriveThru.Visible = false;
				//End #76033
                this.pbBulkMaint.Visible = IsEnableBulkMaint();    //#140895
                this.pbPendingEOB.Visible = _tellerVars.AdTlControl.TellerCapture.Value == "Y";   // #140895, #28748
                this.pbApplTotals.Visible = _tellerVars.AdTlControl.TellerCapture.Value == "Y";     //#52625
			}
			else if (callerInfo == "GridClick")
			{
				if (colEmployee.Text == null || colEmployee.Text == string.Empty || colEmployee.Text == "")
				{
					pbDrawerMaint.Enabled = false;
					//this.pbBulkMaint.Enabled = false;   //#140895
				}
				else
				{
					pbDrawerMaint.Enabled = true;
					//this.pbBulkMaint.Enabled = IsEnableBulkMaint();    //#140895
				}

                // #140895 - Teller Capture Integration
                pbBulkMaint.Enabled = (IsEnableBulkMaint() && colPendingATVCBatches.UnFormattedValue != null);  //#30926 #30969

//				if (Convert.ToInt16(colUnfwdTransactions.UnFormattedValue) == 0 || colUnfwdTransactions.UnFormattedValue == null)
//					pbFwdOffline.Enabled = false;

			}
		}

		private bool IsEnableBulkMaint()   //#140895
		{
            return (!_tellerVars.AdTlControl.TellerCapture.IsNull && _tellerVars.AdTlControl.TellerCapture.Value == "Y");
		}

		private void CallXMThruCDS( string origin )
		{
			if (origin == "PopulateGrid")
			{
				this._busObjDrawerBal.BranchNo.Value = Convert.ToInt16(cmbBranch.CodeValue);
				this._busObjDrawerBal.OutputType.Value = 1;	
				this._busObjDrawerBal.ClosedDt.Value = Convert.ToDateTime(this.dfPositionDt.UnFormattedValue);
                this._busObjDrawerBal.IncludeClosed.Value = cbIncludeClosed.Value; //117975
                gridNode = Phoenix.FrameWork.CDS.DataService.Instance.GetListView( this._busObjDrawerBal, null);
				this.gridTellerDrawers.PopulateTable(gridNode,true);
				AfterGridPopulate();
			}
		}

		private void GridClick()
		{
			if (cmbBranch.Text.Trim() == GlobalVars.Instance.ML.All)
			{
//				if (gridTellerDrawers.ContextRow >= 0)
//					cmbBranch.CodeValue = colBranchNo.UnFormattedValue;
			}
			EnableDisableVisibleLogic("GridClick");

		}

		private void AfterGridPopulate()
		{
			EnableDisableVisibleLogic("AfterPopulate");
			this.gridTellerDrawers.SelectRow(0);
			EnableDisableVisibleLogic("GridClick");
		}

		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;

			if ( origin == "TlJournal" )
			{
				//Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
				tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournal");
				tempWin.InitParameters(GlobalVars.Instance.ML.Y, Convert.ToInt16(colBranchNo.UnFormattedValue), Convert.ToInt16(colDrawerNo.UnFormattedValue),
					Convert.ToDateTime(this.dfPositionDt.UnFormattedValue), colDrawerStatus.Text.Trim(),
					drawerCombo,
					false,
					Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue));

			}
			else if ( origin == "TlTranTotals" )
			{
				//Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
				tempWin = Helper.CreateWindow("phoenix.client.tltrantotal", "Phoenix.Client.TlTranTotal", "frmTlJournalTranTotals");
				tempWin.InitParameters(Convert.ToInt16(colBranchNo.UnFormattedValue), Convert.ToInt16(colDrawerNo.UnFormattedValue),
					Convert.ToDateTime(this.dfPositionDt.UnFormattedValue));
			}
			else if ( origin == "Position" )
			{
				//Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
				//(colDrCurPostingDt.UnFormattedValue == null? Convert.ToDateTime(colCurPostingDt.UnFormattedValue) : Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue)),
				tempWin = Helper.CreateWindow("phoenix.client.tlposition", "Phoenix.Client.TlPosition", "frmTlPosition");
				if (colEmployee.Text != null && colEmployee.Text != string.Empty)
				{
					if (colCurPostingDt.UnFormattedValue == null && colDrCurPostingDt.UnFormattedValue == null)
						tempWin.InitParameters(4, Convert.ToInt16(colBranchNo.UnFormattedValue), 
							Convert.ToInt16(colDrawerNo.UnFormattedValue),
							Convert.ToDateTime(this.dfPositionDt.UnFormattedValue),
							null,
							Convert.ToInt16(colEmployeeId.UnFormattedValue),
							null, false);
					else
						tempWin.InitParameters(4, Convert.ToInt16(colBranchNo.UnFormattedValue), 
							Convert.ToInt16(colDrawerNo.UnFormattedValue),
							(colCurPostingDt.UnFormattedValue == null? Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue) : Convert.ToDateTime(colCurPostingDt.UnFormattedValue)),
							null,
							Convert.ToInt16(colEmployeeId.UnFormattedValue),
							null, false);
				}
				else
				{
					if (colCurPostingDt.UnFormattedValue == null && colDrCurPostingDt.UnFormattedValue == null)
						tempWin.InitParameters(4, Convert.ToInt16(colBranchNo.UnFormattedValue), 
							Convert.ToInt16(colDrawerNo.UnFormattedValue),
							Convert.ToDateTime(this.dfPositionDt.UnFormattedValue),
							null,
							null,
							null, false);
					else
						tempWin.InitParameters(4, Convert.ToInt16(colBranchNo.UnFormattedValue), 
							Convert.ToInt16(colDrawerNo.UnFormattedValue),
							(colCurPostingDt.UnFormattedValue == null? Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue) : Convert.ToDateTime(colCurPostingDt.UnFormattedValue)),
							null,
							null,
							null, false);
				}
			}
			else if ( origin == "BranchPosition" )
			{
				//Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
				tempWin = Helper.CreateWindow("phoenix.client.tlposition", "Phoenix.Client.TlPosition", "frmTlPosition");
				tempWin.InitParameters(1, Convert.ToInt16(colBranchNo.UnFormattedValue), 
					null,
					Convert.ToDateTime(this.dfPositionDt.UnFormattedValue),
					null, 
					null,
					null, false);
			}
			else if ( origin == "DrMaint" )
			{
                //Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
                tempWin = Helper.CreateWindow("phoenix.client.tldrawermaintenance", "Phoenix.Client.TlDrawerMaint", "frmDrawerMaintenance");
				tempWin.InitParameters(Convert.ToInt16(colBranchNo.UnFormattedValue), 
					Convert.ToInt16(colDrawerNo.UnFormattedValue),
					Convert.ToInt16(colEmployeeId.UnFormattedValue),
					colEmployee.Text,
					Convert.ToDecimal(colDrawerPtid.UnFormattedValue),
					(colDrCurPostingDt.UnFormattedValue == null? Convert.ToDateTime(colCurPostingDt.UnFormattedValue) : Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue)));
			}
			else if (origin == "ProofTotals")   //#76409
			{
				tempWin = Helper.CreateWindow("phoenix.client.tlprooftotals", "phoenix.client.tlprooftotals", "frmTlProofTotals");
				tempWin.InitParameters(cmbBranch.CodeValue.ToString(), -1, this.dfPositionDt.Text); // #4366
			}
			else if ( origin == "CTRReview" )
			{
				//Replaced cmbBranch.CodeValue with colBranchNo.UnFormattedValue
				tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournal");
				tempWin.InitParameters(GlobalVars.Instance.ML.Y, Convert.ToInt16(colBranchNo.UnFormattedValue), 
					Convert.ToInt16(colDrawerNo.UnFormattedValue),
					Convert.ToDateTime(GlobalVars.SystemDate), 
					colDrawerStatus.Text,
					drawerCombo, true,Convert.ToDateTime(colDrCurPostingDt.UnFormattedValue));
			}
			else if (origin == "Inventory")
			{
				if (TellerVars.Instance.IsAppOnline)
				{
					tempWin = CreateWindow("phoenix.client.gbinventoryitem", "Phoenix.Client.Global", "frmInventoryItemSearch");
					tempWin.InitParameters(false, -1,
					-1,
					-1,
					colBranchNo.UnFormattedValue,
					colDrawerNo.UnFormattedValue);
				}
			}
			else if (origin == "BulkMaint")
			{
				tempWin = CreateWindow("Phoenix.Client.TlTranCode", "Phoenix.Client.Teller", "frmTlCaptureBulkTran");

				tempWin.InitParameters(
					null,
					colBranchNo.Text,
					colDrawerNo.Text,
					-1,
					"ViewOnly");
			}
            //Begin #140895, #28748
            else if (origin == "PendingEOB")
            {
                tempWin = CreateWindow("phoenix.client.tlsupervisor", "Phoenix.Client.Teller", "frmTlCaptureViewPendingEOB");
            }
            //End #140895, #28748

            //Begin #52625
            else if (origin == "ApplTotals")
            {
                tempWin = CreateWindow("phoenix.client.tlsupervisor", "Phoenix.Client.Teller", "frmTlCaptureApplTotals");
                tempWin.InitParameters(
                    Convert.ToInt16(colBranchNo.Text),
                    Convert.ToDateTime(this.dfPositionDt.UnFormattedValue));
            }
            //End #52625

			if ( tempWin != null )
			{
				tempWin.Workspace = this.Workspace;
				tempWin.Show();
			}
		}

		#region 76033 - changes
		//Begin #76033
		private string HandleRegSettings(bool save)
		{
			string keyValue = null;
			string computerClientName = System.Environment.GetEnvironmentVariable("CLIENTNAME"); //#5671
			if (string.IsNullOrEmpty(computerClientName)) //#5671
				computerClientName = System.Environment.GetEnvironmentVariable("COMPUTERNAME"); //#5671
			string keyName = "DriveThruWorkstation" + computerClientName; //#5671
			string location = Phoenix.Shared.Constants.ScreenId.TellerLogin.ToString();
			//
			#region read from registry
			if (!save)
			{
				if (Phoenix.Windows.Client.Helper.GetFromRegistry(location, keyName, ref keyValue))
				{
					if (keyValue != null && keyValue != String.Empty)
						return keyValue.Trim();
				}
			}
			#endregion
			//
			#region save to registry
			if (save)
			{
				string regSetting = HandleRegSettings(false);
				if (!string.IsNullOrEmpty(regSetting))
				{
					keyValue = (regSetting == GlobalVars.Instance.ML.Y ? GlobalVars.Instance.ML.N : GlobalVars.Instance.ML.Y);
					Phoenix.Windows.Client.Helper.SaveToRegistry(location, keyName, keyValue);
					_tellerVars.IsDriveThruWorkstation = (keyValue == GlobalVars.Instance.ML.Y ? 1 : 0);
					SetDriveThruLabel(keyValue);
				}
				return keyValue;
			}
			#endregion
			//
			return keyValue;
		}

		private void SetDriveThruLabel(string regSetting)
		{
			if (!string.IsNullOrEmpty(regSetting))
			{
				if (regSetting == GlobalVars.Instance.ML.Y)
				{
					this.pbDriveThru.MLInfo.ObjectId = 0;
					this.pbDriveThru.ObjectId = 0;
					this.pbDriveThru.ResetObject(43);
				}
				else if (regSetting == GlobalVars.Instance.ML.N)
				{
					this.pbDriveThru.MLInfo.ObjectId = 0;
					this.pbDriveThru.ObjectId = 0;
					this.pbDriveThru.ResetObject(44);
				}
			}
			//
			this.Refresh();
			this.UpdateView();
		}
		#endregion

		private void AssignNextScreenId()
		{			
			pbJournal.NextScreenId = Phoenix.Shared.Constants.ScreenId.Journal;
			pbTranTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalTranTotals;
			pbPosition.NextScreenId = Phoenix.Shared.Constants.ScreenId.SummaryPosition;
			pbBranchPosition.NextScreenId = Phoenix.Shared.Constants.ScreenId.SummaryPosition;
			pbDrawerMaint.NextScreenId = Phoenix.Shared.Constants.ScreenId.DrawerMaintenance;
			pbCTRReview.NextScreenId = Phoenix.Shared.Constants.ScreenId.Journal;
		}

		private void PopulateGrid()
		{
			using (new WaitCursor())
			{
				CallXMThruCDS("PopulateGrid");
			}
		}
		#endregion

		private void dfPositionDt_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			EnableDisableVisibleLogic("PositionDtEdit");
		}

		private void frmTlSupervisor_Load(object sender, EventArgs e)
		{

		}

        //Begin #140895, #28748
        private void pbPendingEOB_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("PendingEOB");
        }
        //End #140895, #28748

        //Begin #52625
        private void pbApplTotals_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("ApplTotals");
        }
        //End #52625
	}
}
