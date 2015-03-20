using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		readonly static Dictionary<Tuple<MemberInfo, Type>, Attribute[]> Cache = new Dictionary<Tuple<MemberInfo,Type>, Attribute[]>();

		public static bool IsDecoratedWith<TAttribute>( this MemberInfo target ) where TAttribute : Attribute
		{
			var result = target.GetAttribute<TAttribute>() != null;
			return result;
		}

		public static TAttribute GetAttribute<TAttribute>( this MemberInfo target ) where TAttribute : Attribute
		{
			var result = target.GetAttributes<TAttribute>().FirstOrDefault();
			return result;
		}

		public static TAttribute[] GetAttributes<TAttribute>( this MemberInfo target ) where TAttribute : Attribute
		{
			var key = new Tuple<MemberInfo, Type>( target, typeof(TAttribute) );
			var result = Cache.Ensure( key, ResolveAttributes ).OfType<TAttribute>().ToArray();
			return result;
		}

		static Attribute[] ResolveAttributes( Tuple<MemberInfo, Type> key )
		{
			var info = ServiceLocation.With<IMemberInfoLocator, MemberInfo>( x => x.Locate( key.Item1 ) ) ?? key.Item1;
			var result = info.GetCustomAttributes( key.Item2, true ).ToArray();
			return result;
		}
	}
}