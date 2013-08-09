using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Runtime;
using DragonSpark.Web.Configuration;
using Microsoft.Practices.Unity;
using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Http;
using System.Windows.Markup;

namespace DragonSpark.Web
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

	public class ExceptionHandlingConfiguration : DragonSpark.Configuration.ExceptionHandlingConfiguration
	{
		protected override void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			base.ConfigureExceptionHandling( handler );
			HttpContext.Current.ApplicationInstance.Error += ( s, a ) => HttpContext.Current.Error.As<Exception>( handler.Process );
		}
	}
}
