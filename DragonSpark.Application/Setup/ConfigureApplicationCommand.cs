using DragonSpark.Application.Markup;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Unity;
using System.Windows.Markup;

namespace DragonSpark.Application.Setup
{
	/*[ContentProperty( "Profile" )]
	public class ConfigureApplicationCommand : SetupCommand
	{
		public ApplicationProfile Profile { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Container().RegisterInstance( Profile );
		}
	}*/

	public class InitializeMarkupCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			MarkupExtensionMonitor.Instance.Initialize();
		}
	}
}