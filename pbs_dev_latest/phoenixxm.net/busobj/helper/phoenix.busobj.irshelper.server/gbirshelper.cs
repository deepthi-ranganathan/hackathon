#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GbIrsHelper.cs
// NameSpace: phoenix.busobj.irshelper
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//8/20/2018     1       Chinju T    Task 95014:Dev Task-209333-Alternate address selection in The Recipient Information tab in Add New /Edit Existing form window
//11/05/2018    2       Chinju T    Task 104153:Dev Task-209333-Enable Filing Status Combo Box in Add New/ Edit Existing Window
//11/8/2018     3       Chinju T    Task 104153:Handled corrected Filed to corrected pending status change

//-------------------------------------------------------------------------------

#endregion

using Phoenix.BusObj.Global.Server;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.BusObj.IrsHelper.Server
{
    public class GbIrsHelper : Phoenix.BusObj.IrsHelper.GbIrsHelper
    {
        #region Private Variables
        private GbHelper _gbHelper = new GbHelper();//#71457
        #endregion


        #region public constructors
        public GbIrsHelper() : base()
        {

        }
        #endregion
        protected override void InitializeMap()
        {
            base.InitializeMap();
        }
        protected override bool OnActionCustom(IDbHelper dbHelper, bool isPrimaryDb)    //#76425
        {

            if (this.CustomActionName == "GetDefaultAddrId")
            {

                int rimNo = (_paramNodes[0] as PBaseType).IntValue;
                PopulateRecipPayerInfo(dbHelper, rimNo);
            }
            //Begin #104153
            else if (this.CustomActionName == "SetCorrectedFlag")
            {
                string filingStatus = (_paramNodes[0] as PBaseType).StringValue;
                string prevFilingStatus = (_paramNodes[1] as PBaseType).StringValue;
                bool eligibleForStatusChange = (_paramNodes[2] as PBaseType).BooleanValue;
                SetCorrectedFlag(filingStatus, prevFilingStatus, eligibleForStatusChange);
            }
            //End #104153

            return true;
        }
        private void PopulateRecipPayerInfo(IDbHelper dbHelper, int pnRimNo)
        {
            PInt addrId = new PInt("addrId");
            //Select Rim/Recipient details
            if (pnRimNo > 0)
            {
                StringBuilder sSql = new StringBuilder();
                sSql.AppendFormat(@"Select MAX(X.ADDR_ID)
								  From {0}RM_ADDRESS X
								  Where X.RIM_NO = {1}
								  And ( ( X.ADDR_TYPE_ID = 10 And '{2}' Between X.START_DT AND X.End_DT And X.STATUS = 'Active'  )
									Or X.ADDR_TYPE_ID = 1)", dbHelper.DbPrefix, pnRimNo, BusGlobalVars.SystemDate.ToShortDateString());
                if (!_gbHelper.ExecSqlImmediateInto(dbHelper, sSql.ToString(), addrId))
                {
                    return;
                }
             (_paramNodes[1] as PBaseType).ValueObject = addrId.Value;
            }
        }
        //Begin #104153
        private void SetCorrectedFlag(string filingStatus, string prevFilingStatus, bool eligibleForStatusChange)
        {
            string corrected = null;
            if (filingStatus == "Original Filed" && prevFilingStatus == "Original Filed") //User clicks on Yes  for message 15851 (i.e., automatically save corrected pending by system) or user manually changes Original Filed to Corrected Pending.            
            {
                if (eligibleForStatusChange)
                {
                    corrected = "G";
                    filingStatus = "Corrected Pending";
                }
            }
            else if (filingStatus.Contains("Corrected") && prevFilingStatus.Contains("Original"))  //When user changes from Original Filed to Corrected filed status.
            {
                corrected = "G";
            }
            else if (filingStatus.Contains("Original") &&
                    (prevFilingStatus.Contains("Corrected")))
            {
                corrected = null;
            }
            else if (filingStatus.Contains("Corrected") &&
                  (prevFilingStatus.Contains("Corrected")))
            {
                corrected = "G";
            }
            (_paramNodes[0] as PBaseType).ValueObject = filingStatus;
            (_paramNodes[3] as PBaseType).ValueObject = corrected;
        }
        //End #104153

    }
}
