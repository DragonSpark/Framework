using DragonSpark.Application.Diagnostics.Initialization;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Run;

sealed class LoggedProgram<T> : IProgram
{
	readonly ICommand _start;
	readonly IProgram _previous;

	public LoggedProgram(IProgram previous) : this(EmitRunningLog<T>.Default, previous) {}

	public LoggedProgram(ICommand start, IProgram previous)
	{
		_start    = start;
		_previous = previous;
	}

	public Task Get(Array<string> parameter)
	{
		_start.Execute();
		return _previous.Get(parameter);
	}
}