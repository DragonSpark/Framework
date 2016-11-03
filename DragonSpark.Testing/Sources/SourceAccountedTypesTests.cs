using DragonSpark.Sources;
using System;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class SourceAccountedTypesTests
	{
		[Fact]
		public void VerifyCaching()
		{
			var one = SourceAccountedAlteration.Defaults.Get( typeof(Guid) );
			var item = one.Invoke( Source.Default );
			Assert.IsType<Guid>( item );
			var two = SourceAccountedAlteration.Defaults.Get( typeof(Guid) );
			Assert.Same( one, two );
			Assert.Equal( item, two.Invoke( Source.Default ) );
		}

		sealed class Source : SourceBase<Guid>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override Guid Get() => Guid.NewGuid();
		}
	}
}