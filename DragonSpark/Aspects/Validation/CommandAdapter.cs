using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using System;
using System.Reflection;
using System.Windows.Input;

namespace DragonSpark.Aspects.Validation
{
	public sealed class CommandAdapter : ParameterValidationAdapterBase<object>
	{
		readonly static Func<MethodInfo, bool> Method = MethodEqualitySpecification.For( typeof(ICommand).GetTypeInfo().GetDeclaredMethod( nameof(ICommand.Execute) ) );

		public CommandAdapter( ICommand inner ) : base( new DelegatedSpecification<object>( inner.CanExecute ), Method ) {}
	}

	public sealed class CommandAdapter<T> : ParameterValidationAdapterBase<T>
	{
		readonly static Func<MethodInfo, bool> Method = MethodEqualitySpecification.For( typeof(ICommand<T>).GetTypeInfo().GetDeclaredMethod( nameof(ICommand<T>.Execute) ) );

		public CommandAdapter( ICommand<T> inner ) : this( inner, new CommandAdapter( inner ) ) {}
		CommandAdapter( ISpecification<T> inner, ISpecification<object> fallback ) : base( inner, fallback, Method ) {}
	}
}