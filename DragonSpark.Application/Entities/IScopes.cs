using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public interface IScopes : IResult<DbContext>;