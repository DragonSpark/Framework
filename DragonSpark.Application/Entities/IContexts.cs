using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public interface IContexts<out T> : IResult<T> where T : DbContext {}