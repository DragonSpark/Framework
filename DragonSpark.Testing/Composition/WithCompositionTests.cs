using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	// ReSharper disable once TestFileNameWarning
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

		interface IWaves : ISelect<None, uint> {}

		sealed class Waves : IWaves
		{
			public static Waves Default { get; } = new Waves();

			Waves() : this(Time.Default, x => x) {}

			readonly ITime       _time;
			readonly Alter<uint> _total;

			public Waves(ITime time, Alter<uint> total)
			{
				_time  = time;
				_total = total;
			}

			public uint Get(None parameter) => _total((uint)_time.Get().Ticks);
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Configure(x => x.AddSingleton<Selection>())
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<Selection>().Should().BeSameAs(Selection.Default);
		}

		[Fact]
		public async Task VerifyWaves()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Configure(x => x.AddSingleton<IWaves, Waves>())
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<IWaves>().Should().BeSameAs(Waves.Default);
		}

		[Fact]
		public async Task VerifyDefault()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Operations()
			                            .Run();

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
			                            .Run();

			host.Services.GetRequiredService<Selection>().Should().NotBeSameAs(Selection.Default);
		}
	}
}