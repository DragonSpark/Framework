using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using Moq;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Xunit;
using DirectoryInfo = DragonSpark.Windows.FileSystem.DirectoryInfo;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class PurgeDirectoryCommandTests
	{
		[Theory, AutoData, RegisterFiles( @"FolderToDelete\File1.txt", @"FolderToDelete\Directory\SomeFile.txt", @"FolderToDelete\Directory2\AnotherFile.txt" )]
		public void Verify( [Service]IFileSystemRepository repository )
		{
			var directory = repository.FromDirectoryName( @".\FolderToDelete" );
			Assert.IsType<MockDirectoryInfo>( directory );
			Assert.True( directory.Exists );
			var directoryInfo = new DirectoryInfo( directory );
			Assert.Equal( 3, AllFilesSource.Default.Get( directoryInfo ).Length );
			PurgeDirectoryCommand.Default.Execute( directoryInfo );
			Assert.True( directory.Exists );
			Assert.Empty( AllFilesSource.Default.Get( directoryInfo ) );
		}

		[Theory, AutoData]
		public void Exceptions( Mock<IDirectoryInfo> directory, Mock<IDirectoryInfo> child, Mock<IFileInfo> success, Mock<IFileInfo> fail )
		{
			success.Setup( info => info.Delete() ).Verifiable();
			fail.Setup( info => info.Delete() ).Throws<InvalidOperationException>().Verifiable();
			child.Setup( info => info.Delete( It.IsAny<bool>() ) ).Throws<IOException>().Verifiable();
			directory.Setup( info => info.GetDirectories() ).Returns( () => child.Object.Yield().ToArray() ).Verifiable();
			directory.Setup( info => info.Exists ).Returns( () => true ).Verifiable();

			var log = LoggingHistory.Default.Get();
			Assert.Empty( log.Events );
			var sut = new PurgeDirectoryCommand( info => new[] { success.Object, fail.Object }.ToImmutableArray() );
			sut.Execute( directory.Object );

			success.Verify();
			fail.Verify();
			child.Verify();
			directory.Verify();

			Assert.Single( log.Events );
		}
	}
}