using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public interface IToArray<T> : IMaterializer<T, Array<T>> {}
}