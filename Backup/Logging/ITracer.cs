using System;

namespace DragonSpark.Logging
{
	public interface ITracer
	{
		void Trace( Action action, string message, Guid? id  = null );
	}
}