using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public class Remove<TIn, T> : Modify<TIn, T> where T : class
{
	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, IOperation<T> configure)
		: this(scopes, query, configure.AsStop().Off) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<T> configure)
		: base(scopes, query, x => configure(x.Subject.Subject)) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query)
		: this(scopes, query, (Edit<T> _) => ValueTask.CompletedTask.Off()) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, IOperation<Edit<T>> configure)
		: this(scopes, query, configure.AsStop().Off) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Stop<T>> configure)
		: this(scopes, query, x => configure(x.Subject.Subject.Stop(x))) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Edit<T>> configure)
		: base(scopes, query, Start.An.Operation(configure).Append(RemoveLocal<T>.Default)) {}

	protected Remove(IEnlistedScopes scopes, IQuery<TIn, T> query, Await<Stop<Edit<T>>> configure)
		: base(scopes, query, configure.Then().Append(x => RemoveLocal<T>.Default.Execute(x.Subject))) {}

	protected Remove(IEdit<TIn, T> select, IOperation<T> configure) : this(select, configure.Off) {}

	protected Remove(IEdit<TIn, T> select, Await<T> configure) : this(select, x => configure(x.Subject)) {}

	protected Remove(IEdit<TIn, T> select, IOperation<Edit<T>> configure) : this(select, configure.Off) {}

	protected Remove(IEdit<TIn, T> select, Await<Edit<T>> configure)
		: base(select, Start.An.Operation(configure).Append(RemoveLocal<T>.Default)) {}
}

public sealed class Remove<T> : Modify<T> where T : class
{
	public Remove(IEnlistedScopes scopes) : base(scopes, RemoveLocal<T>.Default.Then().Operation()) {}
}