using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class3 {}

	public static class ActiveContentExtensions
	{
		public static ActiveContent<T> Content<T>(this DragonSpark.Compose.Model.ResultContext<ValueTask<T>> @this)
			=> new(@this);
	}

	public sealed class ActiveContent<T> : IResulting<T>
	{
		public static implicit operator ActiveContent<T>(Func<ValueTask<T>> instance) => new(instance);

		readonly IMutable<ValueTuple<T>?> _store;
		readonly Func<ValueTask<T>>       _content;

		public ActiveContent(Func<ValueTask<T>> content) : this(new Variable<ValueTuple<T>?>(), content) {}

		public ActiveContent(IMutable<ValueTuple<T>?> store, Func<ValueTask<T>> content)
		{
			_store   = store;
			_content = content;
		}

		public bool HasValue => _store.Get().HasValue;

		public async ValueTask<T> Get()
		{
			var store = _store.Get();
			if (store is null)
			{
				var result = await _content();
				_store.Execute(ValueTuple.Create(result));
				return result;
			}

			return store.Value.Item1;
		}

		public override string? ToString() => _store.Get()?.Item1?.ToString();
	}

	public abstract class ContentComponentBase<T> : ComponentBase
	{
		protected ActiveContent<T> Content { get; set; } = default!;

		protected abstract ValueTask<T> GetContent();

		protected override ValueTask Initialize() => InitializeContent();

		protected override async ValueTask RefreshState()
		{
			await InitializeContent();
			await Content.Get();
			await base.RefreshState().ConfigureAwait(false);
		}

		protected ValueTask InitializeContent()
		{
			Content = new ActiveContent<T>(GetContent);
			return ValueTask.CompletedTask;
		}
	}
}