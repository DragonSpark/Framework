using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query.Construction
{
	public interface IBodyBuilder<T> : ISelect<Partitioning, IBody<T>> {}
}