namespace DragonSpark.Presentation.Components.Navigation
{
	public class ServerNavigateTo : NavigateTo
	{
		protected override void OnInitialized() {}

		protected override void OnAfterRender(bool firstRender)
		{
			Navigate();
		}

	}
}