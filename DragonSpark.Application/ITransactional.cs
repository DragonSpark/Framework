using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application
{
	public interface ITransactional<T> : ISelect<(Array<T> Stored, Array<T> Updated), Transactions<T>> {}
}