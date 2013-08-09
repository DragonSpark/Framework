using Microsoft.Practices.Unity;

namespace DragonSpark.Testing.Framework.Testing
{
	public class SimpleObject
	{
		[Dependency]
		public string HelloWorld { get; set; }
	}
}