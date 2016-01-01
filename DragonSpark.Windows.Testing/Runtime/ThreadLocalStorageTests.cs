using DragonSpark.Testing.Objects;
using DragonSpark.Windows.Runtime;
using Xunit;

namespace DragonSpark.Windows.Testing.Runtime
{
	public class ThreadLocalStorageTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void Push( Class sut )
		{
			using ( ThreadLocalStorage.Push( sut ) )
			{
				Assert.Equal( sut, ThreadLocalStorage.Peek<Class>() );
			}

			Assert.Null( ThreadLocalStorage.Peek<Class>() );
		}
	}
}