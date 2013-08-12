using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Server.Configuration;
using Microsoft.Practices.Unity;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Windows.Markup;

namespace DragonSpark.Server
{
	[ContentProperty( "Configurators" )]
    public class ApplicationConfigurationCommand : DragonSpark.Configuration.ApplicationConfigurationCommand
	{
		protected override void OnConfigure( IUnityContainer container )
		{
			base.OnConfigure( container );

			var httpConfiguration = container.TryResolve<HttpConfiguration>();
			var configuration = httpConfiguration ?? GlobalConfiguration.Configuration;
			Configurators.Apply( x => x.WithDefaults().Configure( configuration ) );
		}

		public Collection<IHttpApplicationConfigurator> Configurators
		{
			get { return configurators; }
		}	readonly Collection<IHttpApplicationConfigurator> configurators = new Collection<IHttpApplicationConfigurator>();
	}
}
