using DragonSpark.Activation;
using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		public static bool IsDecoratedWith<TAttribute>( this MemberInfo target, bool inherit = true ) where TAttribute : Attribute
		{
			var result = target.GetAttribute<TAttribute>( inherit ) != null;
			return result;
		}

		public static TAttribute GetAttribute<TAttribute>( this MemberInfo target, bool inherit = true ) where TAttribute : Attribute
		{
			var result = target.GetAttributes<TAttribute>( inherit ).FirstOrDefault();
			return result;
		}

		public static TAttribute[] GetAttributes<TAttribute>( this MemberInfo target, bool inherit = true ) where TAttribute : Attribute
		{
			var result = ResolveAttributes( typeof(TAttribute), target, inherit ).OfType<TAttribute>().ToArray();
			return result;
		}

		[Cache]
		static IEnumerable<Attribute> ResolveAttributes( Type attributeType, MemberInfo member, bool inherit )
		{
			var info = Services.Location.With<IMemberInfoLocator, MemberInfo>( x => x.Locate( member ) ) ?? member;
			var result = info.GetCustomAttributes( attributeType, inherit ).ToArray();
			return result;
		}
	}
}