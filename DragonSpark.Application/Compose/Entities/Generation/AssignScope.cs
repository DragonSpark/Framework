using DragonSpark.Application.Entities.Generation;

namespace DragonSpark.Application.Compose.Entities.Generation;

public delegate IRule<T, TOther> AssignScope<T, TOther>(Scope<T, TOther> scope) where TOther : class;