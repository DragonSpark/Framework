using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Diagnostics;

public interface IApplicationErrorHandler : ICommand<Exception>;