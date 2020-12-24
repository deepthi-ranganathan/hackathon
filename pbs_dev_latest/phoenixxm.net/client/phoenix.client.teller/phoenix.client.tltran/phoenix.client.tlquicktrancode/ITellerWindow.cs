using Phoenix.MultiTranTeller.Base.ViewModels;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Global;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Phoenix.Client.Teller
{
    public interface ITellerWindow
    {
        
        DataTable Customers { get; set; }
        DataTable Accounts { get; set; }
        DataTable CustomerRelations { get; set; }
        DataTable AcctRelations { get; set; }
        Control LastActiveControl { get; set; }

        PAction ActionSave { get; set; }


        event CustomerInfoChanged OnCustomerChanged;

        EasyCaptureTranSetViewModel TransactionSet { get; }
        Task<CustomerInfoResponse> PopulateCustomerInfo(int RimNo);
        Task<CustomerInfoResponse> PopulateCustomerInfo(string acctNo); //Task#Task 87723
        DataTable GetDetailCustomerByAccountSync(string AcctNo, string acctType);
        void PopulateAccountRelationship(string acctType, string acctNo); 
        void PopulateCustomerRelationship(int rimNo);
        void MoveAcctToTran(object selectedMenuItem);

        void SetTransactionMenuIndicator();

        void ClearInformation();

        AcctAlerts GetAlertInfo(string acctType, string acctNo);

        void SetTransactionAccountInfo(string accType =null, string acctNo=null); //Task#94607/94608 Bug #109821
        void SetTransactionAccountInfo(string accType = null, string acctNo = null, bool fromTCDeleteRows = false); //Task#94607/94608 Bug #109821 #124210

        IPhoenixWorkspace CurWkspace { get; }

        bool AssumeDecimals { get; }

        Phoenix.MultiTranTeller.Base.ITlTranCodeWindow TlTranCodeWindow { get; }

        void PerformQuickTranActionClick(string actionName, bool fromEnterKey, params object[] paramList); //#87731 #124249

        void QuickTranDeviceDeposit();  //#89998

        void QuickTranReverseDeviceDeposit();  //#89998

        void QuickTranTcdDrawerIconToggle(PictureBox picTempTCD, PictureBox picTempDrawer); //#89998

        void PerformEnableDisableControls(string origin);

        void ChildControlFocused(object sender); //#89907

        void SetCurrentTran(string description, string trancode);

        bool IsQuickTranRealTime { get; }

        ////Begin Task#100769
        bool ConfAcctSecurity { get; }
        bool UserConfidentialAccess { get; }
        ////End Task#100769

        void SetQuickAccountSelectedRow(DataRow row); //Task#89976

        int GetRmAcctPtid(int rmAcctPtid); //Task#89967

        //Begin 87757
        string[] TfrAcctList { get; set; } 
        void ActionRequest(string req, string value, ref string reqResultStr);

        //End 87757

        
        //void setTitleBarName(int rimNo, bool pushRimIntoTran);//Bug#103638

        //void setTitleBar(string rimName); //Bug# 108552 

        void MapEasyCaptureToTranSetAsync(bool populateViewTran, string triggerSource, Action callback = null);

        void CallOtherForms(string origin);

        bool AllowHardHold { get; }

        int CurTranId { get; set; }


        void HandleTCDDrawerIcon();   //#89998

        decimal DeviceDeposit { get; }  //#89998

        void HandleWaiveSignature();   //#90021

        void HandleQuickTranStatusText(string statusText);  //#90021

        bool IsFocusOnPanel(); //122356
    }

    public delegate void CustomerInfoChanged();

    public delegate void UserControlEvent(string eventName, object affectedObj);

    //Begin Bug #92488
    public class CustomerInfoResponse
    {
        public bool Success { get; set; }
        public int RowCount { get; set; }
        public List<string> AccountNumbers { get; set; } //Bug #92489
        public int RimNo { get; set; } //Bug #92489
        public bool UserCanceled { get; set; } //Bug #96236/96237/96238
    }
    //End Bug #92488
}