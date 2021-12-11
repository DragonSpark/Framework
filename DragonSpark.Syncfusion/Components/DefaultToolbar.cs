using DragonSpark.Compose;
using System.Collections.Generic;

namespace DragonSpark.Syncfusion.Components;

public sealed class DefaultToolbar : List<string>
{
	public static DefaultToolbar Default { get; } = new();

	DefaultToolbar() : base("Filter".Yield()) {}
}