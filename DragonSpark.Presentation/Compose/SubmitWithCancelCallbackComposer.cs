using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;

namespace DragonSpark.Presentation.Compose;

public sealed class SubmitWithCancelCallbackComposer : CallbackComposer<SubmittingInput>
{
	public SubmitWithCancelCallbackComposer(Func<SubmittingInput, Task> valid) : this(valid, _ => Task.CompletedTask) {}

	public SubmitWithCancelCallbackComposer(Func<SubmittingInput, Task> valid, IOperation invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), new Accepting<SubmittingInput>(invalid)) {}

	public SubmitWithCancelCallbackComposer(Func<SubmittingInput, Task> valid, IOperation<SubmittingInput> invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), invalid) {}

	public SubmitWithCancelCallbackComposer(Func<SubmittingInput, Task> valid, Func<SubmittingInput, Task> invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), invalid.Start().Then().Structure().Out()) {}

	public SubmitWithCancelCallbackComposer(object? target, IOperation<SubmittingInput> valid,
	                                        IOperation<SubmittingInput> invalid)
		: base(target, new SubmitWithCancelOperation(valid, invalid).Allocate) {}
}