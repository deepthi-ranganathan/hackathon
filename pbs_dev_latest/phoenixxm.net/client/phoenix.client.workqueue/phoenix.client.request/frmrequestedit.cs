#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - frmRequestEdit - Edit ther Request record.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: frmRequestEdit.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/20/06		1		VDevadoss	#69248 - Created
//12/12/07      2       SDighe      #73762 - The enter key does not add a paragraph and saves the record.
//11/17/2008    3       Nelsehety   #01698 - .Net Porting
//01/26/2009    4       Nelsehety   #01698 -Bug Fixing
//03/08/2009    5       ewahib      #01698 - Add Code to set Focus On correct tab for Mandatory fields
//03/08/2009    6       ewahib      #01698 - #GEMS #70179 retrofit 
//09/10/2009    7       aHussein    #77295-aHussein Re-Porting Issues
//10/15/2009    8       Mona        #06057 
//01/08/2010    9       vsharma     #6683 , #6684 - Do not force security if there is no security defined in admin
//07/24/2010    10      VDevadoss   #9885 - Added code to pop the category and queue id properly.
//01/23/2011    11      jwatts      #15234 - To ensure work_status of completed Referrals and Requests have proper vals.
//08/06/2012    12      Mkrishna    #19058 - Adding call to base on initParameters.
//12/30/2013    13      NKasim      #140811 - Sales and Services. Embedded frmWqUserDefinedFeildsEdit in third tab.
//01/30/2014    21      NKasim      #26953 - Additional Fields validation message is shown only OnActionSave(). 
//                                          OnActionClose() shows Default message. 
//                                          PerformCheck() handles isDirty from Container and Embedded windows
//02/20/2014    22      NKasim      #27262 - On changing category instance of frmUserDefVal cleared.
//02/24/2014    23      NKasim      #27225 - Owner employee is Loaded from Gb_work_Queue.owner_empl_id if exists
//05/22/2014	24		JRHyne		WI#29008 - adhoc window, set ui pref=  false, set adhoc window grid dimensions to fit
//03/16/2018    25      Ashok HCL   #74463 - Additional Fields Information on Sales & Service Requests are not displaying in Member Management on the Edit Existing Request window>Additional Information tab when Sales & Service
//12/07/2018    26      SChacko     Task#104392 -  None value is not saving for an Additional Field
//02/01/2019    27      RBhavsar     #94129 - Fixing coverity issue for dereference after null check
//10/07/2019    28      SChacko     US#117573/Task#120098 - Getting error when adding new Request - added null validation. 
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using Phoenix.BusObj.Global;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Misc;//Task #74463
using System.Collections.Generic;//Task #74463

namespace Phoenix.Client.WorkQueue
{
    /// <summary>
    /// Summary description for frmRequestEdit.
    /// </summary>
    public class frmRequestEdit : Phoenix.Windows.Forms.PfwStandard
    {
        #region Private variables

        private bool wasFormNew = true;
        private Phoenix.BusObj.Global.GbWorkQueue _gbWorkQueue = new Phoenix.BusObj.Global.GbWorkQueue();
        private Phoenix.BusObj.Global.GbHelper _gbHelper = new Phoenix.BusObj.Global.GbHelper();
        private PfwStandard _parentForm = null;
        private bool bRefreshParent = false;
        PString _reqAcctType = new PString("ReqAcctType");
        PString _reqAcctNo = new PString("ReqAcctNo");
        /*Begin #140811 */
        private Phoenix.Client.DepLoan.frmUserDefinedFieldsEdit frmUserDefFieldsEdit;
        //private bool bFromSave = false;
        private bool bCategoryChanged = false;
        private bool bDirtyUserDefVal = false;
        private bool bIsGridEdited = false;//Task #74463
        /* To default group email check box from AD_WQ_CATEGORY.auto_email_group. */
        private Phoenix.BusObj.Admin.Misc.AdWqCategory _adWqCategory;
        /*End #140811 */

        #region #77295
        private bool _onwerChanged = false;
        #endregion

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        private Phoenix.Windows.Forms.PTabControl picTabs;
        private Phoenix.Windows.Forms.PTabPage dfTabTitle1;
        private Phoenix.Windows.Forms.PTabPage dfTabTitle0;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbRequestInformation;
        private Phoenix.Windows.Forms.PdfStandard dfRequestId;
        private Phoenix.Windows.Forms.PLabelStandard lblCustomer;
        private Phoenix.Windows.Forms.PdfStandard dfCustomer;
        private PLabelStandard lblQueue;
        private PComboBoxStandard cmbQueue;
        private Phoenix.Windows.Forms.PLabelStandard lblCategory;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbCategory;
        private Phoenix.Windows.Forms.PLabelStandard lblDueDate;
        private Phoenix.Windows.Forms.PdfStandard dfDueDate;
        private Phoenix.Windows.Forms.PLabelStandard lblTime;
        private Phoenix.Windows.Forms.PdfStandard dfDueTime;
        private Phoenix.Windows.Forms.PLabelStandard lblDescription;
        private Phoenix.Windows.Forms.PdfStandard dfDescription;
        private Phoenix.Windows.Forms.PLabelStandard lblDetails;
        private Phoenix.Windows.Forms.PdfStandard mulDetails;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbCustomerContactInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblContactMethod;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbContactMethod;
        private Phoenix.Windows.Forms.PLabelStandard lblContactName;
        private Phoenix.Windows.Forms.PdfStandard dfContactName;
        private Phoenix.Windows.Forms.PLabelStandard lblOtherInformation;
        private Phoenix.Windows.Forms.PdfStandard dfOtherInfo;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbRequestOwnershipInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblOwner;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbOwner;
        private Phoenix.Windows.Forms.PCheckBoxStandard cbEmailOwner;
        private Phoenix.Windows.Forms.PLabelStandard lblInitiatedBy;
        private Phoenix.Windows.Forms.PdfStandard dfInitiatedBy;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbRequestStatusInformation;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbPending;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbInProcess;
        private Phoenix.Windows.Forms.PLabelStandard lblPriority;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbPriority;
        private Phoenix.Windows.Forms.PAction pbGetRIM;
        private Phoenix.Windows.Forms.PdfStandard dfRimNo;
        private Phoenix.Windows.Forms.PdfStandard dfProductAcctType;
        private Phoenix.Windows.Forms.PdfStandard dfProductClassCode;
        private Phoenix.Windows.Forms.PdfStandard dfPrioritySort;
        private Phoenix.Windows.Forms.PdfStandard dfRecordType;
        private Phoenix.Windows.Forms.PdfStandard dfPrevEmplId;
        private Phoenix.Windows.Forms.PdfStandard dfQueueId;
        private Phoenix.Windows.Forms.PdfStandard dfLastWorkDt;
        private Phoenix.Windows.Forms.PLabelStandard lblRequestID;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbCompleted;
        private Phoenix.Windows.Forms.PLabelStandard lblRequestStatus;
        private Phoenix.Windows.Forms.PLabelStandard lblAccount;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbAccount;
        private Phoenix.Windows.Forms.PTabPage dfTabTitle2;
        private PAction pbRemove;
        private PCheckBoxStandard cbEmailGroup;
        private Phoenix.Windows.Forms.PdfStandard dfDueDateWithTime;
        private List<WqUserDefVal> _BoToProcessList; //Task #74463
        #endregion

        #region Constructor
        public frmRequestEdit()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        #endregion

        #region Destructor
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Phoenix.FrameWork.Core.ControlInfo controlInfo1 = new Phoenix.FrameWork.Core.ControlInfo();
            Phoenix.FrameWork.Core.ControlInfo controlInfo2 = new Phoenix.FrameWork.Core.ControlInfo();
            Phoenix.FrameWork.Core.ControlInfo controlInfo3 = new Phoenix.FrameWork.Core.ControlInfo();
            this.picTabs = new Phoenix.Windows.Forms.PTabControl();
            this.dfTabTitle0 = new Phoenix.Windows.Forms.PTabPage();
            this.gbRequestInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblAccount = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbAccount = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.mulDetails = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDetails = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDescription = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueTime = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTime = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueDate = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDueDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbQueue = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblQueue = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbCategory = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblCategory = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfCustomer = new Phoenix.Windows.Forms.PdfStandard();
            this.lblCustomer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRequestId = new Phoenix.Windows.Forms.PdfStandard();
            this.lblRequestID = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbRequestStatusInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cmbPriority = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblPriority = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbCompleted = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbInProcess = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbPending = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.lblRequestStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTabTitle1 = new Phoenix.Windows.Forms.PTabPage();
            this.gbCustomerContactInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfOtherInfo = new Phoenix.Windows.Forms.PdfStandard();
            this.lblOtherInformation = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfContactName = new Phoenix.Windows.Forms.PdfStandard();
            this.lblContactName = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbContactMethod = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblContactMethod = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbRequestOwnershipInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfInitiatedBy = new Phoenix.Windows.Forms.PdfStandard();
            this.lblInitiatedBy = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbEmailOwner = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbOwner = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblOwner = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTabTitle2 = new Phoenix.Windows.Forms.PTabPage();
            this.pbGetRIM = new Phoenix.Windows.Forms.PAction();
            this.dfRimNo = new Phoenix.Windows.Forms.PdfStandard();
            this.dfProductAcctType = new Phoenix.Windows.Forms.PdfStandard();
            this.dfProductClassCode = new Phoenix.Windows.Forms.PdfStandard();
            this.dfPrioritySort = new Phoenix.Windows.Forms.PdfStandard();
            this.dfRecordType = new Phoenix.Windows.Forms.PdfStandard();
            this.dfPrevEmplId = new Phoenix.Windows.Forms.PdfStandard();
            this.dfQueueId = new Phoenix.Windows.Forms.PdfStandard();
            this.dfLastWorkDt = new Phoenix.Windows.Forms.PdfStandard();
            this.dfDueDateWithTime = new Phoenix.Windows.Forms.PdfStandard();
            this.pbRemove = new Phoenix.Windows.Forms.PAction();
            this.cbEmailGroup = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.picTabs.SuspendLayout();
            this.dfTabTitle0.SuspendLayout();
            this.gbRequestInformation.SuspendLayout();
            this.gbRequestStatusInformation.SuspendLayout();
            this.dfTabTitle1.SuspendLayout();
            this.gbCustomerContactInformation.SuspendLayout();
            this.gbRequestOwnershipInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbGetRIM,
            this.pbRemove});
            // 
            // picTabs
            // 
            this.picTabs.Controls.Add(this.dfTabTitle0);
            this.picTabs.Controls.Add(this.dfTabTitle1);
            this.picTabs.Controls.Add(this.dfTabTitle2);
            this.picTabs.Dock = System.Windows.Forms.DockStyle.None;
            this.picTabs.Location = new System.Drawing.Point(0, 0);
            this.picTabs.Name = "picTabs";
            this.picTabs.SelectedIndex = 0;
            this.picTabs.Size = new System.Drawing.Size(690, 448);
            this.picTabs.TabIndex = 0;
            this.picTabs.SelectedIndexChanged += new System.EventHandler(this.picTabs_SelectedIndexChanged);
            // 
            // dfTabTitle0
            // 
            this.dfTabTitle0.Controls.Add(this.gbRequestInformation);
            this.dfTabTitle0.Controls.Add(this.gbRequestStatusInformation);
            this.dfTabTitle0.Location = new System.Drawing.Point(4, 22);
            this.dfTabTitle0.MLInfo = controlInfo1;
            this.dfTabTitle0.Name = "dfTabTitle0";
            this.dfTabTitle0.Size = new System.Drawing.Size(682, 422);
            this.dfTabTitle0.TabIndex = 0;
            this.dfTabTitle0.Text = "&Basic Information";
            // 
            // gbRequestInformation
            // 
            this.gbRequestInformation.Controls.Add(this.lblAccount);
            this.gbRequestInformation.Controls.Add(this.cmbAccount);
            this.gbRequestInformation.Controls.Add(this.mulDetails);
            this.gbRequestInformation.Controls.Add(this.lblDetails);
            this.gbRequestInformation.Controls.Add(this.dfDescription);
            this.gbRequestInformation.Controls.Add(this.lblDescription);
            this.gbRequestInformation.Controls.Add(this.dfDueTime);
            this.gbRequestInformation.Controls.Add(this.lblTime);
            this.gbRequestInformation.Controls.Add(this.dfDueDate);
            this.gbRequestInformation.Controls.Add(this.lblDueDate);
            this.gbRequestInformation.Controls.Add(this.cmbQueue);
            this.gbRequestInformation.Controls.Add(this.lblQueue);
            this.gbRequestInformation.Controls.Add(this.cmbCategory);
            this.gbRequestInformation.Controls.Add(this.lblCategory);
            this.gbRequestInformation.Controls.Add(this.dfCustomer);
            this.gbRequestInformation.Controls.Add(this.lblCustomer);
            this.gbRequestInformation.Controls.Add(this.dfRequestId);
            this.gbRequestInformation.Controls.Add(this.lblRequestID);
            this.gbRequestInformation.Location = new System.Drawing.Point(0, 0);
            this.gbRequestInformation.Name = "gbRequestInformation";
            this.gbRequestInformation.PhoenixUIControl.ObjectId = 1;
            this.gbRequestInformation.Size = new System.Drawing.Size(680, 192);
            this.gbRequestInformation.TabIndex = 0;
            this.gbRequestInformation.TabStop = false;
            this.gbRequestInformation.Text = "Request Information";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoEllipsis = true;
            this.lblAccount.Location = new System.Drawing.Point(424, 40);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.PhoenixUIControl.ObjectId = 4;
            this.lblAccount.Size = new System.Drawing.Size(60, 20);
            this.lblAccount.TabIndex = 4;
            this.lblAccount.Text = "Account:";
            // 
            // cmbAccount
            // 
            this.cmbAccount.Location = new System.Drawing.Point(488, 40);
            this.cmbAccount.Name = "cmbAccount";
            this.cmbAccount.PhoenixUIControl.ObjectId = 4;
            this.cmbAccount.Size = new System.Drawing.Size(188, 21);
            this.cmbAccount.TabIndex = 5;
            this.cmbAccount.Value = null;
            // 
            // mulDetails
            // 
            this.mulDetails.AcceptsReturn = true;
            this.mulDetails.Location = new System.Drawing.Point(96, 136);
            this.mulDetails.MaxLength = 254;
            this.mulDetails.Multiline = true;
            this.mulDetails.Name = "mulDetails";
            this.mulDetails.PhoenixUIControl.ObjectId = 8;
            this.mulDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mulDetails.Size = new System.Drawing.Size(580, 52);
            this.mulDetails.TabIndex = 17;
            this.mulDetails.PhoenixUIEnterEvent += new Phoenix.Windows.Forms.EnterEventHandler(this.mulDetails_PhoenixUIEnterEvent);
            this.mulDetails.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.mulDetails_PhoenixUILeaveEvent);
            // 
            // lblDetails
            // 
            this.lblDetails.AutoEllipsis = true;
            this.lblDetails.Location = new System.Drawing.Point(8, 136);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.PhoenixUIControl.ObjectId = 8;
            this.lblDetails.Size = new System.Drawing.Size(70, 20);
            this.lblDetails.TabIndex = 16;
            this.lblDetails.Text = "Details:";
            // 
            // dfDescription
            // 
            this.dfDescription.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.Location = new System.Drawing.Point(96, 112);
            this.dfDescription.MaxLength = 80;
            this.dfDescription.Name = "dfDescription";
            this.dfDescription.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.PhoenixUIControl.ObjectId = 7;
            this.dfDescription.Size = new System.Drawing.Size(366, 20);
            this.dfDescription.TabIndex = 15;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 112);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 7;
            this.lblDescription.Size = new System.Drawing.Size(70, 20);
            this.lblDescription.TabIndex = 14;
            this.lblDescription.Text = "Description:";
            // 
            // dfDueTime
            // 
            this.dfDueTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.Location = new System.Drawing.Point(328, 88);
            this.dfDueTime.Name = "dfDueTime";
            this.dfDueTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfDueTime.PhoenixUIControl.ObjectId = 51;
            this.dfDueTime.Size = new System.Drawing.Size(72, 20);
            this.dfDueTime.TabIndex = 13;
            // 
            // lblTime
            // 
            this.lblTime.AutoEllipsis = true;
            this.lblTime.Location = new System.Drawing.Point(284, 88);
            this.lblTime.Name = "lblTime";
            this.lblTime.PhoenixUIControl.ObjectId = 51;
            this.lblTime.Size = new System.Drawing.Size(38, 20);
            this.lblTime.TabIndex = 12;
            this.lblTime.Text = "Time:";
            // 
            // dfDueDate
            // 
            this.dfDueDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDate.Location = new System.Drawing.Point(96, 88);
            this.dfDueDate.Name = "dfDueDate";
            this.dfDueDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfDueDate.PhoenixUIControl.ObjectId = 11;
            this.dfDueDate.Size = new System.Drawing.Size(84, 20);
            this.dfDueDate.TabIndex = 11;
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoEllipsis = true;
            this.lblDueDate.Location = new System.Drawing.Point(8, 88);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.Enable;
            this.lblDueDate.PhoenixUIControl.ObjectId = 11;
            this.lblDueDate.Size = new System.Drawing.Size(59, 20);
            this.lblDueDate.TabIndex = 10;
            this.lblDueDate.Text = "Due Date:";
            // 
            // cmbQueue
            // 
            this.cmbQueue.Location = new System.Drawing.Point(96, 64);
            this.cmbQueue.Name = "cmbQueue";
            this.cmbQueue.PhoenixUIControl.ObjectId = 52;
            this.cmbQueue.PhoenixUIControl.XmlTag = "QueueId";
            this.cmbQueue.Size = new System.Drawing.Size(302, 21);
            this.cmbQueue.TabIndex = 7;
            this.cmbQueue.Value = null;
            this.cmbQueue.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbQueue_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblQueue
            // 
            this.lblQueue.AutoEllipsis = true;
            this.lblQueue.Location = new System.Drawing.Point(8, 64);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.PhoenixUIControl.ObjectId = 52;
            this.lblQueue.Size = new System.Drawing.Size(74, 20);
            this.lblQueue.TabIndex = 6;
            this.lblQueue.Text = "Queue:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(488, 64);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.PhoenixUIControl.ObjectId = 5;
            this.cmbCategory.PhoenixUIControl.XmlTag = "CategoryId";
            this.cmbCategory.Size = new System.Drawing.Size(188, 21);
            this.cmbCategory.TabIndex = 9;
            this.cmbCategory.Value = null;
            this.cmbCategory.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbCategory_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoEllipsis = true;
            this.lblCategory.Location = new System.Drawing.Point(424, 64);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.PhoenixUIControl.ObjectId = 5;
            this.lblCategory.Size = new System.Drawing.Size(60, 20);
            this.lblCategory.TabIndex = 8;
            this.lblCategory.Text = "Category:";
            // 
            // dfCustomer
            // 
            this.dfCustomer.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.Location = new System.Drawing.Point(96, 40);
            this.dfCustomer.MaxLength = 80;
            this.dfCustomer.Name = "dfCustomer";
            this.dfCustomer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.PhoenixUIControl.ObjectId = 3;
            this.dfCustomer.Size = new System.Drawing.Size(302, 20);
            this.dfCustomer.TabIndex = 3;
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoEllipsis = true;
            this.lblCustomer.Location = new System.Drawing.Point(8, 40);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.PhoenixUIControl.ObjectId = 3;
            this.lblCustomer.Size = new System.Drawing.Size(78, 20);
            this.lblCustomer.TabIndex = 2;
            this.lblCustomer.Text = "Customer:";
            // 
            // dfRequestId
            // 
            this.dfRequestId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfRequestId.Location = new System.Drawing.Point(96, 16);
            this.dfRequestId.Name = "dfRequestId";
            this.dfRequestId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfRequestId.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfRequestId.PhoenixUIControl.ObjectId = 2;
            this.dfRequestId.Size = new System.Drawing.Size(74, 20);
            this.dfRequestId.TabIndex = 1;
            // 
            // lblRequestID
            // 
            this.lblRequestID.AutoEllipsis = true;
            this.lblRequestID.Location = new System.Drawing.Point(8, 16);
            this.lblRequestID.Name = "lblRequestID";
            this.lblRequestID.PhoenixUIControl.ObjectId = 2;
            this.lblRequestID.Size = new System.Drawing.Size(80, 20);
            this.lblRequestID.TabIndex = 0;
            this.lblRequestID.Text = "Request ID:";
            // 
            // gbRequestStatusInformation
            // 
            this.gbRequestStatusInformation.Controls.Add(this.cmbPriority);
            this.gbRequestStatusInformation.Controls.Add(this.lblPriority);
            this.gbRequestStatusInformation.Controls.Add(this.rbCompleted);
            this.gbRequestStatusInformation.Controls.Add(this.rbInProcess);
            this.gbRequestStatusInformation.Controls.Add(this.rbPending);
            this.gbRequestStatusInformation.Controls.Add(this.lblRequestStatus);
            this.gbRequestStatusInformation.Location = new System.Drawing.Point(0, 192);
            this.gbRequestStatusInformation.Name = "gbRequestStatusInformation";
            this.gbRequestStatusInformation.PhoenixUIControl.ObjectId = 19;
            this.gbRequestStatusInformation.Size = new System.Drawing.Size(680, 44);
            this.gbRequestStatusInformation.TabIndex = 1;
            this.gbRequestStatusInformation.TabStop = false;
            this.gbRequestStatusInformation.Text = "Request Status Information";
            // 
            // cmbPriority
            // 
            this.cmbPriority.Location = new System.Drawing.Point(564, 16);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.PhoenixUIControl.ObjectId = 40;
            this.cmbPriority.Size = new System.Drawing.Size(113, 21);
            this.cmbPriority.TabIndex = 5;
            this.cmbPriority.Value = null;
            this.cmbPriority.SelectedIndexChanged += new System.EventHandler(this.cmbPriority_SelectedIndexChanged);
            // 
            // lblPriority
            // 
            this.lblPriority.AutoEllipsis = true;
            this.lblPriority.Location = new System.Drawing.Point(508, 16);
            this.lblPriority.Name = "lblPriority";
            this.lblPriority.PhoenixUIControl.ObjectId = 40;
            this.lblPriority.Size = new System.Drawing.Size(48, 20);
            this.lblPriority.TabIndex = 4;
            this.lblPriority.Text = "Priority:";
            // 
            // rbCompleted
            // 
            this.rbCompleted.BackColor = System.Drawing.SystemColors.Control;
            this.rbCompleted.Description = null;
            this.rbCompleted.Location = new System.Drawing.Point(256, 16);
            this.rbCompleted.Name = "rbCompleted";
            this.rbCompleted.PhoenixUIControl.ObjectId = 49;
            this.rbCompleted.Size = new System.Drawing.Size(80, 20);
            this.rbCompleted.TabIndex = 3;
            this.rbCompleted.Text = "Completed";
            this.rbCompleted.UseVisualStyleBackColor = false;
            // 
            // rbInProcess
            // 
            this.rbInProcess.BackColor = System.Drawing.SystemColors.Control;
            this.rbInProcess.Description = null;
            this.rbInProcess.Location = new System.Drawing.Point(172, 16);
            this.rbInProcess.Name = "rbInProcess";
            this.rbInProcess.PhoenixUIControl.ObjectId = 48;
            this.rbInProcess.Size = new System.Drawing.Size(78, 20);
            this.rbInProcess.TabIndex = 2;
            this.rbInProcess.Text = "In Process";
            this.rbInProcess.UseVisualStyleBackColor = false;
            // 
            // rbPending
            // 
            this.rbPending.BackColor = System.Drawing.SystemColors.Control;
            this.rbPending.Description = null;
            this.rbPending.IsMaster = true;
            this.rbPending.Location = new System.Drawing.Point(96, 16);
            this.rbPending.Name = "rbPending";
            this.rbPending.PhoenixUIControl.ObjectId = 47;
            this.rbPending.Size = new System.Drawing.Size(70, 20);
            this.rbPending.TabIndex = 1;
            this.rbPending.Text = "Pending";
            this.rbPending.UseVisualStyleBackColor = false;
            // 
            // lblRequestStatus
            // 
            this.lblRequestStatus.AutoEllipsis = true;
            this.lblRequestStatus.Location = new System.Drawing.Point(8, 16);
            this.lblRequestStatus.Name = "lblRequestStatus";
            this.lblRequestStatus.PhoenixUIControl.ObjectId = 39;
            this.lblRequestStatus.Size = new System.Drawing.Size(86, 20);
            this.lblRequestStatus.TabIndex = 0;
            this.lblRequestStatus.Text = "Request Status:";
            // 
            // dfTabTitle1
            // 
            this.dfTabTitle1.Controls.Add(this.gbCustomerContactInformation);
            this.dfTabTitle1.Controls.Add(this.gbRequestOwnershipInformation);
            this.dfTabTitle1.Location = new System.Drawing.Point(4, 22);
            this.dfTabTitle1.MLInfo = controlInfo2;
            this.dfTabTitle1.Name = "dfTabTitle1";
            this.dfTabTitle1.Size = new System.Drawing.Size(682, 422);
            this.dfTabTitle1.TabIndex = 1;
            this.dfTabTitle1.Text = "C&ontact, Owner && Status Information";
            // 
            // gbCustomerContactInformation
            // 
            this.gbCustomerContactInformation.Controls.Add(this.dfOtherInfo);
            this.gbCustomerContactInformation.Controls.Add(this.lblOtherInformation);
            this.gbCustomerContactInformation.Controls.Add(this.dfContactName);
            this.gbCustomerContactInformation.Controls.Add(this.lblContactName);
            this.gbCustomerContactInformation.Controls.Add(this.cmbContactMethod);
            this.gbCustomerContactInformation.Controls.Add(this.lblContactMethod);
            this.gbCustomerContactInformation.Location = new System.Drawing.Point(0, 0);
            this.gbCustomerContactInformation.Name = "gbCustomerContactInformation";
            this.gbCustomerContactInformation.PhoenixUIControl.ObjectId = 12;
            this.gbCustomerContactInformation.Size = new System.Drawing.Size(680, 92);
            this.gbCustomerContactInformation.TabIndex = 0;
            this.gbCustomerContactInformation.TabStop = false;
            this.gbCustomerContactInformation.Text = "Customer Contact Information";
            // 
            // dfOtherInfo
            // 
            this.dfOtherInfo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOtherInfo.Location = new System.Drawing.Point(108, 66);
            this.dfOtherInfo.MaxLength = 80;
            this.dfOtherInfo.Name = "dfOtherInfo";
            this.dfOtherInfo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOtherInfo.PhoenixUIControl.ObjectId = 15;
            this.dfOtherInfo.Size = new System.Drawing.Size(424, 20);
            this.dfOtherInfo.TabIndex = 5;
            // 
            // lblOtherInformation
            // 
            this.lblOtherInformation.AutoEllipsis = true;
            this.lblOtherInformation.Location = new System.Drawing.Point(6, 66);
            this.lblOtherInformation.Name = "lblOtherInformation";
            this.lblOtherInformation.PhoenixUIControl.ObjectId = 15;
            this.lblOtherInformation.Size = new System.Drawing.Size(98, 20);
            this.lblOtherInformation.TabIndex = 4;
            this.lblOtherInformation.Text = "Other Information:";
            // 
            // dfContactName
            // 
            this.dfContactName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.Location = new System.Drawing.Point(108, 42);
            this.dfContactName.MaxLength = 80;
            this.dfContactName.Name = "dfContactName";
            this.dfContactName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.PhoenixUIControl.ObjectId = 14;
            this.dfContactName.Size = new System.Drawing.Size(424, 20);
            this.dfContactName.TabIndex = 3;
            // 
            // lblContactName
            // 
            this.lblContactName.AutoEllipsis = true;
            this.lblContactName.Location = new System.Drawing.Point(6, 42);
            this.lblContactName.Name = "lblContactName";
            this.lblContactName.PhoenixUIControl.ObjectId = 14;
            this.lblContactName.Size = new System.Drawing.Size(84, 20);
            this.lblContactName.TabIndex = 2;
            this.lblContactName.Text = "Contact Name:";
            // 
            // cmbContactMethod
            // 
            this.cmbContactMethod.Location = new System.Drawing.Point(108, 18);
            this.cmbContactMethod.Name = "cmbContactMethod";
            this.cmbContactMethod.PhoenixUIControl.ObjectId = 13;
            this.cmbContactMethod.Size = new System.Drawing.Size(424, 21);
            this.cmbContactMethod.TabIndex = 1;
            this.cmbContactMethod.Value = null;
            // 
            // lblContactMethod
            // 
            this.lblContactMethod.AutoEllipsis = true;
            this.lblContactMethod.Location = new System.Drawing.Point(6, 18);
            this.lblContactMethod.Name = "lblContactMethod";
            this.lblContactMethod.PhoenixUIControl.ObjectId = 13;
            this.lblContactMethod.Size = new System.Drawing.Size(89, 20);
            this.lblContactMethod.TabIndex = 0;
            this.lblContactMethod.Text = "Contact Method:";
            // 
            // gbRequestOwnershipInformation
            // 
            this.gbRequestOwnershipInformation.Controls.Add(this.cbEmailGroup);
            this.gbRequestOwnershipInformation.Controls.Add(this.dfInitiatedBy);
            this.gbRequestOwnershipInformation.Controls.Add(this.lblInitiatedBy);
            this.gbRequestOwnershipInformation.Controls.Add(this.cbEmailOwner);
            this.gbRequestOwnershipInformation.Controls.Add(this.cmbOwner);
            this.gbRequestOwnershipInformation.Controls.Add(this.lblOwner);
            this.gbRequestOwnershipInformation.Location = new System.Drawing.Point(0, 92);
            this.gbRequestOwnershipInformation.Name = "gbRequestOwnershipInformation";
            this.gbRequestOwnershipInformation.PhoenixUIControl.ObjectId = 16;
            this.gbRequestOwnershipInformation.Size = new System.Drawing.Size(680, 68);
            this.gbRequestOwnershipInformation.TabIndex = 1;
            this.gbRequestOwnershipInformation.TabStop = false;
            this.gbRequestOwnershipInformation.Text = "Request Ownership Information";
            // 
            // dfInitiatedBy
            // 
            this.dfInitiatedBy.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfInitiatedBy.Location = new System.Drawing.Point(108, 42);
            this.dfInitiatedBy.MaxLength = 40;
            this.dfInitiatedBy.Name = "dfInitiatedBy";
            this.dfInitiatedBy.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfInitiatedBy.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfInitiatedBy.PhoenixUIControl.ObjectId = 18;
            this.dfInitiatedBy.Size = new System.Drawing.Size(257, 20);
            this.dfInitiatedBy.TabIndex = 4;
            // 
            // lblInitiatedBy
            // 
            this.lblInitiatedBy.AutoEllipsis = true;
            this.lblInitiatedBy.Location = new System.Drawing.Point(6, 42);
            this.lblInitiatedBy.Name = "lblInitiatedBy";
            this.lblInitiatedBy.PhoenixUIControl.ObjectId = 18;
            this.lblInitiatedBy.Size = new System.Drawing.Size(84, 20);
            this.lblInitiatedBy.TabIndex = 3;
            this.lblInitiatedBy.Text = "Initiated By:";
            // 
            // cbEmailOwner
            // 
            this.cbEmailOwner.BackColor = System.Drawing.SystemColors.Control;
            this.cbEmailOwner.Checked = true;
            this.cbEmailOwner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEmailOwner.Location = new System.Drawing.Point(463, 18);
            this.cbEmailOwner.Name = "cbEmailOwner";
            this.cbEmailOwner.PhoenixUIControl.ObjectId = 17;
            this.cbEmailOwner.PhoenixUIControl.XmlTag = "EmailOwner";
            this.cbEmailOwner.Size = new System.Drawing.Size(139, 20);
            this.cbEmailOwner.TabIndex = 2;
            this.cbEmailOwner.Text = "Notify Owner via E-Mail";
            this.cbEmailOwner.UseVisualStyleBackColor = false;
            this.cbEmailOwner.Value = null;
            // 
            // cmbOwner
            // 
            this.cmbOwner.Location = new System.Drawing.Point(108, 18);
            this.cmbOwner.Name = "cmbOwner";
            this.cmbOwner.PhoenixUIControl.ObjectId = 38;
            this.cmbOwner.Size = new System.Drawing.Size(257, 21);
            this.cmbOwner.TabIndex = 1;
            this.cmbOwner.Value = null;
            this.cmbOwner.SelectedIndexChanged += new System.EventHandler(this.cmbOwner_SelectedIndexChanged);
            // 
            // lblOwner
            // 
            this.lblOwner.AutoEllipsis = true;
            this.lblOwner.Location = new System.Drawing.Point(6, 18);
            this.lblOwner.Name = "lblOwner";
            this.lblOwner.PhoenixUIControl.ObjectId = 38;
            this.lblOwner.Size = new System.Drawing.Size(82, 20);
            this.lblOwner.TabIndex = 0;
            this.lblOwner.Text = "Owner:";
            // 
            // dfTabTitle2
            // 
            this.dfTabTitle2.Location = new System.Drawing.Point(4, 22);
            this.dfTabTitle2.MLInfo = controlInfo3;
            this.dfTabTitle2.Name = "dfTabTitle2";
            this.dfTabTitle2.Size = new System.Drawing.Size(682, 422);
            this.dfTabTitle2.TabIndex = 2;
            this.dfTabTitle2.Text = "Additional Information";
            this.dfTabTitle2.UseVisualStyleBackColor = true;
            // 
            // pbGetRIM
            // 
            this.pbGetRIM.LongText = "pbGetRIM";
            this.pbGetRIM.Name = "pbGetRIM";
            this.pbGetRIM.ObjectId = 45;
            this.pbGetRIM.ShortText = "pbGetRIM";
            this.pbGetRIM.Tag = null;
            this.pbGetRIM.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbGetRIM_Click);
            // 
            // dfRimNo
            // 
            this.dfRimNo.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRimNo.Location = new System.Drawing.Point(653, 4);
            this.dfRimNo.Name = "dfRimNo";
            this.dfRimNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfRimNo.Size = new System.Drawing.Size(100, 20);
            this.dfRimNo.TabIndex = 0;
            this.dfRimNo.Visible = false;
            // 
            // dfProductAcctType
            // 
            this.dfProductAcctType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfProductAcctType.Location = new System.Drawing.Point(653, 4);
            this.dfProductAcctType.Name = "dfProductAcctType";
            this.dfProductAcctType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfProductAcctType.Size = new System.Drawing.Size(100, 20);
            this.dfProductAcctType.TabIndex = 0;
            this.dfProductAcctType.Visible = false;
            // 
            // dfProductClassCode
            // 
            this.dfProductClassCode.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfProductClassCode.Location = new System.Drawing.Point(653, 4);
            this.dfProductClassCode.Name = "dfProductClassCode";
            this.dfProductClassCode.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfProductClassCode.Size = new System.Drawing.Size(100, 20);
            this.dfProductClassCode.TabIndex = 0;
            this.dfProductClassCode.Visible = false;
            // 
            // dfPrioritySort
            // 
            this.dfPrioritySort.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPrioritySort.Location = new System.Drawing.Point(653, 4);
            this.dfPrioritySort.Name = "dfPrioritySort";
            this.dfPrioritySort.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPrioritySort.Size = new System.Drawing.Size(100, 20);
            this.dfPrioritySort.TabIndex = 0;
            this.dfPrioritySort.Visible = false;
            // 
            // dfRecordType
            // 
            this.dfRecordType.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfRecordType.Location = new System.Drawing.Point(653, 4);
            this.dfRecordType.Name = "dfRecordType";
            this.dfRecordType.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfRecordType.Size = new System.Drawing.Size(100, 20);
            this.dfRecordType.TabIndex = 0;
            this.dfRecordType.Visible = false;
            // 
            // dfPrevEmplId
            // 
            this.dfPrevEmplId.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPrevEmplId.Location = new System.Drawing.Point(653, 4);
            this.dfPrevEmplId.Name = "dfPrevEmplId";
            this.dfPrevEmplId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfPrevEmplId.Size = new System.Drawing.Size(100, 20);
            this.dfPrevEmplId.TabIndex = 0;
            this.dfPrevEmplId.Visible = false;
            // 
            // dfQueueId
            // 
            this.dfQueueId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfQueueId.Location = new System.Drawing.Point(653, 4);
            this.dfQueueId.Name = "dfQueueId";
            this.dfQueueId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfQueueId.Size = new System.Drawing.Size(100, 20);
            this.dfQueueId.TabIndex = 0;
            this.dfQueueId.Visible = false;
            // 
            // dfLastWorkDt
            // 
            this.dfLastWorkDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfLastWorkDt.Location = new System.Drawing.Point(653, 4);
            this.dfLastWorkDt.Name = "dfLastWorkDt";
            this.dfLastWorkDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfLastWorkDt.Size = new System.Drawing.Size(100, 20);
            this.dfLastWorkDt.TabIndex = 0;
            this.dfLastWorkDt.Visible = false;
            // 
            // dfDueDateWithTime
            // 
            this.dfDueDateWithTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDateWithTime.Location = new System.Drawing.Point(653, 4);
            this.dfDueDateWithTime.Name = "dfDueDateWithTime";
            this.dfDueDateWithTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDateWithTime.Size = new System.Drawing.Size(52, 20);
            this.dfDueDateWithTime.TabIndex = 0;
            this.dfDueDateWithTime.Visible = false;
            // 
            // pbRemove
            // 
            this.pbRemove.LongText = "&Remove";
            this.pbRemove.Name = "pbRemove";
            this.pbRemove.ObjectId = 54;
            this.pbRemove.ShortText = "&Remove";
            this.pbRemove.Tag = null;
            this.pbRemove.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRemove_Click);
            // 
            // cbEmailGroup
            // 
            this.cbEmailGroup.AutoSize = true;
            this.cbEmailGroup.BackColor = System.Drawing.SystemColors.Control;
            this.cbEmailGroup.Location = new System.Drawing.Point(464, 44);
            this.cbEmailGroup.Name = "cbEmailGroup";
            this.cbEmailGroup.PhoenixUIControl.ObjectId = 55;
            this.cbEmailGroup.Size = new System.Drawing.Size(197, 18);
            this.cbEmailGroup.TabIndex = 5;
            this.cbEmailGroup.Text = "Auto E-Mail All Queue Group Users";
            this.cbEmailGroup.UseVisualStyleBackColor = false;
            this.cbEmailGroup.Value = null;
            // 
            // frmRequestEdit
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.picTabs);
            this.Name = "frmRequestEdit";
            this.ScreenId = 12504;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmRequestEdit_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmRequestEdit_PInitCompleteEvent);
            this.PBeforeSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmRequestEdit_PBeforeSave);
            this.PAfterSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmRequestEdit_PAfterSave);
            this.PShowCompletedEvent += new System.EventHandler(this.frmRequestEdit_PShowCompletedEvent);
            this.PAfterActionEvent += new Phoenix.Windows.Forms.FormActionHandler(this.frmRequestEdit_PAfterActionEvent);//Task #74463
            this.picTabs.ResumeLayout(false);
            this.dfTabTitle0.ResumeLayout(false);
            this.gbRequestInformation.ResumeLayout(false);
            this.gbRequestInformation.PerformLayout();
            this.gbRequestStatusInformation.ResumeLayout(false);
            this.dfTabTitle1.ResumeLayout(false);
            this.gbCustomerContactInformation.ResumeLayout(false);
            this.gbCustomerContactInformation.PerformLayout();
            this.gbRequestOwnershipInformation.ResumeLayout(false);
            this.gbRequestOwnershipInformation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #region Init
        public override void InitParameters(params Object[] paramList)
        {
            if (paramList.Length == 7)
            {
                //_parentForm = (PfwStandard)paramList[0];
                _parentForm = paramList[0] as PfwStandard;

                this._gbWorkQueue.Type.Value = paramList[1].ToString().Trim();

                if (paramList[2] != null)
                    this._gbWorkQueue.WorkId.Value = Convert.ToInt32(paramList[2]);
                if (paramList[3] != null)
                    this._gbWorkQueue.RimNo.Value = Convert.ToInt32(paramList[3]);
                if (paramList[4] != null)
                    this._gbWorkQueue.CustomerName.Value = Convert.ToString(paramList[4]);
                if (paramList[5] != null)
                {
                    this._gbWorkQueue.AcctType.Value = Convert.ToString(paramList[5]);
                    _reqAcctType.Value = Convert.ToString(paramList[5]);

                }
                if (paramList[6] != null)
                {
                    this._gbWorkQueue.AcctNo.Value = paramList[6].ToString();
                    _reqAcctNo.Value = paramList[6].ToString();
                }

                if (Phoenix.Shared.Variables.GlobalVars.Module == "Teller")
                {
                    this.ScreenId = 12504;

                }
                else if (Phoenix.Shared.Variables.GlobalVars.Module == "AcProc")
                {
                    this.ScreenId = Phoenix.Shared.Constants.ScreenId.RequestEdit - Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset;
                }
                this._gbWorkQueue.OutputType.Value = 12504;
            }

            if (this._gbWorkQueue.WorkId.IsNull)
            {
                this.IsNew = true;
                this.wasFormNew = true;
            }
            else
            {
                this.IsNew = false;
                this.wasFormNew = false;
            }
            #region 6139
            if (_parentForm == null)
            {
                _parentForm = new PfwStandard();
                _parentForm.Name = string.Empty;
            }
            #endregion
            InitXmlTags();
            base.InitParameters(paramList); //#19058
        }
        private void InitXmlTags()
        {
            //TODO: Verify the XmlTag Mapping	
            //this.dfRequestID.PhoenixUIControl.XmlTag = "WorkId";
            this.dfCustomer.PhoenixUIControl.XmlTag = "CustomerName";
            this.cmbQueue.PhoenixUIControl.XmlTag = "QueueId";
            this.cmbCategory.PhoenixUIControl.XmlTag = "CategoryId";
            this.dfDescription.PhoenixUIControl.XmlTag = "Description";
            this.mulDetails.PhoenixUIControl.XmlTag = "Text1";
            this.cmbContactMethod.PhoenixUIControl.XmlTag = "ContactMethod";
            this.dfContactName.PhoenixUIControl.XmlTag = "ContactName";
            this.dfOtherInfo.PhoenixUIControl.XmlTag = "OtherInfo";
            this.cmbOwner.PhoenixUIControl.XmlTag = "OwnerEmplId";
            this.cbEmailOwner.PhoenixUIControl.XmlTag = "EmailOwner";
            this.rbPending.PhoenixUIControl.XmlTag = "WorkStatus";
            this.cmbPriority.PhoenixUIControl.XmlTag = "Priority";
            this.dfRimNo.PhoenixUIControl.XmlTag = "RimNo";
            this.dfPrioritySort.PhoenixUIControl.XmlTag = "PrioritySort";
            this.dfRecordType.PhoenixUIControl.XmlTag = "Type";
            this.dfPrevEmplId.PhoenixUIControl.XmlTag = "PrevOwnerEmplId";
            this.dfQueueId.PhoenixUIControl.XmlTag = "QueueId";
            this.dfLastWorkDt.PhoenixUIControl.XmlTag = "LastWorkDt";
            this.dfDueDateWithTime.PhoenixUIControl.XmlTag = "DueDt";

        }

        #endregion

        #region Window Events

        #region Window Begin/Complete Events
        private Phoenix.Windows.Forms.ReturnType frmRequestEdit_PInitBeginEvent()
        {
            this.MainBusinesObject = _gbWorkQueue;
            _gbWorkQueue.ResponseTypeId = 14;

            /*Begin #140811 */
            _adWqCategory = new Phoenix.BusObj.Admin.Misc.AdWqCategory();
            _adWqCategory.SelectAllFields = false;
            _adWqCategory.AutoEmailGroup.Selected = true;
            if (IsNew)
            {
                /* Disabling Additional tab */
                EnableTab(dfTabTitle2, false);
            }
            /* End #140811 */

            //#6683
            if (BusGlobalVars.Module == "Teller")
            {
                int screenId = ScreenId > Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset ? ScreenId - Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset : ScreenId;
                if (CoreService.UIAccessProvider.GetScreenAccess(screenId) == AuthorizationType.NoDefinition)
                {
                    // If the screen security is not defined or coded in the Admin Tree then do not force security on Save button
                    this.ActionSave.NextScreenId = 0;
                }
            }
            /* Constraint move from server side of bus obj, WI-15234 */
            _gbWorkQueue.WorkStatus.Constraint = new Phoenix.FrameWork.BusFrame.Constraint(43088, true);

            //Begin #34896
            if (Workspace is PwksWindow && (Workspace as PwksWindow).IsHighResWorkspace)
            {
                (this.Extension as Phoenix.Shared.Windows.FormExtension).ResizeFormEnd += new EventHandler(frmRequestEdit_ResizeFormEnd);
            }
            //End 34896
            return ReturnType.Success;

        }

        void frmRequestEdit_ResizeFormEnd(object sender, EventArgs e)
        {
            ResizeEmbeddedWinCntrls();
        }

        private void ResizeEmbeddedWinCntrls()
        {
            if (Workspace == null)
                return;

            if (frmUserDefFieldsEdit != null && Workspace is PwksWindow && (Workspace as PwksWindow).IsHighResWorkspace)
            {
                Phoenix.Shared.Windows.FormExtension formExt = Extension as Phoenix.Shared.Windows.FormExtension;

                if (formExt == null)
                    return;

                frmUserDefFieldsEdit.grdValues.Height = formExt.WksAvailableHeight - 40;
                frmUserDefFieldsEdit.grdValues.Width = formExt.WksAvailableWidth - 16;
            }
        }

        private void frmRequestEdit_PInitCompleteEvent()
        {
            PopulateDefaultCombos();

            if (this.IsNew)
            {
                this._gbWorkQueue.CanChangeOwner.Value = "Y";

                if (!_reqAcctType.IsNull)
                {
                    this.cmbAccount.SetValueAndSelect(this._reqAcctType.Value.Trim() + " - " + this._reqAcctNo.Value.Trim(), true);

                }

            }
            else
            {
                this._gbWorkQueue.CanChangeOwner.Value = this._gbWorkQueue.CheckCanChgOwner();  //Load the canchangeowner property in busobj
                                                                                                //Set the key.
                this.dfRequestId.UnFormattedValue = this._gbWorkQueue.WorkId.Value;
                if (!this._gbWorkQueue.Account.IsNull)
                {
                    this.cmbAccount.SetValueAndSelect(this._gbWorkQueue.Account.Value, true);
                }
            }

            LocCheckPMA();

            //#06057//if (!this._gbWorkQueue.RimNo.IsNull)
            if (!this._gbWorkQueue.RimNo.IsNull && _gbWorkQueue.RimNo.Value != 0)
                this.dfCustomer.UnFormattedValue = this._gbWorkQueue.GetCustomerName(this._gbWorkQueue.RimNo.Value);

            if (this.IsNew)
            {
                this.dfInitiatedBy.UnFormattedValue = Phoenix.FrameWork.Shared.Variables.GlobalVars.EmployeeName;

                if (!this._gbWorkQueue.CustomerName.IsNull)
                {
                    this.dfContactName.UnFormattedValue = this._gbWorkQueue.CustomerName.Value.Trim();
                }

                this.cmbPriority.SetValueAndSelect("Medium", true);
            }
            else
            {
                if (!this._gbWorkQueue.DueDt.IsNull)
                {
                    this.dfDueDate.UnFormattedValue = Convert.ToDateTime(this._gbWorkQueue.DueDt.Value);
                    this.dfDueTime.UnFormattedValue = Convert.ToDateTime(this._gbWorkQueue.DueDt.Value);
                }

                this.dfInitiatedBy.UnFormattedValue = this._gbHelper.GetEmployeeName(this._gbWorkQueue.EmplId.Value);

                /*Begin #140811  - 
                 * In Edit mode, group email checkbox is checked if there is no owner*/
                if (this._gbWorkQueue.OwnerEmplId.IsNull)
                {
                    cbEmailGroup.Checked = true;
                }
                else
                {
                    cbEmailGroup.Checked = false;
                }
                /*End #140811 */
            }

            this._gbWorkQueue.DecodePriority(this.cmbPriority.Text);

            #region #77295-aHussein
            //! Begin #77295
            //Call SalSendMsg(cmbQueue, PAM_Click, 0, 0)
            //Set cmbCategory.nCodeValue = dfCategory
            //Call SalSendMsg( cmbCategory, PM_Decode, 0, 0 )
            //! End #77295 
            //cmbQueue_SelectedIndexChanged(null, null);
            //cmbCategory.SetValueAndSelect(_gbWorkQueue.CategoryId.Value, true);
            #endregion

            EnableDisableVisibleLogic("Initialize");
        }

        #region #01698
        void frmRequestEdit_PShowCompletedEvent(object sender, EventArgs e)
        {
            cmbAccount.Focus();
        }
        #endregion
        #endregion

        #region Tab Events
        private void picTabs_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            SetTabAssociates(picTabs.SelectedIndex);
        }

        #endregion

        #region Combo Box Events
        private void cmbPriority_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this._gbWorkQueue.DecodePriority(this.cmbPriority.Text);
        }

        /*private void cmbCategory_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopOwnerNames();
			int nDefCatEmplId = Convert.ToInt32(this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatEmplId));

            if (nDefCatEmplId > 0)
			{
				this.cmbOwner.SetValueAndSelect(nDefCatEmplId, true);
				LocCheckPMA();
			}
            LocCanChangeOwner();//#77295-aHussein
		}*/
        #region #77295-aHussein
        private void LocCanChangeOwner()
        {
            if (IsNew)
            {
                cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
            }
            else
            {
                string sChangeOwner = _gbWorkQueue.CheckCanChgOwner();

                if (sChangeOwner.Trim() == Phoenix.Shared.Variables.GlobalVars.Instance.ML.Y)
                {
                    cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                }
                else
                {
                    cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                }

            }

        }
        #endregion

        private void cmbOwner_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.cmbOwner.ScreenToObject();
            LocCheckPMA();
        }

        /*private void cmbQueue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Phoenix.Windows.Client.Helper.PopulateCombo(cmbCategory, _gbWorkQueue, _gbWorkQueue.CategoryId);
        }*/

        #endregion
        //Begin Issue #73762
        private void mulDetails_PhoenixUIEnterEvent(object sender, EventArgs e)
        {
            this.DefaultAction = null;
        }
        private void mulDetails_PhoenixUILeaveEvent(object sender, EventArgs e)
        {
            this.DefaultAction = this.ActionSave;
        }//end Issue#73762

        #region Checkbox Events
        #endregion

        #region Pushbutton Events
        public override bool OnActionClose()
        {
            /* Begin #140811 */
            if (frmUserDefFieldsEdit != null)
            {
                /* Begin #104392 */
                this.frmUserDefFieldsEdit.IsFormClose = true;
                /* End #104392 */
                bDirtyUserDefVal = this.frmUserDefFieldsEdit.IsFormDirty();
            }
            else
            {
                bDirtyUserDefVal = false;
            }
            /* End #140811 */
            if (base.OnActionClose())
            {
                PfwStandard parentForm = this.Workspace.ContentWindow.PreviousWindow as PfwStandard;
                //if( parentForm != null && bRefreshParent )    //#077295 
                if (parentForm != null && (bRefreshParent || _onwerChanged))    //#077295 _onwerChanged
                {
                    parentForm.CallParent(this.ScreenId);
                }
                return true;
            }
            return false;
        }
        private void pbGetRIM_Click(object sender, Phoenix.Windows.Forms.PActionEventArgs e)
        {
            PfwStandard tempWin = null;
            tempWin = Phoenix.Windows.Client.Helper.CreateWindow("phoenix.client.tbarsearch", "Phoenix.Client.Search", "frmTBarSearch");
            #region Comment - Parameter Meaning
            //				//-1, Embedded Window , 0 - From MDI, > 0 Coming from Other Windows for Search
            //				paramScreenId = Convert.ToInt32(paramList[0]); 
            //				//Name, AcctNo, Phone, and Tin			
            //				paramSearchType = (SetSearchType)paramList[1];
            //				//Acct Type, or Appl Type, or TIN
            //				paramSubSearchType = (SetSubSearchType)paramList[2];
            //				//Search Value==> Last Name, Acct Type, Phone, Tin
            //				paramSearchValue2 = Convert.ToString(paramList[3]);
            //				//First Name, AcctNo
            //				paramSearchValue3 = Convert.ToString(paramList[4]);
            //				//Invoke Accounts List
            //				paramInvokeScreenId = Convert.ToInt32(paramList[5]); 
            #endregion
            tempWin.InitParameters(this.ScreenId, Phoenix.BusObj.Global.SetSearchType.Name, string.Empty, string.Empty, string.Empty, -1);
            tempWin.Workspace = this.Workspace;
            tempWin.Show();
        }

        /* Begin #140811 */
        private void pbRemove_Click(object sender, PActionEventArgs e)
        {
            if (frmUserDefFieldsEdit != null)
            {
                frmUserDefFieldsEdit.RemoveWqUDF();
            }
        }
        /* End #140811 */

        #endregion

        #region Save Events
        private void frmRequestEdit_PBeforeSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
        {
            if (!this.IsNew && rbCompleted.Checked)
            {
                if (this._gbWorkQueue.TotalNotes.Value > 0)
                {
                    PMessageBox.Show(this, 319958, MessageType.Warning, MessageBoxButtons.OK);
                    //319958 - The request status cannot be set to \'Completed\' when pending tasks exist for the request record.
                    e.Cancel = true;
                    return;
                }
            }
            #region #GEMS #70179
            if (this.cmbOwner.Text.Trim() == string.Empty && !rbPending.Checked)
            {
                PMessageBox.Show(this, 9995, MessageType.Warning, MessageBoxButtons.OK);
                //! 9995 - An Owner is required for items which are not in Pending status.

                e.Cancel = true;
                return;

            }
            #endregion

            #region Set the Prev Empl Id
            if (this.cmbOwner.Text != string.Empty)
            {
                if (this.cmbOwner.CodeValue.ToString() != Convert.ToString(this._gbWorkQueue.OwnerEmplId.Value))
                {
                    this._gbWorkQueue.PrevOwnerEmplId.Value = this._gbWorkQueue.OwnerEmplId.Value;
                }
            }
            #endregion

            #region Set the LastWorkDate
            if (this.IsNew)
                this._gbWorkQueue.LastWorkDt.Value = DateTime.MinValue;
            else
                this._gbWorkQueue.LastWorkDt.Value = DateTime.Now;
            #endregion

            #region Parse and the account
            if (this.cmbAccount.Text.Trim() != string.Empty)
            {
                LocParseAccount(this.cmbAccount.Text.Trim());
            }
            else
            {
                this._gbWorkQueue.AcctType.Value = string.Empty;
                this._gbWorkQueue.ClassCode.Value = short.MinValue;
            }
            #endregion

            #region Build the Due Date with the time
            if (this.dfDueDate.Text.Trim() != string.Empty)
            {
                DateTime dttmpdt = Convert.ToDateTime(this.dfDueDate.UnFormattedValue);

                if (this.dfDueTime.Text.Trim() != string.Empty)
                {
                    DateTime dttmptime = Convert.ToDateTime(this.dfDueTime.UnFormattedValue);
                    this.dfDueDateWithTime.UnFormattedValue = new DateTime(dttmpdt.Year, dttmpdt.Month, dttmpdt.Day, dttmptime.Hour, dttmptime.Minute, dttmptime.Second);
                }
                else
                {
                    this.dfDueDateWithTime.UnFormattedValue = new DateTime(dttmpdt.Year, dttmpdt.Month, dttmpdt.Day, 0, 0, 0);
                }

                this._gbWorkQueue.DueDt.Value = Convert.ToDateTime(this.dfDueDateWithTime.UnFormattedValue);
            }
            #endregion

            #region Get the Queue ID of the selected Category
            if (this.cmbCategory.Text.Trim() != string.Empty)
            {
                this.dfQueueId.UnFormattedValue = Convert.ToInt32(this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatQueueId));
                this._gbWorkQueue.QueueId.Value = Convert.ToInt16(this.dfQueueId.UnFormattedValue);
            }
            #endregion

            #region Send Email
            this._gbWorkQueue.SendMailSQL.Value = string.Empty;

            if (this.cbEmailOwner.Checked)
            {
                string sTmpWorkStatus = string.Empty;

                sTmpWorkStatus = (rbPending.Checked ? "Pending" : string.Empty);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbInProcess.Checked ? "In Process" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbCompleted.Checked ? "Completed" : string.Empty) : sTmpWorkStatus);

                this._gbWorkQueue.SendWQMail(this.IsNew,
                                            dfCustomer.Text.Trim(),
                                            dfDescription.Text.Trim(),
                                            mulDetails.Text.Trim(),
                                            (cmbOwner.Text.Trim() == string.Empty ? string.Empty : cmbOwner.CodeValue.ToString()),
                                            cmbOwner.Text.Trim(),
                                            cmbCategory.Text.Trim(),
                                            sTmpWorkStatus,
                                            cmbPriority.Text.Trim(),
                                            "Request");

            }

            /* Begin #140811 */
            #region Send Group Email
            this._gbWorkQueue.SendGrpMailSQL.Value = string.Empty;

            /* Need to send the group email if the group email check box is checked and if it is a new referral/request 
            In edit mode if there is no owner assigned OR groupEmail is checked */

            if (cmbOwner.Text.Trim() == string.Empty || this.cbEmailGroup.Checked)
            {
                string sTmpWorkStatus = string.Empty;

                sTmpWorkStatus = (rbPending.Checked ? "Pending" : string.Empty);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbInProcess.Checked ? "In Process" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbCompleted.Checked ? "Completed" : string.Empty) : sTmpWorkStatus);

                this._gbWorkQueue.SendWQGrpMail(this.IsNew,
                                            dfCustomer.Text.Trim(),
                                            dfDescription.Text.Trim(),
                                            mulDetails.Text.Trim(),
                                            (cmbOwner.Text.Trim() == string.Empty ? string.Empty : cmbOwner.CodeValue.ToString()),
                                            cmbOwner.Text.Trim(),
                                            (cmbCategory.Text.Trim() == string.Empty ? string.Empty : cmbCategory.CodeValue.ToString()),
                                            cmbCategory.Text.Trim(),
                                            sTmpWorkStatus,
                                            cmbPriority.Text.Trim(),
                                            "Request");

            }
            #endregion
            /* End #140811 */
            #endregion

            #region #77295-aHussein

            //If ( cmbOwner.sCodeValue != cmbOwner.sMyPrevValue ) And IntGetItemNameX( phWndOwner ) = 'dfwWorkRefReq'
            //    Set phWndOwner.bOwnerChanged = TRUE
            if (cmbOwner.IsDirty)
            {
                _onwerChanged = true;
            }
            #endregion

            this.ScreenToObject(XmActionType.Default);
        }
        private void frmRequestEdit_PAfterSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
        {
            if (this.wasFormNew)
            {
                this.dfRequestId.UnFormattedValue = this._gbWorkQueue.WorkId.Value;
                // Save previous Value for this disabled field 
                this.dfRequestId.SavePrevValue(false);
            }

            #region #01698
            //SetGetRimButtonState();
            SetTabAssociates(picTabs.SelectedIndex);
            #endregion

            bRefreshParent = true;

        }
        /* Begin - Task #74463 */
        protected void frmRequestEdit_PAfterActionEvent(object sender, FormActionEventArgs e)//Ashok
        {
            //#94129 - Fixing coverity issue for dereference after null check
            //if (e != null && e.ActionType == XmActionType.New || e.ActionType == XmActionType.Update)
            if (e != null && (e.ActionType == XmActionType.New || e.ActionType == XmActionType.Update))
            {
                int pendingResponseCount = 0;

                _gbWorkQueue.InteractiveMessages.LoadFromXML(ref pendingResponseCount);
                if (pendingResponseCount > 0)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }
        /* End - Task #74463 */

        /* Begin #140811 */
        public override bool OnActionSave(bool isAddNext)
        {
            /* To call OnActionSave of the shared window on the third Tab */
            //bFromSave = true;
            bool bReturn = false;
            int nContextRow = 0;
            _gbWorkQueue.InteractiveMessages.Reset(); // Task #74463

            while (true) // Task #74463
            {
                if (this.frmUserDefFieldsEdit != null)
                {
                    if (frmUserDefFieldsEdit.CheckMandatoryValues())
                    {
                        /*14566 - There is at least one mandatory additional field that must be completed before closing this window.*/
                        PMessageBox.Show(this, 14566, MessageType.Error, MessageBoxButtons.OK);
                        picTabs.SelectTab(2);
                        return false;
                    }
                    /* Begin #104392 */
                    this.frmUserDefFieldsEdit.IsFormClose = false;
                    /* End #104392 */
                    /* if additional fields tab is not accessed it is not saved */
                    bDirtyUserDefVal = this.frmUserDefFieldsEdit.IsFormDirty();
                    if (bDirtyUserDefVal)
                    {
                        /* Begin - Task #74463 */
                        bIsGridEdited = true;
                        if (!this.frmUserDefFieldsEdit.OnActionSave(false))
                        { return false; }
                    }
                    else if (bIsGridEdited)
                        bDirtyUserDefVal = true;
                }
                //#120098 - Added frmUserDefFieldsEdit null validation
                //Checking whether Additional info tab edited if edited append BO object to process
                if (this.frmUserDefFieldsEdit != null && this.frmUserDefFieldsEdit.BoToProcessList.Count == 0 && bIsGridEdited && _BoToProcessList != null && _BoToProcessList.Count > 0)
                    this.frmUserDefFieldsEdit.BoToProcessList.AddRange(_BoToProcessList);
                /* End - Task #74463 */
                bReturn = base.OnActionSave(isAddNext);
                /* Begin - Task #74463 */
                if (!bReturn)
                {
                    int responseCollectedCount = 0;
                    bool quitProcessing = false;
                    Extension.ShowInteractiveMessages(_gbWorkQueue.InteractiveMessages, ref responseCollectedCount, ref quitProcessing);
                    if (quitProcessing && bIsGridEdited)
                    {
                        if (this.frmUserDefFieldsEdit.BoToProcessList != null && this.frmUserDefFieldsEdit.BoToProcessList.Count > 0)
                        {
                            this._BoToProcessList = new List<WqUserDefVal>();
                            _BoToProcessList.AddRange(this.frmUserDefFieldsEdit.BoToProcessList);
                        }
                    }
                    if (quitProcessing || responseCollectedCount == 0)
                    {
                        break;
                    }
                    _gbWorkQueue.InteractiveMessages.SerializeToXML(ref responseCollectedCount);
                }
                else if (bReturn && this.frmUserDefFieldsEdit != null)
                {
                    nContextRow = frmUserDefFieldsEdit.grdValues.ContextRow;
                    this.frmUserDefFieldsEdit = null;
                    CreateEmbeddedWindows("Additional");
                    frmUserDefFieldsEdit.grdValues.SelectRow(nContextRow);
                    this._BoToProcessList = null;
                    bIsGridEdited = false;
                    break;
                }
                /* End - Task #74463 */
            }
            return bReturn;
        }
        /* End #140811 */

        #endregion

        #region CallParent
        public override void CallParent(params object[] paramList)
        {
            string paramLocal;

            int closingScreenId = -1;

            if (paramList.Length > 0) // Some thing returned from 
            {
                closingScreenId = Convert.ToInt32(paramList[0]);
                if (closingScreenId == Shared.Constants.ScreenId.TBarSearchRim)
                {
                    paramLocal = Convert.ToString(paramList[1]);
                    #region RimNo - paramList[1]
                    if (paramLocal != string.Empty && paramLocal.Trim().Length > 0)
                    {
                        try
                        {
                            this._gbWorkQueue.RimNo.Value = Convert.ToInt32(paramLocal);
                            //Get the customer name.
                            this.dfCustomer.UnFormattedValue = this._gbHelper.GetNameDetails(this._gbWorkQueue.RimNo.Value);
                            this.dfContactName.UnFormattedValue = this.dfCustomer.UnFormattedValue;
                            this.dfCustomer.Enabled = false;
                            //Refresh the accts for this RIM
                            PopAccounts();
                        }
                        catch (PhoenixException ex)
                        {
                            //We do not want to catch anything
                            Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("Some Junk Must be there:" + ex.Message + "\n" + ex.InnerException);
                        }
                    }
                    else
                    {
                        this._gbWorkQueue.RimNo.Value = int.MinValue;
                    }
                    #endregion

                    #region Rim Type - paramList[2]
                    paramLocal = Convert.ToString(paramList[2]);
                    if (paramLocal != string.Empty && paramLocal.Trim().Length > 0)
                    {
                        try
                        {
                            this._gbWorkQueue.RimType.Value = paramLocal.Trim();
                        }
                        catch (PhoenixException ex)
                        {
                            //We do not want to catch anything
                            Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug("RimType:Unknown Error:" + ex.Message + "\n" + ex.InnerException);
                        }
                    }
                    else
                    {
                        this._gbWorkQueue.RimType.Value = string.Empty;
                    }
                    #endregion

                } //End of Search Screen Returns
            }
            base.CallParent(paramList); // #77295
            return;
        }
        #endregion CallParent

        #endregion

        #region Private Functions/Procedures
        private void EnableDisableVisibleLogic(string caseType)
        {
            switch (caseType)
            {
                case "Initialize":
                    #region Initialize Logic
                    if (this._gbWorkQueue.CustomerName.IsNull)
                        this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.Enable);
                    else
                        this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.DisableShowText);

                    //#06057 - disable it all the time as per centura
                    this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.DisableShowText);


                    if (this._gbWorkQueue.CanChangeOwner.Value == "Y")
                        this.cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    else
                        this.cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);

                    SetGetRimButtonState();
                    /* Begin #140811 Hiding Remove Button.*/
                    this.pbRemove.SetObjectStatus(VisibilityState.Hide, EnableState.Default);
                    /*End #140811*/
                    //SetCustomerDefaults();
                    #endregion
                    break;
            }

            return;
        }

        private void SetTabAssociates(int pnTabIndex)
        {
            /* Begin #140811 */
            if (pnTabIndex == 0)
            {
                SetGetRimButtonState();
                this.pbRemove.Visible = false;
            }
            else if (pnTabIndex == 1)
            {
                this.pbGetRIM.Visible = false;
                this.pbRemove.Visible = false;

            }
            else if (pnTabIndex == 2)
            {
                this.pbGetRIM.Visible = false;
                this.pbRemove.Visible = true;
                pbRemove.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
            }
            /* End #140811 */

            this.Workspace.UpdateView();
        }

        private void PopulateDefaultCombos()
        {
            //Pop the accounts
            PopAccounts();
            cmbCategory.PhoenixUIControl.IsNullWhenDisabled = PBoolState.False;//#77295-aHussein

            //Pop the owner names
            /* Commented by Nishad */
            //PopOwnerNames();
            return;
        }

        private void PopOwnerNames()
        {
            //Pop the owner names
            Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.OwnerEmplId);
            cmbOwner.Populate(_gbWorkQueue.OwnerEmplId.Constraint.EnumValues);
        }

        private void PopAccounts()
        {
            //Pop the accounts for this rim.
            if (!this._gbWorkQueue.RimNo.IsNull)
            {
                Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.ReqAccts);
                cmbAccount.Populate(_gbWorkQueue.ReqAccts.Constraint.EnumValues);
            }
        }
        private void LocCheckPMA()
        {
            string IsPMAEnabled = this._gbWorkQueue.IsPMAEnabled();

            if (IsPMAEnabled.Trim() == "N" || this._gbWorkQueue.OwnerEmplId.IsNull || cmbOwner.Text.Trim() == "<none>")
            {
                this.cbEmailOwner.Checked = false;
                this.cbEmailOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);
            }
            else
            {
                if (!this._gbWorkQueue.OwnerEmplId.IsNull)
                {
                    this.cbEmailOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    string sAutoEmailOwner = this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatEmailOwner);
                    if (sAutoEmailOwner.Trim() == "Y")
                        this.cbEmailOwner.Checked = true;
                }
            }
        }

        private void LocParseAccount(string psAccount)
        {
            psAccount = psAccount.Trim();

            if (psAccount == string.Empty)
                return;

            //Example Source String for DP, LN and EXT = "DDA - 123456" and for SDB = "SDB - 100-15245"
            string[] psaTemp = psAccount.Split('-');

            if (psaTemp[0].Trim() != string.Empty)
                this._gbWorkQueue.AcctType.Value = psaTemp[0].Trim();

            if (psaTemp[1].Trim() != string.Empty)
                this._gbWorkQueue.AcctNo.Value = psaTemp[1].Trim();

            if (psaTemp.Length > 2)
            {
                //For SDB accts we have a "-" in the acct no itself so gsaTemp[2] will have the second part of the acct no
                if (psaTemp[2] != string.Empty)
                    this._gbWorkQueue.AcctNo.Value += "-" + psaTemp[2].Trim();
            }

            return;
        }

        private void SetGetRimButtonState()
        {
            #region Commented Code - #01698

            // this.pbGetRIM.Visible =  this._gbWorkQueue.SetGetRimButtonState(this.IsNew, "Request", string.Empty);
            // this.pbGetRIM.Enabled = this.pbGetRIM.Visible;

            #endregion

            if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "Teller")
            {
                this.pbGetRIM.Visible = this._gbWorkQueue.SetGetRimButtonState(this.IsNew, "Request", string.Empty);
                this.pbGetRIM.Enabled = this.pbGetRIM.Visible;
            }
            else if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
            {
                this.pbGetRIM.Enabled = this._gbWorkQueue.SetGetRimButtonState(this.IsNew, "Request", string.Empty);
                this.pbGetRIM.Visible = true;
            }

            return;
        }

        /* Begin #140811 */

        private void CreateEmbeddedWindows(string windowType)
        {
            // we want to load the window if the window is null or if the category is changed otherwise no need to load window again and again.
            if (windowType == "Additional" && (frmUserDefFieldsEdit == null || bCategoryChanged))
            {
                //360821 - Please wait while retriving customer information...71231
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360821));
                frmUserDefFieldsEdit = CreateWindow("phoenix.client.dplnuserdefval", "Phoenix.Client.DepLoan", "frmUserDefinedFieldsEdit") as Phoenix.Client.DepLoan.frmUserDefinedFieldsEdit;
                frmUserDefFieldsEdit.IsAllowedToBeMaximizedAlways = false;  // WI#29008
                frmUserDefFieldsEdit.Size = new Size(682, 422);
                if (frmUserDefFieldsEdit.Controls.Count > 0 && frmUserDefFieldsEdit.Controls[0] is PGrid)// WI#29008
                {
                    frmUserDefFieldsEdit.Controls[0].Size = new Size(679, 415);
                    frmUserDefFieldsEdit.Controls[0].Top = 4;
                    frmUserDefFieldsEdit.Controls[0].Left = 0;
                }

                frmUserDefFieldsEdit.InitParameters(_gbWorkQueue.WorkId.Value, /* ServiceId */
                    null, /*RimNo */
                    null, /*RelRimNo */
                    null, /*AcctType */
                    null, /* AcctNo */
                    this._gbWorkQueue.CategoryId.Value, /*AcctId*/
                    "WQ", /*FromWhere */
                    true /*EnableEdit */
                    );

                dfTabTitle2.AssociatedForm = frmUserDefFieldsEdit;
                frmUserDefFieldsEdit.WQType = _gbWorkQueue.Type.Value;
                frmUserDefFieldsEdit.Show();

                //foreach (PAction action in frmUserDefFieldsEdit.ActionManager.Actions)
                //{
                //    action.Enabled = false;
                //    action.Visible = false;
                //}
                bCategoryChanged = false;
                frmUserDefFieldsEdit.grdValues.SelectedIndexChanged += new GridClickedEventHandler(grdValuesUserDefFieldsEdit_SelectedIndexChanged);
                frmUserDefFieldsEdit.grdValues.RowClicked += new GridClickedEventHandler(grdValueUserDefFieldsEdit_RowClicked);
            }
        }

        public void EnableTab(PTabPage page, bool enable)
        {
            page.IsDisabled = !enable;
        }

        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Visible = enable;
                EnableControls(ctl.Controls, enable);
            }
        }

        void grdValueUserDefFieldsEdit_RowClicked(object source, GridClickEventArgs e)
        {
            if (frmUserDefFieldsEdit != null)
            {
                if (frmUserDefFieldsEdit.colValueExists != null)
                {
                    if (frmUserDefFieldsEdit.colValueExists.UnFormattedValue.ToString() == "Y")
                    {
                        pbRemove.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                    }
                    else
                    {
                        pbRemove.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                    }
                }
            }
        }

        private void grdValuesUserDefFieldsEdit_SelectedIndexChanged(object source, GridClickEventArgs e)
        {
            if (frmUserDefFieldsEdit != null)
            {
                if (frmUserDefFieldsEdit.colValueExists != null)
                {
                    if (frmUserDefFieldsEdit.colValueExists.UnFormattedValue.ToString() == "Y")
                    {
                        pbRemove.SetObjectStatus(VisibilityState.Default, EnableState.Enable);
                    }
                    else
                    {
                        pbRemove.SetObjectStatus(VisibilityState.Default, EnableState.Disable);
                    }
                }
            }

        }
        /* End #140811 */

        #endregion

        #region override
        /* Begin #140811 */
        protected override void SetBusObjectsToProcess(XmActionType action, System.Collections.Generic.List<BusObjectBase> busObjectsToProcess)
        {
            busObjectsToProcess.Clear();
            base.SetBusObjectsToProcess(action, busObjectsToProcess); // add the main business object
            if (frmUserDefFieldsEdit != null)
            {
                if (action == XmActionType.Delete || action == XmActionType.New || action == XmActionType.Update)
                {
                    // make sure BoToProcess is not null and length is greater than 0
                    for (int idx = 0; idx < this.frmUserDefFieldsEdit.BoToProcessList.Count; idx++)
                    {
                        if (this.frmUserDefFieldsEdit.BoToProcessList[idx] != null)
                        {
                            busObjectsToProcess.Add(this.frmUserDefFieldsEdit.BoToProcessList[idx]);
                        }
                    }
                }
            }
            return;
        }
        public override bool PerformCheck(CheckType checkType, bool showMessage)
        {
            if (checkType == CheckType.EditTest && bDirtyUserDefVal)
            {
                bDirtyUserDefVal = false;
                return true;
            }
            else
            {
                return base.PerformCheck(checkType, showMessage);
            }
        }
        /* End #140811 */
        #endregion

        #region #Set Focus on Correct TAB
        public override bool SetFocusOnControl(Control focusControl)
        {
            if (focusControl == null)
                return false;

            picTabs.Focus();
            for (int index = 0; index < picTabs.TabPages.Count; index++)
            {
                if (picTabs.TabPages[index].Contains(focusControl))
                {
                    picTabs.SuspendLayout();
                    picTabs.SelectTab(index);

                    focusControl.Focus();
                    picTabs.ResumeLayout(true);
                    this.Invalidate(true);
                    break;
                }
            }
            return true;
        }
        #endregion

        /*Begin #9885*/
        private void cmbQueue_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
        {
            this.cmbCategory.Repopulate();
            return;
        }

        private void cmbCategory_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
        {
            PopOwnerNames();
            int nDefCatEmplId = Convert.ToInt32(this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatEmplId));

            /* Begin #27225 */
            if (!_gbWorkQueue.OwnerEmplId.IsNull)
            {
                this.cmbOwner.SetValueAndSelect(_gbWorkQueue.OwnerEmplId.Value, true);
            }
            else if (nDefCatEmplId > 0)
            {
                this.cmbOwner.SetValueAndSelect(nDefCatEmplId, true);
                LocCheckPMA();
            }
            else if (_gbWorkQueue.OwnerEmplId.IsNull)
            {
                this.cmbOwner.SetValueAndSelect(-1, false);
            }
            /* End #27225 */

            LocCanChangeOwner();//#77295-aHussein
            /* Begin #140811 - Changing Category Changed flag. 
             * If Category is changed Additional fiels window will be reloaded.*/
            bCategoryChanged = true;
            /* Begin #27262 */
            this.frmUserDefFieldsEdit = null;
            /* End #27262 */
            if (IsNew)
            {
                /* Need to default Group Email check box from Ad_WQ_Category */
                if (cmbCategory.Text != string.Empty)
                {
                    _adWqCategory.CategoryId.Value = Convert.ToInt16(cmbCategory.CodeValue.ToString());
                    _adWqCategory.ActionType = XmActionType.Select;
                    CoreService.DataService.ProcessRequest(_adWqCategory);
                    if (_adWqCategory.AutoEmailGroup.Value == "Y")
                    {
                        cbEmailGroup.Checked = true;
                    }
                    else
                    {
                        cbEmailGroup.Checked = false;
                    }
                    /* Enabling third tab (Additional fields tab) since Category is selected */
                    EnableTab(dfTabTitle2, true);
                }
            }
            /* End #140811 */
            /*Creating new Instance of frmUserDefinedFeildsEdit*/
            CreateEmbeddedWindows("Additional");
        }
        /*End #9885*/
    }
}
