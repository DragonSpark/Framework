using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DragonSpark.Entity
{
	public static partial class PropertyInfoExtensions
	{
		public static Type ResolveEntityType( this PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType.GetCollectionElementType() ?? propertyInfo.PropertyType;
			return result;
		}

		public static Type GetCollectionType( this PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType.GetCollectionElementType();
			return result;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Yes, ugly.  Will attend to in a later version." )]
        public static IEnumerable<Tuple<string, string>> DetermineAssociationProperties( this PropertyInfo target )
		{
			var result = target.Transform( y => y.FromMetadata<AssociationAttribute, IEnumerable<Tuple<string,string>>>( z => z.ThisKeyMembers.TupleWith( z.OtherKeyMembers ) ) );
			return result;
		}
	}
}