#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
//ScreenId = 16004
#endregion
//-------------------------------------------------------------------------------
// File Name: frmTlEditFailedRIMTransaction.cs
// NameSpace: Phoenix.Client...
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//07/13/2006	1		Vreddy		Issue#67883 - Created.
//12/10/2006	2		Vreddy		#71004 - 71003
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Shared.Variables;
//using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Teller;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Admin;
using Phoenix.BusObj.RIM;
using Phoenix.Shared.Variables;

namespace Phoenix.Client.TlForward
{
	/// <summary>
	/// Summary description for frmTlEditFailedRIMTransaction.
	/// </summary>
	public class frmTlEditFailedRIMTransaction : Phoenix.Windows.Forms.PfwStandard
	{

		#region Programmer Declared Variables
		private bool _passRecordBack = false;
		private Phoenix.BusObj.Global.GbHelper _gbHelperBusObj;
		private Phoenix.BusObj.RIM.RmAcct _rmAcctBusObj;
		private Phoenix.BusObj.Teller.TlJournal _tlJournalBusObj;
		private Phoenix.BusObj.Admin.Teller.AdTlTc _adTlTcBusObj;
		private int _searchRimNo;
		private string _searchTinNo;
		private string _searchRimType;
		private string _searchMiddleInitial;
		private string _searchFirstName;
		private string _searchLastName;
		private string _originalrimNo;
		#endregion 
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		private Phoenix.Windows.Forms.PLabelStandard lblRim;
		private Phoenix.Windows.Forms.PdfStandard dfRIM;
		private Phoenix.Windows.Forms.PLabelStandard lblName;
		private Phoenix.Windows.Forms.PdfStandard dfName;
		private Phoenix.Windows.Forms.PdfStandard dfAccountNo;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbNewTranInfo;
		private Phoenix.Windows.Forms.PdfStandard dfOldRIM;
		private Phoenix.Windows.Forms.PLabelStandard lblOldRim;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbOldTransactionInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblTranCode;
		private Phoenix.Windows.Forms.PdfStandard dfTranCode;
		private Phoenix.Windows.Forms.PLabelStandard lblAmount;
		private Phoenix.Windows.Forms.PAction pbReturn;
		private Phoenix.Windows.Forms.PAction pbGetRIM;
		private Phoenix.Windows.Forms.PdfStandard dfOldAcctNo;
		private Phoenix.Windows.Forms.PLabelStandard lblTwoDash1;
		private Phoenix.Windows.Forms.PLabelStandard dfTwoDash2;
		private Phoenix.Windows.Forms.PLabelStandard dfTwoDash3;
		private Phoenix.Windows.Forms.PdfStandard dfTCDesc;
		private Phoenix.Windows.Forms.PdfCurrency dfAmt;

		public frmTlEditFailedRIMTransaction()
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
			this.lblRim = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfRIM = new Phoenix.Windows.Forms.PdfStandard();
			this.lblName = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfName = new Phoenix.Windows.Forms.PdfStandard();
			this.dfAccountNo = new Phoenix.Windows.Forms.PdfStandard();
			this.lblTwoDash1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbNewTranInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfOldRIM = new Phoenix.Windows.Forms.PdfStandard();
			this.lblOldRim = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbOldTransactionInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfTCDesc = new Phoenix.Windows.Forms.PdfStandard();
			this.dfTwoDash3 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfTwoDash2 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfOldAcctNo = new Phoenix.Windows.Forms.PdfStandard();
			this.dfAmt = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblAmount = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfTranCode = new Phoenix.Windows.Forms.PdfStandard();
			this.lblTranCode = new Phoenix.Windows.Forms.PLabelStandard();
			this.pbReturn = new Phoenix.Windows.Forms.PAction();
			this.pbGetRIM = new Phoenix.Windows.Forms.PAction();
			this.gbNewTranInfo.SuspendLayout();
			this.gbOldTransactionInformation.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionManager
			// 
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
																						this.pbReturn,
																						this.pbGetRIM});
			// 
			// lblRim
			// 
			this.lblRim.Location = new System.Drawing.Point(8, 20);
			this.lblRim.Name = "lblRim";
			this.lblRim.Size = new System.Drawing.Size(72, 20);
			this.lblRim.TabIndex = 0;
			this.lblRim.Text = "RIM";
			// 
			// dfRIM
			// 
			this.dfRIM.Location = new System.Drawing.Point(84, 16);
			this.dfRIM.Name = "dfRIM";
			this.dfRIM.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfRIM.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
			this.dfRIM.PhoenixUIControl.ObjectId = 2;
			this.dfRIM.Size = new System.Drawing.Size(56, 20);
			this.dfRIM.TabIndex = 1;
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 44);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(72, 20);
			this.lblName.TabIndex = 4;
			this.lblName.Text = "Name:";
			// 
			// dfName
			// 
			this.dfName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfName.Location = new System.Drawing.Point(84, 44);
			this.dfName.Name = "dfName";
			this.dfName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfName.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfName.PhoenixUIControl.ObjectId = 4;
			this.dfName.Size = new System.Drawing.Size(348, 20);
			this.dfName.TabIndex = 5;
			this.dfName.TabStop = false;
			// 
			// dfAccountNo
			// 
			this.dfAccountNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.dfAccountNo.Location = new System.Drawing.Point(160, 16);
			this.dfAccountNo.MaxLength = 10;
			this.dfAccountNo.Name = "dfAccountNo";
			this.dfAccountNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.dfAccountNo.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.Enable;
			this.dfAccountNo.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
			this.dfAccountNo.PhoenixUIControl.ObjectId = 3;
			this.dfAccountNo.Size = new System.Drawing.Size(272, 20);
			this.dfAccountNo.TabIndex = 3;			
			this.dfAccountNo.TextChanged += new System.EventHandler(this.dfAccountNo_TextChanged);
			this.dfAccountNo.PhoenixUIValidateEvent += new ValidateEventHandler(dfAccountNo_PhoenixUIValidateEvent);
			// 
			// lblTwoDash1
			// 
			this.lblTwoDash1.Location = new System.Drawing.Point(144, 16);
			this.lblTwoDash1.Name = "lblTwoDash1";
			this.lblTwoDash1.Size = new System.Drawing.Size(12, 20);
			this.lblTwoDash1.TabIndex = 2;
			this.lblTwoDash1.Text = "--";
			// 
			// gbNewTranInfo
			// 
			this.gbNewTranInfo.Controls.Add(this.lblRim);
			this.gbNewTranInfo.Controls.Add(this.dfRIM);
			this.gbNewTranInfo.Controls.Add(this.lblName);
			this.gbNewTranInfo.Controls.Add(this.dfName);
			this.gbNewTranInfo.Controls.Add(this.dfAccountNo);
			this.gbNewTranInfo.Controls.Add(this.lblTwoDash1);
			this.gbNewTranInfo.Location = new System.Drawing.Point(4, 0);
			this.gbNewTranInfo.Name = "gbNewTranInfo";
			this.gbNewTranInfo.PhoenixUIControl.ObjectId = 1;
			this.gbNewTranInfo.Size = new System.Drawing.Size(682, 72);
			this.gbNewTranInfo.TabIndex = 0;
			this.gbNewTranInfo.TabStop = false;
			this.gbNewTranInfo.Text = "New Transaction Information";
			// 
			// dfOldRIM
			// 
			this.dfOldRIM.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfOldRIM.Location = new System.Drawing.Point(80, 16);
			this.dfOldRIM.MaxLength = 50;
			this.dfOldRIM.Name = "dfOldRIM";
			this.dfOldRIM.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfOldRIM.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfOldRIM.PhoenixUIControl.ObjectId = 6;
			this.dfOldRIM.Size = new System.Drawing.Size(56, 20);
			this.dfOldRIM.TabIndex = 1;
			// 
			// lblOldRim
			// 
			this.lblOldRim.Location = new System.Drawing.Point(8, 16);
			this.lblOldRim.Name = "lblOldRim";
			this.lblOldRim.Size = new System.Drawing.Size(68, 20);
			this.lblOldRim.TabIndex = 0;
			this.lblOldRim.Text = "RIM:";
			// 
			// gbOldTransactionInformation
			// 
			this.gbOldTransactionInformation.Controls.Add(this.dfTCDesc);
			this.gbOldTransactionInformation.Controls.Add(this.dfTwoDash3);
			this.gbOldTransactionInformation.Controls.Add(this.dfTwoDash2);
			this.gbOldTransactionInformation.Controls.Add(this.dfOldAcctNo);
			this.gbOldTransactionInformation.Controls.Add(this.dfAmt);
			this.gbOldTransactionInformation.Controls.Add(this.lblAmount);
			this.gbOldTransactionInformation.Controls.Add(this.dfTranCode);
			this.gbOldTransactionInformation.Controls.Add(this.lblTranCode);
			this.gbOldTransactionInformation.Controls.Add(this.lblOldRim);
			this.gbOldTransactionInformation.Controls.Add(this.dfOldRIM);
			this.gbOldTransactionInformation.Location = new System.Drawing.Point(4, 72);
			this.gbOldTransactionInformation.Name = "gbOldTransactionInformation";
			this.gbOldTransactionInformation.PhoenixUIControl.ObjectId = 5;
			this.gbOldTransactionInformation.Size = new System.Drawing.Size(682, 92);
			this.gbOldTransactionInformation.TabIndex = 1;
			this.gbOldTransactionInformation.TabStop = false;
			this.gbOldTransactionInformation.Text = "Old Transaction Information";
			// 
			// dfTCDesc
			// 
			this.dfTCDesc.Location = new System.Drawing.Point(156, 40);
			this.dfTCDesc.Name = "dfTCDesc";
			this.dfTCDesc.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfTCDesc.PhoenixUIControl.ObjectId = 12;
			this.dfTCDesc.Size = new System.Drawing.Size(276, 20);
			this.dfTCDesc.TabIndex = 7;
			// 
			// dfTwoDash3
			// 
			this.dfTwoDash3.Location = new System.Drawing.Point(140, 40);
			this.dfTwoDash3.Name = "dfTwoDash3";
			this.dfTwoDash3.Size = new System.Drawing.Size(12, 20);
			this.dfTwoDash3.TabIndex = 6;
			this.dfTwoDash3.Text = "--";
			// 
			// dfTwoDash2
			// 
			this.dfTwoDash2.Location = new System.Drawing.Point(140, 20);
			this.dfTwoDash2.Name = "dfTwoDash2";
			this.dfTwoDash2.Size = new System.Drawing.Size(12, 20);
			this.dfTwoDash2.TabIndex = 2;
			this.dfTwoDash2.Text = "--";
			// 
			// dfOldAcctNo
			// 
			this.dfOldAcctNo.Location = new System.Drawing.Point(156, 16);
			this.dfOldAcctNo.Name = "dfOldAcctNo";
			this.dfOldAcctNo.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfOldAcctNo.PhoenixUIControl.ObjectId = 13;
			this.dfOldAcctNo.Size = new System.Drawing.Size(276, 20);
			this.dfOldAcctNo.TabIndex = 3;
			// 
			// dfAmt
			// 
			this.dfAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfAmt.Location = new System.Drawing.Point(80, 64);
			this.dfAmt.Name = "dfAmt";
			this.dfAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfAmt.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfAmt.PhoenixUIControl.MaxPrecision = 14;
			this.dfAmt.PhoenixUIControl.ObjectId = 9;
			this.dfAmt.Size = new System.Drawing.Size(104, 20);
			this.dfAmt.TabIndex = 9;
			this.dfAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblAmount
			// 
			this.lblAmount.Location = new System.Drawing.Point(8, 64);
			this.lblAmount.Name = "lblAmount";
			this.lblAmount.Size = new System.Drawing.Size(68, 20);
			this.lblAmount.TabIndex = 8;
			this.lblAmount.Text = "Amount:";
			// 
			// dfTranCode
			// 
			this.dfTranCode.Location = new System.Drawing.Point(80, 40);
			this.dfTranCode.Name = "dfTranCode";
			this.dfTranCode.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
			this.dfTranCode.PhoenixUIControl.ObjectId = 8;
			this.dfTranCode.Size = new System.Drawing.Size(56, 20);
			this.dfTranCode.TabIndex = 5;
			// 
			// lblTranCode
			// 
			this.lblTranCode.Location = new System.Drawing.Point(8, 40);
			this.lblTranCode.Name = "lblTranCode";
			this.lblTranCode.Size = new System.Drawing.Size(68, 20);
			this.lblTranCode.TabIndex = 4;
			this.lblTranCode.Text = "Tran Code:";
			// 
			// pbReturn
			// 
			this.pbReturn.ObjectId = 10;
			this.pbReturn.ShowHiddenActions = false;
			this.pbReturn.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReturn_Click);
			// 
			// pbGetRIM
			// 
			this.pbGetRIM.ObjectId = 11;
			this.pbGetRIM.ShowHiddenActions = false;
			this.pbGetRIM.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbGetRIM_Click);
			// 
			// frmTlEditFailedRIMTransaction
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.gbOldTransactionInformation);
			this.Controls.Add(this.gbNewTranInfo);
			this.Name = "frmTlEditFailedRIMTransaction";
			
			this.Closed += new System.EventHandler(this.frmTlEditFailedRIMTransaction_Closed);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlEditFailedRIMTransaction_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlEditFailedRIMTransaction_PInitCompleteEvent);
			this.gbNewTranInfo.ResumeLayout(false);
			this.gbOldTransactionInformation.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		#region InitParameters
		public override void InitParameters(params object[] paramList)
		{
			_tlJournalBusObj = new TlJournal();
			_adTlTcBusObj = new Phoenix.BusObj.Admin.Teller.AdTlTc();
			_gbHelperBusObj =new GbHelper();
			
			if (paramList.Length == 1 && paramList[0] != null)
			{
				try
				{
					_tlJournalBusObj = paramList[0] as Phoenix.BusObj.Teller.TlJournal;
				}
				catch(PhoenixException ex)
				{
					CoreService.LogPublisher.LogDebug("\n"+ "Some Parameter misshap" + ex.Message + "\n" +  ex.InnerException.Message);
					return;
				}
			}			
			//Needs some explanation
			this.AutoFetch = false;
			_originalrimNo = _tlJournalBusObj.AcctNo.StringValue;

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.TlEditFailedRIMTransaction;
			base.InitParameters (paramList);			
		}
		#endregion InitParameters

		#region CallParent		

		public override void CallParent(params object[] paramList)
		{
			string paramLocal;
			int closingScreenId = -1;
			//CallParent(colRimNo.Text, colRimType.Text, colTINUnFormatted.Text, colFirstName.Text, colLastName.Text, colMiddleInit.Text, colAcctType.Text, colAcctNo.Text); //
			if (paramList.Length > 0) // Some thing returned from 
			{
				closingScreenId = Convert.ToInt32(paramList[0]);
				if (closingScreenId == Shared.Constants.ScreenId.TBarSearchRim)
				{
					paramLocal = Convert.ToString(paramList[1]);
					#region RimNo - paramList[1]
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							this._searchRimNo = Convert.ToInt32(paramLocal);
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Some Junk Must be there:" + ex.Message + "\n" + ex.InnerException);
						}
					}
					else
					{
						_searchRimNo = 0;
					}
					#endregion 

					#region Rim Type - paramList[2]
					paramLocal = Convert.ToString(paramList[2]);
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							this._searchRimType = paramLocal.Trim();
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("RimType:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
						}
					}
					else
					{
						_searchRimType = "";
					}
					#endregion 

					#region TIN Unfarmatted - paramList[3]
					paramLocal = Convert.ToString(paramList[3]);
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							this._searchTinNo = paramLocal.Trim();
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Tin:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
						}
					}
					#endregion 
				
					#region First Name - paramList[4]
					paramLocal = Convert.ToString(paramList[4]);
					_searchFirstName = "";
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							_searchFirstName = paramLocal.Trim();
						
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("First Name:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
						}
					}

					#endregion 

					#region Last Name - paramList[5]
					paramLocal = Convert.ToString(paramList[5]);
					_searchLastName = "";
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							_searchLastName = paramLocal.Trim();
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Last Name:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
						}
					}
					#endregion 

					#region Middle Initial - paramList[6]
					paramLocal = Convert.ToString(paramList[6]);
					_searchMiddleInitial = "";
					if 	(paramLocal != string.Empty && paramLocal.Trim().Length > 0)			
					{
						try
						{
							this._searchMiddleInitial = paramLocal.Trim();
						}
						catch(PhoenixException ex)
						{
							//We do not want to catch anything
							Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Middle Ini:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
						}
					}
					#endregion 
					
					if (_searchRimType == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Personal)
						dfName.Text = _gbHelperBusObj.ConcateNameX(_searchFirstName, _searchLastName, _searchMiddleInitial, false, string.Empty, string.Empty);
					else
						dfName.Text = _gbHelperBusObj.ConcateNameX(_searchFirstName, _searchLastName, string.Empty, true, string.Empty, string.Empty);

					dfAccountNo.UnFormattedValue = _searchRimNo;

				} //End of Search Screen Returns
			}
			return;
			//base.CallParent (paramList);
		}
		#endregion CallParent

		#region Form Events

		#region OnClosing
		protected override void OnClosing(CancelEventArgs e)
		{	//On Action close fire after this guy, So I had to repeat the code
			if (_passRecordBack)
			{
				if (!SendRimBack())
					e.Cancel = true;
			}
			base.OnClosing (e);
		}
		//
		//
		public override bool OnActionClose()
		{
			//Do not do it Twice
			if (!_passRecordBack)
			{
				if (!SendRimBack())
					return false;
			}
			return base.OnActionClose ();
		}
		//
		private bool SendRimBack()
		{
			if (!_passRecordBack)
			{
				if ( _originalrimNo.Trim() != dfAccountNo.Text.Trim())
				{
					if (dfAccountNo.Text.Trim().Length > 0)
					{
						//360653 - You have entered a new RIM account for the selected transaction, are you sure you wish to close the window without returning your changes?
						if (DialogResult.No == PMessageBox.Show(this, 360653, MessageType.Message, MessageBoxButtons.YesNo, string.Empty))
						{
							dfAccountNo.Focus();
							return false;
						}
					}
				}
			}
			return true;
		}
		//
		#endregion OnClosing

		#region frmTlEditFailedRIMTransaction_PInitBeginEvent
		private ReturnType frmTlEditFailedRIMTransaction_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.NoEdit;

			//this.MainBusinesObject = this._gbMemoBusObj;

			#region Screen Security
			pbGetRIM.NextScreenId = 0; //No Screen Id
			pbReturn.NextScreenId = 0;
			#endregion Screen Security

			return ReturnType.Success;
		}
		#endregion Form Events

		#region frmTlEditFailedRIMTransaction_PInitCompleteEvent
		private void frmTlEditFailedRIMTransaction_PInitCompleteEvent()
		{
			dfRIM.UnFormattedValue = Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM;
			EnableDisableVisibleLogic(EnableDisableVisible.PbReturn);
			//Set the Form Values...
			if ( _tlJournalBusObj.OfflineAcctNo.IsNull )
			{
				dfOldRIM.UnFormattedValue = _tlJournalBusObj.AcctType.Value;
				dfOldAcctNo.UnFormattedValue = _tlJournalBusObj.AcctNo.Value;
			}
			else
			{
				dfOldRIM.UnFormattedValue = _tlJournalBusObj.OfflineAcctType.Value;
				dfOldAcctNo.UnFormattedValue = _tlJournalBusObj.OfflineAcctNo.Value;
			}
			dfTranCode.UnFormattedValue = _tlJournalBusObj.TranCode.Value;
			dfAmt.UnFormattedValue = _tlJournalBusObj.NetAmt.Value;
			CallXMThruCDS(XMThruCDSOrigin.GetTranCodeDesc);
			dfTCDesc.UnFormattedValue = _adTlTcBusObj.Description.Value;
			//dfAccountNo.Focus();
			//this.Workspace.ResetStaus();			
		}
		#endregion frmTlEditFailedRIMTransaction_PInitCompleteEvent

		#endregion Form Events

		#region Control Events

		#region dfAccountNo_TextChanged
		private void dfAccountNo_TextChanged(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic(EnableDisableVisible.PbReturn);
		}
		#endregion dfAccountNo_TextChanged

		#region dfAccountNo_PhoenixUIValidateEvent
		private void dfAccountNo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			if (dfAccountNo.Text.Length > 0) //Validate only if there is a value
			{
				if (!RiseErrorMessage())
					e.Cancel = true;
			}
		}
		#endregion dfAccountNo_PhoenixUIValidateEvent

		#endregion Control Events

		#region PushButton Events

		#region pbGetRIM_Click
		private void pbGetRIM_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms(InvokeForm.RimSearchWindow);
		}
		#endregion pbGetRIM_Click

		#region npbReturn_Click
		private void pbReturn_Click(object sender, PActionEventArgs e)
		{
			if (!RiseErrorMessage())
				return;
			if ( _tlJournalBusObj.OfflineAcctNo.IsNull )
			{
				_tlJournalBusObj.OfflineAcctNo.Value = _tlJournalBusObj.AcctNo.Value;
				_tlJournalBusObj.OfflineAcctType.Value = _tlJournalBusObj.AcctType.Value;
			}
			_tlJournalBusObj.AcctNo.Value = this.dfAccountNo.Text.Trim();
			_tlJournalBusObj.AcctType.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM;
			CallXMThruCDS( XMThruCDSOrigin.UpdateRimNo );
			_passRecordBack = true;
			PfwStandard parentForm = this.Workspace.ContentWindow.PreviousWindow as PfwStandard;
			if (_passRecordBack) //Comming from Return Button		
				this.Close();			
		}
		#endregion pbReturn_Click

		#region frmTlEditFailedRIMTransaction_Closed
		private void frmTlEditFailedRIMTransaction_Closed(object sender, EventArgs e)
		{

//			PfwStandard parentForm = this.Workspace.ContentWindow.PreviousWindow as PfwStandard;
//			if (_passRecordBack) //Comming from Return Button
//			{
////				using( new WaitCursor())
////				{
////					parentForm.CallParent(this.ScreenId); //
////				}				
//			}
//			else //Coming from Close Button
//			{
////				if (_originalrimNo.Trim() != dfAccountNo.Text.Trim() && dfAccountNo.Text.Trim().Length > 0)
////				{
////					//360653 - You have entered a new RIM account for the selected transaction, are you sure you wish to close the window without returning your changes?
////					if (DialogResult.No == PMessageBox.Show(this, 360653, MessageType.Message, MessageBoxButtons.YesNo, string.Empty))
////					{
////						dfAccountNo.Focus();						
////						return;
////					}
////				}
//			}
		}
		#endregion frmTlEditFailedRIMTransaction_Closed

		//This is more appropriate to keep this method here
		#region CallOtherForms
		private void CallOtherForms( InvokeForm formName)
		{
			PfwStandard tempWin = null;
			if ( formName == InvokeForm.RimSearchWindow )
			{
				tempWin = Helper.CreateWindow("phoenix.client.tbarsearch", "Phoenix.Client.Search", "frmTBarSearch" );
				#region Comment - Parameter Meaning
				//				//-1, Embedded Window , 0 - From MDI, > 0 Coming from Other Windows for Search
				//				paramScreenId = Convert.ToInt32(paramList[0]); 
				//				//Name, AcctNo, Phone, and Tin			
				//				paramSearchType = (SetSearchType)paramList[1];
				//				//Acct Type, or Appl Type, or TIN
				//				paramSubSearchType = (SetSubSearchType)paramList[2];
				//				//Search Value==> Last Name, Acct Type, Phone, Tin
				//				paramSearchValue2 = Convert.ToString(paramList[3]);
				//				//First Name, AcctNo
				//				paramSearchValue3 = Convert.ToString(paramList[4]);
				//				//Invoke Accounts List
				//				paramInvokeScreenId = Convert.ToInt32(paramList[5]); 
				#endregion 
				tempWin.InitParameters(Shared.Constants.ScreenId.TlEditFailedRIMTransaction, Phoenix.BusObj.Global.SetSearchType.Name, string.Empty, string.Empty, string.Empty, -1);
			}
			if ( tempWin != null )
			{
				tempWin.Workspace = this.Workspace;
				tempWin.Show();
			}
		}
		#endregion

		#endregion PushButton Events

		#region Private Methods

		#region CallXMThruCDS
		private void CallXMThruCDS(XMThruCDSOrigin origin)
		{
			try
			{
				if (origin == XMThruCDSOrigin.GetTranCodeDesc )
				{
					#region GetTranCodeDesc
					_adTlTcBusObj.SelectAllFields = false;
					_adTlTcBusObj.Description.Selected = true;
					_adTlTcBusObj.TlTranCode.Value = _tlJournalBusObj.TlTranCode.Value;
					_adTlTcBusObj.ActionType = XmActionType.Select;
					DataService.Instance.ProcessRequest(this._adTlTcBusObj);
					#endregion GetTranCodeDesc
				}
				else if (origin == XMThruCDSOrigin.ValidateRimNo)
				{
					#region ValidateRimNo
					_rmAcctBusObj = new RmAcct();
					_rmAcctBusObj.SelectAllFields = false;
					_rmAcctBusObj.LastName.Selected = true;
					_rmAcctBusObj.FirstName.Selected = true;
					_rmAcctBusObj.MiddleInitial.Selected = true;
					_rmAcctBusObj.RimType.Selected = true;
					_rmAcctBusObj.RimNo.Selected = true;
//					try
//					{
						_rmAcctBusObj.RimNo.Value = Convert.ToInt32(dfAccountNo.Text);
						_rmAcctBusObj.ActionType = XmActionType.Select;
						DataService.Instance.ProcessRequest(_rmAcctBusObj);
						if (_rmAcctBusObj.RimType.Value.Trim() == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Personal)
							dfName.UnFormattedValue = _gbHelperBusObj.ConcateNameX(_rmAcctBusObj.FirstName.Value, _rmAcctBusObj.LastName.Value, _rmAcctBusObj.MiddleInitial.Value, false, string.Empty, string.Empty);
						else
							dfName.UnFormattedValue = _gbHelperBusObj.ConcateNameX(_rmAcctBusObj.FirstName.Value, _rmAcctBusObj.LastName.Value, _rmAcctBusObj.MiddleInitial.Value, true, string.Empty, string.Empty);
//					}
//					catch(Exception ex) //Record not found exception will be found here it self and no message
//					{
//						CoreService.LogPublisher.LogDebug(ex.Message);
//						//360650 - The RIM number you have entered does not exist on the system.  Please enter a valid RIM to continue.
//						//PMessageBox.Show(this, 360650, MessageType.Error, MessageBoxButtons.OK);
//					}
					#endregion ValidateRimNo
				}
				else if (origin == XMThruCDSOrigin.UpdateRimNo )
				{
					#region UpdateRimNo
					if ( TellerVars.Instance.OfflineCDS != null )
						TellerVars.Instance.OfflineCDS.ProcessCustomAction(_tlJournalBusObj, "UpdateRimNo" );
					#endregion UpdateRimNo
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show(  pe );
			}
		}
		#endregion 

		#region EnableDisableVisibleLogic
		private void EnableDisableVisibleLogic(EnableDisableVisible caseType)
		{
			if (caseType == EnableDisableVisible.PbReturn)
			{
				if (dfAccountNo.Text != null && dfAccountNo.Text.Trim().Length > 0)
					pbReturn.Enabled = true;
				else
					pbReturn.Enabled = false;
			}
		}
		#endregion 

		#region RiseErrorMessage
		private bool RiseErrorMessage()
		{
			if (dfAccountNo.Text.Length == 0)
				return true;
			int rimNo = 0;
			bool makeSelect = true;
			try
			{
				rimNo = Convert.ToInt32(dfAccountNo.Text);
			}
			catch
			{
				rimNo = 0;
			}

			if (rimNo == 0 || rimNo < 0)				
				makeSelect = false;				
			else
				makeSelect = true;

			if (makeSelect)
			{
				try
				{
					CallXMThruCDS(XMThruCDSOrigin.ValidateRimNo);
				}
				catch{rimNo = 0;}

				if (_rmAcctBusObj.LastName.IsNull)
					rimNo = 0;
			}
			if (rimNo == 0 || rimNo < 0)
			{
				//360651 The RIM number you have entered does not exist on the system.  Please enter a valid RIM to continue.
				PMessageBox.Show(this, 360651, MessageType.Error, MessageBoxButtons.OK);
				dfAccountNo.Text = string.Empty; //Clear the field
				//dfAccountNo.Focus();
				return false;
			}
			return true;
		}
		#endregion 

		#endregion Private Methods

		// Enumerations
		#region EnableDisableVisible
		public enum EnableDisableVisible
		{
			PbReturn
		}
		#endregion

		#region Enum Invoke Forms
		public enum InvokeForm
		{
			RimSearchWindow
		}
		#endregion 

		#region Class enum XMThruCDSOrigin
		public enum XMThruCDSOrigin
		{
			GetTranCodeDesc,
			ValidateRimNo,
			UpdateRimNo
		}
		#endregion




	}
}
