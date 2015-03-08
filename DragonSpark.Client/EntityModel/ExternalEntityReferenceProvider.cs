using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Ria.Data;
using Common.Extensions;

namespace Common.EntityModel
{
	public class ExternalEntityReferenceProvider : IExternalEntityReferenceProvider
	{
		readonly static Dictionary<Type,IEnumerable<ExternalEntityReference>> References = new Dictionary<Type,IEnumerable<ExternalEntityReference>>();

		public IEnumerable<ExternalEntityReference> Resolve( Type entityType )
		{
			var result = References.Ensure( entityType, ResolveReferences );
			return result;
		}

		static IEnumerable<ExternalEntityReference> ResolveReferences( Type entityType )
		{
			var query = from property in entityType.GetProperties()
			            where property.IsDecoratedWith<ExternalReferenceAttribute>()
			            select property.FromMetadata<AssociationAttribute, ExternalEntityReference>( item => CreateReference( property, item ) );
			var result = query.NotNull();
			return result;
		}

		static ExternalEntityReference CreateReference( PropertyInfo property, AssociationAttribute item )
		{
			var keyMembers = item.ThisKeyMembers.ToList();
			var query = from keyMember in keyMembers
			            let otherKeyMember = item.OtherKeyMembers.ElementAt( keyMembers.IndexOf( keyMember ) )
			            select new { Key = otherKeyMember, Value = property.DeclaringType.GetProperty( keyMember ) };
			var keys = query.ToDictionary( i => i.Key, i => i.Value );
			var result = new ExternalEntityReference( property, keys );
			return result;
		}
	}
}