using DragonSpark.Model.Results;
using DragonSpark.Runtime.Execution;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition
{
	internal class Class1 {}

	public interface IScopes : IResult<IServiceScope> {}

	sealed class Scopes : IScopes
	{
		readonly IServiceScopeFactory _scopes;

		public Scopes(IServiceScopeFactory scopes) => _scopes = scopes;

		public IServiceScope Get() => _scopes.CreateScope();
	}

	public interface IScoping : IResult<AsyncServiceScope> {}

	sealed class Scoping : IScoping
	{
		readonly IServiceScopeFactory _scopes;

		public Scoping(IServiceScopeFactory scopes) => _scopes = scopes;

		public AsyncServiceScope Get() => _scopes.CreateAsyncScope();
	}

	public sealed class LogicalScope : Logical<AsyncServiceScope?>
	{
		public static LogicalScope Default { get; } = new();

		LogicalScope() {}
	}

	/*
	sealed class LogicalAwareScopes : IScopes
	{
		readonly IScopes      _previous;
		readonly LogicalScope _scope;

		public LogicalAwareScopes(IScopes previous) : this(previous, LogicalScope.Default) {}

		public LogicalAwareScopes(IScopes previous, LogicalScope scope)
		{
			_previous = previous;
			_scope    = scope;
		}

		public AsyncServiceScope Get()
		{
			var current = _scope.Get();
			if (current.HasValue)
			{
				return current.Value;
			}

			var result = _previous.Get();
			_scope.Execute(result);
			return result;
		}
	}
*/
	public sealed class AmbientProvider : IResult<IServiceProvider?>
	{
		public static AmbientProvider Default { get; } = new();

		AmbientProvider() : this(LogicalScope.Default) {}

		readonly IResult<AsyncServiceScope?> _scope;

		public AmbientProvider(IResult<AsyncServiceScope?> scope) => _scope = scope;

		public IServiceProvider? Get() => _scope.Get()?.ServiceProvider;
	}
}