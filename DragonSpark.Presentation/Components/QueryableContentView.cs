using AsyncUtilities;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class QueryableContentView<TItem> : ComponentBase
	{
		readonly Func<object, AsyncLock> _lock;

		public QueryableContentView() : this(Locks.Default.Get) {}

		public QueryableContentView(Func<object, AsyncLock> @lock) => _lock = @lock;

		protected override async Task OnParametersSetAsync()
		{
			Items = Empty.Array<TItem>();

			var queryable = Content ?? Items.AsQueryable();

			using (await _lock(queryable.Provider).LockAsync())
			{
				Items = await queryable.ToArrayAsync();
			}

			await base.OnParametersSetAsync().ConfigureAwait(false);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (!Items.Any())
			{
				builder.AddContent(2, NotFoundTemplate);
			}
			else if (ChildContent != null)
			{
				builder.AddContent(1, ChildContent(Items));
			}
		}

		TItem[] Items { get; set; } = default!;

		[Parameter, UsedImplicitly]
		public IQueryable<TItem>? Content { get; set; }

		[Parameter, UsedImplicitly]
		public RenderFragment<IEnumerable<TItem>>? ChildContent { get; set; }

		[Parameter]
		public RenderFragment NotFoundTemplate { get; set; } = x => x.AddContent(2, "No elements found.");
	}
}