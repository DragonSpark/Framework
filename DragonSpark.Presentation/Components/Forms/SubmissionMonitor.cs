using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class SubmissionMonitor : Membership<EventCallback<SubmittingInput>>, IAllocated<EditContext>
{
	readonly EventCallback<EditContext> _submit;

	public SubmissionMonitor(EventCallback<EditContext> submit) : this(submit, []) {}

	public SubmissionMonitor(EventCallback<EditContext> submit, HashSet<EventCallback<SubmittingInput>> collection)
		: base(collection)
		=> _submit = submit;

	public Task Get(EditContext parameter) => _submit.Invoke(parameter);
}