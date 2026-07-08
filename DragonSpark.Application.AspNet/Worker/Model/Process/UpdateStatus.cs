using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class UpdateStatus : IStopAware<UpdateProcessStatusInput>
{
	readonly IEdit _edit;
	readonly ITime _time;

	protected UpdateStatus(IEdit edit) : this(edit, Time.Default) {}

	protected UpdateStatus(IEdit edit, ITime time)
	{
		_edit = edit;
		_time = time;
	}

	public async ValueTask Get(Stop<UpdateProcessStatusInput> parameter)
	{
		var ((process, status, message), stop) = parameter;
		using var edit = await _edit.Off(new(process, stop));
		var update = new ProcessUpdate
		{
			Created = _time.Get(), Status = status, Message = message
		};
		process.Update(update);
		await edit.Off();
	}
}