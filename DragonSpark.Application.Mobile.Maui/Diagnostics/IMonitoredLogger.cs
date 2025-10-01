using DragonSpark.Model.Commands;
using Serilog;

namespace DragonSpark.Application.Mobile.Maui.Diagnostics;

public interface IMonitoredLogger : ILogger, ICommand<ILogger>;