using Radzen.Blazor;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class DataList<T> : RadzenDataList<T>
	{
		protected override Task OnParametersSetAsync()
			=> Data == null && Rendered ? Reload() : base.OnParametersSetAsync();

		bool Rendered { get; set; }

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);
			Rendered = true;
		}
	}
}