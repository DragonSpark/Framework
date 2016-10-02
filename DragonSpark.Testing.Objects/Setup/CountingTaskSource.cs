using DragonSpark.Tasks;

namespace DragonSpark.Testing.Objects.Setup
{
	public class CountingTaskSource : SuppliedTaskSource<object>
	{
		public static CountingTaskSource Default { get; } = new CountingTaskSource();
		CountingTaskSource() : base( CountingCommand.Default, CountingTarget.Default.Get ) {}
	}
}