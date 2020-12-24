#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - frmReferralEdit - Edit ther referral record.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: frmWorkRefReq.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//09/20/06		1		VDevadoss	#69248 - Created
//12/12/07      2       SDighe      #73762 - The enter key does not add a paragraph and saves the record.
//11/23/2008    3       Nelsehety   #01698 - .Net Porting
//01/26/2009    4       Nelsehety   #01698 -Bug Fixing
//03/08/2009    5       ewahib      #01698 - Add Code to set Focus On correct tab for Mandatory fields
//03/08/2009    6       ewahib      #01698 - #GEMS #70179 retrofit
//09/10/2009    7       aHussein    #77295-aHussein Re-Porting Issues
//10/15/2009    8       Mona        #06057
//12/08/2009    9       ewahib      #06688 - fix the tab order
//12/24/2009    10      ewahib      #06139 - parent screen could be null.
//01/08/2010    11      vsharma     #6683, 6684 - Do not force security if there is no security defined in admin
//07/24/2010    12      VDevadoss   #9885 - Added code to pop the category and queue id properly.
//09/20/2011    13      TTaylor     #15229 - Added code to populate values from the Product Sales screen
//12/01/2011    14      TTaylor     #16252 - To allow the user to edit the screen.
//01/23/2011    15      jwatts      #15234 - To ensure work_status of completed Referrals and Requests have proper vals.
//05/16/2012    16      SDighe      WI#17953 - When working a referral - choosing the Edit Details button opens a New Referral window.
//08/06/2012    17      Mkrishna    #19058 - Adding call to base on initParameters.
//3/8/2013      18      MBachala    21482 - Receive Application error when selection E-Mail > New Referral from Relationship Management Summary window
//07/03/2013    19      Alfred      #22291 -Issue fixed -  system will present a 'Read Only' error and not allow the user to save their changes
//12/20/2013    20      NKasim      #140811 - Sales and Service. Embedded additional fields edit window in this window.
//01/30/2014    21      NKasim      #26953 - Additional Fields validation message is shown only OnActionSave(). 
//                                          OnActionClose() shows Default message.
//                                          PerformCheck() handles isDirty from Container and Embedded windows
//02/07/2014    22      NKasim      #27262 - On changing category instance of frmUserDefVal cleared.
//02/24/2014    23      NKasim      #27225 - Owner employee is Loaded from Gb_work_Queue.owner_empl_id if exists
//02/17/2015    24      MKrishna      #174961 - Changes as part of 174961 - Sales and Service.
//03/16/2015    25      MKrishna      #35569 - Exipiry_dt modification added.
//03/23/2015    26      rpoddar     #34896 - Resizing fixes
//04/09/2015    27      Hirankumar  #36167 - 174961- Sales and Service Phase 1 - Task Type not updated correctly when Referral Created.
//04/27/2015    28      Hirankumar  #35638 - 174961- Enhanced Sales and Service Phase 1- Referral Window Issues
//05/07/2015    29      Vidya       #36915 - Added shortcut char for the second tab title.
//05/20/2015    30      MKrishna     #36470,#37177 - 179461- Sales and Service Phase 1 Task issues
//05/26/2015    31      MKrishna     CR 37141. Add ATM type to  application type.
//06/15/2015    32      Kiran       WI37589 - Set ResponseTypeId = 13 on cmbQueue_PhoenixUIEnterEvent,change ResponseTypeId = 13 to enum "ReferralEdit"
//07/29/2015    33      Vipin       #37896 - Modified the grids with new columns for Rim3rdTypeId and Rim3rdInsId to insert into Gb_Work_Queue_Note
//08/18/2015    34      Vipin       #37896 - Added Service Id = -14 for SelectedProductData to insert Rm_3rd_type_id and Rm_3rd_Ins_Id                                           
//08/26/2015    35      Vipin       #38646 - Cosmetic issues with the window.Reduced the size of controls and hide unwanted columns from Grid
//                                           Issue in window while accessing through teller.Issue with workspace    
//10/27/2015    36      RDeepthi    WI#39394 - Added CheckTypeEdit check in Perform Check. So that Nulvalidation can be check in perform check    
//11/06/2015    37      RDeepthi    WI#39440 - Event is defined but event handler was not definded for cmb_owner (Defa). So Defined event handler for it.
//02/01/2016    38      NKasim      #41178 - For ATM and External products AcctType and ApplType is set properly while adding to the second grid.
//03/03/2016    39      NKasim      #42231 - Window cannot be saved if prod or services are NOT selected.
//03/16/2018    40      Ashok HCL   #74463 - Additional Fields Information on Sales & Service Requests are not displaying in Member Management on the Edit Existing Request window>Additional Information tab when Sales & Service
//12/07/2018    41      SChacko     Task#104392 -  None value is not saving for an Additional Field
//01/21/2019    42      Kiran       Bug #108676: Add new referral – Error while saving adde null validation 
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
using Phoenix.Client.DepLoan;
using Phoenix.Client.WorkQueue; // 174961
using Phoenix.BusObj.Misc;//Task #74463
using System.Collections.Generic;//Task #74463

namespace Phoenix.Client.WorkQueue
{
    /// <summary>
    /// Summary description for frmReferralEdit.
    /// </summary>
    public class frmReferralEdit : Phoenix.Windows.Forms.PfwStandard
    {
        #region Private variables

        private bool wasFormNew = true;
        private Phoenix.BusObj.Global.GbWorkQueue _gbWorkQueue = new Phoenix.BusObj.Global.GbWorkQueue();
        private Phoenix.BusObj.Global.GbHelper _gbHelper = new Phoenix.BusObj.Global.GbHelper();
        private Form _parentForm = null;
        private bool bRefreshParent = false;
        private string type = ""; // #15229
        private string sClass = ""; // #15229
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
        private Phoenix.Windows.Forms.PTabPage dfTabTitle0;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbReferralInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblReferralID;
        private Phoenix.Windows.Forms.PdfStandard dfReferralId;
        private Phoenix.Windows.Forms.PLabelStandard lblCustomer;
        private Phoenix.Windows.Forms.PdfStandard dfCustomer;
        private Phoenix.Windows.Forms.PCheckBoxStandard cbNewProspect;
        private Phoenix.Windows.Forms.PLabelStandard lblCategory;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbCategory;
        private Phoenix.Windows.Forms.PLabelStandard lblReferralCode;
        private Phoenix.Windows.Forms.PdfStandard dfReferralCode;
        private Phoenix.Windows.Forms.PLabelStandard lblDueDate;
        private Phoenix.Windows.Forms.PdfStandard dfDueDate;
        private Phoenix.Windows.Forms.PLabelStandard lblTime;
        private Phoenix.Windows.Forms.PdfStandard dfDueTime;
        private Phoenix.Windows.Forms.PLabelStandard lblDescription;
        private Phoenix.Windows.Forms.PdfStandard dfDescription;
        private Phoenix.Windows.Forms.PLabelStandard lblDetails;
        private Phoenix.Windows.Forms.PdfStandard mulDetails;
        private Phoenix.Windows.Forms.PLabelStandard lblProduct;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbProductAppl;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbProductClass;
        //private Phoenix.Windows.Forms.PLabelStandard lblEffective;
        //private Phoenix.Windows.Forms.PdfEffectiveDt dfEffectiveDt;
        //private Phoenix.Windows.Forms.PLabelStandard lblStatus;
        //private Phoenix.Windows.Forms.PCmbStatus cmbStatus;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbReferralStatusInformation;
        private Phoenix.Windows.Forms.PLabelStandard lblReferralStatus;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbPending;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbInProcess;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbSold;
        private Phoenix.Windows.Forms.PRadioButtonStandard rbDeclined;
        private Phoenix.Windows.Forms.PLabelStandard lblPriority;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbPriority;
        private Phoenix.Windows.Forms.PLabelStandard lblReason;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbReason;
        private Phoenix.Windows.Forms.PAction pbGetRIM;
        private Phoenix.Windows.Forms.PdfStandard dfRimNo;
        private Phoenix.Windows.Forms.PdfStandard dfProductAcctType;
        private Phoenix.Windows.Forms.PdfStandard dfProductClassCode;
        private Phoenix.Windows.Forms.PdfStandard dfPrioritySort;
        private Phoenix.Windows.Forms.PdfStandard dfRecordType;
        private Phoenix.Windows.Forms.PdfStandard dfPrevEmplId;
        private Phoenix.Windows.Forms.PdfStandard dfQueueId;
        private Phoenix.Windows.Forms.PdfStandard dfLastWorkDt;
        private PComboBoxStandard cmbQueue;
        private PLabelStandard lblQueue;
        private Phoenix.Windows.Forms.PTabPage dfTabTitle2;
        private PAction pbRemove;
        private PGroupBoxStandard pGroupBoxStandard1;
        private PdfStandard dfOtherInfo;
        private PLabelStandard pLabelStandard1;
        private PdfStandard dfContactName;
        private PLabelStandard pLabelStandard2;
        private PComboBoxStandard cmbContactMethod;
        private PLabelStandard pLabelStandard3;
        private PGroupBoxStandard pGroupBoxStandard2;
        private PCheckBoxStandard cbEmailGroup;
        private PdfStandard dfInitiatedBy;
        private PLabelStandard pLabelStandard4;
        private PCheckBoxStandard cbEmailOwner;
        private PComboBoxStandard cmbOwner;
        private PLabelStandard pLabelStandard5;
        private PRadioButtonStandard rbExpired;
        private TabPage dfProductSvcs;
        private PButtonStandard pbRemoveProd;
        private PButtonStandard pbAdd;
        private PGroupBoxStandard gbSelProdServices;
        private PDataGridView grdSelProdSer;
        private PGroupBoxStandard gbAvailProdSer;
        private PDataGridView grdAvailProd;
        private PComboBoxStandard cmbApplType;
        private PComboBoxStandard cmbType;
        private PLabelStandard lblApplType;
        private PLabelStandard lblType;
        private PDataGridViewColumn colGroupId;
        private PDataGridViewColumn colDescription;
        private PDataGridViewCheckBoxColumn cbSelectedProd;
        private PDataGridViewColumn colType;
        private PDataGridViewColumn colQuantity;
        private PDataGridViewColumn colSelDescription;
        private PDataGridViewColumn colQuantityHidden;
        private PDataGridViewColumn colProductGroupId;
        private PDataGridViewColumn colServiceId;
        private PDataGridViewColumn colApplType;
        private PDataGridViewColumn colAcctType;
        private PDataGridViewColumn colClassCode;
        /*Begin #37896*/
        private PDataGridViewColumn colRm3rdType;
        private PDataGridViewColumn colRm3rdIns;
        private PDataGridViewColumn colRmThrdType;
        private PDataGridViewColumn colRmThirdIns;
        /*End #37896*/
        private Phoenix.Windows.Forms.PdfStandard dfDueDateWithTime;
        private List<WqUserDefVal> _BoToProcessList; //Task #74463
        #endregion

        #region Constructor
        public frmReferralEdit()
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            Phoenix.FrameWork.Core.ControlInfo controlInfo2 = new Phoenix.FrameWork.Core.ControlInfo();
            this.picTabs = new Phoenix.Windows.Forms.PTabControl();
            this.dfTabTitle0 = new Phoenix.Windows.Forms.PTabPage();
            this.pGroupBoxStandard2 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cbEmailGroup = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfInitiatedBy = new Phoenix.Windows.Forms.PdfStandard();
            this.pLabelStandard4 = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbEmailOwner = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.cmbOwner = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.pLabelStandard5 = new Phoenix.Windows.Forms.PLabelStandard();
            this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfOtherInfo = new Phoenix.Windows.Forms.PdfStandard();
            this.pLabelStandard1 = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfContactName = new Phoenix.Windows.Forms.PdfStandard();
            this.pLabelStandard2 = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbContactMethod = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.pLabelStandard3 = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbReferralInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cmbQueue = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblQueue = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbProductClass = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cmbProductAppl = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblProduct = new Phoenix.Windows.Forms.PLabelStandard();
            this.mulDetails = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDetails = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDescription = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDescription = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueTime = new Phoenix.Windows.Forms.PdfStandard();
            this.lblTime = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfDueDate = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDueDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReferralCode = new Phoenix.Windows.Forms.PdfStandard();
            this.lblReferralCode = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbCategory = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblCategory = new Phoenix.Windows.Forms.PLabelStandard();
            this.cbNewProspect = new Phoenix.Windows.Forms.PCheckBoxStandard();
            this.dfCustomer = new Phoenix.Windows.Forms.PdfStandard();
            this.lblCustomer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfReferralId = new Phoenix.Windows.Forms.PdfStandard();
            this.lblReferralID = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbReferralStatusInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.rbExpired = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.cmbReason = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblReason = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbPriority = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblPriority = new Phoenix.Windows.Forms.PLabelStandard();
            this.rbDeclined = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbSold = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbInProcess = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.rbPending = new Phoenix.Windows.Forms.PRadioButtonStandard();
            this.lblReferralStatus = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfProductSvcs = new System.Windows.Forms.TabPage();
            this.pbRemoveProd = new Phoenix.Windows.Forms.PButtonStandard();
            this.pbAdd = new Phoenix.Windows.Forms.PButtonStandard();
            this.gbSelProdServices = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdSelProdSer = new Phoenix.Windows.Forms.PDataGridView();
            this.cbSelectedProd = new Phoenix.Windows.Forms.PDataGridViewCheckBoxColumn();
            this.colType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colQuantity = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colSelDescription = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colQuantityHidden = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colProductGroupId = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colServiceId = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colApplType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colClassCode = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRmThrdType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRmThirdIns = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.gbAvailProdSer = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdAvailProd = new Phoenix.Windows.Forms.PDataGridView();
            this.colGroupId = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colDescription = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRm3rdType = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.colRm3rdIns = new Phoenix.Windows.Forms.PDataGridViewColumn();
            this.cmbApplType = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.cmbType = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblApplType = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblType = new Phoenix.Windows.Forms.PLabelStandard();
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
            this.picTabs.SuspendLayout();
            this.dfTabTitle0.SuspendLayout();
            this.pGroupBoxStandard2.SuspendLayout();
            this.pGroupBoxStandard1.SuspendLayout();
            this.gbReferralInformation.SuspendLayout();
            this.gbReferralStatusInformation.SuspendLayout();
            this.dfProductSvcs.SuspendLayout();
            this.gbSelProdServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSelProdSer)).BeginInit();
            this.gbAvailProdSer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdAvailProd)).BeginInit();
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
            this.picTabs.Controls.Add(this.dfProductSvcs);
            this.picTabs.Controls.Add(this.dfTabTitle2);
            this.picTabs.Dock = System.Windows.Forms.DockStyle.None;
            this.picTabs.Location = new System.Drawing.Point(0, 0);
            this.picTabs.Name = "picTabs";
            this.picTabs.SelectedIndex = 0;
            this.picTabs.Size = new System.Drawing.Size(690, 504);
            this.picTabs.TabIndex = 0;
            this.picTabs.SelectedIndexChanged += new System.EventHandler(this.picTabs_SelectedIndexChanged);
            // 
            // dfTabTitle0
            // 
            this.dfTabTitle0.BackColor = System.Drawing.Color.Transparent;
            this.dfTabTitle0.Controls.Add(this.pGroupBoxStandard2);
            this.dfTabTitle0.Controls.Add(this.pGroupBoxStandard1);
            this.dfTabTitle0.Controls.Add(this.gbReferralInformation);
            this.dfTabTitle0.Controls.Add(this.gbReferralStatusInformation);
            this.dfTabTitle0.Location = new System.Drawing.Point(4, 22);
            this.dfTabTitle0.MLInfo = controlInfo1;
            this.dfTabTitle0.Name = "dfTabTitle0";
            this.dfTabTitle0.Size = new System.Drawing.Size(682, 478);
            this.dfTabTitle0.TabIndex = 0;
            this.dfTabTitle0.Text = "&Pending Tasks";
            this.dfTabTitle0.UseVisualStyleBackColor = true;
            // 
            // pGroupBoxStandard2
            // 
            this.pGroupBoxStandard2.Controls.Add(this.cbEmailGroup);
            this.pGroupBoxStandard2.Controls.Add(this.dfInitiatedBy);
            this.pGroupBoxStandard2.Controls.Add(this.pLabelStandard4);
            this.pGroupBoxStandard2.Controls.Add(this.cbEmailOwner);
            this.pGroupBoxStandard2.Controls.Add(this.cmbOwner);
            this.pGroupBoxStandard2.Controls.Add(this.pLabelStandard5);
            this.pGroupBoxStandard2.Location = new System.Drawing.Point(3, 408);
            this.pGroupBoxStandard2.Name = "pGroupBoxStandard2";
            this.pGroupBoxStandard2.PhoenixUIControl.ObjectId = 16;
            this.pGroupBoxStandard2.Size = new System.Drawing.Size(680, 69);
            this.pGroupBoxStandard2.TabIndex = 3;
            this.pGroupBoxStandard2.TabStop = false;
            this.pGroupBoxStandard2.Text = "Referral Ownership Information";
            // 
            // cbEmailGroup
            // 
            this.cbEmailGroup.AutoSize = true;
            this.cbEmailGroup.BackColor = System.Drawing.SystemColors.Control;
            this.cbEmailGroup.Checked = true;
            this.cbEmailGroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEmailGroup.Location = new System.Drawing.Point(463, 44);
            this.cbEmailGroup.Name = "cbEmailGroup";
            this.cbEmailGroup.PhoenixUIControl.ObjectId = 52;
            this.cbEmailGroup.Size = new System.Drawing.Size(197, 18);
            this.cbEmailGroup.TabIndex = 5;
            this.cbEmailGroup.Text = "Auto E-Mail All Queue Group Users";
            this.cbEmailGroup.UseVisualStyleBackColor = false;
            this.cbEmailGroup.Value = null;
            // 
            // dfInitiatedBy
            // 
            this.dfInitiatedBy.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfInitiatedBy.Location = new System.Drawing.Point(97, 42);
            this.dfInitiatedBy.MaxLength = 40;
            this.dfInitiatedBy.Name = "dfInitiatedBy";
            this.dfInitiatedBy.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfInitiatedBy.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfInitiatedBy.PhoenixUIControl.ObjectId = 18;
            this.dfInitiatedBy.PreviousValue = null;
            this.dfInitiatedBy.Size = new System.Drawing.Size(302, 20);
            this.dfInitiatedBy.TabIndex = 4;
            // 
            // pLabelStandard4
            // 
            this.pLabelStandard4.AutoEllipsis = true;
            this.pLabelStandard4.Location = new System.Drawing.Point(6, 42);
            this.pLabelStandard4.Name = "pLabelStandard4";
            this.pLabelStandard4.PhoenixUIControl.ObjectId = 18;
            this.pLabelStandard4.Size = new System.Drawing.Size(74, 20);
            this.pLabelStandard4.TabIndex = 3;
            this.pLabelStandard4.Text = "Initiated By:";
            // 
            // cbEmailOwner
            // 
            this.cbEmailOwner.BackColor = System.Drawing.SystemColors.Control;
            this.cbEmailOwner.Checked = true;
            this.cbEmailOwner.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEmailOwner.Location = new System.Drawing.Point(463, 18);
            this.cbEmailOwner.Name = "cbEmailOwner";
            this.cbEmailOwner.PhoenixUIControl.ObjectId = 17;
            this.cbEmailOwner.PhoenixUIControl.XmlTag = "";
            this.cbEmailOwner.Size = new System.Drawing.Size(139, 20);
            this.cbEmailOwner.TabIndex = 2;
            this.cbEmailOwner.Text = "Notify Owner via E-Mail";
            this.cbEmailOwner.UseVisualStyleBackColor = false;
            this.cbEmailOwner.Value = null;
            // 
            // cmbOwner
            // 
            this.cmbOwner.Location = new System.Drawing.Point(97, 18);
            this.cmbOwner.Name = "cmbOwner";
            this.cmbOwner.PhoenixUIControl.ObjectId = 38;
            this.cmbOwner.Size = new System.Drawing.Size(302, 21);
            this.cmbOwner.TabIndex = 1;
            this.cmbOwner.Value = null;
            this.cmbOwner.SelectedIndexChanged += new System.EventHandler(this.cmbOwner_SelectedIndexChanged); //WI#39440
            // 
            // pLabelStandard5
            // 
            this.pLabelStandard5.AutoEllipsis = true;
            this.pLabelStandard5.Location = new System.Drawing.Point(6, 18);
            this.pLabelStandard5.Name = "pLabelStandard5";
            this.pLabelStandard5.PhoenixUIControl.ObjectId = 38;
            this.pLabelStandard5.Size = new System.Drawing.Size(74, 20);
            this.pLabelStandard5.TabIndex = 0;
            this.pLabelStandard5.Text = "Owner:";
            // 
            // pGroupBoxStandard1
            // 
            this.pGroupBoxStandard1.Controls.Add(this.dfOtherInfo);
            this.pGroupBoxStandard1.Controls.Add(this.pLabelStandard1);
            this.pGroupBoxStandard1.Controls.Add(this.dfContactName);
            this.pGroupBoxStandard1.Controls.Add(this.pLabelStandard2);
            this.pGroupBoxStandard1.Controls.Add(this.cmbContactMethod);
            this.pGroupBoxStandard1.Controls.Add(this.pLabelStandard3);
            this.pGroupBoxStandard1.Location = new System.Drawing.Point(0, 316);
            this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
            this.pGroupBoxStandard1.PhoenixUIControl.ObjectId = 12;
            this.pGroupBoxStandard1.Size = new System.Drawing.Size(680, 92);
            this.pGroupBoxStandard1.TabIndex = 2;
            this.pGroupBoxStandard1.TabStop = false;
            this.pGroupBoxStandard1.Text = "Customer Contact Information";
            // 
            // dfOtherInfo
            // 
            this.dfOtherInfo.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOtherInfo.Location = new System.Drawing.Point(97, 66);
            this.dfOtherInfo.MaxLength = 80;
            this.dfOtherInfo.Name = "dfOtherInfo";
            this.dfOtherInfo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfOtherInfo.PhoenixUIControl.ObjectId = 15;
            this.dfOtherInfo.PreviousValue = null;
            this.dfOtherInfo.Size = new System.Drawing.Size(302, 20);
            this.dfOtherInfo.TabIndex = 5;
            // 
            // pLabelStandard1
            // 
            this.pLabelStandard1.AutoEllipsis = true;
            this.pLabelStandard1.Location = new System.Drawing.Point(6, 66);
            this.pLabelStandard1.Name = "pLabelStandard1";
            this.pLabelStandard1.PhoenixUIControl.ObjectId = 15;
            this.pLabelStandard1.Size = new System.Drawing.Size(96, 20);
            this.pLabelStandard1.TabIndex = 4;
            this.pLabelStandard1.Text = "Other Information:";
            // 
            // dfContactName
            // 
            this.dfContactName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.Location = new System.Drawing.Point(97, 42);
            this.dfContactName.MaxLength = 80;
            this.dfContactName.Name = "dfContactName";
            this.dfContactName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfContactName.PhoenixUIControl.ObjectId = 14;
            this.dfContactName.PreviousValue = null;
            this.dfContactName.Size = new System.Drawing.Size(302, 20);
            this.dfContactName.TabIndex = 3;
            // 
            // pLabelStandard2
            // 
            this.pLabelStandard2.AutoEllipsis = true;
            this.pLabelStandard2.Location = new System.Drawing.Point(6, 42);
            this.pLabelStandard2.Name = "pLabelStandard2";
            this.pLabelStandard2.PhoenixUIControl.ObjectId = 14;
            this.pLabelStandard2.Size = new System.Drawing.Size(84, 20);
            this.pLabelStandard2.TabIndex = 2;
            this.pLabelStandard2.Text = "Contact Name:";
            // 
            // cmbContactMethod
            // 
            this.cmbContactMethod.Location = new System.Drawing.Point(97, 18);
            this.cmbContactMethod.Name = "cmbContactMethod";
            this.cmbContactMethod.PhoenixUIControl.ObjectId = 13;
            this.cmbContactMethod.Size = new System.Drawing.Size(302, 21);
            this.cmbContactMethod.TabIndex = 1;
            this.cmbContactMethod.Value = null;
            // 
            // pLabelStandard3
            // 
            this.pLabelStandard3.AutoEllipsis = true;
            this.pLabelStandard3.Location = new System.Drawing.Point(8, 18);
            this.pLabelStandard3.Name = "pLabelStandard3";
            this.pLabelStandard3.PhoenixUIControl.ObjectId = 13;
            this.pLabelStandard3.Size = new System.Drawing.Size(88, 20);
            this.pLabelStandard3.TabIndex = 0;
            this.pLabelStandard3.Text = "Contact Method:";
            // 
            // gbReferralInformation
            // 
            this.gbReferralInformation.Controls.Add(this.cmbQueue);
            this.gbReferralInformation.Controls.Add(this.lblQueue);
            this.gbReferralInformation.Controls.Add(this.cmbProductClass);
            this.gbReferralInformation.Controls.Add(this.cmbProductAppl);
            this.gbReferralInformation.Controls.Add(this.lblProduct);
            this.gbReferralInformation.Controls.Add(this.mulDetails);
            this.gbReferralInformation.Controls.Add(this.lblDetails);
            this.gbReferralInformation.Controls.Add(this.dfDescription);
            this.gbReferralInformation.Controls.Add(this.lblDescription);
            this.gbReferralInformation.Controls.Add(this.dfDueTime);
            this.gbReferralInformation.Controls.Add(this.lblTime);
            this.gbReferralInformation.Controls.Add(this.dfDueDate);
            this.gbReferralInformation.Controls.Add(this.lblDueDate);
            this.gbReferralInformation.Controls.Add(this.dfReferralCode);
            this.gbReferralInformation.Controls.Add(this.lblReferralCode);
            this.gbReferralInformation.Controls.Add(this.cmbCategory);
            this.gbReferralInformation.Controls.Add(this.lblCategory);
            this.gbReferralInformation.Controls.Add(this.cbNewProspect);
            this.gbReferralInformation.Controls.Add(this.dfCustomer);
            this.gbReferralInformation.Controls.Add(this.lblCustomer);
            this.gbReferralInformation.Controls.Add(this.dfReferralId);
            this.gbReferralInformation.Controls.Add(this.lblReferralID);
            this.gbReferralInformation.Location = new System.Drawing.Point(0, 4);
            this.gbReferralInformation.Name = "gbReferralInformation";
            this.gbReferralInformation.PhoenixUIControl.ObjectId = 1;
            this.gbReferralInformation.Size = new System.Drawing.Size(680, 246);
            this.gbReferralInformation.TabIndex = 0;
            this.gbReferralInformation.TabStop = false;
            this.gbReferralInformation.Text = "Referral Information";
            // 
            // cmbQueue
            // 
            this.cmbQueue.Location = new System.Drawing.Point(97, 64);
            this.cmbQueue.Name = "cmbQueue";
            this.cmbQueue.PhoenixUIControl.ObjectId = 49;
            this.cmbQueue.PhoenixUIControl.XmlTag = "QueueId";
            this.cmbQueue.Size = new System.Drawing.Size(302, 21);
            this.cmbQueue.TabIndex = 6;
            this.cmbQueue.Value = null;
            this.cmbQueue.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbQueue_PhoenixUISelectedIndexChangedEvent);
            this.cmbQueue.PhoenixUIEnterEvent += new Phoenix.Windows.Forms.EnterEventHandler(this.cmbQueue_PhoenixUIEnterEvent);
            // 
            // lblQueue
            // 
            this.lblQueue.AutoEllipsis = true;
            this.lblQueue.Location = new System.Drawing.Point(8, 64);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.PhoenixUIControl.ObjectId = 49;
            this.lblQueue.Size = new System.Drawing.Size(74, 20);
            this.lblQueue.TabIndex = 5;
            this.lblQueue.Text = "Queue:";
            // 
            // cmbProductClass
            // 
            this.cmbProductClass.Location = new System.Drawing.Point(172, 216);
            this.cmbProductClass.Name = "cmbProductClass";
            this.cmbProductClass.PhoenixUIControl.ObjectId = 10;
            this.cmbProductClass.Size = new System.Drawing.Size(228, 21);
            this.cmbProductClass.TabIndex = 21;
            this.cmbProductClass.Value = null;
            this.cmbProductClass.Visible = false;
            // 
            // cmbProductAppl
            // 
            this.cmbProductAppl.Location = new System.Drawing.Point(97, 216);
            this.cmbProductAppl.Name = "cmbProductAppl";
            this.cmbProductAppl.PhoenixUIControl.ObjectId = 9;
            this.cmbProductAppl.Size = new System.Drawing.Size(64, 21);
            this.cmbProductAppl.TabIndex = 20;
            this.cmbProductAppl.Value = null;
            this.cmbProductAppl.Visible = false;
            this.cmbProductAppl.SelectedIndexChanged += new System.EventHandler(this.cmbProductAppl_SelectedIndexChanged);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoEllipsis = true;
            this.lblProduct.Location = new System.Drawing.Point(8, 220);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.PhoenixUIControl.ObjectId = 9;
            this.lblProduct.Size = new System.Drawing.Size(74, 20);
            this.lblProduct.TabIndex = 19;
            this.lblProduct.Text = "Product:";
            this.lblProduct.Visible = false;
            // 
            // mulDetails
            // 
            this.mulDetails.AcceptsReturn = true;
            this.mulDetails.Location = new System.Drawing.Point(96, 160);
            this.mulDetails.MaxLength = 254;
            this.mulDetails.Multiline = true;
            this.mulDetails.Name = "mulDetails";
            this.mulDetails.PhoenixUIControl.ObjectId = 8;
            this.mulDetails.PreviousValue = null;
            this.mulDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.mulDetails.Size = new System.Drawing.Size(580, 52);
            this.mulDetails.TabIndex = 18;
            this.mulDetails.PhoenixUIEnterEvent += new Phoenix.Windows.Forms.EnterEventHandler(this.mulDetails_PhoenixUIEnterEvent);
            this.mulDetails.PhoenixUILeaveEvent += new Phoenix.Windows.Forms.LeaveEventHandler(this.mulDetails_PhoenixUILeaveEvent);
            // 
            // lblDetails
            // 
            this.lblDetails.AutoEllipsis = true;
            this.lblDetails.Location = new System.Drawing.Point(8, 160);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.PhoenixUIControl.ObjectId = 8;
            this.lblDetails.Size = new System.Drawing.Size(70, 20);
            this.lblDetails.TabIndex = 17;
            this.lblDetails.Text = "Details:";
            // 
            // dfDescription
            // 
            this.dfDescription.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.Location = new System.Drawing.Point(97, 136);
            this.dfDescription.MaxLength = 80;
            this.dfDescription.Name = "dfDescription";
            this.dfDescription.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfDescription.PhoenixUIControl.ObjectId = 7;
            this.dfDescription.PreviousValue = null;
            this.dfDescription.Size = new System.Drawing.Size(302, 20);
            this.dfDescription.TabIndex = 16;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            this.lblDescription.Location = new System.Drawing.Point(8, 136);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.PhoenixUIControl.ObjectId = 7;
            this.lblDescription.Size = new System.Drawing.Size(70, 20);
            this.lblDescription.TabIndex = 15;
            this.lblDescription.Text = "Description:";
            // 
            // dfDueTime
            // 
            this.dfDueTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.Location = new System.Drawing.Point(328, 112);
            this.dfDueTime.Name = "dfDueTime";
            this.dfDueTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.TimeNoSeconds;
            this.dfDueTime.PhoenixUIControl.ObjectId = 48;
            this.dfDueTime.PreviousValue = null;
            this.dfDueTime.Size = new System.Drawing.Size(71, 20);
            this.dfDueTime.TabIndex = 14;
            // 
            // lblTime
            // 
            this.lblTime.AutoEllipsis = true;
            this.lblTime.Location = new System.Drawing.Point(284, 112);
            this.lblTime.Name = "lblTime";
            this.lblTime.PhoenixUIControl.ObjectId = 48;
            this.lblTime.Size = new System.Drawing.Size(38, 20);
            this.lblTime.TabIndex = 13;
            this.lblTime.Text = "Time:";
            // 
            // dfDueDate
            // 
            this.dfDueDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDate.Location = new System.Drawing.Point(97, 112);
            this.dfDueDate.Name = "dfDueDate";
            this.dfDueDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfDueDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfDueDate.PhoenixUIControl.ObjectId = 11;
            this.dfDueDate.PreviousValue = null;
            this.dfDueDate.Size = new System.Drawing.Size(84, 20);
            this.dfDueDate.TabIndex = 12;
            // 
            // lblDueDate
            // 
            this.lblDueDate.AutoEllipsis = true;
            this.lblDueDate.Location = new System.Drawing.Point(8, 112);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.Enable;
            this.lblDueDate.PhoenixUIControl.ObjectId = 11;
            this.lblDueDate.Size = new System.Drawing.Size(59, 20);
            this.lblDueDate.TabIndex = 11;
            this.lblDueDate.Text = "Due Date:";
            // 
            // dfReferralCode
            // 
            this.dfReferralCode.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfReferralCode.Location = new System.Drawing.Point(560, 88);
            this.dfReferralCode.Name = "dfReferralCode";
            this.dfReferralCode.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfReferralCode.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfReferralCode.PhoenixUIControl.ObjectId = 6;
            this.dfReferralCode.PreviousValue = null;
            this.dfReferralCode.Size = new System.Drawing.Size(90, 20);
            this.dfReferralCode.TabIndex = 10;
            // 
            // lblReferralCode
            // 
            this.lblReferralCode.AutoEllipsis = true;
            this.lblReferralCode.Location = new System.Drawing.Point(463, 88);
            this.lblReferralCode.Name = "lblReferralCode";
            this.lblReferralCode.PhoenixUIControl.ObjectId = 6;
            this.lblReferralCode.Size = new System.Drawing.Size(80, 20);
            this.lblReferralCode.TabIndex = 9;
            this.lblReferralCode.Text = "Referral Code:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(97, 88);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.PhoenixUIControl.ObjectId = 5;
            this.cmbCategory.PhoenixUIControl.XmlTag = "CategoryId";
            this.cmbCategory.Size = new System.Drawing.Size(302, 21);
            this.cmbCategory.TabIndex = 8;
            this.cmbCategory.Value = null;
            this.cmbCategory.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbCategory_PhoenixUISelectedIndexChangedEvent);
            // 
            // lblCategory
            // 
            this.lblCategory.AutoEllipsis = true;
            this.lblCategory.Location = new System.Drawing.Point(8, 88);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.PhoenixUIControl.ObjectId = 5;
            this.lblCategory.Size = new System.Drawing.Size(74, 20);
            this.lblCategory.TabIndex = 7;
            this.lblCategory.Text = "Category:";
            // 
            // cbNewProspect
            // 
            this.cbNewProspect.BackColor = System.Drawing.SystemColors.Control;
            this.cbNewProspect.Checked = true;
            this.cbNewProspect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbNewProspect.Location = new System.Drawing.Point(463, 40);
            this.cbNewProspect.Name = "cbNewProspect";
            this.cbNewProspect.PhoenixUIControl.ObjectId = 4;
            this.cbNewProspect.Size = new System.Drawing.Size(108, 20);
            this.cbNewProspect.TabIndex = 4;
            this.cbNewProspect.Text = "New Prospect";
            this.cbNewProspect.UseVisualStyleBackColor = false;
            this.cbNewProspect.Value = null;
            this.cbNewProspect.CheckedChanged += new System.EventHandler(this.cbNewProspect_CheckedChanged);
            // 
            // dfCustomer
            // 
            this.dfCustomer.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.Location = new System.Drawing.Point(97, 40);
            this.dfCustomer.MaxLength = 80;
            this.dfCustomer.Name = "dfCustomer";
            this.dfCustomer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfCustomer.PhoenixUIControl.ObjectId = 3;
            this.dfCustomer.PhoenixUIControl.XmlTag = "CustomerName";
            this.dfCustomer.PreviousValue = null;
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
            // dfReferralId
            // 
            this.dfReferralId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfReferralId.Location = new System.Drawing.Point(97, 16);
            this.dfReferralId.Name = "dfReferralId";
            this.dfReferralId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfReferralId.PhoenixUIControl.IsEnabled = Phoenix.Windows.Forms.EnableState.DisableShowText;
            this.dfReferralId.PhoenixUIControl.ObjectId = 2;
            this.dfReferralId.PreviousValue = null;
            this.dfReferralId.Size = new System.Drawing.Size(74, 20);
            this.dfReferralId.TabIndex = 1;
            // 
            // lblReferralID
            // 
            this.lblReferralID.AutoEllipsis = true;
            this.lblReferralID.Location = new System.Drawing.Point(8, 16);
            this.lblReferralID.Name = "lblReferralID";
            this.lblReferralID.PhoenixUIControl.ObjectId = 2;
            this.lblReferralID.Size = new System.Drawing.Size(80, 20);
            this.lblReferralID.TabIndex = 0;
            this.lblReferralID.Text = "Referral ID:";
            // 
            // gbReferralStatusInformation
            // 
            this.gbReferralStatusInformation.Controls.Add(this.rbExpired);
            this.gbReferralStatusInformation.Controls.Add(this.cmbReason);
            this.gbReferralStatusInformation.Controls.Add(this.lblReason);
            this.gbReferralStatusInformation.Controls.Add(this.cmbPriority);
            this.gbReferralStatusInformation.Controls.Add(this.lblPriority);
            this.gbReferralStatusInformation.Controls.Add(this.rbDeclined);
            this.gbReferralStatusInformation.Controls.Add(this.rbSold);
            this.gbReferralStatusInformation.Controls.Add(this.rbInProcess);
            this.gbReferralStatusInformation.Controls.Add(this.rbPending);
            this.gbReferralStatusInformation.Controls.Add(this.lblReferralStatus);
            this.gbReferralStatusInformation.Location = new System.Drawing.Point(0, 249);
            this.gbReferralStatusInformation.Name = "gbReferralStatusInformation";
            this.gbReferralStatusInformation.PhoenixUIControl.ObjectId = 19;
            this.gbReferralStatusInformation.Size = new System.Drawing.Size(680, 68);
            this.gbReferralStatusInformation.TabIndex = 1;
            this.gbReferralStatusInformation.TabStop = false;
            this.gbReferralStatusInformation.Text = "Referral Status Information";
            // 
            // rbExpired
            // 
            this.rbExpired.BackColor = System.Drawing.SystemColors.Control;
            this.rbExpired.Description = null;
            this.rbExpired.Location = new System.Drawing.Point(424, 16);
            this.rbExpired.Name = "rbExpired";
            this.rbExpired.PhoenixUIControl.ObjectId = 53;
            this.rbExpired.Size = new System.Drawing.Size(70, 20);
            this.rbExpired.TabIndex = 5;
            this.rbExpired.Text = "Expired";
            this.rbExpired.UseVisualStyleBackColor = false;
            this.rbExpired.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbExpired_PhoenixUICheckedChangedEvent);
            // 
            // cmbReason
            // 
            this.cmbReason.Location = new System.Drawing.Point(97, 40);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.PhoenixUIControl.IsNullWhenDisabled = Phoenix.Windows.Forms.PBoolState.True;
            this.cmbReason.PhoenixUIControl.ObjectId = 41;
            this.cmbReason.Size = new System.Drawing.Size(302, 21);
            this.cmbReason.TabIndex = 8;
            this.cmbReason.Value = null;
            // 
            // lblReason
            // 
            this.lblReason.AutoEllipsis = true;
            this.lblReason.Location = new System.Drawing.Point(8, 40);
            this.lblReason.Name = "lblReason";
            this.lblReason.PhoenixUIControl.ObjectId = 41;
            this.lblReason.Size = new System.Drawing.Size(72, 20);
            this.lblReason.TabIndex = 7;
            this.lblReason.Text = "Reason:";
            // 
            // cmbPriority
            // 
            this.cmbPriority.Location = new System.Drawing.Point(564, 16);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.PhoenixUIControl.ObjectId = 40;
            this.cmbPriority.Size = new System.Drawing.Size(90, 21);
            this.cmbPriority.TabIndex = 6;
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
            this.lblPriority.TabIndex = 5;
            this.lblPriority.Text = "Priority:";
            // 
            // rbDeclined
            // 
            this.rbDeclined.BackColor = System.Drawing.SystemColors.Control;
            this.rbDeclined.Description = null;
            this.rbDeclined.Location = new System.Drawing.Point(340, 16);
            this.rbDeclined.Name = "rbDeclined";
            this.rbDeclined.PhoenixUIControl.ObjectId = 22;
            this.rbDeclined.Size = new System.Drawing.Size(70, 20);
            this.rbDeclined.TabIndex = 4;
            this.rbDeclined.Text = "Declined";
            this.rbDeclined.UseVisualStyleBackColor = false;
            this.rbDeclined.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbDeclined_PhoenixUICheckedChangedEvent);
            // 
            // rbSold
            // 
            this.rbSold.BackColor = System.Drawing.SystemColors.Control;
            this.rbSold.Description = null;
            this.rbSold.Location = new System.Drawing.Point(266, 16);
            this.rbSold.Name = "rbSold";
            this.rbSold.PhoenixUIControl.ObjectId = 21;
            this.rbSold.Size = new System.Drawing.Size(84, 20);
            this.rbSold.TabIndex = 3;
            this.rbSold.Text = "Sold";
            this.rbSold.UseVisualStyleBackColor = false;
            this.rbSold.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbSold_PhoenixUICheckedChangedEvent);
            // 
            // rbInProcess
            // 
            this.rbInProcess.BackColor = System.Drawing.SystemColors.Control;
            this.rbInProcess.Description = null;
            this.rbInProcess.Location = new System.Drawing.Point(172, 16);
            this.rbInProcess.Name = "rbInProcess";
            this.rbInProcess.PhoenixUIControl.ObjectId = 47;
            this.rbInProcess.Size = new System.Drawing.Size(88, 20);
            this.rbInProcess.TabIndex = 2;
            this.rbInProcess.Text = "In Process";
            this.rbInProcess.UseVisualStyleBackColor = false;
            this.rbInProcess.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbInProcess_PhoenixUICheckedChangedEvent);
            // 
            // rbPending
            // 
            this.rbPending.BackColor = System.Drawing.SystemColors.Control;
            this.rbPending.Description = null;
            this.rbPending.IsMaster = true;
            this.rbPending.Location = new System.Drawing.Point(96, 16);
            this.rbPending.Name = "rbPending";
            this.rbPending.PhoenixUIControl.ObjectId = 20;
            this.rbPending.Size = new System.Drawing.Size(70, 20);
            this.rbPending.TabIndex = 1;
            this.rbPending.Text = "Pending";
            this.rbPending.UseVisualStyleBackColor = false;
            this.rbPending.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbPending_PhoenixUICheckedChangedEvent);
            // 
            // lblReferralStatus
            // 
            this.lblReferralStatus.AutoEllipsis = true;
            this.lblReferralStatus.Location = new System.Drawing.Point(8, 16);
            this.lblReferralStatus.Name = "lblReferralStatus";
            this.lblReferralStatus.PhoenixUIControl.ObjectId = 39;
            this.lblReferralStatus.Size = new System.Drawing.Size(86, 20);
            this.lblReferralStatus.TabIndex = 0;
            this.lblReferralStatus.Text = "Referral Status:";
            // 
            // dfProductSvcs
            // 
            this.dfProductSvcs.BackColor = System.Drawing.SystemColors.Control;
            this.dfProductSvcs.Controls.Add(this.pbRemoveProd);
            this.dfProductSvcs.Controls.Add(this.pbAdd);
            this.dfProductSvcs.Controls.Add(this.gbSelProdServices);
            this.dfProductSvcs.Controls.Add(this.gbAvailProdSer);
            this.dfProductSvcs.Location = new System.Drawing.Point(4, 22);
            this.dfProductSvcs.Name = "dfProductSvcs";
            this.dfProductSvcs.Size = new System.Drawing.Size(682, 478);
            this.dfProductSvcs.TabIndex = 3;
            this.dfProductSvcs.Text = "P&roducts and Services";
            // 
            // pbRemoveProd
            // 
            this.pbRemoveProd.Location = new System.Drawing.Point(267, 189);
            this.pbRemoveProd.Name = "pbRemoveProd";
            this.pbRemoveProd.PhoenixUIControl.ObjectId = 59;
            this.pbRemoveProd.Size = new System.Drawing.Size(75, 23);
            this.pbRemoveProd.TabIndex = 2;
            this.pbRemoveProd.Text = "Remove";
            this.pbRemoveProd.Click += new System.EventHandler(this.pbRemoveProd_Click);
            // 
            // pbAdd
            // 
            this.pbAdd.Location = new System.Drawing.Point(267, 160);
            this.pbAdd.Name = "pbAdd";
            this.pbAdd.PhoenixUIControl.ObjectId = 58;
            this.pbAdd.Size = new System.Drawing.Size(75, 23);
            this.pbAdd.TabIndex = 1;
            this.pbAdd.Text = "Add";
            this.pbAdd.Click += new System.EventHandler(this.pbAdd_Click);
            // 
            // gbSelProdServices
            // 
            this.gbSelProdServices.Controls.Add(this.grdSelProdSer);
            this.gbSelProdServices.Location = new System.Drawing.Point(343, 4);
            this.gbSelProdServices.Name = "gbSelProdServices";
            this.gbSelProdServices.Size = new System.Drawing.Size(336, 466);
            this.gbSelProdServices.TabIndex = 3;
            this.gbSelProdServices.TabStop = false;
            this.gbSelProdServices.Text = "Selected Product(s) and Services";
            // 
            // grdSelProdSer
            // 
            this.grdSelProdSer.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cbSelectedProd,
            this.colType,
            this.colQuantity,
            this.colSelDescription,
            this.colQuantityHidden,
            this.colProductGroupId,
            this.colServiceId,
            this.colApplType,
            this.colAcctType,
            this.colClassCode,
            this.colRmThrdType,
            this.colRmThirdIns});
            this.grdSelProdSer.IsDataGridReadOnly = false;
            this.grdSelProdSer.IsMaxNumRowsCustomized = false;
            this.grdSelProdSer.Location = new System.Drawing.Point(7, 18);
            this.grdSelProdSer.Name = "grdSelProdSer";
            this.grdSelProdSer.Size = new System.Drawing.Size(323, 442);
            this.grdSelProdSer.TabIndex = 0;
            this.grdSelProdSer.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdSelProdSer_BeforePopulate);
            this.grdSelProdSer.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdSelProdSer_FetchRowDone);
            this.grdSelProdSer.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdSelProdSer_SelectedIndexChanged);
            // 
            // cbSelectedProd
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cbSelectedProd.DefaultCellStyle = dataGridViewCellStyle1;
            this.cbSelectedProd.HeaderText = "";
            this.cbSelectedProd.Name = "cbSelectedProd";
            this.cbSelectedProd.PhoenixUIControl.ObjectId = 64;
            this.cbSelectedProd.Text = "";
            this.cbSelectedProd.Title = "";
            this.cbSelectedProd.Width = 25;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.PhoenixUIControl.ObjectId = 65;
            this.colType.PhoenixUIControl.XmlTag = "SelType";
            this.colType.Text = "";
            this.colType.Title = "Type";
            this.colType.Width = 50;
            // 
            // colQuantity
            // 
            this.colQuantity.HeaderText = "Quantity";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.PhoenixUIControl.ObjectId = 66;
            this.colQuantity.PhoenixUIControl.XmlTag = "Quantity";
            this.colQuantity.Text = "";
            this.colQuantity.Title = "Quantity";
            this.colQuantity.Width = 70;
            // 
            // colSelDescription
            // 
            this.colSelDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSelDescription.HeaderText = "Description";
            this.colSelDescription.Name = "colSelDescription";
            this.colSelDescription.PhoenixUIControl.ObjectId = 67;
            this.colSelDescription.PhoenixUIControl.XmlTag = "NoteTitle";
            this.colSelDescription.Text = "";
            this.colSelDescription.Title = "Description";
            // 
            // colQuantityHidden
            // 
            this.colQuantityHidden.HeaderText = "colQuantityHidden";
            this.colQuantityHidden.Name = "colQuantityHidden";
            this.colQuantityHidden.PhoenixUIControl.ObjectId = 68;
            this.colQuantityHidden.PhoenixUIControl.XmlTag = "Quantity";
            this.colQuantityHidden.Text = "";
            this.colQuantityHidden.Title = "colQuantityHidden";
            this.colQuantityHidden.Visible = false;
            // 
            // colProductGroupId
            // 
            this.colProductGroupId.HeaderText = "colProductGroupId";
            this.colProductGroupId.Name = "colProductGroupId";
            this.colProductGroupId.PhoenixUIControl.ObjectId = 69;
            this.colProductGroupId.Text = "";
            this.colProductGroupId.Title = "colProductGroupId";
            this.colProductGroupId.Visible = false;
            // 
            // colServiceId
            // 
            this.colServiceId.HeaderText = "colServiceId";
            this.colServiceId.Name = "colServiceId";
            this.colServiceId.PhoenixUIControl.ObjectId = 70;
            this.colServiceId.Text = "";
            this.colServiceId.Title = "colServiceId";
            this.colServiceId.Visible = false;
            // 
            // colApplType
            // 
            this.colApplType.HeaderText = "colApplType";
            this.colApplType.Name = "colApplType";
            this.colApplType.PhoenixUIControl.ObjectId = 71;
            this.colApplType.Text = "";
            this.colApplType.Title = "colApplType";
            this.colApplType.Visible = false;
            // 
            // colAcctType
            // 
            this.colAcctType.HeaderText = "colAcctType";
            this.colAcctType.Name = "colAcctType";
            this.colAcctType.PhoenixUIControl.ObjectId = 72;
            this.colAcctType.Text = "";
            this.colAcctType.Title = "colAcctType";
            this.colAcctType.Visible = false;
            // 
            // colClassCode
            // 
            this.colClassCode.HeaderText = "colClassCode";
            this.colClassCode.Name = "colClassCode";
            this.colClassCode.PhoenixUIControl.ObjectId = 73;
            this.colClassCode.Text = "";
            this.colClassCode.Title = "colClassCode";
            this.colClassCode.Visible = false;
            // 
            // colRmThrdType
            // 
            this.colRmThrdType.HeaderText = "colRmThrdType";
            this.colRmThrdType.Name = "colRmThrdType";
            this.colRmThrdType.Text = "";
            this.colRmThrdType.Title = "colRmThrdType";
            this.colRmThrdType.Visible = false;
            // 
            // colRmThirdIns
            // 
            this.colRmThirdIns.HeaderText = "colRmThirdIns";
            this.colRmThirdIns.Name = "colRmThirdIns";
            this.colRmThirdIns.Text = "";
            this.colRmThirdIns.Title = "colRmThirdIns";
            this.colRmThirdIns.Visible = false;
            // 
            // gbAvailProdSer
            // 
            this.gbAvailProdSer.Controls.Add(this.grdAvailProd);
            this.gbAvailProdSer.Controls.Add(this.cmbApplType);
            this.gbAvailProdSer.Controls.Add(this.cmbType);
            this.gbAvailProdSer.Controls.Add(this.lblApplType);
            this.gbAvailProdSer.Controls.Add(this.lblType);
            this.gbAvailProdSer.Location = new System.Drawing.Point(4, 4);
            this.gbAvailProdSer.Name = "gbAvailProdSer";
            this.gbAvailProdSer.PhoenixUIControl.ObjectId = 54;
            this.gbAvailProdSer.Size = new System.Drawing.Size(257, 466);
            this.gbAvailProdSer.TabIndex = 0;
            this.gbAvailProdSer.TabStop = false;
            this.gbAvailProdSer.Text = "Available Product(s) and Services";
            // 
            // grdAvailProd
            // 
            this.grdAvailProd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroupId,
            this.colDescription,
            this.colRm3rdType,
            this.colRm3rdIns});
            this.grdAvailProd.IsDataGridReadOnly = false;
            this.grdAvailProd.IsMaxNumRowsCustomized = false;
            this.grdAvailProd.Location = new System.Drawing.Point(7, 83);
            this.grdAvailProd.MultiSelect = false;
            this.grdAvailProd.Name = "grdAvailProd";
            this.grdAvailProd.Size = new System.Drawing.Size(240, 377);
            this.grdAvailProd.TabIndex = 4;
            this.grdAvailProd.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.grdAvailProd_BeforePopulate);
            // 
            // colGroupId
            // 
            this.colGroupId.HeaderText = "colGroupId";
            this.colGroupId.Name = "colGroupId";
            this.colGroupId.PhoenixUIControl.ObjectId = 62;
            this.colGroupId.PhoenixUIControl.XmlTag = "GroupId";
            this.colGroupId.Text = "";
            this.colGroupId.Title = "colGroupId";
            this.colGroupId.Visible = false;
            // 
            // colDescription
            // 
            this.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescription.HeaderText = "Description";
            this.colDescription.Name = "colDescription";
            this.colDescription.PhoenixUIControl.ObjectId = 63;
            this.colDescription.PhoenixUIControl.XmlTag = "Des";
            this.colDescription.Text = "";
            this.colDescription.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.colDescription.Title = "Description";
            // 
            // colRm3rdType
            // 
            this.colRm3rdType.HeaderText = "colRm3rdType";
            this.colRm3rdType.Name = "colRm3rdType";
            this.colRm3rdType.PhoenixUIControl.XmlTag = "Rim3rdTypeId";
            this.colRm3rdType.Text = "";
            this.colRm3rdType.Title = "colRm3rdType";
            this.colRm3rdType.Visible = false;
            // 
            // colRm3rdIns
            // 
            this.colRm3rdIns.HeaderText = "pDataGridViewColumn1";
            this.colRm3rdIns.Name = "colRm3rdIns";
            this.colRm3rdIns.PhoenixUIControl.XmlTag = "Rim3rdInsId";
            this.colRm3rdIns.Text = "";
            this.colRm3rdIns.Title = "pDataGridViewColumn1";
            this.colRm3rdIns.Visible = false;
            // 
            // cmbApplType
            // 
            this.cmbApplType.Location = new System.Drawing.Point(100, 49);
            this.cmbApplType.Name = "cmbApplType";
            this.cmbApplType.PhoenixUIControl.ObjectId = 56;
            this.cmbApplType.PhoenixUIControl.XmlTag = "ProdApplType";
            this.cmbApplType.Size = new System.Drawing.Size(151, 21);
            this.cmbApplType.TabIndex = 3;
            this.cmbApplType.Value = null;
            this.cmbApplType.SelectedIndexChanged += new System.EventHandler(this.cmbApplType_SelectedIndexChanged);
            // 
            // cmbType
            // 
            this.cmbType.Location = new System.Drawing.Point(100, 18);
            this.cmbType.Name = "cmbType";
            this.cmbType.PhoenixUIControl.ObjectId = 55;
            this.cmbType.PhoenixUIControl.XmlTag = "ProdType";
            this.cmbType.Size = new System.Drawing.Size(151, 21);
            this.cmbType.TabIndex = 1;
            this.cmbType.Value = null;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // lblApplType
            // 
            this.lblApplType.AutoEllipsis = true;
            this.lblApplType.Location = new System.Drawing.Point(7, 49);
            this.lblApplType.Name = "lblApplType";
            this.lblApplType.PhoenixUIControl.ObjectId = 56;
            this.lblApplType.Size = new System.Drawing.Size(97, 20);
            this.lblApplType.TabIndex = 2;
            this.lblApplType.Text = "Application Type:";
            // 
            // lblType
            // 
            this.lblType.AutoEllipsis = true;
            this.lblType.Location = new System.Drawing.Point(7, 20);
            this.lblType.Name = "lblType";
            this.lblType.PhoenixUIControl.ObjectId = 55;
            this.lblType.Size = new System.Drawing.Size(87, 20);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Type:";
            // 
            // dfTabTitle2
            // 
            this.dfTabTitle2.BackColor = System.Drawing.Color.Transparent;
            this.dfTabTitle2.Location = new System.Drawing.Point(4, 22);
            this.dfTabTitle2.MLInfo = controlInfo2;
            this.dfTabTitle2.Name = "dfTabTitle2";
            this.dfTabTitle2.Size = new System.Drawing.Size(682, 478);
            this.dfTabTitle2.TabIndex = 2;
            this.dfTabTitle2.Text = "Additional";
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
            this.dfRimNo.PreviousValue = null;
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
            this.dfProductAcctType.PreviousValue = null;
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
            this.dfProductClassCode.PreviousValue = null;
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
            this.dfPrioritySort.PreviousValue = null;
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
            this.dfRecordType.PreviousValue = null;
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
            this.dfPrevEmplId.PreviousValue = null;
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
            this.dfQueueId.PreviousValue = null;
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
            this.dfLastWorkDt.PreviousValue = null;
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
            this.dfDueDateWithTime.PreviousValue = null;
            this.dfDueDateWithTime.Size = new System.Drawing.Size(52, 20);
            this.dfDueDateWithTime.TabIndex = 0;
            this.dfDueDateWithTime.Visible = false;
            // 
            // pbRemove
            // 
            this.pbRemove.LongText = "Remove";
            this.pbRemove.Name = "pbRemove";
            this.pbRemove.ObjectId = 51;
            this.pbRemove.ShortText = "Remove";
            this.pbRemove.Tag = null;
            this.pbRemove.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRemove_Click);
            // 
            // frmReferralEdit
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(908, 530);
            this.Controls.Add(this.picTabs);
            this.Name = "frmReferralEdit";
            this.ResolutionType = Phoenix.Windows.Forms.PResolutionType.Size1024x768;
            this.ScreenId = 12503;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmReferralEdit_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmReferralEdit_PInitCompleteEvent);
            this.PBeforeSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmReferralEdit_PBeforeSave);
            this.PAfterSave += new Phoenix.Windows.Forms.FormActionHandler(this.frmReferralEdit_PAfterSave);
            this.PAfterActionEvent += new Phoenix.Windows.Forms.FormActionHandler(this.frmReferralEdit_PAfterActionEvent);//Task #74463
            this.picTabs.ResumeLayout(false);
            this.dfTabTitle0.ResumeLayout(false);
            this.pGroupBoxStandard2.ResumeLayout(false);
            this.pGroupBoxStandard2.PerformLayout();
            this.pGroupBoxStandard1.ResumeLayout(false);
            this.pGroupBoxStandard1.PerformLayout();
            this.gbReferralInformation.ResumeLayout(false);
            this.gbReferralInformation.PerformLayout();
            this.gbReferralStatusInformation.ResumeLayout(false);
            this.dfProductSvcs.ResumeLayout(false);
            this.gbSelProdServices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdSelProdSer)).EndInit();
            this.gbAvailProdSer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdAvailProd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private enum ResponseTypeIdList
        {
            ReferralEdit = 13,
            ProductGroupDetail = 15,
            ProductGroupDetSelected = 16,
        }
        #region Init
        public override void InitParameters(params Object[] paramList)
        {
            #region 6139 - 21482
            if (_parentForm == null)
            {
                _parentForm = new PfwStandard();
                _parentForm.Name = string.Empty;
            }
            #endregion
            if (paramList.Length == 8)
            {
                if (paramList[0] != null)           //ITSC Issue #01698 //
                {
                    _parentForm = (Form)paramList[0];
                }

                this._gbWorkQueue.Type.Value = paramList[1].ToString().Trim();


                if (paramList[3] != null)
                    this._gbWorkQueue.RimNo.Value = Convert.ToInt32(paramList[3]);
                //WI#17953
                //if (paramList[4] != null) //#16252
                //	{
                //Begin #15229
                //if (paramList[5] != null || paramList[4] != null) #16252

                if (_parentForm is PfwStandard && (  _parentForm as PfwStandard).FormSource == "FRMONSALES")
                {
                    if (paramList[2] != null)
                        _gbWorkQueue.ApplType.Value = Convert.ToString(paramList[2]);

                    if (paramList[4] != null)
                        type = Convert.ToString(paramList[4]);

                    if (paramList[5] != null)
                        this._gbWorkQueue.Description.Value = Convert.ToString(paramList[5]);
                }
                else
                {
                    if (paramList[2] != null)
                        this._gbWorkQueue.WorkId.Value = Convert.ToInt32(paramList[2]);

                    if (paramList[4] != null)
                        this._gbWorkQueue.ReferralCode.Value = Convert.ToString(paramList[4]);

                    if (paramList[5] != null)
                        this._gbWorkQueue.CustomerName.Value = Convert.ToString(paramList[5]);
                }
                //   }//WI#17953
                //End #15229
                if (paramList[6] != null)
                {
                    this._gbWorkQueue.AcctType.Value = Convert.ToString(paramList[6]);
                    sClass = Convert.ToString(paramList[6]);
                }

                if (paramList[7] != null)
                    this._gbWorkQueue.ClassCode.Value = Convert.ToInt16(paramList[7]);
                #region Bug Fixing
                if (Phoenix.Shared.Variables.GlobalVars.Module == "Teller")
                {
                    this.ScreenId = 12503;

                }
                else if (Phoenix.Shared.Variables.GlobalVars.Module == "AcProc")
                {
                    this.ScreenId = Phoenix.Shared.Constants.ScreenId.ReferralEdit - Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset;

                }
                this._gbWorkQueue.OutputType.Value = this.ScreenId;
                #endregion

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
            /* 21482 - moved it to beginning of this function
          if (_parentForm == null)
            {
                _parentForm = new PfwStandard();
                _parentForm.Name = string.Empty;
            } */
            #endregion
            InitXmlTags();

            base.InitParameters(paramList); //#19058

        }
        private void InitXmlTags()
        {
            //TODO: Verify the XmlTag Mapping
            //this.dfReferralId.PhoenixUIControl.XmlTag = "WorkId";
            this.dfCustomer.PhoenixUIControl.XmlTag = "CustomerName";
            this.cbNewProspect.PhoenixUIControl.XmlTag = "NewProspect";
            this.cmbQueue.PhoenixUIControl.XmlTag = "QueueId";
            this.cmbCategory.PhoenixUIControl.XmlTag = "CategoryId";
            this.dfReferralCode.PhoenixUIControl.XmlTag = "ReferralCode";
            this.dfDescription.PhoenixUIControl.XmlTag = "Description";
            this.mulDetails.PhoenixUIControl.XmlTag = "Text1";
            this.cmbProductAppl.PhoenixUIControl.XmlTag = "ApplType";
            this.cmbProductClass.PhoenixUIControl.XmlTag = "ProductClass";
            //this.cmbStatus.PhoenixUIControl.XmlTag = "Status";
            //this.dfEffectiveDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.cmbContactMethod.PhoenixUIControl.XmlTag = "ContactMethod";
            this.dfContactName.PhoenixUIControl.XmlTag = "ContactName";
            this.dfOtherInfo.PhoenixUIControl.XmlTag = "OtherInfo";
            this.cmbOwner.PhoenixUIControl.XmlTag = "OwnerEmplId";
            this.cbEmailOwner.PhoenixUIControl.XmlTag = "EmailOwner";
            this.rbPending.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbDeclined.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbSold.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbInProcess.PhoenixUIControl.XmlTag = "WorkStatus";
            this.rbExpired.PhoenixUIControl.XmlTag = "WorkStatus";//174961
            this.cmbPriority.PhoenixUIControl.XmlTag = "Priority";
            this.cmbReason.PhoenixUIControl.XmlTag = "ReasonId";
            this.dfRimNo.PhoenixUIControl.XmlTag = "RimNo";
            this.dfProductAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.dfProductClassCode.PhoenixUIControl.XmlTag = "ClassCode";
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
        private Phoenix.Windows.Forms.ReturnType frmReferralEdit_PInitBeginEvent()
        {
            this.UseStateFromBusinessObject = true; // 174961
            this.MainBusinesObject = _gbWorkQueue;
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

            if (!this._gbWorkQueue.ReferralCode.IsNull)
                this.dfReferralCode.UnFormattedValue = this._gbWorkQueue.ReferralCode.Value;

            _gbWorkQueue.ResponseTypeId = (int)ResponseTypeIdList.ReferralEdit; //WI37589

            //#6683
            if (BusGlobalVars.Module == "Teller")
            {
                int screenId = ScreenId > Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset ? ScreenId - Phoenix.Shared.Constants.ScreenId.PortedScreenIdOffset : ScreenId;
                if (CoreService.UIAccessProvider.GetScreenAccess(screenId) == AuthorizationType.NoDefinition)
                {
                    // If the screen security is not defined or coded in the Admin Tree then do not force security on Save button
                    this.ActionSave.NextScreenId = 0;

                }
                (this.Extension as Phoenix.Shared.Windows.FormExtension).ResizeFormEnd += new EventHandler(frmReferralEdit_ResizeFormEnd);//#38646
            }
            /* Constraint move from server side of bus obj, WI-15234 */
            _gbWorkQueue.WorkStatus.Constraint = new Phoenix.FrameWork.BusFrame.Constraint(43086, true);

            //Begin #34896
            this.dfTabTitle0.UseVisualStyleBackColor = false;
            this.dfTabTitle2.UseVisualStyleBackColor = false;
            if (Workspace is PwksWindow && (Workspace as PwksWindow).IsHighResWorkspace)
            {
                (this.Extension as Phoenix.Shared.Windows.FormExtension).ResizeFormEnd += new EventHandler(frmReferralEdit_ResizeFormEnd);
                //frmReferralEdit_ResizeFormEnd(null, null);
            }
            //End 34896

            return ReturnType.Success;
        }

        //Begin #34896
        void frmReferralEdit_ResizeFormEnd(object sender, EventArgs e)
        {
            if (Workspace == null)
                return;

            int grpBoxHeight = (Workspace as PwksWindow).Height - 120;
            gbAvailProdSer.Height = grpBoxHeight;
            grdAvailProd.Height = gbAvailProdSer.Height - 90;
            gbSelProdServices.Location = new Point(gbSelProdServices.Location.X, gbAvailProdSer.Location.Y);
            gbSelProdServices.Height = grpBoxHeight;
            grdSelProdSer.Height = gbSelProdServices.Height - 25;
            pbAdd.Location = new Point(gbAvailProdSer.Right + (gbSelProdServices.Left - gbAvailProdSer.Right - pbAdd.Width) / 2,
                gbAvailProdSer.Height / 2 - 20);
            pbRemoveProd.Location = new Point(pbAdd.Location.X, pbAdd.Location.Y + 30);
            ResizeEmbeddedWinCntrls();
            /*Begin #38646*/
            if (BusGlobalVars.Module == "Teller")
            {
                mulDetails.Height = 30;
                //picTabs.Location = new Point(picTabs.Location.X, picTabs.Location.Y - 2);
                gbReferralInformation.Location = new Point(gbReferralInformation.Location.X, gbReferralInformation.Location.Y - 1);
                gbReferralInformation.Height = 198;
                gbReferralStatusInformation.Location = new Point(gbReferralInformation.Location.X, gbReferralInformation.Location.Y + 199);
                gbReferralStatusInformation.Height = gbReferralStatusInformation.Height - 1;
                pGroupBoxStandard1.Location = new Point(gbReferralStatusInformation.Location.X, gbReferralStatusInformation.Location.Y + 66);
                pGroupBoxStandard2.Location = new Point(pGroupBoxStandard1.Location.X, pGroupBoxStandard1.Location.Y + 92);
                pGroupBoxStandard2.Height = 68;
            }
            /*End  #38646*/
        }

        private void ResizeEmbeddedWinCntrls()
        {
            if (Workspace == null)
                return;

            if (frmUserDefFieldsEdit != null && Workspace is PwksWindow && (Workspace as PwksWindow).IsHighResWorkspace)
            {
                frmUserDefFieldsEdit.grdValues.Height = dfProductSvcs.Height - 10;
                frmUserDefFieldsEdit.grdValues.Width = dfProductSvcs.Width - 10;
            }
        }
        //End #34896

        private void frmReferralEdit_PInitCompleteEvent()
        {
            //this.cmbCategory.PhoenixUIControl.UseStateFromObject = true;
            //Begin #15229
            PopulateDefaultCombos();

            this.cmbProductAppl.SetValueAndSelect(_gbWorkQueue.ApplType.Value, true);

            if (type == "External")
            {
                this.cmbProductAppl.SetValueAndSelect("EXT", true);
                string description = _gbWorkQueue.AcctType.Value + " - " + _gbWorkQueue.Description.Value;
                this.cmbProductClass.SetValueAndSelect(description, true);
            }

            if (type != "External" && type != "")// #15229
            {
                string sType = sClass.Substring(0, sClass.IndexOf('-'));
                string sCode = sClass.Substring(sClass.IndexOf('-') + 1);
                sType = sType.Trim();
                sCode = sCode.Trim();
                string description = sType + " - " + sCode + " - " + _gbWorkQueue.Description.Value;
                this.cmbProductClass.SetValueAndSelect(description, true);
            }
            //End #15229

            if (this.IsNew)
            {
                this._gbWorkQueue.CanChangeOwner.Value = "Y";
            }
            else
            {
                this._gbWorkQueue.CanChangeOwner.Value = this._gbWorkQueue.CheckCanChgOwner();	//Load the canchangeowner property in busobj
                //Set the key.
                if (_gbWorkQueue.WorkId.Value > 0) // #15229
                    this.dfReferralId.UnFormattedValue = this._gbWorkQueue.WorkId.Value;

                if (!this._gbWorkQueue.ClassDesc.IsNull)
                {
                    this.cmbProductClass.SetValueAndSelect(this._gbWorkQueue.ClassDesc.Value, true);
                }
            }

            LocCheckPMA();
            //#06057//if (!this._gbWorkQueue.RimNo.IsNull)
            if (!this._gbWorkQueue.RimNo.IsNull && _gbWorkQueue.RimNo.Value != 0)
            {
                this.dfCustomer.UnFormattedValue = this._gbWorkQueue.GetCustomerName(this._gbWorkQueue.RimNo.Value);
            }
            //Begin #22291
            //else
            //{
            //    dfCustomer.UnFormattedValue = null;
            //}
            //End #22291
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

            /*Begin 174961*/
            HideProductControls();
            /*End 174961*/

        }

        /// <summary>
        /// Hiding the Product and class controls.WI #35638
        /// </summary>
        private void HideProductControls()
        {
            cmbProductAppl.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
            cmbProductClass.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
            lblProduct.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);
        }

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
        /*
		private void cmbCategory_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			PopOwnerNames();
			int nDefCatEmplId = Convert.ToInt32(this._gbWorkQueue.GetWQCategoryDetails(Phoenix.BusObj.Global.GbWorkQueue.WqDetailsValueCode.CatEmplId));

            if (nDefCatEmplId > 0)
			{
				this.cmbOwner.SetValueAndSelect(nDefCatEmplId, true);
				LocCheckPMA();
			}
            LocCanChangeOwner();//#77295-aHussein
		}
*/
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
        private void cmbProductAppl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.cmbProductAppl.Text.Trim() == "<none>" || this.cmbProductAppl.Text.Trim() == string.Empty)
            {
                this.cmbProductClass.Items.Clear();
                return;
            }
            this.cmbProductAppl.ScreenToObject();
            PopProductClass();
        }

        /*private void cmbQueue_SelectedIndexChanged(object sender, EventArgs e)
        {
            Phoenix.Windows.Client.Helper.PopulateCombo(cmbCategory, _gbWorkQueue, _gbWorkQueue.CategoryId);
            return;
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
        }
        //end Issue #73762

        #region Checkbox Events
        private void cbNewProspect_CheckedChanged(object sender, System.EventArgs e)
        {
            SetCustomerDefaults();
        }
        #endregion

        #region Radio Button Events
        private void rbPending_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
        {
            this._gbWorkQueue.WorkStatus.Value = "Pending";
            _gbWorkQueue.ExpiryDt.SetValueToNull(); /*35569*/
            RefreshReason();
        }

        private void rbInProcess_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
        {
            this._gbWorkQueue.WorkStatus.Value = "InProcess";
            _gbWorkQueue.ExpiryDt.SetValueToNull(); /*35569*/
            RefreshReason();
        }

        private void rbSold_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
        {
            this._gbWorkQueue.WorkStatus.Value = "Sold";
            _gbWorkQueue.ExpiryDt.SetValueToNull(); /*35569*/
            RefreshReason();
        }

        private void rbDeclined_PhoenixUICheckedChangedEvent(object sender, System.EventArgs e)
        {
            this._gbWorkQueue.WorkStatus.Value = "Declined";
            _gbWorkQueue.ExpiryDt.SetValueToNull();/*35569*/
            RefreshReason();
        }
        private void rbExpired_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            this._gbWorkQueue.WorkStatus.Value = "Expired";
            /*Begin 35569 */
            _gbWorkQueue.ExpiryDt.Value = Phoenix.FrameWork.Shared.Variables.GlobalVars.SystemDate;
            /*End 35569 */
            RefreshReason();
        }
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
        private void frmReferralEdit_PBeforeSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
        {

            if (!this.IsNew && (rbSold.Checked || rbDeclined.Checked))
            {
                if (this._gbWorkQueue.TotalNotes.Value > 0)
                {
                    PMessageBox.Show(this, 319959, MessageType.Warning, MessageBoxButtons.OK);
                    //319959 - The referral status cannot be set to \'Sold\' or \'Declined\' when pending tasks exist for this referral record.

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

            #region Set the Rim No as needed
            if (this.cbNewProspect.Checked)
                this._gbWorkQueue.RimNo.Value = int.MinValue;
            #endregion

            #region Parse and the product acct_type and class_Code
            if (this.cmbProductClass.Text.Trim() != string.Empty)
            {
                LocParseProductClass(this.cmbProductClass.Text.Trim());
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
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbSold.Checked ? "Sold" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbDeclined.Checked ? "Declined" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbExpired.Checked ? "Expired" : string.Empty) : sTmpWorkStatus); //35569

                this._gbWorkQueue.SendWQMail(this.IsNew,
                                            dfCustomer.Text.Trim(),
                                            dfDescription.Text.Trim(),
                                            mulDetails.Text.Trim(),
                                            (cmbOwner.Text.Trim() == string.Empty ? string.Empty : cmbOwner.CodeValue.ToString()),
                                            cmbOwner.Text.Trim(),
                                            cmbCategory.Text.Trim(),
                                            sTmpWorkStatus,
                                            cmbPriority.Text.Trim(),
                                            "Referral");

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
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbSold.Checked ? "Sold" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbDeclined.Checked ? "Declined" : string.Empty) : sTmpWorkStatus);
                sTmpWorkStatus = (sTmpWorkStatus == string.Empty ? (rbExpired.Checked ? "Expired" : string.Empty) : sTmpWorkStatus); //35569

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
                                            "Referral");

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
        private void frmReferralEdit_PAfterSave(object sender, Phoenix.Windows.Forms.FormActionEventArgs e)
        {
            if (this.wasFormNew)
            {
                this.dfReferralId.UnFormattedValue = this._gbWorkQueue.WorkId.Value;
                // Save previous Value for this disabled field
                this.dfReferralId.SavePrevValue(false);
            }
            #region #01698 - Adjust Get Customer Enable / Disable Status After Save Operation
            //SetGetRimButtonState();
            SetTabAssociates(picTabs.SelectedIndex);
            #endregion
            bRefreshParent = true;


        }
        /* Begin - Task #74463 */
        protected void frmReferralEdit_PAfterActionEvent(object sender, FormActionEventArgs e)//Ashok
        {
            if (e != null && e.ActionType == XmActionType.New || e.ActionType == XmActionType.Update)
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

            /* Begin #42231 */
            if (grdSelProdSer.RowCount <= 0)
            {
                /* 15398 - A \'Product\', \'Service\' or \'Product Group\' must be selected.*/
                PMessageBox.Show(this, 15398, MessageType.Error, MessageBoxButtons.OK);
                picTabs.SelectTab(1);
                cmbType.Focus();
                return false;
            }
            /* End #42231 */

            _gbWorkQueue.ResponseTypeId = (int)ResponseTypeIdList.ReferralEdit;//WI37589
            _gbWorkQueue.InteractiveMessages.Reset(); //Task #74463
            while (true) //Task #74463
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
                //Checking whether Additional info tab edited if edited append BO object to process
                //108676 - Added frmUserDefFieldsEdit null validation
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
                        //108676 - Added frmUserDefFieldsEdit null validation
                        if (this.frmUserDefFieldsEdit != null && this.frmUserDefFieldsEdit.BoToProcessList != null && this.frmUserDefFieldsEdit.BoToProcessList.Count > 0)
                        {
                            this._BoToProcessList = new List<WqUserDefVal>();
                            _BoToProcessList.AddRange(this.frmUserDefFieldsEdit.BoToProcessList);
                        }
                    }
                    if (quitProcessing || responseCollectedCount == 0)
                        break;
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
                HideProductControls(); //WI #35638
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
                            this.cbNewProspect.Checked = false;
                            this.dfCustomer.Enabled = false;
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
                    return;
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
                    //#6057 //
                    if (Phoenix.Shared.Variables.GlobalVars.Module != "AcProc")
                    {
                        if (this._gbWorkQueue.CustomerName.IsNull)
                            this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.Enable);
                        else
                            this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.DisableShowText);
                    }
                    if (this._gbWorkQueue.CanChangeOwner.Value == "Y")
                        this.cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    else
                        this.cmbOwner.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);

                    SetGetRimButtonState();

                    SetRadioButtonDefaults();

                    if (_parentForm != null)            //ITSC Issue #01698 //
                    {
                        if (this._parentForm.Name == "frmOnTicklers")
                        {
                            this.cbNewProspect.Checked = true;
                        }

                    }
                    /* Begin #140811 Hiding Remove Button.*/
                    this.pbRemove.SetObjectStatus(VisibilityState.Hide, EnableState.Default);
                    /*End #140811*/
                    SetCustomerDefaults();
                    WorkStatusClick();
                    #endregion
                    break;
                case "WorkStatusClick":
                    #region Workstatus Logic
                    WorkStatusClick();
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

        private void SetCustomerDefaults()
        {
            if (this.cbNewProspect.Checked)
                this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.Enable);
            else
                this.dfCustomer.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.DisableShowText);
        }
        private void SetRadioButtonDefaults()
        {
            if (_parentForm != null)
            {
                if (this._parentForm.Name == "frmOnMessenger")
                {
                    this.rbPending.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    this.rbInProcess.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);
                    this.rbSold.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);
                    this.rbDeclined.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText);
                    this.rbExpired.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.DisableShowText); //35569

                }
                else
                {
                    this.rbPending.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    this.rbInProcess.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    this.rbSold.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    this.rbDeclined.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    this.rbExpired.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable); //35569

                }
            }
            else
            {
                this.rbPending.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                this.rbInProcess.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                this.rbSold.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                this.rbDeclined.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                this.rbExpired.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);//35569
            }

            if (this.IsNew)
            {
                this.rbPending.Checked = true;
                this.rbPending.ScreenToObject();
            }

        }
        private void PopulateDefaultCombos()
        {
            //Pop the appl type
            Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.ApplType);
            cmbProductAppl.Populate(_gbWorkQueue.ApplType.Constraint.EnumValues);
            //Pop the owner names
            PopOwnerNames();
            return;
        }
        private void RefreshReason()
        {
            EnableDisableVisibleLogic("WorkStatusClick");

            if (rbPending.Checked || rbInProcess.Checked)
            {
                this.cmbReason.Items.Clear();
            }
            else
            {
                Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.ReasonId);
                cmbReason.Populate(_gbWorkQueue.ReasonId.Constraint.EnumValues);
            }
        }

        private void PopOwnerNames()
        {
            //Pop the owner names
            Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.OwnerEmplId);
            //cmbOwner.Populate(_gbWorkQueue.OwnerEmplId.Constraint.EnumValues);
            cmbOwner.Repopulate(true);
        }

        private void PopProductClass()
        {
            //Pop the product class
            Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_gbWorkQueue, _gbWorkQueue.ProductClass);
            cmbProductClass.Populate(_gbWorkQueue.ProductClass.Constraint.EnumValues);
        }

        private void WorkStatusClick()
        {
            if (rbPending.Checked || rbInProcess.Checked)
                this.cmbReason.SetObjectStatus(NullabilityState.Null, VisibilityState.Show, EnableState.Disable);
            else
                this.cmbReason.SetObjectStatus(NullabilityState.Null, VisibilityState.Show, EnableState.Enable);

            return;
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

        private void LocParseProductClass(string psClassDesc)
        {
            psClassDesc = psClassDesc.Trim();

            if (psClassDesc == string.Empty)
                return;

            if (this.cmbProductAppl.Text.Trim().ToUpper() == "EXT")
            {
                //Example Source String: "BLR - Business Loan Referral"
                string[] psaTemp = psClassDesc.Split('-');
                if (psaTemp[0].Trim() != string.Empty)
                    this._gbWorkQueue.AcctType.Value = psaTemp[0].Trim();

                this._gbWorkQueue.ClassCode.Value = short.MinValue;

                if (psaTemp[1].Trim() != string.Empty)
                    this._gbWorkQueue.ClassDesc.Value = psaTemp[1].Trim();
            }
            else
            {
                //Example Source String: "DDA - 101 - Consumer Checking"
                string[] psaTemp = psClassDesc.Split('-');

                if (psaTemp[0].Trim() != string.Empty)
                    this._gbWorkQueue.AcctType.Value = psaTemp[0].Trim();

                if (psaTemp[1].Trim() != string.Empty)
                    this._gbWorkQueue.ClassCode.Value = Convert.ToInt16(psaTemp[1].Trim());

                if (psaTemp.Length > 2)
                {
                    if (psaTemp[2].Trim() != string.Empty)
                        this._gbWorkQueue.ClassDesc.Value = psaTemp[1].Trim() + " - " + psaTemp[2].Trim();
                }
            }
            return;
        }

        private void SetGetRimButtonState()
        {
            PString parentFormName = new PString("ParentFormName");

            if (_parentForm != null)
            {
                parentFormName.Value = this._parentForm.Name;

            }
            else
            {
                parentFormName.Value = "dfwOnSales";
            }

            if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "Teller")
            {
                this.pbGetRIM.Visible = this._gbWorkQueue.SetGetRimButtonState(this.IsNew, "Referral", this._parentForm.Name);
                this.pbGetRIM.Enabled = this.pbGetRIM.Visible;
            }
            else if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
            {
                if (_parentForm != null)
                {
                    this.pbGetRIM.Enabled = this._gbWorkQueue.SetGetRimButtonState(this.IsNew, "Referral", this._parentForm.Name);
                    this.pbGetRIM.Visible = true;
                }
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
                ResizeEmbeddedWinCntrls();  // #34896
            }
        }

        public void EnableTab(PTabPage page, bool enable)
        {
            page.IsDisabled = !enable;
        }

        /* Begin #140811 */
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

        private static void EnableControls(Control.ControlCollection ctls, bool enable)
        {
            foreach (Control ctl in ctls)
            {
                ctl.Visible = enable;
                EnableControls(ctl.Controls, enable);
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

        #region Overrides
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
                /*Begin 174961*/

                // This will add all the busobject Gb_Work_Queue_Note to the busobj collection.

                for (int rCount = 0; rCount < grdSelProdSer.Rows.Count; rCount++)
                {

                    int nAddedQuantity = Convert.ToInt32(grdSelProdSer.Rows[rCount].Cells["colQuantity"].Value.ToString());
                    int nAddedQuantityHidden = 0;
                    try
                    {
                        // Storing the hidden quantity already saved.
                        nAddedQuantityHidden = Convert.ToInt32(grdSelProdSer.Rows[rCount].Cells["colQuantityHidden"].Value.ToString());
                    }
                    catch
                    {
                        nAddedQuantityHidden = 0;
                    }
                    // Find the number of newly added product from the list of available to selected.
                    int nNetQuanitytoAdd = nAddedQuantity - nAddedQuantityHidden;

                    if (nAddedQuantity != nAddedQuantityHidden)
                    {
                        // Saves only the newly added quantiy.
                        for (int nInsertionCount = 1; nInsertionCount <= nNetQuanitytoAdd; nInsertionCount++)
                        {
                            SelectedProductData(rCount, busObjectsToProcess);
                        }
                    }
                }

                /*End 174961*/
            }
            return;
        }
        /*Begin 174961*/
        /// <summary>
        /// Adding the rows in the Selected grid to the business object collection to process.
        /// </summary>
        /// <param name="rCount"></param>
        /// <param name="busObjectsToProcess"></param>
        private void SelectedProductData(int rCount, System.Collections.Generic.List<BusObjectBase> busObjectsToProcess)
        {
            PInt nServiceId = new PInt("ServiceId"); //#37896 - For Get Service Id
            GbWorkQueueNote _grdRowObj = new GbWorkQueueNote();
            _grdRowObj.ActionType = XmActionType.New;
            _grdRowObj.WorkId.Value = _gbWorkQueue.WorkId.Value;
            _grdRowObj.ResponseTypeId = (int)ResponseTypeIdList.ProductGroupDetail;

            string sProdType = DetermineProductType(grdSelProdSer.Rows[rCount].Cells["colType"].Value.ToString(), true);
            _grdRowObj.Type.Value = "Task";//WI #36167.
            _grdRowObj.TaskType.Value = sProdType; //WI #36167.

            _grdRowObj.NoteTitle.Value = grdSelProdSer.Rows[rCount].Cells["colSelDescription"].Value.ToString();
            if (Convert.ToString(grdSelProdSer.Rows[rCount].Cells["colProductGroupId"].Value) != string.Empty)
            {
                _grdRowObj.ProductGroupId.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colProductGroupId"].Value.ToString());
            }
            if (grdSelProdSer.Rows[rCount].Cells["colServiceId"].Value != null)
            {
                _grdRowObj.ServiceId.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colServiceId"].Value);
                nServiceId.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colServiceId"].Value);//#37896 - For Get Service Id
            }
            _grdRowObj.ApplType.Value = Convert.ToString(grdSelProdSer.Rows[rCount].Cells["colApplType"].Value);
            _grdRowObj.AcctType.Value = Convert.ToString(grdSelProdSer.Rows[rCount].Cells["colAcctType"].Value);

            /*Begin 36470*/
            if (cmbType.Value.ToString().Trim() == "External" || cmbType.Value.ToString().Trim() == "ATM Card")
            {
                //_grdRowObj.ApplType.Value = cmbType.Value.ToString().ToUpper(); // 37177
                string[] sAcctType = _grdRowObj.NoteTitle.Value.ToString().Split('-');
                _grdRowObj.AcctType.Value = sAcctType[0].ToString();
                /*CR 37141*/
                if (cmbType.Value.ToString().Trim() == "ATM Card")
                {
                    // Application type is hard coded here as per BA requirement.
                    _grdRowObj.ApplType.Value = "ATM";
                }
            }
            /*End 36470*/
            if (grdSelProdSer.Rows[rCount].Cells["colClassCode"].Value != null)
            {
                _grdRowObj.ClassCode.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colClassCode"].Value);
            }

            _grdRowObj.Status.Value = "Pending";
            _grdRowObj.EmailOwner.Value = "Y";
            _grdRowObj.StatusSort.Value = 10;
            //Begin #37896 - For new Rows it always send value to null so it will get error always so modified createDt as GbWorkQueue window.
            //_grdRowObj.CreateDt.Value = _gbWorkQueue.CreateDt.Value;
            _grdRowObj.CreateDt.Value = new DateTime(DateTime.Now.Year,
                                                                        DateTime.Now.Month,
                                                                        DateTime.Now.Day,
                                                                        DateTime.Now.Hour,
                                                                        DateTime.Now.Minute,
                                                                        DateTime.Now.Second);
            _grdRowObj.DueDt.Value = _gbWorkQueue.DueDt.Value;
            /*Begin #37896*/
            if (nServiceId.Value == -14)//#37896 - Check if it is -14 to insert Rm_3rd_type_id and Rm_3rd_Ins_Id
            {
                _grdRowObj.Rim3rdInsId.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colRmThirdIns"].Value);
                _grdRowObj.Rim3rdTypeId.Value = Convert.ToInt16(grdSelProdSer.Rows[rCount].Cells["colRmThrdType"].Value);
            }
            /*End #37896*/
            busObjectsToProcess.Add(_grdRowObj);

        }
        /*End 174961*/
        public override bool PerformCheck(CheckType checkType, bool showMessage)
        {
            if (checkType == CheckType.EditTest && bDirtyUserDefVal)
            {
                bDirtyUserDefVal = false;
                return true;
            }
            //WI #39394. Added CheckType.EditTest. So that below condition should not execute on NullCheck.
            else if (checkType == CheckType.EditTest && CheckGridDirty()) // Added extra if as part of 174961.
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

        /*Begin 174961*/
        /// <summary>
        /// Population of available grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdAvailProd_BeforePopulate(object sender, GridPopulateArgs e)
        {
            _gbWorkQueue.ResponseTypeId = (int)ResponseTypeIdList.ProductGroupDetail;
            grdAvailProd.ListViewObject = _gbWorkQueue;

        }
        /*Begin 37589*/
        void cmbQueue_PhoenixUIEnterEvent(object sender, PEventArgs e)
        {
            _gbWorkQueue.ResponseTypeId = (int)ResponseTypeIdList.ReferralEdit;//WI37589
        }
        /*End 37589*/
        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {

            grdAvailProd.Rows.Clear();
            grdAvailProd.PopulateTable();
        }

        private void cmbApplType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdAvailProd.Rows.Clear();
            grdAvailProd.PopulateTable();
        }
        /// <summary>
        /// Moves rows from available to selected grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbAdd_Click(object sender, EventArgs e)
        {
            if (grdAvailProd.SelectedItems.Count == 1)
            {
                BuildSelectedProduct();
                EnableDisableControls();
            }
            else
            {
                /*15106 - Please select a product or service to add to the selected Product(s) and Services grid..*/
                PMessageBox.Show(this, 15106, MessageType.Error, MessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// Removes grid from the selected product and services grid. This button will be enabled only for the newly added rows, rows once
        /// saved cannot be removed by this button click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbRemoveProd_Click(object sender, EventArgs e)
        {
            if (grdSelProdSer.SelectedItems.Count == 1)
            {
                RemoveSelectedProdcut();
                EnableDisableControls();
            }
            else
            {
                /*15107 - Please select a product or service to remove.*/
                PMessageBox.Show(this, 15107, MessageType.Error, MessageBoxButtons.OK);
            }
        }

        private void RemoveSelectedProdcut()
        {
            if (CheckExisingRows(colType.UnFormattedValue.ToString(), colSelDescription.UnFormattedValue.ToString()))
            {
                int nQuantitySel = Convert.ToInt32(colQuantity.UnFormattedValue);

                if (nQuantitySel == 1)
                {
                    grdSelProdSer.RemoveRow(grdSelProdSer.CurrentRow.Index);
                }
                else
                {
                    grdSelProdSer.SetCellValueUnFormatted(grdSelProdSer.CurrentRow.Index, 2, --nQuantitySel);
                }
            }

        }
        private void BuildSelectedProduct()
        {
            int nNewRow = 0;
            string sSelTaskType = string.Empty;
            string sAcctType = string.Empty;
            /* Begin #41178 */
            string sApplType = string.Empty;
            /* End #41178 */
            bool isProduct = false;
            bool isService = false;
            bool isProductGroup = false;

            if (cmbType.Value != null)
            {
                if (cmbType.Value.ToString() == "Product Group")
                {
                    sSelTaskType = "G";
                }
                else if (cmbType.Value.ToString() == "Service")
                {
                    sSelTaskType = "S";
                }
                else
                {
                    sSelTaskType = "P";
                }

                sSelTaskType = DetermineProductType(sSelTaskType, false);

                if (CheckExisingRows(sSelTaskType, colDescription.UnFormattedValue.ToString()))
                {
                    for (int i = 0; i < grdSelProdSer.Rows.Count; i++)
                    {
                        if (Convert.ToString(grdSelProdSer.Rows[i].Cells["colType"].Value).Trim() == sSelTaskType && Convert.ToString(grdSelProdSer.Rows[i].Cells["colSelDescription"].Value).Trim() == Convert.ToString(colDescription.UnFormattedValue))
                        {
                            int nQuantitySel = Convert.ToInt32(grdSelProdSer.Rows[i].Cells["colQuantity"].Value);
                            grdSelProdSer.SetCellValueUnFormatted(i, 2, ++nQuantitySel);
                            grdSelProdSer.SelectRow(i);
                            cbSelectedProd.Checked = true;
                            break;
                        }
                    }
                }
                else
                {
                    /* Begin #41178 */
                    sApplType = Convert.ToString(cmbApplType.Value);
                    /* End #41178 */
                    if (Convert.ToString(cmbType.Value) == "Deposit" || Convert.ToString(cmbType.Value) == "Loan" || Convert.ToString(cmbType.Value) == "Box")
                    {
                        sAcctType = Convert.ToString(colDescription.UnFormattedValue).Substring(0, colDescription.UnFormattedValue.ToString().IndexOf('-'));
                        isProduct = true;
                    }
                    else if (Convert.ToString(cmbType.Value) == "Service")
                    {
                        isService = true;
                    }
                    /* Begin #41178 - For ATM and External grid column values were not setting properly*/
                    else if (cmbType.Value.ToString().Trim() == "External" || cmbType.Value.ToString().Trim() == "ATM Card")
                    {
                        sAcctType = Convert.ToString(colDescription.UnFormattedValue).Substring(0, colDescription.UnFormattedValue.ToString().IndexOf('-'));
                        if (cmbType.Value.ToString().Trim() == "ATM Card")
                        {
                            // Application type is hard coded here as per BA requirement.
                            sApplType = "ATM";
                        }
                    }
                    /* End #41178 */
                    else
                    {
                        isProductGroup = true;
                    }
                    nNewRow = grdSelProdSer.AddNewRow();
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 0, "Y");
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 1, sSelTaskType);
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 2, 1);
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 3, colDescription.UnFormattedValue.ToString());
                    if (isProductGroup)
                    {
                        grdSelProdSer.SetCellValueUnFormatted(nNewRow, 5, Convert.ToString(colGroupId.UnFormattedValue)); // This will store product groupId in case of product group.
                    }
                    if (isService)
                    {
                        grdSelProdSer.SetCellValueUnFormatted(nNewRow, 6, Convert.ToString(colGroupId.UnFormattedValue)); // This will store service id in case of service.
                        /*Begin #37896*/
                        grdSelProdSer.SetCellValueUnFormatted(nNewRow, 10, Convert.ToString(colRm3rdType.UnFormattedValue));
                        grdSelProdSer.SetCellValueUnFormatted(nNewRow, 11, Convert.ToString(colRm3rdIns.UnFormattedValue));
                        /*End   #37896*/
                    }
                    /* #41178 - Used common variable in below code to set ApplType */
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 7, sApplType);
                    grdSelProdSer.SetCellValueUnFormatted(nNewRow, 8, sAcctType);
                    if (isProduct)
                    {
                        grdSelProdSer.SetCellValueUnFormatted(nNewRow, 9, Convert.ToString(colGroupId.UnFormattedValue));//This will store the class code.
                    }

                    //grdSelProdSer.SetCellValueUnFormatted(newRow, 10, sSelTaskType);
                    grdSelProdSer.SelectRow(nNewRow);
                }

                grdSelProdSer.Refresh();
            }
        }
        private bool CheckExisingRows(string type, string description)
        {
            for (int i = 0; i < grdSelProdSer.Rows.Count; i++)
            {
                if (Convert.ToString(grdSelProdSer.Rows[i].Cells["colType"].Value).Trim() == type.Trim() && Convert.ToString(grdSelProdSer.Rows[i].Cells["colSelDescription"].Value).Trim() == description.Trim())
                {
                    return true;
                }
            }

            return false;
        }
        private bool CheckGridDirty()
        {
            for (int grdCount = 0; grdCount < grdSelProdSer.Rows.Count; grdCount++)
            {
                if (Convert.ToString(grdSelProdSer.Rows[grdCount].Cells["colQuantity"].Value).Trim() != Convert.ToString(grdSelProdSer.Rows[grdCount].Cells["colQuantityHidden"].Value).Trim())
                {
                    return true;

                }
            }
            return false;
        }
        private string DetermineProductType(string type, bool isCode)
        {
            if (!isCode)
            {
                if (type.Trim() == "G")
                {
                    type = "Group";
                }
                else if (type.Trim() == "S")
                {
                    type = "Service";
                }
                else
                {
                    type = "Product";
                }
            }
            else
            {
                if (type.Trim() == "Group")
                {
                    type = "G";
                }
                else if (type.Trim() == "Service")
                {
                    type = "S";
                }
                else
                {
                    type = "P";
                }
            }
            return type;
        }
        private void grdSelProdSer_BeforePopulate(object sender, GridPopulateArgs e)
        {
            _gbWorkQueue.ResponseTypeId = (int)ResponseTypeIdList.ProductGroupDetSelected;
            grdSelProdSer.ListViewObject = _gbWorkQueue;
        }

        private void grdSelProdSer_SelectedIndexChanged(object source, GridClickEventArgs e)
        {
            EnableDisableControls();
        }

        public void EnableDisableControls()
        {
            if (colQuantityHidden.UnFormattedValue != null)
            {
                if (Convert.ToInt32(colQuantity.UnFormattedValue) == Convert.ToInt32(colQuantityHidden.UnFormattedValue))
                {
                    pbRemoveProd.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
                    pbRemoveProd.Enabled = false; // Added this code since the SetObjectStatus is not working.
                    cbSelectedProd.Checked = false;
                }
                else
                {
                    pbRemoveProd.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                    pbRemoveProd.Enabled = true; // Added this code since the SetObjectStatus is not working.
                }
            }
            else
            {
                pbRemoveProd.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);
                pbRemoveProd.Enabled = true; // Added this code since the SetObjectStatus is not working.
            }
        }

        private void grdSelProdSer_FetchRowDone(object sender, GridRowArgs e)
        {
            // With reference to a CR comment it was asked whether this logic can be done in proc,
            // but the problem here is it is displaying full text in the grid like 'Group' in the table window
            // and just 'G' in table.
            if (colType != null)
            {
                if (Convert.ToString(colType.UnFormattedValue).Trim() == "G")
                {
                    colType.UnFormattedValue = "Group";
                }
                else if (Convert.ToString(colType.UnFormattedValue).Trim() == "S")
                {
                    colType.UnFormattedValue = "Service";
                }
                else if (Convert.ToString(colType.UnFormattedValue).Trim() == "P")
                {
                    colType.UnFormattedValue = "Product";
                }
            }
        }



        /*End 174961*/

    }
}
