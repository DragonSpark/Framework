using DragonSpark.Configuration;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Configuration
{
	public class RegistrationTests
	{
		[Theory, AutoData]
		public void CheckEquality( Registration sut, string key, string other )
		{
			sut.Key = key;

			Assert.True( sut.Equals( key ) );

			Assert.False( sut.Equals( other ) );
		} 
	}
}