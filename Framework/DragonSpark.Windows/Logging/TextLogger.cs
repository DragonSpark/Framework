using DragonSpark.Diagnostics;
using DragonSpark.Runtime;
using System;
using System.IO;

namespace DragonSpark.Windows.Logging
{
	public class TextLogger : LoggerBase, IDisposable
	{
		readonly TextWriter writer;

		public TextLogger() : this( ExceptionFormatter.Instance )
		{}

		public TextLogger( IExceptionFormatter formatter ) : this( formatter, CurrentTime.Instance, Console.Out )
		{}

		public TextLogger( IExceptionFormatter formatter, ICurrentTime time, TextWriter writer) : base( formatter, time )
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

		protected override void Write( Line line )
		{
			writer.WriteLine( line.Message );
		}
	}
}