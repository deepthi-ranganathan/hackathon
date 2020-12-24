#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GlAcct.cs
// NameSpace: PUnit.BusObj.GL
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//7/12/2006		1		vreddy		Created
//-------------------------------------------------------------------------------

#endregion

using System;
using System.Xml;
using System.IO;
//
using NUnit.Framework;
using PUnit.Common;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Admin.Global;


namespace PUnit.BusObj.Global
{
	/// <summary>
	/// Summary description for TestHoldStop.
	/// </summary>
	public class GbHelper
	{	
		private Phoenix.BusObj.Global.GbHelper _testObject;
		private Phoenix.BusObj.Admin.Global.AdGbImageControl _adGbImageControl;
		private Phoenix.BusObj.Global.GbImageHelper _testGbImageHelper;
		
		public void TestListView()
		{
			_testObject = new  Phoenix.BusObj.Global.GbHelper();
			//System.Xml.XmlNode node = DataService.Instance.GetListView( _testObject,  null); 
		}
		[Test]
		public void GLAccountStructure()
		{
			string glAcctFullMask;
			string ledgermask;
			int groupMember;
			_testObject = new  Phoenix.BusObj.Global.GbHelper();
			_testObject.GetGLAccountStructure( out glAcctFullMask, out ledgermask, out groupMember);
			System.Console.WriteLine("Full GL Acct Mask=(" + glAcctFullMask + " Ledger Mask for Level 100=(" + ledgermask + ") groupMember=" + groupMember.ToString() + ")");
		}
		[Test]
		public void GLLedgerMask()
		{
			_testObject = new  Phoenix.BusObj.Global.GbHelper();
			int ledgerlevel = 100;
			string ledgermask = _testObject.GetGLLedgerMask(ledgerlevel); //Level 100
			System.Console.WriteLine("Ledger mask Level=(" + ledgerlevel.ToString() + " Mask=(" + ledgermask + ")");
		}
		[Test]
		public void GetEmplGLRestricts()
		{
			_testObject = new  Phoenix.BusObj.Global.GbHelper();
			string restrictPrefixes;
			string restrictLedgers;
			_testObject.GetEmplGLRestrictions(217, out restrictPrefixes, out restrictLedgers);
			System.Console.WriteLine("restrictPrefixes=(" + restrictPrefixes.Length.ToString() + ") ***** restrictLedgers=(" + restrictLedgers.Length.ToString() + ")");
		}
		[Test]
		public void CQGetImages()
		{
			_adGbImageControl = new AdGbImageControl();
			_adGbImageControl.ActionType = XmActionType.Select;
			_adGbImageControl.Visible.Value = "Y";
			_adGbImageControl.VendorType.Value = "Check image";
			DataService.Instance.ProcessRequest(_adGbImageControl);
			//_testObject = new  Phoenix.BusObj.Global.GbHelper();
			_testGbImageHelper = new Phoenix.BusObj.Global.GbImageHelper();
			_testGbImageHelper.CQGetAcctImages("DDA", "1008191919", "DP", Convert.ToDateTime("6/1/2006"), Convert.ToDateTime("8/1/2006"), _adGbImageControl);
		}
		[Test]
		public void BuildLinks()
		{
			_testGbImageHelper = new Phoenix.BusObj.Global.GbImageHelper();
			_testGbImageHelper.ExtractLinks(@"http://localhost/images/imageLinks.html");
		}

        [Test]
        public void CalcBusDate()
        {
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            PDateTime inputDate = new PDateTime("inputDate");
            PSmallInt prevNext = new PSmallInt("PrevNext");
            PSmallInt leadDays = new PSmallInt("leadDays");
            PDateTime busDate = new PDateTime("busDate");
            //
            inputDate.Value = new DateTime(2007, 4, 4);
            prevNext.Value = 1;
            leadDays.Value = 5;
            //
            _testObject.CalcBusDate(inputDate, prevNext, leadDays, busDate);
        }

        [Test]
        public void CalculateDateAfterTodayX()
        {
            DateTime newDate;
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            newDate = _testObject.CalculateDateAfterTodayX(new DateTime(1999, 1, 5), 1, "Month(s)");
            _T(newDate.ToShortDateString());
            
        }

        [Test]
        public void GetRate()
        {
            PDecimal newRate = new PDecimal("Rate");
            PDateTime TargetDt = new PDateTime("TargetDt");
            PSmallInt IndexId = new PSmallInt("IndexId");
            //
            IndexId.Value = 233;
            TargetDt.Value = new DateTime(1999, 1, 5);

            _testObject = new Phoenix.BusObj.Global.GbHelper();
            _testObject.GetRate(IndexId, TargetDt, newRate);
            _T(newRate.Value.ToString());

        }

        [Test]
        public void GenerateAccount()
        {
            string nextAcctNo = "";
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            _testObject.GenerateAccount("MOD10(001)", "DP", "DDA", "CK", ref nextAcctNo);
            _T(nextAcctNo);

        }

        private void _T(string myResult)
        {
            System.Diagnostics.Trace.WriteLine(myResult);
        }
        [Test]
        public void TestIsExternalAcct()
        {
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            bool isTrue = _testObject.IsExternalAcct("VIS");
            _T(isTrue.ToString());
        }

        [Test]
        public void IsExternalAdapterAcct()
        {
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            bool isTrue = _testObject.IsExternalAdapterAcct("VIS");
            _T(isTrue.ToString());
        }



        [Test]
        public void GetSPMessageText()
        {
            _testObject = new Phoenix.BusObj.Global.GbHelper();
            string dvBal = _testObject.GetSPMessageText(-900211);

        }
	}
	
	/// <summary>
	/// Summary description for .
	/// </summary>
	[TestFixture]
	public class SybGbHelper:GbHelper
	{	
		[TestFixtureSetUp]
		public void  Init()
		{
			PUnit.Common.ConfigInit.Instance.InitSybase( true );
		}
	
	}
	
	/// <summary>
	/// Summary description for .
	/// </summary>
	[TestFixture]
	public class MSGbHelper:GbHelper
	{	
		[TestFixtureSetUp]
		public void  Init()
		{
			PUnit.Common.ConfigInit.Instance.InitMSSqlServer( true );
		}
	
	}
	
	
	
}
