using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class FluentTextArea : Microsoft.FluentUI.AspNetCore.Components.FluentTextArea
{
	protected override Task ChangeHandlerAsync(ChangeEventArgs e)
		=> InvokeAsync(() => base.ChangeHandlerAsync(e));
}