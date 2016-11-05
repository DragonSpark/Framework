using DragonSpark.Tasks;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Objects.Setup
{
	[UsedImplicitly]
	public class CountingTaskSource : SuppliedTaskSource<object>
	{
		public CountingTaskSource() : base( CountingCommand.Default, CountingTarget.Default.Get ) {}
	}
}