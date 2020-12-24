#region Comments

//-------------------------------------------------------------------------------
// File Name: dlgAcctNicknames.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//
//08/28/2008    1       JStainth    Created
//08/28/2008    2       JStainth    #76041 - Accout Nicknames
//12/30/2008    3       mselvaga    #76458 - Added credit card changes.
//20May2009     4       Dfutcher    #3538 - Summary Criteria for Nickname
//28Aug2009     5       SDhamija    #79156 - added RimNo to ownership accounts window
//09/25/2009    6       mselvaga    #5700 - Balances are displayed when access has View access restricted in the account summary window that pops up after add next.
//03/22/2010    7       iezikeanyi  #6479 - Select was enabled even when window was not populated. Gave error when clicked
//05/14/2010    8       mselvaga    WI#8830 - UAT2 Wrong tab active when using Add Next
//05/09/2012    9       jabreu      17820 - Add confidential support
//10/05/2012    10      rpoddar     #19415 - Performance Fixes
//08/17/2020    11      Ivin        Task #130297 - Changing Ownership description to the Account relation description. Get account screen with Add Next doesn't show actual account ownership.
//---------------------------------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Variables;
using GlacialComponents.Controls;

namespace Phoenix.Windows.Client
{
	/// <summary>
	/// Summary description for dlgAcctNicknames.
	/// </summary>
	public class dlgAcctNicknames : Phoenix.Windows.Forms.PfwStandard
	{
		#region Private Fields
		private System.ComponentModel.Container components = null;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbAccountInformation;
        private TellerVars _tellerVars = TellerVars.Instance;
        private PGrid ctwAcctNicknames;
        private PGridColumn colAcctType;
        private PGridColumn colAcctNo;
        private PGridColumn colNickname;
        private PGridColumn colCurBal;
        private PGridColumn colOwnership;
        private PGroupBoxStandard gbTotals;
        private PLabelStandard lblCurrentBal;
        private PLabelStandard lblAccounts;
        private PDfDisplay dfAccounts;
        private PAction pbSelect;
        private PAction pbNew;
        private PAction pbClose;
		private AcctInfo _acctInfo = null;
        private PdfCurrency dfTotalBal;
		private ArrayList _acctList = new ArrayList();
        private PGridColumn colDescription;
        private PGridColumn colRealAcctNo;
        private PGridColumn colStatus;
        private PGroupBoxStandard pGroupBoxStandard1;
        private PRadioButtonStandard rbPrimary;
        private PRadioButtonStandard rbSecondary;
        private PCheckBoxStandard cbIncludeClosed;
        private PRadioButtonStandard rbAll;
        private Phoenix.BusObj.Global.GbHelper _gbHelper = new Phoenix.BusObj.Global.GbHelper();
        private string _relationship;
		private PGridColumn colRimNo; //#79156
        private string _allowClosed; //#3538
        private bool _isShowBalance = true; //#5700

        #endregion

        #region Constructor/Disposal
		public dlgAcctNicknames()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gbAccountInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.ctwAcctNicknames = new Phoenix.Windows.Forms.PGrid();
			this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
			this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colNickname = new Phoenix.Windows.Forms.PGridColumn();
			this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
			this.colCurBal = new Phoenix.Windows.Forms.PGridColumn();
			this.colOwnership = new Phoenix.Windows.Forms.PGridColumn();
			this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
			this.colRealAcctNo = new Phoenix.Windows.Forms.PGridColumn();
			this.gbTotals = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfTotalBal = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCurrentBal = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblAccounts = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfAccounts = new Phoenix.Windows.Forms.PDfDisplay();
			this.pbSelect = new Phoenix.Windows.Forms.PAction();
			this.pbNew = new Phoenix.Windows.Forms.PAction();
			this.pbClose = new Phoenix.Windows.Forms.PAction();
			this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.cbIncludeClosed = new Phoenix.Windows.Forms.PCheckBoxStandard();
			this.rbAll = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbSecondary = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbPrimary = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.gbAccountInformation.SuspendLayout();
			this.gbTotals.SuspendLayout();
			this.pGroupBoxStandard1.SuspendLayout();
			this.SuspendLayout();
			//
			// ActionManager
			//
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbClose,
            this.pbSelect,
            this.pbNew});
			//
			// gbAccountInformation
			//
			this.gbAccountInformation.Controls.Add(this.ctwAcctNicknames);
			this.gbAccountInformation.Location = new System.Drawing.Point(4, 52);
			this.gbAccountInformation.Name = "gbAccountInformation";
			this.gbAccountInformation.PhoenixUIControl.ObjectId = -1;
			this.gbAccountInformation.Size = new System.Drawing.Size(680, 338);
			this.gbAccountInformation.TabIndex = 0;
			this.gbAccountInformation.TabStop = false;
			//
			// ctwAcctNicknames
			//
			this.ctwAcctNicknames.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colRimNo,
            this.colAcctType,
            this.colAcctNo,
            this.colNickname,
            this.colDescription,
            this.colCurBal,
            this.colOwnership,
            this.colStatus,
            this.colRealAcctNo});
			this.ctwAcctNicknames.Location = new System.Drawing.Point(4, 16);
			this.ctwAcctNicknames.Name = "ctwAcctNicknames";
			this.ctwAcctNicknames.Size = new System.Drawing.Size(672, 317);
			this.ctwAcctNicknames.TabIndex = 0;
			this.ctwAcctNicknames.Text = "pGrid1";
//			this.ctwAcctNicknames.RowClicked += new Phoenix.Windows.Forms.GridClickedEventHandler(this.ctwAcctNicknames_RowClicked);
            this.ctwAcctNicknames.DoubleClick += new EventHandler(ctwAcctNicknames_DoubleClick);
			//
			// colRimNo
			//
			this.colRimNo.PhoenixUIControl.ObjectId = 20;
			this.colRimNo.Title = "Rim No";
			this.colRimNo.Width = 60;
			//
			// colAcctType
			//
			this.colAcctType.PhoenixUIControl.ObjectId = 6;
			this.colAcctType.PhoenixUIControl.XmlTag = "0";
			this.colAcctType.Title = "Acct Type";
			this.colAcctType.Width = 65;
			//
			// colAcctNo
			//
			this.colAcctNo.PhoenixUIControl.ObjectId = 7;
			this.colAcctNo.PhoenixUIControl.XmlTag = "00";
			this.colAcctNo.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
			this.colAcctNo.Title = "Acct Number";
			this.colAcctNo.Width = 90;
			//
			// colNickname
			//
			this.colNickname.PhoenixUIControl.ObjectId = 8;
			this.colNickname.PhoenixUIControl.XmlTag = "1";
			this.colNickname.Title = "Nickname";
			this.colNickname.Width = 197;
			//
			// colDescription
			//
			this.colDescription.PhoenixUIControl.ObjectId = 13;
			this.colDescription.PhoenixUIControl.XmlTag = "2";
			this.colDescription.Title = "Description";
			this.colDescription.Width = 197;
			//
			// colCurBal
			//
			this.colCurBal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCurBal.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCurBal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colCurBal.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colCurBal.PhoenixUIControl.ObjectId = 9;
			this.colCurBal.PhoenixUIControl.XmlTag = "20";
			this.colCurBal.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colCurBal.Title = "Current Bal";
			//
			// colOwnership
			//
			this.colOwnership.PhoenixUIControl.ObjectId = 10;
			this.colOwnership.PhoenixUIControl.XmlTag = "3";
			this.colOwnership.Title = "Ownership";
			this.colOwnership.Width = 60;
			//
			// colStatus
			//
			this.colStatus.PhoenixUIControl.ObjectId = 19;
			this.colStatus.PhoenixUIControl.XmlTag = "Status";
			this.colStatus.Title = "Status";
			this.colStatus.Width = 75;
			//
			// colRealAcctNo
			//
			this.colRealAcctNo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.colRealAcctNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.colRealAcctNo.PhoenixUIControl.XmlTag = "RealAcctNo";
			this.colRealAcctNo.Title = "colRealAcctNo";
			this.colRealAcctNo.Visible = false;
			this.colRealAcctNo.Width = 0;
			//
			// gbTotals
			//
			this.gbTotals.Controls.Add(this.dfTotalBal);
			this.gbTotals.Controls.Add(this.lblCurrentBal);
			this.gbTotals.Controls.Add(this.lblAccounts);
			this.gbTotals.Controls.Add(this.dfAccounts);
			this.gbTotals.Location = new System.Drawing.Point(4, 392);
			this.gbTotals.Name = "gbTotals";
			this.gbTotals.PhoenixUIControl.ObjectId = 1;
			this.gbTotals.Size = new System.Drawing.Size(680, 52);
			this.gbTotals.TabIndex = 12;
			this.gbTotals.TabStop = false;
			this.gbTotals.Text = "Totals";
			//
			// dfTotalBal
			//
			this.dfTotalBal.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfTotalBal.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfTotalBal.Location = new System.Drawing.Point(392, 24);
			this.dfTotalBal.Name = "dfTotalBal";
			this.dfTotalBal.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfTotalBal.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfTotalBal.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfTotalBal.PhoenixUIControl.MaxPrecision = 14;
			this.dfTotalBal.ReadOnly = true;
			this.dfTotalBal.Size = new System.Drawing.Size(104, 13);
			this.dfTotalBal.TabIndex = 4;
			this.dfTotalBal.TabStop = false;
			this.dfTotalBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			//
			// lblCurrentBal
			//
			this.lblCurrentBal.AutoEllipsis = true;
			this.lblCurrentBal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblCurrentBal.Location = new System.Drawing.Point(316, 20);
			this.lblCurrentBal.Name = "lblCurrentBal";
			this.lblCurrentBal.Size = new System.Drawing.Size(76, 20);
			this.lblCurrentBal.TabIndex = 3;
			this.lblCurrentBal.Text = "Current Bal:";
			//
			// lblAccounts
			//
			this.lblAccounts.AutoEllipsis = true;
			this.lblAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblAccounts.Location = new System.Drawing.Point(8, 20);
			this.lblAccounts.Name = "lblAccounts";
			this.lblAccounts.Size = new System.Drawing.Size(68, 20);
			this.lblAccounts.TabIndex = 2;
			this.lblAccounts.Text = "Accounts:";
			//
			// dfAccounts
			//
			this.dfAccounts.Location = new System.Drawing.Point(80, 24);
			this.dfAccounts.Name = "dfAccounts";
			this.dfAccounts.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfAccounts.Size = new System.Drawing.Size(100, 13);
			this.dfAccounts.TabIndex = 0;
			//
			// pbSelect
			//
			this.pbSelect.LongText = "&Select";
			this.pbSelect.ObjectId = 4;
			this.pbSelect.Shortcut = System.Windows.Forms.Keys.End;
			this.pbSelect.ShortText = "&Select";
			this.pbSelect.Tag = null;
			this.pbSelect.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSelect_Click);
			//
			// pbNew
			//
			this.pbNew.LongText = "&New";
			this.pbNew.ObjectId = 5;
			this.pbNew.ShortText = "&New";
			this.pbNew.Tag = null;
			this.pbNew.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbNew_Click);
			//
			// pbClose
			//
			this.pbClose.LongText = "&Close";
			this.pbClose.ObjectId = 11;
			this.pbClose.ShortText = "&Close";
			this.pbClose.Tag = null;
			this.pbClose.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClose_Click);
			//
			// pGroupBoxStandard1
			//
			this.pGroupBoxStandard1.Controls.Add(this.cbIncludeClosed);
			this.pGroupBoxStandard1.Controls.Add(this.rbAll);
			this.pGroupBoxStandard1.Controls.Add(this.rbSecondary);
			this.pGroupBoxStandard1.Controls.Add(this.rbPrimary);
			this.pGroupBoxStandard1.Location = new System.Drawing.Point(5, 4);
			this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
			this.pGroupBoxStandard1.PhoenixUIControl.ObjectId = 14;
			this.pGroupBoxStandard1.Size = new System.Drawing.Size(674, 47);
			this.pGroupBoxStandard1.TabIndex = 13;
			this.pGroupBoxStandard1.TabStop = false;
			this.pGroupBoxStandard1.Text = "Summary Criteria";
			//
			// cbIncludeClosed
			//
			this.cbIncludeClosed.AutoSize = true;
			this.cbIncludeClosed.CodeValue = null;
			this.cbIncludeClosed.Location = new System.Drawing.Point(255, 19);
			this.cbIncludeClosed.Name = "cbIncludeClosed";
			this.cbIncludeClosed.PhoenixUIControl.ObjectId = 18;
			this.cbIncludeClosed.Size = new System.Drawing.Size(102, 18);
			this.cbIncludeClosed.TabIndex = 3;
			this.cbIncludeClosed.Text = "Include Closed";
			this.cbIncludeClosed.CheckedChanged += new System.EventHandler(this.SummaryCriteria_CheckedChanged);
			//
			// rbAll
			//
			this.rbAll.AutoSize = true;
			this.rbAll.Description = null;
			this.rbAll.Location = new System.Drawing.Point(174, 19);
			this.rbAll.Name = "rbAll";
			this.rbAll.PhoenixUIControl.ObjectId = 17;
			this.rbAll.Size = new System.Drawing.Size(42, 18);
			this.rbAll.TabIndex = 2;
			this.rbAll.Text = "All";
			this.rbAll.CheckedChanged += new System.EventHandler(this.SummaryCriteria_CheckedChanged);
			//
			// rbSecondary
			//
			this.rbSecondary.AutoSize = true;
			this.rbSecondary.Description = null;
			this.rbSecondary.Location = new System.Drawing.Point(86, 19);
			this.rbSecondary.Name = "rbSecondary";
			this.rbSecondary.PhoenixUIControl.ObjectId = 16;
			this.rbSecondary.Size = new System.Drawing.Size(82, 18);
			this.rbSecondary.TabIndex = 1;
			this.rbSecondary.Text = "Secondary";
			this.rbSecondary.CheckedChanged += new System.EventHandler(this.SummaryCriteria_CheckedChanged);
			//
			// rbPrimary
			//
			this.rbPrimary.AutoSize = true;
			this.rbPrimary.Description = null;
			this.rbPrimary.IsMaster = true;
			this.rbPrimary.Location = new System.Drawing.Point(10, 19);
			this.rbPrimary.Name = "rbPrimary";
			this.rbPrimary.PhoenixUIControl.ObjectId = 15;
			this.rbPrimary.Size = new System.Drawing.Size(65, 18);
			this.rbPrimary.TabIndex = 0;
			this.rbPrimary.Text = "Primary";
			this.rbPrimary.CheckedChanged += new System.EventHandler(this.SummaryCriteria_CheckedChanged);
			//
			// dlgAcctNicknames
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.pGroupBoxStandard1);
			this.Controls.Add(this.gbTotals);
			this.Controls.Add(this.gbAccountInformation);
			this.Name = "dlgAcctNicknames";
			this.ScreenId = 26010;
			this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
			this.Text = "Ownership Account";
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgAcctNicknames_PInitCompleteEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgAcctNicknames_PInitBeginEvent);
			this.gbAccountInformation.ResumeLayout(false);
			this.gbTotals.ResumeLayout(false);
			this.gbTotals.PerformLayout();
			this.pGroupBoxStandard1.ResumeLayout(false);
			this.pGroupBoxStandard1.PerformLayout();
			this.ResumeLayout(false);

		}

		public override void InitParameters(params object[] paramList)
        {   //#3538 begin
            if (paramList.Length >= 4)
			{
			    _acctList = paramList[0] as ArrayList;
				_acctInfo = paramList[1] as AcctInfo;
                _relationship = paramList[2].ToString();       // All, Primary, Secondary
                _allowClosed = paramList[3].ToString();      // Y, N

                _relationship = (string.IsNullOrEmpty(_relationship)) ? "Primary" : _relationship;
                _allowClosed = (string.IsNullOrEmpty(_allowClosed)) ? "N" : _allowClosed;
                //#3538 end

                if (paramList.Length > 4)
                {
                    _isShowBalance = (Convert.ToUInt32(paramList[4]) == 1); //#5700
                }
			}

		}
		#endregion

		#region Events
		private Phoenix.Windows.Forms.ReturnType dlgAcctNicknames_PInitBeginEvent()
		{
			//this.AppToolBarId = ToolbarType.TB_Processing;
			//this.MainBusinesObject = _tlTranSet.CurTran;
            ctwAcctNicknames.DoubleClickAction = pbSelect;  //#8830
			return ReturnType.Success;

		}
		private void dlgAcctNicknames_PInitCompleteEvent()
		{
		    this.DefaultAction = this.pbClose;
			this.pbClose.Image = Images.Cancel;

            switch (_relationship)     //#3538
            {
                case "Primary":
                    rbPrimary.Checked = true;
                    break;
                case "Secondary":
                    rbSecondary.Checked = true;
                    break;
                default:
                    rbAll.Checked = true;
                    break;
            }
            if (_allowClosed != "Y") //#3538
            {
                cbIncludeClosed.SetObjectStatus(NullabilityState.Null, VisibilityState.Show, EnableState.Disable);
                cbIncludeClosed.Checked = false;
            }

            //if (_acctList.Count > 0) //#6479
            if (_acctList.Count > 0 && _acctList[0] != null && _acctList[0].ToString() != "") //#6479
			{
                ctwAcctNicknames.Items.Clear();

                PopulateGrid(); // #3538
                pbSelect.Enabled = true;
                this.DefaultAction = pbSelect; //#6479
			}
			else
			{
                pbSelect.Enabled = false;
			}

            if (ctwAcctNicknames.Count > 0)
                ctwAcctNicknames.SelectRow(0, true);

            //this.DefaultAction = pbSelect; //#6479

            colNickname.Visible = (_tellerVars.AdTlControl.DisplayNicknames.Value == "Y");    //#76041
            colDescription.Visible = !colNickname.Visible;
		}
        private void pbNew_Click(object sender, PActionEventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
            _acctInfo = new AcctInfo();
            this.Close();
        }
        private void pbClose_Click(object sender, PActionEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void pbSelect_Click(object sender, PActionEventArgs e)
        {
            _acctInfo.AcctNo = colAcctNo.UnFormattedValue.ToString();
            _acctInfo.AcctType = colAcctType.UnFormattedValue.ToString();
            //Begin #76458
            if (colRealAcctNo.UnFormattedValue != null) //External account
            {
                _acctInfo.RealAcctNo = colRealAcctNo.UnFormattedValue.ToString();
            }
            //End #76458
            if (colNickname.UnFormattedValue != null)
                _acctInfo.AcctNickname = colNickname.UnFormattedValue.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //private void ctwAcctNicknames_RowClicked(object sender, GridClickEventArgs e)
        //{
        //    _acctInfo.AcctNo = colAcctNo.UnFormattedValue.ToString();
        //    _acctInfo.AcctType = colAcctType.UnFormattedValue.ToString();
        //    //Begin #76458
        //    if (colRealAcctNo.UnFormattedValue != null) //External account
        //    {
        //        _acctInfo.RealAcctNo = colRealAcctNo.UnFormattedValue.ToString();
        //    }
        //    //End #76458
        //    if (colNickname.UnFormattedValue != null)
        //        _acctInfo.AcctNickname = colNickname.UnFormattedValue.ToString();

        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //}

        void ctwAcctNicknames_DoubleClick(object sender, EventArgs e)
        {
            _acctInfo.AcctNo = colAcctNo.UnFormattedValue.ToString();
            _acctInfo.AcctType = colAcctType.UnFormattedValue.ToString();
            //Begin #76458
            if (colRealAcctNo.UnFormattedValue != null) //External account
            {
                _acctInfo.RealAcctNo = colRealAcctNo.UnFormattedValue.ToString();
            }
            //End #76458
            if (colNickname.UnFormattedValue != null)
                _acctInfo.AcctNickname = colNickname.UnFormattedValue.ToString();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
		#endregion

        private void SummaryCriteria_CheckedChanged(object sender, EventArgs e)         //#3538
        {
            PopulateGrid();
        }

        private void PopulateGrid()             //#3538
        {
            bool containsConfidentialAccount = false; // 17820
            GLItem listItem = null;
            Decimal amountTotal = new decimal(0);
            Decimal amount = new decimal(0);
            Int32 accounts = 0;
            bool displayRow = false;
            string acctType,
                    acctNo,
                    nickname,
                    curBal,
                    relationship,
                    classDesc,
                    acctNoMask,
                    status,
                    relationshipType; //#130297 need to assign Primary/Secondary ownership values.
            int rimNo;

            string confidential = null;     // #19415

            ctwAcctNicknames.Items.Clear();

            for (int i = 0; i < _acctList.Count - 10; i = i + 11)  //#76458 + 7, #3538 + 8, #79156  , #19415 - changed counter from 9 to 10, #130297 - changed counter from 10 to 11. Added actual relationship desc from ad_rm_rel to select.
            {
                //Decode
                acctType = _acctList[i + 0].ToString();
                acctNo = _acctList[i + 1].ToString();
                nickname = _acctList[i + 2].ToString();
                curBal = _acctList[i + 3].ToString();
                relationshipType = _acctList[i + 4].ToString(); //#130297 assigning Primary/Secondary ownership values.
                classDesc = _acctList[i + 5].ToString();
                acctNoMask = _acctList[i + 6].ToString();
				status = _acctList[i + 7].ToString();
				rimNo = Convert.ToInt32(_acctList[i + 8]);		//#79156
                confidential = _acctList[i + 9].ToString();     // #19415
                relationship = _acctList[i + 10].ToString(); //#130297 changed existing ownership description to Relationship description.

                //Display Row
                displayRow = true;
                displayRow = displayRow && (rbAll.Checked || rbPrimary.Checked && relationshipType == "Primary" || rbSecondary.Checked && relationshipType == "Secondary") ;
                displayRow = displayRow && (status != "Closed" || status == "Closed" && cbIncludeClosed.Checked );

                if (displayRow)
                {
                    accounts++;

                    listItem = ctwAcctNicknames.Items[ctwAcctNicknames.AddNewRow()];

                    //Visible Columns
					listItem.SubItems.Add(rimNo.ToString());	//#79156
					listItem.SubItems.Add(acctType);
					if (string.IsNullOrEmpty(acctNoMask) == false)
                        listItem.SubItems.Add(acctNoMask);
                    else
                        listItem.SubItems.Add(acctNo);
                    if (string.IsNullOrEmpty(acctNo) == true || string.IsNullOrEmpty(nickname) == true)
                        listItem.SubItems.Add(classDesc);
                    else
                        listItem.SubItems.Add(nickname);
                    listItem.SubItems.Add(classDesc);
                    amount = Convert.ToDecimal(curBal);

                    /* Begin 17820 */

                    if (!(this.Extension as Phoenix.Shared.Windows.FormExtension).VerifyConfAcctAccess(colCurBal, acctType, acctNo, confidential))  // #19415 - passed confidential instead of null
                    {
                            listItem.SubItems.Add("Confidential");
                            containsConfidentialAccount = true;
                    }
                    else
                    {
                            listItem.SubItems.Add(CurrencyHelper.GetFormattedValue(amount));
                    }

                    /* End 17820 */

                    listItem.SubItems.Add(relationship);
                    listItem.SubItems.Add(status); //#3538

                    //Hidden Columns
                    listItem.SubItems.Add(acctNo); //#76458 RealAcctNo column to handle external account

                    amountTotal = amountTotal + amount;
                }
            } // for


            dfAccounts.UnFormattedValue = accounts;

            /* Begin 17820 */

            if (containsConfidentialAccount)
            {
                    dfTotalBal.Text = "Confidential";
            }
            else
            {
                    dfTotalBal.UnFormattedValue = amountTotal;
            }

            /* End 17820 */

            //
            if (!_isShowBalance)    //#5700
            {
                this.colCurBal.Visible = false;
                this.dfTotalBal.UnFormattedValue = 0;
            }

        }
	}
}
