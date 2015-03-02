using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Linq;
using System.Threading.Tasks;
using PostSharp.Patterns.Threading;

namespace DragonSpark.Application.Client.Eventing
{
	public class FormsEventSubscription<T> : EventSubscription<T>
	{
		readonly WeakReference<object> subscriber;

		public FormsEventSubscription( object subscriber, IDelegateReference actionReference, IDelegateReference filterReference ) : base( actionReference, filterReference )
		{
			this.subscriber = new WeakReference<object>( subscriber );
		}

		public object Subscriber
		{
			get
			{
				object result;
				return subscriber.TryGetTarget( out result ) ? result : null;
			}
		}
	}

	public abstract class FormsEventBase<TSender> : PubSubEvent<TSender>
	{
		protected string GetMessageName()
		{
			var result = this.FromMetadata<MessageNameAttribute, string>( attribute => attribute.Name ) ?? GetType().FullName;
			return result;
		}

		public override void Publish( TSender sender )
		{
			var name = GetMessageName();
			Publish( sender, name );
		}

		protected abstract void Publish( TSender sender, string name );

		protected FormsEventSubscription<TSender> Create( object subscriber, Action<TSender> action, bool keepReferenceAlive, Predicate<TSender> filter )
		{
			var actionReference = new DelegateReference( action, keepReferenceAlive );
			var filterReference = new DelegateReference( filter ?? ( obj => true ), filter == null || keepReferenceAlive );
			var subscription = new FormsEventSubscription<TSender>( subscriber, actionReference, filterReference );
			return subscription;
		}

		FormsEventSubscription<TSender> GetSubscription( SubscriptionToken token )
		{
			var result = Subscriptions.Cast<FormsEventSubscription<TSender>>().SingleOrDefault( eventSubscription => Equals( eventSubscription.SubscriptionToken, token ) ).InvalidIfNull( string.Format( "A subscription was not found with the specified token '{0}'.", token.GetHashCode() ) );
			return result;
		}

		public override SubscriptionToken Subscribe( Action<TSender> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TSender> filter )
		{
			throw new InvalidOperationException( "This event requires a subscriber.  Use Subscribe( object subscriber, Action<T> action, T source = null, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = false, Predicate<T> filter = null ) instead." );
		}

		/*protected abstract void OnSubscribe(  );*/

		protected void Cleanup()
		{
			Unsubscribe( subscription => subscription.Subscriber == null );
		}

		public override void Unsubscribe( Action<TSender> action )
		{
			Unsubscribe( subscription => subscription.Action == action );
		}

		public void Unsubscribe( object subscriber )
		{
			Unsubscribe( subscription => subscription.Subscriber == subscriber );
		}

		protected virtual void Unsubscribe( Func<FormsEventSubscription<TSender>, bool> predicate )
		{
			var core = Subscriptions.Cast<FormsEventSubscription<TSender>>().ToArray();
			var events = core.Where( predicate ).Concat( core.Where( subscription => subscription.Subscriber == null ) ).Distinct();
			events.Apply( subscription => Unsubscribe( subscription.SubscriptionToken ) );
		}

		public override void Unsubscribe( SubscriptionToken token )
		{
			var name = GetMessageName();
			var subscription = GetSubscription( token );
			
			OnUnsubscribe( subscription, name );

			base.Unsubscribe( token );
		}

		[Dispatched( true )] // Might be unsubscribing during a publish call... might be happening on the same thread as MessageCenter and it throws an error if so.
		protected abstract void OnUnsubscribe( FormsEventSubscription<TSender> subscription, string name );

		protected override void InternalPublish( params object[] arguments )
		{
			Cleanup();
			base.InternalPublish( arguments );
		}
	}
}