using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities.Transactions;

public interface IContextAware : IResult<DbContext>;