using DragonSpark.Runtime.Specifications;
using DragonSpark.Runtime.Values;
using Xunit;

namespace DragonSpark.Testing.Runtime.Values
{
	public class AmbientContextTests
	{
		[Fact]
		public void Construct()
		{
			var key = new AmbientKey<AmbientContextTests>( AlwaysSpecification.Instance );
			Assert.Null( AmbientValues.Get<AmbientContextTests>() );
			using ( new AmbientContext( key, this ) )
			{
				Assert.Same( this, AmbientValues.Get<AmbientContextTests>() );
			}

			Assert.Null( AmbientValues.Get<AmbientContextTests>() );
		}
	}
}