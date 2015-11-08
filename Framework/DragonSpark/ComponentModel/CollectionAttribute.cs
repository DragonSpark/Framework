using System;
using System.Reflection;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.ComponentModel
{
	public class CollectionAttribute : ActivateAttribute
	{
		public CollectionAttribute()
		{}

		public CollectionAttribute( Type elementType ) : base( typeof(Collection<>).MakeGenericType( elementType ) )
		{}

		protected override Type DetermineType( PropertyInfo propertyInfo )
		{
			var result = typeof(Collection<>).MakeGenericType( propertyInfo.PropertyType.GetInnerType() );
			return result;
		}
	}
}