using System.ComponentModel;
using System.Configuration.Install;


namespace LoggingServer.WcfWindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
