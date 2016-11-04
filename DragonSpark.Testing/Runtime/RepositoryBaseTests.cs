using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class RepositoryBaseTests
	{
		[Fact]
		public void Coverage()
		{
			var instance = new object();
			Repository.Default.Add( instance );
			Assert.Contains( instance, Repository.Default );
		}

		sealed class Repository : RepositoryBase<object>
		{
			public static Repository Default { get; } = new Repository();
			Repository() {}
		}
	}
}