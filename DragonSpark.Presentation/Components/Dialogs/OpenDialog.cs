using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Dialogs;

sealed class OpenDialog : IOperation
{
	readonly DialogService                 _service;
	readonly string                        _title;
	readonly RenderFragment<DialogService> _fragment;

	public OpenDialog(DialogService service, string title, RenderFragment fragment)
		: this(service, title, fragment.Accept) {}

	public OpenDialog(DialogService service, string title, RenderFragment<DialogService> fragment)
	{
		_service  = service;
		_title    = title;
		_fragment = fragment;
	}

	public async ValueTask Get()
	{
		await _service.OpenAsync(_title, _fragment).Off();
	}
}