using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class DefaultActiveContents<T> : Select<Func<ValueTask<T?>>, IActiveContent<T>>, IActiveContents<T>
{
	public static DefaultActiveContents<T> Default { get; } = new();

	DefaultActiveContents() : base(new SingletonAwareActiveContents<T>(ActiveContents<T>.Default)) {}
}