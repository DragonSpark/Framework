using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	public class EnumerableTypeExtension : MarkupExtension
	{
		readonly Type elementType;

		public EnumerableTypeExtension( Type elementType )
		{
			this.elementType = elementType;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = typeof(IEnumerable<>).MakeGenericType( elementType );
			return result;
		}
	}

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
			var result = ResolveType( Get<IXamlTypeResolver>( serviceProvider ) );
			return result;
		}

		Type ResolveType( IXamlTypeResolver resolver )
		{
			var result = resolver.Resolve( TypeName );
			return result;
		}

		static TResult Get<TResult>( IServiceProvider serviceProvider ) where TResult : class
		{
			var result = serviceProvider.GetService( typeof(TResult) ).As<TResult>();
			if ( result == null )
			{
				throw new ActivationException( string.Format( "The Generic markup extension requires an {0} service provider", typeof(TResult).Name ) );
			}
			return result;
		}
	}
}
