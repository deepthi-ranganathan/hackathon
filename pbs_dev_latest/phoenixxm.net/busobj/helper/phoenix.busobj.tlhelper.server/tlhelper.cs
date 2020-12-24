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
// File Name: TlCc.cs
// NameSpace: Phoenix.BusObj.Teller.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//9/27/2005		1		rpoddar		Created
//04/07/2006	2		mselvaga	Issue#66440 - Fixed DBPrefix issue for secondary db option.
//05/12/2006	3		vdevadoss	#66123 - Added the GetAcctRate helper method.
//05/16/2006	4		rpoddar		#66440 - Moved the sql functions to gbhelper
//05/24/2006	5		mselvaga	#67873 - added GetCTRTranCount helper.
//11-15-2006	6		mselvaga	#70724 - Added new deploan filter.
//02/07/07		7		VDevadoss	#70728 - Append Cmt Acct types in LocPopAcctTypes
//03/14/2007	8		mselvaga	#72101 - Identify correct database source for transaction select during copy over and copy back mode - 24/7.
//03-22-2007	9		rpoddar		#72171 - comment static
//05/07/2007	10		mselvaga	#72644 - Trancode fetch for commitments - replaced filter by tran_code instead of tl_tran_code.
//May-24-07		11   	Muthu       #72780 VS2005 Migration 
//06/08/2007    12      mselvaga    #72324 - 24/7 - user receives PE #300058 when highlighting a non-assigned drawer and selecting the Position action.
//08/16/2007    13      mselvaga    #72916 - Added helper for GetTcdDrawerNo and GetTcdMachineId.
//02/04/2008    14      JStainth    #73023 - Teller2007 - Receiving error when reversing trans in 24/7 mode
//04/28/2008    15      mselvaga    #75912 - Beta 2008 Account type selection in transaction window is not in alpha order.
//11/25/2008    16      mselvaga    #76458 - Added EX account changes.
//02/20/2009    17      mselvaga    #2677 - Fixed LocPopTlTranCode for typo error.
//05/11/2009    18      mselvaga    #76425 - Added New LOC Product changes.
//06/24/2009    19      mselvaga    WI#1157 - Loan prepay penalty part II changes added.
//03/31/2010    20      LSimpson    #8426 - Modifications for non cash where clause with external where logic.
//04/19/2010    21		rpoddar		#79510 - Shared Branch Changes
//05/21/2010    22      rpoddar     #09038 - Handled SharedBranchCustomOption
//05/05/2011    23      LSimpson    #12552 - Changed mainCol from undisbursed to actual_undisbursed within GetAvailBalDbCol.
//05/06/2011    23      sdhamija	#13768 - TC313 will not be supported in offline.
//02/04/2014    24      Vipin       #32322 - Added new custom action  CheckIsTLTCCredit for check a trancode is credit transaction
//06/30/2017    25      mselvaga    Task#196262 - #67143 - Enh. Commitment changes added.
//07/10/2017    26      mselvaga    Task#196262 - #65186 - Enh. Commitment changes added.
//07/27/2018    27      FOyebola    Task#196262 - Task# 95645
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Text;
using System.Globalization;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Global.Server;
using Phoenix.Shared.BusFrame;          //#06167


namespace Phoenix.BusObj.Teller.Server
{
	/// <summary>
	/// Summary description for TlCc.
	/// </summary>
	public class TlHelper: Phoenix.BusObj.Teller.TlHelper
	{
		#region private vars
		private GbHelper GbHelper = new GbHelper();			// #72171 - removed static
        private SqlHelper _sqlHelper = new SqlHelper();     // #06167
		#endregion


		#region public constructors
		public TlHelper():base()
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
			this.AcctType.BeforePopulateEvent+=new BusObjectEventHandler(AcctType_BeforePopulateEvent);
			this.TlTranCode.BeforePopulateEvent+=new BusObjectEventHandler(TlTranCode_BeforePopulateEvent);
			this.EmployeeId.BeforePopulateEvent+=new BusObjectEventHandler(EmployeeId_BeforePopulateEvent);
			this.TranCode.BeforePopulateEvent+=new BusObjectEventHandler(TranCode_BeforePopulateEvent);
			this.FormId.BeforePopulateEvent+=new BusObjectEventHandler(FormId_BeforePopulateEvent);



		}
		#endregion

		#region events
		private void AcctType_BeforePopulateEvent(object sender, BusActionEventArgs e)
		{
			this.AcctType.Constraint.SpecialSql = LocPopAcctTypes(e.DbHelper);
			e.IsContinue = true;
		}
		private void TlTranCode_BeforePopulateEvent(object sender, BusActionEventArgs e)
		{
			this.TlTranCode.Constraint.SpecialSql = LocPopTlTranCode(e.DbHelper);
			e.IsContinue = true;
		}

		private void EmployeeId_BeforePopulateEvent(object sender, BusActionEventArgs e)
		{
			this.EmployeeId.Constraint.SpecialSql = LocPopEmployee(e.DbHelper);
			e.IsContinue = true;
		}
		private void TranCode_BeforePopulateEvent(object sender, BusActionEventArgs e)
		{
			this.TranCode.Constraint.SpecialSql = LocPopTranCode(e.DbHelper);
			e.IsContinue = true;
		}

		private void FormId_BeforePopulateEvent(object sender, BusActionEventArgs e)
		{
			this.FormId.Constraint.SpecialSql = LocPopFormId(e.DbHelper);
			e.IsContinue = true;
		}
		#endregion

        #region ******** Custom Action ***********
        protected override bool OnActionCustom(IDbHelper dbHelper, bool isPrimaryDb)    //#76425
        {
            #region GetHistoryPtidFromXm
            if (this.CustomActionName == "GetHistoryPtidFromXm")
            {
                string acctNo = "";
                string acctType = "";
                int tranCode = 0;
                decimal parentPtid = 0;
                //
                if (_paramNodes.Count > 0)
                {
                    acctNo = (_paramNodes[0] as PBaseType).StringValue;
                    acctType = (_paramNodes[1] as PBaseType).StringValue;
                    tranCode = (_paramNodes[2] as PBaseType).IntValue;  //#76425
                    parentPtid = (_paramNodes[3] as PBaseType).DecimalValue;

                    GetHistoryPtidFromXm(dbHelper, acctNo, acctType, tranCode, ref parentPtid);
                    //
                    (_paramNodes[3] as PBaseType).Value = parentPtid; //load back the history ptid
                }
            }
            #endregion
            #region check credit transaction or not
            //Begin 32322
            if (this.CustomActionName == "CheckIsTLTCCredit")
            {
                CheckIsTLTCCredit(dbHelper);

            }
            //End 32322
            #endregion

            return true;
        }
        #endregion

		#region LocPopAcctTypes(IDbHelper dbHelper)
		private string LocPopAcctTypes(IDbHelper dbHelper )
		{
			string sql = "";
			string extraWhere = "";
            string extraWhere1 = "";
            string extraWhere2 = "";
            bool depositOnly = false;

            //#2677
            if (!this.FilterByDepLoan.IsNull && this.FilterByDepLoan.Value != string.Empty &&
                this.FilterByDepLoan.Value.Trim() == "'DP'")
                depositOnly = true;

			if (!this.FilterByDepLoan.IsNull && this.FilterByDepLoan.Value != string.Empty)
			{
				extraWhere = string.Format(@"AND A.DEP_LOAN IN ( {0} )", this.FilterByDepLoan.Value);
			}

            //Begin #76458
            if (this.FilterByDepLoan.IsNull || (!this.FilterByDepLoan.IsNull && this.FilterByDepLoan.Value.Contains("EX")))
            {
                if (this.FilterByDepLoan.IsNull)
                    extraWhere2 = string.Format(@"AND     (A.CR_TC_SUPPORT = '{0}'
                        OR      A.DR_TC_SUPPORT = '{0}')", GlobalVars.Instance.ML.Y);
                else if (!this.FilterByDepLoan.IsNull && this.FilterByDepLoan.Value.Contains("EXCR"))
                    extraWhere2 = string.Format(@"AND     ((A.CR_TC_SUPPORT = '{0}'
                        AND      A.DR_TC_SUPPORT = '{0}') 
                        OR (A.CR_TC_SUPPORT = '{0}'
                        AND      A.DR_TC_SUPPORT != '{0}'))", GlobalVars.Instance.ML.Y);
                else if (!this.FilterByDepLoan.IsNull && this.FilterByDepLoan.Value.Contains("EXDR"))
                    extraWhere2 = string.Format(@"AND     ((A.CR_TC_SUPPORT = '{0}'
                        AND      A.DR_TC_SUPPORT = '{0}')
                        OR (A.CR_TC_SUPPORT != '{0}'
                        AND      A.DR_TC_SUPPORT = '{0}'))", GlobalVars.Instance.ML.Y);
                
                extraWhere1 = string.Format(@"UNION 
                
                SELECT	rtrim({2}),
								'EX'+'~'+'EX'+'~'+A.ACCT_NO_FORMAT
						FROM	{0}AD_EX_ACCT_TYPE A
						WHERE	A.STATUS = '{1}'
                        AND     A.ADAPTER = '{3}'
                        {4}", GbHelper.InquireDbPrefix(dbHelper),
                                                 GlobalVars.Instance.ML.Active,
                    (TlWhereClause.Value == "Y" ? "A.ACCT_TYPE" : "A.ACCT_TYPE"),
                    GlobalVars.Instance.ML.Y,
                    extraWhere2);
            }
            //End #76458


			if (this.SkipSafeDeposit.IsNull || this.SkipSafeDeposit.Value == 0)
			{
				//#75912 - added order by clause, do not depend on database primary key index, let us force order by
                if (depositOnly)
                {
                    sql = string.Format(@"
						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
						{3}

                        ORDER BY 1", GbHelper.InquireDbPrefix(dbHelper), GlobalVars.Instance.ML.Active,
                        (TlWhereClause.Value == "Y" ? "A.ACCT_TYPE" : "B.APPL_TYPE"),
                        extraWhere);   //#76458 - added extraWhere1
                }
                else
                {
                    sql = string.Format(@"
						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
						{4}

						UNION

						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+ B.DEP_LOAN +'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_CM_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'

						UNION

						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+'{3}'+'~'+rtrim(B.ACCT_NO_FORMAT)
						FROM	{0}AD_SD_ACCT_TYPE A,
								{0}AD_SD_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
                        
                        {5}

                        ORDER BY 1", GbHelper.InquireDbPrefix(dbHelper), GlobalVars.Instance.ML.Active,
                        (TlWhereClause.Value == "Y" ? "A.ACCT_TYPE" : "B.APPL_TYPE"),
                        GlobalVars.Instance.ML.SD, extraWhere, extraWhere1);   //#76458 - added extraWhere1
                }
			}
			else
			{
                //#75912 - added order by clause, do not depend on database primary key index, let us force order by
                if (depositOnly)
                {
                    sql = string.Format(@"
						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
                        {3}

                        ORDER BY 1", GbHelper.InquireDbPrefix(dbHelper), GlobalVars.Instance.ML.Active,
                        (TlWhereClause.Value == "Y" ? "A.ACCT_TYPE" : "B.APPL_TYPE"),
                        extraWhere);
                }
                else
                {
                    sql = string.Format(@"
						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
                        {3}

						UNION

						SELECT	rtrim({2}),
								rtrim(B.APPL_TYPE)+'~'+ B.DEP_LOAN +'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_CM_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
                        
                        {4}

                        ORDER BY 1", GbHelper.InquireDbPrefix(dbHelper), GlobalVars.Instance.ML.Active,
                        (TlWhereClause.Value == "Y" ? "A.ACCT_TYPE" : "B.APPL_TYPE"),
                        extraWhere, extraWhere1); //#76458 - added extraWhere & extraWhere1
                }
			}

			return sql;
		}
		#endregion

		#region LocPopTlTranCode(IDbHelper dbHelper)
		private string LocPopTlTranCode(IDbHelper dbHelper )
		{
			string sql = "";
            string tranCodeList = "";
            string nonCashWhere = ""; //#8426
            string shBranchWhere = SharedBranchCustomOption.Value ? string.Format(" AND TL_TRAN_CODE NOT IN ( SELECT TL_TRAN_CODE FROM {0}PC_SH_BRANCH_TL_TC )", GbHelper.InquireDbPrefix(dbHelper)) : null;    // #09038
            string offlineWhere = (dbHelper.IsOfflineDb) ? string.Format(" and tran_code not in (313) ") : "";	//#13768 - TC313 not available in offline mode. 
            string excludeQTCs = ""; //#95645

            /*Begin Enh# 70728*/
            if (TlWhereClause.Value == "Commitment" || TlWhereClause.Value == "Commitment and batch_id = NULL" || //#8426
                TlWhereClause.Value == "Commitment and substring(tl_tran_code, 1, 1) != 'Q'" ||                   //#95645
                TlWhereClause.Value == "Commitment and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'") //#95645
            {
                if (TlWhereClause.Value == "Commitment and batch_id = NULL") //#8426
                    nonCashWhere = " and batch_id = NULL";
                //Begin #95645
                else if (TlWhereClause.Value == "Commitment and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "Commitment and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    nonCashWhere = " and batch_id = NULL";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                //End #95645

                //Begin: #72644
                //#1157
                //#79510 - Added PC_SH_BRANCH_TL_TC where clause
                sql = string.Format(@"
							Select	TL_TRAN_CODE,
									DESCRIPTION
							From	{0}AD_TL_TC
							WHERE	TRAN_CODE IN (300, 301, 304, 308,345)
							{3}
                            {2}
                            {4}
                           	ORDER BY TL_TRAN_CODE", GbHelper.InquireDbPrefix( dbHelper ), GlobalVars.Instance.ML.Active, nonCashWhere,
                                                  shBranchWhere, excludeQTCs); //95645		// ML   // #09038 #67143 - added 301, #65186 - added 304
				//End: #72644
			}	/*End Enh# 70728*/
            else if (TlWhereClause.Value == "ExternalCR" || TlWhereClause.Value == "ExternalDR" ||
                TlWhereClause.Value == "ExternalCRDR" || TlWhereClause.Value == "ExternalCR and batch_id = NULL" ||
                TlWhereClause.Value == "ExternalDR and batch_id = NULL" ||
                TlWhereClause.Value == "ExternalCRDR and batch_id = NULL" ||   //#76458 //#8426
                //Begin 95645
                TlWhereClause.Value == "ExternalCR and substring(tl_tran_code, 1, 1) != 'Q'" || 
                TlWhereClause.Value == "ExternalDR and substring(tl_tran_code, 1, 1) != 'Q'" ||
                TlWhereClause.Value == "ExternalCRDR and substring(tl_tran_code, 1, 1) != 'Q'" || 
                TlWhereClause.Value == "ExternalCR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'" ||
                TlWhereClause.Value == "ExternalDR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'" ||
                TlWhereClause.Value == "ExternalCRDR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'")
                //End 95645
            {
                if (TlWhereClause.Value == "ExternalCR")
                    tranCodeList = "520";
                else if (TlWhereClause.Value == "ExternalDR")   //#2677 - Changed from CR to DR
                    tranCodeList = "570";
                else if (TlWhereClause.Value == "ExternalCRDR")
                    tranCodeList = "520,570";
                else if (TlWhereClause.Value == "ExternalCR and batch_id = NULL") //#8426
                {
                    tranCodeList = "520";
                    nonCashWhere = " and batch_id = NULL";
                }
                else if (TlWhereClause.Value == "ExternalDR and batch_id = NULL") //#8426
                {
                    tranCodeList = "570";
                    nonCashWhere = " and batch_id = NULL";
                }
                else if (TlWhereClause.Value == "ExternalCRDR and batch_id = NULL") //#8426
                {
                    tranCodeList = "520,570";
                    nonCashWhere = " and batch_id = NULL";
                }
                //Begin 95645
                else if (TlWhereClause.Value == "ExternalCR and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "520";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "ExternalDR and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "570";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "ExternalCRDR and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "520,570";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "ExternalCR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "520";
                    nonCashWhere = " and batch_id = NULL";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "ExternalDR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "570";
                    nonCashWhere = " and batch_id = NULL";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                else if (TlWhereClause.Value == "ExternalCRDR and batch_id = NULL and substring(tl_tran_code, 1, 1) != 'Q'")
                {
                    tranCodeList = "520,570";
                    nonCashWhere = " and batch_id = NULL";
                    excludeQTCs = " and substring(tl_tran_code, 1, 1) != 'Q'";
                }
                //End 95645

                //#79510 - Added PC_SH_BRANCH_TL_TC where clause
                sql = string.Format(@"
							Select	TL_TRAN_CODE,
									DESCRIPTION
							From	{0}AD_TL_TC
							WHERE	TRAN_CODE IN ({1})
							{4}
                            {3}
                            {5}
                            And STATUS = '{2}'
							ORDER BY TL_TRAN_CODE", GbHelper.InquireDbPrefix(dbHelper), tranCodeList, GlobalVars.Instance.ML.Active, nonCashWhere,
                                                  shBranchWhere, excludeQTCs);	//95645	 // ML   // #09038
            }
			else
			{
                //#79510 - Added PC_SH_BRANCH_TL_TC where clause
				TlWhereClause.Value += shBranchWhere;	// #09038
				TlWhereClause.Value += offlineWhere;	// #13768
                sql = string.Format(@"
							Select	TL_TRAN_CODE,
									DESCRIPTION
							From	{0}AD_TL_TC
							WHERE	" + TlWhereClause.Value + @"
							ORDER BY TL_TRAN_CODE", GbHelper.InquireDbPrefix( dbHelper ), GlobalVars.Instance.ML.Active);		// ML
			}

			return sql;
		}
		#endregion

		#region LocPopEmployee(IDbHelper dbHelper)
		private string LocPopEmployee(IDbHelper dbHelper )
		{
			string sql = "";

			sql = string.Format(@"
						Select	EMPLOYEE_ID,
								NAME
						From	{0}AD_GB_RSM
						{1}", GbHelper.InquireDbPrefix( dbHelper ),
				(TlWhereClause.IsNull || TlWhereClause.Value == "-1"? "" : TlWhereClause.Value));

			return sql;
		}
		#endregion

		#region LocPopTranCode(IDbHelper dbHelper)
		private string LocPopTranCode(IDbHelper dbHelper )
		{
			string sql = "";

			sql = string.Format(@"
						Select	TRAN_CODE,
								DESCRIPTION
						From	{0}AD_GB_TC
						WHERE 	{1}
						ORDER BY TRAN_CODE", GbHelper.InquireDbPrefix( dbHelper ), TlWhereClause.Value);

			return sql;
		}
		#endregion

		#region LocPopFormId(IDbHelper dbHelper)
		private string LocPopFormId(IDbHelper dbHelper )
		{
			string sql = "";

			sql = string.Format(@"
						Select	FORM_ID,
								DESCRIPTION
						From	{0}AD_TL_FORM
						WHERE	SERVICE_TYPE = {1}
						AND		STATUS = '{2}'
						ORDER BY 2", GbHelper.InquireDbPrefix( dbHelper ), ( TlWhereClause.IsNull ? "SERVICE_TYPE" : "'" + TlWhereClause.Value + "'" ),
						GlobalVars.Instance.ML.Active );

			return sql;
		}
		#endregion


        #region helper methods

        //Begin #06167
        #region commented code
        //#region use db functions
        //public DbToUse DbToUpdate( IDbHelper dbHelper )
        //{
        //    return GbHelper.DbToUpdate( dbHelper );
        //}

        //public DbToUse DbToInquire( IDbHelper dbHelper )
        //{
        //    return GbHelper.DbToInquire( dbHelper );
        //}

        //public DbToUse DbToInsert( IDbHelper dbHelper )
        //{
        //    return GbHelper.DbToInsert( dbHelper );
        //}
        //#endregion

        //#region database table prefixes
        //public string[] UpdateDbPrefix( IDbHelper dbHelper )
        //{
        //    return GbHelper.UpdateDbPrefix( dbHelper );
        //}

        //public string InquireDbPrefix( IDbHelper dbHelper )
        //{
        //    return GbHelper.InquireDbPrefix( dbHelper );
        //}

        //public string InsertDbPrefix( IDbHelper dbHelper )
        //{
        //    return GbHelper.InsertDbPrefix( dbHelper );
        //}
        //#endregion

        //#region sql functions
        //public bool ExecSqlImmediate( IDbHelper dbHelper, string sql, ref object[] result )
        //{
        //    return GbHelper.ExecSqlImmediate( dbHelper, sql, ref result );
        //}

        //public bool ExecSqlImmediate( IDbHelper dbHelper, string sql, ref object[] result, bool forcePrimeDb )
        //{
        //    return GbHelper.ExecSqlImmediate( dbHelper, sql, ref result, forcePrimeDb );
        //}

        //public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, params IPhoenixDataType[] intoVars )
        //{
        //    return GbHelper.ExecSqlImmediateInto( dbHelper, sql, intoVars );
        //}

        //public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, bool forcePrimeDb, params IPhoenixDataType[] intoVars )
        //{
        //    return GbHelper.ExecSqlImmediateInto( dbHelper, sql, forcePrimeDb, intoVars );
        //}

        //public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars )
        //{
        //    return GbHelper.ExecSqlImmediateInto( dbHelper, sql, forcePrimeDb, multipleRows, intoVars );
        //}

        //public bool ExecSqlImmediateInto( IDbHelper dbHelper, IDbConnection dbConnection, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars )
        //{
        //    return GbHelper.ExecSqlImmediateInto( dbHelper, dbConnection, sql, forcePrimeDb, multipleRows, intoVars );
        //}

        //public bool ExecSqlGetNextResultSet( )
        //{
        //    return GbHelper.ExecSqlGetNextResultSet( );
        //}

        //public bool ExecSqlGetNextRow( bool moveToNextResultSet, params IPhoenixDataType[] intoVars )
        //{
        //    return GbHelper.ExecSqlGetNextRow( moveToNextResultSet, intoVars );
        //}

        //public bool ExecSqlGetNextRow( )
        //{
        //    return GbHelper.ExecSqlGetNextRow( );
        //}

        //public bool ExecSqlExists( IDbHelper dbHelper, string sql )
        //{
        //    return GbHelper.ExecSqlExists( dbHelper, sql );
        //}
        //#endregion

        //public bool ExecSqlUpdate( IDbHelper dbHelper, string sql )
        //{
        //    int phxRowsUpdated = 0;
        //    int xpRowsUpdated = 0;

        //    ExecSqlUpdate( dbHelper, sql, ref phxRowsUpdated, ref xpRowsUpdated );
        //    if ( phxRowsUpdated == 0 && xpRowsUpdated == 0 )
        //        return false;
        //    return true;
        //}

        //public void ExecSqlUpdate( IDbHelper dbHelper, string sql, ref int phxRowsUpdated, ref int xpRowsUpdated )
        //{
        //    string copyBackStatus = "";
        //    string sqlTemp = null;
        //    foreach( string tblPfx in GbHelper.UpdateDbPrefix( dbHelper ))
        //    {
        //        if ( tblPfx != "" )
        //        {

        //            if ( tblPfx == "X_" )
        //            {
        //                if ( GbHelper.DbToUpdate( dbHelper ) != DbToUse.SecondaryOnly  )
        //                    copyBackStatus = " copy_back_status = isnull(copy_back_status, 0) | ((isnull(copy_back_status, 0) & 1 ) * 2 ) , " + "\n" ;
        //                else
        //                    copyBackStatus = " copy_back_status = isnull(copy_back_status, 0) | 2 , " + "\n" ;
        //                sqlTemp = string.Format(sql, tblPfx, copyBackStatus );
        //                xpRowsUpdated = dbHelper.ExecuteNonQuery( sqlTemp );
        //            }
        //            else
        //            {
        //                copyBackStatus = "";
        //                sqlTemp = string.Format(sql, tblPfx, copyBackStatus );
        //                phxRowsUpdated = dbHelper.ExecuteNonQuery( sqlTemp );
        //            }

        //        }
        //    }
        //    return;
        //}

        //public bool ExecSqlInsert( IDbHelper dbHelper, string sql )
        //{
        //    string tblPfx = GbHelper.InsertDbPrefix( dbHelper ) ;
        //    if ( tblPfx == "X_" )
        //    {
        //        sql = string.Format(sql, tblPfx, " copy_back_status, ", " 1, " );
        //        if ( dbHelper.ExecuteNonQuery( sql ) == 0 )
        //            return false;
        //    }
        //    else
        //    {
        //        sql = string.Format(sql, tblPfx, "", "" );
        //        if ( dbHelper.ExecuteNonQuery( sql ) == 0 )
        //            return false;
        //    }
        //    return true;
        //}

        //public void CloseReader()
        //{
        //    GbHelper.CloseReader();
        //    return;
        //}

        //private bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj, short dbType )

        //{
        //    DbToUse dbToUpdate;
        //    if ( dbType == -1)
        //        dbToUpdate = GbHelper.DbToUpdate( dbHelper );
        //    else
        //        dbToUpdate = (DbToUse) dbType;

        //    busObj.ActionType = XmActionType.New;
        //    if ( busObj.DoAction( dbHelper ) != RecordStatus.Success )
        //        return false;
        //    //busObj.OnActionInsert( dbHelper, "D" );
        //    if (( dbToUpdate & DbToUse.PrimaryOnly)  == DbToUse.PrimaryOnly  || ( dbToUpdate & DbToUse.Offline)  == DbToUse.Offline )
        //    {
        //        //if ( !busObj.OnActionInsert( dbHelper, true ))
        //        //	return false;
        //    }
        //    else if (( dbToUpdate & DbToUse.SecondaryOnly )  == DbToUse.SecondaryOnly   )
        //    {
        //        //if (!busObj.OnActionInsert( dbHelper, false ))
        //        //	return false;
        //    }
        //    return true;

        //}
        //public bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj )
        //{
        //    return ExecObjInsert( dbHelper, busObj, -1 );
        //}
        //public bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj, DbToUse useDb )
        //{
        //    return ExecObjInsert( dbHelper, busObj, (short) useDb );
        //}

        #endregion

        #region use db functions
        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public DbToUse DbToUpdate(IDbHelper dbHelper)
        {
            return _sqlHelper.DbToUpdate(dbHelper);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public DbToUse DbToInquire(IDbHelper dbHelper)
        {
            return _sqlHelper.DbToInquire(dbHelper);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public DbToUse DbToInsert(IDbHelper dbHelper)
        {
            return _sqlHelper.DbToInsert(dbHelper);
        }
        #endregion

        #region database table prefixes
        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public string[] UpdateDbPrefix(IDbHelper dbHelper)
        {
            return _sqlHelper.UpdateDbPrefix(dbHelper);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public string InquireDbPrefix(IDbHelper dbHelper)
        {
            return _sqlHelper.InquireDbPrefix(dbHelper);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <returns></returns>
        public string InsertDbPrefix(IDbHelper dbHelper)
        {
            return _sqlHelper.InsertDbPrefix(dbHelper);
        }
        #endregion

        #region sql functions
        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool ExecSqlImmediate(IDbHelper dbHelper, string sql, ref object[] result)
        {
            return _sqlHelper.ExecSqlImmediate(dbHelper, sql, ref result);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="result"></param>
        /// <param name="forcePrimeDb"></param>
        /// <returns></returns>
        public bool ExecSqlImmediate(IDbHelper dbHelper, string sql, ref object[] result, bool forcePrimeDb)
        {
            return _sqlHelper.ExecSqlImmediate(dbHelper, sql, ref result, forcePrimeDb);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="intoVars"></param>
        /// <returns></returns>
        public bool ExecSqlImmediateInto(IDbHelper dbHelper, string sql, params IPhoenixDataType[] intoVars)
        {
            return _sqlHelper.ExecSqlImmediateInto(dbHelper, sql, intoVars);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="forcePrimeDb"></param>
        /// <param name="intoVars"></param>
        /// <returns></returns>
        public bool ExecSqlImmediateInto(IDbHelper dbHelper, string sql, bool forcePrimeDb, params IPhoenixDataType[] intoVars)
        {
            return _sqlHelper.ExecSqlImmediateInto(dbHelper, sql, forcePrimeDb, intoVars);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="forcePrimeDb"></param>
        /// <param name="multipleRows"></param>
        /// <param name="intoVars"></param>
        /// <returns></returns>
        public bool ExecSqlImmediateInto(IDbHelper dbHelper, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars)
        {
            return _sqlHelper.ExecSqlImmediateInto(dbHelper, sql, forcePrimeDb, multipleRows, intoVars);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="dbConnection"></param>
        /// <param name="sql"></param>
        /// <param name="forcePrimeDb"></param>
        /// <param name="multipleRows"></param>
        /// <param name="intoVars"></param>
        /// <returns></returns>
        public bool ExecSqlImmediateInto(IDbHelper dbHelper, IDbConnection dbConnection, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars)
        {
            return _sqlHelper.ExecSqlImmediateInto(dbHelper, dbConnection, sql, forcePrimeDb, multipleRows, intoVars);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <returns></returns>
        public bool ExecSqlGetNextResultSet()
        {
            return _sqlHelper.ExecSqlGetNextResultSet();
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="moveToNextResultSet"></param>
        /// <param name="intoVars"></param>
        /// <returns></returns>
        public bool ExecSqlGetNextRow(bool moveToNextResultSet, params IPhoenixDataType[] intoVars)
        {
            return _sqlHelper.ExecSqlGetNextRow(moveToNextResultSet, intoVars);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <returns></returns>
        public bool ExecSqlGetNextRow()
        {
            return _sqlHelper.ExecSqlGetNextRow();
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSqlExists(IDbHelper dbHelper, string sql)
        {
            return _sqlHelper.ExecSqlExists(dbHelper, sql);
        }
        #endregion

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSqlUpdate(IDbHelper dbHelper, string sql)
        {
            return _sqlHelper.ExecSqlUpdate(dbHelper, sql);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <param name="phxRowsUpdated"></param>
        /// <param name="xpRowsUpdated"></param>
        public void ExecSqlUpdate(IDbHelper dbHelper, string sql, ref int phxRowsUpdated, ref int xpRowsUpdated)
        {
             _sqlHelper.ExecSqlUpdate(dbHelper, sql, ref phxRowsUpdated, ref xpRowsUpdated);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool ExecSqlInsert(IDbHelper dbHelper, string sql)
        {
            return _sqlHelper.ExecSqlInsert(dbHelper, sql);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        public void CloseReader()
        {
            _sqlHelper.CloseReader();
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="busObj"></param>
        /// <returns></returns>
        public bool ExecObjInsert(IDbHelper dbHelper, BusObjectBase busObj)
        {
            return _sqlHelper.ExecObjInsert(dbHelper, busObj);
        }

        /// <summary>
        /// Do not use this method. Use the method of Phoenix.Shared.BusFrame.SqlHelper class instead.
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="busObj"></param>
        /// <param name="useDb"></param>
        /// <returns></returns>
        public bool ExecObjInsert(IDbHelper dbHelper, BusObjectBase busObj, DbToUse useDb)
        {
            return _sqlHelper.ExecObjInsert(dbHelper, busObj, useDb);
        }
        //End #06167

		#region other helper functions

		#region Get Sequence#
		// #73023 Changed return from short to int
        public short GetSequenceNo(IDbHelper dbHelper, short branchNo, short drawerNo)
        //public int GetSequenceNo( IDbHelper dbHelper, short branchNo, short drawerNo )		
		{
			string sql;
			object[] result = null;

			sql = string.Format(@"
			update	{0}tl_drawer
			set	{1}sequence_no = sequence_no + 1
			where	branch_no = {2}
			and	drawer_no = {3}", "{0}", "{1}", branchNo, drawerNo );

						if ( !ExecSqlUpdate( dbHelper, sql ))
							return -1;

						sql = string.Format(@"
			select sequence_no
			from {0}tl_drawer
			where	branch_no = {1}
			and	drawer_no = {2}", "{0}", branchNo, drawerNo );

			if ( !GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
				return -1;

            // Begin #73023
            return Convert.ToInt16( result[0]);
            //return Convert.ToInt32(result[0]);
            // End #73023
		}
		#endregion

        #region Get Tcd DrawerNo
        public short GetTcdDrawerNo(IDbHelper dbHelper, short tcdMachineId)
        {
            string sql;
            object[] result = null;
            string fixColumn = "";
            string fixWhere = "";

            if (tcdMachineId >= 0)
            {
                fixColumn = "drawer_no";
                fixWhere = string.Format(@"tcd_device_no = {0}", tcdMachineId);
            }
            else
            {
                fixColumn = "min(drawer_no)";
                fixWhere = string.Format(@"drawer_type = '{0}'", "TCD");
            }

            sql = string.Format(@"
			select {1}
			from {0}tl_drawer
			where	{2}", "{0}", fixColumn, fixWhere);

            if (!GbHelper.ExecSqlImmediate(dbHelper, sql, ref result))
                return -1;
            if (result != null)
                return Convert.ToInt16(result[0]);
            else
                return -1;
        }
        #endregion

        #region Get Tcd Machine ID
        public short GetTcdMachineId(IDbHelper dbHelper, short branchNo, short tcdDrawerNo)
        {
            string sql;
            object[] result = null;
            string fixColumn = "";
            string fixWhere = "";

            fixColumn = "tcd_device_no";
            if (branchNo > 0 && tcdDrawerNo > 0)
                fixWhere = string.Format(@"branch_no = {0}
                                            and drawer_no = {1}", branchNo, tcdDrawerNo);
            else
            {
                fixWhere = string.Format(@"drawer_type = '{0}'
                                        and drawer_no = {1}", "TCD", GetTcdDrawerNo(dbHelper, -1));
            }

            sql = string.Format(@"
			select {1}
			from {0}tl_drawer
			where	{2}", "{0}", fixColumn, fixWhere);

            if (!GbHelper.ExecSqlImmediate(dbHelper, sql, ref result))
                return -1;
            if (result != null)
                return Convert.ToInt16(result[0]);
            else
                return -1;
        }
        #endregion

		#region Get GL Posting Prefix
		public bool GetGlPostingPrefix(IDbHelper dbHelper, short branchNo, ref string postingPrefix)
		{
			string sql = null;
			object[] result = null;

			if(branchNo <= 0)
				return false;
			sql = string.Format(@"
			SELECT	gl_posting_prefix
			FROM	{0}AD_GB_BRANCH
			WHERE	BRANCH_NO = {1}","{0}",branchNo);

			if(!GbHelper.ExecSqlImmediate(dbHelper,sql,ref result))
				return false;
			postingPrefix = Convert.ToString(result[0]);
			return true;
		}
		#endregion

		public bool ResolveGLAccount(IDbHelper dbHelper,short branchNo, ref string acctNo)
		{
			string postingPrefix = null;
			if(branchNo <= 0)
				return false;
			else
			{
				if(GetGlPostingPrefix(dbHelper,branchNo, ref postingPrefix))
				{
					if(!ResolveGLAccount(postingPrefix,ref acctNo))
						return false;
				}
				else
					return false;
			}
			return true;
		}

		public short GetPostBalDef( IDbHelper dbHelper, string applType, string postingMethod )
		{
			return GetBalDef( dbHelper, applType, postingMethod, "post" );
		}

		public short GetAvailBalDef( IDbHelper dbHelper, string applType, string postingMethod )
		{
			return GetBalDef( dbHelper, applType, postingMethod, "avail" );
		}

		public string GetAvailBalDbColumn( IDbHelper dbHelper, string applType, string postingMethod, string tblPrefix )
		{
			short baldef = GetAvailBalDef( dbHelper, applType, postingMethod );
			string mainCol = null;
			string floatCol = null;
			if ( tblPrefix == null || tblPrefix.Trim() == String.Empty )
				tblPrefix = String.Empty;
			else
				tblPrefix = tblPrefix + ".";

			if ( baldef < 0 )
				baldef = 0;

			if ( applType == "CD" || applType == "CK" || applType == "SV" )
			{
				mainCol = "CUR_BAL";
				floatCol = " - " + tblPrefix + "MEMO_FLOAT";
			}
			else
			{
				//mainCol = "UNDISBURSED";
                mainCol = "ACTUAL_UNDISB"; // #12552
				floatCol = String.Empty;
			}

			if ( baldef == 0 )
				return string.Format("{0}CUR_BAL", tblPrefix );
			else if ( baldef == 1 )
				return string.Format("{0}{1}", tblPrefix, mainCol );
			else if ( baldef == 2 )
				return string.Format("{0}{1} - {0}FLOAT_BAL_1", tblPrefix, mainCol );
			else if ( baldef == 3 )
				return string.Format("{0}{1} - {0}FLOAT_BAL_2", tblPrefix, mainCol );
			else if ( baldef == 6 )
				return string.Format("{0}{1} + {0}MEMO_CR - {0}MEMO_DR  {2}", tblPrefix, mainCol, floatCol );
			else if ( baldef == 8 )
				return string.Format("{0}{1} - {0}FLOAT_BAL_1 + {0}MEMO_CR - {0}MEMO_DR  {2}", tblPrefix, mainCol, floatCol );
			else if ( baldef == 9 )
				return string.Format("{0}{1} - {0}FLOAT_BAL_2 + {0}MEMO_CR - {0}MEMO_DR  {2}", tblPrefix, mainCol, floatCol );

			return null;
		}

		private short GetBalDef( IDbHelper dbHelper, string applType, string postingMethod, string balType )
		{
			int svcId = Phoenix.FrameWork.BusFrame.BusGlobalVars.ServiceId;
			string colName = "";
			string sql = "";
			object[] result = null;

			DebugInfo("TlHelper" + "GetPostBalDef:", "Entering..", false );

			if (!( applType == "CD" || applType == "CK" || applType == "SV" ))
				applType = "LN";
			colName = balType + "_bal_" + applType + "_" + ( postingMethod == "R" ? "real" : "memo" );



			#region sql
			sql = string.Format(@"
			select {1}
			from {0}ad_xp_svcs
			where service_id = {2}", "{0}", colName, svcId );
			#endregion

			if (!GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
				return -1;

			#region sql
			sql = string.Format(@"
			declare @nBalDef tinyint
			exec psp_x_map_bal_def
				'{0}',
				'{1}',
				0,
				@nBalDef OUTPUT,
				NULL
			select @nBalDef ", ( applType == "LN" ? "LN" : "DP" ), Convert.ToString( result[0] ) );
			#endregion

			result = null;

			if (!GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
				return -1;

			DebugInfo("TlHelper" + "GetPostBalDef:", "Leaving..", false );

			return Convert.ToInt16( result[0]);
		}


		public int GetTranSetId(IDbHelper dbHelper)
		{
			return GbHelper.GetTranSetId( dbHelper );
		}


		public bool GetJournalSource( IDbHelper dbHelper, short branchNo, short drawerNo,
			short seqNo, short subSeq, DateTime effectiveDt, ref string recSource, ref decimal jrnlPtid )
		{
			string sql = null;
			object[] result = null;
			string extraWhere = "";
			//#72101
			if (dbHelper.IsOfflineDb)
			{
				recSource = "P";
				return true;
			}
			//
			DbToUse sourceDb = GbHelper.DbToInquire( dbHelper );
			//#72101
			if (jrnlPtid > 0 && jrnlPtid != DbDecimal.Null &&
				jrnlPtid != DbDecimal.MinValue && jrnlPtid != DbDecimal.MaxValue)
			{
				extraWhere = string.Format(@" and ptid = {0}", jrnlPtid);
			}
			else
			{
				extraWhere = string.Format(@" and sequence_no = {0}
												{1}", seqNo, (subSeq >= 0 ? "and sub_sequence = " + subSeq.ToString() : ""));
			}

			#region old code
//			sql = string.Format(@"
//			select ptid
//			from {0}tl_journal
//			where	branch_no = {1}
//			and	drawer_no = {2}
//			and sequence_no = {3}
//			{4}
//			and effective_dt = '{5}'
//			{6}", "{0}", branchNo, drawerNo, seqNo,
//				(subSeq >= 0 ? "and sub_sequence = " + subSeq.ToString() : ""), effectiveDt, extraWhere);
			#endregion
			//
			sql = string.Format(@"
			select ptid
			from {0}tl_journal
			where	branch_no = {1}
			and	drawer_no = {2}
			and effective_dt = '{3}'
			{4}", "{0}", branchNo, drawerNo, effectiveDt, extraWhere);

			if ( !GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
			{
				if ( dbHelper.CopyStatus != "D" && dbHelper.IsPrimaryDbAvailable )
				{
					if ( sourceDb == DbToUse.SecondaryOnly  )
					{
						sql = string.Format( sql, dbHelper.PhoenixDbName + ".." );
						sourceDb = DbToUse.PrimaryOnly;
					}
					else
					{
						sql = string.Format( sql, "X_" );
						sourceDb = DbToUse.SecondaryOnly ;
					}
					if ( !GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
					{
						return false;
					}
				}
				else
					return false;
			}

			if ( subSeq >= 0 && result[0] != null )
                jrnlPtid = Convert.ToInt32(result[0]);      //#73023
                //jrnlPtid = Convert.ToInt16(result[0]);    //#73023

			if ( sourceDb == DbToUse.PrimaryOnly || sourceDb == DbToUse.Offline )
				recSource = "P";
			else
				recSource = "X";

			return true;
		}


		public bool GetJournalSource( IDbHelper dbHelper, short branchNo, short drawerNo,
			short seqNo, DateTime effectiveDt, ref string recSource )
		{
			decimal jrnlPtid = -1;
			//#72101
			if (dbHelper.IsOfflineDb)
			{
				recSource = "P";
				return true;
			}
			return GetJournalSource( dbHelper, branchNo, drawerNo,
			seqNo, -1, effectiveDt, ref recSource, ref jrnlPtid );
		}

        //Begin #76425
        public bool GetHistoryPtidFromXm(IDbHelper dbHelper, string acctNo, string acctType,
            int tranCode, ref decimal historyPtid)
        {
            string sql = null;
            object[] result = null;
            PString AcctNo = new PString("AcctNo");
            PString AcctType = new PString("AcctType");
            PInt TranCode = new PInt("TranCode");
            PDecimal ParentPtid = new PDecimal("ParentPtid");
            //
            AcctNo.Value = acctNo;
            AcctType.Value = acctType;
            ParentPtid.Value = historyPtid;
            TranCode.Value = tranCode;
            //
            DbToUse sourceDb = GbHelper.DbToInquire(dbHelper);
            //
            sql = string.Format(@"
			select history_ptid
			from {0}xp_tran_log
			where	tran_acct_no = {1}
            and tran_acct_type = {2}
            and parent_ptid = {3}
            and xapi_tc = {4}", "{0}", AcctNo.SqlString, AcctType.SqlString,
                              ParentPtid.SqlString, TranCode.SqlString);

            if (!GbHelper.ExecSqlImmediate(dbHelper, sql, ref result))
            {
                if (dbHelper.CopyStatus != "D" && dbHelper.IsPrimaryDbAvailable)
                {
                    if (sourceDb == DbToUse.SecondaryOnly)
                        sql = string.Format(sql, dbHelper.PhoenixDbName + "..");
                    else
                        sql = string.Format(sql, "X_");
                    //
                    if (!GbHelper.ExecSqlImmediate(dbHelper, sql, ref result))
                        return false;
                }
                else
                    return false;
            }
            if (result[0] != null)
                historyPtid = Convert.ToDecimal(result[0]);

            return true;
        }
        //End #76425

		//#72101 - Get position record source
		public bool GetPositionSource( IDbHelper dbHelper, short branchNo, short drawerNo,
			DateTime closedDt, ref string recSource, ref int posPtid )
		{
			string sql = null;
			object[] result = null;
			string extraWhere = null;
			//
			//#72101
			if (dbHelper.IsOfflineDb)
			{
				recSource = "P";
				return true;
			}
			//
			DbToUse sourceDb = GbHelper.DbToInquire( dbHelper );

			if (posPtid <= 0 || posPtid == DbInt.Null)
			{
				//#72324 - fixed the index problem for 24/7 mode
                extraWhere = string.Format(@" and ptid = (select max(ptid) from {0}tl_position
									where	branch_no = {1}
									and	drawer_no = {2}
									and closed_dt = '{3}' )", "{0}", branchNo, drawerNo, closedDt);
			}
			else
			{
				extraWhere = string.Format(@" and ptid = {0}", posPtid);
			}

			sql = string.Format(@"
			select ptid
			from {0}tl_position
			where	branch_no = {1}
			and	drawer_no = {2}
			and closed_dt = '{3}'
			{4}
			", "{0}", branchNo, drawerNo, closedDt, extraWhere);

			if ( !GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
			{
				if ( dbHelper.CopyStatus != "D" && dbHelper.IsPrimaryDbAvailable )
				{
					if ( sourceDb == DbToUse.SecondaryOnly  )
					{
						sql = string.Format( sql, dbHelper.PhoenixDbName + ".." );
						sourceDb = DbToUse.PrimaryOnly;
					}
					else
					{
						sql = string.Format( sql, "X_" );
						sourceDb = DbToUse.SecondaryOnly ;
					}
					if ( !GbHelper.ExecSqlImmediate( dbHelper, sql, ref result ))
					{
						return false;
					}
				}
				else
					return false;
			}

			if ( result[0] != null )
				posPtid = Convert.ToInt32( result[0]);

			if ( sourceDb == DbToUse.PrimaryOnly || sourceDb == DbToUse.Offline )
				recSource = "P";
			else
				recSource = "X";

			return true;
		}

		//#72101
		public bool GetPositionSource( IDbHelper dbHelper, short branchNo, short drawerNo,
			DateTime closedDt, ref string recSource )
		{
			int posPtid = -1;

			if (dbHelper.IsOfflineDb)
			{
				recSource = "P";
				return true;
			}

			return GetPositionSource( dbHelper, branchNo, drawerNo,
				closedDt, ref recSource, ref posPtid );
		}


		public decimal GetAcctRate(IDbHelper dbHelper, DateTime pdtTargetDt, string psAcctType, string psAcctNo, string psDepLn)
		{
			return GbHelper.GetAcctRate(dbHelper, pdtTargetDt, psAcctType, psAcctNo, psDepLn);
		}

		public string FormatAnyNumber(string unformattedNo, string numberFormat)
		{
			return GbHelper.FormatAnyNumber(unformattedNo, numberFormat);
		}

		#region Check for CTR pending trans
		public short GetCTRTranCount( IDbHelper dbHelper, short branchNo, short drawerNo, DateTime curBusDt, DateTime postingDt )
		{
			string sql;
			object[] result = null;

			if (curBusDt == DateTime.MinValue && postingDt != DateTime.MinValue)
				curBusDt = postingDt;
			if (curBusDt != DateTime.MinValue && postingDt == DateTime.MinValue)
				postingDt = curBusDt;

			sql = string.Format(@"
			select count(*)
			from {0}tl_journal
			where	branch_no = {1}
			{2}
			and empl_id = {3}
			and (effective_dt >= '{4}'
			and effective_dt <= '{5}')
			and crncy_id = 1
			and ctr_status IN ('D', 'I')", "{0}", branchNo, (drawerNo > 0? "and drawer_no = " + Convert.ToString(drawerNo) : ""), GlobalVars.EmployeeId, curBusDt, postingDt);

			if (GbHelper.ExecSqlImmediate(dbHelper, sql, ref result))
			{
			//	return -1;
				//#70997 - modified to look into ctr_control ctr_status column instead
				if (Convert.ToInt16( result[0]) > 0)
				{
					sql = string.Format(@"
				select count(*)
				from {0}tl_journal a, {0}ctr_acct b, {0}ctr_control c
				where	a.branch_no = {1}
				{2}
				and a.empl_id = {3}
				and (a.effective_dt >= '{4}'
				and a.effective_dt <= '{5}')
				and a.crncy_id = 1
				and a.ctr_status IN ('D', 'I')
				and a.ptid = b.journal_ptid
				and b.ctr_control_ptid = c.ptid
				and c.status IN ('D', 'I')", "{0}", branchNo, (drawerNo > 0? "and a.drawer_no = " + Convert.ToString(drawerNo) : ""), GlobalVars.EmployeeId, curBusDt, postingDt);

					if (!GbHelper.ExecSqlImmediate( dbHelper, sql, ref result))
						return 0;
				}
			}
			if (result != null)
				return Convert.ToInt16( result[0]);
			else
				return 0;
		}
		#endregion
        #region Check for credit trancode
        //Begin 32322
        public bool CheckIsTLTCCredit(IDbHelper dbHelper)
        {
            string sSql = null;
            PInt rnTranCount = new PInt("TranCount");
            short trancode = Convert.ToInt16(((PBaseType)_paramNodes[0]).IntValue);
            sSql = string.Format(@"
                                    Select	count(1)
                                    From	{0}ad_gb_tc AT Inner Join {0}ad_tl_tc ATC 
                                    On		AT.tran_code = ATC.tran_code
                                    And		AT.teller	 = 'Y'
                                    And		AT.debit_credit	= 0	
                                    And		AT.tran_code    = {1}", dbHelper.DbPrefix, trancode);
            GbHelper.ExecSqlImmediateInto(dbHelper, sSql, rnTranCount);
            _paramNodes[1] = rnTranCount;
            return true;
        }
        //End 32322

        #endregion




		#endregion other helper functions

		#endregion helper methods
	}

}
