﻿using DragonSpark.Application.Hosting.BenchmarkDotNet;
using DragonSpark.Testing.Application.Model.Sequences.Query.Construction;

namespace DragonSpark.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments).To(Run.A<ExitTests.Benchmarks>);
		}
	}
}