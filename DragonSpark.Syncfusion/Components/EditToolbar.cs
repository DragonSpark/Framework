using DragonSpark.Compose;
using System.Collections.Generic;

namespace DragonSpark.SyncfusionRendering.Components;

public sealed class DefaultToolbar : List<string>
{
	public static DefaultToolbar Default { get; } = new();

	DefaultToolbar() : base("Filter".Yield()) {}
}

// TODO

public sealed class EditToolbar : List<string>
{
	public static EditToolbar Default { get; } = new();

	EditToolbar() : base(new[] { "Add", "Edit", "Delete", "Update", "Cancel" }) {}
}