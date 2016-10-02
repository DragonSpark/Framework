using System;

namespace DragonSpark.Sources
{
	public interface IScopeAware<in T> : IScopeAware, IAssignable<Func<object, T>>, IAssignable<Func<T>> {}
}