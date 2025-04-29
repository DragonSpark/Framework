using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile;

public interface IApplicationErrorHandler : ICommand<Exception>;