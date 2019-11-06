using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Model.Sequences
{
	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Store<T> store, ICommand<T[]> command) : this(store.Instance, store.Length, command) {}

		public Session(T[] store, ICommand<T[]> command) : this(store, (uint)store.Length, command) {}

		public Session(T[] store, in uint size, ICommand<T[]> command)
		{
			Store    = store;
			Size     = size;
			_command = command;
		}

		public T[] Store { get; }
		public uint Size { get; }

		public void Dispose()
		{
			_command?.Execute(Store);
		}
	}
}