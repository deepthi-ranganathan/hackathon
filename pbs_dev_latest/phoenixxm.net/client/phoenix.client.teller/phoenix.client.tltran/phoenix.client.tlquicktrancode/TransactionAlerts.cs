using DevExpress.XtraBars.Docking;
using DevExpress.XtraGrid.Views.BandedGrid;
using Phoenix.BusObj.Global;
using Phoenix.Shared.Windows;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phoenix.Windows.Forms;
using Phoenix.MultiTranTeller.Base;

namespace Phoenix.Client.Teller
{
    public class TransactionAlerts
    {
        AlertUIGroup _alertGroup1;
        AlertUIGroup _alertGroup2;
        ITellerWindow _parentWindow;
        Dictionary<KeyValuePair<string, string>, AcctAlerts> _alertsCache;

        public TransactionAlerts(ITellerWindow parentWindow, GroupControl groupControl)
        {
            _parentWindow = parentWindow;
            var form = parentWindow.TlTranCodeWindow as PfwStandard;
            _alertsCache = new Dictionary<KeyValuePair<string, string>, AcctAlerts>();
            _alertGroup1 = new AlertUIGroup(this, "Acct:", form, groupControl);
            _alertGroup2 = new AlertUIGroup(this, "Tfr Acct:", form, groupControl);
        }

        public void ShowAlerts(string acctType, string acctNo)
        {
            _alertGroup1.ShowAlerts(acctType, acctNo);
            _alertGroup1.HideLabel();
            _alertGroup2.HideAlerts();
            _alertGroup2.HideLabelAndSeparator();
        }

        public void ShowFromToAlerts(string fromAcctType,string fromAcctNo,string toAcctType, string toAcctNo)
        {
            try
            {
                _alertGroup1.ShowAlerts(fromAcctType, fromAcctNo);
                _alertGroup2.ShowAlerts(toAcctType, toAcctNo);
                if (!_alertGroup2.HasAlerts())
                {
                    _alertGroup1.HideLabel();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public AcctAlerts GetAlertInfo(string acctType,string acctNo)
        {
            AcctAlerts alerts = new AcctAlerts();
            KeyValuePair<string, string> pair = new KeyValuePair<string, string>(acctType, acctNo);

            if (_alertsCache.ContainsKey(pair))
            {
                alerts = _alertsCache[pair];
            }
            else
            {
                alerts = _alertsCache[pair] = _parentWindow.GetAlertInfo(acctType, acctNo);
            }

            return alerts;
        }

        public void HideAlerts()
        {
            _alertGroup1.HideAlerts();
            _alertGroup2.HideAlerts();
        }

        public void ResetAlertCache()
        {
            _alertsCache = new Dictionary<KeyValuePair<string, string>, AcctAlerts>();
        }

        public class AlertUIGroup
        {
            TransactionAlerts _alertContainer;
            ITellerWindow _parentWindow;
            GroupControl _parentControl;
            CustomHeaderButton _label;
            CustomHeaderButton _separator;
            QuickTranAlerts _alerts;
            public AcctAlerts _currentAlerts;
            PfwStandard _form;

            public AlertUIGroup(TransactionAlerts alertContainer,string labelText, PfwStandard parentWindow, GroupControl groupControl)
            {
                _alertContainer = alertContainer;
                _parentWindow = parentWindow as ITellerWindow;
                _parentControl = groupControl;
                _form = parentWindow;

                _label = CreateButton(labelText);
                _parentControl.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _label });

                InitializeAlerts();

                _separator = CreateButton("|");
                _parentControl.CustomHeaderButtons.AddRange(new DevExpress.XtraBars.Docking2010.IButton[] { _separator });

                HideAlerts();
            }

            public void HideAlerts()
            {
                if (_alerts != null)
                {
                    _alerts.ShowAcctAlerts(new AcctAlerts());
                }
                HideLabelAndSeparator();
            }

            public void ShowAlerts(string acctType, string acctNo)
            {
                if (_alerts == null)
                {
                    InitializeAlerts();
                    HideAlerts();
                }
                if (string.IsNullOrEmpty(acctType) || string.IsNullOrEmpty(acctNo) || acctType == "RIM")
                {
                    HideAlerts();
                    return;
                }

                if (_alerts != null && !string.IsNullOrEmpty(acctType) && !string.IsNullOrEmpty(acctNo))
                {
                    _currentAlerts = _alertContainer.GetAlertInfo(acctType, acctNo);
                    if (_currentAlerts.HasAlerts())
                    {
                        ShowLabelAndSeparator();
                        _alerts.ShowAcctAlerts(_currentAlerts);
                        ForceRedrawAlerts();
                    }
                    else
                    {
                        HideAlerts();
                    }
                }
                else
                {
                    HideLabelAndSeparator();
                }
            }

            public void ShowLabelAndSeparator()
            {
                _label.Visible = true;
                _separator.Visible = true;
            }

            public void ShowLabel()
            {
                _label.Visible = true;
            }

            public void HideLabel()
            {
                if (_label != null)
                    _label.Visible = false;
            }

            public bool HasAlerts()
            {
                if (_currentAlerts != null)
                {
                    return _currentAlerts.HasAlerts();
                }

                return false;
            }

            public void HideLabelAndSeparator()
            {
                if (_label != null)
                    _label.Visible = false;

                if (_separator != null)
                    _separator.Visible = false;
            }

            private void ForceRedrawAlerts()
            {
                try
                {
                    foreach (var obj in _parentControl.CustomHeaderButtons)
                    {
                        CustomHeaderButton btn = obj as CustomHeaderButton;
                        if (btn != null)
                        {
                                btn.BeginUpdate();
                                btn.EndUpdate();
                        }
                    }
                }
                catch
                {
                }
            }

            private void InitializeAlerts()
            {
                if (_alerts != null)    // already initialized 
                    return;

                _alerts = new QuickTranAlerts();
                _alerts.Add(AlertsNames.AcctBankrupt, null);
                _alerts.Add(AlertsNames.AdvRestrict, null);
                _alerts.Add(AlertsNames.AcctRegD, null);
                _alerts.Add(AlertsNames.Retirement, null);
                _alerts.Add(AlertsNames.Sweep, null);
                _alerts.Add(AlertsNames.Analysis, null);
                _alerts.Add(AlertsNames.AcctCrossRef, null);
                _alerts.Add(AlertsNames.AcctNotes, null);
                _alerts.Add(AlertsNames.Caution, null);
                _alerts.Add(AlertsNames.Hold, null);
                _alerts.Add(AlertsNames.Stop, null);
                _alerts.Add(AlertsNames.REJ, null);
                _alerts.Add(AlertsNames.UCF, null);
                _alerts.Add(AlertsNames.NSF, null);

                _alerts.Initialize(_form, _parentControl);
            }

            CustomHeaderButton CreateButton(string text)
            {
                CustomHeaderButton button = new CustomHeaderButton("", null, -1, DevExpress.XtraBars.Docking2010.HorizontalImageLocation.Default, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", false, -1, false, null, false, false, true, null, null, null, -1);
                button.UseCaption = true;
                button.Caption = text;
                return button;
            }
        }
    }
}
