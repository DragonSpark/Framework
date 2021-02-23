using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Eventing
{
	public class Publish<T> : IOperation where T : class
	{
		readonly IPublisher<T> _publisher;
		readonly T             _message;

		public Publish(IPublisher<T> publisher, T message)
		{
			_publisher = publisher;
			_message   = message;
		}

		public ValueTask Get() => _publisher.Get(_message);
	}
}