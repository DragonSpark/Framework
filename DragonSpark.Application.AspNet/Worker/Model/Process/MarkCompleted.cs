using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class MarkCompleted : IStopAware<ExternalProcess>
{
	readonly Editors                         _editors;
	readonly ICommand<Edit<ExternalProcess>> _mark;

	public MarkCompleted(Editors editors) : this(editors, MarkProcessCompleted.Default) {}

	public MarkCompleted(Editors editors, ICommand<Edit<ExternalProcess>> mark)
	{
		_editors = editors;
		_mark    = mark;
	}

	public async ValueTask Get(Stop<ExternalProcess> parameter)
	{
		using var editor = _editors.Get(parameter);
		// ReSharper disable once NotDisposedResource
		_mark.Execute(new(editor, parameter));
		await editor.Off();
	}
}