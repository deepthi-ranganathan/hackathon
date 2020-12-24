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
// File Name: dlgAdHocReceipt.cs
// NameSpace: Phoenix.Client.Print
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//09/14/2006    	1       mselvaga	#67882 - Created.
//12/15/2006		2		mselvaga	#70293 - Added short cut for print form action button.
//04/07/2008        3       mselvaga    #75744 - QA Release 2008 TCD - Make Required Receipt and TCD Mach ID/TCD Dr # field contents bolder and a bigger font on the Miscellaneous Teller Receipt form.
//11/20/2008        4       mselvaga    #76057 - Added chk printing fix.
//02/11/2009        5       LSimpson    #76409 - Added number of copies.
//04/24/2009        6       mramalin    WI-3475 - Terminal Services Printing Enhancement
//02/25/2010        7       LSimpson    #79574 - UI Label change (ml and object).
// 20May2010        8       dfutcher    #8986 exception when copies blank
//06/24/2010        9       rpoddar 	#79510, #09368 - Item endoresment change
//12/22/2010        10      LSimpson    #80615 - MT Printing.
//02/14/2011        11      LSimpson    #12215 - Revert changes related to passing true form id to dlgAdHocReceipt.  This will uphold changes made for sqr printing.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Xml;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.Shared.Xfs;
using Phoenix.Shared.Constants;

namespace Phoenix.Client.TlPrinting
{
	/// <summary>
	/// Summary description for dlgAdHocReceipt.
	/// </summary>
	public class dlgAdHocReceipt : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private Phoenix.Windows.Forms.PGroupBoxStandard gbInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblRequiredreceipt;
		private Phoenix.Windows.Forms.PdfStandard dfRequiredReceipt;
		private Phoenix.Windows.Forms.PLabelStandard lblDeviceTCDDrawer;
		private Phoenix.Windows.Forms.PdfStandard dfTCDDrawer;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbAvailableFormTypes;
		private Phoenix.Windows.Forms.PGrid gridFormTypes;
		private Phoenix.Windows.Forms.PGridColumn colFormId;
		private Phoenix.Windows.Forms.PGridColumn colFormDescription;
		private Phoenix.Windows.Forms.PAction pbPrintForm;
		private Phoenix.Windows.Forms.PLabelStandard lblInformaiton;
		private Phoenix.Windows.Forms.PAction pbCancel;

		#region Initialize
		private PString requiredReceipt = new PString();
		private PString checkItemInfo = new PString();
		private TellerVars _tellerVars = TellerVars.Instance;
		private AdTlForm _busObjAdTlForm = new AdTlForm();
		private XmlNode _adTlFormNode = TellerVars.Instance.AdTlFormNode as XmlNode;
		private PSmallInt _reprintFormId;
        private PDecimal _noCopies; //#76409
        private PString _printerService; //#76409
		private int _callerScreenId = 0;
        private PdfStandard dfNumberOfCopies;   //#76409
        private PLabelStandard lblNumberOfCopies;
        private PComboBoxStandard cmbWosaService;
        private PLabelStandard lblLogicalService;
        private PGridColumn colServiceType;
        private PGridColumn colLogicalService;   //#76409
        private string tcdMachineInfo = "";
		#endregion

		public dlgAdHocReceipt()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private string _wosaServices;

        public string PTRWosaService
        {
            get { return _wosaServices; }
            set { _wosaServices = value; }
        }
	

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.cmbWosaService = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblLogicalService = new Phoenix.Windows.Forms.PLabelStandard();
            this.lblNumberOfCopies = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfNumberOfCopies = new Phoenix.Windows.Forms.PdfStandard();
            this.lblInformaiton = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfTCDDrawer = new Phoenix.Windows.Forms.PdfStandard();
            this.lblDeviceTCDDrawer = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfRequiredReceipt = new Phoenix.Windows.Forms.PdfStandard();
            this.lblRequiredreceipt = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbAvailableFormTypes = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridFormTypes = new Phoenix.Windows.Forms.PGrid();
            this.colFormId = new Phoenix.Windows.Forms.PGridColumn();
            this.colFormDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colServiceType = new Phoenix.Windows.Forms.PGridColumn();
            this.colLogicalService = new Phoenix.Windows.Forms.PGridColumn();
            this.pbPrintForm = new Phoenix.Windows.Forms.PAction();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.gbInformation.SuspendLayout();
            this.gbAvailableFormTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbPrintForm,
            this.pbCancel});
            // 
            // gbInformation
            // 
            this.gbInformation.Controls.Add(this.cmbWosaService);
            this.gbInformation.Controls.Add(this.lblLogicalService);
            this.gbInformation.Controls.Add(this.lblNumberOfCopies);
            this.gbInformation.Controls.Add(this.dfNumberOfCopies);
            this.gbInformation.Controls.Add(this.lblInformaiton);
            this.gbInformation.Controls.Add(this.dfTCDDrawer);
            this.gbInformation.Controls.Add(this.lblDeviceTCDDrawer);
            this.gbInformation.Controls.Add(this.dfRequiredReceipt);
            this.gbInformation.Controls.Add(this.lblRequiredreceipt);
            this.gbInformation.Location = new System.Drawing.Point(4, 0);
            this.gbInformation.Name = "gbInformation";
            this.gbInformation.PhoenixUIControl.ObjectId = 1;
            this.gbInformation.Size = new System.Drawing.Size(380, 164);
            this.gbInformation.TabIndex = 1;
            this.gbInformation.TabStop = false;
            this.gbInformation.Text = "Information";
            // 
            // cmbWosaService
            // 
            this.cmbWosaService.Location = new System.Drawing.Point(144, 136);
            this.cmbWosaService.Name = "cmbWosaService";
            this.cmbWosaService.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
            this.cmbWosaService.Size = new System.Drawing.Size(228, 21);
            this.cmbWosaService.TabIndex = 8;
            this.cmbWosaService.Value = null;
            // 
            // lblLogicalService
            // 
            this.lblLogicalService.AutoEllipsis = true;
            this.lblLogicalService.Location = new System.Drawing.Point(4, 136);
            this.lblLogicalService.Name = "lblLogicalService";
            this.lblLogicalService.Size = new System.Drawing.Size(132, 20);
            this.lblLogicalService.TabIndex = 9;
            this.lblLogicalService.Text = "Logical Service:";
            // 
            // lblNumberOfCopies
            // 
            this.lblNumberOfCopies.AutoEllipsis = true;
            this.lblNumberOfCopies.Location = new System.Drawing.Point(4, 112);
            this.lblNumberOfCopies.Name = "lblNumberOfCopies";
            this.lblNumberOfCopies.Size = new System.Drawing.Size(100, 20);
            this.lblNumberOfCopies.TabIndex = 7;
            this.lblNumberOfCopies.Text = "Number of Copies:";
            // 
            // dfNumberOfCopies
            // 
            this.dfNumberOfCopies.Location = new System.Drawing.Point(144, 112);
            this.dfNumberOfCopies.Name = "dfNumberOfCopies";
            this.dfNumberOfCopies.PhoenixUIControl.ObjectId = 13;
            this.dfNumberOfCopies.Size = new System.Drawing.Size(100, 20);
            this.dfNumberOfCopies.TabIndex = 6;
            this.dfNumberOfCopies.Leave += new System.EventHandler(this.dfNumberOfCopies_Leave);
            // 
            // lblInformaiton
            // 
            this.lblInformaiton.AutoEllipsis = true;
            this.lblInformaiton.Location = new System.Drawing.Point(4, 16);
            this.lblInformaiton.Name = "lblInformaiton";
            this.lblInformaiton.PhoenixUIControl.ObjectId = 2;
            this.lblInformaiton.Size = new System.Drawing.Size(368, 40);
            this.lblInformaiton.TabIndex = 5;
            this.lblInformaiton.Text = "The activity you are performing requires a form to be printed.  Please select a f" +
                "orm type from the list below that most accurately matches the requested receipt:" +
                "";
            this.lblInformaiton.WordWrap = true;
            // 
            // dfTCDDrawer
            // 
            this.dfTCDDrawer.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTCDDrawer.Location = new System.Drawing.Point(144, 88);
            this.dfTCDDrawer.Name = "dfTCDDrawer";
            this.dfTCDDrawer.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfTCDDrawer.PhoenixUIControl.ObjectId = 12;
            this.dfTCDDrawer.Size = new System.Drawing.Size(228, 20);
            this.dfTCDDrawer.TabIndex = 0;
            // 
            // lblDeviceTCDDrawer
            // 
            this.lblDeviceTCDDrawer.AutoEllipsis = true;
            this.lblDeviceTCDDrawer.Location = new System.Drawing.Point(4, 88);
            this.lblDeviceTCDDrawer.Name = "lblDeviceTCDDrawer";
            this.lblDeviceTCDDrawer.PhoenixUIControl.ObjectId = 12;
            this.lblDeviceTCDDrawer.Size = new System.Drawing.Size(136, 20);
            this.lblDeviceTCDDrawer.TabIndex = 1;
            this.lblDeviceTCDDrawer.Text = "TCD/TCR Drawer #:";
            // 
            // dfRequiredReceipt
            // 
            this.dfRequiredReceipt.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfRequiredReceipt.Location = new System.Drawing.Point(144, 64);
            this.dfRequiredReceipt.Name = "dfRequiredReceipt";
            this.dfRequiredReceipt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
            this.dfRequiredReceipt.PhoenixUIControl.ObjectId = 3;
            this.dfRequiredReceipt.Size = new System.Drawing.Size(228, 20);
            this.dfRequiredReceipt.TabIndex = 2;
            // 
            // lblRequiredreceipt
            // 
            this.lblRequiredreceipt.AutoEllipsis = true;
            this.lblRequiredreceipt.Location = new System.Drawing.Point(4, 64);
            this.lblRequiredreceipt.Name = "lblRequiredreceipt";
            this.lblRequiredreceipt.PhoenixUIControl.ObjectId = 3;
            this.lblRequiredreceipt.Size = new System.Drawing.Size(120, 20);
            this.lblRequiredreceipt.TabIndex = 3;
            this.lblRequiredreceipt.Text = "Required receipt:";
            // 
            // gbAvailableFormTypes
            // 
            this.gbAvailableFormTypes.Controls.Add(this.gridFormTypes);
            this.gbAvailableFormTypes.Location = new System.Drawing.Point(4, 164);
            this.gbAvailableFormTypes.Name = "gbAvailableFormTypes";
            this.gbAvailableFormTypes.PhoenixUIControl.ObjectId = 4;
            this.gbAvailableFormTypes.Size = new System.Drawing.Size(380, 172);
            this.gbAvailableFormTypes.TabIndex = 0;
            this.gbAvailableFormTypes.TabStop = false;
            this.gbAvailableFormTypes.Text = "Available Form Types";
            // 
            // gridFormTypes
            // 
            this.gridFormTypes.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colFormId,
            this.colFormDescription,
            this.colServiceType,
            this.colLogicalService});
            this.gridFormTypes.Location = new System.Drawing.Point(6, 16);
            this.gridFormTypes.Name = "gridFormTypes";
            this.gridFormTypes.Size = new System.Drawing.Size(368, 152);
            this.gridFormTypes.TabIndex = 0;
            this.gridFormTypes.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridFormTypes_SelectedIndexChanged);
            this.gridFormTypes.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridFormTypes_BeforePopulate);
            // 
            // colFormId
            // 
            this.colFormId.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
            this.colFormId.PhoenixUIControl.ObjectId = 6;
            this.colFormId.PhoenixUIControl.XmlTag = "FormId";
            this.colFormId.Title = "Form Id";
            this.colFormId.Visible = false;
            this.colFormId.Width = 0;
            // 
            // colFormDescription
            // 
            this.colFormDescription.PhoenixUIControl.ObjectId = 7;
            this.colFormDescription.PhoenixUIControl.XmlTag = "Description";
            this.colFormDescription.Title = "Form Type";
            this.colFormDescription.Width = 345;
            // 
            // colServiceType
            // 
            this.colServiceType.PhoenixUIControl.XmlTag = "ServiceType";
            this.colServiceType.Title = "Column";
            this.colServiceType.Visible = false;
            // 
            // colLogicalService
            // 
            this.colLogicalService.PhoenixUIControl.XmlTag = "LogicalService";
            this.colLogicalService.Title = "Column";
            this.colLogicalService.Visible = false;
            // 
            // pbPrintForm
            // 
            this.pbPrintForm.LongText = "pbPrintForm";
            this.pbPrintForm.ObjectId = 9;
            this.pbPrintForm.Shortcut = System.Windows.Forms.Keys.F2;
            this.pbPrintForm.ShortText = "pbPrintForm";
            this.pbPrintForm.Tag = null;
            this.pbPrintForm.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPrintForm_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.LongText = "pbCancel";
            this.pbCancel.ObjectId = 10;
            this.pbCancel.ShortText = "pbCancel";
            this.pbCancel.Tag = null;
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // dlgAdHocReceipt
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(386, 344);
            this.Controls.Add(this.gbAvailableFormTypes);
            this.Controls.Add(this.gbInformation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "dlgAdHocReceipt";
            this.ScreenId = 10884;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.Text = "Miscellaneous Teller Receipt";
            this.Load += new System.EventHandler(this.dlgAdHocReceipt_Load);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgAdHocReceipt_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgAdHocReceipt_PInitBeginEvent);
            this.gbInformation.ResumeLayout(false);
            this.gbInformation.PerformLayout();
            this.gbAvailableFormTypes.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
            _noCopies = new PDecimal("_noCopies");
			if (paramList.Length >= 2)
			{
				if (paramList[0] != null)
					requiredReceipt.Value = Convert.ToString(paramList[0]);
				_reprintFormId = paramList[1] as PSmallInt;
				//
				_busObjAdTlForm.OutputTypeId.Value = 1;
                //Begin #76057 Selva Chk Reversal Fix
                if (!_reprintFormId.IsNull)
                {
                    if (_reprintFormId.Value == Convert.ToInt16(-1))
                        _busObjAdTlForm.ResponseTypeId = 10; //exclude TEXT_QRP = S
                }
                //End #76057
                if (paramList.Length > 2)
				{
					if (paramList[2] != null)
					{
						_callerScreenId = Convert.ToInt32(paramList[2]);
						if (_callerScreenId == Phoenix.Shared.Constants.ScreenId.CapturedItems)
							_busObjAdTlForm.OutputTypeId.Value = 2;
						else if (_callerScreenId == Phoenix.Shared.Constants.ScreenId.PODTotals)
							_busObjAdTlForm.OutputTypeId.Value = 3;
                        #region #76409
                        if (_callerScreenId == Phoenix.Shared.Constants.ScreenId.TlProofTotals)
                            _busObjAdTlForm.ResponseTypeId = 11;
                        #endregion
                        //Begin #79510, #09368
                        if (_callerScreenId == Phoenix.Shared.Constants.ScreenId.TlEndorseItems)
                            _busObjAdTlForm.ResponseTypeId = 12;
                        //End #79510, #09368
                        #region #80615
                        //Exclude TEXT_QRP = F, H, M (Footer, Header, Mailer)
                        if (_reprintFormId.Value >= 0) // #12215
                        {
                            if (_callerScreenId == Phoenix.Shared.Constants.ScreenId.Journal && _reprintFormId.Value != Convert.ToInt16(-1))
                                _busObjAdTlForm.ResponseTypeId = 13;
                        }
                        #endregion
					}
				}
				if (paramList.Length > 3)
				{
					if (paramList[3] != null)
						checkItemInfo.Value = Convert.ToString(paramList[3]);
				}

                /*Begin #72916 - Phase 3 */
                if (paramList.Length > 4)
                {
                    if (paramList[4] != null)
                        tcdMachineInfo = Convert.ToString(paramList[4]);
                }
                /*End #72916 - Phase 3 */

                #region #76409
                if (paramList.Length > 5)
                {
                    if (paramList[5] != null)
                    {
                        _noCopies = paramList[5] as PDecimal;
                    }
                }
                #endregion

                if (paramList.Length > 6)
                {
                    if (paramList[6] != null)
                    {
                        _printerService = paramList[6] as PString;
                    }
                }
            }

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.AdHocReceipt;
			base.InitParameters (paramList);
		}
		#endregion

		#region AdHoc Receipt Events
		private ReturnType dlgAdHocReceipt_PInitBeginEvent()
		{
			#region handle security
			this.pbCancel.NextScreenId = 0;
			this.pbPrintForm.NextScreenId = 0;
			#endregion

			if (!checkItemInfo.IsNull)
			{
				this.dfTCDDrawer.Text = checkItemInfo.Value;
			}			
			if (!requiredReceipt.IsNull)
			{
				int pos = requiredReceipt.Value.IndexOf('-');
				#region TCD Drawer
                //#72916 - Selva -commented out
                //if (_tellerVars.AdTlControl.TcdDevices.Value == GlobalVars.Instance.ML.Y && checkItemInfo.IsNull)
                //{
                //    if (pos != -1)
                //        this.dfTCDDrawer.Text = requiredReceipt.Value.Substring(pos + 1, requiredReceipt.Value.Length - pos);
                //    else
                //        this.dfTCDDrawer.Text = requiredReceipt.Value;
                //}
				#endregion
				//
				#region Required Receipt
				if (pos != -1)
					this.dfRequiredReceipt.Text = requiredReceipt.Value.Substring(0, pos - 1);
				else
					this.dfRequiredReceipt.Text = requiredReceipt.Value;
				#endregion
			}
			this.DefaultAction = pbPrintForm;
			//
			gridFormTypes.ListViewObject = _busObjAdTlForm;
            #region #76409
            if (this.dfNumberOfCopies.UnFormattedValue == null && _noCopies.StringValue != "-1" && _noCopies.IntValue <= 1)
            {
                this.dfNumberOfCopies.UnFormattedValue = 1;
                this.dfNumberOfCopies.Text = "1";
            }
            if (_noCopies.IntValue >= 1)
            {
                this.dfNumberOfCopies.UnFormattedValue = _noCopies.Value;
                this.dfNumberOfCopies.Text = _noCopies.StringValue;
                //DataColumn mycol = new DataColumn();
                //int cnt = gridFormTypes.Count;
                //string [] myarray = new string [cnt];
                //for (int a = 0; a < cnt; a++)
                //{
                //    myarray[a] = gridFormTypes.Items[a];
                //}
                //int idx;
                //gridFormTypes.SelectRow();
            }
            #endregion
            WosaServicesHelper.PopulateCombo(cmbWosaService);
            return ReturnType.Success;
		}

		private void dlgAdHocReceipt_PInitCompleteEvent()
		{
//			this.gridFormTypes.PopulateTable(_adTlFormNode, true);
			this.gridFormTypes.DoubleClickAction = pbPrintForm;
			EnableDisableVisibleLogic("FormComplete");
			//
			this.pbPrintForm.Image = Images.Print;
			this.pbCancel.Image = Images.Cancel;
            //Begin #72916
            //Begin #75744
            if (_tellerVars.IsTCDConnected && tcdMachineInfo != "" &&
                tcdMachineInfo != string.Empty)
            {
                this.dfTCDDrawer.Text = tcdMachineInfo;
            }
            SetFieldReadOnly(dfRequiredReceipt);
            SetFieldReadOnly(dfTCDDrawer);
            //if (dfRequiredReceipt.Text == string.Empty )
            //    this.dfRequiredReceipt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
            //if (dfTCDDrawer.Text == string.Empty)
            //    this.dfTCDDrawer.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
            //End #75744
            //End #72916
         
            UpdateServiceCombo();
        }

        #region #76409
        void dfNumberOfCopies_Leave(object sender, EventArgs e)
        {
            if (dfNumberOfCopies.Text.Trim() != "")
            {
                if (Convert.ToInt32(dfNumberOfCopies.Text) <= 0)
                    PMessageBox.Show(this, 11110, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
            }
        }
        #endregion

        private void pbPrintForm_Click(object sender, PActionEventArgs e)
		{
			_reprintFormId.Value = Convert.ToInt16(colFormId.UnFormattedValue);
            
            //#76409 begin

            if (dfNumberOfCopies.UnFormattedValue == null //8986
                || string.IsNullOrEmpty(dfNumberOfCopies.UnFormattedValue.ToString())) 
            {
                dfNumberOfCopies.UnFormattedValue = 1;
                dfNumberOfCopies.Text = "1";
            }
            
            _noCopies.Value = Convert.ToInt16(dfNumberOfCopies.UnFormattedValue);

            if( _printerService != null )
                _printerService.Value = cmbWosaService.Text;

            //#76409 end
            
            this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void pbCancel_Click(object sender, PActionEventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion
		
		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
            {
                #region #76409
                if (_noCopies.StringValue == "-1" && _callerScreenId == Phoenix.Shared.Constants.ScreenId.TlProofTotals)
                    this.dfNumberOfCopies.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Show, EnableState.Enable);
                else
                    this.dfNumberOfCopies.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Disable);
                #endregion
                //Begin #75744
                //this.dfRequiredReceipt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //this.dfTCDDrawer.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //if (_tellerVars.IsTCDConnected && tcdMachineInfo != "" &&
                //    tcdMachineInfo != string.Empty)
                //{
                //    //this.dfRequiredReceipt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //    //this.dfTCDDrawer.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //}
                //else
                //{
                //    this.dfRequiredReceipt.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //    this.dfTCDDrawer.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                //}
			}
		}
		#endregion

		private void gridFormTypes_BeforePopulate(object sender, GridPopulateArgs e)
		{
			this.gridFormTypes.ListViewObject = _busObjAdTlForm;
		}

        //#75744
        private void SetFieldReadOnly(PdfStandard dfField)
        {
            dfField.Font = new Font(dfField.Font, FontStyle.Bold);
            dfField.ReadOnly = true;
            dfField.BackColor = Color.White;
            dfField.TabStop = false;
        }

        private void dlgAdHocReceipt_Load(object sender, EventArgs e)
        {

        }

        private void gridFormTypes_SelectedIndexChanged(object source, GridClickEventArgs e)
        {
            if( this.IsFormInitialized )
                UpdateServiceCombo();
        }

        private void UpdateServiceCombo()
        {
            if (gridFormTypes.ContextRow >= 0)
            {
                WosaServicesHelper.SetWosaService(cmbWosaService, colServiceType.Text, colLogicalService.Text);
            }
        }
	}
}
