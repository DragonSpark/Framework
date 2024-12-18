using System;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace DragonSpark.Compose;

// ReSharper disable TemplateIsNotCompileTimeConstantProblem
// ReSharper disable TooManyArguments
public static partial class ExtensionMethods
{
	public static void LogTrace<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogTrace(@this, template, parameter);
	}

	public static void LogTrace<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogTrace(@this, template, first, second);
	}

	public static void LogTrace<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogTrace(@this, template, first, second, third);
	}

	public static void LogTrace<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogTrace(@this, exception, template, parameter);
	}

	public static void LogTrace<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                    T2 second)
	{
		LoggerExtensions.LogTrace(@this, exception, template, first, second);
	}

	public static void LogTrace<T1, T2, T3>(this ILogger @this, Exception exception, string template, T1 first,
	                                        T2 second, T3 third)
	{
		LoggerExtensions.LogTrace(@this, exception, template, first, second, third);
	}

	public static void LogDebug<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogDebug(@this, template, parameter);
	}

	public static void LogDebug<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogDebug(@this, template, first, second);
	}

	public static void LogDebug<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogDebug(@this, template, first, second, third);
	}

	public static void LogDebug<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogDebug(@this, exception, template, parameter);
	}

	public static void LogDebug<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                    T2 second)
	{
		LoggerExtensions.LogDebug(@this, exception, template, first, second);
	}

	public static void LogDebug<T1, T2, T3>(this ILogger @this, Exception exception, string template, T1 first,
	                                        T2 second, T3 third)
	{
		LoggerExtensions.LogDebug(@this, exception, template, first, second, third);
	}

	public static void LogInformation<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogInformation(@this, template, parameter);
	}

	public static void LogInformation<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogInformation(@this, template, first, second);
	}

	public static void LogInformation<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second,
	                                              T3 third)
	{
		LoggerExtensions.LogInformation(@this, template, first, second, third);
	}

	public static void LogInformation<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogInformation(@this, exception, template, parameter);
	}

	public static void LogInformation<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                          T2 second)
	{
		LoggerExtensions.LogInformation(@this, exception, template, first, second);
	}

	public static void LogInformation<T1, T2, T3>(this ILogger @this, Exception exception, string template,
	                                              T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogInformation(@this, exception, template, first, second, third);
	}

	public static void LogWarning<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogWarning(@this, template, parameter);
	}

	public static void LogWarning<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogWarning(@this, template, first, second);
	}

	public static void LogWarning<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogWarning(@this, template, first, second, third);
	}

	public static void LogWarning<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogWarning(@this, exception, template, parameter);
	}

	public static void LogWarning<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                      T2 second)
	{
		LoggerExtensions.LogWarning(@this, exception, template, first, second);
	}

	public static void LogWarning<T1, T2, T3>(this ILogger @this, Exception exception, string template, T1 first,
	                                          T2 second, T3 third)
	{
		LoggerExtensions.LogWarning(@this, exception, template, first, second, third);
	}

	public static void LogError<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogError(@this, template, parameter);
	}

	public static void LogError<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogError(@this, template, first, second);
	}

	public static void LogError<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogError(@this, template, first, second, third);
	}

	public static void LogError<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogError(@this, exception, template, parameter);
	}

	public static void LogError<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                    T2 second)
	{
		LoggerExtensions.LogError(@this, exception, template, first, second);
	}

	public static void LogError<T1, T2, T3>(this ILogger @this, Exception exception, string template, T1 first,
	                                        T2 second, T3 third)
	{
		LoggerExtensions.LogError(@this, exception, template, first, second, third);
	}

	public static void LogCritical<T>(this ILogger @this, string template, T parameter)
	{
		LoggerExtensions.LogCritical(@this, template, parameter);
	}

	public static void LogCritical<T1, T2>(this ILogger @this, string template, T1 first, T2 second)
	{
		LoggerExtensions.LogCritical(@this, template, first, second);
	}

	public static void LogCritical<T1, T2, T3>(this ILogger @this, string template, T1 first, T2 second, T3 third)
	{
		LoggerExtensions.LogCritical(@this, template, first, second, third);
	}

	public static void LogCritical<T>(this ILogger @this, Exception exception, string template, T parameter)
	{
		LoggerExtensions.LogCritical(@this, exception, template, parameter);
	}

	public static void LogCritical<T1, T2>(this ILogger @this, Exception exception, string template, T1 first,
	                                       T2 second)
	{
		LoggerExtensions.LogCritical(@this, exception, template, first, second);
	}

	public static void LogCritical<T1, T2, T3>(this ILogger @this, Exception exception, string template, T1 first,
	                                           T2 second, T3 third)
	{
		LoggerExtensions.LogCritical(@this, exception, template, first, second, third);
	}

	public static void Execute(this ICommand<ExceptionParameter<Array<object>>> @this,
	                           Exception exception, params object[] arguments)
	{
		@this.Execute(new(exception, arguments));
	}

	public static void Execute<T>(this ICommand<ExceptionParameter<T>> @this, Exception exception, T argument)
	{
		@this.Execute(new(exception, argument));
	}

	public static void Execute<T1, T2>(this ICommand<ExceptionParameter<ValueTuple<T1, T2>>> @this,
	                                   Exception exception, T1 first, T2 second)
	{
		@this.Execute(new(exception, (first, second)));
	}

	public static void Execute<T1, T2, T3>(this ICommand<ExceptionParameter<ValueTuple<T1, T2, T3>>> @this,
	                                       Exception exception, T1 first, T2 second, T3 third)
	{
		@this.Execute(new(exception, (first, second, third)));
	}

	public static TemplateException Get<T>(this ITemplate<T> @this, T parameter)
		=> @this.Get(new InvalidOperationException(), parameter);

	public static TemplateException Get<T>(this ITemplate<T> @this, Exception exception, T parameter)
		=> @this.Get(new(exception, parameter));

	public static TemplateException Get<T1, T2>(this ITemplate<(T1, T2)> @this, T1 first, T2 second)
		=> @this.Get(new InvalidOperationException(), first, second);

	public static TemplateException Get<T1, T2>(this ITemplate<(T1, T2)> @this,
	                                            Exception exception, T1 first, T2 second)
		=> @this.Get(new(exception, (first, second)));

	public static TemplateException Get<T1, T2, T3>(this ITemplate<(T1, T2, T3)> @this,
	                                                T1 first, T2 second, T3 third)
		=> @this.Get(new InvalidOperationException(), first, second, third);

	public static TemplateException Get<T1, T2, T3>(this ITemplate<(T1, T2, T3)> @this,
	                                                Exception exception, T1 first, T2 second, T3 third)
		=> @this.Get(new(exception, (first, second, third)));

	public static bool IsNullOrEmpty(this string? @this) => string.IsNullOrEmpty(@this);
	public static bool IsNullOrWhiteSpace(this string? @this) => string.IsNullOrWhiteSpace(@this);
}
