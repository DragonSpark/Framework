using DragonSpark.Model.Commands;
using System;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class Binding<T> : ICommand<T>
{
	readonly Action<T>            _notify;
	readonly IEqualityComparer<T> _comparer;

	public Binding(T value, Action<T> notify) : this(value, notify, EqualityComparer<T>.Default) {}

	public Binding(T value, Action<T> notify, IEqualityComparer<T> comparer)
	{
		_value    = value;
		_notify   = notify;
		_comparer = comparer;
	}

	public T Value
	{
		get => _value;
		set
		{
			if (!_comparer.Equals(value, _value))
			{
				_value = value;
				_notify(value);
			}
		}
	}	T _value;

	public void Execute(T parameter)
	{
		_value = parameter;
	}
}