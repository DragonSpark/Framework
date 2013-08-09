using System.Collections.Generic;
using System.Linq;
using DragonSpark.Runtime;

namespace DragonSpark.IoC.Configuration
{
	public abstract class LifetimeManagerBase<TLifetime> : LifetimeManager where TLifetime : Microsoft.Practices.Unity.LifetimeManager
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected override Microsoft.Practices.Unity.LifetimeManager Create()
		{
			var result = Activator.Create<TLifetime>( Parameters.ToArray() );
			return result;
		}

		protected virtual IEnumerable<object> Parameters
		{
			get { return Enumerable.Empty<object>(); }
		}
	}
}