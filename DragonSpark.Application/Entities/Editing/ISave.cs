using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities.Editing
{
	public interface ISave<in T> : IOperation<T> {}
}