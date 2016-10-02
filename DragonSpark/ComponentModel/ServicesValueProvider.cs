using System;
using System.Reflection;
using Defaults = DragonSpark.Activation.Location.Defaults;

namespace DragonSpark.ComponentModel
{
	public class ServicesValueProvider : ValueProvider<Type>
	{
		public ServicesValueProvider( Func<PropertyInfo, Type> convert ) : this( convert, Defaults.ServiceSource ) {}

		public ServicesValueProvider( Func<PropertyInfo, Type> convert, Func<Type, object> create ) : base( convert, create ) {}
	}
}