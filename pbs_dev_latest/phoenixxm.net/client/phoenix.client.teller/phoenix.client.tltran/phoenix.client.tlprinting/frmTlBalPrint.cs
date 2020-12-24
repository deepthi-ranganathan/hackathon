#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlBalPrint.cs
// NameSpace: phoenix.client.tlprinting
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//1/18/2011     1		LSimpson    Created  
//04/26/2011    2       LSimpson    #13713 - Evaluate bAcctHasPostAccessOnly for 
//                                      tfr acct now that it's loaded correctly.  
//05/03/2011    3       LSimpson    #13849 - Disable printing for loan memo posted items.
//05/23/2011    4       LSimpson    #14258 - Disable print bal option for confidential accounts.
//06/29/2011    5       LSimpson    #14628 - Added retrieval of gl description.
//07/11/2011    6       LSimpson    #14628 - Add check for GL transfer account.
//05/18/2012    7       LSimpson    #17818 - Added confidential balance printing check for AllowEmployeeToViewConfidentialFlag and AllowEmployeeToIgnoreConfidentialFlag.
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.GL; // #14628
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame; // #14628
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;

namespace phoenix.client.tlprinting
{
    public partial class frmTlBalPrint : PfwStandard
    {
        #region Private Variables
        private Phoenix.BusObj.Teller.TlTransactionSet _tlTranSet = new TlTransactionSet();
        private Phoenix.BusObj.Teller.TlTransaction _tlTranTest = new TlTransaction();
        private AdTlControl _adTlControl = new AdTlControl();
        private GbMapAcctRel _gbMapAcctRel = new GbMapAcctRel(); // #13845
        private bool _isMt = false;
        private bool _isFromSave = false;
        private bool _gridIsLoaded = false;
        #endregion
        #region Constructors
        public frmTlBalPrint()
        {
            InitializeComponent();
        }




        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        public override void InitParameters(params object[] paramList)
        {
            _tlTranSet = paramList[0] as TlTransactionSet;
            _isMt = (bool)paramList[1];
            // Must call the base to store the parameters.
            base.InitParameters(paramList);
        }
        #endregion



        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            //this.MainBusinesObject = _adTlControl;
            return ReturnType.Success;
        }
        
        private void fwStandard_PInitCompleteEvent()
        {
            SetDefaults();
            _gridIsLoaded = true;
            // Below is a widely used hack to enable the save button....
            if (this.ActionSave != null)
            {
                this.ActionSave.NextScreenId = 0;
            }
        }

        private void SetDefaults()
        {
            grdBalancesToPrint.RowsDefaultCellStyle.WrapMode = DataGridViewTriState.True; // wrapped to subsequent lines
            grdBalancesToPrint.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;  //Row height autosize

            grdBalancesToPrint.Columns["colTc"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colDescription"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colAccount"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colCustName"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colRelationship"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colCustNo"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colPrintBal"].DefaultCellStyle.NullValue = false;
            grdBalancesToPrint.Columns["colIsEditable"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colSequenceNo"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colSubSequenceNo"].DefaultCellStyle.NullValue = null;
            grdBalancesToPrint.Columns["colOrigPrintCustBal"].DefaultCellStyle.NullValue = null;

            foreach (TlTransaction _tlTranTest in _tlTranSet.Transactions)
            {
                string sRimFirstName = string.Empty;
                string sRimLastName = string.Empty;
                string sRimMiddleInitial = string.Empty;
                string sRimType = string.Empty;
                string sTitle = string.Empty;
                string sSuffix = string.Empty;
                bool tranFound = false;
                int newRow;
                int tfrRimNo = int.MinValue; // #14628
                GbHelper gbHelper = new GbHelper();

                if (_tlTranTest.AcctType.Value == "GL") // #14628
                    sRimLastName = GetGlTitle(_tlTranTest.AcctNo.Value);
                else
                    gbHelper.GetNameDetails(_tlTranTest.RimNo.Value, ref sRimFirstName, ref sRimMiddleInitial, ref sRimLastName, ref sRimType, ref sTitle, ref sSuffix);
                
                tranFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", _tlTranTest.TlTranCode.Value);
                if (!_isMt || (_isMt && _tlTranSet.TellerVars.AdTlTc.IncludeMtPrint.Value == "Y"))
                {
                    newRow = grdBalancesToPrint.AddNewRow();
                    grdBalancesToPrint.Rows[newRow].Cells["colTc"].Value = _tlTranTest.TlTranCode.StringValue;
                    grdBalancesToPrint.Rows[newRow].Cells["colDescription"].Value = _tlTranSet.TellerVars.AdTlTc.Description.StringValue;
                    grdBalancesToPrint.Rows[newRow].Cells["colAccount"].Value = _tlTranTest.AcctType.StringValue + "-" + _tlTranTest.AcctNo.StringValue;
                    grdBalancesToPrint.Rows[newRow].Cells["colCustName"].Value = sRimLastName + (sRimFirstName.Trim() == "" ? " " : ", ") + sRimFirstName + " " + sRimMiddleInitial;
                    grdBalancesToPrint.Rows[newRow].Cells["colRelationship"].Value = "Primary";
                    if (!_tlTranTest.RimNo.IsNull) // #14628
                        grdBalancesToPrint.Rows[newRow].Cells["colCustNo"].Value = _tlTranTest.RimNo.StringValue;
                    grdBalancesToPrint.Rows[newRow].Cells["colSequenceNo"].Value = _tlTranTest.SequenceNo.StringValue;
                    grdBalancesToPrint.Rows[newRow].Cells["colSubSequenceNo"].Value = _tlTranTest.SubSequence.StringValue;
                    
                    //#13849 - Disable printing for loan memo posted items.

                    // Have to circumvent disabling of PrintBal column since FrameWork does not work properly
                    // Will store editable value in colIsEditable....
                    if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y" && !_tlTranTest.TranAcct.bAcctHasPostAccessOnly.Value == true
                        && !((gbHelper.IsConfAcct(_tlTranTest.TranAcct.AcctType.Value, _tlTranTest.TranAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag)) // #14258
                        && !(_tlTranTest.TranAcct.TranCode.Value >= 300 && _tlTranTest.TranAcct.TranCode.Value <= 399 && _tlTranSet.TellerVars.AdTlTc.RealTimeEnable.Value != "Y")
                        && !(_tlTranTest.TranAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranTest.TranAcct.TranCode.Value)))
                    {
                        grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value = "Y";
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].ReadOnly = false;
                    }
                    else
                    {
                        grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value = "N";
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].ReadOnly = true;
                    }
                    grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "N";
                    grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = false;
                    grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "N";

                    if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "A" && grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value.ToString() == "Y")
                    {
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = true;
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "Y";
                        grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "Y";
                    }
                    else if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "N" || _tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "S")
                    {
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = false;
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "N";
                        grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "N";
                    }
                    // Get relationships....
                    _gbMapAcctRel = new GbMapAcctRel();
                    _gbMapAcctRel.ResponseTypeId = 13;
                    _gbMapAcctRel.AcctNo.Value = _tlTranTest.AcctNo.Value;
                    _gbMapAcctRel.AcctType.Value = _tlTranTest.AcctType.Value;
                    _gbMapAcctRel.RimNo.Value = _tlTranTest.RimNo.Value;
                    _gbMapAcctRel.ActionType = Phoenix.FrameWork.BusFrame.XmActionType.ListView;
                    // Load into array....
                    ArrayList relArray = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(_gbMapAcctRel);
                    if (relArray != null && relArray.Count > 0)
                    {
                        foreach (GbMapAcctRel gbMapAcctRel in relArray)
                        {

                            grdBalancesToPrint.Rows[newRow].Cells["colCustName"].Value += "\r\n" + gbMapAcctRel.Related.StringValue;
                            grdBalancesToPrint.Rows[newRow].Cells["colRelationship"].Value += "\r\n" + gbMapAcctRel.RelationshipText.StringValue;
                            grdBalancesToPrint.Rows[newRow].Cells["colCustNo"].Value += "\r\n" + gbMapAcctRel.RimNo.StringValue;
                        }
                    }
                }
                if (!_tlTranTest.TfrTranCode.IsNull || (_tlTranTest.TfrTranCode.IsNull && !_tlTranTest.TfrAcctNo.IsNull && _tlTranTest.TfrAcctNo.Value.Trim() != ""))
                {
                    if (!_isMt || (_isMt && _tlTranSet.TellerVars.AdTlTc.IncludeMtPrint.Value == "Y"))
                    {
                        newRow = grdBalancesToPrint.AddNewRow();
                        if (_tlTranTest.TfrAcctType.Value == "GL")
                        {
                            sRimLastName = GetGlTitle(_tlTranTest.TfrAcctNo.Value);
                        }
                        else
                        {
                            tfrRimNo = gbHelper.GetRimNo(_tlTranTest.TfrAcctType.Value, _tlTranTest.TfrAcctNo.Value);
                            gbHelper.GetNameDetails(tfrRimNo, ref sRimFirstName, ref sRimMiddleInitial, ref sRimLastName, ref sRimType, ref sTitle, ref sSuffix);
                        }
                        grdBalancesToPrint.Rows[newRow].Cells["colTc"].Value = _tlTranTest.TfrTranCode.IsNull ? "Transer" : _tlTranTest.TfrTranCode.StringValue;
                        grdBalancesToPrint.Rows[newRow].Cells["colAccount"].Value = _tlTranTest.TfrAcctType.StringValue + "-" + _tlTranTest.TfrAcctNo.StringValue;
                        grdBalancesToPrint.Rows[newRow].Cells["colCustName"].Value = sRimLastName + (sRimFirstName.Trim() == "" ? " " : ", ") + sRimFirstName + " " + sRimMiddleInitial;
                        grdBalancesToPrint.Rows[newRow].Cells["colRelationship"].Value = "Primary";
                        if (tfrRimNo != int.MinValue)   // #14628
                            grdBalancesToPrint.Rows[newRow].Cells["colCustNo"].Value = tfrRimNo.ToString();

                        if (!_tlTranTest.TfrTranCode.IsNull)
                        {
                            tranFound = _tlTranSet.TellerVars.SetContextObject("AdTlTcArray", _tlTranTest.TfrTranCode.StringValue, 0);

                            grdBalancesToPrint.Rows[newRow].Cells["colDescription"].Value = _tlTranSet.TellerVars.AdTlTc.Description.StringValue;
                        }
                        else
                            grdBalancesToPrint.Rows[newRow].Cells["colDescription"].Value = "Transer";

                        //#13849 - Disable printing for loan memo posted items.
                        //#14628 - Add check for GL transfer account.
                        // Have to circumvent disabling of PrintBal column since FrameWork does not work properly
                        // Will store editable value in colIsEditable....
                        if (_tlTranSet.TellerVars.AdTlTc.IncludePrtPrompt.Value == "Y" && !_tlTranTest.TfrAcct.bAcctHasPostAccessOnly.Value == true
                            && !((gbHelper.IsConfAcct(_tlTranTest.TfrAcct.AcctType.Value, _tlTranTest.TfrAcct.AcctNo.Value) && _tlTranSet.TellerVars.AdTlControl.PrintConfAcctBal.Value != "Y") && !(_tlTranSet.GlobalHelper.AllowEmployeeToViewConfidentialFlag || _tlTranSet.GlobalHelper.AllowEmployeeToIgnoreConfidentialFlag)) // #14258
                            && !(_tlTranTest.TfrTranCode.Value >= 300 && _tlTranTest.TfrTranCode.Value <= 399 && _tlTranSet.TellerVars.AdTlTc.RealTimeEnable.Value != "Y")
                            && !(_tlTranTest.TfrAcct.DepLoan.Value == "GL")
                            && !(_tlTranTest.TfrAcct.DepLoan.Value == "SD" && _tlTranSet.TellerHelper.IsSafeDepositTran(_tlTranTest.TfrTranCode.Value))) // #13713
                        {
                            grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value = "Y";
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].ReadOnly = false;
                        }
                        else
                        {
                            grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value = "N";
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].ReadOnly = true;
                        }
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "N";
                        grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = false;
                        grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "N";

                        if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "A" && grdBalancesToPrint.Rows[newRow].Cells["colIsEditable"].Value.ToString() == "Y")
                        {
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = true;
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "Y";
                            grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "Y";
                        }
                        else if (_tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "N" || _tlTranSet.TellerVars.AdTlControl.PrintBalDefault.Value == "S")
                        {
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Selected = false;
                            grdBalancesToPrint.Rows[newRow].Cells["colPrintBal"].Value = "N";
                            grdBalancesToPrint.Rows[newRow].Cells["colOrigPrintCustBal"].Value = "N";
                        }
                        // Get relationships....
                        _gbMapAcctRel = new GbMapAcctRel();
                        _gbMapAcctRel.ResponseTypeId = 13;
                        _gbMapAcctRel.AcctNo.Value = _tlTranTest.TfrAcctNo.Value;
                        _gbMapAcctRel.AcctType.Value = _tlTranTest.TfrAcctType.Value;
                        _gbMapAcctRel.RimNo.Value = tfrRimNo;
                        _gbMapAcctRel.ActionType = Phoenix.FrameWork.BusFrame.XmActionType.ListView;
                        // Load into array....
                        ArrayList relArray = Phoenix.FrameWork.CDS.DataService.Instance.GetListViewObjects(_gbMapAcctRel);
                        if (relArray != null && relArray.Count > 0)
                        {
                            foreach (GbMapAcctRel gbMapAcctRel in relArray)
                            {
                                grdBalancesToPrint.Rows[newRow].Cells["colCustName"].Value += "\r\n" + gbMapAcctRel.Related.StringValue;
                                grdBalancesToPrint.Rows[newRow].Cells["colRelationship"].Value += "\r\n" + gbMapAcctRel.RelationshipText.StringValue;
                                grdBalancesToPrint.Rows[newRow].Cells["colCustNo"].Value += "\r\n" + gbMapAcctRel.RimNo.StringValue;
                            }
                        }
                    }
                }
            }
        }

        #region #14628
        /// <summary>
        /// GetGlTitle returns GL title for gl acct
        /// </summary>
        /// <param name="acctNo"></param>
        /// <returns></returns>
        private string GetGlTitle(string acctNo)
        {
            string title1 = null;

            GlAcct Acct = new GlAcct();
            Acct.SelectAllFields = false;
            Acct.Description.Selected = true;
            Acct.AcctType.Value = "GL";
            Acct.AcctNo.Value = acctNo;

            Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(XmActionType.Select, Acct);

            title1 = Acct.Description.Value;
            return title1;
        }
        #endregion

        /// <summary>
        /// Update _tlTranSet PrintBalance and PrintTfrBalance for each transaction
        /// </summary>
        private void UpdateTransaction()
        {
            int seq = 0; // Have to circumvent since _tlTranTest.SubSequence.Value is not set by framework....
            foreach (TlTransaction _tlTranTest in _tlTranSet.Transactions)
            {
                if (_tlTranTest.TfrTranCode.IsNull && (_tlTranTest.TfrAcctNo.IsNull || _tlTranTest.TfrAcctNo.Value.Trim() == ""))
                {
                    _tlTranTest.PrintBalance.Value = grdBalancesToPrint.Rows[seq].Cells["colPrintBal"].Value.ToString();
                    _tlTranTest.PrintTfrBalance.Value = "N";
                }
                else
                {
                    _tlTranTest.PrintBalance.Value = grdBalancesToPrint.Rows[seq].Cells["colPrintBal"].Value.ToString();
                    _tlTranTest.PrintTfrBalance.Value = grdBalancesToPrint.Rows[seq + 1].Cells["colPrintBal"].Value.ToString();
                    seq++; // Must advance for transfer row
                }
                seq++;
            }
        }

        private void SetFormDirty()
        {
            int cntr = 0;
            foreach (DataGridViewRow it in grdBalancesToPrint.Rows)
            {
                if (grdBalancesToPrint.Rows[cntr].Cells["colPrintBal"].Value.ToString() != grdBalancesToPrint.Rows[cntr].Cells["colOrigPrintCustBal"].Value.ToString())
                    this.IsDirty = true;
                cntr++;
            }
        }
        #endregion

        void grdBalancesToPrint_CurrentCellDirtyStateChanged(object sender, System.EventArgs e)
        {
            if (grdBalancesToPrint.IsCurrentCellDirty)
            {
                grdBalancesToPrint.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

/// <summary>
/// grdBalancesToPrint_CellClick - invoked when any cell is clicked within the grid
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        void grdBalancesToPrint_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            //Framework CheckedChanged event does not work properly....
            //Circumvent CheckedChanged event - if cell 6 (colPrintBal) is clicked and readonly, populate label....
            if (_gridIsLoaded)
            {
                if (grdBalancesToPrint.Rows[grdBalancesToPrint.CurrentRow.Index].Cells["colIsEditable"].Value.ToString() == "N"
                    && grdBalancesToPrint.CurrentCell.ColumnIndex == 6)
                {
                    lblOptNotAvail.Text = "Print Balance option can not be changed for this account.";
                    if (colPrintBal.Checked == true)
                        colPrintBal.Checked = false;
                }
                else
                {
                    lblOptNotAvail.Text = "";
                }
            }
        }

        void colPrintBal_CheckedChanged(object sender, Phoenix.Windows.Forms.PEventArgs e)
        {
            if (colIsEditable.FormattedValue != "Y")
            {
                lblOptNotAvail.Text = "Print Balance option can not be changed for this account.";
                if (colPrintBal.Checked == true)
                    colPrintBal.Checked = false;
            }
            else
            {
                lblOptNotAvail.Text = "";
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int WM_KEYDOWN = 0x100;

            const int WM_SYSKEYDOWN = 0x104;

            if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Space:
                        if (colIsEditable.FormattedValue != "Y")
                        {
                            lblOptNotAvail.Text = "Print Balance option can not be changed for this account.";
                            if (colPrintBal.Checked == true)
                                colPrintBal.Checked = false;
                        }
                        else
                        {
                            colPrintBal.Checked = !colPrintBal.Checked;
                            grdBalancesToPrint.EndEdit();
                            lblOptNotAvail.Text = "";
                        }

                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
 
        public override bool OnActionSave(bool isAddNext)
        {
            // Per design, Save is to function the same as Close....
            SetFormDirty(); // Another circumvent due to framework not setting form or grid dirty attributes....
            if (!this.IsDirty)
            {
                if (DialogResult.No == PMessageBox.Show(this, 13310, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
                {
                    return false;
                }
            }
            _isFromSave = true;
            this.Close();
            return true;
        }

        #region OnActionClose
        public override bool OnActionClose()
        {
            SetFormDirty(); // Another circumvent due to framework not setting form or grid dirty attributes....
            if (!this.IsDirty && !_isFromSave)
            {
                if (DialogResult.No == PMessageBox.Show(this, 13310, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
                {
                    return false;
                }
            }
            UpdateTransaction();
            
            base.OnActionClose();
            return true;
        }
        #endregion OnActionClose

    }
}