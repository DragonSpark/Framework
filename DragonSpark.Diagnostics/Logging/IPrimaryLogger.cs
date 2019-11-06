using System;
using Serilog;

namespace DragonSpark.Diagnostics.Logging
{
	public interface IPrimaryLogger : ILogger, IDisposable {}
}