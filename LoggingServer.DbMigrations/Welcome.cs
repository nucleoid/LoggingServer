using System.Windows.Forms;
using System.Configuration;

namespace LoggingServer.DbMigrations
{
    public partial class Welcome : UserControl
    {
        public Welcome()
        {
            InitializeComponent();

            label1.Text += ConfigurationManager.AppSettings["Environment"] +" " +GetType().Assembly.GetName().Version;
        }
    }
}
