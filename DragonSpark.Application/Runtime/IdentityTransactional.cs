using DragonSpark.Application.Model;
using System;

namespace DragonSpark.Application.Runtime;

internal class Class1 {}

public class IdentityTransactional<T> : Transactional<T> where T : class, IIdentityAware
{
	protected IdentityTransactional(Func<(T, T), bool> modified)
		: base(IdentityAwareEqualityComparer.Default, modified) {}

	protected IdentityTransactional(Func<(T, T), bool> modified, Func<(T, Memory<T>), (T, T)> select)
		: base(IdentityAwareEqualityComparer.Default, modified, select) {}
}