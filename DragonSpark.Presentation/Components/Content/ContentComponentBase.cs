using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public abstract class ContentComponentBase<T> : ComponentBase
	{
		protected ActiveContent<T> Content { get; set; } = default!;

		protected abstract ValueTask<T> GetContent();

		protected override ValueTask Initialize() => InitializeContent();

		protected ValueTask InitializeContent()
		{
			Content = new ActiveContent<T>(GetContent);
			return ValueTask.CompletedTask;
		}

		protected override async ValueTask RefreshState()
		{
			await InitializeContent();
			await Content.Get();
			await base.RefreshState().ConfigureAwait(false);
		}
	}
}