using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
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
			var cached = one.ToSingleton();
			var item = cached.Invoke( Source.Default );
			Assert.IsType<Guid>( item );
			var two = SourceAccountedAlteration.Defaults.Get( typeof(Guid) );
			Assert.Same( one, two );
			Assert.Equal( item, cached.Invoke( Source.Default ) );
		}

		sealed class Source : SourceBase<Guid>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override Guid Get() => Guid.NewGuid();
		}
	}
}