using System.Collections;
using System.ComponentModel;


namespace DragonSpark.Features
{
    [RunInstaller( true )]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			eventLog1.WriteEntry( string.Format( "{0} has been installed.", eventLog1.Source ) );
		}
    }
}
