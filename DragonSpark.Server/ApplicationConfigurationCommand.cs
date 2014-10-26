using Microsoft.Practices.Unity;
using System.Windows.Markup;

namespace DragonSpark.Server
{
	[ContentProperty( "Configurators" )]
	public class ApplicationConfigurationCommand : DragonSpark.IoC.Commands.ApplicationConfigurationCommand
	{
		protected override void OnConfigure( IUnityContainer container )
		{
			base.OnConfigure( container );
		}
	}
}
