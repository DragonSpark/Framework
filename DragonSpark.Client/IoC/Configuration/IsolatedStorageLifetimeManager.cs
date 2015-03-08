using DragonSpark.Objects.Synchronization;

namespace DragonSpark.IoC.Configuration
{
	public class IsolatedStorageLifetimeManager : LifetimeManagerBase<IoC.IsolatedStorageLifetimeManager>
	{
		public string KeyName { get; set; }

		public IsolatedStorageLifetimeDisposeAction DisposeAction { get; set; }

		protected override Microsoft.Practices.Unity.LifetimeManager Create()
		{
			var result = base.Create().SynchronizeFrom( this );
			return result;
		}

		protected override System.Collections.Generic.IEnumerable<object> Parameters
		{
			get { yield return KeyName; }
		}
	}
}