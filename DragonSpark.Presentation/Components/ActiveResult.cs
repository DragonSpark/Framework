﻿using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components
{
	public sealed class ActiveResult<T> : IViewProperty, ISource<T>
	{
		public static implicit operator ActiveResult<T>(ValueTask<T> instance) => new ActiveResult<T>(instance);

		readonly ValueTask<T> _source;

		public ActiveResult(ValueTask<T> source) => _source = source;

		public T Value { get; private set; }

		public bool HasValue { get; private set; }

		public async ValueTask Get()
		{
			Value    = await _source;
			HasValue = true;
		}

		public override string ToString() => Value.ToString();
	}
}