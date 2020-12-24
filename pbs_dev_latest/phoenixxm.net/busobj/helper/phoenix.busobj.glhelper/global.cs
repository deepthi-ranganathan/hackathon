#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: AvailBal.cs
// NameSpace: Phoenix.BusObj.AvailBal.Helper
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/20/2004	1.0		mselvaga	Created.
//-------------------------------------------------------------------------------

#endregion

using System;
using System.Data;
using System.Collections;
using System.Globalization;
using Phoenix.FrameWork.Core;
using Phoenix.Shared.Constants;

namespace Phoenix.BusObj.Global.Helper
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class Global
	{
		#region Constructor
		public Global()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region Compute Available Balance
		/// <summary>
		/// Gets the available balance based on global available balance definition defined in XM service table.
		/// </summary>
		/// <param name="balanceDef">Available balance definition defined in XM service</param>
		/// <param name="gnDpAvailBalDef">Global deposit available balance definition defined in ad_dp_control.</param>
		/// <param name="curBal">Accounts Cur_bal</param>
		/// <param name="holdBal">Account Hold_bal</param>
		/// <param name="floatBal1">Account Float_bal_1</param>
		/// <param name="floatBal2">Account Float_bal_2</param>
		/// <param name="memoPostedCredits">Account Memo_cr</param>
		/// <param name="memoPostedDebits">Account Memo_dr</param>
		/// <param name="memoFloat">Account Memo_float</param>
		/// <returns>Calculated available balance definition</returns>
		public decimal GetBalance(short balanceDef, short gnDpAvailBalDef, decimal curBal, decimal holdBal, decimal floatBal1, decimal floatBal2, decimal memoPostedCredits, decimal memoPostedDebits, decimal memoFloat )
		{
			decimal rnAvailBalance = 0;
			if(balanceDef == gnDpAvailBalDef)		// always remove hold from available
				curBal = curBal - holdBal;

			switch (balanceDef)
			{
				case 0:
					rnAvailBalance = curBal;
					break;
				case 1:
					rnAvailBalance = curBal - holdBal;
					break;
				case 2:
					rnAvailBalance = curBal - floatBal1;
					break;
				case 3:
					rnAvailBalance = curBal - floatBal2;
					break;
				case 4:
					rnAvailBalance = curBal - holdBal - floatBal1;
					break;
				case 5:
					rnAvailBalance = curBal - holdBal - floatBal2;
					break;
				case 6:
					rnAvailBalance = curBal + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				case 7:
					rnAvailBalance = curBal - holdBal + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				case 8:
					rnAvailBalance = curBal - floatBal1 + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				case 9:
					rnAvailBalance = curBal - floatBal2 + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				case 10:
					rnAvailBalance = curBal - holdBal - floatBal1 + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				case 11:
					rnAvailBalance = curBal - holdBal - floatBal2 + memoPostedCredits - memoPostedDebits - memoFloat;
					break;
				default:
					CoreService.ExceptionMgr.NewException(1,474);	//474 - Could not resolve loan available balance.
					break;
			}

			return rnAvailBalance;

		}
		#endregion

		#region Customer Name
		/// <summary>
		/// Gets the full name of the customer by combining first_name, last_name and middle initial.
		/// </summary>
		/// <param name="firstName">First name of the customer</param>
		/// <param name="lastName">Last name of the customer</param>
		/// <param name="middleInitial">Middle initial of the customer</param>
		/// <param name="lastNameFirst">Pass true in case of customer last_name first</param>
		/// <returns>Customer full name string</returns>
		public string ConcateNameX(string firstName, string lastName, string middleInitial, bool lastNameFirst)
		{
			#region Trim input string
			firstName = firstName.Trim();
			lastName = lastName.Trim();
			middleInitial = middleInitial.Trim();
			#endregion

			string rsName = "";

			if (lastNameFirst == true)
			{
				if(lastName != string.Empty)
					rsName = lastName;
				if(firstName != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = firstName;
					else
						rsName = rsName + "," + firstName;
				}
				if(middleInitial != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = middleInitial;
					else
						rsName = rsName + " " + middleInitial;
				}					
			}
			else
			{
				if(firstName != string.Empty)
					rsName = firstName;
				if(middleInitial != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = middleInitial;
					else
						rsName = rsName + " " + middleInitial;
				}
				if(lastName != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = lastName;
					else
						rsName = rsName + " " + lastName;
				}
			}

			return rsName;
		}

		/// <summary>
		/// Gets the full name of the customer by combining first_name, last_name and middle initial,
		/// title and suffix.
		/// </summary>
		/// <param name="firstName">First name of the customer</param>
		/// <param name="lastName">Last name of the customer</param>
		/// <param name="middleInitial">Middle Initial of the customer</param>
		/// <param name="lastNameFirst">If true customer last_name will be placed before first_name</param>
		/// <param name="suffix">Suffix</param>
		/// <param name="title">Title</param>
		/// <returns>Customer full name string</returns>
		public string ConcateNameX(string firstName, string lastName, string middleInitial, bool lastNameFirst, string suffix, string title)
		{
			#region Trim input string
			suffix = suffix.Trim();
			title = title.Trim();
			#endregion

			string rsName = "";

			rsName = ConcateNameX(firstName,lastName,middleInitial,lastNameFirst);

			// Add suffix
			if(suffix != string.Empty)
				rsName = rsName + " " + suffix;

			// Add title
			if(title != string.Empty)
				rsName = title + " " + rsName;

			return rsName;
		}

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
				tempReader.Close();
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
					"exec " + dbHelper.DbPrefix + "psp_get_tran_set_id @nTranSetId output\n" +
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
				tempReader.Close();
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
	}
}
