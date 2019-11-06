using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Adapters;

namespace DragonSpark.Compose.Commands
{
	public sealed class Context
	{
		public static Context Default { get; } = new Context();

		Context() {}

		public Extent Of => Extent.Default;
	}

	public sealed class Context<T>
	{
		public static Context<T> Instance { get; } = new Context<T>();

		Context() {}

		public ICommand<T> Empty => EmptyCommand<T>.Default;

		public Action<T> Calling(System.Action<T> body)
			=> new Action<T>(body);
	}
}