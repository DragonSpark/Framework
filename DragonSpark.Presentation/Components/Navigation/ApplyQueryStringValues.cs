using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DragonSpark.Presentation.Components.Navigation;

/// <summary>
/// ATTRIBUTION: https://www.meziantou.net/bind-parameters-from-the-query-string-in-blazor.htm
/// </summary>
sealed class ApplyQueryStringValues : IApplyQueryStringValues
{
	readonly Func<Dictionary<string, StringValues>>    _values;
	readonly ISelect<Type, Array<QueryStringProperty>> _properties;
	readonly IPropertyAssignmentDelegate               _delegate;

	public ApplyQueryStringValues(NavigationManager manager)
		: this(QueryString.Default.Then().Bind(manager), StoredQueryStringProperties.Default,
		       PropertyAssignmentDelegates.Default) {}

	public ApplyQueryStringValues(Func<Dictionary<string, StringValues>> values,
	                              ISelect<Type, Array<QueryStringProperty>> properties,
	                              IPropertyAssignmentDelegate @delegate)
	{
		_values     = values;
		_properties = properties;
		_delegate   = @delegate;
	}

	public void Execute(Microsoft.AspNetCore.Components.ComponentBase parameter)
	{
		var query      = _values();
		var properties = _properties.Get(parameter.GetType()).Open();
		for (var i = 0; i < properties.Length; i++)
		{
			var (property, name) = properties[i];
			if (query.TryGetValue(name, out var value))
			{
				var @delegate = _delegate.Get(property);
				var converted = Convert.ChangeType(value[0], property.PropertyType, CultureInfo.InvariantCulture);
				@delegate(parameter, converted);
			}
		}
	}
}