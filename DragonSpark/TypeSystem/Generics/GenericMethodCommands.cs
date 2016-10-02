using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public sealed class GenericMethodCommands : InstanceActionContext
	{
		public GenericMethodCommands( object instance ) : base( instance, instance.GetType().GetRuntimeMethods().Where( info => info.IsGenericMethod && !info.IsStatic ) ) {}
	}
}