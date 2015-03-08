using System;

namespace DragonSpark.Runtime
{
    public interface ILogger
    {
        void Write( string message, string category, Priority priority );
    }

	public interface ITracer
	{
		void Trace( Action action, Guid id );
	}
}