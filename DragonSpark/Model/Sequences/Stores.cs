using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	public sealed class Stores<T> : Select<uint, Store<T>>, IStores<T>
	{
		public static Stores<T> Default { get; } = new Stores<T>();

		Stores() : base(x => new Store<T>(new T[x])) {}
	}
}