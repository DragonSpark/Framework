﻿using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace DragonSpark.Testing.Composition.Compose
{
	// ReSharper disable once TestFileNameWarning
	public sealed class DependencyRegistrationContextTests
	{
		sealed class Subject
		{
			public Subject(Other other) => Other = other;

			public Other Other { get; }
		}

		sealed class Other {}

		sealed class Multiple
		{
			public Multiple(Subject subject, Other other)
			{
				Subject = subject;
				Other   = other;
			}

			public Subject Subject { get; }

			public Other Other { get; }
		}

		sealed class Multiple<T>
		{
			public Multiple(Subject subject, Other other, List<T> list)
			{
				Subject = subject;
				Other   = other;
				List    = list;
			}

			public Subject Subject { get; }

			public Other Other { get; }
			public List<T> List { get; }
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Configure(x => x.For<Subject>()
			                                             .Register.WithDependencies.Singleton())
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<Subject>()
			    .Should()
			    .NotBeNull()
			    .And.Subject.To<Subject>()
			    .Other.Should()
			    .NotBeNull();
		}

		[Fact]
		public async Task VerifyGeneric()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Configure(x => x.For<Multiple<int>>().Register.WithDependencies.Singleton())
			                            .Operations()
			                            .Run();

			var multiple = host.Services.GetRequiredService<Multiple<int>>();
			multiple.Should().NotBeNull();
			multiple.Other.Should().NotBeNull();
			multiple.Subject.Should().NotBeNull();
			multiple.Subject.Other.Should().BeSameAs(multiple.Other);
			multiple.List.Should().NotBeNull();

			host.Services.Invoking(x => x.GetRequiredService<Multiple<object>>())
			    .Should()
			    .Throw<InvalidOperationException>();
		}

		[Fact]
		public async Task VerifyGenericDefinition()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Configure(x => x.ForDefinition<Multiple<object>>()
			                                             .Include(y => y.Dependencies)
			                                             .Singleton())
			                            .Operations()
			                            .Run();

			var multiple = host.Services.GetRequiredService<Multiple<int>>();
			multiple.Should().NotBeNull();
			multiple.Other.Should().NotBeNull();
			multiple.Subject.Should().NotBeNull();
			multiple.Subject.Other.Should().BeSameAs(multiple.Other);
			multiple.List.Should().NotBeNull();
		}

		[Fact]
		public async Task VerifyMultiple()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Configure(x => x.For<Multiple>()
			                                             .Register.WithDependencies.Singleton())
			                            .Operations()
			                            .Run();

			var multiple = host.Services.GetRequiredService<Multiple>();
			multiple.Should().NotBeNull();
			multiple.Other.Should().NotBeNull();
			multiple.Subject.Should().NotBeNull();
			multiple.Subject.Other.Should().BeSameAs(multiple.Other);
		}
	}
}