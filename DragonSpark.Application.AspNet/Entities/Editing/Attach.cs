using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Attach<TIn, T> : Modify<TIn, T> where T : class
{
	protected Attach(IEnlistedScopes scopes, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
		: this(scopes, query, modification.Off) {}

	protected Attach(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<T> configure)
		: this(scopes, query, x => configure(x.Subject)) {}

	protected Attach(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Edit<T>> configure)
		: base(scopes, query, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}

	protected Attach(IEnlistedScopes scopes, IQuery<TIn, T> query, ICommand<T> configure)
		: this(scopes, query, configure.Then().Operation()) {}

	protected Attach(IEnlistedScopes scopes, IQuery<TIn, T> query, Action<T> configure)
		: this(scopes, query, Start.A.Command(configure).Operation()) {}

	protected Attach(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Off) {}

	protected Attach(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	protected Attach(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Off) {}

	protected Attach(IEdit<TIn, T> select, Await<Edit<T>> configure)
		: base(select, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}
}

[UsedImplicitly]
public class Attach<T> : Modify<T> where T : class
{
	protected Attach(IEnlistedScopes scopes, IOperation<Edit<T>> modification) : this(scopes, modification.Off) {}

	protected Attach(IEnlistedScopes scopes, Await<T> configure) : this(scopes, x => configure(x.Subject)) {}

	protected Attach(IEnlistedScopes scopes, Await<Edit<T>> configure)
		: base(scopes, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}
}