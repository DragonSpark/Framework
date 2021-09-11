using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public abstract class OwningContentComponentBase<TService, TContent> : Scoped.OwningComponentBase<TService>
		where TService : class
	{
		readonly Func<ValueTask<TContent>> _content;

		protected OwningContentComponentBase() => _content = GetContent;

		[Parameter, Inject]
		public IActiveContents<TContent> Contents { get; set; } = ActiveContents<TContent>.Default;

		protected IActiveContent<TContent> Content => _current.Verify();

		IActiveContent<TContent>? _current;

		protected abstract ValueTask<TContent> GetContent();

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			Apply();
		}

		void Apply()
		{
			_current ??= Contents.Get(_content);
		}

		protected void RequestNewContent()
		{
			_current = null;
		}

		protected override ValueTask RefreshState()
		{
			RequestNewContent();
			Apply();
			return base.RefreshState();
		}
	}
}