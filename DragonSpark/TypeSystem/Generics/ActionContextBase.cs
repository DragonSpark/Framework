using DragonSpark.Application;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public abstract class ActionContextBase : DelegateCreationContextBase<Execute>
	{
		protected ActionContextBase( Func<MethodInfo, Execute> creator, IEnumerable<MethodInfo> methods ) : base( creator, methods, info => info.ReturnType == Defaults.Void ) {}
	}
}