using Radzen.Blazor;

namespace DragonSpark.Presentation.Components
{
	public class DataList<T> : RadzenDataList<T>
	{
		/*protected override Task OnParametersSetAsync()
			=> /*Data == null && Rendered ? Reload() :#1# base.OnParametersSetAsync();

		bool Rendered { get; set; }

		protected override void OnAfterRender(bool firstRender)
		{
			base.OnAfterRender(firstRender);
			Rendered = true;
		}*/
	}
}