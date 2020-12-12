using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class MultipleCandidatePrincipalProperty<T, TValue> : ISelect<Memory<PropertyInfo>, PropertyInfo?>
	{
		readonly ISelect<Memory<PropertyInfo>, PropertyInfo?> _previous;

		public MultipleCandidatePrincipalProperty(ISelect<Memory<PropertyInfo>, PropertyInfo?> previous)
			=> _previous = previous;

		public PropertyInfo? Get(Memory<PropertyInfo> parameter)
		{
			var previous = _previous.Get(parameter);

			var result = previous == null && parameter.Length > 0
				             ? throw new
					               InvalidOperationException($"Could not locate a definitive property on type '{A.Type<T>()}' that is of type '{A.Type<TValue>()}'.  Manually select this property via the `Include` override.")
				             : previous;
			return result;
		}
	}
}