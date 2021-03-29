using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface ISave : IResulting<int> {}

	public interface ISaveChanges<in T> : ISelecting<T, uint> {}
}