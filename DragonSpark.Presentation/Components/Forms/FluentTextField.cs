namespace DragonSpark.Presentation.Components.Forms;

public sealed class FluentTextField : Microsoft.FluentUI.AspNetCore.Components.FluentTextField
{
	public FluentTextField() : this(DefaultSize.Default) {}

	public FluentTextField(byte size) => Size = size;
}