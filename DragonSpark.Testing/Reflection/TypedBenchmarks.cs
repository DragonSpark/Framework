using BenchmarkDotNet.Attributes;
using System;

namespace DragonSpark.Testing.Reflection
{
	public class TypedBenchmarks
	{
		readonly Container _container;
		readonly Structure _structure;

		public TypedBenchmarks()
			: this(new Container(), new Structure(() => "Hello", () => "World", () => "Again!")) {}

		public TypedBenchmarks(Container container, Structure structure)
		{
			_container = container;
			_structure = structure;
		}

		[Benchmark]
		public Container Class()
			=> _container.WithFirst(() => "Hello").WithSecond(() => "World").WithThird(() => "Again!");

		[Benchmark]
		public Structure Structured()
			=> _structure.WithFirst(() => "Hello").WithSecond(() => "World").WithThird(() => "Again!");

		public sealed class Container
		{
			readonly Func<string> _first;
			readonly Func<string> _second;
			readonly Func<string> _third;

			public Container() : this(() => "Hello", () => "World", () => "Again!") {}

			public Container(Func<string> first, Func<string> second, Func<string> third)
			{
				_first  = first;
				_second = second;
				_third  = third;
			}

			public Container WithFirst(Func<string> parameter) => new Container(parameter, _second, _third);

			public Container WithSecond(Func<string> parameter) => new Container(_first, parameter, _third);

			public Container WithThird(Func<string> parameter) => new Container(_first, _second, parameter);
		}

		public readonly struct Structure
		{
			readonly Func<string> _first;
			readonly Func<string> _second;
			readonly Func<string> _third;

			public Structure(Func<string> first, Func<string> second, Func<string> third)
			{
				_first  = first;
				_second = second;
				_third  = third;
			}

			public Structure WithFirst(Func<string> parameter) => new Structure(parameter, _second, _third);

			public Structure WithSecond(Func<string> parameter) => new Structure(_first, parameter, _third);

			public Structure WithThird(Func<string> parameter) => new Structure(_first, _second, parameter);
		}
	}
}
