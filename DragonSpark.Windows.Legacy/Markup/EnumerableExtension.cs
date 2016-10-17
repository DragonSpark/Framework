using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class EnumerableExtension : MarkupExtension
	{
		readonly Type elementType;

		public EnumerableExtension( Type elementType )
		{
			this.elementType = elementType;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = typeof(IEnumerable<>).MakeGenericType( elementType );
			return result;
		}
	}
}