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
// File Name: frmTlForwardTransactions.cs
// NameSpace: Phoenix.Client.TlForward
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//6/6/2006		1		phoenix	 - Created.
//06/29/2009    2      iezikeanyi  #2224 - Added code to ensure that cross ref accts are properly populated
// 06Nov2009	3		GDiNatale	#6615 - SetValue Framework change
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
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Core;
using System.Xml;
using Phoenix.Windows.Client;
using Phoenix.FrameWork.Shared.Variables;

namespace Phoenix.Client.TlForward
{
	/// <summary>
	/// Summary description for dlgTlForwardTransactions.
	/// </summary>
	public class frmTlForwardTransactions : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbUnforwardedTransactions;
		private Phoenix.Windows.Forms.PGridColumn colEffectiveDt;
		private Phoenix.Windows.Forms.PGridColumn colDrawerNo;
		private Phoenix.Windows.Forms.PGridColumn colReversal;
		private Phoenix.Windows.Forms.PGridColumn colSequenceNo;
		private Phoenix.Windows.Forms.PGridColumn colSubSequence;
		private Phoenix.Windows.Forms.PGridColumn colAccount;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private Phoenix.Windows.Forms.PGridColumn colCheckNo;
		private Phoenix.Windows.Forms.PGridColumn colAmt;
		private Phoenix.Windows.Forms.PAction pbSave;
		private Phoenix.Windows.Forms.PAction pbForcePost;
		private Phoenix.Windows.Forms.PAction pbEditRim;
		private Phoenix.Windows.Forms.PGrid grdForward;
		private Phoenix.Windows.Forms.PGridColumn colTlTranCode;
		private Phoenix.Windows.Forms.PGridColumn colTranCode;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;

		private bool rePopulated = false;
		private TlTransactionSet _tlTranSet = new TlTransactionSet();		
		private string prevTlTranCode = null;
		private DateTime prevClosingDt = DateTime.MinValue;
		private Phoenix.Windows.Forms.PGridColumn colTranStatus;
		private Phoenix.Windows.Forms.PGridColumn colPTID;
		private TlJournal _tlJournal;
		private Phoenix.Windows.Forms.PGridColumn colOfflineAcctType;
		private Phoenix.Windows.Forms.PGridColumn colOfflineAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colTranDescription;
		private Phoenix.Windows.Forms.PGridColumn colBatchDescription;
		private decimal pendingCloseOutPtid = -1;
        private PGridColumn colIncomingAcctNo;
        private PGridColumn colIncomingTfrAcctNo;
		private bool closedWkspace = false;

		public frmTlForwardTransactions()
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
            this.gbUnforwardedTransactions = new Phoenix.Windows.Forms.PGroupBoxStandard();
            this.grdForward = new Phoenix.Windows.Forms.PGrid();
            this.colEffectiveDt = new Phoenix.Windows.Forms.PGridColumn();
            this.colDrawerNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colReversal = new Phoenix.Windows.Forms.PGridColumn();
            this.colSequenceNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colSubSequence = new Phoenix.Windows.Forms.PGridColumn();
            this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
            this.colTlTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colCheckNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAmt = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranCode = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranStatus = new Phoenix.Windows.Forms.PGridColumn();
            this.colPTID = new Phoenix.Windows.Forms.PGridColumn();
            this.colOfflineAcctType = new Phoenix.Windows.Forms.PGridColumn();
            this.colOfflineAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colTranDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.colBatchDescription = new Phoenix.Windows.Forms.PGridColumn();
            this.pbSave = new Phoenix.Windows.Forms.PAction();
            this.pbForcePost = new Phoenix.Windows.Forms.PAction();
            this.pbEditRim = new Phoenix.Windows.Forms.PAction();
            this.colIncomingAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.colIncomingTfrAcctNo = new Phoenix.Windows.Forms.PGridColumn();
            this.gbUnforwardedTransactions.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionManager
            // 
            this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbSave,
            this.pbForcePost,
            this.pbEditRim});
            // 
            // gbUnforwardedTransactions
            // 
            this.gbUnforwardedTransactions.Controls.Add(this.grdForward);
            this.gbUnforwardedTransactions.Location = new System.Drawing.Point(4, 0);
            this.gbUnforwardedTransactions.Name = "gbUnforwardedTransactions";
            this.gbUnforwardedTransactions.PhoenixUIControl.ObjectId = 1;
            this.gbUnforwardedTransactions.Size = new System.Drawing.Size(684, 444);
            this.gbUnforwardedTransactions.TabIndex = 0;
            this.gbUnforwardedTransactions.TabStop = false;
            this.gbUnforwardedTransactions.Text = "Unforwarded Transactions";
            // 
            // grdForward
            // 
            this.grdForward.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colEffectiveDt,
            this.colDrawerNo,
            this.colReversal,
            this.colSequenceNo,
            this.colSubSequence,
            this.colAccount,
            this.colTlTranCode,
            this.colDescription,
            this.colCheckNo,
            this.colAmt,
            this.colTranCode,
            this.colAcctNo,
            this.colAcctType,
            this.colTranStatus,
            this.colPTID,
            this.colOfflineAcctType,
            this.colOfflineAcctNo,
            this.colTranDescription,
            this.colBatchDescription,
            this.colIncomingAcctNo,
            this.colIncomingTfrAcctNo});
            this.grdForward.LinesInHeader = 2;
            this.grdForward.Location = new System.Drawing.Point(4, 12);
            this.grdForward.Name = "grdForward";
            this.grdForward.Size = new System.Drawing.Size(676, 428);
            this.grdForward.TabIndex = 0;
            this.grdForward.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdForward_SelectedIndexChanged);
            this.grdForward.RowClicked += new Phoenix.Windows.Forms.GridClickedEventHandler(this.grdForward_SelectedIndexChanged);
            this.grdForward.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.grdForward_FetchRowDone);
            // 
            // colEffectiveDt
            // 
            this.colEffectiveDt.PhoenixUIControl.ObjectId = 41;
            this.colEffectiveDt.PhoenixUIControl.XmlTag = "EffectiveDt";
            this.colEffectiveDt.Title = "EffectiveDt";
            this.colEffectiveDt.Visible = false;
            this.colEffectiveDt.Width = 0;
            // 
            // colDrawerNo
            // 
            this.colDrawerNo.PhoenixUIControl.ObjectId = 54;
            this.colDrawerNo.PhoenixUIControl.XmlTag = "DrawerNo";
            this.colDrawerNo.Title = "DrawerNo";
            this.colDrawerNo.Visible = false;
            this.colDrawerNo.Width = 0;
            // 
            // colReversal
            // 
            this.colReversal.PhoenixUIControl.ObjectId = 24;
            this.colReversal.PhoenixUIControl.XmlTag = "Reversal";
            this.colReversal.Title = "Rev";
            this.colReversal.Width = 30;
            // 
            // colSequenceNo
            // 
            this.colSequenceNo.PhoenixUIControl.ObjectId = 42;
            this.colSequenceNo.PhoenixUIControl.XmlTag = "SequenceNo";
            this.colSequenceNo.Title = "Seq #";
            this.colSequenceNo.Width = 48;
            // 
            // colSubSequence
            // 
            this.colSubSequence.PhoenixUIControl.ObjectId = 59;
            this.colSubSequence.PhoenixUIControl.XmlTag = "SubSequence";
            this.colSubSequence.Title = "Sub Seq #";
            this.colSubSequence.Width = 52;
            // 
            // colAccount
            // 
            this.colAccount.PhoenixUIControl.ObjectId = 43;
            this.colAccount.PhoenixUIControl.XmlTag = "4";
            this.colAccount.Title = "Account";
            this.colAccount.Width = 142;
            // 
            // colTlTranCode
            // 
            this.colTlTranCode.PhoenixUIControl.ObjectId = 44;
            this.colTlTranCode.PhoenixUIControl.XmlTag = "TlTranCode";
            this.colTlTranCode.Title = "TranCode";
            this.colTlTranCode.Width = 33;
            // 
            // colDescription
            // 
            this.colDescription.PhoenixUIControl.ObjectId = 45;
            this.colDescription.PhoenixUIControl.XmlTag = "Description";
            this.colDescription.Title = "Description";
            this.colDescription.Width = 188;
            // 
            // colCheckNo
            // 
            this.colCheckNo.PhoenixUIControl.ObjectId = 46;
            this.colCheckNo.PhoenixUIControl.XmlTag = "CheckNo";
            this.colCheckNo.Title = "Check No";
            this.colCheckNo.Width = 49;
            // 
            // colAmt
            // 
            this.colAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
            this.colAmt.PhoenixUIControl.ObjectId = 47;
            this.colAmt.PhoenixUIControl.XmlTag = "NetAmt";
            this.colAmt.Title = "Amount";
            this.colAmt.Width = 112;
            // 
            // colTranCode
            // 
            this.colTranCode.PhoenixUIControl.XmlTag = "TranCode";
            this.colTranCode.Title = "Column";
            this.colTranCode.Visible = false;
            this.colTranCode.Width = 0;
            // 
            // colAcctNo
            // 
            this.colAcctNo.PhoenixUIControl.XmlTag = "AcctNo";
            this.colAcctNo.Title = "AcctNo";
            this.colAcctNo.Visible = false;
            this.colAcctNo.Width = 0;
            // 
            // colAcctType
            // 
            this.colAcctType.PhoenixUIControl.XmlTag = "AcctType";
            this.colAcctType.Title = "AcctType";
            this.colAcctType.Visible = false;
            this.colAcctType.Width = 0;
            // 
            // colTranStatus
            // 
            this.colTranStatus.PhoenixUIControl.XmlTag = "TranStatus";
            this.colTranStatus.Title = "TranStatus";
            this.colTranStatus.Visible = false;
            this.colTranStatus.Width = 0;
            // 
            // colPTID
            // 
            this.colPTID.PhoenixUIControl.XmlTag = "Ptid";
            this.colPTID.Title = "Column";
            this.colPTID.Visible = false;
            this.colPTID.Width = 0;
            // 
            // colOfflineAcctType
            // 
            this.colOfflineAcctType.PhoenixUIControl.XmlTag = "OfflineAcctType";
            this.colOfflineAcctType.Title = "Column";
            this.colOfflineAcctType.Visible = false;
            this.colOfflineAcctType.Width = 0;
            // 
            // colOfflineAcctNo
            // 
            this.colOfflineAcctNo.PhoenixUIControl.XmlTag = "OfflineAcctNo";
            this.colOfflineAcctNo.Title = "Column";
            this.colOfflineAcctNo.Visible = false;
            this.colOfflineAcctNo.Width = 0;
            // 
            // colTranDescription
            // 
            this.colTranDescription.PhoenixUIControl.XmlTag = "TranDescription";
            this.colTranDescription.Title = "Tran Description";
            this.colTranDescription.Visible = false;
            this.colTranDescription.Width = 0;
            // 
            // colBatchDescription
            // 
            this.colBatchDescription.PhoenixUIControl.XmlTag = "BatchDescription";
            this.colBatchDescription.Title = "Batch Description";
            this.colBatchDescription.Visible = false;
            this.colBatchDescription.Width = 0;
            // 
            // pbSave
            // 
            this.pbSave.LongText = "&Forward";
            this.pbSave.ObjectId = 49;
            this.pbSave.ShortText = "&Forward";
            this.pbSave.Tag = null;
            this.pbSave.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSave_Click);
            // 
            // pbForcePost
            // 
            this.pbForcePost.LongText = "&Force Post ";
            this.pbForcePost.ObjectId = 60;
            this.pbForcePost.ShortText = "&Force Post ";
            this.pbForcePost.Tag = null;
            this.pbForcePost.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbForcePost_Click);
            // 
            // pbEditRim
            // 
            this.pbEditRim.LongText = "Edit &RIM";
            this.pbEditRim.ObjectId = 61;
            this.pbEditRim.ShortText = "Edit &RIM";
            this.pbEditRim.Tag = null;
            this.pbEditRim.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbEditRim_Click);
            // 
            // colIncomingAcctNo
            // 
            this.colIncomingAcctNo.PhoenixUIControl.XmlTag = "IncomingAcctNo";
            this.colIncomingAcctNo.Title = "Column";
            this.colIncomingAcctNo.Visible = false;
            // 
            // colIncomingTfrAcctNo
            // 
            this.colIncomingTfrAcctNo.PhoenixUIControl.XmlTag = "IncomingTfrAcctNo";
            this.colIncomingTfrAcctNo.Title = "Column";
            this.colIncomingTfrAcctNo.Visible = false;
            // 
            // frmTlForwardTransactions
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(690, 448);
            this.Controls.Add(this.gbUnforwardedTransactions);
            this.Name = "frmTlForwardTransactions";
            this.ScreenId = 10885;
            this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlForwardTransactions_PInitCompleteEvent);
            this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlForwardTransactions_PInitBeginEvent);
            this.gbUnforwardedTransactions.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		private void EnableDisableVisibleLogic( string origin )
		{
			if ( origin == "TableClick" || origin == "FormCreate" )
			{
				pbEditRim.Enabled =  (rePopulated || Convert.ToDecimal( colPTID.UnFormattedValue ) < pendingCloseOutPtid )&& ( colTranCode != null && 
					_tlTranSet.IsRimTran( Convert.ToInt16( colTranCode.UnFormattedValue )));
			}
			if ( origin == "FormCreate" )
			{
				if ( TellerVars.Instance.IsNoDrawerOptionSelected )
				{
					colDrawerNo.Visible = true;
					colDrawerNo.Width = 44;
				}
				else
					colDrawerNo.Visible = false;
				colEffectiveDt.Visible = false;
				pbForcePost.Enabled = ( rePopulated || pendingCloseOutPtid > 0 ) && grdForward.Count > 0 ;
			}
		}

		private void pbSave_Click(object sender, PActionEventArgs e)
		{
			ForwardTransactions( false );
		}

		private void pbForcePost_Click(object sender, PActionEventArgs e)
		{
			ForwardTransactions( true );
		}

		private void pbEditRim_Click(object sender, PActionEventArgs e)
		{
			_tlJournal.BranchNo.Value = TellerVars.Instance.BranchNo;
			_tlJournal.DrawerNo.Value = Convert.ToInt16( colDrawerNo.UnFormattedValue );
			_tlJournal.EffectiveDt.Value = Convert.ToDateTime( colEffectiveDt.UnFormattedValue );
			_tlJournal.SequenceNo.Value = Convert.ToInt16( colSequenceNo.UnFormattedValue );
			_tlJournal.SubSequence.Value = Convert.ToInt16( colSubSequence.UnFormattedValue );
			_tlJournal.AcctType.Value = Convert.ToString( colAcctType.UnFormattedValue );
			_tlJournal.AcctNo.Value = Convert.ToString( colAcctNo.UnFormattedValue );
			_tlJournal.TlTranCode.Value = Convert.ToString( colTlTranCode.UnFormattedValue );
			_tlJournal.TranCode.Value = Convert.ToInt16( colTranCode.UnFormattedValue );
			_tlJournal.NetAmt.Value = Convert.ToDecimal( colAmt.UnFormattedValue );

            

			if ( Convert.ToString( colOfflineAcctNo.UnFormattedValue ) != null && 
				Convert.ToString( colOfflineAcctNo.UnFormattedValue ).Trim() != String.Empty )
			{
				_tlJournal.OfflineAcctType.Value = Convert.ToString( colOfflineAcctType.UnFormattedValue );
				_tlJournal.OfflineAcctNo.Value = Convert.ToString( colOfflineAcctNo.UnFormattedValue );
			}
			else
			{
				_tlJournal.OfflineAcctNo.SetValue(null, Phoenix.FrameWork.BusFrame.EventBehavior.None);		// #6615 - _tlJournal.OfflineAcctNo.SetValueToNull();
				_tlJournal.OfflineAcctType.SetValue(null, Phoenix.FrameWork.BusFrame.EventBehavior.None);	// #6615 - _tlJournal.OfflineAcctType.SetValueToNull();
			}
			CallOtherForms( "EditRim" );
		}

		private ReturnType frmTlForwardTransactions_PInitBeginEvent()
		{
			return new ReturnType ();
		}

		private void frmTlForwardTransactions_PInitCompleteEvent()
		{
			XmlNode listNode = null;
			if ( _tlJournal == null )
				_tlJournal = new TlJournal();

			MainBusinesObject = _tlJournal;
			_tlJournal.BranchNo.Value = TellerVars.Instance.BranchNo;
			if ( !TellerVars.Instance.IsNoDrawerOptionSelected )
			{
				_tlJournal.DrawerNo.Value = TellerVars.Instance.DrawerNo;
			}
			_tlJournal.OutputType.Value = 6;
			if ( TellerVars.Instance.OfflineCDS != null )
				listNode = TellerVars.Instance.OfflineCDS.GetListView( _tlJournal );
			//grdForward.ListViewObject = _tlJournal;
			//grdForward.ObjectToScreen();
			grdForward.PopulateTable( listNode, true );
			EnableDisableVisibleLogic( "FormCreate" );
			prevTlTranCode = null;
			prevClosingDt = DateTime.MinValue;
			foreach( PGridColumn col in grdForward.Columns )
				col.SortOrder = SortOrder.None;
			if ( this.Workspace != null )
			{
				//(this.Workspace as Form).Closing +=new CancelEventHandler(frmTlForwardTransactions_Closing);
				(this.Workspace as Form).Closed +=new EventHandler(wksSpace_Closed);
			}

		}

		private void ForwardTransactions( bool forcePost )
		{
			short curDrawerNo =-1;
			short curSeqNo =-1;
			DateTime curEffectiveDt = DateTime.MinValue;
			bool invalidRim = false;
			bool cTRTriggered = false;
			short tranStatus = -1;
			int ctrCount = 0;
			int invalidRimCount = 0;
			int failedTranCount = 0;
			int contextRow = -1;
	
			try
			{
				if ( contextRow < 0 )
					contextRow = 0;
				pendingCloseOutPtid = -1;

				while ( contextRow < grdForward.Count )
				{
					grdForward.ContextRow = contextRow;

					if ( colDrawerNo.UnFormattedValue == null || colSequenceNo.UnFormattedValue == null || 
						colEffectiveDt.UnFormattedValue == null )
					{
						break;
					}
					if ( curDrawerNo != Convert.ToInt16( colDrawerNo.UnFormattedValue ) || 
						curSeqNo != Convert.ToInt16( colSequenceNo.UnFormattedValue ) || 
						curEffectiveDt != Convert.ToDateTime( colEffectiveDt.UnFormattedValue )) 
					{
						curDrawerNo = Convert.ToInt16( colDrawerNo.UnFormattedValue );
						curSeqNo = Convert.ToInt16( colSequenceNo.UnFormattedValue );
						curEffectiveDt = Convert.ToDateTime( colEffectiveDt.UnFormattedValue );
						if ( colTranStatus.UnFormattedValue == null )
							tranStatus = 1;
						else
							tranStatus = Convert.ToInt16( colTranStatus.UnFormattedValue );

						if ( tranStatus != 1 )
						{
							if ( tranStatus != 0 )
								failedTranCount++;
							continue;
						}

						if ( _tlJournal.CheckForUnfwdTransBeforeForward( Convert.ToString( colTlTranCode.UnFormattedValue )))
						{							
							if ( failedTranCount > 0 )
							{
								//One or more miscellaneous transactions could not be forwarded due to transactions with invalid RIM Number or RIM status.  Please review and correct the failed RIM transactions as marked in red and then continue forwarding.
								PMessageBox.Show( 360694, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
								pendingCloseOutPtid = Convert.ToDecimal( colPTID.UnFormattedValue );
								frmTlForwardTransactions_PInitCompleteEvent();
								return;
							}
							forcePost = false;
						}
                       

						//360969 - Forwarding Transactions: Sequence No. %1!
						string msg = CoreService.Translation.GetTokenizeMessageX(360969, curSeqNo.ToString());
						//dlgInformation.Instance.ShowInfo( "Forwarding Transactions: Sequence No. " +  curSeqNo );
						dlgInformation.Instance.ShowInfo( msg );
						_tlTranSet = new TlTransactionSet();
						_tlTranSet.BranchNo.Value = TellerVars.Instance.BranchNo;
						_tlTranSet.DrawerNo.Value = curDrawerNo;
						_tlTranSet.EffectiveDt.Value = curEffectiveDt;
						_tlTranSet.SequenceNo.Value = curSeqNo;

                        //Begin #2224
                        if (colIncomingAcctNo.UnFormattedValue != null)
                            _tlTranSet.IncomingAcctNo.Value = Convert.ToString(colIncomingAcctNo.UnFormattedValue);
                        if (colIncomingTfrAcctNo.UnFormattedValue != null)
                            _tlTranSet.IncomingTfrAcctNo.Value = Convert.ToString(colIncomingTfrAcctNo.UnFormattedValue);
                        //End #2224

						if ( _tlTranSet.ForwardTransaction( forcePost, Convert.ToString( colTlTranCode.UnFormattedValue ), 
							ref invalidRim, ref prevTlTranCode, ref prevClosingDt, ref cTRTriggered ))
							tranStatus = 0;
						else
						{
							if ( invalidRim  )
							{
								if ( !forcePost )
								{
									tranStatus = 3;
									invalidRimCount++;
								}
								else
								{
									tranStatus = 2;
								}
							}
							else
							{
								tranStatus = 2;
							}
						}

						if ( tranStatus != 0 )
							failedTranCount++;
						colTranStatus.UnFormattedValue = tranStatus;
						if ( failedTranCount != invalidRimCount )
						{
							//The system could not force post the transaction with sequence number %1! 
							PMessageBox.Show( 360693, MessageType.Warning, MessageBoxButtons.OK, new string[1] {Convert.ToString( curSeqNo )} );
							return;
						}
						if ( cTRTriggered )
							ctrCount++;
					}
					contextRow++;
				}

				if ( ctrCount > 0 )
					PMessageBox.Show( 318317, MessageType.Warning, MessageBoxButtons.OK, String.Empty );

				if ( invalidRimCount > 0 )
					PMessageBox.Show( 360688, MessageType.Warning, MessageBoxButtons.OK, String.Empty );


			}
			catch( PhoenixException pe )
			{
				colTranStatus.UnFormattedValue = 2;
				PMessageBox.Show( pe, MessageBoxButtons.OK );
				return;
			}
			finally
			{				
				dlgInformation.Instance.HideInfo();
				if ( contextRow == grdForward.Count )
				{
					rePopulated = true;					
				}
				frmTlForwardTransactions_PInitCompleteEvent();

				if ( grdForward.Count == 0 )
					this.Close();
			}	
		}

		private void grdForward_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			EnableDisableVisibleLogic( "TableClick" );
		}

		private void grdForward_FetchRowDone(object sender, GridRowArgs e)
		{
			string tlTrandesc;
			short realTranCode; 
			colAccount.Text = _tlJournal.GetAccountDesc(colAcctType.Text, colAcctNo.Text, 0 );
			colReversal.Text = Convert.ToInt32( colReversal.UnFormattedValue ) == 2 ? GlobalVars.Instance.ML.Y : null;
			if ( Convert.ToDecimal( colPTID.UnFormattedValue ) < pendingCloseOutPtid && colTranCode != null && 
				_tlTranSet.IsRimTran( Convert.ToInt16( colTranCode.UnFormattedValue )) && 
				Convert.ToString( colOfflineAcctNo.UnFormattedValue ) != null && 
				Convert.ToString( colOfflineAcctNo.UnFormattedValue ).Trim() != String.Empty )
				e.CurrentRow.ForeColor = System.Drawing.Color.Red;
			if (_tlTranSet.TellerHelper.IsMiscTran(colTlTranCode.Text))
			{
				_tlJournal.GetTellerTranCodeDesc(colTlTranCode.Text, out tlTrandesc, out realTranCode);
				colDescription.Text = tlTrandesc;
				if ( colBatchDescription.Text != null && colBatchDescription.Text.Trim() != string.Empty )
				{
					colDescription.Text = colBatchDescription.Text + " - " + colDescription.Text;
				}
			}
			else if (_tlTranSet.TellerHelper.IsGenericTran(colTlTranCode.Text))
			{			
				colDescription.Text = colTranDescription.Text;
			}
		}

		
		private void CallOtherForms( string origin )
		{
			PfwStandard tempWin = null;

			try
			{
				if ( origin == "EditRim" )
				{
					tempWin = Helper.CreateWindow( "phoenix.client.tlforward","Phoenix.Client.TlForward","frmTlEditFailedRIMTransaction");
					tempWin.InitParameters( _tlJournal );
					tempWin.Workspace = this.Workspace;
					tempWin.Show();
					tempWin.Closed +=new EventHandler(tempWin_Closed);
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
				return;
			}

		}

		private void tempWin_Closed(object sender, EventArgs e)
		{
			if (( sender as Form ).Name == "frmTlEditFailedRIMTransaction" )
			{
				if ( !_tlJournal.OfflineAcctType.IsNull )
					frmTlForwardTransactions_PInitCompleteEvent();
			}
		}

		private void wksSpace_Closed(object sender, EventArgs e)
		{
			if ( !closedWkspace )
			{
				closedWkspace = true;
				this.Close();
			}
		}

	}
}
