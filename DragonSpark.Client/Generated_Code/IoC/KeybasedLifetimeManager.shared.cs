using System;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public abstract class KeyBasedLifetimeManager : LifetimeManager, IDisposable 
	{
		readonly string key;

		protected KeyBasedLifetimeManager( string key )
		{
			this.key = key;
		}

		public void Dispose() 
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			RemoveValue(); 
		}

		protected string Key
		{
			get { return key; }
		}
	}
}