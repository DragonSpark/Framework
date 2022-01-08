using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition.Compose;

sealed class DependencyCandidates : ArrayStore<Type, Type>
{
	public static DependencyCandidates Default { get; } = new();

	DependencyCandidates() : base(x => Constructors.Default.Select(new Select(x)).Get(x)) {}

	sealed class Select : ISelect<IEnumerable<ConstructorInfo>, Type[]>
	{
		readonly Func<ParameterInfo, Type> _select;
		readonly Func<Type, bool>          _where;

		public Select(Type source)
			: this(ParameterType.Default.Then().Select(new GenericTypeDependencySelector(source)).Get,
			       IsClass.Default.Then().And(Is.AssignableFrom<Delegate>().Inverse()).Get) {}

		public Select(Func<ParameterInfo, Type> select, Func<Type, bool> where)
		{
			_select = select;
			_where  = where;
		}

		public Type[] Get(IEnumerable<ConstructorInfo> parameter) => parameter.SelectMany(x => x.GetParameters())
		                                                                      .AsValueEnumerable()
		                                                                      .Select(_select)
		                                                                      .Where(_where)
		                                                                      .Distinct()
		                                                                      .ToArray();
	}
}