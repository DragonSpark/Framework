using Microsoft.FluentUI.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Icons;

public sealed class AddIcon : DragonSpark.Model.Results.Instance<Icon>
{
	public static AddIcon Default { get; } = new();

	AddIcon() : base(new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size24.AddCircle()) {}
}