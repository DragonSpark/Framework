using System;
using System.Collections.Generic;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	internal class MockContainerAdapter : ServiceLocatorImplBase
	{
		readonly Dictionary<Type, object> resolvedInstances = new Dictionary<Type, object>();

		protected override object DoGetInstance(Type serviceType, string key)
		{
			var result = resolvedInstances.Ensure( serviceType, System.Activator.CreateInstance );
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			throw new NotImplementedException();
		}
	}
}