using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using System;
using System.Reflection;

namespace DragonSpark.Runtime.Environment
{
	sealed class Selections : ISelect<Type, ISelect<Type, Type>>
	{
		public static Selections Default { get; } = new Selections();

		Selections() : this(Start.A.Selection.Of.System.Type.By.Returning(Default<Type, Type>.Instance)
		                         .Unless.Input.Is(IsDefinedGenericType.Default)
		                         .ThenUse(Make.Instance),
		                    ResultDefinition.Default.Get) {}

		readonly Func<Type, ISelect<Type, Type>> _default;
		readonly Func<Type, Type>                _result;

		public Selections(Func<Type, ISelect<Type, Type>> @default, Func<Type, Type> result)
		{
			_default = @default;
			_result  = result;
		}

		public ISelect<Type, Type> Get(Type parameter)
			=> _default(parameter)
			   .Then()
			   .Unless.Using(new Specification(_result(parameter)))
			   .ResultsInAssigned()
			   .Unless.Using(new Specification(parameter))
			   .ResultsInAssigned()
			   .Get();

		sealed class Specification : Conditional<Type, Type>, IActivateUsing<Type>
		{
			public Specification(Type type) : base(new IsAssignableFrom(type).Get, Delegate.Self<Type>()) {}
		}

		sealed class Make : ISelect<Type, ISelect<Type, Type>>, IActivateUsing<Type>
		{
			public static Make Instance { get; } = new Make();

			Make() : this(Specifications.Instance.Get(),
			              GenericArguments.Default.Then().Subject.StoredActivation<GenericTypeBuilder>().Get(),
			              IsGenericTypeDefinition.Default) {}

			readonly ISelect<Type, ISelect<Type, Type>> _source;

			readonly ISelect<Type, ICondition<Type>> _specification;
			readonly ICondition<Type>                _valid;

			public Make(ISelect<Type, ICondition<Type>> specification,
			            ISelect<Type, ISelect<Type, Type>> source, ICondition<Type> valid)
			{
				_specification = specification;
				_source        = source;
				_valid         = valid;
			}

			public ISelect<Type, Type> Get(Type parameter)
				=> _source.Get(parameter)
				          .Then()
				          .Ensure.Input.Is(_valid.Then()
				                                 .And(_specification.Get(parameter))
				                                 .Out())
				          .Otherwise.UseDefault()
				          .Get();
		}

		sealed class Specifications : Instance<ISelect<Type, ICondition<Type>>>
		{
			public static Specifications Instance { get; } = new Specifications();

			Specifications() : this(TypeMetadata.Default) {}

			public Specifications(ISelect<Type, TypeInfo> metadata)
				: base(metadata.Select(GenericInterfaceImplementations.Default)
				               .Select(x => x.Condition.ToDelegate())
				               .Then()
				               .StoredActivation<OneItemIs<Type>>()
				               .Select(metadata.Select(GenericInterfaces.Default).Open().Select)
				               .Select(x => x.Then().Out())
				               .Get()) {}
		}
	}
}