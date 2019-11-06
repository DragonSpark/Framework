using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.Console
{
	sealed class ConsoleApplicationContext<T> : ApplicationContext<Array<string>>
		where T : class, ICommand<Array<string>>
	{
		public ConsoleApplicationContext(T application, IServices services) :
			base(application, services.ToDisposable()) {}
	}
}