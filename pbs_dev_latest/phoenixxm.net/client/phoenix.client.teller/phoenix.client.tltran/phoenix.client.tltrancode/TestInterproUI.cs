#region Comments
//-------------------------------------------------------------------------------
// File Name: frmTlTrancode.cs
// NameSpace: Phoenix.Window.Client
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//08/06/2012    1       Mkrishna    #19058 - Adding call to base on initParameters.
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

namespace Phoenix.Windows.Client
{
    public partial class TestInterproUI : PfwStandard
    {
        private PComboBoxStandard drawerCombo;
        private PAction bondAction;
        private PAction tcdAction;
        IPhoenixWorkspace wkspace;

        public TestInterproUI()
        {
            InitializeComponent();
        }

        private void pbProcess_Click(object sender, EventArgs e)
        {

            wkspace = this.Workspace;
            //this.Close();
            pbAddNext_Click(null, null);
            this.DialogResult = DialogResult.OK;
            this.Close();
            //IPhoenixWorkspace wkspace = CreateNewWorkspace(wkspaceName, true);

        }

        public override void InitParameters(params object[] paramList)
        {
            if (paramList.Length >= 3)
            {
                drawerCombo = paramList[0] as PComboBoxStandard;
                bondAction = paramList[1] as PAction;
                tcdAction = paramList[2] as PAction;
                //
            }
            this.IsActionPaneNeeded = false;
            this.Visible = false;
            base.InitParameters(paramList); //#19058

        }

        private void TestInterproUI_PInitCompleteEvent()
        {
            this.dfResponse.Text = @"   <?xml version=""1.0"" encoding=""utf-8"" ?> 
- <XAPI_RESPONSE>
- <RESPONSE>
- <RECORD>
- <TL_TRANSACTION_SET objectId=""49362176"" customAction=""PostTransactions"" sfOnCustom=""1"">
  <ReturnCode>0</ReturnCode> 
  <BranchNo>1</BranchNo> 
  <DrawerNo>999</DrawerNo> 
  <EffectiveDt>2008-06-27 00:00:00.000</EffectiveDt> 
  <SequenceNo>118</SequenceNo> 
  <TRAN_SET_DETAILS><TRANSACTION> <JournalPtid>12020321</JournalPtid> <XapiPtid>1904295</XapiPtid> <ReturnCode>0</ReturnCode> <ReturnCodeDesc></ReturnCodeDesc> <CTRExempt>N</CTRExempt> <CTRType></CTRType> <CTRCashInAmt></CTRCashInAmt> <CTRCashOutAmt></CTRCashOutAmt> <JournalCrDate>3/30/2010 9:50:00 AM</JournalCrDate> <TL_ITEM> <ReturnCode>0</ReturnCode> <ReturnCodeDesc></ReturnCodeDesc> </TL_ITEM> <TL_ITEM> <ReturnCode>0</ReturnCode> <ReturnCodeDesc></ReturnCodeDesc> </TL_ITEM> <TL_CHARGE> <ReturnCode>0</ReturnCode> <ReturnCodeDesc></ReturnCodeDesc> </TL_CHARGE> </TRANSACTION> <ReturnCode>0</ReturnCode> <ReturnCodeDesc></ReturnCodeDesc></TRAN_SET_DETAILS> 
  <CTROwnBehalf>0</CTROwnBehalf> 
  </TL_TRANSACTION_SET>
  </RECORD>
  <Sequence>1</Sequence> 
  <ReferenceNo>0330094948002</ReferenceNo> 
  <ReturnCode>0</ReturnCode> 
  <OvrdInfo1 /> 
  <OvrdInfo2 /> 
  </RESPONSE>
  <Rc>0</Rc> 
  <SQL>0</SQL> 
  <XmDbStatus>D</XmDbStatus> 
  <PrimDbAvailable>1</PrimDbAvailable> 
  <HRESULT>0</HRESULT> 
  </XAPI_RESPONSE>";

            //this.Closed += new EventHandler(TestInterproUI_Closed);
        }

        void TestInterproUI_Closed(object sender, EventArgs e)
        {
            PfwStandard temp = CreateWindow("Phoenix.Client.TlTrancode", "Phoenix.Windows.Client", "frmTlTranCode");
            temp.Workspace = wkspace;
            temp.InitParameters(drawerCombo, bondAction, tcdAction, string.Empty, dfResponse.Text);//#72916TBD  #76430 added Rim_No
            temp.Show();
        }

        private void TestInterproUI_PShowCompletedEvent(object sender, EventArgs e)
        {
            this.Visible = true;
        }

        private void pbAddNext_Click(object sender, EventArgs e)
        {
            if (dfResponse.Text == null)
            {
                return;
            }
            Phoenix.Windows.Client.Helper.SendMessageToMDI((int)GlobalActions.ChildAction, this, dfResponse.Text);
            dfResponse.Text = null;
            
        }
    }
}