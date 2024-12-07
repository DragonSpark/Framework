using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public interface INewContext<out T> : IResult<T> where T : DbContext;