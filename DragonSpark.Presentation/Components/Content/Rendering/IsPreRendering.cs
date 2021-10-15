using DragonSpark.Application;
using DragonSpark.Application.Components;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class IsPreRendering : AllCondition<None>, ICondition
{
	public IsPreRendering(IsTracking tracking, ConnectionStartTime start)
		: base(A.Result(tracking).Then().Accept(),
		       Time.Default.WithinLast(PreRenderingWindow.Default).Then().Bind(start.Get).Accept()) {}
}