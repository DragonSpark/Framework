using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Text;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Forms;

public class ModelPersistenceComponentBase<T> : ComponentBase where T : class
{
	[Parameter]
	public T Model
	{
		get => _model;
		set
		{
			if (_model != value)
			{
				_model = value;
				Save.Down();
			}
		}
	}   T _model = default!;

	[Parameter]
	public Switch Load { get; set; } = new();

	[Parameter]
	public Switch Save { get; set; } = new();

	[Parameter]
	public EventCallback<T> ModelChanging { get; set; }

	[Parameter]
	public EventCallback<T> ModelChanged { get; set; }

	[Parameter]
	public IClientVariable<string> Store { get; set; } = default!;

	[Parameter]
	public IFormatter<T> Formatter { get; set; } = default!;

	[Parameter]
	public ITarget<T> Target { get; set; } = default!;

	[Parameter]
	public EventCallback ErrorOccurred { get; set; }
}