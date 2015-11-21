using System;

namespace DragonSpark.Diagnostics
{
	public class EmptyLogger : LoggerBase
	{
		public static EmptyLogger Instance { get; } = new EmptyLogger();
		protected override void Write( Line line )
		{
		}
	}
}