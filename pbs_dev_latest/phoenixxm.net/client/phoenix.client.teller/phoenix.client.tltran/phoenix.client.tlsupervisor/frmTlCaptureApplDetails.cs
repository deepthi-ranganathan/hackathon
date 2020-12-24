#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2014 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlCaptureApplDetails.cs
// NameSpace: Phoenix.Client.TlSupervisor
//-------------------------------------------------------------------------------
//Date							    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/26/2016 12:15:49 PM			1		mselvaga    #52625 - Created
//12/13/2016                        2       mselvaga    #56467 - The list view filter for TlCaptureWorkstation should be passed as All instead of Null
//11/2/2017                         3      RDeepthi    WI#75604. Teller Window. So always refer Decimal Config
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Variables;

// * NOTE TO PROGRAMMER ****************************************************************************************//
// This is the basic structure of a Phoenix .NET window. Several points of interest are marked with "TODO".
// Template windows have been created as an instructional guide (http://teamwork.harlandfs.com/team/shark/Lists/FAQ/DispForm.aspx?ID=62)
// Additional information can be found here: http://teamwork.harlandfs.com/team/phoenixdev/Programmer%20Resources/Home.aspx
//
// TODO: Add as "link" the file pbs_dev_latest\phoenixxm.net\common\commonassembly.cs
// TODO: Change the file name and references, both here, in the designer file, and in the comments section
// *************************************************************************************************************//
namespace Phoenix.Client.Teller
{
	public partial class frmTlCaptureApplDetails : PfwStandard
	{
		#region Private Variables

		// todo: add private variables, such as those which will hold form parameters
		// example: PString _psAcctNo = new PString("AcctNo");
        TlJournal _tlJournal;
        private PInt _pnBranchNo = new PInt("BranchNo");
        private PDateTime _pdtPostingDt = new PDateTime("PostingDt");
        private PString _psBranchDesc = new PString("BranchDesc");
        private PString _psApplType = new PString("ApplType");
        private PString _RevItemsFound = new PString("RevItemsFound");
        private decimal _debitAmtTot;
        private decimal _creditAmtTot;
		/// <summary>
		/// This enum contains the various conditions which will enable/disable push buttons
		/// </summary>
		private enum EnableDisableVisible
		{
			InitBegin,
			InitComplete,
            ApplDetails
		}

		private enum CallOtherForm
		{
			EditClick
		}
		#endregion

		#region Constructors
		public frmTlCaptureApplDetails()
		{
			InitializeComponent();
		}
		#endregion

		#region Form Events
		/// <summary>
		/// Event executed before form is populated with data
		/// </summary>
		/// <returns></returns>
        private ReturnType frmTlCaptureApplDetails_PInitBeginEvent()
		{
            //Begin #75604
            if (this.Workspace.Variables["AssumeDecimal"] != null)
            {
                #region config
                PdfCurrency.ApplicationAssumeDecimal = (Convert.ToString(this.Workspace.Variables["AssumeDecimal"]).Trim() == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y); ;
                CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
                this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
                #endregion
            }
            //End #75604
            _tlJournal = new TlJournal();
			// todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureApplDetails;
            _tlJournal.BranchNo.Value = Convert.ToInt16(_pnBranchNo.Value);
            dfBranch.UnFormattedValue = _psBranchDesc.Value;
            _tlJournal.IsIncludeSupervisor.Value = "Y";
            Phoenix.Windows.Client.Helper.PopulateCombo(cmbDrawer, _tlJournal, _tlJournal.DrawerNo);
            cmbDrawer.Append(-1, "All");
            cmbDrawer.DefaultCodeValue = -1;
            cmbDrawer.SetDefaultValue();

            Phoenix.Windows.Client.Helper.PopulateCombo(cmbTlCaptureWorkstation, _tlJournal, _tlJournal.TlCaptureWorkstation);
            cmbTlCaptureWorkstation.Append(-1, "All");
            cmbTlCaptureWorkstation.DefaultCodeValue = -1;
            cmbTlCaptureWorkstation.SetDefaultValue();

            if (cmbDrawer.CodeValue != null)
            {
                this.SetTitleInfo(string.Format("{0} [Application Type: {1}][{2}]", dfBranch.Text, _psApplType.Value, "All Drawers"));
            }
            this.DefaultAction = this.pbSearch;
            if (!_RevItemsFound.IsNull && _RevItemsFound.Value == "Y")
                cbIncludeRev.Checked = true;
            dfEffectiveDate.UnFormattedValue = _pdtPostingDt.Value;
			return default(ReturnType);
		}
		#endregion

		#region Overriddes
		/// <summary>
		/// Move parameters into local variables
		/// </summary>
		public override void OnCreateParameters()
		{
			// todo: Assign parameters before calling base method
            Parameters.Add(_pnBranchNo);
            Parameters.Add(_pdtPostingDt);
            Parameters.Add(_psBranchDesc);
            Parameters.Add(_psApplType);
            Parameters.Add(_RevItemsFound);

			base.OnCreateParameters();
		}

		/// <summary>
		/// Perform additional actions during the save process
		/// </summary>
		/// <param name="isAddNext"></param>
		/// <returns></returns>
		public override bool OnActionSave(bool isAddNext)
		{
			// todo: perform additional actions before calling base method

			return base.OnActionSave(isAddNext);
		}

		/// <summary>
		/// Perform actions when closing a form
		/// </summary>
		/// <returns></returns>
		public override bool OnActionClose()
		{
			bool bRet = true;

			if (bRet)
			{
				// Validations
			}

			if (bRet)
			{
				bRet = base.OnActionClose();
			}

			if (bRet)
			{
				// Refresh Parent Window
				// CallParent( this.ScreenId );
			}

			return bRet;
		}

		/// <summary>
		/// Called by child when parent needs to perform an action
		/// </summary>
		/// <param name="paramList"></param>
		public override void CallParent(params object[] paramList)
		{
			// todo: Perform actions when called from a child window. 
			// ScreenId is the first parameter of paramList

			base.CallParent(paramList);
		}

		#endregion

		#region Methods
		/// <summary>
		/// This contains the various conditions which will enable/disable push buttons
		/// </summary>
		/// <param name="caseName"></param>
		private void EnableDisableVisibleLogic(EnableDisableVisible caseName)
		{
			// Note: If you need to enable/disable fields based on business rules, this must
			// be done in the business object.

			switch (caseName)
			{
				case EnableDisableVisible.InitComplete:
					break;
				case EnableDisableVisible.InitBegin:
					break;
			}
		}

		/// <summary>
		/// Open new form windows
		/// </summary>
		/// <param name="caseName"></param>
		private void CallOtherForms(CallOtherForm caseName)
		{
			PfwStandard tempWin = null;

			switch (caseName)
			{
				case CallOtherForm.EditClick:
					// TODO: invoke new window using the following format
					// tempWin = CreateWindow("{ASSEMBLYNAME}", "{NAMESPACE}", "{FORMNAME}");
					// tempWin.InitParameters(PhoenixVariable, PhoenixVariable....);
					break;
			}

			if (tempWin != null)
			{
				tempWin.Workspace = this.Workspace;
				// TODO: if you want a grid to refresh on this form when this window is closed,
				// set the following property: tempWin.ParentGrid = this.grid;	
				tempWin.Show();
			}
		}

		#endregion


        private void grdApplicationDetails_BeforePopulate(object sender, GridPopulateArgs e)
        {
            //Set Branch
            if (dfBranch.UnFormattedValue != null)
                _tlJournal.BranchNo.SetValue(_pnBranchNo.Value);
            else
                _tlJournal.BranchNo.SetValue(TellerVars.Instance.BranchNo);
            //Set Effective Date
            if (dfEffectiveDate.UnFormattedValue != null && !string.IsNullOrEmpty(dfEffectiveDate.Text))
                _tlJournal.EffectiveDt.SetValue(dfEffectiveDate.UnFormattedValue);
            else
                _tlJournal.EffectiveDt.SetValue(TellerVars.Instance.PostingDt);
            //Set Include Reversal
            if (cbIncludeRev.Checked)
                _tlJournal.Reversal.SetValue(2);
            else
                _tlJournal.Reversal.SetValue(0);
            //Set Drawer
            if (cmbDrawer.CodeValue != null)
                _tlJournal.DrawerNo.SetValue(cmbDrawer.CodeValue);
            else
                _tlJournal.DrawerNo.SetValue(-1);
            //Set Workstation
            //#56468
            //if (cmbTlCaptureWorkstation.CodeValue != null && cmbTlCaptureWorkstation.Description != "All")
            //    _tlJournal.TlCaptureWorkstation.SetValue(cmbTlCaptureWorkstation.Description);
            //else
            //    _tlJournal.TlCaptureWorkstation.SetValueToNull();
            if (cmbTlCaptureWorkstation.CodeValue != null)
                _tlJournal.TlCaptureWorkstation.SetValue(cmbTlCaptureWorkstation.Description);
            //Set ISN#
            if (dfTlCaptureISN.UnFormattedValue != null)
                _tlJournal.TlCaptureISN.Value = dfTlCaptureISN.Text;
            else
                _tlJournal.TlCaptureISN.SetValueToNull();
            //Set Debit/Credit Amt
            if (dfDrCrAmt.UnFormattedValue != null)
                _tlJournal.NetAmt.Value = Convert.ToDecimal(dfDrCrAmt.UnFormattedValue);
            else
                _tlJournal.NetAmt.SetValue(-1);
            //Set Appl Type
            if (!_psApplType.IsNull)
                _tlJournal.TlCaptApplTypeDesc.Value = _psApplType.Value;

            grdApplicationDetails.ListViewObject = _tlJournal;
            _tlJournal.OutputType.Value = 11; // TBD
            _debitAmtTot = 0;
            _creditAmtTot = 0;
        }

        void grdApplicationDetails_FetchRowDone(object sender, Windows.Forms.GridRowArgs e)
        {
            if (colDebitAmt.UnFormattedValue != null)
                _debitAmtTot += Convert.ToDecimal(colDebitAmt.UnFormattedValue);
            if (colCreditAmt.UnFormattedValue != null)
                _creditAmtTot += Convert.ToDecimal(colCreditAmt.UnFormattedValue);
        }
                

        private void dfEffectiveDate_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            EnableDisableVisibleLogic(EnableDisableVisible.ApplDetails);
        }

        private void frmTlCaptureApplDetails_PInitCompleteEvent()
        {

        }

        private void grdApplicationDetails_AfterPopulate(object sender, GridPopulateArgs e)
        {
            if (!_tlJournal.EffectiveDt.IsNull)
                dfEffectiveDate.SetValue(_tlJournal.EffectiveDt.Value);
            dfDebitAmtTot.SetValue(_debitAmtTot);
            dfCreditAmtTot.SetValue(_creditAmtTot);
        }

        private void pbSearch_Click(object sender, PActionEventArgs e)
        {
            grdApplicationDetails.PopulateTable();
        }

        void cmbDrawer_PhoenixUISelectedIndexChangedEvent(object sender, Windows.Forms.PEventArgs e)
        {
            if (cmbDrawer.CodeValue != null && Convert.ToInt32(cmbDrawer.CodeValue) > 0)
                this.SetTitleInfo(string.Format("{0} [Application Type: {1}][Drawer# {2}]", dfBranch.Text, _psApplType.Value, cmbDrawer.CodeValue.ToString()));
            else
                this.SetTitleInfo(string.Format("{0} [Application Type: {1}][{2}]", dfBranch.Text, _psApplType.Value, "All Drawers"));
            UpdateView();
        }

        void cmbTlCaptureWorkstation_PhoenixUISelectedIndexChangedEvent(object sender, Windows.Forms.PEventArgs e)
        {
           //string test = cmbTlCaptureWorkstation.Description;
        }

        void cmbTlCaptureWorkstation_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string test = cmbTlCaptureWorkstation.Description;
        }

	}
}