using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;

namespace DragonSpark.Windows.Markup
{
	public sealed class DesignTimeValueProvider : CompositeFactory<Type, object>
	{
		public static DesignTimeValueProvider Default { get; } = new DesignTimeValueProvider();
		DesignTimeValueProvider() : base( new DelegatedParameterizedSource<Type, object>( SpecialValues.DefaultOrEmpty ), MockFactory.Default, StringDesignerValueFactory.Default ) {}
	}
}