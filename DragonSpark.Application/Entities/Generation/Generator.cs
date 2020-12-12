using AutoBogus;
using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Entities.Generation
{
	public interface IGenerator<T> : ISelect<uint?, AutoFaker<T>> where T : class {}

	sealed class Generator<T> : IGenerator<T> where T : class
	{
		public static Generator<T> Default { get; } = new Generator<T>();

		Generator() : this(Configure<T>.Default.Execute) {}

		readonly Action<IAutoGenerateConfigBuilder> _configure;

		public Generator(Action<IAutoGenerateConfigBuilder> configure) => _configure = configure;

		public AutoFaker<T> Get(uint? parameter)
			=> new AutoFaker<T>().UseSeed(parameter.GetValueOrDefault(Randomizer.Seed.Next().Grade()).Degrade())
			                     .To<AutoFaker<T>>()
			                     .Configure(_configure);
	}
}