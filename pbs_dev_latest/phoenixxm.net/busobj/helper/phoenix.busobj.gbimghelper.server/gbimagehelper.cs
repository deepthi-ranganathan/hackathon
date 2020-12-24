#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GbHelper.cs
// NameSpace: Phoenix.BusObj.Global.Helper
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//10/13/2006	1		vreddy		Created.
//02/27/2007	2		bhughes		70970 - ECM interface.  Found bug in decrypt function. 
//03/28/2014    3       mselvaga    #140895 - CR - Account History Image Center retrival changes added.
//-------------------------------------------------------------------------------


#endregion

using System;
using System.Data;
using System.Text;
using System.Collections;


//
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Global.Server;

namespace Phoenix.BusObj.Global.Server //Phoenix.BusObj.ImgHelper.Server
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class GbImageHelper : Phoenix.BusObj.Global.GbImageHelper
	{
		#region private vars
		Phoenix.BusObj.Global.Server.GbHelper _gbHelperServerBusObj = new Phoenix.BusObj.Global.Server.GbHelper();
		#endregion

		#region public constructors
		public GbImageHelper():base()
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

		protected override bool OnActionCustom(IDbHelper dbHelper, bool isPrimaryDb)
		{
			string ascii167 = Convert.ToChar(167).ToString();
			string sqlCommand = string.Empty;

			#region GetNameDetails
			if (this.CustomActionName == "GetAcctHist")
			{
				if (_paramNodes.Count > 0)
				{
					PBaseType result =  _paramNodes[0] as PBaseType;
					PBaseType acctType = _paramNodes[1] as PBaseType;
					PBaseType acctNo =  _paramNodes[2] as PBaseType;
					PBaseType depLoan =  _paramNodes[3] as PBaseType;
					PBaseType startDt =  _paramNodes[4] as PBaseType;
					PBaseType endDt =  _paramNodes[5] as PBaseType;
					
					//
					PDecimal Ptid = new PDecimal("Ptid");
					PDecimal Amount = new PDecimal("Amount");
					PDecimal CheckNo = new PDecimal("CheckNo"); //We will move 
					PString OriginTracerNo = new PString("OriginTracerNo");
					PDateTime EffectiveDt = new PDateTime("EffectiveDt");
                    PString TlCaptureISN = new PString("TlCaptureISN");
                    PString TlCaptureParentISN = new PString("TlCaptureParentISN");

					string tableName = string.Empty;
					string resultconcat = string.Empty;
                    string tellerCaptureISN = string.Empty;

					if (depLoan.StringValue.Trim() == "DP")
						tableName = "{0}dp_history";
					else if (depLoan.StringValue.Trim() == "LN")
						tableName = "{0}ln_history";
					else if (depLoan.StringValue.Trim() == "SD")
						tableName = "{0}sd_history"; //Crazy

                    if (depLoan.StringValue.Trim() == "DP" || depLoan.StringValue.Trim() == "LN")   //#140895
                    {
                        tellerCaptureISN = ", tl_capture_isn, tl_capture_parent_isn";
                    }
					
					sqlCommand = @"
								select effective_dt, origin_tracer_no, amt, Convert(decimal(12, 0),check_no), convert(decimal(12, 0),ptid) " +
                                tellerCaptureISN +
								@" from " + tableName + " " +					
								@" where acct_no = '" + acctNo.StringValue + "' " + 
								@" and acct_type = '" + acctType.StringValue + "' " + 
								@" and effective_dt between '" + startDt.StringValue + "' and '" + endDt.StringValue + "' " + 
								@" and origin_id in ( 3, 9, 12, 13) 
								   Order by 1 desc, 5 desc "; //History has order so here also
                    _gbHelperServerBusObj.ExecSqlImmediateInto(dbHelper, sqlCommand, true, true, EffectiveDt, OriginTracerNo, Amount, CheckNo, Ptid, TlCaptureISN, TlCaptureParentISN);
					do
					{
						if (!EffectiveDt.IsNull)
						{
							if (resultconcat == string.Empty)
								resultconcat = EffectiveDt.Value.ToString("yyyyMMdd");
							else 
								resultconcat = resultconcat + ascii167 + EffectiveDt.Value.ToString("yyyyMMdd");
							if (!OriginTracerNo.IsNull)
								resultconcat = resultconcat + "^" + OriginTracerNo.StringValue;
							else
								resultconcat = resultconcat + "^" + "";
							if (!Amount.IsNull)
								resultconcat = resultconcat + "^" + Amount.StringValue;
							else
								resultconcat = resultconcat + "^" + "";

							if (!CheckNo.IsNull)
								resultconcat = resultconcat + "^" + CheckNo.StringValue;
							else
								resultconcat = resultconcat + "^" + "-1";
                            if (!TlCaptureISN.IsNull)
                                resultconcat = resultconcat + "^" + TlCaptureISN.StringValue;
                            else
                                resultconcat = resultconcat + "^" + "";
                            if (!TlCaptureParentISN.IsNull)
                                resultconcat = resultconcat + "^" + TlCaptureParentISN.StringValue;
                            else
                                resultconcat = resultconcat + "^" + "";
						}
					}while (_gbHelperServerBusObj.ExecSqlGetNextRow( ));

					result.ValueObject = resultconcat;
				}
			}
			#endregion

			#region DecryptPassword
			if (this.CustomActionName == "DecryptPassword")
			{
				if (_paramNodes.Count > 0)
				{
					PBaseType result =  _paramNodes[0] as PBaseType;
					PBaseType encryptedPassword = _paramNodes[1] as PBaseType;
					//
					PString EncrPIN = new PString("EncrPIN");
					PString NullInd = new PString("NullInd");
					PString MasterFileKey = new PString("MasterFileKey");
					
					sqlCommand = @"SELECT  master_file_key  FROM {0}pc_appl_config";
					_gbHelperServerBusObj.ExecSqlImmediateInto(dbHelper, sqlCommand, true, false, MasterFileKey);
					if (!MasterFileKey.IsNull)
					{
						sqlCommand = 
						@"declare @sEncrPIN char(250), @sNullInd char(250)
						EXEC {0}psp_text_trans " +  "'" + MasterFileKey.StringValue + @"' ,'" + encryptedPassword.StringValue + "' , " + @" @sEncrPIN OUTPUT, @sNullInd OUTPUT 
						 Select @sEncrPIN, @sNullInd ";
						_gbHelperServerBusObj.ExecSqlImmediateInto(dbHelper, sqlCommand, true, false, EncrPIN, NullInd);
						if (!EncrPIN.IsNull)
							result.ValueObject = EncrPIN.StringValue;
						else
							result.ValueObject = encryptedPassword.StringValue.Trim();
					}
					else
						result.ValueObject = encryptedPassword.StringValue.Trim();
				}
			}
			#endregion DecryptPassword

			return true;
			//return base.OnActionCustom (dbHelper, isPrimaryDb);
		}


	} //End of Class
}
