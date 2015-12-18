using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Entity
{
	public static class PropertyInfoExtensions
	{
		public static Type ResolveEntityType( this PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType.Adapt().GetInnerType() ?? propertyInfo.PropertyType;
			return result;
		}

		public static Type GetCollectionType( this PropertyInfo propertyInfo )
		{
			var result = propertyInfo.PropertyType.Adapt().GetInnerType();
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Yes, ugly.  Will attend to in a later version." )]
		public static IEnumerable<Tuple<string, string>> DetermineAssociationProperties( this PropertyInfo target )
		{
			var result = target.With( y => y.FromMetadata<AssociationAttribute, IEnumerable<Tuple<string,string>>>( z => z.ThisKeyMembers.TupleWith( z.OtherKeyMembers ) ) );
			return result;
		}
	}
}