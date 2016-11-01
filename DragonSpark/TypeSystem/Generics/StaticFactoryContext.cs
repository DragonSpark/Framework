using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Expressions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem.Generics
{
	public class StaticFactoryContext : FactoryContextBase
	{
		readonly static Func<MethodInfo, Invoke> ToDelegate = InvokeMethodDelegate<Invoke>.Default.ToDelegate();
		public StaticFactoryContext( IEnumerable<MethodInfo> methods ) : base( ToDelegate, methods ) {}
	}
}