using System;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public interface IValidated
{
	event EventHandler<ValidationCallbackEventArgs> Validated;
}