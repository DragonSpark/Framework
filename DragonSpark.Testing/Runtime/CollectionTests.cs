using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using System.Collections;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class CollectionTests
	{
		[Theory, MoqAutoData]
		void Add( Collection<Class> sut )
		{
			Assert.Equal( -1, sut.To<IList>().Add( new object() ) );
		} 
	}
}