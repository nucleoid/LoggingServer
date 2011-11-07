using System.Windows.Forms;
using System.Threading;

namespace LoggingServer.DbMigrations
{
    public partial class Error : UserControl
    {
        public string Log
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
        }

        public Error()
        {
            InitializeComponent();
        }

        public override void Refresh()
        {
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
            base.Refresh();

            Thread.Sleep(1);
        }
    }
}
