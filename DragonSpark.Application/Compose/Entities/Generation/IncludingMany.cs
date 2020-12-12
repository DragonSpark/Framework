namespace DragonSpark.Application.Compose.Entities.Generation
{
	public delegate IncludeMany<T, TOther> IncludingMany<T, TOther>(IncludeMany<T, TOther> include) where TOther : class;
}