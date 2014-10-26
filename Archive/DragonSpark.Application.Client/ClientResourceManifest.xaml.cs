using System.ComponentModel.Composition;

namespace DragonSpark.Application.Client
{
	/// <summary>
	/// Interaction logic for ClientResourceManifest.xaml
	/// </summary>
	[Export( typeof(Server.ClientHosting.ClientResourceManifest) )]
	public partial class ClientResourceManifest
	{
		public ClientResourceManifest()
		{
			InitializeComponent();
		}
	}
}
