using System;
using System.Web;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	public class ClientPathExtension : MarkupExtension
	{
		readonly string path;

		public ClientPathExtension( string path )
		{
			this.path = path;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = VirtualPathUtility.ToAbsolute( path );
			return result;
		}
	}
}