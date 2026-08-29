namespace DragonSpark.Application.AspNet.Entities.Migration.Steps;

public interface IExecution<T> : DragonSpark.Model.Operations.Stop.IStopAware<T>;