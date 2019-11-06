using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.Console
{
	public sealed class ConsoleApplication<T> : IConsoleApplication where T : class, IConsoleApplication
	{
		public static ConsoleApplication<T> Default { get; } = new ConsoleApplication<T>();

		ConsoleApplication() : this(ApplicationContexts<T>.Default) {}

		readonly IApplicationContexts _contexts;

		public ConsoleApplication(IApplicationContexts contexts) => _contexts = contexts;

		public void Execute(Array<string> parameter)
		{
			using (var context = _contexts.Get(parameter))
			{
				context.Execute(parameter);
			}
		}
	}
}