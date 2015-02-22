using System;
using System.Runtime.CompilerServices;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.PubSubEvents;

namespace DragonSpark.Application
{
	public static class EventSupport
	{
		public static Action<TContext> Create<TContext>( Action<TContext> action )
		{
			var context = new ExecutionContext<TContext>( action );
			Action<TContext> result = context.Execute;
			return result;
		}

		public class ExecutionContext<TContext>
		{
			readonly Action<TContext> action;

			public ExecutionContext( Action<TContext> action )
			{
				this.action = action;
			}

			public void Execute( TContext context )
			{
				action( context );
			}
		}

		public class SubscriptionContext<TEvent,TPayload> where TEvent : PubSubEvent<TPayload>, new()
		{
			readonly TEvent @event;
			readonly Func<TEvent, TPayload, bool> unsubscribeDelegate;
			
			internal SubscriptionContext( TEvent @event, Func<TEvent,TPayload,bool> unsubscribeDelegate )
			{
				this.@event = @event;
				this.unsubscribeDelegate = unsubscribeDelegate;               
			}

			public void Initialize( ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = true )
			{
				Token = @event.Subscribe( Execute, option, keepReferenceAlive );
			}

			SubscriptionToken Token { get; set; }

			[MethodImpl( MethodImplOptions.Synchronized )]
			public void Execute( TPayload payload )
			{
				Token.NotNull( x =>
				{
					var unsubscribe = unsubscribeDelegate( @event, payload );
					unsubscribe.IsTrue( () =>
					{
						@event.Unsubscribe( Token );
						Token = null;
					} );
				} );
			}
		}

		public static void Subscribe<TEvent,TPayload>( this IEventAggregator target, Func<TEvent,TPayload,bool> unsubscribeDelegate, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = true ) where TEvent : PubSubEvent<TPayload>, new()
		{
			var e = target.GetEvent<TEvent>();
			var context = new SubscriptionContext<TEvent, TPayload>( e, unsubscribeDelegate );
			context.Initialize( option, keepReferenceAlive );
		}
	}
}