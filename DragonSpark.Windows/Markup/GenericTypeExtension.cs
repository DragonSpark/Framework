using System;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	/*public class EnumerableType : MarkupExtension
	{
		readonly Type elementType;

		public EnumerableType( Type elementType )
		{
			this.elementType = elementType;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = typeof(IEnumerable<>).MakeGenericType( elementType );
			return result;
		}
	}*/

	[MarkupExtensionReturnType( typeof(Type) )]
	public class GenericTypeExtension : MarkupExtension
	{
		public GenericTypeExtension()
		{}

		public GenericTypeExtension( string typeName )
		{
			// var items = typeName.Split( new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries );
			TypeName = typeName;
			// ProvidedTypes = items.Skip( 1 ).ToArray();
		}

		public string TypeName { get; set; }

		// ProvideValue, which returns an object instance of the constructed generic type
		public override object ProvideValue( System.IServiceProvider serviceProvider )
		{
			var result = serviceProvider.Get<IXamlTypeResolver>().Resolve( TypeName );
			return result;
		}
	}
}
