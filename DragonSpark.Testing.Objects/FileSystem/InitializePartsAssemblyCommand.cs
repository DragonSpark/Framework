using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Parts;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Objects.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(OncePerScopeSpecification<object>) )]
	public sealed class InitializePartsCommand : CompositeCommand
	{
		readonly static Func<string, Assembly> Loader = AssemblyLoader.Default.Get;

		public static InitializePartsCommand Default { get; } = new InitializePartsCommand();
		InitializePartsCommand() : base( 
			InitializePartsAssemblyCommand.Default,
			TypeSystem.AssemblyLoader.Default.ToCommand( Loader )
			) {}

		public sealed class Public : ApplicationPublicPartsAttribute
		{
			public Public() : base( Default.Execute ) {}
		}

		public sealed class All : ApplicationAllPartsAttribute
		{
			public All() : base( Default.Execute ) {}
		}
	}

	public sealed class InitializePartsAssemblyCommand : SuppliedCommand<Type>
	{
		public static InitializePartsAssemblyCommand Default { get; } = new InitializePartsAssemblyCommand();
		InitializePartsAssemblyCommand() : base( Framework.FileSystem.InitializePartsAssemblyCommand.Default, typeof(PublicClass) ) {}
	}
}
