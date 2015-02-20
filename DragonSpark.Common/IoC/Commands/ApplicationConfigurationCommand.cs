using DragonSpark.Activation.IoC;
using Microsoft.Practices.Unity;
using System.Windows.Markup;

namespace DragonSpark.Common.IoC.Commands
{
	[ContentProperty( "ApplicationDetails" )]
	public class ApplicationConfigurationCommand : IContainerConfigurationCommand
	{
		public ApplicationDetails ApplicationDetails { get; set; }

		public void Configure( IUnityContainer container )
		{
			OnConfigure( container );
		}

		protected virtual void OnConfigure( IUnityContainer container )
		{
			container.RegisterInstance( ApplicationDetails );
		}
	}
}