#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2013 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlCaptureBulkTran.cs
// NameSpace: Phoenix.Client.TlTranCode
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//12/30/2013 1:52:50 PM			1		spatterson  Created
//01/25/2016                    2       rpoddar     #40977 - AVTC OOB Fixes
//07/24/2017                    3       mselvaga    #68911 - 209688 - 11 - AVTC10 - Client 10.x to 10.x - Bulk Tran Window not Sized Correctly
//05/26/2019                    4       mselvaga    Task#111431 - Enable client workstation property changes for Citrix - JIRA 376
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Variables;
using Phoenix.Windows.Client;

// * NOTE TO PROGRAMMER ****************************************************************************************//
// This is the basic structure of a Phoenix .NET window. Several points of interest are marked with "TODO".
// Template windows have been created as an instructional guide (http://teamwork.harlandfs.com/team/shark/Lists/FAQ/DispForm.aspx?ID=62)
// Additional information can be found here: http://teamwork.harlandfs.com/team/phoenixdev/Programmer%20Resources/Home.aspx
//
// TODO: Add as "link" the file pbs_dev_latest\phoenixxm.net\common\commonassembly.cs
// TODO: Change the file name and references, both here, in the designer file, and in the comments section
// *************************************************************************************************************//
namespace Phoenix.Client.Teller
{
    public partial class frmTlCaptureBulkTran : PfwStandard
    {
        #region Private Variables

        private PString _psBulkTranXml = new PString("BulkTranXml");
        private PInt _pnBranchNo = new PInt("BranchNo");
        private PInt _pnDrawerNo = new PInt("DrawerNo");
        private PDecimal _pnBatchId = new PDecimal("BatchId");
        private PString _psMode = new PString("Mode"); /* Post/ViewOnly */

        private TlCaptureBulkTran _tlCaptureBulkTran;
        private TlCaptureScanItemsCollection _bulkScanItems;

        //private frmTlTranCode tranCodeWin;
        private PfwStandard _tranCodeWin = null;
        private int tranSetId;
        private string _batchID;
        private string _tranSetID;
        private int _contextRow;
        //private bool closedWkspace = false;

        /// <summary>
        /// This enum contains the various conditions which will enable/disable push buttons
        /// </summary>
        private enum EnableDisableVisible 
        {
            InitBegin,
            InitComplete,
            GridClick
        }

        private enum CallOtherForm 
        {
            EditClick,
            ProcessTran
        }
        #endregion

        #region Constructors
        public frmTlCaptureBulkTran()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Events
        /// <summary>
        /// Event executed before form is populated with data
        /// </summary>
        /// <returns></returns>
        private ReturnType form_PInitBeginEvent() 
        {
            // todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureBulkTran;

            // To make sure any changes made to values in a bus obj are displayed correctly on parent screens, make sure you set the UseStateFromBusinessObject 
            this.UseStateFromBusinessObject = true;
            // set business object for edit/select/new/update for this form. Must happen after UseStateFromBusinessObject
            // this.MainBusinesObject = [BO];

            grdBulkTransactions.BeforePopulate += new GridPopulateEventHandler(grdBulkTransactions_BeforePopulate);
            grdBulkTransactions.FetchRowDone += new GridRowFetchDone(grdBulkTransactions_FetchRowDone);

            _tlCaptureBulkTran = new TlCaptureBulkTran();
            _bulkScanItems = new TlCaptureScanItemsCollection();

            ActionSave.Visible = false;

            return default(ReturnType);
        }

        private void grdBulkTransactions_FetchRowDone(object sender, GridRowArgs e)
        {
            if (colTranStatus.Text == "4")
                colReversal.Text = Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y;

            if (!string.IsNullOrWhiteSpace(colTranCode.Text))
            {
                colDescription.Text = colTranCode.Text;

                if (!string.IsNullOrWhiteSpace(colTranDescription.Text))
                    colDescription.Text += " - " + colTranDescription.Text;
            }

            colStatus.Text = GetTranStatusFromValue(colTranStatusValue.Text);
        }

        private void grdBulkTransactions_BeforePopulate(object sender, GridPopulateArgs e) 
        {
            _tlCaptureBulkTran.ResponseTypeId = 10;
            grdBulkTransactions.ListViewObject = _tlCaptureBulkTran;

            Filter includeProcessed = new Filter("IncludeProcessed", "N");

            includeProcessed.Value = cbIncludeProcessedBulk.Checked ?
                Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.Y :
                    Phoenix.FrameWork.Shared.Variables.GlobalVars.Instance.ML.N;

            if (grdBulkTransactions.Filters.Count > 0)
                grdBulkTransactions.Filters.Clear();

            grdBulkTransactions.Filters.Add(includeProcessed);
        }

        private void grdBulkTransactions_AfterPopulate(object sender, GridPopulateArgs e) 
        {
            EnableDisableVisibleLogic(EnableDisableVisible.GridClick);
        }

        private void grdBulkTransactions_Click(object sender, EventArgs e) 
        {
            EnableDisableVisibleLogic(EnableDisableVisible.GridClick);
        }

        private void frmTlCaptureBulkTran_PInitCompleteEvent() 
        {
            if (_psMode.Value == "ViewOnly")
            {
                _tlCaptureBulkTran.BranchNo.Value = Convert.ToInt16(_pnBranchNo.Value);
                _tlCaptureBulkTran.DrawerNo.Value = Convert.ToInt16(_pnDrawerNo.Value);
                _tlCaptureBulkTran.TlCaptureWorkstation.Value = TellerVars.Instance.TlCaptureWorkstation;   //#111431

                ActionClose.Visible = true;
                pbProcess.Enabled = false;
                pbReverse.Enabled = false;

                grdBulkTransactions.PopulateTable();

                return;
            }
            else
            {
                ActionClose.Visible = false;
                pbProcess.Enabled = true;
                pbReverse.Enabled = true;
            }

            int errorId = 0;
            int batchId = (_pnBatchId.IsNull ? 0 : Convert.ToInt32(_pnBatchId.Value));
            string errorString = string.Empty;

            if (_bulkScanItems.LoadFromXML(_psBulkTranXml.Value) && 
                _pnBatchId.Value <= 0)
            {
                _tlCaptureBulkTran.CreateBulkTranSets(
                    _psBulkTranXml.Value,
                    _pnBranchNo.Value,
                    _pnDrawerNo.Value,
                    TellerVars.Instance.PostingDt,
                    TellerVars.Instance.TlCaptureWorkstation,
                    out errorId,
                    out errorString,
                    out batchId);   //#111431

                //if (string.IsNullOrWhiteSpace(errorString))
                //{
                //    _tlCaptureBulkTran.BatchId.Value = batchId;
                //    _tlCaptureBulkTran.BranchNo.Value = Convert.ToInt16(_pnBranchNo.Value);
                //    _tlCaptureBulkTran.DrawerNo.Value = Convert.ToInt16(_pnDrawerNo.Value);

                //    grdBulkTransactions.PopulateTable();
                //}
            }


            if (string.IsNullOrWhiteSpace(errorString))
            {
                _tlCaptureBulkTran.BatchId.Value = batchId;
                _tlCaptureBulkTran.BranchNo.Value = Convert.ToInt16(_pnBranchNo.Value);
                _tlCaptureBulkTran.DrawerNo.Value = Convert.ToInt16(_pnDrawerNo.Value);
                _tlCaptureBulkTran.TlCaptureWorkstation.Value = TellerVars.Instance.TlCaptureWorkstation;   //#111431

                grdBulkTransactions.PopulateTable();
            }

            //if (this.Workspace != null)
            //{
            //    (this.Workspace as Form).FormClosing += new FormClosingEventHandler(frmTlCaptureBulkTran_FormClosing);
            //}

            SetFocusOnForm();
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        public override void OnCreateParameters() 
        {
            Parameters.Add(_psBulkTranXml);
            Parameters.Add(_pnBranchNo);
            Parameters.Add(_pnDrawerNo);
            Parameters.Add(_pnBatchId);
            Parameters.Add(_psMode);
            
            base.OnCreateParameters();
        }

        /// <summary>
        /// Perform additional actions during the save process
        /// </summary>
        /// <param name="isAddNext"></param>
        /// <returns></returns>
        public override bool OnActionSave(bool isAddNext) 
        {
            return base.OnActionSave(isAddNext);
        }

        /// <summary>
        /// Perform actions when closing a form
        /// </summary>
        /// <returns></returns>
        public override bool OnActionClose() 
        {
            bool bRet = true;

            if (bRet)
            {
                // Validations
            }

            if (bRet)
            {
                bRet = base.OnActionClose();
            }

            if (bRet)
            {
                // Refresh Parent Window
                // CallParent( this.ScreenId );
            }

            return bRet;
        }

        /// <summary>
        /// Called by child when parent needs to perform an action
        /// </summary>
        /// <param name="paramList"></param>
        public override void CallParent(params object[] paramList) 
        {
            int callerScreenId = Convert.ToInt32(paramList[0]);
            bool hasPendingBatch = true;
            
            if (callerScreenId == Phoenix.Shared.Constants.ScreenId.TlTranCode)
            {
                string tranStatus = string.Empty;
                string postStatus = paramList[1].ToString();
                // TranStatus/PostStatus Values
                // 1 - "Normal Posted"
                // 2 - "Force Posted"
                // 3 - "Failed"
                if (!string.IsNullOrEmpty(postStatus))                
                    tranStatus = postStatus;
                //if (postStatus == "Normal Posted")
                //{
                //    tranStatus = "1";
                //}
                //else if (postStatus == "Force Posted")
                //{
                //    tranStatus = "2";
                //}
                //else if (postStatus == "Failed")
                //{
                //    tranStatus = "3";
                //}

                grdBulkTransactions.ContextRow = _contextRow;

                if (tranStatus == "1" || tranStatus == "2" || tranStatus == "3")
                {
                    _batchID = colBatchId.Text.Replace(",", "");
                    _tranSetID = colTranSetId.Text.Replace(",", "");

                    _tlCaptureBulkTran.UpdateTranSetStatus(
                        Convert.ToInt32(_batchID),
                        Convert.ToInt32(_tranSetID),
                        tranStatus,
                        colRecordSource.Text,
                        0);

                    // if BatchId is not pending ( != 0 ) delete batch
                    hasPendingBatch = _tlCaptureBulkTran.BatchHasPending(Convert.ToInt32(_batchID));
                    if (!hasPendingBatch)
                        _tlCaptureBulkTran.DeleteBatch(Convert.ToInt32(_batchID));
                }
             
                grdBulkTransactions.PopulateTable();

                if (tranStatus != "3" || hasPendingBatch == false)
                    ProcessCurTranSet();
                else
                    SetFocusOnForm();

            }

            base.CallParent(paramList);
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

        #endregion

        #region Methods
        /// <summary>
        /// This contains the various conditions which will enable/disable push buttons
        /// </summary>
        /// <param name="caseName"></param>
        private void EnableDisableVisibleLogic(EnableDisableVisible caseName) 
        {
            // Note: If you need to enable/disable fields based on business rules, this must
            // be done in the business object.

            switch (caseName)
            {
                case EnableDisableVisible.InitComplete:
                {
                    if (_psMode.Value == "ViewOnly")
                    {
                        pbProcess.Enabled = false;
                        pbReverse.Enabled = false;
                    }

                    break;
                }
                case EnableDisableVisible.InitBegin:
                {
                    break;
                }
                case EnableDisableVisible.GridClick:
                {
                    if (_psMode.Value != "ViewOnly")
                        pbReverse.Enabled = colTranStatus.Text != "4";
                    
                    break;
                }
            }
        }

        /// <summary>
        /// Open new form windows
        /// </summary>
        /// <param name="caseName"></param>
        private void CallOtherForms(CallOtherForm caseName) 
        {
            PfwStandard tempWin = null;
            PComboBoxStandard cmbDrawer = null;
            PAction actionDummyTCD = null;

            switch (caseName)
            {
                case CallOtherForm.EditClick:
                    // TODO: invoke new window using the following format
                    // tempWin = CreateWindow("{ASSEMBLYNAME}", "{NAMESPACE}", "{FORMNAME}");
                    // tempWin.InitParameters(PhoenixVariable, PhoenixVariable....);
                    break;
                case CallOtherForm.ProcessTran:
                    if (this.Workspace.Variables["_DrawerComboObj"] != null)
                        cmbDrawer = (PComboBoxStandard)this.Workspace.Variables["_DrawerComboObj"];
                    if (this.Workspace.Variables["_TCDActionObj"] != null)
                        actionDummyTCD = (PAction)this.Workspace.Variables["_TCDActionObj"];
                    tempWin = CreateWindow("Phoenix.Client.TlTrancode", "Phoenix.Windows.Client", "frmTlTranCode");
                    tempWin.InitParameters(cmbDrawer, null, actionDummyTCD, string.Empty, null);
                    _tranCodeWin = tempWin;
                    break;
            }

            if (_tranCodeWin != null)
            {
                _tranCodeWin.Workspace = this.Workspace;
                _tranCodeWin.FormClosed += new FormClosedEventHandler(_tranCodeWin_FormClosed);
            }

            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                // TODO: if you want a grid to refresh on this form when this window is closed,
                // set the following property: tempWin.ParentGrid = this.grid;	
                tempWin.Show();
            }
        }

        void _tranCodeWin_FormClosed(object sender, FormClosedEventArgs e)
        {
            _tranCodeWin = null;
        }

        #endregion

        private void cbIncludeProcessedBulk_CheckedChanged(object sender, EventArgs e) 
        {
            grdBulkTransactions.PopulateTable();
        }

        private void pbReverse_Click(object sender, PActionEventArgs e) 
        {
            // 14567 - Are you sure you want to reverse the selected transaction set and delete the images from AVTC?
            if (PMessageBox.Show(14567, MessageType.Warning, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            _batchID = colBatchId.Text.Replace(",", "");
            _tranSetID = colTranSetId.Text.Replace(",", "");

            _tlCaptureBulkTran.UpdateTranSetStatus(
                Convert.ToInt32(_batchID),
                Convert.ToInt32(_tranSetID),
                "4", // Reversal
                colRecordSource.Text,
                0);

            // if BatchId is not pending ( != 0 ) delete batch
            if (!_tlCaptureBulkTran.BatchHasPending(Convert.ToInt32(_batchID)))
                _tlCaptureBulkTran.DeleteBatch(Convert.ToInt32(_batchID));

            grdBulkTransactions.PopulateTable();

            DoCloseCheck();
        }

        private void pbProcess_Click(object sender, PActionEventArgs e)
        {
            //if (_tranCodeWin != null)
            //    _tranCodeWin.Dispose();

            //tranCodeWin = new frmTlTranCode();
            //Begin #40977
            if (!TellerVars.Instance.IsTellerCaptureEnabled)
                return;
            //End #40977

            ProcessCurTranSet();
        }

        private void ProcessCurTranSet() 
        {
            var curTranScanItems = new TlCaptureScanItemsCollection();
            int tranStatusColumnId = colTranStatus.ColumnId;
            int captureIsnColumnId = colIsnNo.ColumnId;
            int tranSetIdColumnId = colTranSetId.ColumnId;            
            int SeqNoColumnId = colSeqNo.ColumnId;
            int curTranSetId = 0;
            int bulkTranSeqNo = -1;
            _contextRow = 0;

            tranSetId = -1;

            for (int x = 0; x < grdBulkTransactions.Items.Count; x++)
            {
                int tranStatus = Convert.ToInt32(grdBulkTransactions.Items[x].SubItems[tranStatusColumnId].Text);                 
                //curTranSetId = Convert.ToInt32(colTranSetId.Text);
                if (grdBulkTransactions.Items[x].SubItems[tranSetIdColumnId] != null)
                {
                    curTranSetId = Convert.ToInt32(grdBulkTransactions.Items[x].SubItems[tranSetIdColumnId].Text.Replace(",", ""));
                }

                if (tranStatus == 0 || tranStatus == 3) // Pending or Failed
                {
                    TlCaptureScanItem scanItem = 
                        _bulkScanItems.GetScanItemByISN(
                            grdBulkTransactions.Items[x].SubItems[captureIsnColumnId].Text);

                    if (tranSetId < 0)
                        tranSetId = curTranSetId;

                    if (scanItem != null)
                    {
                        //curTranScanItems.Add(scanItem);
                        //curTranSetId = Convert.ToInt32(colTranSetId.Text);
                        if (tranSetId != curTranSetId)
                            break;
                        else
                        {
                            _contextRow = x;
                            curTranScanItems.Add(scanItem);
                            bulkTranSeqNo = Convert.ToInt32(grdBulkTransactions.Items[x].SubItems[SeqNoColumnId].Text);
                        }
                        tranSetId = curTranSetId;
                    }
                }
            }

            if (!DoCloseCheck())
                return;

            if (_tranCodeWin == null)
                CallOtherForms(CallOtherForm.ProcessTran);

            SendProcessMessage("OnTransmitForPost", curTranScanItems.ToXML(false, false), true, bulkTranSeqNo);
        }

        private bool SendProcessMessage(string messageType, params object[] paramList)
        {
            if (_tranCodeWin != null)
            {
                return _tranCodeWin.ProcessMessage(
                    (int)GlobalActions.TellerCapture,
                    this,
                    null,
                    messageType,
                    paramList);
            }
            return false;
        }

        private bool HasPendingBatches() 
        {
            return _tlCaptureBulkTran.OtherInfo["HasPendingBatches"].BooleanValue;
        }

        private bool DoCloseCheck() 
        {
            if (!HasPendingBatches())
            {
                if (_tranCodeWin != null)
                    _tranCodeWin.Close();

                _tlCaptureBulkTran.DeleteBatch(Convert.ToInt32(_tlCaptureBulkTran.BatchId.Value));

                this.Close();

                return false;
            }
            return true;
        }

        private string GetTranStatusFromValue(string text) 
        {
            switch (text)
            {
                case "0":
                {
                    return "Pending";
                }

                case "1":
                {
                    return "Normal Posted";
                }

                case "2":
                {
                    return "Force Posted";
                }

                case "3":
                {
                    return "Failed";
                }

                case "4":
                {
                    return "Reversed";
                }

                default:
                {
                    return string.Empty;
                }
            }
        }

        private void SetFocusOnForm()
        {
            int WM_SETFOCUS = 0x0007;
            int lParam = 0;
            PostMessage(this.Handle, WM_SETFOCUS, 0, ref lParam);     // WM_SETFOCUS = 0x0007
        }

        //void frmTlCaptureBulkTran_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        //{
        //    if (!closedWkspace && !e.Cancel)
        //    {
        //        WorkSpace_Close(sender, e);
        //        if (!e.Cancel)
        //            closedWkspace = true;
        //    }
        //}

        //void WorkSpace_Close(object sender, CancelEventArgs e)
        //{
        //    if (TellerVars.Instance.IsTellerCaptureEnabled && this.pbProcess.Visible)
        //    {
        //        int bulkScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureBulkTran;  //TODO - Fix the screen id for frmTlCaptureBulkTran
        //        if (Workspace != null && Workspace.Screens.ContainsKey(bulkScreenId) && Convert.ToInt16(Workspace.Screens[bulkScreenId]) != 0)
        //        {
        //            if (e != null)
        //            {
        //                e.Cancel = true;
        //                return;
        //            }
        //        }

        //    }
        //    base.OnClosing(e);
        //    return;
        //}
    }
}
