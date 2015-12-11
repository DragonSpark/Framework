using DragonSpark.Activation;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ActivateAttribute : DefaultAttribute
	{
		readonly IActivator activator;
		readonly Type activatedType;
		private readonly string name;

		public ActivateAttribute() : this( null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType, string name = null ) : this( Activation.Activator.Current, activatedType, name )
		{}

		public ActivateAttribute( IActivator activator, Type activatedType, string name )
		{
			this.activator = activator;
			this.activatedType = activatedType;
			this.name = name;
		}

		protected internal sealed override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var type = activatedType ?? DetermineType( propertyInfo );
			var result = Activate( instance, propertyInfo, type, name );
			return result;
		}

		protected virtual object Activate( object instance, PropertyInfo info, Type type, string s )
		{
			var result = activator.Activate<object>( type, s );
			return result;
		}

		protected virtual Type DetermineType( PropertyInfo propertyInfo )
		{
			return propertyInfo.PropertyType;
		}
	}
}