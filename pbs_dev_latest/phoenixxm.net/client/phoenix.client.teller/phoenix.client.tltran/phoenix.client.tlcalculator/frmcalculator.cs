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
// File Name: 
// NameSpace: 
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//01/02/07		2		vreddy		#71444 - F10 is setting Foicus to MDI because return true was not added on key override
//May-24-07		3   	Muthu       #72780 VS2005 Migration 
//06/14/2007    4       mselvaga    #72059 - Add option for 10-key calculator function.
//08/26/2008    5       SDhamija    Gap#137 - Assume Decimals in Teller Calculator
//05/12/2011    6       FOyebola    WI #13944
//08/06/2012    7       Mkrishna    #19058 - Adding call to base on initParameters.
//02/15/2013    8       rpoddar     #19234 - Pass data back to only those windows which are coded to handle the calculator data
//8/3/2013		9		apitava		#157637 Uses new xfsprinter
//08/28/2018    10      AshishBbau  Task#99223 added F10 functionality of calculator in dlgTlCashCount Window   
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Shared.Xfs;
using Phoenix.BusObj.Admin.Teller;

namespace Phoenix.Client.TlCalculator
{
	/// <summary>
	/// Summary description for frmCalculator.
	/// </summary>
	public class frmCalculator : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbTapeandDisplay;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbKeepTape;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbRememberCalcPosition;
		private Phoenix.Windows.Forms.PCheckBoxStandard  cbSaveTapeInJournal;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbKeys;
		private Phoenix.Windows.Forms.PCheckBoxStandard cbDisplayCount;
		private Phoenix.Windows.Forms.PButtonStandard pbBackspace;
		private Phoenix.Windows.Forms.PButtonStandard pbReturn;
		private Phoenix.Windows.Forms.PButtonStandard pbClearAll;
		private Phoenix.Windows.Forms.PButtonStandard pbClear;
		private Phoenix.Windows.Forms.PButtonStandard pb7;
		private Phoenix.Windows.Forms.PButtonStandard pb8;
		private Phoenix.Windows.Forms.PButtonStandard pb9;
		private Phoenix.Windows.Forms.PButtonStandard pbSubtract;
		private Phoenix.Windows.Forms.PButtonStandard pbClose;
		private Phoenix.Windows.Forms.PButtonStandard pb4;
		private Phoenix.Windows.Forms.PButtonStandard pb5;
		private Phoenix.Windows.Forms.PButtonStandard pbAdd;
		private Phoenix.Windows.Forms.PButtonStandard pbSubtotal;
		private Phoenix.Windows.Forms.PButtonStandard pb1;
		private Phoenix.Windows.Forms.PButtonStandard pb2;
		private Phoenix.Windows.Forms.PButtonStandard pb3;
		private Phoenix.Windows.Forms.PButtonStandard pbMultiply;
		private Phoenix.Windows.Forms.PButtonStandard pbEquals;
		private Phoenix.Windows.Forms.PButtonStandard pb0;
		private Phoenix.Windows.Forms.PButtonStandard pbDecimalPoint;
		private Phoenix.Windows.Forms.PButtonStandard pbDivide;
		private Phoenix.Windows.Forms.PdfStandard dfDisplayTop;
		private Phoenix.Windows.Forms.PdfStandard dfTape;
		private Phoenix.Windows.Forms.PdfStandard dfDisplayBottom;
		private Phoenix.Windows.Forms.PButtonStandard pb6;
		private Phoenix.Windows.Forms.PButtonStandard pbPrint;

		private	string tape = null;
		private string prevOper = null;
		private string prevAddOper = null;
		private decimal prevNo = 0;
		private int subSetItemCount = 0;
		private int totalItemCount = 0;
		private decimal subSetItemTotal = 0;
		private decimal totalItemTotal = 0;
		private bool subTotalled = false;
		private TlJournal _tlJournal = null;
		private TlJournalCalc _tlJournalCalc = null;
		private Phoenix.Windows.Forms.PLabelStandard lblHelp;
		private decimal lastAmt = 0;
		private int lastItemCount = 0;
		private bool savingTape = false;
		private PfwStandard _activeWindow;
		private PMdiForm _mdiHandle;
		private bool returnClicked = false;
		private DialogResult dialogResult = DialogResult.None;
        private PCheckBoxStandard cbTenKeyFunction;		
		private PSmallInt _reprintFormId;
        private string _windowsHandlingCalcReturn = null;       // #19234
        #region WI-3475
        private PDecimal _noCopies;
        private PString _printerService;
        private string _wosaServiceName;
        #endregion
		public frmCalculator()
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
            this.gbTapeandDisplay = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbTenKeyFunction = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbDisplayCount = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbSaveTapeInJournal = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbRememberCalcPosition = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cbKeepTape = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.gbKeys = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pbDivide = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbDecimalPoint = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb0 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbEquals = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbMultiply = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb3 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb2 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb1 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbSubtotal = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbAdd = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb6 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb5 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb4 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbClose = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbSubtract = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb9 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb8 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pb7 = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbReturn = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbClearAll = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbClear = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbBackspace = new Phoenix.Windows.Forms.PButtonStandard();
            this.lblHelp = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDisplayTop = new Phoenix.Windows.Forms.PdfStandard();
            this.dfTape = new Phoenix.Windows.Forms.PdfStandard();
            this.dfDisplayBottom = new Phoenix.Windows.Forms.PdfStandard();
            this.pbPrint = new Phoenix.Windows.Forms.PButtonStandard();
            this.gbTapeandDisplay.SuspendLayout();
            this.gbKeys.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbTapeandDisplay
            // 
            this.gbTapeandDisplay.Controls.Add(this.cbTenKeyFunction);
            this.gbTapeandDisplay.Controls.Add(this.cbDisplayCount);
            this.gbTapeandDisplay.Controls.Add(this.cbSaveTapeInJournal);
            this.gbTapeandDisplay.Controls.Add(this.cbRememberCalcPosition);
            this.gbTapeandDisplay.Controls.Add(this.cbKeepTape);
            this.gbTapeandDisplay.Location = new System.Drawing.Point(4, 4);
            this.gbTapeandDisplay.Name = "gbTapeandDisplay";
            this.gbTapeandDisplay.PhoenixUIControl.ObjectId = 1;
            this.gbTapeandDisplay.Size = new System.Drawing.Size(184, 126);
            this.gbTapeandDisplay.TabIndex = 0;
            this.gbTapeandDisplay.TabStop = false;
            this.gbTapeandDisplay.Text = "Tape and Display";
            // 
            // cbTenKeyFunction
            // 
            this.cbTenKeyFunction.CodeValue = null;
            this.cbTenKeyFunction.Location = new System.Drawing.Point(6, 104);
            this.cbTenKeyFunction.Name = "cbTenKeyFunction";
            this.cbTenKeyFunction.PhoenixUIControl.ObjectId = 32;
            this.cbTenKeyFunction.Size = new System.Drawing.Size(174, 18);
            this.cbTenKeyFunction.TabIndex = 4;
            this.cbTenKeyFunction.Text = "10-Key &Function";
            // 
            // cbDisplayCount
            // 
            this.cbDisplayCount.CodeValue = null;
            this.cbDisplayCount.Location = new System.Drawing.Point(6, 58);
            this.cbDisplayCount.Name = "cbDisplayCount";
            this.cbDisplayCount.PhoenixUIControl.ObjectId = 31;
            this.cbDisplayCount.Size = new System.Drawing.Size(108, 18);
            this.cbDisplayCount.TabIndex = 2;
            this.cbDisplayCount.Text = "&Display Count";
            // 
            // cbSaveTapeInJournal
            // 
            this.cbSaveTapeInJournal.CodeValue = null;
            this.cbSaveTapeInJournal.Location = new System.Drawing.Point(6, 81);
            this.cbSaveTapeInJournal.Name = "cbSaveTapeInJournal";
            this.cbSaveTapeInJournal.PhoenixUIControl.ObjectId = 29;
            this.cbSaveTapeInJournal.Size = new System.Drawing.Size(174, 18);
            this.cbSaveTapeInJournal.TabIndex = 3;
            this.cbSaveTapeInJournal.Text = "&Save Tape in Journal on Return";
            // 
            // cbRememberCalcPosition
            // 
            this.cbRememberCalcPosition.CodeValue = null;
            this.cbRememberCalcPosition.Location = new System.Drawing.Point(6, 12);
            this.cbRememberCalcPosition.Name = "cbRememberCalcPosition";
            this.cbRememberCalcPosition.PhoenixUIControl.ObjectId = 28;
            this.cbRememberCalcPosition.Size = new System.Drawing.Size(144, 18);
            this.cbRememberCalcPosition.TabIndex = 0;
            this.cbRememberCalcPosition.Text = "&Remember Position";
            // 
            // cbKeepTape
            // 
            this.cbKeepTape.CodeValue = null;
            this.cbKeepTape.Location = new System.Drawing.Point(6, 35);
            this.cbKeepTape.Name = "cbKeepTape";
            this.cbKeepTape.PhoenixUIControl.ObjectId = 2;
            this.cbKeepTape.Size = new System.Drawing.Size(81, 18);
            this.cbKeepTape.TabIndex = 1;
            this.cbKeepTape.Text = "&Keep Tape";
            // 
            // gbKeys
            // 
            this.gbKeys.Controls.Add(this.pbDivide);
            this.gbKeys.Controls.Add(this.pbDecimalPoint);
            this.gbKeys.Controls.Add(this.pb0);
            this.gbKeys.Controls.Add(this.pbEquals);
            this.gbKeys.Controls.Add(this.pbMultiply);
            this.gbKeys.Controls.Add(this.pb3);
            this.gbKeys.Controls.Add(this.pb2);
            this.gbKeys.Controls.Add(this.pb1);
            this.gbKeys.Controls.Add(this.pbSubtotal);
            this.gbKeys.Controls.Add(this.pbAdd);
            this.gbKeys.Controls.Add(this.pb6);
            this.gbKeys.Controls.Add(this.pb5);
            this.gbKeys.Controls.Add(this.pb4);
            this.gbKeys.Controls.Add(this.pbClose);
            this.gbKeys.Controls.Add(this.pbSubtract);
            this.gbKeys.Controls.Add(this.pb9);
            this.gbKeys.Controls.Add(this.pb8);
            this.gbKeys.Controls.Add(this.pb7);
            this.gbKeys.Controls.Add(this.pbReturn);
            this.gbKeys.Controls.Add(this.pbClearAll);
            this.gbKeys.Controls.Add(this.pbClear);
            this.gbKeys.Controls.Add(this.pbBackspace);
            this.gbKeys.Location = new System.Drawing.Point(4, 129);
            this.gbKeys.Name = "gbKeys";
            this.gbKeys.PhoenixUIControl.ObjectId = 5;
            this.gbKeys.Size = new System.Drawing.Size(184, 156);
            this.gbKeys.TabIndex = 1;
            this.gbKeys.TabStop = false;
            this.gbKeys.Text = "Keys";
            // 
            // pbDivide
            // 
            this.pbDivide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbDivide.Location = new System.Drawing.Point(108, 128);
            this.pbDivide.Name = "pbDivide";
            this.pbDivide.PhoenixUIControl.ObjectId = 20;
            this.pbDivide.Size = new System.Drawing.Size(32, 23);
            this.pbDivide.TabIndex = 21;
            this.pbDivide.Text = "÷";
            this.pbDivide.Click += new System.EventHandler(this.pbOperator_Click);
            // 
            // pbDecimalPoint
            // 
            this.pbDecimalPoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbDecimalPoint.Location = new System.Drawing.Point(68, 128);
            this.pbDecimalPoint.Name = "pbDecimalPoint";
            this.pbDecimalPoint.PhoenixUIControl.ObjectId = 16;
            this.pbDecimalPoint.Size = new System.Drawing.Size(32, 23);
            this.pbDecimalPoint.TabIndex = 20;
            this.pbDecimalPoint.Text = ".";
            this.pbDecimalPoint.Click += new System.EventHandler(this.pbDecimalPoint_Click);
            // 
            // pb0
            // 
            this.pb0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb0.Location = new System.Drawing.Point(4, 128);
            this.pb0.Name = "pb0";
            this.pb0.PhoenixUIControl.ObjectId = 15;
            this.pb0.Size = new System.Drawing.Size(64, 23);
            this.pb0.TabIndex = 19;
            this.pb0.Text = "0";
            this.pb0.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pbEquals
            // 
            this.pbEquals.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbEquals.Location = new System.Drawing.Point(148, 99);
            this.pbEquals.Name = "pbEquals";
            this.pbEquals.PhoenixUIControl.ObjectId = 24;
            this.pbEquals.Size = new System.Drawing.Size(32, 52);
            this.pbEquals.TabIndex = 18;
            this.pbEquals.Text = "=";
            this.pbEquals.Click += new System.EventHandler(this.pbEquals_Click);
            // 
            // pbMultiply
            // 
            this.pbMultiply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbMultiply.Location = new System.Drawing.Point(108, 100);
            this.pbMultiply.Name = "pbMultiply";
            this.pbMultiply.PhoenixUIControl.ObjectId = 19;
            this.pbMultiply.Size = new System.Drawing.Size(32, 23);
            this.pbMultiply.TabIndex = 17;
            this.pbMultiply.Text = "x";
            this.pbMultiply.Click += new System.EventHandler(this.pbOperator_Click);
            // 
            // pb3
            // 
            this.pb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb3.Location = new System.Drawing.Point(68, 100);
            this.pb3.Name = "pb3";
            this.pb3.PhoenixUIControl.ObjectId = 14;
            this.pb3.Size = new System.Drawing.Size(32, 23);
            this.pb3.TabIndex = 16;
            this.pb3.Text = "3";
            this.pb3.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb2
            // 
            this.pb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb2.Location = new System.Drawing.Point(36, 100);
            this.pb2.Name = "pb2";
            this.pb2.PhoenixUIControl.ObjectId = 13;
            this.pb2.Size = new System.Drawing.Size(32, 23);
            this.pb2.TabIndex = 15;
            this.pb2.Text = "2";
            this.pb2.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb1
            // 
            this.pb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb1.Location = new System.Drawing.Point(4, 100);
            this.pb1.Name = "pb1";
            this.pb1.PhoenixUIControl.ObjectId = 12;
            this.pb1.Size = new System.Drawing.Size(32, 23);
            this.pb1.TabIndex = 14;
            this.pb1.Text = "1";
            this.pb1.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pbSubtotal
            // 
            this.pbSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbSubtotal.Location = new System.Drawing.Point(148, 72);
            this.pbSubtotal.Name = "pbSubtotal";
            this.pbSubtotal.PhoenixUIControl.ObjectId = 30;
            this.pbSubtotal.Size = new System.Drawing.Size(32, 23);
            this.pbSubtotal.TabIndex = 13;
            this.pbSubtotal.Text = "&ST";
            this.pbSubtotal.Click += new System.EventHandler(this.pbEquals_Click);
            // 
            // pbAdd
            // 
            this.pbAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbAdd.Location = new System.Drawing.Point(108, 72);
            this.pbAdd.Name = "pbAdd";
            this.pbAdd.PhoenixUIControl.ObjectId = 18;
            this.pbAdd.Size = new System.Drawing.Size(32, 23);
            this.pbAdd.TabIndex = 12;
            this.pbAdd.Text = "+";
            this.pbAdd.Click += new System.EventHandler(this.pbOperator_Click);
            // 
            // pb6
            // 
            this.pb6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb6.Location = new System.Drawing.Point(68, 72);
            this.pb6.Name = "pb6";
            this.pb6.PhoenixUIControl.ObjectId = 11;
            this.pb6.Size = new System.Drawing.Size(32, 23);
            this.pb6.TabIndex = 11;
            this.pb6.Text = "6";
            this.pb6.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb5
            // 
            this.pb5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb5.Location = new System.Drawing.Point(36, 72);
            this.pb5.Name = "pb5";
            this.pb5.PhoenixUIControl.ObjectId = 10;
            this.pb5.Size = new System.Drawing.Size(32, 23);
            this.pb5.TabIndex = 10;
            this.pb5.Text = "5";
            this.pb5.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb4
            // 
            this.pb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb4.Location = new System.Drawing.Point(4, 72);
            this.pb4.Name = "pb4";
            this.pb4.PhoenixUIControl.ObjectId = 9;
            this.pb4.Size = new System.Drawing.Size(32, 23);
            this.pb4.TabIndex = 9;
            this.pb4.Text = "4";
            this.pb4.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pbClose
            // 
            this.pbClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.pbClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbClose.Location = new System.Drawing.Point(148, 44);
            this.pbClose.Name = "pbClose";
            this.pbClose.PhoenixUIControl.ObjectId = 26;
            this.pbClose.Size = new System.Drawing.Size(32, 23);
            this.pbClose.TabIndex = 8;
            this.pbClose.Text = "&Off";
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // pbSubtract
            // 
            this.pbSubtract.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbSubtract.Location = new System.Drawing.Point(108, 44);
            this.pbSubtract.Name = "pbSubtract";
            this.pbSubtract.PhoenixUIControl.ObjectId = 17;
            this.pbSubtract.Size = new System.Drawing.Size(32, 23);
            this.pbSubtract.TabIndex = 7;
            this.pbSubtract.Text = "-";
            this.pbSubtract.Click += new System.EventHandler(this.pbOperator_Click);
            // 
            // pb9
            // 
            this.pb9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb9.Location = new System.Drawing.Point(68, 44);
            this.pb9.Name = "pb9";
            this.pb9.PhoenixUIControl.ObjectId = 8;
            this.pb9.Size = new System.Drawing.Size(32, 23);
            this.pb9.TabIndex = 6;
            this.pb9.Text = "9";
            this.pb9.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb8
            // 
            this.pb8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb8.Location = new System.Drawing.Point(36, 44);
            this.pb8.Name = "pb8";
            this.pb8.PhoenixUIControl.ObjectId = 7;
            this.pb8.Size = new System.Drawing.Size(32, 23);
            this.pb8.TabIndex = 5;
            this.pb8.Text = "8";
            this.pb8.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pb7
            // 
            this.pb7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pb7.Location = new System.Drawing.Point(4, 44);
            this.pb7.Name = "pb7";
            this.pb7.PhoenixUIControl.ObjectId = 6;
            this.pb7.Size = new System.Drawing.Size(32, 23);
            this.pb7.TabIndex = 4;
            this.pb7.Text = "7";
            this.pb7.Click += new System.EventHandler(this.pbNum_Click);
            // 
            // pbReturn
            // 
            this.pbReturn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbReturn.Location = new System.Drawing.Point(136, 16);
            this.pbReturn.Name = "pbReturn";
            this.pbReturn.PhoenixUIControl.ObjectId = 25;
            this.pbReturn.Size = new System.Drawing.Size(44, 23);
            this.pbReturn.TabIndex = 3;
            this.pbReturn.Text = "&Ret";
            this.pbReturn.Click += new System.EventHandler(this.pbReturn_Click);
            // 
            // pbClearAll
            // 
            this.pbClearAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbClearAll.Location = new System.Drawing.Point(92, 16);
            this.pbClearAll.Name = "pbClearAll";
            this.pbClearAll.PhoenixUIControl.ObjectId = 22;
            this.pbClearAll.Size = new System.Drawing.Size(44, 23);
            this.pbClearAll.TabIndex = 2;
            this.pbClearAll.Text = "&Clr All";
            this.pbClearAll.Click += new System.EventHandler(this.pbClear_Click);
            // 
            // pbClear
            // 
            this.pbClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbClear.Location = new System.Drawing.Point(48, 16);
            this.pbClear.Name = "pbClear";
            this.pbClear.PhoenixUIControl.ObjectId = 23;
            this.pbClear.Size = new System.Drawing.Size(44, 23);
            this.pbClear.TabIndex = 1;
            this.pbClear.Text = "C&lr";
            this.pbClear.Click += new System.EventHandler(this.pbClear_Click);
            // 
            // pbBackspace
            // 
            this.pbBackspace.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pbBackspace.Location = new System.Drawing.Point(4, 16);
            this.pbBackspace.Name = "pbBackspace";
            this.pbBackspace.PhoenixUIControl.ObjectId = 21;
            this.pbBackspace.Size = new System.Drawing.Size(44, 23);
            this.pbBackspace.TabIndex = 0;
            this.pbBackspace.Text = "&Bksp";
            this.pbBackspace.Click += new System.EventHandler(this.pbBackspace_Click);
            // 
            // lblHelp
            // 
            this.lblHelp.AutoEllipsis = true;
            this.lblHelp.Location = new System.Drawing.Point(12, 293);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.PhoenixUIControl.ObjectId = 27;
            this.lblHelp.Size = new System.Drawing.Size(252, 12);
            this.lblHelp.TabIndex = 2;
            this.lblHelp.Text = "Press F10 to return the amount";
            // 
            // dfDisplayTop
            // 
            this.dfDisplayTop.Location = new System.Drawing.Point(196, 8);
            this.dfDisplayTop.Name = "dfDisplayTop";
            this.dfDisplayTop.ReadOnly = true;
            this.dfDisplayTop.Size = new System.Drawing.Size(164, 20);
            this.dfDisplayTop.TabIndex = 3;
            this.dfDisplayTop.TabStop = false;
            this.dfDisplayTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dfDisplayTop.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dfFields_KeyPress);
            // 
            // dfTape
            // 
            this.dfTape.Location = new System.Drawing.Point(196, 36);
            this.dfTape.Multiline = true;
            this.dfTape.Name = "dfTape";
            this.dfTape.ReadOnly = true;
            this.dfTape.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dfTape.Size = new System.Drawing.Size(164, 221);
            this.dfTape.TabIndex = 4;
            this.dfTape.TabStop = false;
            this.dfTape.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dfTape.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dfFields_KeyPress);
            // 
            // dfDisplayBottom
            // 
            this.dfDisplayBottom.Location = new System.Drawing.Point(196, 265);
            this.dfDisplayBottom.Name = "dfDisplayBottom";
            this.dfDisplayBottom.ReadOnly = true;
            this.dfDisplayBottom.Size = new System.Drawing.Size(164, 20);
            this.dfDisplayBottom.TabIndex = 5;
            this.dfDisplayBottom.TabStop = false;
            this.dfDisplayBottom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.dfDisplayBottom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dfFields_KeyPress);
            // 
            // pbPrint
            // 
            this.pbPrint.Location = new System.Drawing.Point(284, 276);
            this.pbPrint.Name = "pbPrint";
            this.pbPrint.Size = new System.Drawing.Size(1, 1);
            this.pbPrint.TabIndex = 6;
            this.pbPrint.TabStop = false;
            this.pbPrint.Text = "&Print";
            this.pbPrint.Click += new System.EventHandler(this.pbPrint_Click);
            // 
            // frmCalculator
            // 
            this.AcceptButton = this.pbEquals;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.pbClose;
            this.ClientSize = new System.Drawing.Size(364, 311);
            this.Controls.Add(this.pbPrint);
            this.Controls.Add(this.dfDisplayBottom);
            this.Controls.Add(this.dfTape);
            this.Controls.Add(this.dfDisplayTop);
            this.Controls.Add(this.gbKeys);
            this.Controls.Add(this.gbTapeandDisplay);
            this.Controls.Add(this.lblHelp);
            this.MaximizeBox = false;
            this.Name = "frmCalculator";
            this.ScreenId = 10427;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmCalculator_PInitCompleteEvent);
            this.Closed += new System.EventHandler(this.frmCalculator_Closed);
            this.PMdiPrintEvent += new System.EventHandler(this.frmCalculator_PMdiPrintEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmCalculator_PInitBeginEvent);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmCalculator_Closing);
            this.gbTapeandDisplay.ResumeLayout(false);
            this.gbKeys.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region form related events
		public override void InitParameters(params object[] paramList)
		{
			if( paramList.Length > 0 )
				_activeWindow = paramList[0] as PfwStandard;

            base.InitParameters(paramList); //#19058

		}
		protected override bool ProcessDialogChar( char charCode )
		{
			ProcessKeys( charCode );
			return base.ProcessDialogChar( charCode );
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ( keyData == Keys.Enter )
			{
				pbEquals_Click( pbEquals, null );
				return true;
			}
			if ( keyData == Keys.F10 )
			{
				pbReturn_Click( pbReturn, null );
				return true;
			}
			return base.ProcessCmdKey( ref msg, keyData );
		}

		private Phoenix.Windows.Forms.ReturnType frmCalculator_PInitBeginEvent()
		{
			HandleRegSettings( false );
            _windowsHandlingCalcReturn = "frmTlTranCode~frmTlCapturedItems~dlgTlCashCount~";    // #19234 //Task#99223
            return new Phoenix.Windows.Forms.ReturnType();
		}

		private void frmCalculator_PInitCompleteEvent()
		{			
			if ( _mdiHandle != null && _mdiHandle.ActiveWksWindow.ContentWindow.CurrentWindow is PfwStandard )
				_activeWindow = _mdiHandle.ActiveWksWindow.ContentWindow.CurrentWindow as PfwStandard;
			FormBorderStyle = FormBorderStyle.FixedSingle;
		}

		private void frmCalculator_Closing(object sender, CancelEventArgs e)
		{
			pbClose_Click( sender, null );
		}

		private void frmCalculator_PMdiPrintEvent(object sender, EventArgs e)
		{
			HandlePrinting();
		}

		public DialogResult ShowDialog( PMdiForm mdiForm )
		{
			_mdiHandle = mdiForm;
			if( ! this.IsFormInitialized)
			{
				//this.LoadMLInfo( (Language) CoreService.AppSetting.ClientLanguage );
				this.InitializeForm();
			}	
			return (this as Form).ShowDialog( );
		}

		private void frmCalculator_Closed(object sender, EventArgs e)
		{
			Invalidate();
            if (returnClicked && _activeWindow != null &&
                _windowsHandlingCalcReturn != null && _windowsHandlingCalcReturn.IndexOf(_activeWindow.Name + "~") >= 0) // #19234
				_activeWindow.CallParent("Calculator",lastAmt,lastItemCount);
		}
		#endregion

		#region child control events
		private void pbNum_Click(object sender, EventArgs e)
		{
			ProcessKeys( Convert.ToChar( (sender as Control).Name.Substring(2,1)));
		}

		private void pbDecimalPoint_Click(object sender, EventArgs e)
		{
			ProcessKeys( Convert.ToChar( "."));
		}

		private void pbBackspace_Click(object sender, EventArgs e)
		{
			ProcessKeys( Convert.ToChar( 8 ) );
		}

		private void dfFields_KeyPress(object sender, KeyPressEventArgs e)
		{
			ProcessKeys( e.KeyChar );
		}

		private void pbClear_Click(object sender, EventArgs e)
		{
			if ( savingTape )
				return;
			dfDisplayTop.UnFormattedValue = null;
			if ( pbClearAll == ( sender as Control ) )
			{
				tape = null;
				dfTape.UnFormattedValue = tape;
				subSetItemCount = 0;
				totalItemCount = 0;
				subSetItemTotal = 0;
				totalItemTotal = 0;
				prevOper = null;
				prevNo = 0;
				subTotalled = false;
				dfDisplayBottom.UnFormattedValue = null;
				lastAmt = 0;
				lastItemCount = 0;

                // Begin #13944
                if (!cbTenKeyFunction.Checked)
                {
                    prevAddOper = null;
                }
                // End #13944
			}
		}

		private void pbOperator_Click(object sender, EventArgs e)
		{
			string operType = null;
			if ( pbAdd == (sender as Control ) )
				operType = "+";
			else if ( pbSubtract == (sender as Control ) )
				operType = "-";
			else if ( pbDivide == (sender as Control ) )
				operType = "/";
			else if ( pbMultiply == (sender as Control ) )
				operType = "*";

			ProcessOperator( operType );
		}
		private void pbClose_Click(object sender, EventArgs e)
		{
			if ( savingTape )
				return;
			HandleRegSettings( true );
			//this.Close( );
		}

		private void pbEquals_Click(object sender, EventArgs e)
		{
			bool showSubTotal = false;
			string itemCount = null;

			if ( savingTape )
				return;
			ProcessOperator("=");
			if (!subTotalled || pbSubtotal == (sender as Control ) )
			{
//				if ( pbSubtotal != (sender as Control ))
					subTotalled = true;
				showSubTotal = true;

				// the sub total is not exactly sub total but total only
				subSetItemCount = totalItemCount;
				subSetItemTotal = totalItemTotal;

			}

			if ( cbDisplayCount.Checked )
				itemCount = ( showSubTotal ?  subSetItemCount : totalItemCount).ToString().PadLeft(3,'0');

			tape = tape +string.Format(@"
{0}----------------------------
",itemCount);
			if ( showSubTotal )
				tape = tape + String.Format("{0:0.00}", subSetItemTotal);
			else
				tape = tape + String.Format("{0:0.00}", totalItemTotal);

			//tape = tape + ( showSubTotal ? Convert.ToChar(176).ToString() : "*" );
			tape = tape + " " + ( showSubTotal ? "s" : "=" ) + @"
";
			dfTape.UnFormattedValue = tape;
			ScrollTape();

			if ( showSubTotal )
			{
				subSetItemCount = 0;
				subSetItemTotal = 0;
			}
			else
			{
				lastAmt = decimal.Round(totalItemTotal,2);
				lastItemCount = totalItemCount;
				subSetItemCount = 0;
				totalItemCount = 0;
				subSetItemTotal = 0;
				totalItemTotal = 0;
			}
			prevOper = null;
			prevAddOper = null;
		}

		private void pbReturn_Click(object sender, EventArgs e)
		{
			if ( savingTape )
				return;
//			if ( lastAmt == 0 )
//			{
//				subTotalled = true;
//				pbEquals_Click( sender, e );
//
//			}
			if ( (dfDisplayTop.UnFormattedValue != null &&  dfDisplayTop.UnFormattedValue != String.Empty ) ||
				!subTotalled )
			{
				pbEquals_Click( sender, e );
			}
			if ( subTotalled && totalItemCount != 0 )
			{
				pbEquals_Click( sender, e );
			}

			if ( lastAmt >= (decimal)System.Math.Pow( 10, 12 ))
			{
				PMessageBox.Show( this, 360711, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
				return;
			}

			if ( cbSaveTapeInJournal.Checked )
			{
				if ( TellerVars.Instance.ClosedOut )
				{
					PMessageBox.Show( this, 360600, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
					cbSaveTapeInJournal.Focus();
					return;
				}
				if ( TellerVars.Instance.IsNoDrawerOptionSelected )
				{
					//You may not Save Tape in Journal on Return when you are not signed on to a teller drawer.
					PMessageBox.Show( this, 360734, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
					cbSaveTapeInJournal.Focus();
					return;
				}
				CallXMThruCDS( "SaveTape" );
			}
			returnClicked = true;
            this.Close();
            //pbClose_Click( sender, null );
		}
		#endregion

		#region other functions
		private void ProcessKeys( char charCode )
		{
			string displayTop = null;
			if ( savingTape )
				return;
			if ( "0123456789.".IndexOf( charCode.ToString()) >= 0 || charCode == 8 )
			{
				subTotalled = false;
				if ( dfDisplayTop.UnFormattedValue != null )
					displayTop = dfDisplayTop.UnFormattedValue.ToString();
				if ( charCode == 8 )
				{
					if ( displayTop != null && displayTop.Length >= 1 )
						displayTop = displayTop.Substring(0, displayTop.Length - 1 );
				}
				else
				{
					if ( charCode.ToString() == "." && displayTop != null && displayTop.IndexOf("." )>= 0 )
						return;
					//Begin Gap#137
					else if (displayTop != null && displayTop.IndexOf(".") > -1 && displayTop.Split('.')[1].Length >= 2)
						return;
					//End Gap#137
					displayTop = displayTop + charCode.ToString();
				}
				dfDisplayTop.UnFormattedValue = displayTop;
			}
			else if ( "+-*/".IndexOf( charCode.ToString()) >= 0 )
				ProcessOperator( charCode.ToString() );
			else if ( "=".IndexOf( charCode.ToString()) >= 0 )
				pbEquals_Click( pbEquals, null );
		}

		private void ProcessOperator( string operType )
		{
			decimal curNo = 0;
			string displayOper = null;
			
            string itemCount = null;

			if ( savingTape )
				return;
			if ( dfDisplayTop.UnFormattedValue == null || dfDisplayTop.UnFormattedValue.ToString() == String.Empty )
				return;

			if ( operType != "=" )
				subTotalled = false;

			//Begin Gap#137
			if (((AdTlControl)TellerVars.Instance.AdTlControl).AssumeDecimals.StringValue == "Y")
			{
				if (dfDisplayTop.UnFormattedValue.ToString().IndexOf(".") < 0)
					dfDisplayTop.UnFormattedValue = Convert.ToDecimal(dfDisplayTop.UnFormattedValue) / 100;
			}
			//End Gap#137
			
			try
			{
				//Begin Gap#137
				//curNo = Convert.ToDecimal( dfDisplayTop.UnFormattedValue );
				if (dfDisplayTop.UnFormattedValue.ToString() == ".")
					return;
				else
					curNo = Convert.ToDecimal(dfDisplayTop.UnFormattedValue);
				//End Gap#137
						
				if ( prevOper == "*" )
					curNo = prevNo * curNo;
				else if ( prevOper == "/" && curNo != 0 )
					curNo = prevNo / curNo;

				subSetItemCount = subSetItemCount + 1;
				totalItemCount = totalItemCount + 1;

//				if ( operType == "+"  )
//				{
//					subSetItemTotal = subSetItemTotal + curNo;
//					totalItemTotal = totalItemTotal + curNo;
//				}
//				else if ( operType == "-"  )
//				{
//					subSetItemTotal = subSetItemTotal - curNo;
//					totalItemTotal = totalItemTotal - curNo;
//				}

				if ( operType == "+"  || operType == "-" || operType == "=" )
				{
					if ( prevAddOper == "+" || prevAddOper == null )
					{
                        //#72059
                        if (cbTenKeyFunction.Checked)
                        {
                            if (prevAddOper == "+")
                            {
                                if (operType == "-")
                                {
                                    subSetItemTotal = subSetItemTotal + curNo * (-1);
                                    totalItemTotal = totalItemTotal + curNo * (-1);
                                }
                                else
                                {
                                    subSetItemTotal = subSetItemTotal + curNo;
                                    totalItemTotal = totalItemTotal + curNo;
                                }
                            }
                            else
                            {
                                if (operType == "+" || operType == "=")
                                {
                                    subSetItemTotal = subSetItemTotal + curNo;
                                    totalItemTotal = totalItemTotal + curNo;
                                }
                                else if (operType == "-")
                                {
                                    subSetItemTotal = subSetItemTotal - curNo;
                                    totalItemTotal = totalItemTotal - curNo;
                                }
                            }
                        }
                        else
                        {
                            subSetItemTotal = subSetItemTotal + curNo;
                            totalItemTotal = totalItemTotal + curNo;
                        }
					}
					else if ( prevAddOper == "-"  )
					{
                        //#72059
                        if (cbTenKeyFunction.Checked)
                        {
                            if (operType == "+")
                            {
                                subSetItemTotal = subSetItemTotal - curNo * (-1);
                                totalItemTotal = totalItemTotal - curNo * (-1);
                            }
                            else
                            {
                                subSetItemTotal = subSetItemTotal - curNo;
                                totalItemTotal = totalItemTotal - curNo;
                            }
                        }
                        else
                        {
                            subSetItemTotal = subSetItemTotal - curNo;
                            totalItemTotal = totalItemTotal - curNo;
                        }
					}
					prevAddOper = operType;
				}

				if ( tape != null && tape != String.Empty )
					tape = tape + @"
";
				displayOper = operType;
				if ( operType == "*" )
					displayOper = "x";
				else if ( operType == "/" )
					displayOper = "÷";
				else if ( operType == "=" )
					displayOper = " ";

				//Begin Gap#137
				//tape = tape + dfDisplayTop.UnFormattedValue.ToString() + " " + displayOper;
				tape = tape + String.Format("{0:0.00}", Convert.ToDecimal(dfDisplayTop.UnFormattedValue)) + " " + displayOper;
				//End Gap#137
				
				dfTape.UnFormattedValue = tape;
				//dfTape.ScrollToCaret();
//				dfTape.SelectionStart = dfTape.Text.Length;
//				dfTape.SelectionLength = 0;
				ScrollTape();
				if ( cbDisplayCount.Checked )
					itemCount = totalItemCount.ToString().PadLeft(3, '0');

				//Begin Gap#137
				//dfDisplayBottom.UnFormattedValue = itemCount +	"            " + decimal.Round(totalItemTotal,2).ToString();
				dfDisplayBottom.UnFormattedValue = itemCount + "            " + String.Format("{0:0.00}", totalItemTotal);
				//End Gap#137
				dfDisplayTop.UnFormattedValue = null;
			}
			finally
			{
				prevNo = curNo;
				prevOper = operType;
			}

		}



		private void HandleRegSettings( bool save )
		{
			string keyValue = null;
			string keyName = "settings";
			string location = ScreenId.ToString();
			string[] regValues = null;
			int screenSettings = 0;
			int xLoc = this.Location.X;
			int yLoc = this.Location.Y;

			#region read from registry
			if (!save)
			{
				if ( Helper.GetFromRegistry( location, keyName, ref keyValue ))
				{
					if ( keyValue != null && keyValue != String.Empty )
					{
						regValues = keyValue.Split(",".ToCharArray());
						if ( regValues.GetUpperBound(0) > 1 )
						{
							screenSettings = Convert.ToInt32( regValues[0]);
							xLoc = Convert.ToInt32( regValues[1]);
							yLoc = Convert.ToInt32( regValues[2]);
							cbRememberCalcPosition.Checked = ( screenSettings & 1 ) > 0;
							cbDisplayCount.Checked = ( screenSettings & 2 ) > 0;
							cbKeepTape.Checked = ( screenSettings & 4 ) > 0;
							cbSaveTapeInJournal.Checked = ( screenSettings & 8 ) > 0;
                            cbTenKeyFunction.Checked = (screenSettings & 16) > 0; //#72059
							if ( xLoc > 0 && yLoc > 0 && cbRememberCalcPosition.Checked )
							{
								Location = new Point( xLoc, yLoc );
							}
							else
							{
								StartPosition = FormStartPosition.CenterScreen;
							}
							if ( cbKeepTape.Checked )
							{
								tape = TellerVars.Instance.CalcData;
								dfTape.UnFormattedValue = tape;								
							}
						}
					}
				}
			}
			#endregion
			#region save to registry
			if (save)
			{
				screenSettings = ( cbRememberCalcPosition.Checked ? 1 : 0 );
				screenSettings = screenSettings |( cbDisplayCount.Checked ? 2 : 0 );
				screenSettings = screenSettings |( cbKeepTape.Checked ? 4 : 0 );
				screenSettings = screenSettings |( cbSaveTapeInJournal.Checked ? 8 : 0 );
                screenSettings = screenSettings | (cbTenKeyFunction.Checked ? 16 : 0); //#72059
				xLoc = this.Location.X;
				yLoc = this.Location.Y;
				keyValue = screenSettings.ToString() + "," + xLoc.ToString() + "," + yLoc.ToString();
				Helper.SaveToRegistry( location, keyName, keyValue );
				if ( cbKeepTape.Checked )
					TellerVars.Instance.CalcData = Convert.ToString( dfTape.UnFormattedValue );
				else
					TellerVars.Instance.CalcData = null;

			}
			#endregion
		}

		private void CallXMThruCDS(string origin )
		{
			try
			{
				if ( origin == "SaveTape" )
				{
//					dlgInformation.Instance.SetParent( this );
//					dlgInformation.Instance.ShowInfo( "Saving Tape..." );
					savingTape = true;
					_tlJournal = new TlJournal();
					_tlJournalCalc = new TlJournalCalc();
					_tlJournal.ActionType = XmActionType.New;
					_tlJournalCalc.ActionType = XmActionType.New;

					_tlJournal.EffectiveDt.Value = TellerVars.Instance.PostingDt;
					_tlJournal.BranchNo.Value = TellerVars.Instance.BranchNo;
					_tlJournal.DrawerNo.Value = TellerVars.Instance.DrawerNo;
					_tlJournal.EmplId.Value = TellerVars.Instance.EmployeeId;
					_tlJournal.NetAmt.Value = lastAmt;
					_tlJournal.ItemCount.Value = (short)lastItemCount;
					_tlJournal.TlTranCode.Value = "CLC";
					using ( new WaitCursor() )
					{
						Invalidate();
						Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _tlJournal );
						_tlJournalCalc.JournalPtid.Value = _tlJournal.Ptid.Value;
						_tlJournalCalc.CalcData.Value = tape;
						_tlJournalCalc.CopyBackStatus.Value = 1;
						Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _tlJournalCalc );
					}
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( this, pe, MessageBoxButtons.OK );
			}
			finally
			{
//				dlgInformation.Instance.HideInfo();
//				dlgInformation.Instance.SetParent( PApplication.Instance.MdiMain );
				savingTape = false;
			}
		}
		private void HandlePrinting( )
		{
			PrintInfo printInfo;
			AdTlForm adTlForm;

			_reprintFormId = new PSmallInt( "A1" );
			CallOtherForms( "AdHocReceipt" );
			if ( dialogResult != DialogResult.OK )
				return;

			if (!TellerVars.Instance.SetContextObject( "AdTlFormArray", _reprintFormId.Value ))
			{
				PMessageBox.Show( this, 360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
				return;
			}
			adTlForm = TellerVars.Instance.AdTlForm;
            _wosaServiceName = _printerService.Value;
			printInfo = new PrintInfo();
			printInfo.CalculatorTape = Convert.ToString( dfTape.UnFormattedValue );
			XfsPrinter xfsPrinter = new XfsPrinter(_wosaServiceName);  //#157637
			try
			{
				xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);  //#157637
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( this, pe, MessageBoxButtons.OK );
				return;
			}
			xfsPrinter.Close();	 //#157637
		}
		private void CallOtherForms( string origin )
		{
			PfwStandard tempDlg = null;

			try
			{
				if ( origin == "AdHocReceipt" )
				{
                    _reprintFormId = new PSmallInt("FormId");
                    _printerService = new PString("PrinterService");
                    _noCopies = new PDecimal("NoCopies", 1);
                    //
					tempDlg = Helper.CreateWindow( "phoenix.client.tlprinting","Phoenix.Client.TlPrinting","dlgAdHocReceipt");
					//tempDlg.InitParameters( null,_reprintFormId );
                    tempDlg.InitParameters(null, _reprintFormId, null, null, null, _noCopies, _printerService);
       
					dialogResult = tempDlg.ShowDialog(this);
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( this, pe, MessageBoxButtons.OK );
				return;
			}
//			catch( Exception e )
//			{
//				MessageBox.Show( e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop );
//				return;
//			}
		}
		#endregion

		private void pbPrint_Click(object sender, EventArgs e)
		{
			HandlePrinting();
		}

		private void ScrollTape( )
		{
			Control activeControl = this.ActiveControl;
			const int EM_LINESCROLL = 0xB6;
			int noOfLines = dfTape.Lines.GetUpperBound(0);

			dfTape.Focus();	
			NativeMethods.SendMessage( dfTape.Handle, EM_LINESCROLL, 0, ref noOfLines );
			if ( activeControl != null )
				activeControl.Focus();
		}


	}
}
