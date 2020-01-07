namespace DragonSpark.Model.Commands
{
	public sealed class EmptyCommand<T> : ICommand<T>
	{
		public static ICommand<T> Default { get; } = new EmptyCommand<T>();

		EmptyCommand() {}

		public void Execute(T parameter) {}
	}

	public sealed class EmptyCommand : ICommand
	{
		public static EmptyCommand Default { get; } = new EmptyCommand();

		EmptyCommand() {}

		public void Execute(None parameter) {}
	}
}