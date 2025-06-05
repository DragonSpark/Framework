using System;

namespace DragonSpark.Application.Mobile.Maui.Presentation.Markup;

public sealed class UriMarkupExtension : MarkupExtension<Uri>
{
    public required string Address { get; set; }

    public override Uri ProvideValue(IServiceProvider serviceProvider) => new(Address);
}