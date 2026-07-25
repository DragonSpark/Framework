using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public interface IApplication : IStopAware<Guid>;