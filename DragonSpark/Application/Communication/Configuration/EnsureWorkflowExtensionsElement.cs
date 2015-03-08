using System;
using System.ServiceModel.Configuration;

namespace DragonSpark.Application.Communication.Configuration
{
	public class EnsureWorkflowExtensionsElement : BehaviorExtensionElement
	{
		protected override object CreateBehavior()
		{
			var result = new EnsureWorkflowExtensionsBehavior();
			return result;
		}

		public override Type BehaviorType
		{
			get { return typeof(EnsureWorkflowExtensionsBehavior); }
		}
	}
}