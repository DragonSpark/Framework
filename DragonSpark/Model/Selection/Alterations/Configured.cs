using System;

namespace DragonSpark.Model.Selection.Alterations
{
	public class Configured<T> : IAlteration<T>
	{
		readonly Action<T> _configure;

		public Configured(Action<T> configure) => _configure = configure;

		public T Get(T parameter)
		{
			_configure(parameter);
			return parameter;
		}
	}
}