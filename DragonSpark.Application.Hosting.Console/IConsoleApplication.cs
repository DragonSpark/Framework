using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Hosting.Console
{
	public interface IConsoleApplication : ICommand<Array<string>> {}
}