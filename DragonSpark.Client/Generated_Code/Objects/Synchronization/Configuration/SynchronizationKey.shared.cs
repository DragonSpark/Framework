using System;
using DragonSpark.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public class SynchronizationKey : IInstanceSource<Synchronization.SynchronizationKey>
	{
		bool created;

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type First { get; set; }

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type Second { get; set; }

		public string KeyName { get; set; }

		public Synchronization.SynchronizationKey Instance
		{
			get { return !created && ( created = true ) ? ( instance = Create() ) : instance; }
		}	Synchronization.SynchronizationKey instance;

		protected virtual Synchronization.SynchronizationKey Create()
		{
			var result = new Synchronization.SynchronizationKey( First, Second, KeyName );
			return result;
		}
	}
}