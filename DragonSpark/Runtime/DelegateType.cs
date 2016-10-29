using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime
{
	sealed class DelegateType : Cache<MethodInfo, Type>
	{
		public static DelegateType Default { get; } = new DelegateType();
		DelegateType() : base( info => Expression.GetDelegateType( info.GetParameterTypes().AsEnumerable().Append( info.ReturnType ).Fixed() ) ) {}
	}
}