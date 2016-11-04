using DragonSpark.Application;
using DragonSpark.Extensions;
using Xunit;

namespace DragonSpark.Testing.Application
{
	public class SystemPartsTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.NotEmpty( new SystemParts( GetType().Assembly.Yield() ).Types );
		}
	}
}