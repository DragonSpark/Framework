using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Entity
{
	public static partial class PropertyInfoExtensions
	{
		public static Type ResolveEntityType( this PropertyInfo propertyInfo )
		{
			var result = TypeExtensions.GetItemType( propertyInfo.PropertyType ) ?? propertyInfo.PropertyType;
			return result;
		}

		public static Type GetCollectionType( this PropertyInfo propertyInfo )
		{
			var result = TypeExtensions.GetItemType( propertyInfo.PropertyType );
			return result;
		}

        public static IEnumerable<Tuple<string, string>> DetermineAssociationProperties( this PropertyInfo target )
		{
			var result = target.Transform( y => y.FromMetadata<AssociationAttribute, IEnumerable<Tuple<string,string>>>( z => z.ThisKeyMembers.TupleWith( z.OtherKeyMembers ) ) );
			return result;
		}
	}
}