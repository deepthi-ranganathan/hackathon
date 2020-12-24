using Phoenix.Windows.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Phoenix.Windows.Client
{
    //public class Transaction : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    // This method is called by the Set accessor of each property.  
    //    // The CallerMemberName attribute that is applied to the optional propertyName  
    //    // parameter causes the property name of the caller to be substituted as an argument.  
    //    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    private string _trancode;

    //    public string Trancode
    //    {
    //        get { return _trancode; }
    //        set { _trancode = value; }
    //    }

    //    public int MyProperty { get; set; 

    //}

   





    //public class CustomerTransaction:INotifyPropertyChanged
    //{
    //    private string _tc;

    //    public string TC
    //    {
    //        get { return _tc; }
    //        set {
    //            if (_tc != value)
    //            {
    //                _tc = value;
    //                NotifyPropertyChanged();
    //            }
    //        }


    //    }

    //    private string _dpLN;

    //    public string DpLN
    //    {
    //        get { return _dpLN; }
    //        set
    //        {
    //            if (_dpLN != value)
    //            {
    //                _dpLN = value;
    //                NotifyPropertyChanged();
    //            }
    //        }


    //    }



    //    private string _acct;
    //    public string Account
    //    {
    //        get { return _acct; }
    //        set
    //        {
    //            if (_acct != value)
    //            {
    //                _acct = value;
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

    //    private string _acctType;

    //    public string AcctType
    //    {
    //        get { return _acctType; }
    //        set
    //        {
    //            if (_acctType != value)
    //            {
    //                _acctType = value;
    //                Account = $"{AcctType}-{AcctNo}";
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }



    //    private string _acctNo;

    //    public string AcctNo
    //    {
    //        get { return _acctNo; }
    //        set
    //        {
    //            if (_acctNo != value)
    //            {
    //                _acctNo = value;
    //                Account = $"{AcctType}-{AcctNo}";
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

    //    private decimal _amount;

    //    public decimal Amount
    //    {
    //        get { return _amount; }
    //        set
    //        {
    //            if (_amount != value)
    //            {
    //                _amount = value;
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

    //    private string _tfrAccount;

    //    public string TfrAccount
    //    {
    //        get { return _tfrAccount; }
    //        set
    //        {
    //            if (_tfrAccount != value)
    //            {
    //                _tfrAccount = value;
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

    //    private string _tfrAcctType;

    //    public string TfrAcctType
    //    {
    //        get { return _tfrAcctType; }
    //        set
    //        {
    //            if (_tfrAcctType != value)
    //            {
    //                _tfrAcctType = value;
    //                TfrAccount = $"{TfrAcctType}-{TfrAcctNo}";
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }


    //    private string _tfrAcctNo;

    //    public string TfrAcctNo
    //    {
    //        get { return _tfrAcctNo; }
    //        set
    //        {
    //            if (_tfrAcctNo != value)
    //            {
    //                _tfrAcctNo = value;
    //                TfrAccount = $"{TfrAcctType}-{TfrAcctNo}";
    //                NotifyPropertyChanged();
    //            }
    //        }
    //    }

        
        
        

    //    private TransactionDefinition _transactionDefinition;

    //    public event PropertyChangedEventHandler PropertyChanged;


    //    // This method is called by the Set accessor of each property.  
    //    // The CallerMemberName attribute that is applied to the optional propertyName  
    //    // parameter causes the property name of the caller to be substituted as an argument.  
    //    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }

    //    public TransactionDefinition TranDef
    //    {
    //        get { return _transactionDefinition; }
    //        set {
    //            if (_transactionDefinition != value)
    //            {
    //                _transactionDefinition = value;
    //                if (_transactionDefinition != null)
    //                    this.TC = _transactionDefinition.Combined;
    //                else
    //                    this.TC = null;
    //                NotifyPropertyChanged();
    //            }
    //        }


    //    }




    //}


    //public class ChecksInfo
    //{

    //    public string AcctNo { get; set; }
    //    public decimal Amount { get; set; }
    //    public string RoutingNo { get; set; }

    //    public EasyCaptureCheckType CheckType { get; set; }

    //}

    //public class TransactionSet
    //{
    //    ObservableCollection<CustomerTransaction> _customerTransaction = new ObservableCollection<CustomerTransaction>();
    //    List<Denomination> _cashIn = new List<Denomination>();
    //    List<Check> _cashIn = new List<Denomination>();


    //    public Decimal TotalDebits { get; set; }

    //    public Decimal TotalCredits { get; set; }

    //    public Decimal TotalDifference { get; set; }


        

    //    public List<Denomination> CashIn
    //    {
    //        get { return _cashIn; }

    //    }
    //}


}
