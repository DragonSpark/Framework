using DragonSpark.Model.Selection;
using System.Collections.Immutable;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

public interface ITypeKeys : ISelect<Type, ImmutableHashSet<object>>;