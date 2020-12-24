#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//??/??/??		1		RPoddar		#????? - Created.
//05/09/2018    2       FOyebola    Enh#220316 - Task 87735
//05/23/2018    3       FOyebola    Enh#220316 - Task 87757
//06/04/2018    4       FOyebola    Enh#220316 - Task 87759
//06/08/2018    5       FOyebola    Enh#220316 - Task 87761
//06/15/2018    6       FOyebola    Enh#220316 - Task 87751
//07/25/2018    7       FOyebola    Enh#220316 - Bug 95598
//08/10/2018    8       CRoy        Task #90004
//08/21/2018    9       CRoy        Bug #99045
//09/07/2018    10      FOyebola    Enh#220316 - Bug 99054
//09/25/2018    11      CRoy        Bug 99053 - Add menu indicator
//01/09/2019    12      CRoy        Bug 106778 - Add menu change duplicated shortcut CtrlR to CtrlQ
//06/06/2019    13      DGarcia     Task# 115070 New Multi Tran Hotkeys for Transactions
//11/13/2019    14      mselvaga    Task#120877 - 240921 - 2019 SP5 - 8 - DEV - Porting of all the bugs into 2019 - Fixed the Tran type menu order and shotcuts for LN/RM.


#endregion


using DevExpress.Utils.Menu;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using Phoenix.MultiTranTeller.Base;
using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix.Client.Teller
{

    public class TransactionsMenu : GridViewMenu
    {
        int _rowHandle;
        ITellerWindow _parentWindow;
        private ucAccounts ucAcctsCaller = null;
        private ucTransactions ucTransCaller = null;  //87757
        private Action onAfterMenuItemClick = null;

        public TransactionsMenu(DevExpress.XtraGrid.Views.Grid.GridView view, int rowHandle, ITellerWindow _parentWindow, ucAccounts ucAccts, Action onAfterMenuItemClick = null) : base(view)
        {
            _rowHandle = rowHandle;
            this._parentWindow = _parentWindow;
            this.ucAcctsCaller = ucAccts;
            this.onAfterMenuItemClick = onAfterMenuItemClick;
        }

        //Begin 87757
        public TransactionsMenu(DevExpress.XtraGrid.Views.Grid.GridView view, int rowHandle, ITellerWindow _parentWindow, ucTransactions ucTrans, Action onAfterMenuItemClick = null) : base(view)
        {
            _rowHandle = rowHandle;
            this._parentWindow = _parentWindow;
            this.ucTransCaller = ucTrans;
            this.onAfterMenuItemClick = onAfterMenuItemClick;
        }
        //End 87757


        // Create menu items. 
        // This method is automatically called by the menu's public Init method. 
        protected override void CreateItems()
        {
            Items.Clear();
            //

            //Begin 87757
            if (ucAcctsCaller != null)
            {
                DataRow row = View.GetDataRow(_rowHandle);
                string rowDepLoan = ((string)row["DepLoan"] + "").Trim();
                string skipType = rowDepLoan == "DP" ? "LN" : "DP";

                //Begin 87751
                string[] DepLoan = new string[3] { "DP", "LN", "EXT" };
                string Adapter = ((string)row["Adapter"] + "");
                //End 87751

                string ApplType = ((string)row["ApplType"] + "").Trim();  //95598


                foreach (var a in TranHelper.Instance.TranTypeList)     //87759
                {
                    if (a.DpLN == skipType) continue;

                    //Begin 87761
                    if (rowDepLoan == "EXT" && ((a.DpLN != "EX" && a.DpLN != "RM") || (a.DpLN == "EX" && Adapter != "Y"))) continue;  //87751
                    if (rowDepLoan != "EXT" && a.DpLN == "EX") continue;
                    //End 87761

                    if (Array.FindIndex(DepLoan, item => item == rowDepLoan) < 0 && a.DpLN != "RM") continue; //87751: SDB & CMM accts

                    if (a.DpLN == "DP" && ApplType == "CD") continue; //95598: Exclude CDs

                    var menuItem = CreateMenuItem(GetMenuItemDescription(a.TranType), (Image)null, a, true);
                    menuItem.Shortcut = GetShortcut(a.TranType); //Task #90004
                    Items.Add(menuItem); //87759
                }
            }
            else if (ucTransCaller != null)
            {
                if (View.GetRowCellValue(_rowHandle, "Options") == null || View.GetRowCellValue(_rowHandle, "Options").ToString() == string.Empty)
                    return;

                string tranType = View.GetRowCellValue(_rowHandle, "TcDescription").ToString();
                string CurrTranOption = View.GetRowCellValue(_rowHandle, "Options").ToString();
                string optionInfo = "[Current Selected Option]";

                if (tranType == "Transfer")
                {
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranTfrOptions.TransferDeposit.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranTfrOptions.TransferDeposit.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranTfrOptions.TransferDeposit.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranTfrOptions.TransferWithdrawal.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranTfrOptions.TransferWithdrawal.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranTfrOptions.TransferWithdrawal.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranTfrOptions.AutoLoanPayment.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranTfrOptions.AutoLoanPayment.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranTfrOptions.AutoLoanPayment.ToString()]), (Image)null, tranType, true));  //87759
                }
                else if (tranType == "Payment")
                {
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranPmtOptions.PaymentAutoSplitPaytoZero.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranPmtOptions.PaymentAutoSplitPaytoZero.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranPmtOptions.PaymentAutoSplitPaytoZero.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranPmtOptions.PrincipalPayment.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranPmtOptions.PrincipalPayment.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranPmtOptions.PrincipalPayment.ToString()]), (Image)null, tranType, true));
                }
                //Begin 87751
                else if (tranType == "External")
                {
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranExtOptions.ExternalCredit.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranExtOptions.ExternalCredit.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranExtOptions.ExternalCredit.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranExtOptions.ExternalDebit.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranExtOptions.ExternalDebit.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranExtOptions.ExternalDebit.ToString()]), (Image)null, tranType, true));
                }
                //End 87751
                else if (tranType == "Purchase")    //#87755
                {
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranPurchaseOptions.CashiersCheck.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranPurchaseOptions.CashiersCheck.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranPurchaseOptions.CashiersCheck.ToString()]), (Image)null, tranType, true));
                    Items.Add(CreateMenuItem((CurrTranOption == EasyCaptureTranPurchaseOptions.MoneyOrder.ToString() ? string.Format("{0} - {1}", TranMenuOptions[EasyCaptureTranPurchaseOptions.MoneyOrder.ToString()], optionInfo) : TranMenuOptions[EasyCaptureTranPurchaseOptions.MoneyOrder.ToString()]), (Image)null, tranType, true));
                }

                //End 99054 

            }
            //End 87757
        }

        //Begin Bug #99045
        private string GetMenuItemDescription(string tranType)
        {
            string menuItemName = tranType;

            switch (tranType)
            {
                case "OnUsCashedCheck":
                    menuItemName = "On-Us Check Cashing";
                    break;
                case "CashCheck":
                    menuItemName = "Check Cashing";
                    break;
            }
            return menuItemName;
        }
        //End Bug #99045

        //Begin Task #90004
        private Shortcut GetShortcut(string tranType)
        {
            Shortcut shortcut = Shortcut.None;
            if (string.IsNullOrEmpty(tranType))
                return shortcut;

            switch (tranType.ToLower())
            {
                //Begin Task# 115070
                //#120877
                case "deposit":
                    //shortcut = Shortcut.CtrlE;
                    shortcut = Shortcut.Ctrl1;
                    break;
                case "withdrawal":
                    //shortcut = Shortcut.CtrlQ; //Bug #106778
                    shortcut = Shortcut.Ctrl2;
                    break;
                case "onuscashedcheck":
                    // = Shortcut.CtrlO;
                    shortcut = Shortcut.Ctrl3;
                    break;
                case "transfer":
                    //shortcut = Shortcut.CtrlG;
                    shortcut = Shortcut.Ctrl4;
                    break;
                case "payment":
                    //shortcut = Shortcut.CtrlY;
                    shortcut = Shortcut.Ctrl5;
                    break;
                case "advance":
                    //shortcut = Shortcut.CtrlA;
                    shortcut = Shortcut.Ctrl6;
                    break;
                case "external":
                    //shortcut = Shortcut.CtrlL;
                    shortcut = Shortcut.Ctrl7;
                    break;
                case "cashcheck":
                    //shortcut = Shortcut.CtrlC;
                    shortcut = Shortcut.Ctrl8;
                    break;
                case "purchase":
                    //shortcut = Shortcut.CtrlU;
                    shortcut = Shortcut.Ctrl9;
                    break;
                    //End Task# 115070
            }

            return shortcut;
        }
        //End Task #90004

        protected override void OnMenuItemClick(object sender, EventArgs e)
        {
            if (RaiseClickEvent(sender, null)) return;

            DXMenuItem item = sender as DXMenuItem;

            //Begin 87757
            if (ucAcctsCaller != null)
            {
                _parentWindow.MoveAcctToTran(item);
                //

                TransactionTypeDefinition a = item.Tag as TransactionTypeDefinition;  //87759

                if (a == null)
                    return;

                EasyCaptureTran tran = new EasyCaptureTran();

                tran.TranType = TranHelper.Instance.TranList.Find(x => x.Description == a.Description).TranType; //87759

                DataRow row = this.View.GetDataRow(_rowHandle);
                if ((row.GetStringValue("DepLoan") + "").Trim() != a.DpLN && a.DpLN != "RM" && (!(row.GetStringValue("DepLoan") == "EXT" && a.DpLN == "EX"))) //87751-2
                    return;

                if (a.DpLN == "RM")
                {
                    //if (_parentWindow.Customers == null || _parentWindow.Customers.Rows.Count == 0)
                    //    return;

                    //var rimNo = _parentWindow.Customers.Rows[0]["RimNo"];
                    tran.AcctNo = row.GetStringValue("RimNo");
                    tran.AcctType = "RIM";
                }
                else
                {
                    tran.AcctNo = row.GetStringValue("AcctNo");
                    tran.AcctType = row.GetStringValue("AcctType");
                }

                tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == a.Description); //87759

                //Begin 87751
                tran.SelectedAcctNo = row.GetStringValue("AcctNo");
                tran.SelectedAcctType = row.GetStringValue("AcctType");
                tran.SelectedDepLoan = ((string)row["DepLoan"] + "").Trim();
                //End 87751

                if (tran.TranType == EasyCaptureTranType.Deposit && tran.TranDef != null)   //#87731-1
                {
                    //Begin 87759
                    if (row.GetStringValue("Status") == "Unfunded")
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.OpeningDeposit.ToString());
                    else
                        tran.TranDef = TranHelper.Instance.TranList.Find(x => x.Description == EasyCaptureTranCodeDesc.Deposit.ToString());
                    //End 87759
                }

                _parentWindow.TransactionSet.Transactions.Add(tran);

                _parentWindow.SetTransactionMenuIndicator(); //Bug #99053

                //Vidya - Move focus to tran grid
                KeyEventArgs ka = new KeyEventArgs(a.ShortcutKey);
                ((PDxDockPanel)(this.ucAcctsCaller._parentDockPanel)).OnPanelKeyDown(ka);
                if (this.onAfterMenuItemClick != null)
                {
                    onAfterMenuItemClick();
                }
            }
            else if (ucTransCaller != null)
            {
                string trantype = item.Tag.ToString();

                if (trantype == string.Empty)
                    return;

                if (trantype == "Transfer" || trantype == "Payment" || trantype == "External" || trantype == "Purchase") //87751 #87755
                {
                    if (item.Caption != string.Empty)
                    {
                        string optionSelected;

                        if (item.Caption.IndexOf("-") > 0)
                            optionSelected = item.Caption.Substring(0, item.Caption.IndexOf("-") - 1); //MAYBE WE DO NOTHING ???
                        else
                            optionSelected = item.Caption;

                        //Begin 99054 

                        //this.View.SetRowCellValue(this.View.FocusedRowHandle, "Options", optionSelected.Trim()); //87759

                        //string MenuOption = from entry in TranMenuOptions where entry.Value == optionSelected.Trim() select entry.Key;
                        string MenuOption = TranMenuOptions.FirstOrDefault(x => x.Value == optionSelected.Trim()).Key;

                        this.View.SetRowCellValue(this.View.FocusedRowHandle, "Options", MenuOption);
                        if (this.onAfterMenuItemClick != null)
                        {
                            onAfterMenuItemClick();
                        }

                        //End 99054 
                    }
                }
            }
            //End 87757
        }

        //Begin 99054 
        public Dictionary<string, string> TranMenuOptions = new Dictionary<string, string>() {
                { EasyCaptureTranPmtOptions.PaymentAutoSplit.ToString(), "Payment Auto Split" },
                { EasyCaptureTranPmtOptions.PaymentAutoSplitPaytoZero.ToString(), "Pay To Zero" },
                { EasyCaptureTranPmtOptions.PrincipalPayment.ToString(), "Principal Payment" },

                { EasyCaptureTranPurchaseOptions.CashiersCheck.ToString(), "Cashiers Check" },

                { EasyCaptureTranPurchaseOptions.MoneyOrder.ToString(), "Money Order" },

                { EasyCaptureTranTfrOptions.TransferDeposit.ToString(), "Transfer Deposit" },
                { EasyCaptureTranTfrOptions.TransferWithdrawal.ToString(), "Transfer Withdrawal" },
                { EasyCaptureTranTfrOptions.AutoLoanPayment.ToString(), "Auto Split Payment" },

                { EasyCaptureTranExtOptions.ExternalCredit.ToString(), "External Credit"},
                { EasyCaptureTranExtOptions.ExternalDebit.ToString(), "External Debit"}
                };
        //End 99054 
    }
}
