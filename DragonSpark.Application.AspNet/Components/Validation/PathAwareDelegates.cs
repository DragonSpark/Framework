using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace DragonSpark.Application.Components.Validation;

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