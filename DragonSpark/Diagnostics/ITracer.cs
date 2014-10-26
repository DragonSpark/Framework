using System;

namespace DragonSpark.Diagnostics
{
	public interface ITracer
	{
		void Trace( Action action, string message, Guid? id  = null );
	}
}