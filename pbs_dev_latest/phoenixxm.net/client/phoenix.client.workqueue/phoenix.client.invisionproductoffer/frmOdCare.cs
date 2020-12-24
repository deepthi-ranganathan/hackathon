#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2020 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmOdCare.cs
// NameSpace: Phoenix.Client.Deposit
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//12/16/2020 11:53:49 AM			1		kiran.mani  Created
//-------------------------------------------------------------------------------
#endregion

using Phoenix.BusObj.Misc;
using Phoenix.Eam.Client;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
    public partial class frmOdCare : PfwStandard
    {
        #region Private Variables
        private GraphResource resource = new GraphResource();
        private int pnRimNo;
        private int pnType;
        // todo: add private variables, such as those which will hold form parameters
        // example: PString _psAcctNo = new PString("AcctNo");

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
        public frmOdCare()
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

            return default(ReturnType);
        }
        #endregion

        #region Overriddes
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        //public override void OnCreateParameters()
        //{
        //    pnRimNo = Convert.ToInt32(Parameters[0]);
        //    pnType = Convert.ToInt32(Parameters[1]);

        //    base.OnCreateParameters();
        //}

        public override void InitParameters(params object[] paramList)// Task #84178- InitParameters is used instead of OnCreateParameters since there are 2 parameters of type Form and PwksWindow, which is not supported in OnCreateParameters.
        {
            //// Must call the base to store the parameters.
            pnRimNo = Convert.ToInt32(paramList[0]);
            pnType = Convert.ToInt32(paramList[1]);

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

        private void frmOdCare_PInitCompleteEvent()
        {
            ShowGraph();
        }
        private void PbChart_Click(object sender, Windows.Forms.PActionEventArgs e)
        {
            //ShowGraph();
        }

        public void ShowGraph()
        {
             
            CashReward _cashRWd = new CashReward();
            _cashRWd.ActionType = FrameWork.BusFrame.XmActionType.Select;
            _cashRWd.RimNo.Value = pnRimNo;
             
            if (pnType == 2)
            {
                _cashRWd.ResponseTypeId = 20;
                txtDescription.Text = "Autotransfer setup based on frequent OD";
            }
            else if (pnType == 3)
            {
                pbChart.Enabled = false;
                txtDescription.Text = "Evaluate financial status";
                _cashRWd.ResponseTypeId = 21;
            }
            Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, _cashRWd);
            //_cashRWd.DoAction(CoreService.DbHelper);

            string graphName1 = resource.GetLoadingSwf();
            //string graphName1 = @"C:\Users\kiran.mani\swf\loading.swf";  //( cmbGraphType.CodeValue.ToString() );
            // The align mode consists of bit flags. (Left=+1, Right=+2, Top=+4, Bottom=+8). 
            flashObject.AlignMode = 16;
            if (flashObject.Movie != graphName1)
                flashObject.Movie = graphName1;
            //Scale mode (0=ShowAll, 1= NoBorder, 2 = ExactFit) 
            flashObject.ScaleMode = 3;

            //
            string graphName = resource.GetFileName("Column3D");
            if (pnType == 3)
            {
                graphName = resource.GetFileName("MSColumn2D");
            }
             
            bool newData = false;
            //#39688 - Switch off the right click menu
            flashObject.Menu = false;
            //

            // Set the Movie Name
            if (flashObject.Movie != graphName)
                flashObject.Movie = graphName;
            else
                newData = true;
            // TL
            string chartWidth = (this.flashObject.Width - 2).ToString();
            string chartHeight = (this.flashObject.Height - 2).ToString();
            //
            //int scaleMode = 1;
            int _scaleMode = 2;     //#15817 - changing scale mode to exactfit always,
            if (!PApplication.Instance.Is800x600 && this.flashObject.Width > 500 && this.flashObject.Height > 360)
            {
                chartWidth = "";
                chartHeight = "";
                //_scaleMode = 2;
                flashObject.AlignMode = 16;

            }
            else
            {
                //_scaleMode = 3;
                //chartWidth = ( this.flashObject.Width > 20 ? (this.flashObject.Width - 20).ToString():this.flashObject.Width.ToString());
                //chartHeight = ( this.flashObject.Height > 20 ? (this.flashObject.Height - 20).ToString():this.flashObject.Height.ToString());
                flashObject.AlignMode = 5;
            }

            // ExactFit
            flashObject.ScaleMode = _scaleMode;
            flashObject.FlashVars = string.Format("&chartWidth={0}&chartHeight={1}", chartWidth, chartHeight);
            flashObject.SetVariable("_root.chartWidth", chartWidth);
            flashObject.SetVariable("_root.chartHeight", chartHeight);
            //flashObject.SetVariable("_root.dataURL","");
            //flashObject.SetVariable("_root.dataXML","" );

            //

            //string graphXml = System.IO.File.ReadAllText(@"C:\Users\kiran.mani\swf\xmlText.txt");
            string graphXml = _cashRWd.GraphXml.Value;



            if (newData)
            {
                //Set the flag
                flashObject.SetVariable("_root.isNewData", "1");
                //
                flashObject.SetVariable("_root.newData", graphXml);
                //	
            }//Set the actual data
            else
            {
                flashObject.SetVariable("_root.dataXML", graphXml);
                //
            }
            flashObject.ScaleMode = _scaleMode;
            flashObject.TGotoLabel("/", "JavaScriptHandler");
            //flashObject.ScaleMode  = scaleMode;


        }
        //public void ShowGraph()
        //{
        //    string graphName1 = @"C:\Users\kiran.mani\AppData\Local\Temp\loading.swf";  //( cmbGraphType.CodeValue.ToString() );
        //                                                 // The align mode consists of bit flags. (Left=+1, Right=+2, Top=+4, Bottom=+8). 
        //    flashObject.AlignMode = 16;
        //    if (flashObject.Movie != graphName1)
        //        flashObject.Movie = graphName1;
        //    //Scale mode (0=ShowAll, 1= NoBorder, 2 = ExactFit) 
        //    flashObject.ScaleMode = 3;
        //    //
        //    //string graphName = resource.GetFileName(cmbGraphType.CodeValue.ToString());
        //    string graphName = @"C:\Users\kiran.mani\AppData\Local\Temp\FC_2_3_Column3D.swf";
        //    bool newData = false;
        //    //#39688 - Switch off the right click menu
        //    flashObject.Menu = false;
        //    //

        //    // Set the Movie Name
        //    if (flashObject.Movie != graphName)
        //        flashObject.Movie = graphName;
        //    else
        //        newData = true;
        //    // TL
        //    string chartWidth = (this.flashObject.Width - 2).ToString();
        //    string chartHeight = (this.flashObject.Height - 2).ToString();
        //    //
        //    //int scaleMode = 1;
        //    int _scaleMode = 2;     //#15817 - changing scale mode to exactfit always,
        //    if (!PApplication.Instance.Is800x600 && this.flashObject.Width > 500 && this.flashObject.Height > 360)
        //    {
        //        chartWidth = "";
        //        chartHeight = "";
        //        //_scaleMode = 2;
        //        flashObject.AlignMode = 16;

        //    }
        //    else
        //    {
        //        //_scaleMode = 3;
        //        //chartWidth = ( this.flashObject.Width > 20 ? (this.flashObject.Width - 20).ToString():this.flashObject.Width.ToString());
        //        //chartHeight = ( this.flashObject.Height > 20 ? (this.flashObject.Height - 20).ToString():this.flashObject.Height.ToString());
        //        flashObject.AlignMode = 5;
        //    }

        //    // ExactFit
        //    flashObject.ScaleMode = _scaleMode;
        //    flashObject.FlashVars = string.Format("&chartWidth={0}&chartHeight={1}", chartWidth, chartHeight);
        //    flashObject.SetVariable("_root.chartWidth", chartWidth);
        //    flashObject.SetVariable("_root.chartHeight", chartHeight);
        //    //flashObject.SetVariable("_root.dataURL","");
        //    //flashObject.SetVariable("_root.dataXML","" );

        //    //

        //    string graphXml = System.IO.File.ReadAllText(@"C:\Users\kiran.mani\AppData\Local\Temp\xmlText.txt");



        //    if (newData)
        //    {
        //        //Set the flag
        //        flashObject.SetVariable("_root.isNewData", "1");
        //        //
        //        flashObject.SetVariable("_root.newData", graphXml);
        //        //	
        //    }//Set the actual data
        //    else
        //    {
        //        flashObject.SetVariable("_root.dataXML", graphXml);
        //        //
        //    }
        //    flashObject.ScaleMode = _scaleMode;
        //    flashObject.TGotoLabel("/", "JavaScriptHandler");
        //    //flashObject.ScaleMode  = scaleMode;


        //}
    }
}