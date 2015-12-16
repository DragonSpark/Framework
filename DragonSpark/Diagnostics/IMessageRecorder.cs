using System.Collections.Generic;

namespace DragonSpark.Diagnostics
{
	public interface IMessageRecorder : IMessageLogger
	{
		IEnumerable<Message> Messages { get; }
	}
}