using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public interface IConfigureExceptions : ICommand<UnhandledExceptionEventHandler>;