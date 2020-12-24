#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2020 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: $fileinputname$.cs
// NameSpace: $rootnamespace$
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//11/20/2020 5:58:57 PM			1		kiran.mani  Created
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
using System.Xml;
using Phoenix.FrameWork.Core;
using Microsoft.SqlServer.Server;

// * NOTE TO PROGRAMMER ****************************************************************************************//
// This is the basic structure of a Phoenix .NET window. Several points of interest are marked with "TODO".
// If you are using an editable ListView, please uncomment the region titled "Uncomment If Using Editable Grid"
// Template windows have been created as an instructional guide (http://teamwork.harlandfs.com/team/shark/Lists/FAQ/DispForm.aspx?ID=62)
// Additional information can be found here: http://teamwork.harlandfs.com/team/phoenixdev/Programmer%20Resources/Home.aspx
//
// TODO: Add as "link" the file pbs_dev_latest\phoenixxm.net\common\commonassembly.cs
// TODO: Change the file name and references, both here, in the designer file, and in the comments section
// *************************************************************************************************************//
namespace phoenix.client.CashReward
{
    public partial class frmCashRwdInquiry : PfwStandard
    {
        #region Private Variables

        Phoenix.BusObj.Misc.CashReward _CashReward = null;

        private PInt _pnRimNo = new PInt("_pnRimNo");

        /// <summary>
        /// This enum contains the various conditions which will enable/disable push buttons
        /// </summary>
        private enum EnableDisableVisible
        {
            InitBegin,
            InitComplete,
            BeforePopulate,
            AfterPopulate,
            RowSelected
        }

        private enum CallOtherForm
        {
            EditClick,
            HistoryClick
        }
        #endregion

        #region Constructors
        public frmCashRwdInquiry()
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
            _CashReward = new Phoenix.BusObj.Misc.CashReward();
            _CashReward.RimNo.Value = _pnRimNo.IntValue;
            _CashReward.ResponseTypeId = 11;

            this.UseStateFromBusinessObject = true;
            this.MainBusinesObject = _CashReward;
            (this.Extension as Phoenix.Shared.Windows.FormExtension).ResizeFormEnd += FrmCashRwdInquiry_ResizeFormEnd; 
            gridView1.ShowingEditor += GridView1_ShowingEditor;
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;
            return default(ReturnType);
        }

        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string _status = Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colStatus)).Trim();
            if(_status == "Dormant" || _status == "Locked" || _status == "Escheated")
            {
                colRedAMt.OptionsColumn.AllowFocus = false;
            }
            else
            {
                colRedAMt.OptionsColumn.AllowFocus = true;
            }
            gridView1.RefreshRowCell(e.FocusedRowHandle,colRedAMt);
        }

        private void FrmCashRwdInquiry_ResizeFormEnd(object sender, EventArgs e)
        {
            this.gbCashRwdRedmption.Height = Convert.ToInt32((Workspace as PwksWindow).Height * 0.84);
            this.gridControl1.Height = Convert.ToInt32((Workspace as PwksWindow).Height * 0.81);
        }

        private void GridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            string _status =Convert.ToString(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, colStatus)).Trim();
            if ((sender as DevExpress.XtraGrid.Views.Grid.GridView) != null &&
                (sender as DevExpress.XtraGrid.Views.Grid.GridView).FocusedColumn != null &&
                (sender as DevExpress.XtraGrid.Views.Grid.GridView).FocusedColumn.FieldName == "AmtRedeemed" &&
                (_status == "Dormant" || _status == "Locked" || _status== "Escheated"))
            {
                e.Cancel = true;
            }
        }

        private void frmCashRwdInquiry_PInitCompleteEvent()
        {
            this.dfCashRewardEarnedLtd.UnFormattedValue = _CashReward.ObjectState[dfCashRewardEarnedLtd.XmlTag].Value;
            this.dfCashRewardAvailable.UnFormattedValue = _CashReward.ObjectState[dfCashRewardAvailable.XmlTag].Value;
            this.dfCashRewardRedeemedLtd.UnFormattedValue = _CashReward.ObjectState[dfCashRewardRedeemedLtd.XmlTag].Value;
            pbRedeem.Enabled = (!CoreService.UIAccessProvider.IsSecurityEnabled || CoreService.UIAccessProvider.HasWriteAccess(Phoenix.Shared.Constants.ScreenId.CashRwdInquiry));
            PopulateGrid();
            this.EnableDisableVisibleLogic(EnableDisableVisible.InitComplete);
        }

        #endregion

        #region Overriddes
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        public override void OnCreateParameters()
        {
            Parameters.Add(_pnRimNo);

            base.OnCreateParameters();
        }

        /// <summary>
        /// Perform additional actions during the save process
        /// </summary>
        /// <param name="isAddNext"></param>
        /// <returns></returns>
        public override bool OnActionSave(bool isAddNext)
        {
            // todo: perform additional actions before calling base method
            return true;
            //return base.OnActionSave(isAddNext);
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
            // todo: Perform actions when called from a child window. 
            // ScreenId is the first parameter of paramList

            base.CallParent(paramList);
        }

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
                        if(gridView1.RowCount >0)
                        {
                            this.pbRedeem.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                        }
                        else
                        {
                            this.pbRedeem.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                        }

                    }
                    break;
                case EnableDisableVisible.InitBegin:
                    break;
                case EnableDisableVisible.AfterPopulate:
                    break;
                case EnableDisableVisible.BeforePopulate:
                    break;
                case EnableDisableVisible.RowSelected:
                    break;
            }
        }

        /// <summary>
        /// Open new form windows
        /// </summary>
        /// <param name="caseName"></param>
        private void CallOtherForms(CallOtherForm caseName)
        {
            PfwStandard tempWin = null;

            switch (caseName)
            {
                case CallOtherForm.HistoryClick:
                    tempWin = CreateWindow("phoenix.client.dplntchistory", "Phoenix.Windows.Client", "frmDpLnTCHistory"); 
                  
                    DataRow _selecteRow = gridView1.GetFocusedDataRow(); 
                    int indexAcctType = gridView1.Columns.IndexOf(colAcctTp);
                    string acctType = Convert.ToString(gridView1.GetFocusedRowCellValue(gridView1.Columns[indexAcctType]));
                    string acctNo = Convert.ToString(gridView1.GetFocusedRowCellValue(gridView1.Columns[gridView1.Columns.IndexOf(colAcctNum)]));
                    tempWin.InitParameters(acctType, acctNo, "DP", "N","",Phoenix.Shared.Constants.ScreenId.CashRwdInquiry);                   
                    break;
            }

            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                tempWin.Show();
            }
        }

        #endregion

        private void frmCashRwdInquiry_PShowCompletedEvent(object sender, EventArgs e)
        {
            if (gridView1.RowCount > 0)
            {
                gridView1.SelectRow(0);
                gridView1.FocusedColumn = gridView1.Columns["AmtRedeemed"];
            }
        }

        private void PbRedeem_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            //16888 - The requested cash reward will be redeemed and credited to the corresponding account.
            if (PMessageBox.Show(this, 16888, MessageType.Question, MessageBoxButtons.YesNo, string.Empty) == DialogResult.No)
            {
                return;
            }
            bool hasChanges = false;
            Phoenix.BusObj.Misc.CashReward busObj = null;
           

            List<BusObjectBase> objList = new List<BusObjectBase>();
            DataTable dt = gridControl1.DataSource as DataTable;

            foreach(DataRow dr in dt.Rows)
            {
                string acctType = string.Empty;
                string acctno = string.Empty;
                Decimal redeemAmt = 0;

                acctType = Convert.ToString(dr["AcctType"]);
                acctno = Convert.ToString(dr["AcctNo"]);
                redeemAmt = dr["AmtRedeemed"] == DBNull.Value? 0: Convert.ToDecimal(dr["AmtRedeemed"]);

                if (redeemAmt > 0)
                {
                    busObj = new Phoenix.BusObj.Misc.CashReward();

                    busObj.GetFieldByXmlTag("RedeemAmt").Value = redeemAmt;
                    busObj.GetFieldByXmlTag("AcctNo").Value = acctno;
                    busObj.GetFieldByXmlTag("AcctType").Value = acctType;
                    busObj.ActionType = XmActionType.New;
                    busObj.ResponseTypeId = 12;
                    CoreService.DataService.AddObject(busObj);
                    objList.Add(busObj);
                    hasChanges = true;
                }

            }
          
            if (hasChanges)
            {
                CoreService.DataService.ProcessRequest();

                bool bContinue = true;

                foreach (BusObjectBase obj in objList)
                {

                    foreach (PMessage msg in obj.Messages)
                    {
                        PMessageBox.Show(msg);
                        bContinue = false;
                        break;
                    }
                    if(!bContinue)
                    {
                        break;
                    }
                }

                if (!bContinue) return ;

            }
            else
            {
                //16873 - There were no changes made, so nothing was saved.
                DialogResult result = PMessageBox.Show(this, 16873, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                frmCashRwdInquiry_PShowCompletedEvent(null, null);
                return;
            }

            if(_CashReward == null)
                _CashReward = new Phoenix.BusObj.Misc.CashReward();
            _CashReward.RimNo.Value = _pnRimNo.IntValue;
            _CashReward.ResponseTypeId = 11;
            _CashReward.ActionType = XmActionType.Select;
            CoreService.DataService.AddObject(_CashReward);
            CoreService.DataService.ProcessRequest();

            frmCashRwdInquiry_PInitCompleteEvent();

            frmCashRwdInquiry_PShowCompletedEvent(null, null);

        }

        private void repositoryItemHyperLinkEdit1_Click(object sender, EventArgs e)
        {
            CallOtherForms(CallOtherForm.HistoryClick);
           
        } 
        private void PopulateGrid()
        {
            Phoenix.BusObj.Misc.CashReward ListObj = new Phoenix.BusObj.Misc.CashReward();
            ListObj.RimNo.Value = _pnRimNo.IntValue;
            ListObj.ResponseTypeId = 11;

            XmlNode _nodeListview = Phoenix.FrameWork.CDS.DataService.Instance.GetListView(ListObj, null);

          
            short colIdx = 0;
            XmlNodeList RecordNodeList = _nodeListview.SelectNodes("RECORD");
            DataTable table = new DataTable();
            if (RecordNodeList.Count > 0)
            {
                table = ConvertXmlNodeListToDataTable(RecordNodeList);
                gridControl1.DataSource = table;
            }
        }
       
        private DataTable ConvertXmlNodeListToDataTable(XmlNodeList xnl)
        {
            DataTable dt = new DataTable();
            int TempColumn = 0;
            string innerText = "";
            //Create Columns
            foreach (XmlNode node in xnl.Item(0).ChildNodes)
            {
                TempColumn++;
                DataColumn dc = null;
                if (node.Name == "CashRwdBal" || node.Name == "MinRewardAmt" || node.Name == "AmtRedeemed")
                {
                     dc = new DataColumn(node.Name, System.Type.GetType("System.Decimal"));
                    dc.AllowDBNull = true;
                }
                else
                {
                     dc = new DataColumn(node.Name, System.Type.GetType("System.String"));

                }
                if (dt.Columns.Contains(node.Name))
                {
                    dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                }
                else
                {
                    //XmlNode titleNode =  xnl.Item(1);
                    //dc.ColumnName = titleNode.Name;
                    dt.Columns.Add(dc);
                }
            }


            //Create Rows
            int ColumnsCount = dt.Columns.Count;
            for (int i = 0; i < xnl.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (dt.Columns[j].ColumnName == "AmtRedeemed")
                    {
                        dr[j] = DBNull.Value;
                    }
                    else
                    {
                        dr[j] = xnl.Item(i).ChildNodes[j].InnerText;
                    }
                }

                dt.Rows.Add(dr);
            }
            return dt;

        }
    }
}