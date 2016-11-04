using DragonSpark.Application.Setup;
using DragonSpark.Testing.Framework.Application;
using Xunit;

namespace DragonSpark.Testing.Application.Setup
{
	public class InputOutputBaseTests
	{
		[Theory, AutoDataBasic]
		public void Cover( InputOutputBase sut )
		{
			Assert.NotNull( sut.Reader );
			Assert.NotNull( sut.Writer );
		}
	}
}