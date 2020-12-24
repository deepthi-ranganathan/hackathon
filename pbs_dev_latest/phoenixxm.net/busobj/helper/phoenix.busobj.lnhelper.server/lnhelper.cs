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
// File Name: LnHelper.cs
// NameSpace: Phoenix.BusObj.Loan.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//4/08/2008		1		rpoddar		Created
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Text;
using System.Globalization;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Global.Server;
using Phoenix.Shared.Constants;


namespace Phoenix.BusObj.Loan.Server
{
	/// <summary>
	/// Summary description for LnHelper.
	/// </summary>
	public class LnHelper: Phoenix.BusObj.Loan.LnHelper
	{
		#region private vars     
		#endregion


		#region public constructors
		public LnHelper():base()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion


		#region InitializeMap
		protected override void InitializeMap()
		{
			base.InitializeMap();
			// Define Constraints

			// Define Event Handlers


		}
		#endregion

		#region events
		#endregion

        #region Helper functions

        #region Update Loan Status
        public void UpdateLoanStatusFromInCompleteToUnfunded(IDbHelper dbHelper,BusObjectBase busobj,
            string acctType, string acctNo, string ignoreCaseType, 
            ref bool updatedStatus)
        {
            
            string sql = null;
            string dbPrefix = CoreService.DbHelper.PhoenixDbName + "..";

            GbHelper gbHelper = new GbHelper();
            PString Status = new PString( "Status" );
            PString Hmda = new PString("Hmda");
            PString StmtReqd = new PString("StmtReqd" );
            PInt RimNo = new PInt("RimNo");
            PInt Ptid = new PInt("Ptid");

            bool workSheetExists = false;
            bool scheduleExists = false;
            bool hmdaExists = false;
            bool statementExist = false;
            bool udfExist = false;

            updatedStatus = false;

            sql = string.Format(@"select
                status,
                hmda,
                stmt_reqd,
                rim_no,
                ptid
            from {0}ln_acct
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo);
            gbHelper.ExecSqlImmediateInto(dbHelper, sql, Status, Hmda, StmtReqd, RimNo, Ptid);
            
            if (Status.Value != GlobalVars.Instance.ML.Incomplete)
                return;

            if (ignoreCaseType != null)
            {
                ignoreCaseType = ignoreCaseType.ToUpper();
                if (ignoreCaseType == "WORKSHEET")
                    workSheetExists = true;
                else if (ignoreCaseType == "SCHEDULE")
                    scheduleExists = true;
                else if (ignoreCaseType == "HMDA")
                    hmdaExists = true;
                else if (ignoreCaseType == "STATEMENT")
                    statementExist = true;

            }

            if (!workSheetExists)
            {
                sql = string.Format(@"select
                1
            from {0}LN_WORKSHEET
            where acct_type = '{1}'
            and acct_no = '{2}'
            and worksheet_code = 'S'", dbPrefix, acctType, acctNo);
                workSheetExists = gbHelper.ExecSqlExists(dbHelper, sql);
                    
            }

            if (!scheduleExists)
            {
                sql = string.Format(@"select
                1
            from {0}LN_PMT_SCHEDULE
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo);
                scheduleExists = gbHelper.ExecSqlExists(dbHelper, sql);

            }

            if (Hmda.Value == GlobalVars.Instance.ML.Y && !hmdaExists)
            {
                sql = string.Format(@"select
                1
            from {0}LN_HMDA
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo);
                hmdaExists = gbHelper.ExecSqlExists(dbHelper, sql);

            }

            if (StmtReqd.Value == GlobalVars.Instance.ML.Y && !statementExist)
            {
                sql = string.Format(@"select
                1
            From
	            {0}GB_COMB_STMT A, 
	            {0}GB_COMB_STMT_ACCT B,
	            {0}GB_RPT_STATEMENT R
            Where
	            A.RIM_NO = {3}
            And
	            A.STATUS = 'Active'
            And
	            B.STATUS = 'Active'
            And
	            A.COMB_STMT_ID = B.COMB_STMT_ID
            And
	            A.RPT_STMT_ID = R.RPT_STATEMENT_ID
            And
	            B.ACCT_TYPE = '{1}'
            And
	            B.ACCT_NO = '{2}'", dbPrefix, acctType, acctNo, RimNo.SqlString );
                statementExist = gbHelper.ExecSqlExists(dbHelper, sql);

            }

            if (!udfExist)
            {
                sql = string.Format(@"select
                1
            from {0}ln_user_def_val
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo);
                udfExist = gbHelper.ExecSqlExists(dbHelper, sql);

            }

            if (workSheetExists && scheduleExists && (hmdaExists || StmtReqd.Value != GlobalVars.Instance.ML.Y) &&
                (statementExist || StmtReqd.Value != GlobalVars.Instance.ML.Y) && !udfExist)
            {
                sql = string.Format(@"update {0}ln_display
            set status = '{3}',
                status_sort = 10
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo, GlobalVars.Instance.ML.Unfunded);

                dbHelper.ExecuteNonQuery(sql);

                gbHelper.CreateAuditEntry(dbHelper, busobj,
                    CoreService.Translation.GetTokenizeTranslateX(ScreenId.LnEdit, 11, new string[] { acctType, acctNo }),
                    ScreenId.LnEdit, Ptid.Value, "LN_DISPLAY", "STATUS", Ptid.Value, GlobalVars.Instance.ML.Unfunded, GlobalVars.Instance.ML.Unfunded,
                    GlobalVars.Instance.ML.Incomplete, GlobalVars.Instance.ML.Incomplete);

                sql = string.Format(@"update {0}ln_acct
            set status = '{3}',
                status_sort = 10
            where acct_type = '{1}'
            and acct_no = '{2}'", dbPrefix, acctType, acctNo, GlobalVars.Instance.ML.Unfunded);

                dbHelper.ExecuteNonQuery(sql);

                gbHelper.CreateAuditEntry(dbHelper, busobj,
                    CoreService.Translation.GetTokenizeTranslateX(ScreenId.LnEdit, 12, new string[] { acctType, acctNo }),
                    ScreenId.LnEdit, Ptid.Value, "LN_ACCT", "STATUS", Ptid.Value, GlobalVars.Instance.ML.Unfunded, GlobalVars.Instance.ML.Unfunded,
                    GlobalVars.Instance.ML.Incomplete, GlobalVars.Instance.ML.Incomplete);

                updatedStatus = true;

            }


        }
        #endregion
        #endregion


    }

}
