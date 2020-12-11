namespace DragonSpark.Application.Compose.Entities.Generation
{
	public static class Include
	{
		public static Include<T, TOther> New<T, TOther>() where TOther : class
			=> new Include<T, TOther>((generator, _) => generator.Generate(), (_, __, ___) => {},
			                          scope => scope.Once());
	}
}