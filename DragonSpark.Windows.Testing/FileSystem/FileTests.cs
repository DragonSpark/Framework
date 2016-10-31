using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using System.Text;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class FileTests
	{
		[Theory, AutoData]
		public void Verify( string path, string message )
		{
			var repository = FileSystemRepository.Current.Get();
			var expected = Encoding.Default.GetBytes( message );
			repository.Set( path, new FileElement( expected ) );


			File.DefaultImplementation.Implementation.Assign( Sources.Factory.Cache(  () => new Mock<MockFile> { CallBase = true }.Object ) );
			var implementation = File.DefaultImplementation.Implementation.Get();
			Assert.Same( File.DefaultImplementation.Implementation.Get(), implementation );
			var mock = Mock.Get( (MockFile)implementation );
			var instance = File.Current.Get();
			Assert.Same( instance, File.Current.Get() );
			var pathToTest = $@".\{path}";
			mock.Verify( i => i.ReadAllText( pathToTest ), Times.Never );

			var item = instance.ReadAllText( pathToTest );
			Assert.Equal( message, item );
			mock.Verify( i => i.ReadAllText( pathToTest ), Times.Once );
		}
	}
}