using DragonSpark.Activation;
using DragonSpark.Extensions;
using Prism.Events;
using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Application
{
	public static class Events
	{
		public static TEvent Event<TEvent>( this object @this ) where TEvent : EventBase, new()
		{
			var result = With<TEvent>( null );
			return result;
		}

		public static TEvent With<TEvent>( Action<TEvent> action ) where TEvent : EventBase, new()
		{
			var result = Services.With<IEventAggregator, TEvent>( aggregator => aggregator.GetEvent<TEvent>().With( action ) );
			return result;
		}

		public static void Publish<TEvent, TPayload>( TPayload payload ) where TEvent : PubSubEvent<TPayload>, new()
		{
			With<TEvent>( @event => @event.Publish( payload ) );
		}

		public static SubscriptionToken SubscribeUntil<TEvent, TPayload>( this IEventAggregator target, Func<TEvent, TPayload, bool> until, ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = true ) 
			where TEvent : PubSubEvent<TPayload>, new()
		{
			var e = target.GetEvent<TEvent>();
			var context = new SubscriptionContext<TEvent, TPayload>( e, until );
			var result = context.Initialize( option, keepReferenceAlive );
			return result;
		}

		/*public static Action<TContext> Create<TContext>( Action<TContext> action )
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
		}*/

		class SubscriptionContext<TEvent, TPayload> where TEvent : PubSubEvent<TPayload>, new()
		{
			readonly TEvent @event;
			readonly Func<TEvent, TPayload, bool> unsubscribeDelegate;
			
			internal SubscriptionContext( TEvent @event, Func<TEvent,TPayload,bool> unsubscribeDelegate )
			{
				this.@event = @event;
				this.unsubscribeDelegate = unsubscribeDelegate;               
			}

			public SubscriptionToken Initialize( ThreadOption option = ThreadOption.PublisherThread, bool keepReferenceAlive = false )
			{
				Token = Token ?? @event.Subscribe( Execute, option, keepReferenceAlive );
				return Token;
			}

			SubscriptionToken Token { get; set; }

			[MethodImpl( MethodImplOptions.Synchronized )]
			void Execute( TPayload payload )
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
	}
}