using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public class Remove<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts) : base(contexts, Remove<T>.Default.Then().Operation()) {}
	}

	public sealed class Remove<T> : Command<Edit<T>>, IModify<T> where T : class
	{
		public static Remove<T> Default { get; } = new Remove<T>();

		Remove() : base(x => x.Remove(x.Subject)) {}
	}

	public class Remove<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<T> configure)
			: this(contexts, query, configure.Await) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: base(contexts, query, x => configure(x.Subject)) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query)
			: this(contexts, query, (Edit<T> _) => Task.CompletedTask.ToOperation().ConfigureAwait(false)) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> configure)
			: this(contexts, query, configure.Await) {}

		protected Remove(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: base(contexts, query, Start.An.Operation(configure).Append(Remove<T>.Default)) {}

		protected Remove(IEdit<TIn, T> @select, IOperation<T> configure) : this(@select, configure.Await) {}

		protected Remove(IEdit<TIn, T> @select, Await<T> configure) : this(@select, x => configure(x.Subject)) {}

		protected Remove(IEdit<TIn, T> @select, IOperation<Edit<T>> configure) : this(@select, configure.Await) {}

		protected Remove(IEdit<TIn, T> @select, Await<Edit<T>> configure)
			: base(@select, Start.An.Operation(configure).Append(Remove<T>.Default)) {}
	}
}