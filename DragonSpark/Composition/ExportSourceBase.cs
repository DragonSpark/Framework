using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition
{
	public abstract class ExportSourceBase<T> : SpecificationParameterizedSource<Type, T>
	{
		protected ExportSourceBase( IEnumerable<Type> types, IParameterizedSource<Type, T> source ) : base( new ContainsItemSpecification<Type>( types ), source.ToSourceDelegate() ) {}
	}
}