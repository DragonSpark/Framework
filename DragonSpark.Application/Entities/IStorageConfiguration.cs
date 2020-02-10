using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities
{
	public interface IStorageConfiguration : ISelect<IServiceCollection, Action<DbContextOptionsBuilder>> {}
}