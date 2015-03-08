﻿using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace DragonSpark.Application.Communication.Configuration
{
	public class UnityWcfExtension<T> : IExtension<T>
		where T : IExtensibleObject<T>
	{
		/// <summary>
		/// Backing store for relating keys to object instances for Unity.
		/// </summary>
		readonly Dictionary<Guid, object> instances = new Dictionary<Guid, object>();

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( instances != null );
		}*/

		/// <summary>
		/// Enables an extension object to find out when it has been aggregated. Called when the extension is added to the IExtensibleObject.Extensions property.
		/// </summary>
		/// <param name="owner">The extensible object that aggregates this extension.</param>
		public void Attach( T owner )
		{}

		/// <summary>
		/// Enables an object to find out when it is no longer aggregated. Called when an extension is removed from the IExtensibleObject.Extensions property.
		/// </summary>
		/// <param name="owner">The extensible object that aggregates this extension.</param>
		public void Detach( T owner )
		{
			// If we are being detached, let's go ahead and clean up, just in case.
			var keys = new List<Guid>( instances.Keys );

			foreach ( var t in keys )
			{
				RemoveInstance( t );
			}
		}

		/// <summary>
		/// Registers the given instance with the given key into the backing store.
		/// </summary>
		/// <param name="key">Key to associate with the object instance.</param>
		/// <param name="value">Object instance to associate with the given key in the backing store.</param>
		public void RegisterInstance( Guid key, object value )
		{
			if ( value == null )
			{
				throw new ArgumentNullException( "value" );
			}

			instances.Add( key, value );
		}

		/// <summary>
		/// Finds the object associated with the given key.
		/// </summary>
		/// <param name="key">Key used to find the associated object instance.</param>
		/// <returns>The object instance associated with the supplied key.  If no instance is registered, null is returned.</returns>
		public object FindInstance( Guid key )
		{
			object obj;

			// We don't care whether or not this succeeds or fails.
			instances.TryGetValue( key, out obj );
			return obj;
		}

		/// <summary>
		/// Removes the given key from the backing store.  This method will also dispose of the associated object instance if it implements <see cref="System.IDisposable"/>.
		/// </summary>
		/// <param name="key">Key to remove from the backing store.</param>
		public void RemoveInstance( Guid key )
		{
			// We don't want to use FindInstance JUST IN CASE somehow a key gets in there with a null object.

			if ( instances.ContainsKey( key ) )
			{
				// Get the instance.
				var instance = instances[ key ];

				// See if it needs disposing.
				var disposable = instance as IDisposable;
				if ( disposable != null )
				{
					disposable.Dispose();
				}

				// Remove it from the instances list.
				instances.Remove( key );
			}
		}
	}
}