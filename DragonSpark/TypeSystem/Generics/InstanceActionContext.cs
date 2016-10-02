using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Application;
using DragonSpark.Expressions;

namespace DragonSpark.TypeSystem.Generics
{
	public class InstanceActionContext : ActionContextBase
	{
		public InstanceActionContext( object instance, IEnumerable<MethodInfo> methods ) : base( new InvokeInstanceMethodDelegate<Execute>( instance ).Get, methods ) {}
	}
}