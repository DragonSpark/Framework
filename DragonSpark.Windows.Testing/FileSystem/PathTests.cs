using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class PathTests
	{
		[Fact]
		public void Verify()
		{
			Path.Default.Assign( Scopes.ToSingleton( () => new Mock<MockPath> { CallBase = true }.Object ) );

			var implementation = Path.Default.Get();
			Assert.Same( Path.Default.Get(), implementation );
			var mock = Mock.Get( (MockPath)implementation );
			var instance = Path.Default.Get();
			Assert.Same( Path.Default.Get(), instance  );
			mock.Verify( i => i.GetRandomFileName(), Times.Never() );
			Assert.NotEmpty( instance.GetRandomFileName() );
			mock.Verify( i => i.GetRandomFileName(), Times.Once() );
		}
	}
}