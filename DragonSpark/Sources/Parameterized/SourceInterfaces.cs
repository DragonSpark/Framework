using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources.Parameterized
{
	public sealed class SourceInterfaces : FactoryCache<Type, Type>
	{
		readonly static Func<Type, bool> Specification = Defaults.KnownSourcesSpecification.IsSatisfiedBy;

		public static ICache<Type, Type> Default { get; } = new SourceInterfaces();
		SourceInterfaces() {}

		protected override Type Create( Type parameter ) => parameter.Adapt().GetAllInterfaces().FirstOrDefault( Specification );
	}
}