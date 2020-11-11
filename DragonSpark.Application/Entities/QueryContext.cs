using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Entities
{
#pragma warning disable EF1001
	/// <summary>
	/// Attribution: https://stackoverflow.com/a/53340563/3602057
	/// </summary>
	sealed class QueryContext : ISelect<IQueryProvider, DbContext>
	{
		public static QueryContext Default { get; } = new QueryContext();

		QueryContext() : this(BindingFlags.NonPublic | BindingFlags.Instance) {}

		readonly Fields       _fields;
		readonly PropertyInfo _stateManager;

		public QueryContext(BindingFlags flags)
			: this(new Fields(typeof(EntityQueryProvider).GetField("_queryCompiler", flags).Verify(),
			                  typeof(QueryCompiler).GetField("_queryContextFactory", flags).Verify(),
			                  typeof(RelationalQueryContextFactory).GetField("_dependencies", flags).Verify()),
			       typeof(DbContext).Assembly.GetType(typeof(QueryContextDependencies).FullName.Verify())
			                        .Verify()
			                        .GetProperty("StateManager", flags | BindingFlags.Public)
			                        .Verify()) {}

		public QueryContext(Fields fields, PropertyInfo stateManager)
		{
			_fields       = fields;
			_stateManager = stateManager;
		}

		public DbContext Get(IQueryProvider parameter)
		{

			var compiler     = _fields.Compiler.GetValue(parameter).Verify();
			var factory      = _fields.Factory.GetValue(compiler);
			var dependencies = _fields.Dependencies.GetValue(factory);
			var result       = _stateManager.GetValue(dependencies).Verify().To<IStateManager>().Context;
			return result;
		}

		public sealed class Fields
		{
			public Fields(FieldInfo compiler, FieldInfo factory, FieldInfo dependencies)
			{
				Compiler     = compiler;
				Factory      = factory;
				Dependencies = dependencies;
			}

			public FieldInfo Compiler { get; }

			public FieldInfo Factory { get; }

			public FieldInfo Dependencies { get; }

			public void Deconstruct(out FieldInfo compiler, out FieldInfo factory, out FieldInfo dependencies)
			{
				compiler     = Compiler;
				factory      = Factory;
				dependencies = Dependencies;
			}
		}
	}
#pragma warning restore
}
