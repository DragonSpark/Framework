using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public interface IInitializer : IStopAware<DbContext>;