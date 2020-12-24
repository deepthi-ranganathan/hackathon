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
// File Name: frmDrawerMaintenance.cs
// NameSpace: Phoenix.Client.TlDrawerMaint
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/04/2006	2		mselvaga	#70371 - Supervisor close-out can be performed more than once for same drawer. Fixed.
//03/26/2007	3		mselvaga	#72131 - Drawer Maintenance window (frmDrawerMaintenance) allows user to save employee change multiple times and writes a row to the position history for each.
//04/25/2007	4		mselvaga	#71625 - When cmbEmployee is null do not allow save action.
//04/24/2009    5       mramalin    WI-3475 - Terminal Services Printing Enhancement
//07/01/2009    6       LSimpson    #4753 - Modifications for offline.
// 06Nov2009	7		GDiNatale	#6615 - SetValue Framework change
//06/01/2010    8       LSimpson    #9165 - Added new output parameters and data manipulation for check proofing.
//8/3/2013		9		apitava		#157637 Uses new xfsprinter
//11/26/2013    10      mselvaga    #25579 - Teller - 10946 – frmDrawerMaintenance  - Application Error (object).
//06/21/2019    11      AshishBabu  Task#116193 - Preventing closedout drawer from reassigning
//06/25/2019    12      AshishBabu  Bug#116424 - Revoking changes done for Task#116193
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
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.CDS;
using Phoenix.Windows.Client;
using Phoenix.Client.TlPosition;
using Phoenix.Shared.Xfs;

namespace Phoenix.Client.TlDrawerMaint
{
	/// <summary>
	/// Summary description for frmDrawerMaintenance.
	/// </summary>
	public class frmDrawerMaintenance : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TellerVars _tellerVars = TellerVars.Instance;
		private TlDrawerBalances _busObjTlDrawerBalances = new TlDrawerBalances();
		private TlHelper _tlHelper = new TlHelper();

		private PDateTime postingDt = new PDateTime();
		private PSmallInt branchNo = new PSmallInt();
		private PSmallInt drawerNo = new PSmallInt();
		private PDecimal ptid = new PDecimal();
		private PSmallInt employeeId = new PSmallInt();
		private PString employeeName = new PString();

		private Phoenix.Windows.Forms.PGroupBoxStandard gbNewTeller;
		private Phoenix.Windows.Forms.PLabelStandard lblEmployee;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbEmployee;
		private Phoenix.Windows.Forms.PAction pbSave;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbPerformCloseOut;
		private PSmallInt prevEmployeeId = new PSmallInt();
        #region #76409
        private DialogResult dialogResult = DialogResult.None;
        private PSmallInt _reprintFormId = new PSmallInt("reprintFormId");
        private PString _checkType = new PString("checkType");
        private PDecimal _proofPtid = new PDecimal("proofPtid");
        private PDecimal _noCopies = new PDecimal("noCopies");
        #region WI-3475
        private PString _printerService = new PString("PrinterService");    //#25579
        #endregion
        private string _reprintInfo = "";     
        #endregion


        public frmDrawerMaintenance()
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDrawerMaintenance));
            this.gbNewTeller = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbPerformCloseOut = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbEmployee = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblEmployee = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbSave = new Phoenix.Windows.Forms.PAction();
            this.gbNewTeller.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSave});
            // 
            // gbNewTeller
            // 
            this.gbNewTeller.Controls.Add(this.cbPerformCloseOut);
            this.gbNewTeller.Controls.Add(this.cmbEmployee);
            this.gbNewTeller.Controls.Add(this.lblEmployee);
            this.gbNewTeller.Location = new System.Drawing.Point(4, 0);
            this.gbNewTeller.Name = "gbNewTeller";
            this.gbNewTeller.PhoenixUIControl.ObjectId = 1;
            this.gbNewTeller.Size = new System.Drawing.Size(684, 44);
            this.gbNewTeller.TabIndex = 0;
            this.gbNewTeller.TabStop = false;
            this.gbNewTeller.Text = "New Teller";
            // 
            // cbPerformCloseOut
            // 
            this.cbPerformCloseOut.CodeValue = null;
            this.cbPerformCloseOut.Location = new System.Drawing.Point(360, 16);
            this.cbPerformCloseOut.Name = "cbPerformCloseOut";
            this.cbPerformCloseOut.PhoenixUIControl.ObjectId = 4;
            this.cbPerformCloseOut.Size = new System.Drawing.Size(172, 20);
            this.cbPerformCloseOut.TabIndex = 0;
            this.cbPerformCloseOut.Text = "Perform CloseOut/Adj. Totals";
            this.cbPerformCloseOut.Click += new System.EventHandler(this.cbPerformCloseOut_Click);
            // 
            // cmbEmployee
            // 
            this.cmbEmployee.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CombinedValue;
            this.cmbEmployee.Location = new System.Drawing.Point(140, 16);
            this.cmbEmployee.Name = "cmbEmployee";
            this.cmbEmployee.PhoenixUIControl.ObjectId = 2;
            this.cmbEmployee.PhoenixUIControl.XmlTag = "NewEmplId";
            this.cmbEmployee.Size = new System.Drawing.Size(212, 21);
            this.cmbEmployee.TabIndex = 3;
            // 
            // lblEmployee
            // 
            this.lblEmployee.AutoEllipsis = true;
            this.lblEmployee.Location = new System.Drawing.Point(4, 16);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.PhoenixUIControl.ObjectId = 2;
            this.lblEmployee.Size = new System.Drawing.Size(120, 20);
            this.lblEmployee.TabIndex = 4;
            this.lblEmployee.Text = "Employee:";
            // 
            // pbSave
            // 
            this.pbSave.Image = ((System.Drawing.Image)(resources.GetObject("pbSave.Image")));
            this.pbSave.NextScreenId = 10430;
            this.pbSave.ObjectId = 6;
            this.pbSave.ShortText = "Save";
            this.pbSave.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSave_Click);
            // 
            // frmDrawerMaintenance
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbNewTeller);
            this.Name = "frmDrawerMaintenance";
            this.Load += new System.EventHandler(this.frmDrawerMaintenance_Load);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmDrawerMaintenance_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmDrawerMaintenance_PInitBeginEvent);
            this.gbNewTeller.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Init param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 6)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				employeeId.Value = Convert.ToInt16(paramList[2]);
				employeeName.Value = Convert.ToString(paramList[3]);
				prevEmployeeId.Value = employeeId.Value;
				if (!employeeName.IsNull)
				{
					employeeName.Value = employeeName.Value.Replace("*", "").Trim();
				}
				ptid.Value = Convert.ToDecimal(paramList[4]);
				postingDt.Value = Convert.ToDateTime(paramList[5]);

				LoadPositionFromDrawer();
			}

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.DrawerMaintenance;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			base.InitParameters (paramList);
		}
		#endregion

		public override bool OnActionClose()
		{
			CallParent("DrawerMaint");			
			return base.OnActionClose ();
		}


		#region events
		private ReturnType frmDrawerMaintenance_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.MainBusinesObject = _busObjTlDrawerBalances;

			#region screen title
			this.NewRecordTitle = string.Format(this.NewRecordTitle, Convert.ToString(drawerNo.Value));
			this.EditRecordTitle = string.Format(this.EditRecordTitle, Convert.ToString(drawerNo.Value));
			#endregion

			this.pbSave.NextScreenId = Phoenix.Shared.Constants.ScreenId.SummaryPosition;
			return ReturnType.Success;
		}

		private void frmDrawerMaintenance_PInitCompleteEvent()
		{
            cbPerformCloseOut.Checked = true;
			PerformCloseOutClick();
		}

		private void cbPerformCloseOut_Click(object sender, EventArgs e)
		{
			PerformCloseOutClick();
		}

		private void pbSave_Click(object sender, PActionEventArgs e)
		{
			if (this.cbPerformCloseOut.Checked)
			{
                if (_tellerVars.AdTlControl.RealTimeProof.Value == "Y")
                    ProofChecks();
				frmTlPosition temp = new frmTlPosition();
				temp.InitParameters(3, branchNo.Value, drawerNo.Value, postingDt.Value, null, employeeId.Value, Convert.ToInt16(cmbEmployee.CodeValue), false);
				temp.Workspace = this.Workspace;
				temp.Show();
				temp.Closed +=new EventHandler(temp_Closed);
			}
			else
			{
				if (cmbEmployee.CodeValue != null && cmbEmployee.CodeValue.ToString() != "" &&
					Convert.ToInt32(cmbEmployee.CodeValue) != -2) //#72625
				{
					if (!AssignEmployee())
						return;
				}
			}
        }

        #region #76409
        private void ProofChecks()
        {
            PSmallInt brNo = new PSmallInt("brNo");
            brNo.Value = branchNo.Value;
            PSmallInt drNo = new PSmallInt("drNo");
            drNo.Value = drawerNo.Value;
            PInt curTranCode = new PInt("curTranCode");
            PDateTime postingDt = new PDateTime("postingDt");
            PString OfflineDb = new PString("OfflineDb");   // #4753
            PDecimal proofAmt = new PDecimal("proofAmt"); //#9165
            PDecimal proofNoItems = new PDecimal("proofNoItems"); //#9165
            OfflineDb.Value = AppInfo.Instance.OfflineDbNames;  // #4753
            _proofPtid.Value = 0;
            _noCopies.Value = 1;
            PrintInfo printInfo = null;
            AdTlForm adTlForm;
            XfsPrinter xfsPrinter = null;	 //#157637
            TlJournal tlJournal = new TlJournal();
            postingDt.Value = _tellerVars.PostingDt;
            Phoenix.BusObj.Teller.TlDrawerBalances _tempDrBalObj = new TlDrawerBalances();
            _tempDrBalObj.ActionType = XmActionType.Custom;
            _tempDrBalObj.BranchNo.Value = branchNo.Value;
            _tempDrBalObj.DrawerNo.Value = drNo.Value;
            _tempDrBalObj.ClosedDt.Value = Convert.ToDateTime(_tellerVars.PostingDt);
            // Process OnUs
            _checkType.Value = "O";
            DataService.Instance.ProcessCustomAction(_tempDrBalObj, "ProofTotals", brNo, drNo, _checkType, postingDt, _proofPtid, OfflineDb, proofAmt, proofNoItems);
            
            if (proofAmt.IsNull) //#9165
                proofAmt.Value = 0;
            if (proofNoItems.IsNull) //#9165
                proofNoItems.Value = 0;

            if (TellerVars.Instance.OfflineCDS != null) //#9165
            {
                TellerVars.Instance.OfflineCDS.ProcessCustomAction(_tempDrBalObj, "ProofTotalsOfflineUpdate", brNo, drNo, _checkType, proofAmt, proofNoItems);
            }

            if (_proofPtid.Value > 0)
            {
                CallOtherForms("AdHocReceipt");
                if (dialogResult != DialogResult.OK)
                    return;

                if (!TellerVars.Instance.SetContextObject("AdTlFormArray", _reprintFormId.Value))
                {
                    PMessageBox.Show(360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                    return;
                }
                adTlForm = TellerVars.Instance.AdTlForm;
                try
                {
                    printInfo = tlJournal.GetCheckProofPrintInfo(adTlForm.PrintString.Value, _proofPtid.IntValue);
                    printInfo.TellerDrawer = drNo.IntValue;
                    printInfo.SequenceNo = _tempDrBalObj.DrawerSeqNo.IntValue;
                    
                    if (printInfo != null)
                    {
                        if (xfsPrinter == null)
                            xfsPrinter = new XfsPrinter(_printerService.Value);
						xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);	 //#157637
                    }
                }
                catch (PhoenixException pe)
                {
                    PMessageBox.Show(pe, MessageBoxButtons.OK);
                    return;
                }
            }
            // Process Transit
            _checkType.Value = "T";
            DataService.Instance.ProcessCustomAction(_tempDrBalObj, "ProofTotals", brNo, drNo, _checkType, postingDt, _proofPtid, OfflineDb, proofAmt, proofNoItems);
            
            if (proofAmt.IsNull) //#9165
                proofAmt.Value = 0;
            if (proofNoItems.IsNull) //#9165
                proofNoItems.Value = 0;

            if (TellerVars.Instance.OfflineCDS != null) //#9165
            {
                TellerVars.Instance.OfflineCDS.ProcessCustomAction(_tempDrBalObj, "ProofTotalsOfflineUpdate", brNo, drNo, _checkType, proofAmt, proofNoItems);
            }

            if (_proofPtid.Value > 0)
            {
                CallOtherForms("AdHocReceipt");
                if (dialogResult != DialogResult.OK)
                    return;

                if (!TellerVars.Instance.SetContextObject("AdTlFormArray", _reprintFormId.Value))
                {
                    PMessageBox.Show(360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                    return;
                }
                adTlForm = TellerVars.Instance.AdTlForm;
                try
                {
                    printInfo = tlJournal.GetCheckProofPrintInfo(adTlForm.PrintString.Value, _proofPtid.IntValue);
                    printInfo.TellerDrawer = drNo.IntValue;
                    printInfo.SequenceNo = _tempDrBalObj.DrawerSeqNo.IntValue;
                    if (printInfo != null)
                    {
                        if (xfsPrinter == null)
                            xfsPrinter = new XfsPrinter(_printerService.Value);
						xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);	  //#157637
                    }
                }
                catch (PhoenixException pe)
                {
                    PMessageBox.Show(pe, MessageBoxButtons.OK);
                    return;
                }
            }
            if (xfsPrinter != null)
    			xfsPrinter.Close();	  //#157637
        }

        private void CallOtherForms(string origin)
        {
            PfwStandard tempDlg = null;

            try
            {
                if (origin == "AdHocReceipt")
                {
                    _reprintInfo = CoreService.Translation.GetUserMessageX(360612);
                    _reprintFormId = new PSmallInt("FormId");
                    _printerService.Value = string.Empty;
                    _reprintFormId.Value = Convert.ToInt16(Phoenix.Shared.Constants.ScreenId.TlProofTotals);
                    tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt");
                    tempDlg.InitParameters(_reprintInfo, _reprintFormId, _reprintFormId.Value, null, null, _noCopies, _printerService);
                }
                if (tempDlg != null)
                {
                    dialogResult = tempDlg.ShowDialog(this);
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
                return;
            }
        }
        #endregion

        #endregion

        #region private methods
        private void EnableDisableVisibleLogic( string callerInfo )
		{
			try
			{
				if (callerInfo == "FormComplete")
				{
					cmbEmployee.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Default);

					if (_tellerVars.NetworkOffline)
						this.cbPerformCloseOut.Enabled = false;
				}
				else if (callerInfo == "CloseOutClick")
				{
					if (cbPerformCloseOut.Checked)
					{
						cmbEmployee.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.DisableShowText);
					}
					else
					{
						cmbEmployee.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
						cmbEmployee.CodeValue = null;
						cmbEmployee.Text = null;
					}
				}

			}
			catch (PhoenixException peedvlogic)
			{
				PMessageBox.Show(peedvlogic);
			}
//			catch (Exception eedvlogic)
//			{
//				MessageBox.Show(this, eedvlogic.Message + "\r\n" + eedvlogic.InnerException, "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
//			}
		}

		private bool AssignEmployee()
		{
			short tempEmployeeId = prevEmployeeId.Value;
			try
			{
				//#72131 - replaced employeeId with prevEmployeeId
				if (prevEmployeeId.Value == Convert.ToInt16(cmbEmployee.CodeValue))
				{
					PMessageBox.Show(this, 360742, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					cmbEmployee.Focus();
					return false;
				}

				this._busObjTlDrawerBalances.CheckForValidEmployee(cmbEmployee.CodeValue.ToString());

				if (DialogResult.No == PMessageBox.Show(this, 314220, MessageType.Question, MessageBoxButtons.YesNo, string.Empty))
					return false;

				LoadPositionFromDrawer();

				dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360281));

				PBaseType PrevEmplId = new PSmallInt("A0");
				PBaseType NewEmplId = new PSmallInt("A1");
				PBaseType PostingDt = new PDateTime("A2");

				PrevEmplId.ValueObject = employeeId.Value;
				NewEmplId.ValueObject = Convert.ToInt16(cmbEmployee.CodeValue);
				PostingDt.ValueObject = postingDt.Value;
				_busObjTlDrawerBalances.ScreenId.Value = this.ScreenId;
				_busObjTlDrawerBalances.GlobalEmplId.Value = _tellerVars.EmployeeId;

				using (new WaitCursor())
				{
					this._busObjTlDrawerBalances.MessageId.Value = 0;
					if (cmbEmployee.CodeValue != null)
						prevEmployeeId.Value = Convert.ToInt16(cmbEmployee.CodeValue); //#72131

					DataService.Instance.ProcessCustomAction(_busObjTlDrawerBalances,"AssignDrawer",PrevEmplId,NewEmplId,
						PostingDt);
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe );
				prevEmployeeId.Value = tempEmployeeId; //#72131
				return false;
			}
//			catch (Exception dwrAssign)
//			{
//				MessageBox.Show(this, dwrAssign.Message + "\r\n" + dwrAssign.InnerException, "Error", MessageBoxButtons.OK);
//				return false;
//			}
			finally
			{
				dlgInformation.Instance.HideInfo();
			}

			return true;
		}

		private void LoadPositionFromDrawer()
		{
			_busObjTlDrawerBalances.PosView.Value = 3;
			_busObjTlDrawerBalances.BranchNo.Value = branchNo.Value;
			_busObjTlDrawerBalances.DrawerNo.Value = drawerNo.Value;
			_busObjTlDrawerBalances.ClosedDt.Value = postingDt.Value;
			_busObjTlDrawerBalances.CrncyId.Value = _tellerVars.LocalCrncyId;
			_busObjTlDrawerBalances.PrevEmplId.Value = employeeId.Value;
			_busObjTlDrawerBalances.NewEmplId.SetValue(null, EventBehavior.None);		// #6615 - _busObjTlDrawerBalances.NewEmplId.SetValueToNull();
			_busObjTlDrawerBalances.TellerOffOnStatus.Value = (short)TranOfflineStatus.OnlineOnly;
			_busObjTlDrawerBalances.SelectType.Value = 1;
		}

		private void PerformCloseOutClick()
		{
			EnableDisableVisibleLogic("CloseOutClick");

			if (cbPerformCloseOut.Checked && !employeeName.IsNull)
			{
//				cmbEmployee.Text = Convert.ToString(employeeId.Value) + " - " + employeeName.Value;
				cmbEmployee.CodeValue = employeeId.Value;
			}
			cmbEmployee.Focus();
		}
		#endregion

		private void temp_Closed(object sender, EventArgs e)
		{
			this.Close();
		}

        private void frmDrawerMaintenance_Load(object sender, EventArgs e)
        {

        }
	}
}
