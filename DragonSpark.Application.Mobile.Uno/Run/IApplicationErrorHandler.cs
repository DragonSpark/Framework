using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Uno.Run;

public interface IApplicationErrorHandler : ICommand<Exception>;