using System;
using System.Web.Hosting;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[MarkupExtensionReturnType( typeof(string) )]
	public class MapPathExtension : MarkupExtension
	{
		readonly string path;

		public MapPathExtension( string path )
		{
			this.path = path;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = HostingEnvironment.MapPath( path );
			return result;
		}
	}
}