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
// File Name: frmTlJournal.cs
// NameSpace: Phoenix.Client.Journal
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//5/24/2006		2		vreddy		Added TLO100 report
//06/04/2006    3       Vreddy		Now Rundate is Client Datetime Stamp
//08/11/2006	4		mselvaga	#67880 - Offline storing changes added.
//10/31/2006    5       Vreddy		#67883 - Offline TL Journal Report
//12/07/2006	6		mselvaga	#71010 - Teller journal fixed No data available display text for repopulate.
//12/07/2006	7		mselvaga	#69257 - Added additional param to support sup. reversal.
//12/14/2006	8		mselvaga	#71068 - Fixed EmId wosa short code for incorrect value.
//12/17/2006	9		Vreddy		#70253 - Offline printing of report crash if there are no records in table window.  This is happening because selected Record Count is coming as null
//01/09/2007	10		Vreddy		#70185 - Accoun tdisplay is not disabling because of temp and order of calls
//01/11/2007	11		Vreddy		#71363 - Journal Filters
//01/17/2007	12		mselvaga	#71436 - Added fix for entered date with from and/or to time search.
//01/22/2007	13		mselvaga	#71435 - Added custom grid message text for teller journal fetch.
//01/23/2007	14		Vreddy		#71440- Browser object is kiiling the form so I am moving the creation back to Printing
//02/01/2007	15		mselvaga	#71668 - Reference_acct_type and reference_acct_no are not printing for Coin/Currency TC 928.
//02/07/2007	16		rpoddar		#71734 - Skip tabe population by framework.
//02/15/2007	17		rpoddar		#71853 - Trim the Acct Type as for GL casuing problem
//02/20/2007	18		rpoddar		#71873 - Commented the closing of XfsPrinter.
//05/04/2007	19		mselvaga	#71644 - Teller 2007 - Trying to do an Account display in teller for Commitments - receive Phoenix Error #1 - Value was either too small or too large for a decimal.
//05/14/2007	20		Vreddy 		#72827 - Unable to print Online Reports from Teller 2007
//								 			1. ShowUrlPdf is called to print the Report
//May-24-07		21   	Muthu       #72780 VS2005 Migration
//06/04/07      22      Vdevadoss   #71329 - Added the validation for the enhancement.
//06/12/2007    23      mselvaga    #73098 - .Net Teller Issue Printing Cashier Checks - Tran posted in OLD Teller - Trying to Reprint in New Teller.
//06/19/2007    24      Vreddy      #73125 - Moving the Resetting the Printer Layout orientation to on disposing and commented the code at Report locations...
//07/24/2007    25      mselvaga    #73098 - Fixed LoadAndReprint for null validation.
//08/24/2007    26      bbedi       #72916 - Add TCD Support in Teller 2007 ye
//08/24/2007    25      Deiland     #72916 - Add TCD Support for TLF100 Offline Report
//08/31/2007    26      Deiland     #72916 - Remove spaces around /'s in for phx_d_TCDDrPositin replacement on TLF100
//09/06/2007    27      BBedi       #72916 - Add TCD Support for Teller 2007ye, Phase 2.
//09/19/2007    28      BBedi       #72916 - Issue Fix
//01/25/2008    29      JStainth    #74014 - Reactivate service when reversing trancode
//02/13/2008    30      LSimpson    #74269 - Added xp_ptid to grid and CurTran.XpPtid.
//02/26/2008    31      mselvaga    #74772 - Icons are not refreshing when selecting different accounts in Teller Journal.
//02/27/2008    32      LSimpson    #74269 - Modified reprint to handle 3 float lines per form.
//03/05/2008    33      mselvaga    #74662 - Reversal message of multiple transactions is only displaying amount for 1st transaction.
//04/17/2008    34      LSimpson    #74269 - Cosmetic changes for float.
//07/09/2008    35      DEiland     SDR#72248 - Want to pull Reference Acct_No and Acct_Type as seperate fields for new print codes added
//09/18/2008    36      LSimpson    #76057 - Modifications for real time recon.
//09/24/2008    37      iezikeanyi  #76429 - Added two new grid columns - IncomingAcctNo and IncomingTfrAcctNo
//11/20/2008    38      mselvaga    #76057 - Added more check printing fixes.
//12/16/2008    39      mselvaga    #1841 - Added code to block check printing when admin flag is off.
//12/17/2008    40      mselvaga    #1871 - Added Fix for previous day transaction reprint issue.
//01/27/2009    41      mselvaga    #76458 - Added support for EX account.
//02/13/2009    42      LSimpson    #76409 - Teller check proof modifications.
//03/26/2009    43      mselvaga    #76052 - Added dummy column for NSF CC pending.
//03/31/2009    44      mselvaga    #76033 - Added hyland onbase integration changes.
//04/14/2009    45      mselvaga    #76033 - Fixed reversal void key problem.
//04/24/2009    46      mramalin    WI-3475 - Terminal Services Printing Enhancement
//05/02/2009    47      LSimpson    #3590  - Commented default 'All' proof status.
//05/30/2009    48      JStainth    #76047 - reversal button should only be enabled when bond purchase status = 'closed'
//05/03/2009    49      mslevaga    WI-3548 - Modified check info reprint default.
//05/29/2009    50      bhuhges     4222 - Disabled reversal button if adapter transfer.
//06/05/2009    51      iezikeanyi  #4450 - Hide Cross Ref Accounts when Cross Ref is not enabled
//06/09/2009    52      mselvaga    WI-4542 - Handle reprint problem using the print source property.
//06/22/2009    53      rpoddar     #04747 - BondPurchaseError in offline mode
//06/24/2009    54      mselvaga    WI#1157 - Added warning message for loan fee charge.
//09/17/2009    55      vsharma     #5851,5863 - Removed security from calculator display screen
// 05Nov2009	56		GDiNatale	#6615 - SetValue Framework change
//10/23/2009	57		njoshi		#5977/5978 If the user checks any of the check boxes in the search criteria and reverses a transaction the grid does not get refreshed. So unchecking all the cb after a succesful reversal.
//12/23/2009    58      LSimpson    #79569 - Local date/time zone modifications.
//03/09/2010    59      LSimpson    #8137 - Uncheck cbLocalDateTime when reversing so datagrid is refreshed and validation error is not presented.
// 15Apr2010	60		GDiNatale	#8560 - Need to pass TranCode when initing frmCheckInfo's parameters
//04/28/2010    60      mselvaga    Enh#79574 - Added cash recycler changes.
//02/26/2010    61      mramalin    #79510 - Shared Branch
//04/19/2010    62      rpoddar     #79510 - Shared Branch
//05/14/2010    63      LSimpson    #8908 - Do not show cash message for non cash drawer.
//05/27/2010    64      rpoddar     #09143 - Shared Branch
// 12May2010	65		GDiNatale	#79621
//06/09/2010    66      sdhamija	#9311, CR#8075
//06/11/2010    67      sdhamija	#9311 - avoid restriction check for non cust.
//06/24/2010    68      rpoddar 	#79510, #09368 - Item endoresment change
// 30Jun2010	69		Dfutcher	#9591
//06/30/2010    70      rpoddar     #79510, #09368 - Make the journal display window work in 24 x 7 mode.
//07/30/2010    71      mselvaga    WI#10036 - PW #361075 - new message text for TCR users.
//08/13/2010    72      mselvaga    WI#9470 - UAT - Reversals for TCR Deposits and TCD/TCR Dispenses.
//10/05/2010    73      mselvaga    WI#10787 - 10848 - Reversal is wrong when TCR cash in and TCR cash out with coin dispense is in same tran.
//10/05/2010    74      mselvaga    WI#10702 - 10687 - Added TCR reversal fixes.
//10/05/2010    75      rpoddar     #79510, #10883 - Fix for empty error message 360188
//10/25/2010    76      rpoddar     #79510, #10883-2 - Populate acct # for shared branch during reprint
//11/05/2010    77      mselvaga    WI#79314 - Added Remote Override Teller enh changes.
//01/26/2011    78      mselvaga    WI#11784 - Reverse a TCR transaction while connected to a different TCR.
//01/27/2011    79      mselvaga    WI#12215 - When a cashier's check is issued and needs to be reprinted, receiving Nexus error.  Check is a Phoenix SQR Check.
//11/19/2010    78      LSimpson    #80615 - MT Voucher Printing.
//02/10/2011    79      mselvaga    #12769 - DUT 79314 - Supervisor View - When the “Journal” button is selected the system is not going to the Teller Journal on the Supervisor Overrideable Errors Window (16001 - dlgTlSupervisorOverride).
//02/14/2011    80      LSimpson    #12215 - Revert changes related to passing true form id to dlgAdHocReceipt.  This will uphold changes made for sqr printing.
//02/18/2011    81      mselvaga    WI#12836 - Need to add a Phoenix Warning message to a TCR reversal for the Cash In.
//02/14/2011    82      LSimpson    CR#12881 - Suppress Balances.
//02/16/2011    83      LSimpson    CR#12870 - Disable pbPrintMailer for noncust.
//03/10/2011    84      SDighe      WI#12941 - No error message displays advising that the TCD has to be manually reversed
//03/15/2011    85      LSimpson    #79502 - Enhanced Voucher Printing.
//05/16/2011    86      DEIland     WI#13545/#13546 - Remove code stopping Transfers to Adapters from getting Reversed in TL_JOURNAL.  This should have been done for the Frontend Window and not Teller.  Teller is where they need ro reverse it
//05/20/2011    87      LSimpson    #13655 - Corrected offline issues.
//06/28/2011    88      LSimpson    #13758 - Added population of Tfr account info.
//07/12/2011    89      LSimpson    #14708 - Print 'Unavailable' for balances when offline.
//07/13/2011    89      SDighe      WI#13877 - Teller unable to close out drawer - Checks Must Be Proofed message appearing even though all checks have been proofed
//07/21/2011    90      njoshi      wi 14367 - Reversing a transaction with cash dispensed/coin not dispensed causes a drawer offage.
//09/07/2011    91      sdhamija	WI#15431 - group boxes were overlapping, UI only changes.
//11/25/2011    92      LSimpson    #80660 - Suspicious Transaction Scoring and Alerts modifications.
//10/18/2011    92      mselvaga    WI#15117 - UAT 2 79314-Not all overrides are showing in Override Decision window.
//10/19/2011    93      SDighe      WI#15138 - TCR - Batch TCD/TCR Cash Out ticket prints null on a Reprint.
//01/09/2012	94		NJoshi		wi#16279 - Reprint Issues
//03/15/2012    95      LSimpson    #17166 - If print balance prompt not used, post access only for balances was not being considered.
//03/29/2012	96		NJoshi		#16279 - further changes.
//05/29/2012    97      LSimpson    #17818 - Added confidential balance printing check for AllowEmployeeToViewConfidentialFlag and AllowEmployeeToIgnoreConfidentialFlag.
//08/08/2012	98		NJoshi		wi#18900 - Proofing and Batching here does not refresh the tl journal grid.
//10/10/2012    99      LSimpson    #140784-2 - Added parameter within ReverseVoucher to ParameterService for IsSharedBranch.
//12/20/2012    100     mselvaga    #140772 - WI#20589 - Merged up Inventory Tracking - Phase II changes into the latest.
//01/18/2013    101     njoshi      #19703 - When you go into the "Reprint" option for a shared branch transaction the balance is showing.
//02/04/2013    102      mselvaga    #20928 - 3212 - frmGbInventoryTfrAdd - NewTfr From Location = Branch where Drawer _no= 0-NOT WORKING.
//03/01/2013    102     mselvaga    #21274 - Fixed Inventory search parameter values.
//03/04/2013    103     mselvaga    WI#21401 - 10443 - frmTlJournal - change cursor focus.
//8/3/2013		104		apitava		#157637	Uses new xfsprinter
//07/16/2013    105     mselvaga    #140798 - Savings Bond Redemption History Desc changes added.
//12/04/2013	106		JRhyne		WI#26012 - fix mt printing locks
//04/11/2014    107     RDeepthi    WI#27776. Disabled push button when there is no row in the grid
//01/13/2014    108     spatterson  #140895 - Teller Capture Integration changes.
//04/03/2014    109     mselvaga    #140895 - Teller Capture - Log On Without Drawer - Teller Journal (Supervisor) - Teller Capture columns missing.
//08/14/2014    110     mselvaga    #30622 - We are missing Teller Capture Workstation and Employee columns to be visible in Teller Journal 2014.
//10/09/2014    111     mselvaga    #30969 - #140895 - AVTC Part I changes added.
//01/13/2015    112     mselvaga    #34100 - Teller Capture 2014 Standardization - "Image"- push button enabled from Teller Journal after Closeout causes appl error.
//05/18/2015    113     MKrishna    #175838 - Optimize to prevent OPS latency.
//08/06/2015    114     mselvaga    #38309 - REL 2015 - Reprinting Teller Receipts from the Journal is returning Application error.
//09/28/2015    115     rpoddar     #195669, #35513 - Automate EOB Changes
//11/06/2015    116     BSchlottman #194535 - eReceipts
//12/16/2015    117     BSchlottman #40196 If the user select Print, clear out the email address.
//12/21/2015    118     BSchlottman #40276 Change the receipt information to remember changes and compact changes to minimize auditing.
//01/08/2016    119     BSchlottman #40787 Remove compact changes
//02/01/2016    120     BSchlottman #41316 Code review changes
//                                  #41344 RIM set to Email is printing on Reprint from Journal
//03/11/2016    121     rpoddar     #40977 - AVTC OOB Fixes
//03/18/2016    122     BSchlottman #42555 Set the rim in account details for secondary owners.
//04/06/2016    123     mselvaga    #43402 - Brentwood - When AVTC/TCR involved the routing number on the virtual Cash In ticket(s) for TCR is incorrect and is causing the teller drawer to be OOB.
//03/14/2016    121     Kiran       WI#41426 - loan set up as Statement Payment Method has a tc345 (loan payoff) posted and then reversed; the Statement is not getting re-activated - Added warning Message
//06/01/2016    122     BSchlottman #42567 and #43143 Reg CC changes
//06/17/2016    123     BSchlottman #43143 Reg CC MT voucher changes 
//10/31/2016    124     mselvaga    #54638 - Error when GL TC has a MT form attached 
//11/29/2016    125     AshishBabu  #55116 Fixed reprinting Error for Check Printing and Reciept Printing if Nexus is not Configured
//01/19/2017    126     AshishBabu  #55116 Revoked changes made on #55116
//05/15/2017    127     Shebin      Bug #65025 - Fix the issue with WI#41426 - while reversing PFR,we got Input string was not in a correct format error. 
//05/30/2017    128     AshishBabu  #60494 Fixed reprinting Error for Check Printing and Reciept Printing if Nexus is not Configured
//07/24/2017    129     AshishBabu  #68874 Fixed reprint forms from Teller for TC 902/910
//08/10/2017    129     mselvaga    #69369 - 196262 - 37 - EC-Trancode 301 Posting in Teller.
//11/2/2017     130      RDeepthi    WI#75604. Teller Window. So always refer Decimal Config
//05/24/2018    131     AshishBabu  Task#90213 Fixing the Nexus Error while reprinting debit item form from Tljournal.
//07/09/2018	132		RBhavsar	#95425 - Bank IOWA Transporter flag issue.
//02/14/2019    133     AshishBabu  Task#110967 Fixed Application error on Teller summary position window
//05/26/2019    134     mselvaga    Task#111431 - Enable client workstation property changes for Citrix - JIRA 376
//08/29/2019    135     mselvaga    Task#118298 - Added View Receipt action button. The action click functionality will be added later.
//09/20/2019    136     FOyebola    Task#119501 - 232338 - Reversal Transaction
//09/26/2019    137     FOyebola    Task#119792 - 232338 - View receipt through Teller Journal
//09/12/2019    138     AshishBabu  Task#118393 added code to fetch the correct rim no if tl_jouranl having rim_no as -1
//10/07/2019    139     FOyebola    Task#119792-2 - 232338 - View receipt through Teller Journal
//10/16/2019    140     mselvaga    #120397 - Receipts via Documents paper clip - sub sequence number not displaying value
//11/13/2019    141     FOyebola    Bug#121336
//---------------------------------------------------------------------------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.IO;

/*
 * TLO10000 - Gets effected if you are adding any filters.  You need to fix the report also.
 *
 *
 * */

//
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.BusObj.Global;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.BusObj.Admin.Global;
using Phoenix.BusObj.Admin.RIM;
using Phoenix.Client.TlTranTotal;
using Phoenix.Client.TlCalculator;
using Phoenix.Windows.Client;
using GlacialComponents.Controls.Common;
using Phoenix.BusObj.Misc;
using Phoenix.Busobj.Reports;
using Phoenix.Ops.OvConsole.Common;
using Phoenix.Shared.Xfs;
using Phoenix.Shared.Windows;
using Phoenix.Hyland.Service;
using Phoenix.Shared.Ucm;   //#9470
using phoenix.client.tlprinting; //#194535
using System.Collections.Generic;   //#194535

// using Phoenix.Client.TlPrinting;	// #79621

using Phoenix.Client.EcmReceipt; //119792 
using System.Net;                //119792 
using Newtonsoft.Json;           //119792
using System.Text.RegularExpressions;//119792


namespace Phoenix.Client.Journal
{
	/// <summary>
	/// Summary description for frmTlJournal.
	/// </summary>
	public partial class frmTlJournal : Phoenix.Windows.Forms.PfwStandard
	{
		#region private objects
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Phoenix.Windows.Forms.PGroupBoxStandard gbBranchDrawerandStatusCriteria;
		private Phoenix.Windows.Forms.PLabelStandard lblBranch;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbBranch;
		private Phoenix.Windows.Forms.PLabelStandard lblDrawerTeller;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbDrawers;
		private Phoenix.Windows.Forms.PLabelStandard lblTranStatus;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbTranStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblProofStatus;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbProofStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblCTRStatus;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbCTRStatus;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTransactionCriteria;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbAccount;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbAcctType;
		private Phoenix.Windows.Forms.PdfStandard dfAccount;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbReversal;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbSupervisor;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbEffectiveDate;
		private Phoenix.Windows.Forms.PLabelStandard lblFrom;
		private Phoenix.Windows.Forms.PdfStandard dfEffectiveFrom;
		private Phoenix.Windows.Forms.PLabelStandard lblTo;
		private Phoenix.Windows.Forms.PdfStandard dfEffectiveTo;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbDate;
		private Phoenix.Windows.Forms.PdfStandard dfFrom;
		private Phoenix.Windows.Forms.PdfStandard dfFromTime;
		private Phoenix.Windows.Forms.PdfStandard dfTo;
		private Phoenix.Windows.Forms.PdfStandard dfToTime;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbAmt;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbAmtType;
		private Phoenix.Windows.Forms.PLabelStandard lblFrom_Dup2;
		private Phoenix.Windows.Forms.PdfCurrency dfFromAmt;
		private Phoenix.Windows.Forms.PLabelStandard lblTo_Dup2;
		private Phoenix.Windows.Forms.PdfCurrency dfToAmt;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbMisc;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbMiscSrch;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbTransactionHistory;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private PGridColumn colTfrTranCode;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colAmt;
		private Phoenix.Windows.Forms.PGridColumn colBatchId;
		private Phoenix.Windows.Forms.PGridColumn colBatchDescription;
		private Phoenix.Windows.Forms.PGridColumn colBatchStatus;
		private Phoenix.Windows.Forms.PGridColumn colCashInAmt;
		private Phoenix.Windows.Forms.PGridColumn colCashOutAmt;
		private Phoenix.Windows.Forms.PGridColumn colCCEnable;
		private Phoenix.Windows.Forms.PGridColumn colCCAmt;
		private Phoenix.Windows.Forms.PGridColumn colChksAsCashAmt;
		private Phoenix.Windows.Forms.PGridColumn colCloseAmt;
		private Phoenix.Windows.Forms.PGridColumn colEmplId;
		private Phoenix.Windows.Forms.PGridColumn colTellerNo;
		private Phoenix.Windows.Forms.PGridColumn colEmployeeName;
		private Phoenix.Windows.Forms.PGridColumn colGlCcAcct;
		private Phoenix.Windows.Forms.PGridColumn colIntAmt;
		private Phoenix.Windows.Forms.PGridColumn colItemCount;
		private Phoenix.Windows.Forms.PGridColumn colMemoPostAmt;
		private Phoenix.Windows.Forms.PGridColumn colOnUsChksAmt;
		private Phoenix.Windows.Forms.PGridColumn colPenaltyAmt;
		private Phoenix.Windows.Forms.PGridColumn colPODStatus;
		private Phoenix.Windows.Forms.PGridColumn colPTID;
		private Phoenix.Windows.Forms.PGridColumn colPbUpdated;
		private Phoenix.Windows.Forms.PGridColumn colRealTranCode;
		private Phoenix.Windows.Forms.PGridColumn colReversal;
		private Phoenix.Windows.Forms.PGridColumn colReversalPrint;
		private Phoenix.Windows.Forms.PGridColumn colRimNo;
		private Phoenix.Windows.Forms.PGridColumn colSuperEmplId;
		private Phoenix.Windows.Forms.PGridColumn colSupervisor;
		private Phoenix.Windows.Forms.PGridColumn colSuspectAcct;
		private Phoenix.Windows.Forms.PGridColumn colTfrAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colTfrAcctType;
		private Phoenix.Windows.Forms.PGridColumn colTfrAccount;
		private Phoenix.Windows.Forms.PGridColumn colTfrEmplId;
		private Phoenix.Windows.Forms.PGridColumn colTfrPbUpdated;
		private Phoenix.Windows.Forms.PGridColumn colTransitChksAmt;
		private Phoenix.Windows.Forms.PGridColumn colTranCodePrint;
		private Phoenix.Windows.Forms.PGridColumn colTranStatus;
		private Phoenix.Windows.Forms.PGridColumn colUtilityId;
		private Phoenix.Windows.Forms.PGridColumn colUtility;
		private Phoenix.Windows.Forms.PGridColumn colUtilityAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colUtilityAcctType;
		private Phoenix.Windows.Forms.PGridColumn colMemoFloat;
		private Phoenix.Windows.Forms.PGridColumn colTranDescription;
		private Phoenix.Windows.Forms.PGridColumn colReference;
		private Phoenix.Windows.Forms.PGridColumn colExchRateType;
		private Phoenix.Windows.Forms.PGridColumn colEndpoint;
		private Phoenix.Windows.Forms.PGridColumn colEndpointDesc;
		private Phoenix.Windows.Forms.PGridColumn colForceReason;
		private Phoenix.Windows.Forms.PGridColumn colTfrChkNo;
		private Phoenix.Windows.Forms.PGridColumn colCrncyId;
		private Phoenix.Windows.Forms.PGridColumn colExchCrncyId;
		private Phoenix.Windows.Forms.PGridColumn colFcCCAmt;
		private Phoenix.Windows.Forms.PGridColumn colEquivAmt;
		private Phoenix.Windows.Forms.PGridColumn colExchRate;
		private Phoenix.Windows.Forms.PGridColumn colActualRate;
		private Phoenix.Windows.Forms.PGridColumn colExchCrIsoCode;
		private Phoenix.Windows.Forms.PGridColumn colCrncyIsoCode;
		private Phoenix.Windows.Forms.PGridColumn colExternalId;
		private Phoenix.Windows.Forms.PGridColumn colBatchPTID;
		private Phoenix.Windows.Forms.PGridColumn colAcctClosedDt;
		private Phoenix.Windows.Forms.PGridColumn colUmbCode;
		private Phoenix.Windows.Forms.PGridColumn colUmbCodeDescription;
		private Phoenix.Windows.Forms.PGridColumn colOrigPenaltyAmt;
		private Phoenix.Windows.Forms.PGridColumn colCashCount;
		private Phoenix.Windows.Forms.PGridColumn colTCDDrawerPosition;
		private Phoenix.Windows.Forms.PGridColumn colTCDCashOut;
		private Phoenix.Windows.Forms.PGridColumn colTCDCashIn;
		private Phoenix.Windows.Forms.PGridColumn colTCDDrawerNo;
		private Phoenix.Windows.Forms.PGridColumn colPayroll;
		private Phoenix.Windows.Forms.PGridColumn colReversalDisplay;
		private Phoenix.Windows.Forms.PGridColumn colCtrStatus;
		private Phoenix.Windows.Forms.PGridColumn colAggregated;
		private Phoenix.Windows.Forms.PGridColumn colTCDDevice;
		private Phoenix.Windows.Forms.PGridColumn colTellerDrawerNo;
		private Phoenix.Windows.Forms.PGridColumn colSequenceNo;
		private Phoenix.Windows.Forms.PGridColumn colSubSequence;
		private Phoenix.Windows.Forms.PGridColumn colAccount;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colDescription1;
		private Phoenix.Windows.Forms.PGridColumn colEffectiveDt;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colIsoCode;
		private Phoenix.Windows.Forms.PGridColumn colCheckNo;
		private Phoenix.Windows.Forms.PGridColumn colNetAmt;
		private Phoenix.Windows.Forms.PGridColumn colReferenceAcct;
		private Phoenix.Windows.Forms.PGridColumn colDecision;
		private Phoenix.Windows.Forms.PGridColumn colJournalDescription;
		private Phoenix.Windows.Forms.PGridColumn colNetAmtTemp;
		private Phoenix.Windows.Forms.PdfStandard dfHiddenSupOverride;
		private Phoenix.Windows.Forms.PLabelStandard lblTotalofTransactionsFetched;
		private Phoenix.Windows.Forms.PdfStandard dfTranCount;
		private Phoenix.Windows.Forms.PAction pbDisplay;
		private Phoenix.Windows.Forms.PAction pbItemCapture;
		private Phoenix.Windows.Forms.PAction pbReversal;
		private Phoenix.Windows.Forms.PAction pbReprint;
		private Phoenix.Windows.Forms.PAction pbTranTotals;
		private Phoenix.Windows.Forms.PAction pbSearch;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbTranEffectiveDt;
		private Phoenix.Windows.Forms.PLabelStandard lblFromTranEffective;
		private Phoenix.Windows.Forms.PLabelStandard lblToEnteredDt;
		private Phoenix.Windows.Forms.PLabelStandard lblFromEnteredDt;
		private Phoenix.Windows.Forms.PLabelStandard lblToTranEffective;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbCustomerName;
		private Phoenix.Windows.Forms.PLabelStandard lblMiscFrom;
		private Phoenix.Windows.Forms.PLabelStandard lblMiscTo;
		private Phoenix.Windows.Forms.PGridColumn colCustomerName;
		private Phoenix.Windows.Forms.PGridColumn colTranEffectiveDt;
		private Phoenix.Windows.Forms.PGridColumn colLastName;
		private Phoenix.Windows.Forms.PGridColumn colFirstName;
		private Phoenix.Windows.Forms.PGridColumn colMiddleInitial;
		private Phoenix.Windows.Forms.PGridColumn colTitleId;
		private Phoenix.Windows.Forms.PGridColumn colRimType;
		private Phoenix.Windows.Forms.PdfStandard dfTranEffectiveDtTo;
		private Phoenix.Windows.Forms.PdfStandard dfTranEffectiveDtFrom;
		private Phoenix.Windows.Forms.PAction pbAcctDisplay;
		private Phoenix.Windows.Forms.PGridColumn colCtrStatusTemp;
		private Phoenix.Windows.Forms.PGridColumn colCalcData1;
		private Phoenix.Windows.Forms.PGridColumn colCalcData2;
		private Phoenix.Windows.Forms.PGridColumn colBranchNo;
		private Phoenix.Windows.Forms.PLabelStandard lblHiddenSupervisorOverride;
		private Phoenix.Windows.Forms.PGridColumn colGlAcctDesc;
		private Phoenix.Windows.Forms.PGridColumn colTranCodeDisp;
		private Phoenix.Windows.Forms.PGridColumn colTranCode;
		private Phoenix.Windows.Forms.PGridColumn colForwardCreateDt;

		private Phoenix.Windows.Forms.PGridColumn colReversalCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colReversalEmplId;
		private Phoenix.Windows.Forms.PGridColumn colOfflineSeqNo;
		private PGridColumn colTcdDeviceNo;
		private PGridColumn colXpPtid;
		private PGridColumn colIncomingAcctNo;
		private PGridColumn colIncomingTfrAcctNo;
		private PComboBoxStandard cmbProofStatusMisc;
		private PGridColumn colProofStatusOnus;
		private PGridColumn colProofStatusTransit;
		private PGridColumn colOdpNsfCcAmt;
		private PGridColumn colColdStorComplete;
		private PGridColumn colColdStorMessage;
		private PdfStandard dfToLocalTime;
		private PdfStandard dfToLocalDate;
		private PLabelStandard lblToLocalDate;
		private PdfStandard dfFromLocalTime;
		private PdfStandard dfFromLocalDate;
		private PLabelStandard lblFromLocalDate;
		private PCheckBoxStandard cbLocalDateTime;
		private PGridColumn colLocalDtTime;
		private PGridColumn colLocalTZ;
		private PdfStandard dfSeqNoFrom;
		private PdfStandard dfSeqNoTo;
		private PdfStandard dfCheckNo;
		private PComboBoxStandard cmbBatchItem;
		private PGrid gridJournal;
		private PComboBoxStandard cmbTranCode;
		#region #79510
		private PGridColumn colChannel;
		private PGridColumn colSharedBranch;
		#endregion
		private PGridColumn colNonCust;
		private PAction pbEndorsement;
		private PAction pbPrintMailer; // #80615
		private PAction pbSuspectDtls; // #80660
		private PComboBoxStandard cmbMiscBranch;
		private PLabelStandard lblMiscBranch;
		private Phoenix.Windows.Forms.PGridColumn colReverserName;
		private PGridColumn colSuspectPtid;
		#endregion

		#region private variables

		#region bus objects
		private TlJournal _busObjTlJournal = new TlJournal();
		private TlJournalCalc _busObjTlJrnlCalc = new TlJournalCalc();
		private AcctTypeDetail _acctTypeDetails;
		private Phoenix.BusObj.Teller.TlJournalOvrd _tlJournalOvrd = null;
		private TellerVars _tellerVars = TellerVars.Instance;
		private GbHelper _globalHelper = new GbHelper();
		private TlHelper _tellerHelper = new TlHelper();
		//private AdGbAcctType _adGbAcctType = new AdGbAcctType(); // Commented - 175838
		private AdGbRsmRim _adGbRsmRim;
		private AdRmRestrict _adRmRestrict = new AdRmRestrict();
		private Phoenix.BusObj.Teller.TlTransactionSet _tlTranSet = new TlTransactionSet();
		private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);
		private AdGbBank _adGbBank = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBank] as AdGbBank);
		/* Begin #72916 */
		//private TlDrawer _tlDrawer = new TlDrawer(); // Commented 175838.
		/* End #72916 */
		#region #79502
		TlTransaction thisTran;
		TlTransactionSet thisTranSet;
		#endregion

		private AdGbBankControl _adGbBankControl = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBankControl] as AdGbBankControl);   //#4450
		private CashDispenser _cashDisp = null; //#72916
		private Phoenix.BusObj.Misc.CustomerSearch _acctSearchBusObj = new CustomerSearch(); // #80615
		private Phoenix.BusObj.Admin.Teller.AdTlForm _adTlForm = new Phoenix.BusObj.Admin.Teller.AdTlForm(); // #80615

		private TlOvrdTranInfo _tranInfo;   //#79314
		#endregion

		#region control objects
		private System.ComponentModel.Container components = null;
		private Filter[] filters = new Filter[100];
		private XmlNode gridNode;
		private PComboBoxStandard drawerCombo = null;
		private DialogResult dialogResult = DialogResult.None;
		private PrintInfo _wosaPrintInfo = new PrintInfo();
		private PrintInfo _tempPrintInfo = new PrintInfo();
		private XfsPrinter _xfsPrinter;
		Phoenix.Shared.Windows.HtmlPrinter _htmlPrinter; // = new Phoenix.Shared.Windows.HtmlPrinter();
		Phoenix.Shared.Windows.PSetting _printDriver = new Phoenix.Shared.Windows.PSetting();
		//
		Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;
		ArrayList _tcdDeviceResp;   //#9470
		ArrayList MailerForm = new ArrayList(); // #80615
		ArrayList CutForm = new ArrayList(); // #80615
		ArrayList MTForm = new ArrayList(); // #80615
		ArrayList MTDepositForm = new ArrayList(); // #80615
		ArrayList HoldTlTranSet = new ArrayList(); // #79502
        //begin #194535
        ReceiptDeliveryInstruction myInstruction;   //#41316
        SigBoxDetails sigPos = new SigBoxDetails();
        List<PrintInfo> PrintInfoArray = new List<PrintInfo>();
        PrintInfo printInfoForm = null;
        Bitmap combinedBitmap;
        //end   #194535
		//
		#endregion

		#region string objects
		private string _tcddrawerlist = "";
		private string _tcdDeviceNoList = "";
		private string tempAcctType = "";
		private string tempAcctNo = "";
		private string tempSuperEmpl = "";
		private string jrnlTranDesc = "";
		private string tempNonCust = "";	//#9311, CR#8975
		// Printing
		private string _reprintInfo = "";
		private string _reprintTextQrp = "";
		private string _partialPrintString = "";
		private string _wosaServiceName = "";
		//private string _logicalService = "";
		private string _formName = "";
		private string _mediaName = "";
		private string _calculatorTape = "";
		// Begin #72248 //
		private string tempRefAcctNo;
		//Begin #79510
		private string acquirerResponseXml = null;
		//End #79510

		private string MailerStatus = null; // #80615
		private string _printBalOption = null; // #79502
        //begin #194535
        private string emailAddress = string.Empty;
        private string _idsSignedArchieved = "";
        private string _printCustName = null;
        public const string PRINTANDEMAIL = "Print and Email";
        public const string EMAIL = "Email";
        public const string PRINT = "Print";
        //end   #194535
		#endregion

		#region boolean objects
		private bool isReviewCtr = false;
		private bool _isEnableTranEffectiveDt = true;
		private bool isLoadingAfterReversal = false;
		private bool isCtrDeferrelOverride = true;
		private bool isCtrDeferrelOvrdSuccess = true;
		private bool _printBalances = true;  // #12881
		private bool _printPrimaryBalances = true; // #79502
		private bool _printTfrBalances = true; // #79502
		private bool _promptBalances = false; // #79502
		private bool _formFound = false; //#79502
		private bool acctHasPostAccessOnly = false; // #79502
		private bool acctHasViewAcesssOnly = false; // #79502

		private bool isReversalOvrdSuccess = true;
		private bool isCtrDeferrelOvrdRequired = false;
		private bool isRevesalOvrdRequired = false;
		// #72916 - Issue
		private bool isOriginTcdAdmin = false;
		//
		bool _clearPendingTcrTran = false;  //#9470
		bool _isTransactionRevSuccess = false;  //#9470
		bool _diffIsOnlyCoinAmt = false;
		bool _isTCDConnected = false;
		bool _tcrNotConnectedMsgShown = false;
		bool _isSupervisorViewOnlyMode = false; //#79314
		bool _isTcdTcrDeviceIdDifferentFromTran = false;    //#11784
        //begin #194535
        private bool _isFormRecalcPmtHistoryClosed = true;
        private bool ovrdSig = false;
        private bool showItemField = false;
        private bool canReprint = false;
        bool skipped = false;
        //end   #194535
        private bool isPrintRequired = false;    //#43143 Do not disable the email radio button in dlgReceiptInfo
        private bool isReadOnly = false;      //#42567 Do not disable screen edits in dlgReceiptInfo
		#endregion

		#region pbase objects
		private PString isSupervisor = new PString();
		public PSmallInt _RimRestrictLevel = new PSmallInt(); // #79502

		private PSmallInt branchNo = new PSmallInt();
		private PSmallInt drawerNo = new PSmallInt();
		private PDateTime postingDate = new PDateTime();
		private PString drawerStatus = new PString();
		private PDateTime drCurPostingDate = new PDateTime();
		private PSmallInt _reprintFormId;
		//
		private PDecimal TcrCashOutJournalPtid = new PDecimal("TcrCashOutJournalPtid"); //#9470
		private PDecimal TcrCashInJournalPtid = new PDecimal("TcrCashOutJournalPtid"); //#9470
		private PDecimal TcrTotalCashInAmt = new PDecimal("TcrTotalCashInAmt");
		private PDecimal TcrTotalCashOutAmt = new PDecimal("TcrTotalCashOutAmt");      //#9470
		#region #76409
		private PDecimal _noCopies;
		#endregion
		//
		#region 3475
		private PString _printerService;
		#endregion
		//
		#endregion
		//
		#region date
		private DateTime searchFromDate = DateTime.MinValue;
		private DateTime searchFromTime = DateTime.MinValue;
		private DateTime searchToDate = DateTime.MinValue;
		private DateTime searchToTime = DateTime.MinValue;
		//
		#endregion

		#region short
		private short realTranCode = 0;
		private short ctrRevSuperEmplId = 0;
		private short revSuperEmplId = 0;
		private int messageId = 0;
		private short ovrdStatus = 0;
		#endregion

		#region int
		private int indexOfDash;
		// End #72248 //
		private int _checkInfoRimNo = 0;
		private int myGridContextRow = -1;
		private int _rowCount;
		private int tempRimNo = 0;
		private int _tcrRevPendingSeqNo = -1;   //#9470
		#endregion

		#region decimal
		decimal _clearPendingTcrTranAmt = 0;    //#9470
		decimal _balCoinAmt = 0;    //#9470-Coin
		decimal _tranInfoPtid = 0;  //#79314
        //begin #194535
        private decimal _archivePtid = decimal.MinValue;
        private int _printRimNo = 0;
        private int formId = -1;
        //end   #194535
		#endregion

		#region string
		string _clearPendingTcrTranType = "D";  //#9470
		#endregion

		#endregion

		public frmTlJournal()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_htmlPrinter != null)
				{
					//_htmlPrinter.Dispose();
					//73125
					if (_printDriver != null)
						_printDriver.ChangePageOrientation(_printDriver.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);

				}
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbBranchDrawerandStatusCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cmbCTRStatus = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblCTRStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbProofStatus = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblProofStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbTranStatus = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblTranStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbDrawers = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblDrawerTeller = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbBranch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbTransactionCriteria = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfTlCaptureISN = new Phoenix.Windows.Forms.PdfStandard();
            this.cmbTlCaptureWorkstation = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblNoInvItems = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbMiscBranch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.dfSeqNoFrom = new Phoenix.Windows.Forms.PdfStandard();
            this.lblMiscBranch = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfSeqNoTo = new Phoenix.Windows.Forms.PdfStandard();
            this.dfCheckNo = new Phoenix.Windows.Forms.PdfStandard();
            this.cmbBatchItem = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cmbTranCode = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.dfToLocalTime = new Phoenix.Windows.Forms.PdfStandard();
            this.dfToLocalDate = new Phoenix.Windows.Forms.PdfStandard();
            this.lblToLocalDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfFromLocalTime = new Phoenix.Windows.Forms.PdfStandard();
            this.dfFromLocalDate = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFromLocalDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbLocalDateTime = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbProofStatusMisc = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblMiscTo = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblMiscFrom = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTranEffectiveDtTo = new Phoenix.Windows.Forms.PdfStandard();
            this.cbCustomerName = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.lblToTranEffective = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTranEffectiveDtFrom = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFromTranEffective = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbTranEffectiveDt = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbMiscSrch = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cbMisc = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfToAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblTo_Dup2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfFromAmt = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblFrom_Dup2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbAmtType = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cbAmt = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfToTime = new Phoenix.Windows.Forms.PdfStandard();
            this.dfTo = new Phoenix.Windows.Forms.PdfStandard();
            this.lblToEnteredDt = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfFromTime = new Phoenix.Windows.Forms.PdfStandard();
            this.dfFrom = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFromEnteredDt = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbDate = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfEffectiveTo = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTo = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfEffectiveFrom = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFrom = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbEffectiveDate = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbSupervisor = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbReversal = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfAccount = new Phoenix.Windows.Forms.PdfStandard();
            this.cmbAcctType = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cbAccount = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbTransactionHistory = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridJournal = new Phoenix.Windows.Forms.PGrid();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchId = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colCCEnable = new Phoenix.Windows.Forms.PGridColumn();
            this.colCCAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colChksAsCashAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCloseAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTellerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colGlCcAcct = new Phoenix.Windows.Forms.PGridColumn();
            this.colIntAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colItemCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colMemoPostAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colOnUsChksAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colPenaltyAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colPODStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colPTID = new Phoenix.Windows.Forms.PGridColumn();
            this.colPbUpdated = new Phoenix.Windows.Forms.PGridColumn();
            this.colRealTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversal = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversalPrint = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSuperEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colSupervisor = new Phoenix.Windows.Forms.PGridColumn();
            this.colSuspectAcct = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrPbUpdated = new Phoenix.Windows.Forms.PGridColumn();
            this.colTransitChksAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCodePrint = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colUtilityId = new Phoenix.Windows.Forms.PGridColumn();
            this.colUtility = new Phoenix.Windows.Forms.PGridColumn();
            this.colUtilityAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colUtilityAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colMemoFloat = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colReference = new Phoenix.Windows.Forms.PGridColumn();
            this.colExchRateType = new Phoenix.Windows.Forms.PGridColumn();
            this.colEndpoint = new Phoenix.Windows.Forms.PGridColumn();
            this.colEndpointDesc = new Phoenix.Windows.Forms.PGridColumn();
            this.colForceReason = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrChkNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCrncyId = new Phoenix.Windows.Forms.PGridColumn();
            this.colExchCrncyId = new Phoenix.Windows.Forms.PGridColumn();
            this.colFcCCAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colEquivAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colExchRate = new Phoenix.Windows.Forms.PGridColumn();
            this.colActualRate = new Phoenix.Windows.Forms.PGridColumn();
            this.colExchCrIsoCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colCrncyIsoCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colExternalId = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchPTID = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctClosedDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colUmbCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colUmbCodeDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colOrigPenaltyAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashCount = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDDrawerPosition = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colPayroll = new Phoenix.Windows.Forms.PGridColumn();
            this.colBranchNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTellerDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversalDisplay = new Phoenix.Windows.Forms.PGridColumn();
            this.colCtrStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colAggregated = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDDevice = new Phoenix.Windows.Forms.PGridColumn();
            this.colSequenceNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colOfflineSeqNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSubSequence = new Phoenix.Windows.Forms.PGridColumn();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colCustomerName = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCodeDisp = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colNetAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashInAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCashOutAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDCashIn = new Phoenix.Windows.Forms.PGridColumn();
            this.colTCDCashOut = new Phoenix.Windows.Forms.PGridColumn();
            this.colEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocalDtTime = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocalTZ = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colIsoCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colReferenceAcct = new Phoenix.Windows.Forms.PGridColumn();
            this.colJournalDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colNetAmtTemp = new Phoenix.Windows.Forms.PGridColumn();
            this.colDecision = new Phoenix.Windows.Forms.PGridColumn();
            this.colLastName = new Phoenix.Windows.Forms.PGridColumn();
            this.colFirstName = new Phoenix.Windows.Forms.PGridColumn();
            this.colMiddleInitial = new Phoenix.Windows.Forms.PGridColumn();
            this.colTitleId = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimType = new Phoenix.Windows.Forms.PGridColumn();
            this.colCtrStatusTemp = new Phoenix.Windows.Forms.PGridColumn();
            this.colCalcData1 = new Phoenix.Windows.Forms.PGridColumn();
            this.colCalcData2 = new Phoenix.Windows.Forms.PGridColumn();
            this.colGlAcctDesc = new Phoenix.Windows.Forms.PGridColumn();
            this.colForwardCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversalCreateDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversalEmplId = new Phoenix.Windows.Forms.PGridColumn();
            this.colReverserName = new Phoenix.Windows.Forms.PGridColumn();
            this.colTcdDeviceNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colXpPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colIncomingAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colIncomingTfrAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colProofStatusOnus = new Phoenix.Windows.Forms.PGridColumn();
            this.colProofStatusTransit = new Phoenix.Windows.Forms.PGridColumn();
            this.colOdpNsfCcAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colColdStorComplete = new Phoenix.Windows.Forms.PGridColumn();
            this.colColdStorMessage = new Phoenix.Windows.Forms.PGridColumn();
            this.colSharedBranch = new Phoenix.Windows.Forms.PGridColumn();
            this.colChannel = new Phoenix.Windows.Forms.PGridColumn();
            this.colNonCust = new Phoenix.Windows.Forms.PGridColumn();
            this.colTfrTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colSuspectPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colTypeId = new Phoenix.Windows.Forms.PGridColumn();
            this.colPacketId = new Phoenix.Windows.Forms.PGridColumn();
            this.colClass = new Phoenix.Windows.Forms.PGridColumn();
            this.colInvItemAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colStateTaxAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colLocalTaxAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureTranNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureISN = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureBatchPtid = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureBatchId = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureImageCommited = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureWorkstation = new Phoenix.Windows.Forms.PGridColumn();
            this.colEmployeeName = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureOptionString = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlCaptureOption = new Phoenix.Windows.Forms.PGridColumn();
            this.colDepType = new Phoenix.Windows.Forms.PGridColumn();
            this.colApplType = new Phoenix.Windows.Forms.PGridColumn();
            this.dfTranCount = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTotalofTransactionsFetched = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfHiddenSupOverride = new Phoenix.Windows.Forms.PdfStandard();
            this.lblHiddenSupervisorOverride = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbDisplay = new Phoenix.Windows.Forms.PAction();
            this.pbItemCapture = new Phoenix.Windows.Forms.PAction();
            this.pbReversal = new Phoenix.Windows.Forms.PAction();
            this.pbReprint = new Phoenix.Windows.Forms.PAction();
            this.pbTranTotals = new Phoenix.Windows.Forms.PAction();
            this.pbSearch = new Phoenix.Windows.Forms.PAction();
            this.pbAcctDisplay = new Phoenix.Windows.Forms.PAction();
            this.pbEndorsement = new Phoenix.Windows.Forms.PAction();
            this.pbPrintMailer = new Phoenix.Windows.Forms.PAction();
            this.pbSuspectDtls = new Phoenix.Windows.Forms.PAction();
            this.pbInventory = new Phoenix.Windows.Forms.PAction();
            this.pbBondDetails = new Phoenix.Windows.Forms.PAction();
            this.pbImage = new Phoenix.Windows.Forms.PAction();
            this.pbViewReceipt = new Phoenix.Windows.Forms.PAction();
            this.gbBranchDrawerandStatusCriteria.SuspendLayout();
            this.gbTransactionCriteria.SuspendLayout();
            this.gbTransactionHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbDisplay,
            this.pbAcctDisplay,
            this.pbItemCapture,
            this.pbEndorsement,
            this.pbReversal,
            this.pbReprint,
            this.pbPrintMailer,
            this.pbTranTotals,
            this.pbSearch,
            this.pbSuspectDtls,
            this.pbInventory,
            this.pbBondDetails,
            this.pbImage,
            this.pbViewReceipt});
            // 
            // gbBranchDrawerandStatusCriteria
            // 
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.cmbCTRStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.lblCTRStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.cmbProofStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.lblProofStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.cmbTranStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.lblTranStatus);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.cmbDrawers);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.lblDrawerTeller);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.cmbBranch);
            this.gbBranchDrawerandStatusCriteria.Controls.Add(this.lblBranch);
            this.gbBranchDrawerandStatusCriteria.Location = new System.Drawing.Point(4, 0);
            this.gbBranchDrawerandStatusCriteria.Name = "gbBranchDrawerandStatusCriteria";
            this.gbBranchDrawerandStatusCriteria.PhoenixUIControl.ObjectId = 1;
            this.gbBranchDrawerandStatusCriteria.Size = new System.Drawing.Size(224, 185);
            this.gbBranchDrawerandStatusCriteria.TabIndex = 1;
            this.gbBranchDrawerandStatusCriteria.TabStop = false;
            this.gbBranchDrawerandStatusCriteria.Text = "Branch, Drawer and Status Criteria";
            // 
            // cmbCTRStatus
            // 
            this.cmbCTRStatus.Location = new System.Drawing.Point(80, 112);
            this.cmbCTRStatus.Name = "cmbCTRStatus";
            this.cmbCTRStatus.PhoenixUIControl.ObjectId = 108;
            this.cmbCTRStatus.PhoenixUIControl.XmlTag = "CtrStatus";
            this.cmbCTRStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbCTRStatus.TabIndex = 9;
            this.cmbCTRStatus.Value = null;
            // 
            // lblCTRStatus
            // 
            this.lblCTRStatus.AutoEllipsis = true;
            this.lblCTRStatus.Location = new System.Drawing.Point(4, 112);
            this.lblCTRStatus.Name = "lblCTRStatus";
            this.lblCTRStatus.PhoenixUIControl.ObjectId = 108;
            this.lblCTRStatus.Size = new System.Drawing.Size(68, 20);
            this.lblCTRStatus.TabIndex = 8;
            this.lblCTRStatus.Text = "CTR Status:";
            // 
            // cmbProofStatus
            // 
            this.cmbProofStatus.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbProofStatus.Location = new System.Drawing.Point(80, 88);
            this.cmbProofStatus.Name = "cmbProofStatus";
            this.cmbProofStatus.PhoenixUIControl.ObjectId = 5;
            this.cmbProofStatus.PhoenixUIControl.XmlTag = "ProofStatusOnus";
            this.cmbProofStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbProofStatus.TabIndex = 7;
            this.cmbProofStatus.Value = null;
            // 
            // lblProofStatus
            // 
            this.lblProofStatus.AutoEllipsis = true;
            this.lblProofStatus.Location = new System.Drawing.Point(4, 88);
            this.lblProofStatus.Name = "lblProofStatus";
            this.lblProofStatus.PhoenixUIControl.ObjectId = 5;
            this.lblProofStatus.Size = new System.Drawing.Size(72, 20);
            this.lblProofStatus.TabIndex = 6;
            this.lblProofStatus.Text = "Proof Status:";
            // 
            // cmbTranStatus
            // 
            this.cmbTranStatus.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbTranStatus.Location = new System.Drawing.Point(80, 64);
            this.cmbTranStatus.Name = "cmbTranStatus";
            this.cmbTranStatus.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbTranStatus.PhoenixUIControl.ObjectId = 4;
            this.cmbTranStatus.PhoenixUIControl.XmlTag = "TranStatus";
            this.cmbTranStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbTranStatus.TabIndex = 5;
            this.cmbTranStatus.Value = null;
            // 
            // lblTranStatus
            // 
            this.lblTranStatus.AutoEllipsis = true;
            this.lblTranStatus.Location = new System.Drawing.Point(4, 64);
            this.lblTranStatus.Name = "lblTranStatus";
            this.lblTranStatus.PhoenixUIControl.ObjectId = 4;
            this.lblTranStatus.Size = new System.Drawing.Size(68, 20);
            this.lblTranStatus.TabIndex = 4;
            this.lblTranStatus.Text = "Tran Status:";
            // 
            // cmbDrawers
            // 
            this.cmbDrawers.Location = new System.Drawing.Point(80, 40);
            this.cmbDrawers.Name = "cmbDrawers";
            this.cmbDrawers.PhoenixUIControl.ObjectId = 3;
            this.cmbDrawers.PhoenixUIControl.XmlTag = "DrawerNo";
            this.cmbDrawers.Size = new System.Drawing.Size(140, 21);
            this.cmbDrawers.TabIndex = 3;
            this.cmbDrawers.Value = null;
            this.cmbDrawers.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbDrawers_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblDrawerTeller
            // 
            this.lblDrawerTeller.AutoEllipsis = true;
            this.lblDrawerTeller.Location = new System.Drawing.Point(4, 40);
            this.lblDrawerTeller.Name = "lblDrawerTeller";
            this.lblDrawerTeller.PhoenixUIControl.ObjectId = 3;
            this.lblDrawerTeller.Size = new System.Drawing.Size(80, 20);
            this.lblDrawerTeller.TabIndex = 2;
            this.lblDrawerTeller.Text = "Drawer/&Teller:";
            // 
            // cmbBranch
            // 
            this.cmbBranch.Location = new System.Drawing.Point(80, 16);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.PhoenixUIControl.ObjectId = 2;
            this.cmbBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.cmbBranch.Size = new System.Drawing.Size(140, 21);
            this.cmbBranch.TabIndex = 1;
            this.cmbBranch.Value = null;
            this.cmbBranch.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbBranch_PhoenixUISelectedIndexChangedEvent);
            this.cmbBranch.Validated += new System.EventHandler(this.cmbBranch_Validated);
            // 
            // lblBranch
            // 
            this.lblBranch.AutoEllipsis = true;
            this.lblBranch.Location = new System.Drawing.Point(4, 16);
            this.lblBranch.Name = "lblBranch";
            this.lblBranch.PhoenixUIControl.ObjectId = 2;
            this.lblBranch.Size = new System.Drawing.Size(68, 20);
            this.lblBranch.TabIndex = 0;
            this.lblBranch.Text = "Branch:";
            // 
            // gbTransactionCriteria
            // 
            this.gbTransactionCriteria.Controls.Add(this.dfTlCaptureISN);
            this.gbTransactionCriteria.Controls.Add(this.cmbTlCaptureWorkstation);
            this.gbTransactionCriteria.Controls.Add(this.lblNoInvItems);
            this.gbTransactionCriteria.Controls.Add(this.cmbMiscBranch);
            this.gbTransactionCriteria.Controls.Add(this.dfSeqNoFrom);
            this.gbTransactionCriteria.Controls.Add(this.lblMiscBranch);
            this.gbTransactionCriteria.Controls.Add(this.dfSeqNoTo);
            this.gbTransactionCriteria.Controls.Add(this.dfCheckNo);
            this.gbTransactionCriteria.Controls.Add(this.cmbBatchItem);
            this.gbTransactionCriteria.Controls.Add(this.cmbTranCode);
            this.gbTransactionCriteria.Controls.Add(this.dfToLocalTime);
            this.gbTransactionCriteria.Controls.Add(this.dfToLocalDate);
            this.gbTransactionCriteria.Controls.Add(this.lblToLocalDate);
            this.gbTransactionCriteria.Controls.Add(this.dfFromLocalTime);
            this.gbTransactionCriteria.Controls.Add(this.dfFromLocalDate);
            this.gbTransactionCriteria.Controls.Add(this.lblFromLocalDate);
            this.gbTransactionCriteria.Controls.Add(this.cbLocalDateTime);
            this.gbTransactionCriteria.Controls.Add(this.cmbProofStatusMisc);
            this.gbTransactionCriteria.Controls.Add(this.lblMiscTo);
            this.gbTransactionCriteria.Controls.Add(this.lblMiscFrom);
            this.gbTransactionCriteria.Controls.Add(this.dfTranEffectiveDtTo);
            this.gbTransactionCriteria.Controls.Add(this.cbCustomerName);
            this.gbTransactionCriteria.Controls.Add(this.lblToTranEffective);
            this.gbTransactionCriteria.Controls.Add(this.dfTranEffectiveDtFrom);
            this.gbTransactionCriteria.Controls.Add(this.lblFromTranEffective);
            this.gbTransactionCriteria.Controls.Add(this.cbTranEffectiveDt);
            this.gbTransactionCriteria.Controls.Add(this.cmbMiscSrch);
            this.gbTransactionCriteria.Controls.Add(this.cbMisc);
            this.gbTransactionCriteria.Controls.Add(this.dfToAmt);
            this.gbTransactionCriteria.Controls.Add(this.lblTo_Dup2);
            this.gbTransactionCriteria.Controls.Add(this.dfFromAmt);
            this.gbTransactionCriteria.Controls.Add(this.lblFrom_Dup2);
            this.gbTransactionCriteria.Controls.Add(this.cmbAmtType);
            this.gbTransactionCriteria.Controls.Add(this.cbAmt);
            this.gbTransactionCriteria.Controls.Add(this.dfToTime);
            this.gbTransactionCriteria.Controls.Add(this.dfTo);
            this.gbTransactionCriteria.Controls.Add(this.lblToEnteredDt);
            this.gbTransactionCriteria.Controls.Add(this.dfFromTime);
            this.gbTransactionCriteria.Controls.Add(this.dfFrom);
            this.gbTransactionCriteria.Controls.Add(this.lblFromEnteredDt);
            this.gbTransactionCriteria.Controls.Add(this.cbDate);
            this.gbTransactionCriteria.Controls.Add(this.dfEffectiveTo);
            this.gbTransactionCriteria.Controls.Add(this.lblTo);
            this.gbTransactionCriteria.Controls.Add(this.dfEffectiveFrom);
            this.gbTransactionCriteria.Controls.Add(this.lblFrom);
            this.gbTransactionCriteria.Controls.Add(this.cbEffectiveDate);
            this.gbTransactionCriteria.Controls.Add(this.cbSupervisor);
            this.gbTransactionCriteria.Controls.Add(this.cbReversal);
            this.gbTransactionCriteria.Controls.Add(this.dfAccount);
            this.gbTransactionCriteria.Controls.Add(this.cmbAcctType);
            this.gbTransactionCriteria.Controls.Add(this.cbAccount);
            this.gbTransactionCriteria.Location = new System.Drawing.Point(232, 0);
            this.gbTransactionCriteria.Name = "gbTransactionCriteria";
            this.gbTransactionCriteria.PhoenixUIControl.ObjectId = 6;
            this.gbTransactionCriteria.Size = new System.Drawing.Size(456, 185);
            this.gbTransactionCriteria.TabIndex = 2;
            this.gbTransactionCriteria.TabStop = false;
            this.gbTransactionCriteria.Text = "Transaction Criteria";
            // 
            // dfTlCaptureISN
            // 
            this.dfTlCaptureISN.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptureISN.Location = new System.Drawing.Point(280, 157);
            this.dfTlCaptureISN.MaxLength = 12;
            this.dfTlCaptureISN.Name = "dfTlCaptureISN";
            this.dfTlCaptureISN.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTlCaptureISN.PhoenixUIControl.ObjectId = 164;
            this.dfTlCaptureISN.PhoenixUIControl.XmlTag = "TlCaptureIsn";
            this.dfTlCaptureISN.PreviousValue = null;
            this.dfTlCaptureISN.Size = new System.Drawing.Size(172, 20);
            this.dfTlCaptureISN.TabIndex = 48;
            this.dfTlCaptureISN.Visible = false;
            this.dfTlCaptureISN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dfTlCaptureISN_KeyPress);
            // 
            // cmbTlCaptureWorkstation
            // 
            this.cmbTlCaptureWorkstation.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CombinedValue;
            this.cmbTlCaptureWorkstation.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayDefault;
            this.cmbTlCaptureWorkstation.Location = new System.Drawing.Point(280, 158);
            this.cmbTlCaptureWorkstation.Name = "cmbTlCaptureWorkstation";
            this.cmbTlCaptureWorkstation.PhoenixUIControl.ObjectId = 163;
            this.cmbTlCaptureWorkstation.PhoenixUIControl.XmlTag = "TlCaptureWorkstation";
            this.cmbTlCaptureWorkstation.Size = new System.Drawing.Size(172, 21);
            this.cmbTlCaptureWorkstation.TabIndex = 47;
            this.cmbTlCaptureWorkstation.Value = null;
            this.cmbTlCaptureWorkstation.Visible = false;
            // 
            // lblNoInvItems
            // 
            this.lblNoInvItems.AutoEllipsis = true;
            this.lblNoInvItems.Location = new System.Drawing.Point(236, 156);
            this.lblNoInvItems.Name = "lblNoInvItems";
            this.lblNoInvItems.PhoenixUIControl.ObjectId = 151;
            this.lblNoInvItems.Size = new System.Drawing.Size(44, 20);
            this.lblNoInvItems.TabIndex = 38;
            this.lblNoInvItems.Text = "# Items:";
            this.lblNoInvItems.Visible = false;
            // 
            // cmbMiscBranch
            // 
            this.cmbMiscBranch.Location = new System.Drawing.Point(292, 156);
            this.cmbMiscBranch.Name = "cmbMiscBranch";
            this.cmbMiscBranch.PhoenixUIControl.ObjectId = 147;
            this.cmbMiscBranch.PhoenixUIControl.XmlTag = "BranchNo";
            this.cmbMiscBranch.Size = new System.Drawing.Size(160, 21);
            this.cmbMiscBranch.TabIndex = 41;
            this.cmbMiscBranch.Value = null;
            this.cmbMiscBranch.Visible = false;
            // 
            // dfSeqNoFrom
            // 
            this.dfSeqNoFrom.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSeqNoFrom.Location = new System.Drawing.Point(280, 157);
            this.dfSeqNoFrom.Name = "dfSeqNoFrom";
            this.dfSeqNoFrom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSeqNoFrom.PhoenixUIControl.ObjectId = 125;
            this.dfSeqNoFrom.PhoenixUIControl.XmlTag = "FromSearchNum";
            this.dfSeqNoFrom.PreviousValue = null;
            this.dfSeqNoFrom.Size = new System.Drawing.Size(68, 20);
            this.dfSeqNoFrom.TabIndex = 40;
            // 
            // lblMiscBranch
            // 
            this.lblMiscBranch.AutoEllipsis = true;
            this.lblMiscBranch.Location = new System.Drawing.Point(236, 156);
            this.lblMiscBranch.Name = "lblMiscBranch";
            this.lblMiscBranch.PhoenixUIControl.ObjectId = 147;
            this.lblMiscBranch.Size = new System.Drawing.Size(44, 20);
            this.lblMiscBranch.TabIndex = 39;
            this.lblMiscBranch.Text = "Branch:";
            this.lblMiscBranch.Visible = false;
            // 
            // dfSeqNoTo
            // 
            this.dfSeqNoTo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSeqNoTo.Location = new System.Drawing.Point(384, 157);
            this.dfSeqNoTo.Name = "dfSeqNoTo";
            this.dfSeqNoTo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfSeqNoTo.PhoenixUIControl.ObjectId = 126;
            this.dfSeqNoTo.PhoenixUIControl.XmlTag = "ToSearchNum";
            this.dfSeqNoTo.PreviousValue = null;
            this.dfSeqNoTo.Size = new System.Drawing.Size(68, 20);
            this.dfSeqNoTo.TabIndex = 43;
            // 
            // dfCheckNo
            // 
            this.dfCheckNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfCheckNo.Location = new System.Drawing.Point(280, 157);
            this.dfCheckNo.Name = "dfCheckNo";
            this.dfCheckNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfCheckNo.PhoenixUIControl.ObjectId = 21;
            this.dfCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.dfCheckNo.PreviousValue = null;
            this.dfCheckNo.Size = new System.Drawing.Size(64, 20);
            this.dfCheckNo.TabIndex = 42;
            // 
            // cmbBatchItem
            // 
            this.cmbBatchItem.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayFirst;
            this.cmbBatchItem.Location = new System.Drawing.Point(280, 158);
            this.cmbBatchItem.Name = "cmbBatchItem";
            this.cmbBatchItem.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbBatchItem.PhoenixUIControl.ObjectId = 8;
            this.cmbBatchItem.PhoenixUIControl.XmlTag = "BatchId";
            this.cmbBatchItem.Size = new System.Drawing.Size(172, 21);
            this.cmbBatchItem.TabIndex = 43;
            this.cmbBatchItem.Value = null;
            // 
            // cmbTranCode
            // 
            this.cmbTranCode.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CombinedValue;
            this.cmbTranCode.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayFirst;
            this.cmbTranCode.Location = new System.Drawing.Point(280, 158);
            this.cmbTranCode.Name = "cmbTranCode";
            this.cmbTranCode.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbTranCode.PhoenixUIControl.ObjectId = 8;
            this.cmbTranCode.PhoenixUIControl.XmlTag = "TlTranCode";
            this.cmbTranCode.Size = new System.Drawing.Size(172, 21);
            this.cmbTranCode.TabIndex = 44;
            this.cmbTranCode.Value = null;
            this.cmbTranCode.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbTranCode_PhoenixUISelectedIndexChangedEvent);
            // 
            // dfToLocalTime
            // 
            this.dfToLocalTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToLocalTime.Location = new System.Drawing.Point(392, 112);
            this.dfToLocalTime.Name = "dfToLocalTime";
            this.dfToLocalTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToLocalTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfToLocalTime.PhoenixUIControl.ObjectId = 141;
            this.dfToLocalTime.PreviousValue = null;
            this.dfToLocalTime.Size = new System.Drawing.Size(60, 20);
            this.dfToLocalTime.TabIndex = 29;
            // 
            // dfToLocalDate
            // 
            this.dfToLocalDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToLocalDate.Location = new System.Drawing.Point(320, 112);
            this.dfToLocalDate.Name = "dfToLocalDate";
            this.dfToLocalDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToLocalDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfToLocalDate.PhoenixUIControl.InputMask = "";
            this.dfToLocalDate.PhoenixUIControl.ObjectId = 141;
            this.dfToLocalDate.PreviousValue = null;
            this.dfToLocalDate.Size = new System.Drawing.Size(68, 20);
            this.dfToLocalDate.TabIndex = 28;
            // 
            // lblToLocalDate
            // 
            this.lblToLocalDate.AutoEllipsis = true;
            this.lblToLocalDate.Location = new System.Drawing.Point(300, 112);
            this.lblToLocalDate.Name = "lblToLocalDate";
            this.lblToLocalDate.PhoenixUIControl.ObjectId = 141;
            this.lblToLocalDate.Size = new System.Drawing.Size(24, 20);
            this.lblToLocalDate.TabIndex = 27;
            this.lblToLocalDate.Text = "&To:";
            // 
            // dfFromLocalTime
            // 
            this.dfFromLocalTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromLocalTime.Location = new System.Drawing.Point(240, 112);
            this.dfFromLocalTime.Name = "dfFromLocalTime";
            this.dfFromLocalTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromLocalTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfFromLocalTime.PhoenixUIControl.ObjectId = 140;
            this.dfFromLocalTime.PreviousValue = null;
            this.dfFromLocalTime.Size = new System.Drawing.Size(60, 20);
            this.dfFromLocalTime.TabIndex = 26;
            // 
            // dfFromLocalDate
            // 
            this.dfFromLocalDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromLocalDate.Location = new System.Drawing.Point(168, 112);
            this.dfFromLocalDate.Name = "dfFromLocalDate";
            this.dfFromLocalDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromLocalDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfFromLocalDate.PhoenixUIControl.InputMask = "";
            this.dfFromLocalDate.PhoenixUIControl.ObjectId = 140;
            this.dfFromLocalDate.PreviousValue = null;
            this.dfFromLocalDate.Size = new System.Drawing.Size(68, 20);
            this.dfFromLocalDate.TabIndex = 25;
            // 
            // lblFromLocalDate
            // 
            this.lblFromLocalDate.AutoEllipsis = true;
            this.lblFromLocalDate.Location = new System.Drawing.Point(128, 112);
            this.lblFromLocalDate.Name = "lblFromLocalDate";
            this.lblFromLocalDate.PhoenixUIControl.ObjectId = 140;
            this.lblFromLocalDate.Size = new System.Drawing.Size(36, 20);
            this.lblFromLocalDate.TabIndex = 24;
            this.lblFromLocalDate.Text = "&From:";
            // 
            // cbLocalDateTime
            // 
            this.cbLocalDateTime.BackColor = System.Drawing.SystemColors.Control;
            this.cbLocalDateTime.Checked = true;
            this.cbLocalDateTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLocalDateTime.Location = new System.Drawing.Point(4, 112);
            this.cbLocalDateTime.Name = "cbLocalDateTime";
            this.cbLocalDateTime.PhoenixUIControl.ObjectId = 139;
            this.cbLocalDateTime.Size = new System.Drawing.Size(120, 20);
            this.cbLocalDateTime.TabIndex = 23;
            this.cbLocalDateTime.Text = "Local Date/Time:";
            this.cbLocalDateTime.UseVisualStyleBackColor = false;
            this.cbLocalDateTime.Value = null;
            this.cbLocalDateTime.Click += new System.EventHandler(this.cbLocalDateTime_Click);
            // 
            // cmbProofStatusMisc
            // 
            this.cmbProofStatusMisc.InitialDisplayType = Phoenix.Windows.Forms.UIComboInitialDisplayType.DisplayFirst;
            this.cmbProofStatusMisc.Location = new System.Drawing.Point(280, 158);
            this.cmbProofStatusMisc.Name = "cmbProofStatusMisc";
            this.cmbProofStatusMisc.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbProofStatusMisc.PhoenixUIControl.ObjectId = 8;
            this.cmbProofStatusMisc.PhoenixUIControl.XmlTag = "ProofStatusOnus";
            this.cmbProofStatusMisc.Size = new System.Drawing.Size(172, 21);
            this.cmbProofStatusMisc.TabIndex = 46;
            this.cmbProofStatusMisc.Value = null;
            // 
            // lblMiscTo
            // 
            this.lblMiscTo.AutoEllipsis = true;
            this.lblMiscTo.Location = new System.Drawing.Point(356, 157);
            this.lblMiscTo.Name = "lblMiscTo";
            this.lblMiscTo.PhoenixUIControl.ObjectId = 16;
            this.lblMiscTo.Size = new System.Drawing.Size(24, 20);
            this.lblMiscTo.TabIndex = 42;
            this.lblMiscTo.Text = "To:";
            // 
            // lblMiscFrom
            // 
            this.lblMiscFrom.AutoEllipsis = true;
            this.lblMiscFrom.Location = new System.Drawing.Point(236, 158);
            this.lblMiscFrom.Name = "lblMiscFrom";
            this.lblMiscFrom.PhoenixUIControl.ObjectId = 15;
            this.lblMiscFrom.Size = new System.Drawing.Size(36, 20);
            this.lblMiscFrom.TabIndex = 38;
            this.lblMiscFrom.Text = "From:";
            this.lblMiscFrom.Visible = false;
            // 
            // dfTranEffectiveDtTo
            // 
            this.dfTranEffectiveDtTo.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTranEffectiveDtTo.Location = new System.Drawing.Point(268, 64);
            this.dfTranEffectiveDtTo.Name = "dfTranEffectiveDtTo";
            this.dfTranEffectiveDtTo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTranEffectiveDtTo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfTranEffectiveDtTo.PhoenixUIControl.InputMask = "";
            this.dfTranEffectiveDtTo.PhoenixUIControl.ObjectId = 124;
            this.dfTranEffectiveDtTo.PhoenixUIControl.XmlTag = "TranEffectiveDtTo";
            this.dfTranEffectiveDtTo.PreviousValue = null;
            this.dfTranEffectiveDtTo.Size = new System.Drawing.Size(68, 20);
            this.dfTranEffectiveDtTo.TabIndex = 14;
            // 
            // cbCustomerName
            // 
            this.cbCustomerName.BackColor = System.Drawing.SystemColors.Control;
            this.cbCustomerName.Checked = true;
            this.cbCustomerName.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCustomerName.Location = new System.Drawing.Point(340, 64);
            this.cbCustomerName.Name = "cbCustomerName";
            this.cbCustomerName.PhoenixUIControl.ObjectId = 121;
            this.cbCustomerName.Size = new System.Drawing.Size(104, 20);
            this.cbCustomerName.TabIndex = 15;
            this.cbCustomerName.Text = "Acct Name/Desc";
            this.cbCustomerName.UseVisualStyleBackColor = false;
            this.cbCustomerName.Value = null;
            this.cbCustomerName.Click += new System.EventHandler(this.cbCustomerName_Click);
            // 
            // lblToTranEffective
            // 
            this.lblToTranEffective.AutoEllipsis = true;
            this.lblToTranEffective.Location = new System.Drawing.Point(240, 64);
            this.lblToTranEffective.Name = "lblToTranEffective";
            this.lblToTranEffective.Size = new System.Drawing.Size(24, 20);
            this.lblToTranEffective.TabIndex = 13;
            this.lblToTranEffective.Text = "&To:";
            // 
            // dfTranEffectiveDtFrom
            // 
            this.dfTranEffectiveDtFrom.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTranEffectiveDtFrom.Location = new System.Drawing.Point(168, 64);
            this.dfTranEffectiveDtFrom.Name = "dfTranEffectiveDtFrom";
            this.dfTranEffectiveDtFrom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTranEffectiveDtFrom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfTranEffectiveDtFrom.PhoenixUIControl.InputMask = "";
            this.dfTranEffectiveDtFrom.PhoenixUIControl.ObjectId = 123;
            this.dfTranEffectiveDtFrom.PhoenixUIControl.XmlTag = "TranEffectiveDtFrom";
            this.dfTranEffectiveDtFrom.PreviousValue = null;
            this.dfTranEffectiveDtFrom.Size = new System.Drawing.Size(68, 20);
            this.dfTranEffectiveDtFrom.TabIndex = 12;
            // 
            // lblFromTranEffective
            // 
            this.lblFromTranEffective.AutoEllipsis = true;
            this.lblFromTranEffective.Location = new System.Drawing.Point(128, 64);
            this.lblFromTranEffective.Name = "lblFromTranEffective";
            this.lblFromTranEffective.Size = new System.Drawing.Size(36, 20);
            this.lblFromTranEffective.TabIndex = 11;
            this.lblFromTranEffective.Text = "&From:";
            // 
            // cbTranEffectiveDt
            // 
            this.cbTranEffectiveDt.BackColor = System.Drawing.SystemColors.Control;
            this.cbTranEffectiveDt.Checked = true;
            this.cbTranEffectiveDt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTranEffectiveDt.Location = new System.Drawing.Point(4, 64);
            this.cbTranEffectiveDt.Name = "cbTranEffectiveDt";
            this.cbTranEffectiveDt.PhoenixUIControl.ObjectId = 122;
            this.cbTranEffectiveDt.Size = new System.Drawing.Size(120, 20);
            this.cbTranEffectiveDt.TabIndex = 10;
            this.cbTranEffectiveDt.Text = "Tran Effective Date:";
            this.cbTranEffectiveDt.UseVisualStyleBackColor = false;
            this.cbTranEffectiveDt.Value = null;
            this.cbTranEffectiveDt.Click += new System.EventHandler(this.cbTranEffectiveDt_Click);
            // 
            // cmbMiscSrch
            // 
            this.cmbMiscSrch.Location = new System.Drawing.Point(72, 158);
            this.cmbMiscSrch.Name = "cmbMiscSrch";
            this.cmbMiscSrch.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbMiscSrch.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.True;
            this.cmbMiscSrch.PhoenixUIControl.ObjectId = 129;
            this.cmbMiscSrch.PhoenixUIControl.XmlTag = "MiscSearch";
            this.cmbMiscSrch.Size = new System.Drawing.Size(156, 21);
            this.cmbMiscSrch.TabIndex = 37;
            this.cmbMiscSrch.Value = null;
            this.cmbMiscSrch.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbMiscSrch_PhoenixUISelectedIndexChangedEvent);
            // 
            // cbMisc
            // 
            this.cbMisc.BackColor = System.Drawing.SystemColors.Control;
            this.cbMisc.Checked = true;
            this.cbMisc.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbMisc.Location = new System.Drawing.Point(4, 158);
            this.cbMisc.Name = "cbMisc";
            this.cbMisc.PhoenixUIControl.ObjectId = 9;
            this.cbMisc.Size = new System.Drawing.Size(64, 20);
            this.cbMisc.TabIndex = 36;
            this.cbMisc.Text = "Misc:";
            this.cbMisc.UseVisualStyleBackColor = false;
            this.cbMisc.Value = null;
            this.cbMisc.Click += new System.EventHandler(this.cbMisc_Click);
            // 
            // dfToAmt
            // 
            this.dfToAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfToAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfToAmt.Location = new System.Drawing.Point(384, 134);
            this.dfToAmt.Name = "dfToAmt";
            this.dfToAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfToAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfToAmt.PhoenixUIControl.MaxPrecision = 14;
            this.dfToAmt.PhoenixUIControl.ObjectId = 16;
            this.dfToAmt.PhoenixUIControl.XmlTag = "ToSearchAmt";
            this.dfToAmt.PreviousValue = null;
            this.dfToAmt.Size = new System.Drawing.Size(68, 20);
            this.dfToAmt.TabIndex = 35;
            this.dfToAmt.Text = "$0.00";
            // 
            // lblTo_Dup2
            // 
            this.lblTo_Dup2.AutoEllipsis = true;
            this.lblTo_Dup2.Location = new System.Drawing.Point(356, 138);
            this.lblTo_Dup2.Name = "lblTo_Dup2";
            this.lblTo_Dup2.PhoenixUIControl.ObjectId = 16;
            this.lblTo_Dup2.Size = new System.Drawing.Size(24, 12);
            this.lblTo_Dup2.TabIndex = 34;
            this.lblTo_Dup2.Text = "&To:";
            // 
            // dfFromAmt
            // 
            this.dfFromAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfFromAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfFromAmt.Location = new System.Drawing.Point(280, 134);
            this.dfFromAmt.Name = "dfFromAmt";
            this.dfFromAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfFromAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.dfFromAmt.PhoenixUIControl.MaxPrecision = 14;
            this.dfFromAmt.PhoenixUIControl.ObjectId = 15;
            this.dfFromAmt.PhoenixUIControl.XmlTag = "FromSearchAmt";
            this.dfFromAmt.PreviousValue = null;
            this.dfFromAmt.Size = new System.Drawing.Size(68, 20);
            this.dfFromAmt.TabIndex = 33;
            this.dfFromAmt.Text = "$0.00";
            // 
            // lblFrom_Dup2
            // 
            this.lblFrom_Dup2.AutoEllipsis = true;
            this.lblFrom_Dup2.Location = new System.Drawing.Point(236, 135);
            this.lblFrom_Dup2.Name = "lblFrom_Dup2";
            this.lblFrom_Dup2.PhoenixUIControl.ObjectId = 15;
            this.lblFrom_Dup2.Size = new System.Drawing.Size(36, 20);
            this.lblFrom_Dup2.TabIndex = 32;
            this.lblFrom_Dup2.Text = "&From:";
            // 
            // cmbAmtType
            // 
            this.cmbAmtType.Location = new System.Drawing.Point(72, 134);
            this.cmbAmtType.Name = "cmbAmtType";
            this.cmbAmtType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbAmtType.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.True;
            this.cmbAmtType.PhoenixUIControl.ObjectId = 128;
            this.cmbAmtType.PhoenixUIControl.XmlTag = "AmtType";
            this.cmbAmtType.Size = new System.Drawing.Size(156, 21);
            this.cmbAmtType.TabIndex = 31;
            this.cmbAmtType.Value = null;
            this.cmbAmtType.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbAmtType_PhoenixUISelectedIndexChangedEvent);
            // 
            // cbAmt
            // 
            this.cbAmt.BackColor = System.Drawing.SystemColors.Control;
            this.cbAmt.Checked = true;
            this.cbAmt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAmt.Location = new System.Drawing.Point(4, 134);
            this.cbAmt.Name = "cbAmt";
            this.cbAmt.PhoenixUIControl.ObjectId = 14;
            this.cbAmt.Size = new System.Drawing.Size(64, 20);
            this.cbAmt.TabIndex = 30;
            this.cbAmt.Text = "Amount:";
            this.cbAmt.UseVisualStyleBackColor = false;
            this.cbAmt.Value = null;
            this.cbAmt.Click += new System.EventHandler(this.cbAmt_Click);
            // 
            // dfToTime
            // 
            this.dfToTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToTime.Location = new System.Drawing.Point(392, 88);
            this.dfToTime.Name = "dfToTime";
            this.dfToTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfToTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfToTime.PhoenixUIControl.ObjectId = 12;
            this.dfToTime.PreviousValue = null;
            this.dfToTime.Size = new System.Drawing.Size(60, 20);
            this.dfToTime.TabIndex = 22;
            // 
            // dfTo
            // 
            this.dfTo.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTo.Location = new System.Drawing.Point(320, 88);
            this.dfTo.Name = "dfTo";
            this.dfTo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfTo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfTo.PhoenixUIControl.InputMask = "";
            this.dfTo.PhoenixUIControl.ObjectId = 12;
            this.dfTo.PreviousValue = null;
            this.dfTo.Size = new System.Drawing.Size(68, 20);
            this.dfTo.TabIndex = 21;
            // 
            // lblToEnteredDt
            // 
            this.lblToEnteredDt.AutoEllipsis = true;
            this.lblToEnteredDt.Location = new System.Drawing.Point(300, 88);
            this.lblToEnteredDt.Name = "lblToEnteredDt";
            this.lblToEnteredDt.PhoenixUIControl.ObjectId = 12;
            this.lblToEnteredDt.Size = new System.Drawing.Size(24, 20);
            this.lblToEnteredDt.TabIndex = 20;
            this.lblToEnteredDt.Text = "&To:";
            // 
            // dfFromTime
            // 
            this.dfFromTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromTime.Location = new System.Drawing.Point(240, 88);
            this.dfFromTime.Name = "dfFromTime";
            this.dfFromTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFromTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfFromTime.PhoenixUIControl.ObjectId = 11;
            this.dfFromTime.PreviousValue = null;
            this.dfFromTime.Size = new System.Drawing.Size(60, 20);
            this.dfFromTime.TabIndex = 19;
            // 
            // dfFrom
            // 
            this.dfFrom.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFrom.Location = new System.Drawing.Point(168, 88);
            this.dfFrom.Name = "dfFrom";
            this.dfFrom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFrom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfFrom.PhoenixUIControl.InputMask = "";
            this.dfFrom.PhoenixUIControl.ObjectId = 11;
            this.dfFrom.PreviousValue = null;
            this.dfFrom.Size = new System.Drawing.Size(68, 20);
            this.dfFrom.TabIndex = 18;
            // 
            // lblFromEnteredDt
            // 
            this.lblFromEnteredDt.AutoEllipsis = true;
            this.lblFromEnteredDt.Location = new System.Drawing.Point(128, 88);
            this.lblFromEnteredDt.Name = "lblFromEnteredDt";
            this.lblFromEnteredDt.PhoenixUIControl.ObjectId = 11;
            this.lblFromEnteredDt.Size = new System.Drawing.Size(36, 20);
            this.lblFromEnteredDt.TabIndex = 17;
            this.lblFromEnteredDt.Text = "&From:";
            // 
            // cbDate
            // 
            this.cbDate.BackColor = System.Drawing.SystemColors.Control;
            this.cbDate.Checked = true;
            this.cbDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDate.Location = new System.Drawing.Point(4, 88);
            this.cbDate.Name = "cbDate";
            this.cbDate.PhoenixUIControl.ObjectId = 10;
            this.cbDate.Size = new System.Drawing.Size(120, 20);
            this.cbDate.TabIndex = 16;
            this.cbDate.Text = "&Entered Date/Time:";
            this.cbDate.UseVisualStyleBackColor = false;
            this.cbDate.Value = null;
            this.cbDate.Click += new System.EventHandler(this.cbDate_Click);
            // 
            // dfEffectiveTo
            // 
            this.dfEffectiveTo.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveTo.Location = new System.Drawing.Point(268, 40);
            this.dfEffectiveTo.Name = "dfEffectiveTo";
            this.dfEffectiveTo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveTo.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfEffectiveTo.PhoenixUIControl.InputMask = "";
            this.dfEffectiveTo.PhoenixUIControl.ObjectId = 105;
            this.dfEffectiveTo.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.dfEffectiveTo.PreviousValue = null;
            this.dfEffectiveTo.Size = new System.Drawing.Size(68, 20);
            this.dfEffectiveTo.TabIndex = 8;
            // 
            // lblTo
            // 
            this.lblTo.AutoEllipsis = true;
            this.lblTo.Location = new System.Drawing.Point(240, 40);
            this.lblTo.Name = "lblTo";
            this.lblTo.PhoenixUIControl.ObjectId = 105;
            this.lblTo.Size = new System.Drawing.Size(24, 20);
            this.lblTo.TabIndex = 7;
            this.lblTo.Text = "&To:";
            // 
            // dfEffectiveFrom
            // 
            this.dfEffectiveFrom.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveFrom.Location = new System.Drawing.Point(168, 40);
            this.dfEffectiveFrom.Name = "dfEffectiveFrom";
            this.dfEffectiveFrom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfEffectiveFrom.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfEffectiveFrom.PhoenixUIControl.InputMask = "";
            this.dfEffectiveFrom.PhoenixUIControl.ObjectId = 104;
            this.dfEffectiveFrom.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.dfEffectiveFrom.PreviousValue = null;
            this.dfEffectiveFrom.Size = new System.Drawing.Size(68, 20);
            this.dfEffectiveFrom.TabIndex = 6;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoEllipsis = true;
            this.lblFrom.Location = new System.Drawing.Point(128, 40);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.PhoenixUIControl.ObjectId = 104;
            this.lblFrom.Size = new System.Drawing.Size(36, 20);
            this.lblFrom.TabIndex = 5;
            this.lblFrom.Text = "&From:";
            // 
            // cbEffectiveDate
            // 
            this.cbEffectiveDate.BackColor = System.Drawing.SystemColors.Control;
            this.cbEffectiveDate.Checked = true;
            this.cbEffectiveDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEffectiveDate.Location = new System.Drawing.Point(4, 40);
            this.cbEffectiveDate.Name = "cbEffectiveDate";
            this.cbEffectiveDate.PhoenixUIControl.ObjectId = 103;
            this.cbEffectiveDate.Size = new System.Drawing.Size(120, 20);
            this.cbEffectiveDate.TabIndex = 4;
            this.cbEffectiveDate.Text = "P&osting Date:";
            this.cbEffectiveDate.UseVisualStyleBackColor = false;
            this.cbEffectiveDate.Value = null;
            this.cbEffectiveDate.Click += new System.EventHandler(this.cbEffectiveDate_Click);
            // 
            // cbSupervisor
            // 
            this.cbSupervisor.BackColor = System.Drawing.SystemColors.Control;
            this.cbSupervisor.Checked = true;
            this.cbSupervisor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSupervisor.Location = new System.Drawing.Point(340, 40);
            this.cbSupervisor.Name = "cbSupervisor";
            this.cbSupervisor.PhoenixUIControl.ObjectId = 22;
            this.cbSupervisor.Size = new System.Drawing.Size(76, 20);
            this.cbSupervisor.TabIndex = 9;
            this.cbSupervisor.Text = "Supervisor";
            this.cbSupervisor.UseVisualStyleBackColor = false;
            this.cbSupervisor.Value = null;
            // 
            // cbReversal
            // 
            this.cbReversal.BackColor = System.Drawing.SystemColors.Control;
            this.cbReversal.Checked = true;
            this.cbReversal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbReversal.Location = new System.Drawing.Point(340, 16);
            this.cbReversal.Name = "cbReversal";
            this.cbReversal.PhoenixUIControl.ObjectId = 17;
            this.cbReversal.Size = new System.Drawing.Size(76, 20);
            this.cbReversal.TabIndex = 3;
            this.cbReversal.Text = "Reversal";
            this.cbReversal.UseVisualStyleBackColor = false;
            this.cbReversal.Value = null;
            // 
            // dfAccount
            // 
            this.dfAccount.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccount.Location = new System.Drawing.Point(132, 16);
            this.dfAccount.Name = "dfAccount";
            this.dfAccount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfAccount.PhoenixUIControl.ObjectId = 13;
            this.dfAccount.PhoenixUIControl.XmlTag = "AcctNo";
            this.dfAccount.PreviousValue = null;
            this.dfAccount.Size = new System.Drawing.Size(140, 20);
            this.dfAccount.TabIndex = 2;
            this.dfAccount.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfAccount_PhoenixUIValidateEvent);
            this.dfAccount.Validating += new System.ComponentModel.CancelEventHandler(this.dfAccount_Validating);
            // 
            // cmbAcctType
            // 
            this.cmbAcctType.DisplayType = Phoenix.Windows.Forms.UIComboDisplayType.CodeValue;
            this.cmbAcctType.Location = new System.Drawing.Point(72, 16);
            this.cmbAcctType.Name = "cmbAcctType";
            this.cmbAcctType.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbAcctType.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.True;
            this.cmbAcctType.PhoenixUIControl.ObjectId = 8;
            this.cmbAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.cmbAcctType.Size = new System.Drawing.Size(56, 21);
            this.cmbAcctType.TabIndex = 1;
            this.cmbAcctType.Value = null;
            this.cmbAcctType.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbAcctType_PhoenixUISelectedIndexChangedEvent);
            this.cmbAcctType.PhoenixUIEditedEvent += new Phoenix.Windows.Forms.ValueEditedEventHandler(this.cmbAcctType_PhoenixUIEditedEvent);
            // 
            // cbAccount
            // 
            this.cbAccount.BackColor = System.Drawing.SystemColors.Control;
            this.cbAccount.Checked = true;
            this.cbAccount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAccount.Location = new System.Drawing.Point(4, 16);
            this.cbAccount.Name = "cbAccount";
            this.cbAccount.PhoenixUIControl.ObjectId = 7;
            this.cbAccount.Size = new System.Drawing.Size(64, 20);
            this.cbAccount.TabIndex = 0;
            this.cbAccount.Text = "Account:";
            this.cbAccount.UseVisualStyleBackColor = false;
            this.cbAccount.Value = null;
            this.cbAccount.Click += new System.EventHandler(this.cbAccount_Click);
            // 
            // gbTransactionHistory
            // 
            this.gbTransactionHistory.Controls.Add(this.gridJournal);
            this.gbTransactionHistory.Controls.Add(this.dfTranCount);
            this.gbTransactionHistory.Controls.Add(this.lblTotalofTransactionsFetched);
            this.gbTransactionHistory.Controls.Add(this.dfHiddenSupOverride);
            this.gbTransactionHistory.Controls.Add(this.lblHiddenSupervisorOverride);
            this.gbTransactionHistory.Location = new System.Drawing.Point(4, 184);
            this.gbTransactionHistory.Name = "gbTransactionHistory";
            this.gbTransactionHistory.PhoenixUIControl.ObjectId = 23;
            this.gbTransactionHistory.Size = new System.Drawing.Size(684, 260);
            this.gbTransactionHistory.TabIndex = 0;
            this.gbTransactionHistory.TabStop = false;
            // 
            // gridJournal
            // 
            this.gridJournal.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colAcctNo,
            this.colAcctType,
            this.colAmt,
            this.colBatchId,
            this.colBatchDescription,
            this.colBatchStatus,
            this.colCCEnable,
            this.colCCAmt,
            this.colChksAsCashAmt,
            this.colCloseAmt,
            this.colEmplId,
            this.colTellerNo,
            this.colGlCcAcct,
            this.colIntAmt,
            this.colItemCount,
            this.colMemoPostAmt,
            this.colOnUsChksAmt,
            this.colPenaltyAmt,
            this.colPODStatus,
            this.colPTID,
            this.colPbUpdated,
            this.colRealTranCode,
            this.colReversal,
            this.colReversalPrint,
            this.colRimNo,
            this.colSuperEmplId,
            this.colSupervisor,
            this.colSuspectAcct,
            this.colTfrAcctNo,
            this.colTfrAcctType,
            this.colTfrAccount,
            this.colTfrEmplId,
            this.colTfrPbUpdated,
            this.colTransitChksAmt,
            this.colTranCodePrint,
            this.colTranStatus,
            this.colUtilityId,
            this.colUtility,
            this.colUtilityAcctNo,
            this.colUtilityAcctType,
            this.colMemoFloat,
            this.colTranDescription,
            this.colReference,
            this.colExchRateType,
            this.colEndpoint,
            this.colEndpointDesc,
            this.colForceReason,
            this.colTfrChkNo,
            this.colCrncyId,
            this.colExchCrncyId,
            this.colFcCCAmt,
            this.colEquivAmt,
            this.colExchRate,
            this.colActualRate,
            this.colExchCrIsoCode,
            this.colCrncyIsoCode,
            this.colExternalId,
            this.colBatchPTID,
            this.colAcctClosedDt,
            this.colUmbCode,
            this.colUmbCodeDescription,
            this.colOrigPenaltyAmt,
            this.colCashCount,
            this.colTCDDrawerPosition,
            this.colTCDDrawerNo,
            this.colPayroll,
            this.colBranchNo,
            this.colTellerDrawerNo,
            this.colReversalDisplay,
            this.colCtrStatus,
            this.colAggregated,
            this.colTCDDevice,
            this.colSequenceNo,
            this.colOfflineSeqNo,
            this.colSubSequence,
            this.colAccount,
            this.colCustomerName,
            this.colTranCodeDisp,
            this.colTranCode,
            this.colDescription,
            this.colDescription1,
            this.colTranEffectiveDt,
            this.colNetAmt,
            this.colCashInAmt,
            this.colCashOutAmt,
            this.colTCDCashIn,
            this.colTCDCashOut,
            this.colEffectiveDt,
            this.colCreateDt,
            this.colLocalDtTime,
            this.colLocalTZ,
            this.colCheckNo,
            this.colIsoCode,
            this.colReferenceAcct,
            this.colJournalDescription,
            this.colNetAmtTemp,
            this.colDecision,
            this.colLastName,
            this.colFirstName,
            this.colMiddleInitial,
            this.colTitleId,
            this.colRimType,
            this.colCtrStatusTemp,
            this.colCalcData1,
            this.colCalcData2,
            this.colGlAcctDesc,
            this.colForwardCreateDt,
            this.colReversalCreateDt,
            this.colReversalEmplId,
            this.colReverserName,
            this.colTcdDeviceNo,
            this.colXpPtid,
            this.colIncomingAcctNo,
            this.colIncomingTfrAcctNo,
            this.colProofStatusOnus,
            this.colProofStatusTransit,
            this.colOdpNsfCcAmt,
            this.colColdStorComplete,
            this.colColdStorMessage,
            this.colSharedBranch,
            this.colChannel,
            this.colNonCust,
            this.colTfrTranCode,
            this.colSuspectPtid,
            this.colTypeId,
            this.colPacketId,
            this.colClass,
            this.colInvItemAmt,
            this.colStateTaxAmt,
            this.colLocalTaxAmt,
            this.colTlCaptureTranNo,
            this.colTlCaptureISN,
            this.colTlCaptureBatchPtid,
            this.colTlCaptureBatchId,
            this.colTlCaptureImageCommited,
            this.colTlCaptureWorkstation,
            this.colEmployeeName,
            this.colTlCaptureOptionString,
            this.colTlCaptureOption,
            this.colDepType,
            this.colApplType});
            this.gridJournal.FetchingMessageId = 360982;
            this.gridJournal.IsMaxNumRowsCustomized = false;
            this.gridJournal.LinesInHeader = 3;
            this.gridJournal.Location = new System.Drawing.Point(4, 16);
            this.gridJournal.Name = "gridJournal";
            this.gridJournal.Size = new System.Drawing.Size(676, 240);
            this.gridJournal.TabIndex = 0;
            this.gridJournal.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridJournal_BeforePopulate);
            this.gridJournal.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridJournal_FetchRowDone);
            this.gridJournal.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridJournal_SelectedIndexChanged);
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctNo.PhoenixUIControl.ObjectId = 25;
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.Title = "Column";
            this.colAcctNo.Visible = false;
            this.colAcctNo.Width = 0;
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctType.PhoenixUIControl.ObjectId = 26;
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "Column";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 0;
            // 
            // colAmt
            // 
            this.colAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAmt.PhoenixUIControl.ObjectId = 70;
            this.colAmt.PhoenixUIControl.XmlTag = "NetAmt";
            this.colAmt.Title = "Column";
            this.colAmt.Visible = false;
            this.colAmt.Width = 0;
            // 
            // colBatchId
            // 
            this.colBatchId.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.colBatchId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBatchId.PhoenixUIControl.ObjectId = 27;
            this.colBatchId.PhoenixUIControl.XmlTag = "BatchId";
            this.colBatchId.Title = "Column";
            this.colBatchId.Visible = false;
            this.colBatchId.Width = 0;
            // 
            // colBatchDescription
            // 
            this.colBatchDescription.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBatchDescription.PhoenixUIControl.ObjectId = 28;
            this.colBatchDescription.PhoenixUIControl.XmlTag = "BatchStatusDesc";
            this.colBatchDescription.Title = "Column";
            this.colBatchDescription.Visible = false;
            this.colBatchDescription.Width = 0;
            // 
            // colBatchStatus
            // 
            this.colBatchStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBatchStatus.PhoenixUIControl.ObjectId = 29;
            this.colBatchStatus.PhoenixUIControl.XmlTag = "BatchStatus";
            this.colBatchStatus.Title = "Column";
            this.colBatchStatus.Visible = false;
            this.colBatchStatus.Width = 0;
            // 
            // colCCEnable
            // 
            this.colCCEnable.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCCEnable.PhoenixUIControl.ObjectId = 32;
            this.colCCEnable.PhoenixUIControl.XmlTag = "CcEnable";
            this.colCCEnable.Title = "Column";
            this.colCCEnable.Visible = false;
            this.colCCEnable.Width = 0;
            // 
            // colCCAmt
            // 
            this.colCCAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCCAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCCAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCCAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCCAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCCAmt.PhoenixUIControl.ObjectId = 32;
            this.colCCAmt.PhoenixUIControl.XmlTag = "CcAmt";
            this.colCCAmt.Title = "Column";
            this.colCCAmt.Visible = false;
            this.colCCAmt.Width = 0;
            // 
            // colChksAsCashAmt
            // 
            this.colChksAsCashAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colChksAsCashAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colChksAsCashAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colChksAsCashAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colChksAsCashAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colChksAsCashAmt.PhoenixUIControl.ObjectId = 33;
            this.colChksAsCashAmt.PhoenixUIControl.XmlTag = "ChksAsCash";
            this.colChksAsCashAmt.Title = "Column";
            this.colChksAsCashAmt.Visible = false;
            this.colChksAsCashAmt.Width = 0;
            // 
            // colCloseAmt
            // 
            this.colCloseAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCloseAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCloseAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCloseAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCloseAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCloseAmt.PhoenixUIControl.ObjectId = 34;
            this.colCloseAmt.PhoenixUIControl.XmlTag = "CloseAmt";
            this.colCloseAmt.Title = "Column";
            this.colCloseAmt.Visible = false;
            this.colCloseAmt.Width = 0;
            // 
            // colEmplId
            // 
            this.colEmplId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEmplId.PhoenixUIControl.ObjectId = 36;
            this.colEmplId.PhoenixUIControl.XmlTag = "EmplId";
            this.colEmplId.Title = "Column";
            this.colEmplId.Visible = false;
            this.colEmplId.Width = 0;
            // 
            // colTellerNo
            // 
            this.colTellerNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTellerNo.PhoenixUIControl.ObjectId = 36;
            this.colTellerNo.PhoenixUIControl.XmlTag = "TellerNo";
            this.colTellerNo.Title = "Column";
            this.colTellerNo.Visible = false;
            this.colTellerNo.Width = 0;
            // 
            // colGlCcAcct
            // 
            this.colGlCcAcct.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colGlCcAcct.PhoenixUIControl.ObjectId = 37;
            this.colGlCcAcct.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxx";
            this.colGlCcAcct.Title = "Column";
            this.colGlCcAcct.Visible = false;
            this.colGlCcAcct.Width = 0;
            // 
            // colIntAmt
            // 
            this.colIntAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colIntAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colIntAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colIntAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colIntAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colIntAmt.PhoenixUIControl.ObjectId = 38;
            this.colIntAmt.PhoenixUIControl.XmlTag = "IntAmt";
            this.colIntAmt.Title = "Column";
            this.colIntAmt.Visible = false;
            this.colIntAmt.Width = 0;
            // 
            // colItemCount
            // 
            this.colItemCount.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colItemCount.PhoenixUIControl.ObjectId = 39;
            this.colItemCount.PhoenixUIControl.XmlTag = "ItemCount";
            this.colItemCount.Title = "Column";
            this.colItemCount.Visible = false;
            this.colItemCount.Width = 0;
            // 
            // colMemoPostAmt
            // 
            this.colMemoPostAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colMemoPostAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colMemoPostAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colMemoPostAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colMemoPostAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colMemoPostAmt.PhoenixUIControl.ObjectId = 40;
            this.colMemoPostAmt.PhoenixUIControl.XmlTag = "MemoPostAmt";
            this.colMemoPostAmt.Title = "Column";
            this.colMemoPostAmt.Visible = false;
            this.colMemoPostAmt.Width = 0;
            // 
            // colOnUsChksAmt
            // 
            this.colOnUsChksAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOnUsChksAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOnUsChksAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOnUsChksAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOnUsChksAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOnUsChksAmt.PhoenixUIControl.ObjectId = 41;
            this.colOnUsChksAmt.PhoenixUIControl.XmlTag = "OnUsChks";
            this.colOnUsChksAmt.Title = "Column";
            this.colOnUsChksAmt.Visible = false;
            this.colOnUsChksAmt.Width = 0;
            // 
            // colPenaltyAmt
            // 
            this.colPenaltyAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPenaltyAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPenaltyAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPenaltyAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colPenaltyAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPenaltyAmt.PhoenixUIControl.ObjectId = 42;
            this.colPenaltyAmt.PhoenixUIControl.XmlTag = "PenaltyAmt";
            this.colPenaltyAmt.Title = "Column";
            this.colPenaltyAmt.Visible = false;
            this.colPenaltyAmt.Width = 0;
            // 
            // colPODStatus
            // 
            this.colPODStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPODStatus.PhoenixUIControl.ObjectId = 43;
            this.colPODStatus.PhoenixUIControl.XmlTag = "PodStatus";
            this.colPODStatus.Title = "Column";
            this.colPODStatus.Visible = false;
            this.colPODStatus.Width = 0;
            // 
            // colPTID
            // 
            this.colPTID.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPTID.PhoenixUIControl.ObjectId = 44;
            this.colPTID.PhoenixUIControl.XmlTag = "Ptid";
            this.colPTID.Title = "Column";
            this.colPTID.Visible = false;
            this.colPTID.Width = 0;
            // 
            // colPbUpdated
            // 
            this.colPbUpdated.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPbUpdated.PhoenixUIControl.ObjectId = 80;
            this.colPbUpdated.PhoenixUIControl.XmlTag = "PbUpdated";
            this.colPbUpdated.Title = "Column";
            this.colPbUpdated.Visible = false;
            this.colPbUpdated.Width = 0;
            // 
            // colRealTranCode
            // 
            this.colRealTranCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colRealTranCode.PhoenixUIControl.ObjectId = 45;
            this.colRealTranCode.PhoenixUIControl.XmlTag = "TranCode";
            this.colRealTranCode.Title = "Column";
            this.colRealTranCode.Visible = false;
            this.colRealTranCode.Width = 0;
            // 
            // colReversal
            // 
            this.colReversal.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReversal.PhoenixUIControl.ObjectId = 46;
            this.colReversal.PhoenixUIControl.XmlTag = "Reversal";
            this.colReversal.Title = "Column";
            this.colReversal.Visible = false;
            this.colReversal.Width = 0;
            // 
            // colReversalPrint
            // 
            this.colReversalPrint.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReversalPrint.PhoenixUIControl.ObjectId = 47;
            this.colReversalPrint.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colReversalPrint.Title = "Column";
            this.colReversalPrint.Visible = false;
            this.colReversalPrint.Width = 0;
            // 
            // colRimNo
            // 
            this.colRimNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colRimNo.PhoenixUIControl.ObjectId = 48;
            this.colRimNo.PhoenixUIControl.XmlTag = "RimNo";
            this.colRimNo.Title = "Column";
            this.colRimNo.Visible = false;
            this.colRimNo.Width = 0;
            // 
            // colSuperEmplId
            // 
            this.colSuperEmplId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colSuperEmplId.PhoenixUIControl.ObjectId = 49;
            this.colSuperEmplId.PhoenixUIControl.XmlTag = "SuperEmplId";
            this.colSuperEmplId.Title = "Column";
            this.colSuperEmplId.Visible = false;
            this.colSuperEmplId.Width = 0;
            // 
            // colSupervisor
            // 
            this.colSupervisor.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colSupervisor.PhoenixUIControl.ObjectId = 50;
            this.colSupervisor.PhoenixUIControl.XmlTag = "SuperEmployeeName";
            this.colSupervisor.Title = "Column";
            this.colSupervisor.Visible = false;
            this.colSupervisor.Width = 0;
            // 
            // colSuspectAcct
            // 
            this.colSuspectAcct.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colSuspectAcct.PhoenixUIControl.ObjectId = 51;
            this.colSuspectAcct.PhoenixUIControl.XmlTag = "SuspectAcct";
            this.colSuspectAcct.Title = "Column";
            this.colSuspectAcct.Visible = false;
            this.colSuspectAcct.Width = 0;
            // 
            // colTfrAcctNo
            // 
            this.colTfrAcctNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrAcctNo.PhoenixUIControl.ObjectId = 52;
            this.colTfrAcctNo.PhoenixUIControl.XmlTag = "TfrAcctNo";
            this.colTfrAcctNo.Title = "Column";
            this.colTfrAcctNo.Visible = false;
            this.colTfrAcctNo.Width = 0;
            // 
            // colTfrAcctType
            // 
            this.colTfrAcctType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrAcctType.PhoenixUIControl.ObjectId = 53;
            this.colTfrAcctType.PhoenixUIControl.XmlTag = "TfrAcctType";
            this.colTfrAcctType.Title = "Column";
            this.colTfrAcctType.Visible = false;
            this.colTfrAcctType.Width = 0;
            // 
            // colTfrAccount
            // 
            this.colTfrAccount.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrAccount.PhoenixUIControl.ObjectId = 54;
            this.colTfrAccount.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colTfrAccount.Title = "Column";
            this.colTfrAccount.Visible = false;
            this.colTfrAccount.Width = 0;
            // 
            // colTfrEmplId
            // 
            this.colTfrEmplId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrEmplId.PhoenixUIControl.ObjectId = 55;
            this.colTfrEmplId.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colTfrEmplId.Title = "Column";
            this.colTfrEmplId.Visible = false;
            this.colTfrEmplId.Width = 0;
            // 
            // colTfrPbUpdated
            // 
            this.colTfrPbUpdated.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrPbUpdated.PhoenixUIControl.ObjectId = 81;
            this.colTfrPbUpdated.PhoenixUIControl.XmlTag = "TfrPbUpdated";
            this.colTfrPbUpdated.Title = "Column";
            this.colTfrPbUpdated.Visible = false;
            this.colTfrPbUpdated.Width = 0;
            // 
            // colTransitChksAmt
            // 
            this.colTransitChksAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTransitChksAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTransitChksAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTransitChksAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTransitChksAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTransitChksAmt.PhoenixUIControl.ObjectId = 56;
            this.colTransitChksAmt.PhoenixUIControl.XmlTag = "TransitChks";
            this.colTransitChksAmt.Title = "Column";
            this.colTransitChksAmt.Visible = false;
            this.colTransitChksAmt.Width = 0;
            // 
            // colTranCodePrint
            // 
            this.colTranCodePrint.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranCodePrint.PhoenixUIControl.ObjectId = 57;
            this.colTranCodePrint.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colTranCodePrint.Title = "Column";
            this.colTranCodePrint.Visible = false;
            this.colTranCodePrint.Width = 0;
            // 
            // colTranStatus
            // 
            this.colTranStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranStatus.PhoenixUIControl.ObjectId = 58;
            this.colTranStatus.PhoenixUIControl.XmlTag = "TranStatus";
            this.colTranStatus.Title = "Column";
            this.colTranStatus.Visible = false;
            this.colTranStatus.Width = 0;
            // 
            // colUtilityId
            // 
            this.colUtilityId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUtilityId.PhoenixUIControl.ObjectId = 59;
            this.colUtilityId.PhoenixUIControl.XmlTag = "UtilityId";
            this.colUtilityId.Title = "Column";
            this.colUtilityId.Visible = false;
            this.colUtilityId.Width = 0;
            // 
            // colUtility
            // 
            this.colUtility.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUtility.PhoenixUIControl.ObjectId = 60;
            this.colUtility.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colUtility.Title = "Column";
            this.colUtility.Visible = false;
            this.colUtility.Width = 0;
            // 
            // colUtilityAcctNo
            // 
            this.colUtilityAcctNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUtilityAcctNo.PhoenixUIControl.ObjectId = 61;
            this.colUtilityAcctNo.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colUtilityAcctNo.Title = "Column";
            this.colUtilityAcctNo.Visible = false;
            this.colUtilityAcctNo.Width = 0;
            // 
            // colUtilityAcctType
            // 
            this.colUtilityAcctType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUtilityAcctType.PhoenixUIControl.ObjectId = 62;
            this.colUtilityAcctType.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colUtilityAcctType.Title = "Column";
            this.colUtilityAcctType.Visible = false;
            this.colUtilityAcctType.Width = 0;
            // 
            // colMemoFloat
            // 
            this.colMemoFloat.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colMemoFloat.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colMemoFloat.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colMemoFloat.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colMemoFloat.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colMemoFloat.PhoenixUIControl.XmlTag = "MemoFloat";
            this.colMemoFloat.Title = "Column";
            this.colMemoFloat.Visible = false;
            this.colMemoFloat.Width = 0;
            // 
            // colTranDescription
            // 
            this.colTranDescription.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranDescription.PhoenixUIControl.ObjectId = 86;
            this.colTranDescription.PhoenixUIControl.XmlTag = "TranDescription";
            this.colTranDescription.Title = "Column";
            this.colTranDescription.Visible = false;
            this.colTranDescription.Width = 0;
            // 
            // colReference
            // 
            this.colReference.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReference.PhoenixUIControl.ObjectId = 87;
            this.colReference.PhoenixUIControl.XmlTag = "Reference";
            this.colReference.Title = "Column";
            this.colReference.Visible = false;
            this.colReference.Width = 0;
            // 
            // colExchRateType
            // 
            this.colExchRateType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colExchRateType.PhoenixUIControl.ObjectId = 87;
            this.colExchRateType.PhoenixUIControl.XmlTag = "ExchRateType";
            this.colExchRateType.Title = "Column";
            this.colExchRateType.Visible = false;
            this.colExchRateType.Width = 0;
            // 
            // colEndpoint
            // 
            this.colEndpoint.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEndpoint.PhoenixUIControl.ObjectId = 88;
            this.colEndpoint.PhoenixUIControl.XmlTag = "Endpoint";
            this.colEndpoint.Title = "Column";
            this.colEndpoint.Visible = false;
            this.colEndpoint.Width = 0;
            // 
            // colEndpointDesc
            // 
            this.colEndpointDesc.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEndpointDesc.PhoenixUIControl.ObjectId = 88;
            this.colEndpointDesc.PhoenixUIControl.XmlTag = "EndpointDesc";
            this.colEndpointDesc.Title = "Column";
            this.colEndpointDesc.Visible = false;
            this.colEndpointDesc.Width = 0;
            // 
            // colForceReason
            // 
            this.colForceReason.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colForceReason.PhoenixUIControl.ObjectId = 89;
            this.colForceReason.PhoenixUIControl.XmlTag = "ForceReason";
            this.colForceReason.Title = "Column";
            this.colForceReason.Visible = false;
            this.colForceReason.Width = 0;
            // 
            // colTfrChkNo
            // 
            this.colTfrChkNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTfrChkNo.PhoenixUIControl.ObjectId = 90;
            this.colTfrChkNo.PhoenixUIControl.XmlTag = "TfrChkNo";
            this.colTfrChkNo.Title = "Column";
            this.colTfrChkNo.Visible = false;
            this.colTfrChkNo.Width = 0;
            // 
            // colCrncyId
            // 
            this.colCrncyId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCrncyId.PhoenixUIControl.ObjectId = 91;
            this.colCrncyId.PhoenixUIControl.XmlTag = "CrncyId";
            this.colCrncyId.Title = "Column";
            this.colCrncyId.Visible = false;
            this.colCrncyId.Width = 0;
            // 
            // colExchCrncyId
            // 
            this.colExchCrncyId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colExchCrncyId.PhoenixUIControl.ObjectId = 92;
            this.colExchCrncyId.PhoenixUIControl.XmlTag = "ExchCrncyId";
            this.colExchCrncyId.Title = "Column";
            this.colExchCrncyId.Visible = false;
            this.colExchCrncyId.Width = 0;
            // 
            // colFcCCAmt
            // 
            this.colFcCCAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFcCCAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFcCCAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colFcCCAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colFcCCAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colFcCCAmt.PhoenixUIControl.ObjectId = 91;
            this.colFcCCAmt.PhoenixUIControl.XmlTag = "FcCcAmt";
            this.colFcCCAmt.Title = "Column";
            this.colFcCCAmt.Visible = false;
            this.colFcCCAmt.Width = 0;
            // 
            // colEquivAmt
            // 
            this.colEquivAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colEquivAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colEquivAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colEquivAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colEquivAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEquivAmt.PhoenixUIControl.ObjectId = 93;
            this.colEquivAmt.PhoenixUIControl.XmlTag = "EquivAmt";
            this.colEquivAmt.Title = "Column";
            this.colEquivAmt.Visible = false;
            this.colEquivAmt.Width = 0;
            // 
            // colExchRate
            // 
            this.colExchRate.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colExchRate.PhoenixUIControl.ObjectId = 94;
            this.colExchRate.PhoenixUIControl.XmlTag = "ExchRate";
            this.colExchRate.Title = "Column";
            this.colExchRate.Visible = false;
            this.colExchRate.Width = 0;
            // 
            // colActualRate
            // 
            this.colActualRate.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colActualRate.PhoenixUIControl.ObjectId = 95;
            this.colActualRate.PhoenixUIControl.XmlTag = "ActualRate";
            this.colActualRate.Title = "Column";
            this.colActualRate.Visible = false;
            this.colActualRate.Width = 0;
            // 
            // colExchCrIsoCode
            // 
            this.colExchCrIsoCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colExchCrIsoCode.PhoenixUIControl.ObjectId = 95;
            this.colExchCrIsoCode.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colExchCrIsoCode.Title = "Column";
            this.colExchCrIsoCode.Visible = false;
            this.colExchCrIsoCode.Width = 0;
            // 
            // colCrncyIsoCode
            // 
            this.colCrncyIsoCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCrncyIsoCode.PhoenixUIControl.ObjectId = 91;
            this.colCrncyIsoCode.PhoenixUIControl.XmlTag = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
            this.colCrncyIsoCode.Title = "Column";
            this.colCrncyIsoCode.Visible = false;
            this.colCrncyIsoCode.Width = 0;
            // 
            // colExternalId
            // 
            this.colExternalId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colExternalId.PhoenixUIControl.ObjectId = 101;
            this.colExternalId.PhoenixUIControl.XmlTag = "ExternalId";
            this.colExternalId.Title = "Column";
            this.colExternalId.Visible = false;
            this.colExternalId.Width = 0;
            // 
            // colBatchPTID
            // 
            this.colBatchPTID.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBatchPTID.PhoenixUIControl.ObjectId = 102;
            this.colBatchPTID.PhoenixUIControl.XmlTag = "BatchPtid";
            this.colBatchPTID.Title = "Column";
            this.colBatchPTID.Visible = false;
            this.colBatchPTID.Width = 0;
            // 
            // colAcctClosedDt
            // 
            this.colAcctClosedDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAcctClosedDt.PhoenixUIControl.ObjectId = 114;
            this.colAcctClosedDt.PhoenixUIControl.XmlTag = "AcctCloseDt";
            this.colAcctClosedDt.Title = "Column";
            this.colAcctClosedDt.Visible = false;
            this.colAcctClosedDt.Width = 0;
            // 
            // colUmbCode
            // 
            this.colUmbCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUmbCode.PhoenixUIControl.ObjectId = 114;
            this.colUmbCode.PhoenixUIControl.XmlTag = "UmbCode";
            this.colUmbCode.Title = "Column";
            this.colUmbCode.Visible = false;
            this.colUmbCode.Width = 0;
            // 
            // colUmbCodeDescription
            // 
            this.colUmbCodeDescription.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colUmbCodeDescription.PhoenixUIControl.ObjectId = 114;
            this.colUmbCodeDescription.PhoenixUIControl.XmlTag = "UmbCodeDescription";
            this.colUmbCodeDescription.Title = "Column";
            this.colUmbCodeDescription.Visible = false;
            this.colUmbCodeDescription.Width = 0;
            // 
            // colOrigPenaltyAmt
            // 
            this.colOrigPenaltyAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colOrigPenaltyAmt.PhoenixUIControl.ObjectId = 114;
            this.colOrigPenaltyAmt.PhoenixUIControl.XmlTag = "OrigPenaltyAmt";
            this.colOrigPenaltyAmt.Title = "Column";
            this.colOrigPenaltyAmt.Visible = false;
            this.colOrigPenaltyAmt.Width = 0;
            // 
            // colCashCount
            // 
            this.colCashCount.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCashCount.PhoenixUIControl.XmlTag = "CashCount";
            this.colCashCount.Title = "Column";
            this.colCashCount.Visible = false;
            this.colCashCount.Width = 0;
            // 
            // colTCDDrawerPosition
            // 
            this.colTCDDrawerPosition.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTCDDrawerPosition.PhoenixUIControl.XmlTag = "TcdDrawerPositn";
            this.colTCDDrawerPosition.Title = "Column";
            this.colTCDDrawerPosition.Visible = false;
            this.colTCDDrawerPosition.Width = 0;
            // 
            // colTCDDrawerNo
            // 
            this.colTCDDrawerNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTCDDrawerNo.PhoenixUIControl.XmlTag = "TcdDrawerNo";
            this.colTCDDrawerNo.Title = "Column";
            this.colTCDDrawerNo.Visible = false;
            this.colTCDDrawerNo.Width = 0;
            // 
            // colPayroll
            // 
            this.colPayroll.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colPayroll.PhoenixUIControl.ObjectId = 109;
            this.colPayroll.PhoenixUIControl.XmlTag = "PayRoll";
            this.colPayroll.Title = "Column";
            this.colPayroll.Visible = false;
            this.colPayroll.Width = 0;
            // 
            // colBranchNo
            // 
            this.colBranchNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colBranchNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colBranchNo.PhoenixUIControl.ObjectId = 130;
            this.colBranchNo.PhoenixUIControl.XmlTag = "BranchNo";
            this.colBranchNo.Title = "Branch No";
            this.colBranchNo.Visible = false;
            this.colBranchNo.Width = 40;
            // 
            // colTellerDrawerNo
            // 
            this.colTellerDrawerNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTellerDrawerNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.colTellerDrawerNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTellerDrawerNo.PhoenixUIControl.ObjectId = 106;
            this.colTellerDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colTellerDrawerNo.Title = "Drawer No";
            this.colTellerDrawerNo.Visible = false;
            this.colTellerDrawerNo.Width = 40;
            // 
            // colReversalDisplay
            // 
            this.colReversalDisplay.PhoenixUIControl.ObjectId = 64;
            this.colReversalDisplay.PhoenixUIControl.XmlTag = "ReversalDisplay";
            this.colReversalDisplay.Title = "Rev";
            this.colReversalDisplay.Width = 28;
            // 
            // colCtrStatus
            // 
            this.colCtrStatus.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCtrStatus.PhoenixUIControl.ObjectId = 109;
            this.colCtrStatus.PhoenixUIControl.XmlTag = "CtrStatus";
            this.colCtrStatus.Title = "CTR Status";
            this.colCtrStatus.Width = 55;
            // 
            // colAggregated
            // 
            this.colAggregated.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colAggregated.PhoenixUIControl.ObjectId = 118;
            this.colAggregated.PhoenixUIControl.XmlTag = "CtrFlag";
            this.colAggregated.Title = "Aggregated";
            this.colAggregated.Width = 64;
            // 
            // colTCDDevice
            // 
            this.colTCDDevice.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTCDDevice.PhoenixUIControl.ObjectId = 107;
            this.colTCDDevice.PhoenixUIControl.XmlTag = "DeviceName";
            this.colTCDDevice.Title = "Column";
            this.colTCDDevice.Visible = false;
            this.colTCDDevice.Width = 0;
            // 
            // colSequenceNo
            // 
            this.colSequenceNo.PhoenixUIControl.ObjectId = 65;
            this.colSequenceNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.colSequenceNo.Title = "Seq#";
            this.colSequenceNo.Width = 35;
            // 
            // colOfflineSeqNo
            // 
            this.colOfflineSeqNo.PhoenixUIControl.ObjectId = 134;
            this.colOfflineSeqNo.PhoenixUIControl.XmlTag = "OfflineSequenceNo";
            this.colOfflineSeqNo.Title = "Off Seq #";
            this.colOfflineSeqNo.Width = 0;
            // 
            // colSubSequence
            // 
            this.colSubSequence.PhoenixUIControl.ObjectId = 85;
            this.colSubSequence.PhoenixUIControl.XmlTag = "SubSequence";
            this.colSubSequence.Title = "Sub Seq#";
            this.colSubSequence.Width = 32;
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 66;
            this.colAccount.PhoenixUIControl.XmlTag = "Account";
            this.colAccount.Title = "Account";
            this.colAccount.Width = 111;
            // 
            // colCustomerName
            // 
            this.colCustomerName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCustomerName.PhoenixUIControl.ObjectId = 119;
            this.colCustomerName.PhoenixUIControl.XmlTag = "CustomerName";
            this.colCustomerName.Title = "Account Name/ Account Description";
            this.colCustomerName.Visible = false;
            this.colCustomerName.Width = 135;
            // 
            // colTranCodeDisp
            // 
            this.colTranCodeDisp.PhoenixUIControl.ObjectId = 67;
            this.colTranCodeDisp.PhoenixUIControl.XmlTag = "TlTranCode";
            this.colTranCodeDisp.Title = "TC";
            this.colTranCodeDisp.Width = 45;
            // 
            // colTranCode
            // 
            this.colTranCode.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.Null;
            this.colTranCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranCode.Title = "TC";
            this.colTranCode.Visible = false;
            this.colTranCode.Width = 0;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDescription.PhoenixUIControl.ObjectId = 68;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 148;
            // 
            // colDescription1
            // 
            this.colDescription1.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDescription1.PhoenixUIControl.ObjectId = 68;
            this.colDescription1.PhoenixUIControl.XmlTag = "Description1";
            this.colDescription1.Title = "Description";
            this.colDescription1.Visible = false;
            this.colDescription1.Width = 148;
            // 
            // colTranEffectiveDt
            // 
            this.colTranEffectiveDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTranEffectiveDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colTranEffectiveDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colTranEffectiveDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colTranEffectiveDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTranEffectiveDt.PhoenixUIControl.ObjectId = 120;
            this.colTranEffectiveDt.PhoenixUIControl.XmlTag = "TranEffectiveDt";
            this.colTranEffectiveDt.Title = "Transaction Effective Date";
            this.colTranEffectiveDt.Visible = false;
            // 
            // colNetAmt
            // 
            this.colNetAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNetAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNetAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNetAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNetAmt.PhoenixUIControl.ObjectId = 83;
            this.colNetAmt.PhoenixUIControl.XmlTag = "NetAmtx";
            this.colNetAmt.Title = "Transaction Amount";
            this.colNetAmt.Width = 85;
            // 
            // colCashInAmt
            // 
            this.colCashInAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashInAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashInAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashInAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashInAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCashInAmt.PhoenixUIControl.ObjectId = 30;
            this.colCashInAmt.PhoenixUIControl.XmlTag = "CashIn";
            this.colCashInAmt.Title = "Cash In Amount";
            this.colCashInAmt.Visible = false;
            this.colCashInAmt.Width = 85;
            // 
            // colCashOutAmt
            // 
            this.colCashOutAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashOutAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashOutAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colCashOutAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colCashOutAmt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCashOutAmt.PhoenixUIControl.ObjectId = 31;
            this.colCashOutAmt.PhoenixUIControl.XmlTag = "CashOut";
            this.colCashOutAmt.Title = "Cash Out Amount";
            this.colCashOutAmt.Visible = false;
            this.colCashOutAmt.Width = 85;
            // 
            // colTCDCashIn
            // 
            this.colTCDCashIn.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashIn.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashIn.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashIn.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashIn.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTCDCashIn.PhoenixUIControl.ObjectId = 135;
            this.colTCDCashIn.PhoenixUIControl.XmlTag = "TcdCashIn";
            this.colTCDCashIn.Title = "Column";
            this.colTCDCashIn.Visible = false;
            this.colTCDCashIn.Width = 0;
            // 
            // colTCDCashOut
            // 
            this.colTCDCashOut.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashOut.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashOut.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTCDCashOut.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colTCDCashOut.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTCDCashOut.PhoenixUIControl.ObjectId = 136;
            this.colTCDCashOut.PhoenixUIControl.XmlTag = "TcdCashOut";
            this.colTCDCashOut.Title = "Column";
            this.colTCDCashOut.Visible = false;
            this.colTCDCashOut.Width = 0;
            // 
            // colEffectiveDt
            // 
            this.colEffectiveDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colEffectiveDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colEffectiveDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colEffectiveDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.colEffectiveDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEffectiveDt.PhoenixUIControl.ObjectId = 63;
            this.colEffectiveDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.colEffectiveDt.Title = "Posting Date";
            this.colEffectiveDt.Visible = false;
            // 
            // colCreateDt
            // 
            this.colCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colCreateDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCreateDt.PhoenixUIControl.ObjectId = 35;
            this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
            this.colCreateDt.Title = "Entered Date";
            this.colCreateDt.Visible = false;
            // 
            // colLocalDtTime
            // 
            this.colLocalDtTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLocalDtTime.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colLocalDtTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.colLocalDtTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
            this.colLocalDtTime.PhoenixUIControl.ObjectId = 142;
            this.colLocalDtTime.PhoenixUIControl.XmlTag = "LocalCreateDt";
            this.colLocalDtTime.Title = "Local Date";
            this.colLocalDtTime.Visible = false;
            // 
            // colLocalTZ
            // 
            this.colLocalTZ.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocalTZ.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colLocalTZ.PhoenixUIControl.ObjectId = 143;
            this.colLocalTZ.PhoenixUIControl.XmlTag = "LocalTimeZone";
            this.colLocalTZ.Title = "Local Time Zone";
            this.colLocalTZ.Visible = false;
            this.colLocalTZ.Width = 4;
            // 
            // colCheckNo
            // 
            this.colCheckNo.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCheckNo.PhoenixUIControl.ObjectId = 69;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.Title = "Check# Drawer# #Of Items ";
            this.colCheckNo.Width = 59;
            // 
            // colIsoCode
            // 
            this.colIsoCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colIsoCode.PhoenixUIControl.ObjectId = 84;
            this.colIsoCode.PhoenixUIControl.XmlTag = "IsoCode";
            this.colIsoCode.Title = "Iso Code";
            this.colIsoCode.Visible = false;
            this.colIsoCode.Width = 0;
            // 
            // colReferenceAcct
            // 
            this.colReferenceAcct.PhoenixUIControl.ObjectId = 110;
            this.colReferenceAcct.PhoenixUIControl.XmlTag = "ReferenceAcct";
            this.colReferenceAcct.Title = "Reference Account";
            this.colReferenceAcct.Width = 111;
            // 
            // colJournalDescription
            // 
            this.colJournalDescription.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colJournalDescription.PhoenixUIControl.ObjectId = 68;
            this.colJournalDescription.PhoenixUIControl.XmlTag = "Descriptionx";
            this.colJournalDescription.Title = "Description";
            this.colJournalDescription.Visible = false;
            this.colJournalDescription.Width = 0;
            // 
            // colNetAmtTemp
            // 
            this.colNetAmtTemp.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNetAmtTemp.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNetAmtTemp.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colNetAmtTemp.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colNetAmtTemp.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colNetAmtTemp.PhoenixUIControl.XmlTag = "NetAmtTemp";
            this.colNetAmtTemp.Title = "Transaction Amount";
            this.colNetAmtTemp.Visible = false;
            this.colNetAmtTemp.Width = 0;
            // 
            // colDecision
            // 
            this.colDecision.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colDecision.PhoenixUIControl.ObjectId = 116;
            this.colDecision.PhoenixUIControl.XmlTag = "Decision";
            this.colDecision.Title = "Decision";
            this.colDecision.Visible = false;
            this.colDecision.Width = 0;
            // 
            // colLastName
            // 
            this.colLastName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colLastName.PhoenixUIControl.XmlTag = "LastName";
            this.colLastName.Title = "Last Name";
            this.colLastName.Visible = false;
            this.colLastName.Width = 0;
            // 
            // colFirstName
            // 
            this.colFirstName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colFirstName.PhoenixUIControl.XmlTag = "FirstName";
            this.colFirstName.Title = "First Name";
            this.colFirstName.Visible = false;
            this.colFirstName.Width = 0;
            // 
            // colMiddleInitial
            // 
            this.colMiddleInitial.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colMiddleInitial.PhoenixUIControl.XmlTag = "MiddleInitial";
            this.colMiddleInitial.Title = "Middle Initial";
            this.colMiddleInitial.Visible = false;
            this.colMiddleInitial.Width = 0;
            // 
            // colTitleId
            // 
            this.colTitleId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colTitleId.PhoenixUIControl.XmlTag = "TitleId";
            this.colTitleId.Title = "Title Id";
            this.colTitleId.Visible = false;
            this.colTitleId.Width = 0;
            // 
            // colRimType
            // 
            this.colRimType.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colRimType.PhoenixUIControl.XmlTag = "RimType";
            this.colRimType.Title = "Rim Type";
            this.colRimType.Visible = false;
            this.colRimType.Width = 0;
            // 
            // colCtrStatusTemp
            // 
            this.colCtrStatusTemp.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCtrStatusTemp.PhoenixUIControl.XmlTag = "CtrStatusx";
            this.colCtrStatusTemp.Title = "Temp Ctr Status";
            this.colCtrStatusTemp.Visible = false;
            this.colCtrStatusTemp.Width = 0;
            // 
            // colCalcData1
            // 
            this.colCalcData1.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCalcData1.PhoenixUIControl.XmlTag = "CalcData1";
            this.colCalcData1.Title = "Column";
            this.colCalcData1.Visible = false;
            // 
            // colCalcData2
            // 
            this.colCalcData2.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colCalcData2.PhoenixUIControl.XmlTag = "CalcData2";
            this.colCalcData2.Title = "Column";
            this.colCalcData2.Visible = false;
            // 
            // colGlAcctDesc
            // 
            this.colGlAcctDesc.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colGlAcctDesc.PhoenixUIControl.XmlTag = "GlAcctDesc";
            this.colGlAcctDesc.Title = "GlAcctDesc";
            this.colGlAcctDesc.Visible = false;
            // 
            // colForwardCreateDt
            // 
            this.colForwardCreateDt.PhoenixUIControl.XmlTag = "ForwardCreateDt";
            this.colForwardCreateDt.Title = "Column";
            this.colForwardCreateDt.Visible = false;
            this.colForwardCreateDt.Width = 0;
            // 
            // colReversalCreateDt
            // 
            this.colReversalCreateDt.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReversalCreateDt.PhoenixUIControl.ObjectId = 131;
            this.colReversalCreateDt.PhoenixUIControl.XmlTag = "ReversalCreateDt";
            this.colReversalCreateDt.Title = "ReversalCreateDt";
            this.colReversalCreateDt.Visible = false;
            this.colReversalCreateDt.Width = 0;
            // 
            // colReversalEmplId
            // 
            this.colReversalEmplId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReversalEmplId.PhoenixUIControl.ObjectId = 132;
            this.colReversalEmplId.PhoenixUIControl.XmlTag = "ReversalEmplId";
            this.colReversalEmplId.Title = "ReversalEmplId";
            this.colReversalEmplId.Visible = false;
            this.colReversalEmplId.Width = 0;
            // 
            // colReverserName
            // 
            this.colReverserName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colReverserName.PhoenixUIControl.ObjectId = 133;
            this.colReverserName.PhoenixUIControl.XmlTag = "xxxxxxxx";
            this.colReverserName.Title = "ReverserName";
            this.colReverserName.Visible = false;
            this.colReverserName.Width = 0;
            // 
            // colTcdDeviceNo
            // 
            this.colTcdDeviceNo.PhoenixUIControl.XmlTag = "TcdDeviceNo";
            this.colTcdDeviceNo.Title = "Column";
            this.colTcdDeviceNo.Visible = false;
            // 
            // colXpPtid
            // 
            this.colXpPtid.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colXpPtid.PhoenixUIControl.ObjectId = 73;
            this.colXpPtid.PhoenixUIControl.XmlTag = "XpPtid";
            this.colXpPtid.Title = "Column";
            this.colXpPtid.Visible = false;
            this.colXpPtid.Width = 0;
            // 
            // colIncomingAcctNo
            // 
            this.colIncomingAcctNo.PhoenixUIControl.ObjectId = 137;
            this.colIncomingAcctNo.PhoenixUIControl.XmlTag = "IncomingAcctNo";
            this.colIncomingAcctNo.Title = "Incoming Account";
            // 
            // colIncomingTfrAcctNo
            // 
            this.colIncomingTfrAcctNo.PhoenixUIControl.ObjectId = 138;
            this.colIncomingTfrAcctNo.PhoenixUIControl.XmlTag = "IncomingTfrAcctNo";
            this.colIncomingTfrAcctNo.Title = "Incoming Transfer Account";
            // 
            // colProofStatusOnus
            // 
            this.colProofStatusOnus.PhoenixUIControl.XmlTag = "ProofStatusOnus";
            this.colProofStatusOnus.Title = "Column";
            this.colProofStatusOnus.Visible = false;
            // 
            // colProofStatusTransit
            // 
            this.colProofStatusTransit.PhoenixUIControl.XmlTag = "ProofStatusTransit";
            this.colProofStatusTransit.Title = "Column";
            this.colProofStatusTransit.Visible = false;
            // 
            // colOdpNsfCcAmt
            // 
            this.colOdpNsfCcAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOdpNsfCcAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOdpNsfCcAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colOdpNsfCcAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colOdpNsfCcAmt.PhoenixUIControl.XmlTag = "OdpNsfCcAmt";
            this.colOdpNsfCcAmt.Title = "Column";
            this.colOdpNsfCcAmt.Visible = false;
            this.colOdpNsfCcAmt.Width = 0;
            // 
            // colColdStorComplete
            // 
            this.colColdStorComplete.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colColdStorComplete.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colColdStorComplete.PhoenixUIControl.XmlTag = "ColdStorComplete";
            this.colColdStorComplete.Title = "Cold Store Complete";
            this.colColdStorComplete.Visible = false;
            this.colColdStorComplete.Width = 0;
            // 
            // colColdStorMessage
            // 
            this.colColdStorMessage.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colColdStorMessage.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colColdStorMessage.PhoenixUIControl.XmlTag = "ColdStorMessage";
            this.colColdStorMessage.Title = "Cold Store Message";
            this.colColdStorMessage.Visible = false;
            this.colColdStorMessage.Width = 0;
            // 
            // colSharedBranch
            // 
            this.colSharedBranch.PhoenixUIControl.XmlTag = "SharedBranch";
            this.colSharedBranch.Title = "Shared Branch";
            this.colSharedBranch.Visible = false;
            // 
            // colChannel
            // 
            this.colChannel.PhoenixUIControl.ObjectId = 144;
            this.colChannel.PhoenixUIControl.XmlTag = "ChannelDescription";
            this.colChannel.Title = "Channel";
            // 
            // colNonCust
            // 
            this.colNonCust.PhoenixUIControl.XmlTag = "NonCust";
            this.colNonCust.Title = "Column";
            this.colNonCust.Visible = false;
            // 
            // colTfrTranCode
            // 
            this.colTfrTranCode.PhoenixUIControl.XmlTag = "TfrTranCode";
            this.colTfrTranCode.Title = "TfrTranCode";
            this.colTfrTranCode.Visible = false;
            this.colTfrTranCode.Width = 20;
            // 
            // colSuspectPtid
            // 
            this.colSuspectPtid.PhoenixUIControl.ObjectId = 150;
            this.colSuspectPtid.PhoenixUIControl.XmlTag = "SuspectPtid";
            this.colSuspectPtid.Title = "Suspect Ptid";
            this.colSuspectPtid.Visible = false;
            this.colSuspectPtid.Width = 30;
            // 
            // colTypeId
            // 
            this.colTypeId.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colTypeId.PhoenixUIControl.XmlTag = "TypeId";
            this.colTypeId.Title = "Column";
            this.colTypeId.Visible = false;
            // 
            // colPacketId
            // 
            this.colPacketId.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colPacketId.PhoenixUIControl.XmlTag = "PacketId";
            this.colPacketId.Title = "Column";
            this.colPacketId.Visible = false;
            // 
            // colClass
            // 
            this.colClass.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.colClass.PhoenixUIControl.XmlTag = "Class";
            this.colClass.Title = "Column";
            this.colClass.Visible = false;
            // 
            // colInvItemAmt
            // 
            this.colInvItemAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colInvItemAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colInvItemAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colInvItemAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colInvItemAmt.PhoenixUIControl.XmlTag = "InvItemAmt";
            this.colInvItemAmt.Title = "Column";
            this.colInvItemAmt.Visible = false;
            // 
            // colStateTaxAmt
            // 
            this.colStateTaxAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStateTaxAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStateTaxAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colStateTaxAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colStateTaxAmt.PhoenixUIControl.XmlTag = "StateTaxAmt";
            this.colStateTaxAmt.Title = "Column";
            this.colStateTaxAmt.Visible = false;
            // 
            // colLocalTaxAmt
            // 
            this.colLocalTaxAmt.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLocalTaxAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLocalTaxAmt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.colLocalTaxAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colLocalTaxAmt.PhoenixUIControl.XmlTag = "LocalTaxAmt";
            this.colLocalTaxAmt.Title = "Column";
            this.colLocalTaxAmt.Visible = false;
            // 
            // colTlCaptureTranNo
            // 
            this.colTlCaptureTranNo.PhoenixUIControl.ObjectId = 154;
            this.colTlCaptureTranNo.PhoenixUIControl.XmlTag = "TlCaptureTranNo";
            this.colTlCaptureTranNo.Title = "";
            this.colTlCaptureTranNo.Visible = false;
            // 
            // colTlCaptureISN
            // 
            this.colTlCaptureISN.PhoenixUIControl.ObjectId = 155;
            this.colTlCaptureISN.PhoenixUIControl.XmlTag = "TlCaptureIsn";
            this.colTlCaptureISN.Title = "ISN #";
            this.colTlCaptureISN.Visible = false;
            // 
            // colTlCaptureBatchPtid
            // 
            this.colTlCaptureBatchPtid.PhoenixUIControl.ObjectId = 156;
            this.colTlCaptureBatchPtid.PhoenixUIControl.XmlTag = "TlCaptureBatchPtid";
            this.colTlCaptureBatchPtid.Title = "";
            this.colTlCaptureBatchPtid.Visible = false;
            // 
            // colTlCaptureBatchId
            // 
            this.colTlCaptureBatchId.PhoenixUIControl.ObjectId = 157;
            this.colTlCaptureBatchId.PhoenixUIControl.XmlTag = "TlCaptureBatchId";
            this.colTlCaptureBatchId.Title = "";
            this.colTlCaptureBatchId.Visible = false;
            // 
            // colTlCaptureImageCommited
            // 
            this.colTlCaptureImageCommited.PhoenixUIControl.ObjectId = 158;
            this.colTlCaptureImageCommited.PhoenixUIControl.XmlTag = "TlCaptureImageCommited";
            this.colTlCaptureImageCommited.Title = "";
            this.colTlCaptureImageCommited.Visible = false;
            // 
            // colTlCaptureWorkstation
            // 
            this.colTlCaptureWorkstation.PhoenixUIControl.ObjectId = 159;
            this.colTlCaptureWorkstation.PhoenixUIControl.XmlTag = "TlCaptureWorkstation";
            this.colTlCaptureWorkstation.Title = "";
            // 
            // colEmployeeName
            // 
            this.colEmployeeName.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colEmployeeName.PhoenixUIControl.ObjectId = 82;
            this.colEmployeeName.PhoenixUIControl.XmlTag = "EmployeeName";
            this.colEmployeeName.Title = "Column";
            // 
            // colTlCaptureOptionString
            // 
            this.colTlCaptureOptionString.PhoenixUIControl.ObjectId = 160;
            this.colTlCaptureOptionString.PhoenixUIControl.XmlTag = "";
            this.colTlCaptureOptionString.Title = "Teller Capture Option";
            this.colTlCaptureOptionString.Visible = false;
            // 
            // colTlCaptureOption
            // 
            this.colTlCaptureOption.PhoenixUIControl.ObjectId = 161;
            this.colTlCaptureOption.PhoenixUIControl.XmlTag = "TlCaptureOptionType";
            this.colTlCaptureOption.Title = "Teller Capture Option";
            this.colTlCaptureOption.Visible = false;
            // 
            // colDepType
            // 
            this.colDepType.PhoenixUIControl.XmlTag = "DepType";
            this.colDepType.Title = "Column";
            this.colDepType.Visible = false;
            // 
            // colApplType
            // 
            this.colApplType.PhoenixUIControl.XmlTag = "ApplType";
            this.colApplType.Title = "Column";
            this.colApplType.Visible = false;
            // 
            // dfTranCount
            // 
            this.dfTranCount.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTranCount.Location = new System.Drawing.Point(628, 264);
            this.dfTranCount.Name = "dfTranCount";
            this.dfTranCount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfTranCount.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfTranCount.PhoenixUIControl.ObjectId = 71;
            this.dfTranCount.PhoenixUIControl.XmlTag = "TranCount";
            this.dfTranCount.PreviousValue = null;
            this.dfTranCount.ReadOnly = true;
            this.dfTranCount.Size = new System.Drawing.Size(50, 13);
            this.dfTranCount.TabIndex = 4;
            this.dfTranCount.TabStop = false;
            this.dfTranCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTotalofTransactionsFetched
            // 
            this.lblTotalofTransactionsFetched.AutoEllipsis = true;
            this.lblTotalofTransactionsFetched.Location = new System.Drawing.Point(444, 260);
            this.lblTotalofTransactionsFetched.Name = "lblTotalofTransactionsFetched";
            this.lblTotalofTransactionsFetched.PhoenixUIControl.ObjectId = 71;
            this.lblTotalofTransactionsFetched.Size = new System.Drawing.Size(175, 20);
            this.lblTotalofTransactionsFetched.TabIndex = 3;
            this.lblTotalofTransactionsFetched.Text = "Total # of Transactions Fetched:";
            // 
            // dfHiddenSupOverride
            // 
            this.dfHiddenSupOverride.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfHiddenSupOverride.Location = new System.Drawing.Point(176, 264);
            this.dfHiddenSupOverride.Name = "dfHiddenSupOverride";
            this.dfHiddenSupOverride.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfHiddenSupOverride.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
            this.dfHiddenSupOverride.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Show;
            this.dfHiddenSupOverride.PhoenixUIControl.ObjectId = 80;
            this.dfHiddenSupOverride.PhoenixUIControl.XmlTag = "SupervisorOverride";
            this.dfHiddenSupOverride.PreviousValue = null;
            this.dfHiddenSupOverride.ReadOnly = true;
            this.dfHiddenSupOverride.Size = new System.Drawing.Size(0, 13);
            this.dfHiddenSupOverride.TabIndex = 2;
            this.dfHiddenSupOverride.TabStop = false;
            // 
            // lblHiddenSupervisorOverride
            // 
            this.lblHiddenSupervisorOverride.AutoEllipsis = true;
            this.lblHiddenSupervisorOverride.Location = new System.Drawing.Point(4, 260);
            this.lblHiddenSupervisorOverride.Name = "lblHiddenSupervisorOverride";
            this.lblHiddenSupervisorOverride.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Show;
            this.lblHiddenSupervisorOverride.PhoenixUIControl.ObjectId = 80;
            this.lblHiddenSupervisorOverride.Size = new System.Drawing.Size(153, 20);
            this.lblHiddenSupervisorOverride.TabIndex = 1;
            this.lblHiddenSupervisorOverride.Text = "* = Supervisor Override";
            // 
            // pbDisplay
            // 
            this.pbDisplay.LongText = "&Display...";
            this.pbDisplay.Name = "&Display...";
            this.pbDisplay.ObjectId = 74;
            this.pbDisplay.ShortText = "&Display...";
            this.pbDisplay.Tag = null;
            this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
            // 
            // pbItemCapture
            // 
            this.pbItemCapture.LongText = "&Item Capt...";
            this.pbItemCapture.Name = "&Item Capt...";
            this.pbItemCapture.ObjectId = 75;
            this.pbItemCapture.ShortText = "&Item Capt...";
            this.pbItemCapture.Tag = null;
            this.pbItemCapture.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbItemCapture_Click);
            // 
            // pbReversal
            // 
            this.pbReversal.LongText = "&Reversal...";
            this.pbReversal.Name = "&Reversal...";
            this.pbReversal.ObjectId = 76;
            this.pbReversal.ShortText = "&Reversal...";
            this.pbReversal.Tag = null;
            this.pbReversal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReversal_Click);
            // 
            // pbReprint
            // 
            this.pbReprint.LongText = "Re&print...";
            this.pbReprint.Name = "Re&print...";
            this.pbReprint.ObjectId = 77;
            this.pbReprint.ShortText = "Re&print...";
            this.pbReprint.Tag = null;
            this.pbReprint.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReprint_Click);
            // 
            // pbTranTotals
            // 
            this.pbTranTotals.LongText = "Tran Totals...";
            this.pbTranTotals.Name = "Tran Totals...";
            this.pbTranTotals.ObjectId = 78;
            this.pbTranTotals.ShortText = "Tran Totals...";
            this.pbTranTotals.Tag = null;
            this.pbTranTotals.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTranTotals_Click);
            // 
            // pbSearch
            // 
            this.pbSearch.LongText = "&Search...";
            this.pbSearch.Name = "&Search...";
            this.pbSearch.ObjectId = 79;
            this.pbSearch.Shortcut = System.Windows.Forms.Keys.End;
            this.pbSearch.ShortText = "&Search...";
            this.pbSearch.Tag = null;
            this.pbSearch.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSearch_Click);
            // 
            // pbAcctDisplay
            // 
            this.pbAcctDisplay.LongText = "&Acct Display...";
            this.pbAcctDisplay.Name = "&Acct Display...";
            this.pbAcctDisplay.ObjectId = 127;
            this.pbAcctDisplay.ShortText = "&Acct Display...";
            this.pbAcctDisplay.Tag = null;
            this.pbAcctDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbAcctDisplay_Click);
            // 
            // pbEndorsement
            // 
            this.pbEndorsement.LongText = "E&ndorsement";
            this.pbEndorsement.Name = "E&ndorsement";
            this.pbEndorsement.ObjectId = 146;
            this.pbEndorsement.ShortText = "E&ndorsement";
            this.pbEndorsement.Tag = null;
            this.pbEndorsement.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEndorsement_Click);
            // 
            // pbPrintMailer
            // 
            this.pbPrintMailer.LongText = "Print Mailer...";
            this.pbPrintMailer.Name = "Print Mailer...";
            this.pbPrintMailer.ObjectId = 148;
            this.pbPrintMailer.ShortText = "Print Mailer...";
            this.pbPrintMailer.Tag = null;
            this.pbPrintMailer.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPrintMailer_Click);
            // 
            // pbSuspectDtls
            // 
            this.pbSuspectDtls.LongText = "Suspt Dtl ";
            this.pbSuspectDtls.Name = "Suspt Dtl ";
            this.pbSuspectDtls.ObjectId = 149;
            this.pbSuspectDtls.ShortText = "Suspt Dtl ";
            this.pbSuspectDtls.Tag = null;
            this.pbSuspectDtls.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSuspectDtls_Click);
            // 
            // pbInventory
            // 
            this.pbInventory.LongText = "In&ventory...";
            this.pbInventory.Name = null;
            this.pbInventory.ObjectId = 152;
            this.pbInventory.Tag = null;
            this.pbInventory.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbInventory_Click);
            // 
            // pbBondDetails
            // 
            this.pbBondDetails.LongText = "B&ond Details...";
            this.pbBondDetails.Name = "pbBondDetails";
            this.pbBondDetails.ObjectId = 153;
            this.pbBondDetails.Tag = null;
            this.pbBondDetails.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbBondDetails_Click);
            // 
            // pbImage
            // 
            this.pbImage.LongText = "I&mage";
            this.pbImage.Name = null;
            this.pbImage.ObjectId = 162;
            this.pbImage.Tag = null;
            this.pbImage.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbImage_Click);
            // 
            // pbViewReceipt
            // 
            this.pbViewReceipt.LongText = "&View Receipt";
            this.pbViewReceipt.Name = "pbViewReceipt";
            this.pbViewReceipt.ObjectId = 165;
            this.pbViewReceipt.ShortText = "&View Receipt";
            this.pbViewReceipt.Tag = null;
            this.pbViewReceipt.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.PbViewReceipt_Click);
            // 
            // frmTlJournal
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbTransactionHistory);
            this.Controls.Add(this.gbTransactionCriteria);
            this.Controls.Add(this.gbBranchDrawerandStatusCriteria);
            this.Name = "frmTlJournal";
            this.ScreenId = 10443;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlJournal_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlJournal_PInitCompleteEvent);
            this.PMdiPrintEvent += new System.EventHandler(this.frmTlJournal_PMdiPrintEvent);
            this.Load += new System.EventHandler(this.frmTlJournal_Load);
            this.gbBranchDrawerandStatusCriteria.ResumeLayout(false);
            this.gbTransactionCriteria.ResumeLayout(false);
            this.gbTransactionCriteria.PerformLayout();
            this.gbTransactionHistory.ResumeLayout(false);
            this.gbTransactionHistory.PerformLayout();
            this.ResumeLayout(false);

		}
        #endregion

        #region Init Param
        public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 5 && paramList.Length <= 9) //#72916 - Phase 4 changed from 8 to 9
			{
				isSupervisor.Value = Convert.ToString(paramList[0]);
				branchNo.Value = Convert.ToInt16(paramList[1]);
				drawerNo.Value = Convert.ToInt16(paramList[2]);
				postingDate.Value = Convert.ToDateTime(paramList[3]);
				drawerStatus.Value = Convert.ToString(paramList[4]);
				//
				if (paramList.Length == 6)
				{
					if (paramList[5] != null)
						drawerCombo = paramList[5] as PComboBoxStandard;
				}
				if (paramList.Length == 7)
				{
					if (paramList[6] != null)
						isReviewCtr = Convert.ToBoolean(paramList[6]);

				}
				if (paramList.Length == 8)
				{
					if (paramList[7] != null)
						drCurPostingDate.Value = Convert.ToDateTime(paramList[7]);

				}

				if (paramList.Length == 9)
				{
					if (paramList[8] != null)
					{
						if (Convert.ToInt32(paramList[8]) == 16015)
						{
							isOriginTcdAdmin = true;
						}
						else
						{
							isOriginTcdAdmin = false;
						}
					}
				}
			}

			//Begin #79510
			if (paramList.Length == 3)  // for shared branch call from mdi
			{
				drawerStatus.Value = Convert.ToString(paramList[0]);
				drawerCombo = paramList[1] as PComboBoxStandard;
				acquirerResponseXml = Convert.ToString(paramList[2]);
			}
			//End #79510

			#region #79314 #12769
			if (paramList.Length == 1)
			{
				drawerCombo = new PComboBoxStandard();
				_tranInfoPtid = Convert.ToDecimal(paramList[0]);
			}
			#endregion

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.Journal;
			this.CurrencyId = _tellerVars.LocalCrncyId;

			base.InitParameters(paramList);
		}

		#endregion

		#region FetchEditData
		public override void OnFetchEditData()
		{
			if (MainBusinesObject != null)
			{
				_busObjTlJournal.BranchNo.Value = (branchNo.Value == -1 ? _tellerVars.BranchNo : branchNo.Value);
				_busObjTlJournal.DrawerNo.Value = drawerNo.Value;
				_busObjTlJournal.IsIncludeSupervisor.Value = isSupervisor.Value;
				MainBusinesObject.ActionType = XmActionType.EnumOnly;
				MainBusinesObject.EnumType = XmEnumerationType.EnumAll;
				this.PerformAction(XmActionType.EnumOnly);
				_busObjTlJournal.TranStatus.Value = -1;  // Workaround
				_busObjTlJournal.PodStatus.Value = -1; // Workaround
				_busObjTlJournal.EffectiveDt.IsNullable = true; // to prevent validation on from/to effective dt.
				this.IsNew = true;
			}

		}
		#endregion

		#region Call Parent
		public override void CallParent(params object[] paramList)
		{
			string caller = "";
            string callerrefresh = "";/*Task#110967*/
            if (paramList != null && paramList[0] != null)
				caller = Convert.ToString(paramList[0]);
            /*Begin Task#110967*/
            if (paramList != null && paramList[1] != null)
                callerrefresh = Convert.ToString(paramList[1]);
            /*End Task#110967*/
            if (caller == "Override")
			{
				if (paramList.Length > 1)
				{
					if (Convert.ToBoolean(paramList[1]) == true)
						dialogResult = DialogResult.OK;
					else
						dialogResult = DialogResult.Cancel;

					if (dialogResult == DialogResult.OK)
					{
						if (isCtrDeferrelOverride)
						{
							ctrRevSuperEmplId = _tlJournalOvrd.SuperEmplId.Value;
							if (!_tlJournalOvrd.MessageId.IsNull)
								messageId = _tlJournalOvrd.MessageId.Value;
							if (!_tlJournalOvrd.OvrdStatus.IsNull)
								ovrdStatus = _tlJournalOvrd.OvrdStatus.Value;
							isCtrDeferrelOvrdSuccess = true;
						}
						else
						{
							revSuperEmplId = _tlJournalOvrd.SuperEmplId.Value;
							if (!_tlJournalOvrd.MessageId.IsNull)
								messageId = _tlJournalOvrd.MessageId.Value;
							if (!_tlJournalOvrd.OvrdStatus.IsNull)
								ovrdStatus = _tlJournalOvrd.OvrdStatus.Value;
							isReversalOvrdSuccess = true;
						}
					}
					else
					{
						if (isCtrDeferrelOverride)
							isCtrDeferrelOvrdSuccess = false;
						else
							isReversalOvrdSuccess = false;
					}
				}
				//
				if (isCtrDeferrelOvrdRequired && !isRevesalOvrdRequired)
					HandleReversalOvrd();
				else
					HandleReversal();
			}
			/*begin 18900*/
			else if (callerrefresh == "RefreshTellerJournalGrid")/*Task#110967*/
			{
				PopulateGrid();
			}
			/* end 18900*/
			else
			{
				//base.CallParent( paramList );
			}
		}
		#endregion

		#region events
		private ReturnType frmTlJournal_PInitBeginEvent()
		{
			try
			{
                //Begin #75604
                #region config
                PdfCurrency.ApplicationAssumeDecimal = (_tellerVars.AdTlControl.AssumeDecimals.Value == GlobalVars.Instance.ML.Y);
                CurrencyHelper.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
                this.AssumeDecimal = PdfCurrency.ApplicationAssumeDecimal; //#6588
                #endregion
                //End #75604
                this.AppToolBarId = AppToolBarType.NoEdit;
				this.MainBusinesObject = _busObjTlJournal;
				this.pbDisplay.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalDisplay;
				this.pbSuspectDtls.NextScreenId = Phoenix.Shared.Constants.ScreenId.SuspiciousTranDetails; // #80660
				this.pbTranTotals.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalTranTotals;
				this.CurrencyId = _tellerVars.LocalCrncyId;
				this.colBatchId.PhoenixUIControl.IsNullable = NullabilityState.NotNull;
				this.cmbBatchItem.PhoenixUIControl.IsNullable = NullabilityState.NotNull;
				this._busObjTlJournal.BatchId.IsNullable = false;
                this.pbBondDetails.NextScreenId = Phoenix.Shared.Constants.ScreenId.RedemptionHistory; //#140798
                                                                                                       //#71734 - commented the line below.
                                                                                                       //this.gridJournal.ListViewObject =  _busObjTlJournal; // Just se the column Properties
                this.pbViewReceipt.NextScreenId = 0; //3698; //119792   //#118298 - TBD - Resolve the next screen ID when the Phoenix window is available

                #region grid default
                this.gridJournal.DoubleClickAction = this.pbDisplay;
				#endregion

				if (IsRemoteOverrideEnabled()) //#12769
				{
					_isSupervisorViewOnlyMode = true;

					_tranInfo = new TlOvrdTranInfo();
					_tranInfo.Ptid.Value = _tranInfoPtid;
					_tranInfo.ActionType = XmActionType.Select;
					//
					CallCDSProcess(_tranInfo);
					//
					this.branchNo.Value = _tranInfo.BranchNo.Value;
					this.drawerNo.Value = _tranInfo.DrawerNo.Value;
				}

				#region set window title
				if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
				{
					/* #Begin #72916 - Issue fix
					 *
					 * this.NewRecordTitle = string.Format(this.NewRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(360081));
					   this.EditRecordTitle = string.Format(this.EditRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(360081));
					 */
					if (!isOriginTcdAdmin)
					{
						this.NewRecordTitle = string.Format(this.NewRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(360081));
						this.EditRecordTitle = string.Format(this.EditRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(360081));
					}
					else
					{
						this.NewRecordTitle = string.Format(this.NewRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(361097));
						this.EditRecordTitle = string.Format(this.EditRecordTitle, Convert.ToString(drawerNo.Value) + " - " + CoreService.Translation.GetUserMessageX(361097));
					}
				}
				else
				{
					this.NewRecordTitle = string.Format(this.NewRecordTitle, Convert.ToString(drawerNo.Value));
					this.EditRecordTitle = string.Format(this.EditRecordTitle, Convert.ToString(drawerNo.Value));
				}
				#endregion

				#region populate combo box
				this.AutoFetch = true;
				this.IsNew = true;
				#endregion

				if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
				{
					this.cmbDrawers.DisplayType = UIComboDisplayType.Description;
				}
				else
					this.cmbDrawers.DisplayType = UIComboDisplayType.CombinedValue;
				SetFormDefaults();

				#region Fix No Security
				this.pbSearch.NextScreenId = 0;
				#endregion

				#region #76409
				this._noCopies = new PDecimal("NoCopies");
				this._noCopies.Value = 1;
				#endregion

				//Begin #79510
				_tlTranSet.IsAcquirerTran = !string.IsNullOrEmpty(acquirerResponseXml);
				//End #79510
				//Begin #79510
				if (_tlTranSet.IsAcquirerTran)
					GetAcquirerTranSet();
				//End #79510

			}
			catch (PhoenixException pebegin)
			{
				PMessageBox.Show(pebegin);
			}
			return ReturnType.Success;
		}

		private void frmTlJournal_PInitCompleteEvent()
		{
			try
			{

				if (this.isSupervisor.Value == GlobalVars.Instance.ML.Y)
				{
					this.lblDrawerTeller.Text = CoreService.Translation.GetUserMessageX(360082);	// Drawer:
					this.cmbBranch.Append(-1, GlobalVars.Instance.ML.All);
					this.cmbDrawers.Append(-1, GlobalVars.Instance.ML.All);
				}
				this.cmbMiscBranch.Append(-1, GlobalVars.Instance.ML.All);  //#79510, #09368
				EnableDisableVisibleLogic("FormComplete");

				EnableDisableVisibleLogic("CrossRefAccounts");  //#4450
				#region #80615
				// Get Mailer Form info....
				_adTlForm.OutputTypeId.Value = 4;
				MailerForm = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(_adTlForm);
				if (MailerForm != null && MailerForm.Count > 0)
				{
					foreach (Phoenix.BusObj.Admin.Teller.AdTlForm adtlform in MailerForm)
					{
						MailerStatus = adtlform.Status.Value;
					}
				}
				#endregion
				//hack
				cmbAcctType.CodeValue = null;
				PopulateComboBox("AcctType");
				FixAmountFields();
				//
				//#69357 - Fix for Review CTR
				if (isReviewCtr)
				{
					this.cmbCTRStatus.CodeValue = CoreService.Translation.GetListItemX(ListId.CTRStatus, "All CTRs");
					this.cmbCTRStatus.Description = CoreService.Translation.GetListItemX(ListId.CTRStatus, "All CTRs");
				}
				#region #76409
				// #3590 - commented default of 'All'
				//this.cmbProofStatus.CodeValue = CoreService.Translation.GetListItemX(ListId.ProofStatus, "All");
				//this.cmbProofStatus.Description = CoreService.Translation.GetListItemX(ListId.ProofStatus, "All");
				#endregion
				//
				this.gridJournal.ListViewObject = _busObjTlJournal; // Just se the column Properties

				//Begin #79510
				if (_tlTranSet.IsAcquirerTran)
				{
					this.cbMisc.SetValue(true);
					this.cmbMiscSrch.SetValue(CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Sequence Number"));
					this.dfSeqNoFrom.SetValue(_tlTranSet.SequenceNo.Value);
					this.dfSeqNoTo.SetValue(_tlTranSet.SequenceNo.Value);
				}
				//End #79510

				if (_isSupervisorViewOnlyMode)  //#79314
					EnableDisableVisibleLogic("SupervisorViewOnlyMode");

				JournalSearch();
				if (gridJournal.Items.Count > 0)
				{
					gridJournal.Focus();
					gridJournal.SelectRow(0);
					#region #80615
					if (colRimNo.Text.Trim() != "" && MailerForm.Count > 0 && MailerStatus != "Closed" && MailerStatus != "Inactive" && colNonCust.Text.Trim() != "Y") // #12870
						this.pbPrintMailer.SetObjectStatus(VisibilityState.Show, EnableState.Enable);
					else
						this.pbPrintMailer.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					#endregion
				}
				#region #79510 -
				PopulateComboBox("Misc");
				//PopulateComboBox("Channel");

				#endregion

				//Begin #79510
				this.PShowCompletedEvent += new EventHandler(frmTlJournal_PShowCompletedEvent);
				//End #79510
			}
			catch (PhoenixException pecomp)
			{
				PMessageBox.Show(pecomp);
			}
		}

		//Begin #79510
		void frmTlJournal_PShowCompletedEvent(object sender, EventArgs e)
		{
			if (_tlTranSet.IsAcquirerTran)
			{
				//WI#12941
				HandleRevFromInterpro();
				UpdateDrawerBalance();
				if (colColdStorComplete.UnFormattedValue != null && colColdStorComplete.Text.Trim() == GlobalVars.Instance.ML.Y)
				{
					ReverseVoucher();
				}
                //Begin 121336
                else if (_tlTranSet.TellerVars.IsECMVoucherAvailable)
                {
                    if (colSubSequence.UnFormattedValue != null && Convert.ToInt32(colSubSequence.UnFormattedValue) > 0)
                    {
                        gridJournal.RowToObject(_busObjTlJournal);

                        if (_busObjTlJournal.IsECMReceiptArchived())
                        {
                            ReverseVoucher();
                        }
                    }
                }
                //End 121336

                this.Close();
			}
		}
		//End #79510
		//WI#12941
		private void HandleRevFromInterpro()
		{
			int tcdTcrMessageId = (_tellerVars.IsRecycler ? Convert.ToInt32(13288) : Convert.ToInt32(361075));
			string inputMsg = "";
			decimal totalCashInOutAmt = 0;
			decimal tcrCashInOutJrnlPtid = 0;
			bool isTcdCashOut = true;
			if (colTranCode.UnFormattedValue.ToString() != "BAT")
			{
				if (colSubSequence.UnFormattedValue != null && Convert.ToInt16(colSubSequence.UnFormattedValue) > 0)
				{
					gridJournal.RowToObject(_busObjTlJournal);
					isTcdCashOut = true;
					_busObjTlJournal.GetTotalTcdCashInOut(isTcdCashOut, ref totalCashInOutAmt, ref tcrCashInOutJrnlPtid);

					TcrCashOutJournalPtid.Value = tcrCashInOutJrnlPtid;
					TcrTotalCashOutAmt.Value = totalCashInOutAmt;
				}
				else
				{
					TcrTotalCashOutAmt.Value = (this.colTCDCashOut.UnFormattedValue != null ? Convert.ToDecimal(this.colTCDCashOut.UnFormattedValue) : 0);
					TcrCashOutJournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
				}

				if (TcrTotalCashOutAmt.Value > 0 && tcdTcrMessageId > 0 && (!_tellerVars.IsRecycler || (_tellerVars.IsRecycler && (!IsTcrConnected() || _isTcdTcrDeviceIdDifferentFromTran))))   //#11783
				{
					inputMsg = (_tellerVars.IsRecycler ? string.Format("This transaction had a TCR cash out for {0} and the TCR is either not available or the drawer is not connected to the TCR that processed the TCR Dispense. You must post a GL Credit using the TCD/TCR Cash GL with a Cash In amount equal to {0}.", CurrencyHelper.GetFormattedValue(TcrTotalCashOutAmt.Value)) : CurrencyHelper.GetFormattedValue(TcrTotalCashOutAmt.Value));
					//361075 - This transaction had a TCD cash out for %1!. You must post a GL Credit using the TCD Vault Cash GL with a Cash In amount equal to %1!.
					//13288 - This transaction had a TCR cash out for %1!. You must post a GL Credit using the TCR Vault Cash GL with a TCR Cash In amount equal to %1!.
					//PMessageBox.Show(this, 361075, MessageType.Warning, MessageBoxButtons.OK, this.colTCDCashOut.UnFormattedValue.ToString());
					PMessageBox.Show(this, tcdTcrMessageId, MessageType.Warning, MessageBoxButtons.OK, new string[] { inputMsg });
				}
				inputMsg = "";
			}
		} //end WI12941

		private void gridJournal_BeforePopulate(object sender, GridPopulateArgs e)
		{
			_rowCount = 0;
		}

		private void gridJournal_FetchRowDone(object sender, GridRowArgs e)
		{
			try
			{
				_rowCount++;

				#region Amount
				colNetAmt.UnFormattedValue = colAmt.UnFormattedValue;
				#endregion

				#region acct and desc
				string tempAcctDesc = "";
				string exAcctNoMask = ""; //#76458
				string exTfrAcctNoMask = "";    //#76458
				//
				//Begin #76458
				if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 520 ||
					Convert.ToInt32(colRealTranCode.UnFormattedValue) == 570)
				{
					exAcctNoMask = _globalHelper.GetMaskedExtAcct(colAcctType.Text, colAcctNo.Text);
				}
				//End #76458
				colDescription1.Text = colDescription.Text;
				colTranCode.Text = colTranCodeDisp.Text;
				jrnlTranDesc = colTranCode.Text;
				if (jrnlTranDesc != "" && colDescription.Text != "" && colDescription.Text != string.Empty)
					jrnlTranDesc = jrnlTranDesc + " - " + colDescription.Text;

				colAccount.Text = this._busObjTlJournal.GetAccountDesc(colAcctType.Text, (exAcctNoMask == "" ? colAcctNo.Text : exAcctNoMask), Convert.ToInt32(colRimNo.UnFormattedValue)); //#76458

				if (colTfrAcctType.Text != "" && colTfrAcctType.Text != string.Empty && colTfrAcctType.Text != null)
				{

					//Begin #76458
					if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 102 ||
						Convert.ToInt32(colRealTranCode.UnFormattedValue) == 163)
					{
						if (_tellerVars.SetContextObject("AdExAcctTypeNode", colTfrAcctType.Text))
							exTfrAcctNoMask = _globalHelper.GetMaskedExtAcct(colTfrAcctType.Text, colTfrAcctNo.Text);

					}
					//End #76458

					tempAcctDesc = this._busObjTlJournal.GetTfrAccountDesc(colTfrAcctType.Text, (exTfrAcctNoMask == "" ? colTfrAcctNo.Text : exTfrAcctNoMask), Convert.ToInt16(colRealTranCode.UnFormattedValue)); //#76458

					if (tempAcctDesc != "")
					{
						colDescription.Text = colDescription.Text + " " + tempAcctDesc;
						colDescription1.Text = colDescription.Text;
					}
				}
				#endregion

				#region revesal flag
				if (colReversal.Text == "2")
				{
					colReversalDisplay.Text = GlobalVars.Instance.ML.Y;
					colReversalPrint.Text = CoreService.Translation.GetUserMessageX(360091);	// Reversed
				}
				#endregion

				#region trancode desc
				string tlTrandesc = "";
				short realTranCode = 0;
				if (_tellerHelper.IsMiscTran(colTranCode.Text))
				{
					this._busObjTlJournal.GetTellerTranCodeDesc(colTranCode.Text, out tlTrandesc, out realTranCode);
					colDescription.Text = tlTrandesc;
					colDescription1.Text = tlTrandesc;
					colRealTranCode.UnFormattedValue = realTranCode;
				}

				#region Generic Tran Desc
				if (_tellerHelper.IsGenericTran(colTranCode.Text))
				{
					colDescription.Text = colTranCode.Text + " - " + colTranDescription.Text;
					colDescription1.Text = colDescription.Text;
				}
				#endregion

				if (colDescription.Text != "" && colDescription.Text != string.Empty && colDescription.Text != null)
					colTranCodePrint.Text = colTranCode.Text + " - " + colDescription.Text;
				else
					colTranCodePrint.Text = colTranCode.Text;
				#endregion

				#region batch desc
				if (colBatchId.Text != null && colBatchId.Text != string.Empty)
				{
					colDescription.Text = colBatchDescription.Text + " - " + colDescription.Text;
					colDescription1.Text = colDescription.Text;
				}
				#endregion

				#region CTR
				colCtrStatus.Text = this._busObjTlJournal.GetCtrDescFromCtrStatus(colCtrStatus.Text, colAggregated.Text);
				#endregion

				#region payroll
				if (colPayroll.Text == "Y")
					colPayroll.Text = CoreService.Translation.GetUserMessageX(360103);	// Yes
				else
					colPayroll.Text = CoreService.Translation.GetUserMessageX(360102);	// No
				#endregion

				#region misc
				if (Convert.ToInt32(colCrncyId.UnFormattedValue) > 0)
				{
					colIsoCode.Text = "USD";
					this.CurrencyId = Convert.ToInt32(colCrncyId.Text);
				}
				//#71644
				if (colSupervisor.Text != "" && colSupervisor.Text != null && colSupervisor.Text != string.Empty && colSuperEmplId.Text != "0" &&
					colTranCode.UnFormattedValue != null && "ABC~ACI~ACO~CLC~CLO".IndexOf(colTranCode.UnFormattedValue.ToString()) < 0)
				{
					tempSuperEmpl = colTranCodeDisp.Text;
					colTranCodeDisp.Text = tempSuperEmpl + "*";
				}

				if (colDescription.Text == null || colDescription.Text == "" || colDescription.Text == string.Empty)
					colDescription.Text = colJournalDescription.Text;

				#region #76409
				if (colDescription.Text == null || colDescription.Text == "" || colDescription.Text == string.Empty)
					colDescription.Text = colTranDescription.Text;
				#endregion

				#endregion

				#region aggregated
				if (colAggregated.Text == "A")
					colAggregated.Text = CoreService.Translation.GetUserMessageX(360103);	// Yes
				else
					colAggregated.Text = CoreService.Translation.GetUserMessageX(360102);	// No
				#endregion

				#region Customer Name
				if (this.cbCustomerName.Checked)
				{
					string tempCustName = "";

					if ((colLastName.Text != string.Empty && colLastName.Text != "" && colLastName.Text != null) ||
						(colFirstName.Text != string.Empty && colFirstName.Text != "" && colFirstName.Text != null) ||
						(colGlAcctDesc.Text != string.Empty && colGlAcctDesc.Text != "" && colGlAcctDesc.Text != null))
					{
						if (_tellerHelper.IsGLTran(Convert.ToInt16(colRealTranCode.UnFormattedValue)))
							colCustomerName.Text = colGlAcctDesc.Text;
						else
						{
							if (colRimType.Text == CoreService.Translation.GetListItemX(ListId.PersonalNonPersonal, "NonPersonal"))
							{
								tempCustName = colLastName.Text;
								if (tempCustName == "")
									tempCustName = colFirstName.Text;
								else
									tempCustName = tempCustName + "," + " " + colFirstName.Text;
							}
							else
							{
								tempCustName = colLastName.Text;
								if (tempCustName == "")
									tempCustName = colFirstName.Text;
								else
									tempCustName = tempCustName + "," + " " + colFirstName.Text + " " + colMiddleInitial.Text;
							}
							//
							colCustomerName.Text = tempCustName;
						}
					}
				}
				#endregion

				#region forwarded transactions
				if (colSuperEmplId.Text == "0" && colForwardCreateDt.Text != String.Empty)
					e.CurrentRow.ForeColor = System.Drawing.Color.Red;
				#endregion

				#region Reference Acct
				if (colReferenceAcct.Text == "-")
				{
					colReferenceAcct.Text = string.Empty;
				}
				#endregion

				/* Begin #72916 */
				#region MachineID

				if (colTCDDrawerNo.UnFormattedValue != null &&
					Convert.ToInt16(colTCDDrawerNo.UnFormattedValue) > 0 &&
					_tcddrawerlist.IndexOf(colTCDDrawerNo.UnFormattedValue.ToString()) == -1)
				{
                    // Begin - 175838
                    //_tlDrawer.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
                    //_tlDrawer.DrawerNo.Value = Convert.ToInt16(colTCDDrawerNo.UnFormattedValue);

                    //_tlDrawer.ActionType = XmActionType.Select;
                    //_tlDrawer.SelectAllFields = false;
                    //_tlDrawer.TcdDeviceNo.Selected = true;
                    //CallXMThruCDS("TlDrawerMachineID");
                   

					//colTcdDeviceNo.Text = _tlDrawer.TcdDeviceNo.Value.ToString();
					_tcddrawerlist = _tcddrawerlist + "~" + Convert.ToString(colTCDDrawerNo.UnFormattedValue);
					if (_tcdDeviceNoList == "")
						//_tcdDeviceNoList = _tcdDeviceNoList + Convert.ToString(colTCDDrawerNo.UnFormattedValue) + '^' + _tlDrawer.TcdDeviceNo.Value.ToString();
                         _tcdDeviceNoList = _tcdDeviceNoList + Convert.ToString(colTCDDrawerNo.UnFormattedValue) + '^' + Convert.ToString(colTcdDeviceNo.UnFormattedValue);
					else
						//_tcdDeviceNoList = _tcdDeviceNoList + "~" + Convert.ToString(colTCDDrawerNo.UnFormattedValue) + '^' + _tlDrawer.TcdDeviceNo.Value.ToString();
                    _tcdDeviceNoList = _tcdDeviceNoList + "~" + Convert.ToString(colTCDDrawerNo.UnFormattedValue) + '^' + Convert.ToString(colTcdDeviceNo.UnFormattedValue);
                    // End - 175838
				}
				//
				if (colTCDDrawerNo.UnFormattedValue != null &&
					Convert.ToInt16(colTCDDrawerNo.UnFormattedValue) > 0 && _tcdDeviceNoList != "")
				{
					string[] deviceTemp2;
					if (_tcdDeviceNoList.IndexOf(Convert.ToString(colTCDDrawerNo.UnFormattedValue)) >= 0)
					{
						string[] deviceTemp = _tcdDeviceNoList.Split('~');
						if (deviceTemp.Length > 0)
						{
							foreach (String device in deviceTemp)
							{
								if (device != "" && device != string.Empty)
								{
									deviceTemp2 = device.Split('^');
									if (deviceTemp2[0].IndexOf(Convert.ToString(colTCDDrawerNo.UnFormattedValue)) >= 0)
									{
										colTcdDeviceNo.Text = deviceTemp2[1];
										break;
									}
								}
							}
						}
					}
				}
				#endregion
				/* End #72916 */

				#region #79510
				if (colSharedBranch.Text == "Y")
					e.CurrentRow.ForeColor = Color.Blue;
				#endregion

				// Begin #140895 - Teller Capture Integration
				if (colTlCaptureOption.Text == "S")
				{
					colTlCaptureOptionString.Text = "Single Related";
				}
				else if (colTlCaptureOption.Text == "B")
				{
					colTlCaptureOptionString.Text = "Bulk Unrelated";
				}
				// End #140895 - Teller Capture Integration

			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void gridJournal_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			if (gridJournal.ContextRow == -1 && gridJournal.Count > 0)
				gridJournal.SelectRow(0, false);
			JouranlGridClick();
		}

		private void cmbAcctType_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			try
			{
				SetMask(cmbAcctType, dfAccount);
				SetAcctNoField(dfAccount, cmbAcctType);
				this.dfAccount.Focus();
				this.cmbAcctType.ScreenToObject();
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void cmbMiscSrch_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			cmbMiscSrch.ScreenToObject();
			EnableDisableVisibleLogic("MiscSrchClick");
		}

		private void cbAccount_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("AccountClick");
		}

		private void cbEffectiveDate_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("EffectiveDtClick");
		}

		private void cbDate_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("EnteredDtClick");
		}

		#region #79569

		void cbLocalDateTime_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("LocalDtClick");
		}
		#endregion


		#region pbSuspectDtls_Click - #80660
		void pbSuspectDtls_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("SuspectDetail");
		}
		#endregion

		private void cbAmt_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("AmtClick");
		}

		private void cbMisc_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("MiscClick");
		}

		private void cbCustomerName_Click(object sender, EventArgs e)
		{
			if (!cbCustomerName.Checked)
				this.colCustomerName.Visible = false;
		}

		private void cbTranEffectiveDt_Click(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("TranEffectiveDtClick");
		}

		private void dfAccount_Validating(object sender, CancelEventArgs e)
		{
			SetAcctNoField(dfAccount, cmbAcctType);
		}

		private void cmbAcctType_PhoenixUIEditedEvent(object sender, ValueEditedEventArgs e)
		{
			if (dfAccount.Text != string.Empty && dfAccount.Text != null)
				dfAccount.Text = string.Empty;
		}

		private void pbTranTotals_Click(object sender, PActionEventArgs e)
		{
			#region totals form
			frmTlJournalTranTotals JournalTranTotals = new frmTlJournalTranTotals();
			JournalTranTotals.InitParameters(Convert.ToInt16(this.cmbBranch.CodeValue), Convert.ToInt16(cmbDrawers.CodeValue), postingDate.Value);
			JournalTranTotals.Workspace = this.Workspace;
			JournalTranTotals.ParentGrid = this.gridJournal;
			JournalTranTotals.Show();
			#endregion
		}

		private void pbDisplay_Click(object sender, PActionEventArgs e)
		{
			PopUpDisplay(Convert.ToInt16(colRealTranCode.UnFormattedValue));
		}

		private void pbSearch_Click(object sender, PActionEventArgs e)
		{
			JournalSearch();
			if (gridJournal.Items.Count > 0)
			{
				gridJournal.Focus();
				if (gridJournal.ContextRow >= 0)
					gridJournal.SelectRow(gridJournal.ContextRow);
				else
					gridJournal.SelectRow(0, true);
				JouranlGridClick();
			}
		}

		private void cmbAmtType_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			this.cmbAmtType.ScreenToObject();
			EnableDisableVisibleLogic("AmtClick");
		}

		private void pbAcctDisplay_Click(object sender, PActionEventArgs e)
		{

			if (colForwardCreateDt.UnFormattedValue != null)
			{
				if (Convert.ToDateTime(colForwardCreateDt.UnFormattedValue) != DateTime.MinValue
					&& Convert.ToInt16(colSuperEmplId.UnFormattedValue) == 0)
				{
					if (!_globalHelper.ValidateAccount(colAcctType.Text, colAcctNo.Text))
					{
						PMessageBox.Show(this, 360735, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						return;
					}
				}
			}
			//#72358 - control screen access through message instead of enable/disable
			#region handle acct display enable/disable
			//70185 -
			tempAcctType = Convert.ToString(colAcctType.UnFormattedValue);
			tempAcctNo = Convert.ToString(colAcctNo.UnFormattedValue);
			tempRimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
			tempNonCust = Convert.ToString(colNonCust.UnFormattedValue);	//#9311, CR#8075

			bool isRimAccessAllowed = true;
			CallXMThruCDS("GridClick");
			//
			//#9311 - avoid restriction check cause there is no restrict_id for non cust (for cust its rm_acct.restrict_id, debug CallXMThruCDS("GridClick");)
			if ((_tellerVars.DebugSecurity || _adRmRestrict.RestrictLevel.Value < _tellerVars.EmplRestrictLevel) && tempNonCust != "Y")
			{
				if (SetContext())
				{
					if (AdGbRsmRim.Edit.Value != GlobalVars.Instance.ML.Y)
						isRimAccessAllowed = false;
				}
				else
					isRimAccessAllowed = false;
				//
				if (!isRimAccessAllowed)
				{
					PMessageBox.Show(this, 319018, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					return;
				}
			}
			PopUpAcctDisplay();
			#endregion
		}

		//Mile2
		private void pbReversal_Click(object sender, PActionEventArgs e)
		{
			decimal totalCashInOutAmt = 0;
			decimal tcrCashInOutJrnlPtid = 0;
			bool isTcdCashOut = true;
			_isTcdTcrDeviceIdDifferentFromTran = (colTcdDeviceNo.UnFormattedValue == null || (Convert.ToInt32(colTcdDeviceNo.UnFormattedValue) != _tellerVars.TcdMachineId));   //11784
			//
			TcrCashInJournalPtid.Value = 0;
			TcrCashOutJournalPtid.Value = 0;
			TcrTotalCashInAmt.Value = 0;
			TcrTotalCashOutAmt.Value = 0;

			//Begin #79510, #10883
			isCtrDeferrelOvrdSuccess = true;
			isReversalOvrdSuccess = true;
			ctrRevSuperEmplId = 0;
			revSuperEmplId = 0;
			messageId = 0;
			ovrdStatus = 0;
			//End #79510, #10883

			if (_tellerVars.IsTCDEnabled && _isTCDConnected)    //#9470
			{
				if (!IsPendingTcrCashTranCompleted())
					return;
			}

			#region check drawer closed out
			if (_tellerVars.ClosedOut)
			{
				PMessageBox.Show(this, 360566, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return;
			}
			#endregion
			//
			#region Reversal Qs
			if (DialogResult.No == PMessageBox.Show(this, 311140, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
				return;

			//Begin #79510, #10883
			if (colSharedBranch.Text == GlobalVars.Instance.ML.Y)
			{
				//13331 - Shared Branch transactions must be reversed using Interpro else it may cause out of balance. In case the transaction has been already reversed on Interpro side, you may proceed with a supervisor override. Do you want to proceed ?
				if (DialogResult.No == PMessageBox.Show(this, 13331, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
					return;
			}
			//End #79510, #10883

			if (Convert.ToInt16(colSubSequence.UnFormattedValue) > 0)
			{
				if (DialogResult.No == PMessageBox.Show(this, 314794, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
					return;
			}
			//#9470
			_isTransactionRevSuccess = false;

			/* Begin #72916 - Phase 2 */
			//Begin #74662
			//#10036 - added TCD/TCR device specific message text
			int tcdTcrMessageId = (_tellerVars.IsRecycler ? Convert.ToInt32(13288) : Convert.ToInt32(361075));
			string inputMsg = "";

			if (colTranCode.UnFormattedValue.ToString() != "BAT")
			{
				if (colSubSequence.UnFormattedValue != null && Convert.ToInt16(colSubSequence.UnFormattedValue) > 0)
				{
					gridJournal.RowToObject(_busObjTlJournal);
					isTcdCashOut = true;
					_busObjTlJournal.GetTotalTcdCashInOut(isTcdCashOut, ref totalCashInOutAmt, ref tcrCashInOutJrnlPtid);
					//
					TcrCashOutJournalPtid.Value = tcrCashInOutJrnlPtid;
					TcrTotalCashOutAmt.Value = totalCashInOutAmt;
				}
				else
				{
					TcrTotalCashOutAmt.Value = (this.colTCDCashOut.UnFormattedValue != null ? Convert.ToDecimal(this.colTCDCashOut.UnFormattedValue) : 0);
					TcrCashOutJournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
				}

				if (TcrTotalCashOutAmt.Value > 0 && tcdTcrMessageId > 0 && (!_tellerVars.IsRecycler || (_tellerVars.IsRecycler && (!IsTcrConnected() || _isTcdTcrDeviceIdDifferentFromTran))))   //#11784
				{
					inputMsg = (_tellerVars.IsRecycler ? string.Format("This transaction had a TCR cash out for {0} and the TCR is either not available or the drawer is not connected to the TCR that processed the TCR Dispense. You must post a GL Credit using the TCD/TCR Cash GL with a Cash In amount equal to {0}.", CurrencyHelper.GetFormattedValue(TcrTotalCashOutAmt.Value)) : CurrencyHelper.GetFormattedValue(TcrTotalCashOutAmt.Value));
					//361075 - This transaction had a TCD cash out for %1!. You must post a GL Credit using the TCD Vault Cash GL with a Cash In amount equal to %1!.
					//13288 - This transaction had a TCR cash out for %1!. You must post a GL Credit using the TCR Vault Cash GL with a TCR Cash In amount equal to %1!.
					//PMessageBox.Show(this, 361075, MessageType.Warning, MessageBoxButtons.OK, this.colTCDCashOut.UnFormattedValue.ToString());
					PMessageBox.Show(this, tcdTcrMessageId, MessageType.Warning, MessageBoxButtons.OK, new string[] { inputMsg });
				}
				inputMsg = "";  //#12836
			}
			//Begin #1157
			if (Convert.ToInt16(colRealTranCode.UnFormattedValue) > 0 && (Convert.ToInt16(colRealTranCode.UnFormattedValue) == 300 ||
				Convert.ToInt16(colRealTranCode.UnFormattedValue) == 301 || Convert.ToInt16(colRealTranCode.UnFormattedValue) == 304))
			{
				//Begin #04953
				TlCc TlCc = new TlCc();
				TlCc.JournalPtid.Value = Convert.ToInt32(colPTID.UnFormattedValue);
				if (TlCc.LoanChargeExists())
				{
					//End #04953
					//11408 - You are reversing a loan fees payment, which may cause assessed loan fees assessed to be reversed.
					if (DialogResult.Cancel == PMessageBox.Show(this, 11408, MessageType.Warning, MessageBoxButtons.OKCancel, string.Empty))
						return;
				}
			}
			//End #1157

			#region #79574
			if (colSubSequence.UnFormattedValue != null && Convert.ToInt16(colSubSequence.UnFormattedValue) > 0)
			{
				gridJournal.RowToObject(_busObjTlJournal);
				totalCashInOutAmt = 0;
				tcrCashInOutJrnlPtid = 0;
				isTcdCashOut = false;
				_busObjTlJournal.GetTotalTcdCashInOut(isTcdCashOut, ref totalCashInOutAmt, ref tcrCashInOutJrnlPtid);
				//
				TcrCashInJournalPtid.Value = tcrCashInOutJrnlPtid;
				TcrTotalCashInAmt.Value = totalCashInOutAmt;
			}
			else
			{
				TcrTotalCashInAmt.Value = (this.colTCDCashIn.UnFormattedValue != null ? Convert.ToDecimal(this.colTCDCashIn.UnFormattedValue) : 0);
				TcrCashInJournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
			}
			//
			if (TcrTotalCashInAmt.Value > 0 && _tellerVars.IsRecycler) //#11784 //#12836
			{
				inputMsg = string.Format("This transaction had a TCR cash in for {0} and the TCR is either not available or the drawer is not connected to the TCR that processed the TCR Deposit. You must post a GL Debit using the TCD/TCR Cash GL with a Cash Out amount equal to {0}.", CurrencyHelper.GetFormattedValue(TcrTotalCashInAmt.Value));
				//13295
				if (_tellerVars.IsRecycler && (!IsTcrConnected() || (IsTcrConnected() && _isTcdTcrDeviceIdDifferentFromTran)))
					PMessageBox.Show(this, 13295, MessageType.Warning, MessageBoxButtons.OK, new string[] { inputMsg });
			}
			#endregion

			/* Begin 74014 */
			this.CallOtherForms("Services");
			/* End 74014 */
			HandleCtrDefferelOvrd();
			#endregion
		}


		private void pbItemCapture_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("ItemCapture");
		}

		#region #80615
		void pbPrintMailer_Click(object sender, PActionEventArgs e)
		{
			int tempRimNo = 0;
			try
			{
				tempRimNo = Convert.ToInt32(colRimNo.Text);
			}
			catch (Exception ex)
			{
				throw new PhoenixException(13432, 13432);
			}
			if (tempRimNo > 0)
				Helper.PrintMailer(tempRimNo, _tlTranSet);
		}
		#endregion


		void pbInventory_Click(object sender, PActionEventArgs e)   //#140772 - Part II
		{
			CallOtherForms("InventoryItemSearch");
		}

        void pbBondDetails_Click(object sender, PActionEventArgs e) //#140798
        {
            CallOtherForms("SavingsBond");
        }

		private void cmbTranCode_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			this.cmbTranCode.ScreenToObject();
		}

		private void cmbBatchItem_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			this.cmbBatchItem.ScreenToObject();
		}

		private void cmbBranch_Validated(object sender, System.EventArgs e)
		{
			try
			{
				PopulateComboBox("Drawer");
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void cmbDrawers_Validated(object sender, EventArgs e)
		{
		}

		private void cmbBranch_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("TranTotals");
		}

		private void cmbDrawers_PhoenixUISelectedIndexChangedEvent(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("TranTotals");
		}

		private void dfAccount_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			this.dfAccount.ScreenToObject();
		}

		private void pbReprint_Click(object sender, PActionEventArgs e)
		{
            //begin #194535
            SigBoxDetails sigPos = new SigBoxDetails();
            //Begin Task#118393
            if (colRimNo.FormattedValue == "-1")
            {
                PInt nRimNo = new PInt("nRimNo");
                nRimNo.Value = _busObjTlJournal.GetCurRimNo(colAcctType.FormattedValue, colAcctNo.FormattedValue);
                if (nRimNo.Value > 0)
                {
                    this.colRimNo.FormattedValue = nRimNo.Value.ToString();
                }
            }
            //End Task#118393
            if (_tellerVars.AdTlControl.WosaPrinting.Value != GlobalVars.Instance.ML.Y)
			{
				PMessageBox.Show(this, 360610, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				return;
			}
			//
			if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 946) // calc
				_reprintInfo = CoreService.Translation.GetUserMessageX(360611);
			else
				_reprintInfo = CoreService.Translation.GetUserMessageX(360612);
			//
			#region #76409
			#endregion
			CallOtherForms("AdHocReceipt");
			#region #80615
			if (dialogResult == DialogResult.Cancel)
				_reprintFormId.Value = 0;
			#endregion
			if (_reprintFormId.IsNull || _reprintFormId.Value == 0)
				return;
			#region get form info
			if (_tellerVars.SetContextObject("AdTlFormArray", null, _reprintFormId.Value))
			{
				_reprintTextQrp = _tellerVars.AdTlForm.TextQrp.Value;
				_partialPrintString = _tellerVars.AdTlForm.PrintString.Value;
				_wosaServiceName = _printerService.Value;
				//_wosaServiceName = _tellerVars.AdTlForm.LogicalService.Value;
				//_logicalService = _printerService.Value;
				//_logicalService = _tellerVars.AdTlForm.ServiceType.Value;
				_mediaName = _tellerVars.AdTlForm.MediaName.Value;
				_formName = _tellerVars.AdTlForm.FormName.Value;
            }
            #endregion
            //Begin Task#90213
            if (_tellerVars.AdTlForm.TextQrp.Value !=null && _tellerVars.AdTlForm.TextQrp.Value == "I")
            {   //16002 - Debit Item Forms can be Reprinted only in Item Capture Window
                PMessageBox.Show(this, 16002, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                return;
            }
            //End Task#90213
            //Begin #60494
            //  _xfsPrinter = new XfsPrinter(_wosaServiceName);     //#157637
            //Begin #55116 //commented the changes made
            if (String.IsNullOrEmpty(_wosaServiceName)) //#68874 added if condition to check whether Nexus configured or not
            {
                if (colTranCode.Text == "902" || colTranCode.Text == "910")
                {
                    if (!(_reprintTextQrp == "S" || _reprintTextQrp == "Q"))
                    {
                        // #Warning -15656 - Make sure the Printer Configuration and all the related components are setup properly before using the 'Reprint' action.
                        PMessageBox.Show(15656, MessageType.Warning, MessageBoxButtons.OK);
                        return;
                    }

                }
            }
            else
            {
                _xfsPrinter = new XfsPrinter(_wosaServiceName);
            }
            //End #55116
            //End #60494

            #region #12881 - Prompt for Print Balances
            //if (PMessageBox.Show(13438, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1, "") == DialogResult.No)
            //    _printBalances = false;
            //else
            //    _printBalances = true;
            #endregion
            #region #79502
            _promptBalances = false;

			if (!TellerVars.Instance.IsAppOnline)
				_printBalances = false;

			if (_tellerVars.AdTlControl.EnableMtPrint.Value == "Y" && _reprintTextQrp == "R")
			{
				GetPrintBalancePrompt(true);
				if (_tlTranSet.Transactions.Count == 0)
					_tlTranSet = (TlTransactionSet)thisTranSet;
			}
			else
			{
				if (_tlTranSet.Transactions != null && _tlTranSet.Transactions.Count > 0)
					_tlTranSet.Transactions.Clear();

				_formFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", colTranCode.Text);
				if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y") // #79502
					_promptBalances = true;
				thisTranSet = _tlTranSet;
				GetPrintBalancePrompt(false);

				if (_tlTranSet.Transactions.Count == 0)
				{
					acctHasPostAccessOnly = thisTran.TranAcct.bAcctHasPostAccessOnly.Value;
					_tlTranSet.Transactions.Add(thisTran);
				}
				else
					acctHasPostAccessOnly = _tlTranSet.CurTran.TranAcct.bAcctHasPostAccessOnly.Value;

			}
			if (_promptBalances && _tellerVars.AdTlControl.EnablePrtPrompt.Value == "Y" && TellerVars.Instance.IsAppOnline) // #13655
				CallOtherForms("PrintBalPrompt");
			#endregion
			try
			{

				#region #80615
				// If MT Printing configured and this is an MT Transaction,
				// call HandleMtPrinting() else process as usual....
				if (_tellerVars.AdTlControl.EnableMtPrint.Value == "Y" && _reprintTextQrp == "R")
				{
					_xfsPrinter.DoLock();	//WI#26012
					HandleMtPrinting();
				}
				else
				{
				#endregion

					//
					//			GetCheckInfo();
					//
					//			CallOtherForms("PrintForms");
					//
					// Load print info and print
                    skipped = LoadAndRePrint(""); // #80615 //#194535

				} // #80615 - ending bracket added
			}
			finally
			{
                if (!skipped && !(String.IsNullOrEmpty(_mediaName)) && !(String.IsNullOrEmpty(_formName)))   //#194535 //#60494
                {
                    _xfsPrinter.Unlock();	//WI#26012
                    _xfsPrinter.Close();	//#157637
                    skipped = false;        //#41316    Reset the skipped flag.
                }
			}

			// Begin #140895 - Teller Capture
			if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text))
			{
				if (colTranCode.Text == "902" || colTranCode.Text == "910")
				{
					TellerVars.Instance.SetContextObject("AdTlTcArray", colTranCode.Text);

					if (TellerVars.Instance.AdTlTc.RealTimeEnable.Value != GlobalVars.Instance.ML.Y)
					{
						PMessageBox.Show(this, 14580, MessageType.Warning, MessageBoxButtons.OK, null);
					}
				}
			}
			// End #140895 - Teller Capture

		}
        private void pbImage_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("TlCaptureImageViewer");
        }

        private void PbViewReceipt_Click(object sender, PActionEventArgs e) //#118298
        {
            //Begin 119792

            DocumentService doc;
            ParameterService param;

            param = new ParameterService(null, 0, Convert.ToString(colPTID.UnFormattedValue), null, 0,  false);

            doc = new DocumentService();

            string[] deliverylResults = doc.SearchAndDelivery(param); //119792-2

            if (deliverylResults == null || deliverylResults.Length < 1)
            {
                //16442 - Receipt unavailable in ECM for the selected transaction
                PMessageBox.Show(this, 16442, MessageType.Warning, MessageBoxButtons.OK, null);
            }
            else
            {
                try
                {
                    //Begin 119792-2

                    //Pass the array of image links to the viewer
                    ReceiptsViewer ecmReceiptView = new ReceiptsViewer(deliverylResults);
                    ecmReceiptView.Workspace = this.Workspace;
                    ecmReceiptView.ShowDialog();

                    //End 119792-2
                }
                catch (Exception exp)
                {
                    PMessageBox.Show(exp);
                } 
            }

            //End 119792
        }

        private void dfTlCaptureISN_KeyPress(object sender, KeyPressEventArgs e)    //#30969
        {
            if (!char.IsControl(e.KeyChar)
                   && !char.IsDigit(e.KeyChar)
                   && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
		#endregion

		#region private methods

		private void JournalSearch()
		{
			this._busObjTlJournal.PhoenixErrorId.Value = 0;
			EnableDisableVisibleLogic("CustomerNameClick");
			EnableDisableVisibleLogic("OfflineSeqSearch");
			EnableDisableVisibleLogic("CtrStatusClick");
			EnableDisableVisibleLogic("BranchAndDrawer");
			if (!ValidateSearchFilters())
			{
				if (this._busObjTlJournal.PhoenixErrorId.Value != 0)
				{
					PMessageBox.Show(this, _busObjTlJournal.PhoenixErrorId.Value, MessageType.Warning, MessageBoxButtons.OK);
					return;
				}
			}
			PopulateGrid();
			//
		}

		private bool ProcessResponses()
		{
			MessageType msgType = MessageType.Warning;
			MessageBoxButtons buttons = MessageBoxButtons.OK;
			DialogResult response;

			foreach (Phoenix.BusObj.Teller.TranResponse tranResponse in _tlTranSet.TranResponses)
			{
				if (tranResponse.RetCode < 0 && tranResponse.RetCodeDesc == null || tranResponse.RetCodeDesc == String.Empty)
					tranResponse.RetCodeDesc = _globalHelper.GetSPMessageText(tranResponse.RetCode, false);

				if (tranResponse.ErrorType == "E")
				{
					msgType = MessageType.Warning;
					buttons = MessageBoxButtons.OK;
				}
				else if (tranResponse.ErrorType == "Q")
				{
					msgType = MessageType.Question;
					buttons = MessageBoxButtons.YesNo;
				}
				else if (tranResponse.ErrorType == "I")
				{
					msgType = MessageType.Message;
					buttons = MessageBoxButtons.OK;
				}

				response = PMessageBox.Show(360301, msgType, buttons, new string[] { tranResponse.RetCode.ToString() + "-" + tranResponse.RetCodeDesc });

				if (tranResponse.ErrorType == "E" || response == DialogResult.No)
					return false;

			}
			return true;
		}

		private void PopulateComboBox(string comboName)
		{
			using (new WaitCursor())
			{
				#region combo populate
				try
				{
					if (comboName == "Drawer")
					{
						if (cmbBranch.CodeValue != null && Convert.ToInt16(cmbBranch.CodeValue) != -1)
						{
							if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
							{
								CallXMThruCDS("DrawerPopulate");
								this.cmbDrawers.Populate(this._busObjTlJournal.DrawerNo.Constraint.EnumValues);
								this.cmbDrawers.Append(-1, GlobalVars.Instance.ML.All);
							}
						}
						else
						{
							this.cmbDrawers.CodeValue = -1;
							this.cmbDrawers.Description = GlobalVars.Instance.ML.All;
						}

						this.cmbDrawers.DefaultCodeValue = this.drawerNo.Value;
						this.cmbDrawers.InitialDisplayType = UIComboInitialDisplayType.DisplayDefault;
						if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
							this.cmbDrawers.DisplayType = UIComboDisplayType.Description;
						else
							this.cmbDrawers.DisplayType = UIComboDisplayType.CombinedValue;
					}
					else if (comboName == "AcctType")
					{
						PopulateAcctTypeCombo(cmbAcctType, true, true);
					}
					#region 79510 Shared Branch
					else if (comboName == "Misc")
					{
						ArrayList miscEnums = _busObjTlJournal.MiscSearch.EnumValues;
						if (TellerVars.Instance.ShareBranchEnabled == false)  // #09143 - replaced SharedBranchCustomOption
						{

							for (int index = miscEnums.Count - 1; index >= 0; index--)
							{
								EnumValue ev = miscEnums[index] as EnumValue;
								if (ev != null && IsMLSharedBranch(ev.Description))
									miscEnums.RemoveAt(index);
							}


						}
						cmbMiscSrch.Populate(miscEnums);
					}
					//else if (comboName == "Channel")
					//{
					//    cmbChannels.PhoenixUIControl.IsNullable = NullabilityState.NotNull; // #79510
					//    // This is a Hack since the Designd didn't want the list to populate everything from PC_CHANNEL
					//    cmbChannels.Append(-1, "All"); // #79510
					//    cmbChannels.Append(3, "Acquirer");
					//    cmbChannels.Append(4, "Issuer");
					//}
					#endregion - 79510
					//
				}
				catch (PhoenixException pecbpop)
				{
					PMessageBox.Show(pecbpop);
				}
				#endregion
			}
		}

		//Begin #79510
		private bool IsMLSharedBranch(string textTocompare)
		{
			return textTocompare == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Shared Branch Transactions") ||
				textTocompare == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Endorsement");    //#79510, #09368
		}
		//End #79510

		private void UpdateDrawerBalance()
		{
			//#10702 - added fix for toolbar cash update
			//_tellerVars.DrawerCash = _tellerVars.DrawerCash + (_tlTranSet.TotalCash.IsNull? Convert.ToDecimal(0) : _tlTranSet.TotalCash.Value) +
			//    _balCoinAmt;    //#9470-Coin
			_tellerVars.DrawerCash = _tellerVars.DrawerCash + (_tlTranSet.TotalCash.IsNull ? Convert.ToDecimal(0) : _tlTranSet.TotalCash.Value) +
				((!_tlTranSet.TranSetTotalDispCoinAmt.IsNull && _tlTranSet.TranSetTotalDispCoinAmt.Value > 0) ? _tlTranSet.TranSetTotalDispCoinAmt.Value : Convert.ToDecimal(0));


			if (drawerCombo != null)
			{
				drawerCombo.Items.Clear();
				drawerCombo.Items.Add(CurrencyHelper.GetFormattedValue(_tellerVars.DrawerCash));
				drawerCombo.SelectedIndex = 0;

				// TO DO update the drawer combo
				// #8908 - Do not show cash message for non cash drawer.
				if (_tellerVars.DrawerCash < _tellerVars.AdGbRsmLimits.LowDrawerLim.Value && !_tlTranSet.TellerVars.IsNonCashDrawer)
				{
					drawerCombo.ForeColor = System.Drawing.Color.Blue;
					PMessageBox.Show(311350, MessageType.Warning, MessageBoxButtons.OK, "USD");
				}
				else if (_tellerVars.DrawerCash > _tellerVars.AdGbRsmLimits.HighDrawerLim.Value && !_tlTranSet.TellerVars.IsNonCashDrawer)
				{
					drawerCombo.ForeColor = System.Drawing.Color.Red;
					PMessageBox.Show(311349, MessageType.Warning, MessageBoxButtons.OK, "USD");
				}
				else
					drawerCombo.ForeColor = System.Drawing.Color.Black;
			}
		}

		private void EnableDisableVisibleLogic(string callerInfo)
		{
			try
			{
				if (callerInfo == "FormComplete")
				{
					if (this.isSupervisor.Value != GlobalVars.Instance.ML.Y)
					{
						this.cmbBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
						this.cmbDrawers.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
					}
					EnableDisableVisibleLogic("AccountClick");
					EnableDisableVisibleLogic("MiscClick");
					EnableDisableVisibleLogic("AmtClick");
					EnableDisableVisibleLogic("EnteredDtClick");
					EnableDisableVisibleLogic("LocalDtClick"); //#79569
					EnableDisableVisibleLogic("TranEffectiveDtClick");
					EnableDisableVisibleLogic("EffectiveDtClick");

					this.cmbTranStatus.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Default);
					if (!_isEnableTranEffectiveDt)
						this.cbTranEffectiveDt.Enabled = false;
					cbCustomerName.Enabled = (TellerVars.Instance.IsAppOnline); //#67880
					EnableDisableVisibleLogic("ComboClick");

					//Begin #79510, #09368
					pbEndorsement.Visible = TellerVars.Instance.SharedBranchCustomOption;
					pbEndorsement.Enabled = TellerVars.Instance.ShareBranchEnabled;
					//End #79510, #09368

					// Begin #140895 Teller Capture
                    //#27875 - changed the condition to look for admin teller control flag instead of connection
                    colTlCaptureISN.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y"); // TellerVars.Instance.IsTellerCaptureEnabled;
                    colTlCaptureOptionString.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y");
					// End #140895 Teller Capture
                    colTlCaptureWorkstation.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y"); //#30622
                    colEmployeeName.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y"); //#30622
                    colTlCaptureBatchId.Visible = (_tellerVars.AdTlControl.TellerCapture.Value == "Y"); //#30969
                    pbViewReceipt.Visible = TellerVars.Instance.IsECMVoucherAvailable;  //#118298
                }
				else if (callerInfo == "Alerts")        //#74772 - Added account alert fix - Selva
				{
					if (gridJournal.ContextRow >= 0)
					{
						((Workspace as PwksWindow).WksExtension as Phoenix.Shared.Windows.WkspaceExtension).RefreshAlerts();
						((Workspace as PwksWindow).WksExtension as WkspaceExtension).SetAlertAccount(
											colAcctType.Text, colAcctNo.Text, 0, true, null);

						UpdateView();
					}
				}
				else if (callerInfo == "GridClick")
				{
					//#71644
					//Begin #74772 - Added account alert fix - Selva
					((Workspace as PwksWindow).WksExtension as Phoenix.Shared.Windows.WkspaceExtension).RefreshAlerts();
					((Workspace as PwksWindow).WksExtension as WkspaceExtension).SetAlertAccount(
										string.Empty, string.Empty, 0, true, null);

					UpdateView();
					//End #74772
					if ((colTranCode.UnFormattedValue != null && _tellerHelper.IsGenericTran(colTranCode.UnFormattedValue.ToString())) ||
						(colTranCode.UnFormattedValue != null && colTranCode.UnFormattedValue.ToString() == "CLO"))
						this.pbReprint.Enabled = false;
					else
						this.pbReprint.Enabled = true;
					#region #76409
					if (colTranCode.Text == "PFR" && Convert.ToInt16(colReversal.UnFormattedValue) != 0)
						this.pbReprint.Enabled = false;
					#endregion

					#region #80615
					if (colRimNo.Text.Trim() != "" && MailerForm.Count > 0 && MailerStatus != "Closed" && MailerStatus != "Inactive" && colNonCust.Text.Trim() != "Y") // #12870
						this.pbPrintMailer.SetObjectStatus(VisibilityState.Show, EnableState.Enable);
					else
						this.pbPrintMailer.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					#endregion

					#region Enable/Disable pbSuspTranDtls
					// #80660
					if (colSuspectPtid.Text != "" && CoreService.UIAccessProvider.HasReadAcces(Phoenix.Shared.Constants.ScreenId.SuspiciousTranDetails) && TellerVars.Instance.SuspiciousTransactionScoringAlertsCustomOption)
						pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
					else
						pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
					#endregion

					//
					//#71644
					if (colTranCode.UnFormattedValue != null)
					{
						if (_tellerHelper.IsGenericTran(colTranCode.UnFormattedValue.ToString()) ||
							_tellerHelper.IsMiscTran(colTranCode.UnFormattedValue.ToString()))
						{
							this.pbAcctDisplay.Enabled = false;
						}
						else
						{
							if (colSharedBranch.Text == GlobalVars.Instance.ML.Y)  // 79510
							{
								pbAcctDisplay.Enabled = false;
							}
							else if (TellerVars.Instance.IsAppOnline)
							{
								if (colSuspectAcct.Text == GlobalVars.Instance.ML.Y)
									this.pbAcctDisplay.Enabled = false;
								//Begin #72644
								//Begin #76458
								if (_tellerHelper.IsLoanTran(realTranCode))
								{
									if (colAcctType.Text != string.Empty && colAcctType.Text != "")
									{
                                        // Begin 175838
                                        //_acctTypeDetails = _globalHelper.GetAcctTypeDetails(colAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                                        //if (_acctTypeDetails != null)
                                        //{
                                        //    if (_acctTypeDetails.DepLoan == GlobalVars.Instance.ML.CM)
                                        //        this.pbAcctDisplay.Enabled = false;
                                        //    else
                                        //        this.pbAcctDisplay.Enabled = HasScreenAccess(realTranCode);
                                        //}

                                        if (colDepType.UnFormattedValue == GlobalVars.Instance.ML.CM)
                                            this.pbAcctDisplay.Enabled = false;
                                        else
                                            this.pbAcctDisplay.Enabled = HasScreenAccess(realTranCode);
									}
								}
								else if (_tellerHelper.IsExternalTran(realTranCode))    //#76458
									this.pbAcctDisplay.Enabled = HasScreenAccess(realTranCode, colAcctType.Text.Trim());
								else
									this.pbAcctDisplay.Enabled = HasScreenAccess(realTranCode);
								//End #76458
								//this.pbAcctDisplay.Enabled = HasScreenAccess(realTranCode);
								//End #72644
								//#74772 - Added account alert fix - Selva
								if (this.pbAcctDisplay.Enabled && this.pbAcctDisplay.Visible)
									EnableDisableVisibleLogic("Alerts");
							}
							else
								this.pbAcctDisplay.Visible = false;
							//this.pbInventory.Enabled = _tellerVars.IsInventoryTrackingEnabled;  //#140772
						}
					}
					else
						this.pbAcctDisplay.Enabled = false;
					//
					if (this.colItemCount.Text == null || this.colItemCount.Text == string.Empty || this.colItemCount.Text == "0" ||
						this.colTranCode.Text == "BAT" || this.colTranCode.Text == "CLC")
						this.pbItemCapture.Enabled = false;
					else
						this.pbItemCapture.Enabled = true;

                    if (drawerStatus.Value == GlobalVars.Instance.ML.Open)//  && colSharedBranch.Text != GlobalVars.Instance.ML.Y) // 79510 , #10883 - removed
                    {
                        //WI#13877
                        PInt seqNo = new PInt("seqNo");
                        PInt Proofed = new PInt("Proofed");
                        //end WI#13877
                        //#71644
                        if (colTranCode.Text != null && colTranCode.Text != string.Empty && !_tellerHelper.IsTranReversible(colTranCode.Text, colBatchStatus.Text) ||
                            Convert.ToInt16(colReversal.UnFormattedValue) != 0)
                            this.pbReversal.Enabled = false;
                        else if ((Convert.ToInt16(colRealTranCode.UnFormattedValue) == 195 && Convert.ToDateTime(colEffectiveDt.UnFormattedValue) != postingDate.Value) ||
                            Convert.ToInt16(colRealTranCode.UnFormattedValue) == 939)   //#140772 - added #939
                            this.pbReversal.Enabled = false;
                        #region #76409
                        else if ((colTranCode.UnFormattedValue.ToString() != "PFR" && (Convert.ToInt16(colProofStatusOnus.UnFormattedValue) == 2 || Convert.ToInt16(colProofStatusTransit.UnFormattedValue) == 2)))
                            this.pbReversal.Enabled = false;
                        #endregion
                        //WI#13877
                        else if (colTranCode.UnFormattedValue.ToString() != "PFR" && (Convert.ToInt16(colSubSequence.UnFormattedValue) > 0))
                        {
                            seqNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
                            PDateTime EffDate = new PDateTime("EffDate");
                            EffDate.Value = Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate;
                            CoreService.DataService.ProcessCustomAction(_busObjTlJournal, "TransInMultiSet", seqNo, EffDate, Proofed);

                            if (Proofed.Value > 0)
                                pbReversal.Enabled = false;
                            else
                                pbReversal.Enabled = true;

                        }
                        //WI#13877
                        else if (Convert.ToDateTime(colEffectiveDt.UnFormattedValue) < postingDate.Value)
                            this.pbReversal.Enabled = false;
                        else if (Convert.ToDateTime(colEffectiveDt.UnFormattedValue) != drCurPostingDate.Value && !drCurPostingDate.IsNull)
                            this.pbReversal.Enabled = false;
                        else if ((Convert.ToDateTime(colEffectiveDt.UnFormattedValue) != postingDate.Value &&		//supervisor validation
                            (isSupervisor.Value == GlobalVars.Instance.ML.Y ||
                            Convert.ToInt16(colTranStatus.UnFormattedValue) == 9)) ||
                            Convert.ToInt16(colPODStatus.UnFormattedValue) != 9 ||
                            (isSupervisor.Value == GlobalVars.Instance.ML.Y &&
                            _adGbRsm.Supervisor.Value != GlobalVars.Instance.ML.Y))
                            this.pbReversal.Enabled = false;
                        else if (!TellerVars.Instance.IsAppOnline && (Convert.ToInt16(colTranStatus.UnFormattedValue) == (short)TlJournalTranStatus.NetworkOfflineForwarded ||
                            Convert.ToInt16(colTranStatus.UnFormattedValue) == (short)TlJournalTranStatus.NetworkOfflineForceForwarded))
                        {
                            this.pbReversal.Enabled = false;
                        }
                        // Begin WI#13545 - We Must Allow Reversals In Teller So Commenting the part of this section added with WI#4222
                        // 4222
                        //else if (colTfrAcctType.UnFormattedValue != null && _globalHelper.IsExternalAdapterAcct(colTfrAcctType.UnFormattedValue.ToString()))
                        //{
                        //    this.pbReversal.Enabled = false;
                        //}
                        // End WI#13545
                        else
                        {
                            this.pbReversal.Enabled = true;
                        }
                    }
                    else
                    {
                        this.pbReversal.Enabled = false;
                        this.pbImage.Enabled = false; //#34100
                    }

                    // Begin #76047
                    if ((colTranCode.UnFormattedValue.ToString() == "913" || colTranCode.UnFormattedValue.ToString() == "922")
                        && (colReversalDisplay.UnFormattedValue == null ||
                        (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() != "Y")) && TellerVars.Instance.IsAppOnline)  // #04747 - Added offline check
                    {
                        GbBondPurchase bond = new GbBondPurchase();
                        PString whereClause = new PString("WhereCluase");

						whereClause.Value = "Where journal_ptid = " + Convert.ToDecimal(colPTID.UnFormattedValue)
											+ " And Status = 'Closed'";

						if (bond.RowExists(whereClause))
						{
							this.pbReversal.Enabled = false;
						}
						else
						{
							this.pbReversal.Enabled = true;
						}
					}
					// End #76047

					//Begin #140772 Part II
					if (colTypeId.UnFormattedValue != null && Convert.ToDecimal(colTypeId.UnFormattedValue) > 0)   //#20928 - Changed from colRealTranCode to colTypeId
						this.pbInventory.Enabled = ((CoreService.UIAccessProvider.GetScreenAccess(Phoenix.Shared.Constants.ScreenId.InventoryItemSearchTeller) & AuthorizationType.Read) == AuthorizationType.Read &&
											_tellerVars.IsInventoryTrackingEnabled);  //#140772
					else
						this.pbInventory.Enabled = false;
					//End #140772 Part II


					// Begin #140895 - Teller Capture Integration
					/*
					 * If ISN is not null       AND
					 * Image has been commited  AND
					 *      (
					 *          (batch ptid is not null AND batch ptid is > 0)  OR
					 *          the workstation scanned is NOT this one         OR
					 *          the batch is not in progress
					 *      )
					 */
                    //#28714 - removed colTlCaptureImageCommited condition
                    //if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text) &&
                    //    colTlCaptureImageCommited.Text == GlobalVars.Instance.ML.Y &&
                    //    ((!string.IsNullOrWhiteSpace(colTlCaptureBatchPtid.Text) &&
                    //        Convert.ToInt32(colTlCaptureBatchPtid.UnFormattedValue) > 0) ||
                    //        colTlCaptureWorkstation.Text != System.Environment.MachineName ||
                    //        !Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper.IsBatchInProgress))
                    //{
                    //    pbReversal.Enabled = false;
                    //}
                    if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text) &&

                        ((!string.IsNullOrWhiteSpace(colTlCaptureBatchPtid.Text) &&
                            Convert.ToInt32(colTlCaptureBatchPtid.UnFormattedValue) > 0) ||
                            colTlCaptureWorkstation.Text != TellerVars.Instance.TlCaptureWorkstation ||
                            !Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper.IsBatchInProgress ||
                            !TellerVars.Instance.IsTellerCaptureEnabled)) // #40977 #111431
                    {
                        pbReversal.Enabled = false;
                    }

					if (colTranCode.Text == "EOB" || colTranCode.Text == "BOB")
						pbReversal.Enabled = false;

                    // Begin #95425
                    if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text) &&
                        (!string.IsNullOrWhiteSpace(colTlCaptureImageCommited.Text) && colTlCaptureImageCommited.Text != GlobalVars.Instance.ML.Y)) // #40977
                    {
                        pbReversal.Enabled = true;
                    }
                    // End #95425

					pbImage.Enabled = !Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper.IsTransactionInProgress &&
                        !string.IsNullOrWhiteSpace(colTlCaptureISN.Text) && _tellerVars.IsTellerCaptureEnabled; //#27875

					// End #140895 - Teller Capture Integration

                    // Begin #140798
                    if ((colTranCode.UnFormattedValue.ToString() == "912" || colTranCode.UnFormattedValue.ToString() == "921")
                        && (colReversalDisplay.UnFormattedValue == null ||
                        (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() != "Y")))
                    {
                        this.pbBondDetails.Enabled = true;
                    }
                    else
                        this.pbBondDetails.Enabled = false;
                    // End #140798

                    //Begin #118298
                    if (colColdStorComplete.UnFormattedValue != null && colColdStorComplete.Text.Trim() == GlobalVars.Instance.ML.Y)
                    {
                        this.pbViewReceipt.Enabled = true;
                    }
                    else
                        this.pbViewReceipt.Enabled = false;
                    //End #118298

                }
				else if (callerInfo == "AccountClick")
				{
					if (this.cbAccount.Checked)
					{
						this.cmbAcctType.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfAccount.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.cmbAcctType.Focus();
					}
					else
					{
						ClearFields(cmbAcctType);
						this.cmbAcctType.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfAccount);
						this.dfAccount.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
					}

				}
				else if (callerInfo == "CustomerNameClick")
				{
					if (this.cbCustomerName.Checked)
					{
						this.colCustomerName.Visible = true;
						this.colCustomerName.Width = 135;
					}
					else
					{
						this.colCustomerName.Visible = false;
					}

				}
				else if (callerInfo == "OfflineSeqSearch")
				{
					if (this.cbMisc.Checked && this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Offline Sequence Number"))
					{
						this.colOfflineSeqNo.Visible = true;
						this.colOfflineSeqNo.Width = this.colSequenceNo.Width;
					}
					else
					{
						this.colOfflineSeqNo.Visible = false;
						this.colOfflineSeqNo.Width = 0;
					}

				}
				else if (callerInfo == "BranchAndDrawer")
				{
					if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
					{
						if (Convert.ToInt16(cmbBranch.CodeValue) == -1)
						{
							this.colBranchNo.Visible = true;
							this.colBranchNo.Width = 55;
						}
						else
						{
							this.colBranchNo.Visible = false;
						}
						//
						if (Convert.ToInt16(cmbDrawers.CodeValue) == -1)
						{
							this.colTellerDrawerNo.Visible = true;
							this.colTellerDrawerNo.Width = 55;
						}
						else
						{
							this.colTellerDrawerNo.Visible = false;
						}
						EnableDisableVisibleLogic("TranTotals");
					}
					else
					{
						this.colBranchNo.Visible = false;
						this.colTellerDrawerNo.Visible = false;
					}
				}
				else if (callerInfo == "TranTotals")
				{
					if (Convert.ToInt16(cmbBranch.CodeValue) == -1 || Convert.ToInt16(cmbDrawers.CodeValue) == -1)
					{
						this.pbTranTotals.Enabled = false;
					}
					else
						this.pbTranTotals.Enabled = true;
				}
				else if (callerInfo == "EffectiveDtClick")
				{
					if (this.cbEffectiveDate.Checked)
					{
						this.dfEffectiveFrom.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
						this.dfEffectiveTo.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
						this.colDescription.Visible = false;
						this.colDescription1.Visible = true;
						this.colDescription1.Width = 148;
						this.colEffectiveDt.Visible = true;
						this.colEffectiveDt.Width = 100;
					}
					else
					{
						this.dfEffectiveFrom.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Disable);
						this.dfEffectiveTo.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfEffectiveFrom);
						ClearFields(dfEffectiveTo);
						this.dfEffectiveFrom.UnFormattedValue = null;
						this.dfEffectiveTo.UnFormattedValue = null;
						this.colDescription.Visible = true;
						this.colDescription.Width = 148;
						this.colDescription1.Visible = false;
						this.colEffectiveDt.Visible = false;
					}

				}
				else if (callerInfo == "TranEffectiveDtClick")
				{
					if (this.cbTranEffectiveDt.Checked)
					{
						this.dfTranEffectiveDtFrom.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
						this.dfTranEffectiveDtTo.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
						this.colDescription.Visible = false;
						this.colDescription1.Visible = true;
						this.colDescription1.Width = 148;
						this.colTranEffectiveDt.Visible = true;
						this.colTranEffectiveDt.Width = 100;
					}
					else
					{
						this.dfTranEffectiveDtFrom.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Disable);
						this.dfTranEffectiveDtTo.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfTranEffectiveDtFrom);
						ClearFields(dfTranEffectiveDtTo);
						this.colDescription.Visible = true;
						this.colDescription.Width = 148;
						this.colDescription1.Visible = false;
						this.colTranEffectiveDt.Visible = false;
					}

				}
				else if (callerInfo == "EnteredDtClick")
				{
					if (this.cbDate.Checked)
					{
						this.dfFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfFromTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfToTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.colDescription.Visible = false;
						this.colDescription1.Visible = true;
						this.colDescription1.Width = 148;
						this.colCreateDt.Visible = true;
						this.colCreateDt.Width = 100;
					}
					else
					{
						this.dfFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfFromTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfToTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfFrom);
						ClearFields(dfFromTime);
						ClearFields(dfTo);
						ClearFields(dfToTime);
						this.colDescription.Visible = true;
						this.colDescription.Width = 148;
						this.colDescription1.Visible = false;
						this.colCreateDt.Visible = false;
					}

				}
				else if (callerInfo == "LocalDtClick") // #79569
				{
					if (this.cbLocalDateTime.Checked)
					{
						this.dfFromLocalDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfFromLocalTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfToLocalDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfToLocalTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.colDescription.Visible = false;
						this.colDescription1.Visible = true;
						this.colDescription1.Width = 148;
						this.colLocalDtTime.Visible = true;
						this.colLocalTZ.Visible = true;
						this.colLocalDtTime.Width = 100;
						this.colLocalTZ.Width = 40;
					}
					else
					{
						this.dfFromLocalDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfFromLocalTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfToLocalDate.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfToLocalTime.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfFromLocalDate);
						ClearFields(dfFromLocalTime);
						ClearFields(dfToLocalDate);
						ClearFields(dfToLocalTime);
						this.colDescription.Visible = true;
						this.colDescription.Width = 148;
						this.colDescription1.Visible = false;
						this.colLocalDtTime.Visible = false;
						this.colLocalTZ.Visible = false;
					}

				}
				else if (callerInfo == "AmtClick")
				{
					if (this.cbAmt.Checked)
					{
						this.cmbAmtType.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfFromAmt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.dfToAmt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.cmbAmtType.Focus();
						if (cmbAmtType.Text != string.Empty && cmbAmtType.Text != "" && cmbAmtType.Text != null)
						{
							if (cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "CashIn/CashOut") ||
								cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "Cash In") ||
								cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "Cash Out"))
							{
								this.colCashInAmt.Visible = true;
								this.colCashInAmt.Width = 85;
								this.colCashOutAmt.Visible = true;
								this.colCashOutAmt.Width = 85;
								if (cmbAmtType.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.AmountType1, "Cash In"))
									this.colCashOutAmt.Visible = false;
								else if (cmbAmtType.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.AmountType1, "Cash Out"))
									this.colCashInAmt.Visible = false;
							}
							else
							{
								this.colCashInAmt.Visible = false;
								this.colCashOutAmt.Visible = false;
							}

							/* Begin #72916 */
							//#79574 - Modified ML list item values
							if (cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "TCD/TCR Cash In/Cash Out") ||
								cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "TCD/TCR Cash In") ||
								cmbAmtType.CodeValue.ToString().Trim() == CoreService.Translation.GetListItemX(ListId.AmountType1, "TCD/TCR Cash Out"))
							{
								this.colTCDCashIn.Visible = true;
								this.colTCDCashIn.Width = 85;
								this.colTCDCashOut.Visible = true;
								this.colTCDCashOut.Width = 85;
								if (cmbAmtType.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.AmountType1, "TCD/TCR Cash In"))
									this.colTCDCashOut.Visible = false;
								else if (cmbAmtType.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.AmountType1, "TCD/TCR Cash Out"))
									this.colTCDCashIn.Visible = false;
							}
							else
							{
								this.colTCDCashIn.Visible = false;
								this.colTCDCashOut.Visible = false;
							}
							/* End #72916 */
						}
					}
					else
					{
						ClearFields(cmbAmtType);
						this.cmbAmtType.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						ClearFields(dfFromAmt);
						ClearFields(dfToAmt);
						this.dfFromAmt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.dfToAmt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.colCashInAmt.Visible = false;
						this.colCashOutAmt.Visible = false;
						/* Begin #72916 */
						this.colTCDCashIn.Visible = false;
						this.colTCDCashOut.Visible = false;
						/* End #72916 */
					}

				}
				else if (callerInfo == "MiscClick")
				{
					if (this.cbMisc.Checked)
					{
						this.cmbMiscSrch.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
						this.cmbMiscSrch.Focus();
					}
					else
					{
						#region #79510
						//ClearFields(cmbChannels);
						//cmbChannels.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						////cbChannel.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						#endregion	 #79510
						ClearFields(cmbMiscSrch);
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);    // #76409
						ClearFields(cmbTranCode);
						this.cmbMiscSrch.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   // #76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						//Begin #79510, #09368
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						//End #79510, #09368
						this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);    //#140772
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					}

				}
				else if (callerInfo == "MiscSrchClick")
				{
					//Begin #79510, #09368
					//this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					//this.cmbMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					//ClearFields(cmbMiscBranch);
					////End #79510, #09368
					//this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);    //#140772

					//Begin #140772
					ClearFields(cmbBatchItem);
					ClearFields(cmbProofStatusMisc);    // #76409
					ClearFields(cmbTranCode);
					this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   // #76409
					this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					ClearFields(dfCheckNo);
					ClearFields(dfSeqNoFrom);
					ClearFields(dfSeqNoTo);
					this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					//Begin #79510, #09368
					this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					this.cmbMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					//End #79510, #09368
					this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);    //#140772
					this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					//End #140772
                    //#30969
                    ClearFields(cmbTlCaptureWorkstation);
                    this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                    ClearFields(dfTlCaptureISN);
                    this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Transaction"))
					{
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);  //#76409
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.cmbTranCode.Focus();
					}
					else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Batch Items"))
					{
						ClearFields(cmbTranCode);
						ClearFields(cmbProofStatusMisc);  //#76409
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default); //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.cmbBatchItem.Focus();
					}
					#region #76409
					else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Check Proof"))
					{
						ClearFields(cmbTranCode);
						ClearFields(cmbBatchItem);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.cmbProofStatusMisc.Focus();
					}
					#endregion
					else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Sequence Number") ||
						this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Offline Sequence Number"))
					{
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);    //#76409
						ClearFields(cmbTranCode);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.dfSeqNoFrom.Focus();
					}
					else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Check Number") ||
						this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Inventory Item"))  //#140772
					{
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);    //#76409
						ClearFields(cmbTranCode);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default); //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Inventory Item"))
							this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						else
							this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.dfCheckNo.Focus();
					}
					//Begin #79510, #09368
					else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Endorsement"))
					{
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);
						ClearFields(cmbTranCode);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.cmbMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        if (cmbMiscBranch.CodeValue == null)
                            cmbMiscBranch.SetValue(branchNo.Value);

					}
                    else if ((this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Device/Workstation") ||
                        this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "ISN #") ||
                        this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "AVTC Image not committed")) && TellerVars.Instance.IsAppOnline)  //#30969 // #95425
                    {
                        ClearFields(cmbBatchItem);
                        ClearFields(cmbProofStatusMisc);
                        ClearFields(cmbTranCode);
                        this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfCheckNo);
                        ClearFields(dfSeqNoFrom);
                        ClearFields(dfSeqNoTo);
                        this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        this.lblNoInvItems.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Device/Workstation"))
                        {
                            cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                            if (cmbTlCaptureWorkstation.CodeValue == null)
                            {
                                cmbTlCaptureWorkstation.DefaultCodeValue = _tlTranSet.TellerVars.TlCaptureDeviceNo;
                                cmbTlCaptureWorkstation.InitialDisplayType = UIComboInitialDisplayType.DisplayDefault;
                                cmbTlCaptureWorkstation.SetDefaultValue();
                            }
                            this.cmbTlCaptureWorkstation.Focus();
                        }
                        // Begin #95425
                        else if (this.cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "ISN #"))
                        {
                            this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                            this.dfTlCaptureISN.Focus();
                        }
                        // End #95425
                    }
					//End #79510, #09368
					else
					{
						ClearFields(cmbBatchItem);
						ClearFields(cmbProofStatusMisc);
						ClearFields(cmbTranCode);
						this.cmbBatchItem.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.cmbProofStatusMisc.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);   //#76409
						this.cmbTranCode.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						ClearFields(dfCheckNo);
						ClearFields(dfSeqNoFrom);
						ClearFields(dfSeqNoTo);
						this.dfCheckNo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.dfSeqNoTo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscBranch.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
						this.lblMiscFrom.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        //#30969
                        ClearFields(cmbTlCaptureWorkstation);
                        this.cmbTlCaptureWorkstation.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        ClearFields(dfTlCaptureISN);
                        this.dfTlCaptureISN.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
					}
				}
				else if (callerInfo == "CtrStatusClick")
				{
					if ((cmbCTRStatus.CodeValue != null && cmbCTRStatus.CodeValue.ToString().Trim() == "No CTRs") || cmbCTRStatus.CodeValue == null)
					{
						this.colCtrStatus.Visible = false;
						this.colAggregated.Visible = false;
					}
					else
					{
						this.colAggregated.Visible = true;
						this.colAggregated.Width = 68;
						this.colCtrStatus.Visible = true;
						this.colCtrStatus.Width = 68;
					}
				}
				//Begin #4450
				else if (callerInfo == "CrossRefAccounts")
				{
					if (_adGbBankControl.EnableCrossRef.Value != "Y")
					{
						this.colIncomingAcctNo.Visible = false;
						this.colIncomingTfrAcctNo.Visible = false;
					}
					else
					{
						this.colIncomingAcctNo.Visible = true;
						this.colIncomingTfrAcctNo.Visible = true;
					}
				}
				//End   #4450
				else if (callerInfo == "SupervisorViewOnlyMode")
				{
					ResetFormForSupViewOnlyMode();
				}
                //Begin WI #27776
                else if (callerInfo == "AfterGridPopulate")
                {
                    if (gridJournal.Items.Count <= 0)
                    {
                        pbAcctDisplay.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbItemCapture.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbReversal.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbReprint.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbPrintMailer.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbInventory.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbBondDetails.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        pbImage.SetObjectStatus(VisibilityState.Default, EnableState.Disable);  //#34100
                        pbViewReceipt.SetObjectStatus(VisibilityState.Default, EnableState.Disable);  //#118298
                    }
                    else
                    {
                        pbAcctDisplay.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbItemCapture.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbReversal.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbReprint.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbPrintMailer.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbSuspectDtls.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbInventory.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        pbBondDetails.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        JouranlGridClick();
                    }

                }
                //End   WI #27776
			}
			catch (PhoenixException peedvlogic)
			{
				PMessageBox.Show(peedvlogic);
			}
		}

		private void PopulateGrid()
		{
			using (new WaitCursor())
			{
				#region Initialize
				_rowCount = 0;
				//				filters.Initialize();
				filters = new Filter[100];
				int count = 0;
				this.gridJournal.Filters.Clear();
				//#71010 - let's try reset later when return from server
				//this.gridJournal.ResetTable();
				int fromTimeHour = 0;
				int fromTimeMin = 0;
				int toTimeHour = 0;
				int toTimeMin = 0;
				_busObjTlJournal.SharedBranch.Value = null; // #79510
				#endregion

				try
				{
					if (_isSupervisorViewOnlyMode && _tranInfo != null && !_tranInfo.BranchNo.IsNull && !_tranInfo.DrawerNo.IsNull)  //#79314
					{
						filters[count] = new Filter("BranchNo", _tranInfo.BranchNo.Value);
						filters[++count] = new Filter("DrawerNo", _tranInfo.DrawerNo.Value);
						filters[++count] = new Filter("FromSequenceNo", _tranInfo.SequenceNo.Value);
						filters[++count] = new Filter("ToSequenceNo", _tranInfo.SequenceNo.Value);
						filters[++count] = new Filter("EffectiveDt", _tranInfo.TranEffectiveDt.Value);
					}
					else
					{
						if (!isLoadingAfterReversal)
							dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360982)); //#71435 - replaced 360137
						//360982 - Fetching teller transaction information...
						else
							isLoadingAfterReversal = false;

						filters[count] = new Filter("BranchNo", Convert.ToInt16(cmbBranch.CodeValue));
						filters[++count] = new Filter("DrawerNo", Convert.ToInt16(cmbDrawers.CodeValue));


						if (this.cbAccount.Checked)
						{
							if (this.cmbAcctType.Text != null && this.cmbAcctType.Text != string.Empty && this.cmbAcctType.Text != "")
							{
								filters[++count] = new Filter("AcctType", cmbAcctType.CodeValue.ToString().Trim());
							}
							if (this.dfAccount.Text != null && this.dfAccount.Text != string.Empty && this.dfAccount.Text != "")
								filters[++count] = new Filter("AcctNo", dfAccount.Text);
						}

						if (this.cbAmt.Checked)
						{
							if (this.dfFromAmt.Text != null && this.dfFromAmt.Text != "" && this.dfToAmt.Text != null && this.dfToAmt.Text != "")
							{
								filters[++count] = new Filter(this.cmbAmtType.CodeValue.ToString().Trim() + "AmountFrom", Convert.ToDecimal(dfFromAmt.UnFormattedValue));
								filters[++count] = new Filter(this.cmbAmtType.CodeValue.ToString().Trim() + "AmountTo", Convert.ToDecimal(dfToAmt.UnFormattedValue));
							}
						}
						#region #76409
						if (this.cmbProofStatus.Text != null && this.cmbProofStatus.Text != "" && this.cmbAcctType.Text != "All")
						{
							filters[++count] = new Filter("ProofStatusOnUs", Convert.ToInt16(cmbProofStatus.CodeValue));
							filters[++count] = new Filter("ProofStatusTransit", Convert.ToInt16(cmbProofStatus.CodeValue));
						}
						#endregion
						if (this.cbSupervisor.Checked)
							filters[++count] = new Filter("Supervisor", 1);
						else
							filters[++count] = new Filter("Supervisor", 0);
						if (this.cbReversal.Checked)
							filters[++count] = new Filter("Reversal", 1);
						else
							filters[++count] = new Filter("Reversal", 0);
						if (this.cbCustomerName.Checked)
							filters[++count] = new Filter("CustomerName", 1);
						else
							filters[++count] = new Filter("CustomerName", 0);

						if (this.cmbTranStatus.Text != null && this.cmbTranStatus.Text != "")
							filters[++count] = new Filter("TranStatus", Convert.ToInt16(cmbTranStatus.CodeValue));

						if (this.cmbCTRStatus.Text != null && this.cmbCTRStatus.Text != "")
							filters[++count] = new Filter("CTRStatus", cmbCTRStatus.CodeValue.ToString().Trim());

						if (this.cbMisc.Checked)
						{
							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Sequence Number") &&
								Convert.ToInt16(this.dfSeqNoFrom.UnFormattedValue) >= 0 && Convert.ToInt16(this.dfSeqNoTo.UnFormattedValue) >= 0)
							{
								filters[++count] = new Filter("FromSequenceNo", Convert.ToInt16(this.dfSeqNoFrom.UnFormattedValue));
								filters[++count] = new Filter("ToSequenceNo", Convert.ToInt16(this.dfSeqNoTo.UnFormattedValue));

							}

							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Offline Sequence Number") &&
								Convert.ToInt16(this.dfSeqNoFrom.UnFormattedValue) >= 0 && Convert.ToInt16(this.dfSeqNoTo.UnFormattedValue) >= 0)
							{
								filters[++count] = new Filter("FromOfflineSequenceNo", Convert.ToInt16(this.dfSeqNoFrom.UnFormattedValue));
								filters[++count] = new Filter("ToOfflineSequenceNo", Convert.ToInt16(this.dfSeqNoTo.UnFormattedValue));

							}

                            if (this.cmbMiscSrch.Text != null && ((this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Check Number") &&
                                Convert.ToInt32(this.dfCheckNo.UnFormattedValue) >= 0) ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Inventory Item") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Single Related") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Bulk Unrelated") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Teller Capture Transactions") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Device/Workstation") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "ISN #") ||
                                this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "AVTC Image not committed")))  //#140772 #140895 #30969 // #95425
                            {
                                if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Check Number"))
                                    filters[++count] = new Filter("CheckNo", Convert.ToInt32(this.dfCheckNo.UnFormattedValue));
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Inventory Item"))
                                    filters[++count] = new Filter("InventoryItem", Convert.ToInt32(this.dfCheckNo.UnFormattedValue));
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Single Related"))
                                    filters[++count] = new Filter("TlCaptureOptionType", CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Single Related"));
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Bulk Unrelated"))
                                    filters[++count] = new Filter("TlCaptureOptionType", CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Bulk Unrelated"));
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Teller Capture Transactions"))
                                    filters[++count] = new Filter("TlCaptureOptionType", CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Teller Capture Transactions"));
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Device/Workstation"))
                                    filters[++count] = new Filter("TlCaptureWorkstation", this.cmbTlCaptureWorkstation.Description);    //#30969
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "ISN #"))  //#30969
                                {
                                    if (this.dfTlCaptureISN.UnFormattedValue == null)
                                        filters[++count] = new Filter("TlCaptureIsn", "AVTCUnScannedOnly");
                                    else
                                        filters[++count] = new Filter("TlCaptureIsn", Convert.ToString(this.dfTlCaptureISN.UnFormattedValue));
                                }
                                // Begin #95425
                                else if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "AVTC Image not committed"))
                                {
                                    filters[++count] = new Filter("TlCaptureImageCommited", "Y");
                                }
                                // End #95425

                            }

							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Transaction") &&
								this.cmbTranCode.CodeValue.ToString() != "")
							{
								filters[++count] = new Filter("Transaction", this.cmbTranCode.CodeValue.ToString().Trim()); //#71342 - this shoud not get converted to int

							}

							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Batch Items") &&
								this.cmbBatchItem.CodeValue != null)
							{
								filters[++count] = new Filter("BatchItem", Convert.ToInt16(this.cmbBatchItem.CodeValue));

							}
							#region #76409
							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Check Proof"))
							{
								filters[++count] = new Filter("ProofStatusOnUs", Convert.ToInt16(cmbProofStatusMisc.CodeValue));
								filters[++count] = new Filter("ProofStatusTransit", Convert.ToInt16(cmbProofStatusMisc.CodeValue));
							}
							#endregion

							#region #79574
							if (this.cmbMiscSrch.Text != null && this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "TCR Reverse Deposit Items"))
							{
								filters[++count] = new Filter("TCRReverseDepositItems", 1);
							}
							#endregion

							#region #79510 Shared Branch Channeld id
							if (IsMLSharedBranch(this.cmbMiscSrch.Text))
							{
								_busObjTlJournal.SharedBranch.Value = GlobalVars.Instance.ML.Y; // Acquirer
							}



							#endregion #79510 Shared Branch Channeld id
						}
						//#region #79510 Shared Branch Channeld id
						//if (cbChannel.CheckState == CheckState.Checked && cmbChannels.SelectedIndex >= 0)
						//{
						//    _busObjTlJournal.ChannelId.Value = Convert.ToInt16(this.cmbChannels.CodeValue);
						//}
						//else
						//    _busObjTlJournal.ChannelId.Value = DbSmallInt.Null;
						//#endregion
						if (this.cbEffectiveDate.Checked)
						{
							if ((DateTime)this.dfEffectiveFrom.UnFormattedValue != DateTime.MinValue &&
								(DateTime)this.dfEffectiveTo.UnFormattedValue != DateTime.MinValue)
							{
								filters[++count] = new Filter("PostingDtFrom", Convert.ToDateTime(this.dfEffectiveFrom.UnFormattedValue));
								filters[++count] = new Filter("PostingDtTo", Convert.ToDateTime(this.dfEffectiveTo.UnFormattedValue));
							}
						}
						else
						{
							if (!this.cbTranEffectiveDt.Checked && !this.cbDate.Checked)
							{
								filters[++count] = new Filter("EffectiveDt", Convert.ToDateTime(postingDate.Value));
							}
						}

						if (this.cbTranEffectiveDt.Checked)
						{
							if ((DateTime)this.dfTranEffectiveDtFrom.UnFormattedValue != DateTime.MinValue &&
								(DateTime)this.dfTranEffectiveDtTo.UnFormattedValue != DateTime.MinValue)
							{
								filters[++count] = new Filter("TranEffectiveDtFrom", Convert.ToDateTime(this.dfTranEffectiveDtFrom.UnFormattedValue));
								filters[++count] = new Filter("TranEffectiveDtTo", Convert.ToDateTime(this.dfTranEffectiveDtTo.UnFormattedValue));
							}
						}

						#region #79569
						if (this.cbLocalDateTime.Checked)
						{
							{
								if (this.dfFromLocalDate.Text != string.Empty && this.dfFromLocalDate.Text != "" && this.dfFromLocalDate.Text != null &&
									this.dfToLocalDate.Text != string.Empty && this.dfToLocalDate.Text != "" && this.dfToLocalDate.Text != null)
								{
									if ((this.dfFromLocalTime.Text != string.Empty && this.dfFromLocalTime.Text != "" && this.dfFromLocalTime.Text != null) ||
										(this.dfToLocalTime.Text != string.Empty && this.dfToLocalTime.Text != "" && this.dfToLocalTime.Text != null))
									{
										if (this.dfFromLocalTime.Text != string.Empty && this.dfFromLocalTime.Text != "" && this.dfFromLocalTime.Text != null)
										{
											fromTimeHour = Convert.ToDateTime(dfFromLocalTime.UnFormattedValue).Hour;
											fromTimeMin = Convert.ToDateTime(dfFromLocalTime.UnFormattedValue).Minute;
										}
										if (this.dfToLocalTime.Text != string.Empty && this.dfToLocalTime.Text != "" && this.dfToLocalTime.Text != null)
										{
											toTimeHour = Convert.ToDateTime(dfToLocalTime.UnFormattedValue).Hour;
											toTimeMin = Convert.ToDateTime(dfToLocalTime.UnFormattedValue).Minute;
										}


										filters[++count] = new Filter("LocalDtFrom", new DateTime(Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Year,
											Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Month,
											Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Day,
											fromTimeHour,
											fromTimeMin,
											0,
											0));

										filters[++count] = new Filter("LocalDtTo", new DateTime(Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Year,
											Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Month,
											Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Day,
											toTimeHour,
											toTimeMin,
											0,
											0));
									}
									else
									{
										filters[++count] = new Filter("LocalDtFrom", new DateTime(Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Year,
											Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Month,
											Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Day,
											0,
											0,
											0,
											0));

										filters[++count] = new Filter("LocalDtTo", new DateTime(Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Year,
											Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Month,
											Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Day,
											23,
											59,
											0,
											0));
									}
								}
							}
						}
						#endregion

						if (this.cbDate.Checked)
						{
							if (this.dfFrom.Text != string.Empty && this.dfFrom.Text != "" && this.dfFrom.Text != null &&
								this.dfTo.Text != string.Empty && this.dfTo.Text != "" && this.dfTo.Text != null)
							{
								//#71362
								if ((this.dfFromTime.Text != string.Empty && this.dfFromTime.Text != "" && this.dfFromTime.Text != null) ||
									(this.dfToTime.Text != string.Empty && this.dfToTime.Text != "" && this.dfToTime.Text != null))
								{
									//#71436 - fixed from and to time search
									if (this.dfFromTime.Text != string.Empty && this.dfFromTime.Text != "" && this.dfFromTime.Text != null)
									{
										fromTimeHour = Convert.ToDateTime(dfFromTime.UnFormattedValue).Hour;
										fromTimeMin = Convert.ToDateTime(dfFromTime.UnFormattedValue).Minute;
									}
									if (this.dfToTime.Text != string.Empty && this.dfToTime.Text != "" && this.dfToTime.Text != null)
									{
										toTimeHour = Convert.ToDateTime(dfToTime.UnFormattedValue).Hour;
										toTimeMin = Convert.ToDateTime(dfToTime.UnFormattedValue).Minute;
									}


									filters[++count] = new Filter("CreateDateFrom", new DateTime(Convert.ToDateTime(dfFrom.UnFormattedValue).Year,
										Convert.ToDateTime(dfFrom.UnFormattedValue).Month,
										Convert.ToDateTime(dfFrom.UnFormattedValue).Day,
										fromTimeHour,
										fromTimeMin,
										0,
										0));

									filters[++count] = new Filter("CreateDateTo", new DateTime(Convert.ToDateTime(dfTo.UnFormattedValue).Year,
										Convert.ToDateTime(dfTo.UnFormattedValue).Month,
										Convert.ToDateTime(dfTo.UnFormattedValue).Day,
										toTimeHour,
										toTimeMin,
										0,
										0));
								}
								else
								{
									//#71362 - fake the time stamp in case of null
									filters[++count] = new Filter("CreateDateFrom", new DateTime(Convert.ToDateTime(dfFrom.UnFormattedValue).Year,
										Convert.ToDateTime(dfFrom.UnFormattedValue).Month,
										Convert.ToDateTime(dfFrom.UnFormattedValue).Day,
										0,
										0,
										0,
										0));

									filters[++count] = new Filter("CreateDateTo", new DateTime(Convert.ToDateTime(dfTo.UnFormattedValue).Year,
										Convert.ToDateTime(dfTo.UnFormattedValue).Month,
										Convert.ToDateTime(dfTo.UnFormattedValue).Day,
										23,
										59,
										0,
										0));
								}
							}
						}
					}

					foreach (Filter filter in filters)
					{
						if (filter != null)
							this.gridJournal.Filters.Add(filter);
					}
					CallXMThruCDS("GridPopulate");
					//#71010
					//this.gridJournal.ResetTable();
					this.gridJournal.PopulateTable(gridNode, true);
					AfterGridPopulate();

					dlgInformation.Instance.HideInfo();
				}
				catch (PhoenixException pepop)
				{
					PMessageBox.Show(pepop);
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
				}
			}
		}

		private void FixAmountFields()
		{
			try
			{
				cmbAcctType.CodeValue = null;
				dfFromAmt.PhoenixUIControl.ObjectToScreen();
				if (dfFromAmt.UnFormattedValue != null && (dfFromAmt.UnFormattedValue.ToString() == String.Empty || (decimal)dfFromAmt.UnFormattedValue == (decimal)0))
					dfFromAmt.UnFormattedValue = null;

				dfToAmt.PhoenixUIControl.ObjectToScreen();
				if (dfToAmt.UnFormattedValue != null && (dfToAmt.UnFormattedValue.ToString() == String.Empty || (decimal)dfToAmt.UnFormattedValue == (decimal)0))
					dfToAmt.UnFormattedValue = null;
			}
			catch
			{

			}
		}

		private void SetFormDefaults()
		{
			this.cbReversal.Checked = true;
			this.cbSupervisor.Checked = true;
			_isEnableTranEffectiveDt = (_tellerVars.AdTlControl.EnableBackdate.Value == GlobalVars.Instance.ML.Y);
			this.DefaultAction = this.pbSearch;
		}

		private void PopUpDisplay(short tranCode)
		{

			try
			{
				if (tranCode == 946)
				{
					frmTlJournalTapeDisplay JournalTapeDisp = new frmTlJournalTapeDisplay();
					JournalTapeDisp.InitParameters(Convert.ToInt16(colBranchNo.UnFormattedValue),
						Convert.ToInt16(colTellerDrawerNo.UnFormattedValue),
						Convert.ToDateTime(colEffectiveDt.UnFormattedValue),
						Convert.ToDecimal(colPTID.UnFormattedValue));
					JournalTapeDisp.Workspace = this.Workspace;
					JournalTapeDisp.ParentGrid = this.gridJournal;
					JournalTapeDisp.Show();
				}
				else
				{
					frmTlJournalDisplay JournalDisp = new frmTlJournalDisplay();
					JournalDisp.InitParameters(Convert.ToInt16(colBranchNo.UnFormattedValue),
						Convert.ToInt16(colTellerDrawerNo.UnFormattedValue),
						Convert.ToDateTime(colEffectiveDt.UnFormattedValue),
						Convert.ToDecimal(colPTID.UnFormattedValue));
					JournalDisp.Workspace = this.Workspace;
					JournalDisp.ParentGrid = this.gridJournal;
					JournalDisp.Show();
				}
			}
			catch (PhoenixException pepop)
			{
				PMessageBox.Show(pepop);
			}
		}

		private void ClearFields(PdfStandard clearField)
		{
			clearField.Clear();
			clearField.UnFormattedValue = null;
			clearField.SetValue(clearField.UnFormattedValue);		// #6615 - clearField.SetValue(clearField.UnFormattedValue, true);
		}

		private void ClearFields(PComboBoxStandard clearField)
		{
			clearField.CodeValue = null;
			clearField.Description = null;
			clearField.SetValue(null);								// #6615 - clearField.SetValue(null, true);
			clearField.ScreenToObject();
		}

		private void ValidateDateFields(PdfStandard fromDateField, PdfStandard toDateField, ref DateTime searchFromDate, ref DateTime searchToDate)
		{
			searchFromDate = DateTime.MinValue;
			searchToDate = DateTime.MinValue;
			if (toDateField.Text != "" && toDateField.Text != string.Empty && toDateField.Text != null)
				searchToDate = Convert.ToDateTime(toDateField.UnFormattedValue);
			if (fromDateField.Text != "" && fromDateField.Text != string.Empty && fromDateField.Text != null)
				searchFromDate = Convert.ToDateTime(fromDateField.UnFormattedValue);
		}

		private void ValidateDateFields(PdfStandard fromDateField, PdfStandard toDateField, PdfStandard fromTimeField, PdfStandard toTimeField,
			ref DateTime searchFromDate, ref DateTime searchToDate, ref DateTime searchFromTime, ref DateTime searchToTime)
		{
			searchFromDate = DateTime.MinValue;
			searchToDate = DateTime.MinValue;
			searchFromTime = DateTime.MinValue;
			searchToTime = DateTime.MinValue;
			if (toDateField.Text != "" && toDateField.Text != string.Empty && toDateField.Text != null)
				searchToDate = Convert.ToDateTime(toDateField.UnFormattedValue);
			if (fromDateField.Text != "" && fromDateField.Text != string.Empty && fromDateField.Text != null)
				searchFromDate = Convert.ToDateTime(fromDateField.UnFormattedValue);
			if (toTimeField.Text != "" && toTimeField.Text != string.Empty && toTimeField.Text != null)
				searchToTime = Convert.ToDateTime(toTimeField.UnFormattedValue);
			if (fromTimeField.Text != "" && fromTimeField.Text != string.Empty && fromTimeField.Text != null)
				searchFromTime = Convert.ToDateTime(fromTimeField.UnFormattedValue);
		}

		private bool ValidateSearchFilters()
		{
			try
			{
				#region Validate Account
				if (this.cbAccount.Checked)
				{
					if (!_busObjTlJournal.ValidateSrchByAccount())
						return false;
				}
				#endregion

				#region Validate Posting Date
				if (this.cbEffectiveDate.Checked)
				{
					ValidateDateFields(dfEffectiveFrom, dfEffectiveTo, ref searchFromDate, ref searchToDate);
					//
					if (!_busObjTlJournal.ValidateSrchByPostingDate(searchFromDate, searchToDate))
						return false;
				}
				#endregion

				#region Validate Tran Effective Date
				if (this.cbTranEffectiveDt.Checked)
				{
					ValidateDateFields(dfTranEffectiveDtFrom, dfTranEffectiveDtTo, ref searchFromDate, ref searchToDate);
					//
					if (!_busObjTlJournal.ValidateSrchByTranEffectiveDate(searchFromDate, searchToDate))
						return false;
				}
				#endregion

				#region Validate Entered Date
				if (this.cbDate.Checked)
				{
					ValidateDateFields(dfFrom, dfTo, dfFromTime, dfToTime,
						ref searchFromDate, ref searchToDate, ref searchFromTime, ref searchToTime);
					//
					if (!_busObjTlJournal.ValidateSrchByDate(searchFromDate, searchToDate, searchFromTime, searchToTime))
						return false;
					//					if (!_busObjTlJournal.ValidateSrchByDate( Convert.ToDateTime(dfFrom.UnFormattedValue),
					//						Convert.ToDateTime(dfTo.UnFormattedValue),
					//						Convert.ToDateTime(dfFromTime.UnFormattedValue),
					//						Convert.ToDateTime(dfToTime.UnFormattedValue)))
					//						return false;
				}
				#endregion

				#region Validate Local Date - #79569
				if (this.cbLocalDateTime.Checked)
				{
					ValidateDateFields(dfFromLocalDate, dfToLocalDate, dfFromLocalTime, dfToLocalTime,
						ref searchFromDate, ref searchToDate, ref searchFromTime, ref searchToTime);
					if (!_busObjTlJournal.ValidateSrchByDate(searchFromDate, searchToDate, searchFromTime, searchToTime))
						return false;
				}
				#endregion

				#region Validate Amount Type
				if (this.cbAmt.Checked)
				{
					if (!_busObjTlJournal.ValidateSrchByAmount())
						return false;
				}
				#endregion

				#region Validate Misc Search
				if (this.cbMisc.Checked)
				{
					if (!_busObjTlJournal.ValidateSrchByMisc())
						return false;
				}
				#endregion
			}
			catch (PhoenixException peval)
			{
				PMessageBox.Show(peval);
			}
			return true;
		}

		private void AfterGridPopulate()
		{
			if (gridJournal.Items.Count == 0)
			{
				this.pbDisplay.Enabled = false;
			}
			else
			{
				this.pbDisplay.Enabled = true;
			}
			this.dfTranCount.Text = Convert.ToString(_rowCount);
            EnableDisableVisibleLogic("AfterGridPopulate");        //WI #27776
		}

		private void PopUpAcctDisplay()
		{
			try
			{
				//Begin #9311, CR#8075
				if (tempNonCust != "Y")
					CallOtherForms("AcctDisplayClick");
				else
					CallOtherForms("NonCustDisplayClick");
				//End #9311, CR#8075
			}
			catch (PhoenixException pepop)
			{
				PMessageBox.Show(pepop);
			}
		}

		private bool HasScreenAccess(short realTranCode, string acctType) //#76458
		{
			int nextScreenId = 0;

			if (!TellerVars.Instance.IsAppOnline)
				return false;

			if (_tellerHelper.IsDepositTran(realTranCode))
			{
                //if (_adGbAcctType.DepType.Value == GlobalVars.Instance.ML.Tran) //175838
                if (Convert.ToString(colDepType.UnFormattedValue).Trim() == GlobalVars.Instance.ML.Tran)
                {
                    nextScreenId = Phoenix.Shared.Constants.ScreenId.DpDisplayTran;
                }
                else
                {
                    nextScreenId = Phoenix.Shared.Constants.ScreenId.DpDisplayTime;
                }
			}
			else if (_tellerHelper.IsLoanTran(realTranCode))
				nextScreenId = Phoenix.Shared.Constants.ScreenId.LnDisplay;
			else if (_tellerHelper.IsRIMTran(realTranCode))
				nextScreenId = Phoenix.Shared.Constants.ScreenId.RmDisplay;
			else if (_tellerHelper.IsSafeDepositTran(realTranCode))
				nextScreenId = Phoenix.Shared.Constants.ScreenId.SdDepositDisplay;
			else if (_tellerHelper.IsExternalTran(realTranCode)) //#76458
				nextScreenId = _tlTranSet.GetExternalDisplayScreenId(acctType);
			else
				nextScreenId = 0;

			if (nextScreenId > 0)
			{
				//#72358 - control screen access through message instead of enable/disable
				//				if (_tellerVars.DebugSecurity || _adRmRestrict.RestrictLevel.Value < _tellerVars.EmplRestrictLevel)
				//				{
				//					if (SetContext())
				//					{
				//						if (AdGbRsmRim.Edit.Value != GlobalVars.Instance.ML.Y)
				//							return false;
				//						else
				//						{
				//							this.pbAcctDisplay.NextScreenId = nextScreenId;
				//							return true;
				//						}
				//					}
				//				}
				//#76458 - add code change to allow xmal external adapter window access through window security once they are available
				//this.pbAcctDisplay.NextScreenId = nextScreenId;
				//if (realTranCode == 520 || realTranCode == 570) //#76458 - block adapter external acct display
				//{
				//    if (colAcctType.UnFormattedValue != null && colAcctType.Text != "" && colAcctType.Text != null && colAcctType.Text != string.Empty &&
				//        _tlTranSet.IsExternalAdapterAcct(colAcctType.Text.Trim()))
				//        this.pbAcctDisplay.NextScreenId = _tlTranSet.GetExternalDisplayScreenId(colAcctType.Text.Trim());
				//    return false;
				//}
				return true;
			}
			else
				return false;
		}

		private bool HasScreenAccess(short realTranCode)
		{
			return HasScreenAccess(realTranCode, string.Empty);
		}


		public AdGbRsmRim AdGbRsmRim
		{
			get
			{
				return _adGbRsmRim;
			}
		}

		//public AdExAcctType AdExAcctType    //#76458
		//{
		//    get { return _adExAcctType; }
		//}

		private bool SetContext()
		{
			if (!_tellerVars.SetContextObject("AdGbRsmRimArray", tempRimNo))
			{
				// raise exception
				return false;
			}
			_adGbRsmRim = _tellerVars.AdGbRsmRim;
			return true;
		}

		//Begin #1871
		private bool IsAllowReprintOfChecks()
		{
			bool allowReprint = true;

			#region reason for why not
			if (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() == "Y") //if reversal
				allowReprint = false;
			else if (Convert.ToDateTime(colEffectiveDt.UnFormattedValue) < postingDate.Value)
				allowReprint = false;
			else if (Convert.ToDateTime(colEffectiveDt.UnFormattedValue) != drCurPostingDate.Value && !drCurPostingDate.IsNull)
				allowReprint = false;
			else if ((Convert.ToDateTime(colEffectiveDt.UnFormattedValue) != postingDate.Value &&		//supervisor validation
				(isSupervisor.Value == GlobalVars.Instance.ML.Y ||
				Convert.ToInt16(colTranStatus.UnFormattedValue) == 9)))
				allowReprint = false;
			#endregion
			//
			return allowReprint;
		}
		//End #1871

		private void CallOtherForms(string origin)
		{
			PfwStandard tempWin = null;
			PfwStandard tempDlg = null;

			try
			{
				if (origin == "AcctDisplayClick")
				{

					if (_tellerHelper.IsDepositTran(realTranCode))
					{
                        //if (_adGbAcctType.DepType.Value == GlobalVars.Instance.ML.Tran) // Begin 175838
                        if (Convert.ToString(colDepType.UnFormattedValue).Trim() == GlobalVars.Instance.ML.Tran)
						{
							tempWin = Helper.CreateWindow("phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", "frmDpDisplayTran");
							tempWin.InitParameters(tempAcctType, tempAcctNo);
						}
						else
						{
							tempWin = Helper.CreateWindow("phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", "frmDpDisplayTime");
							tempWin.InitParameters(tempAcctType, tempAcctNo);
						}
					}
					else if (_tellerHelper.IsLoanTran(realTranCode))
					{
                        //#69369
                        string depLoan = string.Empty;
                        if (_tellerHelper.IsCommitmentTran(realTranCode) && !string.IsNullOrEmpty(colAcctType.Text))
                        {
                            _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                            //
                            if (_acctTypeDetails != null)		// #71853 - Added the condition
                            {
                                depLoan = _acctTypeDetails.DepLoan;
                            }
                        }

                        if (!string.IsNullOrEmpty(depLoan) && depLoan == "CM")
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.cmtdisplay", "Phoenix.Client.Commitment", "frmIraLnCommitmentDisplay");
                            tempWin.InitParameters(tempAcctType, tempAcctNo, tempRimNo);
                        }
                        else
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.lndisplay", "Phoenix.Client.Loan", "frmLnDisplay");
                            tempWin.InitParameters(tempAcctType, tempAcctNo);
                        }
					}
					else if (_tellerHelper.IsSafeDepositTran(realTranCode))
					{
						tempWin = Helper.CreateWindow("phoenix.client.sddisplay", "Phoenix.Client.Sdb", "frmSdDisplay");
						tempWin.InitParameters(tempAcctType, tempAcctNo, -1);
					}
					else if (_tellerHelper.IsRIMTran(realTranCode))
					{
						tempWin = Helper.CreateWindow("phoenix.client.RmDisplay", "Phoenix.Client", "frmRmDisplay");
						tempWin.InitParameters(tempRimNo);
					}
					else if (_tellerHelper.IsExternalTran(realTranCode))
					{
						//Launch XAML Window
						ShowFormHelper.XAMLWindow(this, "ExAcctDisplay", tempAcctType, tempAcctNo, tempRimNo);
					}
				}
				else if (origin == "ItemCapture")
				{
					tempWin = Helper.CreateWindow("phoenix.client.tlcaptureditems", "Phoenix.Client.TlCapturedItems", "frmTlCapturedItems");
					tempWin.InitParameters(true, Convert.ToInt16(colBranchNo.UnFormattedValue),
						Convert.ToInt16(colTellerDrawerNo.UnFormattedValue),
						Convert.ToInt16(colSequenceNo.UnFormattedValue),
						Convert.ToDateTime(colEffectiveDt.UnFormattedValue),
						Convert.ToInt16(colSubSequence.UnFormattedValue),
						null,
						Convert.ToDecimal(colPTID.UnFormattedValue)); //#71472 - uncommented ptid param
				}
				else if (origin == "Override")
				{
					tempWin = Helper.CreateWindow("phoenix.client.tloverride", "Phoenix.Windows.TlOverride", "dlgTlSupervisorOverride");
					tempWin.InitParameters("JournalRev", _tlJournalOvrd, Convert.ToInt16(colBranchNo.UnFormattedValue),
						Convert.ToInt16(colTellerDrawerNo.UnFormattedValue),
						Convert.ToInt16(colSequenceNo.UnFormattedValue),
						Convert.ToDateTime(colEffectiveDt.UnFormattedValue));   //#79314 - added branch, drawer, seq and effectivedt params
				}
				else if (origin == "AdHocReceipt")
				{
					_reprintFormId = new PSmallInt("FormId");
					_printerService = new PString("PrinterService");
					// #12215 Commented -  _reprintFormId.Value = Convert.ToInt16(this.ScreenId); // #80615

					#region #76409
					if (this.colTranCode.Text == "PFR")
						_reprintFormId.Value = Convert.ToInt16(Phoenix.Shared.Constants.ScreenId.TlProofTotals);
					#endregion
					tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt");
					//#76057 - SelvaFix
					//Begin #1871
					//if (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() == "Y") //if reversal
					//    _reprintFormId.Value = -1;
					if (!IsAllowReprintOfChecks() && this.colTranCode.Text != "PFR")    //#76409 - Added check for PFR
						_reprintFormId.Value = -1;
					//End #1871
					// #76409 - Added noCopies param
					//tempDlg.InitParameters( _reprintInfo, _reprintFormId, _reprintFormId.Value, null, null, _noCopies );
					tempDlg.InitParameters(_reprintInfo, _reprintFormId, this.ScreenId, null, null, _noCopies, _printerService); // #12215 - pass screen id
				}
				else if (origin == "CheckInfo")
				{
					_checkInfoRimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
					tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgCheckInfo");
					tempDlg.InitParameters(_checkInfoRimNo, _wosaPrintInfo, _tlTranSet);   // #79621 - Added _tlTranSet to the params ... // tempDlg.InitParameters( _checkInfoRimNo, _wosaPrintInfo, _tlTranSet.CurTran.TranCode.Value );	// #8560 - Added TranCode.Value to the InitParameters call ...

					_tlTranSet.IsSqrFormExist();    //#12215

					if (!_reprintFormId.IsNull && _tlTranSet.IsCheckPrintAvailable(true, _reprintFormId.Value, false))   //#1871
					{

						//9591 DONT NEED ANY OF THIS. ALREADY done in Window

						//PInt branchNo = new PInt("brNo");
						//PInt tranCode = new PInt("tranCode");
						//branchNo.Value = Convert.ToInt16(_tlTranSet.TellerVars.BranchNo);
						//tranCode.Value = _tlTranSet.CurTran.TranCode.Value;
						//string printerInfo = _tlTranSet.GetPrinterInfo(branchNo, tranCode);
						//string[] printerInfoArray = new string[2];
						//printerInfoArray = printerInfo.Split('^');
						//string[] descArray = new string[20];
						//string convStr = printerInfoArray[2].Replace(@"\", "/");
						//descArray = convStr.Split('/');
						//string ptrName = descArray[descArray.Length - 1];

						//_wosaPrintInfo.CheckPrinterID = printerInfoArray[2];
						//_wosaPrintInfo.CheckPrinterName = ptrName;
						_wosaPrintInfo.ShowPrintInfo = true;

						//Begin #3548
						_wosaPrintInfo.IsCheckReprint = true;
						_wosaPrintInfo.JournalPtid = Convert.ToDecimal(colPTID.UnFormattedValue);
                        _wosaPrintInfo.SbSeqNo = Convert.ToString(colSubSequence.UnFormattedValue); //#120397
                        //End #3548


                    }
				}
				// Begin #74014
				else if (origin == "Services")
				{
					if (colTranCode.UnFormattedValue.ToString() == "176" || colTranCode.UnFormattedValue.ToString() == "177"
						|| colTranCode.UnFormattedValue.ToString() == "178" || colTranCode.UnFormattedValue.ToString() == "131"
						|| colTranCode.UnFormattedValue.ToString() == "193" || colTranCode.UnFormattedValue.ToString() == "333"
						|| colTranCode.UnFormattedValue.ToString() == "334" || colTranCode.UnFormattedValue.ToString() == "345")
					{
						tempWin = Helper.CreateWindow("Phoenix.Client.Svcs.RemoveSvcs", "Phoenix.Client.Svcs.RemoveSvcs", "frmRemoveAcctSvcs");
						tempWin.InitParameters(colAcctType.UnFormattedValue,
												colAcctNo.UnFormattedValue,
												"1");
					}
				}
				// End #74014
				// Begin #74269
				else if (origin == "PrintForm")
				{
					#region Print Form
					tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgPrintForms");
					tempDlg.InitParameters(Convert.ToInt16(_reprintFormId.Value), 0, (short)-1, _wosaPrintInfo, null, 0);
					#endregion
				}
				// End #74269
                //begin #194535
                else if (origin == "eReceipt")
                {
                    // Call dlgReceiptInfo
                    string sConfig = "";    //#42567, #43143
                    string sXmlTlTransaction = "";  //#40787
                    string sXmlTlAcctDetail = "";   //#40787
                    string sUpdateData = "";    //#40276
                    int nTransaction = 0;       //#40787

                    phoenix.client.tlprinting.dlgRecieptInfo dlgReceipt = new phoenix.client.tlprinting.dlgRecieptInfo();
                    TlTransaction myTlTransaction = new TlTransaction();    //#40787
                    myTlTransaction = _tlTranSet.CurTran.Clone();   //#40787
                    myTlTransaction.TranAcct.FromJournal.Value = "Y";     //#40787
                    myTlTransaction.TranCodeDesc.Value = colDescription1.Text;  //#40787
                    sXmlTlTransaction = BusObjSerializer.SerializeToXml(myTlTransaction, false, false);
                    sXmlTlAcctDetail = BusObjSerializer.SerializeToXml(myTlTransaction.TranAcct, false, false);
                    //begin #43143
                    isPrintRequired = false;
                    if (myTlTransaction.TranAcct.FromMTForms.Value == "Y")
                    {
                        if (!_tlTranSet.TellerVars.AdTlForm.PrintString.IsNull && _tlTranSet.TellerVars.AdTlForm.PrintString.Value.IndexOf("Float") >= 0)   //#54638 - added null validation
                        {
                            if (_wosaPrintInfo.RegCCHolds != null)   //#43143
                            {
                                isPrintRequired = true;
                            }
                        }
                    }
                    //end  #43143
                    //begin #40276
                    //begin #40787
                    if (_tlTranSet.Transactions.Count > 0)
                    {
                        //Multiple transactions
                        for (nTransaction = 0; nTransaction < _tlTranSet.Transactions.Count; nTransaction++)
                        {
                            if ((_tlTranSet.Transactions[nTransaction] as TlTransaction).TranAcct.UpdateEmailList != null)
                            {
                                //Get the answers from previous calls to dlgReceiptInfo and put them in a string.
                                sUpdateData = sUpdateData + (_tlTranSet.Transactions[nTransaction] as TlTransaction).TranAcct.UpdateEmailList.GetXMLFromObject((_tlTranSet.Transactions[0] as TlTransaction).TranAcct.UpdateEmailList) + "~";
                            }
                        }
                    }
                    else
                    {
                        //One transaction
                        if ((_tlTranSet.CurTran as TlTransaction).TranAcct.UpdateEmailList != null)
                        {
                            //Get the answer from a previous call to dlgReceiptInfo and put it in a string.
                            sUpdateData = sUpdateData + (_tlTranSet.CurTran as TlTransaction).TranAcct.UpdateEmailList.GetXMLFromObject((_tlTranSet.CurTran as TlTransaction).TranAcct.UpdateEmailList) + "~";
                        }
                    }

                    if (isPrintRequired)  //#43143
                    {
                        sConfig += "PrintRequired" + "^";
                    }
                    if (isReadOnly)   //#42567
                    {
                        sConfig += "ReadOnly" + "^";
                    }

                    dlgReceipt.InitParameters(sXmlTlTransaction, sXmlTlAcctDetail, sUpdateData, sConfig,
                        Convert.ToInt16(_reprintFormId.Value), 0, (short)-1,
                         _wosaPrintInfo, null); //#42567 #43143
                    //end   #40276
                    dlgReceipt.Workspace = this.Workspace;
                    dialogResult = dlgReceipt.ShowDialog(this);
                    string myXml = dlgReceipt.DialogOptions;
                    Object myObject = new Object();
                    if (myXml != "" && dialogResult != DialogResult.Cancel && dialogResult != DialogResult.Ignore)
                    {
                        //Get the answers from the dlgReceiptInfo and put them in myInstrunction.
                        if (myInstruction == null)
                        {
                            myInstruction = new ReceiptDeliveryInstruction();
                        }
                        myObject = myInstruction.ObjectToXML(myXml);
                        myInstruction = (ReceiptDeliveryInstruction)myObject;

                        if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Retry || dialogResult == DialogResult.No)
                        {
                            //Email the form.
                            emailAddress = myInstruction.EmailAddress;
                        }
                        else
                        {
                            emailAddress = "";  //Don't email the form
                        }
                        //begin #40276
                        _tlTranSet.CurTran.TranAcct.UpdateEmailList = myInstruction;    //#40787
                        //end   #40276
                    }
                    else
                    {
                        myInstruction = null;
                        emailAddress = "";  //Don't email the form
                    }
                    AfterDialogClosed(origin);
                }
                //end   #194535
				// Begin #9311, CR#8075
				else if (origin == "NonCustDisplayClick")
				{
					PInt tmpRimNo = new PInt();
					tempWin = Helper.CreateWindow("phoenix.client.tlnonrmacct", "Phoenix.Client.TlNonRmAcct", "frmTlNonRmAcctEdit");
					tmpRimNo.Value = Convert.ToInt32(colRimNo.UnFormattedValue);
					tempWin.InitParameters(tmpRimNo);
				}
				// End #9311, CR#8075

				//Begin #79510, #09368
				else if (origin == "Endorsement")
				{
					short paraBranchNo = -1;
					short paraDrawerNo = -1;
					DateTime paraFromCreateDt = DateTime.MinValue;
					DateTime paraToCreateDt = DateTime.MinValue;

					if (cbMisc.Checked && cmbMiscSrch.Description == CoreService.Translation.GetListItemX(ListId.TlMiscSrch, "Endorsement"))
					{
						if (Convert.ToInt16(cmbMiscBranch.CodeValue) > 0)
						{
							paraBranchNo = Convert.ToInt16(cmbMiscBranch.CodeValue);
						}
					}
					else
					{
						paraBranchNo = branchNo.Value;
						paraDrawerNo = drawerNo.Value;
					}

					if (this.cbDate.Checked)
					{
						if (dfFrom.UnFormattedValue != null && dfFrom.UnFormattedValue.ToString() != string.Empty)
							paraFromCreateDt = Convert.ToDateTime(dfFrom.UnFormattedValue);
						if (dfTo.UnFormattedValue != null && dfTo.UnFormattedValue.ToString() != string.Empty)
							paraToCreateDt = Convert.ToDateTime(dfTo.UnFormattedValue);

					}

					tempWin = Helper.CreateWindow("phoenix.client.tlcaptureditems", "Phoenix.Client.Teller", "frmTlEndorseItems");
					tempWin.InitParameters(paraBranchNo, paraDrawerNo, paraFromCreateDt, paraToCreateDt);
				}
				//End #79510, #09368
				#region #79502 - us2011
				else if (origin == "PrintBalPrompt")
				{
					bool isMt = false;

					if (_reprintTextQrp == "R" && _tlTranSet.TellerVars.AdTlControl.EnableMtPrint.Value == "Y")
						isMt = true;
					#region Print Prompt
					tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "phoenix.client.tlprinting", "dlgPrintBalPrompt");
					tempDlg.InitParameters(_tlTranSet, isMt, !TellerVars.Instance.IsAppOnline);
					#endregion
				}
				#endregion

				#region Suspect Dtl - #80660
				else if (origin == "SuspectDetail")
				{
					tempWin = CreateWindow("Phoenix.Client.GbSuspectAlertControl", "Phoenix.Client.Global", "frmSuspiciousTranDetail");
					tempWin.InitParameters(decimal.MinValue,
											decimal.MinValue,
											Convert.ToDecimal(colPTID.Text),
											decimal.MinValue,
											colAcctType.UnFormattedValue,
											colAcctNo.UnFormattedValue,
											null,
											null);
				}
				#endregion

                else if (origin == "InventoryItemSearch")   //#140772
                {
                    if (TellerVars.Instance.IsAppOnline)
                    {
                        tempWin = CreateWindow("phoenix.client.gbinventoryitem", "Phoenix.Client.Global", "frmInventoryItemSearch");
                        tempWin.InitParameters(true, -1,
                        ((colRealTranCode.UnFormattedValue != null &&  Convert.ToInt16(colRealTranCode.UnFormattedValue) == 938 &&
                        (colReversalDisplay.UnFormattedValue == null || (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() != "Y"))) ? colRimNo.UnFormattedValue : -1),
                        colTypeId.UnFormattedValue,
                        colBranchNo.UnFormattedValue,
                        colTellerDrawerNo.UnFormattedValue,
                        colRealTranCode.UnFormattedValue,
                        colPTID.UnFormattedValue,
                        ((colRealTranCode.UnFormattedValue != null && Convert.ToInt16(colRealTranCode.UnFormattedValue) == 938 &&
                        (colReversalDisplay.UnFormattedValue == null || (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() != "Y"))) ? colEmplId.UnFormattedValue : -1),
                        (colNonCust.UnFormattedValue != null && Convert.ToString(colNonCust.UnFormattedValue) == "Y")); //#20928 #21274
                    }
                }
				else if (origin == "TlCaptureImageViewer") //#140895 - Teller Capture Integration
				{
					tempWin = CreateWindow("Phoenix.Client.TlJournal", "Phoenix.Client.Journal", "frmTlCaptureImgViewer");

                    if (colSubSequence.UnFormattedValue != null)
                    {
                        int subSeq = Convert.ToInt32(colSubSequence.UnFormattedValue);

                        if (subSeq > 1)
                        {
                            // find main tran
                            int mainPtid = -1;
                            string mainAccount = string.Empty;
                            DateTime effectiveDate = Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue);

                            GetMainSequenceInfo(
                                Convert.ToInt16(colSequenceNo.UnFormattedValue),
                                effectiveDate,
                                ref mainPtid,
                                ref mainAccount);

                            tempWin.InitParameters(mainPtid, colAccount.Text);
                        }
                        else
                        {
                            tempWin.InitParameters(
                                Convert.ToInt32(colPTID.UnFormattedValue),
                                colAccount.Text);
                        }
                    }
				}
                else if (origin == "SavingsBond")   //#140798
                {
                    tempWin = Helper.CreateWindow("phoenix.client.tlredemption", "Phoenix.Windows.TlRedemption", "frmCalcRedemption");
                    tempWin.InitParameters(null, this.ScreenId,
                                            null,
                                            colPTID.UnFormattedValue,
                                            false,
                                            colRimNo.UnFormattedValue);
                }

				if (tempWin != null)
				{
					tempWin.Closed += new EventHandler(tempWin_Closed); //Selva - #74772
					tempWin.Workspace = this.Workspace;
					tempWin.Show();
				}

				else if (tempDlg != null)
				{
					tempDlg.Closed += new EventHandler(tempDlg_Closed); //Selva-ChkPrintFix
					dialogResult = tempDlg.ShowDialog(this);
					AfterDialogClosed(origin);  // #79502
				}
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe, MessageBoxButtons.OK);
				return;
			}
		}

        private void GetMainSequenceInfo(short sequenceNo, DateTime effectiveDate, ref int mainPtid, ref string mainAccount)
        {
            int theMainPtid = -1;

            _busObjTlJournal.GetSequenceMainPtid(
                _busObjTlJournal.BranchNo.Value,
                _busObjTlJournal.DrawerNo.Value,
                effectiveDate,
                sequenceNo,
                out theMainPtid);

            mainPtid = theMainPtid;

            //int originalContextRow = gridJournal.ContextRow;
            //string originalSequenceNo = colSequenceNo.Text;

            //try
            //{
            //    for (int x = originalContextRow - 1; x >= 0; x--)
            //    {
            //        gridJournal.ContextRow = x;

            //        if (colSequenceNo.Text == originalSequenceNo && colSubSequence.Text == "1")
            //        {
            //            mainPtid = Convert.ToInt32(colPTID.UnFormattedValue);
            //            mainAccount = colAccount.Text;

            //            break;
            //        }
            //    }
            //}
            //finally
            //{
            //    gridJournal.ContextRow = originalContextRow;
            //}

            //for (int x = 0; x < gridJournal.Items.Count; x++)
            //{
            //    gridJournal.ContextRow = 
            //}
        }



		private void JouranlGridClick()
		{
			try
			{
				realTranCode = Convert.ToInt16(colRealTranCode.UnFormattedValue);
				//#72358 - moved adrmrestrict call to pbacctdisplay.click

				if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 946)
				{
					//this.pbDisplay.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalTapeDisplay;
					this.pbDisplay.NextScreenId = 0; //#5851
				}
				else
					this.pbDisplay.NextScreenId = Phoenix.Shared.Constants.ScreenId.JournalDisplay;


                /*Begin 175838*/
                //if (_tellerHelper.IsDepositTran(realTranCode))
                //{
                //    if (_adGbAcctType == null || (_adGbAcctType != null && _adGbAcctType.AcctType.Value != colAcctType.Text))
                //    {
                //        _adGbAcctType.AcctType.Value = colAcctType.Text;
                //        CallXMThruCDS("PopulateGbAcctType");
                //    }
                //}
                /*End 175838*/
				EnableDisableVisibleLogic("GridClick");
			}
			catch (PhoenixException pegclick)
			{
				PMessageBox.Show(pegclick);
			}
		}

		private void HandleCtrDefferelOvrd()
		{
			isCtrDeferrelOvrdRequired = false;
			isRevesalOvrdRequired = false;
			if (colCtrStatus.Text != "No")
			{
				if (DialogResult.No == PMessageBox.Show(this, 317896, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
					return;

				isCtrDeferrelOverride = true;
				HandleOverrides(true);
				if (!isCtrDeferrelOvrdRequired)
					HandleReversalOvrd();
			}
			else
			{
				if (!isCtrDeferrelOvrdRequired)
					HandleReversalOvrd();
			}
			return;
		}

		private void HandleReversalOvrd()
		{
			isRevesalOvrdRequired = false;
			isCtrDeferrelOverride = false;
			HandleOverrides(false);
			if (!isRevesalOvrdRequired)
				HandleReversal();
			return;
		}

		//#74772 - Selva
		private void tempWin_Closed(object sender, EventArgs e)
		{
			EnableDisableVisibleLogic("GridClick");
		}

		private void tempDlg_Closed(object sender, EventArgs e) //Selva-ChkPrintFix
		{
			Form form = sender as Form;
			if (form.Name == "dlgAdHocReceipt")
			{
				if (!_reprintFormId.IsNull && _reprintFormId.Value == -1) //the game is really over
					_reprintFormId.Value = 0;
			}
		}

		#region #79502
		/// <summary>
		/// Special processing after dialog has been closed
		/// </summary>
		/// <param name="origin">Dialog origin</param>
		private void AfterDialogClosed(string origin)
		{
			if (origin == "PrintBalPrompt") // #79502
			{
				if (dialogResult == DialogResult.None || dialogResult == DialogResult.Cancel || !TellerVars.Instance.IsAppOnline)
				{
					_printBalOption = "N";
					_printBalances = false;
				}
				else if (dialogResult == DialogResult.OK)
				{
					_printBalOption = "A";
					_printBalances = true;
				}
				else
				{
					_printBalOption = "S";
					_printBalances = true;
				}
			}
		}
		#endregion

		private void HandleOverrides(bool isCtrOverride)
		{
			if (isCtrOverride)
			{
				_tlJournalOvrd = new TlJournalOvrd();
				if (_tlJournalOvrd.RequireCTRReversalOvrd(colCtrStatus.Text, _adGbRsm, _tellerVars.AdTlControl))
				{
					isCtrDeferrelOvrdRequired = true;
					_tlJournalOvrd.OvrdId.Value = OverrideID.CTRReversal;
					_tlJournalOvrd.AcctNo.Value = (colAcctNo.Text == null ? "" : colAcctNo.Text);
					_tlJournalOvrd.AcctType.Value = (colAcctType.Text == null ? "" : colAcctType.Text);
					_tlJournalOvrd.TranCode.Value = Convert.ToInt16(colRealTranCode.UnFormattedValue);
					_tlJournalOvrd.Amount.Value = Convert.ToDecimal(colNetAmt.UnFormattedValue);
					//
					CallOtherForms("Override");
					return;
				}
			}
			else
			{
				_tlJournalOvrd = new TlJournalOvrd();
				if (colSharedBranch.Text == GlobalVars.Instance.ML.Y || _tlJournalOvrd.RequireReversalOvrd(Convert.ToInt16(colEmplId.UnFormattedValue), _adGbRsm))  // #79510, #10883 - Added colSharedBranch
				{
					isRevesalOvrdRequired = true;
					_tlJournalOvrd.OvrdId.Value = OverrideID.Reversal;
					_tlJournalOvrd.AcctNo.Value = colAcctNo.Text;
					_tlJournalOvrd.AcctType.Value = colAcctType.Text;
					_tlJournalOvrd.TranCode.Value = Convert.ToInt16(colRealTranCode.UnFormattedValue);
					_tlJournalOvrd.Amount.Value = Convert.ToDecimal(colNetAmt.UnFormattedValue);
					//
					CallOtherForms("Override");
					return;
				}
			}
		}

		private void HandleReversal()
		{
			if (isCtrDeferrelOvrdSuccess && isReversalOvrdSuccess)
			{
				try
				{
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360305));
					myGridContextRow = gridJournal.ContextRow;

					using (new WaitCursor())
					{
						_tlTranSet.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
						_tlTranSet.DrawerNo.Value = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
						_tlTranSet.EffectiveDt.Value = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
						_tlTranSet.SequenceNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
						_tlTranSet.EmplId.Value = Convert.ToInt16(colEmplId.UnFormattedValue);
						_tlTranSet.PhoenixTeller.Value = 1;
						_tlTranSet.TranSetTotalDispCoinAmt.Value = 0; /*14367*/
						//
						//#9470
						SetCurTran();
						//
						if (_tellerVars.IsTCDEnabled && _isTCDConnected && !_isTcdTcrDeviceIdDifferentFromTran)  //#11784
						{
							if (TcrTotalCashOutAmt.Value > 0 && !_clearPendingTcrTran)
							{
								_tlTranSet.CurTran.Ptid.Value = TcrCashOutJournalPtid.DecimalValue;
								ReverseDeviceDispense(TcrTotalCashOutAmt.Value, 0);
							}
							if (TcrTotalCashInAmt.Value > 0 && (!_clearPendingTcrTran || (_clearPendingTcrTran && _diffIsOnlyCoinAmt)))    //#10787
							{
								_clearPendingTcrTran = false;   //#10787
								_tlTranSet.CurTran.Ptid.Value = TcrCashInJournalPtid.DecimalValue;
								ReverseDeviceDispense(0, TcrTotalCashInAmt.Value);
							}
							//
							if (_clearPendingTcrTran && !_diffIsOnlyCoinAmt)   //#9470-Coin
							{
								//Prompt partial tcr cash reversal message - Todo
								if (_clearPendingTcrTranType == "D")
									PMessageBox.Show(13293, MessageType.Warning, MessageBoxButtons.OK,
										new string[] { CurrencyHelper.GetFormattedValue(TcrTotalCashOutAmt.Value) });
								else
									PMessageBox.Show(13294, MessageType.Warning, MessageBoxButtons.OK,
										new string[] { CurrencyHelper.GetFormattedValue(TcrTotalCashInAmt.Value) });
								//
								if (!IsPendingTcrCashTranCompleted())    //#9470
									return;
							}
						}
						//
						if (_diffIsOnlyCoinAmt && _balCoinAmt > 0) //#9470-Coin
						{
							_tlTranSet.TranSetTotalDispCoinAmt.Value = _balCoinAmt;
							_clearPendingTcrTran = false;
							_clearPendingTcrTranAmt = 0;
							_clearPendingTcrTranType = "";
							_diffIsOnlyCoinAmt = false;
							_balCoinAmt = 0;
						}
						//

						// Begin #140895 - Teller Capture Integration
                        //#28714
                        //if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text))
                        if (!string.IsNullOrWhiteSpace(colTlCaptureISN.Text) &&
                        string.IsNullOrWhiteSpace(colTlCaptureBatchPtid.Text) &&
                        colTlCaptureWorkstation.Text == TellerVars.Instance.TlCaptureWorkstation &&
                        Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper.IsBatchInProgress)  //#111431
                        {
                            if (!Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper.ReverseTransaction(
								Convert.ToInt32(colTlCaptureTranNo.Text),
								colTlCaptureISN.Text))
							{
                                if (!(colTlCaptureImageCommited.Text == "E" || colTlCaptureImageCommited.Text == "F" ||
                                    colTlCaptureImageCommited.Text == "R")) // #40977 - check for uncommitted image - it's ok to fail
								return;
							}
						}
						// End #140895 - Teller Capture Integration

						_tlTranSet.ReverseTransactions(_tellerVars.EmployeeId, ctrRevSuperEmplId, revSuperEmplId, this.colAcctType.Text, this.colAcctNo.Text, messageId, ovrdStatus);   //#15117
					}

					if (!_tlTranSet.ErrorString.IsNull && _tlTranSet.TranPostedStatus.Value == (short)TranPostStatus.Failure)
						PMessageBox.Show(360306, MessageType.Warning, MessageBoxButtons.OK,
							new string[] { _tlTranSet.ErrorString.Value });
					else	// reversal success
					{
                        /*Begin #41426 */
                        /*Bug #65025 - corrected the below code*/
                        /*if (Convert.ToInt32(this.colTranCode.UnFormattedValue) == 345 && Convert.ToDateTime(colCreateDt.UnFormattedValue).Date != Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate.Date) */
                        if (colTranCode.Text == "345" && Convert.ToDateTime(colCreateDt.UnFormattedValue).Date != Phoenix.FrameWork.BusFrame.BusGlobalVars.SystemDate.Date)
                        {
                            TellerVars.Instance.SetContextObject("AdTlTcArray", colTranCode.Text);
                            if (TellerVars.Instance.AdTlTc.RealTimeEnable.Value == GlobalVars.Instance.ML.Y)
                            {
                                bool isActiveStmt = true;
                                isActiveStmt = _tlTranSet.CheckAttachedStatements(this.colAcctType.Text, this.colAcctNo.Text);
                                if (isActiveStmt == false)
                                {
                                    PMessageBox.Show(15403, MessageType.Warning, string.Empty);
                                    /*15403 - A reversal on a payoff has been posted to an account that has an active payment schedule where the Payment Method is Statement. 
                                    The statement record has not been reactivated. Please manually attach a statement to the account. */
                                }
                            }
                        }
                        /*End #41426 */
						/*begin wi 5977*/

						this.cbAccount.Checked = false;
						this.cbAmt.Checked = false;
						this.cbCustomerName.Checked = false;
						this.cbDate.Checked = false;
						this.cbEffectiveDate.Checked = false;
						this.cbMisc.Checked = false;
						this.cbTranEffectiveDt.Checked = false;
						/* end wi 5977*/
						this.cbLocalDateTime.Checked = false; //#8137

						UpdateDrawerBalance();
						_isTransactionRevSuccess = true;    //#9470
						//
						#region #76033
						if (colColdStorComplete.UnFormattedValue != null && colColdStorComplete.Text.Trim() == GlobalVars.Instance.ML.Y)
						{
							ReverseVoucher();
						}
                        //Begin 121336
                        else if (_tlTranSet.TellerVars.IsECMVoucherAvailable)
                        {
                            if (colSubSequence.UnFormattedValue != null && Convert.ToInt32(colSubSequence.UnFormattedValue) > 0)
                            {
                                gridJournal.RowToObject(_busObjTlJournal);

                                if (_busObjTlJournal.IsECMReceiptArchived())
                                {
                                    ReverseVoucher();
                                }
                            }
                        }
                        //End 121336

                        #endregion
                        //
                        JournalSearch();
						//
						if (!_tlTranSet.ErrorString.IsNull && _tlTranSet.TranPostedStatus.Value == (short)TranPostStatus.Success)
							PMessageBox.Show(360306, MessageType.Warning, MessageBoxButtons.OK,
								new string[] { _tlTranSet.ErrorString.Value });
						//
						if (isSupervisor.Value == GlobalVars.Instance.ML.Y)
						{
							PMessageBox.Show(this, 317207, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						}

						/*Begin Enh# 71329*/
						if (_tlTranSet.DisplayCollateralMsg.Value == 1)
						{
							PMessageBox.Show(this, 361033, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
						}
						/*End Enh# 71329*/
						// Begin #76047
						if (_busObjTlJournal.TlTranCode.Value == "913" || _busObjTlJournal.TlTranCode.Value == "922")
						{
							GbBondPurchase bond = new GbBondPurchase();
							bond.JournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
							bond.TfrIdNo.Value = Convert.ToInt32(colTranCode.UnFormattedValue);

							bond.Status.Value = "Closed";
							bond.ActionType = XmActionType.Update;
							bond.DoAction(CoreService.DbHelper);
						}
						// End #76047
					}
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
					if (gridJournal.Count >= myGridContextRow)
						gridJournal.SelectRow(myGridContextRow, true);
					else if (gridJournal.Count > 0)
						gridJournal.SelectRow(0, true);
					isCtrDeferrelOvrdRequired = false;
					isRevesalOvrdRequired = false;
					//
					//EnableDisableVisibleLogic("GridClick");

					//#9470
					if (_isTransactionRevSuccess && (TcrTotalCashInAmt.Value > 0 || TcrTotalCashOutAmt.Value > 0) && !_clearPendingTcrTran && _tellerVars.IsTCDEnabled && _isTCDConnected)
					{
						if (TcrTotalCashInAmt.Value > 0)
						{
							_tlTranSet.CurTran.Ptid.Value = TcrCashInJournalPtid.DecimalValue;
							//_tlTranSet.SaveCashInDenom(_tlTranSet.CurTran, true);
							if (colTranCode.Text.Trim() == "VLS")
								_tlTranSet.SaveCashOutDenom(true, "S", _tlTranSet.CurTran, true);
							else
								_tlTranSet.SaveCashOutDenom(true, "D", _tlTranSet.CurTran, true);
						}
						if (TcrTotalCashOutAmt.Value > 0)
						{
							_tlTranSet.CurTran.Ptid.Value = TcrCashOutJournalPtid.DecimalValue;
							if (colTranCode.Text.Trim() == "VLT")
								_tlTranSet.SaveCashOutDenom(true, "B", _tlTranSet.CurTran, true);
							else
								_tlTranSet.SaveCashOutDenom(true, "I", _tlTranSet.CurTran, true);

							if (!_tlTranSet.TranSetTotalDispCoinAmt.IsNull && _tlTranSet.TranSetTotalDispCoinAmt.Value > 0) //#9470-Coin
								_tlTranSet.SaveCashOutDenom(true, "C", _tlTranSet.CurTran, true);
						}
					}
					JouranlGridClick();
					gridJournal.Focus();

                    Helper.SendMessageToMDI((int)GlobalActions.ChildAction, this, "TranCompleted");     //#195669, #35513
				}
			}
		}

		private void GetCheckInfo()
		{
			if (_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "C") ||
				_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "S") ||    //#76057
				_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "Q"))
				CallOtherForms("CheckInfo");
		}

		//#80615 - added string parameter
        private bool LoadAndRePrint(string printMethod) //#194535
		{
            //begin #194535
            bool isReceipt = false;
            bool skipTran = false;
            bool rePrint = false;
            bool skip = false;
            bool isEmail = false;              //#41316
            bool bIsHylandServiceAvailable = _tlTranSet.IsHylandVoucherSvcAvailable();
            Boolean isPrintAndEmail = false;    //#41316
            byte[] image = null;
            //end   #194535

			#region load print info and print
			if ((_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "T") ||
				_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "S") ||    //#76057
				_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "P") ||    //#76409
				_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "C"))
                || printMethod == "MT") // #80615 //#41316
			{
				#region reset cur tran
				_tlTranSet.CurTran.CleanUpValues();
				if (_tlTranSet.Transactions != null && _tlTranSet.Transactions.Count > 0)
					_tlTranSet.Transactions.Clear();
				#endregion
				//
				#region add tcd fields

				/* Begin #72916 - Replaced colTCDDrawerNo to colTCDDrawerPosition */
				/*WI#15138- Commented the if -condition*/
				/*if (colTCDDrawerPosition.UnFormattedValue != null)
				{
					_tlTranSet.CurTran.TcdCashOut.Value = Convert.ToDecimal(colTCDCashOut.UnFormattedValue);
					_tlTranSet.CurTran.TcdCashIn.Value = Convert.ToDecimal(colTCDCashIn.UnFormattedValue);
					_tlTranSet.CurTran.TcdDrawerNo.Value = Convert.ToInt16(colTCDDrawerNo.UnFormattedValue);

				}*/
				if ((colTCDCashOut.UnFormattedValue) != null && Convert.ToDecimal(colTCDCashOut.UnFormattedValue) > 0)
					_tlTranSet.CurTran.TcdCashOut.Value = Convert.ToDecimal(colTCDCashOut.UnFormattedValue);
				if ((colTCDCashIn.UnFormattedValue) != null && Convert.ToDecimal(colTCDCashIn.UnFormattedValue) > 0)
					_tlTranSet.CurTran.TcdCashIn.Value = Convert.ToDecimal(colTCDCashIn.UnFormattedValue);
				if ((colTCDDrawerNo.UnFormattedValue) != null && Convert.ToInt16(colTCDDrawerNo.UnFormattedValue) > 0)
					_tlTranSet.CurTran.TcdDrawerNo.Value = Convert.ToInt16(colTCDDrawerNo.UnFormattedValue);
				/*end WI#15138*/
				#endregion
				//
				#region add common print fields
				_tlTranSet.CurTran.CashIn.Value = Convert.ToDecimal(colCashInAmt.UnFormattedValue);
				_tlTranSet.CurTran.CashOut.Value = Convert.ToDecimal(colCashOutAmt.UnFormattedValue);
				if (colCheckNo.UnFormattedValue != null && Convert.ToInt32(colCheckNo.UnFormattedValue) > 0)
					_tlTranSet.CurTran.CheckNo.Value = Convert.ToInt32(colCheckNo.UnFormattedValue);
				_tlTranSet.CurTran.ChksAsCash.Value = Convert.ToDecimal(colChksAsCashAmt.UnFormattedValue);
				_tlTranSet.CurTran.CcAmt.Value = Convert.ToDecimal(colCCAmt.UnFormattedValue);
				_tlTranSet.CurTran.IntAmt.Value = Convert.ToDecimal(colIntAmt.UnFormattedValue);
				_tlTranSet.CurTran.OnUsChks.Value = Convert.ToDecimal(colOnUsChksAmt.UnFormattedValue);
				_tlTranSet.CurTran.RimNo.Value = Convert.ToInt32(colRimNo.UnFormattedValue);
				_tlTranSet.CurTran.SequenceNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
				_tlTranSet.CurTran.NetAmt.Value = Convert.ToDecimal(colAmt.UnFormattedValue);
				_tlTranSet.CurTran.TransitChks.Value = Convert.ToDecimal(colTransitChksAmt.UnFormattedValue);
				_tlTranSet.CurTran.UtilityId.Value = Convert.ToInt16(colUtilityId.UnFormattedValue);
				_tlTranSet.CurTran.AcctNo.Value = Convert.ToString(colAcctNo.Text);
				_tlTranSet.CurTran.AcctType.Value = Convert.ToString(colAcctType.Text);
				_tlTranSet.CurTran.TfrAcctType.Value = colTfrAcctType.Text;
				_tlTranSet.CurTran.TfrAcctNo.Value = colTfrAcctNo.Text;
				_tlTranSet.CurTran.TfrAcct.AcctType.Value = colTfrAcctType.Text; // #13758
				_tlTranSet.CurTran.TfrAcct.AcctNo.Value = colTfrAcctNo.Text; // #13758
				//
				_tlTranSet.CurTran.TlTranCode.Value = colTranCode.Text;
				_tlTranSet.CurTran.TranCode.Value = Convert.ToInt16(colRealTranCode.UnFormattedValue);
				if (colBatchId.UnFormattedValue != null && Convert.ToInt32(colBatchId.UnFormattedValue) > 0)
					_tlTranSet.CurTran.BatchId.Value = Convert.ToInt16(colBatchId.UnFormattedValue);
				if (colItemCount.UnFormattedValue != null && Convert.ToInt32(colItemCount.UnFormattedValue) > 0)
					_tlTranSet.CurTran.ItemCount.Value = Convert.ToInt16(colItemCount.UnFormattedValue);
				if (colTfrChkNo.UnFormattedValue != null && Convert.ToInt32(colTfrChkNo.UnFormattedValue) > 0)
					_tlTranSet.CurTran.TfrChkNo.Value = Convert.ToInt32(colTfrChkNo.UnFormattedValue);
				_tlTranSet.CurTran.Reference.Value = colReference.Text;
				_tlTranSet.CurTran.EmplId.Value = Convert.ToInt16(colEmplId.UnFormattedValue); //#71068
				_tlTranSet.CurTran.CreateDt.Value = Convert.ToDateTime(colCreateDt.UnFormattedValue);
				_tlTranSet.CurTran.TranEffectiveDt.Value = Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue);
				//
				//#73098
				if (!_tlTranSet.CurTran.AcctNo.IsNull && _tlTranSet.CurTran.AcctNo.Value != string.Empty &&
				   _tlTranSet.CurTran.AcctNo.Value != "")
				{
					if (_tlTranSet.CurTran.AcctType.Value == GlobalVars.Instance.ML.RIM &&
						Convert.ToInt64(_tlTranSet.CurTran.AcctNo.Value) <= DbInt.MaxValue)
						_tlTranSet.CurTran.RimNo.Value = Convert.ToInt32(colAcctNo.Text);
				}
				//
				if (colAcctType.Text != string.Empty && colAcctType.Text != "")
				{
                    //#38309 - I am uncommenting the Performance optimization fix added below to reduce the number of CDS call because we need to make these calls for reprint to work
                    _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;	// #71853 - Added Trim
                    //
                    if (_acctTypeDetails != null)		// #71853 - Added the condition
                    {
                        _tlTranSet.CurTran.TranAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                        _tlTranSet.CurTran.TranAcct.ApplType.Value = _acctTypeDetails.ApplType;
                    }
                    ////#73098

                    /* 175838 */
                    //if (colAcctType != null)		// #71853 - Added the condition  #38309
                    //{
                    //    //_tlTranSet.CurTran.TranAcct.DepLoan.Value = colDepType.UnFormattedValue.ToString();
                    //    if (colApplType.UnFormattedValue != null)
                    //        _tlTranSet.CurTran.TranAcct.ApplType.Value = colApplType.UnFormattedValue.ToString();
                    //}
                    //#73098
                    //End #38309

					if (_tlTranSet.CurTran.AcctType.Value == GlobalVars.Instance.ML.RIM &&
					(_tlTranSet.CurTran.AcctNo.IsNull || _tlTranSet.CurTran.AcctNo.Value == string.Empty ||
					_tlTranSet.CurTran.AcctNo.Value == ""))
					{
						_tlTranSet.CurTran.TranAcct.AcctNo.Value = colRimNo.Text;
						_tlTranSet.CurTran.AcctNo.Value = colRimNo.Text;
					}
					else
					{
						_tlTranSet.CurTran.TranAcct.AcctNo.Value = Convert.ToString(colAcctNo.Text);
					}
					_tlTranSet.CurTran.TranAcct.AcctType.Value = Convert.ToString(colAcctType.Text);
                    _tlTranSet.CurTran.TranAcct.RimNo.Value = _tlTranSet.CurTran.RimNo.Value;   //#42555
					//
					#region acct details
					try
					{
						using (new WaitCursor())
						{
							_tlTranSet.CurTran.TranAcct.GetAcctDetails();
						}
					}
					catch (PhoenixException pe)
					{
						PMessageBox.Show(pe, MessageBoxButtons.OK);
                        return false;   //#194535
					}
					#endregion
					//
					//_tlTranSet.CurTran.TranAcct.Title1.Value = _acctDetails.Title1.Value;
					//#73098 - added null validation for Acct#
					if (!_tlTranSet.CurTran.TranAcct.AcctNo.IsNull &&
						_tlTranSet.CurTran.TranAcct.AcctNo.Value != string.Empty &&
						_tlTranSet.CurTran.TranAcct.AcctNo.Value != "" && _tlTranSet.CurTran.TranAcct.AcctType.Value == GlobalVars.Instance.ML.RIM &&
						Convert.ToInt64(_tlTranSet.CurTran.TranAcct.AcctNo.Value) <= DbInt.MaxValue)
						_tlTranSet.CurTran.TranAcct.RimNo.Value = Convert.ToInt32(_tlTranSet.CurTran.TranAcct.AcctNo.Value);
					else
					{
						_tlTranSet.CurTran.TranAcct.RimNo.Value = Convert.ToInt32(colRimNo.UnFormattedValue);
						// begin 16279
						#region Get Customer Information
						TlAcctDetails tlAcctDetails = new TlAcctDetails();
						tlAcctDetails.AcctNo.Value = Convert.ToString(_tlTranSet.CurTran.TranAcct.RimNo.Value);
						tlAcctDetails.RimNo.Value = _tlTranSet.CurTran.TranAcct.RimNo.Value;
						tlAcctDetails.ApplType.Value = GlobalVars.Instance.ML.RIM;
						tlAcctDetails.DepLoan.Value = GlobalVars.Instance.ML.RM;
						tlAcctDetails.AcctType.Value = GlobalVars.Instance.ML.RIM;
						try
						{
							using (new WaitCursor())
							{
								tlAcctDetails.GetAcctDetails();
							}
						}
						catch (PhoenixException pe)
						{
							PMessageBox.Show(pe, MessageBoxButtons.OK);
						}

						#endregion
						_tlTranSet.CurTran.TranAcct.RimNo.Value = tlAcctDetails.RimNo.Value;
						_tlTranSet.CurTran.TranAcct.RimFirstName.Value = tlAcctDetails.RimFirstName.Value;
						_tlTranSet.CurTran.TranAcct.RimLastName.Value = tlAcctDetails.RimLastName.Value;
						_tlTranSet.CurTran.TranAcct.RimMiddleInitial.Value = tlAcctDetails.RimMiddleInitial.Value;

					}//end 16279
					_tlTranSet.CurTran.TranAcct.TranCode.Value = Convert.ToInt16(colRealTranCode.UnFormattedValue);

				}
				#region #13758
				if (colTfrAcctType.Text != string.Empty && colTfrAcctType.Text != "")
				{
                    /* 175838 */
                    //#38309 - I am uncommenting the Performance optimization fix added below to reduce the number of CDS call because we need to make these calls for reprint to work
                    //_acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;
                    //
                    //if (_acctTypeDetails != null)
                    //{
                    //    _tlTranSet.CurTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                    //    _tlTranSet.CurTran.TfrAcct.ApplType.Value = _acctTypeDetails.ApplType;
                    //}
                    /* 175838 */
                    //if (colTfrAcctType.UnFormattedValue != null)
                    //{
                    //    //_tlTranSet.CurTran.TfrAcct.DepLoan.Value = colDepType.UnFormattedValue.ToString();
                    //    //_tlTranSet.CurTran.TfrAcct.ApplType.Value = colApplType.UnFormattedValue.ToString();

                    //}
                    _acctTypeDetails = _globalHelper.GetAcctTypeDetails(colTfrAcctType.Text.Trim(), string.Empty) as AcctTypeDetail;

                    if (_acctTypeDetails != null)
                    {
                        _tlTranSet.CurTran.TfrAcct.DepLoan.Value = _acctTypeDetails.DepLoan;
                        _tlTranSet.CurTran.TfrAcct.ApplType.Value = _acctTypeDetails.ApplType;
                    }

					#region acct details
					try
					{
						using (new WaitCursor())
						{
							_tlTranSet.CurTran.TfrAcct.GetAcctDetails();
						}
					}
					catch (PhoenixException pe)
					{
						PMessageBox.Show(pe, MessageBoxButtons.OK);
                        return false;   //#194535
					}
					#endregion
				}
				#endregion
				//Begin #79510, #10883-2
				else if (colSharedBranch.Text == GlobalVars.Instance.ML.Y)
					_tlTranSet.CurTran.TranAcct.AcctNo.Value = colAcctNo.Text;
				//End #79510, #10883-2
				//
				#endregion
				//
				#region add key fields
				_tlTranSet.CurTran.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
				if (colTellerDrawerNo.UnFormattedValue != null)
					_tlTranSet.CurTran.DrawerNo.Value = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
				_tlTranSet.CurTran.EffectiveDt.Value = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
				_tlTranSet.CurTran.SequenceNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
				_tlTranSet.CurTran.SubSequence.Value = Convert.ToInt16(colSubSequence.UnFormattedValue);
				_tlTranSet.CurTran.Ptid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
				_tlTranSet.CurTran.XpPtid.Value = Convert.ToDecimal(colXpPtid.UnFormattedValue); //74269
				#endregion
				//
				#region add utility fields
				if (colUtilityId.UnFormattedValue != null && Convert.ToInt32(colUtilityId.UnFormattedValue) > 0)
				{
					_tlTranSet.CurTran.UtilityId.Value = Convert.ToInt16(colUtilityId.UnFormattedValue);
				}
				#endregion
				//
				#region add tran status fields
				_tlTranSet.CurTran.TranStatus.Value = Convert.ToInt32(colTranStatus.UnFormattedValue);
				_tlTranSet.CurTran.PostRealTime.Value = (Convert.ToInt32(colTranStatus.UnFormattedValue) == 9 ? Convert.ToInt16(1) : Convert.ToInt16(0));
				#endregion

				#region #79569 add local date/time/zone fields
				_tlTranSet.CurTran.LocalCreateDt.Value = Convert.ToDateTime(colLocalDtTime.UnFormattedValue);
				_tlTranSet.CurTran.LocalTimeZone.Value = colLocalTZ.Text;
				#endregion

				//
				//Begin #76052
				if (colOdpNsfCcAmt.UnFormattedValue != null && Convert.ToInt32(colOdpNsfCcAmt.UnFormattedValue) > 0)
					_tlTranSet.CurTran.OdpNsfCcAmt.Value = Convert.ToDecimal(colOdpNsfCcAmt.UnFormattedValue);
				//End #76052

				//
				if (printMethod != "MT") // #80615
				{
					#region get next print object info
					int formId = _reprintFormId.Value;
					int copiesCount = 0;
					bool collectChkInfo = false;
					//#4542
					_wosaPrintInfo.PrintSourceScreenId = this.ScreenId;
					try
					{
						_tlTranSet.IsAcquirerTran = (colSharedBranch.Text == GlobalVars.Instance.ML.Y); // #79510, #10883-2

						_tlTranSet.GetNextPrintObject(_tlTranSet.CurTran, ref formId, ref copiesCount,
							ref collectChkInfo, ref _wosaPrintInfo);
					}
					finally
					{

						_tlTranSet.IsAcquirerTran = false;  // #79510, #10883-2
					}
					#endregion
					//
					#region append additional print info
					//_wosaPrintInfo.AcctTitle1 = CoreService.Translation.GetUserMessageX(360615); //Unknown
					//
					#region calculator tape
					_calculatorTape = string.Empty;
					if (colTranCode.Text == "CLC")
					{
						_calculatorTape = colCalcData1.Text + colCalcData2.Text;
						if (_calculatorTape == string.Empty || _calculatorTape == "")
						{
							_busObjTlJrnlCalc.JournalPtid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
							CallXMThruCDS("Calculator");
							_calculatorTape = _busObjTlJrnlCalc.CalcData.Value;
						}
						_calculatorTape = _calculatorTape.Replace("\n", "\r\n");
						_wosaPrintInfo.CalculatorTape = _calculatorTape;
					}
					#endregion
					//
					#region BAT Items
					if (colTranCode.Text == CoreService.Translation.GetUserMessageX(360092))
					{
						_busObjTlJournal.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
						_busObjTlJournal.DrawerNo.Value = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
						_busObjTlJournal.EffectiveDt.Value = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
						_busObjTlJournal.BatchId.Value = Convert.ToInt16(colBatchId.UnFormattedValue);
						_busObjTlJournal.Ptid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
						_tempPrintInfo = _busObjTlJournal.GetBatchPrintInfo(_partialPrintString);
						if (_tempPrintInfo.BatchItems != string.Empty)
							_wosaPrintInfo.BatchItems = _tempPrintInfo.BatchItems;
						if (_tempPrintInfo.ItemCount > 0)
							_wosaPrintInfo.ItemCount = _tempPrintInfo.ItemCount;
					}
					#endregion
					//
					#region get check info
					GetCheckInfo();
					#endregion
					//
					// _wosaPrintInfo.TlTranCodeDesc = colDescription.Text;16278
					//begin 16279, 12-14-2011 use tltrancode instead of trancode
					#region Get  Trancode desc from adtltc
					AdTlTc adTlTc = new AdTlTc();
					adTlTc.TlTranCode.Value = Convert.ToString(_tlTranSet.CurTran.TlTranCode.Value);
					adTlTc.SelectAllFields = true;
					adTlTc.ActionType = XmActionType.Select;
					CoreService.DataService.ProcessRequest(adTlTc);
					_wosaPrintInfo.TlTranCodeDesc = adTlTc.Description.Value;
					#endregion
					//end 16279
					//_wosaPrintInfo.TlTranCodeDesc = colDescription.Text; /*16279*/
					_wosaPrintInfo.Description = colTranDescription.Text;

					//Reference Account
					if (Convert.ToInt32(colRealTranCode.UnFormattedValue) == 500 ||
						Convert.ToInt32(colRealTranCode.UnFormattedValue) == 550 ||
						Convert.ToInt32(colRealTranCode.UnFormattedValue) == 928)	//#71668 - added TC928
					{
						_wosaPrintInfo.RefAcctNo = colReferenceAcct.Text;
					}
					// Begin #72248 //
					tempRefAcctNo = colReferenceAcct.Text;
					indexOfDash = tempRefAcctNo.IndexOf('-');
					if (indexOfDash != -1)
					{
						_wosaPrintInfo.RefAcctNoOnly = tempRefAcctNo.Substring(indexOfDash + 1);
						_wosaPrintInfo.RefAcctType = tempRefAcctNo.Substring(0, indexOfDash);
					}
					// End #72248 //

					#endregion

					#region handle printing
					try
					{
						/*-----#74269 - Commented original code-----
						if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
						{
							_xfsPrinter.PrintForm(PApplication.Instance.MdiMain, _wosaServiceName, _mediaName, _formName, _wosaPrintInfo);
						}
						-------#74269 - End commented code-------*/
						#region Get Float Lines - #74269
						string[] asRegCCHolds = null;
						bool skipChecks = false;
						bool skipPrintWithOfacMatch = false;    // #76057

						if (_reprintTextQrp == CoreService.Translation.GetListItemX(ListId.TextType, "S") && !_reprintFormId.IsNull && _tlTranSet.IsCheckPrintAvailable(true, _reprintFormId.Value, false))    //#1841 #1871
						{
							#region Check/Money Order Additional Processing- #76057
							string copyStatus = Phoenix.FrameWork.CDS.DataService.Instance.XmDbStatus.ToString().Substring(0, 1).ToUpper();
							bool isPrimaryDb = (copyStatus == "D" || copyStatus == "C");
							#region OfacSearch
							bool isFound = false;
							if (!_tlTranSet.SearchGbOfacSdnList(_wosaPrintInfo.CheckLine1, Convert.ToInt32(colRimNo.UnFormattedValue)))
							{
								if (_tlTranSet.SearchGbOfacSdnList(_wosaPrintInfo.CheckLine2, Convert.ToInt32(colRimNo.UnFormattedValue)))
									isFound = true;
							}
							else
								isFound = true;
							if (isFound)
							{
								// 11/07/2008 - It was decided not to prompt user to print check - just print always
								//if (PMessageBox.Show(this, 10788, MessageType.Message, MessageBoxButtons.YesNo) == DialogResult.No)
								//{
								//    // do not print check
								//    skipPrintWithOfacMatch = true;
								//}
								PMessageBox.Show(this, 10788, MessageType.Message, MessageBoxButtons.OK);
							}
							#endregion

							if (!skipPrintWithOfacMatch)
							{
								_tlTranSet.Transactions.Add(_tlTranSet.CurTran); //Follow posting trancode logic

								// Regen a new check. Assign new Check No etc, number (Server side)
								int checkNo = Convert.ToInt32(colCheckNo.Text);
								_tlTranSet.SaveTlChkFormInfo(_wosaPrintInfo, true, -1, ref checkNo);

								_tlTranSet.CurTran.CheckNo.Value = checkNo;

								// Print Check
								_wosaPrintInfo.CheckNo = _tlTranSet.CurTran.CheckNo.Value; //9591
								_wosaPrintInfo.JournalPtid = _tlTranSet.CurTran.Ptid.Value; //9591
                                _wosaPrintInfo.SbSeqNo = _tlTranSet.CurTran.SubSequence.StringValue;    //#120397
								GenerateChkPrintingReport(_wosaPrintInfo); //9591

								gridJournal.PopulateTable();
							}
							#endregion
                            return false;   //#194535
						}


						if (_wosaPrintInfo.RegCCHolds != null)
						{
                            if (_wosaPrintInfo.RegCCHolds.Contains("^"))
                                asRegCCHolds = _wosaPrintInfo.RegCCHolds.Split('^');
                            else
                                asRegCCHolds = _wosaPrintInfo.RegCCHolds.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);   //#42567 split holds based on new lines
                            if (_adGbBankControl.RegCcEsignChange.Value == "N") //#43143
                            {
                                isPrintRequired = true;  //Disable email radio button in dlgReceiptInfo.
                            }
						}
						#endregion
						#region #79502
						_printTfrBalances = true;
						_printPrimaryBalances = true;

						#region #17166
						if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value != "Y")
						{
							if ((thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true) || (colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
							{
								_tlTranSet.CurTran.PrintBalance.Value = "N";
								thisTran.PrintBalance.Value = "N";
								_printPrimaryBalances = false;
							}
							if ((!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != "") && thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true) || (colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
							{
								_tlTranSet.CurTran.PrintTfrBalance.Value = "N";
								thisTran.PrintTfrBalance.Value = "N";
								_printTfrBalances = false;
							}
							_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
						}
						#endregion

						if (_printBalOption == "A")
						{
							if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TranAcct.bAcctHasPostAccessOnly.Value == true
								&& !((_tlTranSet.GlobalHelper.IsConfAcct(thisTran.TranAcct.AcctType.Value, thisTran.TranAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
								&& !(thisTran.TranAcct.TranCode.Value >= 300 && thisTran.TranAcct.TranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
								&& !(thisTran.TranAcct.DepLoan.Value == "GL")
								&& !(thisTran.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(thisTran.TranAcct.TranCode.Value))
								&& !(colSharedBranch.Text == GlobalVars.Instance.ML.Y))/*19703*/
							{
								_tlTranSet.CurTran.PrintBalance.Value = "Y";
								thisTran.PrintBalance.Value = "Y";
							}
							else
							{
								_tlTranSet.CurTran.PrintBalance.Value = "N";
								thisTran.PrintBalance.Value = "N";
							}
							if (!_tlTranSet.CurTran.TfrTranCode.IsNull || (_tlTranSet.CurTran.TfrTranCode.IsNull && !_tlTranSet.CurTran.TfrAcctNo.IsNull && _tlTranSet.CurTran.TfrAcctNo.Value.Trim() != ""))
							{
								if (_tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y" && !thisTran.TfrAcct.bAcctHasPostAccessOnly.Value == true
									&& !((_tlTranSet.GlobalHelper.IsConfAcct(thisTran.TfrAcct.AcctType.Value, thisTran.TfrAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag))
									&& !(_tlTranSet.CurTran.TfrTranCode.Value >= 300 && _tlTranSet.CurTran.TfrTranCode.Value <= 399 && _tlTranSet.AdTlTc.RealTimeEnable.Value != "Y")
									&& !(thisTran.TfrAcct.DepLoan.Value == "GL")
									&& !(thisTran.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranSet.CurTran.TfrTranCode.Value))
									&& !(colSharedBranch.Text == GlobalVars.Instance.ML.Y)) // #13713 /*19703*/
								{
									_tlTranSet.CurTran.PrintTfrBalance.Value = "Y";
									thisTran.PrintTfrBalance.Value = "Y";
								}
								else
								{
									_tlTranSet.CurTran.PrintTfrBalance.Value = "N";
									thisTran.PrintTfrBalance.Value = "N";
								}
							}
						}
						if ((!_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y") || (colSharedBranch.Text == GlobalVars.Instance.ML.Y))// #79502/*19703*/
						{
							_printPrimaryBalances = false;
							_printTfrBalances = false;
							_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
						}
						if (_printBalances && _tlTranSet.AdTlTc.IncludePrtPrompt.Value == "Y")
						{
							if (thisTran.PrintBalance.Value == "N")
								_printPrimaryBalances = false;
							if ((thisTran.TfrTranCode.IsNull && (thisTran.TfrAcctNo.IsNull || thisTran.TfrAcctNo.Value.Trim() == "")) || thisTran.PrintTfrBalance.Value == "N")
								_printTfrBalances = false;
							if (!_printPrimaryBalances || !_printTfrBalances)
								_wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
						}
						#endregion
						#region #12881
						//#79502 Commented - if (!_printBalances)
						//#79502 Commented -     _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
						#endregion

                        //begin #194535
                        isReceipt = _tlTranSet.TellerVars.AdTlForm.ServiceType.Value == "Receipt";    //#194535
                        if (_tlTranSet.TellerVars.AdTlControl.EnableEreceipt.Value == "Y" && isReceipt)
                        {
                            _tlTranSet.CurTran.TranAcct.FromMTForms.Value = "N";
                            // Show dlgReceiptInfo
                            CallOtherForms("eReceipt");
                            if (dialogResult == DialogResult.No || dialogResult == DialogResult.OK)
                            {
                                if (dialogResult == DialogResult.OK)
                                {
                                    if (myInstruction.ReceiptDelMethod == PRINTANDEMAIL)
                                    {
                                        isPrintAndEmail = true; //#41316 Print the form and tell Hyland to archive this form. When it archives it it will email it.
                                    }
                                    else if (myInstruction.ReceiptDelMethod == EMAIL)
                                    {
                                        isEmail = true;    //#41316 Tell Hyland to archive this form. When it archives it it will email it.
                                    }
                                    //begin #40196
                                    else if (myInstruction.ReceiptDelMethod == PRINT)
                                    {
                                        emailAddress = "";  //Do not email this form.
                                    }
                                    //end   #40196
                                }
                            }
                            skipTran = dialogResult == DialogResult.Cancel;
                            rePrint = dialogResult == DialogResult.Retry;
                            skip = dialogResult == DialogResult.Ignore;
                        }
                        //end   #194535

                        if (rePrint || !(skipTran || skip))    //#194535
                        {
                            #region Print Single/Multiple Forms - #74269
                            if (asRegCCHolds != null)
                            {
                                if (asRegCCHolds.Length <= 3)
                                {
                                    _wosaPrintInfo.RegCCHolds = _wosaPrintInfo.RegCCHolds.Replace("^", " | ");
                                    if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
                                    {
                                        if (isPrintAndEmail || isEmail)    //#41316
                                        {
                                            PrintEmailForm(ref image, ref isEmail);
                                        }
                                        else
                                        {
                                            _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
                                        }
                                    }
                                }
                                else
                                {
                                    string holdInfo = "";
                                    isReadOnly = false;    //#42567 Do not disable screen edits in dlgReceiptInfo
                                    for (int b = 0; b < asRegCCHolds.Length; b++)
                                    {
                                        if (b == asRegCCHolds.Length - 1)
                                            holdInfo += asRegCCHolds[b];
                                        else
                                            holdInfo += asRegCCHolds[b] + " | ";
                                        if (b == 2)
                                        {
                                            if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
                                            {
                                                _wosaPrintInfo.RegCCHolds = holdInfo;
                                                if (isPrintAndEmail || isEmail)    //#41316
                                                {
                                                    PrintEmailForm(ref image, ref isEmail);
                                                }
                                                else
                                                {
                                                    _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
                                                }
                                                holdInfo = "";
                                            }
                                        }
                                        else if ((b + 1) % 3 == 0 || b == asRegCCHolds.Length - 1)
                                        {
                                            _wosaPrintInfo.RegCCHolds = holdInfo;
                                            if (_tellerVars.AdTlControl.EnableEreceipt.Value == "Y") //#41316
                                            {
                                                isReadOnly = true;    //Disable screen edits in dlgReceiptInfo
                                                CallOtherForms("eReceipt");
                                                if (dialogResult == DialogResult.No || dialogResult == DialogResult.OK) //#42567 Set email flags
                                                {
                                                    if (dialogResult == DialogResult.OK)
                                                    {
                                                        if (myInstruction.ReceiptDelMethod == PRINTANDEMAIL)
                                                        {
                                                            isPrintAndEmail = true; //#41316 Print the form and tell Hyland to archive this form. When it archives it it will email it.
                                                        }
                                                        else if (myInstruction.ReceiptDelMethod == EMAIL)
                                                        {
                                                            isEmail = true;    //#41316 Tell Hyland to archive this form. When it archives it it will email it.
                                                        }
                                                        //begin #40196
                                                        else if (myInstruction.ReceiptDelMethod == PRINT)
                                                        {
                                                            emailAddress = "";  //Do not email this form.
                                                        }
                                                        //end   #40196
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CallOtherForms("PrintForm");
                                            }
                                            holdInfo = "";
                                            skipTran = dialogResult == DialogResult.Cancel;
                                            rePrint = dialogResult == DialogResult.Retry;
                                            skipChecks = dialogResult == DialogResult.Abort;
                                            skip = dialogResult == DialogResult.Ignore;
                                            if (rePrint || skipTran || skipChecks || skip) // Break out of print loop   //#194535
                                                break;
                                            //begin #42567
                                            if (isPrintAndEmail || isEmail)   //Email the reg CC changes
                                            {
                                                PrintEmailForm(ref image, ref isEmail);
                                            }
                                            else
                                            {
                                                _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo); //#157637
                                            }
                                            //end #42567
                                        }
                                    }
                                    isReadOnly = false;    //#42567 Do not disable screen edits in dlgReceiptInfo
                                }
                            }
                            else
                            {
                                #region #12881
                                //#79502 Commented - if (!_printBalances)
                                if (!_printPrimaryBalances || !_printTfrBalances) // #79502
                                    _wosaPrintInfo = SuppressBalances(_wosaPrintInfo);
                                #endregion

                                if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
                                {
                                    if (isPrintAndEmail || isEmail)     //#194535  #41316 Moved below 
                                    {
                                        PrintEmailForm(ref image, ref isEmail);
                                    }
                                    else
                                    {   //#41344
                                        _xfsPrinter.PrintForm(_mediaName, _formName, _wosaPrintInfo);	//#157637
                                    }
                                }
                            }
                            #endregion
                            
                            _tlTranSet.UpdateRimReceiptandEmail();  //#40276

                        }
					}
					finally
					{
					}
					#endregion
				}
			}
			#endregion
            //begin #194535
            if (skipTran || skip)
            {
                return true;
            }
            else
            {
                return false;
            }
            //end   #194535
		}

        private void PrintEmailForm( ref byte[] image, ref bool isEmail)   //#41316
        {
            // Print and email a form

            combinedBitmap = null;
            _xfsPrinter.UseImaging = true;
            _printCustName = _wosaPrintInfo.CustomerName;
            _printRimNo = _tlTranSet.CurTran.TranAcct.RimNo.Value;
            _xfsPrinter.PrintFormAndReturnImage(_mediaName, _formName, _wosaPrintInfo, out image, isEmail);    // Print and/or create an image to email.
            combinedBitmap = new Bitmap(new MemoryStream((byte[])image));   //Turn the image into a bitmap
            //_printer.GetCombinedBitmapAndSigBoxDetails(out combinedBitmap, out sigPos);
            sigPos = new SigBoxDetails();
            _xfsPrinter.ClearBitmaps();
            if (_printCustName == null)
            {
                _printCustName = "";
            }
            sigPos.CombinedformHeight = combinedBitmap.Height;
            sigPos.CombinedFormWidth = combinedBitmap.Width;
            //Phoenix.Windows.Client.frmTlTranCode.SignAndArchiveVoucher(combinedBitmap, sigPos, printInfo, mtRequireSignature, true, emailAddress);
            SignAndArchiveVoucher(combinedBitmap, sigPos, _wosaPrintInfo, false, false, emailAddress);  //Tell Hyland to email the form.
        }

		#endregion

		#region helper methods
		private void PopulateAcctTypeCombo(PComboBoxStandard acctTypeField, bool appendRM, bool appendGL)
		{

			try
			{
				string codeValue = null;
				string desc = null;
				bool acctTypeSrch = (_tellerVars.AdTlControl.AcctTypeSearch.Value == "Y");

				if (this._tellerVars.ComboCache["frmTlJournal.cmbAcctType"] == null)
				{
					this._tellerHelper.TlWhereClause.Value = (acctTypeSrch ? "Y" : "N");
					CallXMThruCDS("AcctTypePopulate");
					this._tellerVars.ComboCache["frmTlJournal.cmbAcctType"] = this._tellerHelper.AcctType.Constraint.EnumValues;
				}
				acctTypeField.Populate(this._tellerVars.ComboCache["frmTlJournal.cmbAcctType"]);

				if (appendRM)
				{
					if (acctTypeSrch)
						codeValue = GlobalVars.Instance.ML.RIM;
					else
						codeValue = GlobalVars.Instance.ML.RM;

					desc = GlobalVars.Instance.ML.RM + "~" + GlobalVars.Instance.ML.RM + "~";

					acctTypeField.Append(codeValue, desc);
				}

				if (appendGL)
				{
					codeValue = GlobalVars.Instance.ML.GL;
					desc = GlobalVars.Instance.ML.GL + "~" + GlobalVars.Instance.ML.GL + "~" + _tellerVars.XpControl.GlAcctNoFormat.Value;

					acctTypeField.Append(codeValue, desc);
				}
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private void SetMask(Phoenix.Windows.Forms.PComboBoxStandard acctTypeField, Phoenix.Windows.Forms.PdfStandard acctNoField)
		{
			try
			{
				string applType = null;
				string depLoan = null;
				string format = null;

				GetAcctTypeDetails(acctTypeField, ref applType, ref depLoan, ref format);
				if (depLoan == GlobalVars.Instance.ML.GL)
					acctNoField.MaxLength = 60;
				else
					acctNoField.MaxLength = 12;

				if (format != null && format != string.Empty)
					acctNoField.PhoenixUIControl.InputMask = format;
				else
					acctNoField.PhoenixUIControl.InputMask = String.Empty;
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}

		}

		private void GetAcctTypeDetails(Phoenix.Windows.Forms.PComboBoxStandard acctTypeField,
			ref string applType, ref string depLoan, ref string format)
		{

			try
			{
				string[] applDetails = null;
				// hack
				if (acctTypeField.PhoenixUIControl.BusObjectProperty == null)
					acctTypeField.PhoenixUIControl.BusObjectProperty = this._tellerHelper.AcctType;

				if (acctTypeField.CodeValue == null || acctTypeField.Text == "" || acctTypeField.Text == String.Empty)
				{
					return;
				}

				applDetails = (acctTypeField.Description).Split("~".ToCharArray());
				applType = applDetails[0];
				depLoan = applDetails[1];
				format = applDetails[2];
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}

		}

		private void CallXMThruCDS(string origin)
		{
			try
			{
				if (origin == "AcctTypePopulate")
				{
					Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(this._tellerHelper, _tellerHelper.AcctType);
				}
				else if (origin == "DrawerPopulate")
				{
					this._busObjTlJournal.BranchNo.Value = (cmbBranch.Text == string.Empty ? branchNo.Value : (short)cmbBranch.CodeValue);
					Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(this._busObjTlJournal, _busObjTlJournal.DrawerNo);
				}
				else if (origin == "GridPopulate")
				{
					gridNode = Phoenix.FrameWork.CDS.DataService.Instance.GetListView(this._busObjTlJournal, this.filters);
				}
                //else if (origin == "PopulateGbAcctType")
                //{
                //    _adGbAcctType.ActionType = XmActionType.Select;
                //    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_adGbAcctType);
                //}
				else if (origin == "GridClick")
				{
					this._adRmRestrict.ActionType = XmActionType.Select;
					this._adRmRestrict.RimNo.Value = tempRimNo;
					this._adRmRestrict.RestrictId.Value = -1;
					this._adRmRestrict.OutputTypeId.Value = 1;
					if (TellerVars.Instance.IsAppOnline)
						DataService.Instance.ProcessRequest(_adRmRestrict);
				}
				else if (origin == "Calculator")
				{
					_busObjTlJrnlCalc.ActionType = XmActionType.Select;
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_busObjTlJrnlCalc);
				}
                // Commented - 175838.
                //else if (origin == "ReverserName")
                //{
                //    AdGbRsm adGbRsmTempObj = new AdGbRsm();
                //    adGbRsmTempObj.SelectAllFields = false;
                //    adGbRsmTempObj.EmployeeId.ValueObject = colReversalEmplId.UnFormattedValue;
                //    adGbRsmTempObj.EmployeeName.Selected = true;
                //    adGbRsmTempObj.ActionType = XmActionType.Select;
                //    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(adGbRsmTempObj);
                //    if (!adGbRsmTempObj.EmployeeName.IsNull)
                //        colReverserName.UnFormattedValue = adGbRsmTempObj.EmployeeName.Value;
                //    else
                //        colReverserName.UnFormattedValue = string.Empty;
                //}
                ///* #Begin #72916 */
                //else if (origin == "TlDrawerMachineID")
                //{
                //    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_tlDrawer);

                //}
				/* End #72916 */
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
		}

		private bool SetAcctNoField(PdfStandard acctNoField,
			PComboBoxStandard acctTypeField)
		{
			string applType = null;
			string depLoan = null;
			string format = null;
			string acctNo = dfAccount.Text;

			try
			{
				if (acctNo != null && acctNo != string.Empty && acctNo != "" && cmbAcctType.Text != null)
				{

					GetAcctTypeDetails(acctTypeField, ref applType, ref depLoan, ref format);
					if (format != null && format != String.Empty)
					{
						acctNoField.UnFormattedValue = _tellerHelper.FormatAccount(acctNo, format);
					}
					else
						acctNoField.UnFormattedValue = acctNo;
				}
				return true;
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
				return false;
			}
		}
		#endregion

		#region Report Generation

		#region frmTlJournal_PMdiPrintEvent
		public void frmTlJournal_PMdiPrintEvent(object sender, EventArgs e)
		{
			//71440 - These window has heavy trafic and creating lot of trouble so I have moved the code back here...
			if (!TellerVars.Instance.IsAppOnline)
			{
				try
				{
					_htmlPrinter = new HtmlPrinter();
					//Hack - Not much we can do
					for (int i = 0; i < 1000; i++)
					{
						if (CoreService.LogPublisher.IsLogEnabled)
						{
							if (_htmlPrinter == null)
								CoreService.LogPublisher.LogDebug("Still object is getting created");
							else
								CoreService.LogPublisher.LogDebug("frmTlJournal_PMdiPrintEvent - Browser object created");
						}
						if (_htmlPrinter != null)
							break;
					}
				}
				catch (System.Runtime.InteropServices.COMException ex)
				{
					CoreService.LogPublisher.LogDebug("\n(frmtlPosotion window)For some reason creating of HtmlPrinter Failed." + ex.Message);
				}
			}
			//System.Windows.Forms.MessageBox.Show("Print Can Happen Here!");
			if (TellerVars.Instance.IsAppOnline)
				GenerateReport();
			else
			{
				if (gridJournal.Items.Count > 0)
					GenerateOfflineReport(false);
				else
				{
					// 360813 - There are no records in the table window to print.  Please adjust the search criteria and try again.
					PMessageBox.Show(360813, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				}
			}
		}
		#endregion

		#region GenerateReport
		public void GenerateReport()
		{
			try
			{
				_pdfFileManipulation = new PdfFileManipulation();
			}
			catch (System.Runtime.InteropServices.COMException ex)
			{
				CoreService.LogPublisher.LogDebug("\n(frmTLJournal window/ GenerateReport)For some reason creating of PdfFileManipulation Failed." + ex.ToString());
			}
			//string text1 = string.Empty;
			RunSqrReport report1 = new RunSqrReport();
			report1.ReportName.Value = "TLO10000.sqr";
			report1.EmplId.Value = GlobalVars.EmployeeId;
			report1.FromDt.Value = GlobalVars.SystemDate;
			report1.ToDt.Value = GlobalVars.SystemDate;
			report1.RunDate.Value = DateTime.Now;
			report1.Param1.Value = this.cmbBranch.CodeValue.ToString();
			report1.Param2.Value = this.cmbDrawers.CodeValue.ToString();
			report1.MiscParams.Value = ReportSearchString();
			try
			{
				dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360391));
				//360391 - Generating Teller Journal report. Please wait...
				DataService.Instance.ProcessRequest(XmActionType.Select, report1);
				//System.Diagnostics.Process.Start(report1.OutputLink.Value);
				_pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
			}
			catch (PhoenixException pe)
			{
				dlgInformation.Instance.HideInfo();
				//Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - "  + pe.InnerException);
				Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.ToString());
				PMessageBox.Show(360392, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
				//360392 - Failed to produce Teller Journal report.  Please try again.
			}
			finally
			{
				dlgInformation.Instance.HideInfo();
			}
			//text1 = report1.OutputLink.Value;
		}
		#endregion

		#region GenerateChkPrintingReport - #76057
		//9591 public void GenerateChkPrintingReport()
		public void GenerateChkPrintingReport(PrintInfo printInfo)
		{
			bool Result = false;
			string rptName = null;

			//9591 comment block

			//PInt pBranch = new PInt("branch");
			PInt pTranCode = new PInt("tranCode");
			//PString pJournalPtid = new PString("journalPtid");
			//pBranch.Value = Convert.ToInt16(this.cmbBranch.CodeValue);
			pTranCode.Value = Convert.ToInt16(colRealTranCode.UnFormattedValue);
			//pJournalPtid.Value = Convert.ToString(_tlTranSet.CurTran.Ptid.Value.ToString());
			///*
			// * #79621 - No longer needed; we're using a TlChkPrinter object instead ...
			// * string printerInfo = _tlTranSet.GetPrinterInfo(pBranch, pTranCode);
			// * string[] printerInfoArray = new string[2];
			// * printerInfoArray = printerInfo.Split('^');
			//*/

			if (pTranCode.Value == 902)
				rptName = "TLC60000.SQR";
			else if (pTranCode.Value == 910)
				rptName = "TLC60500.SQR";

			if (OnlineCheckHelper.IsInitialized)
			{
				DateTime Today = GlobalVars.SystemDate;

				OnlineCheckHelper.DeleteAfterPrint = false;

				/* #79621 - Change this call, using the Printer object properties instead of the printerInfoArray ..
				 *
				 * Result = OnlineCheckHelper.PrintCheck(rptName,
				 *    printerInfoArray[1].ToString(),
				 *    printerInfoArray[2].ToString(),
				 *    Today.AddDays(-1),
				 *    Today,
				 *    DateTime.Now,
				 *    GlobalVars.EmployeeId,
				 *    pJournalPtid.Value,
				 *    null,
				 *    null,
				 *    null,
				 *    null,
				 *    null);
				*/

				// #9591 - Added tlTranSet.PrinterPtid and _tlTranSet.CurTran.CheckNo.Value to the report parameters ...
				Result = OnlineCheckHelper.PrintCheck(rptName,
					printInfo.PrinterTray, //_tlTranSet.TellerVars.TlChkPrinter.Tray.Value,
					printInfo.CheckPrinterID, //_tlTranSet.TellerVars.TlChkPrinter.ChkPrintPort.Value,
					Today.AddDays(-1),
					Today,
					DateTime.Now,
					GlobalVars.EmployeeId,
					printInfo.JournalPtid.ToString(), //pJournalPtid.Value,
					printInfo.PrinterPtid.ToString(), //this._tlTranSet.PrinterPtid.Value.ToString(),
					printInfo.CheckNo.ToString(), //_tlTranSet.CurTran.CheckNo.Value.ToString(),
					null,
					null,
					null);
			}
		}

		#endregion


		#region ReportSearchString

		private string ReportSearchString()
		{
			int fromTimeHour = 0;
			int fromTimeMin = 0;
			int toTimeHour = 0;
			int toTimeMin = 0;
			StringBuilder builder1 = new StringBuilder();
			builder1.Append(string.Concat(new object[] { this.cmbTranStatus.Name, "=", this.cmbTranStatus.CodeValue, "~" }));
			builder1.Append(string.Concat(new object[] { this.cmbProofStatus.Name, "=", this.cmbProofStatus.CodeValue, "~" }));
			builder1.Append(string.Concat(new object[] { this.cmbCTRStatus.Name, "=", this.cmbCTRStatus.CodeValue, "~" }));
			bool addtoWhere = false;
			if (this.cbAccount.Checked && _busObjTlJournal.ValidateSrchByAccount())
			{
				builder1.Append(this.cbAccount.Name + "=Y" + "~");
				addtoWhere = true;
			}
			else
				builder1.Append(this.cbAccount.Name + "=N" + "~");
			if (this.cbAccount.Checked && addtoWhere)
			{
				if ((this.cmbAcctType.Text != null) && (this.cmbAcctType.Text.Length > 0))
				{
					builder1.Append(this.cmbAcctType.Name + "=" + this.cmbAcctType.CodeValue.ToString().Trim() + "~");
				}
				else
				{
					builder1.Append(this.cmbAcctType.Name + "=~");
				}
				if (this.dfAccount.Text.Trim().Length > 0)
				{
					builder1.Append(this.dfAccount.Name + "=" + this.dfAccount.Text.Trim() + "~");
				}
				else
				{
					builder1.Append(this.dfAccount.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.cmbAcctType.Name + "=~");
				builder1.Append(this.dfAccount.Name + "=~");
			}
			addtoWhere = false;
			if (this.cbEffectiveDate.Checked)
				if (dfEffectiveFrom.Text != null && dfEffectiveFrom.Text.Trim() != string.Empty && dfEffectiveTo.Text != null && dfEffectiveTo.Text.Trim() != string.Empty)
				{
					builder1.Append(this.cbEffectiveDate.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbEffectiveDate.Name + "=N" + "~");
			else
				builder1.Append(this.cbEffectiveDate.Name + "=" + this.cbEffectiveDate.CodeValue + "~");
			//
			if (this.cbEffectiveDate.Checked && addtoWhere)
			{
				if ((((DateTime)this.dfEffectiveFrom.UnFormattedValue) != DateTime.MinValue) && (((DateTime)this.dfEffectiveTo.UnFormattedValue) != DateTime.MinValue))
				{
					builder1.Append(this.dfEffectiveFrom.Name + "=" + this.dfEffectiveFrom.Text.Trim() + "~");
					builder1.Append(this.dfEffectiveTo.Name + "=" + this.dfEffectiveTo.Text.Trim() + "~");
				}
				else
				{
					builder1.Append(this.dfEffectiveFrom.Name + "=~");
					builder1.Append(this.dfEffectiveTo.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.dfEffectiveFrom.Name + "=" + postingDate.Value.ToShortDateString() + "~");
				builder1.Append(this.dfEffectiveTo.Name + "=" + postingDate.Value.ToShortDateString() + "~");
			}
			//
			addtoWhere = false;
			if (this.cbTranEffectiveDt.Checked)
				if (dfTranEffectiveDtFrom.Text != null && dfTranEffectiveDtFrom.Text.Trim() != string.Empty && dfTranEffectiveDtTo.Text != null && dfTranEffectiveDtTo.Text.Trim() != string.Empty)
				{
					builder1.Append(this.cbTranEffectiveDt.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbTranEffectiveDt.Name + "=N" + "~");
			else
				builder1.Append(this.cbTranEffectiveDt.Name + "=" + this.cbTranEffectiveDt.CodeValue + "~");
			if (this.cbTranEffectiveDt.Checked && addtoWhere)
			{
				if ((((DateTime)this.dfTranEffectiveDtFrom.UnFormattedValue) != DateTime.MinValue) && (((DateTime)this.dfTranEffectiveDtTo.UnFormattedValue) != DateTime.MinValue))
				{
					builder1.Append(this.dfTranEffectiveDtFrom.Name + "=" + this.dfTranEffectiveDtFrom.Text + "~");
					builder1.Append(this.dfTranEffectiveDtTo.Name + "=" + this.dfTranEffectiveDtTo.Text + "~");
				}
				else
				{
					builder1.Append(this.dfTranEffectiveDtFrom.Name + "=~");
					builder1.Append(this.dfTranEffectiveDtTo.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.dfTranEffectiveDtFrom.Name + "=~");
				builder1.Append(this.dfTranEffectiveDtTo.Name + "=~");
			}
			addtoWhere = false;
			if (this.cbDate.Checked)
			{
				//
				//if (dfFrom.Text != null && dfFrom.Text.Trim() != string.Empty && dfTo.Text != null && dfTo.Text.Trim() != string.Empty)
				//{
				ValidateDateFields(dfFrom, dfTo, dfFromTime, dfToTime,
					ref searchFromDate, ref searchToDate, ref searchFromTime, ref searchToTime);
				if (_busObjTlJournal.ValidateSrchByDate(searchFromDate, searchToDate, searchFromTime, searchToTime))
				{
					builder1.Append(this.cbDate.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbDate.Name + "=N" + "~");
			}
			else
				builder1.Append(this.cbDate.Name + "=" + this.cbDate.CodeValue + "~");
			if (this.cbDate.Checked && addtoWhere)
			{
				string text1 = string.Empty;
				if ((dfFrom.Text != null && dfFrom.Text.Trim() != string.Empty && dfTo.Text != null && dfTo.Text.Trim() != string.Empty) &&
					((DateTime)this.dfFrom.UnFormattedValue != DateTime.MinValue && (DateTime)this.dfTo.UnFormattedValue != DateTime.MinValue)
				   )
				{
					if ((this.dfFromTime.Text != string.Empty && this.dfFromTime.Text != "" && this.dfFromTime.Text != null) ||
						(this.dfToTime.Text != string.Empty && this.dfToTime.Text != "" && this.dfToTime.Text != null))
					{
						//#71436 - fixed from and to time search
						if (this.dfFromTime.Text != string.Empty && this.dfFromTime.Text != "" && this.dfFromTime.Text != null)
						{
							fromTimeHour = Convert.ToDateTime(dfFromTime.UnFormattedValue).Hour;
							fromTimeMin = Convert.ToDateTime(dfFromTime.UnFormattedValue).Minute;
						}
						if (this.dfToTime.Text != string.Empty && this.dfToTime.Text != "" && this.dfToTime.Text != null)
						{
							toTimeHour = Convert.ToDateTime(dfToTime.UnFormattedValue).Hour;
							toTimeMin = Convert.ToDateTime(dfToTime.UnFormattedValue).Minute;
						}


						text1 = (new DateTime(Convert.ToDateTime(dfFrom.UnFormattedValue).Year,
							Convert.ToDateTime(dfFrom.UnFormattedValue).Month,
							Convert.ToDateTime(dfFrom.UnFormattedValue).Day,
							fromTimeHour,
							fromTimeMin,
							0,
							0)).ToString();
						builder1.Append(this.dfFrom.Name + "=" + text1 + "~");

						text1 = (new DateTime(Convert.ToDateTime(dfTo.UnFormattedValue).Year,
							Convert.ToDateTime(dfTo.UnFormattedValue).Month,
							Convert.ToDateTime(dfTo.UnFormattedValue).Day,
							toTimeHour,
							toTimeMin,
							0,
							0)).ToString();
						builder1.Append(this.dfTo.Name + "=" + text1 + "~");
					}
					else
					{
						//#71362 - fake the time stamp in case of null
						text1 = (new DateTime(Convert.ToDateTime(dfFrom.UnFormattedValue).Year,
							Convert.ToDateTime(dfFrom.UnFormattedValue).Month,
							Convert.ToDateTime(dfFrom.UnFormattedValue).Day,
							0,
							0,
							0,
							0)).ToString();
						builder1.Append(this.dfFrom.Name + "=" + text1 + "~");
						text1 = (new DateTime(Convert.ToDateTime(dfTo.UnFormattedValue).Year,
							Convert.ToDateTime(dfTo.UnFormattedValue).Month,
							Convert.ToDateTime(dfTo.UnFormattedValue).Day,
							23,
							59,
							0,
							0)).ToString();
						builder1.Append(this.dfFrom.Name + "=" + text1 + "~");
					}
				}
				else
				{
					builder1.Append(this.dfFrom.Name + "=~");
					builder1.Append(this.dfTo.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.dfFrom.Name + "=~");
				builder1.Append(this.dfTo.Name + "=~");
			}
			addtoWhere = false;
			if (this.cbAmt.Checked)
			{
				if (cmbAmtType.CodeValue != null && _busObjTlJournal.ValidateSrchByAmount())
				{
					builder1.Append(this.cbAmt.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbAmt.Name + "=N" + "~");
			}
			else
				builder1.Append(this.cbAmt.Name + "=" + this.cbAmt.CodeValue + "~");
			if (this.cbAmt.Checked && addtoWhere)
			{
				builder1.Append(this.cmbAmtType.Name + "=" + this.cmbAmtType.CodeValue.ToString().Trim() + "~");
				if (((this.dfFromAmt.Text != null) && (this.dfFromAmt.Text.Trim().Length > 0)) && ((this.dfToAmt.Text != null) && (this.dfToAmt.Text.Trim().Length > 0)))
				{
					builder1.Append(this.dfFromAmt.Name + "=" + this.dfFromAmt.UnFormattedValue.ToString() + "~");
					builder1.Append(this.dfToAmt.Name + "=" + this.dfToAmt.UnFormattedValue.ToString() + "~");
				}
				else
				{
					builder1.Append(this.dfFromAmt.Name + "=~");
					builder1.Append(this.dfToAmt.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.cmbAmtType.Name + "=~");
				builder1.Append(this.dfFromAmt.Name + "=~");
				builder1.Append(this.dfToAmt.Name + "=~");
			}
			addtoWhere = false;

			if (cbMisc.Checked)
			{
				if (cmbMiscSrch != null && _busObjTlJournal.ValidateSrchByMisc())
				{
					builder1.Append(this.cbMisc.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbMisc.Name + "=N" + "~");
			}
			else
				builder1.Append(this.cbMisc.Name + "=" + this.cbMisc.CodeValue + "~");

			if (this.cbMisc.Checked && addtoWhere)
			{
				if (cmbMiscSrch.CodeValue != null)
				{
					builder1.Append(this.cmbMiscSrch.Name + "=" + this.cmbMiscSrch.CodeValue.ToString().Trim() + "~");
					//
					if ((cmbMiscSrch.CodeValue.ToString() == "Sequence Number" || cmbMiscSrch.CodeValue.ToString() == "Offline Sequence Number") &&
						(Convert.ToInt16(this.dfSeqNoFrom.UnFormattedValue) >= 0 && Convert.ToInt16(this.dfSeqNoTo.UnFormattedValue) >= 0)
						)
					{
						builder1.Append(this.dfSeqNoFrom.Name + "=" + this.dfSeqNoFrom.Text.Trim() + "~");
						builder1.Append(this.dfSeqNoTo.Name + "=" + this.dfSeqNoTo.Text.Trim() + "~");
					}
					else
					{
						builder1.Append(this.dfSeqNoFrom.Name + "=~");
						builder1.Append(this.dfSeqNoTo.Name + "=~");
					}
					//
					if ((this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(0x3d7, "Check Number")) && (Convert.ToInt32(this.dfCheckNo.UnFormattedValue) >= 0))
						builder1.Append(this.dfCheckNo.Name + "=" + this.dfCheckNo.Text + "~");
					else
						builder1.Append(this.dfCheckNo.Name + "=~");
					//
					if ((this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(0x3d7, "Transaction")) && (this.cmbTranCode.CodeValue.ToString() != ""))
						builder1.Append(this.cmbTranCode.Name + "=" + this.cmbTranCode.CodeValue.ToString() + "~");
					else
						builder1.Append(this.cmbTranCode.Name + "=~");
					//
					if ((this.cmbMiscSrch.CodeValue.ToString() == CoreService.Translation.GetListItemX(0x3d7, "Batch Items")) && ((Convert.ToInt16(cmbBatchItem.CodeValue)) > 0))
						builder1.Append(this.cmbBatchItem.Name + "=" + this.cmbBatchItem.CodeValue.ToString() + "~");
					else
						builder1.Append(this.cmbBatchItem.Name + "=~");
				}
				else
				{
					builder1.Append(this.cmbMiscSrch.Name + "=~");
					builder1.Append(this.dfSeqNoFrom.Name + "=~");
					builder1.Append(this.dfSeqNoTo.Name + "=~");
					builder1.Append(this.dfCheckNo.Name + "=~");
					builder1.Append(this.cmbTranCode.Name + "=~");
					builder1.Append(this.cmbBatchItem.Name + "=~");
				}
			}
			else
			{

				builder1.Append(this.cmbMiscSrch.Name + "=~");
				builder1.Append(this.dfSeqNoFrom.Name + "=~");
				builder1.Append(this.dfSeqNoTo.Name + "=~");
				builder1.Append(this.dfCheckNo.Name + "=~");
				builder1.Append(this.cmbTranCode.Name + "=~");
				builder1.Append(this.cmbBatchItem.Name + "=~");
			}
			builder1.Append(this.cbReversal.Name + "=" + this.cbReversal.CodeValue + "~");
			builder1.Append(this.cbSupervisor.Name + "=" + this.cbSupervisor.CodeValue + "~");
			//builder1.Append(this.cbCustomerName.Name + "=" + this.cbCustomerName.CodeValue);
			builder1.Append(this.cbCustomerName.Name + "=" + this.cbCustomerName.CodeValue + "~");

			addtoWhere = false;
			#region #79569
			if (this.cbLocalDateTime.Checked)
			{
				ValidateDateFields(dfFromLocalDate, dfToLocalDate, dfFromLocalTime, dfToLocalTime,
					ref searchFromDate, ref searchToDate, ref searchFromTime, ref searchToTime);
				if (_busObjTlJournal.ValidateSrchByDate(searchFromDate, searchToDate, searchFromTime, searchToTime))
				{
					builder1.Append(this.cbLocalDateTime.Name + "=Y" + "~");
					addtoWhere = true;
				}
				else
					builder1.Append(this.cbLocalDateTime.Name + "=N" + "~");
			}
			else
				builder1.Append(this.cbLocalDateTime.Name + "=" + this.cbLocalDateTime.CodeValue + "~");
			if (this.cbLocalDateTime.Checked && addtoWhere)
			{
				string text1 = string.Empty;
				if ((dfFromLocalDate.Text != null && dfFromLocalDate.Text.Trim() != string.Empty && dfToLocalDate.Text != null && dfToLocalDate.Text.Trim() != string.Empty) &&
					((DateTime)this.dfFromLocalDate.UnFormattedValue != DateTime.MinValue && (DateTime)this.dfToLocalDate.UnFormattedValue != DateTime.MinValue)
				   )
				{
					if ((this.dfFromLocalTime.Text != string.Empty && this.dfFromLocalTime.Text != "" && this.dfFromLocalTime.Text != null) ||
						(this.dfToLocalTime.Text != string.Empty && this.dfToLocalTime.Text != "" && this.dfToLocalTime.Text != null))
					{
						if (this.dfFromLocalTime.Text != string.Empty && this.dfFromLocalTime.Text != "" && this.dfFromLocalTime.Text != null)
						{
							fromTimeHour = Convert.ToDateTime(dfFromLocalTime.UnFormattedValue).Hour;
							fromTimeMin = Convert.ToDateTime(dfFromLocalTime.UnFormattedValue).Minute;
						}
						if (this.dfToLocalTime.Text != string.Empty && this.dfToLocalTime.Text != "" && this.dfToLocalTime.Text != null)
						{
							toTimeHour = Convert.ToDateTime(dfToLocalTime.UnFormattedValue).Hour;
							toTimeMin = Convert.ToDateTime(dfToLocalTime.UnFormattedValue).Minute;
						}


						text1 = (new DateTime(Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Year,
							Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Month,
							Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Day,
							fromTimeHour,
							fromTimeMin,
							0,
							0)).ToString();
						builder1.Append(this.dfFromLocalDate.Name + "=" + text1 + "~");

						text1 = (new DateTime(Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Year,
							Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Month,
							Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Day,
							toTimeHour,
							toTimeMin,
							0,
							0)).ToString();
						builder1.Append(this.dfToLocalDate.Name + "=" + text1 + "~");
					}
					else
					{
						text1 = (new DateTime(Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Year,
							Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Month,
							Convert.ToDateTime(dfFromLocalDate.UnFormattedValue).Day,
							0,
							0,
							0,
							0)).ToString();
						builder1.Append(this.dfFromLocalDate.Name + "=" + text1 + "~");
						text1 = (new DateTime(Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Year,
							Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Month,
							Convert.ToDateTime(dfToLocalDate.UnFormattedValue).Day,
							23,
							59,
							0,
							0)).ToString();
						builder1.Append(this.dfFromLocalDate.Name + "=" + text1 + "~");
					}
				}
				else
				{
					builder1.Append(this.dfFromLocalDate.Name + "=~");
					builder1.Append(this.dfToLocalDate.Name + "=~");
				}
			}
			else
			{
				builder1.Append(this.dfFromLocalDate.Name + "=~");
				builder1.Append(this.dfToLocalDate.Name + "=~");
			}
			#endregion #79569

			CoreService.LogPublisher.LogDebug("SQR Search String:" + builder1.ToString());
			addtoWhere = false;
			return builder1.ToString();
		}
		#endregion

		#region GenerateOfflineReport
		public void GenerateOfflineReport(bool silentPrinting)
		{
			#region Variables
			StringBuilder finalHTMLText = new StringBuilder();
			string templateFileName = "TLF10000.html";
			string machineTmpDir = @"C:\TEMP\Phoenix\offlineReports\";
			string htmlString;
			string part1Html = string.Empty;
			string part2Html = string.Empty;
			string fixedHeaderTable = string.Empty;
			string repeatingHeaderTable = string.Empty;
			string repeatDetailsTable = string.Empty;
			string reportEndTable = string.Empty;
			string tempHeader;
			string tempDetails;
			string workingStr;
			string tranDesc = string.Empty;
			string space5Chars = @"&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
			//To Format the unformatted string
			Phoenix.Windows.Forms.PDfDisplay myTempVar = new PDfDisplay();
			_printDriver.ChangePageOrientation(_printDriver.DefaultPrinter, PageOrientation.DMORIENT_LANDSCAPE);
			myTempVar.Visible = false;
			myTempVar.TabStop = false;
			myTempVar.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			myTempVar.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			//
			int selectedRow = Convert.ToInt32(gridJournal.SelectedIndicies[0]);
			#endregion Variables


			try
			{
				//360391 - Generating Teller Journal report. please wait...
				dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360391));
				if (!System.IO.Directory.Exists(machineTmpDir))
					System.IO.Directory.CreateDirectory(machineTmpDir);
				templateFileName = Environment.CurrentDirectory + @"\forms\" + templateFileName;
				if (!System.IO.File.Exists(templateFileName))
				{
					//File Not Found Exception
					if (!silentPrinting)
					{
						dlgInformation.Instance.HideInfo();
						PMessageBox.Show(360717, MessageType.Warning, MessageBoxButtons.OK, templateFileName);
						//360717 - Offline Teller Journal template %1! is not found.  Please contact support to rectify the problem.
					}
					return;
				}

				using (StreamReader sr = new StreamReader(templateFileName))
				{
					htmlString = sr.ReadToEnd();
				}
				ExtractTablesFromHTML100(htmlString, out part1Html, out part2Html, out fixedHeaderTable, out repeatingHeaderTable,
					out repeatDetailsTable, out reportEndTable);

				fixedHeaderTable = fixedHeaderTable.Replace("phx_BankName", _adGbBank.Name1.Value.Trim());
				fixedHeaderTable = fixedHeaderTable.Replace("phx_RunDateTime", DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
				tempHeader = fixedHeaderTable.Replace("phx_PageNo", "1");

				finalHTMLText.Append(part1Html + tempHeader + repeatingHeaderTable);
				//Now Loop the grid...
				short j = 0;
				short k = 2; //We already set 1 so start with Two...
				for (int i = 0; i < gridJournal.Count; i++)
				{
					gridJournal.SelectRow(i, false);
					//gridJournal.ContextRow = i; //set the context
					if (j == 7)
					{
						finalHTMLText.Append("<p STYLE=\"page-break-before: always\"></p>" + "\n"); //Break Page
						tempHeader = fixedHeaderTable.Replace("phx_PageNo", k.ToString());
						finalHTMLText.Append(tempHeader + repeatingHeaderTable);
						tempHeader = string.Empty;
						j = 0;
						k++;
					}
					j++;
					//Build the Details Here...
					#region Detail Header
					//
					#region Line 1
					if (colSubSequence.Text != null && colSubSequence.Text.Trim().Length > 0)
						if (Convert.ToInt32(colSubSequence.UnFormattedValue) > 0)
							workingStr = colSequenceNo.Text.Trim() + @"/" + colSubSequence.Text.Trim();
						else
							workingStr = colSequenceNo.Text.Trim();
					else
						workingStr = colSequenceNo.Text.Trim();
					tempDetails = repeatDetailsTable;
					tempDetails = tempDetails.Replace("phx_d_SeqSubSeqNo", workingStr);
					//
					tempDetails = tempDetails.Replace("phx_d_AcctNoAcctType", colAccount.Text);
					//
					tempDetails = tempDetails.Replace("phx_d_NetAmt", colNetAmt.Text);
					//
					if (colCashInAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colCashInAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_CashIn", workingStr);
					//
					if (colCCAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colCCAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_CcAmt", workingStr);
					//
					if (colReference.UnFormattedValue != null)
						workingStr = colReference.Text.Trim();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_Reference", workingStr);
					#endregion Line 1
					//
					#region Line 2
					//
					#region #79569 - Line 2, col 1 - Local Date/Time/Zone
					//
					if (colLocalDtTime.UnFormattedValue != null)
						workingStr = Convert.ToDateTime(colLocalDtTime.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt") + " " + colLocalTZ.Text.Trim();
					else
						workingStr = "";
					tempDetails = tempDetails.Replace("phx_d_LocalCreateDt", workingStr);
					//
					#endregion #79569
					if (colChksAsCashAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colChksAsCashAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_ChksAsCash", workingStr);
					//
					if (colTCDCashIn.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colTCDCashIn.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_TCDCashIn", workingStr);
					//
					if (colIntAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colIntAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_IntAmt", workingStr);
					//
					if (colReferenceAcct.UnFormattedValue != null)
						workingStr = colReferenceAcct.UnFormattedValue.ToString();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_RefAcctNoType", workingStr);
					#endregion Line 2
					//
					#region Line 3
					//
					tempDetails = tempDetails.Replace("phx_d_CreateDt", Convert.ToDateTime(colCreateDt.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt"));
					//
					// begin #72916 DHE - Don't Pull Tran_Code field when = 'LOF' or 'LON' because it's in description
					if ((colTranCodeDisp.Text.Trim() == "LOF") || (colTranCodeDisp.Text.Trim() == "LON"))
					{
						tranDesc = colDescription.Text.Trim();
					}
					else
					{
						tranDesc = (colTranCodeDisp.Text.Trim() + " - " + colDescription.Text.Trim());
					}
					// End #72916 DHE
					if (tranDesc.Length > 49)
						tranDesc = tranDesc.Substring(0, 48);
					tempDetails = tempDetails.Replace("phx_d_Transaction", tranDesc);
					//
					if (colTransitChksAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colTransitChksAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_TransitChks", workingStr);
					//
					if (colCashOutAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colCashOutAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_CashOut", workingStr);
					//
					if (colSuperEmplId.UnFormattedValue != null)
						workingStr = "Yes";
					else
						workingStr = "No";
					tempDetails = tempDetails.Replace("phx_d_SupName", workingStr);
					//
					#endregion Line 3
					//
					#region Line 4
					//

					tempDetails = tempDetails.Replace("phx_d_EffectiveDT", Convert.ToDateTime(colEffectiveDt.UnFormattedValue).ToString("MM/dd/yyyy"));
					//
					if (colTfrAccount.UnFormattedValue != null)
						workingStr = colTfrAccount.Text.Trim();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_TfrAcctNoType", workingStr);
					//
					if (colOnUsChksAmt.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colOnUsChksAmt.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_OnUsChks", workingStr);
					//
					if (colTCDCashOut.UnFormattedValue != null)
					{
						myTempVar.UnFormattedValue = colTCDCashOut.UnFormattedValue;
						workingStr = myTempVar.Text;
					}
					else
						workingStr = "$0.0";
					tempDetails = tempDetails.Replace("phx_d_TCDCashOut", workingStr);
					//
					if (colCheckNo.UnFormattedValue != null)
						workingStr = colCheckNo.UnFormattedValue.ToString();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_CheckNo", workingStr);
					//
					#endregion Line 4
					//
					#region Line 5
					//
					if (colTranEffectiveDt.UnFormattedValue != null)
					{
						if (DateTime.Compare(Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue), Convert.ToDateTime(colEffectiveDt.UnFormattedValue)) == 0)
							workingStr = space5Chars;
						else
							workingStr = Convert.ToDateTime(colTranEffectiveDt.UnFormattedValue).ToString("MM/dd/yyyy") + "*";
					}
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_TranEffectiveDt", workingStr);
					//
					if (colJournalDescription.UnFormattedValue != null)
						workingStr = colJournalDescription.Text.Trim();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_JourNalDesc", workingStr);
					//
					// #72916 DHE - Set Mach Id/Dr#/Postn column
					if (colTcdDeviceNo.Text != null)
					{
						workingStr = colTcdDeviceNo.Text.Trim() + "/";
					}
					else
					{
						workingStr = "/";
					}
					if (colTCDDrawerNo.Text != null)
					{
						workingStr = workingStr + colTCDDrawerNo.Text.Trim() + "/";
					}
					else
					{
						workingStr = workingStr + "/";
					}
					if (colTCDDrawerPosition.Text != null)
					{
						workingStr = workingStr + colTCDDrawerPosition.Text.Trim();
					}
					if (workingStr == "//")
					{
						workingStr = "";
					}
					tempDetails = tempDetails.Replace("phx_d_TCDDrPositin", workingStr);
					// End #72916 DHE
					//
					if (colUtility.UnFormattedValue != null)
						workingStr = colUtility.Text.Trim();
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_UtilityDesc", workingStr);
					//
					#endregion Line 5
					//
					#region Line 6
					//
					if (colReversalDisplay.UnFormattedValue != null && colReversalDisplay.Text.Trim() == "Y")
						workingStr = "Reversed";
					else
						workingStr = space5Chars;
					tempDetails = tempDetails.Replace("phx_d_Reversal", workingStr);
					//
					if (colReversalCreateDt.UnFormattedValue != null)
					{
                        // CallXMThruCDS("ReverserName"); - Commented as part of 175838.
						workingStr = Convert.ToDateTime(colReversalCreateDt.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt") + " - " + colReverserName.Text.Trim();
						tempDetails = tempDetails.Replace("phx_d_RevDateRevBy", workingStr);
					}
					else
						tempDetails = tempDetails.Replace("phx_d_RevDateRevBy", space5Chars);
					//
					if (colPayroll.UnFormattedValue != null && colPayroll.Text.Trim() == "Y")
						workingStr = "Yes";
					else
						workingStr = "No";
					if (colCtrStatus.UnFormattedValue != null && colCtrStatus.Text.Trim() == "Y")
						workingStr = workingStr + @"/Yes";
					else
						workingStr = workingStr + @"/No";
					tempDetails = tempDetails.Replace("phx_d_PayRollWdCtrTrig", workingStr);
					//
					#endregion Line 6

					//
					finalHTMLText.Append(tempDetails);
					#endregion Detail Header
				}
				#region Add End of Report and Close HTML
				finalHTMLText.Append(reportEndTable);
				finalHTMLText.Append(part2Html);
				if (CoreService.LogPublisher.IsLogEnabled)
					CoreService.LogPublisher.LogInfo(finalHTMLText.ToString());
				#endregion

				//Print the Built String...
				_htmlPrinter.PrintHtml(finalHTMLText.ToString(), false);
			}
			catch (Exception ex)
			{
				CoreService.LogPublisher.LogDebug(ex.Message);
				if (!silentPrinting)
				{
					//Message
					dlgInformation.Instance.HideInfo();
					Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(ex.Message + " - " + ex.InnerException);
					PMessageBox.Show(360392, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
					//360392 - Failed to produce Teller Journal report.  Please try again.
				}
			}
			finally
			{
				gridJournal.ContextRow = selectedRow;
				dlgInformation.Instance.HideInfo();
				//73125
				//if (_printDriver != null)
				//	_printDriver.ChangePageOrientation(_printDriver.DefaultPrinter, PageOrientation.DMORIENT_PORTRAIT);
			}
		}
		#endregion GenerateOfflineReport

		#region ExtractTablesFromHTML100
		private void ExtractTablesFromHTML100(string htmlString, out string part1Html, out string part2Html,
								out string fixedHeaderTable, out string repeatingHeaderTable,
								out string repeatDetailsTable, out string reportEndTable)
		{
			#region Varibales
			int startLoc = 0;
			int endLoc = 0;
			part1Html = string.Empty;
			part2Html = string.Empty;
			fixedHeaderTable = string.Empty;
			repeatingHeaderTable = string.Empty;
			repeatDetailsTable = string.Empty;
			reportEndTable = string.Empty;
			string endTable = @"table>";
			#endregion Varibales
			//The whole thing can be replaced with regular expression, I have the code...
			//
			//Regex regex = new Regex(@"<\s*(table)\s*([^/>]*)(?:>(.*?)</\1>|/>)",
			//	RegexOptions.IgnoreCase |
			//	RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
			//MatchCollection  collection = regex.Matches(htmlString);
			//
			// You get table tags, Content between tags and properties of the tags
			//
			startLoc = htmlString.ToLower().IndexOf("<table id=\"fixedheader\">", 0);
			part1Html = htmlString.Substring(0, startLoc);
			endLoc = htmlString.ToLower().IndexOf(endTable, startLoc) + 7;
			fixedHeaderTable = htmlString.Substring(startLoc, endLoc - startLoc); ///table>
			//
			startLoc = htmlString.ToLower().IndexOf("<table id=\"repeatingheader\">", 0);
			endLoc = htmlString.ToLower().IndexOf(endTable, startLoc) + 7;
			repeatingHeaderTable = htmlString.Substring(startLoc, endLoc - startLoc); ///table>
			//
			startLoc = htmlString.ToLower().IndexOf("<table id=\"repeatdetails\">", 0);
			endLoc = htmlString.ToLower().IndexOf(endTable, startLoc) + 7;
			repeatDetailsTable = htmlString.Substring(startLoc, endLoc - startLoc); ///table>
			//
			startLoc = htmlString.ToLower().IndexOf("<table id=\"reportend\">", 0);
			endLoc = htmlString.ToLower().IndexOf(endTable, startLoc) + 7;
			reportEndTable = htmlString.Substring(startLoc, endLoc - startLoc); ///table>
			part2Html = htmlString.Substring(endLoc, htmlString.Length - endLoc); ///table>
			//
		}
		#endregion ExtractTablesFromHTML100

		private void frmTlJournal_Load(object sender, EventArgs e)
		{

		}

		#endregion Report Generation

		private void cmbBatchItem_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
		{

		}


		#region ReverseVoucher
		private void ReverseVoucher()
		{
			// #140784-2 - Added parameter for IsSharedBranch
			try
			{
				DocumentService doc;
				ParameterService param;
				int rimNo = 0;
				int instNo = 0;
				string voidKey = "VOID";
				short branchNo = Convert.ToInt16(colBranchNo.UnFormattedValue);
				short drawerNo = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
				DateTime effectiveDt = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
				short sequenceNo = Convert.ToInt16(colSequenceNo.UnFormattedValue);
				//
				if (colSubSequence.UnFormattedValue == null || Convert.ToInt32(colSubSequence.UnFormattedValue) == 0)
				{
					#region set param
					rimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
					param = new ParameterService(null, rimNo, Convert.ToString(colPTID.UnFormattedValue), voidKey, _tellerVars.HylandInstitutionNo, colSharedBranch.Text == "Y" ? true : false);
					#endregion
					//
					#region process
					doc = new DocumentService();
                    doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable; //119501
                    doc.ReverseVourcher(param);
					#endregion
				}
				else if (Convert.ToInt32(colSubSequence.UnFormattedValue) > 0)
				{
                    //Begin 121336

                    /*
                    for (int i = 0; i < gridJournal.Items.Count; i++)
					{
						gridJournal.SelectRow(i, false);
						//
						if (Convert.ToInt16(colBranchNo.UnFormattedValue) == branchNo &&
							Convert.ToInt16(colTellerDrawerNo.UnFormattedValue) == drawerNo &&
							Convert.ToDateTime(colEffectiveDt.UnFormattedValue) == effectiveDt &&
							Convert.ToInt16(colSequenceNo.UnFormattedValue) == sequenceNo)
						{
							#region set param
							rimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
							param = new ParameterService(null, rimNo, Convert.ToString(colPTID.UnFormattedValue), voidKey, _tellerVars.HylandInstitutionNo, colSharedBranch.Text == "Y" ? true : false);
                            #endregion
                            //
                            #region process
                            doc = new DocumentService();
                            doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable; //119501
                            doc.ReverseVourcher(param);
							#endregion
						}
					}
                    */

                    string subSeqPTIDs = string.Empty;
                   
                    gridJournal.RowToObject(_busObjTlJournal);

                    subSeqPTIDs = _busObjTlJournal.GetTransReversePTIDS();

                    if (subSeqPTIDs.IndexOf("^") >= 0)
                    {
                        if (_tlTranSet.TellerVars.IsECMVoucherAvailable)
                        {
                            #region set param
                            rimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
                            param = new ParameterService(null, rimNo, subSeqPTIDs, voidKey, _tellerVars.HylandInstitutionNo, colSharedBranch.Text == "Y" ? true : false);
                            #endregion
                            //
                            #region process
                            doc = new DocumentService();
                            doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable;
                            doc.ReverseVourcher(param);
                            #endregion
                        }
                        else
                        {
                            char[] splitchar = {'^'};
                            string[] TranRevPTIDS = subSeqPTIDs.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < TranRevPTIDS.Length; i++)
                            {
                                if (!String.IsNullOrEmpty(TranRevPTIDS[i]))
                                {
                                    #region set param
                                    rimNo = Convert.ToInt32(colRimNo.UnFormattedValue);
                                    param = new ParameterService(null, rimNo, TranRevPTIDS[i], voidKey, _tellerVars.HylandInstitutionNo, colSharedBranch.Text == "Y" ? true : false);
                                    #endregion
                                    //
                                    #region process
                                    doc = new DocumentService();
                                    doc.IsECMEnabled = _tlTranSet.TellerVars.IsECMVoucherAvailable;
                                    doc.ReverseVourcher(param);
                                    #endregion
                                }
                            }
                        }
                    }

                    //End 121336
                }
            }
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe, MessageBoxButtons.OK);
			}
		}
		#endregion

		private void cmbTranCode_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
		{

		}

		//Begin #79510
		private void GetAcquirerTranSet()
		{
			this.Enabled = false; // disable the whole form

			try
			{
				if (!_tlTranSet.FormAcquirerRevTranSet(acquirerResponseXml))
				{
					if (_tlTranSet.TranPostedStatus.Value == (short)TranPostStatus.Failure)
					{
						PMessageBox.Show(360188, MessageType.Warning, MessageBoxButtons.OK,
							//#79510, #10883 - Added 13330
							new string[] { CoreService.Translation.GetUserMessageX( 13330 ) + @"

" + _tlTranSet.ErrorString.Value });
					}
					else
					{
						ProcessResponses();
					}
					this.OnActionClose();   // to set the _isClosing of PfwStandard
					this.Close();
					if (this.Workspace != null)
						Workspace.CloseAll();
					return;
				}
				this.branchNo.Value = _tlTranSet.BranchNo.Value;
				this.drawerNo.Value = _tlTranSet.DrawerNo.Value;
				this.postingDate.Value = _tlTranSet.EffectiveDt.Value;
				this.cmbBranch.SetValue(this.branchNo.Value);
				this.cmbDrawers.SetValue(this.drawerNo.Value);
			}
			catch (PhoenixException pe)
			{
				PMessageBox.Show(pe);
			}
			finally
			{

			}

		}

		private void cmbBranch_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
		{

		}

		//Begin #79510, #09368
		private void pbEndorsement_Click(object sender, PActionEventArgs e)
		{
			CallOtherForms("Endorsement");
		}
		//End #79510, #09368

		//End #79510

		//#9470 - Begin

		protected override void OnClosing(CancelEventArgs e)
		{
			if (!IsPendingTcrCashTranCompleted())    //#9470
				e.Cancel = true;

			base.OnClosing(e);
		}

		private bool IsPendingTcrCashTranCompleted()
		{

			if (_clearPendingTcrTran)   //#9470
			{
				TcrTotalCashInAmt.Value = 0;
				TcrTotalCashOutAmt.Value = 0;
				//
				ReverseDeviceDispense((_clearPendingTcrTranType == "I" ? _clearPendingTcrTranAmt : 0), (_clearPendingTcrTranType == "D" ? _clearPendingTcrTranAmt : 0));
				if (_clearPendingTcrTran)
					return false;
			}
			return true;
		}

		private bool IsTcrConnected()
		{
			if (!_tellerVars.IsTCDEnabled)
				return false;
			if (_tellerVars.IsTCDEnabled && _isTCDConnected)
				return true;
			else
			{
				if (!_tcrNotConnectedMsgShown)
				{
					_tcrNotConnectedMsgShown = true;
					//
					using (new WaitCursor())
					{
						try
						{
							dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361049));
							Initialize_cashDispense();
							//
							_cashDisp.IgnoreReturnString = true;
							if (_cashDisp.GetStatus(_tellerVars.DrawerNo.ToString()))
							{
								_isTCDConnected = true;
							}
							else
							{
								_isTCDConnected = false;
							}
						}
						finally
						{
							dlgInformation.Instance.HideInfo();
						}

					}
				}
				return _isTCDConnected;
			}
		}

		private void Initialize_cashDispense()
		{
			if (_cashDisp == null)
				_cashDisp = CashDispenser.Instance;
		}

		private void SetCurTran()
		{
			#region reset cur tran
			_tlTranSet.CurTran.CleanUpValues();
			if (_tlTranSet.Transactions != null && _tlTranSet.Transactions.Count > 0)
				_tlTranSet.Transactions.Clear();
			#endregion

			#region add key fields
			_tlTranSet.CurTran.BranchNo.Value = Convert.ToInt16(colBranchNo.UnFormattedValue);
			if (colTellerDrawerNo.UnFormattedValue != null)
				_tlTranSet.CurTran.DrawerNo.Value = Convert.ToInt16(colTellerDrawerNo.UnFormattedValue);
			_tlTranSet.CurTran.EffectiveDt.Value = Convert.ToDateTime(colEffectiveDt.UnFormattedValue);
			_tlTranSet.CurTran.SequenceNo.Value = Convert.ToInt16(colSequenceNo.UnFormattedValue);
			_tlTranSet.CurTran.SubSequence.Value = Convert.ToInt16(colSubSequence.UnFormattedValue);
			_tlTranSet.CurTran.Ptid.Value = Convert.ToDecimal(colPTID.UnFormattedValue);
			#endregion
		}

		private void ReverseDeviceDispense(decimal tcdCashOut, decimal tcdCashIn)
		{
			if (tcdCashOut <= 0 && tcdCashIn <= 0)
				return;

			decimal devDeposit = 0;
			decimal devDispense = 0;
			_tcdDeviceResp = new ArrayList();
			decimal expDepositAmt = 0;
			decimal coinAmt = 0;
			decimal dollarAmt = 0;
			//
			try
			{
				if (tcdCashOut > 0)
				{
					if (!_clearPendingTcrTran)
					{
						coinAmt = tcdCashOut - System.Math.Round(tcdCashOut, 0);
						dollarAmt = tcdCashOut - coinAmt;
						if (coinAmt > 0)
							//13320 - This transaction had a TCR dispense amount for %1!.  Place %1! in the deposit hopper and click OK. The coin amount of %2! is to be placed in your teller drawer.
							PMessageBox.Show(13320, MessageType.Warning, MessageBoxButtons.OK, dollarAmt.ToString("$#,#0.00"), coinAmt.ToString("$#,#0.00"));
						else
							//13290 - This transaction had a TCR dispense amount for %1!.  Place %1! in the deposit hopper and click OK.
							PMessageBox.Show(13290, MessageType.Warning, MessageBoxButtons.OK, tcdCashOut.ToString("$#,#0.00"));

					}
					//
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11633));
				}
				else if (tcdCashIn > 0)
				{
					if (!_clearPendingTcrTran)
					{
						//13289 - This transaction had a TCR deposited amount for $<999,999,999,999,999.99>.  This amount will be dispensed from the TCR. OK
						PMessageBox.Show(13289, MessageType.Warning, MessageBoxButtons.OK, tcdCashIn.ToString("$#,#0.00"));
					}
					//
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361057));
				}


				using (new WaitCursor())
				{
					Initialize_cashDispense();
					//
					_cashDisp.ReturnCode = 0;
					_cashDisp.IgnoreReturnString = true;
					//
					#region reset cur tran
					_tlTranSet.CurTran.CleanUpValues();
					if (_tlTranSet.Transactions != null && _tlTranSet.Transactions.Count > 0)
						_tlTranSet.Transactions.Clear();
					#endregion
					//
					if (tcdCashOut > 0)
					{
						if (_cashDisp.WindowTitle != _tellerVars.DeviceDepWindowTitle)
						{
							_cashDisp.IsConnOpen = false;
							_cashDisp.WindowTitle = _tellerVars.DeviceDepWindowTitle;
						}

						expDepositAmt = tcdCashOut;
						if (_cashDisp.WindowTitle != _tellerVars.DeviceDepWindowTitle)
						{
							_cashDisp.IsConnOpen = false;
							_cashDisp.WindowTitle = _tellerVars.DeviceDepWindowTitle;
						}
						if (_cashDisp.Deposit(_tellerVars.DrawerNo.ToString(),
							(double)expDepositAmt,
							false,
							_tcdDeviceResp))
						{
							_tlTranSet.LoadTCDCashOutDenom(_tcdDeviceResp, TlTransactionSet.UcmInterface.DeviceDeposit);
							devDeposit = _tlTranSet.GetTotalTcrCash(_tlTranSet.CurTran.CashInDenoms, true);
						}
					}
					if (tcdCashIn > 0)
					{
						if (_cashDisp.WindowTitle != _tellerVars.DeviceRevDepWindowTitle)
						{
							_cashDisp.IsConnOpen = false;
							_cashDisp.WindowTitle = _tellerVars.DeviceRevDepWindowTitle;
						}

						if (_cashDisp.Dispense(_tellerVars.DrawerNo.ToString(),
							(double)tcdCashIn,
							true,
							_tcdDeviceResp))
						{
							_tlTranSet.LoadTCDCashOutDenom(_tcdDeviceResp, TlTransactionSet.UcmInterface.DeviceDispense);
							devDispense = _tlTranSet.GetTotalTcrCash(_tlTranSet.TcdCashOutDenoms, false);
						}
					}
				}

			}
			catch (PhoenixException)
			{
				if (_cashDisp.ReturnCode != 0)
				{
					PMessageBox.Show(317446, MessageType.Warning, MessageBoxButtons.OK,
					new string[] { _cashDisp.GetErrorString(_cashDisp.ReturnCode) });
				}

			}
			finally
			{
				dlgInformation.Instance.HideInfo();
				if (_cashDisp.ReturnCode != 0)
				{
					if (_cashDisp.ReturnCode != 0 && _cashDisp.ReturnCode != -1)
					{
						PMessageBox.Show(317446, MessageType.Warning, MessageBoxButtons.OK,
						new string[] { _cashDisp.GetErrorString(_cashDisp.ReturnCode) });
					}
				}
				else
				{
					if (_clearPendingTcrTran && _clearPendingTcrTranType == "D" && _clearPendingTcrTranAmt == tcdCashIn)
					{
						_clearPendingTcrTran = false;
						_diffIsOnlyCoinAmt = false;
						_clearPendingTcrTranAmt = 0;
						_balCoinAmt = 0;
					}
					if (_clearPendingTcrTran && _clearPendingTcrTranType == "I" && _clearPendingTcrTranAmt == tcdCashOut)
					{
						_clearPendingTcrTran = false;
						_diffIsOnlyCoinAmt = false;
						_clearPendingTcrTranAmt = 0;
						_balCoinAmt = 0;
					}
					//
					if (tcdCashOut > 0 && devDeposit > 0 && tcdCashOut != devDeposit)
					{
						tcdCashIn = 0;
						_clearPendingTcrTran = true;
						_clearPendingTcrTranAmt = devDeposit;
						_clearPendingTcrTranType = "D";
						//
						if (tcdCashOut > devDeposit && (tcdCashOut - devDeposit) < 1)
						{
							_diffIsOnlyCoinAmt = true;
							_balCoinAmt = tcdCashOut - devDeposit;
						}
						tcdCashOut = 0;
					}

					if (tcdCashIn > 0 && devDispense > 0 && tcdCashIn != devDispense)
					{
						tcdCashOut = devDispense;
						tcdCashIn = 0;
						tcdCashOut = 0;
						_clearPendingTcrTran = true;
						_clearPendingTcrTranAmt = devDispense;
						_clearPendingTcrTranType = "I";
					}
				}

			}
		}
		//#9470 - End

		#region #79314
		private bool IsRemoteOverrideEnabled()
		{
			if (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled && Phoenix.Windows.Client.Helper.IsWorkspaceReadOnly(this.Workspace))
				return true;
			return false;
		}

		private void ResetFormForSupViewOnlyMode()
		{

			this.pbReprint.Enabled = false;
			this.pbReversal.Enabled = false;
			this.pbEndorsement.Enabled = false;
			this.pbTranTotals.Enabled = false;
            this.pbImage.Enabled = false;   //#34100
            this.pbViewReceipt.Enabled = false; //#118298
            if (_isSupervisorViewOnlyMode)
			{
				//this.gbBranchDrawerandStatusCriteria.Enabled = !_isSupervisorViewOnlyMode;
				//this.gbTransactionCriteria.Enabled = !_isSupervisorViewOnlyMode;
				MakeReadOnly(false);
				foreach (PAction action in ActionManager.Actions)
				{
					if (!(action == ActionClose || action == pbDisplay || action == pbAcctDisplay ||
						action == pbItemCapture || action == pbSearch))
						action.Enabled = false;
				}
			}
		}
		#endregion


	}
}
