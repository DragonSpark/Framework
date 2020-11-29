using DragonSpark.Application.Runtime;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
	{
		/*readonly IProperties _properties;

		public ComponentBase() : this(Properties.Default) {}

		public ComponentBase(IProperties properties) => _properties = properties;*/

		/*protected override async Task OnParametersSetAsync()
		{
			var type = GetType();
			foreach (var operation in _properties.Get(type).Open())
			{
				var task = operation(this).Get();

				try
				{
					if (task.IsFaulted)
					{
						throw task.AsTask().Exception!;
					}

					if (!task.IsCompleted)
					{
						await task.ConfigureAwait(false);
					}
				}
				// ReSharper disable once CatchAllClause
				catch (Exception e)
				{
					await Exceptions.Get((type, e));
				}
			}
		}*/

		[Inject]
		protected IExceptions Exceptions { get; [UsedImplicitly]set; } = default!;
	}
}