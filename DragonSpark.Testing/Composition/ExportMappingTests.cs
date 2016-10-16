using DragonSpark.Composition;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class ExportMappingTests
	{
		[Fact]
		public void Create()
		{
			var type = GetType();
			var sut = new ExportMapping( type );
			Assert.Equal( sut.ExportAs, sut.Subject );
			Assert.Equal( type, sut.Subject );
		}
	}
}
