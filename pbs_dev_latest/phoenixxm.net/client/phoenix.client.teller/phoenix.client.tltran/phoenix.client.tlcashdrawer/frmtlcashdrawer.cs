using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
//
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Shared.Variables;
using Phoenix.BusObj.Teller;
using Phoenix.Shared.Constants;
using GlacialComponents.Controls;

namespace Phoenix.Client.TlCashDrawer
{
	/// <summary>
	/// Summary description for frmTlCashDrawer.
	/// </summary>
	public class frmTlCashDrawer : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.BusObj.Admin.Global.AdGbCrncyDenom _busObjCrncyDenom = new Phoenix.BusObj.Admin.Global.AdGbCrncyDenom();
		private TellerVars _tellerVars = TellerVars.Instance;

		private Phoenix.Windows.Forms.PPanel pnl0;
		private Phoenix.Windows.Forms.PGrid gridCashDrawerCount;
		private Phoenix.Windows.Forms.PGridColumn colCrncyDenom;
		private Phoenix.Windows.Forms.PGridColumn colAmount;
		private Phoenix.Windows.Forms.PLabelStandard lblCountedDrawerAmt;
		private Phoenix.Windows.Forms.PdfCurrency dfTotalCash;
		private Phoenix.Windows.Forms.PAction pbTotalCash;
		private Phoenix.Windows.Forms.PdfStandard dfAction;

		public frmTlCashDrawer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.SupportedActions |= StandardAction.Save;

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
			this.pnl0 = new Phoenix.Windows.Forms.PPanel();
			this.gridCashDrawerCount = new Phoenix.Windows.Forms.PGrid();
			this.colCrncyDenom = new Phoenix.Windows.Forms.PGridColumn();
			this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
			this.dfTotalCash = new Phoenix.Windows.Forms.PdfCurrency();
			this.lblCountedDrawerAmt = new Phoenix.Windows.Forms.PLabelStandard();
			this.dfAction = new Phoenix.Windows.Forms.PdfStandard();
			this.pbTotalCash = new Phoenix.Windows.Forms.PAction();
			this.pnl0.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionManager
			// 
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
																						this.pbTotalCash});
			// 
			// pnl0
			// 
			this.pnl0.Controls.Add(this.gridCashDrawerCount);
			this.pnl0.Controls.Add(this.dfTotalCash);
			this.pnl0.Controls.Add(this.lblCountedDrawerAmt);
			this.pnl0.Location = new System.Drawing.Point(4, 0);
			this.pnl0.Name = "pnl0";
			this.pnl0.Size = new System.Drawing.Size(684, 444);
			this.pnl0.TabIndex = 1;
			this.pnl0.TabStop = true;
			// 
			// gridCashDrawerCount
			// 
			this.gridCashDrawerCount.Columns.AddRange(new PGridColumn[] {
																									this.colCrncyDenom,
																									this.colAmount});
			this.gridCashDrawerCount.ItemHeight = 0;
			this.gridCashDrawerCount.Location = new System.Drawing.Point(0, 4);
			this.gridCashDrawerCount.Name = "gridCashDrawerCount";
			this.gridCashDrawerCount.Size = new System.Drawing.Size(680, 416);
			this.gridCashDrawerCount.TabIndex = 0;
			// 
			// colCrncyDenom
			// 
			this.colCrncyDenom.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.colCrncyDenom.PhoenixUIControl.ObjectId = 2;
			this.colCrncyDenom.PhoenixUIControl.XmlTag = "CrncyDenom";
			this.colCrncyDenom.Title = "Denomination";
			this.colCrncyDenom.Width = 489;
			// 
			// colAmount
			// 
			//this.colAmount.ActivatedEmbeddedType = GlacialComponents.Controls.Common.ActivatedEmbeddedTypes.TextBox;
			this.colAmount.ReadOnly = false;
			this.colAmount.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colAmount.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colAmount.PhoenixUIControl.ObjectId = 7;
			this.colAmount.PhoenixUIControl.XmlTag = "Amount";
			this.colAmount.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
			this.colAmount.Title = "Amount";
			this.colAmount.Width = 175;
			// 
			// dfTotalCash
			// 
			this.dfTotalCash.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfTotalCash.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfTotalCash.Location = new System.Drawing.Point(516, 428);
			this.dfTotalCash.Name = "dfTotalCash";
			this.dfTotalCash.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfTotalCash.PhoenixUIControl.DisplayType = Phoenix.Windows.Forms.UIDisplayType.Display;
			this.dfTotalCash.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.dfTotalCash.PhoenixUIControl.MaxPrecision = 14;
			this.dfTotalCash.PhoenixUIControl.XmlTag = "CashDrawerAmt";
			this.dfTotalCash.ReadOnly = true;
			this.dfTotalCash.Size = new System.Drawing.Size(164, 13);
			this.dfTotalCash.TabIndex = 3;
			this.dfTotalCash.TabStop = false;
			this.dfTotalCash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// lblCountedDrawerAmt
			// 
			this.lblCountedDrawerAmt.Location = new System.Drawing.Point(436, 428);
			this.lblCountedDrawerAmt.Name = "lblCountedDrawerAmt";
			this.lblCountedDrawerAmt.Size = new System.Drawing.Size(64, 16);
			this.lblCountedDrawerAmt.TabIndex = 2;
			this.lblCountedDrawerAmt.Text = "Total Cash:";
			// 
			// dfAction
			// 
			this.dfAction.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfAction.Location = new System.Drawing.Point(256, 24);
			this.dfAction.Name = "dfAction";
			this.dfAction.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Decimal;
			this.dfAction.PhoenixUIControl.ObjectId = 10;
			this.dfAction.Size = new System.Drawing.Size(56, 20);
			this.dfAction.TabIndex = 0;
			this.dfAction.Visible = false;
			// 
			// pbTotalCash
			// 
			this.pbTotalCash.ShortText = "Total Cash";
			this.pbTotalCash.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbTotalCash_Click);
			// 
			// frmTlCashDrawer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.pnl0);
			this.Name = "frmTlCashDrawer";
			this.Text = "Teller Cash Drawer";
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlCashDrawer_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlCashDrawer_PInitCompleteEvent);
			this.pnl0.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Init param
		public override void InitParameters(params object[] paramList)
		{
			base.InitParameters (paramList);
		}
		#endregion	

		#region standard actions
		public override bool OnActionSave(bool isAddNext)
		{
			CalcCashTotal();
			_tellerVars.CountedAmount = Convert.ToDecimal(dfTotalCash.UnFormattedValue);
			_tellerVars.DrawerCounted = true;

			CallParent( "CashDrawerCount" );
			
			return base.OnActionSave (isAddNext);
		}
		
		#endregion

		#region events

		private ReturnType frmTlCashDrawer_PInitBeginEvent()
		{
			this.MainBusinesObject = _busObjCrncyDenom;
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.AvoidSave = true;
			return new ReturnType ();
		}

		private void frmTlCashDrawer_PInitCompleteEvent()
		{
			CallXMThruCDS("SelectCrncyDenom");
			PopulateCashDrawer();	
		}

		private void pbTotalCash_Click(object sender, PActionEventArgs e)
		{
			CalcCashTotal();
		}
		#endregion

		#region private methods

		private void CallXMThruCDS(string origin)
		{
			try
			{
				if ( origin == "SelectCrncyDenom" )
				{
					_busObjCrncyDenom.ActionType = XmActionType.Select;
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _busObjCrncyDenom );
				}
			}
			catch(PhoenixException excds)
			{
				//360978 - %1!
				PMessageBox.Show(this, 360978, MessageType.Error, MessageBoxButtons.OK, (excds.Message + "\r\n" + excds.InnerException));
			}

		}

		private void PopulateCashDrawer()
		{
//			GLItem listItem = new GLItem();
			string denomType = "";
			gridCashDrawerCount.Items.Clear();
			for (int i=1; i <= 10; i++)
			{
				if (!_busObjCrncyDenom.GetFieldByXmlTag("Bills" + Convert.ToString(i) + "Desc").IsNull)
				{
					denomType = _busObjCrncyDenom.GetFieldByXmlTag("Bills" + Convert.ToString(i) + "Desc").Value.ToString();
//					listItem.Text = denomType;
					gridCashDrawerCount.Items.Add(denomType);
				}

//				if (listItem != null)
//					this.gridCashDrawerCount.Items.Add(listItem);
			}
		}

		private void CalcCashTotal()
		{
			dfTotalCash.Clear();
			dfTotalCash.UnFormattedValue = Convert.ToDecimal(0);
			
			for ( int i = 0; i < gridCashDrawerCount.Items.Count; i++)
			{

				gridCashDrawerCount.SelectRow(i);

				foreach(Phoenix.Windows.Forms.PGridColumn column in gridCashDrawerCount.Columns)
				{
					if (column.XmlTag == "Amount" && column.Text.Trim() != "" && column.Text.Trim() != string.Empty && column.Text.Trim() != null)
					{
						dfTotalCash.UnFormattedValue = Convert.ToDecimal(dfTotalCash.UnFormattedValue) +  Convert.ToDecimal(column.UnFormattedValue);
					}
				}
			}
		}
		#endregion
	}
}
