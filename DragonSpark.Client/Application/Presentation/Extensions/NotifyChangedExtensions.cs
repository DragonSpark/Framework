using System.Collections.Specialized;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static partial class NotifyChangedExtensions
	{
		partial class NotificationContext
		{
			internal void CollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
			{
				switch ( e.Action )
				{
					case NotifyCollectionChangedAction.Add:
					case NotifyCollectionChangedAction.Remove:
					case NotifyCollectionChangedAction.Reset:
						target.NotifyOfPropertyChange( name );
						break;
				}
			}
		}

	}
}