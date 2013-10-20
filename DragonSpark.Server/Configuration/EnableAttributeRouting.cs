using System;
using System.Collections.ObjectModel;
using System.Web.Http;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Types" )]
	public class EnableAttributeRouting : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
//			var assemblies = Types.Any() ? Types.Select( x => x.Assembly ) : new[] { Assembly.GetExecutingAssembly() };

			configuration.MapHttpAttributeRoutes();
			configuration.EnsureInitialized();

			/*configuration.Routes.MapHttpAttributeRoutes( x =>
			{
				x.InheritActionsFromBaseController = true;
				assemblies.Apply( x.AddRoutesFromAssembly );
			} );*/
		}

		public Collection<Type> Types
		{
			get { return types; }
		}	readonly Collection<Type> types = new Collection<Type>();

		/*public Type HostingType { get; set; }*/
	}
}