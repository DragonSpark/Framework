using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Testing.Objects.Setup
{
	public class CountingCommand : CommandBase<object>
	{
		public static CountingCommand Default { get; } = new CountingCommand();
		CountingCommand() {}

		public override void Execute( object parameter ) => Counting.Default.SetValue( parameter, Counting.Default.Get( parameter ) + 1 );
	}
}