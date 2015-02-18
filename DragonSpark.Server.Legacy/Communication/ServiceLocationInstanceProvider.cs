using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Server.Legacy.Communication
{
	public class ServiceLocationInstanceProvider : IInstanceProvider
	{
		readonly IServiceLocator locator;
		readonly Type type;

		public ServiceLocationInstanceProvider( IServiceLocator locator, Type type )
		{
			this.locator = locator;
			this.type = type;
		}

		#region IInstanceProvider Members
		public object GetInstance( InstanceContext instanceContext, Message message )
		{
			return GetInstance( instanceContext );
		}

		public object GetInstance( InstanceContext instanceContext )
		{
			return locator.GetInstance( type );
		}

		public void ReleaseInstance( InstanceContext instanceContext, object instance )
		{}
		#endregion
	}
}