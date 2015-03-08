using System;
using System.Globalization;
using System.ServiceModel;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication.Configuration
{
	/// <summary>
	/// Abstract base class for Unity WCF lifetime support.
	/// </summary>
	/// <typeparam name="T">IExtensibleObject for which to register Unity lifetime manager support.</typeparam>
	public abstract class UnityWcfLifetimeManager<T> : SynchronizedLifetimeManager
		where T : IExtensibleObject<T>
	{
		/// <summary>
		/// Key for Unity to use for the associated object's instance.
		/// </summary>
		readonly Guid _key = Guid.NewGuid();

		/// <summary>
		/// Gets the currently registered UnityWcfExtension for this lifetime manager.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "UnityWcfExtension")]
		UnityWcfExtension<T> Extension
		{
			get
			{
				var extension = FindExtension();
				if ( extension == null )
				{
					throw new InvalidOperationException(
						string.Format( CultureInfo.CurrentCulture, "UnityWcfExtension<{0}> must be registered in WCF.", typeof(T).Name ) );
				}

				return extension;
			}
		}

		public Guid? InstanceKey { get; set; }

		/// <summary>
		/// Gets the object instance for the given key from the currently registered extension.
		/// </summary>
		/// <returns>The object instance associated with the given key.  If no instance is registered, null is returned.</returns>
		protected override object SynchronizedGetValue()
		{
			return Extension.FindInstance( InstanceKey ?? _key );
		}

		/// <summary>
		/// Associates the supplied object instance with the given key in the currently registered extension.
		/// </summary>
		/// <param name="newValue">The object to register in the currently registered extension.</param>
		protected override void SynchronizedSetValue( object newValue )
		{
			Extension.RegisterInstance( InstanceKey ?? _key, newValue );
		}
		
		/// <summary>
		/// Removes the object instance for the given key from the currently registered extension.
		/// </summary>
		public override void RemoveValue()
		{
			// Not called, but just in case.
			Extension.RemoveInstance( InstanceKey ?? _key );
		}

		/// <summary>
		/// Finds the currently registered UnityWcfExtension for this lifetime manager.
		/// </summary>
		/// <returns>Currently registered UnityWcfExtension of the given type, or null if no UnityWcfExtension is registered.</returns>
		protected abstract UnityWcfExtension<T> FindExtension();
	}
}