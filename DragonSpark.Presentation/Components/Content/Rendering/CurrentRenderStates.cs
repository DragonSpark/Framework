using DragonSpark.Application.Components;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderStates : SelectedResult<Guid, RenderStates>
{
	public CurrentRenderStates(IClientIdentifier previous, InMemoryRenderStates select) : base(previous, select) {}
}