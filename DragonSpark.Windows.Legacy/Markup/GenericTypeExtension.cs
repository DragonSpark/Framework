using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	[MarkupExtensionReturnType( typeof(Type) )]
	public class GenericTypeExtension : MarkupExtensionBase
	{
		public GenericTypeExtension( string typeName )
		{
			TypeName = typeName;
		}

		[NotEmpty]
		public string TypeName { [return: NotEmpty]get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) => serviceProvider.Get<IXamlTypeResolver>().Resolve( TypeName );
	}
}
