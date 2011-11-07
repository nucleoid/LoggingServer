using System.Windows.Forms;
using System.Threading;

namespace LoggingServer.DbMigrations
{
    public partial class Progress : UserControl
    {
        public int ProgressCount
        {
            get { return progressBar1.Value; }
            set 
            { 
                progressBar1.Value = value;
                lblStatus.Text = string.Format("Completed {0} of {1}", progressBar1.Value, progressBar1.Maximum);
            } 
        }

        public int ProgressMax
        {
            get { return progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }

        public Progress()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            base.Refresh();

            Thread.Sleep(1);
        }
    }
}
