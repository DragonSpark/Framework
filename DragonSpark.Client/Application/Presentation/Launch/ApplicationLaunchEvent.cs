using System.Collections.Generic;
using Microsoft.Practices.Prism.Events;

namespace DragonSpark.Application.Presentation.Launch
{
	public class ApplicationLaunchEvent : CompositePresentationEvent<ApplicationLaunchStatus>
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