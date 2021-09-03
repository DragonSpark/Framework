using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public abstract class OwningContentComponentBase<TContent, TService> : OwningComponentBase<TService>
		where TService : class
	{
		readonly Func<ValueTask<TContent>> _content;

		protected OwningContentComponentBase() => _content = GetContent;

		[Parameter]
		public IActiveContents<TContent> Contents { get; set; } = ActiveContents<TContent>.Default;

		protected IActiveContent<TContent> Content { get; set; } = default!;

		protected abstract ValueTask<TContent> GetContent();

		protected override ValueTask Initialize() => RefreshContent().Return(base.Initialize());

		IActiveContent<TContent> RefreshContent()
		{
			var result = Contents.Get(_content);
			Content = result;
			return result;
		}

		protected override async ValueTask RefreshState()
		{
			await RefreshContent().Get();
			await base.RefreshState().ConfigureAwait(false);
		}
	}

	public abstract class ContentComponentBase<T> : ComponentBase
	{
		readonly Func<ValueTask<T>> _content;

		protected ContentComponentBase() => _content = GetContent;

		[Parameter]
		public IActiveContents<T> Contents { get; set; } = ActiveContents<T>.Default;

		protected IActiveContent<T> Content { get; set; } = default!;

		protected abstract ValueTask<T> GetContent();

		protected override ValueTask Initialize() => RefreshContent().Return(base.Initialize());

		IActiveContent<T> RefreshContent()
		{
			var result = Contents.Get(_content);
			Content = result;
			return result;
		}

		protected override async ValueTask RefreshState()
		{
			await RefreshContent().Get();
			await base.RefreshState().ConfigureAwait(false);
		}
	}
}