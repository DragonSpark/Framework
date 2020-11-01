using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using NetFabric.Hyperlinq;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components
{
	public sealed class CollectionContentView<TItem> : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var materialized = Content?.AsValueEnumerable().ToArray() ?? Empty.Array<TItem>();
			if (!materialized.Any())
			{
				builder.AddContent(2, NotFoundTemplate);
			}
			else if (ChildContent != null)
			{
				builder.AddContent(1, ChildContent(materialized));
			}
		}

		[Parameter, UsedImplicitly]
		public IEnumerable<TItem>? Content { get; set; }

		[Parameter, UsedImplicitly]
		public RenderFragment<IEnumerable<TItem>>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "No elements found.");
	}
}