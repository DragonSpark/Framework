﻿using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Templates;

public sealed class DefaultExceptionTemplate : DragonSpark.Model.Results.Instance<RenderFragment>
{
	public static DefaultExceptionTemplate Default { get; } = new();

	DefaultExceptionTemplate() : base(x => x.AddContent(0, "There was a problem loading this view.")) {}
}