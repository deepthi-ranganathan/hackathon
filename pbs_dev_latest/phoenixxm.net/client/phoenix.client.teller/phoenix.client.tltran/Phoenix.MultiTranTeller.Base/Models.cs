using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix.MultiTranTeller.Base.Models
{
    public abstract class EasyCaptureBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        // This method is called by the Set accessor of each property.  
        // The CallerMemberName attribute that is applied to the optional propertyName  
        // parameter causes the property name of the caller to be substituted as an argument.  
        protected virtual void NotifyPropertyChanged( String propertyName )
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            //87731 - Build Issues Fixed
        }
    }

    public class EasyCaptureTran : EasyCaptureBase
    {
        #region
        private string _tc;
        public string TcDescription
        {
            get { return _tc; }
            set
            {
                if (_tc != value)
                {
                    _tc = value;
                    NotifyPropertyChanged("TcDescription");
                }
            }
        }

        


        private TransactionDefinition _transactionDefinition;


        public TransactionDefinition TranDef
        {
            get { return _transactionDefinition; }
            set
            {
                if (_transactionDefinition != value)
                {
                    _transactionDefinition = value;
               
                    if (_transactionDefinition != null)
                        this.TcDescription = _transactionDefinition.TranType.ToString();   //87757: was_transactionDefinition.Description;
                    else
                        this.TcDescription = null;
                
                    NotifyPropertyChanged("TranDef");
                }
            }
        }





        private string _acctCombined;
        public string AcctCombined
        {
            get { return _acctCombined; }
            set
            {
                if (_acctCombined != value)
                {
                    _acctCombined = value;
                    NotifyPropertyChanged("AcctCombined");
                }
            }
        }

        protected override void NotifyPropertyChanged( string propertyName )
        {
            base.NotifyPropertyChanged(propertyName);
            if (propertyName == "AcctNo" || propertyName == "AcctType")
                AcctCombined =  TranHelper.Instance.CombineAcctNo(AcctNo, AcctType); //, row.GetStringValue("AcctType"))
            if (propertyName == "TfrAcctNo" || propertyName == "TfrAcctType")
                AcctCombined = TranHelper.Instance.CombineAcctNo(AcctNo, AcctType); //, row.GetStringValue("AcctType"))
        }
#endregion


        private decimal _amount;
        //private decimal _holdBal = 100.25M;
        private decimal _holdBal;
        EasyCaptureTranType _tranType;
        private string _acctType;
        private string _acctNo;
        private decimal _ccAmt;
        private string _description;
        private string _reference;
        private string _checkNo; //Task #113989
        private bool _passDescriptionToHistory;
        private bool _regD;
        //Begin 87757
        private string _tfrAcctNo;
        private string _tfrAcctType;
        private string _options;
        private decimal _tfrAmount;
        //End 87757
        //Begin 87751
        private string _selectedAcctNo;
        private string _selectedAcctType;
        private string _selectedDepLoan;
        private int _tranId;    //#87767
        //End 87751
        private object _menuIndicator;

        public object MenuIndicator
        {
            get { return _menuIndicator; }
            set
            {
                if (_menuIndicator != value)
                {
                    _menuIndicator = value;

                    NotifyPropertyChanged("MenuIndicator");
                }
            }
        }

        public string AcctType
        {
            get { return _acctType; }
            set
            {
                if (_acctType != value)
                {
                    _acctType = value;
                   
                    NotifyPropertyChanged("AcctType");
                }
            }
        }


        public string AcctNo
        {
            get { return _acctNo; }
            set
            {
                if (_acctNo != value)
                {
                    _acctNo = value;

                    NotifyPropertyChanged("AcctNo");
                }
            }
        }


        //Begin 87757
        public string TfrAcctType
        {
            get { return _tfrAcctType; }
            set
            {
                if (_tfrAcctType != value)
                {
                    _tfrAcctType = value;

                    NotifyPropertyChanged("TfrAcctType");
                }
            }
        }

        public string TfrAcctNo
        {
            get { return _tfrAcctNo; }
            set
            {
                if (_tfrAcctNo != value)
                {
                    _tfrAcctNo = value;

                    NotifyPropertyChanged("TfrAcctNo");
                }
            }
        }

        public string Options
        {
            get { return _options; }
            set
            {
                if (_options != value)
                {
                    _options = value;

                    NotifyPropertyChanged("Options");
                }
            }
        }

        public decimal TfrAmount
        {
            get { return _tfrAmount; }

            set
            {
                if (_tfrAmount != value)
                {
                    _tfrAmount = value;
                    NotifyPropertyChanged("TfrAmount");
                }
            }
        }
        //End 87757
        //Begin 87751
        public string SelectedAcctType
        {
            get { return _selectedAcctType; }
            set
            {
                if (_selectedAcctType != value)
                {
                    _selectedAcctType = value;

                    //NotifyPropertyChanged("SelectedAcctType");
                }
            }
        }

        public string SelectedAcctNo
        {
            get { return _selectedAcctNo; }
            set
            {
                if (_selectedAcctNo != value)
                {
                    _selectedAcctNo = value;

                    //NotifyPropertyChanged("SelectedAcctNo");
                }
            }
        }

        public string SelectedDepLoan
        {
            get { return _selectedDepLoan; }
            set
            {
                if (_selectedDepLoan != value)
                {
                    _selectedDepLoan = value;

                    //NotifyPropertyChanged("SelectedDepLoan");
                }
            }
        }
        //End 87751
        public decimal Amount
        {
            get { return _amount; }

            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public decimal HoldBal
        {
            get { return _holdBal; }

            set
            {
                if (_holdBal != value)
                {
                    _holdBal = value;
                    NotifyPropertyChanged("HoldBal");
                }
            }
        }

        public int TranId
        {
            get { return _tranId; }

            set { _tranId = value; }
        }


        public EasyCaptureTranType TranType
        {
            get { return _tranType; }
            set

            {
                if (_tranType != value)
                {
                    _tranType = value;
                    NotifyPropertyChanged("TranType");
                }
            }
        }

        public int MappedTranIndex
        {
            get
            {
                return _mappedTranIndex;
            }

            set
            {
                _mappedTranIndex = value;
            }
        }

        public bool IsValidated
        {
            get
            {
                return _isValidated;
            }

            set
            {
                _isValidated = value;
            }
        }

        public List<EasyCaptureTranResponse> TranResponse
        {
            get
            {
                if (_tranResponse == null)
                    _tranResponse = new List<EasyCaptureTranResponse>();
                return _tranResponse;
            }

       }

        public bool ShowError
        {
            get
            {
                return (_tranResponse != null && _tranResponse.Count > 0);
            }
        }

        /// <summary>
        /// Returns the Error Text for the transaction
        /// </summary>
        /// <returns></returns>
        public string GetErrorText()
        {
            string errorText = null;
            foreach (var tranResponse in TranResponse)
            {
                //errorText += $"ErrorType:{tranResponse.ErrorType}, Error:{tranResponse.ErrorCode} - {tranResponse.ErrorText}";
                //errorText += string.Format("ErrorType:{0}, Error:{1} - {2}", tranResponse.ErrorType, tranResponse.ErrorCode, tranResponse.ErrorText);
                errorText += string.Format("Error:{0} - {1}", tranResponse.ErrorCode, tranResponse.ErrorText);
            }
            return errorText;
        }

        public decimal CcAmt
        {
            get
            {
                return _ccAmt;
            }

            set
            {
                if (_ccAmt != value)
                {
                    _ccAmt = value;
                    NotifyPropertyChanged("CcAmt");
                }
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if (_description != value)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        public string Reference
        {
            get
            {
                return _reference;
            }

            set
            {
                if (_reference != value)
                {
                    _reference = value;
                    NotifyPropertyChanged("Reference");
                }
            }
        }

        public string CheckNo //Task #113989
        {
            get
            {
                return _checkNo;
            }

            set
            {
                if (_checkNo != value)
                {
                    _checkNo = value;
                    NotifyPropertyChanged("CheckNo");
                }
            }
        }

        public bool PassDescriptionToHistory
        {
            get
            {
                return _passDescriptionToHistory;
            }

            set
            {
                if (_passDescriptionToHistory != value)
                {
                    _passDescriptionToHistory = value;
                    NotifyPropertyChanged("PassDescriptionToHistory");
                }
            }
        }

        public bool RegD
        {
            get
            {
                return _regD;
            }

            set
            {
                if (_regD != value)
                {
                    _regD = value;
                    NotifyPropertyChanged("RegD");
                }
            }
        }

        private int _mappedTranIndex = -1;
        private bool _isValidated = false;
        private List<EasyCaptureTranResponse> _tranResponse;

    }




    public class EasyCaptureChecks: EasyCaptureBase
    {
        public string RoutingNo { get; set; }

        public EasyCaptureCheckType CheckType { get; set; }


        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        private string _acctNo;
        public string AcctNo
        {
            get { return _acctNo; }
            set
            {
                if (_acctNo != value)
                {
                    _acctNo = value;
                    NotifyPropertyChanged("AcctNo");
                }
            }
        }


    }

    public class EasyCaptureTranSet: EasyCaptureBase
    {

        ObservableCollection<EasyCaptureTran> _transactions;
        //ObservableCollection<EasyCaptureChecks> _checks = new ObservableCollection<EasyCaptureChecks>();
        //ObservableCollection<EasyCaptureDenomination> _cashIns = new ObservableCollection<EasyCaptureDenomination>();

        ArrayList _checks = new ArrayList();
        ArrayList _cashIns = new ArrayList();
        ArrayList _cashOuts = new ArrayList();

        int _cashInTranMappedIndex;

        //public event PropertyChangedEventHandler PropertyChanged;
        //// This method is called by the Set accessor of each property.  
        //// The CallerMemberName attribute that is applied to the optional propertyName  
        //// parameter causes the property name of the caller to be substituted as an argument.  
        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        //}
        public virtual void InitializeCollections()
        {
            if (_transactions == null)
            {
                _transactions = new ObservableCollection<EasyCaptureTran>();
            }
        }

        public EasyCaptureTranSet()
        {
            InitializeCollections();
        }


        public virtual ObservableCollection<EasyCaptureTran> Transactions { get { return _transactions; } }
        //public ObservableCollection<EasyCaptureChecks> Checks { get { return _checks; } }
        //public ObservableCollection<EasyCaptureDenomination> CashIns { get { return _cashIns; } }

        public ArrayList Checks { get { return _checks; } }
        public ArrayList CashIns { get { return _cashIns; } set { _cashIns = value; } }

        public ArrayList CashOuts { get { return _cashOuts; } }

        public int CashInTranMappedIndex { get {return _cashInTranMappedIndex; } set { _cashInTranMappedIndex = value; } }

        public EasyCaptureTran Find(EasyCaptureTranType tranType)
        {
            foreach( var item in Transactions )
            {
                if (item.TranType == tranType)
                    return item;
            }
            return null;
        }

        public EasyCaptureTran Find(bool findFirstTran)
        {
            foreach (var item in Transactions)
            {
                return item;
            }
            return null;
        }

        public EasyCaptureTran Find(EasyCaptureTranType tranType, string collectionName)
        {
            foreach (var item in Transactions)
            {
                if (collectionName == "CashIn")
                {
                    if (CashInTran == null && item.TranType == tranType)
                        return item;
                    if (item == CashInTran && item.TranType == tranType)
                        return item;
                }

                if (collectionName == "CashOut")
                {
                    if (CashOutTran == null && item.TranType == tranType)
                        return item;
                    if (item == CashOutTran && item.TranType == tranType)
                        return item;
                }

                if (collectionName == "Checks")
                {
                    if (ChecksTran == null && item.TranType == tranType)
                        return item;
                    if (item == ChecksTran && item.TranType == tranType)
                        return item;
                }
            }
            return null;
        }

        public EasyCaptureTran Find(string description, string tranCode)
        {
            foreach (var item in Transactions)
            {
                if (item.TcDescription == description && item.TranDef.TranCode == tranCode)
                    return item;
            }
            return null;
        }

        public EasyCaptureTran Find(string description, string tranCode, int tranId)    //#87767
        {
            foreach (var item in Transactions)
            {
                if (item.TcDescription == description && item.TranDef.TranCode == tranCode && item.TranId == tranId)
                    return item;
            }
            return null;
        }

        public EasyCaptureTran Find(int tranId)    //#87767
        {
            foreach (var item in Transactions)
            {
                if (item.TranId == tranId)
                    return item;
            }
            return null;
        }

        public void UpdateTranId(string description, string tranCode, int tranId)    //#87767
        {
            foreach (var item in Transactions)
            {
                if (item.TcDescription == description && item.TranDef.TranCode == tranCode && item.TranId <= 0)
                {
                    item.TranId = tranId;
                    break;
                }
            }
        }

        public EasyCaptureTran Find(EasyCaptureTranCodeDesc tcDesc)
        {
            foreach (var item in Transactions)
            {
                if (item.Description == tcDesc.ToString())
                    return item;
            }
            return null;
        }

        public EasyCaptureTran Find(string depLoan)
        {
            foreach (var item in Transactions)
            {
                var tranDef = TranHelper.Instance.TranList.Find(x => x.TranType == item.TranType);
                if ( tranDef != null && tranDef.DpLN == depLoan)
                    return item;
            }
            return null;
        }

        static int _iSNIdCounter = 0;

        private EasyCaptureTran _checksTran;
        private EasyCaptureTran _cashInTran;
        private EasyCaptureTran _cashOutTran;

        private decimal _cashin;
        public decimal CashIn
        {
            get { return _cashin; }
            set
            {
                if (_cashin != value)
                {
                    if (value > 1 && !Convert.ToString(value).Contains("."))
                        value = Convert.ToDecimal(string.Format("{0}.00", value.ToString()));
                    _cashin = value;
                    NotifyPropertyChanged("CashIn");
                }
            }
        }

        private decimal _checksIn;
        public decimal ChecksIn
        {
            get { return _checksIn; }
            set
            {
                if (_checksIn != value)
                {
                    _checksIn = value;
                    NotifyPropertyChanged("ChecksIn");
                }
            }
        }

        private int _noItems;
        public int NoItems
        {
            get { return _noItems; }
            set
            {
                if (_noItems != value)
                {
                    _noItems = value;
                }
            }
        }

        private decimal _cashOut;
        public decimal CashOut
        {
            get { return _cashOut; }
            set
            {
                if (_cashOut != value)
                {
                    _cashOut = value;
                    NotifyPropertyChanged("CashOut");
                }
            }
        }

        private decimal _checksMakeAvail = 0;


        public EasyCaptureTran ChecksTran
        {
            get
            {
                return _checksTran;
            }

            set
            {
                _checksTran = value;
            }
        }

        public EasyCaptureTran CashInTran
        {
            get
            {
                return _cashInTran;
            }

            set
            {
                _cashInTran = value;
            }
        }

        public EasyCaptureTran CashOutTran
        {
            get
            {
                return _cashOutTran;
            }

            set
            {
                _cashOutTran = value;
            }
        }

        public decimal ChecksMakeAvail
        {
            get
            {
                return _checksMakeAvail;
            }

            set
            {
                if (_checksMakeAvail != value)
                {
                    _checksMakeAvail = value;
                    NotifyPropertyChanged("ChecksMakeAvail");
                }
            }
        }

        private decimal _totalDebits;
        public decimal TotalDebits
        {
            get { return _totalDebits; }
            set
            {
                if (_totalDebits != value)
                {
                    _totalDebits = value;
                    NotifyPropertyChanged("TotalDebits");
                }
            }
        }

        private decimal _totalCredits;
        public decimal TotalCredits
        {
            get { return _totalCredits; }
            set
            {
                if (_totalCredits != value)
                {
                    _totalCredits = value;
                    NotifyPropertyChanged("TotalCredits");
                }
            }
        }

        private decimal _totalDifference;
        public decimal TotalDifference
        {
            get { return _totalDifference; }
            set
            {
                if (_totalDifference != value)
                {
                    _totalDifference = value;
                    NotifyPropertyChanged("TotalDifference");
                }
            }
        }
        private string _denominate; //#89998
        public string Denominate
        {
            get { return _denominate; }
            set
            {
                if (_denominate != value)
                {
                    _denominate = value;
                    NotifyPropertyChanged("Denominate");
                }
            }
        }

        private string _waiveSignature; //#90021
        public string WaiveSignature
        {
            get { return _waiveSignature; }
            set
            {
                if (_waiveSignature != value)
                {
                    _waiveSignature = value;
                    NotifyPropertyChanged("WaiveSignature");
                }
            }
        }
    }

    public enum EasyCaptureTranType
    {
        OpeningDeposit,
        Deposit,
        Withdrawal,      //93431
        OnUsCashedCheck,
        Payment,
        Advance,
        Transfer,
        CashCheck,
        Purchase,
        External
    }

    //Begin 87759
    public enum EasyCaptureTranCodeDesc
    {
        OpeningDeposit,
        Deposit,
        Withdrawal,
        CashedCheck,
        PaymentAutoSplit,
        PaymentAutoSplitPaytoZero,
        PrincipalPayment,
        LoanAdvance,
        CashCheck,
        CashiersCheck,
        MoneyOrder,
        TransferWithdrawal,
        AutoLoanPayment,
        TransferDeposit,
        ExternalCredit,
        ExternalDebit
    }
    //End 87759

    public enum EasyCaptureTranCode
    {
        [Description("Q01")]
        OpeningDeposit,
        [Description("Q02")]
        Deposit,
        [Description("Q11")]
        Withdrawal,
        [Description("Q12")]
        CashedCheck,
        [Description("Q21")]
        PaymentAutoSplit,
        [Description("Q22")]
        PaymentAutoSplitPaytoZero,
        [Description("Q23")]
        PrincipalPayment,
        [Description("Q31")]
        LoanAdvance,
        [Description("Q46")]
        CashCheck,
        [Description("Q47")]
        CashiersCheck,
        [Description("Q48")]
        MoneyOrder,
        [Description("Q36")]
        TransferWithdrawal,
        [Description("Q37")]
        AutoLoanPayment,
        [Description("Q38")]
        TransferDeposit,
        [Description("Q56")]
        ExternalCredit,
        [Description("Q57")]
        ExternalDebit
    }

    public enum EasyCaptureTranPmtOptions
    {
        PaymentAutoSplit,
        PaymentAutoSplitPaytoZero,
        PrincipalPayment
    }

    public enum EasyCaptureTranPurchaseOptions
    {
        CashiersCheck,
        MoneyOrder
    }

    //Begin 87759
    public enum EasyCaptureTranTfrOptions
    {
        TransferDeposit,
        TransferWithdrawal,
        AutoLoanPayment
    }
    //End 87759

    //Begin 87751
    public enum EasyCaptureTranExtOptions
    {
        ExternalCredit,
        ExternalDebit
    }
    //End 87751

    public enum EasyCaptureCheckType
    {
        Transit,
        OnUs,
        ChksAsCash
    }

    public class EasyCaptureTranResponse
    {
        public string ErrorType;
        public int ErrorCode;
        public string ErrorText;
        public string FocusField; //Bug #91947
    }



    public class EasyCaptureDenomination : EasyCaptureBase
    {
        private int _count;
        private decimal _amount;
        private DenomincationUnit _unit;

        public int Count
        {
            get { return _count; }
            set
            {
                if (_count != value)
                {
                    _count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        public Decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    NotifyPropertyChanged("Amount");
                }
            }
        }

        public DenomincationUnit Unit
        {
            get { return _unit; }
            set
            {
                if (_unit != value)
                {
                    _unit = value;
                    NotifyPropertyChanged("Unit");
                }
            }
        }

        protected override void NotifyPropertyChanged( string propertyName )
        {
            base.NotifyPropertyChanged(propertyName);
            if( propertyName != "Amount" && Unit != null)
            {
                Amount = Count * Unit.Multiplier;
            }
        }

    }

    public class DenomincationUnit
    {
        public DenomincationUnit(string name, decimal multiplier)
        {
            Name = name;
            Multiplier = multiplier;
        }
        public string Name { get; private set; }
        public decimal Multiplier { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class TranEditComponent
    {
        public bool ItemsEdited;
        public bool AmtEdited;
        public bool CashInEdited;
        public bool CashOutEdited;
        public bool AccountEdited;
        public bool TranCodeEdited;
        public bool MiscInfoEdited;
        public bool RecreateTran;
    }

    public class TransactionDefinition
    {
        public EasyCaptureTranType TranType { get; set; } 
        public string TranCode { get; set; }
        public string Description { get; set; }
        public string Combined { get; set; }
        public string DpLN { get; set; }
        //public Keys ShortcutKey { get; set; }  //87759

        public TransactionDefinition(string trancode, string description, string dpLn, EasyCaptureTranType tranType)  //87759
        {
            TranCode = trancode;
            Description = description;
            //Combined = $"{TranCode}-{Description}";
            Combined = string.Format("{0}-{1}", TranCode, Description);
            DpLN = dpLn;
            TranType = tranType;     
            //ShortcutKey = shortKey; //87759
        }
    }


    //Begin 87759
    public class TransactionTypeDefinition
    {
        string _menuDisplayDescription = null;//#101350

        public string Description { get; set; }
        public string TranType { get; set; }
        public string DpLN { get; set; }
        public Keys ShortcutKey { get; set; }
        public Keys SecondShortcutKey { get; set; } //// Task# 115764
        public string TranCode { get; set; }

        //Begin #101350
        public string MenuDisplayDescription
        {
            get
            {
                return _menuDisplayDescription ?? TranType;
            }
        }
        //End #101350
        //Mockup
        public TransactionTypeDefinition(string trancode, string description, string dpLn, string tranType, Keys shortKey, Keys? secondShortcut = null,string menuDisplayDescription = null)
        {
            TranCode = trancode;
            Description = description;
            TranType = tranType;
            DpLN = dpLn;
            ShortcutKey = shortKey;
            SecondShortcutKey = secondShortcut.Value;// Task# 115764
            _menuDisplayDescription = menuDisplayDescription;
        }
    }
    //End 87759

}
