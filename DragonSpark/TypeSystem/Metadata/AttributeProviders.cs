using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public sealed class AttributeProviders : ParameterConstructedCompositeFactory<IAttributeProvider>
	{
		public static IParameterizedSource<object, IAttributeProvider> Default { get; } = new AttributeProviders().ToSingletonScope();
		AttributeProviders() : this( MemberInfoDefinitions.Default.Get, ReflectionElementAttributeProvider.Default.Get ) {}

		readonly Func<object, MemberInfo> memberSource;
		readonly Func<object, IAttributeProvider> providerSource;

		[UsedImplicitly]
		public AttributeProviders( Func<object, MemberInfo> memberSource, Func<object, IAttributeProvider> providerSource ) : base( typeof(ParameterInfoAttributeProvider), typeof(AssemblyAttributeProvider) )
		{
			this.memberSource = memberSource;
			this.providerSource = providerSource;
		}

		public override IAttributeProvider Get( object parameter ) => base.Get( parameter ) ?? providerSource( memberSource( parameter ) );
	}
}