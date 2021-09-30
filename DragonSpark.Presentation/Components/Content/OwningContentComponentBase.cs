using DragonSpark.Compose;
using DragonSpark.Presentation.Components.Content.Rendering;
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

		[Inject]
		IContentInteraction Interaction { get; set; } = default!;

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
			_current ??= Create(Contents.Get(_content));
		}

		protected virtual IActiveContent<TContent> Create(IActiveContent<TContent> parameter)
			=> parameter.Then().Handle(Exceptions, GetType()).Get();

		protected void RequestNewContent()
		{
			_current = null;
		}

		protected override ValueTask RefreshState()
		{
			Interaction.Execute();
			RequestNewContent();
			Apply();
			return base.RefreshState();
		}
	}
}