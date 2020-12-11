using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public sealed class GeneratorState
	{
		readonly ITypedTable<object>                _instances;
		readonly ISelect<TypeInfo, IFakerTInternal> _generators;

		public GeneratorState() : this(new TypedTable<object>(), GeneratorTables.Default.ToStandardTable()) {}

		public GeneratorState(ITypedTable<object> instances, ISelect<TypeInfo, IFakerTInternal> generators)
		{
			_instances  = instances;
			_generators = generators;
		}

		public Faker<T> Get<T>() where T : class => _generators.Get(A.Type<T>()).To<Faker<T>>();

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Expression<Func<TOther, T>> other,
		                                                         Including<T, TOther> including)
			where TOther : class
		{
			var includes = including(Include.New<T, TOther>()).Complete();
			var configure = LocateAssignments<TOther, T>.Default.Get(other.GetMemberAccess().Name)
			                                            .Get()
			                                            .Verify($"The expression '{other}' did not resolve to a valid assignment setter.");
			var assign    = new Assign<T, TOther>(includes.Post, configure);
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var current   = new Rule<T, TOther>(generator, includes.Generate, assign.Execute);
			var result    = generator.Pair(includes.Scope(new Scope<T, TOther>(current, _instances)));
			return result;
		}

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Including<T, TOther> including)
			where TOther : class
		{
			var includes   = including(Include.New<T, TOther>()).Complete();
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure = assignment != null
				                ? new Assign<T, TOther>(includes.Post, assignment).Execute
				                : includes.Post;
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var current   = new Rule<T, TOther>(generator, includes.Generate, configure);
			var result    = generator.Pair(includes.Scope(new Scope<T, TOther>(current, _instances)));
			return result;
		}
	}
}