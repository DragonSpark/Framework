using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IToArray<T> : IQuerying<T, Array<T>> {}
}