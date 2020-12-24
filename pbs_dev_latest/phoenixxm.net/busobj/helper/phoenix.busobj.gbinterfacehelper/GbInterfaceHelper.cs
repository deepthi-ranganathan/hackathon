#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GbInterfaceHelper.cs
// NameSpace: Phoenix.BusObj.Global.Helper
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/13/2013	1.0		bschlottman	#23784 - .Net Port the dfwRm Created
//02/05/2014    2       BSchlottman #26415 Register HFSChkCnt
//1/20/2014     3       33341 - added code to support check center
//2/2/2015      4       34651 - MBachala -  contact history is not logging everytime you click on check order
//2/4/2015      5       34651 - MBachala - contact date should be calendar date and time and acct_no/acct_type should be populated
//2/26/2015     6       DGarcia     WI#29051 - Added code to handle error retrieving credentials for both EFUND And CHKCNTR.
//7/21/2017     7       Swenzy      UserStory#68301: Task #68302: - Changed code due to the migration of dll
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;           //#26415
using System.IO;                    //#26415
using System.Linq;
using System.Reflection;            //#26415
using System.Text;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.BusObj.RIM;   //33341

namespace Phoenix.BusObj.Global
{
    
    public class GbInterfaceHelper: BusObjectBase
    {
        Phoenix.BusObj.RIM.RmContactHist _rmContactHistory = new Phoenix.BusObj.RIM.RmContactHist();     //33341

        public void LaunchInterface(string application, int rimNo, string acctType, string acctNo)
        {
            PString sClientId = new PString("sClientId","");
            PString sRimNo = new PString("sRimNo", Convert.ToString(rimNo));
            PString sKeyFilePath = new PString("sKeyFilePath", "");
            PString sUserName = new PString("sUserName", "");
            PString sPassword = new PString("sPassword", "");
            PString sApplication = new PString("sApplication", "");
            PString sUrl = new PString("sUrl", "");
            PString sId = new PString("sId", "");
            PString sPath = new PString("sPath", "");
            PString sOptional = new PString("sOptional", "");
            PString sErroHandling = new PString("sErroHandling"); //WI#29051 

            string[] words;
            int i;
            PString sAcctNo = new PString("AcctNo", acctNo);        // 33341
            PString sAcctType = new PString("AcctType", acctType);  //33341

            if (application != "EFUNDS" && application != "CHKCNTR")
            {
                Messages.AddError(9433, null, string.Empty);
            }

            CoreService.DataService.ProcessCustomAction(this, "GetClientId", sClientId);

            sKeyFilePath.Value = sClientId.Value + ".key";
            //sKeyFilePath.Value = "C:\\Phoenix\\us2014\\runtime\\" + sKeyFilePath.Value;
            sKeyFilePath.Value = CurrentAssemblyDirectory() + Path.DirectorySeparatorChar + sKeyFilePath.Value; //#26415

            if (application.ToUpper().Trim() == "CHKCNTR")      // 33341
            {
                CoreService.DataService.ProcessCustomAction(this, "GetChkCntrData", sRimNo, sAcctNo, sAcctType, sKeyFilePath, sUserName, sPassword, sApplication, sUrl, sId, sPath, sOptional, sErroHandling);
                if (sErroHandling.Value != "0")
                {
                    this.Messages.AddError(15109, null, string.Empty);
                    return;
                }
            }
            else
            {
                CoreService.DataService.ProcessCustomAction(this, "GetEfundsData", sRimNo, sKeyFilePath, sUserName, sPassword, sApplication, sUrl, sId, sPath, sOptional, sErroHandling);
                if (sErroHandling.Value != "0")
                {
                    this.Messages.AddError(15108, null, string.Empty);
                    return;
                }
            }

            Register_HFSChkCnt();   //#26415
            
            Phoenix.Interop.HFSChkCnt.Interface.HFSChecksCenterObj _checkCenterObj = new Phoenix.Interop.HFSChkCnt.Interface.HFSChecksCenterObj();

            //set the properties here
            _checkCenterObj.UserName = sUserName.Value;
            _checkCenterObj.Password = sPassword.Value;
            _checkCenterObj.Application = sApplication.Value;
            //_checkCenterObj.Application = "SECURITY_TESTING";       // end to end testing only
            _checkCenterObj.URL = sUrl.Value;
            _checkCenterObj.ClientID = sClientId.Value;
            _checkCenterObj.KeyFilePath = sKeyFilePath.Value;

            words = sOptional.Value.Split('~');
            for (i = 1; i < words.Length; i = i + 2)
            {
                if (i < (words.Length - 1))
                {
                    _checkCenterObj.AddOptionalFld(words[i], words[i + 1]);
                }
                else
                {
                    break;
                }
            }

            bool isOk = true;
            if (words.Length > i)
            {
                if (words[i] != "0")    //nRC
                {
                    isOk = false;
                    this.Messages.AddError(9441, null, string.Empty);
                }
            }

            if (isOk)
            {
                //launch the interface
                _checkCenterObj.Launch();
            }

            // 33341 - added code to log into rm_contact_hist
            if (_rmContactHistory == null)
            {
                _rmContactHistory = new RmContactHist();
            }
            if (application.ToUpper().Trim() == "CHKCNTR")
            {
                _rmContactHistory.ContactType.Value = 20;
                _rmContactHistory.ContactInfo.Value = "Checks.Center Access";
            }
            else
            {
                _rmContactHistory.ContactType.Value = 45;
                _rmContactHistory.ContactInfo.Value = "eFunds Access";
            }
            _rmContactHistory.RimNo.Value = rimNo;
            _rmContactHistory.ContactDetails.Value = "";
            _rmContactHistory.SkipCOCheck.Value = true;     // 34651
            _rmContactHistory.ContactDt.Value = DateTime.Now; // 34651
            _rmContactHistory.AcctType.Value = acctType;        //34651
            _rmContactHistory.AcctNo.Value = acctNo;            //34651
            _rmContactHistory.ActionType = XmActionType.New;
            CoreService.DataService.ProcessRequest(_rmContactHistory);
        }
        //begin #26415
        /// <summary>
        /// Register hfschkcnt.dll. 
        /// </summary>
        /// <returns></returns>
        public bool Register_HFSChkCnt()
        {
            bool operationFailed = false;

            if (!(System.Environment.OSVersion.Version.Major == 6 && System.Environment.OSVersion.Version.Minor == 2)) // !WinVista
            {
                ProcessStartInfo psi = new ProcessStartInfo();

                //psi.CreateNoWindow = true;
                psi.FileName = "regsvr32.exe";
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.WorkingDirectory = Phoenix.FrameWork.Core.AppInfo.Instance.AssemblyDir;
                psi.Arguments = @"/s hfschkcnt.dll";
                psi.UseShellExecute = false;
                System.Diagnostics.Process registerOCX;

                try
                {
                    registerOCX = System.Diagnostics.Process.Start(psi);
                    registerOCX.WaitForExit();
                    if (registerOCX.ExitCode != 0)
                    {
                        operationFailed = true;
                    }
                }
                catch
                {
                    operationFailed = true;

                }
            }
            return operationFailed;
        }

        public string CurrentAssemblyDirectory()
        {
            //string codeBase = Environment.CurrentDirectory;
            //UriBuilder uri = new UriBuilder(codeBase);
            //string path = Uri.UnescapeDataString(uri.Path);
            return Environment.CurrentDirectory;
        }
        //end   #26415
        #region Constructor
        public GbInterfaceHelper() :
			base()
		{
			InitializeMap();
		}
		#endregion

        #region Initialize method
        protected override void InitializeMap()
        {
            #region Table Mapping
            this.DbObjects.Add("GB_INTERFACE_HELPER", "X_GB_INTERFACE_HELPER");
            #endregion Table Mapping

            #region  Column Mapping
            #endregion

            #region  Keys
            #endregion

            #region  Enumerable Values
            #endregion

            #region  Supported Actions
            this.SupportedAction |= XmActionType.Update | XmActionType.New | XmActionType.Delete | XmActionType.Custom;
            this.SupportedActions24x7 |= XmActionType.Custom;
            this.IsOfflineSupported = true;
            #endregion

            base.InitializeMap();
        }
        #endregion
    }
}
