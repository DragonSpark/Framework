using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Reflection.Types;
using System;
using System.Collections;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Forms;

public sealed class ModelValidator : ICondition<object>
{
	public static ModelValidator Default { get; } = new();

	ModelValidator() : this(IsRecord.Default) {}

	readonly ICondition<Type> _record;

	public ModelValidator(ICondition<Type> record) => _record = record;

	public bool Get(object parameter)
	{
		var instance = Instance(parameter);
		if (instance.HasValue)
		{
			return instance.Value;
		}

		var type = parameter.GetType();

		if (_record.Get(type))
		{
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			var context    = new NullabilityInfoContext();

			foreach (var property in properties)
			{
				if (!Property(parameter, property, context))
				{
					return false;
				}
			}
		}

		return true;
	}

	bool? Instance(object parameter)
	{
		if (parameter is IEnumerable enumerable)
		{
			foreach (var item in enumerable)
			{
				if (item is not null && !Get(item))
				{
					return false;
				}
			}

			return true;
		}

		return null;
	}

	bool Property(object parameter, PropertyInfo property, NullabilityInfoContext context)
	{
		if (property.GetIndexParameters().Length == 0 && property.Name != "EqualityContract")
		{
			var value = property.GetValue(parameter);
			var info  = context.Create(property);

			if ((info.WriteState == NullabilityState.NotNull || info.ReadState == NullabilityState.NotNull) &&
			    value is null)
			{
				return false;
			}

			if (value is not null && _record.Get(property.PropertyType) && !Get(value))
			{
				return false;
			}
		}

		return true;
	}
}