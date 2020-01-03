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
		                         .Unless(IsDefinedGenericType.Default, Make.Instance),
		                    ResultDefinition.Default.Get) {}

		readonly ISelect<Type, ISelect<Type, Type>> _default;
		readonly Func<Type, Type> _result;

		public Selections(ISelect<Type, ISelect<Type, Type>> @default, Func<Type, Type> result)
		{
			_default = @default;
			_result = result;
		}

		public ISelect<Type, Type> Get(Type parameter)
			=> _default.Get(parameter)
			           .Unless(new Specification(_result(parameter)))
			           .Unless(new Specification(parameter));

		sealed class Specification : Conditional<Type, Type>, IActivateUsing<Type>
		{
			public Specification(Type type) : base(new IsAssignableFrom(type).Get, Delegate.Self<Type>()) {}
		}

		sealed class Make : ISelect<Type, ISelect<Type, Type>>, IActivateUsing<Type>
		{
			public static Make Instance { get; } = new Make();

			Make() : this(Specifications.Instance.Get(),
			              GenericArguments.Default.Then().Select<GenericTypeBuilder>().Get(),
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

			public ISelect<Type, Type> Get(Type parameter) => _valid.Then()
			                                                        .And(_specification.Get(parameter))
			                                                        .Out()
			                                                        .To(_source.Get(parameter).If);
		}

		sealed class Specifications : Instance<ISelect<Type, ICondition<Type>>>
		{
			public static Specifications Instance { get; } = new Specifications();

			Specifications() : this(TypeMetadata.Default) {}

			public Specifications(ISelect<Type, TypeInfo> metadata)
				: base(metadata.Select(GenericInterfaceImplementations.Default)
				               .Select(x => x.Condition.ToDelegate())
				               .Then()
				               .Select<OneItemIs<Type>>()
				               .Select(metadata.Select(GenericInterfaces.Default)
				                               .Open()
				                               .Select)
				               .Select(x => x.ToCondition())
				               .Get()) {}
		}
	}
}