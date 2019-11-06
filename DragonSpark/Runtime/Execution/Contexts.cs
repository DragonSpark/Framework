using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection;

namespace DragonSpark.Runtime.Execution
{
	sealed class Contexts : ISelect<string, IDisposable>
	{
		public static Contexts Default { get; } = new Contexts();

		Contexts() : this(DisposeContext.Default, ExecutionContextStore.Default, I<ContextDetails>.Default.From) {}

		// ReSharper disable once TooManyDependencies
		Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context, Func<object, ICommand> command)
		{
			_dispose = dispose;
			_store   = store;
			_context = context;
			_command = command;
		}

		readonly Func<object, ICommand> _command;
		readonly Func<string, object>   _context;

		readonly ICommand         _dispose;
		readonly IMutable<object> _store;

		public Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context)
			: this(dispose, store, context, store.AsCommand().Then().Out) {}

		public IDisposable Get(string parameter)
		{
			var current = _store.Get();
			var result = _dispose.Then()
			                     .Then(_command(current))
			                     .Selector()
			                     .To(I<Disposable>.Default);
			_store.Execute(_context(parameter));
			return result;
		}
	}
}