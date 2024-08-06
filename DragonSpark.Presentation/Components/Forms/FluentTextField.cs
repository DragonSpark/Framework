using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class FluentTextField : Microsoft.FluentUI.AspNetCore.Components.FluentTextField
{
	readonly Switch _invoke = new();

	protected override Task ChangeHandlerAsync(ChangeEventArgs e)
	{
		return _invoke.Down() ? InvokeAsync(() => base.ChangeHandlerAsync(e)) : Task.CompletedTask;
	}

	protected override Task InputHandlerAsync(ChangeEventArgs e)
	{
		_invoke.Up();
		return base.InputHandlerAsync(e);
	}
}
// TODO
public sealed class FluentTextArea : Microsoft.FluentUI.AspNetCore.Components.FluentTextArea
{
	protected override Task ChangeHandlerAsync(ChangeEventArgs e) => InvokeAsync(() => base.ChangeHandlerAsync(e));
}