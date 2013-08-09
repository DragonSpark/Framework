using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AttributeProviderExtensions
	{
		readonly static Dictionary<Tuple<ICustomAttributeProvider,Type>,Attribute[]> Cache = new Dictionary<Tuple<ICustomAttributeProvider,Type>,Attribute[]>();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience method for metadata purposes." )]
		public static bool IsDecoratedWith<TAttribute>( this ICustomAttributeProvider target ) where TAttribute : Attribute
		{
			var result = target.GetAttributes<TAttribute>().Any();
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience method for metadata purposes." )]
		public static TAttribute GetAttribute<TAttribute>( this ICustomAttributeProvider target ) where TAttribute : Attribute
		{
			var result = target.GetAttributes<TAttribute>().FirstOrDefault();
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Convenience method for metadata purposes." )]
		public static TAttribute[] GetAttributes<TAttribute>( this ICustomAttributeProvider target ) where TAttribute : Attribute
		{
			var key = new Tuple<ICustomAttributeProvider, Type>( target, typeof(TAttribute) );
			var result = Cache.Ensure( key, ResolveAttributes ).OfType<TAttribute>().ToArray();
			return result;
		}

		public static IEnumerable<T> GetAttributes<T>(this MemberInfo member, bool inherit) {
			return Attribute.GetCustomAttributes(member, inherit).OfType<T>();
		}

		static Attribute[] ResolveAttributes( Tuple<ICustomAttributeProvider, Type> key )
		{
			var provider = key.Item1.As<MemberInfo>().Transform( x => new MetadataEnabledMemberInfoAttributeProvider( x ), () => key.Item1, x => !( x is Type ) );
			var attributes = provider.GetCustomAttributes( key.Item2, true );
			var result = attributes.OfType<Attribute>().ToArray();
			return result;
		}
	}
}