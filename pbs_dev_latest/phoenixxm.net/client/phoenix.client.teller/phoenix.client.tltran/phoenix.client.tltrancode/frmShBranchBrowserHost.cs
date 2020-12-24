#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 HFS Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmShBranchBrowserHost.cs
// NameSpace: Phoenix.Windows.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//03/21/2010		1	rpoddar		#79510 - Created
//06/14/2010		1	rpoddar		#79510 - Extended Browser changes
//10/03/2012        3   rpoddar     #19617 - Handled bankId fix
//10/10/2012		4	rpoddar		#140784 - Handled message POST_TRAN_RETRY
//-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Core;
using Phoenix.Shared.Windows;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Admin.Global;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Core.Utilities;

namespace Phoenix.Windows.Client
{
    public partial class frmShBranchBrowserHost : PfwStandard
    {
        PString Url = new PString("Url");
        DateTime lastSubmitTime = DateTime.MinValue;
        bool okToClose = false;
        string lastTPMessage = null;
        int tranCount = -1;
        int tranCountWithRespXML = 0;
        string tranType = null;
        bool isPopWin = false;

        public frmShBranchBrowserHost()
        {
            InitializeComponent();
        }

        public frmShBranchBrowserHost(bool isPopWin )
        {
            this.isPopWin = isPopWin;
            InitializeComponent();
        }

        public override void OnCreateParameters()
        {
            Parameters.Add(Url);
        }
        private ReturnType frmShBranchBrowserHost_PInitBeginEvent()
        {
            bool closeForm = false;

            this.DialogResult = DialogResult.Cancel;    // default is cancel
            this.ScreenId = Phoenix.Shared.Constants.ScreenId.TlSharedBranch;
            PMdiForm mdiForm = PApplication.Instance.MdiMain as PMdiForm;
            if (mdiForm != null && mdiForm.ClientSize.Width > 850 && mdiForm.ClientSize.Height > 650)
            {
                this.Size = new Size(825, 640);
            }
            //this.Size = new Size(825, 600);

            try
            {
                string url = Url.Value;
                string userId = null;
                //string bankId = "100";        // hardcoded
                string bankId = "001";        // hardcoded
                //string custNo = "1";        // hardcoded
                string custNo = null;

                if (!isPopWin)
                {
                    if (url == null || url.Trim() == string.Empty)
                    {
                        //13175 - Url is not configured. Please check your teller system admin settings.
                        PMessageBox.Show(13175, MessageType.Error, string.Empty);
                        closeForm = true;
                        return default(ReturnType);
                    }

                    if (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] != null)
                    {
                        userId = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).UserName.Value;
                        custNo = (GlobalObjects.Instance[GlobalObjectNames.AdGbRsm] as AdGbRsm).EmployeeId.Value.ToString();
                    }
                    //Begin #19617
                    int paramSepIndex = url.IndexOf("?");
                    if (paramSepIndex > 0)
                    {
                        string urlParams = null;
                        if ( url.Length > ( paramSepIndex + 1 )  )
                            urlParams = url.Substring(paramSepIndex + 1);

                        url = url.Substring(0, paramSepIndex + 1);

                        if (urlParams != null)
                        {
                            int bankIdIndex = urlParams.IndexOf("bankId=");
                            if (bankIdIndex >= 0)
                            {
                                bankIdIndex = bankIdIndex + "bankId=".Length;
                                int bankIdEndIndex = urlParams.IndexOf("&", bankIdIndex);
                                if (bankIdEndIndex >= 0)
                                    bankId = urlParams.Substring(bankIdIndex, bankIdEndIndex - bankIdIndex);
                                else
                                    bankId = urlParams.Substring(bankIdIndex);
                            }
                        }
                    }
                    else
                        url = url + "/?";
                    //End #19617

                    url = url + string.Format("userId={0}&branchId={1}&bankId={2}&drawer={3}&custno={4}&tellername={5}&effdate={6}",        // #19617 - removed "/?"
                        EscapeSpecialChars(userId.ToLower(), true),
                        TellerVars.Instance.BranchNo.ToString().PadLeft(3, '0'),
                        bankId,
                        TellerVars.Instance.DrawerNo,
                        custNo,
                        EscapeSpecialChars(TellerVars.EmployeeName, true),
                        TellerVars.Instance.PostingDt.ToString("MMddyyyy"));
                    //url = "https://demo.interpro-tech.com:8181";
                }
                try
                {
                    if (!string.IsNullOrEmpty(url))
                        if ( CoreService.LogPublisher.DebugLevel == LogLevel.MoreDetailed )
                            CoreService.LogPublisher.LogDebug("URL:" + url);

                    webBrowserShBranch.ObjectForScripting = new ExtBrowserInterface(this);
                    webBrowserShBranch.IsWebBrowserContextMenuEnabled = false;
                    webBrowserShBranch.WebBrowserShortcutsEnabled = false;
                    if (!string.IsNullOrEmpty(url))
                        webBrowserShBranch.Navigate(url);
                    //webBrowserShBranch.Scale(new SizeF(0.9F, 0.9F));
                    //webBrowserShBranch.Navigated += new WebBrowserNavigatedEventHandler(webBrowserShBranch_Navigated);
                    //webBrowserShBranch.ScrollBarsEnabled = false;
                    webBrowserShBranch.NewWindow2 += new EventHandler<NewWindow2EventArgs>(webBrowserShBranch_NewWindow2);
                    webBrowserShBranch.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowserShBranch_DocumentCompleted);
                }
                catch (Exception ex)
                {
                    CoreService.LogPublisher.LogError("Error invoking url:" + url);
                    CoreService.LogPublisher.LogError(ex.ToString());
                    PMessageBox.Show(ex);
                    closeForm = true;
                }
                this.IsActionPaneNeeded = false;
                return default(ReturnType);
            }
            finally
            {
                if (closeForm)
                {
                    okToClose = true;
                    this.OnActionClose();   // to set the _isClosing of PfwStandard
                    this.Close();
                }
            }
        }

        void webBrowserShBranch_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //hack to force repaint of the embedded browser
            this.Size = new Size(this.Size.Width - 1, this.Size.Height - 1);
            this.Size = new Size(this.Size.Width + 1, this.Size.Height + 1);
        }

        void webBrowserShBranch_NewWindow2(object sender, NewWindow2EventArgs e)
        {
            frmShBranchBrowserHost popUpForm = new frmShBranchBrowserHost(true);
            popUpForm.Show(this);
            e.PPDisp = popUpForm.webBrowserShBranch.Application;
            popUpForm.webBrowserShBranch.RegisterAsBrowser();
        }

        void webBrowserShBranch_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            //this.Size = new Size(webBrowserShBranch.PreferredSize.Width + 20, webBrowserShBranch.PreferredSize.Height + 20 );

        }

        public override bool ProcessMessage(int messageId, object sender, CancelEventArgs eArg, params object[] paramList)
        {
            bool closeForm = false; ;
            string tpMessage = null;
            string tranCountMsg = "The transaction";
            try
            {
                if (messageId == (int)GlobalActions.NotifyBrowserHost)
                {

                    if (paramList != null && paramList.Length >= 1)
                    {
                        tpMessage = Convert.ToString(paramList[0]);
                        if (tpMessage != null)
                            tpMessage = tpMessage.Trim();
                    }
                    if (tpMessage == "VERIFY_WITH_PHX_UI")
                    {
                        return true;
                    }

                    if (tpMessage == "POST_TRAN_CLICKED" || tpMessage == "REV_TRAN_CLICKED")
                    {
                        lastSubmitTime = DateTime.Now;
                        tranCount++;
                        if (tpMessage == "POST_TRAN_CLICKED")
                            tranType = "POST";
                        else
                            tranType = "REV";
                        return true;
                    }

                    if ( tranCount >= 1 )
                        tranCountMsg = "Transaction " + ( tranCount + 1 ).ToString();

                    if (tpMessage == "NO_XM_COMM_ON_POST" || tpMessage == "NO_XM_COMM_ON_REV")
                    {

                        if (tpMessage == "NO_XM_COMM_ON_POST")
                        {
                            //13176 - %1! could not be posted as Interpro cannot communicate with Phoenix XM server.
                            PMessageBox.Show(13176, MessageType.Error, tranCountMsg ) ;
                        }
                        else
                        {
                            //13178 - The reversal could not be executed as Interpro cannot communicate with Phoenix XM server.
                            PMessageBox.Show(13178, MessageType.Error, string.Empty);
                        }
                        //closeForm = true;
                        if (tranCount < 0)
                            tranCount++;
                        tranCount++;

                    }
                    else if (tpMessage == "NWK_DEC_ON_POST" || tpMessage == "NWK_DEC_ON_REV")       // network decline
                    {

                        if (tpMessage == "NWK_DEC_ON_POST")
                        {
                            //13185 - %1! could not be posted as it was declined by the network.
                            PMessageBox.Show(13185, MessageType.Error, tranCountMsg );
                        }
                        else
                        {
                            //13186 - The reversal could not be executed as it was declined by the network.
                            PMessageBox.Show(13186, MessageType.Error, string.Empty);
                        }
                        //closeForm = true;
                        if (tranCount < 0)
                            tranCount++;
                        tranCount++;

                    }
                    else if (tpMessage == "NO_XM_RESP_ON_POST" || tpMessage == "NO_XM_RESP_ON_REV")
                    {
                        if (tpMessage == "NO_XM_RESP_ON_POST")
                        {
                            //13179 - %1! could not be posted as Interpro did not get a response with Phoenix XM server. Please make sure that the transaction was not posted in the phoenix journal and make manual adjustments if necessary.
                            PMessageBox.Show(13179, MessageType.Error, tranCountMsg );
                        }
                        else
                        {
                            //13180 - The reversal may not have executed successfully as Interpro did not get a response with Phoenix XM server. Please make sure that the reveral occured in the phoenix journal and make manual adjustments if necessary.
                            PMessageBox.Show(13180, MessageType.Error, string.Empty);
                        }
                        //closeForm = true;
                        if (tranCount < 0)
                            tranCount++;
                        tranCount++;
                    }
                    else if (tpMessage == "POST_TRAN_COMPLETED" || tpMessage == "REV_TRAN_COMPLETED")
                    {
                        closeForm = true;
                    }
                    //Begin #140784
                    else if (tpMessage == "POST_TRAN_RETRY" )
                    {
                        //13965 - Do you want to continue processing approved transaction(s) in PhoenixEFE Teller?
                        if (DialogResult.No == PMessageBox.Show(13965, MessageType.Warning, MessageBoxButtons.YesNo, string.Empty))
                        {
                            tranCount = -1;
                            lastSubmitTime = DateTime.MinValue;
                            return true;
                        }
                        return false;
                    }
                    //End #140784
                    else
                    {
                        if (lastSubmitTime != DateTime.MinValue || (tpMessage != null &&
                            (tpMessage.IndexOf(@"customAction=""ReverseTransactions""") > 0 ||
                            tpMessage.IndexOf(@"customAction=""PostTransactions""") > 0)))
                        {
                            //closeForm = true;
                            if (tranCount < 0)
                                tranCount++;
                            tranCount++;
                            Phoenix.Windows.Client.Helper.SendMessageToMDI((int)GlobalActions.ChildAction, this, Convert.ToString(paramList[0]));
                            this.DialogResult = DialogResult.OK;
                            tranCountWithRespXML++;
                        }
                    }
                    return true;
                }
            }
            finally
            {
                lastTPMessage = tpMessage;
                if (closeForm)
                {
                    okToClose = true;
                    this.Close();
                }
            }
            return base.ProcessMessage(messageId, sender, eArg, paramList);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (lastSubmitTime != DateTime.MinValue)
            {
                if (!okToClose)
                {
                    if (lastSubmitTime.AddSeconds(10) > DateTime.Now)
                    {
                        //13181 - Please wait for response for the submitted request.
                        PMessageBox.Show(13181, MessageType.Message, string.Empty);
                        e.Cancel = true;
                    }
                    else
                    {
                        if (tranType == "POST")
                        {
                            //13182 - You are attempting to close the window after the transaction request has been submitted. In case you are sure to close, then click Ok and take appropraite actions by checking the Interpro journal.
                            if ( DialogResult.Cancel ==  PMessageBox.Show(13182, MessageType.Warning, MessageBoxButtons.OKCancel, string.Empty ))
                                e.Cancel = true;
                        }
                        else if (lastTPMessage == "REV_TRAN_CLICKED")
                        {
                            //13183 - You are attempting to close the window after the reversal request has been submitted. In case you are sure to close, then click Ok and take appropraite actions by checking the Interpro journal.
                            if (DialogResult.Cancel == PMessageBox.Show(13183, MessageType.Warning, MessageBoxButtons.OKCancel, string.Empty))
                                e.Cancel = true;
                        }
                    }
                }
            }
            base.OnClosing(e);
        }

        public string EscapeSpecialChars(string input, bool treatEmptyAsSpace)
        {
            if (input == null)
                return null;

            string outputStr = null;

            if (input == string.Empty)
                return treatEmptyAsSpace ? "%20" : null;

            for (int i = 0; i < input.Length; i++)
            {
                bool specialChar = " &<>[]{}?'\"".IndexOf(input[i]) >= 0;

                if (!specialChar)
                    outputStr += input[i];
                else
                    outputStr += "%" + string.Format("{0:X}", Convert.ToInt16(input[i])).PadLeft(2, '0');
            }
            return outputStr;
        }
    }
}