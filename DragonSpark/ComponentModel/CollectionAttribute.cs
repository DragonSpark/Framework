using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class CollectionAttribute : ActivateAttribute
	{
		public CollectionAttribute( Type elementType = null ) : base( typeof(CollectionProvider), elementType.With( type => typeof(Collection<>).MakeGenericType( type ) ) )
		{}
	}

	public class CollectionProvider : ActivatedValueProvider
	{
		public CollectionProvider( Type type, string name ) : base( type, name )
		{}

		protected override Type DetermineType( PropertyInfo propertyInfo )
		{
			var result = typeof(Collection<>).MakeGenericType( propertyInfo.PropertyType.Adapt().GetInnerType() );
			return result;;
		}
	}
}