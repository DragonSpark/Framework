using Bogus;
using System;

namespace DragonSpark.Application.Hosting.xUnit.Objects
{
	public class SomeProfile<T> : ISome<T> where T : class
	{
		readonly Func<Faker<T>, Faker<T>> _alteration;

		public SomeProfile(Func<Faker<T>, Faker<T>> alteration) => _alteration = alteration;

		public Faker<T> Get(Seeding<T> parameter) => _alteration(parameter.Source);
	}
}