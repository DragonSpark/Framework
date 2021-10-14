using DragonSpark.Application.Entities.Queries.Compiled;
using DragonSpark.Compose;
using DragonSpark.Model;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
// ReSharper disable NotAccessedPositionalProperty.Local

namespace DragonSpark.Application.Testing.Entities.Queries.Compilation
{
	public sealed class ParameterUsageEditorTests
	{
		[Fact]
		public void Verify()
		{
			Expression<Func<Input, string>> expression = input => input.One.Two.Three.Name;

			var sut = new ParameterUsageEditor(expression);
			var (lambda, types, delegates) = sut.Rewrite();
			lambda.ToString().Should().Be("input_parameter_0 => input_parameter_0");

			const string? expected = "Third";
			var           instance = new Input("Root", 1, new("First", 2, new("Second", 3, new(expected, 123))));
			delegates.Open().Only().To<Func<Input, string>>()(instance).Should().Be(expected);
			types.Open().Should().Equal(A.Type<string>());
		}

		[Fact]
		public void VerifyComplex()
		{
			Expression<Func<DbContext, Input, string>> expression
				= (context, input) => context.Set<Subject>()
				                             .Where(x => x.Name == input.Name)
				                             .Select(x => x.Name + input.One.Two.Name)
				                             .Single();

			var sut = new ParameterUsageEditor(expression);
			var (lambda, types, delegates) = sut.Rewrite();
			lambda.ToString().Should().Be("(context, input_parameter_0, input_parameter_1) => context.Set().Where(x => (x.Name == input_parameter_0)).Select(x => (x.Name + input_parameter_1)).Single()");

			var expected = "Third";
			var instance = new Input("Root", 1, new("First", 2, new("Second", 3, new(expected, 123))));
			var open     = delegates.Open();
			open[0].To<Func<Input, string>>()(instance).Should().Be(instance.Name);
			open[1].To<Func<Input, string>>()(instance).Should().Be(instance.One.Two.Name);
			types.Open().Should().Equal(A.Type<string>(), A.Type<string>());
		}

		[Fact]
		public void VerifyNone()
		{
			Expression<Func<DbContext, None, string>> expression
				= (context, _) => context.Set<Subject>().Select(x => x.Name).Single();

			var sut = new ParameterUsageEditor(expression);
			var (lambda, types, delegates) = sut.Rewrite();
			lambda.ToString().Should().Be("context => context.Set().Select(x => x.Name).Single()");

			types.Open().Should().BeEmpty();
			delegates.Open().Should().BeEmpty();
		}

		[Fact]
		public void VerifyParameterReuse()
		{
			Expression<Func<DbContext, Input, string>> expression
				= (context, input) => context.Set<Subject>()
				                             .Where(x => x.Name == input.One.Name)
				                             .Select(x => x.Name + input.One.Name)
				                             .Single();

			var sut = new ParameterUsageEditor(expression);
			var (lambda, types, delegates) = sut.Rewrite();
			lambda.ToString().Should().Be("(context, input_parameter_0) => context.Set().Where(x => (x.Name == input_parameter_0)).Select(x => (x.Name + input_parameter_0)).Single()");

			var instance = new Input("Root", 1, new("First", 2, new("Second", 3, new("Third", 123))));
			var open     = delegates.Open();
			open.Only().To<Func<Input, string>>()(instance).Should().Be(instance.One.Name);
			types.Open().Should().Equal(A.Type<string>());
		}

		[Fact]
		public void VerifyParameterUsage()
		{
			Expression<Func<DbContext, Input, string>> expression
				= (context, input) => context.Set<SubjectWithInput>()
				                             .Where(x => x.Name == input.One.Name && x.Input == input)
				                             .Select(x => x.Name)
				                             .Single();

			var sut = new ParameterUsageEditor(expression);
			var (lambda, types, delegates) = sut.Rewrite();
			lambda.ToString().Should().Be("(context, input, input_parameter_0) => context.Set().Where(x => ((x.Name == input_parameter_0) AndAlso (x.Input == input))).Select(x => x.Name).Single()");

			var instance = new Input("Root", 1, new("First", 2, new("Second", 3, new("Third", 123))));
			var open     = delegates.Open();
			open[0].To<Func<Input, Input>>()(instance).Should().Be(instance);
			open[1].To<Func<Input, string>>()(instance).Should().Be(instance.One.Name);
			types.Open().Should().Equal(A.Type<Input>(), A.Type<string>());
		}

		readonly record struct Input(string Name, int Number, One One);

		readonly record struct One(string Name, int Number, Two Two);

		readonly record struct Two(string Name, int Number, Three Three);

		readonly record struct Three(string Name, int Number);

		sealed class Subject
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			public string Name { get; set; } = default!;

			[UsedImplicitly]
			public int Number { get; set; }
		}

		sealed class SubjectWithInput
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			public string Name { get; set; } = default!;

			[UsedImplicitly]
			public int Number { get; set; }

			[UsedImplicitly]
			public Input Input { get; set; }
		}
	}

}