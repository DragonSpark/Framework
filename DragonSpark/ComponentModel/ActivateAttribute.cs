using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ValueAttribute : ActivateAttribute
	{
		protected override object Activate( object instance, PropertyInfo info, Type type, string s )
		{
			var item = base.Activate( instance, info, type, s );
			var result = item.AsTo<IValue, object>( value => value.Item );
			return result;
		}
	}

	public class SingletonAttribute : DefaultAttribute
	{
		readonly Type hostType;
		readonly string propertyName;

		public SingletonAttribute( Type hostType ) : this( hostType, "Instance" )
		{}

		public SingletonAttribute( Type hostType, string propertyName )
		{
			this.hostType = hostType;
			this.propertyName = propertyName;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			var result = new SingletonLocator( propertyName ).Locate( hostType );
			return result;
		}
	}

	public class ActivateAttribute : DefaultAttribute
	{
		readonly IActivator activator;
		readonly Type activatedType;
		private readonly string name;

		public ActivateAttribute() : this( null, null )
		{}

		public ActivateAttribute( string name ) : this( null, name )
		{}

		public ActivateAttribute( Type activatedType ) : this( activatedType, null )
		{}

		public ActivateAttribute( Type activatedType, string name ) : this( Activation.Activator.Current, activatedType, name )
		{
		}

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