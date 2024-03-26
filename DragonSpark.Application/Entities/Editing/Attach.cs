using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Application.Entities.Editing;

public class Attach<TIn, T> : Modify<TIn, T> where T : class
{
	protected Attach(IEnlistedContexts contexts, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
		: this(contexts, query, modification.Await) {}

	protected Attach(IEnlistedContexts contexts, IQuery<TIn, T> query, Await<T> configure)
		: this(contexts, query, x => configure(x.Subject)) {}

	protected Attach(IEnlistedContexts contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
		: base(contexts, query, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}

	protected Attach(IEnlistedContexts contexts, IQuery<TIn, T> query, ICommand<T> configure)
		: this(contexts, query, configure.Then().Operation()) {}

	protected Attach(IEnlistedContexts contexts, IQuery<TIn, T> query, Action<T> configure)
		: this(contexts, query, Start.A.Command(configure).Operation()) {}

	protected Attach(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Await) {}

	protected Attach(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	protected Attach(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Await) {}

	protected Attach(IEdit<TIn, T> select, Await<Edit<T>> configure)
		: base(select, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}
}

[UsedImplicitly]
public class Attach<T> : Modify<T> where T : class
{
	protected Attach(IEnlistedContexts contexts, IOperation<Edit<T>> modification) : this(contexts, modification.Await) {}

	protected Attach(IEnlistedContexts contexts, Await<T> configure) : this(contexts, x => configure(x.Subject)) {}

	protected Attach(IEnlistedContexts contexts, Await<Edit<T>> configure)
		: base(contexts, AttachLocal<T>.Default.Then().Operation().Append(configure)) {}
}