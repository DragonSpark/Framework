using System;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;
using Xamarin.Forms;

namespace DragonSpark.Application.Client
{
	public class FormsEvent<T> : PubSubEvent<T> where T : class
	{
		protected string GetMessageName()
		{
			var result = typeof(T).FromMetadata<MessageNameAttribute, string>( attribute => attribute.Name ) ?? typeof(T).FullName;
			return result;
		}

		public override void Publish( T payload )
		{
			base.Publish( payload );

			var name = GetMessageName();
			Send( payload, name );
		}

		protected  virtual void Send( T payload, string name )
		{
			MessagingCenter.Send( payload, name );
		}

		public SubscriptionToken Subscribe( object subscriber, Action<T> action, T source = null, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = false, Predicate<T> filter = null )
		{
			OnSubscribe( subscriber, action, source );

			var result = Subscribe( action, ThreadOption.PublisherThread, keepReferenceAlive, filter );
			return result;
		}

		protected virtual void OnSubscribe( object subscriber, Action<T> action, T source )
		{
			var name = GetMessageName();
			MessagingCenter.Subscribe( subscriber, name, action, source );
		}

		public void Unsubscribe( object subscriber, Action<T> action )
		{
			OnUnsubscribe( subscriber );
			Unsubscribe( action );
		}

		protected virtual void OnUnsubscribe( object subscriber )
		{
			var name = GetMessageName();
			MessagingCenter.Unsubscribe<T>( subscriber, name );
		}

		public void Unsubscribe( object subscriber, SubscriptionToken token )
		{
			OnUnsubscribe( subscriber );
			Unsubscribe( token );
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

	public class AplicationInitializedEvent : FormsEvent<System.Windows.Application>
	{}
}