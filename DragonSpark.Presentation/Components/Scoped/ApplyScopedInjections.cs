using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Scoped
{
	sealed class ApplyScopedInjections : ICommand<ScopedInjection>
	{
		public static ApplyScopedInjections Default { get; } = new ApplyScopedInjections();

		ApplyScopedInjections() : this(Properties.Default, PropertyAssignmentDelegates.Default) {}

		readonly IPropertyAssignmentDelegate _delegate;

		readonly ISelect<Type, Array<PropertyInfo>> _properties;

		public ApplyScopedInjections(ISelect<Type, Array<PropertyInfo>> properties,
									 IPropertyAssignmentDelegate @delegate)
		{
			_properties = properties;
			_delegate   = @delegate;
		}

		public void Execute(ScopedInjection parameter)
		{
			var (target, provider) = parameter;

			var properties = _properties.Get(target.GetType());
			for (byte i = 0; i < properties.Length; i++)
			{
				var property  = properties[i];
				var @delegate = _delegate.Get(property);
				var service   = provider.GetRequiredService(property.PropertyType);
				var converted = service is IConvertible convertible
									? Convert.ChangeType(convertible, property.PropertyType,
														 CultureInfo.InvariantCulture)
									: service;
				@delegate(target, converted);
			}
		}
	}
}