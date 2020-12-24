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
//12/19/2006	2		mselvaga	Issue#71197 - Adjusted column width for chargecode and amounts in offline.
//12/22/2006	3		mselvaga	Issue#71200 - Fixed ChargeEdited flag manipulation.
//01/03/2007	4		mselvaga	#71279 - Fixed charge amount reset problem for waived charge.
//01/04/2007	5		Vreddy		#71300 - New validation if charge is not avalized
//01/04/2007	6		mselvaga	#71299 - Anal charge for RIM Tran.
//08/03/2007	7		njoshi		#73557 - Changing Charges in Teller 2007 - charges are not updated when it's a zero charge
// 06Nov2009	8		GDiNatale	#6615 - SetValue Framework change
//11/08/2010    9       mselvaga    #79314 - Added remote override enh. changes.
//1/25/2013     10      Mbachala    19568 - 140769 -  Adding rewards to teller charges
//11/2/2017     11      RDeepthi    WI#75604. Teller Window. So always refer Decimal Config
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

namespace Phoenix.Client.TlCapturedItems
{
	/// <summary>
	/// Summary description for frmTlCharges.
	/// </summary>
	public class frmTlCharges : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGridColumn colIndex;
		private Phoenix.Windows.Forms.PGridColumn colChargeCode;
		private Phoenix.Windows.Forms.PGridColumn colNormalCCAmount;
		private Phoenix.Windows.Forms.PGridColumn colCCAdjAmount;
		private Phoenix.Windows.Forms.PGridColumn colCalcAmount;
		private Phoenix.Windows.Forms.PGridColumn colChargeAmount;
		private Phoenix.Windows.Forms.PGroupBoxStandard pGroupBoxStandard1;
		private Phoenix.Windows.Forms.PGrid gridCharges;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalChargeAmount;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalChargeAmt;
		//
		private Phoenix.Windows.Forms.PGridColumn colAnalCharge;
		private Phoenix.Windows.Forms.PGridColumnComboBox colAnalAcctType;
		private Phoenix.Windows.Forms.PGridColumn colAnalAcctNo;
		private Phoenix.Windows.Forms.PGridColumnCheckBox colChargeNow;
		private Phoenix.Windows.Forms.PGridColumn colChargeStatus;
		private Phoenix.Windows.Forms.PAction pbRemove;
		private Phoenix.Windows.Forms.PLabelStandard lblWaiveAnalCharge;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalWaivedAnalChgAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblRemovedAnalCharge;
		private Phoenix.Windows.Forms.PDfDisplay dfTotalRemovedAnalChgAmt;
		private Phoenix.Windows.Forms.PGridColumn colWaiveCharge;
		private Phoenix.Windows.Forms.PGridColumnComboBox colAnalAccount;
		private Phoenix.Windows.Forms.PGridColumn colIsAnalyzedAcct;

		#region Initialize
		private TellerVars _tellerVars = TellerVars.Instance;
		private TlTransactionSet _tlTranSet = null;
		private TlCc _busObjCharge = new TlCc();
		private TlHelper _tellerHelper = new TlHelper();
		private GbHelper _gbHelper = new GbHelper();
		//
		private PDecimal journalPtid = new PDecimal("JournalPtid");
		private ArrayList charges = new ArrayList();
		private bool isViewOnly = false;
        private PGridColumn colPkgAdjAmt;
		private bool isRimTran = false;
		#endregion

		public frmTlCharges()
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
            this.colIndex = new Phoenix.Windows.Forms.PGridColumn();
            this.colChargeCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colNormalCCAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colCCAdjAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colCalcAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colChargeAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTotalRemovedAnalChgAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblRemovedAnalCharge = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalWaivedAnalChgAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblWaiveAnalCharge = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblTotalChargeAmount = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTotalChargeAmt = new Phoenix.Windows.Forms.PDfDisplay();
            this.gridCharges = new Phoenix.Windows.Forms.PGrid();
            this.colWaiveCharge = new Phoenix.Windows.Forms.PGridColumn();
            this.colPkgAdjAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colIsAnalyzedAcct = new Phoenix.Windows.Forms.PGridColumn();
            this.colAnalCharge = new Phoenix.Windows.Forms.PGridColumn();
            this.colAnalAcctType = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colAnalAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAnalAccount = new Phoenix.Windows.Forms.PGridColumnComboBox();
            this.colChargeNow = new Phoenix.Windows.Forms.PGridColumnCheckBox();
            this.colChargeStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.pbRemove = new Phoenix.Windows.Forms.PAction();
            this.pGroupBoxStandard1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbRemove});
            // 
            // colIndex
            // 
            this.colIndex.PhoenixUIControl.ObjectId = 1;
            this.colIndex.PhoenixUIControl.XmlTag = "ChargeCode";
            this.colIndex.Title = "Index";
            this.colIndex.Visible = false;
            this.colIndex.Width = 0;
            // 
            // colChargeCode
            // 
            this.colChargeCode.PhoenixUIControl.ObjectId = 4;
            this.colChargeCode.PhoenixUIControl.XmlTag = "Description";
            this.colChargeCode.Title = "Charge Code";
            this.colChargeCode.Width = 158;
            // 
            // colNormalCCAmount
            // 
            this.colNormalCCAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNormalCCAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNormalCCAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNormalCCAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNormalCCAmount.PhoenixUIControl.ObjectId = 12;
            this.colNormalCCAmount.PhoenixUIControl.XmlTag = "CcNormalAmt";
            this.colNormalCCAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colNormalCCAmount.Title = "Normal CC Amount";
            this.colNormalCCAmount.Width = 62;
            // 
            // colCCAdjAmount
            // 
            this.colCCAdjAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCCAdjAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCCAdjAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCCAdjAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCCAdjAmount.PhoenixUIControl.ObjectId = 13;
            this.colCCAdjAmount.PhoenixUIControl.XmlTag = "CcAdjAmt";
            this.colCCAdjAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colCCAdjAmount.Title = "CC Adj Amount";
            this.colCCAdjAmount.Width = 62;
            // 
            // colCalcAmount
            // 
            this.colCalcAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCalcAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCalcAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCalcAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCalcAmount.PhoenixUIControl.ObjectId = 6;
            this.colCalcAmount.PhoenixUIControl.XmlTag = "CalcAmt";
            this.colCalcAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colCalcAmount.Title = "Calculated Amount";
            this.colCalcAmount.Width = 62;
            // 
            // colChargeAmount
            // 
            this.colChargeAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colChargeAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colChargeAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colChargeAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colChargeAmount.PhoenixUIControl.ObjectId = 7;
            this.colChargeAmount.PhoenixUIControl.XmlTag = "Amt";
            this.colChargeAmount.ReadOnly = false;
            this.colChargeAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colChargeAmount.Title = "Charge Amount";
            this.colChargeAmount.Width = 65;
            this.colChargeAmount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colChargeAmount_PhoenixUILeaveEvent);
            this.colChargeAmount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colChargeAmount_PhoenixUIValidateEvent);
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.dfTotalRemovedAnalChgAmt);
            this.pGroupBoxStandard1.Controls.Add(this.lblRemovedAnalCharge);
            this.pGroupBoxStandard1.Controls.Add(this.dfTotalWaivedAnalChgAmt);
            this.pGroupBoxStandard1.Controls.Add(this.lblWaiveAnalCharge);
            this.pGroupBoxStandard1.Controls.Add(this.lblTotalChargeAmount);
            this.pGroupBoxStandard1.Controls.Add(this.dfTotalChargeAmt);
            this.pGroupBoxStandard1.Controls.Add(this.gridCharges);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 0);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(684, 444);
            this.pGroupBoxStandard1.TabIndex = 0;
            this.pGroupBoxStandard1.TabStop = false;
            // 
            // dfTotalRemovedAnalChgAmt
            // 
            this.dfTotalRemovedAnalChgAmt.Location = new System.Drawing.Point(228, 416);
            this.dfTotalRemovedAnalChgAmt.Multiline = true;
            this.dfTotalRemovedAnalChgAmt.Name = "dfTotalRemovedAnalChgAmt";
            this.dfTotalRemovedAnalChgAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalRemovedAnalChgAmt.PhoenixUIControl.ObjectId = 21;
            this.dfTotalRemovedAnalChgAmt.Size = new System.Drawing.Size(88, 20);
            this.dfTotalRemovedAnalChgAmt.TabIndex = 6;
            // 
            // lblRemovedAnalCharge
            // 
            this.lblRemovedAnalCharge.AutoEllipsis = true;
            this.lblRemovedAnalCharge.Location = new System.Drawing.Point(4, 416);
            this.lblRemovedAnalCharge.Name = "lblRemovedAnalCharge";
            this.lblRemovedAnalCharge.PhoenixUIControl.ObjectId = 21;
            this.lblRemovedAnalCharge.Size = new System.Drawing.Size(220, 20);
            this.lblRemovedAnalCharge.TabIndex = 5;
            this.lblRemovedAnalCharge.Text = "Total Removed Analysis Charge Amount:";
            // 
            // dfTotalWaivedAnalChgAmt
            // 
            this.dfTotalWaivedAnalChgAmt.Location = new System.Drawing.Point(228, 392);
            this.dfTotalWaivedAnalChgAmt.Multiline = true;
            this.dfTotalWaivedAnalChgAmt.Name = "dfTotalWaivedAnalChgAmt";
            this.dfTotalWaivedAnalChgAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalWaivedAnalChgAmt.PhoenixUIControl.ObjectId = 20;
            this.dfTotalWaivedAnalChgAmt.Size = new System.Drawing.Size(88, 20);
            this.dfTotalWaivedAnalChgAmt.TabIndex = 4;
            // 
            // lblWaiveAnalCharge
            // 
            this.lblWaiveAnalCharge.AutoEllipsis = true;
            this.lblWaiveAnalCharge.Location = new System.Drawing.Point(4, 392);
            this.lblWaiveAnalCharge.Name = "lblWaiveAnalCharge";
            this.lblWaiveAnalCharge.PhoenixUIControl.ObjectId = 20;
            this.lblWaiveAnalCharge.Size = new System.Drawing.Size(220, 20);
            this.lblWaiveAnalCharge.TabIndex = 3;
            this.lblWaiveAnalCharge.Text = "Total Waived Analysis Charge Amount:";
            // 
            // lblTotalChargeAmount
            // 
            this.lblTotalChargeAmount.AutoEllipsis = true;
            this.lblTotalChargeAmount.Location = new System.Drawing.Point(428, 392);
            this.lblTotalChargeAmount.Name = "lblTotalChargeAmount";
            this.lblTotalChargeAmount.PhoenixUIControl.ObjectId = 9;
            this.lblTotalChargeAmount.Size = new System.Drawing.Size(156, 20);
            this.lblTotalChargeAmount.TabIndex = 1;
            this.lblTotalChargeAmount.Text = "Total  Posted Charge Amount:";
            // 
            // dfTotalChargeAmt
            // 
            this.dfTotalChargeAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChargeAmt.Location = new System.Drawing.Point(588, 392);
            this.dfTotalChargeAmt.Multiline = true;
            this.dfTotalChargeAmt.Name = "dfTotalChargeAmt";
            this.dfTotalChargeAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalChargeAmt.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTotalChargeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfTotalChargeAmt.PhoenixUIControl.ObjectId = 9;
            this.dfTotalChargeAmt.Size = new System.Drawing.Size(88, 20);
            this.dfTotalChargeAmt.TabIndex = 2;
            // 
            // gridCharges
            // 
            this.gridCharges.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colIndex,
            this.colWaiveCharge,
            this.colChargeCode,
            this.colNormalCCAmount,
            this.colCCAdjAmount,
            this.colPkgAdjAmt,
            this.colCalcAmount,
            this.colIsAnalyzedAcct,
            this.colChargeAmount,
            this.colAnalCharge,
            this.colAnalAcctType,
            this.colAnalAcctNo,
            this.colAnalAccount,
            this.colChargeNow,
            this.colChargeStatus});
            this.gridCharges.IsMaxNumRowsCustomized = false;
            this.gridCharges.LinesInHeader = 2;
            this.gridCharges.Location = new System.Drawing.Point(4, 12);
            this.gridCharges.Name = "gridCharges";
            this.gridCharges.Size = new System.Drawing.Size(676, 372);
            this.gridCharges.TabIndex = 0;
            this.gridCharges.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridCharges_BeforePopulate);
            this.gridCharges.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridCharges_FetchRowDone);
            this.gridCharges.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridCharges_AfterPopulate);
            this.gridCharges.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridCharges_SelectedIndexChanged);
            // 
            // colWaiveCharge
            // 
            this.colWaiveCharge.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colWaiveCharge.PhoenixUIControl.ObjectId = 14;
            this.colWaiveCharge.PhoenixUIControl.XmlTag = "WaiveCharge";
            this.colWaiveCharge.Title = "Waive Charge";
            this.colWaiveCharge.Visible = false;
            // 
            // colPkgAdjAmt
            // 
            this.colPkgAdjAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPkgAdjAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPkgAdjAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPkgAdjAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPkgAdjAmt.PhoenixUIControl.XmlTag = "PkgAdjAmt";
            this.colPkgAdjAmt.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.colPkgAdjAmt.Title = "Pkg Adj Amount";
            this.colPkgAdjAmt.Width = 62;
            // 
            // colIsAnalyzedAcct
            // 
            this.colIsAnalyzedAcct.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colIsAnalyzedAcct.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colIsAnalyzedAcct.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colIsAnalyzedAcct.PhoenixUIControl.XmlTag = "0";
            this.colIsAnalyzedAcct.Title = "Analyzed";
            this.colIsAnalyzedAcct.Visible = false;
            // 
            // colAnalCharge
            // 
            this.colAnalCharge.PhoenixUIControl.ObjectId = 14;
            this.colAnalCharge.PhoenixUIControl.XmlTag = "Analysized";
            this.colAnalCharge.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.colAnalCharge.Title = "Analysis Charge";
            this.colAnalCharge.Width = 47;
            // 
            // colAnalAcctType
            // 
            this.colAnalAcctType.AutoDrop = false;
            this.colAnalAcctType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAnalAcctType.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.colAnalAcctType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAnalAcctType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colAnalAcctType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAnalAcctType.PhoenixUIControl.ObjectId = 15;
            this.colAnalAcctType.PhoenixUIControl.XmlTag = "AnalAcctType";
            this.colAnalAcctType.ReadOnly = false;
            this.colAnalAcctType.Title = "Analysis Acct Type";
            this.colAnalAcctType.Visible = false;
            this.colAnalAcctType.Width = 63;
            this.colAnalAcctType.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAnalAcctType_PhoenixUIValidateEvent);
            // 
            // colAnalAcctNo
            // 
            this.colAnalAcctNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAnalAcctNo.PhoenixUIControl.ObjectId = 16;
            this.colAnalAcctNo.PhoenixUIControl.XmlTag = "AnalAcctNo";
            this.colAnalAcctNo.ReadOnly = false;
            this.colAnalAcctNo.Title = "Analysis Acct Number";
            this.colAnalAcctNo.Visible = false;
            this.colAnalAcctNo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAnalAcctNo_PhoenixUIValidateEvent);
            // 
            // colAnalAccount
            // 
            this.colAnalAccount.AutoDrop = false;
            this.colAnalAccount.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAnalAccount.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.colAnalAccount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colAnalAccount.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colAnalAccount.PhoenixUIControl.ObjectId = 22;
            this.colAnalAccount.PhoenixUIControl.XmlTag = "AnalAccount";
            this.colAnalAccount.ReadOnly = false;
            this.colAnalAccount.Title = "Analysis Account";
            this.colAnalAccount.Width = 120;
            this.colAnalAccount.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colAnalAccount_PhoenixUILeaveEvent);
            this.colAnalAccount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.colAnalAccount_PhoenixUIValidateEvent);
            // 
            // colChargeNow
            // 
            this.colChargeNow.Checked = false;
            this.colChargeNow.PhoenixUIControl.ObjectId = 17;
            this.colChargeNow.PhoenixUIControl.XmlTag = "ChargeNow";
            this.colChargeNow.ReadOnly = false;
            this.colChargeNow.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.colChargeNow.Title = "Charge Now";
            this.colChargeNow.Width = 41;
            this.colChargeNow.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.colChargeNow_PhoenixUILeaveEvent);
            this.colChargeNow.PhoenixUIClickedEvent += new Phoenix.Windows.Forms.ClickedEventHandler(this.colChargeNow_PhoenixUIClickedEvent);
            this.colChargeNow.PhoenixUIActivating += new Phoenix.Windows.Forms.ValidateEventHandler(this.colChargeNow_PhoenixUIActivating);
            // 
            // colChargeStatus
            // 
            this.colChargeStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colChargeStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colChargeStatus.PhoenixUIControl.ObjectId = 18;
            this.colChargeStatus.PhoenixUIControl.XmlTag = "ChargeStatus";
            this.colChargeStatus.Title = "Status";
            this.colChargeStatus.Width = 55;
            // 
            // pbRemove
            // 
            this.pbRemove.LongText = "Remove";
            this.pbRemove.ObjectId = 19;
            this.pbRemove.ShortText = "Remove";
            this.pbRemove.Tag = null;
            this.pbRemove.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRemove_Click);
            // 
            // frmTlCharges
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.pGroupBoxStandard1);
            this.Name = "frmTlCharges";
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCharges_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCharges_PInitCompleteEvent);
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.pGroupBoxStandard1.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 2)
			{
				journalPtid.Value = Convert.ToDecimal(paramList[0]);
				if (paramList[1] != null)
				{
					//charges = (ArrayList)paramList[1];
					_tlTranSet = (TlTransactionSet)paramList[1];
					charges = _tlTranSet.CurTran.Charges;
					isRimTran = _tellerHelper.IsRIMTran(_tlTranSet.CurTran.TranCode.Value);
				}

				if (charges.Count == 0)
					isViewOnly = true;
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.Charges;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			this.colChargeAmount.ReadOnly = isViewOnly;
			//
			this.colAnalCharge.ReadOnly = true;
			this.colAnalAccount.ReadOnly = isViewOnly;
			this.colChargeNow.ReadOnly = isViewOnly;
			this.colAnalAcctType.ReadOnly = isViewOnly;
			this.colAnalAcctNo.ReadOnly = isViewOnly;
			this.colChargeStatus.ReadOnly = true;

			base.InitParameters (paramList);
		}

		#endregion

		#region standard actions
		public override bool OnActionClose()
		{
			if (!isViewOnly)
			{
				LoadCharges(true);
				CallParent( "Charges" );
			}
			else
			{
				return true;
			}
			return base.OnActionClose ();
		}
		#endregion

		#region charges events
		private ReturnType frmTlCharges_PInitBeginEvent()
		{
            //Begin 75604
            #region config
            PdfCurrency.ApplicationAssumeDecimal = (_tellerVars.AdTlControl.AssumeDecimals.Value == GlobalVars.Instance.ML.Y);
            CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
            #endregion
            //End 75604
            this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = _busObjCharge;
			return ReturnType.Success;
		}

		private void frmTlCharges_PInitCompleteEvent()
		{
			if (!isViewOnly)
			{
				LoadCharges(false);
				if (!_tellerVars.IsAppOnline)
				{
					colAnalAcctType.CodeValue = null;					
					PopulateComboBox( "AnalAcctType" );
				}
				else
				{
					this._busObjCharge.RimNo.IsNullable = false;
					colAnalAccount.CodeValue = null;
					PopulateComboBox( "AnalAccount" );
				}
				CalcTotal();
			}
			if (gridCharges.Items.Count > 0 && !isViewOnly)
				gridCharges.SelectRow(0,true);
			EnableDisableVisibleLogic("FormComplete");
			EnableDisableVisibleLogic("RemoveAction"); //#72097
		}

		private void gridCharges_FetchRowDone(object sender, GridRowArgs e)
		{
			if (isViewOnly)
			{
				if (colIndex.Text != string.Empty && colChargeCode.Text != string.Empty)
					colChargeCode.Text = colIndex.Text + " - " + colChargeCode.Text;
			}
			if (colWaiveCharge.Text != string.Empty)
				colWaiveCharge.Text = colWaiveCharge.Text.Trim();
			if (colChargeNow.Text != string.Empty)
				colChargeNow.Text = colChargeNow.Text.Trim();
			if (colChargeStatus.Text != string.Empty)
				colChargeStatus.Text = colChargeStatus.Text.Trim();
			if (colChargeNow.Text == GlobalVars.Instance.ML.Y)
				colChargeNow.Checked = true;
			else
				colChargeNow.Checked = false;
			
		}

		private void gridCharges_AfterPopulate(object sender, GridPopulateArgs e)
		{
			if (isViewOnly)
				CalcTotal();
		}

		private void pbRemove_Click(object sender, PActionEventArgs e)
		{
			if (colAnalCharge.Text != string.Empty && colAnalCharge.Text == GlobalVars.Instance.ML.Y)
			{
				SetAnalyzedAcctStatus();
				if (colIsAnalyzedAcct.Text == GlobalVars.Instance.ML.Y)
				{
					if (DialogResult.Yes == PMessageBox.Show(this, 360781, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
					{
						this.colChargeStatus.Text = GlobalVars.Instance.ML.Closed;
						this.colChargeStatus.UnFormattedValue = GlobalVars.Instance.ML.Closed;
						this.colChargeAmount.UnFormattedValue = 0;
						ValidateChargeAmount();
						//
						CalcTotal();
					}
					else
						return;
				}
			}
			EnableDisableVisibleLogic("RemoveAction"); //#72097
		}

		private void gridCharges_BeforePopulate(object sender, GridPopulateArgs e)
		{
			if (isViewOnly)
			{
				_busObjCharge.JournalPtid.Value = journalPtid.IntValue;
				_busObjCharge.OutputTypeId.Value = 1;
				gridCharges.ListViewObject = _busObjCharge;
			}
		}

		private void gridCharges_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			if (!isViewOnly)
			{
				ResetChargeNowFieldAccess();
				EnableDisableVisibleLogic("RemoveAction");
			}
		}

		private void colAnalAcctType_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!_tellerVars.IsAppOnline)
			{
				SetMask( colAnalAcctType , colAnalAcctNo );
				SetAcctNoField( colAnalAcctNo, colAnalAcctType );
			}
			ResetChargeNowFieldAccess();
		}

		private void colChargeNow_PhoenixUIActivating(object sender, PCancelEventArgs e)
		{
			ResetChargeNowFieldAccess();
			SetAnalyzedAcctStatus();
		}


		private void colAnalAccount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			//71300 - Added this validation
			if ((colAnalAccount.Text != null && colAnalAccount.Text != string.Empty) && colAnalCharge.Text != null && colAnalCharge.Text == "N")
			{
				PMessageBox.Show(this, 360832, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				colAnalAccount.CodeValue = null;
				colAnalAccount.UnFormattedValue = null;
				colAnalAccount.Text = string.Empty;				
				e.Cancel = true;
				return;
			}
			if (_busObjCharge.AnalAccount.Constraint.Values.GetDescriptionIndex(colAnalAccount.Text) >= 0 ||
				colAnalAccount.Text == string.Empty)
			{
				ResetAnalAccountFieldAccess();			
				ResetChargeStatusValue();
			}
			else
				e.Cancel = true;
		}

		private void colAnalAccount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			SetAnalyzedAcctStatus();
			ValidateChargeNow();
			CalcTotal();
		}

		private void colChargeAmount_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			ValidateChargeAmount();
			LoadCharges(true);
			CalcTotal();
		}

		private void colAnalAcctNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (!_tellerVars.IsAppOnline)							
				SetAcctNoField( colAnalAcctNo, colAnalAcctType );
			ResetChargeNowFieldAccess();
		}

		private void colChargeNow_PhoenixUILeaveEvent(object sender, PEventArgs e)
		{
			CalcTotal();
		}

		private void colChargeNow_PhoenixUIClickedEvent(object sender, EventArgs e)
		{
			//do this to force the value for calcTotal
			gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colChargeNow.ColumnId, (colChargeNow.Checked? GlobalVars.Instance.ML.Y : GlobalVars.Instance.ML.N));
			//
			ValidateChargeNow();
			//ValidateChargeAmount();
			CalcTotal();
		}

		private void colChargeAmount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			SetAnalyzedAcctStatus();
			if (colChargeAmount.UnFormattedValue == null || colChargeAmount.Text == string.Empty)
				colChargeAmount.UnFormattedValue = 0;
			if (colIsAnalyzedAcct.Text == GlobalVars.Instance.ML.Y && colAnalCharge.Text == GlobalVars.Instance.ML.Y &&
				colChargeStatus.Text == GlobalVars.Instance.ML.Closed)
			{
				if (Convert.ToDecimal(colChargeAmount.UnFormattedValue) > 0)
				{
					PMessageBox.Show(this, 360782, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					colChargeAmount.UnFormattedValue = 0;
					return;
				}
			}
		}

		#endregion

		#region private methods
		private void LoadCharges(bool isLoadBackList)
		{

			if (!isLoadBackList)
			{
				gridCharges.ResetTable();

				foreach( TlCc charge in charges )
				{
					// M-Start
					gridCharges.AddNewRow();
					colIndex.Text = charge.ChargeCode.Value.ToString();
					colChargeCode.Text = charge.ChargeCode.Value.ToString() + " - " + charge.Description.Value;
					gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colNormalCCAmount.ColumnId, (charge.CcNormalAmt.IsNull? Convert.ToDecimal(0) : charge.CcNormalAmt.Value) );
					gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colCCAdjAmount.ColumnId, (charge.CcAdjAmt.IsNull? Convert.ToDecimal(0) : charge.CcAdjAmt.Value));
					gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colCalcAmount.ColumnId, (charge.CalcAmt.IsNull? Convert.ToDecimal(0) : charge.CalcAmt.Value));
					gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colChargeAmount.ColumnId, (charge.Amt.IsNull? Convert.ToDecimal(0) : charge.Amt.Value));
                    gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colPkgAdjAmt.ColumnId, (charge.PkgAdjAmt.IsNull ? Convert.ToDecimal(0) : charge.PkgAdjAmt.Value)); //19568
					//
					if (_tellerVars.IsAppOnline && !charge.AnalAccount.IsNull)
					{
						colAnalAccount.CodeValue = charge.AnalAccount.Value;
						colAnalAccount.Text = charge.AnalAccount.Value;
					}
					if (!charge.Analysized.IsNull)
						gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colAnalCharge.ColumnId, charge.Analysized.Value );
					if (!charge.ChargeNow.IsNull)
						gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colChargeNow.ColumnId, charge.ChargeNow.Value );
					if (!charge.AnalAcctType.IsNull)
						gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colAnalAcctType.ColumnId, charge.AnalAcctType.Value );
					if (!charge.AnalAcctNo.IsNull)
						gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colAnalAcctNo.ColumnId, charge.AnalAcctNo.Value );
					if (!charge.ChargeStatus.IsNull)
						gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colChargeStatus.ColumnId, charge.ChargeStatus.Value );
					//workaround
					colWaiveCharge.UnFormattedValue = charge.Waive.Value;
					if (charge.ChargeNow.Value == GlobalVars.Instance.ML.Y)
					{
						colChargeNow.Checked = true;
					}
					else
					{
						colChargeNow.Checked = false;
					}
					SetAnalyzedAcctStatus();
					//
					if (charge.Analysized.Value == GlobalVars.Instance.ML.Y && colIsAnalyzedAcct.Text == GlobalVars.Instance.ML.Y)
					{
						if (charge.ChargeStatus.IsNull)
							gridCharges.SetCellValueUnFormatted(gridCharges.ContextRow, colChargeStatus.ColumnId, GlobalVars.Instance.ML.Active );
					}
					else
					{
						charge.ChargeNow.SetValue(null, EventBehavior.None);		// #6615 - charge.ChargeNow.SetValueToNull();
					}
				}
			}
			else
			{	
				bool isAnalAcctEdited = false;
				short tmpChargeCode = 0;
				string tmpAnalAcctType = null;
				string tmpAnalAcctNo = null;
				string tmpAnalAccount = null;
				string tmpChargeNow = null;
				string tmpChargeStatus = null;
				decimal tmpChargeAmt = 0;
				for( int rowId = 0; rowId < gridCharges.Count; rowId++ )				
				{
					object objectValue = gridCharges.GetCellValueUnformatted( rowId, colIndex.ColumnId );
					if (objectValue != null)
						tmpChargeCode = Convert.ToInt16(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colAnalAcctType.ColumnId );
					if (objectValue != null)
						tmpAnalAcctType = Convert.ToString(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colAnalAcctNo.ColumnId );
					if (objectValue != null)
						tmpAnalAcctNo = Convert.ToString(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colAnalAccount.ColumnId );
					if (objectValue != null)
						tmpAnalAccount = Convert.ToString(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeNow.ColumnId );
					if (objectValue != null)
						tmpChargeNow = Convert.ToString(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeStatus.ColumnId );
					if (objectValue != null)
						tmpChargeStatus = Convert.ToString(objectValue);
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeAmount.ColumnId );
					if (objectValue != null)
						tmpChargeAmt = Convert.ToDecimal(objectValue);
					foreach(TlCc charge in charges)
					{
						if (charge.ChargeCode.Value == tmpChargeCode)
						{
							if (_tellerVars.IsAppOnline)
							{
								if (charge.AnalAccount.Value != tmpAnalAccount)
									isAnalAcctEdited = true;
								else
									isAnalAcctEdited = false;							
							}
							else
							{
								if (charge.AnalAcctType.Value != tmpAnalAcctType || 
									charge.AnalAcctNo.Value != tmpAnalAcctNo)
									isAnalAcctEdited = true;
								else
									isAnalAcctEdited = false;
							}

							if (charge.Amt.Value != tmpChargeAmt ||
								charge.ChargeNow.Value != tmpChargeNow ||
								charge.ChargeStatus.Value != tmpChargeStatus ||
								isAnalAcctEdited)
							{
								//issue# 73557 changed '> 0' to '>=0'
								if (!charge.CalcAmt.IsNull && charge.CalcAmt.Value >= 0)
									charge.ChargeEdited.Value = 1;
								charge.Amt.ValueObject = gridCharges.GetCellValueUnformatted( rowId, colChargeAmount.ColumnId );
								charge.ChargeNow.ValueObject = gridCharges.GetCellValueUnformatted( rowId, colChargeNow.ColumnId );
								charge.ChargeStatus.ValueObject = gridCharges.GetCellValueUnformatted( rowId, colChargeStatus.ColumnId );
								//								colAnalAccount.ColumnToField(charge.AnalAccount);
								//workaround for non-db fields
								if (_tellerVars.IsAppOnline)
									charge.AnalAccount.Value = gridCharges.Items[rowId].SubItems[colAnalAccount.ColumnId].Text;
								objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeNow.ColumnId );
								if (objectValue != null)
								{
									if (Convert.ToString(objectValue) == GlobalVars.Instance.ML.Y)
										charge.ChargeNow.Value = GlobalVars.Instance.ML.Y;
									else
										charge.ChargeNow.Value = GlobalVars.Instance.ML.N;
								}
								if (!charge.AnalAccount.IsNull && _tellerVars.IsAppOnline)
								{
									string tempAcct = charge.AnalAccount.Value;
									if (tempAcct.Length > 4)
									{
										charge.AnalAcctType.Value = tempAcct.Substring(0,3);
										charge.AnalAcctNo.Value = tempAcct.Substring(4,tempAcct.Length - 4);
									}
								}
								else
								{
									charge.AnalAcctType.ValueObject = gridCharges.GetCellValueUnformatted( rowId, colAnalAcctType.ColumnId );
									charge.AnalAcctNo.ValueObject = gridCharges.GetCellValueUnformatted( rowId, colAnalAcctNo.ColumnId );
								}
								//#71200 - commented out - the charge amount change does not reflect in input transactions
//								if (isRimTran && charge.CalcAmt.Value > 0 && charge.Amt.Value == 0 && charge.ChargeNow.Value == GlobalVars.Instance.ML.N)
//									charge.ChargeEdited.Value = 0;
							}
							break;
						}
					}
				}			
			}
		}

		private void ValidateChargeAmount()
		{
			if (!isViewOnly)
			{
				ResetChargeNowFieldAccess();
				SetAnalyzedAcctStatus();
				if (colIsAnalyzedAcct.Text == GlobalVars.Instance.ML.Y)
				{
					if (colAnalCharge.Text == GlobalVars.Instance.ML.Y)
					{
//						if (colWaiveCharge.Text == GlobalVars.Instance.ML.Y)
//						{
//							if (!colChargeNow.Checked && Convert.ToDecimal(colChargeAmount.UnFormattedValue) > 0)
//								colChargeNow.Checked = true;
//							else
//							{
//								if ((Convert.ToDecimal(colChargeAmount.UnFormattedValue) == 0 || 
//									colChargeAmount.UnFormattedValue == null) && colChargeNow.Checked)
//									colChargeNow.Checked = false;
//							}
//						}
//						else
//						{
							if (Convert.ToDecimal(colChargeAmount.UnFormattedValue) > 0)
								colChargeNow.Checked = true;
							else
								colChargeNow.Checked = false;
//						}
					}
				}
			}
		}

		private void ValidateChargeNow()
		{
			if (!isViewOnly)
			{
				ResetChargeNowFieldAccess();
				SetAnalyzedAcctStatus();
				if (colIsAnalyzedAcct.Text == GlobalVars.Instance.ML.Y)
				{
					if (colAnalCharge.Text == GlobalVars.Instance.ML.Y)
					{
						if (colWaiveCharge.Text == GlobalVars.Instance.ML.Y)
						{
							//#71279 - do not flip the value in case of non-zero
							if (colChargeNow.Checked)
							{
								if (Convert.ToDecimal(colChargeAmount.UnFormattedValue) == 0 || colChargeAmount.UnFormattedValue == null)
									colChargeAmount.UnFormattedValue = Convert.ToDecimal(colCalcAmount.UnFormattedValue);
							}
							else
								colChargeAmount.UnFormattedValue = 0;
						}
						else //#71299
						{
							if (colChargeAmount.UnFormattedValue != null && Convert.ToDecimal(colChargeAmount.UnFormattedValue) > 0)
								colChargeNow.Checked = true;
							else
								colChargeNow.Checked = false;
						}
					}
				}
			}
		}

		private void CalcTotal()
		{
			dfTotalChargeAmt.UnFormattedValue = 0;
			decimal totalPostedChgAmt = 0;
			decimal totalWaivedAnalChgAmt = 0;
			decimal totalRemovedAnalChgAmt = 0;
			decimal chgAmt = 0;
			bool isWaiveChgAmt = false;
			bool isAnalyzedAcct = false;
			bool isChargeNow = false;
			bool isAnalCharge = false;
			bool isStatusClosed = false;
			object objectValue = null;
			//
			for( int rowId = 0; rowId < gridCharges.Count; rowId++ )
			{
				objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeAmount.ColumnId );
				if (objectValue == null)
					chgAmt = 0;
				else
					chgAmt = Convert.ToDecimal(objectValue);
				//
				isWaiveChgAmt = (gridCharges.GetCellValueFormatted(rowId, colWaiveCharge.ColumnId) == GlobalVars.Instance.ML.Y);
				isAnalyzedAcct = (gridCharges.GetCellValueFormatted(rowId, colIsAnalyzedAcct.ColumnId) == GlobalVars.Instance.ML.Y);
				objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeNow.ColumnId );
				if (objectValue == null)
					isChargeNow = false;
				else
					isChargeNow = (gridCharges.GetCellValueUnformatted(rowId, colChargeNow.ColumnId).ToString() == GlobalVars.Instance.ML.Y);
				isAnalCharge = (gridCharges.GetCellValueFormatted(rowId, colAnalCharge.ColumnId) == GlobalVars.Instance.ML.Y);
				objectValue = gridCharges.GetCellValueUnformatted( rowId, colChargeStatus.ColumnId );
				if (objectValue == null)
					isStatusClosed = false;
				else
					isStatusClosed = (gridCharges.GetCellValueUnformatted(rowId, colChargeStatus.ColumnId).ToString() == GlobalVars.Instance.ML.Closed);
				if (isRimTran || isViewOnly)
				{
					isAnalyzedAcct = (gridCharges.Items[rowId].SubItems[colAnalAccount.ColumnId].Text != string.Empty);
				}
				//
				if (isWaiveChgAmt || isStatusClosed)
				{
					objectValue = gridCharges.GetCellValueUnformatted( rowId, colCalcAmount.ColumnId );
					decimal calcAmt =  0;
					if( objectValue != null )
						calcAmt = Convert.ToDecimal(objectValue);  
					if (chgAmt == 0 && calcAmt > 0 && isAnalyzedAcct)
						chgAmt = calcAmt;
				}
				if( isAnalyzedAcct )
				{
					if( isStatusClosed )
						totalRemovedAnalChgAmt = totalRemovedAnalChgAmt + chgAmt;
					else if ( isAnalCharge == false || isChargeNow || isWaiveChgAmt == false )
						totalPostedChgAmt = totalPostedChgAmt + chgAmt; 
					else if ( isAnalCharge && isWaiveChgAmt && isChargeNow == false && isStatusClosed == false )
						totalWaivedAnalChgAmt = totalWaivedAnalChgAmt + chgAmt;
				}
				else
				{
					totalPostedChgAmt = totalPostedChgAmt + chgAmt;
				}
			}
			dfTotalChargeAmt.UnFormattedValue = totalPostedChgAmt;
			dfTotalWaivedAnalChgAmt.UnFormattedValue = totalWaivedAnalChgAmt;
			dfTotalRemovedAnalChgAmt.UnFormattedValue = totalRemovedAnalChgAmt;
		}


		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
			{
				if (isViewOnly)
				{
					this.pbRemove.Visible = false;
				}
				colAnalAccount.Visible = (_tellerVars.IsAppOnline);
				colAnalAcctType.Visible = (!_tellerVars.IsAppOnline);
				colAnalAcctNo.Visible = (!_tellerVars.IsAppOnline);
				//
				if (!isViewOnly)
				{
					if (!isRimTran && _tlTranSet.CurTran.TranAcct.Analysis.Value != GlobalVars.Instance.ML.Y && _tellerVars.IsAppOnline)
					{
						colAnalAccount.ReadOnly = true;
						colAnalAcctType.ReadOnly = true;
						colAnalAcctNo.ReadOnly = true;
						colChargeNow.ReadOnly = true;
						pbRemove.Visible = false;
					}
					if (!_tellerVars.IsAppOnline) //new change for offline
					{
						this.colChargeCode.Width = this.colChargeCode.Width + 130;
						this.colNormalCCAmount.Width = this.colNormalCCAmount.Width + 30;
						this.colCCAdjAmount.Width = this.colCCAdjAmount.Width + 30;
						this.colCalcAmount.Width = this.colCalcAmount.Width + 30;
						this.colChargeAmount.Width = this.colChargeAmount.Width + 30;
						this.colAnalCharge.Visible = false;
						this.colAnalAcctType.Visible = false;
						this.colAnalAcctNo.Visible = false;
						this.colChargeNow.Visible = false;
						this.colChargeStatus.Visible = false;
						this.pbRemove.Visible = false;
						
					}
				}
                //
                ResetFormForSupViewOnlyMode();  //#79314
			}
			else if (callerInfo == "RemoveAction")
			{
				//#72097
				if (!isViewOnly && _tellerVars.IsAppOnline)
				{
					if (colAnalCharge.Text != GlobalVars.Instance.ML.Y || colChargeStatus.Text == GlobalVars.Instance.ML.Closed)
						this.pbRemove.Enabled = false;
					else if (colAnalCharge.Text == GlobalVars.Instance.ML.Y && !isRimTran && 
						(_tlTranSet.CurTran.TranAcct.Analysis.Value != GlobalVars.Instance.ML.Y  ||
						_tlTranSet.CurTran.TranAcct.IncludeTlrCharge.Value != GlobalVars.Instance.ML.Y))
						this.pbRemove.Enabled = false;
					else if (colAnalCharge.Text == GlobalVars.Instance.ML.Y && isRimTran && 
						(colAnalAccount.Text == String.Empty || colAnalAccount.CodeValue == null))
						this.pbRemove.Enabled = false;
					else
						this.pbRemove.Enabled = true;
				}
			}

		}

		private void CallXMThruCDS(string origin)
		{
			if ( origin == "AnalAccountPopulate" )
			{
				this._busObjCharge.AnalAccount.Value = null;
				Phoenix.FrameWork.CDS.DataService.Instance.EnumValues( this._busObjCharge, _busObjCharge.AnalAccount );
			}
			if (origin == "AnalAcctTypePopulate")
			{
				Phoenix.FrameWork.CDS.DataService.Instance.EnumValues( this._tellerHelper, this._tellerHelper.AcctType );
			}
		}

		private void PopulateComboBox(string comboName)
		{
			using( new WaitCursor())
			{
				#region combo populate
				try
				{
					if (comboName == "AnalAccount")
					{

						if (isRimTran)
						{
							_busObjCharge.RimNo.Value = _tlTranSet.CurTran.TranAcct.RimNo.Value;
							CallXMThruCDS("AnalAccountPopulate");
							this.colAnalAccount.PopulateFromField(_busObjCharge.AnalAccount);
						}
						else
						{
							//71300 - Do No populate With accounts
							
//							if (colAnalCharge.Text != null && colAnalCharge.Text.Trim() == "Y" && 
//								(!_tlTranSet.CurTran.TranAcct.IncludeTlrCharge.IsNull && _tlTranSet.CurTran.TranAcct.IncludeTlrCharge.Value == "Y") &&
//								_tlTranSet.CurTran.TranAcct.Analysis.Value == "Y")
							if ( (!_tlTranSet.CurTran.TranAcct.IncludeTlrCharge.IsNull && 
								_tlTranSet.CurTran.TranAcct.IncludeTlrCharge.Value == "Y") &&
								_tlTranSet.CurTran.TranAcct.Analysis.Value == "Y")
							{
								this.colAnalAccount.Append(_tlTranSet.CurTran.TranAcct.AcctType.Value + "-" + _tlTranSet.CurTran.TranAcct.AcctNo.Value,
									_tlTranSet.CurTran.TranAcct.AcctType.Value + "-" + _tlTranSet.CurTran.TranAcct.AcctNo.Value);
							}
							else if (colAnalCharge.Text != null && colAnalCharge.Text.Trim() != "Y")
							{
								colAnalAccount.CodeValue = null;
								colAnalAccount.Text = string.Empty;
								colAnalAccount.UnFormattedValue = null;
							}
						}
					}
					else if (comboName == "AnalAcctType")
					{
						PopulateAcctTypeCombo( colAnalAcctType );
					}
				}
				catch (PhoenixException pecbpop)
				{
					PMessageBox.Show(pecbpop);
				}
				#endregion
			}
		}

		private void ResetChargeNowFieldAccess()
		{
			if (_tellerVars.IsAppOnline && colAnalAccount.CodeValue == null)
				colChargeNow.ReadOnly = true;
			else if (!_tellerVars.IsAppOnline && (colAnalAcctType.CodeValue == null ||
				colAnalAcctNo.Text == null || colAnalAcctNo.Text == string.Empty))
				colChargeNow.ReadOnly = true;
			else
			{
				if (colWaiveCharge.Text == GlobalVars.Instance.ML.Y)
					colChargeNow.ReadOnly = false;
				else
					colChargeNow.ReadOnly = true;
			}
		}

		private void ResetChargeStatusValue()
		{
			if (_tellerVars.IsAppOnline && isRimTran)
			{
				if (colAnalAccount.CodeValue == null)
					colChargeStatus.Text = string.Empty;
				else
				{
					if (colChargeStatus.Text == null || colChargeStatus.Text == string.Empty)
						colChargeStatus.Text = GlobalVars.Instance.ML.Active;
				}
			}
		}

		private void ResetAnalAccountFieldAccess()
		{
			if (colAnalCharge.Text != GlobalVars.Instance.ML.Y && _tellerVars.IsAppOnline)
			{
				colAnalAccount.Text = string.Empty;
				colAnalAccount.CodeValue = null;
			}
		}

		private void SetAnalyzedAcctStatus()
		{
			if (isRimTran)
			{	
				//#71299
//				if ((_tellerVars.IsAppOnline && colAnalAccount.CodeValue != null) ||
//					(!_tellerVars.IsAppOnline && colAnalAcctType.CodeValue != null &&
//					colAnalAcctNo.Text != string.Empty))
//					colIsAnalyzedAcct.Text = GlobalVars.Instance.ML.Y;
				if (_tellerVars.IsAppOnline && colAnalAccount.CodeValue != null &&
					colAnalAccount.Text != string.Empty)
					colIsAnalyzedAcct.Text = GlobalVars.Instance.ML.Y;
				else
					colIsAnalyzedAcct.Text = GlobalVars.Instance.ML.N;
			}
			else
			{
				colIsAnalyzedAcct.Text = _tlTranSet.CurTran.TranAcct.Analysis.Value;
			}
			//#72097
			if (_tellerVars.IsAppOnline && isRimTran)
				EnableDisableVisibleLogic("RemoveAction");

		}

        #region #79314
        private void ResetFormForSupViewOnlyMode()
        {
            if (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled &&
                Phoenix.Windows.Client.Helper.IsWorkspaceReadOnly(this.Workspace))
            {
                MakeReadOnly(true);
                foreach (PAction action in ActionManager.Actions)
                {
                    if (!(action == ActionClose))
                        action.Enabled = false;
                }
            }
        }
        #endregion

		#endregion

		#region accout methods
		private bool SetAcctFields( string acctType, string acctNo, PGridColumn acctNoField,
			PGridColumnComboBox acctTypeField )
		{
			bool retValue = false;
			acctTypeField.CodeValue = acctType;
			if ( acctTypeField.SelectedIndex >= 0 )
			{
				SetMask( acctTypeField, acctNoField );
				if ( acctNo != null && acctNo != String.Empty )
					retValue = SetAcctNoField( acctNoField, acctTypeField );
			}
			return retValue;
		}

		private bool SetAcctNoField( PGridColumn acctNoField,
			PGridColumnComboBox acctTypeField )
		{
			string applType = null;
			string depLoan = null;
			string format = null;
			string acctNo = acctNoField.Text;

			try
			{
				if ( acctNo != null && acctNo != string.Empty && acctNo != "" && acctTypeField.Text != null )
				{
					GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );
					if ( format != null && format != String.Empty )
					{
						acctNoField.UnFormattedValue = _tellerHelper.FormatAccount( acctNo, format );
					}
					else
						acctNoField.UnFormattedValue = acctNo;
				}
				return true;
			}
			catch(PhoenixException pe)
			{
				PMessageBox.Show(pe);
				return false;
			}
		}

		private string GetDepLoan( PGridColumnComboBox acctTypeField )
		{
			string applType = null;
			string depLoan = null;
			string format = null;

			GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );
			return depLoan;
		}

		private void PopulateAcctTypeCombo( PGridColumnComboBox acctTypeField )
		{

			try
			{
				bool acctTypeSrch = (this._tellerVars.AdTlControl.AcctTypeSearch.Value == GlobalVars.Instance.ML.Y );

				if (  this._tellerVars.ComboCache["frmTlCharges.colAnalAcctType"] == null )
				{
					this._tellerHelper.TlWhereClause.Value = ( acctTypeSrch ? "Y" : "N" );
					this._tellerHelper.FilterByDepLoan.Value = "'" + GlobalVars.Instance.ML.DP + "'";
					CallXMThruCDS( "AnalAcctTypePopulate" );
					this._tellerVars.ComboCache["frmTlCharges.colAnalAcctType"] = this._tellerHelper.AcctType.Constraint.EnumValues;
				}
				acctTypeField.Append( this._tellerVars.ComboCache["frmTlCharges.colAnalAcctType"]);
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void SetMask( PGridColumnComboBox acctTypeField, PGridColumn acctNoField )
		{
			try
			{
				string applType = null;
				string depLoan = null;
				string format = null;

				GetAcctTypeDetails( acctTypeField, ref applType, ref depLoan, ref format );

				if ( format != null && format != string.Empty )
					acctNoField.PhoenixUIControl.InputMask = format;
				else
					acctNoField.PhoenixUIControl.InputMask = String.Empty;
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}

		}

		private void GetAcctTypeDetails( PGridColumnComboBox acctTypeField,
			ref string applType, ref string depLoan, ref string format )
		{

			try
			{
				string[] applDetails = null;
				// hack
				if ( acctTypeField.PhoenixUIControl.BusObjectProperty == null )
					acctTypeField.PhoenixUIControl.BusObjectProperty = this._tellerHelper.AcctType;

				if ( acctTypeField.CodeValue == null || acctTypeField.Text == "" || acctTypeField.Text == String.Empty )
				{
					return;
				}

				applDetails = acctTypeField.GetEnumValue(acctTypeField.Text).Description.Split("~".ToCharArray());
				applType = applDetails[0];
				depLoan = applDetails[1];
				format = applDetails[2];
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}
		#endregion
	}
}
