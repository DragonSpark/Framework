using System;
using System.Windows.Threading;

namespace DragonSpark.Application.Presentation
{
	public partial class DispatcherDelegateWorker
	{
		DispatcherTimer CreateTimer( Action target, TimeSpan time )
		{
			var result = new DispatcherTimer { Interval = time };
			result.Tick += ( s, a ) =>
			{
				result.Stop();
				Execute( target );
			};
			return result;
		}
	}
}
