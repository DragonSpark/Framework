using System;
using System.ServiceModel.Configuration;

namespace DragonSpark.Application.Communication.Configuration
{
	public class ServiceHostLifetimeElement : BehaviorExtensionElement
	{
		protected override object CreateBehavior()
		{
			var result = new ServiceLifetimeBehavior( UnityServiceHostBaseExtension.Instance );
			return result;
		}

		public override Type BehaviorType
		{
			get { return typeof(ServiceLifetimeBehavior); }
		}
	}
}