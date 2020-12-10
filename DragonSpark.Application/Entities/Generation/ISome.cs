using Bogus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Entities.Generation
{
	public interface ISome<T> : ISelect<Seeding<T>, Faker<T>> where T : class {}
}