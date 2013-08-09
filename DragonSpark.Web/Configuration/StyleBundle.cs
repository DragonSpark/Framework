using System;
using System.Web.Hosting;
using System.Windows.Markup;

namespace DragonSpark.Web.Configuration
{
	public class StyleBundle : Bundle
	{
		protected override System.Web.Optimization.Bundle CreateInstance()
		{
			var result = new System.Web.Optimization.StyleBundle( Path );
			return result;
		}
	}

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