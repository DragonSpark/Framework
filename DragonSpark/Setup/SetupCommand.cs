using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Runtime;

namespace DragonSpark.Setup
{
	public abstract class SetupCommandBase<T> : Command<T> where T : ISetupParameter
	{
		[Default( true )]
		public bool Enabled { get; set; }

		public override bool CanExecute( T parameter ) => base.CanExecute( parameter ) && Enabled;
	}

	public abstract class SetupCommand : SetupCommandBase<ISetupParameter> {}

	public abstract class SetupCommand<TArgument> : SetupCommandBase<ISetupParameter<TArgument>> {}
}