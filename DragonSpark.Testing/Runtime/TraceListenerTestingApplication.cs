using DragonSpark.Logging;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.Unity;

namespace DragonSpark.Testing.Logging
{
	public class TraceListenerTestingContext : TestingContext<TraceListener>
	{
		[Dependency]
		public ILogger Log { get; set; }
	}
}
