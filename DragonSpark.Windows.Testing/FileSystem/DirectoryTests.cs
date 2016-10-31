using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class DirectoryTests
	{
		[Fact]
		public void Verify()
		{
			Directory.DefaultImplementation.Implementation.Assign( Sources.Factory.Cache(  () => new Mock<MockDirectory> { CallBase = true }.Object ) );
			var implementation = Directory.DefaultImplementation.Implementation.Get();
			Assert.Same( Directory.DefaultImplementation.Implementation.Get(), implementation );
			var mock = Mock.Get( (MockDirectory)implementation );
			var instance = Directory.Current.Get();
			Assert.Same( instance, Directory.Current.Get() );
			mock.Verify( i => i.GetCurrentDirectory(), Times.Never() );

			var item = instance.GetCurrentDirectory();
			Assert.NotEmpty( item );
			mock.Verify( i => i.GetCurrentDirectory(), Times.Once );
		}
	}
}