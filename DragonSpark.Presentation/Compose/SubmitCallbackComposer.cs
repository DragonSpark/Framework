using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Compose;

public sealed class SubmitCallbackComposer : CallbackComposer<EditContext>
{
    public SubmitCallbackComposer(Func<EditContext, Task> valid) : this(valid, valid.Target) {}

    public SubmitCallbackComposer(Func<EditContext, Task> valid, object? target)
        : this(target, valid, _ => Task.CompletedTask) {}

    public SubmitCallbackComposer(Func<EditContext, Task> valid, IOperation invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), new Accepting<EditContext>(invalid)) {}

	public SubmitCallbackComposer(Func<EditContext, Task> valid, IOperation<EditContext> invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), invalid) {}

	public SubmitCallbackComposer(object? target, Func<EditContext, Task> valid, Func<EditContext, Task> invalid)
		: this(target, valid.Start().Then().Structure().Out(), invalid.Start().Then().Structure().Out()) {}

	public SubmitCallbackComposer(object? target, IOperation<EditContext> valid, IOperation<EditContext> invalid)
		: base(target, new SubmitOperation(valid, invalid).Allocate) {}
}