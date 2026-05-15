using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface IKnownKeys : ISelect<DbContext, ImmutableHashSet<object>>;