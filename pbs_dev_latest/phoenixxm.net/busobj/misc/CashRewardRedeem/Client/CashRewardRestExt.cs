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
// File Name: CashRewardRestExt.cs
// NameSpace: Phoenix.BusObj.Misc
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//11/20/2020		1		kiran.mani		Created
//12/08/2020	2		RDeepthi		Task#133887. Rest changes for Cash Reward Inquiry.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Data;
using System.Globalization;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Misc;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json;

namespace Phoenix.BusObj.Misc
{
	/// <summary>
	/// Summary description for CashRewardRestExt.
	/// </summary>
	public class CashRewardRestExt: BusObjectRestExtBase
	{
		
		private CashReward _busObj
        {
            get
            {
                return BusObj as CashReward;
            }
        }

        public override void Initialize()
        {
            this.Title = "Cash Rewards";
            this.Description = "Cash Rewards";
            this.RootPath = "/Customers/CashRewards";

            #region Add REST supported generic actions        
            GetRimCashRwdDetails();
            #endregion

            #region Add REST supported non-generic actions
            #endregion

        }
        //Begin 133887
        private IOpenApiBOAction GetRimCashRwdDetails()
        {
            var action = AddAction(responseTypeId: 11, actionType: XmActionType.Select
                , description: "This call returns cash reward details for Customer/Member."
                    , summary: "Retrieve Cash Reward Details", path: "/Customers/{encId}/CashRewards", httpVerb: HttpVerb.GET)
                        .AddTags(RestDocTags.BO, RestDocTags.UI, RestDocTags.Employee, RestDocTags.Consumer);

            action.PathHasModuleEncId = true;
            this.CreateJsonResponse = true;
            this.AddEncIdToResponse = false;

            action.Response.AddBodyBusProperties(
                                                 _busObj.TotalCashRwdBal,
                                                 _busObj.TotalCashRwddRedeemLtd,
                                                 _busObj.TotalCashRwdEarnLtd
                                                 );



            var accountDet = action.Response.AddBodyChildModelArray("accounts", "List of Accounts");

            action.Response.AddChildModelField(accountDet, "acctType", FieldType.Char);
            action.Response.AddChildModelField(accountDet, "acctNo", FieldType.VarChar);
            action.Response.AddChildModelField(accountDet, "status", FieldType.VarChar);
            action.Response.AddChildModelField(accountDet, "minRewardAmt", FieldType.Decimal);
            action.Response.AddChildModelField(accountDet, "cashRwdBal", FieldType.Decimal);

            return action;
        }

        public override void ModifySingleRecordResponse(IDictionary<string, object> resultDict)
        {
            if (_busObj.ActionType == XmActionType.Select)
            {
                var cashRewardRestExt = CurrentAction.BusObj.RestExtension as CashRewardRestExt;

                var acctDetBo = BusObjHelper.MakeServerObject("CASH_REWARD");
                SetBusFieldValue(acctDetBo, "RimNo", _busObj.RimNo.Value);
                acctDetBo.ActionType = XmActionType.ListView;
                acctDetBo.ResponseTypeId = 11;
                acctDetBo.RestExtension.CreateJsonResponse = false;
                acctDetBo.RestExtension.AddEncIdToResponse = true;

                ExecChildBusObjDoAction(acctDetBo, CoreService.DbHelper);

                string xmlBusObjResponse = BusObjSerializer.SerializeToXml(acctDetBo, true, false);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlBusObjResponse);

                List<JRaw> _result = new List<JRaw>();

                if (doc.DocumentElement != null)
                {
                    foreach (XmlNode node in doc.DocumentElement.SelectNodes("RECORD"))
                    {
                        string tabGroup = string.Empty;
                        string acctType = string.Empty;
                        string acctNo = string.Empty;
                        Dictionary<string, Object> encIdKeyValues = new Dictionary<string, object>();
                        var xdoc = JsonConvert.SerializeXmlNode(node, Newtonsoft.Json.Formatting.None, true);


                        var jobj = JsonConvert.DeserializeObject<JObject>(xdoc);

                        if (jobj["AcctType"] != null)
                            acctType = Convert.ToString(jobj["AcctType"]);

                        if (jobj["AcctNo"] != null)
                            acctNo = Convert.ToString(jobj["AcctNo"]);

                        encIdKeyValues = new Dictionary<string, object>();

                        encIdKeyValues.Add("acctType", acctType);
                        encIdKeyValues.Add("acctNo", acctNo);

                        string AcctEncId = BusObj.RestExtension.GetEncId(encIdKeyValues);

                        if (jobj["AmtRedeemed"] != null)
                            jobj.Remove("AmtRedeemed");
                        if (jobj["AcctDesc"] != null)
                            jobj.Remove("AcctDesc");
                        if (jobj["Signor"] != null)
                            jobj.Remove("Signor");
                        if (jobj["Account"] != null)
                            jobj.Remove("Account");
                     
                        jobj.Add("encId", AcctEncId);
                        var detObj = JTokenExtensions.ToCamelCase(jobj);

                            var result = JsonConvert.DeserializeObject<JRaw>(detObj.ToString());

                            _result.Add(result);

                    }
                }
                acctDetBo.JsonResponse = _result;

                if (acctDetBo.JsonResponse == null)
                    acctDetBo.JsonResponse = new List<JRaw>();
                resultDict.Add("Accounts", acctDetBo.JsonResponse as List<JRaw>);
            }
        }

    }
    public static class JTokenExtensions
    {
        public static JObject ToCamelCase(JObject original)
        {
            var newObj = new JObject();
            foreach (var property in original.Properties())
            {
                var newPropertyName = property.Name.ToCamelCaseString();
                newObj[newPropertyName] = property.Value;
            }

            return newObj;
        }
        // Convert a string to camelCase        
        public static string ToCamelCaseString(this string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
    //End 133887

}
