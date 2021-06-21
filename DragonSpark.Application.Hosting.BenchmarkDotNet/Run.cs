using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Application.Hosting.BenchmarkDotNet
{
	public static class Run
	{
		public static void A<T>(IConfig parameter) => Run<T>.Default.Get(parameter);
	}

	public class Run<T> : Select<IConfig, Summary>
	{
		public static Run<T> Default { get; } = new Run<T>();

		Run() : this(x => BenchmarkRunner.Run<T>(x)) {}

		public Run(Func<IConfig, Summary> select) : base(select) {}
	}
}