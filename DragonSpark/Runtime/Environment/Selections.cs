using System;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Query;
using DragonSpark.Reflection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Environment
{
	sealed class Selections : ISelect<Type, ISelect<Type, Type>>
	{
		public static Selections Default { get; } = new Selections();

		Selections() : this(Start.A.Selection.Of.System.Type.By.Returning(Default<Type, Type>.Instance)
		                         .Unless(IsDefinedGenericType.Default, Make.Instance)) {}

		readonly ISelect<Type, ISelect<Type, Type>> _default;

		public Selections(ISelect<Type, ISelect<Type, Type>> @default) => _default = @default;

		public ISelect<Type, Type> Get(Type parameter)
			=> _default.Get(parameter)
			           .Unless(parameter.To(ResultDefinition.Default.Get)
			                            .To(I<Specification>.Default))
			           .Unless(parameter.To(I<Specification>.Default));

		sealed class Specifications : Instance<ISelect<Type, ICondition<Type>>>
		{
			public static Specifications Instance { get; } = new Specifications();

			Specifications() : this(TypeMetadata.Default) {}

			public Specifications(ISelect<Type, TypeInfo> metadata)
				: base(metadata.Select(GenericInterfaceImplementations.Default)
				               .Select(x => x.Condition.ToDelegate())
				               .Then()
				               .Activate<OneItemIs<Type>>()
				               .Select(metadata.Select(GenericInterfaces.Default)
				                               .Open()
				                               .Select)
				               .Select(x => x.ToCondition())
				               .Get()) {}
		}

		sealed class Make : ISelect<Type, ISelect<Type, Type>>, IActivateUsing<Type>
		{
			public static Make Instance { get; } = new Make();

			Make() : this(Specifications.Instance.Get(),
			              GenericArguments.Default.Then().Activate<GenericTypeBuilder>().Get(),
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

		sealed class Specification : Conditional<Type, Type>, IActivateUsing<Type>
		{
			public Specification(Type type) : base(new IsAssignableFrom(type).Get, Delegates<Type>.Self) {}
		}
	}
}