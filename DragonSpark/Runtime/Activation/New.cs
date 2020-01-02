using System;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Invocation.Expressions;

namespace DragonSpark.Runtime.Activation
{
	public sealed class New<TIn, TOut> : Select<TIn, TOut>
	{
		public static ISelect<TIn, TOut> Default { get; } = new New<TIn, TOut>();

		New() : base(Start.A.Selection(ConstructorLocator.Default)
		                  .Select(new ParameterConstructors<TIn, TOut>(ConstructorExpressions.Default))
		                  .Unless(Start.An.Instance(new ConstructorLocator(HasSingleParameterConstructor<TIn>.Default))
		                               .Select(ParameterConstructors<TIn, TOut>.Default.Assigned()))
		                  .Get(A.Metadata<TOut>())) {}
	}

	public sealed class New<T> : FixedActivator<T>
	{
		public static implicit operator Func<T>(New<T> instance) => instance.Get;

		public static New<T> Default { get; } = new New<T>();

		New() : base(Start.A.Selection(TypeMetadata.Default)
		                  .Select(ConstructorLocator.Default)
		                  .Select(Constructors<T>.Default)
		                  .Then()
		                  .Invoke()
		                  .Get()) {}
	}
}