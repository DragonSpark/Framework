using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation.IoC
{
	public class CompositeServiceLocator : ServiceLocatorImplBase
	{
		readonly IEnumerable<IServiceLocator> locators;

		public CompositeServiceLocator( params IServiceLocator[] locators )
		{
			this.locators = locators;
		}

		protected override object DoGetInstance( Type serviceType, string key )
		{
			var result = locators.Select( locator => locator.GetInstance( serviceType, key ) ).FirstOrDefault( x => x != null );
			return result;
		}

		protected override IEnumerable<object> DoGetAllInstances( Type serviceType )
		{
			var result = locators.SelectMany( x => x.GetAllInstances( serviceType ) );
			return result;
		}
	}
}