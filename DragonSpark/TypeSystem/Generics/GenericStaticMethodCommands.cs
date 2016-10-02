using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public sealed class GenericStaticMethodCommands : StaticActionContext
	{
		public GenericStaticMethodCommands( Type type ) : base( type.GetRuntimeMethods().Where( info => info.IsGenericMethod && info.IsStatic ) ) {}
	}
}