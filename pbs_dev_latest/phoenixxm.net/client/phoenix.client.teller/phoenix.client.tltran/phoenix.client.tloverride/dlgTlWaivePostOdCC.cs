#region Comments

//-------------------------------------------------------------------------------
// File Name: dlgTlWaivePostOdCC.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//02/11/09      1       DGupta      #76052 - Created
//05/23/2012    2       fspath      #142717 -  Set the IsCustomizablePrefs to false on the grid since 
//                                  columns are referenced by the index and this is not supported.
//07/20/2012    3       FOyebola    #140780 - Reg D Enhancement.
//01/22/2014    4       FOyebola    #161239 - Uncollected Funds Ovrdraft Protection
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
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

namespace Phoenix.Windows.TlOverride
{
	/// <summary>
	/// Summary description for dlgOverrideableErrors.
	/// </summary>
	public class dlgTlWaivePostOdCC : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbInstructions;
		private Phoenix.Windows.Forms.PGrid gridOverrideableErrors ;
		private Phoenix.Windows.Forms.PGridColumn colAccount;
        private Phoenix.Windows.Forms.PGridColumn colChargeType;
        private Phoenix.Windows.Forms.PGridColumn colAmount;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
        private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PAction pbPostCC;
        private Phoenix.Windows.Forms.PGroupBoxStandard gbAction;
		private Phoenix.Windows.Forms.PAction pbWaive;
        private Phoenix.Windows.Forms.PAction pbCancel;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbOverrides;
		private Phoenix.Windows.Forms.PLabelStandard lblInstruction;


		#region private vars
        private string caller;
		private ArrayList ovrdsArrays;
		private TlJournalOvrd _ovrdBusObj;
        //private string tlTranCode;
        //private decimal jrnlPtid;
		private TellerVars _tellerVars = TellerVars.Instance;
        private DialogResult dialogResult = DialogResult.None;
        //private PSmallInt _superEmplId;
        //private AdGbRsm _adGbRsm;
        //private AdGbRsmLimits _adGbRsmLimits;
        private PfwStandard _parentForm = null;
        private PLabelStandard lblTotalCharges;
        private PGroupBoxStandard gbTotal;
        private PdfCurrency dfTotalCharges;
        //private bool formRetValue = false;
        
     
		#endregion

		private TlJournalOvrd OvrdBusObj
		{
			get
			{
				if ( _ovrdBusObj == null )
					_ovrdBusObj = new TlJournalOvrd();
				return _ovrdBusObj;
			}
		}

        public dlgTlWaivePostOdCC()
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
            this.gbOverrides = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gridOverrideableErrors = new Phoenix.Windows.Forms.PGrid();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colChargeType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.dfTotalCharges = new Phoenix.Windows.Forms.PdfCurrency();
            this.lblTotalCharges = new Phoenix.Windows.Forms.PLabelStandard();
            this.gbAction = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.pbWaive = new Phoenix.Windows.Forms.PAction();
            this.pbPostCC = new Phoenix.Windows.Forms.PAction();
            this.gbTotal = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.gbInstructions.SuspendLayout();
            this.gbOverrides.SuspendLayout();
            this.gbTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbPostCC,
            this.pbWaive,
            this.pbCancel});
            // 
            // gbInstructions
            // 
            this.gbInstructions.Controls.Add(this.lblInstruction);
            this.gbInstructions.Location = new System.Drawing.Point(4, 0);
            this.gbInstructions.Name = "gbInstructions";
            this.gbInstructions.PhoenixUIControl.ObjectId = 1;
            this.gbInstructions.Size = new System.Drawing.Size(535, 91);
            this.gbInstructions.TabIndex = 1;
            this.gbInstructions.TabStop = false;
            this.gbInstructions.Text = "Instructions";
            // 
            // lblInstruction
            // 
            this.lblInstruction.AutoEllipsis = true;
            this.lblInstruction.Location = new System.Drawing.Point(7, 16);
            this.lblInstruction.Name = "lblInstruction";
            this.lblInstruction.PhoenixUIControl.ObjectId = 2;
            this.lblInstruction.Size = new System.Drawing.Size(522, 61);
            this.lblInstruction.TabIndex = 0;
            this.lblInstruction.Text = "The transaction you attempted to post rejected for the reason(s) below. With prop" +
    "er approval, these ";
            this.lblInstruction.WordWrap = true;
            // 
            // gbOverrides
            // 
            this.gbOverrides.Controls.Add(this.gridOverrideableErrors);
            this.gbOverrides.Location = new System.Drawing.Point(4, 97);
            this.gbOverrides.Name = "gbOverrides";
            this.gbOverrides.PhoenixUIControl.ObjectId = 19;
            this.gbOverrides.Size = new System.Drawing.Size(535, 298);
            this.gbOverrides.TabIndex = 0;
            this.gbOverrides.TabStop = false;
            this.gbOverrides.Text = "Overrides";
            // 
            // gridOverrideableErrors
            // 
            this.gridOverrideableErrors.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colAccount,
            this.colChargeType,
            this.colAmount,
            this.colAcctType,
            this.colAcctNo});
            this.gridOverrideableErrors.IsCustomizablePrefs = false;
            this.gridOverrideableErrors.ItemWordWrap = true;
            this.gridOverrideableErrors.LinesInHeader = 2;
            this.gridOverrideableErrors.Location = new System.Drawing.Point(5, 16);
            this.gridOverrideableErrors.MultiSelect = true;
            this.gridOverrideableErrors.Name = "gridOverrideableErrors";
            this.gridOverrideableErrors.Size = new System.Drawing.Size(526, 276);
            this.gridOverrideableErrors.TabIndex = 0;
            this.gridOverrideableErrors.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridOverrideableErrors_FetchRowDone);
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 4;
            this.colAccount.PhoenixUIControl.XmlTag = "Account";
            this.colAccount.Title = "Account";
            this.colAccount.Width = 107;
            // 
            // colChargeType
            // 
            this.colChargeType.PhoenixUIControl.ObjectId = 5;
            this.colChargeType.PhoenixUIControl.XmlTag = "OvrdType";
            this.colChargeType.Title = "Charge Type";
            this.colChargeType.Width = 144;
            // 
            // colAmount
            // 
            this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmount.PhoenixUIControl.ObjectId = 7;
            this.colAmount.PhoenixUIControl.XmlTag = "Amount";
            this.colAmount.Title = "Amount";
            this.colAmount.Width = 163;
            // 
            // colAcctType
            // 
            this.colAcctType.Title = "Column";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 0;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.XmlTag = "0";
            this.colAcctNo.Title = "Column";
            this.colAcctNo.Visible = false;
            this.colAcctNo.Width = 0;
            // 
            // dfTotalCharges
            // 
            this.dfTotalCharges.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCharges.Enabled = false;
            this.dfTotalCharges.Location = new System.Drawing.Point(431, 13);
            this.dfTotalCharges.MaxLength = 14;
            this.dfTotalCharges.Name = "dfTotalCharges";
            this.dfTotalCharges.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
            this.dfTotalCharges.PhoenixUIControl.ObjectId = 24;
            this.dfTotalCharges.Size = new System.Drawing.Size(100, 20);
            this.dfTotalCharges.TabIndex = 21;
            this.dfTotalCharges.Text = "$0.00";
            // 
            // lblTotalCharges
            // 
            this.lblTotalCharges.AutoEllipsis = true;
            this.lblTotalCharges.Location = new System.Drawing.Point(5, 13);
            this.lblTotalCharges.Name = "lblTotalCharges";
            this.lblTotalCharges.PhoenixUIControl.ObjectId = 186;
            this.lblTotalCharges.Size = new System.Drawing.Size(108, 20);
            this.lblTotalCharges.TabIndex = 1;
            this.lblTotalCharges.Text = "Total Charges:";
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
            // pbCancel
            // 
            this.pbCancel.LongText = "&Cancel";
            this.pbCancel.ObjectId = 14;
            this.pbCancel.ShortText = "&Cancel";
            this.pbCancel.Tag = null;
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // pbWaive
            // 
            this.pbWaive.LongText = "&Waive All";
            this.pbWaive.ObjectId = 13;
            this.pbWaive.ShortText = "&Waive All";
            this.pbWaive.Tag = null;
            this.pbWaive.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbWaive_Click);
            // 
            // pbPostCC
            // 
            this.pbPostCC.LongText = "&Charge All";
            this.pbPostCC.ObjectId = 12;
            this.pbPostCC.ShortText = "&Charge All";
            this.pbPostCC.Tag = null;
            this.pbPostCC.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPostCC_Click);
            // 
            // gbTotal
            // 
            this.gbTotal.Controls.Add(this.lblTotalCharges);
            this.gbTotal.Controls.Add(this.dfTotalCharges);
            this.gbTotal.Location = new System.Drawing.Point(4, 395);
            this.gbTotal.Name = "gbTotal";
            this.gbTotal.PhoenixUIControl.ObjectId = 20;
            this.gbTotal.Size = new System.Drawing.Size(535, 43);
            this.gbTotal.TabIndex = 22;
            this.gbTotal.TabStop = false;
            this.gbTotal.Text = "Totals";
            // 
            // dlgTlWaivePostOdCC
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(551, 448);
            this.Controls.Add(this.gbTotal);
            this.Controls.Add(this.gbAction);
            this.Controls.Add(this.gbOverrides);
            this.Controls.Add(this.gbInstructions);
            this.Name = "dlgTlWaivePostOdCC";
            this.ScreenId = 26016;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgTlWaivePostOdCC_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgTlWaivePostOdCC_PInitCompleteEvent);
            this.Closed += new System.EventHandler(this.dlgTlWaivePostOdCC_Closed);
            this.gbInstructions.ResumeLayout(false);
            this.gbOverrides.ResumeLayout(false);
            this.gbTotal.ResumeLayout(false);
            this.gbTotal.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public override void InitParameters(params object[] paramList)
		{		

				if ( paramList.Length > 1 )
					ovrdsArrays = paramList[1] as ArrayList;
                
			
		}

        private ReturnType dlgTlWaivePostOdCC_PInitBeginEvent()
		{
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = OvrdBusObj;
			this.ActionManager.ShowHiddenActions = false;
			return ReturnType.Success;
		}

        private void dlgTlWaivePostOdCC_PInitCompleteEvent()
		{
//			if (!isViewOnly)
//				LoadCharges(false);
//			if (gridOverrideableErrors.Items.Count > 0 && !isViewOnly)
//				gridOverrideableErrors.SelectRow(0,true);
            _parentForm = Workspace.ContentWindow.CurrentWindow as PfwStandard;

            if (caller != "JournalView")
            {
                this.lblInstruction.Text = @"
Press the 'Charge All' button to post the listed charge(s).
Press the 'Waive All' button to waive the listed charge(s).
Press the 'Cancel' button to return to the Post Transactions window without posting the transaction(s).";

                LoadOdCcOverrides(false);
            }

		}

		

	
		private void gridOverrideableErrors_FetchRowDone(object sender, GridRowArgs e)
		{
            //colTrancode.UnFormattedValue = GetTranCode( tlTranCode, Convert.ToString( colTrancode.UnFormattedValue ),
            //    Convert.ToString( colOvrdType.UnFormattedValue ));
            //colChargeType.UnFormattedValue = DecodeOvrdType(Convert.ToString(colChargeType.UnFormattedValue));
			if ( colAccount.UnFormattedValue != null && colAccount.UnFormattedValue.ToString().Trim() == "-" )
				colAccount.UnFormattedValue = String.Empty;
            //dfTotalCharges.UnFormattedValue = Convert.ToDecimal(dfTotalCharges.UnFormattedValue) + Convert.ToDecimal(colAmount.UnFormattedValue);
		}

		
		



		#region pbCancel
		private void pbCancel_Click( object sender, PActionEventArgs e )
		{
            this.dialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		

		

		#region pbWaive
		private void pbWaive_Click( object sender, PActionEventArgs e )
		{
            dialogResult = DialogResult.No;
            this.Close();
		}
		#endregion

		#region pbPostCC
		private void pbPostCC_Click( object sender, PActionEventArgs e )
		{
            dialogResult = DialogResult.Yes;    
            this.Close();

        
           
		}
        private void dlgTlWaivePostOdCC_Closed(object sender, EventArgs e)
        {
            if (_parentForm != null)
                _parentForm.CallParent("OdCcOverride", dialogResult);
        }
		#endregion



		#region private methods

	

		private string DecodeCCType( int ovrdId )
		{
            if (ovrdId == OverrideID.ODLCharge)
                return "ODL";
            if (ovrdId == OverrideID.ODPCharge)
                return "ODP";
            if (ovrdId == OverrideID.NSFCharge)
                return "NSF";
            //Begin #140780
            if (ovrdId == OverrideID.RegDCharge)
                return "REG D";
            //End #140780 
            //Begin #161239
            if (ovrdId == OverrideID.UCFCharge)	
                return "UCF";
            //End #161239 
			
			return String.Empty;
		}

        //private string GetTranCode( string tlTranCode, string tranCode, string ovrdType )
        //{
        //    if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "T" ))
        //        return tranCode;
        //    return tlTranCode;
        //}



		private void LoadOverride( TlJournalOvrd ovrd )
		{
			GLItem listItem = new GLItem();
			string acctType;
			string acctNo;

			if (!ovrd.AcctNo.IsNull && !ovrd.AcctType.IsNull &&
				ovrd.AcctNo.Value.Trim() != String.Empty &&
				ovrd.AcctType.Value.Trim() != String.Empty )
			{
				acctType = ovrd.AcctType.Value;
				acctNo = ovrd.AcctNo.Value;
				listItem.SubItems.Add(acctType + "-" + acctNo);
			}
			else
			{
				listItem.SubItems.Add(String.Empty );
				acctType = String.Empty;
				acctNo = String.Empty;
			}
            listItem.SubItems.Add(DecodeCCType(ovrd.OvrdId.Value));
            //_tellerVars.SetContextObject("PcOverrideArray", ovrd.OvrdId.Value );
            //listItem.SubItems.Add(DecodeOvrdType( _tellerVars.PcOverride.OvrdType.Value));
			// handle monetary columns here
            // Begin 72995
            //if (!ovrd.ItemNo.IsNull)
            //{
            //    listItem.SubItems.Add(Convert.ToString(ovrd.TranCode.Value));
            //}
            //else
            //{
            //    listItem.SubItems.Add(GetTranCode(ovrd.TlTranCode.Value,
            //                            Convert.ToString(ovrd.TranCode.Value),
            //                            _tellerVars.PcOverride.OvrdType.Value));
            //}
            //// End 72995	

            //Begin #140780
            if (ovrd.OvrdId.Value == OverrideID.RegDCharge)
            {
                listItem.SubItems.Add(gridOverrideableErrors.Columns[3].MakeFormattedValue(
                                    ovrd.RegdCcAmt.IsNull ? 0 : ovrd.RegdCcAmt.Value));
            }
            else
            {
                listItem.SubItems.Add(gridOverrideableErrors.Columns[3].MakeFormattedValue(
                    ovrd.OdCcAmount.IsNull ? 0 : ovrd.OdCcAmount.Value));
            }
            //End #140780		

            //listItem.SubItems.Add(ovrd.ItemNo.IsNull ? String.Empty : ovrd.ItemNo.Value.ToString());
            //listItem.SubItems.Add( _tellerVars.PcOverride.TellerDescription.IsNull ?
            //    _tellerVars.PcOverride.Description.Value : _tellerVars.PcOverride.TellerDescription.Value );
//			if ( !ovrd.SuperEmplName.IsNull )
//				listItem.SubItems.Add( ovrd.SuperEmplName.Value );
//			else
                //listItem.SubItems.Add(  ovrd.SuperEmplId.IsNull ? String.Empty : GetSuperEmplName( ovrd.SuperEmplId.Value ));
            //listItem.SubItems.Add(ovrd.SuperEmplId.IsNull ? String.Empty : ovrd.SuperEmplId.Value.ToString());
            //listItem.SubItems.Add( acctType );
            //listItem.SubItems.Add( acctNo );
            //listItem.SubItems.Add( ovrd.MiscAcctInfo.DepType == null ? String.Empty : ovrd.MiscAcctInfo.DepType );
            //listItem.SubItems.Add(ovrd.MiscAcctInfo.AcctId < 0 ? String.Empty : ovrd.MiscAcctInfo.AcctId.ToString());
            //listItem.SubItems.Add( ovrd.MiscAcctInfo.RimNo < 0 ? String.Empty : ovrd.MiscAcctInfo.RimNo.ToString() );
            //listItem.SubItems.Add( ovrd.MiscAcctInfo.DepLoan == null ? String.Empty : ovrd.MiscAcctInfo.DepLoan );
            //listItem.SubItems.Add( ovrd.MiscAcctInfo.ViewAccess == null ? GlobalVars.Instance.ML.Y : ovrd.MiscAcctInfo.ViewAccess );
			gridOverrideableErrors.Items.Add( listItem );

            //Begin #140780
            if (ovrd.OvrdId.Value == OverrideID.RegDCharge)
            {
                dfTotalCharges.UnFormattedValue = Convert.ToDecimal(dfTotalCharges.UnFormattedValue) + ovrd.RegdCcAmt.Value;
            }
            else
            {
                dfTotalCharges.UnFormattedValue = Convert.ToDecimal(dfTotalCharges.UnFormattedValue) + ovrd.OdCcAmount.Value;
            }
            //End #140780
		}

      
        private void LoadOdCcOverrides(bool isLoadBackList)
        {
            if (!isLoadBackList)
            {
                gridOverrideableErrors.ResetTable();                
                foreach (TlJournalOvrd ovrd in ovrdsArrays)
                {
                        LoadOverride(ovrd);
                }  
            }
            //else
            //{
            //    int contextRow = 0;            

            //        //foreach (TlJournalOvrd ovrd in ovrdsArrays)
            //        //{
            //        //    //SetContextRow(contextRow);
            //        //    //contextRow++;
            //        //    if (colSuperEmplId.UnFormattedValue != null && colSuperEmplId.UnFormattedValue.ToString() != String.Empty)
            //        //    {
            //        //        ovrd.SuperEmplId.Value = Convert.ToInt16(colSuperEmplId.UnFormattedValue);
            //        //        //ovrd.SuperEmplName.Value = Convert.ToString(colOverridenby.UnFormattedValue);
            //        //    }
            //        //}                    
              
            //}
        }

	

		
		#endregion

 

		

		
	}
}
