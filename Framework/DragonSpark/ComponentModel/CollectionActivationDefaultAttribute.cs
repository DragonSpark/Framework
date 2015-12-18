using System;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.ComponentModel
{
	public class CollectionActivationDefaultAttribute : ActivationDefaultAttribute
	{
		public CollectionActivationDefaultAttribute()
		{}

		public CollectionActivationDefaultAttribute( Type elementType ) : base( typeof(ActivatedCollection<>).MakeGenericType( elementType ) )
		{}

		protected override Type DetermineType( PropertyInfo propertyInfo )
		{
			return typeof(ActivatedCollection<>).MakeGenericType( propertyInfo.PropertyType.GetCollectionElementType() );
		}

		class ActivatedCollection<TItem> : System.Collections.ObjectModel.Collection<TItem>
		{}
	}
}