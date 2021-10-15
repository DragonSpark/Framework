namespace DragonSpark.Application.Compose.Entities.Generation;

public delegate Include<T, TOther> Including<T, TOther>(Include<T, TOther> include) where TOther : class;