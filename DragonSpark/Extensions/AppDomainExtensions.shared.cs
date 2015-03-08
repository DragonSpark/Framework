using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Extensions
{
	public static partial class AppDomainExtensions
	{
		static readonly Dictionary<AppDomain,Type[]> TypeCache = new Dictionary<AppDomain, Type[]>();

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for convenience.")]
		public static IEnumerable<Tuple<TAttribute,Type>> GetAllTypesWith<TAttribute>( this AppDomain target ) where TAttribute : Attribute
		{
			var result = from type in TypeCache.Ensure( target, ResolveTypes )
			             let attribute = type.GetAttribute<TAttribute>()
			             where attribute != null
			             select new Tuple<TAttribute, Type>( attribute, type );
			return result;
		}

		static Type[] ResolveTypes( AppDomain target )
		{
			var query = from assembly in target.GetAssemblies()
			            from type in assembly.GetTypes()
			            select type;
			var result = query.ToArray();
			return result;
		}
	}
}