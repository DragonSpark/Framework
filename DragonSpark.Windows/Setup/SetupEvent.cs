﻿namespace DragonSpark.Windows.Setup
{
	/*public static class SetupEventExtensions
	{
		public static bool ExecuteWhenStatusIs( this IEventAggregator target, SetupStatus status, Action action )
		{
			var history = target.GetEvent<SetupEvent>().History;
			var ready = history.Transform( y => y.Contains( status ) );
			if ( ready )
			{
				action();
			}
			else
			{
				target.SubscribeUntil<SetupEvent, SetupStatus>( ( e, p ) =>
				{
					var result = p == status;
					result.IsTrue( action );
					return result;
				} );
			}
			return ready;
		}
	}

	public class SetupEvent : PubSubEvent<SetupStatus>
	{
		readonly IList<SetupStatus> history = new List<SetupStatus>();

		public override void Publish( SetupStatus payload )
		{
			base.Publish( payload );
			history.Add( payload );
		}

		public IEnumerable<SetupStatus> History
		{
			get { return history; }
		}
	}*/
}