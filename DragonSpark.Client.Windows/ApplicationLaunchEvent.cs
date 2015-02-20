using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Common.Application;

namespace DragonSpark.Client.Windows
{
	public enum ApplicationLaunchStatus
	{
		Initializing, Initialized, Loading, Loaded, Complete
	}
	public static class ApplicationLaunchEventExtensions
	{
		public static bool ExecuteWhenStatusIs( this IEventAggregator target, ApplicationLaunchStatus status, Action action )
		{
			var ready = target.GetEvent<ApplicationLaunchEvent>().History.Transform( y => y.Contains( status ) );
			if ( ready )
			{
				action();
			}
			else
			{
				target.Subscribe<ApplicationLaunchEvent, ApplicationLaunchStatus>( ( e, p ) =>
				{
					var result = p == status;
					result.IsTrue( action );
					return result;
				} );
			}
			return ready;
		}
	}

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