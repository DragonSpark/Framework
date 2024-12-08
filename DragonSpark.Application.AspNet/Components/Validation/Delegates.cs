using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components.Forms;
using System;

namespace DragonSpark.Application.AspNet.Components.Validation;

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