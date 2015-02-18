using System;
using System.ServiceModel;

namespace DragonSpark.Server.Legacy.Configuration
{
	public abstract class IoCLifetimeManager<TLifetime, U> : LifetimeManagerBase<TLifetime> where TLifetime : IoCLifetimeManager<U> where U : IExtensibleObject<U>
	{
		public Guid Key { get; set; }

		protected override System.Collections.Generic.IEnumerable<object> Parameters
		{
			get { yield return Key; }
		}
	}
}