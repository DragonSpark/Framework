using DragonSpark.Extensions;

namespace DragonSpark.Application.Setup
{
	public static class EventListenerRegistryExtensions
	{
		public static void EnableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => EventListenerExtensions.ListenTo( x, y.Key, y.Value.Level, y.Value.Keywords ) ) );
		}

		public static void DisableAll( this IEventListenerRegistry target )
		{
			target.GetAll().Apply( x => target.Retrieve( x ).Apply( y => x.Ignore( y.Key ) ) );
		}
	}
}