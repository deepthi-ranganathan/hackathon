using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Shared.Variables;

namespace Phoenix.Client.TlCalculator
{
	/// <summary>
	/// Summary description for dfwTlJournalTapeDisplay.
	/// </summary>
	public class frmTlJournalTapeDisplay : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private TlJournal _busObjTlJournalTape = new TlJournal();
		private TlHelper _tellerHelper = new TlHelper();
		private PSmallInt branchNo	= new PSmallInt();
		private PSmallInt drawerNo	= new PSmallInt();
		private PDateTime effectiveDt	= new PDateTime();
		private PDecimal ptid		= new PDecimal();
		private string calcData	= "";
		private string tempCalcData	= "";
		private string tempDescription = "";
		private TlJournalCalc _tlJournalCalc;
		private Phoenix.Windows.Forms.PPanel pnl3;
		private Phoenix.Windows.Forms.PLabelStandard lblSequence;
		private Phoenix.Windows.Forms.PDfDisplay dfSequenceNo;
		private Phoenix.Windows.Forms.PLabelStandard lblTransaction;
		private Phoenix.Windows.Forms.PDfDisplay dfTransaction;
		private Phoenix.Windows.Forms.PLabelStandard lblTranStatus;
		private Phoenix.Windows.Forms.PDfDisplay dfTranStatus;
		private Phoenix.Windows.Forms.PLabelStandard lblTeller;
		private Phoenix.Windows.Forms.PDfDisplay dfTellerName;
		private Phoenix.Windows.Forms.PPanel pnl12;
		private Phoenix.Windows.Forms.PdfStandard mulTapeDisplay;

		public frmTlJournalTapeDisplay()
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
			this.pnl3 = new Phoenix.Windows.Forms.PPanel();
			this.dfTellerName = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblTeller = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfTranStatus = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblTranStatus = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfTransaction = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblTransaction = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfSequenceNo = new Phoenix.Windows.Forms.PDfDisplay();
			this.lblSequence = new Phoenix.Windows.Forms.PLabelStandard();
			this.pnl12 = new Phoenix.Windows.Forms.PPanel();
			this.mulTapeDisplay = new Phoenix.Windows.Forms.PdfStandard();
			this.pnl3.SuspendLayout();
			this.pnl12.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnl3
			// 
			this.pnl3.Controls.Add(this.dfTellerName);
			this.pnl3.Controls.Add(this.lblTeller);
			this.pnl3.Controls.Add(this.dfTranStatus);
			this.pnl3.Controls.Add(this.lblTranStatus);
			this.pnl3.Controls.Add(this.dfTransaction);
			this.pnl3.Controls.Add(this.lblTransaction);
			this.pnl3.Controls.Add(this.dfSequenceNo);
			this.pnl3.Controls.Add(this.lblSequence);
			this.pnl3.Location = new System.Drawing.Point(4, 0);
			this.pnl3.Name = "pnl3";
			this.pnl3.RaisedBorder = true;
			this.pnl3.Size = new System.Drawing.Size(684, 92);
			this.pnl3.TabIndex = 1;
			this.pnl3.TabStop = true;
			// 
			// dfTellerName
			// 
			this.dfTellerName.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTellerName.Location = new System.Drawing.Point(100, 68);
			this.dfTellerName.Multiline = true;
			this.dfTellerName.Name = "dfTellerName";
			this.dfTellerName.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTellerName.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfTellerName.PhoenixUIControl.ObjectId = 6;
			this.dfTellerName.PhoenixUIControl.XmlTag = "EmplId";
			this.dfTellerName.Size = new System.Drawing.Size(268, 16);
			this.dfTellerName.TabIndex = 0;
			// 
			// lblTeller
			// 
			this.lblTeller.Location = new System.Drawing.Point(4, 68);
			this.lblTeller.Name = "lblTeller";
			this.lblTeller.PhoenixUIControl.ObjectId = 6;
			this.lblTeller.Size = new System.Drawing.Size(84, 16);
			this.lblTeller.TabIndex = 1;
			this.lblTeller.Text = "Teller:";
			// 
			// dfTranStatus
			// 
			this.dfTranStatus.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTranStatus.Location = new System.Drawing.Point(100, 48);
			this.dfTranStatus.Multiline = true;
			this.dfTranStatus.Name = "dfTranStatus";
			this.dfTranStatus.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTranStatus.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfTranStatus.PhoenixUIControl.ObjectId = 5;
			this.dfTranStatus.PhoenixUIControl.XmlTag = "TranStatusDesc";
			this.dfTranStatus.Size = new System.Drawing.Size(268, 16);
			this.dfTranStatus.TabIndex = 2;
			// 
			// lblTranStatus
			// 
			this.lblTranStatus.Location = new System.Drawing.Point(4, 48);
			this.lblTranStatus.Name = "lblTranStatus";
			this.lblTranStatus.PhoenixUIControl.ObjectId = 5;
			this.lblTranStatus.Size = new System.Drawing.Size(84, 16);
			this.lblTranStatus.TabIndex = 3;
			this.lblTranStatus.Text = "Tran Status:";
			// 
			// dfTransaction
			// 
			this.dfTransaction.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTransaction.Location = new System.Drawing.Point(100, 28);
			this.dfTransaction.Multiline = true;
			this.dfTransaction.Name = "dfTransaction";
			this.dfTransaction.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.dfTransaction.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfTransaction.PhoenixUIControl.ObjectId = 4;
			this.dfTransaction.PhoenixUIControl.XmlTag = "TranDesc";
			this.dfTransaction.Size = new System.Drawing.Size(268, 16);
			this.dfTransaction.TabIndex = 4;
			// 
			// lblTransaction
			// 
			this.lblTransaction.Location = new System.Drawing.Point(4, 28);
			this.lblTransaction.Name = "lblTransaction";
			this.lblTransaction.PhoenixUIControl.ObjectId = 4;
			this.lblTransaction.Size = new System.Drawing.Size(84, 16);
			this.lblTransaction.TabIndex = 5;
			this.lblTransaction.Text = "Transaction:";
			// 
			// dfSequenceNo
			// 
			this.dfSequenceNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.dfSequenceNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dfSequenceNo.Location = new System.Drawing.Point(288, 8);
			this.dfSequenceNo.Multiline = true;
			this.dfSequenceNo.Name = "dfSequenceNo";
			this.dfSequenceNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.dfSequenceNo.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfSequenceNo.PhoenixUIControl.ObjectId = 3;
			this.dfSequenceNo.PhoenixUIControl.XmlTag = "SequenceNo";
			this.dfSequenceNo.Size = new System.Drawing.Size(80, 16);
			this.dfSequenceNo.TabIndex = 6;
			// 
			// lblSequence
			// 
			this.lblSequence.Location = new System.Drawing.Point(4, 8);
			this.lblSequence.Name = "lblSequence";
			this.lblSequence.PhoenixUIControl.ObjectId = 3;
			this.lblSequence.Size = new System.Drawing.Size(84, 16);
			this.lblSequence.TabIndex = 7;
			this.lblSequence.Text = "Sequence #:";
			// 
			// pnl12
			// 
			this.pnl12.Controls.Add(this.mulTapeDisplay);
			this.pnl12.Location = new System.Drawing.Point(4, 92);
			this.pnl12.Name = "pnl12";
			this.pnl12.RaisedBorder = true;
			this.pnl12.Size = new System.Drawing.Size(684, 352);
			this.pnl12.TabIndex = 0;
			this.pnl12.TabStop = true;
			// 
			// mulTapeDisplay
			// 
			this.mulTapeDisplay.Location = new System.Drawing.Point(4, 8);
			this.mulTapeDisplay.Multiline = true;
			this.mulTapeDisplay.Name = "mulTapeDisplay";
			this.mulTapeDisplay.PhoenixUIControl.ObjectId = 7;
			this.mulTapeDisplay.PhoenixUIControl.XmlTag = "CalcData1";
			this.mulTapeDisplay.ReadOnly = true;
			this.mulTapeDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mulTapeDisplay.Size = new System.Drawing.Size(364, 340);
			this.mulTapeDisplay.TabIndex = 0;
			this.mulTapeDisplay.TabStop = false;
			this.mulTapeDisplay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// frmTlJournalTapeDisplay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.pnl12);
			this.Controls.Add(this.pnl3);
			this.Name = "frmTlJournalTapeDisplay";
			this.ScreenId = 10985;
			this.Text = "Calculator Tape Display";
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlJournalTapeDisplay_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlJournalTapeDisplay_PInitCompleteEvent);
			this.pnl3.ResumeLayout(false);
			this.pnl12.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Init Param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 4)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				effectiveDt.Value = Convert.ToDateTime(paramList[2]);
				ptid.Value = Convert.ToDecimal(paramList[3]);

				_busObjTlJournalTape.BranchNo.Value = branchNo.Value;
				_busObjTlJournalTape.DrawerNo.Value = drawerNo.Value;
				_busObjTlJournalTape.EffectiveDt.Value = effectiveDt.Value;
				_busObjTlJournalTape.Ptid.Value = ptid.Value;
			}

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.JournalTapeDisplay;			
			base.InitParameters (paramList);
		}

		#endregion

		#region events
		private ReturnType frmTlJournalTapeDisplay_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.Display;
			this.MainBusinesObject = _busObjTlJournalTape;
			this.IsNew = false;

			return ReturnType.Success;
		}

		private void frmTlJournalTapeDisplay_PInitCompleteEvent()
		{
			this.dfSequenceNo.ObjectToScreen();
			// Fk Value parse
			if (_tellerHelper.GetFKValueDesc(_busObjTlJournalTape.TranStatus.Value, _busObjTlJournalTape.TranStatus.FKValue) == "")
				this.dfTranStatus.Text = "";
			else
				this.dfTranStatus.Text = _tellerHelper.GetFKValueDesc(_busObjTlJournalTape.TranStatus.Value, _busObjTlJournalTape.TranStatus.FKValue);

			if (_tellerHelper.GetFKValueDesc(_busObjTlJournalTape.EmplId.Value, _busObjTlJournalTape.EmplId.FKValue) == "")
				this.dfTellerName.Text = "";
			else
				this.dfTellerName.Text = _tellerHelper.GetFKValueDesc(_busObjTlJournalTape.EmplId.Value, _busObjTlJournalTape.EmplId.FKValue);

			this._busObjTlJournalTape.GetTellerTranCodeDesc(this._busObjTlJournalTape.TlTranCode.Value, out tempDescription);
			if (tempDescription != "")
				this.dfTransaction.Text = this._busObjTlJournalTape.TlTranCode.Value + " - " + tempDescription;
			else
				this.dfTransaction.Text = this._busObjTlJournalTape.TlTranCode.Value;

			tempCalcData = _busObjTlJournalTape.CalcData1.Value + _busObjTlJournalTape.CalcData2.Value;
			GetAlternateTapeData( ref tempCalcData );
			if (tempCalcData.Trim() != string.Empty && tempCalcData.Trim() != "")
			{
				calcData = tempCalcData.Replace("\n", "\r\n");
				this.mulTapeDisplay.Text = calcData;
				this.mulTapeDisplay.TextAlign = HorizontalAlignment.Right;
				this.mulTapeDisplay.ReadOnly = true;
			}

		}
		#endregion


		#region private methods
		private void TextAlign()
		{
			this.dfSequenceNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.dfTransaction.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.dfTranStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
//			this.dfTellerName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		}
		private void GetAlternateTapeData( ref string calcData )
		{
			_tlJournalCalc = new TlJournalCalc();
			_tlJournalCalc.ActionType = XmActionType.Select;
			_tlJournalCalc.JournalPtid.Value = _busObjTlJournalTape.Ptid.Value;
			CallXMThruCDS( "GetAlternateTapeData" );
			if ( !_tlJournalCalc.CalcData.IsNull )
				calcData = _tlJournalCalc.CalcData.Value;
		}

		private void CallXMThruCDS(string origin )
		{
			try
			{
				if ( origin == "GetAlternateTapeData" )
				{
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _tlJournalCalc );
				}
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
			}
		}

		#endregion
	}
}
