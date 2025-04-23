using Microsoft.Extensions.Localization;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Markup;

public sealed class Localized : MarkupExtension<IStringLocalizer>
{
    public string Name { get; set; } = string.Empty;

    public override object Get(MarkupExtensionContext<IStringLocalizer> parameter)
    {
        var (_, subject) = parameter;
        return subject[Name];
    }
}