#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: teller.cs
// NameSpace: Phoenix.BusObj.Teller.Helper
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//Dec-06-2004	1.0		rpoddar		Created.
//-------------------------------------------------------------------------------

#endregion

using System;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
//
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;


namespace Phoenix.BusObj.Teller.Helper
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class Teller
	{
		#region Constructor
		public Teller()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region private vars
		private static PSqlHelper pSqlHelper = new PSqlHelper();
		#endregion

		#region TrancodeTypes
		/// <summary>
		/// Identifies whether the passed trancode is a credit transaction.
		/// </summary>
		/// <returns>true if the passed trancode is credit else false</returns>
		public bool IsCreditTran(short tranCode )
		{
			if (( tranCode >= 100 && tranCode <= 149 ) || ( tranCode >= 300 && tranCode <= 349 )
				|| ( tranCode >= 500 && tranCode <= 549 && tranCode != 505 )
				|| tranCode == 555 || tranCode == 900 || tranCode == 902 || tranCode == 910
				|| tranCode == 913 || tranCode == 916 || tranCode == 920 || tranCode == 922
				|| tranCode == 925 || tranCode == 927 || tranCode == 929 || tranCode == 931
				|| tranCode == 933 || tranCode == 934 )
				return true;
			return false;
		}

		/// <summary>
		/// Identifies whether the passed trancode is a debit transaction.
		/// </summary>
		/// <returns>true if the passed trancode is debit else false</returns>
		public bool IsDebitTran(short tranCode )
		{
			if (( tranCode >= 150 && tranCode <= 199 ) || ( tranCode >= 350 && tranCode <= 399 )
				|| ( tranCode >= 550 && tranCode <= 899 && tranCode != 555 )
				|| tranCode == 505 || tranCode == 912 || tranCode == 915 ||  tranCode == 921
				|| tranCode == 926 || tranCode == 930 || tranCode == 932 || tranCode == 935
				|| tranCode == 936 || tranCode == 937 )
				return true;
			return false;
		}

		/// <summary>
		/// Identifies whether the passed trancode is a transfer transaction.
		/// </summary>
		/// <returns>true if the passed trancode is transfer else false</returns>
		public bool IsTransferTran(short tranCode )
		{
			if ( tranCode == 102 || tranCode == 120 || tranCode == 128 ||  tranCode == 156
				|| tranCode == 162 || tranCode == 163 || tranCode == 164 )
				return true;
			return false;
		}
		#endregion

		#region Database To Use
		public UseDb DbToUpdate( IDbHelper dbHelper )
		{
			return DbToUpdate( dbHelper.CopyStatus );
		}

		public UseDb DbToInquire( IDbHelper dbHelper )
		{
			return DbToInquire( dbHelper.CopyStatus );
		}

		public UseDb DbToInsert( IDbHelper dbHelper )
		{
			return DbToInsert( dbHelper.CopyStatus );
		}
		private UseDb DbToUpdate( string copyStatus )
		{
			if( copyStatus == "B" || copyStatus == "C" )
				return UseDb.Primary | UseDb.Secondary;
			else if ( copyStatus == "D" )
				return UseDb.Primary;
			else if ( copyStatus == "N" )
				return UseDb.Secondary;
			else
				return UseDb.Offline;
		}

		private UseDb DbToInquire( string copyStatus )
		{
			if( copyStatus == "D" || copyStatus == "C" )
				return UseDb.Primary;
			else if( copyStatus == "B" || copyStatus == "N" )
				return UseDb.Secondary;
			else
				return UseDb.Offline;
		}

		private UseDb DbToInsert( string copyStatus )
		{
			if( copyStatus == "D" || copyStatus == "B" )
				return UseDb.Primary;
			else if( copyStatus == "N" || copyStatus == "C" )
				return UseDb.Secondary;
			else
				return UseDb.Offline;
		}
		#endregion

		#region database table prefixes
		public string[] UpdateDbPrefix( IDbHelper dbHelper )
		{
			UseDb dbToUpdate = DbToUpdate( dbHelper );
			string delimiterStr = ",";
			string prefix = null;
			char[] delimiter = delimiterStr.ToCharArray();
			string[] updateDbPrefix = null;
			if (( dbToUpdate & UseDb.Primary)  == UseDb.Primary  )
				//				updateDbPrefix[index++] = dbHelper.PhoenixDbName + "..";
				prefix = prefix + dbHelper.PhoenixDbName + "..";
			if (( dbToUpdate & UseDb.Offline)  == UseDb.Offline  )
				//				updateDbPrefix[index++] = " ";
				prefix = prefix + delimiter + " ";
			if (( dbToUpdate & UseDb.Secondary)  == UseDb.Secondary  )
				//				updateDbPrefix[index] = "X_";
				prefix = prefix + delimiter + "X_";
			updateDbPrefix = prefix.Split(delimiter);
			return updateDbPrefix;
		}

		public string InquireDbPrefix( IDbHelper dbHelper )
		{
			UseDb dbToInquire = DbToInquire( dbHelper );
			if (( dbToInquire & UseDb.Primary)  == UseDb.Primary  )
				return dbHelper.PhoenixDbName + "..";
			if (( dbToInquire & UseDb.Offline)  == UseDb.Offline  )
				return " ";
			if (( dbToInquire & UseDb.Secondary)  == UseDb.Secondary  )
				return "X_";
			return "";
		}

		public string InsertDbPrefix( IDbHelper dbHelper )
		{
			UseDb dbToInsert = DbToInquire( dbHelper );
			if (( dbToInsert & UseDb.Primary)  == UseDb.Primary  )
				return dbHelper.PhoenixDbName + "..";
			if (( dbToInsert & UseDb.Offline)  == UseDb.Offline  )
				return " ";
			if (( dbToInsert & UseDb.Secondary)  == UseDb.Secondary  )
				return "X_";
			return "";
		}
		#endregion

		#region sql functions
		public bool ExecSqlUpdate( IDbHelper dbHelper, string sql )
		{
			int phxRowsUpdated = 0;
			int xpRowsUpdated = 0;

			ExecSqlUpdate( dbHelper, sql, ref phxRowsUpdated, ref xpRowsUpdated );
			if ( phxRowsUpdated == 0 && xpRowsUpdated == 0 )
				return false;
			return true;
		}

		public void ExecSqlUpdate( IDbHelper dbHelper, string sql, ref int phxRowsUpdated, ref int xpRowsUpdated )
		{
			string copyBackStatus = "";
			foreach( string tblPfx in UpdateDbPrefix( dbHelper ))
			{
				if ( tblPfx != "" )
				{
					
					if ( tblPfx == "X_" )
					{
						if ( DbToUpdate( dbHelper ) == UseDb.Secondary )
							copyBackStatus = "";
						else
							copyBackStatus = " copy_back_status = isnull(copy_back_status, 0) | 2 , " + "\n" ;
						sql = string.Format(sql, tblPfx, copyBackStatus );
						xpRowsUpdated = dbHelper.ExecuteNonQuery( sql );
					}
					else
					{
						copyBackStatus = "";
						sql = string.Format(sql, tblPfx, copyBackStatus );
						phxRowsUpdated = dbHelper.ExecuteNonQuery( sql );
					}

				}
			}
			return;
		}

		public bool ExecSqlInsert( IDbHelper dbHelper, string sql )
		{
			string tblPfx = InsertDbPrefix( dbHelper ) ;
			if ( tblPfx == "X_" )
			{
				sql = string.Format(sql, tblPfx, " copy_back_status, ", " 1, " );
				if ( dbHelper.ExecuteNonQuery( sql ) == 0 )
					return false;
			}
			else
			{
				sql = string.Format(sql, tblPfx, "", "" );
				if ( dbHelper.ExecuteNonQuery( sql ) == 0 )
					return false;
			}
			return true;
		}

		public bool ExecSqlImmediate( IDbHelper dbHelper, string sql, ref object[] result )
		{
			sql = string.Format(sql, InquireDbPrefix( dbHelper ) );
			result = pSqlHelper.SqlImmediate( dbHelper, sql );
			if ( result != null )
				return true;
			else 
				return false;
		}

		public bool ExecSqlExists( IDbHelper dbHelper, string sql )
		{
			object [] result = null;
			return ExecSqlImmediate( dbHelper, sql, ref result );
		}
		private bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj, short dbType )

		{
			UseDb dbToUpdate;
			if ( dbType == -1)
				dbToUpdate = DbToUpdate( dbHelper );
			else
				dbToUpdate = (UseDb) dbType;

			busObj.ActionType = XmActionType.New;
			if ( busObj.DoAction( dbHelper ) != RecordStatus.Success )
				return false;
			//busObj.OnActionInsert( dbHelper, "D" );
			if (( dbToUpdate & UseDb.Primary)  == UseDb.Primary  || ( dbToUpdate & UseDb.Offline)  == UseDb.Offline )
			{
				//if ( !busObj.OnActionInsert( dbHelper, true ))
				//	return false;
			}
			else if (( dbToUpdate & UseDb.Secondary)  == UseDb.Secondary  )
			{
				//if (!busObj.OnActionInsert( dbHelper, false ))
				//	return false;
			}
			return true;
			
		}
		public bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj )
		{
			return ExecObjInsert( dbHelper, busObj, -1 );
		}
		public bool ExecObjInsert( IDbHelper dbHelper, BusObjectBase busObj, UseDb useDb )
		{
			return ExecObjInsert( dbHelper, busObj, (short) useDb );
		}

		#endregion

		#region other helper functions

		#region Get Sequence#
		public short GetSequenceNo( IDbHelper dbHelper, short branchNo, short drawerNo )
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

			if ( !ExecSqlImmediate( dbHelper, sql, ref result ))
				return -1;

			return Convert.ToInt16( result[0]);
		}
		#endregion

		#region Resolve GL Account
		public bool ResolveGLAccount(string postingPrefix, ref string acctNo)
		{
			string pattern = @"[0-9][0-9]\-\*\*";
			if(!Regex.IsMatch(acctNo,pattern))
				return false;
			else
				acctNo = Regex.Replace(acctNo,pattern,postingPrefix);
			return true;
		}

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

			if(!ExecSqlImmediate(dbHelper,sql,ref result))
				return false;
			postingPrefix = Convert.ToString(result[0]);
			return true;
		}
		#endregion

		#region Get FKvalue description
		public string GetFKValueDesc(int fieldCodeValue, string fieldFkValue)
		{
			char[] charDelimiterArray = new char[] {(char)167};
			string[] result = null;
			result = fieldFkValue.Split(charDelimiterArray);
			if(result.Length == 2)
				return Convert.ToString(result[1]);
			else
				return fieldFkValue;
		}
		#endregion

		#endregion
	}

}
