#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2006 Harland Financial Solutions - Phoenix Systems
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments

// Screen Id = 12349 - frmWorkQueueNoteEdit.cs - Edit ther Request record.
// Next Available Error Number

//-------------------------------------------------------------------------------
// File Name: frmWorkQueueNoteEdit.cs
// NameSpace: Phoenix.Client.WorkQueue
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//10/16/06		1		VDevadoss	#69248 - Created
//11/17/06		2		VDevadoss	#70826 - Line between notes is too long – overlaps to next line.
//											Also, hidden grid can be seen if Action bar is moved out of the way.
//01/25/2009    3       Nelsehety   #01698 - Bug Fixing
//10/22/2009    4       vdevadoss   #5752 - Hook up the Hybrid report REF001.
//08/06/2012    5       Mkrishna    #19058 - Adding call to base on initParameters.
//05/22/2014	6		JRHyne		WI#290008 - disallow ui prefs, not grid, shouldn't be maximizing
//04/13/2015    7       rpoddar     #177003 - Resing fixes
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Xml;

using Phoenix.BusObj.Global;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;
using Phoenix.Windows.Forms;
using Phoenix.Windows.Client;
using Phoenix.Shared;
using Phoenix.Busobj.Reports;
using Phoenix.BusObj.Misc;
using Phoenix.Shared.Printing;
using Phoenix.Shared.Windows;

namespace Phoenix.Client.WorkQueue
{
	/// <summary>
	/// Summary description for frmWorkQueueNotesDisplay.
	/// </summary>
	public class frmWorkQueueNotesDisplay : Phoenix.Windows.Forms.PfwStandard
	{
		#region Private Vars
		private Phoenix.BusObj.Global.GbWorkQueueNote _gbWorkQueueNote = new Phoenix.BusObj.Global.GbWorkQueueNote();
		private StringBuilder sDataView = new StringBuilder();
		const string sSeperatorLine = "___________________________________________________________________________________________________________" + "\r\n\r\n";

        #region #01698
        GbWorkQueue _gbWorkQueue;
        #endregion
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Phoenix.Windows.Forms.PGrid TmpGrid;
		private Phoenix.Windows.Forms.PGridColumn colCreateDt;
		private Phoenix.Windows.Forms.PGridColumn colDueDt;
		private Phoenix.Windows.Forms.PGridColumn colName;
		private Phoenix.Windows.Forms.PGridColumn colStatus;
		private Phoenix.Windows.Forms.PGridColumn colCompleteDt;
		private Phoenix.Windows.Forms.PGridColumn colNoteTitle;
		private Phoenix.Windows.Forms.PGridColumn colTaskInfo;
		private Phoenix.Windows.Forms.PGridColumn colNoteText1;
		private Phoenix.Windows.Forms.PGridColumn colNoteText2;
		private Phoenix.Windows.Forms.PGridColumn colNoteText3;
		private Phoenix.Windows.Forms.PGridColumn colNoteText4;
		private Phoenix.Windows.Forms.PGridColumn colNoteType;
        private PPanel pPanel2;
        private PPanel pPanel1;

		private Phoenix.Windows.Forms.PdfStandard mlNotes;
      
		#endregion

		#region Constructor
		public frmWorkQueueNotesDisplay()
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

		#endregion
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mlNotes = new Phoenix.Windows.Forms.PdfStandard();
			this.TmpGrid = new Phoenix.Windows.Forms.PGrid();
			this.colCreateDt = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteType = new Phoenix.Windows.Forms.PGridColumn();
			this.colName = new Phoenix.Windows.Forms.PGridColumn();
			this.colDueDt = new Phoenix.Windows.Forms.PGridColumn();
			this.colStatus = new Phoenix.Windows.Forms.PGridColumn();
			this.colCompleteDt = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteTitle = new Phoenix.Windows.Forms.PGridColumn();
			this.colTaskInfo = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteText1 = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteText2 = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteText3 = new Phoenix.Windows.Forms.PGridColumn();
			this.colNoteText4 = new Phoenix.Windows.Forms.PGridColumn();
			this.pPanel2 = new Phoenix.Windows.Forms.PPanel();
			this.pPanel1 = new Phoenix.Windows.Forms.PPanel();
			this.pPanel2.SuspendLayout();
			this.pPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mlNotes
			// 
			this.mlNotes.AcceptsReturn = true;
			this.mlNotes.AcceptsTab = true;
			this.mlNotes.Location = new System.Drawing.Point(4, 8);
			this.mlNotes.Multiline = true;
			this.mlNotes.Name = "mlNotes";
			this.mlNotes.PhoenixUIControl.ObjectId = 1;
			this.mlNotes.ReadOnly = true;
			this.mlNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mlNotes.Size = new System.Drawing.Size(681, 437);
			this.mlNotes.TabIndex = 0;
			this.mlNotes.TabStop = false;
			// 
			// TmpGrid
			// 
			this.TmpGrid.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colCreateDt,
            this.colNoteType,
            this.colName,
            this.colDueDt,
            this.colStatus,
            this.colCompleteDt,
            this.colNoteTitle,
            this.colTaskInfo,
            this.colNoteText1,
            this.colNoteText2,
            this.colNoteText3,
            this.colNoteText4});
			this.TmpGrid.IsMaxNumRowsCustomized = false;
			this.TmpGrid.Location = new System.Drawing.Point(56, 24);
			this.TmpGrid.Name = "TmpGrid";
			this.TmpGrid.Size = new System.Drawing.Size(256, 104);
			this.TmpGrid.TabIndex = 1;
			this.TmpGrid.Text = "pGrid1";
			this.TmpGrid.Visible = false;
			this.TmpGrid.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.TmpGrid_FetchRowDone);
			// 
			// colCreateDt
			// 
			this.colCreateDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colCreateDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colCreateDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colCreateDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colCreateDt.PhoenixUIControl.InputMask = "MM/dd/yyyy hh:mm tt";
			this.colCreateDt.PhoenixUIControl.XmlTag = "CreateDt";
			this.colCreateDt.Title = "CreateDt";
			// 
			// colNoteType
			// 
			this.colNoteType.PhoenixUIControl.XmlTag = "Type";
			this.colNoteType.Title = "Note Type";
			// 
			// colName
			// 
			this.colName.PhoenixUIControl.XmlTag = "Name";
			this.colName.Title = "Name";
			// 
			// colDueDt
			// 
			this.colDueDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colDueDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colDueDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colDueDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colDueDt.PhoenixUIControl.InputMask = "MM/dd/yyyy hh:mm:ss tt";
			this.colDueDt.PhoenixUIControl.XmlTag = "DueDt";
			this.colDueDt.Title = "DueDt";
			// 
			// colStatus
			// 
			this.colStatus.PhoenixUIControl.XmlTag = "Status";
			this.colStatus.Title = "Status";
			// 
			// colCompleteDt
			// 
			this.colCompleteDt.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colCompleteDt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colCompleteDt.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.DateTime;
			this.colCompleteDt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.DateTimeNoSeconds;
			this.colCompleteDt.PhoenixUIControl.InputMask = "MM/dd/yyyy hh:mm:ss tt";
			this.colCompleteDt.PhoenixUIControl.XmlTag = "CompleteDt";
			this.colCompleteDt.Title = "Complete Dt";
			// 
			// colNoteTitle
			// 
			this.colNoteTitle.PhoenixUIControl.XmlTag = "NoteTitle";
			this.colNoteTitle.Title = "Note Title";
			// 
			// colTaskInfo
			// 
			this.colTaskInfo.PhoenixUIControl.XmlTag = "TaskInfo";
			this.colTaskInfo.Title = "TaskInfo";
			// 
			// colNoteText1
			// 
			this.colNoteText1.PhoenixUIControl.XmlTag = "NoteText1";
			this.colNoteText1.Title = "Note Text 1";
			// 
			// colNoteText2
			// 
			this.colNoteText2.PhoenixUIControl.XmlTag = "NoteText2";
			this.colNoteText2.Title = "Note Text 2";
			// 
			// colNoteText3
			// 
			this.colNoteText3.PhoenixUIControl.XmlTag = "NoteText3";
			this.colNoteText3.Title = "Note Text 3";
			// 
			// colNoteText4
			// 
			this.colNoteText4.PhoenixUIControl.XmlTag = "NoteText4";
			this.colNoteText4.Title = "Note Text 4";
			// 
			// pPanel2
			// 
			this.pPanel2.Controls.Add(this.TmpGrid);
			this.pPanel2.Location = new System.Drawing.Point(60, 64);
			this.pPanel2.Name = "pPanel2";
			this.pPanel2.Size = new System.Drawing.Size(568, 132);
			this.pPanel2.TabIndex = 3;
			this.pPanel2.TabStop = true;
			// 
			// pPanel1
			// 
			this.pPanel1.Controls.Add(this.mlNotes);
			this.pPanel1.Location = new System.Drawing.Point(0, -4);
			this.pPanel1.Name = "pPanel1";
			this.pPanel1.Size = new System.Drawing.Size(692, 452);
			this.pPanel1.TabIndex = 4;
			this.pPanel1.TabStop = true;
			// 
			// frmWorkQueueNotesDisplay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 448);
			this.Controls.Add(this.pPanel1);
			this.Controls.Add(this.pPanel2);
			this.IsAllowedToBeMaximizedAlways = false;
			this.Name = "frmWorkQueueNotesDisplay";
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.frmWorkQueueNotesDisplay_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.frmWorkQueueNotesDisplay_PInitCompleteEvent);
			this.PMdiPrintEvent += new System.EventHandler(this.frmWorkQueueNotesDisplay_PMdiPrintEvent);
			this.pPanel2.ResumeLayout(false);
			this.pPanel1.ResumeLayout(false);
			this.pPanel1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		#region Init
		public override void InitParameters(params Object[] paramList)
		{
			this.IsNew = false;

			if (paramList.Length == 2)
			{
				if(paramList[0] != null)
				{
					this._gbWorkQueueNote.WorkId.Value = Convert.ToInt32(paramList[0]);
				}
				if(paramList[1] != null)
				{
					this._gbWorkQueueNote.RecordType.Value = paramList[1].ToString().Trim();
                   
				}
                if (Phoenix.Shared.Variables.GlobalVars.Module == "Teller")
                {
				if (this._gbWorkQueueNote.RecordType.Value.Trim() == "Referral")
					this.ScreenId = 12507;
				else
					this.ScreenId = 12509;
                }
                #region Fixing Bugs
                else if (Phoenix.FrameWork.Shared.Variables.GlobalVars.Module == "AcProc")
                {
                    if (this._gbWorkQueueNote.RecordType.Value.Trim() == "Referral")
                        this.ScreenId = Phoenix.Shared.Constants.ScreenId.RefWorkQueueNotesDisplay;
                    else
                        this.ScreenId = Phoenix.Shared.Constants.ScreenId.ReqWorkQueueNotesDisplay;

                }
                #endregion

				this._gbWorkQueueNote.OutputType.Value = this.ScreenId;
			}

            base.InitParameters(paramList); //#19058

		}
		#endregion	

		#region Begin / Complete Events
		private Phoenix.Windows.Forms.ReturnType frmWorkQueueNotesDisplay_PInitBeginEvent()
		{
			this.MainBusinesObject = this._gbWorkQueueNote;
			this.AutoFetch = false;

            // Begin #177003, #36354
            if (Workspace is PwksWindow && (Workspace as PwksWindow).IsHighResWorkspace)
            {
                //this.mlNotes.Dock = DockStyle.Fill;
                (this.Extension as Phoenix.Shared.Windows.FormExtension).ResizeFormEnd += new EventHandler(frmWorkQueueNotesDisplay_ResizeFormEnd);
            }
            // End #177003, #36354

			return ReturnType.Success;
		}

        void frmWorkQueueNotesDisplay_ResizeFormEnd(object sender, EventArgs e)
        {
            Phoenix.Shared.Windows.FormExtension formExt = Extension as Phoenix.Shared.Windows.FormExtension;
            if (formExt == null)
                return;

            pPanel1.Height = formExt.WksAvailableHeight - pPanel1.Top - 4;
            mlNotes.Height = pPanel1.Height - mlNotes.Top - 4;
        }

		private void frmWorkQueueNotesDisplay_PInitCompleteEvent()
		{
          
			XmlNode _nodeListview = Phoenix.FrameWork.CDS.DataService.Instance.GetListView(this._gbWorkQueueNote, null);
			this.TmpGrid.PopulateTable(_nodeListview, true);
			

            #region Adjust Screen Title
            CallXMThruCDS(CallXMThru.FormInitComplete);
            if (!_gbWorkQueue.RimNo.IsNull)
            {
                GbHelper gbHelper = new GbHelper();
                string localCustomerName = gbHelper.GetNameDetails(_gbWorkQueue.RimNo.Value);
                if (localCustomerName == null || localCustomerName.Trim().Length == 0)
                {
                    localCustomerName = _gbWorkQueue.CustomerName.Value;
                }


                if (this._gbWorkQueueNote.RecordType.Value.Trim() == "Referral")
                {
                    this.EditRecordTitle = string.Format("{0} [{1}]", this.EditRecordTitle, localCustomerName);
                }
                else
                {
                    this.EditRecordTitle = string.Format("{0} ({1} {2}-{3})", this.EditRecordTitle,
                        localCustomerName,
                        _gbWorkQueue.AcctType.Value,
                        _gbWorkQueue.AcctNo.Value);
                }
                this.NewRecordTitle = this.EditRecordTitle;
            }
            #endregion
			
		}
		#endregion

		#region Grid Events (Note - The grid is hidden on the right side of the multiline box)
		private void TmpGrid_FetchRowDone(object sender, Phoenix.Windows.Forms.GridRowArgs e)
		{
           
			this.sDataView.Remove(0, sDataView.Length);
            #region Fixing Bugs
            this.sDataView.AppendFormat("Date:" + "\t\t\t" + (this.colCreateDt.Text.Trim()==string.Empty?string.Empty:Convert.ToDateTime(this.colCreateDt.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt")) + "\r\n");
			this.sDataView.AppendFormat("Type:" + "\t\t\t" + this.colNoteType.Text.Trim() + "\r\n");
			this.sDataView.AppendFormat("Created By:" + "\t\t" + this.colName.Text.Trim() + "\r\n");
            this.sDataView.AppendFormat("Due Date:" + "\t\t" + (this.colDueDt.Text.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(this.colDueDt.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt")) + "\r\n");
			this.sDataView.AppendFormat("Status:" + "\t\t\t" + this.colStatus.Text.Trim() + "\r\n");
            this.sDataView.AppendFormat("Completed On:" + "\t\t" + (this.colCompleteDt.Text.Trim() == string.Empty ? string.Empty : Convert.ToDateTime(this.colCompleteDt.UnFormattedValue).ToString("MM/dd/yyyy hh:mm tt")) + "\r\n");
			this.sDataView.AppendFormat("Description:" + "\t\t" + this.colNoteTitle.Text.Trim() + "\r\n");
            #endregion
			string sNoteText = this.colNoteText1.Text.ToString() + this.colNoteText2.Text.ToString() + this.colNoteText3.Text.ToString() + this.colNoteText4.Text.ToString();
			sNoteText = sNoteText.Replace("\n", Environment.NewLine ); 
			this.sDataView.AppendFormat("Details:" + "\t\t\t" + sNoteText + "\r\n\r\n");

			sNoteText = this.colTaskInfo.Text.ToString();
			sNoteText = sNoteText.Replace("\n", Environment.NewLine ); 
			this.sDataView.AppendFormat("Comment:" + "\t\t\t" + sNoteText + "\r\n");

			//Append the above strings to the mlNotes multiline text.
			if (this.mlNotes.Text.Trim() == string.Empty)
				this.mlNotes.Text = sDataView.ToString();
			else
				this.mlNotes.Text += sSeperatorLine + sDataView.ToString();
		}
		#endregion

        #region CallXMThruCDS
        void CallXMThruCDS(CallXMThru origin)
        {
            if (origin == CallXMThru.FormInitComplete)
            {
                _gbWorkQueue = new GbWorkQueue();
                _gbWorkQueue.WorkId.Value = _gbWorkQueueNote.WorkId.Value;
                _gbWorkQueue.ActionType = XmActionType.Select;

                CoreService.DataService.ProcessRequest(_gbWorkQueue);

                HandleBusObjMessages(_gbWorkQueue);

            }

        }

        enum CallXMThru
        {
            FormInitComplete
        }
        
        #endregion

        //Begin report
        private void frmWorkQueueNotesDisplay_PMdiPrintEvent(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            Phoenix.BusObj.Misc.RunSqrReport sqrOnlineObj;
            sqrOnlineObj = new Phoenix.BusObj.Misc.RunSqrReport();

            // Online Execution mode
            sqrOnlineObj.ExecutionMode.Value = SQRExecutionMode.Online.ToString();

            sqrOnlineObj.RptFileName.Value = "REF00100";
            sqrOnlineObj.FromDt.Value = Phoenix.Shared.Variables.GlobalVars.SystemDate;
            sqrOnlineObj.ToDt.Value = Phoenix.Shared.Variables.GlobalVars.SystemDate;
            sqrOnlineObj.RunDate.Value = Phoenix.Shared.Variables.GlobalVars.SystemDate;
            sqrOnlineObj.EmplId.Value = Phoenix.Shared.Variables.GlobalVars.EmployeeId;
            sqrOnlineObj.OutputFileFormat.Value = "PDF";

            sqrOnlineObj.Param1.Value = this._gbWorkQueueNote.WorkId.Value.ToString();

            try
            {
                dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(11482));
                //11482 - Generating Mail Message Report. Please wait...

                sqrOnlineObj.ActionType = XmActionType.Select;
                DataService.Instance.ProcessRequest(XmActionType.Select, sqrOnlineObj);

                string url = sqrOnlineObj.OutputLink.StringValue;

                    PdfFileManipulation pdfFileManipulation = new PdfFileManipulation();
                    pdfFileManipulation.ShowUrlPdf(url);
            }
            catch (PhoenixException pe)
            {
                dlgInformation.Instance.HideInfo();
                Phoenix.FrameWork.Core.CoreService.LogPublisher.LogDebug(pe.ToString());
                PMessageBox.Show(11483, MessageType.Warning, MessageBoxButtons.OK, string.Empty);
                //11483 - Failed to generate the Work Queue Notes report. Please try again.
            }
            finally
            {
                dlgInformation.Instance.HideInfo();
            }
        }
        //End Report
	}
}
