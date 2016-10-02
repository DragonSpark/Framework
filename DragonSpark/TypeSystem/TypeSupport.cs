using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.TypeSystem
{
	public static class TypeSupport
	{
		public static Type From( object item )
		{
			var parameter = item as ParameterInfo;
			if ( parameter != null )
			{
				return parameter.ParameterType;
			}

			var type = item as Type;
			if ( type != null )
			{
				return type;
			}

			var member = item as MemberInfo;
			var result = member?.GetMemberType() ?? item.GetType();
			return result;
		}
	}
}