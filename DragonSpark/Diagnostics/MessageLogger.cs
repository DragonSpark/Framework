using System.Diagnostics;

namespace DragonSpark.Diagnostics
{
	public class MessageLogger : MessageLoggerBase
	{
		public static MessageLogger Instance { get; } = new MessageLogger();

		protected override void OnLog( Message message ) => Debug.WriteLine( message.Text );
	}
}
