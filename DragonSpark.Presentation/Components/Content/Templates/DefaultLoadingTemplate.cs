﻿using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class DefaultLoadingTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
{
	public static DefaultLoadingTemplate Default { get; } = new();

	DefaultLoadingTemplate() : base(x => x.AddContent(0, "Loading, please wait.")) {}
}