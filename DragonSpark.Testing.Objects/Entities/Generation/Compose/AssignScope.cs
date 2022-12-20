namespace DragonSpark.Testing.Objects.Entities.Generation.Compose;

public delegate IRule<T, TOther> AssignScope<T, TOther>(Scope<T, TOther> scope) where TOther : class;