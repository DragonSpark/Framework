using System;
using System.Configuration;
using System.Windows.Markup;

namespace DragonSpark.Markup
{
	[MarkupExtensionReturnType( typeof(string) )]
	public class ConnectionStringExtension : MarkupExtension
	{
		readonly string name;

		public ConnectionStringExtension( string name )
		{
			this.name = name;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = ConfigurationManager.ConnectionStrings[name].ConnectionString;
			return result;
		}
	}
}