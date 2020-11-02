using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components;
using NetFabric.Hyperlinq;
using System;
using System.Reflection;
using A = DragonSpark.Compose.A;

namespace DragonSpark.Presentation.Components
{
	sealed class Delegates : Generic<IArray<Func<ComponentBase, IOperation>>>
	{
		public static Delegates Default { get; } = new Delegates();

		Delegates() : base(typeof(Delegates<>)) {}
	}

	sealed class Delegates<T> : IArray<Func<ComponentBase, IOperation>> where T : ComponentBase
	{
		[UsedImplicitly]
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : this(Start.A.Selection<PropertyInfo>()
		                        .By.Calling(x => x.PropertyType)
		                        .Select(Is.AssignableFrom<IViewProperty>())
		                        .Then()
		                        .And(Is.DecoratedWith<ParameterAttribute>()),
		                   DefaultPropertyDelegate<T, IOperation>.Default.Get,
		                   A.Type<T>().GetRuntimeProperties().Result()) {}

		readonly Predicate<PropertyInfo> _where;
		readonly NullableSelector<PropertyInfo, Func<ComponentBase, IOperation>> _select;
		readonly Array<PropertyInfo>                                 _properties;

		public Delegates(Predicate<PropertyInfo> where,
		                 NullableSelector<PropertyInfo, Func<ComponentBase, IOperation>> select,
		                 Array<PropertyInfo> properties)
		{
			_where      = @where;
			_select     = select;
			_properties = properties;
		}

		public Array<Func<ComponentBase, IOperation>> Get() => _properties.Open()
		                                                                  .Where(_where)
		                                                                  .Select(_select)
		                                                                  .ToList()
		                                                                  .Result();
	}
}