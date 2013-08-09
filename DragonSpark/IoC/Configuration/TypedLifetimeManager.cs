using System;
using DragonSpark.Extensions;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.IoC.Configuration
{
	public class TypedLifetimeManager : LifetimeManager
	{
		public Type LifetimeManagerType { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "This is the factory method.  The result is disposed elsewhere." )]
		protected override Microsoft.Practices.Unity.LifetimeManager Create()
		{
			var result = LifetimeManagerType.Transform( item => Activator.CreateInstance<Microsoft.Practices.Unity.LifetimeManager>( LifetimeManagerType ), () => new Microsoft.Practices.Unity.ContainerControlledLifetimeManager() ) ;
			return result;
		}
	}
}