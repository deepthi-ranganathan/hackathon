#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2007 Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: frmShBranchTranDisplay.cs
// NameSpace: Phoenix.Client.Journal
//-------------------------------------------------------------------------------
//Date		    Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//2/26/2010     1       mramalin  
//6/15/2010     2       rpoddar     #09372 - Add account fields
//06/30/2010    3      rpoddar     #79510, #09368 - Make the journal display window work in 24 x 7 mode.
//-------------------------------------------------------------------------------

#endregion



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Phoenix.Windows.Forms;
using Phoenix.FrameWork.BusFrame;
using Phoenix.BusObj.Teller;
using Phoenix.FrameWork.Core;

namespace Phoenix.Client.Journal
{
    public partial class frmShBranchTranDisplay : PfwStandard
    {
        #region Private Variables
        TlJournal _tlJournal;
        #endregion
        #region Constructors
        public frmShBranchTranDisplay()
        {
            InitializeComponent();
        }




        #endregion

        #region Public Properties

        #endregion

        #region Public Methods
        public override void InitParameters(params object[] paramList)
        {
            
            //TODO: modify the parameters

            // Must call the base to store the parameters.
            base.InitParameters(paramList);
        }

        PDecimal _ptid = new PDecimal("Ptid");
        PDecimal _branchNo = new PDecimal("BranchNo");
        PDecimal _drawerNo = new PDecimal("DrawerNo");
        PDecimal _sequenceNo = new PDecimal("SequenceNo");
        PDecimal _subSequenceNo = new PDecimal("SubSequenceNo");
        PDateTime _effectiveDt = new PDateTime("EffectiveDt");
        PString _recordSource = new PString("RecordSource");
        TlJournalAddlInfo _boTlJournalAddlInfo;
        public override void OnCreateParameters()
        {
            Parameters.Add(_ptid);
            Parameters.Add(_branchNo);
            Parameters.Add(_drawerNo);
            Parameters.Add(_sequenceNo);
            Parameters.Add(_subSequenceNo);
            Parameters.Add(_effectiveDt);
            Parameters.Add(_recordSource);

            //base.OnCreateParameters();
        }

        #endregion
        


        #region Others
        private ReturnType fwStandard_PInitBeginEvent()
        {
            ScreenId = 2992;
            this.AppToolBarId = AppToolBarType.NoEdit;
            //TODO: Add code to set the main business object here
            _boTlJournalAddlInfo = new TlJournalAddlInfo();
            _boTlJournalAddlInfo.JournalPtid.Value = _ptid.Value;
            _boTlJournalAddlInfo.RecordSource.Value = _recordSource.Value;
            MainBusinesObject = _boTlJournalAddlInfo;
            return ReturnType.Success;
        }

        private void fwStandard_PInitCompleteEvent()
        {
            //TODO: Add code to handle after the form created logic
            dfImageRetreivalID.Text = Convert.ToString(this.dfImageRetreivalID.UnFormattedValue);

            CallXMThruCDS(CallXMThru.FormInitComplete);

            dfAcct.Text = _tlJournal.GetAccountDesc((_tlJournal.AcctType.IsNull ? string.Empty : _tlJournal.AcctType.Value),
                            (_tlJournal.AcctNo.IsNull ? string.Empty : _tlJournal.AcctNo.Value),
                            (_tlJournal.RimNo.IsNull ? 0 : _tlJournal.RimNo.Value));

            
            dfTfrAcct.Text = _tlJournal.GetTfrAccountDesc(_tlJournal.TfrAcctType.Value, _tlJournal.TfrAcctNo.Value, _tlJournal.TranCode.Value);
            //if (string.IsNullOrEmpty(dfTfrAcct.Text))
            //    dfTfrAcct.Text = _tlJournal.TfrAcctNo.Value;

        }
        #endregion

        private void CallXMThruCDS(CallXMThru origin)
        {
            if (origin == CallXMThru.FormInitComplete)
            {
                _tlJournal = new TlJournal();
                _tlJournal.BranchNo.Value = (short)_branchNo.Value;
                _tlJournal.DrawerNo.Value = (short)_drawerNo.Value;
                _tlJournal.EffectiveDt.Value = _effectiveDt.Value;
                _tlJournal.SequenceNo.Value = (short)_sequenceNo.Value;
                _tlJournal.SubSequence.Value = (short)_subSequenceNo.Value;
                _tlJournal.Ptid.Value = _ptid.Value;
                _tlJournal.SelectAllFields = false;
                _tlJournal.OutputType.Value = 4;
                _tlJournal.AcctNo.Selected = true;
                _tlJournal.AcctType.Selected = true;
                _tlJournal.TfrAcctNo.Selected = true;
                _tlJournal.TfrAcctType.Selected = true;
                _tlJournal.TranCode.Selected = true;
                _tlJournal.RimNo.Selected = true;

                CoreService.DataService.ProcessRequest(XmActionType.Select, _tlJournal);
            }
        }

        enum CallXMThru
        {
            FormInitComplete
        }
    }
}