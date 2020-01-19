using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Application.Composition
{
	public sealed class WithCompositionTests
	{
		sealed class Instances : Select<int, object>
		{
			public static Instances Default { get; } = new Instances();

			Instances() : base(_ => new object()) {}
		}

		sealed class Selection : Select<int, object>
		{
			public static Selection Default { get; } = new Selection();

			Selection() : this(Instances.Default.Get) {}

			public Selection(Func<int, object> select) : base(select) {}
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Configure(x => x.AddSingleton<Selection>())
			                            .Operations()
			                            .Start();

			host.Services.GetRequiredService<Selection>().Should().BeSameAs(Selection.Default);
		}

		[Fact]
		public async Task VerifyDefault()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Operations()
			                            .Start();

			host.Services.GetRequiredService<Selection>().Should().BeSameAs(Selection.Default);
		}

		[Fact]
		public async Task VerifyRegistered()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Configure(x => x.AddSingleton<Selection>()
			                                             .AddSingleton(Instances.Default.ToDelegate()))
			                            .Operations()
			                            .Start();

			host.Services.GetRequiredService<Selection>().Should().NotBeSameAs(Selection.Default);
		}
	}
}