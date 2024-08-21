using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

public interface IProgram : IAllocated<IHostBuilder>;