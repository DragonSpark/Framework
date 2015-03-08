using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	public partial class PropertyInfoTypeConverterValueConverter
	{
		static Type ResolvePropertyConverter( PropertyInfo info )
		{
			var result = TypeDescriptor.GetConverter( info.PropertyType ).Transform( x => x.GetType() );
			return result;
		}
	}
}
