using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Contracts.Worker;
using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Workers.Model.Process.States;

sealed class Update : IUpdate
{
	readonly string?       _message;
	readonly ProcessStatus _status;
	readonly ITime         _time;

	public Update(ProcessStatus status, string? message = null) : this(status, message, Time.Default) {}

	public Update(ProcessStatus status, string? message, ITime time)
	{
		_status  = status;
		_message = message;
		_time    = time;
	}

	public ProcessUpdate Get() => new() { Created = _time.Get(), Status = _status, Message = _message };
}