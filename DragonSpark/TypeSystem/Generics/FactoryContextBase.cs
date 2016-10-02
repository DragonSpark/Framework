using DragonSpark.Expressions;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public abstract class FactoryContextBase : DelegateCreationContextBase<Invoke>
	{
		protected FactoryContextBase( Func<MethodInfo, Invoke> creator, IEnumerable<MethodInfo> methods ) : base( creator, methods, info => info.ReturnType != Defaults.Void ) {}
	}
}