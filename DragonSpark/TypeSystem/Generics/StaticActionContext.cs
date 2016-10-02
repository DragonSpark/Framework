using System;
using System.Collections.Generic;
using System.Reflection;
using DragonSpark.Application;
using DragonSpark.Expressions;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem.Generics
{
	public class StaticActionContext : ActionContextBase
	{
		readonly static Func<MethodInfo, Execute> ToDelegate = InvokeMethodDelegate<Execute>.Default.ToSourceDelegate();
		public StaticActionContext( IEnumerable<MethodInfo> methods ) : base( ToDelegate, methods ) {}
	}
}