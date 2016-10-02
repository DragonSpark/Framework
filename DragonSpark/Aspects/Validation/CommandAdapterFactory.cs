using System.Windows.Input;

namespace DragonSpark.Aspects.Validation
{
	class CommandAdapterFactory : AdapterSourceBase<ICommand>
	{
		public static CommandAdapterFactory Default { get; } = new CommandAdapterFactory();
		CommandAdapterFactory() : base( instance => new CommandAdapter( instance ) ) {}
	}
}