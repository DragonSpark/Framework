using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public interface IStorageConfiguration : ISelect<IServiceCollection, Action<DbContextOptionsBuilder>>;