using DragonSpark.Application;
using DragonSpark.Extensions;
using JetBrains.Annotations;
using System.Composition;
using Xunit;

namespace DragonSpark.Testing.Application
{
	public class DefaultExportProviderTests
	{
		[Fact]
		public void ExportsAsExpected()
		{
			GetType().Adapt().WithNested().AsApplicationParts();
			var exports = DefaultExportProvider.Default.GetExports<ITarget>();
			var single = Assert.Single( exports.AsEnumerable() );
			Assert.IsType<TargetWithExportedSingleton>( single );
		}

		interface ITarget {}

		[UsedImplicitly]
		class Target : ITarget {}

		[UsedImplicitly]
		class TargetWithSingleton : ITarget
		{
			[UsedImplicitly]
			public static TargetWithSingleton Default { get; } = new TargetWithSingleton();
			TargetWithSingleton() {}
		}

		class TargetWithExportedSingleton : ITarget
		{
			[Export, UsedImplicitly]
			public static TargetWithExportedSingleton Default { get; } = new TargetWithExportedSingleton();
			TargetWithExportedSingleton() {}
		}
	}
}
