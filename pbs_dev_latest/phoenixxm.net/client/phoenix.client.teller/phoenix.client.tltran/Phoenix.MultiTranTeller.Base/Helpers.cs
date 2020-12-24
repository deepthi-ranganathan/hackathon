#region Archived Comments
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//11/13/2019    2       mselvaga    Task#120877 - 240921 - 2019 SP5 - 8 - DEV - Porting of all the bugs into 2019 - Fixed the Tran type menu order and shotcuts for LN/RM.
//03/04/2020    3       mselvaga    #124918 - MT Offline- Error on cashier check and MO purchase


#endregion

using Phoenix.MultiTranTeller.Base.Models;
using Phoenix.Shared.Variables;
using System.Collections.Generic;
using System.Windows.Forms;
namespace Phoenix.MultiTranTeller.Base
{

    public class TranHelper
    {
        private static TranHelper _instance = new TranHelper();

        List<TransactionDefinition> _listTlTran = new List<TransactionDefinition>();

        List<TransactionTypeDefinition> _listTlTranType = new List<TransactionTypeDefinition>(); //87759

        List<DenomincationUnit> _denominationDef;

        public static TranHelper Instance { get { return _instance; } }

        //Begin Task# 115764
        //public enum ShortKeys { A = Keys.A, C = Keys.C, D = Keys.D, O = Keys.O, P = Keys.P, T = Keys.T, U = Keys.U, W = Keys.W, E = Keys.E }; //87759: Added Key.E
        public enum ShortKeys { NumPad1 = Keys.D1, NumPad2 = Keys.D2, NumPad3 = Keys.D3, NumPad4 = Keys.D4, NumPad5 = Keys.D5, NumPad6 = Keys.D6, NumPad7 = Keys.D7, NumPad8 = Keys.D8, NumPad9 = Keys.D9 };
        //End Task# 115764



        public TranHelper()
        {
            //Begin 87759
            //_listTlTran = new List<TransactionDefinition>(){
            //    new TransactionDefinition("Q01", EasyCaptureTranType.Deposit.ToString(), "DP", EasyCaptureTranType.Deposit, (Keys)ShortKeys.D),
            //    new TransactionDefinition("Q11", EasyCaptureTranType.Withdrawl.ToString(), "DP", EasyCaptureTranType.Withdrawl, (Keys)ShortKeys.W),
            //    new TransactionDefinition("Q12", EasyCaptureTranType.OnUsCashedCheck.ToString(), "DP", EasyCaptureTranType.OnUsCashedCheck, (Keys)ShortKeys.O),
            //    new TransactionDefinition("Q21", EasyCaptureTranType.Payment.ToString(), "LN", EasyCaptureTranType.Payment, (Keys)ShortKeys.P),
            //    new TransactionDefinition("Q31", EasyCaptureTranType.Advance.ToString(), "LN", EasyCaptureTranType.Advance, (Keys)ShortKeys.A),
            //    new TransactionDefinition("Q36", EasyCaptureTranType.Transfer.ToString(), "DP", EasyCaptureTranType.Transfer, (Keys)ShortKeys.T),
            //    new TransactionDefinition("Q46", EasyCaptureTranType.CashCheck.ToString(), "RM", EasyCaptureTranType.CashCheck, (Keys)ShortKeys.C),
            //    new TransactionDefinition("Q47", EasyCaptureTranType.Purchase.ToString(), "RM", EasyCaptureTranType.Purchase, (Keys)ShortKeys.U)
            //    };  //#87731-1



            _listTlTran = new List<TransactionDefinition>(){
                new TransactionDefinition("Q01", EasyCaptureTranCodeDesc.OpeningDeposit.ToString(), "DP", TellerVars.Instance.IsAppOnline? EasyCaptureTranType.Deposit: EasyCaptureTranType.OpeningDeposit),  //Bug 109838 //107401: was EasyCaptureTranType.OpeningDeposi 
                new TransactionDefinition("Q02", EasyCaptureTranCodeDesc.Deposit.ToString(), "DP", EasyCaptureTranType.Deposit),

                new TransactionDefinition("Q11", EasyCaptureTranCodeDesc.Withdrawal.ToString(), "DP", EasyCaptureTranType.Withdrawal),  //93431
                new TransactionDefinition("Q12", EasyCaptureTranCodeDesc.CashedCheck.ToString(), "DP", EasyCaptureTranType.OnUsCashedCheck),

                new TransactionDefinition("Q21", EasyCaptureTranCodeDesc.PaymentAutoSplit.ToString(), "LN", EasyCaptureTranType.Payment),
                new TransactionDefinition("Q22", EasyCaptureTranCodeDesc.PaymentAutoSplitPaytoZero.ToString(), "LN", EasyCaptureTranType.Payment),
                new TransactionDefinition("Q23", EasyCaptureTranCodeDesc.PrincipalPayment.ToString(), "LN", EasyCaptureTranType.Payment),

                new TransactionDefinition("Q31", EasyCaptureTranCodeDesc.LoanAdvance.ToString(), "LN", EasyCaptureTranType.Advance),

                new TransactionDefinition("Q36", EasyCaptureTranCodeDesc.TransferWithdrawal.ToString(), "DP", EasyCaptureTranType.Transfer),
                new TransactionDefinition("Q37", EasyCaptureTranCodeDesc.AutoLoanPayment.ToString(), "DP", EasyCaptureTranType.Transfer),
                new TransactionDefinition("Q38", EasyCaptureTranCodeDesc.TransferDeposit.ToString(), "DP", EasyCaptureTranType.Transfer),

                new TransactionDefinition("Q46", EasyCaptureTranCodeDesc.CashCheck.ToString(), "RM", EasyCaptureTranType.CashCheck),
                new TransactionDefinition("Q47", EasyCaptureTranCodeDesc.CashiersCheck.ToString(), "RM", EasyCaptureTranType.Purchase),
                new TransactionDefinition("Q48", EasyCaptureTranCodeDesc.MoneyOrder.ToString(), "RM", EasyCaptureTranType.Purchase),

                new TransactionDefinition("Q56", EasyCaptureTranCodeDesc.ExternalCredit.ToString(), "EX", EasyCaptureTranType.External),
                new TransactionDefinition("Q57", EasyCaptureTranCodeDesc.ExternalDebit.ToString(), "EX", EasyCaptureTranType.External)
                };

            //Begin Task# 115764
            #region Commnected out code
            //The tran code is the default value
            //_listTlTranType = new List<TransactionTypeDefinition>(){
            //    new TransactionTypeDefinition("Q01", EasyCaptureTranCodeDesc.OpeningDeposit.ToString(), "DP", EasyCaptureTranType.OpeningDeposit.ToString(), Keys.Control | Keys.N,"Opening Deposit"),
            //    new TransactionTypeDefinition("Q02", EasyCaptureTranCodeDesc.Deposit.ToString(), "DP", EasyCaptureTranType.Deposit.ToString(), Keys.Control | Keys.E),
            //    new TransactionTypeDefinition("Q11", EasyCaptureTranCodeDesc.Withdrawal.ToString(), "DP", EasyCaptureTranType.Withdrawal.ToString(), Keys.Control | Keys.Q), //106778  //93431
            //    new TransactionTypeDefinition("Q12", EasyCaptureTranCodeDesc.CashedCheck.ToString(), "DP", EasyCaptureTranType.OnUsCashedCheck.ToString(), Keys.Control | Keys.O,"On-Us Check Cashing"),
            //    new TransactionTypeDefinition("Q21", EasyCaptureTranCodeDesc.PaymentAutoSplit.ToString(), "LN", EasyCaptureTranType.Payment.ToString(), Keys.Control | Keys.Y),
            //    new TransactionTypeDefinition("Q31", EasyCaptureTranCodeDesc.LoanAdvance.ToString(), "LN", EasyCaptureTranType.Advance.ToString(), Keys.Control | Keys.A),
            //    new TransactionTypeDefinition("Q36", EasyCaptureTranCodeDesc.TransferWithdrawal.ToString(), "DP", EasyCaptureTranType.Transfer.ToString(), Keys.Control | Keys.G),
            //    new TransactionTypeDefinition("Q46", EasyCaptureTranCodeDesc.CashCheck.ToString(), "RM", EasyCaptureTranType.CashCheck.ToString(), Keys.Control | Keys.C,"Check Cashing"),
            //    new TransactionTypeDefinition("Q47", EasyCaptureTranCodeDesc.CashiersCheck.ToString(), "RM", EasyCaptureTranType.Purchase.ToString(), Keys.Control | Keys.U),
            //    new TransactionTypeDefinition("Q56", EasyCaptureTranCodeDesc.ExternalCredit.ToString(), "EX", EasyCaptureTranType.External.ToString(), Keys.Control | Keys.L)
            //    };
            #endregion
            //#120877
            _listTlTranType = new List<TransactionTypeDefinition>(){
                new TransactionTypeDefinition("Q01", EasyCaptureTranCodeDesc.OpeningDeposit.ToString(), "DP", EasyCaptureTranType.OpeningDeposit.ToString(), Keys.Control | Keys.NumPad0, Keys.Control | Keys.D0,"Opening Deposit"),
                new TransactionTypeDefinition("Q02", EasyCaptureTranCodeDesc.Deposit.ToString(), "DP", EasyCaptureTranType.Deposit.ToString(), Keys.Control | Keys.NumPad1 , Keys.Control | Keys.D1),
                new TransactionTypeDefinition("Q11", EasyCaptureTranCodeDesc.Withdrawal.ToString(), "DP", EasyCaptureTranType.Withdrawal.ToString(), Keys.Control | Keys.NumPad2 , Keys.Control | Keys.D2), //106778  //93431
                new TransactionTypeDefinition("Q12", EasyCaptureTranCodeDesc.CashedCheck.ToString(), "DP", EasyCaptureTranType.OnUsCashedCheck.ToString(), Keys.Control | Keys.NumPad3 ,Keys.Control | Keys.D3,"On-Us Check Cashing"),
                new TransactionTypeDefinition("Q36", EasyCaptureTranCodeDesc.TransferWithdrawal.ToString(), "DP", EasyCaptureTranType.Transfer.ToString(), Keys.Control | Keys.NumPad4 ,Keys.Control | Keys.D4),
                //new TransactionTypeDefinition("Q46", EasyCaptureTranCodeDesc.CashCheck.ToString(), "RM", EasyCaptureTranType.CashCheck.ToString(), Keys.Control | Keys.NumPad5 ,Keys.Control | Keys.D5,"Check Cashing"),
                //new TransactionTypeDefinition("Q47", EasyCaptureTranCodeDesc.CashiersCheck.ToString(), "RM", EasyCaptureTranType.Purchase.ToString(), Keys.Control | Keys.NumPad6 , Keys.Control | Keys.D6),
                new TransactionTypeDefinition("Q21", EasyCaptureTranCodeDesc.PaymentAutoSplit.ToString(), "LN", EasyCaptureTranType.Payment.ToString(), Keys.Control | Keys.NumPad5 , Keys.Control | Keys.D5),
                new TransactionTypeDefinition("Q31", EasyCaptureTranCodeDesc.LoanAdvance.ToString(), "LN", EasyCaptureTranType.Advance.ToString(), Keys.Control | Keys.NumPad6 ,Keys.Control | Keys.D6),
                new TransactionTypeDefinition("Q56", EasyCaptureTranCodeDesc.ExternalCredit.ToString(), "EX", EasyCaptureTranType.External.ToString(), Keys.Control | Keys.NumPad7, Keys.Control | Keys.D7),
                new TransactionTypeDefinition("Q46", EasyCaptureTranCodeDesc.CashCheck.ToString(), "RM", EasyCaptureTranType.CashCheck.ToString(), Keys.Control | Keys.NumPad8 ,Keys.Control | Keys.D8,"Check Cashing"),
                new TransactionTypeDefinition("Q47", EasyCaptureTranCodeDesc.CashiersCheck.ToString(), "RM", EasyCaptureTranType.Purchase.ToString(), Keys.Control | Keys.NumPad9 , Keys.Control | Keys.D9)
                };
            //End Task# 115764

            //Begin Bug #103647
            if (TellerVars.Instance.IsAppOnline)
            {
                //_listTlTran.RemoveAt(0);    //107401
                _listTlTranType.RemoveAt(0);
            }
            else
            {
                _listTlTranType.RemoveAt(9);    //#124918 - Do not show Purchase option for Offline
            }
            //End Bug #103647

            //End 87759

            _denominationDef = new List<DenomincationUnit>() {
                new DenomincationUnit("100$", 100),
                new DenomincationUnit("50$", 50),
                new DenomincationUnit("20$", 20),
                new DenomincationUnit("10$", 10),
                new DenomincationUnit("5$", 5)
            };

            //_customerTransaction.CollectionChanged += _customerTransaction_CollectionChanged;
        }

        public List<TransactionDefinition> TranList { get { return _listTlTran; } }

        public List<TransactionTypeDefinition> TranTypeList { get { return _listTlTranType; } } //87759

        public List<DenomincationUnit> DenomDefinitions
        {
            get { return _denominationDef; }

        }

        public string CombineAcctNo(string acctNo, string acctType)
        {
            if (acctNo != null)
                acctNo = acctNo.Trim();
            if (acctType != null)
                acctType = acctType.Trim();

            //return $"{acctType} - {acctNo}";
            return string.Format("{0} - {1}", acctType, acctNo);
        }

        public int FormatAcctNo(string acctNo)
        {
            string formattedAccountNumber = acctNo.Replace("-", "");
            int acctNumber = 0;
            if (!int.TryParse(formattedAccountNumber, out acctNumber))
            {
                acctNumber = -1;
            }

            return acctNumber;
        }
    }


}
