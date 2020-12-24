#region Comments

//-------------------------------------------------------------------------------
// File Name: dlgTlFloatInfo.cs
// NameSpace: Phoenix.Client.Teller
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//04/23/2011    1       rpoddar     #79420, #13851 - Float Changes
//08/16/2011    2       LSimpson    #15065 - Do not automatically close dialog when saving.
//09/22/2014    3       rpoddar     #161243, #30710 - Flaot fixes
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Serialization;
//
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using GlacialComponents.Controls;
using GlacialComponents.Controls.Common;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.BusObj.Admin.Global;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Global;
using Phoenix.BusObj.Admin.Teller;
using Phoenix.FrameWork.Core.Utilities;

namespace Phoenix.Client.Teller
{
	/// <summary>
	/// Summary description for dlgTlFloatInfo.
	/// </summary>
	public class dlgTlFloatInfo : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        private Phoenix.Windows.Forms.PGroupBoxStandard gbInstructions;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbAction;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbFloatInfo;
		private Phoenix.Windows.Forms.PLabelStandard lblInstruction;
        private Phoenix.Windows.Forms.PdfStandard dfFloatDays;
        private Phoenix.Windows.Forms.PdfStandard dfFloatDate;
        private Phoenix.Windows.Forms.PLabelStandard lblFloatDays;
        private Phoenix.Windows.Forms.PLabelStandard lblFloatDate;
        private Phoenix.Windows.Forms.PLabelStandard lblExceptRsnCode;
        private Phoenix.Windows.Forms.PComboBoxStandard cmbExceptRsnCode;


        private PBaseType exceptRsnCode = null;
        private PBaseType floatDays = null;
        private TlItemCapture _busObjCapturedItems = null;
        private DateTime tranEffectiveDt;
        private bool zeroFloatCall = false;
        private bool isMemoMode = false;    // #14799
        private bool formChanged = false;   // #15065
        private PBaseType calcFloatDays = null;         //#79420, #15357
        private short _globalRegCFloatDays = 0;        //#161243, #30710


		public dlgTlFloatInfo()
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



		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.gbInstructions = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lblInstruction = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbFloatInfo = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.dfFloatDate = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFloatDate = new Phoenix.Windows.Forms.PLabelStandard();
            this.dfFloatDays = new Phoenix.Windows.Forms.PdfStandard();
            this.lblFloatDays = new Phoenix.Windows.Forms.PLabelStandard();
            this.cmbExceptRsnCode = new Phoenix.Windows.Forms.PComboBoxStandard();
            this.lblExceptRsnCode = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbAction = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbInstructions.SuspendLayout();
            this.gbFloatInfo.SuspendLayout();
            this.SuspendLayout();
            //
            // gbInstructions
            //
            this.gbInstructions.Controls.Add(this.lblInstruction);
            this.gbInstructions.Location = new System.Drawing.Point(4, 0);
            this.gbInstructions.Name = "gbInstructions";
            this.gbInstructions.PhoenixUIControl.ObjectId = 5;
            this.gbInstructions.Size = new System.Drawing.Size(488, 80);
            this.gbInstructions.TabIndex = 1;
            this.gbInstructions.TabStop = false;
            this.gbInstructions.Text = "Instructions";
            //
            // lblInstruction
            //
            this.lblInstruction.AutoEllipsis = true;
            this.lblInstruction.Location = new System.Drawing.Point(7, 16);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.PhoenixUIControl.ObjectId = 6;
            this.lblInstruction.Size = new System.Drawing.Size(477, 60);
            this.lblInstruction.TabIndex = 0;
            this.lblInstruction.Text = "The transaction you attempted to post rejected for the reason(s) below. With prop" +
                "er approval, these ";
            this.lblInstruction.WordWrap = true;
            //
            // gbFloatInfo
            //
            this.gbFloatInfo.Controls.Add(this.dfFloatDate);
            this.gbFloatInfo.Controls.Add(this.lblFloatDate);
            this.gbFloatInfo.Controls.Add(this.dfFloatDays);
            this.gbFloatInfo.Controls.Add(this.lblFloatDays);
            this.gbFloatInfo.Controls.Add(this.cmbExceptRsnCode);
            this.gbFloatInfo.Controls.Add(this.lblExceptRsnCode);
            this.gbFloatInfo.Location = new System.Drawing.Point(4, 80);
            this.gbFloatInfo.Name = "gbFloatInfo";
            this.gbFloatInfo.PhoenixUIControl.ObjectId = 4;
            this.gbFloatInfo.Size = new System.Drawing.Size(488, 68);
            this.gbFloatInfo.TabIndex = 0;
            this.gbFloatInfo.TabStop = false;
            this.gbFloatInfo.Text = "Float Info";
            //
            // dfFloatDate
            //
            this.dfFloatDate.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFloatDate.Enabled = false;
            this.dfFloatDate.Location = new System.Drawing.Point(380, 40);
            this.dfFloatDate.Name = "dfFloatDate";
            this.dfFloatDate.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
            this.dfFloatDate.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
            this.dfFloatDate.PhoenixUIControl.ObjectId = 3;
            this.dfFloatDate.Size = new System.Drawing.Size(100, 20);
            this.dfFloatDate.TabIndex = 5;
            //
            // lblFloatDate
            //
            this.lblFloatDate.AutoEllipsis = true;
            this.lblFloatDate.Location = new System.Drawing.Point(236, 40);
            this.lblFloatDate.Name = "lblFloatDate";
            this.lblFloatDate.PhoenixUIControl.ObjectId = 3;
            this.lblFloatDate.Size = new System.Drawing.Size(144, 20);
            this.lblFloatDate.TabIndex = 4;
            this.lblFloatDate.Text = "Expiry Date";
            //
            // dfFloatDays
            //
            this.dfFloatDays.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfFloatDays.Location = new System.Drawing.Point(116, 40);
            this.dfFloatDays.Name = "dfFloatDays";
            this.dfFloatDays.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
            this.dfFloatDays.PhoenixUIControl.InputMask = "99";
            this.dfFloatDays.PhoenixUIControl.ObjectId = 2;
            this.dfFloatDays.Size = new System.Drawing.Size(40, 20);
            this.dfFloatDays.TabIndex = 3;
            this.dfFloatDays.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfFloatDays_PhoenixUIValidateEvent);
            //
            // lblFloatDays
            //
            this.lblFloatDays.AutoEllipsis = true;
            this.lblFloatDays.Location = new System.Drawing.Point(8, 40);
            this.lblFloatDays.Name = "lblFloatDays";
            this.lblFloatDays.PhoenixUIControl.ObjectId = 2;
            this.lblFloatDays.Size = new System.Drawing.Size(100, 20);
            this.lblFloatDays.TabIndex = 2;
            this.lblFloatDays.Text = "Float Days:";
            //
            // cmbExceptRsnCode
            //
            this.cmbExceptRsnCode.Location = new System.Drawing.Point(116, 12);
            this.cmbExceptRsnCode.Name = "cmbExceptRsnCode";
            this.cmbExceptRsnCode.PhoenixUIControl.ObjectId = 1;
            this.cmbExceptRsnCode.Size = new System.Drawing.Size(364, 21);
            this.cmbExceptRsnCode.TabIndex = 1;
            this.cmbExceptRsnCode.Value = null;

            //
            // lblExceptRsnCode
            //
            this.lblExceptRsnCode.AutoEllipsis = true;
            this.lblExceptRsnCode.Location = new System.Drawing.Point(8, 12);
            this.lblExceptRsnCode.Name = "lblExceptRsnCode";
            this.lblExceptRsnCode.PhoenixUIControl.ObjectId = 1;
            this.lblExceptRsnCode.Size = new System.Drawing.Size(100, 20);
            this.lblExceptRsnCode.TabIndex = 0;
            this.lblExceptRsnCode.Text = "Except Rsn Code:";
            //
            // gbAction
            //
            this.gbAction.Location = new System.Drawing.Point(676, 0);
            this.gbAction.Name = "gbAction";
            this.gbAction.PhoenixUIControl.ObjectId = 18;
            this.gbAction.Size = new System.Drawing.Size(0, 0);
            this.gbAction.TabIndex = 2;
            this.gbAction.TabStop = false;
            this.gbAction.Text = "Action";
            this.gbAction.Visible = false;
            //
            // dlgTlFloatInfo
            //
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(494, 153);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.gbFloatInfo);
            this.Controls.Add(this.gbInstructions);
            this.Name = "dlgTlFloatInfo";
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.Editable;
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgTlFloatInfo_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgTlFloatInfo_PInitBeginEvent);
            this.PShowCompletedEvent += new System.EventHandler(this.dlgTlFloatInfo_PShowCompletedEvent);
            this.gbInstructions.ResumeLayout(false);
            this.gbFloatInfo.ResumeLayout(false);
            this.gbFloatInfo.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public override void InitParameters(params object[] paramList)
		{
            if (paramList != null && paramList.Length > 3)
            {
                tranEffectiveDt = Convert.ToDateTime(paramList[0]);
                zeroFloatCall = Convert.ToBoolean(paramList[1]);
                exceptRsnCode = paramList[2] as PBaseType;
                floatDays = paramList[3] as PBaseType;

                //Begin #14799
                if (paramList.Length > 4)
                {
                    isMemoMode = Convert.ToBoolean(paramList[4]);
                }
                //End #14799

                //Begin #79420, #15357
                if (paramList.Length > 5)
                {
                    calcFloatDays = paramList[5] as PBaseType;
                }
                //End #79420, #15357

                //Begin #161243, #30710
                if (paramList.Length > 6)
                {
                    _globalRegCFloatDays = Convert.ToInt16( paramList[6] );
                }
                //End #161243, #30710


            }
		}

		private ReturnType dlgTlFloatInfo_PInitBeginEvent()
		{
			this.ActionManager.ShowHiddenActions = false;
            _busObjCapturedItems = new TlItemCapture();
            this.ScreenId = Phoenix.Shared.Constants.ScreenId.TlFloatInfo;

            ActionSave.NextScreenId = 0;

			return ReturnType.Success;
		}

		private void dlgTlFloatInfo_PInitCompleteEvent()
		{
            //if (zeroFloatCall)
            //    this.lblInstruction.Text = @"You have decreased the Make Available amount that affects a float item with zero float days. Please modify the float days and then change the Make Available amount.";
            //else
            //    this.lblInstruction.Text = @"Enter the Exception Reason Code/ Float Days for the selected item.";

            if (zeroFloatCall)
                this.lblInstruction.Text = CoreService.Translation.GetTranslateX(ScreenId, 1);
            else
                this.lblInstruction.Text = CoreService.Translation.GetTranslateX(ScreenId, 2);

            if ( TellerVars.Instance.IsAppOnline)
            {
                if (TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCode"] == null)
                {
                    CallXMThruCDS("MiscComboPopulate");
                    TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCode"] = _busObjCapturedItems.ExceptRsnCode.Constraint.EnumValues;
                    // Begin #161243
                    ArrayList enumsWithoutLD = _busObjCapturedItems.ExceptRsnCode.Constraint.EnumValues.Clone() as ArrayList;
                    ArrayList enumsWithoutNA = _busObjCapturedItems.ExceptRsnCode.Constraint.EnumValues.Clone() as ArrayList;
                    ArrayList enumsWithoutLDNA = _busObjCapturedItems.ExceptRsnCode.Constraint.EnumValues.Clone() as ArrayList;
                    EnumValue enumLD = null;
                    EnumValue enumNA = null;
                    if (enumsWithoutLD != null)
                    {
                        foreach (EnumValue e in enumsWithoutLD)
                        {
                            if (Convert.ToInt16(e.CodeValue) == 5)
                            {
                                enumLD = e;
                            }
                            else if (Convert.ToInt16(e.CodeValue) == 2)
                            {
                                enumNA = e;
                            }
                        }
                    }
                    if (enumLD != null)
                    {
                        enumsWithoutLD.Remove(enumLD);
                        enumsWithoutLDNA.Remove(enumLD);
                    }
                    if (enumNA != null)
                    {
                        enumsWithoutNA.Remove(enumNA);
                        enumsWithoutLDNA.Remove(enumNA);
                    }
                    TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoLD"] = enumsWithoutLD;
                    TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoNA"] = enumsWithoutNA;
                    TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoLDNA"] = enumsWithoutLDNA;
                    // End #161243
                }

                // Begin #161243
                //cmbExceptRsnCode.Populate(TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCode"], null);
                short inputExceptRsnCode = exceptRsnCode.IsNull ? (short)0 : Convert.ToInt16(exceptRsnCode.Value);
                if ( inputExceptRsnCode == 2 )
                    cmbExceptRsnCode.Populate(TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoLD"], null);
                else if ( inputExceptRsnCode == 5 )
                    cmbExceptRsnCode.Populate(TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoNA"], null);
                else
                    cmbExceptRsnCode.Populate(TellerVars.Instance.ComboCache["dlgTlFloatInfo.cmbExceptRsnCodeNoLDNA"], null);
                // End #161243
            }

            if (!exceptRsnCode.IsNull)
            {
                cmbExceptRsnCode.SetValue(exceptRsnCode.Value);
                cmbExceptRsnCode_PhoenixUISelectedIndexChangedEvent(null, null);    //#79420, #15357
            }

            if (!floatDays.IsNull)
            {
                dfFloatDays.SetValue(floatDays.Value);
                dfFloatDays_PhoenixUIValidateEvent(null, null);
            }

            //Begin #79420, #15357
            EnableDisableVisibleLogic("InitComplete");
            this.cmbExceptRsnCode.PhoenixUISelectedIndexChangedEvent += new Phoenix.Windows.Forms.SelectedIndexChangedEventHandler(this.cmbExceptRsnCode_PhoenixUISelectedIndexChangedEvent);
            //if (zeroFloatCall)
            //    dfFloatDays.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Default, EnableState.Default);
            //else if ( isMemoMode )
            //    dfFloatDays.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);    // #14799
            //End #79420, #15357

            DialogResult = DialogResult.Cancel;
 		}

        void dlgTlFloatInfo_PShowCompletedEvent(object sender, EventArgs e)
        {

        }

        #region #call parent
        public override void CallParent(params object[] paramList)
        {

        }
        #endregion

        private void CallXMThruCDS(string origin)
        {
            if (origin == "MiscComboPopulate")
            {
                Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(_busObjCapturedItems, _busObjCapturedItems.ExceptRsnCode);
            }

        }

        public override bool OnActionSave(bool isAddNext)
        {
            #region #15065
            if (!this.IsDirty)
            {
                PMessageBox.Show(300046, MessageType.NoChanges, MessageBoxButtons.OK, String.Empty);

                return false;
            }
            #endregion

            if (!PerformCheck(CheckType.NullTest, true))
            {
                dfFloatDays.Focus();
                return false;
            }

            if (exceptRsnCode != null)
                exceptRsnCode.Value = cmbExceptRsnCode.Value;

            if (floatDays != null)
                floatDays.Value = dfFloatDays.Value;

            //#15065 - Commented - this.DialogResult = DialogResult.OK;
            //#15065 - Commented - this.Close();
            formChanged = true; // #15065
            return true;
        }

        public override bool OnActionClose()
        {
            bool isSuccess = true;
            //if( AvoidSave == false && 	(HasWriteAccess || && this.IsDirty  ) //#mramalin: #74705 - Unsaved Changes is not prompted, if the the window has New only access
            if (this.IsDirty)
            {
                DialogResult result = PMessageBox.Show(300047, MessageType.UnsavedChanges, MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Cancel)
                    isSuccess = false;
                else if (result == DialogResult.Yes)
                    isSuccess = OnActionSave(false);

            }

            AvoidSave = true;
            #region #15065
            if (formChanged)
                this.DialogResult = DialogResult.OK;
            #endregion

            return isSuccess && base.OnActionClose();
        }


        public override bool IsFormDirty()
        {
            short prevVal = -1;
            short newVal = -1;

            if (cmbExceptRsnCode.CodeValue != null)
                newVal = Convert.ToInt16(cmbExceptRsnCode.CodeValue);

            if (exceptRsnCode != null && !exceptRsnCode.IsNull)
                prevVal = Convert.ToInt16(exceptRsnCode.Value);

            if (newVal != prevVal)
                return true;

            prevVal = -1;
            newVal = -1;

            if (dfFloatDays.Value != null)
                newVal = Convert.ToInt16(dfFloatDays.Value);

            if (floatDays != null && !floatDays.IsNull)
                prevVal = Convert.ToInt16(floatDays.Value);

            if (newVal != prevVal)
                return true;

            return false;

        }

        private void dfFloatDays_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
        {
            _busObjCapturedItems.TranEffectiveDt.Value = tranEffectiveDt;
            _busObjCapturedItems.FloatDays.Value = Convert.ToInt16(dfFloatDays.Value);

            //Begin #79420, #15357
            if (sender == cmbExceptRsnCode )
            {
                short floatDays = 0;

                if (!zeroFloatCall && calcFloatDays != null && _busObjCapturedItems.ExtendCalcDaysForExceptRsn(Convert.ToInt16(cmbExceptRsnCode.CodeValue)))    // #161243 - Added check for ExtendCalcDaysForExceptRsn
                    floatDays += (calcFloatDays.IsNull ? (short)0 : (short)calcFloatDays.IntValue);

                //Begin #161243, #30710
                if (!_busObjCapturedItems.ExtendCalcDaysForExceptRsn(Convert.ToInt16(cmbExceptRsnCode.CodeValue)) && _globalRegCFloatDays > 0
                    && Convert.ToInt16(cmbExceptRsnCode.CodeValue) != 2 && Convert.ToInt16(cmbExceptRsnCode.CodeValue) != 5)
                    floatDays += _globalRegCFloatDays;
                //End #161243, #30710

                floatDays += _busObjCapturedItems.GetExceptRsnFloatDays(Convert.ToInt16(cmbExceptRsnCode.CodeValue));

                // Begin #161243
                if (floatDays == 0 && (Convert.ToInt16(cmbExceptRsnCode.CodeValue) == 2 || Convert.ToInt16(cmbExceptRsnCode.CodeValue) == 5))
                    floatDays = (calcFloatDays.IsNull ? (short)0 : (short)calcFloatDays.IntValue);
                // End #161243

                if (Convert.ToInt16(dfFloatDays.Value) == floatDays)    // if days are same as current on form, then no need to recalculate
                    return;

                _busObjCapturedItems.FloatDays.Value = floatDays;
                dfFloatDays.SetValue(floatDays);
            }
            //End #79420, #15357

            string focusFieldName = null;
            if (_busObjCapturedItems.ValidateFloatDays( TellerVars.Instance, ref focusFieldName ))
            {
                dfFloatDate.SetValue(_busObjCapturedItems.FloatDate.Value);
            }
            else
            {
                if ( sender != null )
                    PMessageBox.Show(this, _busObjCapturedItems.ActionReturnCode.Value, MessageType.Warning, MessageBoxButtons.OK, string.Empty);

                if (e != null)
                {
                    e.Cancel = true;
                }
            }
        }

        //Begin #79420, #15357
        private void cmbExceptRsnCode_PhoenixUISelectedIndexChangedEvent(object sender, PEventArgs e)
        {
            EnableDisableVisibleLogic("cmbExceptRsnCodeIndexChanged");
            dfFloatDays_PhoenixUIValidateEvent(sender, null);

        }

        private void EnableDisableVisibleLogic(string origin)
        {
            if (origin == "cmbExceptRsnCodeIndexChanged")
            {
                if (cmbExceptRsnCode.SelectedIndex >= 0 )
                    dfFloatDays.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Default, EnableState.Default);
                else
                    dfFloatDays.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Default);    // #14799

                // Begin #161243
                if (cmbExceptRsnCode.SelectedIndex >= 0 && Convert.ToInt16(cmbExceptRsnCode.CodeValue) == 2 || Convert.ToInt16(cmbExceptRsnCode.CodeValue) == 5 )
                    dfFloatDays.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);
                else if ( !isMemoMode ) 
                    dfFloatDays.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.Enable);    // #14799
                // End #161243
            }

            if (origin == "InitComplete" || origin == "cmbExceptRsnCodeIndexChanged" )
            {
                if (zeroFloatCall)
                    dfFloatDays.SetObjectStatus(NullabilityState.NotNull, VisibilityState.Default, EnableState.Default);
                else if (isMemoMode)
                    dfFloatDays.SetObjectStatus(NullabilityState.Default, VisibilityState.Default, EnableState.DisableShowText);    // #14799
            }
        }
        //End #79420, #15357




	}
}
