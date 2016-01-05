using DragonSpark.Diagnostics;
using PostSharp.Patterns.Contracts;
using System;
using System.IO;

namespace DragonSpark.Windows.Diagnostics
{
	public class TextMessageLogger : MessageLoggerBase, IDisposable
	{
		readonly TextWriter writer;

		public TextMessageLogger() : this( Console.Out )
		{}

		public TextMessageLogger( [Required]TextWriter writer )
		{
			this.writer = writer;
		}

		protected override void OnLog( Message message ) => writer.WriteLine( message.Text );

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				writer.Dispose();
			}
		}
	}
}