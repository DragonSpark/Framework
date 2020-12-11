using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Types;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members
{

	public sealed class GeneralPropertyValueDelegates : ReferenceValueStore<PropertyInfo, Func<object, object>>,
	                                                    IPropertyValueDelegate
	{
		public static GeneralPropertyValueDelegates Default { get; } = new GeneralPropertyValueDelegates();

		GeneralPropertyValueDelegates() : base(GeneralPropertyValueDelegate.Default.Then().Stores().New().Get) {}
	}

	sealed class GeneralPropertyValueDelegate : IPropertyValueDelegate
	{
		readonly IGeneric<IPropertyValueDelegate> _generic;

		public static GeneralPropertyValueDelegate Default { get; } = new GeneralPropertyValueDelegate();

		GeneralPropertyValueDelegate()
			: this(new Generic<IPropertyValueDelegate>(typeof(PropertyValueDelegateAdapter<,>))) {}

		public GeneralPropertyValueDelegate(IGeneric<IPropertyValueDelegate> generic) => _generic = generic;

		public Func<object, object> Get(PropertyInfo parameter)
			=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
			           .Get(parameter);
	}

	sealed class PropertyValueDelegateAdapter<T, TValue> : Select<PropertyInfo, Func<object, object>>,
	                                                       IPropertyValueDelegate
	{
		public static PropertyValueDelegateAdapter<T, TValue> Default { get; }
			= new PropertyValueDelegateAdapter<T, TValue>();

		PropertyValueDelegateAdapter()
			: base(Start.An.Instance(PropertyDelegateAdapter<T, TValue>.Default)
			            .Then()
			            .Select(x => x.Start().Cast<object>().Get().ToDelegate())) {}
	}

	public sealed class GeneralPropertyValueDelegates<T> : ReferenceValueStore<PropertyInfo, Func<object, T>>, IPropertyValueDelegate<T>
	{
		public static GeneralPropertyValueDelegates<T> Default { get; } = new GeneralPropertyValueDelegates<T>();

		GeneralPropertyValueDelegates() : base(GeneralPropertyValueDelegate<T>.Default.Then().Stores().New().Get) {}
	}

	sealed class GeneralPropertyValueDelegate<T> : IPropertyValueDelegate<T>
	{
		readonly IGeneric<IPropertyValueDelegate<T>> _generic;

		public static GeneralPropertyValueDelegate<T> Default { get; } = new GeneralPropertyValueDelegate<T>();

		GeneralPropertyValueDelegate()
			: this(new Generic<IPropertyValueDelegate<T>>(typeof(PropertyDelegateAdapter<,>))) {}

		public GeneralPropertyValueDelegate(IGeneric<IPropertyValueDelegate<T>> generic) => _generic = generic;

		public Func<object, T> Get(PropertyInfo parameter)
			=> _generic.Get(parameter.DeclaringType ?? parameter.ReflectedType.Verify(), parameter.PropertyType)
			           .Get(parameter);
	}


	sealed class PropertyDelegateAdapter<T, TValue> : Select<PropertyInfo, Func<object, TValue>>,
	                                                  IPropertyValueDelegate<TValue>
	{
		public static PropertyDelegateAdapter<T, TValue> Default { get; } = new PropertyDelegateAdapter<T, TValue>();

		PropertyDelegateAdapter() : base(Start.An.Instance(PropertyValueDelegates<T, TValue>.Default)
		                                      .Select(Start.A.Selection.Of.Any.By.CastDown<T>().Get().Select)
		                                      .Then()
		                                      .Delegate()) {}
	}

	public sealed class PropertyValueDelegates<T, TValue> : ReferenceValueTable<PropertyInfo, Func<T, TValue>>,
	                                                        IPropertyValueDelegate<T, TValue>
	{
		public static PropertyValueDelegates<T, TValue> Default { get; } = new PropertyValueDelegates<T, TValue>();

		PropertyValueDelegates() : base(PropertyValueDelegate<T, TValue>.Default.Then().Stores().New().Get) {}
	}
}