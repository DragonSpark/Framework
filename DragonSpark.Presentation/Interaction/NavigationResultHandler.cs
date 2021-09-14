using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Interaction
{
	public sealed class NavigationResultHandler : IOperation<NavigationResult>
	{
		readonly NavigationManager _manager;

		public NavigationResultHandler(NavigationManager manager) => _manager = manager;

		public ValueTask Get(NavigationResult parameter)
		{
			parameter.Execute(_manager);
			return ValueTask.CompletedTask;
		}
	}
}