#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion

//-------------------------------------------------------------------------------
// File Name: GbInterfaceHelper.Server.cs
// NameSpace: Phoenix.BusObj.Global.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/13/2013	1		bschlottman	#23784   - .Net Port the dfwRm Created
//1/13/2015     2       MBachala    33341    - added chkcntr
//2/26/2015     3       DGarcia     WI#29051 - Added code to handle error retrieving credentials for both EFUND And CHKCNTR.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Shared;
using Phoenix.Shared.BusFrame;
using Phoenix.Shared.Utility;

namespace Phoenix.BusObj.Global.Server
{
    public class GbInterfaceHelper : Phoenix.BusObj.Global.GbInterfaceHelper
    {
        protected PString sSQL = new PString("sSQL");
        private SqlHelper sqlHelper = new SqlHelper();

        #region InitializeMap
        protected override void InitializeMap()
        {
            base.InitializeMap();
            // Define Constraints

            // Define Event Handlers
        }
        #endregion

        #region ******** Custom Action ***********
        protected override bool OnActionCustom(IDbHelper dbHelper, bool isPrimaryDb)
        {
            if (CustomActionName == "GetClientId")
            {
                GetClientId(dbHelper);
            }
            else if (CustomActionName == "GetEfundsData")
            {
                GetEfundsData(dbHelper);
            }
            else if (CustomActionName == "GetChkCntrData")
            {
                GetChkCntrData(dbHelper);
            }

            return true;
        }

        public void GetClientId(IDbHelper dbHelper)
        {
            PString sClientId = new PString("sClientId");

            sClientId.Value = "0";


            sSQL.Value = string.Format(@" select a.client_id 
	                    from {0}ad_gb_check_order a", DbPrefix);
            sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, false, false, sClientId);

            (_paramNodes[0] as PBaseType).ValueObject = sClientId.Value;
        }

        public void GetEfundsData(IDbHelper dbHelper)
        {
            PString sRimNo = new PString("sRimNo");
            PString sRimType = new PString("sRimType");
            PString sResult = new PString("sResult");
            PString sResult1 = new PString("sResult1");
            PString sResult2 = new PString("sResult2");
            PString sResult3 = new PString("sResult3");
            PString sResult4 = new PString("sResult4");
            PString sResult5 = new PString("sResult5");
            PString sKeyFile = new PString("sKeyFile");
            PString sUserName = new PString("sUserName");
            PString sPassword = new PString("sPassword");
            PString sApplication = new PString("sApplication");
            PString sUrl = new PString("sUrl");
            PString sId = new PString("sId");
            PString sPath = new PString("sPath");
            PString sOptional = new PString("sOptional","");
            PString sErroHandling = new PString("sErroHandling"); //WI#29051 

            int nRow = 0;

            sRimNo.Value = Convert.ToString((ParamNodes[0] as PBaseType).ValueObject);
            sKeyFile.Value = Convert.ToString((ParamNodes[1] as PBaseType).ValueObject);

            sSQL.Value = string.Format(@"Declare @nRC int
    exec @nRC = {0}psp_eforms_efunds_data '{1}', '{2}', 'USER1', '{3}', 0, null
    Select convert(varchar, @nRC)", DbPrefix, BusGlobalVars.SystemDate, sRimNo.Value, sKeyFile.Value);
            sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, false, true, sResult);

            do
            {
                switch (nRow)
                {
                    case 0:
                        sRimType.Value = StringHelper.StrTrimX(sResult.Value);
                        break;
                    case 1:
                        sUserName.Value = StringHelper.StrTrimX(sResult.Value);
                        sPassword.Value = StringHelper.StrTrimX(sResult1.Value);
                        sApplication.Value = StringHelper.StrTrimX(sResult2.Value);
                        sUrl.Value = StringHelper.StrTrimX(sResult3.Value);
                        sId.Value = StringHelper.StrTrimX(sResult4.Value);
                        sPath.Value = StringHelper.StrTrimX(sResult5.Value);
                        break;
                    default:
                        sOptional.Value = sOptional.Value + "~" + StringHelper.StrTrimX(sResult.Value);
                        break;
                }
                nRow++;
            }
            while (nRow == 1 ? sqlHelper.ExecSqlGetNextRow(true, sResult, sResult1, sResult2, sResult3, sResult4, sResult5) : sqlHelper.ExecSqlGetNextRow(true, sResult));

            (_paramNodes[2] as PBaseType).ValueObject = sUserName.Value;
            (_paramNodes[3] as PBaseType).ValueObject = sPassword.Value;
            (_paramNodes[4] as PBaseType).ValueObject = sApplication.Value;
            (_paramNodes[5] as PBaseType).ValueObject = sUrl.Value;
            (_paramNodes[6] as PBaseType).ValueObject = sId.Value;
            (_paramNodes[7] as PBaseType).ValueObject = sPath.Value;
            (_paramNodes[8] as PBaseType).ValueObject = sOptional.Value;
            (_paramNodes[9] as PBaseType).ValueObject = sResult.Value; //WI#29051 
        }

        public void GetChkCntrData(IDbHelper dbHelper)
        {
            PString sRimNo = new PString("sRimNo");
            PString sRimType = new PString("sRimType");
            PString sResult = new PString("sResult");
            PString sResult1 = new PString("sResult1");
            PString sResult2 = new PString("sResult2");
            PString sResult3 = new PString("sResult3");
            PString sResult4 = new PString("sResult4");
            PString sResult5 = new PString("sResult5");
            PString sKeyFile = new PString("sKeyFile");
            PString sUserName = new PString("sUserName");
            PString sPassword = new PString("sPassword");
            PString sApplication = new PString("sApplication");
            PString sUrl = new PString("sUrl");
            PString sId = new PString("sId");
            PString sPath = new PString("sPath");
            PString sOptional = new PString("sOptional", "");
            PString sErroHandling = new PString("sErroHandling"); //WI#29051 

            int nRow = 0;
            PString sAcctNo = new PString("sAcctNo");
            PString sAcctType = new PString("sAcctType");

            sRimNo.Value = Convert.ToString((ParamNodes[0] as PBaseType).ValueObject);
            sAcctNo.Value = Convert.ToString((ParamNodes[1] as PBaseType).ValueObject);
            sAcctType.Value = Convert.ToString((ParamNodes[2] as PBaseType).ValueObject);
            sKeyFile.Value = Convert.ToString((ParamNodes[3] as PBaseType).ValueObject);

            sSQL.Value = string.Format(@"Declare @nRC int
    exec @nRC = {0}psp_eforms_chk_center_data '{1}', '{2}', '{3}', '{4}','','{5}', 0, null
    Select convert(varchar, @nRC)", DbPrefix, BusGlobalVars.SystemDate, sRimNo.Value,sAcctType.Value,sAcctNo.Value, sKeyFile.Value);
            sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, false, true, sResult);

            do
            {
                switch (nRow)
                {
                    case 0:
                        sRimType.Value = StringHelper.StrTrimX(sResult.Value);
                        break;
                    case 1:
                        sUserName.Value = StringHelper.StrTrimX(sResult.Value);
                        sPassword.Value = StringHelper.StrTrimX(sResult1.Value);
                        sApplication.Value = StringHelper.StrTrimX(sResult2.Value);
                        sUrl.Value = StringHelper.StrTrimX(sResult3.Value);
                        sId.Value = StringHelper.StrTrimX(sResult4.Value);
                        sPath.Value = StringHelper.StrTrimX(sResult5.Value);
                        break;
                    default:
                        sOptional.Value = sOptional.Value + "~" + StringHelper.StrTrimX(sResult.Value);
                        break;
                }
                nRow++;
            }
            while (nRow == 1 ? sqlHelper.ExecSqlGetNextRow(true, sResult, sResult1, sResult2, sResult3, sResult4, sResult5) : sqlHelper.ExecSqlGetNextRow(true, sResult));

            (_paramNodes[4] as PBaseType).ValueObject = sUserName.Value;
            (_paramNodes[5] as PBaseType).ValueObject = sPassword.Value;
            (_paramNodes[6] as PBaseType).ValueObject = sApplication.Value;
            (_paramNodes[7] as PBaseType).ValueObject = sUrl.Value;
            (_paramNodes[8] as PBaseType).ValueObject = sId.Value;
            (_paramNodes[9] as PBaseType).ValueObject = sPath.Value;
            (_paramNodes[10] as PBaseType).ValueObject = sOptional.Value;
            (_paramNodes[11] as PBaseType).ValueObject = sResult.Value; //WI#29051 
        }
        #endregion
    }
}
