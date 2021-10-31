using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation;

public readonly record struct ValidationResultMessage(string Path, FieldIdentifier Field, string Message)
{
	public ValidationResultMessage(string path, object instance, string message)
		: this(path, new FieldIdentifier(instance, string.Empty), message) {}
}