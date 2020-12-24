#region Comments
//-------------------------------------------------------------------------------
// File Name: ucCustomer.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
#endregion

#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//04/13/2018    2       FOyebola    Enh#220316-Task#87721
//04/16/2018    3       FOyebola    Enh#220316-Task#87721-2
//04/19/2018    4       DGarcia     Enh#220316-Task#87723
//04/24/2018    5       DGarcia     Bug#89210/Bug#89324
//04/24/2018    6       DGarcia     Task#88989-Mini RMS
//05/02/2018    7       DGarcia     Task#88992-Customer Alerts
//05/07/2018    8       CRoy        US #89309 - Task #89984
//05/14/2018    9       CRoy        US #89986 - Task #89987 - Add search again button and separator.
//05/11/2018    10       FOyebola    Enh#220316 - Task 87735
//05/11/2018    11      FOyebola    CVT#90899 - Dereference after null check
//05/17/2018    12      CRoy        US #88994 Task #88995 - Add Clear all feature.
//05/18/2018    13      CRoy        Bug #91539 - Show summary panel when we focus on search panel is lost.
//05/18/2018    14      CRoy        Bug #91538 - Fix Search Again button not showing when coming from search tab.
//05/22/2018    15      CRoy        Bug #91419 - Add focus on Search fields when select search again.
//05/23/2018    16      DGarcia     Task#89967 Account Alerts & Notifications
//05/25/2018    17      CRoy        Bug #91420 - Fix ctrl + enter key not being hit from every panel.
//05/25/2018    18      CRoy        Bug #92175 - Fix error when selecting customer from RMS window.
//06/05/2018    19      CRoy        Bug #92488 - Fix bug where search again did not work properly when search has multiple accounts.
//06/06/2018    20      CRoy        Bug #92023 - Item Capture should be disabled until a search is performed.
//06/08/2018    21      CRoy        Bug #93077 - RIM color is not being reinitialized properly.
//06/08/2018    22      DGarcia     Bug#93072
//06/21/2018    33      FOyebola    Enh#220316 - Task 87753
//07/02/2018    34      CRoy        Task 89995 - Transfer alert information.
//07/23/2018    35      DGarcia     Bug #96236/96237/96238
//07/25/2018    36      DGarcia     Task#87765 - Quick Acct Relationships
//07/30/2018    37      DGarcia     Bug#97017/97018
//08/03/2018    38      DGarcia     Task#89981 - Cross Ref Accounts
//08/17/2018    39      CRoy        Bug #98671 - Look and Feel
//08/23/2018    40      CRoy        Bug #99049 - Add tooltips
//09/05/2018    41      CRoy        Bug #100896 - Add search to recent customer lookup
//01/08/2019    42      CRoy        Bug #106554 - Fix tooltip for search button.
//01/10/2019    43      DGarcia     Bug # 108450
//01/14/2019    44      CRoy        Bug #100725
//02/05/2019    45      DGarcia     Bug #110043
//02/26/2019    46      CRoy        Task #111667
//03/26/2019    47      CRoy        Task #112775 - Format non personal rim properly.
//05/03/2019    48      CRoy        Task #114200 - Add focus depending on institution type and always enable search button.
//05/07/2019    49      CRoy        Bug #114298 - Fix panel not showing insution type properly when CU
//05/09/2019    50      CRoy        Bug #114526 - When default tab is Quick tran, proper field is not selected

#endregion

using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DevExpress.XtraBars.Docking;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Windows;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Variables;
using System.Drawing;
using Phoenix.Shared.Images; //Task #89984
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Phoenix.MultiTranTeller.Base;
using Phoenix.BusObj.Misc;

namespace Phoenix.Client.Teller
{
    public partial class ucCustomer : UserControl 
    {
        public event Action<bool> OnSearchPerformed = delegate { }; //Bug #92023
        CustomHeaderButton _iconSeparator; // Task #89987
        CustomHeaderButton _iconSearchAgain; // Task #89987
        CustomHeaderButton _btnRelationShip;
        PDxDockPanelUnderline _dockPanel = null;
        private QuickTranAlerts _alerts;
        private AcctAlerts _customerAlerts;
        private string _searchedCustomerName;
        ITellerWindow _parentWindow;
        private TellerVars _tellerVars; //Muthu- Moved it to constructor = TellerVars.Instance;
        public PDateTime dtNewCustDtLimit = new PDateTime("dtNewCustDtLimit");
        string _ParentInstitutionType; //Task#87721-2
        string _previousSearchedCustomer = ""; // Task #89987
        string _previousSearchedAccount = ""; // Task #89987
        int _currentRim = 0; //Bug #91419
        int _minimumWidth = 0;
        int _nbInitializations = 0; //Bug #114526
        Point _initalSecurityInfoPos = new Point();
        List<String> _currentAccounts = new List<string>();
        bool _previousSearchedFound = false; // Task #89987
        bool _searchHasAlerts = false; // Task #89987
        bool _searchHasMultipleAccounts = false; //Bug #92488
        Color _defaultNameColor; //Bug #93077
        Color _defaultReceiptMethodColor;
        CustomHeaderButton _btnInvisible;
        ToolTip searchAgainTooltip; //#100725

        public string rimStatus = string.Empty;  //87753


        public bool SearchAgainVisible { get { return _iconSearchAgain != null && _iconSearchAgain.Visible; } } //Bug #91420

        public ucCustomer()
        {
            InitializeComponent();
			//
			if( PDesignModeHelper.IsDesignMode == false )
			{
				_tellerVars = TellerVars.Instance; ////Muthu- Moved from variable decleration.  
			}
            //
            panelSearch.Dock = DockStyle.Fill;
            panelSummary.Dock = DockStyle.Fill;
            panelSummary.Visible = false;
            pbSearch.Image = AcProcImages.Search16; //Task #89984
            searchAgainTooltip = new ToolTip(); //Bug #99049
            searchAgainTooltip.SetToolTip(pbSearch, "Search  Enter"); //Bug #99049
            _defaultNameColor = dfName.ForeColor; //Bug #93077
            _defaultReceiptMethodColor = dfReceiptDelMethod.ForeColor;
            _minimumWidth = this.dfReceiptDelMethod.Location.X + dfReceiptDelMethod.Width;
            _initalSecurityInfoPos = lbSecurityInformation.Location;
            FocusProperField(); //Bug #114526
            _nbInitializations = 0; //Bug #114526
        }

        //Begin Bug #114526
        protected override void OnValidated(EventArgs e)
        {
            base.OnValidated(e);
            if (_nbInitializations <= 1)
            {
                FocusProperField();
            }
            _nbInitializations++;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible)
            {
                FocusProperField();
            }
        }
        //End Bug #114526

        private void SetFieldsBackgroundColor()
        {
            PdfStandard[] fields = new PdfStandard[] {
                dfName,
                dfUsualName,
                dfTIN,
                dfStatus,
                dfSecInfo,
                dfReceiptDelMethod
            };
            
            foreach(var field in fields)
            {
                field.BackColor =  ThemeHelper.WindowBgColor; //Color.FromArgb(235, 236, 239);
            }
        }


        public void InitializeTeller( ITellerWindow parentWindow)
        {
            _parentWindow = parentWindow;
            //
            _dockPanel = WinHelper.GetParentDockingControl(this) as PDxDockPanelUnderline;
            if(_dockPanel != null)
            {
                //Task#87721
                /*
                _btnRelationShip = new DevExpress.XtraBars.Docking.CustomHeaderButton("Relationship",ucImages.Images[2], -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
                
                _dockPanel.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] {_btnRelationShip,_btnSearch           });

                _btnRelationShip.Visible = false;
                 */
                _iconSearchAgain = new DevExpress.XtraBars.Docking.CustomHeaderButton("Search", AcProcImages.SearchAgain16, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "Search Again  Ctrl+Enter", false, -1, true, null, true, false, true, null, null, null, -1);

                //Begin Task #89987
                _iconSeparator = new CustomHeaderButton("", null, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, false, false, true, null, null, null, -1);
                _iconSeparator.Enabled = false;
                _iconSeparator.UseCaption = true;
                _iconSeparator.Caption = "|";
                //End Task #89987

                DisplaySearchAgainButton(false);
                _dockPanel.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _iconSearchAgain, _iconSeparator });// Task #89987
                _dockPanel.CustomButtonClick += DockPanel_CustomButtonClick;

                //CVT#90899
                SetDockPanelSearchType(); //Bug #98671
                _btnInvisible = WinHelper.GenerateInvisibleButton(16);
                _dockPanel.CustomHeaderButtons.Add(_btnInvisible);
            }

            /*
             *     
             *     this.panelCustomer.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] {
            new DevExpress.XtraBars.Docking.CustomHeaderButton("Search", ((System.Drawing.Image)(resources.GetObject("panelCustomer.CustomHeaderButtons"))), -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, serializableAppearanceObject1, null, null, -1),
            new DevExpress.XtraBars.Docking.CustomHeaderButton("Relationship", ((System.Drawing.Image)(resources.GetObject("panelCustomer.CustomHeaderButtons1"))), -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, serializableAppearanceObject2, null, null, -1)});
       
             * */

            //foreach (Control control in panelSearch.Controls)
            //{
            //    control.GotFocus += Control_GotFocus;
            //}

            FocusProperField(); //Task #114200

            SetFieldsBackgroundColor();
        }

        public bool IsSecurityInfoOffsetBy(Point point)
        {
            Point offsetedPoint = new Point(_initalSecurityInfoPos.X + point.X, _initalSecurityInfoPos.Y + point.Y);
            return offsetedPoint.X == lbSecurityInformation.Location.X && offsetedPoint.Y == lbSecurityInformation.Location.Y;
        }

        private void MoveInvisibleHeaderButtonToEnd()
        {
           if (_dockPanel != null)
            {
                if (_dockPanel.CustomHeaderButtons.Contains(_btnInvisible))
                {
                    _dockPanel.CustomHeaderButtons.Remove(_btnInvisible);
                    _dockPanel.CustomHeaderButtons.Add(_btnInvisible);
                }
            }
        }

        //Begin Bug #98671
        public void OffsetSecurityInfoAndDelMethod(Point offsetPointSecurityInfo,Point offsetPointReceipt)
        {
            this.lbSecurityInformation.Location = new Point(lbSecurityInformation.Location.X+offsetPointSecurityInfo.X, lbSecurityInformation.Location.Y + offsetPointSecurityInfo.Y);
            this.dfSecInfo.Location = new Point(dfSecInfo.Location.X+offsetPointSecurityInfo.X, dfSecInfo.Location.Y + offsetPointSecurityInfo.Y);
            this.lbReceiptDelMethod.Location = new Point(lbReceiptDelMethod.Location.X+ offsetPointReceipt.X, lbReceiptDelMethod.Location.Y + offsetPointReceipt.Y);
            this.dfReceiptDelMethod.Location = new Point(dfReceiptDelMethod.Location.X+ offsetPointReceipt.X, dfReceiptDelMethod.Location.Y + offsetPointReceipt.Y);
        }

        public void SetDockPanelSearchType()
        {
            _dockPanel.DrawUnderline = true;
            if (GlobalVars.InstitutionType == "CU") //Bug #114298
            {
                _dockPanel.Text = "Member Search"; //Task#87721-2
                _dockPanel.UnderlineLocation = new Point(50, 20);
            }
            else
            {
                _dockPanel.Text = "Customer Search"; //Task#87721-2
                _dockPanel.UnderlineLocation = new Point(57, 20);
            }
            _dockPanel.Update();
        }
        //End Bug #98671

        private void Control_GotFocus(object sender, EventArgs e)
        {
            _parentWindow.LastActiveControl = sender as Control;
        }

        //Begin Task#87721-2
        public void SetInstitutionType(string InstitutionType)
        {
            _ParentInstitutionType = InstitutionType;
            lbCustomerNo.Text = (_ParentInstitutionType == "CU" ? "Member Number:" : "Customer Number:");
            SetDockPanelSearchType();
            FocusProperField(); //Task #114200
        }
        //End Task#87721-2

        private void DockPanel_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
           if( e.Button == _btnRelationShip)
           {

           }
           else if ( e.Button == _iconSearchAgain )
            {
                SearchAgain();
            }
        }

        //public DataTable Customers { get; set ; }
        //public DataTable Accounts { get ; set; }

        public async Task<bool> PopulateCustomerRelations(int RimNo)
        {
            //_btnRelationShip.Visible = false; //Task#87721 
            //XmHelper c = new XmHelper();
            //var t = await c.GetRelatedCustomers(RimNo);
            var t = await GetRelatedCustomers(RimNo);
            this.leCustomerRelations.Properties.DataSource = t;
            this.lbRelations.Visible = false;          //Task#87721 
            this.leCustomerRelations.Visible = false;  //Task#87721 

            _parentWindow.CustomerRelations = t;
            this.panelSummary.Visible = true;
            //_btnRelationShip.Visible = true;  //Task#87721
            return true;
        }

        //public DataTable RelatedCustomers { get; set; }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Enter) && !_iconSearchAgain.Visible)
            {
                SearchCustomer();
                
                KeyEventArgs ka = new KeyEventArgs(keyData);
                ((PDxDockPanel)(_dockPanel)).OnPanelKeyDown(ka);

                return true;
            }       
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public int GetMinimumSize()
        {
            return _minimumWidth;
        }

        //Begin Task#Task 87723
        public async Task<DataTable> GetCustomerByAccount(string AcctNo)
        {
            var t = await AccountSearch(AcctNo);
            return t;
        }

        //Begin Task 89995
        public DataTable GetCustomerByAccountSync(string AcctNo, string acctType = null)
        {
            var t = AccountSearchDetailedSync(AcctNo, acctType);
            return t;
        }
        //End Task 89995

        //Begin Task#Task 87723
        public async Task<bool> PopulateCustomerInfo(int RimNo, bool addToHistory = true)
        {
            //_currentRim = RimNo; //Bug #91419 Bug#97017/97018
            _searchHasAlerts = false;
            InitializeAlerts(); //Task#88992

            //XmHelper c = new XmHelper();
            //var t = await c.CustomerSearch( RimNo);

            var t = await CustomerSearch(RimNo, addToHistory);
            _parentWindow.Customers = t;
            if( t.Rows.Count > 0)
            {
                DataRow row = t.Rows[0];

                Phoenix.BusObj.Global.GbHelper gbHelperBusObj = new GbHelper();
                //this.lblAddress.Text = ((string) row["Address"]+"").Replace("\\~", Environment.NewLine);
                //this.lblName.Text = (string)row["WindowTitle"];
                //_dockPanel.Text = (string) row["WindowTitle"];
                SetDockPanelSearchType();
                string rimType = row["RimType"] as string; //Task #111667
                if (rimType == "NonPersonal") //Task #111667
                {
                    //Begin Task #112775
                    string formattedRimName = "";
                    string rim = Convert.ToString(row["RimNo"]);
                    string title = Convert.ToString(row["Title"]);
                    string firstName = Convert.ToString(row["RimFirstName"]);
                    string lastName = Convert.ToString(row["RimLastName"]);
                    if (!string.IsNullOrEmpty(title))
                    {
                        formattedRimName = title;
                    }
                    if (!string.IsNullOrEmpty(firstName))
                    {
                        formattedRimName += " " + firstName;
                        formattedRimName = formattedRimName.Trim();
                    }
                    if (!string.IsNullOrEmpty(lastName))
                    {
                        formattedRimName += " " + lastName;
                        formattedRimName = formattedRimName.Trim();
                    }

                    _searchedCustomerName = rim + " - " + formattedRimName;
                    //End Task #112775
                }
                else
                {
                    _searchedCustomerName = (string)row["RimNo"] + " - " + (string)row["RimName"];
                }
                dfName.Text = _searchedCustomerName;

                dfUsualName.Text = Convert.ToString(row["UsualName"]); //Task #90004

                dfStatus.Text = Convert.ToString(row["RimStatus"]);
                if (dfStatus.Text != null)
                {
                    dfStatus.Text = dfStatus.Text.Trim();
                    rimStatus = dfStatus.Text;    //87753
                }

                dfSecInfo.Text = Convert.ToString(row["MothersMaidenName"]);

                if (t.Columns.Contains("TIN") && !string.IsNullOrEmpty(Convert.ToString(row["TIN"])))
                    dfTIN.Text = gbHelperBusObj.FormatTIN(Convert.ToString(row["TIN"]).Trim());
                else
                    dfTIN.Text = string.Empty;

                //Begin Task#89967
                if (t.Columns.Contains("RimPTID") && !string.IsNullOrEmpty(Convert.ToString(row["RimPTID"])))
                   _parentWindow.GetRmAcctPtid(Convert.ToInt32(row["RimPTID"]));
                //Begin Task#89967

                //_parentWindow.setTitleBar(_searchedCustomerName);

                _currentRim = Convert.ToInt32(row["RimNo"]); //Bug#97017/97018
            }
            //Begin Task#87721-2
            else
            {
                return false;
            }
            //End Task#87721-2

            this.panelSummary.Visible = true;
            DisplaySearchAgainButton(true);   //Bug #91398

            ShowAlerts(RimNo); //Task#88992

            pbSearch.Visible = false; //Task#87721-2

            return true;
        }

        //Begin Task#Task 87723
        public async Task<DataTable> AccountSearch(string acctNo)
        {
            BusObjectBase busObject = BusObjHelper.MakeClientObject("CUSTOMER_SEARCH");
            busObject.ActionType = XmActionType.ListView;

            FieldBase fieldAcctNo = busObject.GetFieldByXmlTag("AcctNo");
            FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            fieldAcctNo.Value = acctNo;
            fieldOutputType.Value = 35; //Task#89981

            var asyncCDS = CoreService.DataService.GetAsyncCDS();
            var response = await asyncCDS.GetListViewAsync(new System.Threading.CancellationToken(), busObject);

            return ResponseToDataTable(response, "CUSTOMER_SEARCH", true);
        }
        //End Task#Task 87723

        //Begin Task 89995
        public DataTable AccountSearchDetailedSync(string acctNo, string acctType = null)
        {
            var table = AccountSearchShortSync(acctNo, acctType);
            if (table.Rows.Count == 1)
            {
                return SearchListOfAccountsSync(table.Rows[0].GetStringValue("RimNo"));
            }
            else
            {
                return table;
            }
        }

        private DataTable SearchListOfAccountsSync(string rimNo)
        {
            BusObjectBase busObject = BusObjHelper.MakeClientObject("CUSTOMER_SEARCH");

            busObject.ActionType = XmActionType.ListView;

            FieldBase fieldRimNo = busObject.GetFieldByXmlTag("RimNo");
            FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            FieldBase fieldDepLoan = busObject.GetFieldByXmlTag("DepLoan");
            FieldBase fieldIncludeClosed = busObject.GetFieldByXmlTag("IncludeClosed");
            FieldBase fieldRequestMode = busObject.GetFieldByXmlTag("RequestMode");
            FieldBase fieldShowAcctNickname = busObject.GetFieldByXmlTag("ShowAcctNickname");
            FieldBase fieldShowTitles = busObject.GetFieldByXmlTag("ShowTitles");  //Task#87721

            //
            fieldRimNo.Value = rimNo;
            fieldOutputType.Value = 34;    //Task#87721: was 3 
            fieldDepLoan.Value = "All";    //Task#87721: was ALL 
            fieldIncludeClosed.Value = "N";
            fieldRequestMode.Value = "P";
            fieldShowAcctNickname.Value = "Y";
            fieldShowTitles.Value = "Y";    //Task#87721
            //
            var response = CoreService.DataService.GetListView(busObject);
            //
            return ucCustomer.ResponseToDataTable(response, "CUSTOMER_SEARCH", true);
        }

        private DataTable AccountSearchShortSync(string acctNo,string acctType = null)
        {
            BusObjectBase busObject = BusObjHelper.MakeClientObject("CUSTOMER_SEARCH");
            busObject.ActionType = XmActionType.ListView;

            if (!string.IsNullOrEmpty(acctType))
            {
                FieldBase fieldAcctType = busObject.GetFieldByXmlTag("AcctType");
                fieldAcctType.Value = acctType;
            }
            FieldBase fieldAcctNo = busObject.GetFieldByXmlTag("AcctNo");
            FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            fieldAcctNo.Value = acctNo;
            fieldOutputType.Value = 24;

            var response = CoreService.DataService.GetListView(busObject);

            return ResponseToDataTable(response, "CUSTOMER_SEARCH", true);
        }
        //End Task 89995

        public async Task<DataTable> CustomerSearch(int customerNo,bool addToHistory = true)
        {
            _searchHasAlerts = false; // Task #89987
            DataTable table = new DataTable("resultRow");
            BusObjectBase busObject = BusObjHelper.MakeClientObject("CUSTOMER_SEARCH");

            busObject.ActionType = XmActionType.Select;

            FieldBase fieldRimNo = busObject.GetFieldByXmlTag("RimNo");
            FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            fieldRimNo.Value = customerNo;
            fieldOutputType.Value = 2;
          
            var asyncCDS = CoreService.DataService.GetAsyncCDS();
            await asyncCDS.ProcessRequestAsync(new System.Threading.CancellationToken(), busObject);

            //Begin Bug #100896
            string rimStatus = busObject.GetFieldValue("RimStatus");
            if (addToHistory && !string.IsNullOrEmpty(rimStatus))
            {
                var customerSearch = new CustomerSearch();
                customerSearch.IntRmEmplAccess(customerNo, false);
            }
            //End Bug #100896

            //Begin Task#88992
            #region set customer alerts
            _customerAlerts = new AcctAlerts();
            _customerAlerts.RimNo = customerNo;

            if (busObject.OtherInfo.Contains("CustRegD"))
            {
                if (Convert.ToString(busObject.OtherInfo["CustRegD"].Value) == "Y")
                {
                    _searchHasAlerts = true; // Task #89987
                    _customerAlerts.CustRegD = true;
                }
            }
            if (busObject.OtherInfo.Contains("CustBankrupt"))
            {
                if (Convert.ToString(busObject.OtherInfo["CustBankrupt"].Value) == "Y")
                {
                    _searchHasAlerts = true; // Task #89987
                    _customerAlerts.CustBankrupt = true;
                }
            }
            if (busObject.OtherInfo.Contains("CustRelPkg"))
            {
                if (Convert.ToString(busObject.OtherInfo["CustRelPkg"].Value) == "Y")
                {
                    _searchHasAlerts = true; // Task #89987
                    _customerAlerts.CustRelPkg = true;
                }
            }
            if (busObject.OtherInfo.Contains("CustNotesCount"))
            {
                _customerAlerts.CustNotesCount = Convert.ToInt32(busObject.OtherInfo["CustNotesCount"].Value);
            }

            FieldBase fieldInvolvedInFraud = busObject.GetFieldByXmlTag("InvolvedInFraud");
            FieldBase fieldCrossRef = busObject.GetFieldByXmlTag("CrossRef");
            FieldBase fieldHouseholdMember = busObject.GetFieldByXmlTag("HouseholdMember");

            if (fieldInvolvedInFraud != null && Convert.ToString(fieldInvolvedInFraud.Value) == "Y")
            {
                _searchHasAlerts = true; // Task #89987
                _customerAlerts.Fraud = true;
            }
            if (fieldCrossRef != null && Convert.ToString(fieldCrossRef.Value) == "Y")
            {
                _searchHasAlerts = true; // Task #89987
                _customerAlerts.CustCrossRef = true;
            }
            if (fieldHouseholdMember != null && Convert.ToString(fieldHouseholdMember.Value) == "Y")
            {
                _searchHasAlerts = true; // Task #89987
                _customerAlerts.HouseHold = true;
            }

            #endregion
            //End Task#88992

            //Begin Task#88989
            #region Task#88989
            dfName.ForeColor = _defaultNameColor; //Bug #93077
            dfReceiptDelMethod.ForeColor = _defaultReceiptMethodColor;

            if (busObject.OtherInfo.Contains("NewCustDtLimit"))
            {
                dtNewCustDtLimit.Value = busObject.OtherInfo["NewCustDtLimit"].DateTimeValue;
                if (!dtNewCustDtLimit.IsNull && DateTime.Compare(dtNewCustDtLimit.DateTimeValue, GlobalVars.SystemDate) > 0)
                    dfName.ForeColor = System.Drawing.Color.Green;
            }

            FieldBase fieldProhibNewAcct = busObject.GetFieldByXmlTag("ProhibNewAcct");
            if (fieldProhibNewAcct != null && fieldProhibNewAcct.StringValue == "Y")
                dfName.ForeColor = System.Drawing.Color.Red;

            FieldBase fieldReceiptDelMethod = busObject.GetFieldByXmlTag("ReceiptDelMethod");
            if (_tellerVars.AdTlControl.EnableEreceipt.Value == "Y" && !fieldReceiptDelMethod.IsNull) //&& !_tlTranSet.CurTran.TranAcct.ReceiptDelMethod.IsNull)  //#40223
            {
                string receiptDelMethod = fieldReceiptDelMethod.StringValue;

                dfReceiptDelMethod.Text = receiptDelMethod;
               
                if (receiptDelMethod == "Email")
                    dfReceiptDelMethod.ForeColor = Color.Green;
                else if (receiptDelMethod == "Print")
                    dfReceiptDelMethod.ForeColor = Color.Red;
                //else if (receiptDelMethod == "Print and Email")
                    //dfReceiptDelMethod.ForeColor = Color.Black;
            }
            else
            {
                lbReceiptDelMethod.Visible = false;
                dfReceiptDelMethod.Visible = false;
            }
            #endregion
            //End Task#88989
           
            var dataRow = table.NewRow();
            foreach (var field in busObject.CalcOnlyFields)
            {
                var col = table.Columns.Add(field.XmlTag);
                dataRow[field.XmlTag] = field.Value;
            }

            foreach (var field in busObject.DbFields)
            {
                if (!table.Columns.Contains(field.XmlTag))
                {
                    var col = table.Columns.Add(field.XmlTag);
                    dataRow[field.XmlTag] = field.Value;
                }
            }

            //Begin Task#87721-2
            if (dataRow["RimName"].GetType().FullName != "System.DBNull")
            {
                table.Rows.Add(dataRow);
            }
            //End Task#87721-2
      

            return table;

        }

        public async Task<DataTable> GetListofRelatedCustomer(string acctNo, string acctType)
        {

            BusObjectBase busObject = BusObjHelper.MakeClientObject("GB_MAP_ACCT_REL");

            busObject.ActionType = XmActionType.ListView;

            FieldBase fieldAcctType = busObject.GetFieldByXmlTag("AcctType");
            FieldBase fieldAcctNo = busObject.GetFieldByXmlTag("AcctNo");
            FieldBase fieldAcctId = busObject.GetFieldByXmlTag("AcctId");

            //FieldBase fieldOutputType = busObject.GetFieldByXmlTag("OutputType");
            fieldAcctType.Value = acctType;
            fieldAcctNo.Value = acctNo;
            fieldAcctId.Value = 0;
            //fieldOutputType.Value = 16;
            busObject.ResponseTypeId = 16;

            var asyncCDS = CoreService.DataService.GetAsyncCDS();
            var response = await asyncCDS.GetListViewAsync(new System.Threading.CancellationToken(), busObject);

            return ResponseToDataTable(response, "GB_MAP_ACCT_REL", true);

        }

        public async Task<DataTable> GetRelatedCustomers(int customerNo)
        {
            BusObjectBase busObject = BusObjHelper.MakeClientObject("RM_REL");

            busObject.ActionType = XmActionType.ListView;

            FieldBase fieldRimNo = busObject.GetFieldByXmlTag("RimNo");
            fieldRimNo.Value = customerNo;
                        
            //fieldOutputType.Value = 16;
            busObject.ResponseTypeId = 10;


            var asyncCDS = CoreService.DataService.GetAsyncCDS();
            var response = await asyncCDS.GetListViewAsync(new System.Threading.CancellationToken(), busObject);

            return ResponseToDataTable(response, "RM_REL", true);


        }

        internal static DataTable ResponseToDataTable(XmlNode xapiResponse, string bizObjName, bool isListView)
        {
            //string xPath = $"//{bizObjName}";
            string xPath = string.Format("//{0}", bizObjName);

            if (isListView)
            {
                xPath = xPath + "/RECORD";
            }

            var results = xapiResponse.SelectNodes(xPath);

            DataTable table = new DataTable("resultRow");
            if (results.Count == 0)
                return table;
            var firstNode = results[0];
            foreach (XmlElement child in firstNode.ChildNodes)
            {
                if ( !table.Columns.Contains(child.Name))
                    table.Columns.Add(child.Name);
            }

            foreach (XmlElement record in results)
            {
                var dataRow = table.NewRow();
                foreach (XmlElement child in record.ChildNodes)
                {
                    string colName = child.Name;
                    dataRow[colName] = child.InnerText;
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
        private void pbSearch_Click(object sender, EventArgs e)
        {
            SearchCustomer(); 
        }

        //Begin Bug #91539
        private void pbSearch_Leave(object sender, EventArgs e)
        {
            if (_previousSearchedFound && !IsFocusedOnSearch())
            {
                ShowSummaryPanel();
                DisplaySearchAgainButton(true);
            }
        }
        //End Bug #91539

        public async void SearchCustomer()
        {

            var response = new CustomerInfoResponse();
            //Begin Task#Task 87723
            int rimNo ; 
            string acctNo;
            //End Task#Task 87723

            //_btnRelationShip.Visible = false;   //Task#87721

            //Begin Task#Task 87723
            if (dfAccountNo.Text.Length == 0 && dfCustomerNo.Text.Length == 0)
            {
                PMessageBox.Show(360481, MessageType.Warning, MessageBoxButtons.OK, "1");
                return;
            }

            this.panelSearch.Visible = false;

            //If nothing has changed and the previous search was not empty, don't search again.
            //Begin Task #89987
            if (!_searchHasMultipleAccounts && IsCurrentSearchAlreadyDisplayed()) //Bug #91538
            {
                if (_previousSearchedFound)
                {
                    DisplaySearchAgainButton(true);
                    ShowSummaryPanel();
                    ShowAlerts(_currentRim); //Bug #91419
                }
                else
                {
                    HandleNoFoundRecord(false);
                }
                return;
            }
            //End Task #89987

            if (dfAccountNo.Text.Length > 0)
            {
                _previousSearchedAccount = dfAccountNo.Text;// Task #89987
                acctNo = dfAccountNo.Text;
                response = await _parentWindow.PopulateCustomerInfo(acctNo);
                //_currentRim = response.RimNo; //Bug #92489 //Bug#97017/97018
            }
            else
            {
                _previousSearchedCustomer = dfCustomerNo.Text;// Task #89987
                //Bug 110043
                if (int.TryParse(dfCustomerNo.Text, out rimNo))
                {
                    response = await _parentWindow.PopulateCustomerInfo(rimNo);
                }
                else
                {
                    HandleNoFoundRecord(false);
                    return;
                }

                //_parentWindow.GetDisplayHistoryInfo("RIM", dfCustomerNo.Text); //Task#94607/94608//Bug # 108450
                //_currentRim = rimNo; //Bug #92489 //Bug#97017/97018
            }
            //Begin Bug#89210


            if (response.UserCanceled) //Bug #96236/96237/96238
            {
                if (_previousSearchedFound || IsCurrentSearchAlreadyDisplayed())
                {
                    DisplaySearchAgainButton(true);
                    ShowSummaryPanel();
                    ShowAlerts(_currentRim);
                    
                }
                else
                {
                    HandleNoFoundRecord(true);
                }

                return;
            }

            _currentAccounts = FormatAccountNumbers(response.AccountNumbers); //Bug #92489
            _searchHasMultipleAccounts = response.RowCount > 0; //Bug #92488
            _previousSearchedFound = response.Success;
          

            if (!response.Success)
            {
                HandleNoFoundRecord(false);
                return;
            }
            
            // Begin Task #89987
            DisplaySearchAgainButton(true);
            // End Task #89987
            //End Bug#89210 

            //End Task#Task 87723
            return;
        }

        void SetDockPanelTitleAfterSearch()
        {
            if (_ParentInstitutionType == "CU")
            {
                _dockPanel.Text = "Member"; //Task#87721-2
                _dockPanel.UnderlineLocation = new Point(50, 20);
            }
            else
            {
                _dockPanel.Text = "Customer"; //Task#87721-2
                _dockPanel.UnderlineLocation = new Point(57, 20);
            }
        }

        //Begin Bug #92489

        private List<string> FormatAccountNumbers(List<string> accountNumbers)
        {
            List<string> formattedAccountNumbers = new List<string>();
            foreach (var accountNumber in accountNumbers)
            {
                int acctNumber = TranHelper.Instance.FormatAcctNo(accountNumber);;
                if (acctNumber != -1)
                {
                    formattedAccountNumbers.Add(acctNumber.ToString());
                }
                else
                {
                    formattedAccountNumbers.Add(accountNumber.Replace("-", ""));
                }
            }
            return formattedAccountNumbers;
        }

        private bool IsCurrentSearchAlreadyDisplayed()
        {
            bool searchAlreadyDisplayed = false;

            if (!string.IsNullOrEmpty(dfCustomerNo.Text))
            {
                int customerNo;
                if (int.TryParse(dfCustomerNo.Text, out customerNo))
                {
                    searchAlreadyDisplayed = customerNo == _currentRim;
                }
            }
            else if (!string.IsNullOrEmpty(dfAccountNo.Text))
            {
                searchAlreadyDisplayed = _currentAccounts.Contains(dfAccountNo.Text);
            }
                
            return searchAlreadyDisplayed;
        }

        //End Bug #92489

        //Begin Task #89987
        public void SearchAgain()
        {
            panelSummary.Visible = !panelSummary.Visible;
            panelSearch.Visible = !panelSummary.Visible;
            pbSearch.Visible = !panelSummary.Visible; //Task#87721
            if (panelSearch.Visible)
            {
                SetDockPanelSearchType();
            }
            else
            {
                _dockPanel.DrawUnderline = false;
                _dockPanel.Text = _searchedCustomerName;
            }
            DisplaySearchAgainButton(false);
            FocusLastSearchedControl(); //Bug #91419
        }

        //Begin Task #114200
        public void FocusProperField()
        {
            if (!FocusLastSearchedControl())
            {
                if (GlobalVars.InstitutionType == "CU")
                {
                    dfCustomerNo.Focus();
                }
                else
                {
                    dfAccountNo.Focus();
                }
            }
        }
        //End Task #114200

        //Begin Bug #91419
        private bool FocusLastSearchedControl()
        {
            bool focusedField = false;
            if (!string.IsNullOrEmpty(dfCustomerNo.Text))
            {
                focusedField = true;
                dfCustomerNo.Focus();
                dfCustomerNo.SelectAll();
            }
            else if (!string.IsNullOrEmpty(dfAccountNo.Text))
            {
                focusedField = true;
                dfAccountNo.Focus();
                dfAccountNo.SelectAll();
            }
            return focusedField;
        }
        //End Bug #91419

        //End Task #89987

        //Begin Bug#89210 
        public void HandleNoFoundRecord(bool userCanceled)
        {
            if (!userCanceled)
                PMessageBox.Show(310984, MessageType.Message, MessageBoxButtons.OK, string.Empty);

            _parentWindow.ClearInformation();
            ShowSearchPanel();
            pbSearch.Visible = true;
            _currentRim = 0;
            _currentAccounts.Clear(); //Bug #92489
            FocusLastSearchedControl();

            return;
        }
        //End Bug#89210 

        #region Search Events
        //Begin Task#Task 87723
        private void dfCustomerNo_PhoenixUILeaveEvent(object sender, PEventArgs e)
        {
            if (this.dfCustomerNo.Text.Length > 0)
            {
                dfAccountNo.Text = "";
            }
        }

        private void dfAccountNo_PhoenixUILeaveEvent(object sender, PEventArgs e)
        {
            if (this.dfAccountNo.Text.Length > 0)
            {
                dfCustomerNo.Text = "";
            }
        }
        private void dfCustomerNo_TextChanged(object sender, EventArgs e)
        {
            if (this.dfCustomerNo.Text.Length > 0)
            {
                //dfCustomerNo.Focus();
                //dfCustomerNo.SelectAll();
                dfAccountNo.Text = "";
                _previousSearchedAccount = "";
            }
        }

        //Begin Bug #91539

        private void dfCustomerNo_Leave(object sender, EventArgs e)
        {
            if (_previousSearchedFound && !IsFocusedOnSearch())
            {
                ShowSummaryPanel();
                DisplaySearchAgainButton(true);
            }
        }

        //End Bug #91539

        private void dfAccountNo_TextChanged(object sender, EventArgs e)
        {
            if (this.dfAccountNo.Text.Length > 0)
            {
                //dfAccountNo.Focus();
                //dfAccountNo.SelectAll();
                dfCustomerNo.Text = "";
                _previousSearchedCustomer = "";
            }
        }
        //End Task#Task 87723
        #endregion

        //Begin Bug #91539

        private void dfAccountNo_Leave(object sender, EventArgs e)
        {
            if (_previousSearchedFound && !IsFocusedOnSearch())
            {
                ShowSummaryPanel();
                DisplaySearchAgainButton(true);
            }
        }

        //End Bug #91539



        private void InitializeAlerts()
        {
            if (_alerts != null)    // already initialized 
                return;

            _alerts = new QuickTranAlerts();
            _alerts.Add(AlertsNames.CustCrossRef, null);
            _alerts.Add(AlertsNames.HouseHold, null);
            _alerts.Add(AlertsNames.CustNotes, null);
            _alerts.Add(AlertsNames.Fraud, null);
            _alerts.Add(AlertsNames.CustRegD, null);
            _alerts.Add(AlertsNames.CustBankrupt, null);
            _alerts.Add(AlertsNames.CustRelPkg, null);

            _alerts.Initialize(_parentWindow.TlTranCodeWindow as PfwStandard, _dockPanel, MoveInvisibleHeaderButtonToEnd);
        }

        public void ShowAlerts(int rimNo)
        {
            if ( _alerts != null && _customerAlerts != null )   //Task#87721: Commented until the alerts story is coded
                _alerts.ShowAcctAlerts(_customerAlerts);  
            //Task#87721: Commented until the alerts story is coded
        }

        public void HideAlerts()
        {
            if (_alerts != null && _customerAlerts != null)   //Task#87721: Commented until the alerts story is coded
                _alerts.HideAcctAlerts(_customerAlerts);
        }

        public int CurrentRimNo {  get { return _currentRim; } }

        public void ResetCustomerInfo()
        {
            _currentAccounts.Clear(); //Bug #92489
            _currentRim = 0; //Bug #91419
            _previousSearchedCustomer = "";
            _previousSearchedAccount = "";
            _previousSearchedFound = false;
            HideAlerts(); //Task #88995
            ShowSearchPanel();
            SetDockPanelSearchType();
            dfCustomerNo.ResetText();
            dfAccountNo.ResetText();
            dfTIN.ResetText();
            dfSecInfo.ResetText();
            dfStatus.ResetText();
            dfCustomerNo.UnFormattedValue = null;
            dfAccountNo.UnFormattedValue = null;
            return;
        }

        //Begin Bug #91538
        public void SetCustomerInformation(int rimNo,string account)
        {
            _currentAccounts = new List<string>();
            _currentRim = rimNo; //Bug #91419
            _previousSearchedCustomer = "";
            _previousSearchedAccount = "";
            _previousSearchedFound = false;

            if (!string.IsNullOrEmpty(account))
            {
                _currentAccounts = FormatAccountNumbers(new List<string>() { account }); //Bug #92489 
                account = Regex.Replace(account, "[^0-9.]", ""); //Bug #92175
                dfAccountNo.Text = _previousSearchedAccount = account;
                _previousSearchedCustomer = rimNo.ToString();
                _previousSearchedFound = true;
            }
            else if (rimNo != int.MinValue)
            {
                dfCustomerNo.Text = _previousSearchedCustomer = rimNo.ToString();
                _previousSearchedFound = true;
            }
        }
        //End Bug #91538

        public void ResetCustomerSearchFocus()
        {
            HideAlerts(); //Task #88995
            if (this.dfCustomerNo.Visible)
            {
                FocusProperField(); //Task #114200
                pbSearch.Visible = true; //Task 87735
            }
            DisplaySearchAgainButton(false);
            return;
        }

        //Begin Bug #91539

        private bool IsFocusedOnSearch()
        {
            bool searchFocused = false;
            if (this.ActiveControl != null)
            {
                searchFocused = this.ActiveControl == this.pbSearch || this.ActiveControl == this.dfAccountNo || this.ActiveControl == this.dfCustomerNo;
            }
            return searchFocused;
        }

        public void ShowSummaryPanel()
        {
            this.panelSearch.Visible = false;
            this.panelSummary.Visible = true;
        }

        //End Bug #91539

        private void ShowSearchPanel()
        {
            this.panelSearch.Visible = true;
            this.panelSummary.Visible = false;
        }

        private void DisplaySearchAgainButton(bool display)
        {
            if (display)
            {
                SetDockPanelTitleAfterSearch();
            }
            else
            {
                SetDockPanelSearchType();
            }
            _iconSearchAgain.Visible = display;
            _iconSeparator.Visible = display ? _searchHasAlerts : false;
        }

        public void Destroy() //Bug#93072
        {
            if (_alerts != null)
            {
                _alerts.Dispose();
            }
        }

    }
}
