#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: GbIrsHelper.cs
// NameSpace: phoenix.busobj.irshelper
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/03/2017    1		Nisam       Created a global helper for IRS projects
//10/04/2017    2       Nisam       #71467 - Added Common method for Export to XLS functionality
//11/29/2017    3       Chinju T    #Task 76461:Dev Task-209333-Mask the TIN Number and Account Number when the details are exported to XLS
//12/07/2017    4       CNisam      #76508 - Added trim() for Acct No before masking
//8/14/2018     5       Chinju T    Task 95014:Dev Task-209333-Alternate address selection in The Recipient Information tab in Add New /Edit Existing form window
//11/05/2018    6       Chinju T    Task 104153:Dev Task-209333-Enable Filing Status Combo Box in Add New/ Edit Existing Window
//-------------------------------------------------------------------------------

#endregion
using System;
//
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Phoenix.BusObj.Global.Server;
namespace Phoenix.BusObj.IrsHelper
{
    public class GbIrsHelper : Phoenix.FrameWork.BusFrame.BusObjectBase
    {

        #region Constructors
        public GbIrsHelper() : base()
        {
            InitializeMap();
        }
        #endregion
        protected override void InitializeMap()  /* 95014*/
        {
            this.DbObjects.Add("IRS_HELPER", "X_IRS_HELPER");
            this.SupportedAction |= XmActionType.Update | XmActionType.New | XmActionType.Delete | XmActionType.Custom;
            this.IsOfflineSupported = false;
            base.InitializeMap();
        }
        #region Public Properties
        #endregion

        #region Public Methods

        // Begin #71467
        /// <summary>
        /// Exports result of IRS form BO's custom action into a csv formatted file
        /// </summary>
        /// <param name="pIrsForm">IRS Form BO</param>
        /// <param name="pnResponseTypeId">ResponseTypeId specified in IRS form BO</param>
        /// <param name="psCustomAction">Custom Action Name in IRS form BO</param>
        public void ExportActionResultToCsv(BusObjectBase pIrsForm, int pnResponseTypeId, string psCustomAction)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV (Comma delimited)(*.csv*)|*.csv*";
                saveFileDialog.FilterIndex = 2;
                saveFileDialog.Title = "Export";
                saveFileDialog.InitialDirectory = @"c:\";

                DialogResult result = saveFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string sFileName = saveFileDialog.FileName;

                    using (new WaitCursor())
                    {
                        PString sResult = new PString("sResult");
                        pIrsForm.ActionType = XmActionType.Custom;
                        pIrsForm.ResponseTypeId = pnResponseTypeId;
                        CoreService.DataService.ProcessCustomAction(pIrsForm, psCustomAction, sResult);

                        if (!sFileName.EndsWith(".csv"))
                        {
                            sFileName = Path.ChangeExtension(sFileName, "csv");
                        }

                        try
                        {
                            File.WriteAllText(sFileName, sResult.Value);
                        }
                        catch (IOException)
                        {
                            //Display a message if the file already exists and opened.
                            //15756 - Please select different file name, as Excel can't open two workbooks with same name at same time
                            PMessageBox.Show(15756, MessageType.Warning, MessageBoxButtons.OK, MessageBoxDefaultButton.Button1);
                            return;
                        }
                        //15745 - File exported successfully. Do you want to open the file?
                        if (PMessageBox.Show(15745, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Microsoft.Office.Interop.Excel.Application excelApplication = new Microsoft.Office.Interop.Excel.Application();
                            Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApplication.Workbooks.Open(sFileName);

                            excelApplication.Visible = true;
                            excelApplication.WindowState = Microsoft.Office.Interop.Excel.XlWindowState.xlMaximized;
                            //Get a handle to an application window.
                            IntPtr handler = FindWindow(null, excelApplication.Caption);
                            //Call this method to open the excel over the application-Activates the window.
                            SetForegroundWindow(handler);
                        }
                    }
                }
            }
        }
        // End #71467


        /* Begin Task 76461*/
        public string MaskTin(string tin)
        {
            string maskTin = string.Empty;
            string newMaskedTin = string.Empty;

            if (!string.IsNullOrEmpty(tin))
            {
                int length = tin.Length;
                if (length <= 4)
                {
                    return tin;
                }
                else
                {
                    string unmask = tin.Substring(length - 4, 4);
                    maskTin = tin.Remove(length - 4, 4);
                    foreach (char letter in maskTin)
                    {
                        if (letter != '-')
                        {
                            newMaskedTin += "X";
                        }
                        else newMaskedTin += "-";
                    }
                    newMaskedTin += unmask;
                }
            }
            return newMaskedTin;
        }

        public string MaskAccountNumber(string psAccntNo)
        {
            //Begin #76508
            string sAcctNo;
            string maskAccntNo = string.Empty;

            if (!string.IsNullOrWhiteSpace(psAccntNo))
            {
                sAcctNo = psAccntNo.Trim();
                int length = sAcctNo.Length;
                if (length > 4)
                {
                    maskAccntNo = sAcctNo.Substring(length - 4, 4);
                }
                else
                {
                    maskAccntNo = sAcctNo;
                }
                maskAccntNo = maskAccntNo.PadLeft(12, 'X');
            }
            //End #76508
            return maskAccntNo;
        }
        /* End Task 76461*/
        #endregion

        #region External Methods
        // Begin #71467
        //This code is used in framework code. Windows.sln/pfwstandard.cs . 
        //Code is used to open the excel over the application.
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        //Get a handle to an application window.
        [DllImport("user32.dll", SetLastError = true)]
        static extern System.IntPtr FindWindow(string lpClassName, string lpWindowName);
        // End #71467
        //Begin #95014
        public void GetDefaultAddrId(int rimNo)
        {
            PInt pRimNo = new PInt("RimNo");
            pRimNo.Value = rimNo;
            PString pAddrId = new PString("pAddrId");
            pAddrId.Value = null;
            Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(this, "GetDefaultAddrId", pRimNo, pAddrId);
        }
        //Begin #95014
        //Begin #104153
        public void SetIrsFormCorrectedFlag(string filingStatus, string prevFilingStatus, bool eligibleForStatusChange)
        {
            PString pFilingStatus = new PString("FilingStatus");
            pFilingStatus.Value = filingStatus;
            PString pPrevFilingStatus = new PString("PrevFilingStatus");
            pPrevFilingStatus.Value = prevFilingStatus;
            PBoolean pEligibleForStatusChange = new PBoolean("EligibleForStatusChange");
            pEligibleForStatusChange.Value = eligibleForStatusChange;
            PString pCorrected = new PString("Corrected");
            pCorrected.Value = null;
            Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(this, "SetCorrectedFlag", pFilingStatus, pPrevFilingStatus, pEligibleForStatusChange, pCorrected);
        }
        //End #104153
        #endregion External Methods
    }
}
