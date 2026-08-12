namespace DragonSpark.Application.Components.Validation.Objects;

public readonly record struct ValidationResultMessage(string Path, FieldDescriptor Field, string Message)
{
	public ValidationResultMessage(string path, object instance, string message)
		: this(path, new(instance, string.Empty), message) {}
}