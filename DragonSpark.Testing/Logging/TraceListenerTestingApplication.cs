using System;
using DragonSpark.Application.Logging;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace DragonSpark.Testing.Logging
{
	public class TraceListenerTestingContext : TestingContext<TraceListener>
	{
		[Dependency]
		public ILoggerFacade Log { get; set; }
	}
}
