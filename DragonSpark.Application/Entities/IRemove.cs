using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface IRemove<in T> : ISelecting<T, uint> {}
}