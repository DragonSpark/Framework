using System.Collections.Specialized;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	partial class NotificationCollection<TItem>
	{
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			switch ( e.Action )
			{
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Remove:
					action.NotNull( x => x( name ) );
					break;
			}
			base.OnCollectionChanged(e);
		}

		/*partial void OnInnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			sender.As<IEnumerable<TItem>>( x =>
			{
			    ClearItems();
			    AddRange( x );
			} );
		}*/
	}
}