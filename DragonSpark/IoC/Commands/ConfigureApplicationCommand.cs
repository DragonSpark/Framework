using System.Windows.Markup;
using DragonSpark.Activation.IoC;
using Microsoft.Practices.Unity;

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
}