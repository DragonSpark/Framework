using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Environment.Browser;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms;

public abstract class ModelPersistenceComponentBase<T> : ComponentBase where T : class
{
	[Parameter]
	public EventCallback<T> ModelChanged { get; set; }

	[Parameter]
	public IClientVariable<string> Store { get; set; } = null!;

	[Parameter]
	public ISerializer<T> Serializer { get; set; } = null!;

	[Parameter]
	public EventCallback ErrorOccurred { get; set; }

	[CascadingParameter] protected IActivityReceiver Receiver { get; set; } = null!;
}