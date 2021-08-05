using DragonSpark.Compose;
using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components;
using NetFabric.Hyperlinq;
using System;
using System.Globalization;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Navigation
{
	/// <summary>
	/// ATTRIBUTION: https://www.meziantou.net/bind-parameters-from-the-query-string-in-blazor.htm
	/// </summary>
	sealed class ApplyQueryStringValues : IApplyQueryStringValues
	{
		readonly NavigationManager           _manager;
		readonly IPropertyAssignmentDelegate _delegate;
		readonly IQueryString                _query;

		public ApplyQueryStringValues(NavigationManager manager)
			: this(manager, PropertyAssignmentDelegates.Default, QueryString.Default) {}

		public ApplyQueryStringValues(NavigationManager manager, IPropertyAssignmentDelegate @delegate,
		                              IQueryString query)
		{
			_manager  = manager;
			_delegate = @delegate;
			_query    = query;
		}

		public void Execute(Microsoft.AspNetCore.Components.ComponentBase parameter)
		{
			var query = _query.Get(_manager);

			foreach (var property in parameter.GetType().GetRuntimeProperties().AsValueEnumerable())
			{
				var attribute = property.Attribute<QueryStringParameterAttribute>();
				var name      = attribute != null ? attribute.Name ?? property.Name : null;
				if (name != null && query.TryGetValue(name, out var value))
				{
					var @delegate = _delegate.Get(property);
					var converted = Convert.ChangeType(value[0], property.PropertyType, CultureInfo.InvariantCulture);
					@delegate(parameter, converted);
				}			}
		}
	}
}