using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Entities.Migration;

public interface IMigration : IStopAware<ushort>, IStopAware;