#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

//-------------------------------------------------------------------------------
// File Name: frmTlTrancode.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//12/23/2008    5.0     mselvaga    #76458 - Ex mask account# changes added.
//08/28/2009    2		SDhamija    #79156 - changed lbLastAccts from list box to PGrid. 
//									added CustName, RimNo
//03/03/2010    3		SDhamija    #7987 - always display RIM# and customer name if its a credit union.
//11May2010     4       dfutcher    8914 GL Accounts can be null. Don't list in Last Accts
//05/23/2012    5       fspath      #142717 -  Set the IsCustomizablePrefs to false on the grid since 
//                                  columns are referenced by the index and this is not supported.
//01/06/2015    6       BSchlotttman #180274 - Add secondary customer search
//02/02/2015    7       BSchlotttman #34405 - Add Owner Executing Transaction
//05/15/2018    8       FOyebola    Enh#220316 - Task 89989
//---------------------------------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Variables;
using GlacialComponents.Controls;	//#79156

namespace Phoenix.Windows.Client
{
	/// <summary>
	/// Summary description for dlgLastAccts.
	/// </summary>
	public class dlgLastAccts : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbLastAccountInformation;
		private Phoenix.Windows.Forms.PGrid lbLastAccts;	//#79156
		private Phoenix.Windows.Forms.PLabelStandard lblSelect;
		private TellerVars _tellerVars = TellerVars.Instance;
		private Phoenix.Windows.Forms.PAction pbOk;
		private Phoenix.Windows.Forms.PAction pbCancel;
		private PGridColumn colRimNo;
		private PGridColumn colAcctType;
		private PGridColumn colAcctNo;
		private PGridColumn colCustName;
		private PGridColumn colNickname;
        private PGridColumn colOwner;   //#34405
		private AcctInfo _acctInfo = null;
        private PGridColumn colTabTranAcct; //89989
        private string FocusedTab; //89989


        public class MyListBox:ListBox
		{

			protected override void CreateHandle()
			{
				if(!this.IsDisposed)
					base.CreateHandle ();    
			}

		}

		public dlgLastAccts()
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
            this.gbLastAccountInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.lbLastAccts = new Phoenix.Windows.Forms.PGrid();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colCustName = new Phoenix.Windows.Forms.PGridColumn();
            this.colNickname = new Phoenix.Windows.Forms.PGridColumn();
            this.colOwner = new Phoenix.Windows.Forms.PGridColumn();
            this.lblSelect = new Phoenix.Windows.Forms.PLabelStandard();
            this.pbOk = new Phoenix.Windows.Forms.PAction();
            this.pbCancel = new Phoenix.Windows.Forms.PAction();
            this.colTabTranAcct = new Phoenix.Windows.Forms.PGridColumn();
            this.gbLastAccountInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbOk,
            this.pbCancel});
            // 
            // gbLastAccountInformation
            // 
            this.gbLastAccountInformation.Controls.Add(this.lbLastAccts);
            this.gbLastAccountInformation.Controls.Add(this.lblSelect);
            this.gbLastAccountInformation.Location = new System.Drawing.Point(4, 0);
            this.gbLastAccountInformation.Name = "gbLastAccountInformation";
            this.gbLastAccountInformation.PhoenixUIControl.ObjectId = 1;
            this.gbLastAccountInformation.Size = new System.Drawing.Size(578, 196);
            this.gbLastAccountInformation.TabIndex = 0;
            this.gbLastAccountInformation.TabStop = false;
            this.gbLastAccountInformation.Text = "Last Account Information";
            // 
            // lbLastAccts
            // 
            this.lbLastAccts.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colAcctType,
            this.colAcctNo,
            this.colRimNo,
            this.colCustName,
            this.colNickname,
            this.colOwner,
            this.colTabTranAcct});
            this.lbLastAccts.IsCustomizablePrefs = false;
            this.lbLastAccts.IsMaxNumRowsCustomized = false;
            this.lbLastAccts.Location = new System.Drawing.Point(4, 44);
            this.lbLastAccts.Name = "lbLastAccts";
            this.lbLastAccts.Size = new System.Drawing.Size(568, 147);
            this.lbLastAccts.TabIndex = 1;
            this.lbLastAccts.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.lbLastAccts_SelectedIndexChanged);
            this.lbLastAccts.DoubleClick += new System.EventHandler(this.lbLastAccts_DoubleClick);
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.ObjectId = 7;
            this.colAcctType.Title = "acct type";
            this.colAcctType.Width = 60;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.ObjectId = 8;
            this.colAcctNo.PhoenixUIControl.XmlTag = "0";
            this.colAcctNo.Title = "acct no";
            // 
            // colRimNo
            // 
            this.colRimNo.PhoenixUIControl.ObjectId = 9;
            this.colRimNo.PhoenixUIControl.XmlTag = "1";
            this.colRimNo.Title = "rim no";
            this.colRimNo.Width = 60;
            // 
            // colCustName
            // 
            this.colCustName.PhoenixUIControl.ObjectId = 10;
            this.colCustName.PhoenixUIControl.XmlTag = "2";
            this.colCustName.Title = "cust name";
            this.colCustName.Width = 120;
            // 
            // colNickname
            // 
            this.colNickname.PhoenixUIControl.ObjectId = 11;
            this.colNickname.PhoenixUIControl.XmlTag = "3";
            this.colNickname.Title = "nickname";
            this.colNickname.Width = 150;
            // 
            // colOwner
            // 
            this.colOwner.PhoenixUIControl.ObjectId = 12;
            this.colOwner.PhoenixUIControl.XmlTag = "4";
            this.colOwner.Title = "Owner Executing Transaction";
            this.colOwner.Width = 200;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoEllipsis = true;
            this.lblSelect.Location = new System.Drawing.Point(4, 16);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.PhoenixUIControl.ObjectId = 6;
            this.lblSelect.Size = new System.Drawing.Size(491, 25);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Text = "Select the account you wish to work with from the list below:";
            this.lblSelect.WordWrap = true;
            // 
            // pbOk
            // 
            this.pbOk.Name = null;
            this.pbOk.ObjectId = 4;
            this.pbOk.Tag = null;
            this.pbOk.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOk_Click);
            // 
            // pbCancel
            // 
            this.pbCancel.Name = null;
            this.pbCancel.ObjectId = 5;
            this.pbCancel.Tag = null;
            this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
            // 
            // colTabTranAcct
            // 
            this.colTabTranAcct.PhoenixUIControl.ObjectId = 13;
            this.colTabTranAcct.PhoenixUIControl.XmlTag = "5";
            this.colTabTranAcct.Title = "Tab Account";
            // 
            // dlgLastAccts
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(584, 196);
            this.Controls.Add(this.gbLastAccountInformation);
            this.Name = "dlgLastAccts";
            this.ScreenId = 10878;
            this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgLastAccts_PInitBeginEvent);
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgLastAccts_PInitCompleteEvent);
            this.gbLastAccountInformation.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		public override void InitParameters(params object[] paramList)
		{
			if( paramList.Length == 1 )
				_acctInfo = paramList[0] as AcctInfo;

            //Begin 89989
            if (paramList.Length == 2)
            {
                _acctInfo = paramList[0] as AcctInfo;
                FocusedTab = Convert.ToString(paramList[1]);
            }
            //Begin 89989

        }
        #endregion

        #region form related events
        private Phoenix.Windows.Forms.ReturnType dlgLastAccts_PInitBeginEvent()
		{
			//this.AppToolBarId = ToolbarType.TB_Processing;
			//this.MainBusinesObject = _tlTranSet.CurTran;
			return ReturnType.Success;

		}

		private void dlgLastAccts_PInitCompleteEvent()
		{
			bool showName = false;
			bool showRimNo = false;
			bool offline = !_tellerVars.IsAppOnline;
			
			this.pbOk.Image = Images.Ok;
			this.pbCancel.Image = Images.Cancel;
			this.DefaultAction = this.pbOk;
			GLItem listItem = null;
			

			foreach( AcctInfo acctInfo in _tellerVars.LastAccts )
			{
                // Begin #76041 - #2032 changed order for null validation
                //this.lbLastAccts.Items.Add(acctInfo.AcctType + "-" + acctInfo.AcctNo);
                /*
				 * commented for #79156 - changed lbLastAccts from list box to grid.
				 * if (acctInfo.AcctNickname != null)
				{
					if (acctInfo.AcctNickname != string.Empty && acctInfo.AcctNickname.Trim() != "")
						this.lbLastAccts.Items.Add(acctInfo.AcctType + "-" + acctInfo.AcctNo + "-" + acctInfo.AcctNickname);
					else
						this.lbLastAccts.Items.Add(acctInfo.AcctType + "-" + acctInfo.AcctNo);
				}
				else
					this.lbLastAccts.Items.Add(acctInfo.AcctType + "-" + acctInfo.AcctNo);
				 */

                //Begin 89989
                if (FocusedTab == "Quick" && acctInfo.TabTranAcct == "Input")
                {
                    // Exclude input transaction accoounts from Quick tab call
                    continue;
                }
                //End 89989

                // Begin #79156
                listItem = this.lbLastAccts.Items[this.lbLastAccts.AddNewRow()];
				listItem.SubItems.Add(acctInfo.AcctType.ToString());

                // GL trans can post without an account number. Display blank
                if (string.IsNullOrEmpty(acctInfo.AcctNo)) //8914
                {
                    listItem.SubItems.Add("");
                }
                else
                {
                    listItem.SubItems.Add(acctInfo.AcctNo.ToString());
                }
				
                // RIM is null for GL. Display blank //8914
                if (acctInfo.RimNo == int.MinValue)
                {
                    listItem.SubItems.Add("");
                }
                else
                {
                    listItem.SubItems.Add(acctInfo.RimNo.ToString());
                }


				listItem.SubItems.Add(acctInfo.CustName.ToString());	// customer name
				if (acctInfo.AcctNickname != null)
					listItem.SubItems.Add(acctInfo.AcctNickname.ToString());
				// End #79156
                listItem.SubItems.Add(acctInfo.Owner.ToString());   //#34405

                listItem.SubItems.Add(acctInfo.TabTranAcct.ToString());   //89989
            }
			// Begin #79156
			if (_tellerVars.AdTlCls.ShowTitles.Value == "Y")
			{
				if(_tellerVars.AdTlCls.MapRimCust.Value == "Y")
					showRimNo = true;
				showName = true;
			}

			if (!showName || offline) 
			{
				this.lbLastAccts.Columns[2].Visible = false;
				this.lbLastAccts.Columns[3].Visible = false;
			}

			if (!showRimNo)
			{
				this.lbLastAccts.Columns[2].Visible = false;
			}
			
			if(Phoenix.FrameWork.Shared.Variables.GlobalVars.InstitutionType == "CU")	//#7987 - for CU display RIM#/Name regardless of other conditions.
			{
				this.lbLastAccts.Columns[2].Visible = true;
				this.lbLastAccts.Columns[3].Visible = true;
			}
			
			if (_tellerVars.AdTlControl.DisplayNicknames.Value == "N")
				this.lbLastAccts.Columns[4].Visible = false;				
			
			EnableDisableVisibleLogic( "FormCreate" );
			lbLastAccts.SelectRow(0,true);
			// End #79156
		}
		#endregion

		#region pbOk
		private void pbOk_Click( object sender, PActionEventArgs e )
		{
			AcctInfo selectedAcct = null;

			if (lbLastAccts.ContextRow >= 0)	//#79156
			{
				selectedAcct = _tellerVars.LastAccts[ lbLastAccts.ContextRow ] as AcctInfo ;	//#79156
                _acctInfo.RimNo = selectedAcct.RimNo;   //#180274
				_acctInfo.AcctNo = selectedAcct.AcctNo;
				_acctInfo.AcctType = selectedAcct.AcctType;
				_acctInfo.AcctNickname = selectedAcct.AcctNickname;
                _acctInfo.RealAcctNo = selectedAcct.RealAcctNo; //#76458
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
		#endregion

		#region pbCancel
		private void pbCancel_Click( object sender, PActionEventArgs e )
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		#region lbLastAccts
		private void lbLastAccts_SelectedIndexChanged( object sender, System.EventArgs e )
		{
			EnableDisableVisibleLogic( "ListClick" );
		}

		private void lbLastAccts_DoubleClick( object sender, System.EventArgs e )
		{
			EnableDisableVisibleLogic( "ListClick" );
			if (lbLastAccts.ContextRow >= 0)
			{
				pbOk_Click( sender,  null );
				return;
			}

		}
		#endregion


		private void EnableDisableVisibleLogic( string origin )
		{
			if ( origin == "FormCreate" )
			{
				pbOk.Enabled = false;
			}
			else if ( origin == "ListClick" )
			{
				pbOk.Enabled = (lbLastAccts.ContextRow >= 0);

			}
		}

	

	}
}
