using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Navigation;

public sealed class IsEnhancedValue : ContainsText
{
    public static IsEnhancedValue Default { get; } = new();

    IsEnhancedValue() : base("blazor-enhanced-nav=on") {}
}