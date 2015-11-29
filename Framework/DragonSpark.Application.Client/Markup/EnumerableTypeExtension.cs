using System;
using System.Collections.Generic;
using System.Windows.Markup;

namespace DragonSpark.Markup
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
}