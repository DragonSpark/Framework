using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Construction
{
	sealed class WithComposition : ReferenceValueStore<IHostBuilder, IHostBuilder>, IAlteration<IHostBuilder>
	{
		public static WithComposition Default { get; } = new WithComposition();

		WithComposition() : base(Construct.Default) {}
	}
}