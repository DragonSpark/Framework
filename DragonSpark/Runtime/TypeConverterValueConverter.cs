using System;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	partial class TypeConverterValueConverter
	{
		IDisposable ResolveConverter( Type targetType )
		{
			var result = targetType.Transform( x => new TypeConverterManager( targetType, typeConverterType ) );
			return result;
		}
	}
}
