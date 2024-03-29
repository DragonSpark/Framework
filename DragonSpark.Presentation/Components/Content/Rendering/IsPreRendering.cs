﻿using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class IsPreRendering : ICondition
{
	readonly RenderStateStore _state;

	public IsPreRendering(RenderStateStore state) => _state = state;

	public bool Get(None parameter) => _state.Get() is RenderState.Default or RenderState.Ready;
}