using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public sealed class GenericMethodFactories : InstanceFactoryContext
	{
		public GenericMethodFactories( object instance ) : base( instance, instance.GetType().GetRuntimeMethods().Where( info => info.IsGenericMethod && !info.IsStatic ) ) {}
	}
}