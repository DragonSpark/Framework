using DragonSpark.Model.Results;
using DragonSpark.Presentation.Connections;
using System;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class CurrentRenderKey : SelectedResult<Guid, string>
{
	public CurrentRenderKey(IConnectionIdentifier previous, ConnectionRenderKey select) : base(previous, select) {}
}