using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors;
using Phoenix.BusObj.Global;
using Phoenix.MultiTranTeller.Base;
using Phoenix.Shared.Windows;
using Phoenix.Windows.Client;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Client.Teller
{

    public class QuickTranAlerts : Dictionary<string, CustomHeaderButton>
    {
        Dictionary<PAction, CustomHeaderButton> _actionMapping = new Dictionary<PAction, CustomHeaderButton>();
        AlertsCollection _alertsCol = null;
        PActionManager _alertsActionManager; //Bug 93072
        ITlTranCodeWindow _parentWindow;
        Action _onCustomButtonAdded = null;

        public void Initialize(PfwStandard form, DockPanel dockPanel,Action onCustomButtonAdded = null)
        {
            if (_alertsCol != null)
                return;
            _onCustomButtonAdded = onCustomButtonAdded;
            _parentWindow = form as ITlTranCodeWindow;
            _alertsCol = ((form.Workspace as PwksWindow).WksExtension as WkspaceExtension).GetAlertsClone(form, true);

            _alertsActionManager = new PActionManager();
            _alertsActionManager.Parent = form;

            #region commented out code 
            ////List<CustomHeaderButton> devExButtons = new List<CustomHeaderButton>();
            //List<string> keys = new List<string>(this.Keys);

            //foreach (var alertName in keys)
            //{
            //    PAction alertAction = null;
            //    if (_alertsCol.ContainsKey(alertName))
            //        alertAction = _alertsCol[alertName];

            //    var devExButon = new CustomHeaderButton(alertName, alertAction.Image, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
            //    devExButon.Visible = alertAction.Visible;
            //    devExButon.Enabled = alertAction.Enabled;


            //    this[alertName] = devExButon;

            //    alertsActionManger.Actions.Add(alertAction);

            //    _actionMapping.Add(alertAction, devExButon);

            //    dockPanel.CustomHeaderButtons.Add(devExButon);
            //}
            #endregion

            AddHeaderButtons(_alertsActionManager, (p) => 
            {
                dockPanel.CustomHeaderButtons.Add(p);
                if (_onCustomButtonAdded != null)
                    _onCustomButtonAdded();
            });
                //AddHeaderButtons(dockPanel.CustomHeaderButtons, alertsActionManger);
            _alertsActionManager.PropertyChanged += AlertsActionManger_PropertyChanged;

            dockPanel.CustomButtonClick += DockPanel_CustomButtonClick;
        }


        public void Initialize(PfwStandard form, GroupControl groupControl)
        {
            if (_alertsCol != null)
                return;
            _parentWindow = form as ITlTranCodeWindow;
            _alertsCol = ((form.Workspace as PwksWindow).WksExtension as WkspaceExtension).GetAlertsClone(form, true);

            
            _alertsActionManager = new PActionManager();
             _alertsActionManager.Parent = form;

            //List<CustomHeaderButton> devExButtons = new List<CustomHeaderButton>();
            AddHeaderButtons(_alertsActionManager, (p) => { groupControl.CustomHeaderButtons.Add(p); });

            _alertsActionManager.PropertyChanged += AlertsActionManger_PropertyChanged;

            groupControl.CustomButtonClick += GroupControl_CustomButtonClick;
        }

        private void GroupControl_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            var button = e.Button;
            if (button != null)
            {
                var actionMap = _actionMapping.FirstOrDefault(x => x.Value == button);
                if (actionMap.Value != null)
                {
                    actionMap.Key.OnClick(actionMap.Key, null);
                }

            }
        }

        //Begin Task#90763//Bug#93072
        internal void Dispose()
        {
            if (_alertsActionManager != null)
            {
                _alertsActionManager.Dispose();
                _alertsActionManager = null;
            }
        }
        //End Task#90763//Bug 93072

        private void DockPanel_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            var button = e.Button;
            if (button != null)
            {
                var actionMap = _actionMapping.FirstOrDefault(x => x.Value == button);
                if (actionMap.Value != null)
                {
                    actionMap.Key.OnClick(actionMap.Key, null);
                }

            }
        }


        private void AddHeaderButtons(PActionManager alertsActionManger, Action<CustomHeaderButton> addButton)
        {
            List<string> keys = new List<string>(this.Keys);

            foreach (var alertName in keys)
            {
                PAction alertAction = null;
                if (_alertsCol.ContainsKey(alertName))
                    alertAction = _alertsCol[alertName];

                var devExButon = new CustomHeaderButton(alertName, alertAction.Image, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
                devExButon.Visible = alertAction.Visible;
                devExButon.Enabled = alertAction.Enabled;
                if (alertAction.ImageButton != null) //Bug#90200
                    devExButon.ToolTip = alertAction.ImageButton.ToolTip; 
                this[alertName] = devExButon;

                alertsActionManger.Actions.Add(alertAction);
                _actionMapping.Add(alertAction, devExButon);
                //addButton?.Invoke(devExButon);
                if (addButton != null)  //#87731-Build Fix
                    addButton(devExButon);

            }
        }

        public void ShowAcctAlerts(AcctAlerts acctAlerts)
        {
            try
            {
                _alertsCol.ShowAcctAlerts(acctAlerts, true, false, false);
            }
            catch(Exception ex)
            {
            }
        }

        public void HideAcctAlerts(AcctAlerts acctAlerts)
        {
            _alertsCol.HideAllAlerts(acctAlerts);
        }

        private void AlertsActionManger_PropertyChanged(object sender, EventArgs e)
        {
            if (_parentWindow.GetCurrentSelectedTabIndex() == (int)frmTlTranCode.TAB.QuickTran)
            {
                PAction action = sender as PAction;
                var actionMap = _actionMapping.FirstOrDefault(x => x.Key == action);

                if (actionMap.Value != null)
                {
                    actionMap.Value.Enabled = action.Enabled;
                    actionMap.Value.Visible = action.Visible;
                    if (actionMap.Value.ImageOptions.Image != action.Image)     //Bug 90202 #QT-NotesFix
                        actionMap.Value.ImageOptions.Image = action.Image;
                }
            }
        }
    }
}
