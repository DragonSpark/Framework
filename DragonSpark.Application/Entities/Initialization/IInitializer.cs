using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Initialization;

public interface IInitializer : IOperation<DbContext>;