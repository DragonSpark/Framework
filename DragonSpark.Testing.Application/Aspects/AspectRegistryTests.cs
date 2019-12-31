using DragonSpark.Aspects;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Types;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Aspects
{
	public class AspectRegistryTests
	{
		sealed class Aspect<TIn, TOut> : IAspect<TIn, TOut>
		{
			[UsedImplicitly]
			public static Aspect<TIn, TOut> Default { get; } = new Aspect<TIn, TOut>();

			Aspect() {}

			public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => null;
		}

		[Fact]
		void After()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		[Fact]
		void Before()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
		}

		[Fact]
		void Configure()
		{
			var subject = A.Self<object>();
			subject.Configured().Should().BeSameAs(subject);
		}

		[Fact]
		void Verify()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
			AspectRegistry.Default.Execute(new Registration(typeof(Aspect<,>)));
			AspectRegistry.Default.Get().Open().Should().HaveCount(1);
			AspectRegistrations<string, int>.Default.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			                                .Open()
			                                .Should()
			                                .HaveCount(1);
		}

		[Fact]
		void VerifyInvalid()
		{
			AspectRegistry.Default.Get().Open().Should().BeEmpty();
			AspectRegistry.Default.Execute(new Registration(Never<Array<Type>>.Default, typeof(Aspect<,>)));
			AspectRegistry.Default.Get().Open().Should().HaveCount(1);
			AspectRegistrations<string, int>.Default.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			                                .Open()
			                                .Should()
			                                .BeEmpty();
		}
	}
}