namespace DragonSpark.Server.Legacy.Configuration
{
	public class RequestLifetimeManager : LifetimeManagerBase<IoC.RequestLifetimeManager>
	{
		public string KeyName { get; set; }

		protected override System.Collections.Generic.IEnumerable<object> Parameters
		{
			get { yield return KeyName; }
		}
	}
}