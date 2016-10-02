using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public sealed class AttributeProviders : ParameterizedScope<IAttributeProvider>
	{
		public static IParameterizedSource<IAttributeProvider> Default { get; } = new AttributeProviders();
		AttributeProviders() : base( new Factory().ToSourceDelegate().GlobalCache() ) {}

		sealed class Factory : ParameterConstructedCompositeFactory<IAttributeProvider>
		{
			public Factory() : this( MemberInfoDefinitions.Default.Get, ReflectionElementAttributeProvider.Default.Get ) {}

			readonly Func<object, MemberInfo> memberSource;
			readonly Func<object, IAttributeProvider> providerSource;
			
			Factory( Func<object, MemberInfo> memberSource, Func<object, IAttributeProvider> providerSource ) : base( typeof(ParameterInfoAttributeProvider), typeof(AssemblyAttributeProvider) )
			{
				this.memberSource = memberSource;
				this.providerSource = providerSource;
			}

			public override IAttributeProvider Get( object parameter ) => base.Get( parameter ) ?? providerSource( memberSource( parameter ) );
		}
	}
}