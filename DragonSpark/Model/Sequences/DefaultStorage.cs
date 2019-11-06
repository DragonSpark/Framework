using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences
{
	public sealed class DefaultStorage<T> : Storage<T>
	{
		public static DefaultStorage<T> Default { get; } = new DefaultStorage<T>();

		DefaultStorage() : base(Stores<T>.Default, EmptyCommand<T[]>.Default) {}
	}
}