using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
	{
		readonly IProperties _properties;

		public ComponentBase() : this(Properties.Default) {}

		public ComponentBase(IProperties properties) => _properties = properties;

		protected override async Task OnParametersSetAsync()
		{
			foreach (var operation in _properties.Get(GetType()).Open())
			{
				var task = operation(this).Get();
				if (!task.IsCompleted)
				{
					await task.ConfigureAwait(false);
				}
			}
		}

		[Inject]
		protected IExceptions Exceptions { get; set; } = default!;
	}
}