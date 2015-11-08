using System;
using System.Reflection;

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
}