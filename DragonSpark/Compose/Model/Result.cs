using DragonSpark.Model;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model
{
	public class Result<T> : DragonSpark.Model.Results.Result<T>, ISelect<T>
	{
		readonly Func<T> _source;

		public Result(Func<T> source) : base(source) => _source = source;

		public T Get(None _) => _source();

		public static implicit operator Result<T>(Func<T> value) => new Result<T>(value);

		public static implicit operator Func<T>(Result<T> value) => value.Get;
	}
}