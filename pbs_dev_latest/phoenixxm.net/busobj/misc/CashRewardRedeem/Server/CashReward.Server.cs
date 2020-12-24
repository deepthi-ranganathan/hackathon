#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: CashReward.Server.cs
// NameSpace: Phoenix.BusObj.Misc.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//11/20/2020	1		kiran.mani		Created
//11/26/2020	2		RDeepthi	 Task#133881. If call coes from TPI then selec totoals and add in Fetchrowdone.
//12/08/2020    3       RDeepthi        Task#133887.Include Validations for Rest Call.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Globalization;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Misc;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Global.Server;
using Phoenix.BusObj.Admin.Global;
using Phoenix.Shared;
using System.Xml;
using System.Text;

namespace Phoenix.BusObj.Misc.Server
{
	/// <summary>
	/// Summary description for CashReward.
	/// </summary>
	public class CashReward: Phoenix.BusObj.Misc.CashReward
	{
		#region private variables
		private PString sSQL = new PString("sSQL");
		private GbHelper _gbHelper = new GbHelper();
        #endregion

        public enum ResponseTypes
        {
			CashRwdInquiry = 11,
            CashRwdRedeem = 12,
            CashRwdAcctsInquiry = 13,
			TotalCashRwdInquiry = 14,
            HeckathonListView = 15,
            NonRewardAcctList = 16,
            ODGraphSelect = 20,
            LoanGraphSelect = 21
        }
	
		#region public constructors
		public CashReward():base()
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
            this.EventMakeListViewSql += new ListViewEventHandler(CashReward_EventMakeListViewSql);
            this.EventBeforeAction += CashReward_EventBeforeAction;//133881
			// Define Event Handlers
			
			
		
		}
        //Begin 133881
        private void CashReward_EventBeforeAction(object sender, BusActionEventArgs e)
        {
            if (IsRestAPI || (IsThirdParty && e.ActionType == XmActionType.ListView))//133887
            {
                PString rimSatus = new PString();
                sSQL.Value = string.Format(@"Select ltrim(rtrim(status))
											 From   {0}rm_acct r
											 where  r.rim_no = {1}", DbPrefix, RimNo.SqlString);

                _gbHelper.ExecSqlImmediateInto(e.DbHelper, sSQL.Value, rimSatus);
                if (rimSatus.IsNull || rimSatus.Value == string.Empty)
                {
                    // Invalid Rim Number, Enter a Valid Rim Number.
                    Messages.AddError(16891, null, string.Empty);
                }
                else if (rimSatus.Value == "Closed")
                {
                    //Rim Number Entered is in Closed Status.
                    Messages.AddError(16892, null, string.Empty); 
                }
            }
        }
        //End 133881

        #endregion

        #region Overrides
        protected override bool OnActionSelect(FrameWork.Core.IDbHelper dbHelper, bool isPrimaryDb)
        {
			 
			if(this.ResponseTypeId == (int)ResponseTypes.CashRwdInquiry)
            {
				sSQL.Value = string.Format(@"select	sum(isnull(b.cash_rwd_earn_ltd,0)),
													sum(isnull(b.cash_rwd_redeem_ltd,0)),
													sum(isnull(b.cash_rwd_bal,0))
											from {0}gb_map_acct_rel r
											inner join {0}dp_display a on r.acct_no = a.acct_no and r.acct_type = a.acct_type
											inner join {0}dp_display2 b on a.acct_no = b.acct_no and a.acct_type = b.acct_type
											where   r.rim_no = {1}
											/*and     a.status in  ('Active', 'Dormant', 'Escheated', 'Locked', 'Restricted') */
											and     r.signor = 'Y'", DbPrefix, RimNo.SqlString);

				_gbHelper.ExecSqlImmediateInto(dbHelper, sSQL.Value, TotCashRwdEarnLtd,
																	 TotCashRwddRedeemLtd,
																	 TotCashRwdBal);
                //Begin 133887. Assign to NoDbFields to get it in Rest API
                TotalCashRwdEarnLtd.Value = TotCashRwdEarnLtd.Value;
                TotalCashRwddRedeemLtd.Value = Convert.ToDecimal(TotCashRwddRedeemLtd.Value);
                TotalCashRwdBal.Value = Convert.ToDecimal(TotCashRwdBal.Value);
                //End 133887
            }

            if(this.ResponseTypeId == (int)ResponseTypes.CashRwdRedeem)
            {
                sSQL.Value = string.Format(@"select	
	                                        a.status,	 
	                                        isnull(cash_rwd.min_reward_amt,0) as min_reward_amt,
	                                        b.cash_rwd_bal,
	                                        a.branch_no
                                        from {0}gb_map_acct_rel r
                                        inner join {0}dp_display a on r.acct_no = a.acct_no and r.acct_type = a.acct_type
                                        inner join {0}dp_display2 b on b.acct_no = a.acct_no and b.acct_type = a.acct_type
                                        outer apply  (select top 1 cr.min_reward_amt 
				                                        from {0}rm_rp_pkg rp 
				                                        inner join {0}ad_rp_pkg_cash_rwd cr on cr.pkg_ptid = rp.pkg_ptid
				                                        where rp.rim_no = r.rim_no
				                                        and rp.status = 'Active'
				                                        order by cr.min_reward_amt asc ) as cash_rwd

                                        where   r.acct_no = '{1}'
		                                        and r.acct_type = '{2}'", DbPrefix, AcctNo.Value,AcctType.Value);

                _gbHelper.ExecSqlImmediateInto(dbHelper, sSQL.Value, Status,
                                                                     MinRewardAmt,
                                                                     CashRwdBal,
                                                                     BranchNo);
            }

            if (this.ResponseTypeId == (int)ResponseTypes.ODGraphSelect)
            {
                sSQL.Value = string.Format(@"
						Declare @nRC 	int
						Exec @nRC = {0}psp_get_cust_analysis  {1},2
					", DbPrefix, RimNo.SqlString);

                Phoenix.Shared.BusFrame.SqlHelper sqlHelper = new Phoenix.Shared.BusFrame.SqlHelper();

                DataSet OdDs = sqlHelper.GetDataSet(CoreService.DbHelper, sSQL.Value.ToString(), true);

                GetGrapgString(OdDs);
            }

            if (this.ResponseTypeId == (int)ResponseTypes.LoanGraphSelect)
            {
                sSQL.Value = string.Format(@"
						Declare @nRC 	int
						Exec @nRC = {0}psp_get_cust_analysis  {1},3
					", DbPrefix, RimNo.SqlString);

                Phoenix.Shared.BusFrame.SqlHelper sqlHelper = new Phoenix.Shared.BusFrame.SqlHelper();

                DataSet OdDs = sqlHelper.GetDataSet(CoreService.DbHelper, sSQL.Value.ToString(), true);

                GetGrapgString(OdDs);
            }
            return true;
        }      

        protected override bool OnActionInsert(IDbHelper dbHelper, bool isPrimaryDb)
        {
            if (this.ResponseTypeId == (int)ResponseTypes.CashRwdRedeem)
            {
                this.OnActionSelect(dbHelper, isPrimaryDb);
                this.OnValidateFields(dbHelper, isPrimaryDb);
                PostTc117(dbHelper);
            }
            return true;
        }

        protected override bool OnValidateFields(IDbHelper dbHelper, bool isPrimaryDb)
        {
            if(RedeemAmt.IsNull || RedeemAmt.Value <= 0)
            {
                //16874 - Amount to be Redeemed must be greater than zero
                Messages.AddError(16874, null, String.Empty);
            }
            //if (RedeemAmt.Value < MinRewardAmt.Value)
            //{
            //    //16875 - Amount to be Redeemed must be greater than or equal to Minimum Required Balance to Redeem.
            //    Messages.AddError(16875, null, String.Empty);
            //}
            if (RedeemAmt.Value > CashRwdBal.Value)
            {
                //16876 -Amount to be redeemed cannot be greater than available cash reward.
                Messages.AddError(16876, null, String.Empty);
            }
            if (CashRwdBal.Value < MinRewardAmt.Value)
            {
                //16875 - Amount to be Redeemed must be greater than or equal to Minimum Required Balance to Redeem.
                Messages.AddError(16875, null, String.Empty);
            }

            return base.OnValidateFields(dbHelper, isPrimaryDb);
        }
		#endregion

		private void CashReward_EventMakeListViewSql(object sender, ListViewEventArgs e)
        {
			string sSql = string.Empty;
            //Begin 133881
            if (IsThirdParty == true && this.ResponseTypeId == (int)ResponseTypes.TotalCashRwdInquiry)
            {
                //select Totals to show in response
                sSql = string.Format(@"select	sum(isnull(b.cash_rwd_earn_ltd,0)) as tot_cash_rwd_earn_ltd,
													sum(isnull(b.cash_rwd_redeem_ltd,0)) as tot_cash_rwd_redeem_ltd,
													sum(isnull(b.cash_rwd_bal,0)) as tot_cash_rwd_bal
											from {0}gb_map_acct_rel r
											inner join {0}dp_display a on r.acct_no = a.acct_no and r.acct_type = a.acct_type
											inner join {0}dp_display2 b on a.acct_no = b.acct_no and a.acct_type = b.acct_type
											where   r.rim_no = {1}
											/*and         a.status in  ('Active', 'Dormant', 'Escheated', 'Locked', 'Restricted')*/
											and     r.signor = 'Y'", DbPrefix, RimNo.SqlString);

            }
            else if (IsThirdParty == true && this.ResponseTypeId == (int)ResponseTypes.CashRwdAcctsInquiry)
            {
                sSql = string.Format(@"select	   
									r.acct_type + ' - ' + r.acct_no as account,
									a.status,
									ad.relationship,
									r.signor,
									isnull(cash_rwd.min_reward_amt,0)  as min_reward_amt,
									isnull(b.cash_rwd_bal,0) as cash_rwd_bal
								from {0}gb_map_acct_rel r
								inner join {0}dp_display a on r.acct_no = a.acct_no and r.acct_type = a.acct_type
								inner join {0}dp_display2 b on b.acct_no = a.acct_no and b.acct_type = a.acct_type
								inner join {0}ad_gb_acct_rel ad on r.rel_id = ad.rel_id
								outer apply  (select top 1 cr.min_reward_amt 
											 from {0}rm_rp_pkg rp 
											 inner join {0}ad_rp_pkg_cash_rwd cr on cr.pkg_ptid = rp.pkg_ptid
											 where rp.rim_no = {1}
											 and rp.status = 'Active'
											 order by cr.min_reward_amt asc ) as cash_rwd

						where       r.rim_no = {1}
						and         a.status in  ('Active', 'Dormant', 'Escheated', 'Locked', 'Restricted')
						and         r.signor = 'Y'
						and			isnull(b.cash_rwd_bal,0) > 0", DbPrefix, RimNo.SqlString);
            }
            //End 133881
            else if (ResponseTypeId == (int)ResponseTypes.CashRwdInquiry)
            {
                sSql = string.Format(@"select	   
									r.acct_type,
									r.acct_no,
									a.acct_desc,
									a.status,
									ad.relationship,
									r.signor,
									r.acct_type + ' - ' + r.acct_no as account,
									isnull(cash_rwd.min_reward_amt,0)  as min_reward_amt,
									isnull(b.cash_rwd_bal,0) as cash_rwd_bal,
									null as amt_redeemed
								from {0}gb_map_acct_rel r
								inner join {0}dp_display a on r.acct_no = a.acct_no and r.acct_type = a.acct_type
								inner join {0}dp_display2 b on b.acct_no = a.acct_no and b.acct_type = a.acct_type
								inner join {0}ad_gb_acct_rel ad on r.rel_id = ad.rel_id
								outer apply  (select top 1 cr.min_reward_amt 
											 from {0}rm_rp_pkg rp 
											 inner join {0}ad_rp_pkg_cash_rwd cr on cr.pkg_ptid = rp.pkg_ptid
											 where rp.rim_no = {1}
											 and rp.status = 'Active'
											 order by cr.min_reward_amt asc ) as cash_rwd

						where       r.rim_no = {1}
						and         a.status in  ('Active', 'Dormant', 'Escheated', 'Locked', 'Restricted')
						and         r.signor = 'Y'
						and			isnull(b.cash_rwd_bal,0) > 0", DbPrefix, RimNo.SqlString);
            }
            else if (this.ResponseTypeId == (int)ResponseTypes.NonRewardAcctList)
            {
                
                sSql = string.Format(@"
                                        Select rel.atm_acct_no,
	                                           rel.atm_acct_type,
                                               rel.atm_acct_type + ' - ' + rel.atm_acct_no as atm_account,
                                               rel.acct_type + ' - ' + rel.acct_no as account,
	                                           rel.acct_no,
	                                           rel.acct_type,
                                               rel.status
                                         From {0}atm_rel_acct rel
                                         inner join  {0}atm_acct atm on atm.atm_acct_no = rel.atm_acct_no and atm.atm_acct_type = rel.atm_acct_type
                                         Where rel.status = 'Active'
                                         and   atm.last_sys_maint_dt<=Dateadd(MONTH ,-6,'{2}')
                                          and  rel.rim_no = {1} and 
                                               rel.pan not in(Select pan from {0}dp_history where tran_code = 151)", DbPrefix, RimNo.SqlString,BusGlobalVars.SystemDate);
            }
            else if(ResponseTypeId == (int)ResponseTypes.HeckathonListView) {
                sSql = string.Format(@"
						Declare @nRC 	int
						Exec @nRC = {0}psp_get_cust_analysis  {1},1
					", DbPrefix, RimNo.SqlString);
            }
			e.SqlStmt = sSql;

		}

        private string GetChargeCode(int tranCode)
        {
            AdGbTc adGbTc = new AdGbTc();
            adGbTc.SelectAllFields = false;
            adGbTc.ChargeCode.Selected = true;
            adGbTc.TranCode.Value = Convert.ToInt16(tranCode);
            CoreService.DataService.ProcessRequest(XmActionType.Select, adGbTc);            

            if (adGbTc.ChargeCode.IsNull)
            {
                return "NULL";
            }

            else
            {
                return NumberHelper.NumberToStringX(adGbTc.ChargeCode.Value, 0);
            }
        }

        private bool PostTc117(IDbHelper dbHelper)
        {

            PString TranCode = new PString();
            PString sChargeCode = new PString();
            PDateTime sProcessDt = new PDateTime();
            PInt nPostingDef = new PInt();
            PString sTranDescription = new PString();
            PInt nTranSetId = new PInt("nTranSetId");
            PInt RC = new PInt();
            PInt nBranchNo = new PInt();
            PInt ClassCode = new PInt();
            PInt Identity = new PInt();
            PInt SQLError = new PInt();
            string sErrorText = string.Empty;
            string[] gsaTemp = new string[2];

            nTranSetId.Value = _gbHelper.GetTranSetId(dbHelper);
            sChargeCode.Value = GetChargeCode(117);

            sProcessDt.Value = Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate;
            nPostingDef.Value = (Phoenix.Shared.Variables.ApplicationVars.Instance.DpAvailBalDef == Decimal.MinValue ? 0 : Phoenix.Shared.Variables.ApplicationVars.Instance.DpAvailBalDef);
            sTranDescription.Value = "Cash Reward Redemption";
            


            sSQL.Value = string.Format(@"Declare @nRC integer, @rnIdentity numeric(12,0), @rnBranchNo smallint, @rnClassCode smallint, @rnSQLError int 
			exec @nRC = {0}psp_tc_dp_cr  117," +
                                        sProcessDt.SqlDate + "," +
                                        sProcessDt.SqlDate + "," +
                                        "NULL," +
                                        this.RedeemAmt.SqlString + "," +
                                        sChargeCode.Value + "," +
                                        "NULL," +
                                        "NULL," +
                                        Phoenix.Shared.Variables.GlobalVars.EmployeeId.ToString() + "," +
                                        "0," +
                                        "0," +
                                        "NULL," +
                                        "0," +
                                        "0," +
                                        nPostingDef.SqlString + "," +
                                        "NULL," +
                                        AcctNo.SqlString + "," +
                                        AcctType.SqlString + "," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        sTranDescription.SqlString + "," +
                                        "NULL," +
                                        "@rnBranchNo OUTPUT," +
                                        "@rnClassCode OUTPUT," +
                                        "@rnIdentity OUTPUT, " +
                                        "@rnSQLError OUTPUT, " +
                                        nTranSetId.SqlString + "," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "N," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "NULL," +
                                        "null," +
                                        "null," +
                                        "N," +
                                        "N," +
                                        "NULL"+ "  " +
                                       "Select @nRC, @rnBranchNo, @rnClassCode, @rnIdentity, @rnSQLError"
                                        , dbHelper.DbPrefix); 


            if (!_gbHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, RC, nBranchNo, ClassCode, Identity, SQLError))
            {
                // #16877 - Failed to post Tran Code.
                Messages.AddError(16877, null, String.Empty);
                return false;
            }

            if (RC.Value != 0)
            {
                sErrorText = _gbHelper.GetSPMessageText(Convert.ToInt32(RC.Value), true);
               
                gsaTemp = new string[2];
                gsaTemp[0] = sErrorText;
                Messages.AddWarning(16878, null, gsaTemp);
                return false;
            }


            sSQL.Value = string.Format(@"Declare @nRC int
					Exec @nRC =	{0}PSP_UPD_GL " +
                                RedeemAmt.SqlString + ", " +
                                BranchNo.Value + ", " +
                                ClassCode.Value + ", " +
                                @"1,
                                0,
								6153," +
                                AcctType.SqlString + ","+ 
								"NULL," +
							    AcctNo.SqlString +","+ 
								nTranSetId.SqlString+ @",
							    0,
							    1,
							    null,
							    null,
							    null Select @nRC", DbPrefix);

            if (!_gbHelper.ExecSqlImmediateInto(CoreService.DbHelper, sSQL.Value, false, false, RC))
            {
                // #16879 -Failed to post GL.
                Messages.AddError(16879, null, String.Empty);
                return false;
            }

            if (RC.Value != 0)
            {
                sErrorText = _gbHelper.GetSPMessageText(Convert.ToInt32(RC.Value), true);

                gsaTemp = new string[2];
                gsaTemp[0] = sErrorText;
                Messages.AddWarning(16880, null, gsaTemp);
                return false;
            }

            sSQL.Value = string.Format(@"   Update {0}dp_display2
								     set    cash_rwd_redeem_ltd = isnull(cash_rwd_redeem_ltd,0) + {1},
	                                        cash_rwd_bal  = isnull(cash_rwd_bal,0) - {1}
								      where acct_no = {2}
								      and   acct_type = {3}", DbPrefix, RedeemAmt.SqlString, AcctNo.SqlString, AcctType.SqlString);

            if (CoreService.DbHelper.ExecuteNonQuery(sSQL.Value) == 0)
            {
                //#16881 - Unable to update Deposit Display Record.
                Messages.AddError(16881, null, String.Empty);
                return false;
            }

            return true;
        }

        private void GetGrapgString(DataSet ds)
        {
            if (this.ResponseTypeId == (int)ResponseTypes.ODGraphSelect)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    StringBuilder sXml = new StringBuilder();
                    sXml.Append("<?xml version=\"1.0\"?>");
                    sXml.Append("<graph caption=\"Over Drafts\" subcaption=\"\" xAxisName=\"Period\" yAxisName=\"Number of OD\" numberPrefix=\"\" showNames=\"1\" showValues=\"0\" yaxismaxvalue=\"0.1\" canvasBorderThickness=\"1\" divLineDecimalPrecision=\"0\" limitsDecimalPrecision=\"0\">");
                    if (this.ResponseTypeId == (int)ResponseTypes.ODGraphSelect)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {

                            this.AcctNo.Value = Convert.ToString(dr["acct_no"]);
                            this.AcctType.Value = Convert.ToString(dr["acct_type"]);

                            string period = "\"" + Convert.ToString(dr["EFFECTIVE_DT"]) + "\"";
                            string cnt = "\"" + Convert.ToString(dr["NO_OD_FEES_MTD"]) + "\"";
                            sXml.Append("<set color = \"AABBCC\" name = " + period + " hoverText = " + period + " value = " + cnt + "/>");
                        }
                    }
                    sXml.Append("</graph>");
                    this.GraphXml.Value = sXml.ToString();
                }
            }

            if (this.ResponseTypeId == (int)ResponseTypes.LoanGraphSelect)
            {
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    StringBuilder sXml = new StringBuilder();
                    StringBuilder category = new StringBuilder();
                    StringBuilder Dataset1 = new StringBuilder();
                    StringBuilder Dataset2 = new StringBuilder();

                    category.Append("<categories>");
                    Dataset1.Append("<dataset seriesName=\"Paid\" color=\"AABBCC\">");
                    Dataset2.Append("<dataset seriesName=\"Unpaid\" color=\"7C7CB4\">");

                    sXml.Append("<?xml version=\"1.0\"?>");
                    sXml.Append("<graph caption=\"Loan Repayment\" subcaption=\"\" xAxisName=\"Period\" yAxisName=\"Amount Paid/Unpaid\" numberPrefix=\"\" showNames=\"1\" showValues=\"0\" yaxismaxvalue=\"0.1\" canvasBorderThickness=\"1\" divLineDecimalPrecision=\"0\" limitsDecimalPrecision=\"0\">");
                    if (this.ResponseTypeId == (int)ResponseTypes.LoanGraphSelect)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string period = "\"" + Convert.ToString(dr["month"]) +"/"+ Convert.ToString(dr["year"]) + "\"";
                            string ds1Rowval = "\"" + Convert.ToString(dr["paid"]) + "\"";
                            string ds2Rowval = "\"" + Convert.ToString(dr["unPaid"]) + "\"";

                            category.Append("<category name="+ period + " hoverText=" + period + "/>");
                            Dataset1.Append("<set value="+ ds1Rowval + " />");
                            Dataset2.Append("<set value=" + ds2Rowval + " />");

                        }
                    }
                    category.Append("</categories>");
                    Dataset1.Append("</dataset>");
                    Dataset2.Append("</dataset>");

                    sXml.Append(category.ToString());
                    sXml.Append(Dataset1.ToString());
                    sXml.Append(Dataset2.ToString());

                    sXml.Append("</graph>");
                    this.GraphXml.Value = sXml.ToString();
                }
            }
        }

    }



}
