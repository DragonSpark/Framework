using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public class StateAwareComponent : ComponentBase
	{
		[Parameter]
		public EventCallback Initialized { get; set; }

		// ReSharper disable once FlagArgument
		protected override Task OnAfterRenderAsync(bool firstRender)
			=> firstRender ? Initialized.InvokeAsync() : base.OnAfterRenderAsync(firstRender);



	}
}