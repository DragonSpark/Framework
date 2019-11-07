using DragonSpark.Model.Results;

namespace DragonSpark.Model.Sequences
{
	public static class Sequence
	{
		public static ISequence<T> From<T>(params T[] items) => new Sequence<T>(items);

		public static ISequence<T> Using<T>(params IResult<T>[] items) => new DeferredSequence<T>(items);
	}

	class Sequence<T> : ISequence<T>
	{
		readonly Array<T> _items;

		public Sequence(Array<T> items) => _items = items;

		public Store<T> Get() => new Store<T>(_items);
	}
}