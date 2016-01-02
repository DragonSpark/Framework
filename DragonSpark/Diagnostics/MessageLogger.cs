using DragonSpark.Runtime;
using System.Diagnostics;

namespace DragonSpark.Diagnostics
{
	public class MessageLogger : MessageLoggerBase
	{
		public MessageLogger()
		{}

		public MessageLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		protected override void Write( Message message )
		{
			Debug.WriteLine( message.Text );
		}
	}
}
