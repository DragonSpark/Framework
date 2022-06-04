using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Interaction;

public sealed class NavigationResultHandler : IOperation<NavigationResult>
{
	readonly IAdjustRenderState _adjust;
	readonly NavigationManager  _manager;

	public NavigationResultHandler(IAdjustRenderState adjust, NavigationManager manager)
	{
		_adjust  = adjust;
		_manager = manager;
	}

	public ValueTask Get(NavigationResult parameter)
	{
		try
		{
			parameter.Execute(_manager);
		}
		catch (NavigationException)
		{
			_adjust.Execute();
			throw;
		}

		return ValueTask.CompletedTask;
	}
}