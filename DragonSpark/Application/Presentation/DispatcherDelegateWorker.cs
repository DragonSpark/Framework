using System;
using System.Windows.Threading;

namespace DragonSpark.Application.Presentation
{
	partial class DispatcherDelegateWorker
	{
		DispatcherTimer CreateTimer( Action target, TimeSpan time )
		{
			var result = new DispatcherTimer( time, DispatcherPriority.Normal, ( s, a ) => Execute( target ), dispatcher );
			return result;
		}
	}
}
