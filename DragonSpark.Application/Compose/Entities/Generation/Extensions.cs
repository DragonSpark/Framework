using DragonSpark.Compose;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public static class Extensions
	{
		public static GeneratorContext<T> Generator<T>(this ModelContext _)
			where T : class => new GeneratorContext<T>();
	}
}