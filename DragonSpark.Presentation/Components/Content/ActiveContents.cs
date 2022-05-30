using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContents<T> : IActiveContents<T>
{
	public static ActiveContents<T> Default { get; } = new ActiveContents<T>();

	ActiveContents() {}

	public IActiveContent<T> Get(Func<ValueTask<T?>> parameter) => new ActiveContent<T>(parameter);
}