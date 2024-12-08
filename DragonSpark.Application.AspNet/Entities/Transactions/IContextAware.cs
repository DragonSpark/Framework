using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Transactions;

public interface IContextAware : IResult<DbContext>;