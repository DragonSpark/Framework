namespace DragonSpark.Testing.Objects.Entities.Generation.Compose;

public delegate Include<T, TOther> Including<T, TOther>(Include<T, TOther> include) where TOther : class;