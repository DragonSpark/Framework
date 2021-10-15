using DragonSpark.Model.Selection.Stores;
using System;

namespace DragonSpark.Model.Commands;

sealed class DelegateCommands<T> : ReferenceValueStore<Action<T>, ICommand<T>>
{
	public static DelegateCommands<T> Default { get; } = new DelegateCommands<T>();

	DelegateCommands() : base(x => x.Target as ICommand<T> ?? new Command<T>(x)) {}
}