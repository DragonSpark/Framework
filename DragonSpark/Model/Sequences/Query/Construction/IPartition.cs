using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	public interface IPartition : IAlteration<Selection> {}

	public interface IPartition<T> : ISelect<Assigned<uint>, IBody<T>>, ISelect<IPartition, IPartition<T>> {}
}