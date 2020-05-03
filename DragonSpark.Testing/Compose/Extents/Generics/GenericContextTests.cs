using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents.Generics
{
	public sealed class GenericContextTests
	{
		sealed class Subject<T> : IResult<string>
		{
			[UsedImplicitly]
			public static Subject<T> Default { get; } = new Subject<T>();

			Subject() {}

			public string Get() => typeof(T).AssemblyQualifiedName.Verify();
		}

		sealed class SelectedSubject<T> : IAlteration<int>
		{
			readonly uint _seed;

			public SelectedSubject(uint seed) => _seed = seed;

			public int Get(int parameter) => (int)(parameter + _seed) + typeof(T).AssemblyQualifiedName.Verify().Length;
		}

		[Fact]
		public void Verify()
		{
			var parameters = typeof(int);
			Start.A.Generic(typeof(Subject<>))
			     .Of.Type<string>()
			     .As.Result()
			     .Get(parameters)()
			     .Get()
			     .Should()
			     .Be(parameters.AssemblyQualifiedName);
		}

		[Fact]
		public void VerifyParameter()
		{
			const uint start      = 6776u;
			const int  parameter  = 123;
			var        parameters = typeof(GenericContextTests);
			var        expected   = start + parameter + parameters.AssemblyQualifiedName.Verify().Length;
			Start
				.A.Generic(typeof(SelectedSubject<>))
				.Of.Type<IAlteration<int>>()
				.WithParameterOf<uint>()
				.Get(parameters)(start)
				.Get(parameter)
				.Should()
				.Be((int)expected);
		}
	}
}