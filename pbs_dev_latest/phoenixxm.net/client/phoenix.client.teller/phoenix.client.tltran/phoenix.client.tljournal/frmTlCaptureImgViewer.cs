#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2014 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlCaptureImgViewer.cs
// NameSpace: phoenix.client.tljournal
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//1/22/2014 3:01:40 PM			1		spatterson  Created
//02/24/2014                    2       spatterson  Fix for trying to dispose a null image.
//04/04/2014                    3       spatterson  Fixes for using arrows in the grid.
//09/28/2015    				4      	rpoddar     #195669, #35513 - Automate EOB Changes
//12/05/2016                    5       mselvaga    #56226  - AVTC transaction froze but posted part of transaction to journal.
//-------------------------------------------------------------------------------
#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Core;
using Phoenix.Client.Teller;
using System.Xml;

namespace Phoenix.Client.Journal
{
    public partial class frmTlCaptureImgViewer : PfwStandard 
    {
        #region Private Variables

        private PInt _pnJournalPtid = new PInt("JournalPtid");
        private PString _psAccount = new PString("Account");

        private TlCaptureScanItemsCollection _scanItems;
        private TlJournalAddlInfo _tlJournalAddlInfo;
        private TellerCapture _tellerCapture = null;
        private ImageView _currentImageView;
        private TlCaptureScanItem _selectedScanItem;
        private int _validatedItemCount;
        private Zoom _currentZoom;
        private PfwStandard _curForm = null;
        private bool _imageValidationPending = false;   //#56226

        /// <summary>
        /// This enum contains the various conditions which will enable/disable push buttons
        /// </summary>
        private enum EnableDisableVisible 
        {
            InitBegin,
            InitComplete,
            GridClick,
            FrontClick,
            RearClick,
            ZoomClick
        }

        private enum CallOtherForm 
        {
            EditClick
        }
        
        private enum ImageView
        {
            Front,
            Rear
        }

        private enum Zoom
        {
            In,
            Out
        }
        #endregion

        #region Constructors
        public frmTlCaptureImgViewer()
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
            this.ScreenId = Phoenix.Shared.Constants.ScreenId.TlCaptureImgViewer;
            this.UseStateFromBusinessObject = true;

            _scanItems = new TlCaptureScanItemsCollection();
            _tellerCapture = Phoenix.Client.Teller.TlHelper.TellerCaptureWrapper;

            ActionSave.Visible = false;

            return default(ReturnType);
        }

        private void frmTlCaptureImgViewer_PInitCompleteEvent() 
        {
            SetTitleInfo(_psAccount.Value);

            _tlJournalAddlInfo = new TlJournalAddlInfo();
            _tlJournalAddlInfo.JournalPtid.Value = _pnJournalPtid.Value;

            CoreService.DataService.ProcessRequest(XmActionType.Select, _tlJournalAddlInfo);

            if (!_tlJournalAddlInfo.TlCaptureTransetInfo.IsNull)
            {
                _scanItems.LoadFromXML(_tlJournalAddlInfo.TlCaptureTransetInfo.Value);

                PopulateGrid(_scanItems);

                // Pop up a dialog to retrieve images, this will be closed via ProcessMessage-->OnValidateWithImages
                dlgInformation.Instance.ShowInfo("Retrieving Images...");
                
                _validatedItemCount = 0;

                //#56226
                //GetImages();
                try
                {
                    if (_scanItems != null && _scanItems.Count > 0)
                    {
                        _imageValidationPending = true;
                        lblNoImage.Text = "Retrieving Images...Please wait";
                        GetImages();
                        if (_tellerCapture != null && !string.IsNullOrEmpty(_tellerCapture.PhxErrorMessage))
                        {
                            lblNoImage.Text = "No Image";
                            _imageValidationPending = false;
                        }
                    }
                    else
                    {
                        lblNoImage.Text = "No Image";
                        _imageValidationPending = false;
                    }
                }
                catch
                {
                    _imageValidationPending = false;
                }

            }

            grdItems.Focus();

            DefaultAction = ActionClose;

            if ( ActionClose != null )
                grdItems.FocusControlOnTab = ActionClose.ImageButton;

            if ( Workspace != null )
                (Workspace as Form).FormClosing += new FormClosingEventHandler(frmTlCaptureImgViewer_FormClosing);

            grdItems.SelectedIndexChanged += new GridClickedEventHandler(grdItems_SelectedIndexChanged);
        }

        private void grdItems_Click(object sender, EventArgs e) 
        {
            if (grdItems.SelectedItems.Count > 0)
            {
                _currentImageView = ImageView.Front;
                _selectedScanItem = _scanItems.GetScanItemByISN(colISN.Text);

                DisplayImage();

                EnableDisableVisibleLogic(EnableDisableVisible.GridClick);
            }
        }

        private void grdItems_SelectedIndexChanged(object source, GridClickEventArgs e)
        {
            if (grdItems.SelectedItems.Count > 0)
            {
                _currentImageView = ImageView.Front;
                _selectedScanItem = _scanItems.GetScanItemByISN(colISN.Text);

                DisplayImage();

                EnableDisableVisibleLogic(EnableDisableVisible.GridClick);
            }
        }

        private void picImage_Click(object sender, EventArgs e) 
        {
            // Panel must have focus for the scrollbars to scroll from mouse wheel ticks
            //pnlPicture.Focus();
        }

        private void pbFront_Click(object sender, PActionEventArgs e) 
        {
            _currentImageView = ImageView.Front;

            DisplayImage();
            EnableDisableVisibleLogic(EnableDisableVisible.FrontClick);
        }

        private void pbRear_Click(object sender, PActionEventArgs e) 
        {
            _currentImageView = ImageView.Rear;

            DisplayImage();
            EnableDisableVisibleLogic(EnableDisableVisible.RearClick);
        }

        private void pbZoomIn_Click(object sender, PActionEventArgs e) 
        {
            SetZoom(Zoom.In);
        }

        private void pbZoomOut_Click(object sender, PActionEventArgs e) 
        {
            SetZoom(Zoom.Out);
        }

        private void spltContainer_SplitterMoved(object sender, SplitterEventArgs e) 
        {
            picImage.SizeMode = PictureBoxSizeMode.Zoom;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        public override void OnCreateParameters() 
        {
            Parameters.Add(_pnJournalPtid);
            Parameters.Add(_psAccount);

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

            return base.OnActionSave(isAddNext);
        }

        void frmTlCaptureImgViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnActionClose();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            OnActionClose();
            base.OnClosing(e);
        }

        /// <summary>
        /// Perform actions when closing a form
        /// </summary>
        /// <returns></returns>
        public override bool OnActionClose() 
        {
            //#56226
            bool addDelay = false;
            CoreService.LogPublisher.LogDebug("OkToProcessAction OnActionClose " + ActionManager.OkToProcessAction.ToString());

            try
            {
                //if (_imageValidationPending &&
                //    (picImage == null || (picImage != null && picImage.Image == null)))
                if (lblNoImage.Visible && _imageValidationPending)
                {
                    dlgInformation.Instance.ShowInfo("Cannot close window while loading images, please try again later...");
                    addDelay = true;
                    return false;
                }
            }
            finally
            {
                if (addDelay)
                {
                    System.Threading.Thread.Sleep(300);
                    dlgInformation.Instance.HideInfo();
                }
                ActionManager.OkToProcessAction = true;
                CoreService.LogPublisher.LogDebug("OkToProcessAction OnActionClose Finally Block " + ActionManager.OkToProcessAction.ToString());
            }

            if (_tellerCapture.IsTransactionInProgress &&
                AVTCGlobals.TransactionType == TellerCaptureTransactionTypes.ViewImages)
            {
                //if (picImage.Image != null)
                //    picImage.Image.Dispose();

                _tellerCapture.CancelImageViewerTransaction();
                _tellerCapture.CurForm = _curForm;

                DeleteImages();

                Phoenix.Windows.Client.Helper.SendMessageToMDI((int)GlobalActions.ChildAction, this, "TranCompleted");     //#195669, #35513
            }

            return base.OnActionClose();
        }

        /// <summary>
        /// Called by child when parent needs to perform an action
        /// </summary>
        /// <param name="paramList"></param>
        public override void CallParent(params object[] paramList) 
        {
            base.CallParent(paramList); 
        }

        public override bool ProcessMessage(
            int messageId, 
            object sender, 
            CancelEventArgs eArg, 
            params object [] paramList) 
        {
            string messageType = string.Empty;

            if (messageId == (int)GlobalActions.TellerCapture)
            {
                messageType = Convert.ToString(paramList[0]);

                System.Diagnostics.Debug.WriteLine(messageType);

                if (messageType == "OnValidateItemWithImages")
                {
                    object[] data = (object[])paramList[1];

                    if (data.Length == 3)
                    {
                        string xml = Convert.ToString(data[0]);
                        string frontImagePath = Convert.ToString(data[1]).Trim('\0');
                        string rearImagePath = Convert.ToString(data[2]).Trim('\0');

                        System.Diagnostics.Debug.WriteLine("Front Image: " + frontImagePath);
                        System.Diagnostics.Debug.WriteLine("Rear Image: " + rearImagePath);

                        SetImagePaths(xml, frontImagePath, rearImagePath);

                        _tellerCapture.SendItemResponse("", @"<CustomerData></CustomerData>");

                        if (++_validatedItemCount == _scanItems.Count)
                        {
                            dlgInformation.Instance.HideInfo();

                            grdItems_Click(null, EventArgs.Empty);
                        }
                    }
                }

                return true;
            }

            return true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// This contains the various conditions which will enable/disable push buttons
        /// </summary>
        /// <param name="caseName"></param>
        private void EnableDisableVisibleLogic(EnableDisableVisible caseName) 
        {
            // TODO: Reset zoom controls

            switch (caseName)
            {
                case EnableDisableVisible.InitComplete:
                {
                    break;
                }
                case EnableDisableVisible.InitBegin:
                {
                    break;
                }
                case EnableDisableVisible.FrontClick:
                case EnableDisableVisible.GridClick:
                {
                    if (picImage.Image == null)
                    {
                        pbZoomIn.Enabled = false;   
                        pbZoomOut.Enabled = false;
                        pbFront.Enabled = false;
                        pbRear.Enabled = false;

                        return;
                    }

                    pbFront.Enabled = false;
                    pbRear.Enabled = true;

                    //pnlPicture.Focus(); // TODO

                    EnableDisableVisibleLogic(EnableDisableVisible.ZoomClick);

                    break;
                }
                case EnableDisableVisible.RearClick:
                {
                    if (picImage.Image == null)
                    {
                        pbZoomIn.Enabled = false;
                        pbZoomOut.Enabled = false;
                        pbFront.Enabled = false;
                        pbRear.Enabled = false;

                        return; 
                    }

                    pbFront.Enabled = true;
                    pbRear.Enabled = false;

                    EnableDisableVisibleLogic(EnableDisableVisible.ZoomClick);
                
                    break;
                }
                case EnableDisableVisible.ZoomClick:
                {
                    pbZoomIn.Enabled = _currentZoom != Zoom.In;
                    pbZoomOut.Enabled = _currentZoom == Zoom.In;

                    break;
                }
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

        #endregion

        private void GetImages() 
        {
            string xml = _scanItems.ToXML(true, true);

            _tellerCapture.PhxExternalTransaction = xml;

            if (_tellerCapture.CurForm != null)
                _curForm = _tellerCapture.CurForm;

            _tellerCapture.CurForm = this;

            Phoenix.Shared.Variables.TellerVars.Instance.TlCaptureBOBSourceDesc = "Image Retrieval";   //#195669, #35513

            _tellerCapture.InitiateExternalTransactionForImageViewer();
        }

        private void PopulateGrid(TlCaptureScanItemsCollection scanItems) 
        {
            foreach (var item in scanItems)
            {
                grdItems.Rows.Add(
                    item.OrigItemType,  // Item Type
                    item.ScanAux,       // Auxiliary
                    item.ScanRoutingNo, // Routing Number
                    item.ScanAcctNo,    // Account Number
                    item.ScanTranCode,  // Tran Code
                    item.ScanAmt,       // Amount
                    item.ScanISN);      // ISN (Hidden)
            }
        }

        private string GetOriginalISNFromXml(string xml) 
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            //return doc.DocumentElement.SelectSingleNode("/Transaction/Item/PhxISN").InnerText;
            return doc.DocumentElement.SelectSingleNode("/Transaction/Item/OriginalISN").InnerText;
        }

        private void SetImagePaths(string xml, string frontImagePath, string rearImagePath) 
        {
            /* Get ISN number from xml
             * Find TlCaptureScanItem from the collection
             * If found, set the front and rear image paths */

            string isn = GetOriginalISNFromXml(xml);

            if (!string.IsNullOrWhiteSpace(isn)) 
            {
                var scanItem = _scanItems.GetScanItemByISN(isn);

                if (scanItem != null)
                {
                    System.Diagnostics.Debug.WriteLine("ISN found. ISN: " + isn);

                    scanItem.FrontImage = frontImagePath;
                    scanItem.RearImage = rearImagePath;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ISN not found. ISN: " + isn);
                }
            }
        }

        private void DisplayImage() 
        {
            _imageValidationPending = false;    //#56226
            if (_selectedScanItem != null) 
            {
                string value = _currentImageView == ImageView.Front ? 
                    _selectedScanItem.FrontImage : _selectedScanItem.RearImage;

                if (!string.IsNullOrWhiteSpace(value)) 
                {
                    lblNoImage.Visible = false;

                    try
                    {
                        if (picImage.Image != null)
                            picImage.Image.Dispose();

                        picImage.Image = Image.FromFile(value);
                        picImage.Visible = true;

                        SetZoom(Zoom.Out);
                    }
                    catch (Exception ex)
                    { // This can be expected when the TIF image is empty (currently the case for virtual items).
                        lblNoImage.Text = "No Image";   //#56226
                        lblNoImage.Visible = true;
                        picImage.Visible = false;
                        picImage.Image = null;
                    }
                }
            }
        }

        private void SetZoom(Zoom zoom) 
        {
            _currentZoom = zoom;

            if (_currentZoom == Zoom.In)
            {
                picImage.Anchor = AnchorStyles.Left | AnchorStyles.Top;
            }
            else
            {
                picImage.Location = new Point(
                    7 - spltContainer.Panel1.HorizontalScroll.Value, 
                    5 - spltContainer.Panel1.VerticalScroll.Value);

                picImage.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }

            picImage.SizeMode = _currentZoom == Zoom.In ?
                PictureBoxSizeMode.AutoSize : PictureBoxSizeMode.Zoom;

            EnableDisableVisibleLogic(EnableDisableVisible.ZoomClick);
        }

        private void TryDelete(string filePath) 
        {
            try
            {
                System.IO.File.Delete(filePath);
            }
            catch
            {
                // Oh well.
                _imageValidationPending = false;    //#56226
            }
        }

        private void DeleteImages()
        {
            if (_scanItems == null || _scanItems.Count == 0)
                return;

            foreach (var scanItem in _scanItems)
            {
                if (!string.IsNullOrWhiteSpace(scanItem.FrontImage))
                    TryDelete(scanItem.FrontImage);

                if (!string.IsNullOrWhiteSpace(scanItem.RearImage))
                    TryDelete(scanItem.RearImage);
            }

            //#56226
            if (_selectedScanItem != null)
            {
                if (picImage.Image != null)
                    picImage.Image.Dispose();

                if (!string.IsNullOrWhiteSpace(_selectedScanItem.FrontImage))
                    TryDelete(_selectedScanItem.FrontImage);
                if (!string.IsNullOrWhiteSpace(_selectedScanItem.RearImage))
                    TryDelete(_selectedScanItem.RearImage);
            }
        }

    }

    public class MyPDataGridView : PDataGridView
    {
        //public new event GridClickedEventHandler SelectedIndexChanged;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {

                bool retValue = base.ProcessCmdKey(ref msg, keyData); ;
                //if ((keyData & Keys.Down) == Keys.Down || (keyData & Keys.Up) == Keys.Up)
                //{
                //    if (SelectedIndexChanged != null)
                //        SelectedIndexChanged(this, new GridClickEventArgs(LastSelectedIndex, LastSelectedIndex));
                //    retValue = true;
                //}
                //else
                //{
                //    //retValue = base.ProcessCmdKey(ref msg, keyData);
                //}

                return retValue;
            }
            catch (Exception e)
            {
                CoreService.LogPublisher.LogError(e.ToString());

                if (FocusControlOnTab != null && keyData == Keys.Tab)
                    FocusControlOnTab.Focus();
                return true;
            }
        }
        public Control FocusControlOnTab;
    }

}
