using System;
using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DragonSpark.Application.Logging
{
	[Singleton( typeof(ITracer) )]
	public sealed class EnterpriseLibraryTracer : ITracer
	{
		readonly TraceManager manager;
		readonly string category;

		public EnterpriseLibraryTracer( TraceManager manager, string category = "Tracing" )
		{
			this.manager = manager;
			this.category = category;
		}

		public void Trace( Action action, Guid id )
		{
			using ( manager.StartTrace( category, id ) )
			{
				action();
			}
		}
	}
}