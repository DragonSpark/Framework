using DragonSpark.Presentation.Components.Content;
using Radzen.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class DataList<T> : RadzenDataList<T>
	{
		bool Active { get; set; }

		public override async Task Reload()
		{
			Active = true;
			await base.Reload().ConfigureAwait(false);
			Active = false;
		}

		protected override Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender && Visible && LoadData.HasDelegate && Data is null or Reload<T> && !Active)
			{
				InvokeAsync(Reload);
			}
			return base.OnAfterRenderAsync(firstRender);
		}
	}
}