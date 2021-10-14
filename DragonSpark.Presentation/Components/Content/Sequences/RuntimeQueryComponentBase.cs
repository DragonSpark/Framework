using DragonSpark.Application.Entities.Queries.Runtime;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences
{
	public abstract class RuntimeQueryComponentBase<T> : ComponentBase
	{
		protected override async ValueTask Initialize()
		{
			await base.Initialize();
			Content = GetContent();
		}

		protected abstract IQueries<T> GetContent();

		protected IQueries<T> Content { get; private set; } = default!;
	}
}