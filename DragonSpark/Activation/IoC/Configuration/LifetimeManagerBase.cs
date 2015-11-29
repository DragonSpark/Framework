using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.IoC.Configuration
{
	public abstract class LifetimeManagerBase<TLifetime> : LifetimeManager where TLifetime : Microsoft.Practices.Unity.LifetimeManager
	{
		protected override Microsoft.Practices.Unity.LifetimeManager CreateFrom( object parameter )
		{
			var result = Runtime.Activator.Create<TLifetime>( Parameters.ToArray() );
			return result;
		}

		protected virtual IEnumerable<object> Parameters
		{
			get { return Enumerable.Empty<object>(); }
		}
	}
}