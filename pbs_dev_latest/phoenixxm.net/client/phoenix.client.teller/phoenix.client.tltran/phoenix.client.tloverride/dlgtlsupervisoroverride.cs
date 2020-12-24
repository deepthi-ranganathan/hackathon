#region Comments

//-------------------------------------------------------------------------------
// File Name: dlgTlSupervisorOverride.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//??/??/??		1		?????		#????? - Created.
//08/16/07		2		JStainth	#72995 - Tran code was incorrect for override with captured items
//10/02/2007    3       mselvaga    #72916 - Fixed TCD
//12/09/2008    4       mselvaga    #76458 - Added EX account mask changes.
//02/11/09      5       DGupta      #76052 - ODP nsf Changes. Added Charge Amt.
//03/30/2010    6       SDhamija	##79572 - Teller Fraud Detection
//04/01/2010    7       mselvaga    #79574 - Added Cash Recyler changes. - MERGED
//04/19/2010	8		rpoddar		#79510 	Shared Branch Changes	- MERGED
//05/11/2010    9       sdhamija	#8765
//05/20/2010    10      sdhamija	#8765 - colFraudPtid should not be visible.
//06/09/2010    11      sdhamija	#9314 - View Fraud button may be enabled for cust as well.
//06/10/2010    12      sdhamija	#9313 - gb_fraud_log records not included while coming from journal
//07/24/2010    13      mselvaga    WI#9958 - Offline - Receiving application error when selecting Display action on Supervisor Overrideable Errors window.
//08/11/2010    14      sdhamija	CR#9530 - added dfAddlInfo, and added support for GbFraudLog insertion on cancel.
//08/19/2010    15      sdhamija	CR#9539
//10/25/2010    16      rpoddar     #79510, #10883-2 Close event not getting fired when the focus in on actional panel
//10/29/2010    17      sdhamija	#11125
//01/11/2011    18      mselvaga    #79314 - Added Remote Override changes.
//02/10/2011    19      mselvaga    #12769 - DUT 79314 - Supervisor View - When the “Journal” button is selected the system is not going to the Teller Journal on the Supervisor Overrideable Errors Window (16001 - dlgTlSupervisorOverride).
//03/01/2011    20      mselvaga    #13052 - DUT 79314 - From Teller Journal to the Supervisor Overrideabe Errors window (16001 - dlgTlSupervisorOverride) the "View Decision" button is enabled for non-remote override.
//03/01/2011    21      mselvaga    #13051 - DUT 79314 - Supervisor View - When the supervisor has accepted the override request from the queue if the "Esc" keys is pressed...
//04/01/2011    22      mselvaga    #80617 - Addl TCs
//04/01/2011    23      sdhamija	#13220 - PhoenixEFE Teller Allowing Fraud/OFAC Override Even If Teller Class Not Configured to Allow
//03/23/2011    22      rpoddar     #79420 - Float Changes
//04/25/2011    25      mselvaga    #13380 - Remote Supervisor Override - Cancel on Supervisor Overrideable Errors window.
//05/09/2011    26      mselvaga    #14080 - Remote Supervisor Override - Supervisor Overrideable Errors window. Added valid object id for cmbSupervisorGroup.
//05/11/2011    27      mselvaga    #14098 - Remote Supervisor Override - View Fraud not available.
//05/17/2011    28      mselvaga    #14101 - Remote Supervisor Override - Supervisor Overrideable Errors window - View Fraud & Pending Tran.
//05/23/2011    29      mselvaga    #13993 - Passed in drawer posting date as param to override decision window.
//06/24/2011	30		Sdhamija	2010 #14691; 2011 #14692
//09/13/2011    31      mselvaga    #15143 - UAT2-79314-Supervisor Display action button is not active on the Supervisor Override Errors screen.
//09/14/2011    32      mselvaga    #15239 - Supervisor in "All" group (sequential) receives ignore message if 1st supervisor accepts overrride.
//09/16/2011    33      mselvaga    #15317 - UAT 2 79314-Message log saved message from previous override.
//10/17/2011    34      mselvaga    #80660 - Suspicious Transaction Scoring and Alert changes added.
//11/07/2011    35      LSimpson    #80660 - Suspicious Transaction Scoring and Alert modifications.  Added SuspiciousTransactionScoringAlertsCustomOption.
//10/11/2011    34      sdhamija    #15239 - 2nd supervisor's checkbox will not stop flashing.
//11/08/2011    35      mselvaga    #15332 - UAT 2 79314-Override is not returning to employee once time is up.
//01/18/2012    36      mselvaga    #16637 - Suspect Tran: GB_ACCT_SUSPECT_DETAIL not populating on cancelled transactions.
//01/30/2012    37      mselvaga    #16718 - Fixed onactionclose() for duplicate row insert into gb_acct_susepct.
//03/02/2012    38      LSimpson    #17041 - When getting GbAcctSuspect for detail window, loop to get current transaction.
//03/30/2012    39      mselvaga    #17349 - Beta 2012 - Teller freezing and incorrect supervisor name in override window.
//05/23/2012    40      fspath      #142717 -  Set the IsCustomizablePrefs to false on the grid since
//                                  columns are referenced by the index and this is not supported.
//08/28/2012	41		fspath		WI#18821 - set the is allowed to be favorite and is allowed to be maximized to false.
//07/18/2-13    42      FOyebola    Enh#140787 - IRA Annual Contribution limit
//01/22/2014    43       FOyebola    #161239 - Uncollected Funds Ovrdraft Protection
//12/15/2015    44      ThangarajS   Task#39792 - Displayed phoenix error message ,given user who has non-authorized & non-Supervisor
//12/12/2016    45      DEIland     Task#56237 - Fix issue when Supervisor Override is Overriding their own override and it does not set the _AdGbRsmLimits and thus allows success when they are over the limits
//09/05/2019    46      Arun        US #PHX-2573 | Task PHX-2580 | TFS #118329 - OFAC log changes.
//01/06/2020    47      Shebin      Task#123322  - ENH - Need to increase the amount of seconds the override message is displayed for the supervisor
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;   //#79314
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.Serialization; //#79314
//
using Phoenix.Windows.Forms;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.BusFrame;
using GlacialComponents.Controls;
using GlacialComponents.Controls.Common;
using Phoenix.Shared.Constants;
using Phoenix.Shared.Variables;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.CDS;				//2010 #14691; 2011 #14692

using Phoenix.BusObj.Admin.Global;
using Phoenix.FrameWork.Shared.Variables;
using Phoenix.Windows.Client;
using Phoenix.BusObj.Global; //#76458
using Phoenix.BusObj.Admin.Teller; //#79572
using Phoenix.Shared.Communicator;  //#79314
using Phoenix.FrameWork.Core.Utilities; //#79314

using Phoenix.Shared.Windows;				//2010 #14691; 2011 #14692
using Phoenix.BusObj.Misc;					//2010 #14691; 2011 #14692
using Phoenix.BusObj.Control;       /* Task #PHX-2580 */

namespace Phoenix.Windows.TlOverride
{
	/// <summary>
	/// Summary description for dlgOverrideableErrors.
	/// </summary>
	public class dlgTlSupervisorOverride : Phoenix.Windows.Forms.PfwStandard
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private Phoenix.Windows.Forms.PGroupBoxStandard gbInstructions;
		private Phoenix.Windows.Forms.PGrid gridOverrideableErrors ;
		private Phoenix.Windows.Forms.PGridColumn colAccount;
		private Phoenix.Windows.Forms.PGridColumn colOvrdType;
		private Phoenix.Windows.Forms.PGridColumn colTrancode;
		private Phoenix.Windows.Forms.PGridColumn colAmount;
		private Phoenix.Windows.Forms.PGridColumn colItemNo;
		private Phoenix.Windows.Forms.PGridColumn colError;
		private Phoenix.Windows.Forms.PGridColumn colOverridenby;
		private Phoenix.Windows.Forms.PGridColumn colSuperEmplId;
		private Phoenix.Windows.Forms.PGridColumn colAcctType;
		private Phoenix.Windows.Forms.PGridColumn colAcctNo;
		private Phoenix.Windows.Forms.PGridColumn colDepType;
		private Phoenix.Windows.Forms.PGridColumn colAcctId;
		private Phoenix.Windows.Forms.PGridColumn colViewAccess;
		private Phoenix.Windows.Forms.PAction pbOverride;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbAction;
		private Phoenix.Windows.Forms.PAction pbDisplay;
		private Phoenix.Windows.Forms.PAction pbDeselectAll;
		private Phoenix.Windows.Forms.PAction pbSelectAll;
		private Phoenix.Windows.Forms.PAction pbRepost;
		private Phoenix.Windows.Forms.PAction pbCancel;
		private Phoenix.Windows.Forms.PActionClose pbClose;
		private Phoenix.Windows.Forms.PGroupBoxStandard gbOverrides;
		private Phoenix.Windows.Forms.PLabelStandard lblInstruction;


		#region private vars
		private string caller;
		private ArrayList ovrdsArrays;
		private TlJournalOvrd _ovrdBusObj;
		private string tlTranCode;
		private decimal jrnlPtid;
		private TellerVars _tellerVars = TellerVars.Instance;
		private DialogResult dialogResult = DialogResult.None;
		private PSmallInt _superEmplId;
        private AdGbRsm _adGbRsm = (GlobalObjects.Instance[Phoenix.FrameWork.Shared.Variables.GlobalObjectNames.AdGbRsm] as AdGbRsm);   //#79314
		private AdGbRsmLimits _adGbRsmLimits;
		private PfwStandard _parentForm =  null;
		private Phoenix.Windows.Forms.PGridColumn colRimNo;
		private Phoenix.Windows.Forms.PGridColumn colDepLoan;
		private bool formRetValue = false;
        private PGridColumn colChargeAmt;
		private PGridColumn colAddlInfo;	//#79572
		private PGridColumn colFraudPtid;	//#79572
		private PGridColumn colEnteredBy;	//#79572
		private PGridColumnComboBox colOvrdReason;	//#79572
		private GbHelper _gbHelper; //#76458
		private bool _orListLoaded;
		private PGridColumn colOvrdId;
		private PAction pbViewFraud;
		private PGridColumn colNonCust; //#79572
		private bool isNonCust = false; //#79572
		private AdTlCls _adTlCls;		//#79572
		private int fraudReasonCode = 13228;
		private PGridColumn colOvrdReason1;
		private PdfStandard dfAddlInfo;
		private PLabelStandard lblAddlInfo;	//#79572
        private PGroupBoxStandard gbRemoteOverrideInformation;
        private PRadioButtonStandard rbAll;
        private PLabelStandard lblOvrdRouteBy;
        private PRadioButtonStandard rbSupervisor;
        private PRadioButtonStandard rbGroups;
        private PComboBoxStandard cmbSupervisorGroup;
        private PLabelStandard lblMessageLog;
        private PLinkLabel lblMessageInput;
        private PdfStandard mulMessageLog;
        private PAction pbSend;
        private PAction pbRelease;
        private PAction pbForward;
        private PAction pbApprove;
        private PAction pbDeny;
        private PAction pbPendingTran;	//#79572
		private bool hasFraud = false;	//#9032

        #region #79314
        private TlTransactionSet _tranSet = new TlTransactionSet();
        private GbFraudLog _gbFraudLog = new GbFraudLog();
        private TlOvrdTranInfo _tranInfo;
        private Phoenix.Shared.Communicator.CommunicatorService _comm;
        private string _currentMsg = string.Empty;
        private int _ovrdMessageInfoId = -1;
        private List<CommunicatorUser> _commUserlist = new List<CommunicatorUser>();
        private string _firstOvrdErrMsg = string.Empty;
        private PAction pbJournal;
        private PAction pbPosition;
        private AdTlOvrdGrp _tmpOvrdGrp = new AdTlOvrdGrp();
        private PAction pbViewDecision;
        private object _objectValue = null;
        public delegate void ReceiveMessageEventHandler(object sender, MessageEventArgs e);
        private MessageInfo _msgInfo;
        private PSmallInt _branchNo = new PSmallInt("BranchNo");
        private PSmallInt _drawerNo = new PSmallInt("DrawerNo");
        private PSmallInt _sequenceNo = new PSmallInt("SequenceNo");
        private PDateTime _effectiveDt = new PDateTime("EffectiveDt");
        private bool _isSupervisorOverrideActionPressed = true;
        private string _origMsgLogMessage = string.Empty;
        private PGridColumn colMessageId;
        private bool _isRemoteOvrdEnabled = false;
        private System.Windows.Forms.Timer _formTimer;  //#13826
        private System.Windows.Forms.Timer _queueTimer;  //#15087
        //private int _timerInterval = 0;
        private bool _enableViewDecision = false;   //#13052
        private bool closedWkspace = false;
		private PGridColumn colSdnUid;				//2010 #14691; 2011 #14692
		private PGridColumn colSdnFileType;			//2010 #14691; 2011 #14692
        private bool _isOvrdApproved = false;   //#13070
         private PAction _iconRemoteQ = Helper.IconRemoteQ();   //#15332
        private GbAcctSuspect _gbAcctSuspect;   //#80660
        private GbAcctSuspectDetail _gbAcctSuspectDetail;   //#80660
        private bool _hasSuspect = false;
        private PAction pbSuspectDtls;
        private PGridColumn colSuspectPtid;	//#80660
        private bool _onlyWarningOvrdExists = false; //#80660
        private PcCustomOptions _pcCustomOptions;  /* Task #PHX-2580 */
        #endregion


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

		public dlgTlSupervisorOverride()
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
			Phoenix.FrameWork.Core.ControlInfo controlInfo1 = new Phoenix.FrameWork.Core.ControlInfo();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgTlSupervisorOverride));
			this.gbInstructions = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.lblInstruction = new Phoenix.Windows.Forms.PLabelStandard();
			this.gbRemoteOverrideInformation = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.mulMessageLog = new Phoenix.Windows.Forms.PdfStandard();
			this.lblMessageLog = new Phoenix.Windows.Forms.PLabelStandard();
			this.lblMessageInput = new Phoenix.Windows.Forms.PLinkLabel();
			this.cmbSupervisorGroup = new Phoenix.Windows.Forms.PComboBoxStandard();
			this.lblOvrdRouteBy = new Phoenix.Windows.Forms.PLabelStandard();
			this.rbSupervisor = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbGroups = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.rbAll = new Phoenix.Windows.Forms.PRadioButtonStandard();
			this.gbOverrides = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.dfAddlInfo = new Phoenix.Windows.Forms.PdfStandard();
			this.lblAddlInfo = new Phoenix.Windows.Forms.PLabelStandard();
			this.gridOverrideableErrors = new Phoenix.Windows.Forms.PGrid();
			this.colAccount = new Phoenix.Windows.Forms.PGridColumn();
			this.colOvrdType = new Phoenix.Windows.Forms.PGridColumn();
			this.colTrancode = new Phoenix.Windows.Forms.PGridColumn();
			this.colAmount = new Phoenix.Windows.Forms.PGridColumn();
			this.colItemNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colError = new Phoenix.Windows.Forms.PGridColumn();
			this.colOverridenby = new Phoenix.Windows.Forms.PGridColumn();
			this.colSuperEmplId = new Phoenix.Windows.Forms.PGridColumn();
			this.colAcctType = new Phoenix.Windows.Forms.PGridColumn();
			this.colAcctNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colDepType = new Phoenix.Windows.Forms.PGridColumn();
			this.colAcctId = new Phoenix.Windows.Forms.PGridColumn();
			this.colRimNo = new Phoenix.Windows.Forms.PGridColumn();
			this.colDepLoan = new Phoenix.Windows.Forms.PGridColumn();
			this.colViewAccess = new Phoenix.Windows.Forms.PGridColumn();
			this.colChargeAmt = new Phoenix.Windows.Forms.PGridColumn();
			this.colAddlInfo = new Phoenix.Windows.Forms.PGridColumn();
			this.colFraudPtid = new Phoenix.Windows.Forms.PGridColumn();
			this.colEnteredBy = new Phoenix.Windows.Forms.PGridColumn();
			this.colOvrdId = new Phoenix.Windows.Forms.PGridColumn();
			this.colNonCust = new Phoenix.Windows.Forms.PGridColumn();
			this.colOvrdReason = new Phoenix.Windows.Forms.PGridColumnComboBox();
			this.colOvrdReason1 = new Phoenix.Windows.Forms.PGridColumn();
			this.colMessageId = new Phoenix.Windows.Forms.PGridColumn();
			this.colSdnUid = new Phoenix.Windows.Forms.PGridColumn();
			this.colSdnFileType = new Phoenix.Windows.Forms.PGridColumn();
			this.colSuspectPtid = new Phoenix.Windows.Forms.PGridColumn();
			this.gbAction = new Phoenix.Windows.Forms.PGroupBoxStandard();
			this.pbClose = new Phoenix.Windows.Forms.PActionClose();
			this.pbCancel = new Phoenix.Windows.Forms.PAction();
			this.pbDisplay = new Phoenix.Windows.Forms.PAction();
			this.pbDeselectAll = new Phoenix.Windows.Forms.PAction();
			this.pbSelectAll = new Phoenix.Windows.Forms.PAction();
			this.pbRepost = new Phoenix.Windows.Forms.PAction();
			this.pbOverride = new Phoenix.Windows.Forms.PAction();
			this.pbViewFraud = new Phoenix.Windows.Forms.PAction();
			this.pbSend = new Phoenix.Windows.Forms.PAction();
			this.pbRelease = new Phoenix.Windows.Forms.PAction();
			this.pbForward = new Phoenix.Windows.Forms.PAction();
			this.pbApprove = new Phoenix.Windows.Forms.PAction();
			this.pbDeny = new Phoenix.Windows.Forms.PAction();
			this.pbPendingTran = new Phoenix.Windows.Forms.PAction();
			this.pbJournal = new Phoenix.Windows.Forms.PAction();
			this.pbPosition = new Phoenix.Windows.Forms.PAction();
			this.pbViewDecision = new Phoenix.Windows.Forms.PAction();
			this.pbSuspectDtls = new Phoenix.Windows.Forms.PAction();
			this.gbInstructions.SuspendLayout();
			this.gbRemoteOverrideInformation.SuspendLayout();
			this.gbOverrides.SuspendLayout();
			this.SuspendLayout();
			//
			// ActionManager
			//
			this.ActionManager.Actions.AddRange(new Phoenix.Windows.Forms.PAction[] {
            this.pbClose,
            this.pbOverride,
            this.pbRepost,
            this.pbCancel,
            this.pbSelectAll,
            this.pbDeselectAll,
            this.pbDisplay,
            this.pbViewFraud,
            this.pbSend,
            this.pbRelease,
            this.pbForward,
            this.pbApprove,
            this.pbDeny,
            this.pbPendingTran,
            this.pbJournal,
            this.pbPosition,
            this.pbViewDecision,
            this.pbSuspectDtls});
			//
			// gbInstructions
			//
			this.gbInstructions.Controls.Add(this.lblInstruction);
			this.gbInstructions.Location = new System.Drawing.Point(4, 0);
			this.gbInstructions.Name = "gbInstructions";
			this.gbInstructions.PhoenixUIControl.ObjectId = 1;
			this.gbInstructions.Size = new System.Drawing.Size(684, 140);
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
			this.lblInstruction.Size = new System.Drawing.Size(669, 120);
			this.lblInstruction.TabIndex = 0;
			this.lblInstruction.Text = "The transaction you attempted to post rejected for the reason(s) below. With prop" +
    "er approval, these ";
			this.lblInstruction.WordWrap = true;
			//
			// gbRemoteOverrideInformation
			//
			this.gbRemoteOverrideInformation.Controls.Add(this.mulMessageLog);
			this.gbRemoteOverrideInformation.Controls.Add(this.lblMessageLog);
			this.gbRemoteOverrideInformation.Controls.Add(this.lblMessageInput);
			this.gbRemoteOverrideInformation.Controls.Add(this.cmbSupervisorGroup);
			this.gbRemoteOverrideInformation.Controls.Add(this.lblOvrdRouteBy);
			this.gbRemoteOverrideInformation.Controls.Add(this.rbSupervisor);
			this.gbRemoteOverrideInformation.Controls.Add(this.rbGroups);
			this.gbRemoteOverrideInformation.Controls.Add(this.rbAll);
			this.gbRemoteOverrideInformation.Location = new System.Drawing.Point(4, 0);
			this.gbRemoteOverrideInformation.Name = "gbRemoteOverrideInformation";
			this.gbRemoteOverrideInformation.PhoenixUIControl.ObjectId = 27;
			this.gbRemoteOverrideInformation.Size = new System.Drawing.Size(684, 140);
			this.gbRemoteOverrideInformation.TabIndex = 0;
			this.gbRemoteOverrideInformation.TabStop = false;
			this.gbRemoteOverrideInformation.Text = "Remote Override Information";
			//
			// mulMessageLog
			//
			this.mulMessageLog.AcceptsReturn = true;
			this.mulMessageLog.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.mulMessageLog.Location = new System.Drawing.Point(88, 60);
			this.mulMessageLog.Multiline = true;
			this.mulMessageLog.Name = "mulMessageLog";
			this.mulMessageLog.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.mulMessageLog.PhoenixUIControl.ObjectId = 34;
			this.mulMessageLog.ReadOnly = true;
			this.mulMessageLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mulMessageLog.Size = new System.Drawing.Size(592, 74);
			this.mulMessageLog.TabIndex = 7;
			this.mulMessageLog.TabStop = false;
			//
			// lblMessageLog
			//
			this.lblMessageLog.AutoEllipsis = true;
			this.lblMessageLog.Location = new System.Drawing.Point(4, 60);
			this.lblMessageLog.Name = "lblMessageLog";
			this.lblMessageLog.PhoenixUIControl.ObjectId = 34;
			this.lblMessageLog.Size = new System.Drawing.Size(80, 20);
			this.lblMessageLog.TabIndex = 6;
			this.lblMessageLog.Text = "Message Log:";
			//
			// lblMessageInput
			//
			this.lblMessageInput.AutoEllipsis = true;
			this.lblMessageInput.AutoSize = true;
			this.lblMessageInput.BackColor = System.Drawing.SystemColors.Control;
			this.lblMessageInput.Location = new System.Drawing.Point(4, 40);
			this.lblMessageInput.MLInfo = controlInfo1;
			this.lblMessageInput.Name = "lblMessageInput";
			this.lblMessageInput.PhoenixUIControl.ObjectId = 33;
			this.lblMessageInput.Size = new System.Drawing.Size(80, 13);
			this.lblMessageInput.TabIndex = 5;
			this.lblMessageInput.TabStop = true;
			this.lblMessageInput.Text = "Message Input:";
			this.lblMessageInput.Click += new System.EventHandler(this.lblMessageInput_Click);
			//
			// cmbSupervisorGroup
			//
			this.cmbSupervisorGroup.Location = new System.Drawing.Point(364, 16);
			this.cmbSupervisorGroup.Name = "cmbSupervisorGroup";
			this.cmbSupervisorGroup.PhoenixUIControl.ObjectId = 44;
			this.cmbSupervisorGroup.Size = new System.Drawing.Size(316, 21);
			this.cmbSupervisorGroup.TabIndex = 4;
			this.cmbSupervisorGroup.Value = null;
			//
			// lblOvrdRouteBy
			//
			this.lblOvrdRouteBy.AutoEllipsis = true;
			this.lblOvrdRouteBy.Location = new System.Drawing.Point(4, 16);
			this.lblOvrdRouteBy.Name = "lblOvrdRouteBy";
			this.lblOvrdRouteBy.PhoenixUIControl.ObjectId = 29;
			this.lblOvrdRouteBy.Size = new System.Drawing.Size(152, 20);
			this.lblOvrdRouteBy.TabIndex = 0;
			this.lblOvrdRouteBy.Text = "Override Request Sent From:";
			//
			// rbSupervisor
			//
			this.rbSupervisor.AutoSize = true;
			this.rbSupervisor.BackColor = System.Drawing.SystemColors.Control;
			this.rbSupervisor.Description = null;
			this.rbSupervisor.Location = new System.Drawing.Point(276, 16);
			this.rbSupervisor.Name = "rbSupervisor";
			this.rbSupervisor.PhoenixUIControl.ObjectId = 32;
			this.rbSupervisor.PhoenixUIControl.XmlTag = "OverrideRoute";
			this.rbSupervisor.Size = new System.Drawing.Size(81, 18);
			this.rbSupervisor.TabIndex = 3;
			this.rbSupervisor.Text = "Supervisor";
			this.rbSupervisor.UseVisualStyleBackColor = false;
			this.rbSupervisor.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbSupervisor_PhoenixUICheckedChangedEvent);
			//
			// rbGroups
			//
			this.rbGroups.AutoSize = true;
			this.rbGroups.BackColor = System.Drawing.SystemColors.Control;
			this.rbGroups.Description = null;
			this.rbGroups.Location = new System.Drawing.Point(208, 16);
			this.rbGroups.Name = "rbGroups";
			this.rbGroups.PhoenixUIControl.ObjectId = 31;
			this.rbGroups.PhoenixUIControl.XmlTag = "OverrideRoute";
			this.rbGroups.Size = new System.Drawing.Size(65, 18);
			this.rbGroups.TabIndex = 2;
			this.rbGroups.Text = "Groups";
			this.rbGroups.UseVisualStyleBackColor = false;
			this.rbGroups.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbGroups_PhoenixUICheckedChangedEvent);
			//
			// rbAll
			//
			this.rbAll.AutoSize = true;
			this.rbAll.BackColor = System.Drawing.SystemColors.Control;
			this.rbAll.Description = null;
			this.rbAll.Location = new System.Drawing.Point(164, 17);
			this.rbAll.Name = "rbAll";
			this.rbAll.PhoenixUIControl.ObjectId = 30;
			this.rbAll.PhoenixUIControl.XmlTag = "OverrideRoute";
			this.rbAll.Size = new System.Drawing.Size(42, 18);
			this.rbAll.TabIndex = 1;
			this.rbAll.Text = "All";
			this.rbAll.UseVisualStyleBackColor = false;
			this.rbAll.PhoenixUICheckedChangedEvent += new Phoenix.Windows.Forms.CheckedChangedEventHandler(this.rbAll_PhoenixUICheckedChangedEvent);
			//
			// gbOverrides
			//
			this.gbOverrides.Controls.Add(this.dfAddlInfo);
			this.gbOverrides.Controls.Add(this.lblAddlInfo);
			this.gbOverrides.Controls.Add(this.gridOverrideableErrors);
			this.gbOverrides.Location = new System.Drawing.Point(4, 139);
			this.gbOverrides.Name = "gbOverrides";
			this.gbOverrides.Size = new System.Drawing.Size(684, 309);
			this.gbOverrides.TabIndex = 0;
			this.gbOverrides.TabStop = false;
			this.gbOverrides.Text = "Overrides";
			//
			// dfAddlInfo
			//
			this.dfAddlInfo.Location = new System.Drawing.Point(147, 247);
			this.dfAddlInfo.Multiline = true;
			this.dfAddlInfo.Name = "dfAddlInfo";
			this.dfAddlInfo.PhoenixUIControl.ObjectId = 26;
			this.dfAddlInfo.Size = new System.Drawing.Size(533, 56);
			this.dfAddlInfo.TabIndex = 3;
			this.dfAddlInfo.PhoenixUIValidateEvent += new Phoenix.Windows.Forms.ValidateEventHandler(this.dfAddlInfo_PhoenixUIValidateEvent);
			//
			// lblAddlInfo
			//
			this.lblAddlInfo.AutoEllipsis = true;
			this.lblAddlInfo.Location = new System.Drawing.Point(5, 247);
			this.lblAddlInfo.Name = "lblAddlInfo";
			this.lblAddlInfo.PhoenixUIControl.ObjectId = 26;
			this.lblAddlInfo.Size = new System.Drawing.Size(136, 20);
			this.lblAddlInfo.TabIndex = 4;
			this.lblAddlInfo.Text = "additional information";
			//
			// gridOverrideableErrors
			//
			this.gridOverrideableErrors.Columns.AddRange(new Phoenix.Windows.Forms.PGridColumn[] {
            this.colAccount,
            this.colOvrdType,
            this.colTrancode,
            this.colAmount,
            this.colItemNo,
            this.colError,
            this.colOverridenby,
            this.colSuperEmplId,
            this.colAcctType,
            this.colAcctNo,
            this.colDepType,
            this.colAcctId,
            this.colRimNo,
            this.colDepLoan,
            this.colViewAccess,
            this.colChargeAmt,
            this.colAddlInfo,
            this.colFraudPtid,
            this.colEnteredBy,
            this.colOvrdId,
            this.colNonCust,
            this.colOvrdReason,
            this.colOvrdReason1,
            this.colMessageId,
            this.colSdnUid,
            this.colSdnFileType,
            this.colSuspectPtid});
			this.gridOverrideableErrors.IsCustomizablePrefs = false;
			this.gridOverrideableErrors.IsMaxNumRowsCustomized = false;
			this.gridOverrideableErrors.ItemWordWrap = true;
			this.gridOverrideableErrors.LinesInHeader = 2;
			this.gridOverrideableErrors.Location = new System.Drawing.Point(5, 16);
			this.gridOverrideableErrors.MultiSelect = true;
			this.gridOverrideableErrors.Name = "gridOverrideableErrors";
			this.gridOverrideableErrors.Size = new System.Drawing.Size(675, 226);
			this.gridOverrideableErrors.TabIndex = 0;
			this.gridOverrideableErrors.BeforePopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridOverrideableErrors_BeforePopulate);
			this.gridOverrideableErrors.FetchRowDone += new Phoenix.Windows.Forms.GridRowFetchDone(this.gridOverrideableErrors_FetchRowDone);
			this.gridOverrideableErrors.AfterPopulate += new Phoenix.Windows.Forms.GridPopulateEventHandler(this.gridOverrideableErrors_AfterPopulate);
			this.gridOverrideableErrors.SelectedIndexChanged += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridOverrideableErrors_SelectedIndexChanged);
			this.gridOverrideableErrors.RowClicked += new Phoenix.Windows.Forms.GridClickedEventHandler(this.gridOverrideableErrors_RowClicked);
			this.gridOverrideableErrors.Click += new System.EventHandler(this.gridOverrideableErrors_Click);
			//
			// colAccount
			//
			this.colAccount.PhoenixUIControl.ObjectId = 4;
			this.colAccount.PhoenixUIControl.XmlTag = "Account";
			this.colAccount.Title = "Account";
			this.colAccount.Width = 107;
			//
			// colOvrdType
			//
			this.colOvrdType.PhoenixUIControl.ObjectId = 5;
			this.colOvrdType.PhoenixUIControl.XmlTag = "OvrdType";
			this.colOvrdType.Title = "Ovrd Type";
			this.colOvrdType.Width = 60;
			//
			// colTrancode
			//
			this.colTrancode.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.colTrancode.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Text;
			this.colTrancode.PhoenixUIControl.ObjectId = 6;
			this.colTrancode.PhoenixUIControl.XmlTag = "TranCode";
			this.colTrancode.Title = "Tran Code";
			this.colTrancode.Width = 43;
			//
			// colAmount
			//
			this.colAmount.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colAmount.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colAmount.PhoenixUIControl.ObjectId = 7;
			this.colAmount.PhoenixUIControl.XmlTag = "Amount";
			this.colAmount.Title = "Amount";
			this.colAmount.Width = 69;
			//
			// colItemNo
			//
			this.colItemNo.PhoenixUIControl.ObjectId = 8;
			this.colItemNo.PhoenixUIControl.XmlTag = "ItemNo";
			this.colItemNo.Title = "Item #";
			this.colItemNo.Width = 40;
			//
			// colError
			//
			this.colError.PhoenixUIControl.ObjectId = 9;
			this.colError.PhoenixUIControl.XmlTag = "Error";
			this.colError.Title = "Error";
			this.colError.Width = 240;
			//
			// colOverridenby
			//
			this.colOverridenby.PhoenixUIControl.ObjectId = 10;
			this.colOverridenby.PhoenixUIControl.XmlTag = "OverridenBy";
			this.colOverridenby.Title = "Overriden By";
			//
			// colSuperEmplId
			//
			this.colSuperEmplId.PhoenixUIControl.XmlTag = "SuperEmplId";
			this.colSuperEmplId.Title = "Super Empl Id";
			this.colSuperEmplId.Visible = false;
			this.colSuperEmplId.Width = 0;
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
			// colDepType
			//
			this.colDepType.PhoenixUIControl.XmlTag = "1";
			this.colDepType.Title = "Column";
			this.colDepType.Visible = false;
			this.colDepType.Width = 0;
			//
			// colAcctId
			//
			this.colAcctId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colAcctId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colAcctId.PhoenixUIControl.XmlTag = "2";
			this.colAcctId.Title = "Column";
			this.colAcctId.Visible = false;
			this.colAcctId.Width = 0;
			//
			// colRimNo
			//
			this.colRimNo.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colRimNo.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colRimNo.PhoenixUIControl.XmlTag = "3";
			this.colRimNo.Title = "Column";
			this.colRimNo.Visible = false;
			this.colRimNo.Width = 0;
			//
			// colDepLoan
			//
			this.colDepLoan.PhoenixUIControl.XmlTag = "4";
			this.colDepLoan.Title = "Column";
			this.colDepLoan.Visible = false;
			this.colDepLoan.Width = 0;
			//
			// colViewAccess
			//
			this.colViewAccess.PhoenixUIControl.XmlTag = "30";
			this.colViewAccess.Title = "Column";
			this.colViewAccess.Visible = false;
			this.colViewAccess.Width = 0;
			//
			// colChargeAmt
			//
			this.colChargeAmt.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colChargeAmt.PhoenixUIControl.FormatType = Phoenix.Windows.Forms.UIFieldFormat.Amount;
			this.colChargeAmt.PhoenixUIControl.ObjectId = 19;
			this.colChargeAmt.PhoenixUIControl.XmlTag = "ChargeAmt";
			this.colChargeAmt.Title = "Charge Amt";
			//
			// colAddlInfo
			//
			this.colAddlInfo.PhoenixUIControl.ObjectId = 22;
			this.colAddlInfo.PhoenixUIControl.XmlTag = "AddlInfo";
			this.colAddlInfo.Title = "addl info";
			this.colAddlInfo.Width = 150;
			//
			// colFraudPtid
			//
			this.colFraudPtid.PhoenixUIControl.ObjectId = 23;
			this.colFraudPtid.PhoenixUIControl.XmlTag = "FraudPtid";
			this.colFraudPtid.Title = "Fraud Ptid";
			this.colFraudPtid.Visible = false;
			this.colFraudPtid.Width = 10;
			//
			// colEnteredBy
			//
			this.colEnteredBy.PhoenixUIControl.ObjectId = 20;
			this.colEnteredBy.PhoenixUIControl.XmlTag = "EnteredBy";
			this.colEnteredBy.Title = "Entered By";
			//
			// colOvrdId
			//
			this.colOvrdId.PhoenixUIControl.XmlTag = "OvrdId";
			this.colOvrdId.Title = "OvrdId";
			this.colOvrdId.Visible = false;
			//
			// colNonCust
			//
			this.colNonCust.PhoenixUIControl.XmlTag = "NonCust";
			this.colNonCust.Title = "NonCust";
			this.colNonCust.Visible = false;
			//
			// colOvrdReason
			//
			this.colOvrdReason.AutoDrop = false;
			this.colOvrdReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
			this.colOvrdReason.PhoenixUIControl.ObjectId = 21;
			this.colOvrdReason.PhoenixUIControl.XmlTag = "FalsePositiveId";
			this.colOvrdReason.ReadOnly = false;
			this.colOvrdReason.Title = "Override Reason";
			//
			// colOvrdReason1
			//
			this.colOvrdReason1.PhoenixUIControl.ObjectId = 21;
			this.colOvrdReason1.PhoenixUIControl.XmlTag = "OvrdReason";
			this.colOvrdReason1.Title = "Column";
			this.colOvrdReason1.Visible = false;
			//
			// colMessageId
			//
			this.colMessageId.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colMessageId.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colMessageId.PhoenixUIControl.XmlTag = "MessageId";
			this.colMessageId.Title = "MessageId";
			this.colMessageId.Visible = false;
			this.colMessageId.Width = 0;
			//
			// colSdnUid
			//
			this.colSdnUid.PhoenixUIControl.XmlTag = "5";
			this.colSdnUid.Title = "sdn uid";
			this.colSdnUid.Visible = false;
			//
			// colSdnFileType
			//
			this.colSdnFileType.PhoenixUIControl.XmlTag = "6";
			this.colSdnFileType.Title = "sdn file type";
			this.colSdnFileType.Visible = false;
			//
			// colSuspectPtid
			//
			this.colSuspectPtid.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colSuspectPtid.PhoenixUIControl.DataType = Phoenix.Windows.Forms.UIDataType.Integer;
			this.colSuspectPtid.PhoenixUIControl.XmlTag = "SuspectPtid";
			this.colSuspectPtid.Title = "Suspect Ptid";
			this.colSuspectPtid.Visible = false;
			this.colSuspectPtid.Width = 12;
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
			// pbClose
			//
			this.pbClose.Image = ((System.Drawing.Image)(resources.GetObject("pbClose.Image")));
			this.pbClose.ObjectId = -2;
			this.pbClose.Tag = null;
			this.pbClose.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbClose_Click);
			//
			// pbCancel
			//
			this.pbCancel.LongText = "&Cancel";
			this.pbCancel.ObjectId = 14;
			this.pbCancel.ShortText = "&Cancel";
			this.pbCancel.Tag = null;
			this.pbCancel.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbCancel_Click);
			//
			// pbDisplay
			//
			this.pbDisplay.LongText = "&Display";
			this.pbDisplay.ObjectId = 17;
			this.pbDisplay.ShortText = "&Display";
			this.pbDisplay.Tag = null;
			this.pbDisplay.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDisplay_Click);
			//
			// pbDeselectAll
			//
			this.pbDeselectAll.LongText = "Deselect &All";
			this.pbDeselectAll.ObjectId = 16;
			this.pbDeselectAll.ShortText = "Deselect &All";
			this.pbDeselectAll.Tag = null;
			this.pbDeselectAll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDeselectAll_Click);
			//
			// pbSelectAll
			//
			this.pbSelectAll.LongText = "&Select All";
			this.pbSelectAll.ObjectId = 15;
			this.pbSelectAll.ShortText = "&Select All";
			this.pbSelectAll.Tag = null;
			this.pbSelectAll.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSelectAll_Click);
			//
			// pbRepost
			//
			this.pbRepost.LongText = "&Repost";
			this.pbRepost.ObjectId = 13;
			this.pbRepost.ShortText = "&Repost";
			this.pbRepost.Tag = null;
			this.pbRepost.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRepost_Click);
			//
			// pbOverride
			//
			this.pbOverride.LongText = "&Override";
			this.pbOverride.ObjectId = 12;
			this.pbOverride.ShortText = "&Override";
			this.pbOverride.Tag = null;
			this.pbOverride.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbOverride_Click);
			//
			// pbViewFraud
			//
			this.pbViewFraud.LongText = "view fraud";
			this.pbViewFraud.ObjectId = 25;
			this.pbViewFraud.ShortText = "view fraud";
			this.pbViewFraud.Tag = null;
			this.pbViewFraud.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbViewFraud_Click);
			//
			// pbSend
			//
			this.pbSend.ObjectId = 35;
			this.pbSend.Tag = null;
			this.pbSend.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSend_Click);
			//
			// pbRelease
			//
			this.pbRelease.ObjectId = 36;
			this.pbRelease.Tag = null;
			this.pbRelease.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbRelease_Click);
			//
			// pbForward
			//
			this.pbForward.ObjectId = 37;
			this.pbForward.Tag = null;
			this.pbForward.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbForward_Click);
			//
			// pbApprove
			//
			this.pbApprove.ObjectId = 38;
			this.pbApprove.Tag = null;
			this.pbApprove.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbApprove_Click);
			//
			// pbDeny
			//
			this.pbDeny.ObjectId = 39;
			this.pbDeny.Tag = null;
			this.pbDeny.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbDeny_Click);
			//
			// pbPendingTran
			//
			this.pbPendingTran.ObjectId = 40;
			this.pbPendingTran.Tag = null;
			this.pbPendingTran.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPendingTran_Click);
			//
			// pbJournal
			//
			this.pbJournal.ObjectId = 41;
			this.pbJournal.Tag = null;
			this.pbJournal.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbJournal_Click);
			//
			// pbPosition
			//
			this.pbPosition.ObjectId = 42;
			this.pbPosition.Tag = null;
			this.pbPosition.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbPosition_Click);
			//
			// pbViewDecision
			//
			this.pbViewDecision.ObjectId = 43;
			this.pbViewDecision.Tag = null;
			this.pbViewDecision.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbViewDecision_Click);
			//
			// pbSuspectDtls
			//
			this.pbSuspectDtls.LongText = "Suspect Dtl ";
			this.pbSuspectDtls.ObjectId = 45;
			this.pbSuspectDtls.ShortText = "Suspect Dtl ";
			this.pbSuspectDtls.Tag = null;
			this.pbSuspectDtls.Click += new Phoenix.Windows.Forms.PActionEventHandler(this.pbSuspectDtls_Click);
			//
			// dlgTlSupervisorOverride
			//
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(690, 450);
			this.Controls.Add(this.gbAction);
			this.Controls.Add(this.gbOverrides);
			this.Controls.Add(this.gbInstructions);
			this.Controls.Add(this.gbRemoteOverrideInformation);
			this.IsAllowedToBeFavorited = false;
			this.IsAllowedToBeMaximizedAlways = false;
			this.Name = "dlgTlSupervisorOverride";
			this.ScreenId = 16001;
			this.ScreenType = Phoenix.Windows.Forms.PScreenType.None;
			this.PInitBeginEvent += new Phoenix.Windows.Forms.FormInitBeginEventHandler(this.dlgTlSupervisorOverride_PInitBeginEvent);
			this.PInitCompleteEvent += new Phoenix.Windows.Forms.FormInitCompleteEventHandler(this.dlgTlSupervisorOverride_PInitCompleteEvent);
			this.PMdiPrintEvent += new System.EventHandler(this.dlgTlSupervisorOverride_PMdiPrintEvent);
			this.PShowCompletedEvent += new System.EventHandler(this.dlgTlSupervisorOverride_PShowCompletedEvent);
			this.Closed += new System.EventHandler(this.dlgTlSupervisorOverride_Closed);
			this.gbInstructions.ResumeLayout(false);
			this.gbRemoteOverrideInformation.ResumeLayout(false);
			this.gbRemoteOverrideInformation.PerformLayout();
			this.gbOverrides.ResumeLayout(false);
			this.gbOverrides.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		public override void InitParameters(params object[] paramList)
		{
			if( paramList.Length > 0 )
				caller = paramList[0] as string;

            _isRemoteOvrdEnabled = IsRemoteOverrideEnabled();   //#79314

			if ( caller == "TranCodePost" )
			{
                if (paramList.Length > 1)   //#79314
                {
                    if (paramList.Length > 1 && paramList[1] != null)   //#79314
                        ovrdsArrays = paramList[1] as ArrayList;
                    if (paramList.Length > 2 && paramList[2] != null)   //#79314
                        _tranSet = paramList[2] as TlTransactionSet;
                    if (paramList.Length > 3 && paramList[3] != null)   //#79314
                        _ovrdMessageInfoId = Convert.ToInt32(paramList[3]);
                }
			}
			if ( caller == "TranCodeCTR" )
			{
				if ( paramList.Length == 2 )
				{
					_ovrdBusObj = paramList[1] as TlJournalOvrd;
				}
			}
			else if ( caller == "JournalRev" || caller == "Position" )
			{
                if (paramList.Length >= 2)    //#79314
                {
                    _ovrdBusObj = paramList[1] as TlJournalOvrd;
                    if (caller == "JournalRev") //#79314
                    {
                        if (paramList.Length > 2 && paramList[2] != null)
                            _branchNo.Value = Convert.ToInt16(paramList[2]);
                        if (paramList.Length > 3 && paramList[3] != null)
                            _drawerNo.Value = Convert.ToInt16(paramList[3]);
                        if (paramList.Length > 4 && paramList[4] != null)
                            _sequenceNo.Value = Convert.ToInt16(paramList[4]);
                        if (paramList.Length > 5 && paramList[5] != null)
                            _effectiveDt.Value = Convert.ToDateTime(paramList[5]);
                    }
                    else if (caller == "Position") //#79314
                    {
                        if (paramList.Length > 2 && paramList[2] != null)
                            _branchNo.Value = Convert.ToInt16(paramList[2]);
                        if (paramList.Length > 3 && paramList[3] != null)
                            _drawerNo.Value = Convert.ToInt16(paramList[3]);
                        if (paramList.Length > 4 && paramList[4] != null)
                            _effectiveDt.Value = Convert.ToDateTime(paramList[4]);
                    }
                }
			}
			else if (caller == "JournalView")
			{
                //#13993
                //if (paramList.Length == 3)
                //{
                //    jrnlPtid = Convert.ToDecimal(paramList[1]);
                //    tlTranCode = paramList[2] as string;
                //}
                if (paramList.Length > 1 && paramList[1] != null)
                    jrnlPtid = Convert.ToDecimal(paramList[1]);
                if (paramList.Length > 2 && paramList[2] != null)
                    tlTranCode = paramList[2] as string;
                if (paramList.Length > 3 && paramList[3] != null)
                    _effectiveDt.Value = Convert.ToDateTime(paramList[3]);
			}
			//Begin #79572
			else if (caller == "FraudOfac")
			{
				if (paramList.Length > 1)
				{
					ovrdsArrays = paramList[1] as ArrayList;
					isNonCust = true;
					hasFraud = true;	//#8765
				}
			}
			//End #79572
            //
            else if (caller == "RemoteOverride")
            {
                if (paramList.Length > 1)   //#79314
                {
                    if (paramList[1] != null)   //#79314
                        _ovrdMessageInfoId = Convert.ToInt32(paramList[1]);
                    if (_ovrdMessageInfoId > 0)    //#79314
                    {
                        //ovrdsArrays = _tranSet.GetTranSetOvrdsCollection();
                        LoadOverridesFromTranOvrdInfo(_ovrdMessageInfoId, _tranSet, true, false);
                    }
                }
            }
		}

		private ReturnType dlgTlSupervisorOverride_PInitBeginEvent()
		{
			this.CurrencyId = _tellerVars.LocalCrncyId;
			this.MainBusinesObject = OvrdBusObj;
			this.ActionManager.ShowHiddenActions = false;
			_gbHelper = new GbHelper(); //#76458
            _pcCustomOptions = new PcCustomOptions();   /* Task #PHX-2580 */

			//Begin #79572
			if (isNonCust)
			{
				pbRepost.ObjectId = 24;
			}
			else
			{
				colAddlInfo.Visible = false;
				colFraudPtid.Visible = false;
				colEnteredBy.Visible = false;
				colOvrdReason.Visible = false;
			}
			//End #79572
            _comm = CommunicatorService.Instance;   //#79314
            //Begin #13121
            this.pbViewDecision.NextScreenId = Phoenix.Shared.Constants.ScreenId.TlRemoteOvrdDecision;
            this.pbPosition.NextScreenId = Phoenix.Shared.Constants.ScreenId.SummaryPosition;
            this.pbJournal.NextScreenId = Phoenix.Shared.Constants.ScreenId.Journal;
            this.pbPendingTran.NextScreenId = Phoenix.Shared.Constants.ScreenId.TlTranCode;
            //this.pbSuspectDtls.NextScreenId = Phoenix.Shared.Constants.ScreenId.SuspiciousTranDetails; //#80660
            //End #13121
            _onlyWarningOvrdExists = IsWarningOnlyOvrdExists();   //#80660
            if (_onlyWarningOvrdExists)   //#80660
                pbRepost.ObjectId = 24;
			return ReturnType.Success;
		}

		private void dlgTlSupervisorOverride_PInitCompleteEvent()
		{
			string sOverrideText; // #140786
			string sExtraPadding = ""; // #140786
			_parentForm = Workspace.ContentWindow.CurrentWindow as PfwStandard;

			if (_onlyWarningOvrdExists)		// #140786 - add if else block
			{
				sOverrideText = @"Press the 'Continue' button to post the transaction(s) after reviewing the error message(s).";
				sExtraPadding = "\r\n";
			}
			else
				sOverrideText = @"Press the 'Override' button to enter a Supervisor Override for the highlighted error(s).
Press the 'Repost' button to attempt to post the transaction(s) again.";

			if ( caller != "JournalView" )
			{
                if (caller == "FraudOfac")
                {
                    this.lblInstruction.Text = @"The system has located a matching OFAC SDN name or fraud entity or alias for the non customer attempting to be created.  This condition can be overridden by employees with the proper security rights.

				Press the 'Override' button to enter a Supervisor Override for the highlighted OFAC or fraud match(es).
				Press the 'Continue' button to continue with creating the new non customer record.
				Press the 'Cancel' button to return to the Add New Non Customer window.

				Otherwise, the group box will be populated as displayed above.";


                }
                else
                {
					// #140786 - replace hard coded message regarding Override button with variable sOverrideText
                    this.lblInstruction.Text = @"The transaction(s) rejected for the reason(s) below.  With proper approval, these errors can be overridden and the transactions can be reposted.  Transaction Code and Cash Limit rejections can be overridden by employees with proper limits to post the transaction.  All other rejections can be overridden by employees with 'Supervisor' rights and proper security.

" + sOverrideText + @"
Press the 'Cancel' button to return to the Post Transactions window without posting the transaction(s)." + sExtraPadding;

					// add line break to the end if only warning
					if (_onlyWarningOvrdExists)	// #140786
						this.lblInstruction.Text += "\r\n";
                }
				LoadOverrides( false );
				//begin #9032
				if (hasFraud)
					LoadOvrdReasonCombo(OvrdBusObj.GetFalsePositiveId());
				//end #9032
				pbDeselectAll_Click( pbDeselectAll, null );
				if (!SelectUpdateGridRows( "SelectFailedOvrds", -1 ))
				{
					//SelectUpdateGridRows( "SelectFirstRow", -1 );
					pbSelectAll_Click( pbSelectAll, null );
				}
			}

			EnableDisableVisibleLogic( "FormCreate" );
			//EnableDisableVisibleLogic( "TableClick" );    //#14101
            //
            if (_isRemoteOvrdEnabled && AppInfo.Instance.IsAppOnline && !_onlyWarningOvrdExists)           //#79314 #80660
            {
                this.rbAll.Checked = true;
                //#15317
                #region old code
                //if (_comm.CurrMessageId > 0 || _ovrdMessageInfoId > 0)
                //{
                //    int msgId = _ovrdMessageInfoId;
                //    if (msgId <= 0)
                //        msgId = _comm.CurrMessageId;
                //    if (msgId > 0)
                //    {
                //        RetrieveTranOvrdInfo(msgId);
                //        //
                //        if (_tranInfo != null && !_tranInfo.TlrSupChatInfo.IsNull)
                //        {
                //            mulMessageLog.Text = _tranInfo.TlrSupChatInfo.Value.Replace("\n", "\r\n");
                //            _origMsgLogMessage = mulMessageLog.Text;
                //        }
                //    }
                //}
                #endregion
                if (_ovrdMessageInfoId > 0)
                {
                    if (_ovrdMessageInfoId > 0)
                    {
                        RetrieveTranOvrdInfo(_ovrdMessageInfoId);
                        //
                        if (_tranInfo != null && !_tranInfo.TlrSupChatInfo.IsNull)
                        {
                            mulMessageLog.Text = _tranInfo.TlrSupChatInfo.Value.Replace("\n", "\r\n");
                            _origMsgLogMessage = mulMessageLog.Text;
                        }
                    }
                }
                EnableDisableVisibleLogic("RouteOverrideClick");
                //
                if (gridOverrideableErrors.Count > 0)
                {
                    _objectValue = gridOverrideableErrors.GetCellValueUnformatted(0, colError.ColumnId);
                    if (_objectValue != null)
                        _firstOvrdErrMsg = Convert.ToString(_objectValue);
                }
            }

            if (_onlyWarningOvrdExists)  //#80660
            {
                EnableDisableVisibleLogic("WarningOnly");
                this.UpdateView();
                this.Refresh();
            }

            EnableDisableVisibleLogic("TableClick");    //#14101 - moved after "RouteOverrideClick"

            #region #13051
            if (this.Workspace != null)
            {
                (this.Workspace as Form).FormClosing +=new FormClosingEventHandler(dlgTlSupervisorOverride_FormClosing);
            }
            #endregion

            SetFocusOnForm();
		}

        void dlgTlSupervisorOverride_PShowCompletedEvent(object sender, EventArgs e)
        {
            if (!gridOverrideableErrors.Focused)    //#13070
            {
                gridOverrideableErrors.Focus();
                if (gridOverrideableErrors.Count > 0)
                    gridOverrideableErrors.SelectRow(0, true);
                SetFocusOnForm();
            }
        }

		//begin 2010 #14691; 2011 #14692
		void dlgTlSupervisorOverride_PMdiPrintEvent(object sender, EventArgs e)
		{
			if (colSdnUid.Text.Trim() == string.Empty || colSdnFileType.Text.Trim() == string.Empty ||
				gridOverrideableErrors.SelectedIndicies.Count != 1)
			{
				PMessageBox.Show(13605, MessageType.Message, string.Empty);
			}
			else
			{
				PdfFileManipulation pdf = new PdfFileManipulation();
				//try
				//{
				//    pdf = new PdfFileManipulation();
				//}
				//catch (System.Runtime.InteropServices.COMException ex)
				//{
				//    CoreService.LogPublisher.LogDebug("\n(frmGbFraudLog window/ GenerateReport)For some reason creating of PdfFileManipulation Failed." + ex.ToString());
				//}
				RunSqrReport report1 = new RunSqrReport();
				report1.ReportName.Value = "RMO95100.sqr";
				report1.EmplId.Value = GlobalVars.EmployeeId;
				report1.FromDt.Value = GlobalVars.SystemDate;
				report1.ToDt.Value = GlobalVars.SystemDate;
				report1.RunDate.Value = DateTime.Now;
				string rimNo = string.Empty;
				string noMatchingChars = string.Empty;
				rimNo = string.Empty;
				report1.Param1.Value = "SDNUid=" + colSdnUid.Text.Trim() + "~" + "SDNFileType=" + colSdnFileType.Text.Trim() + "~" +
									   "RimNo=" + rimNo + "~" + "NoOfChars=" + noMatchingChars + "~" + "EmplId=" + GlobalVars.EmployeeId.ToString();
				report1.Param2.Value = "FLastName=" + string.Empty + "~" + "FFirstName=" + string.Empty + "~" + "ColTitle=" + string.Empty;
				report1.Param3.Value = "SLName=" + string.Empty + "~" + "SFName=" + string.Empty;
				report1.Param4.Value = string.Empty;
				report1.Param5.Value = string.Empty;
				report1.Param6.Value = string.Empty;
				report1.MiscParams.Value = string.Empty;
				try
				{
					dlgInformation.Instance.ShowInfo(CoreService.Translation.GetUserMessageX(360664));
					DataService.Instance.ProcessRequest(XmActionType.Select, report1);
					pdf.ShowUrlPdf(report1.OutputLink.Value);
				}
				finally
				{
					dlgInformation.Instance.HideInfo();
				}
			}
		}
		//End 2010 #14691; 2011 #14692

        #region #call parent
        public override void CallParent(params object[] paramList)  //#79314
        {
            string caller = "";
            bool addEmplName = true;

            if (paramList != null && paramList[0] != null)
                caller = Convert.ToString(paramList[0]);

            if (caller == "MessageInput")
            {
                if (paramList.Length > 1 && _isRemoteOvrdEnabled && mulMessageLog.Visible)
                {
                    _currentMsg = Convert.ToString(paramList[1]);
                    if (mulMessageLog != null && !string.IsNullOrEmpty(mulMessageLog.Text))
                    {
                        if (mulMessageLog.Text.Trim().StartsWith(GlobalVars.EmployeeName))
                            addEmplName = false;
                    }
                    mulMessageLog.Text = string.Format("{0}{1}{2}", (addEmplName ? GlobalVars.EmployeeName + ":" + "\r\n" : string.Empty), _currentMsg + "\r\n", _origMsgLogMessage);
                }
            }
        }
        #endregion

		//Begin #79572
		public override bool OnActionClose()
		{
            decimal suspParentPtid = 0; //#16637
            bool insertSuspDtls = false; //#16637

            if (!_isRemoteOvrdEnabled || (_isRemoteOvrdEnabled && !IsWorkspaceReadOnly()))    //#79314
            {
                if (isNonCust && formRetValue)
                {
                    for (int selectedRow = 0; selectedRow < gridOverrideableErrors.Items.Count; ++selectedRow)
                    {
                        SetContextRow(selectedRow);
                        GbFraudLog ovrd = ovrdsArrays[selectedRow] as GbFraudLog;
                        if (colOvrdReason.UnFormattedValue != null && colOvrdReason.UnFormattedValue.ToString() != "")
                            ovrd.FalsePositiveId.Value = Convert.ToInt16(colOvrdReason.UnFormattedValue);

                        ovrd.SuperEmplId.Value = Convert.ToInt16(colSuperEmplId.UnFormattedValue);
                        ovrd.AddlInfo.Value = Convert.ToString(colAddlInfo.UnFormattedValue);	//#9530

                        //Begin #9539 - ovrd.OvrdId.Value == 1034 -> coming from non customer search.
                        if (ovrd.OvrdId.Value == 1034)
                            ovrd.ActionType = XmActionType.New;
                        else
                            ovrd.ActionType = XmActionType.Update;
                        //End #9539

                        CoreService.DataService.AddObject(ovrd);
                    }
                    CoreService.DataService.ProcessRequest();
                }
                //Begin #9530
                else if (!formRetValue && caller == "TranCodePost" && (hasFraud || _hasSuspect)) //#80660
                {
                    foreach (ArrayList ovrdsArray in ovrdsArrays)
                    {
                        foreach (TlJournalOvrd ovrd in ovrdsArray)
                        {
                            if (hasFraud)
                            {
                                GbFraudLog tmpGbFraudLog = new GbFraudLog();
                                tmpGbFraudLog.AddlInfo.Value = ovrd.AddlInfo.Value;
                                if (!ovrd.AcctNo.IsNull && ovrd.AcctType.Value == "RIM")	//#found while testing #11126
                                    tmpGbFraudLog.RimNo.Value = Convert.ToInt32(ovrd.AcctNo.Value.Replace("-", ""));

                                if (ovrd.NonCust.IsNull)
                                    tmpGbFraudLog.NonCust.Value = "N";
                                else
                                    tmpGbFraudLog.NonCust.Value = ovrd.NonCust.Value;

                                tmpGbFraudLog.FraudPtid.Value = ovrd.FraudPtid.Value;
                                tmpGbFraudLog.OvrdId.Value = ovrd.OvrdId.Value;
                                tmpGbFraudLog.ItemAcctNo.Value = ovrd.ItemAcctNo.Value;
                                tmpGbFraudLog.ItemRoutingNo.Value = ovrd.ItemRoutingNo.Value;
                                tmpGbFraudLog.Match.Value = "Y";
                                tmpGbFraudLog.Type.Value = ovrd.FraudType.Value;
                                //Begin #79314
                                tmpGbFraudLog.BranchNo.Value = _tellerVars.BranchNo;
                                tmpGbFraudLog.DrawerNo.Value = _tellerVars.DrawerNo;
                                if (_msgInfo != null)
                                {
                                    if (_msgInfo.MessageState == MessageState.Approved.ToString())
                                        tmpGbFraudLog.OvrdStatus.Value = 1;
                                    else if (_msgInfo.MessageState == MessageState.Denied.ToString())
                                        tmpGbFraudLog.OvrdStatus.Value = 2;
                                    else
                                        tmpGbFraudLog.OvrdStatus.Value = 0;
                                }
                                tmpGbFraudLog.MessageId.Value = _ovrdMessageInfoId;
                                //End #79314
                                tmpGbFraudLog.ActionType = XmActionType.New;
                                CoreService.DataService.AddObject(tmpGbFraudLog);
                                CoreService.DataService.ProcessRequest();
                            }
                        }
                    }
                    if (_hasSuspect && this.DialogResult == DialogResult.Cancel)    //#80660 #16718 - moved outside of ovrdsArray loop
                    {
                        foreach (TlTransaction tran in _tranSet.Transactions)
                        {
                            foreach (GbAcctSuspect tmpGbSuspect in tran.GbAcctSuspect)
                            {
                                if (_msgInfo != null)
                                {
                                    if (_msgInfo.MessageState == MessageState.Approved.ToString())
                                        tmpGbSuspect.OvrdStatus.Value = 1;
                                    else if (_msgInfo.MessageState == MessageState.Denied.ToString())
                                        tmpGbSuspect.OvrdStatus.Value = 2;
                                    else
                                        tmpGbSuspect.OvrdStatus.Value = 0;
                                }
                                tmpGbSuspect.MessageId.Value = _ovrdMessageInfoId;
                                tmpGbSuspect.ControlValue5.Value = "Not Posted";    //#16637
                                suspParentPtid = tmpGbSuspect.Ptid.Value;   //#16637
                                //End #79314
                                tmpGbSuspect.ActionType = XmActionType.New;
                                CoreService.DataService.AddObject(tmpGbSuspect);
                                CoreService.DataService.ProcessRequest();
                                //#16637
                                //Let's insert suspect details
                                insertSuspDtls = false;
                                foreach (GbAcctSuspectDetail tmpGbSuspDtls in tran.GbAcctSuspectDetail)
                                {
                                    if (tmpGbSuspDtls.ParentPtid.Value == suspParentPtid)
                                    {
                                        tmpGbSuspDtls.ParentPtid.Value = Convert.ToDecimal(tmpGbSuspect.IdentityField.Value);
                                        tmpGbSuspDtls.ActionType = XmActionType.New;
                                        CoreService.DataService.AddObject(tmpGbSuspDtls);
                                        insertSuspDtls = true;
                                    }
                                }
                                //
                                if (insertSuspDtls)
                                {
                                    CoreService.DataService.ProcessRequest();
                                    suspParentPtid = 0;
                                }
                            }
                        }
                    }
                }
                //End  #9530
            }

			return base.OnActionClose();
		}
		//End #79572

		private void dlgTlSupervisorOverride_Closed(object sender, EventArgs e)
		{
			if ( _parentForm != null )
				_parentForm.CallParent( "Override", formRetValue );
		}

		private void gridOverrideableErrors_BeforePopulate(object sender, GridPopulateArgs e)
		{
			if (caller == "JournalView")
			{
				this.lblInstruction.Text = @"The transaction(s) you posted were rejected and overridden for the reason(s) below.";
				OvrdBusObj.JournalPtid.Value = jrnlPtid;
				OvrdBusObj.OutputType.Value = 1;
				gridOverrideableErrors.ListViewObject = OvrdBusObj;
			}
            //
            _firstOvrdErrMsg = string.Empty;    //#79314
		}

		private void gridOverrideableErrors_FetchRowDone(object sender, GridRowArgs e)
		{
			colTrancode.UnFormattedValue = GetTranCode( tlTranCode, Convert.ToString( colTrancode.UnFormattedValue ),
				Convert.ToString( colOvrdType.UnFormattedValue ));
			colOvrdType.UnFormattedValue = DecodeOvrdType(Convert.ToString(colOvrdType.UnFormattedValue));
			if ( colAccount.UnFormattedValue != null && colAccount.UnFormattedValue.ToString().Trim() == "-" )
				colAccount.UnFormattedValue = String.Empty;

			//Begin #9313
			if(colFraudPtid.Text != "" && Convert.ToInt32(colFraudPtid.Text) > 0)
			{
				hasFraud = true;
				colOvrdReason1.Visible = true;
			}
			//End #9313
            if (!_enableViewDecision)   //#13052
                _enableViewDecision = (colMessageId.UnFormattedValue != null && Convert.ToInt32(colMessageId.UnFormattedValue) > 0);

			//if (colOvrdId.UnFormattedValue != null && (Convert.ToInt16(colOvrdId.UnFormattedValue) == OverrideID.SuspectItemTellerOverrideThresholdMet ||	// #140786 comment out
            //    Convert.ToInt16(colOvrdId.UnFormattedValue) == OverrideID.SuspectItemTellerWarningThresholdMet))
			if (_ovrdBusObj.IsWarning(Convert.ToInt32((colOvrdId.UnFormattedValue == null ? 0 : colOvrdId.UnFormattedValue))))	// #140786
				_hasSuspect = true;
		}

        void gridOverrideableErrors_AfterPopulate(object sender, GridPopulateArgs e)
        {
            if (_onlyWarningOvrdExists)
                EnableDisableVisibleLogic("WarningOnly");
        }

		private void pbClose_Click(object sender, PActionEventArgs e)
		{
			this.Close();
		}

		//Begin #79572
		void pbViewFraud_Click(object sender, PActionEventArgs e)
		{
			if(gridOverrideableErrors.ContextRow >= 0)
			{
				PfwStandard tempWin = Helper.CreateWindow("Phoenix.Client.GbFraud", "Phoenix.Client.Global", "frmGbFraudEdit");
				tempWin.Workspace = this.Workspace;
				tempWin.InitParameters(colFraudPtid.Text);		//#8765
				tempWin.Show();
			}
		}
		//End #79572

		#region events

		#region gridOverrideableErrors
		private void gridOverrideableErrors_SelectedIndexChanged(object source, GridClickEventArgs e)
		{
			EnableDisableVisibleLogic( "TableClick" );
			PopulateAddlInfo();	//#9530
		}

		//Begin #9530
		private void PopulateAddlInfo()
		{
			//for (int selectedRow = 0; selectedRow < gridOverrideableErrors.SelectedIndicies.Count; ++selectedRow)
			foreach(int i in gridOverrideableErrors.SelectedIndicies)
			{
				dfAddlInfo.SetValue(gridOverrideableErrors.Items[i].SubItems[colAddlInfo.ColumnId].Text);
			}
		}
		void dfAddlInfo_PhoenixUIValidateEvent(object sender, PCancelEventArgs e)
		{
			foreach(int i in gridOverrideableErrors.SelectedIndicies)
			{
				gridOverrideableErrors.Items[i].SubItems[colAddlInfo.ColumnId].Text = (dfAddlInfo.Value != null)?dfAddlInfo.Value.ToString():"";
			}
			//colAddlInfo.UnFormattedValue = dfAddlInfo.Value;
		}
		//void dfAddlInfo_PhoenixUIEditedEvent(object sender, ValueEditedEventArgs e)
		//{
		//    colAddlInfo.UnFormattedValue = dfAddlInfo.Value;
		//}
		//End #9530

		private void gridOverrideableErrors_Click(object sender, EventArgs e)
		{
			gridOverrideableErrors.Invalidate();
			EnableDisableVisibleLogic( "TableClick" );
			PopulateAddlInfo();	//#9530 - when page loads with more than 1 row selected, and you click one of the rows, _SelectedIndexChanged doesnt fire.
		}
		#endregion

		#region pbCancel
		private void pbCancel_Click( object sender, PActionEventArgs e )
		{
            bool doNotClose = false;

            if (!IsWorkspaceReadOnly() && IsRemoteOverrideEnabled())
            {
                if (_ovrdMessageInfoId > 0 && _comm != null && _comm.MessageList != null &&
                    _comm.GetMessageInfo(_ovrdMessageInfoId) != null)
                {
                    doNotClose = PMessageBox.Show(13382, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, String.Empty) == DialogResult.No;

                    if (doNotClose)
                    {
                        e.IsSuccess = false;
                        return;
                    }
                    if (_formTimer != null)   //#15143
                    {
                        _formTimer.Stop();
                        _formTimer.Dispose();
                    }
                    SendIgnoreMessage();
                }
            }
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion

		#region pbDisplay
		private void pbDisplay_Click( object sender, PActionEventArgs e )
		{
			CallOtherForms( "DisplayClick" );
		}
		#endregion

		#region pbDeselectAll
		private void pbDeselectAll_Click( object sender, PActionEventArgs e )
		{
			SelectUpdateGridRows( "DeSelectAllRows", -1 );
			EnableDisableVisibleLogic( "DeSelectAllClick" );
		}
		#endregion

		#region pbSelectAll
		private void pbSelectAll_Click( object sender, PActionEventArgs e )
		{
			SelectUpdateGridRows( "SelectAllRows", -1 );
			EnableDisableVisibleLogic( "SelectAllClick" );
			gridOverrideableErrors.Focus();
		}
		#endregion

		#region pbRepost
		private void pbRepost_Click( object sender, PActionEventArgs e )
		{
			short superEmplId = -1;
			int reasonCode = -1;
			bool retValue = false;

			#region verify overrides for ctr/rev/pos
			//#13220 - added (caller == "TranCodePost") for (PhoenixEFE Teller Allowing Fraud/OFAC Override Even If Teller Class Not Configured to Allow)
			if (caller == "TranCodeCTR" || caller == "JournalRev" || caller == "Position" || caller == "FraudOfac" || caller == "TranCodePost")
			{
				superEmplId = Convert.ToInt16( colSuperEmplId.UnFormattedValue );
				if ( superEmplId > 0 )
				{
					if ( _adGbRsm == null || _adGbRsm.EmployeeId.Value != superEmplId )
					{
						_adGbRsm = new AdGbRsm();
						_adGbRsm.EmployeeId.Value = Convert.ToInt16( colSuperEmplId.UnFormattedValue );
						_adGbRsm.ActionType = XmActionType.Select;
						CallXMThruCDS( "AdGbRsm" );

                        if ( caller == "Position" )     // Task#56237 - If we change this condition we must change below too 
						{
							_adGbRsmLimits = new AdGbRsmLimits();
							_adGbRsmLimits.EmployeeId.Value = superEmplId;
							_adGbRsmLimits.TellerClassCode.Value = _adGbRsm.TellerClassCode.Value;
							_adGbRsmLimits.CrncyId.Value = TellerVars.Instance.LocalCrncyId;
							_adGbRsmLimits.ActionType = XmActionType.Select;
							CallXMThruCDS( "AdGbRsmLimits" );

                        }
                    }

                    // Begin Task#56327 - Handle When AdGbRsm is set already but Not AdGbRsmLimits
                    if (_adGbRsmLimits == null && caller == "Position")  
                    {
                        _adGbRsmLimits = new AdGbRsmLimits();
                        _adGbRsmLimits.EmployeeId.Value = superEmplId;
                        _adGbRsmLimits.TellerClassCode.Value = _adGbRsm.TellerClassCode.Value;
                        _adGbRsmLimits.CrncyId.Value = TellerVars.Instance.LocalCrncyId;
                        _adGbRsmLimits.ActionType = XmActionType.Select;
                        CallXMThruCDS("AdGbRsmLimits");
                    }
                    // EndTask#56327

					//Begin #79572
					//#13220 - added (caller == "TranCodePost") for (PhoenixEFE Teller Allowing Fraud/OFAC Override Even If Teller Class Not Configured to Allow)
					if (caller == "FraudOfac" || (caller == "TranCodePost" && (colOvrdId.Text == "1021" ||
						colOvrdId.Text == "1022" || colOvrdId.Text == "1023" || colOvrdId.Text == "1024" ||
						colOvrdId.Text == "1025" || colOvrdId.Text == "1026" || colOvrdId.Text == "1027" ||
						colOvrdId.Text == "1028" || colOvrdId.Text == "1029" || colOvrdId.Text == "1030" ||
						colOvrdId.Text == "1031" || colOvrdId.Text == "1032" || colOvrdId.Text == "1033" ||
						colOvrdId.Text == "1034" || colOvrdId.Text == "1035")))
					{
						_adTlCls = new AdTlCls();
						_adTlCls.TellerClassCode.Value = _adGbRsm.TellerClassCode.Value;
						_adTlCls.SelectAllFields = false;
						_adTlCls.AllowOfacFraudOvrd.Selected = true;
						_adTlCls.ActionType = XmActionType.Select;
						CoreService.DataService.ProcessRequest(_adTlCls);
						if (_adTlCls.AllowOfacFraudOvrd.Value == "Y")
							retValue = true;
						else
							reasonCode = fraudReasonCode;

					}
					//End #79572
					//Begin #80617
					else if (caller == "TranCodePost" && colOvrdId.Text == "1036" && _adGbRsm.ForcePost.Value != "Y")
					{
						reasonCode = 13478;
					}
					else
						retValue = true;
					//End #80617

					//#13220 - opening up outer if for TranCodePost, want to make sure we are not getting un-neccessary overrides.
					if (caller != "TranCodePost")
					{
						if ( caller == "TranCodeCTR" )
							retValue = _ovrdBusObj.VerifyCTRDeferralOvrd( _adGbRsm, ref reasonCode );
						else if (caller == "JournalRev")
							retValue = _ovrdBusObj.VerifyReversalOvrd(_adGbRsm, ref reasonCode);
						else if (caller == "FraudOfac")
						{
							if(!retValue)
								reasonCode = fraudReasonCode;
						}
						//Begin #80617
						else if (colOvrdId.Text == "1036" && _adGbRsm.ForcePost.Value != "Y")
						{
							reasonCode = 13478;
						}
						//End #80617
						else
							retValue = _ovrdBusObj.VerifyOverShtLimitOvr( _ovrdBusObj.OvrdId.Value, _adGbRsm, _adGbRsmLimits,
								_ovrdBusObj.Amount.Value, ref reasonCode );
					}

					if ( !retValue )
					{
						PMessageBox.Show( reasonCode, MessageType.Warning, MessageBoxButtons.OK, String.Empty );
						return;
					}
				}
                else if (caller == "TranCodePost" && superEmplId <= 0 && _onlyWarningOvrdExists) //#80660
                    retValue = true;
				else
					return;
			}
			#endregion

			LoadOverrides( true );
			DialogResult = DialogResult.OK;
			formRetValue = true;
			Close();
		}
		#endregion

		#region pbOverride
		private void pbOverride_Click( object sender, PActionEventArgs e )
		{
            bool doNotAllowManualOverride = false;
			_superEmplId = new PSmallInt("A1");
            if (!IsWorkspaceReadOnly() && IsRemoteOverrideEnabled())
            {
                if (_ovrdMessageInfoId > 0 && _comm != null && _comm.MessageList != null &&
                    _comm.GetMessageInfo(_ovrdMessageInfoId) != null)
                {
                    doNotAllowManualOverride = PMessageBox.Show(13397, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, String.Empty) == DialogResult.No;
                    if (doNotAllowManualOverride)
                    {
                        e.IsSuccess = false;
                        return;
                    }
                    SendIgnoreMessage();
                }
            }
            _isRemoteOvrdEnabled = IsWorkspaceReadOnly();   //#79314
            /*Begin #39792*/
            do
            {
                if (!_superEmplId.IsNull)
                {
                    if (_tellerVars.EmployeeId == _superEmplId.Value)
                        PMessageBox.Show(15378, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                    else
                    {
                        AdGbRsm _adGbRsmOvrd = new AdGbRsm();
                        _adGbRsmOvrd.EmployeeId.Value = _superEmplId.Value;
                        _adGbRsmOvrd.ActionType = XmActionType.Select;
                        CoreService.DataService.ProcessRequest(_adGbRsmOvrd);
                        if (_adGbRsmOvrd.Supervisor.Value != GlobalVars.Instance.ML.Y)
                            PMessageBox.Show(15378, MessageType.Error, MessageBoxButtons.OK, string.Empty);
                        else
                            break;
                    }
                }  

                CallOtherForms("Override");

            } while ((Convert.ToInt32(colOvrdId.Text) == OverrideID.PostToOwnAcct) && (dialogResult == DialogResult.OK));
            /*End #39792*/
			
			if ( dialogResult == DialogResult.OK && !_superEmplId.IsNull )
			{
				SelectUpdateGridRows( "Update", _superEmplId.Value );
				EnableDisableVisibleLogic( "ApproveClick" );
			}
            //Begin #79510, #10883-2
            if (dialogResult == DialogResult.Cancel)
            {
                gridOverrideableErrors.Focus();
            }
            //End #79510, #10883-2
		}
		#endregion

		#endregion

		#region private methods

		private void CallXMThruCDS(string origin )
		{
			try
			{
				if ( origin == "EmployeePopulate" )
				{
					Phoenix.FrameWork.CDS.DataService.Instance.EnumValues( OvrdBusObj, OvrdBusObj.SuperEmplId );
				}
				else if ( origin == "AdGbRsm" )
				{
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _adGbRsm );
				}
				else if ( origin == "AdGbRsmLimits" )
				{
					Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest( _adGbRsmLimits );
				}
                else if (origin == "RemoteSupervisorGroup")
                {
                    Phoenix.FrameWork.CDS.DataService.Instance.EnumValues(OvrdBusObj, OvrdBusObj.OvrdGroupCode);
                }
                else if (origin == "AdTlOvrdGrpSelect")
                {
                    Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest(_tmpOvrdGrp);
                }
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
			}
		}

		private string DecodeOvrdType( string ovrdType )
		{
			if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "A" ))
				return CoreService.Translation.GetListItemX( ListId.OverrideType, "Account" );
			if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "T" ))
				return CoreService.Translation.GetListItemX( ListId.OverrideType, "TranCode" );
			if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "C" ))
				return CoreService.Translation.GetListItemX( ListId.OverrideType, "Cash" );
			if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "O" ))
				return CoreService.Translation.GetListItemX( ListId.OverrideType, "Other" );
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "D")) //#72916*
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "TCD Cash");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "V")) //#72916*
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "TCD Buy");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "P")) //#76052*
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "ODP");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "L")) //#76052*
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "ODL");
			if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "N")) //#76052*
				return CoreService.Translation.GetListItemX(ListId.OverrideType, "NSF");
			if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "F")) //#79572*
            {
                /*Begin -  Task PHX-2580 */
                if (!_pcCustomOptions.CheckCustomOptions(PcCustomOptions.CO_Constants.CO_FraudPrevention) &&
                    _pcCustomOptions.CheckCustomOptions(PcCustomOptions.CO_Constants.CO_LogOfacForChecks))
                {
                    return "OFAC";
                }
                else
                {
                    return CoreService.Translation.GetListItemX(ListId.OverrideType, "OFAC/Fraud");
                }
                /*End -  Task PHX-2580 */               
            }
				
			if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "S")) //#79574*
				return CoreService.Translation.GetListItemX(ListId.OverrideType, "TCR Sell");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "W")) //#80660
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "Suspect Warning");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "I")) //#80660
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "Suspect Override");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "U")) //#140787
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "Plan");
            if (ovrdType == CoreService.Translation.GetListItemX(ListId.OverrideType, "K")) //#161239
                return CoreService.Translation.GetListItemX(ListId.OverrideType, "UCF");

            //Begin #79420
            string decodeValue = CoreService.Translation.GetListDecodeValueX(ListId.OverrideType, ovrdType);
            if (decodeValue != null)
                return decodeValue;
            //End #79420
			return String.Empty;
		}

		private string GetTranCode( string tlTranCode, string tranCode, string ovrdType )
		{
			if ( ovrdType == CoreService.Translation.GetListItemX( ListId.OverrideType, "T" ))
				return tranCode;
			return tlTranCode;
		}

		private string GetSuperEmplName(short superEmplId)
		{
			if (_tellerVars.ComboCache["dlgTlSupervisorOverride.EmployeeName"] == null)
			{
				CallXMThruCDS("EmployeePopulate");
				_tellerVars.ComboCache["dlgTlSupervisorOverride.EmployeeName"] = OvrdBusObj.SuperEmplId.Constraint.EnumValues;
			}
			foreach (EnumValue e in _tellerVars.ComboCache["dlgTlSupervisorOverride.EmployeeName"])
			{
				if (e.CodeValueShort == superEmplId)
					return e.Description;
			}
            if (_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"] != null &&
                _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"].Count > 0)   //#17349
            {
                foreach (EnumValue e in _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"])
                {
                    if (e.CodeValueShort == superEmplId)
                        return e.Description;
                }
            }
			return String.Empty;
		}
		//Begin #79572
		private string GetEnteredByName(decimal fraudPtid)
		{
			return _ovrdBusObj.GetEnteredByName(fraudPtid);
		}
		private void LoadNonCustOverride(GbFraudLog ovrd)
		{
			GLItem listItem = new GLItem();
			//string acctType = "";
			string acctNo = ovrd.RimNo.Value.ToString();

			listItem.SubItems.Add(acctNo);	//account
			_tellerVars.SetContextObject("PcOverrideArray", ovrd.OvrdId.Value);
			listItem.SubItems.Add(DecodeOvrdType(_tellerVars.PcOverride.OvrdType.Value));	//override type
			listItem.SubItems.Add(string.Empty);	//Trancode
			listItem.SubItems.Add(string.Empty);	//amount
			listItem.SubItems.Add(string.Empty);	//item no

			listItem.SubItems.Add(_tellerVars.PcOverride.TellerDescription.IsNull ?
				_tellerVars.PcOverride.Description.Value : _tellerVars.PcOverride.TellerDescription.Value);

			listItem.SubItems.Add(ovrd.SuperEmplId.IsNull ? String.Empty : GetSuperEmplName(ovrd.SuperEmplId.Value));	//overriden by
			listItem.SubItems.Add(ovrd.SuperEmplId.IsNull ? String.Empty : ovrd.SuperEmplId.Value.ToString());			// super empl id
			listItem.SubItems.Add(string.Empty);	//accttype
			listItem.SubItems.Add(acctNo);			//acct no
			listItem.SubItems.Add(string.Empty);	//dep type
			listItem.SubItems.Add(string.Empty);	//acct id
			listItem.SubItems.Add(acctNo);		//rim no
			listItem.SubItems.Add(string.Empty);	//dep loan
			listItem.SubItems.Add(string.Empty);	//view access
			listItem.SubItems.Add(string.Empty);	//charge amt
			listItem.SubItems.Add(ovrd.AddlInfo.IsNull ? string.Empty : ovrd.AddlInfo.Value);
			listItem.SubItems.Add(ovrd.FraudPtid.IsNull ? string.Empty : ovrd.FraudPtid.Value.ToString());
			//listItem.SubItems.Add(ovrd.JournalPtid.IsNull ? string.Empty : GetEnteredByName(ovrd.JournalPtid.Value));
			listItem.SubItems.Add(Phoenix.Shared.Variables.TellerVars.EmployeeName);
			listItem.SubItems.Add(ovrd.OvrdId.Value.ToString());	//override id
			listItem.SubItems.Add("Y");					//non cust
			listItem.SubItems.Add(string.Empty);																//2010 #14691; 2011 #14692 -
			listItem.SubItems.Add(string.Empty);																//2010 #14691; 2011 #14692 -
			listItem.SubItems.Add(string.Empty);																//2010 #14691; 2011 #14692 -
			listItem.SubItems.Add(ovrd.SdnUid.IsNull?string.Empty:ovrd.SdnUid.Value.ToString());				//2010 #14691; 2011 #14692 - SDN Uid
			listItem.SubItems.Add(ovrd.SdnFileType.IsNull ? string.Empty : ovrd.SdnFileType.Value.ToString());	//2010 #14691; 2011 #14692- SDN File Type
			gridOverrideableErrors.Items.Add(listItem);
		}
		//End #79572
		private void LoadOverride(TlJournalOvrd ovrd)
		{
			GLItem listItem = new GLItem();
			string acctType = "";	//#79572
			string acctNo;

			if (!ovrd.AcctNo.IsNull && !ovrd.AcctType.IsNull &&
				ovrd.AcctNo.Value.Trim() != String.Empty &&
				ovrd.AcctType.Value.Trim() != String.Empty)
			{
				acctType = ovrd.AcctType.Value;
				acctNo = ovrd.AcctNo.Value;
				//Begin #76458
				if (_gbHelper.IsExternalAdapterAcct(acctType))
					listItem.SubItems.Add(acctType + "-" + _gbHelper.GetMaskedExtAcct(acctType, acctNo));
				else
					listItem.SubItems.Add(acctType + "-" + acctNo);
				//listItem.SubItems.Add(acctType + "-" + acctNo);
				//End #76458
			}
			else
			{
				//begin #79572, #9032
				if (ovrd.NonCust.Value == "Y" || (!ovrd.AcctNo.IsNull && (ovrd.AcctType.IsNull ||ovrd.AcctType.Value.Trim() != String.Empty) &&
								ovrd.AcctNo.Value.Trim() != String.Empty))
				{
					acctNo = ovrd.AcctNo.Value;
					listItem.SubItems.Add(acctNo);
				}
				//End #79572
				else
				{
					listItem.SubItems.Add(String.Empty);
					acctType = String.Empty;
					acctNo = String.Empty;
				}
			}

			_tellerVars.SetContextObject("PcOverrideArray", ovrd.OvrdId.Value);
			listItem.SubItems.Add(DecodeOvrdType(_tellerVars.PcOverride.OvrdType.Value));
			// handle monetary columns here
			// Begin 72995
			//Begin #79572
			if (ovrd.NonCust.Value == "Y")
			{
				listItem.SubItems.Add(string.Empty);
			}
			//End #79572
			else if (!ovrd.ItemNo.IsNull)
			{
				listItem.SubItems.Add(Convert.ToString(ovrd.TranCode.Value));
			}
			else
			{
				listItem.SubItems.Add(GetTranCode(ovrd.TlTranCode.Value,
										Convert.ToString(ovrd.TranCode.Value),
										_tellerVars.PcOverride.OvrdType.Value));
			}
			// End 72995
			listItem.SubItems.Add(gridOverrideableErrors.Columns[3].MakeFormattedValue(
				ovrd.Amount.IsNull ? 0 : ovrd.Amount.Value));

			//Begin #79572
			if (ovrd.NonCust.Value == "Y")
				listItem.SubItems.Add(String.Empty);
			else
				listItem.SubItems.Add(ovrd.ItemNo.IsNull ? String.Empty : ovrd.ItemNo.Value.ToString());
			//End #79572
			listItem.SubItems.Add(_tellerVars.PcOverride.TellerDescription.IsNull ?
				_tellerVars.PcOverride.Description.Value : _tellerVars.PcOverride.TellerDescription.Value);
			//			if ( !ovrd.SuperEmplName.IsNull )
			//				listItem.SubItems.Add( ovrd.SuperEmplName.Value );
			//			else
            if (!ovrd.AcctNo.IsNull && !ovrd.AcctType.IsNull &&
                (ovrd.MiscAcctInfo != null && string.IsNullOrEmpty(ovrd.MiscAcctInfo.DepLoan)))    //#15143
                LoadAcctDetails(ovrd.MiscAcctInfo, ovrd.AcctType.Value, ovrd.AcctNo.Value, ovrd.TranCode.Value);
            //
			listItem.SubItems.Add(ovrd.SuperEmplId.IsNull ? String.Empty : GetSuperEmplName(ovrd.SuperEmplId.Value));
			listItem.SubItems.Add(ovrd.SuperEmplId.IsNull ? String.Empty : ovrd.SuperEmplId.Value.ToString());
			listItem.SubItems.Add(acctType);
			listItem.SubItems.Add(acctNo);
			listItem.SubItems.Add(ovrd.MiscAcctInfo.DepType == null ? String.Empty : ovrd.MiscAcctInfo.DepType);
			listItem.SubItems.Add(ovrd.MiscAcctInfo.AcctId < 0 ? String.Empty : ovrd.MiscAcctInfo.AcctId.ToString());
			listItem.SubItems.Add(ovrd.MiscAcctInfo.RimNo < 0 ? String.Empty : ovrd.MiscAcctInfo.RimNo.ToString());
			listItem.SubItems.Add(ovrd.MiscAcctInfo.DepLoan == null ? String.Empty : ovrd.MiscAcctInfo.DepLoan);
			listItem.SubItems.Add(ovrd.MiscAcctInfo.ViewAccess == null ? GlobalVars.Instance.ML.Y : ovrd.MiscAcctInfo.ViewAccess);
			listItem.SubItems.Add(gridOverrideableErrors.Columns[3].MakeFormattedValue(
			ovrd.OdCcAmount.IsNull ? 0 : ovrd.OdCcAmount.Value));
			listItem.SubItems.Add(ovrd.AddlInfo.IsNull ? string.Empty : ovrd.AddlInfo.Value);							//#79572
			listItem.SubItems.Add(ovrd.FraudPtid.IsNull ? string.Empty : ovrd.FraudPtid.Value.ToString());				//#79572
			//listItem.SubItems.Add(ovrd.JournalPtid.IsNull ? string.Empty : GetEnteredByName(ovrd.JournalPtid.Value));	//#79572
			listItem.SubItems.Add(Phoenix.Shared.Variables.TellerVars.EmployeeName);									//#79572
			listItem.SubItems.Add(ovrd.OvrdId.Value.ToString());            //#79572
            //#80660
            listItem.SubItems.Add(string.Empty);    //NonCust
            listItem.SubItems.Add(string.Empty);    //OvrdReason1
            listItem.SubItems.Add(string.Empty);    //OvrdReason2
            listItem.SubItems.Add(string.Empty);    //MessageId
            listItem.SubItems.Add(string.Empty);    //SdnUid
            listItem.SubItems.Add(string.Empty);	//SdnFileType
            listItem.SubItems.Add(ovrd.SuspectPtid.IsNull? string.Empty : ovrd.SuspectPtid.Value.ToString());
			gridOverrideableErrors.Items.Add(listItem);
		}

        private bool IsWarningOnlyOvrdExists()  //#80660
        {
            bool warningOnlyExists = false;
            if (caller == "TranCodePost" && ovrdsArrays != null && ovrdsArrays.Count > 0)
            {
                warningOnlyExists = true;
                foreach (ArrayList ovrdsArray in ovrdsArrays)
                {
                    foreach (TlJournalOvrd ovrd in ovrdsArray)
                    {
                        // if (ovrd.OvrdId.Value != OverrideID.SuspectItemTellerWarningThresholdMet)  //#80660	// #140786 comment out
						if (!_ovrdBusObj.IsWarning(ovrd.OvrdId.IntValue))	// #140786
                            warningOnlyExists = false;
                    }
                }
            }
            return warningOnlyExists;
        }

        private void LoadOverrides(bool isLoadBackList)
        {
            bool process = false;
            try
            {
                if (_ovrdMessageInfoId != Int32.MinValue && (_msgInfo == null || (_msgInfo != null && _msgInfo.MessageState != MessageState.Approved.ToString())))
                    _msgInfo = _comm.GetMessageInfo(_ovrdMessageInfoId);

                if (!isLoadBackList)
                {
                    gridOverrideableErrors.ResetTable();

                    if (caller == "TranCodePost")
                    {
                        foreach (ArrayList ovrdsArray in ovrdsArrays)
                        {
                            foreach (TlJournalOvrd ovrd in ovrdsArray)
                            {
                                if (!ovrd.FraudPtid.IsNull)
                                    hasFraud = true;
                                if (!ovrd.SuspectPtid.IsNull)    //#80660
                                    _hasSuspect = true;
                                LoadOverride(ovrd);
                            }
                        }
                    }
                    //Begin #59572
                    else if (caller == "FraudOfac")
                    {
                        foreach (GbFraudLog ovrd in ovrdsArrays)
                        {
                            LoadNonCustOverride(ovrd);
                        }
                    }
                    //End #59572
                    else
                    {
                        LoadOverride(OvrdBusObj);
                    }
                }
                else
                {
                    int contextRow = 0;
                    Phoenix.FrameWork.CDS.DataService.Instance.Reset();
                    if (caller == "TranCodePost")
                    {
                        foreach (ArrayList ovrdsArray in ovrdsArrays)
                        {
                            foreach (TlJournalOvrd ovrd in ovrdsArray)
                            {
                                SetContextRow(contextRow);
                                contextRow++;
                                //Begin #79314
                                if (_onlyWarningOvrdExists)  //#80660
                                    ovrd.SuperEmplId.Value = -1;
                                else if (colSuperEmplId.UnFormattedValue != null && colSuperEmplId.UnFormattedValue.ToString() != String.Empty)
                                {
                                    ovrd.SuperEmplId.Value = Convert.ToInt16(colSuperEmplId.UnFormattedValue);
                                    ovrd.SuperEmplName.Value = Convert.ToString(colOverridenby.UnFormattedValue);
                                }
                                //Begin #79572
                                if (colOvrdReason.UnFormattedValue != null && colOvrdReason.UnFormattedValue.ToString() != "")
                                    ovrd.FalsePositiveId.Value = Convert.ToInt16(colOvrdReason.UnFormattedValue);
                                //End #79572
                                //Begin #9530
                                if (colAddlInfo.UnFormattedValue != null && colAddlInfo.UnFormattedValue.ToString() != "")
                                    ovrd.AddlInfo.Value = Convert.ToString(colAddlInfo.UnFormattedValue);
                                //End #9530
                                if (_isRemoteOvrdEnabled && _ovrdMessageInfoId > 0 && _msgInfo != null)
                                {
                                    if (ovrd.SuperEmplId.IsNull)
                                        ovrd.SuperEmplId.Value = Convert.ToInt16(GlobalVars.EmployeeId);
                                    if (ovrd.SuperEmplName.IsNull)
                                        ovrd.SuperEmplName.Value = GlobalVars.EmployeeName;
                                    //
                                    ovrd.BranchNo.Value = _tellerVars.BranchNo;
                                    ovrd.DrawerNo.Value = _tellerVars.DrawerNo;
                                    ovrd.TellerEmplId.Value = (short)_msgInfo.TlrEmplId;
                                    if (_msgInfo.MessageState == MessageState.Approved.ToString())
                                        ovrd.OvrdStatus.Value = 1;
                                    else if (_msgInfo.MessageState == MessageState.Denied.ToString())
                                        ovrd.OvrdStatus.Value = 2;
                                    else
                                        ovrd.OvrdStatus.Value = 0;
                                    ovrd.MessageId.Value = _ovrdMessageInfoId;
                                    //
                                    if (_msgInfo.MessageState == MessageState.Denied.ToString() ||
                                        _msgInfo.MessageState == MessageState.Released.ToString())
                                    {
                                        process = true;
                                        if (ovrd.JournalPtid.IsNull)
                                            ovrd.JournalPtid.Value = 0;
                                        ovrd.ActionType = XmActionType.New;
                                        Phoenix.FrameWork.CDS.DataService.Instance.AddObject(ovrd);
                                    }
                                }
                                //End #79314
                            }
                        }
                    }
                    //Begin #59572
                    else if (caller == "FraudOfac")
                    {
                        foreach (GbFraudLog ovrd in ovrdsArrays)
                        {
                            SetContextRow(contextRow);
                            contextRow++;

                            ovrd.SuperEmplId.Value = Convert.ToInt16(colSuperEmplId.UnFormattedValue);
                            //ovrd.SuperEmplName.Value = Convert.ToString(colOverridenby.UnFormattedValue);
                            if (colOvrdReason.UnFormattedValue != null && colOvrdReason.UnFormattedValue.ToString() != "")
                                ovrd.FalsePositiveId.Value = Convert.ToInt16(colOvrdReason.UnFormattedValue);
                            //Begin #79314
                            if (_isRemoteOvrdEnabled && _ovrdMessageInfoId > 0 && _msgInfo != null)
                            {
                                if (colAddlInfo.UnFormattedValue != null && colAddlInfo.UnFormattedValue.ToString() != "")
                                    ovrd.AddlInfo.Value = Convert.ToString(colAddlInfo.UnFormattedValue);
                                ovrd.BranchNo.Value = _tellerVars.BranchNo;
                                ovrd.DrawerNo.Value = _tellerVars.DrawerNo;
                                if (_msgInfo.MessageState == MessageState.Approved.ToString())
                                    ovrd.OvrdStatus.Value = 1;
                                else if (_msgInfo.MessageState == MessageState.Denied.ToString())
                                    ovrd.OvrdStatus.Value = 2;
                                else
                                    ovrd.OvrdStatus.Value = 0;
                                ovrd.MessageId.Value = _ovrdMessageInfoId;
                            }
                            //#End #79314
                        }

                    }
                    //End #59572
                    else
                    {
                        gridOverrideableErrors.SelectRow(contextRow, false);
                        if (colSuperEmplId.UnFormattedValue != null && colSuperEmplId.UnFormattedValue.ToString() != String.Empty)
                        {
                            OvrdBusObj.SuperEmplId.Value = Convert.ToInt16(colSuperEmplId.UnFormattedValue);
                            OvrdBusObj.SuperEmplName.Value = Convert.ToString(colOverridenby.UnFormattedValue);
                            //
                            if (_isRemoteOvrdEnabled && _ovrdMessageInfoId > 0 && _msgInfo != null)    //#79314
                            {
                                OvrdBusObj.BranchNo.Value = _tellerVars.BranchNo;
                                OvrdBusObj.DrawerNo.Value = _tellerVars.DrawerNo;
                                OvrdBusObj.TellerEmplId.Value = (short)GlobalVars.EmployeeId;
                                if (_msgInfo.MessageState == MessageState.Approved.ToString())
                                    OvrdBusObj.OvrdStatus.Value = 1;
                                else if (_msgInfo.MessageState == MessageState.Denied.ToString())
                                    OvrdBusObj.OvrdStatus.Value = 2;
                                else
                                    OvrdBusObj.OvrdStatus.Value = 0;
                                OvrdBusObj.MessageId.Value = _ovrdMessageInfoId;
                                if (_msgInfo.MessageState == MessageState.Denied.ToString() ||
                                    _msgInfo.MessageState == MessageState.Released.ToString())
                                {
                                    process = true;
                                    if (OvrdBusObj.JournalPtid.IsNull)
                                        OvrdBusObj.JournalPtid.Value = 0;
                                    OvrdBusObj.ActionType = XmActionType.New;
                                    Phoenix.FrameWork.CDS.DataService.Instance.AddObject(OvrdBusObj);
                                }
                            }
                        }
                    }
                    if (process)
                    {
                        Phoenix.FrameWork.CDS.DataService.Instance.ProcessRequest();
                        Phoenix.FrameWork.CDS.DataService.Instance.Reset();
                    }
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe);
            }
        }

		private bool SelectUpdateGridRows( string actionType, short superEmplId )
		{
			if ( actionType == "CheckSelectedRow" )	// Check if any row is selected
			{
				if ( gridOverrideableErrors.SelectedIndicies.Count > 0 )
					return true;
				else
					return false;
			}
			else if ( actionType == "CheckAllRowsOverridden" )	// Check if all rows have been overridden
			{
				for ( int i = 0; i < gridOverrideableErrors.Items.Count ; i++ )
				{
					SetContextRow( i );
					if (( colSuperEmplId.UnFormattedValue == null || colSuperEmplId.UnFormattedValue.ToString() == String.Empty )
							&& !_ovrdBusObj.IsWarning(Convert.ToInt32((colOvrdId.UnFormattedValue == null ? 0 : colOvrdId.UnFormattedValue))))	// #140786
						return false;
				}
				return true;
			}
			else if ( actionType == "SelectAllRows" )	// Check if all rows have been overridden
			{
				gridOverrideableErrors.SelectedIndicies.Clear();
				gridOverrideableErrors.Items.ClearSelection();
				for ( int i = 0; i < gridOverrideableErrors.Items.Count ; i++ )
				{
					gridOverrideableErrors.SelectedIndicies.Add( i );
					gridOverrideableErrors.SelectRow( i );
				}
				return true;
			}
			else if ( actionType == "SelectFailedOvrds" )	// Check if all rows have been overridden
			{
				gridOverrideableErrors.SelectedIndicies.Clear();
				gridOverrideableErrors.Items.ClearSelection();
				for ( int i = 0; i < gridOverrideableErrors.Items.Count ; i++ )
				{
					SetContextRow( i );
					//contextRow++;
					if ( colOverridenby.UnFormattedValue == null ||
						Convert.ToString( colOverridenby.UnFormattedValue ) == String.Empty  )
//						( colSuperEmplId.UnFormattedValue == null ||
//						Convert.ToString( colSuperEmplId.UnFormattedValue ) == String.Empty ))
					{
						gridOverrideableErrors.SelectedIndicies.Add( i );
						gridOverrideableErrors.SelectRow( i );
					}
				}
				if ( gridOverrideableErrors.SelectedIndicies.Count > 0 )
					return true;
				else
					return false;
			}
			else if ( actionType == "DeSelectAllRows" )	// Check if all rows have been overridden
			{
				gridOverrideableErrors.SelectedIndicies.Clear();
				gridOverrideableErrors.Items.ClearSelection();
				return true;
			}
			else if ( actionType == "CheckSelectedOneRow" )	// Check if any row is selected
			{
				if ( gridOverrideableErrors.SelectedIndicies.Count == 1 )
					return true;
				else
					return false;
			}
			else if ( actionType == "SelectFirstRow" )	// Check if any row is selected
			{
				gridOverrideableErrors.SelectedIndicies.Clear();
				gridOverrideableErrors.Items.ClearSelection();
				gridOverrideableErrors.SelectRow( 0 );
				return true;
			}
			else if ( actionType == "Update" && superEmplId >= 0 )
			{
				foreach( int selectedRow in gridOverrideableErrors.SelectedIndicies )
				{
					SetContextRow( selectedRow );
					colSuperEmplId.UnFormattedValue = superEmplId;
					colOverridenby.UnFormattedValue = GetSuperEmplName( superEmplId );
				}
				return true;
			}
			return false;
		}

		private void EnableDisableVisibleLogic( string origin )
		{
			pbViewFraud.Enabled = false;	//#79572
            pbViewDecision.Visible = (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled && caller == "JournalView");  //#14098
			if ( origin == "FormCreate" )
			{
				if ( caller == "JournalView" )
				{
					pbDisplay.Visible = false;
					pbDeselectAll.Visible = false;
					pbOverride.Visible = false;
					pbRepost.Visible = false;
					pbSelectAll.Visible = false;
					pbCancel.Visible = false;
                    EnableDisableVisibleLogic("RemoteOverride");    //#79314
                    //pbViewDecision.Visible = (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled); //#79314  //#14098
                    if (pbViewDecision.Visible)   //#13052
                        pbViewDecision.Enabled = _enableViewDecision;
				}
				else
				{
					pbClose.Visible = false;
					if ( gridOverrideableErrors.Count > 1 )
					{
						pbDeselectAll.Enabled = gridOverrideableErrors.SelectedIndicies.Count > 0;
						pbSelectAll.Enabled = gridOverrideableErrors.SelectedIndicies.Count != gridOverrideableErrors.Count;
					}
					else
					{
						pbDeselectAll.Enabled = false;
						pbSelectAll.Enabled = false;
					}
                    pbRepost.Enabled = SelectUpdateGridRows("CheckAllRowsOverridden", -1);
					//Begin #79510
					if (Workspace.Variables["IsAcquirerTran"] != null && Convert.ToBoolean(Workspace.Variables["IsAcquirerTran"]))
						pbCancel.Enabled = false;
					//End #79510
                    EnableDisableVisibleLogic("RemoteOverride");    //#79314
				}


			}
			else if ( origin == "ApproveClick" )
			{
                pbRepost.Enabled = SelectUpdateGridRows("CheckAllRowsOverridden", -1) && pbRepost.Visible;    //#79314;
			}
            else if (origin == "RemoteOverride")    //#79314
            {
                //Begin #79314
                if (_isRemoteOvrdEnabled)
                {
                    gbRemoteOverrideInformation.Visible = true;
                    gbInstructions.Visible = false;
                    //
                    rbAll.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    rbGroups.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    rbSupervisor.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    cmbSupervisorGroup.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    lblMessageInput.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    mulMessageLog.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Enable);
                    //
                    pbViewDecision.Visible = false;
                    if (IsWorkspaceReadOnly() && AppInfo.Instance.IsAppOnline)    //Enable view only mode for remote supervisor override purpose
                    {
                        pbRelease.Visible = true;
                        pbForward.Visible = true;
                        _isSupervisorOverrideActionPressed = false;
                        //pbApprove.Enabled = SelectUpdateGridRows("CheckAllRowsOverridden", -1);
                        pbDeny.Visible = (_tranSet == null || (_tranSet != null && !_tranSet.IsAcquirerTran));
                        pbPendingTran.Visible = (_tranInfo != null && (_tranInfo.OverrideType.Value == Convert.ToInt16(OverrideCategory.TranPosting) ||
                            _tranInfo.OverrideType.Value == Convert.ToInt16(OverrideCategory.SharedBranch) ||
                            _tranInfo.OverrideType.Value == Convert.ToInt16(OverrideCategory.TranPostingTellerFraud))); //#14101 - added TranPostingTellerFraud
                        pbJournal.Visible = (_tranInfo != null && _tranInfo.OverrideType.Value == Convert.ToInt16(OverrideCategory.Reversal));
                        pbPosition.Visible = (_tranInfo != null && _tranInfo.OverrideType.Value == Convert.ToInt16(OverrideCategory.CloseOut));
                        pbRepost.Visible = false;
                        pbCancel.Visible = false;
                        pbSend.Visible = false;
                        pbOverride.Visible = false;
                    }
                    else  //Teller view
                    {
                        pbRelease.Visible = false;
                        pbForward.Visible = false;
                        pbApprove.Visible = false;
                        pbDeny.Visible = false;
                        pbPendingTran.Visible = false;
                        //
                        pbSend.Visible = true;
                        //
                        pbJournal.Visible = false;
                        pbPosition.Visible = false;
                    }
                    this.UpdateView();
                }
                else
                {
                    gbInstructions.Visible = true;
                    gbRemoteOverrideInformation.Visible = false;
                    //
                    pbRelease.Visible = false;
                    pbForward.Visible = false;
                    pbApprove.Visible = false;
                    pbDeny.Visible = false;
                    pbPendingTran.Visible = false;
                    pbSend.Visible = false;
                    pbJournal.Visible = false;
                    pbPosition.Visible = false;
                }
                //End #79314
            }
			else if ( origin == "TableClick" )
			{
                pbOverride.Enabled = SelectUpdateGridRows("CheckSelectedRow", -1) && !_isOvrdApproved;  //#13070
				pbDisplay.Enabled = SelectUpdateGridRows( "CheckSelectedOneRow", -1 ) && Convert.ToString( colViewAccess.UnFormattedValue) == GlobalVars.Instance.ML.Y  &&
					Convert.ToString( colAcctType.UnFormattedValue ) != GlobalVars.Instance.ML.GL && Convert.ToString( colOvrdType.UnFormattedValue ) != DecodeOvrdType( "T" ) &&
					Convert.ToString( colOvrdType.UnFormattedValue ) != DecodeOvrdType( "O" );
				if ( gridOverrideableErrors.Count > 1 )
				{
					pbDeselectAll.Enabled = gridOverrideableErrors.SelectedIndicies.Count > 0;
					pbSelectAll.Enabled = gridOverrideableErrors.SelectedIndicies.Count != gridOverrideableErrors.Count;

				}
				//#79572, #9314
				//if(isNonCust)
				{
					 if(colFraudPtid.Text != "" && Convert.ToInt32(colFraudPtid.Text) > 0)
					 	pbViewFraud.Enabled = true;
					else
						pbViewFraud.Enabled = false;
				}
				//#79572

                //#80660
                if (colSuspectPtid.UnFormattedValue != null && !string.IsNullOrEmpty(colSuspectPtid.Text) && Convert.ToDecimal(colSuspectPtid.Text) > 0 && TellerVars.Instance.SuspiciousTransactionScoringAlertsCustomOption)
                    pbSuspectDtls.Enabled = true;
                else
                    pbSuspectDtls.Enabled = false;

			}
			else if ( origin == "SelectAllClick" )
			{
                pbOverride.Enabled = !_isOvrdApproved;  //#13070 - changed from hard coded true
				pbDeselectAll.Enabled = true;
				pbSelectAll.Enabled = false;
				pbDisplay.Enabled = false;
			}
			else if ( origin == "DeSelectAllClick" )
			{
				pbOverride.Enabled = false;
				pbDeselectAll.Enabled = false;
				pbSelectAll.Enabled = true;
				pbDisplay.Enabled = false;
			}
            else if (origin == "RouteOverrideClick")
            {
                if (_isRemoteOvrdEnabled)
                {
                    if (rbAll.Checked)
                        cmbSupervisorGroup.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Disable);
                    else if (rbGroups.Checked || rbSupervisor.Checked)
                        cmbSupervisorGroup.SetObjectStatus(NullabilityState.Null, VisibilityState.Default, EnableState.Enable);
                    //
                    if (!IsWorkspaceReadOnly())
                    {
                        if (rbAll.Checked)
                            pbSend.Enabled = true;
                        if ((rbGroups.Checked || rbSupervisor.Checked) && cmbSupervisorGroup.CodeValue != null)
                            pbSend.Enabled = true;
                    }
                }
            }
            else if (origin == "WarningOnly")   //#80660
            {
                _isRemoteOvrdEnabled = false;
                EnableDisableVisibleLogic("RemoteOverride");    //reset remote override settings if any
                pbOverride.Visible = false;
                pbRepost.Enabled = true;
            }
			//Begin #79572
            //#9958 - commented out else part of the following if clause, because it enables account display unconditionally for online with no security and/or offline.
			if (isNonCust)
			{
				pbDisplay.Enabled = false;
				//pbViewFraud.Enabled = true;
			}
			if(hasFraud)
			{
				colAddlInfo.Visible = true;
				//colFraudPtid.Visible = true;	#8765
				if(caller != "JournalView")	//#9313
				{
					colOvrdReason.Visible = true;
					colEnteredBy.Visible = true;
				}
				gridOverrideableErrors.Refresh();
				dfAddlInfo.SetObjectStatus(NullabilityState.Default, VisibilityState.Show, EnableState.Default);	//#9530
			}
			else
				dfAddlInfo.SetObjectStatus(NullabilityState.Default, VisibilityState.Hide, EnableState.Default);	//#9530
			//End #79572
		}

		private void SetContextRow( int rowNo )
		{
			bool itemSelected = gridOverrideableErrors.Items[rowNo].Selected;
			gridOverrideableErrors.ContextRow = rowNo;
			gridOverrideableErrors.Items[rowNo].Selected = itemSelected;
		}
		private void CallOtherForms( string origin )
		{
			string acctNo = null;
			string acctType = null;
			string depLoan = null;
			string depType = null;
			int rimNo = -1;
			int acctId = 0;
			PfwStandard tempWin = null;
            PfwStandard tempDlg = null;

			try
			{
				if ( origin == "Override" )
				{
					dlgOverride _dlgOverride = new dlgOverride();
					_dlgOverride.InitParameters( _superEmplId );
					dialogResult = _dlgOverride.ShowDialog(this);
				}
                else if (origin == "MessageInput")  //#79314
                {
                    tempDlg = Helper.CreateWindow("phoenix.client.remoteteller", "Phoenix.Client.RemoteTeller", "dlgRemoteMessageInput");
                    tempDlg.InitParameters(_currentMsg, false);
                }
                else if (origin == "PendingTran")  //#79314
                {
                    if (_tranSet != null && _tranSet.Transactions != null && _tranSet.Transactions.Count > 0)
                    {
                        tempWin = Helper.CreateWindow("Phoenix.Client.TlTrancode", "Phoenix.Windows.Client", "frmTlTranCode");
                        tempWin.InitParameters(null, null, null, string.Empty, null, _tranSet.Transactions);
                    }
                }
                else if (origin == "ViewDecision")  //#79314
                {
                    tempWin = Helper.CreateWindow("phoenix.client.remoteoverridedecision", "Phoenix.Client.RemoteOverrideDecision", "frmTlOverrideDecision");
                    tempWin.InitParameters(false, (colMessageId.UnFormattedValue != null ? Convert.ToInt32(colMessageId.UnFormattedValue) : -1), _effectiveDt.Value);   //#13993
                }
                else if (origin == "ViewJournal")
                {
                    if (_ovrdMessageInfoId != Int32.MinValue && _ovrdMessageInfoId > 0) //#12769
                    {
                        tempWin = CreateWindow("phoenix.client.tljournal", "Phoenix.Client.Journal", "frmTlJournal");
                        tempWin.InitParameters(_ovrdMessageInfoId);
                    }
                }
                else if (origin == "ViewSummaryPos")
                {
                    tempWin = CreateWindow("phoenix.client.tlposition", "Phoenix.Client.TlPosition", "frmTlPosition");
                    tempWin.InitParameters(4, _tranInfo.BranchNo.Value, _tranInfo.DrawerNo.Value, _tranInfo.TranEffectiveDt.Value, null, null, null, false);
                }
				else if ( origin == "DisplayClick" )
				{
					acctType = ( colAcctType.UnFormattedValue == null ? null :colAcctType.UnFormattedValue.ToString());
					acctNo = ( colAcctNo.UnFormattedValue == null ? null :colAcctNo.UnFormattedValue.ToString());
					depLoan = ( colDepLoan.UnFormattedValue == null ? null :colDepLoan.UnFormattedValue.ToString());
					depType = ( colDepType.UnFormattedValue == null ? null :colDepType.UnFormattedValue.ToString());
					if ( colAcctId.UnFormattedValue != null )
						acctId = Convert.ToInt32( colAcctId.UnFormattedValue );
					if ( colRimNo.UnFormattedValue != null )
						rimNo = Convert.ToInt32( colRimNo.UnFormattedValue );

					if ( depLoan == GlobalVars.Instance.ML.DP )
					{
						if ( depType == GlobalVars.Instance.ML.Tran )
						{
							tempWin = Helper.CreateWindow( "phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", "frmDpDisplayTran" );
							tempWin.InitParameters( acctType, acctNo );
						}
						else
						{
							tempWin = Helper.CreateWindow( "phoenix.client.dpdisplay", "Phoenix.Client.Account.Dp", "frmDpDisplayTime" );
							tempWin.InitParameters( acctType, acctNo );
						}
					}
					else if ( depLoan == GlobalVars.Instance.ML.LN )
					{
						tempWin = Helper.CreateWindow( "phoenix.client.lndisplay", "Phoenix.Client.Loan", "frmLnDisplay" );
						tempWin.InitParameters( acctType, acctNo );
					}
					else if ( depLoan == GlobalVars.Instance.ML.SD )
					{
						tempWin = Helper.CreateWindow( "phoenix.client.sddisplay", "Phoenix.Client.Sdb", "frmSdDisplay" );
						tempWin.InitParameters( acctType, acctNo,acctId );
					}
					else if ( depLoan == GlobalVars.Instance.ML.Ext || depLoan == "EX" )
					{
                        if (_gbHelper.IsExternalAdapterAcct(acctType))
                        {
                            //Launch XAML Window
                            ShowFormHelper.XAMLWindow(this, "ExAcctDisplay", acctType, acctNo, rimNo);
                        }
                        else
                        {
                            tempWin = Helper.CreateWindow("phoenix.client.exAcctdisplay", "Phoenix.Client.Ex", "frmOnExAcctDisp1");
                            tempWin.InitParameters(acctType, acctNo);
                        }
					}
					else if ( depLoan == GlobalVars.Instance.ML.RM )
					{
						tempWin = Helper.CreateWindow( "phoenix.client.rmdisplay", "Phoenix.Client", "frmRmDisplay" );
						tempWin.InitParameters( rimNo );
					}
				}
                else if (origin == "AcctSuspectDetails")    //#80660
                {
                    ArrayList tempArray = null;
                    ArrayList suspectArray= null;
                    decimal winXpPtid = decimal.MinValue;
                    decimal winSuspectPtid = decimal.MinValue;
                    string winAcctType = null;
                    string winAcctNo = null;

                    if (caller != "JournalView")
                    {
                        tempArray = _tranSet.GetSuspectOvrdDetails(Convert.ToDecimal(colSuspectPtid.UnFormattedValue));
                        // #17041 - Get current transaction....
                        foreach (TlTransaction tran in _tranSet.Transactions)
                        {
                            foreach (GbAcctSuspect tmpGbSuspect in tran.GbAcctSuspect)
                            {
                                if (tmpGbSuspect.Ptid.Value == Convert.ToDecimal(colSuspectPtid.UnFormattedValue))
                                    suspectArray = tran.GbAcctSuspect;
                            }
                        }
                        // #17041 - Commented suspectArray = _tranSet.CurTran.GbAcctSuspect;
                        winXpPtid = Convert.ToDecimal(colSuspectPtid.UnFormattedValue);
                    }
                    else
                    {
                        winSuspectPtid = Convert.ToDecimal(colSuspectPtid.UnFormattedValue);
                        if (colAcctType.UnFormattedValue == null && colAccount.UnFormattedValue != null)
                        {
                            winAcctType = (colAccount.UnFormattedValue.ToString().Substring(0,colAccount.UnFormattedValue.ToString().IndexOf('-')));
                            winAcctNo = (colAccount.UnFormattedValue.ToString().Substring(colAccount.UnFormattedValue.ToString().IndexOf('-') + 1, colAccount.UnFormattedValue.ToString().Length - colAccount.UnFormattedValue.ToString().IndexOf('-') - 1));
                        }
                        else if (colAcctType.UnFormattedValue != null)
                        {
                        winAcctType = (colAcctType.UnFormattedValue.ToString());
                        winAcctNo = (colAcctNo.UnFormattedValue.ToString());
                        }
                    }

                    tempWin = CreateWindow("Phoenix.Client.GbSuspectAlertControl", "Phoenix.Client.Global", "frmSuspiciousTranDetail");
                    tempWin.InitParameters(winXpPtid,
                                            decimal.MinValue,
                                            decimal.MinValue,
                                            winSuspectPtid,
                                            winAcctType,
                                            winAcctNo,
                                            tempArray,
                                            suspectArray);
                }
                if (tempWin != null)    //#79314
                {
                    tempWin.Workspace = this.Workspace;
                    tempWin.Show();
                }
                if (tempDlg != null)    //#79314
                {
                    tempDlg.Workspace = this.Workspace;
                    tempDlg.ShowDialog();
                }
			}
			catch( PhoenixException pe )
			{
				PMessageBox.Show( pe, MessageBoxButtons.OK );
				return;
			}
			catch( Exception e )
			{
				MessageBox.Show( e.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop );
				return;
			}
		}
		#endregion



		private void gridOverrideableErrors_RowClicked(object source, GridClickEventArgs e)
		{
			this.Focus();
			//System.Diagnostics.Trace.WriteLine( "Hello" );
			gridOverrideableErrors.Focus();
		}
		//Begin #79572
		private void LoadOvrdReasonCombo(string orList)
		{
			string[] tempList = null;

			if (orList != null && orList.Length > 0 && _orListLoaded == false)
			{
				tempList = orList.Split('^');
				foreach (string or in tempList)
				{
					string[] innerList = or.Split('~');
					colOvrdReason.Append(innerList[0], innerList[1]);
					_orListLoaded = true;
				}
			}
		}
		//End #79572

        //Begin #79314
        void pbPosition_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("ViewSummaryPos");
        }

        void pbJournal_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("ViewJournal");
        }

        void pbDeny_Click(object sender, PActionEventArgs e)
        {
            _isSupervisorOverrideActionPressed = true;
            _comm.UpdateMessageInfoStatus(_ovrdMessageInfoId, MessageState.Denied.ToString());
            LoadOverrides(true);    //Load overriden info back into the overrrid collection
            SendMessage(-1, -1, MessageState.Denied);
        }

        void pbApprove_Click(object sender, PActionEventArgs e)
        {
            _superEmplId = new PSmallInt("A1");
            _isRemoteOvrdEnabled = IsWorkspaceReadOnly();   //#79314
            CallOtherForms("Override");
            if (dialogResult == DialogResult.OK && !_superEmplId.IsNull)
            {
                SelectUpdateGridRows("Update", _superEmplId.Value);
                //EnableDisableVisibleLogic("ApproveClick");
                //
                _isSupervisorOverrideActionPressed = true;
                _comm.UpdateMessageInfoStatus(_ovrdMessageInfoId, MessageState.Approved.ToString());
                LoadOverrides(true);    //Load overriden info back into the overrrid collection
                SendMessage(-1, -1, MessageState.Approved);
            }
        }

        void pbForward_Click(object sender, PActionEventArgs e)
        {
            short superEmplId = -1;
            short groupCode = 0;
            ArrayList comboCache = new ArrayList();

            if ((rbSupervisor.Checked || rbGroups.Checked) && (cmbSupervisorGroup.CodeValue == null ||
                (cmbSupervisorGroup.CodeValue != null &&
                cmbSupervisorGroup.Value.ToString() == "-2")))
            {
                if (!_ovrdBusObj.RtoUsersList.IsNull && !string.IsNullOrEmpty(_ovrdBusObj.RtoUsersList.Value) && _ovrdBusObj.RtoUsersList.Value.Length > 0)
                    //13729 - No Supervisor available that has the appropriate limit(s).
                    PMessageBox.Show(13729, MessageType.Error, string.Empty);
                else
                    //13393 - Please check the override route information and make a valid selection in order to continue with this request.
                    PMessageBox.Show(13393, MessageType.Error, string.Empty);
                return;
            }
            else if (rbAll.Checked) //#13380 #17349 - added the missing  condition for All scenario for forward
            {
                GetAllRemoteSupervisors(true);
                comboCache = _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"];
                if (comboCache.Count < 2)
                {
                    if (!_ovrdBusObj.RtoUsersList.IsNull && !string.IsNullOrEmpty(_ovrdBusObj.RtoUsersList.Value) && _ovrdBusObj.RtoUsersList.Value.Length > 0)
                        //13729 - No Supervisor available that has the appropriate limit(s).
                        PMessageBox.Show(13729, MessageType.Error, string.Empty);
                    else
                        //13394 - No supervisor is listening for the selected override route option. Please check the override route information and make a valid selection in order to continue with this request.
                        PMessageBox.Show(13394, MessageType.Error, string.Empty);
                    return;
                }
            }
            if (rbSupervisor.Checked)
                superEmplId = Convert.ToInt16(cmbSupervisorGroup.CodeValue);
            if (rbGroups.Checked)
                groupCode = Convert.ToInt16(cmbSupervisorGroup.CodeValue);
            //
            _isSupervisorOverrideActionPressed = true;
            _comm.UpdateMessageInfoStatus(_ovrdMessageInfoId, MessageState.Forwarded.ToString());
            SendMessage(superEmplId, groupCode, MessageState.Forwarded);
        }

        void pbRelease_Click(object sender, PActionEventArgs e)
        {
            ReleaseClick(); //#13051
        }

        void pbViewDecision_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("ViewDecision");
        }

        void pbSuspectDtls_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("AcctSuspectDetails");
        }

        void pbPendingTran_Click(object sender, PActionEventArgs e)
        {
            CallOtherForms("PendingTran");
        }


        void lblMessageInput_Click(object sender, EventArgs e)
        {
            CallOtherForms("MessageInput");
        }

        protected override void OnClosing(CancelEventArgs e)    //#79314
        {
            if (!_isSupervisorOverrideActionPressed)
            {
                bool doNotClose = (_ovrdMessageInfoId > 0 && _comm != null &&
                _comm.GetMessageInfo(_ovrdMessageInfoId) != null);
                if (doNotClose)
                    doNotClose = PMessageBox.Show(13382, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, String.Empty) == DialogResult.No;

                if (doNotClose)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    ReleaseClick(); //#13051
                    closedWkspace = true;
                    base.OnClosing(e);
                    return;
                }
            }
            closedWkspace = true;
            base.OnClosing(e);
        }

        #region #13051
        void WorkSpace_Close(object sender, CancelEventArgs e)
        {
            if (!_isSupervisorOverrideActionPressed)
            {
                bool doNotClose = (_ovrdMessageInfoId > 0 && _comm != null &&
                _comm.GetMessageInfo(_ovrdMessageInfoId) != null);
                if (doNotClose)
                    doNotClose = PMessageBox.Show(13382, MessageType.Question, MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2, String.Empty) == DialogResult.No;

                if (doNotClose)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    ReleaseClick(); //#13051
                    closedWkspace = true;
                    base.OnClosing(e);
                    return;
                }
            }
            closedWkspace = true;
            base.OnClosing(e);
        }

        void dlgTlSupervisorOverride_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedWkspace && !e.Cancel)
            {
                WorkSpace_Close(sender, e);
                if (!e.Cancel)
                    closedWkspace = true;
            }
        }
        #endregion


        void pbSend_Click(object sender, PActionEventArgs e)
        {
            short superEmplId = -1;
            short groupCode = 0;
            _isOvrdApproved = false;
            ArrayList comboCache = new ArrayList();

            if (_ovrdMessageInfoId > 0)
            {
                if (_comm != null && _comm.GetMessageInfo(_ovrdMessageInfoId) != null)
                {
                    //Pending overrides exist.  Please resolve all pending overrides before submitting new one.
                    PMessageBox.Show(13342, MessageType.Warning, MessageBoxButtons.OK, String.Empty);
                    return;
                }
            }
            if ((rbSupervisor.Checked || rbGroups.Checked) &&
                (cmbSupervisorGroup.CodeValue == null ||
                (cmbSupervisorGroup.CodeValue != null &&
                cmbSupervisorGroup.Value.ToString() == "-2")))    //#13380
            {
                if (!_ovrdBusObj.RtoUsersList.IsNull && !string.IsNullOrEmpty(_ovrdBusObj.RtoUsersList.Value) && _ovrdBusObj.RtoUsersList.Value.Length > 0)
                    //13729 - No Supervisor available that has the appropriate limit(s).
                    PMessageBox.Show(13729, MessageType.Error, string.Empty);
                else
                    //13393 - Please check the override route information and make a valid selection in order to continue with this request.
                    PMessageBox.Show(13393, MessageType.Error, string.Empty);
                return;
            }
            else if (rbAll.Checked) //#13380
            {
                GetAllRemoteSupervisors(true);
                comboCache = _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"];
                if (comboCache.Count < 2)
                {
                    if (!_ovrdBusObj.RtoUsersList.IsNull && !string.IsNullOrEmpty(_ovrdBusObj.RtoUsersList.Value) && _ovrdBusObj.RtoUsersList.Value.Length > 0)
                        //13729 - No Supervisor available that has the appropriate limit(s).
                        PMessageBox.Show(13729, MessageType.Error, string.Empty);
                    else
                        //13394 - No supervisor is listening for the selected override route option. Please check the override route information and make a valid selection in order to continue with this request.
                        PMessageBox.Show(13394, MessageType.Error, string.Empty);
                    return;
                }
            }
            if (rbSupervisor.Checked)
                superEmplId = Convert.ToInt16(cmbSupervisorGroup.CodeValue);
            if (rbGroups.Checked)
                groupCode = Convert.ToInt16(cmbSupervisorGroup.CodeValue);
            //
            SendMessage(superEmplId, groupCode, MessageState.Pending);
        }

        #region Create New Message, Set Timer and Send
        private void CreateNewMessageInfo(short superEmplId, MessageState msgState)
        {
            int msgId = -1;

            #region create new message
            _msgInfo = new MessageInfo();
            #endregion
            //
            #region save message and get ID
            msgId = SaveOvrdTranInfo();
            if (msgId <= 0)
                return;
            #endregion
            //
            //Dispose any running timer
            if (_formTimer != null) //#15160
            {
                _formTimer.Stop();
                _formTimer.Dispose();
            }
            //
            #region load message info
            _ovrdMessageInfoId = msgId;
            _msgInfo.CallBackUrl = _comm.ListenUri;
            _msgInfo.TlrEmplId = GlobalVars.EmployeeId;
            _msgInfo.TlrName = GlobalVars.EmployeeName;
            _msgInfo.OvrdErrorMsg = _firstOvrdErrMsg;
            _msgInfo.MessageId = msgId;
            _msgInfo.Message = GetTellerCurrentMessage();
            _msgInfo.SupEmplId = superEmplId;
            _msgInfo.SupName = cmbSupervisorGroup.Description;
            _msgInfo.TimerInterval = _tellerVars.RespTimeout;    //global timeout
            _msgInfo.OrigTimerInterval = _tellerVars.RespTimeout;   //#15239
            _msgInfo.MessageState = msgState.ToString();
            _msgInfo.TlrWkspace = this.Workspace.Text;  //#13070
            _msgInfo.MessageStateUpdate += new MessageInfo.ReceiveMessageEventHandler(_msgInfo_MessageStateUpdate);
            #endregion
        }

        private void SendMessage(short superEmplId, short groupCode, MessageState msgState)
        {
            try
            {
                if (msgState != MessageState.Pending)
                {
                    #region get message info
                    _msgInfo = _comm.GetMessageInfo(_ovrdMessageInfoId);
                    #endregion
                    //
                    #region send message and close form
                    if (_msgInfo != null)
                    {
                        _msgInfo.MessageState = msgState.ToString();
                        _msgInfo.Message = _currentMsg;
                        if (_msgInfo.FwdTargetUrlList != null && _msgInfo.FwdTargetUrlList.Count > 0)
                            _comm.RemoveFromTargetFwdInfoList(_ovrdMessageInfoId, true);
                        if (!string.IsNullOrEmpty(_msgInfo.PriorityList))
                            _msgInfo.PriorityList = string.Empty;
                        if (msgState != MessageState.Forwarded || (msgState == MessageState.Forwarded && HandleOverrideRouteInfo(superEmplId, groupCode, _msgInfo)))    //#12867
                        {
                            #region update message state
                            RetrieveTranOvrdInfo(_ovrdMessageInfoId);   //#14265
                            UpdateOverrideTranInfo(_ovrdMessageInfoId, msgState);
                            #endregion
                            //
                            _comm.SendMessage(_msgInfo.CallBackUrl, _msgInfo);
                            WriteToDebugLog("dlgTlSupervisorOverride SendMessage  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + _msgInfo.MessageState + " ID: " + _msgInfo.MessageId.ToString());
                            //
                            _comm.RemoveMessageListItem(_msgInfo.MessageId);
                            //
                            this.Close();
                        }
                    }
                    #endregion
                }
                else if (msgState == MessageState.Pending)
                {
                    CreateNewMessageInfo(superEmplId, msgState);
                    if (HandleOverrideRouteInfo(superEmplId, groupCode, _msgInfo))
                        SendMsgAndRestartTimer(_msgInfo);
                    WriteToDebugLog("dlgTlSupervisorOverride SendMessage  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + _msgInfo.MessageState + " ID: " + _msgInfo.MessageId.ToString());
                    WriteToDebugLog("Pending message sent...");
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }

        }

        private bool HandleOverrideRouteInfo(short superEmplId, short groupCode, MessageInfo msgInfo)
        {
            CommunicatorUser tempUser = null;
            string targetUrl = string.Empty;
            msgInfo.TimerInterval = _tellerVars.RespTimeout;
            msgInfo.OrigTimerInterval = _tellerVars.RespTimeout; //#15239
            if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)
                _comm.RemoveFromTargetFwdInfoList(msgInfo.MessageId, false);

            #region resolve override route
            if (superEmplId == -1 || groupCode > 0)
            {
                ArrayList comboCache = new ArrayList();
                if (groupCode > 0)
                {
                    GetAllRemoteSupervisorsByGroup(true);
                    comboCache = _tellerVars.ComboCache["dlgTlSupervisorOverride.GroupRemoteSupervisors"];
                    //
                    _tmpOvrdGrp = new AdTlOvrdGrp();
                    _tmpOvrdGrp.GroupCode.Value = groupCode;    //#12906
                    _tmpOvrdGrp.ActionType = XmActionType.Select;
                    _tmpOvrdGrp.SelectAllFields = false;
                    _tmpOvrdGrp.ResTimeout.Selected = true;
                    _tmpOvrdGrp.SequentialGrp.Selected = true;
                    //
                    CallXMThruCDS("AdTlOvrdGrpSelect");
                    //
                    msgInfo.TimerInterval = (_tmpOvrdGrp.ResTimeout.IsNull ? _tellerVars.RespTimeout : _tmpOvrdGrp.ResTimeout.Value);
                    msgInfo.OrigTimerInterval = (_tmpOvrdGrp.ResTimeout.IsNull ? _tellerVars.RespTimeout : _tmpOvrdGrp.ResTimeout.Value); //#15239

                }
                else
                {
                    GetAllRemoteSupervisors(true);
                    comboCache = _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"];
                }
                if (rbAll.Checked && (!IsWorkspaceReadOnly() || msgInfo.MessageState == MessageState.Forwarded.ToString()) &&
                    comboCache.Count < 2)
                {
                    if (!_ovrdBusObj.RtoUsersList.IsNull && !string.IsNullOrEmpty(_ovrdBusObj.RtoUsersList.Value) && _ovrdBusObj.RtoUsersList.Value.Length > 0)
                        //13729 - No Supervisor available that has the appropriate limit(s).
                        PMessageBox.Show(13729, MessageType.Error, string.Empty);
                    else
                        //13394 - No supervisor is listening for the selected override route option. Please check the override route information and make a valid selection in order to continue with this request.
                        PMessageBox.Show(13394, MessageType.Error, string.Empty);
                    return false;
                }

                foreach (EnumValue eValue in comboCache)
                {
                    if (eValue.CodeValueInt > 0)   //exclude <None>
                    {
                        tempUser = GetCommunicatorUserInfo(eValue.CodeValueShort);

                        if (tempUser != null && !string.IsNullOrEmpty(tempUser.Url))
                        {
                            targetUrl = tempUser.Url;
                            //
                            if (msgInfo.MessageState != MessageState.Forwarded.ToString())  //#12867
                            {
                                msgInfo.SupEmplId = eValue.CodeValueShort;
                                msgInfo.SupName = eValue.Description;
                            }
                            //
                            if (msgInfo.MessageState == MessageState.Pending.ToString())
                                _comm.AddToTargetFwdInfoList(false, targetUrl, msgInfo.MessageId, eValue.CodeValueShort, eValue.Description, _currentMsg);
                            else if (msgInfo.MessageState == MessageState.Forwarded.ToString())
                            {
                                //TargetUrlInfo urlInfo = new TargetUrlInfo();  //#15143
                                if (eValue.CodeValueShort != GlobalVars.EmployeeId)
                                {
                                    _comm.AddToTargetFwdInfoList(true, targetUrl, msgInfo.MessageId, eValue.CodeValueShort, eValue.Description, _currentMsg);
                                    //
                                    WriteToDebugLog("dlgTlSupervisorOverride HandleOverrideRouteInfo  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + msgInfo.MessageState + " ID: " + msgInfo.MessageId.ToString());
                                    WriteToDebugLog(targetUrl + " Add message to forward list");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                tempUser = GetCommunicatorUserInfo(superEmplId);
                if (tempUser != null && !string.IsNullOrEmpty(tempUser.Url))
                {
                    targetUrl = tempUser.Url;
                    //_comm.AddToTargetUrlInfoList(targetUrl, msgInfo.MessageId);
                    if (msgInfo.MessageState == MessageState.Pending.ToString())
                        _comm.AddToTargetFwdInfoList(false, targetUrl, msgInfo.MessageId, msgInfo.SupEmplId, msgInfo.SupName, _currentMsg);
                    else if (msgInfo.MessageState == MessageState.Forwarded.ToString())
                    {
                        //_comm.AddToTargetFwdInfoList(true, targetUrl, msgInfo.MessageId, msgInfo.SupEmplId, msgInfo.SupName, _currentMsg);    //#14265
                        _comm.AddToTargetFwdInfoList(true, targetUrl, msgInfo.MessageId, superEmplId, cmbSupervisorGroup.Description, _currentMsg);   //#14265
                        //
                        WriteToDebugLog("dlgTlSupervisorOverride HandleOverrideRouteInfo  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + msgInfo.MessageState + " ID: " + msgInfo.MessageId.ToString());
                        WriteToDebugLog(targetUrl + " Add message to forward list");
                    }

                }
            }
            #endregion
            return true;
        }

        private void SendMsgAndRestartTimer(MessageInfo msgInfo)
        {
            string prevPriorityList = msgInfo.PriorityList;
            bool isSeqGroup = (rbGroups.Checked && !_tmpOvrdGrp.SequentialGrp.IsNull &&
                _tmpOvrdGrp.SequentialGrp.Value == GlobalVars.Instance.ML.Y);    //#15239
            #region remove existing messages if any from the list
            if (_comm.MessageList != null && _comm.MessageList.Count > 0)
            {
                foreach (MessageInfo msg in _comm.MessageList)
                {
                    if (msg.CallBackUrl == msgInfo.CallBackUrl && string.IsNullOrEmpty(msgInfo.PriorityList) && (msg.MessageState == MessageState.Pending.ToString() ||
                        msg.MessageState == MessageState.TimeOut.ToString() ||
                        msg.MessageState == MessageState.Released.ToString() ||
                        msg.MessageState == MessageState.Denied.ToString() ||
                        msg.MessageState == MessageState.Ignore.ToString()))        //#12906 #14265 - added forwarded #15260 - removed forwarded from here
                    {
                        _comm.RemoveMessageListItem(msg.MessageId);
                        break;
                    }
                }
            }
            #endregion
            //
            #region start the response timer
            if ((msgInfo.MessageState == MessageState.Pending.ToString() ||
                msgInfo.MessageState == MessageState.Forwarded.ToString()))  //#13826
            {
                msgInfo.TimerExpired = false;
                if (_formTimer == null) //#13826
                {
                    _formTimer = new Timer();
                    _formTimer.Tick += new EventHandler(_formTimer_Tick);
                }
                if (msgInfo.MessageState == MessageState.Pending.ToString())    //#15160
                    msgInfo.MessageStateUpdate += new MessageInfo.ReceiveMessageEventHandler(_msgInfo_MessageStateUpdate);
                if (msgInfo.MessageState == MessageState.Forwarded.ToString())  //#14265
                {
                    msgInfo.MessageState = MessageState.Pending.ToString();
                    msgInfo.AcceptedFirstUrl = string.Empty;
                    msgInfo.ForwardToUrl = string.Empty;
                    msgInfo.ForwardSupEmplId = 0;
                    msgInfo.FwdTargetUrlList = null;
                    msgInfo.TimerInterval = _tellerVars.RespTimeout;
                    msgInfo.OrigTimerInterval = _tellerVars.RespTimeout;    //#15239
                    msgInfo.PriorityList = string.Empty;     //#15239
                    RetrieveTranOvrdInfo(msgInfo.MessageId);    //#14265
                    UpdateOverrideTranInfo(msgInfo.MessageId, MessageState.Pending);
                }
                else
                {
                    msgInfo.TimerInterval = msgInfo.OrigTimerInterval;
                    #region add the current message to the list
                    if (string.IsNullOrEmpty(msgInfo.PriorityList))    //#15160
                        _comm.MessageList.Add(msgInfo);
                    #endregion
                }
                //
                //WriteToDebugLog("dlgTlSupervisorOverride SendMsgAndRestartTimer  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + msgInfo.MessageState + " ID: " + msgInfo.MessageId.ToString());
                //WriteToDebugLog("Timer Interval: " + msgInfo.TimerInterval.ToString());
            }
            #endregion
            //
            #region send message(s)
            if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)
            {
                _formTimer.Interval = 1000;
                _formTimer.Enabled = true;  //#15143
                _formTimer.Start(); //#15143
                foreach (TargetUrlInfo urlInfo in _comm.TargetUrlList)
                {
                    if (urlInfo.MessageId == msgInfo.MessageId)
                    {
                        if (string.IsNullOrEmpty(msgInfo.PriorityList) || (!string.IsNullOrEmpty(msgInfo.PriorityList) &&
                            !msgInfo.PriorityList.Contains(urlInfo.TargetUrl)))
                        {
                            //Begin #12867
                            msgInfo.SupEmplId = urlInfo.SupEmplId;
                            msgInfo.SupName = urlInfo.SupName;
                            msgInfo.Message = urlInfo.Message;
                            //End #12867
                            if (isSeqGroup)
                            {
                                //WriteToDebugLog("dlgTlSupervisorOverride SendMsgAndRestartTimer  " + System.DateTime.Now.ToLongTimeString() + ": " + "State " + msgInfo.MessageState + " ID: " + msgInfo.MessageId.ToString());
                                WriteToDebugLog("Group Priority Used...");
                                msgInfo.PriorityList = (string.IsNullOrEmpty(msgInfo.PriorityList) ? urlInfo.TargetUrl : msgInfo.PriorityList + "~" + urlInfo.TargetUrl);
                            }
                            _comm.SendMessage(urlInfo.TargetUrl, msgInfo);
                            if (!string.IsNullOrEmpty(msgInfo.PriorityList))
                                break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(prevPriorityList) && prevPriorityList == msgInfo.PriorityList)
                    msgInfo.PriorityList = string.Empty;
            }
            #endregion
        }
        #endregion

        #region Handle Override Route Info
        private void GetAllRemoteSupervisors(bool forceFetch)
        {
            if (_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"] == null || forceFetch)
            {
                if (_tranInfo != null && !_tranInfo.TellerEmplId.IsNull)
                    _ovrdBusObj.TellerEmplId.Value = _tranInfo.TellerEmplId.Value;
                if (_ovrdBusObj.RtoUsersList.IsNull)    //#16877
                {
                    _ovrdBusObj.GetRTOUsersList();
                    ValidateRTOUsersLimit();
                }
                _ovrdBusObj.ResponseTypeId = 10;
                CallXMThruCDS("EmployeePopulate");
                //
                _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"] = OvrdBusObj.SuperEmplId.Constraint.EnumValues;
            }
        }

        private void GetAllRemoteSupervisorsByGroup(bool forceFetch)
        {
            if (_tellerVars.ComboCache["dlgTlSupervisorOverride.GroupRemoteSupervisors"] == null || forceFetch)
            {
                if (_tranInfo != null && !_tranInfo.TellerEmplId.IsNull)
                    _ovrdBusObj.TellerEmplId.Value = _tranInfo.TellerEmplId.Value;
                if (cmbSupervisorGroup.CodeValue != null)
                {
                    if (_ovrdBusObj.RtoUsersList.IsNull)    //#16877
                    {
                        _ovrdBusObj.GetRTOUsersList();
                        ValidateRTOUsersLimit();
                    }

                    _ovrdBusObj.ResponseTypeId = 11;
                    _ovrdBusObj.OvrdGroupCode.Value = Convert.ToInt32(cmbSupervisorGroup.CodeValue);
                    CallXMThruCDS("EmployeePopulate");
                    //
                    _tellerVars.ComboCache["dlgTlSupervisorOverride.GroupRemoteSupervisors"] = OvrdBusObj.SuperEmplId.Constraint.EnumValues;
                }
            }
        }

        private void GetAllRemoteSupervisorGroups(bool forceFetch)
        {
            if (_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"] == null || forceFetch)
            {
                if (_tranInfo != null && !_tranInfo.TellerEmplId.IsNull)
                    _ovrdBusObj.TellerEmplId.Value = _tranInfo.TellerEmplId.Value;
                if (_ovrdBusObj.RtoUsersList.IsNull)    //#16877
                {
                    _ovrdBusObj.GetRTOUsersList();
                    ValidateRTOUsersLimit();
                }
                _ovrdBusObj.ResponseTypeId = 10;
                CallXMThruCDS("RemoteSupervisorGroup");
                //
                _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"] = OvrdBusObj.OvrdGroupCode.Constraint.EnumValues;
            }
        }

        private void PopulateOverrideCombo()
        {
            bool defaultGrpFound = true;
            EnableDisableVisibleLogic("RouteOverrideClick");
            this.cmbSupervisorGroup.DisplayType = UIComboDisplayType.Description;
            //
            if (rbGroups.Checked)
            {
                GetAllRemoteSupervisorGroups(true);
                if (_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"] != null)
                {
                    this.cmbSupervisorGroup.Populate(_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"]);
                    if (!_adGbRsm.RemoteOvrdGroup.IsNull)
                    {
                        if (!IsWorkspaceReadOnly())
                            this.cmbSupervisorGroup.DefaultCodeValue = _adGbRsm.RemoteOvrdGroup.Value;
                        else
                        {
                            if ((_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"] as ArrayList).Contains(_adGbRsm.RemoteOvrdGroup.Value))
                                this.cmbSupervisorGroup.DefaultCodeValue = _adGbRsm.RemoteOvrdGroup.Value;
                            else
                                defaultGrpFound = false;
                        }
                    }
                    else   //#12837
                        defaultGrpFound = false;

                    if (!defaultGrpFound)
                    {
                        foreach (EnumValue eValue in _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisorGroups"])
                        {
                            if (eValue.CodeValue != null)
                            {
                                this.cmbSupervisorGroup.DefaultCodeValue = eValue.CodeValueInt;
                                break;
                            }
                        }
                    }
                    this.cmbSupervisorGroup.InitialDisplayType = UIComboInitialDisplayType.DisplayDefault;
                    this.cmbSupervisorGroup.SetDefaultValue();
                }
            }
            else if (rbSupervisor.Checked)
            {
                GetAllRemoteSupervisors(true);
                if (_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"] != null)
                {
                    this.cmbSupervisorGroup.Populate(_tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"]);
                    this.cmbSupervisorGroup.DisplayType = UIComboDisplayType.Description;   //#12837
                    foreach (EnumValue eValue in _tellerVars.ComboCache["dlgTlSupervisorOverride.AllRemoteSupervisors"])   //#12837
                    {
                        if (eValue.CodeValue != null)
                        {
                            this.cmbSupervisorGroup.DefaultCodeValue = eValue.CodeValue;
                            break;
                        }
                    }
                    this.cmbSupervisorGroup.InitialDisplayType = UIComboInitialDisplayType.DisplayDefault;
                    this.cmbSupervisorGroup.SetDefaultValue();
                }
            }
            else
            {
                GetAllRemoteSupervisors(false); //#17349
                this.cmbSupervisorGroup.InitialDisplayType = UIComboInitialDisplayType.DisplayBlank;    //#12837
                this.cmbSupervisorGroup.SetValue(null); //#12837
                cmbSupervisorGroup.Repopulate(true);
            }
        }

        void rbGroups_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            PopulateOverrideCombo();
        }

        void rbSupervisor_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            PopulateOverrideCombo();
        }


        void rbAll_PhoenixUICheckedChangedEvent(object sender, PEventArgs e)
        {
            PopulateOverrideCombo();
        }
        #endregion

        #region Load Override Error Info From TranInfo
        private void LoadOverridesFromTranOvrdInfo(int tranInfoId, TlTransactionSet tmpTranSet, bool extractTranSet, bool swapOverrides)
        {
            if (!AppInfo.Instance.IsAppOnline)
                return;
            RetrieveTranOvrdInfo(tranInfoId);
            //
            if (!_tranInfo.OverrideType.IsNull)
            {
                if (_tranInfo.OverrideType.Value == (short)OverrideCategory.TranPosting ||
                    _tranInfo.OverrideType.Value == (short)OverrideCategory.TranPostingTellerFraud) //#14101 - added TranPostingTellerFraud
                {
                    caller = "TranCodePost";
                    if (_tranInfo.OverrideType.Value == (short)OverrideCategory.TranPostingTellerFraud) //#14101 - added
                        hasFraud = true;
                }
                else if (_tranInfo.OverrideType.Value == (short)OverrideCategory.Reversal)
                    caller = "JournalRev";
                else if (_tranInfo.OverrideType.Value == (short)OverrideCategory.SharedBranch)
                    caller = "SharedBranch";
                else if (_tranInfo.OverrideType.Value == (short)OverrideCategory.CloseOut)
                    caller = "Position";
                else if (_tranInfo.OverrideType.Value == (short)OverrideCategory.TranCodeCTR)
                    caller = "TranCodeCTR";
                else if (_tranInfo.OverrideType.Value == (short)OverrideCategory.NonCust)
                    caller = "FraudOfac";
                //
                if (extractTranSet)
                {
                    if (!_tranInfo.TransetInfo.IsNull)
                    {
                        if (tmpTranSet != null && caller == "TranCodePost")
                        {
                            tmpTranSet.LoadStructureFromXML(_tranInfo.TransetInfo.Value);
                            if (swapOverrides)
                                _tranSet.SwapOvrdsCollectionList(tmpTranSet.GetTranSetOvrdsCollection());
                        }
                        //
                        if (ovrdsArrays != null && ovrdsArrays.Count > 0)
                            ovrdsArrays.RemoveRange(0, ovrdsArrays.Count);
                        if (caller == "TranCodePost" || caller == "SharedBranch")
                            ovrdsArrays = _tranSet.GetTranSetOvrdsCollection();
                        else
                        {
                            if (caller == "FraudOfac")
                            {
                                ovrdsArrays = _gbFraudLog.LoadStructureFromXML(_tranInfo.TransetInfo.Value, true);
                                hasFraud = true;
                                isNonCust = true;
                            }
                            else
                            {
                                if (OvrdBusObj != null)
                                    OvrdBusObj.LoadStructureFromXML(_tranInfo.TransetInfo.Value, false);
                                else
                                    ovrdsArrays = OvrdBusObj.LoadStructureFromXML(_tranInfo.TransetInfo.Value, true);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Select Message State From TranInfo
        private void RetrieveTranOvrdInfo(int tranInfoId)
        {
            if (!AppInfo.Instance.IsAppOnline)
                return;

            _tranInfo = new TlOvrdTranInfo();
            _tranInfo.ActionType = XmActionType.Select;
            _tranInfo.Ptid.Value = tranInfoId;
            //
            CoreService.DataService.ProcessRequest(_tranInfo);

        }
        #endregion

        #region Save Message Into TranInfo
        private int SaveOvrdTranInfo()
        {
            if (!AppInfo.Instance.IsAppOnline)
                return -1;
            OverrideCategory ovrdCategory = 0;
            _ovrdMessageInfoId = -1;
            //
            try
            {
                switch (caller)
                {
                    case "TranCodePost":
                        ovrdCategory = OverrideCategory.TranPosting;
                        if (_tranSet != null && _tranSet.IsAcquirerTran)
                            ovrdCategory = OverrideCategory.SharedBranch;
                        if (hasFraud)
                            ovrdCategory = OverrideCategory.TranPostingTellerFraud;
                        break;
                    case "JournalRev":
                        ovrdCategory = OverrideCategory.Reversal;
                        break;
                    case "Position":
                        ovrdCategory = OverrideCategory.CloseOut;
                        break;
                    case "FraudOfac":
                        if (isNonCust)
                            ovrdCategory = OverrideCategory.NonCust;
                        break;
                    case "TranCodeCTR":
                        ovrdCategory = OverrideCategory.TranCodeCTR;
                        break;
                }
                if (_tranSet == null)
                    return _ovrdMessageInfoId;
                _tranInfo = new TlOvrdTranInfo();
                _tranInfo.MessageState.Value = MessageState.Pending.ToString();
                _tranInfo.OverrideType.Value = Convert.ToInt16(ovrdCategory);
                _tranInfo.TlrSupChatInfo.Value = mulMessageLog.Text;
                _tranInfo.OvrdMessage2.Value = _currentMsg;
                if (ovrdCategory == OverrideCategory.TranPosting || ovrdCategory == OverrideCategory.TranPostingTellerFraud ||
                    ovrdCategory == OverrideCategory.NonCust || ovrdCategory == OverrideCategory.TranCodeCTR ||
                    ovrdCategory == OverrideCategory.SharedBranch || ovrdCategory == OverrideCategory.Reversal ||
                    ovrdCategory == OverrideCategory.CloseOut) //#12769 - added reversal
                {
                    if (ovrdCategory == OverrideCategory.TranPosting ||
                    ovrdCategory == OverrideCategory.SharedBranch ||
                    ovrdCategory == OverrideCategory.TranPostingTellerFraud)    //#14101 - added TranPostingTellerFraud
                        _tranInfo.TransetInfo.Value = (_tranSet as TlTransactionSet).LoadStructureToXML(false);
                    else
                    {
                        if (ovrdCategory == OverrideCategory.NonCust)
                        {
                            _tranInfo.TransetInfo.Value = _gbFraudLog.LoadStructureToXML(ovrdsArrays);
                        }
                        else
                        {
                            if (ovrdsArrays != null && ovrdsArrays.Count > 0)
                                _tranInfo.TransetInfo.Value = OvrdBusObj.LoadStructureToXML(ovrdsArrays);
                            else
                                _tranInfo.TransetInfo.Value = OvrdBusObj.LoadStructureToXML(null);

                        }
                        if (ovrdCategory == OverrideCategory.NonCust)
                        {
                            _tranInfo.BranchNo.Value = _tranSet.BranchNo.Value;
                            _tranInfo.DrawerNo.Value = _tranSet.DrawerNo.Value;
                            if (ovrdsArrays != null && ovrdsArrays.Count == 1)
                                _tranInfo.FraudPtid.Value = (ovrdsArrays[0] as GbFraudLog).FraudPtid.Value;
                        }
                    }
                }
                if (ovrdCategory == OverrideCategory.Reversal || ovrdCategory == OverrideCategory.CloseOut) //#12769
                {
                    _tranInfo.BranchNo.Value = _branchNo.Value;
                    _tranInfo.DrawerNo.Value = _drawerNo.Value;
                    _tranInfo.TranEffectiveDt.Value = _effectiveDt.Value;
                    _tranInfo.EffectiveDt.Value = _effectiveDt.Value;
                    if (!_sequenceNo.IsNull)
                        _tranInfo.SequenceNo.Value = _sequenceNo.Value;
                }
                else
                {
                    _tranInfo.TranEffectiveDt.Value = _tellerVars.PostingDt;
                    _tranInfo.EffectiveDt.Value = _tellerVars.PostingDt;    //#15910
                }
                //
                _ovrdMessageInfoId = _tranInfo.RegisterTranInfo();
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }
            return _ovrdMessageInfoId;
        }
        #endregion

        #region Update Message State in TranInfo
        private void UpdateOverrideTranInfo(int tranInfoId, MessageState msgState)
        {
            bool updateTranInfo = false;    //#14080
            try
            {
                if (!AppInfo.Instance.IsAppOnline)
                    return;

                if (tranInfoId == Int32.MinValue || tranInfoId <= 0)
                    return;
                if (_tranInfo.MessageState.IsNull || (!_tranInfo.MessageState.IsNull &&
                    _tranInfo.MessageState.Value != msgState.ToString()))
                {
                    _tranInfo.MessageState.Value = msgState.ToString();
                    updateTranInfo = true;
                }
                if ((_currentMsg != null && !string.IsNullOrEmpty(_currentMsg)) ||
                    (mulMessageLog != null && !string.IsNullOrEmpty(mulMessageLog.Text))) //#14080
                {
                    if (mulMessageLog != null && !string.IsNullOrEmpty(mulMessageLog.Text))
                        _tranInfo.TlrSupChatInfo.Value = mulMessageLog.Text;
                    if (_currentMsg != null && !string.IsNullOrEmpty(_currentMsg))
                        _tranInfo.OvrdMessage1.Value = _currentMsg;
                    updateTranInfo = true;
                }
                if (msgState == MessageState.Approved)
                {
                    updateTranInfo = true;
                    if (caller == "TranCodePost")
                        _tranInfo.TransetInfo.Value = (_tranSet as TlTransactionSet).LoadStructureToXML(false);
                    else
                    {
                        if (caller == "FraudOfac")
                        {
                            _tranInfo.TransetInfo.Value = _gbFraudLog.LoadStructureToXML(ovrdsArrays);
                        }
                        else
                        {
                            if (OvrdBusObj != null)
                                _tranInfo.TransetInfo.Value = OvrdBusObj.LoadStructureToXML(null);
                            else
                                _tranInfo.TransetInfo.Value = OvrdBusObj.LoadStructureToXML(ovrdsArrays);
                        }
                    }
                }
                if (updateTranInfo) //#14080
                {
                    _tranInfo.ActionType = XmActionType.Custom;
                    _tranInfo.CustomActionName = "UpdateTranInfo";
                    _tranInfo.Ptid.Value = tranInfoId;
                    if (!_tranInfo.Ptid.IsNull)
                    {
                        Phoenix.FrameWork.CDS.DataService.Instance.ProcessCustomAction(_tranInfo, "UpdateTranInfo");
                    }
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }
        }
        #endregion

        #region Misc Helper Methods
        private bool IsRemoteOverrideEnabled()
        {
            if (AppInfo.Instance.IsAppOnline && _tellerVars.IsRemoteOverrideEnabled && caller != "JournalView")
                return true;

            return false;
        }

        private void RefreshUserList()
        {
            _commUserlist.Clear();
            List<CommunicatorUser> list = _comm.Registry.GetUsers();
            if (list != null && list.Count > 0)
                _commUserlist.AddRange(list);
        }

        private bool IsWorkspaceReadOnly()
        {
            return Phoenix.Windows.Client.Helper.IsWorkspaceReadOnly(this.Workspace);
        }

        private void ReleaseClick() //#13051
        {
            _isSupervisorOverrideActionPressed = true;
            _comm.UpdateMessageInfoStatus(_ovrdMessageInfoId, MessageState.Released.ToString());
            LoadOverrides(true);    //Load overriden info back into the overrrid collection
            SendMessage(-1, -1, MessageState.Released);
        }

        private string GetTellerCurrentMessage()
        {
            return _currentMsg;
        }

        private void WriteToDebugLog(string logInput)
        {
            if (CoreService.LogPublisher.IsLogEnabled)
            {
                if (!string.IsNullOrEmpty(logInput))
                {
                    logInput = logInput.Replace("<", "");
                    logInput = logInput.Replace(">", "");
                    logInput = logInput.Replace("&", "");
                    logInput = logInput.Replace(".", "");
                    logInput = logInput.Replace("/", "");
                }
                CoreService.LogPublisher.LogDebug(logInput);
                System.Diagnostics.Trace.WriteLine(logInput);
            }
        }
        #endregion

        #region Handle Message Queue
        public void _msgInfo_MessageStateUpdate(object sender, MessageEventArgs e)
        {
            MessageInfo msgInfo;
            try
            {
                if (!IsWorkspaceReadOnly() &&
                    e.Message.MessageId > 0 && _tranSet != null)   //#79314
                {
                    //msgInfo = e.Message;
                    msgInfo = _comm.GetMessageInfo(e.Message.MessageId);
                    //
                    if (msgInfo != null && msgInfo.MessageState == MessageState.Accepted.ToString())    //#15160
                    {
                        if (_formTimer != null)
                        {
                            _formTimer.Stop();
                            _formTimer.Dispose();
                        }
                    }
                    if (msgInfo != null && (msgInfo.MessageState == MessageState.Released.ToString() ||
                        msgInfo.MessageState == MessageState.Denied.ToString() ||
                        msgInfo.MessageState == MessageState.TimeOut.ToString()))    //#15332
                    {
                        if (_comm.MessageList != null && _comm.MessageList.Count > 0)
                            _comm.RemoveMessageListItem(msgInfo.MessageId); //Remove the item from the list
                        if (_queueTimer != null)
                            _queueTimer.Enabled = false;
                        if (!_iconRemoteQ.Visible)
                            _iconRemoteQ.Visible = true;
                    }
                    if (msgInfo != null && msgInfo.MessageState == MessageState.Approved.ToString())    //#15087
                    {
                        TlTransactionSet tmpTranSet = new TlTransactionSet();
                        LoadOverridesFromTranOvrdInfo(msgInfo.MessageId, tmpTranSet, true, true);
                        //
                        if (ovrdsArrays != null || OvrdBusObj != null)
                        {
                            LoadOverrides(false);
                            if (_tranInfo != null && !_tranInfo.TlrSupChatInfo.IsNull)
                            {
                                mulMessageLog.Text = _tranInfo.TlrSupChatInfo.Value.Replace("\n", "\r\n");
                                _origMsgLogMessage = mulMessageLog.Text;
                            }
                            //
                            if (hasFraud)
                                LoadOvrdReasonCombo(OvrdBusObj.GetFalsePositiveId());
                            //end #9032
                            pbDeselectAll_Click(pbDeselectAll, null);
                            if (!SelectUpdateGridRows("SelectFailedOvrds", -1))
                            {
                                //SelectUpdateGridRows( "SelectFirstRow", -1 );
                                pbSelectAll_Click(pbSelectAll, null);
                            }
                            //
                            SelectUpdateGridRows("Update", Convert.ToInt16(msgInfo.SupEmplId));
                            EnableDisableVisibleLogic("ApproveClick");
                        }
                        //
                        if (msgInfo.MessageState == MessageState.Approved.ToString())
                        {
                            this.pbSend.Enabled = false;
                            this.pbOverride.Enabled = false;
                            _isOvrdApproved = true; //#13070
                            this.pbDeselectAll.Enabled = false;
                            this.gbRemoteOverrideInformation.Enabled = false;
                        }
                        if (!gridOverrideableErrors.Focused)    //#13070
                        {
                            gridOverrideableErrors.Focus();
                            if (gridOverrideableErrors.Count > 0)
                                gridOverrideableErrors.SelectRow(0, true);
                        }
                        if (_comm.MessageList != null && _comm.MessageList.Count > 0)
                        {
                            _comm.RemoveMessageListItem(msgInfo.MessageId); //Remove the item from the list
                        }
                    }
                    //
                    if (msgInfo != null && msgInfo.MessageState == MessageState.Forwarded.ToString())   //#15087
                    {
                        if (msgInfo.FwdTargetUrlList != null && msgInfo.FwdTargetUrlList.Count > 0)
                        {
                            //msgInfo.MessageState = MessageState.Pending.ToString();   //#14265
                            _comm.RemoveFromTargetFwdInfoList(msgInfo.MessageId, false);    //#15160
                            foreach (TargetUrlInfo urlInfo in msgInfo.FwdTargetUrlList)
                            {
                                if (urlInfo.MessageId == msgInfo.MessageId)
                                {
                                    _comm.AddToTargetFwdInfoList(false, urlInfo.TargetUrl, urlInfo.MessageId, urlInfo.SupEmplId, urlInfo.SupName, urlInfo.Message);
                                }
                            }
                            //
                            //UpdateOverrideTranInfo(msgInfo.MessageId, MessageState.Pending);  //#14265
                            //Dispose any running timer
                            if (_formTimer != null) //#15160
                            {
                                _formTimer.Stop();
                                _formTimer.Dispose();
                            }
                            SendMsgAndRestartTimer(msgInfo);
                        }
                        //
                        if (msgInfo.FwdTargetUrlList != null && msgInfo.FwdTargetUrlList.Count > 0)
                            _comm.RemoveFromTargetFwdInfoList(msgInfo.MessageId, true);
                    }
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
            }

        }
        #endregion

        #region Send Ignore Message
        private void SendIgnoreMessage()
        {
            bool sentIgnoreMsg = true; //#12769
            bool isTimeOutMsgUpdated = false;
            if (_comm.MessageList != null && _comm.MessageList.Count > 0)
            {
                _msgInfo = _comm.GetMessageInfo(_ovrdMessageInfoId);
                if (_msgInfo != null)
                {
                    _msgInfo.MessageState = MessageState.Ignore.ToString();
                    WriteToDebugLog(_msgInfo.MessageId.ToString());
                    WriteToDebugLog(_msgInfo.MessageState);

                    if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)
                    {
                        foreach (TargetUrlInfo urlInfo in _comm.TargetUrlList)
                        {
                            if (urlInfo.MessageId == _msgInfo.MessageId)
                            {
                                if (!isTimeOutMsgUpdated)
                                {
                                    _comm.UpdateMessageInfoStatus(_msgInfo.MessageId, MessageState.Ignore.ToString());
                                    isTimeOutMsgUpdated = true;
                                }
                                if (!string.IsNullOrEmpty(_msgInfo.PriorityList))   //#1560
                                {
                                    if (urlInfo.SupEmplId == _msgInfo.SupEmplId)
                                    {
                                        _comm.SendMessage(urlInfo.TargetUrl, _msgInfo);
                                        break;
                                    }
                                }
                                else
                                {
                                    _comm.SendMessage(urlInfo.TargetUrl, _msgInfo);
                                }
                            }
                        }
                    }
                    if (sentIgnoreMsg)  //#12769
                    {
                        RetrieveTranOvrdInfo(_msgInfo.MessageId);   //#14265
                        UpdateOverrideTranInfo(_msgInfo.MessageId, MessageState.Ignore); //#15239 - Changed from Time Out to Ignore
                        if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)
                            _comm.RemoveFromTargetFwdInfoList(_msgInfo.MessageId, false);
                        //
                        if (_comm.MessageList != null && _comm.MessageList.Count > 0)
                            _comm.RemoveMessageListItem(_msgInfo.MessageId);
                        //
                        #region notify teller about the timeout
                        NotifyTeller(MessageState.Ignore);
                        #endregion
                    }
                }
            }
        }
        #endregion

        #region Send Time Out Message
        private void SendTimeOutMessage()
        {
            bool sentTimeOutMsg = false;
            bool isSeqGroup = (rbGroups.Checked && !_tmpOvrdGrp.SequentialGrp.IsNull &&
                _tmpOvrdGrp.SequentialGrp.Value == GlobalVars.Instance.ML.Y);    //#15239
            string seqTargetUrl = string.Empty;
            string seqGrpUrlList = string.Empty;    //#15239
            bool isTimeOutMsgUpdated = false;
            bool clearSeqMsg = false;		//#15239
			int lastSupEmplId = 0;			//#15239

            //WriteToDebugLog("dlgTlSupervisorOverride SendTimeOutMessage  " + System.DateTime.Now.ToLongTimeString() + ": " + "Handle Timer");
            if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)
            {
                _msgInfo = _comm.GetMessageInfo(_ovrdMessageInfoId);
                //#15088
                //if (_msgInfo != null && _msgInfo.RespTime != null && !_msgInfo.TimerExpired)
                if (_msgInfo != null && _msgInfo.TimerExpired)
                {
                    if (_msgInfo.MessageState == MessageState.Pending.ToString())
                    {
                        #region Build URL list
                        if (isSeqGroup && !string.IsNullOrEmpty(_msgInfo.PriorityList))
                        {
                            foreach (TargetUrlInfo urlInfo in _comm.TargetUrlList)
                            {
                                seqGrpUrlList = (string.IsNullOrEmpty(seqGrpUrlList) ? urlInfo.TargetUrl : seqGrpUrlList + "~" + urlInfo.TargetUrl);
                                lastSupEmplId = urlInfo.SupEmplId;		//#15239
                            }
                        }
                        #endregion

                        foreach (TargetUrlInfo urlInfo in _comm.TargetUrlList)
                        {
                            if (urlInfo.MessageId == _msgInfo.MessageId)
                            {
                                if (isSeqGroup && !string.IsNullOrEmpty(_msgInfo.PriorityList))
                                {
									if (urlInfo.SupEmplId == _msgInfo.SupEmplId)	//#15239
									{
										seqTargetUrl = urlInfo.TargetUrl;
										break;
									}
                                }
                                else
                                {
                                    //Send Time out message to all supervisors...
                                    if (!isTimeOutMsgUpdated)   //#15160
                                    {
                                        RetrieveTranOvrdInfo(_msgInfo.MessageId);   //#14265
                                        _comm.UpdateMessageInfoStatus(_msgInfo.MessageId, MessageState.TimeOut.ToString());
                                        _msgInfo.MessageState = MessageState.TimeOut.ToString();
                                        UpdateOverrideTranInfo(_msgInfo.MessageId, MessageState.TimeOut);
                                        isTimeOutMsgUpdated = true;
                                    }
                                    _comm.SendMessage(urlInfo.TargetUrl, _msgInfo);
                                    sentTimeOutMsg = true;
                                }
                            }
                        }
                        //Begin #15239
						if (isSeqGroup &&
							!string.IsNullOrEmpty(_msgInfo.PriorityList))    //#15239
						{
							//Send Time out message to one supervisor in the priority list
							_msgInfo.MessageState = MessageState.TimeOut.ToString();
							_comm.SendMessage(seqTargetUrl, _msgInfo);
							_msgInfo.MessageState = MessageState.Pending.ToString();
							if (lastSupEmplId == _msgInfo.SupEmplId)
								clearSeqMsg = true;

							SendMsgAndRestartTimer(_msgInfo);
						}
                        if (!string.IsNullOrEmpty(seqGrpUrlList) && clearSeqMsg)    //#15239
                       {
                            //Do not send Time out message instead just update the message state and continue
                            _msgInfo.PriorityList = string.Empty;
                            RetrieveTranOvrdInfo(_msgInfo.MessageId);   //#14265
                            _comm.UpdateMessageInfoStatus(_msgInfo.MessageId, MessageState.TimeOut.ToString());
                            _msgInfo.MessageState = MessageState.TimeOut.ToString();
                            UpdateOverrideTranInfo(_msgInfo.MessageId, MessageState.TimeOut);
                            sentTimeOutMsg = true;
                        }
						//End #15239
                        //
                        if (sentTimeOutMsg)
                        {
                            //Notify teller about the Time out message sent in the previous action(s)
                            //Remove message from teller queue
                            //if (_comm.TargetUrlList != null && _comm.TargetUrlList.Count > 0)    //#15332
                            //    _comm.RemoveFromTargetFwdInfoList(_msgInfo.MessageId, false);
                            //
                            //if (_comm.MessageList != null && _comm.MessageList.Count > 0)   //#15332
                            //    _comm.RemoveMessageListItem(_msgInfo.MessageId);
                            #region notify teller about the timeout
                            NotifyTeller(MessageState.TimeOut);
                            #endregion
                            //_comm.SendMessage(_msgInfo.CallBackUrl, _msgInfo);   //#15332
                            _tellerVars.RemOvrdMsgFromTeller = 0;
                            if (_queueTimer == null)   //#14265  //#13826
                            {
                                _queueTimer = new Timer();
                                _queueTimer.Interval = 1000;
                                _queueTimer.Tick += new EventHandler(_queueTimer_Tick);
                            }
                            _queueTimer.Enabled = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region NotifyTeller
        private void NotifyTeller(MessageState msgState)
        {
            frmShowNotes notify = new frmShowNotes();
            notify.TitleText = "Teller Notification";
            notify.MessageText = GetMessageText(msgState);
            /*Begin Task #123322 */
            if (_tellerVars.AdTlControl.RemoteOvrd.Value == GlobalVars.Instance.ML.Y &&
                _tellerVars.AdTlControl.ResPopTimeout != null &&
                _tellerVars.AdTlControl.ResPopTimeout.Value > 0)
            {
                notify.Delay = _tellerVars.AdTlControl.ResPopTimeout.Value * 1000;
            }
            /*End Task #123322 */
            notify.Animate();
        }
        #endregion

        #region Frame Time Out/Ignore Message Text
        private string GetMessageText(MessageState msgState)
        {
            string messageText = string.Empty;
            if (msgState == MessageState.Ignore)
                messageText = CoreService.Translation.GetUserMessageX(13395);
            if (msgState == MessageState.TimeOut)
                messageText = CoreService.Translation.GetUserMessageX(13396);
            return messageText;
        }
        #endregion

        #region Timer Tick Event
        private void _formTimer_Tick(object sender, EventArgs e)
        {
            UpdateMessageList();
        }
        #endregion

        void _queueTimer_Tick(object sender, EventArgs e)   //#15087
        {
            _queueTimer.Interval = 1000;
            _queueTimer.Enabled = true;
            if (_iconRemoteQ.Visible)
                _iconRemoteQ.Visible = false;
            else
                _iconRemoteQ.Visible = true;
        }

        #region Update Message State
        private void UpdateMessageList()    //#13826
        {
            if (_comm.MessageList != null && _comm.MessageList.Count > 0)
            {
                foreach (MessageInfo msg in _comm.MessageList)
                {
                    if (msg.MessageState == "Pending")  //#13826
                    {
                        if (msg.TimerInterval > 0)
                        {
                            msg.TimerInterval -= 1;
                            WriteToDebugLog("dlgTlSupervisorOverride Timer Tick: " + msg.TimerInterval.ToString());  //#15143
                        }
                        if (msg.TimerInterval <= 0 && !msg.TimerExpired)
                        {
                            msg.TimerExpired = true;
                            SendTimeOutMessage();
                            if (_comm.MessageList.Count <= 0)   //#15088
                                break;
                        }
                        if (_comm.MessageList.Count <= 0)   //#15160
                            break;
                    }
                }
                if (_comm.MessageList.Count <= 0)
                {
                    //_formTimer.Enabled = false;   //   //#15143
                    if (_formTimer != null)
                    {
                        _formTimer.Stop();
                        _formTimer.Dispose();   //#15143
                    }
                }

            }
        }

        private void ValidateRTOUsersLimit()    //#16877
        {
            if (_tranSet != null)
            {
                _ovrdBusObj.RtoUsersList.Value = _tranSet.ValidateRTOUserLimits(_ovrdBusObj.RtoUsersList.Value);
            }
        }
        #endregion

        #region Get Communicator User Info
        private CommunicatorUser GetCommunicatorUserInfo(int employeeId)
        {
            CommunicatorUser _tempUser = null;
            try
            {
                RefreshUserList();
                foreach (CommunicatorUser user in _commUserlist)
                {
                    if (user.EmployeeId == employeeId)
                    {
                        _tempUser = user;
                        break;
                    }
                }
            }
            catch (PhoenixException pe)
            {
                PMessageBox.Show(pe, MessageBoxButtons.OK);
                return _tempUser;
            }
            return _tempUser;
        }
        #endregion

        #region Euum Collection
        enum OverrideCategory
        {
            TranPosting = 1,
            Reversal = 2,
            NonCust = 3,
            TranPostingTellerFraud = 4,
            SharedBranch = 5,
            CloseOut = 6,
            TranCodeCTR = 7
        }

        enum MessageState
        {
            Pending,
            Accepted,
            Released,
            Approved,
            Denied,
            Forwarded,
            Ignore,
            TimeOut
        }
        #endregion
        //End #79314

        #region Load Misc Account Info
        private void LoadAcctDetails(Phoenix.BusObj.Teller.MiscAcctInfo miscAcctInfo, string acctType, string acctNo, short tranCode) //#15143
        {
            AcctTypeDetail acctTypeDetail = null;
            TlAcctDetails acctDetails = new TlAcctDetails();
            if (!isNonCust)
            {
                if (!acctDetails.AcctNo.IsNull)
                    acctDetails.CleanUpValues(true);

                acctDetails.AcctType.Value = acctType;
                acctDetails.AcctNo.Value = acctNo;
                acctDetails.SwapAcct.Value = 0;
                //
                if (!string.IsNullOrEmpty(acctType))
                {
                    acctTypeDetail = _gbHelper.GetAcctTypeDetails(acctType, string.Empty);
                    acctDetails.ApplType.Value = acctTypeDetail.ApplType;
                    acctDetails.DepLoan.Value = acctTypeDetail.DepLoan;
                }
                acctDetails.GetAcctDetails();
                if (acctDetails.TranCode.IsNull)
                    acctDetails.TranCode.Value = tranCode;
                acctDetails.SuspectAcct.Value = 0;
                //
                if (acctDetails != null)
                {
                    miscAcctInfo.DepType = acctDetails.DepType.Value;
                    miscAcctInfo.DepLoan = acctDetails.DepLoan.Value;
                    miscAcctInfo.RimNo = acctDetails.RimNo.Value;
                    miscAcctInfo.AcctId = acctDetails.AcctId.Value;
                }
            }
        }
        #endregion

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int msg, int wParam, ref int lParam);

        private void SetFocusOnForm()
        {
            int WM_SETFOCUS = 0x0007;
            int lParam = 0;
            PostMessage(this.Handle, WM_SETFOCUS, 0, ref lParam);     // WM_SETFOCUS = 0x0007
        }
    }
}
