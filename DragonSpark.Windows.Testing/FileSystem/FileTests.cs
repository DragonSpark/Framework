using DragonSpark.Sources.Scopes;
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
			var repository = FileSystemRepository.Default;
			var expected = Encoding.Default.GetBytes( message );
			repository.Set( path, new FileElement( expected ) );


			File.Default.Assign( Scopes.ToSingleton(  () => new Mock<MockFile> { CallBase = true }.Object ) );
			var implementation = File.Default.Get();
			Assert.Same( File.Default.Get(), implementation );
			var mock = Mock.Get( (MockFile)implementation );
			var instance = File.Default.Get();
			Assert.Same( instance, File.Default.Get() );
			var pathToTest = $@".\{path}";
			mock.Verify( i => i.ReadAllText( pathToTest ), Times.Never );

			var item = instance.ReadAllText( pathToTest );
			Assert.Equal( message, item );
			mock.Verify( i => i.ReadAllText( pathToTest ), Times.Once );
		}
	}
}