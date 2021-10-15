using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class PropertyAssignmentDelegateAdapter<T, TValue> : Select<PropertyInfo, Action<object, TValue>>,
                                                            IPropertyAssignmentDelegate<TValue>
{
	public static PropertyAssignmentDelegateAdapter<T, TValue> Default { get; } = new();

	PropertyAssignmentDelegateAdapter() : base(Start.An.Instance(PropertyAssignmentDelegate<T, TValue>.Default)
	                                                .Select(x => new Adapter(x).ToAssignmentDelegate())) {}

	sealed class Adapter : IAssign<object, TValue>
	{
		readonly Action<T, TValue> _assign;

		public Adapter(Action<T, TValue> assign) => _assign = assign;

		public void Execute(Pair<object, TValue> parameter)
		{
			var (key, value) = parameter;
			_assign(key.To<T>(), value);
		}
	}
}