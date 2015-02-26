using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonSpark.Application.Client
{
	public class FormsEvent<T> : PubSubEvent<T> where T : class
	{
		readonly ConditionalWeakTable<IEventSubscription, object> cache = new ConditionalWeakTable<IEventSubscription, object>(); 

		protected string GetMessageName()
		{
			var result = this.FromMetadata<MessageNameAttribute, string>( attribute => attribute.Name ) ?? GetType().FullName;
			return result;
		}

		public override void Publish( T payload )
		{
			var name = GetMessageName();
			Send( payload, name );
		}

		protected virtual void Send( T payload, string name )
		{
			MessagingCenter.Send( payload, name );
		}

		public SubscriptionToken Subscribe( object subscriber, Action<T> action, T source = null, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = false, Predicate<T> filter = null )
		{
			var result = base.Subscribe( action, option, keepReferenceAlive, filter );

			var subscription = GetSubscription( result );
			cache.Add( subscription, subscriber );

			OnSubscribe( subscriber, action, source );

			return result;
		}

		IEventSubscription GetSubscription( SubscriptionToken token )
		{
			var result = Subscriptions.SingleOrDefault( eventSubscription => Equals( eventSubscription.SubscriptionToken, token ) ).InvalidIfNull( string.Format( "A subscription was not found with the specified token '{0}'.", token.GetHashCode() ) );
			return result;
		}

		public override SubscriptionToken Subscribe( Action<T> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<T> filter )
		{
			throw new InvalidOperationException( "This event requires a subscriber.  Use Subscribe( object subscriber, Action<T> action, T source = null, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = false, Predicate<T> filter = null ) instead." );
			// return base.Subscribe( action, threadOption, keepSubscriberReferenceAlive, filter );
		}

		protected virtual void OnSubscribe( object subscriber, Action<T> action, T source )
		{
			var name = GetMessageName();
			MessagingCenter.Subscribe( subscriber, name, action, source );
		}

		public override void Unsubscribe( SubscriptionToken token )
		{
			var subscription = GetSubscription( token );
			object subscriber;
			if ( !cache.TryGetValue( subscription, out subscriber ) )
			{
				throw new InvalidOperationException( string.Format( "A subscriber was not found with the specified token '{0}'", token.GetHashCode() ) );
			}
			cache.Remove( subscription );
			OnUnsubscribe( subscriber );

			base.Unsubscribe( token );
		}

		public override void Unsubscribe( Action<T> subscriber )
		{
			var events = Subscriptions.Cast<EventSubscription<T>>().Where( subscription => subscription.Action == subscriber ).ToArray();
			events.Apply( subscription => Unsubscribe( subscription.SubscriptionToken ) );
		}

		protected virtual void OnUnsubscribe( object subscriber )
		{
			// TODO: wrap/monitor task exception.
			Task.Run( () => // Might be unsubscribing during a publish call... might be happening on the same thread as MessageCenter and it throws an error if so.
			{
				var name = GetMessageName();
				MessagingCenter.Unsubscribe<T>( subscriber, name );
			} );
		}
	}

	[AttributeUsage( AttributeTargets.Class )]
	public class MessageNameAttribute : Attribute
	{
		readonly string name;

		public MessageNameAttribute( string name )
		{
			this.name = name;
		}

		public string Name
		{
			get { return name; }
		}
	}

	public class ShellInitializedEvent : FormsEvent<System.Windows.Application>
	{}

	public class ShellPageChangedEvent : FormsEvent<Page>
	{}
}