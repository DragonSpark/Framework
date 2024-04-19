using Microsoft.FluentUI.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Icons;

public sealed class WarningIcon : DragonSpark.Model.Results.Instance<Icon>
{
	public static WarningIcon Default { get; } = new();

	WarningIcon() : base(new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size28.Warning()) {}
}