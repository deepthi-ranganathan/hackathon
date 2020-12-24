using Phoenix.Windows.Client;
using Phoenix.Windows.Forms;


namespace Phoenix.Client.Teller
{
    public partial class dlgQuickTranPreference : PfwStandard
    {
        public dlgQuickTranPreference()
        {
            InitializeComponent();
            AvoidSave = true;
        }

        public override bool OnActionSave(bool isAddNext)
        {
            base.OnActionSave(isAddNext);

            bool savedSucceeded = false;
            var tlTranCode = Workspace.CurrentWindow as frmTlTranCode;
            if (tlTranCode == null)
                return false;

            try
            {
                if (rbRestoreOriginalLayout.Checked)
                {
                    tlTranCode._quickTranWindow.RestoreDefaultLayout();
                    savedSucceeded = true;
                }
                else if (rbSaveCurrentLayout.Checked)
                {
                    tlTranCode._quickTranWindow.SaveCurrentLayout();
                    savedSucceeded = true;
                }
            }
            catch
            {
                savedSucceeded = false;
            }
            return savedSucceeded;
        }

        public override bool OnActionClose()
        {
            base.OnActionClose();
            return true;
        }
    }
}
