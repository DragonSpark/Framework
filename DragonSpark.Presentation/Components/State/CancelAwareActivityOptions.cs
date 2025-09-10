using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State;

public sealed record CancelAwareActivityOptions(
	string Message,
	IStopHandle Handle,
	bool RedrawOnStart = false,
	IOperation? Canceled = null,
	PostRenderAction PostRenderAction = PostRenderAction.None) : ActivityOptions(RedrawOnStart, PostRenderAction);