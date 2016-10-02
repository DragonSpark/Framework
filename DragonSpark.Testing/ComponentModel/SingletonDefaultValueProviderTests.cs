using DragonSpark.ComponentModel;
using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class SingletonDefaultValueProviderTests
	{
		[Fact]
		public void VerifySingleton()
		{
			var sut = new Component();
			var source = Assert.IsType<SingletonSource>( sut.Source );
			var expected = source.Get();
			Assert.NotNull( expected );
			Assert.Equal( expected, sut.PropertyName );
		}

		[Fact]
		public void VerifyConvention()
		{
			typeof(Convention).Yield().AsApplicationParts();

			var convention = ConventionTypeSelector.Default.Get( typeof(IConvention) );
			Assert.Equal( typeof(Convention), convention );

			var sut = new ConventionComponent();
			Assert.IsType<Convention>( sut.Subject );
		}

		class ConventionComponent
		{
			[Singleton, UsedImplicitly]
			public IConvention Subject { get; set; }
		}

		interface IConvention {}
		class Convention : IConvention
		{
			[UsedImplicitly]
			public static Convention Default { get; } = new Convention();
			Convention() {}
		}

		class Component
		{
			[Singleton( typeof(SingletonSource), nameof(SingletonSource.Value) ), UsedImplicitly]
			public string PropertyName { get; set; }

			[Singleton( typeof(SingletonSource) ), UsedImplicitly]
			public ISource Source { get; set; }
		}

		class SingletonSource : SourceBase<string>
		{
			[UsedImplicitly]
			public static SingletonSource Default { get; } = new SingletonSource();
			SingletonSource() {}

			public static string Value { get; } = Default.Get();

			public override string Get() => "Hello World!";
		}
	}
}
