using DragonSpark.Application;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework;
using Serilog;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class ExtensionsTests
	{
		[Fact]
		public void Coverage()
		{
			var parameter = new DelegatedSource<IParameterizedSource<object, ILogger>>( () => Logger.Default );
			Assert.NotNull( parameter.GetValueDelegate() );
			Assert.Same( Logger.Default.Get( this ), parameter.GetValue( this ) );

			var scope = new Scope<IClock>( () => Clock.Default );
			Assert.NotNull( scope.GetValueDelegate() );
			Assert.Equal( Time.Default.Get(), scope.GetValue() );
			Assert.Empty( Source.Default.GetImmutable() );
		}

		sealed class Source : SourceBase<IEnumerable<object>>
		{
			public static Source Default { get; } = new Source();
			Source() {}

			public override IEnumerable<object> Get()
			{
				yield break;
			}
		}
	}
}