using DragonSpark.Compose;
using DragonSpark.Presentation.Components.State;
using DragonSpark.Presentation.Environment.Browser;
using DragonSpark.Text;
using Humanizer;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public class ModelPersistenceComponentBase<T> : ComponentBase where T : class
{
	[Inject] ProblemLoadingState ProblemLoading { get; set; } = default!;

	[Inject] ProblemSavingState ProblemSaving { get; set; } = default!;

	[Inject] SavedContentMessage Saved { get; set; } = default!;
	
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

	[Parameter]
	public T? Model { get; set; }

	[CascadingParameter] protected IActivityReceiver Receiver { get; set; } = default!;

	protected virtual T DetermineModel() => Model.Verify();

	protected async Task LoadContent()
	{
		var current = await Store.Get();
		if (current.Success)
		{
			var content = current.Value.Verify();
			try
			{
				var model = DetermineModel();
				await ModelChanging.Invoke(model);
				Target.Execute(new(model, content));
				await ModelChanged.Invoke(model).Off();
			}
			catch (Exception e)
			{
				ProblemLoading.Execute(new(e, content));
				await ErrorOccurred.Invoke().Off();
			}
		}
	}

	protected async Task SaveContent()
	{
		try
		{
			var model   = DetermineModel();
			var content = Formatter.Get(model);
			await Store.Off(content);
			Saved.Execute(content.Length.Bytes().Humanize());
		}
		catch (Exception e)
		{
			ProblemSaving.Execute(e);
			await ErrorOccurred.Invoke().Off();
		}
	}
}