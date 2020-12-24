#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Interface class to determine whether to launch the touche messenger window.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: clsmessengerinvoker.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/27/06		1		VDevadoss	#69248 - Created
//11/01/11		2		VDevadoss	#15978 - Added check for NULL Rim Number before calling the 'GetToucheMessage' method.
//12/27/2011    3       LSimpson    #15828 - Modifications to make window get message data.  Moved messenger code to window.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Windows.Forms;
using Phoenix.BusObj.Misc;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Admin;
using Phoenix.FrameWork.BusFrame;

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for messengerinvoker.
	/// </summary>
	public class messengerinvoker
	{
		private Phoenix.BusObj.Admin.Global.AdGbRsm _adGbRsm = new Phoenix.BusObj.Admin.Global.AdGbRsm();
		private Phoenix.BusObj.Misc.Messenger _tMsgr = new Phoenix.BusObj.Misc.Messenger();
		private int nAcctRimNo = int.MinValue; 
		private string sAcctType = string.Empty;
		private string sAcctNo = string.Empty;
		private int nAcctId = int.MinValue;
		string sToucheMsg = string.Empty;
		private DialogResult dialogResult = DialogResult.None;
		
		#region Constructor
		public messengerinvoker(params Object[] paramList)
        {
            #region #15828 - Commented and moved functionality to window
            //Get the AD_GB_RSM Details.
            //#region Iterate the AD_GB_RSM object to get the s&s flags
            //this._adGbRsm.EmployeeId.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeId;
            //this._adGbRsm.ActionType = XmActionType.Select;
            //this._adGbRsm.EnumType = XmEnumerationType.EnumAll;			
            //Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _adGbRsm );
            //#endregion

            ////Get the touche setup details.
            ////this._tMsgr.GetMsgrControlDetails();

            //this._tMsgr.MsgrPopUp.Value = this._adGbRsm.SalesPopUp.Value;

            //if (this._tMsgr.MsgrPopUp.Value == "Y" )
            //{
            //    if (paramList[0] != null)
            //        nAcctRimNo = Convert.ToInt32(paramList[0]);

            //    if (paramList[1] != null)
            //        sAcctType = paramList[1].ToString();
				
            //    if (paramList[2] != null)
            //        sAcctNo = paramList[2].ToString();
				
            //    if (paramList[3] != null)
            //        nAcctId = Convert.ToInt32(paramList[3]);

            //    //Get the Touche message			
            //    /*Begin #15978*/
            //    if (nAcctRimNo <= 0)
            //    {
            //        if (Phoenix.FrameWork.Core.CoreService.LogPublisher.IsLogEnabled)
            //        {
            //            Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("**** ERROR - The RIM Number sent to Touche Messemger is: " + nAcctRimNo + ".  A valid RIM Number is required for fetch message from Touche.");
            //        }

            //        this._tMsgr.DestroyMsgrWindow.Value = "Y";
            //    }
            //    else
            //    {
            //        sToucheMsg = this._tMsgr.GetToucheMessage(nAcctRimNo);
            //    }
                /*End #15978*/
            //}
            #endregion
        }

		#endregion

		public void InvokeToucheMsgr()
		{
			if (this._tMsgr.MsgrPopUp.Value == "Y" && this._tMsgr.DestroyMsgrWindow.Value != "Y")
			{
				//Invoke the actual messenger window.
				PfwStandard tempDlg = Phoenix.Windows.Client.Helper.CreateWindow( "phoenix.client.messenger","Phoenix.Client.WorkQueue","frmOnMessenger");
				tempDlg.InitParameters(nAcctRimNo, sAcctType.Trim(), sAcctNo.Trim(), nAcctId, sToucheMsg, this._adGbRsm.SalesAcceptMethod.Value);
				dialogResult = tempDlg.ShowDialog();
			}

			return;
		}
	}
}
