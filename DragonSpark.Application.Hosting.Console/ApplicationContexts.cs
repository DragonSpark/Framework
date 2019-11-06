using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.Console
{
	sealed class ApplicationContexts<T> : ApplicationContexts<Array<string>, ConsoleApplicationContext<T>>,
	                                      IApplicationContexts
		where T : class, ICommand<Array<string>>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Start.A.Selection.Of<string>()
		                                  .As.Sequence.Immutable.By.Default<IServices>()) {}
	}
}