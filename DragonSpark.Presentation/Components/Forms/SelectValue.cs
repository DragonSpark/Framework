using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class SelectValue<T> : ISelect<FieldIdentifier, T>
	{
		public static SelectValue<T> Default { get; } = new SelectValue<T>();

		SelectValue()
			: this(new Generic<ISelect<PropertyInfo, Func<object, T>>>(typeof(SelectValue<,>)), A.Type<T>()) {}

		readonly IGeneric<ISelect<PropertyInfo, Func<object, T>>> _generic;
		readonly Type _type;

		public SelectValue(IGeneric<ISelect<PropertyInfo, Func<object, T>>> generic, Type type)
		{
			_generic = generic;
			_type = type;
		}

		public T Get(FieldIdentifier parameter)
		{
			var type = parameter.Model.GetType();
			var result = _generic.Get(type, _type).Get(type.GetProperty(parameter.FieldName).Verify())(parameter.Model);
			return result;
		}
	}

	sealed class SelectValue<T, TValue> : Select<PropertyInfo, Func<object, TValue>>
	{
		[UsedImplicitly]
		public static SelectValue<T, TValue> Default { get; } = new SelectValue<T, TValue>();

		SelectValue() : base(DefaultPropertyDelegate<T, TValue>.Default.Then().Stores().Reference().Get) {}
	}
}