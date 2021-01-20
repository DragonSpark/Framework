using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Dialogs
{
	sealed class OpenDialog : IOperation
	{
		readonly DialogService  _service;
		readonly string         _title;
		readonly RenderFragment _fragment;

		public OpenDialog(DialogService service, string title, RenderFragment fragment)
		{
			_service  = service;
			_title    = title;
			_fragment = fragment;
		}

		public async ValueTask Get()
		{
			await _service.OpenAsync(_title, _ => _fragment).ConfigureAwait(false);
		}
	}
}