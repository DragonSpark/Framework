using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IEntityMap<TFrom, TTo>
	: ISelect<DbContext, IStopAware<IReadOnlyCollection<TFrom>, IConditional<object, TTo>>>;