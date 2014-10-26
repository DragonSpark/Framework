using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.ComponentModel
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IoCDefaultAttribute : DefaultPropertyValueAttribute
	{
		readonly Type type;
		readonly string name;

		public IoCDefaultAttribute() : this( null, null )
		{}

		public IoCDefaultAttribute( Type type ) : this( type, null )
		{}

		public IoCDefaultAttribute( string name ) : this( null, name )
		{}

		public IoCDefaultAttribute( Type type, string name )
		{
			this.type = type;
			this.name = name;
		}

		protected internal override object GetValue( object instance, PropertyInfo propertyInfo )
		{
			try
			{
				var result = ServiceLocator.Current.GetInstance( type ?? ResolvePropertyType( propertyInfo ), name );
				return result;
			}
			catch ( ActivationException )
			{
				return null;
			}
		}

		static Type ResolvePropertyType( PropertyInfo property )
		{
			var result = property.PropertyType.IsGenericType && typeof(IEnumerable<>) == property.PropertyType.GetGenericTypeDefinition() ? typeof(List<>).MakeGeneric( property.PropertyType.GetGenericArguments().First() ) : property.PropertyType;
			return result;
		}
	}
}