#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2014 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlCaptureViewPendingEOB.cs
// NameSpace: Phoenix.Client.TlSupervisor
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//5/8/2014 4:13:49 PM			1		rpoddar  Created
//02/05/2016                    2       mselvaga    #41398 - EHF - Pending EOBs are not displaying correctly through Supervisor Functions.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
	public partial class frmTlCaptureViewPendingEOB : PfwStandard
	{
		#region Private Variables

		// todo: add private variables, such as those which will hold form parameters
		// example: PString _psAcctNo = new PString("AcctNo");
        TlJournal _tlJournal;
		/// <summary>
		/// This enum contains the various conditions which will enable/disable push buttons
		/// </summary>
		private enum EnableDisableVisible
		{
			InitBegin,
			InitComplete
		}

		private enum CallOtherForm
		{
			EditClick
		}
		#endregion

		#region Constructors
		public frmTlCaptureViewPendingEOB()
		{
			InitializeComponent();
		}
		#endregion

		#region Form Events
		/// <summary>
		/// Event executed before form is populated with data
		/// </summary>
		/// <returns></returns>
        private ReturnType frmTlCaptureViewPendingEOB_PInitBeginEvent()
		{
            _tlJournal = new TlJournal();
			// todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureViewPendingEOB;
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


        private void grdPendingEOB_BeforePopulate(object sender, GridPopulateArgs e)
        {
            if (cbAllBranches.Checked)
                _tlJournal.BranchNo.SetValue(null);
            else
                _tlJournal.BranchNo.SetValue(TellerVars.Instance.BranchNo);

            if (dfEffectiveDate.UnFormattedValue != null && !string.IsNullOrEmpty(dfEffectiveDate.Text))
                _tlJournal.EffectiveDt.SetValue(dfEffectiveDate.UnFormattedValue);
            else
                _tlJournal.EffectiveDt.SetValue(null);
            _tlJournal.TlCaptureWorkstation.Value = "ALL";  //#41398
            grdPendingEOB.ListViewObject = _tlJournal;
            _tlJournal.OutputType.Value = 9; // TBD
        }
                

        private void dfEffectiveDate_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            grdPendingEOB.PopulateTable();
        }

        private void cbAllBranches_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            grdPendingEOB.PopulateTable();
        }

        private void frmTlCaptureViewPendingEOB_PInitCompleteEvent()
        {

        }

        private void grdPendingEOB_AfterPopulate(object sender, GridPopulateArgs e)
        {
            if (_tlJournal.EffectiveDt.IsNull && _tlJournal.OtherInfo.Contains("EffectiveDtEOB"))
                _tlJournal.EffectiveDt.Value = _tlJournal.OtherInfo["EffectiveDtEOB"].DateTimeValue;

            if (!_tlJournal.EffectiveDt.IsNull)
                dfEffectiveDate.SetValue(_tlJournal.EffectiveDt.Value);
        }

        private void pbRefresh_Click(object sender, PActionEventArgs e)
        {
            grdPendingEOB.PopulateTable();
        }

	}
}