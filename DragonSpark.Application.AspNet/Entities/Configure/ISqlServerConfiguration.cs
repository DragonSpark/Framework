using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public interface ISqlServerConfiguration : ICommand<SqlServerDbContextOptionsBuilder>;