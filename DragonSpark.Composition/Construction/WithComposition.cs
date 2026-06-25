using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Composition.Construction;

sealed class WithComposition : Alteration<BuildHostContext>
{
    public static WithComposition Default { get; } = new();

    WithComposition() : base(x => x.Select(FactoryConfiguration.Default)) {}
}