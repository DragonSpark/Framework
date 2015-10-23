using DragonSpark.Extensions;
using System;
using System.Reflection;
using DragonSpark.Runtime;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public class ActivateAttribute : DefaultAttribute
	{
		readonly Type activatedType;

		public ActivateAttribute() : this( null )
		{}

		public ActivateAttribute( Type activatedType )
		{
			this.activatedType = activatedType;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var type = activatedType ?? DetermineType( propertyInfo );
			var result = Activation.Activator.CreateInstance<object>( type );
			return result;
		}

		protected virtual Type DetermineType( PropertyInfo propertyInfo )
		{
			return propertyInfo.PropertyType;
		}

		/*public Type ActivatedType
		{
			get { return activatedType; }
		}*/
	}

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