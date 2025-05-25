using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Initialization;

public interface IDataMigrationRegistry : ICommand<ISeed>, IStopAware<DbContext>;