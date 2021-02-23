using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.Eventing
{
	public interface IPublisher<in T> : IOperation<T> where T : class {}
}