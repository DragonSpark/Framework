using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ThreadAwareModel : IRuntimeModel
	{
		readonly IModel        _previous;
		readonly IRuntimeModel _model;

		public ThreadAwareModel(IModel previous) : this(previous, previous.To<IRuntimeModel>()) {}

		public ThreadAwareModel(IModel previous, IRuntimeModel model)
		{
			_previous = previous;
			_model    = model;
		}

		public IAnnotation? FindAnnotation(string name) => _previous.FindAnnotation(name);

		public IEnumerable<IAnnotation> GetAnnotations() => _previous.GetAnnotations();

		public object? this[string name] => _previous[name];

		public ChangeTrackingStrategy GetChangeTrackingStrategy() => _previous.GetChangeTrackingStrategy();

		public PropertyAccessMode GetPropertyAccessMode() => _previous.GetPropertyAccessMode();

		public bool IsShared(Type type) => _previous.IsShared(type);

		public IEnumerable<IEntityType> GetEntityTypes() => _previous.GetEntityTypes();

		IEnumerable<IReadOnlyEntityType> IReadOnlyModel.GetEntityTypes() => ((IReadOnlyModel)_previous).GetEntityTypes();

		public IEntityType? FindEntityType(string name) => _previous.FindEntityType(name);

		public IEntityType? FindEntityType(string name, string definingNavigationName, IEntityType definingEntityType) => _previous.FindEntityType(name, definingNavigationName, definingEntityType);

		IReadOnlyEntityType? IReadOnlyModel.FindEntityType(string name) => ((IReadOnlyModel)_previous).FindEntityType(name);

		public IReadOnlyEntityType? FindEntityType(string name, string definingNavigationName, IReadOnlyEntityType definingEntityType) => _previous.FindEntityType(name, definingNavigationName, definingEntityType);

		public IEntityType? FindEntityType(Type type) => _previous.FindEntityType(type);

		IReadOnlyEntityType? IReadOnlyModel.FindEntityType(Type type) => ((IReadOnlyModel)_previous).FindEntityType(type);

		public IReadOnlyEntityType? FindEntityType(Type type, string definingNavigationName, IReadOnlyEntityType definingEntityType) => _previous.FindEntityType(type, definingNavigationName, definingEntityType);

		public IEnumerable<IEntityType> FindEntityTypes(Type type) => _previous.FindEntityTypes(type);

		public bool IsIndexerMethod(MethodInfo methodInfo) => _previous.IsIndexerMethod(methodInfo);

		IEnumerable<IReadOnlyEntityType> IReadOnlyModel.FindEntityTypes(Type type) => ((IReadOnlyModel)_previous).FindEntityTypes(type);

		public IAnnotation? FindRuntimeAnnotation(string name) => _previous.FindRuntimeAnnotation(name);

		public IEnumerable<IAnnotation> GetRuntimeAnnotations() => _previous.GetRuntimeAnnotations();

		public IAnnotation AddRuntimeAnnotation(string name, object? value) => _previous.AddRuntimeAnnotation(name, value);

		public IAnnotation SetRuntimeAnnotation(string name, object? value) => _previous.SetRuntimeAnnotation(name, value);

		public IAnnotation? RemoveRuntimeAnnotation(string name) => _previous.RemoveRuntimeAnnotation(name);

		public TValue GetOrAddRuntimeAnnotationValue<TValue, TArg>(string name, Func<TArg?, TValue> valueFactory, TArg? factoryArgument)
		{
			lock (_previous)
			{
				return _previous.GetOrAddRuntimeAnnotationValue(name, valueFactory, factoryArgument);
			}
		}

		public bool SkipDetectChanges => _model.SkipDetectChanges;

		public object? RelationalModel => _model.RelationalModel;
	}
}