using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Invocation.Expressions;
using System.Reflection;

namespace DragonSpark.Runtime.Activation
{
	public sealed class New<TIn, TOut> : Select<TIn, TOut>
	{
		public static ISelect<TIn, TOut> Default { get; } = new New<TIn, TOut>();

		New() : this(new ConstructorLocator(HasSingleParameterConstructor<TIn>.Default)) {}

		public New(ISelect<TypeInfo, ConstructorInfo> constructor)
			: base(Start.A.Selection(ConstructorLocator.Default)
			            .Select(new ParameterConstructors<TIn, TOut>(ConstructorExpressions.Default))
			            .Then()
			            .Unless.Using(Start.A.Selection(constructor)
			                               .Select(A.Selection(ParameterConstructors<TIn, TOut>.Default)
			                                        .Then()
			                                        .EnsureAssignedOrDefault()
			                                        .Return()))
			            .ResultsInAssigned()
			            .Get(A.Metadata<TOut>())) {}
	}

	public sealed class New<T> : FixedActivator<T>
	{
		public static New<T> Default { get; } = new New<T>();

		New() : base(Start.A.Selection(TypeMetadata.Default)
		                  .Select(ConstructorLocator.Default)
		                  .Select(Constructors<T>.Default)
		                  .Then()
		                  .Invoke()) {}
	}
}