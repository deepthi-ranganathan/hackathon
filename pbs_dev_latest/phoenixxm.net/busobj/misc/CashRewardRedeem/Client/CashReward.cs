#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: CashReward.cs
// NameSpace: Phoenix.BusObj.Misc.Server
//-------------------------------------------------------------------------------
//Date			Ver 	Init    		Change              
//-------------------------------------------------------------------------------
//11/20/2020	1		kiran.mani		Created
//12/08/2020	2		RDeepthi		Task#133887. Rest changes create NoDbFields.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Collections;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Windows.Forms;


namespace Phoenix.BusObj.Misc
{


	public class CashReward : Phoenix.FrameWork.BusFrame.BusObjectBase
	{

		#region Private Fields

		private Phoenix.FrameWork.BusFrame.NoDbVarChar _sacctType;
		private Phoenix.FrameWork.BusFrame.NoDbVarChar _sacctNo;
		private Phoenix.FrameWork.BusFrame.NoDbDecimal _nRedeemAmt;
		private Phoenix.FrameWork.BusFrame.NoDbInt _rimNo;
		private Phoenix.FrameWork.BusFrame.NoDbSmallInt _branchNo;
		private Phoenix.FrameWork.BusFrame.NoDbChar _status;
		private Phoenix.FrameWork.BusFrame.NoDbDecimal _nCashRwdBal;
		private Phoenix.FrameWork.BusFrame.NoDbDecimal _nMinRewardAmt;

		private Phoenix.FrameWork.BusFrame.NoDbDecimal _totalCashRwdBal; //133887
		private Phoenix.FrameWork.BusFrame.NoDbDecimal _totalCashRwdEarnLtd; //133887
		private Phoenix.FrameWork.BusFrame.NoDbDecimal _totalCashRwddRedeemLtd; //133887

		#endregion Private fields

		#region Constructor

		public CashReward() :
				base()
		{
			InitializeMap();
		}

		#endregion Constructor

		#region Public Properties 

		public Phoenix.FrameWork.BusFrame.NoDbVarChar AcctType
		{
			get
			{
				return this._sacctType;
			}
		}

		public Phoenix.FrameWork.BusFrame.NoDbVarChar AcctNo
		{
			get
			{
				return this._sacctNo;
			}
		}

		public Phoenix.FrameWork.BusFrame.NoDbInt RimNo
		{
			get
			{
				return this._rimNo;
			}
		}

		public Phoenix.FrameWork.BusFrame.NoDbDecimal RedeemAmt
		{
			get
			{
				return this._nRedeemAmt;
			} 
		}

		public Phoenix.FrameWork.BusFrame.NoDbSmallInt BranchNo
		{
			get
			{
				return this._branchNo;
			}
		}
		
		public Phoenix.FrameWork.BusFrame.NoDbChar Status
		{
			get
			{
				return this._status;
			}
		}
		public Phoenix.FrameWork.BusFrame.NoDbDecimal CashRwdBal
		{
			get
			{
				return this._nCashRwdBal;
			}
		}
		public Phoenix.FrameWork.BusFrame.NoDbDecimal MinRewardAmt
		{
			get
			{
				return this._nMinRewardAmt;
			}
		}

		public PDecimal TotCashRwdEarnLtd
		{
			get
			{
				if (ObjectState["TotCashRwdEarnLtd"] == null)
				{
					ObjectState.Add(new PDecimal("TotCashRwdEarnLtd"));
				}
				return ObjectState["TotCashRwdEarnLtd"] as PDecimal;
			}
		}

		public PString TotCashRwddRedeemLtd
		{
			get
			{
				if (ObjectState["TotCashRwddRedeemLtd"] == null)
				{
					ObjectState.Add(new PString("TotCashRwddRedeemLtd"));
				}
				return ObjectState["TotCashRwddRedeemLtd"] as PString;
			}
		}

		public PString TotCashRwdBal
		{
			get
			{
				if (ObjectState["TotCashRwdBal"] == null)
				{
					ObjectState.Add(new PString("TotCashRwdBal"));
				}
				return ObjectState["TotCashRwdBal"] as PString;
			}
		}
		/*Begin #133887*/
		public Phoenix.FrameWork.BusFrame.NoDbDecimal TotalCashRwdEarnLtd
		{
			get
			{
				return this._totalCashRwdEarnLtd;
			}
		}
		public Phoenix.FrameWork.BusFrame.NoDbDecimal TotalCashRwddRedeemLtd
		{
			get
			{
				return this._totalCashRwddRedeemLtd;
			}
		}
		public Phoenix.FrameWork.BusFrame.NoDbDecimal TotalCashRwdBal
		{
			get
			{
				return this._totalCashRwdBal;
			}
		}
		/*End #133887*/

		public PString GraphXml
		{
			get
			{
				if (ObjectState["GraphXml"] == null)
				{
					ObjectState.Add(new PString("GraphXml"));
				}
				return ObjectState["GraphXml"] as PString;
			}
		}

		#endregion Public Properties 

		protected override void InitializeMap()
		{
			#region Table Mapping 
			AlwaysUsePrimaryDb = true;
			SupportedActions24x7 = XmActionType.Custom | XmActionType.Default | XmActionType.EnumOnly | XmActionType.ListView | XmActionType.Select;
			this.DbObjects.Add("CASH_REWARD", "X_CASH_REWARD");
			#endregion Table Mapping 

			#region  Column Mapping 

			_sacctType = new NoDbVarChar(this, "AcctType", 3, false);
			_sacctNo = new NoDbVarChar(this, "AcctNo", 12, false);
			_nRedeemAmt = new NoDbDecimal(this, "RedeemAmt", 14, 2, false);
			_rimNo = new NoDbInt(this, "RimNo", true);
			_branchNo = new NoDbSmallInt(this, "BranchNo", true);
			_status = new NoDbChar(this, "Status", 19, true);
			_nCashRwdBal = new NoDbDecimal(this, "CashRwdBal", 14, 2, true);
			_nMinRewardAmt = new NoDbDecimal(this, "MinRewardAmt", 14, 2, true);
			/*Begin #133887*/
			_totalCashRwdEarnLtd = new NoDbDecimal(this, "TotalCashRwdEarnLtd", 14, 2, true);
			_totalCashRwddRedeemLtd = new NoDbDecimal(this, "TotalCashRwddRedeemLtd", 14, 2, true);
			_totalCashRwdBal = new NoDbDecimal(this, "TotalCashRwdBal", 14, 2, true);
			/*End #133887*/
			#endregion

			#region  Keys 
			#endregion
			SupportedAction |= XmActionType.Custom | XmActionType.Default | XmActionType.Delete | XmActionType.New | XmActionType.Update;
			SupportedActions24x7 |= XmActionType.Custom | XmActionType.Default | XmActionType.Delete | XmActionType.EnumOnly | XmActionType.New | XmActionType.ListView | XmActionType.Select | XmActionType.Update;
			ExtPartyRestrictedActionType = XmActionType.None; /* Set this flag for actions which are supposed to be restricted to external party. */
			IsVisibleToExtParty = true; /* Set this flag to false if the business object is not supposed to be visible to external party.*/

			#region  Enumerable Values
			#endregion
			base.InitializeMap();
		}

		protected override void SetRestExtension()
		{
			this.RestExtension = new CashRewardRestExt();
		}

		
	}
}
