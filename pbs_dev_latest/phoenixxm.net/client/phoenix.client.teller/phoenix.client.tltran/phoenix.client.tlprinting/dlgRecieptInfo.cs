#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2015 Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: dlgRecieptInfo.cs
// NameSpace: phoenix.client.tlprinting
// Description: The screen asks the user whether to print, email or print and email receipts.
//              The screen allows the user to enter an email address or use an existing email address.
//-------------------------------------------------------------------------------
//Date							Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/15/2015 5:09:01 PM			1		bschlottman  Created
//12/04/2015                    2       bschlottman #40098 Reorder and reposition fields
//12/10/2015                    3       bschlottman #40177 Enable eReceipt even in account is not selected.
//12/10/2015                    4       bschlottman #40191 Add an error message if the email address has & : ; or , symbols
//12/14/2015                    5       bschlottman #40274 Disable various push buttons on the Receipt Information window
//12/21/2015                    6       bschlottman #40276 Change the receipt information to remember changes and compact changes to minimize auditing.
//01/08/2016                    7       bschlottman #40787 Rename variables.
//01/27/2016                    8       bschlottman #41218 Skip Add/Update message box if there is no primary email address.
//01/28/2016                    9       bschlottman #41215 Translate message box text
//01/28/2016                    10      BSchlottman #41217 Skip xfs printing if receipt_del_method is email only.
//02/29/2016                    11      BSchlottman #40191 Fix a parenthesis problem.
//03/17/2016                    12      BSchlottman #40191 Check for a bad pre-existing email address.
//05/25/2016                    13      BSchlottman #45806 Do not show message box if the user selects a secondary email address.
//05/31/2016                    14      BSchlottman #42567, #43143  Reg CC changes
//08/21/2019                    15      mselvaga    Task#118299 - ECM Voucher printing changes added
//09/26/2019                    16      mselvaga    #119734 - Added ECM print and archive changes.
//10/07/2019                    17      mselvaga    Bug#119986 - Reprint Last Action button should NOT be enabled for the very 1st Print Pop up
//08/19/2020                    18      mselvaga    Task#131288 - HF MASTER CAS-2439387- Release 2020 - When completing a BUY/SELL in customer management.
//09/02/2020                    19      mselvaga    Bug#130743 - Ereceipts - not updating delivery info and email on tx posting when multiple forms attached
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
using System.Xml;

using Phoenix.Client.MsgBox;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.Windows.Forms;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Client;

using Phoenix.BusObj.Teller;

// * NOTE TO PROGRAMMER ****************************************************************************************//
// This is the basic structure of a Phoenix .NET window. Several points of interest are marked with "TODO".
// Template windows have been created as an instructional guide (http://teamwork.harlandfs.com/team/shark/Lists/FAQ/DispForm.aspx?ID=62)
// Additional information can be found here: http://teamwork.harlandfs.com/team/phoenixdev/Programmer%20Resources/Home.aspx
//
// TODO: Add as "link" the file pbs_dev_latest\phoenixxm.net\common\commonassembly.cs
// TODO: Change the file name and references, both here, in the designer file, and in the comments section
// *************************************************************************************************************//
namespace phoenix.client.tlprinting
{
    public partial class dlgRecieptInfo : PfwStandard
    {

        public string DialogOptions { get { return _dialogOptions; } }
        public string LogicalService { get { return _logicalService; } }

        #region Private Variables
        private string _dialogOptions;
        private string _logicalService;
        private PString _psXmlTlTransaction = new PString("PsXmlTlTransaction");    //#40787
        private PString _psXmlTlAcctDetail = new PString("PsXmlTlAcctDetail");      //#40787
        private PString _psUpdateData = new PString("PsUpdateData");    //#40276
        private PString _psConfig = new PString("PsConfig");    //#42567 #43143 Disables screen edits and the email radio button

        private PSmallInt formId = new PSmallInt();
        private PString formDesc = new PString();
        private PString textQrp = new PString();
        private PString formPrintString = new PString();
        private PSmallInt lastLinePrinted = new PSmallInt();
        private PSmallInt formPrintNo = new PSmallInt();
        private PString logicalService = new PString();
        private PString formName = new PString();
        private PString formMediaName = new PString();
        private PString printString = new PString();
        private PString wosaServiceName = new PString();
        private PString defaultEmailAddress = new PString();    //#40787
        private PString defaultReceiptDelMethod = new PString();    //#40787
        private ArrayList _imageData = null;
        private ArrayList UpdateList = new ArrayList(); //#40276
        private ArrayList ConfigList = new ArrayList(); //#42567

        private TellerVars _tellerVars = TellerVars.Instance;
        private PrintInfo _wosaPrintInfo = new PrintInfo();
        private XfsPrinter _xfsPrinter;
        private XmlNode _adTlFormNode = TellerVars.Instance.AdTlFormNode as XmlNode;
        private Phoenix.BusObj.Teller.TlTransaction _tlTran = new TlTransaction();
        private Phoenix.BusObj.Teller.TlAcctDetails _tlDetails = new TlAcctDetails();
        private ReceiptDeliveryInstruction _myInstruction = new ReceiptDeliveryInstruction();
        private ReceiptDeliveryInstruction _oldInstruction = new ReceiptDeliveryInstruction();  //#40276
        Object myObject = new Object(); //#40276
        private int printFocussedCount = 0;

        public const string PRINTANDEMAIL = "Print and Email";
        public const string EMAIL = "Email";
        public const string PRINT = "Print";

        private enum EnableDisableVisible
        {
            EmailDisable,
            EmailEnable,
            EmailRadioDisable,
            InitBegin,
            InitComplete,
            LogicalServiceDisable,
            LogicalServiceEnable,
            ReceiptDeliveryDisable,
            RegCcDisable
        }   //#42567, #43143
        #endregion

        #region Constructors
        public dlgRecieptInfo()
        {
            InitializeComponent();
        }
        #endregion

        #region Form Events
        /// <summary>
        /// Event executed before form is populated with data
        /// </summary>
        /// <returns></returns>
        private ReturnType dlgRecieptInfo_PInitBeginEvent()
        {
            BusObjSerializer.LoadFromXml(_tlTran, _psXmlTlTransaction.Value);   //#40787
            BusObjSerializer.LoadFromXml(_tlDetails, _psXmlTlAcctDetail.Value); //#40787

            UpdateList = ArrayList.Adapter(_psUpdateData.Value.Split('~'));    //#40276
            ConfigList = ArrayList.Adapter(_psConfig.Value.Split('^'));        //#42567

            lblMulti.ForeColor = System.Drawing.Color.Blue; //#40098

            if (_tlDetails.FromJournal.Value == "1")    //From customer management tab in teller
            {
                pbContinue.ObjectId = 25;
                pbReprintLast.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                pbSkip.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                pbCancel.ObjectId = 26;
            }
            
            this.pbCancel.NextScreenId = 0;
            this.pbContinue.NextScreenId = 0;
            this.pbReprintLast.NextScreenId = 0;
            this.pbSkip.NextScreenId = 0;
            this.pbArchiveOnly.NextScreenId = 0;
            // todo: set ScreenId, ScreenIdML(optional), IsNew, MainBusinessObject etc

            // To make sure any changes made to values in a bus obj are displayed correctly on parent screens, make sure you set the UseStateFromBusinessObject 
            this.UseStateFromBusinessObject = true;
            // set business object for edit/select/new/update for this form. Must happen after UseStateFromBusinessObject
            // this.MainBusinesObject = [BO];

            this.GotFocus += new EventHandler(Control_GotFocus);
            this.cmbWosaService.GotFocus += new EventHandler(Control_GotFocus);

            return default(ReturnType);
        }

        protected override void OnShown(EventArgs e)    //#131288
        {
            base.OnShown(e);

            pbContinue.ImageButton.Focus();
        }

        private void dlgRecieptInfo_PInitCompleteEvent()
        {
            Phoenix.FrameWork.BusFrame.EnumValue removedEnumValue = null;   //#40276
            Phoenix.FrameWork.BusFrame.EnumValue newEnumValue = null;       //#40276

            this.DefaultAction = pbContinue;
            if (!formId.IsNull)
            {
                if (_tellerVars.SetContextObject("AdTlFormArray", null, formId.Value))
                {
                    formDesc.Value = _tellerVars.AdTlForm.Description.Value;
                    textQrp.Value = _tellerVars.AdTlForm.TextQrp.Value;
                    formPrintString.Value = _tellerVars.AdTlForm.PrintString.Value;
                    logicalService.Value = _tellerVars.AdTlForm.ServiceType.Value;
                    formName.Value = _tellerVars.AdTlForm.FormName.Value;
                    formMediaName.Value = _tellerVars.AdTlForm.MediaName.Value;
                    wosaServiceName.Value = _tellerVars.AdTlForm.LogicalService.Value;
                    printString.Value = _tellerVars.AdTlForm.PrintString.Value; //#76033
                    //

                    //
                }
            }

            if (!_tlTran.TranCode.IsNull)
            {
                dfTransaction.Value = Convert.ToString(_tlTran.TranCode.Value) + " - " + _tlTran.TranCodeDesc.Value;
            }
            if (!_tlTran.NetAmt.IsNull)
            {
                dfAmount.Value = _tlTran.NetAmt.Value;
            }
            if ((_tlDetails.AcctNo.Value != null) & (_tlDetails.AcctType.Value != null))
            {
                dfAccount.Value = _tlDetails.AcctType.Value + " - " + _tlDetails.AcctNo.Value;
            }
            if (!_tlDetails.RimNo.IsNull)
            {
                dfCustomer.Value = Convert.ToString(_tlDetails.RimNo.Value);
                dfCustomer.Value = dfCustomer.Value + " - " + _tlDetails.RimFirstName.Value + " " +
                    _tlDetails.RimMiddleInitial.Value + " " + _tlDetails.RimLastName.Value;
            }
            else
            {
            }

            defaultReceiptDelMethod.Value = _tlDetails.ReceiptDelMethod.Value;      //#40787
            defaultEmailAddress.Value = _tlDetails.PrimaryEmail.Value;  //#40276    //#40787
            //cmbEmail.CodeValue = _tlDetails.PrimaryEmail.Value; //#40276 comment out #40787

            cmbEmail.Items.Clear();
            ArrayList emailCollection = new ArrayList();
            if (_tlDetails.EmailList.SqlString != "null")   //#40177
            {
                string[] printerServices = _tlDetails.EmailList.Value.Split("~".ToCharArray());
                foreach (string svcs in printerServices)
                {
                    if (svcs != "")
                    {
                        emailCollection.Add(new EnumValue(svcs, svcs));
                    }
                }
                cmbEmail.Populate(emailCollection);
            }

            //begin #40276
            if (!_tlDetails.RimNo.IsNull)
            {
                foreach (string xml in UpdateList)
                {
                    if (xml != "")
                    {
                        myObject = _oldInstruction.ObjectToXML(xml);
                        _oldInstruction = (ReceiptDeliveryInstruction)myObject;

                        if (_oldInstruction.RimNo == _tlDetails.RimNo.Value)
                        {
                            //if it is read only, default the receipt delivery method to the last receipt delivery method selected.
                            if (_oldInstruction.ReceiptDelMethodAction == (int)XmActionType.Update || ConfigList.Contains("ReadOnly"))    //#42567
                            {
                                defaultReceiptDelMethod.Value = _oldInstruction.ReceiptDelMethod;   //#40787
                            }

                            if (_oldInstruction.EmailAddressAction == (int)XmActionType.New)
                            {
                                newEnumValue = new EnumValue(_oldInstruction.EmailAddress, _oldInstruction.EmailAddress);
                                if (!emailCollection.Contains(newEnumValue))
                                {
                                    emailCollection.Add(newEnumValue);
                                    cmbEmail.Populate(emailCollection);
                                }
                            }

                            if (_oldInstruction.EmailAddressAction == (int)XmActionType.Update)
                            {
                                newEnumValue = new EnumValue(_oldInstruction.EmailAddress, _oldInstruction.EmailAddress);
                                removedEnumValue = new EnumValue(_oldInstruction.OldEmail1, _oldInstruction.OldEmail1);
                                if (!emailCollection.Contains(newEnumValue))
                                {
                                    if (emailCollection.Contains(removedEnumValue))
                                    {
                                        emailCollection.Remove(removedEnumValue);
                                    }
                                    emailCollection.Add(newEnumValue);
                                    cmbEmail.Populate(emailCollection);
                                }
                            }
                            //if it is read only use default the email address to the last email address selected
                            if (_oldInstruction.EmailAddressAction == (int)XmActionType.Update ||
                                (defaultEmailAddress.Value == "" && _oldInstruction.EmailAddressAction == (int)XmActionType.New) || ConfigList.Contains("ReadOnly"))   //#40787   #42567
                            {
                                defaultEmailAddress.Value = _oldInstruction.EmailAddress;   //#40787
                                //cmbEmail.CodeValue = _oldInstruction.EmailAddress; comment out #40276
                            }
                        }
                    }
                }
                if (_tlDetails.RimNo.Value == 0)    //#42567
                {
                    defaultReceiptDelMethod.Value = PRINT;  //If there is no rim on this transaction, default receipt delivery method to print.
                }
            }
            else
            {
                defaultReceiptDelMethod.Value = PRINT;  //#42567 If there is no rim on this transaction, default receipt delivery method to print.
            }

            cmbEmail.CodeValue = defaultEmailAddress.Value; //#40276

            if (defaultReceiptDelMethod.Value == PRINT) //#40276
            {
                rbPrint.Checked = true;
            }
            else if (defaultReceiptDelMethod.Value == EMAIL && !ConfigList.Contains("PrintRequired"))    //#40276 if Reg CC disables email, don't set it to email
            {
                rbEmail.Checked = true;
            }
            else if (defaultReceiptDelMethod.Value == PRINTANDEMAIL || (ConfigList.Contains("PrintRequired") && defaultReceiptDelMethod.Value == EMAIL))    //#40276 if it is email and Reg CC disables email, set it to print and email.
            {
                rbPrintAndEmail.Checked = true;
            }
            else
            {
                rbPrint.Checked = true;
            }
            //end  #40276

            if (!lastLinePrinted.IsNull && lastLinePrinted.Value >= 0)
                this.dfPbLastLine.UnFormattedValue = lastLinePrinted.Value;
            this.dfForm.Text = formDesc.Value;

            if (WosaServicesHelper.PopulateCombo(cmbWosaService) > 0)
            {
                if (!wosaServiceName.IsNull && StringHelper.StrTrimX(wosaServiceName.Value) != String.Empty)
                {
                    WosaServicesHelper.SetWosaService(cmbWosaService, logicalService.StringValue, wosaServiceName.StringValue);
                }
                else
                {
                    this.cmbWosaService.SelectedIndex = 0;
                }
            }

            this.pbReprintLast.Image = Images.Print;
            this.pbCancel.Image = Images.Cancel;

            if (_tlDetails.FromJournal.Value == "1")
            {
                pbContinue.Image = Images.Save;
            }
            else
            {
                this.pbContinue.Image = Images.Print;
            }

            EnableDisableVisibleLogic(EnableDisableVisible.InitComplete);

            if (_tlDetails.RimNo.IsNull || _tlDetails.RimNo.Value == 0)    //#42567 if there is no rim on transaction then disable receipt delivery method
            {
                EnableDisableVisibleLogic(EnableDisableVisible.ReceiptDeliveryDisable);
            }

            if (ConfigList.Contains("PrintRequired"))    //#43143 if Reg CC disable the email radio button
            {
                EnableDisableVisibleLogic(EnableDisableVisible.EmailRadioDisable);
            }

            if (ConfigList.Contains("ReadOnly"))  //#42567 If Reg CC has already printed three lines, disable edits on this dialog.
            {
                EnableDisableVisibleLogic(EnableDisableVisible.RegCcDisable);
            }
            //dfNewEmail.Focus();
            SetFieldReadOnly(dfForm);   //#131288
            //#131288 - Commented out tab key caused incorrect window object focus on form shown
            //SendKeys.Send("{tab}");
            //if (cmbWosaService.Enabled)
            //{
            //    SendKeys.Send("{tab}");
            //}
            this.DefaultAction = pbContinue;
        }

        private void SetFieldReadOnly(PdfStandard dfField)  //#131288
        {
            dfField.Font = new Font(dfField.Font, FontStyle.Bold);
            dfField.ReadOnly = true;
            dfField.BackColor = Color.White;
            dfField.TabStop = false;
        }

        private void rbPrint_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPrint.Checked)
            {
                EnableDisableVisibleLogic(EnableDisableVisible.EmailDisable);
                EnableDisableVisibleLogic(EnableDisableVisible.LogicalServiceEnable);
            }
        }

        private void rbEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEmail.Checked)
            {
                EnableDisableVisibleLogic(EnableDisableVisible.EmailEnable);
                EnableDisableVisibleLogic(EnableDisableVisible.LogicalServiceDisable);
            }
        }

        private void rbPrintAndEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPrintAndEmail.Checked)
            {
                EnableDisableVisibleLogic(EnableDisableVisible.EmailEnable);
                EnableDisableVisibleLogic(EnableDisableVisible.LogicalServiceEnable);
            }
        }

        private void dfNewEmail_Leave(object sender, EventArgs e)
        {
            if (!emailIsValid(dfNewEmail.Text))
            {
                PMessageBox.Show(11285, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                // Please enter a valid email address.
                dfNewEmail.Focus();
            }
            if (IsEmailCompound(dfNewEmail.Text))   //#40191
            {
                PMessageBox.Show(11697, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                //Please enter a valid email address that does not contain the following symbols & : ; or ,  
                //The email cannot have an ampersand, colon, semi-colon, or a comma.
                dfNewEmail.Focus();
            }
        }

        private void pbContinue_Click(object sender, PActionEventArgs e)
        {
            DialogResult dialogResult;
            string sEmailAddress = "";
            string sReceiptDelMethod = "";

            SetEmailandReceipt(ref sEmailAddress, ref sReceiptDelMethod);
            _myInstruction.RimNo = _tlDetails.RimNo.Value;
            _myInstruction.EmailAddress = sEmailAddress;
            _myInstruction.ReceiptDelMethod = sReceiptDelMethod;
            _myInstruction.OldReceiptDelMethod = defaultReceiptDelMethod.Value; //#40276    //#40787
            _myInstruction.OldEmail1 = defaultEmailAddress.Value;   //#40276    //#40787

            if (rbEmail.Checked || rbPrintAndEmail.Checked) //#40191
            {
                if (sEmailAddress == "" || !emailIsValid(sEmailAddress))
                {
                    PMessageBox.Show(15406, MessageType.Error, MessageBoxButtons.OK, String.Empty); //#40191
                    // Please enter a valid email address.
                    dfNewEmail.Focus();
                    return;
                }
                if (IsEmailCompound(sEmailAddress))
                {
                    PMessageBox.Show(15407, MessageType.Error, MessageBoxButtons.OK, string.Empty); //#40191
                    //Please enter a valid email address that does not contain the following symbols & : ; or ,  
                    //The email cannot have an ampersand, colon, semi-colon, or a comma.
                    dfNewEmail.Focus();
                    return;
                }
            }

            if (_tellerVars.AdTlControl.EnableEreceiptChanges.Value == "Y" && defaultEmailAddress.Value != sEmailAddress && sEmailAddress != "")    //#40787
            {
                _myInstruction.EmailAddressAction = (int)XmActionType.Update;   //#41218 Default
                if (defaultEmailAddress.Value != "")    //#41218 If no primary email address skip the question and default it to update.
                {
                    if (Convert.ToString(dfNewEmail.Value) == "")   //#45806  Do not show message box if combo box is selected. Use the email address but do not change anything.
                    {
                        _myInstruction.EmailAddressAction = (int)XmActionType.None;
                    }
                    else
                    {
                        dialogResult = Phoenix.Client.MsgBox.frmMsgBoxCustom.ShowDialog("Phoenix Question", CoreService.TranslateText("Does the customer want to change their current Primary email address or add this address to their email address book?", GlobalVars.InstitutionType, this.FormSource), null, "&Add New Address", null, "&Update Primary Email", SystemIcons.Question.ToBitmap()); //#41215
                        if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                        {
                            _myInstruction.EmailAddressAction = (int)XmActionType.New;
                        }
                        else
                        {
                            _myInstruction.EmailAddressAction = (int)XmActionType.Update;
                        }
                    }
                }
            }
            else
            {
                _myInstruction.EmailAddressAction = (int)XmActionType.None;
            }

            if (_tellerVars.AdTlControl.EnableEreceiptChanges.Value == "Y" && defaultReceiptDelMethod.Value != sReceiptDelMethod && !ConfigList.Contains("ReadOnly"))   //#40787 //#42567 Don't keep asking this question for RegCC.  We already did it once.
            {
                dialogResult = Phoenix.Client.MsgBox.frmMsgBoxCustom.ShowDialog("Phoenix Question", CoreService.TranslateText("Does the customer want to change their current receipt delivery method permanently or just for this transaction?", GlobalVars.InstitutionType, this.FormSource), null, "&Permanent Change", null, "&Only This Transaction", SystemIcons.Question.ToBitmap());    //#41215
                if (dialogResult == System.Windows.Forms.DialogResult.Yes)
                {
                    _myInstruction.ReceiptDelMethodAction = (int)XmActionType.Update;
                }
                else
                {
                    _myInstruction.ReceiptDelMethodAction = (int)XmActionType.None;
                }
            }
            else
            {
                _myInstruction.ReceiptDelMethodAction = (int)XmActionType.None;
            }


            if (defaultEmailAddress.Value == "" && defaultEmailAddress.Value != sEmailAddress && sEmailAddress != "")   //#40787
            {
                if (_tellerVars.AdTlControl.EnableEreceiptChanges.Value == "Y")
                {
                    if (_myInstruction.EmailAddressAction == (int)XmActionType.New || _myInstruction.EmailAddressAction == (int)XmActionType.Update)
                    {
                        PMessageBox.Show(11695, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                        //"The new email address will be set as the customer’s primary email address."
                    }
                }
                else
                {
                    PMessageBox.Show(11696, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                    //"The new email address will be used for this receipt only."
                }
            }

            if (_oldInstruction != null)    //#130743
            {
                if (_oldInstruction.ReceiptDelMethodAction == (int)XmActionType.New || _oldInstruction.ReceiptDelMethodAction == (int)XmActionType.Update)
                {
                    if (_myInstruction.ReceiptDelMethodAction == (int)XmActionType.None)
                    {
                        _myInstruction.ReceiptDelMethodAction = _oldInstruction.ReceiptDelMethodAction;
                        _myInstruction.ReceiptDelMethod = _oldInstruction.ReceiptDelMethod;
                    }
                }

                if (_oldInstruction.EmailAddressAction == (int)XmActionType.New || _oldInstruction.EmailAddressAction == (int)XmActionType.Update)
                {
                    if (_myInstruction.EmailAddressAction == (int)XmActionType.None)
                    {
                        _myInstruction.EmailAddressAction = _oldInstruction.EmailAddressAction;
                        _myInstruction.EmailAddress = _oldInstruction.EmailAddress;
                    }
                }
            }

            if (_tlDetails.FromJournal.Value == "Y" || rbEmail.Checked) //#41217
            {
                // don't print, journal has its own printing.   Don't print if email only.
            }
            else
            {
                if (textQrp.Value == CoreService.Translation.GetListItemX(ListId.TextType, "T") ||
                    textQrp.Value == CoreService.Translation.GetListItemX(ListId.TextType, "Q") ||
                    textQrp.Value == CoreService.Translation.GetListItemX(ListId.TextType, "I") ||
                    textQrp.Value == CoreService.Translation.GetListItemX(ListId.TextType, "S") ||  // #76057
                    textQrp.Value == CoreService.Translation.GetListItemX(ListId.TextType, "C"))
                {
                    //test
                    if (_tellerVars.AdTlControl.WosaPrinting.Value == GlobalVars.Instance.ML.Y)
                    {
                        _wosaPrintInfo.PbLastLine = Convert.ToInt32(dfPbLastLine.UnFormattedValue);
                        if (!printString.IsNull && printString.Value.IndexOf("[Voucher]") >= 0) //#76033
                        {
                            _wosaPrintInfo.WosaFormName = formName.Value;
                            _wosaPrintInfo.WosaMediaName = formMediaName.Value;
                            _wosaPrintInfo.FormPrintStr = printString.Value;
                            _wosaPrintInfo.FormID = formId.Value;   //#119734
                        }

                        //#157637 populate imagedata arraylist with data printed, expected by the parent form
                        _xfsPrinter = new XfsPrinter(cmbWosaService.Text);
                        _xfsPrinter.UseImaging = TellerVars.Instance.IsHylandVoucherAvailable || TellerVars.Instance.IsECMVoucherAvailable; //#118299
                        try
                        {
                            byte[] image = null;
                            if (_xfsPrinter.PrintFormAndReturnImage(formMediaName.Value, formName.Value, _wosaPrintInfo, out image, false))   // #157637 (2)
                            {
                                Bitmap combinedBitmap;
                                SigBoxDetails sigPos;
                                if (_xfsPrinter.GetCombinedBitmapAndSigBoxDetails(out combinedBitmap, out sigPos))
                                {
                                    _imageData.Clear();
                                    _imageData.Add(combinedBitmap);
                                    _imageData.Add(sigPos);
                                    _imageData.Add(_wosaPrintInfo);
                                }
                            }
                            else
                                pbContinue.ImageButton.Focus();		// #72204
                        }
                        finally
                        {
                            _xfsPrinter.Close();
                        }
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pbReprintLast_Click(object sender, PActionEventArgs e)
        {
            string sEmailAddress = "";
            string sReceiptDelMethod = "";

            SetEmailandReceipt(ref sEmailAddress, ref sReceiptDelMethod);

            _myInstruction.RimNo = _tlDetails.RimNo.Value;
            _myInstruction.EmailAddress = sEmailAddress;
            _myInstruction.EmailAddressAction = (int) XmActionType.None;
            _myInstruction.ReceiptDelMethod = sReceiptDelMethod;
            _myInstruction.OldReceiptDelMethod = _tlDetails.ReceiptDelMethod.Value;
            _myInstruction.ReceiptDelMethodAction = (int) XmActionType.None;
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        private void pbSkip_Click(object sender, PActionEventArgs e)
        {
            string sEmailAddress = "";
            string sReceiptDelMethod = "";

            SetEmailandReceipt(ref sEmailAddress, ref sReceiptDelMethod);

            _myInstruction.RimNo = _tlDetails.RimNo.Value;
            _myInstruction.EmailAddress = sEmailAddress;
            _myInstruction.EmailAddressAction = (int)XmActionType.None;
            _myInstruction.ReceiptDelMethod = sReceiptDelMethod;
            _myInstruction.OldReceiptDelMethod = _tlDetails.ReceiptDelMethod.Value;
            _myInstruction.ReceiptDelMethodAction = (int)XmActionType.None;
            this.DialogResult = DialogResult.Ignore;
            this.Close();
        }

        private void pbCancel_Click(object sender, PActionEventArgs e)
        {
            string sEmailAddress = "";
            string sReceiptDelMethod = "";

            SetEmailandReceipt(ref sEmailAddress, ref sReceiptDelMethod);

            _myInstruction.RimNo = _tlDetails.RimNo.Value;
            _myInstruction.EmailAddress = sEmailAddress;
            _myInstruction.EmailAddressAction = (int)XmActionType.None;
            _myInstruction.ReceiptDelMethod = sReceiptDelMethod;
            _myInstruction.OldReceiptDelMethod = _tlDetails.ReceiptDelMethod.Value;
            _myInstruction.ReceiptDelMethodAction = (int)XmActionType.None;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pbArchiveOnly_Click(object sender, PActionEventArgs e)
        {
            string sEmailAddress = "";
            string sReceiptDelMethod = "";

            _myInstruction.RimNo = _tlDetails.RimNo.Value;
            _myInstruction.EmailAddress = sEmailAddress;
            _myInstruction.EmailAddressAction = (int)XmActionType.None;
            _myInstruction.ReceiptDelMethod = sReceiptDelMethod;
            _myInstruction.OldReceiptDelMethod = _tlDetails.ReceiptDelMethod.Value;
            _myInstruction.ReceiptDelMethodAction = (int)XmActionType.None;
            if (_tlDetails.FromJournal.Value == "Y")
            {
                // don't print, teller journal has its own printing
            }
            this.DialogResult = DialogResult.No;
            this.Close();
        }
        #endregion

        #region Overriddes
        /// <summary>
        /// Move parameters into local variables
        /// </summary>
        //public override void OnCreateParameters()
        //{
        //    Parameters.Add(_psTranObject);
        //    Parameters.Add(_psDetailsObject);
        //    Parameters.Add(formId);
        //    Parameters.Add(formPrintNo);
        //    Parameters.Add(lastLinePrinted);
        //    base.OnCreateParameters();
        //}
        public override bool InitializeForm()
        {
            bool avoidSavePrev = AvoidSave;
            try
            {
                AvoidSave = true;
                return base.InitializeForm();
            }
            finally
            {
                AvoidSave = avoidSavePrev;
            }
        }

        //InitParameters is used to just like dlgPrintForms.
        public override void InitParameters(params object[] paramList)
        {
            if (paramList[0] != null)
                _psXmlTlTransaction.Value = Convert.ToString(paramList[0]); //#40787
            if (paramList[1] != null)
                _psXmlTlAcctDetail.Value = Convert.ToString(paramList[1]);  //#40787
            //begin #40276
            if (paramList[2] != null)
                _psUpdateData.Value = Convert.ToString(paramList[2]);
            //end   #40276
            //begin #42567
            if (paramList[3] != null)
                _psConfig.Value = Convert.ToString(paramList[3]);  //#43143 Disables the scrren and email radio button
            //end   #42567
            if (paramList[4] != null)
                formId.Value = Convert.ToInt16(paramList[4]);
            if (paramList[5] != null)
                formPrintNo.Value = Convert.ToInt16(paramList[5]);
            if (paramList[6] != null)
                lastLinePrinted.Value = Convert.ToInt16(paramList[6]);
            if (paramList[7] != null)
                _wosaPrintInfo = (PrintInfo)paramList[7];
            if (paramList[8] != null)
                _imageData = (paramList[8] != null) ? ((ArrayList)paramList[8]) : null;

            if (!formId.IsNull)
            {
                if (_tellerVars.SetContextObject("AdTlFormArray", null, formId.Value))
                {
                    formDesc.Value = _tellerVars.AdTlForm.Description.Value;
                    textQrp.Value = _tellerVars.AdTlForm.TextQrp.Value;
                    formPrintString.Value = _tellerVars.AdTlForm.PrintString.Value;
                    logicalService.Value = _tellerVars.AdTlForm.ServiceType.Value;
                    formName.Value = _tellerVars.AdTlForm.FormName.Value;
                    formMediaName.Value = _tellerVars.AdTlForm.MediaName.Value;
                    wosaServiceName.Value = _tellerVars.AdTlForm.LogicalService.Value;
                    printString.Value = _tellerVars.AdTlForm.PrintString.Value; //#76033
                }
            }

            #region say no to default framework select
            this.AutoFetch = false;
            #endregion

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

            return base.OnActionSave(isAddNext);
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
                _dialogOptions = _myInstruction.GetXMLFromObject(_myInstruction);
                _logicalService = Convert.ToString(cmbWosaService.Value);
            }

            return bRet;
        }

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
                    dfTransaction.SetObjectStatus(NullabilityState.Default, VisibilityState.Show,  EnableState.Disable);
                    dfAmount.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                    dfAccount.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                    dfCustomer.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);

                    if (dfAmount.Value == null)
                    {
                        dfForm.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                        cmbWosaService.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                    }
                    dfPbLastLine.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                    if (cmbEmail.Items.Count > 1)
                    {
                        cmbEmail.Items.Remove("<none>");
                        cmbEmail.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Default, EnableState.Default);
                    }
                    if (_tlDetails.FromJournal.Value == "1")
                    {
                        pbReprintLast.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        pbSkip.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        lblNote.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        lblNote2.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                    }
                    if (_tlDetails.FromJournal.Value == "Y")
                    {
                        pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        pbSkip.SetObjectStatus(VisibilityState.Default, EnableState.Disable);  //#40274
                        lblNote.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                        lblNote2.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                    }
                    //begin #40274
                    if (_tlDetails.FromJournal.Value == "N" && dfAmount.Value == null)
                    {
                        pbReprintLast.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        pbSkip.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                        pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                    }
                    //end   #40274
                    if (_tlDetails.FromMTForms.Value == "Y" || _tlDetails.FromJournal.Value == "Y" || formPrintNo.Value == 1)   //#119986
                    {
                        pbReprintLast.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                    }
                    if (_tlDetails.FromMTForms.Value == "N")
                    {
                        pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                    }
                    if (_tlDetails.FromMTForms.Value != "Y")
                    {
                        lblMulti.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
                    }
                    if (_tellerVars.IsHylandVoucherAvailable == false && _tellerVars.IsECMVoucherAvailable == false) //#118299
                    {
                        rbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                        rbPrintAndEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                        pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                    }
                    break;
                case EnableDisableVisible.InitBegin:
                    break;
                case EnableDisableVisible.EmailDisable:
                    cmbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    dfNewEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    break;
                case EnableDisableVisible.EmailEnable:
                    cmbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    dfNewEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    break;
                case EnableDisableVisible.EmailRadioDisable:    //#43143 Disable the email radio button
                    rbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    break;
                case EnableDisableVisible.LogicalServiceDisable:
                    dfForm.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    cmbWosaService.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    break;
                case EnableDisableVisible.LogicalServiceEnable:
                    if (dfAmount.Value != null)
                    {
                        dfForm.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                        cmbWosaService.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    }
                    break;
                case EnableDisableVisible.ReceiptDeliveryDisable:   //#43143 Disable all the radio buttons
                    rbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    rbPrintAndEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    rbPrint.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    break;
                case EnableDisableVisible.RegCcDisable: //#42567 Disable screen edits
                    rbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    rbPrintAndEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    rbPrint.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    cmbEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    dfNewEmail.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    dfForm.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    cmbWosaService.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    pbArchiveOnly.SetObjectStatus(VisibilityState.Hide, EnableState.Disable);
                    pbReprintLast.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                    pbSkip.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                    break;
            }
        }

        private void SetEmailandReceipt( ref string sEmailAddress, ref string sReceiptDelMethod)
        {
            if (Convert.ToString(dfNewEmail.Value) != "")
            {
                sEmailAddress = Convert.ToString(dfNewEmail.Value);
            }
            else
            {
                sEmailAddress = Convert.ToString(cmbEmail.CodeValue);
            }


            if (rbPrint.Checked)
            {
                sReceiptDelMethod = PRINT;
            }
            else if (rbEmail.Checked)
            {
                sReceiptDelMethod = EMAIL;
            }
            else
            {
                sReceiptDelMethod = PRINTANDEMAIL;
            }
        }

        private bool emailIsValid(string emailAddr)
        {
            if (StringHelper.StrTrimX(emailAddr) != "" && (emailAddr.IndexOf(".") <= 0 || emailAddr.IndexOf("@") < 0 || emailAddr.IndexOf(".") == emailAddr.Length - 1))
                return false;
            else
                return true;
        }
        //begin #40191
        // Description: returns true is the sting has more than one email address in it.
        private bool IsEmailCompound(string emailAddr)
        {
            if (StringHelper.StrTrimX(emailAddr) != "" && (emailAddr.IndexOf("&") >= 0 || emailAddr.IndexOf(":") >= 0 || emailAddr.IndexOf(";") >= 0 || emailAddr.IndexOf(",") >= 0))
                return true;
            else
                return false;
        }
        //end   #40191
        private void Control_GotFocus(object sender, EventArgs e)
        {
            if (printFocussedCount < 3)
            {
                pbContinue.ImageButton.Focus();
                //this.Invalidate();
                printFocussedCount++;
            }
        }

        #endregion


    }

	internal class WosaServicesHelper
	{
		public static int PopulateCombo(PComboBoxStandard svcsCombo)
		{
			if (svcsCombo == null)
				return 0;
			svcsCombo.Items.Clear();
			List<string> printerServices = (new XfsRegHelper()).GetLogicalServices(XfsRegHelper.ServiceFilter.Printer);
			ArrayList evCollection = new ArrayList();
			foreach (string svcs in printerServices)
			{
				evCollection.Add(new EnumValue(svcs, svcs));

			}
			svcsCombo.Populate(evCollection);
			return printerServices.Count;
		}


		public static void SetWosaService(PComboBoxStandard cmbXfsSvcs, string serviceType, string formLogicalService)
		{
			string serviceToUse = GetLogicalService(serviceType, formLogicalService); //logicalService.StringValue, wosaServiceName.StringValue);
			cmbXfsSvcs.SelectedIndex = 0;
            if (serviceToUse != null)
            {
                for (int i = 0; i < cmbXfsSvcs.Items.Count; i++)
			    {
				    if (cmbXfsSvcs.Items[i] != null &&
					    serviceToUse.ToUpper() == Convert.ToString(cmbXfsSvcs.Items[i]).ToUpper())
				    //wosaServiceName.Value.ToUpper() == Convert.ToString( cmbWosaService.Items[i] ).ToUpper())
				    {
					    cmbXfsSvcs.SelectedIndex = i;
					    return;
				    }
			    }
            }
		}

		/// <summary>
		/// Get the logical service usable for the form
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="formLogicalService"></param>
		/// <returns></returns>
		public static string GetLogicalService(string serviceType, string formLogicalService)
		{
			if (TellerVars.Instance.IsRemotePrinting)
			{
				int ML_NC_LogicalServices = 647;
				if (serviceType == CoreService.Translation.GetListItemX(ML_NC_LogicalServices, "Receipt")
					|| serviceType == CoreService.Translation.GetListItemX(ML_NC_LogicalServices, "Journal"))
					return TellerVars.Instance.PrinterSvcReceipt;
				else if (serviceType == CoreService.Translation.GetListItemX(ML_NC_LogicalServices, "Document")
				|| serviceType == CoreService.Translation.GetListItemX(ML_NC_LogicalServices, "Passbook"))
					return TellerVars.Instance.PrinterSvcPassbook;
				else if (serviceType == CoreService.Translation.GetListItemX(ML_NC_LogicalServices, "Eforms"))
					return TellerVars.Instance.PrinterSvcsEForm;
			}
			return formLogicalService;

		}
	}
}