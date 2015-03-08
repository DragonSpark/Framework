using System;
using System.Windows.Markup;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Configuration
{
	public partial class GenericTypeExtension : MarkupExtension
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