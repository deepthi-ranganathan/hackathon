#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions-Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlPositionHistory.cs
// NameSpace: Phoenix.Client.TlPosition
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//##/##/####	1		mselvaga	Created.
//03/20/2007	2		mselvaga	#72101 - Disable display action when no postion history found.
//09/07/2011	3		sdhamija	#15431 - wrapped grid in a group box. anchored grid top, left. UI only change.
//-------------------------------------------------------------------------------
#endregion
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Phoenix.Windows.Forms;

using Phoenix.Shared.Constants;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.Core.Utilities;
using Phoenix.BusObj.Teller;
using Phoenix.Client.TlPosition;

namespace Phoenix.Client.TlPosition
{
	/// <summary>
	/// Summary description for frmTlPositionHistory.
	/// </summary>
	public class frmTlPositionHistory : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		Phoenix.BusObj.Teller.TlPosition _busObjPosition = new Phoenix.BusObj.Teller.TlPosition();
		private PSmallInt branchNo	= new PSmallInt();
		private PSmallInt drawerNo	= new PSmallInt();
		private Phoenix.Windows.Forms.PGrid gridPositionHistory;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colCrncyID;
		private Phoenix.Windows.Forms.PGridColumn colPTID;
		private Phoenix.Windows.Forms.PGridColumn colClosedDt;
		private Phoenix.Windows.Forms.PGridColumn colISOCode;
		private Phoenix.Windows.Forms.PGridColumn colClosedTime;
		private Phoenix.Windows.Forms.PGridColumn colDescription;
		private PGroupBoxStandard pGroupBoxStandard1;
		private Phoenix.Windows.Forms.PAction pbDisplay;

		public frmTlPositionHistory()
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
			this.gridPositionHistory = new Phoenix.Windows.Forms.PGrid();
			this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
			this.colCrncyID = new Phoenix.Windows.Forms.PGridColumn();
			this.colPTID = new Phoenix.Windows.Forms.PGridColumn();
			this.colClosedDt = new Phoenix.Windows.Forms.PGridColumn();
			this.colISOCode = new Phoenix.Windows.Forms.PGridColumn();
			this.colClosedTime = new Phoenix.Windows.Forms.PGridColumn();
			this.colDescription = new Phoenix.Windows.Forms.PGridColumn();
			this.pbDisplay = new Phoenix.Windows.Forms.PAction();
			this.pGroupBoxStandard1 = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.pGroupBoxStandard1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ActionManager
			// 
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbDisplay});
			// 
			// gridPositionHistory
			// 
			this.gridPositionHistory.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colCreateDt,
            this.colCrncyID,
            this.colPTID,
            this.colClosedDt,
            this.colISOCode,
            this.colClosedTime,
            this.colDescription});
			this.gridPositionHistory.Location = new System.Drawing.Point(5, 10);
			this.gridPositionHistory.Name = "gridPositionHistory";
			this.gridPositionHistory.Size = new System.Drawing.Size(671, 426);
			this.gridPositionHistory.TabIndex = 0;
			this.gridPositionHistory.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridPositionHistory_FetchRowDone);
			this.gridPositionHistory.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridPositionHistory_BeforePopulate);
			// 
			// colCreateDt
			// 
			this.colCreateDt.PhoenixUIControl.ObjectId = 9;
			this.colCreateDt.Title = "Column";
			this.colCreateDt.Visible = false;
			this.colCreateDt.Width = 0;
			// 
			// colCrncyID
			// 
			this.colCrncyID.PhoenixUIControl.ObjectId = 9;
			this.colCrncyID.PhoenixUIControl.XmlTag = "x";
			this.colCrncyID.Title = "Column";
			this.colCrncyID.Visible = false;
			this.colCrncyID.Width = 0;
			// 
			// colPTID
			// 
			this.colPTID.PhoenixUIControl.ObjectId = 9;
			this.colPTID.PhoenixUIControl.XmlTag = "Ptid";
			this.colPTID.Title = "Column";
			this.colPTID.Visible = false;
			this.colPTID.Width = 0;
			// 
			// colClosedDt
			// 
			this.colClosedDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colClosedDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
			this.colClosedDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colClosedDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Date;
			this.colClosedDt.PhoenixUIControl.ObjectId = 2;
			this.colClosedDt.PhoenixUIControl.XmlTag = "ClosedDt";
			this.colClosedDt.Title = "Posting Date";
			this.colClosedDt.Width = 117;
			// 
			// colISOCode
			// 
			this.colISOCode.PhoenixUIControl.IsVisible = Phoenix.Windows.Forms.VisibilityState.Hide;
			this.colISOCode.PhoenixUIControl.ObjectId = 10;
			this.colISOCode.PhoenixUIControl.XmlTag = "IsoCode";
			this.colISOCode.Title = "Currency";
			this.colISOCode.Visible = false;
			this.colISOCode.Width = 0;
			// 
			// colClosedTime
			// 
			this.colClosedTime.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colClosedTime.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
			this.colClosedTime.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colClosedTime.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTime;
			this.colClosedTime.PhoenixUIControl.ObjectId = 8;
			this.colClosedTime.PhoenixUIControl.XmlTag = "CreateDt";
			this.colClosedTime.Title = "Closed Date/Time";
			this.colClosedTime.Width = 187;
			// 
			// colDescription
			// 
			this.colDescription.PhoenixUIControl.ObjectId = 3;
			this.colDescription.PhoenixUIControl.XmlTag = "Description";
			this.colDescription.Title = "Description";
			this.colDescription.Width = 356;
			// 
			// pbDisplay
			// 
			this.pbDisplay.LongText = "&Display";
			this.pbDisplay.NextScreenId = 10767;
			this.pbDisplay.ObjectId = 6;
			this.pbDisplay.ShortText = "&Display";
			this.pbDisplay.Tag = null;
			this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
			// 
			// pGroupBoxStandard1
			// 
			this.pGroupBoxStandard1.Controls.Add(this.gridPositionHistory);
			this.pGroupBoxStandard1.Location = new System.Drawing.Point(4, 2);
			this.pGroupBoxStandard1.Name = "pGroupBoxStandard1";
			this.pGroupBoxStandard1.Size = new System.Drawing.Size(682, 442);
			this.pGroupBoxStandard1.TabIndex = 1;
			this.pGroupBoxStandard1.TabStop = false;
			// 
			// frmTlPositionHistory
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.pGroupBoxStandard1);
			this.Name = "frmTlPositionHistory";
			this.ScreenId = 10766;
			this.Text = "Teller Summary Position History";
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmTlPositionHistory_PInitCompleteEvent);
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmTlPositionHistory_PInitBeginEvent);
			this.pGroupBoxStandard1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Init param
		public override void InitParameters(params object[] paramList)
		{
			if (paramList.Length == 2)
			{
				branchNo.Value = Convert.ToInt16(paramList[0]);
				drawerNo.Value = Convert.ToInt16(paramList[1]);
				this._busObjPosition.BranchNo.Value = branchNo.Value;
				this._busObjPosition.DrawerNo.Value = drawerNo.Value;
				this._busObjPosition.OutputTypeId.Value = 1;
			}

			#region say no to default framework select
			this.AutoFetch = false;
			#endregion

			this.ScreenId = Phoenix.Shared.Constants.ScreenId.PositionHistory;
			base.InitParameters (paramList);

		}
		#endregion

		#region events
		private ReturnType frmTlPositionHistory_PInitBeginEvent()
		{
			this.AppToolBarId = AppToolBarType.NoEdit;
			this.MainBusinesObject = _busObjPosition;
			//
			#region set screen/next screen
			pbDisplay.NextScreenId = Phoenix.Shared.Constants.ScreenId.PositionHistoryDisplay;
			#endregion
			//
			return ReturnType.Success;
		}

		private void frmTlPositionHistory_PInitCompleteEvent()
		{
			this.gridPositionHistory.DoubleClickAction = this.pbDisplay;
			//
			EnableDisableVisibleLogic("FormComplete");
		}

		private void gridPositionHistory_BeforePopulate(object sender, GridPopulateArgs e)
		{
			this.gridPositionHistory.ListViewObject = _busObjPosition;
		}

		private void gridPositionHistory_FetchRowDone(object sender, GridRowArgs e)
		{
			colISOCode.UnFormattedValue = "USD";
		}

		private void pbDisplay_Click(object sender, PActionEventArgs e)
		{
			frmTlPosition temObjHistPos = new frmTlPosition();
			temObjHistPos.InitParameters(2, branchNo.Value, drawerNo.Value, Convert.ToDateTime(colClosedDt.UnFormattedValue), Convert.ToDecimal(colPTID.UnFormattedValue), null, null, false);
			temObjHistPos.Workspace = this.Workspace;
			temObjHistPos.Show();
		}
		#endregion

		#region private methods
		private void EnableDisableVisibleLogic( string callerInfo )
		{
			if (callerInfo == "FormComplete")
			{
				if (gridPositionHistory.Count <= 0)
					this.pbDisplay.Enabled = false;
				else
					this.pbDisplay.Enabled = true;
			}
		}
		#endregion

	}
}
