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
//05/23/2007	2		Vreddy		ErasJV Tested at the Bank and Opening the interface
//06/04/2007    3       mselvaga    #73028 - Flag for offline support added.
//08/20/2007    4       FOyebola    #73556 - Fix Financialware html parameter.
//08/20/2007    5       FOyebola    #73478 - Fix Bankware html parameter parameters.
//10/23/2007    6       FOyebola    #74074: Fix problem with retrieving the wrong image from financialWare image capture.
//01/21/2008    7       vsharma    	#74271 - CQGetImageLinks -Added ECM changes for the new web service RetrieveItemImage.aspx
//05/08/2008    8       Deiland     #75465 - Added new PHXGetAcctImages() and PHXGetImageLinks (Copies of CQGetAcctImages and CQGetImageLinks)
//05/28/2009    9       Deiland     #77780 - Added new AVIPGetAcctImages() and AVIPGetImageLinks 
//08/16/2009    10      ewahib      #03333 - GetImageLinks fires expection if Tracer no length is smaller then 9
//01/16/2010    11      ewahib      #07226 - fix RenaiGetImageLinks , check if the value is null or not. 
//08/19/2010    12      bhughes     8736 - fraud images
//12/07/2010    13      sdhamija	#11630-fixed null errors on checkNo.
//12/31/2010    14      sdhamija	#2010 #11630; 2011 #11631
//01/05/2011    15      DEiland     WI#11458/#11459 - GetImageLinks is not gettig Sequence Number When image mask is null (PHXImageLinks is not used so we are not fixing it now)
//07/29/2011    16      SDighe      WI#14293/14294 - Item images printing out extremely large again after sp5 installed.
//05/06/2020    17      SChacko     US#124334/Task#125875 - New check image vendor Catch21 implementation.
//-------------------------------------------------------------------------------


#endregion

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;


//
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Admin.Global;
using Phoenix.Interop.Erasjv.Imageretrieval;
using Phoenix.Interop.FinancialWare;

namespace Phoenix.BusObj.Global
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class GbImageHelper : Phoenix.FrameWork.BusFrame.BusObjectBase
	{
		#region Private Fields
		Phoenix.BusObj.Admin.Global.AdGbImageControl _adGbImageControl = new AdGbImageControl();		
		Phoenix.Interop.Erasjv.Imageretrieval.ImageRetrieverClass _erasJvImgRetClsObj;
		//Phoenix.Interop.FinancialWare.FinIntegrateIDLClass _finIntegrateIDLClass;
		//Phoenix.Interop.FinancialWare.FinResearchIDLClass _finResearchIDLClass;
		#endregion Private fields

		#region private vars

		#endregion

		#region Constructor
		public GbImageHelper():
			base()
		{
			InitializeMap();
		}
		#endregion

		#region Public Methods

		#region GetImageLinks

		public Array GetImageLinks(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
		{
			Array htmlLinks = Array.CreateInstance( typeof(String), (checkInformation.Count * 2) );
			if (checkInformation.Count > 0)
			{
				string originTracerMask = string.Empty;
				string htmlLinkFront = string.Empty;
				string htmlLinkBack = string.Empty;
				
				if (_adGbImageControl.OriginTracerMask.IsNull)
					originTracerMask = _adGbImageControl.OriginTracerMask.Value;
				int counter = 0;
				foreach (CheckImageLinks checkInfoStruct in checkInformation)
                {
                    #region #03333
                    //string acctNo = Convert.ToInt64(checkInfoStruct.AcctNo.Replace("-", "")).ToString();// Old Code 
                    string acctNo = checkInfoStruct.AcctNo.Replace("-", "");
                    string date = checkInfoStruct.EffectiveDt.Trim();
                    string amount = checkInfoStruct.Amount.Trim().Replace(".", "");
                    long temp ;
                    if (Int64.TryParse(acctNo , out temp))
                    {
                        acctNo = Convert.ToInt64(acctNo).ToString();
                    }
                    #endregion
                    // *** Begin #73478 ***
                    // (a)Assigned the correct variable for the 'accct parameter   
                    // (b)Removed the decimal point from amount.
                    // (c)Reconfigured the tracer No
                    //Begin #140895
                    string tracerNo = string.Empty;
                    if (string.IsNullOrEmpty(checkInfoStruct.TlCaptureISN))
                    {
                        tracerNo = checkInfoStruct.TracerNo.Trim();
                        tracerNo = tracerNo.PadLeft(10, '0'); //'0000000000' || psaImgSrcTracerNo[nCounter], SalStrRightX( psaImgSrcTracerNo[nCounter], 10 )
                    }
                    else
                    {
                        tracerNo = checkInfoStruct.TlCaptureISN.Trim();
                        tracerNo = tracerNo.PadLeft(15, '0'); //'000000000000000'
                    }
                    //End #140895
					string seqNoTemp = string.Empty;

                    // Begin WI#11458 - Handle When The Origin Tracer Mask is Null
                    //#140895
                    if (string.IsNullOrEmpty(checkInfoStruct.TlCaptureISN))
                    {
                        if (_adGbImageControl.OriginTracerMask.StringValue.Length > 0)
                        {
                            for (int i = 0; i <= 9; i++)
                            {
                                #region #03333
                                if (_adGbImageControl.OriginTracerMask.StringValue.Length <= i)
                                    break;

                                #endregion

                                if (_adGbImageControl.OriginTracerMask.StringValue.Substring(i, 1) != "*")
                                    seqNoTemp = seqNoTemp + tracerNo.Substring(i, 1);
                            }
                        }
                        else    // Origin Tracer Number Is Null
                        {
                            seqNoTemp = tracerNo;
                        }
                    }
                    else
                    {
                        //Commented out origintracermask for image retrieval processing based on AVTC ISN#
                        //if (_adGbImageControl.OriginTracerMask.StringValue.Length > 0)
                        //{
                        //    for (int i = 0; i <= 14; i++)
                        //    {
                        //        #region #03333
                        //        if (_adGbImageControl.OriginTracerMask.StringValue.Length <= i)
                        //            break;

                        //        #endregion

                        //        if (_adGbImageControl.OriginTracerMask.StringValue.Substring(i, 1) != "*")
                        //            seqNoTemp = seqNoTemp + tracerNo.Substring(i, 1);
                        //    }
                        //}
                        //else    // Origin Tracer Number Is Null
                        //{
                            seqNoTemp = tracerNo;
                        //}
                    }
                    // End WI#11458 

					if (seqNoTemp.Length > 0)
						seqNoTemp = Convert.ToInt64(seqNoTemp).ToString();
					else
						seqNoTemp = "0";//Not possible?
					long checkNo = -1;
					//Begin #11630
					//if (checkInfoStruct.CheckNo.Length > 0)
					//	checkNo = Convert.ToInt64(checkInfoStruct.CheckNo); //We do not break in the future
					checkNo = GetCheckNo(checkInfoStruct.CheckNo);
					//End #11630
                    //#140895-TBD-Set default values for testing only
                    //date = "20140227";
                    //amount = "66500";
                    //acctNo = "17032224";
                    //checkNo = 0;
                    //seqNoTemp = "000000001002054";
                    //
                    //date = "20140505";
                    //amount = "5950";
                    //acctNo = "127878";
                    //checkNo = -1;
                    //seqNoTemp = "000000100102652";
                    //#END #140895-TBD-default
                    htmlLinkFront = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue + "ImgReqIS.dll?PHOENIX&id=" + _adGbImageControl.BankId.StringValue; //gsImgVendorUrl || gsVirtualImgDir || "ImgReqIS.dll?PHOENIX&id=" || gsImgBankId
                    htmlLinkFront = htmlLinkFront + @"&acct=" + acctNo.Trim();
                    htmlLinkFront = htmlLinkFront + @"&side=f" + @"&date=" + date + "&amt=" + amount;
                    // *** End #73478 ***
                    //htmlLinkBack = htmlLinkBack + @"&side=b" + @"&date=" +  checkInfoStruct.EffectiveDt  + "&amt=" + checkInfoStruct.Amount;
                    if (checkNo != -1)
                    {
                        htmlLinkFront = htmlLinkFront + "&check=" + checkNo.ToString();
                        //htmlLinkBack = htmlLinkBack + "&check=" + checkNo.ToString();
                    }
					htmlLinkFront = htmlLinkFront + "&ex_trace=" + seqNoTemp.Trim();
					htmlLinkBack = htmlLinkFront.Replace(@"&side=f", @"&side=b");
					//htmlLinks.SetValue(htmlLinkFront, 0, counter);
					//htmlLinks.SetValue(htmlLinkBack, 1, counter);
					htmlLinks.SetValue(htmlLinkFront, counter);
					counter++;
					htmlLinks.SetValue(htmlLinkBack, counter);
					counter++;
				}
			}
            //Begin #73556
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                StringBuilder str = new StringBuilder();
                if (htmlLinks != null)
                {
                    for (int i = 0; i < htmlLinks.Length; i++)
                    {
                        str.Append(htmlLinks.GetValue(i) + "\n");
                    }
                    CoreService.LogPublisher.LogDebug(str.ToString());
                }
            }
            //End #73556

			return htmlLinks;
		}
		#endregion GetImageLinks

		#region ExtractImageLinks
		public Array ExtractLinks(string url)
		{
			Array htmlLinks = Array.CreateInstance( typeof(String), 0 );
			string htmlPage = string.Empty;
			HttpWebRequest request = null; 
							
			HttpWebResponse response = null;						
			try
			{
				request = (HttpWebRequest)WebRequest.Create(url);
				response = (HttpWebResponse)request.GetResponse();	
				request.KeepAlive = false;
				using (StreamReader sr = new StreamReader( response.GetResponseStream() ))
				{
					htmlPage = sr.ReadToEnd();						
				}
				htmlPage = htmlPage.ToLower();
				if (CoreService.LogPublisher.IsLogEnabled)
					CoreService.LogPublisher.LogInfo( htmlPage );
				

				#region Explain Reg Expression
				/*
<a
Any whitespace character 
+ (one or more times)
Non-capturing Group
  Non-capturing Group
    Any word character 
    + (one or more times)
    Any whitespace character 
    * (zero or more times)
    =
    Any whitespace character 
    * (zero or more times)
  End Capture
  Non-capturing Group
    Any word character 
    + (one or more times)
        or
    "
    Any character not in """
    * (zero or more times)
    "
        or
    '
    Any character not in "'"
    * (zero or more times)
    '
  End Capture
End Capture
* (zero or more times) (non-greedy)
Any whitespace character 
* (zero or more times)
href
Any whitespace character 
* (zero or more times)
=
Any whitespace character 
* (zero or more times)
Capture to <url>
  Any word character 
  + (one or more times)
    or
  "
  Any character not in """
  * (zero or more times)
  "
    or
  '
  Any character not in "'"
  * (zero or more times)
  '
End Capture
Non-capturing Group
  Non-capturing Group
    Any whitespace character 
    + (one or more times)
    Any word character 
    + (one or more times)
    Any whitespace character 
    * (zero or more times)
    =
    Any whitespace character 
    * (zero or more times)
  End Capture
  Non-capturing Group
    Any word character 
    + (one or more times)
        or
    "
    Any character not in """
    * (zero or more times)
    "
        or
    '
    Any character not in "'"
    * (zero or more times)
    '
  End Capture
End Capture
* (zero or more times) (non-greedy)
>
Any character not in "<"
+ (one or more times)
</a>
				 * */
				#endregion 

				Regex regex = null;
				regex = new Regex(@"<a\s+(?:(?:\w+\s*=\s*)(?:\w+|""[^""]*""|'[^']*'))*?\s*href\s*=\s*(?<url>\w+|""[^""]*""|'[^']*')(?:(?:\s+\w+\s*=\s*)(?:\w+|""[^""]*""|'[^']*'))*?>[^<]+</a>", 
					RegexOptions.IgnoreCase | 
					RegexOptions.Singleline);	
				MatchCollection myCollection = regex.Matches(htmlPage);
				htmlLinks = Array.CreateInstance( typeof(String), myCollection.Count );
				int j = 0;
				string[] groupNames = regex.GetGroupNames();
				string[] strings;
				strings = new string[1];
				strings[0] = htmlPage;
				foreach (string s in strings)
				{
					Match m = null;
					m = regex.Match(s);
					bool noMatch = true;
					while (m.Success)
					{
						noMatch = false;
						GroupCollection groups = m.Groups;					
						if (groups[0].Value.ToLower().IndexOf("get image") > 0)
						{
							string stripDoubleQuote = groups[1].Value.ToString();
							stripDoubleQuote = stripDoubleQuote.Substring(1, stripDoubleQuote.Length - 2);
							htmlLinks.SetValue(stripDoubleQuote, j);
							j++;
						}
						m = m.NextMatch();
					}
					if (noMatch)					
						CoreService.LogPublisher.LogInfo("    No Match\r\n");
				}
			
			}
			catch(Exception ex)
			{
				CoreService.LogPublisher.LogDebug("Error Building Links:" + ex.Message + "\n" + ex.InnerException.Message);
			}
			finally
			{
				//Close the Reade and Responce
				if (response != null)
					response.Close();				
			}
			return htmlLinks;
		}
		#endregion ExtractImageLinks

		#region Check Quest And RenaiGetImageLinks

		#region CQGetAcctImages
		public Array CQGetAcctImages(string acctType, string acctNo, string depLoan, DateTime searchStartDt, DateTime searchEndDt, AdGbImageControl adGbImageControlObj)
		{
			//Array string[] htmlLinks;
			string [] historyConcat;
			string [] rowvalues;
			ArrayList hitoryRecords = new ArrayList() ;
			string ascii167 = Convert.ToChar(167).ToString();
			if( _paramNodes == null )
				_paramNodes = new ArrayList();
			else
				_paramNodes.Clear();
			//
			PBaseType result	= new PBaseType( "r1" );
			PBaseType pAcctType = new PBaseType( "p1");
			PBaseType pAcctNo	= new PBaseType( "p2");
			PBaseType pDepLoan	= new PBaseType( "p3" );
			PBaseType pStartDt	= new PBaseType( "p4" );
			PBaseType pEndDt	= new PBaseType( "p5" );
			//
			pAcctType.ValueObject	= acctType;
			pAcctNo.ValueObject		= acctNo;
			pDepLoan.ValueObject	= depLoan;
			pStartDt.ValueObject	= searchStartDt.ToString("MM/dd/yyyy");
			pEndDt.ValueObject		= searchEndDt.ToString("MM/dd/yyyy");
			//
			_paramNodes.Add( result );
			_paramNodes.Add( pAcctType );
			_paramNodes.Add( pAcctNo );
			_paramNodes.Add( pDepLoan );
			_paramNodes.Add( pStartDt );
			_paramNodes.Add( pEndDt );
			//
			if( CoreService.AppSetting.IsServer == false ) 
			{
				CoreService.DataService.ProcessCustomAction( this, "GetAcctHist", (IPhoenixSerializable[])_paramNodes.ToArray(typeof(IPhoenixSerializable)));
			}
			string resultStr = ( result.IsNull ?string.Empty:result.StringValue);
			if (resultStr != string.Empty)
			{
				//Build the ctructures
				historyConcat = resultStr.Split(Convert.ToChar(167));
				Array.CreateInstance( typeof(String), 2, historyConcat.Length );
				for (int i = 0; historyConcat.Length > i; i++)
				{
					//Tokenize
					rowvalues = historyConcat[i].Split('^');
					CheckImageLinks checkStru = new CheckImageLinks();
					checkStru.EffectiveDt = rowvalues[0];
					checkStru.AcctNo = acctNo;
					checkStru.AcctType = acctType;
					checkStru.TracerNo = rowvalues[1]; 
					checkStru.Amount = rowvalues[2]; 
					checkStru.CheckNo = rowvalues[3]; 
					hitoryRecords.Add(checkStru);//Add to the ArrayList
				}
				return CQGetImageLinks(hitoryRecords, adGbImageControlObj);
			}
			else
			{
				//So we do not break
				Array htmlLinks = Array.CreateInstance( typeof(String), 0 );
				return htmlLinks;
			}
		}
		#endregion CQGetAcctImages
		//
		#region CQGetImageLinks
		public Array CQGetImageLinks(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
		{

			Array htmlLinks = Array.CreateInstance( typeof(String), (checkInformation.Count * 2) );
			int noOfLinks = checkInformation.Count;
			string baseUrl = string.Empty;
			if (checkInformation.Count > 0)
			{
				int counter = 0;
                string originTracerMask = string.Empty;

				if (noOfLinks > 1)
					baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue + _adGbImageControl.ImgEngine.Value; // gsImgVendorUrl || gsVirtualImgDir || gsImgEngine
				else
				{
					baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue;
					string imgEngine = string.Empty;
					if (_adGbImageControl.ImgEngine.Value.IndexOf(@"?") < 0) //did not find any
						imgEngine = _adGbImageControl.ImgEngine.Value + @"?";
					else
					{
						imgEngine = _adGbImageControl.ImgEngine.Value;
						if (!imgEngine.EndsWith(@"&"))
							imgEngine = imgEngine + @"&";
					}
					baseUrl = baseUrl + imgEngine;

				}
				//#74271
				if (!_adGbImageControl.BankId.IsNull)
					baseUrl = baseUrl + "id=" + _adGbImageControl.BankId.StringValue + @"&";

                if (_adGbImageControl.OriginTracerMask.IsNull)
                    originTracerMask = _adGbImageControl.OriginTracerMask.Value;
		
				foreach (CheckImageLinks checkInfoStruct in checkInformation)
				{	
					string htmlLinkBack = string.Empty;
					string htmlLinkFront  = string.Empty;
					long checkNo = -1;
                    // Begin #74074 
                    //if (checkInfoStruct.CheckNo.Length > 0)
					//	checkNo = Convert.ToInt64(checkInfoStruct.CheckNo); //We do not break in the future
					//Begin #11630
                    //if (checkInfoStruct.CheckNo.Trim() != "" && Convert.ToInt64(checkInfoStruct.CheckNo) != 0)
                    //    checkNo = Convert.ToInt64(checkInfoStruct.CheckNo);
					checkNo = GetCheckNo(checkInfoStruct.CheckNo);
                    //End  #11630
                    // End #74074
                    
					htmlLinkFront = baseUrl;
					//I see this happening in centura
					//#74271
					if (!htmlLinkFront.EndsWith(@"&"))				
						htmlLinkFront = htmlLinkFront +  @"&Date=" +  checkInfoStruct.EffectiveDt;
					else
						htmlLinkFront = htmlLinkFront +  @"Date=" +  checkInfoStruct.EffectiveDt;

                    //Apply the masking to the tracer number
                    string tracerNo = checkInfoStruct.TracerNo.Trim();
                    tracerNo = tracerNo.PadLeft(10, '0'); //'0000000000' || psaImgSrcTracerNo[nCounter], SalStrRightX( psaImgSrcTracerNo[nCounter], 10 )
                    string seqNoTemp = string.Empty;
                    
                    //Begin 2010 #11630; 2011 #11631
                    if(!_adGbImageControl.OriginTracerMask.IsNull && _adGbImageControl.OriginTracerMask.Value.Length > 9)
                    {
						for (int i = 0; i <= 9; i++)
						{
							if (_adGbImageControl.OriginTracerMask.StringValue.Substring(i, 1) != "*")
								seqNoTemp = seqNoTemp + tracerNo.Substring(i, 1);
						}
                    }
                    else
                    {
						if(tracerNo != null && tracerNo.Length >9)
							seqNoTemp = tracerNo.Substring(0,10);
                    }
					//End 2010 #11630; 2011 #11631
					
                    if (seqNoTemp.Length > 0)
                        //WI#14293 - skipped the conversion
                        //seqNoTemp = Convert.ToInt64(seqNoTemp).ToString();
                        seqNoTemp = seqNoTemp.Trim();
                    else
                        seqNoTemp = "0";//Not possible?

					/*if (checkInfoStruct.TracerNo != string.Empty)
                        htmlLinkFront = htmlLinkFront + "&CoreTraceNum=" + checkInfoStruct.TracerNo;*/
	                htmlLinkFront = htmlLinkFront + "&CoreTraceNum=" + seqNoTemp.Trim();
                     
					htmlLinkFront = htmlLinkFront + "&Face=f" + "&Account=" + checkInfoStruct.AcctNo.Replace("-","").Trim(); // #73556: Removed dashes
					htmlLinkFront = htmlLinkFront + "&Amount=" + checkInfoStruct.Amount.Replace(".","");
					if (checkNo != -1)
					{
							htmlLinkFront = htmlLinkFront +  "&CheckNum=" + checkNo.ToString();					
					}
                    htmlLinkFront = htmlLinkFront + "&ResponseType=png" + "&Cents=y" + "&Index=1";
					htmlLinkBack = htmlLinkFront.Replace("&Face=f", "&Face=b");
					//htmlLinks.SetValue(htmlLinkFront, 0, counter);
					//htmlLinks.SetValue(htmlLinkBack, 1, counter);
					htmlLinks.SetValue(htmlLinkFront, counter);
					counter++;
					htmlLinks.SetValue(htmlLinkBack, counter);
					counter++;
				}
			}

            //Begin #73556
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                StringBuilder str = new StringBuilder();

                if (htmlLinks != null)
                {

                    for (int i = 0; i < htmlLinks.Length; i++)
                    {

                        str.Append(htmlLinks.GetValue(i) + "\n");
                    }

                    CoreService.LogPublisher.LogDebug(str.ToString());
                }
            }
            //End #73556

//#if DEBUG
//			string urlPart1 = @"http://localhost/images/nasa00";
//			string urlPart2 = ".jpg";
//			int maxArray = htmlLinks.Length;
//			if (htmlLinks.Length > 8)
//				maxArray = 8;
//			for (int i=0; i < maxArray; i++)
//			{
//				//Deal with Zero Index
//				htmlLinks.SetValue((urlPart1 + (i + 1).ToString() + urlPart2), i);
//			}
//#endif
			return htmlLinks;
		}
        #endregion CQGetImageLinks		

        /*Begin #125875*/
        #region Catch21ImageLink
        public Array Catch21ImageLink(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
        {
            Array htmlLink = Array.CreateInstance(typeof(String), checkInformation.Count);
            string baseUrl = string.Empty;
            if (checkInformation.Count > 0)
            {
                int counter = 0;
                string originTracerMask = string.Empty;

                baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue;

                if (!_adGbImageControl.ImgEngine.IsNull)
                {
                    string imgEngine = string.Empty;
                    if (_adGbImageControl.ImgEngine.Value.IndexOf(@"?") < 0) //did not find any
                        imgEngine = _adGbImageControl.ImgEngine.Value + @"?";
                    else
                    {
                        imgEngine = _adGbImageControl.ImgEngine.Value;
                        if (!imgEngine.EndsWith(@"&"))
                            imgEngine = imgEngine + @"&";
                    }
                    baseUrl = baseUrl + imgEngine;
                }


                if (!_adGbImageControl.BankId.IsNull)
                    baseUrl = baseUrl + "ArchiveConfig=" + _adGbImageControl.BankId.StringValue + @"&";

                if (_adGbImageControl.OriginTracerMask.IsNull)
                    originTracerMask = _adGbImageControl.OriginTracerMask.Value;

                foreach (CheckImageLinks checkInfoStruct in checkInformation)
                {
                    string htmlLinkTemp = string.Empty;

                    htmlLinkTemp = baseUrl;

                    if (!htmlLinkTemp.EndsWith(@"&"))
                        htmlLinkTemp = htmlLinkTemp + @"&Date=" + checkInfoStruct.EffectiveDt;
                    else
                        htmlLinkTemp = htmlLinkTemp + @"Date=" + checkInfoStruct.EffectiveDt;


                    string tracerNo = checkInfoStruct.TracerNo.Trim();

                    htmlLinkTemp = htmlLinkTemp + "&Sequence=" + checkInfoStruct.TracerNo.Trim().TrimStart('0');

                    htmlLinkTemp = htmlLinkTemp + "&ImageFB=f";
                    htmlLink.SetValue(htmlLinkTemp, counter);
                    counter++;

                }
                if (CoreService.LogPublisher.IsLogEnabled)
                {
                    StringBuilder link = new StringBuilder();
                    if (htmlLink != null)
                    {
                        for (int i = 0; i < htmlLink.Length; i++)
                        {
                            link.Append(htmlLink.GetValue(i) + "\n");
                        }
                        CoreService.LogPublisher.LogDebug("Image Link: " + link.ToString());
                    }
                }
            }
            return htmlLink;
        }
        #endregion
        /*End #125875*/

        // Begin #75465
        #region PHXGetAcctImages
        public Array PHXGetAcctImages(string acctType, string acctNo, string depLoan, DateTime searchStartDt, DateTime searchEndDt, AdGbImageControl adGbImageControlObj)
        {
            //Array string[] htmlLinks;
            string[] historyConcat;
            string[] rowvalues;
            ArrayList hitoryRecords = new ArrayList();
            string ascii167 = Convert.ToChar(167).ToString();
            if (_paramNodes == null)
                _paramNodes = new ArrayList();
            else
                _paramNodes.Clear();
            //
            PBaseType result = new PBaseType("r1");
            PBaseType pAcctType = new PBaseType("p1");
            PBaseType pAcctNo = new PBaseType("p2");
            PBaseType pDepLoan = new PBaseType("p3");
            PBaseType pStartDt = new PBaseType("p4");
            PBaseType pEndDt = new PBaseType("p5");
            //
            pAcctType.ValueObject = acctType;
            pAcctNo.ValueObject = acctNo;
            pDepLoan.ValueObject = depLoan;
            pStartDt.ValueObject = searchStartDt.ToString("MM/dd/yyyy");
            pEndDt.ValueObject = searchEndDt.ToString("MM/dd/yyyy");
            //
            _paramNodes.Add(result);
            _paramNodes.Add(pAcctType);
            _paramNodes.Add(pAcctNo);
            _paramNodes.Add(pDepLoan);
            _paramNodes.Add(pStartDt);
            _paramNodes.Add(pEndDt);
            //
            if (CoreService.AppSetting.IsServer == false)
            {
                CoreService.DataService.ProcessCustomAction(this, "GetAcctHist", (IPhoenixSerializable[])_paramNodes.ToArray(typeof(IPhoenixSerializable)));
            }
            string resultStr = (result.IsNull ? string.Empty : result.StringValue);
            if (resultStr != string.Empty)
            {
                //Build the ctructures
                historyConcat = resultStr.Split(Convert.ToChar(167));
                Array.CreateInstance(typeof(String), 2, historyConcat.Length);
                for (int i = 0; historyConcat.Length > i; i++)
                {
                    //Tokenize
                    rowvalues = historyConcat[i].Split('^');
                    CheckImageLinks checkStru = new CheckImageLinks();
                    checkStru.EffectiveDt = rowvalues[0];
                    checkStru.AcctNo = acctNo;
                    checkStru.AcctType = acctType;
                    checkStru.TracerNo = rowvalues[1];
                    checkStru.Amount = rowvalues[2];
                    checkStru.CheckNo = rowvalues[3];
                    if (rowvalues.Length > 4)   //#140895
                    {
                        checkStru.TlCaptureISN = rowvalues[4];
                        checkStru.TlCaptureParentISN = rowvalues[5];
                    }
                    hitoryRecords.Add(checkStru);//Add to the ArrayList
                }
                return PHXGetImageLinks(hitoryRecords, adGbImageControlObj);
            }
            else
            {
                //So we do not break
                Array htmlLinks = Array.CreateInstance(typeof(String), 0);
                return htmlLinks;
            }
        }
        #endregion PHXGetAcctImages
        //
        #region PHXGetImageLinks
        public Array PHXGetImageLinks(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
        {

            Array htmlLinks = Array.CreateInstance(typeof(String), (checkInformation.Count * 2));
            int noOfLinks = checkInformation.Count;
            string baseUrl = string.Empty;
            if (checkInformation.Count > 0)
            {
                int counter = 0;
                string originTracerMask = string.Empty;

                if (noOfLinks > 1)
                    baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue + _adGbImageControl.ImgEngine.Value; // gsImgVendorUrl || gsVirtualImgDir || gsImgEngine
                else
                {
                    baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue;
                    string imgEngine = string.Empty;
                    if (_adGbImageControl.ImgEngine.Value.IndexOf(@"?") < 0) //did not find any
                        imgEngine = _adGbImageControl.ImgEngine.Value + @"?";
                    else
                    {
                        imgEngine = _adGbImageControl.ImgEngine.Value;
                        if (!imgEngine.EndsWith(@"&"))
                            imgEngine = imgEngine + @"&";
                    }
                    baseUrl = baseUrl + imgEngine;

                }
                //#74271
                if (!_adGbImageControl.BankId.IsNull)
                    baseUrl = baseUrl + "id=" + _adGbImageControl.BankId.StringValue + @"&";

                if (_adGbImageControl.OriginTracerMask.IsNull)
                    originTracerMask = _adGbImageControl.OriginTracerMask.Value;

                foreach (CheckImageLinks checkInfoStruct in checkInformation)
                {
                    string htmlLinkBack = string.Empty;
                    string htmlLinkFront = string.Empty;
                    long checkNo = -1;
                    // Begin #74074 
                    //if (checkInfoStruct.CheckNo.Length > 0)
                    //	checkNo = Convert.ToInt64(checkInfoStruct.CheckNo); //We do not break in the future
					//Begin #11630
                    //if (checkInfoStruct.CheckNo.Trim() != "" && Convert.ToInt64(checkInfoStruct.CheckNo) != 0)
                    //    checkNo = Convert.ToInt64(checkInfoStruct.CheckNo);
					checkNo = GetCheckNo(checkInfoStruct.CheckNo);
					//End  #11630
                    // End #74074

                    htmlLinkFront = baseUrl;
                    //I see this happening in centura
                    //#74271
                    if (!htmlLinkFront.EndsWith(@"&"))
                        htmlLinkFront = htmlLinkFront + @"&Date=" + checkInfoStruct.EffectiveDt;
                    else
                        htmlLinkFront = htmlLinkFront + @"Date=" + checkInfoStruct.EffectiveDt;

                    //Apply the masking to the tracer number
                    string tracerNo = checkInfoStruct.TracerNo.Trim();
                    tracerNo = tracerNo.PadLeft(10, '0'); //'0000000000' || psaImgSrcTracerNo[nCounter], SalStrRightX( psaImgSrcTracerNo[nCounter], 10 )
                    string seqNoTemp = string.Empty;
                    for (int i = 0; i <= 9; i++)
                    {
                        if (_adGbImageControl.OriginTracerMask.StringValue.Substring(i, 1) != "*")
                            seqNoTemp = seqNoTemp + tracerNo.Substring(i, 1);
                    }
                    if (seqNoTemp.Length > 0)
                        seqNoTemp = Convert.ToInt64(seqNoTemp).ToString();
                    else
                        seqNoTemp = "0";//Not possible?

                    /*if (checkInfoStruct.TracerNo != string.Empty)
                        htmlLinkFront = htmlLinkFront + "&CoreTraceNum=" + checkInfoStruct.TracerNo;*/
                    htmlLinkFront = htmlLinkFront + "&CoreTraceNum=" + seqNoTemp.Trim();

                    htmlLinkFront = htmlLinkFront + "&Face=f" + "&Account=" + checkInfoStruct.AcctNo.Replace("-", "").Trim(); // #73556: Removed dashes
                    htmlLinkFront = htmlLinkFront + "&Amount=" + checkInfoStruct.Amount.Replace(".", "");
                    if (checkNo != -1)
                    {
                        htmlLinkFront = htmlLinkFront + "&CheckNum=" + checkNo.ToString();
                    }
                    htmlLinkFront = htmlLinkFront + "&ResponseType=png" + "&Cents=y" + "&Index=1";
                    htmlLinkBack = htmlLinkFront.Replace("&Face=f", "&Face=b");
                    //htmlLinks.SetValue(htmlLinkFront, 0, counter);
                    //htmlLinks.SetValue(htmlLinkBack, 1, counter);
                    htmlLinks.SetValue(htmlLinkFront, counter);
                    counter++;
                    htmlLinks.SetValue(htmlLinkBack, counter);
                    counter++;
                }
            }

            //Begin #73556
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                StringBuilder str = new StringBuilder();

                if (htmlLinks != null)
                {

                    for (int i = 0; i < htmlLinks.Length; i++)
                    {

                        str.Append(htmlLinks.GetValue(i) + "\n");
                    }

                    CoreService.LogPublisher.LogDebug(str.ToString());
                }
            }
            //End #73556

            //#if DEBUG
            //			string urlPart1 = @"http://localhost/images/nasa00";
            //			string urlPart2 = ".jpg";
            //			int maxArray = htmlLinks.Length;
            //			if (htmlLinks.Length > 8)
            //				maxArray = 8;
            //			for (int i=0; i < maxArray; i++)
            //			{
            //				//Deal with Zero Index
            //				htmlLinks.SetValue((urlPart1 + (i + 1).ToString() + urlPart2), i);
            //			}
            //#endif
            return htmlLinks;
        }
        #endregion PHXGetImageLinks		
        // End #75465

        // Begin #77780
        #region AVIPGetAcctImages
        public Array AVIPGetAcctImages(string acctType, string acctNo, string depLoan, DateTime searchStartDt, DateTime searchEndDt, AdGbImageControl adGbImageControlObj)
        {
            //Array string[] htmlLinks;
            string[] historyConcat;
            string[] rowvalues;
            ArrayList hitoryRecords = new ArrayList();
            string ascii167 = Convert.ToChar(167).ToString();
            if (_paramNodes == null)
                _paramNodes = new ArrayList();
            else
                _paramNodes.Clear();
            //
            PBaseType result = new PBaseType("r1");
            PBaseType pAcctType = new PBaseType("p1");
            PBaseType pAcctNo = new PBaseType("p2");
            PBaseType pDepLoan = new PBaseType("p3");
            PBaseType pStartDt = new PBaseType("p4");
            PBaseType pEndDt = new PBaseType("p5");
            //
            pAcctType.ValueObject = acctType;
            pAcctNo.ValueObject = acctNo;
            pDepLoan.ValueObject = depLoan;
            pStartDt.ValueObject = searchStartDt.ToString("MM/dd/yyyy");
            pEndDt.ValueObject = searchEndDt.ToString("MM/dd/yyyy");
            //
            _paramNodes.Add(result);
            _paramNodes.Add(pAcctType);
            _paramNodes.Add(pAcctNo);
            _paramNodes.Add(pDepLoan);
            _paramNodes.Add(pStartDt);
            _paramNodes.Add(pEndDt);
            //
            if (CoreService.AppSetting.IsServer == false)
            {
                CoreService.DataService.ProcessCustomAction(this, "GetAcctHist", (IPhoenixSerializable[])_paramNodes.ToArray(typeof(IPhoenixSerializable)));
            }
            string resultStr = (result.IsNull ? string.Empty : result.StringValue);
            if (resultStr != string.Empty)
            {
                //Build the ctructures
                historyConcat = resultStr.Split(Convert.ToChar(167));
                Array.CreateInstance(typeof(String), 2, historyConcat.Length);
                for (int i = 0; historyConcat.Length > i; i++)
                {
                    //Tokenize
                    rowvalues = historyConcat[i].Split('^');
                    CheckImageLinks checkStru = new CheckImageLinks();
                    checkStru.EffectiveDt = rowvalues[0];
                    checkStru.AcctNo = acctNo;
                    checkStru.AcctType = acctType;
                    checkStru.TracerNo = rowvalues[1];
                    checkStru.Amount = rowvalues[2];
                    checkStru.CheckNo = rowvalues[3];
                    hitoryRecords.Add(checkStru);//Add to the ArrayList
                }
                return PHXGetImageLinks(hitoryRecords, adGbImageControlObj);
            }
            else
            {
                //So we do not break
                Array htmlLinks = Array.CreateInstance(typeof(String), 0);
                return htmlLinks;
            }
        }
        #endregion AVIPGetAcctImages
        //
        #region AVIPGetImageLinks
        public Array AVIPGetImageLinks(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
        {

            Array htmlLinks = Array.CreateInstance(typeof(String), (checkInformation.Count * 2));
            int noOfLinks = checkInformation.Count;
            string baseUrl = string.Empty;
            if (checkInformation.Count > 0)
            {
                int counter = 0;
                string originTracerMask = string.Empty;

                //if (noOfLinks > 1)
                //    baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue + _adGbImageControl.ImgEngine.Value; // gsImgVendorUrl || gsVirtualImgDir || gsImgEngine
                //else
                //{
                    baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue;
                    string imgEngine = string.Empty;
                    imgEngine = _adGbImageControl.ImgEngine.Value;

                    // Remove & if at end of ImgEngine
                    if ( imgEngine.EndsWith(@"&") )
                    {
                        imgEngine = imgEngine.Substring(0, imgEngine.Length - 1);
                    }

                    // Must Have ? at end of ImgEngine
                    if (!imgEngine.EndsWith(@"?"))
                    {
                        imgEngine = imgEngine + @"?";
                    }

                    baseUrl = baseUrl + imgEngine;

                //}
                //#74271
                if (!_adGbImageControl.BankId.IsNull)
                    baseUrl = baseUrl + "id=" + _adGbImageControl.BankId.StringValue + @"&";

                foreach (CheckImageLinks checkInfoStruct in checkInformation)
                {
                    string htmlLinkBack = string.Empty;
                    string htmlLinkFront = string.Empty;
                    long checkNo = -1;
                    // Begin #74074 
                    //if (checkInfoStruct.CheckNo.Length > 0)
                    //	checkNo = Convert.ToInt64(checkInfoStruct.CheckNo); //We do not break in the future
                    //Begin #11630
                    //if (checkInfoStruct.CheckNo.Trim() != "" && Convert.ToInt64(checkInfoStruct.CheckNo) != 0)
                    //    checkNo = Convert.ToInt64(checkInfoStruct.CheckNo);
					checkNo = GetCheckNo(checkInfoStruct.CheckNo);
					//End  #11630
                    // End #74074

                    htmlLinkFront = baseUrl;
                    //I see this happening in centura
                    //#74271
                    if (!htmlLinkFront.EndsWith(@"&"))
                        htmlLinkFront = htmlLinkFront + @"&Date=" + checkInfoStruct.EffectiveDt;
                    else
                        htmlLinkFront = htmlLinkFront + @"Date=" + checkInfoStruct.EffectiveDt;

                    htmlLinkFront = htmlLinkFront + "&Side=f" + "&Acct=" + checkInfoStruct.AcctNo.Replace("-", "").Trim(); // #73556: Removed dashes
                    htmlLinkFront = htmlLinkFront + "&Amt=" + checkInfoStruct.Amount.Replace(".", "");
                    if (checkNo != -1)
                    {
                        htmlLinkFront = htmlLinkFront + "&Check=" + checkNo.ToString();
                    }
                    htmlLinkFront = htmlLinkFront + "&fmt=jpg";
                    htmlLinkBack = htmlLinkFront.Replace("&Side=f", "&Side=b");
                    //htmlLinks.SetValue(htmlLinkFront, 0, counter);
                    //htmlLinks.SetValue(htmlLinkBack, 1, counter);
                    htmlLinks.SetValue(htmlLinkFront, counter);
                    counter++;
                    htmlLinks.SetValue(htmlLinkBack, counter);
                    counter++;
                }
            }

            //Begin #73556
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                StringBuilder str = new StringBuilder();

                if (htmlLinks != null)
                {

                    for (int i = 0; i < htmlLinks.Length; i++)
                    {

                        str.Append(htmlLinks.GetValue(i) + "\n");
                    }

                    CoreService.LogPublisher.LogDebug(str.ToString());
                }
            }
            //End #73556

            //#if DEBUG
            //			string urlPart1 = @"http://localhost/images/nasa00";
            //			string urlPart2 = ".jpg";
            //			int maxArray = htmlLinks.Length;
            //			if (htmlLinks.Length > 8)
            //				maxArray = 8;
            //			for (int i=0; i < maxArray; i++)
            //			{
            //				//Deal with Zero Index
            //				htmlLinks.SetValue((urlPart1 + (i + 1).ToString() + urlPart2), i);
            //			}
            //#endif
            return htmlLinks;
        }
        #endregion AVIPGetImageLinks
        // End #77780
		//

		#region RenaiGetAcctImages
		//This code is redundant, 
		public Array RenaiGetAcctImages(string acctType, string acctNo, string depLoan, DateTime searchStartDt, DateTime searchEndDt, AdGbImageControl adGbImageControlObj)
		{
			//Array string[] htmlLinks;
			string [] historyConcat;
			string [] rowvalues;
			ArrayList hitoryRecords = new ArrayList() ;
			string ascii167 = Convert.ToChar(167).ToString();
			if( _paramNodes == null )
				_paramNodes = new ArrayList();
			else
				_paramNodes.Clear();
			//
			PBaseType result	= new PBaseType( "r1" );
			PBaseType pAcctType = new PBaseType( "p1");
			PBaseType pAcctNo	= new PBaseType( "p2");
			PBaseType pDepLoan	= new PBaseType( "p3" );
			PBaseType pStartDt	= new PBaseType( "p4" );
			PBaseType pEndDt	= new PBaseType( "p5" );
			//
			pAcctType.ValueObject	= acctType;
			pAcctNo.ValueObject		= acctNo;
			pDepLoan.ValueObject	= depLoan;
			pStartDt.ValueObject	= searchStartDt.ToString("MM/dd/yyyy");
			pEndDt.ValueObject		= searchEndDt.ToString("MM/dd/yyyy");
			//
			_paramNodes.Add( result );
			_paramNodes.Add( pAcctType );
			_paramNodes.Add( pAcctNo );
			_paramNodes.Add( pDepLoan );
			_paramNodes.Add( pStartDt );
			_paramNodes.Add( pEndDt );
			//
			if( CoreService.AppSetting.IsServer == false ) 
			{
				//It is exact, basically a histor Select
				CoreService.DataService.ProcessCustomAction( this, "GetAcctHist", (IPhoenixSerializable[])_paramNodes.ToArray(typeof(IPhoenixSerializable)));
			}
			string resultStr = ( result.IsNull ?string.Empty:result.StringValue);
			if (resultStr != string.Empty)
			{
				//Build the ctructures
				historyConcat = resultStr.Split(Convert.ToChar(167));
				Array.CreateInstance( typeof(String), 2, historyConcat.Length );
				for (int i = 0; historyConcat.Length > i; i++)
				{
					//Tokenize
					rowvalues = historyConcat[i].Split('^');
					CheckImageLinks checkStru = new CheckImageLinks();
					checkStru.EffectiveDt = rowvalues[0];
					checkStru.AcctNo = acctNo;
					checkStru.AcctType = acctType;
					checkStru.TracerNo = rowvalues[1]; 
					checkStru.Amount = rowvalues[2]; 
					checkStru.CheckNo = rowvalues[3]; 
					hitoryRecords.Add(checkStru);//Add to the ArrayList
				}
				return RenaiGetImageLinks(hitoryRecords, adGbImageControlObj);
			}
			else
			{
				//So we do not break
				Array htmlLinks = Array.CreateInstance( typeof(String), 0 );
				return htmlLinks;
			}
		}
		#endregion RenaiGetAcctImages
		//
		#region RenaiGetImageLinks
		public Array RenaiGetImageLinks(ArrayList checkInformation, AdGbImageControl _adGbImageControl)
		{

			Array htmlLinks = Array.CreateInstance( typeof(String), (checkInformation.Count * 2) );
			int noOfLinks = checkInformation.Count;
			string baseUrl = string.Empty;
			if (checkInformation.Count > 0)
			{
				int counter = 0;
				if (noOfLinks > 1)
					baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue + _adGbImageControl.ImgEngine.Value; // gsImgVendorUrl || gsVirtualImgDir || gsImgEngine
				else
				{
					baseUrl = _adGbImageControl.Url.StringValue + _adGbImageControl.VirtualImgDir.StringValue;
					string imgEngine = string.Empty;
                    
                    //#07226  start 
					
                    //if (_adGbImageControl.ImgEngine.Value.IndexOf(@"?") < 0) //did not find any //#07226 
                    //  imgEngine = _adGbImageControl.ImgEngine.Value + @"?";
                    
                    if (_adGbImageControl.ImgEngine.IsNull ||_adGbImageControl.ImgEngine.Value.IndexOf(@"?") < 0) //did not find any 
                        imgEngine = string.IsNullOrEmpty(_adGbImageControl.ImgEngine.Value) ? @"?" : _adGbImageControl.ImgEngine.Value + @"?";
                    //#07226  end

					else
					{
						imgEngine = _adGbImageControl.ImgEngine.Value;
						if (!imgEngine.EndsWith(@"&"))
							imgEngine = imgEngine + @"&";
					}
					baseUrl = baseUrl + imgEngine;
					if (!_adGbImageControl.BankId.IsNull)
						baseUrl = baseUrl + "id=" + _adGbImageControl.BankId.StringValue + @"&";
				}
				foreach (CheckImageLinks checkInfoStruct in checkInformation)
				{	
					string htmlLinkBack = string.Empty;
					string htmlLinkFront  = string.Empty;
					long checkNo = -1;
					//Begin #11630
					//if (checkInfoStruct.CheckNo.Length > 0)
					//	checkNo = Convert.ToInt64(checkInfoStruct.CheckNo); //We do not break in the future
					checkNo = GetCheckNo(checkInfoStruct.CheckNo);
					//End  #11630
					
					htmlLinkFront = baseUrl;
					htmlLinkFront = htmlLinkFront +  @"&date=" +  checkInfoStruct.EffectiveDt;

					if (checkInfoStruct.TracerNo != string.Empty)
						htmlLinkFront = htmlLinkFront + "&ex_trace=" + checkInfoStruct.TracerNo;
					htmlLinkFront = htmlLinkFront + "&side=f" + "&acct=" + checkInfoStruct.AcctNo.Trim();
					htmlLinkFront = htmlLinkFront + "&amt=" + checkInfoStruct.Amount.Replace(".","");
					if (checkNo != -1)
						htmlLinkFront = htmlLinkFront +  "&check=" + checkNo.ToString();											
					htmlLinkBack = htmlLinkFront.Replace("&side=f", "&side=b");
					htmlLinks.SetValue(htmlLinkFront, counter);
					htmlLinks.SetValue(htmlLinkBack, (counter + 1));
					counter++;
				}
			}

            //Begin #73556
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                StringBuilder str = new StringBuilder();

                if (htmlLinks != null)
                {

                    for (int i = 0; i < htmlLinks.Length; i++)
                    {

                        str.Append(htmlLinks.GetValue(i) + "\n");
                    }

                    CoreService.LogPublisher.LogDebug(str.ToString());
                }
            }
            //End #73556

			return htmlLinks;
		}
		#endregion RenaiGetImageLinks
		//
		#endregion Check Quest And RenaiGetImageLinks
		//
		//

		#region ERAS JV

		#region GetEraHistImages
		public bool GetEraHistImages(ArrayList checkInformation, AdGbImageControl adGbImageControl)
		{
			bool isSuccess = true;
			_erasJvImgRetClsObj = new ImageRetrieverClass();
			
			string decryptedPswd = string.Empty;
			string urlServer = string.Empty;
			string userName = string.Empty;			
			if (checkInformation.Count > 0)
			{
				try
				{
					int counter = 0;
					urlServer = adGbImageControl.Url.StringValue + adGbImageControl.UrlExt.StringValue +  "multipleAutoPdf.asp"; // sUrlServer = gsImgVendorUrl || gsImgVendorUrlExt || "multipleAutoPdf.asp"

					foreach (CheckImageLinks checkInfoStruct in checkInformation)
					{	
						string acctNo = Convert.ToInt64(checkInfoStruct.AcctNo.Replace("-", "")).ToString();
						string tracerNo = checkInfoStruct.TracerNo.Trim();
						tracerNo =  tracerNo.PadLeft(10, '0'); //'0000000000' || psaImgSrcTracerNo[nCounter], SalStrRightX( psaImgSrcTracerNo[nCounter], 10 )
						string seqNoTemp = string.Empty;
						for (int i = 0; i <= 9; i++)
						{
							if (tracerNo.Substring(i, 1) != "*")
								seqNoTemp = seqNoTemp + tracerNo.Substring(i, 1 );
						}
						if (seqNoTemp.Length > 0)
							seqNoTemp = Convert.ToInt64(seqNoTemp).ToString();
						else
							seqNoTemp = "0";//Not possible?					
						_erasJvImgRetClsObj.Add();
					
						object tempErasJvItem = _erasJvImgRetClsObj.NewItem;
						IItem erasJvItem = tempErasJvItem as IItem;
						erasJvItem.Account = acctNo;
						erasJvItem.Amount = checkInfoStruct.Amount;
						erasJvItem.CheckNumber = checkInfoStruct.CheckNo;
						erasJvItem.StartDate = checkInfoStruct.EffectiveDt;
						erasJvItem.EndDate = checkInfoStruct.EffectiveDt;
						erasJvItem.Sequence = seqNoTemp;
					
						counter++;
					}
					decryptedPswd = GetImgVendorPassword((adGbImageControl.Password.IsNull?string.Empty:adGbImageControl.Password.Value));

					if (!adGbImageControl.UserName.IsNull)
						userName = adGbImageControl.UserName.Value.Trim();
					_erasJvImgRetClsObj.Password = decryptedPswd;
					_erasJvImgRetClsObj.Username = userName;
					_erasJvImgRetClsObj.URLServer = urlServer;
					_erasJvImgRetClsObj.Retrieve();
					_erasJvImgRetClsObj.Clear();
				}
				catch(Exception ex)
				{
					//There could be  a COM Exception, You need to turn on the Log to see this...
					CoreService.LogPublisher.LogDebug("Failed for Some reason(GetEraHistImages):" + ex.ToString());
					isSuccess = false;
				}
				finally
				{
					if (_erasJvImgRetClsObj != null)
						_erasJvImgRetClsObj = null;
				}
			}
			return isSuccess;
		}
		#endregion GetEraHistImages
		//
		#region GetErasImages
		/// <summary>
		/// This function sets the parameters for the Eras object to retreive images
		/// via the internet and display them in the browser.		
		/// </summary>
		public bool GetErasImages(string searchStartDt, string searchEndDt, string acctNo , AdGbImageControl adGbImageControl)
		{	

			bool isSuccess = true;
			string decryptedPswd = string.Empty;
			string urlServer = string.Empty;
			string userName = string.Empty;
			//Get the Password
			decryptedPswd = GetImgVendorPassword((adGbImageControl.Password.IsNull?string.Empty:adGbImageControl.Password.Value));
			try
			{
				_erasJvImgRetClsObj = new ImageRetrieverClass();
				
				if (_erasJvImgRetClsObj != null)
				{	
					urlServer = adGbImageControl.Url.Value + adGbImageControl.UrlExt.Value + "searchItem.asp"; //gsImgVendorUrl || gsImgVendorUrlExt || "searchItem.asp"
					if (!adGbImageControl.UserName.IsNull)
						userName = adGbImageControl.UserName.Value.Trim();
					_erasJvImgRetClsObj.Password = decryptedPswd;
					_erasJvImgRetClsObj.Username = userName;
					_erasJvImgRetClsObj.URLServer = urlServer;
					_erasJvImgRetClsObj.Add();
					object tempErasJvItem = _erasJvImgRetClsObj.NewItem;
					IItem erasJvItem = tempErasJvItem as IItem;
					if (erasJvItem != null)
					{
						erasJvItem.Account = acctNo;
						erasJvItem.StartDate = searchStartDt;
						erasJvItem.EndDate = searchEndDt;						
					}
					_erasJvImgRetClsObj.Retrieve();
					_erasJvImgRetClsObj.Clear();				
				}
			}
			catch(Exception ex)
			{
				//There could be  a COM Exception, You need to turn on the Log to see this...
				CoreService.LogPublisher.LogDebug("Failed for Some reason(GetErasImages):" + ex.ToString());
				isSuccess = false;				
			}
			finally
			{
				if (_erasJvImgRetClsObj != null)
					_erasJvImgRetClsObj = null;
			}
			return isSuccess;
		}
		#endregion GetErasImages
		//
		#endregion 
		//
		//
		#region FinancialWare
		public void GetResearchImages(ArrayList checkInformation, AdGbImageControl adGbImageControl)
		{

		}

		#endregion FinancialWare

		//
		//
		#region GetImgVendorPassword
		public string GetImgVendorPassword(string encryptedPswd)
		{
			string decryptPswd = string.Empty;
			if( _paramNodes == null )
				_paramNodes = new ArrayList();
			else
				_paramNodes.Clear();
			//
			PBaseType result	= new PBaseType( "r1" );
			PBaseType pEncryptedPswd = new PBaseType( "p1");
			pEncryptedPswd.ValueObject = encryptedPswd;
			_paramNodes.Add( result );
			_paramNodes.Add( pEncryptedPswd );

			if( CoreService.AppSetting.IsServer == false ) 
			{
				CoreService.DataService.ProcessCustomAction( this, "DecryptPassword", (IPhoenixSerializable[])_paramNodes.ToArray(typeof(IPhoenixSerializable)));
			}
			decryptPswd = ( result.IsNull ?string.Empty:result.StringValue);
			
			return decryptPswd;
		}
		#endregion GetImgVendorPassword

        #region GetUserNameAndPassword
        // 8736
        public void GetUserNameAndPassword(out string userName, out string password)
        {
            userName = "";
            password = "";
            //AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);

            /* Begin 74735 */
            AdGbImageControl _AdGbImageControl = new AdGbImageControl();
            _AdGbImageControl.ActionType = XmActionType.Custom;
            _AdGbImageControl.CustomActionName = "GetDecryptedPassword";
            //Build the parameters
            PString sEmployeeId = new PString("EncPwd0");
            PString sServiceType = new PString("SvcType1");
            PString sDecryptedPwd = new PString("DecPwd2");
            PString sEmplUserName = new PString("EmplDocUserName");
            PString sReturnCode = new PString("RC4");
            //Set the Parameters values
            sServiceType.Value = "ECM";
            sReturnCode.Value = "0";
            sEmployeeId.Value = System.Convert.ToString(BusGlobalVars.EmployeeId); // WebSessionHelper.UserId.ToString();//(string)HttpContext.Current.Session["UserId"].ToString();
            //Make the call 
            try
            {
                Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(_AdGbImageControl, "GetDecryptedPassword", sEmployeeId, sServiceType, sDecryptedPwd, sEmplUserName, sReturnCode);
                userName = sEmplUserName.Value.ToString();
                password = sDecryptedPwd.Value.ToString();
            }
            catch (Exception ex)
            {
                CoreService.LogPublisher.LogDebug("Failed on custom action GetDecryptedPassword: " + ex.Message);
                throw ex;
            }
        }        
        #endregion 

        #endregion Public Methods

        #region Private Methods
		//Begin #11630
		private long GetCheckNo(string str)
		{
			Int64 checkNo = -1;
			Int64.TryParse(str, out checkNo);
			checkNo = (checkNo == 0) ? -1 : checkNo;
			return checkNo;
		}
		//End  #11630
        #endregion Private Methods

        #region Initialize method
        protected override void InitializeMap()
		{
			#region Table Mapping
			this.DbObjects.Add( "GB_IMAGE_HELPER", "X_GB_IMAGE_HELPER");
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

	} //End of Class
	#region CheckImageLinks
	/// <summary>
	/// We pass arraylist of structures all around the place
	/// </summary>
	public struct CheckImageLinks
	{
		private string _acctType;
		private string _acctNo;
		private string _effectiveDt;
		private string _checkNo;
		private string _tracerNo;		
		private string _amount;		
		//private string _sequenceNo;
		//private string _htmlLinkFront;
		//private string _htmlLinkBack;
        private string _tlCaptureISN;   //#140895
        private string _tlCaptureParentISN; //#140895

		/// <summary>
		/// CheckNo as a string
		/// </summary>
		public string CheckNo
		{
			get{return _checkNo;}
			set{_checkNo = value;}
		}
		/// <summary>
		/// Origin Tracer No
		/// </summary>
		public string TracerNo
		{
			get{return _tracerNo;}
			set{_tracerNo = value;}
		}

		/// <summary>
		/// Amount formatted so pass col...Text
		/// </summary>
		public string Amount
		{
			get{return _amount;}
			set{_amount = value;}
		}

		/// <summary>
		/// Account Type
		/// </summary>
		public String AcctType
		{
			set{_acctType = value;}
			get{return _acctType;}
		}
		/// <summary>
		/// Account No (formatted)
		/// </summary>
		public String AcctNo
		{
			set{_acctNo = value;}
			get{return _acctNo;}
		}
		/// <summary>
		/// EffectiveDt as MM/dd/yyyy
		/// </summary>
		public String EffectiveDt
		{
			set{_effectiveDt = value;}
			get{return _effectiveDt;}
		}
//		/// <summary>
//		/// We build this
//		/// </summary>
//		public string SequenceNo
//		{
//			get{return _sequenceNo;}
//			set{_sequenceNo = value;}
//		}
//		/// <summary>
//		/// Helper builds the link
//		/// </summary>
//		public String HtmlLinkFront
//		{
//			set{_htmlLinkFront = value;}
//			get{return _htmlLinkFront;}
//		}
//		/// <summary>
//		/// Helper builds the link
//		/// </summary>
//		public String HtmlLinkBack
//		{
//			set{_htmlLinkBack = value;}
//			get{return _htmlLinkBack;}
//		}

        /// <summary>
        /// TlCaptureISN as a string
        /// </summary>
        public string TlCaptureISN
        {
            get { return _tlCaptureISN; }
            set { _tlCaptureISN = value; }
        }

        /// <summary>
        /// TlCaptureParentISN as a string
        /// </summary>
        public string TlCaptureParentISN
        {
            get { return _tlCaptureParentISN; }
            set { _tlCaptureParentISN = value; }
        }
	}
	#endregion CheckImageLinks

}
