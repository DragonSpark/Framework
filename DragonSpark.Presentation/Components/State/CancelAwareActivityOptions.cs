using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State;

public sealed record CancelAwareActivityOptions(
	string Message,
	IStopHandle Handle,
	bool RedrawOnStart = true,
	IOperation? Canceled = null,
	PostRenderAction PostRenderAction = PostRenderAction.ForceRedraw)
	: ActivityOptions(RedrawOnStart, PostRenderAction);