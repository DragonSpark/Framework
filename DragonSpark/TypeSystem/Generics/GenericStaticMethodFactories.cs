using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public sealed class GenericStaticMethodFactories : StaticFactoryContext
	{
		public GenericStaticMethodFactories( Type type ) : base( type.GetRuntimeMethods().Where( info => info.IsGenericMethod && info.IsStatic ) ) {}
	}
}