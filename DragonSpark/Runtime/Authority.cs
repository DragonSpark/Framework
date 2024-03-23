using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Runtime;

public sealed class Authority : Select<Uri, string>
{
	[UsedImplicitly]
	public static Authority Default { get; } = new();

	Authority() : base(x => x.GetLeftPart(UriPartial.Authority)) {}
}