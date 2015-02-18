using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Server.Legacy.Communication
{
	public class ServiceLocationRegistry
	{
		readonly IDictionary<Type, IServiceLocator> cache = new Dictionary<Type, IServiceLocator>();

		ServiceLocationRegistry()
		{}

		[MethodImpl( MethodImplOptions.Synchronized )]
		public IServiceLocator Retrieve<TConfiguration>() where TConfiguration : ServiceLocatorFactory, new()
		{
			var result = cache.Ensure( typeof(TConfiguration), type => new TConfiguration().Instance );
			return result;
		}

		public static ServiceLocationRegistry Instance
		{
			get { return InstanceField; }
		}	static readonly ServiceLocationRegistry InstanceField = new ServiceLocationRegistry();
	}
}
