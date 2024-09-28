using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Presentation.Components.Dialogs;

public sealed class AlwaysAccept<T> : Selecting<T, DialogResult>, IConfirming<T>
{
	public static AlwaysAccept<T> Default { get; } = new();

	AlwaysAccept() : base(_ => DialogResult.Ok.ToOperation()) {}
}