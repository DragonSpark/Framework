using DragonSpark.Model.Sequences;

namespace DragonSpark.Application
{
	public readonly struct Transactions<T>
	{
		public Transactions(Array<T> add, Array<(T Stored, T Source)> update, Array<T> delete)
		{
			Add    = add;
			Update = update;
			Delete = delete;
		}

		public Array<T> Add { get; }

		public Array<(T Stored, T Source)> Update { get; }

		public Array<T> Delete { get; }

		public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;
	}
}