using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Parts;
using System;

namespace DragonSpark.Testing.Objects.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(OncePerScopeSpecification<object>) )]
	public sealed class InitializePartsCommand : CompositeCommand
	{
		public static InitializePartsCommand Default { get; } = new InitializePartsCommand();
		InitializePartsCommand() : base( 
			InitializePartsAssemblyCommand.Default,
			TypeSystem.Configuration.AssemblyLoader.ToCommand( AssemblyLoader.Default.ToDelegate() )
			) {}

		public sealed class Public : ApplicationPublicPartsAttribute
		{
			public Public() : base( Default.Execute ) {}
		}

		public sealed class All : ApplicationPartsAttribute
		{
			public All() : base( Default.Execute ) {}
		}
	}

	public sealed class InitializePartsAssemblyCommand : SuppliedEnumerableComand<Type>
	{
		public static InitializePartsAssemblyCommand Default { get; } = new InitializePartsAssemblyCommand();
		InitializePartsAssemblyCommand() : base( Framework.FileSystem.InitializePartsAssemblyCommand.Default, typeof(PublicClass) ) {}
	}
}
