using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Forms.Validation;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Compose;

public sealed class SubmitCallbackContext : CallbackContext<EditContext>
{
	public SubmitCallbackContext(Func<EditContext, Task> valid) : this(valid, _ => Task.CompletedTask) {}

	public SubmitCallbackContext(Func<EditContext, Task> valid, IOperation invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), new Accepting<EditContext>(invalid)) {}

	public SubmitCallbackContext(Func<EditContext, Task> valid, IOperation<EditContext> invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), invalid) {}

	public SubmitCallbackContext(Func<EditContext, Task> valid, Func<EditContext, Task> invalid)
		: this(valid.Target, valid.Start().Then().Structure().Out(), invalid.Start().Then().Structure().Out()) {}

	public SubmitCallbackContext(object? target, IOperation<EditContext> valid, IOperation<EditContext> invalid)
		: base(target, new Submit(valid, invalid).Then().Allocate()) {}
}