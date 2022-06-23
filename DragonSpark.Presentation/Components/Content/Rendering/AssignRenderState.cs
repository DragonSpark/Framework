using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Components.Eventing;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class AssignRenderState : ICommand<RenderState>
{
	readonly CurrentRenderState                    _current;
	readonly IPublisher<CurrentRenderStateMessage> _publisher;

	public AssignRenderState(CurrentRenderState current, IPublisher<CurrentRenderStateMessage> publisher)
	{
		_current   = current;
		_publisher = publisher;
	}

	public void Execute(RenderState parameter)
	{
		_current.Execute(parameter);
		Task.Run(_publisher.Then().Allocate().Bind(new CurrentRenderStateMessage(parameter)));
	}
}