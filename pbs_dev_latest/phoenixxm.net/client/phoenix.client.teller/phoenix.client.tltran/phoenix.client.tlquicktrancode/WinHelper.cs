using DevExpress.XtraBars.Docking;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Windows;
using Phoenix.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Phoenix.Client.Teller
{
    class WinHelper
    {
        public static DockPanel GetParentDockingControl(Control control)
        {
            return GetParent<DockPanel>(control);
        }

        public static T GetParent<T>(Control control) where T:class
        {
       
            T panel = control as T;
            if (panel != null)
                return panel;
            
            if (control.Parent != null)
                return GetParent<T>(control.Parent);
            return null;

        }

        public static void GetAlertsCollection( List<string> alertsRequired, PfwStandard form)
        {

        }

        public static CustomHeaderButton GenerateInvisibleButton(int height)
        {
            if (height <= 0)
                height = 0;

            Bitmap bitmap = new Bitmap(1, height);
            CustomHeaderButton btn = new DevExpress.XtraBars.Docking.CustomHeaderButton("", bitmap, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.CheckButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
            btn.UseCaption = true;
            btn.Caption = "";
            btn.Enabled = false;
            return btn;
        }
    }

    //public class DevExAlerts : Dictionary<string, CustomHeaderButton>
    //{
    //    Dictionary<PAction, CustomHeaderButton> _actionMapping = new Dictionary<PAction, CustomHeaderButton>();
    //    AlertsCollection _alertsCol = null;
    //    public void Initialize(PfwStandard form, DockPanel dockPanel)
    //    {
    //        if (_alertsCol != null)
    //            return;

    //        _alertsCol = ((form.Workspace as PwksWindow).WksExtension as WkspaceExtension).GetAlertsClone(form, true);
    //        PActionManager alertsActionManger = new PActionManager();
    //        alertsActionManger.Parent = form;

    //        //List<CustomHeaderButton> devExButtons = new List<CustomHeaderButton>();
    //        List<string> keys = new List<string>(this.Keys);

    //        foreach (var alertName in keys)
    //        {
    //            PAction alertAction = null;
    //            if (_alertsCol.ContainsKey(alertName))
    //                alertAction = _alertsCol[alertName];

    //            var devExButon = new CustomHeaderButton(alertName, alertAction.Image, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, true, null, true, false, true, null, null, null, -1);
    //            devExButon.Visible = alertAction.Visible;
    //            devExButon.Enabled = alertAction.Enabled;


    //            this[alertName] = devExButon;

    //            alertsActionManger.Actions.Add(alertAction);

    //            _actionMapping.Add(alertAction, devExButon);

    //            dockPanel.CustomHeaderButtons.Add(devExButon);
    //        }

    //        alertsActionManger.PropertyChanged += AlertsActionManger_PropertyChanged;
    //        dockPanel.CustomButtonClick += DockPanel_CustomButtonClick;
    //    }

    //    public void ShowAcctAlerts(AcctAlerts acctAlerts)
    //    {
    //        _alertsCol.ShowAcctAlerts(acctAlerts, true, false, false);
    //    }

    //    private void DockPanel_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    //    {
    //        var button = e.Button;
    //        if ( button != null )
    //        {
    //            var actionMap = _actionMapping.FirstOrDefault(x => x.Value == button);
    //            if ( actionMap.Value != null )
    //            {
    //                actionMap.Key.OnClick(actionMap.Key, null);
    //            }

    //        }
    //    }

    //    private void AlertsActionManger_PropertyChanged(object sender, EventArgs e)
    //    {
    //        PAction action = sender as PAction;
    //        var actionMap = _actionMapping.FirstOrDefault(x => x.Key == action);

    //        if (actionMap.Value != null)
    //        {
    //            actionMap.Value.Enabled = action.Enabled;
    //            actionMap.Value.Visible = action.Visible;
    //        }
    //    }
    //}
}
