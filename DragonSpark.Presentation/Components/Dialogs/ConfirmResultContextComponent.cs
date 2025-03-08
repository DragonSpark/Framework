using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Dialogs;

public class ConfirmResultContextComponent<T> : ComponentBase
{
	protected DialogContext<T> _context = null!;

	protected Variable<ConfirmResult?> Store { get; } = new();

	[Parameter]
	public string Title { get; set; } = null!;

	[Parameter]
	public string Width { get; set; } = "unset";

	public async ValueTask<DialogResult> Confirm(T entity)
	{
		await _context.Open(entity);
		var result = Store.Get()?.Result ?? DialogResult.Other;
		return result;
	}
}