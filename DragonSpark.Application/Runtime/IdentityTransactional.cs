using DragonSpark.Application.Model;
using DragonSpark.Application.Model.Sequences;
using System;

namespace DragonSpark.Application.Runtime;

public class IdentityTransactional<T> : Transactional<T> where T : class, IIdentityAware
{
	protected IdentityTransactional(Func<Update<T>, bool> modified)
		: base(IdentityAwareEqualityComparer.Default, modified) {}

	protected IdentityTransactional(Func<Update<T>, bool> modified, Func<Location<T>, Update<T>> select)
		: base(IdentityAwareEqualityComparer.Default, modified, select) {}
}