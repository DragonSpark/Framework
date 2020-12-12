using Bogus;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public sealed class GeneratorState
	{
		readonly ITypedTable<object>                _instances;
		readonly ITypedTable<object>                _chain;
		readonly ISelect<TypeInfo, IFakerTInternal> _generators;

		public GeneratorState(Configuration configuration)
			: this(new TypedTable<object>(), new TypedTable<object>(),
			       new GeneratorTables(configuration).ToStandardTable()) {}

		public GeneratorState(ITypedTable<object> instances, ITypedTable<object> chain,
		                      ISelect<TypeInfo, IFakerTInternal> generators)
		{
			_instances  = instances;
			_chain      = chain;
			_generators = generators;
		}

		public Faker<T> Get<T>() where T : class => _generators.Get(A.Type<T>()).To<Faker<T>>();

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Expression<Func<TOther, T>> other,
		                                                         Including<T, TOther> including)
			where TOther : class where T : class
		{
			var payload = including(Include.New<T, TOther>()).Complete();
			var configure = LocateAssignments<TOther, T>.Default.Get(other.GetMemberAccess().Name)
			                                            .Get()
			                                            .Verify($"The expression '{other}' did not resolve to a valid assignment setter.");
			var result = Rule(payload.With(new Assign<T, TOther>(payload.Configure, configure).Execute));
			return result;
		}

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Including<T, TOther> including)
			where TOther : class where T : class
		{
			var payload    = including(Include.New<T, TOther>()).Complete();
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure = assignment != null
				                ? new Assign<T, TOther>(payload.Configure, assignment).Execute
				                : payload.Configure;
			var result = Rule(payload.With(configure));
			return result;
		}

		(Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Include<T, TOther>.Payload payload)
			where TOther : class where T : class
		{
			var (generate, configure, scope) = payload;
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var create    = new StateAwareGenerate<T, TOther>(new Generate<T, TOther>(generator, generate), _chain);
			var current   = new Rule<T, TOther>(create, configure);
			var result    = generator.Pair(scope(new Scope<T, TOther>(current, _instances)));
			return result;
		}

		public (Faker<TOther>, IRule<T, List<TOther>>) RuleMany<T, TOther>(Expression<Func<TOther, T>> other,
		                                                                   IncludingMany<T, TOther> including)
			where TOther : class where T : class
		{
			var payload = including(Include.Many<T, TOther>()).Complete();
			var configure = LocateAssignments<TOther, T>.Default.Get(other.GetMemberAccess().Name)
			                                            .Get()
			                                            .Verify($"The expression '{other}' did not resolve to a valid assignment setter.");
			var result = Many(payload.With(new AssignMany<T, TOther>(payload.Configure, configure).Execute));
			return result;
		}

		public (Faker<TOther>, IRule<T, List<TOther>>) RuleMany<T, TOther>(IncludingMany<T, TOther> including)
			where TOther : class where T : class
		{
			var payload    = including(Include.Many<T, TOther>()).Complete();
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure = assignment != null
				                ? new AssignMany<T, TOther>(payload.Configure, assignment).Execute
				                : payload.Configure;
			var result = Many(payload.With(configure));
			return result;
		}

		(Faker<TOther>, IRule<T, List<TOther>>) Many<T, TOther>(IncludeMany<T, TOther>.Payload payload)
			where TOther : class where T : class
		{
			var (generate, configure) = payload;
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var list      = new GenerateList<T, TOther>(generator, generate);
			var create    = new StateAwareGenerate<T, List<TOther>>(list, _chain);
			var current   = new ManyRule<T, TOther>(create, configure);
			var result    = generator.Pair(current);
			return result;
		}
	}
}