using Bogus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Generation
{
	// TODO: remove
	public interface ISome<T> : ISelect<Seeding<T>, Faker<T>> where T : class {}
}