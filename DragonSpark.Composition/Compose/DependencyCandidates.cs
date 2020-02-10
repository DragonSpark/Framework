using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
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
			public Plan(Type source)
				: this(Parameters.Default.Open().Get,
				       ParameterType.Default.Select(new GenericTypeDependencySelector(source)).Get,
				       IsClass.Default.Then().And(Is.AssignableFrom<Delegate>().Inverse()).Get) {}

			readonly Func<ConstructorInfo, ParameterInfo[]> _parameters;
			readonly Func<ParameterInfo, Type>              _select;
			readonly Func<Type, bool>                       _where;

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

	sealed class RegisterDependencies : ICommand<Type>
	{
		readonly Func<Type, IServiceCollection> _apply;
		readonly Func<Type, bool>               _can;
		readonly IArray<Type, Type>             _candidates;

		public RegisterDependencies(IServiceCollection services) : this(services, services.AddScoped) {}

		public RegisterDependencies(IServiceCollection services, Func<Type, IServiceCollection> apply)
			: this(apply, new CanRegister(services).Get, DependencyCandidates.Default) {}

		public RegisterDependencies(Func<Type, IServiceCollection> apply, Func<Type, bool> can,
		                            IArray<Type, Type> candidates)
		{
			_apply      = apply;
			_can        = can;
			_candidates = candidates;
		}

		public void Execute(Type parameter)
		{
			foreach (var candidate in _candidates.Get(parameter).Open().Where(_can))
			{
				_apply(candidate);
			}
		}
	}

	sealed class CanRegister : Condition<Type>
	{
		public CanRegister(IServiceCollection services)
			: base(new NotHave<Type>(services.Select(x => x.ServiceType))) {}
	}
}