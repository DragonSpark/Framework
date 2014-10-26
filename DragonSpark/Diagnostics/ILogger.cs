using System;

namespace DragonSpark.Diagnostics
{
	public interface ILogger
	{
		// [Event( 1, Level = EventLevel.Informational )]
		void Information( string message, Priority priority/*, EventKeywords keywords = EventKeywords.None*/ );

		// [Event( 2, Level = EventLevel.Warning )]
		void Warning( string message, Priority priority/*, EventKeywords keywords = EventKeywords.None*/ );

		// [Event( 3, Level = EventLevel.Error )]
		void Exception( string message, Exception item/*, EventKeywords keywords = EventKeywords.None*/ );

		// [Event( 4, Level = EventLevel.Critical )]
		void Fatal( string message, Exception exception/*, EventKeywords keywords = EventKeywords.None*/ );

		// [Event( 5, Level = EventLevel.Informational )]
		void StartTrace( string associatedMessage, Guid associatedId );

		// [Event( 6, Level = EventLevel.Informational )]
		void EndTrace( string associatedMessage, Guid associatedId, TimeSpan totalTime );
	}
}