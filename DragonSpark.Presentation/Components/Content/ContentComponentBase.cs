using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	public abstract class ContentComponentBase<T> : ComponentBase
	{
		readonly Func<ValueTask<T>> _content;

		protected ContentComponentBase() => _content = GetContent;

		[Parameter, Inject]
		public IActiveContents<T> Contents { get; set; } = ActiveContents<T>.Default;

		protected IActiveContent<T> Content => _current.Verify();

		IActiveContent<T>? _current;

		protected abstract ValueTask<T> GetContent();

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