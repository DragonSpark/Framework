using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.TypeSystem;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Aspects.Validation
{
	static class Defaults
	{
		public static ImmutableArray<ITypeDefinition> Definitions { get; } = ImmutableArray.Create<ITypeDefinition>( ParameterizedSourceTypeDefinition.Default, GenericCommandTypeDefinition.Default, CommandTypeDefinition.Default );

		readonly static ImmutableArray<Func<object, IParameterValidationAdapter>> AdapterSources = ImmutableArray.Create<Func<object, IParameterValidationAdapter>>(
			ParameterizedSourceAdapterFactory.Default.Get,
			GenericCommandAdapterFactory.Default.Get,
			CommandAdapterFactory.Default.Get 
		);

		public static IEnumerable<ValueTuple<TypeAdapter, Func<object, IParameterValidationAdapter>>> Factories { get; } = Definitions.Select( definition => definition.DeclaringType.Adapt() ).Tuple( AdapterSources.ToArray() );

		public static Func<object, IAutoValidationController> ControllerSource { get; } = AutoValidationControllerFactory.Default.Get;
	}
}