#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlProofTotals.cs
// NameSpace: phoenix.client.tlprooftotals
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//2/10/2009     1       LSimpson    #76409 - Created
//06/02/2009    2       LSimpson    #4366 - Added position date filter.
//07/01/2009    3       LSimpson    #4753 - Modifications for offline.
//04/09/2010    4       VSharma     #8555- Made offline CDS call for proof totals.
//07/08/2011    5       BSchlottman #WI14129 - Pass the correct branch to the details screen.
//8/3/2013		6		apitava		#157637 Uses new xfsprinter
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Utility;
using Phoenix.Shared.Variables;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Client;
using Phoenix.Windows.Forms;

namespace phoenix.client.tlprooftotals
{
    public partial class frmTlProofTotals : PfwStandard
    {
        private TellerVars _tellerVars =null; 
        private DialogResult dialogResult = DialogResult.None;
        private PSmallInt _reprintFormId;
        private PDecimal _noCopies;
        private PSmallInt _noCopiesPrinted;
        private PString _checkType;
        private PDecimal _proofPtid;
        private AdTlControl _adTlControl;


        private string _reprintInfo = "";

        #region Private Variables
        private PSmallInt branchNo = new PSmallInt();
        private PSmallInt drawerNo = new PSmallInt();
        private PDateTime positionDate = new PDateTime(); // #4366
        private int responseType = 10;  // default select for Pending
        #endregion

        #region Constructors
        public frmTlProofTotals()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        public override void InitParameters(params object[] paramList)
        {
            _tellerVars =   TellerVars.Instance;
            branchNo.Value = Convert.ToInt16(paramList[0]);
            drawerNo.Value = Convert.ToInt16(paramList[1]);
            positionDate.Value = Convert.ToDateTime(paramList[2]); // #4366

            if (drawerNo.Value == -1)
                responseType = 13;
            base.InitParameters(paramList);
        }


        #endregion

        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            //TODO: Add code to set the main business object here
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlProofTotals;            
            //this.pbProofDetails.NextScreenId = Phoenix.Shared.Constants.ScreenId.TlProofDetails;

            return ReturnType.Success;
        }

        #endregion

        
        private void fwStandard_PInitCompleteEvent()
        {
            _noCopies = new PDecimal("NoCopies");
            _noCopiesPrinted = new PSmallInt("NoCopiesPrinted");
            _checkType = new PString("checkType");
            _proofPtid = new PDecimal("proofPtid");
            _noCopies.Value = -1;
            _noCopiesPrinted.Value = 0;
            _adTlControl = TellerVars.Instance.AdTlControl;
            if (drawerNo.Value == -1)
            {
                colBranchNo.Visible = true;
                colDrawerNo.Visible = true;
                colDescription.Width = 140;
                colStatusText.Width = 83;
            }
            rbNotProofed.Select();
            EnableDisableControls();
        }

        private void EnableDisableControls()
        {
            if (colStatusText.Text == "Processed" )
                pbProcess.Enabled = false;
            else
                pbProcess.Enabled = true;

            if (grdProofTotals.Items.Count == 0)
            {
                pbProofDetails.Enabled = false;
                pbProcess.Enabled = false;
            }
            else
                pbProofDetails.Enabled = true;

        }

        #region #76409
        private void CallOtherForms(string origin)
        {
            PfwStandard tempDlg = null;

            try
            {
                PfwStandard tempWin = null;

                if (origin == "ProofDetails")
                {
                    tempWin = Helper.CreateWindow("phoenix.client.tlproofdetails", "phoenix.client.tlproofdetails", "frmTlProofDetails");
                    tempWin.InitParameters(colCheckType.UnFormattedValue, colStatus.UnFormattedValue, branchNo.Value, colDrawerNo.UnFormattedValue, colProofPtid.UnFormattedValue); //#14129
                }
                else if (origin == "AdHocReceipt")
                {
                    _reprintInfo = CoreService.Translation.GetUserMessageX(360612);
                    _reprintFormId = new PSmallInt("FormId");
                    if (_noCopies.Value <= 0 || _noCopies.StringValue.Trim() == "")
                        _noCopies.Value = -1;
                    _reprintFormId.Value = Convert.ToInt16(Phoenix.Shared.Constants.ScreenId.TlProofTotals);
                    tempDlg = Helper.CreateWindow("phoenix.client.tlprinting", "Phoenix.Client.TlPrinting", "dlgAdHocReceipt");
                    tempDlg.InitParameters(_reprintInfo, _reprintFormId, _reprintFormId.Value, null, null,_noCopies);
                    if (_noCopies.IntValue > 1)
                        _noCopiesPrinted.Value += 1;
                    else
                        _noCopiesPrinted.Value = 1;
                }
                if (tempDlg != null)
                {
                    tempDlg.Closed += new EventHandler(tempDlg_Closed); 
                    dialogResult = tempDlg.ShowDialog();
                }
                if (tempWin != null)
                {
                    tempWin.Workspace = this.Workspace;
                    tempWin.Show();
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
                return;
            }
        }

        private void tempDlg_Closed(object sender, EventArgs e)
        {
            Form form = sender as Form;
            if (form.Name == "dlgAdHocReceipt")
            {
                if (!_reprintFormId.IsNull && _reprintFormId.Value == -1 && (_noCopies.IntValue == _noCopiesPrinted.IntValue)) //the game is really over
                    _reprintFormId.Value = 0;
                if (_noCopies.IntValue > _noCopiesPrinted.IntValue)
                {
                    PSmallInt brNo = new PSmallInt("brNo");
                    brNo.Value = branchNo.Value;
                    PSmallInt drNo = new PSmallInt("drNo");
                    drNo.Value = drawerNo.Value;
                    PInt CurTranCode = new PInt("CurTranCode");
                    PDateTime postingDt = new PDateTime("postingDt");
                    PrintInfo printInfo = null;
                    AdTlForm adTlForm;
                    XfsPrinter xfsPrinter = null; 
                    TlJournal tlJournal = new TlJournal();
                    postingDt.Value = _tellerVars.PostingDt;

                    CallOtherForms("AdHocReceipt");
                    if (dialogResult != DialogResult.OK)
                        return;

                    if (!TellerVars.Instance.SetContextObject("AdTlFormArray", _reprintFormId.Value))
                    {
                        PMessageBox.Show(360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                        return;
                    }
                    adTlForm = TellerVars.Instance.AdTlForm;
					xfsPrinter = new XfsPrinter(adTlForm.LogicalService.Value);	  //#157637
                    try
                    {
                        printInfo = tlJournal.GetCheckProofPrintInfo(adTlForm.PrintString.Value, _proofPtid.IntValue);

                        if (printInfo != null)
                        {
							xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);	 //#157637
							xfsPrinter.Close();	 //#157637
						}
                    }
                    catch (PhoenixException pe)
                    {
                        PMessageBox.Show(pe, MessageBoxButtons.OK);
                        return;
                    }
				}
            }
        }
        #endregion

     

        #region events

        void grdProofTotals_BeforePopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
        {
            TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
            _tlDrawerBalances.BranchNo.Value = this.branchNo.Value;
            _tlDrawerBalances.DrawerNo.Value = this.drawerNo.Value;
            _tlDrawerBalances.DrawerCurPostingDt.Value = positionDate.Value; // #4366
            _tlDrawerBalances.ResponseTypeId = responseType;
            grdProofTotals.ListViewObject = _tlDrawerBalances;
        }

        void grdProofTotals_AfterPopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
        {
            EnableDisableControls();
        }

        void grdProofTotals_FetchRowDone(object sender, Phoenix.Windows.Forms.GridRowArgs e)
        {
            if (colCheckType.Text.ToString().Trim() == "O")
                colDescription.UnFormattedValue = "On-Us Checks";
            else
                colDescription.UnFormattedValue = "Not On-Us Checks";
            if (Convert.ToInt32(colStatus.UnFormattedValue) == 1)
                colStatusText.Text = "Pending";
            if (Convert.ToInt32(colStatus.UnFormattedValue) == 2)
                colStatusText.Text = "Processed";
            if (colBranchNo.Text == "")
                colBranchNo.Text = branchNo.StringValue;
            if (drawerNo.Value != -1)
                colDrawerNo.Text = drawerNo.StringValue;
            EnableDisableControls();

        }

        void rbAll_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbAll.Checked == true)
            {
                if (drawerNo.Value == -1)
                    responseType = 15;
                else
                    responseType = 12;
                grdProofTotals.PopulateTable();
            }
        }

        void rbProofed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbProofed.Checked == true)
            {
                if (drawerNo.Value == -1)
                    responseType = 14;
                else
                    responseType = 11;
                grdProofTotals.PopulateTable();
            }
        }

        void rbNotProofed_CheckedChanged(object sender, System.EventArgs e)
        {
            if (rbNotProofed.Checked == true)
            {
                if (drawerNo.Value == -1)
                    responseType = 13;
                else
                    responseType = 10;
                grdProofTotals.PopulateTable();
            }
        }

        void grdProofTotals_SelectedIndexChanged(object source, Phoenix.Windows.Forms.GridClickEventArgs e)
        {
            EnableDisableControls();
        }


        void grdProofTotals_RowClicked(object source, Phoenix.Windows.Forms.GridClickEventArgs e)
        {
            EnableDisableControls();
        }


        void pbProcess_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            if (_adTlControl.RealTimeProof.Value == "Y")
            {
                _checkType.Value = colCheckType.Text;
                PSmallInt brNo = new PSmallInt("brNo");
                brNo.Value = branchNo.Value;
                PSmallInt drNo = new PSmallInt("drNo");
                drNo.Value = drawerNo.Value;
                PDecimal proofAmt = new PDecimal("proofAmt"); //#8555
                PDecimal proofNoItems = new PDecimal("proofNoItems"); //#8555
                PString OfflineDb = new PString("OfflineDb");   // #4753
                OfflineDb.Value = AppInfo.Instance.OfflineDbNames;  // #4753
                if (drNo.Value == -1)
                    drNo.Value = Convert.ToInt16(colDrawerNo.Text);
                PInt CurTranCode = new PInt("CurTranCode");
                PDateTime postingDt = new PDateTime("postingDt");
                _proofPtid.Value = 0;
                PrintInfo printInfo = null;
                AdTlForm adTlForm;
                TlJournal tlJournal = new TlJournal();
                postingDt.Value = _tellerVars.PostingDt;
                Phoenix.BusObj.Teller.TlDrawerBalances _tempDrBalObj = new TlDrawerBalances();
                _tempDrBalObj.ActionType = XmActionType.Custom;
                _tempDrBalObj.BranchNo.Value = branchNo.Value;
                _tempDrBalObj.DrawerNo.Value = drNo.Value;
                _tempDrBalObj.ClosedDt.Value = Convert.ToDateTime(_tellerVars.PostingDt);

                DataService.Instance.ProcessCustomAction(_tempDrBalObj, "ProofTotals", brNo, drNo, _checkType, postingDt, _proofPtid, OfflineDb, proofAmt, proofNoItems);
                if (proofAmt.IsNull) //#8555
                    proofAmt.Value = 0;
                if (proofNoItems.IsNull) //#8555
                    proofNoItems.Value = 0;

                if (TellerVars.Instance.OfflineCDS != null) //#8555
                {
                    TellerVars.Instance.OfflineCDS.ProcessCustomAction(_tempDrBalObj, "ProofTotalsOfflineUpdate", brNo, drNo, _checkType, proofAmt, proofNoItems);
                }

                grdProofTotals.PopulateTable();
                CallOtherForms("AdHocReceipt");
                if (dialogResult != DialogResult.OK)
                    return;

                if (!TellerVars.Instance.SetContextObject("AdTlFormArray", _reprintFormId.Value))
                {
                    PMessageBox.Show(360618, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                    return;
                }
                adTlForm = TellerVars.Instance.AdTlForm;
                try
                {
                    printInfo = tlJournal.GetCheckProofPrintInfo(adTlForm.PrintString.Value, _proofPtid.IntValue);
                    printInfo.TellerDrawer = drNo.IntValue;
                    printInfo.SequenceNo = _tempDrBalObj.DrawerSeqNo.IntValue;
                    if (printInfo != null)
                    {
						XfsPrinter xfsPrinter = new XfsPrinter(adTlForm.LogicalService.Value);	   //#157637
						xfsPrinter.PrintForm(adTlForm.MediaName.Value, adTlForm.FormName.Value, printInfo);	  //#157637
						xfsPrinter.Close();	  //#157637
                    }
                }
                catch (PhoenixException pe)
                {
                    PMessageBox.Show(pe, MessageBoxButtons.OK);
                    return;
                }
            }
        }

        void pbProofDetails_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            CallOtherForms("ProofDetails");
        }

        #endregion
        private void frmTlProofTotals_Load(object sender, EventArgs e)
        {

        }
          
         


    }
        
         
}