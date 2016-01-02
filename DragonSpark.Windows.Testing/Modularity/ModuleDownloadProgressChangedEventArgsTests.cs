using DragonSpark.Modularity;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ModuleDownloadProgressChangedEventArgsTests
	{
		[Theory, AutoData]
		public void Construct( [Frozen]long bytes, ModuleDownloadProgressChangedEventArgs sut )
		{
			Assert.NotNull( sut.ModuleInfo );
			Assert.Equal( bytes, sut.BytesReceived );
			Assert.Equal( bytes, sut.TotalBytesToReceive );
			Assert.Equal( 100, sut.ProgressPercentage );
		} 
	}
}