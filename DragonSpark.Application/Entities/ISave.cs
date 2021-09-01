using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Entities
{
	public interface ISave<in T> : IOperation<T> {}
}