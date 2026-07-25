using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class SubmissionMonitor : Membership<EventCallback<SubmittingInput>>, IAllocated<EditContext>, ISwitch
{
	readonly EventCallback<EditContext> _submit;
	readonly ISwitch?                   _allowed;

	public SubmissionMonitor(EventCallback<EditContext> submit, ISwitch? allowed) : this(submit, allowed, []) {}

	public SubmissionMonitor(EventCallback<EditContext> submit, ISwitch? allowed,
	                         HashSet<EventCallback<SubmittingInput>> collection)
		: base(collection)
	{
		_submit  = submit;
		_allowed = allowed;
	}

	public Task Get(EditContext parameter) => _submit.Invoke(parameter);

	public bool Get() => _allowed?.Get() ?? true;

	public void Execute(bool parameter)
	{
		_allowed?.Execute(parameter);
	}
}