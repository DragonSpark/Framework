using DragonSpark.Application.Runtime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
	{
		[Inject]
		protected IExceptions Exceptions { get; [UsedImplicitly]set; } = default!;

		[Inject]
		protected IExecuteOperation Execute { get; [UsedImplicitly]set; } = default!;
	}
}