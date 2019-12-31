using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Execution
{
	sealed class Contexts : ISelect<string, IDisposable>
	{
		public static Contexts Default { get; } = new Contexts();

		Contexts() : this(DisposeContext.Default, ExecutionContextStore.Default,
		                  Start.An.Extent<ContextDetails>().From) {}

		readonly Func<object, ICommand> _command;

		readonly Func<string, object> _context;

		readonly ICommand _dispose;

		readonly IMutable<object> _store;

		public Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context)
			: this(dispose, store, context, A.Command(store).Then().Out) {}

		// ReSharper disable once TooManyDependencies
		Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context, Func<object, ICommand> command)
		{
			_dispose = dispose;
			_store   = store;
			_context = context;
			_command = command;
		}

		public IDisposable Get(string parameter)
		{
			var current = _store.Get();
			var result = _dispose.Then()
			                     .Then(_command(current))
			                     .Selector()
			                     .To(Start.An.Extent<Disposable>());
			_store.Execute(_context(parameter));
			return result;
		}
	}
}