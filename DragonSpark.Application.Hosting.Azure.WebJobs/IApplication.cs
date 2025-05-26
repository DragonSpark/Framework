using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public interface IApplication : IStopAware<Guid>;