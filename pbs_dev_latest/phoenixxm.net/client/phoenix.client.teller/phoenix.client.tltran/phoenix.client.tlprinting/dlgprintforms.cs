
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
// File Name: dlgPrintForms.cs
// NameSpace: Phoenix.Client.Print
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//09/14/2006    1       mselvaga	#67882 - Created.
//02/20/2007	2		rpoddar		#71873 - Commented the closing of XfsPrinter.
//02/20/2007	3		rpoddar		#71896 - Added code to set the focus on print button.
//03/16/2007	4		rpoddar		#72063 - Make fields read only
//04/03/2007	5		rpoddar		#72204 - Set focus to print button in case print fails.
//May-24-07		6   	Muthu       #72780 VS2005 Migration
//09/17/2008    7       LSimpson    #76057 - pbPrintForm_Click - Added "S" qrp value.
//03/06/2009    8       mselvaga    #76033 - Added logic for voucher printing.
//04/24/2009    9       mramalin    WI-3475 - Terminal Services Printing Enhancement
//06/24/2010    10      rpoddar     #79510, #09368 - Return dialog result as Ignore when skip is clicked.
//09/18/2012	11		rpoddar		#19415 - Performance Fixes
//8/3/2013		12		apitava		#157637 uses new xfsprinter
//4/8/2014		13		jrhyne		#157637 (2) uses new xfsprinter
//08/21/2019    14      mselvaga    Task#118299 - ECM Voucher printing changes added
//09/26/2019    15      mselvaga    #119734 - Added ECM print and archive changes.
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
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Xfs;
using Phoenix.Windows.Client;
using System.Collections.Generic;

namespace Phoenix.Client.TlPrinting
{
	/// <summary>
	/// Summary description for dlgPrintForms.
	/// </summary>
	public class dlgPrintForms : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblInformation;
		private Phoenix.Windows.Forms.PLabelStandard lblNumberofLastPrintedLine;
		private Phoenix.Windows.Forms.PdfStandard dfPbLastLine;
		private Phoenix.Windows.Forms.PLabelStandard lblForm;
		private Phoenix.Windows.Forms.PdfStandard dfFormDescription;
		private Phoenix.Windows.Forms.PAction pbPrintForm;
		private Phoenix.Windows.Forms.PAction pbReprintLast;
		private Phoenix.Windows.Forms.PAction pbSkip;
		private Phoenix.Windows.Forms.PAction pbCancel;
		private Phoenix.Windows.Forms.PLabelStandard lblNote;
		private Phoenix.Windows.Forms.PLabelStandard lblLogicalService;
		private Phoenix.Windows.Forms.PLabelStandard lblNote2;
		private Phoenix.Windows.Forms.PComboBoxStandard cmbWosaService;

		#region Initialize

		private PSmallInt formId = new PSmallInt();
		private PString formDesc = new PString();
		private PString textQrp = new PString();
		private PString formPrintString = new PString();
		private PSmallInt formPrintNo = new PSmallInt();
		private PString logicalService = new PString();
		private PString formName = new PString();
		private PString formMediaName = new PString();
		private PString wosaServiceName = new PString();
		private PSmallInt lastLinePrinted = new PSmallInt();
		//
		private TellerVars _tellerVars = TellerVars.Instance;
		private PrintInfo _wosaPrintInfo = new PrintInfo();
		private XfsPrinter _xfsPrinter;
		private XmlNode _adTlFormNode = TellerVars.Instance.AdTlFormNode as XmlNode;
		//private List<String> _logicalSvcs = null;
		private EnumValueCollection evCollection = new EnumValueCollection();
		private Phoenix.Windows.Forms.PdfStandard dfCheckItemInfo;
		private Phoenix.Windows.Forms.PLabelStandard lblCheckItemInfo;
		private Phoenix.Windows.Forms.PAction pbSkipChecks;
		private bool _checkItemsOnly = false;
		private bool showCheckItem = false;
		private PString checkItemInfo = new PString();
		private int printFocussedCount = 0;		// #71896
		//private string _imageSvcsList = null;
		private PString printString = new PString(); //#76033
		#endregion

		public dlgPrintForms()
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



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.gbInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.lblCheckItemInfo = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfCheckItemInfo = new Phoenix.Windows.Forms.PdfStandard();
			this.cmbWosaService = new Phoenix.Windows.Forms.PComboBoxStandard();
			this.lblLogicalService = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfFormDescription = new Phoenix.Windows.Forms.PdfStandard();
			this.lblForm = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfPbLastLine = new Phoenix.Windows.Forms.PdfStandard();
			this.lblNumberofLastPrintedLine = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblInformation = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblNote2 = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblNote = new Phoenix.Windows.Forms.PLabelStandard();
			this.pbPrintForm = new Phoenix.Windows.Forms.PAction();
			this.pbReprintLast = new Phoenix.Windows.Forms.PAction();
			this.pbSkip = new Phoenix.Windows.Forms.PAction();
			this.pbCancel = new Phoenix.Windows.Forms.PAction();
			this.pbSkipChecks = new Phoenix.Windows.Forms.PAction();
			this.gbInformation.SuspendLayout();
			this.SuspendLayout();
			//
			// ActionManager
			//
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbPrintForm,
            this.pbReprintLast,
            this.pbSkip,
            this.pbSkipChecks,
            this.pbCancel});
			//
			// gbInformation
			//
			this.gbInformation.Controls.Add(this.lblCheckItemInfo);
			this.gbInformation.Controls.Add(this.dfCheckItemInfo);
			this.gbInformation.Controls.Add(this.cmbWosaService);
			this.gbInformation.Controls.Add(this.lblLogicalService);
			this.gbInformation.Controls.Add(this.dfFormDescription);
			this.gbInformation.Controls.Add(this.lblForm);
			this.gbInformation.Controls.Add(this.dfPbLastLine);
			this.gbInformation.Controls.Add(this.lblNumberofLastPrintedLine);
			this.gbInformation.Controls.Add(this.lblInformation);
			this.gbInformation.Location = new System.Drawing.Point(4, 4);
			this.gbInformation.Name = "gbInformation";
			this.gbInformation.PhoenixUIControl.ObjectId = 1;
			this.gbInformation.Size = new System.Drawing.Size(396, 156);
			this.gbInformation.TabIndex = 0;
			this.gbInformation.TabStop = false;
			this.gbInformation.Text = "Information";
			//
			// lblCheckItemInfo
			//
			this.lblCheckItemInfo.AutoEllipsis = true;
			this.lblCheckItemInfo.Location = new System.Drawing.Point(4, 56);
			this.lblCheckItemInfo.Name = "lblCheckItemInfo";
			this.lblCheckItemInfo.PhoenixUIControl.ObjectId = 13;
			this.lblCheckItemInfo.Size = new System.Drawing.Size(148, 20);
			this.lblCheckItemInfo.TabIndex = 13;
			this.lblCheckItemInfo.Text = "Check Item #:";
			//
			// dfCheckItemInfo
			//
			this.dfCheckItemInfo.Location = new System.Drawing.Point(156, 56);
			this.dfCheckItemInfo.Name = "dfCheckItemInfo";
			this.dfCheckItemInfo.PhoenixUIControl.ObjectId = 13;
			this.dfCheckItemInfo.Size = new System.Drawing.Size(231, 20);
			this.dfCheckItemInfo.TabIndex = 12;
			//
			// cmbWosaService
			//
			this.cmbWosaService.Location = new System.Drawing.Point(156, 104);
			this.cmbWosaService.Name = "cmbWosaService";
			this.cmbWosaService.PhoenixUIControl.IsNullable = Phoenix.Windows.Forms.NullabilityState.NotNull;
			this.cmbWosaService.PhoenixUIControl.ObjectId = 12;
			this.cmbWosaService.Size = new System.Drawing.Size(231, 21);
			this.cmbWosaService.TabIndex = 0;
			//
			// lblLogicalService
			//
			this.lblLogicalService.AutoEllipsis = true;
			this.lblLogicalService.Location = new System.Drawing.Point(4, 104);
			this.lblLogicalService.Name = "lblLogicalService";
			this.lblLogicalService.PhoenixUIControl.ObjectId = 12;
			this.lblLogicalService.Size = new System.Drawing.Size(148, 20);
			this.lblLogicalService.TabIndex = 1;
			this.lblLogicalService.Text = "Logical Service:";
			//
			// dfFormDescription
			//
			this.dfFormDescription.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfFormDescription.Location = new System.Drawing.Point(156, 80);
			this.dfFormDescription.Name = "dfFormDescription";
			this.dfFormDescription.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfFormDescription.PhoenixUIControl.ObjectId = 3;
			this.dfFormDescription.Size = new System.Drawing.Size(231, 20);
			this.dfFormDescription.TabIndex = 6;
			//
			// lblForm
			//
			this.lblForm.AutoEllipsis = true;
			this.lblForm.Location = new System.Drawing.Point(4, 80);
			this.lblForm.Name = "lblForm";
			this.lblForm.PhoenixUIControl.ObjectId = 3;
			this.lblForm.Size = new System.Drawing.Size(148, 20);
			this.lblForm.TabIndex = 7;
			this.lblForm.Text = "&Form:";
			//
			// dfPbLastLine
			//
			this.dfPbLastLine.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfPbLastLine.Location = new System.Drawing.Point(344, 128);
			this.dfPbLastLine.Name = "dfPbLastLine";
			this.dfPbLastLine.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfPbLastLine.PhoenixUIControl.ObjectId = 11;
			this.dfPbLastLine.Size = new System.Drawing.Size(44, 20);
			this.dfPbLastLine.TabIndex = 8;
			//
			// lblNumberofLastPrintedLine
			//
			this.lblNumberofLastPrintedLine.AutoEllipsis = true;
			this.lblNumberofLastPrintedLine.Location = new System.Drawing.Point(4, 128);
			this.lblNumberofLastPrintedLine.Name = "lblNumberofLastPrintedLine";
			this.lblNumberofLastPrintedLine.PhoenixUIControl.ObjectId = 11;
			this.lblNumberofLastPrintedLine.Size = new System.Drawing.Size(148, 20);
			this.lblNumberofLastPrintedLine.TabIndex = 9;
			this.lblNumberofLastPrintedLine.Text = "&Number of Last Printed Line:";
			//
			// lblInformation
			//
			this.lblInformation.AutoEllipsis = true;
			this.lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblInformation.Location = new System.Drawing.Point(4, 15);
			this.lblInformation.Name = "lblInformation";
			this.lblInformation.PhoenixUIControl.ObjectId = 2;
			this.lblInformation.Size = new System.Drawing.Size(384, 33);
			this.lblInformation.TabIndex = 11;
			this.lblInformation.Text = "Your transaction has posted successfully.  Please insert the Form listed in the t" +
				"eller printer and press <ENTER> when ready:";
			this.lblInformation.WordWrap = true;
			//
			// lblNote2
			//
			this.lblNote2.AutoEllipsis = true;
			this.lblNote2.Location = new System.Drawing.Point(8, 184);
			this.lblNote2.Name = "lblNote2";
			this.lblNote2.PhoenixUIControl.ObjectId = 10;
			this.lblNote2.Size = new System.Drawing.Size(384, 28);
			this.lblNote2.TabIndex = 12;
			this.lblNote2.Text = "To bypass printing only for Passbook accounts, press the \'Skip\' button. The trans" +
				"action(s) is still considered booked.";
			this.lblNote2.WordWrap = true;
			//
			// lblNote
			//
			this.lblNote.AutoEllipsis = true;
			this.lblNote.Location = new System.Drawing.Point(8, 168);
			this.lblNote.Name = "lblNote";
			this.lblNote.PhoenixUIControl.ObjectId = 9;
			this.lblNote.Size = new System.Drawing.Size(384, 12);
			this.lblNote.TabIndex = 5;
			this.lblNote.Text = "Note: To reprint the last form, press the Reprint Last button (F10).";
			//
			// pbPrintForm
			//
			this.pbPrintForm.ObjectId = 5;
			this.pbPrintForm.Shortcut = System.Windows.Forms.Keys.F2;
			this.pbPrintForm.ShortText = "Print (F2)";
			this.pbPrintForm.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPrintForm_Click);
			//
			// pbReprintLast
			//
			this.pbReprintLast.ObjectId = 6;
			this.pbReprintLast.Shortcut = System.Windows.Forms.Keys.F10;
			this.pbReprintLast.ShortText = "Reprint Last";
			this.pbReprintLast.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbReprintLast_Click);
			//
			// pbSkip
			//
			this.pbSkip.ObjectId = 7;
			this.pbSkip.Shortcut = System.Windows.Forms.Keys.F8;
			this.pbSkip.ShortText = "Skip (F8)";
			this.pbSkip.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSkip_Click);
			//
			// pbCancel
			//
			this.pbCancel.ObjectId = 8;
			this.pbCancel.ShortText = "Cancel";
			this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
			//
			// pbSkipChecks
			//
			this.pbSkipChecks.ObjectId = 14;
			this.pbSkipChecks.Shortcut = System.Windows.Forms.Keys.F9;
			this.pbSkipChecks.ShortText = "Skip Checks";
			this.pbSkipChecks.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSkipChecks_Click);
			//
			// dlgPrintForms
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(402, 215);
			this.Controls.Add(this.gbInformation);
			this.Controls.Add(this.lblNote);
			this.Controls.Add(this.lblNote2);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "dlgPrintForms";
			this.ScreenId = Phoenix.Shared.Constants.ScreenId.PrintForms; //10879; 
			this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
			this.Text = "Printing Forms";
			this.Load += new System.EventHandler(this.dlgPrintForms_Load);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgPrintForms_PInitCompleteEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgPrintForms_PInitBeginEvent);
			this.gbInformation.ResumeLayout(false);
			this.gbInformation.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		ArrayList _imageData = null;
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length >= 4)
			{
				if (paramList[0] != null)
					formId.Value = Convert.ToInt16(paramList[0]);
				if (paramList[1] != null)
					formPrintNo.Value = Convert.ToInt16(paramList[1]);
				if (paramList[2] != null)
					lastLinePrinted.Value = Convert.ToInt16(paramList[2]);
				if (paramList[3] != null)
					_wosaPrintInfo = (PrintInfo)paramList[3];
				_checkItemsOnly = false;
				//
				if (paramList.Length > 4)
				{
					if (paramList[4] != null)
					{
						checkItemInfo.Value = Convert.ToString(paramList[4]);
						_checkItemsOnly = true;
					}
				}
				if (paramList.Length > 5)
				{
					if (paramList[5] != null || !string.IsNullOrEmpty(_wosaPrintInfo.BondOwnerName))    //#3401
					{
						if (!string.IsNullOrEmpty(_wosaPrintInfo.BondOwnerName))    //#3401
						{
							checkItemInfo.Value = GetBondInfo();
							_checkItemsOnly = true;
						}
						else
						{
							showCheckItem = Convert.ToBoolean(paramList[5]);
							if (showCheckItem)
							{
								if (_wosaPrintInfo.ItemNo > 0)
								{
									checkItemInfo.Value = GetCheckInfo();
									_checkItemsOnly = true;
								}
							}
						}
					}
				}

				if (paramList.Length > 6)
					_imageData = (paramList[6] != null) ? ((ArrayList)paramList[6]) : null;

				//				if ( !_checkItemsOnly && _wosaPrintInfo.ItemNo > 0 )
				//				{
				//					checkItemInfo.Value = GetCheckInfo();
				//					_checkItemsOnly = true;
				//				}

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
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			base.InitParameters(paramList);
		}
		#endregion

		#region Print Form Events


		private ReturnType dlgPrintForms_PInitBeginEvent()
		{
			#region handle security
			this.pbCancel.NextScreenId = 0;
			this.pbPrintForm.NextScreenId = 0;
			this.pbReprintLast.NextScreenId = 0;
			this.pbSkip.NextScreenId = 0;
			this.pbSkipChecks.NextScreenId = 0;
			#endregion
			//
			/* Begin #71896 */
			this.GotFocus += new EventHandler(Control_GotFocus);
			this.cmbWosaService.GotFocus += new EventHandler(Control_GotFocus);
			/* End #71896 */
			return ReturnType.Success;
		}

		private void dlgPrintForms_PInitCompleteEvent()
		{
			//
			#region default action
			this.DefaultAction = pbPrintForm;
			#endregion
			//
			#region initialize
			if (!lastLinePrinted.IsNull && lastLinePrinted.Value >= 0)
				this.dfPbLastLine.UnFormattedValue = lastLinePrinted.Value;
			if (!checkItemInfo.IsNull)
				this.dfCheckItemInfo.Text = checkItemInfo.Value;
			else
				this.dfCheckItemInfo.Text = "N/A";		// #72063
			this.dfFormDescription.Text = formDesc.Value;
			//_logicalSvcs = _xfsPrinter.GetLogicalServices();
			//XfsRegHelper regHelper = new XfsRegHelper()
			//_logicalSvcs = (new XfsRegHelper()).GetLogicalPrintersServices(XfsRegHelper.PrinterFilter.NormalPrinterOnly);//.GetLogicalServices( true );

			#endregion
			//
			#region load service
			//_imageSvcsList = null;
			//Phoenix.Shared.RegistryHelper.GetLogicalSvcInfo("ClassesRoot", null, "device_name", null, ref _imageSvcsList);  //#76033 - get image printer svc
			//WosaServicesHelper.PopulateCombo(cmbWosaService);
			//if (_logicalSvcs.Count > 0)
			if (WosaServicesHelper.PopulateCombo(cmbWosaService) > 0)
			{
				//foreach(string svcs in _logicalSvcs)
				//{
				//    evCollection.Add(new EnumValue(svcs, svcs));
				//    //if (_imageSvcsList != null)
				//    //{
				//    //    if (_imageSvcsList.IndexOf(svcs) == -1)
				//    //        evCollection.Add(new EnumValue(svcs, svcs));
				//    //}
				//    //else
				//    //    evCollection.Add(new EnumValue(svcs, svcs));
				//}

				//
				//this.cmbWosaService.Populate(evCollection.EnumValues);
				if (!wosaServiceName.IsNull && wosaServiceName.Value.Trim() != String.Empty)
				{
					//this.cmbWosaService.DefaultCodeValue = wosaServiceName.Value;
					//this.cmbWosaService.InitialDisplayType = UIComboInitialDisplayType.DisplayDefault;
					WosaServicesHelper.SetWosaService(cmbWosaService, logicalService.StringValue, wosaServiceName.StringValue);
				}
				else
				{
					//this.cmbWosaService.InitialDisplayType = UIComboInitialDisplayType.DisplayFirst;
					this.cmbWosaService.SelectedIndex = 0;

				}
			}
			#endregion
			//
			#region handle enable/disable
			EnableDisableVisibleLogic("FormComplete");
			#endregion
			//
			#region images
			this.pbReprintLast.Image = Images.Print;
			this.pbPrintForm.Image = Images.Print;
			this.pbCancel.Image = Images.Cancel;
			#endregion

			/* Begin #72063 */
			SetFieldReadOnly(dfFormDescription);
			SetFieldReadOnly(dfCheckItemInfo);
			/* End #72063 */
			if (!string.IsNullOrEmpty(_wosaPrintInfo.BondOwnerName)) //#3401
				this.lblCheckItemInfo.Text = CoreService.Translation.GetUserMessageX(11265); //11265 - Bond Owner:

		}

		private void pbPrintForm_Click(object sender, PActionEventArgs e)
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
							this.DialogResult = DialogResult.OK;
							this.Close();
						}
						else
							pbPrintForm.ImageButton.Focus();		// #72204
					}
					finally
					{
						_xfsPrinter.Close();
					}
				}
			}
			else
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void pbReprintLast_Click(object sender, PActionEventArgs e)
		{
			this.DialogResult = DialogResult.Retry;
			this.Close();
		}

		private void pbSkip_Click(object sender, PActionEventArgs e)
		{
			//Begin #79510, #09368
			//this.DialogResult = DialogResult.OK;
			this.DialogResult = DialogResult.Ignore;
			//End #79510, #09368
			this.Close();
		}

		private void pbCancel_Click(object sender, PActionEventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void pbSkipChecks_Click(object sender, PActionEventArgs e)
		{
			this.DialogResult = DialogResult.Abort;
			this.Close();
		}
		private string GetCheckInfo()
		{
			string _tempChkAmount = null;
			_tempChkAmount = CurrencyHelper.GetFormattedValue(1, _wosaPrintInfo.ItemAmount);
			_tempChkAmount = _tempChkAmount.PadLeft(14, '*');
			_tempChkAmount = _tempChkAmount.Replace("*", " ");
			//
			return _wosaPrintInfo.ItemNo.ToString() + " - " + _wosaPrintInfo.ItemCheckType + _tempChkAmount;

		}
		//Begin #3401
		private string GetBondInfo()
		{
			string _tempBondAmt = null;
			this.pbSkipChecks.ObjectId = 15;
			this.lblCheckItemInfo.Text = CoreService.Translation.GetUserMessageX(11265); //11265 - Bond Owner:
			_tempBondAmt = CurrencyHelper.GetFormattedValue(1, _wosaPrintInfo.BondTotalOrderAmt);
			_tempBondAmt = _tempBondAmt.PadLeft(38, '*');
			_tempBondAmt = _tempBondAmt.Replace("*", " ");
			//
			return _wosaPrintInfo.BondOwnerName + "  " + _tempBondAmt;

		}
		//End #3401

		//Begin #19415
		//to skip check for dormant account
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
		//End #19415


		#endregion

		#region private methods
		private void EnableDisableVisibleLogic(string callerInfo)
		{
			if (callerInfo == "FormComplete")
			{
				if (lastLinePrinted.Value < 0)
					dfPbLastLine.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Disable);
				else
					pbSkip.Enabled = false;
				if (formPrintNo.Value == 1)
					pbReprintLast.Enabled = false;
				if (_checkItemsOnly || showCheckItem)
				{
					pbSkipChecks.Visible = _checkItemsOnly;
					pbSkip.Enabled = true;
					//dfCheckItemInfo.Visible = true;
					lblCheckItemInfo.Visible = true;
					dfCheckItemInfo.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);	// #72063
				}
				else
				{
					pbSkipChecks.Visible = false;
					//dfCheckItemInfo.Visible = false;
					lblCheckItemInfo.Visible = false;
					dfCheckItemInfo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.DisableShowText);
				}
				//dfFormDescription.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);	// #72063
			}
		}

		/* Begin #71896 */
		private void Control_GotFocus(object sender, EventArgs e)
		{
			if (printFocussedCount < 3)
			{
				pbPrintForm.ImageButton.Focus();
				//this.Invalidate();
				printFocussedCount++;
			}
		}
		/* End #71896 */

		/* Begin #72063 */
		private void SetFieldReadOnly(PdfStandard dfField)
		{
			dfField.Font = new Font(dfField.Font, FontStyle.Bold);
			dfField.ReadOnly = true;
			dfField.BackColor = Color.White;
			dfField.TabStop = false;
		}

		private void dlgPrintForms_Load(object sender, EventArgs e)
		{

		}
		/* End #72063 */
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
			if (cmbXfsSvcs.Items.Count > 0)
				cmbXfsSvcs.SelectedIndex = 0;

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
