using System;

namespace DragonSpark.Application.Mobile.Maui.Presentation;

public abstract class ApplicationBase : Microsoft.Maui.Controls.Application, IApplication
{
    protected ApplicationBase(IServiceProvider services) => Services = services;

    public IServiceProvider Services { get; }
}