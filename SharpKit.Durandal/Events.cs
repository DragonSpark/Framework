using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;

namespace SharpKit.Durandal
{
	public class Chainable
	{}

	/// <summary>
	/// Durandal events originate from backbone.js but also combine some ideas from signals.js as well as some additional improvements.
	/// Events can be installed into any object and are installed into the `app` module by default for convenient app-wide eventing.
	/// </summary>
	[Module( LifetimeMode.PerRequest, Export = false )]
	public class Events : Chainable
	{
		/// <summary>
		/// Represents an event subscription.
		/// </summary>
		public class Subscription : Chainable
		{
			readonly object owner;
			readonly Events events;

			public Subscription( object owner, Events events )
			{
				this.owner = owner;
				this.events = events;
			}

			/// <summary>
			/// Attaches a callback to the event subscription.
			/// </summary>
			/// <param name="callback">The callback function to invoke when the event is triggered.</param>
			/// <param name="context">An object to use as `this` when invoking the `callback`.</param>
			/// <returns>Chainable</returns>
			public Chainable Then( JsFunction callback, object context )
			{
				return this;
			}

			/// <summary>
			/// Attaches a callback to the event subscription.
			/// </summary>
			/// <param name="callback">The callback function to invoke when the event is triggered. If `callback` is not provided, the previous callback will be re-activated.</param>
			/// <param name="context">An object to use as `this` when invoking the `callback`.</param>
			/// <returns></returns>
			public Chainable On( JsFunction callback, object context )
			{
				return this;
			}

			/// <summary>
			/// Cancels the subscription.
			/// </summary>
			/// <returns>Chainable</returns>
			public Chainable Off()
			{
				return this;
			}
		}

		/// <summary>
		/// Creates a subscription or registers a callback for the specified event.
		/// </summary>
		/// <param name="events">One or more events, separated by white space.</param>
		/// <param name="callback">The callback function to invoke when the event is triggered. If `callback` is not provided, a subscription instance is returned.</param>
		/// <param name="context">An object to use as `this` when invoking the `callback`.</param>
		/// <returns>A subscription is returned if no callback is supplied, otherwise the events object is returned for chaining.</returns>
		public Chainable On( string events, JsFunction callback, object context )
		{
			return this;
		}

		/// <summary>
		/// Removes the callbacks for the specified events.
		/// </summary>
		/// <param name="events">One or more events, separated by white space to turn off. If no events are specified, then the callbacks will be removed.</param>
		/// <param name="callback">The callback function to remove. If `callback` is not provided, all callbacks for the specified events will be removed.</param>
		/// <param name="context">The object that was used as `this`. Callbacks with this context will be removed.</param>
		/// <returns>Chainable</returns>
		public Chainable Off( string events, JsFunction callback, object context )
		{
			return this;
		}

		/// <summary>
		/// Triggers the specified events.
		/// </summary>
		/// <param name="events">One or more events, separated by white space to trigger.</param>
		/// <param name="arguments"></param>
		/// <returns>Chainable</returns>
		public Chainable Trigger( string events, params object[] arguments )
		{
			return this;
		}

		/// <summary>
		/// Creates a function that will trigger the specified events when called. Simplifies proxying jQuery (or other) events through to the events object.
		/// </summary>
		/// <param name="events">One or more events, separated by white space to trigger by invoking the returned function.</param>
		/// <returns>Calling the function will invoke the previously specified events on the events object.</returns>
		public JsFunction Proxy( string events )
		{
			return default(JsFunction);
		}

		/// <summary>
		/// Adds eventing capabilities to the specified object.
		/// </summary>
		/// <param name="targetObject">The object to add eventing capabilities to.</param>
		public static void IncludeIn( object targetObject )
		{}
	}
}