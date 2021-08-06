using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IToArray<T> : IMaterializer<T, Array<T>> {}
}