using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public partial class NotificationCollection<TItem> : ViewCollection<TItem>
	{
		readonly Action<string> action;
		readonly string name;

		public NotificationCollection( Action<string> action, string name ) : this( Enumerable.Empty<TItem>(), action, name )
		{}

		public NotificationCollection( IEnumerable<TItem> collection, Action<string> action, string name ) : base( collection )
		{
			this.action = action;
			this.name = name;
			collection.As<INotifyCollectionChanged>( x =>
			{
			    x.CollectionChanged += OnInnerCollectionChanged;
			} );
		}

		void OnInnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			sender.As<IEnumerable<TItem>>( x =>
			{
			    ClearItems();
			    AddRange( x );
			} );
		}
	}
}