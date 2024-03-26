using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing;

public class Remove<TIn, T> : Modify<TIn, T> where T : class
{
	protected Remove(IEnlistedContexts contexts, IQuery<TIn, T> query, IOperation<T> configure)
		: this(contexts, query, configure.Await) {}

	protected Remove(IEnlistedContexts contexts, IQuery<TIn, T> query, Await<T> configure)
		: base(contexts, query, x => configure(x.Subject)) {}

	protected Remove(IEnlistedContexts contexts, IQuery<TIn, T> query)
		: this(contexts, query, (Edit<T> _) => Task.CompletedTask.ToOperation().ConfigureAwait(false)) {}

	protected Remove(IEnlistedContexts contexts, IQuery<TIn, T> query, IOperation<Edit<T>> configure)
		: this(contexts, query, configure.Await) {}

	protected Remove(IEnlistedContexts contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
		: base(contexts, query, Start.An.Operation(configure).Append(RemoveLocal<T>.Default)) {}

	protected Remove(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Await) {}

	protected Remove(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	protected Remove(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Await) {}

	protected Remove(IEdit<TIn, T> select, Await<Edit<T>> configure)
		: base(select, Start.An.Operation(configure).Append(RemoveLocal<T>.Default)) {}
}

public sealed class Remove<T> : Modify<T> where T : class
{
	public Remove(IEnlistedContexts contexts) : base(contexts, RemoveLocal<T>.Default.Then().Operation()) {}
}