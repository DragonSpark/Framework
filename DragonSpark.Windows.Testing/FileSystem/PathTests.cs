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
			Path.DefaultImplementation.Implementation.Assign( Sources.Factory.Cache( () => new Mock<MockPath> { CallBase = true }.Object ) );

			var implementation = Path.DefaultImplementation.Implementation.Get();
			Assert.Same( Path.DefaultImplementation.Implementation.Get(), implementation );
			var mock = Mock.Get( (MockPath)implementation );
			var instance = Path.Current.Get();
			Assert.Same( Path.Current.Get(), instance  );
			mock.Verify( i => i.GetRandomFileName(), Times.Never() );
			Assert.NotEmpty( instance.GetRandomFileName() );
			mock.Verify( i => i.GetRandomFileName(), Times.Once() );
		}
	}
}