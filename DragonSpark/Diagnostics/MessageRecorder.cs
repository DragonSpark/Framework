using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Diagnostics
{
	public class MessageRecorder : MessageLoggerBase, IMessageRecorder
	{
		readonly IList<Message> messages = new List<Message>();

		public MessageRecorder() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public MessageRecorder( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		public void Playback( Action<string> write )
		{
			messages.OrderBy( x => x.Time ).Each( tuple => write( tuple.Text ) );
		}

		protected override void Write( Message message )
		{
			messages.Add( message );
		}

		public IEnumerable<Message> Messages => messages;
	}
}