using DragonSpark.Activation.IoC;
using DragonSpark.Application.Markup;
using Microsoft.Practices.Unity;
using System.Windows.Markup;

namespace DragonSpark.Application.IoC.Commands
{
	[ContentProperty( "Profile" )]
	public class ConfigureApplicationCommand : IContainerConfigurationCommand
	{
		public ApplicationProfile Profile { get; set; }

		public void Configure( IUnityContainer container )
		{
			OnConfigure( container );
		}

		protected virtual void OnConfigure( IUnityContainer container )
		{
			container.RegisterInstance( Profile );
		}
	}

	public class InitializeMarkupCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			MarkupExtensionMonitor.Instance.Initialize();
		}
	}
}