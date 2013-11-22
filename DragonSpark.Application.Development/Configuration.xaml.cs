using System.ComponentModel.Composition;
using DragonSpark.IoC.Configuration;

namespace DragonSpark.Application.Development
{
	/// <summary>
	/// Interaction logic for LoggingConfiguration.xaml
	/// </summary>
	[Export( typeof(IContainerConfigurationCommand) )]
	public partial class Configuration
	{
		public Configuration()
		{
			InitializeComponent();
		}
	}
}
