#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: $fileinputname$.cs
// NameSpace: $rootnamespace$
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//2/20/2009     1		LSimpson    #76409 - Created.
//01/17/2013    2       mselvaga    #145194-CAS-500920-S38S20 - SDR Checks are not in order when looking at proof details in the proof totals screen.
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.Shared.Windows;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Client;
using Phoenix.Windows.Forms;

namespace phoenix.client.tlproofdetails
{
    public partial class frmTlProofDetails : PfwStandard
    {
        #region Private Variables
        private PString checkType = new PString();
        private PSmallInt proofStatus = new PSmallInt();
        private PSmallInt branchNo = new PSmallInt();
        private PSmallInt drawerNo = new PSmallInt();
        private PDecimal totalAmount = new PDecimal();
        private PDecimal proofPtid = new PDecimal();
        private PInt totalChecks = new PInt();
        private int responseType = 20;  // default select for All
        private string[] removeRows = new string[1000];
        private string acctTypeFmt = null;
        private int removeRowCounter = 0;
        Phoenix.Shared.Windows.PdfFileManipulation _pdfFileManipulation;

        #endregion

        #region Constructors
        public frmTlProofDetails()
        {
            InitializeComponent();
        }




        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        public override void InitParameters(params object[] paramList)
        {
            checkType.Value = Convert.ToString(paramList[0]);
            proofStatus.Value = Convert.ToInt16(paramList[1]);
            branchNo.Value = Convert.ToInt16(paramList[2]);
            drawerNo.Value = Convert.ToInt16(paramList[3]);
            proofPtid.Value = Convert.ToDecimal(paramList[4]);
            if (checkType.Value == "T" || checkType.Value == "C")
            {
                if (proofStatus.Value == 1)
                    responseType = 16;
                if (proofStatus.Value == 2)
                    responseType = 17;
            }
            if (checkType.Value == "O")
            {
                if (proofStatus.Value == 1)
                    responseType = 18;
                if (proofStatus.Value == 2)
                    responseType = 19;
            }
            base.InitParameters(paramList);
        }


        #endregion

        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            //TODO: Add code to set the main business object here
            ScreenId = Phoenix.Shared.Constants.ScreenId.TlProofDetails;
            if (checkType.Value == "O")
                this.EditRecordTitle += " On-Us";
            else
                this.EditRecordTitle += " Not On-Us";

            return ReturnType.Success;
        }

        private void fwStandard_PInitCompleteEvent()
        {
            dfTotNumTran.UnFormattedValue = totalChecks.Value;
            dfTotNumTran.Text = totalChecks.Value.ToString();
            dfTotTranAmt.UnFormattedValue = totalAmount.Value;
            dfTotTranAmt.Text = totalAmount.Value.ToString("C");
        }

        private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;

            if (origin == "TlJournalDisplay")
            {
                tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournalDisplay");
                tempWin.InitParameters(Convert.ToInt16(branchNo.Value), Convert.ToInt16(colDrawerNo.UnFormattedValue),
                    Convert.ToDateTime(colEffDt.Text), colJrlPtid.Text);

            }
            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                tempWin.Show();
            }
        }

        #region GenerateReport
        public void GenerateReport(bool silentPrinting)
        {
            //11236 - Generating teller check proof details list.  Please wait...
            dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11236));
            try
            {
                _pdfFileManipulation = new PdfFileManipulation();
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                CoreService.LogPublisher.LogDebug("\n(frmTlProofDetails window/ GenerateReport)For some reason creating of HtmlPrinter Failed." + ex.Message);
            }
            //
            Phoenix.BusObj.Misc.RunSqrReport report1 = new Phoenix.BusObj.Misc.RunSqrReport();
            report1.ReportName.Value = "TLO41000.sqr";
            report1.EmplId.Value = GlobalVars.EmployeeId;
            report1.FromDt.Value = GlobalVars.SystemDate;
            report1.ToDt.Value = GlobalVars.SystemDate;
            report1.Param1.Value = this.branchNo.Value.ToString();
            report1.Param2.Value = this.drawerNo.Value.ToString();
            report1.Param3.Value = this.checkType.Value;
            report1.Param4.Value = this.proofStatus.Value.ToString();
            report1.Param5.Value = this.proofPtid.Value.ToString();
            report1.Param6.Value = colJrlPtid.Text;
            report1.RunDate.Value = DateTime.Now;
            report1.ExecutionMode.Value = Phoenix.BusObj.Misc.SQRExecutionMode.Online.ToString();

            try
            {
            DataService.Instance.ProcessRequest(XmActionType.Select, report1);
            _pdfFileManipulation.ShowUrlPdf(report1.OutputLink.Value);
            }
            catch (PhoenixException pe)
            {
                dlgInformation.Instance.HideInfo();
                Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.Message + " - " + pe.InnerException);
                PMessageBox.Show(11237, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                //11237 - Failed to generate the teller check proof details list.
            }
        finally
        {
            dlgInformation.Instance.HideInfo();
        }
        }
        #endregion

        
        #endregion

        #region Events
        void pbDisplay_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            CallOtherForms("TlJournalDisplay");
        }

        void grdProofDetails_BeforePopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
        {
            TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
            _tlDrawerBalances.BranchNo.Value = this.branchNo.Value;
            _tlDrawerBalances.DrawerNo.Value = this.drawerNo.Value;
            _tlDrawerBalances.ResponseTypeId = responseType;
            _tlDrawerBalances.Ptid.Value = proofPtid.Value;
            grdProofDetails.ListViewObject = _tlDrawerBalances;
        }

        void grdProofDetails_AfterPopulate(object sender, Phoenix.Windows.Forms.GridPopulateArgs e)
        {
            for (int a = 0; a < removeRows.Length; a++)
            {
                if (removeRows[a] != null)
                    grdProofDetails.Items.Remove(Convert.ToInt16(removeRows[a]));
            }
        }

        void frmTlProofDetails_PMdiPrintEvent(object sender, System.EventArgs e)
        {
            if (TellerVars.Instance.IsAppOnline)
                GenerateReport(false);
        }

        void grdProofDetails_FetchRowDone(object sender, Phoenix.Windows.Forms.GridRowArgs e)
        {
            if (colCheckType.Text == "P")
            {
                if (proofStatus.Value == 2)
                    this.EditRecordTitle += " - " + colSeqNo.Text;
                removeRows[removeRowCounter] = grdProofDetails.ContextRow.ToString();
                removeRowCounter += 1;
            }
            else
            {
                totalAmount.Value += Convert.ToDecimal(colAmount.UnFormattedValue);
                totalChecks.Value += Convert.ToInt16(colNoOfItems.UnFormattedValue);
                if (colAcctType.UnFormattedValue != null)
                    acctTypeFmt = colAcctType.UnFormattedValue + " - ";
                else
                    acctTypeFmt = "";
                colAccount.UnFormattedValue = acctTypeFmt + colAcctNo.UnFormattedValue;
            }
        }

        #endregion

        private void frmTlProofDetails_Load(object sender, EventArgs e)
        {

        }


    }
}