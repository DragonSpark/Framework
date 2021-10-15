using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class GeneralPropertyAssignmentDelegateAdapter<T, TValue> : Select<PropertyInfo, Action<object, object>>,
                                                                   IPropertyAssignmentDelegate
{
	public static GeneralPropertyAssignmentDelegateAdapter<T, TValue> Default { get; } = new();

	GeneralPropertyAssignmentDelegateAdapter()
		: base(Start.An.Instance(PropertyAssignmentDelegate<T, TValue>.Default)
		            .Select(x => new Adapter(x).ToAssignmentDelegate())) {}

	sealed class Adapter : IAssign<object, object>
	{
		readonly Action<T, TValue> _assign;

		public Adapter(Action<T, TValue> assign) => _assign = assign;

		public void Execute(Pair<object, object> parameter)
		{
			var (key, value) = parameter;
			_assign(key.To<T>(), value.To<TValue>());
		}
	}

}