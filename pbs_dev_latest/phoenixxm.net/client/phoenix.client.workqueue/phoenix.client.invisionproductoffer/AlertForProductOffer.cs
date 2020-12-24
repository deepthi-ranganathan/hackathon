#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

//-------------------------------------------------------------------------------
// File Name: frmInvisionMessenger.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	    Change              
//-------------------------------------------------------------------------------
//1/6/2017      1       GlaxeenaJ       Created
//1/6/2017      2       GlaxeenaJ       Enh #209693, US #65380, Task #65382  Product Offer Window for teller and Cust Mgnt as an Alert, then to dialogue box
//29/6/2017     3       GlaxeenaJ       Enh #209693, US #65380, Task #67801  Product Offer Screen Implementation - Autopinning productoffer alert on loading

//-------------------------------------------------------------------------------
#endregion


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Alerter;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Windows.Client;
using System.Collections;
using Phoenix.Shared.Images;

namespace Phoenix.Client.WorkQueue
{
    public partial class AlertForProductOffer : UserControl
    {
        #region Private var
        private AlertControl ctrlAlert = new AlertControl();
        private Phoenix.BusObj.Misc.BiInvision _biInvision = new Phoenix.BusObj.Misc.BiInvision();
        private string sAlertText = "";
        private string sAlertCaption = "";
        private string sHyperLinkedAlertText = "";
        private string sInvisionMsg = string.Empty;
        private int nRimNo;
        private Hashtable acctsTrackHash = new Hashtable();
        private DialogResult dialogResult = DialogResult.None;
        PfwStandard tempDlg = null;
        private string sCampaignOfferKey = "";
        private Form _parentWindow;
        private IPhoenixForm _parentForm;
        private Point _location = new Point(0, 0);



        #endregion

        #region Private  Properties
        #endregion

        #region Public Properties
        #endregion

        #region Public Methods

        public AlertForProductOffer()
        {
            InitializeComponent();
            Design();
        }

        public void ShowAlert(int RimNo, string CustName, Form parentWindow, IPhoenixForm parent)
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }

            _parentWindow = parentWindow;
            _parentForm = parent;

            nRimNo = RimNo;
            ctrlAlert.Show(null, sAlertCaption, sAlertText, sHyperLinkedAlertText);
            if (_location.X == 0)
            {
                _location = ctrlAlert.AlertFormList[0].Location;
            }
            parentWindow.FormClosed += ParentWindow_FormClosed;
            // GetMessengerData(RimNo);
            //if (sInvisionMsg == null || sInvisionMsg.Trim() == "")
            //{
            //    Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("*** AlertForProductOffer - No data from Invision Server");
            //    Phoenix.FrameWork.Core.CoreService.LogPublisher.LogError("*** AlertForProductOffer - No data from Invision Server");
            //}
            //else
            //{
            //    if (sInvisionMsg.Length > 100)
            //    {
            //        sAlertText = sInvisionMsg.Substring(0, 100);
            //    }
            //    else
            //    {
            //        sAlertText = sInvisionMsg;
            //    }

            //    sAlertCaption = "Product Offers - " + CustName;
            //    sCampaignOfferKey = Convert.ToString(_biInvision.CampaignOfferLogKey.Value);
            //    ctrlAlert.Show(null, sAlertCaption, sAlertText, sHyperLinkedAlertText);
            //    if (_location.X == 0)
            //    {
            //        _location = ctrlAlert.AlertFormList[0].Location;
            //    }
            //    parentWindow.FormClosed += ParentWindow_FormClosed;
            //}

        }

        public void ShowAlertTeller(int RimNo, string CustName,string isExists, Form parentWindow, IPhoenixForm parent)
        {

            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }

            _parentWindow = parentWindow;
            _parentForm = parent;

            nRimNo = RimNo;
            //GetMessengerData(RimNo);
            //if (sInvisionMsg == null || sInvisionMsg.Trim() == "")
            //{
            //    Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("*** AlertForProductOffer - No data from Invision Server");
            //    Phoenix.FrameWork.Core.CoreService.LogPublisher.LogError("*** AlertForProductOffer - No data from Invision Server");
            //}
            //else
            //{
            //    if (sInvisionMsg.Length > 100)
            //    {
            //        sAlertText = sInvisionMsg.Substring(0, 100);
            //    }
            //    else
            //    {
            //        sAlertText = sInvisionMsg;
            //    }

               sAlertCaption = "Customer Analysis Information";
            //    sCampaignOfferKey = Convert.ToString(_biInvision.CampaignOfferLogKey.Value);
            //    ctrlAlert.Show(null, sAlertCaption, sAlertText, sHyperLinkedAlertText);
            //    if (_location.X == 0)
            //    {
            //        _location = ctrlAlert.AlertFormList[0].Location;
            //    }
            //    parentWindow.FormClosed += ParentWindow_FormClosed;
            //}
            if (isExists == "Y")
            {
                ctrlAlert.Show(null, sAlertCaption, sAlertText, sHyperLinkedAlertText);
                if (_location.X == 0)
                {
                    _location = ctrlAlert.AlertFormList[0].Location;
                }
                parentWindow.FormClosed += ParentWindow_FormClosed;
            }
        }

        public void CloseAlertForProductOffer()
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }
        }
        #endregion

        #region Private Methods

        private enum CallOtherForm
        {
            DeferClick,
            AcceptClick,
            DeclineClick
        }
        private void ParentWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }
        }

        public void alert_ButtonClose(object sender, AlertFormClosingEventArgs e)
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();

            }
        }

        private void alert_AlertClick(object sender, AlertClickEventArgs e)
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }
            CallOtherForms(CallOtherForm.DeferClick);
        }

        private void GetMessengerData(int pnRimNo)
        {
            Phoenix.BusObj.Admin.Global.AdGbRsm adGbRsmObj = new Phoenix.BusObj.Admin.Global.AdGbRsm();
            //Get the AD_GB_RSM Details.
            #region Iterate the AD_GB_RSM object to get the s&s flags
            adGbRsmObj = (Phoenix.BusObj.Admin.Global.AdGbRsm)Phoenix.FrameWork.Core.Utilities.GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm];
            #endregion
            this._biInvision.MsgrPopUp.Value = adGbRsmObj.SalesPopUp.Value;
            if (this._biInvision.MsgrPopUp.Value == "Y")
            {
                if (pnRimNo <= 0)
                {
                    if (Phoenix.FrameWork.Core.CoreService.LogPublisher.IsLogEnabled)
                    {
                        Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("**** ERROR - The RIM Number sent to Invision Messemger is: " + pnRimNo + ".  A valid RIM Number is required for fetch message from Invision.");
                    }
                }
                else
                {
                    this._biInvision.GetInvisionMessage(pnRimNo);
                    sInvisionMsg = this._biInvision.MessageText.Value;
                }
            }
        }

        private void alert_ButtonClick(object sender, AlertButtonClickEventArgs e)
        {
            if (ctrlAlert.AlertFormList.Count > 0)
            {
                ctrlAlert.AlertFormList[0].Close();
            }
            if (e.ButtonName == "buttonDefer")
            {
                CallOtherForms(CallOtherForm.DeferClick);
            }
            else if (e.ButtonName == "buttonAccept")
            {
                CallOtherForms(CallOtherForm.AcceptClick);
            }
            else if (e.ButtonName == "buttonDecline")
            {
                CallOtherForms(CallOtherForm.DeclineClick);

            }
        }


        private void CallOtherForms(CallOtherForm caseName)
        {
            dlgInformation.Instance.ShowInfo("Checking for Product Offers...");
            switch (caseName)
            {
                case CallOtherForm.DeferClick:
                    Callwindow("radioDeferClicked");
                    break;
                case CallOtherForm.AcceptClick:
                    Callwindow("radioAcceptClicked");
                    break;
                case CallOtherForm.DeclineClick:
                    Callwindow("radioDeclineClicked");
                    break;
            }
            dlgInformation.Instance.HideInfo();
            if (tempDlg != null)
            {
                dialogResult = tempDlg.ShowDialog();
            }
        }

        private void Callwindow(string RadioClickedVal)
        {
            _parentForm = _parentWindow as PfwStandard;
            tempDlg = Helper.CreateWindow("phoenix.client.invisionproductoffer", "Phoenix.Client.WorkQueue", "frmCustAnalysis");
            tempDlg.InitParameters(nRimNo);
        }

        private void CtrlAlert_BeforeFormShow(object sender, DevExpress.XtraBars.Alerter.AlertFormEventArgs e)
        {
            e.AlertForm.OpacityLevel = 1;
            if (_location.X != 0)
            {
                e.Location = _location;
            }
        }

        private void Design()
        {


            //AlertButton pbDefer = new AlertButton(TellerImages.RealTime);
            //pbDefer.Hint = "Defer Until Later";
            //pbDefer.Name = "buttonDefer";

            AlertButton pbAccept = new AlertButton(TellerImages.RemoteTellerOvrd);
            pbAccept.Style = AlertButtonStyle.Button;
            pbAccept.Hint = "Show Info";
            pbAccept.Name = "buttonAccept";

            //AlertButton pbDecline = new AlertButton(TellerImages.Offline);
            //pbDecline.Style = AlertButtonStyle.Button;
            //pbDecline.Hint = "Decline Offer";
            //pbDecline.Name = "buttonDecline";

            ctrlAlert.Buttons.Clear();
          //  ctrlAlert.Buttons.Add(pbDefer);
            ctrlAlert.Buttons.Add(pbAccept);
          //  ctrlAlert.Buttons.Add(pbDecline);

            ctrlAlert.ButtonClick += new AlertButtonClickEventHandler(alert_ButtonClick);
            ctrlAlert.AlertClick += new AlertClickEventHandler(alert_AlertClick);
            ctrlAlert.FormLoad += CtrlAlert_FormLoad;
            ctrlAlert.BeforeFormShow += CtrlAlert_BeforeFormShow;

            ctrlAlert.AutoHeight = true;
            ctrlAlert.AllowHotTrack = true;
            ctrlAlert.FormLocation = AlertFormLocation.BottomRight;
            ctrlAlert.AutoFormDelay = 15000;
            ctrlAlert.ShowCloseButton = false;
            ctrlAlert.FormMaxCount = 1;
        }

        //#task 67801
        private void CtrlAlert_FormLoad(object sender, AlertFormLoadEventArgs e)
        {
            e.Buttons.PinButton.SetDown(true);
        }

        #endregion

    }
}
