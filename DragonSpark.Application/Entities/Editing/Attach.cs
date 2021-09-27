using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Editing
{
	public class Attach<TContext, T> : Modify<TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> contexts, IOperation<Edit<T>> modification)
			: this(contexts, modification.Await) {}

		protected Attach(IContexts<TContext> contexts, Await<T> configure)
			: this(contexts, x => configure(x.Subject)) {}

		protected Attach(IContexts<TContext> contexts, Await<Edit<T>> configure)
			: base(contexts, Attach<T>.Default.Then().Operation().Append(configure)) {}
	}

	public sealed class Attach<T> : Command<Edit<T>>, IModify<T> where T : class
	{
		public static Attach<T> Default { get; } = new Attach<T>();

		Attach() : base(x => x.Attach(x.Subject)) {}
	}

	public class Attach<TIn, TContext, T> : Modify<TIn, TContext, T> where TContext : DbContext where T : class
	{
		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, IOperation<Edit<T>> modification)
			: this(contexts, query, modification.Await) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<T> configure)
			: this(contexts, query, x => configure(x.Subject)) {}

		protected Attach(IContexts<TContext> contexts, IQuery<TIn, T> query, Await<Edit<T>> configure)
			: base(contexts, query, Attach<T>.Default.Then().Operation().Append(configure)) {}

		protected Attach(IEdit<TIn, T> @select, IOperation<T> configure) : this(@select, configure.Await) {}

		protected Attach(IEdit<TIn, T> @select, Await<T> configure) : this(@select, x => configure(x.Subject)) {}

		protected Attach(IEdit<TIn, T> @select, IOperation<Edit<T>> configure) : this(@select, configure.Await) {}

		protected Attach(IEdit<TIn, T> @select, Await<Edit<T>> configure)
			: base(@select, Attach<T>.Default.Then().Operation().Append(configure)) {}
	}
}