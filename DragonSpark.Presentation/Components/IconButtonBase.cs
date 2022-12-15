using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DragonSpark.Presentation.Components;

public class IconButtonBase : BaseMatDomComponent
{
	[Inject]
	public NavigationManager UriHelper { get; set; } = null!;

	[Parameter]
	public RenderFragment ChildContent { get; set; } = null!;

	/// <summary>
	/// Default Button Icon
	/// </summary>
	[Parameter]
	public string Icon { get; set; } = null!;

	/// <summary>
	/// Target of Link when clicked.
	/// </summary>
	[Parameter]
	public string Target { get; set; } = null!;

	/// <summary>
	/// Icon to use when Button is clicked
	/// </summary>
	[Parameter]
	public string ToggleIcon { get; set; } = null!;

	[Parameter]
	public bool Toggled { get; set; }

	[Parameter]
	public EventCallback<bool> ToggledChanged { get; set; }

	/// <summary>
	/// Navigate to this url when clicked.
	/// </summary>
	[Parameter]
	public string? Link { get; set; }

	/// <summary>
	/// Force browser to redirect outside component router-space.
	/// </summary>
	///
	[Parameter]
	public bool ForceLoad { get; set; }

	/// <summary>
	/// Button is disabled.
	/// </summary>
	[Parameter]
	public bool Disabled { get; set; }

	public IconButtonBase()
	{
		ClassMapper.Add("mdc-icon-button");
	}

	/// <summary>
	///  Command executed when the user clicks on an element.
	/// </summary>
	[Parameter]
	public ICommand? Command { get; set; }

	/// <summary>
	///  Command parameter.
	/// </summary>
	[Parameter]
	public object? CommandParameter { get; set; }

	/// <summary>
	///  Event occurs when the user clicks on an element.
	/// </summary>
	[Parameter]
	public EventCallback<MouseEventArgs> OnClick { get; set; }

	/// <summary>
	/// Stop propagation of the OnClick event
	/// </summary>
	[Parameter]
	public bool OnClickStopPropagation { get; set; }

	[Parameter]
	public EventCallback<MouseEventArgs> OnMouseDown { get; set; }

	protected async override Task OnFirstAfterRenderAsync()
	{
		await base.OnFirstAfterRenderAsync();
		await JsInvokeAsync<object>("matBlazor.matIconButton.init", Ref);
	}

	protected async Task OnClickHandler(MouseEventArgs ev)
	{
		Toggled = !Toggled;
		await ToggledChanged.InvokeAsync(Toggled);

		if (Link != null)
		{
			if (!string.IsNullOrEmpty(Target))
			{
				await Js.InvokeVoidAsync("open", Link, Target);
			}
			else
			{
				UriHelper.NavigateTo(Link, ForceLoad);
			}
		}
		else
		{
			await OnClick.InvokeAsync(ev);
			if (Command?.CanExecute(CommandParameter) ?? false)
			{
				Command.Execute(CommandParameter);
			}
		}
	}
}