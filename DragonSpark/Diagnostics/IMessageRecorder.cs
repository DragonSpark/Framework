using System.Collections.Generic;

namespace DragonSpark.Diagnostics
{
	public interface IMessageRecorder
	{
		void Record( Message message );

		IEnumerable<Message> Messages { get; }
	}
}