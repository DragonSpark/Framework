using DragonSpark.Application.Components;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderKey : SelectedResult<Guid, string>
{
	public CurrentRenderKey(IClientIdentifier previous, ConnectionRenderKey select) : base(previous, select) {}
}