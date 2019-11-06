using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Sequences
{
	public readonly struct Store<T> : IActivateUsing<T[]>
	{
		public static implicit operator Store<T>(T[] instance) => new Store<T>(instance);

		public Store(T[] instance) : this(instance, (uint)instance.Length, false) {}

		public Store(T[] instance, uint length, bool requested = true)
		{
			Instance  = instance;
			Length    = length;
			Requested = requested;
		}

		public T[] Instance { get; }

		public uint Length { get; }

		public bool Requested { get; }
	}
}