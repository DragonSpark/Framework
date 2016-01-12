using System;
using DragonSpark.Runtime.Values;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public class ServiceLocation : ExecutionContextValue<IServiceLocator>, IServiceLocation, IDisposable
	{
		public static ServiceLocation Instance { get; } = new ServiceLocation();

		public bool IsAvailable => Item != null;

		protected override void OnDispose()
		{
			Assign( null );
			base.OnDispose();
		}
	}
}