using Microsoft.AspNetCore.Components.Authorization;

namespace DragonSpark.Presentation.Components.Security;

public sealed class CompensatingAuthorizeView : AuthorizeView
{
	/*readonly Switch _ready = false;

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync().Off();
		_ready.Up();
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		if (_ready)
		{
			base.BuildRenderTree(builder);
		}
		else
		{
			builder.AddContent(0, Authorizing);
		}
	}*/
}