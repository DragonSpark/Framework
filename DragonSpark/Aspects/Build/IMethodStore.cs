using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.Aspects.Build
{
	public interface IMethodStore : ITypeAware, IParameterizedSource<Type, MethodInfo> {}
}