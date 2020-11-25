﻿using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components
{
	public sealed class DefaultNotAssignedTemplate : Model.Results.Instance<RenderFragment>
	{
		public static DefaultNotAssignedTemplate Default { get; } = new DefaultNotAssignedTemplate();

		DefaultNotAssignedTemplate() : base(x => x.AddContent(0, "This view's required information does not exist.")) {}
	}
}