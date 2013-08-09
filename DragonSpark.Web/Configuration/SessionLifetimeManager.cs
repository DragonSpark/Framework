
using DragonSpark.IoC.Configuration;

namespace DragonSpark.Web.Configuration
{
	public class SessionLifetimeManager : LifetimeManagerBase<IoC.SessionLifetimeManager>
	{
		public string KeyName { get; set; }

		protected override System.Collections.Generic.IEnumerable<object> Parameters
		{
			get { yield return KeyName; }
		}
	}
}