using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized.Caching;
using System.Composition;

namespace DragonSpark.Testing.Objects.Composition
{
	[Export]
	public class ExportedItemFactory : SourceBase<ExportedItem>
	{
		public override ExportedItem Get() => new ExportedItem().WithSelf( item => Condition.Default.Get( item ).Apply() );
	}
}