using DragonSpark.Activation.FactoryModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DragonSpark.Diagnostics
{
	public interface IMessageLocator : IFactory<Message[]>
	{}

	public class RecordingMessageLogger : MessageLoggerBase
	{
		readonly IList<Message> source = new System.Collections.ObjectModel.Collection<Message>();
		readonly IReadOnlyCollection<Message> messages;

		public RecordingMessageLogger()
		{
			messages = new ReadOnlyCollection<Message>( source );
		}
		
		protected override void OnLog( Message message ) => source.Add( message );

		public IEnumerable<Message> Messages => messages;
	}
}