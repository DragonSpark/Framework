using DragonSpark.Sources;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using System;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class SourceAccountedTypesTests
	{
		[Theory, AutoData, ContainingTypeAndNested]
		public void VerifyCaching()
		{
			var item = SourceAccountedAlteration.Defaults.Get( typeof(Guid) ).Invoke( Source.Default );
			Assert.IsType<Guid>( item );
			Assert.Equal( item, SourceAccountedAlteration.Defaults.Get( typeof(Guid) ).Invoke( Source.Default ) );
		}

		sealed class Source : SourceBase<Guid>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override Guid Get() => Guid.NewGuid();
		}
	}
}