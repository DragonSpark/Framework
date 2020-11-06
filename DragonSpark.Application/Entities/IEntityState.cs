using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface IEntityState<in T> : ISelecting<T, int> {}
}