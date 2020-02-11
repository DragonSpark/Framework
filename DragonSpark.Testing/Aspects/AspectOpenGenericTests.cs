using DragonSpark.Model.Aspects;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class AspectOpenGenericTests
	{
		sealed class Aspect<TIn, TOut> : IAspect<TIn, TOut>
		{
			[UsedImplicitly]
			public static Aspect<TIn, TOut> Default { get; } = new Aspect<TIn, TOut>();

			Aspect() {}

			public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter) => null;
		}

		[Fact]
		void Verify()
		{
			AspectOpenGeneric.Default.Get(typeof(Aspect<,>))(new Array<Type>(typeof(string), typeof(string)))()
			                 .Should()
			                 .BeSameAs(Aspect<string, string>.Default);
		}

		[Fact]
		void VerifyInvalid()
		{
			AspectOpenGeneric.Default.Invoking(x => x.Get(typeof(object)))
			                 .Should()
			                 .Throw<InvalidOperationException>();
		}
	}
}