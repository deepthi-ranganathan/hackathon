#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2014 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlCaptureApplTotals.cs
// NameSpace: Phoenix.Client.TlSupervisor
//-------------------------------------------------------------------------------
//Date							    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/26/2016 12:15:49 PM			1		mselvaga    #52625 - Created
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
	public partial class frmTlCaptureApplTotals : PfwStandard
	{
		#region Private Variables

		// todo: add private variables, such as those which will hold form parameters
		// example: PString _psAcctNo = new PString("AcctNo");
        TlJournal _tlJournal;
        private PInt _pnBranchNo = new PInt("BranchNo");
        private PDateTime _pdtPostingDt = new PDateTime("PostingDt");
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
			EditClick,
            ApplDetails
		}
		#endregion

		#region Constructors
		public frmTlCaptureApplTotals()
		{
			InitializeComponent();
		}
		#endregion

		#region Form Events
		/// <summary>
		/// Event executed before form is populated with data
		/// </summary>
		/// <returns></returns>
        private ReturnType frmTlCaptureApplTotals_PInitBeginEvent()
		{
            _tlJournal = new TlJournal();
			// todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureApplTotals;
            Phoenix.Windows.Client.Helper.PopulateCombo(cmbBranch, _tlJournal, _tlJournal.BranchNo);
            cmbBranch.Append("-1", "All");
            cmbBranch.DefaultCodeValue = _pnBranchNo.Value;
            cmbBranch.SetDefaultValue();
            if (cmbBranch.CodeValue != null)
            {
                if (Convert.ToString(cmbBranch.Description) == "All")
                    this.SetTitleInfo("All Branches");
                else
                    this.SetTitleInfo(cmbBranch.Description);
            }
            this.DefaultAction = this.pbSearch;
            dfRevItemsLegend.Text = "*The Application Type includes reversed transaction(s).";
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
                case EnableDisableVisible.ApplDetails:
                    if ((cmbBranch.CodeValue != null && Convert.ToInt32(cmbBranch.CodeValue) > 0) &&
                        (dfEffectiveDate.UnFormattedValue != null && !string.IsNullOrEmpty(dfEffectiveDate.Text)) &&
                        ((colDebitAmt.UnFormattedValue != null && Convert.ToDecimal(colDebitAmt.UnFormattedValue) > 0) ||
                        (colCreditAmt.UnFormattedValue != null && Convert.ToDecimal(colCreditAmt.UnFormattedValue) > 0) ||
                        (colRevItemsFound.UnFormattedValue != null && colRevItemsFound.Text == "Y")))
                        this.pbApplDetails.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                    else
                        this.pbApplDetails.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
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
                case CallOtherForm.ApplDetails:
                tempWin = CreateWindow("phoenix.client.tlsupervisor", "Phoenix.Client.Teller", "frmTlCaptureApplDetails");
                tempWin.InitParameters(
                    Convert.ToInt32(cmbBranch.CodeValue),
                    Convert.ToDateTime(dfEffectiveDate.UnFormattedValue),
                    cmbBranch.Description,
                    colApplType.Text,
                    colRevItemsFound.Text);
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

        void grdApplicationTotals_SelectedIndexChanged(object source, Windows.Forms.GridClickEventArgs e)
        {
            EnableDisableVisibleLogic(EnableDisableVisible.ApplDetails);
        }


        private void grdApplicationTotals_BeforePopulate(object sender, GridPopulateArgs e)
        {
            //Set Branch
            if (cmbBranch.CodeValue != null)
                _tlJournal.BranchNo.SetValue(cmbBranch.CodeValue);
            else
                _tlJournal.BranchNo.SetValue(0);
            //Set Effective Date
            if (dfEffectiveDate.UnFormattedValue != null && !string.IsNullOrEmpty(dfEffectiveDate.Text))
                _tlJournal.EffectiveDt.SetValue(dfEffectiveDate.UnFormattedValue);
            else
                _tlJournal.EffectiveDt.SetValue(TellerVars.Instance.PostingDt);

            grdApplicationTotals.ListViewObject = _tlJournal;
            _tlJournal.OutputType.Value = 10; // TBD
            _debitAmtTot = 0;
            _creditAmtTot = 0;
        }

        void grdApplicationTotals_FetchRowDone(object sender, Windows.Forms.GridRowArgs e)
        {
            if (colDebitAmt.UnFormattedValue != null)
                _debitAmtTot += Convert.ToDecimal(colDebitAmt.UnFormattedValue);
            if (colCreditAmt.UnFormattedValue != null)
                _creditAmtTot += Convert.ToDecimal(colCreditAmt.UnFormattedValue);
            colApplType.UnFormattedValue = colApplTypeDesc.UnFormattedValue;
            if (colRevItemsFound.UnFormattedValue != null && colRevItemsFound.Text == "Y")
                colApplTypeDesc.UnFormattedValue = Convert.ToString(colApplTypeDesc.UnFormattedValue) + " * ";
        }
                

        private void dfEffectiveDate_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            EnableDisableVisibleLogic(EnableDisableVisible.ApplDetails);
            grdApplicationTotals.PopulateTable();
        }

        void cmbBranch_PhoenixUISelectedIndexChangedEvent(object sender, Windows.Forms.PEventArgs e)
        {
            EnableDisableVisibleLogic(EnableDisableVisible.ApplDetails);
            //this.SetTitleInfo(cmbBranch.Description);
            if (Convert.ToString(cmbBranch.Description) == "All")
                this.SetTitleInfo("All Branches");
            else
                this.SetTitleInfo(cmbBranch.Description);
            this.UpdateView();
            grdApplicationTotals.PopulateTable();
        }

        private void frmTlCaptureApplTotals_PInitCompleteEvent()
        {

        }

        private void grdApplicationTotals_AfterPopulate(object sender, GridPopulateArgs e)
        {
            if (!_tlJournal.EffectiveDt.IsNull)
                dfEffectiveDate.SetValue(_tlJournal.EffectiveDt.Value);
            dfDebitAmtTot.SetValue(_debitAmtTot);
            dfCreditAmtTot.SetValue(_creditAmtTot);
            if (grdApplicationTotals.Count > 0)
                EnableDisableVisibleLogic(EnableDisableVisible.ApplDetails);
        }

        private void pbSearch_Click(object sender, PActionEventArgs e)
        {
            grdApplicationTotals.PopulateTable();
        }

        void pbApplDetails_Click(object sender, Windows.Forms.PActionEventArgs e)
        {
            CallOtherForms(CallOtherForm.ApplDetails);
        }
	}
}