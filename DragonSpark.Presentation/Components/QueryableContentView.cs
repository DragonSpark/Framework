using DragonSpark.Model;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Presentation.Components
{
	public sealed class QueryableContentView<TItem> : ComponentBase
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var queryable = Content ?? Empty.Array<TItem>().AsQueryable();
			if (!queryable.Any())
			{
				builder.AddContent(2, NotFoundTemplate);
			}
			else if (ChildContent != null)
			{
				builder.AddContent(1, ChildContent(queryable));
			}
		}

		[Parameter, UsedImplicitly]
		public IQueryable<TItem>? Content { get; set; } = default!;

		[Parameter, UsedImplicitly]
		public RenderFragment<IEnumerable<TItem>>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "No elements found.");
	}
}