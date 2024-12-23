using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Application.Mobile.Run;

public interface IApplicationErrorHandler : ICommand<Exception>;