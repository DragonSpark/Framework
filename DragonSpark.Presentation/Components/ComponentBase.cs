using Radzen.Blazor;
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
				await operation(this).Get().ConfigureAwait(false);
			}
		}
	}

	public sealed class Instance<T>
	{
		public T Value { get; set; }
	}

	/*public class InputForm : Form<string>
	{

	}*/

	public class Form<T> : RadzenTemplateForm<Instance<T>>
	{
		
	}
}