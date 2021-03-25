using DragonSpark.Application.Entities.Diagnostics;

namespace DragonSpark.Presentation.Components.Content
{
	public sealed class ApplicationContentView<TValue> : PolicyAwareDelegatedContentView<TValue>
	{
		public ApplicationContentView() : base(DurableApplicationContentPolicy.Default.Get()) {}
	}
}