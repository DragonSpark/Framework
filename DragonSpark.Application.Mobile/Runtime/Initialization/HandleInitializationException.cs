using System;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class HandleInitializationException : ICommand<Exception>
{
    public static HandleInitializationException Default { get; } = new();

    HandleInitializationException()
        : this(InitializationServices.Default, "A problem was encountered during initialization") {}

    readonly IServiceProvider _services;
    readonly string           _message;

    public HandleInitializationException(IServiceProvider services, string message)
    {
        _services = services;
        _message  = message;
    }

    public void Execute(Exception parameter)
    {
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _services.GetService<ILogger<HandleInitializationException>>()?.LogError(parameter, _message);
    }
}