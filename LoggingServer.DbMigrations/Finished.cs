using System.Configuration;
using System.Windows.Forms;

namespace LoggingServer.DbMigrations
{
    public partial class Finished : UserControl
    {
        public Finished()
        {
            InitializeComponent();

            label1.Text += ConfigurationManager.AppSettings["Environment"] +" " +GetType().Assembly.GetName().Version;
        }
    }
}
