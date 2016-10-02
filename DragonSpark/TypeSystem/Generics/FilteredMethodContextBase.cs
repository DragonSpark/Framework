using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem.Generics
{
	public abstract class FilteredMethodContextBase
	{
		protected FilteredMethodContextBase( IEnumerable<MethodInfo> methods, Func<MethodInfo, bool> filter )
		{
			Methods = methods.Where( filter ).ToImmutableArray();
		}

		protected ImmutableArray<MethodInfo> Methods { get; }
	}
}