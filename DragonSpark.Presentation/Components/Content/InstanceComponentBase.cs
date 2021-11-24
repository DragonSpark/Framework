using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public abstract class InstanceComponentBase<T> : ContentComponentBase<T>
{
	protected abstract T? GetInstance();

	protected override ValueTask<T?> GetContent() => GetInstance().ToOperation();

	public T? Instance
	{
		get
		{
			var content = Content.Get();
			var result = content.IsCompletedSuccessfully
				             ? content.Result
				             : throw new
					               InvalidOperationException($"{GetType()} expected an in-memory instance to be available, but it was not.");
			return result;
		}
	}
		
}