using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms
{
	sealed class SelectValue<T> : ISelect<FieldIdentifier, T>
	{
		public static SelectValue<T> Default { get; } = new SelectValue<T>();

		SelectValue() : this(PropertyValueDelegates<T>.Default) {}

		readonly IPropertyValueDelegate<T> _delegates;

		public SelectValue(IPropertyValueDelegate<T> delegates) => _delegates = delegates;

		public T Get(FieldIdentifier parameter)
		{
			var property = parameter.Model.GetType().GetProperty(parameter.FieldName).Verify();
			var result   = _delegates.Get(property)(parameter.Model);
			return result;
		}
	}
}