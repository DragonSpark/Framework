﻿using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	sealed class SingletonProperty : ReferenceValueStore<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		public SingletonProperty(IResult<Array<string>> candidates)
			: base(Start.A.Selection.Of.System.Type.By.Delegate<string, PropertyInfo>(x => x.GetProperty)
			            .Select(candidates.Select)
			            .Get() // TODO: Get/Then
			            .Then()
			            .Value()
			            .Select(Query.Instance)) {}

		sealed class Query : ISelect<PropertyInfo[], PropertyInfo>
		{
			public static Query Instance { get; } = new Query();

			Query() : this(IsSingletonProperty.Default.Get, Is.Assigned()) {}

			readonly Func<PropertyInfo, bool> _is;
			readonly Func<PropertyInfo, bool> _assigned;

			public Query(Func<PropertyInfo, bool> @is, Func<PropertyInfo, bool> assigned)
			{
				_is       = @is;
				_assigned = assigned;
			}

			public PropertyInfo Get(PropertyInfo[] parameter) => parameter.Where(_is).FirstOrDefault(_assigned);
		}
	}
}