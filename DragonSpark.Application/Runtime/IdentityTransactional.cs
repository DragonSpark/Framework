using DragonSpark.Application.Model;
using System;

namespace DragonSpark.Application.Runtime;

public class IdentityTransactional<T> : Transactional<T> where T : class, IIdentityAware
{
	protected IdentityTransactional(Func<Mapping<T>, bool> modified)
		: base(IdentityAwareEqualityComparer.Default, modified) {}

	protected IdentityTransactional(Func<Mapping<T>, bool> modified, Func<Elements<T>, Mapping<T>> select)
		: base(IdentityAwareEqualityComparer.Default, modified, select) {}
}