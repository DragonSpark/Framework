using System;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;

namespace DragonSpark.ComponentModel
{
	public class ValueAttribute : ActivateAttribute
	{
		protected override object Activate( object instance, PropertyInfo info, Type type, string s )
		{
			var item = base.Activate( instance, info, type, s );
			var result = item.AsTo<IValue, object>( value => value.Item );
			return result;
		}
	}
}