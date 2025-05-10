using DragonSpark.Application.Runtime.Operations;

namespace DragonSpark.Presentation.Components.State;

public sealed record CancelAwareActivityOptions(
	string Message,
	ITokenHandle Handle,
	bool RedrawOnStart = false,
	PostRenderAction PostRenderAction = PostRenderAction.None) : ActivityOptions(RedrawOnStart, PostRenderAction);