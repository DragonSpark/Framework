using Prism.Events;
using System;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Eventing
{
	public abstract class FormsEvent<TSender, TArgument> : FormsEventBase<FormsMessage<TSender, TArgument>> where TSender : class
	{
		public void Publish( TSender sender, TArgument argument )
		{
			Publish( new FormsMessage<TSender, TArgument>( sender, argument ) );
		}

		public SubscriptionToken Subscribe( object subscriber, Action<TSender, TArgument> action, TSender source = null, Predicate<FormsMessage<TSender, TArgument>> filter = null )
		{
			var result = Subscribe( subscriber, message => action( message.Sender, message.Argument ), source, true, filter );
			return result;
		}

		public SubscriptionToken Subscribe( object subscriber, Action<FormsMessage<TSender, TArgument>> action, TSender source = null, bool keepReferenceAlive = false, Predicate<FormsMessage<TSender, TArgument>> filter = null )
		{
			var subscription = Create( subscriber, action, keepReferenceAlive, filter );
			var result = InternalSubscribe( subscription );

			Action<TSender, TArgument> @delegate = ( sender, argument ) =>
			{
				action( new FormsMessage<TSender, TArgument>( sender, argument ) );
			};

			var messageName = GetMessageName();
			MessagingCenter.Subscribe( subscription.Subscriber, messageName, @delegate, source );

			return result;
		}

		protected override void Publish( FormsMessage<TSender, TArgument> sender, string name )
		{
			MessagingCenter.Send( sender.Sender, name, sender.Argument );
		}

		protected override void OnUnsubscribe( FormsEventSubscription<FormsMessage<TSender, TArgument>> subscription, string name )
		{
			MessagingCenter.Unsubscribe<TSender>( subscription.Subscriber, name );
		}
	}

	public abstract class FormsEvent<TSender> : FormsEventBase<TSender> where TSender : class
	{
		public SubscriptionToken Subscribe( object subscriber, Action<TSender> action, TSender source = null, bool keepReferenceAlive = false, Predicate<TSender> filter = null )
		{
			var subscription = Create( subscriber, action, keepReferenceAlive, filter );
			var result = InternalSubscribe( subscription );

			MessagingCenter.Subscribe( subscription.Subscriber, GetMessageName(), action, source );

			return result;
		}

		protected override void Publish( TSender sender, string name )
		{
			MessagingCenter.Send( sender, name );
		}

		protected override void OnUnsubscribe( FormsEventSubscription<TSender> subscription, string name )
		{
			MessagingCenter.Unsubscribe<TSender>( subscription.Subscriber, name );
		}
	}
}