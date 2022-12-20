namespace DragonSpark.Testing.Objects.Entities.Generation.Compose;

public delegate IncludeMany<T, TOther> IncludingMany<T, TOther>(IncludeMany<T, TOther> include) where TOther : class;