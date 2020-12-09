using Bogus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Hosting.xUnit.Objects
{
	public interface ISome<T> : ISelect<Seeding<T>, Faker<T>> where T : class {}
}