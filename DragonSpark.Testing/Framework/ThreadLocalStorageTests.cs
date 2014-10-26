using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Framework.Testing
{
	public class ThreadLocalStorageTests
	{
		[Theory, AutoData]
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