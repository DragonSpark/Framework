using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.Configuration
{
    public class EnumerableType : Type
	{
		protected override object ResolveValue( IServiceProvider serviceProvider, System.Type type )
		{
			var result = typeof(IEnumerable<>).MakeGenericType( type );
			return result;
		}
	}
}