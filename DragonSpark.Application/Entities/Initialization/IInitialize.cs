using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Initialization;

public interface IInitialize : IOperation<DbContext> {}