using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace DragonSpark.Application.Components.Validation;

sealed class Delegates : IDelegates
{
	readonly ISelect<(Type Owner, string Name), Func<object, object?>?> _delegates;
	readonly IDelegates                                                 _pathed;

	public Delegates() : this(PropertyDelegates.Default.ToStandardTable()) {}

	public Delegates(ISelect<(Type Owner, string Name), Func<object, object?>?> delegates)
		: this(delegates, new PathAwareDelegates(delegates)) {}

	public Delegates(ISelect<(Type Owner, string Name), Func<object, object?>?> delegates, IDelegates pathed)
	{
		_delegates = delegates;
		_pathed    = pathed;
	}

	public object? Get(FieldIdentifier parameter)
		=> parameter.FieldName.Contains('.')
			   ? _pathed.Get(parameter)
			   : _delegates.Get(parameter.Key())?.Invoke(parameter.Model);
}
// TODO
sealed class PathAwareDelegates : IDelegates
{
	readonly ISelect<(Type Owner, string Name), Func<object, object?>?> _delegates;

	public PathAwareDelegates(ISelect<(Type Owner, string Name), Func<object, object?>?> delegates)
		=> _delegates = delegates;

	public object? Get(FieldIdentifier parameter)
	{
		var result = parameter.Model;
		foreach (var name in parameter.FieldName.Split('.'))
		{
			var @delegate = _delegates.Get((result.GetType(), name));
			if (@delegate is not null)
			{
				result = @delegate(result);
				if (result is null)
				{
					return null;
				}
			}
		}

		return result;
	}
}