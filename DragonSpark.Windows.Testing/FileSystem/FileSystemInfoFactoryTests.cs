using DragonSpark.Windows.FileSystem;
using System.IO.Abstractions;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class FileSystemInfoFactoryTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new FileSystemImplementationFactory<System.IO.DirectoryInfo, DirectoryInfoWrapper, DirectoryInfoBase>().Get( @"C:\some\path\" );
			Assert.IsType<DirectoryInfoWrapper>( sut );
		}
	}
}