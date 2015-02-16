using System.Collections.Generic;
using Microsoft.Practices.Prism.PubSubEvents;

namespace DragonSpark.Application.Client.Launch
{
	public class ApplicationLaunchEvent : PubSubEvent<ApplicationLaunchStatus>
	{
		readonly IList<ApplicationLaunchStatus> history = new List<ApplicationLaunchStatus>();

		public override void Publish( ApplicationLaunchStatus payload )
		{
			base.Publish( payload );
			history.Add( payload );
		}

		public IEnumerable<ApplicationLaunchStatus> History
		{
			get { return history; }
		}
	}
}