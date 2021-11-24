using DragonSpark.Model.Sequences;

namespace DragonSpark.Diagnostics.Logging;

public sealed class TemplateException : System.Exception
{
	public TemplateException(string message, params object[] parameters)
		: this(message, null, parameters) {}

	public TemplateException(string message, System.Exception? innerException, params object[] parameters)
		: base(message, innerException) => Parameters = parameters;

	public Array<object> Parameters { get; }
}