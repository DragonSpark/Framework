using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Composition.Construction;

sealed class WithComposition : ReferenceValueStore<BuildHostContext, BuildHostContext>
{
    public static WithComposition Default { get; } = new();

    WithComposition() : base(x => x.Select(ComposeWithComposition.Default)) {}
}