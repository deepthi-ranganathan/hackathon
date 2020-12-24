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
// File Name: gbhelper.cs
// NameSpace: Phoenix.BusObj.Global.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//03-09-2006	1		mselvaga	#66440 - Created.
//04-15-2006    2       Vreddy      #67870 - Copied Format Account Number method from tl client helper.
//                                           Added Another methods to get and populate the account format
//05/11/2006	3		VDevadoss	#66123 - Added the GetAcctRate helper function.
//05/16/2006	4		rpoddar		#66440 - Moved the sql functions from tlhelper
//06/01/06		5		DGupta			Added custom action GetNameDetails to support client method.
//06/09/06		6		Vreddy		#67878 - ADded AccoutValidation (IntACctValidate)
//06/18/06		7		Vreddy		#67878 - dp_acct and Ln_acct are not in xapi db
//08/20/06		8		Vreddy		#67881 - Added Some GL common methods
//09/01/2006	9		vdevadoss	#69699 - ADded the phoenixdbname to psp_get_acct
//10/05/06		10		VDevadoss	#69248 - Added the custom option's GetClassDesc, GetNameDetails, GetEmployeeName, IsPMAEnabled, GetDepLoan to ActionCustom
//10/27/06		11		VDevadoss	#69248 - Added the custom option's GetCustomerDetails and GetWQCategoryDetails.
//11/17/06		12		VDevadoss	#70825 - Check if Deploan is empty string when tryin to select acct_type.
//01/02/07		13		vreddy		#71170 - Positive error noshas no error text
//01/30/2007	14		mselvaga	#71645 - Receive Phoenix Error #1 - Value was either too large or too small for an Int32. when selecting Display on a RIM that exceeds rim length
//2/7/07		15		DGupta		#70356 - ported after contention- Added VerifyCpyLimits & VerifyUserRights() methods.
//02/09/07		16		VDevadoss	#70728 - Added code for Commitments.
//May-24-07		17   	Muthu       #72780 VS2005 Migration
//06/05/07      18      vdevadoss   #71329 - Added method to close the Reader.
//10/24/2007    19      mselvaga    #73503 - Ported CalcBusDt from rmachbatch.
//12/04/07      20      bbedi       #74019 - Mailing Address Display
//01/15/2007    21      rnammundi   #74024 - Stops on LOC's
//03/04/08		22		DGupta	    #74497 - Modified pSqlHelper & IPhoenixDataType to protected as OpsHelper is derived from it.
//04/11/2008    23      rpoddar     #75763 - Added  new Custom Action GetAcctStatus. Added Audit functions for CTD to .NET Porting
//07/07/2008    23      rpoddar     #01031 - Added  new method UDFCopyUpdate
//10/02/2008    24      RBhavsar    Moved VerifyCpyLimits method to RmAchCompany BO.
//10/26/2008    25      iezikeanyi  #76429 - Added code to get count of acctounts with cross reference
//12/08/2008	26		EWAHIB		#76404 - Add New Overload for CreateAuditView with Acct No & type as parameters
//12/08/2008	27		rpoddar		#76270 - Added UpdateStmtStatus
//01/27/2009    28      mselvaga    #76458 - Added EX account changes.
//02/02/2009    29      VDevadoss   #76036 - Added function GetRmAddrDetails for address in effect.
//03/02/2009    30      VDevadoss   #76099 - Added custom action GetDBDateDiff to compute datediff between 2 dates.
//04/02/2009    31      ewahib/RB   #02769 - Change Convert.ToInt32 to Convert.Decimal while calling auditDiff.CreateAuditEntry
//05/27/2009    32      ewahib/RB   #03830 - Add CMT handling
//06/17/2009    33      mramlin     WI-4459 - Added code to handle the PrimDbAvaialbe for AuditView, and HC_HC look up
//21Jul2009     34      dfutcher    #5098 - pass 'null' when variables are null.
//05/24/2009    34      Nelsehety   #02512 - reset value of sSQl in GenerateAccount Fn.
//07/07/2009    35	    ahussien    #02829 - When using GetRim for SD Account , we must select the latest one.
//10/02/2009    36      iezikeanyi  #5871 - Added DepLoan select for ex_acc

//07/28/2009    37      Nelsehety   #04976 - Added custom action GetWorkDetailsForNewAcct and NicknameUpdate.
//08/09/2009    38      Mona        #04976 - Converting WorkId in fn "GetWorkDetailsForNewAcct" from int to decimal
//09/26/2009    39      ewahib      #05060 - Fix GetFromGLControl as it is causing problems on Sql Server
//11/23/2009    40      rpoddar     #06167 - Added method GetDataSet. Fixed the sql functions to point to the Sqlhelper class
//11/23/2009    41      Mona        #6674 - In Function IsPMAEnabled select network_email only.
//21Apr2010     42      dfutcher    #8600
//05/27/2010    43      VDevadoss   #9177 - Added custom action "CalculateAge"
//06/09/2010    44      VDevadoss   #9321 - Added primbname instead of dbprefix when calling "psp_calc_age" as the proc does not exist in XAPI db.
//07/27/2010    45      SDhamija	#9888 - holding co doesnt have xapi db. so removing xp_control from union for holding co.
// 29Sep2010    46      dfutcher        #10410 - Handle spaces and nulls for external accounts (masked)
//01/19/2011    47      SDighe      WI#80635 - Change Auth Process Ability to place holds on loans.
//01/27/2011    48      DEiland     WI#12289/#12290 - GenerateAccount Needs to handle DP_UMB account generation
//08/26/2011    49      mselvaga    #80674 - KMS Integration.
//11/01/11      50      VDevadoss   #15978 - Added code to hande AcctType = "RIM" in GetDepLoan
//2012-12-19    51      jabreu      140769 - Relationship Pricing
//01/25/2013    52	    rpoddar     #140789 - Employee Exchange
//03/07/2013    53      DEiland     WI#20348 - GetAcctAlerts passing Module name now
//5/30/2013     54      MBachala    wi 22968 - performance changes for OPS banks
//06/20/2013    55      JRhyne      WI#23705/23706 - workflow search
//6/27/2013     56      JRhyne		wi 22968 (2) fix deploan not set issue
//09/23/2013	57		JRhyne		WI#24730 - fix bug with xapi mode and x_sysobjects
//10/22/2013    58      bhughes     140793
//7/25/2013     59      MKrishna	146541 Added New method for validation loan (MIO)
//03/10/2014    60      RDeepthi    WI #27540. Added New Mthod GetPostEditAccess for Getting Post and Edit Access of the Employee
//04/01/2014	61		jabreu		140796 - Advance Restriction
//07Apr2014     62      dfutcher    140788 Alerts
//07/09/2014    63      SJoseph     #29458 - Added code to returen the account type as SD(if there is no account type corresponding to SK ).
//07/14/2014    64      RDeepthi    #29994 - Changed Convert.ToInt16 to Convert.ToInt32 for rimNo.
//08/05/2014    65      BHUGHES     30303, enh 76458, Change db name when in night mode to x_ex_acct
//08/06/2014	66		jrhyne		WI#30074 - when updating status for statements, close != Active gb_comb_stmt_anal records
//01/23/2014	67		jrhyne		WI#34412 - add boolean to createauditentry so programmer can control isnew flag
//02/05/2015    68      ckane       #32310 - remove hardcoded reference to "USA"
//5/6/2015      69      SPatterson  Enh#173485 - Refactored GetNameDetails custom action to help building names on the server.
//6/9/2015      70      MBachala    174961 -  sales and service
//11/30/2015    71      NikhilNK    #39577 174961 - Safe Deposit Box needs additional Identifier added to SS_PROD_ACCT
//06/14/2016    72      Alfred      #46760 -  Added method IsNachaHoliday().
//10/12/2015	73		Anju        #198018- 53311- Show user name in Display Window.
//11/3/2016     74      bhughes     196262 - Commitments advanced options
//05/3/2017     75      GlaxeenaJ   Enh #209693 US #63933 Task #63936-New method to select biProduct from Helper Class instead of MDI.
//05/25/2017    76      Kiran       Task#65549 - PCI Enhancement - Added new method to get decrypted Pan
//08/30/2017    77      Kiran       Task#71500 - PCI Enhancement - Added new method to get decrypted Ex Acct no
//08/30/2017    78      Kiran       Task#70838 - PCI Enhancement - Select four digit acct no if masked acct No is null
//02/19/2018    79       Minu        Task #84178-Addressing code review comments for Invision
//03/28/2018    80      Shebin      Task#82748 - Added new method to validate GL account, For reverse & reapply,system should allow GL account with ledger type in Asset/Liability
//05/14/2018    81      RBhavsar    #82748/#51409 - Added the missing override for ValidateGLLedgerType due to merge changes.
//07/12/2018    82      Minu        #94549- Show message when user trying to post transactions, when drawer is closed out.
//02/13/2020    83      Vipin       Bug 124318:Disable Workflow button from Customer Management/Teller for Non Workflow Clients
//02/26/2020    84      Arun        US #114528 | Task #124261 - SQL Injection fix for GetAcctAlerts custom action.
//07/29/2020    85      Kiran       US #130110 | Task 130111 - Added new custom option to get rim number by PAN
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Collections;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Xm;
using Phoenix.BusObj.Admin.Deposit;
using Phoenix.BusObj.Admin.Loan;
using Phoenix.BusObj.Admin.Xapi;
//using Phoenix.BusObj.RIM;     #23784
//using Phoenix.BusObj.Admin.Loan;
using Phoenix.Shared;
using Phoenix.Shared.BusFrame;          //#06167

namespace Phoenix.BusObj.Global.Server
{
	/// <summary>
	/// Summary description for TlCc.
	/// </summary>
	public class GbHelper: Phoenix.BusObj.Global.GbHelper
	{
		#region private vars
        //Begin #06167
        //protected PSqlHelper pSqlHelper = new PSqlHelper();
        //protected IPhoenixDataType[] intoList = null;
        private SqlHelper _sqlHelper = new SqlHelper();
        //End #06167
		#endregion

		#region public constructors
		public GbHelper():base()
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

		#region ******** Custom Action ***********
		protected override bool OnActionCustom(IDbHelper dbHelper, bool isPrimaryDb )
        {
            #region Create Audit View
            if (CustomActionName == "CreateViewAudit")
            {

                return CreateViewAudit(dbHelper, isPrimaryDb);
            }
            #endregion

            //Begin Enh#69248
            #region Class Desc
            if (this.CustomActionName == "GetClassDesc")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType deploan = _paramNodes[0] as PBaseType;
                    PBaseType accttype = _paramNodes[1] as PBaseType;
                    PBaseType classcode = _paramNodes[2] as PBaseType;
                    PBaseType classdesc = _paramNodes[3] as PBaseType;

                    if (!deploan.IsNull)
                    {
                        //#70825
                        if (deploan.StringValue.Trim() != string.Empty)
                        {
                            #region Get the class desc
                            string sSQL = string.Empty;
                            string sTablename = String.Empty;
                            string sDepLoan = deploan.StringValue;

                            if (sDepLoan.Trim() == "DP")
                                sTablename = "AD_DP_CLS c";
                            else if (sDepLoan.Trim() == "LN")
                                sTablename = "AD_LN_CLS c";
                            else if (sDepLoan.Trim() == "SD")
                                sTablename = "AD_SD_CLS c";
                            else
                                sTablename = "AD_EX_ACCT_TYPE c";

                            if (dbHelper.CopyStatus != "D")
                                sTablename = dbHelper.XmDbName + "..X_" + sTablename;
                            else
                                sTablename = dbHelper.PhoenixDbName + ".." + sTablename;

                            sSQL = string.Empty;

                            if (sDepLoan == "EXT")
                            {
                                sSQL = string.Format(@"
									Select
										c.DESCRIPTION
									From
										{0}
									Where
										c.ACCT_TYPE = '{1}'", sTablename, accttype.StringValue);
                            }
                            else
                            {
                                sSQL = string.Format(@"
									Select
										c.DESCRIPTION
									From
										{0}
									Where
										c.ACCT_TYPE = '{1}'
									And
										c.CLASS_CODE = {2} ", sTablename, accttype.StringValue, classcode.ValueObject);
                            }
                            ExecSqlImmediateInto(dbHelper, sSQL, classdesc);
                        }

                            #endregion
                    }
                }
            }
            #endregion

            #region GetEmployeeName
            if (this.CustomActionName == "GetEmployeeName")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType emplid = _paramNodes[0] as PBaseType;
                    PBaseType emplname = _paramNodes[1] as PBaseType;

                    string sTablename = string.Empty;
                    string sSQL = string.Empty;

                    if (dbHelper.CopyStatus != "D")
                        sTablename = dbHelper.XmDbName + "..X_AD_GB_RSM a";
                    else
                        sTablename = dbHelper.PhoenixDbName + "..AD_GB_RSM a";

                    sSQL = string.Format(@"
								Select
									ltrim(rtrim(a.NAME))
								From
									{0}
								Where
									EMPLOYEE_ID = {1}", sTablename, emplid.ValueObject);

                    ExecSqlImmediateInto(dbHelper, sSQL, emplname);
                }
            }
            #endregion

            #region GetBankRoutingNo
            if (this.CustomActionName == "GetBankRoutingNo")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType routingNo = _paramNodes[0] as PBaseType;

                    string sTablename = string.Empty;
                    string sSQL = string.Empty;

                    if (dbHelper.CopyStatus != "D")
                        sTablename = dbHelper.XmDbName + "..X_AD_GB_BANK a";
                    else
                        sTablename = dbHelper.PhoenixDbName + "..AD_GB_BANK a";

                    sSQL = string.Format(@"
								Select
									ltrim(rtrim(a.routing_no))
								From
									{0}", sTablename);

                    ExecSqlImmediateInto(dbHelper, sSQL, routingNo);
                }
            }
            #endregion

            #region IsPMAEnabled
            if (this.CustomActionName == "IsPMAEnabled")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType pmaenabled = _paramNodes[0] as PBaseType;

                    string sTablename = string.Empty;
                    string sSQL = string.Empty;

                    if (dbHelper.CopyStatus != "D")
                        sTablename = dbHelper.XmDbName + "..X_AD_GB_BANK_CONTROL c";
                    else
                        sTablename = dbHelper.PhoenixDbName + "..AD_GB_BANK_CONTROL c";

                    #region #6674
                    //                    sSQL = string.Format(@"
                    //								Select
                    //									ltrim(rtrim(ISNULL(c.NETWORK_EMAIL, 'N')))
                    //								From
                    //									{0} ", sTablename);
                    sSQL = string.Format(@"
								Select
								        c.NETWORK_EMAIL
								From
									{0} ", sTablename);
                    #endregion

                    ExecSqlImmediateInto(dbHelper, sSQL, pmaenabled);
                }
            }
            #endregion

			#region IsWorkflowInstalled      
			if (this.CustomActionName == "IsWorkflowInstalled")      // WI#23705/23706
			{
				string sSQL;
				PBaseType pbWorkflowInstalled = _paramNodes[0] as PBaseType;
				PInt nBit = new PInt("Bit");
                /*begin #124318 - Workflow merged to Phoenix. So we are checking whether workflow is licenced or not.*/
				// WI#24730 - look to phoenix only, because no such thing as x_sysobjects 
				sSQL = string.Format(@"select 1 where exists(Select 1 from {0}..pc_wf_product  
                                                             Where product_guid = '9B7CA73F-B940-4995-ACD6-2109001CA029' 
	                                                         and product_name = 'Fusion Workflow Manager' 
	                                                         and product_key is not null and LEN(product_key) > 1)", dbHelper.PhoenixDbName);
                /*end #124318*/                
				ExecSqlImmediateInto(dbHelper, sSQL, nBit);

				pbWorkflowInstalled.Value = (nBit.IntValue == 1);
			}
			#endregion

			#region GetDepLoan
			if (this.CustomActionName == "GetDepLoan")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType valuetype = _paramNodes[0] as PBaseType;
                    PBaseType valuestr = _paramNodes[1] as PBaseType;
                    PBaseType deploan = _paramNodes[2] as PBaseType;

                    deploan.ValueObject = GetDepLoan(dbHelper, valuestr.StringValue, valuetype.StringValue);
                }
            }
            #endregion

            #region Get Rim No
            if (this.CustomActionName == "GetRimNo")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType accttype = _paramNodes[0] as PBaseType;
                    PBaseType acctno = _paramNodes[1] as PBaseType;
                    PBaseType rimno = _paramNodes[2] as PBaseType;

                    rimno.ValueObject = GetRimNo(dbHelper, accttype.StringValue, acctno.StringValue);
                }
            }
            /*Begin #130111*/
            if (this.CustomActionName == "GetRimNoByPan")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType panNo = _paramNodes[0] as PBaseType;
                    PBaseType rimno = _paramNodes[1] as PBaseType;

                    rimno.ValueObject = GetRimNoByPan(dbHelper, panNo.StringValue);
                }
            }
            /*End #130111*/
            #endregion

            #region Get Customer Details
            if (this.CustomActionName == "GetCustomerDetails")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType RimNo = _paramNodes[0] as PBaseType;
                    PBaseType PrimaryEmail = _paramNodes[1] as PBaseType;

                    if (!RimNo.IsNull)
                    {
                        //Begin #23784
                        //Phoenix.BusObj.RIM.RmAcct _rmAcct = new Phoenix.BusObj.RIM.RmAcct();

                        //_rmAcct.SelectAllFields = true;
                        //_rmAcct.ActionType = XmActionType.Select;
                        //_rmAcct.RimNo.Value = Convert.ToInt32(RimNo.ValueObject);
                        //_rmAcct.DoAction(dbHelper);



                        //if (!_rmAcct.RimType.IsNull)	//Just check for a NOT NULL field.
                        //{
                        //    PrimaryEmail.ValueObject = (_rmAcct.Email1.IsNull ? string.Empty : _rmAcct.Email1.Value);
                        //}

                        string sql = string.Format(@"select Email_1 from {0}rm_acct where rim_no = {1}", dbHelper.DbPrefix, RimNo.SqlString);
                        ExecSqlImmediateInto(dbHelper, sql, PrimaryEmail);
                        if (PrimaryEmail.IsNull)
                            PrimaryEmail.Value = string.Empty;


                        // End #23784
                    }
                }
            }
            #endregion

            #region Get Work Queue Catergory Details
            if (this.CustomActionName == "GetWQCategoryDetails")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType CatId = _paramNodes[0] as PBaseType;
                    PBaseType CatDefEmplId = _paramNodes[1] as PBaseType;
                    PBaseType CatDefQueueId = _paramNodes[2] as PBaseType;
                    PBaseType CatDesc = _paramNodes[3] as PBaseType;
                    PBaseType CatAutoEmailOwner = _paramNodes[4] as PBaseType;

                    if (!CatId.IsNull)
                    {
                        Phoenix.BusObj.Admin.Misc.AdWqCategory _adWqCategory = new Phoenix.BusObj.Admin.Misc.AdWqCategory();

                        _adWqCategory.SelectAllFields = true;
                        _adWqCategory.ActionType = XmActionType.Select;
                        _adWqCategory.CategoryId.Value = Convert.ToInt16(CatId.ValueObject);
                        _adWqCategory.DoAction(dbHelper);

                        if (!_adWqCategory.DefaultEmplId.IsNull)
                            CatDefEmplId.ValueObject = _adWqCategory.DefaultEmplId.Value;
                        if (!_adWqCategory.QueueId.IsNull)
                            CatDefQueueId.ValueObject = _adWqCategory.QueueId.Value;
                        if (!_adWqCategory.Description.IsNull)
                            CatDesc.ValueObject = _adWqCategory.Description.Value;
                        if (!_adWqCategory.AutoEmailOwner.IsNull)
                            CatAutoEmailOwner.ValueObject = _adWqCategory.AutoEmailOwner.Value;
                    }
                }
            }
            #endregion

            //End Enh#69248

            /* Begin #74019 */
                #region GetAccessView
                if (this.CustomActionName == "GetAccessView")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType screenid = _paramNodes[0] as PBaseType;
                    PBaseType modulename = _paramNodes[1] as PBaseType;
                    PBaseType accessview = _paramNodes[2] as PBaseType;

                    if (!screenid.IsNull)
                    {
                        #region Get the class desc
                        string sSQL = string.Empty;

                        int nScreenId = screenid.IntValue;
                        string sModuleName = modulename.StringValue;


                        sSQL = string.Format(@"
                                Select view_access
                                From {0}AD_GB_RSM_RES
                                Where EMPLOYEE_ID = {1}
                                and MODULE = '{2}'
                                and SCREEN_ID = {3}", dbHelper.PhoenixDbName + "..", Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId, sModuleName, nScreenId);

                        if (isPrimaryDb)
                        {
                            ExecSqlImmediateInto(dbHelper, sSQL, accessview);
                        }


                        #endregion
                    }
                }
            }
            #endregion
            /* End #74019 */

            #region CalculateDate
            if (this.CustomActionName == "CalculateDate")
            {
                #region Handle paramNode array members
                PBaseType date = _paramNodes[0] as PBaseType;
                PBaseType calculatedDate = _paramNodes[1] as PBaseType;
                #endregion

                #region local variables
                StringBuilder sSQL = new StringBuilder();
                IDataReader DataReader = null;
                string sql = "";
                int error = 0;
                #endregion

                if (_paramNodes.Count == 0)
                    return true;
                if (_paramNodes.Count > 0)
                {

                    try
                    {
                        sql =
                            "declare	@nRC	integer, @dtWorkCalcDt	smalldatetime, @dtWorkStartDt	smalldatetime " + "\n" +
                            "exec @nRC = " + dbHelper.DbPrefix + "psp_processing_dates" + "\n" +
                            "'" + Conversion.DateToString(new DateTime(date.DateTimeValue.Year, date.DateTimeValue.Month, date.DateTimeValue.Day, 0, 0, 0)) + "'" + ",\n" +
                            "2" + ",\n" +
                            "@dtWorkStartDt OUTPUT" + ",\n" +
                            "@dtWorkCalcDt OUTPUT" + "\n" +

                            "Selec @nRC, @dtWorkCalcDt";

                        DataReader = dbHelper.ExecuteReader(sql);

                        while (DataReader.Read())
                        {
                            if (!DataReader.IsDBNull(0))
                            {
                                error = DataReader.GetInt32(0);
                            }
                            if (!DataReader.IsDBNull(1))
                            {
                                calculatedDate.ValueObject = DataReader.GetDateTime(1);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        CoreService.ExceptionMgr.NewException(1002, 300112, ex.InnerException, ex.Message);
                    }
                    finally
                    {
                        if (DataReader != null)
                            DataReader.Dispose();
                    }
                }

            }
            #endregion

            #region 9177 CalculateAge
            if (this.CustomActionName == "CalculateAge")
            {
                #region Handle paramNode array members
                PBaseType birthdate = _paramNodes[0] as PBaseType;
                PBaseType sysdate = _paramNodes[1] as PBaseType;
                PBaseType calcAge = _paramNodes[2] as PBaseType;
                #endregion

                #region local variables
                IDataReader DataReader = null;
                string sql = "";
                int error = 0;
                #endregion

                if (_paramNodes.Count == 0)
                    return true;

                if (_paramNodes.Count > 0 && dbHelper.IsPrimaryDbAvailable)
                {
                    /*???? - Always call the psp_calc_age in phoenix as the proc does not exist in XAPI*/
                    sql =
                        "Declare @nRC int, @rsTotalAge varchar(60) " + "\n" +
                        "exec @nRC = " + dbHelper.PhoenixDbName + "..PSP_CALC_AGE" + "\n" +
                        "'" + Conversion.DateToString(new DateTime(birthdate.DateTimeValue.Year, birthdate.DateTimeValue.Month, birthdate.DateTimeValue.Day, 0, 0, 0)) + "'" + ",\n" +
                        "'" + Conversion.DateToString(new DateTime(sysdate.DateTimeValue.Year, sysdate.DateTimeValue.Month, sysdate.DateTimeValue.Day, 0, 0, 0)) + "'" + ",\n" +
                        "0" + ",\n" +
                        "@rsTotalAge OUTPUT" + "\n" +

                        "Select @nRC, @rsTotalAge";

                    DataReader = dbHelper.ExecuteReader(sql);

                    while (DataReader.Read())
                    {
                        if (!DataReader.IsDBNull(0))
                        {
                            error = DataReader.GetInt32(0);
                        }
                        if (!DataReader.IsDBNull(1))
                        {
                            calcAge.ValueObject = DataReader.GetString(1);
                        }
                    }

                    if (DataReader != null)
                        DataReader.Dispose();
                }
            }
            #endregion

            #region AcctTypeDetails
            else if (this.CustomActionName == "AcctTypeDetails")
            {
                if (_paramNodes.Count > 0)
                    (_paramNodes[0] as PBaseType).ValueObject = LoadAcctTypeDetails(dbHelper);
            }
            #endregion

            #region GetNameDetails
            else if (this.CustomActionName == "GetNameDetails")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType rimNo = _paramNodes[0] as PBaseType;
                    PBaseType firstName = _paramNodes[1] as PBaseType;
                    PBaseType middleInitial = _paramNodes[2] as PBaseType;
                    PBaseType lastName = _paramNodes[3] as PBaseType;
                    PBaseType rimType = _paramNodes[4] as PBaseType;
                    PBaseType title = _paramNodes[5] as PBaseType;
                    PBaseType suffix = _paramNodes[6] as PBaseType;
                    PBaseType fullname = _paramNodes[7] as PBaseType;	//Enh# 69248

                    // Enh#173485 - Refactored
                    GetNameDetails(dbHelper, rimNo, firstName, middleInitial, lastName, rimType, title, suffix, fullname);

                }
            }
            else if (this.CustomActionName == "GetUnRelatedName") /*198018 - For getting the name of unrelated user*/
            {
                GetUnRelatedName(dbHelper);
            }
            #endregion

            #region Validate Account Number
            else if (this.CustomActionName == "ValidateAcctNo")
            {
                if (_paramNodes.Count > 0)
                {

                    PBaseType AccountType = _paramNodes[0] as PBaseType;
                    PBaseType AccountNumber = _paramNodes[1] as PBaseType;
                    PBaseType Result = _paramNodes[2] as PBaseType;
                    string acctStatus = null;       // #75763
                    Result.ValueObject = 0;
                    if (!ValidateAcct(dbHelper, AccountType.StringValue, AccountNumber.StringValue, ref acctStatus))        // #75763 - Added status
                    {
                        Result.ValueObject = 0; // Failed
                    }
                    else
                    {
                        Result.ValueObject = 1; //Success
                    }
                }
            }
             /*Begin Task #82748*/
            else if (this.CustomActionName == "ValidateGLLederType")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType AccountType = _paramNodes[0] as PBaseType;
                    PBaseType AccountNumber = _paramNodes[1] as PBaseType;
                    PBaseType Result = _paramNodes[2] as PBaseType;                      
                    Result.ValueObject = 0;

                    if (!ValidateGLLederType(dbHelper, AccountType.StringValue, AccountNumber.StringValue))
                    {
                        Result.ValueObject = 0; // Failed
                    }
                    else
                    {
                        Result.ValueObject = 1; //Success
                    }
                }
            }
            /*End Task #82748*/
            // Begin #75763
            else if (this.CustomActionName == "GetAcctStatus")
            {
                if (_paramNodes.Count > 0)
                {

                    PBaseType AccountType = _paramNodes[0] as PBaseType;
                    PBaseType AccountNumber = _paramNodes[1] as PBaseType;
                    PBaseType AccountStatus = _paramNodes[2] as PBaseType;
                    string acctStatus = null;
                    if (!ValidateAcct(dbHelper, AccountType.StringValue, AccountNumber.StringValue, ref acctStatus))        // #75763 - Added status
                    {

                    }
                    else
                    {
                        AccountStatus.Value = acctStatus;
                    }
                }
            }
            // End #75763
            #endregion

            #region GL_ACCT_STRUCTURE ==> GLAcctStructure or Mask

            else if (this.CustomActionName == "GetGLAcctStructure")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType FullMask = _paramNodes[0] as PBaseType;
                    PBaseType LedgerMaskLevel100 = _paramNodes[1] as PBaseType;
                    PBaseType GrpMember = _paramNodes[2] as PBaseType;
                    string fullMask = string.Empty;
                    string mask;
                    PString Mask = new PString("Mask");
                    PString Type = new PString("Type");
                    PInt Position = new PInt("Position");
                    string sqlCommand = @"SELECT	MASK, STRUCTURE_TYPE, POSITION
					FROM 	{0}GL_ACCT_STRUCTURE
					ORDER BY POSITION ";
                    if (ExecSqlImmediateInto(dbHelper, sqlCommand, false, true, Mask, Type, Position))
                    {
                        do
                        {
                            if (!Mask.IsNull)
                                mask = Mask.StringValue.Replace("#", "9");
                            else
                                mask = string.Empty;
                            if (!Position.IsNull && Position.Value > 1)
                                fullMask = fullMask + "-";
                            fullMask = fullMask + mask;
                        }
                        while (ExecSqlGetNextRow());
                        fullMask = fullMask + "-";
                        LedgerMaskLevel100.ValueObject = GetGLLedgerMask(dbHelper, 100); //Level 100
                        if (!LedgerMaskLevel100.IsNull)
                            fullMask = fullMask + LedgerMaskLevel100.StringValue;

                        FullMask.ValueObject = fullMask;
                    }

                    //PInt GrpMember = new PInt("GrpMember");
                    sqlCommand = @"Select	count(*)
						From	{0}gl_grp_members
						Where	member_user_id = " + GlobalVars.EmployeeId.ToString() +
                        " And	effective_dt <= '" + GlobalVars.SystemDate.ToString() + "'";
                    ExecSqlImmediateInto(dbHelper, sqlCommand, true, false, GrpMember);
                }

            }
            #endregion GL_ACCT_STRUCTURE

            #region GL_ACCT_TOTAL_STRU
            else if (this.CustomActionName == "GetGLLedgerMask")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType LevelNo = _paramNodes[0] as PBaseType;
                    PBaseType LedgerMaskLevel100 = _paramNodes[1] as PBaseType;
                    LedgerMaskLevel100.ValueObject = GetGLLedgerMask(dbHelper, LevelNo.IntValue);
                }
            }
            #endregion GL_ACCT_TOTAL_STRU

            #region GetGLLedgerRestrictions
            else if (this.CustomActionName == "GetGLLedgerRestrictions")
            {
                if (_paramNodes.Count > 0)
                {

                    #region Get If he has any group attached
                    string sqlCommand;
                    PBaseType EmplId = _paramNodes[0] as PBaseType;
                    PBaseType GLLedgerNos = _paramNodes[1] as PBaseType;
                    PBaseType GrpMember = _paramNodes[2] as PBaseType;
                    GLLedgerNos.ValueObject = string.Empty;
                    string glLedgerNos = string.Empty;
                    //PInt GrpMember = new PInt("GrpMember");
                    sqlCommand = @"Select	count(*)
						From	{0}gl_grp_members
						Where	member_user_id = " + EmplId.StringValue +
                        " And	effective_dt <= '" + GlobalVars.SystemDate.ToString() + "'";
                    ExecSqlImmediateInto(dbHelper, sqlCommand, true, false, GrpMember);
                    #endregion

                    #region If there are groups build the list
                    if (!GrpMember.IsNull && GrpMember.IntValue > 0)
                    {
                        PString RestrictLedger = new PString("RestrictLedger");
                        sqlCommand = @"    Select  c.prefix_acct
										   From {0}gl_grp_members a, {0}gl_access_grp b, {0}gl_grp_content c
										   Where member_user_id = " + EmplId.StringValue +
                                        @" And a.effective_dt <= '" + GlobalVars.SystemDate.ToString() + "'" +
                                        @" And a.access_grp_id = b.access_grp_id
										   And grp_type = '1'
										   And b.access_grp_id = c.access_grp_id
										   And b.restrict_grant = 'Restrict'
										   And b.access_type = 'Y'";

                        if (ExecSqlImmediateInto(dbHelper, sqlCommand, true, true, RestrictLedger))
                        {
                            do
                            {
                                if (glLedgerNos == string.Empty)
                                    glLedgerNos = RestrictLedger.StringValue;
                                else
                                    glLedgerNos = glLedgerNos + @"^" + RestrictLedger.StringValue;
                            }
                            while (ExecSqlGetNextRow());
                        }
                        GLLedgerNos.ValueObject = glLedgerNos;
                    }
                    #endregion

                } //End Of Parameter Count
            }
            #endregion GetGLLedgerRestrictions

            #region GetGLPrefixRestrictions
            else if (this.CustomActionName == "GetGLPrefixRestrictions")
            {
                if (_paramNodes.Count > 0)
                {
                    string sqlCommand;
                    #region Get If he has any group attached
                    PBaseType EmplId = _paramNodes[0] as PBaseType;
                    PBaseType GLPrefixes = _paramNodes[1] as PBaseType;
                    GLPrefixes.ValueObject = string.Empty;
                    string glPrefixes = string.Empty;
                    PInt GrpMember = new PInt("GrpMember");
                    sqlCommand = @"Select	count(*)
						From	{0}gl_grp_members
						Where	member_user_id = " + EmplId.StringValue +
                        " And	effective_dt <= '" + GlobalVars.SystemDate.ToString() + "'";
                    ExecSqlImmediateInto(dbHelper, sqlCommand, true, false, GrpMember);
                    #endregion

                    #region If there are groups build the list
                    if (!GrpMember.IsNull && GrpMember.IntValue > 0)
                    {
                        PString RestrictPrefix = new PString("RestrictPrefix");
                        sqlCommand = @"Select	c.prefix_acct
									    From {0}gl_grp_members a, {0}gl_access_grp b, {0}gl_grp_content c
									   Where member_user_id = " + EmplId.StringValue +
                                    @" And	a.effective_dt <= '" + GlobalVars.SystemDate.ToString() + "'" +
                                    @" And	a.access_grp_id = b.access_grp_id
									   And  b.access_grp_id = c.access_grp_id
									   And a.expire_dt >= '" + GlobalVars.SystemDate.ToString() + "'" +
                                    @" And 	b.restrict_grant = 'Restrict'
									   And grp_type = '0' ";

                        if (ExecSqlImmediateInto(dbHelper, sqlCommand, true, true, RestrictPrefix))
                        {
                            do
                            {
                                if (glPrefixes == string.Empty)
                                    glPrefixes = RestrictPrefix.StringValue;
                                else
                                    glPrefixes = glPrefixes + @"^" + RestrictPrefix.StringValue;
                            }
                            while (ExecSqlGetNextRow());
                        }
                        GLPrefixes.ValueObject = glPrefixes;
                    }
                    #endregion

                } //End Of Parameter Count
            }
            #endregion GetGLPrefixRestrictions

            #region GetAcctAlerts
            else if (this.CustomActionName == "GetAcctAlerts")
            {
                GetAcctAlerts(dbHelper);
            }
            #endregion

            //#73503
            #region Calculate Business date
            if (this.CustomActionName == "CalcBusDate")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType InputDt = _paramNodes[0] as PBaseType;
                    PBaseType PrevNext = _paramNodes[1] as PBaseType;
                    PBaseType LeadDays = _paramNodes[2] as PBaseType;
                    PBaseType BusDate = _paramNodes[3] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    CalcBusDate(dbHelper, InputDt, PrevNext, LeadDays, BusDate);
                }
            }
            #endregion

            #region Get Rate
            if (this.CustomActionName == "GetRate")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType IndexId = _paramNodes[0] as PBaseType;
                    PBaseType TargetDate = _paramNodes[1] as PBaseType;
                    PBaseType Rate = _paramNodes[2] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetRate(dbHelper, IndexId, TargetDate, Rate);
                }
            }
            #endregion

            #region Get DbRelase
            if (this.CustomActionName == "GetDbRelease")    //#80674
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType DbRelease = _paramNodes[0] as PBaseType;
                    GetDbRelease(dbHelper, DbRelease);
                }
            }
            #endregion

            #region GetFromAdLnControl
            if (this.CustomActionName == "GetFromAdLnControl")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType AvailBal = _paramNodes[0] as PBaseType;
                    PBaseType PostBal = _paramNodes[1] as PBaseType;
                    PBaseType CashAccrual = _paramNodes[2] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromAdLnControl(dbHelper, AvailBal, PostBal, CashAccrual);
                }
            }
            #endregion

            #region GetFromAdLnUmbControl
            if (this.CustomActionName == "GetFromAdLnUmbControl")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType CashAccrual = _paramNodes[0] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromAdLnUmbControl(dbHelper, CashAccrual);
                }
            }
            #endregion

            #region GetFromAdDpControl
            if (this.CustomActionName == "GetFromAdDpControl")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType AvailBal = _paramNodes[0] as PBaseType;
                    PBaseType PostBal = _paramNodes[1] as PBaseType;
                    PBaseType ColBal = _paramNodes[2] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromAdDpControl(dbHelper, AvailBal, PostBal, ColBal);
                }
            }
            #endregion

            #region GetFromAdSdControl
            if (this.CustomActionName == "GetFromAdSdControl")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType CashAccrual = _paramNodes[0] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromAdSdControl(dbHelper, CashAccrual);
                }
            }
            #endregion

            #region GetFromHcHc
            if (this.CustomActionName == "GetFromHcHc")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType MultiCurrency = _paramNodes[0] as PBaseType;
                    PBaseType HoldingCompany = _paramNodes[1] as PBaseType;
                    PBaseType CallReportType = _paramNodes[2] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromHcHc(dbHelper, MultiCurrency, HoldingCompany, CallReportType);
                }
            }
            #endregion

            #region GetFromAdGbBankControl
            if (this.CustomActionName == "GetFromAdGbBankControl")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType PhoneMask = _paramNodes[0] as PBaseType;
                    PBaseType PhoneMaskLength = _paramNodes[1] as PBaseType;
                    PBaseType NoDaysFuture = _paramNodes[2] as PBaseType;
                    PBaseType NoDaysBack = _paramNodes[3] as PBaseType;
                    PBaseType DefaultTIN = _paramNodes[4] as PBaseType;
                    PBaseType FormatTIN = _paramNodes[5] as PBaseType;
                    PBaseType DefaultPhone = _paramNodes[6] as PBaseType;
                    PBaseType FormatPhone = _paramNodes[7] as PBaseType;
                    PBaseType DefaultZIP = _paramNodes[8] as PBaseType;
                    PBaseType FormatZIP = _paramNodes[9] as PBaseType;
                    PBaseType PhoenixJobCode = _paramNodes[10] as PBaseType;
                    PBaseType IntlCountryCode = _paramNodes[11] as PBaseType;
                    PBaseType AcctTypeSearch = _paramNodes[12] as PBaseType;
                    PBaseType DatabaseLanguageId = _paramNodes[13] as PBaseType;
                    PBaseType AchDRLeadDays = _paramNodes[14] as PBaseType;
                    PBaseType AchCRLeadDays = _paramNodes[15] as PBaseType;
                    PBaseType RequirePrenote = _paramNodes[16] as PBaseType;
                    PBaseType PrenoteLeadDays = _paramNodes[17] as PBaseType;
                    PBaseType CheckNSF = _paramNodes[18] as PBaseType;
                    PBaseType GenSecClsRel = _paramNodes[19] as PBaseType;
                    PBaseType NetworkEmail = _paramNodes[20] as PBaseType;
                    PBaseType BankID = _paramNodes[21] as PBaseType;
                    PBaseType ViewAudits = _paramNodes[22] as PBaseType;
                    //
                    StringBuilder sSQL = new StringBuilder();
                    //
                    GetFromAdGbBankControl(dbHelper, PhoneMask, PhoneMaskLength, NoDaysFuture,
                NoDaysBack, DefaultTIN, FormatTIN, DefaultPhone, FormatPhone, DefaultZIP,
                FormatZIP, PhoenixJobCode, IntlCountryCode, AcctTypeSearch, DatabaseLanguageId,
                AchDRLeadDays, AchCRLeadDays, RequirePrenote, PrenoteLeadDays, CheckNSF,
                GenSecClsRel, NetworkEmail, BankID, ViewAudits);
                }
            }
            #endregion

            #region GenerateAccount
            if (this.CustomActionName == "GenerateAccount")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType ModStyle = _paramNodes[0] as PBaseType;
                    PBaseType DepLoan = _paramNodes[1] as PBaseType;
                    PBaseType AcctType = _paramNodes[2] as PBaseType;
                    PBaseType ApplType = _paramNodes[3] as PBaseType;
                    PBaseType AcctNo = _paramNodes[4] as PBaseType;
                    PBaseType NextAcct = _paramNodes[5] as PBaseType;

                    StringBuilder sSQL = new StringBuilder();
                    GenerateAccount(dbHelper, ModStyle, DepLoan, AcctType,
                        ApplType, AcctNo, NextAcct);
                }
            }
            #endregion

            #region GetFromGLControl
            if (this.CustomActionName == "GetFromGLControl") //Selva-New
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType RestrictGlAccess = _paramNodes[0] as PBaseType;
                    PBaseType GlAcctNoFormat = _paramNodes[1] as PBaseType;
                    StringBuilder sSQL = new StringBuilder();
                    GetFromGLControl(dbHelper, RestrictGlAccess, GlAcctNoFormat);
                }
            }
            #endregion

            //Begin #76458
            #region GetMaskedExtAcct
            if (this.CustomActionName == "GetMaskedExtAcct")
            {
                if (_paramNodes.Count > 0)
                {

                    PBaseType AccountType = _paramNodes[0] as PBaseType;
                    PBaseType AccountNumber = _paramNodes[1] as PBaseType;
                    string maskedAcct = null;
                    if (!GetMaskedExtAcct(dbHelper, AccountType.StringValue, AccountNumber.StringValue, ref maskedAcct))
                    {

                    }
                    else
                    {
                        (_paramNodes[2] as PBaseType).Value = maskedAcct;
                    }
                }
            }
            #endregion
            //End #76458

            //Begin #76036
            #region GetCurrRmAddrId
            if (this.CustomActionName == "GetCurrRmAddrId")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType rimNo = _paramNodes[0] as PBaseType;
                    PBaseType effdt = _paramNodes[1] as PBaseType;
                    PBaseType addrid = _paramNodes[2] as PBaseType;

                    string sSQL = "";
                    sSQL = string.Format(@"
        					Select 	max(addr_id)
		                	From	{0}RM_ADDRESS
			                Where	rim_no = {1}
			                And	    '{2}' between start_dt and end_dt
                            And		status = 'Active'", dbHelper.DbPrefix, rimNo.ValueObject, effdt.ValueObject);

                    ExecSqlImmediateInto(dbHelper, sSQL, addrid);
                }
            }
            #endregion
            //End #76036

            //Begin #76099
            #region GetCurrRmAddrId
            if (this.CustomActionName == "GetDBDateDiff")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType dtStartDt = _paramNodes[0] as PBaseType;
                    PBaseType dtEndDt = _paramNodes[1] as PBaseType;
                    PBaseType nDays = _paramNodes[2] as PBaseType;
                    PBaseType nMonths = _paramNodes[3] as PBaseType;
                    PBaseType nYears = _paramNodes[4] as PBaseType;

                    /*Get Days Diff*/
                    string sDateDiffSQL = string.Format(@"select abs(datediff(day, '" + dtStartDt.ValueObject + "', '" + dtEndDt.ValueObject + "'))");
                    ExecSqlImmediateInto(dbHelper, sDateDiffSQL, nDays);

                    /*Get Month Diff*/
                    sDateDiffSQL = string.Format(@"select abs(datediff(month, '" + dtStartDt.ValueObject + "', '" + dtEndDt.ValueObject + "'))");
                    ExecSqlImmediateInto(dbHelper, sDateDiffSQL, nMonths);

                    /*Get Year Diff*/
                    sDateDiffSQL = string.Format(@"select abs(datediff(year, '" + dtStartDt.ValueObject + "', '" + dtEndDt.ValueObject + "'))");
                    ExecSqlImmediateInto(dbHelper, sDateDiffSQL, nYears);
                }
            }
            #endregion
            //End #76099
                if ("GetLoanUndisbursedValues" == CustomActionName)
                {
                    GetLoanUndisbursedValues(dbHelper);
				}

            //Begin #04976
            #region GetWorkDetailsForNewAcct
            if (this.CustomActionName == "GetWorkDetailsForNewAcct")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType workId = _paramNodes[0] as PBaseType;
                    PBaseType empId = _paramNodes[1] as PBaseType;
                    PBaseType prodClassCode = _paramNodes[2] as PBaseType;
                    PBaseType reasonId = _paramNodes[3] as PBaseType;
                    PBaseType prodAcctType = _paramNodes[4] as PBaseType;

                    if (!workId.IsNull)
                    {
                        Phoenix.BusObj.Global.GbWorkQueue gbWorkQueue = new GbWorkQueue();

                        gbWorkQueue.SelectAllFields = true;
                        gbWorkQueue.ActionType = XmActionType.Select;

                        #region #04976
                        gbWorkQueue.WorkId.Value = Convert.ToDecimal(workId.ValueObject);
                        #endregion
                        gbWorkQueue.DoAction(dbHelper);

                        if (!gbWorkQueue.OwnerEmplId.IsNull)
                        {
                            empId.ValueObject = gbWorkQueue.OwnerEmplId.Value;
                        }
                        if (!gbWorkQueue.ClassCode.IsNull)
                        {
                            prodClassCode.ValueObject = gbWorkQueue.ClassCode.Value;
                        }
                        if (!gbWorkQueue.ReasonId.IsNull)
                        {
                            reasonId.ValueObject = gbWorkQueue.ReasonId.Value;
                        }
                        prodAcctType.ValueObject = (gbWorkQueue.AcctType.IsNull ? string.Empty : gbWorkQueue.AcctType.Value);
                    }
                }
            }
            #endregion

            #region Nickname Update
            if (this.CustomActionName == "NicknameUpdate")
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType gbMapAcctRel = _paramNodes[0] as PBaseType;
                    PBaseType xpRmSvcsAcct = _paramNodes[1] as PBaseType;
                    PBaseType acctType = _paramNodes[2] as PBaseType;
                    PBaseType acctNo = _paramNodes[3] as PBaseType;
                    PBaseType rimNo = _paramNodes[4] as PBaseType;
                    PBaseType nickname = _paramNodes[5] as PBaseType;
                    PBaseType returnNum = _paramNodes[6] as PBaseType;

                    #region Local Vars
                    string sql = string.Empty;
                    PInt nRc = new PInt("nRc");
                    PInt sqlError = new PInt("sqlError");
                    PString tablesToUpdate = new PString("TablesToUpdate");
                    #endregion

                    nickname.Value = Convert.ToString(nickname.Value).Replace("\"", "'");
                    nickname.Value = "\"" + nickname.Value + "\""; //8600
                    tablesToUpdate.Value = string.Empty;

                    #region update gb_map_acct_rel
                    if (Convert.ToBoolean(gbMapAcctRel.Value))
                    {
                        tablesToUpdate.Value += "GB_MAP_ACCT_REL";
                        //                            sql = string.Format(@"update	{0}gb_map_acct_rel
                        //                                                    set 	acct_nickname = '{1}'
                        //                                                    where   acct_no = '{2}'
                        //                                                    AND	    acct_type = '{3}'
                        //                                                    AND	    rim_no = {4}", dbHelper.DbPrefix,
                        //                                                    nickname.ValueObject, acctNo.ValueObject,
                        //                                                    acctType.ValueObject, rimNo.ValueObject);
                        //                            if(dbHelper.ExecuteNonQuery(sql)!= 1)
                        //                            {
                        //                                returnNum.ValueObject = 10696;
                        //                                return false;
                        // }

                    }
                    #endregion

                    #region Update xp_rm_svcs_acct


                    if (Convert.ToBoolean(xpRmSvcsAcct.Value))
                    {
                        tablesToUpdate.Value += "XP_RM_SVCS_ACCT";
                        //                                sql = string.Format(@"update	{0}xp_rm_svcs_acct
                        //                                                    Set 	acct_desc = '{1}',
                        //	                                                        last_sys_maint_dt = '{2}'
                        //                                                    Where   Acct_no = '{3}'
                        //                                                    AND	    Acct_type = '{4}'
                        //                                                    AND	    Rim_No = {5}", dbHelper.DbPrefix,
                        //                                                       nickname.ValueObject,
                        //                                                       DateHelper.FormatDate(Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate, false),
                        //                                                       acctNo.ValueObject, acctType.ValueObject, rimNo.ValueObject);
                        //                                if (dbHelper.ExecuteNonQuery(sql) != 1)
                        //                                {
                        //                                    returnNum.ValueObject = 10697;
                        //                                    return false ;
                        //                                }

                    }
                    #endregion

                    #region PSP_NICKNAME_UPDATE
                    if (true)
                    {

                        sql = string.Format(@"
Declare 	@nRC int, @nSQLError Int
Exec 	@nRC = {0}PSP_NICKNAME_UPDATE
    '{1}',
    '{2}',
    '{3}',
     {4},
     {5},
    '{6}',
    @nSQLError output,
    0

Select 	@nRC, isnull(@nSQLError , 0)",
                                    dbHelper.DbPrefix,
                                    DateHelper.FormatDate(Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate, false),
                                    acctType.ValueObject,
                                    acctNo.ValueObject,
                                    rimNo.ValueObject,
                                    nickname.Value,
                                    tablesToUpdate.ValueObject);






                        if (!ExecSqlImmediateInto(dbHelper, sql, true, false, nRc, sqlError))
                        {
                            returnNum.ValueObject = 10698;
                            return false;
                        }
                        else if (nRc.Value != 0 || sqlError.Value != 0)
                        {
                            returnNum.ValueObject = 10698;
                            return false;
                        }
                    }
                    #endregion
                    returnNum.ValueObject = 0;
                }
            }

            #endregion
            //End #04976

            #region #03333
            else if (CustomActionName == "LaunchInterface")
            {
                return LaunchInterface(dbHelper);
            }
            #endregion


            //Begin #06167
            if (this.CustomActionName == "GetDataSet")
            {
                GetDataSet(dbHelper);
            }
            //End #06167

            //Begin #140793
            if (this.CustomActionName == "GetAutoCompleteList")
            {
                this.GetAutoCompleteList(dbHelper);
            }
            if (this.CustomActionName == "GetAutoCompleteSelectedValue")
            {
                this.GetAutoCompleteSelectedValue(dbHelper);
            }
            //End #140793

            //Begin WI #27540
            if (this.CustomActionName == "GetPostEditAccess")
            {
                GetPostEditAccess(dbHelper);
            }
            //End WI #27540

            #region GetUSCountryCode
            /*Begin #32310*/
            string sTableName = string.Empty;

            if (this.CustomActionName == "GetUSCountryCode")
            {
                // Returns a valid US country code (has iso code = US in ad_gb_country)
                // Returns USA, if no active country codes
                if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
                    sTableName = string.Format(@" {0}..X_AD_GB_COUNTRY", dbHelper.XmDbName);
                else
                    sTableName = string.Format(@" {0}..AD_GB_COUNTRY", dbHelper.PhoenixDbName);

                PString UsCountryCode = new PString("UsCountryCode");

                ExecSqlImmediateInto(CoreService.DbHelper, string.Format(@" 
                        select top 1 country_code 
                        from {0}
                        where iso_code = 'US' 
                        and status = 'Active'", sTableName), UsCountryCode);

                if (UsCountryCode.IsNull)
                {
                    UsCountryCode.Value = "USA";    /* default to USA, if no valid code found */
                }
                (_paramNodes[0] as PBaseType).ValueObject = UsCountryCode.Value;
            }
            #endregion

            #region Country Codes
            /*Begin #32310 */
            if (this.CustomActionName == "IsUSCountryCode")
            {
                // Returns true if the given country code has iso code = US in ad_gb_country
                if (_paramNodes.Count < 1)	// no parameters so get out
                {
                    return false;
                }

                PBaseType CountryCode = _paramNodes[0] as PBaseType;
                PString sIsoCode = new PString("sIsoCode");

                if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
                    sTableName = string.Format(@" {0}..X_AD_GB_COUNTRY", dbHelper.XmDbName);
                else
                    sTableName = string.Format(@" {0}..AD_GB_COUNTRY", dbHelper.PhoenixDbName);

                ExecSqlImmediateInto(CoreService.DbHelper,
                    string.Format(@"
									Select
										iso_code
									From
										{0}
									Where
										country_code = '{1}'", sTableName, CountryCode.StringValue), sIsoCode);

                if (sIsoCode.Value == "US")
                {
                    (_paramNodes[1] as PBaseType).ValueObject = true;
                }
                else
                {
                    (_paramNodes[1] as PBaseType).ValueObject = false;
                }
            }
            #endregion

            /*Begin #46760 */
            if (this.CustomActionName == "IsNachaHoliday")
            {
                IsNachaHoliday(dbHelper);
            }
            /*End #46760 */



            #region Get GetBiProductType
            if (this.CustomActionName == "GetBiProductType")    //#63936
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType BiProduct= _paramNodes[0] as PBaseType;
                    GetBiProductType(dbHelper, BiProduct);
                }
            }
            if (this.CustomActionName == "CustAnalysisExists")    //#63936
            {
                    CustAnalysisExists(dbHelper);
            }
            #endregion

            /*Begin #65549*/
            if (this.CustomActionName == "GetDecryptedPan")
            {
                GetDecryptedPan(dbHelper);
            }
            /*End #65549 */ 
            /*Begin #71500*/
            if (this.CustomActionName == "GetDecryptedAcctNo")
            {
                GetDecryptedAcctNo(dbHelper);
            }
            /*End #71500 */
            // Begin #94549
            #region GetCurPostingDate

            if (this.CustomActionName == "GetCurPostingDate")    //#63936
            {
                if (_paramNodes.Count > 0)
                {
                    PBaseType curPostDate = _paramNodes[0] as PBaseType;
                    PBaseType branchNo = _paramNodes[1] as PBaseType;
                    PBaseType drawerNo = _paramNodes[2] as PBaseType;
                    GetCurPostingDate(dbHelper, curPostDate, branchNo, drawerNo);
                }
            }
            #endregion
            // End #94549
            return true;
        }

        // Begin - Enh#173485
        private void GetNameDetails(
            IDbHelper dbHelper,
            PBaseType rimNo,
            PBaseType firstName,
            PBaseType middleInitial,
            PBaseType lastName,
            PBaseType rimType,
            PBaseType title,
            PBaseType suffix,
            PBaseType fullname)
        {
            string sSQL =
                string.Format(
                    @"  Select	R.FIRST_NAME, 	R.MIDDLE_INITIAL,
						    	R.LAST_NAME,	R.RIM_TYPE,
							    T.TITLE,		R.SUFFIX
					    From	{0}RM_ACCT R, {0}AD_RM_TITLE T
					    Where	R.RIM_NO = {1}
					    And 	R.TITLE_ID  *=  T.TITLE_ID",
                    dbHelper.DbPrefix,
                    rimNo.ValueObject);

            ExecSqlImmediateInto(dbHelper, sSQL, firstName, middleInitial, lastName, rimType, title, suffix);

            //Begin Enh# 69248
            fullname.ValueObject =
                this.ConcateNameX(
                    firstName.IsNull ? string.Empty : firstName.StringValue,
                    lastName.IsNull ? string.Empty : lastName.StringValue,
                    middleInitial.IsNull ? string.Empty : middleInitial.StringValue,
                    false);

            if ((rimType.IsNull ? string.Empty : rimType.StringValue.Trim()) == "Personal")
            {
                if (!suffix.IsNull)
                    fullname.ValueObject =
                        fullname.StringValue + " " + (suffix.IsNull ? string.Empty : suffix.StringValue);

                if (!title.IsNull)
                    fullname.ValueObject =
                        (title.IsNull ? string.Empty : title.StringValue) + " " + fullname.StringValue;
            }
            //End Enh# 69248
        }

        /*Begin -198018*/
        /// <summary>
        /// For getting the name of unrelared user.
        /// </summary>
        /// <param name="dbHelper"></param>
        public void GetUnRelatedName(IDbHelper dbHelper)
        {
            PString unRelName = new PString("unRelName");
            PString sSQL = new PString("sSQL");
            PInt unRelRimNo = new PInt("unRelRimNo");
            unRelRimNo.Value = Convert.ToInt32((_paramNodes[0] as PBaseType).ValueObject);
            sSQL.Value = string.Format(@"SELECT NAME_1 
                                         FROM   {0}RM_ACCT_UNREL 
                                         WHERE  UNREL_RIM_NO={1}", dbHelper.DbPrefix, unRelRimNo.Value);

            ExecSqlImmediateInto(dbHelper, sSQL.Value, unRelName);
            (ParamNodes[1] as PBaseType).ValueObject = unRelName.Value;
        }
        /*End -198018*/

        // This hides GetNameDetails(int pnRimNo, string mode) in the client and makes sense since
        //  we want to avoid CDS round trips if we're already on the server. 
        public new string GetNameDetails(int pnRimNo, string mode)
        {
            PInt rimNo = new PInt("rimNo", pnRimNo);
            PString firstName = new PString();
            PString middleInitial = new PString();
            PString lastName = new PString();
            PString rimType = new PString();
            PString title = new PString();
            PString suffix = new PString();
            PString fullName = new PString();

            GetNameDetails(
                CoreService.DbHelper,
                rimNo,
                firstName,
                middleInitial,
                lastName,
                rimType,
                title,
                suffix,
                fullName);

            return fullName.StringValue;
        }
        // End - Enh#173485

        //Begin #06167
        private void GetDataSet(IDbHelper dbHelper)
        {
            if (_paramNodes.Count >= 4)
            {
                PBaseType Sql = _paramNodes[0] as PBaseType;
                PBaseType ForcePrimeDb = _paramNodes[1] as PBaseType;
                PBaseType Data = _paramNodes[2] as PBaseType;
                PBaseType Schema = _paramNodes[3] as PBaseType;

                Sql.Value = _sqlHelper.SubstituteQueryDbPrefix(dbHelper, Sql.StringValue, ForcePrimeDb.BooleanValue);
                DataSet dataSet = _sqlHelper.GetDataSet(dbHelper, Sql.StringValue, ForcePrimeDb.BooleanValue);

                if (dataSet != null)
                {
                    Data.Value = dataSet.GetXml();
                    Schema.Value = dataSet.GetXmlSchema();
                }
            }
        }
        //End #06167

		#endregion

		#region Helper methods
		private string LoadAcctTypeDetails( IDbHelper dbHelper )
		{
			PString AcctTypeDetail = new PString("A1");
			System.Text.StringBuilder acctTypeDetails = new System.Text.StringBuilder();
			bool addCaret = false;
			#region sql
			//#72644 - added commitment account type details
			string sql = string.Format(@"
						SELECT	rtrim(A.ACCT_TYPE)+'~'+rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'

						UNION

						SELECT	rtrim(A.ACCT_TYPE)+'~'+rtrim(B.APPL_TYPE)+'~'+'{2}'+'~'+rtrim(B.ACCT_NO_FORMAT)
						FROM	{0}AD_SD_ACCT_TYPE A,
								{0}AD_SD_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}' " + /*#9888*/ (GlobalVars.Module == "HoldCo" ? "" : @"

						UNION

						SELECT	'{3}'+'~'+'{3}'+'~'+'{3}'+'~'+GL_ACCT_NO_FORMAT
						FROM XP_CONTROL

						UNION

						SELECT	'{4}'+'~'+'{5}'+'~'+'{5}'+'~'+null
						FROM XP_CONTROL
						") + ( dbHelper.IsOfflineDb ? "" : @"

						UNION

						SELECT	rtrim(ACCT_TYPE)+'~'+'{6}'+'~'+'{6}'+'~'+rtrim(ACCT_NO_FORMAT)
						FROM {0}AD_EX_ACCT_TYPE
						WHERE STATUS = '{1}'

						UNION

						SELECT	rtrim(ATM_ACCT_TYPE)+'~'+'{7}'+'~'+'{7}'+'~'+replicate('9', ATM_ACCT_NO_LENGTH )
						FROM {0}AD_ATM_ACCT_TYPE
						WHERE {8} = '{1}'

						UNION

						SELECT	rtrim(A.ACCT_TYPE)+'~'+rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_CM_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'" ), InquireDbPrefix( dbHelper ) , GlobalVars.Instance.ML.Active,
				GlobalVars.Instance.ML.SD, GlobalVars.Instance.ML.GL, GlobalVars.Instance.ML.RIM,
				GlobalVars.Instance.ML.RM, "EX", GlobalVars.Instance.ML.Atm,
				(InquireDbPrefix( dbHelper ) == "X_" ? "'" + GlobalVars.Instance.ML.Active + "'" : "STATUS" ));
			#endregion

			if ( ExecSqlImmediateInto( dbHelper, sql, false, true, AcctTypeDetail ))
			{
				do
				{
					acctTypeDetails.Append((addCaret?"^":"") + AcctTypeDetail.Value );
					addCaret = true;
				}
				while (ExecSqlGetNextRow( ));
			}

			return acctTypeDetails.ToString();
		}

        private bool GetAcctAlerts(IDbHelper dbHelper)
        {
            string acctType = null;
            string acctNo = null;
			PString depLoan = new PString("DepLoan");	// 22968 (2) 
            int rimNo = -1; //#80679-2
            string sql = null;
            PString Stops = new PString("A1");
            PString Ira = new PString("A2");
            PString Cautions = new PString("A3");
            PString Analysis = new PString("A4");
            PInt RimNo = new PInt("A5");
            PInt Alerts = new PInt("Alerts");
            PDecimal HoldBal = new PDecimal("A6");
            PString Fraud = new PString("A13");  /*#80660*/
            int index = 0;
            PString PlanNo = new PString("A14");    //#80679-2
            PString Nsf = new PString("A15");    //#80679-2
            PString Ucf = new PString("A16");    //#80679-2
            PString Rejected = new PString("A17");    //#80679-2
            PDecimal SweepControlPtid = new PDecimal("A18");    //#80679-2
            PString HouseHold = new PString("A19");    //#80679-2
            PString AcctCrossRef = new PString("A20");    //#80679-2
            PString CustCrossRef = new PString("A21");    //#80679-2
            PInt CustNotesCount = new PInt("A22");   //#80679-2
            PInt AcctNotesCount = new PInt("A23");  //#80679-2
            PInt ApplNotesCount = new PInt("A23");  //#80679-2
            PString AcctRegD = new PString("A24");      //140780
            PString CustRegD = new PString("A25");      //140780
            PString AcctBankrupt = new PString("A26");      //140775
            PString CustBankrupt = new PString("A27");      //140775
            PString CustRelPkg = new PString("A28");      //140769
			PString AdvRestrict = new PString("A29");	// 140796
            string moduleName = null;                    //WI#20348
            AcctTypeDetail acctTypeDetail;              //22968

            int showAlerts = 1; //#80679-2

            /*Begin - Task #124261 */
            PString sAcctType = new PString();
            PString sAcctNo = new PString();
            PString sModuleName = new PString();
            /*End - Task #124261*/

            if (dbHelper.IsOfflineDb)
                return true;

            #region Task #124261 - Validate for SQL Injection

            foreach (PBaseType param in _paramNodes)
            {
                if (param.IsNull)
                    continue;

                param.Length = (short)param.StringValue.Length;

                if (!param.ValidateDataForSQLInjection())
                {
                    /*SQL injection is an exception.*/
                    //300099 - Value passed for %1!  is not valid due to presence of certain keywords: "exec", "execute", "drop", "delete", "update", "insert", "select", "grant","create", "alter"
                    throw CoreService.ExceptionMgr.NewException(null, 300099, param.XmlTag);

                }

            }

            #endregion

            #region input parameters
            if (_paramNodes.Count >= 4)   //#80679-2
            {
                acctType = Convert.ToString((_paramNodes[index++] as PBaseType).ValueObject);
                acctNo = Convert.ToString((_paramNodes[index++] as PBaseType).ValueObject);
				depLoan.Value = Convert.ToString((_paramNodes[index++] as PBaseType).ValueObject);	// 22968 (2)
                rimNo = Convert.ToInt32((_paramNodes[index++] as PBaseType).ValueObject);
                moduleName = Convert.ToString((_paramNodes[index++] as PBaseType).ValueObject);  // WI#20348

                /*Begin - Task #124261*/
                sAcctType.Value = StringHelper.StrTrimX(acctType);
                sAcctNo.Value = StringHelper.StrTrimX(acctNo);
                sModuleName.Value = StringHelper.StrTrimX(moduleName);
                /*End - Task #124261*/
            }
            #endregion

            //MBAchala-22968
            if (!string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo))
            {
                acctTypeDetail = GetAcctTypeDetails(acctType, null);
                if (acctTypeDetail == null)
                {
                    return false;
                }
                rimNo = GetRimNo(acctType, acctNo);
				depLoan.Value = acctTypeDetail.DepLoan;	// 22968 (2)
            }
            //MBachala-22968
            #region PSP_ON_ALERTS

            showAlerts = 1; //Show Account Alerts
            if (!string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo) && rimNo > 0)  //Show Account and Customer Alerts
                showAlerts = 3;
            else if (rimNo > 0) //Show Customer Alerts
                showAlerts = 2;

            // Begin WI#20348 - Need ModuleName name and default params before it
            //sql = string.Format(@"
            //Exec {0}PSP_ON_ALERTS
            //    '" + acctType + @"',
            //    '" + acctNo + @"',
            //    '" + depLoan + @"',
            //    " + NumberHelper.NumberToStringX(rimNo, 0) + @",
            //    " + NumberHelper.NumberToStringX(showAlerts, 0), CoreService.DbHelper.PhoenixDbName + "..");
            // modify depLoan 22968 (2)

            /*Task #124261 - Using SqlString rather than string vars to escape quotes in case of SQL injection attempt */
            sql = string.Format(@"
	        Exec {0}PSP_ON_ALERTS
		        " + sAcctType.SqlString + @",
                " + sAcctNo.SqlString + @",
                " + depLoan.SqlString + @",
		        " + NumberHelper.NumberToStringX(rimNo, 0) + @",
                " + NumberHelper.NumberToStringX(showAlerts, 0) + @",0 ,0 ,0 , " + sModuleName.SqlString,
                  CoreService.DbHelper.PhoenixDbName + "..");


            //140780 RegD, 140775 Bankrupt, 140769 Rel Pricing, 140788 Alert
            if (!ExecSqlImmediateInto(CoreService.DbHelper, sql, true, false, RimNo,
                    Alerts,
                    CustCrossRef,
                    Fraud,
                    CustNotesCount,
                    HouseHold,
                    Stops,
                    HoldBal,
                    PlanNo,
                    Cautions,
                    Analysis,
                    AcctCrossRef,
                    Nsf,
                    Ucf,
                    Rejected,
                    SweepControlPtid,
                    AcctNotesCount,
                    ApplNotesCount,
                    Ira,
                    AcctRegD,
                    CustRegD,
                    AcctBankrupt,
                    CustBankrupt,
                    CustRelPkg,
					AdvRestrict
                    ))
            {
                return false;
            }
            #endregion

            #region output parameters

            (_paramNodes[index++] as PBaseType).ValueObject = Cautions.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = !HoldBal.IsNull && HoldBal.Value != 0;
            (_paramNodes[index++] as PBaseType).ValueObject = Nsf.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = Rejected.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = Ucf.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = Stops.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = Ira.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = !SweepControlPtid.IsNull && SweepControlPtid.Value > 0;
            (_paramNodes[index++] as PBaseType).ValueObject = Analysis.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = HouseHold.Value == GlobalVars.Instance.ML.Y;
            (_paramNodes[index++] as PBaseType).ValueObject = RimNo.Value;
            (_paramNodes[index++] as PBaseType).ValueObject = AcctCrossRef.Value == GlobalVars.Instance.ML.Y;  /*#76429*/
            (_paramNodes[index++] as PBaseType).ValueObject = Fraud.Value == GlobalVars.Instance.ML.Y;  /*#80660*/
            (_paramNodes[index++] as PBaseType).ValueObject = PlanNo.Value;     //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = SweepControlPtid.Value;    //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = CustCrossRef.Value == GlobalVars.Instance.ML.Y;    //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = CustNotesCount.Value; //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = AcctNotesCount.Value; //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = ApplNotesCount.Value; //#80679-2
            (_paramNodes[index++] as PBaseType).ValueObject = (AcctRegD.Value == "Y");       //140780
            (_paramNodes[index++] as PBaseType).ValueObject = (CustRegD.Value == "Y");       //140780
            (_paramNodes[index++] as PBaseType).ValueObject = (AcctBankrupt.Value == "Y");      //140775
            (_paramNodes[index++] as PBaseType).ValueObject = (CustBankrupt.Value == "Y");      //140775
            (_paramNodes[index++] as PBaseType).ValueObject = (CustRelPkg.Value == "Y");		//140769
			(_paramNodes[index++] as PBaseType).ValueObject = depLoan.Value;					// 22968 (2)
			(_paramNodes[index++] as PBaseType).ValueObject = (AdvRestrict.Value == "Y");		//140796

            #endregion

            return true;
        }

		//Begin Enh# 69248
		private string GetDepLoan(IDbHelper dbHelper, string psValue, string psMode)
		{
            if (psValue.Trim() == "")
            {
                return string.Empty;
            }
            else /*Begin #15978*/
            {
                if (psValue.Trim().ToUpper() == "RIM")
                {
                    return "RM";
                }
            } /*End #15978*/

			string sDepLoan = string.Empty;
			StringBuilder sSQL = new StringBuilder();
			string sTablename = string.Empty;
			PString DepLoan = new PString("A0");

            /*Begin -  Task #124261*/
            PString sValue = new PString();
            sValue.Value = psValue.Trim();
            /*End -  Task #124261*/

            if (dbHelper.CopyStatus != "D")
				sTablename = dbHelper.XmDbName + "..X_AD_GB_ACCT_TYPE a";
			else
				sTablename = dbHelper.PhoenixDbName + "..AD_GB_ACCT_TYPE a";

			sSQL.AppendFormat(@"
							Select
								ltrim(rtrim(a.DEP_LOAN))
							From
								{0}
							Where ", sTablename);

            /* Task #124261 - Replaced psValue with sValue.SqlString to escape quotes in case of SQL injection attempt*/
            if (psMode.Trim() == "ACCTTYPE")
                sSQL.AppendFormat(@"
									ACCT_TYPE = {0}", sValue.SqlString);     /* Task #124261*/
            else    //APPLTYPE
                sSQL.AppendFormat(@"
									APPL_TYPE = {0}", sValue.SqlString);    /* Task #124261*/

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), DepLoan);

            #region #03830 - Old Code
            	//DepLoan not found in AD_GB_ACCT_TYPE so try in AD_SD_ACCT_TYPE
//            if (DepLoan.IsNull)
//            {
//                sTablename = string.Empty;
//                sSQL.Remove(0, sSQL.Length);
//                PInt nCount = new PInt("A0");

//                if (dbHelper.CopyStatus != "D")
//                    sTablename = dbHelper.XmDbName + "..X_AD_SD_ACCT_TYPE a";
//                else
//                    sTablename = dbHelper.PhoenixDbName + "..AD_SD_ACCT_TYPE a";

//                if (psMode.Trim() == "ACCTTYPE")
//                    sSQL.AppendFormat(@"
//							Select	count(*)
//							From	{0}
//							Where	ACCT_TYPE = '{1}'", sTablename, psValue.Trim());
//                else	//APPLTYPE
//                    sSQL.AppendFormat(@"
//							Select	count(*)
//							From	{0}
//							Where	APPL_TYPE = '{1}'", sTablename, psValue.Trim());

//                ExecSqlImmediateInto(dbHelper, sSQL.ToString(), nCount);

//                //DepLoan not found in AD_SD_ACCT_TYPE so try in AD_EX_ACCT_TYPE
//                if (nCount.IsNull)
//                {
//                    sDepLoan = "EXT";
//                }
//                else
//                {
//                    if (nCount.Value == 0)
//                        sDepLoan = "EXT";
//                    else
//                        sDepLoan = "SD";
//                }
            #endregion
			//DepLoan not found in AD_GB_ACCT_TYPE so try in AD_SD_ACCT_TYPE
            #region #03830 - Add CMT handling
            if (DepLoan.IsNull)
            {
                sTablename = string.Empty;
                sSQL.Remove(0, sSQL.Length);
                PInt nCount = new PInt("A0");

                if (dbHelper.CopyStatus != "D")
                    sTablename = dbHelper.XmDbName + "..X_AD_CM_ACCT_TYPE a";
                else
                    sTablename = dbHelper.PhoenixDbName + "..AD_CM_ACCT_TYPE a";

                /*Task #124261 - Replaced psValue with sValue.SqlString to escape quotes in case of SQL injection attempt*/
                if (psMode.Trim() == "ACCTTYPE")
                    sSQL.AppendFormat(@"
							Select	count(*)
							From	{0}
							Where	ACCT_TYPE = {1}", sTablename, sValue.SqlString);    /*Task #124261*/
                else	//APPLTYPE
                    sSQL.AppendFormat(@"
							Select	count(*)
							From	{0}
							Where	APPL_TYPE = {1}", sTablename, sValue.SqlString);    /*Task #124261*/

                ExecSqlImmediateInto(dbHelper, sSQL.ToString(), nCount);

                if (!nCount.IsNull && nCount.Value != 0)
                {
                    sDepLoan = "CM";
                }

                else
                {
                    nCount.SetValue(null); // Safty
                    sTablename = string.Empty;
                    sSQL.Remove(0, sSQL.Length);
                    //PInt nCount = new PInt("A0");

                    if (dbHelper.CopyStatus != "D")
                        sTablename = dbHelper.XmDbName + "..X_AD_SD_ACCT_TYPE a";
                    else
                        sTablename = dbHelper.PhoenixDbName + "..AD_SD_ACCT_TYPE a";

                    /* Task #12426 - Replaced psValue with sValue.SqlString to escape quotes in case of SQL injection attempt*/
                    if (psMode.Trim() == "ACCTTYPE")
                        sSQL.AppendFormat(@"
							Select	count(*)
							From	{0}
							Where	ACCT_TYPE = {1}", sTablename, sValue.SqlString);    /* Task #12426 */
                    else	//APPLTYPE
                        sSQL.AppendFormat(@"
							Select	count(*)
							From	{0}
							Where	APPL_TYPE = {1}", sTablename, sValue.SqlString);    /* Task #12426 */


                    ExecSqlImmediateInto(dbHelper, sSQL.ToString(), nCount);

                    //DepLoan not found in AD_SD_ACCT_TYPE so try in AD_EX_ACCT_TYPE
                    if (nCount.IsNull)
                    {
                        sDepLoan = "EXT";
                    }
                    else
                    {
                        if (nCount.Value == 0 && !psValue.Trim().Equals("SK")) /*WI# 29458*/
                            sDepLoan = "EXT";
                        else
                            sDepLoan = "SD";
                    }

                }

            }
            #endregion
            else
			{
				sDepLoan = DepLoan.StringValue;
			}

			return sDepLoan.Trim().ToUpper();
		}
        public int GetRimNo(IDbHelper dbHelper, string psAcctType, string psAcctNo) // #140789 - made public
		{
            /*Begin 15978*/
            if (psAcctType.ToUpper() == "RIM")
            {
                /*If the AcctType = RIM then it is assumed that the AcctNo sent in will be the Rim Number*/
                return Convert.ToInt32(psAcctNo);
            }
            /*End 15978*/

            /*Begin -  Task #12426*/
            PString sAcctType = new PString();
            PString sAcctNo = new PString();

            sAcctType.Value = StringHelper.StrTrimX(psAcctType);
            sAcctNo.Value = StringHelper.StrTrimX(psAcctNo);
            /*End -  Task #12426*/

            string sDepLoan = GetDepLoan(dbHelper, psAcctType.Trim(), "ACCTTYPE").Trim();

			if (sDepLoan != "")
			{
				StringBuilder sSQL = new StringBuilder();
				string sTablename = string.Empty;
				PString RimNo = new PString("A0");

				if (dbHelper.CopyStatus != "D")
				{
					if (sDepLoan == "DP")
						sTablename = dbHelper.XmDbName + "..X_DP_DISPLAY a";
					else if (sDepLoan == "LN")
						sTablename = dbHelper.XmDbName + "..X_LN_DISPLAY a";
					else if (sDepLoan == "SD")
						sTablename = dbHelper.XmDbName + "..X_SD_ACCT a";
                    #region #03830
                    else if (sDepLoan == "CM")
                        sTablename = dbHelper.XmDbName + "..X_LN_UMB a";
                    #endregion
                    else
						sTablename = dbHelper.XmDbName + "..X_EX_ACCT a";
				}
				else
				{
					if (sDepLoan == "DP")
						sTablename = dbHelper.PhoenixDbName + "..DP_DISPLAY a";
					else if (sDepLoan == "LN")
						sTablename = dbHelper.PhoenixDbName + "..LN_DISPLAY a";
					else if (sDepLoan == "SD")
						sTablename = dbHelper.PhoenixDbName + "..SD_ACCT a";
                    #region #03830
                    else if (sDepLoan == "CM")
                        sTablename = dbHelper.PhoenixDbName + "..LN_UMB a";
                    #endregion
					else
						sTablename = dbHelper.PhoenixDbName + "..EX_ACCT a";
				}

                /* Task #12426 - Using SqlString to escape quotes in case of SQL injection attempt*/
                sSQL.AppendFormat(@"
							Select
								a.RIM_NO
							From
								{0}
							Where
								acct_type = {1}
							and
								acct_no = {2}", sTablename, sAcctType.SqlString, sAcctNo.SqlString);    /* Task #12426*/


                #region #02829
                if (sDepLoan == "SD")
                {
                    sSQL.AppendFormat(" order by ptid desc");
                }
                #endregion

				ExecSqlImmediateInto(dbHelper, sSQL.ToString(), RimNo);

				return (RimNo.IsNull?int.MinValue:RimNo.IntValue);
			}

			return int.MinValue;
		}
		//End Enh# 69248

		#region Get TranSet Id
		/// <summary>
		/// Gets the next tran set id for transaction posting.
		/// </summary>
		/// <param name="dbHelper">IDbHelper object.</param>
		/// <returns>integer tran_set_id value</returns>
		public int GetTranSetId(IDbHelper dbHelper)
		{
			int tranSetId = -1;
			int nSqlError = 0;
			string sSQL = "";
			System.Data.IDataReader tempReader = null;
			try
			{
				sSQL = string.Format(CultureInfo.InvariantCulture,"Declare @rnSQLError int, @nTranSetId int\n" +
					"exec " + dbHelper.DbPrefix + "psp_get_tran_set_id @nTranSetId output , '"+BusGlobalVars.SystemDate.ToShortDateString()+ "',"+ BusGlobalVars.CurrentBranchNo.ToString() + "\n"+
					"Select @rnSQLError, @nTranSetId");

				tempReader = dbHelper.ExecuteReader(sSQL);
				while(tempReader.Read())
				{
					if(tempReader.FieldCount == 2)
					{
						if(tempReader.IsDBNull(0))
							nSqlError = 0;
						else
							nSqlError = tempReader.GetInt32(0);
						if(nSqlError == 0)
						{
							if(!tempReader.IsDBNull(1))
								tranSetId = tempReader.GetInt32(1);
						}
					}
				}
				//tempReader.Close();
			}
			catch
			{
				//System.Diagnostics.Trace.WriteLine("Failed to create tran set id.");
				CoreService.ExceptionMgr.NewException(1, 1, "Failed to create tran set id.");
			}
			finally
			{
				if(tempReader != null)
					tempReader.Close();
			}

			return tranSetId;
		}
		#endregion

		#region Get Customer Name
		/// <summary>
		/// Gets the full name of the customer by combining first_name, last_name and middle initial,
		/// title and suffix.
		/// </summary>
		/// <param name="rimNo">Pass the rim number of the customer for whom you want to fetch the title info</param>
		/// <param name="dbHelper">IDbHelper object</param>
		/// <param name="rsFirstName">First name of the customer</param>
		/// <param name="rsLastName">Last name of the customer</param>
		/// <param name="rsMiddleInitial">Customer middle initial</param>
		/// <param name="rsPhone1">Customer phone1 information</param>
		/// <param name="rsRimType">Customer RIM type</param>
		/// <param name="rsSuffix">Customer Suffix information</param>
		/// <param name="rsTitle">Customer Title</param>
		public void GetRimTitleInfo(int rimNo, IDbHelper dbHelper, out string rsFirstName, out string rsLastName,
			out string rsMiddleInitial, out string rsRimType, out string rsTitle, out string rsSuffix, out string rsPhone1)
		{
			string sSQL = "";
			System.Data.IDataReader tempReader = null;

			rsFirstName = "";
			rsLastName = "";
			rsMiddleInitial = "";
			rsPhone1 = "";
			rsRimType = "";
			rsSuffix = "";
			rsTitle = "";

			#region Rim Name and Title
			//Get Rim Name and Title
			sSQL = string.Format( CultureInfo.InvariantCulture,
				"select a.first_name, a.middle_initial, a.last_name,\n" +
				"a.rim_type, b.title, a.suffix, c.phone_1\n" +
				"from " + dbHelper.DbPrefix + "rm_acct a, " + dbHelper.DbPrefix + "ad_rm_title b, " + dbHelper.DbPrefix + "rm_address c\n" +
				"where a.rim_no = " + Convert.ToString(rimNo)+ " And \n" +
				"a.rim_no = c.rim_no And \n" +
				"c.addr_id = 1 And \n" +
				"a.title_id  *= b.title_id");

			try
			{
				tempReader = dbHelper.ExecuteReader(sSQL );
				while ( tempReader.Read())
				{
					if( tempReader.FieldCount == 7 )
					{
						if(!tempReader.IsDBNull(0))
							rsFirstName = tempReader.GetString(0);
						if(!tempReader.IsDBNull(1))
							rsMiddleInitial = tempReader.GetString(1);
						if(!tempReader.IsDBNull(2))
							rsLastName = tempReader.GetString(2);
						if(!tempReader.IsDBNull(3))
							rsRimType = tempReader.GetString(3);
						if(!tempReader.IsDBNull(4))
							rsTitle = tempReader.GetString(4);
						if(!tempReader.IsDBNull(5))
							rsSuffix = tempReader.GetString(5);
						if(!tempReader.IsDBNull(6))
							rsPhone1 = tempReader.GetString(6);
					}
				}
				//tempReader.Close();
			}
			finally
			{
				if( tempReader != null )
					tempReader.Close();
			}
			#endregion
		}

		/// <summary>
		/// Gets the full name of the customer by combining first_name, last_name and middle initial,
		/// title and suffix.
		/// </summary>
		/// <param name="rimNo">Pass the rim number of the customer for whom you want to fetch the title info</param>
		/// <param name="dbHelper">IDbHelper object</param>
		/// <param name="rsName">output parameter returns customer name</param>
		/// <param name="rsphone1">output parameter returns customer phone</param>
		public void ConcateNameX(int rimNo, IDbHelper dbHelper, out string rsName, out string rsphone1)
		{
			#region string output
			string firstName;
			string lastName;
			string middleInitial;
			string rimType;
			string title;
			string suffix;
			#endregion

			GetRimTitleInfo(rimNo, dbHelper, out firstName, out lastName, out middleInitial,
				out rimType, out title, out suffix, out rsphone1);

			if(rimType.Trim() == CoreService.Translation.GetListItemX(ListId.PersonalNonPersonal, "NonPersonal"))
				rsName = ConcateNameX(firstName,lastName,middleInitial,true,suffix,title);
			else
				rsName = ConcateNameX(firstName,lastName,middleInitial,false,suffix,title);
		}
		#endregion

		#region FormatAccount - Moved to client bus obj.
		/// <summary>
		/// Format the account number based on format Mask
		/// </summary>
		/// <param name="acctNo"></param>
		/// <param name="acctNoFormat"></param>
		/// <returns></returns>

        //public string FormatAccount(string acctNo, string acctNoFormat)
        //{

        //    if( acctNo == null )
        //        return null;

        //    StringBuilder formattedAccount = new StringBuilder(acctNoFormat);
        //    int inputIndex = acctNo.Length -1;
        //    int fmtIndex = acctNoFormat.Length - 1;
        //    //int newLocation = 60;

        //    for( ; fmtIndex > -1;  fmtIndex--)
        //    {
        //        char fmtMask = acctNoFormat[ fmtIndex ];
        //        //newLocation--;
        //        if( fmtMask == '9' )
        //        {
        //            bool foundValue = false;
        //            for( ; inputIndex > -1 && foundValue == false ; inputIndex--)
        //            {
        //                char acctValue = acctNo[ inputIndex ];
        //                if( char.IsNumber( acctValue ))
        //                {
        //                    foundValue = true;
        //                    formattedAccount[fmtIndex] = acctValue;
        //                }

        //            }
        //            if( !foundValue )
        //                formattedAccount[fmtIndex] = '0';

        //        }
        //        //else
        //        //	formattedAccount[newLocation] = fmtMask ;

        //    }
        //    return formattedAccount.ToString();

        //}
		#endregion

		#region MaskTheAccount
		/// <summary>
		/// Based on the Acct Type or Application Type returns the masked account
		/// </summary>
		/// <param name="applType"></param>
		/// <param name="acctType"></param>
		/// <param name="unMaskedAcctNo"></param>
		/// <returns></returns>
		public string MaskTheAccount( string applType, string acctType, string  unMaskedAcctNo)
		{
			///
			XmAcctFormat acctFormat = null;
			if( acctType != null )
				acctFormat =  XmApplicationCollecion.CurrentApplication.AcctFormats[ acctType ];
			else if( applType != null )
				XmApplicationCollecion.CurrentApplication.AcctFormats.GetAcctFormatByApplType(applType);
			if( acctFormat == null )
				throw new Exception( "Invalid Account Type " );

			return acctFormat.FormatAccount( unMaskedAcctNo );

		}
		#endregion

		#region GetSPMessageText
		public string GetSPErrorMessageText( IDbHelper dbHelper, int errorId )
		{
			return GetSPErrorMessageText(dbHelper, errorId, true);
		}

		public string GetSPErrorMessageText( IDbHelper dbHelper, int errorId, bool bAppendNo )
		{
			string errorText = String.Empty;
			//Added 71170 - For Postive Numbers
			if (errorId > 0)
			{
				errorText = GetPhoenixXMMessageText(errorId, bAppendNo );
				if (errorText != string.Empty)
					return errorText;
			}
			Phoenix.BusObj.Control.PcOvError PcOvError = new Phoenix.BusObj.Control.PcOvError();

			PcOvError.ErrorId.Value = errorId;
			PcOvError.ActionType = XmActionType.Select;
			PcOvError.DoAction(dbHelper);
			if ( PcOvError.ErrorText.IsNull || PcOvError.ErrorText.Value == String.Empty )
				errorText = CoreService.Translation.GetUserMessageX(360087);
			else
				errorText = PcOvError.ErrorText.Value;
			if ( bAppendNo )
				return errorId.ToString() + ":" + errorText;
			else
				return errorText;
		}
		#region GetPhoenixXMMessageText
        ////Until 80 we have this method
        //public string GetPhoenixXMMessageText(int errorId, bool bAppendNo )
        //{
        //    string errorText = String.Empty;
        //    if (errorId == 1002)
        //        errorText = "Employee information Needed";
        //    else if (errorId == 1003)
        //        errorText = "User Information Needed";
        //    else if (errorId == 1006)
        //        errorText = "Override is Not Allowed";
        //    else if (errorId == 1009)
        //        errorText = "Invalid Account Currency ISO";
        //    else if (errorId == 1010)
        //        errorText = CoreService.Translation.GetUserMessageX(360607);//"Invalid account";
        //    else if (errorId == 1011)
        //        errorText = CoreService.Translation.GetUserMessageX(360608);//"Invalid account status";
        //    else if (errorId == 1015)
        //        errorText = "Transaction code is not supported from XAPI";
        //    else if (errorId == 1016)
        //        errorText = "Invalid User Information";
        //    else if (errorId == 1017)
        //        errorText = "Invalid Password";
        //    else if (errorId == 1019)
        //        errorText = "User Account Locked due to failed logins";
        //    else if (errorId == 1020)
        //        errorText = "User Must change the password since this is the first time logon";
        //    else if (errorId == 1021)
        //        errorText = "Account Locked by Bank Employee";
        //    else if (errorId == 1023)
        //        errorText = "Amount Information is not provided";
        //    else if (errorId == 1025)
        //        errorText = "User doesn't have access to the account";
        //    else if (errorId == 1028)
        //        errorText = "Transaction already processed";
        //    else if (errorId == 1029)
        //        errorText = "The original transaction was already reversed";
        //    else if (errorId == 1030)
        //        errorText = "Original transaction is in process of extraction and is not available for reversal";
        //    else if (errorId == 1031)
        //        errorText = "Exchange rates cannot be modified during partial/full reversals";
        //    else if (errorId == 1032)
        //        errorText = "Partial reversal cannot be applied on multi component transaction set";
        //    else if (errorId == 1033)
        //        errorText = "Invalid Employee";
        //    else if (errorId == 1034)
        //        errorText = "Employee does not has security permissions for the trancode";
        //    else if (errorId == 1035)
        //        errorText = "Invalid RIM";
        //    else if (errorId == 1036)
        //        errorText = "Invalid Transfer RIM";
        //    else if (errorId == 1037)
        //        errorText = "Database status has changed";
        //    else if (errorId == 1038)
        //        errorText = "Transaction cannot be partially reposted since the transaction was done in the past";
        //    else if (errorId == 1039)
        //        errorText = "Unable to find pre-exchanged transaction";
        //    else if (errorId == 1040)
        //        errorText = "Rate Expired";
        //    else if (errorId == 1041)
        //        errorText = "Monetary Transaction has been changed";
        //    else if (errorId == 1043)
        //        errorText = "Password Expired";
        //    else if (errorId == 1044)
        //        errorText = "Rate Change is Not Allowed";
        //    else if (errorId == 1045)
        //        errorText = "From and To accounts cannot be same for transfers";
        //    else if (errorId == 1046)
        //        errorText = "Transaction code is not supported in XAPI Real-time mode";
        //    else if (errorId == 1047)
        //        errorText = "Transaction code is not supported in XAPI Memo mode";
        //    else if (errorId == 1048)
        //        errorText = "Float currency is not matching with account currency ID";
        //    else if (errorId == 1049)
        //        errorText = "No accounts are setup for Direct Debit Authorization";
        //    else if (errorId == 1050)
        //        errorText = "Transaction has overrides";
        //    else if (errorId == 1051)
        //        errorText = "Invalid User status";
        //    else if (errorId == 1052)
        //        errorText = "Database status has changed. Please retry the transactions";
        //    else if (errorId == 1053)
        //        errorText = "Transaction mode not available currently";
        //    else if (errorId == 1054)
        //        errorText = "One of the Amount/Rate/Check No parameter is negative";
        //    else if (errorId == 1055)
        //        errorText = "The query did not produce any results";
        //    else if (errorId == 1056)
        //        errorText = "Transaction cannot be reversed since the transaction was done in the past";
        //    else if (errorId == 1057)
        //        errorText = "Sum of Float Entries Exceeds the Transaction amounts";
        //    else if (errorId == 1058)
        //        errorText = "Employee does not has access to the service";
        //    else if (errorId == 1059)
        //        errorText = "Invalid ApplType and Trancode Combination";
        //    else if (errorId == 1060)
        //        errorText = "Insufficient Data";
        //    else if (errorId == 1061)
        //        errorText = "Original transaction could not be located";
        //    else if (errorId == 1062)
        //        errorText = "Invalid TIN";
        //    else if (errorId == 1063)
        //        errorText = "Invalid Receipt ID";
        //    else if (errorId == 1064)
        //        errorText = "Invalid origin branch #";
        //    else if (errorId == 1065)
        //        errorText = "Transaction available only during day mode";
        //    else if (errorId == 1066)
        //        errorText = "Primary database unavailable to process the transaction.";
        //    else if (errorId == 1067)
        //        errorText = "Employee cannot access the customer";
        //    else if (errorId == 1068)
        //        errorText = "Employee does not has sufficient limits";
        //    else if (errorId == 1069)
        //        errorText = "Transaction amount exceeds the per transaction limit";
        //    else if (errorId == 1070)
        //        errorText = "Transaction will cause the daily limits to be exceeded";
        //    else if (errorId == 1071)
        //        errorText = "Account has reached per period deposit limit";
        //    else if (errorId == 1072)
        //        errorText = "Minimum Deposit Limit Not Met";
        //    else if (errorId == 1073)
        //        errorText = "Account Has Holds in Place";
        //    else if (errorId == 1074)
        //        errorText = "Account Has Maturity Check Pending";
        //    else if (errorId == 1075)
        //        errorText = "Account is an IRA Account";
        //    else if (errorId == 1076)
        //        errorText = "Account has reached per period withdrawal limit";
        //    else if (errorId == 1077)
        //        errorText = "Account has reached per period check limit";
        //    else if (errorId == 1078)
        //        errorText = "Account not part of retirement plan";
        //    else if (errorId == 1079)
        //        errorText = "The account has invalid IRA status";
        //    else if (errorId == 1080)
        //        errorText = "Transaction Amount less than the minimum withdrawal amount";
        //    else if (errorId == 1081)
        //        errorText = "Transaction Amount does not match the issue amount";
        //    else if (errorId == 1082)
        //        errorText = "Account does not has sufficient funds";
        //    else if (errorId == 1083)
        //        errorText = "Transaction amount exceeds analysis fees";
        //    else if (errorId == 1084)
        //        errorText = "Check is a suspect item";
        //    else if (errorId == 1085)
        //        errorText = "Check has a stop payment";
        //    else if (errorId == 1086)
        //        errorText = "This check already has an active restriction";
        //    else if (errorId == 1087)
        //        errorText = "Check Number exceeds the highest check number issued";
        //    else if (errorId == 1088)
        //        errorText = "Recon status could not be verified";
        //    else if (errorId == 1089)
        //        errorText = "Invalid recon status";
        //    else if (errorId == 1091)
        //        errorText = "Invalid Advance status";
        //    else if (errorId == 1092)
        //        errorText = "Account has unique interest advance conditions";
        //    else if (errorId == 1093)
        //        errorText = "Account has reached maximum disbursement date";
        //    else if (errorId == 1094)
        //        errorText = "Loan has already been advanced";
        //    else if (errorId == 1095)
        //        errorText = "Loan has not been advanced";
        //    else if (errorId == 1096)
        //        errorText = "Invalid effective date";
        //    else if (errorId == 1097)
        //        errorText = "Transaction Amount exceeds undisbursed amount";
        //    else if (errorId == 1098)
        //        errorText = "Transaction Amount less than minimum advance amount";
        //    else if (errorId == 1099)
        //        errorText = "Transaction Amount less than undisbursed amount";
        //    else if (errorId == 1100)
        //        errorText = "Transaction Amount exceeds payoff amount";
        //    else if (errorId == 1101)
        //        errorText = "Invalid Interest Type";
        //    else if (errorId == 1102)
        //        errorText = "Loan is a participation";
        //    else if (errorId == 1103)
        //        errorText = "Transaction Amount exceeds total late fees due";
        //    else if (errorId == 1104)
        //        errorText = "Debit transactions not allowed on the loan account";
        //    else if (errorId == 1105)
        //        errorText = "Credit transactions not allowed on the loan account";
        //    //
        //    //There may be some positibe numbers in ov_errors!?
        //    if (errorText != string.Empty)
        //    {
        //        if ( bAppendNo)
        //            return errorId.ToString() + ":" + errorText;
        //        else
        //            return errorText;
        //    }
        //    else
        //        return errorText;
        //}
        //public string GetPhoenixXMMessageText( int errorId )
        //{
        //    return GetPhoenixXMMessageText( errorId, true );
        //}
		#endregion GetPhoenixXMMessageText
		#endregion

		#region ValidateAcct
        private bool ValidateAcct(IDbHelper dbHelper, string acctType, string acctNo, ref string status)    // #75763 - Added status
		{
			int offset = -1;
			string fromClause = string.Empty;
			string ledger = string.Empty;
			string sqlCommand = string.Empty;
			string whereClause = string.Empty;
			string tempAcctNo = string.Empty;
			acctType = acctType.Trim();
			acctNo = acctNo.Trim();
            //PInt AccountCount = new PInt("AccountCount");         // #75763
            PString AccountStatus = new PString("AccountStatus");       // #75763

			if ( acctType.Length == 0 || acctNo.Length == 0 )
				return false;
			if (acctType == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.GL )
			{
				fromClause = "{0}gl_acct";
				if (acctNo.IndexOf(@"*") == -1)
				{
					whereClause = " acct_type = '" + acctType + "' And acct_no = '" + acctNo + "'";
				}
				else
				{
					offset = acctNo.LastIndexOf(@"-");
					ledger = acctNo.Substring(0, offset - 1);
					tempAcctNo = acctNo.Replace("*", "-");
					whereClause = " ledger_no = '" + ledger + "'" + " and acct_type = '" + acctType + "' and " +
						" acct_no like '" + tempAcctNo + "' ";
				}
			}
			else if (acctType == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.RIM )
			{
				fromClause = "{0}rm_acct";
				whereClause = " rim_no = " + acctNo;
			}
			else //Non GL Acct Type
			{
				PString DepLoan = new PString("DepLoan");

				sqlCommand = " select dep_loan from {0}ad_gb_acct_type where acct_type = '" + acctType + "' " +
                    " union select '" + Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.CM + "'" + " from {0}ad_cm_acct_type where acct_type = '" + acctType + "' " +
                    " union select '" + Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.SD + "'" + " from {0}ad_sd_acct_type where acct_type = '" + acctType + "' " +
                    " union select '" + Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.EX + "'" + " from {0}ad_ex_acct_type where acct_type = '" + acctType + "' " ; //#5871,196262
				whereClause = " acct_type = '" + acctType + "' and acct_no = '" + acctNo + "' ";
				if ( ExecSqlImmediateInto( dbHelper, sqlCommand, false, true, DepLoan ))
				{
					do
					{
						if (DepLoan.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.LN)
						{
							//fromClause = "{0}LN_ACCT";
							fromClause = "{0}LN_DISPLAY";
							break;
						}
						/*Begin Enh# 70728*/
						else if (DepLoan.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.CM)
						{
							//fromClause = "{0}LN_ACCT";
							fromClause = "{0}LN_UMB";
							break;
						}
						/*End Enh# 70728*/
						else if (DepLoan.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.DP)
						{
							//fromClause = "{0}DP_ACCT";
							fromClause = "{0}DP_DISPLAY";
							break;
						}
						else if (DepLoan.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.SD)
						{
							fromClause = "{0}SD_ACCT";
							break;
						}
                        else if (DepLoan.Value == "EX") //Begin #76458
                        {
                            fromClause = "{0}EX_ACCT";
                            break;
                        }
					}
					while (ExecSqlGetNextRow( ));
					if (fromClause.Length == 0)  // Houston we have a problem
						return false;
				}
				else
					return false;
			}
			//Now Search for Account
            //sqlCommand = " select count(*) from " + fromClause + " where " + whereClause; // #75763
            sqlCommand = " select status from " + fromClause + " where " + whereClause;   // #75763
            if (!ExecSqlImmediateInto(dbHelper, sqlCommand, false, false, AccountStatus))  //When No Records are returned    // #75763
				return false;
			else  // We have records
			{
                if (!AccountStatus.IsNull) // && AccountCount.Value > 0)  // #75763
                {
                    status = AccountStatus.Value;       // #75763
                    return true;
                }
                else
                    return false;
			}
		}
        /*Begin Task #82748*/
        /*The GL ledger type should be an 'Asset' or 'Liability'  */
        private bool ValidateGLLederType(IDbHelper dbHelper, string actType, string actNo)  
        {   
            string sqlCommand = string.Empty;         
            PString ledgerType = new PString("ledgerType");
            PString acctType = new PString("acctType");
            PString acctNO = new PString("acctNO");

            acctType.Value= actType.Trim();
            acctNO.Value = actNo.Trim();

            sqlCommand = string.Format(" select ledger_type from {0}gl_acct where acct_type = {1} and acct_no = {2} ",
                                        dbHelper.DbPrefix, acctType.SqlString, acctNO.SqlString);

            ExecSqlImmediateInto(dbHelper, sqlCommand, ledgerType);

            if ( !ledgerType.IsNull &&  (ledgerType.Value.Trim() == "Asset" || 
                                        ledgerType.Value.Trim() == "Liability"))
            {
                return true; 
            }

            return false;

        }
        /*End Task #82748*/

        #endregion

        //Begin #76458
        #region GetMaskedExtAcct
        private bool GetMaskedExtAcct(IDbHelper dbHelper, string acctType, string acctNo, ref string maskedAcct)
        {
            string sqlCommand = string.Empty;
            acctType = acctType.Trim();
            acctNo = acctNo.Trim();
            PString AcctNoMask = new PString("AcctNoMask");
            maskedAcct = null;

            if (acctType.Length == 0 || acctNo.Length == 0)
                return false;

            //#10410 SELECT acct_no_mask  .... Handle spaces for Sybase/MsSql
            //30303, Change db name when in night mode to x_ex_acct
            string sTablename = "";           
            /*Brgin #70838/#820590 - Bug Fix*/
            if (BusGlobalVars.EncryptPanEnabled == "Y" && IsRunningOnServer  
                && XmApplicationCollecion.CurrentApplication.AcctFormats[acctType] != null
                    && XmApplicationCollecion.CurrentApplication.AcctFormats[acctType].EncryptAcctNo == "Y")
            {
                if (dbHelper.CopyStatus != "D")
                    sTablename = dbHelper.XmDbName + "..X_ex_acct  ex inner join " + dbHelper.XmDbName + "..x_ext_acct_encrypt encr on ex.acct_no = encr.acct_id ";
                else
                    sTablename = dbHelper.PhoenixDbName + "..va_ex_acct ex inner join " + dbHelper.PhoenixDbName + "..ext_acct_encrypt encr on ex.acct_no = encr.acct_id";

                sqlCommand = string.Format(@"
                SELECT
                    (CASE
                        WHEN ltrim(rtrim(isnull(acct_no_mask,''))) = '' THEN 'XXXX-XXXX-XXXX-'+encr.four_digit_acct_no
                        ELSE acct_no_mask END
                    )
                FROM " + sTablename + " WHERE acct_type = '{1}' AND acct_no = '{2}'", dbHelper.DbPrefix, acctType, acctNo);
            }
            /*End #70838*/
            else
            {
                if (dbHelper.CopyStatus != "D")
                    sTablename = dbHelper.XmDbName + "..X_ex_acct ";
                else
                    sTablename = dbHelper.PhoenixDbName + "..va_ex_acct ";
                sqlCommand = string.Format(@"
                SELECT
                    (CASE
                    WHEN rtrim(acct_no_mask) = '' THEN NULL
                    WHEN rtrim(acct_no_mask) IS NULL THEN NULL
                    WHEN acct_no_mask IS NULL THEN NULL
                    ELSE acct_no_mask END
                    )
                FROM " + sTablename + " WHERE acct_type = '{1}' AND acct_no = '{2}'", dbHelper.DbPrefix, acctType, acctNo);
            }

            if (ExecSqlImmediateInto(dbHelper, sqlCommand, false, false, AcctNoMask))
            {
                if (AcctNoMask.IsNull)
                    return false;
                if (!AcctNoMask.IsNull && AcctNoMask.Value.Length > 0)
                {
                    maskedAcct = AcctNoMask.Value;
                    return true;
                }
            }
            return false;
        }
        #endregion
        //End #76458

		#region Balance Def
		public short GetDpLnCntrlAvailBalDef( IDbHelper dbHelper, string depLoan )
		{
			LoadObjects( dbHelper, depLoan );
			return GetDpLnCntrlAvailBalDef( depLoan );
		}
		public short GetColBalDef( IDbHelper dbHelper, string depLoan )
		{
			LoadObjects( dbHelper, depLoan );
			return GetColBalDef( depLoan );
		}
		private void LoadObjects( IDbHelper dbHelper, string depLoan )
		{
			if ( depLoan == GlobalVars.Instance.ML.DP )
			{
				if ( GlobalObjects.Instance[ GlobalObjectNames.AdDpControl ] == null )
				{
					AdDpControl tempObj = new AdDpControl();
					tempObj.ActionType = XmActionType.Select;
					tempObj.DoAction( dbHelper );
					GlobalObjects.Instance[ GlobalObjectNames.AdDpControl ] = tempObj;
				}
			}
			else if ( depLoan == GlobalVars.Instance.ML.LN )
			{
				if ( GlobalObjects.Instance[ GlobalObjectNames.AdLnControl ] == null )
				{
					AdLnControl tempObj = new AdLnControl();
					tempObj.ActionType = XmActionType.Select;
					tempObj.DoAction( dbHelper );
					GlobalObjects.Instance[ GlobalObjectNames.AdLnControl ] = tempObj;
				}
			}
			/*Begin Enh# 70728*/
			else if ( depLoan == GlobalVars.Instance.ML.CM )
			{
				if ( GlobalObjects.Instance[ GlobalObjectNames.AdLnUmbControl ] == null )
				{
					AdLnUmbControl tempObj = new AdLnUmbControl();
					tempObj.ActionType = XmActionType.Select;
					tempObj.DoAction( dbHelper );
					GlobalObjects.Instance[ GlobalObjectNames.AdLnUmbControl ] = tempObj;
				}
			}
			/*End Enh# 70728*/
			else
			{
				if ( GlobalObjects.Instance[ "AdXpSvcs" ] == null )
				{
					AdXpSvcs tempObj = new AdXpSvcs();
					tempObj.ActionType = XmActionType.Select;
					tempObj.ServiceId.Value = Phoenix.FrameWork.BusFrame.BusGlobalVars.ServiceId;
					tempObj.DoAction( dbHelper );
					GlobalObjects.Instance[ "AdXpSvcs" ] = tempObj;
				}
			}
		}
		public short GetSvcsAvailBalDef( IDbHelper dbHelper, string postingMethod, string applType )
		{
			LoadObjects( dbHelper, null );
			return GetSvcsAvailBalDef( postingMethod, applType );
		}
		#endregion

		#region UserSecurityRights
		public bool VerifyUserSecurityRights(IDbHelper dbHelper,int rimNo, int relRimNo,int usrDefId, int serviceId)
		{
			PString fieldValue = new PString();
			string sSQL = string.Format(@" select value
							from {0}xp_rm_svcs_user_fld
                           where service_id = {1}
							and rim_no = {2} and rel_rim_no = {3} and field_id = {4}","{0}",serviceId,rimNo,relRimNo,usrDefId);
			if (ExecSqlImmediateInto(dbHelper,sSQL,fieldValue))
			{
				if (fieldValue.IsNull || fieldValue.Value == "N")
					return false;
				else
					return true;
			}
			return false;



		}
		public bool VerifyUserSecurityRights(IDbHelper dbHelper,int rimNo, int relRimNo,int usrDefId, int serviceId, ref string fieldDesc)
		{
			PString fieldValue = new PString();
			PString desc = new PString();
			string sSQL = string.Format(@" select value, field_label
							from {0}xp_rm_svcs_user_fld userfld, {0}ad_xp_addl_fld addl
                           where addl.service_id = {1}
							and addl.field_id = {4}
							and userfld.service_id = {1}
							and userfld.rim_no = {2} and userfld.rel_rim_no = {3} and
							userfld.field_id =* addl.field_id"	 ,"{0}",serviceId,rimNo,relRimNo,usrDefId);
			if (ExecSqlImmediateInto(dbHelper,sSQL,fieldValue,desc))
			{
				fieldDesc = desc.Value;
				if (fieldValue.IsNull || fieldValue.Value == "N")
					return false;
				else
					return true;
			}

			return false;



		}

        //Begin - RBhavsar - Moved this method to RmAchCompany BO
//        public bool VerifyCpyLimits(IDbHelper dbHelper,object sObject,string companyId,DateTime limitVerifyDt, string limitType, string tranType,string fileType, decimal amount)
//        {
//            RmAchCompany rmAchCompany;
//            PDecimal dailyFileDrUsed = new PDecimal();
//            PDecimal dailyFileCrUsed = new PDecimal();
//            PDecimal dailyBatchDrUsed = new PDecimal();
//            PDecimal dailyBatchCrUsed = new PDecimal();
//            PDecimal monthFileDrUsed = new PDecimal();
//            PDecimal monthFileCrUsed = new PDecimal();
//            PDecimal monthBatchDrUsed = new PDecimal();
//            PDecimal monthBatchCrUsed = new PDecimal();
//            PDateTime limitStartDt = new PDateTime();
//            PDateTime limitEndDt = new PDateTime();
//            string sSQL = "";
//            rmAchCompany = sObject as RmAchCompany;
//            if (rmAchCompany == null)
//                return false;
//            #region Per Transaction Limit Validation
//            if (limitType == "T")
//            {
//                if (fileType == "P")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerTranPayrollDr.IsNull && amount > rmAchCompany.PerTranPayrollDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerTranPayrollCr.IsNull && amount > rmAchCompany.PerTranPayrollCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//                else if (fileType == "T")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerTranTaxDr.IsNull && amount > rmAchCompany.PerTranTaxDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerTranTaxCr.IsNull && amount > rmAchCompany.PerTranTaxCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//                else if (fileType == "O")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerTranOtherAchDr.IsNull && amount > rmAchCompany.PerTranOtherAchDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerTranOtherAchCr.IsNull && amount > rmAchCompany.PerTranOtherAchCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//            }
//            #endregion

//            #region Per File Limit Validation
//            if (limitType == "F")
//            {
//                if (fileType == "P")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerFilePayrollDr.IsNull && amount > rmAchCompany.PerFilePayrollDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerFilePayrollCr.IsNull && amount > rmAchCompany.PerFilePayrollCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//                else if (fileType == "T")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerFileTaxDr.IsNull && amount > rmAchCompany.PerFileTaxDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerFileTaxCr.IsNull && amount > rmAchCompany.PerFileTaxCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//                else if (fileType == "O")
//                {
//                    if (tranType == "Dr" && !rmAchCompany.PerFileOtherAchDr.IsNull && amount > rmAchCompany.PerFileOtherAchDr.Value )
//                        return false;
//                    else if (tranType == "Cr" && !rmAchCompany.PerFileOtherAchCr.IsNull && amount > rmAchCompany.PerFileOtherAchCr.Value)
//                        return false;
//                    else
//                        return true;
//                }
//            }
//            #endregion

//            #region Daily Limit Validation
//            if (limitType == "D")
//            {
//                sSQL = string.Format(@"
//						select sum(tot_debit_entry_amt ), sum(tot_credit_entry_amt)
//						from {0}ach_header
//						where file_effective_dt  = '{1}'
//						and company_id = '{2}'
//						and format_type in ('C','P','U')
//						and status != 'Closed' ","{0}",limitVerifyDt.ToShortDateString(),companyId);
//                if (ExecSqlImmediateInto(dbHelper,sSQL,dailyFileDrUsed,dailyFileCrUsed))
//                {
//                    if (tranType == "Dr")
//                    {
//                        sSQL = string.Format(@"
//							select sum(amt )
//							from {0}rm_ach_batch batch ,{0}rm_ach_batch_details detail
//							where batch.next_tfr_dt  = '{1}'
//							and batch.company_id = '{2}'
//							and batch.status != 'Closed'
//							and batch.Template != 'Y'
//							and batch.batch_id = detail.batch_id
//							and batch.rim_no = detail.rim_no
//							and detail.tran_code in (27,37,57) ","{0}",limitVerifyDt.ToShortDateString(),companyId);
//                        if (ExecSqlImmediateInto(dbHelper,sSQL,dailyBatchDrUsed))
//                            if ( !rmAchCompany.DailyOrigDr.IsNull  && dailyFileDrUsed.Value + dailyBatchDrUsed.Value + amount > rmAchCompany.DailyOrigDr.Value)
//                                return false;
//                            else
//                                return true;
//                    }
//                    else
//                    {
//                        sSQL = string.Format(@"
//							select sum(amt )	from
//							{0}rm_ach_batch batch ,{0}rm_ach_batch_details detail
//							where batch.next_tfr_dt  = '{1}'
//							and batch.company_id = '{2}'
//							and batch.status != 'Closed'
//							and batch.Template != 'Y'
//							and batch.batch_id = detail.batch_id
//							and batch.rim_no = detail.rim_no
//							and detail.tran_code in (22,32,52)","{0}",limitVerifyDt.ToShortDateString(),companyId);
//                        if (ExecSqlImmediateInto(dbHelper,sSQL,dailyBatchCrUsed))
//                            if ( !rmAchCompany.DailyOrigCr.IsNull && dailyFileCrUsed.Value + dailyBatchCrUsed.Value + amount > rmAchCompany.DailyOrigCr.Value)
//                                return false;
//                            else
//                                return true;
//                    }
//                }

//            }

//            #endregion

//            #region Monthly Limit Validation
//            if (limitType == "M")
//            {
//                limitStartDt.Value = new System.DateTime(limitVerifyDt.Year,limitVerifyDt.Month,1);
//                limitEndDt.Value = limitStartDt.Value.AddMonths(1);
//                limitEndDt.Value = limitEndDt.Value.AddDays(-1);
//                sSQL = string.Format(@"
//						select sum(tot_debit_entry_amt), sum(tot_credit_entry_amt)
//						from {0}ach_header
//						where file_effective_dt  between  '{1}' and '{2}'
//						and company_id = '{3}'
//						and status != 'Closed'
//						and format_type in ('C','P','U')","{0}",limitStartDt.StringValue,limitEndDt.StringValue,companyId);
//                if (ExecSqlImmediateInto(dbHelper,sSQL,monthFileDrUsed,monthFileCrUsed))
//                {
//                    if (tranType == "Dr")
//                    {
//                        sSQL = 	sSQL = string.Format(@"
//							select sum(amt )	from
//							{0}rm_ach_batch batch ,{0}rm_ach_batch_details detail
//							where batch.next_tfr_dt between  '{1}' and '{2}'
//							and batch.company_id = '{3}'
//							and batch.status != 'Closed'
//							and batch.Template != 'Y'
//							and batch.batch_id = detail.batch_id
//							and batch.rim_no = detail.rim_no
//							and detail.tran_code in (27,37,57)","{0}",limitStartDt.StringValue,limitEndDt.StringValue,companyId);
//                        if (ExecSqlImmediateInto(dbHelper,sSQL,monthBatchDrUsed))
//                            if (!rmAchCompany.MonthlyOrigDr.IsNull && monthFileDrUsed.Value + monthBatchDrUsed.Value + amount > rmAchCompany.MonthlyOrigDr.Value)
//                                return false;
//                            else
//                                return true;

//                    }
//                    else if (tranType == "Cr")
//                    {
//                        sSQL = 	sSQL = string.Format(@"
//							select sum(amt )	from
//							{0}rm_ach_batch batch ,{0}rm_ach_batch_details detail
//							where batch.next_tfr_dt between  '{1}' and '{2}'
//							and batch.company_id = '{3}'
//							and batch.status != 'Closed'
//							and batch.Template != 'Y'
//							and batch.batch_id = detail.batch_id
//							and batch.rim_no = detail.rim_no
//							and detail.tran_code in (22,32,52)","{0}",limitStartDt.StringValue,limitEndDt.StringValue,companyId);
//                        if (ExecSqlImmediateInto(dbHelper,sSQL,monthBatchCrUsed))
//                            if (!rmAchCompany.MonthlyOrigCr.IsNull && monthFileCrUsed.Value + monthBatchCrUsed.Value + amount > rmAchCompany.MonthlyOrigCr.Value)
//                                return false;
//                            else
//                                return true;
//                    }
//                }
//            }

//            #endregion

//            return false;


//        }
		#endregion
        //End - RBhavsar - Moved this method to RmAchCompany BO

        //Begin WI #27540
        /// <summary>
        /// Get Edit and Post Access Flag of a Employee
        /// </summary>
        /// <param name="dbHelper"></param>
        public void GetPostEditAccess(IDbHelper dbHelper)
        {
            if (_paramNodes.Count >= 6)
            {
                PBaseType RimNo = _paramNodes[0] as PBaseType;
                PBaseType AcctNo = _paramNodes[1] as PBaseType;
                PBaseType AcctType = _paramNodes[2] as PBaseType;
                PBaseType RetCode = _paramNodes[3] as PBaseType;
                PBaseType Post = _paramNodes[4] as PBaseType;
                PBaseType Edit = _paramNodes[5] as PBaseType;
                

                int rimNo = 0;
                if (Convert.ToInt32(RimNo.Value) <= 0)   // WI #29994. Changed Convert.Toint16(RimNo.Value) to Convert.Toint32(RimNo.Value)
                {
                    rimNo = GetRimNo(AcctType.Value.ToString(), AcctNo.Value.ToString());
                }
                else
                {
                    rimNo = Convert.ToInt32(RimNo.Value); // WI #29994. Changed Convert.Toint16(RimNo.Value) to Convert.Toint32(RimNo.Value)
                }

                string sSQL = string.Empty;
                SqlHelper sqlHelper = new SqlHelper();
                sSQL =
                string.Format(@"
                    declare @nRC int,
                    @sEdit char(1),
                    @sPost char(1)
                    exec @nRC = {0}psp_verify_emp_access {1}, {2}, {3}, @sPost OUTPUT , @sEdit OUTPUT, NULL 
                    select @nRC, @sPost, @sEdit", dbHelper.DbPrefix, rimNo, GlobalVars.EmployeeId, 0);

                sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL, RetCode, Post, Edit);

            }
        }
        //End WI #27540

        /* Begin WI #46760*/
        /// <summary>
        /// Check whether it is a NACHA holiday or not. Return true if it is Holiday
        /// </summary>
        /// <param name="dbHelper"></param>
        public void IsNachaHoliday(IDbHelper dbHelper)
        {
            if (_paramNodes.Count >= 2)
            {
                PBaseType ProcessingDate = _paramNodes[0] as PBaseType;

                PDateTime dtProcessingDate = new PDateTime();
                PInt IsHoliday = new PInt();
                string sSQL = string.Empty;
                SqlHelper sqlHelper = new SqlHelper();

                dtProcessingDate.Value = Convert.ToDateTime(ProcessingDate.Value); 

    

                sSQL = string.Format(@" select 1 from {0}ov_nacha_holiday 
	                                    where STATUS = '{1}'
	                                    and HOLIDAY_DT = '{2}'
	                                    union 
	                                    select 1 where datepart( dw, '{2}') in(1,7) ",
                                        DbPrefix,
                                        GlobalVars.Instance.ML.Active,
                                        DateHelper.FormatDate(dtProcessingDate.Value, false));

                sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL, IsHoliday);

                if (IsHoliday.Value == 1)
                {
                    (_paramNodes[1] as PBaseType).ValueObject = true;
                }
                else
                {
                    (_paramNodes[1] as PBaseType).ValueObject = false;
                }

                
            }
        }
        /* End #46760*/

        /*Begin #65549*/
        public void GetDecryptedPan(IDbHelper dbHelper)
        {
            if (_paramNodes.Count == 2)  
            {
                PBaseType sPanId        =   _paramNodes[0] as PBaseType;
                PString sPan            =   new PString("sPan");
                PString sEncryptedPan   =   new PString("sEncryptedPan");
                string sTableName       =   string.Empty;

                if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
                    sTableName = string.Format(@" {0}..X_ATM_PAN_ENCRYPT", dbHelper.XmDbName);
                else
                    sTableName = string.Format(@" {0}..ATM_PAN_ENCRYPT", dbHelper.PhoenixDbName);

                ExecSqlImmediateInto(CoreService.DbHelper,
                    string.Format(@"
									Select
										ISNULL(encrypted_pan,encrypted_15_digit_pan)
									From 
										{0} 
									Where
										pan_id = '{1}'", sTableName, sPanId.StringValue), sEncryptedPan);

                if (!sEncryptedPan.IsNull && sEncryptedPan.Value != string.Empty)
                {
                    // #PENTEST_FIX_REQUIRED - Validate that the decryption is happening the old way( Before and code change decrypt. value matches )
                    //Phoenix.FrameWork.Core.Utilities.AesCryptoProvider _aesCryptoProvider = new Phoenix.FrameWork.Core.Utilities.AesCryptoProvider();
                    //sPan.Value = _aesCryptoProvider.Decrypt(sEncryptedPan.Value, BusGlobalVars.PanEcrptPartialKey);
                    sPan.Value = this.DecryptPAN(sEncryptedPan.Value, BusGlobalVars.PanEcrptPartialKey);
                    (_paramNodes[1] as PBaseType).ValueObject = sPan.Value;
                }

            }
        }
        /*End #65549*/

        /*Begin #71500*/
        public void GetDecryptedAcctNo(IDbHelper dbHelper)
        {
            if (_paramNodes.Count == 2)
            {
                PBaseType sAcctId = _paramNodes[0] as PBaseType;
                PString sAcctNo = new PString("sAcctNo");
                PString sEncryptedAcctNo = new PString("sEncryptedAcctNo");
                string sTableName = string.Empty;

                if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
                    sTableName = string.Format(@" {0}..X_EXT_ACCT_ENCRYPT", dbHelper.XmDbName);
                else
                    sTableName = string.Format(@" {0}..EXT_ACCT_ENCRYPT", dbHelper.PhoenixDbName);

                ExecSqlImmediateInto(CoreService.DbHelper,
                    string.Format(@"
									Select
										encrypted_acct_no
									From 
										{0} 
									Where
										acct_id = '{1}'", sTableName, sAcctId.StringValue), sEncryptedAcctNo);

                if (!sEncryptedAcctNo.IsNull && sEncryptedAcctNo.Value != string.Empty)
                {
                    // #PENTEST_FIX_REQUIRED - Validate that the decryption is happening the old way( Before and code change decrypt. value matches )
                    //Phoenix.FrameWork.Core.Utilities.AesCryptoProvider _aesCryptoProvider = new Phoenix.FrameWork.Core.Utilities.AesCryptoProvider();
                    //sAcctNo.Value = _aesCryptoProvider.Decrypt(sEncryptedAcctNo.Value, BusGlobalVars.PanEcrptPartialKey);
                    sAcctNo.Value = this.DecryptPAN(sEncryptedAcctNo.Value, BusGlobalVars.PanEcrptPartialKey);
                    (_paramNodes[1] as PBaseType).ValueObject = sAcctNo.Value;
                }

            }
        }
        /*End #71500*/

        /*Begin #130111*/
        private string GetPanId(IDbHelper dbHelper, string panNumber)
        {
            string _EncryptedpanNo = string.Empty;
            PString _panId = new PString("_panId");
            // #PENTEST_FIX_REQUIRED - Validate that the encryption is happening the old way( Before and code change enc. value matches )
            //Phoenix.FrameWork.Core.Utilities.AesCryptoProvider _aesCryptoProvider = new Phoenix.FrameWork.Core.Utilities.AesCryptoProvider();
            //_EncryptedpanNo = _aesCryptoProvider.Encrypt(panNumber, BusGlobalVars.PanEcrptPartialKey);
            _EncryptedpanNo = this.EncryptPAN(panNumber, BusGlobalVars.PanEcrptPartialKey);

            string sTableName = string.Empty;

            if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
                sTableName = string.Format(@" {0}..X_ATM_PAN_ENCRYPT", dbHelper.XmDbName);
            else
                sTableName = string.Format(@" {0}..ATM_PAN_ENCRYPT", dbHelper.PhoenixDbName);

            string selectSql = string.Empty;

            selectSql = string.Format(@"
											Select top 1 pan_id 
                                            from {0} 
                                            where encrypted_pan = '{1}' or encrypted_15_digit_pan = '{1}'   
                                            ", sTableName, _EncryptedpanNo);

            ExecSqlImmediateInto(dbHelper, selectSql.ToString(), _panId);
            return string.IsNullOrEmpty(_panId.StringValue) ? "" : _panId.StringValue;
        }

        public int GetRimNoByPan(IDbHelper dbHelper, string psPanNo)
        {

            PString sPanNo = new PString();

            sPanNo.Value = StringHelper.StrTrimX(psPanNo);

            if(string.IsNullOrEmpty(sPanNo.Value))
            {
                return int.MinValue;
            }

            if(BusGlobalVars.EncryptPanEnabled == "Y")
            {
                sPanNo.Value = GetPanId(dbHelper, sPanNo.Value);
            }

            StringBuilder sSQL = new StringBuilder();
            string sTablename = string.Empty;
            PString RimNo = new PString("A0");

            if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
            {
                sTablename = dbHelper.XmDbName + "..X_ATM_CARD";
            }
            else
            {
                sTablename = dbHelper.PhoenixDbName + "..ATM_CARD";
            }

            sSQL.AppendFormat(@"
							Select
								RIM_NO
							From
								{0}
							Where
								pan = {1}", sTablename, sPanNo.SqlString);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), RimNo);

            return (RimNo.IsNull ? int.MinValue : RimNo.IntValue);


        }

        /*End #130111*/


        #endregion Helper methods

        //Begin #06167
        #region commented code
        //        #region Database To Use
        //        public DbToUse DbToUpdate( IDbHelper dbHelper )
        //        {
        //            return DbToUpdate( dbHelper.CopyStatus );
        //        }

        //        public DbToUse DbToInquire( IDbHelper dbHelper )
        //        {
        //            return DbToInquire( dbHelper.CopyStatus );
        //        }

        //        public DbToUse DbToInsert( IDbHelper dbHelper )
        //        {
        //            return DbToInsert( dbHelper.CopyStatus );
        //        }
        //        private DbToUse DbToUpdate( string copyStatus )
        //        {
        //            if( copyStatus == "B" || copyStatus == "C" )
        //                return DbToUse.PrimaryOnly | DbToUse.SecondaryOnly ;
        //            else if ( copyStatus == "D" )
        //                return DbToUse.PrimaryOnly;
        //            else if ( copyStatus == "N" )
        //                return DbToUse.SecondaryOnly ;
        //            else
        //                return DbToUse.Offline;
        //        }

        //        private DbToUse DbToInquire( string copyStatus )
        //        {
        //            if( copyStatus == "D" || copyStatus == "C" )
        //                return DbToUse.PrimaryOnly;
        //            else if( copyStatus == "B" || copyStatus == "N" )
        //                return DbToUse.SecondaryOnly ;
        //            else
        //                return DbToUse.Offline;
        //        }

        //        private DbToUse DbToInsert( string copyStatus )
        //        {
        //            if( copyStatus == "D" || copyStatus == "B" )
        //                return DbToUse.PrimaryOnly;
        //            else if( copyStatus == "N" || copyStatus == "C" )
        //                return DbToUse.SecondaryOnly ;
        //            else
        //                return DbToUse.Offline;
        //        }
        //        #endregion

        //        #region database table prefixes
        //        public string[] UpdateDbPrefix( IDbHelper dbHelper )
        //        {
        //            DbToUse dbToUpdate = DbToUpdate( dbHelper );
        //            string delimiterStr = ",";
        //            string prefix = null;
        //            char[] delimiter = delimiterStr.ToCharArray();
        //            string[] updateDbPrefix = null;
        //            if (( dbToUpdate & DbToUse.PrimaryOnly)  == DbToUse.PrimaryOnly  )
        //                //				updateDbPrefix[index++] = dbHelper.PhoenixDbName + "..";
        //                prefix = prefix + dbHelper.PhoenixDbName + "..";
        //            if (( dbToUpdate & DbToUse.Offline)  == DbToUse.Offline  )
        //            {
        //                //				updateDbPrefix[index++] = " ";
        //                //				prefix = prefix + delimiter + " ";
        //                prefix = prefix + delimiterStr + " ";
        //            }
        //            if (( dbToUpdate & DbToUse.SecondaryOnly )  == DbToUse.SecondaryOnly   )
        //            {
        //                //				updateDbPrefix[index] = "X_";
        //                //				prefix = prefix + delimiter + "X_";
        //                if (prefix != null)
        //                    prefix = prefix + delimiterStr + "X_";
        //                else
        //                    prefix = prefix + "X_";
        //            }
        //            updateDbPrefix = prefix.Split(delimiter);
        //            return updateDbPrefix;
        //        }

        //        public string InquireDbPrefix( IDbHelper dbHelper )
        //        {
        //            DbToUse dbToInquire = DbToInquire( dbHelper );
        //            if (( dbToInquire & DbToUse.PrimaryOnly)  == DbToUse.PrimaryOnly  )
        //                return dbHelper.PhoenixDbName + "..";
        //            if (( dbToInquire & DbToUse.Offline)  == DbToUse.Offline  )
        //                return " ";
        //            if (( dbToInquire & DbToUse.SecondaryOnly )  == DbToUse.SecondaryOnly   )
        //                return "X_";
        //            return "";
        //        }

        //        public string InsertDbPrefix( IDbHelper dbHelper )
        //        {
        //            DbToUse dbToInsert = DbToInsert( dbHelper );
        //            if (( dbToInsert & DbToUse.PrimaryOnly)  == DbToUse.PrimaryOnly  )
        //                return dbHelper.PhoenixDbName + "..";
        //            if (( dbToInsert & DbToUse.Offline)  == DbToUse.Offline  )
        //                return " ";
        //            if (( dbToInsert & DbToUse.SecondaryOnly )  == DbToUse.SecondaryOnly   )
        //                return "X_";
        //            return "";
        //        }
        //        #endregion

        //        #region sql functions
        //        public bool ExecSqlImmediate( IDbHelper dbHelper, string sql, ref object[] result )
        //        {
        //            return ExecSqlImmediate( dbHelper, sql, ref result, false );
        //        }

        //        public bool ExecSqlImmediate( IDbHelper dbHelper, string sql, ref object[] result, bool forcePrimeDb )
        //        {
        //            try
        //            {
        //                pSqlHelper.CloseReader();

        //                if ( forcePrimeDb && !dbHelper.IsPrimaryDbAvailable )
        //                    return false;
        //                if ( !forcePrimeDb)
        //                    sql = string.Format(sql, InquireDbPrefix( dbHelper ) );
        //                else
        //                    sql = string.Format(sql, dbHelper.PhoenixDbName + ".." );

        //                result = pSqlHelper.SqlImmediate( dbHelper, sql );
        //                if ( result != null )
        //                    return true;
        //                else
        //                    return false;
        //            }
        //            finally
        //            {
        //                pSqlHelper.CloseReader();
        //            }
        //        }

        //        public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, params IPhoenixDataType[] intoVars )
        //        {
        //            return ExecSqlImmediateInto( dbHelper, sql, false, false, intoVars );
        //        }

        //        public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, bool forcePrimeDb, params IPhoenixDataType[] intoVars )
        //        {
        //            return ExecSqlImmediateInto( dbHelper, sql, forcePrimeDb, false, intoVars );
        //        }

        //        public bool ExecSqlImmediateInto( IDbHelper dbHelper, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars )
        //        {
        //            return ExecSqlImmediateInto( dbHelper, (IDbConnection) null, sql, forcePrimeDb, multipleRows, intoVars );
        //        }

        ////		public bool ExecSqlImmediateInto( IDbConnection dbConnection, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars )
        ////		{
        ////			return ExecSqlImmediateInto( (IDbHelper) null, dbConnection, sql, forcePrimeDb, multipleRows, intoVars );
        ////		}

        //        public bool ExecSqlImmediateInto( IDbHelper dbHelper, IDbConnection dbConnection, string sql, bool forcePrimeDb, bool multipleRows, params IPhoenixDataType[] intoVars )
        //        {
        //            PSqlHelper.HelperStatus pSqlStatus = PSqlHelper.HelperStatus.None;

        //            try
        //            {
        //                pSqlHelper.CloseReader();

        //                if ( forcePrimeDb && !dbHelper.IsPrimaryDbAvailable )
        //                    return false;

        //                if ( !forcePrimeDb)
        //                    sql = string.Format(sql, InquireDbPrefix( dbHelper ) );
        //                else
        //                    sql = string.Format(sql, dbHelper.PhoenixDbName + ".." );

        //                if( dbConnection == null )
        //                    pSqlStatus =  pSqlHelper.SqlImmediateInto(dbHelper, sql, intoVars );
        //                else
        //                    pSqlStatus =  pSqlHelper.SqlImmediateInto(dbConnection, sql, intoVars );

        //                return ( pSqlStatus == PSqlHelper.HelperStatus.Success );
        //            }
        //            finally
        //            {
        //                if (!multipleRows || pSqlStatus == PSqlHelper.HelperStatus.NoMoreRecords || pSqlStatus == PSqlHelper.HelperStatus.NoResults)
        //                    pSqlHelper.CloseReader();
        //                else
        //                    intoList = intoVars;
        //            }

        //        }

        //        public bool ExecSqlGetNextResultSet( )
        //        {
        //            bool returnValue = false;

        //            if( pSqlHelper != null )
        //                returnValue = pSqlHelper.NextResult();

        //            if ( !returnValue )
        //                pSqlHelper.CloseReader();

        //            return returnValue;
        //        }

        //        public bool ExecSqlGetNextRow( bool moveToNextResultSet, params IPhoenixDataType[] intoVars )
        //        {
        //            PSqlHelper.HelperStatus pSqlStatus = PSqlHelper.HelperStatus.None;

        //            if ( pSqlHelper != null )
        //            {
        //                pSqlStatus = pSqlHelper.NextRowInto(intoVars);
        //                if ( pSqlStatus == PSqlHelper.HelperStatus.NoMoreRecords && moveToNextResultSet )
        //                {
        //                    if (ExecSqlGetNextResultSet())
        //                        pSqlStatus = pSqlHelper.NextRowInto(intoVars);
        //                }

        //            }

        //            return ( pSqlStatus == PSqlHelper.HelperStatus.Success );
        //        }

        //        public bool ExecSqlGetNextRow( )
        //        {
        //            return ExecSqlGetNextRow( true, intoList );
        //        }

        //        public void CloseReader()
        //        {
        //            pSqlHelper.CloseReader();

        //            return;
        //        }

        //        public bool ExecSqlExists( IDbHelper dbHelper, string sql )
        //        {
        //            object [] result = null;
        //            return ExecSqlImmediate( dbHelper, sql, ref result );
        //        }
        //        #endregion
        #endregion


        #region Database To Use
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
        public void CloseReader()
        {
            _sqlHelper.CloseReader();
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
        //End #06167

        //140793
        #region AutoCompleteList
        private void GetAutoCompleteSelectedValue(IDbHelper dbHelper)
        {
            PString listName = new PString("listName");
            listName.Value = Convert.ToString((_paramNodes[0] as PBaseType).ValueObject);
            PString selectedListItem = new PString("selectedListItem");
            selectedListItem.Value = Convert.ToString((_paramNodes[1] as PBaseType).ValueObject);
            PInt nEmplId = new PInt("nEmplId");
            nEmplId.Value = Convert.ToInt32((_paramNodes[2] as PBaseType).ValueObject);
            PString sAcctNo = new PString("sAcctNo");
            StringBuilder sSQL = new StringBuilder();

            if (listName.Value == "OffsetNickName")
            {
                sSQL.AppendFormat(string.Format(@"
                        Select	acct_no
						From	{0}empl_gl_acct_nickname 
						Where	nickname = {1}
                        And     Empl_id = {2}
                        And     status Not In ('Closed', 'Inactive')", DbPrefix, selectedListItem.SqlString, nEmplId.SqlString));

                if (ExecSqlImmediateInto(dbHelper, sSQL.ToString(), true, false, sAcctNo))
                {
                    (_paramNodes[3] as PBaseType).ValueObject = sAcctNo.Value;
                }
            }

        }
        
        private void GetAutoCompleteList(IDbHelper dbHelper)
        {            
            
            PString listName = new PString("listName");
            listName.Value = Convert.ToString((_paramNodes[0] as PBaseType).ValueObject);
            PInt nEmplId = new PInt("nEmplId");
            nEmplId.Value = Convert.ToInt32((_paramNodes[1] as PBaseType).ValueObject);

            PString autoList = new PString("autoList");
            //autoList.Value = Convert.ToString((_paramNodes[2] as PBaseType).ValueObject);

            StringBuilder sSQL = new StringBuilder();
            if (listName.Value == "OffsetNickName")
            {
                //NickNameList.SetValueToNull();
                PString offsetNickName = new PString("offsetNickName");
                sSQL.AppendFormat(string.Format(@"
                            Select	nickname
						    From	{0}empl_gl_acct_nickname
						    Where	nickname is not null
                            And     Empl_id = {1}
                            And     status Not In ('Closed', 'Inactive')", DbPrefix, nEmplId.SqlString));

                if (ExecSqlImmediateInto(dbHelper, sSQL.ToString(), true, true, offsetNickName))
                {
                    autoList.Value = autoList.Value + offsetNickName.Value;
                    //NickName.Add(offsetNickName.Value);
                    while (ExecSqlGetNextRow(true, offsetNickName))
                    {
                        autoList.Value = autoList.Value + "|" + offsetNickName.Value;
                        //OffsetNickName.Add(offsetNickName.Value);
                    }
                }
            }
            (_paramNodes[2] as PBaseType).ValueObject = autoList.Value;
        }
#endregion
		//Vidya - I have to add the function here code as I need access to ExecSqlImmediateInto function which
		//is in TLHelper object in order to avoid circular reference between gbHelper and TlHelper.
		#region GetAcctRate
		public decimal GetAcctRate(IDbHelper dbHelper, DateTime pdtTargetDt, string psAcctType, string psAcctNo, string psDepLn)
		{
			StringBuilder selectSql = new StringBuilder();
			string[] Values = {dbHelper.PhoenixDbName, pdtTargetDt.ToShortDateString(), psAcctNo.Trim(), psAcctType.Trim(), psDepLn.Trim()};

			PDecimal rnCurrRate = new PDecimal("A0");

			selectSql.AppendFormat(string.Format(@"
											Declare
												@nCurrentRate	decimal(6, 3) " + Environment.NewLine + @"
											Exec	{0}..psp_get_acct_rate
														'{1}',
														'{2}',
														'{3}',
														'{4}',
														@nCurrentRate output,
														NULL
											Select
												@nCurrentRate", Values) );

			if (ExecSqlImmediateInto( dbHelper, selectSql.ToString(), rnCurrRate) )
			{
				if (!rnCurrRate.IsNull)
					return rnCurrRate.Value;
				else
					return 0;
			}

			return rnCurrRate.Value;
		}
		#endregion

		#region GetGLLedgerMask
		private string GetGLLedgerMask(IDbHelper dbHelper, int levelNo)
		{
			PString LedgerMask = new PString("LedgerMask");
			string sqlCommand = @"SELECT	MASK
								FROM	{0}GL_ACCT_TOTAL_STRU
								WHERE	LEVEL_NO =" + levelNo.ToString();
			ExecSqlImmediateInto( dbHelper, sqlCommand, false, false, LedgerMask);
			if (!LedgerMask.IsNull)
				return LedgerMask.Value.Replace(@"#", "9");
			else
				return "";
		}
		#endregion GetGLLedgerMask

        //#73503
        #region CalcBusDate
        public void CalcBusDate(IDbHelper dbHelper, PBaseType InputDt, PBaseType PrevNext, PBaseType LeadDays, PBaseType BusDate)
        {
            StringBuilder sSQL = new StringBuilder();
            //5098 begin
            string leadDays = (LeadDays.IsNull)? "null": LeadDays.StringValue;
            string prevNext = (PrevNext.IsNull) ? "null" :  PrevNext.StringValue;

            if (dbHelper.CopyStatus == "D" || dbHelper.CopyStatus == "C")
            {
                sSQL = sSQL.AppendFormat(@"Declare	@dtBusDt	   datetime ,@nRC	int
											exec @nRC = {0}psp_calc_bus_days '{1}' , {2}, {3}, @dtBusDt output
											 select  @dtBusDt", dbHelper.DbPrefix, InputDt.DateTimeValue.ToShortDateString(), prevNext, leadDays);
                                             //select  @dtBusDt", dbHelper.DbPrefix, InputDt.DateTimeValue.ToShortDateString(), PrevNext.ValueObject, LeadDays.ValueObject);
            }
            else if (dbHelper.CopyStatus == "N" || dbHelper.CopyStatus == "B")
            {
                sSQL = sSQL.AppendFormat(@"Declare	@dtBusDt	  datetime,@nRC	int
									exec @nRC = psp_x_calc_bus_days '{0}' , {1}, {2}, @dtBusDt output
									select  @dtBusDt", InputDt.DateTimeValue.ToShortDateString(), prevNext, leadDays);
                                    // select  @dtBusDt", InputDt.DateTimeValue.ToShortDateString(), PrevNext.ValueObject, LeadDays.ValueObject);
            }

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, BusDate);

            //5098 end

        }
        #endregion

        #region Get DB Release
        public void GetDbRelease(IDbHelper dbHelper, PBaseType DbRelease)    //#80674
        {
            StringBuilder sSQL = new StringBuilder();
            string dbPrefix = dbHelper.PhoenixDbName + "..";
            if (dbHelper.IsPrimaryDbAvailable)
            {
                sSQL = sSQL.AppendFormat(@"Select db_release
                From {0}PC_CONTROL", dbPrefix, DbRelease);

                ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, DbRelease);
            }
        }
        #endregion

        #region GetRate
        public void GetRate(IDbHelper dbHelper, PBaseType IndexId, PBaseType TargetDate, PBaseType Rate)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select rate
                From {0}AD_GB_RATE_HISTORY
                Where index_id = {1}
                And effective_dt <=  '{2}'
                And status = '{3}'
                Order By effective_dt desc, rate_id desc", dbHelper.DbPrefix, IndexId.ValueObject, TargetDate.DateTimeValue.ToShortDateString(), GlobalVars.Instance.ML.Active);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, Rate);

        }
        #endregion

        #region GetFromAdLnControl
        private void GetFromAdLnControl(IDbHelper dbHelper, PBaseType AvailBal, PBaseType PostBal, PBaseType CashAccrual)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select avail_bal, post_bal, cash_accrual
                From {0}AD_LN_CONTROL", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, AvailBal, PostBal, CashAccrual);

        }
        #endregion

        #region GetFromAdLnUmbControl
        private void GetFromAdLnUmbControl(IDbHelper dbHelper, PBaseType CashAccrual)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select cash_accrual
                From {0}AD_LN_UMB_CONTROL", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, CashAccrual);

        }
        #endregion

        #region GetFromAdDpControl
        private void GetFromAdDpControl(IDbHelper dbHelper, PBaseType AvailBal, PBaseType PostBal, PBaseType ColBal)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select avail_bal, post_bal, coll_bal
                From {0}AD_DP_CONTROL", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, AvailBal, PostBal, ColBal);

        }
        #endregion

        #region GetFromAdSdControl
        private void GetFromAdSdControl(IDbHelper dbHelper, PBaseType CashAccrual)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select cash_accrual
                From {0}AD_SD_CONTROL", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, CashAccrual);
        }
        #endregion

        #region GetFromHcHc
        private void GetFromHcHc(IDbHelper dbHelper, PBaseType MultiCurrency,
            PBaseType HoldingCompany, PBaseType CallReportType)
        {
            if (dbHelper.IsPrimaryDbAvailable == false)
                return;
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select isnull(multi_currency, 'N'),
                call_report_type
                From {0}..HC_HC", dbHelper.PhoenixDbName);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, MultiCurrency, CallReportType);

            if (MultiCurrency.IsNull)
                HoldingCompany.Value = GlobalVars.Instance.ML.N;
            else
                HoldingCompany.Value = GlobalVars.Instance.ML.Y;
            //
            if (HoldingCompany.Value != GlobalVars.Instance.ML.Y)
            {
                sSQL.Remove(0, sSQL.Length);
                sSQL = sSQL.AppendFormat(@"Select call_report_type
                    From {0}AD_GB_BANK", dbHelper.DbPrefix);
                ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, CallReportType);
            }
        }
        #endregion

        #region GetFromAdGbBankControl
        private void GetFromAdGbBankControl(IDbHelper dbHelper, PBaseType PhoneMask, PBaseType PhoneMaskLength,
            PBaseType NoDaysFuture, PBaseType NoDaysBack, PBaseType DefaultTIN, PBaseType FormatTIN,
            PBaseType DefaultPhone, PBaseType FormatPhone, PBaseType DefaultZIP, PBaseType FormatZIP,
            PBaseType PhoenixJobCode, PBaseType IntlCountryCode, PBaseType AcctTypeSearch,
            PBaseType DatabaseLanguageId, PBaseType AchDRLeadDays, PBaseType AchCRLeadDays, PBaseType RequirePrenote,
            PBaseType PrenoteLeadDays, PBaseType CheckNSF, PBaseType GenSecClsRel, PBaseType NetworkEmail,
            PBaseType BankID, PBaseType ViewAudits)
        {
            StringBuilder sSQL = new StringBuilder();

            sSQL = sSQL.AppendFormat(@"Select view_audit,			no_days_future,
                no_days_back,			default_tin_format,
                custom_tin_format,		default_phone_format,
                custom_phone_format,	default_zip_format,
                custom_zip_format,		phoenix_job_code,
                intl_country_code, 		acct_type_search,
                def_corr_lang_id,
                ach_dr_lead_days,		ach_cr_lead_days,
                require_prenote,		prenote_lead_days,
                check_nsf,			gen_sec_cls_rel,
                network_email,		bank_id
                From {0}AD_GB_BANK_CONTROL", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false,
                    ViewAudits, NoDaysFuture,
                    NoDaysBack, DefaultTIN, FormatTIN, DefaultPhone, FormatPhone, DefaultZIP,
                    FormatZIP, PhoenixJobCode, IntlCountryCode, AcctTypeSearch, DatabaseLanguageId,
                    AchDRLeadDays, AchCRLeadDays, RequirePrenote, PrenoteLeadDays, CheckNSF,
                    GenSecClsRel, NetworkEmail, BankID);
            //
            if (!DefaultPhone.IsNull)
            {
                if (DefaultPhone.Value == "S")
                {
                    PhoneMask.ValueObject = "(999) 999-9999";
                    PhoneMaskLength.ValueObject = 14;
                }
                else if (DefaultPhone.Value == "C" && !FormatPhone.IsNull)
                {
                    PhoneMask.ValueObject = FormatPhone.Value;
                    PhoneMaskLength.ValueObject = FormatPhone.StringValue.Length;
                }
                else
                {
                    PhoneMaskLength.ValueObject = 20;
                }
            }
        }
        #endregion

        #region GenerateAccount
        private bool GenerateAccount(IDbHelper dbHelper, PBaseType ModStyle, PBaseType DepLoan,
                    PBaseType AcctType, PBaseType ApplType, PBaseType AcctNo, PBaseType NextAcct)
        {
            StringBuilder sSQL = new StringBuilder();
            PString SaveAcctNo = new PString("SaveAcctNo");
            int modulus = 0;
            int count = 0;
            string nextAccount = "";
            string depLoan = DepLoan.Value.ToString();
            string acctType = AcctType.Value.ToString();
            string applType = ApplType.Value.ToString();
            string modStyle = ModStyle.Value.ToString();

            //
            if (DepLoan.IsNull || AcctType.IsNull || ApplType.IsNull)
            {
                NextAcct.Value = AcctNo.Value;
                return true;
            }
            depLoan = depLoan.Trim();
            acctType = acctType.Trim();
            applType = applType.Trim();
            modStyle = modStyle.Trim();
            //
            try
            {
                if ((modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD10(001)")) ||
                    (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD10(007)")) ||
                    (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD10(ABA)")))
                    modulus = 10;
                else if ((modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(003)")) ||
                    (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(005)")) ||
                    (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(AUS)")))
                    modulus = 11;
                else if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "Sequential"))
                    modulus = -1;
                else
                    return false;

                if (depLoan == GlobalVars.Instance.ML.DP ||
                    depLoan == GlobalVars.Instance.ML.LN)
                {
                    sSQL = sSQL.AppendFormat(@"Select ltrim(rtrim(p.acct_next_no))
                From {0}ad_gb_acct_type c,
                     {0}ad_gb_appl_type p
                Where acct_type = '{1}'
                And c.appl_type = p.appl_type", dbHelper.DbPrefix, AcctType.Value);
                }
                else if (DepLoan.Value == GlobalVars.Instance.ML.CM)
                {
                    sSQL = sSQL.AppendFormat(@"Select ltrim(rtrim(p.acct_next_no))
                From {0}ad_cm_acct_type c,
                     {0}ad_gb_appl_type p
                Where acct_type = '{1}'
                And c.appl_type = p.appl_type", dbHelper.DbPrefix, AcctType.Value);
                }
                else if (DepLoan.Value == GlobalVars.Instance.ML.SD)
                {
                    sSQL = sSQL.AppendFormat(@"Select ltrim(rtrim(p.acct_next_no))
                From {0}ad_sd_acct_type c,
                     {0}ad_sd_appl_type p
                Where acct_type = '{1}'
                And c.appl_type = p.appl_type", dbHelper.DbPrefix, AcctType.Value);
                }
                // Begin WI#12289 - Handle Generation for DP_UMB Plan Numbers
		else if (DepLoan.Value.ToString() == "UDP")
		{
		    sSQL = sSQL.AppendFormat(@"Select ltrim(rtrim(umb_next_no))
		From {0}ad_dp_control", dbHelper.DbPrefix);
		}
                // End WI#12289
                else //there's no account
                {
                    sSQL = sSQL.AppendFormat(@"Select ltrim(rtrim(umb_next_no))
                From {0}ad_{1}_control", dbHelper.DbPrefix, DepLoan.Value);
                }
                ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, SaveAcctNo);
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Failed to get a new account number with SQL error in the select!");
                return false;
            }
            #region #02512
            //Reset value of sSQL its
            AcctNo.Value = SaveAcctNo.Value;
            sSQL.Remove(0,sSQL.Length);
            #endregion
            //
            if (SaveAcctNo.IsNull)
                SaveAcctNo.Value = "1"; //! this is the first account?!
            try
            {
                if (ModAccount(modulus, ModStyle.StringValue, SaveAcctNo.StringValue, ref nextAccount)) //! generate the new one
                {
                    NextAcct.Value = nextAccount;

                    if (depLoan == GlobalVars.Instance.ML.DP ||
                        depLoan == GlobalVars.Instance.ML.LN ||
                        depLoan == GlobalVars.Instance.ML.CM)
                        sSQL = sSQL.AppendFormat(@"update
				                {0}ad_gb_appl_type
			                set
				                acct_next_no = '{1}'
			                where appl_type = '{2}'
                            and acct_next_no = '{3}'", dbHelper.DbPrefix, NextAcct.Value,
                                                         ApplType.Value, SaveAcctNo.Value);
                    else if (DepLoan.Value == GlobalVars.Instance.ML.SD)
                        sSQL = sSQL.AppendFormat(@"update
				                {0}ad_sd_appl_type
			                set
				                acct_next_no = '{1}'
			                where appl_type = '{2}'
                            and acct_next_no = '{3}'", dbHelper.DbPrefix, NextAcct.Value,
                                                         ApplType.Value, SaveAcctNo.Value);
                    else
                        sSQL = sSQL.AppendFormat(@"update
				                {0}{1}
			                set
				                umb_next_no = '{2}'
			                where umb_next_no = '{3}'", dbHelper.DbPrefix, "ad_" + StringHelper.StrRightX(DepLoan.StringValue, 2) + "_control", NextAcct.Value,
                                                         SaveAcctNo.Value);
                    count = dbHelper.ExecuteNonQuery(sSQL.ToString());
                    if (count != 1)
                    {
                        System.Diagnostics.Trace.WriteLine("Failed to generate new " + ModStyle.Value + " for: " + AcctType.Value + " - " + NextAcct.Value);
                        return false;
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine("Successfully generated new " + ModStyle.Value + " for: " + AcctType.Value + " - " + NextAcct.Value);
                        return true;
                    }
                }
            }
            catch
            {
                System.Diagnostics.Trace.WriteLine("Failed to get a new account number with SQL error in the update!");
                return false;
            }
            return true;
        }
        #endregion

        #region CreateViewAudit
        private bool CreateViewAudit(IDbHelper dbHelper, bool isPrimaryDb)
        {
            if (_paramNodes.Count == 8 && dbHelper.IsPrimaryDbAvailable) //WI-4459
            {
                PBaseType dtIn = _paramNodes[0] as PBaseType;
                PBaseType dtOut = _paramNodes[1] as PBaseType;
                PBaseType emplId = _paramNodes[2] as PBaseType;
                PBaseType screenId = _paramNodes[3] as PBaseType;
                PBaseType acctType = _paramNodes[4] as PBaseType;
                PBaseType acctNo = _paramNodes[5] as PBaseType;
                PBaseType application = _paramNodes[6] as PBaseType;
                PBaseType screenPtid = _paramNodes[7] as PBaseType;


                //
                //dtIn, dtOut, emplId, screenId,
                //acctType, acctNo, application, screenPtid
                string sSQL = string.Format( "{0}..psp_audit_view {1},{2},{3},{4},{5},{6},{7},{8}",
                    dbHelper.PhoenixDbName, dtIn.SqlString, dtOut.SqlString, emplId.SqlString, screenId.SqlString,
                    acctNo.SqlString, acctType.SqlString, application.SqlString, screenPtid.SqlString);
                try
                {
                    dbHelper.ExecuteNonQuery(sSQL);
                }
                catch (Exception e)
                {
                    CoreService.LogPublisher.LogDebug(e.ToString());
                }




            }
            return true;
        }
        #endregion

        #region GetFromGLControl
        private void GetFromGLControl(IDbHelper dbHelper, PBaseType RestrictGlAccess,
            PBaseType GlAcctNoFormat) //Selva-New
        {
            StringBuilder sSQL = new StringBuilder();
            PString MultipleLevels = new PString("MultipleLevels");
            PString Mask = new PString("MultipleLevels");
            PString StructureType = new PString("MultipleLevels");
            PInt Position = new PInt("MultipleLevels");
            PInt RowVersion = new PInt("RowVersion");

            PInt dummy = new PInt("dummy"); //#05060

            int position = 0;
            string tempMask = "";
            string gLAcctFormatWild = "";
            string gLAcctFormatExternal = "";
            string ledgerNo = "";
            string tempMask1 = "";

            sSQL = sSQL.AppendFormat(@"Select isnull(gl_access, 'N'),
                isnull(multiple_levels, 'N')
                From {0}gl_control", dbHelper.DbPrefix);

            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, RestrictGlAccess,
                MultipleLevels);
            //
            sSQL.Remove(0, sSQL.Length);
            //
            if (MultipleLevels.StringValue != GlobalVars.Instance.ML.Y)
            {
                sSQL = sSQL.AppendFormat(@"Select mask, position,
                structure_type, row_version,
                0
                From {0}GL_ACCT_STRUCTURE
                UNION
                Select mask, 10001,
                null, row_version,
                1
                From {0}GL_ACCT_TOTAL_STRU
                Where level_no = 100
                Order By 5,2", dbHelper.DbPrefix);

                if (ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, true, Mask,
                    Position, StructureType, RowVersion
                    , dummy   //#05060 - we are expecting 5 columns
                    ))
                {
                    do
                    {
                        position = position + 1;
                        tempMask = Mask.StringValue;
                        //create GL Account mask that allows branch wild carding
                        if (!StructureType.IsNull)
                            StructureType.Value = StructureType.Value.Trim();
                        if (StructureType.Value == CoreService.Translation.GetListItemX(ListId.StructureType, "Branch"))
                            Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "!");
                        else
                            Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "9");
                        if (position > 1)
                            GlAcctNoFormat.Value = GlAcctNoFormat.Value + "-";
                        GlAcctNoFormat.Value = GlAcctNoFormat.Value + Mask.Value;
                        //create GL Account mask that allows wild carding up to ledger position
                        Mask.Value = tempMask;
                        if (Position.Value != 10001)
                            Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "!");
                        else
                            Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "9");
                        if (position > 1)
                            gLAcctFormatWild = gLAcctFormatWild + "-";
                        gLAcctFormatWild = gLAcctFormatWild + Mask.Value;
                        gLAcctFormatExternal = GlAcctNoFormat.Value.ToString();
                        //
                    } while (ExecSqlGetNextRow());
                }
            }
            else
            {
                sSQL = sSQL.AppendFormat(@"Select mask, position,
                structure_type, row_version,
                0
                From {0}GL_ACCT_STRUCTURE
                UNION
                Select mask, 10001,
                null, row_version,
                1
                From {0}GL_ACCT_TOTAL_STRU
                Where level_no = 100
                Order By 5,2", dbHelper.DbPrefix);

                if (ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, true, Mask,
                    Position, StructureType, RowVersion
                    , dummy   //#05060 - we are expecting 5 columns
                    ))
                {
                    do
                    {
                        position = position + 1;
                        tempMask = Mask.StringValue;
                        if (Position.Value == 10001)
                        {
                            tempMask1 = Mask.StringValue;
                            ledgerNo = Mask.StringValue;
                        }
                        else
                        {
                            //create GL Account mask that allows branch wild carding
                            if (!StructureType.IsNull)
                                StructureType.Value = StructureType.Value.Trim();
                            if (StructureType.Value == CoreService.Translation.GetListItemX(ListId.StructureType, "Branch"))
                                Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "!");
                            else
                                Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "9");
                            if (position > 1)
                                GlAcctNoFormat.Value = GlAcctNoFormat.Value + "-";
                            GlAcctNoFormat.Value = GlAcctNoFormat.Value + Mask.Value;
                            //create GL Account mask that allows wild carding up to ledger position
                            Mask.Value = tempMask;
                            if (Position.Value != 10001)
                                Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "!");
                            else
                                Mask.Value = StringHelper.StrSubstitute(Mask.Value, "#", "9");
                            if (position > 1)
                                gLAcctFormatWild = gLAcctFormatWild + "-";
                            gLAcctFormatWild = gLAcctFormatWild + Mask.Value;
                        }
                        //
                    } while (ExecSqlGetNextRow());
                }
                //
                gLAcctFormatExternal = GlAcctNoFormat.Value + "-" + StringHelper.StrSubstitute(ledgerNo, "#", "9");
                if (!GlAcctNoFormat.IsNull)
                    GlAcctNoFormat.Value = GlAcctNoFormat.Value + "-" + StringHelper.StrRepeatX("!", 20);
                if (gLAcctFormatWild != string.Empty)
                    gLAcctFormatWild = gLAcctFormatWild + "-" + StringHelper.StrRepeatX("!", 20);
            }
        }
        #endregion

        // Begin #75763
        #region Audit Functions
        private class AuditElements
        {
            public string Description;
            public int ScreenId;
            public decimal ScreenPtid;
            public string TableName;
            public string ColumnName;
            public decimal Ptid;
            public string NewValue;
            public string ActualNewValue;
            public string PrevValue;
            public string ActualPrevValue;
        }

        private ArrayList auditArray;

        #region #76404

        #region Commented Method
        //public void CreateAuditEntry(IDbHelper dbHelper, BusObjectBase busObj, string description, int screenId,
        //decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
        //string actualNewValue, string prevValue, string actualPrevValue)
        //{
        //    string acctType = null;
        //    string acctNo = null;
        //    string applCode = null;
        //    AuditDef _auditDef = new AuditDef(busObj, false);
        //    _auditDef.DefaultAuditInfo();
        //    if (_auditDef.AcctType != null)
        //        acctType = Convert.ToString(_auditDef.AcctType.Value);
        //    if (_auditDef.AcctNo != null)
        //        acctNo = Convert.ToString(_auditDef.AcctNo.Value);
        //    if (_auditDef.ApplCode != null)
        //        applCode = _auditDef.ApplCode;
        //    //_auditDef.CreateAuditEntry(dbHelper, BusGlobalVars.CurrentBranchNo, applCode, false, BusGlobalVars.EmployeeId,
        //    //    description, screenId, Convert.ToInt32(screenPtid), Convert.ToInt32(ptid), tableName, acctNo, acctType,columnName,
        //    //    prevValue, newValue, actualPrevValue, actualNewValue );
        //    _auditDef.CreateAuditEntry(dbHelper, BusGlobalVars.CurrentBranchNo, applCode, false, BusGlobalVars.EmployeeId,
        //    description, screenId, Convert.ToInt32(screenPtid), Convert.ToInt32(ptid), tableName, acctNo, acctType, columnName,
        //    newValue, prevValue, actualNewValue, actualPrevValue);
        //}
        #endregion

		// WI#34412  overload method for back compat
		public void CreateAuditEntry(IDbHelper dbHelper, BusObjectBase busObj, string description, int screenId,
		decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
		string actualNewValue, string prevValue, string actualPrevValue)	
		{

			CreateAuditEntry(dbHelper, busObj, description, screenId, screenPtid, tableName, 
				columnName, ptid, newValue, actualNewValue, prevValue, actualPrevValue, null, null, false);
		}

		public void CreateAuditEntry(IDbHelper dbHelper, BusObjectBase busObj, string description, int screenId,
        decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
		string actualNewValue, string prevValue, string actualPrevValue, bool isNew)	// WI#34412  add isNew optional parameter
        {

            CreateAuditEntry(dbHelper, busObj, description, screenId, screenPtid, tableName, 
				columnName, ptid, newValue, actualNewValue, prevValue, actualPrevValue, null, null, isNew);	// WI#34412  add isNew
        }

		// WI#34412  overload method for back compat
		public void CreateAuditEntry(IDbHelper dbHelper, BusObjectBase busObj, string description, int screenId,
			 decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
			 string actualNewValue, string prevValue, string actualPrevValue, string acctType, string acctNo)	// WI#34412  add isNew optional parameter
		{
			CreateAuditEntry(dbHelper, busObj, description, screenId, screenPtid, tableName, 
				columnName, ptid, newValue, actualNewValue, prevValue, actualPrevValue, acctType, acctNo, false);
		}

        public void CreateAuditEntry(IDbHelper dbHelper, BusObjectBase busObj, string description, int screenId,
             decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
			 string actualNewValue, string prevValue, string actualPrevValue, string acctType, string acctNo, bool isNew)	// WI#34412  add isNew optional parameter
        {
            string applCode = null;

			AuditDef _auditDef = new AuditDef(busObj, isNew);	// WI#34412 add isNew

            _auditDef.DefaultAuditInfo();

            if (_auditDef.AcctType != null)
                acctType = (acctType == null) ? Convert.ToString(_auditDef.AcctType.Value) : acctType;

            if (_auditDef.AcctNo != null)
                acctNo = (acctNo == null) ? Convert.ToString(_auditDef.AcctNo.Value) : acctNo;

            if (_auditDef.ApplCode != null)
                applCode = _auditDef.ApplCode;

            //_auditDef.CreateAuditEntry(dbHelper, BusGlobalVars.CurrentBranchNo, applCode, false, BusGlobalVars.EmployeeId,
            //    description, screenId, Convert.ToInt32(screenPtid), Convert.ToInt32(ptid), tableName, acctNo, acctType,columnName,
            //    prevValue, newValue, actualPrevValue, actualNewValue );
            #region #2759
            //_auditDef.CreateAuditEntry(dbHelper, BusGlobalVars.CurrentBranchNo, applCode, false, BusGlobalVars.EmployeeId,
            //   description, screenId, Convert.ToInt32(screenPtid), Convert.ToInt32(ptid), tableName, acctNo, acctType, columnName,
            //   newValue, prevValue, actualNewValue, actualPrevValue);

			// WI#34412 add isNew instead of false
            _auditDef.CreateAuditEntry(dbHelper, BusGlobalVars.CurrentBranchNo, applCode, isNew, BusGlobalVars.EmployeeId,
               description, screenId, Convert.ToDecimal(screenPtid), Convert.ToDecimal(ptid), tableName, acctNo, acctType, columnName,
               newValue, prevValue, actualNewValue, actualPrevValue);


            #endregion

        }


        #endregion

        public void AuditListAdd(string description, int screenId,
                decimal screenPtid, string tableName, string columnName, decimal ptid, string newValue,
                string actualNewValue, string prevValue, string actualPrevValue)
        {

            AuditElements auditElements = new AuditElements();

            auditElements.Description = description;
            auditElements.ScreenId = screenId;
            auditElements.ScreenPtid = screenPtid;
            auditElements.TableName = tableName;
            auditElements.ColumnName = columnName;
            auditElements.Ptid = ptid;
            auditElements.NewValue = newValue;
            auditElements.ActualNewValue = actualNewValue;
            auditElements.PrevValue = prevValue;
            auditElements.ActualPrevValue = actualPrevValue;

            if (auditArray == null)
                auditArray = new ArrayList();
            auditArray.Add(auditElements);

        }

        public void AuditListNew(string description, int screenId,
                decimal screenPtid, string tableName, decimal ptid)
        {
            AuditListAdd(description, screenId,
                screenPtid, tableName, "STATUS", ptid, GlobalVars.Instance.ML.Active, GlobalVars.Instance.ML.Active,
                GlobalVars.Instance.ML.New, GlobalVars.Instance.ML.New);
        }

        public void AuditListClear()
        {
            if (auditArray != null)
                auditArray.Clear();
        }

        public void AuditListPost(IDbHelper dbHelper, BusObjectBase busObj)
        {
            foreach (AuditElements auditElements in auditArray)
            {
                CreateAuditEntry(dbHelper, busObj,
                    auditElements.Description,
                    auditElements.ScreenId,
                    auditElements.ScreenPtid,
                    auditElements.TableName,
                    auditElements.ColumnName,
                    auditElements.Ptid,
                    auditElements.NewValue,
                    auditElements.ActualNewValue,
                    auditElements.PrevValue,
                    auditElements.ActualPrevValue);
            }
        }
        #endregion
        // End #75763

        // Begin #01031
        #region Other Functions
        public bool UDFCopyUpdate( BusObjectBase busObj, string type, decimal update, decimal copy,
            string acctType, string acctNo, decimal rimNo, decimal relRimNo, decimal serviceID,
            decimal finStmtItemID, ref string status )
		{
			string sType = null;
			string sRimNo = null;
			string sAcctType = null;
			string sAcctNo = null;
			string sCopy = null;
			string sRelRimNo = null;
			string sServiceID = null;
			string sFinStmtItemID = null;
			string sSQL = null;
			string sUpdate = null;
			decimal nMessageId = 0;
            decimal[] gnaTemp = new decimal[1];
		    string[] gsaTemp = new string[2];
		    PString gsTemp = new PString("gsTemp");

			if ( type == null )
			{
				sType = "null";
			}

			else
			{
				sType = "'" + type + "'";
			}

			if ( acctType == null )
			{
				sAcctType = "null";
			}

			else
			{
				sAcctType = "'" + acctType + "'";
			}

			if ( acctNo == null )
			{
				sAcctNo = "null";
			}

			else
			{
				sAcctNo = "'" + acctNo + "'";
			}

			if ( rimNo == Decimal.MinValue )
			{
				sRimNo = "null";
			}

			else
			{
                sRimNo = NumberHelper.NumberToStringX((rimNo == Decimal.MinValue ? 0 : rimNo), 0);
			}

			if ( relRimNo == Decimal.MinValue )
			{
				sRelRimNo = "null";
			}

			else
			{
                sRelRimNo = NumberHelper.NumberToStringX((relRimNo == Decimal.MinValue ? 0 : relRimNo), 0);
			}

			if ( serviceID == Decimal.MinValue )
			{
				sServiceID = "null";
			}

			else
			{
                sServiceID = NumberHelper.NumberToStringX((serviceID == Decimal.MinValue ? 0 : serviceID), 0);
			}

			if ( finStmtItemID == Decimal.MinValue )
			{
				sFinStmtItemID = "null";
			}

			else
			{
                sFinStmtItemID = NumberHelper.NumberToStringX((finStmtItemID == Decimal.MinValue ? 0 : finStmtItemID), 0);
			}

			if ( (copy == Decimal.MinValue ? 0 : copy )  == 3 || (copy == Decimal.MinValue ? 0 : copy )  == 4  )
			{
				nMessageId = (copy == Decimal.MinValue ? 0 : copy ) ;

				copy = 0;
			}

            sCopy = NumberHelper.NumberToStringX((copy == Decimal.MinValue ? 0 : copy), 0);

            sUpdate = NumberHelper.NumberToStringX((update == Decimal.MinValue ? 0 : update), 0);

			sSQL =
			string.Format(@"declare
			@psType				char(3) ,
			@pnUpdate			tinyint ,
			@pnCopy				tinyint,
			@psAcctType			char(3) ,
			@psAcctNo			varchar(19) ,
			@pnRimNo			int ,
			@pnRelRimNo			int ,
			@pnServiceId			int ,
			@pnFinStmtItemId		int ,
			@rsStatus			varchar(19) ,
			@nMissingFieldId		int  ,
			@nRC int

			select @psType = " + sType + @",
			@psAcctType = " + sAcctType + @",
			@psAcctNo = " + sAcctNo + @",
			@pnUpdate = " + sUpdate + @",
			@pnCopy	= 	" + sCopy + @",
			@pnRimNo = " + sRimNo + @",
			@pnRelRimNo = " + sRelRimNo + @",
			@pnServiceId = " + sServiceID + @",
			@pnFinStmtItemId	= " + sFinStmtItemID + @"

			exec @nRC = {0}psp_get_status
			@psType	,
			@pnUpdate,
			@pnCopy,
			@psAcctType,
			@psAcctNo,
			@pnRimNo,
			@pnRelRimNo,
			@pnServiceId,
			@pnFinStmtItemId	,
			@rsStatus	output,
			@nMissingFieldId	 output

			select  @nRC , rtrim(@rsStatus), @nMissingFieldId",DbPrefix );

            PDecimal returnCode = new PDecimal("returnCode");
            PString fieldStatus = new PString( "fieldStatus" );
            PDecimal missingFieldId = new PDecimal("missingFieldId");

            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, false, false, returnCode, fieldStatus, missingFieldId))
			{
				return false;
			}

            status = fieldStatus.Value;

			if ( (copy == Decimal.MinValue ? 0 : copy )  == 0 )
			{
                if (!missingFieldId.IsNull)
				{
                    sSQL = string.Format(" select field_label   from {0}ad_gb_user_defined where user_defined_id = {1} ", DbPrefix, missingFieldId.Value );

					ExecSqlImmediateInto( CoreService.DbHelper, sSQL,gsTemp ) ;

					//ArrayHelper.ArraySetUpperBound( gsaTemp, 1, -1 ) ;

                    gsaTemp[0] = gsTemp.Value;

                    gsaTemp[1] = gsTemp.Value;

					if ( (nMessageId == Decimal.MinValue ? 0 : nMessageId )  == 4  )
					{
						if ( type == "RM" )
						{
                            if ( busObj != null )
							    busObj.Messages.AddWarning( 9836, null, String.Empty ) ;
						}

						else
						{
							return false;
						}
					}

					else if ( (nMessageId == Decimal.MinValue ? 0 : nMessageId )  != 0 )
					{
                        if (busObj != null)
                            busObj.Messages.AddWarning(9835, null, gsaTemp);
                        //return false;
					}
				}
			}

			return true;
		}

        //Begin #76270
        public bool UpdateStmtStatus(decimal pnCombStmtId, int pnRimNo, ref string rsStatus)
        {
            string sSQL = null;
            PString sStatus = new PString("sStatus");
            PInt nError = new PInt("nError");
			PInt nRC = new PInt("nRC");	// WI#30074

            CoreService.DbHelper.BeginSavePoint("UpdateStmtStatus");

            sSQL =
            string.Format(@"declare	@sStatus char(10),
				@nError	integer,
				@nRC	integer
			Exec @nRC = {0}PSP_STMT_UPD_STAT
				'" + DateHelper.FormatDate(Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate, false) + @"',
				" + NumberHelper.NumberToStringX((pnCombStmtId == Decimal.MinValue ? 0 : pnCombStmtId), 0) + @",
				" + NumberHelper.NumberToStringX((pnRimNo == Decimal.MinValue ? 0 : pnRimNo), 0) + @",
				@sStatus		OUTPUT,
				@nError		OUTPUT

			select	@sStatus, @nError", DbPrefix);

            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, false, false, sStatus, nError))
            {
                CoreService.DbHelper.RollBackSavePoint("UpdateStmtStatus");
                return false;
            }

            rsStatus = sStatus.Value;

            //nReturn = gnTemp.Value;

            if (nError.Value != 0)
            {
                CoreService.DbHelper.RollBackSavePoint("UpdateStmtStatus");
                return false;
            }

			// if status has changed to anything but active, and we are using phoenix db (proc doesn't exist in xapi, but will run during OV for cleanup)
			if (CoreService.DbHelper.IsPrimaryDbAvailable &&
				rsStatus != "Active")	// WI#30074
			{
				PString sqlError = new PString("SqlError");
				string sql = string.Format(@"Declare @nRC int, @nSqlError int
													exec @nRC = {0}..PSP_CLOSE_STMT_ANAL
																'{1}',	/* @pdtProcessDt */
																{2},	/* @pnEmplId */
																{3},	/* @pncombstmtid */
																1,		/* @pnDoAudit */
																'{4}',	/* @psnetworkuser */
																0,
																@nSqlError output
												select @nRC, @nSqlError",
																	CoreService.DbHelper.PhoenixDbName,
																	Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate,
																	Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId,
																	(pnCombStmtId == Decimal.MinValue ? "NULL" : pnCombStmtId.ToString()),
																	Phoenix.FrameWork.Shared.Variables.GlobalVars.ClientNetworkUser);

				ExecSqlImmediateInto(CoreService.DbHelper, sql, nRC, sqlError);

				if (nRC.IntValue != 0 || sqlError.IntValue != 0)
				{
					CoreService.DbHelper.RollBackSavePoint("UpdateStmtStatus");
					return false;
				}
			}
            //GbHelper.IntSqlCommit(Phoenix.Shared.Variables.GlobalVars.hSql);

            return true;
        }

        //End #76270

        #endregion
        // End #01031

        public bool GetLoanUndisbursedValues(IDbHelper dbHelper)
        {
            PBaseType paramAcctType = _paramNodes[0] as PBaseType;
            PBaseType paramAcctNo = _paramNodes[1] as PBaseType;
            PBaseType paramUndisbursed = _paramNodes[2] as PBaseType;
            PBaseType paramBorrowerFunds = _paramNodes[3] as PBaseType;
            PBaseType paramBorrowerNonMonFunds = _paramNodes[4] as PBaseType;
            PBaseType fieldConstruction = new PBaseType( "Construction");

            StringBuilder sSql = new StringBuilder();

            if( dbHelper.XmDbStatus == XmDbStatus.Day || dbHelper.XmDbStatus  == XmDbStatus.CopyBack)
            {
            sSql.AppendFormat( @"Select a.construction, d.undisbursed from {0}ln_acct a, {0}ln_display d
where a.acct_no = d.acct_no
and a.acct_type = d.acct_type
and a.acct_no = {1}
and a.acct_type = {2}", dbHelper.DbPrefix,  paramAcctNo.SqlString, paramAcctType.SqlString );
            }
            else
            {
                sSql.AppendFormat( @"Select a.construction, a.undisbursed from x_ln_display a
where a.acct_no = {0}
and a.acct_type = {1}",   paramAcctNo.SqlString, paramAcctType.SqlString );
            }
            ExecSqlImmediateInto(dbHelper, sSql.ToString(), false, false, fieldConstruction, paramUndisbursed);
            if( dbHelper.IsPrimaryDbAvailable &&  fieldConstruction.IsNull == false && fieldConstruction.StringValue == "Y" )
            {
                sSql.Length = 0; // Reset the SQL
                sSql.AppendFormat( @"Select a.borrower_funds, a.borrower_nonmon_funds from {0}ln_construction a
where a.acct_no = {1}
and a.acct_type = {2}",   dbHelper.DbPrefix,  paramAcctNo.SqlString, paramAcctType.SqlString );
                ExecSqlImmediateInto(dbHelper, sSql.ToString(), false, false, paramBorrowerFunds, paramBorrowerNonMonFunds);
            }

            return true;

            //try
            //{

            //    Phoenix.BusObj.Loan.LnAcct lnAcct = new Phoenix.BusObj.Loan.LnAcct();
            //    lnAcct.AcctType.Value = AcctType;
            //    lnAcct.AcctNo.Value = AcctNo;
            //    lnAcct.SelectAllFields = false;
            //    lnAcct.Construction.Selected = true;
            //    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, lnAcct);

            //    Phoenix.BusObj.Loan.LnDisplay lnDisplay = new Phoenix.BusObj.Loan.LnDisplay();
            //    lnDisplay.AcctType.Value = AcctType;
            //    lnDisplay.AcctNo.Value = AcctNo;
            //    lnDisplay.SelectAllFields = true;
            //    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, lnDisplay);

            //    if (!lnDisplay.Undisbursed.IsNull)
            //    {
            //        UndisbursedAmount = lnDisplay.Undisbursed.Value;
            //    }

            //    if (lnAcct.Construction.Value == "Y")
            //    {
            //        Phoenix.BusObj.Loan.LnConstruction lnConstruction = new Phoenix.BusObj.Loan.LnConstruction();
            //        lnConstruction.AcctType.Value = AcctType;
            //        lnConstruction.AcctNo.Value = AcctNo;
            //        lnConstruction.SelectAllFields = false;
            //        lnConstruction.BorrowerFunds.Selected = true;
            //        lnConstruction.BorrowerNonMonFunds.Selected = true;
            //        Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, lnConstruction);

            //        if (!lnConstruction.BorrowerFunds.IsNull)
            //        {
            //            BorrowerFunds = lnConstruction.BorrowerFunds.Value;
            //        }

            //        if (!lnConstruction.BorrowerNonMonFunds.IsNull)
            //        {
            //            BorrowerNonMonFunds = lnConstruction.BorrowerNonMonFunds.Value;
            //        }

            //        lnConstruction = null;
            //    }

            //    lnDisplay = null;
            //    lnAcct = null;
            //}
            //catch
            //{
            //    return false;
            //}

            //return true;
        }


        #region #03333
        private bool LaunchInterface(IDbHelper dbHelper)
        {
            if (_paramNodes.Count >= 3 )
            {

                PInt rimNo = new PInt("RimNo");
                PString result = new PString("result");

                string interfaceTag;
                string interfaceValue;

                StringBuilder returnBisysFile = new StringBuilder();

                string bisysFileHeader = @"
<!DOCTYPE html PUBLIC ""-//W3C//DTD HTML 3.2//EN"">
<html>
    <head>
        <title>
	IRA Service Center prepopulation example
        </title>
    </head>
	<body>
		By Clicking the Submit Button you are posting information to the IRA Service Center.
		Once the post has taken place you will be directed to the IRA Service Center at which point you
		begin creating your transaction.  You will see that the submitted information will be pre-populated
		On the General Information page of the IRA Service Center questionnaire.

		<form action=""{0}"" method=""post"">
        ";
                string bisysFileData = "<input type=\"hidden\" name=\"{0}\" value=\"{1}\">\n";

                string bisysFileFooter = @"
					<input type=""submit"" name=""submit"" value=""submit"">
				</form>
			</body>
		</html>
        ";

                rimNo.ValueObject = (_paramNodes[2] as PBaseType).ValueObject;

                string sSQL = string.Empty;
                sSQL = string.Format(@"
			    Declare	@nRC	INT

			    Exec	@nRC = {0}..psp_eforms_bisys_data
					    '{1}' ,
					     {2}  ,
					     0,
					    NULL
			    Select	convert(varchar, @nRC)",
                     dbHelper.PhoenixDbName,
                     FrameWork.Shared.Variables.GlobalVars.SystemDate,
                     rimNo.SqlString);

                this.ExecSqlImmediateInto(dbHelper, sSQL,false ,true , result);

                do
                {
                    ParseInterfaceTags(result.Value, out  interfaceTag, out interfaceValue);
                    if (interfaceTag.Length > 0 && interfaceValue.Length > 0)
                    {
                        if (interfaceTag == "URL")
                        {
                            returnBisysFile.AppendFormat(bisysFileHeader, interfaceValue);
                            returnBisysFile.AppendLine();
                            continue;
                        }
                        returnBisysFile.AppendFormat(bisysFileData,interfaceTag, interfaceValue);
                        returnBisysFile.AppendLine();
                    }
                }
                while(ExecSqlGetNextRow(true , result ));

                // Validate last element.
                if (interfaceTag != "0") // rc Value
                {
                    this.Messages.AddError(9441, null, string.Empty );
                    return false;
                }

                // write footer
                returnBisysFile.AppendLine(bisysFileFooter);
                (_paramNodes[3] as PBaseType).ValueObject = returnBisysFile.ToString();
                return true;

            }
            return  true ;
        }

        private void ParseInterfaceTags(string inputString, out string interfaceTag, out string interfaceValue)
        {
            interfaceTag = string.Empty;
            interfaceValue = string.Empty;

            string[] dataToBeSplited = inputString.Split('~');
            if (dataToBeSplited.Length >= 2)
            {
                interfaceTag = dataToBeSplited[0];
                interfaceValue = dataToBeSplited[1];
            }
            else if (dataToBeSplited.Length == 1)
            {
                interfaceTag = dataToBeSplited[0];
            }


        }
        #endregion

        #region 146541
        public bool ValidateLoanInitDisc( string acctType, string acctNo, DateTime dtBeginDate)
        {            
            PString sSQL = new PString();
            PInt RetCode = new PInt("RetCode");
            PInt SqlError = new PInt("SqlError");
           

            PInt bIsEnabled = new PInt("bIsEnabled");
            PInt bIsInitialDiscLeadDaysValid = new PInt("bIsInitialDiscLeadDaysValid");
            PInt bIsFirstPmtAdjDtValid = new PInt("bIsFirstPmtAdjDtValid");

            #region sql
			// WI#24924 - turn off debugging ad "OUTPUT for @rbIsFirstPmtAdjDtValid
            sSQL.Value = string.Format(@"Declare @return_value Int, @rbIsEnabled bit, @rbIsInitialDiscLeadDaysValid bit, @rbIsFirstPmtAdjDtValid bit, @rnSQLError int
							                                        exec @return_value = {0}psp_validate_loan_initial_disc  'Online', 
                                                                        '{1}',
                                                                        '{2}',
                                                                         NULL,
                                                                         {3},
                                                                         NULL,							                                        
							                                            @rbIsEnabled OUTPUT,
							                                            @rbIsInitialDiscLeadDaysValid OUTPUT,
							                                            @rbIsFirstPmtAdjDtValid OUTPUT,
							                                            0,
							                                        @rnSQLError OUTPUT							                                        
							     select @return_value, @rbIsEnabled ,@rbIsInitialDiscLeadDaysValid,@rbIsFirstPmtAdjDtValid, @rnSQLError ", CoreService.DbHelper.DbPrefix, acctType, acctNo, (dtBeginDate == DateTime.MinValue  ? "NULL" : "'" + dtBeginDate.ToString() + "'"));


            #endregion

            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, false, false, RetCode, bIsEnabled, bIsInitialDiscLeadDaysValid, bIsFirstPmtAdjDtValid, SqlError))// == UserConstants.SQL_FetchData)
            {
                if (!CoreService.DbHelper.IsInTransaction)
                    CoreService.DbHelper.Rollback();
            }
            if (bIsEnabled.Value == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 174961


        public bool LocateReferral(int pnRimNo, string psAcctType, PSmallInt pnClassCode, string psDepLoan, int pnServiceId, PDateTime pdtEffDate, ref int pnWorkId, ref Int16 pnRsmIdRefWork)
        {
            string sSQL = null;
            PInt nError = new PInt("nError");
            PInt nRC = new PInt("nRC");
            PInt nWorkId = new PInt("nWorkId");
            PInt nRsmIdRefWork = new PInt("nRsmIdRefWork");

            CoreService.DbHelper.BeginSavePoint("LocateReferral");
            if (psDepLoan == "SER")
            {
                sSQL =
                               string.Format(@"
						Declare @nRC 	int, @rnWorkId int, @rnRsmIdRefWork smallint
						Exec @nRC = {0}psp_ss_locate_referral  {1},null,null,'{2}',{3},'{4}',0, @rnRsmIdRefWork output,@rnWorkId output
						Select @nRC,@rnRsmIdRefWork,@rnWorkId
					", DbPrefix, pnRimNo, psDepLoan, pnServiceId, pdtEffDate.Value);
            }
            else
            {
                sSQL =
                               string.Format(@"
						Declare @nRC 	int, @rnWorkId int, @rnRsmIdRefWork smallint
						Exec @nRC = {0}psp_ss_locate_referral  {1},'{2}',{3},'{4}',{5},'{6}',0, @rnRsmIdRefWork output,@rnWorkId output
						Select @nRC,@rnRsmIdRefWork,@rnWorkId
					", DbPrefix, pnRimNo, psAcctType, pnClassCode.SqlString, psDepLoan, pnServiceId, pdtEffDate.Value);
            }

            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, nRC, nRsmIdRefWork, nWorkId))
            {
                CoreService.DbHelper.RollBackSavePoint("LocateReferral");
                return false;
            }
            if (nRC.Value != 0)
            {
                CoreService.DbHelper.RollBackSavePoint("LocateReferral");
                return false;
            }
            if (!nWorkId.IsNull)
                pnWorkId = nWorkId.Value;
            if (!nRsmIdRefWork.IsNull)
                pnRsmIdRefWork = Convert.ToInt16(nRsmIdRefWork.Value);


            return true;
        }

        public bool InsSSProdAcct(int pnRimNo, string psAcctNo, string psAcctType, PDateTime pdtEffDate, string psDepLoan, PSmallInt pnClassCode, 
            Int16 pnBranchNo, PInt pnWorkId, PSmallInt pnRsmIdRefWork, PSmallInt pnRsmIdRefClose,PSmallInt pnMemberNo, PInt pnAcctId)
        {
            string sSQL = null;
            PString sStatus = new PString("sStatus");
            PInt nError = new PInt("nError");
            PInt nRC = new PInt("nRC");	// WI#30074

            CoreService.DbHelper.BeginSavePoint("InsSSProdAcct");
            sSQL =
                           string.Format(@"
						Declare @nRC 	int, @rnSqlError int
						Exec @nRC = {0}PSP_INS_SS_PROD_ACCT  {1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},0,{11},{12}
						Select @nRC
					", DbPrefix, pnRimNo, psAcctNo, psAcctType, pdtEffDate.Value, psDepLoan, pnClassCode.SqlString, pnBranchNo, pnWorkId.SqlString, 
                     pnRsmIdRefWork.SqlString, pnRsmIdRefClose.SqlString,pnMemberNo.SqlString, pnAcctId.Value < 0 ? "null" : pnAcctId.SqlString); /** #39577 **/

            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, nRC))
            {
                CoreService.DbHelper.RollBackSavePoint("InsSSProdAcct");
                return false;
            }
            if (nRC.Value != 0)
            {
                CoreService.DbHelper.RollBackSavePoint("InsSSProdAcct");
                return false;
            }

            return true;
        }

        /** Begin #39577 **/
        public bool InsSSProdAcct(int pnRimNo, string psAcctNo, string psAcctType, PDateTime pdtEffDate, string psDepLoan, PSmallInt pnClassCode, 
            Int16 pnBranchNo, PInt pnWorkId, PSmallInt pnRsmIdRefWork, PSmallInt pnRsmIdRefClose, PSmallInt pnMemberNo)
        {
            PInt pnAcctId = new PInt("AcctId");

            pnAcctId.Value = -1;

            return this.InsSSProdAcct(pnRimNo, psAcctNo, psAcctType, pdtEffDate, psDepLoan, pnClassCode, pnBranchNo, pnWorkId, pnRsmIdRefWork, 
                pnRsmIdRefClose, pnMemberNo, pnAcctId);
        }
        /** End #39577 **/


        public bool InsSSProdSvcs(int pnRimNo, int pnServiceId, string psAcctNo, string psAcctType, string psDepLoan, int pnDataSvcPtid, PInt pnWorkId, PSmallInt pnRsmIdRefWork)
        {
            string sSQL = null;
            PString sStatus = new PString("sStatus");
            PInt nError = new PInt("nError");
            PInt nRC = new PInt("nRC");	// WI#30074

            CoreService.DbHelper.BeginSavePoint("InsSSProdSvcs");
            sSQL = string.Format(@"
					Declare @nRC 	int
					Exec @nRC = {0}PSP_INS_SS_PROD_SVCS  {1},{2},'{3}','{4}','{5}',{6},{7},{8},{9},{10},0
					Select @nRC
				", DbPrefix, pnRimNo, pnServiceId, psAcctNo, psAcctType, psDepLoan, pnDataSvcPtid, Phoenix.FrameWork.Shared.Variables.GlobalVars.CurrentBranchNo, Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId, pnWorkId.SqlString, pnRsmIdRefWork.SqlString);
            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, nRC))
            {
                CoreService.DbHelper.RollBackSavePoint("InsSSProdSvcs");
                return false;
            }
            if (nRC.Value != 0)
            {
                CoreService.DbHelper.RollBackSavePoint("InsSSProdSvcs");
                return false;
            }

            return true;
        }

        public bool LocateReferral(int pnRimNo, string psAcctNo, string psAcctType, PSmallInt pnClassCode, string psAcctDepLoan, string psDepLoan, int pnServiceId, int pnDataSvcPtid, PDateTime pdtEffDate, string psInsSvcs)
        {
            int svcWorkId = 0;
            Int16 svcRsmIdRefWork = 0;
            PInt nWorkId = new PInt("nWorkId");
            PSmallInt nRsmIdRefWork = new PSmallInt("nRsmIdRefWork");
            if (!LocateReferral(pnRimNo, psAcctType, pnClassCode, psDepLoan, pnServiceId, pdtEffDate, ref svcWorkId, ref svcRsmIdRefWork))
            {
                return false;
            }
            if (svcWorkId != 0 && svcRsmIdRefWork != 0)
            {
                nWorkId.Value = svcWorkId;
                nRsmIdRefWork.Value = svcRsmIdRefWork;
            }
            if (psInsSvcs == "Y")
            {
                if (!InsSSProdSvcs(pnRimNo, pnServiceId, psAcctNo, psAcctType, psAcctDepLoan, pnDataSvcPtid, nWorkId, nRsmIdRefWork))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


        #region Get BiProductType
        public void CustAnalysisExists(IDbHelper dbHelper)    //#63936
        {
            PBaseType rimNo = _paramNodes[0] as PBaseType;
            PString Exists = new PString("Exists");
            string sSQL = null;
            PInt nRC = new PInt("nRC");
            sSQL =  string.Format(@"
						Declare @nRC 	int
						Exec @nRC = {0}psp_get_cust_analysis  {1},0
						Select @nRC
					", DbPrefix, rimNo.SqlString);
            if (!ExecSqlImmediateInto(CoreService.DbHelper, sSQL, nRC))
            {
                return;
            }
            if (nRC.Value == 1)
            {
                Exists.Value = "Y";
            }
            else
            {
                Exists.Value = "N";
            }
             (_paramNodes[1] as PBaseType).Value = Exists.Value;
        }
        #endregion
        #region Get BiProductType
        public void GetBiProductType(IDbHelper dbHelper, PBaseType BiProduct)    //#63936
        {
            StringBuilder sSQL = new StringBuilder();
            string dbPrefix = dbHelper.PhoenixDbName + "..";
            if (dbHelper.CopyStatus == "N" && !dbHelper.IsPrimaryDbAvailable)
            {
                sSQL = sSQL.AppendFormat(@"Select ISNULL(bi_product,'')  
                From {0}X_AD_GB_BANK", dbPrefix, BiProduct); //Task #84178
            }
            else
            {
                sSQL = sSQL.AppendFormat(@"Select ISNULL(bi_product,'')
                From {0}AD_GB_BANK", dbPrefix, BiProduct); //Task #84178
            }
            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), false, false, BiProduct);

        }
        #endregion
        // Begin #94549 - Get Current Posting Date based on drawerno and branch no.
        #region GetCurPostingDate
        public void GetCurPostingDate(IDbHelper dbHelper, PBaseType curPostDate, PBaseType branchNo, PBaseType drawerNo)
        {
            StringBuilder sSQL = new StringBuilder();
            sSQL.AppendFormat(@"
				  		    Select cur_posting_dt	From {0}tl_drawer
				  			Where branch_no = {1}
				  		    And   drawer_no	= {2}", dbHelper.DbPrefix, branchNo.Value, drawerNo.Value);
            ExecSqlImmediateInto(dbHelper, sSQL.ToString(), curPostDate);
            if (curPostDate.Value == null)
            {
                curPostDate.Value = DateTime.MinValue;
            }
            
            
        }
        // End #94549
        #endregion

    }

    //public enum UseDb
    //{
    //    /// <summary>
    //    /// use Primary Database
    //    /// </summary>
    //    Primary = 1,
    //    /// <summary>
    //    /// Use Secondary database
    //    /// </summary>
    //    Secondary= 2,
    //    /// <summary>
    //    /// Use Offline database
    //    /// </summary>
    //    Offline = 4
    //}
}
