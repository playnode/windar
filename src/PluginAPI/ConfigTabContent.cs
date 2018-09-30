using System.Windows.Forms;

namespace Windar.PluginAPI
{
    public partial class ConfigTabContent : UserControl, IConfigFormContainer
    {
        readonly IConfigForm _form;

        public bool Changed
        {
            set
            {
                configSaveButton.Enabled = value;
                configCancelButton.Enabled = value;
            }
        }

        public ConfigTabContent(IConfigForm form)
        {
            _form = form;
            InitializeComponent();

            // Reference container with the form to allow the state of the buttons
            // to be changed by the events on changes to the form.
            form.FormContainer = this;

            // Anchor the form to the four sides of the container.
            configContentPanel.Controls.Add((Control) _form);
            ((Control) _form).Anchor = AnchorStyles.Bottom |
                AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        protected ConfigTabContent()
        {
            InitializeComponent();
        }

        void configSaveButton_Click(object sender, System.EventArgs e)
        {
            _form.Save();
        }

        void configCancelButton_Click(object sender, System.EventArgs e)
        {
            _form.Cancel();
        }
    }
}
