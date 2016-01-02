using DragonSpark.Runtime;

namespace DragonSpark.Setup
{
	public abstract class SetupCommand : Command<ISetupParameter>
	{}

	public abstract class SetupCommand<TArgument> : Command<ISetupParameter<TArgument>> 
	{}
}