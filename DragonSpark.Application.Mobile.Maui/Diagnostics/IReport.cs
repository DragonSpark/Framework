using System;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public interface IReport : ICommand<Exception>;

public interface IReport<T> : ICommand<SendExceptionInput<T>>;