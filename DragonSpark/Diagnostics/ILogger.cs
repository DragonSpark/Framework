using System;

namespace DragonSpark.Diagnostics
{
	public interface ILogger
	{
		void Information( string message, Priority priority );

		void Warning( string message, Priority priority );

		void Exception( string message, Exception item );

		void Fatal( string message, Exception exception );
	}
}