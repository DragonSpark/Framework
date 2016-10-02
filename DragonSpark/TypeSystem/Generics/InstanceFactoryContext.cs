using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Expressions;

namespace DragonSpark.TypeSystem.Generics
{
	public class InstanceFactoryContext : FactoryContextBase
	{
		public InstanceFactoryContext( object instance, IEnumerable<MethodInfo> methods ) : base( new InvokeInstanceMethodDelegate<Invoke>( instance ).Get, methods ) {}
	}
}