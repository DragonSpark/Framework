using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage( AttributeTargets.Property )]
	public class ActivateAttribute : DefaultAttribute
	{
		readonly Type activatedType;
		private readonly string name;

		public ActivateAttribute() : this( null, null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType ) : this( activatedType, null )
		{}

		public ActivateAttribute( Type activatedType, string name )
		{
			this.activatedType = activatedType;
			this.name = name;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var type = activatedType ?? DetermineType( propertyInfo );
			var result = name != null ? Activation.Activator.CreateNamedInstance<object>( type, name ) : Activation.Activator.CreateInstance<object>( type );
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