#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
#region ErrorNumbers
// Error Number Range
// Next Available Error Number
#endregion
//-------------------------------------------------------------------------------
// File Name: frmTcdAdmin.cs
// NameSpace: Phoenix.Client.TcdAdmin
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//08/24/2007    1       bbedi       #72916 - Add TCD Support to Teller 2007
//09/11/2007    2       bbedi       #72916 - Add TCD Support in Teller 2007ye, Phase 3.
//09/12/2007    3       bbedi       #72916 - Add TCD Support in Teller 2007ye, Phase 3.
//09/14/2007    4       bbedi       #72916 - Add TCD Support in Teller 2007ye, Phase 3.
//09/142007     5       mselvaga    #72916 - Fixed coin amount for load/remove.
//09/18/2007    6       mselvaga    #72916 - Added fix for offline drawer balances update.
//09/19/2007    7       bbedi       #72916 - Added information messages in Load/Rem.
//09/20/2007    8       BBEDI       #72916 - Issue Fix
//09/20/2007    9       BBEDI       #72916 - Issue Fix
//10/01/2007    10      mselvaga    #72916 - Added fix for F1 key block during TCD load/remove.
//04/04/2008    11      mselvaga    #75726 - QA 72916 - Release 2008 - Add support for TCD machines in .NET Teller - ADD/Remove TCD Cash function in TCD Administrative Function gets an error on F10.
//04/07/2008    12      mselvaga    #75742 - QA Release 2008 TCD - Load/Remove not updating Toolbar Drawer Cash amount.
//04/09/2008    13      mselvaga    #75738 - QA Release 2008 TCD - TCD Vault Buy is not updating TL_JOURNAL and TL_CASH_COUNT correctly.
//04/14/2008    14      mselvaga    #75742 - Used abs of total amount instead of the actual to fix the problem.
//04/14/2008    15      mselvaga    #75864 - QA Release 2008 TCD - When the TCD DispenserTotals is viewed from TCD Administrative Functions, on closing the window re-set .ini for Allow zero inventory
//02/10/2010    16      mselvaga    Enh#79574 - Cash Recycler changes added.
//05/06/2010    17      mselvaga    #79574 - Added fix for clear all and rollover.
//06/09/2010    18      mselvaga    WI#9309 - Added support for manual cash deposit handling.
//06/21/2010    19      mselvaga    WI#9061 - Added sec changes.
//06/25/2010    20      mselvaga    WI#9350 - TCD/TCR Description field should not be validated on Save when TL_DRAWER.drawer_type = 'TCD'.
//06/29/2010    21      mselvaga    WI#9624 - Added TCD/TCR window title fix.
//02/18/2011    22      mselvaga    WI#12836 - Fixed TCDMachineId value when the machine id not exists in tl_drawer.
//09/15/11      23      Vdevadoss   #10955 - Changed the NextScreenId for pbTlrJournal from 10443 to 3090.
//08/06/2012    24      Mkrishna    #19058 - Adding call to base on initParameters.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
//
using Phoenix.Shared.Ucm;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.CDS;
using Phoenix.FrameWork.Core;
using Phoenix.Shared.Constants;
using Phoenix.Windows.Client;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Teller;
using Phoenix.BusObj.Admin.Global;
using Phoenix.FrameWork.Core.Utilities;

namespace Phoenix.Client.TcdAdmin
{
    public partial class frmTcdAdmin : Phoenix.Windows.Forms.PfwStandard
    {
        CashDispenser _cashDispenser = null;
        TellerVars _tellerVars = TellerVars.Instance;
        TlDrawerBalances _tlDrawerBalances = new TlDrawerBalances();
        TlCashCount _tlCashCount = new TlCashCount();
        private AdGbBranch _adGbBranch = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbBranch] as AdGbBranch);
        private bool _isOkToBlockF1 = false;
        private PAction tcdAction; //#72916TBD
        private PComboBoxStandard drawerCombo = null; //#75742
        private PAction iconHoldUp; //#79574

        public frmTcdAdmin()
        {
            InitializeComponent();
        }

        #region comment
        //void pbGetMachine_Click(object sender, EventArgs e)
        //{
        //    int machineId = 0;
        //    string tempId = "";
        //    tempId = _cashDispenser.GetUcmIniFileSettings("Machine ID");
        //    if (tempId != string.Empty)
        //        machineId = Convert.ToInt32(tempId);

        //    if (machineId >= 0)
        //        PMessageBox.Show(300058, MessageType.Message, MessageBoxButtons.OK, new string[] { "Machine ID:" + machineId.ToString() });
        //}

        //void pbOpen_Click(object sender, EventArgs e)
        //{
        //    _cashDispenser.WindowTitle = "Tcd Dispense";
        //    if (_cashDispenser.Open())
        //    {
        //        PMessageBox.Show(300058, MessageType.Message, MessageBoxButtons.OK, new string[] { "Session is Open" });
        //    }
        //}
        #endregion


        #region teller functions

        /* Begin #72916 - Phase 3 */
        private void InitializeCashDispense()
        {

            if (_cashDispenser == null)

                _cashDispenser = CashDispenser.Instance;

        }
        /* End #72916 - Phase 3 */
     
        private void pbSetup_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {

            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361092));

                string tempValue = "";

                bool localIsTcdConnected;
                bool isTcdDrawerExists = true;  //#79574
                bool isValid = true;    //#9350

                localIsTcdConnected = _tellerVars.IsTCDConnected;

                using (new WaitCursor())
                {

                    InitializeCashDispense(); /* Begin #72916 - Phase 3 */
                    //pbSetup.ImageButton.IsAccessible = false;
                    //pbSetup.UpdateControls();
                    if (_cashDispenser.WindowTitle != "TCD/TCR Setup")
                    {
                        _cashDispenser.IsConnOpen = true;
                        _cashDispenser.WindowTitle = "TCD/TCR Setup";
                    }

                    if (_cashDispenser.Setup(_tellerVars.DrawerNo.ToString()))
                    {
                        #region Station Id
                        tempValue = "";
                        _tellerVars.AllowTcdTcrNonMonetaryOnly = false; //#79574

                        _cashDispenser.GetIniFileSettings("General", "Station ID", string.Empty, ref tempValue, string.Empty);
                        if (tempValue != string.Empty)
                            _tellerVars.TcdStationId = tempValue;
                        #endregion

                        #region #79574

                        #region Verify HoldUp
                        if (_tellerVars.IsHoldUpAllowed)
                        {
                            tempValue = "";
                            _tellerVars.IsHoldUpMixExists = false;
                            _cashDispenser.GetIniFileSettings("General", "Holdup Mix", "0,0,0,0,0,0", ref tempValue, string.Empty);
                            if (tempValue != string.Empty)
                            {
                                if (tempValue.Trim() == "0,0,0,0,0,0")
                                    _tellerVars.IsHoldUpMixExists = false;
                                else
                                    _tellerVars.IsHoldUpMixExists = true;
                            }
                        }
                        #endregion

                        #region Set Expected Dep Amt    #9309
                        _cashDispenser.SetIniFileSettings("General", "Expected Deposit Amount", "NO", string.Empty);
                        #endregion

                        #region Get Expected Dep Amt
                        tempValue = "";
                        _cashDispenser.GetIniFileSettings("General", "Expected Deposit Amount", string.Empty, ref tempValue, string.Empty);
                        if (tempValue != string.Empty)
                            _tellerVars.ValidateExpectedDepAmt = (tempValue.Trim() == "YES" ? true : false);
                        #endregion

                        #region Get Branch#    #79574
                        tempValue = "";
                        _cashDispenser.GetIniFileSettings("General", "Branch No", string.Empty, ref tempValue, string.Empty);
                        if (tempValue != string.Empty)
                        {
                            isValid = true;
                            for (int i = 0; i < tempValue.Length; i++)
                            {
                                if ("0123456789".IndexOf(tempValue[i].ToString()) == -1)
                                    isValid = false;
                            }

                            if (isValid)
                            {
                                if (Convert.ToDecimal(tempValue) > System.Int16.MaxValue)
                                    isValid = false;

                                if (!isValid)
                                {
                                    dfBranch.Text = Convert.ToString(_tellerVars.BranchNo) + " - " + _adGbBranch.Name1.Value;
                                    _tellerVars.AllowTcdTcrNonMonetaryOnly = true;
                                }
                                else
                                {
                                    if (Convert.ToInt16(tempValue) != _tellerVars.BranchNo)
                                    {
                                        _tellerVars.AllowTcdTcrNonMonetaryOnly = true;
                                        AdGbBranch tempBranch = new AdGbBranch();
                                        tempBranch.BranchNo.Value = Convert.ToInt16(tempValue);
                                        tempBranch.SelectAllFields = false;
                                        tempBranch.Name1.Selected = true;
                                        tempBranch.ActionType = XmActionType.Select;
                                        //
                                        DataService.Instance.ProcessRequest(tempBranch);
                                        //
                                        if (!tempBranch.Name1.IsNull)
                                        {
                                            PMessageBox.Show(this, 13218, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                                            dfBranch.Text = Convert.ToString(tempValue) + " - " + tempBranch.Name1.Value;
                                        }
                                        else
                                        {
                                            PMessageBox.Show(this, 13219, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                                            dfBranch.Text = tempValue;
                                        }

                                    }
                                    else
                                        dfBranch.Text = Convert.ToString(_tellerVars.BranchNo) + " - " + _adGbBranch.Name1.Value;
                                }
                            }
                        }
                        #endregion

                        #region Machine Id
                        tempValue = "";
                        _cashDispenser.GetIniFileSettings("General", "Machine ID", string.Empty, ref tempValue, string.Empty);

                        if (_tellerVars.TcdMachineId != Convert.ToInt32(tempValue))
                        {
                            //#79574

                            #region Get TCD cash acct & tcd drawer#
                            TlDrawer _tlTCDDrawer = new TlDrawer();
                            _tlTCDDrawer.ActionType = XmActionType.Select;
                            _tlTCDDrawer.BranchNo.Value = _tellerVars.BranchNo;
                            _tlTCDDrawer.TcdDeviceNo.Value = Convert.ToInt32(tempValue);
                            _tlTCDDrawer.SelectAllFields = false;
                            _tlTCDDrawer.DrawerNo.Selected = true;
                            _tlTCDDrawer.GlCashAcct.Selected = true;
                            //
                            try
                            {
                                DataService.Instance.ProcessRequest(_tlTCDDrawer);
                                //
                                if (_tlTCDDrawer.DrawerNo.IsNull || _tlTCDDrawer.DrawerNo.Value <= 0)
                                    isTcdDrawerExists = false;
                            }
                            catch (PhoenixException pe)
                            {
                                CoreService.LogPublisher.LogError(pe.ToString());
                                isTcdDrawerExists = false;
                            }
                            finally
                            {
                                _tellerVars.TcdMachineId = Convert.ToInt32(tempValue);  //#12836
                                if (isTcdDrawerExists)
                                {
                                    _tellerVars.TcdDrawerNo = _tlTCDDrawer.DrawerNo.Value;
                                    _tellerVars.TcdCashAcctNo = _tlTCDDrawer.GlCashAcct.Value;
                                    //_tellerVars.TcdMachineId = Convert.ToInt32(tempValue);    //#12836 - moved out of the if block
                                }
                                else
                                {
                                    //#79574
                                    PMessageBox.Show(this, 361074, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                                    _tellerVars.AllowTcdTcrNonMonetaryOnly = true;
                                }
                            }
                            #endregion

                            if (!_tellerVars.IsRecycler)
                            {
                                _cashDispenser.IgnoreReturnString = true;
                                _cashDispenser.GetStatus(_tellerVars.DrawerNo.ToString());
                                _cashDispenser.IgnoreReturnString = false;

                                if (_cashDispenser.ReturnCode == 116)
                                {
                                    //361078 - You have modified the TCD Machine ID in the Setup function and it does not match TCD Machine ID for this workstation.  The TCD Machine IDs in Setup and on the TCD Drawer must match.  Please reset the TCD Machine ID.
                                    PMessageBox.Show(this, 361078, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                                    _tellerVars.AllowTcdTcrNonMonetaryOnly = true;
                                    return;
                                }
                                else
                                {
                                    _tellerVars.TcdMachineId = Convert.ToInt32(tempValue);
                                }
                            }
                        }
                        #endregion

                        #endregion
                        EnableDisableVisibleLogic("TCDSetup");   //#79574
                        _cashDispenser.Open();
                        _tellerVars.IsTCDConnected = localIsTcdConnected;
                    }



                    #region Set Eliminate Active Totals
                    _cashDispenser.SetIniFileSettings("General", "Eliminate Active Totals", "YES", string.Empty);
                    #endregion


                    #region Set Eliminate Station Summary
                    _cashDispenser.SetIniFileSettings("General", "Eliminate Station Summary", "YES", string.Empty);
                    #endregion
                }
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        private void pbStatus_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361091));

                InitializeCashDispense();   /* Begin #72916 - Phase 3 */
                if (_cashDispenser.WindowTitle != "TCD/TCR Status")
                {
                    _cashDispenser.IsConnOpen = false;
                    _cashDispenser.WindowTitle = "TCD/TCR Status";
                }
                _cashDispenser.GetStatus(_tellerVars.DrawerNo.ToString(), false, _devOutputInfo);
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        private void pbLoadRem_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            #region OldCode
            //_devOutputInfo = new System.Collections.ArrayList();
            //TcdCashOutDenoms = new System.Collections.ArrayList(); //#72916*
            

            //bool coinUpdated = false;
            //int denomCount = 0;
            //decimal denomValue = 0;
            //decimal coinAmount = 0;
            //totalAmount = 0;
            
            //try
            //{
            //    /* #72916 - Issue Fix */
            //    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361100));

            //    //LOAD
            //    InitializeCashDispense();   /* Begin #72916 - Phase 3 */
            //    _isOkToBlockF1 = true;
            //    tcdAction.Shortcut = Keys.F1;
            //    dlgInformation.Instance.HideInfo(); //#72916*
            //    if (_cashDispenser.Load(_tellerVars.DrawerNo.ToString(), _devOutputInfo))
            //    {
            //        #region IdentifyTran
            //        if (_devOutputInfo != null)
            //        {
            //            /* #72916 - Issue Fix */
            //            if (_devOutputInfo.Count > 0) //#72916*
            //            {

            //                foreach (DeviceOutputDetails devOut in _devOutputInfo)
            //                {
            //                    if (devOut.DenOutputType.ToString() == "T")
            //                    {
            //                        totalAmount = devOut.DispReqAmt;
            //                        break; //#72916 - Added break
            //                    }
            //                }

            //                #region TLDrawerBalances
            //                if (totalAmount != 0)
            //                {
            //                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361089));
            //                    //
            //                    TranAmount.ValueObject = totalAmount;
            //                    FromForward.ValueObject = false;

            //                    _tlDrawerBalances.BranchNo.Value = _tellerVars.BranchNo;
            //                    _tlDrawerBalances.DrawerNo.Value = _tellerVars.DrawerNo;
            //                    _tlDrawerBalances.CrncyId.Value = 1;
            //                    _tlDrawerBalances.TcdDrawerNo.Value = _tellerVars.TcdDrawerNo;
            //                    _tlDrawerBalances.TcdMachineId.Value = Convert.ToInt16(_tellerVars.TcdMachineId);
            //                    _tlDrawerBalances.TellerCashAcct.Value = _tellerVars.CashAcctNo;
            //                    _tlDrawerBalances.OtherCashAcct.Value = _tellerVars.TcdCashAcctNo;
            //                    _tlDrawerBalances.SystemDt.Value = GlobalVars.SystemDate;
            //                    //_tlDrawerBalances.SuperEmplId.Value = 0;
            //                    _tlDrawerBalances.ClosedDt.Value = _tellerVars.PostingDt;
            //                    _tlDrawerBalances.TcdDrawerPosition.Value = _tellerVars.TcdStationId; //#72916*

            //                    try
            //                    {
            //                        DataService.Instance.ProcessCustomAction(_tlDrawerBalances, "TcdLoadRemoveCash", TranAmount, TlJrnlPtid, (totalAmount > 0? "L" : "R"), FromForward);    //#79574
            //                    }

            //                    catch (PhoenixException pe)
            //                    {
            //                        PMessageBox.Show(pe);
            //                        return;
            //                    }
            //                }
            //                #endregion

            //                #region LoadTcdCashOutDenom

            //                #region LoadObject
            //                if (!TlJrnlPtid.IsNull && TlJrnlPtid.DecimalValue > 0)
            //                {
            //                    _tlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
            //                    _tlCashCount.BranchNo.Value = _tellerVars.BranchNo;
            //                    _tlCashCount.DrawerNo.Value = _tellerVars.DrawerNo;
            //                    _tlCashCount.EffectiveDt.Value = GlobalVars.SystemDate;
            //                    if (totalAmount > 0)
            //                    {
            //                        _tlCashCount.LoadCashCountDenom(TcdCashOutDenoms,
            //                     CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O"), true);
            //                    }
            //                    else
            //                    {
            //                        _tlCashCount.LoadCashCountDenom(TcdCashOutDenoms,
            //                     CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I"), true);
            //                    }
            //                }
            //                #endregion

            //                if (_devOutputInfo != null)
            //                {
            //                    #region LoadCashDenomCounts
            //                    foreach (DeviceOutputDetails devOut in _devOutputInfo)
            //                    {
            //                        if (devOut.DenomCount > 0 && devOut.DenOutputType.ToString() == "B") //#75738
            //                        {
            //                            denomCount = devOut.DenomCount;
            //                            denomValue = devOut.DenomValue;
            //                            foreach (Phoenix.BusObj.Teller.TlCashCount tcdCount in TcdCashOutDenoms)
            //                            {
            //                                if (tcdCount.Denom.Value == (decimal)devOut.Denom && tcdCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "LB"))
            //                                {
            //                                    tcdCount.Quantity.Value = denomCount;
            //                                    tcdCount.Amt.Value = (decimal)denomValue;
            //                                    break;
            //                                }
            //                            }
            //                        }
            //                        if (!coinUpdated && devOut.DenOutputType.ToString() == "C")
            //                        {
            //                            foreach (Phoenix.BusObj.Teller.TlCashCount tcdCount in TcdCashOutDenoms)
            //                            {
            //                                coinAmount = devOut.CoinAmount; //#72916                                   
            //                                if (tcdCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "BC"))
            //                                {
            //                                    tcdCount.CountValue.Value = (decimal)coinAmount;
            //                                    tcdCount.Quantity.Value = 1;
            //                                    tcdCount.Amt.Value = (decimal)coinAmount;
            //                                    break;
            //                                }
            //                            }
            //                            coinUpdated = true;
            //                        }


            //                    }
            //                    #endregion

            //                    #region SaveCashOutDenom

            //                    Phoenix.FrameWork.CDS.DataService.Instance.Reset();

            //                    foreach (Phoenix.BusObj.Teller.TlCashCount cashOutDenom in TcdCashOutDenoms)
            //                    {
            //                        if (cashOutDenom.Amt.Value > 0)
            //                        {
            //                            cashOutDenom.JournalPtid.Value = Convert.ToInt32(TlJrnlPtid.ValueObject);
            //                            cashOutDenom.RowVersion.Value = 1;
            //                            if (totalAmount > 0)
            //                            {
            //                                cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O"));
            //                            }
            //                            else
            //                            {
            //                                cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I"));
            //                            }
            //                            cashOutDenom.ActionType = XmActionType.New;
            //                            Phoenix.FrameWork.CDS.DataService.Instance.AddObject(cashOutDenom);

            //                            Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
            //                            Phoenix.FrameWork.CDS.DataService.Instance.Reset();
            //                        }
            //                    }
            //                    //
            //                    //#72916*
            //                    //Let's update the denoms for both sides
            //                    foreach (Phoenix.BusObj.Teller.TlCashCount cashOutDenom in TcdCashOutDenoms)
            //                    {
            //                        if (cashOutDenom.Amt.Value > 0)
            //                        {
            //                            if (totalAmount > 0)
            //                            {
            //                                cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "L"));
            //                            }
            //                            else
            //                            {
            //                                cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "D"));
            //                            }
            //                            cashOutDenom.ActionType = XmActionType.New;
            //                            Phoenix.FrameWork.CDS.DataService.Instance.AddObject(cashOutDenom);

            //                            Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
            //                            Phoenix.FrameWork.CDS.DataService.Instance.Reset();
            //                        }
            //                    }

            //                    #endregion
            //                }
            //                #endregion

            //                #region offline drawer bal update
            //                if (totalAmount != 0)
            //                {
            //                    decimal offlineUpdAmt = totalAmount;
            //                    PBaseType ReversalFlag = new PBaseType("ReversalFlag");
            //                    PBaseType TlTranCode = new PBaseType("TlTranCode");
            //                    PBaseType TranCode = new PBaseType("TranCode");
            //                    PBaseType BatchId = new PBaseType("BatchId");
            //                    PBaseType ItemCount = new PBaseType("ItemCount");
            //                    if (offlineUpdAmt < 0)
            //                    {
            //                        offlineUpdAmt = offlineUpdAmt * -1;
            //                    }

            //                    _tlDrawerBalances.CashIn.Value = offlineUpdAmt;
            //                    _tlDrawerBalances.CashOut.Value = offlineUpdAmt;
            //                    _tlDrawerBalances.Deposits.Value = offlineUpdAmt;
            //                    //
            //                    ReversalFlag.ValueObject = false;
            //                    TlTranCode.ValueObject = (totalAmount < 0 ? "REM" : "LOD");
            //                    TranCode.ValueObject = -1;
            //                    BatchId.ValueObject = -1;
            //                    ItemCount.ValueObject = -1;

            //                    //#75726 - call offlineCDS only when offlineCDS not null
            //                    //if (!_tellerVars.IsAppOnline)
            //                    if (_tellerVars.OfflineCDS != null) //#75726
            //                        _tellerVars.OfflineCDS.ProcessCustomAction(_tlDrawerBalances, "UpdateBalancesForTran",
            //                            ReversalFlag, TlTranCode, TranCode, BatchId, ItemCount);
            //                }
            //                #endregion

            //                //Begin #75742
            //                #region update drawer combo
            //                UpdateDrawerBalance();
            //                #endregion 
            //                //End #75742
            //            }
            //        }
            //        #endregion

            //    }
            //}
            //finally
            //{
            //    if (_devOutputInfo != null && _devOutputInfo.Count != 0)
            //    {
            //        /* #72916 - Issue Fix */
            //        dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361090));
            //    }
            //    dlgInformation.Instance.HideInfo();
            //    _isOkToBlockF1 = false;
            //    tcdAction.Shortcut = Keys.None;
            //}
            #endregion
            //
            LoadRemoveDeplete("L"); //#79574
        }

        private void pbTotals_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361093));

                InitializeCashDispense();   /* Begin #72916 - Phase 3 */
                #region Turn ON Allow Inventory Zeros to supress Rollover
                //#79574
                //_cashDispenser.SetIniFileSettings("General", "Allow Inventory Zeroes", "YES", string.Empty);
                #endregion
                //_cashDispenser.Open();
                if (_cashDispenser.WindowTitle != "TCD/TCR Totals")
                {
                    _cashDispenser.IsConnOpen = false;
                    _cashDispenser.WindowTitle = "TCD/TCR Totals";
                }
                //#79574 - made code change to pass in silent mode to true to supress any change to totals window
                _cashDispenser.GetTotals(_tellerVars.DrawerNo.ToString(), true, _devOutputInfo, true);  //#79574 - added clear all/ rollover change
            }
            finally
            {
                //Begin #75864
                #region Turn OFF Allow Inventory Zeros
                //_cashDispenser.SetIniFileSettings("General", "Allow Inventory Zeroes", "NO", string.Empty);
                #endregion
                //End #75864
                dlgInformation.Instance.HideInfo();
            }
        }

        private void pbTcdJournal_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361094));

                InitializeCashDispense();   /* Begin #72916 - Phase 3 */
                if (_cashDispenser.WindowTitle != "TCD/TCR Journal")
                {
                    _cashDispenser.IsConnOpen = false;
                    _cashDispenser.WindowTitle = "TCD/TCR Journal";
                }
                _cashDispenser.ViewJournal();
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        private void pbTcdHistory_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361095));

                InitializeCashDispense();   /* Begin #72916 - Phase 3 */
                if (_cashDispenser.WindowTitle != "TCD/TCR History")
                {
                    _cashDispenser.IsConnOpen = false;
                    _cashDispenser.WindowTitle = "TCD/TCR History";
                }
                _cashDispenser.ViewHistory();
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        private void pbTlrJournal_Click(object sender, PActionEventArgs e)
        {
            try
            {
                /* #72916 - Issue Fix */
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361096));

                PfwStandard tempWin = null;

                tempWin = Helper.CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournal");

                tempWin.InitParameters(GlobalVars.Instance.ML.Y,
                    Convert.ToInt16(_tellerVars.BranchNo),
                    Convert.ToInt16(_tellerVars.DrawerNo),
                    Convert.ToDateTime(_tellerVars.PostingDt),
                    GlobalVars.Instance.ML.Open,
                    null,
                    false,
                    Convert.ToDateTime(_tellerVars.PostingDt),
                    16015);

                tempWin.Workspace = this.Workspace;
                tempWin.Show();
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }

        void pbDeplete_Click(object sender, PActionEventArgs e)
        {
            LoadRemoveDeplete("D");
        }

        void pbReset_Click(object sender, PActionEventArgs e)
        {
            try
            {
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11593));

                InitializeCashDispense();
                _cashDispenser.Reset(_tellerVars.DrawerNo.ToString());
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }
        #endregion

        private ReturnType frmTcdAdmin_PInitBeginEvent()
        {
            if (_tellerVars.IsRecycler)   //#79574
                pbLoadRem.ObjectId = 12;
            return default(ReturnType);
        }

        private void frmTcdAdmin_PInitCompleteEvent()
        {
            dfBranch.Text = Convert.ToString(_tellerVars.BranchNo) + " - " + _adGbBranch.Name1.Value;
            dfMachineId.Text = Convert.ToString(_tellerVars.TcdMachineId);
            dfStationId.Text = _tellerVars.TcdStationId;

            dfBranch.Enabled = false;
            dfMachineId.Enabled = false;
            dfStationId.Enabled = false;
            //
            EnableDisableVisibleLogic("FormComplete");
        }
        //#72916*
        protected override void OnHelpRequested(HelpEventArgs hevent)
        {
            if (_isOkToBlockF1)
                hevent.Handled = true;
            else
                base.OnHelpRequested(hevent);
        }
        public override void InitParameters(params object[] paramList)
        {
            if (paramList.Length >= 1)
            {
                tcdAction = paramList[0] as PAction;
                tcdAction.Visible = false; //#72916
                drawerCombo = paramList[1] as PComboBoxStandard; //#75742
                iconHoldUp = paramList[2] as PAction; //#79574
            }

            //base.InitParameters(paramList); //#19058
        }

        //#75742
        private void UpdateDrawerBalance(string tcdTcrAdminTranType)
        {
            if (totalAmount < 0 || tcdTcrAdminTranType == "D")  //#79574
                _tellerVars.DrawerCash = _tellerVars.DrawerCash + (tcdTcrAdminTranType == "D"? totalAmount : (totalAmount * -1)); //#75742
            else
                _tellerVars.DrawerCash = _tellerVars.DrawerCash - totalAmount;

            if (drawerCombo != null)
            {
                drawerCombo.Items.Clear();
                drawerCombo.Items.Add(CurrencyHelper.GetFormattedValue(_tellerVars.DrawerCash));
                drawerCombo.SelectedIndex = 0;
            }
            // TO DO update the drawer combo
            if (_tellerVars.DrawerCash < _tellerVars.AdGbRsmLimits.LowDrawerLim.Value)
            {
                drawerCombo.ForeColor = System.Drawing.Color.Blue;
                PMessageBox.Show(311350, MessageType.Warning, MessageBoxButtons.OK, "USD");
            }
            else if (_tellerVars.DrawerCash > _tellerVars.AdGbRsmLimits.HighDrawerLim.Value)
            {
                drawerCombo.ForeColor = System.Drawing.Color.Red;
                PMessageBox.Show(311349, MessageType.Warning, MessageBoxButtons.OK, "USD");
            }
            else
                drawerCombo.ForeColor = System.Drawing.Color.Black;
        }

        private void LoadRemoveDeplete(string tcdTcrAdminTranType)
        {
            _devOutputInfo = new System.Collections.ArrayList();
            TcdCashOutDenoms = new System.Collections.ArrayList(); //#72916*
            PBaseType TcdAdminTranType = new PBaseType("TcdAdminTranType");
            

            bool coinUpdated = false;
            int denomCount = 0;
            decimal denomValue = 0;
            decimal coinAmount = 0;
            totalAmount = 0;
            bool isSuccess = false;
            string loadRemoveTitle = (_tellerVars.IsRecycler ? "TCR Load" : "TCD Load/Remove");

            try
            {
                /* #72916 - Issue Fix */
                if (tcdTcrAdminTranType == "D")
                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11594));
                else
                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361100));

                //LOAD
                InitializeCashDispense();   /* Begin #72916 - Phase 3 */
                _isOkToBlockF1 = true;
                tcdAction.Shortcut = Keys.F1;
                dlgInformation.Instance.HideInfo(); //#72916*
                if (tcdTcrAdminTranType == "D")
                {
                    if (_cashDispenser.WindowTitle != "TCR Deplete")
                    {
                        _cashDispenser.IsConnOpen = false;
                        _cashDispenser.WindowTitle = "TCR Deplete";
                    }
                    isSuccess = _cashDispenser.Deplete(_tellerVars.DrawerNo.ToString(), _tellerVars.DrawerNo.ToString(), _devOutputInfo);
                }
                else
                {

                    if (_cashDispenser.WindowTitle != loadRemoveTitle)
                    {
                        _cashDispenser.IsConnOpen = false;
                        _cashDispenser.WindowTitle = loadRemoveTitle;
                    }
                    isSuccess = _cashDispenser.Load(_tellerVars.DrawerNo.ToString(), _devOutputInfo);
                }
                if (isSuccess)
                {
                    #region IdentifyTran
                    if (_devOutputInfo != null)
                    {
                        /* #72916 - Issue Fix */
                        if (_devOutputInfo.Count > 0) //#72916*
                        {

                            foreach (DeviceOutputDetails devOut in _devOutputInfo)
                            {
                                if (devOut.DenOutputType.ToString() == "T")
                                {
                                    totalAmount = devOut.DispReqAmt;
                                    break; //#72916 - Added break
                                }
                            }

                            #region TLDrawerBalances
                            if (totalAmount != 0)
                            {
                                if (!_tellerVars.IsRecycler)
                                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361089));
                                else
                                {
                                    if (tcdTcrAdminTranType == "D")
                                        dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11606));
                                    else
                                        dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11605));
                                }
                                //
                                TranAmount.ValueObject = totalAmount;
                                FromForward.ValueObject = false;

                                _tlDrawerBalances.BranchNo.Value = _tellerVars.BranchNo;
                                _tlDrawerBalances.DrawerNo.Value = _tellerVars.DrawerNo;
                                _tlDrawerBalances.CrncyId.Value = 1;
                                _tlDrawerBalances.TcdDrawerNo.Value = _tellerVars.TcdDrawerNo;
                                _tlDrawerBalances.TcdMachineId.Value = Convert.ToInt16(_tellerVars.TcdMachineId);
                                _tlDrawerBalances.TellerCashAcct.Value = _tellerVars.CashAcctNo;
                                _tlDrawerBalances.OtherCashAcct.Value = _tellerVars.TcdCashAcctNo;
                                _tlDrawerBalances.SystemDt.Value = GlobalVars.SystemDate;
                                //_tlDrawerBalances.SuperEmplId.Value = 0;
                                _tlDrawerBalances.ClosedDt.Value = _tellerVars.PostingDt;
                                _tlDrawerBalances.TcdDrawerPosition.Value = _tellerVars.TcdStationId; //#72916*
                                TcdAdminTranType.Value = (tcdTcrAdminTranType == "D"? "D" : (totalAmount > 0 ? "L" : "R"));

                                try
                                {
                                    DataService.Instance.ProcessCustomAction(_tlDrawerBalances, "TcdLoadRemoveCash", TranAmount, TlJrnlPtid, TcdAdminTranType, FromForward);    //#79574
                                }

                                catch (PhoenixException pe)
                                {
                                    PMessageBox.Show(pe);
                                    return;
                                }
                            }
                            #endregion

                            #region LoadTcdCashOutDenom

                            #region LoadObject
                            if (!TlJrnlPtid.IsNull && TlJrnlPtid.DecimalValue > 0)
                            {
                                _tlCashCount = new Phoenix.BusObj.Teller.TlCashCount();
                                _tlCashCount.BranchNo.Value = _tellerVars.BranchNo;
                                _tlCashCount.DrawerNo.Value = _tellerVars.DrawerNo;
                                _tlCashCount.EffectiveDt.Value = GlobalVars.SystemDate;
                                if (totalAmount > 0 && tcdTcrAdminTranType != "D")  //#79574
                                {
                                    _tlCashCount.LoadCashCountDenom(TcdCashOutDenoms,
                                 CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O"), true);
                                }
                                else
                                {
                                    _tlCashCount.LoadCashCountDenom(TcdCashOutDenoms,
                                 CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I"), true);
                                }
                            }
                            #endregion

                            if (_devOutputInfo != null)
                            {
                                #region LoadCashDenomCounts
                                foreach (DeviceOutputDetails devOut in _devOutputInfo)
                                {
                                    if (devOut.DenomCount > 0 && devOut.DenOutputType.ToString() == "B") //#75738
                                    {
                                        denomCount = devOut.DenomCount;
                                        denomValue = devOut.DenomValue;
                                        foreach (Phoenix.BusObj.Teller.TlCashCount tcdCount in TcdCashOutDenoms)
                                        {
                                            if (tcdCount.Denom.Value == (decimal)devOut.Denom && tcdCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "LB"))
                                            {
                                                tcdCount.Quantity.Value = denomCount;
                                                tcdCount.Amt.Value = (decimal)denomValue;
                                                break;
                                            }
                                        }
                                    }
                                    if (!coinUpdated && devOut.DenOutputType.ToString() == "C")
                                    {
                                        foreach (Phoenix.BusObj.Teller.TlCashCount tcdCount in TcdCashOutDenoms)
                                        {
                                            coinAmount = devOut.CoinAmount; //#72916                                   
                                            if (tcdCount.DenomType.Value == CoreService.Translation.GetListItemX(ListId.DENOMTYPES, "BC"))
                                            {
                                                tcdCount.CountValue.Value = (decimal)coinAmount;
                                                tcdCount.Quantity.Value = 1;
                                                tcdCount.Amt.Value = (decimal)coinAmount;
                                                break;
                                            }
                                        }
                                        coinUpdated = true;
                                    }


                                }
                                #endregion

                                #region SaveCashOutDenom

                                Phoenix.FrameWork.CDS.DataService.Instance.Reset();

                                foreach (Phoenix.BusObj.Teller.TlCashCount cashOutDenom in TcdCashOutDenoms)
                                {
                                    if (cashOutDenom.Amt.Value > 0)
                                    {
                                        cashOutDenom.JournalPtid.Value = Convert.ToInt32(TlJrnlPtid.ValueObject);
                                        cashOutDenom.RowVersion.Value = 1;
                                        if (totalAmount > 0 && tcdTcrAdminTranType != "D")  //#79574
                                        {
                                            cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "O"));
                                        }
                                        else
                                        {
                                            cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "I"));
                                        }
                                        cashOutDenom.ActionType = XmActionType.New;
                                        Phoenix.FrameWork.CDS.DataService.Instance.AddObject(cashOutDenom);

                                        Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
                                        Phoenix.FrameWork.CDS.DataService.Instance.Reset();
                                    }
                                }
                                //
                                //#72916*
                                //Let's update the denoms for both sides
                                foreach (Phoenix.BusObj.Teller.TlCashCount cashOutDenom in TcdCashOutDenoms)
                                {
                                    if (cashOutDenom.Amt.Value > 0)
                                    {
                                        if (totalAmount > 0 && tcdTcrAdminTranType != "D")  //#79574
                                        {
                                            cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "L"));
                                        }
                                        else
                                        {
                                            cashOutDenom.SetTranType(CoreService.Translation.GetListItemX(ListId.DENOMTRANTYPE, "D"));
                                        }
                                        cashOutDenom.ActionType = XmActionType.New;
                                        Phoenix.FrameWork.CDS.DataService.Instance.AddObject(cashOutDenom);

                                        Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
                                        Phoenix.FrameWork.CDS.DataService.Instance.Reset();
                                    }
                                }

                                #endregion
                            }
                            #endregion

                            #region offline drawer bal update
                            if (totalAmount != 0)
                            {
                                decimal offlineUpdAmt = totalAmount;
                                PBaseType ReversalFlag = new PBaseType("ReversalFlag");
                                PBaseType TlTranCode = new PBaseType("TlTranCode");
                                PBaseType TranCode = new PBaseType("TranCode");
                                PBaseType BatchId = new PBaseType("BatchId");
                                PBaseType ItemCount = new PBaseType("ItemCount");
                                if (offlineUpdAmt < 0)
                                {
                                    offlineUpdAmt = offlineUpdAmt * -1;
                                }

                                _tlDrawerBalances.CashIn.Value = offlineUpdAmt;
                                _tlDrawerBalances.CashOut.Value = offlineUpdAmt;
                                _tlDrawerBalances.Deposits.Value = offlineUpdAmt;
                                //
                                ReversalFlag.ValueObject = false;
                                TlTranCode.ValueObject = (tcdTcrAdminTranType == "D"? "DPL" : (totalAmount < 0 ? "REM" : "LOD"));   //#79574
                                TranCode.ValueObject = -1;
                                BatchId.ValueObject = -1;
                                ItemCount.ValueObject = -1;

                                //#75726 - call offlineCDS only when offlineCDS not null
                                //if (!_tellerVars.IsAppOnline)
                                if (_tellerVars.OfflineCDS != null) //#75726
                                    _tellerVars.OfflineCDS.ProcessCustomAction(_tlDrawerBalances, "UpdateBalancesForTran",
                                        ReversalFlag, TlTranCode, TranCode, BatchId, ItemCount);
                            }
                            #endregion

                            //Begin #75742
                            #region update drawer combo
                            UpdateDrawerBalance(tcdTcrAdminTranType);
                            #endregion
                            //End #75742
                        }
                    }
                    #endregion

                }
            }
            finally
            {
                if (_devOutputInfo != null && _devOutputInfo.Count != 0)
                {
                    /* #72916 - Issue Fix */
                    dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(361090));
                }
                dlgInformation.Instance.HideInfo();
                _isOkToBlockF1 = false;
                tcdAction.Shortcut = Keys.None;
            }
        }


        private void EnableDisableVisibleLogic(string origin)   //#79574
        {
            if (origin == "TCDSetup")
            {
                dfStationId.UnFormattedValue = _tellerVars.TcdStationId;
                dfMachineId.UnFormattedValue = _tellerVars.TcdMachineId;
                iconHoldUp.Enabled = _tellerVars.IsHoldUpMixExists;
                pbLoadRem.Enabled = (!_tellerVars.AllowTcdTcrNonMonetaryOnly);
                pbDeplete.Enabled = (!_tellerVars.AllowTcdTcrNonMonetaryOnly);
            }
            if (origin == "FormComplete")
            {
                pbDeplete.Visible = pbReset.Visible = _tellerVars.IsRecycler;  //#79574
                if (_tellerVars.IsNoDrawerOptionSelected || _tellerVars.ClosedOut ||
                    (_tellerVars.PostingDt < GlobalVars.SystemDate) || _tellerVars.AllowTcdTcrNonMonetaryOnly) //#72916*    #79574
                {
                    pbLoadRem.Enabled = false;
                    pbDeplete.Enabled = false;
                }
            }
        }

    }
}
