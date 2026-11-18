using DragonSpark.Application.AspNet.Diagnostics;

namespace DragonSpark.Sentry;

sealed class DefaultExceptionFilter : ExceptionFilter
{
	public static DefaultExceptionFilter Default { get; } = new();

	DefaultExceptionFilter() : base(IgnoredApplicationException.Default) {}
	
}