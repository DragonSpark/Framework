using DragonSpark.Compose;
using DragonSpark.Model.Aspects;
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
		void Configure()
		{
			var subject = A.Self<object>();
			new Aspects<object, object>().Get(subject).Get(subject).Should().BeSameAs(subject);
		}

		[Fact]
		void Verify()
		{
			var registry = new AspectRegistry();
			registry.Get().Open().Should().BeEmpty();
			registry.Execute(new Registration(typeof(Aspect<,>)));
			registry.Get().Open().Should().HaveCount(1);
			var registrations = new AspectRegistrations<string, int>(registry);
			registrations.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			             .Open()
			             .Should()
			             .HaveCount(1);
		}

		[Fact]
		void VerifyInvalid()
		{
			var registry = new AspectRegistry();
			registry.Get().Open().Should().BeEmpty();
			registry.Execute(new Registration(Never<Array<Type>>.Default, typeof(Aspect<,>)));
			registry.Get().Open().Should().HaveCount(1);
			var registrations = new AspectRegistrations<string, int>(registry);
			registrations.Get(GenericArguments.Default.Get(A.Type<ISelect<string, int>>()))
			             .Open()
			             .Should()
			             .BeEmpty();
		}
	}
}