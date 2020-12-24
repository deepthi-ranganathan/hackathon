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
//09/20/2004	1.0		mselvaga	Created.
//03-13-2006	2.0		mselvaga	Added helper method to calculate expiry date
//									Also added server object.
//03-17-2006	3.0		rpoddar		Check for null in the concat functions
//03-31-2006	4.0		mselvaga	Moved GetTranSetId and GetCustomerName helper methods to server obj.
//04-19-2006    5.0     vreddy      Added IntFormatAddress And IntFormatTIN, Format Account
//05/15/06		6		VDevadoss	Added DecodeOriginID
//06/01/06		7.0		DGupta		Added method GetNamedetails
//06/09/06		8		Vreddy		#67878 - ADded AccoutValidation (IntACctValidate)
//06/21/06      9		Vreddy		#67878 - ADded two enums for search window
//07/02/06		10		Vreddy		#67878 - Added a GetStatusSort int function
//08/07/06		11		Vreddy		#67881 - Added FormatDate Global Method
//09/19/2006	12		vdevadoss	#69248 - Changed the string "year(s)" to use "Years(s)" in CalculateDateX fn.
//09/27/2006	13		vreddy		#6783 - Moved  GetFKValueDesc
//10/05/06		14		VDevadoss	#69248 - Added the fn's GetClassDesc, GetNameDetails, GetEmployeeName, IsPMAEnabled, GetDepLoan
//10/26/06		15		VDevadoss	#69248 - Added the fn's GetCustomerDetails
//10/27/06		16		VDevadoss	#69248 - Added the fn's GetWQCategoryDetails
//12/10/06		17		Vreddy		#71004 - Added Space aftercomma in case of non personal name concatination
//01/02/07		18		vreddy		#71170 - Positive error noshas no error text
//02/09/07		19		Vdevadoss	#70728 - Added the code to handle Commitments.
//May-24-07		20   	Muthu       #72780 VS2005 Migration
//06/04/2007    21      mselvaga    #73028 - Flag for offline support added.
//10/22/2007    22      mselvaga    #73503 - Add helper functions to support Centura SAL functions.
//                                         - Added overload for GetClassDesc.
//                                         - Added CalcBusDate, CalculateDateAfterTodayX
//11/16/2007    23      rpoddar     #73503 - ADded method GetDuplicateRecordMessage
//12/04/07      24      bbedi       #74019 - Mailing Address Display
//01/10/2008    25      rpoddar     #74308 - Added method ValidateDecimal
//02/01/2008    26      jabreu      #74018 - Added GetLoanAmountUndisbursed for Construction Loan Enhancements
//02/21/2008    27      rpoddar     #74915 - Added new overload for method GetNameDetails
//06/11/2008    28      rpoddar     #75763 - Added method IsViewOnlyDormantEscheatedAccess
//07/07/2008    29      rpoddar     #01031 - Added  new method ValidateEnhPassword
//09/26/2008    30      iezikeanyi  #76429 - Added CrossRef to GetAcctAlerts
//11/25/2008    31      mselvaga    #76458 - Added EX account changes.
//02/02/2009    32      VDevadoss   #76036 - Added function GetRmAddrId for address in effect.
//02/03/09      33      Vdevadoss   #2106 - Added origin id 81 to the decode method
//03/02/09      34      Vdevadoss   #76099 - Added DBDateDifference function.
//04/01/09      35      sriley      #76031 - Added origin id 82 to the decode method
//05/28/09      36      bhuhges     4198 - Fully qualified request for isExternalAcct

//01/02/2009    31      rpoddar     #01805 - Added ValidateTranCodeLimit
//12/16/2008    31      ewahib      #01141 - Trim Rim Type as it is returned from the database with extra Spaces.
//12/31/2008    32      ewahib      #01295 - Added CalculateDateIncTodayX function
//01/05/2008    33      MHelmy      #01295 - A fix the method CalculateDate ( Number of day for Feb )
//05/24/2009    34      Nelsehety   #02512- fix GenerateAccount .
//07/21/2009    35      ewahib      #02258 - Add another overload for CalculateDate and add the calling BO as parameter.
//09/15/2009    36      mselvaga    #79156 - Fixed the bug introduced as part of .net port merge.
//09/25/2009    37      SDhamija    #79217 - External Transfers
//10/27/2009    38      Nelsehety   #04976 - Added the fn GetWorkDetailsForNewAcct and NicknameUpdate.
//10/27/2009    39      Mona        #04976 - Converting WorkId in fn "GetWorkDetailsForNewAcct" from int to decimal
//10/27/2009    40      aHussein    #03334 - add another overload for GetRate function
//10/27/2009    41      ewahib      #03333 - Add LaunchInterfaceMethod - for BISYS
//10/27/2009    42      ewahib      #03333 - Remove RmCOntactHistory from LaunchInterfaceMethod as it causes circular dependency
//01/05/2010    43      mramalin    WI-5938 - Modified the account number format for GL with wildcard
//01/28/2010    44      mselvaga    WI#7722 - Teller Offline application error problem.
//02/06/2010    45      ewahib      WI-7738 - fix GetRimNo - Use StringHElper.StrTrimX
//05/25/2010    46      rpoddar     #79510 - Added Shared Branch Origin
//05/27/2010    47      vdevadoss   #9177 - Added IntCalcAge fn.
//06/04/10      48      DGupta      79644 - Added ACH Company Charge Back origin.
//6/30/10       49      DGupta      79442 - Added CSV origin.
//7/25/2010     50      FOyebola    WI9961
//8/23/2010     51      Vdevadoss   #80643 - Introduced as simpler overload to for customer name
//9/15/2010     52      Vdevadoss   #80629 - Handled mode "Default" in GetNameDetails similar to 1099C
//10/29/2010    53      Vdevadoss   #10886 - GetNameDetails fn, if mode is NULL then assume DEFAULT mode.
//12/10/2010    54   	rpoddar     #80630 - Confidential acct support.
//02/04/2011    55      Vdevadoss   #80637 - Decoded the NSF error codes.
//02/17/2011    56      TTaylor     #12098 - Corrected the problem with calculate date giving an error when the start date was before the system date.
//04/18/2011    57      jabreu      80630 - Confidential flag support
//04/22/2011    58      rpoddar     #13814 - Check for offline for conf. account
//05/31/2011    59      jabreu      14330 - Check for AdGbRsm before access for conf acct setting
//06/09/2011    60      rpoddar     #14147 - Support for confidential Households
//06/30/2011    61      jabreu      14340 - Added bools for remaining Confidential flags "AllowEmployeeTo...ConfidentialFlag"
//08/26/2011    62      mselvaga    #80674 - KMS Integration.
//12/05/2011    63      LSimpson    #80660 - Added SuspectSettingsAreEnabled().
//7/27/2012     64      MBachala    145094 - Payment schedule Porting. Date is not calculated correct for period "15 & EOM"
//2012-12-19    65      jabreu      140769 - Relationship Pricing
//01/10/2013	66		rpoddar		#19415 - Performance Fixes
//01/25/2013    67	    rpoddar     #140789 - Employee Exchange
//02/08/2013    68      SDighe      WI#19103 - Glitch with Fraud Indicator lamp in Teller.
//03/07/2013    69      DEiland     WI#20348 - GetAcctAlerts passing Module name now
//03/11/2013    70      rpoddar     #142715 - CTR Electronic Filing Changes
//04/02/2013    71      DEiland     WI#21709/#21708 - Fix Select Using Incorrect Place Holders
//04/25/2013    72      SChacko     #21630 - Customer Contact History is defaulting Search for From Date ON with date of  1/1/0001 on Leap Day 2/29
//04/25/2013    73      SVasu       WI#21627 - Created new method for splitting string with out trim
//5/30/2013     74      MBachala    22968 - performance changes for OPS banks
//06/20/2013	75		JRhyne		WI#23705/23706 - workflow search
//6/27/2013     76      JRhyne		wi 22968 (2) fix deploan not set issue
//10/10/2013    77      SJoseph		WI #24651 Added CSV origin 
//10/21/2013    78      bhughes     140793
//10/10/2013    79      SJoseph		WI #24651 Added CSV origin 
//03/05/2014	80		jrhyne		#140791 - add GetWorkstationName
//03/10/2014    81      RDeepthi    WI #27540. Added GetPostEditAccess Method.
//04/04/2014    82      BSchlottman #27972 Format Canadian Address with city, state, and postal code.
//8/20/2014     83      MBachala    30479 - helper function to format custom tin
//10/1/2014		84		JRhyne		#176406 fix calcdate function 
//02/05/2015    85      ckane       #32310 - remove hardcoded reference to "USA"
//02/13/2015    86      Arun        #176995 - Added helper fn CalculateTermMnths() to calculate Term and Period based on Parent Loan period
//03/11/2015    87      Hirankumar  #33898 - Application error occurred while reversing a 3rd party transaction.
//5/6/2015      88      Sandeep.S   #173485 -  SA New Customer & New Deposit Account Origination
//06/03/2015	89		Partha		#36979 - Account no autogen over 2 billion (10 Digits) was resetting the value.
//09/16/2015	90		Partha		#38867	New Commercial Loan created with incorrect account format when Auto Assign Account # is enabled
//06/14/2016    91      Alfred      #46760 -  Added method IsNachaHoliday().
//07/11/2016    92      sbabcock    User Story #47186 - Task #47187 - Add lookup for Fusion Workflow Manager Origin ID
//10/12/2016	93		Anju        #198018- 53311- Show user name in Display Window.
//11/21/2016    94      DEiland     Task#55416 - We need to not look at account Confidential flags when checking Confidential for the certain RIM windows
//05/3/2017     95      GlaxeenaJ   Enh #209693 US #63933 Task #63936-New method to select biProduct from Helper Class instead of MDI. S removed method added in task #62382
//05/25/2017    96      Kiran       Task#65549 - PCI Enhancement - Added new method to get decrypted Pan
//08/30/2017    97      Kiran       Task#71500 - PCI Enhancement - Added new method to get decrypted Ex Acct no
//12/19/2017    97      SChacko     Task#78073 - Added new method to validate Alphabet only password.
//03/28/2018    98      Shebin      Task#82748 - Added new method to validate GL account, For reverse & reapply,system should allow GL account with ledger type in Asset/Liability 
//07/02/2018    99      CRoy        Task 89995 - Transfer alert information.
//07/06/2018    100     CRoy        Task 90001 - Disable quick tran in night mode.
//07/11/2018    101     CRoy        Bug #95593 - Added some fields to the AcctAlerts class
//07/12/2018    102     Minu        #94549- Show message when user trying to post transactions, when drawer is closed out.
//06/21/2019    103     Alfred      #116113 - Transaction Limit override logic is not working for TC 520 and TC 570..
//09/13/2019    104     Vipin       Task 118792: If origin Id is 100 then we need to set it as 'Workflow'
//09/17/2019    105     Riya        #119436 - Removed the logic for formatting Canadian addresses since City,State and Postal Code will be hidden for international addresses.
//01/24/2020    106     DEiland     Task#122492 - When No Acct Number and We Have a Rim Then Place in Account Fields So It prints on AUD020 Report
//01/27/2020    106     Alfred      #123757 - Added additional comma to separate the code.
//02/26/2020    107     Arun        US #114528 | Task #124261 - SQL Injection fix for GetAcctAlerts custom action.
//07/29/2020    108     Kiran       US #130110 | Task 130111 - Added new custom option to get rim number by PAN
//-------------------------------------------------------------------------------


#endregion

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
//
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;
using Phoenix.FrameWork.Shared.Variables;
//using Phoenix.BusObj.Control;
using Phoenix.BusObj.Admin.Global;
using Phoenix.FrameWork.Core.Utilities;
//using Phoenix.BusObj.Admin.Deposit;
//using Phoenix.BusObj.Admin.Loan;
using Phoenix.BusObj.Admin.Xapi;
//using Phoenix.BusObj.Admin.Loan;
using Phoenix.Shared;
using System.Reflection;        // #01031
using System.Xml;               // #01805 - rpoddar

using System.IO;                 //#03333
using System.Diagnostics;        //#03333
using Phoenix.Shared.BusFrame;   //#142715
using Phoenix.BusObj.Overnight;

//using Phoenix.BusObj.Admin.External; //#76458


namespace Phoenix.BusObj.Global
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class GbHelper : Phoenix.FrameWork.BusFrame.BusObjectBase
	{
		#region Private Fields

		#endregion Private fields

		#region private vars
		protected short invalidBalDef = -1;
		#endregion

		#region Constructor
		public GbHelper():
			base()
		{
			InitializeMap();
		}
		#endregion

		#region Initialize method
		protected override void InitializeMap()
		{
			#region Table Mapping
			this.DbObjects.Add( "GB_HELPER", "X_GB_HELPER");
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

		#region Public Methods

		#region IntGetStatusSort
		/// <summary>
		/// Return StatusSort value.  If null or string empty or bad value it returns -1
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		public short GetStatusSort(string status)
		{
			if (status == null || status.Trim() == string.Empty)
				return -1;
			status = status.Trim();
			if (status == GlobalVars.Instance.ML.Active			||	status == GlobalVars.Instance.ML.Dormant  ||
				status == GlobalVars.Instance.ML.Incomplete		||	status == GlobalVars.Instance.ML.Locked  ||
				status == GlobalVars.Instance.ML.Matured		||	status == GlobalVars.Instance.ML.NonAccrual  ||
				status == GlobalVars.Instance.ML.Pending		||	status == GlobalVars.Instance.ML.RenewPending  ||
				status == GlobalVars.Instance.ML.Restricted		||	status == GlobalVars.Instance.ML.Unadvanced  ||
				status == GlobalVars.Instance.ML.Unfunded		||	status == GlobalVars.Instance.ML.New  ||
				status == GlobalVars.Instance.ML.SDBRented		||	status == GlobalVars.Instance.ML.SDbReKey  ||
				status == GlobalVars.Instance.ML.SDBHold		||	status == GlobalVars.Instance.ML.SDBBankUse  ||
				status == GlobalVars.Instance.ML.SDBAvailable	||	status == GlobalVars.Instance.ML.ActiveChargeOff  ||
				status == GlobalVars.Instance.ML.Escheated )
			{
				return (short)Shared.Constants.UserConstants.STATUS_Active;
			}
			else if (status == GlobalVars.Instance.ML.Inactive	||	status == GlobalVars.Instance.ML.InActiveChargeOff)
			{
				return (short)Shared.Constants.UserConstants.STATUS_Inactive;
			}
			else if (status == GlobalVars.Instance.ML.Assumed	||	status == GlobalVars.Instance.ML.ChargedOff ||
				status == GlobalVars.Instance.ML.Closed			||	status == GlobalVars.Instance.ML.Deceased ||
				status == GlobalVars.Instance.ML.Repossessed	||	status == GlobalVars.Instance.ML.SDBClosed )
			{
				return (short)Shared.Constants.UserConstants.STATUS_Closed;
			}
			else
			{
				return -1;
			}
		}
		#endregion

		#region GetGLAccountStructure

		/// <summary>
		/// Implimentation of LocSelectStructure.  It gives format mask for gl accounts based on GL_ACCT_STRUCTURE
		/// Strip the ledger mask to get just GL mask
		/// </summary>
		/// <param name="glAcctFullMask(##-##-######)"></param>
		/// <param name="glLedgerLevel100Mask(######)"></param>
		public void GetGLAccountStructure(out string glAcctFullMask, out string glLedgerLevel100Mask, out int grpMember)
		{
			glAcctFullMask = string.Empty;
			glLedgerLevel100Mask = string.Empty;
			grpMember = 0;
			PString FullMask = new PString("FullMask");
			PString LedgerMask = new PString("LedgerMask");
			PInt	GrpMember = new PInt("GrpMember");
			DataService.Instance.ProcessCustomAction(this, "GetGLAcctStructure", FullMask, LedgerMask, GrpMember);
			if (!FullMask.IsNull)
				glAcctFullMask = FullMask.StringValue;
			if (!LedgerMask.IsNull)
				glLedgerLevel100Mask = LedgerMask.StringValue;
			if (!GrpMember.IsNull)
				grpMember = GrpMember.IntValue;
		}
		#endregion GetGLAccountStructure


        /* Begin #74019 */
        #region GetAccessViewInfo

        public string GetAccessView(int pnScreenID, string psModuleName)
        {
            PInt screenid = new PInt("A0", pnScreenID);
            PString modulename = new PString("A1", psModuleName);
            PString accessView = new PString("A2");

            DataService.Instance.ProcessCustomAction(this, "GetAccessView", screenid, modulename, accessView);

            return (accessView.IsNull?string.Empty:accessView.Value.ToString().Trim());
        }
        #endregion
        /* End #74019 */

		#region GetGLLedgerMask
		/// <summary>
		/// Get the ledger Mask from GL_ACCT_TOTAL_STRU table
		/// </summary>
		/// <param name="levelNo"></param>
		/// <returns></returns>
		public string GetGLLedgerMask(int levelNo)
		{
			if (levelNo == 0)
				return "";
			PInt LevelNo = new PInt("LevelNo");
			LevelNo.Value = levelNo;
			PString LedgerMask = new PString("LedgerMask");
			DataService.Instance.ProcessCustomAction(this, "GetGLLedgerMask", LevelNo, LedgerMask);
			if (LedgerMask.IsNull)
				return "";
			else
				return LedgerMask.StringValue;
		}
		#endregion GetGLLedgerMask

		#region IntOriginIdTranslate
		public string OriginIdTranslate(int originId)
		{
			string originDesc = CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Unknown");
			switch (originId)
			{
				case  0:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "System" );
					break;
				case  1:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Online" );
					break;
				case  2:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Batch" );
					break;
				case  3:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "POD" );
					break;
				case  4:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "ACH" );
					break;
				case  5:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "ATM" );
					break;
				case  6:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Auto Transfer" );
					break;
				case  7:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Loan Payoff" );
					break;
				case  8:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Loan Worksheet" );
					break;
				case  9:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Teller Item Capture" );
					break;
				case  10:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "HLD" );
					break;
				case  11:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Loan - Worksheet - Finance" );
					break;
				case  12:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "EXT" );
					break;
				case  13:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "FED" );
					break;
				case  14:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "FED" );
					break;
				case  15:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Acct Payable" );
					break;
				case  16:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Prepaid" );
					break;
				case  17:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Fixed Assets" );
					break;
				case  18:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Safe Deposit" );
					break;
				case  22:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "ADP" );
					break;
				case  47:
					originDesc =  CoreService.Translation.GetListItemX( Shared.Constants.ListId.OriginType, "VRU" );
					break;
				case  52:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "NET" );
					break;
				case  57:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Participation Worksheet" );
					break;
				case  58:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Generic - Service for XAPI" );
					break;
				case  59:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "FC Exchange Position Transactions" );
					break;
				case  60:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Treasury Gain and Loss Transactions" );
					break;
				case  61:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Transactions generated through interface" );
					break;
				case  62:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "EOM Accrual Transactions" );
					break;
				case  63:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Revaluation" );
					break;
				case  64:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "FC Close Out" );
					break;
				case  65:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "GL External Clearing" );
					break;
				case  66:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Online External Transfer" );
					break;
				case  72:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin,  "WIP Offset" );
					break;
				case  75:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "BACS" );
					break;
				case  76:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Currency Revaluation"	 );
					break;
				case  77:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Direct Debit" );
					break;
				case  78:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "ACH Company" );
					break;
                //Begin #24651
                case 85:
                    originDesc = CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "CSV");
                    break;
                //End #24651
                //Begin #79510
                case 87:
                    originDesc = CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Shared Branch");
                    break;
                //End #79510
                /*begin #118792*/
                case 100:
                    originDesc = CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Fusion Workflow");
                    break;
                /*End #118792*/
                default:
					originDesc =  CoreService.Translation.GetListItemX(Shared.Constants.ListId.NCOrigin, "Unknown" );
					break;
			}

			return originDesc;
		}
		#endregion IntOriginIdTranslate

		#region Get FKvalue description
		//Made a Copy of this from tlhelper method
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

		#region GetEmplGLRestrictions
		/// <summary>
		/// GL Account Restrictions
		/// </summary>
		/// <returns></returns>
		public void GetEmplGLRestrictions(out string glRestrictPrefixes, out string glRestrictLedgers)
		{
			GetEmplGLRestrictions(GlobalVars.EmployeeId, out glRestrictPrefixes, out glRestrictLedgers);
		}

		public void GetEmplGLRestrictions(int employeeId, out string glRestrictPrefixes, out string glRestrictLedgers)
		{
			PInt EmployeeId = new PInt("EmployeeId");
			EmployeeId.Value = employeeId;
			glRestrictPrefixes = string.Empty;
			glRestrictLedgers = string.Empty;
			PString RestrictPrefix = new PString("RestrictPrefix");
			DataService.Instance.ProcessCustomAction(this, "GetGLPrefixRestrictions", EmployeeId, RestrictPrefix);
			PString RestrictLedger = new PString("RestrictLedger");
			DataService.Instance.ProcessCustomAction(this, "GetGLPrefixRestrictions", EmployeeId, RestrictLedger);
			//Build the Array
			if (!RestrictPrefix.IsNull && RestrictPrefix.Value.Trim().Length > 0)
				glRestrictPrefixes = RestrictPrefix.Value;
				//glRestrictPrefixes = RestrictPrefix.Value.Split('^');
			if (!RestrictLedger.IsNull && RestrictLedger.Value.Trim().Length > 0)
				glRestrictLedgers = RestrictLedger.Value;
				//glRestrictLedgers = RestrictLedger.Value.Split('^');

		}

		#endregion GetEmplGLRestrictions

		//Begin #69248
		#region Class Desc
		public string GetClassDesc(	string psDepLoan,
									string psAcctType,
									int pnClassCode)
		{
			PString deploan = new PString("A0",psDepLoan);
			PString accttype = new PString("A1",psAcctType);
			PInt classcode = new PInt("A2",pnClassCode);
			PString classdesc = new PString("A3");

			DataService.Instance.ProcessCustomAction(this, "GetClassDesc", deploan, accttype, classcode, classdesc);

			return (classdesc.IsNull?string.Empty:classdesc.Value.ToString());
		}

        public string GetClassDesc(string psAcctType,
                                    int pnClassCode)
        {
            PString deploan = new PString("A0");
            PString accttype = new PString("A1", psAcctType);
            PInt classcode = new PInt("A2", pnClassCode);
            PString classdesc = new PString("A3");

            deploan.Value = GetDepLoan(psAcctType, "ACCTTYPE");

            DataService.Instance.ProcessCustomAction(this, "GetClassDesc", deploan, accttype, classcode, classdesc);

            return (classdesc.IsNull ? string.Empty : classdesc.Value.ToString());
        }

		#endregion

		#region Get Employee Name
		public string GetEmployeeName(int pnEmployeeID)
		{
			PInt EmplId = new PInt("A0",pnEmployeeID);
			PString EmplName = new PString("A1");

			DataService.Instance.ProcessCustomAction(this, "GetEmployeeName", EmplId, EmplName);

			return (EmplName.IsNull?string.Empty:EmplName.StringValue);
		}
		#endregion

		#region Get Bank Routing No
		public string GetBankRoutingNo(bool pbCleanRoutingNo)
		{
			PString bankRoutingNo = new PString("A0");

            if ( DataService.Instance != null)  //# Migration Changes - check for null
			    DataService.Instance.ProcessCustomAction(this, "GetBankRoutingNo", bankRoutingNo);

			return (bankRoutingNo.IsNull?string.Empty:(pbCleanRoutingNo?CleanRoutingNo(bankRoutingNo.StringValue):bankRoutingNo.StringValue));
		}
		#endregion

		#region Check if PMA enabled
		public string IsPMAEnabled()
		{
			PString pmaenabled = new PString("A0");
			DataService.Instance.ProcessCustomAction(this, "IsPMAEnabled", pmaenabled);

			return (pmaenabled.IsNull?string.Empty:pmaenabled.StringValue);
		}
		#endregion

		#region Get Dep Loan
		public string GetDepLoan(string psValue, string psInputType)
		{
			PString inputtype = new PString("A0",psInputType);
			PString valuestr = new PString("A1", psValue.Trim());
			PString deploan = new PString("A2");

			DataService.Instance.ProcessCustomAction(this, "GetDepLoan", inputtype, valuestr, deploan);

			return (deploan.IsNull?string.Empty:deploan.StringValue);
		}

		#endregion

		#region Split String into equal Parts
		public void SplitString(string psStr,
								int pnChunkLength,
								ref ArrayList psArrayList)

		{
			psStr = psStr.Trim();

			if (psStr.Length > 0)
			{
				int nTmpLen = 0;

				if (psStr.Length >= pnChunkLength)
					nTmpLen = pnChunkLength;
				else
					nTmpLen = psStr.Length;

				//sTmpArr[pnCntr] = psContactDetails.Substring(0, nTmpLen);
				psArrayList.Add(psStr.Substring(0, nTmpLen));
				psStr = psStr.Substring(nTmpLen, psStr.Length - nTmpLen);

				if (psStr.Trim().Length == 0)
					return;

				//call this fn recusively until we chop the string into 250 char chunks.
				SplitString(psStr, pnChunkLength, ref psArrayList);
			}

			return;
		}
		#endregion

        //Begin WI #21627
        #region Split String into equal Parts not using trim
        /// <summary>
        /// Split String into equal Parts without using trim
        /// </summary>
        /// <param name="psStr"></param>
        /// <param name="pnChunkLength"></param>
        /// <param name="psArrayList"></param>
        public void SplitStringNoTrim(string psStr,
                                int pnChunkLength,
                                ref ArrayList psArrayList)
        {
            //psStr = psStr.Trim();

            if (psStr.Length > 0)
            {
                int nTmpLen = 0;

                if (psStr.Length >= pnChunkLength)
                    nTmpLen = pnChunkLength;
                else
                    nTmpLen = psStr.Length;

                //avoiding end of line with a space
                if (psStr.Substring(0, nTmpLen).EndsWith(" "))
                {
                    for (int nIndex = nTmpLen; nIndex > 0; nIndex--)
                    {
                        if (psStr.Substring(0, nIndex).EndsWith(" ") == false)
                        {
                            nTmpLen = nIndex;
                            break;
                        }

                    }
                }
                psArrayList.Add(psStr.Substring(0, nTmpLen));

                psStr = psStr.Substring(nTmpLen, psStr.Length - nTmpLen);

                if (psStr.Trim().Length == 0)
                    return;

                //call this fn recusively until we chop the string into 250 char chunks.
                SplitStringNoTrim(psStr, pnChunkLength, ref psArrayList);
            }

            return;
        }
        #endregion
        //END WI #21627

		#region Get RIM No
		public int GetRimNo(string psAcctType, string psAcctNo)
		{
            //PString accttype = new PString("A0",psAcctType.Trim());   //7738
			//PString acctno = new PString("A1",psAcctNo.Trim());       //7738
            PString accttype = new PString("A0",StringHelper.StrTrimX(psAcctType));   //7738
            PString acctno = new PString("A1",StringHelper.StrTrimX(psAcctNo));       //7738
			PInt rimno = new PInt("A2");

			DataService.Instance.ProcessCustomAction(this, "GetRimNo", accttype, acctno, rimno);

			return (rimno.IsNull?int.MinValue:rimno.IntValue);
		}

        //Begin #130111
        public int GetRimNo(string psPanNo)
        {
            PString panNo = new PString("A0", StringHelper.StrTrimX(psPanNo));  
            PInt rimno = new PInt("A1");

            DataService.Instance.ProcessCustomAction(this, "GetRimNoByPan", panNo, rimno);

            return (rimno.IsNull ? int.MinValue : rimno.IntValue);
        }
        //End #130111
        #endregion

        #region Get Customer Details
        public void GetCustomerDetails(int pnRimNo, ref string rsPrimaryEmail)
		{
			//Add more params to the custom action as needed to get more values from rm_acct
			PInt RimNo = new PInt("A0", pnRimNo);
			PString PrimaryEmail = new PString("A0");

			DataService.Instance.ProcessCustomAction(this, "GetCustomerDetails", RimNo, PrimaryEmail);

			rsPrimaryEmail = (PrimaryEmail.IsNull?rsPrimaryEmail=string.Empty:rsPrimaryEmail=PrimaryEmail.StringValue);

			return;
		}

		#endregion

		#region Get Work Queue Catergory Details
		public void GetWQCategoryDetails(	int pnCategoryId,
											out int rnCatDefEmplId,
											out int rnCatDefQueueId,
											out string rsCatDesc,
											out string rsAutoEmailOwner)
		{
			//Add more params to the custom action as needed to get more values from rm_acct
			PInt CategoryId = new PInt("A0", pnCategoryId);
			PInt CatDefEmplId = new PInt("A1");
			PInt CatDefQueueId = new PInt("A2");
			PString CatDesc = new PString("A3");
			PString CatAutoEmailOwner = new PString("A4");

			DataService.Instance.ProcessCustomAction(this, "GetWQCategoryDetails",
																					CategoryId,
																					CatDefEmplId,
																					CatDefQueueId,
																					CatDesc,
																					CatAutoEmailOwner);

			rnCatDefEmplId = (CatDefEmplId.IsNull?0:CatDefEmplId.IntValue);
			rnCatDefQueueId = (CatDefQueueId.IsNull?0:CatDefQueueId.IntValue);
			rsCatDesc = (CatDesc.IsNull?string.Empty:CatDesc.StringValue);
			rsAutoEmailOwner = (CatAutoEmailOwner.IsNull?"N":CatAutoEmailOwner.StringValue);

			return;
		}

		#endregion

		#region GetWorkstationName
		/// <summary>
		/// Returns the current workstation name. Needed because MachineName won't work on citrix environements.
		/// </summary>
		/// <returns></returns>
		public string GetWorkstationName()	// #140791
		{
			string sWorkstationName;
			
			// first try to get the name from a PHX variable
			sWorkstationName = Environment.GetEnvironmentVariable("PHXCLIENTNAME");

			// second, try the citrix variable
			if (string.IsNullOrEmpty(sWorkstationName))
				sWorkstationName = Environment.GetEnvironmentVariable("CLIENTNAME");

			// last, if no environment variables are defined, use MachineName (ie, not using virtual machines)
			if (string.IsNullOrEmpty(sWorkstationName))
				sWorkstationName = Environment.MachineName;

			return sWorkstationName;
		}
		#endregion

		#region String Left - Moved to Phoenix.Shared.Utility StringHelper
		[Obsolete("Don't use this; use StringHelper.StrLeftX instead", false)]
		public string Left(string param, int length)
		{
			/*we start at 0 since we want to get the characters starting from the left
			 * and with the specified lenght and assign it to a variable*/
            //string result = param.Substring(0, length); //#73503
			//return the result of the operation
            string result = StringHelper.StrLeftX(param, length); //#73503
			return result;
		}

		#endregion

        #region String Right - Moved to Phoenix.Shared.Utility StringHelper
        [Obsolete("Don't use this; use StringHelper.RightX instead", false)]
		public string Right(string param, int length)
		{
			/*start at the index based on the lenght of the sting minus the specified
			length and assign it a variable*/
			//string result = param.Substring(param.Length - length, length); //#73503
			//return the result of the operation
            string result = StringHelper.StrRightX(param, length); //#73503
			return result;
		}

		#endregion

        #region String Mid - Moved to Phoenix.Shared.Utility StringHelper
        [Obsolete("Don't use this; use StringHelper.StrMidX instead", false)]
		public string Mid(string param,int startIndex, int length)
		{
			/*start at the specified index in the string ang get N number of characters
			 * depending on the lenght and assign it to a variable*/
            //string result = param.Substring(startIndex, length);  //#73503
			//return the result of the operation
            string result = StringHelper.StrMidX(param, startIndex, length); //#73503
			return result;
		}
        [Obsolete("Don't use this; use StringHelper.StrMidX instead", false)]
		public string Mid(string param,int startIndex)
		{
			/*start at the specified index and return all characters after it
			 * and assign it to a variable*/
            //string result = param.Substring(startIndex);  //#73503
			//return the result of the operation
            string result = StringHelper.StrMidX(param, startIndex); //#73503
			return result;
		}
		#endregion
		//End #69248

		#region CalculateDateX
        public DateTime CalculateDate(DateTime origDate, int term, string period)
        {
            return CalculateDate(null , origDate, term, period);
        }

		public DateTime CalculateDate(BusObjectBase busObject , DateTime origDate, int term, string period)
		{
			int day = 0;
			int month = 0;
			int year = 0;
			int dayIndex = 0;
			bool isSuccess = true;
            bool leapYear = false;  //145094
			// period = period.Trim();	// #176406 make null safe
			DateTime calcDate = DateTime.MinValue;

			try
			{
				// #176406 make this null safe
				//if (term == 0 || term.ToString() == "" || period == string.Empty || origDate == DateTime.MinValue || origDate.ToString() == string.Empty)
				if (term == 0 || term.ToString() == "" || string.IsNullOrWhiteSpace(period) || origDate == null || origDate == DateTime.MinValue ||  origDate == DbSmallDateTime.MinValue || origDate.ToString() == string.Empty)
					return DateTime.MinValue;

				period = period.Trim();	// #176406 move after if to make null safe

				if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "Day(s)"))
				{
					calcDate = origDate.Date.AddDays(term);
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "Week(s)"))
				{
					calcDate = origDate.Date.AddDays(term * 7);
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "Month(s)"))
				{
					day = origDate.Day;
					month = origDate.Month;
					year = origDate.Year;
					month = month + term;
					if (month > 12)
					{
						while (month > 12)
						{
							month = month - 12;
							year = year + 1;
						}
					}
					else if (month < 1)
					{
						while (month < 1)
						{
							month = month + 12;
							year = year - 1;
						}
					}
					//Force nDay to be a valid day for the new month
					if (month == 4 || month == 6 || month == 9 || month == 11)
					{
						if (day > 30)
							day = 30;
					}
					else if (month == 2)
					{
                        //#01295 - Begin

                        //if (year % 4 == 0)
                        //{
                        //    if (day > 29)
                        //        day = 29;
                        //    else if (day > 28)
                        //        day = 28;
                        //}

                        if (year % 4 == 0)
                        {
                            if (day > 29)
                                day = 29;
                        }
                        else
                        {
                            if (day > 28)
                                day = 28;
                        }
                        //#01295 - End
					}
					calcDate = new DateTime(year, month, day, 0, 0, 0);
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "Year(s)"))
				{
					day = origDate.Day;
					month = origDate.Month;
					year = origDate.Year;
					year = year + term;
                    //21630 Begin
                    if (month > 12)
                    {
                        while (month > 12)
                        {
                            month = month - 12;
                            year = year + 1;
                        }
                    }
                    else if (month < 1)
                    {
                        while (month < 1)
                        {
                            month = month + 12;
                            year = year - 1;
                        }
                    }
                    //Force nDay to be a valid day for the new month
                    if (month == 4 || month == 6 || month == 9 || month == 11)
                    {
                        if (day > 30)
                            day = 30;
                    }
                    else if (month == 2)
                    {
                        if (year % 4 == 0)
                        {
                            if (day > 29)
                                day = 29;
                        }
                        else
                        {
                            if (day > 28)
                                day = 28;
                        }
                    }
                    //21630 End
					calcDate = new DateTime(year, month, day, 0, 0, 0);
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "EOM(s)"))
				{
					while (term != 0)
					{
						//add one to the day in case we are already at EOM
						if (term > 0)
						{
							origDate = origDate.Date.AddDays(1);
							//remember what month the date is in
							month = origDate.Month;
							// keeping adding a day to the date until the month changes
							while (month == origDate.Month)
							{
								origDate = origDate.Date.AddDays(1);
							}
							// since we are now EOM + 1, take off one day
							origDate = origDate.Date.AddDays(-1);
						}
						else
						{
							origDate = new DateTime(origDate.Year,origDate.Month,1);
							origDate = origDate.Date.AddDays(-1);
						}

						//decrement/increment term
						if (term > 0)
							term = term - 1;
						else if (term < 0)
							term = term + 1;
					}
					calcDate = origDate;
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "DOM 29")
							|| period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "DOM 30"))
				{
					// #176406 re-write DOM 29 and DOM 30 
					int newDayOfMonth;
					if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "DOM 29"))
						newDayOfMonth = 29;
					else 
						newDayOfMonth = 30;

					// set date to term months from the first of this month
					calcDate = new DateTime(origDate.Year, origDate.Month, 1);

					// if the period date is past our DOM, new DOM is > days in this monthg, add term.
					// else add term - 1
					if (origDate.Day >= newDayOfMonth || DateTime.DaysInMonth(origDate.Year, origDate.Month) < newDayOfMonth)
						calcDate = calcDate.AddMonths(term);
					else
						calcDate = calcDate.AddMonths(term-1);

					// determine last day of month, and if > DOM 29 or DOM 30 set to 29 or 30... otherwise set to max day of month
					int lastDayOfMonth = DateTime.DaysInMonth(calcDate.Year, calcDate.Month);

					if (lastDayOfMonth < newDayOfMonth)
						newDayOfMonth = lastDayOfMonth;

					calcDate = new DateTime(calcDate.Year, calcDate.Month, newDayOfMonth);
					
					#region old code #176406
					////Begin #12098
					//day = 29;
					//month = origDate.Month;
					//year = origDate.Year;
					//month = month + term;
					//if (month > 12)
					//{
					//    while (month > 12)
					//    {
					//        month = month - 12;
					//        year = year + 1;
					//    }
					//}
					//else if (month < 1)
					//{
					//    while (month < 1)
					//    {
					//        month = month + 12;
					//        year = year - 1;
					//    }
					//}
					////Force nDay to be a valid day for the new month
					//if (month == 4 || month == 6 || month == 9 || month == 11)
					//{
					//    if (day > 30)
					//        day = 30;
					//}
					//else if (month == 2)
					//{
					//    //#01295 - Begin

					//    //if (year % 4 == 0)
					//    //{
					//    //    if (day > 29)
					//    //        day = 29;
					//    //    else if (day > 28)
					//    //        day = 28;
					//    //}

					//    if (year % 4 == 0)
					//    {
					//        if (day > 29)
					//            day = 29;
					//    }
					//    else
					//    {
					//        if (day > 28)
					//            day = 28;
					//    }
					//    //#01295 - End
					//}
					//calcDate = new DateTime(year, month, day, 0, 0, 0);
					#endregion
					#region previously commented
					//day = 29;
                    //month = origDate.Month;
                    //year = origDate.Year;
                    ////month = month + term;

                    //if (term > 0)
                    //{
                    //    if (origDate.Day > day || (origDate.Day == 28 && month == 2))
                    //        month = month + term;
                    //    else
                    //        month = month + term - 1;
                    //}
                    //else if (term < 0)
                    //{
                    //    //If we are past the 29th then use dont back too far (since pnTerm is negative)
                    //    if (origDate.Day  > day)
                    //        month = month + term + 1;
                    //    else
                    //        month = month + term;
                    //}

                    //if ( month > 12)
                    //{
                    //    while (month > 12)
                    //    {
                    //        month  = month - 12;
                    //        year = year + 1;
                    //    }
                    //}
                    //else if (month < 1)
                    //{
                    //    while (month < 1)
                    //    {
                    //        month = month + 12;
                    //        year = year - 1;
                    //    }

                    //}
                    //if (month == 2)	//it's feb we've got to do a little trick
                    //{
                    //    //						day = new DateTime(year, 3, 1).Date.AddDays(-1).Day;
                    //    calcDate = new DateTime(year, 3, 1);
                    //    calcDate = calcDate.Date.AddDays(-1);
                    //    day = calcDate.Day;
                    //}
                    //calcDate = new DateTime(year, month, day, 0, 0, 0);
                    //End #12098
					#endregion
				}
				#region old code #176406
				//else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "DOM 30"))
				//{
				//    //Begin #12098
				//    day = 30;
				//    month = origDate.Month;
				//    year = origDate.Year;
				//    month = month + term;
				//    if (month > 12)
				//    {
				//        while (month > 12)
				//        {
				//            month = month - 12;
				//            year = year + 1;
				//        }
				//    }
				//    else if (month < 1)
				//    {
				//        while (month < 1)
				//        {
				//            month = month + 12;
				//            year = year - 1;
				//        }
				//    }
				//    //Force nDay to be a valid day for the new month
				//    if (month == 4 || month == 6 || month == 9 || month == 11)
				//    {
				//        if (day > 30)
				//            day = 30;
				//    }
				//    else if (month == 2)
				//    {
				//        //#01295 - Begin

				//        //if (year % 4 == 0)
				//        //{
				//        //    if (day > 29)
				//        //        day = 29;
				//        //    else if (day > 28)
				//        //        day = 28;
				//        //}

				//        if (year % 4 == 0)
				//        {
				//            if (day > 29)
				//                day = 29;
				//        }
				//        else
				//        {
				//            if (day > 28)
				//                day = 28;
				//        }
				//        //#01295 - End
				//    }
				//    calcDate = new DateTime(year, month, day, 0, 0, 0);
				//    //day = 30;
				//    //month = origDate.Month;
				//    //year = origDate.Year;

				//    //if (term > 0)
				//    //{
				//    //    if (origDate.Day > day || (origDate.Day > 28 && month == 2))
				//    //        month = month + term;
				//    //    else
				//    //        month = month + term - 1;
				//    //}
				//    //else if (term < 0)
				//    //{
				//    //    if (origDate.Day > day)
				//    //        month = month + term - 1;
				//    //    else
				//    //        month = month + term;
				//    //}

				//    //if (month > 12)
				//    //{
				//    //    while (month > 12)
				//    //    {
				//    //        month = month - 12;
				//    //        year = year + 1;
				//    //    }
				//    //}
				//    //else if (month < 1)
				//    //{
				//    //    while (month < 1)
				//    //    {
				//    //        month = month + 12;
				//    //        year = year - 1;
				//    //    }
				//    //}

				//    //if (month == 2)
				//    //{
				//    //    //						day = new DateTime(year,3,1).Date.AddDays(-1).Day;
				//    //    calcDate = new DateTime(year, 3, 1);
				//    //    calcDate = calcDate.Date.AddDays(-1);
				//    //    day = calcDate.Day;
				//    //}
				//    //calcDate = new DateTime(year, month, day, 0, 0, 0);
				//    // End #12098
				//}
				#endregion
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "EOQ"))
				{
					//add three EOM(s) multiplied by the number of terms
					origDate = CalculateDate(origDate, term * 3, CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "EOM(s)"));

					while (!(origDate.Month == 3 && origDate.Day == 31) &&
						!(origDate.Month == 6 && origDate.Day == 30) &&
						!(origDate.Month == 9 && origDate.Day == 30) &&
						!(origDate.Month == 12 && origDate.Day == 31))
					{
						if (term > 0)
						{
							//keep looping around taking off the number of days in the month until the EOQ is reached
							origDate = origDate.Date.AddDays(-(origDate.Day));
						}
						else
						{
							//Go from one month to the next until the EOQ is reached
							origDate = CalculateDate(origDate, 1, CoreService.Translation.GetListItemX(ListId.PeriodsAndDOM, "EOM(s)"));
						}
					}
					calcDate = origDate;
				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsEOMAndBUSEOM, "BUS.-EOM"))
				{
					while (term != 0)
					{
						//add one to the day in case we are already at EOM
						if (term > 0)
						{
							origDate = origDate.Date.AddDays(1);
							//remember what month the date is in
							month = origDate.Month;

							while (month == origDate.Month)
							{
								origDate = origDate.Date.AddDays(1);
							}
						}
						else
						{
							origDate = new DateTime(origDate.Year, origDate.Month, 1, 0, 0, 0);
						}

						//decrement/increment term
						if (term > 0)
							term = term - 1;
						else if (term < 0)
							term = term + 1;
					}
					calcDate = origDate;
					//since we are now EOM + 1, find the last processing date

					#region custom action
					PDateTime	date			= new PDateTime("A0");
					PDateTime	calculatedDate	= new PDateTime("A1");
					this.CustomActionName = "CalculateDate";
					this.ActionType = XmActionType.Custom;
					date.Value = origDate;
					DataService.Instance.ProcessCustomAction(this, "CalculateDate", date, calculatedDate);

					if (calculatedDate.Value != DateTime.MinValue && !calculatedDate.IsNull)
						isSuccess = false;
					else
					{
						isSuccess = true;
						calcDate = calculatedDate.Value;
					}
					#endregion

				}
				else if (period == CoreService.Translation.GetListItemX(ListId.PeriodsAndDOMAndDOM15EOM, "15 & EOM"))
				{
					if (term > 0)
					{
						dayIndex = 1;
					}
					else
						dayIndex = -1;

					while (term != 0)
					{
                        origDate = origDate.Date.AddDays(dayIndex);  // 145094

                        // begin 145094

                        if (((origDate.Year % 4) == 0 && (origDate.Year % 100) != 0) || (origDate.Year % 400 == 0))
                            leapYear = true;
                        else
                            leapYear = false;

                        //end 145094

						if ((origDate.Day == 15) ||
                            ((origDate.Month == 2 && origDate.Day == 28 && !leapYear) || (origDate.Month == 2 && origDate.Day == 29 && leapYear)) || // 145094
							(origDate.Day == 31) ||
							(origDate.Day == 30 && (origDate.Month == 4 || origDate.Month == 6 || origDate.Month == 9 || origDate.Month == 11))) // 145094
						{
							if (term > 0)
								term = term - 1;
							else if (term < 0)
								term = term + 1;
						}
					}
					calcDate = origDate;
				}
				else
				{
                    #region #02258 - 1
                    if (busObject != null)
                    {
                        busObject.Messages.AddError(313505, null, new string[] { period });
                    }
                    #endregion

                    CoreService.ExceptionMgr.NewException(313505,313505, new string[] {period});


					isSuccess = false;
				}
			}
			catch(Exception e)
			{
				CoreService.ExceptionMgr.NewException(1, 0, e.InnerException, e.Message);
				isSuccess = false;
			}

			if (isSuccess == true)
				return calcDate;
			else
				return DateTime.MinValue;
		}
		#endregion

        #region CalculateDateAfterTodayX
        public DateTime CalculateDateAfterTodayX(DateTime lastPeriodDate, int term, string period)
        {
            DateTime nextPeriodDate = lastPeriodDate;
            DateTime oldDate = nextPeriodDate;
            //GlobalVars.SystemDate = new DateTime(2007, 4, 4); For Testing from PUnit Only
            try
            {
                if (term == 0 || term.ToString() == "" || period == string.Empty)
                    return DateTime.MinValue;
                while(GlobalVars.SystemDate >= nextPeriodDate)
                {
                    oldDate = nextPeriodDate;
                    nextPeriodDate = CalculateDate(nextPeriodDate, term, period);
                    if ((nextPeriodDate <= oldDate || nextPeriodDate == DateTime.MinValue) && GlobalVars.SystemDate >= nextPeriodDate)
                    {
                        CoreService.ExceptionMgr.NewException(311395, 311395, string.Empty);
                        //CalculateDateAfterTodayX - Failure to calculate a new date after the current date.
                        return DateTime.MinValue;
                    }
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
            return nextPeriodDate;
        }
        #endregion

        //#01295 Start
        #region CalculateDateIncTodayX
        public DateTime CalculateDateIncTodayX(DateTime lastPeriodDate, int term, string period)
        {
            DateTime nextPeriodDate = lastPeriodDate;
            DateTime oldDate = nextPeriodDate;
            //GlobalVars.SystemDate = new DateTime(2007, 4, 4); For Testing from PUnit Only
            try
            {
                if (term == 0 || term.ToString() == "" || period == string.Empty)
                    return DateTime.MinValue;
                while (GlobalVars.SystemDate > nextPeriodDate)
                {
                    oldDate = nextPeriodDate;
                    nextPeriodDate = CalculateDate(nextPeriodDate, term, period);
                    if ((nextPeriodDate <= oldDate || nextPeriodDate == DateTime.MinValue) && GlobalVars.SystemDate >= nextPeriodDate)
                    {
                        CoreService.ExceptionMgr.NewException(311395, 311395, string.Empty);
                        //CalculateDateAfterTodayX - Failure to calculate a new date after the current date.
                        return DateTime.MinValue;
                    }
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
            return nextPeriodDate;
        }
        #endregion
        //#01295 End
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
		public decimal GetBalance(short balanceDef, short gnDpAvailBalDef, decimal curBal,
			decimal holdBal, decimal floatBal1, decimal floatBal2,
			decimal memoPostedCredits, decimal memoPostedDebits, decimal memoFloat )
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
		public short DecodeBalDef( string definition )
		{
			short newDefinition = 0;
			if( definition == null )
				return newDefinition;
			//
			definition = definition.Trim();
			//
			if( definition == "Current")
				newDefinition = 0;
			else if(definition == "Current - Float 1")
				newDefinition = 2;
			else if(definition == "Current - Float 2")
				newDefinition = 3;
			else if(definition == "Current + Memo Posted")
				newDefinition = 6;
			else if(definition == "Current - Float 1 + Memo Posted")
				newDefinition = 8;
			else if(definition == "Current - Float 2 + Memo Posted")
				newDefinition = 9;

			else if(definition == "Undisbursed" || definition == string.Empty)
				newDefinition = 1; //#73503 - Changed from 0 to 1 as part of IntInitialize porting
			else if(definition == "Undisb + Memo Posted")
				newDefinition = 6;

			return newDefinition;

		}
		protected short GetAvailBalDef( BusObjectBase setUpObj, string postingMethod, string applType )
		{
			AdXpSvcs adXpSvcs = null;
			string depLoan = null;
			if ( setUpObj == null )
				return invalidBalDef;
			else if ( setUpObj is AdXpSvcs )
			{
				adXpSvcs = setUpObj as AdXpSvcs;
				adXpSvcs.PostingMethod.Value = postingMethod;
				if ( applType == "CD" || applType == "CK" || applType == "SV" )
					depLoan = GlobalVars.Instance.ML.DP;
				else if ( applType == "CMT")
					depLoan = GlobalVars.Instance.ML.CM;
				else
					depLoan = GlobalVars.Instance.ML.LN;

				return adXpSvcs.GetAvailBalDef( applType, depLoan );
			}
            else
            {
                FieldBase availBal = setUpObj.GetFieldByXmlTag("AvailBal");
                if (availBal != null && availBal.IsNull == false)
                    //return Convert.ToInt16(availBal.Value);
                    return DecodeBalDef(Convert.ToString(availBal.Value));  //#79156
            }
            //else if ( setUpObj is AdDpControl )
            //{
            //    return DecodeBalDef( ( setUpObj as AdDpControl).AvailBal.Value );
            //}
            ////else if ( setUpObj is AdLnControl )
            //if( setUpObj.TableName == "AD_LN_CONTROL" )
            //{
            //    return DecodeBalDef( ( setUpObj as AdLnControl).AvailBal.Value );
            //}

			return invalidBalDef;
		}

		public short GetDpLnCntrlAvailBalDef( string depLoan )
		{
			if ( depLoan == GlobalVars.Instance.ML.DP )
                return GetAvailBalDef(GlobalObjects.Instance[GlobalObjectNames.AdDpControl] as BusObjectBase, null, null);
				//return GetAvailBalDef( GlobalObjects.Instance[ GlobalObjectNames.AdDpControl ] as AdDpControl, null, null );
			else if ( depLoan == GlobalVars.Instance.ML.LN )
                return GetAvailBalDef( GlobalObjects.Instance[ GlobalObjectNames.AdLnControl ] as BusObjectBase, null, null );
				//return GetAvailBalDef( GlobalObjects.Instance[ GlobalObjectNames.AdLnControl ] as AdLnControl, null, null );
			else if ( depLoan == GlobalVars.Instance.ML.CM )
                return GetAvailBalDef(GlobalObjects.Instance[GlobalObjectNames.AdLnUmbControl] as BusObjectBase, null, null);
				//return GetAvailBalDef( GlobalObjects.Instance[ GlobalObjectNames.AdLnUmbControl ] as AdLnUmbControl, null, null );
			return invalidBalDef;
		}
		public short GetSvcsAvailBalDef( string postingMethod, string applType )
		{
			return GetAvailBalDef( GlobalObjects.Instance[ "AdXpSvcs" ] as AdXpSvcs, postingMethod, applType );
		}
		protected short GetColBalDef( BusObjectBase setUpObj )
		{
            if (setUpObj != null)
            {
                FieldBase collBal = setUpObj.GetFieldByXmlTag("CollBal");
                if (collBal != null)
                    return DecodeBalDef(collBal.StringValue);
                //if ( setUpObj is AdDpControl )
                //{
                //    return DecodeBalDef( ( setUpObj as AdDpControl).CollBal.Value );
                //}
                //else if ( setUpObj is AdLnControl )
                //{
                //    return DecodeBalDef( ( setUpObj as AdLnControl).CollBal.Value );
                //}
            }
			return invalidBalDef;
		}
		public short GetColBalDef( string depLoan )
		{
			if ( depLoan == GlobalVars.Instance.ML.DP )
				return GetColBalDef( GlobalObjects.Instance[ GlobalObjectNames.AdDpControl ] as BusObjectBase);
			else if ( depLoan == GlobalVars.Instance.ML.LN )
                return GetColBalDef(GlobalObjects.Instance[GlobalObjectNames.AdLnControl] as BusObjectBase);
			else if ( depLoan == GlobalVars.Instance.ML.CM )
                return GetColBalDef(GlobalObjects.Instance[GlobalObjectNames.AdLnUmbControl] as BusObjectBase);
			return invalidBalDef;
		}
		#endregion

		#region Concat Customer Name
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
			if ( firstName != null && firstName != String.Empty )
				firstName = firstName.Trim();
			if ( lastName != null && lastName != String.Empty )
				lastName = lastName.Trim();
			if ( middleInitial != null && middleInitial != String.Empty )
				middleInitial = middleInitial.Trim();
			#endregion

			string rsName = "";

			if (lastNameFirst == true)
			{
				if( lastName != null && lastName != string.Empty)
					rsName = lastName;
				if( firstName != null && firstName != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = firstName;
					else
						rsName = rsName + ", " + firstName;
				}
				if( middleInitial != null && middleInitial != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = middleInitial;
					else
						rsName = rsName + " " + middleInitial;
				}
			}
			else
			{
				if( firstName != null && firstName != string.Empty)
					rsName = firstName;
				if( middleInitial != null && middleInitial != string.Empty)
				{
					if(rsName == string.Empty)
						rsName = middleInitial;
					else
						rsName = rsName + " " + middleInitial;
				}
				if( lastName != null && lastName != string.Empty)
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
			if ( suffix != null && suffix != String.Empty )
				suffix = suffix.Trim();
			if ( title != null && title != String.Empty )
				title = title.Trim();
			#endregion

			string rsName = "";

			rsName = ConcateNameX(firstName,lastName,middleInitial,lastNameFirst);

			// Add suffix
			if(suffix != null && suffix != string.Empty)
				rsName = rsName + " " + suffix;

			// Add title
			if( title!= null && title != string.Empty)
				rsName = title + " " + rsName;

			return rsName;
		}

		#endregion

		#region GetSPMessageText
		public string GetSPMessageText( int errorId, bool bAppendNo )
		{
			string errorText = String.Empty;
			//Added 71170 - For Postive Numbers
			if (errorId > 0)
			{
				errorText = GetPhoenixXMMessageText(errorId, bAppendNo );
				if (errorText != string.Empty)
					return errorText;
			}
			//PcOvError PcOvError = new PcOvError();
            BusObjectBase pcOvError = BusObjHelper.MakeClientObject("PC_OV_ERROR");
            FieldBase fieldErrorId = pcOvError.GetFieldByXmlTag("ErrorId");
            fieldErrorId.Value = errorId;
            //
            //pcOvError.ErrorId.Value = errorId;
            pcOvError.ActionType = XmActionType.Select;
            DataService.Instance.ProcessRequest(XmActionType.Select, pcOvError);//WI #33898 -Put "XmActionType.Select" as the parameter in the method ProcessRequest as pcOvError is not properly selecting.
            //
            errorText = BusObjHelper.GetFieldValue( pcOvError, "ErrorText");
            //
            if ( string.IsNullOrEmpty( errorText)) // || PcOvError.ErrorText.Value == String.Empty)
                errorText = CoreService.Translation.GetUserMessageX(360087);
            //else
            //    errorText = PcOvError.ErrorText.Value;
            //if ( PcOvError.ErrorText.IsNull || PcOvError.ErrorText.Value == String.Empty )
            //    errorText = CoreService.Translation.GetUserMessageX(360087);
            //else
            //    errorText = PcOvError.ErrorText.Value;
			if ( bAppendNo )
				return errorId.ToString() + ":" + errorText;
			else
				return errorText;

		}
		public string GetSPMessageText( int errorId )
		{
			return GetSPMessageText( errorId, true );
		}

		#region GetPhoenixXMMessageText
		//Until 80 we have this method
		public string GetPhoenixXMMessageText(int errorId, bool bAppendNo )
		{
			string errorText = String.Empty;
			if (errorId == 1002)
				errorText = "Employee information Needed";
			else if (errorId == 1003)
				errorText = "User Information Needed";
			else if (errorId == 1006)
				errorText = "Override is Not Allowed";
			else if (errorId == 1009)
				errorText = "Invalid Account Currency ISO";
			else if (errorId == 1010)
				errorText = CoreService.Translation.GetUserMessageX(360607);//"Invalid account";
			else if (errorId == 1011)
				errorText = CoreService.Translation.GetUserMessageX(360608);//"Invalid account status";
			else if (errorId == 1015)
				errorText = "Transaction code is not supported from XAPI";
			else if (errorId == 1016)
				errorText = "Invalid User Information";
			else if (errorId == 1017)
				errorText = "Invalid Password";
			else if (errorId == 1019)
				errorText = "User Account Locked due to failed logins";
			else if (errorId == 1020)
				errorText = "User Must change the password since this is the first time logon";
			else if (errorId == 1021)
				errorText = "Account Locked by Bank Employee";
			else if (errorId == 1023)
				errorText = "Amount Information is not provided";
			else if (errorId == 1025)
				errorText = "User doesn't have access to the account";
			else if (errorId == 1028)
				errorText = "Transaction already processed";
			else if (errorId == 1029)
				errorText = "The original transaction was already reversed";
			else if (errorId == 1030)
				errorText = "Original transaction is in process of extraction and is not available for reversal";
			else if (errorId == 1031)
				errorText = "Exchange rates cannot be modified during partial/full reversals";
			else if (errorId == 1032)
				errorText = "Partial reversal cannot be applied on multi component transaction set";
			else if (errorId == 1033)
				errorText = "Invalid Employee";
			else if (errorId == 1034)
				errorText = "Employee does not has security permissions for the trancode";
			else if (errorId == 1035)
				errorText = "Invalid RIM";
			else if (errorId == 1036)
				errorText = "Invalid Transfer RIM";
			else if (errorId == 1037)
				errorText = "Database status has changed";
			else if (errorId == 1038)
				errorText = "Transaction cannot be partially reposted since the transaction was done in the past";
			else if (errorId == 1039)
				errorText = "Unable to find pre-exchanged transaction";
			else if (errorId == 1040)
				errorText = "Rate Expired";
			else if (errorId == 1041)
				errorText = "Monetary Transaction has been changed";
			else if (errorId == 1043)
				errorText = "Password Expired";
			else if (errorId == 1044)
				errorText = "Rate Change is Not Allowed";
			else if (errorId == 1045)
				errorText = "From and To accounts cannot be same for transfers";
			else if (errorId == 1046)
				errorText = "Transaction code is not supported in XAPI Real-time mode";
			else if (errorId == 1047)
				errorText = "Transaction code is not supported in XAPI Memo mode";
			else if (errorId == 1048)
				errorText = "Float currency is not matching with account currency ID";
			else if (errorId == 1049)
				errorText = "No accounts are setup for Direct Debit Authorization";
			else if (errorId == 1050)
				errorText = "Transaction has overrides";
			else if (errorId == 1051)
				errorText = "Invalid User status";
			else if (errorId == 1052)
				errorText = "Database status has changed. Please retry the transactions";
			else if (errorId == 1053)
				errorText = "Transaction mode not available currently";
			else if (errorId == 1054)
				errorText = "One of the Amount/Rate/Check No parameter is negative";
			else if (errorId == 1055)
				errorText = "The query did not produce any results";
			else if (errorId == 1056)
				errorText = "Transaction cannot be reversed since the transaction was done in the past";
			else if (errorId == 1057)
				errorText = "Sum of Float Entries Exceeds the Transaction amounts";
			else if (errorId == 1058)
				errorText = "Employee does not has access to the service";
			else if (errorId == 1059)
				errorText = "Invalid ApplType and Trancode Combination";
			else if (errorId == 1060)
				errorText = "Insufficient Data";
			else if (errorId == 1061)
				errorText = "Original transaction could not be located";
			else if (errorId == 1062)
				errorText = "Invalid TIN";
			else if (errorId == 1063)
				errorText = "Invalid Receipt ID";
			else if (errorId == 1064)
				errorText = "Invalid origin branch #";
			else if (errorId == 1065)
				errorText = "Transaction available only during day mode";
			else if (errorId == 1066)
				errorText = "Primary database unavailable to process the transaction.";
			else if (errorId == 1067)
				errorText = "Employee cannot access the customer";
			else if (errorId == 1068)
				errorText = "Employee does not has sufficient limits";
			else if (errorId == 1069)
				errorText = "Transaction amount exceeds the per transaction limit";
			else if (errorId == 1070)
				errorText = "Transaction will cause the daily limits to be exceeded";
			else if (errorId == 1071)
				errorText = "Account has reached per period deposit limit";
			else if (errorId == 1072)
				errorText = "Minimum Deposit Limit Not Met";
			else if (errorId == 1073)
				errorText = "Account Has Holds in Place";
			else if (errorId == 1074)
				errorText = "Account Has Maturity Check Pending";
			else if (errorId == 1075)
				errorText = "Account is an IRA Account";
			else if (errorId == 1076)
				errorText = "Account has reached per period withdrawal limit";
			else if (errorId == 1077)
				errorText = "Account has reached per period check limit";
			else if (errorId == 1078)
				errorText = "Account not part of retirement plan";
			else if (errorId == 1079)
				errorText = "The account has invalid IRA status";
			else if (errorId == 1080)
				errorText = "Transaction Amount less than the minimum withdrawal amount";
			else if (errorId == 1081)
				errorText = "Transaction Amount does not match the issue amount";
			else if (errorId == 1082)
				errorText = "Account does not has sufficient funds";
			else if (errorId == 1083)
				errorText = "Transaction amount exceeds analysis fees";
			else if (errorId == 1084)
				errorText = "Check is a suspect item";
			else if (errorId == 1085)
				errorText = "Check has a stop payment";
			else if (errorId == 1086)
				errorText = "This check already has an active restriction";
			else if (errorId == 1087)
				errorText = "Check Number exceeds the highest check number issued";
			else if (errorId == 1088)
				errorText = "Recon status could not be verified";
			else if (errorId == 1089)
				errorText = "Invalid recon status";
			else if (errorId == 1091)
				errorText = "Invalid Advance status";
			else if (errorId == 1092)
				errorText = "Account has unique interest advance conditions";
			else if (errorId == 1093)
				errorText = "Account has reached maximum disbursement date";
			else if (errorId == 1094)
				errorText = "Loan has already been advanced";
			else if (errorId == 1095)
				errorText = "Loan has not been advanced";
			else if (errorId == 1096)
				errorText = "Invalid effective date";
			else if (errorId == 1097)
				errorText = "Transaction Amount exceeds undisbursed amount";
			else if (errorId == 1098)
				errorText = "Transaction Amount less than minimum advance amount";
			else if (errorId == 1099)
				errorText = "Transaction Amount less than undisbursed amount";
			else if (errorId == 1100)
				errorText = "Transaction Amount exceeds payoff amount";
			else if (errorId == 1101)
				errorText = "Invalid Interest Type";
			else if (errorId == 1102)
				errorText = "Loan is a participation";
			else if (errorId == 1103)
				errorText = "Transaction Amount exceeds total late fees due";
			else if (errorId == 1104)
				errorText = "Debit transactions not allowed on the loan account";
			else if (errorId == 1105)
				errorText = "Credit transactions not allowed on the loan account";
			//
			//There may be some positibe numbers in ov_errors!?
			if (errorText != string.Empty)
			{
				if ( bAppendNo)
					return errorId.ToString() + ":" + errorText;
				else
					return errorText;
			}
			else
				return errorText;
		}
		public string GetPhoenixXMMessageText( int errorId )
		{
			return GetPhoenixXMMessageText( errorId, true );
		}
		#endregion GetPhoenixXMMessageText
		//
		#endregion

		#region FormatAnyNumber
		/// <summary>
		/// Format the Any number based on format Mask.  The expected mask consists of 9s only
		/// </summary>
		/// <param name="unformattedNo"></param>
		/// <param name="numberFormat"></param>
		/// <returns></returns>

		public string FormatAnyNumber(string unformattedNo, string numberFormat)
		{
			if( unformattedNo == null )
				return unformattedNo;

			StringBuilder formattedNumber = new StringBuilder(numberFormat);
			int inputIndex = unformattedNo.Length -1;
			int fmtIndex = numberFormat.Length - 1;
			//int newLocation = 60;

			for( ; fmtIndex > -1;  fmtIndex--)
			{
				char fmtMask = numberFormat[ fmtIndex ];
				//newLocation--;
				if( fmtMask == '9' )
				{
					bool foundValue = false;
					for( ; inputIndex > -1 && foundValue == false ; inputIndex--)
					{
						char acctValue = unformattedNo[ inputIndex ];
						if( char.IsNumber( acctValue ))
						{
							foundValue = true;
							formattedNumber[fmtIndex] = acctValue;
						}
					}
					if( !foundValue )
						formattedNumber[fmtIndex] = '0';
				}
			}
			return formattedNumber.ToString();

		}
		//
		#endregion
        //wi 30479
        #region FormatCustomMask
        /// <summary>
        /// Format the string based on format Mask.  This was written mainly to take of formatting custom Tin formats
        /// </summary>
        /// <param name="stringValue"></param>
        /// <param name="stringFormat"></param>
        /// <returns></returns>

        public string FormatCustomMask(string stringValue, string stringFormat)
        {
            if (stringValue == null)
            {
                return null;
            }
            if (stringValue == string.Empty)
            {
                return string.Empty;
            }
            if (stringFormat == string.Empty || stringFormat == null)
            {
                return stringValue;
            }

            int index = 0, formattedIndex = 0;
            StringBuilder formattedString = new StringBuilder(stringFormat);
            bool invalidChar = false;

            while (formattedIndex < stringFormat.Length && index < stringValue.Length)
            {
                if (stringFormat[formattedIndex] == '9' ||
                    stringFormat[formattedIndex] == 'a' ||
                    stringFormat[formattedIndex] == 'A' ||
                    stringFormat[formattedIndex] == '-' ||
                    stringFormat[formattedIndex] == 'n' ||
                    stringFormat[formattedIndex] == 'N' ||
                    stringFormat[formattedIndex] == 'X' ||
                    stringFormat[formattedIndex] == '!')
                {
                    switch (stringFormat[formattedIndex])
                    {
                        case '9':
                            if (char.IsDigit(stringValue[index]))
                            {
                                formattedString[formattedIndex] = stringValue[index];
                                formattedIndex++;
                            }
                            else
                                invalidChar = true;
                            break;
                        case 'a':
                            if (char.IsLetter(stringValue[index]))
                            {
                                formattedString[formattedIndex] = stringValue[index];
                                formattedIndex++;
                            }
                            else
                                invalidChar = true;
                            break;
                        case 'A':
                            if (char.IsLetter(stringValue[index]))
                            {
                                formattedString[formattedIndex] = char.ToUpperInvariant(stringValue[index]);
                                formattedIndex++;
                            }
                            else
                                invalidChar = true;
                            break;
                        case 'n':
                        case 'N':
                            if (Char.IsLetter(stringValue[index]) || Char.IsDigit(stringValue[index]))
                            {
                                if (stringFormat[formattedIndex] == 'N')
                                    formattedString[formattedIndex] = char.ToUpperInvariant(stringValue[index]);
                                else
                                    formattedString[formattedIndex] = stringValue[index];
                                formattedIndex++;
                            }
                            else
                                invalidChar = true;
                            break;
                        case 'X':
                        case '!':
                            if (!Char.IsControl(stringValue[index]))
                            {
                                if (stringFormat[formattedIndex] == '!' || Char.IsLetter(stringValue[index]))
                                    formattedString[formattedIndex] = char.ToUpperInvariant(stringValue[index]);
                                else
                                    formattedString[formattedIndex] = stringValue[index];
                            }
                            else
                                invalidChar = true;
                            formattedIndex++;
                            break;
                        case '-':
                            {
                                if (stringFormat[formattedIndex] != stringValue[index])
                                    index--;      // Stay as same index for data
                                formattedIndex++;
                            }
                            break;
                        default:
                            break;
                    }  // End of "switch (stringFormat[formattedIndex])"
                } // End of "if (stringFormat[formattedIndex] == '9' or 'A' or '-' or '/'"
                index++;
            }  // End of "while (formattedIndex < stringFormat.Length && index < stringValue.Length)"

            formattedIndex = (formattedIndex < stringFormat.Length
                && stringFormat[formattedIndex] == '-') ? formattedIndex + 1 : formattedIndex;

            if (invalidChar)
                return string.Empty;
            else
                return formattedString.ToString().Substring(0, formattedIndex);

        }
        //
        #endregion
        //end 30479 
		#region FormatAddress
		/// <summary>
		/// This function take address fields as input and returns either a one line or a  multiline formatted address string
		/// </summary>
		/// <param name="formatType">FormatType 1 - Multiline address format for Multiline field 0 - Single line either format for columns , datafields More format type can be used if desired</param>
		/// <param name="international"> Y - International address, N - US Address </param>
		/// <param name="addressLine1">Address Line 1</param>
		/// <param name="addressLine2">Address Line 1</param>
		/// <param name="addressLine3">Address Line 1</param>
		/// <param name="city"></param>
		/// <param name="state"></param>
		/// <param name="zip"></param>
		/// <param name="countryDesc"></param>
		/// <param name="countryCode"></param>
		/// <returns></returns>
		public string FormatAddress(int formatType, string international, string addressLine1, string addressLine2,
			                        string addressLine3, string city, string state, string zip, string countryDesc,
			                        string countryCode)
		{
			StringBuilder newLine = new StringBuilder();
			StringBuilder newLineCountry = new StringBuilder();
			StringBuilder country = new StringBuilder();
			StringBuilder formattedAddress = new StringBuilder();


			if (international == null)
				international = "";

			if (addressLine1 == null)
				addressLine1 = "";

			if (addressLine2 == null)
				addressLine2 = "";

			if (addressLine3 == null)
				addressLine3 = "";

			if (city == null)
				city = "";

			if (state == null)
				state = "";

			if (zip == null)
				zip = "";

			if (countryDesc == null)
				countryDesc = "";

			if (countryCode == null)
				countryCode = "";

			if (formatType > 0)
			{
				newLine.Append("\\~");
				newLineCountry.Append("\\~");
			}
			else
			{
				newLine.Append(", ");
				newLineCountry.Append("  ");
			}
			if (countryDesc.Trim().Length > 0)
				country.Append(countryDesc);
			else
				country.Append(countryCode);
			formattedAddress.Append(addressLine1.Trim());
			if (addressLine2.Trim().Length > 0)
				formattedAddress.Append(newLine + addressLine2);
			if (addressLine3.Trim().Length > 0)
				formattedAddress.Append(newLine + addressLine3);
			if (international == "Y")
			{
				if (country.Length > 0)
				{
                    //119436 - Commenting out the logic as part of the fix for duplication of International address[119014].
                    ////begin #27972
                    //               if (country.ToString() == "Canada")
                    //               {
                    //                   formattedAddress.Append(newLine + city + ", " + state + "    " + zip + newLineCountry + country);
                    //               }
                    //               else
                    //               {
                    //                   formattedAddress.Append(newLineCountry + country.ToString());
                    //               }
                    //               //end #27972
                    formattedAddress.Append(newLineCountry + country.ToString());
                }
			}
			else
			{
				if (country.Length > 0)
					formattedAddress.Append(newLine + city + ", " + state +  "    " + zip +  newLineCountry + country);
				else
					formattedAddress.Append(newLine + city + ", " + state +  "    " + zip);
			}
			return formattedAddress.ToString();
		}
		#endregion FormatAddress

        /*Begin #76036*/
        #region Get Current RIM Address
        public int GetCurrRmAddrId(int pnRimNo, DateTime pdt)
        {
            PInt RimNo = new PInt("A0", pnRimNo);
            PDateTime EffDt = new PDateTime("A1", pdt);
            PInt AddrId = new PInt("A2");

            DataService.Instance.ProcessCustomAction(this, "GetCurrRmAddrId", RimNo, EffDt, AddrId);

            return AddrId.Value;
        }
        #endregion
        /*End #76036*/
		#region FormatTIN
		/// <summary>
		/// Formats a SSN/TIN according to psDisplayLastFour ( AD_GB_RSM.DISPLAY_LAST_FOUR).
		/// NB: Assumes the value has been formatted with dashes and other input mask characters.
		/// </summary>
		/// <param name="formattedTIN"></param>
		/// <returns></returns>
		public string FormatTIN(string formattedTIN)
        {  //76031

			//2010-Convert-moved here
            AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

            //Begin #I9961
            if (_adGbRsmBusObj == null)
            {
                _adGbRsmBusObj = new AdGbRsm();
                _adGbRsmBusObj.EmployeeId.Value = BusGlobalVars.EmployeeId;
                _adGbRsmBusObj.ActionType = XmActionType.Select;
            }
            //End #I9961

            return FormatTIN( formattedTIN, _adGbRsmBusObj.DisplayLastFour.Value);
		}
        public string FormatTIN(string formattedTIN, string displayLast4)
        {  //76031
            formattedTIN = formattedTIN.Trim();
            string sReturn = string.Empty;

            if ( displayLast4 == "Y" && formattedTIN.Length > 4)
            {
                sReturn = formattedTIN.Substring(0, formattedTIN.Length - 4);
                sReturn = Regex.Replace(sReturn, "[0-9]", "X") + formattedTIN.Substring(formattedTIN.Length - 4, 4);
            }
            else
                sReturn = formattedTIN;
            return sReturn;
        }
		#endregion FormatTIN

		#region Compute Customer Age (Centura fn name - IntDateDifference)
		public string DateDifference(DateTime startDate, DateTime endDate)
		{
            return CalcDateDiff(startDate, endDate, false);
		}
        /*Begin #76099*/
        /// <summary>
        /// This Function will call datediff function in the database to compute the days, months and years diff between date1 and date2
        /// </summary>
        /// <param name="startDate">Date1</param>
        /// <param name="endDate">Date2</param>
        /// <param name="rnDays">ref int daysdiff</param>
        /// <param name="rnMonths">ref int monthsdiff</param>
        /// <param name="rnYears">ref int yearsdiff</param>
        public void DBDateDifference(DateTime startDate, DateTime endDate, ref int rnDays, ref int rnMonths, ref int rnYears)
        {
            PDateTime dtStartDt = new PDateTime("A0", startDate);
            PDateTime dtEndDt = new PDateTime("A1", endDate);
            PInt nDays = new PInt("A2");
            PInt nMonths = new PInt("A3");
            PInt nYears = new PInt("A4");

            DataService.Instance.ProcessCustomAction(this, "GetDBDateDiff", dtStartDt, dtEndDt, nDays, nMonths, nYears);

            rnDays = (nDays.IsNull ? 0 : nDays.Value);
            rnMonths = (nMonths.IsNull ? 0 : nMonths.Value);
            rnYears = (nYears.IsNull ? 0 : nYears.Value);

            return;
        }
        /*End #76099*/

        /*Begin #76036*/
        public string DateDifference(DateTime startDate, DateTime endDate, bool useABSValue)
        {
            return CalcDateDiff(startDate, endDate, useABSValue);
        }

        private string CalcDateDiff(DateTime startDate, DateTime endDate, bool useABSValue)
        {
            int nYearEndDate = 0;
            int nYearStartDate = 0;
            int nMonthEndDate = 0;
            int nMonthStartDate = 0;
            int nMonths = 0;
            int nYears = 0;
            string sLength = string.Empty;

            if (startDate.ToString().Trim() != "" && endDate.ToString().Trim() != "")
            {
                TimeSpan span = endDate.Subtract(startDate);

                if ( (Convert.ToInt32(span.TotalDays) < 0) && !useABSValue )
                {
                    sLength = "Invalid Dates";
                }
                else
                {
                    nYearEndDate = endDate.Year;
                    nYearStartDate = startDate.Year;
                    nMonthEndDate = endDate.Month;
                    nMonthStartDate = startDate.Month;

                    nYears = nYearEndDate - nYearStartDate;
                    nMonths = nMonthEndDate - nMonthStartDate;

                    if (nMonths < 0)
                    {
                        nMonths = 12 + nMonths;
                        nYears = nYears - 1;
                    }

                    if (nYears > 0)
                    {
                        if (nYears == 1)
                            sLength = "1 Year,  ";
                        else
                            sLength = nYears.ToString() + " Years,";
                    }

                    if (nMonths == 1)
                        sLength = sLength + " " + "1 Month";
                    else
                        sLength = sLength + " " + nMonths.ToString() + " Months";
                }
            }

            return sLength;
        }
        /*End #76036*/
		#endregion

		#region Decode Origin ID (IntOriginIdTranslate)
		public string DecodeOriginID(int pnOriginID)
		{
			if (pnOriginID < 0)
				return string.Empty;

			switch (pnOriginID)
			{
				case 0:
					return CoreService.Translation.GetListItemX(638, "System");
				case 1:
					return CoreService.Translation.GetListItemX(638, "Online");
				case 2:
					return CoreService.Translation.GetListItemX(638, "Batch");
				case 3:
					return CoreService.Translation.GetListItemX(638, "POD");
				case 4:
					return CoreService.Translation.GetListItemX(638, "ACH");
				case 5:
					return CoreService.Translation.GetListItemX(638, "ATM");
				case 6:
					return CoreService.Translation.GetListItemX(638, "Auto Transfer");
				case 7:
					return CoreService.Translation.GetListItemX(638, "Loan Payoff");
				case 8:
					return CoreService.Translation.GetListItemX(638, "Loan Worksheet");
				case 9:
					return CoreService.Translation.GetListItemX(638, "Teller Item Capture");
				case 10:
					return CoreService.Translation.GetListItemX(638, "HLD");
				case 11:
					return CoreService.Translation.GetListItemX(638, "Loan - Worksheet - Finance");
				case 12:
					return CoreService.Translation.GetListItemX(638, "EXT");
				case 13:
					return CoreService.Translation.GetListItemX(638, "FED");
				case 14:
					return CoreService.Translation.GetListItemX(638, "FED");
				case 15:
					return CoreService.Translation.GetListItemX(638, "Acct Payable");
				case 16:
					return CoreService.Translation.GetListItemX(638, "Prepaid");
				case 17:
					return CoreService.Translation.GetListItemX(638, "Fixed Assets");
				case 18:
					return CoreService.Translation.GetListItemX(638, "Safe Deposit");
				case 22:
					return CoreService.Translation.GetListItemX(638, "ADP");
				case 47:
					return CoreService.Translation.GetListItemX(638, "VRU");
				case 52:
					return CoreService.Translation.GetListItemX(638, "NET");
				case 57:
					return CoreService.Translation.GetListItemX(638, "Participation Worksheet");
				case 58:
					return CoreService.Translation.GetListItemX(638, "Generic - Service for XAPI");
				case 59:
					return CoreService.Translation.GetListItemX(638, "FC Exchange Position Transactions");
				case 60:
					return CoreService.Translation.GetListItemX(638, "Treasury Gain and Loss Transactions");
				case 61:
					return CoreService.Translation.GetListItemX(638, "Transactions generated through interface");
				case 62:
					return CoreService.Translation.GetListItemX(638, "EOM Accrual Transactions");
				case 63:
					return CoreService.Translation.GetListItemX(638, "Revaluation");
				case 64:
					return CoreService.Translation.GetListItemX(638, "FC Close Out");
				case 65:
					return CoreService.Translation.GetListItemX(638, "GL External Clearing");
				case 66:
					return CoreService.Translation.GetListItemX(638, "Online External Transfer");
				case 72:
					return CoreService.Translation.GetListItemX(638, "WIP Offset");
				case 75:
					return CoreService.Translation.GetListItemX(638, "BACS");
				case 76:
					return CoreService.Translation.GetListItemX(638, "Currency Revaluation");
				case 77:
					return CoreService.Translation.GetListItemX(638, "Direct Debit");
				case 78:
					return CoreService.Translation.GetListItemX(638, "ACH Company");
                case 81:    //#2106
                    return CoreService.Translation.GetListItemX(638, "CPI - Insurance File Load");
                case 82:    //#76031
                    return CoreService.Translation.GetListItemX(638, "Paper Payroll");
                case 85:    //79442
                    return CoreService.Translation.GetListItemX(638, "CSV");
                case 87:    // #79510
                    return CoreService.Translation.GetListItemX(638, "Shared Branch");
                case 92:    // #79644
                    return CoreService.Translation.GetListItemX(638, "ACH Company Charge Back");
                case 100:   // #47187
                    return CoreService.Translation.GetListItemX(638, "Fusion Workflow");
                default:
					return CoreService.Translation.GetListItemX(638, "Unknown");
			}
		}
		#endregion

		public bool IsWorkflowInstalled()	// WI#23705/23706
		{
			PBoolean bWorkflowInstalled = new PBoolean("WorkflowInstalled", false);
			CoreService.DataService.ProcessCustomAction(this, "IsWorkflowInstalled", bWorkflowInstalled);

			return bWorkflowInstalled.BooleanValue;
		}

		#region acct type details
		public AcctTypeDetail GetAcctTypeDetails( string acctType, string applType )
		{
			ArrayList acctTypeDetails;

			if ( ( acctType == null || acctType == String.Empty ) && ( applType == null || applType == String.Empty ) )
				return null;

			acctTypeDetails = GlobalObjects.Instance[ "AcctTypeDetails" ] as ArrayList;

			if ( acctTypeDetails == null )
			{
				PString acctTypeDetailStr = new PString("AcctTypeDetails"); 
				string[] acctTypeDetailStrArr = null;
				acctTypeDetails = new ArrayList();

                //Begin #19415
                bool fetchAll = IsRunningOnClient;
                if (IsRunningOnServer)
                {
                    /*Begin- Task #124261*/
                    PString sAcctType = new PString();
                    PString sApplType = new PString();
                    sAcctType.Value = acctType;
                    sApplType.Value = applType;
                    /*End- Task #124261*/

                    #region sql
                    // WI#21709 - Fixed Place Holder Numbering - {1} was used for status = and acct_Type = 
                    /*Task #124261 - Using SqlString to escape quotes incase of SQL injection attempt*/
                    string sql = string.Format(@"
                    SELECT	rtrim(A.ACCT_TYPE)+'~'+rtrim(B.APPL_TYPE)+'~'+B.DEP_LOAN+'~'+B.ACCT_NO_FORMAT
						FROM	{0}AD_GB_ACCT_TYPE A,
								{0}AD_GB_APPL_TYPE B
						WHERE	A.APPL_TYPE = B.APPL_TYPE
						AND		A.STATUS = '{1}'
                        AND     (A.ACCT_TYPE = {2} or A.APPL_TYPE = {3})", CoreService.DbHelper.DbPrefix,
                                                    GlobalVars.Instance.ML.Active, sAcctType.SqlString, sApplType.SqlString);  /*Task #124261*/
                    #endregion

                    Phoenix.Shared.BusFrame.SqlHelper sqlHelper = new Phoenix.Shared.BusFrame.SqlHelper();
                    if (!sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, acctTypeDetailStr))
                        fetchAll = true;

                }
                //End #19415
                if (fetchAll)     // #19415 - check for fetchAll
    				DataService.Instance.ProcessCustomAction(this, "AcctTypeDetails", acctTypeDetailStr );
				acctTypeDetailStrArr = acctTypeDetailStr.Value.Split("^".ToCharArray());
				for ( int i = 0; i <= acctTypeDetailStrArr.GetUpperBound(0); i++ )
				{
					if ( acctTypeDetailStrArr[i] != null && acctTypeDetailStrArr[i] != String.Empty )
						acctTypeDetails.Add( new AcctTypeDetail( acctTypeDetailStrArr[i] ));
				}
                if (fetchAll)     // #19415 - check for fetchAll
				    GlobalObjects.Instance[ "AcctTypeDetails" ] = acctTypeDetails;
			}

			foreach( AcctTypeDetail acctTypeDetail in acctTypeDetails )
			{
                //Begin #80630
				//if ( acctType == acctTypeDetail.AcctType || applType == acctTypeDetail.ApplType )
                if (( acctType != null && acctTypeDetail.AcctType != null && acctType.Trim() == acctTypeDetail.AcctType.Trim()) ||
                    ( applType != null && acctTypeDetail.ApplType != null && applType.Trim() == acctTypeDetail.ApplType.Trim()))
                //End #80630
					return acctTypeDetail;
			}

			return null;

		}
		#endregion
        

		#region GetNameDetails
		public void GetNameDetails(int rimNo, ref string firstName,ref string middleInitial, ref string lastName,ref string rimType, ref string title, ref string suffix)
		{
			PInt RimNo = new PInt("A0",rimNo);
			PString FirstName = new PString("A1",firstName);
			PString MiddleInitial = new PString("A2",middleInitial);
			PString LastName = new PString("A3",lastName);
			PString RimType = new PString("A4",rimType);
			PString Title = new PString("A5",title);
			PString Suffix = new PString("A6",suffix);
			PString FullName = new PString("A7");	//Added for Enh# 69248
			DataService.Instance.ProcessCustomAction(this, "GetNameDetails", RimNo,FirstName,MiddleInitial,LastName ,RimType,Title,Suffix, FullName);

			firstName = FirstName.Value;
			middleInitial = MiddleInitial.Value;
			lastName = LastName.Value;
			rimType = RimType.Value;
			title = Title.Value;
			suffix = Suffix.Value;
		}


        /*Begin #80643 - BA's want the name formatted diffrently depending on situations. Mode is used to make sure we do not keep creating
         overloads as it is getting confusing.*/
        public string GetNameDetails(int pnRimNo, string Mode)
        {
            PInt RimNo = new PInt("A0", pnRimNo);
            PString FirstName = new PString("A1");
            PString MiddleInitial = new PString("A2");
            PString LastName = new PString("A3");
            PString RimType = new PString("A4");
            PString Title = new PString("A5");
            PString Suffix = new PString("A6");
            PString FullName = new PString("A7");

            DataService.Instance.ProcessCustomAction(this, "GetNameDetails", RimNo, FirstName, MiddleInitial, LastName, RimType, Title, Suffix, FullName);

            string sCustName = string.Empty;

            if (Mode == string.Empty || Mode == "")
            {
                Mode = "Default";   //#10886
            }

            if (Mode == "Default" || Mode == "1099C")   //#80629 - Added 'Default'
            {
                if (RimType.Value.Trim() == "Personal")
                {
                    if (!LastName.IsNull)
                        sCustName += LastName.Value;

                    if (!Suffix.IsNull)
                        sCustName += " " + Suffix.Value;

                    if (!FirstName.IsNull)
                        sCustName += ", " + FirstName.Value;

                    if (!MiddleInitial.IsNull)
                        sCustName += " " + MiddleInitial.Value;

                    if (!Title.IsNull)
                        sCustName = Title.Value + " " + sCustName;
                }
                else
                {
                    if (!LastName.IsNull)
                        sCustName += LastName.Value;

                    if (!FirstName.IsNull)
                        sCustName += ", " + FirstName.Value;
                }
            }

            return sCustName;
        }
        /*End #80643*/
        /*Begin-198018*/
        /// <summary>
        ///  For getting the name of unrelared user
        /// </summary>
        /// <param name="pnUnRelRimNo"></param>
        /// <returns></returns>
        public string GetUnRelatedName(int pnUnRelRimNo)
        {
            PInt unRelRimNo = new PInt("unRelRimNo", pnUnRelRimNo);
            PString unRelName = new PString("unRelName");

            DataService.Instance.ProcessCustomAction(this, "GetUnRelatedName", unRelRimNo, unRelName);

            return (unRelName.IsNull ? string.Empty : unRelName.StringValue);
        }
        /*End-198018*/
		//Begin #69248 - overloaded method for existing "getnamedtails"
		public string GetNameDetails(int pnRimNo)
		{
			PInt RimNo = new PInt("A0", pnRimNo);
			PString FirstName = new PString("A1");
			PString MiddleInitial = new PString("A2");
			PString LastName = new PString("A3");
			PString RimType = new PString("A4");
			PString Title = new PString("A5");
			PString Suffix = new PString("A6");
			PString FullName = new PString("A7");

			DataService.Instance.ProcessCustomAction(this, "GetNameDetails", RimNo,FirstName,MiddleInitial,LastName ,RimType,Title,Suffix, FullName);

			return (FullName.IsNull?string.Empty:FullName.StringValue);
		}

		//End #69248 - overloaded method for existing "getnamedtails"

        // Begin #74915
        public string GetNameDetails(int pnRimNo, bool noTitleSuffix, bool nonPersonalLastNameOnly,
            bool nonPersonalLastNameFirst )
        {
            PInt RimNo = new PInt("A0", pnRimNo);
            PString FirstName = new PString("A1");
            PString MiddleInitial = new PString("A2");
            PString LastName = new PString("A3");
            PString RimType = new PString("A4");
            PString Title = new PString("A5");
            PString Suffix = new PString("A6");
            PString FullName = new PString("A7");

            DataService.Instance.ProcessCustomAction(this, "GetNameDetails", RimNo, FirstName, MiddleInitial, LastName, RimType, Title, Suffix, FullName);

            if (!noTitleSuffix && !nonPersonalLastNameOnly && !nonPersonalLastNameFirst )
                return (FullName.IsNull ? string.Empty : FullName.StringValue);
            else
            {
                #region #01143 - Trim RimType.Value as it contains extra spaces
                RimType.Value = StringHelper.StrTrimX(RimType.Value);
                #endregion
                if (nonPersonalLastNameOnly && RimType.Value != "Personal")
                    return ( LastName.IsNull ? String.Empty : LastName.Value );

                if (RimType.Value == "Personal" || !nonPersonalLastNameFirst )
                    return ConcateNameX(
                                        (FirstName.IsNull ? string.Empty : FirstName.StringValue),
                                        (LastName.IsNull ? string.Empty : LastName.StringValue),
                                        (MiddleInitial.IsNull ? string.Empty : MiddleInitial.StringValue),
                                        false);
                else
                    return ConcateNameX(
                                        (FirstName.IsNull ? string.Empty : FirstName.StringValue),
                                        (LastName.IsNull ? string.Empty : LastName.StringValue),
                                        (MiddleInitial.IsNull ? string.Empty : MiddleInitial.StringValue),
                                        true);
            }
        }
        // End #74915
		#endregion

		#region ValidateAccount
		public bool ValidateAccount(string acctType, string acctNo)
		{
			if ( acctType.Length == 0 || acctNo.Length == 0 )
				return false;
			PString AccountType = new PString("AccountType");
			PString AccountNumber = new PString("AccountNumber");
			PInt Result = new PInt("Result");
			AccountType.Value = acctType;
			AccountNumber.Value = acctNo;
			DataService.Instance.ProcessCustomAction(this, "ValidateAcctNo", AccountType, AccountNumber, Result);
			if (Result.Value == 0)
				return false;
			else
				return true;
		}

        /*Begin Task #82748*/
        /*The GL ledger type should be an 'Asset' or 'Liability'  */
        public bool ValidateGLLederType(string acctType, string acctNo)
        {
            PString AccountType = new PString("AccountType");
            PString AccountNumber = new PString("AccountNumber");
            PInt Result = new PInt("Result");
            AccountType.Value = acctType;
            AccountNumber.Value = acctNo;
            DataService.Instance.ProcessCustomAction(this, "ValidateGLLederType", AccountType, AccountNumber, Result);
            if (Result.Value == 0)
                return false;
            else
                return true;
            
        }
        /*End Task #82748*/

        ///*Begin Task #82748*/
        ///*The GL ledger type should be an 'Asset' or 'Liability'  */
        //public bool ValidateGLLederType(string acctType, string acctNo)
        //{
        //    PString AccountType = new PString("AccountType");
        //    PString AccountNumber = new PString("AccountNumber");
        //    PInt Result = new PInt("Result");
        //    AccountType.Value = acctType;
        //    AccountNumber.Value = acctNo;
        //    DataService.Instance.ProcessCustomAction(this, "ValidateGLLederType", AccountType, AccountNumber, Result);
        //    if (Result.Value == 0)
        //        return false;
        //    else
        //        return true;

        //}
        ///*End Task #82748*/

        #endregion ValidateAccount

        #region ValidateTags
        public bool NotValidInputTag(ref string TagName,params Phoenix.FrameWork.BusFrame.FieldBase[] paramList )
		{

			if (paramList.Length > 0)
			{
				for (int i=0;i< paramList.Length;i++)
				{
					if (paramList[i].FieldStatus == FieldStatus.XmlLoad)
					{
						TagName = paramList[i].XmlTag;
						return false;
					}
				}

			}
			return true;
		}
		#endregion

		#region GenModX for Validating routing no check digit
		public bool GenModX(int modulus,string modStyle,string number, ref string checkDigit)
		{
			char[] sTokenize;
			short nDigitMultiplier ;
			short nCounter;
			int nCombinedTotal = 0;
			short nCheckDigit = 0;
			number = number.Substring(0,4)+ number.Substring(5,4);
			if (modulus == 10)
			{
				if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD10(007)"))
				{
					nDigitMultiplier = 7;
					sTokenize = number.ToCharArray(0,number.Length);
					nCounter = (short)sTokenize.Length;
					while( nCounter > 0 )
					{
						nCombinedTotal = nCombinedTotal + (int)Char.GetNumericValue(sTokenize[nCounter-1])* nDigitMultiplier;
						if (nDigitMultiplier == 7)
							nDigitMultiplier = 3;
						else if (nDigitMultiplier == 3 )
							nDigitMultiplier = 1;
						else
							nDigitMultiplier = 7;
						nCounter--;
					}
					while (nCombinedTotal > 10)
						nCombinedTotal = nCombinedTotal - modulus;
					if (nCombinedTotal == 0)
						nCheckDigit = 0;
					else
						nCheckDigit = (short)(modulus - nCombinedTotal) ;
					checkDigit = nCheckDigit.ToString();
					return true;
				}
				else
					return false;
			}
			return false;
		}

		#endregion

        #region FormatDate - Moved to Phoenix.Shared.Utility DateHelper
        /// <summary>
		/// This function transforms a date/time into a string of the format by MyValue refs(IntFormatDate)
		/// </summary>
		/// <param name="unformattedValue"></param>
		/// <param name="includeTimeStamp"></param>
		/// <returns></returns>
        [Obsolete("Don't use this; use DateHelper.FormatDate instead", false)]
		public string FormatDate(DateTime unformattedValue, bool includeTimeStamp)
		{
			try
			{
                //#73503 - commented out use the datehelper equivalent
                //DateTime dtValue = System.Convert.ToDateTime(unformattedValue);
                //if( includeTimeStamp )
                //    return dtValue.ToString( "MM/dd/yyyy HH:mm:ss");
                //else
                //    return String.Format( CultureInfo.CurrentUICulture, "{0:d}", dtValue );
                return DateHelper.FormatDate(unformattedValue, includeTimeStamp); //#73503
			}
			catch(Exception ex)
			{
				if (CoreService.LogPublisher.IsLogEnabled)
					CoreService.LogPublisher.LogDebug("Not a valid date:(" + unformattedValue.ToString() + ") Message:--> " + ex.Message );
				return unformattedValue.ToString();
			}
		}
		#endregion FormatDate

        #region acct alerts Status
        public AcctAlerts GetAcctAlerts(string acctType, string acctNo, ref int rimNo)
        {   //140780
            string app = Phoenix.FrameWork.Shared.Variables.GlobalVars.Module;
            return GetAcctAlerts(acctType, acctNo, ref rimNo, app);
        }
        public AcctAlerts GetAcctAlerts(string acctType, string acctNo, ref int rimNo, string application)  // WI#20348 added moduleName parameter
        {
            AcctTypeDetail acctTypeDetail;
            PString AcctType = new PString("AcctType");
            PString AcctNo = new PString("AcctNo");
            PString DepLoan = new PString("DepLoan");
            PBaseType Caution = new PBaseType("Caution");
            PBaseType Hold = new PBaseType("Hold");
            PBaseType NSF = new PBaseType("NSF");
            PBaseType REJ = new PBaseType("REJ");
            PBaseType UCF = new PBaseType("UCF");
            PBaseType Stop = new PBaseType("Stop");
            PBaseType Retirement = new PBaseType("Retirement");
            PBaseType Sweep = new PBaseType("Sweep");
            PBaseType Analysis = new PBaseType("Analysis");
            PBaseType HouseHold = new PBaseType("HouseHold");
            PInt RimNo = new PInt("RimNo");
            PBaseType CrossRef = new PBaseType("CrossRef"); /*#76429*/
            PBaseType Fraud = new PBaseType("Fraud"); /*#80660*/
			PBaseType AdvRestrict = new PBaseType("AdvRestrict"); //140796;
            AcctAlerts acctAlerts;
            PString PlanNo = new PString("PlanNo");    //#80679-2
            PDecimal SweepControlPtid = new PDecimal("SweepControlPtid");    //#80679-2
            PBaseType CustCrossRef = new PBaseType("CustCrossRef");    //#80679-2
            PInt CustNotesCount = new PInt("CustNotesCount");   //#80679-2
            PInt AcctNotesCount = new PInt("AcctNotesCount");  //#80679-2
            PInt ApplNotesCount = new PInt("ApplNotesCount");  //#80679-2
            PBaseType AcctRegD = new PBaseType("AcctRegD");			//140780
            PBaseType CustRegD = new PBaseType("CustRegD");			//140780
            PBaseType AcctBankrupt = new PBaseType("AcctBankrupt");			//140775
            PBaseType CustBankrupt = new PBaseType("CustBankrupt");			//140775
            PBaseType CustRelPkg = new PBaseType("CustRelPkg");			//140769
            PString ModuleName = new PString("ModuleName");           //WI#20348
            //22968 - for performance we moved this part of function to server side
          /*  if (!string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo))    //#80679-2
            {
                acctTypeDetail = GetAcctTypeDetails(acctType, null);
                if (acctTypeDetail == null)
                    return null;

                DepLoan.Value = acctTypeDetail.DepLoan;
                AcctType.Value = acctType;
                AcctNo.Value = acctNo;
            }

            RimNo.Value = rimNo;               //#80679-2
            ModuleName.Value = application;    //WI#20348

            if (!AcctType.IsNull && !AcctNo.IsNull) //#19103
            {
                RimNo.Value = GetRimNo(AcctType.Value, AcctNo.Value);
            }
            */
            /*#76429: Added CrossRef*/
            // 140780 Reg D, 140775 Bankrupt, 140769 Rel Pricing
            // WI#20348 passing moduleName
            // 22968 - moved outside 

			if (!string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo))		// 22968 (3) add wrapper
			{
				AcctType.Value = acctType;
				AcctNo.Value = acctNo;
			}

			RimNo.Value = rimNo;               //#80679-2	// 22968 (3)
            ModuleName.Value = application;

            DataService.Instance.ProcessCustomAction(this, "GetAcctAlerts", AcctType, AcctNo, DepLoan, RimNo, ModuleName, 
                Caution, Hold, NSF, REJ, UCF, Stop, Retirement, Sweep, Analysis, HouseHold, RimNo, CrossRef, Fraud,
                PlanNo, SweepControlPtid, CustCrossRef, CustNotesCount, AcctNotesCount, ApplNotesCount,
				AcctRegD, CustRegD, AcctBankrupt, CustBankrupt, CustRelPkg, DepLoan, AdvRestrict);   // 22968 (2) add DepLoan, 140796 - AdvRestrict


            acctAlerts = new AcctAlerts();

            acctAlerts.Caution = Convert.ToBoolean(Caution.ValueObject);
            acctAlerts.Hold = Convert.ToBoolean(Hold.ValueObject);
            acctAlerts.NSF = Convert.ToBoolean(NSF.ValueObject);
            acctAlerts.REJ = Convert.ToBoolean(REJ.ValueObject);
            acctAlerts.UCF = Convert.ToBoolean(UCF.ValueObject);
            acctAlerts.Stop = Convert.ToBoolean(Stop.ValueObject);
            acctAlerts.Retirement = Convert.ToBoolean(Retirement.ValueObject);
            acctAlerts.Sweep = Convert.ToBoolean(Sweep.ValueObject);
            acctAlerts.Analysis = Convert.ToBoolean(Analysis.ValueObject);
            acctAlerts.HouseHold = Convert.ToBoolean(HouseHold.ValueObject);
            acctAlerts.Signer = DepLoan.Value != GlobalVars.Instance.ML.GL;
            acctAlerts.CrossRef = Convert.ToBoolean(CrossRef.ValueObject);  /*#76429*/
            acctAlerts.Fraud = Convert.ToBoolean(Fraud.ValueObject);    //#80660
            acctAlerts.PlanNo = PlanNo.Value;   //#80679-2
            acctAlerts.SweepControlPtid = Convert.ToDecimal(SweepControlPtid.ValueObject);  //#80679-2
            acctAlerts.CustCrossRef = Convert.ToBoolean(CustCrossRef.ValueObject);  //#80679-2
            acctAlerts.CustNotesCount = Convert.ToInt16(CustNotesCount.ValueObject);    //#80679-2
            acctAlerts.AcctNotesCount = Convert.ToInt16(AcctNotesCount.ValueObject);    //#80679-2
            acctAlerts.ApplNotesCount = Convert.ToInt16(ApplNotesCount.ValueObject);    //#80679-2
            acctAlerts.DepLoan = DepLoan.Value; //#80679-2
            acctAlerts.AcctRegD = Convert.ToBoolean(AcctRegD.ValueObject);   //140780
            acctAlerts.CustRegD = Convert.ToBoolean(CustRegD.ValueObject);   //140780
            acctAlerts.AcctBankrupt = Convert.ToBoolean(AcctBankrupt.ValueObject);   //140775
            acctAlerts.CustBankrupt = Convert.ToBoolean(CustBankrupt.ValueObject);   //140775
            acctAlerts.CustRelPkg = Convert.ToBoolean(CustRelPkg.ValueObject);      //140769
			acctAlerts.AdvRestrict = Convert.ToBoolean(AdvRestrict.ValueObject);	// 140796

            if (!RimNo.IsNull)
                rimNo = RimNo.Value;

            return acctAlerts;

        }
        #endregion

        #region Calculate Business date
        public void CalcBusDate(PDateTime inputDt, PSmallInt prevNext, PSmallInt leadDays, PDateTime busDate)
        {
            if (!inputDt.IsNull)
            {
                DataService.Instance.ProcessCustomAction(this, "CalcBusDate", inputDt, prevNext, leadDays, busDate);
            }
            return;
        }
        #endregion

        #region AutoComplete
        //140793
        public void GetAutoCompleteList(PString listName, PInt emplId, out AutoCompleteStringCollection AutoCompleteList)
        {
            //private System.Windows.Forms.AutoCompleteStringCollection _OffsetNickNameList;
            AutoCompleteList = new AutoCompleteStringCollection();
            PString autoList = new PString("autoList");
            DataService.Instance.ProcessCustomAction(this, "GetAutoCompleteList", listName, emplId, autoList);
            AutoCompleteList.AddRange(autoList.Value.Split(new char[] { '|' })); 
            return;
        }

        public string GetAutoCompleteSelectedValue(PString listName, PInt emplId, PString value)
        {
            PString selectedValue = new PString("selectedValue");
            DataService.Instance.ProcessCustomAction(this, "GetAutoCompleteSelectedValue", listName, value, emplId, selectedValue);
            return selectedValue.Value;
        }

        #endregion

        #region GetRate
        public void GetRate(PSmallInt indexId, PDateTime targetDate, PDecimal rate)
        {
            if (!indexId.IsNull && indexId.Value!=short.MinValue)//#03334
            {
                DataService.Instance.ProcessCustomAction(this, "GetRate", indexId, targetDate, rate);
            }
            return;
        }
        #region #03334
        public void GetRate(PSmallInt indexId, PDateTime targetDate, PDecimal rate, BusObjectBase obj)
        {
            if (!indexId.IsNull && indexId.Value!=short.MinValue)
            {
                DataService.Instance.ProcessCustomAction(this, "GetRate", indexId, targetDate, rate);
            }
            else
            {
                obj.Messages.AddWarning(1397, null, string.Empty);
            }
            return;
        }
        #endregion
        #endregion

        #region GetDbRelease
        public string GetDbRelease()    //#80674
        {
            PString dbRelease = new PString("DbRelease");
            dbRelease.Value = string.Empty;
            DataService.Instance.ProcessCustomAction(this, "GetDbRelease", dbRelease);
            return dbRelease.Value;
        }
        #endregion

        #region CalculateTerm
        public bool CalculateTerm(DateTime startDate, DateTime endDate, ref int term, ref string period)
        {
            int startDay = 0;
            int startMonth = 0;
            int startYear = 0;
            int endDay = 0;
            int endMonth = 0;
            int endYear = 0;
            //int dayIndex = 0;
            int totalDays = 0;
            int totalMonths = 0;
            int totalYears = 0;
            period = period.Trim();

            try
            {
                if (startDate == DateTime.MinValue || endDate == DateTime.MinValue ||
                    startDate == endDate)
                    return false;

                startDay = startDate.Day;
                startMonth = startDate.Month;
                startYear = startDate.Year;
                //
                endDay = endDate.Day;
                endMonth = endDate.Month;
                endYear = endDate.Year;

                if (startDay == endDay)
                {
                    if (startMonth == endMonth)
                    {
                        term = endYear - startYear;
                        period = CoreService.Translation.GetListItemX(ListId.Periods, "Year(s)");
                    }
                    else
                    {
                        totalMonths = endMonth - startMonth;
                        totalYears = endYear - startYear;
                        totalMonths = totalMonths + (totalYears * 12);
                        term = totalMonths;
                        period = CoreService.Translation.GetListItemX(ListId.Periods, "Month(s)");
                    }
                }
                else
                {
                    TimeSpan tempTimeSpan = new TimeSpan();
                    tempTimeSpan = endDate - startDate;
                    totalDays = tempTimeSpan.Days;
                    if (NumberHelper.NumberMod(totalDays, 7) == 0)
                    {
                        term = totalDays / 7;
                        period = CoreService.Translation.GetListItemX(ListId.Periods, "Week(s)");
                    }
                    else
                    {
                        term = totalDays;
                        period = CoreService.Translation.GetListItemX(ListId.Periods, "Day(s)");
                    }
                }

            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        //Begin - #176995 (Arun)
        #region CalculateTermMnths

        /// <summary>
        /// Returns Freq / Period  based on Start Date and End Date
        /// </summary>
        /// <returns></returns>
        public bool CalculateTermMnths(DateTime pdtStartDate, DateTime pdtEndDate, string parentPeriod, ref int rnTerm, ref string rsPeriod)
        {
            //Local variables
            int nStartDay;
            int nStartMonth;
            int nStartYear;
            int nEndDay;
            int nEndMonth;
            int nEndYear;
            int nTotalDays;
            int nTotalMonths;
            int nTotalYears;
            DateTime dtTempCalcDt;
            int nCalcDays;

            if (pdtStartDate == DateTime.MinValue || pdtEndDate == DateTime.MinValue || pdtStartDate == pdtEndDate)
            {
                return false;
            }

            //Set nStartDay = SalDateDay( pdtStartDate )
            nStartDay = pdtStartDate.Day;
            //Set nStartMonth = SalDateMonth( pdtStartDate )
            nStartMonth = pdtStartDate.Month;
            //Set nStartYear = SalDateYear( pdtStartDate )
            nStartYear = pdtStartDate.Year;
            //Set nEndDay = SalDateDay( pdtEndDate )
            nEndDay = pdtEndDate.Day;
            //Set nEndMonth = SalDateMonth( pdtEndDate )
            nEndMonth = pdtEndDate.Month;
            //Set nEndYear = SalDateYear( pdtEndDate )
            nEndYear = pdtEndDate.Year;

            if (StringHelper.StrTrimX(parentPeriod) == "Day(s)")
            {
                //Set nTotalDays = pdtEndDate - pdtStartDate
                //TimeSpan tempTimeSpan = new TimeSpan();
                TimeSpan tempTimeSpan = pdtEndDate - pdtEndDate;
                nTotalDays = tempTimeSpan.Days;

                //Set rnTerm = nTotalDays
                rnTerm = nTotalDays;

                //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Day(s)' )
                rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Day(s)");
            }

            //If SalStrTrimX(parentPeriod) = 'Week(s)'
            if (StringHelper.StrTrimX(parentPeriod) == "Week(s)")
            {
                //Set nTotalDays = pdtEndDate - pdtStartDate
                //TimeSpan tempTimeSpan = new TimeSpan();
                TimeSpan tempTimeSpan = pdtEndDate - pdtEndDate;
                nTotalDays = tempTimeSpan.Days;

                //If SalNumberMod( nTotalDays, 7 ) = 0
                if (NumberHelper.NumberMod(nTotalDays, 7) == 0)
                {
                    //Set rnTerm = nTotalDays / 7
                    rnTerm = nTotalDays / 7;
                    //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Week(s)' )
                    rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Week(s)");
                }
                //    Else
                else
                {
                    //Set rnTerm = SalNumberRound(nTotalDays / 7)
                    rnTerm = NumberHelper.NumberRound(nTotalDays / 7);
                    //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Week(s)' )
                    rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Week(s)");
                }
            }
            //If SalStrTrimX(parentPeriod) = 'Month(s)'
            if (StringHelper.StrTrimX(parentPeriod) == "Month(s)")
            {
                //Set nTotalMonths = nEndMonth - nStartMonth
                nTotalMonths = nEndMonth - nStartMonth;
                //Set nTotalYears = nEndYear - nStartYear
                nTotalYears = nEndYear - nStartYear;
                //Set nTotalMonths = nTotalMonths + ( nTotalYears * 12 )
                nTotalMonths = nTotalMonths + (nTotalYears * 12);
                //Set rnTerm =nTotalMonths
                rnTerm = nTotalMonths;
                //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Month(s)' )
                rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Month(s)");
            }
            //If SalStrTrimX(parentPeriod) = 'Year(s)'
            if (StringHelper.StrTrimX(parentPeriod) == "Year(s)")
            {
                //Set rnTerm = nEndYear - nStartYear
                rnTerm = nEndYear - nStartYear;
                //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Year(s)' )
                rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Year(s)");
                //Set dtTempCalcDt =  IntCalculateDateX( pdtStartDate, rnTerm, rsPeriod )
                dtTempCalcDt = CalculateDate(pdtStartDate, rnTerm, rsPeriod);
                //Set nCalcDays = dtTempCalcDt - pdtEndDate
                TimeSpan tempTimeSpan = dtTempCalcDt - pdtEndDate;
                nCalcDays = tempTimeSpan.Days;
                //If SalNumberAbs(nCalcDays) >45
                if (NumberHelper.NumberAbs(nCalcDays) > 45)
                {
                    //Set nTotalMonths = nEndMonth - nStartMonth
                    nTotalMonths = nEndMonth - nStartMonth;
                    //Set nTotalYears = nEndYear - nStartYear
                    nTotalYears = nEndYear - nStartYear;
                    //Set nTotalMonths = nTotalMonths + ( nTotalYears * 12 )
                    nTotalMonths = nTotalMonths + (nTotalYears * 12);
                    //Set rnTerm =nTotalMonths
                    rnTerm = nTotalMonths;
                    //Set rsPeriod = IntMLItemX( ML_NC_Periods, 'Month(s)' )
                    rsPeriod = CoreService.Translation.GetListItemX(ListId.Periods, "Month(s)");
                }
            }

            //Set rsPeriod = SalStrTrimX( rsPeriod )
            rsPeriod = StringHelper.StrTrimX(rsPeriod);

            //Return TRUE
            return true;
        }

        #endregion
        //End - #176995 (Arun)

        #region GetFromAdLnControl
        public bool GetFromAdLnControl(ref string availBal, ref string postBal, ref string cashAccrual)
        {
            try
            {
                PString AvailBal = new PString("AvailBal");
                PString PostBal = new PString("PostBal");
                PString CashAccrual = new PString("CashAccrual");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromAdLnControl", AvailBal, PostBal, CashAccrual);
                //
                availBal = AvailBal.Value;
                postBal = PostBal.Value;
                cashAccrual = CashAccrual.Value;
                if (cashAccrual != string.Empty && cashAccrual != null)
                    cashAccrual = cashAccrual.Trim();
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region GetFromAdLnUmbControl
        public bool GetFromAdLnUmbControl(ref string cashAccrual)
        {
            try
            {
                PString CashAccrual = new PString("CashAccrual");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromAdLnUmbControl", CashAccrual);
                //
                cashAccrual = CashAccrual.Value;
                if (cashAccrual != string.Empty && cashAccrual != null)
                    cashAccrual = cashAccrual.Trim();
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region FormatAccount
        /// <summary>
        /// Format the account number based on format Mask
        /// </summary>
        /// <param name="acctNo"></param>
        /// <param name="acctNoFormat"></param>
        /// <returns></returns>

        public string FormatAccount(string acctNo, string acctNoFormat)
        {

            if (acctNo == null)
                return null;

            StringBuilder formattedAccount = new StringBuilder(acctNoFormat);
            int inputIndex = acctNo.Length - 1;
            int fmtIndex = acctNoFormat.Length - 1;
            //int newLocation = 60;

            for (; fmtIndex > -1; fmtIndex--)
            {
                char fmtMask = acctNoFormat[fmtIndex];
                //newLocation--;
                bool foundValue = false;
                if (fmtMask == '9')
                {

                    for (; inputIndex > -1 && foundValue == false; inputIndex--)
                    {
                        char acctValue = acctNo[inputIndex];
                        if (char.IsNumber(acctValue))
                        {
                            foundValue = true;
                            formattedAccount[fmtIndex] = acctValue;
                        }

                    }
                    if (!foundValue)
                        formattedAccount[fmtIndex] = '0';

                }
                else if (fmtMask == '!') // For GL the ! marks are used
                {

                    for (; inputIndex > -1 && foundValue == false; inputIndex--)
                    {
                        char acctValue = acctNo[inputIndex];
                        if (acctValue != '-') // if this is not account format character
                        {
                            foundValue = true;
                            formattedAccount[fmtIndex] = acctValue;
                        }

                    }
                    if (!foundValue)
                        formattedAccount[fmtIndex] = '0';
                }


            }
            return formattedAccount.ToString();


        }
        #endregion

        #region GetFromAdDpControl
        public bool GetFromAdDpControl(ref string availBal, ref string postBal, ref string colBal)
        {
            try
            {
                PString AvailBal = new PString("AvailBal");
                PString PostBal = new PString("PostBal");
                PString ColBal = new PString("colBal");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromAdDpControl", AvailBal, PostBal, ColBal);
                //
                availBal = AvailBal.Value;
                postBal = PostBal.Value;
                colBal = ColBal.Value;
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region GetFromAdSdControl
        public bool GetFromAdSdControl(ref string cashAccrual)
        {
            try
            {
                PString CashAccrual = new PString("CashAccrual");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromAdSdControl", CashAccrual);
                //
                cashAccrual = CashAccrual.Value;
                if (cashAccrual != string.Empty && cashAccrual != null)
                    cashAccrual = cashAccrual.Trim();
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region GetFromHcHc
        public bool GetFromHcHc(ref string multiCurrency, ref string holdingCompany,
            ref string callReportType)
        {
            try
            {
                PString MultiCurrency = new PString("MultiCurrency");
                PString HoldingCompany = new PString("HoldingCompany");
                PString CallReportType = new PString("HoldingCompany");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromHcHc", MultiCurrency,
                    HoldingCompany, CallReportType);
                //
                multiCurrency = MultiCurrency.Value;
                holdingCompany = HoldingCompany.Value;
                callReportType = CallReportType.Value;
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region GetFromAdGbBankControl
        public bool GetFromAdGbBankControl(ref string phoneMask, ref int phoneMaskLength,
            ref int noDaysFuture, ref int noDaysBack, ref string defaultTIN, ref string formatTIN,
            ref string defaultPhone, ref string formatPhone, ref string defaultZIP, ref string formatZIP,
            ref string phoenixJobCode, ref int intlCountryCode, ref string acctTypeSearch, ref int databaseLanguageId,
            ref int achDRLeadDays, ref int achCRLeadDays, ref string requirePrenote, ref int prenoteLeadDays,
            ref string checkNSF, ref string genSecClsRel, ref string networkEmail, ref string bankID, ref string viewAudits)
        {

            try
            {
                PString PhoneMask = new PString("PhoneMask");
                PInt PhoneMaskLength = new PInt("phoneMaskLength");
                PInt NoDaysFuture = new PInt("noDaysFuture");
                PInt NoDaysBack = new PInt("noDaysBack");
                PString DefaultTIN = new PString("defaultTIN");
                PString FormatTIN = new PString("formatTIN");
                PString DefaultPhone = new PString("defaultPhone");
                PString FormatPhone = new PString("formatPhone");
                PString DefaultZIP = new PString("defaultZIP");
                PString FormatZIP = new PString("formatZIP");
                PString PhoenixJobCode = new PString("phoenixJobCode");
                PInt IntlCountryCode = new PInt("intlCountryCode");
                PString AcctTypeSearch = new PString("acctTypeSearch");
                PInt DatabaseLanguageId = new PInt("databaseLanguageId");
                PInt AchDRLeadDays = new PInt("achDRLeadDays");
                PInt AchCRLeadDays = new PInt("achCRLeadDays");
                PString RequirePrenote = new PString("requirePrenote");
                PInt PrenoteLeadDays = new PInt("prenoteLeadDays");
                PString CheckNSF = new PString("checkNSF");
                PString GenSecClsRel = new PString("genSecClsRel");
                PString NetworkEmail = new PString("networkEmail");
                PString BankID = new PString("bankID");
                PString ViewAudits = new PString("viewAudits");
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromAdGbBankControl", PhoneMask,   PhoneMaskLength,  NoDaysFuture,
                    NoDaysBack,  DefaultTIN,  FormatTIN,  DefaultPhone,  FormatPhone,  DefaultZIP,
                    FormatZIP,  PhoenixJobCode,  IntlCountryCode,  AcctTypeSearch,  DatabaseLanguageId,
                    AchDRLeadDays,  AchCRLeadDays,  RequirePrenote,  PrenoteLeadDays,  CheckNSF,
                    GenSecClsRel,  NetworkEmail,  BankID,  ViewAudits);
                //
                phoneMask = PhoneMask.Value;
                phoneMaskLength = PhoneMaskLength.Value;
                noDaysFuture = NoDaysFuture.Value;
                noDaysBack = NoDaysBack.Value;
                defaultTIN = DefaultTIN.Value;
                formatTIN = FormatTIN.Value;
                defaultPhone = DefaultPhone.Value;
                formatPhone = FormatPhone.Value;
                defaultZIP = DefaultZIP.Value;
                formatZIP = FormatZIP.Value;
                phoenixJobCode = PhoenixJobCode.Value;
                intlCountryCode = IntlCountryCode.Value;
                acctTypeSearch = AcctTypeSearch.Value;
                databaseLanguageId = DatabaseLanguageId.Value;
                achDRLeadDays = AchDRLeadDays.Value;
                achCRLeadDays = AchCRLeadDays.Value;
                requirePrenote = RequirePrenote.Value;
                prenoteLeadDays = PrenoteLeadDays.Value;
                checkNSF = CheckNSF.Value;
                genSecClsRel = GenSecClsRel.Value;
                networkEmail = NetworkEmail.Value;
                bankID = BankID.Value;
                viewAudits = ViewAudits.Value;
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region Generate Account
        public bool GenerateAccount(string modStyle, string depLoan, string acctType,
            string applType, ref string nextAcctNo)
        {
            nextAcctNo = string.Empty;

            try
            {
                PString ModStyle = new PString("ModStyle");
                PString DepLoan = new PString("DepLoan");
                PString AcctType = new PString("AcctType");
                PString ApplType = new PString("ApplType");
                PString AcctNo = new PString("AcctNo");
                PString NextAcct = new PString("NextAcct");
                //
                ModStyle.Value = modStyle;
                DepLoan.Value = depLoan;
                AcctType.Value = acctType;
                ApplType.Value = applType;
                AcctNo.Value = nextAcctNo;
                //
                DataService.Instance.ProcessCustomAction(this, "GenerateAccount", ModStyle, DepLoan,
                    AcctType, ApplType, AcctNo, NextAcct);
                //
                #region #02512
                //ITS #02512 - set value of nextAcctNo = AcctNo
                //nextAcctNo = NextAcct.Value;
                nextAcctNo = AcctNo.Value;
                #endregion
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        #region ModAccount
        public bool ModAccount(int modulus, string modStyle, string nxtAcctNo, ref string newAcctNo)
        {
            //
            //This function handles the generating of Mod 10, 11 style account numbers
            //
            int maxLength = 0;
            int position = 0;
            int oldLength = 0;
            string testChar = "";
            string workNumber = "";
			long oldNumber = 0;				//#38867 changed to long
			long tempWorkNumber = 0;		//#38867 changed to long
            string checkDigit = "";

            #region remove any formatting characters from the number
	        nxtAcctNo = StringHelper.StrTrimX(nxtAcctNo);
	        maxLength = StringHelper.StrLength( nxtAcctNo );
            while (position < maxLength)
            {
		        testChar = StringHelper.StrMidX( nxtAcctNo, position, 1);
		        if (testChar == "-" || testChar == "/") //no need to increment on the replace
			        StringHelper.StrReplace(nxtAcctNo, position, 1, string.Empty, ref nxtAcctNo );
		        else
			        position = position + 1;
            }
            oldLength = StringHelper.StrLength(nxtAcctNo);	// get our old length so we know how much to pad by
            #endregion
            //
            if (modulus == -1) // sequential
            {
	           // workNumber = NumberHelper.NumberToStringX(StringHelper.StrToNumber(nxtAcctNo) + 1, 0 ); //#36979 used convert.toint64
				workNumber = NumberHelper.NumberToStringX(Convert.ToInt64(nxtAcctNo) + 1, 0);
	            newAcctNo = StringHelper.PadLeft(workNumber, oldLength, '0');
                return true;
            }
            //
            workNumber = nxtAcctNo;
			//oldNumber = Convert.ToInt32(StringHelper.StrToNumber(StringHelper.StrLeftX(workNumber, Convert.ToInt32(StringHelper.StrLength(workNumber) - Convert.ToInt32(1)))) + Convert.ToInt32(1));		//#38867 used convert.toint64
			oldNumber = Convert.ToInt64(Convert.ToInt64(StringHelper.StrLeftX(workNumber, Convert.ToInt32(StringHelper.StrLength(workNumber) - Convert.ToInt32(1)))) + Convert.ToInt32(1));
            tempWorkNumber = oldNumber;
            workNumber = NumberHelper.NumberToStringX( tempWorkNumber, 0 );
            if (GenMod(modulus, modStyle, workNumber, ref checkDigit))
            {
	            if (checkDigit == "10")
                {
					//workNumber = NumberHelper.NumberToStringX( StringHelper.StrToNumber( workNumber ) + 1, 0 );			//#38867 used convert.toint64
					workNumber = NumberHelper.NumberToStringX(Convert.ToInt64(workNumber) + 1, 0);
		            if (!GenMod( modulus, modStyle, workNumber, ref checkDigit ))
                        return false;
                }
                newAcctNo = StringHelper.PadLeft(workNumber + checkDigit, oldLength, '0');
                return true;
            }
            else
                return false;

        }
        #endregion

        #region GenMod
        public bool GenMod(int modulus, string modStyle, string workNumber, ref string checkDigit)
        {
            //
            //This function handles the generating of Mod 10 and 11 check digits
            //
//int oddTotal = 0;
//int evenTotal = 0;
//int tempWorkNumber = 0;
//bool odd;
            int combinedTotal = 0;
            int tempCheckDigit = 0;
            int tempCurrentDigit = 0;
            string currentDigit = "";
            int digitMultiplier = 0;
//int oldNumber = 0;
            //
            if (modulus == 10)
            {
                if (modStyle == CoreService.Translation.GetListItemX( ListId.Methods, "MOD10(001)" ))
                {
	                digitMultiplier = 2;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
		                tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber( StringHelper.StrRightX( workNumber, 1 )));
		                tempCurrentDigit = tempCurrentDigit * digitMultiplier;
		                if (tempCurrentDigit > 9) // then we need to split and add
                        {
			                currentDigit = NumberHelper.NumberToStringX( tempCurrentDigit, 0 );
			                tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber( StringHelper.StrMidX( currentDigit, 0, 1 ) ) +
							                StringHelper.StrToNumber( StringHelper.StrMidX( currentDigit, 1, 1 )));
                        }
		                combinedTotal = combinedTotal + tempCurrentDigit;
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
		                if (digitMultiplier == 2)
			                digitMultiplier = 1;
		                else
			                digitMultiplier = 2;
                    }
                }
                else if (modStyle == CoreService.Translation.GetListItemX( ListId.Methods, "MOD10(007)" ))
                {
	                digitMultiplier = 7;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
		                tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber( StringHelper.StrRightX( workNumber, 1 )));
		                tempCurrentDigit = tempCurrentDigit * digitMultiplier;
		                combinedTotal = combinedTotal + tempCurrentDigit;
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
		                if (digitMultiplier == 7)
			                digitMultiplier = 3;
		                else if (digitMultiplier == 3)
			                digitMultiplier = 1;
		                else
			                digitMultiplier = 7;
                    }
                }
                else if (modStyle == CoreService.Translation.GetListItemX( ListId.Methods, "MOD10(ABA)" ))
                {
	                digitMultiplier = 7;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
                        tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber(StringHelper.StrRightX(workNumber, 1)));
		                combinedTotal = combinedTotal + (tempCurrentDigit * digitMultiplier);
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
		                if (digitMultiplier == 7)
			                digitMultiplier = 3;
		                else if (digitMultiplier == 3)
			                digitMultiplier = 1;
		                else
			                digitMultiplier = 7;
                    }
                }
                else
                {
                    System.Diagnostics.Trace.WriteLine("Invalid request to generate a mod account - unknown mod style!");
	                return false;
                }
            }
            else if (modulus == 11)
            {
                if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(003)"))
                {
	                digitMultiplier = 2;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
                        tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber(StringHelper.StrRightX(workNumber, 1)));
		                combinedTotal = combinedTotal + (tempCurrentDigit * digitMultiplier);
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
		                digitMultiplier = digitMultiplier + 1;
		                if (digitMultiplier == 14)
			                digitMultiplier = 2;
                    }
                }
                else if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(AUS)"))
                {
	                digitMultiplier = 2;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
                        tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber(StringHelper.StrRightX(workNumber, 1)));
		                combinedTotal = combinedTotal + (tempCurrentDigit * digitMultiplier);
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
		                digitMultiplier = digitMultiplier + 1;
                    }
                }
                else if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(005)"))
                {
	                digitMultiplier = 2;
	                while (StringHelper.StrLength( workNumber ) > 0)
                    {
                        tempCurrentDigit = Convert.ToInt32(StringHelper.StrToNumber(StringHelper.StrRightX(workNumber, 1)));
		                combinedTotal = combinedTotal + (tempCurrentDigit * digitMultiplier);
		                workNumber = StringHelper.StrLeftX( workNumber, StringHelper.StrLength( workNumber ) - 1 );
                        digitMultiplier = digitMultiplier * 2;
                    }
                }
            }
            else
            {
                System.Diagnostics.Trace.WriteLine("Invalid request to generate a mod account - unknown mod style!");
	            return false;
            }
            //this performs modulus function. Remainder left in nCombinedTotal
            while (combinedTotal > 10)
            {
	            combinedTotal = combinedTotal - modulus;
            }
            if (combinedTotal == 0)
            {
                if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(AUS)"))
		            tempCheckDigit = 9;
	            else
		            tempCheckDigit = 0;
            }
            else
            {
                if (modStyle == CoreService.Translation.GetListItemX(ListId.Methods, "MOD11(AUS)"))
		            tempCheckDigit = 10 - combinedTotal;
	            else
		            tempCheckDigit = modulus - combinedTotal;
            }
            checkDigit = NumberHelper.NumberToStringX(tempCheckDigit, 0);
            return true;
        }
        #endregion

        #region GenModX
        public string GenModX(int modules, string modStyle, string workNumber)
        {
            //
            //This function handles the generating of Mod 10 and 11 check digits
            //
            string checkDigit = "";
            if (GenModX(modules, modStyle, workNumber, ref checkDigit))
                return checkDigit;
            return checkDigit;
        }
        #endregion

        #region GetFromGLControl
        public bool GetFromGLControl(ref string restrictGlAccess, ref string glAcctNoFormat)
        {
            try
            {
                PString RestrictGlAccess = new PString("RestrictGlAccess");
                PString GlAcctNoFormat = new PString("GlAcctNoFormat"); //Selva-New
                //
                DataService.Instance.ProcessCustomAction(this, "GetFromGLControl", RestrictGlAccess,
                    GlAcctNoFormat);
                //
                restrictGlAccess = RestrictGlAccess.Value;
                glAcctNoFormat = GlAcctNoFormat.Value;
            }
            catch
            {
                return false;
            }
            //
            return true;
        }
        #endregion

        //Begin #04976
        #region Get Work Details For New Acct
        /// <summary>
        /// Return the required details for creating a new acct. We do this function as the logic is shared between DP, LN, SD, EXT windows
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>
        public void GetWorkDetailsForNewAcct(decimal workIdParam, ref int empIdParam,
                                                ref int prodClassCodeParam,
                                               ref int reasonIdParam, ref string prodAcctTypeParam)
        {
            #region #04976
            PDecimal workId = new PDecimal("A0", workIdParam);
            #endregion
            PInt empId = new PInt("A1");
            PInt prodClassCode = new PInt("A2");
            PInt reasonId = new PInt("A3");
            PString prodAcctType = new PString("A4");


            DataService.Instance.ProcessCustomAction(this, "GetWorkDetailsForNewAcct",
                                                        workId, empId, prodClassCode, reasonId,
                                                        prodAcctType);
            empIdParam = empId.Value;
            prodClassCodeParam = prodClassCode.Value;
            reasonIdParam = reasonId.Value;
            prodAcctTypeParam = prodAcctType.Value;

            return;
        }
        #endregion
        #region Nickname Update
        /// <summary>
        /// Updates Nicknames.RETURNS TRUE if successfull, False and Error Messege otherwise.
        /// </summary>
        /// <param name= "busObj"> BusObject used to call this Fn</param>
        /// <param name= "gbMapAcctRelParam"> bool MapAcctRel true in case update table "gb_map_acct_rel"</param>
        /// <param name= "xpRmSvcsAcctParam"> bool xpRmSvcsAcct true in case update table "xp_rm_svcs_acct"</param>
        /// <param name= "acctTypeParam">Acct Type</param>
        /// <param name= "acctNoParam">Acct No</param>
        /// <param name= "rimNoParam">Rim No</param>
        /// <param name= "nicknameParam">Modified NickName</param>
        /// <returns></returns>
        public bool NicknameUpdate(BusObjectBase busObj ,bool gbMapAcctRelParam,
                                    bool xpRmSvcsAcctParam,
                                   string acctTypeParam, string acctNoParam,
                                    int rimNoParam, string nicknameParam)
        {
            PBoolean gbMapAcctRel = new PBoolean("A0", gbMapAcctRelParam);
            PBoolean xpRmSvcsAcct = new PBoolean("A1", xpRmSvcsAcctParam);
            PString acctType = new PString("A2", acctTypeParam);
            PString acctNo = new PString("A3", acctNoParam);
            PInt rimNo = new PInt("A4", rimNoParam);
            PString nickname = new PString("A5", nicknameParam);
            PInt retNum = new PInt("A6");

            DataService.Instance.ProcessCustomAction(this,"NicknameUpdate",
                                                         gbMapAcctRel,
                                                         xpRmSvcsAcct,
                                                         acctType,
                                                         acctNo,
                                                         rimNo,
                                                         nickname,
                                                         retNum);
            if (retNum.Value != 0)
            {
                if (retNum.Value == 10696)
                {
                    //Unable to update Account Nickname
                    busObj.Messages.AddError(10696, null, null);
                }
                else if (retNum.Value == 10697)
                {
                    //Unable to update Service Nickname
                    busObj.Messages.AddError(10697, null, null);
                }
                else if (retNum.Value == 10697)
                {
                    //Unable to update ATM Nickname
                    busObj.Messages.AddError(10698, null, null);
                }

                return false;

            }
            else
            {
                return true;
            }



            //return true;
        }
        #endregion

        //End #04976
        //Begin WI #27540
        /// <summary>
        /// Get Edit and Post Access Flag of a Employee
        /// </summary>
        /// <param name="rimNo">rimNo</param>
        /// <param name="acctNo">acctNo</param>
        /// <param name="acctType">acctType</param>
        /// <param name="RetCode">return RetCode</param>
        /// <param name="Post">return Post</param>
        /// <param name="Edit">return Edit</param>
        public void GetPostEditAccess(int rimNo, string acctNo, string acctType, PBaseType RetCode, PBaseType Post, PBaseType Edit)
        {
            PInt RimNo = new PInt("RimNo");
            PString AcctNo = new PString("AcctNo");
            PString AcctType = new PString("AcctType");
            RimNo.Value = rimNo;
            AcctNo.Value = acctNo;
            AcctType.Value = acctType;
            DataService.Instance.ProcessCustomAction(this, "GetPostEditAccess", RimNo, AcctNo, AcctType, RetCode, Post, Edit);
        }
        //End WI #27540

        //Begin 90001
        public bool IsOvRunning()
        {
            bool ovRunning = false;

            OvControl ovControl = new OvControl();
            ovControl.Ptid.Value = 1;
            ovControl.SelectAllFields = false;
            ovControl.ReadOnly.Selected = true;
            CoreService.DataService.ProcessRequest(XmActionType.Select, ovControl);
            ovRunning = ovControl.ReadOnly.Value.ToLower() == "y";

            return ovRunning;
        }

        //End 90001

        /*Begin #46760*/
        public bool IsNachaHoliday(DateTime ProcessingDate)
        {
            PBoolean bHoliday = new PBoolean("bHoliday");
            PDateTime dtProcessingDate = new PDateTime("dtProcessingDate");
            dtProcessingDate.Value = ProcessingDate;

            DataService.Instance.ProcessCustomAction(this, "IsNachaHoliday",dtProcessingDate, bHoliday);
            if (bHoliday.Value)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /*End #46760*/

        #region GetBiProductType
        public string GetBiProductType()    //#63936
        {
            PString biProduct = new PString("biProduct");
            biProduct.Value = string.Empty;
            DataService.Instance.ProcessCustomAction(this, "GetBiProductType", biProduct);
            return biProduct.Value;
        }
        public string CustAnalysisExists(int rimNo)    //#63936
        {
            PInt RimNo = new PInt("RimNo");
            RimNo.Value = rimNo;
            PString AnalExists = new PString("AnalExists");
            AnalExists.Value = string.Empty;
            DataService.Instance.ProcessCustomAction(this, "CustAnalysisExists", RimNo, AnalExists);
            return AnalExists.Value;
        }
        #endregion
        // Begin- 94549 - Get Current Posting Date for the current branch and drawer.
        #region GetCurPostingDate
        public DateTime GetCurPostingDate(short ibranchNo, short idrawerNo)
        {
            PDateTime curPostDate = new PDateTime("curPostDate");
            PSmallInt drawerNo = new PSmallInt("drawerNo");
            PSmallInt branchNo = new PSmallInt("branchNo");
            drawerNo.Value = idrawerNo;
            branchNo.Value = ibranchNo;
            DataService.Instance.ProcessCustomAction(this, "GetCurPostingDate", curPostDate, branchNo, drawerNo);
            return curPostDate.Value;
        }
        // End- 94549
        #endregion
        #endregion Public Methods
        /// <summary>
        ///
        /// </summary>
        /// <param name="auditKey"></param>
        /// <param name="moduleName"></param>
        /// <param name="timeIn"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool CreateViewAudit(Phoenix.Windows.Forms.INotesInfo auditKey, string moduleName, DateTime timeIn, DateTime timeOut)
        {
            if (auditKey == null)
                return false;
            try
            {
                //@pdtTimeIn	datetime,
                //@pdtTimeOut	datetime,
                //@pnEmplID	smallint,
                //@pnScreenID	smallint,
                //@psAcctNo	varchar( 60) = null,			/* rmehta - 53465*/
                //@psAcctType	char( 3) = null,				/* rmehta - 53465*/
                //@psPhxApplCode	varchar(6) = null,		/* rmehta - 53465*/
                //@pnScreenPtid	numeric(12,0)			/* #62972 */
                PDateTime dtIn = new PDateTime("inTime");
                PDateTime dtOut = new PDateTime("outTime");
                PInt screenId = new PInt("screenId");
                PDecimal screenPtid = new PDecimal("screenPtid");
                PString acctType = new PString("acctType");
                PString acctNo = new PString("acctNo");
                PString application = new PString("moduleName");
                PInt emplId = new  PInt("emplId");
                // Load the Values
                dtIn.Value = timeIn;
                dtOut.Value = timeOut;
                screenId.Value =  auditKey.ScreenId;
                screenPtid.Value = auditKey.ScreenPtid;
                acctType.Value = auditKey.AcctType;
                acctNo.Value = auditKey.AcctNo;
                application.Value = moduleName;
                emplId.Value = BusGlobalVars.EmployeeId;

                // Begin Task#122492 - When No Acct Number and We Have a Rim Then Place in Account Fields So It prints on AUD020 Report
                if ((acctType.IsNull || acctType.Value == string.Empty) && (acctNo.IsNull || acctNo.Value == string.Empty) && auditKey.RimNo > 0)
                {
                    acctType.Value = "RIM";
                    acctNo.Value = auditKey.RimNo.ToString();
                }
                // End Task#122492

                // Call the service
                DataService.Instance.ProcessCustomAction(this, "CreateViewAudit", dtIn, dtOut, emplId, screenId,
                    acctType, acctNo, application, screenPtid);
                //
                return true;
            }
            catch( Exception e )
            {
                Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(e.ToString());
            }
            return false;
        }

        // Begin #73503 - rpoddar
        public string GetDuplicateRecordMessage(int mlMessageId, params string[] messageTokens)
        {
            string mlText = null;
            if (mlMessageId > 0)
            {
                mlText = CoreService.Translation.GetTokenizeMessageX(mlMessageId, messageTokens);
                mlText = mlText + " already exists.";
            }
            return mlText;
        }
        // End #73503

        // Begin #74308
        public bool ValidateDecimal(BusObjectBase busobj,
            string properName, decimal dataField,
            int precision, int scale, bool allowNegatives,
            bool greater100Percent)
        {
            string sNumber = null;
            string sLeft = null;
            string sRight = null;
            string sDecimalSymbol = null;
            string[] saTemp;

            // enclosing the XmlTag in "[]" will display the proper name in front end.
            if (properName != null && properName.IndexOf("[") < 0)
                properName = "[" + properName + "]";

            if (dataField != Decimal.MinValue)
            {
                if ((!allowNegatives) &&
                   ((dataField == Decimal.MinValue ? 0 : dataField) < 0))
                {
                    saTemp = new string[1];

                    saTemp[0] = properName;

                    if ( busobj != null )
                        busobj.Messages.AddError(3519, null, saTemp);

                    return false;
                }

                if ((!greater100Percent) &&
                   ((dataField == Decimal.MinValue ? 0 : dataField) > 100))
                {
                    saTemp = new string[1];

                    saTemp[0] = properName;

                    if (busobj != null)
                        busobj.Messages.AddError(3520, null, saTemp);

                    return false;
                }

                //sNumber = NumberHelper.NumberToStrX((dataField == Decimal.MinValue ? 0 : dataField), 99);
                sNumber = dataField.ToString( ) ;

                sDecimalSymbol = ".";

                if (StringHelper.StrScan(sNumber, ",") != -1)
                {
                    sDecimalSymbol = ",";
                }

                if (StringHelper.StrScan(sNumber, sDecimalSymbol) >= 0)
                {
                    sLeft = StringHelper.StrLeftX(sNumber, StringHelper.StrScan(sNumber, sDecimalSymbol));

                    sRight = StringHelper.StrRightX(sNumber, StringHelper.StrLength(sNumber) - (StringHelper.StrScan(sNumber, sDecimalSymbol) + 1));

                    while (StringHelper.StrRightX(sRight, 1) == "0" && StringHelper.StrLength(sRight) > 0)
                    {
                        sRight = StringHelper.StrLeftX(sRight, StringHelper.StrLength(sRight) - 1);
                    }
                }
                else
                {
                    sLeft = sNumber;
                    sRight = String.Empty;
                }

                if (StringHelper.StrLength(sRight) > (scale == Int32.MinValue ? 0 : scale))
                {
                    saTemp = new string[3];

                    saTemp[0] = properName;

                    saTemp[1] = StringHelper.StrRepeatX("9", (precision == Int32.MinValue ? 0 : precision) - (scale == Int32.MinValue ? 0 : scale));

                    saTemp[2] = StringHelper.StrRepeatX("9", (scale == Int32.MinValue ? 0 : scale));

                    if (busobj != null)
                        busobj.Messages.AddError(3521, null, saTemp);

                    return false;
                }

                else if (StringHelper.StrLength(sLeft) > ((precision == Int32.MinValue ? 0 : precision) - (scale == Int32.MinValue ? 0 : scale)))
                {
                    saTemp = new string[3];

                    saTemp[0] = properName;

                    saTemp[1] = StringHelper.StrRepeatX("9", (precision == Int32.MinValue ? 0 : precision) - (scale == Int32.MinValue ? 0 : scale));

                    saTemp[2] = StringHelper.StrRepeatX("9", (scale == Int32.MinValue ? 0 : scale));

                    if (busobj != null)
                        busobj.Messages.AddError(3522, null, saTemp);

                    return false;
                }
            }

            return true;
        }
        // End #74308

		#region #74018 - GetLoanAmountUndisbursed

		public bool GetLoanUndisbursedValues(string acctType, string acctNo, out decimal undisbursedAmount, out decimal borrowerFunds, out decimal borrowerNonMonFunds)
		{
            undisbursedAmount = 0;
            borrowerFunds = 0;
            borrowerNonMonFunds = 0;
            PString paramAcctType = new PString("acctType", acctType);
            PString paramAcctNo = new PString("acctNo", acctNo);
            PDecimal paramUndisbursed = new PDecimal("undisbursed", undisbursedAmount);
            PDecimal paramBorrowerFunds = new PDecimal("borrowerFunds", borrowerFunds);
            PDecimal paramBorrowerNonMonFunds = new PDecimal("borrowerNonMonFunds", borrowerNonMonFunds);
            //PString classdesc = new PString("A3");
            DataService.Instance.ProcessCustomAction(this, "GetLoanUndisbursedValues",
                paramAcctType, paramAcctNo, paramUndisbursed, paramBorrowerFunds, paramBorrowerNonMonFunds);

            if (this.Messages.ErrorCount != 0)
            {
                return false;
            }
            //
            undisbursedAmount = paramUndisbursed.Value;
            borrowerFunds = paramBorrowerFunds.Value;
            borrowerNonMonFunds = paramBorrowerNonMonFunds.Value;

            return true;

            //return (classdesc.IsNull ? string.Empty : classdesc.Value.ToString());

            //UndisbursedAmount = 0;
            //BorrowerFunds = 0;
            //BorrowerNonMonFunds = 0;

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

		#endregion

        #region Private Methods
        private string CleanRoutingNo(string psRoutingNo)
		{
			string sCleanedStr = string.Empty;

			char[] sTmpArr = psRoutingNo.ToCharArray();

			for (int i=0; i < sTmpArr.Length; i++)
			{
				if (sTmpArr[i].ToString() != "-")
					sCleanedStr += sTmpArr[i].ToString();
			}

			return sCleanedStr;
		}
		#endregion

        //Begin #75763
        public string GetAcctStatus(string acctType, string acctNo)
        {
            if (acctType == null || acctType.Length == 0 || acctNo == null || acctNo.Length == 0)
                return null;
            PString AccountType = new PString("AccountType");
            PString AccountNumber = new PString("AccountNumber");
            PString AccountStatus = new PString("AccountStatus");
            AccountType.Value = acctType;
            AccountNumber.Value = acctNo;
            DataService.Instance.ProcessCustomAction(this, "GetAcctStatus", AccountType, AccountNumber, AccountStatus);
            if (!AccountStatus.IsNull)
                return AccountStatus.Value;
            return null;
        }

        public bool IsViewOnlyDormantEscheatedAccess(string acctType, string acctNo)
        {
            return IsViewOnlyDormantEscheatedAccess( GetAcctStatus(acctType,acctNo) );
        }

        public bool IsViewOnlyDormantEscheatedAccess(string acctStatus )
        {
            string allowAccess  = null;
            string allowView = null;

            if (acctStatus != GlobalVars.Instance.ML.Dormant && acctStatus != GlobalVars.Instance.ML.Escheated)
                return false;
            if ( GlobalObjects.Instance[ GlobalObjectNames.AdGbRsm ] == null )
            {
                AdGbRsm adGbRsm = new AdGbRsm();
                adGbRsm.EmployeeId.Value = GlobalVars.EmployeeId;
                CoreService.DataService.ProcessRequest(XmActionType.Select, adGbRsm);
                GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] = adGbRsm;

            }

            if (acctStatus == GlobalVars.Instance.ML.Dormant)
            {
                allowAccess = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).DormantAccess.Value;
                allowView = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).AllowViewDormant.Value;
            }
            else
            {
                allowAccess = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).EscheatedAccess.Value;
                allowView = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).AllowViewEscheat.Value;
            }

            return ( allowAccess == GlobalVars.Instance.ML.N && allowView == GlobalVars.Instance.ML.Y );
        }

        //End #75763

        #region //Task#55416 
        // Need to allow old calls to work if not passing the new parameter screenId
        public bool IsConfAcct(string confAcctType, string confAcctNo)
        {
            return IsConfAcct(confAcctType, confAcctNo, false);
        }
        #endregion //Task#55416

        //Begin #80630
        public bool IsConfAcct(string confAcctType, string confAcctNo, bool ignoreRelatedAccts)          // Task#55416 - Add ignoreRelatedAccts parameter
        {
            //Begin #13814
            if (!AppInfo.Instance.IsAppOnline)
                return false;
            //End #13814

            //Begin #19415
            if (GlobalObjects.Instance[GlobalObjectNames.AdGbBankControl] != null &&
                (GlobalObjects.Instance[GlobalObjectNames.AdGbBankControl] as AdGbBankControl).ConfAcctSecurity.Value != GlobalVars.Instance.ML.Y)
                return false;
            //End #19415

            //Begin #140789
            bool primaryRimOnly = false;
            if (confAcctType != null && confAcctType.ToUpper() == "P_RIM")
            {
                confAcctType = GlobalVars.Instance.ML.RIM;
                primaryRimOnly = true;
            }
            //End #140789

            AcctTypeDetail acctTypeDetail = null;
            if (confAcctType != null && confAcctType.ToUpper() == "DP_UMB")
            {
                acctTypeDetail = new AcctTypeDetail(null, null, "UMB", null);
            }
            else if (confAcctType != null && confAcctType.ToUpper() == "ATM_PAN")
            {
                acctTypeDetail = new AcctTypeDetail(null, null, "APN", null);
            }
            //Begin #14147
            else if (confAcctType != null && confAcctType.ToUpper() == "H_HOLD")
            {
                acctTypeDetail = new AcctTypeDetail(null, null, "HHD", null);
            }
            //End #14147
            else
                acctTypeDetail = GetAcctTypeDetails(confAcctType, null);
            if (acctTypeDetail == null || "DP^LN^EX^RM^CM^UMB^APN^HHD^".IndexOf(acctTypeDetail.DepLoan + "^") < 0)  // #14147 - Added HHD
                return false;

            #region sqls
            bool isPrimaryDb = (DataService.Instance.XmDbStatus == XmDbStatus.Day || DataService.Instance.XmDbStatus == XmDbStatus.CopyOver);
            //Begin #19415
            if (IsRunningOnServer)
                isPrimaryDb = CoreService.DbHelper.CopyStatus == "D" || CoreService.DbHelper.CopyStatus == "C";
            //End #19415
            string sql = null;
            string tableName = null;



            if (acctTypeDetail.DepLoan == GlobalVars.Instance.ML.CM )
            {
                #region sql
                sql = string.Format(@"
select '{1}'
from xp_control
where exists( select * from {0}ln_umb_rel_map r, {0}{2} a
    where r.group_acct_type = '{3}'
    and r.group_acct_no = '{4}'
    and r.acct_type = a.acct_type
    and r.acct_no = a.acct_no
    and a.confidential = '{1}' )", "{0}", GlobalVars.Instance.ML.Y,
                                 isPrimaryDb ? "ln_acct" : "ln_display", confAcctType, confAcctNo);
                #endregion
            }
            else if (acctTypeDetail.DepLoan == "UMB")
            {
                #region sql
                sql = string.Format(@"
select '{1}'
from xp_control
where exists( select * from {0}{2} a
    where a.plan_no = '{3}'
    and a.confidential = '{1}' )", "{0}", GlobalVars.Instance.ML.Y,
                                 isPrimaryDb ? "dp_acct" : "dp_display", confAcctNo);
                #endregion
            }
            else if (acctTypeDetail.DepLoan == "APN")
            {
                #region sql
                sql = string.Format(@"
select '{1}' from {0}atm_rel_acct r
where r.pan = '{3}'
and ( exists( select * from {0}dp_{2} a
    where a.acct_type = r.acct_type
    and a.acct_no = r.acct_no
    and a.confidential = '{1}' )

    or exists( select * from {0}ln_{2} a
    where a.acct_type = r.acct_type
    and a.acct_no = r.acct_no
    and a.confidential = '{1}' )

    or exists( select * from {0}ex_acct a
    where a.acct_type = r.acct_type
    and a.acct_no = r.acct_no
    and a.confidential = '{1}' )
    )", "{0}", GlobalVars.Instance.ML.Y,
                                 isPrimaryDb ? "acct" : "display", confAcctNo);
                #endregion
            }
            //Begin #14147
            else if (acctTypeDetail.DepLoan == "HHD")
            {
                #region sql
                sql = string.Format(@"
select '{1}' from {0}gb_house_member h
where h.household_id    = {2}
and (

exists( select *
from {0}rm_acct a
where a.rim_no = h.rim_no
and a.confidential = '{1}' )

or exists( select *
from {0}gb_map_acct_rel g, {0}dp_acct a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = h.rim_no
and a.confidential = '{1}' )

or exists( select *
from {0}gb_map_acct_rel g, {0}ln_acct a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = h.rim_no
and a.confidential = '{1}' )

or exists( select *
from {0}gb_map_acct_rel g, {0}ex_acct a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = h.rim_no
and a.confidential = '{1}' )
    )", "{0}", GlobalVars.Instance.ML.Y, confAcctNo);
                #endregion
            }
            //End #14147
            else
            {
                if (isPrimaryDb)
                {
                    tableName = acctTypeDetail.DepLoan + "_acct";
                }
                else
                {
                    if (acctTypeDetail.DepLoan == "DP" || acctTypeDetail.DepLoan == "LN")
                        tableName = acctTypeDetail.DepLoan + "_display";
                    else
                        tableName = acctTypeDetail.DepLoan + "_acct";
                }

                #region sql
                sql = string.Format(@"
select confidential
from {0}{1}
where {2} ", "{0}", tableName,
               acctTypeDetail.DepLoan == "RM" ? string.Format(" rim_no = {0}", confAcctNo) : string.Format(" acct_type = '{0}' and acct_no =  '{1}' ", confAcctType, confAcctNo));
                #endregion
            }
            #endregion

            PString Confidential = new PString("Confidential" );
            Phoenix.Shared.BusFrame.SqlHelper sqlHelper = new Phoenix.Shared.BusFrame.SqlHelper();
            //Begin #14147
            //sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, Confidential);
            if (acctTypeDetail.DepLoan == "HHD")
            {
                sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, true, Confidential);
            }
            else
            {
                sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, Confidential);
            }
            //End #14147

            if (acctTypeDetail.DepLoan == "RM" && Confidential.Value != GlobalVars.Instance.ML.Y && ignoreRelatedAccts == false)  // Task #55416 - We dont want to check Account ignoreRelatedAccts was 'Y' because because these should only be based on Rm_Acct.Confidential 
            {
                if (isPrimaryDb) // #19415 - replaced (DataService.Instance.XmDbStatus == XmDbStatus.Day || DataService.Instance.XmDbStatus == XmDbStatus.CopyOver)
                {
                    tableName = "acct";
                }
                else
                {
                   tableName = "display";
                }

                #region sql
                sql = string.Format(@"
select 'Y'
from xp_control
where exists( select *
from {0}gb_map_acct_rel g, {0}dp_{2} a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = {1}
and a.confidential = 'Y'
{3}         /* #140789 */ )

or exists( select *
from {0}gb_map_acct_rel g, {0}ln_{2} a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = {1}
and a.confidential = 'Y'
{3}         /* #140789 */)

or exists( select *
from {0}gb_map_acct_rel g, {0}ex_acct a
where g.acct_type = a.acct_type
and g.acct_no = a.acct_no
and g.rim_no = {1}
and a.confidential = 'Y'
{3}         /* #140789 */)", "{0}", confAcctNo, tableName,
                           (primaryRimOnly ? " and g.rel_id = 1 " : null));   // #140789
                #endregion

                sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, Confidential);
            }
            return (Confidential.Value == GlobalVars.Instance.ML.Y);
        }
        //End #80630

        // Begin #01031
        public bool ValidateEnhPassword(string newPassword)
        {
            bool bBase10Digits = false;
            bool bLowerCase = false;
            bool bNonAlphaNumeric = false;
            bool bUpperCase = false;
            int nArrayIndex = 0;
            int nAsciiValue = 0;
            int nCounter = 0;
            int nMaxArrayIndex = 0;
            string[] saIndChars;
            string sPassword = null;

            if (true)
            {
                bUpperCase = false;

                bLowerCase = false;

                bBase10Digits = false;

                bNonAlphaNumeric = false;

                //ArrayHelper.ArraySetUpperBound(saIndChars, -1, -1);

                nCounter = 0;
            }

            if (true)
            {
                sPassword = newPassword;

                nArrayIndex = 0;

                if (newPassword != null)
                    saIndChars = new string[newPassword.Length + 1];
                else
                    saIndChars = new string[1];

                while (StringHelper.StrLength(sPassword) > 0)
                {
                    saIndChars[nArrayIndex] = StringHelper.StrLeftX(sPassword, 1);

                    sPassword = StringHelper.StrRightX(sPassword, StringHelper.StrLength(sPassword) - 1);

                    nArrayIndex = nArrayIndex + 1;
                }
            }

            if (true)
            {
                nMaxArrayIndex = nArrayIndex;

                nArrayIndex = 0;

                //ArrayHelper.ArrayGetUpperBound(saIndChars, 1, (nMaxArrayIndex == Decimal.MinValue ? 0 : nMaxArrayIndex));

                while (nArrayIndex <= nMaxArrayIndex)
                {
                    StringHelper.StrFirstC(ref saIndChars[nArrayIndex], ref nAsciiValue);

                    if (true)
                    {
                        if (bLowerCase == false && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 97 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 122)
                        {
                            nCounter = (nCounter == Decimal.MinValue ? 0 : nCounter) + 1;

                            bLowerCase = true;

                            if ((nCounter == Decimal.MinValue ? 0 : nCounter) > 3)
                            {
                                break;
                            }
                        }
                    }

                    if (true)
                    {
                        if (bUpperCase == false && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 65 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 90)
                        {
                            nCounter = (nCounter == Decimal.MinValue ? 0 : nCounter) + 1;

                            bUpperCase = true;

                            if ((nCounter == Decimal.MinValue ? 0 : nCounter) > 3)
                            {
                                break;
                            }
                        }
                    }

                    if (true)
                    {
                        if (bBase10Digits == false && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 48 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 57)
                        {
                            nCounter = (nCounter == Decimal.MinValue ? 0 : nCounter) + 1;

                            bBase10Digits = true;

                            if ((nCounter == Decimal.MinValue ? 0 : nCounter) > 3)
                            {
                                break;
                            }
                        }
                    }

                    if (true)
                    {
                        if (bNonAlphaNumeric == false && ((nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 32 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 47 ||
                             (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 58 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 64 ||
                             (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 91 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 96 ||
                             (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 123 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 127))
                        {
                            nCounter = (nCounter == Decimal.MinValue ? 0 : nCounter) + 1;

                            bNonAlphaNumeric = true;

                            if ((nCounter == Decimal.MinValue ? 0 : nCounter) > 3)
                            {
                                break;
                            }
                        }
                    }

                    nArrayIndex = (nArrayIndex == Decimal.MinValue ? 0 : nArrayIndex) + 1;
                }
            }

            if (true)
            {
                if ((nCounter == Decimal.MinValue ? 0 : nCounter) <= 2)
                {
                    return false;
                }

                else
                {
                    return true;
                }
            }
        }
        //End #01031

        /* Begin #78073 */
        public bool ValidateAlphaOnlyPassword(string newPassword)
        {
            int nArrayIndex = 0;
            int nAsciiValue = 0;
            string sChar = null; ;
            string sPassword = null;

            sPassword = newPassword;
            nArrayIndex = 0;

            while (nArrayIndex <= StringHelper.StrLength(newPassword) - 1)
            {
                sChar = StringHelper.StrLeftX(sPassword, 1);
                StringHelper.StrFirstC(ref sChar, ref nAsciiValue);

                if (!((nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 97 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 122) &&
                    !((nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) >= 65 && (nAsciiValue == Decimal.MinValue ? 0 : nAsciiValue) <= 90))
                {
                    return false;
                }

                nArrayIndex = (nArrayIndex == Decimal.MinValue ? 0 : nArrayIndex) + 1;
                sPassword = StringHelper.StrRightX(sPassword, StringHelper.StrLength(sPassword) - 1);
            }

            return true;
        }
        /* End #78073 */

        //Begin #76458
        public string GetMaskedExtAcct(string acctType, string acctNo)
        {
            if (acctType == null || acctType.Length == 0 || acctNo == null || acctNo.Length == 0)
                return null;
            PString AccountType = new PString("AccountType");
            PString AccountNumber = new PString("AccountNumber");
            PString MaskedAccount = new PString("MaskedAccount");
            string newMaskedAcct = acctNo;
            AccountType.Value = acctType;
            AccountNumber.Value = acctNo;
            ///if (AppInfo.Instance.IsAppOnline)
                DataService.Instance.ProcessCustomAction(this, "GetMaskedExtAcct", AccountType, AccountNumber, MaskedAccount);
            //else
            //{
            //    AdExAcctType _adExAcctType = new AdExAcctType();
            //    _adExAcctType.AcctType.Value = acctType;
            //    _adExAcctType.AcctNoMask.Selected = true;
            //    _adExAcctType.ActionType = XmActionType.Select;
            //    DataService.Instance.ProcessRequest(_adExAcctType);
            //    //
            //    if (_adExAcctType.AcctNoMask.IsNull)
            //        return null;
            //    if (_adExAcctType.AcctNoMask.Value.Trim().Length != acctNo.Length)
            //        return null;
            //    //
            //    while (newMaskedAcct.IndexOf('X') != -1)
            //    {
            //        newMaskedAcct = newMaskedAcct.Replace(newMaskedAcct.Substring(newMaskedAcct.IndexOf('X'),1), "X");
            //    }
            //    //
            //    MaskedAccount.Value = newMaskedAcct;
            //}
            if (!MaskedAccount.IsNull)
                return MaskedAccount.Value;
            else
                return acctNo;
        }

        public bool IsExternalAcct(string acctType)
        {
            if (acctType == null || acctType.Length == 0)
                return false;
            BusObjectBase adExAcctType = BusObjHelper.MakeClientObject("AD_EX_ACCT_TYPE");
            FieldBase fieldAcctType = adExAcctType.GetFieldByXmlTag("AcctType");
            if (fieldAcctType != null)
                fieldAcctType.Value = acctType;
            DataService.Instance.ProcessRequest(XmActionType.Select, adExAcctType);
            FieldBase fieldStatus = adExAcctType.GetFieldByXmlTag("Status");
            if (fieldStatus != null && fieldStatus.StringValue == GlobalVars.Instance.ML.Active)
                return true;
            return false;

            //AdExAcctType _adExAcctType = new AdExAcctType();
            //_adExAcctType.AcctType.Value = acctType;
            //_adExAcctType.Status.Value = GlobalVars.Instance.ML.Active;
            //_adExAcctType.ActionType = XmActionType.Select;
            ////  4198 - FULLY QUALIFIED REQUEST
            //DataService.Instance.ProcessRequest(XmActionType.Select, _adExAcctType);
            ////
            //if (!_adExAcctType.Status.IsNull && _adExAcctType.Status.Value == GlobalVars.Instance.ML.Active)
            //    return true;
            //return false;
        }

        public bool IsExternalAdapterAcct(string acctType)
        {
            string displaySupport = "";
            string historySupport = "";
            string adapterType = "";
            string crTcSupport = "";
            string drTcSupport = "";
            //
            return IsExternalAdapterAcct(acctType, out crTcSupport, out drTcSupport, out displaySupport, out historySupport, out adapterType);
        }


        public bool IsExternalAdapterAcct(string acctType, out string crTcSupport, out string drTcSupport)
        {
            string displaySupport = "";
            string historySupport = "";
            string adapterType = "";
            crTcSupport = "";
            drTcSupport = "";
            //
            return IsExternalAdapterAcct(acctType, out crTcSupport, out drTcSupport, out displaySupport, out historySupport, out adapterType);
        }

        //private bool SetFieldValue(BusObjectBase busObject, string xmlTag, object fieldValue)
        //{
        //    FieldBase field = busObject.GetFieldByXmlTag(xmlTag);
        //    if (field == null)
        //        return false;

        //     field.Value = fieldValue;
        //     return true;

        //}

        //private bool SelectField(BusObjectBase busObject, string xmlTag, bool isSelected)
        //{
        //    FieldBase field = busObject.GetFieldByXmlTag(xmlTag);
        //    if (field == null)
        //        return false;
        //    field.Selected = true;
        //    return true;

        //}

        public bool IsExternalAdapterAcct(string acctType, out string crTcSupport, out string drTcSupport,
            out string displaySupport, out string historySupport, out string adapterType)
        {
            crTcSupport = GlobalVars.Instance.ML.N;
            drTcSupport = GlobalVars.Instance.ML.N;
            displaySupport = GlobalVars.Instance.ML.N;
            historySupport = GlobalVars.Instance.ML.N;
            adapterType = "";

            if (acctType == null || acctType.Length == 0)
                return false;

            BusObjectBase busObject = BusObjHelper.MakeClientObject("AD_EX_ACCT_TYPE");
            FieldBase fieldAcctType = busObject.GetFieldByXmlTag("AcctType");
            FieldBase fieldStatus = busObject.GetFieldByXmlTag("Status");
            FieldBase fieldCrTcSupport = busObject.GetFieldByXmlTag("CrTcSupport");
            FieldBase fieldDrTcSupport = busObject.GetFieldByXmlTag("DrTcSupport");
            FieldBase fieldDisplaySupport = busObject.GetFieldByXmlTag("DisplaySupport");
            FieldBase fieldHistorySupport = busObject.GetFieldByXmlTag("HistorySupport");
            FieldBase fieldAdapterType = busObject.GetFieldByXmlTag("AdapterType");
            FieldBase fieldAdapter = busObject.GetFieldByXmlTag("Adapter");
            //
            fieldAcctType.Value = acctType;
            fieldStatus.Value = GlobalVars.Instance.ML.Active;
            //
            fieldCrTcSupport.Selected = true;
            fieldDrTcSupport.Selected = true;
            fieldDisplaySupport.Selected = true;
            fieldHistorySupport.Selected = true;
            fieldAdapterType.Selected = true;
            fieldAdapter.Selected = true;
            //
            busObject.ActionType = XmActionType.Select;
            busObject.SelectAllFields = false;
            //
            DataService.Instance.ProcessRequest(busObject);
            //
            if (!fieldAdapter.IsNull && fieldAdapter.StringValue == GlobalVars.Instance.ML.Y)
            {
                crTcSupport = fieldCrTcSupport.StringValue;
                drTcSupport = fieldDrTcSupport.StringValue;
                displaySupport = fieldDisplaySupport.StringValue;
                historySupport = fieldHistorySupport.StringValue;
                adapterType = fieldAdapterType.StringValue;
                return true;
            }
            else
                return false;
            //AdExAcctType _adExAcctType = new AdExAcctType();
            //_adExAcctType.AcctType.Value = acctType;
            //_adExAcctType.Status.Value = GlobalVars.Instance.ML.Active;
            //_adExAcctType.CrTcSupport.Selected = true;
            //_adExAcctType.DrTcSupport.Selected = true;
            //_adExAcctType.DisplaySupport.Selected = true;
            //_adExAcctType.HistorySupport.Selected = true;
            //_adExAcctType.AdapterType.Selected = true;
            //_adExAcctType.Adapter.Selected = true;
            //_adExAcctType.ActionType = XmActionType.Select;
            ////

            //DataService.Instance.ProcessRequest(_adExAcctType);
            ////
            //if (!_adExAcctType.Adapter.IsNull && _adExAcctType.Adapter.Value == GlobalVars.Instance.ML.Y)
            //{
            //    crTcSupport = _adExAcctType.CrTcSupport.Value;
            //    drTcSupport = _adExAcctType.DrTcSupport.Value;
            //    displaySupport = _adExAcctType.DisplaySupport.Value;
            //    historySupport = _adExAcctType.HistorySupport.Value;
            //    adapterType = _adExAcctType.AdapterType.Value;
            //    return true;
            //}
            //else
            //    return false;
        }
        //End #76458


        //Begin #01805 - rpoddar
        public bool ValidateTranCodeLimit(short tranCode, decimal tranAmt)
        {
            AdGbRsm adGbRsm = null;
            AdGbRsmTc adGbRsmTc = null;
            bool allowPosting = true;
            decimal limit = -1;

            #region fetch the data if needed
            if (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] == null)
            {
                adGbRsm = new AdGbRsm();
                adGbRsm.EmployeeId.Value = GlobalVars.EmployeeId;
                CoreService.DataService.ProcessRequest(XmActionType.Select, adGbRsm);
                GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] = adGbRsm;

            }
            adGbRsm = GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm;

            if (!CoreService.AppSetting.IsServer)   // if Client then cache the data
            {
                XmlNode _adGbRsmTcNode = null;
                if (GlobalObjects.Instance["AdGbRsmTcNode"] == null)
                {
                    adGbRsmTc = new AdGbRsmTc();
                    adGbRsmTc.EmployeeId.Value = (short)GlobalVars.EmployeeId;
                    adGbRsmTc.OutputTypeId.Value = 1;
                    _adGbRsmTcNode = DataService.Instance.GetListView(adGbRsmTc);
                    GlobalObjects.Instance["AdGbRsmTcNode"] = _adGbRsmTcNode;

                }
                if (GlobalObjects.Instance["AdGbRsmTcNode"] != null)
                {
                    _adGbRsmTcNode = GlobalObjects.Instance["AdGbRsmTcNode"] as XmlNode;
                    XmlNodeList recordsList = _adGbRsmTcNode.SelectNodes("RECORD");
                    adGbRsmTc = null;
                    foreach (XmlNode record in recordsList)
                    {
                        foreach (XmlNode childNode in record.ChildNodes)
                        {
                            if (childNode.Name == "TranCode")
                            {
                                if (childNode.InnerText == tranCode.ToString())
                                {
                                    adGbRsmTc = new AdGbRsmTc();
                                    adGbRsmTc.OnLoadNodeToObject(record, false, true);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                adGbRsmTc = new AdGbRsmTc();
                adGbRsmTc.EmployeeId.Value = (short)GlobalVars.EmployeeId;
                adGbRsmTc.TranCode.Value = tranCode;
                CoreService.DataService.ProcessRequest(XmActionType.Select, adGbRsmTc);
            }
            #endregion

            if ((tranCode == 150 || tranCode == 103) && adGbRsm != null && adGbRsm.ForcePost.Value != GlobalVars.Instance.ML.Y)
            {
                allowPosting = false;
            }
            else if (adGbRsmTc != null && adGbRsmTc.DefaultAmt.Value == GlobalVars.Instance.ML.N)
            {
                if (tranAmt > adGbRsmTc.Limit.Value)
                    allowPosting = false;
            }
            else if (adGbRsm != null)
            {
                if ((tranCode >= 100 && tranCode < 150) || (tranCode >= 200 && tranCode < 250))
                    limit = (adGbRsm.DepositCr.IsNull ? 0 : adGbRsm.DepositCr.Value);
                else if ((tranCode >= 150 && tranCode < 200) || (tranCode >= 250 && tranCode < 300))
                    limit = (adGbRsm.DepositDr.IsNull ? 0 : adGbRsm.DepositDr.Value);
                else if ((tranCode >= 2000 && tranCode < 2500) || (tranCode >= 2500 && tranCode < 3000))
                    limit = (adGbRsm.DepositDr.IsNull ? 0 : adGbRsm.DepositDr.Value);
                else if (((tranCode >= 300 && tranCode < 350) || (tranCode >= 400 && tranCode < 450)) &&
                    tranCode != 328 && tranCode != 329)
                    limit = (adGbRsm.LoanCr.IsNull ? 0 : adGbRsm.LoanCr.Value);
                else if ((tranCode >= 350 && tranCode < 400) || (tranCode >= 450 && tranCode < 500 ) || tranCode == 328 ||
                    tranCode == 329)
                    limit = (adGbRsm.LoanDr.IsNull ? 0 : adGbRsm.LoanDr.Value);
                else if ((tranCode >= 4000 && tranCode < 4500) || (tranCode >= 4500 && tranCode < 5000))
                    limit = (adGbRsm.LoanDr.IsNull ? 0 : adGbRsm.LoanDr.Value);
                /*Begin ##116113,#123757*/
                else if ((tranCode >= 500 && tranCode <= 519) || (tranCode >= 521 && tranCode < 542))
                {
                    limit = (adGbRsm.GlCr.IsNull ? 0 : adGbRsm.GlCr.Value);
                }
                else if ((tranCode >= 550 && tranCode <= 569) || (tranCode >= 571 && tranCode < 592))
                {
                    limit = (adGbRsm.GlDr.IsNull ? 0 : adGbRsm.GlDr.Value);
                }
                /*End ##116113*/
                else if ((tranCode >= 900 && tranCode < 996) || (tranCode >= 10 && tranCode < 100))
                    limit = (adGbRsm.RimDr.IsNull ? 0 : adGbRsm.RimDr.Value);
                else if (tranCode == 545 || tranCode == 548)
                    limit = (adGbRsm.RimDr.IsNull ? 0 : adGbRsm.SdCr.Value);
                else if (tranCode >= 596 && tranCode < 599)
                    limit = (adGbRsm.RimDr.IsNull ? 0 : adGbRsm.SdDr.Value);
                /*Begin ##116113*/
                else if (tranCode == 520)
                {
                    limit = (adGbRsm.ExCr.IsNull ? 0 : adGbRsm.ExCr.Value);
                }
                else if (tranCode == 570)
                {
                    limit = (adGbRsm.ExDr.IsNull ? 0 : adGbRsm.ExDr.Value);
                }
                /*End ##116113*/
                if ((limit >= 0 && tranAmt > limit))
                    allowPosting = false;
            }
            return allowPosting;

        }
        //End #01805 - rpoddar

		//Begin #79217
		public bool IsAllowExternalTfr()
		{
			AdGbRsm _adGbRsm = new AdGbRsm();
			_adGbRsm.EmployeeId.Value = BusGlobalVars.EmployeeId;
			_adGbRsm.ActionType = XmActionType.Select;
			_adGbRsm.SelectAllFields = false;
			_adGbRsm.AllowExternalTfr.Selected = true;
			CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsm);
			if (_adGbRsm.AllowExternalTfr.Value == "Y")
				return true;
			else
				return false;
		}
		//End #79217

        #region 03333
        /// <summary>
        /// Launch Interface for BISYS - EFUNDS - "CHKCNTR" , Only "BISYS "  is Implemented
        /// </summary>
        /// <param name="busObj"></param>
        /// <param name="applicationParam">BISYS - EFUNDS - "CHKCNTR" </param>
        /// <param name="rimNoParam">RimNo</param>
        /// <param name="acctTypeParam">AcctType</param>
        /// <param name="acctNoParam">AcctNo</param>
        /// <returns></returns>
        public bool LaunchInterface(BusObjectBase busObj , string applicationParam , int rimNoParam , string acctTypeParam , string acctNoParam )
        {

            //int contactHistoryId = -1 ;
            //string contactHistoryText =string.Empty;

            PString acctNo = new PString("AcctNo", acctNoParam);
            PString acctType = new PString("AcctType", acctTypeParam);
            PString bisysFile = new PString("BisysFile");
            PInt rimNo = new PInt("RimNo", rimNoParam);

            Messages.Clear();
            busObj.Messages.Clear();

            if (applicationParam != null && applicationParam.ToUpperInvariant() == "BISYS")
            {
                #region BISYS
                string tempDiectoryName = @"c:\TEMP\EFORMS";
                string newDirectoryName = string.Format("{0}\\{1}", tempDiectoryName, applicationParam);
                string bisysFileName = Path.Combine(newDirectoryName, "bisys.html");

                #region CReate Directory / File
                try
                {

                    // Create Directory
                    if (Directory.Exists(tempDiectoryName) == false)
                    {
                        Directory.CreateDirectory(tempDiectoryName);
                    }
                    if (Directory.Exists(newDirectoryName))
                    {
                        // Make sure that all files have normal attribute
                        DirectoryInfo newDirInfo = new DirectoryInfo(newDirectoryName);
                        foreach (FileInfo fileInfo in newDirInfo.GetFiles())
                        {
                            fileInfo.Attributes = FileAttributes.Normal;
                        }

                        Directory.Delete(newDirectoryName, true);

                    }
                    Directory.CreateDirectory(newDirectoryName);

                    // Create File

                    FileStream fileWriter  = File.Create(bisysFileName);
                    fileWriter.Close();
                }
                catch (Exception ex)
                {

                    CoreService.LogPublisher.LogError(ex.ToString());
                    busObj.Messages.AddError(9679, null, string.Empty);
                    return false;
                }
	            #endregion

                #region Call Server Side
                CoreService.DataService.ProcessCustomAction(this, "LaunchInterface", acctType, acctNo, rimNo, bisysFile);
                if (Messages.Count > 0)
                {
                    busObj.Messages.AddRange(this.Messages);
                    return false;
                }
                #endregion

                #region WriteFile
                StreamWriter writer = File.AppendText(bisysFileName);
                writer.Write(bisysFile.Value);
                writer.Close();
                #endregion

                // launch file
                try
                {
                    System.Diagnostics.Process.Start(bisysFileName);
                }
                catch(Exception ex)
                {
                    CoreService.LogPublisher.LogError(ex.Message);
                }

                #endregion


            }
            #region Log Customer Contact
            //  contactHistoryId = 15;
            //contactHistoryText = "IRA Services Access";
           // this part has been moved to the calling screen it self as it causes a circular dependency.

            //if (contactHistoryId == -1)
            //    return true;

            //RIM.RmContactHist rmContactHist = new Phoenix.BusObj.RIM.RmContactHist();
            //if (rmContactHist.LogContactHistoryRequired(false))
            //{
            //    rmContactHist.RimNo.Value = rimNo.Value;
            //    rmContactHist.ContactType.Value = contactHistoryId;
            //    rmContactHist.ContactDt.Value = DateTime.Now;
            //    rmContactHist.ContactInfo.Value = contactHistoryText;
            //    rmContactHist.ActionType = XmActionType.New;
            //    rmContactHist.EmployeeId.Value = Convert.ToInt16(GlobalVars.EmployeeId);
            //    rmContactHist.CreateDt.Value = GlobalVars.SystemDate.Date;

            //     CoreService.DataService.ProcessRequest(rmContactHist);

            //     busObj.Messages.AddRange(rmContactHist.Messages);
            //}


            #endregion
            return busObj.Messages.Count == 0;
        }

        #endregion

        #region 9177 - IntCalcAge
        public string IntCalcAge(DateTime birthDate, DateTime sysDate)
        {
            #region custom action
            PDateTime dt1 = new PDateTime("A0");
            PDateTime dt2 = new PDateTime("A1");
            PString CalcAge = new PString("A2");

            this.ActionType = XmActionType.Custom;

            dt1.Value = birthDate;
            dt2.Value = sysDate;

            DataService.Instance.ProcessCustomAction(this, "CalculateAge", dt1, dt2, CalcAge);

            if (CalcAge.IsNull)
                return string.Empty;
            else
                return CalcAge.StringValue;
        }
        #endregion
        #endregion

        /*Begin #80637*/
        public string DecodeNSF(Int32 _pnCode)
        {
            /*Shared.Constants.ScreenId.TCPost = 353*/
            switch (_pnCode)
            {
                /*posted NSF*/
                case 1:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 12);
                /*Suspect*/
                case 2:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 13);
                /*Posted NSF/Suspect*/
                case 3:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 14);
                /*Uncollected funds*/
                case 5:
                    /*1620 - This transaction successfully posted, however, it has drawn on uncollected funds*/
                    return CoreService.Translation.GetUserMessageX(1620);
                /*Uncollected funds / Suspect*/
                case 7:
                    /*3684 - This transaction successfully posted, however, it has drawn on uncollected  funds,  and the check posted in this
                     * transaction has been marked "Suspect" due to a Stop Payment on this account.*/
                    return CoreService.Translation.GetUserMessageX(3684);
                /*Caution*/
                case 11:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 60);
                /*Caution/Posted NSF*/
                case 12:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 55);
                /*Caution/Suspect*/
                case 13:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 56);
                /*Caution/Posted NSF/Suspect*/
                case 14:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 57);
                /*Caution/Uncollected Funds*/
                case 16:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 58);
                /*Caution/Uncollected Funds/Suspect*/
                case 18:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 59);
                /*Posted Limit  (OD Limit Processing 21, 23, 32, 34)*/
                case 21:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 81);
                /*Posted Suspect/Limit*/
                case 23:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 82);
                /*Posted Caution/Limit*/
                case 32:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 83);
                /*Posted Suspect/Caution/Limit*/
                case 34:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 84);
                /*Posted Charge-off*/
                case 35:
                    return CoreService.Translation.GetTranslateX(Shared.Constants.ScreenId.TCPost, 78);
                default:
                    return string.Empty;
            }
        }
        /*End #80637*/

        /* Begin 80630 */

        public bool AllowEmployeeToRemoveConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                /* Begin 14330 */
                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }
                /* End 14330 */

                return (_adGbRsmBusObj.ConfAcctRemove.IsNull ? false : _adGbRsmBusObj.ConfAcctRemove.Value.Equals("Y"));
            }
        }

        public bool ConfidentialSettingsAreEnabled
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                /* Begin 14330 */
                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }
                /* End 14330 */

                /* Begin 80630 */
                AdGbBankControl _adGbBankControlBusObj = (AdGbBankControl)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbBankControl];
                /* End 80630 */
                return (_adGbBankControlBusObj.ConfAcctSecurity.IsNull ? false : _adGbBankControlBusObj.ConfAcctSecurity.Value.Equals("Y"));
            }
        }

        /* End 80630 */

        #region #80660
        public bool SuspectSettingsAreEnabled
        {
            get
            {
                /* Begin 80630 */
                AdGbBankControl _adGbBankControlBusObj = (AdGbBankControl)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbBankControl];
                /* End 80630 */
                return (_adGbBankControlBusObj.EnableSuspectAlert.IsNull ? false : _adGbBankControlBusObj.EnableSuspectAlert.Value.Equals("Y"));
            }
        }
        #endregion

        /* Begin 14340 */
        public bool AllowEmployeeToIgnoreConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }

                return (_adGbRsmBusObj.ConfAcctIgnore.IsNull ? false : _adGbRsmBusObj.ConfAcctIgnore.Value.Equals("Y"));
            }
        }

        public bool AllowEmployeeToViewConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }

                return (_adGbRsmBusObj.ConfAcctView.IsNull ? false : _adGbRsmBusObj.ConfAcctView.Value.Equals("Y"));
            }
        }

        public bool AllowEmployeeToItemRepairConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }

                return (_adGbRsmBusObj.ConfAcctItemRepair.IsNull ? false : _adGbRsmBusObj.ConfAcctItemRepair.Value.Equals("Y"));
            }
        }

        public bool AllowEmployeeToPostConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }

                return (_adGbRsmBusObj.ConfAcctPost.IsNull ? false : _adGbRsmBusObj.ConfAcctPost.Value.Equals("Y"));
            }
        }

        public bool AllowEmployeeToEditConfidentialFlag
        {
            get
            {
                //2010-Convert-moved here
                AdGbRsm _adGbRsmBusObj = (AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[GlobalObjectNames.AdGbRsm];

                if (_adGbRsmBusObj == null)
                {
                    _adGbRsmBusObj = new AdGbRsm();
                    _adGbRsmBusObj.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
                    Phoenix.FrameWork.Core.CoreService.DataService.ProcessRequest(XmActionType.Select, _adGbRsmBusObj);
                }

                return (_adGbRsmBusObj.ConfAcctEdit.IsNull ? false : _adGbRsmBusObj.ConfAcctEdit.Value.Equals("Y"));
            }
        }
        /* End 14340 */

        //Begin #142715
        /// <summary>
        /// Validates whether the email address is in proper format or not.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="messages"></param>
        /// <returns></returns>
        public bool ValidateEmailAddressFormat(string email, PMessageCollection messages)
        {
            if (!string.IsNullOrEmpty(email))
            {
                if (email.IndexOf("@") <= 0 ||
                    email.IndexOf(".") <= 0 ||
                    email.IndexOf("..") >= 0 ||
                    email.EndsWith("."))
                {
                    if (messages != null)
                    {
                        // 14058 - Please enter a valid Primary Email Address
                        messages.AddError(14058, null, string.Empty);
                    }
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates whether the passed NAICS code is a valid one or not. 
        /// </summary>
        /// <param name="naicsCode"></param>
        /// <param name="messages"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool ValidateNaicsCode(string naicsCode, PMessageCollection messages, ref string description)
        {
            if (!string.IsNullOrEmpty(naicsCode))
            {
                SqlHelper sqlHelper = new SqlHelper();
                PString naicsCodeDesc = new PString("naicsCodeDesc");

                #region sql
                string sql = string.Format(@"select DESCRIPTION from {0}PC_NAICS_CODE where NAICS_CODE = '{1}'", "{0}", naicsCode);
                #endregion

                if (!sqlHelper.ExecSqlImmediateInto(CoreService.DbHelper, sql, true, naicsCodeDesc))
                {
                    if (messages != null)
                    {
                        // 14059 - The NAICS code you entered does not exist.  Please enter a valid code or select the NAICS button to locate the code.
                        messages.AddError(14059, null, string.Empty);
                    }

                    return false;
                }

                description = naicsCodeDesc.Value;
            }

            return true;
        }

        //End #142715

        #region Country Codes
        /* Begin #32310 */
        public string GetUSCountryCode()
        {
            // Returns a valid US country code (has iso code = US in ad_gb_country)
            // Returns USA, if no active country codes
            PString CountryCd = new PString("CountryCd");
            CoreService.DataService.ProcessCustomAction(this, "GetUSCountryCode", CountryCd);
            return CountryCd.Value.ToString();
        }

        public bool IsUSCountryCode(string countryCode)
        {
            // Returns true if the given country code has iso code = US in ad_gb_country
            PString CountryCd = new PString("CountryCd");
            PBoolean IsUsCode = new PBoolean("IsUsCode");
            CountryCd.Value = countryCode;
            CoreService.DataService.ProcessCustomAction(this, "IsUSCountryCode", CountryCd, IsUsCode);
            return IsUsCode.Value;
        }
        /* End #32310 */
        #endregion

        #region 173485
        /// <summary>
        ///  Added to get the enviornment type from the server.config . Production or Staging   
        /// </summary>
        /// <returns></returns>
        public bool IsStagingEnvironment()
        {
            string tagName = "EnvironmentType";
            if (CoreService.AppSetting.OtherInfo != null && CoreService.AppSetting.OtherInfo.Contains(tagName))
            {
                string value = CoreService.AppSetting.OtherInfo[tagName].Value;
                if (value != null)
                    value = value.ToUpper();
                return value == "STAGING";
            }
            return false;
        }


        #endregion

        /*Begin #65549*/
        public string GetDecryptedPan(string panId)
        {
            PString sPanId = new PString("sPanId");
            PString sPan = new PString("sPan");
            sPanId.Value = panId;
            CoreService.DataService.ProcessCustomAction(this, "GetDecryptedPan", sPanId, sPan);
            return sPan.Value;
        }

        /*End #65549*/

        /*Begin #71500*/
        public string GetDecryptedAcctNo(string acctId)
        {
            PString sAcctId = new PString("sAcctId");
            PString sAcctNo = new PString("sAcctNo");
            sAcctId.Value = acctId;
            CoreService.DataService.ProcessCustomAction(this, "GetDecryptedAcctNo", sAcctId, sAcctNo);
            return sAcctNo.Value;
        }

        /*End #71500*/

    } //End of Class

    #region SetSearchType
    public enum SetSearchType
	{
		/// <summary>
		/// Search By Name
		/// </summary>
		Name,
		/// <summary>
		/// Please provide acct type and Acct No
		/// </summary>
		Account,
		/// <summary>
		/// Provide TIN Type and Tin Number
		/// </summary>
		Tin,
		/// <summary>
		/// Provide Phone Number
		/// </summary>
		Phone
	}
	#endregion SetSearchType

	#region SetSubSearchType
	public enum SetSubSearchType
	{
		/// <summary>
		/// SubSearch within  account type of search
		/// </summary>
		AcctType,
		/// <summary>
		/// SubSearch within acct type
		/// </summary>
		ApplType,
		SSN,
		TIN,
		CustomTin
	}
	#endregion SetSearchType

	public class AcctTypeDetail
	{
		#region private vars
		public string _acctType;
		public string _applType;
		public string _depLoan;
		public string _acctNoFormat;
		#endregion

		#region public prop
		public string AcctType
		{
			get{return _acctType;}
		}

		public string ApplType
		{
			get{return _applType;}
		}

		public string DepLoan
		{
			get{return _depLoan;}
		}

		public string AcctNoFormat
		{
			get{return _acctNoFormat;}
		}
		#endregion

		internal AcctTypeDetail( string detailString )
		{
			string[] acctDetails = null;

			acctDetails  = detailString.Split("~".ToCharArray());

			_acctType = acctDetails[0];
			_applType = acctDetails[1];
			_depLoan = acctDetails[2];
			_acctNoFormat = acctDetails[3];
		}

        public AcctTypeDetail(string acctType, string applType, string depLoan, string acctNoFormat)
        {
            _acctType = acctType;
            _applType = applType;
            _depLoan = depLoan;
            _acctNoFormat = acctNoFormat;
        }
	}
	
    public class AcctAlerts
    {
        public bool Caution = false;
        public bool Hold = false;
        public bool NSF = false;
        public bool REJ = false;
        public bool UCF = false;
        public bool Stop = false;
        public bool Retirement = false;
        public bool Sweep = false;
        public bool Analysis = false;
        public bool Signer = false;
        public bool HouseHold = false;
        public bool CrossRef = false; /*#76429*/
        public bool Fraud = false;  //#80660
        //Begin #80679-2
        public string PlanNo = string.Empty;
        public decimal SweepControlPtid = 0;
        public bool CustCrossRef = false;
        public int CustNotesCount = 0;
        public int AcctNotesCount = 0;
        public int ApplNotesCount = 0;
        public string DepLoan = string.Empty;
        //End #80679-2
        public bool AcctRegD = false;      //140780
        public bool CustRegD = false;      //140780
        public bool AcctBankrupt = false;      //140775
        public bool CustBankrupt = false;      //140775
        public bool CustRelPkg = false;      //140769
        //Begin #19415
        public string AcctType = null;
        public string AcctNo = null;
        public int RimNo = -1;
		public bool AdvRestrict = false; // 140796

        //Begin Bug #95593
        public int CurAnalId = 0;
        public int AnalysisRimNo = 0;
        public int SweepPtid = 0;
        //End Bug #95593

        public string ConvertToString()
        {
            try
            {
                long alerts = 0;

                if (CustCrossRef)
                    alerts |= UserConstants.ALERT_Cust_XRef;

                if (Fraud)
                    alerts |= UserConstants.ALERT_Cust_Fraud;

                if (CustNotesCount == 1)
                    alerts |= UserConstants.ALERT_Cust_Notes;

                else if (CustNotesCount > 1)
                    alerts |= UserConstants.ALERT_Cust_NotesPlus;

                if (HouseHold)
                    alerts |= UserConstants.ALERT_Cust_House;

                if (NSF)
                    alerts |= UserConstants.ALERT_Acct_Nsf;
                if (UCF)
                    alerts |= UserConstants.ALERT_Acct_Ucf;
                if (REJ)
                    alerts |= UserConstants.ALERT_Acct_Reject;
                if (Stop)
                    alerts |= UserConstants.ALERT_Acct_Stop;
                if (Hold)
                    alerts |= UserConstants.ALERT_Acct_Hold;
                if (Caution)
                    alerts |= UserConstants.ALERT_Acct_Caution;
                if (!string.IsNullOrEmpty(PlanNo))
                    alerts |= UserConstants.ALERT_Acct_Umb;
                if (Analysis)
                    alerts |= UserConstants.ALERT_Acct_Analysis;
                if (CrossRef)
                    alerts |= UserConstants.ALERT_Acct_XRef;
                if (Sweep)
                    alerts |= UserConstants.ALERT_Acct_Sweep;

                if (AcctRegD)
                    alerts |= UserConstants.ALERT_Acct_RegD;
                if (CustRegD)
                    alerts |= UserConstants.ALERT_Cust_RegD;
                if (AcctBankrupt)
                    alerts |= UserConstants.ALERT_Acct_Bankrupt;
                if (CustBankrupt)
                    alerts |= UserConstants.ALERT_Cust_Bankrupt;
                if (CustRelPkg)
                    alerts |= UserConstants.ALERT_Cust_RelPkg;

                if (AcctNotesCount == 1)
                    alerts |= UserConstants.ALERT_Acct_Notes;
                else if (AcctNotesCount > 1)
                    alerts |= UserConstants.ALERT_Acct_NotesPlus;

                if (ApplNotesCount > 0)
                    alerts |= UserConstants.ALERT_Notes;

                return (string.IsNullOrEmpty(AcctType) ? "NULL" : AcctType) + '~' +
                  (string.IsNullOrEmpty(AcctNo) ? "NULL" : AcctNo) + '~' +
                  (string.IsNullOrEmpty(DepLoan) ? "NULL" : DepLoan) + '~' +
                  Convert.ToString(RimNo) + '~' +
                  (string.IsNullOrEmpty(PlanNo) ? "0" : PlanNo) + '~' +
                  (SweepControlPtid == decimal.MinValue ? "-1" : Convert.ToString(SweepControlPtid)) + '~' +
                  Convert.ToString(CustNotesCount) + '~' +
                  Convert.ToString(AcctNotesCount) + '~' +
                  Convert.ToString(ApplNotesCount) + '~' +
                  Convert.ToString(alerts);
            }
            catch (Exception e)
            {
                CoreService.LogPublisher.LogError(e.ToString());
                return null;
            }

        }
        //Begin Task 89995
        public bool HasAlerts()
        {
            bool[] alerts = new bool[] { CustCrossRef, Fraud, HouseHold, NSF, UCF, REJ, Stop, Hold, Caution,
                                    Analysis, CrossRef, Sweep, AcctRegD, CustRegD, AcctBankrupt, CustBankrupt, CustRelPkg };

            for (int i = 0; i < alerts.Length; i++)
            {
                if (alerts[i])
                {
                    return true;
                }
            }
            return false;
        }
        //End Task 89995
        public bool LoadFromString(string alertInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(alertInfo))
                    return false;

                string[] alertInfoDetails = null;
                alertInfoDetails = alertInfo.Split("~".ToCharArray());

                if (alertInfoDetails.Length < 10)
                    return false;

                if (alertInfoDetails[0] != "NULL")
                    AcctType = alertInfoDetails[0];

                if (alertInfoDetails[0] != "NULL")
                    AcctNo = alertInfoDetails[1];

                if (alertInfoDetails[0] != "NULL")
                    DepLoan = alertInfoDetails[2];

                RimNo = Convert.ToInt32(alertInfoDetails[3]);

                if (alertInfoDetails[1] != "0")
                    PlanNo = alertInfoDetails[4];

                if (alertInfoDetails[2] != "-1")
                    SweepControlPtid = Convert.ToDecimal(alertInfoDetails[5]);

                CustNotesCount = Convert.ToInt32(alertInfoDetails[6]);
                AcctNotesCount = Convert.ToInt32(alertInfoDetails[7]);
                ApplNotesCount = Convert.ToInt32(alertInfoDetails[8]);

                long alerts = Convert.ToInt64(alertInfoDetails[9]);

                CustCrossRef = (alerts & UserConstants.ALERT_Cust_XRef) == UserConstants.ALERT_Cust_XRef;
                Fraud = (alerts & UserConstants.ALERT_Cust_Fraud) == UserConstants.ALERT_Cust_Fraud;
                HouseHold = (alerts & UserConstants.ALERT_Cust_House) == UserConstants.ALERT_Cust_House;
                NSF = (alerts & UserConstants.ALERT_Acct_Nsf) == UserConstants.ALERT_Acct_Nsf;
                UCF = (alerts & UserConstants.ALERT_Acct_Ucf) == UserConstants.ALERT_Acct_Ucf;
                REJ = (alerts & UserConstants.ALERT_Acct_Reject) == UserConstants.ALERT_Acct_Reject;
                Stop = (alerts & UserConstants.ALERT_Acct_Stop) == UserConstants.ALERT_Acct_Stop;
                Hold = (alerts & UserConstants.ALERT_Acct_Hold) == UserConstants.ALERT_Acct_Hold;
                Caution = (alerts & UserConstants.ALERT_Acct_Caution) == UserConstants.ALERT_Acct_Caution;
                Analysis = (alerts & UserConstants.ALERT_Acct_Analysis) == UserConstants.ALERT_Acct_Analysis;
                CrossRef = (alerts & UserConstants.ALERT_Acct_XRef) == UserConstants.ALERT_Acct_XRef;
                Sweep = (alerts & UserConstants.ALERT_Acct_Sweep) == UserConstants.ALERT_Acct_Sweep;

                AcctRegD = (alerts & UserConstants.ALERT_Acct_RegD) == UserConstants.ALERT_Acct_RegD;
                CustRegD = (alerts & UserConstants.ALERT_Cust_RegD) == UserConstants.ALERT_Cust_RegD;
                AcctBankrupt = (alerts & UserConstants.ALERT_Acct_Bankrupt) == UserConstants.ALERT_Acct_Bankrupt;
                CustBankrupt = (alerts & UserConstants.ALERT_Cust_Bankrupt) == UserConstants.ALERT_Cust_Bankrupt;
                CustRelPkg = (alerts & UserConstants.ALERT_Cust_RelPkg) == UserConstants.ALERT_Cust_RelPkg;

                return true;
            }
            catch (Exception e)
            {
                CoreService.LogPublisher.LogError(e.ToString());
                return false;
            }

        }
        //End #19415

    }
}
