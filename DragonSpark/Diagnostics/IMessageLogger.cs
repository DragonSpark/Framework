using System;

namespace DragonSpark.Diagnostics
{
	public interface IMessageLogger
	{
		void Information( string message, Priority priority = Priority.Normal );

		void Warning( string message, Priority priority = Priority.High );

		void Exception( string message, Exception exception );

		void Fatal( string message, Exception exception );
	}
}