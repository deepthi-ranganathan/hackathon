#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2020 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmCustAnalysis.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//12/16/2020 5:57:18 PM			1		deepthi.ranganathan  Created
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
using Phoenix.BusObj.Misc;
using Phoenix.Windows.Client;

// * NOTE TO PROGRAMMER ****************************************************************************************//
// This is the basic structure of a Phoenix .NET window. Several points of interest are marked with "TODO".
// Template windows have been created as an instructional guide (http://teamwork.harlandfs.com/team/shark/Lists/FAQ/DispForm.aspx?ID=62)
// Additional information can be found here: http://teamwork.harlandfs.com/team/phoenixdev/Programmer%20Resources/Home.aspx
//
// TODO: Add as "link" the file pbs_dev_latest\phoenixxm.net\common\commonassembly.cs
// TODO: Change the file name and references, both here, in the designer file, and in the comments section
// *************************************************************************************************************//
namespace Phoenix.Client.WorkQueue
{
    public partial class frmCustAnalysis : PfwStandard
    {
        #region Private Variables

        // todo: add private variables, such as those which will hold form parameters
        // example: PString _psAcctNo = new PString("AcctNo");
        private int pnRimNo;
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
        public frmCustAnalysis()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Events
        /// <summary>
        /// Event executed before form is populated with data
        /// </summary>
        /// <returns></returns>
        private ReturnType form_PInitBeginEvent()
        {
            // todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc

            // To make sure any changes made to values in a bus obj are displayed correctly on parent screens, make sure you set the UseStateFromBusinessObject 
            this.UseStateFromBusinessObject = true;
            // set business object for edit/select/new/update for this form. Must happen after UseStateFromBusinessObject
            // this.MainBusinesObject = [BO];
            CashReward _cashRWd = new CashReward();
            _cashRWd.RimNo.Value = pnRimNo;
            _cashRWd.ResponseTypeId = 15;
            grdCustAnalDetails.ListViewObject = _cashRWd;
            grdCustAnalDetails.PopulateTable();
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
        public override void InitParameters(params object[] paramList)// Task #84178- InitParameters is used instead of OnCreateParameters since there are 2 parameters of type Form and PwksWindow, which is not supported in OnCreateParameters.
        {
            //// Must call the base to store the parameters.
            pnRimNo = Convert.ToInt32(paramList[0]);
            
            base.InitParameters(paramList);
            
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

        private void pbDetail_Click(object sender, PActionEventArgs e)
        {
            if (Convert.ToInt32(colUseCaseID.Text) == 2 || Convert.ToInt32(colUseCaseID.Text) == 3)
            {
                PfwStandard tempDlg = null;
                Form _parentWindow = null;
                IPhoenixForm _parentForm;
                DialogResult dialogResult = DialogResult.None;
                _parentForm = _parentWindow as PfwStandard;
                tempDlg = Helper.CreateWindow("phoenix.client.invisionproductoffer", "Phoenix.Client.WorkQueue", "frmOdCare");

                tempDlg.InitParameters(pnRimNo, Convert.ToInt32(colUseCaseID.Text));

                if (tempDlg != null)
                {
                    dialogResult = tempDlg.ShowDialog();
                }
            }
            else
            {

                PfwStandard tempWin = null;
                if (Convert.ToInt16(colUseCaseID.UnFormattedValue) == 1)
                {
                    tempWin = CreateWindow("phoenix.client.CashReward", "phoenix.client.CashReward", "frmCashRwdAcctList");
                    tempWin.InitParameters(pnRimNo);
                }
                if (tempWin != null)
                {
                    tempWin.Workspace = this.Workspace;
                    tempWin.Show();
                }
            }
        
        }
    }
}