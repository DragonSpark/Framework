using DragonSpark.Diagnostics;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Diagnostics
{
	public class MessageLoggerTests
	{
		[Theory, Framework.Setup.ConfiguredMoqAutoData]
		public void Log( MessageLogger sut, string message, Priority priority )
		{
			sut.Information( message, priority );
		}

		[Theory, Framework.Setup.ConfiguredMoqAutoData]
		public void Greedy( [Greedy]MessageLogger sut, string message, Priority priority )
		{
			sut.Information( message, priority );
		}
	}
}