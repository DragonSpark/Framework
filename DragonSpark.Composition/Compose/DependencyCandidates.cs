using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition.Compose
{
	sealed class DependencyCandidates : ArrayStore<Type, Type>
	{
		public static DependencyCandidates Default { get; } = new DependencyCandidates();

		DependencyCandidates() : base(x => Constructors.Default.Select(new Plan(x)).Get(x)) {}

		sealed class Plan : IArray<ICollection<ConstructorInfo>, Type>
		{
			readonly Func<ConstructorInfo, ParameterInfo[]> _parameters;
			readonly Func<ParameterInfo, Type>              _select;
			readonly Func<Type, bool>                       _where;

			public Plan(Type source)
				: this(Parameters.Default.Open().Get,
				       ParameterType.Default.Select(new GenericTypeDependencySelector(source)).Get,
				       IsClass.Default.Then().And(Is.AssignableFrom<Delegate>().Inverse()).Get) {}

			public Plan(Func<ConstructorInfo, ParameterInfo[]> parameters, Func<ParameterInfo, Type> select,
			            Func<Type, bool> where)
			{
				_parameters = parameters;
				_select     = select;
				_where      = where;
			}

			public Array<Type> Get(ICollection<ConstructorInfo> parameter) => parameter.SelectMany(_parameters)
			                                                                           .Select(_select)
			                                                                           .Where(_where)
			                                                                           .Distinct()
			                                                                           .ToArray();
		}
	}
}