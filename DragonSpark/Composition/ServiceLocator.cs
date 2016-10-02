using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;

namespace DragonSpark.Composition
{
	public sealed class ServiceLocator : ActivatorBase
	{
		public ServiceLocator( CompositionContext host ) : base( new Specification( host ), new Inner( host ).Get ) {}

		sealed class Inner : ParameterizedSourceBase<Type, object>
		{
			readonly CompositionContext host;

			public Inner( CompositionContext host )
			{
				this.host = host;
			}

			public override object Get( Type parameter )
			{
				var enumerable = parameter.GetTypeInfo().IsGenericType && parameter.GetGenericTypeDefinition() == typeof(IEnumerable<>);
				var result = enumerable ? host.GetExports( parameter.Adapt().GetEnumerableType() ) : host.TryGet<object>( parameter );
				return result;
			}
		}

		sealed class Specification : SpecificationBase<Type>
		{
			readonly CompositionContext host;

			public Specification( CompositionContext host )
			{
				this.host = host;
			}

			public override bool IsSatisfiedBy( Type parameter )
			{
				var type = SpecificationExporter.Definition.MakeGenericType( parameter );
				object specification;
				var result = !host.TryGetExport( type, out specification ) || (bool)specification;
				return result;
			}
		}
	}
}