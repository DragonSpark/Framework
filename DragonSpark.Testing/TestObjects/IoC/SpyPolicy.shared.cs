namespace DragonSpark.Testing.TestObjects.IoC
{
	/// <summary>
	/// A sample policy that gets used by the SpyStrategy
	/// if present to mark execution.
	/// </summary>
	public class SpyPolicy : ISpyPolicy
	{
		public bool Enabled { get; set; }
	}
}