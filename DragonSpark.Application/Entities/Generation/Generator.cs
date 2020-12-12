using AutoBogus;
using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class Generator<T> : IResult<AutoFaker<T>> where T : class
	{
		public static Generator<T> Default { get; } = new Generator<T>();

		Generator() : this(Configure<T>.Default.Execute) {}

		readonly Action<IAutoGenerateConfigBuilder> _configure;

		public Generator(Action<IAutoGenerateConfigBuilder> configure) => _configure = configure;

		public AutoFaker<T> Get() => new AutoFaker<T>().Configure(_configure);
	}
}