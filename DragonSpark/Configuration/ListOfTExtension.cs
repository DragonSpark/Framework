using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[ContentProperty( "Items" )]
	public class ListOfTExtension : CollectionOfTExtensionBase<IList>
	{
		protected override Type GetCollectionType( Type type )
		{
			return typeof(List<>).MakeGenericType( type );
		}
	}

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