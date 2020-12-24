#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmLnDisplayDetail1.cs
// NameSpace: Phoenix.Client.Loan
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//09/14/2006	1		Vreddy		Issue#67878 - Created.
//05/22/2007	2		mselvaga	#72818 - Teller 2007 - Check/Money Order Dialogue window - the Close Button should be labeled OK and the O on OK should be the accelerator character.
//May-24-07		3   	Muthu       #72780 VS2005 Migration
//01/15/2008    4       SDighe      #74176 - Cashiers checks Window deleting the information.
//04/14/2008    5       mselvaga    #75875 - Beta 2008 TC902 fields do not clear.
//09/11/2008    6       LSimpson    #76057 - Added check printing fields.  Modified payee lines to only allow 40 characters.
//                                           changed payee line population per updated DDS.
//12/19/2008    7       mselvaga    #2088  - Added fix for payee line1 null validation.
//05/03/2009    8       mselvaga    WI-3548 - Added logic to fetch checkinfo stored int tlchkform.
//05Feb2010	    9		GDiNatale	#79370
//04/29/2010    10       rpoddar     #79510 - Fix the ","
//04/15/2010	11		sdhamija	#79572 - fraud detection and prevention - MERGED
// 13May2010	12		#8850
// 17May2010	13		#8560
// 10May2010	10		GDiNatale	#79621 - Printer name is now a combo box, swapped positions for printer name / printer ID
//06/08/10      11      rpoddar     #79510, 09218 - Default payee info for acquirer tran
// 18Jun2010	12		GDiNatale	#9467 - Assigned max len of 40 to dfReasonForCheck and dfRemitter fields.
// 30Jun2010    13      dfutcher    #9591 store check printer ptid
//07/07/2010    14      rpoddar     #79510, #09460 - Added changes for CC/MO printer
//07/13/2010    15      LSimpson    #9723 - Require printer to be selected.
//07/19/2010    16      LSimpson    #9868 - Check state of _paramPrintInfo.ShowPrintInfo before displaying error #13285.
//                                          Also do not parse combo box if not ShowPrintInfo.
// 13Aug2010	17		GDiNatale	#10157 - Limit the printer combo to ONLY what the user has configured on their workstation
//											 as a network printer(s).
//08/26/2010    18      LSimpson    #10420 - Correct issue with payee lines not printing for legacy cashier's checks.
//02/16/2010    19      vsharma     #12073 - Handled money order printer remembering and defaluting
//05/26/2011    20      SDighe      WI#14280- Check printing to work with caps or lowercase print server name
//10/10/2012	21		rpoddar		#140784 - Populate Check Reason for Shared Branch
//05/19/2014	22		JRhyne		WI#29017 - add null checks
//05/29/2014	23		JRhyne		WI#29221 - fix printer port not refreshing
//07/21/2014	24		JRhyne		WI#30201/30203 fix comparison of int to decimal, causes error when no printer (using nexux checks)
//05/30/2017    25      AshishBabu  #60494 Fixed reprinting Error for Check Printing and Reciept Printing if Nexus is not Configured
//01/29/2018    26      mselvaga    #82433 - Error while posting TC 902,910 in offline mode
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.Windows.Forms;
using Phoenix.Shared;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Xfs;
using Phoenix.Shared.Variables;
using Phoenix.Shared.Utility;	// #10157

namespace Phoenix.Client.TlPrinting
{
	/// <summary>
	/// Summary description for dlgCheckInfo.
	/// </summary>
	public class dlgCheckInfo : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region Programmer Declared Variables
		private int _paramRimNo;
		private Phoenix.BusObj.RIM.RmAddress _rmAddressBusObj;
		private string [] _custAddressArray;
		private string [] _custModifiedAddr = new string[6];
		private string _custAddress;
        #region #76057
        Phoenix.Shared.Xfs.PrintInfo _paramPrintInfo = null;
        private TlChkForm _tlChkForm;
        // #79621 - No longer used  - private TlChkPrinter _tlChkPrinter = null;
        private TlTransactionSet _tlTransactionSet;
        #endregion #76057
        #endregion

		private short _TranCode = 0;		// #79370
		private bool _IsLoading = false;		// #79621
        private bool isAcquirerTran = false;     // #79510, #09460
        private short _shBrPtrSetUpType = -1;   // #79510, #09460

        private Phoenix.Windows.Forms.PGroupBoxStandard gbCheckInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblReasonforCheck;
		private Phoenix.Windows.Forms.PdfStandard dfReasonForCheck;
		private Phoenix.Windows.Forms.PLabelStandard lblRemitter;
		private Phoenix.Windows.Forms.PdfStandard dfRemitter;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbPayToTheOrderOfInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblLine1;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine1;
		private Phoenix.Windows.Forms.PLabelStandard lblLine2;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine2;
		private Phoenix.Windows.Forms.PLabelStandard lblLine3;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine3;
		private Phoenix.Windows.Forms.PLabelStandard lblLine4;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine4;
		private Phoenix.Windows.Forms.PLabelStandard lblLine5;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine5;
		private Phoenix.Windows.Forms.PLabelStandard lblLine6;
		private Phoenix.Windows.Forms.PdfStandard dfPayToLine6;

		private Phoenix.Windows.Forms.PAction pbClearAll;
		private Phoenix.Windows.Forms.PAction pbClearLine1;
		private Phoenix.Windows.Forms.PdfStandard dfCheckInformation;
        private Phoenix.Windows.Forms.PdfStandard dfPay;
        private PLabelStandard lblPrinterName;
        // #79621 - lose this control ... private PdfStandard dfPrintName;
        private PGroupBoxStandard gbPrintInfo;
		private PdfStandard dfPrintId;
		private PLabelStandard lblPrinterId;
		private PComboBoxStandard cmbPrintName;	// #79621
		private Phoenix.Windows.Forms.PdfStandard dfAction;
		Phoenix.BusObj.Admin.Checks.AdChkType adChkType = new Phoenix.BusObj.Admin.Checks.AdChkType();	// #140791
		public dlgCheckInfo()
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
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gbCheckInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfRemitter = new Phoenix.Windows.Forms.PdfStandard();
			this.lblRemitter = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfReasonForCheck = new Phoenix.Windows.Forms.PdfStandard();
			this.lblReasonforCheck = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbPayToTheOrderOfInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfPayToLine6 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine6 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPayToLine5 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine5 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPayToLine4 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine4 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPayToLine3 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine3 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPayToLine2 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine2 = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPayToLine1 = new Phoenix.Windows.Forms.PdfStandard();
			this.lblLine1 = new Phoenix.Windows.Forms.PLabelStandard();
			this.pbClearAll = new Phoenix.Windows.Forms.PAction();
			this.pbClearLine1 = new Phoenix.Windows.Forms.PAction();
			this.dfCheckInformation = new Phoenix.Windows.Forms.PdfStandard();
			this.dfPay = new Phoenix.Windows.Forms.PdfStandard();
			this.dfAction = new Phoenix.Windows.Forms.PdfStandard();
			this.lblPrinterName = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbPrintInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.cmbPrintName = new Phoenix.Windows.Forms.PComboBoxStandard();
			this.dfPrintId = new Phoenix.Windows.Forms.PdfStandard();
			this.lblPrinterId = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbCheckInformation.SuspendLayout();
			this.gbPayToTheOrderOfInformation.SuspendLayout();
			this.gbPrintInfo.SuspendLayout();
			this.SuspendLayout();
			//
			// ActionManager
			//
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbClearAll,
            this.pbClearLine1});
			//
			// gbCheckInformation
			//
			this.gbCheckInformation.Controls.Add(this.dfRemitter);
			this.gbCheckInformation.Controls.Add(this.lblRemitter);
			this.gbCheckInformation.Controls.Add(this.dfReasonForCheck);
			this.gbCheckInformation.Controls.Add(this.lblReasonforCheck);
			this.gbCheckInformation.Location = new System.Drawing.Point(4, 66);
			this.gbCheckInformation.Name = "gbCheckInformation";
			this.gbCheckInformation.Size = new System.Drawing.Size(400, 68);
			this.gbCheckInformation.TabIndex = 1;
			this.gbCheckInformation.TabStop = false;
			this.gbCheckInformation.Text = "Check Information";
			//
			// dfRemitter
			//
			this.dfRemitter.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfRemitter.Location = new System.Drawing.Point(120, 40);
			this.dfRemitter.MaxLength = 40;
			this.dfRemitter.Name = "dfRemitter";
			this.dfRemitter.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfRemitter.PhoenixUIControl.ObjectId = 3;
			this.dfRemitter.Size = new System.Drawing.Size(269, 20);
			this.dfRemitter.TabIndex = 3;
			//
			// lblRemitter
			//
			this.lblRemitter.AutoEllipsis = true;
			this.lblRemitter.Location = new System.Drawing.Point(8, 40);
			this.lblRemitter.Name = "lblRemitter";
			this.lblRemitter.PhoenixUIControl.ObjectId = 3;
			this.lblRemitter.Size = new System.Drawing.Size(106, 20);
			this.lblRemitter.TabIndex = 2;
			this.lblRemitter.Text = "Remitter:";
			//
			// dfReasonForCheck
			//
			this.dfReasonForCheck.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfReasonForCheck.Location = new System.Drawing.Point(120, 16);
			this.dfReasonForCheck.MaxLength = 40;
			this.dfReasonForCheck.Name = "dfReasonForCheck";
			this.dfReasonForCheck.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfReasonForCheck.PhoenixUIControl.ObjectId = 2;
			this.dfReasonForCheck.Size = new System.Drawing.Size(269, 20);
			this.dfReasonForCheck.TabIndex = 1;
			//
			// lblReasonforCheck
			//
			this.lblReasonforCheck.AutoEllipsis = true;
			this.lblReasonforCheck.Location = new System.Drawing.Point(8, 16);
			this.lblReasonforCheck.Name = "lblReasonforCheck";
			this.lblReasonforCheck.PhoenixUIControl.ObjectId = 2;
			this.lblReasonforCheck.Size = new System.Drawing.Size(106, 20);
			this.lblReasonforCheck.TabIndex = 0;
			this.lblReasonforCheck.Text = "Reason for Check:";
			//
			// gbPayToTheOrderOfInformation
			//
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine6);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine6);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine5);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine5);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine4);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine4);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine3);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine3);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine2);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine2);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.dfPayToLine1);
			this.gbPayToTheOrderOfInformation.Controls.Add(this.lblLine1);
			this.gbPayToTheOrderOfInformation.Location = new System.Drawing.Point(4, 134);
			this.gbPayToTheOrderOfInformation.Name = "gbPayToTheOrderOfInformation";
			this.gbPayToTheOrderOfInformation.Size = new System.Drawing.Size(400, 164);
			this.gbPayToTheOrderOfInformation.TabIndex = 2;
			this.gbPayToTheOrderOfInformation.TabStop = false;
			this.gbPayToTheOrderOfInformation.Text = "Pay To The Order Of Information";
			//
			// dfPayToLine6
			//
			this.dfPayToLine6.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine6.Location = new System.Drawing.Point(120, 136);
			this.dfPayToLine6.MaxLength = 40;
			this.dfPayToLine6.Name = "dfPayToLine6";
			this.dfPayToLine6.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine6.PhoenixUIControl.ObjectId = 12;
			this.dfPayToLine6.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine6.TabIndex = 11;
			//
			// lblLine6
			//
			this.lblLine6.AutoEllipsis = true;
			this.lblLine6.Location = new System.Drawing.Point(8, 136);
			this.lblLine6.Name = "lblLine6";
			this.lblLine6.PhoenixUIControl.ObjectId = 12;
			this.lblLine6.Size = new System.Drawing.Size(106, 20);
			this.lblLine6.TabIndex = 10;
			this.lblLine6.Text = "Line &6:";
			//
			// dfPayToLine5
			//
			this.dfPayToLine5.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine5.Location = new System.Drawing.Point(120, 112);
			this.dfPayToLine5.MaxLength = 40;
			this.dfPayToLine5.Name = "dfPayToLine5";
			this.dfPayToLine5.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine5.PhoenixUIControl.ObjectId = 11;
			this.dfPayToLine5.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine5.TabIndex = 9;
			//
			// lblLine5
			//
			this.lblLine5.AutoEllipsis = true;
			this.lblLine5.Location = new System.Drawing.Point(8, 112);
			this.lblLine5.Name = "lblLine5";
			this.lblLine5.PhoenixUIControl.ObjectId = 11;
			this.lblLine5.Size = new System.Drawing.Size(106, 20);
			this.lblLine5.TabIndex = 8;
			this.lblLine5.Text = "Line &5:";
			//
			// dfPayToLine4
			//
			this.dfPayToLine4.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine4.Location = new System.Drawing.Point(120, 88);
			this.dfPayToLine4.MaxLength = 40;
			this.dfPayToLine4.Name = "dfPayToLine4";
			this.dfPayToLine4.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine4.PhoenixUIControl.ObjectId = 8;
			this.dfPayToLine4.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine4.TabIndex = 7;
			//
			// lblLine4
			//
			this.lblLine4.AutoEllipsis = true;
			this.lblLine4.Location = new System.Drawing.Point(8, 88);
			this.lblLine4.Name = "lblLine4";
			this.lblLine4.PhoenixUIControl.ObjectId = 8;
			this.lblLine4.Size = new System.Drawing.Size(106, 20);
			this.lblLine4.TabIndex = 6;
			this.lblLine4.Text = "Line &4:";
			//
			// dfPayToLine3
			//
			this.dfPayToLine3.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine3.Location = new System.Drawing.Point(120, 64);
			this.dfPayToLine3.MaxLength = 40;
			this.dfPayToLine3.Name = "dfPayToLine3";
			this.dfPayToLine3.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine3.PhoenixUIControl.ObjectId = 7;
			this.dfPayToLine3.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine3.TabIndex = 5;
			//
			// lblLine3
			//
			this.lblLine3.AutoEllipsis = true;
			this.lblLine3.Location = new System.Drawing.Point(8, 64);
			this.lblLine3.Name = "lblLine3";
			this.lblLine3.PhoenixUIControl.ObjectId = 7;
			this.lblLine3.Size = new System.Drawing.Size(106, 20);
			this.lblLine3.TabIndex = 4;
			this.lblLine3.Text = "Line &3:";
			//
			// dfPayToLine2
			//
			this.dfPayToLine2.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine2.Location = new System.Drawing.Point(120, 40);
			this.dfPayToLine2.MaxLength = 40;
			this.dfPayToLine2.Name = "dfPayToLine2";
			this.dfPayToLine2.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine2.PhoenixUIControl.ObjectId = 6;
			this.dfPayToLine2.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine2.TabIndex = 3;
			//
			// lblLine2
			//
			this.lblLine2.AutoEllipsis = true;
			this.lblLine2.Location = new System.Drawing.Point(8, 40);
			this.lblLine2.Name = "lblLine2";
			this.lblLine2.PhoenixUIControl.ObjectId = 6;
			this.lblLine2.Size = new System.Drawing.Size(106, 20);
			this.lblLine2.TabIndex = 2;
			this.lblLine2.Text = "Payee Line &2:";
			//
			// dfPayToLine1
			//
			this.dfPayToLine1.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine1.Location = new System.Drawing.Point(120, 16);
			this.dfPayToLine1.MaxLength = 40;
			this.dfPayToLine1.Name = "dfPayToLine1";
			this.dfPayToLine1.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPayToLine1.PhoenixUIControl.ObjectId = 5;
			this.dfPayToLine1.Size = new System.Drawing.Size(269, 20);
			this.dfPayToLine1.TabIndex = 1;
			//
			// lblLine1
			//
			this.lblLine1.AutoEllipsis = true;
			this.lblLine1.Location = new System.Drawing.Point(8, 16);
			this.lblLine1.Name = "lblLine1";
			this.lblLine1.PhoenixUIControl.ObjectId = 5;
			this.lblLine1.Size = new System.Drawing.Size(106, 20);
			this.lblLine1.TabIndex = 0;
			this.lblLine1.Text = "Payee Line &1:";
			//
			// pbClearAll
			//
			this.pbClearAll.LongText = "pbClearAll";
			this.pbClearAll.ObjectId = 13;
			this.pbClearAll.ShortText = "pbClearAll";
			this.pbClearAll.Tag = null;
			this.pbClearAll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClearAll_Click);
			//
			// pbClearLine1
			//
			this.pbClearLine1.LongText = "pbClearLine1";
			this.pbClearLine1.ObjectId = 14;
			this.pbClearLine1.ShortText = "pbClearLine1";
			this.pbClearLine1.Tag = null;
			this.pbClearLine1.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClearLine1_Click);
			//
			// dfCheckInformation
			//
			this.dfCheckInformation.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfCheckInformation.Location = new System.Drawing.Point(405, 5);
			this.dfCheckInformation.Name = "dfCheckInformation";
			this.dfCheckInformation.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfCheckInformation.PhoenixUIControl.ObjectId = 1;
			this.dfCheckInformation.Size = new System.Drawing.Size(100, 20);
			this.dfCheckInformation.TabIndex = 0;
			//
			// dfPay
			//
			this.dfPay.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPay.Location = new System.Drawing.Point(405, 5);
			this.dfPay.Name = "dfPay";
			this.dfPay.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPay.PhoenixUIControl.ObjectId = 4;
			this.dfPay.Size = new System.Drawing.Size(100, 20);
			this.dfPay.TabIndex = 0;
			//
			// dfAction
			//
			this.dfAction.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfAction.Location = new System.Drawing.Point(405, 5);
			this.dfAction.Name = "dfAction";
			this.dfAction.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfAction.PhoenixUIControl.ObjectId = 9;
			this.dfAction.Size = new System.Drawing.Size(100, 20);
			this.dfAction.TabIndex = 0;
			//
			// lblPrinterName
			//
			this.lblPrinterName.AutoEllipsis = true;
			this.lblPrinterName.Location = new System.Drawing.Point(8, 16);
			this.lblPrinterName.Name = "lblPrinterName";
			this.lblPrinterName.Size = new System.Drawing.Size(106, 20);
			this.lblPrinterName.TabIndex = 2;
			this.lblPrinterName.Text = "Printer Name:";
			//
			// gbPrintInfo
			//
			this.gbPrintInfo.Controls.Add(this.cmbPrintName);
			this.gbPrintInfo.Controls.Add(this.dfPrintId);
			this.gbPrintInfo.Controls.Add(this.lblPrinterId);
			this.gbPrintInfo.Controls.Add(this.lblPrinterName);
			this.gbPrintInfo.Location = new System.Drawing.Point(4, 0);
			this.gbPrintInfo.Name = "gbPrintInfo";
			this.gbPrintInfo.PhoenixUIControl.ObjectId = 15;
			this.gbPrintInfo.Size = new System.Drawing.Size(400, 67);
			this.gbPrintInfo.TabIndex = 0;
			this.gbPrintInfo.TabStop = false;
			this.gbPrintInfo.Text = "Print Information";
			//
			// cmbPrintName
			//
			this.cmbPrintName.Location = new System.Drawing.Point(120, 14);
			this.cmbPrintName.Name = "cmbPrintName";
			this.cmbPrintName.Size = new System.Drawing.Size(269, 21);
			this.cmbPrintName.TabIndex = 6;
			this.cmbPrintName.Value = null;
			this.cmbPrintName.SelectedIndexChanged += new System.EventHandler(this.cmbPrintName_SelectedIndexChanged);
			//
			// dfPrintId
			//
			this.dfPrintId.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPrintId.Enabled = false;
			this.dfPrintId.Location = new System.Drawing.Point(120, 38);
			this.dfPrintId.Name = "dfPrintId";
			this.dfPrintId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfPrintId.PhoenixUIControl.ObjectId = 17;
			this.dfPrintId.ReadOnly = true;
			this.dfPrintId.Size = new System.Drawing.Size(269, 20);
			this.dfPrintId.TabIndex = 5;
			//
			// lblPrinterId
			//
			this.lblPrinterId.AutoEllipsis = true;
			this.lblPrinterId.Location = new System.Drawing.Point(8, 38);
			this.lblPrinterId.Name = "lblPrinterId";
			this.lblPrinterId.Size = new System.Drawing.Size(106, 20);
			this.lblPrinterId.TabIndex = 4;
			this.lblPrinterId.Text = "Printer ID:";
			//
			// dlgCheckInfo
			//
			this.AutoFetch = false;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(408, 303);
			this.Controls.Add(this.gbPrintInfo);
			this.Controls.Add(this.gbPayToTheOrderOfInformation);
			this.Controls.Add(this.gbCheckInformation);
			this.Name = "dlgCheckInfo";
			this.Load += new System.EventHandler(this.dlgCheckInfo_Load);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgCheckInfo_PInitCompleteEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgCheckInfo_PInitBeginEvent);
			this.gbCheckInformation.ResumeLayout(false);
			this.gbCheckInformation.PerformLayout();
			this.gbPayToTheOrderOfInformation.ResumeLayout(false);
			this.gbPayToTheOrderOfInformation.PerformLayout();
			this.gbPrintInfo.ResumeLayout(false);
			this.gbPrintInfo.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region InitParameters
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 3)	// #79370 - added TranCode parameter from parent window ...
			{
				try
				{
					_paramRimNo = Convert.ToInt32(paramList[0]);
					_paramPrintInfo = paramList[1] as Phoenix.Shared.Xfs.PrintInfo; // #76057
					// _TranCode = (short)paramList[2];	// #79370
					_tlTransactionSet = paramList[2] as TlTransactionSet;	// #79621

					if (_tlTransactionSet != null)
					{
						_TranCode = _tlTransactionSet.CurTran.TranCode.Value;	// #79370
					}
				}
				catch(PhoenixException ex)
				{
					_paramRimNo = -1;
					CoreService.LogPublisher.LogDebug("\n"+ "Some Parameter misshap" + ex.Message + "\n" +  ex.InnerException.Message);
					return;
				}
				this.IsNew = false;
			}

            //Begin #79510, #09460
            if (paramList.Length == 1)  // from the MDI menu
            {
                _shBrPtrSetUpType = Convert.ToInt16(paramList[0]);
            }
            //End #79510, #09460

			_rmAddressBusObj = new Phoenix.BusObj.RIM.RmAddress();
            #region #76057
            _tlChkForm = new TlChkForm();

			/* #79621 - We're not using this any more ...
            _tlChkPrinter = new Phoenix.BusObj.TL.TlChkPrinter();
            _tlChkPrinter.ResponseTypeId = 10;
            _tlChkPrinter.BranchNo.Value = Convert.ToInt16(_paramPrintInfo.BranchNo);
			*/

            #endregion #76057
            // #79621 - _tlTransactionSet = new TlTransactionSet();

			//Needs some explanation
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.CheckInfo;
			base.InitParameters (paramList);
		}
		#endregion InitParameters

		#region Form Events

		#region dlgCheckInfo_PInitBeginEvent
		private ReturnType dlgCheckInfo_PInitBeginEvent()
		{
			this.AvoidSave = true;
			pbClearAll.NextScreenId = 0;
			pbClearLine1.NextScreenId = 0;
			ActionClose.ObjectId = 10; //#72818
			ActionClose.Image = Images.Ok;	//#72818

			return ReturnType.Success;
		}
		#endregion dlgCheckInfo_PInitBeginEvent

		#region dlgCheckInfo_PInitCompleteEvent
		private void dlgCheckInfo_PInitCompleteEvent()
		{
            //Begin #79510, #09460
            if (_shBrPtrSetUpType > 0)
            {
                PopulatePrinterCombo(_shBrPtrSetUpType == 1 ? 902 : 910);
                this.gbCheckInformation.Visible = false;
                this.gbPayToTheOrderOfInformation.Visible = false;
                //this.gbPrintInfo.Size = new Size(this.gbPrintInfo.Size.Width, this.gbPrintInfo.Size.Height + 10);
                this.ClientSize = new Size(this.Width, this.gbPrintInfo.Location.Y  + this.gbPrintInfo.Size.Height+ 2);
                foreach (PAction action in this.ActionManager.Actions)
                    action.Visible = false;
                this.ActionClose.Visible = true;

                return;
            }
            isAcquirerTran = Workspace != null && Workspace.Variables["IsAcquirerTran"] != null &&
                Convert.ToBoolean(Workspace.Variables["IsAcquirerTran"]);
            //End #79510, #09460

            if (TellerVars.Instance.IsAppOnline)    //#82433
                PopulatePrinterCombo(_TranCode);	// #79621

			//Begin #79572
			// #8560 - Fix Sukhwinder's logic ... make sure we actually have a workspace!
			if (this.Workspace != null)
			{
				if (this.Workspace.Variables["NonCust"] != null)
					_rmAddressBusObj.NonCust.Value = this.Workspace.Variables["NonCust"].ToString();
				else
					_rmAddressBusObj.NonCust.Value = "N";
				//End #79572
			}

			if ( TellerVars.Instance.IsAppOnline )
			{
				_custAddress = this._rmAddressBusObj.GetCurrentPrimaryAddress(_paramRimNo);
				_custAddressArray = _custAddress.Split(Convert.ToChar(167));
				CreateAddress();
            }
            #region #76057 Resize Window
            if (!TellerVars.Instance.IsAppOnline)
            {
                dfPrintId.UnFormattedValue = "";
                // #79621 - dfPrintName.UnFormattedValue = "";
            }
            else
            {
                // dfPrintId.UnFormattedValue = _paramPrintInfo.CheckPrinterID;
                // #79621 - dfPrintName.UnFormattedValue = _paramPrintInfo.CheckPrinterName;
            }
            if (_paramPrintInfo.ShowPrintInfo == false)
            {
                gbPrintInfo.Visible = false;
                gbCheckInformation.Top = 0;
                gbPayToTheOrderOfInformation.Top = 66;
                this.Height = 276;
            }
            else
            {
                gbPrintInfo.Visible = true;
                gbCheckInformation.Top = 66;
                gbPayToTheOrderOfInformation.Top = 134;
                this.Height = 337;
                //this.dfPayToLine1.PhoenixUIControl.IsNullable = NullabilityState.NotNull; //#2088
            }

            #endregion #76057

            //Begin #79510, 09218
            if (isAcquirerTran)
            {
                if (dfPayToLine1.Value == null || dfPayToLine1.Value.ToString() == string.Empty)
                    dfPayToLine1.SetValue(_paramPrintInfo.CheckLine1);
                //Begin #140784
                if (dfReasonForCheck.Value == null || dfReasonForCheck.Value.ToString() == string.Empty)
                    dfReasonForCheck.SetValue(_paramPrintInfo.ReasonForCheck);
                //End #140784
                cmbPrintName.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);   // #09460
            }
            //End #79510, 09218
        }
		#region CreateAddress
		private void CreateAddress()
		{

			dfReasonForCheck.Text = string.Empty;
			dfReasonForCheck.ScreenToObject();
			dfRemitter.Text = string.Empty;
			dfRemitter.ScreenToObject();
			pbClearAll.PerformClick();
			string international = string.Empty;
			//sLine1, sLine2, sLine3, sLine4, sLine5, sCity, sState, sZip, sInternational, sCountry,rsPayToLine1, rsPayToLine2, rsPayToLine3, rsPayToLine4, rsPayToLine5, rsPayToLine6
			if (_custAddressArray.Length > 0)
			{
				int ncount = 0;
				if (_custAddressArray[0].Length > 0)
					_custModifiedAddr[ncount++] = _custAddressArray[0]; //Name 1
				if (_custAddressArray[1].Length > 0)
					_custModifiedAddr[ncount++] = _custAddressArray[1]; //Name 2
				if (_custAddressArray[2].Length > 0)
					_custModifiedAddr[ncount++] = _custAddressArray[2]; //Address Line 1
				if (_custAddressArray[3].Length > 0)
					_custModifiedAddr[ncount++] = _custAddressArray[3]; //Address Line 2
				if (_custAddressArray[4].Length > 0)
					_custModifiedAddr[ncount++] = _custAddressArray[4]; //Address Line 3
				if (_custAddressArray[8].Length > 0)
					international = _custAddressArray[8];
				else
					international = "N";
				if (international == "Y")
					_custModifiedAddr[ncount++] = _custAddressArray[9]; //Country
				else
				{
					//                              City                         State                          Zip
					_custModifiedAddr[ncount++] = _custAddressArray[5] + ", " +  _custAddressArray[6] + "   " + _custAddressArray[7];
				}

                //Begin #79510
                if (_custModifiedAddr[0] != null && _custModifiedAddr[0].Trim() == ",")
                    _custModifiedAddr[0] = null;
                //End #79510

				dfRemitter.Text = _custModifiedAddr[0]; // #79370 - Always default the RIM name as remitter ...

				if (! CheckInfoDefaultsToBlank())	// #79370 - Fill in the fields if check info does not default to blank ...
				{
					dfPayToLine1.Text = _custModifiedAddr[0];
					#region #76057 Shift payto lines
					//dfPayToLine2.Text = _custModifiedAddr[1];
					//dfPayToLine3.Text = _custModifiedAddr[2];
					//dfPayToLine4.Text = _custModifiedAddr[3];
					//dfPayToLine5.Text = _custModifiedAddr[4];
					//dfPayToLine6.Text = _custModifiedAddr[5];
					dfPayToLine2.Text = null;
					dfPayToLine3.Text = _custModifiedAddr[1];
					dfPayToLine4.Text = _custModifiedAddr[2];
					dfPayToLine5.Text = _custModifiedAddr[3];
					dfPayToLine6.Text = _custModifiedAddr[4];
					// what do we do with _custModifiedAddr[5]?
					#endregion #76057

                //SDighe - Issue 74176
                dfPayToLine1.UnFormattedValue = _custModifiedAddr[0];
                #region #76057 Shift payto lines
                //dfPayToLine2.UnFormattedValue = _custModifiedAddr[1];
                //dfPayToLine3.UnFormattedValue = _custModifiedAddr[2];
                //dfPayToLine4.UnFormattedValue = _custModifiedAddr[3];
                //dfPayToLine5.UnFormattedValue = _custModifiedAddr[4];
                //dfPayToLine6.UnFormattedValue = _custModifiedAddr[5];
                dfPayToLine2.UnFormattedValue = null;
                dfPayToLine3.UnFormattedValue = _custModifiedAddr[1];
                dfPayToLine4.UnFormattedValue = _custModifiedAddr[2];
                dfPayToLine5.UnFormattedValue = _custModifiedAddr[3];
                dfPayToLine6.UnFormattedValue = _custModifiedAddr[4];
                // what do we do with _custModifiedAddr[5]?
                #endregion #76057
				}
                //
                //Begin #3548
                if (_paramPrintInfo.IsCheckReprint)
                {
                    //Copy the values from tl_chk_form
                    _tlChkForm.JournalPtid.Value = _paramPrintInfo.JournalPtid;
                    _tlChkForm.LoadChkInfo();
                    //
                    dfPayToLine1.UnFormattedValue = _tlChkForm.PayLine1.Value;
                    dfPayToLine2.UnFormattedValue = _tlChkForm.PayLine2.Value;
                    dfPayToLine3.UnFormattedValue = _tlChkForm.Line3.Value;
                    dfPayToLine4.UnFormattedValue = _tlChkForm.Line4.Value;
                    dfPayToLine5.UnFormattedValue = _tlChkForm.Line5.Value;
                    dfPayToLine6.UnFormattedValue = _tlChkForm.Line6.Value;
                    dfRemitter.UnFormattedValue = _tlChkForm.Remitter.Value;
                    dfReasonForCheck.UnFormattedValue = _tlChkForm.Reason.Value;
                    //
                    dfRemitter.ScreenToObject();
                    dfReasonForCheck.ScreenToObject();
                }
                //End #3548
                //SDighe - end Issue 74176
				dfPayToLine1.ScreenToObject();
				dfPayToLine2.ScreenToObject();
				dfPayToLine3.ScreenToObject();
				dfPayToLine4.ScreenToObject();
				dfPayToLine5.ScreenToObject();
				dfPayToLine6.ScreenToObject();
			}

		}
		#endregion CreateAddress

		#endregion dlgCheckInfo_PInitCompleteEvent

		#region OnActionClose
		public override bool OnActionClose()
        {
            //Begin #79510, #09460
            if (_shBrPtrSetUpType > 0)
            {
                return base.OnActionClose();
            }
            //End #79510, #09460

            #region  Retain Printing Info #76057
            _paramPrintInfo.ReasonForCheck = dfReasonForCheck.Text;
			_paramPrintInfo.CheckRemitter = dfRemitter.Text;
			_paramPrintInfo.CheckLine1 = dfPayToLine1.Text;
            _paramPrintInfo.CheckLine2 = dfPayToLine2.Text;
            _paramPrintInfo.CheckLine3 = dfPayToLine3.Text;
            _paramPrintInfo.CheckLine4 = dfPayToLine4.Text;
            _paramPrintInfo.CheckLine5 = dfPayToLine5.Text;
            _paramPrintInfo.CheckLine6 = dfPayToLine6.Text;

			// #79621 - Add our current printer name and port to _paramPrintInfo
			_paramPrintInfo.CheckPrinterName = adChkType.PrinterName.Value;	// #140791 //_tlTransactionSet.TellerVars.TlChkPrinter.ChkPrintName.Value;
			_paramPrintInfo.CheckPrinterID = adChkType.PrinterPath.Value;	// #140791 // _tlTransactionSet.TellerVars.TlChkPrinter.ChkPrintPort.Value;

            #region #9723 - #10420 - moved
            if (cmbPrintName.Items.Count == 0)
                return base.OnActionClose();
            if ((cmbPrintName.Text == "" || cmbPrintName.SelectedIndex < 0) && _paramPrintInfo.ShowPrintInfo == true) // #9868 - Added ShowPrintInfo param
            {
                PMessageBox.Show(13285, MessageType.Error, null);
                return false;
            }
            #endregion

            // #9591
            #region #9868
            if (_paramPrintInfo.ShowPrintInfo == true)
            {
                int printerPtid;
                //int.TryParse(this.cmbPrintName.CodeValue.ToString(), out printerPtid);
                int.TryParse(this.cmbPrintName.CodeValue.ToString().Split('~')[0], out printerPtid); //#60494
                _paramPrintInfo.PrinterPtid = printerPtid;
            }
            #endregion
			_paramPrintInfo.PrinterTray = adChkType.PrinterTray.Value; // #140791 // _tlTransactionSet.TellerVars.TlChkPrinter.Tray.Value;

            //Begin #2088
            if (_paramPrintInfo.ShowPrintInfo)
            {
                if (dfPayToLine1.UnFormattedValue == null ||
                    dfPayToLine1.UnFormattedValue != null && (dfPayToLine1.Text.Trim() == "" || dfPayToLine1.Text.Trim() == null ||
                    dfPayToLine1.Text.Trim() == string.Empty))
				{
					#region Deleted Code
					/* #8850 - Lose this original enhancement code and take different approach below ...
					 *
					// #79370 - Lose this message IF AdTlControl.DefaultBlankMo == "Y" AND this is a Money Order ...
					if ((_tlTransactionSet.AdTlControl.DefaultBlankMo.Value != Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y) &&
						_TranCode != 910)
					{
						//10983 - field cannot be null.
						PMessageBox.Show(10983, MessageType.Warning, MessageBoxButtons.OK, new string[] { dfPayToLine1.PhoenixUIControl.MLInfo.ProperName });
						dfPayToLine1.Focus();
						return false;
					}
					*/
					#endregion

					if (CheckInfoDefaultsToBlank())	// #8850
					{
						if (_TranCode == 902)
						{
								// Only Cashier's checks MUST have PayeeLine1, regardless of Blank CC setting ...
								ShowMessage10983();
								return false;
						}
					}
					else
					{
						ShowMessage10983();
						return false;
					}


                }

				// #79621
				//if (TellerVars.Instance.TlChkPrinter == null)	// #140791
				if (adChkType.PrinterPtid.IsNull)				// #140791
				{
					// 13240  - You must select a printer to continue.
					PMessageBox.Show(13240, MessageType.Error, null);
					return false;
				}

            }

            //End #2088
            #endregion
            return base.OnActionClose ();
		}
		#endregion OnActionClose

		#region pbClearLine1_Click
		public override bool OnActionSave(bool isAddNext)
		{
			return true;  //Do Nothing
			//return base.OnActionSave (isAddNext);
		}
		#endregion pbClearLine1_Click

		#endregion Form Events

		#region PushButtons

		#region pbClearAll_Click
		private void pbClearAll_Click(object sender, PActionEventArgs e)
		{
			dfPayToLine1.Text = string.Empty;
			dfPayToLine2.Text = string.Empty;
			dfPayToLine3.Text = string.Empty;
			dfPayToLine4.Text = string.Empty;
			dfPayToLine5.Text = string.Empty;
			dfPayToLine6.Text = string.Empty;
            //
            //#75875 - Damn UnFormattedValue, if not set, it screw up the screen to object.
            dfPayToLine1.UnFormattedValue = null;
            dfPayToLine2.UnFormattedValue = null;
            dfPayToLine3.UnFormattedValue = null;
            dfPayToLine4.UnFormattedValue = null;
            dfPayToLine5.UnFormattedValue = null;
            dfPayToLine6.UnFormattedValue = null;
            //
			dfPayToLine1.ScreenToObject();
			dfPayToLine2.ScreenToObject();
			dfPayToLine3.ScreenToObject();
			dfPayToLine4.ScreenToObject();
			dfPayToLine5.ScreenToObject();
			dfPayToLine6.ScreenToObject();
		}
		#endregion pbClearAll_Click

		#region pbClearLine1_Click
		private void pbClearLine1_Click(object sender, PActionEventArgs e)
		{
			dfPayToLine1.Text = string.Empty;
            //#75875 - Damn UnFormattedValue, if not set, it screw up the screen to object.
            dfPayToLine1.UnFormattedValue = null;
			dfPayToLine1.ScreenToObject();
		}
		#endregion pbClearLine1_Click

        private void dlgCheckInfo_Load(object sender, EventArgs e)
        {

        }

		#endregion PushButtons

		#region #79370 Methods

		/// <summary>
		/// Determines if the check info fields should default to blank ...
		/// </summary>
		/// <returns></returns>
		private bool CheckInfoDefaultsToBlank()
		{
			bool Result = false;

			if (_TranCode == 902 &&
				_tlTransactionSet.AdTlControl.DefaultBlankCc.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y)
			{
				Result = true;
			}
			else if (_TranCode == 910 &&
				_tlTransactionSet.AdTlControl.DefaultBlankMo.Value == Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y)
			{
				Result = true;
			}

			return Result;
		}


		/// <summary>
		/// Shows the Payee1 cannot be blank message.
		/// </summary>
		private void ShowMessage10983()		// #8850
		{
			//10983 - field cannot be null.
			PMessageBox.Show(10983, MessageType.Warning, MessageBoxButtons.OK, new string[] { dfPayToLine1.PhoenixUIControl.MLInfo.ProperName });

			if (dfPayToLine1.CanFocus)
			{
				dfPayToLine1.Focus();
			}
		}

		#endregion

		#region #79621 Methods

		/// <summary>
		/// Populates the printer combo and sets the default printer from tl_journal, if one exists.
		/// </summary>
		/// <param name="tranCode"></param>
		private void PopulatePrinterCombo(int tranCode)	// #140791 - rewrite
		{
			#region Deleted Code #140791
			//ArrayList Constraint = new ArrayList();
			//ArrayList CheckPrinters = null;
			//string CheckType = null;
			//EnumValue EValue = null;
			//int ChkPrinterPtid = int.MinValue;
			//string PrinterPort = null;
			//bool CanSetDefault = false;

			//_IsLoading = true;

			//try
			//{
			//    if (TellerVars.Instance.TlChkPrinterArray != null && TellerVars.Instance.TlChkPrinterArray.Count > 0)
			//    {
			//        CheckPrinters = TellerVars.Instance.TlChkPrinterArray;

			//        switch (tranCode)
			//        {
			//            case 902:
			//                CheckType = "CashiersCheck";
			//                break;

			//            case 910:
			//                CheckType = "MoneyOrder";
			//                break;
			//        }
			//        //12073- Moved Below Switch to get Check Type
			//        ChkPrinterPtid = GetTlChkPrinterPtid(CheckType);		// Get the default check printer from tl_journal ...

			//        foreach(TlChkPrinter Printer in CheckPrinters)
			//        {
			//            if (Printer.CheckType.Value == CheckType)
			//            {
			//                EValue = new EnumValue(Printer.Ptid.Value, Printer.ChkPrintName.Value);
			//                if (PrinterIsDefinedOnWorkstation(Printer.ChkPrintPort.Value))	// #10157
			//                {
			//                    Constraint.Add(EValue);

			//                    if (Printer.Ptid.IntValue == ChkPrinterPtid)
			//                    {
			//                        // Default printer is in our arry list, so we CAN set a default printer
			//                        CanSetDefault = true;
			//                    }
			//                }
			//            }
			//        }

			//        if (Constraint.Count > 0)
			//        {
			//            this.cmbPrintName.Populate(Constraint);
			//            cmbPrintName.Items.Remove("<none>");
			//        }

			//        if (ChkPrinterPtid > 0 && CanSetDefault)	// ONLY do this if we have a default AND it's in our list for the given check type.
			//        {
			//            cmbPrintName.CodeValue = ChkPrinterPtid;
			//            SetPrinter((decimal)ChkPrinterPtid, out PrinterPort, CheckType); //#12073
			//            if (!string.IsNullOrEmpty(PrinterPort))
			//            {
			//                dfPrintId.Text = PrinterPort;
			//            }
			//        }
			//    }
			//}
			//finally
			//{
			//    _IsLoading = false;
			//}
			#endregion

			_IsLoading = true;

			switch (tranCode)
			{
				case 902:
					adChkType.ChkType.Value = "TLC600";
					break;

				case 910:
					adChkType.ChkType.Value = "TLC605";
					break;
			}

			adChkType.PopulatePrinterInfo();
			this.cmbPrintName.Populate(adChkType.PrinterInfo.EnumValues);

			if (adChkType.PrinterInfo.EnumValues.Count > 0)
				cmbPrintName.Items.Remove("<none>");

			string PrinterPort;
			int ChkPrinterPtid = GetTlChkPrinterPtid(adChkType.ChkType.Value);		// Get the default check printer from tl_journal ...	// WI#30201/30202 change to int

			#region Set CheckType variable "CashiersCheck" or "MoneyOrder"
			string CheckType="";
			switch (tranCode)
			{
				case 902:
					CheckType = "CashiersCheck";
					break;

				case 910:
					CheckType = "MoneyOrder";
					break;
			}
			#endregion

			cmbPrintName.SelectedIndex = 0;
			if (ChkPrinterPtid == int.MinValue && this.cmbPrintName.CodeValue != null)	// WI#29017 add null check
				int.TryParse(this.cmbPrintName.CodeValue.ToString().Split('~')[0], out ChkPrinterPtid);	// #140791

			if (ChkPrinterPtid != int.MinValue)	// WI#29017 add null check	// WI#29221 	// WI#30201/30202 change comparison to int.MinValue
				SetPrinter(ChkPrinterPtid, out PrinterPort, CheckType);

			_IsLoading = false;
		}

		/// <summary>
		/// Sets the currently selected printer
		/// </summary>
		/// <param name="ptid"></param>
		/// <param name="printerPort"></param>
		private void SetPrinter(decimal ptid, out string printerPort , string CheckType)
		{
			printerPort = null;

			// begin #140791
			adChkType.PrinterPtid.SetValue(ptid);	// changing the ptid will automatically populate the printers

			if (!adChkType.PrinterPath.IsNull)
			{
				//TellerVars.Instance.SetPrinterContextObject(ptid);	// #40791
				if (_tlTransactionSet != null)    // #79510, #09460
					_tlTransactionSet.PrinterPtid.SetValue(ptid, EventBehavior.Force);


				printerPort = adChkType.PrinterPath.Value;
				this.dfPrintId.SetValue(printerPort);	// #140791
				SetTlChkPrinterPtid((int)ptid, CheckType); //#12073
			}
			else
			{
				printerPort = cmbPrintName.CodeValue.ToString();	// WI#29221
			}
			// end #140791

			this.dfPrintId.SetValue(printerPort);	// WI#29221

			#region Deleted Code #140791
			//foreach (TlChkPrinter Printer in TellerVars.Instance.TlChkPrinterArray)
			//{
			//    if (Printer.Ptid.Value == ptid)
			//    {
			//        TellerVars.Instance.SetPrinterContextObject(ptid);
			//        if (_tlTransactionSet != null)    // #79510, #09460
			//            _tlTransactionSet.PrinterPtid.SetValue(ptid, EventBehavior.Force);

			//        printerPort = Printer.ChkPrintId.Value;
			//        SetTlChkPrinterPtid((int)ptid, CheckType); //#12073
			//        break;
			//    }
			//}
			#endregion
		}

		private bool PrinterIsDefinedOnWorkstation(string printerPort)	// #10157
		{
			bool Result = false;

			foreach (NetworkPrinter NPrinter in NetworkPrinterList.NetworkPrinters)
			{
			//WI#14280
				if (NPrinter.PrinterName.ToUpper() == printerPort.ToUpper())
				{
					Result = true;
					break;
				}
			}

			return Result;
		}

		private void cmbPrintName_SelectedIndexChanged(object sender, EventArgs e)
		{
			decimal PrinterPtid = 0;
			string PrinterPort = null;
            string CheckType = null;

			if (! _IsLoading)
			{
				decimal.TryParse(this.cmbPrintName.CodeValue.ToString().Split('~')[0], out PrinterPtid);	// #140791
                switch (_TranCode)
                {
                    case 902:
                        CheckType = "CashiersCheck";
                        break;

                    case 910:
                        CheckType = "MoneyOrder";
                        break;
                }

				//if (PrinterPtid > 0)	// WI#29221 remove wrapper
				//{
				SetPrinter(PrinterPtid, out PrinterPort ,  CheckType);

				if (!string.IsNullOrEmpty(PrinterPort))
				{
					this.dfPrintId.SetValue(PrinterPort);
				}
			}
		}

		/// <summary>
		/// Gets the tl_chk_printer_ptid from tl_drawer
		/// </summary>
		/// <returns></returns>
		private int GetTlChkPrinterPtid(string CheckType)   //#12073
		{
			int ReturnValue = int.MinValue;

			TlDrawer Drawer = new TlDrawer();
			Drawer.SelectAllFields = false;
			Drawer.TlChkPrinterPtid.Selected = true;
            Drawer.TlMOPrinterPtid.Selected = true;

            //Begin #79510, #09460
            if (_shBrPtrSetUpType > 0 || isAcquirerTran )
            {
                Drawer.ShBrCcPrtPtid.Selected = true;
                Drawer.ShBrMoPrtPtid.Selected = true;
            }
            //End #79510, #09460

			Drawer.DrawerNo.Value = TellerVars.Instance.DrawerNo;
			Drawer.BranchNo.Value = TellerVars.Instance.BranchNo;

			Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, Drawer);

            if (CheckType == "TLC600")								// #140791 - change to match chktype
                ReturnValue = Drawer.TlChkPrinterPtid.Value;		//#12073-This is Cashier's Check
			else if (CheckType == "TLC605")							// #140791 - change to match chktype
                ReturnValue = Drawer.TlMOPrinterPtid.Value;         //#12073 -This is MO Check

            //Begin #79510, #09460
            if (_shBrPtrSetUpType > 0)
            {
                ReturnValue = _shBrPtrSetUpType == 1 ? Drawer.ShBrCcPrtPtid.Value : Drawer.ShBrMoPrtPtid.Value;
            }
            else if (isAcquirerTran)
            {
                ReturnValue = _TranCode == 902 ? Drawer.ShBrCcPrtPtid.Value : Drawer.ShBrMoPrtPtid.Value;
            }
            //End #79510, #09460

			return ReturnValue;
		}

		/// <summary>
		/// Sets the tl_chk_printer_ptid for the current drawer
		/// </summary>
		/// <param name="ptid"></param>
        private void SetTlChkPrinterPtid(int ptid, string CheckType)        //#12073
		{
            //Begin #79510, #09460
            if (isAcquirerTran)
                return;

            //End #79510, #09460
            TlDrawer Drawer = new TlDrawer();
			Drawer.DrawerNo.Value = TellerVars.Instance.DrawerNo;
			Drawer.BranchNo.Value = TellerVars.Instance.BranchNo;
            //Begin #79510, #09460
            if (_shBrPtrSetUpType > 0)
            {
                if ( _shBrPtrSetUpType == 1 )
                    Drawer.ShBrCcPrtPtid.Value = ptid;
                else
                    Drawer.ShBrMoPrtPtid.Value = ptid;
            }
            //End #79510, #09460
            else
            {
                if (CheckType == "CashiersCheck")
                    Drawer.TlChkPrinterPtid.Value = ptid;       //12073-This is Cashier's Check
                else if (CheckType == "MoneyOrder")
                    Drawer.TlMOPrinterPtid.Value = ptid;         //12073-This is MO Check
            }

			Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Update, Drawer);
		}

		#endregion

	}
}
