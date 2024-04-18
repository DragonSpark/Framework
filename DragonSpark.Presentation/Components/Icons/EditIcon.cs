using Microsoft.FluentUI.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Icons;

public sealed class EditIcon : DragonSpark.Model.Results.Instance<Icon>
{
	public static EditIcon Default { get; } = new();

	EditIcon() : base(new Microsoft.FluentUI.AspNetCore.Components.Icons.Regular.Size16.Edit()) {}
}