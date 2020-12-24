
using Phoenix.MultiTranTeller.Base.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Phoenix.MultiTranTeller.Base.ViewModels
{
  


  

    public class EasyCaptureTranSetViewModel: EasyCaptureTranSet
    {

      

        public EasyCaptureTranSetViewModel()
        {
            Transactions.CollectionChanged += _transactions_CollectionChanged;
            //Checks.CollectionChanged += _checks_CollectionChanged;
            //CashIns.CollectionChanged += _cashIns_CollectionChanged;           

        }
    

        private void _cashIns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if( e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach(EasyCaptureDenomination item in e.NewItems)
                {
                    item.PropertyChanged += CashInItem_PropertyChanged;
                }
            }
            CalcCashInTotals();
        }

        private void CashInItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if( e.PropertyName == "Amount")
                CalcCashInTotals();
        }

        private void CalcCashInTotals()
        {
            decimal cashInSum = 0;
            foreach (EasyCaptureDenomination cashIn in CashIns)
            {
                cashInSum += cashIn.Amount;
            }
            CashIn = cashInSum;
            //CalcTranTotals();
        }

        private void CalcChecksTotals()
        {
            decimal sumOfChecks = 0;

            foreach (EasyCaptureChecks check in Checks)
            {
                sumOfChecks = check.Amount;
            }
            ChecksIn = sumOfChecks;
            //CalcTranTotals();
        }


        private void _checks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (EasyCaptureChecks item in e.NewItems)
                {
                    item.PropertyChanged += ChecksItem_PropertyChanged; ;
                }
            }
        }

        private void ChecksItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Amount")
                CalcChecksTotals();
        }

        private void _transactions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (EasyCaptureTran item in e.NewItems)
                {
                    item.PropertyChanged += TranItem_PropertyChanged;
                }
            }
            //CalcTranTotals();
            
        }

        //private void CalcTranTotals()
        //{
        //    //decimal totalCR = CashOut;
        //    //decimal totalDR = CashIn + ChecksIn;

        //    //foreach (EasyCaptureTran tran in Transactions)
        //    //{
        //    //    if (tran.TranType == EasyCaptureTranType.Deposit || tran.TranType == EasyCaptureTranType.Payment)
        //    //        totalCR += tran.Amount;
        //    //    if (tran.TranType == EasyCaptureTranType.Withdrawl || tran.TranType == EasyCaptureTranType.Advance)
        //    //        totalDR += tran.Amount;
        //    //    if (tran.TranType == EasyCaptureTranType.Transfer)
        //    //    {
        //    //        totalCR += tran.Amount;
        //    //        totalDR += tran.Amount;
                    
        //    //    }
        //    //}

        //    //TotalCredits = totalCR;
        //    //TotalDebits = totalDR;
        //    //TotalDifference = totalDR - totalCR;
        //}


        private void TranItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            


        }

        protected override void NotifyPropertyChanged(string propertyName)
        {
            base.NotifyPropertyChanged(propertyName);
            //if (propertyName == "CashIn" || propertyName == "CashOut" || propertyName == "ChecksIn")
            //    CalcTranTotals();
        }


       

  
        public EasyCaptureTran AddNewTransaction()
        {
            var t = new EasyCaptureTran();
            Transactions.Add(t);
            return t;
        }

    }
}
