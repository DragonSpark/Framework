using System;

namespace DragonSpark.Application.Client.Threading
{
	public interface IDispatchHandler
	{
		void Execute( Action target );

		void Start( Action target );

		void Delay( Action target, TimeSpan time );
	}
}