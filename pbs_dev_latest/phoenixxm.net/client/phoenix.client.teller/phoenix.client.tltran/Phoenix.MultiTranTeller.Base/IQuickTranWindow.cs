using Phoenix.MultiTranTeller.Base.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using System.Collections; //87757
//using Phoenix.BusObj.Teller;    //#92886

namespace Phoenix.MultiTranTeller.Base
{
    public interface IQuickTranWindow
    {
        event Action<bool> OnSearchPerformed; //Bug #92023

        EasyCaptureTranSet TranSet { get; }

        void FocusInitialField(); //Bug #114526

        void SetTlTranCodeWindow(ITlTranCodeWindow tlTranCodeWindow);

        void TlTranCodeWindowCloseNotify();

        void ChildWindowClosedNotify( Form form);

        //EasyCaptureTran GetSelectedTran();

        void InitializeTranSet();  //#87731

        void RefreshQuickTran(string source);    //#102719

        void SetRMSInformation(int RimNo, string Account);  //#87725

        void SetCalcValue2GridColumn(Control Grid, decimal calcValue); //#89907

        EasyCaptureTran GetCurrentEasyCaptureTran();

        EasyCaptureTran GetCurrentEasyCaptureTran(int tranId);

        void ResetCurrentEasyCaptureTran();
        void SaveCurrentLayout();
        void RestoreDefaultLayout();

        PAction ActionSave { get; set; }

        void SetGetAcctValue(string acctType, string acctNo);  //87757

        void GetTfrDepLoan(string acctType, ref string deploan);  //87759

        void Destroy(); //Bug#93072

        void FocusProblematicField(EasyCaptureTranResponse tranResponse, int rowIndex);

        bool ValidateTransactions(); //Task #103255

        int GetCurrentRim();

        void RefreshCurrentRimInfo(); //Bug #100897

        int CurTranId { get; set; }

        bool AssumeDecimals { get; set; }

        decimal DeviceDeposit { get; set; }    //#89998

        string GetTransactionGridFocusedColumn();

        Task<bool> SearchCustomer(int RimNo);

        void DeleteEmptyTranRow();  //#124249
    }

    public interface ITlTranCodeWindow
    {
        bool MapEasyCaptureToTranSet(bool populateViewTran, bool fromAsyncCall, bool showErrors, bool showOnlyFirstErrorMessage);

        void PerformQuickTranActionClick(string actionName, params object[] paramList); //#87731

        void PerformEnableDisableControls(string origin); //#87743

        void PopulateRMSWithQuickTranInfo(int rimNo); //#89899
    
        void SetCalcControl(object sender); //#89907

        //Begin Task#89976
        void EnableDisableQuickButtons(bool enableButtons);
        DataRow QuickAccountSelectedRow { get; set; }
        //End Task#89976
        string GetSelectSecondOwner(); //Bug#97688

        void CheckFraud(); //Task#97603

        void setTitleBarName(int rimNo, bool pushRimIntoTran);//Bug#103638

        void setTitleBar(string rimName); //Bug# 108552 

        int RmAcctPtid { get; set; } //Task#89967

        //Begin Task#89981
        string CrossRefSearchAcct { get; set; }
        string CrossRefSearchMapAcctNo { get; set; }
        string CrossRefSearchMapAcctType { get; set; }
        //End Task#89981
        bool isTransactionGridPopulated { get; set; }  //Bug #109821

        //Begin 87757
        void GetTfrAcctTypes(ref ArrayList TfrAcctTypeList); 
        void ActionRequest(string req, string value, ref string reqResultStr);
        //End 87757

        int GetCurrentSelectedTabIndex();

        void SetCurrentHoldBalInfo(object hardHoldObj); //#87767

        object GetCurrentHardHoldBalInfo(); //#87767

        void QuickTranDeviceDeposit();  //#89998

        void QuickTranReverseDeviceDeposit();  //#89998

        void QuickTranTcdDrawerIconToggle(PictureBox picTempTCD, PictureBox picTempDrawer); //#89998

        void HandleQuickTranStatusText(string statusText);  //#90021
    }

}
