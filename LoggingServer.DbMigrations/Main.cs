using System;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using FluentMigrator.Runner.Announcers;

namespace LoggingServer.DbMigrations
{
    public partial class Main : Form
    {
        private readonly Welcome _welcome;
        private readonly Progress _progress;
        private readonly Finished _finished;
        private readonly Error _error;
        private readonly StringBuilder _log;

        public Main()
        {
            InitializeComponent();

            _welcome = new Welcome();
            _progress = new Progress();
            _finished = new Finished();
            _error = new Error();
            _log = new StringBuilder();

            ShowWelcome();
        }

        private void ShowWelcome()
        {
            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(_welcome);
        }

        private void ShowProgress()
        {
            try
            {
                var status = BootStrapper.GetDatabaseMigrationState();
                _progress.ProgressMax = status.Pending;
                _progress.ProgressCount = 0;
            }
            catch (Exception ex)
            {
                ShowFinal(ex);
            }

            pnlMain.Controls.Clear();
            pnlMain.Controls.Add(_progress);
            btnStart.Enabled = false;

            Invalidate();
            Refresh();

            try
            {
                Action<string> announcingDelegate = delegate(string msg)
                {
                    _log.AppendLine(msg);

                    if (msg.EndsWith(": migrated") && !msg.EndsWith("VersionMigration: migrated") && _progress.ProgressCount < _progress.ProgressMax)
                    {
                        _progress.ProgressCount++;
                    }

                    _progress.Refresh();
                    Refresh();
                };

                BootStrapper.Start(new BaseAnnouncer(announcingDelegate));

                _progress.ProgressCount = _progress.ProgressMax;
                _progress.Refresh();
                Refresh();

                Thread.Sleep(1000);

                ShowFinal(null);
            }
            catch (Exception ex)
            {
                ShowFinal(ex);
            }
        }

        private void ShowFinal(Exception ex)
        {
            pnlMain.Controls.Clear();

            if (ex != null)
            {
                _error.Log = _log.AppendLine(ex.ToString()).ToString();
                pnlMain.Controls.Add(_error);

                _error.Refresh();
                Refresh();
            }
            else
            {
                pnlMain.Controls.Add(_finished);
            }

            btnStart.Visible = false;
            btnFinish.Visible = true;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            ShowProgress();
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
