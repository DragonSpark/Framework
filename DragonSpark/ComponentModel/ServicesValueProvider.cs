using System;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class ServicesValueProvider : ValueProvider<Type>
	{
		public ServicesValueProvider( Func<PropertyInfo, Type> convert, Func<Type, object> create ) : base( convert, create ) {}
	}
}