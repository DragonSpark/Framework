using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation
{
	public readonly struct ValidationResultMessage
	{
		public ValidationResultMessage(string path, object instance, string message)
			: this(path, new FieldIdentifier(instance, string.Empty), message) {}

		public ValidationResultMessage(string path, FieldIdentifier field, string message)
		{
			Path    = path;
			Field   = field;
			Message = message;
		}

		public string Path { get; }
		public FieldIdentifier Field { get; }

		public string Message { get; }
	}
}