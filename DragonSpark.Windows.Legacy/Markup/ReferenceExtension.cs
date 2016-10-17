using DragonSpark.Extensions;
using System;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class ReferenceExtension : Reference
	{
		public ReferenceExtension()
		{}

		public ReferenceExtension( string name ) : base( name )
		{}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var item = base.ProvideValue( serviceProvider );
			var result = item.AsTo<MarkupExtension, object>( extension => extension.ProvideValue( serviceProvider ) ) ?? item;
			return result;
		}
	}
}