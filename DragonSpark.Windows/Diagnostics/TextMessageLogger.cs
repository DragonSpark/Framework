using System;
using System.IO;
using DragonSpark.Diagnostics;
using DragonSpark.Runtime;

namespace DragonSpark.Windows.Diagnostics
{
	public class TextMessageLogger : MessageLoggerBase, IDisposable
	{
		readonly TextWriter writer;

		public TextMessageLogger() : this( ExceptionFormatter.Instance )
		{}

		public TextMessageLogger( IExceptionFormatter formatter ) : this( formatter, CurrentTime.Instance, Console.Out )
		{}

		public TextMessageLogger( IExceptionFormatter formatter, ICurrentTime time, TextWriter writer) : base( formatter, time )
		{
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));

			this.writer = writer;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				writer?.Dispose();
			}
		}

		protected override void Write( Message message )
		{
			writer.WriteLine( message.Text );
		}
	}
}