using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public sealed class ValidationCallbackEventArgs : EventArgs
{
	public ValidationCallbackEventArgs() : this(new List<EventCallback>()) {}

	public ValidationCallbackEventArgs(IList<EventCallback> callbacks) => Callbacks = callbacks;

	public IList<EventCallback> Callbacks { get; }
}