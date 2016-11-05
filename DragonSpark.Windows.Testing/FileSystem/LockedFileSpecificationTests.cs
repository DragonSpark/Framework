using DragonSpark.Testing.Framework.Application;
using DragonSpark.Windows.FileSystem;
using Moq;
using System.IO;
using Xunit;

namespace DragonSpark.Windows.Testing.FileSystem
{
	public class LockedFileSpecificationTests
	{
		[Theory, AutoData]
		public void Verify( Mock<IFileInfo> success, Mock<IFileInfo> fail )
		{
			success.Setup( info => info.Open( FileMode.Open, FileAccess.Read, FileShare.None ) ).Verifiable();
			fail.Setup( info => info.Open( FileMode.Open, FileAccess.Read, FileShare.None ) ).Throws<IOException>().Verifiable();

			Assert.False( LockedFileSpecification.Default.IsSatisfiedBy( success.Object ) );
			Assert.True( LockedFileSpecification.Default.IsSatisfiedBy( fail.Object ) );

			success.Verify();
			fail.Verify();
		}
	}
}