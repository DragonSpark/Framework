using DragonSpark.Activation.FactoryModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DragonSpark.Extensions;

namespace DragonSpark.Diagnostics
{
	public class RecordingMessageLogger : MessageLoggerBase
	{
		readonly IList<Message> source = new Collection<Message>();
		readonly IReadOnlyCollection<Message> messages;

		public RecordingMessageLogger()
		{
			messages = new ReadOnlyCollection<Message>( source );
		}

		public Message[] Purge() => source.Purge();
		
		protected override void OnLog( Message message ) => source.Add( message );

		public IEnumerable<Message> Messages => messages;
	}
}