using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	public interface IStores<T> : ISelect<uint, Store<T>> {}
}