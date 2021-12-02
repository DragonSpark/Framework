using Radzen.Blazor;

namespace DragonSpark.Presentation.Components;

public class DataList<T> : RadzenDataList<T>
{
	protected override void OnParametersSet()
	{
		if (Visible && LoadData.HasDelegate && Data == null)
		{
			Visible = false;
			InvokeAsync(Reload);
		}
		else
		{
			Visible |= Data != null;
		}
		base.OnParametersSet();
	}
}