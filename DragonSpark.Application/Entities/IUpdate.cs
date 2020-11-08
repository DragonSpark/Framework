using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface IUpdate<in T> : ISelecting<T, int> {}
}