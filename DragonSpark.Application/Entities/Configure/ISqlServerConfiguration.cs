using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.Entities.Configure;

public interface ISqlServerConfiguration : ICommand<SqlServerDbContextOptionsBuilder> {}