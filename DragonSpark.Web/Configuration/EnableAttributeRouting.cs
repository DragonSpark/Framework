using AttributeRouting.Web.Http.WebHost;
using DragonSpark.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Windows.Markup;

namespace DragonSpark.Web.Configuration
{
	[ContentProperty( "Types" )]
	public class EnableAttributeRouting : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			var assemblies = Types.Any() ? Types.Select( x => x.Assembly ) : new[] { Assembly.GetExecutingAssembly() };

			configuration.Routes.MapHttpAttributeRoutes( x =>
			{
				x.InheritActionsFromBaseController = true;
				assemblies.Apply( x.AddRoutesFromAssembly );
			} );
		}

		public Collection<Type> Types
		{
			get { return types; }
		}	readonly Collection<Type> types = new Collection<Type>();

		/*public Type HostingType { get; set; }*/
	}
}