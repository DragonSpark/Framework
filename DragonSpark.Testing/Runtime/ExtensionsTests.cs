using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class ExtensionsTests
	{
		[Fact]
		public void Coverage()
		{
			var entry = new object();
			var sut = Repository.Default.Registered( entry );
			Assert.Same( entry, sut );
		}

		sealed class Repository : RepositoryBase<object>
		{
			public static Repository Default { get; } = new Repository();
			Repository() {}
		}
	}
}