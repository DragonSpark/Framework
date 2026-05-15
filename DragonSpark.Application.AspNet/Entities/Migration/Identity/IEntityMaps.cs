using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IEntityMaps<TFrom, TTo> : ISelect<DbContext, IEntityMap<TFrom, TTo>>;