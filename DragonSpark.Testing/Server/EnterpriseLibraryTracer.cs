using System;
using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DragonSpark.Testing.Server
{
	public sealed class EnterpriseLibraryTracer : ITracer
	{
		readonly TraceManager manager;
		readonly string category;

		public EnterpriseLibraryTracer( TraceManager manager, string category = "Tracing" )
		{
			this.manager = manager;
			this.category = category;
		}

		public void Trace( Action action, string message, Guid? id  = null )
		{
			using ( id.HasValue ? manager.StartTrace( category, id.Value ) : manager.StartTrace( category ) )
			{
				action();
			}
		}
	}
}