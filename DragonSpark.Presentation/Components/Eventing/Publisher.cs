using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Eventing
{
	sealed class Publisher<T> : IPublisher<T> where T : class
	{
		readonly IEventAggregator _events;

		public Publisher(IEventAggregator events) => _events = events;

		public ValueTask Get(T parameter) => _events.PublishAsync(parameter.To<T>()).ToOperation();
	}
}