using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public sealed class MarkProcessCompleted : ICommand<Edit<ExternalProcess>>
{
	public static MarkProcessCompleted Default { get; } = new();

	MarkProcessCompleted() : this(Time.Default) {}

	readonly ITime _time;

	public MarkProcessCompleted(ITime time) => _time = time;

	public void Execute(Edit<ExternalProcess> parameter)
	{
		var (editor, subject) = parameter;
		foreach (var step in subject.CompletedSteps)
		{
			editor.Remove(step);
		}

		editor.Attach(subject);
		subject.CompletedSteps.Clear();
		subject.Completed = _time.Get();
	}
}