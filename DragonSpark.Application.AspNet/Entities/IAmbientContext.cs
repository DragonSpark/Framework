using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities;

public interface IAmbientContext : IResult<DbContext?>;