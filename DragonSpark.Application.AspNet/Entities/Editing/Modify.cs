using System;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Modify<T> : Modify<T, T>
{
	protected Modify(IScopes scopes, IOperation<Edit<T>> modification) : this(scopes, modification.AsStop()) {}
	protected Modify(IScopes scopes, IStopAware<Edit<T>> modification) : this(scopes, modification.Off) {}

	protected Modify(IScopes scopes, Await<T> configure) : this(scopes, x => configure(x.Subject)) {}

	protected Modify(IScopes scopes, Await<Edit<T>> configure) : this(scopes, x => configure(x.Subject)) {}

	protected Modify(IScopes scopes, Await<Stop<Edit<T>>> configure)
		: base(new Edits<T, T>(scopes, A.Self<T>().Then().Operation().Out().AsStop()), configure) {}

	protected Modify(IEdit<T, T> edit, Await<Stop<Edit<T>>> configure) : base(edit, configure) {}

	protected Modify(IEdit<T, T> edit, ICommand<Edit<T>> modify) : base(edit, modify) {}

	protected Modify(IEdit<T, T> edit, ICommand<T> modify) : base(edit, modify) {}

	protected Modify(IEdit<T, T> edit, IOperation<T> modify) : base(edit, modify) {}

	protected Modify(IEdit<T, T> edit, IOperation<Edit<T>> modify) : base(edit, modify) {}

	protected Modify(IEdit<T, T> edit, Action<T> modify) : this(edit, Start.A.Command(modify).Get()) {}
}

public class Modify<TIn, T> : StopAware<TIn>
{
	protected Modify(IEnlistedScopes scopes, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
		: this(scopes.Then().Use(query).Edit.Single(), modification) {}

	protected Modify(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<T> configure)
		: this(scopes.Then().Use(query).Edit.Single(), configure) {}

	protected Modify(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Edit<T>> configure)
		: this(scopes.Then().Use(query).Edit.Single(), x => configure(x.Subject)) {}

	protected Modify(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Stop<Edit<T>>> configure)
		: this(scopes.Then().Use(query).Edit.Single(), configure) {}

	protected Modify(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Off) {}

	protected Modify(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject.Subject)) {}

	protected Modify(IEdit<TIn, T> select, Await<Edit<T>> configure) : this(select, x => configure(x.Subject)) {}

	protected Modify(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.AsStop()) {}

	protected Modify(IEdit<TIn, T> select, IStopAware<Edit<T>> configure) : this(select, configure.Off) {}

	protected Modify(IEdit<TIn, T> edit, ICommand<Edit<T>> modify) : this(edit, modify.Then().Operation().Out()) {}

	protected Modify(IEdit<TIn, T> edit, ICommand<T> modify)
		: this(edit, Start.A.Selection<Edit<T>>().By.Calling(x => x.Subject).Terminate(modify).Operation().Out()) {}

	protected Modify(IEdit<TIn, T> edit, Action<T> modify) : this(edit, Start.A.Command(modify).Get()) {}

	protected Modify(IEdit<TIn, T> select, Await<Stop<Edit<T>>> configure)
		: base(new ModifyDispatch<TIn, T>(select, configure)) {}
}