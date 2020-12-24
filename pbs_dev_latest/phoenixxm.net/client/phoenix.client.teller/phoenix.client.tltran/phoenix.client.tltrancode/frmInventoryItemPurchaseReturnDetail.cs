#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2012 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: Shark Form1.cs
// NameSpace: Phoenix.Client.TlTranCode
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//9/28/2012 11:30:35 AM			1		mselvaga    Created
//01/31/2012                    2       mselvaga    #20598 - Fixed the incorrect non-customer parameter value.
//04/09/2014                    3       FOyebola    CR#23981
//04/30/2014                    4       FOyebola    WI#28568
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;

using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Global;
using Phoenix.Shared.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.Shared.Constants;

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
    public partial class frmInventoryItemPurchaseReturnDetail : PfwStandard
    {
        #region Private Variables

        private PDecimal _pnTypeId = new PDecimal("TypeId");
        private PDecimal _pnPacketId = new PDecimal("PacketId");
        private PInt _pnRimNo = new PInt("RimNo");
        private PBoolean _pbNonCustomer = new PBoolean("NonCustomer");
        private PString _psCustName = new PString("CustName");
        private PInt _pnNoOfItems = new PInt("NoOfItems");
        private PSmallInt _pnBranchNo = new PSmallInt("BranchNo");
        private PSmallInt _pnDrawerNo = new PSmallInt("DrawerNo");
        private PSmallInt _pnTranCode = new PSmallInt("TranCode");
        private PString _psClass = new PString("Class");
        //private Phoenix.BusObj.Global.GbInventoryItem _gbInventoryItem;
        private Phoenix.BusObj.Admin.Global.AdGbInventoryType _adGbInventoryType = new BusObj.Admin.Global.AdGbInventoryType();
        private Phoenix.BusObj.Admin.Global.AdGbInventoryPacket _adGbInventoryPacket = new BusObj.Admin.Global.AdGbInventoryPacket();
        //private List<GbInventoryItem> _availableList = null;
        //private List<GbInventoryItem> _purchReturnList = null;
        private ArrayList AvailableList = null;
        private ArrayList PurchReturnList = null;
        private PBoolean bEdited = new PBoolean("bEdited");
        private int nEditCount = 0;
        private int _availRowNo = 0;
        private int _purchReturnRowNo = 1;
        private decimal _totalAvailAmt = 0;
        private decimal _totalPurchReturnAmt = 0;
        private decimal _currentAmt = 0;
        private decimal _totalCurrentAmt = 0;
        private decimal _totalAdjCurrentAmt = 0;
        private decimal _upchargeAmt = 0;
        private decimal _totalUpchargeAmt = 0;
        private decimal _itemValue = 0;
        private decimal _totalItemValue = 0;
        private bool _isUpchargeAmtEdited = false;
        private bool _isItemValueEdited = false;
        private decimal _totalPurchReturnUpchargeAmt = 0;
        private bool _isZeroItemValueTran = false;
        private bool _isUpchargeAmtOvrdEnabled = false;     //#21085
        private bool _isItemValueAmtOvrdEnabled = false;    //#21085
        private decimal _totalDiscountOvrdAmt = 0;          //#21085
        //private decimal _tempCurrentValue = 0;
        //private decimal _tempUpchargeValue = 0;
        //private ArrayList _storedPurchReturnList = null;
        private Phoenix.BusObj.Teller.TlTransactionSet _tlTranSet = null;

        private Boolean bPopulatingGrids = false; //23981


        /// <summary>
        /// This enum contains the various conditions which will enable/disable push buttons
        /// </summary>
        private enum EnableDisableVisible
        {
            InitBegin,
            InitComplete
        }

        private enum CallOtherForm
        {
            EditClick
        }

        enum MoveDirection
        {
            Left = 2,
            Right = 1

        }
        #endregion

        #region Constructors
        public frmInventoryItemPurchaseReturnDetail()
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
            this.ScreenId = Phoenix.Shared.Constants.ScreenId.InventoryPurchaseReturnDetail;
            bEdited.Value = false;
            nEditCount = 0;
            this.ActionClose.ObjectId = 25;

            //Setting the MainBusinesObject business ojbect is required in order to use the SetBusObjectsToProcess
            //If you dont set the main business object the method will not fire.  Since I don't  use the main business
            //object setting it to an empty instance of the pcOrigin  business object.
            GbInventoryItem _gbInventoryItemCpy = new GbInventoryItem();
            _gbInventoryItemCpy.Ptid.Value = -1;
            _gbInventoryItemCpy.DiscountType.Value = "Amount";
            _gbInventoryItemCpy.ResponseTypeId = 13;
            dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Amount;
            this.MainBusinesObject = _gbInventoryItemCpy;
            return ReturnType.Success;
        }

        void frmInventoryItemPurchaseReturnDetail_PInitCompleteEvent()
        {
            if (!_pnTypeId.IsNull)
            {
                dfTypeId.UnFormattedValue = _adGbInventoryType.TypeId.StringValue + " - " + _adGbInventoryType.Description.Value;
                dfTypeId.Text = _adGbInventoryType.TypeId.StringValue + " - " + _adGbInventoryType.Description.Value;

                dfClass.UnFormattedValue = _adGbInventoryType.Class.StringValue;
                _isItemValueAmtOvrdEnabled = (!_adGbInventoryType.AllowValueOverride.IsNull && _adGbInventoryType.AllowValueOverride.Value == "Y");  //#21085
                _isUpchargeAmtOvrdEnabled = (!_adGbInventoryType.AllowUpchargeOverride.IsNull && _adGbInventoryType.AllowUpchargeOverride.Value == "Y");  //#21085
            }

            if (!_pnPacketId.IsNull)
            {
                //Begin #23981
                if (_pnPacketId.Value == -1)  // All packets
                {
                    dfPacketId.UnFormattedValue = "All Packets  -  ";
                    dfPacketDesc.Visible = true;
                }
                else
                {
                    _adGbInventoryPacket.TypeId.Value = _pnTypeId.Value;
                    _adGbInventoryPacket.PacketId.Value = _pnPacketId.Value;
                    _adGbInventoryPacket.ActionType = XmActionType.Select;
                    CoreService.DataService.ProcessRequest(_adGbInventoryPacket);
                    //
                    dfPacketId.UnFormattedValue = _adGbInventoryPacket.PacketId.StringValue + " - " + _adGbInventoryPacket.Description.Value;
                }
                //End #23981
            }

            if (!_pnRimNo.IsNull)
            {
                dfRimNo.UnFormattedValue = _pnRimNo.Value;
                dfName.UnFormattedValue = _psCustName.Value;
            }

            cbNonCustomer.Checked = (!_pbNonCustomer.IsNull && _pbNonCustomer.Value == true);

            // Formatting Discount and Percent
            this.dfTotalDiscOvrd.PhoenixUIControl.MakeFormattedValueEvent += new MakeFormattedValueEventHandler(MakeAmountPercentFieldsFormattedValue);
            this.dfTotalDiscOvrd.PhoenixUIControl.MakeUnformattedValueEvent += new MakeUnformattedValueEventHandler(MakeAmountPercentFieldsUnFormatedFormattedValue);
            if (this.rbPercent.Checked)
                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Percent;
            else
                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Amount;

            SetPurcReturnLabels();

            if (!_tlTranSet.CurTran.Class.IsNull && _tlTranSet.CurTran.Class.Value == CoreService.Translation.GetListItemX(ListId.ML_InventoryClass, "Serial"))
            {
                if (_adGbInventoryType.ItemValue.IsNull || (!_adGbInventoryType.ItemValue.IsNull && _adGbInventoryType.ItemValue.Value == 0))
                {
                    _isZeroItemValueTran = true;
                    colCurrentAmt1.ReadOnly = false;
                }
                else
                    colCurrentAmt1.ReadOnly = true;
            }

            if (_tlTranSet.CurTran.InventoryItemsAvailable != null && _tlTranSet.CurTran.InventoryItemsAvailable.Count > 0)
            {
                LoadInventoryItems(_tlTranSet.CurTran.InventoryItemsAvailable, false);
                bEdited.Value = false;
                grdAvailableList.Select();
                grdAvailableList.SelectRow(0);
            }

            if (_tlTranSet.CurTran.InventoryItems != null && _tlTranSet.CurTran.InventoryItems.Count > 0)
            {
                LoadInventoryItems(_tlTranSet.CurTran.InventoryItems, true);
                //RemoveStoredItemsFromAvailable();
                PopulateArrays(false);
                bEdited.Value = false;
                grdPurchReturnList.Select();
                //grdAvailableList.SelectRow(0);
            }

            //Begin #23981
            if ((!_pnPacketId.IsNull) && _pnPacketId.Value == -1)  // All packets
            {
                grdAvailableList_SelectedIndexChanged(null, null);
            }
            //End #23981

            EnableDisableVisibleLogic(EnableDisableVisible.InitComplete);
            this.IsAutoCloseAfterSave = true;   //#20598
        }  
        #endregion

        #region Grid Events

        #region available list
        void grdAvailableList_BeforePopulate(object sender, Windows.Forms.GridPopulateArgs e)
        {
            ////Populating the grid
            //_gbInventoryItem = new BusObj.Global.GbInventoryItem();
            //_gbInventoryItem.ResponseTypeId = 13;

            ////clearing the filters
            //this.grdAvailableList.Filters.Clear();

            //_gbInventoryItem.FilterStatus.Value = "Available";
            //if (!_pnBranchNo.IsNull)
            //{
            //    _gbInventoryItem.FilterBranch.Value = _pnBranchNo.Value;
            //}
            //else
            //{
            //    _gbInventoryItem.FilterBranch.Value = -1;
            //}

            //if (!_pnDrawerNo.IsNull)
            //{
            //    _gbInventoryItem.FilterDrawer.Value = _pnDrawerNo.Value;
            //}
            //else
            //{
            //    _gbInventoryItem.FilterDrawer.Value = -1;
            //}

            //if (!_pnTypeId.IsNull)
            //{
            //    _gbInventoryItem.FilterTypeId.Value = int.Parse(_pnTypeId.StringValue);
            //}
            //else
            //{
            //    _gbInventoryItem.FilterTypeId.Value = -1;
            //}

            //if (!_pnPacketId.IsNull)
            //{
            //    _gbInventoryItem.FilterPacketId.Value = _pnPacketId.Value;
            //}
            //else
            //{
            //    _gbInventoryItem.FilterPacketId.Value = -1;
            //}

            //if (!_pnNoOfItems.IsNull)
            //{
            //    _gbInventoryItem.FilterNoOfItems.Value = _pnNoOfItems.Value;
            //}
            //else
            //{
            //    _gbInventoryItem.FilterNoOfItems.Value = -1;
            //}

            //if (!_pnRimNo.IsNull && !_pnTranCode.IsNull && _pnTranCode.Value == 939)
            //{
            //    _gbInventoryItem.FilterSoldTo.Value = _pnRimNo.Value;
            //    _gbInventoryItem.FilterStatus.Value = "Sold";
            //    //#20598
            //    _gbInventoryItem.FilterNonCust.Value = ((_pbNonCustomer.IsNull || (!_pbNonCustomer.IsNull && _pbNonCustomer.Value == false)) ? false : true);
            //}
            //else
            //{
            //    _gbInventoryItem.FilterSoldTo.Value = -1;
            //}     

            //this.grdAvailableList.ListViewObject = _gbInventoryItem;
        }

        void grdAvailableList_FetchRowDone(object sender, Windows.Forms.GridRowArgs e)
        {
            //if (_availRowNo == 0)
            //{
            //    if (_psClass.Value == CoreService.Translation.GetListItemX(ListId.ML_InventoryClass, "Serial") &&
            //        (colItemValue.UnFormattedValue == null || (colItemValue.UnFormattedValue != null && Convert.ToDecimal(colItemValue.UnFormattedValue) == 0)) &&
            //        (colUpchargeAmt.UnFormattedValue == null || (colUpchargeAmt.UnFormattedValue != null && Convert.ToDecimal(colUpchargeAmt.UnFormattedValue) == 0)))
            //        colCurrentAmt1.ReadOnly = false;
            //    else
            //        colCurrentAmt1.ReadOnly = true;
            //}
            //_availRowNo += 1;
            //colRowNo.UnFormattedValue = _availRowNo;

            //if (_pnTranCode.Value != 938)
            //{
            //    colItemValue.UnFormattedValue = colSoldAmt.UnFormattedValue;
            //    colUpchargeAmt.UnFormattedValue = 0;
            //    colPacketItemValue.UnFormattedValue = 0;
            //    colCurrentAmt.UnFormattedValue = colSoldAmt.UnFormattedValue;
            //}
            //else
            //{
            //    _tempCurrentValue = 0;
            //    if (colPacketItemValue.UnFormattedValue != null && Convert.ToDecimal(colPacketItemValue.UnFormattedValue) > 0)
            //        _tempCurrentValue = Convert.ToDecimal(colPacketItemValue.UnFormattedValue);
            //    else
            //        _tempCurrentValue = Convert.ToDecimal(colItemValue.UnFormattedValue);
            //    if (!_adGbInventoryType.UpchargeType.IsNull && _adGbInventoryType.UpchargeType.Value == "P")  //#21476
            //        _tempUpchargeValue = (colUpchargeAmt.UnFormattedValue == null ? 0 : decimal.Round(((_tempCurrentValue * Convert.ToDecimal(colUpchargeAmt.UnFormattedValue)) / 100), 2));
            //    else
            //        _tempUpchargeValue = (colUpchargeAmt.UnFormattedValue == null ? 0 : Convert.ToDecimal(colUpchargeAmt.UnFormattedValue));
            //    _tempCurrentValue = _tempCurrentValue + _tempUpchargeValue;
            //    colCurrentAmt.UnFormattedValue = _tempCurrentValue;
            //}
            //_totalAvailAmt += Convert.ToDecimal(colCurrentAmt.UnFormattedValue);
        }

        void grdAvailableList_AfterPopulate(object sender, Windows.Forms.GridPopulateArgs e)
        {
            //if (_tlTranSet.CurTran.InventoryItems != null && _tlTranSet.CurTran.InventoryItems.Count > 0)
            //{
            //    PopulateArrays(false);
            //}
            //else
            //{
            //    PopulateArrays(false);
            //    dfTotalAvailCount.UnFormattedValue = _availRowNo;
            //    dfTotalAvailAmt.UnFormattedValue = _totalAvailAmt;
            //    _availRowNo = 0;
            //}
            //bEdited.Value = false;
            //grdAvailableList.SelectRow(0);
        }
        #endregion

        #region purchase/return list
        void colCurrentAmt1_PhoenixUIValidateEvent(object sender, Windows.Forms.PCancelEventArgs e)
        {
            _totalPurchReturnAmt -= (colPrevCurrentAmt1.UnFormattedValue == null ? 0 : Convert.ToDecimal(colPrevCurrentAmt1.UnFormattedValue));
            _totalPurchReturnAmt += (colCurrentAmt1.UnFormattedValue == null ? 0 : Convert.ToDecimal(colCurrentAmt1.UnFormattedValue));
            if (_isZeroItemValueTran && (colPrevCurrentAmt1.UnFormattedValue == null ? 0 : Convert.ToDecimal(colPrevCurrentAmt1.UnFormattedValue)) !=
                (colCurrentAmt1.UnFormattedValue == null ? 0 : Convert.ToDecimal(colCurrentAmt1.UnFormattedValue)))
                bEdited.Value = true;
            colItemValue1.UnFormattedValue = (colCurrentAmt1.UnFormattedValue == null ? 0 : Convert.ToDecimal(colCurrentAmt1.UnFormattedValue));
            dfTotalPurchAmt.UnFormattedValue = decimal.Round(_totalPurchReturnAmt, 2);
            dfTotalPurchAmt.UnFormattedValue = CalcNetPurchReturnAmt();
            colPrevCurrentAmt1.UnFormattedValue = colCurrentAmt1.UnFormattedValue;
        }
        #endregion


        private void RemoveStoredItemsFromAvailable()
        {
            ArrayList myTempList = new ArrayList();
            decimal amt = 0;
            string itemNoList = string.Empty;

            bPopulatingGrids = true;  //28568

            if (_tlTranSet.CurTran.InventoryItems != null && _tlTranSet != null)
            {
                //if (grdPurchReturnList.Count > 0)
                if (PurchReturnList != null && PurchReturnList.Count > 0)
                {
                    foreach (GlacialComponents.Controls.GLItem itemPurch in PurchReturnList)
                    {
                        if (string.IsNullOrEmpty(itemNoList))
                            itemNoList = itemPurch.SubItems[0].Text;
                        else
                            itemNoList = itemNoList + "," + itemPurch.SubItems[0].Text;
                    }

                    for (int i = 0; i <= AvailableList.Count - 1; i++)
                    {
                        GlacialComponents.Controls.GLItem itemAvail = (GlacialComponents.Controls.GLItem)AvailableList[i];
                        if (itemNoList.IndexOf(itemAvail.SubItems[0].Text) < 0)
                        {
                            myTempList.Add(itemAvail);
                            amt += Convert.ToDecimal(itemAvail.SubItems[4].Text.Replace("$", ""));
                        }
                    }

                    //clearing the available items list table and re adding the items and subitmes to the grid.
                    grdAvailableList.Items.Clear();
                    grdAvailableList.ResetTable();

                    foreach (GlacialComponents.Controls.GLItem item in myTempList)
                    {
                        GlacialComponents.Controls.GLItem listItem = grdAvailableList.Items[grdAvailableList.AddNewRow()];
                        MoveSubsItems(item, listItem, grdAvailableList.Count, MoveDirection.Right);   
                    }

                    dfTotalAvailCount.UnFormattedValue = grdAvailableList.Count;
                    dfTotalAvailAmt.UnFormattedValue = amt;
                    _availRowNo = grdAvailableList.Count;   //#20598
                    _totalAvailAmt = amt;   //#20598
                }
            }
            else
            {
                dfTotalAvailCount.UnFormattedValue = _availRowNo;
                dfTotalAvailAmt.UnFormattedValue = _totalAvailAmt;
            }

            bPopulatingGrids = false;  //28568
        }

        ///// <summary>
        ///// Move row move the row from one grid to the other based on the direction.
        ///// </summary>
        ///// <param name="direction"></param>
        private void MoveRow(MoveDirection direction)
        {
            int nLowestSelectedIndicies = 0;
            int nHighestSelectedIndicies = 0;
            if (PurchReturnList == null)
                PurchReturnList = new ArrayList();


            bPopulatingGrids = true;  //28568

            switch (direction)
            {
                case MoveDirection.Left:

                    //Checking to make sure a row is selected.
                    if (grdPurchReturnList.Items.SelectedItems.Count > 0)
                    {

                        //Setting the lowest and highest indicies since I am removing the rows so it doesnt error.
                        nLowestSelectedIndicies = Convert.ToInt32(grdPurchReturnList.SelectedIndicies[0]);
                        nHighestSelectedIndicies = Convert.ToInt32(grdPurchReturnList.SelectedIndicies[grdPurchReturnList.SelectedIndicies.Count - 1]);
                        //Iterate forward through the selectedindicies to load the items into arraylist
                        //Iterate backward through the selectedindicies to remove the items from the collection
                        for (int nSelectedIndex = nLowestSelectedIndicies; nSelectedIndex <= nHighestSelectedIndicies; nSelectedIndex++)
                        {
                            if (grdPurchReturnList.Items[nSelectedIndex].Selected)
                            {
                                //Setting the contextRow.
                                grdPurchReturnList.ContextRow = nSelectedIndex;

                                if (Convert.ToString(colPurchReturnModified.UnFormattedValue) == Convert.ToString("Y"))
                                {
                                    colPurchReturnModified.UnFormattedValue = Convert.ToString("N");
                                    if (nEditCount > 0)
                                    {
                                        nEditCount -= 1;
                                    }
                                }
                                else
                                {
                                    colPurchReturnModified.UnFormattedValue = Convert.ToString("Y");
                                    nEditCount += 1;
                                    //_currentAmt = Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colCurrentAmt1.ColumnId));
                                    //_upchargeAmt = Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colUpchargeAmt1.ColumnId));
                                    //_totalCurrentAmt += _currentAmt;
                                    //_totalUpchargeAmt += _upchargeAmt;
                                    //if (_psClass.Value == CoreService.Translation.GetListItemX(ListId.ML_InventoryClass, "Serial") &&
                                    //      (colItemValue1.UnFormattedValue == null || (colItemValue1.UnFormattedValue != null && Convert.ToDecimal(colItemValue1.UnFormattedValue) == 0)) &&
                                    //      (colUpchargeAmt1.UnFormattedValue == null || (colUpchargeAmt1.UnFormattedValue != null && Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue) == 0)))
                                    //{
                                    //    colCurrentAmt1.UnFormattedValue = Convert.ToDecimal(colItemValue1.UnFormattedValue) + Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue);
                                    //}
                                }
                                _currentAmt = Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colCurrentAmt1.ColumnId));
                                _upchargeAmt = Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colUpchargeAmt1.ColumnId));
                                _itemValue = Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colItemValue1.ColumnId)) +
                                               Convert.ToDecimal(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colPacketItemValue1.ColumnId));
                                _currentAmt = (_currentAmt < 0 ? 0 : _currentAmt);
                                _upchargeAmt = (_upchargeAmt < 0 ? 0 : _upchargeAmt);
                                _itemValue = (_itemValue < 0 ? 0 : _itemValue);
                                _totalCurrentAmt += _currentAmt;
                                _totalUpchargeAmt += _upchargeAmt;
                                _totalItemValue += _itemValue;
                                //if (_psClass.Value == CoreService.Translation.GetListItemX(ListId.ML_InventoryClass, "Serial") &&
                                //      (colItemValue1.UnFormattedValue == null || (colItemValue1.UnFormattedValue != null && Convert.ToDecimal(colItemValue1.UnFormattedValue) == 0)) &&
                                //      (colUpchargeAmt1.UnFormattedValue == null || (colUpchargeAmt1.UnFormattedValue != null && Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue) == 0)))
                                //{
                                //    colCurrentAmt1.UnFormattedValue = Convert.ToDecimal(colItemValue1.UnFormattedValue) + Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue);
                                //    _isZeroAmtTran = true;
                                //}

                                if (_isZeroItemValueTran)
                                {
                                    colItemValue1.UnFormattedValue = Convert.ToDecimal(0);
                                    colCurrentAmt1.UnFormattedValue = Convert.ToDecimal(colItemValue1.UnFormattedValue) + Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue);
                                    colPrevCurrentAmt1.UnFormattedValue = Convert.ToDecimal(colCurrentAmt1.UnFormattedValue);
                                }

                                //adding the item to the purch/return grid.
                                if (Convert.ToInt32(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colItemExists1.ColumnId)) != 0)
                                    AvailableList.Add(grdPurchReturnList.Items[nSelectedIndex]);
                                if (Convert.ToInt32(grdPurchReturnList.GetCellValueUnformatted(nSelectedIndex, colItemExists1.ColumnId)) == 0 ||
                                    _isZeroItemValueTran)
                                    _totalAdjCurrentAmt += _currentAmt;
                            }
                        }

                        //Iterating backwards through the selectedIndicies array in order to remove the rows and not get the index out of range.
                        for (int nSelectedIndex = nHighestSelectedIndicies; nSelectedIndex >= nLowestSelectedIndicies; nSelectedIndex--)
                        {
                            if (grdPurchReturnList.Items[nSelectedIndex].Selected)
                            {
                                //Setting the contextRow.
                                grdPurchReturnList.ContextRow = nSelectedIndex;

                                for (int i = 0; i <= PurchReturnList.Count - 1; i++)
                                {
                                    GlacialComponents.Controls.GLItem item = (GlacialComponents.Controls.GLItem)PurchReturnList[i];
                                    if (colPurchReturnItemNo.UnFormattedValue.ToString() == item.SubItems[0].Text)
                                    {
                                        PurchReturnList.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        //clearing the purch/return items list table and re adding the items and subitmes to the grid.
                        grdPurchReturnList.Items.Clear();
                        grdPurchReturnList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in PurchReturnList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdPurchReturnList.Items[grdPurchReturnList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdPurchReturnList.Count, direction);
                            if (Convert.ToInt32(listItem.SubItems[19].Text) == 0)       //19 - colItemExists1
                                listItem.ForeColor = Color.Red;
                        }

                        dfTotalPurchCount.UnFormattedValue = grdPurchReturnList.Count;
                        _totalPurchReturnAmt -= _totalCurrentAmt;
                        _totalPurchReturnUpchargeAmt -= _totalUpchargeAmt;
                        dfTotalPurchAmt.UnFormattedValue = (dfTotalPurchAmt.UnFormattedValue == null ? 0 : decimal.Round(_totalPurchReturnAmt, 2));
                        dfTotalPurchAmt.UnFormattedValue = CalcNetPurchReturnAmt();

                        //clearing the available items list table and re adding the items and subitmes to the grid.
                        grdAvailableList.Items.Clear();
                        grdAvailableList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in AvailableList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdAvailableList.Items[grdAvailableList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdAvailableList.Count, direction);
                        }
                        PopulateArrays(true);
                        PopulateArrays(false);
                        dfTotalAvailCount.UnFormattedValue = grdAvailableList.Count;
                        //if (!_isZeroItemValueTran)
                        //{
                        _totalAvailAmt = _totalAvailAmt + _totalCurrentAmt - _totalAdjCurrentAmt;
                        dfTotalAvailAmt.UnFormattedValue = (dfTotalAvailAmt.UnFormattedValue == null ? 0 : _totalAvailAmt);
                        _totalAdjCurrentAmt = 0;
                        //}

                    }

                    if (nEditCount > 0)
                    {
                        bEdited.Value = true;
                    }
                    else
                    {
                        bEdited.Value = false;
                    }
                    break;


                case MoveDirection.Right:

                    //Checking to make sure a row is selected.
                    if (grdAvailableList.Items.SelectedItems.Count > 0)
                    {
                        //Setting the lowest and highest indicies since I am removing the rows so it doesnt error.
                        nLowestSelectedIndicies = Convert.ToInt32(grdAvailableList.SelectedIndicies[0]);
                        nHighestSelectedIndicies = Convert.ToInt32(grdAvailableList.SelectedIndicies[grdAvailableList.SelectedIndicies.Count - 1]);
                        //Iterate forward through the selectedindicies to load the items into arraylist
                        //Iterate backward through the selectedindicies to remove the items from the collection
                        for (int nSelectedIndex = nLowestSelectedIndicies; nSelectedIndex <= nHighestSelectedIndicies; nSelectedIndex++)
                        {
                            if (grdAvailableList.Items[nSelectedIndex].Selected)
                            {
                                //Setting the contextRow.
                                grdAvailableList.ContextRow = nSelectedIndex;
                                //Setting the modified column value to Y that it was modified.

                                if (Convert.ToString(colAvailableModified.UnFormattedValue) == Convert.ToString("Y"))
                                {
                                    colAvailableModified.UnFormattedValue = Convert.ToString("N");
                                    //Need to keep track of the number of edits that are mde in order to know if the user reset all the values 
                                    //back to the original to display the correct error.
                                    if (nEditCount > 0)
                                    {
                                        nEditCount -= 1;
                                    }
                                }
                                else
                                {
                                    colAvailableModified.UnFormattedValue = Convert.ToString("Y");
                                    nEditCount += 1;
                                    //_currentAmt = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colCurrentAmt.ColumnId));
                                    //_upchargeAmt = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colUpchargeAmt.ColumnId));
                                    //_itemValue = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colItemValue.ColumnId)) +
                                    //           Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colPacketItemValue.ColumnId));  
                                    //_totalCurrentAmt += _currentAmt;
                                    //_totalUpchargeAmt += _upchargeAmt;
                                    //_totalItemValue += _itemValue;
                                }
                                _currentAmt = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colCurrentAmt.ColumnId));
                                _upchargeAmt = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colUpchargeAmt.ColumnId));
                                _itemValue = Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colItemValue.ColumnId)) +
                                           Convert.ToDecimal(grdAvailableList.GetCellValueUnformatted(nSelectedIndex, colPacketItemValue.ColumnId));
                                _currentAmt = (_currentAmt < 0 ? 0 : _currentAmt);
                                _upchargeAmt = (_upchargeAmt < 0 ? 0 : _upchargeAmt);
                                _itemValue = (_itemValue < 0 ? 0 : _itemValue);
                                _totalCurrentAmt += _currentAmt;
                                _totalUpchargeAmt += _upchargeAmt;
                                _totalItemValue += _itemValue;

                                PurchReturnList.Add(grdAvailableList.Items[nSelectedIndex]);                                
                            }
                        }

                        //Iterating backwards through the selectedIndicies array in order to remove the rows and not get the index out of range.
                        for (int nSelectedIndex = nHighestSelectedIndicies; nSelectedIndex >= nLowestSelectedIndicies; nSelectedIndex--)
                        {
                            if (grdAvailableList.Items[nSelectedIndex].Selected)
                            {
                                //Setting the contextRow.
                                grdAvailableList.ContextRow = nSelectedIndex;
                                //Setting the modified column value to Y that it was modified.

                                for (int i = 0; i <= AvailableList.Count - 1; i++)
                                {
                                    GlacialComponents.Controls.GLItem item = (GlacialComponents.Controls.GLItem)AvailableList[i];
                                    if (colAvailableItemNo.UnFormattedValue.ToString() == item.SubItems[0].Text)
                                    {
                                        AvailableList.RemoveAt(i);
                                    }
                                }
                            }
                        }

                        //clearing the selected origins table and re adding the items and subitmes to the grid.
                        grdPurchReturnList.Items.Clear();
                        grdPurchReturnList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in PurchReturnList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdPurchReturnList.Items[grdPurchReturnList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdPurchReturnList.Count, direction);
                        }

                        dfTotalPurchCount.UnFormattedValue = grdPurchReturnList.Count;
                        _totalPurchReturnAmt += _totalCurrentAmt;
                        _totalPurchReturnUpchargeAmt += _totalUpchargeAmt;
                        dfTotalPurchAmt.UnFormattedValue = (dfTotalPurchAmt.UnFormattedValue == null? 0 : decimal.Round(_totalPurchReturnAmt,2));
                        dfTotalPurchAmt.UnFormattedValue = CalcNetPurchReturnAmt();

                        //clearing the excluded origins table and re adding the items and subitmes to the grid.
                        grdAvailableList.Items.Clear();
                        grdAvailableList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in AvailableList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdAvailableList.Items[grdAvailableList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdAvailableList.Count, direction);
                        }
                        PopulateArrays(true);
                        PopulateArrays(false);
                        dfTotalAvailCount.UnFormattedValue = grdAvailableList.Count;
                        _totalAvailAmt -= _totalCurrentAmt;
                        dfTotalAvailAmt.UnFormattedValue = (dfTotalAvailAmt.UnFormattedValue == null ? 0 : _totalAvailAmt);
                    }


                    if (nEditCount > 0)
                    {
                        bEdited.Value = true;
                    }
                    else
                    {
                        bEdited.Value = false;
                    }
                    break;
                default:
                    break;
            }

            //colPurchReturnItemNo.SortOrder = SortOrder.Ascending;
            //colPurchReturnItemNo.Sort();

            //colAvailableItemNo.SortOrder = SortOrder.Ascending;
            //colAvailableItemNo.Sort();

            //colPurchReturnItemNo.SortOrder = SortOrder.Ascending;
            //colPurchReturnItemNo.Sort();

            //colAvailableItemNo.SortOrder = SortOrder.Ascending;
            //colAvailableItemNo.Sort();

            bPopulatingGrids = false;  //28568

        }
        /// <summary>
        /// Moves all the subitems of the glItmes.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        void MoveSubsItems(GlacialComponents.Controls.GLItem source, GlacialComponents.Controls.GLItem target, int rowNo, MoveDirection direction)
        {
            //decimal currentAmt = 0;
            for (int i = 0; i < source.SubItems.Count; i++)
            {
                if (i != 1) //Row #
                    target.SubItems.Add(source.SubItems[i].Text);
                else
                    target.SubItems.Add(Convert.ToString(rowNo));
                if (direction == MoveDirection.Right && i == 19)    //19 - colItemExists
                {
                    if (source.SubItems[i] != null && Convert.ToInt32(source.SubItems[i].Text) == 0)
                        target.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region Actions
        /// <summary>
        /// Moves the rows to the left grid control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbMoveLeft_Click(object sender, System.EventArgs e)
        {
            _totalCurrentAmt = 0;
            _totalAdjCurrentAmt = 0;
            _currentAmt = 0;
            _upchargeAmt = 0;
            _itemValue = 0;
            _totalUpchargeAmt = 0;
            MoveRow(MoveDirection.Left);
        }

        /// <summary>
        /// Moves the rows to the right grid control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pbMoveRight_Click(object sender, System.EventArgs e)
        {
            _totalCurrentAmt = 0;
            _totalAdjCurrentAmt = 0;
            _currentAmt = 0;
            _upchargeAmt = 0;
            _itemValue = 0;
            _totalUpchargeAmt = 0;
            MoveRow(MoveDirection.Right);
        }

        void rbAmount_PhoenixUICheckedChangedEvent(object sender, Windows.Forms.PEventArgs e)
        {
            if (rbAmount.Checked)
                FormatFields();
        }

        void rbPercent_PhoenixUICheckedChangedEvent(object sender, Windows.Forms.PEventArgs e)
        {
            if (rbPercent.Checked)
                FormatFields();
        }

        // Used for Formatting Discount Override Adjustments
        private void FormatFields()
        {

            if (rbAmount.Checked)
            {
                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Amount;

            }
            else
            {
                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Percent;
            }
            dfTotalDiscOvrd.UnFormattedValue = dfTotalDiscOvrd.UnFormattedValue;
        }
        #endregion

        #region Overriddes
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        /// <param name="paramList"></param>
        public override void InitParameters(params object[] paramList)
        {
            if (paramList.Length > 0)
            {
                if (paramList[0] != null)
                    _psCustName.Value = Convert.ToString(paramList[0]);
                if (paramList[1] != null)
                {
                    _tlTranSet = (Phoenix.BusObj.Teller.TlTransactionSet)paramList[1];
                    if (_tlTranSet != null)
                    {
                        _pnTypeId.Value = _tlTranSet.CurTran.TypeId.Value;
                        _pnPacketId.Value = _tlTranSet.CurTran.PacketId.Value;
                        _pnRimNo.Value = (_tlTranSet.CurTran.RimNo.IsNull ? _tlTranSet.CurTran.TranAcct.RimNo.Value : _tlTranSet.CurTran.RimNo.Value);
                        _pbNonCustomer.Value = (!_tlTranSet.CurTran.NonCust.IsNull && _tlTranSet.CurTran.NonCust.Value == "Y");
                        _pnNoOfItems.Value = _tlTranSet.CurTran.CheckNo.Value;
                        _pnBranchNo.Value = _tlTranSet.TellerVars.BranchNo;
                        _pnDrawerNo.Value = _tlTranSet.TellerVars.DrawerNo;
                        _pnTranCode.Value = _tlTranSet.CurTran.TranCode.Value;
                        if (_pnTranCode.IsNull)
                            _pnTranCode.Value = 938;
                        _psClass.Value = _tlTranSet.CurTran.Class.Value;

                        if (!_pnTypeId.IsNull)
                        {
                            _adGbInventoryType.TypeId.Value = _pnTypeId.Value;
                            _adGbInventoryType.ActionType = XmActionType.Select;
                            CoreService.DataService.ProcessRequest(_adGbInventoryType);
                            if (_adGbInventoryType.UseDrawerLevel.IsNull || (!_adGbInventoryType.UseDrawerLevel.IsNull &&
                                _adGbInventoryType.UseDrawerLevel.Value != "Y"))
                                _pnDrawerNo.Value = Int16.MinValue;
                        }
                    }
                }
                else
                    _tlTranSet = new BusObj.Teller.TlTransactionSet();
            }
            base.InitParameters(paramList);
        }

        /// <summary>
        /// Perform additional actions during the save process
        /// </summary>
        /// <param name="isAddNext"></param>
        /// <returns></returns>
        public override bool OnActionSave(bool isAddNext)
        {
            // todo: perform additional actions before calling base method
            // Only Add MainBusObj if form is dirty
            if (this.IsFormDirty())
            {
                grdAvailableList.TabStop = false;
                if (SaveAvailableItems())
                {
                    grdPurchReturnList.TabStop = false;
                    if (SavePurchReturnItems())
                    {
                        if (_tlTranSet != null)
                        {
                            if (_tlTranSet.CurTran != null)
                            {
                                if (dfTotalPurchAmt.UnFormattedValue != null)
                                    _tlTranSet.CurTran.InvItemAmt.Value = Convert.ToDecimal(dfTotalPurchAmt.UnFormattedValue);
                                else
                                    _tlTranSet.CurTran.InvItemAmt.Value = 0;
                                if (dfTotalDiscOvrd.UnFormattedValue != null)
                                {
                                    _tlTranSet.CurTran.InvDiscountOvrd.Value = Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue);
                                    if (rbPercent.Checked)
                                        _tlTranSet.CurTran.AmountPercent.Value = 1;
                                    else
                                        _tlTranSet.CurTran.AmountPercent.Value = 0;
                                }
                                else
                                    _tlTranSet.CurTran.InvDiscountOvrd.Value = 0;
                                if (dfStateTax.UnFormattedValue != null)
                                    _tlTranSet.CurTran.StateTaxAmt.Value = Convert.ToDecimal(dfStateTax.UnFormattedValue);
                                else
                                    _tlTranSet.CurTran.StateTaxAmt.SetValueToNull();
                                if (dfLocalTax.UnFormattedValue != null)
                                    _tlTranSet.CurTran.LocalTaxAmt.Value = Convert.ToDecimal(dfLocalTax.UnFormattedValue);
                                else
                                    _tlTranSet.CurTran.StateTaxAmt.Value = 0;
                                if (dfTotalPurchCount.UnFormattedValue != null)
                                    _tlTranSet.CurTran.CheckNo.Value = Convert.ToInt32(dfTotalPurchCount.UnFormattedValue);
                                else
                                    _tlTranSet.CurTran.CheckNo.Value = 0;
                            }
                            bEdited.Value = false;
                            _isUpchargeAmtEdited = false;
                            _isItemValueEdited = false;
                        }
                    }
                    else
                        return false;
                }
                else
                    return false;
            }

            //return base.OnActionSave(isAddNext);
            return true;
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
        /// override IsFormDirty validation
        /// </summary>
        /// <returns></returns>
        public override bool IsFormDirty()
        {
            return (bEdited.Value || _isItemValueEdited || _isUpchargeAmtEdited);
        }

        /// <summary>
        /// override validation to determine if form needs to be saved
        /// </summary>
        /// <param name="checkType"></param>
        /// <param name="showMessage"></param>
        /// <returns></returns>
        //public override bool PerformCheck(CheckType checkType, bool showMessage)
        //{
        //    if (checkType == CheckType.EditTest && IsFormDirty())
        //        return true;
        //    else
        //        return base.PerformCheck(checkType, showMessage);
        //}

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
                    if ((!_pnTranCode.IsNull && _pnTranCode.Value == 939) || 
                        (!_tlTranSet.CurTran.Class.IsNull && _tlTranSet.CurTran.Class.Value == CoreService.Translation.GetListItemX(ListId.ML_InventoryClass, "Serial")) ||
                        (!_isItemValueAmtOvrdEnabled && 
                        !_isUpchargeAmtOvrdEnabled))  //#21085
                    {
                        dfTotalDiscOvrd.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                        rbAmount.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                        rbPercent.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                    }
                    ResetFormForSupViewOnlyMode();
                    break;
                case EnableDisableVisible.InitBegin:
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
                case CallOtherForm.EditClick:
                    // TODO: invoke new window using the following format
                    // tempWin = CreateWindow("{ASSEMBLYNAME}", "{NAMESPACE}", "{FORMNAME}");
                    // tempWin.InitParameters(PhoenixVariable, PhoenixVariable....);
                    break;
            }

            if (tempWin != null)
            {
                tempWin.Workspace = this.Workspace;
                // TODO: if you want a grid to refresh on this form when this window is closed,
                // set the following property: tempWin.ParentGrid = this.grid;	
                tempWin.Show();
            }
        }


        /// <summary>
        /// Populates the available and purchase/return lists or inventory items.  These list will be used
        /// when moving items between grids.
        /// </summary>
        /// <param name="IsPurchReturn"></param>
        private void PopulateArrays(Boolean IsPurchReturn)
        {
            if (IsPurchReturn)
            {
                PurchReturnList = new ArrayList();
                foreach (GlacialComponents.Controls.GLItem item in grdPurchReturnList.Items)
                {
                    PurchReturnList.Add(item);

                }
            }

            else
            {
                AvailableList = new ArrayList();
                foreach (GlacialComponents.Controls.GLItem item in grdAvailableList.Items)
                {
                    AvailableList.Add(item);
                }
            }
        }

        private bool SaveAvailableItems()
        {
            GbInventoryItem itemObj;
            int nRow = 0;

            if (_tlTranSet.CurTran.InventoryItemsAvailable != null)
                _tlTranSet.CurTran.InventoryItemsAvailable.Clear();

            if (grdAvailableList.Count > 0)
            {
                while (nRow < grdAvailableList.Count)
                {
                    itemObj = new GbInventoryItem();

                    itemObj.RowNum.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colRowNo.ColumnId);
                    itemObj.Ptid.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colAvailableItemNo.ColumnId);
                    itemObj.SerialNo.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colSerialNo.ColumnId);
                    itemObj.LastActivityDt.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colLastActivityDt.ColumnId);
                    itemObj.ItemValue.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colItemValue.ColumnId);
                    itemObj.PacketItemValue.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colPacketItemValue.ColumnId);
                    itemObj.UpchargeAmt.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colUpchargeAmt.ColumnId);
                    itemObj.InvItemAmt.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colCurrentAmt.ColumnId);
                    itemObj.TypeId.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colTypeId.ColumnId);
                    itemObj.PacketId.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colPacketId.ColumnId);
                    itemObj.Class.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colClass.ColumnId);
                    itemObj.Location.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colLocation.ColumnId);
                    itemObj.BranchNo.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colBranchNo.ColumnId);
                    itemObj.DrawerNo.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colDrawerNo.ColumnId);
                    itemObj.Status.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colStatus.ColumnId);
                    itemObj.StatusSort.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colStatusSort.ColumnId);
                    itemObj.UseDrawerLevel.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colUseDrawerLevel.ColumnId);
                    itemObj.LocationSort.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colLocationSort.ColumnId);
                    itemObj.ItemExists.ValueObject = grdAvailableList.GetCellValueUnformatted(nRow, colItemExists.ColumnId);
                    _tlTranSet.CurTran.InventoryItemsAvailable.Add(itemObj);
                    nRow = nRow + 1;
                }
                _tlTranSet.CurTran.NoInvAvailable.Value = nRow;
            }
            return true;
        }

        private bool SavePurchReturnItems()
        {
            GbInventoryItem itemObj;
            decimal tempTotalItemValue = 0;
            decimal tempTotalItemUpcharge = 0;
            decimal tempItemValue = 0;
            decimal tempItemUpcharge = 0;
            int nRow = 0;

            if (_tlTranSet.CurTran.InventoryItems != null)
                _tlTranSet.CurTran.InventoryItems.Clear();

            if (grdPurchReturnList.Count > 0)
            {
                while (nRow < grdPurchReturnList.Count)
                {
                    itemObj = new GbInventoryItem();

                    itemObj.RowNum.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colRowNo1.ColumnId);
                    itemObj.Ptid.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colPurchReturnItemNo.ColumnId);
                    itemObj.SerialNo.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colSerialNo1.ColumnId);
                    itemObj.LastActivityDt.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colLastActivityDt1.ColumnId);
                    itemObj.ItemValue.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colItemValue1.ColumnId);
                    itemObj.PacketItemValue.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colPacketItemValue1.ColumnId);
                    itemObj.UpchargeAmt.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colUpchargeAmt1.ColumnId);
                    itemObj.InvItemAmt.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colCurrentAmt1.ColumnId);
                    itemObj.TypeId.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colTypeId1.ColumnId);
                    itemObj.PacketId.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colPacketId1.ColumnId);
                    itemObj.Class.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colClass1.ColumnId);
                    itemObj.Location.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colLocation1.ColumnId);
                    itemObj.BranchNo.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colBranchNo1.ColumnId);
                    itemObj.DrawerNo.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colDrawerNo1.ColumnId);
                    itemObj.Status.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colStatus1.ColumnId);
                    itemObj.StatusSort.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colStatusSort1.ColumnId);
                    itemObj.UseDrawerLevel.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colUseDrawerLevel1.ColumnId);
                    itemObj.LocationSort.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colLocationSort1.ColumnId);
                    itemObj.ItemExists.ValueObject = grdPurchReturnList.GetCellValueUnformatted(nRow, colItemExists1.ColumnId);
                    itemObj.UpchargeAmtEdited.Value = (short)(_isUpchargeAmtEdited ? 1 : 0);
                    itemObj.ItemAmtEdited.Value = (short)(_isItemValueEdited ? 1 : 0);
                    _tlTranSet.CurTran.InventoryItems.Add(itemObj);
                    tempItemValue = (itemObj.ItemValue.IsNull ? 0 : itemObj.ItemValue.Value) + (itemObj.PacketItemValue.IsNull ? 0 : itemObj.PacketItemValue.Value);
                    //#
                    //tempItemUpcharge = (itemObj.UpchargeAmt.IsNull ? 0 : itemObj.UpchargeAmt.Value);
                    if (!_adGbInventoryType.UpchargeType.IsNull && _adGbInventoryType.UpchargeType.Value == "P")  //#21476
                        tempItemUpcharge = (itemObj.UpchargeAmt.IsNull ? 0 : decimal.Round(((tempItemValue * itemObj.UpchargeAmt.Value) / 100), 2));
                    else
                        tempItemUpcharge = (itemObj.UpchargeAmt.IsNull ? 0 : itemObj.UpchargeAmt.Value);
                    tempTotalItemValue += tempItemValue;
                    tempTotalItemUpcharge += tempItemUpcharge;
                    nRow = nRow + 1;
                }
                //Begin #21085
                //_tlTranSet.CurTran.InvItemValue.Value = tempTotalItemValue;
                //_tlTranSet.CurTran.InvUpchargeAmt.Value = tempTotalItemUpcharge;
                if (dfTotalPurchAmt.UnFormattedValue == null || (dfTotalPurchAmt.UnFormattedValue != null && Convert.ToDecimal(dfTotalPurchAmt.UnFormattedValue) <= 0))
                {
                    tempTotalItemValue = 0;
                    tempTotalItemUpcharge = 0;
                }
                else
                {
                    if (_totalDiscountOvrdAmt > 0)
                    {
                        if (!_isUpchargeAmtOvrdEnabled || !_isItemValueAmtOvrdEnabled)
                        {
                            if (!_isUpchargeAmtOvrdEnabled)
                            {
                                if (_totalDiscountOvrdAmt > tempTotalItemValue)
                                {
                                    //#14198 - The discount override amount cannot be greater than the total item value amount.
                                    PMessageBox.Show(14198, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                                    dfTotalDiscOvrd.Focus();
                                    return false;
                                }
                            }
                            if (!_isItemValueAmtOvrdEnabled)
                            {
                                if (_totalDiscountOvrdAmt > tempTotalItemUpcharge)
                                {
                                    //#14199 - The discount override amount cannot be greater than the total upcharge amount.
                                    PMessageBox.Show(14199, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                                    dfTotalDiscOvrd.Focus();
                                    return false;
                                }
                            }
                        }
                        if (_isUpchargeAmtOvrdEnabled)
                        {
                            if (_totalDiscountOvrdAmt >= tempTotalItemUpcharge)
                            {
                                _totalDiscountOvrdAmt = _totalDiscountOvrdAmt - tempTotalItemUpcharge;
                                tempTotalItemUpcharge = 0;
                            }
                            else
                            {
                                tempTotalItemUpcharge = tempTotalItemUpcharge - _totalDiscountOvrdAmt;
                                _totalDiscountOvrdAmt = 0;
                            }
                        }
                        if (_isItemValueAmtOvrdEnabled)
                        {
                            if (_totalDiscountOvrdAmt >= tempTotalItemValue)
                                tempTotalItemValue = 0;
                            else
                                tempTotalItemValue = tempTotalItemValue - _totalDiscountOvrdAmt;
                        }
                    }
                }
                _tlTranSet.CurTran.InvItemValue.Value = tempTotalItemValue;
                _tlTranSet.CurTran.InvUpchargeAmt.Value = tempTotalItemUpcharge;
                //End #21085
            }
            return true;
        }

        private decimal CalcNetPurchReturnAmt()
        {
            decimal stateTax = 0;
            decimal localTax = 0;
            decimal purchReturnAmtBeforeTax = 0;  //#21085
            decimal purchReturnAmtAfterTax = 0;
            purchReturnAmtBeforeTax = _totalDiscountOvrdAmt = GetDiscPurchReturnAmt();  //#21085
            purchReturnAmtBeforeTax = _totalPurchReturnAmt - _totalDiscountOvrdAmt;  //#21085

            if (dfTotalPurchAmt.UnFormattedValue == null)
                return purchReturnAmtBeforeTax;
            if (_tlTranSet == null)
                return purchReturnAmtBeforeTax;
            if (_adGbInventoryType == null)
                return purchReturnAmtBeforeTax;

            if (!_adGbInventoryType.StateTax.IsNull || !_adGbInventoryType.LocalTax.IsNull)
            {
                _tlTranSet.GetStateLocalTaxAmt(_tlTranSet.TellerVars.BranchNo, purchReturnAmtBeforeTax, out stateTax, out localTax);

                if (_pnTranCode.Value != 938 || _adGbInventoryType.StateTax.IsNull || (!_adGbInventoryType.StateTax.IsNull && _adGbInventoryType.StateTax.Value != "Y"))
                    stateTax = 0;
                if (_pnTranCode.Value != 938 || _adGbInventoryType.LocalTax.IsNull || (!_adGbInventoryType.LocalTax.IsNull && _adGbInventoryType.LocalTax.Value != "Y"))
                    localTax = 0;

                dfStateTax.UnFormattedValue = stateTax;
                dfLocalTax.UnFormattedValue = localTax;

                purchReturnAmtAfterTax = purchReturnAmtBeforeTax + stateTax + localTax;
                //dfTotalPurchAmt.UnFormattedValue = decimal.Round(purchReturnAmtAfterTax, 2);
            }
            return (purchReturnAmtAfterTax == 0 ? purchReturnAmtBeforeTax : decimal.Round(purchReturnAmtAfterTax, 2));
        }

        private string MakeAmountPercentFieldsFormattedValue(object unformattedValue)
        {
            if (unformattedValue == null || Convert.ToString(unformattedValue) == string.Empty)
                return null;

            if (dfTotalDiscOvrd.FormatType == UIFieldFormat.Amount)
            {
                return CurrencyHelper.GetFormattedValue(Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue));
            }
            else if (dfTotalDiscOvrd.FormatType == UIFieldFormat.Percent)
            {
                NumberFormatInfo nFmt = (NumberFormatInfo)CultureInfo.CurrentUICulture.NumberFormat.Clone();

                decimal dValue = System.Convert.ToDecimal(unformattedValue);

                nFmt.PercentDecimalDigits = 3;

                dValue = dValue / 100;  // The System Will multiply this value with 100

                return dValue.ToString("P", nFmt);
            }
            else
            {
                return null;
            }
        }

        private object MakeAmountPercentFieldsUnFormatedFormattedValue(string formattedValue)
        {
            if (formattedValue == null || Convert.ToString(formattedValue) == string.Empty)
                return null;

            if (UIFieldFormat.Amount == dfTotalDiscOvrd.FormatType)
            {
                int length = formattedValue.Trim().Length;
                if (length == 0)
                    return null;

                try
                {
                    return CurrencyHelper.GetUnformattedValue(formattedValue);
                }
                catch (FormatException)
                {
                    int mlErrorId = 300057;
                    throw new PhoenixException(12346, mlErrorId, MLErrorLevel.Error);
                }
            }
            else
            {
                if (UIFieldFormat.Percent == dfTotalDiscOvrd.FormatType || UIFieldFormat.Rate == dfTotalDiscOvrd.FormatType)
                    return System.Convert.ToDecimal(formattedValue.Replace("%", ""));
            }
            return null;
        }

        void dfTotalDiscOvrd_PhoenixUIEditedEvent(object sender, Windows.Forms.ValueEditedEventArgs e)
        {
            FormatFields();
        }

        void dfTotalDiscOvrd_PhoenixUIValidateEvent(object sender, Windows.Forms.PCancelEventArgs e)
        {
            if (rbAmount.Checked && dfTotalDiscOvrd.UnFormattedValue != null && dfTotalPurchAmt.UnFormattedValue != null)
            {
                if (Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue) > _totalPurchReturnAmt)  //#21085
                {
                    //#13969 - The amount of the discount override cannot exceed total purchase/return amount.
                    PMessageBox.Show(13969, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                    dfTotalDiscOvrd.Focus();
                    e.Cancel = true;
                }
            }
            dfTotalPurchAmt.UnFormattedValue = CalcNetPurchReturnAmt();
        }

        private decimal GetDiscPurchReturnAmt()
        {
            decimal dicountOvrdAmt = 0;
            _isItemValueEdited = false;
            _isUpchargeAmtEdited = false;

            if (dfTotalDiscOvrd.UnFormattedValue != null && Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue) > 0)
            {
                if (rbAmount.Checked)
                    dicountOvrdAmt = Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue);
                else
                {
                    if (Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue) >= 100)
                        dicountOvrdAmt = 0;
                    else
                        dicountOvrdAmt = _totalPurchReturnAmt * (Convert.ToDecimal(dfTotalDiscOvrd.UnFormattedValue) / 100);
                    dicountOvrdAmt = decimal.Round(dicountOvrdAmt, 2);
                }
                if (dicountOvrdAmt > 0)
                {
                    if (_isUpchargeAmtOvrdEnabled)  //#21085
                        _isUpchargeAmtEdited = true;
                    if (dicountOvrdAmt > (_isUpchargeAmtOvrdEnabled? _totalPurchReturnUpchargeAmt : 0) && _isItemValueAmtOvrdEnabled)   //#21085
                        _isItemValueEdited = true;
                }
            }
            return dicountOvrdAmt;
        }

        private void LoadInventoryItems(ArrayList invItems, bool isPurchReturnList)
        {
            int rowId = 0;

            bPopulatingGrids = true;  //23981

            if (isPurchReturnList)
            {
                _totalCurrentAmt = 0;
                if (invItems != null && invItems.Count > 0)
                {
                    //Load stored items from arraylist to the table
                    foreach (GbInventoryItem item in invItems)
                    {
                        grdPurchReturnList.AddNewRow();

                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colRowNo1.ColumnId, item.RowNum.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colPurchReturnItemNo.ColumnId, item.Ptid.Value);
                        if (!item.SerialNo.IsNull && item.SerialNo.Value > 0)   //#21085
                            grdPurchReturnList.SetCellValueUnFormatted(rowId, colSerialNo1.ColumnId, item.SerialNo.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colLastActivityDt1.ColumnId, item.LastActivityDt.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colItemValue1.ColumnId, item.ItemValue.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colPacketItemValue1.ColumnId, item.PacketItemValue.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colUpchargeAmt1.ColumnId, item.UpchargeAmt.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colCurrentAmt1.ColumnId, item.InvItemAmt.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colPrevCurrentAmt1.ColumnId, item.InvItemAmt.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colTypeId1.ColumnId, item.TypeId.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colPacketId1.ColumnId, item.PacketId.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colClass1.ColumnId, item.Class.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colLocation1.ColumnId, item.Location.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colBranchNo1.ColumnId, item.BranchNo.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colDrawerNo1.ColumnId, item.DrawerNo.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colStatus1.ColumnId, item.Status.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colStatusSort1.ColumnId, item.StatusSort.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colUseDrawerLevel1.ColumnId, item.UseDrawerLevel.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colLocationSort1.ColumnId, item.LocationSort.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colItemExists1.ColumnId, item.ItemExists.Value);
                        grdPurchReturnList.SetCellValueUnFormatted(rowId, colPacketDesc1.ColumnId, item.PacketDesc.Value); //23981

                        if (!item.InvItemAmt.IsNull)
                            _totalCurrentAmt += Convert.ToDecimal(colCurrentAmt1.UnFormattedValue);
                        if (!item.UpchargeAmt.IsNull)
                            _totalUpchargeAmt += Convert.ToDecimal(colUpchargeAmt1.UnFormattedValue);
                        //if (rowId == 0)
                        //{
                        //    if (_isZeroItemValueTran)
                        //        colCurrentAmt1.ReadOnly = false;
                        //    else
                        //        colCurrentAmt1.ReadOnly = true;
                        //}
                        rowId++;
                    }
                    _totalPurchReturnAmt = _totalCurrentAmt;
                    _totalPurchReturnUpchargeAmt = _totalUpchargeAmt;
                    _purchReturnRowNo = grdPurchReturnList.Count;

                    dfTotalPurchCount.UnFormattedValue = _purchReturnRowNo;
                    dfTotalPurchAmt.UnFormattedValue = decimal.Round(_totalPurchReturnAmt, 2);
                    //Populate PurchReturn arraylist
                    if (grdPurchReturnList.Count > 0)
                    {
                        PopulateArrays(true);
                        //clearing the purch/return items list table and re adding the items and subitmes to the grid.
                        grdPurchReturnList.Items.Clear();
                        grdPurchReturnList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in PurchReturnList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdPurchReturnList.Items[grdPurchReturnList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdPurchReturnList.Count, MoveDirection.Right);
                        }
                        //
                        if (!_tlTranSet.CurTran.InvItemAmt.IsNull)
                            dfTotalPurchAmt.UnFormattedValue = _tlTranSet.CurTran.InvItemAmt.Value;
                        if (!_tlTranSet.CurTran.StateTaxAmt.IsNull)
                            dfStateTax.UnFormattedValue = _tlTranSet.CurTran.StateTaxAmt.Value;
                        if (!_tlTranSet.CurTran.LocalTaxAmt.IsNull)
                            dfLocalTax.UnFormattedValue = _tlTranSet.CurTran.LocalTaxAmt.Value;
                        if (!_tlTranSet.CurTran.AmountPercent.IsNull)
                        {
                            if (_tlTranSet.CurTran.AmountPercent.Value == 1)
                                this.rbPercent.Checked = true;
                            else
                                this.rbAmount.Checked = true;

                            if (this.rbPercent.Checked)
                                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Percent;
                            else
                                dfTotalDiscOvrd.PhoenixUIControl.FormatType = UIFieldFormat.Amount;
                            if (!_tlTranSet.CurTran.InvDiscountOvrd.IsNull)
                                dfTotalDiscOvrd.UnFormattedValue = _tlTranSet.CurTran.InvDiscountOvrd.Value;
                        }
                    }
                }
            }
            else
            {
                if (invItems != null && invItems.Count > 0)
                {
                    //Load stored items from arraylist to the table
                    foreach (GbInventoryItem item in invItems)
                    {
                        grdAvailableList.AddNewRow();

                        grdAvailableList.SetCellValueUnFormatted(rowId, colRowNo.ColumnId, item.RowNum.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colAvailableItemNo.ColumnId, item.Ptid.Value);
                        if (!item.SerialNo.IsNull && item.SerialNo.Value > 0)   //#21085
                            grdAvailableList.SetCellValueUnFormatted(rowId, colSerialNo.ColumnId, item.SerialNo.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colLastActivityDt.ColumnId, item.LastActivityDt.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colItemValue.ColumnId, item.ItemValue.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colPacketItemValue.ColumnId, item.PacketItemValue.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colUpchargeAmt.ColumnId, item.UpchargeAmt.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colCurrentAmt.ColumnId, item.InvItemAmt.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colTypeId.ColumnId, item.TypeId.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colPacketId.ColumnId, item.PacketId.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colClass.ColumnId, item.Class.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colLocation.ColumnId, item.Location.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colBranchNo.ColumnId, item.BranchNo.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colDrawerNo.ColumnId, item.DrawerNo.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colStatus.ColumnId, item.Status.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colStatusSort.ColumnId, item.StatusSort.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colUseDrawerLevel.ColumnId, item.UseDrawerLevel.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colLocationSort.ColumnId, item.LocationSort.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colItemExists.ColumnId, item.ItemExists.Value);
                        grdAvailableList.SetCellValueUnFormatted(rowId, colPacketDesc.ColumnId, item.PacketDesc.Value); //23981

                        if (!item.InvItemAmt.IsNull)
                            _totalCurrentAmt += Convert.ToDecimal(colCurrentAmt.UnFormattedValue);
                        rowId++;
                    }
                    _totalAvailAmt = _totalCurrentAmt;
                    _availRowNo = grdAvailableList.Count;

                    dfTotalAvailCount.UnFormattedValue = _availRowNo;
                    dfTotalAvailAmt.UnFormattedValue = decimal.Round(_totalAvailAmt, 2);
                    //Populate Available arraylist
                    if (grdAvailableList.Count > 0)
                    {
                        PopulateArrays(false);
                        //clearing the purch/return items list table and re adding the items and subitmes to the grid.
                        grdAvailableList.Items.Clear();
                        grdAvailableList.ResetTable();

                        foreach (GlacialComponents.Controls.GLItem item in AvailableList)
                        {
                            GlacialComponents.Controls.GLItem listItem = grdAvailableList.Items[grdAvailableList.AddNewRow()];
                            MoveSubsItems(item, listItem, grdAvailableList.Count, MoveDirection.Left);
                        }
                        //
                    }
                }
            }

            bPopulatingGrids = false;  //23981

        }
        /// <summary>
        /// Set the total field labels
        /// </summary>
        private void SetPurcReturnLabels()   //#140772
        {
            if (_pnTranCode.Value == 939)
            {
                lblAvailable.Text = CoreService.Translation.GetUserMessageX(13975); // Sold:
                lblPurchReturn.Text = CoreService.Translation.GetUserMessageX(13977); // Return:
                lblTotalAvailCount.Text = CoreService.Translation.GetUserMessageX(13980); // Total Sold Count:
                lblTotalAvailAmt.Text = CoreService.Translation.GetUserMessageX(13981); // Total Sold Amt:
                lblTotalPurchCount.Text = CoreService.Translation.GetUserMessageX(13984); // Total Returned Count:
                lblTotalPurchAmt.Text = CoreService.Translation.GetUserMessageX(13985); // Total Returned Amt:
            }
            else
            {
                lblAvailable.Text = CoreService.Translation.GetUserMessageX(13974); // Available:
                lblPurchReturn.Text = CoreService.Translation.GetUserMessageX(13976); // Purchase:
                lblTotalAvailCount.Text = CoreService.Translation.GetUserMessageX(13978); // Total Available Count:
                lblTotalAvailAmt.Text = CoreService.Translation.GetUserMessageX(13979); // Total Available Amt:
                lblTotalPurchCount.Text = CoreService.Translation.GetUserMessageX(13982); // Total Purchased Count:
                lblTotalPurchAmt.Text = CoreService.Translation.GetUserMessageX(13983); // Total Purchased Amt:
            }
        }


        #region #140772 - #79314
        private void ResetFormForSupViewOnlyMode()
        {
            if (AppInfo.Instance.IsAppOnline && _tlTranSet.TellerVars.IsRemoteOverrideEnabled &&
                Phoenix.Windows.Client.Helper.IsWorkspaceReadOnly(this.Workspace))
            {
                MakeReadOnly(true);
                foreach (PAction action in ActionManager.Actions)
                {
                    if (!(action == ActionClose))
                        action.Enabled = false;
                }
            }
        }
        #endregion

        #endregion


        //Begin #23981
        void grdAvailableList_SelectedIndexChanged(object source, Windows.Forms.GridClickEventArgs e)
        {
            if (grdAvailableList.Count > 0 && bPopulatingGrids == false)
            {
                if (_pnPacketId.Value == -1)
                {
                    dfPacketDesc.Value = colPacketDesc.UnFormattedValue;
                }
            }
        }

        void grdPurchReturnList_SelectedIndexChanged(object source, Windows.Forms.GridClickEventArgs e)
        {
            if (grdPurchReturnList.Count > 0 && bPopulatingGrids == false)
            {
                if (_pnPacketId.Value == -1)
                {
                    dfPacketDesc.Value = colPacketDesc1.UnFormattedValue;
                }
            }
        }
        //End #23981
    }
}
