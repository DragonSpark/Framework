using System;
using System.Runtime.CompilerServices;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Events;

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

        public class SubscriptionContext<TEvent,TPayload> where TEvent : CompositePresentationEvent<TPayload>, new()
        {
            readonly TEvent @event;
            readonly Func<TEvent, TPayload, bool> unsubscribeDelegate;
			
            internal SubscriptionContext( TEvent @event, Func<TEvent,TPayload,bool> unsubscribeDelegate, ThreadOption option = ThreadOption.UIThread, bool keepReferenceAlive = true )
            {
                this.@event = @event;
                this.unsubscribeDelegate = unsubscribeDelegate;
				
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

        public static void Subscribe<TEvent,TPayload>( this IEventAggregator target, Func<TEvent,TPayload,bool> unsubscribeDelegate, ThreadOption option = ThreadOption.UIThread, bool keepReferenceAlive = true ) where TEvent : CompositePresentationEvent<TPayload>, new()
        {
            var e = target.GetEvent<TEvent>();
            new SubscriptionContext<TEvent, TPayload>( e, unsubscribeDelegate, option, keepReferenceAlive ); // Hm.
        }
    }
}