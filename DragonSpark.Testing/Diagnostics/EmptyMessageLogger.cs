using DragonSpark.Diagnostics;

namespace DragonSpark.Testing.Diagnostics
{
	public class EmptyMessageLogger : MessageLoggerBase
	{
		public static EmptyMessageLogger Instance { get; } = new EmptyMessageLogger();

		protected override void OnLog( Message message )
		{}
	}
}