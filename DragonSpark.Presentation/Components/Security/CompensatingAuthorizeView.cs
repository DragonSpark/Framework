using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Security
{
	public sealed class CompensatingAuthorizeView : AuthorizeView
	{
		bool Ready { get; set; }

		protected override async Task OnParametersSetAsync()
		{
			Ready = false;
			await base.OnParametersSetAsync();
			Ready = true;
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (Ready)
			{
				base.BuildRenderTree(builder);
			}
			else
			{
				builder.AddContent(0, Authorizing);
			}
		}
	}
}