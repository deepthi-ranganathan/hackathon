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
// File Name: dlgPODTotals.cs
// NameSpace: Phoenix.Client.TlPosition
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/03/2006    1       mselvaga	#67882 - Created.
//02/20/2007	2		rpoddar		#71873 - Commented the closing of XfsPrinter and creating new instance of XfsPrinter
//May-24-07		3   	Muthu       #72780 VS2005 Migration 
//08/23/2007    4       FOyebola    #73591 Added new fields for POD totals printiing.
//08/23/2007    5       FOyebola    #73591(2) Added new fields for POD totals printiing.
//04/24/2009    6       mramalin    WI-3475 - Terminal Services Printing Enhancement
//8/3/2013		7		apitava		#157637 Uses new xfsprinter
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.Windows.Client;
using Phoenix.Shared.Xfs;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.CDS;
using Phoenix.BusObj.Admin.Global;  //#73591

namespace Phoenix.Client.TlPosition
{
	/// <summary>
	/// Summary description for dlgPODTotals.
	/// </summary>
	public class dlgPODTotals : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PdfStandard dfHiddenText;
		private Phoenix.Windows.Forms.PAction pbPrintForm;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbPodInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblBranch;
		private Phoenix.Windows.Forms.PDfDisplay dfBranchNo;
		private Phoenix.Windows.Forms.PLabelStandard lblDrawer;
		private Phoenix.Windows.Forms.PDfDisplay dfDrawerNo;
		private Phoenix.Windows.Forms.PLabelStandard lblPostingDt;
		private Phoenix.Windows.Forms.PDfDisplay dfPostingDt;
		private Phoenix.Windows.Forms.PLabelStandard lblPodCredits;
		private Phoenix.Windows.Forms.PDfDisplay dfPodCredits;
		private Phoenix.Windows.Forms.PLabelStandard lblPodDebits;
		private Phoenix.Windows.Forms.PDfDisplay dfPodDebits;

		#region Initialize
		private PSmallInt _podTotalsType = new PSmallInt("PodTotalsType");
		private PString _branchName = new PString();
		private PSmallInt _branchNo = new PSmallInt();
		private PSmallInt _drawerNo = new PSmallInt();
		private PDateTime _postingDt = new PDateTime();
		private PDecimal _podCredits = new PDecimal("PodCredits");
		private PDecimal _podDebits = new PDecimal("PodDebits");
		private PDecimal _lastPodPtid = new PDecimal("LastPodPtid");
		private PDecimal _prevLastPodPtid = new PDecimal("PrevLastPodPtid");
		private PDecimal _xpLastPodPtid = new PDecimal("XpLastPodPtid");
		private PDecimal _xpPrevLastPodPtid = new PDecimal("XpPrevLastPodPtid");
		private PSmallInt _reprintFormId;
		private string _reprintInfo = "";
		private string _reprintTextQrp = "";
		private string _partialPrintString = "";
		private string _wosaServiceName = "";
		private string _logicalService = "";
		private string _formName = "";
		private string _mediaName = "";
		private string _title = "";

		private TellerVars _tellerVars = TellerVars.Instance;
		private TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
		private AdTlForm _busObjAdTlForm = new AdTlForm();
		private XmlNode _adTlFormNode = TellerVars.Instance.AdTlFormNode as XmlNode;
		private PrintInfo _wosaPrintInfo = new PrintInfo();
		private XfsPrinter _xfsPrinter;
		private DialogResult dialogResult = new DialogResult();
		private Phoenix.Windows.Forms.PAction pbCancel;
		private TlHelper _tlHelper = new TlHelper();

        // Begin # 73591
        private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);
        private AdGbBank _adGbBank = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBank] as AdGbBank);
        private	AdGbBranch _adGbBranch = (GlobalObjects.Instance[ Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBranch ] as AdGbBranch);
        //private AdGbBankControl _adGbBankControl = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBankControl] as AdGbBankControl);
        private Phoenix.BusObj.Teller.TlHelper _tellerHelper = new TlHelper();
        // End # 73591
        #region 3475
        private PDecimal _noCopies;
        private PString _printerService;
        #endregion

		#endregion



		public dlgPODTotals()
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
            this.gbPodInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfPodDebits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPodDebits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPodCredits = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPodCredits = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfPostingDt = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblPostingDt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDrawerNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblDrawer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfBranchNo = new Phoenix.Windows.Forms.PDfDisplay();
            this.lblBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfHiddenText = new Phoenix.Windows.Forms.PdfStandard();
            this.pbPrintForm = new Phoenix.Windows.Forms.PAction();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.gbPodInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbPrintForm,
            this.pbCancel});
            // 
            // gbPodInformation
            // 
            this.gbPodInformation.Controls.Add(this.dfPodDebits);
            this.gbPodInformation.Controls.Add(this.lblPodDebits);
            this.gbPodInformation.Controls.Add(this.dfPodCredits);
            this.gbPodInformation.Controls.Add(this.lblPodCredits);
            this.gbPodInformation.Controls.Add(this.dfPostingDt);
            this.gbPodInformation.Controls.Add(this.lblPostingDt);
            this.gbPodInformation.Controls.Add(this.dfDrawerNo);
            this.gbPodInformation.Controls.Add(this.lblDrawer);
            this.gbPodInformation.Controls.Add(this.dfBranchNo);
            this.gbPodInformation.Controls.Add(this.lblBranch);
            this.gbPodInformation.Controls.Add(this.dfHiddenText);
            this.gbPodInformation.Location = new System.Drawing.Point(4, 0);
            this.gbPodInformation.Name = "gbPodInformation";
            this.gbPodInformation.PhoenixUIControl.ObjectId = 1;
            this.gbPodInformation.Size = new System.Drawing.Size(452, 76);
            this.gbPodInformation.TabIndex = 1;
            this.gbPodInformation.TabStop = false;
            this.gbPodInformation.Text = "POD Information";
            // 
            // dfPodDebits
            // 
            this.dfPodDebits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPodDebits.Location = new System.Drawing.Point(332, 56);
            this.dfPodDebits.Multiline = true;
            this.dfPodDebits.Name = "dfPodDebits";
            this.dfPodDebits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPodDebits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfPodDebits.PhoenixUIControl.ObjectId = 6;
            this.dfPodDebits.PhoenixUIControl.XmlTag = "PodDebits";
            this.dfPodDebits.Size = new System.Drawing.Size(112, 16);
            this.dfPodDebits.TabIndex = 14;
            this.dfPodDebits.TextChanged += new System.EventHandler(this.dfPodDebits_TextChanged);
            // 
            // lblPodDebits
            // 
            this.lblPodDebits.AutoEllipsis = true;
            this.lblPodDebits.Location = new System.Drawing.Point(228, 56);
            this.lblPodDebits.Name = "lblPodDebits";
            this.lblPodDebits.PhoenixUIControl.ObjectId = 6;
            this.lblPodDebits.Size = new System.Drawing.Size(100, 16);
            this.lblPodDebits.TabIndex = 13;
            this.lblPodDebits.Text = "Debits:";
            // 
            // dfPodCredits
            // 
            this.dfPodCredits.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPodCredits.Location = new System.Drawing.Point(108, 56);
            this.dfPodCredits.Multiline = true;
            this.dfPodCredits.Name = "dfPodCredits";
            this.dfPodCredits.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPodCredits.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfPodCredits.PhoenixUIControl.ObjectId = 5;
            this.dfPodCredits.PhoenixUIControl.XmlTag = "PodCredits";
            this.dfPodCredits.Size = new System.Drawing.Size(112, 16);
            this.dfPodCredits.TabIndex = 12;
            // 
            // lblPodCredits
            // 
            this.lblPodCredits.AutoEllipsis = true;
            this.lblPodCredits.Location = new System.Drawing.Point(4, 56);
            this.lblPodCredits.Name = "lblPodCredits";
            this.lblPodCredits.PhoenixUIControl.ObjectId = 5;
            this.lblPodCredits.Size = new System.Drawing.Size(100, 16);
            this.lblPodCredits.TabIndex = 11;
            this.lblPodCredits.Text = "Credits:";
            // 
            // dfPostingDt
            // 
            this.dfPostingDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPostingDt.Location = new System.Drawing.Point(332, 36);
            this.dfPostingDt.Multiline = true;
            this.dfPostingDt.Name = "dfPostingDt";
            this.dfPostingDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfPostingDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfPostingDt.PhoenixUIControl.ObjectId = 4;
            this.dfPostingDt.PhoenixUIControl.XmlTag = "PostingDt";
            this.dfPostingDt.Size = new System.Drawing.Size(112, 16);
            this.dfPostingDt.TabIndex = 10;
            this.dfPostingDt.TextChanged += new System.EventHandler(this.dfPostingDt_TextChanged);
            // 
            // lblPostingDt
            // 
            this.lblPostingDt.AutoEllipsis = true;
            this.lblPostingDt.Location = new System.Drawing.Point(228, 36);
            this.lblPostingDt.Name = "lblPostingDt";
            this.lblPostingDt.PhoenixUIControl.ObjectId = 4;
            this.lblPostingDt.Size = new System.Drawing.Size(100, 16);
            this.lblPostingDt.TabIndex = 9;
            this.lblPostingDt.Text = "Posting Date:";
            // 
            // dfDrawerNo
            // 
            this.dfDrawerNo.Location = new System.Drawing.Point(108, 36);
            this.dfDrawerNo.Multiline = true;
            this.dfDrawerNo.Name = "dfDrawerNo";
            this.dfDrawerNo.PhoenixUIControl.ObjectId = 3;
            this.dfDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.dfDrawerNo.Size = new System.Drawing.Size(112, 16);
            this.dfDrawerNo.TabIndex = 8;
            // 
            // lblDrawer
            // 
            this.lblDrawer.AutoEllipsis = true;
            this.lblDrawer.Location = new System.Drawing.Point(4, 36);
            this.lblDrawer.Name = "lblDrawer";
            this.lblDrawer.PhoenixUIControl.ObjectId = 3;
            this.lblDrawer.Size = new System.Drawing.Size(100, 16);
            this.lblDrawer.TabIndex = 7;
            this.lblDrawer.Text = "Drawer:";
            // 
            // dfBranchNo
            // 
            this.dfBranchNo.Location = new System.Drawing.Point(108, 16);
            this.dfBranchNo.Multiline = true;
            this.dfBranchNo.Name = "dfBranchNo";
            this.dfBranchNo.PhoenixUIControl.ObjectId = 2;
            this.dfBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.dfBranchNo.Size = new System.Drawing.Size(220, 16);
            this.dfBranchNo.TabIndex = 6;
            this.dfBranchNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBranch
            // 
            this.lblBranch.AutoEllipsis = true;
            this.lblBranch.Location = new System.Drawing.Point(4, 16);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.PhoenixUIControl.ObjectId = 2;
            this.lblBranch.Size = new System.Drawing.Size(100, 16);
            this.lblBranch.TabIndex = 5;
            this.lblBranch.Text = "Branch:";
            // 
            // dfHiddenText
            // 
            this.dfHiddenText.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfHiddenText.Location = new System.Drawing.Point(96, 136);
            this.dfHiddenText.Name = "dfHiddenText";
            this.dfHiddenText.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfHiddenText.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.dfHiddenText.PhoenixUIControl.ObjectId = 2;
            this.dfHiddenText.Size = new System.Drawing.Size(4, 20);
            this.dfHiddenText.TabIndex = 4;
            this.dfHiddenText.Visible = false;
            // 
            // pbPrintForm
            // 
            this.pbPrintForm.ObjectId = 7;
            this.pbPrintForm.Shortcut = System.Windows.Forms.Keys.F2;
            this.pbPrintForm.ShortText = "Print (F2)";
            this.pbPrintForm.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPrintForm_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.ObjectId = 8;
            this.pbCancel.ShortText = "&Cancel";
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // dlgPODTotals
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(458, 80);
            this.Controls.Add(this.gbPodInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgPODTotals";
            this.ScreenId = 16005;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.Text = "POD Totals";
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgPODTotals_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgPODTotals_PInitBeginEvent);
            this.gbPodInformation.ResumeLayout(false);
            this.gbPodInformation.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 5)
			{
				if (paramList[0] != null)
					_podTotalsType.Value = Convert.ToInt16(paramList[0]);
				if (paramList[1] != null && paramList[2] != null)
				{
					_branchNo.Value = Convert.ToInt16(paramList[1]);
					_branchName.Value = Convert.ToString(paramList[2]);
				}
				if (paramList[3] != null)
					_drawerNo.Value = Convert.ToInt16(paramList[3]);
				if (paramList[4] != null)
					_postingDt.Value = Convert.ToDateTime(paramList[4]);
			}

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.PODTotals;
			this._tlDrawerBalances.BranchNo.Value = _branchNo.Value;
			this._tlDrawerBalances.DrawerNo.Value = _drawerNo.Value;
			this._tlDrawerBalances.CrncyId.Value = _tellerVars.LocalCrncyId;
			this._tlDrawerBalances.ClosedDt.Value = _postingDt.Value;
			//
			base.InitParameters (paramList);
		}
		#endregion

		#region Pod Totals Events
		private ReturnType dlgPODTotals_PInitBeginEvent()
		{
			#region handle security
			this.pbPrintForm.NextScreenId = 0;
			#endregion
			//
			#region set window title
			if (_podTotalsType.Value == 1)
				_title = CoreService.Translation.GetUserMessageX(360654);
			else if (_podTotalsType.Value == 2)
				_title = CoreService.Translation.GetUserMessageX(360655);
			else
				_title = CoreService.Translation.GetUserMessageX(360656);
			this.NewRecordTitle = string.Format(this.NewRecordTitle, _title);
			this.EditRecordTitle = string.Format(this.EditRecordTitle, _title);;
			#endregion
			//
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.MainBusinesObject = _tlDrawerBalances;
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.DefaultAction = pbPrintForm;
			return ReturnType.Success;
		}

		private void dlgPODTotals_PInitCompleteEvent()
		{
			_reprintInfo = CoreService.Translation.GetUserMessageX(360657);
			this.dfBranchNo.Text = _branchNo.Value.ToString() + " - " + _branchName.Value;
			this.dfDrawerNo.UnFormattedValue = _drawerNo.Value;
			this.dfPostingDt.UnFormattedValue = _postingDt.Value;
			//
			#region image
			this.pbPrintForm.Image = Images.Print;
			this.pbCancel.Image = Images.Cancel;
			#endregion
			//
			#region calc pod totals
			try
			{
				DataService.Instance.ProcessCustomAction(_tlDrawerBalances,"PodTotals",
					_podTotalsType,
					_lastPodPtid,
					_prevLastPodPtid,
					_xpLastPodPtid,
					_xpPrevLastPodPtid,
					_podCredits,
					_podDebits);
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe );
				return;
			}
			finally
			{
				this.dfPodCredits.UnFormattedValue = (_podCredits.IsNull? Convert.ToDecimal(0) : Convert.ToDecimal(_podCredits.Value));
				this.dfPodDebits.UnFormattedValue = (_podDebits.IsNull? Convert.ToDecimal(0) : Convert.ToDecimal(_podDebits.Value));
			}
			#endregion
		}

		private void pbPrintForm_Click(object sender, PActionEventArgs e)
		{

            string glAcctNo = null; // #73591
            
			if (_tellerVars.AdTlControl.WosaPrinting.Value != GlobalVars.Instance.ML.Y)
			{
				PMessageBox.Show(this, 360658, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return;
			}
			//
			CallOtherForms("AdHocReceipt");
			//
			if (_reprintFormId.IsNull || _reprintFormId.Value == 0)
				return;
			//
			#region get form info
			if (_tellerVars.SetContextObject("AdTlFormArray", null, _reprintFormId.Value))
			{
				_reprintTextQrp = _tellerVars.AdTlForm.TextQrp.Value;
				_partialPrintString = _tellerVars.AdTlForm.PrintString.Value;
                _wosaServiceName = _printerService.Value; // WI-3475
                //_wosaServiceName = _tellerVars.AdTlForm.LogicalService.Value; //WI-3475
				_logicalService = _tellerVars.AdTlForm.ServiceType.Value;
				_mediaName = _tellerVars.AdTlForm.MediaName.Value;
				_formName = _tellerVars.AdTlForm.FormName.Value;
				_xfsPrinter = new XfsPrinter(_wosaServiceName);	 //#157637
			}
			#endregion
			//
			#region load print info and print
			if (_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "T"))
			{
				_wosaPrintInfo.PodCredits = Convert.ToDecimal(dfPodCredits.UnFormattedValue);
				_wosaPrintInfo.PodDebits = Convert.ToDecimal(dfPodDebits.UnFormattedValue);
				_wosaPrintInfo.BranchNo = Convert.ToInt32(dfBranchNo.UnFormattedValue);
				_wosaPrintInfo.TellerDrawer = Convert.ToInt32(dfDrawerNo.UnFormattedValue);
				_wosaPrintInfo.PostingDate = Convert.ToDateTime(dfPostingDt.UnFormattedValue);
               // Begin #73591
               if (_adGbBank != null)
               {
                    _wosaPrintInfo.BankName = _adGbBank.Name1.Value;
                    _wosaPrintInfo.BankRoutingNo = _adGbBank.RoutingNo.Value;
               }
               if (_adGbBranch != null)
               {
                   if (_adGbBranch.GlPostingPrefix.Value.IndexOf("-") >= 0)
                       _wosaPrintInfo.EmplBankPostingPrefix = _adGbBranch.GlPostingPrefix.Value.Substring(0, _adGbBranch.GlPostingPrefix.Value.IndexOf("-"));
                   else
                       _wosaPrintInfo.EmplBankPostingPrefix = _adGbBranch.GlPostingPrefix.Value;

                   _wosaPrintInfo.EmplBranchPostingPrefix = _adGbBranch.GlPostingPrefix.Value;
                   _wosaPrintInfo.EmplBranchRoutingNo = _adGbBranch.RoutingNo.Value;

                   glAcctNo = TellerVars.Instance.CashAcctNo;
                   if (glAcctNo != null && glAcctNo.IndexOf("*") > 0 && !_adGbBranch.GlPostingPrefix.IsNull)
                       _tellerHelper.ResolveGLAccount(_adGbBranch.GlPostingPrefix.Value, ref glAcctNo);
                   _wosaPrintInfo.TellerCashAccount = glAcctNo;
                   _wosaPrintInfo.EmplBranchName = _adGbBranch.Name1.Value; // #73591-2 
               }
               if (_adGbRsm != null)
               {
                   _wosaPrintInfo.EmployeeId = _adGbRsm.EmplId.Value;
                   _wosaPrintInfo.EmployeeName = _adGbRsm.EmployeeName.Value;
                   _wosaPrintInfo.TellerNo = _adGbRsm.TellerNo.Value;
               }
                // End #73591

               // #73591-2
               _wosaPrintInfo.TranEffDt = Convert.ToDateTime(dfPostingDt.UnFormattedValue);

               //
				#region handle printing
				try
				{
					_xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
					//
					DataService.Instance.ProcessCustomAction(_tlDrawerBalances,"UpdateLastPodInfo",
						_lastPodPtid,
						_prevLastPodPtid,
						_xpLastPodPtid,
						_xpPrevLastPodPtid);
				}
				catch( PhoenixException pe )
				{
					PMessageBox.Show( pe );
					return;
				}	
				finally
				{
					_xfsPrinter.Close();	//#157637
				}	
				#endregion
			}
			#endregion
			//
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void pbCancel_Click(object sender, PActionEventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion
		
		#region private methods
		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;
			PfwStandard tempDlg = null;

			try
			{
				if ( origin == "AdHocReceipt" )
				{
                    //
                    _noCopies = new PDecimal("NoCopies");
                    _noCopies.Value = 1;
                    _printerService = new PString("PrinterService");
                    //
					_reprintFormId = new PSmallInt("FormId");
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt" );
					//tempDlg.InitParameters( _reprintInfo, _reprintFormId, this.ScreenId );
                    tempDlg.InitParameters(_reprintInfo, _reprintFormId, this.ScreenId, null, null, _noCopies, _printerService); // WI-3475
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
		#endregion

        private void dfPostingDt_TextChanged(object sender, EventArgs e)
        {

        }

        private void dfPodDebits_TextChanged(object sender, EventArgs e)
        {

        }
	}
}
